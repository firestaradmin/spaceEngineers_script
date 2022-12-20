using SharpDX.Direct3D11;
using VRage.Network;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	internal class MyDepthStencilState : MyPersistentResource<DepthStencilState, DepthStencilStateDescription>, IDepthStencilStateInternal, IDepthStencilState, IMyPersistentResource<DepthStencilStateDescription>
	{
		private class VRage_Render11_Resources_Internal_MyDepthStencilState_003C_003EActor : IActivator, IActivator<MyDepthStencilState>
		{
			private sealed override object CreateInstance()
			{
				return new MyDepthStencilState();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDepthStencilState CreateInstance()
			{
				return new MyDepthStencilState();
			}

			MyDepthStencilState IActivator<MyDepthStencilState>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override DepthStencilState CreateResource(ref DepthStencilStateDescription description)
		{
			return new DepthStencilState(MyRender11.DeviceInstance, description)
			{
				DebugName = base.Name
			};
		}
	}
}
