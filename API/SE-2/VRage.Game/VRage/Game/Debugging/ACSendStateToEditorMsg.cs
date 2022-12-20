using System;
using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game.Debugging
{
	[ProtoContract]
	public struct ACSendStateToEditorMsg : MyExternalDebugStructures.IExternalDebugMsg
	{
		protected class VRage_Game_Debugging_ACSendStateToEditorMsg_003C_003ECurrentNodeAddress_003C_003EAccessor : IMemberAccessor<ACSendStateToEditorMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ACSendStateToEditorMsg owner, in string value)
			{
				owner.CurrentNodeAddress = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ACSendStateToEditorMsg owner, out string value)
			{
				value = owner.CurrentNodeAddress;
			}
		}

		protected class VRage_Game_Debugging_ACSendStateToEditorMsg_003C_003EVisitedTreeNodesPath_003C_003EAccessor : IMemberAccessor<ACSendStateToEditorMsg, int[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ACSendStateToEditorMsg owner, in int[] value)
			{
				owner.VisitedTreeNodesPath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ACSendStateToEditorMsg owner, out int[] value)
			{
				value = owner.VisitedTreeNodesPath;
			}
		}

		private class VRage_Game_Debugging_ACSendStateToEditorMsg_003C_003EActor : IActivator, IActivator<ACSendStateToEditorMsg>
		{
			private sealed override object CreateInstance()
			{
				return default(ACSendStateToEditorMsg);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ACSendStateToEditorMsg CreateInstance()
			{
				return (ACSendStateToEditorMsg)(object)default(ACSendStateToEditorMsg);
			}

			ACSendStateToEditorMsg IActivator<ACSendStateToEditorMsg>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public string CurrentNodeAddress;

		[ProtoMember(10)]
		public int[] VisitedTreeNodesPath;

		public static ACSendStateToEditorMsg Create(string currentNodeAddress, int[] visitedTreeNodesPath)
		{
			ACSendStateToEditorMsg aCSendStateToEditorMsg = default(ACSendStateToEditorMsg);
			aCSendStateToEditorMsg.CurrentNodeAddress = currentNodeAddress;
			aCSendStateToEditorMsg.VisitedTreeNodesPath = new int[64];
			ACSendStateToEditorMsg result = aCSendStateToEditorMsg;
			if (visitedTreeNodesPath != null)
			{
				Array.Copy(visitedTreeNodesPath, result.VisitedTreeNodesPath, Math.Min(visitedTreeNodesPath.Length, 64));
			}
			return result;
		}

		string MyExternalDebugStructures.IExternalDebugMsg.GetTypeStr()
		{
			return "AC_STA";
		}
	}
}
