using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders
{
	/// <summary>
	/// Base class of all object builders of animation tree nodes.
	/// </summary>
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public abstract class MyObjectBuilder_AnimationTreeNode : MyObjectBuilder_Base
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003EEdPos_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNode, Vector2I?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNode owner, in Vector2I? value)
			{
				owner.EdPos = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNode owner, out Vector2I? value)
			{
				value = owner.EdPos;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003EEventNames_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNode, List<string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNode owner, in List<string> value)
			{
				owner.EventNames = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNode owner, out List<string> value)
			{
				value = owner.EventNames;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003EEventTimes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNode, List<double>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNode owner, in List<double> value)
			{
				owner.EventTimes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNode owner, out List<double> value)
			{
				value = owner.EventTimes;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003EKey_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNode owner, in string value)
			{
				owner.Key = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNode owner, out string value)
			{
				value = owner.Key;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNode owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AnimationTreeNode, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNode owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AnimationTreeNode, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNode owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AnimationTreeNode, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNode owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AnimationTreeNode, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNode, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNode owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AnimationTreeNode, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNode owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AnimationTreeNode, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNode, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_AnimationTreeNode owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AnimationTreeNode, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_AnimationTreeNode owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_AnimationTreeNode, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		/// <summary>
		/// Position in editor.
		/// </summary>
		[ProtoMember(1)]
		public Vector2I? EdPos;

		[ProtoMember(3)]
		public List<string> EventNames = new List<string>();

		[ProtoMember(5)]
		public List<double> EventTimes = new List<double>();

		[ProtoMember(7)]
		public string Key;

		/// <summary>
		/// Create deep copy of this node and its children.
		/// </summary>
		/// <param name="selectedNodes">the mask</param>
		/// <param name="parentNode">link to the parent node</param>        
		/// <param name="orphans">link to list of orphaned nodes</param>
		/// <returns>copied hierarchy</returns>
		protected internal abstract MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes, MyObjectBuilder_AnimationTreeNode parentNode, List<MyObjectBuilder_AnimationTreeNode> orphans);

		/// <summary>
		/// Get the child nodes of this node.
		/// </summary>
		public abstract MyObjectBuilder_AnimationTreeNode[] GetChildren();
	}
}
