using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Network;
using VRage.Render11.Resources.Internal;
using VRageRender;

namespace VRage.Render11.Resources.Textures
{
	internal class MyDepthTexture : MyTextureInternal, IDepthTexture, ISrvTexture, ISrvBindable, IResource, ITexture, IDsvBindable
	{
		private class VRage_Render11_Resources_Textures_MyDepthTexture_003C_003EActor : IActivator, IActivator<MyDepthTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyDepthTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDepthTexture CreateInstance()
			{
				return new MyDepthTexture();
			}

			MyDepthTexture IActivator<MyDepthTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private Format m_dsvFormat;

		private MyDsvBindable m_readOnlyDsv;

		public DepthStencilView Dsv { get; private set; }

		public IDsvBindable DsvRo => m_readOnlyDsv;

		public void Init(string name, int width, int height, Format resourceFormat, Format srvFormat, Format dsvFormat, BindFlags bindFlags, int samplesCount, int samplesQuality, ResourceOptionFlags roFlags, ResourceUsage ru, int mipLevels, CpuAccessFlags cpuAccessFlags)
		{
			Init(name, width, height, resourceFormat, srvFormat, bindFlags, samplesCount, samplesQuality, roFlags, ru, mipLevels, cpuAccessFlags);
			m_dsvFormat = dsvFormat;
		}

		public void OnDeviceInit()
		{
			OnDeviceInitInternal();
			DepthStencilViewDescription depthStencilViewDescription = default(DepthStencilViewDescription);
			depthStencilViewDescription.Format = m_dsvFormat;
			depthStencilViewDescription.Dimension = DepthStencilViewDimension.Texture2D;
			depthStencilViewDescription.Texture2D.MipSlice = 0;
			DepthStencilViewDescription description = depthStencilViewDescription;
			Dsv = new DepthStencilView(MyRender11.DeviceInstance, m_resource, description)
			{
				DebugName = m_name
			};
			description.Flags = DepthStencilViewFlags.ReadOnlyDepth | ((m_dsvFormat != Format.D32_Float) ? DepthStencilViewFlags.ReadOnlyStencil : DepthStencilViewFlags.None);
			DepthStencilView dsv = new DepthStencilView(MyRender11.DeviceInstance, base.Resource, description)
			{
				DebugName = base.Name
			};
			m_readOnlyDsv = new MyDsvBindable(this, dsv);
		}

		public void OnDeviceEnd()
		{
			if (Dsv != null)
			{
				Dsv.Dispose();
				Dsv = null;
			}
			if (m_readOnlyDsv != null)
			{
				m_readOnlyDsv.Dispose();
				m_readOnlyDsv = null;
			}
			OnDeviceEndInternal();
		}
	}
}
