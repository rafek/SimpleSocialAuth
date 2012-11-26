using System.Web;
using SimpleSocialAuth.Core;

namespace SimpleSocialAuth.Mvc4
{
    /// <summary>
    /// Uses <c>HttpContext.Current</c> to get the session. Use <c>CurrentContextSession.Instance</c> to access this class
    /// </summary>
    public class CurrentContextSession : ISessionStorage
    {
        private static readonly CurrentContextSession _instance = new CurrentContextSession();

        /// <summary>
        /// Singleton
        /// </summary>
        private CurrentContextSession()
        {
        }

        public static CurrentContextSession Instance
        {
            get { return _instance; }
        }

        #region ISessionStorage Members

        public object Load(string key)
        {
            return HttpContext.Current.Session[key];
        }

        public void Store(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        #endregion
    }
}