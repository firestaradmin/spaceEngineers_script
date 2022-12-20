using System;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.Resources.Internal
{
	internal class MyGeneratedTexture : IGeneratedTexture, ITexture, ISrvBindable, IResource, IRtvBindable
	{
		private string m_name;

		private Vector2I m_size;

		private SharpDX.Direct3D11.Resource m_resource;

		private ShaderResourceView m_srv;

		private RenderTargetView m_rtv;

		private Texture2DDescription m_desc;

		private bool m_isGeneratingMipmaps;

		public MyGeneratedTextureType Type => m_desc.Format.ToGeneratedTextureType();

		public int MipLevels => m_desc.MipLevels;

		public ShaderResourceView Srv => m_srv;

		public SharpDX.Direct3D11.Resource Resource => m_resource;

		public string Name => m_name;

		public Format Format => m_desc.Format;

		public Vector2I Size => m_size;

		public Vector3I Size3 => new Vector3I(m_size.X, m_size.Y, 1);

		public RenderTargetView Rtv => m_rtv;

		public event Action<ITexture> OnFormatChanged;

		internal void Init(string name, Texture2DDescription desc, Vector2I size, bool isGeneratingMipmaps)
		{
			m_name = name;
			m_size = size;
			m_desc = desc;
			m_isGeneratingMipmaps = isGeneratingMipmaps;
			this.OnFormatChanged.InvokeIfNotNull(this);
		}

		protected internal void Reset(DataBox[] dataBoxes)
		{
			Dispose();
			m_resource = new Texture2D(MyRender11.DeviceInstance, m_desc, dataBoxes)
			{
				DebugName = m_name
			};
			m_srv = new ShaderResourceView(MyRender11.DeviceInstance, m_resource)
			{
				DebugName = m_name
			};
			if (m_isGeneratingMipmaps)
			{
				m_rtv = new RenderTargetView(MyRender11.DeviceInstance, m_resource)
				{
					DebugName = m_name
				};
				if (dataBoxes != null)
				{
					MyRender11.RC.GenerateMips(this);
				}
			}
		}

		protected internal void Dispose()
		{
			if (m_resource != null)
			{
				m_resource.Dispose();
				m_resource = null;
			}
			if (m_srv != null)
			{
				m_srv.Dispose();
				m_srv = null;
			}
			if (m_rtv != null)
			{
				m_rtv.Dispose();
				m_rtv = null;
			}
		}
	}
}
