using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal struct MyGeometryTextureSystemReference
	{
		public bool IsUsed;

		public IDynamicFileArrayTexture ColorMetalTexture;

		public IDynamicFileArrayTexture NormalGlossTexture;

		public IDynamicFileArrayTexture ExtensionTexture;

		public IDynamicFileArrayTexture AlphamaskTexture;

		public int ColorMetalIndex;

		public int NormalGlossIndex;

		public int ExtensionIndex;

		public int AlphamaskIndex;

		public Vector4I TextureSliceIndices
		{
			get
			{
				return new Vector4I(ColorMetalIndex, NormalGlossIndex, ExtensionIndex, AlphamaskIndex);
			}
			set
			{
				ColorMetalIndex = value.X;
				NormalGlossIndex = value.Y;
				ExtensionIndex = value.Z;
				AlphamaskIndex = value.W;
			}
		}
	}
}
