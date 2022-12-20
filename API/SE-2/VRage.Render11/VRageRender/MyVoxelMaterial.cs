using VRage;
using VRageMath;

namespace VRageRender
{
	internal struct MyVoxelMaterial
	{
		internal MyVoxelMaterialDetailSet Resource;

		public Boxed<MaterialFoliage> Foliage;

		internal MyRenderVoxelMaterialData.TilingSetup StandardTilingSetup;

		internal MyRenderVoxelMaterialData.TilingSetup SimpleTilingSetup;

		internal Color Far3Color;

		internal bool HasFoliage => Foliage != null;
	}
}
