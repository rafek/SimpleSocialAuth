using System;
using SimpleSocialAuth.MVC.Handlers;

namespace SimpleSocialAuth.MVC
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
