using VRage.Import;
using VRageMath;
using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatTexcoordNormalTangent
	{
		internal Byte4 Normal;

		internal Byte4 Tangent;

		internal HalfVector2 Texcoord;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatTexcoordNormalTangent);

		internal MyVertexFormatTexcoordNormalTangent(HalfVector2 texcoord, Vector3 normal, Vector4 tangent)
		{
			Texcoord = texcoord;
			Normal = VF_Packer.PackNormalB4(ref normal);
			Tangent = VF_Packer.PackTangentSignB4(ref tangent);
		}

		internal MyVertexFormatTexcoordNormalTangent(Vector2 texcoord, Vector3 normal, Vector3 tangent)
		{
			Texcoord = new HalfVector2(texcoord.X, texcoord.Y);
			Normal = VF_Packer.PackNormalB4(ref normal);
			Vector4 tangentW = new Vector4(tangent, 1f);
			Tangent = VF_Packer.PackTangentSignB4(ref tangentW);
		}

		internal MyVertexFormatTexcoordNormalTangent(HalfVector2 texcoord, Byte4 normal, Byte4 tangent)
		{
			Texcoord = texcoord;
			Normal = normal;
			Tangent = tangent;
		}
	}
}
