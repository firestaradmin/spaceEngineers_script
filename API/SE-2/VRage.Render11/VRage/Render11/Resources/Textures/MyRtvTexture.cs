using SharpDX.Direct3D11;
using VRage.Network;
using VRageRender;

namespace VRage.Render11.Resources.Textures
{
	internal class MyRtvTexture : MyTextureInternal, IRtvTexture, ISrvTexture, ISrvBindable, IResource, ITexture, IRtvBindable
	{
		private class VRage_Render11_Resources_Textures_MyRtvTexture_003C_003EActor : IActivator, IActivator<MyRtvTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyRtvTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRtvTexture CreateInstance()
			{
				return new MyRtvTexture();
			}

			MyRtvTexture IActivator<MyRtvTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private RenderTargetView m_rtv;

		public RenderTargetView Rtv => m_rtv;

		public void OnDeviceInit()
		{
			OnDeviceInitInternal();
			m_rtv = new RenderTargetView(MyRender11.DeviceInstance, m_resource);
			m_rtv.DebugName = m_name;
		}

		public void OnDeviceEnd()
		{
			if (m_rtv != null)
			{
				m_rtv.Dispose();
				m_rtv = null;
			}
			OnDeviceEndInternal();
		}
	}
}
