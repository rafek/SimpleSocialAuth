using System;

namespace SimpleSocialAuth.Core
{
	public static class Utils
	{
		public static string GetUrlBase(Uri uri)
		{
			if (uri == null)
			{
				return null;
			}

			return uri.Scheme + "://" + uri.Authority;
		}
	}
}