using System.Runtime.InteropServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	[ProtoContract]
	public struct VSReqTriggersMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		private class Sandbox_Game_Debugging_VSReqTriggersMsg_003C_003EActor : IActivator, IActivator<VSReqTriggersMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSReqTriggersMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSReqTriggersMsg CreateInstance()
			{
				return (VSReqTriggersMsg)(object)default(VSReqTriggersMsg);
			}

			VSReqTriggersMsg IActivator<VSReqTriggersMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_RTRG";
		}
	}
}
