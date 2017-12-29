using UIKit;

namespace untitled
{
	public class AppNavigationController : UINavigationController
	{

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}

		public AppNavigationController (UIViewController vc) : base (vc)
		{
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			if (VisibleViewController is UIAlertController || VisibleViewController == null) {
				return base.GetSupportedInterfaceOrientations ();
			}
			return VisibleViewController.GetSupportedInterfaceOrientations ();
		}

		public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation ()
		{
			if (VisibleViewController is UIAlertController || VisibleViewController == null) {
				return base.PreferredInterfaceOrientationForPresentation ();
			}
			return VisibleViewController.PreferredInterfaceOrientationForPresentation ();
		}
	}
}
