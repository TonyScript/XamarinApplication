using UIKit;
using CoreGraphics;

namespace Homeinns.Common.Views
{
	/// <summary>
	/// IconButtonView Control
	/// </summary>
	public class IconButtonView
		: UIButton
	{
		public IconButtonView(CGRect rect, string title, string iconResFileName)
			: base(rect)
		{
			this.SetImage(UIImage.FromFile(iconResFileName), UIControlState.Normal);
			SetTitle(title, UIControlState.Normal);
			SetTitleColor(UIColor.Black, UIControlState.Normal);
			Font = UIFont.SystemFontOfSize(16F);
		}
	}
}