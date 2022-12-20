using System;
using System.Collections.Generic;
using System.Text;
<<<<<<< HEAD
using Sandbox.Definitions;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenOptionsControls : MyGuiScreenBase
	{
		private class ControlButtonData
		{
			public readonly MyControl Control;

			public readonly MyGuiInputDeviceEnum Device;

			public ControlButtonData(MyControl control, MyGuiInputDeviceEnum device)
			{
				Control = control;
				Device = device;
			}
		}

		private class MyGuiControlAssignKeyMessageBox : MyGuiScreenMessageBox
		{
			private MyControl m_controlBeingSet;

			private MyGuiInputDeviceEnum m_deviceType;

			private List<MyKeys> m_newPressedKeys = new List<MyKeys>();

			private List<MyMouseButtonsEnum> m_newPressedMouseButtons = new List<MyMouseButtonsEnum>();

			private List<MyJoystickButtonsEnum> m_newPressedJoystickButtons = new List<MyJoystickButtonsEnum>();

			private List<MyJoystickAxesEnum> m_newPressedJoystickAxes = new List<MyJoystickAxesEnum>();

			private List<MyKeys> m_oldPressedKeys = new List<MyKeys>();

			private List<MyMouseButtonsEnum> m_oldPressedMouseButtons = new List<MyMouseButtonsEnum>();

			private List<MyJoystickButtonsEnum> m_oldPressedJoystickButtons = new List<MyJoystickButtonsEnum>();

			private List<MyJoystickAxesEnum> m_oldPressedJoystickAxes = new List<MyJoystickAxesEnum>();

			public MyGuiControlAssignKeyMessageBox(MyGuiInputDeviceEnum deviceType, MyControl controlBeingSet, MyStringId messageText)
				: base(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.NONE, MyTexts.Get(messageText), MyTexts.Get(MyCommonTexts.SelectControl), default(MyStringId), default(MyStringId), default(MyStringId), default(MyStringId), null, 0, ResultEnum.YES, canHideOthers: true, null)
			{
				base.DrawMouseCursor = false;
				m_isTopMostScreen = false;
				m_controlBeingSet = controlBeingSet;
				m_deviceType = deviceType;
				MyInput.Static.GetListOfPressedKeys(m_oldPressedKeys);
				MyInput.Static.GetListOfPressedMouseButtons(m_oldPressedMouseButtons);
				m_closeOnEsc = false;
				base.CanBeHidden = true;
			}

			public override void HandleInput(bool receivedFocusInThisUpdate)
			{
				base.HandleInput(receivedFocusInThisUpdate);
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL))
				{
					Canceling();
				}
				if (base.State != MyGuiScreenState.CLOSING && base.State != MyGuiScreenState.HIDING)
				{
					switch (m_deviceType)
					{
					case MyGuiInputDeviceEnum.Keyboard:
					case MyGuiInputDeviceEnum.KeyboardSecond:
						HandleKey();
						break;
					case MyGuiInputDeviceEnum.Mouse:
						HandleMouseButton();
						break;
					case (MyGuiInputDeviceEnum)3:
					case (MyGuiInputDeviceEnum)4:
						break;
					}
				}
			}

			private void HandleKey()
			{
				ReadPressedKeys();
				foreach (MyKeys key in m_newPressedKeys)
				{
					if (m_oldPressedKeys.Contains(key))
<<<<<<< HEAD
					{
						continue;
					}
					if (!MyInput.Static.IsKeyValid(key))
					{
						ShowControlIsNotValidMessageBox();
						break;
					}
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					MyControl ctrl = MyInput.Static.GetControl(key);
					if (ctrl != null)
					{
=======
					{
						continue;
					}
					if (!MyInput.Static.IsKeyValid(key))
					{
						ShowControlIsNotValidMessageBox();
						break;
					}
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					MyControl ctrl = MyInput.Static.GetControl(key);
					if (ctrl != null)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (ctrl.Equals(m_controlBeingSet))
						{
							OverwriteAssignment(ctrl, key);
							CloseScreen();
							break;
						}
						StringBuilder output = null;
						MyControl.AppendName(ref output, key);
						ShowControlIsAlreadyAssigned(ctrl, output, delegate
						{
<<<<<<< HEAD
							OverwriteAssignment(ctrl, key);
=======
							AnywayAssignment(ctrl, key);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						});
					}
					else
					{
						m_controlBeingSet.SetControl(m_deviceType, key);
						CloseScreen();
					}
					break;
				}
				m_oldPressedKeys.Clear();
				MyUtils.Swap(ref m_oldPressedKeys, ref m_newPressedKeys);
			}

			private void HandleMouseButton()
			{
				MyInput.Static.GetListOfPressedMouseButtons(m_newPressedMouseButtons);
				foreach (MyMouseButtonsEnum button in m_newPressedMouseButtons)
				{
					if (m_oldPressedMouseButtons.Contains(button))
<<<<<<< HEAD
					{
						continue;
					}
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					if (!MyInput.Static.IsMouseButtonValid(button))
					{
=======
					{
						continue;
					}
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					if (!MyInput.Static.IsMouseButtonValid(button))
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						ShowControlIsNotValidMessageBox();
						break;
					}
					MyControl ctrl = MyInput.Static.GetControl(button);
					if (ctrl != null)
					{
						if (ctrl.Equals(m_controlBeingSet))
						{
							OverwriteAssignment(ctrl, button);
							CloseScreen();
							break;
						}
						StringBuilder output = null;
						MyControl.AppendName(ref output, button);
						ShowControlIsAlreadyAssigned(ctrl, output, delegate
						{
<<<<<<< HEAD
							OverwriteAssignment(ctrl, button);
=======
							AnywayAssignment(ctrl, button);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						});
					}
					else
					{
						m_controlBeingSet.SetControl(button);
						CloseScreen();
					}
					break;
				}
				m_oldPressedMouseButtons.Clear();
				MyUtils.Swap(ref m_oldPressedMouseButtons, ref m_newPressedMouseButtons);
			}

			private void ReadPressedKeys()
			{
				MyInput.Static.GetListOfPressedKeys(m_newPressedKeys);
				m_newPressedKeys.Remove(MyKeys.Control);
				m_newPressedKeys.Remove(MyKeys.Shift);
				m_newPressedKeys.Remove(MyKeys.Alt);
				if (m_newPressedKeys.Contains(MyKeys.LeftControl) && m_newPressedKeys.Contains(MyKeys.RightAlt))
				{
					m_newPressedKeys.Remove(MyKeys.LeftControl);
				}
			}

			private void ShowControlIsNotValidMessageBox()
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.ControlIsNotValid), MyTexts.Get(MyCommonTexts.CanNotAssignControl)));
			}

			private void ShowControlIsAlreadyAssigned(MyControl controlAlreadySet, StringBuilder controlButtonName, Action overwriteAssignmentCallback)
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox = MakeControlIsAlreadyAssignedDialog(controlAlreadySet, controlButtonName);
				myGuiScreenMessageBox.ResultCallback = delegate(ResultEnum r)
				{
					if (r == ResultEnum.YES)
					{
						overwriteAssignmentCallback();
						CloseScreen();
					}
					else
					{
						MyInput.Static.GetListOfPressedKeys(m_oldPressedKeys);
						MyInput.Static.GetListOfPressedMouseButtons(m_oldPressedMouseButtons);
					}
				};
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
			}

			private MyGuiScreenMessageBox MakeControlIsAlreadyAssignedDialog(MyControl controlAlreadySet, StringBuilder controlButtonName)
			{
				return MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ControlAlreadyAssigned), controlButtonName, MyTexts.Get(controlAlreadySet.GetControlName()))), MyTexts.Get(MyCommonTexts.CanNotAssignControl));
			}

			private void OverwriteAssignment(MyControl controlAlreadySet, MyKeys key)
			{
				if (controlAlreadySet.GetKeyboardControl() == key)
				{
					controlAlreadySet.SetControl(MyGuiInputDeviceEnum.Keyboard, MyKeys.None);
				}
				else
				{
					controlAlreadySet.SetControl(MyGuiInputDeviceEnum.KeyboardSecond, MyKeys.None);
				}
				m_controlBeingSet.SetControl(m_deviceType, key);
			}

			private void AnywayAssignment(MyControl controlAlreadySet, MyKeys key)
			{
				m_controlBeingSet.SetControl(m_deviceType, key);
			}

			private void OverwriteAssignment(MyControl controlAlreadySet, MyMouseButtonsEnum button)
			{
				controlAlreadySet.SetControl(MyMouseButtonsEnum.None);
				m_controlBeingSet.SetControl(button);
			}

			private void AnywayAssignment(MyControl controlAlreadySet, MyMouseButtonsEnum button)
			{
				m_controlBeingSet.SetControl(button);
			}

			public override bool CloseScreen(bool isUnloading = false)
			{
				base.DrawMouseCursor = true;
				return base.CloseScreen(isUnloading);
			}
		}

		private MyGuiControlTypeEnum m_currentControlType;

		private MyGuiControlCombobox m_controlTypeList;

		private Dictionary<MyGuiControlTypeEnum, List<MyGuiControlBase>> m_allControls = new Dictionary<MyGuiControlTypeEnum, List<MyGuiControlBase>>();

		private List<MyGuiControlButton> m_key1Buttons;

		private List<MyGuiControlButton> m_key2Buttons;

		private List<MyGuiControlButton> m_mouseButtons;

		private List<MyGuiControlButton> m_joystickButtons;

		private List<MyGuiControlButton> m_joystickAxes;

		private MyGuiControlCheckbox m_invertMouseXCheckbox;

		private MyGuiControlCheckbox m_invertMouseYCheckbox;

		private MyGuiControlCheckbox m_InvertMouseScrollBlockSelectionCheckbox;

		private MyGuiControlSlider m_mouseSensitivitySlider;

		private MyGuiControlSlider m_joystickSensitivitySlider;

		private MyGuiControlSlider m_zoomMultiplierSlider;

		private MyGuiControlSlider m_joystickDeadzoneSlider;

		private MyGuiControlSlider m_joystickExponentSlider;

		private MyGuiControlCombobox m_joystickCombobox;

<<<<<<< HEAD
		private MyGuiControlCombobox m_controllerScheme;
=======
		private MyGuiControlCombobox m_gamepadScheme;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private MyGuiControlCheckbox m_invertYCharCheckbox;

		private MyGuiControlCheckbox m_invertYVehicleCheckbox;

		private Vector2 m_controlsOriginLeft;

		private Vector2 m_controlsOriginRight;

		private MyGuiControlElementGroup m_elementGroup;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

<<<<<<< HEAD
		private List<string> m_controllerSchemeNames = new List<string>();

		private bool m_invertYCharOriginalStatus;

		private bool m_invertYVehicleOriginalStatus;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyGuiScreenOptionsControls()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 0.9465649f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_elementGroup = new MyGuiControlElementGroup();
			AddCaption(MyCommonTexts.ScreenCaptionControls, null, new Vector2(0f, 0.003f));
			MyInput.Static.TakeSnapshot();
			_ = m_size.Value * new Vector2(0f, -0.5f);
			_ = m_size.Value * new Vector2(0f, 0.5f);
			_ = m_size.Value * -0.5f;
			m_controlsOriginLeft = (m_size.Value / 2f - new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE) * new Vector2(-1f, -1f) + new Vector2(0f, MyGuiConstants.SCREEN_CAPTION_DELTA_Y);
			m_controlsOriginRight = (m_size.Value / 2f - new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE) * new Vector2(1f, -1f) + new Vector2(0f, MyGuiConstants.SCREEN_CAPTION_DELTA_Y);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, m_size.Value.Y / 2f - 0.144f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiControlSeparatorList myGuiControlSeparatorList3 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList3.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList3);
			Vector2 vector = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 vector2 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			float num = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			float num2 = 25f;
			float sCREEN_CAPTION_DELTA_Y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y;
			float num3 = 0.0015f;
			new Vector2(0f, 0.045f);
			Vector2 vector3 = (m_size.Value / 2f - vector) * new Vector2(-1f, -1f) + new Vector2(0f, sCREEN_CAPTION_DELTA_Y);
			Vector2 vector4 = (m_size.Value / 2f - vector) * new Vector2(1f, -1f) + new Vector2(0f, sCREEN_CAPTION_DELTA_Y);
			Vector2 vector5 = (m_size.Value / 2f - vector2) * new Vector2(0f, 1f);
			new Vector2(vector4.X - (num + num3), vector4.Y);
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnOkClick);
			m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
			m_okButton.Position = vector5 + new Vector2(0f - num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_okButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_cancelButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnCancelClick);
			m_cancelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			m_cancelButton.Position = vector5 + new Vector2(num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_cancelButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.ComboBoxButton, MyGuiConstants.MESSAGE_BOX_BUTTON_SIZE_SMALL, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Revert), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_Defaults), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: OnResetDefaultsClick);
			myGuiControlButton.Position = new Vector2(0f, 0f) - new Vector2(0f - m_size.Value.X * 0.832f / 2f + myGuiControlButton.Size.X / 2f, m_size.Value.Y / 2f - 0.113f);
			myGuiControlButton.TextScale = 0.7f;
			Controls.Add(m_okButton);
			m_elementGroup.Add(m_okButton);
			Controls.Add(m_cancelButton);
			m_elementGroup.Add(m_cancelButton);
			Controls.Add(myGuiControlButton);
			m_elementGroup.Add(myGuiControlButton);
			m_currentControlType = MyGuiControlTypeEnum.General;
			m_controlTypeList = new MyGuiControlCombobox(new Vector2(0f - myGuiControlButton.Size.X / 2f - 0.009f, 0f) - new Vector2(0f, m_size.Value.Y / 2f - 0.11f));
			m_controlTypeList.Size = new Vector2(m_size.Value.X * 0.595f, 1f);
			m_controlTypeList.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_Category));
			m_controlTypeList.AddItem(0L, MyCommonTexts.ControlTypeGeneral);
			m_controlTypeList.AddItem(1L, MyCommonTexts.ControlTypeNavigation);
			m_controlTypeList.AddItem(5L, MyCommonTexts.ControlTypeSystems1);
			m_controlTypeList.AddItem(6L, MyCommonTexts.ControlTypeSystems2);
			m_controlTypeList.AddItem(7L, MyCommonTexts.ControlTypeSystems3);
			m_controlTypeList.AddItem(3L, MyCommonTexts.ControlTypeToolsOrWeapons);
			m_controlTypeList.AddItem(8L, MyCommonTexts.ControlTypeView);
			m_controlTypeList.SelectItemByKey((int)m_currentControlType);
			Controls.Add(m_controlTypeList);
			AddControls();
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(vector3.X, m_okButton.Position.Y - minSizeGui.Y / 2f));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			ActivateControls(m_currentControlType);
			base.FocusedControl = m_okButton;
			base.CloseButtonEnabled = true;
			base.GamepadHelpTextId = MySpaceTexts.ControlsOptions_Help_Screen;
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
<<<<<<< HEAD
			if (base.FocusedControl == m_controllerScheme && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_Y))
			{
				switch (m_controllerScheme.GetSelectedKey())
=======
			if (base.FocusedControl == m_gamepadScheme && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_Y))
			{
				switch (m_gamepadScheme.GetSelectedKey())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
				case 0L:
					MyScreenManager.AddScreen(new MyGuiScreenGamepadBindingsHelp(ControlScheme.Default));
					break;
				case 1L:
					MyScreenManager.AddScreen(new MyGuiScreenGamepadBindingsHelp(ControlScheme.Alternative));
					break;
				}
			}
		}

		private void AddControls()
		{
			m_key1Buttons = new List<MyGuiControlButton>();
			m_key2Buttons = new List<MyGuiControlButton>();
			m_mouseButtons = new List<MyGuiControlButton>();
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				m_joystickButtons = new List<MyGuiControlButton>();
				m_joystickAxes = new List<MyGuiControlButton>();
			}
			AddControlsByType(MyGuiControlTypeEnum.General);
			AddControlsByType(MyGuiControlTypeEnum.Navigation);
			AddControlsByType(MyGuiControlTypeEnum.Systems1);
			AddControlsByType(MyGuiControlTypeEnum.Systems2);
			AddControlsByType(MyGuiControlTypeEnum.Systems3);
			AddControlsByType(MyGuiControlTypeEnum.ToolsOrWeapons);
			AddControlsByType(MyGuiControlTypeEnum.Spectator);
			foreach (KeyValuePair<MyGuiControlTypeEnum, List<MyGuiControlBase>> allControl in m_allControls)
			{
				foreach (MyGuiControlBase item in allControl.Value)
				{
					Controls.Add(item);
				}
				DeactivateControls(allControl.Key);
			}
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				RefreshJoystickControlEnabling();
			}
		}

		private MyGuiControlLabel MakeLabel(float deltaMultip, MyStringId textEnum, bool isAutoScaleEnabled = false, float maxWidth = float.PositiveInfinity, bool isAutoEllipsisEnabled = false)
		{
			Vector2? position = m_controlsOriginLeft + deltaMultip * MyGuiConstants.CONTROLS_DELTA;
			string @string = MyTexts.GetString(textEnum);
			bool isAutoScaleEnabled2 = isAutoScaleEnabled;
			return new MyGuiControlLabel(position, null, @string, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, isAutoEllipsisEnabled, maxWidth, isAutoScaleEnabled2);
		}

		private MyGuiControlLabel MakeLabel(MyStringId textEnum, Vector2 position)
		{
			return new MyGuiControlLabel(position, null, MyTexts.GetString(textEnum), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
		}

		private MyGuiControlButton MakeControlButton(MyControl control, Vector2 position, MyGuiInputDeviceEnum device)
		{
			StringBuilder output = null;
			control.AppendBoundButtonNames(ref output, device);
			MyControl.AppendUnknownTextIfNeeded(ref output, MyTexts.GetString(MyCommonTexts.UnknownControl_None));
			return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.ControlSetting, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, output, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnControlClick, GuiSounds.MouseClick, 1f, null, activateOnMouseRelease: false, isAutoscaleEnabled: false, isEllipsisEnabled: false, OnSecondaryControlClick)
			{
				UserData = new ControlButtonData(control, device)
			};
		}

		private void AddControlsByType(MyGuiControlTypeEnum type)
		{
			if (type == MyGuiControlTypeEnum.General)
			{
				AddGeneralControls();
				return;
			}
			MyGuiControlButton.StyleDefinition visualStyle = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.ControlSetting);
			Vector2 controlsOriginRight = m_controlsOriginRight;
			controlsOriginRight.X -= 0.02f;
			controlsOriginRight.Y -= 0.01f;
			m_allControls[type] = new List<MyGuiControlBase>();
			float num = 2f;
			float num2 = 0.85f;
			DictionaryValuesReader<MyStringId, MyControl> gameControlsList = MyInput.Static.GetGameControlsList();
			MyGuiControlLabel myGuiControlLabel = MakeLabel(MyCommonTexts.ScreenOptionsControls_Keyboard1, Vector2.Zero);
			MyGuiControlLabel myGuiControlLabel2 = MakeLabel(MyCommonTexts.ScreenOptionsControls_Keyboard2, Vector2.Zero);
			MyGuiControlLabel myGuiControlLabel3 = MakeLabel(MyCommonTexts.ScreenOptionsControls_Mouse, Vector2.Zero);
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				MakeLabel(MyCommonTexts.ScreenOptionsControls_Gamepad, Vector2.Zero);
			}
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				MakeLabel(MyCommonTexts.ScreenOptionsControls_AnalogAxes, Vector2.Zero);
			}
			float num3 = 1.1f * Math.Max(Math.Max(myGuiControlLabel.Size.X, myGuiControlLabel2.Size.X), Math.Max(myGuiControlLabel3.Size.X, visualStyle.SizeOverride.Value.X));
			Vector2 position = (num - 1f) * MyGuiConstants.CONTROLS_DELTA + controlsOriginRight;
			position.X += num3 * 0.5f - 0.265f;
			position.Y -= 0.015f;
			myGuiControlLabel.Position = position;
			position.X += num3;
			myGuiControlLabel2.Position = position;
			position.X += num3;
			myGuiControlLabel3.Position = position;
			m_allControls[type].Add(myGuiControlLabel);
			m_allControls[type].Add(myGuiControlLabel2);
			m_allControls[type].Add(myGuiControlLabel3);
			_ = MyFakes.ENABLE_JOYSTICK_SETTINGS;
			foreach (MyControl item in gameControlsList)
			{
				if (item.GetControlTypeEnum() == type)
				{
					m_allControls[type].Add(new MyGuiControlLabel(m_controlsOriginLeft + num * MyGuiConstants.CONTROLS_DELTA - new Vector2(0f, 0.03f), null, MyTexts.GetString(item.GetControlName()), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: true, 0.24f, isAutoScaleEnabled: true));
					position = controlsOriginRight + num * MyGuiConstants.CONTROLS_DELTA - new Vector2(0.265f, 0.015f);
					position.X += num3 * 0.5f;
					MyGuiControlButton myGuiControlButton = MakeControlButton(item, position, MyGuiInputDeviceEnum.Keyboard);
					myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_ClickToEdit));
					myGuiControlButton.IsAutoEllipsisEnabled = true;
					myGuiControlButton.IsAutoScaleEnabled = true;
					m_allControls[type].Add(myGuiControlButton);
					m_key1Buttons.Add(myGuiControlButton);
					position.X += num3;
					MyGuiControlButton myGuiControlButton2 = MakeControlButton(item, position, MyGuiInputDeviceEnum.KeyboardSecond);
					myGuiControlButton2.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_ClickToEdit));
					myGuiControlButton2.IsAutoEllipsisEnabled = true;
					myGuiControlButton2.IsAutoScaleEnabled = true;
					m_allControls[type].Add(myGuiControlButton2);
					m_key2Buttons.Add(myGuiControlButton2);
					position.X += num3;
					MyGuiControlButton myGuiControlButton3 = MakeControlButton(item, position, MyGuiInputDeviceEnum.Mouse);
					myGuiControlButton3.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_ClickToEdit));
					myGuiControlButton3.IsAutoEllipsisEnabled = true;
					myGuiControlButton3.IsAutoScaleEnabled = true;
					m_allControls[type].Add(myGuiControlButton3);
					m_mouseButtons.Add(myGuiControlButton3);
					position.X += num3;
					_ = MyFakes.ENABLE_JOYSTICK_SETTINGS;
					num += num2;
				}
			}
		}

		private void AddGeneralControls()
		{
			float num = m_size.Value.X * 0.83f;
			float num2 = 1.7f;
			m_controlsOriginRight.Y -= 0.025f;
			m_controlsOriginLeft.Y -= 0.025f;
			m_allControls[MyGuiControlTypeEnum.General] = new List<MyGuiControlBase>();
			m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(num2, MySpaceTexts.ZoomMultiplier));
			m_zoomMultiplierSlider = new MyGuiControlSlider(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2f, 0f), 0f, 1f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_ZoomMultiplier), defaultValue: MySandboxGame.Config.ZoomMultiplier);
			m_zoomMultiplierSlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
			m_zoomMultiplierSlider.Value = MySandboxGame.Config.ZoomMultiplier;
			m_allControls[MyGuiControlTypeEnum.General].Add(m_zoomMultiplierSlider);
			num2 += 0.585000038f;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(num2 * MyGuiConstants.CONTROLS_DELTA + new Vector2((0f - num) / 2f, m_controlsOriginRight.Y), num);
			m_allControls[MyGuiControlTypeEnum.General].Add(myGuiControlSeparatorList);
			num2 += 0.585000038f;
			m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(num2, MyCommonTexts.MouseSensitivity));
			m_mouseSensitivitySlider = new MyGuiControlSlider(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f), 0.1f, 3f, 0.29f, MyInput.Static.GetMouseSensitivity(), null, null, 1, 0.8f, 0f, "White", MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_MouseSensitivity), MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_mouseSensitivitySlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
			m_mouseSensitivitySlider.Value = MyInput.Static.GetMouseSensitivity();
			m_allControls[MyGuiControlTypeEnum.General].Add(m_mouseSensitivitySlider);
			num2 += 0.97f;
			m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(num2, MyCommonTexts.InvertMouseX));
			m_invertMouseXCheckbox = new MyGuiControlCheckbox(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(456.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f), null, isChecked: MyInput.Static.GetMouseXInversion(), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_InvertMouseX), visualStyle: MyGuiControlCheckboxStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_allControls[MyGuiControlTypeEnum.General].Add(m_invertMouseXCheckbox);
			num2 += 0.85f;
			m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(num2, MyCommonTexts.InvertMouseY));
			m_invertMouseYCheckbox = new MyGuiControlCheckbox(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(456.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f), null, isChecked: MyInput.Static.GetMouseYInversion(), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_InvertMouseY), visualStyle: MyGuiControlCheckboxStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_allControls[MyGuiControlTypeEnum.General].Add(m_invertMouseYCheckbox);
			num2 += 0.85f;
			m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(num2, MyCommonTexts.InvertMouseScrollBlockSelection, isAutoScaleEnabled: true, 0.25f, isAutoEllipsisEnabled: true));
			m_InvertMouseScrollBlockSelectionCheckbox = new MyGuiControlCheckbox(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(456.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f), null, isChecked: MyInput.Static.GetMouseScrollBlockSelectionInversion(), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_InvertMouseScrollBlockSelection), visualStyle: MyGuiControlCheckboxStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_allControls[MyGuiControlTypeEnum.General].Add(m_InvertMouseScrollBlockSelectionCheckbox);
			num2 += 0.585000038f;
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(num2 * MyGuiConstants.CONTROLS_DELTA + new Vector2((0f - num) / 2f, m_controlsOriginRight.Y), num);
			m_allControls[MyGuiControlTypeEnum.General].Add(myGuiControlSeparatorList2);
			num2 += 0.585000038f;
			if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
			{
				m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(num2, MyCommonTexts.Joystick));
				m_joystickCombobox = new MyGuiControlCombobox(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2f, 0f));
				m_joystickCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_JoystickOrGamepad));
				m_joystickCombobox.ItemSelected += OnSelectJoystick;
				AddJoysticksToComboBox();
				m_joystickCombobox.Size = new Vector2(452f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
				m_joystickCombobox.Enabled = !MyFakes.ENFORCE_CONTROLLER || !MyInput.Static.IsJoystickConnected();
				m_allControls[MyGuiControlTypeEnum.General].Add(m_joystickCombobox);
			}
			num2 += 0.97f;
			m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(num2, MyCommonTexts.GamepadScheme));
<<<<<<< HEAD
			m_controllerScheme = new MyGuiControlCombobox(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2f, 0f));
			m_controllerScheme.SetTooltip(MyTexts.GetString(MyCommonTexts.ToolTipOptionsControls_GamepadScheme));
			string gamepadSchemeName = MySandboxGame.Config.GamepadSchemeName;
			int gamepadSchemeId = MySandboxGame.Config.GamepadSchemeId;
			List<MyControllerSchemeDefinition> controllerSchemesSelectable = MyDefinitionManager.Static.GetControllerSchemesSelectable();
			m_controllerSchemeNames = new List<string>();
			int num3 = 0;
			int num4 = 0;
			foreach (MyControllerSchemeDefinition item in controllerSchemesSelectable)
			{
				if (gamepadSchemeName == item.Id.SubtypeName)
				{
					num4 = num3;
				}
				string string2 = MyTexts.GetString(MyStringId.GetOrCompute("GamepadScheme_" + item.Id.SubtypeName));
				m_controllerScheme.AddItem(num3, string2);
				m_controllerSchemeNames.Add(item.Id.SubtypeName);
				num3++;
			}
			if (num4 != -1)
			{
				m_controllerScheme.SelectItemByKey(num4);
			}
			else if (gamepadSchemeId > 0 || gamepadSchemeId < controllerSchemesSelectable.Count)
			{
				m_controllerScheme.SelectItemByKey(gamepadSchemeId);
			}
			else
			{
				m_controllerScheme.SelectItemByKey(0L);
			}
			m_controllerScheme.GamepadHelpTextId = MySpaceTexts.ControlsOptions_Help_Scheme;
			m_controllerScheme.Size = new Vector2(452f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
			m_controllerScheme.Enabled = !MyFakes.ENFORCE_CONTROLLER || !MyInput.Static.IsJoystickConnected();
			m_allControls[MyGuiControlTypeEnum.General].Add(m_controllerScheme);
=======
			m_gamepadScheme = new MyGuiControlCombobox(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2f, 0f));
			m_gamepadScheme.SetTooltip(MyTexts.GetString(MyCommonTexts.ToolTipOptionsControls_GamepadScheme));
			m_gamepadScheme.AddItem(0L, MyTexts.Get(MyCommonTexts.GamepadScheme_Default));
			m_gamepadScheme.AddItem(1L, MyTexts.Get(MyCommonTexts.GamepadScheme_1));
			m_gamepadScheme.SelectItemByKey(MySandboxGame.Config.GamepadSchemeId);
			m_gamepadScheme.GamepadHelpTextId = MySpaceTexts.ControlsOptions_Help_Scheme;
			m_gamepadScheme.Size = new Vector2(452f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
			m_gamepadScheme.Enabled = !MyFakes.ENFORCE_CONTROLLER || !MyInput.Static.IsJoystickConnected();
			m_allControls[MyGuiControlTypeEnum.General].Add(m_gamepadScheme);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			num2 += 0.97f;
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(num2, MyCommonTexts.JoystickSensitivity));
				m_joystickSensitivitySlider = new MyGuiControlSlider(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2f, 0f), 0.1f, 6f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_JoystickSensitivity), defaultValue: MyInput.Static.GetJoystickSensitivity());
				m_joystickSensitivitySlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
				m_joystickSensitivitySlider.Value = MyInput.Static.GetJoystickSensitivity();
				m_allControls[MyGuiControlTypeEnum.General].Add(m_joystickSensitivitySlider);
				num2 += 0.97f;
				m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(num2, MyCommonTexts.JoystickExponent));
				m_joystickExponentSlider = new MyGuiControlSlider(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2f, 0f), 1f, 8f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_JoystickGradualPrecision), defaultValue: MyInput.Static.GetJoystickExponent());
				m_joystickExponentSlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
				m_joystickExponentSlider.Value = MyInput.Static.GetJoystickExponent();
				m_allControls[MyGuiControlTypeEnum.General].Add(m_joystickExponentSlider);
				num2 += 0.97f;
				m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(num2, MyCommonTexts.JoystickDeadzone));
				m_joystickDeadzoneSlider = new MyGuiControlSlider(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2f, 0f), 0f, 0.5f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_JoystickDeadzoneWidth), defaultValue: MyInput.Static.GetJoystickDeadzone());
				m_joystickDeadzoneSlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
				m_joystickDeadzoneSlider.Value = MyInput.Static.GetJoystickDeadzone();
				m_allControls[MyGuiControlTypeEnum.General].Add(m_joystickDeadzoneSlider);
				num2 += 0.85f;
				m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(num2, MyCommonTexts.InvertGamepadYChar));
				m_invertYCharCheckbox = new MyGuiControlCheckbox(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(456.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f), null, isChecked: MyInput.Static.GetJoystickYInversionCharacter(), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_InvertGamepadYChar), visualStyle: MyGuiControlCheckboxStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
<<<<<<< HEAD
				m_invertYCharOriginalStatus = MyInput.Static.GetJoystickYInversionCharacter();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_allControls[MyGuiControlTypeEnum.General].Add(m_invertYCharCheckbox);
				num2 += 0.85f;
				m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(num2, MyCommonTexts.InvertGamepadYVehicle, isAutoScaleEnabled: true, 0.24f));
				m_invertYVehicleCheckbox = new MyGuiControlCheckbox(m_controlsOriginRight + num2 * MyGuiConstants.CONTROLS_DELTA - new Vector2(456.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f), null, isChecked: MyInput.Static.GetJoystickYInversionVehicle(), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_InvertGamepadYVehicle), visualStyle: MyGuiControlCheckboxStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
<<<<<<< HEAD
				m_invertYVehicleOriginalStatus = MyInput.Static.GetJoystickYInversionVehicle();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_allControls[MyGuiControlTypeEnum.General].Add(m_invertYVehicleCheckbox);
			}
			num2 += 0.85f;
			m_controlsOriginRight.Y += 0.025f;
			m_controlsOriginLeft.Y += 0.025f;
		}

		private void DeactivateControls(MyGuiControlTypeEnum type)
		{
			foreach (MyGuiControlBase item in m_allControls[type])
			{
				item.Visible = false;
			}
		}

		private void ActivateControls(MyGuiControlTypeEnum type)
		{
			foreach (MyGuiControlBase item in m_allControls[type])
			{
				item.Visible = true;
			}
		}

		private void AddJoysticksToComboBox()
		{
			if (m_joystickCombobox == null)
			{
				return;
			}
			int num = 0;
			bool flag = false;
			m_joystickCombobox.AddItem(num++, MyTexts.Get(MyCommonTexts.Disabled));
			foreach (string item in MyInput.Static.EnumerateJoystickNames())
			{
				m_joystickCombobox.AddItem(num, new StringBuilder(item));
				if (MyInput.Static.JoystickInstanceName == item)
				{
					flag = true;
					m_joystickCombobox.SelectItemByIndex(num);
				}
				num++;
			}
			if (!flag)
			{
				m_joystickCombobox.SelectItemByIndex(0);
			}
		}

		private void OnSelectJoystick()
		{
			if (m_joystickCombobox != null)
			{
				MyInput.Static.JoystickInstanceName = ((m_joystickCombobox.GetSelectedIndex() == 0) ? null : m_joystickCombobox.GetSelectedValue().ToString());
				RefreshJoystickControlEnabling();
			}
		}

		private void RefreshJoystickControlEnabling()
		{
			if (m_joystickCombobox == null)
			{
				return;
			}
			bool enabled = m_joystickCombobox.GetSelectedIndex() != 0;
			foreach (MyGuiControlButton joystickButton in m_joystickButtons)
			{
				joystickButton.Enabled = enabled;
			}
			foreach (MyGuiControlButton joystickAxis in m_joystickAxes)
			{
				joystickAxis.Enabled = enabled;
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenOptionsControls";
		}

		public override bool Update(bool hasFocus)
		{
			if (m_controlTypeList.GetSelectedKey() != (int)m_currentControlType)
			{
				DeactivateControls(m_currentControlType);
				m_currentControlType = (MyGuiControlTypeEnum)m_controlTypeList.GetSelectedKey();
				ActivateControls(m_currentControlType);
			}
			if (hasFocus)
			{
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
				{
					OnOkClick(null);
				}
				m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			if (!base.Update(hasFocus))
			{
				return false;
			}
			return true;
		}

		private void OnControlClick(MyGuiControlButton button)
		{
			ControlButtonData controlButtonData = (ControlButtonData)button.UserData;
			MyStringId messageText = MyCommonTexts.AssignControlKeyboard;
			if (controlButtonData.Device == MyGuiInputDeviceEnum.Mouse)
			{
				messageText = MyCommonTexts.AssignControlMouse;
			}
			MyGuiControlAssignKeyMessageBox myGuiControlAssignKeyMessageBox = new MyGuiControlAssignKeyMessageBox(controlButtonData.Device, controlButtonData.Control, messageText);
			myGuiControlAssignKeyMessageBox.Closed += delegate
			{
				RefreshButtonTexts();
			};
			MyGuiSandbox.AddScreen(myGuiControlAssignKeyMessageBox);
		}

		private void OnSecondaryControlClick(MyGuiControlButton button)
		{
			ControlButtonData data = (ControlButtonData)button.UserData;
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextRemoveControlBinding), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
				if (result == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					if (data.Device == MyGuiInputDeviceEnum.Mouse)
					{
						data.Control.SetControl(MyMouseButtonsEnum.None);
					}
					else if (data.Device == MyGuiInputDeviceEnum.Keyboard || data.Device == MyGuiInputDeviceEnum.KeyboardSecond)
					{
						data.Control.SetControl(data.Device, MyKeys.None);
					}
					RefreshButtonTexts();
				}
			}));
		}

		private void OnResetDefaultsClick(MyGuiControlButton sender)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionResetControlsToDefault), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextResetControlsToDefault), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum res)
			{
				if (res == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					MyInput.Static.RevertToDefaultControls();
					DeactivateControls(m_currentControlType);
					AddControls();
					ActivateControls(m_currentControlType);
				}
			}));
		}

		protected override void Canceling()
		{
			MyInput.Static.RevertChanges();
			base.Canceling();
		}

		private void OnCancelClick(MyGuiControlButton sender)
		{
			MyInput.Static.RevertChanges();
			CloseScreen();
		}

		private void OnOkClick(MyGuiControlButton sender)
		{
			CloseScreenAndSave();
		}

		private void CloseScreenAndSave()
		{
			MyInput.Static.SetMouseXInversion(m_invertMouseXCheckbox.IsChecked);
			MyInput.Static.SetMouseYInversion(m_invertMouseYCheckbox.IsChecked);
			MyInput.Static.SetMouseScrollBlockSelectionInversion(m_InvertMouseScrollBlockSelectionCheckbox.IsChecked);
			MyInput.Static.SetMouseSensitivity(m_mouseSensitivitySlider.Value);
			MySandboxGame.Config.ZoomMultiplier = m_zoomMultiplierSlider.Value;
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				if (m_joystickCombobox != null)
				{
					MyInput.Static.JoystickInstanceName = ((m_joystickCombobox.GetSelectedIndex() == 0) ? null : m_joystickCombobox.GetSelectedValue().ToString());
				}
				MyInput.Static.SetJoystickSensitivity(m_joystickSensitivitySlider.Value);
				MyInput.Static.SetJoystickExponent(m_joystickExponentSlider.Value);
				MyInput.Static.SetJoystickDeadzone(m_joystickDeadzoneSlider.Value);
				MyInput.Static.UpdateJoystickChanged();
<<<<<<< HEAD
				int num = (int)m_controllerScheme.GetSelectedKey();
				string gamepadSchemeName = string.Empty;
				if (num >= 0 && num < m_controllerSchemeNames.Count)
				{
					gamepadSchemeName = m_controllerSchemeNames[num];
				}
				MySandboxGame.Config.GamepadSchemeId = num;
				MySandboxGame.Config.GamepadSchemeName = gamepadSchemeName;
=======
				MySandboxGame.Config.GamepadSchemeId = (int)m_gamepadScheme.GetSelectedKey();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyInput.Static.SetJoystickYInversionCharacter(m_invertYCharCheckbox.IsChecked);
				MyInput.Static.SetJoystickYInversionVehicle(m_invertYVehicleCheckbox.IsChecked);
				MySpaceBindingCreator.CreateBindingDefault();
			}
			MyInput.Static.SaveControls(MySandboxGame.Config.ControlsGeneral, MySandboxGame.Config.ControlsButtons);
			MySandboxGame.Config.Save();
			MyScreenManager.RecreateControls();
			CloseScreen();
		}

		private void RefreshButtonTexts()
		{
			RefreshButtonTexts(m_key1Buttons);
			RefreshButtonTexts(m_key2Buttons);
			RefreshButtonTexts(m_mouseButtons);
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				RefreshButtonTexts(m_joystickButtons);
				RefreshButtonTexts(m_joystickAxes);
			}
		}

		private void RefreshButtonTexts(List<MyGuiControlButton> buttons)
		{
			StringBuilder output = null;
			foreach (MyGuiControlButton button in buttons)
			{
				ControlButtonData controlButtonData = (ControlButtonData)button.UserData;
				controlButtonData.Control.AppendBoundButtonNames(ref output, controlButtonData.Device);
				MyControl.AppendUnknownTextIfNeeded(ref output, MyTexts.GetString(MyCommonTexts.UnknownControl_None));
				button.Text = output.ToString();
				output.Clear();
			}
		}
	}
}
