using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VRage.Utils
{
	public static class MyEncryptionSymmetricRijndael
	{
		public static string EncryptString(string inputText, string password)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			if (inputText.Length <= 0)
			{
				return "";
			}
			RijndaelManaged val = new RijndaelManaged();
			byte[] bytes = Encoding.Unicode.GetBytes(inputText);
			byte[] bytes2 = Encoding.ASCII.GetBytes(password.Length.ToString());
			PasswordDeriveBytes val2 = new PasswordDeriveBytes(password, bytes2);
			ICryptoTransform val3 = ((SymmetricAlgorithm)val).CreateEncryptor(((DeriveBytes)val2).GetBytes(32), ((DeriveBytes)val2).GetBytes(16));
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream val4 = new CryptoStream((Stream)memoryStream, val3, (CryptoStreamMode)1);
			((Stream)val4).Write(bytes, 0, bytes.Length);
			val4.FlushFinalBlock();
			byte[] inArray = memoryStream.ToArray();
			memoryStream.Close();
			((Stream)val4).Close();
			return Convert.ToBase64String(inArray);
		}

		public static string DecryptString(string inputText, string password)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_0089: Expected O, but got Unknown
			if (inputText.Length <= 0)
			{
				return "";
			}
			RijndaelManaged val = new RijndaelManaged();
			byte[] array = Convert.FromBase64String(inputText);
			byte[] bytes = Encoding.ASCII.GetBytes(password.Length.ToString());
			PasswordDeriveBytes val2 = new PasswordDeriveBytes(password, bytes);
			ICryptoTransform val3 = ((SymmetricAlgorithm)val).CreateDecryptor(((DeriveBytes)val2).GetBytes(32), ((DeriveBytes)val2).GetBytes(16));
			MemoryStream memoryStream = new MemoryStream(array);
			CryptoStream val4 = new CryptoStream((Stream)memoryStream, val3, (CryptoStreamMode)0);
			byte[] array2 = new byte[array.Length];
			int count = ((Stream)val4).Read(array2, 0, array2.Length);
			memoryStream.Close();
			((Stream)val4).Close();
			return Encoding.Unicode.GetString(array2, 0, count);
		}
	}
}
