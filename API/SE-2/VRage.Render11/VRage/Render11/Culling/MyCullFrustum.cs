using VRage.Render11.Common;
using VRageMath;

namespace VRage.Render11.Culling
{
	internal struct MyCullFrustum
	{
		private readonly Plane[] m_planes;

		private unsafe fixed int m_planeIndices[6];

		public unsafe MyCullFrustum(BoundingFrustum frustum)
		{
			m_planes = frustum.Planes;
			for (int i = 0; i < 6; i++)
			{
				m_planeIndices[i] = (((double)m_planes[i].Normal.X >= 0.0) ? 4 : 0) | (((double)m_planes[i].Normal.Y >= 0.0) ? 2 : 0) | (((double)m_planes[i].Normal.Z >= 0.0) ? 1 : 0);
			}
		}

		public unsafe bool Contains(MyCullAABB aabb)
		{
			for (int i = 0; i < 6; i++)
			{
				Vector3 normal = m_planes[i].Normal;
				Vector3 vector = aabb.Data[m_planeIndices[i]];
				if ((double)(normal.X * vector.X + normal.Y * vector.Y + normal.Z * vector.Z + m_planes[i].D) > 0.0)
				{
					return false;
				}
			}
			return true;
		}

		public unsafe ContainmentType Intersects(MyCullAABB aabb)
		{
			for (int i = 0; i < 6; i++)
			{
				Vector3 normal = m_planes[i].Normal;
				Vector3 vector = aabb.Data[m_planeIndices[i]];
				if ((double)(normal.X * vector.X + normal.Y * vector.Y + normal.Z * vector.Z + m_planes[i].D) > 0.0)
				{
					return ContainmentType.Disjoint;
				}
				Vector3 vector2 = aabb.Data[16 + m_planeIndices[i]];
				if ((double)(normal.X * vector2.X + normal.Y * vector2.Y + normal.Z * vector2.Z + m_planes[i].D) >= 0.0)
				{
					return ContainmentType.Intersects;
				}
			}
			return ContainmentType.Contains;
		}
	}
}
