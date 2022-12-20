using VRage.Import;
using VRageMath;
using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatNormalTangent
	{
		internal Byte4 Normal;

		internal Byte4 Tangent;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatNormalTangent);

		internal MyVertexFormatNormalTangent(Vector3 normal, Vector4 tangent)
		{
			Normal = VF_Packer.PackNormalB4(ref normal);
			Tangent = VF_Packer.PackTangentSignB4(ref tangent);
		}

		internal MyVertexFormatNormalTangent(Vector3 normal, Vector3 tangent)
		{
			Normal = VF_Packer.PackNormalB4(ref normal);
			Vector4 tangentW = new Vector4(tangent, 1f);
			Tangent = VF_Packer.PackTangentSignB4(ref tangentW);
		}

		internal MyVertexFormatNormalTangent(Byte4 normal, Byte4 tangent)
		{
			Normal = normal;
			Tangent = tangent;
		}
	}
}
