using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_RadialMenuItemVoxelHand : MyObjectBuilder_RadialMenuItem
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHand_003C_003EMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHand, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, in SerializableDefinitionId value)
			{
				owner.Material = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, out SerializableDefinitionId value)
			{
				value = owner.Material;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHand_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHand, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, in List<string> value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_RadialMenuItem>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, out List<string> value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_RadialMenuItem>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHand_003C_003ELabelName_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003ELabelName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHand, MyStringId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, in MyStringId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_RadialMenuItem>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, out MyStringId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_RadialMenuItem>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHand_003C_003ELabelShortcut_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003ELabelShortcut_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHand, MyStringId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, in MyStringId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_RadialMenuItem>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, out MyStringId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_RadialMenuItem>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHand_003C_003ECloseMenu_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003ECloseMenu_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHand, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_RadialMenuItem>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_RadialMenuItem>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHand_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHand, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHand_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHand, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHand_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHand, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHand_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHand, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemVoxelHand owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemVoxelHand, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHand_003C_003EActor : IActivator, IActivator<MyObjectBuilder_RadialMenuItemVoxelHand>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_RadialMenuItemVoxelHand();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_RadialMenuItemVoxelHand CreateInstance()
			{
				return new MyObjectBuilder_RadialMenuItemVoxelHand();
			}

			MyObjectBuilder_RadialMenuItemVoxelHand IActivator<MyObjectBuilder_RadialMenuItemVoxelHand>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public SerializableDefinitionId Material;
	}
}
