namespace VRage.Library.Collections
{
	public struct BitReaderWriter
	{
		private IBitSerializable m_writeData;

		private BitStream m_readStream;

		private long m_readStreamPosition;

		public readonly bool IsReading;

		public BitReaderWriter(IBitSerializable writeData)
		{
			m_writeData = writeData;
			m_readStream = null;
			m_readStreamPosition = 0L;
			IsReading = false;
		}

		private BitReaderWriter(BitStream readStream, long readPos)
		{
			m_writeData = null;
			m_readStream = readStream;
			m_readStreamPosition = readPos;
			IsReading = true;
		}

		public static BitReaderWriter ReadFrom(BitStream stream)
		{
			uint num = stream.ReadUInt32Variant();
			BitReaderWriter result = new BitReaderWriter(stream, stream.BitPosition);
			stream.SetBitPositionRead(stream.BitPosition + (int)num);
			return result;
		}

		public void Write(BitStream stream)
		{
			if (stream == null || m_writeData == null)
			{
				_ = m_writeData;
				return;
			}
			long bitPosition = stream.BitPosition;
			m_writeData.Serialize(stream, validate: false);
			long num = stream.BitPosition - bitPosition;
			stream.SetBitPositionWrite(bitPosition);
			stream.WriteVariant((uint)num);
			m_writeData.Serialize(stream, validate: false);
		}

		/// <summary>
		/// Returns false when validation was required and failed, otherwise true.
		/// </summary>
		public bool ReadData(IBitSerializable readDataInto, bool validate, bool acceptAndSetValue = true)
		{
			if (m_readStream == null)
			{
				return false;
			}
			long bitPosition = m_readStream.BitPosition;
			m_readStream.SetBitPositionRead(m_readStreamPosition);
			try
			{
				return readDataInto.Serialize(m_readStream, validate, acceptAndSetValue);
			}
			finally
			{
				m_readStream.SetBitPositionRead(bitPosition);
			}
		}
	}
}
