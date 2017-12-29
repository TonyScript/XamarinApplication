using UIKit;
using Homeinns.Common.Configuration;
using System;
using Foundation;
using CoreGraphics;
using Homeinns.Common.Util;

namespace Homeinns.Home
{
	public class HomeViewController : UIViewController
	{
		UITableView _myTableView;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			NavigationItem.Title = "Home";
			View.BackgroundColor = AppUIStyleSetting.ViewControllerColor;

			var creator = new UIViewCreator(View);

			_myTableView = creator.CreateTableView(new CGRect(View.Bounds.X, View.Bounds.Y,
															View.Frame.Width, View.Frame.Height - AppUIStyleSetting.StatusBarHeight - AppUIStyleSetting.NavigationBarHeight - AppUIStyleSetting.TabBarHeight));

			string[] tableItems = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
			_myTableView.Source = new TableSource(tableItems);

			View.AddSubview(_myTableView);
		}

		public class TableSource : UITableViewSource
		{

			string[] TableItems;
			public TableSource(string[] items)
			{
				TableItems = items;
			}
			/// <summary>
			/// 设置section数目
			/// </summary>

			public override nint NumberOfSections(UITableView tableView)
			{
				return TableItems.Length;
			}

			/// <summary>
			/// 设置每个section行数
			/// </summary>
			public override nint RowsInSection(UITableView tableview, nint section)
			{
				if (section == 0)
					return 5;
				else if ((int)section == 1)
					return 3;
				else
					return 3;
			}

			/// <summary>
			/// 设置tableView内容
			/// </summary>
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				const string cellIdentifier = "TableViewCellUserName";
				var cell = tableView.DequeueReusableCell(cellIdentifier);
				if (cell == null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
					cell.TextLabel.Text = TableItems[indexPath.Row];
					cell.ImageView.Image = UIImage.FromFile("contact_default_avatar.png");
				}

				return cell;
			}

			public override string TitleForHeader(UITableView tableView, nint section)
			{
				return "TitleForHeader";
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				UIAlertController selectAC = UIAlertController.Create("Row Selected", TableItems[indexPath.Row], UIAlertControllerStyle.Alert);
				selectAC.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
				HomeViewController homeVC = new HomeViewController();
				homeVC.PresentViewController(selectAC, true, null);

				tableView.DeselectRow(indexPath, true);
			}


		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
		}
	}
}
