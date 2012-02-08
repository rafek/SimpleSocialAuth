using System;
using DotNetOpenAuth.OAuth2;

namespace SimpleSocialAuth.MVC.Consumers
{
  internal class AuthorizationTracker : IClientAuthorizationTracker
  {
    public IAuthorizationState GetAuthorizationState(Uri callbackUrl, string clientState)
    {
      return
        new AuthorizationState
        {
          Callback = callbackUrl
        };
    }
  }
}
