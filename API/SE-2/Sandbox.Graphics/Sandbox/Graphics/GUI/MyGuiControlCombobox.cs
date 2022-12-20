using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlCombobox))]
	public class MyGuiControlCombobox : MyGuiControlBase
	{
		public class StyleDefinition
		{
			public string ItemFontHighlight;

			public string ItemFontNormal;

			public string ItemTextureHighlight;

			public string ItemTextureFocus;

			public Vector4? TextColorHighlight;

			public Vector4? TextColorFocus;

			public Vector4? TextColor;

			public Vector4? ItemTextColorHighlight;

			public Vector4? ItemTextColorFocus;

			public Vector4? ItemTextColor;

<<<<<<< HEAD
			/// <summary>
			/// Offset of the text from left border.
			/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public Vector2 SelectedItemOffset;

			public MyGuiCompositeTexture DropDownTexture;

			public MyGuiCompositeTexture ComboboxTextureNormal;

			public MyGuiCompositeTexture ComboboxTextureHighlight;

			public MyGuiCompositeTexture ComboboxTextureFocus;

			public MyGuiCompositeTexture ComboboxTextureActive;

			public float TextScale;

			public float DropDownHighlightExtraWidth;

			public MyGuiBorderThickness ScrollbarMargin;
		}

		public class Item : IComparable
		{
			public readonly long Key;

			public readonly int SortOrder;

			public readonly StringBuilder Value;

			public MyToolTips ToolTip;

			public float TextScale { get; set; }

			public Item(long key, StringBuilder value, int sortOrder, string toolTip = null)
			{
				Key = key;
				SortOrder = sortOrder;
				if (value != null)
				{
					Value = new StringBuilder(value.Length).AppendStringBuilder(value);
				}
				else
				{
					Value = new StringBuilder();
				}
				if (toolTip != null)
				{
					ToolTip = new MyToolTips(toolTip);
				}
			}

			public Item(long key, string value, int sortOrder, string toolTip = null)
			{
				Key = key;
				SortOrder = sortOrder;
				if (value != null)
				{
					Value = new StringBuilder(value.Length).Append(value);
				}
				else
				{
					Value = new StringBuilder();
				}
				if (toolTip != null)
				{
					ToolTip = new MyToolTips(toolTip);
				}
			}

			public int CompareTo(object compareToObject)
			{
				Item item = (Item)compareToObject;
				int sortOrder = SortOrder;
				return sortOrder.CompareTo(item.SortOrder);
			}
		}

		public delegate void ItemSelectedDelegate();

		private const float ITEM_HEIGHT = 0.03f;

		private static readonly StyleDefinition[] m_styles;

		private bool m_isAutoScaleEnabled;

		private bool m_isAutoEllipsisEnabled;

		private bool m_IsAutoScaleEnabled;

		private bool m_IsAutoEllipsisEnabled;

		private bool m_isOpen;

		private bool m_scrollBarDragging;

		private List<Item> m_items;

		private Item m_selected;

		private Item m_preselectedMouseOver;

		private Item m_preselectedMouseOverPrevious;

		private int? m_preselectedKeyboardIndex;

		private int? m_preselectedKeyboardIndexPrevious;

		private int m_openAreaItemsCount;

		private int m_middleIndex;

		private bool m_showScrollBar;

		private float m_scrollBarCurrentPosition;

		private float m_scrollBarCurrentNonadjustedPosition;

		private float m_mouseOldPosition;

		private bool m_mousePositionReinit;

		private float m_maxScrollBarPosition;

		private float m_scrollBarEndPositionRelative;

		private int m_displayItemsStartIndex;

		private int m_displayItemsEndIndex;

		private int m_scrollBarItemOffSet;

		private float m_scrollBarHeight;

		private float m_scrollBarWidth;

		private float m_comboboxItemDeltaHeight;

		private float m_scrollRatio;

		private Vector2 m_dropDownItemSize;

		private const float ITEM_DRAW_DELTA = 0.0001f;

		private bool m_useScrollBarOffset;

		private MyGuiControlComboboxStyleEnum m_visualStyle;

		private StyleDefinition m_styleDef;

		private RectangleF m_selectedItemArea;

		private RectangleF m_openedArea;

		private RectangleF m_openedItemArea;

		private string m_selectedItemFont;

		private MyGuiCompositeTexture m_scrollbarTexture;

		private Vector4? m_textColorOverride;

		private Vector4 m_textColor;

		private float m_textScaleWithLanguage;

		private bool m_isFlipped;

		private MyKeyThrottler m_keyThrottler;

		public bool IsOpen
		{
			get
			{
				return m_isOpen;
			}
			set
			{
				if (m_isOpen != value)
				{
					m_isOpen = value;
					RefreshInternals();
				}
			}
		}

		public MyGuiControlComboboxStyleEnum VisualStyle
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

		public bool IsFlipped
		{
			get
			{
				return m_isFlipped;
			}
			set
			{
				if (m_isFlipped != value)
				{
					m_isFlipped = value;
					RefreshInternals();
				}
			}
		}

		public event ItemSelectedDelegate ItemSelected;

		static MyGuiControlCombobox()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlComboboxStyleEnum>() + 1];
			StyleDefinition[] styles = m_styles;
			StyleDefinition obj = new StyleDefinition
			{
				DropDownTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST_BORDER,
				ComboboxTextureNormal = MyGuiConstants.TEXTURE_COMBOBOX_NORMAL,
				ComboboxTextureHighlight = MyGuiConstants.TEXTURE_COMBOBOX_HIGHLIGHT,
				ComboboxTextureFocus = MyGuiConstants.TEXTURE_COMBOBOX_FOCUS,
				ComboboxTextureActive = MyGuiConstants.TEXTURE_COMBOBOX_ACTIVE,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemTextureFocus = "Textures\\GUI\\Controls\\item_focus_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				SelectedItemOffset = new Vector2(0.01f, 0.005f),
				TextScale = 0.719999969f,
				DropDownHighlightExtraWidth = 0.007f
			};
			MyGuiBorderThickness scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj.ScrollbarMargin = scrollbarMargin;
			obj.TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			obj.ItemTextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			styles[0] = obj;
			StyleDefinition[] styles2 = m_styles;
			StyleDefinition obj2 = new StyleDefinition
			{
				DropDownTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ComboboxTextureNormal = MyGuiConstants.TEXTURE_COMBOBOX_NORMAL,
				ComboboxTextureHighlight = MyGuiConstants.TEXTURE_COMBOBOX_HIGHLIGHT,
				ComboboxTextureFocus = MyGuiConstants.TEXTURE_COMBOBOX_FOCUS,
				ComboboxTextureActive = MyGuiConstants.TEXTURE_COMBOBOX_ACTIVE,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemTextureFocus = "Textures\\GUI\\Controls\\item_focus_dark.dds",
				ItemFontNormal = "Debug",
				ItemFontHighlight = "White",
				SelectedItemOffset = new Vector2(0.01f, 0.005f),
				TextScale = 0.719999969f,
				DropDownHighlightExtraWidth = 0.007f
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj2.ScrollbarMargin = scrollbarMargin;
			obj2.TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			obj2.ItemTextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			styles2[1] = obj2;
			StyleDefinition[] styles3 = m_styles;
			StyleDefinition obj3 = new StyleDefinition
			{
				DropDownTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ComboboxTextureNormal = MyGuiConstants.TEXTURE_COMBOBOX_NORMAL,
				ComboboxTextureHighlight = MyGuiConstants.TEXTURE_COMBOBOX_HIGHLIGHT,
				ComboboxTextureFocus = MyGuiConstants.TEXTURE_COMBOBOX_FOCUS,
				ComboboxTextureActive = MyGuiConstants.TEXTURE_COMBOBOX_ACTIVE,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemTextureFocus = "Textures\\GUI\\Controls\\item_focus_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				SelectedItemOffset = new Vector2(0.01f, 0.005f),
				TextScale = 0.719999969f,
				DropDownHighlightExtraWidth = 0.007f
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj3.ScrollbarMargin = scrollbarMargin;
			obj3.TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			obj3.ItemTextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			styles3[2] = obj3;
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlComboboxStyleEnum style)
		{
			return m_styles[(int)style];
		}

		private void RefreshVisualStyle()
		{
			m_styleDef = GetVisualStyle(VisualStyle);
			RefreshInternals();
		}

		private void RefreshInternals()
		{
			if (IsOpen)
			{
				BackgroundTexture = m_styleDef.ComboboxTextureActive ?? m_styleDef.ComboboxTextureHighlight;
				m_selectedItemFont = m_styleDef.ItemFontHighlight;
			}
			else if (base.HasHighlight)
			{
				BackgroundTexture = m_styleDef.ComboboxTextureHighlight;
				m_selectedItemFont = m_styleDef.ItemFontHighlight;
<<<<<<< HEAD
				m_textColor = m_styleDef.TextColorHighlight ?? Vector4.One;
=======
				m_textColor = (m_styleDef.TextColorHighlight.HasValue ? m_styleDef.TextColorHighlight.Value : Vector4.One);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else if (base.HasFocus)
			{
				BackgroundTexture = m_styleDef.ComboboxTextureFocus ?? m_styleDef.ComboboxTextureHighlight;
				m_selectedItemFont = m_styleDef.ItemFontHighlight;
<<<<<<< HEAD
				m_textColor = m_styleDef.TextColorFocus ?? Vector4.One;
=======
				m_textColor = (m_styleDef.TextColorFocus.HasValue ? m_styleDef.TextColorFocus.Value : Vector4.One);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				BackgroundTexture = m_styleDef.ComboboxTextureNormal;
				m_selectedItemFont = m_styleDef.ItemFontNormal;
<<<<<<< HEAD
				m_textColor = m_styleDef.TextColor ?? Vector4.One;
=======
				m_textColor = (m_styleDef.TextColor.HasValue ? m_styleDef.TextColor.Value : Vector4.One);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			base.MinSize = BackgroundTexture.MinSizeGui;
			base.MaxSize = BackgroundTexture.MaxSizeGui;
			m_scrollbarTexture = (base.HasHighlight ? MyGuiConstants.TEXTURE_SCROLLBAR_V_THUMB_HIGHLIGHT : MyGuiConstants.TEXTURE_SCROLLBAR_V_THUMB);
			m_selectedItemArea.Position = m_styleDef.SelectedItemOffset;
			m_selectedItemArea.Size = new Vector2(base.Size.X - (m_scrollbarTexture.MinSizeGui.X + m_styleDef.ScrollbarMargin.HorizontalSum + m_styleDef.SelectedItemOffset.X), 0.03f);
			MyRectangle2D openedArea = GetOpenedArea();
			m_openedArea.Position = openedArea.LeftTop;
			m_openedArea.Size = openedArea.Size;
			m_openedItemArea.Position = m_openedArea.Position + new Vector2(m_styleDef.SelectedItemOffset.X, m_styleDef.DropDownTexture.LeftTop.SizeGui.Y);
			m_openedItemArea.Size = new Vector2(m_selectedItemArea.Size.X, (float)(m_showScrollBar ? m_openAreaItemsCount : m_items.Count) * m_selectedItemArea.Size.Y);
			m_textScaleWithLanguage = m_styleDef.TextScale * MyGuiManager.LanguageTextScale;
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			RefreshInternals();
		}

		public override void OnFocusChanged(bool focus)
		{
			base.OnFocusChanged(focus);
			RefreshInternals();
		}

		protected override void OnPositionChanged()
		{
			base.OnPositionChanged();
			RefreshInternals();
		}

		protected override void OnOriginAlignChanged()
		{
			RefreshInternals();
			base.OnOriginAlignChanged();
		}

		public MyGuiControlCombobox()
			: this(null, null, null, null, 10, null, useScrollBarOffset: false, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, isAutoscaleEnabled: false, isAutoEllipsisEnabled: false)
		{
		}

		public MyGuiControlCombobox(Vector2? position = null, Vector2? size = null, Vector4? backgroundColor = null, Vector2? textOffset = null, int openAreaItemsCount = 10, Vector2? iconSize = null, bool useScrollBarOffset = false, string toolTip = null, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, Vector4? textColor = null, bool isAutoscaleEnabled = false, bool isAutoEllipsisEnabled = false)
			: base(position, size ?? (new Vector2(455f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE), backgroundColor, toolTip, null, isActiveControl: true, canHaveFocus: true, MyGuiControlHighlightType.WHEN_CURSOR_OVER, originAlign)
		{
			base.Name = "Combobox";
			HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
			m_items = new List<Item>();
			IsOpen = false;
			m_openAreaItemsCount = openAreaItemsCount;
			m_middleIndex = Math.Max(m_openAreaItemsCount / 2 - 1, 0);
<<<<<<< HEAD
			m_isAutoScaleEnabled = isAutoscaleEnabled;
			m_isAutoEllipsisEnabled = isAutoEllipsisEnabled;
=======
			m_IsAutoScaleEnabled = isAutoscaleEnabled;
			m_IsAutoEllipsisEnabled = isAutoEllipsisEnabled;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_textColorOverride = textColor;
			m_textColor = Vector4.One;
			m_dropDownItemSize = GetItemSize();
			m_comboboxItemDeltaHeight = m_dropDownItemSize.Y;
			m_mousePositionReinit = true;
			RefreshVisualStyle();
			InitializeScrollBarParameters();
			m_showToolTip = true;
			m_useScrollBarOffset = useScrollBarOffset;
			base.GamepadHelpTextId = MyCommonTexts.Gamepad_Help_Combobox;
			m_keyThrottler = new MyKeyThrottler();
		}

		public void ClearItems()
		{
			m_items.Clear();
			m_selected = null;
			m_preselectedKeyboardIndex = null;
			m_preselectedKeyboardIndexPrevious = null;
			m_preselectedMouseOver = null;
			m_preselectedMouseOverPrevious = null;
			InitializeScrollBarParameters();
		}

		public void AddItem(long key, MyStringId value, int? sortOrder = null, MyStringId? toolTip = null, bool sort = true)
		{
			AddItem(key, MyTexts.Get(value), sortOrder, toolTip.HasValue ? MyTexts.GetString(toolTip.Value) : null, sort);
		}

		public void AddItem(long key, StringBuilder value, int? sortOrder = null, string toolTip = null, bool sort = true)
		{
			sortOrder = sortOrder ?? m_items.Count;
			m_items.Add(new Item(key, value, sortOrder.Value, toolTip));
			if (sort)
			{
				m_items.Sort();
			}
			AdjustScrollBarParameters();
			RefreshInternals();
		}

		public void AddItem(long key, string value, int? sortOrder = null, string toolTip = null, bool sort = true)
		{
			sortOrder = sortOrder ?? m_items.Count;
			m_items.Add(new Item(key, value, sortOrder.Value, toolTip));
			if (sort)
			{
				m_items.Sort();
			}
			AdjustScrollBarParameters();
			RefreshInternals();
		}

		public void Sort()
		{
			if (m_items != null)
			{
				m_items.Sort();
			}
		}

		public void RemoveItem(long key)
		{
			Item item = m_items.Find((Item x) => x.Key == key);
			RemoveItem(item);
		}

		public void RemoveItemByIndex(int index)
		{
			if (index < 0 || index >= m_items.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			RemoveItem(m_items[index]);
		}

		public Item GetItemByIndex(int index)
		{
			if (index < 0 || index >= m_items.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return m_items[index];
		}

		private void RemoveItem(Item item)
		{
			if (item != null)
			{
				m_items.Remove(item);
				if (m_selected == item)
				{
					m_selected = null;
				}
			}
		}

		public Item TryGetItemByKey(long key)
		{
			foreach (Item item in m_items)
			{
				if (item.Key == key)
				{
					return item;
				}
			}
			return null;
		}

		public int GetItemsCount()
		{
			return m_items.Count;
		}

		public void SortItemsByValueText()
		{
			if (m_items != null)
			{
				m_items.Sort((Item item1, Item item2) => item1.Value.ToString().CompareTo(item2.Value.ToString()));
			}
		}

		public void CustomSortItems(Comparison<Item> comparison)
		{
			if (m_items != null)
			{
				m_items.Sort(comparison);
			}
		}

		public override MyGuiControlBase GetExclusiveInputHandler()
		{
			if (!IsOpen)
			{
				return null;
			}
			return this;
		}

		public void SelectItemByIndex(int index)
		{
			if (!m_items.IsValidIndex(index))
			{
				m_selected = null;
				return;
			}
			m_selected = m_items[index];
			SetScrollBarPositionByIndex(index);
			this.ItemSelected?.Invoke();
		}

		public void SelectItemByKey(long key, bool sendEvent = true)
		{
			for (int i = 0; i < m_items.Count; i++)
			{
				Item item = m_items[i];
<<<<<<< HEAD
				long key2 = item.Key;
				if (key2.Equals(key) && m_selected != item)
=======
				if (item.Key.Equals(key) && m_selected != item)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					m_selected = item;
					m_preselectedKeyboardIndex = i;
					SetScrollBarPositionByIndex(i);
					if (sendEvent && this.ItemSelected != null)
					{
						this.ItemSelected();
					}
					break;
				}
			}
		}

		public long GetSelectedKey()
		{
			if (m_selected == null)
			{
				return -1L;
			}
			return m_selected.Key;
		}

		public int GetSelectedIndex()
		{
			if (m_selected == null)
			{
				return -1;
			}
			return m_items.IndexOf(m_selected);
		}

		public StringBuilder GetSelectedValue()
		{
			if (m_selected == null)
			{
				return null;
			}
			return m_selected.Value;
		}

		private void Assert()
		{
		}

		private void SwitchComboboxMode()
		{
			if (m_scrollBarDragging)
			{
				return;
			}
			IsOpen = !IsOpen;
			if (!IsOpen)
			{
				return;
			}
			if (IsFlipped)
			{
				MyRectangle2D openedArea = GetOpenedArea();
				if (GetPositionAbsoluteTopRight().Y - openedArea.LeftTop.Y < 1f)
				{
					IsFlipped = false;
				}
			}
			else
			{
				MyRectangle2D openedArea2 = GetOpenedArea();
				if (GetPositionAbsoluteBottomRight().Y + openedArea2.Size.Y > 1f)
				{
					IsFlipped = true;
				}
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase == null && base.Enabled)
			{
				if (base.IsMouseOver && MyInput.Static.IsNewPrimaryButtonPressed() && !IsOpen && !m_scrollBarDragging)
				{
					m_showToolTip = true;
					return this;
				}
				if (MyInput.Static.IsNewPrimaryButtonReleased() && !m_scrollBarDragging && ((base.IsMouseOver && !IsOpen) || (IsMouseOverSelectedItem() && IsOpen)))
				{
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
					SwitchComboboxMode();
					myGuiControlBase = this;
				}
				if (base.HasFocus && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsNewKeyPressed(MyKeys.Space) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01)))
				{
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
					if (m_preselectedKeyboardIndex.HasValue && m_preselectedKeyboardIndex.Value < m_items.Count)
					{
						if (!IsOpen && m_selected != null)
						{
							SetScrollBarPositionByIndex(m_selected.Key);
						}
						else
						{
							SelectItemByKey(m_items[m_preselectedKeyboardIndex.Value].Key);
						}
					}
					SwitchComboboxMode();
					myGuiControlBase = this;
				}
				if (IsOpen)
				{
					if (m_showScrollBar && MyInput.Static.IsPrimaryButtonPressed())
					{
						Vector2 positionAbsoluteCenterLeft = GetPositionAbsoluteCenterLeft();
						MyRectangle2D openedArea = GetOpenedArea();
						openedArea.LeftTop += GetPositionAbsoluteTopLeft();
						float num = positionAbsoluteCenterLeft.X + base.Size.X - m_scrollBarWidth;
						float num2 = positionAbsoluteCenterLeft.X + base.Size.X;
						float num3;
						float num4;
						if (IsFlipped)
						{
							num3 = openedArea.LeftTop.Y - base.Size.Y / 2f;
							num4 = num3 + openedArea.Size.Y;
						}
						else
						{
							num3 = positionAbsoluteCenterLeft.Y + base.Size.Y / 2f;
							num4 = num3 + openedArea.Size.Y;
						}
						if (m_scrollBarDragging)
						{
							num = float.NegativeInfinity;
							num2 = float.PositiveInfinity;
							num3 = float.NegativeInfinity;
							num4 = float.PositiveInfinity;
						}
						if (MyGuiManager.MouseCursorPosition.X >= num && MyGuiManager.MouseCursorPosition.X <= num2 && MyGuiManager.MouseCursorPosition.Y >= num3 && MyGuiManager.MouseCursorPosition.Y <= num4)
						{
							float num5 = m_scrollBarCurrentPosition + openedArea.LeftTop.Y;
							if (MyGuiManager.MouseCursorPosition.Y > num5 && MyGuiManager.MouseCursorPosition.Y < num5 + m_scrollBarHeight)
							{
								if (m_mousePositionReinit)
								{
									m_mouseOldPosition = MyGuiManager.MouseCursorPosition.Y;
									m_mousePositionReinit = false;
								}
								float num6 = MyGuiManager.MouseCursorPosition.Y - m_mouseOldPosition;
								if (num6 > float.Epsilon || num6 < float.Epsilon)
								{
									SetScrollBarPosition(m_scrollBarCurrentNonadjustedPosition + num6);
								}
								m_mouseOldPosition = MyGuiManager.MouseCursorPosition.Y;
							}
							else
							{
								float value = MyGuiManager.MouseCursorPosition.Y - openedArea.LeftTop.Y - m_scrollBarHeight / 2f;
								SetScrollBarPosition(value);
							}
							m_scrollBarDragging = true;
						}
					}
					if (MyInput.Static.IsNewPrimaryButtonReleased())
					{
						m_mouseOldPosition = MyGuiManager.MouseCursorPosition.Y;
						m_mousePositionReinit = true;
					}
					if ((base.HasFocus && (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J02))) || (!IsMouseOverOnOpenedArea() && !base.IsMouseOver && MyInput.Static.IsNewLeftMousePressed()))
					{
						MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
						IsOpen = false;
					}
					myGuiControlBase = this;
					if (!m_scrollBarDragging)
					{
						m_preselectedMouseOverPrevious = m_preselectedMouseOver;
						m_preselectedMouseOver = null;
						int num7 = 0;
						int num8 = m_items.Count;
						float num9 = 0f;
						if (m_showScrollBar)
						{
							num7 = m_displayItemsStartIndex;
							num8 = m_displayItemsEndIndex;
							num9 = 0.025f;
						}
						for (int i = num7; i < num8; i++)
						{
							Vector2 openItemPosition = GetOpenItemPosition(i - m_displayItemsStartIndex);
							Vector2 vector = new Vector2(y: Math.Max(GetOpenedArea().LeftTop.Y, openItemPosition.Y), x: openItemPosition.X);
							Vector2 vector2 = vector + new Vector2(base.Size.X - num9, m_comboboxItemDeltaHeight);
							Vector2 vector3 = MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft();
							if (vector3.X >= vector.X && vector3.X <= vector2.X && vector3.Y >= vector.Y && vector3.Y <= vector2.Y)
							{
								m_preselectedMouseOver = m_items[i];
							}
						}
						if (m_preselectedMouseOver != null && m_preselectedMouseOver != m_preselectedMouseOverPrevious)
						{
							MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
						}
						m_showToolTip = m_preselectedMouseOver != null || IsMouseOverSelectedItem();
						if (MyInput.Static.IsNewPrimaryButtonReleased() && m_preselectedMouseOver != null)
						{
							SelectItemByKey(m_preselectedMouseOver.Key);
							MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
							IsOpen = false;
							myGuiControlBase = this;
						}
						if (base.HasFocus || IsMouseOverOnOpenedArea())
						{
							if (MyInput.Static.DeltaMouseScrollWheelValue() < 0)
							{
								HandleItemMovement(forwardMovement: true);
								myGuiControlBase = this;
							}
							else if (MyInput.Static.DeltaMouseScrollWheelValue() > 0)
							{
								HandleItemMovement(forwardMovement: false);
								myGuiControlBase = this;
							}
							if (m_keyThrottler.GetKeyStatus(MyKeys.Up) == ThrottledKeyStatus.PRESSED_AND_READY || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_UP, MyControlStateType.NEW_PRESSED_REPEATING))
							{
								HandleItemMovement(forwardMovement: false);
								myGuiControlBase = this;
							}
							else if (m_keyThrottler.GetKeyStatus(MyKeys.Down) == ThrottledKeyStatus.PRESSED_AND_READY || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_DOWN, MyControlStateType.NEW_PRESSED_REPEATING))
							{
								HandleItemMovement(forwardMovement: true);
								myGuiControlBase = this;
							}
							else if (MyInput.Static.IsNewKeyPressed(MyKeys.PageDown))
							{
								HandleItemMovement(forwardMovement: true, page: true);
							}
							else if (MyInput.Static.IsNewKeyPressed(MyKeys.PageUp))
							{
								HandleItemMovement(forwardMovement: false, page: true);
							}
							else if (MyInput.Static.IsNewKeyPressed(MyKeys.Home))
							{
								HandleItemMovement(forwardMovement: true, page: false, list: true);
							}
							else if (MyInput.Static.IsNewKeyPressed(MyKeys.End))
							{
								HandleItemMovement(forwardMovement: false, page: false, list: true);
							}
							else if (MyInput.Static.IsNewKeyPressed(MyKeys.Tab))
							{
								if (IsOpen)
								{
									SwitchComboboxMode();
								}
								myGuiControlBase = null;
							}
						}
					}
					else
					{
						if (MyInput.Static.IsNewPrimaryButtonReleased())
						{
							m_scrollBarDragging = false;
						}
						myGuiControlBase = this;
					}
				}
			}
			return myGuiControlBase;
		}

		private void HandleItemMovement(bool forwardMovement, bool page = false, bool list = false)
		{
			m_preselectedKeyboardIndexPrevious = m_preselectedKeyboardIndex;
			int num = 0;
			if (list && forwardMovement)
			{
				m_preselectedKeyboardIndex = 0;
			}
			else if (!list || forwardMovement)
			{
				num = ((page && forwardMovement) ? ((m_openAreaItemsCount <= m_items.Count) ? (m_openAreaItemsCount - 1) : (m_items.Count - 1)) : ((page && !forwardMovement) ? ((m_openAreaItemsCount <= m_items.Count) ? (-m_openAreaItemsCount + 1) : (-(m_items.Count - 1))) : ((!page && !list && forwardMovement) ? 1 : (-1))));
			}
			else
			{
				m_preselectedKeyboardIndex = m_items.Count - 1;
			}
			if (!m_preselectedKeyboardIndex.HasValue)
			{
				m_preselectedKeyboardIndex = ((!forwardMovement) ? (m_items.Count - 1) : 0);
			}
			else
			{
				m_preselectedKeyboardIndex += num;
				if (m_preselectedKeyboardIndex > m_items.Count - 1)
				{
					m_preselectedKeyboardIndex = m_items.Count - 1;
				}
				if (m_preselectedKeyboardIndex < 0)
				{
					m_preselectedKeyboardIndex = 0;
				}
			}
			if (m_preselectedKeyboardIndex != m_preselectedKeyboardIndexPrevious)
			{
				MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
			}
			if (m_preselectedKeyboardIndex.HasValue)
			{
				SetScrollBarPositionByIndex(m_preselectedKeyboardIndex.Value);
			}
		}

		private void SetScrollBarPositionByIndex(long index)
		{
			if (m_showScrollBar)
			{
				m_scrollRatio = 0f;
				if (m_preselectedKeyboardIndex >= m_displayItemsEndIndex)
				{
					m_displayItemsEndIndex = Math.Max(m_openAreaItemsCount, m_preselectedKeyboardIndex.Value + 1);
					m_displayItemsStartIndex = Math.Max(0, m_displayItemsEndIndex - m_openAreaItemsCount);
					SetScrollBarPosition((float)m_preselectedKeyboardIndex.Value * m_maxScrollBarPosition / (float)(m_items.Count - 1), calculateItemIndexes: false);
				}
				else if (m_preselectedKeyboardIndex < m_displayItemsStartIndex)
				{
					m_displayItemsStartIndex = Math.Max(0, m_preselectedKeyboardIndex.Value);
					m_displayItemsEndIndex = Math.Max(m_openAreaItemsCount, m_displayItemsStartIndex + m_openAreaItemsCount);
					SetScrollBarPosition((float)m_preselectedKeyboardIndex.Value * m_maxScrollBarPosition / (float)(m_items.Count - 1), calculateItemIndexes: false);
				}
				else if (m_preselectedKeyboardIndex.HasValue)
				{
					SetScrollBarPosition((float)m_preselectedKeyboardIndex.Value * m_maxScrollBarPosition / (float)(m_items.Count - 1), calculateItemIndexes: false);
				}
			}
		}

		private bool IsMouseOverOnOpenedArea()
		{
			MyRectangle2D openedArea = GetOpenedArea();
			openedArea.Size.Y += m_dropDownItemSize.Y;
			Vector2 leftTop = openedArea.LeftTop;
			Vector2 vector = openedArea.LeftTop + openedArea.Size;
			Vector2 vector2 = MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft();
			if (vector2.X >= leftTop.X && vector2.X <= vector.X && vector2.Y >= leftTop.Y)
			{
				return vector2.Y <= vector.Y;
			}
			return false;
		}

		private MyRectangle2D GetOpenedArea()
		{
			MyRectangle2D result = default(MyRectangle2D);
			if (IsFlipped)
			{
				if (m_showScrollBar)
				{
					result.LeftTop = new Vector2(0f, (float)(-m_openAreaItemsCount) * m_comboboxItemDeltaHeight);
				}
				else
				{
					result.LeftTop = new Vector2(0f, (float)(-m_items.Count) * m_comboboxItemDeltaHeight);
				}
				if (m_showScrollBar)
				{
					result.Size = new Vector2(m_dropDownItemSize.X, (float)m_openAreaItemsCount * m_comboboxItemDeltaHeight);
				}
				else
				{
					result.Size = new Vector2(m_dropDownItemSize.X, (float)m_items.Count * m_comboboxItemDeltaHeight);
				}
			}
			else
			{
				result.LeftTop = new Vector2(0f, base.Size.Y);
				if (m_showScrollBar)
				{
					result.Size = new Vector2(m_dropDownItemSize.X, (float)m_openAreaItemsCount * m_comboboxItemDeltaHeight);
				}
				else
				{
					result.Size = new Vector2(m_dropDownItemSize.X, (float)m_items.Count * m_comboboxItemDeltaHeight);
				}
			}
			return result;
		}

		private Vector2 GetOpenItemPosition(int index)
		{
			float num = m_dropDownItemSize.Y / 2f;
			num += m_comboboxItemDeltaHeight * 0.5f;
			if (IsFlipped)
			{
				return new Vector2(0f, -0.5f * base.Size.Y) + new Vector2(0f, num - (float)(Math.Min(m_openAreaItemsCount, m_items.Count) - index) * m_comboboxItemDeltaHeight);
			}
			return new Vector2(0f, 0.5f * base.Size.Y) + new Vector2(0f, num + (float)index * m_comboboxItemDeltaHeight);
		}

		/// <summary>
		/// two phase draw(two SpriteBatch phase):
		/// 1. combobox itself and selected item
		/// 2. opened area and display items draw(if opened area is displayed)
		///     a. setup up and draw stencil area to stencil buffer for clipping
		///     b. enable stencil
		///     c. draw the display items
		///     d. disable stencil
		/// </summary>
		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, transitionAlpha);
			if (m_selected != null)
			{
				DrawSelectedItemText(transitionAlpha);
			}
			float scrollbarInnerTexturePositionX = GetPositionAbsoluteCenterLeft().X + base.Size.X - m_scrollBarWidth / 2f;
			int startIndex = 0;
			int endIndex = m_items.Count;
			if (m_showScrollBar)
			{
				startIndex = m_displayItemsStartIndex;
				endIndex = m_displayItemsEndIndex;
			}
			if (IsOpen)
			{
				MyRectangle2D openedArea = GetOpenedArea();
				DrawOpenedAreaItems(startIndex, endIndex, transitionAlpha);
				if (m_showScrollBar)
				{
					DrawOpenedAreaScrollbar(scrollbarInnerTexturePositionX, openedArea, transitionAlpha);
				}
			}
		}

		private void DebugDraw()
		{
			BorderEnabled = true;
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			MyGuiManager.DrawBorders(positionAbsoluteTopLeft + m_selectedItemArea.Position, m_selectedItemArea.Size, Color.Cyan, 1);
			if (IsOpen)
			{
				MyGuiManager.DrawBorders(positionAbsoluteTopLeft + m_openedArea.Position, m_openedArea.Size, Color.GreenYellow, 1);
				MyGuiManager.DrawBorders(positionAbsoluteTopLeft + m_openedItemArea.Position, m_openedItemArea.Size, Color.Red, 1);
			}
		}

		private void DrawOpenedAreaScrollbar(float scrollbarInnerTexturePositionX, MyRectangle2D openedArea, float transitionAlpha)
		{
			MyGuiBorderThickness scrollbarMargin = m_styleDef.ScrollbarMargin;
			Vector2 positionTopLeft;
			if (IsFlipped)
			{
				positionTopLeft = GetPositionAbsoluteTopRight();
				positionTopLeft += new Vector2(0f - (scrollbarMargin.Right + m_scrollbarTexture.MinSizeGui.X), 0f - openedArea.Size.Y + scrollbarMargin.Top + m_scrollBarCurrentPosition);
			}
			else
			{
				positionTopLeft = GetPositionAbsoluteBottomRight();
				positionTopLeft += new Vector2(0f - (scrollbarMargin.Right + m_scrollbarTexture.MinSizeGui.X), scrollbarMargin.Top + m_scrollBarCurrentPosition);
			}
			m_scrollbarTexture.Draw(positionTopLeft, m_scrollBarHeight - m_scrollbarTexture.MinSizeGui.Y, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
		}

		private void DrawOpenedAreaItems(int startIndex, int endIndex, float transitionAlpha)
		{
			float num = (float)(endIndex - startIndex) * (m_comboboxItemDeltaHeight + 0.0001f);
			Vector2 minSizeGui = m_styleDef.DropDownTexture.MinSizeGui;
			Vector2 maxSizeGui = m_styleDef.DropDownTexture.MaxSizeGui;
			Vector2 size = Vector2.Clamp(new Vector2(base.Size.X, num + minSizeGui.Y), minSizeGui, maxSizeGui);
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			m_styleDef.DropDownTexture.Draw(positionAbsoluteTopLeft + m_openedArea.Position, size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
			RectangleF normalizedRectangle = m_openedItemArea;
			normalizedRectangle.Position += positionAbsoluteTopLeft;
			normalizedRectangle.Position.X -= m_styleDef.DropDownHighlightExtraWidth;
			normalizedRectangle.Size.X += m_styleDef.DropDownHighlightExtraWidth;
			using (MyGuiManager.UsingScissorRectangle(ref normalizedRectangle))
			{
				Vector2 vector = positionAbsoluteTopLeft + m_openedItemArea.Position;
				for (int i = startIndex; i < endIndex && i < m_items.Count; i++)
				{
					Item item = m_items[i];
					string font = m_styleDef.ItemFontNormal;
					bool num2 = item == m_preselectedMouseOver;
					bool flag = m_preselectedKeyboardIndex.HasValue && m_preselectedKeyboardIndex == i;
					if (num2 || flag)
					{
						MyGuiManager.DrawSpriteBatchRoundUp((flag && m_styleDef.ItemTextureFocus != null) ? m_styleDef.ItemTextureFocus : m_styleDef.ItemTextureHighlight, vector - new Vector2(m_styleDef.DropDownHighlightExtraWidth, 0f), m_selectedItemArea.Size + new Vector2(m_styleDef.DropDownHighlightExtraWidth, 0f), MyGuiControlBase.ApplyColorMaskModifiers(Vector4.One, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
						font = m_styleDef.ItemFontHighlight;
					}
					Vector4 vector2 = Vector4.One;
					if (num2 && m_styleDef.ItemTextColorHighlight.HasValue)
					{
						vector2 = m_styleDef.ItemTextColorHighlight.Value;
					}
					else if (flag && m_styleDef.ItemTextColorFocus.HasValue)
					{
						vector2 = m_styleDef.ItemTextColorFocus.Value;
					}
					else if (m_styleDef.ItemTextColor.HasValue)
					{
						vector2 = m_styleDef.ItemTextColor.Value;
					}
<<<<<<< HEAD
					float textScale = m_textScaleWithLanguage;
					if (m_isAutoScaleEnabled)
					{
						DoEllipsisAndScaleAdjust(item, font, ref textScale);
					}
					else
					{
						item.TextScale = textScale;
					}
					MyGuiManager.DrawString(font, item.Value.ToString(), new Vector2(vector.X, vector.Y + m_styleDef.DropDownHighlightExtraWidth / 2f), item.TextScale, MyGuiControlBase.ApplyColorMaskModifiers(m_textColorOverride ?? vector2, base.Enabled, transitionAlpha));
=======
					float TextScale = m_textScaleWithLanguage;
					StringBuilder stringBuilder = DoEllipsisAndScaleAdjust(ref TextScale, font, item.Value);
					if (stringBuilder != null)
					{
						if (item.ToolTip == null)
						{
							item.ToolTip = new MyToolTips();
						}
						if (item.ToolTip.ToolTips == null || (item.ToolTip.ToolTips != null && ((Collection<MyColoredText>)(object)item.ToolTip.ToolTips).Count == 0))
						{
							item.ToolTip.AddToolTip(stringBuilder.ToString());
						}
					}
					MyGuiManager.DrawString(font, item.Value.ToString(), new Vector2(vector.X, vector.Y + m_styleDef.DropDownHighlightExtraWidth / 2f), TextScale, MyGuiControlBase.ApplyColorMaskModifiers(m_textColorOverride ?? vector2, base.Enabled, transitionAlpha));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					vector.Y += 0.03f;
				}
			}
		}

<<<<<<< HEAD
		private void DoEllipsisAndScaleAdjust(Item item, string font, ref float textScale)
		{
			if (!m_isAutoScaleEnabled || textScale == MyGuiManager.MIN_TEXT_SCALE)
			{
				return;
			}
			Vector2 maxSize = new Vector2(m_openedItemArea.Size.X - (m_scrollbarTexture.MinSizeGui.X + m_styleDef.ScrollbarMargin.HorizontalSum), 0.03f);
			float num = MyGuiControlAutoScaleText.GetScale(font, item.Value, maxSize, textScale, MyGuiManager.MIN_TEXT_SCALE);
			if (textScale != num)
			{
				if (num <= MyGuiManager.MIN_TEXT_SCALE)
				{
					num = MyGuiManager.MIN_TEXT_SCALE;
					if (m_isAutoEllipsisEnabled)
					{
						GetTextWithEllipsis(item.Value, font, textScale, maxSize);
					}
				}
				textScale = num;
				RefreshInternals();
			}
			item.TextScale = textScale;
		}

		public StringBuilder GetTextWithEllipsis(StringBuilder textToDraw, string font, float textScale, Vector2 maxSize)
		{
			if (!textToDraw.ToString().EndsWith("…") && textToDraw != null && maxSize.X != float.PositiveInfinity)
			{
				float scale = MyGuiControlAutoScaleText.GetScale(font, textToDraw, maxSize / MyGuiManager.GetSafeScreenScale(), textScale, 0f);
				if (scale >= textScale)
				{
					return null;
				}
				while (scale < textScale)
				{
					if (textToDraw.Length > 3)
					{
						textToDraw.TrimEnd(2);
						textToDraw.Append("…");
						scale = MyGuiControlAutoScaleText.GetScale(font, textToDraw, maxSize / MyGuiManager.GetSafeScreenScale(), textScale, 0f);
						continue;
					}
					return textToDraw;
				}
				return textToDraw;
			}
=======
		private StringBuilder DoEllipsisAndScaleAdjust(ref float TextScale, string font, StringBuilder TextForDraw)
		{
			Vector2 size = base.Size;
			size.X -= 0.04f;
			if (m_IsAutoScaleEnabled && TextScale != MyGuiManager.MIN_TEXT_SCALE)
			{
				Vector2 size2 = base.Size;
				size2.X -= m_scrollBarWidth;
				float num = MyGuiControlAutoScaleText.GetScale(font, TextForDraw, size / MyGuiManager.GetSafeScreenScale(), TextScale, MyGuiManager.MIN_TEXT_SCALE);
				if (TextScale != num)
				{
					if (num < MyGuiManager.MIN_TEXT_SCALE)
					{
						num = MyGuiManager.MIN_TEXT_SCALE;
					}
					TextScale = num;
					RefreshInternals();
				}
			}
			if (m_IsAutoEllipsisEnabled)
			{
				return GetTextWithEllipsis(TextForDraw, font, TextScale, size);
			}
			return TextForDraw;
		}

		public StringBuilder GetTextWithEllipsis(StringBuilder textToDraw, string font, float textScale, Vector2 maxSize)
		{
			if (!textToDraw.ToString().EndsWith("…") && textToDraw != null && maxSize.X != float.PositiveInfinity)
			{
				float scale = MyGuiControlAutoScaleText.GetScale(font, textToDraw, maxSize / MyGuiManager.GetSafeScreenScale(), textScale, 0f);
				if (scale >= textScale)
				{
					return null;
				}
				while (scale < textScale)
				{
					if (textToDraw.Length > 3)
					{
						textToDraw.TrimEnd(2);
						textToDraw.Append("…");
						scale = MyGuiControlAutoScaleText.GetScale(font, textToDraw, maxSize / MyGuiManager.GetSafeScreenScale(), textScale, 0f);
						continue;
					}
					return textToDraw;
				}
				return textToDraw;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return null;
		}

		private void DrawSelectedItemText(float transitionAlpha)
		{
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			RectangleF normalizedRectangle = m_selectedItemArea;
			normalizedRectangle.Position += positionAbsoluteTopLeft;
			using (MyGuiManager.UsingScissorRectangle(ref normalizedRectangle))
			{
				Vector2 normalizedCoord = positionAbsoluteTopLeft + m_selectedItemArea.Position + new Vector2(0f, m_selectedItemArea.Size.Y * 0.5f);
				MyGuiManager.DrawString(m_selectedItemFont, m_selected.Value.ToString(), normalizedCoord, m_textScaleWithLanguage, MyGuiControlBase.ApplyColorMaskModifiers(m_textColorOverride ?? m_textColor, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			}
		}

		private void InitializeScrollBarParameters()
		{
			m_showScrollBar = false;
			Vector2 cOMBOBOX_VSCROLLBAR_SIZE = MyGuiConstants.COMBOBOX_VSCROLLBAR_SIZE;
			m_scrollBarWidth = cOMBOBOX_VSCROLLBAR_SIZE.X;
			m_scrollBarHeight = cOMBOBOX_VSCROLLBAR_SIZE.Y;
			m_scrollBarCurrentPosition = 0f;
			m_scrollBarEndPositionRelative = (float)m_openAreaItemsCount * m_comboboxItemDeltaHeight + m_styleDef.DropDownTexture.LeftBottom.SizeGui.Y;
			m_displayItemsStartIndex = 0;
			m_displayItemsEndIndex = m_openAreaItemsCount;
		}

		private void AdjustScrollBarParameters()
		{
			m_showScrollBar = m_items.Count > m_openAreaItemsCount;
			if (m_showScrollBar)
			{
				m_maxScrollBarPosition = m_scrollBarEndPositionRelative - m_scrollBarHeight;
				m_scrollBarItemOffSet = m_items.Count - m_openAreaItemsCount;
			}
		}

		private void CalculateStartAndEndDisplayItemsIndex()
		{
			m_scrollRatio = ((m_scrollBarCurrentPosition == 0f) ? 0f : (m_scrollBarCurrentPosition * (float)m_scrollBarItemOffSet / m_maxScrollBarPosition));
			m_displayItemsStartIndex = Math.Max(0, (int)Math.Floor((double)m_scrollRatio + 0.5));
			m_displayItemsEndIndex = Math.Min(m_items.Count, m_displayItemsStartIndex + m_openAreaItemsCount);
		}

		public void ScrollToPreSelectedItem()
		{
			if (m_preselectedKeyboardIndex.HasValue)
			{
				m_displayItemsStartIndex = ((m_preselectedKeyboardIndex.Value > m_middleIndex) ? (m_preselectedKeyboardIndex.Value - m_middleIndex) : 0);
				m_displayItemsEndIndex = m_displayItemsStartIndex + m_openAreaItemsCount;
				if (m_displayItemsEndIndex > m_items.Count)
				{
					m_displayItemsEndIndex = m_items.Count;
					m_displayItemsStartIndex = m_displayItemsEndIndex - m_openAreaItemsCount;
				}
				SetScrollBarPosition((float)m_displayItemsStartIndex * m_maxScrollBarPosition / (float)m_scrollBarItemOffSet);
			}
		}

		private void SetScrollBarPosition(float value, bool calculateItemIndexes = true)
		{
			value = MathHelper.Clamp(value, 0f, m_maxScrollBarPosition);
			if (m_scrollBarCurrentPosition != value)
			{
				m_scrollBarCurrentNonadjustedPosition = value;
				m_scrollBarCurrentPosition = value;
				if (calculateItemIndexes)
				{
					CalculateStartAndEndDisplayItemsIndex();
				}
			}
		}

		protected Vector2 GetItemSize()
		{
			return MyGuiConstants.COMBOBOX_MEDIUM_ELEMENT_SIZE;
		}

		public override bool CheckMouseOver(bool use_IsMouseOverAll = true)
		{
			if (IsOpen)
			{
				int num = (m_showScrollBar ? m_openAreaItemsCount : m_items.Count);
				for (int i = 0; i < num; i++)
				{
					Vector2 openItemPosition = GetOpenItemPosition(i);
					Vector2 vector = new Vector2(y: Math.Max(GetOpenedArea().LeftTop.Y, openItemPosition.Y), x: openItemPosition.X);
					Vector2 vector2 = vector + new Vector2(base.Size.X, m_comboboxItemDeltaHeight);
					Vector2 vector3 = MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft();
					if (vector3.X >= vector.X && vector3.X <= vector2.X && vector3.Y >= vector.Y && vector3.Y <= vector2.Y)
					{
						return true;
					}
				}
			}
			if (m_scrollBarDragging)
			{
				return false;
			}
			return MyGuiControlBase.CheckMouseOver(base.Size, GetPositionAbsolute(), base.OriginAlign);
		}

		private void SnapCursorToControl(int controlIndex)
		{
			Vector2 openItemPosition = GetOpenItemPosition(controlIndex);
			Vector2 openItemPosition2 = GetOpenItemPosition(m_displayItemsStartIndex);
			Vector2 vector = openItemPosition - openItemPosition2;
			MyRectangle2D openedArea = GetOpenedArea();
			Vector2 positionAbsoluteCenter = GetPositionAbsoluteCenter();
			positionAbsoluteCenter.Y += openedArea.LeftTop.Y;
			positionAbsoluteCenter.Y += vector.Y;
			Vector2 screenCoordinateFromNormalizedCoordinate = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(positionAbsoluteCenter);
			m_preselectedMouseOver = m_items[controlIndex];
			MyInput.Static.SetMousePosition((int)screenCoordinateFromNormalizedCoordinate.X, (int)screenCoordinateFromNormalizedCoordinate.Y);
		}

		private bool IsMouseOverSelectedItem()
		{
			Vector2 vector = GetPositionAbsoluteCenterLeft() - new Vector2(0f, base.Size.Y / 2f);
			Vector2 vector2 = vector + base.Size;
			if (MyGuiManager.MouseCursorPosition.X >= vector.X && MyGuiManager.MouseCursorPosition.X <= vector2.X && MyGuiManager.MouseCursorPosition.Y >= vector.Y)
			{
				return MyGuiManager.MouseCursorPosition.Y <= vector2.Y;
			}
			return false;
		}

		public override void ShowToolTip()
		{
			MyToolTips toolTip = m_toolTip;
			if (IsOpen && IsMouseOverOnOpenedArea() && m_preselectedMouseOver != null && m_preselectedMouseOver.ToolTip != null)
			{
				m_toolTip = m_preselectedMouseOver.ToolTip;
			}
			if (IsOpen && MyInput.Static.IsJoystickLastUsed && m_preselectedKeyboardIndex.HasValue && m_preselectedKeyboardIndex.Value >= 0 && m_preselectedKeyboardIndex.Value < m_items.Count)
			{
				m_toolTip = m_items[m_preselectedKeyboardIndex.Value].ToolTip;
				m_showToolTip = true;
			}
			if ((IsMouseOverSelectedItem() || (MyInput.Static.IsJoystickLastUsed && !IsOpen)) && m_selected != null && m_selected.ToolTip != null)
			{
				m_toolTip = m_selected.ToolTip;
			}
			base.ShowToolTip();
			m_toolTip = toolTip;
		}

		protected override bool CheckMouseOverInternal()
		{
			if (IsOpen && IsMouseOverOnOpenedArea() && m_preselectedMouseOver != null)
			{
				return m_preselectedMouseOver.ToolTip != null;
			}
			return false;
		}

		protected override Vector2 GetToolTipPosition()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				RectangleF focusRectangle = FocusRectangle;
				if (IsOpen && m_preselectedKeyboardIndex.HasValue)
				{
					return focusRectangle.Position + m_selectedItemArea.Position + m_selectedItemArea.Size / 2f + new Vector2(0f, (float)(m_preselectedKeyboardIndex.Value + 1) * 0.03f);
				}
				return focusRectangle.Position + focusRectangle.Size / 2f;
			}
			return MyGuiManager.MouseCursorPosition;
		}

		public void ApplyStyle(StyleDefinition style)
		{
			if (style != null)
			{
				m_styleDef = style;
				RefreshInternals();
			}
		}
	}
}
