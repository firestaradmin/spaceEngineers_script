using System;
using BulletXNA.LinearMath;

namespace BulletXNA.BulletCollision
{
	public struct BT_QUANTIZED_BVH_NODE
	{
		public UShortVector3 m_quantizedAabbMin;

		public UShortVector3 m_quantizedAabbMax;

		public byte m_size;

		public unsafe fixed int m_escapeIndexOrDataIndex[6];

		public unsafe bool IsLeafNode()
		{
			return m_escapeIndexOrDataIndex[0] >= 0;
		}

		public unsafe int GetEscapeIndex()
		{
			return -m_escapeIndexOrDataIndex[0];
		}

		public unsafe void SetEscapeIndex(int index)
		{
			m_size = 1;
			m_escapeIndexOrDataIndex[0] = -index;
		}

		public unsafe void SetDataIndices(Span<int> indices)
		{
			m_size = (byte)indices.Length;
			for (int i = 0; i < m_size; i++)
			{
				m_escapeIndexOrDataIndex[i] = indices[i];
			}
		}

		public bool TestQuantizedBoxOverlapp(ref UShortVector3 quantizedMin, ref UShortVector3 quantizedMax)
		{
			if (m_quantizedAabbMin.X > quantizedMax.X || m_quantizedAabbMax.X < quantizedMin.X || m_quantizedAabbMin.Y > quantizedMax.Y || m_quantizedAabbMax.Y < quantizedMin.Y || m_quantizedAabbMin.Z > quantizedMax.Z || m_quantizedAabbMax.Z < quantizedMin.Z)
			{
				return false;
			}
			return true;
		}
	}
}
