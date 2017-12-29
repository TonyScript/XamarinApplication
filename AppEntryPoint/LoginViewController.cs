
using CoreGraphics;
using Foundation;
using System;
using System.IO;
using System.Threading.Tasks;
using UIKit;
using Homeinns.Common.Util;
using Homeinns.Common.Configuration;
using Homeinns.Common.Views;
using Homeinns.Common.Service;
using Homeinns.Common.ViewModel;
using Homeinns.Profile.Service;
using Homeinns.Contacts.Service;
using Homeinns.Application.Service;
using System.Diagnostics;

namespace untitled
{
	/// <summary>
	/// 登录页面
	/// </summary>
	public class LoginViewController : UIViewController
	{
		private UITableView _tableView;
		UIWebView _webView;
		private UIViewCreator _creator;
		private NSObject obs1, obs2;
		private nfloat _logoHeight;
		private UIImage _logo = UIImage.FromFile("login_top.png");


		/// <summary>
		/// 页面加载的时候
		/// </summary>
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
			_logoHeight = (View.Bounds.Width / _logo.Size.Width) * _logo.Size.Height;

			_creator = new UIViewCreator(View);
			_tableView = _creator.CreateTableView(View.Bounds);
			_tableView.BackgroundColor = UIColor.White;
			_tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			_tableView.ScrollEnabled = false;
		}

		/// <summary>
		/// 页面将要出现的时候执行
		/// </summary>
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			_tableView.Source = new Source(this);
			_tableView.ReloadData();

			obs1 = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, delegate (NSNotification n)
			{
				var duration = UIKeyboard.AnimationDurationFromNotification(n);
				UIView.BeginAnimations("ResizeForKeyboard");
				UIView.SetAnimationDuration(duration);
				var contentInsets = new UIEdgeInsets(-(_logoHeight - 15), 0, 0, 0);
				_tableView.ContentInset = contentInsets;
				_tableView.ScrollIndicatorInsets = contentInsets;
				UIView.CommitAnimations();
			});

			obs2 = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, delegate (NSNotification n)
			{
				var duration = UIKeyboard.AnimationDurationFromNotification(n);
				UIView.BeginAnimations("ResizeForKeyboard");
				UIView.SetAnimationDuration(duration);
				var contentInsets = new UIEdgeInsets(0, 0, 0, 0);
				_tableView.ContentInset = contentInsets;
				_tableView.ScrollIndicatorInsets = contentInsets;
				UIView.CommitAnimations();
			});

		}

		/// <summary>
		/// 页面消失的时候执行
		/// </summary>
		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			NSNotificationCenter.DefaultCenter.RemoveObserver(obs1);
			NSNotificationCenter.DefaultCenter.RemoveObserver(obs2);
		}

		private void PreloadWebResource()
		{
			//清除缓存
			NSUrlCache.SharedCache.RemoveAllCachedResponses();

			//设置cookie的接受政策
			NSHttpCookieStorage.SharedStorage.AcceptPolicy = NSHttpCookieAcceptPolicy.Always;

			_webView = new UIWebView(new CGRect(0, 0, 0, 0));

			string address;
			if (AppGlobalSetting.AppApiBaseUrl.EndsWith("/"))
			{
				address = AppGlobalSetting.AppApiBaseUrl;
			}
			else {
				address = AppGlobalSetting.AppApiBaseUrl + "/";
			}

#if DEBUG
			var url = AppGlobalSetting.IsHTML5Debug ? Path.Combine(address, "debug/index.html") :
			Path.Combine(FileSystemUtil.CachesFolder, "www/index.html");
#else
			var url = Path.Combine (FileSystemUtil.CachesFolder, "www/index.html");
#endif
			_webView.LoadRequest(new NSUrlRequest(new NSUrl(url)));//跳转自定义url
			View.AddSubview(_webView);
		}

		/// <summary>
		/// table类
		/// </summary>
		public class Source : UITableViewSource
		{

			UITextField _txtUserName, _txtPassword;
			CheckBoxView _chkIsRememberPassword;

			readonly CGRect _textRect;
			readonly CGRect _btnRect;
			UIButton _btnLogion;

			private readonly LoginViewController _c;


			/// <summary>
			/// table的构造函数
			/// </summary>
			/// <param name="c">C.</param>
			public Source(LoginViewController c)
			{

				SystemSettingService.AddCleanupAction("LoginViewController", () => InvokeOnMainThread(() =>
				{
					AppGlobalSetting.UserCode = string.Empty;
					AppGlobalSetting.Password = string.Empty;
					AppGlobalSetting.IsRememberPassword = false;

					if (_txtUserName != null)
						_txtUserName.Text = string.Empty;
					if (_txtPassword != null)
						_txtPassword.Text = string.Empty;
					if (_chkIsRememberPassword != null)
						_chkIsRememberPassword.IsChecked = false;
				}));

				_c = c;
				_textRect = new CGRect(AppUIStyleSetting.PaddingSizeLarge, 0, _c.View.Bounds.Width - AppUIStyleSetting.PaddingSizeLarge * 2, AppUIStyleSetting.HeightTextBox);
				_btnRect = new CGRect(AppUIStyleSetting.PaddingSizeLarge, 0, _c.View.Bounds.Width - (AppUIStyleSetting.PaddingSizeLarge * 2), AppUIStyleSetting.HeightButton);
			}

			/// <summary>
			/// 每行选中时执行
			/// </summary>
			public override async void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				await Task.Run(() => InvokeOnMainThread(() => _c.View.EndEditing(true)));
			}

			/// <summary>
			/// 设置每行的高度
			/// </summary>
			public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
			{

				if (indexPath.Section == 0)
				{
					if (indexPath.Row == 0)
						return _c._logoHeight;
					else if (indexPath.Row == 1)
						return 60;
					else
						return 60;
				}
				else if (indexPath.Section == 1)
				{
					if (indexPath.Row == 0)
						return 70;
					else
						return 40;
				}
				else {
					return 200;
				}
			}

			/// <summary>
			/// 每个section有几行
			/// </summary>
			public override nint RowsInSection(UITableView tableview, nint section)
			{
				if ((int)section == 0)
					return 3;
				else if ((int)section == 1)
					return 2;
				else
					return 1;
			}

			/// <summary>
			/// 设置有多少节
			/// </summary>
			public override nint NumberOfSections(UITableView tableView)
			{
				return 2;
			}

			/// <summary>
			/// 设置tableView内容
			/// </summary>
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0)
				{
					if (indexPath.Row == 0)
						return CreateLogoCell(tableView);
					else if (indexPath.Row == 1)
						return CreateUserNameCell(tableView);
					else
						return CreatePasswordCell(tableView);
				}
				else {
					if (indexPath.Row == 0)
						return CreateSettingCell(tableView);
					else
						return CreateLoginButtonCell(tableView);
				}
			}

			/// <summary>
			/// 创建用户名的cell
			/// </summary>
			private UITableViewCell CreateUserNameCell(UITableView tableView)
			{
				const string cellIdentifier = "TableViewCellUserName";
				var cell = tableView.DequeueReusableCell(cellIdentifier);
				if (cell == null)
				{
					cell = _c._creator.CreateTableViewCell4Input(tableView, cellIdentifier);
					_txtUserName = _c._creator.CreateIconTextBox4TableViewCell(cell, _textRect, "请输入用户账号", AppGlobalSetting.UserCode, "login_user.png");

					_txtUserName.EditingChanged += (sender, e) =>
					{
						if (AppGlobalSetting.IsRememberPassword)
							_txtPassword.Text = _txtUserName.Text != AppGlobalSetting.UserCode ? string.Empty : AppGlobalSetting.Password;
					};
				}

				return cell;
			}

			/// <summary>
			/// 创建密码的cell
			/// </summary>
			/// <returns>The password cell.</returns>
			/// <param name="tableView">Table view.</param>
			private UITableViewCell CreatePasswordCell(UITableView tableView)
			{
				const string cellIdentifier = "TableViewCellPassword";
				var cell = tableView.DequeueReusableCell(cellIdentifier);
				if (cell == null)
				{
					cell = _c._creator.CreateTableViewCell4Input(tableView, cellIdentifier);
					_txtPassword = _c._creator.CreateIconTextBox4TableViewCell(cell, _textRect, " 请输入密码",
						AppGlobalSetting.Password, "login_password.png");

					_txtPassword.SecureTextEntry = true;
				}
				return cell;
			}

			#region 登录按钮绑定事件
			#endregion
			public async void LoginButtonClick(object sender, EventArgs e)
			{
				using (var t = new Toast())
				{
					try
					{
						_btnLogion.Enabled = false;
						t.ProgressWaiting("正在登录...");
						var authUser = await AuthenticationService.LoginAsync(new UserModel()
						{
							uid = _txtUserName.Text,
							pwd = EncryptionUtil.DESDefaultEncryption(_txtPassword.Text)
						});

						if (authUser == null)
						{
							_btnLogion.Enabled = true;
							return;
						}

						AppGlobalSetting.AppAuthToken = authUser.AuthToken;
						AppGlobalSetting.UserCode = _txtUserName.Text;//xxpang
						AppGlobalSetting.DomainUserCode = authUser.UserCode;//homeinns\xxpang

						//用户在数据库中的主键
						AppGlobalSetting.UserId = authUser.SystemUserId;
						if (AppGlobalSetting.IsRememberPassword)
							AppGlobalSetting.Password = _txtPassword.Text;

						// H5版本检查更新
						//await VersionService.TryUpgradeWww();
						// iOS版本检查更新
						//var needUpdateIos = await VersionService.TryUpgradeIos();
						//if (needUpdateIos)
						//{
						//	UpdateIosClient();
						//	_btnLogion.Enabled = true;
						//	return;
						//}

						//清除联系人缓存
						_c.PreloadWebResource();

						var connected = await AuthenticationService.Logon();

						//var PMSType = await AuthenticationService.CheckPMSType("");
						//var items = JsonConvert.DeserializeObject<IsPMSType>(PMSType);

						//GlobalAppSetting.IsPmsType = items.LabelCd;
						// 如果登录人员是服务员，则直接跳转到服务员的菜单页面
						var type = Convert.ToInt32(await AuthenticationService.CheckTheUserType(_txtUserName.Text));
						// type=1表示当前用户为服务员
						if (connected && (type == 1 || type == 2))
						{
							// 跳转到服务员菜单页面
							var wvc = new AppWebViewController() { _menuUrl = "roomControl/card" };
							_c.NavigationController.PushViewController(wvc, true);
							_btnLogion.Enabled = true;
							return;
						}

						if (connected)
						{
							//从接口获取数据插入数据表中
							await MenuService.LoadMenusToSqlLite();

							ContactService.StartSyncContact();

							NSError cachesError;
							String cachesPath = FileSystemUtil.CachesFolder;
							Array cachesFolder = NSFileManager.DefaultManager.GetDirectoryContent(cachesPath, out cachesError);
							Debug.WriteLine(cachesFolder);

							String wwwPath = Path.Combine("www");
							Array wwwFolder = NSFileManager.DefaultManager.GetDirectoryContent(wwwPath, out cachesError);
							Debug.WriteLine(wwwFolder);

							//_c.NavigationController.PopViewController(false);

							UIWindow Window = new UIWindow(UIScreen.MainScreen.Bounds);
							var nav = new AppNavigationController(new AppViewController());
							Window.RootViewController = nav;
							Window.MakeKeyAndVisible();
							//_c.NavigationController.PushViewController(new AppViewController(), true);
							//UIApplication.SharedApplication.Windows[0].RootViewController = new AppViewController();
						}
						_btnLogion.Enabled = true;
					}
					catch (Exception ex)
					{
						AlertUtil.Error(ex.Message);
						_btnLogion.Enabled = true;
					}
				}
			}

			//检查iOS版本升级
			private void UpdateIosClient()
			{
				var baseUrl = AppGlobalSetting.AppApiBaseUrl;
				if (!baseUrl.ToLower().Contains("https"))
				{
					baseUrl = baseUrl.ToLower().Replace("http", "https");
				}
				if (!baseUrl.EndsWith("/"))
				{
					baseUrl += "/";
				}
				string path = "itms-services://?action=download-manifest&url="
							  + baseUrl
							  + "csupdate/ios/ios.plist";
				UIApplication.SharedApplication.OpenUrl(new NSUrl(path));
			}

			#region 登录按钮
			/// <summary>
			/// 创建登录按钮的cell
			/// </summary>
			/// <returns>The login button cell.</returns>
			/// <param name="tableView">Table view.</param>
			private UITableViewCell CreateLoginButtonCell(UITableView tableView)
			{
				const string cellIdentifier = "TableViewCellLoginButton";
				UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
				if (cell == null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;
					_btnLogion = _c._creator.CreateButton4TableViewCell(cell, _btnRect, "登 录");
					_btnLogion.TouchUpInside += LoginButtonClick;
				}
				return cell;
			}
			#endregion

			/// <summary>
			/// 创建显示logo的cell
			/// </summary>
			/// <returns>The logo cell.</returns>
			/// <param name="tableView">Table view.</param>
			private UITableViewCell CreateLogoCell(UITableView tableView)
			{
				const string cellIdentifier = "TableViewCellLogoLabel";
				UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

				if (cell == null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
					var scale = tableView.Frame.Width / _c._logo.Size.Width;
					UIImageView imageView = new UIImageView(new CGRect(0, 0, _c._logo.Size.Width * scale, _c._logo.Size.Height * scale));
					imageView.Image = _c._logo;
					cell.ContentView.Add(imageView);
				}
				return cell;
			}

			/// <summary>
			/// 创建记住密码和忘记密码的cell
			/// </summary>
			/// <returns>The subject cell.</returns>
			/// <param name="tableView">Table view.</param>
			private UITableViewCell CreateSettingCell(UITableView tableView)
			{
				const string cellIdentifier = "TableViewCellsSubjectLabel";
				UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

				if (cell == null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
					cell.SelectionStyle = UITableViewCellSelectionStyle.None;

					/// 记住密码
					_chkIsRememberPassword = new CheckBoxView(new CGRect(AppUIStyleSetting.PaddingSizeLarge, 25, 120, 20), "记住密码", AppGlobalSetting.IsRememberPassword);
					_chkIsRememberPassword.TouchUpInside += (sender, e) =>
					{
						AppGlobalSetting.IsRememberPassword = _chkIsRememberPassword.IsChecked;
						if (!AppGlobalSetting.IsRememberPassword)
						{
							AppGlobalSetting.Password = string.Empty;
						}
					};
					cell.AddSubview(_chkIsRememberPassword);


					/// 忘记密码
					var settingSeverBtn = new IconButtonView(new CGRect(tableView.Frame.Width - 120, 20, 100, 30), "忘记密码", "login_setting.png");
					settingSeverBtn.SetTitleColor(AppUIStyleSetting.AppBlueColor, UIControlState.Normal);
					settingSeverBtn.TouchUpInside += (sender, e) => {
						var wvc = new AppWebViewController() { _menuUrl = "password/forget", _special = Path.Combine(NSBundle.MainBundle.ResourcePath, "www/index.html") };
						_c.NavigationController.PushViewController(wvc, false);
					};
					cell.AddSubview(settingSeverBtn);


				}
				return cell;
			}

		}
	}
}
