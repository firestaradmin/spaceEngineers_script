using System;
using SharpDX.DXGI;

namespace VRage.Render11.Resources
{
	internal interface ITexture : ISrvBindable, IResource
	{
		Format Format { get; }

		int MipLevels { get; }

		event Action<ITexture> OnFormatChanged;
	}
}
