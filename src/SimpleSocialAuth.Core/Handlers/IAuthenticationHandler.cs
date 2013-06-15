using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SimpleSocialAuth.Core.Handlers
{
    public interface IAuthenticationHandler
    {
        string PrepareAuthRequest(PrepareAuthenticationContext context);
        BasicUserData ProcessAuthRequest(ProcessAuthenticationContext context);
    }
}