using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlTable : MyGuiControlAutoScaleText
	{
		public class StyleDefinition
		{
			public string HeaderFontHighlight;

			public string HeaderFontNormal;

			public string HeaderTextureHighlight;

			public MyGuiBorderThickness Padding;

			public string RowFontHighlight;

			public string RowFontNormal;

			public float RowHeight;

			public string RowTextureHighlight;

			public string RowTextureFocus;

			public string RowTextureActive;

			public float TextScale;

			public MyGuiBorderThickness ScrollbarMargin;

			public MyGuiCompositeTexture Texture;

			public Vector4? TextColorHighlight;

			public Vector4? TextColorFocus;

			public Vector4? TextColorActive;

			public Vector4? TextColor;
		}

		public delegate bool EqualUserData(object first, object second);

		public struct EventArgs
		{
			public int RowIndex;

			public MyMouseButtonsEnum MouseButton;
		}

		public class Cell
		{
			public readonly StringBuilder Text;

			public readonly object UserData;

			public readonly MyToolTips ToolTip;

			public MyGuiHighlightTexture? Icon;

			public readonly MyGuiDrawAlignEnum IconOriginAlign;

			public Color? TextColor;

			public Thickness Margin;

			public MyGuiControlBase Control;

			public bool IsAutoScaleEnabled;
<<<<<<< HEAD
=======

			public Row Row;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

			public Row Row;

			public float TextScale { get; set; }

			public Cell(string text = null, object userData = null, string toolTip = null, Color? textColor = null, MyGuiHighlightTexture? icon = null, MyGuiDrawAlignEnum iconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				if (text != null)
				{
					Text = new StringBuilder().Append(text);
				}
				if (toolTip != null)
				{
					ToolTip = new MyToolTips(toolTip);
				}
				UserData = userData;
				Icon = icon;
				IconOriginAlign = iconOriginAlign;
				TextColor = textColor;
				Margin = new Thickness(0f);
			}

			public Cell(StringBuilder text, object userData = null, string toolTip = null, Color? textColor = null, MyGuiHighlightTexture? icon = null, MyGuiDrawAlignEnum iconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				if (text != null)
				{
					Text = new StringBuilder().AppendStringBuilder(text);
				}
				if (toolTip != null)
				{
					ToolTip = new MyToolTips(toolTip);
				}
				UserData = userData;
				Icon = icon;
				IconOriginAlign = iconOriginAlign;
				TextColor = textColor;
				Margin = new Thickness(0f);
			}

			public virtual void Update()
			{
			}
		}

		public class Row
		{
			internal readonly List<Cell> Cells;

			public readonly object UserData;

			public readonly MyToolTips ToolTip;
<<<<<<< HEAD

			public bool IsGlobalSortEnabled { get; set; }

=======

			public bool IsGlobalSortEnabled { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public Row(object userData = null, string toolTip = null)
			{
				IsGlobalSortEnabled = true;
				UserData = userData;
				if (toolTip != null)
				{
					ToolTip = new MyToolTips(toolTip);
				}
				Cells = new List<Cell>();
			}

			public void AddCell(Cell cell)
			{
				Cells.Add(cell);
				cell.Row = this;
			}

			public Cell GetCell(int cell)
			{
				return Cells[cell];
			}

			public void Update()
			{
				foreach (Cell cell in Cells)
				{
					cell.Update();
				}
			}
		}

		public enum SortStateEnum
		{
			Unsorted,
			Ascending,
			Descending
		}

		protected class ColumnMetaData
		{
			public StringBuilder Name;

			public float Width;

			public Comparison<Cell> AscendingComparison;

			public SortStateEnum SortState;

			public MyGuiDrawAlignEnum TextAlign;

			public MyGuiDrawAlignEnum HeaderTextAlign;

			public Thickness Margin;

			public bool Visible;

			public float VisibleWidth;

			public ColumnMetaData()
			{
				Name = new StringBuilder();
				SortState = SortStateEnum.Unsorted;
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				HeaderTextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				Margin = new Thickness(0.01f);
				Visible = true;
			}
		}

		private static StyleDefinition[] m_styles;

		private MyGuiControls m_controls;

		protected List<ColumnMetaData> m_columnsMetaData;

		protected List<Row> m_rows;

		protected Vector2 m_doubleClickFirstPosition;

		protected int? m_doubleClickStarted;

		protected bool m_mouseOverHeader;

		protected int? m_mouseOverColumnIndex;

		protected int? m_mouseOverRowIndex;

		protected int? m_gamepadSortIndex;

		private int? m_gamepadSortIndex;

		private RectangleF m_headerArea;

		protected RectangleF m_rowsArea;

		protected StyleDefinition m_styleDef;

		protected MyVScrollbar m_scrollBar;

		/// <summary>
		/// Index computed from scrollbar.
		/// </summary>
		protected int m_visibleRowIndexOffset;

		private int m_lastSortedColumnIdx;

		private float m_textScale;

		private float m_textScaleWithLanguage;

		private int m_sortColumn = -1;

		private SortStateEnum? m_sortColumnState;

		private bool m_headerVisible = true;

		private int m_columnsCount = 1;

		private int? m_selectedRowIndex;

		private int m_visibleRows = 1;

		protected bool m_drawSingleSelectRows = true;

		private MyGuiControlTableStyleEnum m_visualStyle;

		protected Vector2 m_entryPoint;

		protected MyDirection m_entryDirection;

		public MyGuiControls Controls => m_controls;

		public MyVScrollbar ScrollBar => m_scrollBar;

		public bool HeaderVisible
		{
			get
			{
				return m_headerVisible;
			}
			set
			{
				m_headerVisible = value;
				RefreshInternals();
			}
		}

		public bool ColumnLinesVisible { get; set; }

		public bool RowLinesVisible { get; set; }

		public int ColumnsCount
		{
			get
			{
				return m_columnsCount;
			}
			set
			{
				m_columnsCount = value;
				RefreshInternals();
			}
		}

		public int? SelectedRowIndex
		{
			get
			{
				return m_selectedRowIndex;
			}
			set
			{
				m_selectedRowIndex = value;
			}
		}

		public Row SelectedRow
		{
			get
			{
				if (IsValidRowIndex(SelectedRowIndex))
				{
					return m_rows[SelectedRowIndex.Value];
				}
				return null;
			}
			set
			{
				int num = m_rows.IndexOf(value);
				if (num >= 0)
				{
					m_selectedRowIndex = num;
				}
			}
		}

		public int VisibleRowsCount
		{
			get
			{
				return m_visibleRows;
			}
			set
			{
				m_visibleRows = value;
				RefreshInternals();
			}
		}

		public bool IgnoreFirstRowForSort { get; set; }

<<<<<<< HEAD
		public bool HasItemConfirmedEvent => this.ItemConfirmed != null;

		public bool HasItemSelectedEvent => this.ItemSelected != null;

		public bool HasColumnClickedEvent => this.ItemSelected != null;

		public bool HasItemDoubleClickedEvent => this.ItemSelected != null;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public float RowHeight { get; private set; }

		public MyGuiControlTableStyleEnum VisualStyle
		{
			get
			{
				return m_visualStyle;
			}
			set
			{
				m_visualStyle = value;
				RefreshVisualStyle();
			}
		}

		public float TextScale
		{
			get
			{
				return m_textScale;
			}
			private set
			{
				m_textScale = value;
				TextScaleWithLanguage = value * MyGuiManager.LanguageTextScale;
			}
		}

		public float TextScaleWithLanguage
		{
			get
			{
				return m_textScaleWithLanguage;
			}
			private set
			{
				m_textScaleWithLanguage = value;
			}
		}

		public int RowsCount => m_rows.Count;

		public override RectangleF FocusRectangle => GetRowRectangle(SelectedRowIndex);

		public event Action<MyGuiControlTable, EventArgs> ItemDoubleClicked;

		public event Action<MyGuiControlTable, EventArgs> ItemSelected;

		public event Action<MyGuiControlTable, EventArgs> ItemConfirmed;

		public event Action<MyGuiControlTable, int> ColumnClicked;

		public event Action<Row> ItemMouseOver;

		public event Action<Row> ItemFocus;

		static MyGuiControlTable()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlTableStyleEnum>() + 1];
			StyleDefinition[] styles = m_styles;
			StyleDefinition obj = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				RowTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				RowTextureFocus = "Textures\\GUI\\Controls\\item_focus_dark.dds",
				RowTextureActive = "Textures\\GUI\\Controls\\item_active_dark.dds",
				HeaderTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				RowFontNormal = "Blue",
				RowFontHighlight = "White",
				HeaderFontNormal = "White",
				HeaderFontHighlight = "White",
				TextScale = 0.8f,
				RowHeight = 40f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			MyGuiBorderThickness myGuiBorderThickness = new MyGuiBorderThickness
			{
				Left = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj.Padding = myGuiBorderThickness;
			myGuiBorderThickness = new MyGuiBorderThickness
			{
				Left = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj.ScrollbarMargin = myGuiBorderThickness;
			obj.TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			styles[0] = obj;
			StyleDefinition[] styles2 = m_styles;
			StyleDefinition obj2 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				RowTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				HeaderTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				RowFontNormal = "White",
				RowFontHighlight = "White",
				HeaderFontNormal = "White",
				HeaderFontHighlight = "White",
				TextScale = 0.8f,
				RowHeight = 40f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			myGuiBorderThickness = new MyGuiBorderThickness
			{
				Left = 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj2.Padding = myGuiBorderThickness;
			myGuiBorderThickness = new MyGuiBorderThickness
			{
				Left = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj2.ScrollbarMargin = myGuiBorderThickness;
			styles2[1] = obj2;
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlTableStyleEnum style)
		{
			return m_styles[(int)style];
		}

<<<<<<< HEAD
		protected void RaiseItemConfirmed(MyGuiControlTable table, EventArgs args)
		{
			if (this.ItemConfirmed != null)
			{
				this.ItemConfirmed(table, args);
			}
		}

		protected void RaiseItemSelected(MyGuiControlTable table, EventArgs args)
		{
			if (this.ItemSelected != null)
			{
				this.ItemSelected(table, args);
			}
		}

		protected void RaiseColumnClicked(MyGuiControlTable table, int column)
		{
			if (this.ColumnClicked != null)
			{
				this.ColumnClicked(table, column);
			}
		}

		protected void RaiseItemDoubleClicked(MyGuiControlTable table, EventArgs args)
		{
			if (this.ItemDoubleClicked != null)
			{
				this.ItemDoubleClicked(table, args);
			}
		}

		public MyGuiControlTable(bool canHaveScrollbar = true)
			: base(null, null, null, isActiveControl: true, canHaveFocus: true)
=======
		public MyGuiControlTable(bool canHaveScrollbar = true)
			: base(null, null, null, null, null, isActiveControl: true, canHaveFocus: true)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (canHaveScrollbar)
			{
				m_scrollBar = new MyVScrollbar(this);
				m_scrollBar.ValueChanged += verticalScrollBar_ValueChanged;
			}
			m_rows = new List<Row>();
			m_columnsMetaData = new List<ColumnMetaData>();
			VisualStyle = MyGuiControlTableStyleEnum.Default;
			m_controls = new MyGuiControls(null);
			base.Name = "Table";
			base.CanFocusChildren = true;
			BorderHighlightEnabled = true;
			base.GamepadHelpTextId = MyCommonTexts.Gamepad_Help_Table;
			UpdateTableSortHelpText();
		}

		public void Add(Row row)
		{
			m_rows.Add(row);
			RefreshScrollbar();
		}

		public void Insert(int index, Row row)
		{
			m_rows.Insert(index, row);
			RefreshScrollbar();
		}

		public override void Clear()
		{
			foreach (Row row in m_rows)
			{
				foreach (Cell cell in row.Cells)
				{
					if (cell.Control != null)
					{
						cell.Control.OnRemoving();
						cell.Control.Clear();
					}
				}
			}
			m_rows.Clear();
			SelectedRowIndex = null;
			RefreshScrollbar();
		}

		public Row GetRow(int index)
		{
			return m_rows[index];
		}

		public Row Find(Predicate<Row> match)
		{
			return m_rows.Find(match);
		}

		public int FindIndex(Predicate<Row> match)
		{
			return m_rows.FindIndex(match);
		}

		public int FindIndexByUserData(ref object data, EqualUserData equals)
		{
			int num = -1;
			foreach (Row row in m_rows)
			{
				num++;
				if (row.UserData == null)
				{
					if (data == null)
					{
						return num;
					}
				}
				else if (equals == null)
				{
					if (row.UserData == data)
					{
						return num;
					}
				}
				else if (equals(row.UserData, data))
				{
					return num;
				}
			}
			return -1;
		}

		public void Remove(Row row)
		{
			int num = m_rows.IndexOf(row);
			if (num != -1)
			{
				m_rows.RemoveAt(num);
				if (SelectedRowIndex.HasValue && SelectedRowIndex.Value != num && SelectedRowIndex.Value > num)
				{
					SelectedRowIndex = SelectedRowIndex.Value - 1;
				}
			}
		}

		public void Remove(Predicate<Row> match)
		{
			int num = m_rows.FindIndex(match);
			if (num != -1)
			{
				m_rows.RemoveAt(num);
				if (SelectedRowIndex.HasValue && SelectedRowIndex.Value != num && SelectedRowIndex.Value > num)
				{
					SelectedRowIndex = SelectedRowIndex.Value - 1;
				}
			}
		}

		public void RemoveSelectedRow()
		{
			if (SelectedRowIndex.HasValue && SelectedRowIndex.Value < m_rows.Count)
			{
				m_rows.RemoveAt(SelectedRowIndex.Value);
				if (!IsValidRowIndex(SelectedRowIndex.Value))
				{
					SelectedRowIndex = null;
				}
				RefreshScrollbar();
			}
		}

		public void MoveSelectedRowUp()
		{
			if (SelectedRow != null && IsValidRowIndex(SelectedRowIndex - 1))
			{
				Row value = m_rows[SelectedRowIndex.Value - 1];
				m_rows[SelectedRowIndex.Value - 1] = m_rows[SelectedRowIndex.Value];
				m_rows[SelectedRowIndex.Value] = value;
				SelectedRowIndex--;
			}
		}

		public void MoveSelectedRowDown()
		{
			if (SelectedRow != null && IsValidRowIndex(SelectedRowIndex + 1))
			{
				Row value = m_rows[SelectedRowIndex.Value + 1];
				m_rows[SelectedRowIndex.Value + 1] = m_rows[SelectedRowIndex.Value];
				m_rows[SelectedRowIndex.Value] = value;
				SelectedRowIndex++;
			}
		}

		public void MoveSelectedRowTop()
		{
			if (SelectedRow != null)
			{
				Row selectedRow = SelectedRow;
				RemoveSelectedRow();
				m_rows.Insert(0, selectedRow);
				SelectedRowIndex = 0;
			}
		}

		public void MoveSelectedRowBottom()
		{
			if (SelectedRow != null)
			{
				Row selectedRow = SelectedRow;
				RemoveSelectedRow();
				m_rows.Add(selectedRow);
				SelectedRowIndex = RowsCount - 1;
			}
		}

		public void MoveToNextRow()
		{
			if (m_rows.Count == 0)
			{
				return;
			}
			if (!SelectedRowIndex.HasValue)
			{
				SelectedRowIndex = 0;
				return;
			}
			int val = SelectedRowIndex.Value + 1;
			val = Math.Min(val, m_rows.Count - 1);
			if (val != SelectedRowIndex.Value)
			{
				SelectedRowIndex = val;
				this.ItemSelected.InvokeIfNotNull(this, new EventArgs
				{
					RowIndex = SelectedRowIndex.Value,
					MouseButton = MyMouseButtonsEnum.Left
				});
				ScrollToSelection();
			}
		}

		public void MoveToPreviousRow()
		{
			if (m_rows.Count == 0)
			{
				return;
			}
			if (!SelectedRowIndex.HasValue)
			{
				SelectedRowIndex = 0;
				return;
			}
			int val = SelectedRowIndex.Value - 1;
			val = Math.Max(val, 0);
			if (val != SelectedRowIndex.Value)
			{
				SelectedRowIndex = val;
				this.ItemSelected.InvokeIfNotNull(this, new EventArgs
				{
					RowIndex = SelectedRowIndex.Value,
					MouseButton = MyMouseButtonsEnum.Left
				});
				ScrollToSelection();
			}
		}

		public void SetColumnName(int colIdx, StringBuilder name)
		{
			m_columnsMetaData[colIdx].Name.Clear().AppendStringBuilder(name);
		}

		public void SetColumnVisibility(int colIdx, bool visible)
		{
			m_columnsMetaData[colIdx].Visible = visible;
			RecalculateVisibleWidths();
		}

		public void SetColumnComparison(int colIdx, Comparison<Cell> ascendingComparison)
		{
			m_columnsMetaData[colIdx].AscendingComparison = ascendingComparison;
		}

		/// <summary>
		/// Modifies width of each column. Note that widths are relative to the width of table (excluding slider),
		/// so they should sum up to 1. Setting widths to 0.75 and 0.25 for 2 column table will give 3/4 of size to
		/// one column and 1/4 to the second one.
		/// </summary>
		public void SetCustomColumnWidths(float[] p)
		{
			for (int i = 0; i < ColumnsCount; i++)
			{
				m_columnsMetaData[i].Width = p[i];
			}
			RecalculateVisibleWidths();
		}

		private void RecalculateVisibleWidths()
		{
			float num = 0f;
			for (int i = 0; i < m_columnsMetaData.Count; i++)
			{
				bool flag = false;
				float num2 = 0f;
				for (int j = i + 1; j < m_columnsMetaData.Count; j++)
				{
					num2 += m_columnsMetaData[j].Width;
					if (m_columnsMetaData[j].Visible)
					{
						flag = true;
						break;
					}
				}
				if (m_columnsMetaData[i].Visible)
				{
					m_columnsMetaData[i].VisibleWidth = m_columnsMetaData[i].Width + num;
					num = 0f;
					if (!flag)
					{
						m_columnsMetaData[i].VisibleWidth += num2;
					}
				}
				else
				{
					num += m_columnsMetaData[i].Width;
				}
			}
		}

		public void SetColumnAlign(int colIdx, MyGuiDrawAlignEnum align = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
		{
			m_columnsMetaData[colIdx].TextAlign = align;
		}

		public void SetHeaderColumnAlign(int colIdx, MyGuiDrawAlignEnum align = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
		{
			m_columnsMetaData[colIdx].HeaderTextAlign = align;
		}

		public void SetHeaderColumnMargin(int colIdx, Thickness margin)
		{
			m_columnsMetaData[colIdx].Margin = margin;
		}

		public void ScrollToSelection(int lineToScrollTo = -1)
		{
			if (SelectedRow == null)
			{
				m_visibleRowIndexOffset = 0;
				return;
			}
<<<<<<< HEAD
			int num = lineToScrollTo;
			if (lineToScrollTo == -1)
			{
				num = SelectedRowIndex.Value;
			}
			if (m_scrollBar != null)
			{
				if (num > m_visibleRowIndexOffset + VisibleRowsCount - 1)
				{
					m_scrollBar.Value = num - VisibleRowsCount + 1;
				}
				if (num < m_visibleRowIndexOffset)
				{
					m_scrollBar.Value = num;
=======
			int value = SelectedRowIndex.Value;
			if (m_scrollBar != null)
			{
				if (value > m_visibleRowIndexOffset + VisibleRowsCount - 1)
				{
					m_scrollBar.Value = value - VisibleRowsCount + 1;
				}
				if (value < m_visibleRowIndexOffset)
				{
					m_scrollBar.Value = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			_ = RowHeight;
			_ = VisibleRowsCount;
			m_styleDef.Texture.Draw(positionAbsoluteTopLeft, base.Size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, backgroundTransitionAlpha));
			if (HeaderVisible)
			{
				DrawHeader(transitionAlpha);
			}
			if (m_drawSingleSelectRows)
			{
				DrawRows(transitionAlpha);
			}
			DrawGridLines(transitionAlpha);
			if (m_scrollBar != null)
			{
				m_scrollBar.Draw(MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
			}
			Vector2 positionAbsoluteTopRight = GetPositionAbsoluteTopRight();
			positionAbsoluteTopRight.X -= m_styleDef.ScrollbarMargin.HorizontalSum + ((m_scrollBar != null) ? m_scrollBar.Size.X : 0f);
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Controls\\scrollable_list_line.dds", positionAbsoluteTopRight, new Vector2(0.0012f, base.Size.Y), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
		}

		private void DrawGridLines(float alpha)
		{
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			if (ColumnLinesVisible)
			{
				Vector2 vector = positionAbsoluteTopLeft + m_headerArea.Position;
				for (int i = 0; i < m_columnsMetaData.Count - 1; i++)
				{
					ColumnMetaData columnMetaData = m_columnsMetaData[i];
					if (columnMetaData.Visible)
					{
						Vector2 normalizedSize = new Vector2(columnMetaData.VisibleWidth * m_rowsArea.Size.X, m_headerArea.Height);
						Vector2 screenCoordinateFromNormalizedCoordinate = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(vector + new Vector2(normalizedSize.X, 0f));
						MyGuiManager.DrawSpriteBatch(height: (int)(MyGuiManager.GetScreenSizeFromNormalizedSize(normalizedSize).Y * (float)(VisibleRowsCount + 1)), texture: "Textures\\GUI\\Blank.dds", x: (int)Math.Round(screenCoordinateFromNormalizedCoordinate.X), y: (int)Math.Round(screenCoordinateFromNormalizedCoordinate.Y), width: 1, color: new Color(0.5f, 0.5f, 0.5f, 0.2f * alpha));
						vector.X += columnMetaData.VisibleWidth * m_headerArea.Width;
					}
				}
			}
			if (RowLinesVisible)
			{
				Vector2 vector2 = positionAbsoluteTopLeft;
				vector2.Y += RowHeight;
				Vector2 vector3 = new Vector2(0f, RowHeight);
				Vector2 screenSizeFromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(m_rowsArea.Size);
				for (int j = 1; j < VisibleRowsCount; j++)
				{
					Vector2 screenCoordinateFromNormalizedCoordinate2 = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(vector2 + vector3);
					MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", (int)Math.Round(screenCoordinateFromNormalizedCoordinate2.X), (int)Math.Round(screenCoordinateFromNormalizedCoordinate2.Y), (int)screenSizeFromNormalizedSize.X, 1, new Color(0.5f, 0.5f, 0.5f, 0.2f * alpha));
					vector2.Y += RowHeight;
				}
			}
		}

		protected MyGuiControlBase HandleBaseInput()
		{
			return base.HandleInput();
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase captureInput = base.HandleInput();
			if (captureInput != null)
			{
				return captureInput;
			}
			if (!base.Enabled)
			{
				return null;
			}
			if (m_scrollBar != null && m_scrollBar.HandleInput())
			{
				captureInput = this;
			}
			HandleMouseOver();
			HandleNewMousePress(ref captureInput);
			using (List<MyGuiControlBase>.Enumerator enumerator = Controls.GetVisibleControls().GetEnumerator())
			{
				while (enumerator.MoveNext() && enumerator.Current.HandleInput() == null)
				{
				}
			}
			if (m_doubleClickStarted.HasValue && (float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) >= 500f)
			{
				m_doubleClickStarted = null;
			}
			if (!base.HasFocus)
			{
				return captureInput;
			}
			if (SelectedRowIndex.HasValue && SelectedRowIndex != -1 && this.ItemConfirmed != null && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01)))
			{
				captureInput = this;
				this.ItemConfirmed(this, new EventArgs
				{
					RowIndex = SelectedRowIndex.Value
				});
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
			{
				if (!m_gamepadSortIndex.HasValue)
				{
					m_gamepadSortIndex = 0;
				}
				int num = 0;
				bool flag = true;
				do
				{
					m_gamepadSortIndex++;
					num++;
					if (num == m_columnsMetaData.Count)
					{
						flag = false;
						break;
					}
					if (m_gamepadSortIndex >= m_columnsMetaData.Count)
					{
						m_gamepadSortIndex = 0;
					}
				}
				while (m_columnsMetaData[m_gamepadSortIndex.Value].AscendingComparison == null);
				if (flag)
				{
					SortByColumn(m_gamepadSortIndex.Value);
				}
			}
			return captureInput;
		}

		public override void Update()
		{
			base.Update();
			if (!base.IsMouseOver && !MyInput.Static.IsJoystickLastUsed)
			{
				m_mouseOverColumnIndex = null;
				m_mouseOverRowIndex = null;
				m_mouseOverHeader = false;
			}
		}

		protected override void OnOriginAlignChanged()
		{
			base.OnOriginAlignChanged();
			RefreshInternals();
		}

		protected override void OnPositionChanged()
		{
			base.OnPositionChanged();
			RefreshInternals();
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			if (m_scrollBar != null)
			{
				m_scrollBar.HasHighlight = base.HasHighlight;
			}
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshInternals();
		}

		public override void ShowToolTip()
		{
			MyToolTips toolTip = m_toolTip;
			if (m_mouseOverRowIndex.HasValue && m_rows.IsValidIndex(m_mouseOverRowIndex.Value) && m_mouseOverColumnIndex.HasValue)
			{
				Row row = m_rows[m_mouseOverRowIndex.Value];
				if (row.Cells.IsValidIndex(m_mouseOverColumnIndex.Value))
				{
					Cell cell = row.Cells[m_mouseOverColumnIndex.Value];
					if (cell.ToolTip != null)
					{
						m_toolTip = cell.ToolTip;
					}
				}
				if (this.ItemMouseOver != null)
				{
					this.ItemMouseOver(row);
				}
			}
			if (MyInput.Static.IsJoystickLastUsed && m_selectedRowIndex.HasValue && m_rows.IsValidIndex(m_selectedRowIndex.Value))
			{
				Row row2 = m_rows[m_selectedRowIndex.Value];
				if (this.ItemFocus != null)
				{
					this.ItemFocus(row2);
				}
				if (row2.ToolTip != null)
				{
					m_toolTip = row2.ToolTip;
				}
			}
			foreach (MyGuiControlBase visibleControl in Controls.GetVisibleControls())
			{
				visibleControl.ShowToolTip();
			}
			base.ShowToolTip();
			m_toolTip = toolTip;
		}

		private int ComputeColumnIndexFromPosition(Vector2 normalizedPosition)
		{
			normalizedPosition -= GetPositionAbsoluteTopLeft();
			float num = (normalizedPosition.X - m_rowsArea.Position.X) / m_rowsArea.Size.X;
			int i;
			for (i = 0; i < m_columnsMetaData.Count; i++)
			{
				if (m_columnsMetaData[i].Visible)
				{
					if (num < m_columnsMetaData[i].VisibleWidth)
					{
						break;
					}
					num -= m_columnsMetaData[i].VisibleWidth;
				}
			}
			return i;
		}

		protected int ComputeRowIndexFromPosition(Vector2 normalizedPosition)
		{
			normalizedPosition -= GetPositionAbsoluteTopLeft();
			return (int)((normalizedPosition.Y - m_rowsArea.Position.Y) / RowHeight) + m_visibleRowIndexOffset;
		}

		private void DebugDraw()
		{
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			MyGuiManager.DrawBorders(positionAbsoluteTopLeft + m_headerArea.Position, m_headerArea.Size, Color.Cyan, 1);
			MyGuiManager.DrawBorders(positionAbsoluteTopLeft + m_rowsArea.Position, m_rowsArea.Size, Color.White, 1);
			Vector2 topLeftPosition = positionAbsoluteTopLeft + m_headerArea.Position;
			for (int i = 0; i < m_columnsMetaData.Count; i++)
			{
				ColumnMetaData columnMetaData = m_columnsMetaData[i];
				if (columnMetaData.Visible)
				{
					MyGuiManager.DrawBorders(size: new Vector2(columnMetaData.VisibleWidth * m_rowsArea.Size.X, m_headerArea.Height), topLeftPosition: topLeftPosition, color: Color.Yellow, borderSize: 1);
					topLeftPosition.X += columnMetaData.VisibleWidth * m_headerArea.Width;
				}
			}
			if (m_scrollBar != null)
			{
				m_scrollBar.DebugDraw();
			}
		}

		private void DrawHeader(float transitionAlpha)
		{
			Vector2 vector = GetPositionAbsoluteTopLeft() + m_headerArea.Position;
			MyGuiManager.DrawSpriteBatch(m_styleDef.HeaderTextureHighlight, new Vector2(vector.X + 0.001f, vector.Y), new Vector2(m_headerArea.Size.X - 0.001f, m_headerArea.Size.Y), MyGuiControlBase.ApplyColorMaskModifiers(Color.White, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			for (int i = 0; i < m_columnsMetaData.Count; i++)
			{
				ColumnMetaData columnMetaData = m_columnsMetaData[i];
				if (columnMetaData.Visible)
				{
					string font = m_styleDef.HeaderFontNormal;
					if (m_mouseOverColumnIndex.HasValue && m_mouseOverColumnIndex.Value == i)
					{
						font = m_styleDef.HeaderFontHighlight;
					}
					Vector2 vector2 = new Vector2(columnMetaData.VisibleWidth * m_rowsArea.Size.X, m_headerArea.Height);
					Vector2 coordAlignedFromCenter = MyUtils.GetCoordAlignedFromCenter(vector + 0.5f * vector2 + new Vector2(columnMetaData.Margin.Left, 0f), vector2, columnMetaData.HeaderTextAlign);
					MyGuiManager.DrawString(font, columnMetaData.Name.ToString(), coordAlignedFromCenter, TextScaleWithLanguage, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), columnMetaData.HeaderTextAlign, useFullClientArea: false, vector2.X - columnMetaData.Margin.Left - columnMetaData.Margin.Right);
					vector.X += columnMetaData.VisibleWidth * m_headerArea.Width;
				}
			}
		}

		private void DrawRows(float transitionAlpha)
		{
			Vector2 vector = GetPositionAbsoluteTopLeft() + m_rowsArea.Position;
			Vector2 normalizedSizeFromScreenSize = MyGuiManager.GetNormalizedSizeFromScreenSize(new Vector2(1f, 1f));
			for (int i = 0; i < VisibleRowsCount; i++)
			{
				int num = i + m_visibleRowIndexOffset;
				if (num >= m_rows.Count)
				{
					break;
				}
				if (num < 0)
				{
					continue;
				}
				bool flag = m_mouseOverRowIndex.HasValue && m_mouseOverRowIndex.Value == num;
				bool flag2 = SelectedRowIndex.HasValue && SelectedRowIndex.Value == num;
				string text = m_styleDef.RowFontNormal;
				if (flag || flag2)
				{
					Vector2 normalizedCoord = vector;
					normalizedCoord.X += normalizedSizeFromScreenSize.X;
					MyGuiManager.DrawSpriteBatch(flag ? m_styleDef.RowTextureHighlight : (base.HasFocus ? m_styleDef.RowTextureFocus : m_styleDef.RowTextureActive), normalizedCoord, new Vector2(m_rowsArea.Size.X - normalizedSizeFromScreenSize.X, RowHeight), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					text = m_styleDef.RowFontHighlight;
				}
				Row row = m_rows[num];
				if (row != null)
				{
					Vector2 vector2 = vector;
					for (int j = 0; j < ColumnsCount && j < row.Cells.Count; j++)
					{
						Cell cell = row.Cells[j];
						ColumnMetaData columnMetaData = m_columnsMetaData[j];
						if (!columnMetaData.Visible)
						{
							continue;
						}
						Vector2 vector3 = new Vector2(columnMetaData.VisibleWidth * m_rowsArea.Size.X, RowHeight);
						if (cell != null && cell.Control != null)
						{
							MyUtils.GetCoordAlignedFromTopLeft(vector2, vector3, cell.IconOriginAlign);
							cell.Control.Position = vector2 + vector3 * 0.5f;
							cell.Control.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
							cell.Control.Draw(transitionAlpha, transitionAlpha);
						}
						else if (cell != null && cell.Text != null)
						{
							float num2 = 0f;
							float num3 = columnMetaData.Margin.Left + cell.Margin.Left;
							if (cell.Icon.HasValue)
							{
								Vector2 coordAlignedFromTopLeft = MyUtils.GetCoordAlignedFromTopLeft(vector2, vector3, cell.IconOriginAlign);
								MyGuiHighlightTexture value = cell.Icon.Value;
								Vector2 vector4 = Vector2.Min(value.SizeGui, vector3) / value.SizeGui;
								float num4 = Math.Min(vector4.X, vector4.Y);
								num2 = value.SizeGui.X;
								MyGuiManager.DrawSpriteBatch(base.HasHighlight ? value.Highlight : ((base.HasFocus && value.Focus != null) ? value.Focus : value.Normal), coordAlignedFromTopLeft + new Vector2(num3, 0f), value.SizeGui * num4, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), cell.IconOriginAlign);
								if (num4.IsValid())
								{
									num3 *= 2f;
								}
							}
							Vector2 vector5 = default(Vector2);
							if (columnMetaData.TextAlign == MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER || columnMetaData.TextAlign == MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP || columnMetaData.TextAlign == MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM)
							{
								Vector2 vector6 = MyGuiManager.MeasureString(text, cell.Text, TextScaleWithLanguage);
								vector5 = MyUtils.GetCoordAlignedFromCenter(vector2 + 0.5f * vector3 + new Vector2(num3, 0f) - new Vector2(vector6.X / 2f, 0f), vector3, columnMetaData.TextAlign);
							}
							else
							{
								vector5 = ((columnMetaData.TextAlign != MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER && columnMetaData.TextAlign != MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP && columnMetaData.TextAlign != MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM) ? MyUtils.GetCoordAlignedFromCenter(vector2 + 0.5f * vector3 + new Vector2(num3, 0f), vector3, columnMetaData.TextAlign) : MyUtils.GetCoordAlignedFromCenter(vector2 + 0.5f * vector3 - new Vector2(num3, 0f), vector3, columnMetaData.TextAlign));
							}
							Vector4 vector7 = Vector4.One;
							if (flag && m_styleDef.TextColorHighlight.HasValue)
							{
								vector7 = m_styleDef.TextColorHighlight.Value;
							}
							if (flag2)
							{
								if (base.HasFocus && m_styleDef.TextColorFocus.HasValue)
<<<<<<< HEAD
								{
									vector7 = m_styleDef.TextColorFocus.Value;
								}
								if (!base.HasFocus && m_styleDef.TextColorActive.HasValue)
								{
									vector7 = m_styleDef.TextColorActive.Value;
=======
								{
									vector7 = m_styleDef.TextColorFocus.Value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								}
								if (!base.HasFocus && m_styleDef.TextColorActive.HasValue)
								{
									vector7 = m_styleDef.TextColorActive.Value;
								}
							}
							float textScale = TextScaleWithLanguage;
							if (cell.IsAutoScaleEnabled)
							{
								DoEllipsisAndScaleAdjust(cell, text, ref textScale, vector3);
							}
<<<<<<< HEAD
							float textScale = TextScaleWithLanguage;
							float num5 = vector3.X - num3 - columnMetaData.Margin.Right - cell.Margin.Right;
							if (cell.IsAutoScaleEnabled)
							{
								DoEllipsisAndScaleAdjust(cell, text, ref textScale, new Vector2(num5, vector3.Y));
							}
							else
							{
								cell.TextScale = textScale;
							}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							vector5.X += num2;
							string font = text;
							string text2 = cell.Text.ToString();
							Vector2 normalizedCoord2 = vector5;
<<<<<<< HEAD
							float textScale2 = cell.TextScale;
							Color value2 = vector7;
							Color? obj = ((!cell.TextColor.HasValue) ? new Color?(MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha)) : (cell.TextColor * transitionAlpha));
							MyGuiManager.DrawString(font, text2, normalizedCoord2, textScale2, value2 * obj, columnMetaData.TextAlign, useFullClientArea: false, num5);
=======
							float scale = textScale;
							Color value2 = vector7;
							Color? obj = ((!cell.TextColor.HasValue) ? new Color?(MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha)) : (cell.TextColor * transitionAlpha));
							MyGuiManager.DrawString(font, text2, normalizedCoord2, scale, value2 * obj, columnMetaData.TextAlign, useFullClientArea: false, vector3.X - num3 - columnMetaData.Margin.Right - cell.Margin.Right);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						vector2.X += vector3.X;
					}
				}
				vector.Y += RowHeight;
			}
		}

<<<<<<< HEAD
		protected void DoEllipsisAndScaleAdjust(Cell cell, string Font, ref float textScale, Vector2 cellSize)
=======
		private void DoEllipsisAndScaleAdjust(Cell cell, string Font, ref float textScale, Vector2 cellSize)
		{
			if (!cell.IsAutoScaleEnabled || textScale == MyGuiManager.MIN_TEXT_SCALE)
			{
				return;
			}
			float num = MyGuiControlAutoScaleText.GetScale(Font, cell.Text, cellSize, textScale, MyGuiManager.MIN_TEXT_SCALE);
			if (textScale != num)
			{
				if (num < MyGuiManager.MIN_TEXT_SCALE)
				{
					num = MyGuiManager.MIN_TEXT_SCALE;
				}
				textScale = num;
				OnSizeChanged();
			}
		}

		private void HandleMouseOver()
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (textScale == MyGuiManager.MIN_TEXT_SCALE || cell.TextScale == MyGuiManager.MIN_TEXT_SCALE)
			{
				return;
			}
			float num = MyGuiControlAutoScaleText.GetScale(Font, cell.Text, cellSize, textScale, MyGuiManager.MIN_TEXT_SCALE);
			if (textScale != num)
			{
				if (num <= MyGuiManager.MIN_TEXT_SCALE)
				{
					num = MyGuiManager.MIN_TEXT_SCALE;
					GetTextWithEllipsis(cell.Text, Font, num, cellSize);
					base.IsTextWithEllipseAlready = false;
				}
				textScale = num;
				OnSizeChanged();
			}
			cell.TextScale = textScale;
		}

		protected void HandleMouseOver()
		{
			if (!MyInput.Static.IsJoystickLastUsed)
			{
				if (m_rowsArea.Contains(MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft()))
				{
					m_mouseOverRowIndex = ComputeRowIndexFromPosition(MyGuiManager.MouseCursorPosition);
					m_mouseOverColumnIndex = ComputeColumnIndexFromPosition(MyGuiManager.MouseCursorPosition);
					m_mouseOverHeader = false;
				}
				else if (m_headerArea.Contains(MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft()))
				{
					m_mouseOverRowIndex = null;
					m_mouseOverColumnIndex = ComputeColumnIndexFromPosition(MyGuiManager.MouseCursorPosition);
					m_mouseOverHeader = true;
				}
				else
				{
					m_mouseOverRowIndex = null;
					m_mouseOverColumnIndex = null;
					m_mouseOverHeader = false;
				}
			}
		}

		public MyGuiControlBase GetInnerControlsFromCurrentCell(int cellIndex)
		{
			if (!m_selectedRowIndex.HasValue || m_selectedRowIndex.Value < 0 || m_selectedRowIndex.Value > m_rows.Count)
			{
				return null;
			}
			Row row = m_rows[m_selectedRowIndex.Value];
			if (cellIndex < 0 || cellIndex >= row.Cells.Count)
			{
				return null;
			}
			return row.Cells[cellIndex].Control;
		}

		private void HandleNewMousePress(ref MyGuiControlBase captureInput)
		{
			bool flag = m_rowsArea.Contains(MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft());
			MyMouseButtonsEnum mouseButton = MyMouseButtonsEnum.None;
			if (MyInput.Static.IsNewPrimaryButtonPressed())
			{
				mouseButton = MyMouseButtonsEnum.Left;
			}
			else if (MyInput.Static.IsNewSecondaryButtonPressed())
			{
				mouseButton = MyMouseButtonsEnum.Right;
			}
			else if (MyInput.Static.IsNewMiddleMousePressed())
			{
				mouseButton = MyMouseButtonsEnum.Middle;
			}
			else if (MyInput.Static.IsNewXButton1MousePressed())
			{
				mouseButton = MyMouseButtonsEnum.XButton1;
			}
			else if (MyInput.Static.IsNewXButton2MousePressed())
			{
				mouseButton = MyMouseButtonsEnum.XButton2;
			}
			EventArgs arg;
			if (MyInput.Static.IsAnyNewMouseOrJoystickPressed() && flag)
			{
				SelectedRowIndex = ComputeRowIndexFromPosition(MyGuiManager.MouseCursorPosition);
				captureInput = this;
				if (this.ItemSelected != null)
				{
					Action<MyGuiControlTable, EventArgs> itemSelected = this.ItemSelected;
					arg = new EventArgs
					{
						RowIndex = SelectedRowIndex.Value,
						MouseButton = mouseButton
					};
					itemSelected.InvokeIfNotNull(this, arg);
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				}
			}
			if (!MyInput.Static.IsNewPrimaryButtonPressed())
			{
				return;
			}
			if (m_mouseOverHeader)
			{
				SortByColumn(m_mouseOverColumnIndex.Value);
				if (this.ColumnClicked != null)
				{
					this.ColumnClicked(this, m_mouseOverColumnIndex.Value);
				}
			}
			else
			{
				if (!flag)
				{
					return;
				}
				if (!m_doubleClickStarted.HasValue)
				{
					m_doubleClickStarted = MyGuiManager.TotalTimeInMilliseconds;
					m_doubleClickFirstPosition = MyGuiManager.MouseCursorPosition;
				}
				else if ((float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) <= 500f && (m_doubleClickFirstPosition - MyGuiManager.MouseCursorPosition).Length() <= 0.005f)
				{
					if (this.ItemDoubleClicked != null && SelectedRowIndex.HasValue)
					{
						Action<MyGuiControlTable, EventArgs> itemDoubleClicked = this.ItemDoubleClicked;
						arg = new EventArgs
						{
							RowIndex = SelectedRowIndex.Value,
							MouseButton = mouseButton
						};
						itemDoubleClicked(this, arg);
					}
					m_doubleClickStarted = null;
					captureInput = this;
				}
			}
		}

		public void Sort(bool switchSort = true)
		{
			if (m_sortColumn != -1)
			{
				SortByColumn(m_sortColumn, null, switchSort);
			}
		}

		public void SortByColumn(int columnIdx, SortStateEnum? sortState = null, bool switchSort = true)
		{
			if (MyInput.Static.IsJoystickLastUsed && m_gamepadSortIndex.HasValue && columnIdx != m_gamepadSortIndex)
			{
				columnIdx = m_gamepadSortIndex.Value;
			}
			columnIdx = MathHelper.Clamp(columnIdx, 0, m_columnsMetaData.Count - 1);
			m_sortColumn = columnIdx;
			m_sortColumnState = (sortState.HasValue ? new SortStateEnum?(sortState.Value) : m_sortColumnState);
			ColumnMetaData columnMetaData = m_columnsMetaData[columnIdx];
			SortStateEnum sortState2 = columnMetaData.SortState;
			m_columnsMetaData[m_lastSortedColumnIdx].SortState = SortStateEnum.Unsorted;
			Comparison<Cell> comparison = columnMetaData.AscendingComparison;
			if (comparison == null)
			{
				return;
			}
			SortStateEnum sortStateEnum = sortState2;
			if (switchSort)
			{
				sortStateEnum = ((sortState2 != SortStateEnum.Ascending) ? SortStateEnum.Ascending : SortStateEnum.Descending);
			}
			if (sortState.HasValue)
<<<<<<< HEAD
			{
				sortStateEnum = sortState.Value;
			}
			else if (m_sortColumnState.HasValue)
			{
				sortStateEnum = m_sortColumnState.Value;
			}
			Row row = null;
			if (IgnoreFirstRowForSort && m_rows.Count > 0)
			{
				row = m_rows[0];
				m_rows.RemoveAt(0);
			}
			List<Row> list = m_rows.Where((Row r) => !r.IsGlobalSortEnabled).ToList();
			foreach (Row item in list)
			{
				m_rows.Remove(item);
			}
			if (sortStateEnum == SortStateEnum.Ascending)
			{
=======
			{
				sortStateEnum = sortState.Value;
			}
			else if (m_sortColumnState.HasValue)
			{
				sortStateEnum = m_sortColumnState.Value;
			}
			Row row = null;
			if (IgnoreFirstRowForSort && m_rows.Count > 0)
			{
				row = m_rows[0];
				m_rows.RemoveAt(0);
			}
			List<Row> list = Enumerable.ToList<Row>(Enumerable.Where<Row>((IEnumerable<Row>)m_rows, (Func<Row, bool>)((Row r) => !r.IsGlobalSortEnabled)));
			foreach (Row item in list)
			{
				m_rows.Remove(item);
			}
			if (sortStateEnum == SortStateEnum.Ascending)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_rows.Sort((Row a, Row b) => Compare(columnIdx, comparison, a, b));
				list.Sort((Row a, Row b) => Compare(columnIdx, comparison, a, b));
			}
			else
			{
				m_rows.Sort((Row a, Row b) => Compare(columnIdx, comparison, b, a));
				list.Sort((Row a, Row b) => Compare(columnIdx, comparison, b, a));
			}
			if (row != null)
			{
				m_rows.Insert(0, row);
			}
			m_rows.InsertRange(0, list);
			m_lastSortedColumnIdx = columnIdx;
			columnMetaData.SortState = sortStateEnum;
			SelectedRowIndex = null;
			if (MyInput.Static.IsJoystickLastUsed && columnMetaData.Name != null && columnMetaData.Name.Length != 0)
			{
				string text = string.Empty;
				if (base.GamepadHelpTextId != MyStringId.NullOrEmpty)
				{
					text = MyTexts.GetString(base.GamepadHelpTextId);
				}
				base.GamepadHelpText = text + string.Format(MyTexts.GetString(MyCommonTexts.Gamepad_Help_TableSort), columnMetaData.Name);
				IMyGuiControlsOwner myGuiControlsOwner = this;
				while (myGuiControlsOwner.Owner != null)
				{
					myGuiControlsOwner = myGuiControlsOwner.Owner;
				}
				MyGuiScreenBase myGuiScreenBase;
				if (myGuiControlsOwner != null && (myGuiScreenBase = myGuiControlsOwner as MyGuiScreenBase) != null)
				{
					myGuiScreenBase.UpdateGamepadHelp(this);
				}
				if (!m_gamepadSortIndex.HasValue)
				{
					m_gamepadSortIndex = columnIdx;
				}
			}
		}

		public void UpdateTableSortHelpText()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				string text = string.Empty;
				if (base.GamepadHelpTextId != MyStringId.NullOrEmpty)
				{
					text = MyTexts.GetString(base.GamepadHelpTextId);
				}
				if (m_sortColumn != -1)
				{
					ColumnMetaData columnMetaData = m_columnsMetaData[m_sortColumn];
					base.GamepadHelpText = text + string.Format(MyTexts.GetString(MyCommonTexts.Gamepad_Help_TableSort), columnMetaData.Name);
				}
				else
				{
					base.GamepadHelpText = text + MyTexts.GetString(MyCommonTexts.Gamepad_Help_TableSort_Empty);
				}
			}
		}

		private static int Compare(int columnIdx, Comparison<Cell> comparison, Row a, Row b)
		{
<<<<<<< HEAD
			Cell x = ((a.Cells.Count > columnIdx) ? a.Cells[columnIdx] : null);
			Cell y = ((b.Cells.Count > columnIdx) ? b.Cells[columnIdx] : null);
			return comparison(x, y);
=======
			Cell cell = ((a.Cells.Count > columnIdx) ? a.Cells[columnIdx] : null);
			Cell y = ((b.Cells.Count > columnIdx) ? b.Cells[columnIdx] : null);
			return comparison(cell, y);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public int FindRow(Row row)
		{
			return m_rows.IndexOf(row);
		}

		protected bool IsValidRowIndex(int? index)
		{
			if (index.HasValue && 0 <= index.Value)
			{
				return index.Value < m_rows.Count;
			}
			return false;
		}

		private void RefreshInternals()
		{
			while (m_columnsMetaData.Count < ColumnsCount)
			{
				m_columnsMetaData.Add(new ColumnMetaData());
			}
			Vector2 minSizeGui = m_styleDef.Texture.MinSizeGui;
			Vector2 maxSizeGui = m_styleDef.Texture.MaxSizeGui;
			base.Size = Vector2.Clamp(new Vector2(base.Size.X, RowHeight * (float)(VisibleRowsCount + 1) + minSizeGui.Y), minSizeGui, maxSizeGui);
			m_headerArea.Position = new Vector2(m_styleDef.Padding.Left, m_styleDef.Padding.Top);
			m_headerArea.Size = new Vector2(base.Size.X - (m_styleDef.Padding.Left + m_styleDef.ScrollbarMargin.HorizontalSum + ((m_scrollBar != null) ? m_scrollBar.Size.X : 0f)), RowHeight);
			m_rowsArea.Position = m_headerArea.Position + (HeaderVisible ? new Vector2(0f, RowHeight) : Vector2.Zero);
			m_rowsArea.Size = new Vector2(m_headerArea.Size.X, RowHeight * (float)VisibleRowsCount);
			RefreshScrollbar();
		}

		private void RefreshScrollbar()
		{
			if (m_scrollBar != null)
			{
				m_scrollBar.Visible = m_rows.Count > VisibleRowsCount;
				m_scrollBar.Init(m_rows.Count, VisibleRowsCount);
				Vector2 vector = base.Size * new Vector2(0.5f, -0.5f);
				MyGuiBorderThickness scrollbarMargin = m_styleDef.ScrollbarMargin;
				Vector2 position = new Vector2(vector.X - (scrollbarMargin.Right + m_scrollBar.Size.X), vector.Y + scrollbarMargin.Top);
				m_scrollBar.Layout(position, base.Size.Y - (scrollbarMargin.Top + scrollbarMargin.Bottom));
				m_scrollBar.ChangeValue(0f);
			}
		}

		private void RefreshVisualStyle()
		{
			m_styleDef = GetVisualStyle(VisualStyle);
			RowHeight = m_styleDef.RowHeight;
			TextScale = m_styleDef.TextScale;
			RefreshInternals();
		}

		private void verticalScrollBar_ValueChanged(MyScrollbar scrollbar)
		{
			m_visibleRowIndexOffset = (int)scrollbar.Value;
		}

		private RectangleF GetRowRectangle(int? row)
		{
			if (row.HasValue)
			{
				return new RectangleF(GetPositionAbsoluteTopLeft() + new Vector2(0f, m_rowsArea.Position.Y + (float)(row.Value - m_visibleRowIndexOffset) * RowHeight), new Vector2(base.Size.X, RowHeight));
			}
			return base.Rectangle;
		}

		public override void OnFocusChanged(bool focus)
		{
			if (focus && !SelectedRowIndex.HasValue)
			{
				int num = MathHelper.Clamp((int)((m_entryPoint.Y - m_rowsArea.Y) / RowHeight), 0, VisibleRowsCount);
				num += m_visibleRowIndexOffset;
				if (num >= m_rows.Count)
				{
					num = m_rows.Count - 1;
				}
				if (SelectedRowIndex != num)
				{
					SelectedRowIndex = num;
					this.ItemSelected.InvokeIfNotNull(this, new EventArgs
					{
						RowIndex = SelectedRowIndex.Value,
						MouseButton = MyMouseButtonsEnum.Left
					});
					ScrollToSelection();
				}
			}
			base.OnFocusChanged(focus);
		}

		public override MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			if (currentFocusControl == this)
			{
				if (page)
				{
					return null;
				}
				int num = 0;
				if (SelectedRowIndex.HasValue)
				{
					num = SelectedRowIndex.Value;
				}
				switch (direction)
				{
				case MyDirection.Down:
					num++;
					break;
				case MyDirection.Up:
					num--;
					break;
				case MyDirection.Right:
					return null;
				case MyDirection.Left:
					return null;
				}
				if (num < 0 || num >= m_rows.Count)
				{
					return null;
				}
				if (SelectedRowIndex != num)
				{
					SelectedRowIndex = num;
					this.ItemSelected.InvokeIfNotNull(this, new EventArgs
					{
						RowIndex = SelectedRowIndex.Value,
						MouseButton = MyMouseButtonsEnum.Left
					});
					if (base.Owner == null)
					{
						return null;
					}
					ScrollToSelection();
				}
			}
			else
			{
				m_entryPoint = currentFocusControl.FocusRectangle.Center;
				m_entryDirection = direction;
			}
			return this;
		}
	}
}
