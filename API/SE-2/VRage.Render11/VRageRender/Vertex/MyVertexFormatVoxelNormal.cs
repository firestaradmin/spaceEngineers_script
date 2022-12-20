using System.Runtime.InteropServices;
using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct MyVertexFormatVoxelNormal
	{
		[FieldOffset(0)]
		internal Byte4 Normal;

		[FieldOffset(4)]
		public uint ColorShift;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatVoxelNormal);

		internal MyVertexFormatVoxelNormal(Byte4 normal, uint colorShift)
		{
			Normal = normal;
			ColorShift = colorShift;
		}
	}
}
