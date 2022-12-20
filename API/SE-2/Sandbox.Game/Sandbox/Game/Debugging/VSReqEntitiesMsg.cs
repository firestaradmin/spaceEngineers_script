using System.Runtime.InteropServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	[ProtoContract]
	public struct VSReqEntitiesMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		private class Sandbox_Game_Debugging_VSReqEntitiesMsg_003C_003EActor : IActivator, IActivator<VSReqEntitiesMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSReqEntitiesMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSReqEntitiesMsg CreateInstance()
			{
				return (VSReqEntitiesMsg)(object)default(VSReqEntitiesMsg);
			}

			VSReqEntitiesMsg IActivator<VSReqEntitiesMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_RENT";
		}
	}
}
