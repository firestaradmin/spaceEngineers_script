using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlListbox))]
	public class MyGuiControlListbox : MyGuiControlAutoScaleText
	{
		public class StyleDefinition
		{
			public string ItemFontHighlight;

			public string ItemFontNormal;

			public Vector2 ItemSize;

			public string ItemTextureHighlight;

			public string ItemTextureFocus;

			public string ItemTextureActive;

			public Vector2 ItemsOffset;

			/// <summary>
			/// Offset of the text from left border.
			/// </summary>
			public float TextOffset;

			public bool DrawScroll;

			public bool PriorityCaptureInput;

			public bool XSizeVariable;

			public float TextScale;

			public MyGuiCompositeTexture Texture;

			public MyGuiBorderThickness ScrollbarMargin;

			public Vector4? TextColorHighlight;

			public Vector4? TextColorFocus;

			public StyleDefinition CloneShallow()
			{
				return new StyleDefinition
				{
					ItemFontHighlight = ItemFontHighlight,
					ItemFontNormal = ItemFontNormal,
					ItemSize = new Vector2(ItemSize.X, ItemSize.Y),
					ItemTextureHighlight = ItemTextureHighlight,
					ItemsOffset = new Vector2(ItemsOffset.X, ItemsOffset.Y),
					TextOffset = TextOffset,
					DrawScroll = DrawScroll,
					PriorityCaptureInput = PriorityCaptureInput,
					XSizeVariable = XSizeVariable,
					TextScale = TextScale,
					Texture = Texture,
					ScrollbarMargin = ScrollbarMargin
				};
			}
		}

		public class Item
		{
			private bool m_visible;

			public readonly string Icon;

			public readonly MyToolTips ToolTip;

			public readonly object UserData;

			public string FontOverride;

			public Vector4? ColorMask;

<<<<<<< HEAD
			public float TextScale { get; set; }

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public StringBuilder Text { get; set; }

			public bool Visible
			{
				get
				{
					return m_visible;
				}
				set
				{
					if (m_visible != value)
					{
						m_visible = value;
						if (this.OnVisibleChanged != null)
						{
							this.OnVisibleChanged();
						}
					}
				}
			}

			public event Action OnVisibleChanged;

			/// <summary>
			/// Do not construct directly. Use AddItem on listbox for that.
			/// </summary>
			public Item(StringBuilder text = null, string toolTip = null, string icon = null, object userData = null, string fontOverride = null)
			{
				Text = new StringBuilder((text != null) ? text.ToString() : "");
				ToolTip = ((toolTip != null) ? new MyToolTips(toolTip) : null);
				Icon = icon;
				UserData = userData;
				FontOverride = fontOverride;
				Visible = true;
			}

			public Item(ref StringBuilder text, string toolTip = null, string icon = null, object userData = null, string fontOverride = null)
			{
				Text = text;
				ToolTip = ((toolTip != null) ? new MyToolTips(toolTip) : null);
				Icon = icon;
				UserData = userData;
				FontOverride = fontOverride;
				Visible = true;
			}
		}

		public bool SimpleFocusMode;

		public bool UseSimpleItemListMouseOverCheck;

		private static StyleDefinition[] m_styles;

		private Vector2 m_doubleClickFirstPosition;

		private int? m_doubleClickStarted;

		private RectangleF m_itemsRectangle;

		private Item m_mouseOverItem;

		private StyleDefinition m_styleDef;

		private StyleDefinition m_customStyle;

		private bool m_useCustomStyle;

		private MyVScrollbar m_scrollBar;

		private int m_visibleRowIndexOffset;

		private int m_visibleRows;

<<<<<<< HEAD
		private float m_innerMarginBottom;

		private int m_focusedItemIndex = -1;

=======
		private int m_focusedItemIndex = -1;

		private bool m_isAutoscaleEnabled;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public readonly ObservableCollection<Item> Items;

		public List<Item> SelectedItems = new List<Item>();

		public Item m_pivotItem;

		private Item m_focusedItem;

		public Item LastSelectedItem;

		public bool SelectItemOnItemFocusChangeByKeyboard;

		private MyGuiControlListboxStyleEnum m_visualStyle;

		public bool MultiSelect;

		private bool m_isInBulkInsert;

		private List<Item> m_StoredSelectedItems = new List<Item>();

		private int m_StoredTopmostSelectedPosition;

		private Item m_StoredTopmostSelectedItem;

		private Item m_StoredMouseOverItem;

		private int m_StoredMouseOverPosition;

		private Item m_StoredItemOnTop;

		private float m_StoredScrollbarValue;

		private Vector2 m_entryPoint;

		private MyDirection m_entryDirection;

<<<<<<< HEAD
=======
		public bool IsAutoscaleEnabled
		{
			get
			{
				return m_isAutoscaleEnabled;
			}
			set
			{
				m_isAutoscaleEnabled = value;
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public Item PivotItem
		{
			get
			{
				return m_pivotItem;
			}
			set
			{
				if (m_pivotItem != value)
				{
					m_pivotItem = value;
				}
			}
		}

		public Item FocusedItem
		{
			get
			{
				return m_focusedItem;
			}
			set
			{
				if (m_focusedItem != value)
				{
					m_focusedItem = value;
					if (SelectItemOnItemFocusChangeByKeyboard && (MyInput.Static.IsKeyPress(MyKeys.Down) || MyInput.Static.IsKeyPress(MyKeys.Up)))
					{
						SelectFocusedItem(selectFocusedOnly: true);
					}
				}
			}
		}

		public Item MouseOverItem => m_mouseOverItem;

		public Vector2 ItemSize { get; set; }

		public float TextScale { get; set; }

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

		public float InnerMarginBottom
		{
			get
			{
				return m_innerMarginBottom;
			}
			set
			{
				m_innerMarginBottom = value;
				RefreshInternals();
			}
		}

		public int FirstVisibleRow
		{
			get
			{
				return m_visibleRowIndexOffset;
			}
			set
			{
				m_scrollBar.Value = value;
			}
		}

		public MyGuiControlListboxStyleEnum VisualStyle
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

		public bool IsInBulkInsert
		{
			get
			{
				return m_isInBulkInsert;
			}
			set
			{
				if (value != m_isInBulkInsert)
				{
					m_isInBulkInsert = value;
					if (!m_isInBulkInsert)
					{
						RefreshScrollBar();
					}
				}
			}
		}

		public override RectangleF FocusRectangle => GetRowRectangle(SelectedItems);

		public event Action<MyGuiControlListbox> ItemClicked;

		public event Action<MyGuiControlListbox> ItemDoubleClicked;

		public event Action<MyGuiControlListbox> ItemsSelected;

		public event Action<MyGuiControlListbox> ItemMouseOver;

		static MyGuiControlListbox()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlListboxStyleEnum>() + 1];
			SetupStyles();
		}

		private static void SetupStyles()
		{
			StyleDefinition[] styles = m_styles;
			StyleDefinition obj = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemTextureFocus = "Textures\\GUI\\Controls\\item_focus_dark.dds",
				ItemTextureActive = "Textures\\GUI\\Controls\\item_active_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.25f, 0.034f),
				TextScale = 0.8f,
				TextOffset = 0.006f,
				ItemsOffset = new Vector2(6f, 2f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = false,
				XSizeVariable = false
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
			styles[0] = obj;
			StyleDefinition[] styles2 = m_styles;
			StyleDefinition obj2 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemTextureFocus = "Textures\\GUI\\Controls\\item_focus_dark.dds",
				ItemTextureActive = "Textures\\GUI\\Controls\\item_active_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.2535f, 0.034f),
				TextScale = 0.8f,
				TextOffset = 0.006f,
				ItemsOffset = new Vector2(6f, 2f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = false,
				XSizeVariable = false
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
			styles2[1] = obj2;
			StyleDefinition[] styles3 = m_styles;
			StyleDefinition obj3 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemTextureFocus = "Textures\\GUI\\Controls\\item_focus_dark.dds",
				ItemTextureActive = "Textures\\GUI\\Controls\\item_active_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.25f, 0.035f),
				TextScale = 0.8f,
				TextOffset = 0.004f,
				ItemsOffset = new Vector2(6f, 2f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = true,
				XSizeVariable = true
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 0f,
				Right = 0f,
				Top = 0f,
				Bottom = 0f
			};
			obj3.ScrollbarMargin = scrollbarMargin;
			obj3.TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			styles3[2] = obj3;
			StyleDefinition[] styles4 = m_styles;
			StyleDefinition obj4 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemTextureFocus = "Textures\\GUI\\Controls\\item_focus_dark.dds",
				ItemTextureActive = "Textures\\GUI\\Controls\\item_active_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.25f, 0.035f),
				TextScale = 0.8f,
				TextOffset = 0.006f,
				ItemsOffset = new Vector2(6f, 2f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = false,
				XSizeVariable = false
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj4.ScrollbarMargin = scrollbarMargin;
			obj4.TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			styles4[3] = obj4;
			StyleDefinition[] styles5 = m_styles;
			StyleDefinition obj5 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST_TOOLS_BLOCKS,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemTextureFocus = "Textures\\GUI\\Controls\\item_focus_dark.dds",
				ItemTextureActive = "Textures\\GUI\\Controls\\item_active_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.15f, 0.0272f),
				TextScale = 0.78f,
				TextOffset = 0.006f,
				ItemsOffset = new Vector2(6f, 6f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = false,
				XSizeVariable = false
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj5.ScrollbarMargin = scrollbarMargin;
			obj5.TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			styles5[4] = obj5;
			StyleDefinition[] styles6 = m_styles;
			StyleDefinition obj6 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemTextureFocus = "Textures\\GUI\\Controls\\item_focus_dark.dds",
				ItemTextureActive = "Textures\\GUI\\Controls\\item_active_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.21f, 0.025f),
				TextScale = 0.8f,
				TextOffset = 0.006f,
				ItemsOffset = new Vector2(6f, 2f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = false,
				XSizeVariable = false
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj6.ScrollbarMargin = scrollbarMargin;
			obj6.TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			styles6[5] = obj6;
			StyleDefinition[] styles7 = m_styles;
			StyleDefinition obj7 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemTextureFocus = "Textures\\GUI\\Controls\\item_focus_dark.dds",
				ItemTextureActive = "Textures\\GUI\\Controls\\item_active_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.231f, 0.035f),
				TextScale = 0.8f,
				TextOffset = 0.006f,
				ItemsOffset = new Vector2(6f, 2f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = false,
				XSizeVariable = false
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj7.ScrollbarMargin = scrollbarMargin;
			obj7.TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			styles7[6] = obj7;
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlListboxStyleEnum style)
		{
			return m_styles[(int)style];
		}

		public MyGuiControlListbox()
			: this(null, MyGuiControlListboxStyleEnum.Default, isAutoscaleEnabled: false)
		{
		}

		public MyGuiControlListbox(Vector2? position = null, MyGuiControlListboxStyleEnum visualStyle = MyGuiControlListboxStyleEnum.Default, bool isAutoscaleEnabled = false)
<<<<<<< HEAD
			: base(position, null, null, isActiveControl: true, canHaveFocus: true)
=======
			: base(position, null, null, null, null, isActiveControl: true, canHaveFocus: true)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			SetupStyles();
			m_scrollBar = new MyVScrollbar(this);
			m_scrollBar.ValueChanged += verticalScrollBar_ValueChanged;
<<<<<<< HEAD
			base.IsAutoScaleEnabled = isAutoscaleEnabled;
=======
			m_isAutoscaleEnabled = isAutoscaleEnabled;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Items = new ObservableCollection<Item>();
			((ObservableCollection<Item>)Items).add_CollectionChanged(new NotifyCollectionChangedEventHandler(Items_CollectionChanged));
			VisualStyle = visualStyle;
			base.Name = "Listbox";
			MultiSelect = true;
			base.CanFocusChildren = true;
			base.GamepadHelpTextId = MyCommonTexts.Gamepad_Help_Listbox;
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_GuiControlListbox myObjectBuilder_GuiControlListbox = (MyObjectBuilder_GuiControlListbox)objectBuilder;
			VisibleRowsCount = myObjectBuilder_GuiControlListbox.VisibleRows;
			VisualStyle = myObjectBuilder_GuiControlListbox.VisualStyle;
			InnerMarginBottom = myObjectBuilder_GuiControlListbox.InnerMarginBottom;
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlListbox obj = (MyObjectBuilder_GuiControlListbox)base.GetObjectBuilder();
			obj.VisibleRows = VisibleRowsCount;
			obj.VisualStyle = VisualStyle;
			return obj;
		}

		public Item GetLastSelected()
		{
			if (SelectedItems == null)
			{
				SelectedItems = new List<Item>();
			}
			if (SelectedItems.Count == 0)
			{
				return null;
			}
			return SelectedItems[SelectedItems.Count - 1];
		}

		public void SelectSingleItem(Item item)
		{
			if (SelectedItems == null)
			{
				SelectedItems = new List<Item>();
			}
			SelectedItems.Clear();
			SelectedItems.Add(item);
		}

		public bool SelectByUserData(object data)
		{
			bool result = false;
			if (SelectedItems == null)
			{
				SelectedItems = new List<Item>();
			}
			foreach (Item item in Items)
			{
				if (item.UserData == data)
				{
					result = true;
					SelectedItems.Add(item);
				}
			}
			return result;
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase captureInput = base.HandleInput();
			if (captureInput != null)
			{
				return captureInput;
			}
			if (base.HasFocus)
			{
				if (SimpleFocusMode)
				{
					if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LISTBOX_SIMPLE_SELECT) && FocusedItem != null)
					{
						SelectedItems.Clear();
						SelectedItems.Add(FocusedItem);
						LastSelectedItem = FocusedItem;
						this.ItemsSelected?.Invoke(this);
						this.ItemClicked?.Invoke(this);
						return this;
					}
				}
				else
				{
					if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LISTBOX_TOGGLE_SELECTION))
					{
						SelectFocusedItem();
						return this;
					}
					if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LISTBOX_SELECT_RANGE) && FocusedItem != null)
					{
<<<<<<< HEAD
						int num = Items.IndexOf(FocusedItem);
						int num2 = ((PivotItem != null) ? Items.IndexOf(PivotItem) : ((SelectedItems.Count != 0) ? GetFirstSelectedIndex() : 0));
=======
						int num = ((Collection<Item>)(object)Items).IndexOf(FocusedItem);
						int num2 = ((PivotItem != null) ? ((Collection<Item>)(object)Items).IndexOf(PivotItem) : ((SelectedItems.Count != 0) ? GetFirstSelectedIndex() : 0));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						SelectedItems.Clear();
						if (num != -1 && num2 != -1)
						{
							int num3 = Math.Min(num2, num);
							int num4 = Math.Max(num2, num);
							for (int i = num3; i <= num4; i++)
							{
<<<<<<< HEAD
								SelectedItems.Add(Items[i]);
=======
								SelectedItems.Add(((Collection<Item>)(object)Items)[i]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
						}
						LastSelectedItem = FocusedItem;
						this.ItemsSelected?.Invoke(this);
						this.ItemClicked?.Invoke(this);
						return this;
					}
					if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LISTBOX_SELECT_ALL))
					{
						LastSelectedItem = null;
						if (SelectedItems.Count > 0)
						{
							SelectedItems.Clear();
						}
						else
						{
							SelectedItems.Clear();
<<<<<<< HEAD
							SelectedItems.AddRange(Items);
=======
							SelectedItems.AddRange((IEnumerable<Item>)Items);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						this.ItemsSelected?.Invoke(this);
						this.ItemClicked?.Invoke(this);
						return this;
					}
					if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LISTBOX_SELECT_ONLY_FOCUSED) && FocusedItem != null)
					{
						SelectedItems.Clear();
						LastSelectedItem = FocusedItem;
						PivotItem = FocusedItem;
						SelectedItems.Add(FocusedItem);
						this.ItemsSelected?.Invoke(this);
						this.ItemClicked?.Invoke(this);
						return this;
					}
				}
			}
			if (!base.Enabled || (!CheckMouseOver(!UseSimpleItemListMouseOverCheck) && !base.HasFocus))
			{
				return null;
			}
			if (m_scrollBar != null && m_scrollBar.HandleInput())
			{
				return this;
			}
			HandleNewMousePress(ref captureInput);
			Vector2 vector = MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft();
			if (m_itemsRectangle.Contains(vector))
			{
				int num5 = ComputeIndexFromPosition(vector);
<<<<<<< HEAD
				m_mouseOverItem = (IsValidIndex(num5) ? Items[num5] : null);
=======
				m_mouseOverItem = (IsValidIndex(num5) ? ((Collection<Item>)(object)Items)[num5] : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (this.ItemMouseOver != null)
				{
					this.ItemMouseOver(this);
				}
				if (m_styleDef.PriorityCaptureInput)
				{
					captureInput = this;
				}
			}
			else
			{
				m_mouseOverItem = null;
			}
			if (m_doubleClickStarted.HasValue && (float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) >= 500f)
			{
				m_doubleClickStarted = null;
			}
			return captureInput;
		}

		private void SelectFocusedItem(bool selectFocusedOnly = false)
		{
			if (FocusedItem != null)
			{
				if (selectFocusedOnly)
				{
					SelectedItems.Clear();
				}
				if (SelectedItems.Contains(FocusedItem))
				{
					SelectedItems.Remove(FocusedItem);
				}
				else
				{
					SelectedItems.Add(FocusedItem);
				}
				LastSelectedItem = FocusedItem;
				PivotItem = FocusedItem;
				this.ItemsSelected?.Invoke(this);
				this.ItemClicked?.Invoke(this);
			}
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			m_styleDef.Texture.Draw(positionAbsoluteTopLeft, base.Size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, backgroundTransitionAlpha));
			Vector2 vector = positionAbsoluteTopLeft + new Vector2(m_itemsRectangle.X, m_itemsRectangle.Y);
			int i = m_visibleRowIndexOffset;
			Vector2 normalizedSize = Vector2.Zero;
			Vector2 vector2 = Vector2.Zero;
			if (ShouldDrawIconSpacing())
			{
				normalizedSize = MyGuiConstants.LISTBOX_ICON_SIZE;
				vector2 = MyGuiConstants.LISTBOX_ICON_OFFSET;
			}
			for (int j = 0; j < VisibleRowsCount; j++)
			{
				int num = j + m_visibleRowIndexOffset;
				if (num >= ((Collection<Item>)(object)Items).Count)
				{
					break;
				}
				if (num < 0)
				{
					continue;
				}
				for (; i < ((Collection<Item>)(object)Items).Count && !((Collection<Item>)(object)Items)[i].Visible; i++)
				{
				}
				if (i >= ((Collection<Item>)(object)Items).Count)
				{
					break;
				}
				Item item = ((Collection<Item>)(object)Items)[i];
				i++;
				if (item != null)
				{
					Color color = MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha);
					Color color3;
					Color color2;
					Color color4;
					Color color6;
					Color color5;
					Color color7;
					if (item.ColorMask.HasValue)
					{
						color3 = (color2 = MyGuiControlBase.ApplyColorMaskModifiers(item.ColorMask.Value * base.ColorMask, base.Enabled, transitionAlpha));
						color4 = MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha);
						color6 = (color5 = Vector4.One);
						color7 = MyGuiControlBase.ApplyColorMaskModifiers(item.ColorMask.Value * base.ColorMask, base.Enabled, transitionAlpha);
					}
					else
					{
						color3 = MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha);
						color2 = MyGuiControlBase.ApplyColorMaskModifiers((m_styleDef.TextColorHighlight ?? Vector4.One) * base.ColorMask, base.Enabled, transitionAlpha);
						color4 = MyGuiControlBase.ApplyColorMaskModifiers((m_styleDef.TextColorFocus ?? Vector4.One) * base.ColorMask, base.Enabled, transitionAlpha);
						color6 = (color5 = (color7 = Vector4.One));
					}
					bool flag = SelectedItems.Contains(item);
					bool flag2 = item == m_mouseOverItem;
					bool flag3 = item == FocusedItem;
					string font = item.FontOverride ?? (flag ? m_styleDef.ItemFontHighlight : m_styleDef.ItemFontNormal);
					_ = FocusedItem;
					if (flag || flag2 || item == FocusedItem)
					{
						Vector2 itemSize = ItemSize;
						Vector4 one = Vector4.One;
						string text = (flag2 ? m_styleDef.ItemTextureHighlight : ((item == FocusedItem) ? (m_styleDef.ItemTextureFocus ?? m_styleDef.ItemTextureHighlight) : ((!flag) ? null : (m_styleDef.ItemTextureActive ?? m_styleDef.ItemTextureHighlight))));
						if (!string.IsNullOrEmpty(text))
						{
							MyGuiManager.DrawSpriteBatch(text, vector, itemSize, (flag2 ? color5 : (flag3 ? color7 : color6)).ToVector4() * one * transitionAlpha, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
						}
						if (flag)
						{
							MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", vector, new Vector2(0.003f, itemSize.Y), new Color(225, 230, 236).ToVector4() * one * transitionAlpha, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
						}
					}
					if (!string.IsNullOrEmpty(item.Icon))
					{
						MyGuiManager.DrawSpriteBatch(item.Icon, vector + vector2, normalizedSize, color, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					}
<<<<<<< HEAD
					float textScale = TextScale;
					float num2 = ItemSize.X - normalizedSize.X - 5f * MyGuiConstants.LISTBOX_ICON_OFFSET.X;
					if (base.IsAutoScaleEnabled)
					{
						Vector2 itemSize2 = new Vector2(num2, ItemSize.Y);
						DoEllipsisAndScaleAdjust(item, font, ref textScale, itemSize2);
					}
					else
					{
						item.TextScale = textScale;
					}
					MyGuiManager.DrawString(font, item.Text.ToString(), vector + new Vector2(normalizedSize.X + 2f * vector2.X, 0.5f * ItemSize.Y) + new Vector2(m_styleDef.TextOffset, 0f), item.TextScale, flag2 ? color2 : (flag3 ? color4 : color3), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, useFullClientArea: false, num2);
=======
					Vector2 itemSize2 = ItemSize;
					float scale = TextScale;
					if (m_isAutoscaleEnabled)
					{
						scale = MyGuiControlAutoScaleText.GetScale(font, item.Text, itemSize2, TextScale, MyGuiManager.MIN_TEXT_SCALE);
					}
					MyGuiManager.DrawString(font, item.Text.ToString(), vector + new Vector2(normalizedSize.X + 2f * vector2.X, 0.5f * ItemSize.Y) + new Vector2(m_styleDef.TextOffset, 0f), scale, flag2 ? color2 : (flag3 ? color4 : color3), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, useFullClientArea: false, ItemSize.X - normalizedSize.X - 5f * MyGuiConstants.LISTBOX_ICON_OFFSET.X);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				vector.Y += ItemSize.Y;
			}
			if (m_styleDef.DrawScroll)
			{
				m_scrollBar.Draw(MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
				Vector2 positionAbsoluteTopRight = GetPositionAbsoluteTopRight();
				positionAbsoluteTopRight.X -= m_styleDef.ScrollbarMargin.HorizontalSum + m_scrollBar.Size.X + 0.0005f;
				MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Controls\\scrollable_list_line.dds", positionAbsoluteTopRight, new Vector2(0.0012f, base.Size.Y), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
		}

		private void DoEllipsisAndScaleAdjust(Item item, string Font, ref float textScale, Vector2 itemSize)
		{
			if (textScale == MyGuiManager.MIN_TEXT_SCALE || item.TextScale == MyGuiManager.MIN_TEXT_SCALE)
			{
				return;
			}
			float num = MyGuiControlAutoScaleText.GetScale(Font, item.Text, itemSize, textScale, MyGuiManager.MIN_TEXT_SCALE);
			if (textScale != num)
			{
				if (num <= MyGuiManager.MIN_TEXT_SCALE)
				{
					num = MyGuiManager.MIN_TEXT_SCALE;
					if (base.IsAutoEllipsisEnabled)
					{
						GetTextWithEllipsis(item.Text, Font, num, itemSize);
						base.IsTextWithEllipseAlready = false;
					}
				}
				textScale = num;
				OnSizeChanged();
			}
			item.TextScale = textScale;
		}

		private bool ShouldDrawIconSpacing()
		{
			int i = m_visibleRowIndexOffset;
			for (int j = 0; j < VisibleRowsCount; j++)
			{
				int num = j + m_visibleRowIndexOffset;
				if (num >= ((Collection<Item>)(object)Items).Count)
				{
					break;
				}
				if (num >= 0)
				{
					for (; i < ((Collection<Item>)(object)Items).Count && !((Collection<Item>)(object)Items)[i].Visible; i++)
					{
					}
					if (i >= ((Collection<Item>)(object)Items).Count)
					{
						break;
					}
					Item item = ((Collection<Item>)(object)Items)[i];
					i++;
					if (item != null && !string.IsNullOrEmpty(item.Icon))
					{
						return true;
					}
				}
			}
			return false;
		}

		public override void ShowToolTip()
		{
			if (m_mouseOverItem != null && m_mouseOverItem.ToolTip != null && ((Collection<MyColoredText>)(object)m_mouseOverItem.ToolTip.ToolTips).Count > 0)
			{
				m_toolTip = m_mouseOverItem.ToolTip;
			}
			else
			{
				m_toolTip = null;
			}
			if (MyInput.Static.IsJoystickLastUsed && FocusedItem != null)
			{
				m_toolTip = FocusedItem.ToolTip;
			}
			base.ShowToolTip();
		}

		protected override Vector2 GetToolTipPosition()
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				return GetPositionAbsoluteTopLeft() + new Vector2(m_itemsRectangle.X, m_itemsRectangle.Y) + ItemSize / 2f + new Vector2(0f, (float)(m_focusedItemIndex - m_visibleRowIndexOffset) * ItemSize.Y);
			}
			return MyGuiManager.MouseCursorPosition;
		}

		protected override void OnPositionChanged()
		{
			base.OnPositionChanged();
			RefreshInternals();
		}

		protected override void OnOriginAlignChanged()
		{
			base.OnOriginAlignChanged();
			RefreshInternals();
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			m_scrollBar.HasHighlight = base.HasHighlight;
			m_mouseOverItem = null;
		}

		public override void OnRemoving()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			((ObservableCollection<Item>)Items).remove_CollectionChanged(new NotifyCollectionChangedEventHandler(Items_CollectionChanged));
			((Collection<Item>)(object)Items).Clear();
			this.ItemClicked = null;
			this.ItemDoubleClicked = null;
			this.ItemsSelected = null;
			base.OnRemoving();
		}

		public void Remove(Predicate<Item> match)
		{
			int num = Items.FindIndex(match);
			if (num != -1)
			{
				((Collection<Item>)(object)Items).RemoveAt(num);
			}
		}

		public void Add(Item item, int? position = null)
		{
			item.OnVisibleChanged += item_OnVisibleChanged;
			if (position.HasValue)
			{
				((Collection<Item>)(object)Items).Insert(position.Value, item);
			}
			else
			{
				((Collection<Item>)(object)Items).Add(item);
			}
		}

		private void item_OnVisibleChanged()
		{
			RefreshScrollBar();
		}

		private int ComputeIndexFromPosition(Vector2 position)
		{
			int num = (int)((position.Y - m_itemsRectangle.Position.Y) / ItemSize.Y);
			num++;
			int num2 = 0;
			for (int i = m_visibleRowIndexOffset; i < ((Collection<Item>)(object)Items).Count; i++)
			{
				if (((Collection<Item>)(object)Items)[i].Visible)
				{
					num2++;
				}
				if (num2 == num)
				{
					return i;
				}
			}
			return -1;
		}

		private void DebugDraw()
		{
			MyGuiManager.DrawBorders(GetPositionAbsoluteTopLeft() + m_itemsRectangle.Position, m_itemsRectangle.Size, Color.White, 1);
			m_scrollBar.DebugDraw();
		}

		private int GetFirstSelectedIndex()
		{
			if (SelectedItems == null || SelectedItems.Count == 0)
			{
				return -1;
			}
			return Math.Max(-1, Items.FindIndex((Item x) => x == SelectedItems[0]));
		}

		private void HandleNewMousePress(ref MyGuiControlBase captureInput)
		{
			Vector2 vector = MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft();
			bool flag = m_itemsRectangle.Contains(vector);
			if (MyInput.Static.IsAnyNewMouseOrJoystickPressed() && flag)
			{
				int num = ComputeIndexFromPosition(vector);
				if (IsValidIndex(num) && ((Collection<Item>)(object)Items)[num].Visible)
				{
<<<<<<< HEAD
					if (MultiSelect && MyInput.Static.IsAnyCtrlKeyPressed() && num < Items.Count)
=======
					if (MultiSelect && MyInput.Static.IsAnyCtrlKeyPressed() && num < ((Collection<Item>)(object)Items).Count)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						if (SelectedItems.Contains(((Collection<Item>)(object)Items)[num]))
						{
							SelectedItems.Remove(((Collection<Item>)(object)Items)[num]);
						}
						else
						{
							SelectedItems.Add(((Collection<Item>)(object)Items)[num]);
						}
<<<<<<< HEAD
						PivotItem = Items[num];
					}
					else if (MultiSelect && MyInput.Static.IsAnyShiftKeyPressed())
					{
						int val = ((PivotItem != null) ? Items.IndexOf(PivotItem) : ((SelectedItems.Count != 0) ? GetFirstSelectedIndex() : 0));
=======
						PivotItem = ((Collection<Item>)(object)Items)[num];
					}
					else if (MultiSelect && MyInput.Static.IsAnyShiftKeyPressed())
					{
						int val = ((PivotItem != null) ? ((Collection<Item>)(object)Items).IndexOf(PivotItem) : ((SelectedItems.Count != 0) ? GetFirstSelectedIndex() : 0));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						SelectedItems.Clear();
						int num2 = Math.Min(val, num);
						int num3 = Math.Max(val, num);
						for (int i = num2; i <= num3 && IsValidIndex(i); i++)
						{
<<<<<<< HEAD
							if (Items[i].Visible)
							{
								SelectedItems.Add(Items[i]);
=======
							if (((Collection<Item>)(object)Items)[i].Visible)
							{
								SelectedItems.Add(((Collection<Item>)(object)Items)[i]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
						}
					}
					else
					{
						SelectedItems.Clear();
<<<<<<< HEAD
						if (num < Items.Count)
						{
							SelectedItems.Add(Items[num]);
							PivotItem = Items[num];
=======
						if (num < ((Collection<Item>)(object)Items).Count)
						{
							SelectedItems.Add(((Collection<Item>)(object)Items)[num]);
							PivotItem = ((Collection<Item>)(object)Items)[num];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					if (this.ItemsSelected != null)
					{
						this.ItemsSelected(this);
					}
					m_focusedItemIndex = num;
<<<<<<< HEAD
					if (num < Items.Count)
					{
						FocusedItem = Items[num];
=======
					if (num < ((Collection<Item>)(object)Items).Count)
					{
						FocusedItem = ((Collection<Item>)(object)Items)[num];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					captureInput = this;
					if (this.ItemClicked != null)
					{
						this.ItemClicked(this);
						MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
					}
				}
			}
			if (!(MyInput.Static.IsNewPrimaryButtonPressed() && flag))
			{
				return;
			}
			if (!m_doubleClickStarted.HasValue)
			{
				int num4 = ComputeIndexFromPosition(vector);
<<<<<<< HEAD
				if (IsValidIndex(num4) && Items[num4].Visible)
=======
				if (IsValidIndex(num4) && ((Collection<Item>)(object)Items)[num4].Visible)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					m_doubleClickStarted = MyGuiManager.TotalTimeInMilliseconds;
					m_doubleClickFirstPosition = MyGuiManager.MouseCursorPosition;
				}
			}
			else if ((float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) <= 500f && (m_doubleClickFirstPosition - MyGuiManager.MouseCursorPosition).Length() <= 0.005f)
			{
				if (this.ItemDoubleClicked != null)
				{
					this.ItemDoubleClicked(this);
				}
				m_doubleClickStarted = null;
				captureInput = this;
			}
		}

		private void RefreshVisualStyle()
		{
			if (m_useCustomStyle)
			{
				m_styleDef = m_customStyle;
			}
			else
			{
				m_styleDef = GetVisualStyle(VisualStyle);
			}
			ItemSize = m_styleDef.ItemSize;
			TextScale = m_styleDef.TextScale;
			RefreshInternals();
		}

		private float ComputeVariableItemWidth()
		{
			float num = 0.015f;
			int num2 = 0;
			foreach (Item item in Items)
			{
				if (item.Text.Length > num2)
				{
					num2 = item.Text.Length;
				}
			}
			return (float)num2 * num;
		}

		public bool IsOverScrollBar()
		{
			return m_scrollBar.IsOverCaret;
		}

		private void RefreshInternals()
		{
			Vector2 minSizeGui = m_styleDef.Texture.MinSizeGui;
			Vector2 maxSizeGui = m_styleDef.Texture.MaxSizeGui;
			if (m_styleDef.XSizeVariable)
			{
				ItemSize = new Vector2(ComputeVariableItemWidth(), ItemSize.Y);
			}
			if (m_styleDef.DrawScroll && !m_styleDef.XSizeVariable)
			{
				base.Size = Vector2.Clamp(new Vector2(m_styleDef.TextOffset + m_styleDef.ScrollbarMargin.HorizontalSum + m_styleDef.ItemSize.X + m_scrollBar.Size.X, minSizeGui.Y + m_styleDef.ItemSize.Y * (float)VisibleRowsCount + InnerMarginBottom), minSizeGui, maxSizeGui);
			}
			else
			{
				base.Size = Vector2.Clamp(new Vector2(m_styleDef.TextOffset + ItemSize.X, minSizeGui.Y + ItemSize.Y * (float)VisibleRowsCount + m_innerMarginBottom), minSizeGui, maxSizeGui);
			}
			RefreshScrollBar();
			m_itemsRectangle.X = m_styleDef.ItemsOffset.X;
			m_itemsRectangle.Y = m_styleDef.ItemsOffset.Y + m_styleDef.Texture.LeftTop.SizeGui.Y;
			m_itemsRectangle.Width = ItemSize.X;
			m_itemsRectangle.Height = ItemSize.Y * (float)VisibleRowsCount;
		}

		private void RefreshScrollBar()
		{
			if (IsInBulkInsert)
			{
				return;
			}
			int num = 0;
			foreach (Item item in Items)
			{
				if (item.Visible)
				{
					num++;
				}
			}
			m_scrollBar.Visible = num > VisibleRowsCount;
			m_scrollBar.Init(num, VisibleRowsCount);
			Vector2 vector = base.Size * new Vector2(0.5f, -0.5f);
			MyGuiBorderThickness scrollbarMargin = m_styleDef.ScrollbarMargin;
			Vector2 position = new Vector2(vector.X - (scrollbarMargin.Right + m_scrollBar.Size.X), vector.Y + scrollbarMargin.Top);
			m_scrollBar.Layout(position, base.Size.Y - scrollbarMargin.VerticalSum);
		}

		/// <summary>
		/// GR: Individual controls should reset toolbar postion if needed.
		/// Do no do in refresh of toolbar becaues it may happen often and cause bugs (autoscrolling to top every few frames when not intended)
		/// </summary>
		public void ScrollToolbarToTop()
		{
			m_scrollBar.SetPage(0f);
		}

		public void ScrollToFirstSelection()
		{
			if (((Collection<Item>)(object)Items).Count == 0 || SelectedItems.Count == 0)
			{
				return;
			}
			Item item = SelectedItems[0];
			int num = -1;
			for (int i = 0; i < ((Collection<Item>)(object)Items).Count; i++)
			{
				Item item2 = ((Collection<Item>)(object)Items)[i];
				if (item == item2)
				{
					num = i;
					break;
				}
			}
			if (m_visibleRowIndexOffset > num || num >= m_visibleRowIndexOffset + m_visibleRows)
			{
				SetScrollPosition(num);
			}
		}

		public void ScrollToItem(Item item)
		{
			int num = -1;
<<<<<<< HEAD
			for (int i = 0; i < Items.Count; i++)
			{
				Item item2 = Items[i];
=======
			for (int i = 0; i < ((Collection<Item>)(object)Items).Count; i++)
			{
				Item item2 = ((Collection<Item>)(object)Items)[i];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (item2.Visible)
				{
					num++;
				}
				if (item == item2)
				{
					break;
				}
			}
			bool flag = m_scrollBar.Value > (float)num;
			bool flag2 = (double)num >= Math.Floor(m_scrollBar.Value + (float)m_visibleRows);
			if (flag || flag2)
			{
				if (flag)
				{
					SetScrollPosition(num);
				}
				else if (flag2)
				{
					SetScrollPosition(num - m_visibleRows + 1);
				}
			}
		}

		private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			if ((int)e.get_Action() == 1 || (int)e.get_Action() == 2)
			{
				foreach (object oldItem in e.get_OldItems())
				{
					if (SelectedItems.Contains((Item)oldItem))
					{
						SelectedItems.Remove((Item)oldItem);
					}
				}
				if (this.ItemsSelected != null)
				{
					this.ItemsSelected(this);
				}
			}
			RefreshScrollBar();
		}

		private void verticalScrollBar_ValueChanged(MyScrollbar scrollbar)
		{
			int num = (int)scrollbar.Value;
			int num2 = -1;
			for (int i = 0; i < ((Collection<Item>)(object)Items).Count; i++)
			{
				if (((Collection<Item>)(object)Items)[i].Visible)
				{
					num2++;
				}
				if (num2 == num)
				{
					num = i;
					break;
				}
			}
			m_visibleRowIndexOffset = num;
		}

		private bool IsValidIndex(int idx)
		{
			if (0 <= idx)
			{
				return idx < ((Collection<Item>)(object)Items).Count;
			}
			return false;
		}

		public void SelectAllVisible()
		{
			SelectedItems.Clear();
			foreach (Item item in Items)
			{
				if (item.Visible)
				{
					SelectedItems.Add(item);
				}
			}
			if (this.ItemsSelected != null)
			{
				this.ItemsSelected(this);
			}
		}

		public void ChangeSelection(List<bool> states)
		{
			SelectedItems.Clear();
			int num = 0;
			foreach (Item item in Items)
			{
				if (num >= states.Count)
				{
					break;
				}
				if (states[num])
				{
					SelectedItems.Add(item);
				}
				num++;
			}
			if (this.ItemsSelected != null)
			{
				this.ItemsSelected(this);
			}
		}

		public void ClearItems()
		{
			((Collection<Item>)(object)Items).Clear();
		}

		public float GetScrollPosition()
		{
			return m_scrollBar.Value;
		}

		public void SetScrollPosition(float position)
		{
			m_scrollBar.Value = position;
		}

		public void StoreSituation()
		{
			m_StoredSelectedItems.Clear();
			m_StoredTopmostSelectedItem = null;
			m_StoredMouseOverItem = null;
			m_StoredItemOnTop = null;
			m_StoredTopmostSelectedPosition = m_visibleRows;
			foreach (Item selectedItem in SelectedItems)
			{
				m_StoredSelectedItems.Add(selectedItem);
				int num = ((Collection<Item>)(object)Items).IndexOf(SelectedItems[0]);
				if (num < m_StoredTopmostSelectedPosition && num >= m_visibleRowIndexOffset)
				{
					m_StoredTopmostSelectedPosition = num;
					m_StoredTopmostSelectedItem = selectedItem;
				}
			}
			m_StoredMouseOverItem = m_mouseOverItem;
			int num2 = 0;
			if (m_mouseOverItem != null)
			{
				foreach (Item item in Items)
				{
					if (m_mouseOverItem == item)
					{
						m_StoredMouseOverPosition = num2;
						break;
					}
					num2++;
				}
			}
			if (FirstVisibleRow < ((Collection<Item>)(object)Items).Count)
			{
				m_StoredItemOnTop = ((Collection<Item>)(object)Items)[FirstVisibleRow];
			}
			m_StoredScrollbarValue = m_scrollBar.Value;
		}

		private bool CompareItems(Item item1, Item item2, bool compareUserData, bool compareText)
		{
			if (compareUserData && compareText)
			{
				if (item1.UserData == item2.UserData && item1.Text.CompareTo(item2.Text) == 0)
				{
					return true;
				}
				return false;
			}
			if (compareUserData && item1.UserData == item2.UserData)
			{
				return true;
			}
			if (compareText && item1.Text.CompareTo(item2.Text) == 0)
			{
				return true;
			}
			return false;
		}

		public void RestoreSituation(bool compareUserData, bool compareText)
		{
			SelectedItems.Clear();
			foreach (Item storedSelectedItem in m_StoredSelectedItems)
			{
				foreach (Item item in Items)
				{
					if (CompareItems(storedSelectedItem, item, compareUserData, compareText))
					{
						SelectedItems.Add(item);
						break;
					}
				}
			}
			int num = -1;
			int num2 = -1;
			int num3 = 0;
			foreach (Item item2 in Items)
			{
				if (m_StoredMouseOverItem != null && CompareItems(item2, m_StoredMouseOverItem, compareUserData, compareText))
				{
					m_scrollBar.Value = m_StoredScrollbarValue + (float)num3 - (float)m_StoredMouseOverPosition;
					return;
				}
				if (m_StoredTopmostSelectedItem != null && CompareItems(item2, m_StoredTopmostSelectedItem, compareUserData, compareText))
				{
					num = num3;
				}
				if (m_StoredItemOnTop != null && CompareItems(item2, m_StoredItemOnTop, compareUserData, compareText))
				{
					num2 = num3;
				}
				num3++;
			}
			if (m_StoredTopmostSelectedPosition != m_visibleRows)
			{
				m_scrollBar.Value = m_StoredScrollbarValue + (float)num - (float)m_StoredTopmostSelectedPosition;
			}
			else if (num2 != -1)
			{
				m_scrollBar.Value = num2;
			}
			else
			{
				m_scrollBar.Value = m_StoredScrollbarValue;
			}
		}

		public void ApplyStyle(StyleDefinition style)
		{
			m_useCustomStyle = true;
			m_customStyle = style;
			RefreshVisualStyle();
		}

		private RectangleF GetRowRectangle(List<Item> selectedItems)
		{
			if (selectedItems.Count > 0)
			{
				int num = Items.FindIndex((Item x) => x == selectedItems[0]);
				int num2 = 0;
				for (int i = m_visibleRowIndexOffset; i < num; i++)
				{
					if (((Collection<Item>)(object)Items)[i].Visible)
					{
						num2++;
					}
				}
				return new RectangleF(GetPositionAbsoluteTopLeft() + new Vector2(0f, (float)num2 * ItemSize.Y), new Vector2(base.Size.X, ItemSize.Y));
			}
			return base.Rectangle;
		}

		public override void OnFocusChanged(bool focus)
		{
<<<<<<< HEAD
			if (!SimpleFocusMode && focus && SelectedItems.Count <= 0 && Items.Count > 0)
			{
				int num = (int)((m_entryPoint.Y - m_itemsRectangle.Position.Y) / ItemSize.Y);
				if (num >= Items.Count)
=======
			if (!SimpleFocusMode && focus && SelectedItems.Count <= 0 && ((Collection<Item>)(object)Items).Count > 0)
			{
				int num = (int)((m_entryPoint.Y - m_itemsRectangle.Position.Y) / ItemSize.Y);
				if (num >= ((Collection<Item>)(object)Items).Count)
				{
					num = ((Collection<Item>)(object)Items).Count - 1;
				}
				else if (num < 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					num = 0;
				}
<<<<<<< HEAD
				else if (num < 0)
				{
					num = 0;
				}
				SelectedItems.Add(Items[num]);
=======
				SelectedItems.Add(((Collection<Item>)(object)Items)[num]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				this.ItemsSelected?.Invoke(this);
				ScrollToFirstSelection();
			}
			if (!focus)
			{
				if (FocusedItem != null)
				{
					PivotItem = FocusedItem;
				}
				FocusedItem = null;
			}
			base.OnFocusChanged(focus);
		}

		public override MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			if (currentFocusControl == this)
			{
				if (page || ((Collection<Item>)(object)Items).Count == 0)
				{
					return null;
				}
<<<<<<< HEAD
				if ((m_focusedItemIndex == -1 && SelectedItems.Count > 0) || m_focusedItemIndex > Items.Count - 1)
=======
				if ((m_focusedItemIndex == -1 && SelectedItems.Count > 0) || m_focusedItemIndex > ((Collection<Item>)(object)Items).Count - 1)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					m_focusedItemIndex = GetFirstSelectedIndex();
				}
				bool flag = MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MODIF_R, MyControlStateType.PRESSED);
				bool flag2 = MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_A, MyControlStateType.PRESSED);
				if (SimpleFocusMode)
				{
					SelectedItems.Clear();
				}
				Item item = null;
				_ = FocusedItem;
				int itemIndex = -1;
				item = ((m_focusedItemIndex != -1) ? GetNextHighlightItem(direction, out itemIndex) : GetDefaultHighlightItem(direction, out itemIndex));
				if (item == null)
				{
					if (FocusedItem != null)
					{
						PivotItem = FocusedItem;
					}
					FocusedItem = null;
					return null;
				}
				m_focusedItemIndex = itemIndex;
				FocusedItem = item;
				if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MODIF_L, MyControlStateType.PRESSED))
				{
					PivotItem = item;
				}
				ScrollToItem(FocusedItem);
				if (flag && flag2)
				{
					if (SelectedItems.Contains(item))
					{
						SelectedItems.Remove(item);
					}
					else
					{
						SelectedItems.Add(item);
					}
					this.ItemsSelected?.Invoke(this);
					this.ItemClicked?.Invoke(this);
					return this;
				}
			}
			else
			{
				if (PivotItem != null)
				{
					FocusedItem = PivotItem;
				}
				m_entryPoint = currentFocusControl.FocusRectangle.Center;
				m_entryDirection = direction;
			}
			return this;
		}

		private Item GetNextHighlightItem(MyDirection direction, out int itemIndex)
		{
			itemIndex = -1;
<<<<<<< HEAD
			if (Items.Count == 0 || m_focusedItemIndex == -1)
=======
			if (((Collection<Item>)(object)Items).Count == 0 || m_focusedItemIndex == -1)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return null;
			}
			switch (direction)
			{
			case MyDirection.Down:
			{
				int num;
<<<<<<< HEAD
				for (num = m_focusedItemIndex + 1; num >= 0 && num <= Items.Count - 1 && !Items[num].Visible; num++)
				{
				}
				if (num < 0 || num >= Items.Count)
=======
				for (num = m_focusedItemIndex + 1; num >= 0 && num <= ((Collection<Item>)(object)Items).Count - 1 && !((Collection<Item>)(object)Items)[num].Visible; num++)
				{
				}
				if (num < 0 || num >= ((Collection<Item>)(object)Items).Count)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return null;
				}
				itemIndex = num;
<<<<<<< HEAD
				return Items[num];
=======
				return ((Collection<Item>)(object)Items)[num];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			case MyDirection.Up:
			{
				int num = m_focusedItemIndex - 1;
<<<<<<< HEAD
				while (num >= 0 && num < Items.Count && !Items[num].Visible)
				{
					num--;
				}
				if (num < 0 || num >= Items.Count)
=======
				while (num >= 0 && num < ((Collection<Item>)(object)Items).Count && !((Collection<Item>)(object)Items)[num].Visible)
				{
					num--;
				}
				if (num < 0 || num >= ((Collection<Item>)(object)Items).Count)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return null;
				}
				itemIndex = num;
<<<<<<< HEAD
				return Items[num];
=======
				return ((Collection<Item>)(object)Items)[num];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			default:
				return null;
			}
		}

		private Item GetDefaultHighlightItem(MyDirection direction, out int itemIndex)
		{
			itemIndex = -1;
<<<<<<< HEAD
			if (Items.Count == 0)
=======
			if (((Collection<Item>)(object)Items).Count == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return null;
			}
			int num = 0;
			switch (direction)
			{
			case MyDirection.Down:
<<<<<<< HEAD
				for (num = 0; num < Items.Count && !Items[num].Visible; num++)
				{
				}
				itemIndex = num;
				return Items[num];
			case MyDirection.Up:
				num = Items.Count - 1;
				while (num > 0 && !Items[num].Visible)
=======
				for (num = 0; num < ((Collection<Item>)(object)Items).Count && !((Collection<Item>)(object)Items)[num].Visible; num++)
				{
				}
				itemIndex = num;
				return ((Collection<Item>)(object)Items)[num];
			case MyDirection.Up:
				num = ((Collection<Item>)(object)Items).Count - 1;
				while (num > 0 && !((Collection<Item>)(object)Items)[num].Visible)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					num--;
				}
				itemIndex = num;
<<<<<<< HEAD
				return Items[num];
=======
				return ((Collection<Item>)(object)Items)[num];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			default:
				return null;
			}
		}
	}
}
