using System;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlSliderBase : MyGuiControlBase
	{
		public class StyleDefinition
		{
			public MyGuiCompositeTexture RailTexture;

			public MyGuiCompositeTexture RailHighlightTexture;

			public MyGuiCompositeTexture RailFocusTexture;

			public MyGuiHighlightTexture ThumbTexture;
		}

		private static StyleDefinition[] m_styles;

		public Action<MyGuiControlSliderBase> ValueChanged;

		private bool m_controlCaptured;

		private string m_thumbTexture;

		private MyGuiControlLabel m_label;

		private bool showLabel = true;

		private MyGuiCompositeTexture m_railTexture;

		private float m_labelSpaceWidth;

		private float m_debugScale = 1f;

		public float? DefaultRatio;

		private MyGuiControlSliderStyleEnum m_visualStyle;

		private StyleDefinition m_styleDef;

		private MyGuiSliderProperties m_props;

		private float m_ratio;

		public Func<MyGuiControlSliderBase, bool> SliderClicked;

		private int m_lastTimeArrowPressed;

		private bool m_lastMoveDirection;

		private float m_consecutiveSliderMoves;

		public MyGuiControlSliderStyleEnum VisualStyle
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

		/// Control properties of the slider.
		public MyGuiSliderProperties Propeties
		{
			get
			{
				return m_props;
			}
			set
			{
				m_props = value;
				Ratio = m_ratio;
			}
		}

		/// <summary>
		/// This is values selected on slider in original units, e.g. meters, so it can be for example 1000 meters.
		/// </summary>
		public float Ratio
		{
			get
			{
				return m_ratio;
			}
			set
			{
				value = MathHelper.Clamp(value, 0f, 1f);
				if (m_ratio != value)
				{
					m_ratio = m_props.RatioFilter(value);
					UpdateLabel();
					OnValueChange();
				}
			}
		}

		protected virtual float MinimumStep => 0f;

		public float DebugScale
		{
			get
			{
				return m_debugScale;
			}
			set
			{
				if (m_debugScale != value)
				{
					m_debugScale = value;
					RefreshInternals();
				}
			}
		}

		public float Value
		{
			get
			{
				return m_props.RatioToValue(m_ratio);
			}
			set
			{
				float arg = m_props.ValueToRatio(value);
				arg = m_props.RatioFilter(arg);
				arg = MathHelper.Clamp(arg, 0f, 1f);
				if (arg != m_ratio)
				{
					m_ratio = arg;
					UpdateLabel();
					OnValueChange();
				}
			}
		}

		static MyGuiControlSliderBase()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlSliderStyleEnum>() + 1];
			m_styles[0] = new StyleDefinition
			{
				RailTexture = MyGuiConstants.TEXTURE_SLIDER_RAIL,
				RailHighlightTexture = MyGuiConstants.TEXTURE_SLIDER_RAIL_HIGHLIGHT,
				RailFocusTexture = MyGuiConstants.TEXTURE_SLIDER_RAIL_FOCUS,
				ThumbTexture = MyGuiConstants.TEXTURE_SLIDER_THUMB_DEFAULT
			};
			m_styles[1] = new StyleDefinition
			{
				RailTexture = MyGuiConstants.TEXTURE_HUE_SLIDER_RAIL,
				RailHighlightTexture = MyGuiConstants.TEXTURE_HUE_SLIDER_RAIL_HIGHLIGHT,
				RailFocusTexture = MyGuiConstants.TEXTURE_HUE_SLIDER_RAIL_FOCUS,
				ThumbTexture = MyGuiConstants.TEXTURE_HUE_SLIDER_THUMB_DEFAULT
			};
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlSliderStyleEnum style)
		{
			return m_styles[(int)style];
		}

		protected virtual void OnValueChange()
		{
			ValueChanged.InvokeIfNotNull(this);
		}

		public MyGuiControlSliderBase(Vector2? position = null, float width = 0.29f, MyGuiSliderProperties props = null, float? defaultRatio = null, Vector4? color = null, float labelScale = 0.8f, float labelSpaceWidth = 0f, string labelFont = "White", string toolTip = null, MyGuiControlSliderStyleEnum visualStyle = MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, bool showLabel = true)
			: base(position, null, null, toolTip, null, isActiveControl: true, canHaveFocus: true, MyGuiControlHighlightType.WHEN_CURSOR_OVER, originAlign)
		{
			this.showLabel = showLabel;
			if (defaultRatio.HasValue)
			{
				defaultRatio = MathHelper.Clamp(defaultRatio.Value, 0f, 1f);
			}
			if (props == null)
			{
				props = MyGuiSliderProperties.Default;
			}
			m_props = props;
			DefaultRatio = defaultRatio;
			m_ratio = (defaultRatio.HasValue ? defaultRatio.Value : 0f);
			m_labelSpaceWidth = labelSpaceWidth;
			m_label = new MyGuiControlLabel(null, null, string.Empty, null, labelScale, labelFont, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			if (showLabel)
			{
				Elements.Add(m_label);
			}
			VisualStyle = visualStyle;
			base.Size = new Vector2(width, base.Size.Y);
			UpdateLabel();
		}

		public override void OnRemoving()
		{
			SliderClicked = null;
			ValueChanged = null;
			base.OnRemoving();
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase ret = base.HandleInput();
			if (ret != null)
			{
				return ret;
			}
			if (!base.Enabled)
			{
				return null;
			}
			if (base.IsMouseOver && MyInput.Static.IsNewPrimaryButtonPressed() && !OnSliderClicked())
			{
				m_controlCaptured = true;
			}
			if (MyInput.Static.IsNewPrimaryButtonReleased())
			{
				m_controlCaptured = false;
			}
			if (base.IsMouseOver)
			{
				if (m_controlCaptured)
				{
					float start = GetStart();
					float end = GetEnd();
					Ratio = (MyGuiManager.MouseCursorPosition.X - start) / (end - start);
					ret = this;
				}
				else if (MyInput.Static.IsNewSecondaryButtonPressed() && DefaultRatio.HasValue)
				{
					Ratio = DefaultRatio.Value;
					ret = this;
				}
			}
			else if (m_controlCaptured)
			{
				ret = this;
			}
			float multiplier;
			if (base.HasFocus)
			{
				multiplier = 1f;
				bool num = MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MODIF_L, MyControlStateType.PRESSED);
				bool flag = MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MODIF_R, MyControlStateType.PRESSED);
				int num2 = (num ? 1 : 0) + (flag ? 2 : 0);
				bool flag2 = false;
				switch (num2)
				{
				case 0:
					multiplier = 1f;
					break;
				case 1:
					multiplier = 10f;
					break;
				case 2:
					multiplier = 100f;
					break;
				case 3:
					multiplier = 0f;
					flag2 = true;
					break;
				}
				if (MyGuiManager.TotalTimeInMilliseconds - m_lastTimeArrowPressed > MyGuiConstants.REPEAT_PRESS_DELAY)
				{
					if (MyInput.Static.IsKeyPress(MyKeys.Left) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_LEFT, MyControlStateType.PRESSED))
					{
<<<<<<< HEAD
						if (m_consecutiveSliderMoves != 0f || MyInput.Static.IsNewKeyPressed(MyKeys.Left) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_LEFT))
						{
							if (flag2 && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_LEFT))
							{
								HalfSkip(direction: false);
							}
							else
							{
								MoveSlider(direction: false, multiplier);
							}
=======
						if (flag2 && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_LEFT))
						{
							HalfSkip(direction: false);
						}
						else
						{
							MoveSlider(direction: false, multiplier);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					else if (MyInput.Static.IsKeyPress(MyKeys.Right) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_RIGHT, MyControlStateType.PRESSED))
					{
<<<<<<< HEAD
						if (m_consecutiveSliderMoves != 0f || MyInput.Static.IsNewKeyPressed(MyKeys.Right) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_RIGHT))
						{
							if (flag2 && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_RIGHT))
							{
								HalfSkip(direction: true);
							}
							else
							{
								MoveSlider(direction: true, multiplier);
							}
=======
						if (flag2 && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_RIGHT))
						{
							HalfSkip(direction: true);
						}
						else
						{
							MoveSlider(direction: true, multiplier);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					else
					{
						m_consecutiveSliderMoves = 0f;
					}
				}
			}
			return ret;
			void HalfSkip(bool direction)
			{
				if (direction)
				{
					if (Ratio + 0.1f < 0.5f)
					{
						Ratio = 0.5f;
					}
					else if (Ratio <= 1f)
					{
						Ratio = 1f;
					}
				}
				else if (Ratio - 0.1f > 0.5f)
				{
					Ratio = 0.5f;
				}
				else if (Ratio >= 0f)
				{
					Ratio = 0f;
				}
				ret = this;
			}
			void MoveSlider(bool direction, float stepMultiplier)
			{
				if (direction != m_lastMoveDirection)
				{
					m_consecutiveSliderMoves = 0f;
				}
				m_consecutiveSliderMoves += 1f;
				m_lastMoveDirection = direction;
				if (m_consecutiveSliderMoves < 5f)
				{
					m_lastTimeArrowPressed = MyGuiManager.TotalTimeInMilliseconds;
				}
				if (m_consecutiveSliderMoves == 1f || m_consecutiveSliderMoves > 7f)
				{
					Ratio += Math.Max(m_consecutiveSliderMoves / 3f * 0.0001f * multiplier, MinimumStep) * (float)(direction ? 1 : (-1));
				}
				ret = this;
			}
		}

		public void SetControllCaptured(bool captured)
		{
			m_controlCaptured = captured;
		}

		protected virtual bool OnSliderClicked()
		{
			if (SliderClicked != null)
			{
				return SliderClicked(this);
			}
			return false;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			m_railTexture.Draw(GetPositionAbsoluteTopLeft(), base.Size - new Vector2(m_labelSpaceWidth, 0f), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), DebugScale);
			DrawThumb(transitionAlpha);
			if (showLabel)
			{
				m_label.Draw(transitionAlpha, backgroundTransitionAlpha);
			}
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshInternals();
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

		private void DrawThumb(float transitionAlpha)
		{
			float y = GetPositionAbsoluteTopLeft().Y + base.Size.Y / 2f;
			float start = GetStart();
			float end = GetEnd();
			float num = MathHelper.Lerp(start, end, m_ratio);
			MyGuiManager.DrawSpriteBatch(m_thumbTexture, new Vector2(num, y), m_styleDef.ThumbTexture.SizeGui * ((DebugScale != 1f) ? (DebugScale * 0.5f) : DebugScale), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
		}

		private float GetStart()
		{
			return GetPositionAbsoluteTopLeft().X + MyGuiConstants.SLIDER_INSIDE_OFFSET_X;
		}

		private float GetEnd()
		{
			return GetPositionAbsoluteTopLeft().X + (base.Size.X - (MyGuiConstants.SLIDER_INSIDE_OFFSET_X + m_labelSpaceWidth));
		}

		protected void UpdateLabel()
		{
			m_label.Text = m_props.FormatLabel(Value);
			RefreshInternals();
		}

		private void RefreshVisualStyle()
		{
			m_styleDef = GetVisualStyle(VisualStyle);
			RefreshInternals();
		}

		private void RefreshInternals()
		{
			if (m_styleDef == null)
			{
				m_styleDef = m_styles[0];
			}
			if (base.HasHighlight)
			{
				m_railTexture = m_styleDef.RailHighlightTexture;
				m_thumbTexture = m_styleDef.ThumbTexture.Highlight;
			}
			else if (base.HasFocus)
			{
				m_railTexture = m_styleDef.RailFocusTexture ?? m_styleDef.RailHighlightTexture;
				m_thumbTexture = m_styleDef.ThumbTexture.Focus ?? m_styleDef.ThumbTexture.Highlight;
			}
			else
			{
				m_railTexture = m_styleDef.RailTexture;
				m_thumbTexture = m_styleDef.ThumbTexture.Normal;
			}
			base.MinSize = new Vector2(m_railTexture.MinSizeGui.X + m_labelSpaceWidth, Math.Max(m_railTexture.MinSizeGui.Y, m_label.Size.Y)) * DebugScale;
			base.MaxSize = new Vector2(m_railTexture.MaxSizeGui.X + m_labelSpaceWidth, Math.Max(m_railTexture.MaxSizeGui.Y, m_label.Size.Y)) * DebugScale;
			m_label.Position = new Vector2(base.Size.X * 0.5f, 0f);
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
