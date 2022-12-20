using VRageMath;
using VRageRender.Voxels;

namespace VRageRender
{
	internal struct MyLodMeshInfo
	{
		internal string Name;

		internal string FileName;

		internal int PartsNum;

		internal string[] SectionNames;

		internal int VerticesNum;

		internal int IndicesNum;

		internal int TrianglesNum;

		/// <summary>Triangle density in the squared diagonal</summary>
		internal float TriangleDensity;

		internal float LodDistance;

		internal MyMeshRawData Data;

		internal MyVoxelMeshPartIndex[] PartOffsets;

		/// <summary>
		/// Summary offset where multimaterial parts start.
		///
		/// While we keep meshes organized by materials we draw all single material at once,
		/// we similarly draw all multi material at once.
		/// </summary>
		internal int MultimaterialOffset;

		internal BoundingBox? BoundingBox;

		internal BoundingSphere? BoundingSphere;

		internal bool NullLodMesh;

		internal bool HasBones => Data.VertexLayout.Info.HasBonesInfo;
	}
}
