using SharpDX.Direct3D11;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	[GenerateActivator]
	internal class MyArrayTextureResource : ISrvBindable, IResource
	{
		private class VRage_Render11_Resources_Internal_MyArrayTextureResource_003C_003EActor : IActivator, IActivator<MyArrayTextureResource>
		{
			private sealed override object CreateInstance()
			{
				return new MyArrayTextureResource();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyArrayTextureResource CreateInstance()
			{
				return new MyArrayTextureResource();
			}

			MyArrayTextureResource IActivator<MyArrayTextureResource>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected string m_name;

		protected Texture2DDescription m_resourceDesc;

		protected ShaderResourceViewDescription m_srvDesc;

		private MyArraySubresourceSrv[,] m_arraySubresourcesSrv;

		public Resource Resource { get; private set; }

		public string Name => m_name;

		public Vector2I Size => new Vector2I(m_resourceDesc.Width, m_resourceDesc.Height);

		public Vector3I Size3 => new Vector3I(m_resourceDesc.Width, m_resourceDesc.Height, m_resourceDesc.ArraySize);

		public int MipLevels => m_resourceDesc.MipLevels;

		public int NumSlices => m_resourceDesc.ArraySize;

		public ShaderResourceView Srv { get; private set; }

		public ISrvBindable SubresourceSrv(int id, int mipLevel = 0)
		{
			return m_arraySubresourcesSrv[id, mipLevel];
		}

		public void InitInternal(string name, Texture2DDescription resourceDesc, ShaderResourceViewDescription srvDesc)
		{
			m_name = name;
			m_resourceDesc = resourceDesc;
			m_srvDesc = srvDesc;
			m_arraySubresourcesSrv = new MyArraySubresourceSrv[NumSlices, MipLevels];
			for (int i = 0; i < NumSlices; i++)
			{
				for (int j = 0; j < MipLevels; j++)
				{
					m_arraySubresourcesSrv[i, j] = new MyArraySubresourceSrv();
					m_arraySubresourcesSrv[i, j].Init(this, i, j, srvDesc.Format);
				}
			}
		}

		protected void OnDeviceInitInternal()
		{
			Resource = new Texture2D(MyRender11.DeviceInstance, m_resourceDesc)
			{
				DebugName = m_name
			};
			Srv = new ShaderResourceView(MyRender11.DeviceInstance, Resource, m_srvDesc)
			{
				DebugName = m_name
			};
			for (int i = 0; i < NumSlices; i++)
			{
				for (int j = 0; j < MipLevels; j++)
				{
					m_arraySubresourcesSrv[i, j].OnDeviceInit();
				}
			}
		}

		protected void OnDeviceEndInternal()
		{
			for (int i = 0; i < NumSlices; i++)
			{
				for (int j = 0; j < MipLevels; j++)
				{
					m_arraySubresourcesSrv[i, j].OnDeviceEnd();
				}
			}
			if (Srv != null)
			{
				Srv.Dispose();
				Srv = null;
			}
			if (Resource != null)
			{
				Resource.Dispose();
				Resource = null;
			}
		}
	}
}
