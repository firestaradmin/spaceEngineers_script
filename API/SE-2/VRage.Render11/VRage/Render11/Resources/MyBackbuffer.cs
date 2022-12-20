using SharpDX.Direct3D11;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources
{
	internal class MyBackbuffer : IRtvBindable, IResource, ISrvBindable
	{
		private RenderTargetView m_rtv;

		private Resource m_resource;

		private string m_debugName = "MyBackbuffer";

		private ShaderResourceView m_srv;

		public ShaderResourceView Srv => m_srv;

		public string Name => m_debugName;

		public RenderTargetView Rtv => m_rtv;

		public UnorderedAccessView Uav => null;

		public Resource Resource => m_resource;

		public Vector3I Size3 => new Vector3I(MyRender11.ResolutionI.X, MyRender11.ResolutionI.Y, 1);

		public Vector2I Size => new Vector2I(MyRender11.ResolutionI.X, MyRender11.ResolutionI.Y);

		internal MyBackbuffer(Resource swapChainBB)
		{
			m_resource = swapChainBB;
			m_resource.DebugName = m_debugName;
			m_rtv = new RenderTargetView(MyRender11.DeviceInstance, swapChainBB);
			m_rtv.DebugName = m_debugName;
			m_srv = new ShaderResourceView(MyRender11.DeviceInstance, swapChainBB);
			m_srv.DebugName = m_debugName;
		}

		internal void Release()
		{
			if (m_srv != null)
			{
				m_srv.Dispose();
				m_srv = null;
			}
			if (m_rtv != null)
			{
				m_rtv.Dispose();
				m_rtv = null;
			}
			if (m_resource != null)
			{
				m_resource.Dispose();
				m_resource = null;
			}
		}
	}
}
