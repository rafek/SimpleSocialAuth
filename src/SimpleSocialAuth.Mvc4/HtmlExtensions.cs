using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using HtmlTags;

namespace SimpleSocialAuth.Mvc4
{
	public static class HtmlExtensions
	{
		public static MvcHtmlString RenderAuthWarnings(this HtmlHelper htmlHelper)
		{
			var appSettingsKeys =
				new[]
					{
						"googleAppID", "googleAppSecret",
						"facebookAppID", "facebookAppSecret",
						"twitterConsumerKey", "twitterConsumerSecret"
					};

			var noValueForSetting = appSettingsKeys
					.Any(key => string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]));

			var message = "";

			if (noValueForSetting)
			{
				message = new HtmlTag("p")
						.Attr("style", "color: Red;")
						.Text("Not all key and secrets are filled in a configuration file.")
						.ToHtmlString();
			}

			return new MvcHtmlString(message);
		}

		public static MvcHtmlString AuthButtons(this HtmlHelper htmlHelper)
		{
			var authContainer =
				new HtmlTag("div")
					.Attr("style", "overflow: hidden;")
					.Append("input",
							ht => ht.Id("authType").Attr("name", "authType").Attr("type", "hidden").Attr("value", "1"))
					.Append("a",
							ht => ht
									  .Attr("href", "javascript:auth.signin('twitter')")
									  .AddClasses("simpleAuthButton", "twitter")
									  .Text("twitter"))
					.Append("a",
							ht => ht
									  .Attr("href", "javascript:auth.signin('facebook')")
									  .AddClasses("simpleAuthButton", "facebook")
									  .Text("facebook"))
					.Append("a",
							ht => ht
									  .Attr("href", "javascript:auth.signin('google')")
									  .AddClasses("simpleAuthButton", "google")
									  .Text("google"))
					.ToHtmlString();

			return new MvcHtmlString(authContainer);
		}
	}
}