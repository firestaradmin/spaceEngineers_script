using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems.Chat;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.Animation;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenHelpSpace : MyGuiScreenBase
	{
		protected class MyHackyQuestLogComparer : IComparer<KeyValuePair<string, bool>>
		{
			int IComparer<KeyValuePair<string, bool>>.Compare(KeyValuePair<string, bool> x, KeyValuePair<string, bool> y)
			{
				return string.Compare(x.Key, y.Key);
			}
		}

		private enum HelpPageEnum
		{
			Tutorials,
			BasicControls,
			AdvancedControls,
			Controller,
			ControllerAdvanced,
			Chat,
			Support,
			IngameHelp,
			Welcome,
			ReportIssue
		}

		private static readonly MyHackyQuestLogComparer m_hackyQuestComparer = new MyHackyQuestLogComparer();

		public MyGuiControlList contentList;

		private HelpPageEnum m_currentPage;

		private MyGuiControlMultilineText m_screenDescText;

		private MyGuiControlButton m_backButton;

		public static readonly List<string> TutorialPartsUrlsKeyboard = new List<string> { "https://www.youtube.com/watch?v=wHa54ebUluE&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=oDxa3vBbddE&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=-pWmU06oT4s&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=OsULjlQYWyg&index=4&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=XvpNC9lkLwQ&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=rCrnRxcwxKI&index=6&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=23YHvmYLAuk&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=q3jyDhIsMFw&index=8&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=JvMbRMw_a2Q&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=__G10lQXmXQ" };

		public static readonly List<string> TutorialPartsUrlsContoller = new List<string> { "https://www.youtube.com/watch?v=wHa54ebUluE&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=qdLi5V5dGH8", "https://www.youtube.com/watch?v=-pWmU06oT4s&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=OsULjlQYWyg&index=4&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=7_7bBwckAuw", "https://www.youtube.com/watch?v=rCrnRxcwxKI&index=6&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=23YHvmYLAuk&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=q3jyDhIsMFw&index=8&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=JvMbRMw_a2Q&list=PL1Lkz--s-OxuBbVZjkYiDpb4QguXXEy8O", "https://www.youtube.com/watch?v=__G10lQXmXQ" };

		public MyGuiScreenHelpSpace()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.97f, 0.97f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			m_currentPage = HelpPageEnum.Tutorials;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			int num = -1;
			for (int i = 0; i < Controls.Count; i++)
			{
				if (Controls[i].HasFocus)
				{
					num = i;
					_ = (Controls[i] as MyGuiControlTable)?.SelectedRowIndex;
				}
			}
			base.RecreateControls(constructor);
			AddCaption(MyTexts.GetString(MyCommonTexts.HelpScreenHeader), null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.87f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.87f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.87f / 2f, m_size.Value.Y / 2f - 0.847f), m_size.Value.X * 0.87f);
			Controls.Add(myGuiControlSeparatorList);
			StringBuilder output = new StringBuilder();
			MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN).AppendBoundButtonNames(ref output, ",", MyInput.Static.GetUnassignedName(), includeSecondary: false);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(MyTexts.GetString(MyCommonTexts.HelpScreen_Description), output);
			StringBuilder contents = stringBuilder;
			m_screenDescText = new MyGuiControlMultilineText(new Vector2(-0.42f, 0.381f), new Vector2(0.4f, 0.2f), null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, contents);
			m_screenDescText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_screenDescText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_screenDescText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			Controls.Add(m_screenDescText);
			m_backButton = new MyGuiControlButton(new Vector2(0.336f, 0.415f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.ScreenMenuButtonBack), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Close));
			m_backButton.ButtonClicked += backButton_ButtonClicked;
			Controls.Add(m_backButton);
			MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2(-0.422f, -0.39f), new Vector2(0.211f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = myGuiControlPanel.Position + new Vector2(0.01f, 0.005f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.HelpScreen_HomeSelectCategory)
			};
			Controls.Add(myGuiControlPanel);
			Controls.Add(control);
			MyGuiControlTable myGuiControlTable = new MyGuiControlTable
			{
				Position = myGuiControlPanel.Position + new Vector2(0f, 0.033f),
				Size = new Vector2(0.211f, 0.5f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				ColumnsCount = 1,
				VisibleRowsCount = 20,
				HeaderVisible = false
			};
			myGuiControlTable.SetCustomColumnWidths(new float[1] { 1f });
			myGuiControlTable.ItemSelected += OnTableItemSelected;
			Controls.Add(myGuiControlTable);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_Tutorials), HelpPageEnum.Tutorials);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_BasicControls), HelpPageEnum.BasicControls);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedControls), HelpPageEnum.AdvancedControls);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_Gamepad), HelpPageEnum.Controller);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_GamepadAdvanced), HelpPageEnum.ControllerAdvanced);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_Chat), HelpPageEnum.Chat);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_Support), HelpPageEnum.Support);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_IngameHelp), HelpPageEnum.IngameHelp);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_Welcome), HelpPageEnum.Welcome);
			AddHelpScreenCategory(myGuiControlTable, MyTexts.GetString(MyCommonTexts.HelpScreen_ReportIssue), HelpPageEnum.ReportIssue);
			myGuiControlTable.SelectedRow = myGuiControlTable.GetRow((int)m_currentPage);
			contentList = new MyGuiControlList(myGuiControlPanel.Position + new Vector2(0.22f, 0f), new Vector2(0.624f, 0.74f));
			contentList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			contentList.VisualStyle = MyGuiControlListStyleEnum.Dark;
			Controls.Add(contentList);
			switch (m_currentPage)
			{
			case HelpPageEnum.Support:
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_SupportDescription)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\KSWLink.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_SupportLinkUserResponse), "https://support.keenswh.com/"));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\KSWLink.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_SupportLinkForum), "http://forums.keenswh.com/"));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_SupportContactDescription)));
				contentList.Controls.Add(AddLinkPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_SupportContact), "mailto:support@keenswh.com"));
				break;
			case HelpPageEnum.Tutorials:
			{
				contentList.Controls.Add(AddSeparatorPanel());
				bool isJoystickLastUsed = MyInput.Static.IsJoystickLastUsed;
				if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
				{
					contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\Intro.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_Introduction), GetTutorialPartUrl(0, isJoystickLastUsed)));
					contentList.Controls.Add(AddSeparatorPanel());
				}
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\BasicControls.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_BasicControls), GetTutorialPartUrl(1, isJoystickLastUsed)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\GameModePossibilities.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_PossibilitiesWithinTheGameModes), GetTutorialPartUrl(2, isJoystickLastUsed)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\DrillingRefiningAssembling.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_DrillingRefiningAssemblingSurvival), GetTutorialPartUrl(3, isJoystickLastUsed)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\Building1stShip.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_BuildingYour1stShipCreative), GetTutorialPartUrl(4, isJoystickLastUsed)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\Survival.dds", MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival), GetTutorialPartUrl(9, isJoystickLastUsed)));
				contentList.Controls.Add(AddSeparatorPanel());
				if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
				{
					contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\ExperimentalMode.dds", MyTexts.GetString(MyCommonTexts.ExperimentalMode), GetTutorialPartUrl(5, isJoystickLastUsed)));
					contentList.Controls.Add(AddSeparatorPanel());
				}
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\Building1stVehicle.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_BuildingYour1stGroundVehicle), GetTutorialPartUrl(6, isJoystickLastUsed)));
				contentList.Controls.Add(AddSeparatorPanel());
				if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
				{
					contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\SteamWorkshopBlueprints.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_SteamWorkshopAndBlueprints), GetTutorialPartUrl(7, isJoystickLastUsed)));
					contentList.Controls.Add(AddSeparatorPanel());
				}
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\OtherAdvice.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_OtherAdviceClosingThoughts), GetTutorialPartUrl(8, isJoystickLastUsed)));
				contentList.Controls.Add(AddSeparatorPanel());
				if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
				{
					contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\SteamLink.dds", string.Format(MyTexts.GetString(MyCommonTexts.HelpScreen_TutorialsLinkSteam), MyGameService.Service.ServiceName), "http://steamcommunity.com/app/244850/guides"));
					contentList.Controls.Add(AddSeparatorPanel());
				}
				contentList.Controls.Add(AddImageLinkPanel("Textures\\GUI\\HelpScreen\\WikiLink.dds", MyTexts.GetString(MyCommonTexts.HelpScreen_TutorialsLinkWiki), "http://spaceengineerswiki.com/Main_Page"));
				contentList.Controls.Add(AddSeparatorPanel());
				break;
			}
			case HelpPageEnum.BasicControls:
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_BasicDescription)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeNavigation) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddControlsByType(MyGuiControlTypeEnum.Navigation);
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeSystems1) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddControlsByType(MyGuiControlTypeEnum.Systems1);
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("Ctrl + " + GetControlButtonName(MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.ControlName_RelativeDampening)));
				contentList.Controls.Add(AddKeyPanel("Ctrl + " + GetControlButtonName(MyControlsSpace.TOGGLE_REACTORS), MyTexts.GetString(MySpaceTexts.ControlName_PowerSwitchOnOff_All)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeSystems2) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddControlsByType(MyGuiControlTypeEnum.Systems2);
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeSystems3) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddControlsByType(MyGuiControlTypeEnum.Systems3);
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeToolsOrWeapons) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddControlsByType(MyGuiControlTypeEnum.ToolsOrWeapons);
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeView) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddControlsByType(MyGuiControlTypeEnum.Spectator);
				contentList.Controls.Add(AddTinySpacePanel());
				break;
			case HelpPageEnum.AdvancedControls:
			{
				StringBuilder output2 = null;
				MyInput.Static.GetGameControl(MyControlsSpace.CUBE_COLOR_CHANGE).AppendBoundButtonNames(ref output2, ", ", MyInput.Static.GetUnassignedName());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedDescription)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedGeneral)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("F10", MyTexts.Get(MySpaceTexts.OpenBlueprints).ToString()));
				contentList.Controls.Add(AddKeyPanel("SHIFT + F10", MyTexts.Get(MySpaceTexts.OpenSpawnScreen).ToString()));
				contentList.Controls.Add(AddKeyPanel("ALT + F10", MyTexts.Get(MySpaceTexts.OpenAdminScreen).ToString()));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("F5", MyTexts.GetString(MyCommonTexts.ControlDescQuickLoad)));
				contentList.Controls.Add(AddKeyPanel("SHIFT + F5", MyTexts.GetString(MyCommonTexts.ControlDescQuickSave)));
				contentList.Controls.Add(AddKeyPanel("CTRL + H", MyTexts.GetString(MySpaceTexts.ControlDescNetgraph)));
				contentList.Controls.Add(AddKeyPanel("F3", MyTexts.GetString(MyCommonTexts.ControlDescPlayersList)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedGridsAndBlueprints)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("CTRL + B", MyTexts.Get(MySpaceTexts.CreateManageBlueprints).ToString()));
				contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.MouseWheel), MyTexts.GetString(MyCommonTexts.ControlName_ChangeBlockVariants)));
				contentList.Controls.Add(AddKeyPanel("Ctrl + " + MyTexts.GetString(MyCommonTexts.MouseWheel), MyTexts.GetString(MyCommonTexts.ControlDescCopyPasteMove)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("CTRL + C", MyTexts.Get(MySpaceTexts.CopyObject).ToString()));
				contentList.Controls.Add(AddKeyPanel("CTRL + SHIFT + C", MyTexts.Get(MySpaceTexts.CopyObjectDetached).ToString()));
				contentList.Controls.Add(AddKeyPanel("CTRL + V", MyTexts.Get(MySpaceTexts.PasteObject).ToString()));
				contentList.Controls.Add(AddKeyPanel("CTRL + X", MyTexts.Get(MySpaceTexts.CutObject).ToString()));
				contentList.Controls.Add(AddKeyPanel("CTRL + Del", MyTexts.Get(MySpaceTexts.DeleteObject).ToString()));
				contentList.Controls.Add(AddKeyPanel("CTRL + ALT + E", MyTexts.GetString(MyCommonTexts.ControlDescExportModel)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedCamera)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("Alt + " + MyTexts.Get(MyCommonTexts.MouseWheel).ToString(), MyTexts.Get(MySpaceTexts.ControlDescZoom).ToString()));
				contentList.Controls.Add(AddKeyPanel(GetControlButtonName(MyControlsSpace.SWITCH_LEFT), GetControlButtonDescription(MyControlsSpace.SWITCH_LEFT)));
				contentList.Controls.Add(AddKeyPanel(GetControlButtonName(MyControlsSpace.SWITCH_RIGHT), GetControlButtonDescription(MyControlsSpace.SWITCH_RIGHT)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedColorPicker)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(GetControlButtonName(MyControlsSpace.COLOR_PICKER), GetControlButtonDescription(MyControlsSpace.COLOR_PICKER)));
				contentList.Controls.Add(AddKeyPanel("SHIFT + P", MyTexts.GetString(MySpaceTexts.PickColorFromCube)));
				contentList.Controls.Add(AddKeyPanel(output2.ToString(), MyTexts.GetString(MySpaceTexts.ControlDescHoldToColor)));
				contentList.Controls.Add(AddKeyPanel("CTRL + " + output2.ToString(), MyTexts.GetString(MySpaceTexts.ControlDescMediumBrush)));
				contentList.Controls.Add(AddKeyPanel("SHIFT + " + output2.ToString(), MyTexts.GetString(MySpaceTexts.ControlDescLargeBrush)));
				contentList.Controls.Add(AddKeyPanel("CTRL + SHIFT + " + output2.ToString(), MyTexts.GetString(MySpaceTexts.ControlDescWholeBrush)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedVoxelHands)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(GetControlButtonName(MyControlsSpace.VOXEL_HAND_SETTINGS), MyTexts.GetString(MyCommonTexts.ControlDescOpenVoxelHandSettings)));
				contentList.Controls.Add(AddKeyPanel("[", MyTexts.GetString(MyCommonTexts.ControlDescNextVoxelMaterial)));
				contentList.Controls.Add(AddKeyPanel("]", MyTexts.GetString(MyCommonTexts.ControlDescPreviousVoxelMaterial)));
				contentList.Controls.Add(AddKeyPanel("MMB", MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPaint)));
				contentList.Controls.Add(AddKeyPanel("CTRL + " + MyTexts.GetString(MyCommonTexts.RightMouseButton), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRevert)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedSpectator)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("CTRL + SPACE", MyTexts.GetString(MyCommonTexts.ControlDescMoveToSpectator)));
				contentList.Controls.Add(AddKeyPanel("SHIFT + " + MyTexts.GetString(MyCommonTexts.MouseWheel), MyTexts.GetString(MySpaceTexts.ControlDescSpectatorSpeed)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_BuildPlanner)));
				contentList.Controls.Add(AddTinySpacePanel());
				StringBuilder output3 = null;
				MyInput.Static.GetGameControl(MyControlsSpace.BUILD_PLANNER).AppendBoundButtonNames(ref output3, ", ", MyInput.Static.GetUnassignedName());
				contentList.Controls.Add(AddKeyPanel(output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_Withdraw)));
				contentList.Controls.Add(AddKeyPanel("ALT + CTRL + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_WithdrawKeep)));
				contentList.Controls.Add(AddKeyPanel("CTRL + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_Withdraw10Keep)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("SHIFT + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_PutToProduction)));
				contentList.Controls.Add(AddKeyPanel("SHIFT + CTRL + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_Put10ToProduction)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel("ALT + " + output3.ToString(), MyTexts.GetString(MySpaceTexts.BuildPlanner_DepositAll)));
				contentList.Controls.Add(AddTinySpacePanel());
				break;
			}
			case HelpPageEnum.Controller:
			{
				int gamepadSchemeId = MySandboxGame.Config.GamepadSchemeId;
				if (gamepadSchemeId != 0 && gamepadSchemeId == 1)
				{
					contentList.Controls.Add(new MyGuiControlGamepadBindings(BindingType.Character, ControlScheme.Alternative));
					contentList.Controls.Add(new MyGuiControlGamepadBindings(BindingType.Jetpack, ControlScheme.Alternative));
					contentList.Controls.Add(new MyGuiControlGamepadBindings(BindingType.Ship, ControlScheme.Alternative));
				}
				else
				{
					contentList.Controls.Add(new MyGuiControlGamepadBindings(BindingType.Character, ControlScheme.Default));
					contentList.Controls.Add(new MyGuiControlGamepadBindings(BindingType.Jetpack, ControlScheme.Default));
					contentList.Controls.Add(new MyGuiControlGamepadBindings(BindingType.Ship, ControlScheme.Default));
				}
				break;
			}
			case HelpPageEnum.ControllerAdvanced:
			{
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_AdvancedDescription)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerShipControl)));
				contentList.Controls.Add(AddTinySpacePanel());
				int gamepadSchemeId = MySandboxGame.Config.GamepadSchemeId;
				if (gamepadSchemeId != 0 && gamepadSchemeId == 1)
				{
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.FAKE_MOVEMENT_H), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHorizontalMover_Forward), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.FAKE_MOVEMENT_V), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerVerticalMover_Up), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_RB_V), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING_RELATIVE), MyTexts.GetString(MySpaceTexts.Dampeners_Relative), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.WHEEL_JUMP), MyTexts.GetString(MySpaceTexts.BlockActionTitle_Jump), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.TOGGLE_REACTORS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Reactors), Color.White, isGamepadKey: true));
<<<<<<< HEAD
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.TOGGLE_REACTORS_ALL), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Reactors_All), Color.White, isGamepadKey: true));
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOOLBAR_NEXT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleShipToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOOLBAR_PREVIOUS), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleShipToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(string.Format("{0} + {1} + {2}", "\ue005", "\ue006", MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", null)), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.ACTIVE_CONTRACT_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Contracts), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOGGLE_HUD), MyTexts.GetString(MySpaceTexts.HelpScreen_ToggleHud), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.CHAT_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Chat), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), Color.White, isGamepadKey: true));
				}
				else
				{
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL) + " + " + MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRoll), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING_RELATIVE), MyTexts.GetString(MySpaceTexts.Dampeners_Relative), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.WHEEL_JUMP), MyTexts.GetString(MySpaceTexts.BlockActionTitle_Jump), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.TOGGLE_REACTORS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Reactors), Color.White, isGamepadKey: true));
<<<<<<< HEAD
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.TOGGLE_REACTORS_ALL), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Reactors_All), Color.White, isGamepadKey: true));
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOOLBAR_NEXT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleShipToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOOLBAR_PREVIOUS), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleShipToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(string.Format("{0} + {1} + {2}", "\ue005", "\ue006", MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", null)), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.ACTIVE_CONTRACT_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Contracts), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.TOGGLE_HUD), MyTexts.GetString(MySpaceTexts.HelpScreen_ToggleHud), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.CHAT_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Chat), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), Color.White, isGamepadKey: true));
				}
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.BLUEPRINTS_MENU), MyTexts.GetString(MySpaceTexts.BlueprintsScreen), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.WARNING_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Warnings), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.BROADCASTING), MyTexts.GetString(MySpaceTexts.ControlName_Broadcasting), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.LOOKAROUND) + " + " + MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerLookAround), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerLookAround_PassengerSeat), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerTurretControl)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerLookAround), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCharacterControl)));
				contentList.Controls.Add(AddTinySpacePanel());
				gamepadSchemeId = MySandboxGame.Config.GamepadSchemeId;
				if (gamepadSchemeId != 0 && gamepadSchemeId == 1)
				{
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_RB_V), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING_RELATIVE), MyTexts.GetString(MySpaceTexts.Dampeners_Relative), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.HELMET), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Helmet), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.COLOR_TOOL), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorTool), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.CONSUME_HEALTH), MyTexts.GetString(MySpaceTexts.DisplayName_Item_Medkit), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(string.Format("{0} + {1} + {2}", "\ue005", "\ue006", MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", null)), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.FAKE_RB_LS_H), MyTexts.GetString(MySpaceTexts.HelpScreen_Strafe), Color.White, isGamepadKey: true));
				}
				else
				{
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING_RELATIVE), MyTexts.GetString(MySpaceTexts.Dampeners_Relative), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.HELMET), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Helmet), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.COLOR_TOOL), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorTool), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.CONSUME_HEALTH), MyTexts.GetString(MySpaceTexts.DisplayName_Item_Medkit), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(string.Format("{0} + {1} + {2}", "\ue005", "\ue006", MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", null)), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), Color.White, isGamepadKey: true));
				}
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.BLUEPRINTS_MENU), MyTexts.GetString(MySpaceTexts.BlueprintsScreen), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.WARNING_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Warnings), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.BROADCASTING), MyTexts.GetString(MySpaceTexts.ControlName_Broadcasting), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerJetpackControl)));
				contentList.Controls.Add(AddTinySpacePanel());
				gamepadSchemeId = MySandboxGame.Config.GamepadSchemeId;
				if (gamepadSchemeId != 0 && gamepadSchemeId == 1)
				{
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.FAKE_MOVEMENT_H), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHorizontalMover_Forward), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.FAKE_MOVEMENT_V), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerVerticalMover_Up), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_RB_V), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING_RELATIVE), MyTexts.GetString(MySpaceTexts.Dampeners_Relative), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.HELMET), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Helmet), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.COLOR_TOOL), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorTool), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.CONSUME_HEALTH), MyTexts.GetString(MySpaceTexts.DisplayName_Item_Medkit), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(string.Format("{0} + {1} + {2}", "\ue005", "\ue006", MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", null)), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), Color.White, isGamepadKey: true));
				}
				else
				{
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.ROLL) + " + " + MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_ROTATION", null), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRoll), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING), MyTexts.GetString(MySpaceTexts.Dampeners), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.DAMPING_RELATIVE), MyTexts.GetString(MySpaceTexts.Dampeners_Relative), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.HELMET), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Helmet), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.HEADLIGHTS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Lights), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.CAMERA_MODE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_CameraMode), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.COLOR_TOOL), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorTool), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCycleEmoteToolbar), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.CONSUME_HEALTH), MyTexts.GetString(MySpaceTexts.DisplayName_Item_Medkit), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.ADMIN_MENU), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(string.Format("{0} + {1} + {2}", "\ue005", "\ue006", MyControllerHelper.ButtonTextEvaluator.TokenEvaluate("AXIS_DPAD", null)), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerEmoteToolbarActions), Color.White, isGamepadKey: true));
					contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.FAKE_CAMERA_ZOOM), MyTexts.GetString(MySpaceTexts.HelpScreen_ZoomCamera), Color.White, isGamepadKey: true));
				}
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.BLUEPRINTS_MENU), MyTexts.GetString(MySpaceTexts.BlueprintsScreen), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_BASE, MyControlsSpace.WARNING_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Warnings), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.BROADCASTING), MyTexts.GetString(MySpaceTexts.ControlName_Broadcasting), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_Tools)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.ACTIVE_CONTRACT_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Contracts), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.TOGGLE_HUD), MyTexts.GetString(MySpaceTexts.HelpScreen_ToggleHud), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.CHAT_SCREEN), MyTexts.GetString(MySpaceTexts.HelpScreen_Chat), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_TOOLS, MyControlsSpace.PROGRESSION_MENU), MyTexts.GetString(MySpaceTexts.HelpScreen_Progression), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCharacterSurvival)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.TOGGLE_SIGNALS), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ToggleSignals), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCharacterCreative)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BASE, MyControlsSpace.VOXEL_SELECT_SPHERE), MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_EquipVoxelhand), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.BuildPlanner)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.BUILD_PLANNER_DEPOSIT_ORE), MyTexts.GetString(MySpaceTexts.BuildPlanner_DepositAll), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.BUILD_PLANNER_ADD_COMPONNETS), MyTexts.GetString(MySpaceTexts.BuildPlanner_PutToProduction), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.BUILD_PLANNER_WITHDRAW_COMPONENTS), MyTexts.GetString(MySpaceTexts.BuildPlanner_Withdraw), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.TERMINAL), MyTexts.GetString(MySpaceTexts.TerminalAccess), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.Spectator)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_LS), MyTexts.GetString(MySpaceTexts.Spectator_HorizontalMovement), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_LB_ROTATION_H), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRoll), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_RS), MyTexts.GetString(MySpaceTexts.Spectator_Rotation), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_LS_PRESS), MyTexts.GetString(MySpaceTexts.Spectator_BlockRadialMenu), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.FAKE_RS_PRESS), MyTexts.GetString(MySpaceTexts.Spectator_SystemRadialMenu), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_FOCUS_PLAYER), MyTexts.GetString(MySpaceTexts.Spectator_FocusPlayer), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_PLAYER_CONTROL), MyTexts.GetString(MySpaceTexts.Spectator_PlayerControl), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_LOCK), MyTexts.GetString(MyCommonTexts.ControlName_SpectatorLock), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_TELEPORT), MyTexts.GetString(MySpaceTexts.Spectator_Teleport), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_SPEED_BOOST), MyTexts.GetString(MySpaceTexts.Spectator_SpeedBoost), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_CHANGE_SPEED_UP), MyTexts.GetString(MySpaceTexts.Spectator_SpeedUp), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_CHANGE_SPEED_DOWN), MyTexts.GetString(MySpaceTexts.Spectator_SpeedDown), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_CHANGE_ROTATION_SPEED_UP), MyTexts.GetString(MySpaceTexts.Spectator_RotationSpeedUp), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_CHANGE_ROTATION_SPEED_DOWN), MyTexts.GetString(MySpaceTexts.Spectator_RotationSpeedDown), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerBuilding)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.NEXT_BLOCK_STAGE), MyTexts.GetString(MyCommonTexts.ControlName_ChangeBlockVariants), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE), MyTexts.GetString(MyCommonTexts.ControlName_CubeSizeMode), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_DEFAULT_MOUNTPOINT), MyTexts.GetString(MySpaceTexts.ControlName_BlockAutorotation), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerBuildingSurvival)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerSecondaryBuildSurvival), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerBuildingCreative)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerSecondayBuildCreative), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SYMMETRY_SWITCH), MyTexts.GetString(MySpaceTexts.ControlName_UseSymmetry), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPlacing)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPlace), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.FREE_ROTATION), MyTexts.GetString(MySpaceTexts.StationRotation_Static), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MyCommonTexts.Cancel), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.ROTATE_AXIS_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRotateCw), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.ROTATE_AXIS_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRotateCcw), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CHANGE_ROTATION_AXIS), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerChangeRotationAxis), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_FURTHER), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerFurther), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_CLOSER), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCloser), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SLOT0), MyTexts.GetString(MySpaceTexts.HelpScreen_SymmetryUnequip), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.ControlName_SymmetrySwitch)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ResetPlane), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_SetPlane), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.NEXT_BLOCK_STAGE), MyTexts.GetString(MySpaceTexts.HelpScreen_SymmetryTurnOffSetup), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.CHANGE_ROTATION_AXIS), MyTexts.GetString(MySpaceTexts.HelpScreen_SymmetryNextPlane), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_SYMMETRY, MyControlsSpace.SLOT0), MyTexts.GetString(MySpaceTexts.HelpScreen_SymmetryUnequip), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.RadialMenuGroupTitle_VoxelHandBrushes)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPlace), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.Remove), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_PAINT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerPaint), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_REVERT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRevert), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_SCALE_DOWN), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerScaleDown), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_SCALE_UP), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerScaleUp), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_MATERIAL_SELECT), MyTexts.GetString(MySpaceTexts.RadialMenu_Materials), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.VOXEL_HAND_SETTINGS), MyTexts.GetString(MyCommonTexts.ControlDescOpenVoxelHandSettings), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.ROTATE_AXIS_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerRotateCw), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_VOXEL, MyControlsSpace.CHANGE_ROTATION_AXIS), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerChangeRotationAxis), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_FURTHER), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerFurther), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.MOVE_CLOSER), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerCloser), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SLOT0), MyTexts.GetString(MySpaceTexts.HelpScreen_SymmetryUnequip), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorTool)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.ControlDescHoldToColor), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_ACTIONS, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MySpaceTexts.PickColorFromCube), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.MEDIUM_COLOR_BRUSH), MyTexts.GetString(MySpaceTexts.ControlDescMediumBrush), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.LARGE_COLOR_BRUSH), MyTexts.GetString(MySpaceTexts.ControlDescLargeBrush), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.RECOLOR_WHOLE_GRID), MyTexts.GetString(MySpaceTexts.ControlDescWholeBrush), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_COLOR_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorPrevious), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_COLOR_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerColorNext), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_SKIN_LEFT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerSkinPrevious), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.CYCLE_SKIN_RIGHT), MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerSkinNext), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_COLOR_PICKER, MyControlsSpace.SLOT0), MyTexts.GetString(MySpaceTexts.HelpScreen_SymmetryUnequip), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory)));
<<<<<<< HEAD
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Transfer), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)}", MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Transfer10), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)}", MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Transfer100), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)}", MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Transfer1000), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Split), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {'\ue00f'.ToString()}", MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_MoveItem), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_UseItem), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
=======
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Transfer), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)}", MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Transfer10), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)}", MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Transfer100), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)}", MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Transfer1000), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_Split), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {'\ue00f'.ToString()}", MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_MoveItem), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y), MyTexts.GetString(MyCommonTexts.HelpScreen_Inventory_UseItem), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_Production)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT), MyTexts.GetString(MyCommonTexts.HelpScreen_Production_Queue1), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT)}", MyTexts.GetString(MyCommonTexts.HelpScreen_Production_Queue10), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT)}", MyTexts.GetString(MyCommonTexts.HelpScreen_Production_Queue100), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT)}", MyTexts.GetString(MyCommonTexts.HelpScreen_Production_Queue1000), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT), MyTexts.GetString(MyCommonTexts.HelpScreen_Production_Dequeue), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {'\ue00f'.ToString()}", MyTexts.GetString(MyCommonTexts.HelpScreen_Production_MoveItem), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_GeneralUI)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_Slider), string.Empty, Color.White));
				contentList.Controls.Add(AddKeyPanel('\ue026'.ToString(), MyTexts.GetString(MyCommonTexts.HelpScreen_Slider_Move1), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {'\ue026'.ToString()}", MyTexts.GetString(MyCommonTexts.HelpScreen_Slider_Move10), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {'\ue026'.ToString()}", MyTexts.GetString(MyCommonTexts.HelpScreen_Slider_Move100), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {'\ue026'.ToString()}", MyTexts.GetString(MyCommonTexts.HelpScreen_Slider_MoveHalf), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_Numeric), string.Empty, Color.White));
				contentList.Controls.Add(AddKeyPanel('\ue026'.ToString(), MyTexts.GetString(MyCommonTexts.HelpScreen_Numeric_Move1), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {'\ue026'.ToString()}", MyTexts.GetString(MyCommonTexts.HelpScreen_Numeric_Move10), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {'\ue026'.ToString()}", MyTexts.GetString(MyCommonTexts.HelpScreen_Numeric_Move100), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {'\ue026'.ToString()}", MyTexts.GetString(MyCommonTexts.HelpScreen_Numeric_Move1000), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox), string.Empty, Color.White));
				contentList.Controls.Add(AddKeyPanel('\ue027'.ToString(), MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_SelectMove), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {'\ue027'.ToString()}", MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_AddMove), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {'\ue027'.ToString()}", MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_ToggleMove), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {'\ue027'.ToString()}", MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_Move), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A), MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_SelectCurrent), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)}", MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_SelectRange), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)}", MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_ToggleCurrent), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_LEFT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SHIFT_RIGHT)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_A)}", MyTexts.GetString(MyCommonTexts.HelpScreen_Listbox_DeSelectAll), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_ActionSetup)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.PRIMARY_TOOL_ACTION), MyTexts.GetString(MyCommonTexts.HelpScreen_GamepadAdvanced_NextToolbar), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION), MyTexts.GetString(MyCommonTexts.HelpScreen_GamepadAdvanced_PreviousToolbar), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_GeneralGame)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_BUTTON)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_BUTTON)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X)}", MyTexts.GetString(MyCommonTexts.ControlDescQuickSave), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddKeyPanel($"{MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_BUTTON)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_BUTTON)} + {MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y)}", MyTexts.GetString(MyCommonTexts.ControlDescQuickLoad), Color.White, isGamepadKey: true));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint1)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint2)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint3)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint4)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint5)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint6)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint7)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint8)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.HelpScreen_ControllerHint10)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddSeparatorPanel());
				break;
			}
			case HelpPageEnum.Chat:
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_ChatDescription)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Header_Name) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddChatColors_Name();
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Header_Text) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddChatColors_Text();
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Controls) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddChatControls();
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Commands) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddChatCommands();
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Emotes) + ":"));
				contentList.Controls.Add(AddTinySpacePanel());
				AddEmoteCommands();
				contentList.Controls.Add(AddTinySpacePanel());
				break;
			case HelpPageEnum.IngameHelp:
				AddIngameHelpContent(contentList);
				break;
			case HelpPageEnum.Welcome:
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.ScreenCaptionWelcomeScreen)));
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text1)));
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text2)));
				contentList.Controls.Add(AddTextPanel(string.Format(MyTexts.GetString(MySpaceTexts.WelcomeScreen_Text3), MyGameService.Service.ServiceName)));
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddSignaturePanel());
				break;
			case HelpPageEnum.ReportIssue:
			{
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_ReportIssue_Description)));
				contentList.Controls.Add(AddMultilineTextBox(out var feedbackBox));
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.HelpScreen_ReportIssue_Email)));
				contentList.Controls.Add(AddTextBox(out var emailBox));
				contentList.Controls.Add(MakeButton(new Vector2(0f, 0f), MyCommonTexts.HelpScreen_ReportIssue_SendReport, delegate
				{
					SendIssueReport(emailBox.Text, feedbackBox.Text.ToString());
				}));
				break;
			}
			default:
				contentList.Controls.Add(AddTextPanel("Incorrect page selected"));
				break;
			}
			_ = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(m_screenDescText.Position.X, m_backButton.Position.Y));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.GamepadHelpTextId = MySpaceTexts.Gamepad_Help_Back;
			if (num != -1)
			{
				if (num >= Controls.Count)
				{
					num = Controls.Count - 1;
				}
				base.FocusedControl = Controls[num];
			}
			else
			{
				base.FocusedControl = myGuiControlTable;
			}
		}

		public static string GetTutorialPartUrl(int index, bool forController)
		{
			if (index < 0)
			{
				return string.Empty;
			}
			if (forController && TutorialPartsUrlsContoller.Count > index)
			{
				return TutorialPartsUrlsContoller[index];
			}
			if (!forController && TutorialPartsUrlsKeyboard.Count > index)
			{
				return TutorialPartsUrlsKeyboard[index];
			}
			return string.Empty;
		}

		private void SendIssueReport(string email, string message)
		{
			MyLog.Default.WriteLine("User" + (string.IsNullOrWhiteSpace(email) ? "" : (" " + email)) + " is Reporting issue:\n " + message);
			MyGuiScreenProgress waitScreen = new MyGuiScreenProgress(MyTexts.Get(MyCommonTexts.HelpScreen_ReportIssue_WaitForSending));
			MyGuiSandbox.AddScreen(waitScreen);
			Parallel.Start(delegate
			{
				CrashInfo crashInfo = MyErrorReporter.BuildCrashInfo();
				MySandboxGame.Log.WriteLine($"\n{crashInfo}");
				MyErrorReporter.ReportNotInteractive(MyLog.Default.GetFilePath(), MyPerGameSettings.BasicGameInfo.GameAcronym, includeAdditionalLogs: true, null, isCrash: false, email, message, crashInfo);
			}, delegate
			{
				waitScreen.CloseScreen();
				MyGuiSandbox.Show(MyCommonTexts.HelpScreen_ReportIssue_ThanksForSending, MyCommonTexts.HelpScreen_ReportIssue_ThanksForSendingCaption, MyMessageBoxStyleEnum.Info);
			}, WorkPriority.VeryHigh);
		}

		private void AddIngameHelpContent(MyGuiControlList contentList)
		{
			foreach (MyIngameHelpObjective item in Enumerable.Reverse<MyIngameHelpObjective>((IEnumerable<MyIngameHelpObjective>)MySessionComponentIngameHelp.GetFinishedObjectives()))
			{
				contentList.Controls.Add(AddKeyCategoryPanel(MyTexts.GetString(item.TitleEnum)));
				contentList.Controls.Add(AddTinySpacePanel());
				MyIngameHelpDetail[] details = item.Details;
				foreach (MyIngameHelpDetail myIngameHelpDetail in details)
				{
					contentList.Controls.Add(AddTextPanel((myIngameHelpDetail.Args == null) ? MyTexts.GetString(myIngameHelpDetail.TextEnum) : string.Format(MyTexts.GetString(myIngameHelpDetail.TextEnum), myIngameHelpDetail.Args), 0.9f));
				}
				contentList.Controls.Add(AddTinySpacePanel());
				contentList.Controls.Add(AddSeparatorPanel());
				contentList.Controls.Add(AddTinySpacePanel());
			}
			LearningToSurviveQuestLog(contentList);
		}

		private void LearningToSurviveQuestLog(MyGuiControlList contentList)
		{
<<<<<<< HEAD
=======
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (MySessionComponentScriptSharedStorage.Instance == null)
			{
				return;
			}
			Regex nameRegex = new Regex("O_..x.._IsFinished");
			Regex nameRegex2 = new Regex("O_..x.._IsFailed");
			string text = "Caption";
<<<<<<< HEAD
			List<KeyValuePair<string, bool>> list = MySessionComponentScriptSharedStorage.Instance.GetBoolsByRegex(nameRegex).ToList();
			List<KeyValuePair<string, bool>> list2 = MySessionComponentScriptSharedStorage.Instance.GetBoolsByRegex(nameRegex2).ToList();
=======
			List<KeyValuePair<string, bool>> list = Enumerable.ToList<KeyValuePair<string, bool>>((IEnumerable<KeyValuePair<string, bool>>)MySessionComponentScriptSharedStorage.Instance.GetBoolsByRegex(nameRegex));
			List<KeyValuePair<string, bool>> list2 = Enumerable.ToList<KeyValuePair<string, bool>>((IEnumerable<KeyValuePair<string, bool>>)MySessionComponentScriptSharedStorage.Instance.GetBoolsByRegex(nameRegex2));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			list.Sort(m_hackyQuestComparer);
			list2.Sort(m_hackyQuestComparer);
			int num = -1;
			foreach (KeyValuePair<string, bool> item in list)
			{
				num++;
				if (item.Value)
				{
					string text2 = item.Key.Substring(0, 8);
					contentList.Controls.Add(AddKeyCategoryPanel(MyStatControlText.SubstituteTexts("{LOCC:" + MyTexts.GetString(text2 + text) + "}")));
					contentList.Controls.Add(AddTinySpacePanel());
					contentList.Controls.Add(AddTextPanel(MyStatControlText.SubstituteTexts("{LOCC:" + (list2[num].Value ? MyTexts.GetString("QuestlogDetail_Failed") : MyTexts.GetString("QuestlogDetail_Success")) + "}"), 0.9f));
					contentList.Controls.Add(AddTinySpacePanel());
					contentList.Controls.Add(AddSeparatorPanel());
					contentList.Controls.Add(AddTinySpacePanel());
				}
			}
		}

		private MyGuiControlTable.Row AddHelpScreenCategory(MyGuiControlTable table, string rowName, HelpPageEnum pageEnum)
		{
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(pageEnum);
			StringBuilder stringBuilder = new StringBuilder(rowName);
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(stringBuilder, null, stringBuilder.ToString(), Color.White);
			cell.IsAutoScaleEnabled = true;
			row.AddCell(cell);
			table.Add(row);
			return row;
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, Action<MyGuiControlButton> onClick)
		{
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			Vector4 bACK_BUTTON_BACKGROUND_COLOR = MyGuiConstants.BACK_BUTTON_BACKGROUND_COLOR;
			_ = MyGuiConstants.BACK_BUTTON_TEXT_COLOR;
			float textScale = 0.8f;
			return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, bACK_BUTTON_BACKGROUND_COLOR, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(text), textScale, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, onClick);
		}

		private void AddControlsByType(MyGuiControlTypeEnum type)
		{
			DictionaryValuesReader<MyStringId, MyControl> gameControlsList = MyInput.Static.GetGameControlsList();
			int num = 0;
			foreach (MyControl item in gameControlsList)
			{
				if (item.GetControlTypeEnum() == type)
				{
					num++;
					if (num % 5 == 0)
					{
						contentList.Controls.Add(AddTinySpacePanel());
					}
					contentList.Controls.Add(AddKeyPanel(GetControlButtonName(item), GetControlButtonDescription(item)));
				}
			}
		}

		private void AddChatColors_Name()
		{
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Self), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Self), Color.CornflowerBlue));
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Ally), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Ally), Color.LightGreen));
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Neutral), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Neutral), Color.PaleGoldenrod));
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Enemy), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Enemy), Color.Crimson));
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Name_Admin), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_NameDesc_Admin), Color.Purple));
		}

		private void AddChatColors_Text()
		{
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Text_Faction), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_TextDesc_Faction), Color.LimeGreen));
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Text_Private), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_TextDesc_Private), Color.Violet));
			contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_Text_Global), MyTexts.GetString(MyCommonTexts.ControlTypeChat_Colors_TextDesc_Global), Color.White));
		}

		private void AddChatControls()
		{
			contentList.Controls.Add(AddKeyPanel("PageUp", MyTexts.GetString(MyCommonTexts.ChatCommand_HelpSimple_PageUp)));
			contentList.Controls.Add(AddKeyPanel("PageDown", MyTexts.GetString(MyCommonTexts.ChatCommand_HelpSimple_PageDown)));
		}

		private void AddChatCommands()
		{
			if (MySession.Static == null)
			{
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.ChatCommands_Menu)));
				return;
			}
			int num = 1;
			foreach (KeyValuePair<string, IMyChatCommand> chatCommand in MySession.Static.ChatSystem.CommandSystem.ChatCommands)
			{
				contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyStringId.GetOrCompute(chatCommand.Value.CommandText)), MyTexts.GetString(MyStringId.GetOrCompute(chatCommand.Value.HelpSimpleText))));
				num++;
				if (num % 5 == 0)
				{
					contentList.Controls.Add(AddTinySpacePanel());
				}
			}
		}

		private void AddEmoteCommands()
		{
			if (MySession.Static == null)
			{
				contentList.Controls.Add(AddTextPanel(MyTexts.GetString(MyCommonTexts.ChatCommands_Menu)));
				return;
			}
			List<string> list = new List<string>();
			int num = 0;
			foreach (MyAnimationDefinition animationDefinition in MyDefinitionManager.Static.GetAnimationDefinitions())
			{
				if (!string.IsNullOrEmpty(animationDefinition.ChatCommandName))
				{
					list.Add(MyStringId.GetOrCompute(animationDefinition.ChatCommand).String);
					contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyStringId.GetOrCompute(animationDefinition.ChatCommand)), MyTexts.GetString(MyStringId.GetOrCompute(animationDefinition.ChatCommandDescription))));
					num++;
					if (num % 5 == 0)
					{
						contentList.Controls.Add(AddTinySpacePanel());
					}
				}
			}
<<<<<<< HEAD
			foreach (MyEmoteDefinition emoteDefinition in MyDefinitionManager.Static.GetEmoteDefinitions())
			{
				if (!string.IsNullOrEmpty(emoteDefinition.ChatCommandName) && !list.Contains(emoteDefinition.ChatCommand))
				{
					list.Add(MyStringId.GetOrCompute(emoteDefinition.ChatCommand).String);
					contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyStringId.GetOrCompute(emoteDefinition.ChatCommand)), MyTexts.GetString(MyStringId.GetOrCompute(emoteDefinition.ChatCommandDescription))));
=======
			foreach (MyGameInventoryItem item in Enumerable.Select<IGrouping<MyGameInventoryItemDefinition, MyGameInventoryItem>, MyGameInventoryItem>(Enumerable.GroupBy<MyGameInventoryItem, MyGameInventoryItemDefinition>((IEnumerable<MyGameInventoryItem>)MyGameService.InventoryItems, (Func<MyGameInventoryItem, MyGameInventoryItemDefinition>)((MyGameInventoryItem x) => x.ItemDefinition)), (Func<IGrouping<MyGameInventoryItemDefinition, MyGameInventoryItem>, MyGameInventoryItem>)((IGrouping<MyGameInventoryItemDefinition, MyGameInventoryItem> y) => Enumerable.First<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)y))))
			{
				if (item == null || item.ItemDefinition == null || item.ItemDefinition.ItemSlot != MyGameInventoryItemSlot.Emote)
				{
					continue;
				}
				MyEmoteDefinition definition = MyDefinitionManager.Static.GetDefinition<MyEmoteDefinition>(item.ItemDefinition.AssetModifierId);
				if (definition != null && !string.IsNullOrWhiteSpace(definition.ChatCommandName))
				{
					contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyStringId.GetOrCompute(definition.ChatCommand)), MyTexts.GetString(MyStringId.GetOrCompute(definition.ChatCommandDescription))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					num++;
					if (num % 5 == 0)
					{
						contentList.Controls.Add(AddTinySpacePanel());
<<<<<<< HEAD
					}
				}
			}
			foreach (MyGameInventoryItem item in from x in MyGameService.InventoryItems
				group x by x.ItemDefinition into y
				select y.First())
			{
				if (item == null || item.ItemDefinition == null || item.ItemDefinition.ItemSlot != MyGameInventoryItemSlot.Emote)
				{
					continue;
				}
				MyEmoteDefinition definition = MyDefinitionManager.Static.GetDefinition<MyEmoteDefinition>(item.ItemDefinition.AssetModifierId);
				if (definition != null && !string.IsNullOrWhiteSpace(definition.ChatCommandName) && !list.Contains(definition.ChatCommand))
				{
					list.Add(MyStringId.GetOrCompute(definition.ChatCommand).String);
					contentList.Controls.Add(AddKeyPanel(MyTexts.GetString(MyStringId.GetOrCompute(definition.ChatCommand)), MyTexts.GetString(MyStringId.GetOrCompute(definition.ChatCommandDescription))));
					num++;
					if (num % 5 == 0)
					{
						contentList.Controls.Add(AddTinySpacePanel());
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
		}

		public string GetControlButtonName(MyStringId control)
		{
			MyControl gameControl = MyInput.Static.GetGameControl(control);
			StringBuilder output = new StringBuilder();
			gameControl.AppendBoundButtonNames(ref output, ", ", MyInput.Static.GetUnassignedName());
			return output.ToString();
		}

		public string GetControlButtonName(MyControl control)
		{
			StringBuilder output = new StringBuilder();
			control.AppendBoundButtonNames(ref output, ", ", MyInput.Static.GetUnassignedName());
			return output.ToString();
		}

		public string GetControlButtonDescription(MyStringId control)
		{
			return MyTexts.GetString(MyInput.Static.GetGameControl(control).GetControlName());
		}

		public string GetControlButtonDescription(MyControl control)
		{
			return MyTexts.GetString(control.GetControlName());
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenHelp";
		}

		private void OnCloseClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
		}

		private void backButton_ButtonClicked(MyGuiControlButton obj)
		{
			CloseScreen();
		}

		private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs args)
		{
			if (sender.SelectedRow != null)
			{
				m_currentPage = (HelpPageEnum)sender.SelectedRow.UserData;
				RecreateControls(constructor: false);
			}
		}

		private MyGuiControlParent AddTextPanel(string text, float textScaleMultiplier = 1f)
		{
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText();
			myGuiControlMultilineText.Size = new Vector2(0.588f, 0.5f);
			myGuiControlMultilineText.TextScale *= textScaleMultiplier;
			myGuiControlMultilineText.Text = new StringBuilder(text);
			myGuiControlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlMultilineText.PositionX += 0.015f;
			myGuiControlMultilineText.Parse();
			myGuiControlMultilineText.DisableLabelScissor();
			return new MyGuiControlParent
			{
				Size = new Vector2(0.588f, myGuiControlMultilineText.TextSize.Y + 0.01f),
				Controls = { (MyGuiControlBase)myGuiControlMultilineText }
			};
		}

		private MyGuiControlParent AddTextBox(out MyGuiControlTextbox textBox, float textScaleMultiplier = 1f)
		{
			textBox = new MyGuiControlTextbox();
			textBox.Size = new Vector2(0.445f, 0.5f);
			textBox.TextScale *= textScaleMultiplier;
			return new MyGuiControlParent
			{
				Size = textBox.Size + 0.01f,
				Controls = { (MyGuiControlBase)textBox }
			};
		}

		private MyGuiControlParent AddMultilineTextBox(out MyGuiControlMultilineEditableText textBox, float textScaleMultiplier = 1f)
		{
			textBox = new MyGuiControlMultilineEditableText();
			textBox.Size = new Vector2(0.445f, 0.13f);
			textBox.TextScale *= textScaleMultiplier;
			textBox.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			textBox.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			textBox.TextWrap = true;
			return new MyGuiControlParent
			{
				Size = textBox.Size + 0.01f,
				Controls = { (MyGuiControlBase)textBox }
			};
		}

		private MyGuiControlParent AddSeparatorPanel()
		{
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.278f, 0f), 0.557f);
			return new MyGuiControlParent
			{
				Size = new Vector2(0.2f, 0.001f),
				Controls = { (MyGuiControlBase)myGuiControlSeparatorList }
			};
		}

		private MyGuiControlParent AddImageLinkPanel(string imagePath, string text, string url)
		{
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage
			{
				Size = new Vector2(0.158f, 0.108f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = new Vector2(-0.279f, 0.003f),
				BorderEnabled = true,
				BorderSize = 1,
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
			MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage
			{
				Size = new Vector2(0.158f, 0.108f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = new Vector2(-0.279f, 0.003f),
				BorderEnabled = true,
				BorderSize = 1,
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage2.SetTexture(imagePath);
			myGuiControlImage2.SetTooltip(url);
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText();
			myGuiControlMultilineText.Size = new Vector2(0.4f, 0.1f);
			myGuiControlMultilineText.Text = new StringBuilder(text);
			myGuiControlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlMultilineText.Position = new Vector2(0.12f, -0.005f);
			MyGuiControlButton myGuiControlButton = MakeButton(new Vector2(0.08f, 0f), MySpaceTexts.Blank, delegate
			{
				MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);
			});
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			myGuiControlButton.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			myGuiControlButton.Text = string.Format(MyTexts.GetString(MyCommonTexts.HelpScreen_HomeSteamOverlay), MyGameService.Service.ServiceName);
			myGuiControlButton.Alpha = 1f;
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
			myGuiControlButton.Size = new Vector2(0.22f, 0.13f);
			myGuiControlButton.TextScale = 0.736f;
			myGuiControlButton.CanHaveFocus = true;
			myGuiControlButton.PositionY += 0.05f;
			myGuiControlButton.PositionX += 0.175f;
			MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage
			{
				Size = new Vector2(0.0128f, 0.0176f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = myGuiControlButton.Position + new Vector2(0.01f, -0.01f),
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage3.SetTexture("Textures\\GUI\\link.dds");
			return new MyGuiControlParent
			{
				Size = new Vector2(0.5342f, 0.12f),
				Controls = 
				{
					(MyGuiControlBase)myGuiControlImage,
					(MyGuiControlBase)myGuiControlImage2,
					(MyGuiControlBase)myGuiControlMultilineText,
					(MyGuiControlBase)myGuiControlButton,
					(MyGuiControlBase)myGuiControlImage3
				}
			};
		}

		private MyGuiControlParent AddLinkPanel(string text, string url)
		{
			MyGuiControlButton myGuiControlButton = MakeButton(new Vector2(0.08f, 0f), MySpaceTexts.Blank, delegate
			{
				MyGuiSandbox.OpenUrl(url, UrlOpenMode.ExternalWithConfirm);
			});
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			myGuiControlButton.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			myGuiControlButton.Text = text;
			myGuiControlButton.Alpha = 1f;
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.ClickableText;
			myGuiControlButton.Size = new Vector2(0.22f, 0.13f);
			myGuiControlButton.TextScale = 0.736f;
			myGuiControlButton.CanHaveFocus = false;
			myGuiControlButton.PositionY += 0.01f;
			myGuiControlButton.PositionX += 0.175f;
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage
			{
				Size = new Vector2(0.0128f, 0.0176f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = myGuiControlButton.Position + new Vector2(0.01f, -0.01f),
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			myGuiControlImage.SetTexture("Textures\\GUI\\link.dds");
			return new MyGuiControlParent
			{
				Size = new Vector2(0.4645f, 0.024f),
				Controls = 
				{
					(MyGuiControlBase)myGuiControlButton,
					(MyGuiControlBase)myGuiControlImage
				}
			};
		}

		private MyGuiControlParent AddKeyCategoryPanel(string text)
		{
			MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(null, null, null, "Textures\\GUI\\Controls\\item_highlight_dark.dds");
			myGuiControlPanel.Size = new Vector2(0.557f, 0.035f);
			myGuiControlPanel.BorderEnabled = true;
			myGuiControlPanel.BorderSize = 1;
			myGuiControlPanel.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText();
			myGuiControlMultilineText.Size = new Vector2(0.5881f, 0.5f);
			myGuiControlMultilineText.Text = new StringBuilder(text);
			myGuiControlMultilineText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlMultilineText.PositionX += 0.02f;
			return new MyGuiControlParent
			{
				Size = new Vector2(0.2f, myGuiControlMultilineText.TextSize.Y + 0.01f),
				Controls = 
				{
					(MyGuiControlBase)myGuiControlPanel,
					(MyGuiControlBase)myGuiControlMultilineText
				}
			};
		}

		private MyGuiControlParent AddKeyPanel(string key, string description, Color? color = null, bool isGamepadKey = false)
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel();
			myGuiControlLabel.Text = key;
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlLabel.Font = (color.HasValue ? "White" : "Red");
			myGuiControlLabel.PositionX -= 0.25f;
			if (color.HasValue)
			{
				myGuiControlLabel.ColorMask = new Vector4((float)(int)color.Value.X / 256f, (float)(int)color.Value.Y / 256f, (float)(int)color.Value.Z / 256f, (float)(int)color.Value.A / 256f);
			}
			if (isGamepadKey)
			{
				myGuiControlLabel.TextScale = 1f;
			}
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel();
			myGuiControlLabel2.Text = description;
			myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			myGuiControlLabel2.PositionX += 0.25f;
			return new MyGuiControlParent
			{
				Size = new Vector2(0.5f, isGamepadKey ? 0.025f : 0.013f),
				Controls = 
				{
					(MyGuiControlBase)myGuiControlLabel,
					(MyGuiControlBase)myGuiControlLabel2
				}
			};
		}

		private MyGuiControlParent AddTinySpacePanel()
		{
			return new MyGuiControlParent
			{
				Size = new Vector2(0.2f, 0.005f)
			};
		}

		private MyGuiControlParent AddSignaturePanel()
		{
			MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2(-0.08f, -0.04f), MyGuiConstants.TEXTURE_KEEN_LOGO.MinSizeGui, null, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_KEEN_LOGO;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(0.19f, -0.01f), null, MyTexts.GetString(MySpaceTexts.WelcomeScreen_SignatureTitle));
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2(0.19f, 0.015f), null, MyTexts.GetString(MySpaceTexts.WelcomeScreen_Signature));
			myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			return new MyGuiControlParent
			{
				Size = new Vector2(0.2f, 0.1f),
				Controls = 
				{
					(MyGuiControlBase)myGuiControlLabel,
					(MyGuiControlBase)myGuiControlLabel2,
					(MyGuiControlBase)myGuiControlPanel
				}
			};
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			float num = MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_DOWN) - MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_UP);
			contentList.GetScrollBar().Value = contentList.GetScrollBar().Value + MyControllerHelper.GAMEPAD_ANALOG_SCROLL_SPEED * num;
			m_backButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_screenDescText.Visible = !MyInput.Static.IsJoystickLastUsed;
		}
	}
}
