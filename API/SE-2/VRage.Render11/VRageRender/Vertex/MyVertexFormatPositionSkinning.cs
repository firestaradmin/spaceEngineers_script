using VRageMath;
using VRageMath.PackedVector;

namespace VRageRender.Vertex
{
	internal struct MyVertexFormatPositionSkinning
	{
		internal HalfVector4 Position;

		internal HalfVector4 BoneWeights;

		internal Byte4 BoneIndices;

		internal unsafe static int STRIDE = sizeof(MyVertexFormatPositionSkinning);

		internal MyVertexFormatPositionSkinning(HalfVector4 position, Byte4 indices, Vector4 weights)
		{
			Position = position;
			BoneIndices = indices;
			BoneWeights = new HalfVector4(weights);
		}
	}
}
