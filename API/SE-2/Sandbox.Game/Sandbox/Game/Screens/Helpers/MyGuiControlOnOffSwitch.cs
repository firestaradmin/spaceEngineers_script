using System;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlOnOffSwitch))]
	public class MyGuiControlOnOffSwitch : MyGuiControlBase
	{
		public class StyleDefinition
		{
			public MyGuiCompositeTexture NormalTexture;

			public MyGuiCompositeTexture HighlightTexture;

			public MyGuiCompositeTexture FocusTexture;
		}

		private static StyleDefinition[] m_styles;

		private MyGuiControlOnOffSwitchStyleEnum m_visualStyle;

		private StyleDefinition m_styleDef;

		private MyGuiControlCheckbox m_onButton;

		private MyGuiControlLabel m_onLabel;

		private MyGuiControlCheckbox m_offButton;

		private MyGuiControlLabel m_offLabel;

		private bool m_value;

		public MyGuiControlOnOffSwitchStyleEnum VisualStyle
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

<<<<<<< HEAD
		/// <summary>
		/// On/Off value of this switch. true = On; false = Off
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool Value
		{
			get
			{
				return m_value;
			}
			set
			{
				if (m_value != value)
				{
					m_value = value;
					UpdateButtonState();
					if (this.ValueChanged != null)
					{
						this.ValueChanged(this);
					}
				}
			}
		}

		public event Action<MyGuiControlOnOffSwitch> ValueChanged;

		static MyGuiControlOnOffSwitch()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlOnOffSwitchStyleEnum>() + 1];
			m_styles[0] = new StyleDefinition
			{
				NormalTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER,
				HighlightTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER_HIGHLIGHT,
				FocusTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER_FOCUS
			};
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlOnOffSwitchStyleEnum style)
		{
			return m_styles[(int)style];
		}

		private void RefreshVisualStyle()
		{
			m_styleDef = GetVisualStyle(VisualStyle);
			RefreshInternals();
		}

		public void RefreshInternals()
		{
			if (base.HasHighlight)
			{
				BackgroundTexture = m_styleDef.HighlightTexture;
			}
			else if (base.HasFocus)
			{
				BackgroundTexture = m_styleDef.FocusTexture ?? m_styleDef.HighlightTexture;
			}
			else
			{
				BackgroundTexture = m_styleDef.NormalTexture;
			}
			m_onButton.Refresh();
			m_offButton.Refresh();
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

		public MyGuiControlOnOffSwitch(bool initialValue = false, string onText = null, string offText = null, bool is_buttonAutoScaleEnabled = false)
			: base(null, null, null, null, null, isActiveControl: true, canHaveFocus: true)
		{
			m_onButton = new MyGuiControlCheckbox(null, null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.SwitchOnOffLeft, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			m_offButton = new MyGuiControlCheckbox(null, null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.SwitchOnOffRight, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
<<<<<<< HEAD
			float x = m_offButton.Size.X;
			m_onLabel = new MyGuiControlLabel(new Vector2(m_onButton.Size.X * -0.5f, 0f), null, onText ?? MyTexts.GetString(MySpaceTexts.SwitchText_On), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: false, x, is_buttonAutoScaleEnabled);
			m_offLabel = new MyGuiControlLabel(new Vector2(m_onButton.Size.X * 0.5f, 0f), null, offText ?? MyTexts.GetString(MySpaceTexts.SwitchText_Off), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: false, x, is_buttonAutoScaleEnabled);
=======
			float maxWidth = m_offButton.Size.X;
			m_onLabel = new MyGuiControlLabel(new Vector2(m_onButton.Size.X * -0.5f, 0f), null, onText ?? MyTexts.GetString(MySpaceTexts.SwitchText_On), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: false, maxWidth, is_buttonAutoScaleEnabled);
			m_offLabel = new MyGuiControlLabel(new Vector2(m_onButton.Size.X * 0.5f, 0f), null, offText ?? MyTexts.GetString(MySpaceTexts.SwitchText_Off), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: false, maxWidth, is_buttonAutoScaleEnabled);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			VisualStyle = MyGuiControlOnOffSwitchStyleEnum.Default;
			float num = 0.003f;
			base.Size = new Vector2(m_onButton.Size.X + m_offButton.Size.X + num, Math.Max(m_onButton.Size.Y, m_offButton.Size.Y));
			Elements.Add(m_onButton);
			Elements.Add(m_offButton);
			Elements.Add(m_onLabel);
			Elements.Add(m_offLabel);
			m_value = initialValue;
			UpdateButtonState();
		}

		public override void Init(MyObjectBuilder_GuiControlBase builder)
		{
			base.Init(builder);
			base.Size = new Vector2(m_onButton.Size.X + m_offButton.Size.X, Math.Max(m_onButton.Size.Y, m_offButton.Size.Y));
			UpdateButtonState();
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase != null)
			{
				return myGuiControlBase;
			}
<<<<<<< HEAD
=======
			if (!base.IsMouseOver)
			{
				_ = base.HasFocus;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			bool flag = MyInput.Static.IsNewLeftMouseReleased() || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT, MyControlStateType.NEW_RELEASED);
			if ((base.Enabled && base.IsMouseOver && flag) || (base.Enabled && base.HasFocus && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01))))
			{
				Value = !Value;
				myGuiControlBase = this;
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			}
			return myGuiControlBase;
		}

		private void UpdateButtonState()
		{
			m_onButton.IsChecked = Value;
			m_offButton.IsChecked = !Value;
			m_onLabel.Font = (Value ? "White" : "Blue");
			m_offLabel.Font = (Value ? "Blue" : "White");
		}

		protected override void OnVisibleChanged()
		{
			if (m_onButton != null)
			{
				m_onButton.Visible = base.Visible;
			}
			if (m_offButton != null)
			{
				m_offButton.Visible = base.Visible;
			}
			base.OnVisibleChanged();
		}
	}
}
