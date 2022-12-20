using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Sandbox.Game.Entities.Blocks
{
	internal static class StringCompressor
	{
		public static void CopyTo(Stream src, Stream dest)
		{
			byte[] array = new byte[4096];
			int count;
			while ((count = src.Read(array, 0, array.Length)) != 0)
			{
				dest.Write(array, 0, count);
			}
		}

		public static byte[] CompressString(string str)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			using MemoryStream src = new MemoryStream(Encoding.UTF8.GetBytes(str));
			using MemoryStream memoryStream = new MemoryStream();
			GZipStream val = new GZipStream((Stream)memoryStream, (CompressionMode)1);
			try
			{
				CopyTo(src, (Stream)(object)val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			return memoryStream.ToArray();
		}

		public static string DecompressString(byte[] bytes)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			using MemoryStream memoryStream = new MemoryStream(bytes);
			using MemoryStream memoryStream2 = new MemoryStream();
			GZipStream val = new GZipStream((Stream)memoryStream, (CompressionMode)0);
			try
			{
				CopyTo((Stream)(object)val, memoryStream2);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			return Encoding.UTF8.GetString(memoryStream2.ToArray());
		}
	}
}
