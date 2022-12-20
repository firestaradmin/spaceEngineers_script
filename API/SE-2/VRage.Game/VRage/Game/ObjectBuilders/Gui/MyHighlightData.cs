using System;
using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Gui
{
	[Serializable]
	[ProtoContract]
	public struct MyHighlightData
	{
		protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyHighlightData, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyHighlightData owner, in long value)
			{
				owner.EntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyHighlightData owner, out long value)
			{
				value = owner.EntityId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData_003C_003EOutlineColor_003C_003EAccessor : IMemberAccessor<MyHighlightData, Color?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyHighlightData owner, in Color? value)
			{
				owner.OutlineColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyHighlightData owner, out Color? value)
			{
				value = owner.OutlineColor;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData_003C_003EThickness_003C_003EAccessor : IMemberAccessor<MyHighlightData, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyHighlightData owner, in int value)
			{
				owner.Thickness = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyHighlightData owner, out int value)
			{
				value = owner.Thickness;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData_003C_003EPulseTimeInFrames_003C_003EAccessor : IMemberAccessor<MyHighlightData, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyHighlightData owner, in ulong value)
			{
				owner.PulseTimeInFrames = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyHighlightData owner, out ulong value)
			{
				value = owner.PulseTimeInFrames;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData_003C_003EPlayerId_003C_003EAccessor : IMemberAccessor<MyHighlightData, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyHighlightData owner, in long value)
			{
				owner.PlayerId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyHighlightData owner, out long value)
			{
				value = owner.PlayerId;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData_003C_003EIgnoreUseObjectData_003C_003EAccessor : IMemberAccessor<MyHighlightData, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyHighlightData owner, in bool value)
			{
				owner.IgnoreUseObjectData = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyHighlightData owner, out bool value)
			{
				value = owner.IgnoreUseObjectData;
			}
		}

		protected class VRage_Game_ObjectBuilders_Gui_MyHighlightData_003C_003ESubPartNames_003C_003EAccessor : IMemberAccessor<MyHighlightData, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyHighlightData owner, in string value)
			{
				owner.SubPartNames = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyHighlightData owner, out string value)
			{
				value = owner.SubPartNames;
			}
		}

		private class VRage_Game_ObjectBuilders_Gui_MyHighlightData_003C_003EActor : IActivator, IActivator<MyHighlightData>
		{
			private sealed override object CreateInstance()
			{
				return default(MyHighlightData);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyHighlightData CreateInstance()
			{
				return (MyHighlightData)(object)default(MyHighlightData);
			}

			MyHighlightData IActivator<MyHighlightData>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// Id of entity that should be highlighted.
		/// </summary>
		[ProtoMember(5)]
		public long EntityId;

		/// <summary>
		/// Color of highlight overlay.
		/// </summary>
		[ProtoMember(10)]
		public Color? OutlineColor;

		/// <summary>
		/// Overlay thickness.
		/// </summary>
		[ProtoMember(15)]
		public int Thickness;

		/// <summary>
		/// Number of frames between pulses.
		/// </summary>
		[ProtoMember(20)]
		public ulong PulseTimeInFrames;

		/// <summary>
		/// Id of player that should do the highlight.
		/// (For non local players its send to client)
		/// </summary>
		[ProtoMember(25)]
		public long PlayerId;

		/// <summary>
		/// When set to true the system does not use the 
		/// IMyUseObject logic to process the highlight.
		/// </summary>
		[ProtoMember(30)]
		public bool IgnoreUseObjectData;

		/// <summary>
		/// Specify there the names of the subparts that would be highlighted
		/// instead of the full model.
		/// Format: "subpart_1;subpart_2"
		/// </summary>
		[ProtoMember(35)]
		public string SubPartNames;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="entityId">Id of entity that should be highlighted.</param>
		/// <param name="thickness">Overlay thickness.</param>
		/// <param name="pulseTimeInFrames">Number of frames between the pulses.</param>
		/// <param name="outlineColor">Color of overlay.</param>
		/// <param name="ignoreUseObjectData">Used to ignore IMyUseObject logic for highlighting.</param>
		/// <param name="playerId">Id of receiving player.</param>
		/// <param name="subPartNames">Names of subparts that should be highlighted instead of the full model.</param>
		public MyHighlightData(long entityId = 0L, int thickness = -1, ulong pulseTimeInFrames = 0uL, Color? outlineColor = null, bool ignoreUseObjectData = false, long playerId = -1L, string subPartNames = null)
		{
			EntityId = entityId;
			Thickness = thickness;
			OutlineColor = outlineColor;
			PulseTimeInFrames = pulseTimeInFrames;
			PlayerId = playerId;
			IgnoreUseObjectData = ignoreUseObjectData;
			SubPartNames = subPartNames;
		}
	}
}
