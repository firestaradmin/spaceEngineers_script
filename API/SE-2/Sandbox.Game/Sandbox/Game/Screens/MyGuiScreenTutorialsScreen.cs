using System;
using System.Text;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenTutorialsScreen : MyGuiScreenBase
	{
		private MyGuiControlButton m_okBtn;

		private MyGuiControlCheckbox m_dontShowAgainCheckbox;

		private const string m_linkImgTex = "Textures\\GUI\\link.dds";

		private Action m_okAction;

		public MyGuiScreenTutorialsScreen(Action okAction)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.5264286f, 175f / 262f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenWelcomeScreen.ctor START");
			m_okAction = okAction;
			base.EnabledBackgroundFade = true;
			m_closeOnEsc = true;
			m_drawEvenWithoutFocus = true;
			base.CanHideOthers = true;
			base.CanBeHidden = true;
		}

		public override void LoadContent()
		{
			base.LoadContent();
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			BuildControls();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenTutorialsScreen";
		}

		protected void BuildControls()
		{
			AddCaption("Tutorials", null, new Vector2(0f, 0.003f));
			_ = MySandboxGame.Config.NewsletterCurrentStatus;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(-new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.79f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(-new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.79f);
			Controls.Add(myGuiControlSeparatorList2);
			float num = 0.145f;
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(new Vector2(0.015f, -0.162f + num), new Vector2(0.44f, 0.45f), null, "Blue", 0.76f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			myGuiControlMultilineText.AppendText(MyTexts.GetString(MyCommonTexts.HelpScreen_HelloEngineer), "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.AppendText("\n\n", "Blue", 0.76f, Color.White);
			myGuiControlMultilineText.OnLinkClicked += OnLinkClicked;
			myGuiControlMultilineText.AppendText(MyTexts.GetString(MyCommonTexts.HelpScreen_AccessHelpScreen), "Blue", 0.76f, Color.White);
			bool isJoystickLastUsed = MyInput.Static.IsJoystickLastUsed;
			float num2 = 0.02f;
			float x = -0.205f;
			float num3 = -0.04f;
			if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
			{
				Controls.Add(MakeURLButton(new Vector2(x, num3), MyTexts.GetString(MyCommonTexts.HelpScreen_Introduction), isJoystickLastUsed, 0));
				num3 += num2;
			}
			Controls.Add(MakeURLButton(new Vector2(x, num3), MyTexts.GetString(MyCommonTexts.HelpScreen_BasicControls), isJoystickLastUsed, 1));
			num3 += num2;
			Controls.Add(MakeURLButton(new Vector2(x, num3), MyTexts.GetString(MyCommonTexts.HelpScreen_PossibilitiesWithinTheGameModes), isJoystickLastUsed, 2));
			num3 += num2;
			Controls.Add(MakeURLButton(new Vector2(x, num3), MyTexts.GetString(MyCommonTexts.HelpScreen_DrillingRefiningAssemblingSurvival), isJoystickLastUsed, 3));
			num3 += num2;
			Controls.Add(MakeURLButton(new Vector2(x, num3), MyTexts.GetString(MyCommonTexts.HelpScreen_BuildingYour1stShipCreative), isJoystickLastUsed, 4));
			num3 += num2;
			Controls.Add(MakeURLButton(new Vector2(x, num3), MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival), isJoystickLastUsed, 9));
			num3 += num2;
			Controls.Add(MakeURLButton(new Vector2(x, num3), MyTexts.GetString(MyCommonTexts.ExperimentalMode), isJoystickLastUsed, 5));
			num3 += num2;
			Controls.Add(MakeURLButton(new Vector2(x, num3), MyTexts.GetString(MyCommonTexts.HelpScreen_BuildingYour1stGroundVehicle), isJoystickLastUsed, 6));
			num3 += num2;
			Controls.Add(MakeURLButton(new Vector2(x, num3), MyTexts.GetString(MyCommonTexts.HelpScreen_OtherAdviceClosingThoughts), isJoystickLastUsed, 8));
			m_dontShowAgainCheckbox = new MyGuiControlCheckbox(new Vector2(0.08f, 0.017f + num), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			Controls.Add(m_dontShowAgainCheckbox);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(0.195f, 0.047f + num), null, MyTexts.GetString(MyCommonTexts.HelpScreen_DontShowAgain));
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			Controls.Add(myGuiControlLabel);
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			m_okBtn = new MyGuiControlButton(new Vector2(0f, 0.155f + num), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnOKButtonClick);
			m_okBtn.Enabled = true;
			m_okBtn.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
			Controls.Add(myGuiControlMultilineText);
			Controls.Add(m_okBtn);
			base.CloseButtonEnabled = true;
		}

		private MyGuiControlButton MakeURLButton(Vector2 position, string text, bool isForController, int tutorialPart)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, null, MyGuiConstants.BACK_BUTTON_BACKGROUND_COLOR, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder(text), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, delegate
			{
				MyGuiSandbox.OpenUrl(MyGuiScreenHelpSpace.GetTutorialPartUrl(tutorialPart, isForController), UrlOpenMode.SteamOrExternalWithConfirm);
			});
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlButton.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlButton.Alpha = 1f;
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
			myGuiControlButton.Size = new Vector2(0.22f, 0.13f);
			myGuiControlButton.TextScale = 0.736f;
			myGuiControlButton.CanHaveFocus = true;
			myGuiControlButton.ColorMask = Color.PowderBlue;
			Vector2 vector = MyGuiManager.MeasureString(myGuiControlButton.TextFont, new StringBuilder(myGuiControlButton.Text), myGuiControlButton.TextScale);
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage
			{
				Size = new Vector2(0.0128f, 0.0176f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = myGuiControlButton.Position,
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage.PositionX += vector.X + 0.005f;
			myGuiControlImage.SetTexture("Textures\\GUI\\link.dds");
			myGuiControlImage.ColorMask = Color.White;
			myGuiControlImage.Visible = true;
			Controls.Add(myGuiControlImage);
			return myGuiControlButton;
		}

		private void OnOKButtonClick(object sender)
		{
			MySandboxGame.Config.FirstTimeTutorials = !m_dontShowAgainCheckbox.IsChecked;
			MySandboxGame.Config.Save();
			CloseScreen();
			m_okAction();
		}

		protected override void Canceling()
		{
			m_okAction();
			base.Canceling();
		}

		private void OnLinkClicked(MyGuiControlBase sender, string url)
		{
			MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);
		}
	}
}
