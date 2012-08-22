namespace SimpleSocialAuth.Core
{
    public static class Utils
    {
        public static string GetUrlBase(IHttpRequest request)
        {
            if (request.Url == null)
            {
                return null;
            }

            return
                request.Url.Scheme + "://" +
                request.Url.Authority;
        }
    }
}