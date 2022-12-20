using System;
using System.IO;
using System.IO.Compression;

namespace VRage
{
	public class MyCompressionStreamLoad : IMyCompressionLoad
	{
		private static byte[] m_intBytesBuffer = new byte[4];

		private MemoryStream m_input;

		private GZipStream m_gz;

		private BufferedStream m_buffer;

		public MyCompressionStreamLoad(byte[] compressedData)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			m_input = new MemoryStream(compressedData);
			m_input.Read(m_intBytesBuffer, 0, 4);
			m_gz = new GZipStream((Stream)m_input, (CompressionMode)0);
			m_buffer = new BufferedStream((Stream)(object)m_gz, 16384);
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
	}
}
