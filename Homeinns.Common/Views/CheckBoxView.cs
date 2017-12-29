using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace Homeinns.Common.Views
{
	/// <summary>
	/// CheckBox Control
	/// </summary>
	public class CheckBoxView
		: UIButton
	{
		private static readonly UIImage _imageCheckOn;
		private static readonly UIImage _imageCheckOff;

		static CheckBoxView()
		{
			_imageCheckOn = UIImage.FromFile("ic_checkbox_on.png");
			_imageCheckOff = UIImage.FromFile("ic_checkbox_off.png");
		}

		private UIImageView _iconCheckBox;
		private bool _isChecked = false;

		public bool IsChecked
		{
			get { return _isChecked; }
			set
			{
				if (value)
				{
					_iconCheckBox.Image = _imageCheckOn;
				}
				else {
					_iconCheckBox.Image = _imageCheckOff;
				}

				_isChecked = value;
			}
		}

		public CheckBoxView(CGRect rect, string title, bool isChecked = false)
			: base(rect)
		{
			_iconCheckBox = new UIImageView(new CGRect(0, 0, 20, 20));
			_iconCheckBox.Image = _imageCheckOff;
			SetTitle(title, UIControlState.Normal);
			SetTitleColor(UIColor.Black, UIControlState.Normal);
			Font = UIFont.SystemFontOfSize(16);
			AddSubview(_iconCheckBox);

			IsChecked = isChecked;
			this.TouchUpInside += (s, e) =>
			{
				IsChecked = !IsChecked;
			};
		}
	}
}

