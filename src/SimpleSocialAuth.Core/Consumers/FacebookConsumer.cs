using System;
using DotNetOpenAuth.OAuth2;

namespace SimpleSocialAuth.Core.Consumers
{
    internal class FacebookConsumer : WebServerClient
    {
        private static readonly AuthorizationServerDescription FacebookDescription =
            new AuthorizationServerDescription
                {
                    TokenEndpoint = new Uri("https://graph.facebook.com/oauth/access_token"),
                    AuthorizationEndpoint = new Uri("https://graph.facebook.com/oauth/authorize")
                };

        public FacebookConsumer(string clientIdentifier, string clientSecret)
            : base(FacebookDescription, clientIdentifier, clientSecret)
        {
            AuthorizationTracker =
                new AuthorizationTracker();
        }
    }
}