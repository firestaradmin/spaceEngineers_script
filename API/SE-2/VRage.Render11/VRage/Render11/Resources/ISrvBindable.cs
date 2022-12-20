using SharpDX.Direct3D11;

namespace VRage.Render11.Resources
{
	internal interface ISrvBindable : IResource
	{
		ShaderResourceView Srv { get; }
	}
}
