using System;
using DotNetOpenAuth.OAuth2;

namespace SimpleSocialAuth.Core.Consumers
{
    internal class AuthorizationTracker : IClientAuthorizationTracker
    {
        #region IClientAuthorizationTracker Members

        public IAuthorizationState GetAuthorizationState(Uri callbackUrl, string clientState)
        {
            return
                new AuthorizationState
                    {
                        Callback = callbackUrl
                    };
        }

        #endregion
    }
}