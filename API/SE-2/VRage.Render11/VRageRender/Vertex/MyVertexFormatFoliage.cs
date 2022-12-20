using System.Runtime.InteropServices;
using VRageMath;

namespace VRageRender.Vertex
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct MyVertexFormatFoliage
	{
		[FieldOffset(0)]
		public Vector3 Position;

		[FieldOffset(12)]
		private uint NormalSeed;

		[FieldOffset(16)]
		private uint ColorShift;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatFoliage);
	}
}
