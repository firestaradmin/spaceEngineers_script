using SharpDX.Direct3D11;

namespace VRage.Render11.Resources
{
	internal interface IRasterizerStateInternal : IRasterizerState, IMyPersistentResource<RasterizerStateDescription>
	{
		RasterizerState Resource { get; }
	}
}
