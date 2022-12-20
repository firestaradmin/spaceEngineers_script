using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game.VisualScripting
{
	[ProtoContract]
	public struct MyDebuggingNodeLog
	{
		protected class VRage_Game_VisualScripting_MyDebuggingNodeLog_003C_003ENodeID_003C_003EAccessor : IMemberAccessor<MyDebuggingNodeLog, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDebuggingNodeLog owner, in int value)
			{
				owner.NodeID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDebuggingNodeLog owner, out int value)
			{
				value = owner.NodeID;
			}
		}

		protected class VRage_Game_VisualScripting_MyDebuggingNodeLog_003C_003EValues_003C_003EAccessor : IMemberAccessor<MyDebuggingNodeLog, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDebuggingNodeLog owner, in string[] value)
			{
				owner.Values = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDebuggingNodeLog owner, out string[] value)
			{
				value = owner.Values;
			}
		}

		private class VRage_Game_VisualScripting_MyDebuggingNodeLog_003C_003EActor : IActivator, IActivator<MyDebuggingNodeLog>
		{
			private sealed override object CreateInstance()
			{
				return default(MyDebuggingNodeLog);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDebuggingNodeLog CreateInstance()
			{
				return (MyDebuggingNodeLog)(object)default(MyDebuggingNodeLog);
			}

			MyDebuggingNodeLog IActivator<MyDebuggingNodeLog>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public int NodeID;

		[ProtoMember(10)]
		public string[] Values;
	}
}
