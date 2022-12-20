using SharpDX.Direct3D11;
using VRageMath;

namespace VRage.Render11.Resources.Internal
{
	internal class MyDsvBindable : IDsvBindable, IResource
	{
		private readonly IResource m_parent;

		public string Name => m_parent.Name;

		public Resource Resource => m_parent.Resource;

		public Vector3I Size3 => m_parent.Size3;

		public Vector2I Size => m_parent.Size;

		public DepthStencilView Dsv { get; private set; }

		public MyDsvBindable(IResource parent, DepthStencilView dsv)
		{
			m_parent = parent;
			Dsv = dsv;
		}

		public void Dispose()
		{
			Dsv.Dispose();
		}
	}
}
