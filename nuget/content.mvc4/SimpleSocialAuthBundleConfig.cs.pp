using System.Web.Optimization;

namespace namespace $rootnamespace$.App_Start
{
	public class SimpleSocialAuthBundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/SimpleSocialAuth")
				.Include("~/Scripts/SimpleSocialAuth.js"));

			bundles.Add(new StyleBundle("~/content/SimpleSocialAuth").Include(
				"~/Content/SimpleSocialAuth.js"));
		}
	}
}