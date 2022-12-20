using System.Runtime.InteropServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	[ProtoContract]
	public struct VSSaveWorldMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		private class Sandbox_Game_Debugging_VSSaveWorldMsg_003C_003EActor : IActivator, IActivator<VSSaveWorldMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSSaveWorldMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSSaveWorldMsg CreateInstance()
			{
				return (VSSaveWorldMsg)(object)default(VSSaveWorldMsg);
			}

			VSSaveWorldMsg IActivator<VSSaveWorldMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_SWRD";
		}
	}
}
