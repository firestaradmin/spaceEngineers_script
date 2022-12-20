using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VRage.Utils
{
	public static class MySerialKey
	{
		private static int m_dataSize = 14;

		private static int m_hashSize = 4;

		public static string[] Generate(short productTypeId, short distributorId, int keyCount)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			byte[] bytes = BitConverter.GetBytes(productTypeId);
			byte[] bytes2 = BitConverter.GetBytes(distributorId);
			RNGCryptoServiceProvider val = new RNGCryptoServiceProvider();
			try
			{
				SHA1Managed val2 = new SHA1Managed();
				try
				{
					List<string> list = new List<string>(keyCount);
					byte[] array = new byte[m_dataSize + m_hashSize];
					for (int i = 0; i < keyCount; i++)
					{
						((RandomNumberGenerator)val).GetBytes(array);
						array[0] = bytes[0];
						array[1] = bytes[1];
						array[2] = bytes2[0];
						array[3] = bytes2[1];
						for (int j = 0; j < 4; j++)
						{
							array[j] = (byte)(array[j] ^ array[j + 4]);
						}
						byte[] array2 = ((HashAlgorithm)val2).ComputeHash(array, 0, m_dataSize);
						for (int k = 0; k < m_hashSize; k++)
						{
							array[m_dataSize + k] = array2[k];
						}
						list.Add(new string(My5BitEncoding.Default.Encode(Enumerable.ToArray<byte>((IEnumerable<byte>)array))) + "X");
					}
					return list.ToArray();
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		public static string AddDashes(string key)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < key.Length; i++)
			{
				if (i % 5 == 0 && i > 0)
				{
					stringBuilder.Append('-');
				}
				stringBuilder.Append(key[i]);
			}
			return stringBuilder.ToString();
		}

		public static string RemoveDashes(string key)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < key.Length; i++)
			{
				if ((i + 1) % 6 != 0)
				{
					stringBuilder.Append(key[i]);
				}
			}
			return stringBuilder.ToString();
		}

		public static bool ValidateSerial(string serialKey, out int productTypeId, out int distributorId)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			RNGCryptoServiceProvider val = new RNGCryptoServiceProvider();
			try
			{
				SHA1 val2 = SHA1.Create();
				try
				{
					if (serialKey.EndsWith("X"))
					{
						byte[] array = My5BitEncoding.Default.Decode(Enumerable.ToArray<char>(Enumerable.Take<char>((IEnumerable<char>)serialKey, serialKey.Length - 1)));
						byte[] array2 = Enumerable.ToArray<byte>(Enumerable.Take<byte>((IEnumerable<byte>)array, array.Length - m_hashSize));
						byte[] array3 = ((HashAlgorithm)val2).ComputeHash(array2);
						if (Enumerable.SequenceEqual<byte>(Enumerable.Take<byte>(Enumerable.Skip<byte>((IEnumerable<byte>)array, array2.Length), m_hashSize), Enumerable.Take<byte>((IEnumerable<byte>)array3, m_hashSize)))
						{
							for (int i = 0; i < 4; i++)
							{
								array2[i] = (byte)(array2[i] ^ array2[i + 4]);
							}
							productTypeId = BitConverter.ToInt16(array2, 0);
							distributorId = BitConverter.ToInt16(array2, 2);
							return true;
						}
					}
					productTypeId = 0;
					distributorId = 0;
					return false;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}
}
