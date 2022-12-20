using System;
using System.IO;
using System.IO.Compression;

namespace VRage
{
	public class MyCompressionFileLoad : IMyCompressionLoad, IDisposable
	{
		[ThreadStatic]
		private static byte[] m_intBytesBuffer;

		private FileStream m_input;

		private GZipStream m_gz;

		private BufferedStream m_buffer;

		public MyCompressionFileLoad(string fileName)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			if (m_intBytesBuffer == null)
			{
				m_intBytesBuffer = new byte[4];
			}
			m_input = File.OpenRead(fileName);
			m_input.Read(m_intBytesBuffer, 0, 4);
			m_gz = new GZipStream((Stream)m_input, (CompressionMode)0);
			m_buffer = new BufferedStream((Stream)(object)m_gz, 16384);
		}

		public void Dispose()
		{
			if (m_buffer != null)
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
				try
				{
					m_input.Close();
				}
				finally
				{
					m_input = null;
				}
			}
		}

		public int GetInt32()
		{
			((Stream)(object)m_buffer).Read(m_intBytesBuffer, 0, 4);
			return BitConverter.ToInt32(m_intBytesBuffer, 0);
		}

		public byte GetByte()
		{
			return (byte)((Stream)(object)m_buffer).ReadByte();
		}

		public int GetBytes(int bytes, byte[] output)
		{
			return ((Stream)(object)m_buffer).Read(output, 0, bytes);
		}

		public bool EndOfFile()
		{
			return m_input.Position == m_input.Length;
		}

		public byte[] GetCompressedBuffer()
		{
			m_input.Position = 0L;
			byte[] array = new byte[m_input.Length];
			m_input.Read(array, 0, (int)m_input.Length);
			return array;
		}
	}
}
