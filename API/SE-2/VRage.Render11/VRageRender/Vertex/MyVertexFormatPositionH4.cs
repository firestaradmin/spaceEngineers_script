using VRage.Import;
using VRageMath;
using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatPositionH4
	{
		internal HalfVector4 Position;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatPositionH4);

		internal MyVertexFormatPositionH4(HalfVector4 position)
		{
			Position = position;
		}

		internal MyVertexFormatPositionH4(Vector3 position)
		{
			Position = VF_Packer.PackPosition(ref position);
		}
	}
}
