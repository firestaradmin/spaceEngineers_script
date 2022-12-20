using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatGenericInstance
	{
		internal HalfVector4 row0;

		internal HalfVector4 row1;

		internal HalfVector4 row2;

		internal HalfVector4 colorMaskHSV;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatGenericInstance);
	}
}
