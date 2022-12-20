using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct SelectedTreeMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_SelectedTreeMsg_003C_003EBehaviorTreeName_003C_003EAccessor : IMemberAccessor<SelectedTreeMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SelectedTreeMsg owner, in string value)
			{
				owner.BehaviorTreeName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SelectedTreeMsg owner, out string value)
			{
				value = owner.BehaviorTreeName;
			}
		}

		private class Sandbox_Game_Debugging_SelectedTreeMsg_003C_003EActor : IActivator, IActivator<SelectedTreeMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(SelectedTreeMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override SelectedTreeMsg CreateInstance()
			{
				return (SelectedTreeMsg)(object)default(SelectedTreeMsg);
			}

			SelectedTreeMsg IActivator<SelectedTreeMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public string BehaviorTreeName;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "SELTREE";
		}
	}
}
