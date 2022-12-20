using System.Runtime.InteropServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	[ProtoContract]
	public struct VSReqWaypointCreateMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		private class Sandbox_Game_Debugging_VSReqWaypointCreateMsg_003C_003EActor : IActivator, IActivator<VSReqWaypointCreateMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSReqWaypointCreateMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSReqWaypointCreateMsg CreateInstance()
			{
				return (VSReqWaypointCreateMsg)(object)default(VSReqWaypointCreateMsg);
			}

			VSReqWaypointCreateMsg IActivator<VSReqWaypointCreateMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_RWCT";
		}
	}
}
