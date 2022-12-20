using VRage.Render11.Scene.Resources;

namespace VRage.Render11.Resources.Textures
{
	internal interface IMyStreamedTextureBase : IMySceneResource
	{
		int TextureTokenId { get; }

		uint Size { get; }

		void OnStreamingPriorityChangedFromZero(int flags);

		void Touch(ushort priority = 100);
	}
}
