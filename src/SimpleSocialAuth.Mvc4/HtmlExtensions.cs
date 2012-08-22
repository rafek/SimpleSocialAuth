using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using HtmlTags;
using SimpleSocialAuth.MVC3;

namespace SimpleSocialAuth.Mvc4
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString RenderAuthScript(this HtmlHelper htmlHelper)
        {
            return
                new MvcHtmlString(
                    new HtmlTag("script")
                        .Attr("src", WebResource(typeof (Utils), "SimpleSocialAuth.MVC3.Scripts.jquery.auth.js"))
                        .Attr("type", @"text/javascript")
                        .ToHtmlString());
        }

        public static MvcHtmlString RenderAuthStylesheet(this HtmlHelper htmlHelper)
        {
            var inlineStyles =
                new HtmlTag("style")
                    .Attr("type", "text/css")
                    .Text(@".largeBtn { background-image: url(" +
                          WebResource(typeof (Utils), "SimpleSocialAuth.MVC3.Images.auth_logos.png") + "); }");

            var includedStyles =
                new HtmlTag("link")
                    .Attr("rel", "stylesheet")
                    .Attr("href", WebResource(typeof (Utils), "SimpleSocialAuth.MVC3.Stylesheets.auth.css"))
                    .Attr("type", @"text/css");

            return
                new MvcHtmlString(
                    inlineStyles
                        .After(includedStyles)
                        .ToHtmlString());
        }

        public static MvcHtmlString RenderAuthWarnings(this HtmlHelper htmlHelper)
        {
            var appSettingsKeys =
                new[]
                    {
                        "googleAppID", "googleAppSecret",
                        "facebookAppID", "facebookAppSecret",
                        "twitterConsumerKey", "twitterConsumerSecret"
                    };

            var noValueForSetting =
                appSettingsKeys
                    .Any(key => string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]));

            var message = "";

            if (noValueForSetting)
            {
                message =
                    new HtmlTag("p")
                        .Attr("style", "color: Red;")
                        .Text("Not all key and secrets are filled in a configuration file.")
                        .ToHtmlString();
            }

            return
                new MvcHtmlString(message);
        }

        private static string WebResource(Type type, string resourcePath)
        {
            var page = new Page();

            return
                page.ClientScript.GetWebResourceUrl(type, resourcePath);
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
                                      .AddClasses("largeBtn", "twitter")
                                      .Text("twitter"))
                    .Append("a",
                            ht => ht
                                      .Attr("href", "javascript:auth.signin('facebook')")
                                      .AddClasses("largeBtn", "facebook")
                                      .Text("facebook"))
                    .Append("a",
                            ht => ht
                                      .Attr("href", "javascript:auth.signin('google')")
                                      .AddClasses("largeBtn", "google")
                                      .Text("google"))
                    .ToHtmlString();

            return new MvcHtmlString(authContainer);
        }
    }
}