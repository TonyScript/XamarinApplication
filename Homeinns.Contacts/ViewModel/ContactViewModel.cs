using System;
using Foundation;
using UIKit;
using Homeinns.Common.Util;
using SQLite;

namespace Homeinns.Contacts.ViewModel
{
	/// <summary>
	/// 存在本地数据库联系人的数据模型
	/// </summary>
	public class ContactViewModel
	{
		#region 联系人默认头像

		private static UIImage _defaultAvatar;

		public static UIImage DefaultAvatar
		{
			get
			{
				if (_defaultAvatar == null)
					_defaultAvatar = UIImage.FromFile("contact_default_avatar.png");

				return _defaultAvatar;
			}
		}

		#endregion

		public string XrmUserId { get; set; }

		private string _contactId;

		[PrimaryKey]
		public string ContactId
		{
			get { return _contactId; }
			set { _contactId = value; }
		}


		[Ignore]
		public string ContactCode
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_contactId))
					return string.Empty;
				var len = _contactId.IndexOf("@") < 0 ? _contactId.Length : _contactId.IndexOf("@");
				return _contactId.Substring(0, len);
			}
		}

		private string _contactName;

		public string ContactName
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_contactName))
					return ContactCode;
				else
					return _contactName;
			}
			set { _contactName = value; }
		}

		private string _contactNamePinYin;

		[Ignore]
		public string ContactNamePinYin
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_contactNamePinYin))
					_contactNamePinYin = NPinyin.Pinyin.GetPinyin(ContactName, System.Text.Encoding.Unicode);

				return _contactNamePinYin;
			}
		}

		private string _contactNamePinYinFirst;

		[Ignore]
		public string ContactNamePinYinFirst
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_contactNamePinYinFirst))
					_contactNamePinYinFirst = NPinyin.Pinyin.GetInitials(ContactName, System.Text.Encoding.Unicode);

				return _contactNamePinYinFirst;
			}
		}

		[Ignore]
		public string AvatarImageBase64String
		{
			get
			{
				return FileSystemUtil.GetBase64StringFromCache(ContactId);
			}

			set { FileSystemUtil.SaveBase64StringToCache(ContactId, value); }
		}

		/// <summary>
		/// 联系电话
		/// </summary>
		public string Phone { get; set; }

		/// <summary>
		/// 部门
		/// </summary>
		public string Department { get; set; }

		/// <summary>
		/// 邮箱
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// 职位
		/// </summary>
		public string Position { get; set; }

		/// <summary>
		/// 是否禁用
		/// </summary>
		/// <value><c>true</c> if this instance is disabled; otherwise, <c>false</c>.</value>
		public bool IsDisabled { get; set; }

		/// <summary>
		/// 是否更新头像
		/// </summary>
		/// <value><c>true</c> if this instance is disabled; otherwise, <c>false</c>.</value>
		public bool IsUpdate { get; set; }

		public UIImage GetAvatarImage()
		{
			var avatarImage = ImageUtil.GetImageFromCache(ContactId);
			return avatarImage ?? DefaultAvatar;
		}


		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var c = obj as ContactViewModel;
			if (c == null)
				return false;

			if (string.IsNullOrWhiteSpace(ContactId))
				return false;

			if (string.IsNullOrWhiteSpace(c.ContactId))
				return false;

			return ContactId.ToLower().Equals(c.ContactId.ToLower());
		}

		public override int GetHashCode()
		{
			if (ContactId == null)
				return string.Empty.GetHashCode();
			else
				return ContactId.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[ContactViewModel: XrmUserId={0}, ContactId={1}, ContactCode={2}, ContactName={3}, ContactNamePinYin={4}, ContactNamePinYinFirst={5}, AvatarImageBase64String={6}, Phone={7}, Department={8}, Email={9}, Position={10}]", XrmUserId, ContactId, ContactCode, ContactName, ContactNamePinYin, ContactNamePinYinFirst, AvatarImageBase64String, Phone, Department, Email, Position);
		}
	}
}

