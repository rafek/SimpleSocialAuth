using System.Web.Optimization;

namespace $rootnamespace$.App_Start
{
	public class SimpleSocialAuthBundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/scripts/simplesocialauth")
				.Include("~/Scripts/simplesocialauth.js"));

			bundles.Add(new StyleBundle("~/content/simplesocialauth").Include(
				"~/Content/simplesocialauth.css"));
		}
	}
}