using SharpDX.Direct3D11;
using VRage.Network;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	internal class MyRasterizerState : MyPersistentResource<RasterizerState, RasterizerStateDescription>, IRasterizerStateInternal, IRasterizerState, IMyPersistentResource<RasterizerStateDescription>
	{
		private class VRage_Render11_Resources_Internal_MyRasterizerState_003C_003EActor : IActivator, IActivator<MyRasterizerState>
		{
			private sealed override object CreateInstance()
			{
				return new MyRasterizerState();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRasterizerState CreateInstance()
			{
				return new MyRasterizerState();
			}

			MyRasterizerState IActivator<MyRasterizerState>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override RasterizerState CreateResource(ref RasterizerStateDescription description)
		{
			return new RasterizerState(MyRender11.DeviceInstance, description)
			{
				DebugName = base.Name
			};
		}
	}
}
