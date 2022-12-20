using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Gui
{
	[ProtoContract]
	public class MultilineData
	{
		protected class VRage_Game_ObjectBuilders_Gui_MultilineData_003C_003ECompleted_003C_003EAccessor : IMemberAccessor<MultilineData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MultilineData owner, in bool value)
			{
				owner.Completed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MultilineData owner, out bool value)
			{
				value = owner.Completed;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MultilineData_003C_003EIsObjective_003C_003EAccessor : IMemberAccessor<MultilineData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MultilineData owner, in bool value)
			{
				owner.IsObjective = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MultilineData owner, out bool value)
			{
				value = owner.IsObjective;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MultilineData_003C_003EData_003C_003EAccessor : IMemberAccessor<MultilineData, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MultilineData owner, in string value)
			{
				owner.Data = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MultilineData owner, out string value)
			{
				value = owner.Data;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MultilineData_003C_003ECharactersDisplayed_003C_003EAccessor : IMemberAccessor<MultilineData, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MultilineData owner, in int value)
			{
				owner.CharactersDisplayed = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MultilineData owner, out int value)
			{
				value = owner.CharactersDisplayed;
			}
		}

		private class VRage_Game_ObjectBuilders_Gui_MultilineData_003C_003EActor : IActivator, IActivator<MultilineData>
		{
			private sealed override object CreateInstance()
			{
				return new MultilineData();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MultilineData CreateInstance()
			{
				return new MultilineData();
			}

			MultilineData IActivator<MultilineData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public bool Completed;

		[ProtoMember(10)]
		public bool IsObjective;

		[ProtoMember(15)]
		public string Data;

		[ProtoMember(20)]
		public int CharactersDisplayed;
	}
}
