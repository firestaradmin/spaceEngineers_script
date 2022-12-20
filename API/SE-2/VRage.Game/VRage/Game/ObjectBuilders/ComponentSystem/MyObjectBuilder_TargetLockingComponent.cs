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
	public class MyObjectBuilder_TargetLockingComponent : MyObjectBuilder_ComponentBase
	{
		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TargetLockingComponent_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetLockingComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetLockingComponent owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetLockingComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetLockingComponent owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetLockingComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TargetLockingComponent_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetLockingComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetLockingComponent owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetLockingComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetLockingComponent owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetLockingComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TargetLockingComponent_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetLockingComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetLockingComponent owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetLockingComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetLockingComponent owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetLockingComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TargetLockingComponent_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_TargetLockingComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_TargetLockingComponent owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetLockingComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_TargetLockingComponent owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_TargetLockingComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TargetLockingComponent_003C_003EActor : IActivator, IActivator<MyObjectBuilder_TargetLockingComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_TargetLockingComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_TargetLockingComponent CreateInstance()
			{
				return new MyObjectBuilder_TargetLockingComponent();
			}

			MyObjectBuilder_TargetLockingComponent IActivator<MyObjectBuilder_TargetLockingComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
