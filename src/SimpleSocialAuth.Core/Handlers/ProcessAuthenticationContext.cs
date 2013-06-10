using System;

namespace SimpleSocialAuth.Core.Handlers
{
    public class ProcessAuthenticationContext
    {
        public ProcessAuthenticationContext(ISessionStorage sessionStorage, Uri requestUri)
        {
            RequestUri = requestUri;
            SessionStorage = sessionStorage;
        }

        public Uri RequestUri { get; private set; }
        public ISessionStorage SessionStorage { get; private set; }
    }
}