using System;
using BulletXNA.LinearMath;

namespace BulletXNA.BulletCollision
{
	public struct AABB
	{
		public IndexedVector3 m_min;

		public IndexedVector3 m_max;

		public AABB(ref IndexedVector3 min, ref IndexedVector3 max)
		{
			m_min = min;
			m_max = max;
		}

		public AABB(IndexedVector3 min, IndexedVector3 max)
		{
			m_min = min;
			m_max = max;
		}

		public void Invalidate()
		{
			m_min.X = float.MaxValue;
			m_min.Y = float.MaxValue;
			m_min.Z = float.MaxValue;
			m_max.X = float.MinValue;
			m_max.Y = float.MinValue;
			m_max.Z = float.MinValue;
		}

		public void Merge(AABB box)
		{
			Merge(ref box);
		}

		public void Merge(ref AABB box)
		{
			m_min.X = BoxCollision.BT_MIN(m_min.X, box.m_min.X);
			m_min.Y = BoxCollision.BT_MIN(m_min.Y, box.m_min.Y);
			m_min.Z = BoxCollision.BT_MIN(m_min.Z, box.m_min.Z);
			m_max.X = BoxCollision.BT_MAX(m_max.X, box.m_max.X);
			m_max.Y = BoxCollision.BT_MAX(m_max.Y, box.m_max.Y);
			m_max.Z = BoxCollision.BT_MAX(m_max.Z, box.m_max.Z);
		}

		public void GetCenterExtend(out IndexedVector3 center, out IndexedVector3 extend)
		{
			center = new IndexedVector3((m_max + m_min) * 0.5f);
			extend = new IndexedVector3(m_max - center);
		}

		public bool CollideRay(ref IndexedVector3 vorigin, ref IndexedVector3 vdir)
		{
			GetCenterExtend(out var center, out var extend);
			float num = vorigin.X - center.X;
			if (BoxCollision.BT_GREATER(num, extend.X) && num * vdir.X >= 0f)
			{
				return false;
			}
			float num2 = vorigin.Y - center.Y;
			if (BoxCollision.BT_GREATER(num2, extend.Y) && num2 * vdir.Y >= 0f)
			{
				return false;
			}
			float num3 = vorigin.Z - center.Z;
			if (BoxCollision.BT_GREATER(num3, extend.Z) && num3 * vdir.Z >= 0f)
			{
				return false;
			}
			float value = vdir.Y * num3 - vdir.Z * num2;
			if (Math.Abs(value) > extend.Y * Math.Abs(vdir.Z) + extend.Z * Math.Abs(vdir.Y))
			{
				return false;
			}
			value = vdir.Z * num - vdir.X * num3;
			if (Math.Abs(value) > extend.X * Math.Abs(vdir.Z) + extend.Z * Math.Abs(vdir.X))
			{
				return false;
			}
			value = vdir.X * num2 - vdir.Y * num;
			if (Math.Abs(value) > extend.X * Math.Abs(vdir.Y) + extend.Y * Math.Abs(vdir.X))
			{
				return false;
			}
			return true;
		}

		public float? CollideRayDistance(ref IndexedVector3 origin, ref IndexedVector3 direction)
		{
			IndexedVector3 indexedVector = new IndexedVector3(1f / direction.X, 1f / direction.Y, 1f / direction.Z);
			float val = (m_min.X - origin.X) * indexedVector.X;
			float val2 = (m_max.X - origin.X) * indexedVector.X;
			float val3 = (m_min.Y - origin.Y) * indexedVector.Y;
			float val4 = (m_max.Y - origin.Y) * indexedVector.Y;
			float val5 = (m_min.Z - origin.Z) * indexedVector.Z;
			float val6 = (m_max.Z - origin.Z) * indexedVector.Z;
			float num = Math.Max(Math.Max(Math.Min(val, val2), Math.Min(val3, val4)), Math.Min(val5, val6));
			float num2 = Math.Min(Math.Min(Math.Max(val, val2), Math.Max(val3, val4)), Math.Max(val5, val6));
			if (num2 < 0f)
			{
				float num3 = num2;
				return null;
			}
			if (num > num2)
			{
				float num3 = num2;
				return null;
			}
			return num;
		}
	}
}
