using System;
using SimpleSocialAuth.MVC3.Handlers;

namespace SimpleSocialAuth.MVC3
{
  public class AuthHandlerFactory
  {
    public static AbstractAuthHandler CreateAuthHandler(AuthType authType)
    {
      switch (authType)
      {
        case AuthType.Google:
          return
            new GoogleHandler();
        case AuthType.Facebook:
          return
            new FacebookHandler();
        case AuthType.Twitter:
          return
            new TwitterHandler();
        default:
          throw new ArgumentOutOfRangeException("authType");
      }
    }
  }
}
