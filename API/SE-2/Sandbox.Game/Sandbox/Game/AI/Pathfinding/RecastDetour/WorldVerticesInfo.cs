using VRage.Library.Collections;
using VRageMath;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
	public class WorldVerticesInfo
	{
		public MyList<Vector3> Vertices = new MyList<Vector3>();

		public int VerticesMaxValue;

		public MyList<int> Triangles = new MyList<int>();
	}
}
