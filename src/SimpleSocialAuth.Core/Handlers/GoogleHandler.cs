using System;
using System.Configuration;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using SimpleSocialAuth.Core.Consumers;

namespace SimpleSocialAuth.Core.Handlers
{
    /// <summary>
    /// Google authentication handler
    /// </summary>
    /// <remarks>Requires that the "googleAppID" and "googleAppSecret" is specified in the appSettings in web/app.config.</remarks>
    public class GoogleHandler : IAuthenticationHandler
    {
        private static readonly GoogleConsumer googleConsumer = new GoogleConsumer(
            ConfigurationManager.AppSettings["googleAppID"], 
            ConfigurationManager.AppSettings["googleAppSecret"]);

        #region IAuthenticationHandler Members

        public string PrepareAuthRequest(PrepareAuthenticationContext context)
        {
            var authorization =
                googleConsumer.ProcessUserAuthorization();

            var callback =
                new Uri(Utils.GetUrlBase(context.Request) + context.RedirectPath);

            if (authorization == null)
            {
                return
                    googleConsumer
                        .PrepareRequestUserAuthorization(returnTo: callback,
                                                         scopes:
                                                             new[] {"https://www.googleapis.com/auth/userinfo.profile"})
                        .Headers["Location"];
            }

            return null;
        }

        public BasicUserData ProcessAuthRequest(ProcessAuthenticationContext context)
        {
            var authorization =
                googleConsumer.ProcessUserAuthorization();

            if (authorization.AccessToken == null)
            {
                return null;
            }

            var graphRequest =
                WebRequest
                    .Create("https://www.googleapis.com/oauth2/v1/userinfo?access_token=" +
                            Uri.EscapeDataString(authorization.AccessToken));

            using (var response = graphRequest.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var streamReader = new StreamReader(responseStream))
            {
                var json = streamReader.ReadToEnd();
                var jsonObject = JObject.Parse(json);

                return
                    new BasicUserData
                        {
                            UserId = jsonObject["id"].ToString(),
                            UserName = jsonObject["name"].ToString(),
                            PictureUrl = jsonObject["picture"].ToString()
                        };
            }
        }

        #endregion
    }
}