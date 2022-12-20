using System;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	[GenerateActivator]
	internal class MyGeneratedTextureFromPattern : IGeneratedTexture, ITexture, ISrvBindable, IResource
	{
		private class VRage_Render11_Resources_Internal_MyGeneratedTextureFromPattern_003C_003EActor : IActivator, IActivator<MyGeneratedTextureFromPattern>
		{
			private sealed override object CreateInstance()
			{
				return new MyGeneratedTextureFromPattern();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGeneratedTextureFromPattern CreateInstance()
			{
				return new MyGeneratedTextureFromPattern();
			}

			MyGeneratedTextureFromPattern IActivator<MyGeneratedTextureFromPattern>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private string m_name;

		private Vector2I m_size;

		private SharpDX.Direct3D11.Resource m_resource;

		private ShaderResourceView m_srv;

		private Format m_format;

		private int m_mipLevels;

		public ShaderResourceView Srv => m_srv;

		public SharpDX.Direct3D11.Resource Resource => m_resource;

		public string Name => m_name;

		public Format Format => m_format;

		public Vector2I Size => m_size;

		public Vector3I Size3 => new Vector3I(m_size.X, m_size.Y, 1);

		public int MipLevels => m_mipLevels;

		public event Action<ITexture> OnFormatChanged;

		private byte[] CreateTextureDataByPattern(Vector2I strides, byte[] pattern4x4)
		{
			int num = 0;
			Vector2I vector2I = default(Vector2I);
			for (int i = 0; i < MyResourceUtils.GetMipLevels(Math.Max(strides.X, strides.Y)); i++)
			{
				vector2I.X = MyResourceUtils.GetMipStride(strides.X, i);
				vector2I.Y = MyResourceUtils.GetMipStride(strides.Y, i);
				num += vector2I.X * vector2I.Y / 16;
			}
			byte[] array = new byte[num * pattern4x4.Length];
			for (int j = 0; j < num; j++)
			{
				Array.Copy(pattern4x4, 0, array, pattern4x4.Length * j, pattern4x4.Length);
			}
			return array;
		}

		private unsafe void InitDxObjects(string name, Vector2I size, Format format, byte[] bytePattern)
		{
			Texture2DDescription description = default(Texture2DDescription);
			description.Format = (m_format = format);
			description.ArraySize = 1;
			description.Height = size.Y;
			description.Width = size.X;
			description.MipLevels = (m_mipLevels = MyResourceUtils.GetMipLevels(Math.Max(size.X, size.Y)));
			description.BindFlags = BindFlags.ShaderResource;
			description.CpuAccessFlags = CpuAccessFlags.None;
			description.OptionFlags = ResourceOptionFlags.None;
			description.Usage = ResourceUsage.Immutable;
			description.SampleDescription = new SampleDescription(1, 0);
			Vector2I strides = default(Vector2I);
			strides.X = MyResourceUtils.GetMipStride(description.Width, 0);
			strides.Y = MyResourceUtils.GetMipStride(description.Height, 0);
			byte[] array = CreateTextureDataByPattern(strides, bytePattern);
			DataBox[] array2 = new DataBox[description.MipLevels];
			int num = 0;
			fixed (byte* ptr = array)
			{
				void* ptr2 = ptr;
				Vector2I vector2I = default(Vector2I);
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].SlicePitch = 0;
					array2[i].RowPitch = MyResourceUtils.GetMipStride(size.X, i) * bytePattern.Length / 16;
					array2[i].DataPointer = new IntPtr((byte*)ptr2 + num * bytePattern.Length);
					vector2I.X = MyResourceUtils.GetMipStride(size.X, i) / 4;
					vector2I.Y = MyResourceUtils.GetMipStride(size.Y, i) / 4;
					num += vector2I.X * vector2I.Y;
				}
				m_resource = new Texture2D(MyRender11.DeviceInstance, description, array2);
				m_resource.DebugName = name;
			}
			m_srv = new ShaderResourceView(MyRender11.DeviceInstance, m_resource);
			m_srv.DebugName = name;
			this.OnFormatChanged.InvokeIfNotNull(this);
		}

		internal void Init(string name, Vector2I size, Format format, byte[] bytePattern)
		{
			m_name = name;
			m_size = size;
			InitDxObjects(name, size, format, bytePattern);
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
		}
	}
}
