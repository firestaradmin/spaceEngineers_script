using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ObjectBuilders.Gui;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenJoinGame : MyGuiScreenBase
	{
		private class CellRemainingTime : MyGuiControlTable.Cell
		{
			private readonly DateTime m_timeEstimatedEnd;

			public CellRemainingTime(float remainingTime)
				: base("")
			{
				m_timeEstimatedEnd = DateTime.UtcNow + TimeSpan.FromSeconds(remainingTime);
				FillText();
			}

			public override void Update()
			{
				base.Update();
				FillText();
			}

			private void FillText()
			{
				TimeSpan timeSpan = m_timeEstimatedEnd - DateTime.UtcNow;
				if (timeSpan < TimeSpan.Zero)
				{
					timeSpan = TimeSpan.Zero;
				}
				Text.Clear().Append(timeSpan.ToString("mm\\:ss"));
			}
		}

		private enum RefreshStateEnum
		{
			Pause,
			Resume,
			Refresh
		}

		private enum ContextMenuFavoriteAction
		{
			Add,
			Remove
		}

		private struct ContextMenuFavoriteActionItem
		{
			public MyGameServerItem Server;

			public ContextMenuFavoriteAction _Action;
		}

		private MyGuiControlTabControl m_joinGameTabs;

		private MyGuiControlContextMenu m_contextMenu;

		private readonly StringBuilder m_textCache = new StringBuilder();

		private readonly StringBuilder m_gameTypeText = new StringBuilder();

		private readonly StringBuilder m_gameTypeToolTip = new StringBuilder();

		private MyGuiControlTable m_gamesTable;

		private MyGuiControlButton m_joinButton;

		private MyGuiControlButton m_refreshButton;

		private MyGuiControlButton m_detailsButton;

		private MyGuiControlButton m_directConnectButton;

		private MyGuiControlSearchBox m_searchBox;

		private MyGuiControlButton m_advancedSearchButton;

		private MyGuiControlRotatingWheel m_loadingWheel;

		private readonly string m_dataHash;

		private bool m_searchChanged;

		private DateTime m_searchLastChanged = DateTime.Now;

		private Action m_searchChangedFunc;

		private MyRankedServers m_rankedServers;

		public MyGuiControlTabPage m_selectedPage;

		private int m_remainingTimeUpdateFrame;

		public MyServerFilterOptions FilterOptions;

		public bool EnableAdvancedSearch;

		public bool refresh_favorites;

		private MyGuiControlImageButton m_bannerImage;

		private Action m_stopServerRequest;

		private readonly bool m_enableDedicatedServers;

		private readonly List<MyGuiControlButton> m_networkingButtons = new List<MyGuiControlButton>();

		private readonly List<MyGuiControlImage> m_networkingIcons = new List<MyGuiControlImage>();

		private MyServerDiscoveryAggregator m_serverDiscoveryAggregator;

		private bool m_networkingService0Selected;

		private bool m_networkingButtonsVisible;

		private IMyServerDiscovery m_currentServerDiscovery;

		private IMyServerDiscovery m_serverDiscovery;

		private MyGuiControlTabPage m_serversPage;

		private readonly HashSet<MyCachedServerItem> m_dedicatedServers = new HashSet<MyCachedServerItem>();

		private RefreshStateEnum m_nextState;

		private bool m_refreshPaused;

		private bool m_dedicatedResponding;

		private bool m_lastVersionCheck;

		private readonly List<IMyLobby> m_lobbies = new List<IMyLobby>();

		private MyGuiControlTabPage m_lobbyPage;

		private MyGuiControlTabPage m_favoritesPage;

		private HashSet<MyCachedServerItem> m_favoriteServers = new HashSet<MyCachedServerItem>();

		private bool m_favoritesResponding;

		private MyGuiControlTabPage m_historyPage;

		private HashSet<MyCachedServerItem> m_historyServers = new HashSet<MyCachedServerItem>();

		private bool m_historyResponding;

		private MyGuiControlTabPage m_LANPage;

		private HashSet<MyCachedServerItem> m_lanServers = new HashSet<MyCachedServerItem>();

		private bool m_lanResponding;

		private MyGuiControlTabPage m_friendsPage;

		private HashSet<ulong> m_friendIds;

		private HashSet<string> m_friendNames;

		public bool IsMultipleNetworking
		{
			get
			{
				if (m_serverDiscoveryAggregator != null)
				{
					return m_serverDiscoveryAggregator.GetAggregates().Count > 1;
				}
				return false;
			}
		}

		public bool SupportsPing { get; private set; }

		public bool SupportsGroups { get; private set; }

		private event Action<MyGuiControlButton> RefreshRequest;

		public MyGuiScreenJoinGame(bool enableDedicatedServers)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(1f, 0.9f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_serverDiscoveryAggregator = MyGameService.ServerDiscovery as MyServerDiscoveryAggregator;
			m_serverDiscovery = m_serverDiscoveryAggregator?.GetAggregates()[0] ?? MyGameService.ServerDiscovery;
			m_networkingService0Selected = true;
			base.EnabledBackgroundFade = true;
			m_enableDedicatedServers = enableDedicatedServers;
			m_dataHash = MyDataIntegrityChecker.GetHashBase64();
			MyObjectBuilder_ServerFilterOptions serverSearchSettings = MySandboxGame.Config.ServerSearchSettings;
			if (serverSearchSettings != null)
			{
				FilterOptions = new MySpaceServerFilterOptions(serverSearchSettings);
			}
			else
			{
				FilterOptions = new MySpaceServerFilterOptions();
			}
			RecreateControls(constructor: true);
			m_selectedPage = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageFavoritesPanel");
			joinGameTabs_OnPageChanged();
			MyRankedServers.LoadAsync(MyPerGameSettings.RankedServersUrl, OnRankedServersLoaded);
		}

		private void OnRankedServersLoaded(MyRankedServers rankedServers)
		{
			if (base.IsOpened)
			{
				m_rankedServers = rankedServers;
				m_searchChangedFunc();
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenJoinGame";
		}

		protected override void OnClosed()
		{
			if (m_currentServerDiscovery != null)
			{
				m_currentServerDiscovery.OnDedicatedServerListResponded -= OnFriendsServerListResponded;
				m_currentServerDiscovery.OnDedicatedServersCompleteResponse -= OnFriendsServersCompleteResponse;
			}
			CloseDedicatedServerListRequest();
			CloseFavoritesRequest();
			CloseHistoryRequest();
			CloseLANRequest();
			MySandboxGame.Config.ServerSearchSettings = FilterOptions.GetObjectBuilder();
			MySandboxGame.Config.Save();
			base.OnClosed();
		}

		private int PlayerCountComparison(MyGuiControlTable.Cell b, MyGuiControlTable.Cell a)
		{
			List<StringBuilder> list = a.Text.Split('/');
			List<StringBuilder> list2 = b.Text.Split('/');
			int result = 0;
			int result2 = 0;
			int result3 = 0;
			int result4 = 0;
			bool flag = true;
			if (list.Count >= 2 && list2.Count >= 2)
			{
				flag &= int.TryParse(list[0].ToString(), out result);
				flag &= int.TryParse(list2[0].ToString(), out result2);
				flag &= int.TryParse(list[1].ToString(), out result3);
				flag &= int.TryParse(list2[1].ToString(), out result4);
			}
			else
			{
				flag = false;
			}
			if (result == result2 || !flag)
			{
				if (result3 == result4 || !flag)
				{
					IMyMultiplayerGame myMultiplayerGame = a.Row.UserData as IMyMultiplayerGame;
					if (myMultiplayerGame != null)
					{
						ulong gameID = myMultiplayerGame.GameID;
						IMyMultiplayerGame myMultiplayerGame2 = b.Row.UserData as IMyMultiplayerGame;
						if (myMultiplayerGame2 != null)
						{
							ulong gameID2 = myMultiplayerGame2.GameID;
							return gameID.CompareTo(gameID2);
						}
						return 0;
					}
					return 0;
				}
				return result3.CompareTo(result4);
			}
			return result.CompareTo(result2);
		}

		private int TextComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
		{
			int num = a.Text.CompareToIgnoreCase(b.Text);
			if (num == 0)
			{
				IMyMultiplayerGame myMultiplayerGame = a.Row.UserData as IMyMultiplayerGame;
				if (myMultiplayerGame != null)
				{
					ulong gameID = myMultiplayerGame.GameID;
					IMyMultiplayerGame myMultiplayerGame2 = b.Row.UserData as IMyMultiplayerGame;
					if (myMultiplayerGame2 != null)
					{
						ulong gameID2 = myMultiplayerGame2.GameID;
						return gameID.CompareTo(gameID2);
					}
					return 0;
				}
				return 0;
			}
			return num;
		}

		private int PingComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
		{
			if (!int.TryParse(a.Text.ToString(), out var result))
			{
				result = -1;
			}
			if (!int.TryParse(b.Text.ToString(), out var result2))
			{
				result2 = -1;
			}
			if (result == result2)
			{
				IMyMultiplayerGame myMultiplayerGame = a.Row.UserData as IMyMultiplayerGame;
				if (myMultiplayerGame != null)
				{
					ulong gameID = myMultiplayerGame.GameID;
					IMyMultiplayerGame myMultiplayerGame2 = b.Row.UserData as IMyMultiplayerGame;
					if (myMultiplayerGame2 != null)
					{
						ulong gameID2 = myMultiplayerGame2.GameID;
						return gameID.CompareTo(gameID2);
					}
					return 0;
				}
				return 0;
			}
			return result.CompareTo(result2);
		}

		private int ModsComparison(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
		{
			int result = 0;
			int.TryParse(a.Text.ToString(), out result);
			int result2 = 0;
			int.TryParse(b.Text.ToString(), out result2);
			if (result == result2)
			{
				IMyMultiplayerGame myMultiplayerGame = a.Row.UserData as IMyMultiplayerGame;
				if (myMultiplayerGame != null)
				{
					ulong gameID = myMultiplayerGame.GameID;
					IMyMultiplayerGame myMultiplayerGame2 = b.Row.UserData as IMyMultiplayerGame;
					if (myMultiplayerGame2 != null)
					{
						ulong gameID2 = myMultiplayerGame2.GameID;
						return gameID.CompareTo(gameID2);
					}
					return 0;
				}
				return 0;
			}
			return result.CompareTo(result2);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			string path = MyGuiScreenBase.MakeScreenFilepath("JoinScreen");
			MyObjectBuilderSerializer.DeserializeXML(Path.Combine(MyFileSystem.ContentPath, path), out MyObjectBuilder_GuiScreen objectBuilder);
			Init(objectBuilder);
			m_joinGameTabs = Controls.GetControlByName("JoinGameTabs") as MyGuiControlTabControl;
			m_joinGameTabs.PositionY -= 0.018f;
			m_joinGameTabs.TabButtonScale = 0.86f;
			m_joinGameTabs.OnPageChanged += joinGameTabs_OnPageChanged;
			joinGameTabs_OnPageChanged();
			AddCaption(MyCommonTexts.ScreenMenuButtonJoinGame, null, new Vector2(0f, 0.003f));
			base.CloseButtonEnabled = true;
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(m_detailsButton.Position.X - minSizeGui.X / 2f, m_detailsButton.Position.Y));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			if ((float)MySandboxGame.ScreenSize.X / (float)MySandboxGame.ScreenSize.Y == 1.25f)
			{
				SetCloseButtonOffset_5_to_4();
			}
			else
			{
				SetDefaultCloseButtonOffset();
			}
			CreateGTXGamingBanner();
		}

		private void CreateGTXGamingBanner()
		{
			MyGuiControlImageButton.StyleDefinition style = new MyGuiControlImageButton.StyleDefinition
			{
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture(MyPerGameSettings.JoinScreenBannerTextureHighlight)
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture(MyPerGameSettings.JoinScreenBannerTexture)
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture(MyPerGameSettings.JoinScreenBannerTexture)
				}
			};
			Vector2 value = new Vector2(0.2375f, 0.13f) * 0.7f;
			m_bannerImage = new MyGuiControlImageButton("Button", new Vector2(base.Size.Value.X / 2f - 0.04f, (0f - base.Size.Value.Y) / 2f - 0.01f), value, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, onBannerClick)
			{
				BackgroundTexture = new MyGuiCompositeTexture(MyPerGameSettings.JoinScreenBannerTexture),
				CanHaveFocus = false,
				UserData = MyPerGameSettings.JoinScreenBannerURL
			};
			m_bannerImage.ApplyStyle(style);
			m_bannerImage.SetToolTip(MyTexts.GetString(MySpaceTexts.JoinScreen_GTXGamingBanner));
			Controls.Add(m_bannerImage);
		}

		private void onBannerClick(MyGuiControlImageButton button)
		{
			MyGuiSandbox.OpenUrl((string)button.UserData, UrlOpenMode.SteamOrExternalWithConfirm);
		}

		private void joinGameTabs_OnPageChanged()
		{
			if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
			{
				base.GamepadHelpTextId = MySpaceTexts.JoinGameScreen_Help_Screen;
			}
			else
			{
				base.GamepadHelpTextId = MySpaceTexts.JoinGameScreen_Help_ScreenXbox;
			}
			MyGuiControlTabPage myGuiControlTabPage = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageServersPanel");
			if (myGuiControlTabPage != null)
			{
				myGuiControlTabPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Servers));
				bool enabled = (myGuiControlTabPage.IsTabVisible = MyGameService.ServerDiscovery.DedicatedSupport && m_enableDedicatedServers);
				myGuiControlTabPage.Enabled = enabled;
			}
			MyGuiControlTabPage myGuiControlTabPage2 = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageLobbiesPanel");
			if (myGuiControlTabPage2 != null)
			{
				myGuiControlTabPage2.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Lobbies));
				bool enabled = (myGuiControlTabPage2.IsTabVisible = MyGameService.LobbyDiscovery.Supported);
				myGuiControlTabPage2.Enabled = enabled;
			}
			MyGuiControlTabPage myGuiControlTabPage3 = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageFavoritesPanel");
			if (myGuiControlTabPage3 != null)
			{
				myGuiControlTabPage3.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Favorites));
				bool enabled = (myGuiControlTabPage3.IsTabVisible = MyGameService.ServerDiscovery.FavoritesSupport && m_enableDedicatedServers);
				myGuiControlTabPage3.Enabled = enabled;
			}
			MyGuiControlTabPage myGuiControlTabPage4 = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageHistoryPanel");
			if (myGuiControlTabPage4 != null)
			{
				myGuiControlTabPage4.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_History));
				bool enabled = (myGuiControlTabPage4.IsTabVisible = MyGameService.ServerDiscovery.HistorySupport && m_enableDedicatedServers);
				myGuiControlTabPage4.Enabled = enabled;
			}
			MyGuiControlTabPage myGuiControlTabPage5 = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageLANPanel");
			if (myGuiControlTabPage5 != null)
			{
				myGuiControlTabPage5.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_LAN));
				bool enabled = (myGuiControlTabPage5.IsTabVisible = MyGameService.ServerDiscovery.LANSupport);
				myGuiControlTabPage5.Enabled = enabled;
			}
			MyGuiControlTabPage myGuiControlTabPage6 = (MyGuiControlTabPage)m_joinGameTabs.Controls.GetControlByName("PageFriendsPanel");
			if (myGuiControlTabPage6 != null)
			{
				myGuiControlTabPage6.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Friends));
				bool enabled = (myGuiControlTabPage6.IsTabVisible = MyGameService.ServerDiscovery.FriendSupport || MyGameService.LobbyDiscovery.FriendSupport);
				myGuiControlTabPage6.Enabled = enabled;
			}
			if (m_selectedPage == myGuiControlTabPage)
			{
				CloseServersPage();
			}
			else if (m_selectedPage == myGuiControlTabPage2)
			{
				CloseLobbyPage();
			}
			else if (m_selectedPage == myGuiControlTabPage3)
			{
				CloseFavoritesPage();
			}
			else if (m_selectedPage == myGuiControlTabPage5)
			{
				CloseLANPage();
			}
			else if (m_selectedPage == myGuiControlTabPage4)
			{
				CloseHistoryPage();
			}
			else if (m_selectedPage == myGuiControlTabPage6)
			{
				CloseFriendsPage();
			}
			m_selectedPage = m_joinGameTabs.GetTabSubControl(m_joinGameTabs.SelectedPage);
			while (!m_selectedPage.IsTabVisible && m_joinGameTabs.SelectedPage < m_joinGameTabs.PagesCount)
			{
				m_joinGameTabs.SelectedPage++;
				m_selectedPage = m_joinGameTabs.GetTabSubControl(m_joinGameTabs.SelectedPage);
			}
			InitPageControls(m_selectedPage);
			if (m_selectedPage == myGuiControlTabPage)
			{
				InitServersPage();
				EnableAdvancedSearch = true;
			}
			else if (m_selectedPage == myGuiControlTabPage2)
			{
				InitLobbyPage();
				EnableAdvancedSearch = false;
				if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
				{
					base.GamepadHelpTextId = MySpaceTexts.JoinGameScreen_Help_ScreenGamesTab;
				}
				else
				{
					base.GamepadHelpTextId = MySpaceTexts.JoinGameScreen_Help_ScreenGamesTabXbox;
				}
				UpdateGamepadHelp(base.FocusedControl);
			}
			else if (m_selectedPage == myGuiControlTabPage3)
			{
				InitFavoritesPage();
				EnableAdvancedSearch = true;
			}
			else if (m_selectedPage == myGuiControlTabPage4)
			{
				InitHistoryPage();
				EnableAdvancedSearch = true;
			}
			else if (m_selectedPage == myGuiControlTabPage5)
			{
				InitLANPage();
				EnableAdvancedSearch = true;
			}
			else if (m_selectedPage == myGuiControlTabPage6)
			{
				InitFriendsPage();
				EnableAdvancedSearch = false;
			}
			if (m_contextMenu != null)
			{
				m_contextMenu.Deactivate();
				m_contextMenu = null;
			}
			m_contextMenu = new MyGuiControlContextMenu();
			m_contextMenu.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			m_contextMenu.Deactivate();
			m_contextMenu.ItemClicked += OnContextMenu_ItemClicked;
			Controls.Add(m_contextMenu);
		}

		private void OnContextMenu_ItemClicked(MyGuiControlContextMenu sender, MyGuiControlContextMenu.EventArgs eventArgs)
		{
			ContextMenuFavoriteActionItem contextMenuFavoriteActionItem = (ContextMenuFavoriteActionItem)eventArgs.UserData;
			MyGameServerItem server = contextMenuFavoriteActionItem.Server;
			if (server != null)
			{
				switch (contextMenuFavoriteActionItem._Action)
				{
				case ContextMenuFavoriteAction.Add:
					MyGameService.AddFavoriteGame(server);
					break;
				case ContextMenuFavoriteAction.Remove:
					MyGameService.RemoveFavoriteGame(server);
					m_gamesTable.RemoveSelectedRow();
					m_favoritesPage.Text = new StringBuilder().Append((object)MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Favorites)).Append(" (").Append(m_gamesTable.RowsCount)
						.Append(")");
					break;
				default:
					throw new InvalidBranchException();
				}
			}
		}

		private void InitPageControls(MyGuiControlTabPage page)
		{
			page.Controls.Clear();
			if (m_joinButton != null)
			{
				Controls.Remove(m_joinButton);
			}
			if (m_detailsButton != null)
			{
				Controls.Remove(m_detailsButton);
			}
			if (m_directConnectButton != null)
			{
				Controls.Remove(m_directConnectButton);
			}
			if (m_refreshButton != null)
			{
				Controls.Remove(m_refreshButton);
			}
			Vector2 vector = new Vector2(-0.676f, -0.352f);
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			float y = 0.033f;
			m_searchBox = new MyGuiControlSearchBox(vector + new Vector2(minSizeGui.X, y));
			m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_searchBox.OnTextChanged += OnBlockSearchTextChanged;
			page.Controls.Add(m_searchBox);
			if (IsMultipleNetworking)
			{
				m_networkingButtons.Clear();
				m_networkingIcons.Clear();
				for (int i = 0; i < m_serverDiscoveryAggregator.GetAggregates().Count; i++)
				{
					IMyServerDiscovery myServerDiscovery = m_serverDiscoveryAggregator.GetAggregates()[i];
					MyGuiControlButton myGuiControlButton = new MyGuiControlButton
					{
						OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
						VisualStyle = MyGuiControlButtonStyleEnum.Rectangular,
						ShowTooltipWhenDisabled = true,
						Size = new Vector2(0.03f, 0.04f)
					};
					myGuiControlButton.SetToolTip(myServerDiscovery.ServiceName);
					page.Controls.Add(myGuiControlButton);
					MyGuiControlImage icon = new MyGuiControlImage(null, null, null, null, new string[1] { "Textures\\GUI\\Icons\\Browser\\" + myServerDiscovery.ServiceName + "CB.png" });
					icon.Size = myGuiControlButton.Size * 0.75f;
					myGuiControlButton.FocusChanged += delegate(MyGuiControlBase x, bool state)
					{
						icon.ColorMask = (state ? MyGuiConstants.HIGHLIGHT_TEXT_COLOR : Vector4.One);
					};
					page.Controls.Add(icon);
					m_networkingButtons.Add(myGuiControlButton);
					m_networkingIcons.Add(icon);
					myGuiControlButton.ButtonClicked += delegate
					{
						OnToggleNetworkingClicked();
					};
				}
				UpdateNetworkingButtons();
			}
			else
			{
				m_networkingButtonsVisible = false;
			}
			SetSearchBoxSize();
			m_advancedSearchButton = new MyGuiControlButton
			{
				Position = vector + new Vector2(minSizeGui.X, 0.033f) + new Vector2(0.909000039f, 0.006f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				VisualStyle = MyGuiControlButtonStyleEnum.ComboBoxButton,
				Text = MyTexts.GetString(MyCommonTexts.Advanced)
			};
			m_advancedSearchButton.ButtonClicked += AdvancedSearchButtonClicked;
			m_advancedSearchButton.SetToolTip(MySpaceTexts.ToolTipJoinGame_Advanced);
			m_advancedSearchButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			page.Controls.Add(m_advancedSearchButton);
			m_gamesTable = new MyGuiControlTable();
			m_gamesTable.Position = vector + new Vector2(minSizeGui.X, 0.067f);
			m_gamesTable.Size = new Vector2(1450f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 1f);
			m_gamesTable.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_gamesTable.VisibleRowsCount = 17;
			page.Controls.Add(m_gamesTable);
			Vector2 vector2 = new Vector2(vector.X, 0f) - new Vector2(-0.3137f, (0f - m_size.Value.Y) / 2f + 0.071f);
			Vector2 vector3 = new Vector2(0.1825f, 0f);
			int num = 0;
			m_detailsButton = MakeButton(vector2 + vector3 * num++, MyCommonTexts.JoinGame_ServerDetails, MySpaceTexts.ToolTipJoinGame_ServerDetails, ServerDetailsClick);
			m_detailsButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			Controls.Add(m_detailsButton);
			if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
			{
				m_directConnectButton = MakeButton(vector2 + vector3 * num++, MyCommonTexts.JoinGame_DirectConnect, MySpaceTexts.ToolTipJoinGame_DirectConnect, DirectConnectClick);
				m_directConnectButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				Controls.Add(m_directConnectButton);
			}
			m_refreshButton = MakeButton(vector2 + vector3 * num++, MyCommonTexts.ScreenLoadSubscribedWorldRefresh, MySpaceTexts.ToolTipJoinGame_Refresh, null);
			m_refreshButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			Controls.Add(m_refreshButton);
			m_joinButton = MakeButton(vector2 + vector3 * num++, MyCommonTexts.ScreenMenuButtonJoinWorld, MySpaceTexts.ToolTipJoinGame_JoinWorld, null);
			m_joinButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			Controls.Add(m_joinButton);
			m_joinButton.Enabled = false;
			m_detailsButton.Enabled = false;
			m_loadingWheel = new MyGuiControlRotatingWheel(m_joinButton.Position + new Vector2(0.2f, -0.026f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.22f);
			page.Controls.Add(m_loadingWheel);
			m_loadingWheel.Visible = false;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.895f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.895f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.895f / 2f, m_size.Value.Y / 2f - 0.152f), m_size.Value.X * 0.895f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.895f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.895f);
			Controls.Add(myGuiControlSeparatorList2);
			base.FocusedControl = m_searchBox.TextBox;
		}

		private void OnToggleNetworkingClicked()
		{
			m_networkingService0Selected = !m_networkingService0Selected;
			m_serverDiscovery = m_serverDiscoveryAggregator.GetAggregates()[(!m_networkingService0Selected) ? 1 : 0];
			UpdateNetworkingButtons();
			m_stopServerRequest();
			this.RefreshRequest.InvokeIfNotNull(m_refreshButton);
		}

		private void ShowNetworkingButtons()
		{
			if (!IsMultipleNetworking)
			{
				return;
			}
			m_networkingButtonsVisible = true;
			foreach (MyGuiControlImage networkingIcon in m_networkingIcons)
			{
				networkingIcon.Visible = true;
			}
			foreach (MyGuiControlButton networkingButton in m_networkingButtons)
			{
				networkingButton.Visible = true;
			}
			SetSearchBoxSize();
		}

		private void HideNetworkingButtons()
		{
			if (!IsMultipleNetworking)
			{
				return;
			}
			m_networkingButtonsVisible = false;
			foreach (MyGuiControlImage networkingIcon in m_networkingIcons)
			{
				networkingIcon.Visible = false;
			}
			foreach (MyGuiControlButton networkingButton in m_networkingButtons)
			{
				networkingButton.Visible = false;
			}
			SetSearchBoxSize();
		}

		private void UpdateNetworkingButtons()
		{
			for (int i = 0; i < m_networkingButtons.Count; i++)
			{
				m_networkingButtons[i].VisualStyle = ((((!m_networkingService0Selected) ? 1 : 0) == i) ? MyGuiControlButtonStyleEnum.RectangularChecked : MyGuiControlButtonStyleEnum.Rectangular);
			}
		}

		private void SetSearchBoxSize()
		{
			if (m_searchBox != null)
			{
				m_searchBox.Size = ((!MyInput.Static.IsJoystickLastUsed) ? new Vector2(0.754f, 0.02f) : new Vector2(1450f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0.02f)) - (m_networkingButtonsVisible ? new Vector2(0.06f, 0f) : Vector2.Zero);
				Vector2 position = m_searchBox.Position + new Vector2(m_searchBox.Size.X, 0f) + new Vector2(0.0022f, 0.0065f);
				for (int i = 0; i < m_networkingButtons.Count; i++)
				{
					m_networkingButtons[i].Position = position;
					m_networkingIcons[i].Position = m_networkingButtons[i].Position + new Vector2(m_networkingButtons[i].Size.X / 2f, 0f) + new Vector2(-0.0016f, -0.0016f);
					position += new Vector2(m_networkingButtons[i].Size.X, 0f);
				}
			}
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, MyStringId toolTip, Action<MyGuiControlButton> onClick)
		{
			return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(text), toolTip: MyTexts.GetString(toolTip), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: onClick);
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		public override bool Update(bool hasFocus)
		{
			if (refresh_favorites && hasFocus)
			{
				refresh_favorites = false;
				m_joinButton.Enabled = false;
				m_detailsButton.Enabled = false;
				RebuildFavoritesList();
			}
			if (m_searchChanged && DateTime.Now.Subtract(m_searchLastChanged).Milliseconds > 500)
			{
				m_searchChanged = false;
				m_searchChangedFunc.InvokeIfNotNull();
			}
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_remainingTimeUpdateFrame++;
				if (m_remainingTimeUpdateFrame % 50 == 0)
				{
					for (int i = 0; i < m_gamesTable.RowsCount; i++)
					{
						m_gamesTable.GetRow(i).Update();
					}
					m_remainingTimeUpdateFrame = 0;
				}
			}
			if (hasFocus)
			{
				if (base.FocusedControl == m_joinGameTabs)
				{
					base.FocusedControl = m_gamesTable;
				}
				m_detailsButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				if (m_directConnectButton != null)
				{
					m_directConnectButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				}
				m_refreshButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_joinButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_advancedSearchButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				SetSearchBoxSize();
			}
			return base.Update(hasFocus);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F5))
			{
				this.RefreshRequest.InvokeIfNotNull(m_refreshButton);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT) && m_gamesTable == base.FocusedControl)
			{
				OnJoinServer(m_joinButton);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				ServerDetailsClick(m_detailsButton);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
			{
				m_refreshButton.RaiseButtonClicked();
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU))
			{
				AdvancedSearchButtonClicked(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.VIEW) && !MyPlatformGameSettings.LIMITED_MAIN_MENU)
			{
				DirectConnectClick(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_BUTTON))
			{
				onBannerClick(m_bannerImage);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_BUTTON) && !MyPlatformGameSettings.LIMITED_MAIN_MENU && m_networkingButtonsVisible)
			{
				OnToggleNetworkingClicked();
			}
		}

		/// <summary>
		///     Filters on simple values that are passed along as prefixes when we first get the server from Steam.
		///     Filters here are common to both games.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="searchText"></param>
		/// <returns></returns>
		private bool FilterSimple(MyCachedServerItem item, string searchText = null)
		{
			MyGameServerItem server = item.Server;
			if (server.AppID != MyGameService.AppId)
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Name + " by appId: " + server.AppID);
				return false;
			}
			string map = server.Map;
			int serverVersion = server.ServerVersion;
			if (string.IsNullOrEmpty(map))
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Name + " by sessionName: " + map);
				return false;
			}
			if (!string.IsNullOrWhiteSpace(searchText) && !System.StringExtensions.Contains(server.Name, searchText, StringComparison.CurrentCultureIgnoreCase) && !System.StringExtensions.Contains(server.Map, searchText, StringComparison.CurrentCultureIgnoreCase))
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Name + " by searchText: " + searchText);
				return false;
			}
			if (FilterOptions.AllowedGroups && !item.AllowedInGroup)
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Name + " by AllowedGroups");
				return false;
			}
			if (FilterOptions.SameVersion && serverVersion != (int)MyFinalBuildConstants.APP_VERSION)
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Name + " by appVersion: " + serverVersion);
				return false;
			}
			if (FilterOptions.HasPassword.HasValue && FilterOptions.HasPassword.Value != server.Password)
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Name + " by HasPassword");
				return false;
			}
			if (MyFakes.ENABLE_MP_DATA_HASHES && FilterOptions.SameData)
			{
				string gameTagByPrefix = server.GetGameTagByPrefix("datahash");
				if (gameTagByPrefix != "" && gameTagByPrefix != m_dataHash)
				{
					MyLog.Default.WriteLine("Server filtered: " + server.Name + " by SameData");
					return false;
				}
			}
			string gameTagByPrefix2 = server.GetGameTagByPrefix("gamemode");
			if (gameTagByPrefix2 == "C" && !FilterOptions.CreativeMode)
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Name + " by CreativeMode: " + gameTagByPrefix2);
				return false;
			}
			if (gameTagByPrefix2.StartsWith("S") && !FilterOptions.SurvivalMode)
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Name + " by SurvivalMode: " + gameTagByPrefix2);
				return false;
			}
			ulong gameTagByPrefixUlong = server.GetGameTagByPrefixUlong("mods");
			if (FilterOptions.CheckMod && !FilterOptions.ModCount.ValueBetween(gameTagByPrefixUlong))
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Name + " by MOD_COUNT_TAG: " + gameTagByPrefixUlong);
				return false;
			}
			if (FilterOptions.CheckPlayer && !FilterOptions.PlayerCount.ValueBetween(server.Players))
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Name + " by PlayerCount: " + FilterOptions.PlayerCount);
				return false;
			}
			if (FilterOptions.Ping > -1 && server.Ping > FilterOptions.Ping)
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Name + " by Ping: " + FilterOptions.Ping);
				return false;
			}
			if (float.TryParse(server.GetGameTagByPrefix("view"), out var result) && FilterOptions.CheckDistance && !FilterOptions.ViewDistance.ValueBetween(result))
			{
				MyLog.Default.WriteLine("Server filtered: " + server.Name + " by viewDistance: " + result);
				return false;
			}
			return true;
		}

		/// <summary>
		///     Filters on values passed along in the server's Rules Dictionary.
		///     Game-specific filtering happens here.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="searchText"></param>
		/// <returns></returns>
		private bool FilterAdvanced(MyCachedServerItem item, string searchText = null)
		{
			if (!FilterSimple(item, searchText))
			{
				return false;
			}
			if (item.Rules == null || !Enumerable.Any<KeyValuePair<string, string>>((IEnumerable<KeyValuePair<string, string>>)item.Rules))
			{
				MyLog.Default.WriteLine("Server filtered: " + item.Server.Name + " by Rules: " + item.Rules);
				return false;
			}
			if (!FilterOptions.FilterServer(item))
			{
				MyLog.Default.WriteLine("Server filtered: " + item.Server.Name + " by FilterServer");
				return false;
			}
			if (FilterOptions.Mods != null && Enumerable.Any<WorkshopId>((IEnumerable<WorkshopId>)FilterOptions.Mods) && FilterOptions.AdvancedFilter)
			{
				if (FilterOptions.ModsExclusive)
				{
<<<<<<< HEAD
					if (!FilterOptions.Mods.All((WorkshopId modId) => item.Mods.Contains(modId)))
=======
					if (!Enumerable.All<WorkshopId>((IEnumerable<WorkshopId>)FilterOptions.Mods, (Func<WorkshopId, bool>)((WorkshopId modId) => item.Mods.Contains(modId))))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyLog.Default.WriteLine("Server filtered: " + item.Server.Name + " by Mods");
						return false;
					}
				}
<<<<<<< HEAD
				else if (item.Mods == null || !item.Mods.Any((WorkshopId modId) => FilterOptions.Mods.Contains(modId)))
=======
				else if (item.Mods == null || !Enumerable.Any<WorkshopId>((IEnumerable<WorkshopId>)item.Mods, (Func<WorkshopId, bool>)((WorkshopId modId) => FilterOptions.Mods.Contains(modId))))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyLog.Default.WriteLine("Server filtered: " + item.Server.Name + " by Mods");
					return false;
				}
			}
			m_loadingWheel.Visible = false;
			return true;
		}

		private void AdvancedSearchButtonClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_detailsButton != null && m_joinButton != null)
			{
				m_detailsButton.Enabled = false;
				m_joinButton.Enabled = false;
			}
			MyGuiScreenServerSearchSpace myGuiScreenServerSearchSpace = new MyGuiScreenServerSearchSpace(this);
			myGuiScreenServerSearchSpace.Closed += delegate
			{
				m_searchChangedFunc.InvokeIfNotNull();
			};
			m_loadingWheel.Visible = false;
			MyGuiSandbox.AddScreen(myGuiScreenServerSearchSpace);
		}

		private void ServerDetailsClick(MyGuiControlButton detailButton)
		{
			if (m_gamesTable.SelectedRow == null)
			{
				return;
			}
			MyGameServerItem ser = m_gamesTable.SelectedRow.UserData as MyGameServerItem;
			if (ser != null)
			{
<<<<<<< HEAD
				MyCachedServerItem myCachedServerItem = (detailButton.UserData as HashSet<MyCachedServerItem>).FirstOrDefault((MyCachedServerItem x) => x.Server.ConnectionString.Equals(ser.ConnectionString));
=======
				MyCachedServerItem myCachedServerItem = Enumerable.FirstOrDefault<MyCachedServerItem>((IEnumerable<MyCachedServerItem>)(detailButton.UserData as HashSet<MyCachedServerItem>), (Func<MyCachedServerItem, bool>)((MyCachedServerItem x) => x.Server.ConnectionString.Equals(ser.ConnectionString)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (myCachedServerItem != null)
				{
					m_loadingWheel.Visible = false;
					MyGuiSandbox.AddScreen(new MyGuiScreenServerDetailsSpace(myCachedServerItem));
				}
			}
		}

		private void DirectConnectClick(MyGuiControlButton button)
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenServerConnect());
		}

		private void OnBlockSearchTextChanged(string text)
		{
			if (m_detailsButton != null && m_joinButton != null)
			{
				m_detailsButton.Enabled = false;
				m_joinButton.Enabled = false;
			}
			m_searchChanged = true;
			m_searchLastChanged = DateTime.Now;
		}

		private void InitServersPage()
		{
			InitServersTable(MyGameService.ServerDiscovery.PingSupport, MyGameService.ServerDiscovery.GroupSupport);
			m_joinButton.ButtonClicked += OnJoinServer;
			m_refreshButton.ButtonClicked += OnRefreshServersClick;
			this.RefreshRequest = OnRefreshServersClick;
			m_stopServerRequest = CloseDedicatedServerListRequest;
			ShowNetworkingButtons();
			m_detailsButton.UserData = m_dedicatedServers;
			m_dedicatedResponding = true;
			m_searchChangedFunc = (Action)Delegate.Combine(m_searchChangedFunc, (Action)delegate
			{
				RefreshServerGameList(m_currentServerDiscovery.SupportsDirectServerSearch);
			});
			m_serversPage = m_selectedPage;
			m_serversPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Servers));
			RefreshServerGameList(resetSteamQuery: true);
		}

		private void CloseServersPage()
		{
			CloseDedicatedServerListRequest();
			m_dedicatedResponding = false;
			m_searchChangedFunc = (Action)Delegate.Remove(m_searchChangedFunc, (Action)delegate
			{
				RefreshServerGameList(resetSteamQuery: false);
			});
		}

		private void InitServersTable(bool pingSupport, bool groupSupport)
		{
			int num = (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME ? 9 : 8);
			if (pingSupport)
			{
				num++;
			}
			if (MyPlatformGameSettings.IsModdingAllowed)
			{
				num++;
			}
			m_gamesTable.ColumnsCount = num;
			m_gamesTable.ItemSelected += OnTableItemSelected;
			m_gamesTable.ItemSelected += OnServerTableItemSelected;
			m_gamesTable.ItemDoubleClicked += OnTableItemDoubleClick;
			List<float> list = new List<float>();
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				list.AddRange(new float[9] { 0.024f, 0.024f, 0.024f, 0.24f, 0.15f, 0.024f, 0.16f, 0.15f, 0.09f });
				if (pingSupport)
				{
					list.Add(0.07f);
				}
				if (MyPlatformGameSettings.IsModdingAllowed)
				{
					list.Add(0.07f);
				}
				m_gamesTable.SetCustomColumnWidths(list.ToArray());
			}
			else
			{
				list.AddRange(new float[8] { 0.024f, 0.024f, 0.024f, 0.26f, 0.15f, 0.024f, 0.26f, 0.09f });
				if (pingSupport)
				{
					list.Add(0.07f);
				}
				if (MyPlatformGameSettings.IsModdingAllowed)
				{
					list.Add(0.07f);
				}
				m_gamesTable.SetCustomColumnWidths(list.ToArray());
			}
			m_gamesTable.SetHeaderColumnMargin(num - 1, new Thickness(0.01f, 0.01f, 0.005f, 0.01f));
			int num2 = 3;
			m_gamesTable.SetColumnComparison(num2++, TextComparison);
			m_gamesTable.SetColumnComparison(num2++, TextComparison);
			m_gamesTable.SetColumnComparison(num2++, TextComparison);
			m_gamesTable.SetColumnComparison(num2++, TextComparison);
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_gamesTable.SetColumnComparison(num2, TextComparison);
				m_gamesTable.SetColumnAlign(num2);
				m_gamesTable.SetHeaderColumnAlign(num2);
				num2++;
			}
			m_gamesTable.SetColumnComparison(num2, PlayerCountComparison);
			m_gamesTable.SetColumnAlign(num2);
			m_gamesTable.SetHeaderColumnAlign(num2);
			num2++;
			int num3 = num2;
			if (pingSupport)
			{
				m_gamesTable.SetColumnComparison(num2, PingComparison);
				m_gamesTable.SetColumnAlign(num2);
				m_gamesTable.SetHeaderColumnAlign(num2);
				num2++;
			}
			if (MyPlatformGameSettings.IsModdingAllowed)
			{
				m_gamesTable.SetColumnComparison(num2, ModsComparison);
				m_gamesTable.SetColumnAlign(num2);
				m_gamesTable.SetHeaderColumnAlign(num2);
				num2++;
			}
			SupportsPing = pingSupport;
			SupportsGroups = groupSupport;
			m_gamesTable.SortByColumn(pingSupport ? num3 : 0);
		}

		private void OnJoinServer(MyGuiControlButton obj)
		{
			JoinSelectedServer();
		}

		private void OnServerTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			if (sender.SelectedRow == null)
			{
				return;
			}
			MyGameServerItem myGameServerItem = sender.SelectedRow.UserData as MyGameServerItem;
			if (myGameServerItem == null || string.IsNullOrEmpty(myGameServerItem.ConnectionString))
			{
				return;
			}
			MyGuiControlTable.Cell cell = sender.SelectedRow.GetCell(5);
			if (cell == null || cell.ToolTip == null)
			{
				return;
			}
			if (eventArgs.MouseButton == MyMouseButtonsEnum.Right)
			{
				m_contextMenu.CreateNewContextMenu();
				ContextMenuFavoriteAction contextMenuFavoriteAction = ((m_selectedPage == m_favoritesPage) ? ContextMenuFavoriteAction.Remove : ContextMenuFavoriteAction.Add);
				MyStringId id = MyCommonTexts.JoinGame_Favorites_Remove;
				if (contextMenuFavoriteAction == ContextMenuFavoriteAction.Add)
				{
					id = MyCommonTexts.JoinGame_Favorites_Add;
				}
				m_contextMenu.AddItem(MyTexts.Get(id), "", "", new ContextMenuFavoriteActionItem
				{
					Server = myGameServerItem,
					_Action = contextMenuFavoriteAction
				});
				m_contextMenu.ItemList_UseSimpleItemListMouseOverCheck = true;
				m_contextMenu.Activate();
			}
			else
			{
				m_contextMenu.Deactivate();
			}
		}

		private void OnTableItemDoubleClick(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			JoinSelectedServer();
		}

		private void JoinSelectedServer(bool checkPing = true)
		{
			MyGuiControlTable.Row selectedRow = m_gamesTable.SelectedRow;
			if (selectedRow == null)
			{
				return;
			}
			MyGameServerItem myGameServerItem = selectedRow.UserData as MyGameServerItem;
			if (myGameServerItem != null)
			{
				if (!MySandboxGame.Config.ExperimentalMode && myGameServerItem.Experimental)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: MyTexts.Get(MyCommonTexts.MultiplayerErrorExperimental)));
				}
				else if (checkPing && myGameServerItem.Ping > 150)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), messageText: MyTexts.Get(MyCommonTexts.MultiplayerWarningPing), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
					{
						if (result == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							JoinSelectedServer(checkPing: false);
						}
					}));
				}
				else
				{
					MyJoinGameHelper.JoinGame(myGameServerItem);
					MyLocalCache.SaveLastSessionInfo(null, isOnline: true, isLobby: false, myGameServerItem.Name, myGameServerItem.ConnectionString);
				}
			}
			else
			{
				IMyLobby myLobby = selectedRow.UserData as IMyLobby;
				if (myLobby != null)
				{
					MyJoinGameHelper.JoinGame(myLobby);
					MyLocalCache.SaveLastSessionInfo(null, isOnline: true, isLobby: true, selectedRow.GetCell(0).Text.ToString(), myLobby.LobbyId.ToString(), 0);
				}
			}
		}

		private void OnRefreshServersClick(MyGuiControlButton obj)
		{
			if (m_detailsButton != null && m_joinButton != null)
			{
				m_detailsButton.Enabled = false;
				m_joinButton.Enabled = false;
			}
			switch (m_nextState)
			{
			case RefreshStateEnum.Pause:
				m_refreshPaused = true;
				m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldResume);
				m_nextState = RefreshStateEnum.Resume;
				m_loadingWheel.Visible = false;
				break;
			case RefreshStateEnum.Resume:
				m_refreshPaused = false;
				if (m_loadingWheel.Visible)
				{
					m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldPause);
					m_nextState = RefreshStateEnum.Pause;
					m_loadingWheel.Visible = true;
				}
				else
				{
					m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldRefresh);
					m_nextState = RefreshStateEnum.Refresh;
					m_loadingWheel.Visible = false;
				}
				RefreshServerGameList(resetSteamQuery: false);
				break;
			case RefreshStateEnum.Refresh:
				if (!MyInput.Static.IsJoystickLastUsed)
				{
					m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldPause);
					m_nextState = RefreshStateEnum.Pause;
				}
				m_dedicatedServers.Clear();
				RefreshServerGameList(resetSteamQuery: true);
				m_loadingWheel.Visible = true;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void OnDedicatedServerListResponded(object sender, int server)
		{
			OnDedicateServerResult(m_currentServerDiscovery.GetDedicatedServerDetails(server));
		}

		private void OnDedicateServerResult(MyGameServerItem game, bool fromRankedQuery = false)
		{
			MyCachedServerItem serverItem = new MyCachedServerItem(game);
<<<<<<< HEAD
			if (!m_currentServerDiscovery.SupportsDirectServerSearch || fromRankedQuery || !IsRanked(serverItem) || !m_dedicatedServers.Any((MyCachedServerItem x) => x.Server.ConnectionString == game.ConnectionString))
=======
			if (!m_currentServerDiscovery.SupportsDirectServerSearch || fromRankedQuery || !IsRanked(serverItem) || !Enumerable.Any<MyCachedServerItem>((IEnumerable<MyCachedServerItem>)m_dedicatedServers, (Func<MyCachedServerItem, bool>)((MyCachedServerItem x) => x.Server.ConnectionString == game.ConnectionString)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_currentServerDiscovery.GetServerRules(serverItem.Server, delegate(Dictionary<string, string> rules)
				{
					DedicatedRulesResponse(rules, serverItem);
				}, delegate
				{
					DedicatedRulesResponse(null, serverItem);
				});
			}
		}

		private void DedicatedRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
		{
<<<<<<< HEAD
			if (server.Server.ConnectionString == null || m_dedicatedServers.Any(Predicate))
=======
			if (server.Server.ConnectionString == null || Enumerable.Any<MyCachedServerItem>((IEnumerable<MyCachedServerItem>)m_dedicatedServers, (Func<MyCachedServerItem, bool>)Predicate))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyLog.Default.WriteLine("Connection string duplicate: " + server.Server.ConnectionString);
			}
			else
			{
				if (m_serverDiscoveryAggregator != null && m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) != m_currentServerDiscovery)
				{
					return;
				}
				server.Rules = rules;
				if (rules != null)
				{
					server.DeserializeSettings();
				}
				m_dedicatedServers.Add(server);
				if (!m_dedicatedResponding)
				{
					MyLog.Default.WriteLine("Server page closed.");
					return;
				}
				server.Server.IsRanked = IsRanked(server);
				AddServerItem(server);
				if (!m_refreshPaused)
				{
					m_serversPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Servers)).Append(" (")
						.Append(m_gamesTable.RowsCount)
						.Append(")");
				}
			}
			bool Predicate(MyCachedServerItem x)
			{
				if (x == null)
				{
					MyLog.Default.WriteLine("Existing item in dedicated server list is null.");
				}
				else if (x.Server == null)
				{
					MyLog.Default.WriteLine("Existing item in dedicated server list has null server.");
				}
				else if (server?.Server == null)
				{
					MyLog.Default.WriteLine("Incoming item has become null after this call started.");
				}
				return server.Server.ConnectionString.Equals(x.Server.ConnectionString);
			}
		}

		private bool IsRanked(MyCachedServerItem server)
		{
			MyGameServerItem server2 = server.Server;
			return IsRanked(server2);
		}

		private bool IsRanked(MyGameServerItem gameServer)
		{
			if (m_rankedServers == null)
			{
				return false;
			}
			string connectionString = gameServer.ConnectionString;
<<<<<<< HEAD
			return m_rankedServers.GetByPrefix(m_currentServerDiscovery.ConnectionStringPrefix).Any((MyRankServer server) => server.ConnectionString == connectionString);
=======
			return Enumerable.Any<MyRankServer>(m_rankedServers.GetByPrefix(m_currentServerDiscovery.ConnectionStringPrefix), (Func<MyRankServer, bool>)((MyRankServer server) => server.ConnectionString == connectionString));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void OnDedicatedServersCompleteResponse(object sender, MyMatchMakingServerResponse response)
		{
			CloseDedicatedServerListRequest();
		}

		private void CloseDedicatedServerListRequest()
		{
			if (m_loadingWheel != null)
			{
				m_loadingWheel.Visible = false;
			}
			if (m_currentServerDiscovery != null)
			{
				m_currentServerDiscovery.OnDedicatedServerListResponded -= OnDedicatedServerListResponded;
				m_currentServerDiscovery.OnDedicatedServersCompleteResponse -= OnDedicatedServersCompleteResponse;
				m_currentServerDiscovery.CancelInternetServersRequest();
			}
			if (m_nextState == RefreshStateEnum.Pause)
			{
				if (m_refreshButton != null)
				{
					m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldRefresh);
				}
				m_nextState = RefreshStateEnum.Refresh;
				m_refreshPaused = false;
			}
		}

		private void AddServerHeaders(bool pingSupport)
		{
			int num = 0;
			int columnsCount = m_gamesTable.ColumnsCount;
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, new StringBuilder());
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, new StringBuilder());
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, new StringBuilder());
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_World));
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_GameMode));
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, new StringBuilder());
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Server));
			}
			if (num < columnsCount && MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_RemainingTime));
			}
			if (num < columnsCount)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Players));
			}
			if (num < columnsCount && pingSupport)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Ping));
			}
			if (num < columnsCount && MyPlatformGameSettings.IsModdingAllowed)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Mods));
			}
		}

		private void RefreshServerGameList(bool resetSteamQuery)
		{
			if (m_lastVersionCheck != FilterOptions.SameVersion || FilterOptions.AdvancedFilter)
			{
				resetSteamQuery = true;
			}
			m_lastVersionCheck = FilterOptions.SameVersion;
			m_detailsButton.Enabled = false;
			m_joinButton.Enabled = false;
			m_gamesTable.Clear();
			AddServerHeaders(MyGameService.ServerDiscovery.PingSupport);
			m_textCache.Clear();
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			m_serversPage.TextEnum = MyCommonTexts.JoinGame_TabTitle_Servers;
			if (resetSteamQuery)
			{
				m_dedicatedServers.Clear();
				CloseDedicatedServerListRequest();
				if (!MyInput.Static.IsJoystickLastUsed)
				{
					m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.ScreenLoadSubscribedWorldPause);
					m_nextState = RefreshStateEnum.Pause;
				}
				m_refreshPaused = false;
				if (m_enableDedicatedServers)
				{
					m_currentServerDiscovery = m_serverDiscovery;
					IMyServerDiscovery currentServerDiscovery = m_currentServerDiscovery;
<<<<<<< HEAD
					MySessionSearchFilter networkFilter = FilterOptions.GetNetworkFilter(currentServerDiscovery.SupportedSearchParameters, m_searchBox.SearchText.ToLower());
=======
					MySessionSearchFilter networkFilter = FilterOptions.GetNetworkFilter(currentServerDiscovery.SupportedSearchParameters, m_searchBox.SearchText);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MySandboxGame.Log.WriteLine("Requesting dedicated servers, filterOps: " + networkFilter);
					currentServerDiscovery.OnDedicatedServerListResponded += OnDedicatedServerListResponded;
					currentServerDiscovery.OnDedicatedServersCompleteResponse += OnDedicatedServersCompleteResponse;
					if (currentServerDiscovery.SupportsDirectServerSearch && m_rankedServers != null)
					{
<<<<<<< HEAD
						string[] array = (from x in m_rankedServers.GetByPrefix(currentServerDiscovery.ConnectionStringPrefix)
							select x.ConnectionString).ToArray();
=======
						string[] array = Enumerable.ToArray<string>(Enumerable.Select<MyRankServer, string>(m_rankedServers.GetByPrefix(currentServerDiscovery.ConnectionStringPrefix), (Func<MyRankServer, string>)((MyRankServer x) => x.ConnectionString)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (array.Length != 0)
						{
							currentServerDiscovery.RequestServerItems(array, networkFilter, RankedServerQueryComplete);
						}
					}
					currentServerDiscovery.RequestInternetServerList(networkFilter);
					m_loadingWheel.Visible = true;
				}
			}
			m_gamesTable.SelectedRowIndex = null;
			RebuildServerList();
		}

		private void RankedServerQueryComplete(IEnumerable<MyGameServerItem> servers)
		{
			foreach (MyGameServerItem server in servers)
			{
				OnDedicateServerResult(server, fromRankedQuery: true);
			}
		}

		private void RebuildServerList()
		{
<<<<<<< HEAD
			string text = m_searchBox.SearchText.ToLower();
=======
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			string text = m_searchBox.SearchText;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (string.IsNullOrWhiteSpace(text))
			{
				text = null;
			}
			m_detailsButton.Enabled = false;
			m_joinButton.Enabled = false;
			m_gamesTable.Clear();
			Enumerator<MyCachedServerItem> enumerator = m_dedicatedServers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyCachedServerItem current = enumerator.get_Current();
					if (FilterOptions.AdvancedFilter)
					{
						if (!FilterAdvanced(current, text))
						{
							continue;
						}
					}
<<<<<<< HEAD
				}
				else if (!FilterSimple(dedicatedServer, text))
				{
					continue;
				}
				MyGameServerItem server = dedicatedServer.Server;
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				string gameTagByPrefix = server.GetGameTagByPrefix("gamemode");
				if (gameTagByPrefix == "C")
				{
					stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
					stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
				}
				else if (!string.IsNullOrWhiteSpace(gameTagByPrefix))
				{
					string text2 = gameTagByPrefix.Substring(1);
					string[] array = text2.Split(new char[1] { '-' });
					if (array.Length == 4)
=======
					else if (!FilterSimple(current, text))
					{
						continue;
					}
					MyGameServerItem server = current.Server;
					StringBuilder stringBuilder = new StringBuilder();
					StringBuilder stringBuilder2 = new StringBuilder();
					string gameTagByPrefix = server.GetGameTagByPrefix("gamemode");
					if (gameTagByPrefix == "C")
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
						stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
					}
					else if (!string.IsNullOrWhiteSpace(gameTagByPrefix))
					{
						string text2 = gameTagByPrefix.Substring(1);
						string[] array = text2.Split(new char[1] { '-' });
						if (array.Length == 4)
						{
							stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival)).Append(" ").Append(text2);
							stringBuilder2.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_MultipliersFormat), array[0], array[1], array[2], array[3]);
						}
						else
						{
							stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival));
							stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival));
						}
					}
					AddServerItem(server, server.Map, stringBuilder, stringBuilder2, sort: false, current.Settings);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_gamesTable.Sort(switchSort: false);
			m_serversPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Servers)).Append(" (")
				.Append(m_gamesTable.RowsCount)
				.Append(")");
		}

		private bool AddServerItem(MyCachedServerItem item)
		{
			MyGameServerItem server = item.Server;
			server.Name = MySandboxGame.Static.FilterOffensive(server.Name);
			server.Map = MySandboxGame.Static.FilterOffensive(server.Map);
			server.Experimental = item.ExperimentalMode;
			if (FilterOptions.AdvancedFilter && item.Rules != null)
			{
				if (!FilterAdvanced(item, m_searchBox.SearchText.ToLower()))
				{
					return false;
				}
			}
			else if (!FilterSimple(item, m_searchBox.SearchText.ToLower()))
			{
				return false;
			}
			string map = server.Map;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			string gameTagByPrefix = server.GetGameTagByPrefix("gamemode");
			if (gameTagByPrefix == "C")
			{
				stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
				stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeCreative));
			}
			else if (!string.IsNullOrWhiteSpace(gameTagByPrefix))
			{
				string text = gameTagByPrefix.Substring(1);
				string[] array = text.Split(new char[1] { '-' });
				if (array.Length == 4)
				{
					stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival)).Append(" ").Append(text);
					stringBuilder2.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_MultipliersFormat), array[0], array[1], array[2], array[3]);
				}
				else
				{
					stringBuilder.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival));
					stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.WorldSettings_GameModeSurvival));
				}
			}
			if (!m_refreshPaused)
			{
				AddServerItem(server, map, stringBuilder, stringBuilder2, sort: true, item.Settings);
			}
			return true;
		}

		private void AddServerItem(MyGameServerItem server, string sessionName, StringBuilder gamemodeSB, StringBuilder gamemodeToolTipSB, bool sort = true, MyObjectBuilder_SessionSettings settings = null)
		{
			ulong gameTagByPrefixUlong = server.GetGameTagByPrefixUlong("mods");
			string text = server.MaxPlayers.ToString();
			StringBuilder stringBuilder = new StringBuilder(server.Players + "/" + text);
			string gameTagByPrefix = server.GetGameTagByPrefix("view");
			if (!string.IsNullOrEmpty(gameTagByPrefix))
			{
				gamemodeToolTipSB.AppendLine();
				gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_ViewDistance), gameTagByPrefix);
			}
			if (settings != null)
			{
				gamemodeToolTipSB.AppendLine();
				gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_PCU_Max), settings.TotalPCU);
				gamemodeToolTipSB.AppendLine();
				gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_PCU_Settings), settings.BlockLimitsEnabled);
				gamemodeToolTipSB.AppendLine();
				gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_PCU_Initial), MyObjectBuilder_SessionSettings.GetInitialPCU(settings));
				gamemodeToolTipSB.AppendLine();
				gamemodeToolTipSB.AppendFormat(MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_Airtightness), settings.EnableOxygenPressurization ? MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_ON) : MyTexts.GetString(MyCommonTexts.JoinGame_GameTypeToolTip_OFF));
			}
			Color? color = Color.White;
			if (server.Experimental && !MySandboxGame.Config.ExperimentalMode)
			{
				color = Color.DarkGray;
			}
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(server);
			string toolTip = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Rank).ToString();
			MyGuiHighlightTexture? icon;
			if (server.IsRanked)
			{
				StringBuilder text2 = new StringBuilder();
				icon = MyGuiConstants.TEXTURE_ICON_STAR;
				row.AddCell(new MyGuiControlTable.Cell(text2, null, toolTip, color, icon, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			else
			{
				StringBuilder text3 = new StringBuilder();
				Color? textColor = color;
				icon = null;
				row.AddCell(new MyGuiControlTable.Cell(text3, null, toolTip, textColor, icon, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			string toolTip2 = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Passworded).ToString();
			if (server.Password)
			{
				StringBuilder text4 = new StringBuilder();
				icon = MyGuiConstants.TEXTURE_ICON_LOCK;
				row.AddCell(new MyGuiControlTable.Cell(text4, null, toolTip2, color, icon, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			else
			{
				StringBuilder text5 = new StringBuilder();
				Color? textColor2 = color;
				icon = null;
				row.AddCell(new MyGuiControlTable.Cell(text5, null, null, textColor2, icon, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			if (server.Experimental)
			{
				StringBuilder text6 = new StringBuilder();
				string @string = MyTexts.GetString(MyCommonTexts.ServerIsExperimental);
				icon = MyGuiConstants.TEXTURE_ICON_EXPERIMENTAL;
				row.AddCell(new MyGuiControlTable.Cell(text6, null, @string, color, icon, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			else
			{
				StringBuilder text7 = new StringBuilder();
				Color? textColor3 = color;
				icon = null;
				row.AddCell(new MyGuiControlTable.Cell(text7, null, null, textColor3, icon, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			}
			m_textCache.Clear().Append(sessionName);
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.AppendLine(sessionName);
			if (server.Experimental)
			{
				stringBuilder2.Append(MyTexts.GetString(MyCommonTexts.ServerIsExperimental));
			}
			StringBuilder textCache = m_textCache;
			object userData = server.GameID;
			string toolTip3 = stringBuilder2.ToString();
			Color? textColor4 = color;
			icon = null;
			row.AddCell(new MyGuiControlTable.Cell(textCache, userData, toolTip3, textColor4, icon));
			string toolTip4 = gamemodeToolTipSB.ToString();
			Color? textColor5 = color;
			icon = null;
			row.AddCell(new MyGuiControlTable.Cell(gamemodeSB, null, toolTip4, textColor5, icon));
			int num = server.ConnectionString.IndexOf("://");
			string text8 = ((num == -1) ? "steam" : server.ConnectionString.Substring(0, num));
			string toolTip5 = MyTexts.Get(MyStringId.GetOrCompute("JoinGame_Networking_" + text8)).ToString();
			StringBuilder text9 = new StringBuilder();
			icon = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\Services\\" + text8 + ".png",
				Highlight = "Textures\\GUI\\Icons\\Services\\" + text8 + ".png",
				SizePx = new Vector2(24f, 24f)
			};
			row.AddCell(new MyGuiControlTable.Cell(text9, null, toolTip5, color, icon, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			StringBuilder text10 = m_textCache.Clear().Append(server.Name);
			string toolTip6 = m_gameTypeToolTip.Clear().AppendLine(server.Name).Append(server.ConnectionString)
				.ToString();
			Color? textColor6 = color;
			icon = null;
			row.AddCell(new MyGuiControlTable.Cell(text10, null, toolTip6, textColor6, icon));
			string toolTip7 = stringBuilder.ToString();
			Color? textColor7 = color;
			icon = null;
			row.AddCell(new MyGuiControlTable.Cell(stringBuilder, null, toolTip7, textColor7, icon));
			if (SupportsPing)
			{
				StringBuilder text11 = m_textCache.Clear().Append((server.Ping < 0) ? "---" : server.Ping.ToString());
				string toolTip8 = m_textCache.ToString();
				Color? textColor8 = color;
				icon = null;
				row.AddCell(new MyGuiControlTable.Cell(text11, null, toolTip8, textColor8, icon));
			}
			if (MyPlatformGameSettings.IsModdingAllowed)
			{
				StringBuilder text12 = m_textCache.Clear().Append((gameTagByPrefixUlong == 0L) ? "---" : gameTagByPrefixUlong.ToString());
				Color? textColor9 = color;
				icon = null;
				row.AddCell(new MyGuiControlTable.Cell(text12, null, null, textColor9, icon));
			}
			if (server.IsRanked)
			{
				row.IsGlobalSortEnabled = false;
				m_gamesTable.Insert(0, row);
			}
			else
			{
				m_gamesTable.Add(row);
			}
			if (sort && !server.IsRanked)
			{
				MyGuiControlTable.Row selectedRow = m_gamesTable.SelectedRow;
				m_gamesTable.Sort(switchSort: false);
				m_gamesTable.SelectedRowIndex = m_gamesTable.FindRow(selectedRow);
			}
		}

		private void InitLobbyPage()
		{
			InitLobbyTable();
			m_detailsButton.Enabled = false;
			HideNetworkingButtons();
			m_joinButton.ButtonClicked += OnJoinServer;
			m_refreshButton.ButtonClicked += OnRefreshLobbiesClick;
			this.RefreshRequest = OnRefreshLobbiesClick;
			m_searchChangedFunc = (Action)Delegate.Combine(m_searchChangedFunc, new Action(RefreshGameList));
			m_lobbyPage = m_selectedPage;
			m_lobbyPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Lobbies));
			LoadPublicLobbies();
		}

		private void CloseLobbyPage()
		{
			m_searchChangedFunc = (Action)Delegate.Remove(m_searchChangedFunc, new Action(LoadPublicLobbies));
		}

		private void InitLobbyTable()
		{
			m_gamesTable.ColumnsCount = (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME ? 6 : 5);
			m_gamesTable.ItemSelected += OnTableItemSelected;
			m_gamesTable.ItemDoubleClicked += OnTableItemDoubleClick;
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				MyGuiControlTable gamesTable = m_gamesTable;
				float[] obj = new float[6] { 0.3f, 0.18f, 0.2f, 0.16f, 0.08f, 0f };
				obj[5] = (MyPlatformGameSettings.IsModdingAllowed ? 0.07f : 0f);
				gamesTable.SetCustomColumnWidths(obj);
			}
			else
			{
				MyGuiControlTable gamesTable2 = m_gamesTable;
				float[] obj2 = new float[5] { 0.29f, 0.19f, 0.37f, 0.08f, 0f };
				obj2[4] = (MyPlatformGameSettings.IsModdingAllowed ? 0.07f : 0f);
				gamesTable2.SetCustomColumnWidths(obj2);
			}
			int num = 0;
			m_gamesTable.SetColumnComparison(num++, TextComparison);
			m_gamesTable.SetColumnComparison(num++, TextComparison);
			m_gamesTable.SetColumnComparison(num++, TextComparison);
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_gamesTable.SetColumnComparison(num, TextComparison);
				m_gamesTable.SetColumnAlign(num);
				m_gamesTable.SetHeaderColumnAlign(num);
				num++;
			}
			m_gamesTable.SetColumnComparison(num, PlayerCountComparison);
			m_gamesTable.SetColumnAlign(num);
			m_gamesTable.SetHeaderColumnAlign(num);
			num++;
			m_gamesTable.SetColumnComparison(num, ModsComparison);
			m_gamesTable.SetColumnAlign(num);
			m_gamesTable.SetHeaderColumnAlign(num);
			num++;
			SupportsPing = false;
			SupportsGroups = false;
			AddHeaders();
		}

		private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			sender.CanHaveFocus = true;
			base.FocusedControl = sender;
			if (m_gamesTable.SelectedRow != null)
			{
				m_joinButton.Enabled = true;
				if (m_gamesTable.SelectedRow.UserData is MyGameServerItem)
				{
					m_detailsButton.Enabled = true;
				}
			}
			else
			{
				m_joinButton.Enabled = false;
				m_detailsButton.Enabled = false;
			}
		}

		private void OnRefreshLobbiesClick(MyGuiControlButton obj)
		{
			LoadPublicLobbies();
		}

		private void PublicLobbiesCallback(bool success)
		{
			if (m_selectedPage == m_lobbyPage)
			{
				if (!success)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder("Cannot enumerate worlds")));
					return;
				}
				m_lobbies.Clear();
				MyGameService.AddPublicLobbies(m_lobbies);
				RefreshGameList();
				m_loadingWheel.Visible = false;
			}
		}

		private void LoadPublicLobbies()
		{
			m_loadingWheel.Visible = true;
			MySandboxGame.Log.WriteLine("Requesting lobbies");
			if (FilterOptions.SameVersion)
			{
				MyGameService.AddLobbyFilter("appVersion", MyFinalBuildConstants.APP_VERSION.ToString());
			}
			MySandboxGame.Log.WriteLine("Requesting worlds, only compatible: " + FilterOptions.SameVersion);
			MyGameService.RequestLobbyList(PublicLobbiesCallback);
		}

		private void AddHeaders()
		{
			int num = 0;
			m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_World));
			m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_GameMode));
			m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Username));
			if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_RemainingTime));
			}
			m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Players));
			if (MyPlatformGameSettings.IsModdingAllowed)
			{
				m_gamesTable.SetColumnName(num++, MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Mods));
			}
		}

		private void RefreshGameList()
		{
<<<<<<< HEAD
=======
			//IL_0492: Unknown result type (might be due to invalid IL or missing references)
			//IL_0497: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (base.State == MyGuiScreenState.CLOSED)
			{
				return;
			}
			m_gamesTable.Clear();
			AddHeaders();
			m_textCache.Clear();
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			m_lobbyPage.Text = MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Lobbies);
			if (m_lobbies != null)
			{
				int num = 0;
				for (int i = 0; i < m_lobbies.Count; i++)
				{
					IMyLobby myLobby = m_lobbies[i];
					MyGuiControlTable.Row row = new MyGuiControlTable.Row(myLobby);
					if (FilterOptions.AdvancedFilter && !FilterOptions.FilterLobby(myLobby))
					{
						continue;
					}
					string lobbyWorldName = MyMultiplayerLobby.GetLobbyWorldName(myLobby);
					MyMultiplayerLobby.GetLobbyWorldSize(myLobby);
					int lobbyAppVersion = MyMultiplayerLobby.GetLobbyAppVersion(myLobby);
					int lobbyModCount = MyMultiplayerLobby.GetLobbyModCount(myLobby);
					string text = null;
					float? num2 = null;
					string text2 = m_searchBox.SearchText.Trim();
					if (!string.IsNullOrWhiteSpace(text2) && !lobbyWorldName.ToLower().Contains(text2.ToLower()))
					{
						continue;
					}
					m_gameTypeText.Clear();
					m_gameTypeToolTip.Clear();
					float lobbyFloat = MyMultiplayerLobby.GetLobbyFloat("inventoryMultiplier", myLobby, 1f);
					float lobbyFloat2 = MyMultiplayerLobby.GetLobbyFloat("refineryMultiplier", myLobby, 1f);
					float lobbyFloat3 = MyMultiplayerLobby.GetLobbyFloat("assemblerMultiplier", myLobby, 1f);
					float lobbyFloat4 = MyMultiplayerLobby.GetLobbyFloat("blocksInventoryMultiplier", myLobby, 1f);
					MyGameModeEnum lobbyGameMode = MyMultiplayerLobby.GetLobbyGameMode(myLobby);
					if (MyMultiplayerLobby.GetLobbyScenario(myLobby))
					{
						m_gameTypeText.AppendStringBuilder(MyTexts.Get(MySpaceTexts.WorldSettings_GameScenario));
						DateTime lobbyDateTime = MyMultiplayerLobby.GetLobbyDateTime("scenarioStartTime", myLobby, DateTime.MinValue);
						if (lobbyDateTime > DateTime.MinValue)
						{
							TimeSpan timeSpan = DateTime.UtcNow - lobbyDateTime;
							double num3 = Math.Truncate(timeSpan.TotalHours);
							int num4 = (int)((timeSpan.TotalHours - num3) * 60.0);
							m_gameTypeText.Append(" ").Append(num3).Append(":")
								.Append(num4.ToString("D2"));
						}
						else
						{
							m_gameTypeText.Append(" Lobby");
						}
					}
					else
					{
						switch (lobbyGameMode)
						{
						case MyGameModeEnum.Creative:
							if (!FilterOptions.CreativeMode)
							{
								continue;
							}
							m_gameTypeText.AppendStringBuilder(MyTexts.Get(MyCommonTexts.WorldSettings_GameModeCreative));
							break;
						case MyGameModeEnum.Survival:
							if (!FilterOptions.SurvivalMode)
							{
								continue;
							}
							m_gameTypeText.AppendStringBuilder(MyTexts.Get(MyCommonTexts.WorldSettings_GameModeSurvival));
							m_gameTypeText.Append($" {lobbyFloat}-{lobbyFloat4}-{lobbyFloat3}-{lobbyFloat2}");
							break;
						}
					}
					m_gameTypeToolTip.AppendFormat(MyTexts.Get(MyCommonTexts.JoinGame_GameTypeToolTip_MultipliersFormat).ToString(), lobbyFloat, lobbyFloat4, lobbyFloat3, lobbyFloat2);
					int lobbyViewDistance = MyMultiplayerLobby.GetLobbyViewDistance(myLobby);
					m_gameTypeToolTip.AppendLine();
					m_gameTypeToolTip.AppendFormat(MyTexts.Get(MyCommonTexts.JoinGame_GameTypeToolTip_ViewDistance).ToString(), lobbyViewDistance);
					if (string.IsNullOrEmpty(lobbyWorldName) || (FilterOptions.SameVersion && lobbyAppVersion != (int)MyFinalBuildConstants.APP_VERSION) || (FilterOptions.SameData && MyFakes.ENABLE_MP_DATA_HASHES && !MyMultiplayerLobby.HasSameData(myLobby)))
					{
						continue;
					}
					string lobbyHostName = MyMultiplayerLobby.GetLobbyHostName(myLobby);
					int memberLimit = myLobby.MemberLimit;
					if (memberLimit == 0)
					{
						continue;
					}
					string text3 = memberLimit.ToString();
					string value = myLobby.MemberCount + "/" + text3;
					if ((FilterOptions.CheckDistance && !FilterOptions.ViewDistance.ValueBetween(MyMultiplayerLobby.GetLobbyViewDistance(myLobby))) || (FilterOptions.CheckPlayer && !FilterOptions.PlayerCount.ValueBetween(myLobby.MemberCount)) || (FilterOptions.CheckMod && !FilterOptions.ModCount.ValueBetween(lobbyModCount)))
					{
						continue;
					}
					List<MyObjectBuilder_Checkpoint.ModItem> lobbyMods = MyMultiplayerLobby.GetLobbyMods(myLobby);
					if (FilterOptions.Mods != null && Enumerable.Any<WorkshopId>((IEnumerable<WorkshopId>)FilterOptions.Mods) && FilterOptions.AdvancedFilter)
					{
						if (FilterOptions.ModsExclusive)
						{
							bool flag = false;
<<<<<<< HEAD
							foreach (WorkshopId modId in FilterOptions.Mods)
							{
								if (lobbyMods == null || !lobbyMods.Any((MyObjectBuilder_Checkpoint.ModItem m) => m.PublishedFileId == modId.Id && m.PublishedServiceName == modId.ServiceName))
=======
							Enumerator<WorkshopId> enumerator = FilterOptions.Mods.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								{
									WorkshopId modId = enumerator.get_Current();
									if (lobbyMods == null || !Enumerable.Any<MyObjectBuilder_Checkpoint.ModItem>((IEnumerable<MyObjectBuilder_Checkpoint.ModItem>)lobbyMods, (Func<MyObjectBuilder_Checkpoint.ModItem, bool>)((MyObjectBuilder_Checkpoint.ModItem m) => m.PublishedFileId == modId.Id && m.PublishedServiceName == modId.ServiceName)))
									{
										flag = true;
										break;
									}
								}
							}
							finally
							{
								((IDisposable)enumerator).Dispose();
							}
							if (flag)
							{
								continue;
							}
						}
<<<<<<< HEAD
						else if (lobbyMods == null || !lobbyMods.Any((MyObjectBuilder_Checkpoint.ModItem m) => FilterOptions.Mods.Contains(new WorkshopId(m.PublishedFileId, m.PublishedServiceName))))
=======
						else if (lobbyMods == null || !Enumerable.Any<MyObjectBuilder_Checkpoint.ModItem>((IEnumerable<MyObjectBuilder_Checkpoint.ModItem>)lobbyMods, (Func<MyObjectBuilder_Checkpoint.ModItem, bool>)((MyObjectBuilder_Checkpoint.ModItem m) => FilterOptions.Mods.Contains(new WorkshopId(m.PublishedFileId, m.PublishedServiceName)))))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							continue;
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					int val = 15;
					int num5 = Math.Min(val, lobbyModCount - 1);
					foreach (MyObjectBuilder_Checkpoint.ModItem item in lobbyMods)
					{
						if (val-- <= 0)
						{
							stringBuilder.Append("...");
							break;
						}
						if (num5-- <= 0)
						{
							stringBuilder.Append(item.FriendlyName);
						}
						else
						{
							stringBuilder.AppendLine(item.FriendlyName);
						}
					}
					row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(lobbyWorldName), myLobby.LobbyId, m_textCache.ToString()));
					row.AddCell(new MyGuiControlTable.Cell(m_gameTypeText, null, (m_gameTypeToolTip.Length > 0) ? m_gameTypeToolTip.ToString() : null));
					row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(lobbyHostName), null, m_textCache.ToString()));
					if (MyFakes.ENABLE_JOIN_SCREEN_REMAINING_TIME)
					{
						if (text != null)
						{
							row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(text)));
						}
						else if (num2.HasValue)
						{
							row.AddCell(new CellRemainingTime(num2.Value));
						}
						else
						{
							row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear()));
						}
					}
					row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(value)));
					if (MyPlatformGameSettings.IsModdingAllowed)
					{
						row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append((lobbyModCount == 0) ? "---" : lobbyModCount.ToString()), null, stringBuilder.ToString()));
					}
					else
					{
						row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear()));
					}
					m_gamesTable.Add(row);
					num++;
				}
				m_lobbyPage.Text = new StringBuilder().Append((object)MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Lobbies)).Append(" (").Append(num)
					.Append(")");
			}
			m_gamesTable.SelectedRowIndex = null;
		}

		public void RemoveFavoriteServer(MyCachedServerItem server)
		{
			m_favoriteServers.Remove(server);
			refresh_favorites = true;
		}

		private void InitFavoritesPage()
		{
			InitServersTable(MyGameService.ServerDiscovery.PingSupport, MyGameService.ServerDiscovery.GroupSupport);
			m_joinButton.ButtonClicked += OnJoinServer;
			m_refreshButton.ButtonClicked += OnRefreshFavoritesServersClick;
			this.RefreshRequest = OnRefreshFavoritesServersClick;
			m_stopServerRequest = CloseFavoritesRequest;
			ShowNetworkingButtons();
			m_detailsButton.UserData = m_favoriteServers;
			m_favoritesResponding = true;
			m_searchChangedFunc = (Action)Delegate.Combine(m_searchChangedFunc, new Action(RefreshFavoritesGameList));
			m_favoritesPage = m_selectedPage;
			m_favoritesPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Favorites));
			RefreshFavoritesGameList();
		}

		private void CloseFavoritesPage()
		{
			CloseFavoritesRequest();
			m_searchChangedFunc = (Action)Delegate.Remove(m_searchChangedFunc, new Action(RefreshFavoritesGameList));
			m_favoritesResponding = false;
		}

		private void OnRefreshFavoritesServersClick(MyGuiControlButton obj)
		{
			RefreshFavoritesGameList();
		}

		private void RefreshFavoritesGameList()
		{
			CloseFavoritesRequest();
			m_gamesTable.Clear();
			AddServerHeaders(MyGameService.ServerDiscovery.PingSupport);
			m_textCache.Clear();
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			m_favoriteServers.Clear();
			m_favoritesPage.Text = new StringBuilder().Append((object)MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Favorites));
			if (m_enableDedicatedServers)
			{
				MySandboxGame.Log.WriteLine("Requesting dedicated servers");
				m_currentServerDiscovery = m_serverDiscovery;
				m_currentServerDiscovery.OnFavoritesServerListResponded += OnFavoritesServerListResponded;
				m_currentServerDiscovery.OnFavoritesServersCompleteResponse += OnFavoritesServersCompleteResponse;
<<<<<<< HEAD
				MySessionSearchFilter networkFilter = FilterOptions.GetNetworkFilter(m_currentServerDiscovery.SupportedSearchParameters, m_searchBox.SearchText.ToLower());
=======
				MySessionSearchFilter networkFilter = FilterOptions.GetNetworkFilter(m_currentServerDiscovery.SupportedSearchParameters, m_searchBox.SearchText);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MySandboxGame.Log.WriteLine("Requesting favorite servers, filterOps: " + networkFilter);
				m_currentServerDiscovery.RequestFavoritesServerList(networkFilter);
				m_loadingWheel.Visible = true;
				m_gamesTable.SelectedRowIndex = null;
			}
		}

		private void OnFavoritesServerListResponded(object sender, int server)
		{
			MyCachedServerItem serverItem = new MyCachedServerItem(m_currentServerDiscovery.GetFavoritesServerDetails(server));
			if (serverItem != null)
			{
				MyGameService.GetServerRules(serverItem.Server, delegate(Dictionary<string, string> rules)
				{
					FavoritesRulesResponse(rules, serverItem);
				}, delegate
				{
					FavoritesRulesResponse(null, serverItem);
				});
			}
		}

		private void FavoritesRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
		{
<<<<<<< HEAD
			if (string.IsNullOrEmpty(server.Server.ConnectionString) || m_favoriteServers.Any((MyCachedServerItem x) => server.Server.ConnectionString.Equals(x.Server.ConnectionString)) || (m_serverDiscoveryAggregator != null && m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) != m_currentServerDiscovery))
=======
			if (string.IsNullOrEmpty(server.Server.ConnectionString) || Enumerable.Any<MyCachedServerItem>((IEnumerable<MyCachedServerItem>)m_favoriteServers, (Func<MyCachedServerItem, bool>)((MyCachedServerItem x) => server.Server.ConnectionString.Equals(x.Server.ConnectionString))) || (m_serverDiscoveryAggregator != null && m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) != m_currentServerDiscovery))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			server.Rules = rules;
			if (rules != null)
			{
				server.DeserializeSettings();
			}
			m_favoriteServers.Add(server);
			if (m_favoritesResponding)
			{
				server.Server.IsRanked = IsRanked(server);
				AddServerItem(server);
				if (!m_refreshPaused)
				{
					m_favoritesPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Favorites)).Append(" (")
						.Append(m_gamesTable.RowsCount)
						.Append(")");
				}
			}
		}

		private void RebuildFavoritesList()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			m_detailsButton.Enabled = false;
			m_joinButton.Enabled = false;
			m_gamesTable.Clear();
			Enumerator<MyCachedServerItem> enumerator = m_favoriteServers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyCachedServerItem current = enumerator.get_Current();
					AddServerItem(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_favoritesPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Favorites)).Append(" (")
				.Append(m_gamesTable.RowsCount)
				.Append(")");
		}

		private void OnFavoritesServersCompleteResponse(object sender, MyMatchMakingServerResponse response)
		{
			CloseFavoritesRequest();
		}

		private void CloseFavoritesRequest()
		{
			if (m_currentServerDiscovery != null)
			{
				m_currentServerDiscovery.OnFavoritesServerListResponded -= OnFavoritesServerListResponded;
				m_currentServerDiscovery.OnFavoritesServersCompleteResponse -= OnFavoritesServersCompleteResponse;
				m_currentServerDiscovery.CancelFavoritesServersRequest();
			}
			m_loadingWheel.Visible = false;
		}

		private void InitHistoryPage()
		{
			InitServersTable(MyGameService.ServerDiscovery.PingSupport, MyGameService.ServerDiscovery.GroupSupport);
			m_joinButton.ButtonClicked += OnJoinServer;
			m_refreshButton.ButtonClicked += OnRefreshHistoryServersClick;
			this.RefreshRequest = OnRefreshHistoryServersClick;
			m_stopServerRequest = CloseHistoryRequest;
			ShowNetworkingButtons();
			m_historyResponding = true;
			m_searchChangedFunc = (Action)Delegate.Combine(m_searchChangedFunc, new Action(RefreshHistoryGameList));
			m_historyPage = m_selectedPage;
			m_historyPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_History));
			m_detailsButton.UserData = m_historyServers;
			RefreshHistoryGameList();
		}

		private void CloseHistoryPage()
		{
			CloseHistoryRequest();
			m_historyResponding = false;
			m_searchChangedFunc = (Action)Delegate.Remove(m_searchChangedFunc, new Action(RefreshHistoryGameList));
		}

		private void OnRefreshHistoryServersClick(MyGuiControlButton obj)
		{
			RefreshHistoryGameList();
		}

		private void RefreshHistoryGameList()
		{
			CloseHistoryRequest();
			m_gamesTable.Clear();
			AddServerHeaders(MyGameService.ServerDiscovery.PingSupport);
			m_textCache.Clear();
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			m_historyServers.Clear();
			m_historyPage.Text = new StringBuilder().Append((object)MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_History));
			if (m_enableDedicatedServers)
			{
				MySandboxGame.Log.WriteLine("Requesting dedicated servers");
				m_currentServerDiscovery = m_serverDiscovery;
				m_currentServerDiscovery.OnHistoryServerListResponded += OnHistoryServerListResponded;
				m_currentServerDiscovery.OnHistoryServersCompleteResponse += OnHistoryServersCompleteResponse;
<<<<<<< HEAD
				MySessionSearchFilter networkFilter = FilterOptions.GetNetworkFilter(m_currentServerDiscovery.SupportedSearchParameters, m_searchBox.SearchText.ToLower());
=======
				MySessionSearchFilter networkFilter = FilterOptions.GetNetworkFilter(m_currentServerDiscovery.SupportedSearchParameters, m_searchBox.SearchText);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MySandboxGame.Log.WriteLine("Requesting history servers, filterOps: " + networkFilter);
				m_currentServerDiscovery.RequestHistoryServerList(networkFilter);
				m_loadingWheel.Visible = true;
				m_gamesTable.SelectedRowIndex = null;
			}
		}

		private void OnHistoryServerListResponded(object sender, int server)
		{
			MyCachedServerItem serverItem = new MyCachedServerItem(m_currentServerDiscovery.GetHistoryServerDetails(server));
			if (serverItem != null)
			{
				MyGameService.GetServerRules(serverItem.Server, delegate(Dictionary<string, string> rules)
				{
					HistoryRulesResponse(rules, serverItem);
				}, delegate
				{
					HistoryRulesResponse(null, serverItem);
				});
			}
		}

		private void HistoryRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
		{
<<<<<<< HEAD
			if (string.IsNullOrEmpty(server.Server.ConnectionString) || m_historyServers.Any((MyCachedServerItem x) => server.Server.ConnectionString.Equals(x.Server.ConnectionString)) || (m_serverDiscoveryAggregator != null && m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) != m_currentServerDiscovery))
=======
			if (string.IsNullOrEmpty(server.Server.ConnectionString) || Enumerable.Any<MyCachedServerItem>((IEnumerable<MyCachedServerItem>)m_historyServers, (Func<MyCachedServerItem, bool>)((MyCachedServerItem x) => server.Server.ConnectionString.Equals(x.Server.ConnectionString))) || (m_serverDiscoveryAggregator != null && m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) != m_currentServerDiscovery))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			server.Rules = rules;
			if (rules != null)
			{
				server.DeserializeSettings();
			}
			m_historyServers.Add(server);
			if (m_historyResponding)
			{
				server.Server.IsRanked = IsRanked(server);
				AddServerItem(server);
				if (!m_refreshPaused)
				{
					m_historyPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_History)).Append(" (")
						.Append(m_gamesTable.RowsCount)
						.Append(")");
				}
			}
		}

		private void OnHistoryServersCompleteResponse(object sender, MyMatchMakingServerResponse response)
		{
			CloseHistoryRequest();
		}

		private void CloseHistoryRequest()
		{
			if (m_currentServerDiscovery != null)
			{
				m_currentServerDiscovery.OnHistoryServerListResponded -= OnHistoryServerListResponded;
				m_currentServerDiscovery.OnHistoryServersCompleteResponse -= OnHistoryServersCompleteResponse;
				m_currentServerDiscovery.CancelHistoryServersRequest();
			}
			m_loadingWheel.Visible = false;
		}

		private void InitLANPage()
		{
			InitServersTable(MyGameService.ServerDiscovery.PingSupport, MyGameService.ServerDiscovery.GroupSupport);
			m_joinButton.ButtonClicked += OnJoinServer;
			m_refreshButton.ButtonClicked += OnRefreshLANServersClick;
			this.RefreshRequest = OnRefreshLANServersClick;
			m_stopServerRequest = CloseLANRequest;
			ShowNetworkingButtons();
			m_detailsButton.UserData = m_lanServers;
			m_lanResponding = true;
			m_searchChangedFunc = (Action)Delegate.Combine(m_searchChangedFunc, new Action(RefreshLANGameList));
			m_LANPage = m_selectedPage;
			m_LANPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_LAN));
			RefreshLANGameList();
		}

		private void CloseLANPage()
		{
			CloseLANRequest();
			m_lanResponding = false;
			m_searchChangedFunc = (Action)Delegate.Remove(m_searchChangedFunc, new Action(RefreshLANGameList));
		}

		private void OnRefreshLANServersClick(MyGuiControlButton obj)
		{
			RefreshLANGameList();
		}

		private void RefreshLANGameList()
		{
			CloseLANRequest();
			m_gamesTable.Clear();
			AddServerHeaders(MyGameService.ServerDiscovery.PingSupport);
			m_textCache.Clear();
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			m_lanServers.Clear();
			m_LANPage.Text = new StringBuilder().Append((object)MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_LAN));
			MySandboxGame.Log.WriteLine("Requesting dedicated servers");
			m_currentServerDiscovery = m_serverDiscovery;
			m_currentServerDiscovery.OnLANServerListResponded += OnLANServerListResponded;
			m_currentServerDiscovery.OnLANServersCompleteResponse += OnLANServersCompleteResponse;
<<<<<<< HEAD
=======
			m_currentServerDiscovery.RequestLANServerList();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_loadingWheel.Visible = true;
			m_currentServerDiscovery.RequestLANServerList();
			m_gamesTable.SelectedRowIndex = null;
		}

		private void OnLANServerListResponded(object sender, int server)
		{
			MyCachedServerItem serverItem = new MyCachedServerItem(m_currentServerDiscovery.GetLANServerDetails(server));
			if (serverItem != null)
			{
				MyGameService.GetServerRules(serverItem.Server, delegate(Dictionary<string, string> rules)
				{
					LanRulesResponse(rules, serverItem);
				}, delegate
				{
					LanRulesResponse(null, serverItem);
				});
			}
		}

		private void LanRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
		{
<<<<<<< HEAD
			if (string.IsNullOrEmpty(server.Server.ConnectionString) || m_lanServers.Any((MyCachedServerItem x) => server.Server.ConnectionString.Equals(x.Server.ConnectionString)) || (m_serverDiscoveryAggregator != null && m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) != m_currentServerDiscovery))
=======
			if (string.IsNullOrEmpty(server.Server.ConnectionString) || Enumerable.Any<MyCachedServerItem>((IEnumerable<MyCachedServerItem>)m_lanServers, (Func<MyCachedServerItem, bool>)((MyCachedServerItem x) => server.Server.ConnectionString.Equals(x.Server.ConnectionString))) || (m_serverDiscoveryAggregator != null && m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) != m_currentServerDiscovery))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			server.Rules = rules;
			if (rules != null)
			{
				server.DeserializeSettings();
			}
			m_lanServers.Add(server);
			if (m_lanResponding)
			{
				server.Server.IsRanked = IsRanked(server);
				AddServerItem(server);
				if (!m_refreshPaused)
				{
					m_LANPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_LAN)).Append(" (")
						.Append(m_gamesTable.RowsCount)
						.Append(")");
				}
			}
		}

		private void OnLANServersCompleteResponse(object sender, MyMatchMakingServerResponse response)
		{
			CloseLANRequest();
		}

		private void CloseLANRequest()
		{
			if (m_currentServerDiscovery != null)
			{
				m_currentServerDiscovery.OnLANServerListResponded -= OnLANServerListResponded;
				m_currentServerDiscovery.OnLANServersCompleteResponse -= OnLANServersCompleteResponse;
				m_currentServerDiscovery.CancelLANServersRequest();
			}
			m_loadingWheel.Visible = false;
		}

		private void InitFriendsPage()
		{
			InitServersTable(MyGameService.ServerDiscovery.PingSupport, MyGameService.ServerDiscovery.GroupSupport);
			m_joinButton.ButtonClicked += OnJoinServer;
			m_refreshButton.ButtonClicked += OnRefreshFriendsServersClick;
			this.RefreshRequest = OnRefreshFriendsServersClick;
			m_stopServerRequest = CloseFriendsRequest;
			ShowNetworkingButtons();
			m_searchChangedFunc = (Action)Delegate.Combine(m_searchChangedFunc, new Action(RefreshFriendsGameList));
			m_detailsButton.UserData = m_dedicatedServers;
			m_friendsPage = m_selectedPage;
			m_friendsPage.SetToolTip(MyTexts.GetString(MyCommonTexts.JoinGame_TabTooltip_Friends));
			if (m_friendIds == null)
			{
				m_friendIds = new HashSet<ulong>();
				m_friendNames = new HashSet<string>();
				RequestFriendsList();
			}
			RefreshFriendsGameList();
		}

		private void CloseFriendsPage()
		{
			CloseFriendsRequest();
			m_searchChangedFunc = (Action)Delegate.Remove(m_searchChangedFunc, new Action(RefreshFriendsGameList));
		}

		private void OnRefreshFriendsServersClick(MyGuiControlButton obj)
		{
			RefreshFriendsGameList();
		}

		/// <summary>
		/// Dedicated servers only return *names* of connected players,
		/// and lobbies only return *IDs*, so we need to get all ID and all names
		/// of all friends to filter our games list with.
		/// </summary>
		private void RequestFriendsList()
		{
			_ = DateTime.Now;
			int friendsCount = MyGameService.GetFriendsCount();
			for (int i = 0; i < friendsCount; i++)
			{
				ulong friendIdByIndex = MyGameService.GetFriendIdByIndex(i);
				string friendNameByIndex = MyGameService.GetFriendNameByIndex(i);
				m_friendIds.Add(friendIdByIndex);
				m_friendNames.Add(friendNameByIndex);
			}
		}

		private void RefreshFriendsGameList()
		{
			CloseFriendsRequest();
			m_gamesTable.Clear();
			AddServerHeaders(MyGameService.ServerDiscovery.PingSupport);
			m_textCache.Clear();
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			m_friendsPage.Text = new StringBuilder().Append((object)MyTexts.Get(MyCommonTexts.JoinGame_TabTitle_Friends));
			MySandboxGame.Log.WriteLine("Requesting dedicated servers");
			CloseFriendsRequest();
			if (FilterOptions.SameVersion)
			{
				MyGameService.AddLobbyFilter("appVersion", MyFinalBuildConstants.APP_VERSION.ToString());
			}
			MyGameService.RequestLobbyList(FriendsLobbyResponse);
			m_dedicatedServers.Clear();
			m_refreshButton.Text = MyTexts.GetString(MyCommonTexts.Refresh);
			m_nextState = RefreshStateEnum.Pause;
			m_refreshPaused = false;
			if (m_enableDedicatedServers)
			{
<<<<<<< HEAD
				MySessionSearchFilter networkFilter = FilterOptions.GetNetworkFilter(m_currentServerDiscovery.SupportedSearchParameters, m_searchBox.SearchText.ToLower());
=======
				MySessionSearchFilter networkFilter = FilterOptions.GetNetworkFilter(m_currentServerDiscovery.SupportedSearchParameters, m_searchBox.SearchText);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MySandboxGame.Log.WriteLine("Requesting dedicated servers, filterOps: " + networkFilter);
				m_currentServerDiscovery = m_serverDiscovery;
				m_currentServerDiscovery.OnDedicatedServerListResponded += OnFriendsServerListResponded;
				m_currentServerDiscovery.OnDedicatedServersCompleteResponse += OnFriendsServersCompleteResponse;
				m_currentServerDiscovery.RequestInternetServerList(networkFilter);
			}
			m_loadingWheel.Visible = true;
			m_gamesTable.SelectedRowIndex = null;
		}

		private void FriendsLobbyResponse(bool success)
		{
			if (!success)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder("Cannot enumerate worlds")));
			}
			m_lobbies.Clear();
			MyGameService.AddFriendLobbies(m_lobbies);
			MyGameService.AddPublicLobbies(m_lobbies);
			foreach (IMyLobby lobby in m_lobbies)
			{
				if (m_friendIds.Contains(lobby.OwnerId) || m_friendIds.Contains(MyMultiplayerLobby.GetLobbyHostSteamId(lobby)) || m_friendIds.Contains(lobby.LobbyId) || (lobby.MemberList != null && Enumerable.Any<ulong>(lobby.MemberList, (Func<ulong, bool>)((ulong m) => m_friendIds.Contains(m)))))
				{
					lock (m_friendsPage)
					{
						AddFriendLobby(lobby);
					}
				}
			}
		}

		private void OnFriendsServerListResponded(object sender, int server)
		{
			MyCachedServerItem serverItem = new MyCachedServerItem(m_currentServerDiscovery.GetDedicatedServerDetails(server));
			if (serverItem != null && serverItem.Server.Players > 0)
			{
				MyGameService.GetPlayerDetails(serverItem.Server, delegate(Dictionary<string, float> players)
				{
					LoadPlayersCompleted(server, players, serverItem);
				}, delegate
				{
					LoadPlayersCompleted(server, null, serverItem);
				});
			}
		}

		private void LoadPlayersCompleted(int server, Dictionary<string, float> players, MyCachedServerItem serverItem)
		{
			if (players != null && Enumerable.Any<string>((IEnumerable<string>)players.Keys, (Func<string, bool>)((string n) => m_friendNames.Contains(n))))
			{
				MyGameService.GetServerRules(serverItem.Server, delegate(Dictionary<string, string> rules)
				{
					FriendRulesResponse(rules, serverItem);
				}, delegate
				{
					FriendRulesResponse(null, serverItem);
				});
			}
		}

		private void FriendRulesResponse(Dictionary<string, string> rules, MyCachedServerItem server)
		{
<<<<<<< HEAD
			if (!string.IsNullOrEmpty(server.Server.ConnectionString) && !m_dedicatedServers.Any((MyCachedServerItem x) => server.Server.ConnectionString.Equals(x.Server.ConnectionString)) && (m_serverDiscoveryAggregator == null || m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) == m_currentServerDiscovery))
=======
			if (!string.IsNullOrEmpty(server.Server.ConnectionString) && !Enumerable.Any<MyCachedServerItem>((IEnumerable<MyCachedServerItem>)m_dedicatedServers, (Func<MyCachedServerItem, bool>)((MyCachedServerItem x) => server.Server.ConnectionString.Equals(x.Server.ConnectionString))) && (m_serverDiscoveryAggregator == null || m_serverDiscoveryAggregator.FindAggregate(server.Server.ConnectionString) == m_currentServerDiscovery))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				server.Rules = rules;
				if (rules != null)
				{
					server.DeserializeSettings();
				}
				m_dedicatedServers.Add(server);
				server.Server.IsRanked = IsRanked(server);
				lock (m_friendsPage)
				{
					AddServerItem(server);
					m_friendsPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Friends)).Append(" (")
						.Append(m_gamesTable.RowsCount)
						.Append(")");
				}
			}
		}

		private void OnFriendsServersCompleteResponse(object sender, MyMatchMakingServerResponse response)
		{
			CloseFriendsRequest();
		}

		private void CloseFriendsRequest()
		{
			if (m_currentServerDiscovery != null)
			{
				m_currentServerDiscovery.OnDedicatedServerListResponded -= OnFriendsServerListResponded;
				m_currentServerDiscovery.OnDedicatedServersCompleteResponse -= OnFriendsServersCompleteResponse;
				m_currentServerDiscovery.CancelInternetServersRequest();
			}
			m_loadingWheel.Visible = false;
		}

		private void AddFriendLobby(IMyLobby lobby)
		{
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			if (FilterOptions.AdvancedFilter && !FilterOptions.FilterLobby(lobby))
			{
				return;
			}
			string lobbyWorldName = MyMultiplayerLobby.GetLobbyWorldName(lobby);
			MyMultiplayerLobby.GetLobbyWorldSize(lobby);
			int lobbyAppVersion = MyMultiplayerLobby.GetLobbyAppVersion(lobby);
			int lobbyModCount = MyMultiplayerLobby.GetLobbyModCount(lobby);
			string text = m_searchBox.SearchText.Trim();
			if (!string.IsNullOrWhiteSpace(text) && !lobbyWorldName.ToLower().Contains(text.ToLower()))
			{
				return;
			}
			m_gameTypeText.Clear();
			m_gameTypeToolTip.Clear();
			float lobbyFloat = MyMultiplayerLobby.GetLobbyFloat("blocksInventoryMultiplier", lobby, 1f);
			float lobbyFloat2 = MyMultiplayerLobby.GetLobbyFloat("inventoryMultiplier", lobby, 1f);
			float lobbyFloat3 = MyMultiplayerLobby.GetLobbyFloat("refineryMultiplier", lobby, 1f);
			float lobbyFloat4 = MyMultiplayerLobby.GetLobbyFloat("assemblerMultiplier", lobby, 1f);
			MyGameModeEnum lobbyGameMode = MyMultiplayerLobby.GetLobbyGameMode(lobby);
			if (MyMultiplayerLobby.GetLobbyScenario(lobby))
			{
				m_gameTypeText.AppendStringBuilder(MyTexts.Get(MySpaceTexts.WorldSettings_GameScenario));
				DateTime lobbyDateTime = MyMultiplayerLobby.GetLobbyDateTime("scenarioStartTime", lobby, DateTime.MinValue);
				if (lobbyDateTime > DateTime.MinValue)
				{
					TimeSpan timeSpan = DateTime.UtcNow - lobbyDateTime;
					double num = Math.Truncate(timeSpan.TotalHours);
					int num2 = (int)((timeSpan.TotalHours - num) * 60.0);
					m_gameTypeText.Append(" ").Append(num).Append(":")
						.Append(num2.ToString("D2"));
				}
				else
				{
					m_gameTypeText.Append(" Lobby");
				}
			}
			else
			{
				switch (lobbyGameMode)
				{
				case MyGameModeEnum.Creative:
					if (!FilterOptions.CreativeMode)
					{
						return;
					}
					m_gameTypeText.AppendStringBuilder(MyTexts.Get(MyCommonTexts.WorldSettings_GameModeCreative));
					break;
				case MyGameModeEnum.Survival:
					if (!FilterOptions.SurvivalMode)
					{
						return;
					}
					m_gameTypeText.AppendStringBuilder(MyTexts.Get(MyCommonTexts.WorldSettings_GameModeSurvival));
					m_gameTypeText.Append($" {lobbyFloat2}-{lobbyFloat}-{lobbyFloat4}-{lobbyFloat3}");
					break;
				}
			}
			m_gameTypeToolTip.AppendFormat(MyTexts.Get(MyCommonTexts.JoinGame_GameTypeToolTip_MultipliersFormat).ToString(), lobbyFloat2, lobbyFloat, lobbyFloat4, lobbyFloat3);
			int lobbyViewDistance = MyMultiplayerLobby.GetLobbyViewDistance(lobby);
			m_gameTypeToolTip.AppendLine();
			m_gameTypeToolTip.AppendFormat(MyTexts.Get(MyCommonTexts.JoinGame_GameTypeToolTip_ViewDistance).ToString(), lobbyViewDistance);
			if (string.IsNullOrEmpty(lobbyWorldName) || (FilterOptions.SameVersion && lobbyAppVersion != (int)MyFinalBuildConstants.APP_VERSION) || (FilterOptions.SameData && MyFakes.ENABLE_MP_DATA_HASHES && !MyMultiplayerLobby.HasSameData(lobby)))
			{
				return;
			}
			string lobbyHostName = MyMultiplayerLobby.GetLobbyHostName(lobby);
			string text2 = lobby.MemberLimit.ToString();
			string value = lobby.MemberCount + "/" + text2;
			if ((FilterOptions.CheckDistance && !FilterOptions.ViewDistance.ValueBetween(MyMultiplayerLobby.GetLobbyViewDistance(lobby))) || (FilterOptions.CheckPlayer && !FilterOptions.PlayerCount.ValueBetween(lobby.MemberCount)) || (FilterOptions.CheckMod && !FilterOptions.ModCount.ValueBetween(lobbyModCount)))
			{
				return;
			}
			List<MyObjectBuilder_Checkpoint.ModItem> lobbyMods = MyMultiplayerLobby.GetLobbyMods(lobby);
			if (FilterOptions.Mods != null && Enumerable.Any<WorkshopId>((IEnumerable<WorkshopId>)FilterOptions.Mods) && FilterOptions.AdvancedFilter)
			{
				if (FilterOptions.ModsExclusive)
				{
					bool flag = false;
<<<<<<< HEAD
					foreach (WorkshopId modId in FilterOptions.Mods)
					{
						if (lobbyMods == null || !lobbyMods.Any((MyObjectBuilder_Checkpoint.ModItem m) => m.PublishedFileId == modId.Id && m.PublishedServiceName == modId.ServiceName))
=======
					Enumerator<WorkshopId> enumerator = FilterOptions.Mods.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							WorkshopId modId = enumerator.get_Current();
							if (lobbyMods == null || !Enumerable.Any<MyObjectBuilder_Checkpoint.ModItem>((IEnumerable<MyObjectBuilder_Checkpoint.ModItem>)lobbyMods, (Func<MyObjectBuilder_Checkpoint.ModItem, bool>)((MyObjectBuilder_Checkpoint.ModItem m) => m.PublishedFileId == modId.Id && m.PublishedServiceName == modId.ServiceName)))
							{
								flag = true;
								break;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					if (flag)
					{
						return;
					}
				}
<<<<<<< HEAD
				else if (lobbyMods == null || !lobbyMods.Any((MyObjectBuilder_Checkpoint.ModItem m) => FilterOptions.Mods.Contains(new WorkshopId(m.PublishedFileId, m.PublishedServiceName))))
=======
				else if (lobbyMods == null || !Enumerable.Any<MyObjectBuilder_Checkpoint.ModItem>((IEnumerable<MyObjectBuilder_Checkpoint.ModItem>)lobbyMods, (Func<MyObjectBuilder_Checkpoint.ModItem, bool>)((MyObjectBuilder_Checkpoint.ModItem m) => FilterOptions.Mods.Contains(new WorkshopId(m.PublishedFileId, m.PublishedServiceName)))))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			int val = 15;
			int num3 = Math.Min(val, lobbyModCount - 1);
			foreach (MyObjectBuilder_Checkpoint.ModItem item in lobbyMods)
			{
				if (val-- <= 0)
				{
					stringBuilder.Append("...");
					break;
				}
				if (num3-- <= 0)
				{
					stringBuilder.Append(item.FriendlyName);
				}
				else
				{
					stringBuilder.AppendLine(item.FriendlyName);
				}
			}
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(lobby);
			string toolTip = MyTexts.Get(MyCommonTexts.JoinGame_ColumnTitle_Rank).ToString();
			row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), null, toolTip, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), null, null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(), null, null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(lobbyWorldName), lobby.LobbyId, m_textCache.ToString()));
			row.AddCell(new MyGuiControlTable.Cell(m_gameTypeText, null, (m_gameTypeToolTip.Length > 0) ? m_gameTypeToolTip.ToString() : null));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear()));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(lobbyHostName), null, m_textCache.ToString()));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append(value)));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append("---")));
			row.AddCell(new MyGuiControlTable.Cell(m_textCache.Clear().Append((lobbyModCount == 0) ? "---" : lobbyModCount.ToString()), null, stringBuilder.ToString()));
			m_gamesTable.Add(row);
			m_friendsPage.Text.Clear().Append(MyTexts.GetString(MyCommonTexts.JoinGame_TabTitle_Friends)).Append(" (")
				.Append(m_gamesTable.RowsCount)
				.Append(")");
		}

		public override void CloseScreenNow(bool isUnloading = false)
		{
			m_searchChangedFunc = null;
			base.CloseScreenNow(isUnloading);
		}
	}
}
