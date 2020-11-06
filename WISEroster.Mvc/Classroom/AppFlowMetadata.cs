
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Classroom.v1;
using WISEroster.Business;

namespace WISEroster.Mvc.Classroom
{
    public class AppFlowMetadata : FlowMetadata
    {
        private readonly int _lea;
        private readonly IAuthorizationCodeFlow _flow;
        private readonly ISetupBusiness _setupBusiness;
        public AppFlowMetadata(int leaId, ISetupBusiness setupBusiness)
        {
            _lea = leaId;
            _setupBusiness = setupBusiness;
            var cred = setupBusiness.GetClientCredentials(leaId);
            string client = string.Empty;
            string secret = string.Empty;
            if (cred != null)
            {
                client = cred.ClientId;
                secret = cred.ClientSecret;
            }

            _flow = new OfflineAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = client,
                    ClientSecret = secret
                },
                Scopes = new[] { ClassroomService.Scope.ClassroomCourses, ClassroomService.Scope.ClassroomRosters, ClassroomService.Scope.ClassroomProfileEmails  },
                DataStore = new ClassroomDataStore(setupBusiness),
               
            });

        }

        public override string GetUserId(Controller controller)
        {

            return _setupBusiness.GetClientEmail(_lea);

        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return _flow; }
        }

        public override string AuthCallback
        {
            get { return AppSettings.IsLocal? "/AuthCallback/IndexAsync" :
                "/WISEroster/AuthCallback/IndexAsync"; }//AS need Wiseroster in path on server
        }
    }

}