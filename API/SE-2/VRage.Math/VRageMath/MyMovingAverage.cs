using System;
using System.Collections.Generic;

namespace VRageMath
{
	public class MyMovingAverage
	{
		private readonly Queue<float> m_queue = new Queue<float>();

		private readonly int m_windowSize;

		private int m_enqueueCounter;

		private readonly int m_enqueueCountToReset;

		public float Avg
		{
			get
			{
				if (m_queue.get_Count() <= 0)
				{
					return 0f;
				}
				return (float)Sum / (float)m_queue.get_Count();
			}
		}

		public double Sum { get; private set; }

		public MyMovingAverage(int windowSize, int enqueueCountToReset = 1000)
		{
			m_windowSize = windowSize;
			m_enqueueCountToReset = enqueueCountToReset;
		}

		private void UpdateSum()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Sum = 0.0;
			Enumerator<float> enumerator = m_queue.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					float current = enumerator.get_Current();
					Sum += current;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void Enqueue(float value)
		{
			m_queue.Enqueue(value);
			m_enqueueCounter++;
			if (m_enqueueCounter > m_enqueueCountToReset)
			{
				m_enqueueCounter = 0;
				UpdateSum();
			}
			else
			{
				Sum += value;
			}
			while (m_queue.get_Count() > m_windowSize)
			{
				float num = m_queue.Dequeue();
				Sum -= num;
			}
		}

		public void Reset()
		{
			Sum = 0.0;
			m_queue.Clear();
		}
	}
}
