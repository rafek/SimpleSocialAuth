using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SimpleSocialAuth.Core
{
    public class CurrentContextSession : ISessionStorage
    {

        private static CurrentContextSession _instance;
        public static CurrentContextSession Instance
        {
            get { return _instance ?? (_instance = new CurrentContextSession()); }
        }
       
        public object Load(string key)
        {
            return HttpContext.Current.Session[key];
        }

        public void Store(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }
    }
}
