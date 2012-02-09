using System;
using System.Configuration;
using System.Web;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;

namespace SimpleSocialAuth.MVC3.Consumers
{
  internal class TwitterConsumer
  {
    public static readonly ServiceProviderDescription SignInWithTwitterServiceDescription =
      new ServiceProviderDescription
      {
        RequestTokenEndpoint =
          new MessageReceivingEndpoint(
            "http://twitter.com/oauth/request_token",
            HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
        UserAuthorizationEndpoint =
          new MessageReceivingEndpoint(
            "http://twitter.com/oauth/authenticate",
            HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
        AccessTokenEndpoint =
          new MessageReceivingEndpoint(
            "http://twitter.com/oauth/access_token",
            HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
        TamperProtectionElements =
          new ITamperProtectionChannelBindingElement[]
            {
              new HmacSha1SigningBindingElement()
            },
      };

    private static WebConsumer signInConsumer;
    private static readonly object signInConsumerInitLock = new object();

    private static bool IsTwitterConsumerConfigured
    {
      get
      {
        return
          !string.IsNullOrEmpty(ConfigurationManager.AppSettings["twitterConsumerKey"]) &&
          !string.IsNullOrEmpty(ConfigurationManager.AppSettings["twitterConsumerSecret"]);
      }
    }

    private static WebConsumer TwitterSignIn
    {
      get
      {
        if (signInConsumer == null)
        {
          lock (signInConsumerInitLock)
          {
            if (signInConsumer == null)
            {
              signInConsumer =
                new WebConsumer(
                  SignInWithTwitterServiceDescription,
                  ShortTermUserSessionTokenManager);
            }
          }
        }

        return signInConsumer;
      }
    }

    private static InMemoryTokenManager ShortTermUserSessionTokenManager
    {
      get
      {
        var store =
          HttpContext.Current.Session;

        var tokenManager =
          (InMemoryTokenManager)store["TwitterShortTermUserSessionTokenManager"];

        if (tokenManager == null)
        {
          string consumerKey =
            ConfigurationManager.AppSettings["twitterConsumerKey"];

          string consumerSecret =
            ConfigurationManager.AppSettings["twitterConsumerSecret"];

          if (IsTwitterConsumerConfigured)
          {
            tokenManager =
              new InMemoryTokenManager(consumerKey, consumerSecret);

            store["TwitterShortTermUserSessionTokenManager"] = tokenManager;
          }
          else
          {
            throw new InvalidOperationException(
              "No Twitter OAuth consumer key and secret could be found in web.config AppSettings.");
          }
        }

        return
          tokenManager;
      }
    }

    public static OutgoingWebResponse StartSignInWithTwitter(Uri callback = null)
    {
      if (callback == null)
      {
        callback =
          MessagingUtilities
            .GetRequestUrlFromContext()
            .StripQueryArgumentsWithPrefix("oauth_");
      }

      var request =
        TwitterSignIn
          .PrepareRequestUserAuthorization(callback, null, null);

      return
        TwitterSignIn
          .Channel
          .PrepareResponse(request);
    }

    public static bool TryFinishSignInWithTwitter(out string screenName, out int userId)
    {
      screenName = null;
      userId = 0;

      var response =
        TwitterSignIn
          .ProcessUserAuthorization();

      if (response == null)
      {
        return false;
      }

      screenName =
        response.ExtraData["screen_name"];

      userId =
        int.Parse(response.ExtraData["user_id"]);

      return true;
    }
  }
}
