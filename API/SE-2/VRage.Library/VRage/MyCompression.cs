using System;
using System.IO;
using System.IO.Compression;

namespace VRage
{
	public static class MyCompression
	{
		private static byte[] m_buffer = new byte[16384];

		public static byte[] Compress(byte[] buffer)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected O, but got Unknown
			using MemoryStream memoryStream = new MemoryStream();
			GZipStream val = new GZipStream((Stream)memoryStream, (CompressionMode)1, true);
			try
			{
				((Stream)(object)val).Write(buffer, 0, buffer.Length);
				((Stream)(object)val).Close();
				memoryStream.Position = 0L;
				byte[] array = new byte[memoryStream.Length + 4];
				memoryStream.Read(array, 4, (int)memoryStream.Length);
				Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, array, 0, 4);
				return array;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		public static void CompressFile(string fileName)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			using MemoryStream memoryStream = new MemoryStream();
			FileInfo val = new FileInfo(fileName);
			Buffer.BlockCopy(BitConverter.GetBytes(val.get_Length()), 0, m_buffer, 0, 4);
			memoryStream.Write(m_buffer, 0, 4);
			GZipStream val2 = new GZipStream((Stream)memoryStream, (CompressionMode)1, true);
			try
			{
				using (FileStream fileStream = File.OpenRead(fileName))
				{
					for (int num = fileStream.Read(m_buffer, 0, m_buffer.Length); num > 0; num = fileStream.Read(m_buffer, 0, m_buffer.Length))
					{
						((Stream)(object)val2).Write(m_buffer, 0, num);
					}
				}
				((Stream)(object)val2).Close();
				memoryStream.Position = 0L;
				using FileStream fileStream2 = File.Create(fileName);
				for (int num2 = memoryStream.Read(m_buffer, 0, m_buffer.Length); num2 > 0; num2 = memoryStream.Read(m_buffer, 0, m_buffer.Length))
				{
					fileStream2.Write(m_buffer, 0, num2);
					fileStream2.Flush();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}

		public static byte[] Decompress(byte[] gzBuffer)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			using MemoryStream memoryStream = new MemoryStream();
			int num = BitConverter.ToInt32(gzBuffer, 0);
			memoryStream.Write(gzBuffer, 4, gzBuffer.Length - 4);
			memoryStream.Position = 0L;
			byte[] array = new byte[num];
			GZipStream val = new GZipStream((Stream)memoryStream, (CompressionMode)0);
			try
			{
				((Stream)(object)val).Read(array, 0, array.Length);
				return array;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		public static void DecompressFile(string fileName)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			using MemoryStream memoryStream = new MemoryStream();
			using (FileStream fileStream = File.OpenRead(fileName))
			{
				fileStream.Read(m_buffer, 0, 4);
				GZipStream val = new GZipStream((Stream)fileStream, (CompressionMode)0);
				try
				{
					for (int num = ((Stream)(object)val).Read(m_buffer, 0, m_buffer.Length); num > 0; num = ((Stream)(object)val).Read(m_buffer, 0, m_buffer.Length))
					{
						memoryStream.Write(m_buffer, 0, num);
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			memoryStream.Position = 0L;
			using FileStream fileStream2 = File.Create(fileName);
			for (int num2 = memoryStream.Read(m_buffer, 0, m_buffer.Length); num2 > 0; num2 = memoryStream.Read(m_buffer, 0, m_buffer.Length))
			{
				fileStream2.Write(m_buffer, 0, num2);
				fileStream2.Flush();
			}
		}
	}
}
