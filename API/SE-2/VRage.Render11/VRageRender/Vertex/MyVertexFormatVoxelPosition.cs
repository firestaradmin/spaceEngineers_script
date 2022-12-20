using System.Runtime.InteropServices;
using VRageMath;
using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct MyVertexFormatVoxelPosition
	{
		[FieldOffset(0)]
		public Vector3 Position;

		[FieldOffset(12)]
		public Byte4 Material;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatVoxelPosition);
	}
}
