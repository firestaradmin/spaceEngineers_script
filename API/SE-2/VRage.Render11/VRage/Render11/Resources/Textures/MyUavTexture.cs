using SharpDX.Direct3D11;
using VRage.Network;
using VRageRender;

namespace VRage.Render11.Resources.Textures
{
	internal class MyUavTexture : MyTextureInternal, IUavTexture, IRtvTexture, ISrvTexture, ISrvBindable, IResource, ITexture, IRtvBindable, IUavBindable
	{
		private class VRage_Render11_Resources_Textures_MyUavTexture_003C_003EActor : IActivator, IActivator<MyUavTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyUavTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyUavTexture CreateInstance()
			{
				return new MyUavTexture();
			}

			MyUavTexture IActivator<MyUavTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private RenderTargetView m_rtv;

		private UnorderedAccessView m_uav;

		public RenderTargetView Rtv => m_rtv;

		public UnorderedAccessView Uav => m_uav;

		public void OnDeviceInit()
		{
			OnDeviceInitInternal();
			m_rtv = new RenderTargetView(MyRender11.DeviceInstance, m_resource);
			m_rtv.DebugName = m_name;
			m_uav = new UnorderedAccessView(MyRender11.DeviceInstance, m_resource);
			m_uav.DebugName = m_name;
		}

		public void OnDeviceEnd()
		{
			if (m_rtv != null)
			{
				m_rtv.Dispose();
				m_rtv = null;
			}
			if (m_uav != null)
			{
				m_uav.Dispose();
				m_uav = null;
			}
			OnDeviceEndInternal();
		}
	}
}
