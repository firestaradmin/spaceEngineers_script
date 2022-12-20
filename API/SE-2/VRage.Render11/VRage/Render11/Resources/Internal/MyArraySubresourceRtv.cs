using SharpDX.Direct3D11;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	internal class MyArraySubresourceRtv : MyArraySubresourceInternal, IRtvArraySubresource, IRtvBindable, IResource
	{
		public RenderTargetView Rtv { get; private set; }

		public void OnDeviceInit()
		{
			RenderTargetViewDescription description = default(RenderTargetViewDescription);
			description.Format = m_format;
			description.Dimension = RenderTargetViewDimension.Texture2DArray;
			description.Texture2DArray.ArraySize = 1;
			description.Texture2DArray.FirstArraySlice = m_slice;
			description.Texture2DArray.MipSlice = m_mipLevel;
			Rtv = new RenderTargetView(MyRender11.DeviceInstance, m_owner.Resource, description);
		}

		public void OnDeviceEnd()
		{
			if (Rtv != null)
			{
				Rtv.Dispose();
				Rtv = null;
			}
		}
	}
}
