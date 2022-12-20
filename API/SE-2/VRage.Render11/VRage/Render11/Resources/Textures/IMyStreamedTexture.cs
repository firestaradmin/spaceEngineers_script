using System;
using VRage.Render11.Scene.Resources;

namespace VRage.Render11.Resources.Textures
{
	internal interface IMyStreamedTexture : IMyStreamedTextureBase, IMySceneResource
	{
		ITexture Texture { get; }

		event Action<IMyStreamedTexture> OnTextureHandleChanged;

		MyStreamedTexturePin Pin();
	}
}
