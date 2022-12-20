using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Generics;
using VRage.Library.Memory;
using VRage.Network;
using VRage.Render11.Common;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	[GenerateActivator]
	internal class MyDepthStencil : IDepthStencil, IResource
	{
		private class VRage_Render11_Resources_Internal_MyDepthStencil_003C_003EActor : IActivator, IActivator<MyDepthStencil>
		{
			private sealed override object CreateInstance()
			{
				return new MyDepthStencil();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDepthStencil CreateInstance()
			{
				return new MyDepthStencil();
			}

			MyDepthStencil IActivator<MyDepthStencil>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyObjectsPool<MyDepthStencilSrv> m_objectsPoolSrvs = new MyObjectsPool<MyDepthStencilSrv>(32);

		private Format m_resourceFormat;

		private Format m_srvDepthFormat;

		private Format m_srvStencilFormat;

		private int m_samplesCount;

		private int m_samplesQuality;

		private readonly MyDepthStencilSrv m_srvDepth;

		private readonly MyDepthStencilSrv m_srvStencil;

		private MyMemorySystem.AllocationRecord m_allocationRecord;

		public string Name { get; private set; }

		public Vector2I Size { get; private set; }

		public Vector3I Size3 => new Vector3I(Size.X, Size.Y, 1);

		public SharpDX.Direct3D11.Resource Resource { get; private set; }

		public IDsvBindable Dsv { get; private set; }

		public IDsvBindable DsvRoDepth { get; private set; }

		public IDsvBindable DsvRoStencil { get; private set; }

		public IDsvBindable DsvRo { get; private set; }

		public ISrvBindable SrvDepth => m_srvDepth;

		public ISrvBindable SrvStencil => m_srvStencil;

		public Format Format { get; private set; }

		public MyDepthStencil()
		{
			m_objectsPoolSrvs.AllocateOrCreate(out m_srvDepth);
			m_objectsPoolSrvs.AllocateOrCreate(out m_srvStencil);
		}

		~MyDepthStencil()
		{
			m_objectsPoolSrvs.Deallocate(m_srvDepth);
			m_objectsPoolSrvs.Deallocate(m_srvStencil);
		}

		public void Init(string debugName, int width, int height, Format resourceFormat, Format dsvFormat, Format srvDepthFormat, Format srvStencilFormat, int samplesNum, int samplesQuality)
		{
			Name = debugName;
			Size = new Vector2I(width, height);
			m_resourceFormat = resourceFormat;
			Format = dsvFormat;
			m_srvDepthFormat = srvDepthFormat;
			m_srvStencilFormat = srvStencilFormat;
			m_samplesCount = samplesNum;
			m_samplesQuality = samplesQuality;
		}

		public void OnDeviceInit()
		{
			Texture2DDescription texture2DDescription = default(Texture2DDescription);
			texture2DDescription.Width = Size.X;
			texture2DDescription.Height = Size.Y;
			texture2DDescription.Format = m_resourceFormat;
			texture2DDescription.ArraySize = 1;
			texture2DDescription.MipLevels = 1;
			texture2DDescription.BindFlags = BindFlags.ShaderResource | BindFlags.DepthStencil;
			texture2DDescription.Usage = ResourceUsage.Default;
			texture2DDescription.CpuAccessFlags = CpuAccessFlags.None;
			texture2DDescription.SampleDescription.Count = m_samplesCount;
			texture2DDescription.SampleDescription.Quality = m_samplesQuality;
			texture2DDescription.OptionFlags = ResourceOptionFlags.None;
			Texture2DDescription description = texture2DDescription;
			Resource = new Texture2D(MyRender11.DeviceInstance, description)
			{
				DebugName = Name
			};
			long textureByteSize = MyResourceUtils.GetTextureByteSize(Size3, description.MipLevels, description.Format);
			m_allocationRecord = MyManagers.DepthStencils.MemoryTracker.RegisterAllocation(Name, textureByteSize);
			DepthStencilViewDescription depthStencilViewDescription = default(DepthStencilViewDescription);
			depthStencilViewDescription.Format = Format;
			depthStencilViewDescription.Dimension = ((m_samplesCount == 1) ? DepthStencilViewDimension.Texture2D : DepthStencilViewDimension.Texture2DMultisampled);
			DepthStencilViewDescription description2 = depthStencilViewDescription;
			Dsv = new MyDsvBindable(this, new DepthStencilView(MyRender11.DeviceInstance, Resource, description2));
			description2.Flags = DepthStencilViewFlags.ReadOnlyDepth;
			DsvRoDepth = new MyDsvBindable(this, new DepthStencilView(MyRender11.DeviceInstance, Resource, description2));
			description2.Flags = DepthStencilViewFlags.ReadOnlyStencil;
			DsvRoStencil = new MyDsvBindable(this, new DepthStencilView(MyRender11.DeviceInstance, Resource, description2));
			description2.Flags = DepthStencilViewFlags.ReadOnlyDepth | DepthStencilViewFlags.ReadOnlyStencil;
			DsvRo = new MyDsvBindable(this, new DepthStencilView(MyRender11.DeviceInstance, Resource, description2));
			ShaderResourceViewDescription shaderResourceViewDescription = default(ShaderResourceViewDescription);
			shaderResourceViewDescription.Format = m_srvDepthFormat;
			ShaderResourceViewDescription desc = shaderResourceViewDescription;
			if (m_samplesCount == 1)
			{
				desc.Dimension = ShaderResourceViewDimension.Texture2D;
				desc.Texture2D.MipLevels = -1;
				desc.Texture2D.MostDetailedMip = 0;
			}
			else
			{
				desc.Dimension = ShaderResourceViewDimension.Texture2DMultisampled;
			}
			m_srvDepth.OnDeviceInit(this, desc);
			desc.Format = m_srvStencilFormat;
			if (m_samplesCount == 1)
			{
				desc.Dimension = ShaderResourceViewDimension.Texture2D;
				desc.Texture2D.MipLevels = -1;
				desc.Texture2D.MostDetailedMip = 0;
			}
			else
			{
				desc.Dimension = ShaderResourceViewDimension.Texture2DMultisampled;
			}
			m_srvStencil.OnDeviceInit(this, desc);
		}

		public void OnDeviceEnd()
		{
			if (Resource != null)
			{
				m_allocationRecord.Dispose();
				Resource.Dispose();
				Resource = null;
				((MyDsvBindable)Dsv).Dispose();
				((MyDsvBindable)DsvRoDepth).Dispose();
				((MyDsvBindable)DsvRoStencil).Dispose();
				((MyDsvBindable)DsvRo).Dispose();
				m_srvDepth.OnDeviceEnd();
				m_srvStencil.OnDeviceEnd();
			}
		}
	}
}
