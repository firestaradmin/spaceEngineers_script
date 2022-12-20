using System;

namespace VRage.Library.Utils
{
	public class MyValueAggregator
	{
		private readonly int m_bufferSize;

		private readonly int[] m_percentiles;

		private readonly double[] m_data;

		private readonly double[] m_percentileSums;

		private int m_dataSize;

		private int m_flushNumber;

		private readonly object m_lock = new object();

		public double[] PercentileValues
		{
			get
			{
				double[] array = new double[m_percentiles.Length];
				GetPercentileValues(array);
				return array;
			}
		}

		public bool HasData
		{
			get
			{
				lock (m_lock)
				{
					return m_flushNumber > 0 || CanForceFlush;
				}
			}
		}

		private bool CanForceFlush => m_dataSize > m_bufferSize * 100 / 20;

		public MyValueAggregator(int bufferSize, params int[] percentiles)
		{
			if (percentiles.Length == 0)
			{
				throw new ArgumentException("at least one percentile should be specified", "percentiles");
			}
			foreach (int num in percentiles)
			{
				if (num < 0 || num > 100)
				{
					throw new ArgumentOutOfRangeException("percentiles", "percentile should have value in range [0, 100]");
				}
			}
			if (bufferSize < percentiles.Length)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "should not be less than number of percentiles");
			}
			m_bufferSize = bufferSize;
			m_percentiles = percentiles;
			m_data = new double[m_bufferSize];
			m_percentileSums = new double[m_percentiles.Length];
		}

		public void Push(double value)
		{
			lock (m_lock)
			{
				m_data[m_dataSize] = value;
				m_dataSize++;
				if (m_dataSize == m_bufferSize)
				{
					Flush();
				}
			}
		}

		public void GetPercentileValues(double[] valuesBuffer)
		{
			if (valuesBuffer == null)
			{
				throw new ArgumentNullException("valuesBuffer");
			}
			if (valuesBuffer.Length != m_percentiles.Length)
			{
				throw new ArgumentOutOfRangeException("valuesBuffer", "should be exact same length as percentiles array");
			}
			lock (m_lock)
			{
				if (CanForceFlush)
				{
					Flush();
				}
				double num = 1.0 / (double)m_flushNumber;
				for (int i = 0; i < m_percentileSums.Length; i++)
				{
					valuesBuffer[i] = m_percentileSums[i] * num;
				}
			}
		}

		private void Flush()
		{
			Array.Sort(m_data, 0, m_dataSize);
			for (int i = 0; i < m_percentiles.Length; i++)
			{
				int num = m_percentiles[i] * (m_dataSize - 1) / 100;
				m_percentileSums[i] += m_data[num];
			}
			m_flushNumber++;
			m_dataSize = 0;
		}

		public void Clear()
		{
			m_flushNumber = 0;
			m_dataSize = 0;
			Array.Clear(m_percentileSums, 0, m_percentileSums.Length);
		}
	}
}
