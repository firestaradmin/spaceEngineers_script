using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using VRage.FileSystem;

namespace Sandbox.Engine.Utils
{
	internal static class MyDataIntegrityChecker
	{
		public const int HASH_SIZE = 20;

		private static byte[] m_combinedData = new byte[40];

		private static byte[] m_hash = new byte[20];

		private static StringBuilder m_stringBuilder = new StringBuilder(8);

		public static void ResetHash()
		{
			Array.Clear(m_hash, 0, 20);
		}

		public static void HashInFile(string fileName)
		{
			using (Stream data = MyFileSystem.OpenRead(fileName).UnwrapGZip())
			{
				HashInData(fileName.ToLower(), data);
			}
			MySandboxGame.Log.WriteLine(GetHashHex());
		}

		public static void HashInData(string dataName, Stream data)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			HashAlgorithm val = (HashAlgorithm)new SHA1Managed();
			try
			{
				byte[] sourceArray = val.ComputeHash(data);
				byte[] sourceArray2 = val.ComputeHash(Encoding.Unicode.GetBytes(dataName.ToCharArray()));
				Array.Copy(sourceArray, m_combinedData, 20);
				Array.Copy(sourceArray2, 0, m_combinedData, 20, 20);
				byte[] array = val.ComputeHash(m_combinedData);
				for (int i = 0; i < 20; i++)
				{
					m_hash[i] ^= array[i];
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		public static string GetHashHex()
		{
			uint num = 0u;
			m_stringBuilder.Clear();
			byte[] hash = m_hash;
			foreach (byte b in hash)
			{
				m_stringBuilder.AppendFormat("{0:x2}", b);
				num += b;
			}
			return m_stringBuilder.ToString();
		}

		public static string GetHashBase64()
		{
			return Convert.ToBase64String(m_hash);
		}
	}
}
