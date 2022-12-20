using System;
using VRage.Render11.Scene.Resources;

namespace VRage.Render11.Resources.Textures
{
	internal interface IMyStreamedTextureArrayTile : IMyStreamedTextureBase, IMySceneResource
	{
		IDynamicFileArrayTexture TextureArray { get; }

		bool IsLoaded { get; }

		int TileID { get; }

		string Filepath { get; }

		event Action<IMyStreamedTextureArrayTile> OnTextureTileHandleChanged;
<<<<<<< HEAD

		void ResetOnTextureTileHandleChangedEvent();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
