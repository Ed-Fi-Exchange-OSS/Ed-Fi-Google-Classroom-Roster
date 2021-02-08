using log4net;
using System.Collections.Generic;
using System.Web.Mvc.Filters;
using WISEroster.Business;

namespace WISEroster.Mvc.ImplementationSpecific
{
    public class DebugWamsAuthenticationFilter : IAuthenticationFilter
    {
        private readonly ISessionInfo _sessionInfo;
        private readonly IOrganizationBusiness _orgBusiness;
        private readonly ILog _log;


        public DebugWamsAuthenticationFilter(ISessionInfo sessionInfo, IOrganizationBusiness orgBusiness, ILog log)
        {
            _sessionInfo = sessionInfo;
            _orgBusiness = orgBusiness;
            _log = log;
        }


        public virtual void OnAuthentication(AuthenticationContext filterContext)
        {
            var roles = new List<WamsRole>();
            roles.Add(new WamsRole
            {
                EducationOrganizationId = 6991,
                AgencyName = "Milton School District",
                Role = "ClassroomAdmin",
                Privileges = new List<WisePrivilege> { WisePrivilege.EditClassroom, WisePrivilege.ViewClassroom }
            });
          
            var wamsPrincipal = new WamsPrincipal("1234567890", "Demo", "User", "dev@dpi.wi.gov", roles);

            _sessionInfo.User = wamsPrincipal;

            filterContext.HttpContext.User = wamsPrincipal;
            filterContext.Controller.ViewBag.User = wamsPrincipal;
        }

        public virtual void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}