using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.GameServices
{
	[Serializable]
	public class MyGameInventoryItemDefinition
	{
		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EID_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in int value)
			{
				owner.ID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out int value)
			{
				value = owner.ID;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EName_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
				value = owner.Name;
			}
		}

<<<<<<< HEAD
		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003ETradable_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
=======
		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EDescription_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
<<<<<<< HEAD
				owner.Tradable = value;
=======
				owner.Description = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
<<<<<<< HEAD
				value = owner.Tradable;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EMarketable_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
=======
				value = owner.Description;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EDisplayType_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
<<<<<<< HEAD
				owner.Marketable = value;
=======
				owner.DisplayType = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
<<<<<<< HEAD
				value = owner.Marketable;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EDescription_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
=======
				value = owner.DisplayType;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EIconTexture_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
<<<<<<< HEAD
				owner.Description = value;
=======
				owner.IconTexture = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
<<<<<<< HEAD
				value = owner.Description;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EDisplayType_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
=======
				value = owner.IconTexture;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EAssetModifierId_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
<<<<<<< HEAD
				owner.DisplayType = value;
=======
				owner.AssetModifierId = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
<<<<<<< HEAD
				value = owner.DisplayType;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EIconTexture_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
				owner.IconTexture = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
				value = owner.IconTexture;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EAssetModifierId_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
				owner.AssetModifierId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
				value = owner.AssetModifierId;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EItemSlot_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, MyGameInventoryItemSlot>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in MyGameInventoryItemSlot value)
			{
				owner.ItemSlot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out MyGameInventoryItemSlot value)
			{
				value = owner.ItemSlot;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EToolName_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
=======
				value = owner.AssetModifierId;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EItemSlot_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, MyGameInventoryItemSlot>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in MyGameInventoryItemSlot value)
			{
				owner.ItemSlot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out MyGameInventoryItemSlot value)
			{
				value = owner.ItemSlot;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EToolName_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
				owner.ToolName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
				value = owner.ToolName;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003ENameColor_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
<<<<<<< HEAD
				owner.ToolName = value;
=======
				owner.NameColor = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
<<<<<<< HEAD
				value = owner.ToolName;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003ENameColor_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
=======
				value = owner.NameColor;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EBackgroundColor_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
<<<<<<< HEAD
				owner.NameColor = value;
=======
				owner.BackgroundColor = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
<<<<<<< HEAD
				value = owner.NameColor;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EBackgroundColor_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
				owner.BackgroundColor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
				value = owner.BackgroundColor;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EDefinitionType_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, MyGameInventoryItemDefinitionType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in MyGameInventoryItemDefinitionType value)
			{
				owner.DefinitionType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out MyGameInventoryItemDefinitionType value)
			{
				value = owner.DefinitionType;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EHidden_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, bool>
=======
				value = owner.BackgroundColor;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EDefinitionType_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, MyGameInventoryItemDefinitionType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in MyGameInventoryItemDefinitionType value)
			{
				owner.DefinitionType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out MyGameInventoryItemDefinitionType value)
			{
				value = owner.DefinitionType;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EHidden_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in bool value)
			{
				owner.Hidden = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out bool value)
			{
				value = owner.Hidden;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EIsStoreHidden_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, bool>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in bool value)
			{
<<<<<<< HEAD
				owner.Hidden = value;
=======
				owner.IsStoreHidden = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out bool value)
			{
<<<<<<< HEAD
				value = owner.Hidden;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EIsStoreHidden_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, bool>
=======
				value = owner.IsStoreHidden;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003ECanBePurchased_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, bool>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in bool value)
			{
<<<<<<< HEAD
				owner.IsStoreHidden = value;
=======
				owner.CanBePurchased = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out bool value)
			{
<<<<<<< HEAD
				value = owner.IsStoreHidden;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003ECanBePurchased_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in bool value)
			{
				owner.CanBePurchased = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out bool value)
			{
				value = owner.CanBePurchased;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EItemQuality_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, MyGameInventoryItemQuality>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in MyGameInventoryItemQuality value)
			{
				owner.ItemQuality = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out MyGameInventoryItemQuality value)
			{
				value = owner.ItemQuality;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EExchange_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
				owner.Exchange = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
				value = owner.Exchange;
			}
		}

=======
				value = owner.CanBePurchased;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EItemQuality_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, MyGameInventoryItemQuality>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in MyGameInventoryItemQuality value)
			{
				owner.ItemQuality = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out MyGameInventoryItemQuality value)
			{
				value = owner.ItemQuality;
			}
		}

		protected class VRage_GameServices_MyGameInventoryItemDefinition_003C_003EExchange_003C_003EAccessor : IMemberAccessor<MyGameInventoryItemDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyGameInventoryItemDefinition owner, in string value)
			{
				owner.Exchange = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyGameInventoryItemDefinition owner, out string value)
			{
				value = owner.Exchange;
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public int ID { get; set; }

		public string Name { get; set; }

<<<<<<< HEAD
		public string Tradable { get; set; }

		public string Marketable { get; set; }

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public string Description { get; set; }

		public string DisplayType { get; set; }

		public string IconTexture { get; set; }

		public string AssetModifierId { get; set; }

		public MyGameInventoryItemSlot ItemSlot { get; set; }

		public string ToolName { get; set; }

		public string NameColor { get; set; }

		public string BackgroundColor { get; set; }

		public MyGameInventoryItemDefinitionType DefinitionType { get; set; }

		public bool Hidden { get; set; }

		public bool IsStoreHidden { get; set; }

		public bool CanBePurchased { get; set; }

		public MyGameInventoryItemQuality ItemQuality { get; set; }

		public string Exchange { get; set; }

		public override string ToString()
		{
			return $"({ID}) {Name}";
		}
	}
}
