using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WISEroster.Business;
using WISEroster.Business.Models;
using WISEroster.Mvc.Extensions;
using WISEroster.Mvc.ImplementationSpecific;
using WISEroster.Mvc.Models;

namespace WISEroster.Mvc.Controllers
{
    public class HomeController : Controller
    {

        private readonly IOrganizationBusiness _orgBusiness;
        private readonly ISessionInfo _sessionInfo;
        private readonly IRosterBusiness _rosterBusiness;
        private readonly IProvisioningRulesBusiness _provisioningRuleBusiness;

        public HomeController(IOrganizationBusiness orgBusiness, ISessionInfo sessionInfo, IRosterBusiness rosterBusiness, IProvisioningRulesBusiness provisioningRuleBusiness)
        {
            _orgBusiness = orgBusiness;
            _sessionInfo = sessionInfo;
            _rosterBusiness = rosterBusiness;
            _provisioningRuleBusiness = provisioningRuleBusiness;
        }

        public ActionResult Index()
        {

            var schoolYear = DateTime.Now.GetSchoolYear();
            if (HttpContext.Session["SchoolYear"] != null)
            {
                short.TryParse(HttpContext.Session["SchoolYear"].ToString(), out schoolYear);
            }

            var model = new ProvisioningRulesViewModel();
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            model.EducationOrganizationId = lea;
            model.SchoolYear = schoolYear;
            model.TypeId = 1;

            SetDropdowns(model);

            model.IncludeExclude = true;
            model.IncludeExcludeByTeacher = true;

            return View(model);
        }

        public ActionResult Rule(ProvisioningRulesViewModel model)
        {
            if (model != null)
            {
                if (model.SchoolYear == 0)
                {
                    ModelState.AddModelError("SchoolYear", "School Year selection is required");
                }

                if ((model.SchoolCategoryDescriptorId == null || model.SchoolCategoryDescriptorId == 0) && (model.SelectedSchools == null || !model.SelectedSchools.Any()))
                {
                    ModelState.AddModelError("SelectedSchools", "School selection is required");
                }

                if (model.TypeId == 1)
                {
                    if (model.SelectedSession == null || string.IsNullOrWhiteSpace(model.SelectedSession))
                    {
                        ModelState.AddModelError("SelectedSession", "Session selection is required");
                    }
                    if (model.SelectedCourses == null || !model.SelectedCourses.Any())
                    {
                        ModelState.AddModelError("SelectedCourses", "Course selection is required");
                    }
                }
                else if(model.TypeId == 2)
                {
                    if (model.SelectedGrades == null || !model.SelectedGrades.Any())
                    {
                        ModelState.AddModelError("SelectedGrades", "Grade selection is required");
                    }
                }

                else 
                {
                    if (model.SelectedSessionByTeacher == null || string.IsNullOrWhiteSpace(model.SelectedSessionByTeacher))
                    {
                        ModelState.AddModelError("SelectedSessionByTeacher", "Session selection is required");
                    }
                    if (model.SelectedCoursesByTeacher == null || !model.SelectedCoursesByTeacher.Any())
                    {
                        ModelState.AddModelError("SelectedCoursesByTeacher", "Course selection is required");
                    }
                    if (model.SelectedTeacher == null || model.SelectedTeacher == 0)
                    {
                        ModelState.AddModelError("SelectedTeacher", "Teacher selection is required");
                    }
                }
                //TO DO
                if (ModelState.IsValid)
                {
                    var insert = new ProvisioningRulesInsertModel();
                    insert.SchoolYear = model.SchoolYear;
                    insert.EducationOrganizationId = model.EducationOrganizationId;
                    insert.SchoolCategoryDescriptorId = model.SchoolCategoryDescriptorId;
                    insert.SelectedSchools = model.SelectedSchools;                   
                    insert.TypeId = model.TypeId;
                    if (model.TypeId == 1)
                    {
                        insert.SelectedSession = model.SelectedSession;
                        insert.SelectedCourses = model.SelectedCourses;
                        insert.IncludeExclude = model.IncludeExclude;
                        insert.GroupByTitle = model.GroupByTitle;
                        insert.StaffOnly = model.StaffOnly;
                    }
                    else if (model.TypeId == 2)
                    {
                        insert.SelectedGrades = model.SelectedGrades;
                    }
                    else
                    {
                        insert.SelectedSession = model.SelectedSessionByTeacher;
                        insert.SelectedTeacher = model.SelectedTeacher;
                        insert.SelectedCourses = model.SelectedCoursesByTeacher;
                        insert.IncludeExclude = model.IncludeExcludeByTeacher;
                        insert.GroupByTitle = model.GroupByTitleByTeacher;
                        insert.StaffOnly = model.StaffOnlyByTeacher;
                    }

                    _provisioningRuleBusiness.InsertRule(insert);
                    TempData["Message"] = "Success";
                    HttpContext.Session["SchoolYear"] = model.SchoolYear;
                    return RedirectToAction("Index");
                }

                SetDropdowns(model);
                return View("Index", model);
            }

            return RedirectToAction("Index");
        }


        private void SetDropdowns(ProvisioningRulesViewModel model)
        {
            var sy = DateTime.Now.GetSchoolYear();
            model.Years = new List<SelectListItem>
            {
                new SelectListItem{Text = string.Format("{0}-{1}", sy-1, sy), Value = sy.ToString()},
                new SelectListItem{Text = string.Format("{0}-{1}", sy, sy+1), Value = (sy+1).ToString()}
            };
            model.Categories = _orgBusiness.GetSchoolCategoriesForLea(model.SchoolYear, model.EducationOrganizationId).Select(c => new SelectListItem
            { Text = c.ShortDescription, Value = c.DescriptorId.ToString() }).ToList();

            model.Schools = _orgBusiness.GetSchoolsForLea(model.SchoolYear, model.EducationOrganizationId, model.SchoolCategoryDescriptorId).Select(c => new SelectListItem
            { Text = c.EducationOrganization.NameOfInstitution, Value = c.SchoolId.ToString() }).ToList();
            model.Schools.Insert(0, item: new SelectListItem { Text = "All Schools", Value = "" });
            model.Sessions = _rosterBusiness.GetSessions(model.SchoolYear, model.EducationOrganizationId, model.SchoolCategoryDescriptorId, model.SelectedSchools).Select(c => new SelectListItem
            { Text = c, Value = c }).ToList();

            model.Courses = _rosterBusiness.GetCourseTitles(model.SchoolYear, model.EducationOrganizationId, model.SchoolCategoryDescriptorId, model.SelectedSchools, model.SelectedSession).Select(c => new SelectListItem
            { Text = c.CourseCode + " - " + c.CourseTitle, Value = c.CourseCode }).ToList();
            model.Courses.Insert(0, item: new SelectListItem { Text = "All Courses", Value = "" });

            model.Grades = _rosterBusiness.GetGrades(model.SchoolYear, model.EducationOrganizationId, model.SchoolCategoryDescriptorId, model.SelectedSchools).Select(c => new SelectListItem
            { Text = c.ShortDescription, Value = c.DescriptorId.ToString() }).ToList();

            model.Teachers = _rosterBusiness.GetStaff(model.SchoolYear, model.EducationOrganizationId, model.SchoolCategoryDescriptorId, model.SelectedSchools, model.SelectedSession).Select(c => new SelectListItem
            { Text = c.FirstName + "  " + c.LastSurname, Value = c.StaffUsi.ToString() }).ToList();

            model.CoursesByTeacher = _rosterBusiness.GetCourseTitlesByTeacher(model.SchoolYear, model.EducationOrganizationId, model.SchoolCategoryDescriptorId, model.SelectedSchools, model.SelectedSession, model.SelectedTeacher).Select(c => new SelectListItem
            { Text = c.CourseCode + " - " + c.CourseTitle, Value = c.CourseCode }).ToList();
            model.CoursesByTeacher.Insert(0, item: new SelectListItem { Text = "All Courses", Value = "" });
        }

        [HttpPost]
        public JsonResult DeleteRule(int id)
        {
            try
            {
                _provisioningRuleBusiness.DeleteRule(id);
            }
            catch (Exception ex)
            {
                //TODO: log this
            }
            return Json(true);

        }

        [HttpPost]
        public JsonResult StaffOnly(int id)
        {
            try
            {
                _provisioningRuleBusiness.StaffOnly(id);
            }
            catch (Exception ex)
            {
                //TODO: log this
            }
            return Json(true);

        }

        [HttpPost]
        public JsonResult IncludeStudents(int id)
        {
            try
            {
                _provisioningRuleBusiness.IncludeStudents(id);
            }
            catch (Exception ex)
            {
                //TODO: log this
            }
            return Json(true);

        }

        public JsonResult RuleList(short schoolYear)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var rules = _provisioningRuleBusiness.GetRuleList(lea, schoolYear);
            HttpContext.Session["SchoolYear"] = schoolYear;
            return Json(rules);
        }

        public JsonResult ClassList(int ruleId)
        {
            var rules = _provisioningRuleBusiness.GetClassesForRule(ruleId);
            return Json(rules);
        }

        public JsonResult FilterSchools(ProvisioningRulesViewModel model)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var options = _orgBusiness.GetSchoolsForLea(model.SchoolYear, lea, model.SchoolCategoryDescriptorId).Select(c => new SelectListItem
            { Text = c.EducationOrganization.NameOfInstitution, Value = c.SchoolId.ToString() }).ToList();
            return Json(options, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterTerms(ProvisioningRulesViewModel model)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();

            var options = _rosterBusiness.GetSessions(model.SchoolYear, lea, model.SchoolCategoryDescriptorId, model.SelectedSchools).Select(c => new SelectListItem
            { Text = c, Value = c }).ToList();
            return Json(options, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterTitles(ProvisioningRulesViewModel model)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();

            var options = _rosterBusiness.GetCourseTitles(model.SchoolYear, lea, model.SchoolCategoryDescriptorId, model.SelectedSchools, model.SelectedSession).Select(c => new SelectListItem
            { Text = c.CourseCode + " - " + c.CourseTitle, Value = c.CourseCode }).ToList();
            return Json(options, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterGrades(ProvisioningRulesViewModel model)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();

            var options = _rosterBusiness.GetGrades(model.SchoolYear, lea, model.SchoolCategoryDescriptorId, model.SelectedSchools).Select(c => new SelectListItem
            { Text = c.ShortDescription, Value = c.DescriptorId.ToString() }).ToList();
            return Json(options, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterTeachers(ProvisioningRulesViewModel model)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();

            var options = _rosterBusiness.GetStaff(model.SchoolYear, lea, model.SchoolCategoryDescriptorId, model.SelectedSchools, model.SelectedSessionByTeacher).Select(c => new SelectListItem
            { Text = c.FirstName + "  " + c.LastSurname, Value = c.StaffUsi.ToString() }).ToList();
            return Json(options, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterTitlesByTeacher(ProvisioningRulesViewModel model)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();

            var options = _rosterBusiness.GetCourseTitlesByTeacher(model.SchoolYear, lea, model.SchoolCategoryDescriptorId, model.SelectedSchools, model.SelectedSessionByTeacher, model.SelectedTeacher).Select(c => new SelectListItem
            { Text = c.CourseCode + " - " + c.CourseTitle, Value = c.CourseCode }).ToList();
            return Json(options, JsonRequestBehavior.AllowGet);
        }
    }
}