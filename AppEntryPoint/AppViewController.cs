using System;

using UIKit;
using Homeinns.Business;
using Homeinns.Home;
using Homeinns.Profile;
using Homeinns.Contacts.Controllers;

namespace untitled
{
	public class AppViewController : UITabBarController
	{
		// Declaration
		private HomeViewController homeVC;
		private ContactViewController contactVC;
		private BusinessViewController businessVC;
		private ProfileViewController profileVC;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Initialization
			homeVC = new HomeViewController();
			contactVC = new ContactViewController();
			businessVC = new BusinessViewController();
			profileVC = new ProfileViewController();

			homeVC.TabBarItem = new UITabBarItem("Message", UIImage.FromFile("message_normal.png"), UIImage.FromFile("message_selected.png"));
			contactVC.TabBarItem = new UITabBarItem("Contact", UIImage.FromFile("contact_normal.png"), UIImage.FromFile("contact_selected.png"));
			businessVC.TabBarItem = new UITabBarItem("Business", UIImage.FromFile("application_normal.png"), UIImage.FromFile("application_selected.png"));
			profileVC.TabBarItem = new UITabBarItem("Profile", UIImage.FromFile("setting_normal.png"), UIImage.FromFile("setting_selected.png"));

			ViewControllers = new UIViewController[] {
				homeVC, contactVC, businessVC, profileVC
			};

			ViewControllerSelected += (sender, e) => {
				NavigationItem.Title = e.ViewController.NavigationItem.Title ?? string.Empty;
			};

			//NavigationItem.BackBarButtonItem = new UIBarButtonItem(string.Empty, UIBarButtonItemStyle.Plain, null);

			SetNavigationBar();
		}

		// Set default selected tab
		public override nint SelectedIndex
		{
			get
			{
				return base.SelectedIndex;
			}
			set
			{
				base.SelectedIndex = value;
			}
		}

		// Set navigation bar
		private void SetNavigationBar()
		{
			UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
			if (SelectedIndex == 0)
			{
				NavigationItem.Title = homeVC.NavigationItem.Title;
				UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
			}
			else if (SelectedIndex == 1)
			{
				NavigationItem.Title = contactVC.NavigationItem.Title;
			}
			else if (SelectedIndex == 2)
			{
				NavigationItem.Title = businessVC.NavigationItem.Title;
			}
			else {
				NavigationItem.Title = profileVC.NavigationItem.Title;
			}
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
		}
	}
}
