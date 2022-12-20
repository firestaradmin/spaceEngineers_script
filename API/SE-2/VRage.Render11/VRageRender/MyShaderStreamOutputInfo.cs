using SharpDX.Direct3D11;

namespace VRageRender
{
	internal struct MyShaderStreamOutputInfo
	{
		internal StreamOutputElement[] Elements;

		internal int[] Strides;

		internal int RasterizerStreams;
	}
}
