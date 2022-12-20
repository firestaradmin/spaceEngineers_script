using System;
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
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlButton))]
	public class MyGuiControlButton : MyGuiControlAutoScaleText
	{
		public class StyleDefinition
		{
			public string NormalFont = "Blue";

			public string HighlightFont = "White";

			public MyGuiCompositeTexture NormalTexture;

			public MyGuiCompositeTexture HighlightTexture;

			public MyGuiCompositeTexture FocusTexture;

			public MyGuiCompositeTexture ActiveTexture;

			public Vector2? SizeOverride;

			public MyGuiBorderThickness Padding;

			public Vector4 BackgroundColor = Vector4.One;

			public string Underline;

			public string UnderlineHighlight;

			public string MouseOverCursor = "Textures\\GUI\\MouseCursor.dds";

			public Vector4? TextColorHighlight;

			public Vector4? TextColorFocus;

			public Vector4? TextColorActive;

			public Vector4? TextColor;
		}

		private static readonly StyleDefinition[] m_styles;

		public float IconScale = 1f;

		public float IconScale = 1f;

		private bool m_readyToClick;

		private bool m_buttonPressedGamepad;

		private string m_text;

		private MyStringId m_textEnum;

		private float m_textScale;

		private float m_buttonScale = 1f;

		private bool m_activateOnMouseRelease;

		private float m_iconRotation;

		public bool ClickCallbackRespectsEnabledState = true;

		private StringBuilder m_drawText = new StringBuilder();

		private RectangleF m_internalArea;

		private Vector4 m_textColor = Vector4.One;

		protected GuiSounds m_cueEnum;

		private bool m_checked;

		public bool Selected;

		private float m_textScaleWithLanguage;

		public MyGuiDrawAlignEnum TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;

		public string TextFont;

		public bool DrawCrossTextureWhenDisabled;

		private MyGuiControlButtonStyleEnum m_visualStyle;

		private StyleDefinition m_styleDef;

		public MyGuiHighlightTexture? Icon;

		public MyGuiDrawAlignEnum IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;

		private bool m_useCustomStyle;

		private StyleDefinition m_customStyle;

		private bool ReadyToClick
		{
			get
			{
				return m_readyToClick;
			}
			set
			{
				if (m_readyToClick != value)
				{
					m_readyToClick = value;
					RefreshInternals();
				}
			}
		}

		private bool ButtonPressedGamepad
		{
			get
			{
				return m_buttonPressedGamepad;
			}
			set
			{
				if (m_buttonPressedGamepad != value)
				{
					m_buttonPressedGamepad = value;
					RefreshInternals();
				}
			}
		}

		public float IconRotation
		{
			get
			{
				return m_iconRotation;
			}
			set
			{
				m_iconRotation = value;
				RefreshInternals();
			}
		}

		public bool Checked
		{
			get
			{
				return m_checked;
			}
			set
			{
				m_checked = value;
				RefreshInternals();
			}
		}

		public bool ActivateOnMouseRelease
		{
			get
			{
				return m_activateOnMouseRelease;
			}
			set
			{
				m_activateOnMouseRelease = value;
			}
		}

		public int Index { get; private set; }

		public string Text
		{
			get
			{
				return m_text;
			}
			set
			{
				if (value == null || (value != null && !value.EndsWith("…")))
				{
					base.IsTextWithEllipseAlready = false;
				}
				m_text = value;
				UpdateText();
			}
		}

		public MyStringId TextEnum
		{
			get
			{
				return m_textEnum;
			}
			set
			{
				m_textEnum = value;
				UpdateText();
			}
		}

		public GuiSounds CueEnum
		{
			get
			{
				return m_cueEnum;
			}
			set
			{
				m_cueEnum = value;
			}
		}

		public float TextScale
		{
			get
			{
				return m_textScale;
			}
			set
			{
				m_textScale = value;
				TextScaleWithLanguage = value * MyGuiManager.LanguageTextScale;
			}
		}

		protected float ButtonScale
		{
			get
			{
				return m_buttonScale;
			}
			set
			{
				m_buttonScale = value;
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

		public MyGuiControlButtonStyleEnum VisualStyle
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

		public StyleDefinition CustomStyle
		{
			get
			{
				if (!m_useCustomStyle)
				{
					return null;
				}
				return m_customStyle;
			}
			set
			{
				m_customStyle = value;
				m_useCustomStyle = value != null;
				RefreshVisualStyle();
			}
		}

		public event Action<MyGuiControlButton> ButtonClicked;

		public event Action<MyGuiControlButton> SecondaryButtonClicked;

		static MyGuiControlButton()
		{
			MyGuiBorderThickness myGuiBorderThickness = new MyGuiBorderThickness
			{
				Left = 7f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 6f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 10f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			MyGuiBorderThickness padding = myGuiBorderThickness;
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlButtonStyleEnum>() + 1];
			m_styles[0] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_OUTLINELESS_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_FOCUS,
				ActiveTexture = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_ACTIVE,
				NormalFont = "Blue",
				HighlightFont = "White",
				Padding = padding,
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[1] = new StyleDefinition
			{
				NormalTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.LeftTop
				},
				HighlightTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_OUTLINELESS_HIGHLIGHT.LeftTop
				},
				FocusTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_FOCUS.LeftTop
				},
				ActiveTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_ACTIVE.LeftTop
				},
				NormalFont = "Blue",
				HighlightFont = "White",
				SizeOverride = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui * 0.75f,
				Padding = padding,
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[13] = new StyleDefinition
			{
				NormalTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_SWITCHONOFF_LEFT_NORMAL.LeftTop
				},
				HighlightTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_SWITCHONOFF_LEFT_HIGHLIGHT.LeftTop
				},
				NormalFont = "Blue",
				HighlightFont = "White",
				SizeOverride = MyGuiConstants.TEXTURE_SWITCHONOFF_LEFT_NORMAL.MinSizeGui * 0.75f,
				Padding = padding
			};
			m_styles[9] = new StyleDefinition
			{
				NormalFont = "Blue",
				HighlightFont = "White",
				Underline = "Textures\\GUI\\UnderlineHighlight.dds",
				UnderlineHighlight = "Textures\\GUI\\UnderlineHighlight.dds",
				MouseOverCursor = "Textures\\GUI\\MouseCursorHand.dds",
				FocusTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_FOCUS_BORDER,
				ActiveTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_ACTIVE_BORDER,
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[18] = new StyleDefinition
			{
				NormalFont = "UrlNormal",
				HighlightFont = "UrlHighlight",
				Underline = "Textures\\GUI\\Underline.dds",
				UnderlineHighlight = "Textures\\GUI\\UnderlineHighlight.dds",
				MouseOverCursor = "Textures\\GUI\\MouseCursorHand.dds"
			};
			m_styles[19] = new StyleDefinition
			{
				NormalFont = "UrlNormal",
				HighlightFont = "UrlHighlight",
				MouseOverCursor = "Textures\\GUI\\MouseCursorHand.dds"
			};
			m_styles[34] = new StyleDefinition
			{
				NormalFont = "UrlNormal",
				HighlightFont = "UrlHighlight",
				MouseOverCursor = "Textures\\GUI\\MouseCursorHand.dds",
				NormalTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER,
				HighlightTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER,
				FocusTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_FOCUS_BORDER,
				ActiveTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_ACTIVE_BORDER,
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[2] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_RED_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_RED_HIGHLIGHT,
				NormalFont = "Red",
				HighlightFont = "White",
				Padding = padding
			};
			m_styles[3] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_CLOSE_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_CLOSE_HIGHLIGHT,
				NormalFont = "Blue",
				HighlightFont = "White"
			};
			m_styles[4] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_CLOSE_BCG_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_CLOSE_BCG_HIGHLIGHT,
				NormalFont = "Blue",
				HighlightFont = "White",
				BackgroundColor = new Vector4(1f, 1f, 1f, 0.9f)
			};
			m_styles[5] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_INFO_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_INFO_HIGHLIGHT,
				NormalFont = "Blue",
				HighlightFont = "White"
			};
			m_styles[6] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_INVENTORY_TRASH_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_INVENTORY_TRASH_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_INVENTORY_TRASH_FOCUS,
				ActiveTexture = MyGuiConstants.TEXTURE_INVENTORY_TRASH_ACTIVE,
				NormalFont = "Blue",
				HighlightFont = "White"
			};
			m_styles[29] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_WITHDRAW_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_WITHDRAW_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_WITHDRAW_FOCUS,
				ActiveTexture = MyGuiConstants.TEXTURE_WITHDRAW_ACTIVE,
				NormalFont = "Blue",
				HighlightFont = "White"
			};
			m_styles[30] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_DEPOSITALL_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_DEPOSITALL_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_DEPOSITALL_FOCUS,
				ActiveTexture = MyGuiConstants.TEXTURE_DEPOSITALL_ACTIVE,
				NormalFont = "Blue",
				HighlightFont = "White"
			};
			m_styles[31] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_ADDTOPRODUCTION_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_ADDTOPRODUCTION_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_ADDTOPRODUCTION_FOCUS,
				ActiveTexture = MyGuiConstants.TEXTURE_ADDTOPRODUCTION_ACTIVE,
				NormalFont = "Blue",
				HighlightFont = "White"
			};
			m_styles[32] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_SELECTEDTOPRODUCTION_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_SELECTEDTOPRODUCTION_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_SELECTEDTOPRODUCTION_FOCUS,
				ActiveTexture = MyGuiConstants.TEXTURE_SELECTEDTOPRODUCTION_ACTIVE,
				NormalFont = "Blue",
				HighlightFont = "White"
			};
			m_styles[7] = new StyleDefinition
			{
				NormalTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.LeftTop
				},
				HighlightTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_HIGHLIGHT.LeftTop
				},
				FocusTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_FOCUS.LeftTop
				},
				ActiveTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_ACTIVE.LeftTop
				},
				NormalFont = "Blue",
				HighlightFont = "White",
				SizeOverride = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui * new Vector2(0.55f, 0.65f),
				Padding = padding,
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[8] = new StyleDefinition
			{
				NormalTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.LeftTop
				},
				HighlightTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_HIGHLIGHT.LeftTop
				},
				FocusTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_FOCUS.LeftTop
				},
				ActiveTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_ACTIVE.LeftTop
				},
				NormalFont = "Blue",
				HighlightFont = "White",
				SizeOverride = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui * new Vector2(0.5f, 0.8f),
				Padding = padding,
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[10] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_INCREASE,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_INCREASE_HIGHLIGHT,
				ActiveTexture = MyGuiConstants.TEXTURE_BUTTON_INCREASE_ACTIVE,
				FocusTexture = MyGuiConstants.TEXTURE_BUTTON_INCREASE_FOCUS
			};
			m_styles[11] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_DECREASE,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_DECREASE_HIGHLIGHT,
				ActiveTexture = MyGuiConstants.TEXTURE_BUTTON_DECREASE_ACTIVE,
				FocusTexture = MyGuiConstants.TEXTURE_BUTTON_DECREASE_FOCUS
			};
			m_styles[12] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER,
				HighlightTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER,
				FocusTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_FOCUS_BORDER,
				ActiveTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_ACTIVE_BORDER,
<<<<<<< HEAD
				NormalFont = "Blue",
				HighlightFont = "White",
				Padding = new MyGuiBorderThickness(5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y),
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[35] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_CHECKED_BORDER,
				HighlightTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER,
				FocusTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_FOCUS_BORDER,
				ActiveTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_CHECKED_BORDER,
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				NormalFont = "Blue",
				HighlightFont = "White",
				Padding = new MyGuiBorderThickness(5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y),
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
<<<<<<< HEAD
=======
			};
			m_styles[35] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_CHECKED_BORDER,
				HighlightTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER,
				FocusTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_FOCUS_BORDER,
				ActiveTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_CHECKED_BORDER,
				NormalFont = "Blue",
				HighlightFont = "White",
				Padding = new MyGuiBorderThickness(5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y),
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			};
			m_styles[33] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON,
				HighlightTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_FOCUS,
				ActiveTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_ACTIVE,
				NormalFont = "Blue",
				HighlightFont = "White",
				Padding = new MyGuiBorderThickness(5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y),
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[14] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_ARROW_LEFT,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_ARROW_LEFT_HIGHLIGHT
			};
			m_styles[15] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_ARROW_RIGHT,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_ARROW_RIGHT_HIGHLIGHT
			};
			m_styles[16] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_SQUARE_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_SQUARE_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_BUTTON_SQUARE_FOCUS,
				ActiveTexture = MyGuiConstants.TEXTURE_BUTTON_SQUARE_ACTIVE,
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[17] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_SQUARE_SMALL_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_SQUARE_SMALL_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_BUTTON_SQUARE_SMALL_FOCUS,
				ActiveTexture = MyGuiConstants.TEXTURE_BUTTON_SQUARE_SMALL_ACTIVE,
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[20] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_RED_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_RED_HIGHLIGHT,
				NormalFont = "ErrorMessageBoxText",
				HighlightFont = "White",
				Padding = padding
			};
			m_styles[23] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_BUG_NORMAL,
				FocusTexture = MyGuiConstants.TEXTURE_BUTTON_BUG_FOCUS
			};
			m_styles[21] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_LIKE_NORMAL,
				FocusTexture = MyGuiConstants.TEXTURE_BUTTON_LIKE_FOCUS
			};
			m_styles[22] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_ENVELOPE_NORMAL,
				FocusTexture = MyGuiConstants.TEXTURE_BUTTON_ENVELOPE_FOCUS
			};
			m_styles[24] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_HELP_NORMAL,
				FocusTexture = MyGuiConstants.TEXTURE_BUTTON_HELP_FOCUS
			};
			StyleDefinition[] styles = m_styles;
			StyleDefinition obj = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_STRIPE_LEFT_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_STRIPE_LEFT_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_BUTTON_STRIPE_LEFT_FOCUS,
				ActiveTexture = MyGuiConstants.TEXTURE_BUTTON_STRIPE_LEFT_ACTIVE,
				NormalFont = "Blue",
				HighlightFont = "White"
			};
			myGuiBorderThickness = new MyGuiBorderThickness
			{
				Left = 11.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 6f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 10f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj.Padding = myGuiBorderThickness;
			obj.BackgroundColor = new Vector4(1f, 1f, 1f, 0.9f);
			obj.TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
			styles[25] = obj;
			m_styles[28] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_BUTTON_SQUARE_48_NORMAL,
				HighlightTexture = MyGuiConstants.TEXTURE_BUTTON_SQUARE_48_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_BUTTON_SQUARE_48_FOCUS,
				ActiveTexture = MyGuiConstants.TEXTURE_BUTTON_SQUARE_48_ACTIVE,
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[26] = new StyleDefinition
			{
				NormalTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.LeftTop
				},
				HighlightTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_HIGHLIGHT.LeftTop
				},
				FocusTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_FOCUS.LeftTop
				},
				ActiveTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_ACTIVE.LeftTop
				},
				NormalFont = "Blue",
				HighlightFont = "White",
				SizeOverride = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui * 0.78f,
				Padding = padding,
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[27] = new StyleDefinition
			{
				NormalTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.LeftTop
				},
				HighlightTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_HIGHLIGHT.LeftTop
				},
				FocusTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_FOCUS.LeftTop
				},
				ActiveTexture = new MyGuiCompositeTexture
				{
					Center = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_ACTIVE.LeftTop
				},
				NormalFont = "Blue",
				HighlightFont = "White",
				SizeOverride = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui * 0.815f * new Vector2(1.05f, 1f),
				Padding = padding,
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlButtonStyleEnum style)
		{
			return m_styles[(int)style];
		}

		public MyGuiControlButton()
			: this(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, null, GuiSounds.MouseClick, 1f, null, activateOnMouseRelease: false, isAutoscaleEnabled: false, isEllipsisEnabled: false, null)
		{
		}

		public MyGuiControlButton(Vector2? position = null, MyGuiControlButtonStyleEnum visualStyle = MyGuiControlButtonStyleEnum.Default, Vector2? size = null, Vector4? colorMask = null, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, string toolTip = null, StringBuilder text = null, float textScale = 0.8f, MyGuiDrawAlignEnum textAlignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType highlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER, Action<MyGuiControlButton> onButtonClick = null, GuiSounds cueEnum = GuiSounds.MouseClick, float buttonScale = 1f, int? buttonIndex = null, bool activateOnMouseRelease = false, bool isAutoscaleEnabled = false, bool isEllipsisEnabled = false, Action<MyGuiControlButton> onSecondaryButtonClick = null)
			: base(position ?? Vector2.Zero, size, colorMask ?? MyGuiConstants.BUTTON_BACKGROUND_COLOR, originAlign, toolTip, highlightType, canHaveFocus: true)
		{
			base.Name = "Button";
			this.ButtonClicked = onButtonClick;
			this.SecondaryButtonClicked = onSecondaryButtonClick;
			Index = buttonIndex ?? 0;
			UpdateText();
			m_drawText.Clear().Append((object)text);
			base.IsAutoScaleEnabled = isAutoscaleEnabled;
			base.IsAutoEllipsisEnabled = isEllipsisEnabled;
			if (text != null)
			{
				Text = text.ToString();
			}
			TextScale = textScale;
			TextAlignment = textAlignment;
			VisualStyle = visualStyle;
			m_cueEnum = cueEnum;
			m_activateOnMouseRelease = activateOnMouseRelease;
			ButtonScale = buttonScale;
			base.Size *= ButtonScale;
			base.GamepadHelpTextId = MyCommonTexts.Gamepad_Help_Button;
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase == null)
			{
				if (!m_activateOnMouseRelease)
				{
					if (base.IsMouseOver && (MyInput.Static.IsNewPrimaryButtonPressed() || MyInput.Static.IsNewSecondaryButtonPressed()))
					{
						ReadyToClick = true;
					}
					if (!base.IsMouseOver && (MyInput.Static.IsNewPrimaryButtonReleased() || MyInput.Static.IsNewSecondaryButtonPressed()))
					{
						ReadyToClick = false;
					}
				}
				else
				{
					ReadyToClick = true;
<<<<<<< HEAD
				}
				if (base.HasFocus && (MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01) || MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsNewKeyPressed(MyKeys.Space)))
				{
					ButtonPressedGamepad = true;
				}
=======
				}
				if (base.HasFocus && (MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01) || MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsNewKeyPressed(MyKeys.Space)))
				{
					ButtonPressedGamepad = true;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!base.HasFocus && (MyInput.Static.IsJoystickButtonNewReleased(MyJoystickButtonsEnum.J01) || MyInput.Static.IsNewKeyReleased(MyKeys.Enter) || MyInput.Static.IsNewKeyReleased(MyKeys.Space)))
				{
					ButtonPressedGamepad = false;
				}
				if ((base.IsMouseOver && (MyInput.Static.IsNewPrimaryButtonReleased() || MyInput.Static.IsNewSecondaryButtonReleased()) && ReadyToClick) || (base.HasFocus && (MyInput.Static.IsNewKeyReleased(MyKeys.Enter) || MyInput.Static.IsNewKeyReleased(MyKeys.Space) || MyInput.Static.IsJoystickButtonNewReleased(MyJoystickButtonsEnum.J01)) && ButtonPressedGamepad))
				{
					if (base.Enabled || !ClickCallbackRespectsEnabledState)
					{
						MyGuiSoundManager.PlaySound(m_cueEnum);
						if (MyInput.Static.IsNewSecondaryButtonReleased())
						{
							this.SecondaryButtonClicked.InvokeIfNotNull(this);
						}
						else
						{
							this.ButtonClicked.InvokeIfNotNull(this);
						}
					}
					myGuiControlBase = this;
					ReadyToClick = false;
					ButtonPressedGamepad = false;
					return myGuiControlBase;
				}
			}
			return myGuiControlBase;
		}

		public void PressButton(bool isPrimary = true)
		{
			if (isPrimary)
			{
				this.ButtonClicked(this);
			}
			else
			{
				this.SecondaryButtonClicked(this);
			}
		}

		public override string GetMouseCursorTexture()
		{
			if (base.IsMouseOver)
			{
				return m_styleDef.MouseOverCursor;
			}
			return "Textures\\GUI\\MouseCursor.dds";
		}

		public void RaiseButtonClicked()
		{
			this.ButtonClicked.InvokeIfNotNull(this);
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (BackgroundTexture == null)
			{
				base.MinSize = Vector2.Zero;
				base.MaxSize = Vector2.PositiveInfinity;
				base.Size = m_styleDef.SizeOverride ?? Vector2.Zero;
			}
			base.Position -= m_internalArea.Position / 2f;
			base.Draw(transitionAlpha, transitionAlpha);
			Vector4 textColor = m_textColor;
			if (!base.Enabled && DrawCrossTextureWhenDisabled)
			{
				MyGuiManager.DrawSpriteBatch("Textures\\GUI\\LockedButton.dds", GetPositionAbsolute(), base.Size * MyGuiConstants.LOCKBUTTON_SIZE_MODIFICATION, MyGuiConstants.DISABLED_BUTTON_COLOR, base.OriginAlign);
			}
			Color color = new Color(1f, 1f, 1f, transitionAlpha);
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			Vector2 topLeft = positionAbsoluteTopLeft + m_internalArea.Position;
			Vector2 size = m_internalArea.Size;
			if (Icon.HasValue)
			{
				Vector2 positionAbsoluteCenter = GetPositionAbsoluteCenter();
				MyGuiHighlightTexture value = Icon.Value;
				Vector2 vector = Vector2.Min(value.SizeGui, size) / value.SizeGui;
				float scale = Math.Min(vector.X, vector.Y);
				string texture = (base.HasHighlight ? value.Highlight : ((base.HasFocus && value.Focus != null) ? value.Focus : value.Normal));
				Vector2 normalizedSize = MyGuiManager.GetNormalizedSize(base.HasHighlight ? value.HighlightSize : ((base.HasFocus && value.Focus != null) ? value.FocusSize : value.NormalSize), scale) * IconScale;
				MyGuiManager.DrawSpriteBatch(texture, positionAbsoluteCenter, normalizedSize, color, IconOriginAlign, useFullClientArea: false, waitTillLoaded: false, null, IconRotation);
			}
			if (m_drawText.Length > 0 && TextScaleWithLanguage > 0f)
			{
				if (TextScale < MyGuiManager.MIN_TEXT_SCALE)
				{
					TextScale = MyGuiManager.MIN_TEXT_SCALE;
				}
				Vector2 size2 = base.Size;
				size2.X -= 0.009f;
				if (base.IsAutoScaleEnabled && TextScale != MyGuiManager.MIN_TEXT_SCALE)
				{
					TextScale = MyGuiControlAutoScaleText.GetScale(TextFont, m_drawText, size2, TextScale, MyGuiManager.MIN_TEXT_SCALE);
				}
				if (base.IsAutoEllipsisEnabled)
				{
					string textWithEllipsis = GetTextWithEllipsis(m_drawText, TextFont, TextScale, size2);
					if (textWithEllipsis != null)
					{
						if (m_toolTip == null)
						{
							m_toolTip = new MyToolTips(textWithEllipsis);
						}
						else if (m_toolTip.ToolTips == null)
						{
							m_toolTip = new MyToolTips(textWithEllipsis);
						}
<<<<<<< HEAD
						else if (m_toolTip.ToolTips.Count == 0)
=======
						else if (((Collection<MyColoredText>)(object)m_toolTip.ToolTips).Count == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							m_toolTip.AddToolTip(textWithEllipsis);
						}
					}
				}
				Vector2 coordAlignedFromTopLeft = MyUtils.GetCoordAlignedFromTopLeft(topLeft, m_internalArea.Size, TextAlignment);
				MyGuiManager.DrawString(TextFont, m_drawText.ToString(), coordAlignedFromTopLeft, TextScaleWithLanguage, MyGuiControlBase.ApplyColorMaskModifiers(textColor, base.Enabled, transitionAlpha), TextAlignment);
			}
			if (m_styleDef.Underline != null)
			{
				Vector2 normalizedCoord = positionAbsoluteTopLeft;
				normalizedCoord.Y += base.Size.Y;
				MyGuiManager.DrawSpriteBatch(normalizedSize: new Vector2(MyGuiManager.MeasureString(TextFont, m_drawText, TextScaleWithLanguage).X, MyGuiManager.GetNormalizedSizeFromScreenSize(new Vector2(0f, 2f)).Y), texture: base.HasHighlight ? m_styleDef.UnderlineHighlight : m_styleDef.Underline, normalizedCoord: normalizedCoord, color: color, drawAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
			base.Position += m_internalArea.Position / 2f;
		}

		private void DebugDraw()
		{
			MyGuiManager.DrawBorders(GetPositionAbsoluteTopLeft() + m_internalArea.Position, m_internalArea.Size, Color.White, 1);
		}

		private void UpdateText()
		{
			if (!string.IsNullOrEmpty(m_text))
			{
				m_drawText.Clear();
				m_drawText.Append(m_text);
			}
			else
			{
				m_drawText.Clear();
				m_drawText.Append(MyTexts.GetString(m_textEnum));
			}
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlButton obj = (MyObjectBuilder_GuiControlButton)base.GetObjectBuilder();
			obj.Text = Text;
			obj.TextEnum = m_textEnum.ToString();
			obj.TextScale = TextScale;
			obj.TextAlignment = (int)TextAlignment;
			obj.DrawCrossTextureWhenDisabled = DrawCrossTextureWhenDisabled;
			obj.VisualStyle = VisualStyle;
			return obj;
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_GuiControlButton myObjectBuilder_GuiControlButton = (MyObjectBuilder_GuiControlButton)objectBuilder;
			Text = myObjectBuilder_GuiControlButton.Text;
			m_textEnum = MyStringId.GetOrCompute(myObjectBuilder_GuiControlButton.TextEnum);
			TextScale = myObjectBuilder_GuiControlButton.TextScale;
			TextAlignment = (MyGuiDrawAlignEnum)myObjectBuilder_GuiControlButton.TextAlignment;
			DrawCrossTextureWhenDisabled = myObjectBuilder_GuiControlButton.DrawCrossTextureWhenDisabled;
			VisualStyle = myObjectBuilder_GuiControlButton.VisualStyle;
			UpdateText();
		}

		protected override bool ShouldHaveHighlight()
		{
			if (HighlightType == MyGuiControlHighlightType.CUSTOM)
			{
				return base.HasHighlight;
			}
			if (HighlightType == MyGuiControlHighlightType.FORCED)
			{
				return Selected;
			}
			return base.ShouldHaveHighlight();
		}

		protected override void OnHasHighlightChanged()
		{
			RefreshVisualStyle();
			base.OnHasHighlightChanged();
		}

		protected override void OnOriginAlignChanged()
		{
			base.OnOriginAlignChanged();
			RefreshInternals();
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshInternals();
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
			RefreshInternals();
		}

		private void RefreshInternals(bool forcedFocus = false)
		{
			base.ColorMask = m_styleDef.BackgroundColor;
			bool flag = base.Enabled && ((!m_activateOnMouseRelease && ReadyToClick) || ButtonPressedGamepad);
			bool flag2 = base.Enabled && (m_activateOnMouseRelease || !ReadyToClick) && !ButtonPressedGamepad;
			if (flag2 && base.HasHighlight)
			{
				BackgroundTexture = m_styleDef.HighlightTexture;
				TextFont = m_styleDef.HighlightFont;
<<<<<<< HEAD
				m_textColor = m_styleDef.TextColorHighlight ?? Vector4.One;
=======
				m_textColor = (m_styleDef.TextColorHighlight.HasValue ? m_styleDef.TextColorHighlight.Value : Vector4.One);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else if (flag2 && (base.HasFocus || forcedFocus))
			{
				BackgroundTexture = m_styleDef.FocusTexture ?? m_styleDef.HighlightTexture;
				TextFont = m_styleDef.HighlightFont;
<<<<<<< HEAD
				m_textColor = m_styleDef.TextColorFocus ?? Vector4.One;
=======
				m_textColor = (m_styleDef.TextColorFocus.HasValue ? m_styleDef.TextColorFocus.Value : Vector4.One);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else if (flag || Checked)
			{
				BackgroundTexture = m_styleDef.ActiveTexture ?? m_styleDef.HighlightTexture;
				TextFont = m_styleDef.HighlightFont;
<<<<<<< HEAD
				m_textColor = m_styleDef.TextColorActive ?? Vector4.One;
=======
				m_textColor = (m_styleDef.TextColorActive.HasValue ? m_styleDef.TextColorActive.Value : Vector4.One);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				BackgroundTexture = m_styleDef.NormalTexture;
				TextFont = m_styleDef.NormalFont;
<<<<<<< HEAD
				m_textColor = m_styleDef.TextColor ?? Vector4.One;
=======
				m_textColor = (m_styleDef.TextColor.HasValue ? m_styleDef.TextColor.Value : Vector4.One);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			Vector2 vector = base.Size;
			if (BackgroundTexture != null)
			{
				base.MinSize = BackgroundTexture.MinSizeGui;
				base.MaxSize = BackgroundTexture.MaxSizeGui;
				if (ButtonScale == 1f)
				{
					vector = m_styleDef.SizeOverride ?? vector;
				}
			}
			else
			{
				base.MinSize = Vector2.Zero;
				base.MaxSize = Vector2.PositiveInfinity;
				vector = m_styleDef.SizeOverride ?? Vector2.Zero;
			}
			if (vector == Vector2.Zero && m_drawText != null)
			{
				vector = MyGuiManager.MeasureString(TextFont, m_drawText, TextScaleWithLanguage);
			}
			MyGuiBorderThickness padding = m_styleDef.Padding;
			m_internalArea.Position = padding.TopLeftOffset;
			m_internalArea.Size = base.Size - padding.SizeChange;
			base.Size = vector;
		}

		public override void OnFocusChanged(bool focus)
		{
			base.OnFocusChanged(focus);
			RefreshInternals(focus);
		}
	}
}
