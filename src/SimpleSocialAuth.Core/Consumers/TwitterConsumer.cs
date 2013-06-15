using System;
using System.Configuration;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;

namespace SimpleSocialAuth.Core.Consumers
{
	internal class TwitterConsumer
	{
		public static readonly ServiceProviderDescription SignInWithTwitterServiceDescription =
			new ServiceProviderDescription
				{
					RequestTokenEndpoint =
						new MessageReceivingEndpoint(
							"https://twitter.com/oauth/request_token",
							HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
					UserAuthorizationEndpoint =
						new MessageReceivingEndpoint(
							"https://twitter.com/oauth/authenticate",
							HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
					AccessTokenEndpoint =
						new MessageReceivingEndpoint(
							"https://twitter.com/oauth/access_token",
							HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
					TamperProtectionElements =
						new ITamperProtectionChannelBindingElement[]
							{
								new HmacSha1SigningBindingElement()
							},
				};

		private static WebConsumer signInConsumer;
		private static readonly object signInConsumerInitLock = new object();
		private readonly ISessionStorage _sessionStorage;

		public TwitterConsumer(ISessionStorage sessionStorage)
		{
			_sessionStorage = sessionStorage;
		}

		private bool IsTwitterConsumerConfigured
		{
			get
			{
				return
					!string.IsNullOrEmpty(ConfigurationManager.AppSettings["twitterConsumerKey"]) &&
					!string.IsNullOrEmpty(ConfigurationManager.AppSettings["twitterConsumerSecret"]);
			}
		}

		private WebConsumer TwitterSignIn
		{
			get
			{
				if (signInConsumer == null)
				{
					lock (signInConsumerInitLock)
					{
						if (signInConsumer == null)
						{
							signInConsumer = new WebConsumer(SignInWithTwitterServiceDescription, ShortTermUserSessionTokenManager);
						}
					}
				}

				return signInConsumer;
			}
		}

		private InMemoryTokenManager ShortTermUserSessionTokenManager
		{
			get
			{
				var tokenManager =
					(InMemoryTokenManager)_sessionStorage.Load("TwitterShortTermUserSessionTokenManager");

				if (tokenManager == null)
				{
					var consumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"];
					var consumerSecret = ConfigurationManager.AppSettings["twitterConsumerSecret"];

					if (IsTwitterConsumerConfigured)
					{
						tokenManager = new InMemoryTokenManager(consumerKey, consumerSecret);
						_sessionStorage.Store("TwitterShortTermUserSessionTokenManager", tokenManager);
					}
					else
					{
						throw new InvalidOperationException(
							"No Twitter OAuth consumer key and secret could be found in web.config AppSettings.");
					}
				}

				return tokenManager;
			}
		}

		public OutgoingWebResponse StartSignInWithTwitter(Uri callbackUri = null)
		{
			if (callbackUri == null)
			{
				callbackUri = MessagingUtilities.GetRequestUrlFromContext().StripQueryArgumentsWithPrefix("oauth_");
			}

			var request = TwitterSignIn.PrepareRequestUserAuthorization(callbackUri, null, null);

			return TwitterSignIn.Channel.PrepareResponse(request);
		}

		public bool TryFinishSignInWithTwitter(out string screenName, out int userId)
		{
			screenName = null;
			userId = 0;

			var response = TwitterSignIn.ProcessUserAuthorization();

			if (response == null)
			{
				return false;
			}

			screenName = response.ExtraData["screen_name"];
			userId = int.Parse(response.ExtraData["user_id"]);
			return true;
		}
	}
}