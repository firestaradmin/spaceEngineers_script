using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.GameServices
{
	[Serializable]
	public class MyGameInventoryItem
	{
		protected class VRage_GameServices_MyGameInventoryItem_003C_003EID_003C_003EAccessor : IMemberAccessor<MyGameInventoryItem, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItem owner, in ulong value)
			{
				owner.ID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItem owner, out ulong value)
			{
				value = owner.ID;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItem_003C_003EItemDefinition_003C_003EAccessor : IMemberAccessor<MyGameInventoryItem, MyGameInventoryItemDefinition>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItem owner, in MyGameInventoryItemDefinition value)
			{
				owner.ItemDefinition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItem owner, out MyGameInventoryItemDefinition value)
			{
				value = owner.ItemDefinition;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItem_003C_003EQuantity_003C_003EAccessor : IMemberAccessor<MyGameInventoryItem, ushort>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItem owner, in ushort value)
			{
				owner.Quantity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItem owner, out ushort value)
			{
				value = owner.Quantity;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItem_003C_003EUsingCharacters_003C_003EAccessor : IMemberAccessor<MyGameInventoryItem, HashSet<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItem owner, in HashSet<long> value)
			{
				owner.UsingCharacters = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItem owner, out HashSet<long> value)
			{
				value = owner.UsingCharacters;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItem_003C_003EIsStoreFakeItem_003C_003EAccessor : IMemberAccessor<MyGameInventoryItem, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItem owner, in bool value)
			{
				owner.IsStoreFakeItem = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItem owner, out bool value)
			{
				value = owner.IsStoreFakeItem;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItem_003C_003EIsNew_003C_003EAccessor : IMemberAccessor<MyGameInventoryItem, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItem owner, in bool value)
			{
				owner.IsNew = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItem owner, out bool value)
			{
				value = owner.IsNew;
			}
		}

		public ulong ID { get; set; }

		public MyGameInventoryItemDefinition ItemDefinition { get; set; }

		public ushort Quantity { get; set; }

		public HashSet<long> UsingCharacters { get; set; } = new HashSet<long>();


		public bool IsStoreFakeItem { get; set; }

		public bool IsNew { get; set; }
	}
}
