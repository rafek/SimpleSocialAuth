using System;
using DotNetOpenAuth.OAuth2;

namespace SimpleSocialAuth.MVC3.Consumers
{
  internal class GoogleConsumer : WebServerClient
  {
    private static readonly AuthorizationServerDescription GoogleDescription =
      new AuthorizationServerDescription
      {
        TokenEndpoint = new Uri("https://accounts.google.com/o/oauth2/token?grant_type=authorization_code"),
        AuthorizationEndpoint = new Uri("https://accounts.google.com/o/oauth2/auth")
      };

    public GoogleConsumer()
      : base(GoogleDescription)
    {
      AuthorizationTracker =
        new AuthorizationTracker();
    }
  }
}
