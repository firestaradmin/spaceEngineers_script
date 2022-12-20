using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Network;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	internal class MyRtvArrayTexture : MyArrayTextureResource, IRtvArrayTexture, IArrayTexture, ISrvBindable, IResource, IRtvBindable
	{
		private class VRage_Render11_Resources_Internal_MyRtvArrayTexture_003C_003EActor : IActivator, IActivator<MyRtvArrayTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyRtvArrayTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRtvArrayTexture CreateInstance()
			{
				return new MyRtvArrayTexture();
			}

			MyRtvArrayTexture IActivator<MyRtvArrayTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyArraySubresourceRtv[,] m_arraySubresourcesRtv;

		public RenderTargetView Rtv { get; private set; }

		public void InitRtv(string debugName, Texture2DDescription resourceDesc, ShaderResourceViewDescription srvDesc, Format rtvFormat)
		{
			InitInternal(debugName, resourceDesc, srvDesc);
			m_arraySubresourcesRtv = new MyArraySubresourceRtv[base.NumSlices, base.MipLevels];
			for (int i = 0; i < base.NumSlices; i++)
			{
				for (int j = 0; j < base.MipLevels; j++)
				{
					m_arraySubresourcesRtv[i, j] = new MyArraySubresourceRtv();
					m_arraySubresourcesRtv[i, j].Init(this, i, j, rtvFormat);
				}
			}
		}

		public IRtvBindable SubresourceRtv(int faceId, int mipLevel = 0)
		{
			return m_arraySubresourcesRtv[faceId, mipLevel];
		}

		public void OnDeviceInit()
		{
			OnDeviceInitInternal();
			Rtv = new RenderTargetView(MyRender11.DeviceInstance, base.Resource);
			for (int i = 0; i < base.NumSlices; i++)
			{
				for (int j = 0; j < base.MipLevels; j++)
				{
					m_arraySubresourcesRtv[i, j].OnDeviceInit();
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
				}
			}
			Rtv.Dispose();
			Rtv = null;
			OnDeviceEndInternal();
		}
	}
}
