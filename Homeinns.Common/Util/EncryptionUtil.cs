using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Homeinns.Common.Util
{
	/// <summary>
	/// 字符串加密方法的封装帮助类
	/// </summary>
	public static class EncryptionUtil
	{
		/// <summary>
		/// 使用MD5加密
		/// </summary>
		/// <param name="plainText">明文</param>
		/// <returns>密文</returns>
		public static string MD5Encryption(string plainText)
		{
			var md5 = MD5.Create();
			var hashs = md5.ComputeHash(Encoding.UTF8.GetBytes(plainText));

			var builder = new StringBuilder();
			// Loop through each byte of the hashed data 
			// and format each one as a hexadecimal string. 
			foreach (var t in hashs)
			{
				builder.Append(t.ToString("x2"));
			}

			// Return the hexadecimal string. 
			return builder.ToString();

			/*
            return BitConverter.ToString(
                (new MD5CryptoServiceProvider()).ComputeHash(
                Encoding.ASCII.GetBytes(plainText)));
             */
		}

		/// <summary>
		/// 使用RSA加密
		/// </summary>
		/// <param name="plainText">明文</param>
		/// <returns>密文</returns>
		public static string RSAEncryption(string plainText)
		{
			return BitConverter.ToString(
				(new RSACryptoServiceProvider()).Encrypt(
					Encoding.ASCII.GetBytes(plainText), false));
		}

		/// <summary>
		/// 使用RSA解密
		/// </summary>
		/// <param name="encryptedText">密文</param>
		/// <returns>明文</returns>
		public static string RSADecryption(string encryptedText)
		{
			return BitConverter.ToString(
				(new RSACryptoServiceProvider()).Decrypt(
					Encoding.ASCII.GetBytes(encryptedText), false));
		}

		/// <summary>
		/// 使用DES加密
		/// </summary>
		/// <param name="plainText">明文</param>
		/// <returns>DES密文</returns>
		public static string DESEncryption(string plainText)
		{
			return _enCrypt(new DESCryptoServiceProvider(), plainText);
		}

		/// <summary>
		/// 使用DES算法解密数据
		/// </summary>
		/// <param name="encryptedText">要解密的密文</param>
		/// <returns></returns>
		public static string DESDecryption(string encryptedText)
		{
			return _deCrypt(new DESCryptoServiceProvider(), encryptedText);
		}

		/// <summary>
		/// 诗句加密使用的Key
		/// </summary>
		public static String EncryptionKey = typeof(BinaryReader) + "-w9" +
			typeof(System.Xml.NameTable) + "sdf3f" + typeof(Random) + "jsow23j235ay2s" +
			typeof(EncryptionUtil) + "a2skwp230a" + typeof(System.Collections.Queue) + "dadjm" +
			typeof(NullReferenceException);

		/// <summary>
		/// 通用DES密钥
		/// </summary>
		public static string DefaultEncryptionKEY = "SZRekTec";

		/// <summary>
		/// 通用DES向量
		/// </summary>
		public static string DefaultEncryptionIV = "RekTecSZ";

		/// <summary>
		/// 通用的DES加密
		/// </summary>
		/// <param name="plainText">明文</param>
		/// <returns>DES密文</returns>
		public static string DESDefaultEncryption(string plainText)
		{
			DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
			cryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(DefaultEncryptionKEY);
			cryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(DefaultEncryptionIV);

			MemoryStream ms = new MemoryStream();
			CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(), CryptoStreamMode.Write);

			StreamWriter sw = new StreamWriter(cst);
			sw.Write(plainText);
			sw.Flush();
			cst.FlushFinalBlock();
			sw.Flush();

			return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

		}

		/// <summary>
		/// 通用的DES解密
		/// </summary>
		/// <param name="encryptedText">要解密的密文</param>
		/// <returns>明文</returns>
		public static string DESDefaultDecryption(string encryptedText)
		{
			byte[] byEnc;

			try
			{
				encryptedText.Replace("_%_", "/");
				encryptedText.Replace("-%-", "#");
				byEnc = Convert.FromBase64String(encryptedText);

			}
			catch
			{
				return null;
			}

			DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
			cryptoProvider.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(DefaultEncryptionKEY);
			cryptoProvider.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(DefaultEncryptionIV);
			MemoryStream ms = new MemoryStream(byEnc);
			CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(), CryptoStreamMode.Read);
			StreamReader sr = new StreamReader(cst);
			return sr.ReadToEnd();

		}

		#region implementation
		private static String _enCrypt(SymmetricAlgorithm Algorithm, String ValueToEnCrypt)
		{
			// 将字符串保存到字节数组中
			Byte[] InputByteArray = Encoding.UTF8.GetBytes(ValueToEnCrypt);

			// 创建一个key.
			Byte[] Key = Encoding.ASCII.GetBytes(EncryptionKey);
			Algorithm.Key = (Byte[])ArrayFunctions.ReDim(Key, Algorithm.Key.Length);
			Algorithm.IV = (Byte[])ArrayFunctions.ReDim(Key, Algorithm.IV.Length);

			var MemStream = new MemoryStream();
			var CrypStream = new CryptoStream(MemStream, Algorithm.CreateEncryptor(), CryptoStreamMode.Write);

			// Write the byte array into the crypto stream( It will end up in the memory stream).
			CrypStream.Write(InputByteArray, 0, InputByteArray.Length);
			CrypStream.FlushFinalBlock();

			// Get the data back from the memory stream, and into a string.
			var StringBuilder = new StringBuilder();
			for (Int32 i = 0; i < MemStream.ToArray().Length; i++)
			{
				Byte ActualByte = MemStream.ToArray()[i];

				// Format the actual byte as HEX.
				StringBuilder.AppendFormat("{0:X2}", ActualByte);
			}

			return StringBuilder.ToString();
		}


		private static String _deCrypt(SymmetricAlgorithm Algorithm, String ValueToDeCrypt)
		{
			// Put the input string into the byte array.
			var InputByteArray = new Byte[ValueToDeCrypt.Length / 2];

			for (Int32 i = 0; i < ValueToDeCrypt.Length / 2; i++)
			{
				Int32 Value = (Convert.ToInt32(ValueToDeCrypt.Substring(i * 2, 2), 16));
				InputByteArray[i] = (Byte)Value;
			}

			// Create the key.
			Byte[] Key = Encoding.ASCII.GetBytes(EncryptionKey);
			Algorithm.Key = (Byte[])ArrayFunctions.ReDim(Key, Algorithm.Key.Length);
			Algorithm.IV = (Byte[])ArrayFunctions.ReDim(Key, Algorithm.IV.Length);

			var MemStream = new MemoryStream();
			var CrypStream = new CryptoStream(MemStream, Algorithm.CreateDecryptor(), CryptoStreamMode.Write);

			// Flush the data through the crypto stream into the memory stream.
			CrypStream.Write(InputByteArray, 0, InputByteArray.Length);
			CrypStream.FlushFinalBlock();

			return Encoding.UTF8.GetString(MemStream.ToArray());
		}
		#endregion

		#region RSA
		/// <summary>
		/// 生成新的RSA的公钥和私钥密码对
		/// </summary>
		/// <param name="publicKey">公钥</param>
		/// <param name="privateKey">私钥</param>
		public static void AssignNewRsaKey(out string publicKey, out string privateKey)
		{
			using (var rsa = new RSACryptoServiceProvider(1024))
			{
				rsa.PersistKeyInCsp = false;
				publicKey = rsa.ToXmlString(false);
				privateKey = rsa.ToXmlString(true);
			}
		}


		/// <summary>
		/// 使用RSA的 public key 加密数据
		/// </summary>
		/// <param name="publicKey">公钥</param>
		/// <param name="plainText">明文</param>
		/// <returns>密文</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static string EncryptRSA(string publicKey, string plainText)
		{
			if (string.IsNullOrEmpty(publicKey)) throw new ArgumentNullException("publicKey");
			if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException("plainText");

			byte[] cipherbytes;

			using (var rsa = new RSACryptoServiceProvider(1024))
			{
				rsa.PersistKeyInCsp = false;

				rsa.FromXmlString(publicKey);

				byte[] plainbytes = Encoding.UTF8.GetBytes(plainText);
				cipherbytes = rsa.Encrypt(plainbytes, false);
			}

			return Convert.ToBase64String(cipherbytes);
		}

		/// <summary>
		/// 使用RSA的private key 解密数据
		/// </summary>
		/// <param name="privateKey">私钥</param>
		/// <param name="encryptedText">密文</param>
		/// <returns>明文</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static string DecryptRSA(string privateKey, string encryptedText)
		{
			if (string.IsNullOrEmpty(privateKey)) throw new ArgumentNullException("privateKey");
			if (string.IsNullOrEmpty(encryptedText)) throw new ArgumentNullException("encryptedText");

			byte[] plain;

			using (var rsa = new RSACryptoServiceProvider(1024))
			{
				rsa.PersistKeyInCsp = false;

				byte[] encodedCipherText = Convert.FromBase64String(encryptedText);
				rsa.FromXmlString(privateKey);
				plain = rsa.Decrypt(encodedCipherText, false);
			}

			return Encoding.UTF8.GetString(plain);
		}
		#endregion
	}

	internal class ArrayFunctions : Object
	{
		/// <summary>
		/// 重新定义一个数组列表
		/// </summary>
		/// <param name="originalArray">需要被重定义的数组</param>
		/// <param name="newSize">这个数组的新大小</param>
		public static Array ReDim(Array originalArray, Int32 newSize)
		{
			Type arrayElementsType = originalArray.GetType().GetElementType();
			Array newArray = Array.CreateInstance(arrayElementsType, newSize);
			Array.Copy(originalArray, 0, newArray, 0, Math.Min(originalArray.Length, newSize));
			return newArray;
		}
	}
}
