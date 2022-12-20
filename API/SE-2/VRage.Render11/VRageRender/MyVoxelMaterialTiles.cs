using System.Collections.Generic;
using VRage.Render11.Resources.Textures;

namespace VRageRender
{
	internal class MyVoxelMaterialTiles
	{
		public IMyStreamedTextureArrayTile[] ColorMetalXZnY;

		public IMyStreamedTextureArrayTile[] ColorMetalY;

		public IMyStreamedTextureArrayTile[] NormalGlossXZnY;

		public IMyStreamedTextureArrayTile[] NormalGlossY;

		public IMyStreamedTextureArrayTile[] ExtXZnY;

		public IMyStreamedTextureArrayTile[] ExtY;

		public IEnumerable<IMyStreamedTextureArrayTile> GetAllTiles()
		{
			IMyStreamedTextureArrayTile[] colorMetalXZnY = ColorMetalXZnY;
			for (int i = 0; i < colorMetalXZnY.Length; i++)
			{
				yield return colorMetalXZnY[i];
			}
			colorMetalXZnY = ColorMetalY;
			for (int i = 0; i < colorMetalXZnY.Length; i++)
			{
				yield return colorMetalXZnY[i];
			}
			colorMetalXZnY = NormalGlossXZnY;
			for (int i = 0; i < colorMetalXZnY.Length; i++)
			{
				yield return colorMetalXZnY[i];
			}
			colorMetalXZnY = NormalGlossY;
			for (int i = 0; i < colorMetalXZnY.Length; i++)
			{
				yield return colorMetalXZnY[i];
			}
			colorMetalXZnY = ExtXZnY;
			for (int i = 0; i < colorMetalXZnY.Length; i++)
			{
				yield return colorMetalXZnY[i];
			}
			colorMetalXZnY = ExtY;
			for (int i = 0; i < colorMetalXZnY.Length; i++)
			{
				yield return colorMetalXZnY[i];
			}
		}

		public bool AreLoaded()
		{
			if (!AreComponentTilesLoaded(ColorMetalXZnY))
			{
				return false;
			}
			if (!AreComponentTilesLoaded(ColorMetalY))
			{
				return false;
			}
			if (!AreComponentTilesLoaded(NormalGlossXZnY))
			{
				return false;
			}
			if (!AreComponentTilesLoaded(NormalGlossY))
			{
				return false;
			}
			if (!AreComponentTilesLoaded(ExtXZnY))
			{
				return false;
			}
			if (!AreComponentTilesLoaded(ExtY))
			{
				return false;
			}
			return true;
		}

		private static bool AreComponentTilesLoaded(IMyStreamedTextureArrayTile[] tiles)
		{
			for (int i = 0; i < tiles.Length; i++)
			{
				if (!tiles[i].IsLoaded)
				{
					return false;
				}
			}
			return true;
		}
	}
}
