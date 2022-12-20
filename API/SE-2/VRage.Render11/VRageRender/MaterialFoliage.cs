using VRage;
using VRage.Render11.Resources;

namespace VRageRender
{
	internal struct MaterialFoliage
	{
		public const int MaxTextures = 16;

		public MyFoliageType Type;

		public float Density;

		public IFileArrayTexture ColorTextureArray;

		public IFileArrayTexture NormalTextureArray;

		public MaterialFoliageData Data;
	}
}
