using System.Runtime.InteropServices;
using VRageRender.Vertex;

namespace VRageRender
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct MyBillboardVertexData
	{
		public MyVertexFormatPositionTextureH V0;

		public MyVertexFormatPositionTextureH V1;

		public MyVertexFormatPositionTextureH V2;

		public MyVertexFormatPositionTextureH V3;
	}
}
