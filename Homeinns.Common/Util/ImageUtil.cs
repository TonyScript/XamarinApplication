using System;
using CoreGraphics;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Homeinns.Common.Util
{
	/// <summary>
	/// Image操作的工具类
	/// </summary>
	public static class ImageUtil
	{
		/// <summary>
		/// 将图像转换为Base64编码格式的字符串
		/// </summary>
		/// <param name="image">UIImage对象</param>
		/// <param name="sizeK">最终的文件的大小</param>
		/// <param name="width">图片的宽度（像素）</param>
		/// <returns>图像的Base64编码</returns>
		public static Task<string> ConvertImage2Base64StringAsync(UIImage image, float sizeK = 1F, float width = 0F)
		{
			return Task.Run(() => ConvertImage2Base64String(image, sizeK, width));
		}

		/// <summary>
		/// 将图像转换为Base64编码格式的字符串
		/// </summary>
		/// <param name="image">UIImage对象</param>
		/// <param name="sizeK">最终的文件的大小</param>
		/// <param name="width">图片的宽度（像素）</param>
		/// <returns>图像的Base64编码</returns>
		public static string ConvertImage2Base64String(UIImage image, float sizeK = 1F, float width = 0F, bool isNeedRotate = false)
		{
			if (image == null)
				return null;

			if (isNeedRotate)
				image = RotateImage(image);

			if (sizeK <= 0)
				sizeK = 1;
			if (width < 0)
				width = 0;
			try
			{
				image = ScaleImage(image, width);

				var bytes = CompressImage(image, sizeK * 1024, 1);

				return Convert.ToBase64String(bytes);
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
				return null;
			}
		}

		private static UIImage RotateImage(UIImage image, UIImageOrientation orientation = UIImageOrientation.Down)
		{
			var cgImage = image.CGImage;
			var newImage = new UIImage(cgImage, 1, orientation);
			return newImage;
		}

		private static UIImage ScaleImage(UIImage image, nfloat width)
		{
			var scaleRate = (nfloat)1F;
			if (width <= 0)
			{
				width = 1024; //未设定，则默认压缩到1024
			}
			//根据图片的小边进行缩放率计算，保持图片长宽比不变
			if (image.Size.Height < image.Size.Width)
			{
				if (image.Size.Height > 1024)
					scaleRate = width / image.Size.Height;
			}
			else {
				if (image.Size.Width > 1024)
					scaleRate = width / image.Size.Width;
			}

			if (scaleRate != 1F)
				image = image.Scale(new CGSize(image.Size.Width * scaleRate, image.Size.Height * scaleRate));

			return image;
		}

		private static byte[] CompressImage(UIImage image, float size, float compressQuality)
		{
			if (compressQuality > 1)
				compressQuality = 1;
			if (compressQuality < 0)
				compressQuality = 0;

			if (compressQuality == 0)
				return image.AsJPEG(compressQuality).ToArray();

			var bytes = image.AsJPEG(compressQuality).ToArray();
			if (bytes.Length > size)
				return CompressImage(image, size, compressQuality - 0.05F);
			else
				return bytes;
		}

		/// <summary>
		/// 将Base64的字符串转化为图像格式（UIImage）
		/// </summary>
		/// <param name="base64">base64格式的图像</param>
		/// <returns>UIImage</returns>
		public static UIImage ConvertBase64String2Image(string base64)
		{
			if (string.IsNullOrWhiteSpace(base64))
				return null;

			try
			{
				var bytes = Convert.FromBase64String(base64);
				var data = NSData.FromArray(bytes);
				return UIImage.LoadFromData(data);
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
				return null;
			}
		}

		/// <summary>
		/// 从文件缓存中获取图像
		/// </summary>
		/// <param name="fileId">图像的文件缓存Id</param>
		/// <returns>UIImage</returns>
		public static UIImage GetImageFromCache(string fileId)
		{
			try
			{
				var base64 = FileSystemUtil.GetBase64StringFromCache(fileId);
				return string.IsNullOrWhiteSpace(base64) ? null : ConvertBase64String2Image(base64);
			}
			catch (Exception ex)
			{
				ErrorHandlerUtil.ReportException(ex);
				return null;
			}
		}


		/// <summary>
		/// Creates the color of the image with.
		/// </summary>
		/// <returns>The image with color.</returns>
		/// <param name="color">Color.</param>
		/// <param name="size">Size.</param>
		public static UIImage CreateImageWithColor(UIColor color, CGRect rect)
		{
			UIGraphics.BeginImageContext((CGSize)rect.Size);
			CGContext context = UIGraphics.GetCurrentContext();
			context.SetFillColor(color.CGColor);
			context.FillRect((CGRect)rect);
			UIImage image = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return image;
		}
	}
}

