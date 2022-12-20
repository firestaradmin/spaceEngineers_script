using System.Runtime.InteropServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	[ProtoContract]
	public struct VSReqEntityIdsMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		private class Sandbox_Game_Debugging_VSReqEntityIdsMsg_003C_003EActor : IActivator, IActivator<VSReqEntityIdsMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSReqEntityIdsMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSReqEntityIdsMsg CreateInstance()
			{
				return (VSReqEntityIdsMsg)(object)default(VSReqEntityIdsMsg);
			}

			VSReqEntityIdsMsg IActivator<VSReqEntityIdsMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_RENI";
		}
	}
}
