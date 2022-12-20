using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	[ProtoContract]
	public class MyObjectBuilder_VisualScriptManagerSessionComponent : MyObjectBuilder_SessionComponent
	{
		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003EFirstRun_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in bool value)
			{
				owner.FirstRun = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out bool value)
			{
				value = owner.FirstRun;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003ELevelScriptFiles_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in string[] value)
			{
				owner.LevelScriptFiles = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out string[] value)
			{
				value = owner.LevelScriptFiles;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003EStateMachines_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in string[] value)
			{
				owner.StateMachines = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out string[] value)
			{
				value = owner.StateMachines;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003EScriptStateMachineManager_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_ScriptStateMachineManager>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in MyObjectBuilder_ScriptStateMachineManager value)
			{
				owner.ScriptStateMachineManager = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out MyObjectBuilder_ScriptStateMachineManager value)
			{
				value = owner.ScriptStateMachineManager;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003EQuestlog_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_Questlog>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in MyObjectBuilder_Questlog value)
			{
				owner.Questlog = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out MyObjectBuilder_Questlog value)
			{
				value = owner.Questlog;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003EExclusiveHighlights_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_ExclusiveHighlights>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in MyObjectBuilder_ExclusiveHighlights value)
			{
				owner.ExclusiveHighlights = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out MyObjectBuilder_ExclusiveHighlights value)
			{
				value = owner.ExclusiveHighlights;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003EWorldOutlineFolders_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in string[] value)
			{
				owner.WorldOutlineFolders = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out string[] value)
			{
				value = owner.WorldOutlineFolders;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003EBoardScreens_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_BoardScreen[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in MyObjectBuilder_BoardScreen[] value)
			{
				owner.BoardScreens = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out MyObjectBuilder_BoardScreen[] value)
			{
				value = owner.BoardScreens;
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in SerializableDefinitionId? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_SessionComponent>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out SerializableDefinitionId? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_SessionComponent>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_VisualScriptManagerSessionComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_VisualScriptManagerSessionComponent owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_VisualScriptManagerSessionComponent, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_VisualScriptManagerSessionComponent_003C_003EActor : IActivator, IActivator<MyObjectBuilder_VisualScriptManagerSessionComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_VisualScriptManagerSessionComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_VisualScriptManagerSessionComponent CreateInstance()
			{
				return new MyObjectBuilder_VisualScriptManagerSessionComponent();
			}

			MyObjectBuilder_VisualScriptManagerSessionComponent IActivator<MyObjectBuilder_VisualScriptManagerSessionComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool FirstRun = true;

		[XmlArray("LevelScriptFiles", IsNullable = true)]
		[XmlArrayItem("FilePath")]
		[ProtoMember(5)]
		public string[] LevelScriptFiles;

		[XmlArray("StateMachines", IsNullable = true)]
		[XmlArrayItem("FilePath")]
		[ProtoMember(10)]
		public string[] StateMachines;

		[DefaultValue(null)]
		[ProtoMember(15)]
		public MyObjectBuilder_ScriptStateMachineManager ScriptStateMachineManager;

		[DefaultValue(null)]
		[ProtoMember(20)]
		public MyObjectBuilder_Questlog Questlog;

		[DefaultValue(null)]
		[ProtoMember(22)]
		public MyObjectBuilder_ExclusiveHighlights ExclusiveHighlights;

		[ProtoMember(25)]
		public string[] WorldOutlineFolders;

		[ProtoMember(30)]
		public MyObjectBuilder_BoardScreen[] BoardScreens;
	}
}
