using System;
using CoreGraphics;
using UIKit;
using CoreLocation;

namespace Homeinns.Common.Configuration
{
    /// <summary>
    ///   Global UI Style Setting
    /// </summary>
    public static class AppUIStyleSetting
    {
        public static readonly float TabBarHeight = 49;

        public static readonly float NavigationBarHeight = 44;
        public static readonly float SearchBarHeight = 44;
        public static readonly float ToolBarHeight = 49;

        public static readonly float NormalButtonHeight = 40;
        public static readonly float NormalButtonWidth = 80;

        public static readonly float PaddingSizeSmall = 5;
        public static readonly float PaddingSizeMedium = 10;
        public static readonly float PaddingSizeLarge = 20;

        public static readonly float HeadIconSizeSmall = 40;
        public static readonly float HeadIconSizeMedium = 45;

        public static readonly float FontTitleSize = 16F;
        public static readonly float FontDetailSize = 14F;


        public static readonly float HeightTableViewRowDefault = 45F;
        public static readonly float HeightTableViewHeaderDefault = 30F;

        public static readonly CGSize SizeTableViewIconDefault = new CGSize(20, 20);

        public static readonly float HeightTextBox = 60F;
        public static readonly float HeightButton = 40F;
        public static readonly float CornerRadiusSize = 5F;
        public static readonly float BorderSize = 0.5F;
        //TableView的行的默认高度

        public static nfloat ScreenWidth;

        public static readonly UIColor ButtonTextBlue = UIColor.FromRGBA((nfloat)0F, (nfloat)0.478431F, (nfloat)1.0F, (nfloat)1.0F);
        public static readonly UIColor ButtonBackgroundColor = UIColor.FromRGB(69, 190, 228);
        public static readonly UIColor ViewControllerColor = UIColor.FromRGB(239, 239, 244);
        public static readonly UIColor ToolBarColor = UIColor.FromRGB(248, 248, 248);
        public static readonly UIColor NavigationBarColor = UIColor.FromRGB(249, 249, 249);
        public static readonly UIColor AppBlueColor = UIColor.FromRGB(0, 153, 255);
        public static readonly UIColor AppBlueColorHighlighted = UIColor.FromRGB(0, 119, 221);

        private static readonly CLLocationManager _locationManager = new CLLocationManager();

        public static void Initialize(UINavigationController nav)
        {
            ScreenWidth = UIScreen.MainScreen.Bounds.Width;

            if (IsIos7OrLater) {
                UINavigationBar.Appearance.BarTintColor = AppBlueColor;
                UINavigationBar.Appearance.TintColor = UIColor.White;
                nav.NavigationBar.Translucent = false;
            } else {
                UINavigationBar.Appearance.TintColor = AppBlueColor;
            }
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes() {
                TextColor = UIColor.White,
                TextShadowColor = UIColor.White
            });
           
            nav.NavigationBar.BarStyle = UIBarStyle.Default;
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);


        }

        public static void RequestAlwaysAuthorizationOfLocationManager()
        {
            if (AppUIStyleSetting.IsIos8OrLater) {
                _locationManager.RequestAlwaysAuthorization();
            }   
        }

        public static bool IsIos7OrLater {
            get {
                return UIDevice.CurrentDevice.CheckSystemVersion(7, 0);
            }
        }

        public static bool IsIos8OrLater {
            get {
                return UIDevice.CurrentDevice.CheckSystemVersion(8, 0);
            }
        }

        public static float StatusBarHeight {
            get {
                if (IsIos7OrLater)
                    return 20F;
                else
                    return 0F;
            }
        }

        public static UIStatusBarStyle BarStyle = UIStatusBarStyle.LightContent;
    }
}
