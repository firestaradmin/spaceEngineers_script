using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiScreenDialogAmount : MyGuiScreenBase
	{
		private MyGuiControlTextbox m_amountTextbox;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlButton m_increaseButton;

		private MyGuiControlButton m_decreaseButton;

		private MyGuiControlLabel m_errorLabel;

		private MyGuiControlSlider m_amountSlider;

		private StringBuilder m_textBuffer;

		private MyStringId m_caption;

		private bool m_parseAsInteger;

		private float m_amountMin;

		private float m_amountMax;

		private float m_amount;

		private bool m_lastGamepadControlsVisibility;

		private int m_decimalDigits;

		public event Action<float> OnConfirmed;

		public MyGuiScreenDialogAmount(float min, float max, MyStringId caption, int minMaxDecimalDigits = 3, bool parseAsInteger = false, float? defaultAmount = null, float backgroundTransition = 0f, float guiTransition = 0f)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(87f / 175f, 147f / 524f), isTopMostScreen: false, null, backgroundTransition, guiTransition)
		{
			base.CanHideOthers = false;
			base.EnabledBackgroundFade = true;
			m_textBuffer = new StringBuilder();
			m_amountMin = min;
			m_amountMax = max;
			m_amount = defaultAmount ?? max;
			m_parseAsInteger = parseAsInteger;
			m_caption = caption;
			m_decimalDigits = ((!parseAsInteger) ? minMaxDecimalDigits : 0);
			RecreateControls(contructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDialogAmount";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			string path = MyGuiScreenBase.MakeScreenFilepath("DialogAmount");
			MyObjectBuilderSerializer.DeserializeXML(Path.Combine(MyFileSystem.ContentPath, path), out MyObjectBuilder_GuiScreen objectBuilder);
			Init(objectBuilder);
			AddCaption(m_caption, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.78f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList);
<<<<<<< HEAD
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, ConfirmButton_OnButtonClick);
			m_cancelButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, CancelButton_OnButtonClick);
=======
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, confirmButton_OnButtonClick);
			m_cancelButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, cancelButton_OnButtonClick);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Vector2 vector = new Vector2(0.002f, m_size.Value.Y / 2f - 0.071f);
			Vector2 vector2 = new Vector2(0.018f, 0f);
			m_okButton.Position = vector - vector2;
			m_cancelButton.Position = vector + vector2;
			Controls.Add(m_okButton);
			Controls.Add(m_cancelButton);
			m_amountTextbox = (MyGuiControlTextbox)Controls.GetControlByName("AmountTextbox");
			m_increaseButton = (MyGuiControlButton)Controls.GetControlByName("IncreaseButton");
			m_decreaseButton = (MyGuiControlButton)Controls.GetControlByName("DecreaseButton");
			m_errorLabel = (MyGuiControlLabel)Controls.GetControlByName("ErrorLabel");
			m_errorLabel.TextScale = 0.68f;
			m_errorLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_errorLabel.Position = new Vector2(-0.19f, 0.008f);
			m_errorLabel.Visible = false;
			int num = m_amountMax.ToString("F0").Length + m_decimalDigits;
			m_amountSlider = new MyGuiControlSlider(new Vector2(0f, m_amountTextbox.Position.Y), m_amountMin, m_amountMax, 0.39f, m_amount, null, "{0}", m_decimalDigits, 0.8f, (float)num * 0.01f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, m_parseAsInteger, showLabel: true);
			m_amountSlider.ValueChanged = OnSliderValueChanged;
			Controls.Add(m_amountSlider);
<<<<<<< HEAD
			m_amountTextbox.TextChanged += AmountTextbox_TextChanged;
			m_increaseButton.ButtonClicked += IncreaseButton_OnButtonClick;
			m_decreaseButton.ButtonClicked += DecreaseButton_OnButtonClick;
=======
			m_amountTextbox.TextChanged += amountTextbox_TextChanged;
			m_increaseButton.ButtonClicked += increaseButton_OnButtonClick;
			m_decreaseButton.ButtonClicked += decreaseButton_OnButtonClick;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			RefreshAmountTextbox();
			m_amountTextbox.SelectAll();
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(m_okButton.Position.X - minSizeGui.X / 2f, m_okButton.Position.Y - minSizeGui.Y / 2f));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.GamepadHelpTextId = MyCommonTexts.DialogAmount_Help_Screen;
			ChangeGamepadControlsVisibility(MyInput.Static.IsJoystickLastUsed);
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			base.HandleUnhandledInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
<<<<<<< HEAD
			{
				ConfirmButton_OnButtonClick(m_okButton);
			}
			if (MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01))
			{
				MyVRage.Platform.Input2.ShowVirtualKeyboardIfNeeded(OnVirtualKeyboardDataReceived);
			}
		}

		private void OnVirtualKeyboardDataReceived(string text)
		{
			string text2 = "";
			bool flag = false;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (char.IsDigit(c))
				{
					text2 += c;
				}
				if ((c == '.' || c == ',') && !flag)
				{
					text2 += ".";
					flag = true;
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				if (!char.IsDigit(text2.First()))
				{
					text2 = "0" + text2;
				}
				if (!char.IsDigit(text2.Last()))
				{
					text2 += "0";
				}
				m_amountTextbox.Text = text2;
				TryParseAndStoreAmount(text2);
				ChangeGamepadControlsVisibility(gamepadControlsVisible: true);
				text2 = null;
			}
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (hasFocus)
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				if (m_lastGamepadControlsVisibility != MyInput.Static.IsJoystickLastUsed)
				{
					ChangeGamepadControlsVisibility(MyInput.Static.IsJoystickLastUsed);
				}
			}
<<<<<<< HEAD
=======
			if (MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01))
			{
				MyVRage.Platform.Input2.ShowVirtualKeyboardIfNeeded(OnVirtualKeyboardDataReceived);
			}
		}

		private void OnVirtualKeyboardDataReceived(string text)
		{
			string text2 = "";
			bool flag = false;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (char.IsDigit(c))
				{
					text2 += c;
				}
				if ((c == '.' || c == ',') && !flag)
				{
					text2 += ".";
					flag = true;
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				if (!char.IsDigit(Enumerable.First<char>((IEnumerable<char>)text2)))
				{
					text2 = "0" + text2;
				}
				if (!char.IsDigit(Enumerable.Last<char>((IEnumerable<char>)text2)))
				{
					text2 += "0";
				}
				m_amountTextbox.Text = text2;
				TryParseAndStoreAmount(text2);
				ChangeGamepadControlsVisibility(gamepadControlsVisible: true);
				text2 = null;
			}
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (hasFocus)
			{
				m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				if (m_lastGamepadControlsVisibility != MyInput.Static.IsJoystickLastUsed)
				{
					ChangeGamepadControlsVisibility(MyInput.Static.IsJoystickLastUsed);
				}
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return result;
		}

		private void RefreshAmountTextbox()
		{
			m_textBuffer.Clear();
			if (m_parseAsInteger)
			{
				m_textBuffer.AppendInt32((int)m_amount);
			}
			else
			{
				m_textBuffer.AppendDecimalDigit(m_amount, 4);
			}
			m_amountTextbox.TextChanged -= AmountTextbox_TextChanged;
			m_amountTextbox.Text = m_textBuffer.ToString();
			m_amountTextbox.TextChanged += AmountTextbox_TextChanged;
			m_amountTextbox.ColorMask = Vector4.One;
		}

		private bool TryParseAndStoreAmount(string text)
		{
			if (text.TryParseWithSuffix(NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var value) && !float.IsNaN(value) && !float.IsInfinity(value))
			{
				m_amount = (m_parseAsInteger ? ((float)Math.Floor(value)) : value);
				return true;
			}
			return false;
		}

		private void ShowError(string text)
		{
			m_errorLabel.Text = text;
			m_errorLabel.Visible = true;
			m_amountTextbox.ColorMask = Color.Red.ToVector4();
		}

		private void ChangeGamepadControlsVisibility(bool gamepadControlsVisible)
		{
			m_amountSlider.Visible = gamepadControlsVisible;
			m_lastGamepadControlsVisibility = gamepadControlsVisible;
			m_amountTextbox.Visible = !gamepadControlsVisible;
			m_increaseButton.Visible = !gamepadControlsVisible;
			m_decreaseButton.Visible = !gamepadControlsVisible;
			if (gamepadControlsVisible)
			{
				if (!TryParseAndStoreAmount(m_amountTextbox.Text))
				{
					ShowError(MyTexts.GetString(MyCommonTexts.DialogAmount_ParsingError));
				}
				else
				{
					m_amountSlider.Value = m_amount;
				}
			}
			else
			{
				RefreshAmountTextbox();
				base.FocusedControl = m_amountTextbox;
				m_amountTextbox.SelectAll();
			}
		}

		private void OnSliderValueChanged(MyGuiControlSlider slider)
		{
			m_amount = slider.Value;
			m_errorLabel.Visible = false;
		}

<<<<<<< HEAD
		private void AmountTextbox_TextChanged(MyGuiControlTextbox obj)
=======
		private void amountTextbox_TextChanged(MyGuiControlTextbox obj)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			m_amountTextbox.ColorMask = Vector4.One;
			m_errorLabel.Visible = false;
		}

<<<<<<< HEAD
		private void IncreaseButton_OnButtonClick(MyGuiControlButton sender)
=======
		private void increaseButton_OnButtonClick(MyGuiControlButton sender)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (!TryParseAndStoreAmount(m_amountTextbox.Text))
			{
				ShowError(MyTexts.GetString(MyCommonTexts.DialogAmount_ParsingError));
				return;
<<<<<<< HEAD
			}
			m_amount += 1f;
=======
			}
			m_amount += 1f;
			m_amount = MathHelper.Clamp(m_amount, m_amountMin, m_amountMax);
			RefreshAmountTextbox();
		}

		private void decreaseButton_OnButtonClick(MyGuiControlButton sender)
		{
			if (!TryParseAndStoreAmount(m_amountTextbox.Text))
			{
				ShowError(MyTexts.GetString(MyCommonTexts.DialogAmount_ParsingError));
				return;
			}
			m_amount -= 1f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_amount = MathHelper.Clamp(m_amount, m_amountMin, m_amountMax);
			RefreshAmountTextbox();
		}

		private void DecreaseButton_OnButtonClick(MyGuiControlButton sender)
		{
			if (m_lastGamepadControlsVisibility)
			{
				if (m_parseAsInteger)
				{
					m_amount = (float)Math.Floor(m_amount);
				}
			}
			else if (!TryParseAndStoreAmount(m_amountTextbox.Text))
			{
				ShowError(MyTexts.GetString(MyCommonTexts.DialogAmount_ParsingError));
				return;
			}
			m_amount -= 1f;
			m_amount = MathHelper.Clamp(m_amount, m_amountMin, m_amountMax);
			RefreshAmountTextbox();
		}

		private void ConfirmButton_OnButtonClick(MyGuiControlButton sender)
		{
			if (m_lastGamepadControlsVisibility)
			{
				if (m_parseAsInteger)
				{
					m_amount = (float)Math.Floor(m_amount);
				}
			}
			else if (!TryParseAndStoreAmount(m_amountTextbox.Text))
			{
<<<<<<< HEAD
				ShowError(MyTexts.GetString(MyCommonTexts.DialogAmount_ParsingError));
=======
				ShowError(string.Format(MyTexts.GetString(MyCommonTexts.DialogAmount_RangeError), m_amountMin, m_amountMax));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return;
			}
			if (m_amount > m_amountMax || m_amount < m_amountMin)
			{
				ShowError(string.Format(MyTexts.GetString(MyCommonTexts.DialogAmount_RangeError), m_amountMin, m_amountMax));
				return;
			}
			this.OnConfirmed.InvokeIfNotNull(m_amount);
			CloseScreen();
		}

		private void CancelButton_OnButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}
	}
}
