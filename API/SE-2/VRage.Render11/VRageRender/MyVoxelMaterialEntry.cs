using VRage;
using VRageMath;

namespace VRageRender
{
	internal struct MyVoxelMaterialEntry
	{
		public MyRenderVoxelMaterialData.TilingSetup SimpleTilingSetup;

		public MyRenderVoxelMaterialData.TilingSetup StandardTilingSetup;

		public Vector3 Far3Color;

		public bool FullQuality;

		public Vector4I SliceNear1;

		public Vector4I SliceNear2;

		public Vector4I SliceFar1;

		public Vector4I SliceFar2;

		public Vector4I SliceFar21;

		public Vector4I SliceFar22;
	}
}
