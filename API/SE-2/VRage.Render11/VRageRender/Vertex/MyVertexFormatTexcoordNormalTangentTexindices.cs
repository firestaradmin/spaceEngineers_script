using VRage.Import;
using VRageMath;
using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatTexcoordNormalTangentTexindices
	{
		internal Byte4 Normal;

		internal Byte4 Tangent;

		internal HalfVector2 Texcoord;

		internal Byte4 TexIndices;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatTexcoordNormalTangentTexindices);

		internal MyVertexFormatTexcoordNormalTangentTexindices(Vector2 texcoord, Vector3 normal, Vector3 tangent, Byte4 texIndices)
		{
			Texcoord = new HalfVector2(texcoord.X, texcoord.Y);
			Normal = VF_Packer.PackNormalB4(ref normal);
			Vector4 tangentW = new Vector4(tangent, 1f);
			Tangent = VF_Packer.PackTangentSignB4(ref tangentW);
			TexIndices = texIndices;
		}
	}
}
