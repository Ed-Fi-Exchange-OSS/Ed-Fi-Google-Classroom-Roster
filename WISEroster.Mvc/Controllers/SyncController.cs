using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WISEroster.Business;
using WISEroster.Domain.Models;
using WISEroster.Mvc.Classroom;
using WISEroster.Mvc.Extensions;
using WISEroster.Mvc.ImplementationSpecific;
using WISEroster.Mvc.Models;

namespace WISEroster.Mvc.Controllers
{
    public class SyncController : Controller
    {
        private readonly IOrganizationBusiness _orgBusiness;
        private readonly ISessionInfo _sessionInfo;
        private readonly ISetupBusiness _setupBusiness;
        private readonly IProvisioningRulesBusiness _provisioningRuleBusiness; public SyncController(IOrganizationBusiness orgBusiness, ISessionInfo sessionInfo, ISetupBusiness setupBusiness, IProvisioningRulesBusiness provisioningRuleBusiness)
        {
            _orgBusiness = orgBusiness;
            _sessionInfo = sessionInfo;
            _setupBusiness = setupBusiness;
            _provisioningRuleBusiness = provisioningRuleBusiness;
        }

        public ActionResult Preview(short? schoolYear, int? schoolId)
        {
            var model = new SyncViewModel();
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            short sy = DateTime.Now.GetSchoolYear();
            if (schoolYear == null)
            {
                if (HttpContext.Session["SchoolYear"] != null)
                {
                    schoolYear = short.Parse(HttpContext.Session["SchoolYear"].ToString());
                }
                else
                {
                    schoolYear = sy;
                }
            }
            else
            {
                HttpContext.Session["SchoolYear"] = schoolYear;
            }

            model.SchoolYear = schoolYear.GetValueOrDefault();
            model.Years = new List<SelectListItem>
            {
                new SelectListItem{Text = string.Format("{0}-{1}", sy-1, sy), Value = sy.ToString()},
                new SelectListItem{Text = string.Format("{0}-{1}", sy, sy+1), Value = (sy+1).ToString()}
            };
            model.Schools = _orgBusiness.GetSchoolsForLea(schoolYear.GetValueOrDefault(), lea, null).Select(c => new SelectListItem
            { Text = c.EducationOrganization.NameOfInstitution, Value = c.SchoolId.ToString() }).ToList();
            if (schoolId == null)
            {
                int.TryParse(model.Schools.Select(v => v.Value).FirstOrDefault(), out var school);
                schoolId = school;
            }

            model.SelectedSchool = schoolId.GetValueOrDefault();

            model.SyncList =
                _provisioningRuleBusiness.GetSyncList(lea,
                    schoolYear.GetValueOrDefault(), schoolId.GetValueOrDefault());

            return View(model);
        }


        public JsonResult ClassList(short schoolYear, int schoolId)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var syncList =
                _provisioningRuleBusiness.GetSyncList(lea,
                    schoolYear, schoolId)
                    .Select(l => new { l.SchoolYear, l.LocalCourseTitle, l.LocalCourseCode, l.SessionName, l.SectionIdentifier, l.CreateDate, l.GcName, l.GcMessage, l.Activated, l.Saved, Staff = String.Join(", ", l.GcCourseUsers.Where(u => u.IsTeacher).Select(u => u.EmailAddress)) , Student = String.Join(", ", l.GcCourseUsers.Where(u => u.IsTeacher == false).Select(u => u.EmailAddress)) });
            HttpContext.Session["SchoolYear"] = schoolYear;
            return Json(syncList);
        }

        public ActionResult Generate(short schoolYear, int selectedSchool)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();

            var saved = _provisioningRuleBusiness.GenerateSyncList(lea, schoolYear, selectedSchool);

            return RedirectToAction("Preview", new { schoolYear = schoolYear, schoolId = selectedSchool });
        }

        [HttpGet]
        public async Task<JsonResult> TestConnection(CancellationToken cancellationToken)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var gcEmail = _setupBusiness.GetClientEmail(lea);
            if (string.IsNullOrWhiteSpace(gcEmail))
            {
                return Json(new TestConnectionMessage { Connected = false, Message = "Google Classroom setup not completed" }, JsonRequestBehavior.AllowGet);
            }

            var task = await Task.Run(async () => await GcSync.TestAsync(this, lea, gcEmail, _setupBusiness, cancellationToken).ConfigureAwait(true));

            return Json(task, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SendCourses(short schoolYear, int schoolId, bool allCourses, CancellationToken cancellationToken)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var gcEmail = _setupBusiness.GetClientEmail(lea);
            if (string.IsNullOrWhiteSpace(gcEmail))
            {
                return Json(new SyncCourseMessage { Message = "Google Classroom setup not completed" });
            }

            var syncList = _provisioningRuleBusiness.GetSyncList(lea, schoolYear, schoolId).Where(g => allCourses || !g.Saved.GetValueOrDefault()).ToList();
            var school = _orgBusiness.GetEducationOrganization(schoolYear, schoolId);

            foreach (var gcCourse in syncList)
            {
                gcCourse.GcId = "p:" + gcCourse.GcName;
                gcCourse.Owner = "me";
            }

            var syncMessage = new SyncCourseMessage { Courses = syncList, LeaId = lea, UserEmail = gcEmail, School = school };
            var task = await Task.Run(async () => await GcSync.SendCourses(this, _setupBusiness, syncMessage, cancellationToken).ConfigureAwait(true));

            _provisioningRuleBusiness.SaveSyncProgress(lea, schoolYear, schoolId, task.Courses);

            return Json(task);
        }

        [HttpPost]
        public async Task<string> SendSelectedCourse(short schoolYear, int schoolId, string classToSync, CancellationToken cancellationToken)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var gcEmail = _setupBusiness.GetClientEmail(lea);
            if (string.IsNullOrWhiteSpace(gcEmail))
            {
                return "Google Classroom setup not completed";
            }

            var gcCourse = _provisioningRuleBusiness.GetClassToSync(lea, schoolYear, schoolId, classToSync);
            var school = _orgBusiness.GetEducationOrganization(schoolYear, schoolId);

            gcCourse.GcId = "p:" + gcCourse.GcName;
            gcCourse.Owner = "me";

            var syncMessage = new SyncCourseMessage { Courses = new List<GcCourse> { gcCourse }, LeaId = lea, UserEmail = gcEmail, School = school };
            var task = await Task.Run(async () => await GcSync.SendCourses(this, _setupBusiness, syncMessage, cancellationToken).ConfigureAwait(true));

            _provisioningRuleBusiness.SaveSyncProgress(lea, schoolYear, schoolId, task.Courses);

            return task.Courses.First().GcMessage;
        }

        public async Task<JsonResult> GcList(CancellationToken cancellationToken)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var gcEmail = _setupBusiness.GetClientEmail(lea);

            var task = await Task.Run(async () => await GcSync.GetCourses(this, lea, gcEmail, _setupBusiness, cancellationToken).ConfigureAwait(true));
            return Json(task);
        }

        public async Task<ActionResult> Existing(CancellationToken cancellationToken)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var gcEmail = _setupBusiness.GetClientEmail(lea);
            var task = await Task.Run(async () => await GcSync.TestAsync(this, lea, gcEmail, _setupBusiness, cancellationToken).ConfigureAwait(true));
            var model = new DeleteCourseMessage();
            model.Message = task.Message;
            model.Success = task.Connected;
            return View(model);
        }

        [HttpPost]
        public async Task<string> DeleteCourse(string id, CancellationToken cancellationToken)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var gcEmail = _setupBusiness.GetClientEmail(lea);
            if (string.IsNullOrWhiteSpace(gcEmail))
            {
                return "Google Classroom setup not completed";
            }

            var task = await Task.Run(async () => await GcSync.DeleteCourse(this, _setupBusiness, id, lea, gcEmail, cancellationToken).ConfigureAwait(true));

            return task;
        }

        [HttpPost]
        public async Task<JsonResult> DeleteAll(List<string> ids, CancellationToken cancellationToken)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var gcEmail = _setupBusiness.GetClientEmail(lea);
            if (string.IsNullOrWhiteSpace(gcEmail))
            {
                return Json("Google Classroom setup not completed");
            }

            var task = await Task.Run(async () => await GcSync.DeleteCourses(this, _setupBusiness, ids, lea, gcEmail, cancellationToken).ConfigureAwait(true));

            return Json(task);
        }

        [HttpPost]
        public async Task<string> ActivateCourse(short schoolYear, int schoolId, string id, CancellationToken cancellationToken)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var gcEmail = _setupBusiness.GetClientEmail(lea);
            if (string.IsNullOrWhiteSpace(gcEmail))
            {
                return "Google Classroom setup not completed";
            }

            var task = await Task.Run(async () => await GcSync.ActivateCourse(this, _setupBusiness, "p:" + id, lea, gcEmail, cancellationToken).ConfigureAwait(true));
            if (task == "Active")
            {
                var gc = _provisioningRuleBusiness.GetClassToSync(lea, schoolYear, schoolId, id);
                gc.Activated = true;
                gc.GcMessage = $"Activated {DateTime.Now}, {gc.GcMessage}";
                _provisioningRuleBusiness.SaveSyncProgress(lea, schoolYear, schoolId, new List<GcCourse>{gc});
            }

            return task;
        }

        [HttpPost]
        public async Task<JsonResult> SendCourseActivations(short schoolYear, int schoolId, CancellationToken cancellationToken)
        {
            var lea = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var gcEmail = _setupBusiness.GetClientEmail(lea);
            if (string.IsNullOrWhiteSpace(gcEmail))
            {
                return Json(new SyncCourseMessage { Message = "Google Classroom setup not completed" });
            }

            var syncList = _provisioningRuleBusiness.GetActivateList(lea, schoolYear, schoolId);
            var school = _orgBusiness.GetEducationOrganization(schoolYear,  schoolId);

            foreach (var gcCourse in syncList)
            {
                gcCourse.GcId = "p:" + gcCourse.GcName;
                gcCourse.Owner = "me";
            }

            var syncMessage = new SyncCourseMessage { Courses = syncList, LeaId = lea, UserEmail = gcEmail, School = school };
            var task = await Task.Run(async () => await GcSync.SendCourseActivations(this, _setupBusiness, syncMessage, cancellationToken).ConfigureAwait(true));

            _provisioningRuleBusiness.SaveSyncProgress(lea, schoolYear, schoolId, task.Courses);

            return Json(task);
        }

        [HttpPost]
        public JsonResult KeepAlive()
        {
            return Json("success: true");
        }
    }
}