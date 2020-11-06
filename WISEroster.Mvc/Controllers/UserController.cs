using Google.Apis.Auth.OAuth2.Mvc;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using WISEroster.Business;
using WISEroster.Business.Models;
using WISEroster.Domain.Models;
using WISEroster.Mvc.Classroom;
using WISEroster.Mvc.Extensions;
using WISEroster.Mvc.ImplementationSpecific;
using WISEroster.Mvc.Models;

namespace WISEroster.Mvc.Controllers
{

    public class UserController : Controller
    {

        private readonly ISessionInfo _sessionInfo;
        private readonly IOrganizationBusiness _orgBusiness;
        private readonly ISetupBusiness _setupBusiness;
     
        public UserController(IOrganizationBusiness orgBusiness, ISetupBusiness setupBusiness, ISessionInfo sessionInfo)
        {
            _orgBusiness = orgBusiness;
            _sessionInfo = sessionInfo;
            _setupBusiness = setupBusiness;
        }

       [HttpGet]
        public ActionResult Index()
        {

            var wamsUser = _sessionInfo.User;
            var agencies = wamsUser.Roles.Select(x => x.EducationOrganizationId).ToList();
            var impersonateAgencyPrivilege = ConfigurationManager.AppSettings["DPI.ImpersonateAgencyPrivilege"];
            var dpiAgencyKey = int.Parse( ConfigurationManager.AppSettings["DPI.AgencyKey"]);

            var orgs = _orgBusiness.GetEducationOrganizations(DateTime.Now.GetSchoolYear(), agencies).Select(o => new SelectableAgency { EducationOrganizationId = o.EducationOrganizationId, NameOfInstitution = o.NameOfInstitution });
            var viewModel = new ChangeAgencyViewModel { Organizations = orgs.ToList() };
            var impersonateAgencies = wamsUser.HasPrivilege(dpiAgencyKey, impersonateAgencyPrivilege);
            if (impersonateAgencies)
            {

                viewModel.AllOrganizations = _orgBusiness.GetLeas(DateTime.Now.GetSchoolYear())
                    .Select(o => new SelectableAgency { EducationOrganizationId = o.EducationOrganizationId, NameOfInstitution = o.NameOfInstitution })
                    .OrderBy(n => n.NameOfInstitution).ToList();
            }

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult ChangeAgency(int id)
        {
            _sessionInfo.SetCurrentAgency(id);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Debug()
        {

            var model = new UserDebugViewModel { EducationOrganizationId = null, Name = "null" };
            try
            {
                model.SchoolYear = DateTime.Now.GetSchoolYear();
                if (_sessionInfo != null)
                {
                    if (_sessionInfo.User != null)
                    {
                        model.Name = _sessionInfo.User.FullName;

                    }

                    if (_sessionInfo.CurrentAgencyId != null)
                    {
                        model.EducationOrganizationId = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
                    }

                    var org = _orgBusiness.GetEducationOrganization(DateTime.Now.GetSchoolYear(), 7533);
                    if (org != null)
                    {
                        model.ApiConnect = true;
                    }

                    if (_sessionInfo.User != null)
                    {
                        var agencies = _sessionInfo.User.Roles.Select(x => x.EducationOrganizationId).ToList();
                        model.Agencies = agencies;
                        var impersonateAgencyPrivilege = ConfigurationManager.AppSettings["DPI.ImpersonateAgencyPrivilege"];
                        var dpiAgencyKey = int.Parse(ConfigurationManager.AppSettings["DPI.AgencyKey"]);
                        var orgs = _orgBusiness.GetEducationOrganizations(DateTime.Now.GetSchoolYear(), agencies).Select(o => new SelectableAgency { EducationOrganizationId = o.EducationOrganizationId, NameOfInstitution = o.NameOfInstitution });
                        model.SelectableAgencies = orgs.ToList();

                        model.CanImpersonate = _sessionInfo.User.HasPrivilege(dpiAgencyKey, impersonateAgencyPrivilege);
                    }

                }
            }
            catch (Exception ex)
            {
                model.Exception = ex.ToString();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GcPreferences(CancellationToken cancellationToken)
        {
            var edOrgId = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var pref = _setupBusiness.GetGcPreference(edOrgId);
            var model = new GcPreferenceViewModel();
            if (pref != null)
            {
                model.GcUserEmail = pref.GcUserEmail;
                model.AllowExternalDomains = pref.AllowExternalDomains;

                var cred = _setupBusiness.GetClientCredentials(edOrgId);
                if (cred != null)
                {
                    model.ClientId = cred.ClientId;
                    model.ClientSecret = cred.ClientSecret;
                    var task = await Task.Run(async () => await GcSync.TestAsync(this, edOrgId, model.GcUserEmail, _setupBusiness, cancellationToken).ConfigureAwait(true));

                    model.CredentialMessage = task.Message;
                }

            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GcPreferences(GcPreferenceViewModel model, CancellationToken cancellationToken)
        {
            var edOrgId = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var pref = new OrgGcPreference
            {
                EducationOrganizationId = edOrgId,
                AllowExternalDomains = model.AllowExternalDomains,
                GcUserEmail = model.GcUserEmail
            };
            var creds = new ClientCredentials
            {
                EducationOrganizationId = edOrgId,
                ClientId = model.ClientId,
                ClientSecret = model.ClientSecret
            };

            try
            {
                _setupBusiness.UpsertGcPreference(pref);
                _setupBusiness.UpsertClientCredentials(creds);

                return RedirectToAction("ConfirmAsync","User",new{ leaId=edOrgId, userEmail=model.GcUserEmail});

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View(model);
            }
        }

        public async Task<ActionResult> ConfirmAsync(int leaId, string userEmail, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata(leaId, _setupBusiness)).
                AuthorizeAsync(userEmail,cancellationToken).ConfigureAwait(true);

            if (result.Credential != null)
            {
                return RedirectToAction("GcPreferences");
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }
    }
}