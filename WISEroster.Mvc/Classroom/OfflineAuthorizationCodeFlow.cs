namespace WISEroster.Mvc.Classroom
{

    //https://keestalkstech.com/2016/07/offline-google-authentication-for-mvc-net/

    using Google.Apis.Auth.OAuth2.Flows;
    using Google.Apis.Auth.OAuth2.Requests;
    using System;

    /// <summary>
    /// Requests an offline token from Google.
    /// </summary>
    /// <seealso cref="Google.Apis.Auth.OAuth2.Flows.GoogleAuthorizationCodeFlow" />
    public class OfflineAuthorizationCodeFlow : GoogleAuthorizationCodeFlow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfflineAuthorizationCodeFlow"/> class.
        /// </summary>
        /// <param name="initializer"></param>
        public OfflineAuthorizationCodeFlow(Initializer initializer) : base(initializer)
        {
        }

        /// <summary>
        /// Creates the authorization code request.
        /// </summary>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <returns></returns>
        public override AuthorizationCodeRequestUrl CreateAuthorizationCodeRequest(string redirectUri)
        {
            return new GoogleAuthorizationCodeRequestUrl(new Uri(AuthorizationServerUrl))
            {
                ClientId = ClientSecrets.ClientId,
                Scope = string.Join(" ", Scopes),
                RedirectUri = redirectUri,
                AccessType = "offline",
                ApprovalPrompt = "auto"
            };
        }
    }
}