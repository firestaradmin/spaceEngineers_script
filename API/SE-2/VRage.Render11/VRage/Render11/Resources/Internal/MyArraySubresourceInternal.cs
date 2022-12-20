using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRageMath;

namespace VRage.Render11.Resources.Internal
{
	internal class MyArraySubresourceInternal
	{
		protected IResource m_owner;

		protected int m_slice;

		protected int m_mipLevel;

		protected Format m_format;

		public string Name => m_owner.Name;

		public SharpDX.Direct3D11.Resource Resource => m_owner.Resource;

		public Vector2I Size => m_owner.Size;

		public Vector3I Size3 => m_owner.Size3;

		public void Init(MyArrayTextureResource owner, int slice, int mip, Format srvFormat)
		{
			m_owner = owner;
			m_slice = slice;
			m_mipLevel = mip;
			m_format = srvFormat;
		}
	}
}
