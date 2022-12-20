using System;
using System.IO;
using System.IO.Compression;

namespace Sandbox.Game.Replication.StateGroups
{
	internal static class MemoryCompressor
	{
		private static void CopyTo(Stream src, Stream dest)
		{
			byte[] array = new byte[4096];
			int count;
			while ((count = src.Read(array, 0, array.Length)) != 0)
			{
				dest.Write(array, 0, count);
			}
		}

		public static byte[] Compress(byte[] bytes)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			using MemoryStream src = new MemoryStream(bytes);
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

		public static byte[] Decompress(byte[] bytes)
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
			return memoryStream2.ToArray();
		}
	}
}
