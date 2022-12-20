using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.ComponentSystem
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_UpdateTrigger : MyObjectBuilder_TriggerBase
	{
		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003ESize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_UpdateTrigger, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in int value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out int value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003EIsPirateStation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_UpdateTrigger, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in bool value)
			{
				owner.IsPirateStation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out bool value)
			{
				value = owner.IsPirateStation;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003ESerializedPirateStation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_CubeGrid>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in MyObjectBuilder_CubeGrid value)
			{
				owner.SerializedPirateStation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out MyObjectBuilder_CubeGrid value)
			{
				value = owner.SerializedPirateStation;
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003EType_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EType_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_TriggerBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_TriggerBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003EAABB_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EAABB_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, SerializableBoundingBoxD>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in SerializableBoundingBoxD value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_TriggerBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out SerializableBoundingBoxD value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_TriggerBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003EBoundingSphere_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EBoundingSphere_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, SerializableBoundingSphereD>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in SerializableBoundingSphereD value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_TriggerBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out SerializableBoundingSphereD value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_TriggerBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003EOffset_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EOffset_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in SerializableVector3D value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_TriggerBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out SerializableVector3D value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_TriggerBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003EOrientedBoundingBox_003C_003EAccessor : VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_TriggerBase_003C_003EOrientedBoundingBox_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, SerializableOrientedBoundingBoxD>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in SerializableOrientedBoundingBoxD value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_TriggerBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out SerializableOrientedBoundingBoxD value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_TriggerBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_UpdateTrigger, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_UpdateTrigger owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_UpdateTrigger owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_UpdateTrigger, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_UpdateTrigger_003C_003EActor : IActivator, IActivator<MyObjectBuilder_UpdateTrigger>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_UpdateTrigger();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_UpdateTrigger CreateInstance()
			{
				return new MyObjectBuilder_UpdateTrigger();
			}

			MyObjectBuilder_UpdateTrigger IActivator<MyObjectBuilder_UpdateTrigger>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public int Size = 25000;

		[ProtoMember(2)]
		public bool IsPirateStation;

		[Nullable]
		[ProtoMember(3)]
		public MyObjectBuilder_CubeGrid SerializedPirateStation;
	}
}
