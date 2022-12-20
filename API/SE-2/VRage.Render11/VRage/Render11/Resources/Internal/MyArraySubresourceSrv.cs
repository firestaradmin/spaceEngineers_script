using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	internal class MyArraySubresourceSrv : MyArraySubresourceInternal, ISrvArraySubresource, ISrvBindable, IResource
	{
		public ShaderResourceView Srv { get; private set; }

		public void OnDeviceInit()
		{
			ShaderResourceViewDescription shaderResourceViewDescription = default(ShaderResourceViewDescription);
			shaderResourceViewDescription.Format = m_format;
			shaderResourceViewDescription.Dimension = ShaderResourceViewDimension.Texture2DArray;
			shaderResourceViewDescription.Texture2DArray.ArraySize = 1;
			shaderResourceViewDescription.Texture2DArray.FirstArraySlice = m_slice;
			shaderResourceViewDescription.Texture2DArray.MipLevels = 1;
			shaderResourceViewDescription.Texture2DArray.MostDetailedMip = m_mipLevel;
			ShaderResourceViewDescription description = shaderResourceViewDescription;
			Srv = new ShaderResourceView(MyRender11.DeviceInstance, m_owner.Resource, description);
		}

		public void OnDeviceEnd()
		{
			if (Srv != null)
			{
				Srv.Dispose();
				Srv = null;
			}
		}
	}
}
