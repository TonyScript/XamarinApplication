using System;
using CoreGraphics;
using UIKit;
using Homeinns.Common.Configuration;

namespace Homeinns.Common.Util
{
	/// <summary>
	/// 构建用户界面元素的帮助类
	/// </summary>
	public class UIViewCreator
	{
		UIView _contentView;

		public UIViewCreator(UIView contentView)
		{
			_contentView = contentView;
		}

		#region ToolBar

		public UIImageView CreateToolBarByImageView(bool withBorder = true)
		{
			var toolbar = new UIImageView()
			{
				ClearsContextBeforeDrawing = false,
				AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleWidth,
				BackgroundColor = AppUIStyleSetting.ToolBarColor,
				UserInteractionEnabled = true
			};
			if (withBorder)
			{
				toolbar.Layer.BorderColor = UIColor.LightGray.CGColor;
				toolbar.Layer.BorderWidth = 1;
			}
			_contentView.AddSubview(toolbar);

			return toolbar;
		}

		public UIToolbar CreateToolbar(CGRect r, UIColor color)
		{
			var tb = new UIToolbar(r);
			if (AppUIStyleSetting.IsIos7OrLater)
			{
				tb.BarTintColor = color;
			}
			else {
				tb.TintColor = color;
			}
			tb.Translucent = false;
			tb.BarStyle = UIBarStyle.Black;
			tb.ClearsContextBeforeDrawing = false;
			tb.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleWidth;
			tb.UserInteractionEnabled = true;

			_contentView.Add(tb);
			return tb;
		}

		public UIToolbar CreateToolbar(CGRect r)
		{
			return CreateToolbar(r, AppUIStyleSetting.ToolBarColor);
		}

		#endregion

		#region Button

		public UIButton CreateButton(string title, string backgroudImageNormal = "", string backgroundImageSelected = "")
		{
			var btn = UIButton.FromType(UIButtonType.Custom);
			btn.BackgroundColor = UIColor.Clear;
			btn.ClearsContextBeforeDrawing = false;
			btn.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleLeftMargin;
			btn.SetTitle(title, UIControlState.Normal);
			btn.Font = UIFont.SystemFontOfSize((nfloat)AppUIStyleSetting.FontDetailSize);
			btn.SetTitleColor(AppUIStyleSetting.ButtonTextBlue, UIControlState.Normal);

			if (!string.IsNullOrWhiteSpace(backgroudImageNormal))
				btn.SetBackgroundImage(UIImage.FromFile(backgroudImageNormal), UIControlState.Normal);
			if (!string.IsNullOrWhiteSpace(backgroundImageSelected))
				btn.SetBackgroundImage(UIImage.FromFile(backgroundImageSelected), UIControlState.Selected);

			_contentView.AddSubview(btn);
			return btn;
		}

		public UIButton CreateButton4TableViewCell(UITableViewCell cell, CGRect rect, string text)
		{
			var btn = new UIButton(rect);
			btn.Font = UIFont.SystemFontOfSize((nfloat)AppUIStyleSetting.FontTitleSize);
			btn.BackgroundColor = AppUIStyleSetting.AppBlueColor;
			btn.TintColor = UIColor.White;
			btn.Layer.BorderWidth = 1;
			btn.Layer.BorderColor = AppUIStyleSetting.AppBlueColor.CGColor;
			btn.Layer.CornerRadius = 5;
			btn.SetTitle(text, UIControlState.Normal);
			btn.SetBackgroundImage(ImageUtil.CreateImageWithColor(AppUIStyleSetting.AppBlueColor, (CGRect)btn.Bounds), UIControlState.Normal);
			btn.SetBackgroundImage(ImageUtil.CreateImageWithColor(AppUIStyleSetting.AppBlueColorHighlighted, (CGRect)btn.Bounds), UIControlState.Highlighted);

			cell.AddSubview(btn);

			return btn;

		}

		public UIButton CreateButton(CGRect rect, string text)
		{
			var btn = new UIButton(rect);
			btn.Font = UIFont.SystemFontOfSize((nfloat)AppUIStyleSetting.FontTitleSize);
			btn.BackgroundColor = AppUIStyleSetting.AppBlueColor;
			btn.TintColor = UIColor.White;
			btn.Layer.BorderWidth = 1;
			btn.Layer.BorderColor = AppUIStyleSetting.AppBlueColor.CGColor;
			btn.Layer.CornerRadius = 5;
			btn.SetTitle(text, UIControlState.Normal);
			btn.SetBackgroundImage(ImageUtil.CreateImageWithColor(AppUIStyleSetting.AppBlueColor, (CGRect)btn.Bounds), UIControlState.Normal);
			btn.SetBackgroundImage(ImageUtil.CreateImageWithColor(AppUIStyleSetting.AppBlueColorHighlighted, (CGRect)btn.Bounds), UIControlState.Highlighted);

			_contentView.AddSubview(btn);

			return btn;
		}

		#endregion

		#region Search Bar

		public UISearchBar CreateSearchBar(CGRect r)
		{
			var searchBar = new UISearchBar(r)
			{
				BarStyle = UIBarStyle.Default,
				Placeholder = "搜索",
				AutocapitalizationType = UITextAutocapitalizationType.None,
				AutocorrectionType = UITextAutocorrectionType.No
			};

			if (AppUIStyleSetting.IsIos7OrLater)
				searchBar.BarTintColor = AppUIStyleSetting.ViewControllerColor;
			else
				searchBar.TintColor = AppUIStyleSetting.ViewControllerColor;

			return searchBar;
		}

		#endregion

		#region Table View

		public UITableView CreateTableView(CGRect r)
		{
			var tableView = new UITableView(r);
			tableView.BackgroundView = null;
			tableView.BackgroundColor = AppUIStyleSetting.ViewControllerColor;
			tableView.TableFooterView = new UIView() { BackgroundColor = UIColor.Clear }; //设置uiview  用于遮挡uitableview 没有数据时显示的分割线

			_contentView.AddSubview(tableView);
			return tableView;
		}

		public void SetTableViewStyle(UITableView tv)
		{
			tv.BackgroundView = null;
			tv.BackgroundColor = AppUIStyleSetting.ViewControllerColor;
		}

		public UITableViewCell GetTableViewCell(UITableView tv, string cellId, string text, UIImage image)
		{
			var cell = tv.DequeueReusableCell(cellId);
			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, cellId);
				cell.TextLabel.Text = text;
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				if (image != null)
					cell.ImageView.Image = image;
			}

			return cell;
		}

		public UITableViewCell CreateTableViewCell4Input(UITableView tv, string cellId)
		{
			var cell = new UITableViewCell(UITableViewCellStyle.Default, cellId);

			cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			cell.Layer.BorderColor = AppUIStyleSetting.ViewControllerColor.CGColor;
			cell.Layer.BorderWidth = 0.5F;

			return cell;
		}

		#endregion

		#region Text Box

		public UITextField CreateTextBox4TableViewCell(UITableViewCell cell, CGRect rect, string placeholder, string text)
		{
			var textBox = new UITextField(rect);
			textBox.Placeholder = placeholder;
			//textBox.Layer.BorderWidth = 1F;
			//textBox.Layer.BorderColor = AppUIStyleSetting.ViewControllerColor.CGColor;
			textBox.AdjustsFontSizeToFitWidth = true;
			textBox.Font = UIFont.SystemFontOfSize((nfloat)AppUIStyleSetting.FontTitleSize);
			//设置文字离边框的距离
			textBox.LeftView = new UIView(new CGRect(0, 0, AppUIStyleSetting.PaddingSizeSmall, 0));
			//设置显示模式为永远显示(默认不显示)
			textBox.LeftViewMode = UITextFieldViewMode.Always;
			textBox.Text = text;

			cell.ContentView.AddSubview(textBox);

			return textBox;
		}

		public UITextField CreateIconTextBox4TableViewCell(UITableViewCell cell, CGRect rect, string placeholder, string text, string iconResFileName)
		{
			var txtBox = CreateTextBox4TableViewCell(cell, rect, placeholder, text);
			if (string.IsNullOrWhiteSpace(iconResFileName))
			{
				return txtBox;
			}

			var icon = new UIImageView(new CGRect(0, (rect.Height - 20) / 2, 20, 20));
			icon.Image = UIImage.FromFile(iconResFileName);
			txtBox.LeftView = new UIView(new CGRect(0, 0, 20 + AppUIStyleSetting.PaddingSizeMedium, rect.Height));
			txtBox.LeftView.AddSubview(icon);

			return txtBox;
		}

		#endregion
	}
}

