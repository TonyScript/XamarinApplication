using System;
using Homeinns.Common.Configuration;
using UIKit;

namespace Homeinns.Profile
{
	public class ProfileViewController : UIViewController
	{

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			NavigationItem.Title = "Profile";
			View.BackgroundColor = AppUIStyleSetting.ViewControllerColor;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
