using System;
using CoreGraphics;
using Foundation;
using UIKit;
using MonoTouch.Dialog;
using Homeinns.Common.Configuration;

namespace Homeinns.Contacts.Views
{
	/// <summary>
	/// 用于Monotouch Dialog中的按钮的Element
	/// </summary>
	public class ButtonElement : Element, IElementSizing
	{
		GlassButton _button;
		static int count;
		NSString key;
		protected UIView View;
		public CellFlags Flags;

		public enum CellFlags
		{
			Transparent = 1,
			DisableSelection = 2
		}

		public event Action Tapped;

		public ButtonElement(string caption)
			: base(caption)
		{
			this.Flags = CellFlags.Transparent;
			key = new NSString("BTButtonElement" + count++);
		}

		private void CreateButton(UITableView tv)
		{
			if (_button != null)
				return;

			this._button = new GlassButton(new CGRect(AppUIStyleSetting.PaddingSizeMedium, 5, tv.Bounds.Width - AppUIStyleSetting.PaddingSizeMedium * 2, 50))
			{
				Font = UIFont.BoldSystemFontOfSize((nfloat)16),
				NormalColor = AppUIStyleSetting.AppBlueColor,
				HighlightedColor = AppUIStyleSetting.AppBlueColorHighlighted
			};
			_button.SetTitle(this.Caption, UIControlState.Normal);
			_button.Tapped += delegate (GlassButton obj)
			{
				if (Tapped != null)
				{
					Tapped();
				}
			};
		}

		public override UITableViewCell GetCell(UITableView tv)
		{
			var cell = tv.DequeueReusableCell(key);
			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, key);
				cell.SelectionStyle = UITableViewCellSelectionStyle.None;
				CreateButton(tv);
				if ((Flags & CellFlags.Transparent) != 0)
				{
					cell.BackgroundColor = UIColor.Clear;

					cell.BackgroundView = new UIView(CGRect.Empty)
					{
						BackgroundColor = UIColor.Clear
					};
				}
				if ((Flags & CellFlags.DisableSelection) != 0)
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;

				cell.ContentView.AddSubview(_button);

				cell.ContentView.BackgroundColor = UIColor.Clear;
				cell.ContentView.Superview.BackgroundColor = UIColor.Clear;
				cell.Layer.BorderWidth = 0F;
				cell.Layer.BorderColor = AppUIStyleSetting.ViewControllerColor.CGColor;
			}
			return cell;
		}

		public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath)
		{
			return (nfloat)60;
		}
	}

	public class GlassButton : UIButton
	{
		bool pressed;

		public UIColor NormalColor, HighlightedColor, DisabledColor;

		/// <summary>
		/// Invoked when the user touches 
		/// </summary>
		public event Action<GlassButton> Tapped;

		/// <summary>
		/// Creates a new instance of the GlassButton using the specified dimensions
		/// </summary>
		public GlassButton(CGRect frame)
			: base(frame)
		{
			NormalColor = new UIColor(0.55f, 0.04f, 0.02f, 1);
			//NormalColor = new UIColor (0.04f, 0.55f, 0.02f, 1);
			HighlightedColor = UIColor.Black;
			DisabledColor = UIColor.Gray;

		}

		public void SetGreen()
		{
			NormalColor = new UIColor(0.04f, 0.55f, 0.02f, 1);
			HighlightedColor = UIColor.Black;
			DisabledColor = UIColor.Gray;
		}

		/// <summary>
		/// Whether the button is rendered enabled or not.
		/// </summary>
		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
				SetNeedsDisplay();
			}
		}

		public override bool BeginTracking(UITouch uitouch, UIEvent uievent)
		{
			SetNeedsDisplay();
			pressed = true;
			return base.BeginTracking(uitouch, uievent);
		}

		public override void EndTracking(UITouch uitouch, UIEvent uievent)
		{
			if (pressed && Enabled)
			{
				if (Tapped != null)
					Tapped(this);
			}
			pressed = false;
			SetNeedsDisplay();
			base.EndTracking(uitouch, uievent);
		}

		public override bool ContinueTracking(UITouch uitouch, UIEvent uievent)
		{
			var touch = uievent.AllTouches.AnyObject as UITouch;
			if (Bounds.Contains((CGPoint)touch.LocationInView((UIView)this)))
				pressed = true;
			else
				pressed = false;
			return base.ContinueTracking(uitouch, uievent);
		}

		public override void Draw(CGRect rect)
		{
			var context = UIGraphics.GetCurrentContext();
			var bounds = Bounds;
			SetTitleColor(NormalColor == UIColor.White ? UIColor.Black : UIColor.White, UIControlState.Normal);

			UIColor background = Enabled ? pressed ? HighlightedColor : NormalColor : DisabledColor;
			float alpha = 1;

			CGPath container;
			container = MakeRoundedRectPath(bounds, 14);
			context.AddPath(container);
			context.Clip();

			using (var cs = CGColorSpace.CreateDeviceRGB())
			{
				var topCenter = new CGPoint((float)bounds.GetMidX(), 0);
				var midCenter = new CGPoint((float)bounds.GetMidX(), (float)bounds.GetMidY());
				var bottomCenter = new CGPoint((float)bounds.GetMidX(), (float)bounds.GetMaxY());

				using (var gradient = new CGGradient(cs, new nfloat[] {
					0.23f,
					0.23f,
					0.23f,
					alpha,
					0.47f,
					0.47f,
					0.47f,
					alpha
				}, new nfloat[] {
					0,
					1
				}))
				{
					//context.DrawLinearGradient (gradient, topCenter, bottomCenter, 0);
				}

				container = MakeRoundedRectPath((CGRect)bounds.Inset(1, 1), 13);
				context.AddPath(container);
				context.Clip();
				using (var gradient = new CGGradient(cs, new nfloat[] {
					0.05f,
					0.05f,
					0.05f,
					alpha,
					0.15f,
					0.15f,
					0.15f,
					alpha
				}, new nfloat[] {
					0,
					1
				}))
				{
					//context.DrawLinearGradient (gradient, topCenter, bottomCenter, 0);
				}

				var nb = (CGRect)bounds.Inset(4, 4);
				container = MakeRoundedRectPath(nb, 10);
				context.AddPath(container);
				context.Clip();

				background.SetFill();
				context.FillRect((CGRect)nb);

				using (var gradient = new CGGradient(cs, new nfloat[] { 1, 1, 1, .35f, 1, 1, 1, 0.06f }, new nfloat[] {
					0,
					1
				}))
				{

					context.DrawLinearGradient((CGGradient)gradient, (CGPoint)topCenter, (CGPoint)midCenter, (CGGradientDrawingOptions)0);
				}
				context.SetLineWidth((nfloat)1);
				context.AddPath(container);
				context.ReplacePathWithStrokedPath();
				context.Clip();

				using (var gradient = new CGGradient(cs, new nfloat[] { 1, 1, 1, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f }, new nfloat[] {
					0,
					1
				}))
				{
					context.DrawLinearGradient((CGGradient)gradient, (CGPoint)topCenter, (CGPoint)bottomCenter, (CGGradientDrawingOptions)0);
				}
			}
		}

		public CGPath MakeRoundedRectPath(CGRect rect, float radius)
		{
			var minx = rect.Left;
			var midx = rect.Left + (rect.Width) / 2;
			var maxx = rect.Right;
			var miny = rect.Top;
			var midy = rect.Y + rect.Size.Height / 2;
			var maxy = rect.Bottom;

			var path = new CGPath();
			path.MoveToPoint(minx, midy);
			path.AddArcToPoint(minx, miny, midx, miny, radius);
			path.AddArcToPoint(maxx, miny, maxx, midy, radius);
			path.AddArcToPoint(maxx, maxy, midx, maxy, radius);
			path.AddArcToPoint(minx, maxy, minx, midy, radius);
			path.CloseSubpath();

			return path;
		}
	}
}

