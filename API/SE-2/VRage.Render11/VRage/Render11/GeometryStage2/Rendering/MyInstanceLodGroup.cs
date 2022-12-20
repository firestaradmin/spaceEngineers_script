using VRage.Render11.GeometryStage2.Common;
using VRage.Render11.GeometryStage2.Model;

namespace VRage.Render11.GeometryStage2.Rendering
{
	internal struct MyInstanceLodGroup
	{
		public MyLod Lod;

		public MyLodInstance LodInstance;

		public MyInstanceLodState State;

		public bool MetalnessColorable;

		public int OffsetInInstanceBuffer;

		public int InstancesIncrement;

		public int InstancesCount;

		public int InstanceMaterialsCount;
	}
}
