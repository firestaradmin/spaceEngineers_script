using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenBoard : MyGuiScreenBase
	{
		[Serializable]
		public struct MyColumn
		{
			protected class Sandbox_Game_Gui_MyGuiScreenBoard_003C_003EMyColumn_003C_003EHeaderText_003C_003EAccessor : IMemberAccessor<MyColumn, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyColumn owner, in string value)
				{
					owner.HeaderText = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyColumn owner, out string value)
				{
					value = owner.HeaderText;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenBoard_003C_003EMyColumn_003C_003EHeaderDrawAlign_003C_003EAccessor : IMemberAccessor<MyColumn, MyGuiDrawAlignEnum>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyColumn owner, in MyGuiDrawAlignEnum value)
				{
					owner.HeaderDrawAlign = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyColumn owner, out MyGuiDrawAlignEnum value)
				{
					value = owner.HeaderDrawAlign;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenBoard_003C_003EMyColumn_003C_003EColumnDrawAlign_003C_003EAccessor : IMemberAccessor<MyColumn, MyGuiDrawAlignEnum>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyColumn owner, in MyGuiDrawAlignEnum value)
				{
					owner.ColumnDrawAlign = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyColumn owner, out MyGuiDrawAlignEnum value)
				{
					value = owner.ColumnDrawAlign;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenBoard_003C_003EMyColumn_003C_003EWidth_003C_003EAccessor : IMemberAccessor<MyColumn, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyColumn owner, in float value)
				{
					owner.Width = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyColumn owner, out float value)
				{
					value = owner.Width;
				}
			}

			protected class Sandbox_Game_Gui_MyGuiScreenBoard_003C_003EMyColumn_003C_003EVisible_003C_003EAccessor : IMemberAccessor<MyColumn, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyColumn owner, in bool value)
				{
					owner.Visible = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyColumn owner, out bool value)
				{
					value = owner.Visible;
				}
			}

			public string HeaderText;

			public MyGuiDrawAlignEnum HeaderDrawAlign;

			public MyGuiDrawAlignEnum ColumnDrawAlign;

			public float Width;

			public bool Visible;
		}

		private class MyRow
		{
			public Dictionary<string, string> Cells;

			public int Ranking;

			public MyRow()
			{
				Cells = new Dictionary<string, string>();
			}
		}

		private MyGuiControlTable m_boardTable;

		private readonly StringBuilder m_textCache = new StringBuilder();

		private Dictionary<string, MyColumn> m_columns = new Dictionary<string, MyColumn>();

		private List<string> m_indexToColumnIdMap = new List<string>();

		private Dictionary<string, MyRow> m_rows = new Dictionary<string, MyRow>();

		private string m_sortByColumn;

		private bool m_sortAscending = true;

		private string m_showOrderColumn;

		private Vector2 m_normalizedSize;

		private Vector2 m_normalizedCoord;

		public MyGuiScreenBoard(Vector2 normalizedCoord, Vector2 localOffset, Vector2 size)
			: base(normalizedCoord + localOffset, null, size, isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_normalizedSize = size;
			m_normalizedCoord = normalizedCoord;
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenBoard";
		}

		protected override void OnClosed()
		{
			base.OnClosed();
		}

		private int TextComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
		{
			if (m_sortByColumn == null)
			{
				MyRow myRow = (MyRow)a.Row.UserData;
				MyRow myRow2 = (MyRow)b.Row.UserData;
				if (!m_sortAscending)
				{
					return myRow2.Ranking.CompareTo(myRow.Ranking);
				}
				return myRow.Ranking.CompareTo(myRow2.Ranking);
			}
			if (!m_sortAscending)
			{
				return b.Text.CompareToIgnoreCase(a.Text);
			}
			return a.Text.CompareToIgnoreCase(b.Text);
		}

		private void Clear()
		{
			m_columns.Clear();
			m_indexToColumnIdMap.Clear();
			m_rows.Clear();
			m_sortByColumn = null;
			m_sortAscending = true;
			m_showOrderColumn = null;
		}

		public void Init(MyObjectBuilder_BoardScreen ob)
		{
			Clear();
			m_sortByColumn = ob.SortByColumn;
			m_showOrderColumn = ob.ShowOrderColumn;
			m_sortAscending = ob.SortAscending;
			MyObjectBuilder_BoardScreen.BoardColumn[] columns = ob.Columns;
			for (int i = 0; i < columns.Length; i++)
			{
				MyObjectBuilder_BoardScreen.BoardColumn boardColumn = columns[i];
				AddColumn(boardColumn.Id, boardColumn.Width, boardColumn.HeaderText, boardColumn.HeaderDrawAlign, boardColumn.ColumnDrawAlign);
				if (!boardColumn.Visible)
				{
					SetColumnVisibility(boardColumn.Id, visible: false);
				}
			}
<<<<<<< HEAD
			m_indexToColumnIdMap = ((ob.ColumnSort != null) ? ob.ColumnSort.ToList() : new List<string>());
=======
			m_indexToColumnIdMap = ((ob.ColumnSort != null) ? Enumerable.ToList<string>((IEnumerable<string>)ob.ColumnSort) : new List<string>());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyObjectBuilder_BoardScreen.BoardRow[] rows = ob.Rows;
			for (int i = 0; i < rows.Length; i++)
			{
				MyObjectBuilder_BoardScreen.BoardRow boardRow = rows[i];
				AddRow(boardRow.Id);
				SetRowRanking(boardRow.Id, boardRow.Ranking);
				foreach (KeyValuePair<string, string> item in boardRow.Cells.Dictionary)
				{
					SetCell(boardRow.Id, item.Key, item.Value);
				}
			}
			Sort();
		}

		public MyObjectBuilder_BoardScreen GetBoardObjectBuilder(string id)
		{
			MyObjectBuilder_BoardScreen myObjectBuilder_BoardScreen = new MyObjectBuilder_BoardScreen();
			myObjectBuilder_BoardScreen.Id = id;
			myObjectBuilder_BoardScreen.SortByColumn = m_sortByColumn;
			myObjectBuilder_BoardScreen.ShowOrderColumn = m_showOrderColumn;
			myObjectBuilder_BoardScreen.SortAscending = m_sortAscending;
			myObjectBuilder_BoardScreen.Coords = m_normalizedCoord;
			myObjectBuilder_BoardScreen.Size = m_normalizedSize;
			myObjectBuilder_BoardScreen.ColumnSort = m_indexToColumnIdMap.ToArray();
<<<<<<< HEAD
			myObjectBuilder_BoardScreen.Columns = m_columns.Select(delegate(KeyValuePair<string, MyColumn> x)
=======
			myObjectBuilder_BoardScreen.Columns = Enumerable.ToArray<MyObjectBuilder_BoardScreen.BoardColumn>(Enumerable.Select<KeyValuePair<string, MyColumn>, MyObjectBuilder_BoardScreen.BoardColumn>((IEnumerable<KeyValuePair<string, MyColumn>>)m_columns, (Func<KeyValuePair<string, MyColumn>, MyObjectBuilder_BoardScreen.BoardColumn>)delegate(KeyValuePair<string, MyColumn> x)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyObjectBuilder_BoardScreen.BoardColumn result = default(MyObjectBuilder_BoardScreen.BoardColumn);
				result.Id = x.Key;
				result.Width = x.Value.Width;
				result.ColumnDrawAlign = x.Value.ColumnDrawAlign;
				result.HeaderDrawAlign = x.Value.HeaderDrawAlign;
				result.HeaderText = x.Value.HeaderText;
				result.Visible = x.Value.Visible;
				return result;
<<<<<<< HEAD
			}).ToArray();
=======
			}));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			List<MyObjectBuilder_BoardScreen.BoardRow> list = new List<MyObjectBuilder_BoardScreen.BoardRow>();
			foreach (KeyValuePair<string, MyRow> row in m_rows)
			{
				MyObjectBuilder_BoardScreen.BoardRow boardRow = default(MyObjectBuilder_BoardScreen.BoardRow);
				boardRow.Id = row.Key;
				boardRow.Ranking = row.Value.Ranking;
				MyObjectBuilder_BoardScreen.BoardRow item = boardRow;
				item.Cells = new SerializableDictionary<string, string>();
				foreach (KeyValuePair<string, string> cell in row.Value.Cells)
				{
					item.Cells.Dictionary.Add(cell.Key, cell.Value);
				}
				list.Add(item);
			}
			myObjectBuilder_BoardScreen.Rows = list.ToArray();
			return myObjectBuilder_BoardScreen;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			base.CloseButtonEnabled = false;
			base.CanHaveFocus = false;
			base.CanBeHidden = false;
			base.CanHideOthers = false;
			m_boardTable = new MyGuiControlTable(canHaveScrollbar: false);
			m_boardTable.Position = new Vector2(0f, 0f);
			m_boardTable.Size = m_size.Value;
			m_boardTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_boardTable.VisibleRowsCount = 1;
			m_boardTable.ColumnLinesVisible = true;
			m_boardTable.RowLinesVisible = true;
			m_boardTable.BackgroundTexture = MyGuiConstants.TEXTURE_HIGHLIGHT_DARK;
			m_boardTable.ColorMask = new Vector4(1f, 1f, 1f, 0.5f);
			Controls.Add(m_boardTable);
		}

		public void AddColumn(string columnId, float width, string headerText, MyGuiDrawAlignEnum headerDrawAlign, MyGuiDrawAlignEnum columnDrawAlign)
		{
			MyColumn myColumn = default(MyColumn);
			myColumn.Width = width;
			myColumn.HeaderText = headerText;
			myColumn.HeaderDrawAlign = headerDrawAlign;
			myColumn.ColumnDrawAlign = columnDrawAlign;
			myColumn.Visible = true;
			MyColumn column = myColumn;
			AddColumn(columnId, column);
		}

		public void AddColumn(string columnId, MyColumn column)
		{
			m_columns[columnId] = column;
			if (!m_indexToColumnIdMap.Contains(columnId))
			{
				m_indexToColumnIdMap.Add(columnId);
			}
			UpdateColumns();
			UpdateRows();
		}

		public void RemoveColumn(string columnId)
		{
			m_indexToColumnIdMap.RemoveAt(m_indexToColumnIdMap.FindIndex((string x) => x == columnId));
			m_columns.Remove(columnId);
			if (columnId == m_showOrderColumn)
			{
				m_showOrderColumn = null;
			}
			if (columnId == m_sortByColumn)
			{
				m_sortByColumn = null;
			}
			UpdateColumns();
			UpdateRows();
			Sort();
		}

		public void AddRow(string rowId)
		{
			m_rows[rowId] = new MyRow();
			UpdateRows();
			Sort();
		}

		public void RemoveRow(string rowId)
		{
			m_rows.Remove(rowId);
			UpdateRows();
			Sort();
		}

		private void UpdateRows()
		{
			m_boardTable.Clear();
			foreach (KeyValuePair<string, MyRow> row2 in m_rows)
			{
				MyGuiControlTable.Row row = new MyGuiControlTable.Row(row2.Value);
				for (int i = 0; i < m_indexToColumnIdMap.Count; i++)
				{
					string key = m_indexToColumnIdMap[i];
					string value = (row2.Value.Cells.ContainsKey(key) ? row2.Value.Cells[key] : "");
					row.AddCell(new MyGuiControlTable.Cell(new StringBuilder().Append(value), null, ""));
				}
				m_boardTable.Add(row);
			}
			m_boardTable.VisibleRowsCount = m_rows.Count;
		}

		public void SetCell(string rowId, string columnId, string text)
		{
			if (m_rows.TryGetValue(rowId, out var value))
			{
				value.Cells[columnId] = text;
			}
			UpdateRows();
			Sort();
		}

		public void SetRowRanking(string rowId, int ranking)
		{
			if (m_rows.TryGetValue(rowId, out var value))
			{
				value.Ranking = ranking;
			}
			Sort();
		}

		public void SortByColumn(string columnId, bool ascending)
		{
			m_sortByColumn = columnId;
			m_sortAscending = ascending;
			Sort();
		}

		public void SortByRanking(bool ascending)
		{
			m_sortByColumn = null;
			m_sortAscending = ascending;
			Sort();
		}

		public void ShowOrderInColumn(string columnId)
		{
			m_showOrderColumn = columnId;
			UpdateRows();
			Sort();
		}

		public void SetColumnVisibility(string columnId, bool visible)
		{
			if (m_columns.TryGetValue(columnId, out var value))
			{
				MyColumn value2 = value;
				value2.Visible = visible;
				m_columns[columnId] = value2;
			}
			UpdateColumns();
		}

		private void Sort()
		{
			if (m_sortByColumn != null)
			{
				int columnIdx = m_indexToColumnIdMap.FindIndex((string x) => x == m_sortByColumn);
				m_boardTable.SortByColumn(columnIdx, m_sortAscending ? MyGuiControlTable.SortStateEnum.Ascending : MyGuiControlTable.SortStateEnum.Descending);
			}
			else if (m_columns.Count > 0)
			{
				m_boardTable.SortByColumn(0, m_sortAscending ? MyGuiControlTable.SortStateEnum.Ascending : MyGuiControlTable.SortStateEnum.Descending);
			}
			if (m_showOrderColumn != null)
			{
				int cell = m_indexToColumnIdMap.FindIndex((string x) => x == m_showOrderColumn);
				for (int i = 0; i < m_boardTable.RowsCount; i++)
				{
					m_boardTable.GetRow(i).GetCell(cell).Text.Clear().Append(i + 1);
				}
			}
		}

		private void UpdateColumns()
		{
			m_boardTable.ColumnsCount = m_columns.Count;
<<<<<<< HEAD
			m_boardTable.SetCustomColumnWidths(m_columns.Values.Select((MyColumn x) => x.Width).ToArray());
=======
			m_boardTable.SetCustomColumnWidths(Enumerable.ToArray<float>(Enumerable.Select<MyColumn, float>((IEnumerable<MyColumn>)m_columns.Values, (Func<MyColumn, float>)((MyColumn x) => x.Width))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			for (int i = 0; i < m_indexToColumnIdMap.Count; i++)
			{
				string key = m_indexToColumnIdMap[i];
				m_boardTable.SetHeaderColumnAlign(i, m_columns[key].HeaderDrawAlign);
				m_boardTable.SetColumnAlign(i, m_columns[key].ColumnDrawAlign);
				m_boardTable.SetColumnName(i, new StringBuilder().Append(m_columns[key].HeaderText));
				m_boardTable.SetColumnComparison(i, TextComparison);
				m_boardTable.SetColumnVisibility(i, m_columns[key].Visible);
			}
		}
	}
}
