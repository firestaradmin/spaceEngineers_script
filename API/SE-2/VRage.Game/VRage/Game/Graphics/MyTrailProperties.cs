using VRage.Utils;
using VRageMath;

namespace VRage.Game.Graphics
{
	public class MyTrailProperties
	{
		public Vector3D Position = Vector3D.Zero;

		public Vector3D Normal = Vector3D.Zero;

		public Vector3D ForwardDirection = Vector3D.Zero;

		public long EntityId;

		public MyStringHash PhysicalMaterial;

		public MyStringHash VoxelMaterial;
	}
}
