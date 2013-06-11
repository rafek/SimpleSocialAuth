using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Security;
using SimpleSocialAuth.Core;
using SimpleSocialAuth.Core.Handlers;

namespace $rootnamespace$.Controllers
{
	/// <summary>
	/// Simple scaffolding class for handling OAuth-based authentication.
	/// Many scenarios will require that this controller be extended or logic from this
	/// controller moved into other classes.
	/// </summary>
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

		[HttpPost]
		public ActionResult Authenticate(string authType)
		{
			try
			{
				var authHandler = AuthHandlerFactory.Create(authType);
				var authContext = new PrepareAuthenticationContext(
					CurrentContextSession.Instance,
					Request.Url,
					Url.Action("DoAuth", new { authType }));
				string redirectUrl = authHandler.PrepareAuthRequest(authContext);
				return Redirect(redirectUrl);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error calling OAuth provider: " + ex);
				TempData["authError"] = ex.Message;
				return RedirectToAction("LogIn");
			}
		}

		public ActionResult DoAuth(string authType)
		{
			try
			{
				var authHandler = AuthHandlerFactory.Create(authType);
				var authContext = new ProcessAuthenticationContext(CurrentContextSession.Instance, Request.Url);
				var userData = authHandler.ProcessAuthRequest(authContext);

				if (userData == null)
				{
					TempData["authError"] = "Authentication has failed.";

					return RedirectToAction("LogIn");
				}

				// NOTE: this is just simple usage of setting AuthCookie
				// TODO: You may also want to check to see if this user exists in your database, create an entry if they don't, etc.
				FormsAuthentication.SetAuthCookie(userData.UserName, true);

				return Session["ReturnUrl"] != null
					       ? (ActionResult) Redirect((string) Session["ReturnUrl"])
					       : RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error responding to OAuth request: " + ex);
				TempData["authError"] = ex.Message;
				return RedirectToAction("LogIn");
			}
		}
	}
}
