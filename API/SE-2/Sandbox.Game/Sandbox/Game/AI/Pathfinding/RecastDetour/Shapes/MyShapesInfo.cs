using System.Collections.Generic;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour.Shapes
{
	public class MyShapesInfo
	{
		public List<MyBoxShapeInfo> Boxes { get; set; } = new List<MyBoxShapeInfo>();


		public List<MySphereShapeInfo> Spheres { get; set; } = new List<MySphereShapeInfo>();


		public List<MyConvexVerticesInfo> ConvexVertices { get; set; } = new List<MyConvexVerticesInfo>();

	}
}
