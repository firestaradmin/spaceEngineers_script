using VRageMath;
using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatPositionSkinningTexcoord
	{
		internal HalfVector4 Position;

		internal HalfVector4 BoneWeights;

		internal Byte4 BoneIndices;

		internal HalfVector2 Texcoord;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatPositionSkinningTexcoord);

		internal MyVertexFormatPositionSkinningTexcoord(HalfVector4 position, Byte4 indices, Vector4 weights, HalfVector2 texcoord)
		{
			Position = position;
			BoneIndices = indices;
			BoneWeights = new HalfVector4(weights);
			Texcoord = texcoord;
		}
	}
}
