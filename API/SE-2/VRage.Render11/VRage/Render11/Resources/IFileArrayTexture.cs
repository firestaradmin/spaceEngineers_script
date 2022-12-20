using System.Collections.Generic;

namespace VRage.Render11.Resources
{
	internal interface IFileArrayTexture : ITexture, ISrvBindable, IResource
	{
		int SubTexturesCount { get; }

		IEnumerable<string> SubTextures { get; }

		bool IsLowMipMapVersion { get; }

		MyFileTextureEnum Type { get; }

		void Update(bool isDeviceInit);

		void AddSlices(IEnumerable<(string Path, ITexture texture)> textures);

		void SwapSlice(int sliceIndex, string path, ITexture texture);
	}
}
