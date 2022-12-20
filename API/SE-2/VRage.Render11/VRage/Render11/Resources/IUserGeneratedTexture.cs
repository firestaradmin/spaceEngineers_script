using VRageRender.Messages;

namespace VRage.Render11.Resources
{
	internal interface IUserGeneratedTexture : IGeneratedTexture, ITexture, ISrvBindable, IResource, IRtvBindable, IAsyncTexture
	{
		MyGeneratedTextureType Type { get; }

		void Reset(byte[] data = null);

		void SetTextureReady();
	}
}
