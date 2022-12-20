using System;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources.Textures
{
	[GenerateActivator]
	internal abstract class MyTextureInternal : ISrvTexture, ISrvBindable, IResource, ITexture
	{
		private ShaderResourceView m_srv;

		protected SharpDX.Direct3D11.Resource m_resource;

		protected string m_name;

		protected Vector2I m_size;

		protected Format m_resourceFormat;

		protected Format m_srvFormat;

		protected BindFlags m_bindFlags;

		protected int m_samplesCount;

		protected int m_samplesQuality;

		protected ResourceOptionFlags m_roFlags;

		protected ResourceUsage m_resourceUsage;

		protected int m_mipLevels;

		protected CpuAccessFlags m_cpuAccessFlags;

		public ShaderResourceView Srv => m_srv;

		public SharpDX.Direct3D11.Resource Resource => m_resource;

		public string Name => m_name;

		public Vector2I Size => m_size;

		public Vector3I Size3 => new Vector3I(m_size.X, m_size.Y, 1);

		public Format Format => m_resourceFormat;

		public int MipLevels => m_mipLevels;

		public event Action<ITexture> OnFormatChanged;

		public void Init(string name, int width, int height, Format resourceFormat, Format srvFormat, BindFlags bindFlags, int samplesCount, int samplesQuality, ResourceOptionFlags roFlags, ResourceUsage ru, int mipLevels, CpuAccessFlags cpuAccessFlags)
		{
			m_name = name;
			m_size = new Vector2I(width, height);
			m_resourceFormat = resourceFormat;
			m_srvFormat = srvFormat;
			m_bindFlags = bindFlags;
			m_samplesCount = samplesCount;
			m_samplesQuality = samplesQuality;
			m_roFlags = roFlags;
			m_resourceUsage = ru;
			m_mipLevels = mipLevels;
			m_cpuAccessFlags = cpuAccessFlags;
			this.OnFormatChanged.InvokeIfNotNull(this);
		}

		public void OnDeviceInitInternal()
		{
			Texture2DDescription description = default(Texture2DDescription);
			description.Width = Size.X;
			description.Height = Size.Y;
			description.Format = m_resourceFormat;
			description.ArraySize = 1;
			description.MipLevels = m_mipLevels;
			description.BindFlags = m_bindFlags;
			description.Usage = m_resourceUsage;
			description.CpuAccessFlags = m_cpuAccessFlags;
			description.SampleDescription.Count = m_samplesCount;
			description.SampleDescription.Quality = m_samplesQuality;
			description.OptionFlags = m_roFlags;
			m_resource = new Texture2D(MyRender11.DeviceInstance, description);
			ShaderResourceViewDescription description2 = default(ShaderResourceViewDescription);
			description2.Format = m_srvFormat;
			description2.Dimension = ShaderResourceViewDimension.Texture2D;
			description2.Texture2D.MipLevels = m_mipLevels;
			description2.Texture2D.MostDetailedMip = 0;
			m_srv = new ShaderResourceView(MyRender11.DeviceInstance, m_resource, description2);
			m_resource.DebugName = m_name;
			m_srv.DebugName = m_name;
		}

		public void OnDeviceEndInternal()
		{
			if (m_srv != null)
			{
				m_srv.Dispose();
				m_srv = null;
			}
			if (m_resource != null)
			{
				m_resource.Dispose();
				m_resource = null;
			}
		}
	}
}
