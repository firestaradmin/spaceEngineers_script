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
	internal class MyCustomTexture : ICustomTexture, IUavBindable, IResource
	{
		private class VRage_Render11_Resources_Internal_MyCustomTexture_003C_003EActor : IActivator, IActivator<MyCustomTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyCustomTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCustomTexture CreateInstance()
			{
				return new MyCustomTexture();
			}

			MyCustomTexture IActivator<MyCustomTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static MyObjectsPool<MyCustomTextureFormat> m_objectsPoolFormats = new MyObjectsPool<MyCustomTextureFormat>(32);

		private string m_name;

		private Vector2I m_size;

		private Format m_resourceFormat;

		private int m_samplesCount;

		private int m_samplesQuality;

		private SharpDX.Direct3D11.Resource m_resource;

		private MyCustomTextureFormat m_formatSRgb;

		private MyCustomTextureFormat m_formatLinear;

		private UnorderedAccessView m_uav;

		private MyMemorySystem.AllocationRecord m_allocationRecord;

		public string Name => m_name;

		public Vector2I Size => m_size;

		public Vector3I Size3 => new Vector3I(m_size.X, m_size.Y, 1);

		public int MipLevels => 1;

		public SharpDX.Direct3D11.Resource Resource => m_resource;

		public IRtvTexture SRgb => m_formatSRgb;

		public IRtvTexture Linear => m_formatLinear;

		public UnorderedAccessView Uav => m_uav;

		public void Init(string debugName, int width, int height, int samplesNum, int samplesQuality)
		{
			m_name = debugName;
			m_size = new Vector2I(width, height);
			m_resourceFormat = Format.R8G8B8A8_Typeless;
			m_samplesCount = samplesNum;
			m_samplesQuality = samplesQuality;
		}

		public MyCustomTexture()
		{
			m_objectsPoolFormats.AllocateOrCreate(out m_formatSRgb);
			m_objectsPoolFormats.AllocateOrCreate(out m_formatLinear);
		}

		~MyCustomTexture()
		{
			m_objectsPoolFormats.Deallocate(m_formatSRgb);
			m_objectsPoolFormats.Deallocate(m_formatLinear);
		}

		public void OnDeviceInit()
		{
			Texture2DDescription texture2DDescription = default(Texture2DDescription);
			texture2DDescription.Width = Size.X;
			texture2DDescription.Height = Size.Y;
			texture2DDescription.Format = m_resourceFormat;
			texture2DDescription.ArraySize = 1;
			texture2DDescription.MipLevels = 1;
			texture2DDescription.BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget | BindFlags.UnorderedAccess;
			texture2DDescription.Usage = ResourceUsage.Default;
			texture2DDescription.CpuAccessFlags = CpuAccessFlags.None;
			texture2DDescription.SampleDescription.Count = m_samplesCount;
			texture2DDescription.SampleDescription.Quality = m_samplesQuality;
			texture2DDescription.OptionFlags = ResourceOptionFlags.None;
			Texture2DDescription description = texture2DDescription;
			m_resource = new Texture2D(MyRender11.DeviceInstance, description);
			m_resource.DebugName = Name;
			m_formatSRgb.Init(this, Format.R8G8B8A8_UNorm_SRgb);
			m_formatSRgb.OnDeviceInit();
			m_formatLinear.Init(this, Format.R8G8B8A8_UNorm);
			m_formatLinear.OnDeviceInit();
			m_uav = new UnorderedAccessView(MyRender11.DeviceInstance, m_resource, new UnorderedAccessViewDescription
			{
				Format = Format.R8G8B8A8_UNorm,
				Dimension = UnorderedAccessViewDimension.Texture2D
			});
			m_uav.DebugName = Name;
			long textureByteSize = MyResourceUtils.GetTextureByteSize(Size3, description.MipLevels, description.Format);
			m_allocationRecord = MyManagers.CustomTextures.MemoryTracker.RegisterAllocation(Name, textureByteSize);
		}

		public void OnDeviceEnd()
		{
			m_allocationRecord.Dispose();
			m_resource.Dispose();
			m_resource = null;
			m_formatSRgb.OnDeviceEnd();
			m_formatLinear.OnDeviceEnd();
			m_uav.Dispose();
			m_uav = null;
		}
	}
}
