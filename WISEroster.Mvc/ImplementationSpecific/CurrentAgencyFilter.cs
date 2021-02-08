using log4net;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using WISEroster.Business;
using WISEroster.Mvc.Extensions;

namespace WISEroster.Mvc.ImplementationSpecific
{

    public class CurrentAgencyFilter : IAuthenticationFilter
        {
        private readonly ISessionInfo _sessionInfo;
        private readonly IOrganizationBusiness _orgBusiness;
        private readonly ILog _log;

        public CurrentAgencyFilter(ISessionInfo sessionInfo, IOrganizationBusiness orgBusiness, ILog log)
        {
            _sessionInfo = sessionInfo;
            _orgBusiness = orgBusiness;
            _log = log;
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var wamsUser = (WamsPrincipal)filterContext.RequestContext.HttpContext.User;
            if (wamsUser == null)
            {
                _log.Error($"{wamsUser} is null");

                filterContext.Result = SevereError();
                return;
            }

            var agencies = wamsUser.Roles
                .Select(x => x.EducationOrganizationId)
                .Distinct()
                .ToList();

            if (!agencies.Any())
            {
                _log.Error($"{wamsUser.FullName} has no agencies");
                filterContext.Result = SevereError();

                return;
            }

            var schoolYear = DateTime.Today.GetSchoolYear();


            _sessionInfo.User = wamsUser;

            if (_sessionInfo.CurrentAgencyId == null)
            {
                if (agencies.Count() == 1)
                {
                    var agencyKey = wamsUser.Roles.First().EducationOrganizationId;
                    var edOrgId = System.Convert.ToInt32(agencyKey);
                    var org = _orgBusiness.GetEducationOrganization(schoolYear, edOrgId);
                    if (org == null)
                    {
                        _log.Error($"{wamsUser.FullName} {_sessionInfo.CurrentAgencyId} org is null");
                        filterContext.Result = SevereError();
                        return;
                    }
                    _sessionInfo.SetCurrentAgency(agencyKey);

                    filterContext.Controller.ViewBag.DefaultAgency = org.NameOfInstitution;
                }
                else
                {
                    var area = (string)filterContext.RouteData.DataTokens["area"];
                    var controller = (string)filterContext.RouteData.Values["controller"];
                    var action = (string)filterContext.RouteData.Values["action"];
                    if (controller.Equals("Debug", StringComparison.InvariantCultureIgnoreCase) || controller.Equals("Error", StringComparison.InvariantCultureIgnoreCase) ||
                        (
                            controller.Equals("User", StringComparison.InvariantCultureIgnoreCase)
                            && (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) || action.Equals("ChangeAgency", StringComparison.InvariantCultureIgnoreCase)))
                        )
                        return;

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        {"area", ""},
                        {"controller", "User"},
                        {"action", "Index"}
                    });
                }
            }
            else
            {
                var org = _orgBusiness.GetEducationOrganization(schoolYear, _sessionInfo.CurrentAgencyId.GetValueOrDefault());
                if (org == null)
                {
                    var impersonateAgencyPrivilege = ConfigurationManager.AppSettings["DPI.ImpersonateAgencyPrivilege"];
                    var dpiAgencyKey = int.Parse(ConfigurationManager.AppSettings["DPI.AgencyKey"]);

                    if (agencies.Count() > 1 || wamsUser.HasPrivilege(dpiAgencyKey, impersonateAgencyPrivilege))
                    {
                        //agency not found, but more than one agency or can impersonate
                        var area = (string)filterContext.RouteData.DataTokens["area"];
                        var controller = (string)filterContext.RouteData.Values["controller"];
                        var action = (string)filterContext.RouteData.Values["action"];
                        if (controller.Equals("Debug", StringComparison.InvariantCultureIgnoreCase) || controller.Equals("Error", StringComparison.InvariantCultureIgnoreCase) ||
                            (
                                controller.Equals("User", StringComparison.InvariantCultureIgnoreCase)
                                && (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) || action.Equals("ChangeAgency", StringComparison.InvariantCultureIgnoreCase)))
                        )
                            return;

                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                            {
                                {"area", ""},
                                {"controller", "User"},
                                {"action", "Index"}
                            });
                    }
                    else
                    {

                        _log.Error($"{wamsUser.FullName} cannot access {_sessionInfo.CurrentAgencyId}");
                        filterContext.Result = SevereError();
                        return;
                    }

                }
                else
                {

                    filterContext.Controller.ViewBag.DefaultAgency = org.NameOfInstitution;
                }
            }


            if (_sessionInfo.CurrentAgencyId.GetValueOrDefault()>0)
            {
                var impersonateAgencyPrivilege = ConfigurationManager.AppSettings["DPI.ImpersonateAgencyPrivilege"];
                var dpiAgencyKey =  int.Parse(ConfigurationManager.AppSettings["DPI.AgencyKey"]);

                filterContext.Controller.ViewBag.Privileges = wamsUser.GetPrivileges(_sessionInfo.CurrentAgencyId.GetValueOrDefault());
                filterContext.Controller.ViewBag.MultipleAgency = agencies.Count() > 1 || wamsUser.HasPrivilege(dpiAgencyKey, impersonateAgencyPrivilege);
            }

        }

        private ActionResult SevereError()
        {

            //severe error for when we cannot load user info
            return new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"area", ""},
                    {"controller", "Error"},
                    {"action", "SevereError"}
                });

        }

        private ActionResult Debug()
        {
            //severe error for when we cannot load user info
            return new RedirectToRouteResult(new RouteValueDictionary
            {
                {"area", ""},
                {"controller", "User"},
                {"action", "Debug"}
            });

        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }


    }


}
