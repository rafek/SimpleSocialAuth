using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SimpleSocialAuth.Core;
using SimpleSocialAuth.Core.Handlers;
using SimpleSocialAuth.Mvc4;

namespace $rootnamespace$.Controllers
{
	public class SimpleAuthController : Controller
	{
		public ActionResult LogIn()
		{
			if (Request.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}

			Session["ReturnUrl"] = Request.QueryString["returnUrl"];

			return View();
		}

		// TODO: HI Errors handling
		[HttpPost]
		public ActionResult Authenticate(string authType)
		{
			var authHandler = AuthHandlerFactory.Create(authType);
			var authContext = new PrepareAuthenticationContext(
				CurrentContextSession.Instance, 
				Request.Url, 
				(string)Session["ReturnUrl"]);
			string redirectUrl = authHandler.PrepareAuthRequest(authContext);
			return Redirect(redirectUrl);
		}

		public ActionResult DoAuth(string authType)
		{
			var authHandler = AuthHandlerFactory.Create(authType);
			var authContext = new ProcessAuthenticationContext(CurrentContextSession.Instance, Request.Url);
			var userData = authHandler.ProcessAuthRequest(authContext);

			if (userData == null)
			{
				TempData["authError"] = "Authentication has failed.";

				return RedirectToAction("LogIn");
			}

			// TODO: Here you can check if such user exists in your database, etc.

			// NOTE: this is just simple usage of setting AuthCookie
			FormsAuthentication.SetAuthCookie(userData.UserName, true);

			return Session["ReturnUrl"] != null
			  ? (ActionResult)Redirect((string)Session["ReturnUrl"])
			  : RedirectToAction("Index", "Home");
		}
	}
}
