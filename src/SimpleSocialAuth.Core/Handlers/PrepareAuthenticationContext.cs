using System.Collections.Generic;

namespace SimpleSocialAuth.Core.Handlers
{
    public class PrepareAuthenticationContext
    {
        public PrepareAuthenticationContext(IHttpRequest request, ISessionStorage sessionStorage, string redirectPath,
                                            IDictionary<string, string> parameters)
        {
            Request = request;
            SessionStorage = sessionStorage;
            RedirectPath = redirectPath;
            Parameters = parameters;
        }

        public PrepareAuthenticationContext(IHttpRequest request, ISessionStorage sessionStorage, string redirectPath)
        {
            Request = request;
            SessionStorage = sessionStorage;
            RedirectPath = redirectPath;
        }

        public IHttpRequest Request { get; private set; }
        public ISessionStorage SessionStorage { get; private set; }
        public string RedirectPath { get; private set; }
        public IDictionary<string, string> Parameters { get; private set; }
    }
}