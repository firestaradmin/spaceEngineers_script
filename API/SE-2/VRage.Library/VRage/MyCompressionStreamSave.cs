using System;
using System.IO;
using System.IO.Compression;

namespace VRage
{
	public class MyCompressionStreamSave : IMyCompressionSave, IDisposable
	{
		private MemoryStream m_output;

		private GZipStream m_gz;

		private BufferedStream m_buffer;

		public MyCompressionStreamSave()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			m_output = new MemoryStream();
			m_output.Write(BitConverter.GetBytes(0), 0, 4);
			m_gz = new GZipStream((Stream)m_output, (CompressionMode)1);
			m_buffer = new BufferedStream((Stream)(object)m_gz, 16384);
		}

		public byte[] Compress()
		{
			byte[] result = null;
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
				try
				{
					m_output.Close();
				}
				finally
				{
					result = m_output.ToArray();
					m_output = null;
				}
			}
			return result;
		}

		public void Dispose()
		{
			Compress();
		}

		public void Add(byte[] value)
		{
			((Stream)(object)m_buffer).Write(value, 0, value.Length);
		}

		public void Add(byte[] value, int count)
		{
			((Stream)(object)m_buffer).Write(value, 0, count);
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
		}
	}
}
