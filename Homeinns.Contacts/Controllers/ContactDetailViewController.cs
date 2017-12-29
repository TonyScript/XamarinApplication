using MonoTouch.Dialog;
using Foundation;
using UIKit;
using Homeinns.Contacts.ViewModel;
using Homeinns.Common.Configuration;
using Homeinns.Common.Util;
using Homeinns.Contacts.Views;

namespace Homeinns.Contacts.Controllers
{
	/// <summary>
	/// 联系人详情界面
	/// </summary>
	public class ContactDetailViewController : DialogViewController
	{
		UIViewCreator _creator;
		RootElement _root;
		ProfileElement _profileElement;
		StyledStringElement _departmentElement, _phoneElement, _emailElement;
		ButtonElement _callPhoneButton;

		ContactViewModel _contact;

		public ContactDetailViewController(RootElement r, ContactViewModel c)
			: base(UITableViewStyle.Grouped, r, true)
		{
			_creator = new UIViewCreator(this.View);
			_creator.SetTableViewStyle(this.TableView);

			_root = r;
			_root.UnevenRows = true;
			_contact = c;

			_profileElement = new ProfileElement(_contact.ContactName, "职位: " + _contact.Position, UITableViewCellStyle.Subtitle)
			{
				Image = _contact.GetAvatarImage()
			};
			_profileElement.BackgroundColor = AppUIStyleSetting.NavigationBarColor;

			_departmentElement = new StyledStringElement("所属部门", _contact.Department, UITableViewCellStyle.Value1);
			_departmentElement.BackgroundColor = AppUIStyleSetting.NavigationBarColor;
			_departmentElement.Image = UIImage.FromFile("department.png");
			_departmentElement.Alignment = UITextAlignment.Left;

			_emailElement = new StyledStringElement("电子邮箱", _contact.Email);
			_emailElement.BackgroundColor = AppUIStyleSetting.NavigationBarColor;
			_emailElement.Image = UIImage.FromFile("mail.png");
			_emailElement.Alignment = UITextAlignment.Left;

			_phoneElement = new StyledStringElement("联系电话", _contact.Phone);
			_phoneElement.BackgroundColor = AppUIStyleSetting.NavigationBarColor;
			_phoneElement.Image = UIImage.FromFile("phone.png");

			_root.Add(new Section[] {
				new Section() {
					_profileElement
				},
				new Section() {
					_departmentElement, _emailElement, _phoneElement
				}
			});

			if (!string.IsNullOrWhiteSpace(_contact.Phone))
			{
				_callPhoneButton = new ButtonElement("拨打电话");
				_callPhoneButton.Tapped += () =>
				{
					var urlToSend = new NSUrl("tel:" + _phoneElement.Value); // phonenum is in the format 1231231234

					if (UIApplication.SharedApplication.CanOpenUrl(urlToSend))
					{
						UIApplication.SharedApplication.OpenUrl(urlToSend);
					}
					else {
						AlertUtil.Error("not support in simulator!");
					}
				};

				_root.Add(new Section[] {
					new Section() {
						_callPhoneButton
					}
				});
			}
		}


		/// <summary>
		///  错误处理
		/// </summary>
		private bool HandleError(string msg)
		{
			AlertUtil.Error(msg);
			return true;
		}

		/// <summary>
		/// 页面即将出现的时候执行
		/// </summary>
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			ErrorHandlerUtil.Subscribe(HandleError);
		}

		/// <summary>
		/// 页面将要消失的时候执行
		/// </summary>
		public override void ViewWillDisappear(bool animated)
		{
			ViewDidDisappear(animated);
			ErrorHandlerUtil.UnSubscribe(HandleError);
		}
	}
}

