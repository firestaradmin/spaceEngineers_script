using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlTextbox))]
	public class MyGuiControlTextbox : MyGuiControlBase, IMyImeActiveControl
	{
		public class StyleDefinition
		{
			public string NormalFont;

			public string HighlightFont;

			public MyGuiCompositeTexture NormalTexture;

			public MyGuiCompositeTexture HighlightTexture;

			public MyGuiCompositeTexture FocusTexture;

			public Vector4? TextColorHighlight;

			public Vector4? TextColorFocus;

			public Vector4? TextColor;
		}

		public struct MySkipCombination
		{
			public bool Alt;

			public bool Ctrl;

			public bool Shift;

			public MyKeys[] Keys;
		}

		private class MyGuiControlTextboxSelection
		{
			private int m_startIndex;

			private int m_endIndex;

			private string ClipboardText;

			private bool m_dragging;

			public bool Dragging
			{
				get
				{
					return m_dragging;
				}
				set
				{
					m_dragging = value;
				}
			}

			public int Start => Math.Min(m_startIndex, m_endIndex);

			public int End => Math.Max(m_startIndex, m_endIndex);

			public int Length => End - Start;

			public MyGuiControlTextboxSelection()
			{
				m_startIndex = 0;
				m_endIndex = 0;
			}

			public void SetEnd(MyGuiControlTextbox sender)
			{
				m_endIndex = MathHelper.Clamp(sender.CarriagePositionIndex, 0, sender.m_text.Length);
			}

			public void Reset(MyGuiControlTextbox sender)
			{
				m_startIndex = (m_endIndex = MathHelper.Clamp(sender.CarriagePositionIndex, 0, sender.m_text.Length));
			}

			public void SelectAll(MyGuiControlTextbox sender)
			{
				m_startIndex = 0;
				m_endIndex = sender.m_text.Length;
				sender.CarriagePositionIndex = sender.m_text.Length;
			}

			public void EraseText(MyGuiControlTextbox sender)
			{
				if (Start != End)
				{
					string text = sender.m_text.ToString();
					StringBuilder stringBuilder = new StringBuilder(text.Substring(0, Start));
					StringBuilder value = new StringBuilder(text.Substring(End));
					sender.CarriagePositionIndex = Start;
					sender.SetText(stringBuilder.Append((object)value));
				}
			}

			public void CutText(MyGuiControlTextbox sender)
			{
				CopyText(sender);
				EraseText(sender);
			}

			public void CopyText(MyGuiControlTextbox sender)
			{
				ClipboardText = sender.m_text.ToString().Substring(Start, Length);
				if (!string.IsNullOrEmpty(ClipboardText))
				{
					MyVRage.Platform.System.Clipboard = ClipboardText;
				}
			}

			public void PasteText(MyGuiControlTextbox sender)
			{
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				EraseText(sender);
				string text = sender.m_text.ToString();
				string text2 = text.Substring(0, sender.CarriagePositionIndex);
				string value = text.Substring(sender.CarriagePositionIndex);
<<<<<<< HEAD
				Thread thread = new Thread(PasteFromClipboard);
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				thread.Join();
=======
				Thread val = new Thread((ThreadStart)PasteFromClipboard);
				val.set_ApartmentState(ApartmentState.STA);
				val.Start();
				val.Join();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				string text3 = ClipboardText.Replace("\n", "");
				string text4;
				if (text3.Length + text.Length <= sender.MaxLength)
				{
					text4 = text3;
				}
				else
				{
					int num = sender.MaxLength - text.Length;
					text4 = ((num <= 0) ? "" : text3.Substring(0, num));
				}
				sender.SetText(new StringBuilder(text2).Append(text4).Append(value));
				sender.CarriagePositionIndex = text2.Length + text4.Length;
				Reset(sender);
			}

			private void PasteFromClipboard()
			{
				string clipboard = MyVRage.Platform.System.Clipboard;
				for (int i = 0; i < clipboard.Length; i++)
				{
					if (!XmlConvert.IsXmlChar(clipboard[i]))
					{
						ClipboardText = string.Empty;
						return;
					}
				}
				ClipboardText = clipboard;
			}
		}

		private static StyleDefinition[] m_styles;

		private int m_carriageBlinkerTimer;

		private int m_carriagePositionIndex;

		private bool m_formattedAlready;

		private int m_maxLength;

		private List<MyKeys> m_pressedKeys = new List<MyKeys>(10);

		private Vector4? m_textColorOverride;

		private Vector4 m_textColor;

		private float m_textScale;

		private float m_textScaleWithLanguage;

		private bool m_hadFocusLastTime;

		private float m_slidingWindowOffset;

		private MyRectangle2D m_textAreaRelative;

		private MyGuiCompositeTexture m_compositeBackground;

		private StringBuilder m_text = new StringBuilder();

		private StringBuilder m_textValid = new StringBuilder();

		private bool m_textIsValid = true;

		private MyGuiControlTextboxSelection m_selection = new MyGuiControlTextboxSelection();

		private bool m_isImeActive;

		private MyToolTips m_originalToolTip;

		private bool m_enableJoystickTextPaste;

<<<<<<< HEAD
		private decimal m_minNumericValue;

		private decimal m_maxNumericValue;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyGuiControlTextboxType Type;

		public bool TruncateDecimalDigits = true;

		private MyGuiControlTextboxStyleEnum m_visualStyle;

		private StyleDefinition m_styleDef;

		private StyleDefinition m_customStyle;

		private bool m_useCustomStyle;

		private static MyKeyThrottler m_keyThrottler;

		private string m_virtualKeyboardPendingData;

		public bool TextIsOffensive => !m_textIsValid;

		public bool IsImeActive
		{
			get
			{
				return m_isImeActive;
			}
			set
			{
				m_isImeActive = value;
			}
		}

		public int MaxLength
		{
			get
			{
				return m_maxLength;
			}
			set
			{
				m_maxLength = value;
				if (m_text.Length > m_maxLength)
				{
					m_text.Remove(m_maxLength, m_text.Length - m_maxLength);
				}
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

		/// <summary>
		/// When setting text to textbox, make sure you won't set it to unsuported charact
		/// </summary>
		public string Text
		{
			get
			{
				return m_textValid.ToString();
			}
			set
			{
				m_text.Clear().Append(value);
				if (CarriagePositionIndex >= m_text.Length)
				{
					CarriagePositionIndex = m_text.Length;
				}
				OnTextChangedInternal();
			}
		}

		public TextAlingmentMode TextAlignment { get; set; }

		public MyGuiControlTextboxStyleEnum VisualStyle
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

		public int CarriagePositionIndex
		{
			get
			{
				return m_carriagePositionIndex;
			}
			private set
			{
				m_carriagePositionIndex = MathHelper.Clamp(value, 0, m_text.Length);
			}
		}

		public MySkipCombination[] SkipCombinations { get; set; }

		public string TextFont { get; private set; }

		public event Action<MyGuiControlTextbox> TextChanged;

		public event Action<MyGuiControlTextbox> EnterPressed;

		static MyGuiControlTextbox()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlTextboxStyleEnum>() + 1];
			m_styles[0] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_TEXTBOX,
				HighlightTexture = MyGuiConstants.TEXTURE_TEXTBOX_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_TEXTBOX_FOCUS,
				NormalFont = "Blue",
				HighlightFont = "White",
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_styles[1] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_TEXTBOX,
				HighlightTexture = MyGuiConstants.TEXTURE_TEXTBOX_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_TEXTBOX_FOCUS,
				NormalFont = "Debug",
				HighlightFont = "Debug",
				TextColorFocus = MyGuiConstants.HIGHLIGHT_TEXT_COLOR
			};
			m_keyThrottler = new MyKeyThrottler();
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlTextboxStyleEnum style)
		{
			return m_styles[(int)style];
		}

		private void OnTextChangedInternal()
		{
			RefreshSlidingWindow();
			m_selection.Reset(this);
			DelayCaretBlink();
			string text = MyScreenManager.ValidateText(m_text);
			bool textIsValid = m_textIsValid;
			m_textIsValid = text == null;
			if (m_textIsValid)
			{
				if (!textIsValid)
				{
					m_toolTip = m_originalToolTip;
				}
				m_textValid.Clear().Append((object)m_text);
				OnTextChanged();
			}
			else
			{
				if (textIsValid)
				{
					m_originalToolTip = m_toolTip;
				}
				m_toolTip = new MyToolTips();
				m_toolTip.AddToolTip(string.Format(MyTexts.Get(MyCommonTexts.OffensiveTextTooltip).ToString(), text), 0.7f, "Red");
			}
		}

		public bool TextEquals(StringBuilder text)
		{
			return m_text.CompareTo(text) == 0;
		}

		public void GetText(StringBuilder result)
		{
			result.AppendStringBuilder(m_textValid);
		}

		/// <summary>
		/// Copies string from source to internal string builder
		/// </summary>
		public void SetText(StringBuilder source)
		{
			m_text.Clear().AppendStringBuilder(source);
			if (CarriagePositionIndex >= m_text.Length)
			{
				CarriagePositionIndex = m_text.Length;
			}
			OnTextChangedInternal();
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

		private void RefreshInternals()
		{
			if (base.HasHighlight)
			{
				m_compositeBackground = m_styleDef.HighlightTexture;
				base.MinSize = m_compositeBackground.MinSizeGui * TextScale;
				base.MaxSize = m_compositeBackground.MaxSizeGui * TextScale;
				TextFont = m_styleDef.HighlightFont;
				m_textColor = (m_styleDef.TextColorHighlight.HasValue ? m_styleDef.TextColorHighlight.Value : Vector4.One);
			}
			else if (base.HasFocus)
			{
				m_compositeBackground = m_styleDef.FocusTexture ?? m_styleDef.HighlightTexture;
				base.MinSize = m_compositeBackground.MinSizeGui * TextScale;
				base.MaxSize = m_compositeBackground.MaxSizeGui * TextScale;
				TextFont = m_styleDef.HighlightFont;
				m_textColor = (m_styleDef.TextColorFocus.HasValue ? m_styleDef.TextColorFocus.Value : Vector4.One);
			}
			else
			{
				m_compositeBackground = m_styleDef.NormalTexture;
				base.MinSize = m_compositeBackground.MinSizeGui * TextScale;
				base.MaxSize = m_compositeBackground.MaxSizeGui * TextScale;
				TextFont = m_styleDef.NormalFont;
				m_textColor = (m_styleDef.TextColor.HasValue ? m_styleDef.TextColor.Value : Vector4.One);
			}
			RefreshTextArea();
		}

		public MyGuiControlTextbox()
<<<<<<< HEAD
			: this(null, null, 512, null, 0.8f, MyGuiControlTextboxType.Normal, MyGuiControlTextboxStyleEnum.Default, enableJoystickTextPaste: false, decimal.MinValue, decimal.MaxValue)
		{
		}

		public MyGuiControlTextbox(Vector2? position = null, string defaultText = null, int maxLength = 512, Vector4? textColor = null, float textScale = 0.8f, MyGuiControlTextboxType type = MyGuiControlTextboxType.Normal, MyGuiControlTextboxStyleEnum visualStyle = MyGuiControlTextboxStyleEnum.Default, bool enableJoystickTextPaste = false, decimal minNumericValue = decimal.MinValue, decimal maxNumericValue = decimal.MaxValue)
=======
			: this(null, null, 512, null, 0.8f, MyGuiControlTextboxType.Normal, MyGuiControlTextboxStyleEnum.Default, enableJoystickTextPaste: false)
		{
		}

		public MyGuiControlTextbox(Vector2? position = null, string defaultText = null, int maxLength = 512, Vector4? textColor = null, float textScale = 0.8f, MyGuiControlTextboxType type = MyGuiControlTextboxType.Normal, MyGuiControlTextboxStyleEnum visualStyle = MyGuiControlTextboxStyleEnum.Default, bool enableJoystickTextPaste = false)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			: base(position, new Vector2(512f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, null, null, null, isActiveControl: true, canHaveFocus: true)
		{
			base.Name = "Textbox";
			Type = type;
			m_carriagePositionIndex = 0;
			m_carriageBlinkerTimer = 0;
			m_textColorOverride = textColor;
			m_textColor = Vector4.One;
			TextScale = textScale;
			TextAlignment = TextAlingmentMode.Left;
			m_maxLength = maxLength;
<<<<<<< HEAD
			m_minNumericValue = minNumericValue;
			m_maxNumericValue = maxNumericValue;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Text = defaultText ?? "";
			m_visualStyle = visualStyle;
			RefreshVisualStyle();
			m_slidingWindowOffset = 0f;
			base.GamepadHelpTextId = MyCommonTexts.Gamepad_Help_Textbox;
			m_enableJoystickTextPaste = enableJoystickTextPaste;
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			m_slidingWindowOffset = 0f;
			m_carriagePositionIndex = 0;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			if (!base.Visible)
			{
				return;
			}
			m_compositeBackground.Draw(GetPositionAbsoluteTopLeft(), base.Size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			MyRectangle2D textAreaRelative = m_textAreaRelative;
			textAreaRelative.LeftTop += GetPositionAbsoluteTopLeft();
			float num = GetCarriageOffset(CarriagePositionIndex);
			RectangleF normalizedRectangle = new RectangleF(textAreaRelative.LeftTop, new Vector2(textAreaRelative.Size.X, textAreaRelative.Size.Y * 2f));
			using (MyGuiManager.UsingScissorRectangle(ref normalizedRectangle))
			{
				RefreshSlidingWindow();
				if (m_selection.Length > 0)
				{
					float num2 = GetCarriageOffset(m_selection.End) - GetCarriageOffset(m_selection.Start);
					MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", new Vector2(textAreaRelative.LeftTop.X + GetCarriageOffset(m_selection.Start), textAreaRelative.LeftTop.Y), new Vector2(num2 + 0.002f, textAreaRelative.Size.Y * 1.38f), MyGuiControlBase.ApplyColorMaskModifiers(new Vector4(1f, 1f, 1f, 0.5f), base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				}
				StringBuilder modifiedText = GetModifiedText();
				Vector2 vector = MyGuiManager.MeasureString(TextFont, modifiedText, TextScaleWithLanguage);
				Vector2 normalizedCoord = new Vector2(textAreaRelative.LeftTop.X + m_slidingWindowOffset, textAreaRelative.LeftTop.Y);
				if (TextAlignment == TextAlingmentMode.Right)
				{
					normalizedCoord = new Vector2(textAreaRelative.LeftTop.X + m_slidingWindowOffset + textAreaRelative.Size.X - vector.X, textAreaRelative.LeftTop.Y);
				}
				MyGuiManager.DrawString(TextFont, modifiedText.ToString(), normalizedCoord, TextScaleWithLanguage, MyGuiControlBase.ApplyColorMaskModifiers((!m_textIsValid) ? Color.Red.ToVector4() : (m_textColorOverride ?? m_textColor), base.Enabled, transitionAlpha));
				if (base.HasFocus)
				{
					int num3 = m_carriageBlinkerTimer % 60;
					if (num3 >= 0 && num3 <= 45)
					{
						if (TextAlignment == TextAlingmentMode.Left && CarriagePositionIndex == 0)
						{
							num += 0.0005f;
<<<<<<< HEAD
						}
						else if (TextAlignment == TextAlingmentMode.Right && (CarriagePositionIndex == 0 || CarriagePositionIndex == m_text.Length))
						{
							num -= 0.0005f;
						}
=======
						}
						else if (TextAlignment == TextAlingmentMode.Right && (CarriagePositionIndex == 0 || CarriagePositionIndex == m_text.Length))
						{
							num -= 0.0005f;
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						Vector2 normalizedCoord2 = new Vector2(textAreaRelative.LeftTop.X + num, GetPositionAbsoluteTopLeft().Y);
						MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", normalizedCoord2, 1, base.Size.Y, MyGuiControlBase.ApplyColorMaskModifiers(Vector4.One, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					}
				}
				m_carriageBlinkerTimer++;
			}
		}

		private void DebugDraw()
		{
			MyRectangle2D textAreaRelative = m_textAreaRelative;
			textAreaRelative.LeftTop += GetPositionAbsoluteTopLeft();
			MyGuiManager.DrawBorders(textAreaRelative.LeftTop, textAreaRelative.Size, Color.White, 1);
		}

		/// <summary>
		/// gets the position of the first space to the left of the carriage or 0 if there isn't any
		/// </summary>
		/// <returns></returns>
		private int GetPreviousSpace()
		{
			if (CarriagePositionIndex == 0)
			{
				return 0;
			}
			int num = m_text.ToString().Substring(0, CarriagePositionIndex).LastIndexOf(" ");
			if (num == -1)
			{
				return 0;
			}
			return num;
		}

		/// <summary>
		/// gets the position of the first space to the right of the carriage or the text length if there isn't any
		/// </summary>
		/// <returns></returns>
		private int GetNextSpace()
		{
			if (CarriagePositionIndex == m_text.Length)
			{
				return m_text.Length;
			}
			int num = m_text.ToString().Substring(CarriagePositionIndex + 1).IndexOf(" ");
			if (num == -1)
			{
				return m_text.Length;
			}
			return CarriagePositionIndex + num + 1;
		}

		/// <summary>
		/// Method returns true if input was captured by control, so no other controls, nor screen can use input in this update
		/// </summary>
		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase ret = base.HandleInput();
			try
			{
				HandleVirtualKeyboardInput();
				if (ret == null && base.Enabled)
				{
					if (MyInput.Static.IsNewLeftMousePressed())
					{
						if (base.IsMouseOver)
						{
							if (MyVRage.Platform.ImeProcessor != null)
							{
								MyVRage.Platform.ImeProcessor.CaretRepositionReaction();
							}
							m_selection.Dragging = true;
							CarriagePositionIndex = GetCarriagePositionFromMouseCursor();
							if (MyInput.Static.IsAnyShiftKeyPressed())
							{
								m_selection.SetEnd(this);
							}
							else
							{
								m_selection.Reset(this);
							}
							ret = this;
						}
						else
						{
							m_selection.Reset(this);
						}
					}
					else if (MyInput.Static.IsNewLeftMouseReleased())
					{
						m_selection.Dragging = false;
					}
					else if (m_selection.Dragging)
					{
						if (MyVRage.Platform.ImeProcessor != null)
						{
							MyVRage.Platform.ImeProcessor.CaretRepositionReaction();
						}
						CarriagePositionIndex = GetCarriagePositionFromMouseCursor();
						m_selection.SetEnd(this);
						ret = this;
					}
					if (base.HasFocus)
					{
						if (!MyInput.Static.IsAnyCtrlKeyPressed())
						{
							HandleTextInputBuffered(ref ret);
						}
						if (m_keyThrottler.GetKeyStatus(MyKeys.Left) == ThrottledKeyStatus.PRESSED_AND_READY)
						{
							ret = this;
							if (!m_isImeActive)
							{
								if (MyInput.Static.IsAnyCtrlKeyPressed())
								{
									CarriagePositionIndex = GetPreviousSpace();
								}
								else
								{
									CarriagePositionIndex--;
								}
								if (MyInput.Static.IsAnyShiftKeyPressed())
								{
									m_selection.SetEnd(this);
								}
								else
								{
									m_selection.Reset(this);
								}
								DelayCaretBlink();
							}
						}
						if (m_keyThrottler.GetKeyStatus(MyKeys.Right) == ThrottledKeyStatus.PRESSED_AND_READY)
						{
							ret = this;
							if (!m_isImeActive)
							{
								if (MyInput.Static.IsAnyCtrlKeyPressed())
								{
									CarriagePositionIndex = GetNextSpace();
								}
								else
								{
									CarriagePositionIndex++;
								}
								if (MyInput.Static.IsAnyShiftKeyPressed())
								{
									m_selection.SetEnd(this);
								}
								else
								{
									m_selection.Reset(this);
								}
								DelayCaretBlink();
							}
						}
						if (m_keyThrottler.GetKeyStatus(MyKeys.Back) == ThrottledKeyStatus.PRESSED_AND_READY && MyInput.Static.IsAnyCtrlKeyPressed() && !m_isImeActive)
						{
							ret = this;
							CarriagePositionIndex = GetPreviousSpace();
							m_selection.SetEnd(this);
							m_selection.EraseText(this);
						}
						if (m_keyThrottler.GetKeyStatus(MyKeys.Delete) == ThrottledKeyStatus.PRESSED_AND_READY && MyInput.Static.IsAnyCtrlKeyPressed() && !m_isImeActive)
						{
							ret = this;
							CarriagePositionIndex = GetNextSpace();
							m_selection.SetEnd(this);
							m_selection.EraseText(this);
						}
						if (!IsImeActive)
						{
							if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.Home))
							{
								CarriagePositionIndex = 0;
								if (MyInput.Static.IsAnyShiftKeyPressed())
								{
									m_selection.SetEnd(this);
								}
								else
								{
									m_selection.Reset(this);
								}
								ret = this;
								DelayCaretBlink();
							}
							if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.End))
							{
								CarriagePositionIndex = m_text.Length;
								if (MyInput.Static.IsAnyShiftKeyPressed())
								{
									m_selection.SetEnd(this);
								}
								else
								{
									m_selection.Reset(this);
								}
								ret = this;
								DelayCaretBlink();
							}
							if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.X) && MyInput.Static.IsAnyCtrlKeyPressed())
							{
								m_selection.CutText(this);
							}
							if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.C) && MyInput.Static.IsAnyCtrlKeyPressed())
							{
								m_selection.CopyText(this);
							}
							if ((m_keyThrottler.IsNewPressAndThrottled(MyKeys.V) && MyInput.Static.IsAnyCtrlKeyPressed()) || (m_enableJoystickTextPaste && MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J10)))
							{
								m_selection.PasteText(this);
							}
							if (m_keyThrottler.IsNewPressAndThrottled(MyKeys.A) && MyInput.Static.IsAnyCtrlKeyPressed())
							{
								m_selection.SelectAll(this);
							}
							if (MyInput.Static.IsNewKeyPressed(MyKeys.Enter))
							{
								this.EnterPressed.InvokeIfNotNull(this);
							}
							if (MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01))
							{
								ret = this;
								MyVRage.Platform.Input2.ShowVirtualKeyboardIfNeeded(OnVirtualKeyboardDataReceived, null, m_text.ToString(), null, m_maxLength);
							}
						}
						m_formattedAlready = false;
					}
					else if (Type == MyGuiControlTextboxType.DigitsOnly && !m_formattedAlready && m_text.Length != 0)
					{
<<<<<<< HEAD
						decimal num = MyValueFormatter.GetDecimalFromString(m_text.ToString(), 1);
						if (num < m_minNumericValue)
						{
							num = m_minNumericValue;
						}
						if (num > m_maxNumericValue)
						{
							num = m_maxNumericValue;
						}
						int decimalDigits = ((!TruncateDecimalDigits || num - decimal.Truncate(num) > 0m) ? 1 : 0);
						m_text.Clear().Append(MyValueFormatter.GetFormatedFloat((float)num, decimalDigits, ""));
=======
						decimal decimalFromString = MyValueFormatter.GetDecimalFromString(m_text.ToString(), 1);
						int decimalDigits = ((!TruncateDecimalDigits || decimalFromString - decimal.Truncate(decimalFromString) > 0m) ? 1 : 0);
						m_text.Clear().Append(MyValueFormatter.GetFormatedFloat((float)decimalFromString, decimalDigits, ""));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						CarriagePositionIndex = m_text.Length;
						m_formattedAlready = true;
					}
				}
			}
			catch (IndexOutOfRangeException)
			{
			}
			m_hadFocusLastTime = base.HasFocus;
			return ret;
		}

		public void PasteTextFromClipboard()
		{
			m_selection.PasteText(this);
		}

		private void OnVirtualKeyboardDataReceived(string text)
		{
			m_virtualKeyboardPendingData = text;
		}

		private void HandleVirtualKeyboardInput()
		{
			string text = Interlocked.Exchange(ref m_virtualKeyboardPendingData, null);
			if (text != null)
			{
				if (!m_text.EqualsStrFast(text))
				{
					m_text.Clear().Append(text);
					CarriagePositionIndex = m_text.Length;
					m_selection.Reset(this);
					OnTextChangedInternal();
				}
				this.EnterPressed.InvokeIfNotNull(this);
			}
		}

		public bool IsSkipCharacter(MyKeys character)
		{
			if (SkipCombinations != null)
			{
				MySkipCombination[] skipCombinations = SkipCombinations;
				for (int i = 0; i < skipCombinations.Length; i++)
				{
					MySkipCombination mySkipCombination = skipCombinations[i];
					if (mySkipCombination.Alt == MyInput.Static.IsAnyAltKeyPressed() && mySkipCombination.Ctrl == MyInput.Static.IsAnyCtrlKeyPressed() && mySkipCombination.Shift == MyInput.Static.IsAnyShiftKeyPressed() && (mySkipCombination.Keys == null || mySkipCombination.Keys.Contains(character)))
					{
						return true;
					}
				}
			}
			return false;
		}

		private void HandleTextInputBuffered(ref MyGuiControlBase ret)
		{
			bool flag = false;
			foreach (char item in MyInput.Static.TextInput)
			{
				if (IsSkipCharacter((MyKeys)item))
				{
					continue;
				}
				if (char.IsControl(item))
				{
					if (item == '\b')
					{
						KeypressBackspaceInternal(compositionEnd: true);
						flag = true;
					}
					continue;
<<<<<<< HEAD
				}
				if (m_selection.Length > 0)
				{
					m_selection.EraseText(this);
				}
=======
				}
				if (m_selection.Length > 0)
				{
					m_selection.EraseText(this);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				InsertCharInternal(conpositionEnd: true, item);
				flag = true;
			}
			if (m_keyThrottler.GetKeyStatus(MyKeys.Delete) == ThrottledKeyStatus.PRESSED_AND_READY)
			{
				KeypressDeleteInternal(compositionEnd: true);
				flag = true;
			}
			if (flag)
			{
				OnTextChangedInternal();
				ret = this;
			}
		}

		public void InsertChar(bool conpositionEnd, char character)
		{
			InsertCharInternal(conpositionEnd, character);
			OnTextChangedInternal();
		}

		private void InsertCharInternal(bool conpositionEnd, char character)
		{
			if (m_selection.Length > 0)
			{
				m_selection.EraseText(this);
			}
			if (m_text.Length < m_maxLength)
			{
				m_text.Insert(CarriagePositionIndex, character);
				CarriagePositionIndex++;
			}
		}

		public void InsertCharMultiple(bool conpositionEnd, string chars)
		{
			if (m_selection.Length > 0)
			{
				m_selection.EraseText(this);
			}
			if (m_text.Length + chars.Length <= m_maxLength)
			{
				m_text.Insert(CarriagePositionIndex, chars);
				CarriagePositionIndex += chars.Length;
			}
			OnTextChangedInternal();
		}

		public void KeypressBackspace(bool compositionEnd)
		{
			KeypressBackspaceInternal(compositionEnd);
			OnTextChangedInternal();
		}

		private void KeypressBackspaceInternal(bool compositionEnd)
		{
			if (m_selection.Length == 0)
			{
				ApplyBackspace();
			}
			else
			{
				m_selection.EraseText(this);
			}
		}

		public void KeypressBackspaceMultiple(bool conpositionEnd, int count)
		{
			if (m_selection.Length == 0)
			{
				ApplyBackspaceMultiple(count);
			}
			else
			{
				m_selection.EraseText(this);
			}
			OnTextChangedInternal();
		}

		public void KeypressDelete(bool compositionEnd)
		{
			KeypressDeleteInternal(compositionEnd);
			OnTextChangedInternal();
		}

		private void KeypressDeleteInternal(bool compositionEnd)
		{
			if (m_selection.Length == 0)
			{
				ApplyDelete();
			}
			else
			{
				m_selection.EraseText(this);
			}
		}

		private void ApplyBackspace()
		{
			if (CarriagePositionIndex > 0)
			{
				CarriagePositionIndex--;
				m_text.Remove(CarriagePositionIndex, 1);
			}
		}

		private void ApplyBackspaceMultiple(int count)
		{
			if (CarriagePositionIndex >= count)
			{
				CarriagePositionIndex -= count;
				m_text.Remove(CarriagePositionIndex, count);
			}
		}

		private void ApplyDelete()
		{
			if (CarriagePositionIndex < m_text.Length)
			{
				m_text.Remove(CarriagePositionIndex, 1);
			}
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			RefreshInternals();
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshTextArea();
			RefreshSlidingWindow();
		}

		public void FocusEnded()
		{
			OnFocusChanged(focus: false);
		}

		public override void OnFocusChanged(bool focus)
		{
			if (focus)
			{
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Tab))
				{
					MoveCarriageToEnd();
					m_selection.SelectAll(this);
				}
				if (MyVRage.Platform.ImeProcessor != null)
				{
					MyVRage.Platform.ImeProcessor.Activate(this);
				}
			}
			else
			{
				m_selection.Reset(this);
				if (MyVRage.Platform.ImeProcessor != null)
				{
					MyVRage.Platform.ImeProcessor.Deactivate();
				}
			}
			RefreshInternals();
			base.OnFocusChanged(focus);
		}

		public void MoveCarriageToEnd()
		{
			CarriagePositionIndex = m_text.Length;
		}

		public void ResetPosition()
		{
			m_slidingWindowOffset = 0f;
		}

		/// <summary>
		/// Converts carriage (or just char) position to normalized coordinates
		/// </summary>
		public float GetCarriageOffset(int index)
		{
			string value = GetModifiedText().ToString().Substring(0, index);
			Vector2 vector = MyGuiManager.MeasureString("Blue", new StringBuilder(value), TextScaleWithLanguage);
			if (TextAlignment == TextAlingmentMode.Left)
			{
				return vector.X + m_slidingWindowOffset;
			}
			StringBuilder modifiedText = GetModifiedText();
			Vector2 vector2 = MyGuiManager.MeasureString("Blue", modifiedText, TextScaleWithLanguage);
			return vector.X + m_slidingWindowOffset + m_textAreaRelative.Size.X - vector2.X;
		}

		/// <summary>
		/// After user clicks on textbox, we will try to set carriage position where the cursor is
		/// </summary>
		private int GetCarriagePositionFromMouseCursor()
		{
			RefreshSlidingWindow();
			float num = MyGuiManager.MouseCursorPosition.X - GetPositionAbsoluteTopLeft().X - m_textAreaRelative.LeftTop.X;
			int result = 0;
			float num2 = float.MaxValue;
			for (int i = 0; i <= m_text.Length; i++)
			{
				float carriageOffset = GetCarriageOffset(i);
				float num3 = Math.Abs(num - carriageOffset);
				if (num3 < num2)
				{
					num2 = num3;
					result = i;
				}
			}
			return result;
		}

		private void RefreshTextArea()
		{
			m_textAreaRelative = new MyRectangle2D(MyGuiConstants.TEXTBOX_TEXT_OFFSET, base.Size - 2f * MyGuiConstants.TEXTBOX_TEXT_OFFSET);
		}

<<<<<<< HEAD
		/// <summary>
		/// If type of textbox is password, this method returns asterisk. Otherwise original text.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private StringBuilder GetModifiedText()
		{
			switch (Type)
			{
			case MyGuiControlTextboxType.Normal:
			case MyGuiControlTextboxType.DigitsOnly:
				return m_text;
			case MyGuiControlTextboxType.Password:
				return new StringBuilder(new string('*', m_text.Length));
			default:
				return m_text;
			}
		}

		private void OnTextChanged()
		{
			if (this.TextChanged != null)
			{
				this.TextChanged(this);
			}
		}

		private void DelayCaretBlink()
		{
			m_carriageBlinkerTimer = 0;
		}

		/// <summary>
		/// If carriage is in current sliding window, then we don't change it. If it's over its left or right borders, we move sliding window.
		/// Of course on on X axis, Y is ignored at all.
		/// This method could be called from Update() or Draw() - it doesn't matter now
		/// </summary>
		private void RefreshSlidingWindow()
		{
			float carriageOffset = GetCarriageOffset(CarriagePositionIndex);
			MyRectangle2D textAreaRelative = m_textAreaRelative;
			if (carriageOffset < 0f)
			{
				m_slidingWindowOffset -= carriageOffset;
			}
			else if (carriageOffset > textAreaRelative.Size.X)
			{
				m_slidingWindowOffset -= carriageOffset - textAreaRelative.Size.X;
			}
		}

		/// <summary>
		/// GR: Use this to select all text outside for current textbox.
		/// </summary>
		public void SelectAll()
		{
			if (m_selection != null)
			{
				m_selection.SelectAll(this);
			}
		}

		public void ApplyStyle(StyleDefinition style)
		{
			m_useCustomStyle = true;
			m_customStyle = style;
			RefreshVisualStyle();
		}

		public Vector2 GetCornerPosition()
		{
			return GetPositionAbsoluteBottomLeft();
		}

		public Vector2 GetCarriagePosition(int shiftX)
		{
			int num = m_text.Length - shiftX;
			Vector2 result = new Vector2(GetCarriageOffset((num >= 0) ? num : 0), 0f);
			result.X += 0.009f;
			return result;
		}

		public void KeypressEnter(bool compositionEnd)
		{
			if (this.EnterPressed != null)
			{
				this.EnterPressed(this);
			}
		}

		public void DeactivateIme()
		{
			m_isImeActive = false;
		}

		public void KeypressRedo()
		{
		}

		public void KeypressUndo()
		{
		}

		public int GetMaxLength()
		{
			return m_maxLength;
		}

		public int GetSelectionLength()
		{
			if (m_selection == null)
			{
				return 0;
			}
			return m_selection.Length;
		}

		public int GetTextLength()
		{
			return m_textValid.Length;
		}
	}
}
