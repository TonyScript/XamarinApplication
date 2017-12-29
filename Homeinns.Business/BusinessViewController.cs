using UIKit;
using Homeinns.Common.Configuration;

namespace Homeinns.Business
{
	public class BusinessViewController : UIViewController
	{

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			NavigationItem.Title = "Business";
			View.BackgroundColor = AppUIStyleSetting.ViewControllerColor;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
