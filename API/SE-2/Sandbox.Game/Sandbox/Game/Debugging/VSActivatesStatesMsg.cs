using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSActivatesStatesMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSActivatesStatesMsg_003C_003ESMName_003C_003EAccessor : IMemberAccessor<VSActivatesStatesMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSActivatesStatesMsg owner, in string value)
			{
				owner.SMName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSActivatesStatesMsg owner, out string value)
			{
				value = owner.SMName;
			}
		}

		protected class Sandbox_Game_Debugging_VSActivatesStatesMsg_003C_003EActiveStates_003C_003EAccessor : IMemberAccessor<VSActivatesStatesMsg, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSActivatesStatesMsg owner, in string[] value)
			{
				owner.ActiveStates = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSActivatesStatesMsg owner, out string[] value)
			{
				value = owner.ActiveStates;
			}
		}

		private class Sandbox_Game_Debugging_VSActivatesStatesMsg_003C_003EActor : IActivator, IActivator<VSActivatesStatesMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSActivatesStatesMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSActivatesStatesMsg CreateInstance()
			{
				return (VSActivatesStatesMsg)(object)default(VSActivatesStatesMsg);
			}

			VSActivatesStatesMsg IActivator<VSActivatesStatesMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public string SMName;

		[ProtoMember(10)]
		public string[] ActiveStates;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_SETS";
		}
	}
}
