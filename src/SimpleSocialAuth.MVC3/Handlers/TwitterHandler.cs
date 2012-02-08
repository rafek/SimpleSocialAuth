using System;
using System.Collections.Generic;
using System.Web;
using SimpleSocialAuth.MVC.Consumers;

namespace SimpleSocialAuth.MVC.Handlers
{
  public class TwitterHandler : AbstractAuthHandler
  {
    public override string PrepareAuthRequest(HttpRequestBase request, string redirectPath, IDictionary<string, string> parameters = null)
    {
      var callback =
        new Uri(Utils.GetUrlBase(request) + redirectPath);

      return
        TwitterConsumer
          .StartSignInWithTwitter(callback)
          .Headers["Location"];
    }

    public override BasicUserData ProcessAuthRequest(HttpRequestBase request)
    {
      string screenName;
      int userId;

      return
        TwitterConsumer.TryFinishSignInWithTwitter(out screenName, out userId)
          ? new BasicUserData
          {
            UserId = userId.ToString(),
            UserName = screenName
          }
          : null;
    }
  }
}
