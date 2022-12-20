using VRageMath;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatCubeInstance
	{
		internal unsafe fixed byte bones[32];

		internal Vector4 translationRotation;

		internal Vector4 colorMaskHSV;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatCubeInstance);
	}
}
