using System;
using System.IO;
using System.IO.Compression;

namespace VRage
{
	public class MyCompressionFileSave : IMyCompressionSave, IDisposable
	{
		private int m_uncompressedSize;

		private FileStream m_output;

		private GZipStream m_gz;

		private BufferedStream m_buffer;

		public MyCompressionFileSave(string targetFile)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			m_output = new FileStream(targetFile, FileMode.Create, FileAccess.Write);
			for (int i = 0; i < 4; i++)
			{
				m_output.WriteByte(0);
			}
			m_gz = new GZipStream((Stream)m_output, (CompressionMode)1, true);
			m_buffer = new BufferedStream((Stream)(object)m_gz, 16384);
		}

		public void Dispose()
		{
			if (m_output != null)
			{
				try
				{
					((Stream)(object)m_buffer).Close();
				}
				finally
				{
					m_buffer = null;
				}
				try
				{
					((Stream)(object)m_gz).Close();
				}
				finally
				{
					m_gz = null;
				}
				m_output.Position = 0L;
				WriteUncompressedSize(m_output, m_uncompressedSize);
				try
				{
					m_output.Close();
				}
				finally
				{
					m_output = null;
				}
			}
		}

		public void Add(byte[] value)
		{
			Add(value, value.Length);
		}

		public void Add(byte[] value, int count)
		{
			((Stream)(object)m_buffer).Write(value, 0, count);
			m_uncompressedSize += count;
		}

		public void Add(float value)
		{
			Add(BitConverter.GetBytes(value));
		}

		public void Add(int value)
		{
			Add(BitConverter.GetBytes(value));
		}

		public void Add(byte value)
		{
			((Stream)(object)m_buffer).WriteByte(value);
			m_uncompressedSize++;
		}

		private unsafe static void WriteUncompressedSize(FileStream output, int uncompressedSize)
		{
			byte* ptr = (byte*)(&uncompressedSize);
			for (int i = 0; i < 4; i++)
			{
				output.WriteByte(ptr[i]);
			}
		}
	}
}
