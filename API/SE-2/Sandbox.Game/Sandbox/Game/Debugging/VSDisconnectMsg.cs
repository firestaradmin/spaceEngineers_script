using System.Runtime.InteropServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	[ProtoContract]
	public struct VSDisconnectMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		private class Sandbox_Game_Debugging_VSDisconnectMsg_003C_003EActor : IActivator, IActivator<VSDisconnectMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSDisconnectMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSDisconnectMsg CreateInstance()
			{
				return (VSDisconnectMsg)(object)default(VSDisconnectMsg);
			}

			VSDisconnectMsg IActivator<VSDisconnectMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_DIS";
		}
	}
}
