using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Web;
using Ninject;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using WISEroster.Business;
using WISEroster.Mvc.Classroom;
using WISEroster.Mvc.ImplementationSpecific;

namespace WISEroster.Mvc.Controllers
{
    public class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        
        [Inject]
        public ISessionInfo _sessionInfo { private get; set; }

        [Inject]
        public ISetupBusiness _setupBusiness { private get; set; }


        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get { return new AppFlowMetadata(_sessionInfo.CurrentAgencyId.GetValueOrDefault(), _setupBusiness); }
        }

        [AsyncTimeout(10000)]
        public override async Task<ActionResult> IndexAsync(AuthorizationCodeResponseUrl authorizationCode,
            CancellationToken taskCancellationToken)
        {
            if (string.IsNullOrEmpty(authorizationCode.Code))
            {
                var errorResponse = new TokenErrorResponse(authorizationCode);
                Logger.Info("Received an error. The response is: {0}", errorResponse);

                return OnTokenError(errorResponse);
            }

            Logger.Debug("Received \"{0}\" code", authorizationCode.Code);

            var returnUrl = Request.Url.ToString();
            returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("?"));

            //AS- make sure _ISessionInfo is initialized
            var l = _sessionInfo.CurrentAgencyId.GetValueOrDefault();
            var u = _setupBusiness.GetClientEmail(l);
            //AS

            var token = await Flow.ExchangeCodeForTokenAsync(u, authorizationCode.Code, returnUrl,
                taskCancellationToken).ConfigureAwait(true);//AS-include context

            // Extract the right state.
            var oauthState = await AuthWebUtility.ExtracRedirectFromState(Flow.DataStore, u,
                authorizationCode.State).ConfigureAwait(true);//AS-include context

            return new RedirectResult(oauthState);
        }

    }
}