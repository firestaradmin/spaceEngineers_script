using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Network;

namespace VRage.Render11.Resources.Internal
{
	internal class MyDepthArrayTexture : MyArrayTextureResource, IDepthArrayTexture, IArrayTexture, ISrvBindable, IResource
	{
		private class VRage_Render11_Resources_Internal_MyDepthArrayTexture_003C_003EActor : IActivator, IActivator<MyDepthArrayTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyDepthArrayTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDepthArrayTexture CreateInstance()
			{
				return new MyDepthArrayTexture();
			}

			MyDepthArrayTexture IActivator<MyDepthArrayTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyArraySubresourceDepth[,] m_arraySubresourcesDsv;

		private MyArraySubresourceDepth[,] m_arraySubresourcesDsvRo;

		public IDsvBindable SubresourceDsv(int nSlice, int mipLevel = 0)
		{
			return m_arraySubresourcesDsv[nSlice, mipLevel];
		}

		public IDsvBindable SubresourceDsvRo(int nSlice, int mipLevel = 0)
		{
			return m_arraySubresourcesDsvRo[nSlice, mipLevel];
		}

		public void InitDepth(string debugName, Texture2DDescription resourceDesc, ShaderResourceViewDescription srvDesc, Format dsvFormat)
		{
			InitInternal(debugName, resourceDesc, srvDesc);
			m_arraySubresourcesDsv = new MyArraySubresourceDepth[base.NumSlices, base.MipLevels];
			m_arraySubresourcesDsvRo = new MyArraySubresourceDepth[base.NumSlices, base.MipLevels];
			for (int i = 0; i < base.NumSlices; i++)
			{
				for (int j = 0; j < base.MipLevels; j++)
				{
					m_arraySubresourcesDsv[i, j] = new MyArraySubresourceDepth();
					m_arraySubresourcesDsv[i, j].Init(this, i, j, dsvFormat);
					m_arraySubresourcesDsvRo[i, j] = new MyArraySubresourceDepth();
					m_arraySubresourcesDsvRo[i, j].Init(this, i, j, dsvFormat);
				}
			}
		}

		public virtual void OnDeviceInit()
		{
			OnDeviceInitInternal();
			for (int i = 0; i < base.NumSlices; i++)
			{
				for (int j = 0; j < base.MipLevels; j++)
				{
					m_arraySubresourcesDsv[i, j].OnDeviceInit();
					m_arraySubresourcesDsvRo[i, j].OnDeviceInit(readOnly: true);
				}
			}
		}

		public virtual void OnDeviceEnd()
		{
			for (int i = 0; i < base.NumSlices; i++)
			{
				for (int j = 0; j < base.MipLevels; j++)
				{
					m_arraySubresourcesDsv[i, j].OnDeviceEnd();
					m_arraySubresourcesDsvRo[i, j].OnDeviceEnd();
				}
			}
			OnDeviceEndInternal();
		}
	}
}
