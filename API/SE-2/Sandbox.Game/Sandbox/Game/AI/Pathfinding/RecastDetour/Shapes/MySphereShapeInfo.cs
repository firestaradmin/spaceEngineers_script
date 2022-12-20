using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes
{
	public class MySphereShapeInfo
	{
		public Vector3 RdWorldTranslation { get; set; }

		public float Radius { get; set; }

		public MySphereShapeInfo(Vector3 rdWorldTranslation, float radius)
		{
			RdWorldTranslation = rdWorldTranslation;
			Radius = radius;
		}
	}
}
