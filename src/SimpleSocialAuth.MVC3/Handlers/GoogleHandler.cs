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
  public class GoogleHandler : AbstractAuthHandler
  {
    private static readonly GoogleConsumer googleConsumer =
      new GoogleConsumer
      {
        ClientIdentifier = ConfigurationManager.AppSettings["googleAppID"],
        ClientSecret = ConfigurationManager.AppSettings["googleAppSecret"]
      };

    public override string PrepareAuthRequest(HttpRequestBase request, string redirectPath, IDictionary<string, string> parameters = null)
    {
      IAuthorizationState authorization =
        googleConsumer.ProcessUserAuthorization();

      var callback =
        new Uri(Utils.GetUrlBase(request) + redirectPath);

      if (authorization == null)
      {
        return
          googleConsumer
          .PrepareRequestUserAuthorization(returnTo: callback, scopes: new[] { "https://www.googleapis.com/auth/userinfo.profile" })
            .Headers["Location"];
      }

      return null;
    }

    public override BasicUserData ProcessAuthRequest(HttpRequestBase request)
    {
      IAuthorizationState authorization =
        googleConsumer.ProcessUserAuthorization();

      if (authorization.AccessToken == null)
      {
        return null;
      }

      var graphRequest =
        WebRequest
          .Create("https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + Uri.EscapeDataString(authorization.AccessToken));

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
  }
}
