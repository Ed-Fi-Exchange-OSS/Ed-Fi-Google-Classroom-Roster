using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using WISEroster.Business.Models;
using WISEroster.Domain.Api;
using WISEroster.Domain.Models;


namespace WISEroster.Business
{



    public interface IProvisioningRulesBusiness
    {
        void InsertRule(ProvisioningRulesInsertModel model);
        void DeleteRule(int id);
        List<RulesListModel> GetRuleList(int leaId, short schoolYear);
        List<ClassToRosterModel> GetClassesForRule(int ruleId);

        List<GcCourse> GetSyncList(int leaId, short schoolYear, int schoolId);
        List<GcCourse> GetActivateList(int leaId, short schoolYear, int schoolId);
        GcCourse GetClassToSync(int leaId, short schoolYear, int schoolId, string gcName);
        int GenerateSyncList(int leaId, short schoolYear, int schoolId);
        int SaveSyncProgress(int leaId, short schoolYear, int schoolId, List<GcCourse> courses);
    }
    public class ProvisioningRulesBusiness : IProvisioningRulesBusiness
    {
        private readonly IV3ApiDbContext _apiContext;
        private readonly IWISErosterDbContext _context;

        public ProvisioningRulesBusiness(IWISErosterDbContext context, IV3ApiDbContext apiContext)
        {
            _context = context;
            _apiContext = apiContext;
        }

        public void InsertRule(ProvisioningRulesInsertModel model)
        {
            if (model != null)
            {
                var rule = new ProvisioningRule
                {
                    EducationOrganizationId = model.EducationOrganizationId,
                    SchoolYear = model.SchoolYear,
                    SchoolCategoryTypeId = model.SchoolCategoryDescriptorId,
                    SessionName = model.SelectedSession,
                    StaffOnly = model.StaffOnly,
                    IncludeExclude = model.IncludeExclude,
                    CreateDate = DateTime.Now,
                    TypeId = model.TypeId,
                    GroupByTitle = model.GroupByTitle

                };

                if (model.SelectedSchools != null && model.SelectedSchools.Any() && model.SelectedSchools.Any(s => s > 0))
                {
                    rule.RosterSchools = model.SelectedSchools.Select(i => new RosterSchool { SchoolId = i }).ToList();
                }

                var schoolYear = rule.SchoolYear;
                _apiContext.ChangeSchoolYear(schoolYear);

                if (model.TypeId == 1)
                {
                    //don't save default text as a title
                    rule.RosterLocalCourses = model.SelectedCourses.Where(t => _apiContext.CourseOfferings
                            .Where(c => c.School.LocalEducationAgencyId ==
                                        rule.EducationOrganizationId).Any(c => c.LocalCourseCode == t))
                        .Select(j => new RosterLocalCourse { LocalCourseCode = j }).Distinct().ToList();

                }
                else if ( model.TypeId == 2)
                {
                    rule.RosterGradeLevels = model.SelectedGrades.Where(g => g > 0)
                        .Select(g => new RosterGradeLevel { GradeLevelDescriptorId = g }).ToList();
                }
                else
                {
                    rule.StaffUSI = model.SelectedTeacher;
                    rule.RosterLocalCourses = model.SelectedCourses.Where(t => _apiContext.CourseOfferings
                            .Where(c => c.School.LocalEducationAgencyId ==
                                        rule.EducationOrganizationId).Any(c => c.LocalCourseCode == t))
                        .Select(j => new RosterLocalCourse { LocalCourseCode = j }).Distinct().ToList();
                }

                using (var dbContext = new WISErosterDbContext())
                {
                    dbContext.ProvisioningRules.Add(rule);
                    dbContext.SaveChanges();
                }
            }
        }
        public void DeleteRule(int id)
        {
            using (var dbContext = new WISErosterDbContext())
            {
                var rule = dbContext.ProvisioningRules.First(r => r.RuleId == id);
                dbContext.ProvisioningRules.Remove(rule);
                dbContext.SaveChanges();
            }
        }

        public List<RulesListModel> GetRuleList(int leaId, short schoolYear)
        {
            var rule = _context.ProvisioningRules;

            var ruleList = rule.Where(r => r.EducationOrganizationId == leaId && r.SchoolYear == schoolYear).OrderBy(r => r.RuleId).Select(i => new RulesListModel
            {
                RuleId = i.RuleId,
                SchoolYear = i.SchoolYear,
                SessionName = i.SessionName,
                StaffOnly = i.StaffOnly,
                EducationOrganizationId = i.EducationOrganizationId,
                SchoolCategoryDescriptorId = i.SchoolCategoryTypeId,
                IncludeExclude = (bool)i.IncludeExclude,
                GroupByTitle = i.GroupByTitle,
                Courses = i.RosterLocalCourses.Select(t => t.LocalCourseCode).ToList(),
                GradeLevels = i.RosterGradeLevels.Select(t => t.GradeLevelDescriptorId).ToList(),
                Schools = i.RosterSchools.Select(s => s.SchoolId).ToList(),
                TypeId = i.TypeId

            }).ToList();


            _apiContext.ChangeSchoolYear(schoolYear);

            var schoolIds = ruleList.SelectMany(r => r.Schools).ToList();

            if (schoolIds.Any())

            {
                var schools = _apiContext.EducationOrganizations.Where(s => schoolIds.Contains(s.EducationOrganizationId)).Select(x => new { EducationOrganizationId = x.EducationOrganizationId, NameOfInstitution = x.NameOfInstitution }).ToList();

                foreach (var rulesListModel in ruleList)
                {
                    rulesListModel.SchoolNames = schools
                        .Where(s => rulesListModel.Schools.Any(e => s.EducationOrganizationId == e))
                        .Select(s => s.NameOfInstitution).ToList();
                }
            }

            var gradeIds = ruleList.SelectMany(r => r.GradeLevels).ToList();

            if (gradeIds.Any())

            {
                var grades = _apiContext.Descriptors.Where(s => gradeIds.Contains(s.DescriptorId)).Select(x => new { DescriptorId = x.DescriptorId, CodeValue = x.CodeValue }).ToList();

                foreach (var rulesListModel in ruleList.Where(r => r.TypeId == 2))
                {
                    rulesListModel.GradeNames = grades
                        .Where(s => rulesListModel.GradeLevels.Any(e => s.DescriptorId == e))
                        .Select(s => s.CodeValue).ToList();
                    if (!rulesListModel.GradeNames.Any()) rulesListModel.GradeNames.Add("All");
                }
            }

            var courseCodes = ruleList.SelectMany(r => r.Courses).ToList();

            if (courseCodes.Any())

            {
                var courses = _apiContext.CourseOfferings.Where(s => courseCodes.Contains(s.LocalCourseCode)).Select(x => new { x.LocalCourseCode, x.LocalCourseTitle }).Distinct().ToList();

                foreach (var rulesListModel in ruleList.Where(r => r.TypeId == 1))
                {
                    rulesListModel.Courses = courses
                        .Where(s => rulesListModel.Courses.Any(e => s.LocalCourseCode == e))
                        .Select(s => s.LocalCourseCode + " - " + s.LocalCourseTitle).ToList();
                    if (!rulesListModel.Courses.Any()) rulesListModel.Courses.Add("All");
                }

                foreach (var rulesListModel in ruleList.Where(r => r.TypeId == 3))
                {
                    rulesListModel.Courses = courses
                        .Where(s => rulesListModel.Courses.Any(e => s.LocalCourseCode == e))
                        .Select(s => s.LocalCourseCode + " - " + s.LocalCourseTitle).ToList();
                    if (!rulesListModel.Courses.Any()) rulesListModel.Courses.Add("All");
                }
            }

            return ruleList;
        }

        public List<ClassToRosterModel> GetClassesForRule(int ruleId)
        {
            var rule = _context.ProvisioningRules.Include(s => s.RosterSchools).Include(s => s.RosterLocalCourses).Include(s => s.RosterGradeLevels).First(r => r.RuleId == ruleId);
            var schoolYear = rule.SchoolYear;
            var schoolIds = rule.RosterSchools.Select(s => s.SchoolId).ToList();
            _apiContext.ChangeSchoolYear(schoolYear);
            List<ClassToRosterModel> classes;

            if (rule.TypeId == 1)
            {

                var codes = rule.RosterLocalCourses.Select(s => s.LocalCourseCode).ToList();
                var classQuery = _apiContext.Sections.Where(s =>
                    s.CourseOffering.School.LocalEducationAgencyId == rule.EducationOrganizationId &&
                    s.StaffSectionAssociations.Any()
                                                               && s.CourseOffering.School.LocalEducationAgencyId ==
                                                               rule.EducationOrganizationId
                                                               && s.CourseOffering.SessionName ==
                                                               rule.SessionName

              );
                if (schoolIds.Any())
                {
                    classQuery = classQuery.Where(s => schoolIds.Any(r => r == s.SchoolId));
                }
                else
                {
                    if (rule.SchoolCategoryTypeId > 0)
                    {
                        classQuery = classQuery.Where(s =>
                            s.CourseOffering.School.SchoolCategories.Any(c =>
                                c.SchoolCategoryDescriptorId == rule.SchoolCategoryTypeId));
                    }
                }

                if (rule.IncludeExclude.GetValueOrDefault())
                {
                    if (codes.Any(s => !string.IsNullOrWhiteSpace(s)))
                    {
                        classQuery = classQuery.Where(s => codes.Any(r => r == s.LocalCourseCode));
                    }

                }
                else
                {

                    classQuery = classQuery.Where(s => codes.All(r => r != s.LocalCourseCode));
                }

                if (rule.GroupByTitle == true)
                {
                    classes = classQuery.GroupBy(c => new { c.CourseOffering.LocalCourseTitle, c.LocalCourseCode, c.SchoolId, c.CourseOffering.School.EducationOrganization.NameOfInstitution, c.SessionName }).Select(c => new ClassToRosterModel
                    {
                        SchoolYear = rule.SchoolYear,
                        SchoolId = c.Key.SchoolId,
                        LocalCourseCode = c.Key.LocalCourseCode,
                        LocalCourseTitle = c.Key.LocalCourseTitle,
                        SectionIdentifier = c.Key.LocalCourseTitle,
                        SessionName = c.Key.SessionName,
                        StaffName = c.SelectMany(g => g.StaffSectionAssociations.Select(s => s.Staff.FirstName + " " + s.Staff.LastSurname)).Distinct().ToList(),
                        SchoolName = c.Key.NameOfInstitution,
                        StudentCount = c.Sum(g => g.StudentSectionAssociations.Count())

                    }).ToList();
                }
                else
                {
                    classes = classQuery.Select(c => new ClassToRosterModel
                    {
                        SchoolYear = c.SchoolYear,
                        SchoolId = c.SchoolId,
                        SectionIdentifier = c.SectionIdentifier,
                        LocalCourseCode = c.LocalCourseCode,
                        LocalCourseTitle = c.CourseOffering.LocalCourseTitle,
                        SessionName = c.SessionName,
                        StaffName = c.StaffSectionAssociations.Select(s => s.Staff.FirstName + " " + s.Staff.LastSurname).Distinct().ToList(),
                        SchoolName = c.CourseOffering.School.EducationOrganization.NameOfInstitution,
                        StudentCount = c.StudentSectionAssociations.Count()

                    }).ToList();
                }
            }
            else if (rule.TypeId == 2)
            {
                var schoolQuery = _apiContext.SchoolGradeLevels.Where(g => g.School.LocalEducationAgencyId == rule.EducationOrganizationId);
                if (schoolIds.Any())
                {
                    schoolQuery = schoolQuery.Where(s => schoolIds.Any(r => r == s.SchoolId));
                }
                else
                {
                    if (rule.SchoolCategoryTypeId > 0)
                    {
                        schoolQuery = schoolQuery.Where(s =>
                            s.School.SchoolCategories.Any(c =>
                                c.SchoolCategoryDescriptorId == rule.SchoolCategoryTypeId));
                    }
                }
                var gradeIds = rule.RosterGradeLevels.Select(s => s.GradeLevelDescriptorId).ToList();
                if (gradeIds.Any())
                {
                    schoolQuery = schoolQuery.Where(s => gradeIds.Any(g => s.GradeLevelDescriptorId == g));
                }

                classes = schoolQuery.Select(c => new ClassToRosterModel
                {
                    SchoolYear = rule.SchoolYear,
                    SchoolId = c.SchoolId,
                    LocalCourseCode = c.GradeLevelDescriptor.Descriptor.CodeValue,
                    SchoolName = c.School.EducationOrganization.NameOfInstitution,
                    SectionIdentifier = "Grade",
                    SessionName = rule.SchoolYear.ToString(),
                    StudentCount = c.School.StudentSchoolAssociations.Count(s => s.EntryGradeLevelDescriptorId == c.GradeLevelDescriptorId)

                }).ToList();

            }
            else 
            {

                var codes = rule.RosterLocalCourses.Select(s => s.LocalCourseCode).ToList();
                var classQuery = _apiContext.Sections.Where(s =>
                    s.CourseOffering.School.LocalEducationAgencyId == rule.EducationOrganizationId &&
                    s.StaffSectionAssociations.Any(ssa => ssa.StaffUSI == rule.StaffUSI)
                                                               && s.CourseOffering.School.LocalEducationAgencyId ==
                                                               rule.EducationOrganizationId
                                                               && s.CourseOffering.SessionName ==
                                                               rule.SessionName

              );
                if (schoolIds.Any())
                {
                    classQuery = classQuery.Where(s => schoolIds.Any(r => r == s.SchoolId));
                }
                else
                {
                    if (rule.SchoolCategoryTypeId > 0)
                    {
                        classQuery = classQuery.Where(s =>
                            s.CourseOffering.School.SchoolCategories.Any(c =>
                                c.SchoolCategoryDescriptorId == rule.SchoolCategoryTypeId));
                    }
                }

                if (rule.IncludeExclude.GetValueOrDefault())
                {
                    if (codes.Any(s => !string.IsNullOrWhiteSpace(s)))
                    {
                        classQuery = classQuery.Where(s => codes.Any(r => r == s.LocalCourseCode));
                    }

                }
                else
                {

                    classQuery = classQuery.Where(s => codes.All(r => r != s.LocalCourseCode));
                }

                if (rule.GroupByTitle == true)
                {
                    classes = classQuery.GroupBy(c => new { c.CourseOffering.LocalCourseTitle, c.LocalCourseCode, c.SchoolId, c.CourseOffering.School.EducationOrganization.NameOfInstitution, c.SessionName }).Select(c => new ClassToRosterModel
                    {
                        SchoolYear = rule.SchoolYear,
                        SchoolId = c.Key.SchoolId,
                        LocalCourseCode = c.Key.LocalCourseCode,
                        LocalCourseTitle = c.Key.LocalCourseTitle,
                        SectionIdentifier = c.Key.LocalCourseTitle,
                        SessionName = c.Key.SessionName,
                        StaffName = c.SelectMany(g => g.StaffSectionAssociations.Select(s => s.Staff.FirstName + " " + s.Staff.LastSurname)).Distinct().ToList(),
                        SchoolName = c.Key.NameOfInstitution,
                        StudentCount = c.Sum(g => g.StudentSectionAssociations.Count()),
                        StaffUSI = (int)rule.StaffUSI

                    }).ToList();
                }
                else
                {
                    classes = classQuery.Select(c => new ClassToRosterModel
                    {
                        SchoolYear = c.SchoolYear,
                        SchoolId = c.SchoolId,
                        SectionIdentifier = c.SectionIdentifier,
                        LocalCourseCode = c.LocalCourseCode,
                        LocalCourseTitle = c.CourseOffering.LocalCourseTitle,
                        SessionName = c.SessionName,
                        StaffName = c.StaffSectionAssociations.Select(s => s.Staff.FirstName + " " + s.Staff.LastSurname).Distinct().ToList(),
                        SchoolName = c.CourseOffering.School.EducationOrganization.NameOfInstitution,
                        StudentCount = c.StudentSectionAssociations.Count(),
                        StaffUSI = c.StaffSectionAssociations.Select(s => s.StaffUSI).FirstOrDefault()

                    }).ToList();
                }
            }

            return classes;
        }

        public int GenerateSyncList(int leaId, short schoolYear, int schoolId)
        {
            var schoolCourses = new List<GcCourse>();
            var schoolCourseUsers = new List<GcCourseUser>();
            _apiContext.ChangeSchoolYear(schoolYear);

            var category = _apiContext.Schools.Where(s => s.SchoolId == schoolId)
                .SelectMany(s => s.SchoolCategories.Select(c => c.SchoolCategoryDescriptorId)).FirstOrDefault();

            //class title rules
            var classRules = _context.ProvisioningRules.Include(s => s.RosterLocalCourses)
                .Where(r => r.EducationOrganizationId == leaId && r.SchoolYear == schoolYear && (r.TypeId == 1 || r.TypeId == 3)
                      && (r.RosterSchools.Any(s => s.SchoolId == schoolId)
                       || (!r.RosterSchools.Any() && (r.SchoolCategoryTypeId == category || r.SchoolCategoryTypeId == 0)))).ToList();

            var sessions = classRules.Select(t => t.SessionName).ToList();

            var pref = _context.OrgGcPreferences.FirstOrDefault(p => p.EducationOrganizationId == leaId);
            var domain = pref.GcUserEmail.Substring(pref.GcUserEmail.IndexOf("@", StringComparison.CurrentCultureIgnoreCase));
            var classPool = _apiContext.Sections.Where(s => s.StaffSectionAssociations.Any()
                                                            && s.SchoolId == schoolId
                                                            && sessions.Contains(s.CourseOffering.SessionName)

            ).Select(c => new
            {
                SchoolId = c.SchoolId,
                SchoolYear = c.SchoolYear,
                LocalCourseCode = c.LocalCourseCode,
                SectionIdentifier = c.SectionIdentifier,
                SessionName = c.SessionName,
                EducationOrganizationId = leaId,
                LocalCourseTitle = c.CourseOffering.LocalCourseTitle,
                StaffEmails = c.StaffSectionAssociations.SelectMany(e => e.Staff.StaffElectronicMails.Where(a => pref.AllowExternalDomains || a.ElectronicMailAddress.EndsWith(domain)).Select(a => a.ElectronicMailAddress)).ToList(),
                StudentEmails = c.StudentSectionAssociations.SelectMany(ssa => ssa.Student.StudentEducationOrganizationAssociations.SelectMany( e => e.StudentEducationOrganizationAssociationElectronicMails.Where(a => pref.AllowExternalDomains || a.ElectronicMailAddress.EndsWith(domain)).Select(a => a.ElectronicMailAddress))).ToList()
            }).ToList();


            foreach (var rule in classRules)
            {
                if (rule.GroupByTitle == true)
                {
                    if (rule.IncludeExclude.GetValueOrDefault())
                    {
                        var i = rule.RosterLocalCourses.Count != 0 ? classPool.Where(c => c.SessionName == rule.SessionName &&
                            rule.RosterLocalCourses.Any(t => t.LocalCourseCode.Trim() == c.LocalCourseCode.Trim()))
                            .GroupBy(c => new { c.LocalCourseTitle, c.LocalCourseCode, c.SessionName }).Select(g => new { g.Key, Emails = g.SelectMany(e => e.StaffEmails), StudentEmails = g.SelectMany(e => e.StudentEmails) })
                            : classPool.Where( c => c.SessionName == rule.SessionName)
                            .GroupBy(c => new { c.LocalCourseTitle, c.LocalCourseCode, c.SessionName }).Select(g => new { g.Key, Emails = g.SelectMany(e => e.StaffEmails), StudentEmails = g.SelectMany(e => e.StudentEmails) });

                        foreach (var course in i)
                        {
                            schoolCourses.Add(new GcCourse
                            {
                                SchoolId = schoolId,
                                SchoolYear = schoolYear,
                                LocalCourseCode = course.Key.LocalCourseCode,
                                SectionIdentifier = course.Key.LocalCourseTitle,
                                SessionName = course.Key.SessionName,
                                EducationOrganizationId = leaId,
                                LocalCourseTitle = course.Key.LocalCourseTitle
                            });

                            if (rule.StaffOnly == true)
                            {
                                schoolCourseUsers.AddRange(course.Emails.Distinct().Select(e => new GcCourseUser
                                {
                                    SchoolId = schoolId,
                                    SchoolYear = schoolYear,
                                    LocalCourseCode = course.Key.LocalCourseCode,
                                    SectionIdentifier = course.Key.LocalCourseTitle,
                                    SessionName = course.Key.SessionName,
                                    EducationOrganizationId = leaId,
                                    EmailAddress = e,
                                    IsTeacher = true
                                }));
                            }
                            else 
                            {
                                schoolCourseUsers.AddRange(course.Emails.Distinct().Select(e => new GcCourseUser
                                {
                                    SchoolId = schoolId,
                                    SchoolYear = schoolYear,
                                    LocalCourseCode = course.Key.LocalCourseCode,
                                    SectionIdentifier = course.Key.LocalCourseTitle,
                                    SessionName = course.Key.SessionName,
                                    EducationOrganizationId = leaId,
                                    EmailAddress = e,
                                    IsTeacher = true
                                }));

                                schoolCourseUsers.AddRange(course.StudentEmails.Distinct().Select(e => new GcCourseUser
                                {
                                    SchoolId = schoolId,
                                    SchoolYear = schoolYear,
                                    LocalCourseCode = course.Key.LocalCourseCode,
                                    SectionIdentifier = course.Key.LocalCourseTitle,
                                    SessionName = course.Key.SessionName,
                                    EducationOrganizationId = leaId,
                                    EmailAddress = e,
                                    IsTeacher = false
                                }));
                            }

                        }

                    }
                    else
                    {
                        var j = classPool.Where(c => c.SessionName == rule.SessionName &&
                            rule.RosterLocalCourses.Any(t => t.LocalCourseCode.Trim() != c.LocalCourseCode.Trim()))
                            .GroupBy(c => new { c.LocalCourseTitle, c.LocalCourseCode, c.SessionName })
                            .Select(g => new { g.Key, Emails = g.SelectMany(e => e.StaffEmails) , StudentEmails = g.SelectMany(e => e.StudentEmails) });




                        foreach (var c in j)
                        {
                            schoolCourses.Add(new GcCourse
                            {
                                SchoolId = schoolId,
                                SchoolYear = schoolYear,
                                LocalCourseCode = c.Key.LocalCourseCode,
                                SectionIdentifier = c.Key.LocalCourseTitle,
                                SessionName = c.Key.SessionName,
                                EducationOrganizationId = leaId,
                                LocalCourseTitle = c.Key.LocalCourseTitle
                            });

                            if (rule.StaffOnly == true)
                            {
                                schoolCourseUsers.AddRange(c.Emails.Distinct().Select(e => new GcCourseUser
                                {
                                    SchoolId = schoolId,
                                    SchoolYear = schoolYear,
                                    LocalCourseCode = c.Key.LocalCourseCode,
                                    SectionIdentifier = c.Key.LocalCourseTitle,
                                    SessionName = c.Key.SessionName,
                                    EducationOrganizationId = leaId,
                                    EmailAddress = e,
                                    IsTeacher = true
                                }));
                            }
                            else
                            {
                                schoolCourseUsers.AddRange(c.Emails.Distinct().Select(e => new GcCourseUser
                                {
                                    SchoolId = schoolId,
                                    SchoolYear = schoolYear,
                                    LocalCourseCode = c.Key.LocalCourseCode,
                                    SectionIdentifier = c.Key.LocalCourseTitle,
                                    SessionName = c.Key.SessionName,
                                    EducationOrganizationId = leaId,
                                    EmailAddress = e,
                                    IsTeacher = true
                                }));

                                schoolCourseUsers.AddRange(c.StudentEmails.Distinct().Select(e => new GcCourseUser
                                {
                                    SchoolId = schoolId,
                                    SchoolYear = schoolYear,
                                    LocalCourseCode = c.Key.LocalCourseCode,
                                    SectionIdentifier = c.Key.LocalCourseTitle,
                                    SessionName = c.Key.SessionName,
                                    EducationOrganizationId = leaId,
                                    EmailAddress = e,
                                    IsTeacher = false
                                }));
                            }

                        }

                    }

                }
                else
                {
                    if (rule.IncludeExclude.GetValueOrDefault())
                    {
                        var i = rule.RosterLocalCourses.Count != 0 ? classPool.Where(c => c.SessionName == rule.SessionName &&
                            rule.RosterLocalCourses.Any(t => t.LocalCourseCode.Trim() == c.LocalCourseCode.Trim())) : classPool.Where(c => c.SessionName == rule.SessionName);


                        foreach (var c in i)
                        {

                            schoolCourses.Add(new GcCourse
                            {
                                SchoolId = c.SchoolId,
                                SchoolYear = c.SchoolYear,
                                LocalCourseCode = c.LocalCourseCode,
                                SectionIdentifier = c.SectionIdentifier,
                                SessionName = c.SessionName,
                                EducationOrganizationId = c.EducationOrganizationId,
                                LocalCourseTitle = c.LocalCourseTitle,
                            });

                            if (rule.StaffOnly == true)
                            {
                                schoolCourseUsers.AddRange(c.StaffEmails.Distinct().Select(e => new GcCourseUser
                                {
                                    SchoolId = c.SchoolId,
                                    SchoolYear = c.SchoolYear,
                                    LocalCourseCode = c.LocalCourseCode,
                                    SectionIdentifier = c.SectionIdentifier,
                                    SessionName = c.SessionName,
                                    EducationOrganizationId = c.EducationOrganizationId,
                                    EmailAddress = e,
                                    IsTeacher = true
                                }));
                            }
                            else 
                            {
                                schoolCourseUsers.AddRange(c.StaffEmails.Distinct().Select(e => new GcCourseUser
                                {
                                    SchoolId = c.SchoolId,
                                    SchoolYear = c.SchoolYear,
                                    LocalCourseCode = c.LocalCourseCode,
                                    SectionIdentifier = c.SectionIdentifier,
                                    SessionName = c.SessionName,
                                    EducationOrganizationId = c.EducationOrganizationId,
                                    EmailAddress = e,
                                    IsTeacher = true
                                }));

                                schoolCourseUsers.AddRange(c.StudentEmails.Distinct().Select(e => new GcCourseUser
                                {
                                    SchoolId = c.SchoolId,
                                    SchoolYear = c.SchoolYear,
                                    LocalCourseCode = c.LocalCourseCode,
                                    SectionIdentifier = c.SectionIdentifier,
                                    SessionName = c.SessionName,
                                    EducationOrganizationId = c.EducationOrganizationId,
                                    EmailAddress = e,
                                    IsTeacher = false
                                }));
                            }

                        }
                    }
                    else
                    {
                        var j = classPool.Where(c => c.SessionName == rule.SessionName &&
                            rule.RosterLocalCourses.Any(t => t.LocalCourseCode.Trim() != c.LocalCourseCode.Trim()));

                        foreach (var c in j)
                        {
                            schoolCourses.Add(new GcCourse
                            {
                                SchoolId = c.SchoolId,
                                SchoolYear = c.SchoolYear,
                                LocalCourseCode = c.LocalCourseCode,
                                SectionIdentifier = c.SectionIdentifier,
                                SessionName = c.SessionName,
                                EducationOrganizationId = c.EducationOrganizationId,
                                LocalCourseTitle = c.LocalCourseTitle,
                            });

                            if (rule.StaffOnly == true)
                            {
                                schoolCourseUsers.AddRange(c.StaffEmails.Distinct().Select(e => new GcCourseUser
                                {
                                    SchoolId = c.SchoolId,
                                    SchoolYear = c.SchoolYear,
                                    LocalCourseCode = c.LocalCourseCode,
                                    SectionIdentifier = c.SectionIdentifier,
                                    SessionName = c.SessionName,
                                    EducationOrganizationId = c.EducationOrganizationId,
                                    EmailAddress = e,
                                    IsTeacher = true
                                }));
                            }
                            else 
                            {
                                schoolCourseUsers.AddRange(c.StaffEmails.Distinct().Select(e => new GcCourseUser
                                {
                                    SchoolId = c.SchoolId,
                                    SchoolYear = c.SchoolYear,
                                    LocalCourseCode = c.LocalCourseCode,
                                    SectionIdentifier = c.SectionIdentifier,
                                    SessionName = c.SessionName,
                                    EducationOrganizationId = c.EducationOrganizationId,
                                    EmailAddress = e,
                                    IsTeacher = true
                                }));

                                schoolCourseUsers.AddRange(c.StudentEmails.Distinct().Select(e => new GcCourseUser
                                {
                                    SchoolId = c.SchoolId,
                                    SchoolYear = c.SchoolYear,
                                    LocalCourseCode = c.LocalCourseCode,
                                    SectionIdentifier = c.SectionIdentifier,
                                    SessionName = c.SessionName,
                                    EducationOrganizationId = c.EducationOrganizationId,
                                    EmailAddress = e,
                                    IsTeacher = false
                                }));
                            }

                        }

                    }
                }
            }


            //grade rules
            var gradeRules = _context.ProvisioningRules.Include(s => s.RosterGradeLevels)
                .Where(r => r.EducationOrganizationId == leaId && r.SchoolYear == schoolYear && r.TypeId == 2
                            && (r.RosterSchools.Any(s => s.SchoolId == schoolId)
                                || (!r.RosterSchools.Any() &&
                                    (r.SchoolCategoryTypeId == category || r.SchoolCategoryTypeId == 0)))).ToList();
            var allSchools = gradeRules.Any(r => !r.RosterGradeLevels.Any());
            var gradeIds = gradeRules.SelectMany(r => r.RosterGradeLevels.Select(g => g.GradeLevelDescriptorId)).ToList();


            var schoolGrades = _apiContext.SchoolGradeLevels.Where(g => g.School.LocalEducationAgencyId == leaId && g.SchoolId == schoolId && (gradeIds.Any(r => g.GradeLevelDescriptorId == r) || allSchools))
                .Select(c => new GcCourse
                {
                    SchoolId = c.SchoolId,
                    SchoolYear = schoolYear,
                    LocalCourseCode = c.GradeLevelDescriptor.Descriptor.CodeValue,
                    SectionIdentifier = "Grade " + c.GradeLevelDescriptor.Descriptor.CodeValue,
                    LocalCourseTitle = "Grade " + c.GradeLevelDescriptor.Descriptor.CodeValue,
                    SessionName = "SY",
                    EducationOrganizationId = leaId
                }).ToList();

            if (schoolGrades.Any())
            {
                schoolCourses.AddRange(schoolGrades);
            }



            var existing = _context.GcCourses.Where(g => g.SchoolId == schoolId && g.SchoolYear == schoolYear).ToList();

            var toDelete = existing.Where(a => !schoolCourses.Any(b => a.LocalCourseCode == b.LocalCourseCode && a.SectionIdentifier == b.SectionIdentifier && a.SessionName == b.SessionName)).ToList();

            if (toDelete.Any())
            {
                _context.GcCourses.RemoveRange(toDelete);
            }

            var toAdd = schoolCourses.Where(a => !existing.Any(b => a.LocalCourseCode == b.LocalCourseCode && a.SectionIdentifier == b.SectionIdentifier && a.SessionName == b.SessionName)).ToList();

            if (toAdd.Any())
            {
                _context.GcCourses.AddRange(toAdd);
            }


            var existingUsers = _context.GcCourseUsers.Where(g => g.SchoolId == schoolId && g.SchoolYear == schoolYear).ToList();

            var toDeleteUsers = existingUsers.Where(a => !schoolCourseUsers.Any(b => a.LocalCourseCode == b.LocalCourseCode && a.SectionIdentifier == b.SectionIdentifier && a.SessionName == b.SessionName && a.EmailAddress == b.EmailAddress)).ToList();

            if (toDeleteUsers.Any())
            {
                _context.GcCourseUsers.RemoveRange(toDeleteUsers);
            }

            var toAddUsers = schoolCourseUsers.Where(a => !existingUsers.Any(b => a.LocalCourseCode == b.LocalCourseCode && a.SectionIdentifier == b.SectionIdentifier && a.SessionName == b.SessionName && a.EmailAddress == b.EmailAddress)).ToList();

            if (toAddUsers.Any())
            {
                _context.GcCourseUsers.AddRange(toAddUsers);
            }

                return _context.SaveChanges();

        }

        public List<GcCourse> GetSyncList(int leaId, short schoolYear, int schoolId)
        {
            var courses = _context.GcCourses.Include(g => g.GcCourseUsers).Where(g => g.SchoolId == schoolId && g.SchoolYear == schoolYear).ToList();

            var pref = _context.OrgGcPreferences.FirstOrDefault(p => p.EducationOrganizationId == leaId);
            if (pref != null && !string.IsNullOrWhiteSpace(pref.GcUserEmail) && !pref.AllowExternalDomains)
            {
                var domain = pref.GcUserEmail.Substring(pref.GcUserEmail.IndexOf("@", StringComparison.CurrentCultureIgnoreCase));
                foreach (var gcCourse in courses.Where(c => c.GcCourseUsers.Any(u => !u.EmailAddress.Contains(domain))))
                {
                    var users = gcCourse.GcCourseUsers.Where(u => u.EmailAddress.Contains(domain)).ToList();
                    gcCourse.GcCourseUsers.Clear();
                    foreach (var gcCourseUser in users)
                    {
                        gcCourse.GcCourseUsers.Add(gcCourseUser);
                    }
                }

            }

            return courses;

        }

        public List<GcCourse> GetActivateList(int leaId, short schoolYear, int schoolId)
        {
            var courses = _context.GcCourses.Where(g => g.SchoolId == schoolId && g.SchoolYear == schoolYear && g.Saved==true && (g.Activated==null || g.Activated==false)).ToList();
            return courses;

        }
        public GcCourse GetClassToSync(int leaId, short schoolYear, int schoolId, string gcName)
        {
            var course = _context.GcCourses.Include(g => g.GcCourseUsers).FirstOrDefault(g => g.SchoolId == schoolId && g.SchoolYear == schoolYear && g.GcName == gcName);
            var pref = _context.OrgGcPreferences.FirstOrDefault(p => p.EducationOrganizationId == leaId);
            if (pref != null && !string.IsNullOrWhiteSpace(pref.GcUserEmail) && !pref.AllowExternalDomains)
            {
                var domain = pref.GcUserEmail.Substring(pref.GcUserEmail.IndexOf("@", StringComparison.CurrentCultureIgnoreCase));
                var users = course.GcCourseUsers.Where(u => u.EmailAddress.Contains(domain)).ToList();
                course.GcCourseUsers.Clear();
                foreach (var gcCourseUser in users)
                {
                    course.GcCourseUsers.Add(gcCourseUser);
                }
            }
            
            return course;
        }

        public int SaveSyncProgress(int leaId, short schoolYear, int schoolId, List<GcCourse> courses)
        {
            var names = courses.Select(c => c.GcName).ToList();
            var existing = _context.GcCourses.Where(g => g.SchoolId == schoolId && g.SchoolYear == schoolYear && names.Contains(g.GcName)).ToList();
            foreach (var gcCourse in courses)
            {
                var toUpdate = existing.First(e => e.GcName == gcCourse.GcName);
                toUpdate.Saved = gcCourse.Saved;
                toUpdate.GcMessage = gcCourse.GcMessage.Length>200? gcCourse.GcMessage.Substring(0,197) + "...": gcCourse.GcMessage;
                if (gcCourse.Saved.GetValueOrDefault())
                {
                    toUpdate.Owner = gcCourse.Owner;
                    toUpdate.GcId = gcCourse.GcId;
                }
            }
            return _context.SaveChanges();

        }
    }
}
