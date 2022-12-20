using SharpDX.Direct3D11;
using VRage.Network;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	internal class MyBlendState : MyPersistentResource<BlendState, BlendStateDescription>, IBlendStateInternal, IBlendState, IMyPersistentResource<BlendStateDescription>
	{
		private class VRage_Render11_Resources_Internal_MyBlendState_003C_003EActor : IActivator, IActivator<MyBlendState>
		{
			private sealed override object CreateInstance()
			{
				return new MyBlendState();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBlendState CreateInstance()
			{
				return new MyBlendState();
			}

			MyBlendState IActivator<MyBlendState>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override BlendStateDescription CloneDescription(ref BlendStateDescription desc)
		{
			return desc.Clone();
		}

		protected override BlendState CreateResource(ref BlendStateDescription description)
		{
			return new BlendState(MyRender11.DeviceInstance, description)
			{
				DebugName = base.Name
			};
		}
	}
}
