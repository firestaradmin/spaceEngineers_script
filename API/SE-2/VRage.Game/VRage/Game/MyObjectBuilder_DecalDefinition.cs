using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageRender;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_DecalDefinition : MyObjectBuilder_DefinitionBase
	{
		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EMaterial_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, MyDecalMaterialDesc>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in MyDecalMaterialDesc value)
			{
				owner.Material = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out MyDecalMaterialDesc value)
			{
				value = owner.Material;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003ETarget_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in string value)
			{
				owner.Target = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out string value)
			{
				value = owner.Target;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003ESource_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in string value)
			{
				owner.Source = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out string value)
			{
				value = owner.Source;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EMinSize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in float value)
			{
				owner.MinSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out float value)
			{
				value = owner.MinSize;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EMaxSize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in float value)
			{
				owner.MaxSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out float value)
			{
				value = owner.MaxSize;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EDepth_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in float value)
			{
				owner.Depth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out float value)
			{
				value = owner.Depth;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003ERotation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in float value)
			{
				owner.Rotation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out float value)
			{
				value = owner.Rotation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003ETransparent_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in bool value)
			{
				owner.Transparent = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out bool value)
			{
				value = owner.Transparent;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EXOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in float value)
			{
				owner.XOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out float value)
			{
				value = owner.XOffset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EYOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in float value)
			{
				owner.YOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out float value)
			{
				value = owner.YOffset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EAlpha_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in float value)
			{
				owner.Alpha = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out float value)
			{
				value = owner.Alpha;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003ESpacing_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in float value)
			{
				owner.Spacing = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out float value)
			{
				value = owner.Spacing;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EBlacklist_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in string[] value)
			{
				owner.Blacklist = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out string[] value)
			{
				value = owner.Blacklist;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003ERenderDistance_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_DecalDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in float value)
			{
				owner.RenderDistance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out float value)
			{
				value = owner.RenderDistance;
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EId_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, SerializableDefinitionId>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in SerializableDefinitionId value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out SerializableDefinitionId value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EDisplayName_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDisplayName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EDescription_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescription_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EIcons_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EIcons_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EPublic_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EPublic_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EEnabled_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EEnabled_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EAvailableInSurvival_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EAvailableInSurvival_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in bool value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out bool value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EDescriptionArgs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDescriptionArgs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EDLCs_003C_003EAccessor : VRage_Game_MyObjectBuilder_DefinitionBase_003C_003EDLCs_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in string[] value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out string[] value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_DefinitionBase>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_DecalDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_DecalDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_DecalDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_DecalDefinition, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_DecalDefinition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_DecalDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_DecalDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_DecalDefinition CreateInstance()
			{
				return new MyObjectBuilder_DecalDefinition();
			}

			MyObjectBuilder_DecalDefinition IActivator<MyObjectBuilder_DecalDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyDecalMaterialDesc Material;

		[ProtoMember(4)]
		public string Target = string.Empty;

		[ProtoMember(7)]
		public string Source = string.Empty;

		[ProtoMember(10)]
		public float MinSize = 1f;

		[ProtoMember(13)]
		public float MaxSize = 2f;

		[ProtoMember(16)]
		public float Depth = 0.2f;

		[ProtoMember(19)]
		public float Rotation = float.PositiveInfinity;

		[ProtoMember(22)]
		public bool Transparent;

		[ProtoMember(25)]
		public float XOffset;

		[ProtoMember(28)]
		public float YOffset;

		[ProtoMember(31)]
		public float Alpha = 1f;

		[ProtoMember(34)]
		public float Spacing = -1f;

		[XmlArray(ElementName = "Blacklist")]
		[XmlArrayItem("VoxelMaterial")]
		[ProtoMember(37)]
		public string[] Blacklist;

		[ProtoMember(40)]
		public float RenderDistance = 50f;
	}
}
