using VRageRender.Voxels;

namespace VRageRender
{
	internal struct MyVoxelPartInfo1
	{
		internal int IndexCount;

		internal int StartIndex;

		internal int BaseVertex;

		public MyVoxelMaterialTriple MaterialTriple;

		public bool TriplanarMulti => ((MaterialTriple.I0 != byte.MaxValue) ? 1 : 0) + ((MaterialTriple.I1 != byte.MaxValue) ? 1 : 0) + ((MaterialTriple.I2 != byte.MaxValue) ? 1 : 0) > 1;
	}
}
