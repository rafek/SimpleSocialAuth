using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using DotNetOpenAuth.OAuth2;
using Newtonsoft.Json.Linq;
using SimpleSocialAuth.MVC3.Consumers;

namespace SimpleSocialAuth.MVC3.Handlers
{
  public class FacebookHandler : AbstractAuthHandler
  {
    private static readonly FacebookConsumer facebookConsumer =
      new FacebookConsumer
      {
        ClientIdentifier = ConfigurationManager.AppSettings["facebookAppID"],
        ClientSecret = ConfigurationManager.AppSettings["facebookAppSecret"]
      };

    public override string PrepareAuthRequest(HttpRequestBase request, string redirectPath, IDictionary<string, string> parameters = null)
    {
      IAuthorizationState authorization =
        facebookConsumer.ProcessUserAuthorization();

      var callback =
        new Uri(Utils.GetUrlBase(request) + redirectPath);

      if (authorization == null)
      {
        return
          facebookConsumer
            .PrepareRequestUserAuthorization(returnTo: callback)
            .Headers["Location"];
      }

      return null;
    }

    public override BasicUserData ProcessAuthRequest(HttpRequestBase request)
    {
      IAuthorizationState authorization =
        facebookConsumer.ProcessUserAuthorization();

      if (authorization.AccessToken == null)
      {
        return null;
      }

      var graphRequest =
        WebRequest
          .Create("https://graph.facebook.com/me?access_token=" + Uri.EscapeDataString(authorization.AccessToken));

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
  }
}
