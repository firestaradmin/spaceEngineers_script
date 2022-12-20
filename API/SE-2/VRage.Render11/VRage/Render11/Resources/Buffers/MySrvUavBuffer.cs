using SharpDX.Direct3D11;
using VRageRender;

namespace VRage.Render11.Resources.Buffers
{
	internal class MySrvUavBuffer : MyUavBuffer, ISrvUavBuffer, ISrvUavBindable, ISrvBindable, IResource, IUavBindable, ISrvBuffer, IBuffer, IUavBuffer
	{
		private ShaderResourceView m_srv;

		public ShaderResourceView Srv => m_srv;

		protected override void AfterBufferInit()
		{
			base.AfterBufferInit();
			m_srv = new ShaderResourceView(MyRender11.DeviceInstance, base.Resource)
			{
				DebugName = base.Name + "_Srv"
			};
		}

		public override void DisposeInternal()
		{
			if (m_srv != null)
			{
				m_srv.Dispose();
				m_srv = null;
			}
			base.DisposeInternal();
		}
	}
}
