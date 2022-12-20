using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes
{
	public class MyBoxShapeInfo
	{
		public Matrix RdWorldMatrix { get; set; }

		public float HalfExtentsX { get; set; }

		public float HalfExtentsY { get; set; }

		public float HalfExtentsZ { get; set; }

		public MyBoxShapeInfo(Matrix rdWorldTranslation, float halfExtentsX, float halfExtentsY, float halfExtentsZ)
		{
			RdWorldMatrix = rdWorldTranslation;
			HalfExtentsX = halfExtentsX;
			HalfExtentsY = halfExtentsY;
			HalfExtentsZ = halfExtentsZ;
		}
	}
}
