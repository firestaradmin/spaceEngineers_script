using VRageMath;

namespace Sandbox.Graphics
{
	public struct MyTextureAtlasItem
	{
		/// <summary>
		/// The Texture2D that this item is part of
		/// </summary>
		public string AtlasTexture;

		/// <summary>
		/// The UVOffsets describe where this item
		/// sits in the AtlasTexture. The four components
		/// are U offset, V offset, Width and Height
		/// </summary>
		public Vector4 UVOffsets;

		public MyTextureAtlasItem(string atlasTex, Vector4 uvOffsets)
		{
			AtlasTexture = atlasTex;
			UVOffsets = uvOffsets;
		}
	}
}
