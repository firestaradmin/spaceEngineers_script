using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	[ProtoContract]
	public class MyObjectBuilder_BoardScreen : MyObjectBuilder_SessionComponent
	{
		[ProtoContract]
		public struct BoardColumn
		{
			protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EBoardColumn_003C_003EId_003C_003EAccessor : IMemberAccessor<BoardColumn, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BoardColumn owner, in string value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BoardColumn owner, out string value)
				{
					value = owner.Id;
				}
			}

			protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EBoardColumn_003C_003EWidth_003C_003EAccessor : IMemberAccessor<BoardColumn, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BoardColumn owner, in float value)
				{
					owner.Width = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BoardColumn owner, out float value)
				{
					value = owner.Width;
				}
			}

			protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EBoardColumn_003C_003EHeaderText_003C_003EAccessor : IMemberAccessor<BoardColumn, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BoardColumn owner, in string value)
				{
					owner.HeaderText = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BoardColumn owner, out string value)
				{
					value = owner.HeaderText;
				}
			}

			protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EBoardColumn_003C_003EHeaderDrawAlign_003C_003EAccessor : IMemberAccessor<BoardColumn, MyGuiDrawAlignEnum>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BoardColumn owner, in MyGuiDrawAlignEnum value)
				{
					owner.HeaderDrawAlign = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BoardColumn owner, out MyGuiDrawAlignEnum value)
				{
					value = owner.HeaderDrawAlign;
				}
			}

			protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EBoardColumn_003C_003EColumnDrawAlign_003C_003EAccessor : IMemberAccessor<BoardColumn, MyGuiDrawAlignEnum>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BoardColumn owner, in MyGuiDrawAlignEnum value)
				{
					owner.ColumnDrawAlign = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BoardColumn owner, out MyGuiDrawAlignEnum value)
				{
					value = owner.ColumnDrawAlign;
				}
			}

			protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EBoardColumn_003C_003EVisible_003C_003EAccessor : IMemberAccessor<BoardColumn, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BoardColumn owner, in bool value)
				{
					owner.Visible = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BoardColumn owner, out bool value)
				{
					value = owner.Visible;
				}
			}

			private class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EBoardColumn_003C_003EActor : IActivator, IActivator<BoardColumn>
			{
				private sealed override object CreateInstance()
				{
					return default(BoardColumn);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override BoardColumn CreateInstance()
				{
					return (BoardColumn)(object)default(BoardColumn);
				}

				BoardColumn IActivator<BoardColumn>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(50)]
			public string Id;

			[ProtoMember(55)]
			public float Width;

			[ProtoMember(60)]
			public string HeaderText;

			[ProtoMember(65)]
			public MyGuiDrawAlignEnum HeaderDrawAlign;

			[ProtoMember(70)]
			public MyGuiDrawAlignEnum ColumnDrawAlign;

			[ProtoMember(80)]
			public bool Visible;
		}

		[ProtoContract]
		public struct BoardRow
		{
			protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EBoardRow_003C_003EId_003C_003EAccessor : IMemberAccessor<BoardRow, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BoardRow owner, in string value)
				{
					owner.Id = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BoardRow owner, out string value)
				{
					value = owner.Id;
				}
			}

			protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EBoardRow_003C_003ERanking_003C_003EAccessor : IMemberAccessor<BoardRow, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BoardRow owner, in int value)
				{
					owner.Ranking = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BoardRow owner, out int value)
				{
					value = owner.Ranking;
				}
			}

			protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EBoardRow_003C_003ECells_003C_003EAccessor : IMemberAccessor<BoardRow, SerializableDictionary<string, string>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref BoardRow owner, in SerializableDictionary<string, string> value)
				{
					owner.Cells = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref BoardRow owner, out SerializableDictionary<string, string> value)
				{
					value = owner.Cells;
				}
			}

			private class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EBoardRow_003C_003EActor : IActivator, IActivator<BoardRow>
			{
				private sealed override object CreateInstance()
				{
					return default(BoardRow);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override BoardRow CreateInstance()
				{
					return (BoardRow)(object)default(BoardRow);
				}

				BoardRow IActivator<BoardRow>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(150)]
			public string Id;

			[ProtoMember(160)]
			public int Ranking;

			[ProtoMember(170)]
			public SerializableDictionary<string, string> Cells;
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in string value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out string value)
			{
				value = owner.Id;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003ECoords_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, SerializableVector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in SerializableVector2 value)
			{
				owner.Coords = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out SerializableVector2 value)
			{
				value = owner.Coords;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003ESize_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, SerializableVector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in SerializableVector2 value)
			{
				owner.Size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out SerializableVector2 value)
			{
				value = owner.Size;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003ESortByColumn_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in string value)
			{
				owner.SortByColumn = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out string value)
			{
				value = owner.SortByColumn;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EShowOrderColumn_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in string value)
			{
				owner.ShowOrderColumn = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out string value)
			{
				value = owner.ShowOrderColumn;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003ESortAscending_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in bool value)
			{
				owner.SortAscending = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out bool value)
			{
				value = owner.SortAscending;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EColumns_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, BoardColumn[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in BoardColumn[] value)
			{
				owner.Columns = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out BoardColumn[] value)
			{
				value = owner.Columns;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003ERows_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, BoardRow[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in BoardRow[] value)
			{
				owner.Rows = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out BoardRow[] value)
			{
				value = owner.Rows;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EColumnSort_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_BoardScreen, string[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in string[] value)
			{
				owner.ColumnSort = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out string[] value)
			{
				value = owner.ColumnSort;
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BoardScreen, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_BoardScreen, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_BoardScreen, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BoardScreen, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_BoardScreen, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_BoardScreen, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BoardScreen, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in SerializableDefinitionId? value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_BoardScreen, MyObjectBuilder_SessionComponent>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out SerializableDefinitionId? value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_BoardScreen, MyObjectBuilder_SessionComponent>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BoardScreen, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in MyStringHash value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_BoardScreen, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out MyStringHash value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_BoardScreen, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_BoardScreen_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_BoardScreen, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_BoardScreen owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_BoardScreen, MyObjectBuilder_Base>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_BoardScreen owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyObjectBuilder_BoardScreen, MyObjectBuilder_Base>(ref owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_BoardScreen_003C_003EActor : IActivator, IActivator<MyObjectBuilder_BoardScreen>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_BoardScreen();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_BoardScreen CreateInstance()
			{
				return new MyObjectBuilder_BoardScreen();
			}

			MyObjectBuilder_BoardScreen IActivator<MyObjectBuilder_BoardScreen>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public string Id;

		[ProtoMember(6)]
		public SerializableVector2 Coords;

		[ProtoMember(7)]
		public SerializableVector2 Size;

		[Nullable]
		[ProtoMember(10)]
		public string SortByColumn;

		[Nullable]
		[ProtoMember(20)]
		public string ShowOrderColumn;

		[ProtoMember(30)]
		public bool SortAscending;

		[XmlArray("Columns", IsNullable = true)]
		[ProtoMember(40)]
		public BoardColumn[] Columns;

		[XmlArray("Rows", IsNullable = true)]
		[ProtoMember(50)]
		public BoardRow[] Rows;

		[XmlArray("ColumnSort", IsNullable = true)]
		[ProtoMember(60)]
		public string[] ColumnSort;
	}
}
