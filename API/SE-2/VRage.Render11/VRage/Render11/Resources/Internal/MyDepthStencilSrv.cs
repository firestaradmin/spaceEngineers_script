using SharpDX.Direct3D11;
using VRage.Network;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	[GenerateActivator]
	internal class MyDepthStencilSrv : ISrvBindable, IResource
	{
		private class VRage_Render11_Resources_Internal_MyDepthStencilSrv_003C_003EActor : IActivator, IActivator<MyDepthStencilSrv>
		{
			private sealed override object CreateInstance()
			{
				return new MyDepthStencilSrv();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDepthStencilSrv CreateInstance()
			{
				return new MyDepthStencilSrv();
			}

			MyDepthStencilSrv IActivator<MyDepthStencilSrv>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyDepthStencil m_owner;

		public string Name => m_owner.Name;

		public Resource Resource => m_owner.Resource;

		public Vector3I Size3 => m_owner.Size3;

		public Vector2I Size => m_owner.Size;

		public ShaderResourceView Srv { get; private set; }

		public void OnDeviceInit(MyDepthStencil owner, ShaderResourceViewDescription desc)
		{
			m_owner = owner;
			Srv = new ShaderResourceView(MyRender11.DeviceInstance, m_owner.Resource, desc)
			{
				DebugName = owner.Name
			};
		}

		public void OnDeviceEnd()
		{
			if (Srv != null)
			{
				Srv.Dispose();
				Srv = null;
			}
		}
	}
}
