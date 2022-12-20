using System.Runtime.InteropServices;
using VRageMath;
using VRageMath.PackedVector;

namespace VRage.Voxels
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct VrVoxelVertex
	{
		public Vector3I Cell;

		public Vector3 Position;

		public Vector3 Normal;

		public Byte4 Color;

		public byte Material;
	}
}
