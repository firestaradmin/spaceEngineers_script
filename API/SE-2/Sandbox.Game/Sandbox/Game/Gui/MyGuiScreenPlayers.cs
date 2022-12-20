using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems.Trading;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.VoiceChat;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.GameServices;
using VRage.Input;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	public class MyGuiScreenPlayers : MyGuiScreenBase
	{
		protected sealed class Promote_003C_003ESystem_UInt64_0023System_Boolean : ICallSite<IMyEventOwner, ulong, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong playerId, in bool promote, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				Promote(playerId, promote);
			}
		}

		protected sealed class ShowPromoteMessage_003C_003EVRage_Game_ModAPI_MyPromoteLevel_0023System_Boolean : ICallSite<IMyEventOwner, MyPromoteLevel, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyPromoteLevel promoteLevel, in bool promote, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowPromoteMessage(promoteLevel, promote);
			}
		}

		protected sealed class RequestPingsAndRefresh_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestPingsAndRefresh();
			}
		}

		protected sealed class SendPingsAndRefresh_003C_003EVRage_Serialization_SerializableDictionary_00602_003CSystem_UInt64_0023System_Int16_003E : ICallSite<IMyEventOwner, SerializableDictionary<ulong, short>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in SerializableDictionary<ulong, short> dictionary, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendPingsAndRefresh(dictionary);
			}
		}

		protected sealed class SyncRemainingTimeWithClient_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SyncRemainingTimeWithClient();
			}
		}

		protected sealed class RecieveTimeSync_003C_003ESystem_Single_0023System_Single : ICallSite<IMyEventOwner, float, float, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float syncTimeSeconds, in float timeLeftSeconds, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RecieveTimeSync(syncTimeSeconds, timeLeftSeconds);
			}
		}

		protected const string OWNER_MARKER = "*****";

		protected const string SERVICE_XBL = "Xbox Live";

		protected const string SERVICE_Steam = "Steam";

		protected int GamePlatformColumn;
<<<<<<< HEAD

		protected int PlayerNameColumn = 1;

		protected int PlayerFactionNameColumn = 2;

		protected int GameAdminColumn = 3;

		protected int GamePingColumn = 4;

		protected int PlayerMutedColumn = 5;

		protected int PlayerTableColumnsCount = 6;

=======

		protected int PlayerNameColumn = 1;

		protected int PlayerFactionNameColumn = 2;

		protected int GameAdminColumn = 3;

		protected int GamePingColumn = 4;

		protected int PlayerMutedColumn = 5;

		protected int PlayerTableColumnsCount = 6;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private int m_warfareUpdate_frameCount = 30;

		private int m_warfareUpdate_frameCount_current = 30;

		private bool m_getPingAndRefresh = true;

		protected MyGuiControlButton m_profileButton;

		protected MyGuiControlButton m_promoteButton;

		protected MyGuiControlButton m_demoteButton;

		protected MyGuiControlButton m_kickButton;

		protected MyGuiControlButton m_banButton;

		protected MyGuiControlButton m_inviteButton;

		protected MyGuiControlButton m_tradeButton;

		protected bool m_isScenarioRunning;

		protected MyGuiControlLabel m_maxPlayersValueLabel;

		protected MyGuiControlTable m_playersTable;

		protected MyGuiControlCombobox m_lobbyTypeCombo;

		protected MyGuiControlSlider m_maxPlayersSlider;

		protected Dictionary<ulong, short> pings = new Dictionary<ulong, short>();

		private MyGuiControlLabel m_warfare_timeRemainting_time;

		private MyGuiControlLabel m_warfare_timeRemainting_label;

		protected ulong m_lastSelected;

		private MyGuiControlLabel m_caption;

		private readonly MyGuiControlButton.StyleDefinition m_buttonSizeStyleMuted = new MyGuiControlButton.StyleDefinition
		{
			NormalFont = "White",
			HighlightFont = "White",
			NormalTexture = MyGuiConstants.TEXTURE_HUD_VOICE_CHAT_MUTED,
			HighlightTexture = MyGuiConstants.TEXTURE_HUD_VOICE_CHAT_MUTED
		};

		private readonly MyGuiControlButton.StyleDefinition m_buttonSizeStyleUnMuted = new MyGuiControlButton.StyleDefinition
		{
			NormalFont = "White",
			HighlightFont = "White",
			NormalTexture = MyGuiConstants.TEXTURE_HUD_VOICE_CHAT,
			HighlightTexture = MyGuiConstants.TEXTURE_HUD_VOICE_CHAT
		};

		private bool m_waitingTradeResponse;

		private int m_maxPlayers;

		private int m_lastMuteIndicatorsUpdate;

		public MyGuiScreenPlayers()
			: base(null, size: new Vector2(0.837f, 0.813f), backgroundColor: MyGuiConstants.SCREEN_BACKGROUND_COLOR, isTopMostScreen: false, backgroundTexture: MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.Texture, backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			MyMultiplayer.Static.ClientJoined += Multiplayer_PlayerJoined;
			MyMultiplayer.Static.ClientLeft += Multiplayer_PlayerLeft;
			MySession.Static.Factions.FactionCreated += OnFactionCreated;
			MySession.Static.Factions.FactionEdited += OnFactionEdited;
			MySession.Static.Factions.FactionStateChanged += OnFactionStateChanged;
			MySession.Static.OnUserPromoteLevelChanged += OnUserPromoteLevelChanged;
			MyMultiplayerLobby myMultiplayerLobby = MyMultiplayer.Static as MyMultiplayerLobby;
			if (myMultiplayerLobby != null)
			{
				myMultiplayerLobby.OnLobbyDataUpdated += Matchmaking_LobbyDataUpdate;
			}
			MyMultiplayerLobbyClient myMultiplayerLobbyClient = MyMultiplayer.Static as MyMultiplayerLobbyClient;
			if (myMultiplayerLobbyClient != null)
			{
				myMultiplayerLobbyClient.OnLobbyDataUpdated += Matchmaking_LobbyDataUpdate;
			}
			RecreateControls(constructor: true);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SyncRemainingTimeWithClient, MyEventContext.Current.Sender);
			MyVoiceChatSessionComponent.Static.OnPlayerMutedStateChanged += OnPlayerMutedStateChanged;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenPlayers";
		}

		protected override void OnClosed()
		{
			if (m_waitingTradeResponse)
			{
				MyTradingManager.Static.TradeCancel_Client();
				m_waitingTradeResponse = false;
			}
			base.OnClosed();
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.ClientJoined -= Multiplayer_PlayerJoined;
				MyMultiplayer.Static.ClientLeft -= Multiplayer_PlayerLeft;
			}
			if (MySession.Static != null)
			{
				MySession.Static.Factions.FactionCreated -= OnFactionCreated;
				MySession.Static.Factions.FactionEdited -= OnFactionEdited;
				MySession.Static.Factions.FactionStateChanged -= OnFactionStateChanged;
				MySession.Static.OnUserPromoteLevelChanged -= OnUserPromoteLevelChanged;
			}
			MyMultiplayerLobby myMultiplayerLobby = MyMultiplayer.Static as MyMultiplayerLobby;
			if (myMultiplayerLobby != null)
			{
				myMultiplayerLobby.OnLobbyDataUpdated -= Matchmaking_LobbyDataUpdate;
			}
			MyMultiplayerLobbyClient myMultiplayerLobbyClient = MyMultiplayer.Static as MyMultiplayerLobbyClient;
			if (myMultiplayerLobbyClient != null)
			{
				myMultiplayerLobbyClient.OnLobbyDataUpdated -= Matchmaking_LobbyDataUpdate;
			}
			MyVoiceChatSessionComponent.Static.OnPlayerMutedStateChanged -= OnPlayerMutedStateChanged;
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			base.CloseButtonEnabled = true;
			Vector2 vector = base.Size.Value / MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.SizeGui;
			_ = -0.5f * base.Size.Value + vector * MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.PaddingSizeGui * 1.1f;
			m_caption = AddCaption(MyCommonTexts.ScreenCaptionPlayers, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.364f, -0.331f), 0.728f);
			Vector2 start = new Vector2(-0.364f, 0.358f);
			myGuiControlSeparatorList.AddHorizontal(start, 0.728f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.36f, 0.05f), 0.17f);
			Controls.Add(myGuiControlSeparatorList);
			Vector2 vector2 = new Vector2(0f, 0.057f);
			Vector2 vector3 = new Vector2(-0.361f, -0.304f);
			m_profileButton = new MyGuiControlButton(vector3, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MyCommonTexts.ScreenPlayers_Profile));
			m_profileButton.ButtonClicked += profileButton_ButtonClicked;
			Controls.Add(m_profileButton);
			vector3 += vector2;
			m_promoteButton = new MyGuiControlButton(vector3, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MyCommonTexts.ScreenPlayers_Promote));
			m_promoteButton.ButtonClicked += promoteButton_ButtonClicked;
			Controls.Add(m_promoteButton);
			vector3 += vector2;
			m_demoteButton = new MyGuiControlButton(vector3, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MyCommonTexts.ScreenPlayers_Demote));
			m_demoteButton.ButtonClicked += demoteButton_ButtonClicked;
			Controls.Add(m_demoteButton);
			vector3 += vector2;
			m_kickButton = new MyGuiControlButton(vector3, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MyCommonTexts.ScreenPlayers_Kick));
			m_kickButton.ButtonClicked += kickButton_ButtonClicked;
			Controls.Add(m_kickButton);
			vector3 += vector2;
			m_banButton = new MyGuiControlButton(vector3, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MyCommonTexts.ScreenPlayers_Ban));
			m_banButton.ButtonClicked += banButton_ButtonClicked;
			Controls.Add(m_banButton);
			vector3 += vector2;
			m_tradeButton = new MyGuiControlButton(vector3, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MySpaceTexts.PlayersScreen_TradeBtn));
			m_tradeButton.SetTooltip(MyTexts.GetString(MySpaceTexts.PlayersScreen_TradeBtn_TTP));
			m_tradeButton.ButtonClicked += tradeButton_ButtonClicked;
			Controls.Add(m_tradeButton);
			bool num = MyMultiplayer.Static != null && MyMultiplayer.Static.IsLobby;
			Vector2 vector4 = vector3 + new Vector2(-0.002f, m_tradeButton.Size.Y + 0.03f);
			MyGuiControlLabel control = new MyGuiControlLabel(vector4, null, MyTexts.GetString(MySpaceTexts.PlayersScreen_LobbyType), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			if (num)
			{
				Controls.Add(control);
			}
			vector4 += new Vector2(0f, 0.033f);
			m_lobbyTypeCombo = new MyGuiControlCombobox(vector4, null, null, null, 3);
			m_lobbyTypeCombo.Size = new Vector2(0.175f, 0.04f);
			m_lobbyTypeCombo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_lobbyTypeCombo.AddItem(0L, MyCommonTexts.ScreenPlayersLobby_Private);
			m_lobbyTypeCombo.AddItem(1L, MyCommonTexts.ScreenPlayersLobby_Friends);
			m_lobbyTypeCombo.AddItem(2L, MyCommonTexts.ScreenPlayersLobby_Public);
			m_lobbyTypeCombo.SelectItemByKey((long)MyMultiplayer.Static.GetLobbyType());
			if (num)
			{
				Controls.Add(m_lobbyTypeCombo);
			}
			Vector2 vector5 = vector4 + new Vector2(0f, 0.05f);
			MyGuiControlLabel control2 = new MyGuiControlLabel(vector5 + new Vector2(0.001f, 0f), null, MyTexts.GetString(MyCommonTexts.MaxPlayers) + ":", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			if (num)
			{
				Controls.Add(control2);
			}
			m_maxPlayersValueLabel = new MyGuiControlLabel(vector5 + new Vector2(0.169f, 0f), null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			if (num)
			{
				Controls.Add(m_maxPlayersValueLabel);
			}
			vector5 += new Vector2(0f, 0.03f);
			m_maxPlayers = (Sync.IsServer ? MyMultiplayerLobby.MAX_PLAYERS : 16);
			m_maxPlayersSlider = new MyGuiControlSlider(vector5, 2f, Math.Max(m_maxPlayers, 3), 0.177f, Sync.IsServer ? MySession.Static.MaxPlayers : MyMultiplayer.Static.MemberLimit, null, null, 1, 0.8f, 0f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, intValue: true);
			m_isScenarioRunning = MyCampaignManager.Static.IsScenarioRunning;
			m_maxPlayersSlider.Enabled = !m_isScenarioRunning;
			m_maxPlayersValueLabel.Text = m_maxPlayersSlider.Value.ToString();
			m_maxPlayersSlider.ValueChanged = MaxPlayersSlider_Changed;
			if (num)
			{
				Controls.Add(m_maxPlayersSlider);
			}
			m_inviteButton = new MyGuiControlButton(new Vector2(-0.361f, 0.285000026f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MyCommonTexts.ScreenPlayers_Invite));
			m_inviteButton.ButtonClicked += inviteButton_ButtonClicked;
			Controls.Add(m_inviteButton);
			Vector2 vector6 = new Vector2(0.364f, -0.307f);
			Vector2 size = new Vector2(0.54f, 0.813f);
			int num2 = 18;
			float num3 = 0f;
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component.IsEnabled)
			{
				Vector2 vector7 = GetPositionAbsolute() + vector6 + new Vector2(0f - size.X, 0f);
				m_warfare_timeRemainting_label = new MyGuiControlLabel(vector6 - new Vector2(size.X, 0f));
				m_warfare_timeRemainting_label.Text = MyTexts.GetString(MySpaceTexts.WarfareCounter_TimeRemaining).ToString() + ": ";
				Controls.Add(m_warfare_timeRemainting_label);
				TimeSpan timeSpan = TimeSpan.FromMinutes(component.RemainingMinutes);
				m_warfare_timeRemainting_time = new MyGuiControlLabel(vector6 - new Vector2(size.X, 0f) + new Vector2(m_warfare_timeRemainting_label.Size.X, 0f));
				m_warfare_timeRemainting_time.Text = timeSpan.ToString((timeSpan.TotalHours >= 1.0) ? "hh\\:mm\\:ss" : "mm\\:ss");
				Controls.Add(m_warfare_timeRemainting_time);
				float num4 = 0.09f;
				float num5 = size.X / 3f - 2f * num3;
				int num6 = 0;
				MyFaction[] allFactions = MySession.Static.Factions.GetAllFactions();
				foreach (MyFaction myFaction in allFactions)
				{
					if ((myFaction.Name.StartsWith("Red") || myFaction.Name.StartsWith("Green") || myFaction.Name.StartsWith("Blue")) && myFaction.Name.EndsWith("Faction"))
					{
						Controls.Add(new MyGuiScreenPlayersWarfareTeamScoreTable(vector7 + new Vector2((float)num6 * (num5 + num3), m_warfare_timeRemainting_label.Size.Y + num3), num5, num4, myFaction.Name, myFaction.FactionIcon.Value.String, MyTexts.GetString(MySpaceTexts.WarfareCounter_EscapePod), myFaction.FactionId, drawOwnBackground: false, drawBorders: true, myFaction.IsMember(MySession.Static.LocalHumanPlayer.Identity.IdentityId)));
						num6++;
					}
				}
				vector6.Y += m_warfare_timeRemainting_label.Size.Y + num4 + num3 * 2f;
				num2 -= 3;
			}
			m_playersTable = new MyGuiControlTable
			{
				Position = vector6,
				Size = size,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				ColumnsCount = PlayerTableColumnsCount
			};
			m_playersTable.GamepadHelpTextId = MySpaceTexts.PlayersScreen_Help_PlayersList;
			m_playersTable.VisibleRowsCount = num2;
			float num7 = 0.3f;
			float num8 = 0.12f;
			float num9 = 0.12f;
			float num10 = 0.13f;
			float num11 = 0.1f;
			m_playersTable.SetCustomColumnWidths(new float[6]
			{
				num11,
				num7,
				1f - num7 - num8 - num10 - num9 - num11,
				num8,
				num9,
				num10
			});
			m_playersTable.SetColumnComparison(PlayerNameColumn, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => a.Text.CompareToIgnoreCase(b.Text));
			m_playersTable.SetColumnName(PlayerNameColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_PlayerName));
			m_playersTable.SetColumnComparison(PlayerFactionNameColumn, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => a.Text.CompareToIgnoreCase(b.Text));
			m_playersTable.SetColumnName(PlayerFactionNameColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_FactionName));
			m_playersTable.SetColumnName(PlayerMutedColumn, new StringBuilder(MyTexts.GetString(MyCommonTexts.ScreenPlayers_Muted)));
			m_playersTable.SetColumnComparison(GameAdminColumn, GameAdminCompare);
			m_playersTable.SetColumnName(GameAdminColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_Rank));
			m_playersTable.SetColumnComparison(GamePingColumn, GamePingCompare);
			m_playersTable.SetColumnName(GamePingColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_Ping));
			m_playersTable.SetColumnComparison(GamePlatformColumn, PlatformCompare);
			m_playersTable.ItemSelected += playersTable_ItemSelected;
			m_playersTable.UpdateTableSortHelpText();
			Controls.Add(m_playersTable);
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Id.SerialId != 0)
<<<<<<< HEAD
				{
					continue;
				}
				for (int j = 0; j < m_playersTable.RowsCount; j++)
				{
=======
				{
					continue;
				}
				for (int j = 0; j < m_playersTable.RowsCount; j++)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyGuiControlTable.Row row = m_playersTable.GetRow(j);
					if (row.UserData is ulong)
					{
						_ = (ulong)row.UserData;
						_ = onlinePlayer.Id.SteamId;
					}
				}
				AddPlayer(onlinePlayer.Id.SteamId);
			}
			m_lobbyTypeCombo.ItemSelected += lobbyTypeCombo_OnSelect;
			if (m_lastSelected != 0L)
			{
				MyGuiControlTable.Row row2 = m_playersTable.Find((MyGuiControlTable.Row r) => (ulong)r.UserData == m_lastSelected);
				if (row2 != null)
				{
					m_playersTable.SelectedRow = row2;
				}
			}
			UpdateButtonsEnabledState();
			UpdateCaption();
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(start.X, start.Y + minSizeGui.Y / 2f));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.GamepadHelpTextId = MySpaceTexts.PlayersScreen_Help_Screen;
			base.FocusedControl = m_playersTable;
		}

		private void profileButton_ButtonClicked(MyGuiControlButton obj)
		{
			MyGuiControlTable.Row selectedRow = m_playersTable.SelectedRow;
			if (selectedRow != null)
			{
				MyGameService.OpenOverlayUser((ulong)selectedRow.UserData);
			}
		}

		private void OnPlayerMutedStateChanged(ulong playerId, bool isMuted)
		{
			RefreshMuteIcons();
		}

		private void tradeButton_ButtonClicked(MyGuiControlButton obj)
		{
			ulong num = ((m_playersTable.SelectedRow != null) ? ((ulong)m_playersTable.SelectedRow.UserData) : 0);
			if (num != 0L && num != MyGameService.UserId)
			{
				m_waitingTradeResponse = true;
				MyTradingManager.Static.TradeRequest_Client(MyGameService.UserId, num, OnAnswerRecieved);
				if (m_waitingTradeResponse)
				{
					m_tradeButton.Enabled = false;
					m_tradeButton.Text = MyTexts.GetString(MySpaceTexts.PlayersScreen_TradeBtn_Waiting);
				}
			}
		}

		private void OnAnswerRecieved(MyTradeResponseReason reason)
		{
			m_waitingTradeResponse = false;
			UpdateTradeButton();
			m_tradeButton.Text = MyTexts.GetString(MySpaceTexts.PlayersScreen_TradeBtn);
		}

		private int GameAdminCompare(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
		{
			ulong steamId = (ulong)a.Row.UserData;
			ulong steamId2 = (ulong)b.Row.UserData;
			int userPromoteLevel = (int)MySession.Static.GetUserPromoteLevel(steamId);
			int userPromoteLevel2 = (int)MySession.Static.GetUserPromoteLevel(steamId2);
			return userPromoteLevel.CompareTo(userPromoteLevel2);
		}

		private int GamePingCompare(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
		{
			if (!int.TryParse(a.Text.ToString(), out var result))
			{
				result = -1;
			}
			if (!int.TryParse(b.Text.ToString(), out var result2))
			{
				result2 = -1;
			}
			return result.CompareTo(result2);
		}

		private int PlatformCompare(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
<<<<<<< HEAD
		{
			return a.Text.CompareTo(b.Text);
		}

		protected void OnToggleMutePressed(MyGuiControlButton button)
		{
=======
		{
			return a.Text.CompareTo(b.Text);
		}

		protected void OnToggleMutePressed(MyGuiControlButton button)
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ulong num = (ulong)button.UserData;
			bool muted = button.CustomStyle == m_buttonSizeStyleUnMuted;
			string memberServiceName = MyMultiplayer.Static.GetMemberServiceName(num);
			if (MyGameService.Service.ServiceName == "Xbox Live" && memberServiceName == "Xbox Live")
			{
				profileButton_ButtonClicked(null);
				return;
			}
			MyVoiceChatSessionComponent.Static.SetPlayerMuted(num, muted);
			RefreshMuteIcons();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F3))
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
				CloseScreen();
			}
			if (base.FocusedControl == m_playersTable && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				tradeButton_ButtonClicked(null);
			}
			if (base.FocusedControl == m_playersTable && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT))
			{
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				profileButton_ButtonClicked(null);
			}
			MyGuiControlButton myGuiControlButton;
			if (base.FocusedControl == m_playersTable && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y) && (myGuiControlButton = m_playersTable.GetInnerControlsFromCurrentCell(PlayerMutedColumn) as MyGuiControlButton) != null)
			{
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				myGuiControlButton.PressButton();
			}
		}

		protected void AddPlayer(ulong userId)
		{
			string memberName = MyMultiplayer.Static.GetMemberName(userId);
			if (string.IsNullOrEmpty(memberName))
			{
				return;
			}
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(userId);
			string memberServiceName = MyMultiplayer.Static.GetMemberServiceName(userId);
			StringBuilder text = new StringBuilder();
			MyGuiHighlightTexture? icon = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\Services\\" + memberServiceName + ".png",
				Highlight = "Textures\\GUI\\Icons\\Services\\" + memberServiceName + ".png",
				SizePx = new Vector2(24f, 24f)
			};
			row.AddCell(new MyGuiControlTable.Cell(text, null, memberServiceName, Color.White, icon, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			StringBuilder text2 = new StringBuilder(memberName);
			icon = null;
			row.AddCell(new MyGuiControlTable.Cell(text2, memberName, null, null, icon));
			long playerId = Sync.Players.TryGetIdentityId(userId);
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(playerId);
			string text3 = "";
			StringBuilder stringBuilder = new StringBuilder();
			if (playerFaction != null)
			{
				text3 += playerFaction.Name;
				text3 = text3 + " | " + memberName;
				foreach (KeyValuePair<long, MyFactionMember> member in playerFaction.Members)
				{
					if ((member.Value.IsLeader || member.Value.IsFounder) && MySession.Static.Players.TryGetPlayerId(member.Value.PlayerId, out var result) && MySession.Static.Players.TryGetPlayerById(result, out var player))
					{
						text3 = text3 + " | " + player.DisplayName;
						break;
					}
				}
				stringBuilder.Append(MyStatControlText.SubstituteTexts(playerFaction.Name));
				if (playerFaction.IsLeader(playerId))
				{
					stringBuilder.Append(" (").Append((object)MyTexts.Get(MyCommonTexts.Leader)).Append(")");
				}
				if (!string.IsNullOrEmpty(playerFaction.Tag))
				{
					stringBuilder.Insert(0, "[" + playerFaction.Tag + "] ");
				}
			}
<<<<<<< HEAD
			string toolTip = text3;
			icon = null;
			row.AddCell(new MyGuiControlTable.Cell(stringBuilder, null, toolTip, null, icon));
=======
			icon = null;
			row.AddCell(new MyGuiControlTable.Cell(stringBuilder, null, null, null, icon));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			StringBuilder stringBuilder2 = new StringBuilder();
			MyPromoteLevel userPromoteLevel = MySession.Static.GetUserPromoteLevel(userId);
			for (int i = 0; i < (int)userPromoteLevel; i++)
			{
				stringBuilder2.Append("*");
			}
			icon = null;
			row.AddCell(new MyGuiControlTable.Cell(stringBuilder2, null, null, null, icon));
			if (pings.ContainsKey(userId))
			{
<<<<<<< HEAD
				StringBuilder text4 = new StringBuilder(pings[userId].ToString());
				icon = null;
				row.AddCell(new MyGuiControlTable.Cell(text4, null, null, null, icon));
			}
			else
			{
				StringBuilder text5 = new StringBuilder("----");
				icon = null;
				row.AddCell(new MyGuiControlTable.Cell(text5, null, null, null, icon));
			}
			StringBuilder text6 = new StringBuilder("");
			icon = null;
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(text6, null, null, null, icon);
=======
				StringBuilder text3 = new StringBuilder(pings[userId].ToString());
				icon = null;
				row.AddCell(new MyGuiControlTable.Cell(text3, null, null, null, icon));
			}
			else
			{
				StringBuilder text4 = new StringBuilder("----");
				icon = null;
				row.AddCell(new MyGuiControlTable.Cell(text4, null, null, null, icon));
			}
			StringBuilder text5 = new StringBuilder("");
			icon = null;
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(text5, null, null, null, icon);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			row.AddCell(cell);
			if (userId != Sync.MyId)
			{
				MyGuiControlButton myGuiControlButton = new MyGuiControlButton();
				myGuiControlButton.CustomStyle = m_buttonSizeStyleUnMuted;
				myGuiControlButton.Size = new Vector2(0.03f, 0.04f);
				myGuiControlButton.CueEnum = GuiSounds.None;
				myGuiControlButton.ButtonClicked += OnToggleMutePressed;
				myGuiControlButton.UserData = userId;
				cell.Control = myGuiControlButton;
				m_playersTable.Controls.Add(myGuiControlButton);
				RefreshMuteIcons();
			}
			m_playersTable.Add(row);
			UpdateCaption();
		}

		protected void RemovePlayer(ulong userId)
		{
			m_playersTable.Remove((MyGuiControlTable.Row row) => (ulong)row.UserData == userId);
			UpdateButtonsEnabledState();
			if (MySession.Static != null)
			{
				UpdateCaption();
			}
		}

		private void UpdateCaption()
		{
			string text = string.Empty;
			MyMultiplayerClient myMultiplayerClient = MyMultiplayer.Static as MyMultiplayerClient;
			if (myMultiplayerClient != null)
			{
				if (myMultiplayerClient.Server != null)
				{
					text = myMultiplayerClient.Server.Name;
				}
			}
			else
			{
				MyMultiplayerLobbyClient myMultiplayerLobbyClient = MyMultiplayer.Static as MyMultiplayerLobbyClient;
				if (myMultiplayerLobbyClient != null)
				{
					text = myMultiplayerLobbyClient.HostName;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				m_caption.Text = string.Concat(MyTexts.Get(MyCommonTexts.ScreenCaptionPlayers), " (", m_playersTable.RowsCount, " / ", MySession.Static.MaxPlayers, ")");
			}
			else
			{
				m_caption.Text = string.Concat(MyTexts.Get(MyCommonTexts.ScreenCaptionServerName), text, "  -  ", MyTexts.Get(MyCommonTexts.ScreenCaptionPlayers), " (", m_playersTable.RowsCount, " / ", MySession.Static.MaxPlayers, ")");
			}
		}

		protected void UpdateButtonsEnabledState()
		{
			if (MyMultiplayer.Static != null)
			{
				bool flag = m_playersTable.SelectedRow != null;
				ulong userId = MyGameService.UserId;
				ulong owner = MyMultiplayer.Static.GetOwner();
				ulong num = (flag ? ((ulong)m_playersTable.SelectedRow.UserData) : 0);
				bool flag2 = userId == num;
				bool flag3 = MySession.Static.IsUserAdmin(userId);
				bool flag4 = userId == owner;
				bool flag5 = flag && MySession.Static.CanPromoteUser(Sync.MyId, num);
				bool enabled = flag && MySession.Static.CanDemoteUser(Sync.MyId, num);
				m_lobbyTypeCombo.GetSelectedKey();
				if (flag && !flag2)
				{
					m_promoteButton.Enabled = flag5;
					m_demoteButton.Enabled = enabled;
					m_kickButton.Enabled = flag5 && flag3;
					m_banButton.Enabled = flag5 && flag3;
				}
				else
				{
					m_promoteButton.Enabled = false;
					m_demoteButton.Enabled = false;
					m_kickButton.Enabled = false;
					m_banButton.Enabled = false;
				}
				m_banButton.Enabled &= MyMultiplayer.Static is MyMultiplayerClient;
				m_inviteButton.Enabled = MyGameService.IsInviteSupported();
				m_lobbyTypeCombo.Enabled = flag4;
				m_maxPlayersSlider.Enabled = flag4 && m_maxPlayers > 2 && !m_isScenarioRunning;
				m_profileButton.Enabled = flag;
				UpdateTradeButton();
			}
		}

		private void UpdateTradeButton()
		{
			bool num = m_playersTable.SelectedRow != null;
			ulong userId = MyGameService.UserId;
			ulong num2 = (num ? ((ulong)m_playersTable.SelectedRow.UserData) : 0);
			bool flag = userId == num2;
			bool flag2 = MyTradingManager.ValidateTradeProssible(userId, num2, out var _, out var _) == MyTradeResponseReason.Ok;
			flag2 = !flag && flag2;
			m_tradeButton.Enabled = flag2;
		}

		protected void Multiplayer_PlayerJoined(ulong userId, string userName)
		{
			AddPlayer(userId);
		}

		protected void Multiplayer_PlayerLeft(ulong userId, MyChatMemberStateChangeEnum arg2)
		{
			RemovePlayer(userId);
		}

		protected void Matchmaking_LobbyDataUpdate(bool success, IMyLobby lobby, ulong memberOrLobby)
		{
			if (success)
			{
				ulong newOwnerId = lobby.OwnerId;
				MyGuiControlTable.Row row2 = m_playersTable.Find((MyGuiControlTable.Row row) => row.GetCell(GameAdminColumn).Text.Length == "*****".Length);
				MyGuiControlTable.Row row3 = m_playersTable.Find((MyGuiControlTable.Row row) => (ulong)row.UserData == newOwnerId);
				row2?.GetCell(GameAdminColumn).Text.Clear();
				row3?.GetCell(GameAdminColumn).Text.Clear().Append("*****");
				MyLobbyType lobbyType = lobby.LobbyType;
				m_lobbyTypeCombo.SelectItemByKey((long)lobbyType, sendEvent: false);
				MySession.Static.Settings.OnlineMode = GetOnlineMode(lobbyType);
				UpdateButtonsEnabledState();
				if (!Sync.IsServer)
				{
					m_maxPlayersSlider.ValueChanged = null;
					MySession.Static.Settings.MaxPlayers = (short)MyMultiplayer.Static.MemberLimit;
					m_maxPlayersSlider.Value = MySession.Static.MaxPlayers;
					m_maxPlayersSlider.ValueChanged = MaxPlayersSlider_Changed;
					m_maxPlayersValueLabel.Text = m_maxPlayersSlider.Value.ToString();
					UpdateCaption();
				}
			}
		}

		protected MyOnlineModeEnum GetOnlineMode(MyLobbyType lobbyType)
		{
			return lobbyType switch
			{
				MyLobbyType.Private => MyOnlineModeEnum.PRIVATE, 
				MyLobbyType.FriendsOnly => MyOnlineModeEnum.FRIENDS, 
				MyLobbyType.Public => MyOnlineModeEnum.PUBLIC, 
				_ => MyOnlineModeEnum.PUBLIC, 
			};
		}

		protected void playersTable_ItemSelected(MyGuiControlTable table, MyGuiControlTable.EventArgs args)
		{
			UpdateButtonsEnabledState();
			if (m_playersTable.SelectedRow != null)
			{
				m_lastSelected = (ulong)m_playersTable.SelectedRow.UserData;
			}
		}

		protected void inviteButton_ButtonClicked(MyGuiControlButton obj)
		{
			MyGameService.OpenInviteOverlay();
		}

		protected void promoteButton_ButtonClicked(MyGuiControlButton obj)
		{
			MyGuiControlTable.Row selectedRow = m_playersTable.SelectedRow;
			if (selectedRow != null && MySession.Static.CanPromoteUser(Sync.MyId, (ulong)selectedRow.UserData))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => Promote, (ulong)selectedRow.UserData, arg3: true);
			}
		}

		protected void demoteButton_ButtonClicked(MyGuiControlButton obj)
		{
			MyGuiControlTable.Row selectedRow = m_playersTable.SelectedRow;
			if (selectedRow != null && MySession.Static.CanDemoteUser(Sync.MyId, (ulong)selectedRow.UserData))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => Promote, (ulong)selectedRow.UserData, arg3: false);
			}
		}

<<<<<<< HEAD
		[Event(null, 875)]
=======
		[Event(null, 860)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void Promote(ulong playerId, bool promote)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && ((promote && !MySession.Static.CanPromoteUser(MyEventContext.Current.Sender.Value, playerId)) || (!promote && !MySession.Static.CanDemoteUser(MyEventContext.Current.Sender.Value, playerId))))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, kick: false);
			}
			else
			{
				PromoteImplementation(playerId, promote);
			}
		}

		public static void PromoteImplementation(ulong playerId, bool promote)
		{
			MyPromoteLevel userPromoteLevel = MySession.Static.GetUserPromoteLevel(playerId);
			if (promote)
			{
				if (userPromoteLevel >= MyPromoteLevel.Admin)
				{
					return;
				}
				userPromoteLevel++;
				if (!MySession.Static.EnableScripterRole && userPromoteLevel == MyPromoteLevel.Scripter)
				{
					userPromoteLevel++;
				}
			}
			else
			{
				if (userPromoteLevel == MyPromoteLevel.None)
				{
					return;
				}
				userPromoteLevel--;
				if (!MySession.Static.EnableScripterRole && userPromoteLevel == MyPromoteLevel.Scripter)
				{
					userPromoteLevel--;
				}
			}
			MySession.Static.SetUserPromoteLevel(playerId, userPromoteLevel);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ShowPromoteMessage, userPromoteLevel, promote, new EndpointId(playerId));
		}

<<<<<<< HEAD
		[Event(null, 924)]
=======
		[Event(null, 909)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		protected static void ShowPromoteMessage(MyPromoteLevel promoteLevel, bool promote)
		{
			ClearPromoteNotificaions();
			switch (promoteLevel)
			{
			case MyPromoteLevel.None:
				MyHud.Notifications.Add(MyNotificationSingletons.PlayerDemotedNone);
				break;
			case MyPromoteLevel.Scripter:
				MyHud.Notifications.Add(promote ? MyNotificationSingletons.PlayerPromotedScripter : MyNotificationSingletons.PlayerDemotedScripter);
				break;
			case MyPromoteLevel.Moderator:
				MyHud.Notifications.Add(promote ? MyNotificationSingletons.PlayerPromotedModerator : MyNotificationSingletons.PlayerDemotedModerator);
				break;
			case MyPromoteLevel.SpaceMaster:
				MyHud.Notifications.Add(promote ? MyNotificationSingletons.PlayerPromotedSpaceMaster : MyNotificationSingletons.PlayerDemotedSpaceMaster);
				break;
			case MyPromoteLevel.Admin:
				MyHud.Notifications.Add(MyNotificationSingletons.PlayerPromotedAdmin);
				break;
			default:
				throw new ArgumentOutOfRangeException("promoteLevel", promoteLevel, null);
			case MyPromoteLevel.Owner:
				break;
			}
		}

		private static void ClearPromoteNotificaions()
		{
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerDemotedNone);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerDemotedScripter);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerDemotedModerator);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerDemotedSpaceMaster);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerPromotedScripter);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerPromotedModerator);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerPromotedSpaceMaster);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerPromotedAdmin);
		}

		protected static void Refresh()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyScreenManager.GetFirstScreenOfType<MyGuiScreenPlayers>()?.RecreateControls(constructor: false);
			}
		}

		protected void kickButton_ButtonClicked(MyGuiControlButton obj)
		{
			MyGuiControlTable.Row selectedRow = m_playersTable.SelectedRow;
			if (selectedRow != null)
			{
				MyMultiplayer.Static.KickClient((ulong)selectedRow.UserData);
			}
		}

		protected void banButton_ButtonClicked(MyGuiControlButton obj)
		{
			MyGuiControlTable.Row selectedRow = m_playersTable.SelectedRow;
			if (selectedRow != null)
			{
				MyMultiplayer.Static.BanClient((ulong)selectedRow.UserData, banned: true);
			}
		}

		protected void lobbyTypeCombo_OnSelect()
		{
			MyLobbyType lobbyType = (MyLobbyType)m_lobbyTypeCombo.GetSelectedKey();
			m_lobbyTypeCombo.SelectItemByKey((long)MyMultiplayer.Static.GetLobbyType(), sendEvent: false);
			MyMultiplayer.Static.SetLobbyType(lobbyType);
		}

		protected void MaxPlayersSlider_Changed(MyGuiControlSlider control)
		{
			MySession.Static.Settings.MaxPlayers = (short)m_maxPlayersSlider.Value;
			MyMultiplayer.Static.SetMemberLimit(MySession.Static.MaxPlayers);
			m_maxPlayersValueLabel.Text = m_maxPlayersSlider.Value.ToString();
			UpdateCaption();
		}

		private void OnFactionCreated(long insertedId)
		{
			RefreshPlusPings();
		}

		private void OnFactionEdited(long editedId)
		{
			RefreshPlusPings();
		}

		private void OnFactionStateChanged(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId, long senderId)
		{
			RefreshPlusPings();
		}

		private void OnUserPromoteLevelChanged(ulong steamId, MyPromoteLevel promotionLevel)
		{
			for (int i = 0; i < m_playersTable.RowsCount; i++)
			{
				MyGuiControlTable.Row row = m_playersTable.GetRow(i);
				if (row.UserData is ulong && (ulong)row.UserData == steamId)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int j = 0; j < (int)promotionLevel; j++)
					{
						stringBuilder.Append("*");
					}
					MyGuiControlTable.Cell cell = row.GetCell(GameAdminColumn);
					cell.Text.Clear();
					cell.Text.Append((object)stringBuilder);
					break;
				}
			}
			UpdateButtonsEnabledState();
			if (!m_promoteButton.Enabled && m_promoteButton.HasFocus)
			{
				if (m_demoteButton.Enabled)
				{
					base.FocusedControl = m_demoteButton;
				}
			}
			else if (!m_demoteButton.Enabled && m_demoteButton.HasFocus && m_promoteButton.Enabled)
			{
				base.FocusedControl = m_promoteButton;
			}
		}

		public static void RefreshPlusPings()
		{
			if (Sync.IsServer)
			{
				if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static != null)
				{
					MyReplicationServer myReplicationServer = MyMultiplayer.Static.ReplicationLayer as MyReplicationServer;
					if (myReplicationServer != null)
					{
						myReplicationServer.GetClientPings(out var dictionary);
						SendPingsAndRefresh(dictionary);
					}
				}
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RequestPingsAndRefresh);
			}
		}

<<<<<<< HEAD
		[Event(null, 1090)]
=======
		[Event(null, 1075)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void RequestPingsAndRefresh()
		{
			if (!Sync.IsServer || MySession.Static == null)
			{
				return;
			}
			MyReplicationServer myReplicationServer = MyMultiplayer.Static.ReplicationLayer as MyReplicationServer;
			if (myReplicationServer != null)
			{
				myReplicationServer.GetClientPings(out var arg);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SendPingsAndRefresh, arg, new EndpointId(MyEventContext.Current.Sender.Value));
			}
		}

<<<<<<< HEAD
		[Event(null, 1111)]
=======
		[Event(null, 1096)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void SendPingsAndRefresh(SerializableDictionary<ulong, short> dictionary)
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			MyGuiScreenPlayers firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenPlayers>();
			if (firstScreenOfType == null)
			{
				return;
			}
			firstScreenOfType.pings.Clear();
			foreach (KeyValuePair<ulong, short> item in dictionary.Dictionary)
			{
				firstScreenOfType.pings[item.Key] = item.Value;
			}
			firstScreenOfType.RecreateControls(constructor: false);
		}

		public override bool Draw()
		{
			bool result = base.Draw();
			if (m_getPingAndRefresh)
			{
				m_getPingAndRefresh = false;
				RefreshPlusPings();
			}
			return result;
		}

		private void RefreshMuteIcons()
		{
			m_lastMuteIndicatorsUpdate = 0;
		}

		public override bool Update(bool hasFocus)
		{
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (component.IsEnabled && m_warfareUpdate_frameCount_current >= m_warfareUpdate_frameCount)
			{
				m_warfareUpdate_frameCount_current = 0;
				foreach (MyGuiControlBase control in Controls)
				{
					if (!(control.GetType() == typeof(MyGuiScreenPlayersWarfareTeamScoreTable)))
					{
						continue;
					}
					IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(((MyGuiScreenPlayersWarfareTeamScoreTable)control).FactionId);
					if (myFaction != null && myFaction.FactionId == ((MyGuiScreenPlayersWarfareTeamScoreTable)control).FactionId)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyFactionCollection.RequestFactionScoreAndPercentageUpdate, myFaction.FactionId, MyEventContext.Current.Sender);
					}
				}
			}
			m_warfareUpdate_frameCount_current++;
			if (component != null && component.IsEnabled)
			{
				TimeSpan timeSpan = TimeSpan.FromMinutes(component.RemainingMinutes);
				string text = timeSpan.ToString((timeSpan.TotalHours >= 1.0) ? "hh\\:mm\\:ss" : "mm\\:ss");
				if (m_warfare_timeRemainting_time.Text != text)
				{
					m_warfare_timeRemainting_time.Text = text;
				}
			}
			foreach (MyGuiControlBase control2 in Controls)
			{
				if (control2.GetType() == typeof(MyGuiScreenPlayersWarfareTeamScoreTable))
				{
					IMyFaction myFaction2 = MySession.Static.Factions.TryGetFactionById(((MyGuiScreenPlayersWarfareTeamScoreTable)control2).FactionId);
					if (myFaction2 != null && myFaction2.FactionId == ((MyGuiScreenPlayersWarfareTeamScoreTable)control2).FactionId)
					{
						((MyGuiScreenPlayersWarfareTeamScoreTable)control2).ScorePoints = myFaction2.Score;
						((MyGuiScreenPlayersWarfareTeamScoreTable)control2).ObjectiveFinishedPercentage = myFaction2.ObjectivePercentageCompleted;
					}
				}
			}
			m_tradeButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_profileButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			if (MyGuiManager.TotalTimeInMilliseconds - m_lastMuteIndicatorsUpdate > 1000)
			{
				for (int i = 0; i < m_playersTable.RowsCount; i++)
				{
					MyGuiControlButton myGuiControlButton;
					if ((myGuiControlButton = m_playersTable.GetRow(i).GetCell(PlayerMutedColumn).Control as MyGuiControlButton) != null)
					{
						bool flag = MyGameService.GetPlayerMutedState((ulong)myGuiControlButton.UserData) == MyPlayerChatState.Muted;
						myGuiControlButton.CustomStyle = (flag ? m_buttonSizeStyleMuted : m_buttonSizeStyleUnMuted);
					}
				}
			}
			return base.Update(hasFocus);
		}

<<<<<<< HEAD
		[Event(null, 1205)]
=======
		[Event(null, 1190)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void SyncRemainingTimeWithClient()
		{
			if (MySession.Static.IsServer)
			{
				MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RecieveTimeSync, (float)MySession.Static.ElapsedGameTime.TotalSeconds, (float)component.RemainingTime.Seconds, MyEventContext.Current.Sender);
			}
		}

<<<<<<< HEAD
		[Event(null, 1215)]
=======
		[Event(null, 1200)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void RecieveTimeSync(float syncTimeSeconds, float timeLeftSeconds)
		{
			MySessionComponentMatch.SetTimeRemainingInternal(syncTimeSeconds, timeLeftSeconds);
		}
	}
}
