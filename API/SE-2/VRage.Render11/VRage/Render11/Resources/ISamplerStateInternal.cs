using SharpDX.Direct3D11;

namespace VRage.Render11.Resources
{
	internal interface ISamplerStateInternal : ISamplerState, IMyPersistentResource<SamplerStateDescription>
	{
		SamplerState Resource { get; }
	}
}
