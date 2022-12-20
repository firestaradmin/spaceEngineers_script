using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Localization;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenNewGame : MyGuiScreenBase
	{
		private MyGuiControlScreenSwitchPanel m_screenSwitchPanel;

		private MyGuiControlList m_campaignList;

		private MyGuiControlRadioButtonGroup m_campaignTypesGroup;

		private MyObjectBuilder_Campaign m_selectedCampaign;

		private MyGuiControlContentButton m_selectedButton;

		private MyLayoutTable m_tableLayout;

		private MyGuiControlLabel m_nameLabel;

		private MyGuiControlLabel m_nameText;

		private MyGuiControlLabel m_onlineModeLabel;

		private MyGuiControlCombobox m_onlineMode;

		private MyGuiControlSlider m_maxPlayersSlider;

		private MyGuiControlLabel m_maxPlayersLabel;

		private MyGuiControlLabel m_authorLabel;

		private MyGuiControlLabel m_authorText;

		private MyGuiControlLabel m_ratingLabel;

		private MyGuiControlRating m_ratingDisplay;

		private MyGuiControlMultilineText m_descriptionMultilineText;

		private MyGuiControlPanel m_descriptionPanel;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_publishButton;

		private MyGuiControlRotatingWheel m_asyncLoadingWheel;

		private Task m_refreshTask;

		private MyGuiControlLabel m_workshopError;

		private float MARGIN_TOP = 0.22f;

		private float MARGIN_BOTTOM = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;

		private float MARGIN_LEFT_INFO = 15f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private float MARGIN_RIGHT = 81f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private float MARGIN_LEFT_LIST = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private bool m_displayTabScenario;

		private bool m_displayTabWorkshop;

		private bool m_displayTabCustom;

		private int m_maxPlayers;

		private bool m_parallelLoadIsRunning;

		private bool m_workshopPermitted;

		public MyGuiScreenNewGame(bool displayTabScenario = true, bool displayTabWorkshop = true, bool displayTabCustom = true)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.878f, 0.97f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_workshopPermitted = true;
			MyGameService.Service.RequestPermissions(Permissions.UGC, attemptResolution: true, delegate(bool x)
			{
				m_workshopPermitted = x;
			});
			m_displayTabScenario = displayTabScenario;
			m_displayTabWorkshop = displayTabWorkshop;
			m_displayTabCustom = displayTabCustom;
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "New Game";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenMenuButtonCampaign);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.38f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.625f);
			Controls.Add(myGuiControlSeparatorList);
			m_workshopError = new MyGuiControlLabel(null, null, null, Color.Red);
			m_workshopError.Position = new Vector2(-0.382f, 0.46f);
			m_workshopError.Visible = false;
			Controls.Add(m_workshopError);
			m_screenSwitchPanel = new MyGuiControlScreenSwitchPanel(this, MyTexts.Get(MyCommonTexts.NewGameScreen_Description), m_displayTabScenario, m_displayTabWorkshop, m_displayTabCustom);
			Controls.Add(m_screenSwitchPanel);
			InitCampaignList();
			InitRightSide();
			m_refreshTask = MyCampaignManager.Static.RefreshModData();
			m_asyncLoadingWheel = new MyGuiControlRotatingWheel(new Vector2(m_size.Value.X / 2f - 0.077f, (0f - m_size.Value.Y) / 2f + 0.108f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.2f);
			Controls.Add(m_asyncLoadingWheel);
			CheckUGCServices();
		}

		public void SetWorkshopErrorText(string text = "", bool visible = true, bool skipUGCCheck = false)
		{
			if (!skipUGCCheck && string.IsNullOrEmpty(text))
			{
				CheckUGCServices();
				return;
			}
			m_workshopError.Text = text;
			m_workshopError.Visible = visible;
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		public override bool Update(bool hasFocus)
		{
			m_publishButton.Visible = m_selectedCampaign != null && m_selectedCampaign.IsLocalMod && !MyInput.Static.IsJoystickLastUsed;
			if (m_refreshTask.valid && m_refreshTask.IsComplete)
			{
				m_refreshTask.valid = false;
				m_asyncLoadingWheel.Visible = false;
				RefreshCampaignList();
			}
			else if (base.FocusedControl == null)
			{
				MyGuiControlButton myGuiControlButton = (MyGuiControlButton)(base.FocusedControl = (MyGuiControlButton)m_screenSwitchPanel.Controls.GetControlByName("CampaignButton"));
			}
			m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			return base.Update(hasFocus);
		}

		private void CheckUGCServices()
		{
			string text = "";
			foreach (IMyUGCService aggregate in MyGameService.WorkshopService.GetAggregates())
			{
				if (!aggregate.IsConsentGiven)
				{
					text = aggregate.ServiceName;
				}
			}
			if (text != "")
			{
				SetWorkshopErrorText(text + MyTexts.GetString(MySpaceTexts.UGC_ServiceNotAvailable_NoConsent));
			}
			else
			{
				SetWorkshopErrorText("", visible: false, skipUGCCheck: true);
			}
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (!receivedFocusInThisUpdate)
			{
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
				{
					StartSelectedWorld();
				}
				if (m_selectedCampaign != null && m_selectedCampaign.IsLocalMod && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
				{
					OnPublishButtonOnClick(null);
				}
			}
		}

		private void InitCampaignList()
		{
			Vector2 position = -m_size.Value / 2f + new Vector2(MARGIN_LEFT_LIST, MARGIN_TOP);
			m_campaignTypesGroup = new MyGuiControlRadioButtonGroup();
			m_campaignTypesGroup.SelectedChanged += CampaignSelectionChanged;
			m_campaignTypesGroup.MouseDoubleClick += CampaignDoubleClick;
			m_campaignList = new MyGuiControlList
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = position,
				Size = new Vector2(MyGuiConstants.LISTBOX_WIDTH, m_size.Value.Y - MARGIN_TOP - 0.048f)
			};
			Controls.Add(m_campaignList);
		}

		private void CampaignSelectionChanged(MyGuiControlRadioButtonGroup args)
		{
			MyGuiControlContentButton myGuiControlContentButton = args.SelectedButton as MyGuiControlContentButton;
			if (myGuiControlContentButton == null)
			{
				return;
			}
			MyObjectBuilder_Campaign myObjectBuilder_Campaign = myGuiControlContentButton.UserData as MyObjectBuilder_Campaign;
			if (myObjectBuilder_Campaign == null)
			{
				return;
			}
			string name = (string.IsNullOrEmpty(myObjectBuilder_Campaign.ModFolderPath) ? myObjectBuilder_Campaign.Name : Path.Combine(myObjectBuilder_Campaign.ModFolderPath, myObjectBuilder_Campaign.Name));
			MyCampaignManager.Static.ReloadMenuLocalization(name);
			string text = null;
			MyLocalizationContext myLocalizationContext = null;
			if (string.IsNullOrEmpty(myObjectBuilder_Campaign.DescriptionLocalizationFile))
			{
				text = myObjectBuilder_Campaign.Name;
				myLocalizationContext = MyLocalization.Static[text];
			}
			else
			{
				Dictionary<string, string> pathToContextTranslator = MyLocalization.Static.PathToContextTranslator;
				string key = (string.IsNullOrEmpty(myObjectBuilder_Campaign.ModFolderPath) ? Path.Combine(MyFileSystem.ContentPath, myObjectBuilder_Campaign.DescriptionLocalizationFile) : Path.Combine(myObjectBuilder_Campaign.ModFolderPath, myObjectBuilder_Campaign.DescriptionLocalizationFile));
				if (pathToContextTranslator.ContainsKey(key))
				{
					text = pathToContextTranslator[key];
				}
				if (!string.IsNullOrEmpty(text))
				{
					myLocalizationContext = MyLocalization.Static[text];
				}
			}
			m_descriptionMultilineText.Text = null;
			if (myLocalizationContext != null)
			{
				StringBuilder stringBuilder = myLocalizationContext["Name"];
				if (stringBuilder != null)
				{
					m_nameText.Text = stringBuilder.ToString();
				}
				else
				{
					m_nameText.Text = "name";
				}
				m_descriptionMultilineText.Text = myLocalizationContext["Description"];
			}
			if (m_descriptionMultilineText.Text == null || (m_descriptionMultilineText.Text != null && string.IsNullOrEmpty(m_descriptionMultilineText.Text.ToString())))
			{
				m_nameText.Text = myObjectBuilder_Campaign.Name;
				m_descriptionMultilineText.Text = new StringBuilder(myObjectBuilder_Campaign.Description);
			}
			m_maxPlayers = 0;
			if (myObjectBuilder_Campaign != null && myObjectBuilder_Campaign.IsMultiplayer)
			{
				int val = myObjectBuilder_Campaign.MaxPlayers;
				m_onlineMode.Enabled = true;
				if (!Sync.IsDedicated)
				{
					string platform = (MyPlatformGameSettings.CONSOLE_COMPATIBLE ? "XBox" : null);
					MyObjectBuilder_CampaignSM stateMachine = myObjectBuilder_Campaign.GetStateMachine(platform);
					int num = (MySandboxGame.Config.ExperimentalMode ? stateMachine.MaxLobbyPlayersExperimental : stateMachine.MaxLobbyPlayers);
					if (stateMachine != null && num > 0)
					{
						if (num == 1)
						{
							m_onlineMode.Enabled = false;
						}
						else
						{
							val = num;
						}
					}
				}
				m_maxPlayers = Math.Min(MyMultiplayerLobby.MAX_PLAYERS, val);
				m_maxPlayersSlider.MaxValue = Math.Max(m_maxPlayers, 3);
				m_maxPlayersSlider.Value = m_maxPlayers;
				FillOnlineMode(myObjectBuilder_Campaign.IsOfflineEnabled);
				m_onlineMode.SelectItemByIndex(0);
			}
			else
			{
				m_onlineMode.Enabled = false;
				m_onlineMode.SelectItemByIndex(0);
			}
			m_authorText.Text = myObjectBuilder_Campaign.Author;
			m_maxPlayersSlider.Enabled = m_onlineMode.Enabled && m_onlineMode.GetSelectedIndex() > 0 && m_maxPlayers > 2;
			uint num2 = 0u;
			if (!string.IsNullOrEmpty(myObjectBuilder_Campaign.DLC))
			{
				uint appId = MyDLCs.GetDLC(myObjectBuilder_Campaign.DLC).AppId;
				if (!MyGameService.IsDlcInstalled(appId) || Sync.MyId != MyGameService.UserId)
				{
					num2 = appId;
				}
			}
			if (num2 != 0)
			{
				m_okButton.Text = MyTexts.GetString(MyCommonTexts.VisitStore);
				m_okButton.SetToolTip(MyDLCs.GetRequiredDLCStoreHint(num2));
			}
			else
			{
				m_okButton.Text = MyTexts.GetString(MyCommonTexts.Start);
				m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Start));
			}
			m_selectedCampaign = myObjectBuilder_Campaign;
			if (m_selectedButton != null)
			{
				m_selectedButton.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
			}
			m_selectedButton = myGuiControlContentButton;
			m_selectedButton.HighlightType = MyGuiControlHighlightType.CUSTOM;
		}

		private void InitRightSide()
		{
			int num = 5;
			Vector2 topLeft = -m_size.Value / 2f + new Vector2(MARGIN_LEFT_LIST + m_campaignList.Size.X + MARGIN_LEFT_INFO + 0.012f, MARGIN_TOP - 0.011f);
			Vector2 value = m_size.Value;
			Vector2 size = new Vector2(value.X / 2f - topLeft.X, value.Y - MARGIN_TOP - MARGIN_BOTTOM - 0.0345f) - new Vector2(MARGIN_RIGHT, 0.12f);
			float num2 = size.X * 0.6f;
			float num3 = size.X - num2;
			float num4 = 0.052f;
			float num5 = size.Y - (float)num * num4;
			m_tableLayout = new MyLayoutTable(this, topLeft, size);
			m_tableLayout.SetColumnWidthsNormalized(num2 - 0.055f, num3 + 0.055f);
			m_tableLayout.SetRowHeightsNormalized(num4, num4, num4, num4, num4, num5);
			m_nameLabel = new MyGuiControlLabel
			{
				Text = MyTexts.GetString(MyCommonTexts.Name),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			m_nameText = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			m_authorLabel = new MyGuiControlLabel
			{
				Text = MyTexts.GetString(MyCommonTexts.WorldSettings_Author),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
			};
			m_authorText = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			m_ratingLabel = new MyGuiControlLabel
			{
				Text = MyTexts.GetString(MyCommonTexts.WorldSettings_Rating),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
			};
			m_ratingDisplay = new MyGuiControlRating(10)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			m_onlineModeLabel = new MyGuiControlLabel
			{
				Text = MyTexts.GetString(MyCommonTexts.WorldSettings_OnlineMode),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
			};
			m_onlineMode = new MyGuiControlCombobox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			FillOnlineMode();
			m_onlineMode.SelectItemByIndex(0);
			m_onlineMode.ItemSelected += m_onlineMode_ItemSelected;
			m_onlineMode.Enabled = false;
			m_maxPlayers = MyMultiplayerLobby.MAX_PLAYERS;
			m_maxPlayersSlider = new MyGuiControlSlider(Vector2.Zero, 2f, width: m_onlineMode.Size.X, maxValue: Math.Max(m_maxPlayers, 3), defaultValue: null, color: null, labelText: new StringBuilder("{0}").ToString(), labelDecimalPlaces: 0, labelScale: 0.8f, labelSpaceWidth: 0.028f, labelFont: "White", toolTip: null, visualStyle: MyGuiControlSliderStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: true, showLabel: true);
			m_maxPlayersSlider.Value = m_maxPlayers;
			m_maxPlayersLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.MaxPlayers));
			m_maxPlayersSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxPlayer));
			m_descriptionMultilineText = new MyGuiControlMultilineText(null, null, null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, selectable: false, showTextShadow: false, null, null)
			{
				Name = "BriefingMultilineText",
				Position = new Vector2(-0.009f, -0.115f),
				Size = new Vector2(0.419f, 0.412f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_descriptionPanel = new MyGuiControlCompositePanel
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			m_tableLayout.Add(m_nameLabel, MyAlignH.Left, MyAlignV.Center, 0, 0);
			m_tableLayout.Add(m_authorLabel, MyAlignH.Left, MyAlignV.Center, 1, 0);
			m_tableLayout.Add(m_onlineModeLabel, MyAlignH.Left, MyAlignV.Center, 2, 0);
			m_tableLayout.Add(m_maxPlayersLabel, MyAlignH.Left, MyAlignV.Center, 3, 0);
			m_tableLayout.Add(m_ratingLabel, MyAlignH.Left, MyAlignV.Center, 4, 0);
			m_nameLabel.PositionX -= 0.003f;
			m_authorLabel.PositionX -= 0.003f;
			m_onlineModeLabel.PositionX -= 0.003f;
			m_maxPlayersLabel.PositionX -= 0.003f;
			m_ratingLabel.PositionX -= 0.003f;
			m_tableLayout.AddWithSize(m_nameText, MyAlignH.Left, MyAlignV.Center, 0, 1);
			m_tableLayout.AddWithSize(m_authorText, MyAlignH.Left, MyAlignV.Center, 1, 1);
			m_tableLayout.AddWithSize(m_onlineMode, MyAlignH.Left, MyAlignV.Center, 2, 1);
			m_tableLayout.AddWithSize(m_maxPlayersSlider, MyAlignH.Left, MyAlignV.Center, 3, 1);
			m_tableLayout.AddWithSize(m_ratingDisplay, MyAlignH.Left, MyAlignV.Center, 4, 1);
			m_nameText.PositionX -= 0.001f;
			m_nameText.Size += new Vector2(0.002f, 0f);
			m_onlineMode.PositionX -= 0.002f;
			m_onlineMode.PositionY -= 0.005f;
			m_maxPlayersSlider.PositionX -= 0.003f;
			m_tableLayout.AddWithSize(m_descriptionPanel, MyAlignH.Left, MyAlignV.Top, 5, 0, 1, 2);
			m_tableLayout.AddWithSize(m_descriptionMultilineText, MyAlignH.Left, MyAlignV.Top, 5, 0, 1, 2);
			m_descriptionMultilineText.PositionY += 0.012f;
			float num6 = 0.01f;
			m_descriptionPanel.Position = new Vector2(m_descriptionPanel.PositionX - num6, m_descriptionPanel.PositionY - num6 + 0.012f);
			m_descriptionPanel.Size = new Vector2(m_descriptionPanel.Size.X + num6, m_descriptionPanel.Size.Y + num6 * 2f - 0.012f);
			Vector2 vector = m_size.Value / 2f;
			vector.X -= MARGIN_RIGHT + 0.004f;
			vector.Y -= MARGIN_BOTTOM + 0.004f;
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			_ = MyGuiConstants.GENERIC_BUTTON_SPACING;
			_ = MyGuiConstants.GENERIC_BUTTON_SPACING;
			m_okButton = new MyGuiControlButton(vector, MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Start), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnOkButtonClicked);
			m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Start));
			m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_publishButton = new MyGuiControlButton(vector - new Vector2(bACK_BUTTON_SIZE.X + 0.0245f, 0f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.LoadScreenButtonPublish), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnPublishButtonOnClick);
			m_publishButton.Visible = true;
			m_publishButton.Enabled = MyFakes.ENABLE_WORKSHOP_PUBLISH;
			m_descriptionPanel.Size = new Vector2(m_descriptionPanel.Size.X, m_descriptionPanel.Size.Y + MyGuiConstants.BACK_BUTTON_SIZE.Y);
			m_descriptionMultilineText.Size = new Vector2(m_descriptionMultilineText.Size.X, m_descriptionMultilineText.Size.Y + MyGuiConstants.BACK_BUTTON_SIZE.Y);
			Controls.Add(m_publishButton);
			Controls.Add(m_okButton);
			base.CloseButtonEnabled = true;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(m_nameLabel.Position.X, m_okButton.Position.Y - bACK_BUTTON_SIZE.Y / 2f));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.GamepadHelpTextId = MySpaceTexts.NewGameScenarios_Help_Screen;
		}

		private void FillOnlineMode(bool isOfflineEnabled = true)
		{
			m_onlineMode.ClearItems();
			if (isOfflineEnabled)
			{
				m_onlineMode.AddItem(0L, MyCommonTexts.WorldSettings_OnlineModeOffline);
			}
			m_onlineMode.AddItem(3L, MyCommonTexts.WorldSettings_OnlineModePrivate);
			m_onlineMode.AddItem(2L, MyCommonTexts.WorldSettings_OnlineModeFriends);
			m_onlineMode.AddItem(1L, MyCommonTexts.WorldSettings_OnlineModePublic);
		}

		private void m_onlineMode_ItemSelected()
		{
			m_maxPlayersSlider.Enabled = m_onlineMode.Enabled && m_onlineMode.GetSelectedIndex() > 0 && m_maxPlayers > 2;
		}

		private void OnPublishButtonOnClick(MyGuiControlButton myGuiControlButton)
		{
			if (m_selectedCampaign != null)
			{
				OnPublishConsent();
			}
			void OnPublishConsent()
			{
				MyCampaignManager.Static.SwitchCampaign(m_selectedCampaign.Name, m_selectedCampaign.IsVanilla, m_selectedCampaign.PublishedFileId, m_selectedCampaign.PublishedServiceName, m_selectedCampaign.ModFolderPath);
				MyGuiSandbox.AddScreen(new MyGuiScreenWorkshopTags("campaign", MyWorkshop.ScenarioCategories, null, OnPublishWorkshopTagsResult));
			}
		}

		private void OnPublishWorkshopTagsResult(MyGuiScreenMessageBox.ResultEnum result, string[] outTags, string[] serviceNames)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MyCampaignManager.Static.PublishActive(outTags, serviceNames);
			}
		}

		private void OnOkButtonClicked(MyGuiControlButton myGuiControlButton)
		{
			if (!m_parallelLoadIsRunning)
			{
				m_parallelLoadIsRunning = true;
				MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
				MyScreenManager.AddScreen(progressScreen);
				Parallel.StartBackground(delegate
				{
					StartSelectedWorld();
				}, delegate
				{
					progressScreen.CloseScreen();
					m_parallelLoadIsRunning = false;
				});
			}
		}

		private void CampaignDoubleClick(MyGuiControlRadioButton obj)
		{
			if (!m_parallelLoadIsRunning)
			{
				m_parallelLoadIsRunning = true;
				MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
				MyScreenManager.AddScreen(progressScreen);
				Parallel.StartBackground(delegate
				{
					StartSelectedWorld();
				}, delegate
				{
					progressScreen.CloseScreen();
					m_parallelLoadIsRunning = false;
				});
			}
		}

		private void StartSelectedWorld()
		{
			MyObjectBuilder_Campaign campaignToStart = m_selectedCampaign;
			if (MyInput.Static.IsJoystickLastUsed && base.FocusedControl != null && base.FocusedControl.GetType() == typeof(MyGuiControlContentButton) && base.FocusedControl.UserData != null && base.FocusedControl.UserData.GetType() == typeof(MyObjectBuilder_Campaign))
<<<<<<< HEAD
			{
				campaignToStart = base.FocusedControl.UserData as MyObjectBuilder_Campaign;
			}
			if (campaignToStart == null)
			{
				return;
			}
			uint num = 0u;
			if (!string.IsNullOrEmpty(campaignToStart.DLC))
			{
				uint appId = MyDLCs.GetDLC(campaignToStart.DLC).AppId;
				if (!MyGameService.IsDlcInstalled(appId) || Sync.MyId != MyGameService.UserId)
				{
					num = appId;
				}
			}
			if (num != 0)
			{
=======
			{
				campaignToStart = base.FocusedControl.UserData as MyObjectBuilder_Campaign;
			}
			if (campaignToStart == null)
			{
				return;
			}
			uint num = 0u;
			if (!string.IsNullOrEmpty(campaignToStart.DLC))
			{
				uint appId = MyDLCs.GetDLC(campaignToStart.DLC).AppId;
				if (!MyGameService.IsDlcInstalled(appId) || Sync.MyId != MyGameService.UserId)
				{
					num = appId;
				}
			}
			if (num != 0)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyGameService.OpenDlcInShop(MyDLCs.GetDLC(campaignToStart.DLC).AppId);
				return;
			}
			MyCampaignManager.Static.SwitchCampaign(campaignToStart.Name, campaignToStart.IsVanilla, campaignToStart.PublishedFileId, campaignToStart.PublishedServiceName, campaignToStart.ModFolderPath, MyPlatformGameSettings.CONSOLE_COMPATIBLE ? "XBox" : null);
			MyOnlineModeEnum onlineMode = (MyOnlineModeEnum)m_onlineMode.GetSelectedKey();
			int maxPlayers = (int)m_maxPlayersSlider.Value;
			if (onlineMode != 0)
			{
				MyGameService.Service.RequestPermissions(Permissions.Multiplayer, attemptResolution: true, delegate(PermissionResult granted)
				{
					switch (granted)
					{
					case PermissionResult.Granted:
						MyGameService.Service.RequestPermissions(Permissions.UGC, attemptResolution: true, delegate(PermissionResult ugcGranted)
						{
							switch (ugcGranted)
							{
							case PermissionResult.Granted:
								Run();
								break;
							case PermissionResult.Error:
								MySandboxGame.Static.Invoke(delegate
								{
									MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, default(MyStringId), MyMessageBoxStyleEnum.Info);
								}, "New Game screen");
								break;
							}
						});
						break;
					case PermissionResult.Error:
						MySandboxGame.Static.Invoke(delegate
						{
							MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, default(MyStringId), MyMessageBoxStyleEnum.Info);
						}, "New Game screen");
						break;
					}
				});
			}
			else
			{
				Run();
			}
			void Run()
			{
				if (MyCloudHelper.IsError(MyCampaignManager.Static.RunNewCampaign(campaignToStart.Name, onlineMode, maxPlayers, MyPlatformGameSettings.CONSOLE_COMPATIBLE ? "XBox" : null), out var errorMessage))
				{
					MySandboxGame.Static.Invoke(delegate
					{
						MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(errorMessage), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
						myGuiScreenMessageBox.SkipTransition = true;
						myGuiScreenMessageBox.InstantClose = false;
						MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
					}, "New Game screen");
				}
			}
		}

		private void OnCancelButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CloseScreen();
		}

		public override void OnScreenOrderChanged(MyGuiScreenBase oldLast, MyGuiScreenBase newLast)
		{
			base.OnScreenOrderChanged(oldLast, newLast);
			CheckUGCServices();
		}

		private void RefreshCampaignList()
		{
			(MyGameServiceCallResult, string) refreshSubscribedModDataResult = MyCampaignManager.Static.RefreshSubscribedModDataResult;
			if (refreshSubscribedModDataResult.Item1 != MyGameServiceCallResult.OK)
			{
				string workshopErrorText = MyWorkshop.GetWorkshopErrorText(refreshSubscribedModDataResult.Item1, refreshSubscribedModDataResult.Item2, m_workshopPermitted);
				SetWorkshopErrorText(workshopErrorText);
			}
			else
			{
				SetWorkshopErrorText("", visible: false);
			}
<<<<<<< HEAD
			List<MyObjectBuilder_Campaign> source = MyCampaignManager.Static.Campaigns.ToList();
			List<MyObjectBuilder_Campaign> list = new List<MyObjectBuilder_Campaign>();
=======
			List<MyObjectBuilder_Campaign> list = Enumerable.ToList<MyObjectBuilder_Campaign>(MyCampaignManager.Static.Campaigns);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			List<MyObjectBuilder_Campaign> list2 = new List<MyObjectBuilder_Campaign>();
			List<MyObjectBuilder_Campaign> list3 = new List<MyObjectBuilder_Campaign>();
			List<MyObjectBuilder_Campaign> list4 = new List<MyObjectBuilder_Campaign>();
			List<MyObjectBuilder_Campaign> list5 = new List<MyObjectBuilder_Campaign>();
			foreach (MyObjectBuilder_Campaign item in Enumerable.ToList<MyObjectBuilder_Campaign>((IEnumerable<MyObjectBuilder_Campaign>)Enumerable.OrderBy<MyObjectBuilder_Campaign, int>((IEnumerable<MyObjectBuilder_Campaign>)list, (Func<MyObjectBuilder_Campaign, int>)((MyObjectBuilder_Campaign x) => x.Order))))
			{
				if (item.IsVanilla && !item.IsDebug)
				{
					list2.Add(item);
				}
				else if (item.IsLocalMod)
				{
					list3.Add(item);
				}
				else if (item.IsVanilla && item.IsDebug)
				{
					list5.Add(item);
				}
				else
				{
					list4.Add(item);
				}
			}
			m_campaignList.Controls.Clear();
			m_campaignTypesGroup.Clear();
<<<<<<< HEAD
			foreach (MyObjectBuilder_Campaign item2 in list)
			{
				AddCampaignButton(item2);
			}
			if (MySandboxGame.Config.ExperimentalMode && !MyPlatformGameSettings.CONSOLE_COMPATIBLE)
			{
				if (list3.Count > 0)
				{
					AddSeparator(MyTexts.Get(MyCommonTexts.Workshop).ToString());
				}
				foreach (MyObjectBuilder_Campaign item3 in list3)
				{
					AddCampaignButton(item3);
				}
				if (list2.Count > 0)
				{
					AddSeparator(MyTexts.Get(MyCommonTexts.Local).ToString());
				}
				foreach (MyObjectBuilder_Campaign item4 in list2)
=======
			foreach (MyObjectBuilder_Campaign item2 in list2)
			{
				AddCampaignButton(item2);
			}
			if (MySandboxGame.Config.ExperimentalMode && !MyPlatformGameSettings.CONSOLE_COMPATIBLE)
			{
				if (list4.Count > 0)
				{
					AddSeparator(MyTexts.Get(MyCommonTexts.Workshop).ToString());
				}
				foreach (MyObjectBuilder_Campaign item3 in list4)
				{
					AddCampaignButton(item3);
				}
				if (list3.Count > 0)
				{
					AddSeparator(MyTexts.Get(MyCommonTexts.Local).ToString());
				}
				foreach (MyObjectBuilder_Campaign item4 in list3)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					AddCampaignButton(item4, isLocalMod: true);
				}
			}
			if (m_campaignList.Controls.Count > 0)
			{
				m_campaignTypesGroup.SelectByIndex(0);
				base.FocusedControl = m_campaignList.Controls[0];
			}
			CheckUGCServices();
		}

		private void AddCampaignButton(MyObjectBuilder_Campaign campaign, bool isLocalMod = false)
		{
			string dlcIcon = null;
			if (!string.IsNullOrEmpty(campaign.DLC))
			{
				MyDLCs.MyDLC dLC = MyDLCs.GetDLC(campaign.DLC);
				if (dLC == null)
				{
					return;
				}
				dlcIcon = dLC.Icon;
			}
			string title = campaign.Name;
			MyLocalizationContext myLocalizationContext = MyLocalization.Static[campaign.Name];
			if (myLocalizationContext != null)
			{
				StringBuilder stringBuilder = myLocalizationContext["Name"];
				if (stringBuilder != null)
				{
					title = stringBuilder.ToString();
				}
			}
			MyGuiControlContentButton myGuiControlContentButton = new MyGuiControlContentButton(title, GetImagePath(campaign), dlcIcon)
			{
				UserData = campaign,
				Key = m_campaignTypesGroup.Count
			};
			myGuiControlContentButton.FocusHighlightAlsoSelects = true;
			myGuiControlContentButton.SetModType(isLocalMod ? MyBlueprintTypeEnum.LOCAL : (campaign.IsVanilla ? MyBlueprintTypeEnum.DEFAULT : MyBlueprintTypeEnum.WORKSHOP), campaign.PublishedServiceName);
			if (!string.IsNullOrEmpty(campaign.DLC))
			{
				if (MyGameService.IsDlcInstalled(MyDLCs.GetDLC(campaign.DLC).AppId) && Sync.MyId == MyGameService.UserId)
				{
					myGuiControlContentButton.ColorMask = new Vector4(1f);
				}
				else
				{
					myGuiControlContentButton.ColorMask = new Vector4(0.5f);
				}
			}
			myGuiControlContentButton.HighlightChanged += Button_HighlightChanged;
			m_campaignTypesGroup.Add(myGuiControlContentButton);
			m_campaignList.Controls.Add(myGuiControlContentButton);
			if (campaign != null && campaign.IsLocalMod)
			{
				myGuiControlContentButton.GamepadHelpTextId = MySpaceTexts.NewGameScenarios_Help_ScenarioWithPublish;
			}
		}

		private void Button_HighlightChanged(MyGuiControlBase obj)
		{
		}

		private string GetImagePath(MyObjectBuilder_Campaign campaign)
		{
			string text = campaign.ImagePath;
			if (string.IsNullOrEmpty(campaign.ImagePath))
			{
				return string.Empty;
			}
			if (!campaign.IsVanilla)
			{
				text = ((campaign.ModFolderPath != null) ? Path.Combine(campaign.ModFolderPath, campaign.ImagePath) : string.Empty);
				if (!MyFileSystem.FileExists(text))
				{
					text = Path.Combine(MyFileSystem.ContentPath, campaign.ImagePath);
				}
			}
			return text;
		}

		private void AddSeparator(string sectionName)
		{
			MyGuiControlCompositePanel myGuiControlCompositePanel = new MyGuiControlCompositePanel
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = Vector2.Zero
			};
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = sectionName,
				Font = "Blue",
				PositionX = 0.005f
			};
			float num = 0.003f;
			Color tHEMED_GUI_LINE_COLOR = MyGuiConstants.THEMED_GUI_LINE_COLOR;
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(null, null, null, null, new string[1] { "Textures\\GUI\\FogSmall3.dds" })
			{
				Size = new Vector2(myGuiControlLabel.Size.X + num * 10f, 0.007f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				ColorMask = tHEMED_GUI_LINE_COLOR.ToVector4(),
				Position = new Vector2(0f - num, myGuiControlLabel.Size.Y)
			};
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent
			{
				Size = new Vector2(m_campaignList.Size.X, myGuiControlLabel.Size.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = Vector2.Zero
			};
			myGuiControlCompositePanel.Size = myGuiControlParent.Size + new Vector2(-0.035f, 0.01f);
			myGuiControlCompositePanel.Position -= myGuiControlParent.Size / 2f - new Vector2(-0.01f, 0f);
			myGuiControlLabel.Position -= myGuiControlParent.Size / 2f;
			myGuiControlImage.Position -= myGuiControlParent.Size / 2f;
			myGuiControlParent.Controls.Add(myGuiControlCompositePanel);
			myGuiControlParent.Controls.Add(myGuiControlImage);
			myGuiControlParent.Controls.Add(myGuiControlLabel);
			m_campaignList.Controls.Add(myGuiControlParent);
		}
	}
}
