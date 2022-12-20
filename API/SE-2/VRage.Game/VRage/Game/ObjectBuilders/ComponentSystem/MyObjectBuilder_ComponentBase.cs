using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public abstract class MyObjectBuilder_ComponentBase : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentBase_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentBase owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentBase, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentBase owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentBase, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentBase_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentBase owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentBase, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentBase owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentBase, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentBase_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentBase owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentBase, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentBase owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentBase, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_ComponentBase_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ComponentBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ComponentBase owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentBase, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ComponentBase owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ComponentBase, MyObjectBuilder_Base>(ref owner), out value);
			}
		}
	}
}
