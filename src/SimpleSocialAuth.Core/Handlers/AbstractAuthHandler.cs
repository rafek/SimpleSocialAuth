namespace SimpleSocialAuth.Core.Handlers
{
    public interface IAuthenticationHandler
    {
        string PrepareAuthRequest(PrepareAuthenticationContext context);
        BasicUserData ProcessAuthRequest(ProcessAuthenticationContext context);
    }
}