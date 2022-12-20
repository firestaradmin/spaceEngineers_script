using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.VisualScripting
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	[ProtoContract]
	public class MyObjectBuilder_ScriptStateMachineManager : MyObjectBuilder_Base
	{
		[ProtoContract]
		public struct CursorStruct
		{
			protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager_003C_003ECursorStruct_003C_003EStateMachineName_003C_003EAccessor : IMemberAccessor<CursorStruct, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CursorStruct owner, in string value)
				{
					owner.StateMachineName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CursorStruct owner, out string value)
				{
					value = owner.StateMachineName;
				}
			}

			protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager_003C_003ECursorStruct_003C_003ECursors_003C_003EAccessor : IMemberAccessor<CursorStruct, MyObjectBuilder_ScriptSMCursor[]>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref CursorStruct owner, in MyObjectBuilder_ScriptSMCursor[] value)
				{
					owner.Cursors = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref CursorStruct owner, out MyObjectBuilder_ScriptSMCursor[] value)
				{
					value = owner.Cursors;
				}
			}

			private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager_003C_003ECursorStruct_003C_003EActor : IActivator, IActivator<CursorStruct>
			{
				private sealed override object CreateInstance()
				{
					return default(CursorStruct);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override CursorStruct CreateInstance()
				{
					return (CursorStruct)(object)default(CursorStruct);
				}

				CursorStruct IActivator<CursorStruct>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(5)]
			public string StateMachineName;

			[ProtoMember(10)]
			public MyObjectBuilder_ScriptSMCursor[] Cursors;
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager_003C_003EActiveStateMachines_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptStateMachineManager, List<CursorStruct>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptStateMachineManager owner, in List<CursorStruct> value)
			{
				owner.ActiveStateMachines = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptStateMachineManager owner, out List<CursorStruct> value)
			{
				value = owner.ActiveStateMachines;
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptStateMachineManager, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptStateMachineManager owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ScriptStateMachineManager, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptStateMachineManager owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ScriptStateMachineManager, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptStateMachineManager, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptStateMachineManager owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ScriptStateMachineManager, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptStateMachineManager owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ScriptStateMachineManager, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptStateMachineManager, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptStateMachineManager owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ScriptStateMachineManager, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptStateMachineManager owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ScriptStateMachineManager, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptStateMachineManager, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ScriptStateMachineManager owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ScriptStateMachineManager, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ScriptStateMachineManager owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_ScriptStateMachineManager, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ScriptStateMachineManager_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ScriptStateMachineManager>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ScriptStateMachineManager();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ScriptStateMachineManager CreateInstance()
			{
				return new MyObjectBuilder_ScriptStateMachineManager();
			}

			MyObjectBuilder_ScriptStateMachineManager IActivator<MyObjectBuilder_ScriptStateMachineManager>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public List<CursorStruct> ActiveStateMachines;
	}
}
