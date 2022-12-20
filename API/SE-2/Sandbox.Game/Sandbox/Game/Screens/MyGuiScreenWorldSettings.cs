using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenWorldSettings : MyGuiScreenBase
	{
		private float MARGIN_TOP = 0.22f;

		private float MARGIN_BOTTOM = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;

		private float MARGIN_LEFT_INFO = 29.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private float MARGIN_RIGHT = 81f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private float MARGIN_LEFT_LIST = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private string m_sessionPath;

<<<<<<< HEAD
=======
		private MyGuiScreenWorldSettings m_parent;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private List<MyObjectBuilder_Checkpoint.ModItem> m_mods;

		private MyObjectBuilder_Checkpoint m_checkpoint;

		private MyGuiControlTextbox m_nameTextbox;

		private MyGuiControlTextbox m_descriptionTextbox;

		private MyGuiControlCombobox m_onlineMode;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_survivalModeButton;

		private MyGuiControlButton m_creativeModeButton;

		private MyGuiControlButton m_worldGeneratorButton;

		private MyGuiControlSlider m_maxPlayersSlider;

		private MyGuiControlCheckbox m_autoSave;

		private MyGuiControlLabel m_maxPlayersLabel;

		private MyGuiControlLabel m_autoSaveLabel;

		private MyGuiControlList m_scenarioTypesList;

		private MyGuiControlRadioButtonGroup m_scenarioTypesGroup;

		private MyGuiControlRotatingWheel m_asyncLoadingWheel;

		private IMyAsyncResult m_loadingTask;

		private MyGuiControlRadioButton m_selectedButton;

		private MyGuiControlButton m_advanced;

		private bool m_displayTabScenario;

		private bool m_displayTabWorkshop;

		private bool m_displayTabCustom;

		private bool m_descriptionChanged;

		protected bool m_isNewGame;

		protected MyObjectBuilder_SessionSettings m_settings;

		internal MyGuiScreenAdvancedWorldSettings Advanced;

		internal MyGuiScreenWorldGeneratorSettings WorldGenerator;

		internal MyGuiScreenMods ModsScreen;

		private MyGuiControlButton m_modsButton;

		private readonly bool m_isCloudPath;

		private int m_maxPlayers;

		private bool m_parallelLoadIsRunning;

		public MyObjectBuilder_SessionSettings Settings
		{
			get
			{
				GetSettingsFromControls();
				return m_settings;
			}
		}

		public MyObjectBuilder_Checkpoint Checkpoint => m_checkpoint;

		public MyGuiScreenWorldSettings(bool displayTabScenario = true, bool displayTabWorkshop = true, bool displayTabCustom = true)
			: this(null, null, displayTabScenario, displayTabWorkshop, displayTabCustom)
		{
		}

		public MyGuiScreenWorldSettings(MyObjectBuilder_Checkpoint checkpoint, string path, bool displayTabScenario = true, bool displayTabWorkshop = true, bool displayTabCustom = true, bool isCloudPath = false)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, CalcSize(checkpoint), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_displayTabScenario = displayTabScenario;
			m_displayTabWorkshop = displayTabWorkshop;
			m_displayTabCustom = displayTabCustom;
			m_isCloudPath = isCloudPath;
			MySandboxGame.Log.WriteLine("MyGuiScreenWorldSettings.ctor START");
			base.EnabledBackgroundFade = true;
			m_checkpoint = checkpoint;
			if (checkpoint == null || checkpoint.Mods == null)
			{
				m_mods = new List<MyObjectBuilder_Checkpoint.ModItem>();
			}
			else
			{
				m_mods = checkpoint.Mods;
			}
			m_sessionPath = path;
			m_isNewGame = checkpoint == null;
			RecreateControls(constructor: true);
			MySandboxGame.Log.WriteLine("MyGuiScreenWorldSettings.ctor END");
		}

		public static Vector2 CalcSize(MyObjectBuilder_Checkpoint checkpoint)
		{
			float x = ((checkpoint == null) ? 0.878f : (183f / 280f));
			float y = ((checkpoint == null) ? 0.97f : 0.9398855f);
			return new Vector2(x, y);
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			if (WorldGenerator != null)
			{
				WorldGenerator.CloseScreen(isUnloading);
			}
			WorldGenerator = null;
			if (Advanced != null)
			{
				Advanced.CloseScreen(isUnloading);
			}
			Advanced = null;
			if (ModsScreen != null)
			{
				ModsScreen.CloseScreen(isUnloading);
			}
			ModsScreen = null;
			return base.CloseScreen(isUnloading);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenWorldSettings";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			BuildControls();
			if (m_isNewGame)
			{
				SetDefaultValues();
<<<<<<< HEAD
				MyGuiControlScreenSwitchPanel control = new MyGuiControlScreenSwitchPanel(this, MyTexts.Get(MyCommonTexts.WorldSettingsScreen_Description), m_displayTabScenario, m_displayTabWorkshop, m_displayTabCustom);
				Controls.Add(control);
=======
				new MyGuiControlScreenSwitchPanel(this, MyTexts.Get(MyCommonTexts.WorldSettingsScreen_Description), m_displayTabScenario, m_displayTabWorkshop, m_displayTabCustom);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				base.GamepadHelpTextId = ((!MyPlatformGameSettings.IsModdingAllowed || !MyFakes.ENABLE_WORKSHOP_MODS) ? MySpaceTexts.WorldSettings_Help_ScreenNewGame_Modless : MySpaceTexts.WorldSettings_Help_ScreenNewGame);
			}
			else
			{
				LoadValues();
				m_nameTextbox.MoveCarriageToEnd();
				m_descriptionTextbox.MoveCarriageToEnd();
				base.GamepadHelpTextId = ((!MyPlatformGameSettings.IsModdingAllowed || !MyFakes.ENABLE_WORKSHOP_MODS) ? MySpaceTexts.WorldSettings_Help_Screen_Modless : MySpaceTexts.WorldSettings_Help_Screen);
			}
		}

		private void InitCampaignList()
		{
			if (MyDefinitionManager.Static.GetScenarioDefinitions().Count == 0)
			{
				MyDefinitionManager.Static.LoadScenarios();
			}
			Vector2 position = -m_size.Value / 2f + new Vector2(MARGIN_LEFT_LIST, MARGIN_TOP);
			m_scenarioTypesGroup = new MyGuiControlRadioButtonGroup();
			m_scenarioTypesGroup.SelectedChanged += scenario_SelectedChanged;
			m_scenarioTypesGroup.MouseDoubleClick += delegate
			{
				OnOkButtonClick(null);
			};
			m_scenarioTypesList = new MyGuiControlList
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = position,
				Size = new Vector2(MyGuiConstants.LISTBOX_WIDTH, m_size.Value.Y - MARGIN_TOP - 0.048f)
			};
		}

		protected virtual void BuildControls()
		{
			if (m_isNewGame)
			{
				AddCaption(MyCommonTexts.ScreenMenuButtonCampaign);
			}
			else
			{
				AddCaption(MyCommonTexts.ScreenCaptionEditSettings);
			}
			int num = 0;
			if (m_isNewGame)
			{
				InitCampaignList();
			}
			Vector2 vector = new Vector2(0f, 0.052f);
			Vector2 vector2 = -m_size.Value / 2f + new Vector2(m_isNewGame ? (MARGIN_LEFT_LIST + m_scenarioTypesList.Size.X + MARGIN_LEFT_INFO) : MARGIN_LEFT_LIST, m_isNewGame ? (MARGIN_TOP + 0.015f) : (MARGIN_TOP - 0.105f));
			Vector2 vector3 = m_size.Value / 2f - vector2;
			vector3.X -= MARGIN_RIGHT + 0.005f;
			vector3.Y -= MARGIN_BOTTOM;
			Vector2 vector4 = vector3 * (m_isNewGame ? 0.339f : 0.329f);
			Vector2 vector5 = vector2 + new Vector2(vector4.X, 0f);
			Vector2 vector6 = vector3 - vector4;
			MyGuiControlLabel control = MakeLabel(MyCommonTexts.Name);
			MyGuiControlLabel control2 = MakeLabel(MyCommonTexts.Description);
			MyGuiControlLabel control3 = MakeLabel(MyCommonTexts.WorldSettings_GameMode);
			MyGuiControlLabel control4 = MakeLabel(MyCommonTexts.WorldSettings_OnlineMode);
			m_maxPlayersLabel = MakeLabel(MyCommonTexts.MaxPlayers);
			m_autoSaveLabel = MakeLabel(MyCommonTexts.WorldSettings_AutoSave);
			m_nameTextbox = new MyGuiControlTextbox(null, null, 90);
			m_descriptionTextbox = new MyGuiControlTextbox(null, null, 7999);
			m_descriptionTextbox.TextChanged += OnDescriptionChanged;
			m_onlineMode = new MyGuiControlCombobox(null, new Vector2(vector6.X, 0.04f));
			m_maxPlayers = MyMultiplayerLobby.MAX_PLAYERS;
			m_maxPlayersSlider = new MyGuiControlSlider(Vector2.Zero, 2f, width: m_onlineMode.Size.X, maxValue: Math.Max(m_maxPlayers, 3), defaultValue: null, color: null, labelText: new StringBuilder("{0}").ToString(), labelDecimalPlaces: 0, labelScale: 0.8f, labelSpaceWidth: 0.028f, labelFont: "White", toolTip: null, visualStyle: MyGuiControlSliderStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: true, showLabel: true);
			m_maxPlayersSlider.Value = m_maxPlayers;
			m_autoSave = new MyGuiControlCheckbox();
			m_autoSave.SetToolTip(new StringBuilder().AppendFormat(MyCommonTexts.ToolTipWorldSettingsAutoSave, 5u).ToString());
			m_creativeModeButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Small, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.WorldSettings_GameModeCreative), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnCreativeClick);
			m_creativeModeButton.SetToolTip(MySpaceTexts.ToolTipWorldSettingsModeCreative);
			m_survivalModeButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Small, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.WorldSettings_GameModeSurvival), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnSurvivalClick);
			m_survivalModeButton.SetToolTip(MySpaceTexts.ToolTipWorldSettingsModeSurvival);
			m_onlineMode.ItemSelected += OnOnlineModeSelect;
			m_onlineMode.AddItem(0L, MyCommonTexts.WorldSettings_OnlineModeOffline);
			m_onlineMode.AddItem(3L, MyCommonTexts.WorldSettings_OnlineModePrivate);
			m_onlineMode.AddItem(2L, MyCommonTexts.WorldSettings_OnlineModeFriends);
			m_onlineMode.AddItem(1L, MyCommonTexts.WorldSettings_OnlineModePublic);
			if (m_isNewGame)
			{
				if (MyDefinitionManager.Static.GetScenarioDefinitions().Count == 0)
				{
					MyDefinitionManager.Static.LoadScenarios();
				}
				m_scenarioTypesGroup = new MyGuiControlRadioButtonGroup();
				m_scenarioTypesGroup.SelectedChanged += scenario_SelectedChanged;
				m_scenarioTypesGroup.MouseDoubleClick += OnOkButtonClick;
				m_asyncLoadingWheel = new MyGuiControlRotatingWheel(new Vector2(m_size.Value.X / 2f - 0.077f, (0f - m_size.Value.Y) / 2f + 0.108f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.2f);
				m_loadingTask = StartLoadingWorldInfos();
			}
			m_nameTextbox.SetToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorldSettingsName), 5, 90));
			m_nameTextbox.FocusChanged += NameFocusChanged;
			m_descriptionTextbox.SetToolTip(MyTexts.GetString(MyCommonTexts.ToolTipWorldSettingsDescription));
			m_descriptionTextbox.FocusChanged += DescriptionFocusChanged;
			m_onlineMode.SetToolTip(string.Format(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsOnlineMode), MySession.GameServiceName));
			m_onlineMode.HideToolTip();
			m_maxPlayersSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipWorldSettingsMaxPlayer));
			m_worldGeneratorButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.WorldSettings_WorldGenerator), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnWorldGeneratorClick);
			Controls.Add(control);
			Controls.Add(m_nameTextbox);
			Controls.Add(control2);
			Controls.Add(m_descriptionTextbox);
			Controls.Add(control3);
			Controls.Add(m_creativeModeButton);
			Controls.Add(control4);
			Controls.Add(m_onlineMode);
			Controls.Add(m_maxPlayersLabel);
			Controls.Add(m_maxPlayersSlider);
			Controls.Add(m_autoSaveLabel);
			Controls.Add(m_autoSave);
			Vector2 vector7 = m_size.Value / 2f;
			vector7.X -= MARGIN_RIGHT + 0.004f;
			vector7.Y -= MARGIN_BOTTOM + 0.004f;
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			_ = MyGuiConstants.GENERIC_BUTTON_SPACING;
			_ = MyGuiConstants.GENERIC_BUTTON_SPACING;
			m_advanced = new MyGuiControlButton(vector7 - new Vector2(bACK_BUTTON_SIZE.X + 0.0245f, 0f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MySpaceTexts.WorldSettings_Advanced), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewGameCustomGame_Advanced), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: OnAdvancedClick);
			m_advanced.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_modsButton = new MyGuiControlButton(vector7 - new Vector2(bACK_BUTTON_SIZE.X * 2f + 0.049f, 0f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, text: MyTexts.Get(MyCommonTexts.WorldSettings_Mods), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewGameCustomGame_Mods), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: OnModsClick);
			m_modsButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			if (MyPlatformGameSettings.IsModdingAllowed && MyFakes.ENABLE_WORKSHOP_MODS)
			{
				Controls.Add(m_modsButton);
			}
			Controls.Add(m_advanced);
			m_modsButton.SetEnabledByExperimental();
			foreach (MyGuiControlBase control5 in Controls)
			{
				control5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				if (control5 is MyGuiControlLabel)
				{
					control5.Position = vector2 + vector * num;
				}
				else
				{
					control5.Position = vector5 + vector * num++;
				}
			}
			if (m_isNewGame)
			{
				Controls.Add(m_scenarioTypesList);
			}
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			Vector2 start;
			if (m_isNewGame)
			{
				start = new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.38f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f);
				myGuiControlSeparatorList.AddHorizontal(start, m_size.Value.X * 0.625f);
			}
			else
			{
				start = new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f);
				myGuiControlSeparatorList.AddHorizontal(start, m_size.Value.X * 0.835f);
				myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.835f);
			}
			Controls.Add(myGuiControlSeparatorList);
			if (m_isNewGame)
			{
				m_okButton = new MyGuiControlButton(vector7, MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Start), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnOkButtonClick);
				m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Start));
			}
			else
			{
				m_okButton = new MyGuiControlButton(vector7, MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnOkButtonClick);
				m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
			}
			m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			Controls.Add(m_okButton);
			Controls.Add(m_survivalModeButton);
			m_survivalModeButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			m_creativeModeButton.PositionX += 0.0025f;
			m_creativeModeButton.PositionY += 0.005f;
			m_survivalModeButton.Position = m_creativeModeButton.Position + new Vector2(m_onlineMode.Size.X + 0.0005f, 0f);
			m_nameTextbox.Size = m_onlineMode.Size;
			m_descriptionTextbox.Size = m_nameTextbox.Size;
			m_maxPlayersSlider.PositionX = m_nameTextbox.PositionX - 0.001f;
			m_autoSave.PositionX = m_maxPlayersSlider.PositionX;
			float num2 = 0.007f;
			if (m_modsButton != null)
			{
				m_modsButton.PositionX = m_maxPlayersSlider.PositionX + 0.003f;
				m_modsButton.PositionY += num2;
			}
			if (m_advanced != null)
			{
				m_advanced.PositionX += 0.0045f + m_modsButton.Size.X + 0.01f;
				if (MyPlatformGameSettings.IsModdingAllowed && MyFakes.ENABLE_WORKSHOP_MODS)
				{
					m_advanced.PositionY = m_modsButton.Position.Y;
				}
				else
				{
					m_advanced.PositionY += num2;
				}
			}
			if (m_isNewGame)
			{
				Controls.Add(m_asyncLoadingWheel);
			}
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(start.X, m_okButton.Position.Y - bACK_BUTTON_SIZE.Y / 2f));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.CloseButtonEnabled = true;
		}

		private void OnDescriptionChanged(MyGuiControlTextbox obj)
		{
			m_descriptionChanged = true;
		}

		private void NameFocusChanged(MyGuiControlBase obj, bool focused)
		{
			if (focused && !m_nameTextbox.IsImeActive)
			{
				m_nameTextbox.SelectAll();
				m_nameTextbox.MoveCarriageToEnd();
			}
		}

		private void DescriptionFocusChanged(MyGuiControlBase obj, bool focused)
		{
			if (focused && !m_descriptionTextbox.IsImeActive)
			{
				m_descriptionTextbox.SelectAll();
				m_descriptionTextbox.MoveCarriageToEnd();
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (m_loadingTask != null && m_loadingTask.IsCompleted)
			{
				OnLoadingFinished(m_loadingTask, null);
				m_loadingTask = null;
				m_asyncLoadingWheel.Visible = false;
			}
			if (hasFocus)
			{
				m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_advanced.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_modsButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			return base.Update(hasFocus);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (!receivedFocusInThisUpdate)
			{
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
				{
					OnOkButtonClick(null);
				}
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
				{
					OnAdvancedClick(null);
				}
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU, MyControlStateType.NEW_RELEASED) && MyPlatformGameSettings.IsModdingAllowed && MyFakes.ENABLE_WORKSHOP_MODS)
				{
					OnModsClick(null);
				}
			}
		}

		private void scenario_SelectedChanged(MyGuiControlRadioButtonGroup group)
		{
			SetDefaultName();
			if (MyFakes.ENABLE_PLANETS)
			{
				m_worldGeneratorButton.Enabled = true;
				if (m_worldGeneratorButton.Enabled && WorldGenerator != null)
				{
					WorldGenerator.GetSettings(m_settings);
				}
			}
			ulong sizeInBytes;
			MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(group.SelectedButton.UserData as string, out sizeInBytes);
			if (myObjectBuilder_Checkpoint != null)
			{
				m_settings = CopySettings(myObjectBuilder_Checkpoint.Settings);
				SetSettingsToControls();
			}
			if (m_selectedButton != null)
			{
				m_selectedButton.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
			}
			m_selectedButton = group.SelectedButton;
			m_selectedButton.HighlightType = MyGuiControlHighlightType.CUSTOM;
		}

		private MyGuiControlLabel MakeLabel(MyStringId textEnum)
		{
			return new MyGuiControlLabel(null, null, MyTexts.GetString(textEnum));
		}

		private void SetDefaultName()
		{
			if (m_scenarioTypesGroup.SelectedButton != null)
			{
				string title = ((MyGuiControlContentButton)m_scenarioTypesGroup.SelectedButton).Title;
				if (title != null)
				{
					m_nameTextbox.Text = title.ToString() + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
				}
				m_descriptionTextbox.Text = string.Empty;
			}
		}

		private void LoadValues()
		{
			m_nameTextbox.Text = m_checkpoint.SessionName ?? "";
			m_descriptionTextbox.TextChanged -= OnDescriptionChanged;
			m_descriptionTextbox.Text = (string.IsNullOrEmpty(m_checkpoint.Description) ? "" : MyTexts.SubstituteTexts(m_checkpoint.Description));
			m_descriptionTextbox.TextChanged += OnDescriptionChanged;
			m_descriptionChanged = false;
			m_settings = CopySettings(m_checkpoint.Settings);
			m_mods = m_checkpoint.Mods;
			SetSettingsToControls();
		}

		private void SetDefaultValues()
		{
			m_settings = GetDefaultSettings();
			m_settings.EnableToolShake = true;
			m_settings.EnableSunRotation = MyPerGameSettings.Game == GameEnum.SE_GAME;
			m_settings.VoxelGeneratorVersion = 4;
			m_settings.EnableOxygen = true;
			m_settings.CargoShipsEnabled = true;
			m_mods = new List<MyObjectBuilder_Checkpoint.ModItem>();
			SetSettingsToControls();
			SetDefaultName();
		}

		protected virtual MyObjectBuilder_SessionSettings GetDefaultSettings()
		{
			return MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SessionSettings>();
		}

		protected virtual MyObjectBuilder_SessionSettings CopySettings(MyObjectBuilder_SessionSettings source)
		{
			return source.Clone() as MyObjectBuilder_SessionSettings;
		}

		private void OnOnlineModeSelect()
		{
			bool flag = m_onlineMode.GetSelectedKey() == 0;
			m_maxPlayersSlider.Enabled = !flag && m_maxPlayers > 2;
			m_maxPlayersLabel.Enabled = !flag && m_maxPlayers > 2;
			int? num = (flag ? MyPlatformGameSettings.OFFLINE_TOTAL_PCU_MAX : MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX);
			if (!num.HasValue)
			{
				return;
			}
			if (flag)
			{
				if (m_settings.TotalPCU == MyPlatformGameSettings.LOBBY_TOTAL_PCU_MAX)
				{
					m_settings.TotalPCU = num.Value;
				}
			}
			else if (m_settings.TotalPCU > num)
			{
				m_settings.TotalPCU = num.Value;
				if (!m_isNewGame)
				{
					MyGuiSandbox.Show(MyCommonTexts.MessageBoxTextBlockLimitsInMP, MyCommonTexts.MessageBoxCaptionWarning, MyMessageBoxStyleEnum.Info);
				}
			}
		}

		private void OnCreativeClick(object sender)
		{
			UpdateSurvivalState(survivalEnabled: false);
			Settings.EnableCopyPaste = true;
		}

		private void OnSurvivalClick(object sender)
		{
			UpdateSurvivalState(survivalEnabled: true);
			Settings.EnableCopyPaste = false;
		}

		private void OnAdvancedClick(object sender)
		{
			Advanced = new MyGuiScreenAdvancedWorldSettings(this);
			Advanced.UpdateSurvivalState(GetGameMode() == MyGameModeEnum.Survival);
			Advanced.OnOkButtonClicked += Advanced_OnOkButtonClicked;
			MyGuiSandbox.AddScreen(Advanced);
		}

		private void OnWorldGeneratorClick(object sender)
		{
			WorldGenerator = new MyGuiScreenWorldGeneratorSettings(this);
			WorldGenerator.OnOkButtonClicked += WorldGenerator_OnOkButtonClicked;
			MyGuiSandbox.AddScreen(WorldGenerator);
		}

		private void WorldGenerator_OnOkButtonClicked()
		{
			WorldGenerator.GetSettings(m_settings);
			SetSettingsToControls();
		}

		private void OnModsClick(object sender)
		{
			if (m_modsButton.Enabled)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenMods(m_mods));
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.WorldSettings_ModsNeedExperimental)));
			}
		}

		private void UpdateSurvivalState(bool survivalEnabled)
		{
			m_creativeModeButton.Checked = !survivalEnabled;
			m_survivalModeButton.Checked = survivalEnabled;
		}

		private void Advanced_OnOkButtonClicked()
		{
			Advanced.GetSettings(m_settings);
			SetSettingsToControls();
		}

		private void OnOkButtonClick(object sender)
		{
			bool flag = m_nameTextbox.Text.ToString().Replace(':', '-').IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
			if (flag || m_nameTextbox.Text.Length < 5 || m_nameTextbox.Text.Length > 90)
			{
				MyStringId id = (flag ? MyCommonTexts.ErrorNameInvalid : ((m_nameTextbox.Text.Length >= 5) ? MyCommonTexts.ErrorNameTooLong : MyCommonTexts.ErrorNameTooShort));
				MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(id), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
				myGuiScreenMessageBox.SkipTransition = true;
				myGuiScreenMessageBox.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
				return;
			}
			if (m_descriptionTextbox.Text.Length > 7999)
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox2 = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.ErrorDescriptionTooLong), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
				myGuiScreenMessageBox2.SkipTransition = true;
				myGuiScreenMessageBox2.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox2);
				return;
			}
			GetSettingsFromControls();
			if (m_settings.OnlineMode != 0 && !MyGameService.IsActive)
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox3 = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.ErrorStartSessionNoUser), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
				myGuiScreenMessageBox3.SkipTransition = true;
				myGuiScreenMessageBox3.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox3);
			}
			else if (m_isNewGame)
			{
				if (!m_parallelLoadIsRunning)
				{
					m_parallelLoadIsRunning = true;
					MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
					MyScreenManager.AddScreen(progressScreen);
					Parallel.StartBackground(delegate
					{
						StartNewSandbox();
					}, delegate
					{
						progressScreen.CloseScreen();
						m_parallelLoadIsRunning = false;
					});
				}
			}
			else
			{
				OnOkButtonClickQuestions(0);
			}
		}

		private void OnOkButtonClickQuestions(int skipQuestions)
		{
			if (skipQuestions <= 0)
			{
				bool num = m_checkpoint.Settings.GameMode == MyGameModeEnum.Creative && GetGameMode() == MyGameModeEnum.Survival;
				bool flag = m_checkpoint.Settings.GameMode == MyGameModeEnum.Survival && GetGameMode() == MyGameModeEnum.Creative;
				if (num || (!flag && m_checkpoint.Settings.InventorySizeMultiplier > m_settings.InventorySizeMultiplier))
				{
					MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.HarvestingWarningInventoryMightBeTruncatedAreYouSure), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum x)
					{
						OnOkButtonClickAnswer(x, 1);
					});
					myGuiScreenMessageBox.SkipTransition = true;
					myGuiScreenMessageBox.InstantClose = false;
					MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
					return;
				}
			}
			if (skipQuestions <= 1 && (m_checkpoint.Settings.WorldSizeKm == 0 || m_checkpoint.Settings.WorldSizeKm > m_settings.WorldSizeKm) && m_settings.WorldSizeKm != 0)
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox2 = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.WorldSettings_WarningChangingWorldSize), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum x)
				{
					OnOkButtonClickAnswer(x, 2);
				});
				myGuiScreenMessageBox2.SkipTransition = true;
				myGuiScreenMessageBox2.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox2);
			}
			else
			{
				ChangeWorldSettings();
			}
		}

		private void OnOkButtonClickAnswer(MyGuiScreenMessageBox.ResultEnum answer, int skipQuestions)
		{
			if (answer == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				OnOkButtonClickQuestions(skipQuestions);
			}
		}

		private MyGameModeEnum GetGameMode()
		{
			if (!m_survivalModeButton.Checked)
			{
				return MyGameModeEnum.Creative;
			}
			return MyGameModeEnum.Survival;
		}

		protected virtual bool GetSettingsFromControls()
		{
			if (m_onlineMode == null || m_settings == null || m_maxPlayersSlider == null || m_autoSave == null)
			{
				return false;
			}
			m_settings.OnlineMode = (MyOnlineModeEnum)m_onlineMode.GetSelectedKey();
			if (m_checkpoint != null)
			{
				m_checkpoint.PreviousEnvironmentHostility = m_settings.EnvironmentHostility;
			}
			m_settings.MaxPlayers = (short)m_maxPlayersSlider.Value;
			m_settings.GameMode = GetGameMode();
			m_settings.ScenarioEditMode = false;
			m_settings.AutoSaveInMinutes = (m_autoSave.IsChecked ? 5u : 0u);
			return true;
		}

		protected virtual void SetSettingsToControls()
		{
			m_onlineMode.SelectItemByKey((long)m_settings.OnlineMode);
			m_maxPlayersSlider.Value = Math.Min(m_settings.MaxPlayers, m_maxPlayers);
			UpdateSurvivalState(m_settings.GameMode == MyGameModeEnum.Survival);
			m_autoSave.IsChecked = m_settings.AutoSaveInMinutes != 0;
		}

		private string GetPassword()
		{
			if (Advanced != null && Advanced.IsConfirmed)
			{
				return Advanced.Password;
			}
			if (m_checkpoint != null)
			{
				return m_checkpoint.Password;
			}
			return "";
		}

		private string GetDescription()
		{
			if (m_checkpoint != null)
			{
				return m_checkpoint.Description;
			}
			return m_descriptionTextbox.Text;
		}

		private bool DescriptionChanged()
		{
			return m_descriptionChanged;
		}

		private void CopySaveTo(string destName, out bool fileExists, out CloudResult copyResult)
		{
			fileExists = false;
			copyResult = CloudResult.Failed;
			if (!m_isCloudPath)
			{
				string sessionPath = m_sessionPath;
				string text = m_sessionPath.Replace(m_checkpoint.SessionName, destName);
				if (text == m_sessionPath)
				{
					text = Path.Combine(MyFileSystem.SavesPath, destName);
				}
				if (Directory.Exists(text))
				{
					fileExists = true;
					return;
				}
				try
				{
					Directory.CreateDirectory(Path.GetDirectoryName(text));
					Directory.Move(sessionPath, text);
					m_sessionPath = text;
					copyResult = CloudResult.Ok;
				}
				catch
				{
					copyResult = CloudResult.Failed;
				}
				return;
			}
			string sessionPath2 = m_sessionPath;
			string text2 = m_sessionPath.Replace(m_checkpoint.SessionName, destName);
			if (text2 == m_sessionPath)
			{
				text2 = MyLocalCache.GetSessionSavesPath(destName, contentFolder: false, createIfNotExists: false, isCloud: true);
			}
			List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(text2);
			if (cloudFiles != null && cloudFiles.Count > 0)
			{
				fileExists = true;
				return;
			}
			CloudResult cloudResult = MyCloudHelper.CopyFiles(sessionPath2, text2);
			if (cloudResult == CloudResult.Ok)
			{
				MyCloudHelper.Delete(sessionPath2);
				m_sessionPath = text2;
			}
			copyResult = cloudResult;
		}

		private void ChangeWorldSettings()
		{
			if (m_nameTextbox.Text != m_checkpoint.SessionName)
			{
				string destName = MyUtils.StripInvalidChars(m_nameTextbox.Text);
				CopySaveTo(destName, out var fileExists, out var copyResult);
				if (fileExists)
				{
					MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.WorldSettings_Error_NameExists), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
					myGuiScreenMessageBox.SkipTransition = true;
					myGuiScreenMessageBox.InstantClose = false;
					MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
					return;
				}
				if (MyCloudHelper.IsError(copyResult, out var errorMessage, MySpaceTexts.WorldSettings_Error_SavingFailed))
				{
					MyGuiScreenMessageBox myGuiScreenMessageBox2 = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(errorMessage), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
					myGuiScreenMessageBox2.SkipTransition = true;
					myGuiScreenMessageBox2.InstantClose = false;
					MyGuiSandbox.AddScreen(myGuiScreenMessageBox2);
					return;
				}
			}
			m_checkpoint.SessionName = m_nameTextbox.Text;
			if (DescriptionChanged())
			{
				m_checkpoint.Description = m_descriptionTextbox.Text;
				m_descriptionChanged = false;
			}
			GetSettingsFromControls();
			m_checkpoint.Settings = m_settings;
			m_checkpoint.Mods = m_mods;
			MyCampaignManager.Static?.SetExperimentalCampaign(m_checkpoint);
			if (m_isCloudPath)
			{
				MyLocalCache.SaveCheckpointToCloud(m_checkpoint, m_sessionPath);
			}
			else
			{
				MyLocalCache.SaveCheckpoint(m_checkpoint, m_sessionPath);
			}
			if (MySession.Static != null && MySession.Static.Name == m_checkpoint.SessionName && m_sessionPath == MySession.Static.CurrentPath)
			{
				MySession @static = MySession.Static;
				@static.Password = GetPassword();
				@static.Description = GetDescription();
				@static.Settings = m_checkpoint.Settings;
				@static.Mods = m_checkpoint.Mods;
			}
			CloseScreen();
		}

		private void OnCancelButtonClick(object sender)
		{
			CloseScreen();
		}

		private void OnSwitchAnswer(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MySandboxGame.Config.GraphicsRenderer = MySandboxGame.DirectX11RendererKey;
				MySandboxGame.Config.Save();
				MyGuiSandbox.BackToMainMenu();
				StringBuilder messageText = MyTexts.Get(MySpaceTexts.QuickstartDX11PleaseRestartGame);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText, MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
			else
			{
				StringBuilder messageText2 = MyTexts.Get(MySpaceTexts.QuickstartSelectDifferent);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText2, MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
		}

		private void StartNewSandbox()
		{
			MyLog.Default.WriteLine("StartNewSandbox - Start");
			if (m_scenarioTypesGroup.SelectedButton == null || m_scenarioTypesGroup.SelectedButton.UserData == null || !GetSettingsFromControls())
			{
				return;
			}
			string sessionPath = m_scenarioTypesGroup.SelectedButton.UserData as string;
			ulong checkpointSizeInBytes;
			MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out checkpointSizeInBytes);
			if (checkpoint == null)
			{
				return;
			}
			if (!MySessionLoader.HasOnlyModsFromConsentedUGCs(checkpoint))
			{
				MySessionLoader.ShowUGCConsentNotAcceptedWarning(MySessionLoader.GetNonConsentedServiceNameInCheckpoint(checkpoint));
				return;
			}
			GetSettingsFromControls();
			checkpoint.Settings = m_settings;
			checkpoint.SessionName = m_nameTextbox.Text;
			checkpoint.Password = GetPassword();
			checkpoint.Description = GetDescription();
			checkpoint.Mods = m_mods;
			if (checkpoint.Settings.OnlineMode != 0)
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
								StartSession();
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
				StartSession();
			}
			void StartSession()
			{
				MySessionLoader.LoadSingleplayerSession(checkpoint, sessionPath, checkpointSizeInBytes, delegate
				{
					string text = Path.Combine(MyFileSystem.SavesPath, checkpoint.SessionName.Replace(':', '-'));
					MySession.Static.CurrentPath = text;
					MyAsyncSaving.DelayedSaveAfterLoad(text);
				});
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Starts Async loading.
		/// </summary>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private IMyAsyncResult StartLoadingWorldInfos()
		{
			string path = "CustomWorlds";
			string item = Path.Combine(MyFileSystem.ContentPath, path);
			if (m_isNewGame)
			{
				return new MyNewCustomWorldInfoListResult(new List<string> { item });
			}
			return new MyLoadWorldInfoListResult(new List<string> { item });
		}

		/// <summary>
		/// Checks for corrupted worlds and refreshes the table cells.
		/// </summary>
		/// <param name="result">result after load</param>
		/// <param name="screen">progress screen</param>
		private void OnLoadingFinished(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			MyLoadListResult myLoadListResult = (MyLoadListResult)result;
			m_scenarioTypesGroup.Clear();
			m_scenarioTypesList.Clear();
			if (myLoadListResult.AvailableSaves.Count != 0)
			{
				myLoadListResult.AvailableSaves.Sort((Tuple<string, MyWorldInfo> a, Tuple<string, MyWorldInfo> b) => a.Item2.SessionName.CompareTo(b.Item2.SessionName));
			}
			foreach (Tuple<string, MyWorldInfo> availableSafe in myLoadListResult.AvailableSaves)
			{
				if ((MySandboxGame.Config.ExperimentalMode || !availableSafe.Item2.IsExperimental) && (MyFakes.ENABLE_PLANETS || !availableSafe.Item2.HasPlanets))
				{
					string text = availableSafe.Item1;
					if (Path.HasExtension(availableSafe.Item1))
					{
						text = Path.GetDirectoryName(availableSafe.Item1);
					}
					MyGuiControlContentButton myGuiControlContentButton = new MyGuiControlContentButton(availableSafe.Item2.SessionName, Path.Combine(text, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION))
					{
						UserData = text,
						Key = m_scenarioTypesGroup.Count
					};
					myGuiControlContentButton.FocusHighlightAlsoSelects = true;
					m_scenarioTypesGroup.Add(myGuiControlContentButton);
					m_scenarioTypesList.Controls.Add(myGuiControlContentButton);
				}
			}
			if (m_scenarioTypesList.Controls.Count > 0)
			{
				m_scenarioTypesGroup.SelectByIndex(0);
				base.FocusedControl = m_scenarioTypesList.Controls[0];
			}
			else
			{
				SetDefaultValues();
			}
		}
	}
}
