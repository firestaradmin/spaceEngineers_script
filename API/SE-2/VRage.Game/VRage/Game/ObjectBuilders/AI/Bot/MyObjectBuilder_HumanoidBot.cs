using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.AI.Bot
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_HumanoidBot : MyObjectBuilder_AgentBot
	{
		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_HumanoidBot_003C_003EAiTarget_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentBot_003C_003EAiTarget_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBot, MyObjectBuilder_AiTarget>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBot owner, in MyObjectBuilder_AiTarget value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_AgentBot>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBot owner, out MyObjectBuilder_AiTarget value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_AgentBot>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_HumanoidBot_003C_003ERemoveAfterDeath_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentBot_003C_003ERemoveAfterDeath_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBot, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBot owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_AgentBot>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBot owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_AgentBot>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_HumanoidBot_003C_003ERespawnCounter_003C_003EAccessor : VRage_Game_MyObjectBuilder_AgentBot_003C_003ERespawnCounter_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBot, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBot owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_AgentBot>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBot owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_AgentBot>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_HumanoidBot_003C_003EBotDefId_003C_003EAccessor : VRage_Game_MyObjectBuilder_Bot_003C_003EBotDefId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBot, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBot owner, in SerializableDefinitionId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Bot>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBot owner, out SerializableDefinitionId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Bot>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_HumanoidBot_003C_003EBotMemory_003C_003EAccessor : VRage_Game_MyObjectBuilder_Bot_003C_003EBotMemory_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBot, MyObjectBuilder_BotMemory>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBot owner, in MyObjectBuilder_BotMemory value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Bot>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBot owner, out MyObjectBuilder_BotMemory value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Bot>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_HumanoidBot_003C_003ELastBehaviorTree_003C_003EAccessor : VRage_Game_MyObjectBuilder_Bot_003C_003ELastBehaviorTree_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBot owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Bot>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBot owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Bot>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_HumanoidBot_003C_003EAsociatedMyPlayerId_003C_003EAccessor : VRage_Game_MyObjectBuilder_Bot_003C_003EAsociatedMyPlayerId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBot, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBot owner, in ulong value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Bot>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBot owner, out ulong value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Bot>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_HumanoidBot_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBot, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBot owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBot owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_HumanoidBot_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBot owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBot owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_HumanoidBot_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBot, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBot owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBot owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_HumanoidBot_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_HumanoidBot, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_HumanoidBot owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_HumanoidBot owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_HumanoidBot, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_AI_Bot_MyObjectBuilder_HumanoidBot_003C_003EActor : IActivator, IActivator<MyObjectBuilder_HumanoidBot>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_HumanoidBot();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_HumanoidBot CreateInstance()
			{
				return new MyObjectBuilder_HumanoidBot();
			}

			MyObjectBuilder_HumanoidBot IActivator<MyObjectBuilder_HumanoidBot>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
