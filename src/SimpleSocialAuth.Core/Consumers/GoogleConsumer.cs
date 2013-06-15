using System;
using DotNetOpenAuth.OAuth2;

namespace SimpleSocialAuth.Core.Consumers
{
    internal class GoogleConsumer : WebServerClient
    {
        private static readonly AuthorizationServerDescription GoogleDescription =
            new AuthorizationServerDescription
                {
                    TokenEndpoint = new Uri("https://accounts.google.com/o/oauth2/token?grant_type=authorization_code"),
                    AuthorizationEndpoint = new Uri("https://accounts.google.com/o/oauth2/auth")
                };

        public GoogleConsumer(string clientIdentifier, string clientSecret)
            : base(GoogleDescription, clientIdentifier, clientSecret)
        {
            AuthorizationTracker = new AuthorizationTracker();

            // See https://groups.google.com/forum/?fromgroups#!topic/dotnetopenid/ibzRfE4TpB0
            ClientCredentialApplicator = ClientCredentialApplicator.PostParameter(clientSecret);
        }
    }
}