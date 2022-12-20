using BulletXNA.LinearMath;

namespace BulletXNA.BulletCollision
{
	public class GImpactQuantizedBvh
	{
		private QuantizedBvhTree m_box_tree;

		private IPrimitiveManagerBase m_primitive_manager;

		private int m_size;

		public int Size => m_size;

		public byte[] Save()
		{
			return m_box_tree.Save();
		}

		public void Load(byte[] byteArray)
		{
			m_box_tree.Load(byteArray);
			m_size = byteArray.Length;
		}

		public GImpactQuantizedBvh()
		{
			m_box_tree = new QuantizedBvhTree();
		}

		public GImpactQuantizedBvh(IPrimitiveManagerBase primitive_manager)
		{
			m_primitive_manager = primitive_manager;
			m_box_tree = new QuantizedBvhTree();
		}

		public void BuildSet()
		{
			int primitiveCount = m_primitive_manager.GetPrimitiveCount();
			GIM_BVH_DATA_ARRAY gIM_BVH_DATA_ARRAY = new GIM_BVH_DATA_ARRAY(primitiveCount);
			gIM_BVH_DATA_ARRAY.Resize(primitiveCount);
			GIM_BVH_DATA[] rawArray = gIM_BVH_DATA_ARRAY.GetRawArray();
			for (int i = 0; i < primitiveCount; i++)
			{
				m_primitive_manager.GetPrimitiveBox(i, out rawArray[i].m_bound);
				rawArray[i].m_data = i;
			}
			m_box_tree.BuildTree(gIM_BVH_DATA_ARRAY);
		}

		public unsafe bool BoxQuery(ref AABB box, ProcessHandler handler)
		{
			int num = 0;
			int nodeCount = GetNodeCount();
			m_box_tree.QuantizePoint(out var quantizedpoint, ref box.m_min);
			m_box_tree.QuantizePoint(out var quantizedpoint2, ref box.m_max);
			while (num < nodeCount)
			{
				bool flag = m_box_tree.TestQuantizedBoxOverlap(num, ref quantizedpoint, ref quantizedpoint2);
				bool flag2 = IsLeafNode(num);
				if (flag2 && flag)
				{
					BT_QUANTIZED_BVH_NODE node = GetNode(num);
					for (int i = 0; i < node.m_size; i++)
					{
						if (handler(node.m_escapeIndexOrDataIndex[i]))
						{
							return true;
						}
					}
				}
				num = ((!(flag || flag2)) ? (num + GetEscapeNodeIndex(num)) : (num + 1));
			}
			return false;
		}

		public unsafe bool RayQueryClosest(ref IndexedVector3 ray_dir, ref IndexedVector3 ray_origin, ProcessCollisionHandler handler)
		{
			int num = 0;
			int nodeCount = GetNodeCount();
			float num2 = float.PositiveInfinity;
			while (num < nodeCount)
			{
				GetNodeBound(num, out var bound);
				float? num3 = bound.CollideRayDistance(ref ray_origin, ref ray_dir);
				bool flag = IsLeafNode(num);
				bool flag2 = num3.HasValue && num3.Value < num2;
				if (flag2 && flag)
				{
					BT_QUANTIZED_BVH_NODE node = GetNode(num);
					for (int i = 0; i < node.m_size; i++)
					{
						float? num4 = handler(node.m_escapeIndexOrDataIndex[i]);
						if (num4.HasValue && num4.Value < num2)
						{
							num2 = num4.Value;
						}
					}
				}
				num = ((!(flag2 || flag)) ? (num + GetEscapeNodeIndex(num)) : (num + 1));
			}
			return num2 != float.PositiveInfinity;
		}

		public unsafe bool RayQuery(ref IndexedVector3 ray_dir, ref IndexedVector3 ray_origin, ProcessCollisionHandler handler)
		{
			int num = 0;
			int nodeCount = GetNodeCount();
			bool result = false;
			while (num < nodeCount)
			{
				GetNodeBound(num, out var bound);
				bool flag = bound.CollideRay(ref ray_origin, ref ray_dir);
				bool flag2 = IsLeafNode(num);
				if (flag2 && flag)
				{
					BT_QUANTIZED_BVH_NODE node = GetNode(num);
					for (int i = 0; i < node.m_size; i++)
					{
						handler(node.m_escapeIndexOrDataIndex[i]);
						result = true;
					}
				}
				num = ((!(flag || flag2)) ? (num + GetEscapeNodeIndex(num)) : (num + 1));
			}
			return result;
		}

		private int GetNodeCount()
		{
			return m_box_tree.GetNodeCount();
		}

		private bool IsLeafNode(int nodeindex)
		{
			return m_box_tree.IsLeafNode(nodeindex);
		}

		private BT_QUANTIZED_BVH_NODE GetNode(int nodeindex)
		{
			return m_box_tree.GetNode(nodeindex);
		}

		private void GetNodeBound(int nodeindex, out AABB bound)
		{
			m_box_tree.GetNodeBound(nodeindex, out bound);
		}

		private int GetEscapeNodeIndex(int nodeindex)
		{
			return m_box_tree.GetEscapeNodeIndex(nodeindex);
		}
	}
}
