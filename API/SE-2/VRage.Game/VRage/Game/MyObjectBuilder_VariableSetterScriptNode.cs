using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_VariableSetterScriptNode : MyObjectBuilder_ScriptNode
	{
		protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003EVariableName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in string value)
			{
				owner.VariableName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out string value)
			{
				value = owner.VariableName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003EVariableValue_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in string value)
			{
				owner.VariableValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out string value)
			{
				value = owner.VariableValue;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003ESequenceInputID_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in int value)
			{
				owner.SequenceInputID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out int value)
			{
				value = owner.SequenceInputID;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003ESequenceInputs_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, List<int>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in List<int> value)
			{
				owner.SequenceInputs = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out List<int> value)
			{
				value = owner.SequenceInputs;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003ESequenceOutputID_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in int value)
			{
				owner.SequenceOutputID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out int value)
			{
				value = owner.SequenceOutputID;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003EValueInputID_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, MyVariableIdentifier>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in MyVariableIdentifier value)
			{
				owner.ValueInputID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out MyVariableIdentifier value)
			{
				value = owner.ValueInputID;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003EID_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EID_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in int value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VariableSetterScriptNode, MyObjectBuilder_ScriptNode>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out int value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VariableSetterScriptNode, MyObjectBuilder_ScriptNode>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003EPosition_003C_003EAccessor : VRage_Game_MyObjectBuilder_ScriptNode_003C_003EPosition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in Vector2 value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VariableSetterScriptNode, MyObjectBuilder_ScriptNode>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out Vector2 value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VariableSetterScriptNode, MyObjectBuilder_ScriptNode>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VariableSetterScriptNode, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VariableSetterScriptNode, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VariableSetterScriptNode, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VariableSetterScriptNode, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VariableSetterScriptNode, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VariableSetterScriptNode, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VariableSetterScriptNode, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VariableSetterScriptNode, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_VariableSetterScriptNode_003C_003EActor : IActivator, IActivator<MyObjectBuilder_VariableSetterScriptNode>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_VariableSetterScriptNode();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_VariableSetterScriptNode CreateInstance()
			{
				return new MyObjectBuilder_VariableSetterScriptNode();
			}

			MyObjectBuilder_VariableSetterScriptNode IActivator<MyObjectBuilder_VariableSetterScriptNode>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public string VariableName = string.Empty;

		[ProtoMember(5)]
		public string VariableValue = string.Empty;

		[ProtoMember(10)]
		public int SequenceInputID = -2;

		[ProtoMember(13)]
		public List<int> SequenceInputs = new List<int>();

		[ProtoMember(15)]
		public int SequenceOutputID = -1;

		[ProtoMember(20)]
		public MyVariableIdentifier ValueInputID = MyVariableIdentifier.Default;

		public override void AfterDeserialize()
		{
			if (SequenceInputID != -2)
			{
				if (SequenceInputID != -1)
				{
					SequenceInputs.Add(SequenceInputID);
				}
				SequenceInputID = -2;
			}
		}
	}
}
