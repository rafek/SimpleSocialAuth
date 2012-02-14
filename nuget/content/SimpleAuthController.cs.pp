using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SimpleSocialAuth.MVC3;

namespace $rootnamespace$.Controllers
{
  public class SimpleAuthController : Controller
  {
    public ActionResult LogIn()
    {
	  if (Request.IsAuthenticated) 
	  {
	    return
          RedirectToAction("Index", "Home");		
	  }
	
      Session["ReturnUrl"] =
        Request.QueryString["returnUrl"];

      return View();
    }

    // TODO: HI Errors handling
    [HttpPost]
    public ActionResult Authenticate(AuthType authType)
    {
      var authHandler =
        AuthHandlerFactory.CreateAuthHandler(authType);

      string redirectUrl =
        authHandler
          .PrepareAuthRequest(
            Request,
            Url.Action("DoAuth", new { authType = (int)authType }));

      return
        Redirect(redirectUrl);
    }

    public ActionResult DoAuth(AuthType authType)
    {
      var authHandler =
        AuthHandlerFactory.CreateAuthHandler(authType);

      var userData = 
        authHandler
          .ProcessAuthRequest(Request as HttpRequestWrapper);

      if (userData == null)
      {
        TempData["authError"] =
          "Authentication has failed.";

        return
          RedirectToAction("LogIn");
      }

	  // TODO: Here you can check if such user exists in your database, etc.
	  
	  // NOTE: this is just simple usage of setting AuthCookie
      FormsAuthentication.SetAuthCookie(userData.UserName, true);

      return 
        Session["ReturnUrl"] != null
        ? (ActionResult) Redirect((string) Session["ReturnUrl"])
        : RedirectToAction("Index", "Home");
    }
  }
}
