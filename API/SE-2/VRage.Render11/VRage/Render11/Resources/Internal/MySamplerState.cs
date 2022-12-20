using SharpDX.Direct3D11;
using VRage.Network;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	internal class MySamplerState : MyPersistentResource<SamplerState, SamplerStateDescription>, ISamplerStateInternal, ISamplerState, IMyPersistentResource<SamplerStateDescription>
	{
		private class VRage_Render11_Resources_Internal_MySamplerState_003C_003EActor : IActivator, IActivator<MySamplerState>
		{
			private sealed override object CreateInstance()
			{
				return new MySamplerState();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySamplerState CreateInstance()
			{
				return new MySamplerState();
			}

			MySamplerState IActivator<MySamplerState>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override SamplerState CreateResource(ref SamplerStateDescription description)
		{
			return new SamplerState(MyRender11.DeviceInstance, description)
			{
				DebugName = base.Name
			};
		}
	}
}
