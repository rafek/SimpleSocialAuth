namespace SimpleSocialAuth.Core.Handlers
{
    public class ProcessAuthenticationContext
    {
        public ProcessAuthenticationContext(IHttpRequest request, ISessionStorage sessionStorage)
        {
            Request = request;
            SessionStorage = sessionStorage;
        }

        public IHttpRequest Request { get; private set; }
        public ISessionStorage SessionStorage { get; private set; }
    }
}