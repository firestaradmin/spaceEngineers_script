using System;

namespace VRageRender
{
	internal struct MyDrawSubmesh
	{
		[Flags]
		internal enum MySubmeshFlags
		{
			None = 0x0,
			Gbuffer = 0x1,
			Depth = 0x2,
			Forward = 0x4,
			All = 0x7
		}

		internal int IndexCount;

		internal int StartIndex;

		internal int BaseVertex;

		internal MyMaterialProxyId MaterialId;

		internal int[] BonesMapping;

		internal MySubmeshFlags Flags;

		internal MyDrawSubmesh(int indexCount, int startIndex, int baseVertex, MyMaterialProxyId materialId, int[] bonesMapping = null, MySubmeshFlags flags = MySubmeshFlags.All)
		{
			IndexCount = indexCount;
			StartIndex = startIndex;
			BaseVertex = baseVertex;
			MaterialId = materialId;
			BonesMapping = bonesMapping;
			Flags = flags;
		}
	}
}
