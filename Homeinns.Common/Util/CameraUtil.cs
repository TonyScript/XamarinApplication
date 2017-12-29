using System;
using UIKit;
using Foundation;

namespace Homeinns.Common.Util
{
	/// <summary>
	/// 拍照相机相关的帮助类
	/// </summary>
	public static class CameraUtil
	{
		private static UIImagePickerController _picker;
		private static Action<UIImage> _callback;

		static void Init()
		{
			if (_picker != null)
				return;

			_picker = new UIImagePickerController();
			_picker.AllowsEditing = true;
			_picker.Delegate = new CameraDelegate();
		}

		private static UIImage GetImage(NSDictionary info)
		{
			if (info[UIImagePickerController.MediaType].ToString().ToLower() != "public.image")
				return null;

			if (_picker.AllowsEditing)
				return info[UIImagePickerController.EditedImage] as UIImage;
			else
				return info[UIImagePickerController.OriginalImage] as UIImage;
		}

		class CameraDelegate : UIImagePickerControllerDelegate
		{
			public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
			{
				var cb = _callback;
				_callback = null;

				picker.DismissViewController(true, (Action)null);
				cb(GetImage(info));
			}
		}

		/// <summary>
		/// 打开手机的摄像头拍照
		/// </summary>
		/// <param name="parent">使用拍照功能的ViewController</param>
		/// <param name="callback">拍照成功后的回调函数</param>
		/// <param name="allowaEditing">拍照后，是否允许编辑</param>
		public static void TakePicture(UIViewController parent, Action<UIImage> callback, bool allowaEditing = false)
		{
			Init();
			_picker.AllowsEditing = allowaEditing;
			_picker.SourceType = UIImagePickerControllerSourceType.Camera;
			_callback = callback;
			parent.PresentViewController((UIViewController)_picker, true, (Action)null);
		}

		/// <summary>
		/// 打开手机的图库选择照片
		/// </summary>
		/// <param name="parent">使用拍照功能的ViewController</param>
		/// <param name="callback">拍照成功后的回调函数</param>
		/// <param name="allowaEditing">拍照后，是否允许编辑</param>
		public static void SelectPicture(UIViewController parent, Action<UIImage> callback, bool allowaEditing = false)
		{
			Init();
			_picker.AllowsEditing = allowaEditing;
			_picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
			_callback = callback;
			parent.PresentViewController((UIViewController)_picker, true, (Action)null);
		}
	}
}

