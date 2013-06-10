using System;
using System.Configuration;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using SimpleSocialAuth.Core.Consumers;

namespace SimpleSocialAuth.Core.Handlers
{
    public class FacebookHandler : IAuthenticationHandler
    {
        private static readonly FacebookConsumer facebookConsumer =
            new FacebookConsumer(
				ConfigurationManager.AppSettings["facebookAppID"],
				ConfigurationManager.AppSettings["facebookAppSecret"]);

        #region IAuthenticationHandler Members

        public string PrepareAuthRequest(PrepareAuthenticationContext context)
        {
            var authorization =
                facebookConsumer.ProcessUserAuthorization();

            var callback =
                new Uri(Utils.GetUrlBase(context.Request) + context.RedirectPath);

            if (authorization == null)
            {
                return
                    facebookConsumer
                        .PrepareRequestUserAuthorization(returnTo: callback)
                        .Headers["Location"];
            }

            return null;
        }

        public BasicUserData ProcessAuthRequest(ProcessAuthenticationContext context)
        {
            var authorization =
                facebookConsumer.ProcessUserAuthorization();

            if (authorization.AccessToken == null)
            {
                return null;
            }

            var graphRequest =
                WebRequest
                    .Create("https://graph.facebook.com/me?access_token=" +
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
                            PictureUrl = string.Format("http://graph.facebook.com/{0}/picture", jsonObject["id"])
                        };
            }
        }

        #endregion
    }
}