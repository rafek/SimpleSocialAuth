using System;
using System.Collections.Generic;

namespace SimpleSocialAuth.Core.Handlers
{
	public class PrepareAuthenticationContext
	{
		public PrepareAuthenticationContext(ISessionStorage sessionStorage, Uri requestUri, string redirectPath, IDictionary<string, string> parameters)
		{
			RequestUri = requestUri;
			SessionStorage = sessionStorage;
			RedirectPath = redirectPath;
			Parameters = parameters;
		}

		public PrepareAuthenticationContext(ISessionStorage sessionStorage, Uri requestUri, string redirectPath)
		{
			RequestUri = requestUri;
			SessionStorage = sessionStorage;
			RedirectPath = redirectPath;
		}

		public Uri RequestUri { get; private set; }
		public ISessionStorage SessionStorage { get; private set; }
		public string RedirectPath { get; private set; }
		public IDictionary<string, string> Parameters { get; private set; }
	}
}