using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Google;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Http;
using Google.Apis.Services;
using Microsoft.Ajax.Utilities;
using WISEroster.Business;
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
                        return new TestConnectionMessage
                        { Connected = true, Message = "Successful Google Classroom connection" };
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
                    Course c = new Course
                    {
                        Id = gcCourse.GcId,
                        Name = gcCourse.LocalCourseTitle,
                        Section = $"{gcCourse.SectionIdentifier} - {gcCourse.SchoolYear} {gcCourse.SessionName} at {schoolName}",
                        OwnerId = gcCourse.Owner

                    };
                    try
                    {
                        var x = service.Courses.Create(c).Execute();
                        gcCourse.GcMessage = $"Saved {DateTime.Now}";
                        gcCourse.Saved = true;
                        gcCourse.Activated = false;
                    }
                    catch (GoogleApiException ex)
                    {
                        //doing a get when course does not exists throws an exception, so does adding a course with the same id
                        if (ex.HttpStatusCode == HttpStatusCode.Conflict)
                        {
                            gcCourse.GcMessage = $"Course '{c.Id}' already exists.";
                            gcCourse.Saved = true;
                        }
                        else
                        {
                            gcCourse.Saved = false;
                            gcCourse.GcMessage = ex.Message;

                        }
                    }

                    if (gcCourse.Saved == true)
                    {
                        foreach (var gcCourseUser in gcCourse.GcCourseUsers.Where(u => u.IsTeacher == true))
                        {
                            if (userIsDpi && !gcCourseUser.EmailAddress.Contains(DPI_EMAIL_DOMAIN))
                            {
                                gcCourse.GcMessage = $"{gcCourseUser.EmailAddress} is outside {DPI_EMAIL_DOMAIN}, {gcCourse.GcMessage}";
                            }
                            else
                            {
                                Invitation i = new Invitation
                                {
                                    CourseId = gcCourse.GcId,
                                    UserId = gcCourseUser.EmailAddress,
                                    Role = "TEACHER"
                                };

                                try
                                {
                                    var x = service.Invitations.Create(i).Execute();
                                    gcCourse.GcMessage = $"{gcCourseUser.EmailAddress} invited {DateTime.Now}, {gcCourse.GcMessage}";
                                }
                                catch (GoogleApiException ex)
                                {
                                    //doing a get when course does not exists throws an exception, so does adding a course with the same id
                                    if (ex.HttpStatusCode == HttpStatusCode.Conflict)
                                    {
                                        gcCourse.GcMessage = $"Invitation for '{gcCourseUser.EmailAddress}' already exists, {gcCourse.GcMessage}";
                                    }
                                    else
                                    {
                                        gcCourse.Saved = false;
                                        gcCourse.GcMessage = $"{ex.Message}, {gcCourse.GcMessage}";
                                    }
                                }

                            }


                        }

                        foreach (var gcCourseUser in gcCourse.GcCourseUsers.Where(u => u.IsTeacher == false))
                        {
                            if (userIsDpi && !gcCourseUser.EmailAddress.Contains(DPI_EMAIL_DOMAIN))
                            {
                                gcCourse.GcMessage = $"{gcCourseUser.EmailAddress} is outside {DPI_EMAIL_DOMAIN}, {gcCourse.GcMessage}";
                            }
                            else
                            {
                                Invitation i = new Invitation
                                {
                                    CourseId = gcCourse.GcId,
                                    UserId = gcCourseUser.EmailAddress,
                                    Role = "STUDENT"
                                };

                                try
                                {
                                    var x = service.Invitations.Create(i).Execute();
                                    gcCourse.GcMessage = $"{gcCourseUser.EmailAddress} invited {DateTime.Now}, {gcCourse.GcMessage}";
                                }
                                catch (GoogleApiException ex)
                                {
                                    //doing a get when course does not exists throws an exception, so does adding a course with the same id
                                    if (ex.HttpStatusCode == HttpStatusCode.Conflict)
                                    {
                                        gcCourse.GcMessage = $"Invitation for '{gcCourseUser.EmailAddress}' already exists, {gcCourse.GcMessage}";
                                    }
                                    else
                                    {
                                        gcCourse.Saved = false;
                                        gcCourse.GcMessage = $"{ex.Message}, {gcCourse.GcMessage}";
                                    }
                                }

                            }


                        }
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

        public static async Task<string> DeleteCourse(Controller ct, ISetupBusiness setupBusiness, string id, int leaId, string userEmail, CancellationToken cancellationToken)
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

                try
                {
                    Delete(service, id);
                }
                catch (GoogleApiException ex)
                {

                    return ex.Message;
                }
            }
            return "No Access";
        }


        public static async Task<string> DeleteCourses(Controller ct, ISetupBusiness setupBusiness, List<string> ids, int leaId, string userEmail, CancellationToken cancellationToken)
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

                Parallel.ForEach(ids, (id) =>
                {
                    Delete(service, id);
                });

                return "Done";
            }
            return "No Access";
        }



        public static async Task<string> ActivateCourse(Controller ct, ISetupBusiness setupBusiness, string id, int leaId, string userEmail, CancellationToken cancellationToken)
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
                try
                {
                    var c = service.Courses.Get(id).Execute();
                    if (c.CourseState != "ACTIVE")
                    {
                        c.CourseState = "ACTIVE";
                        service.Courses.Update(c, id).Execute();
                    }


                    return "Active";
                }
                catch (GoogleApiException ex)
                {

                    return ex.Message;
                }
            }
            return "No Access";
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

                        var c = service.Courses.Get(gcCourse.GcId).Execute();
                        if (c.CourseState != "ACTIVE")
                        {
                            c.CourseState = "ACTIVE";
                            service.Courses.Update(c, gcCourse.GcId).Execute();
                        };
                        gcCourse.Activated = true;
                        gcCourse.GcMessage = $"Activated {DateTime.Now}, {gcCourse.GcMessage}";
                    }
                    catch (GoogleApiException ex)
                    {
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


        private static string Delete(ClassroomService service, string id)
        {
            try
            {
                var c = service.Courses.Get(id).Execute();
                if (c.CourseState == "DECLINED")
                {
                    c.CourseState = "PROVISIONED";
                    service.Courses.Update(c, id).Execute();
                }

                c.CourseState = "ARCHIVED";
                var x = service.Courses.Update(c, id).Execute();
                var y = service.Courses.Delete(id).Execute();
                return "Deleted";
            }
            catch (GoogleApiException ex)
            {
                return ex.Message;
            }
        }

    }

}