using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Network;

namespace VRage.Render11.Resources.Internal
{
	internal class MyUavArrayTexture : MyArrayTextureResource, IUavArrayTexture, IArrayTexture, ISrvBindable, IResource
	{
		private class VRage_Render11_Resources_Internal_MyUavArrayTexture_003C_003EActor : IActivator, IActivator<MyUavArrayTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyUavArrayTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyUavArrayTexture CreateInstance()
			{
				return new MyUavArrayTexture();
			}

			MyUavArrayTexture IActivator<MyUavArrayTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyArraySubresourceRtv[,] m_arraySubresourcesRtv;

		private MyArraySubresourceUav[,] m_arraySubresourcesUav;

		public void InitUav(string debugName, Texture2DDescription resourceDesc, ShaderResourceViewDescription srvDesc, Format rtvFormat, Format uavFormat)
		{
			InitInternal(debugName, resourceDesc, srvDesc);
			m_arraySubresourcesRtv = new MyArraySubresourceRtv[base.NumSlices, base.MipLevels];
			m_arraySubresourcesUav = new MyArraySubresourceUav[base.NumSlices, base.MipLevels];
			for (int i = 0; i < base.NumSlices; i++)
			{
				for (int j = 0; j < base.MipLevels; j++)
				{
					m_arraySubresourcesRtv[i, j] = new MyArraySubresourceRtv();
					m_arraySubresourcesRtv[i, j].Init(this, i, j, rtvFormat);
					m_arraySubresourcesUav[i, j] = new MyArraySubresourceUav();
					m_arraySubresourcesUav[i, j].Init(this, i, j, uavFormat);
				}
			}
		}

		public IRtvBindable SubresourceRtv(int faceId, int mipLevel = 0)
		{
			return m_arraySubresourcesRtv[faceId, mipLevel];
		}

		public IUavBindable SubresourceUav(int faceId, int mipLevel = 0)
		{
			return m_arraySubresourcesUav[faceId, mipLevel];
		}

		public void OnDeviceInit()
		{
			OnDeviceInitInternal();
			for (int i = 0; i < base.NumSlices; i++)
			{
				for (int j = 0; j < base.MipLevels; j++)
				{
					m_arraySubresourcesRtv[i, j].OnDeviceInit();
					m_arraySubresourcesUav[i, j].OnDeviceInit();
				}
			}
		}

		public void OnDeviceEnd()
		{
			for (int i = 0; i < base.NumSlices; i++)
			{
				for (int j = 0; j < base.MipLevels; j++)
				{
					m_arraySubresourcesRtv[i, j].OnDeviceEnd();
					m_arraySubresourcesUav[i, j].OnDeviceEnd();
				}
			}
			OnDeviceEndInternal();
		}
	}
}
