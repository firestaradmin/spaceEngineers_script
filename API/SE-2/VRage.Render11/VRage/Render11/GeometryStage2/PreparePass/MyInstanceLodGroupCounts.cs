using System;
using VRage.Render11.GeometryStage2.Common;

namespace VRage.Render11.GeometryStage2.PreparePass
{
	internal struct MyInstanceLodGroupCounts
	{
		private int[] m_instancesPerGroup;

		private int m_usedGroupsCount;

		private int m_maxPossibleLodId;

		private static readonly int m_instanceLodStatesCount = Enum.GetValues(typeof(MyInstanceLodState)).Length;

		public int GetDirectOffset(int lodId, MyInstanceLodState state)
		{
			return (m_maxPossibleLodId + 1) * (int)state + lodId;
		}

		public void PrepareByZeroes(int maxLodId)
		{
			m_maxPossibleLodId = maxLodId;
			m_usedGroupsCount = (m_maxPossibleLodId + 1) * m_instanceLodStatesCount;
			if (m_instancesPerGroup == null || m_instancesPerGroup.Length < m_usedGroupsCount)
			{
				m_instancesPerGroup = new int[m_usedGroupsCount];
			}
			for (int i = 0; i < m_usedGroupsCount; i++)
			{
				m_instancesPerGroup[i] = 0;
			}
		}

		public void PrepareByNegativeOnes(int maxLodId)
		{
			m_maxPossibleLodId = maxLodId;
			m_usedGroupsCount = (m_maxPossibleLodId + 1) * m_instanceLodStatesCount;
			if (m_instancesPerGroup == null || m_instancesPerGroup.Length < m_usedGroupsCount)
			{
				m_instancesPerGroup = new int[m_usedGroupsCount];
			}
			for (int i = 0; i < m_usedGroupsCount; i++)
			{
				m_instancesPerGroup[i] = -1;
			}
		}

		public bool IsEmpty(int lodId, MyInstanceLodState state)
		{
			int directOffset = GetDirectOffset(lodId, state);
			return m_instancesPerGroup[directOffset] == 0;
		}

		public int At(int lodId, MyInstanceLodState state)
		{
			int directOffset = GetDirectOffset(lodId, state);
			return m_instancesPerGroup[directOffset];
		}

		public void SetAt(int lodId, MyInstanceLodState state, int value)
		{
			int directOffset = GetDirectOffset(lodId, state);
			m_instancesPerGroup[directOffset] = value;
		}

		public void Inc(int lodId, MyInstanceLodState state)
		{
			int directOffset = GetDirectOffset(lodId, state);
			m_instancesPerGroup[directOffset]++;
		}

		public int AtByDirectOffset(int directOffset)
		{
			return m_instancesPerGroup[directOffset];
		}

		public void SetAtDirectOffset(int directOffset, int value)
		{
			m_instancesPerGroup[directOffset] = value;
		}

		public int GetDirectOffsetsCount()
		{
			return m_usedGroupsCount;
		}

		public void GetDetailsFromDirectOffset(int directOffset, out int lodId, out MyInstanceLodState state)
		{
			lodId = directOffset % (m_maxPossibleLodId + 1);
			state = (MyInstanceLodState)(directOffset / (m_maxPossibleLodId + 1));
		}
	}
}
