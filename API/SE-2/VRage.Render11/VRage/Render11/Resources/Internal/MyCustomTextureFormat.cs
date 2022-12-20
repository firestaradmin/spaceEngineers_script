using System;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	[GenerateActivator]
	internal class MyCustomTextureFormat : IRtvTexture, ISrvTexture, ISrvBindable, IResource, ITexture, IRtvBindable
	{
		private class VRage_Render11_Resources_Internal_MyCustomTextureFormat_003C_003EActor : IActivator, IActivator<MyCustomTextureFormat>
		{
			private sealed override object CreateInstance()
			{
				return new MyCustomTextureFormat();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCustomTextureFormat CreateInstance()
			{
				return new MyCustomTextureFormat();
			}

			MyCustomTextureFormat IActivator<MyCustomTextureFormat>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyCustomTexture m_owner;

		private Format m_format;

		private ShaderResourceView m_srv;

		private RenderTargetView m_rtv;

		public ShaderResourceView Srv => m_srv;

		public RenderTargetView Rtv => m_rtv;

		public SharpDX.Direct3D11.Resource Resource => m_owner.Resource;

		public string Name => m_owner.Name;

		public Vector2I Size => m_owner.Size;

		public Vector3I Size3 => m_owner.Size3;

		public int MipLevels => 1;

		public Format Format => m_format;

		public event Action<ITexture> OnFormatChanged;

		public void Init(MyCustomTexture owner, Format format)
		{
			m_owner = owner;
			m_format = format;
			this.OnFormatChanged.InvokeIfNotNull(this);
		}

		public void OnDeviceInit()
		{
			ShaderResourceViewDescription description = default(ShaderResourceViewDescription);
			description.Format = m_format;
			description.Dimension = ShaderResourceViewDimension.Texture2D;
			description.Texture2D.MipLevels = 1;
			description.Texture2D.MostDetailedMip = 0;
			m_srv = new ShaderResourceView(MyRender11.DeviceInstance, m_owner.Resource, description);
			m_srv.DebugName = m_owner.Name;
			RenderTargetViewDescription description2 = default(RenderTargetViewDescription);
			description2.Format = m_format;
			description2.Dimension = RenderTargetViewDimension.Texture2D;
			description2.Texture2D.MipSlice = 0;
			m_rtv = new RenderTargetView(MyRender11.DeviceInstance, m_owner.Resource, description2);
			m_rtv.DebugName = m_owner.Name;
		}

		public void OnDeviceEnd()
		{
			m_srv.Dispose();
			m_srv = null;
			m_rtv.Dispose();
			m_rtv = null;
		}
	}
}
