using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Campaign
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_CampaignSM : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSM, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSM owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSM owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM_003C_003ENodes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSM, MyObjectBuilder_CampaignSMNode[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSM owner, in MyObjectBuilder_CampaignSMNode[] value)
			{
				owner.Nodes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSM owner, out MyObjectBuilder_CampaignSMNode[] value)
			{
				value = owner.Nodes;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM_003C_003ETransitions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSM, MyObjectBuilder_CampaignSMTransition[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSM owner, in MyObjectBuilder_CampaignSMTransition[] value)
			{
				owner.Transitions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSM owner, out MyObjectBuilder_CampaignSMTransition[] value)
			{
				value = owner.Transitions;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM_003C_003EMaxLobbyPlayers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSM, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSM owner, in int value)
			{
				owner.MaxLobbyPlayers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSM owner, out int value)
			{
				value = owner.MaxLobbyPlayers;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM_003C_003EMaxLobbyPlayersExperimental_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_CampaignSM, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSM owner, in int value)
			{
				owner.MaxLobbyPlayersExperimental = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSM owner, out int value)
			{
				value = owner.MaxLobbyPlayersExperimental;
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSM, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSM owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_CampaignSM, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSM owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_CampaignSM, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSM, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSM owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_CampaignSM, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSM owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_CampaignSM, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSM, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSM owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_CampaignSM, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSM owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_CampaignSM, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_CampaignSM, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_CampaignSM owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_CampaignSM, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_CampaignSM owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_CampaignSM, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Campaign_MyObjectBuilder_CampaignSM_003C_003EActor : IActivator, IActivator<MyObjectBuilder_CampaignSM>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_CampaignSM();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_CampaignSM CreateInstance()
			{
				return new MyObjectBuilder_CampaignSM();
			}

			MyObjectBuilder_CampaignSM IActivator<MyObjectBuilder_CampaignSM>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string Name;

		public MyObjectBuilder_CampaignSMNode[] Nodes;

		public MyObjectBuilder_CampaignSMTransition[] Transitions;

		public int MaxLobbyPlayers;

		public int MaxLobbyPlayersExperimental;
	}
}
