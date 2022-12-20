using VRageRender.Import;

namespace VRageRender
{
	internal struct MySubmeshInfo
	{
		internal int IndexCount;

		internal int StartIndex;

		internal int BaseVertex;

		internal int[] BonesMapping;

		internal MyMeshDrawTechnique Technique;

		internal string Material;
	}
}
