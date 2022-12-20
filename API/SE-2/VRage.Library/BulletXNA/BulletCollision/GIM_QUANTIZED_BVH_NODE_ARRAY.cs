using BulletXNA.LinearMath;

namespace BulletXNA.BulletCollision
{
	public class GIM_QUANTIZED_BVH_NODE_ARRAY : ObjectArray<BT_QUANTIZED_BVH_NODE>
	{
		public GIM_QUANTIZED_BVH_NODE_ARRAY()
		{
		}

		public GIM_QUANTIZED_BVH_NODE_ARRAY(int capacity)
			: base(capacity)
		{
		}
	}
}
