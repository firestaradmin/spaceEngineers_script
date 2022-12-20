using System;
using System.Collections.Generic;
using System.Text;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Networking;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlScreenSwitchPanel : MyGuiControlParent
	{
		private List<Action> m_screenSwithingActions = new List<Action>();

		private List<bool> m_screenEnabled = new List<bool>();

		private int m_activeScreenIndex = -1;

		private const int PAGE_COUNT = 3;

		private Vector2 m_gamepadHelpTopLeftPos;

		public MyGuiControlScreenSwitchPanel(MyGuiScreenBase owner, StringBuilder ownerDescription, bool displayTabScenario = true, bool displayTabWorkshop = true, bool displayTabCustom = true)
		{
			m_screenSwithingActions.Add(OpenCampaignScreen);
			m_screenSwithingActions.Add(OpenWorkshopScreen);
			m_screenSwithingActions.Add(OpenCustomWorldScreen);
			m_screenEnabled.Add(displayTabScenario);
			m_screenEnabled.Add(displayTabWorkshop);
			m_screenEnabled.Add(displayTabCustom);
			Vector2 value = new Vector2(0.002f, 0.05f);
			Vector2 vector = new Vector2(50f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 vector2 = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			MyGuiControlMultilineText control = new MyGuiControlMultilineText
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = new Vector2(0.002f, 0.13f),
				Size = new Vector2(owner.Size.Value.X - 0.1f, 0.07f),
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Text = ownerDescription,
				Font = "Blue"
			};
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(value, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MyCommonTexts.ScreenCaptionNewGame), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Campaign), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: OnCampaignButtonClick)
			{
				CanHaveFocus = false
			};
			myGuiControlButton.Enabled = displayTabScenario;
			value.X += myGuiControlButton.Size.X + MyGuiConstants.GENERIC_BUTTON_SPACING.X;
			MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(value, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MyCommonTexts.ScreenCaptionWorkshop), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewGame_WorkshopContent), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: OnWorkshopButtonClick)
			{
				CanHaveFocus = false
			};
			myGuiControlButton2.Enabled = displayTabWorkshop;
			value.X += myGuiControlButton2.Size.X + MyGuiConstants.GENERIC_BUTTON_SPACING.X;
			MyGuiControlButton myGuiControlButton3 = new MyGuiControlButton(value, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MyCommonTexts.ScreenCaptionCustomWorld), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewGame_CustomGame), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: OnCustomWorldButtonClick, cueEnum: GuiSounds.MouseClick, buttonScale: 1f, buttonIndex: null, activateOnMouseRelease: false, isAutoscaleEnabled: true)
			{
				CanHaveFocus = false
			};
			myGuiControlButton3.Enabled = displayTabCustom;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0.0305f), owner.Size.Value.X - 2f * vector2.X);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0.098f), owner.Size.Value.X - 2f * vector2.X);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0.166f), owner.Size.Value.X - 2f * vector2.X);
			if (owner is MyGuiScreenNewGame)
			{
				owner.FocusedControl = myGuiControlButton;
				myGuiControlButton.Checked = true;
				myGuiControlButton.Selected = true;
				myGuiControlButton.Name = "CampaignButton";
				m_activeScreenIndex = 0;
			}
			else if (owner is MyGuiScreenWorldSettings)
			{
				owner.FocusedControl = myGuiControlButton3;
				myGuiControlButton3.Checked = true;
				myGuiControlButton3.Selected = true;
				m_activeScreenIndex = 2;
			}
			else if ((owner is MyGuiScreenLoadSubscribedWorld || owner is MyGuiScreenNewWorkshopGame) && myGuiControlButton2 != null)
			{
				owner.FocusedControl = myGuiControlButton2;
				myGuiControlButton2.Checked = true;
				myGuiControlButton2.Selected = true;
				m_activeScreenIndex = 1;
			}
			base.Controls.Add(control);
			base.Controls.Add(myGuiControlSeparatorList);
			base.Controls.Add(myGuiControlButton);
			base.Controls.Add(myGuiControlButton3);
			if (myGuiControlButton2 != null)
			{
				base.Controls.Add(myGuiControlButton2);
			}
			base.Position = -owner.Size.Value / 2f + new Vector2(vector2.X, vector.Y);
			base.Size = new Vector2(1f, 0.2f);
<<<<<<< HEAD
=======
			owner.Controls.Add(this);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_gamepadHelpTopLeftPos = myGuiControlButton.GetPositionAbsoluteTopLeft();
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
				Vector2 value = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.ToolbarButton).SizeOverride.Value;
				Vector2 gamepadHelpTopLeftPos = m_gamepadHelpTopLeftPos;
				gamepadHelpTopLeftPos.Y += value.Y / 2f;
				gamepadHelpTopLeftPos.X -= value.X / 6f;
				Vector2 gamepadHelpTopLeftPos2 = m_gamepadHelpTopLeftPos;
				gamepadHelpTopLeftPos2.Y = gamepadHelpTopLeftPos.Y;
				Color value2 = MyGuiControlBase.ApplyColorMaskModifiers(MyGuiConstants.LABEL_TEXT_COLOR, enabled: true, transitionAlpha);
				gamepadHelpTopLeftPos2.X += 3f * value.X + value.X / 8f;
				MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Left), gamepadHelpTopLeftPos, 1f, value2, drawAlign);
				MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Right), gamepadHelpTopLeftPos2, 1f, value2, drawAlign);
			}
		}

		private void OnCampaignButtonClick(MyGuiControlButton myGuiControlButton)
		{
			OpenCampaignScreen();
		}

		private void OpenCampaignScreen()
		{
			MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
			if (!(screenWithFocus is MyGuiScreenNewGame))
			{
				SeamlesslyChangeScreen(screenWithFocus, new MyGuiScreenNewGame(m_screenEnabled[0], m_screenEnabled[1], m_screenEnabled[2]));
			}
		}

		private void OnCustomWorldButtonClick(MyGuiControlButton myGuiControlButton)
		{
			OpenCustomWorldScreen();
		}

		private void OpenCustomWorldScreen()
		{
			MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
			if (!(screenWithFocus is MyGuiScreenWorldSettings))
			{
				SeamlesslyChangeScreen(screenWithFocus, new MyGuiScreenWorldSettings(m_screenEnabled[0], m_screenEnabled[1], m_screenEnabled[2]));
			}
		}

		private void OnWorkshopButtonClick(MyGuiControlButton myGuiControlButton)
		{
			OpenWorkshopScreen();
		}

		private void OpenWorkshopScreen()
		{
			MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
			if (!(screenWithFocus is MyGuiScreenNewWorkshopGame))
			{
				SeamlesslyChangeScreen(screenWithFocus, new MyGuiScreenNewWorkshopGame(m_screenEnabled[0], m_screenEnabled[1], m_screenEnabled[2]));
			}
		}

		private static void SeamlesslyChangeScreen(MyGuiScreenBase focusedScreen, MyGuiScreenBase exchangedFor)
		{
			focusedScreen.SkipTransition = true;
			focusedScreen.CloseScreen();
			exchangedFor.SkipTransition = true;
			MyScreenManager.AddScreenNow(exchangedFor);
		}

		private void SwitchToNextScreen(bool positiveDirection = true)
		{
			if (m_activeScreenIndex >= 0 && m_activeScreenIndex < m_screenSwithingActions.Count)
			{
				Action nextAction = GetNextAction(positiveDirection, m_activeScreenIndex);
				if (CheckWorkshopConsent(positiveDirection, nextAction))
				{
					nextAction();
				}
			}
		}

		private Action GetNextAction(bool positiveDirection, int activeScreenIndex)
		{
			if (activeScreenIndex < 0 || activeScreenIndex >= m_screenSwithingActions.Count)
			{
				return null;
			}
			if (positiveDirection)
			{
				int num = activeScreenIndex;
				bool flag = false;
				do
				{
					num = (num + 1) % m_screenSwithingActions.Count;
					if (m_screenEnabled[num])
					{
						flag = true;
						break;
					}
				}
				while (num != activeScreenIndex);
				if (flag)
				{
					return m_screenSwithingActions[num];
				}
			}
			else
			{
				int num2 = activeScreenIndex;
				bool flag2 = false;
				do
				{
					num2 = (num2 + m_screenSwithingActions.Count - 1) % m_screenSwithingActions.Count;
					if (m_screenEnabled[num2])
					{
						flag2 = true;
						break;
					}
				}
				while (num2 != activeScreenIndex);
				if (flag2)
				{
					return m_screenSwithingActions[num2];
				}
			}
			return null;
		}

		private bool CheckWorkshopConsent(bool positiveDirection, Action openingAction)
		{
			if (openingAction == new Action(OpenWorkshopScreen) && !MyGameService.AtLeastOneUGCServiceConsented)
			{
				Action nextAction = GetNextAction(positiveDirection, m_activeScreenIndex);
				Action nextAction2 = GetNextAction(positiveDirection, m_screenSwithingActions.IndexOf(nextAction));
				MyModIoConsentViewModel viewModel = new MyModIoConsentViewModel(nextAction, nextAction2);
				ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen(viewModel);
				return false;
			}
			return true;
		}

		public override MyGuiControlBase HandleInput()
		{
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT))
			{
				SwitchToNextScreen(positiveDirection: false);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
			{
				SwitchToNextScreen();
			}
			return base.HandleInput();
		}
	}
}
