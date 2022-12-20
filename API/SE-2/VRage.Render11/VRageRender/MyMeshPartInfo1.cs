using System.Collections.Generic;
using VRageMath;

namespace VRageRender
{
	internal struct MyMeshPartInfo1
	{
		internal int IndexCount;

		internal int StartIndex;

		internal int BaseVertex;

		internal int SectionSubmeshCount;

		internal int[] BonesMapping;

		internal Vector3 CenterOffset;

		internal MyMeshMaterialId Material;

		public HashSet<string> UnusedMaterials;

		public bool IsValid()
		{
			if (IndexCount > 0 && BaseVertex >= 0)
			{
				return StartIndex >= 0;
			}
			return false;
		}
	}
}
