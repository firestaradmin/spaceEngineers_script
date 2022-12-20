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
	public class MyObjectBuilder_RadialMenuItemSystem : MyObjectBuilder_RadialMenuItem
	{
		protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem_003C_003ESystemAction_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in int value)
			{
				owner.SystemAction = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out int value)
			{
				value = owner.SystemAction;
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in List<string> value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_RadialMenuItem>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out List<string> value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_RadialMenuItem>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem_003C_003ELabelName_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003ELabelName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, MyStringId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in MyStringId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_RadialMenuItem>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out MyStringId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_RadialMenuItem>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem_003C_003ELabelShortcut_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003ELabelShortcut_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, MyStringId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in MyStringId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_RadialMenuItem>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out MyStringId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_RadialMenuItem>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem_003C_003ECloseMenu_003C_003EAccessor : VRage_Game_MyObjectBuilder_RadialMenuItem_003C_003ECloseMenu_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_RadialMenuItem>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_RadialMenuItem>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_RadialMenuItemSystem, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_RadialMenuItemSystem_003C_003EActor : IActivator, IActivator<MyObjectBuilder_RadialMenuItemSystem>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_RadialMenuItemSystem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_RadialMenuItemSystem CreateInstance()
			{
				return new MyObjectBuilder_RadialMenuItemSystem();
			}

			MyObjectBuilder_RadialMenuItemSystem IActivator<MyObjectBuilder_RadialMenuItemSystem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int SystemAction;
	}
}
