using Google;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using WISEroster.Business;
using WISEroster.Domain.Models;
using WISEroster.Mvc.Extensions;
using WISEroster.Mvc.Models;

namespace WISEroster.Mvc.Classroom
{
    public static class GcSync
    {
        public const string DPI_EMAIL_DOMAIN = "@test.dpi.wi.gov";
        public const string APPLICATION_NAME = "WISERoster";
        public static async Task<TestConnectionMessage> TestAsync(Controller ct, int leaId, string userEmail, ISetupBusiness setupBusiness, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(ct, new AppFlowMetadata(leaId, setupBusiness)).
                AuthorizeAsync(userEmail, cancellationToken).ConfigureAwait(true);

            if (result.Credential != null)
            {
                var service = new ClassroomService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = APPLICATION_NAME
                });


                CoursesResource.ListRequest request = service.Courses.List();
                request.PageSize = 1;
                // List courses.
                try
                {
                    ListCoursesResponse response = request.Execute();

                    if (response != null)
                    {
                        var thisUser = service.UserProfiles.Get(userEmail).Execute();
                        return new TestConnectionMessage
                        { Connected = true, Message = "Successful Google Classroom connection", UserId = thisUser.Id };
                    }
                    else
                    {
                        return new TestConnectionMessage
                        { Connected = false, Message = "No Google Classroom response" };
                    }
                }
                catch (Exception ex)
                {
                    return new TestConnectionMessage
                    { Connected = false, Message = ex.Message };
                }

            }
            else
            {
                return new TestConnectionMessage
                { Connected = false, Message = "Credential failure for Google Classroom" };
            }
        }

        public static async Task<SyncCourseMessage> SendCourses(Controller ct, ISetupBusiness setupBusiness, SyncCourseMessage message, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(ct, new AppFlowMetadata(message.LeaId, setupBusiness)).
                AuthorizeAsync(message.UserEmail, cancellationToken).ConfigureAwait(true);

            if (result.Credential != null)
            {
                var service = new ClassroomService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = APPLICATION_NAME
                });

                var schoolName = message.School.NameOfInstitution;
                var userIsDpi = message.UserEmail.Contains(DPI_EMAIL_DOMAIN);

                Parallel.ForEach(message.Courses, (gcCourse) =>
                {
                    Course c = null;
                    c = SendCourse(message, c, service, gcCourse, schoolName);


                    if (gcCourse.Saved == true && c != null)
                    {
                        SendTeacherInvitations(message, service, gcCourse, userIsDpi);

                        SendStudentInvitations(message, gcCourse, userIsDpi, service);
                    }
                });

                if (message.Courses.Any(c => c.Saved != true))
                {
                    message.Message = message + string.Join(", ",
                                          message.Courses.Where(c => c.Saved != true).Select(m => m.GcMessage));
                }

            }
            else
            {
                message.Message = "Failed to Connect";
            }

            return message;
        }

        private static Course SendCourse(SyncCourseMessage message, Course c, ClassroomService service, GcCourse gcCourse,
            string schoolName)
        {
            try
            {
                c = service.Courses.Get(gcCourse.AliasId).Execute();
                gcCourse.CourseId = c.Id;
                gcCourse.Owner = c.OwnerId;
                gcCourse.Saved = true;
                if (c.CourseState == "ACTIVE")
                {
                    gcCourse.Activated = true;
                };

                //if previously saved and then deleted directly in Google, clear the UserId so it can save again
                if (gcCourse.GcCourseUsers.Any(u => u.UserId != null))
                {
                    foreach (var gcCourseGcCourseUser in gcCourse.GcCourseUsers)
                    {
                        gcCourseGcCourseUser.UserId = null;
                    }
                }
            }
            catch (GoogleApiException ex)
            {
                //new course
                if (ex.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    var gc = new Course
                    {
                        Id = gcCourse.AliasId,
                        Name = gcCourse.LocalCourseTitle,
                        Section =
                            $"{gcCourse.SectionIdentifier} - {gcCourse.SchoolYear} {gcCourse.SessionName} at {schoolName}",
                        OwnerId = gcCourse.Owner
                    };
                    try
                    {
                        c = service.Courses.Create(gc).Execute();
                        gcCourse.GcMessage = $"Saved {DateTime.Now}";
                        gcCourse.Saved = true;
                        gcCourse.CourseId = c.Id;
                        gcCourse.Owner = c.OwnerId;
                        gcCourse.Activated = false;
                        message.Logs.Add(new GcLog {GcId = gcCourse.AliasId.IdToName(), Message = gcCourse.GcMessage});
                    }
                    catch (GoogleApiException ex2)
                    {
                        message.Logs.Add(new GcLog {GcId = gcCourse.AliasId.IdToName(), Error = true, Message = ex2.Message});
                        gcCourse.Saved = false;
                        gcCourse.GcMessage = ex2.Message;
                    }
                }
                else
                {
                    gcCourse.Saved = false;
                    gcCourse.GcMessage = ex.Message;
                }
            }

            return c;
        }


        private static void SendTeacherInvitations(SyncCourseMessage message, ClassroomService service, GcCourse gcCourse,
            bool userIsDpi)
        {
            var teachers = service.Courses.Teachers.List(gcCourse.AliasId).Execute();

            foreach (var gcCourseUser in gcCourse.GcCourseUsers.Where(u => u.IsTeacher == true))
            {
                if (userIsDpi && !gcCourseUser.EmailAddress.Contains("@test.dpi.wi.gov"))
                {
                    gcCourse.GcMessage = $"{gcCourseUser.EmailAddress} is outside test.dpi.wi.gov, {gcCourse.GcMessage}";
                }
                else
                {
                    if (gcCourseUser.UserId == null || teachers.Teachers.All(t => t.UserId != gcCourseUser.UserId))
                    {

                        Invitation i = new Invitation
                        {
                            CourseId = gcCourse.AliasId,
                            UserId = gcCourseUser.EmailAddress,
                            Role = "TEACHER"
                        };

                        try
                        {
                            var x = service.Invitations.Create(i).Execute();
                            gcCourse.GcMessage =
                                $"{gcCourseUser.EmailAddress} invited {DateTime.Now}, {gcCourse.GcMessage}";
                            message.Logs.Add(new GcLog
                            { GcId = gcCourse.AliasId.IdToName(), Message = gcCourse.GcMessage });
                            gcCourseUser.UserId = x.UserId;
                        }
                        catch (GoogleApiException ex)
                        {
                            message.Logs.Add(new GcLog
                            {
                                GcId = gcCourse.AliasId.IdToName(),
                                Error = true,
                                Message = $"{gcCourseUser.EmailAddress} - {ex.Message}"
                            });
                            //doing a get when course does not exists throws an exception, so does adding a course with the same id
                            if (ex.HttpStatusCode == HttpStatusCode.Conflict)
                            {
                                gcCourse.GcMessage =
                                    $"Invitation for '{gcCourseUser.EmailAddress}' already exists, {gcCourse.GcMessage}";
                            }
                            else
                            {
                                gcCourse.GcMessage = $"{ex.Message}, {gcCourse.GcMessage}";
                            }
                        }
                    }
                }
            }
        }

        private static void SendStudentInvitations(SyncCourseMessage message, GcCourse gcCourse, bool userIsDpi,
           ClassroomService service)
        {

            foreach (var gcCourseUser in gcCourse.GcCourseUsers.Where(u => u.IsTeacher == false))
            {
                if (userIsDpi && !gcCourseUser.EmailAddress.Contains("@test.dpi.wi.gov"))
                {
                    gcCourse.GcMessage = $"{gcCourseUser.EmailAddress} is outside test.dpi.wi.gov, {gcCourse.GcMessage}";
                }
                else
                {
                    Invitation i = new Invitation
                    {
                        CourseId = gcCourse.AliasId,
                        UserId = gcCourseUser.EmailAddress,
                        Role = "STUDENT"
                    };
                    try
                    {
                        var x = service.Invitations.Create(i).Execute();
                        gcCourse.GcMessage = $"{gcCourseUser.EmailAddress} invited {DateTime.Now}, {gcCourse.GcMessage}";
                        message.Logs.Add(new GcLog { GcId = gcCourse.AliasId.IdToName(), Message = gcCourse.GcMessage });
                        gcCourseUser.UserId = x.UserId;
                    }
                    catch (GoogleApiException ex)
                    {
                        message.Logs.Add(new GcLog
                        {
                            GcId = gcCourse.AliasId.IdToName(),
                            Error = true,
                            Message = $"{gcCourseUser.EmailAddress} - {ex.Message}"
                        });
                        if (ex.HttpStatusCode == HttpStatusCode.Conflict)
                        {
                            gcCourse.GcMessage =
                                $"Invitation for '{gcCourseUser.EmailAddress}' already exists, {gcCourse.GcMessage}";
                        }
                        else
                        {
                            gcCourse.GcMessage = $"{ex.Message}, {gcCourse.GcMessage}";
                        }
                    }
                }
            }
        }


        public static async Task<List<Course>> GetCourses(Controller ct, int leaId, string userEmail, ISetupBusiness setupBusiness, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(ct, new AppFlowMetadata(leaId, setupBusiness)).
                AuthorizeAsync(userEmail, cancellationToken).ConfigureAwait(true);
            var courses = new List<Course>();

            if (result.Credential != null)
            {
                var service = new ClassroomService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = APPLICATION_NAME
                });

                string pageToken = null;

                do
                {
                    var request = service.Courses.List();
                    request.PageSize = 100;
                    request.PageToken = pageToken;
                    var response = request.Execute();
                    if (response.Courses != null)
                    {
                        courses.AddRange(response.Courses);
                    }
                    pageToken = response.NextPageToken;
                } while (pageToken != null);
            }
            return courses;
        }

        public static async Task<SyncCourseMessage> DeleteCourses(Controller ct, ISetupBusiness setupBusiness, SyncCourseMessage message, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(ct, new AppFlowMetadata(message.LeaId, setupBusiness)).
                AuthorizeAsync(message.UserEmail, cancellationToken).ConfigureAwait(true);

            if (result.Credential != null)
            {
                var service = new ClassroomService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = APPLICATION_NAME
                });

                Parallel.ForEach(message.Courses, (course) =>
                {
                    try
                    {
                        var c = service.Courses.Get(course.CourseId).Execute();
                        if (c.CourseState == "DECLINED")
                        {
                            c.CourseState = "PROVISIONED";
                            service.Courses.Update(c, course.CourseId).Execute();
                        }

                        c.CourseState = "ARCHIVED";
                        var x = service.Courses.Update(c, course.CourseId).Execute();
                        var y = service.Courses.Delete(course.CourseId).Execute();
                        message.Logs.Add(new GcLog { GcId = course.CourseId, Message = "Deleted" });
                    }
                    catch (GoogleApiException ex)
                    {
                        course.GcMessage = ex.Message;
                        message.Logs.Add(new GcLog { GcId = course.CourseId, Error = true, Message = ex.Message });
                    }
                });

                message.Message = "Done";
            }
            else
            {
                message.Message = "Failed to Connect";
            }

            return message;


        }

        public static async Task<SyncCourseMessage> SendCourseActivations(Controller ct, ISetupBusiness setupBusiness, SyncCourseMessage message, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(ct, new AppFlowMetadata(message.LeaId, setupBusiness)).
                AuthorizeAsync(message.UserEmail, cancellationToken).ConfigureAwait(true);

            if (result.Credential != null)
            {
                var service = new ClassroomService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = APPLICATION_NAME
                });


                foreach (var gcCourse in message.Courses)
                {
                    try
                    {

                        var c = service.Courses.Get(gcCourse.AliasId).Execute();
                        if (c.CourseState != "ACTIVE")
                        {
                            c.CourseState = "ACTIVE";
                            service.Courses.Update(c, gcCourse.AliasId).Execute();
                        };
                        gcCourse.Activated = true;
                        gcCourse.GcMessage = $"Activated {DateTime.Now}, {gcCourse.GcMessage}";
                        message.Logs.Add(new GcLog { GcId = gcCourse.AliasId.IdToName(), Message = gcCourse.GcMessage });
                    }
                    catch (GoogleApiException ex)
                    {
                        message.Logs.Add(new GcLog { GcId = gcCourse.AliasId.IdToName(), Error = true, Message = ex.Message });
                        gcCourse.GcMessage = ex.Message + ", " + gcCourse.GcMessage;
                        break;
                    }

                }

            }
            else
            {
                message.Message = "Failed to Connect";
            }

            return message;
        }

        public static async Task<SyncCourseMessage> InviteToOwn(Controller ct, ISetupBusiness setupBusiness, SyncCourseMessage message, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(ct, new AppFlowMetadata(message.LeaId, setupBusiness)).
                AuthorizeAsync(message.UserEmail, cancellationToken).ConfigureAwait(true);

            if (result.Credential != null)
            {
                var service = new ClassroomService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = APPLICATION_NAME
                });

                var thisUser = service.UserProfiles.Get(message.UserEmail).Execute();

                Parallel.ForEach(message.Courses, (gcCourse) =>
                {

                    try
                    {

                        var c = service.Courses.Get(gcCourse.AliasId).Execute();
                        gcCourse.Activated = c.CourseState == "ACTIVE";

                        var teachers = service.Courses.Teachers.List(gcCourse.AliasId).Execute();

                        if (c.OwnerId == thisUser.Id)
                        {
                            var availableTeachers = teachers.Teachers.Where(t => t.UserId != thisUser.Id)
                                .Select(t => t.UserId).ToList();
                            if (availableTeachers.Count == 1)
                            {
                                Invitation i = new Invitation
                                {
                                    CourseId = gcCourse.AliasId,
                                    UserId = availableTeachers.First(),
                                    Role = "OWNER"
                                };

                                try
                                {
                                    var x = service.Invitations.Create(i).Execute();
                                    gcCourse.GcMessage = $"Transfer ownership to {availableTeachers.First()} invited {DateTime.Now}, {gcCourse.GcMessage}";
                                    message.Logs.Add(new GcLog { GcId = gcCourse.AliasId.IdToName(), Message = gcCourse.GcMessage });
                                    
                                }
                                catch (GoogleApiException ex)
                                {
                                    message.Logs.Add(new GcLog { GcId = gcCourse.AliasId.IdToName(), Error = true, Message = $"{availableTeachers.First()} - {ex.Message}" });
                                    //doing a get when course does not exists throws an exception, so does adding a course with the same id
                                    if (ex.HttpStatusCode == HttpStatusCode.BadRequest)
                                    {
                                        gcCourse.GcMessage = $"Invitation for '{availableTeachers.First()}' already exists, {gcCourse.GcMessage}";
                                        message.Logs.Add(new GcLog { GcId = gcCourse.AliasId.IdToName(), Message = ex.Message });
                                    }
                                    else
                                    {
                                        gcCourse.GcMessage = $"{ex.Message}, {gcCourse.GcMessage}";
                                    }
                                }

                            }
                            else
                            {
                                gcCourse.GcMessage = "Admin is owner, no teacher available to transfer";

                            }

                        }
                        else
                        {
                            gcCourse.GcMessage = "Admin is not owner";
                        }



                    }
                    catch (GoogleApiException ex)
                    {
                        message.Logs.Add(new GcLog { GcId = gcCourse.AliasId.IdToName(), Error = true, Message = ex.Message });
                        gcCourse.GcMessage = ex.Message + ", " + gcCourse.GcMessage;
                    }

                });


            }
            else
            {
                message.Message = "Failed to Connect";
            }

            return message;
        }

    }

}