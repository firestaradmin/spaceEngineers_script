using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Game;
using VRage.Game.Debugging;
using VRage.Network;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct VSStatusMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class Sandbox_Game_Debugging_VSStatusMsg_003C_003EWorld_003C_003EAccessor : IMemberAccessor<VSStatusMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSStatusMsg owner, in string value)
			{
				owner.World = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSStatusMsg owner, out string value)
			{
				value = owner.World;
			}
		}

		protected class Sandbox_Game_Debugging_VSStatusMsg_003C_003EVSComponent_003C_003EAccessor : IMemberAccessor<VSStatusMsg, MyObjectBuilder_VisualScriptManagerSessionComponent>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref VSStatusMsg owner, in MyObjectBuilder_VisualScriptManagerSessionComponent value)
			{
				owner.VSComponent = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref VSStatusMsg owner, out MyObjectBuilder_VisualScriptManagerSessionComponent value)
			{
				value = owner.VSComponent;
			}
		}

		private class Sandbox_Game_Debugging_VSStatusMsg_003C_003EActor : IActivator, IActivator<VSStatusMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(VSStatusMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override VSStatusMsg CreateInstance()
			{
				return (VSStatusMsg)(object)default(VSStatusMsg);
			}

			VSStatusMsg IActivator<VSStatusMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public string World;

		[ProtoMember(10)]
		public MyObjectBuilder_VisualScriptManagerSessionComponent VSComponent;

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "VS_STS";
		}
	}
}
