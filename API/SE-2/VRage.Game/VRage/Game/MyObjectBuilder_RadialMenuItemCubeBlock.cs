using System.Collections.Generic;
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
	public class MyObjectBuilder_RadialMenuItemCubeBlock : MyObjectBuilder_RadialMenuItem
	{
		protected class VRage_Game_MyObjectBuilder_RadialMenuItemCubeBlock_003C_003EId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RadialMenuItemCubeBlock, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, in SerializableDefinitionId value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, out SerializableDefinitionId value)
			{
				value = owner.Id;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemCubeBlock_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemCubeBlock, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, in List<string> value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_RadialMenuItem>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, out List<string> value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_RadialMenuItem>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemCubeBlock_003C_003ELabelName_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003ELabelName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemCubeBlock, MyStringId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, in MyStringId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_RadialMenuItem>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, out MyStringId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_RadialMenuItem>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemCubeBlock_003C_003ELabelShortcut_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003ELabelShortcut_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemCubeBlock, MyStringId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, in MyStringId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_RadialMenuItem>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, out MyStringId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_RadialMenuItem>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemCubeBlock_003C_003ECloseMenu_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003ECloseMenu_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemCubeBlock, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_RadialMenuItem>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_RadialMenuItem>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemCubeBlock_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemCubeBlock, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemCubeBlock_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemCubeBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemCubeBlock_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemCubeBlock, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemCubeBlock_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemCubeBlock, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemCubeBlock owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemCubeBlock, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_RadialMenuItemCubeBlock_003C_003EActor : IActivator, IActivator<MyObjectBuilder_RadialMenuItemCubeBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_RadialMenuItemCubeBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_RadialMenuItemCubeBlock CreateInstance()
			{
				return new MyObjectBuilder_RadialMenuItemCubeBlock();
			}

			MyObjectBuilder_RadialMenuItemCubeBlock IActivator<MyObjectBuilder_RadialMenuItemCubeBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public SerializableDefinitionId Id;
	}
}
