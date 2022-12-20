using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	internal class MyArraySubresourceDepth : MyArraySubresourceInternal, IDsvBindable, IResource
	{
		public DepthStencilView Dsv { get; private set; }

		public void OnDeviceInit(bool readOnly = false)
		{
			DepthStencilViewDescription depthStencilViewDescription = default(DepthStencilViewDescription);
			depthStencilViewDescription.Format = m_format;
			depthStencilViewDescription.Flags = (readOnly ? ((m_format == Format.D32_Float) ? DepthStencilViewFlags.ReadOnlyDepth : (DepthStencilViewFlags.ReadOnlyDepth | DepthStencilViewFlags.ReadOnlyStencil)) : DepthStencilViewFlags.None);
			depthStencilViewDescription.Dimension = DepthStencilViewDimension.Texture2DArray;
			depthStencilViewDescription.Texture2DArray.ArraySize = 1;
			depthStencilViewDescription.Texture2DArray.FirstArraySlice = m_slice;
			depthStencilViewDescription.Texture2DArray.MipSlice = m_mipLevel;
			DepthStencilViewDescription description = depthStencilViewDescription;
			Dsv = new DepthStencilView(MyRender11.DeviceInstance, m_owner.Resource, description)
			{
				DebugName = m_owner.Name
			};
		}

		public void OnDeviceEnd()
		{
			if (Dsv != null)
			{
				Dsv.Dispose();
				Dsv = null;
			}
		}
	}
}
