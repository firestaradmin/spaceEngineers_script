using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_AgentDefinition : MyObjectBuilder_BotDefinition
	{
		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EBotModel_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string value)
			{
				owner.BotModel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string value)
			{
				value = owner.BotModel;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ETargetType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string value)
			{
				owner.TargetType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string value)
			{
				value = owner.TargetType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EInventoryContentGenerated_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in bool value)
			{
				owner.InventoryContentGenerated = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out bool value)
			{
				value = owner.InventoryContentGenerated;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EInventoryContainerTypeId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in SerializableDefinitionId? value)
			{
				owner.InventoryContainerTypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out SerializableDefinitionId? value)
			{
				value = owner.InventoryContainerTypeId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ERemoveAfterDeath_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in bool value)
			{
				owner.RemoveAfterDeath = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out bool value)
			{
				value = owner.RemoveAfterDeath;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ERespawnTimeMs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in int value)
			{
				owner.RespawnTimeMs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out int value)
			{
				value = owner.RespawnTimeMs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ERemoveTimeMs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in int value)
			{
				owner.RemoveTimeMs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out int value)
			{
				value = owner.RemoveTimeMs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EFactionTag_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string value)
			{
				owner.FactionTag = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string value)
			{
				value = owner.FactionTag;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EAttackSound_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string value)
			{
				owner.AttackSound = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string value)
			{
				value = owner.AttackSound;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EAttackLength_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in int value)
			{
				owner.AttackLength = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out int value)
			{
				value = owner.AttackLength;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ECharacterDamage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in int value)
			{
				owner.CharacterDamage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out int value)
			{
				value = owner.CharacterDamage;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EGridDamage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in int value)
			{
				owner.GridDamage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out int value)
			{
				value = owner.GridDamage;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EAttackRadius_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in double value)
			{
				owner.AttackRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out double value)
			{
				value = owner.AttackRadius;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ETargetCharacters_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in bool value)
			{
				owner.TargetCharacters = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out bool value)
			{
				value = owner.TargetCharacters;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ETargetGrids_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AgentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in bool value)
			{
				owner.TargetGrids = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out bool value)
			{
				value = owner.TargetGrids;
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EBotBehaviorTree_003C_003EAccessor : VRage_Game_MyObjectBuilder_BotDefinition_003C_003EBotBehaviorTree_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, BotBehavior>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in BotBehavior value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_BotDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out BotBehavior value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_BotDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EBehaviorType_003C_003EAccessor : VRage_Game_MyObjectBuilder_BotDefinition_003C_003EBehaviorType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_BotDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_BotDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EBehaviorSubtype_003C_003EAccessor : VRage_Game_MyObjectBuilder_BotDefinition_003C_003EBehaviorSubtype_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_BotDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_BotDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ECommandable_003C_003EAccessor : VRage_Game_MyObjectBuilder_BotDefinition_003C_003ECommandable_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_BotDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_BotDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AgentDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AgentDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AgentDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AgentDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_AgentDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_AgentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_AgentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_AgentDefinition CreateInstance()
			{
				return new MyObjectBuilder_AgentDefinition();
			}

			MyObjectBuilder_AgentDefinition IActivator<MyObjectBuilder_AgentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string BotModel = "";

		[ProtoMember(4)]
		[DefaultValue("")]
		public string TargetType = "";

		[ProtoMember(7)]
		public bool InventoryContentGenerated;

		[ProtoMember(10)]
		public SerializableDefinitionId? InventoryContainerTypeId;

		[ProtoMember(13)]
		public bool RemoveAfterDeath = true;

		[ProtoMember(16)]
		public int RespawnTimeMs = 10000;

		[ProtoMember(19)]
		public int RemoveTimeMs = 30000;

		[ProtoMember(22)]
		public string FactionTag;

		[ProtoMember(25)]
		public string AttackSound = "ArcBotWolfAttack";

		[ProtoMember(30)]
		public int AttackLength = 1000;

		[ProtoMember(35)]
		public int CharacterDamage = 12;

		[ProtoMember(36)]
		public int GridDamage;

		[ProtoMember(40)]
		public double AttackRadius = 0.5;

		[ProtoMember(45)]
		public bool TargetCharacters = true;

		[ProtoMember(50)]
		public bool TargetGrids;
	}
}
