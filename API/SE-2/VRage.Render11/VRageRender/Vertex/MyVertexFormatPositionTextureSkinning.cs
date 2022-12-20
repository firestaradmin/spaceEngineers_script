using VRageMath;
using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatPositionTextureSkinning
	{
		internal HalfVector4 Position;

		internal HalfVector2 Texcoord;

		internal Byte4 BoneIndices;

		internal HalfVector4 BoneWeights;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatPositionTextureSkinning);

		internal MyVertexFormatPositionTextureSkinning(HalfVector4 position, HalfVector2 texcoord, Byte4 indices, Vector4 weights)
		{
			Position = position;
			Texcoord = texcoord;
			BoneIndices = indices;
			BoneWeights = new HalfVector4(weights);
		}
	}
}
