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
			AuthorizationTracker = new AuthorizationTracker();

			// See http://stackoverflow.com/questions/15212959/bad-request-on-processuserauthorization-dotnetopenauth-4-2-2-13055
			ClientCredentialApplicator = ClientCredentialApplicator.PostParameter(clientSecret);
		}
	}
}