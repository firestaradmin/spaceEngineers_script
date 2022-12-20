using System.Threading;

namespace VRage.Render11.GeometryStage2.Common
{
	internal class MyIDGenerator
	{
		private int m_lastUsedID;

		private int m_highestID;

		public int Generate()
		{
			return Interlocked.Increment(ref m_lastUsedID);
		}

		public void UpdateHighestID()
		{
			m_highestID = m_lastUsedID;
		}

		public int GetHighestID()
		{
			return m_highestID;
		}

		public void Reset()
		{
			m_lastUsedID = (m_highestID = 0);
		}
	}
}
