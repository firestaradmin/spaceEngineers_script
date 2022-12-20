using System;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRageMath;

namespace VRage.Render11.Resources
{
	internal class MySrvWrapper : ISrvTexture, ISrvBindable, IResource, ITexture
	{
		private readonly ShaderResourceViewDescription m_desc;

		private readonly Vector2I m_size;

		private readonly Vector3I m_sizeArray;

		public string Name => Srv.DebugName;

		public SharpDX.Direct3D11.Resource Resource => Srv.Resource;

		public Vector3I Size3 => m_sizeArray;

		public Vector2I Size => m_size;

		public ShaderResourceView Srv { get; }

		public Format Format => m_desc.Format;

		public int MipLevels => m_desc.Texture2D.MipLevels;

		public event Action<ITexture> OnFormatChanged;

		public MySrvWrapper(IntPtr texture, Vector2I size)
		{
			Srv = new ShaderResourceView(texture);
			m_desc = Srv.Description;
			m_size = size;
			m_sizeArray = new Vector3I(size, 1);
		}

		public void Dispose()
		{
			Srv.Dispose();
		}
	}
}
