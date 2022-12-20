using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public abstract class MyGuiScreenServerDetailsBase : MyGuiScreenBase
	{
		protected enum DetailsPageEnum
		{
			Settings,
			Mods,
			Players
		}

		private class LoadPlayersResult : IMyAsyncResult
		{
			public Dictionary<string, float> Players { get; private set; }

			public bool IsCompleted { get; private set; }

			public Task Task { get; private set; }

			public LoadPlayersResult(MyCachedServerItem server)
			{
				MyGameService.GetPlayerDetails(server.Server, LoadCompleted, delegate
				{
					LoadCompleted(null);
				});
			}

			private void LoadCompleted(Dictionary<string, float> players)
			{
				Players = players;
				IsCompleted = true;
			}
		}

		private class LoadModsResult : IMyAsyncResult
		{
			public bool IsCompleted => Task.IsComplete;

			public Task Task { get; private set; }

			public List<MyWorkshopItem> ServerMods { get; private set; }

			public LoadModsResult(MyCachedServerItem server)
			{
				LoadModsResult loadModsResult = this;
				ServerMods = new List<MyWorkshopItem>();
				Task = Parallel.Start(delegate
				{
					if (MyGameService.IsOnline && server.Mods != null && server.Mods.Count > 0)
					{
						MyWorkshop.GetItemsBlockingUGC(server.Mods, loadModsResult.ServerMods);
					}
				});
			}
		}

		private MyGuiControlButton m_btSettings;

		private MyGuiControlButton m_btMods;

		private MyGuiControlButton m_btPlayers;

		private MyGuiControlRotatingWheel m_loadingWheel;

		private bool m_serverIsFavorited;

		protected DetailsPageEnum m_currentPage;

		protected Vector2 m_currentPosition;

		protected List<MyWorkshopItem> m_mods;

		protected float m_padding = 0.02f;

		protected Dictionary<string, float> m_players;

		protected MyCachedServerItem m_server;

		private MyGuiControlButton m_btnJoin;

		private MyGuiControlButton m_btnAddFavorite;

		private MyGuiControlButton m_btnRemoveFavorite;

		private const int PAGE_COUNT = 3;

		protected MyObjectBuilder_SessionSettings Settings => m_server.Settings;

		protected MyGuiScreenServerDetailsBase(MyCachedServerItem server)
			: base(new Vector2(0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 0.9398855f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_server = server;
			CreateScreen();
			TestFavoritesGameList();
		}

		private void CreateScreen()
		{
			base.CanHideOthers = true;
			base.CanBeHidden = true;
			base.EnabledBackgroundFade = true;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.JoinGame_ServerDetails, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiControlSeparatorList myGuiControlSeparatorList3 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList3.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.15f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList3);
			MyGuiControlSeparatorList myGuiControlSeparatorList4 = new MyGuiControlSeparatorList();
			float num = 0.303f;
			if (m_server.ExperimentalMode)
			{
				num = 0.34f;
			}
			myGuiControlSeparatorList4.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - num), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList4);
			m_currentPosition = new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f - 0.003f, m_size.Value.Y / 2f - 0.116f);
			DrawButtons();
			m_loadingWheel = new MyGuiControlRotatingWheel(m_btPlayers.Position + new Vector2(0.137f, -0.004f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.2f);
			Controls.Add(m_loadingWheel);
			m_loadingWheel.Visible = false;
			if (!m_serverIsFavorited)
			{
				m_btnAddFavorite = new MyGuiControlButton(new Vector2(0f, 0f) - new Vector2(-0.003f, (0f - m_size.Value.Y) / 2f + 0.071f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ServerDetails_AddFavorite));
				m_btnAddFavorite.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_AddFavorite));
				m_btnAddFavorite.ButtonClicked += FavoriteButtonClick;
				m_btnAddFavorite.Visible = !MyInput.Static.IsJoystickLastUsed;
				Controls.Add(m_btnAddFavorite);
			}
			else
			{
				m_btnRemoveFavorite = new MyGuiControlButton(new Vector2(0f, 0f) - new Vector2(-0.003f, (0f - m_size.Value.Y) / 2f + 0.071f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ServerDetails_RemoveFavorite));
				m_btnRemoveFavorite.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_RemoveFavorite));
				m_btnRemoveFavorite.ButtonClicked += UnFavoriteButtonClick;
				m_btnRemoveFavorite.Visible = !MyInput.Static.IsJoystickLastUsed;
				Controls.Add(m_btnRemoveFavorite);
			}
			m_btnJoin = new MyGuiControlButton(new Vector2(0f, 0f) - new Vector2(0.18f, (0f - m_size.Value.Y) / 2f + 0.071f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.JoinGame_Title));
			m_btnJoin.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGame_JoinWorld));
			m_btnJoin.ButtonClicked += ConnectButtonClick;
			m_btnJoin.Visible = !MyInput.Static.IsJoystickLastUsed;
			Controls.Add(m_btnJoin);
			m_currentPosition.Y += 0.012f;
			AddLabel(MyCommonTexts.ServerDetails_Server, m_server.Server.Name);
			AddLabel(MyCommonTexts.ServerDetails_Map, m_server.Server.Map);
			AddLabel(MyCommonTexts.ServerDetails_Version, new MyVersion((int)m_server.Server.GetGameTagByPrefixUlong("version")).FormattedText.ToString().Replace("_", "."));
			AddLabel(MyCommonTexts.ServerDetails_IPAddress, m_server.Server.ConnectionString);
			if (m_server.ExperimentalMode)
			{
				AddLabel(MyCommonTexts.ServerIsExperimental);
			}
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(m_btnJoin.Position.X - minSizeGui.X / 2f, m_btnJoin.Position.Y));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			if (!m_serverIsFavorited)
			{
				base.GamepadHelpTextId = MySpaceTexts.ServerDetails_Help_ScreenAddFavorites;
			}
			else
			{
				base.GamepadHelpTextId = MySpaceTexts.ServerDetails_Help_ScreenRemoveFavorites;
			}
			m_currentPosition.Y += 0.028f;
			switch (m_currentPage)
			{
			case DetailsPageEnum.Settings:
				base.FocusedControl = m_btSettings;
				m_btSettings.Checked = true;
				m_btSettings.Selected = true;
				DrawSettings();
				break;
			case DetailsPageEnum.Mods:
				base.FocusedControl = m_btMods;
				m_btMods.Checked = true;
				m_btMods.Selected = true;
				DrawMods();
				break;
			case DetailsPageEnum.Players:
				base.FocusedControl = m_btPlayers;
				m_btPlayers.Checked = true;
				m_btPlayers.Selected = true;
				DrawPlayers();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public override bool Draw()
		{
			base.Draw();
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
				Vector2 positionAbsoluteTopLeft = m_btSettings.GetPositionAbsoluteTopLeft();
				Vector2 size = m_btSettings.Size;
				Vector2 normalizedCoord = positionAbsoluteTopLeft;
				normalizedCoord.Y += size.Y / 2f;
				normalizedCoord.X -= size.X / 6f;
				Vector2 normalizedCoord2 = positionAbsoluteTopLeft;
				normalizedCoord2.Y = normalizedCoord.Y;
				Color value = MyGuiControlBase.ApplyColorMaskModifiers(MyGuiConstants.LABEL_TEXT_COLOR, enabled: true, m_transitionAlpha);
				normalizedCoord2.X += 3f * size.X + size.X / 6f;
				MyGuiManager.DrawString("Blue", MyTexts.Get(MyCommonTexts.Gamepad_Help_TabControl_Left).ToString(), normalizedCoord, 1f, value, drawAlign);
				MyGuiManager.DrawString("Blue", MyTexts.Get(MyCommonTexts.Gamepad_Help_TabControl_Right).ToString(), normalizedCoord2, 1f, value, drawAlign);
			}
			return true;
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (receivedFocusInThisUpdate)
			{
				return;
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				ParseIPAndConnect();
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
			{
				if (!m_serverIsFavorited)
				{
					FavoriteButtonClick(null);
				}
				else
				{
					UnFavoriteButtonClick(null);
				}
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT))
			{
				ChangeCurentPage(forward: false);
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
			{
				ChangeCurentPage(forward: true);
			}
			m_btnJoin.Visible = !MyInput.Static.IsJoystickLastUsed;
			if (m_btnAddFavorite != null)
			{
				m_btnAddFavorite.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			if (m_btnRemoveFavorite != null)
			{
				m_btnRemoveFavorite.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Override this to draw controls specific to your game version
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected abstract void DrawSettings();

		private void ChangeCurentPage(bool forward)
		{
			int num = (int)(m_currentPage + (forward ? 1 : (-1))) % 3;
			m_currentPage = ((num < 0) ? DetailsPageEnum.Players : ((DetailsPageEnum)num));
			switch (m_currentPage)
			{
			default:
				SettingButtonClick(null);
				break;
			case DetailsPageEnum.Mods:
				if (m_btMods.Enabled)
				{
					ModsButtonClick(null);
				}
				else
				{
					ChangeCurentPage(forward);
				}
				break;
			case DetailsPageEnum.Players:
				PlayersButtonClick(null);
				break;
			}
		}

		private void DrawMods()
		{
			if (m_mods != null && m_mods.Count > 0)
			{
<<<<<<< HEAD
				double byteSize = m_mods.Sum((MyWorkshopItem m) => (long)m.Size);
=======
				double byteSize = Enumerable.Sum<MyWorkshopItem>((IEnumerable<MyWorkshopItem>)m_mods, (Func<MyWorkshopItem, long>)((MyWorkshopItem m) => (long)m.Size));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				string text = MyUtils.FormatByteSizePrefix(ref byteSize);
				AddLabel(MyCommonTexts.ServerDetails_ModDownloadSize, byteSize.ToString("0.") + " " + text + "B");
			}
			AddLabel(MyCommonTexts.WorldSettings_Mods, null);
			if (m_mods == null)
			{
				Controls.Add(new MyGuiControlLabel(m_currentPosition, null, MyTexts.GetString(MyCommonTexts.ServerDetails_ModError), null, 0.8f, "Red"));
				return;
			}
			if (m_mods.Count == 0)
			{
				Controls.Add(new MyGuiControlLabel(m_currentPosition, null, MyTexts.GetString(MyCommonTexts.ServerDetails_NoMods)));
				return;
			}
			m_mods.Sort((MyWorkshopItem a, MyWorkshopItem b) => string.Compare(a.Title, b.Title, StringComparison.CurrentCultureIgnoreCase));
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent();
			MyGuiControlScrollablePanel myGuiControlScrollablePanel = new MyGuiControlScrollablePanel(myGuiControlParent);
			myGuiControlScrollablePanel.ScrollbarVEnabled = true;
			myGuiControlScrollablePanel.Position = m_currentPosition;
			myGuiControlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlScrollablePanel.Size = new Vector2(base.Size.Value.X - 0.112f, base.Size.Value.Y / 2f - m_currentPosition.Y - 0.145f);
			myGuiControlScrollablePanel.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
			myGuiControlScrollablePanel.ScrolledAreaPadding = new MyGuiBorderThickness(0.005f);
			Controls.Add(myGuiControlScrollablePanel);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Close);
			myGuiControlParent.Size = new Vector2(myGuiControlScrollablePanel.Size.X, (float)m_mods.Count * (myGuiControlButton.Size.Y / 2f + m_padding) + myGuiControlButton.Size.Y / 2f);
			Vector2 value = new Vector2((0f - myGuiControlScrollablePanel.Size.X) / 2f, (0f - myGuiControlParent.Size.Y) / 2f);
			foreach (MyWorkshopItem mod in m_mods)
			{
				MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(value, MyGuiControlButtonStyleEnum.ClickableText, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, new StringBuilder(mod.Title));
				myGuiControlButton2.UserData = mod;
				int num = Math.Min(mod.Description.Length, 128);
				int num2 = mod.Description.IndexOf("\n");
				if (num2 > 0)
				{
					num = Math.Min(num, num2 - 1);
				}
				myGuiControlButton2.SetToolTip(mod.Description.Substring(0, num));
				myGuiControlButton2.ButtonClicked += ModURLClick;
				value.Y += myGuiControlButton2.Size.Y / 2f + m_padding;
				myGuiControlParent.Controls.Add(myGuiControlButton2);
			}
		}

		private void ModURLClick(MyGuiControlButton button)
		{
			MyGuiSandbox.OpenUrl((button.UserData as MyWorkshopItem).GetItemUrl(), UrlOpenMode.SteamOrExternalWithConfirm);
		}

		private void DrawPlayers()
		{
			MyGuiControlLabel myGuiControlLabel = AddLabel(MyCommonTexts.ScreenCaptionPlayers, null);
			if (m_players == null)
			{
				Controls.Add(new MyGuiControlLabel(m_currentPosition, null, MyTexts.GetString(MyCommonTexts.ServerDetails_PlayerError), null, 0.8f, "Red"));
				return;
			}
			if (m_players.Count == 0)
			{
				Controls.Add(new MyGuiControlLabel(m_currentPosition, null, MyTexts.GetString(MyCommonTexts.ServerDetails_ServerEmpty)));
				return;
			}
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent();
			MyGuiControlScrollablePanel myGuiControlScrollablePanel = new MyGuiControlScrollablePanel(myGuiControlParent);
			myGuiControlScrollablePanel.ScrollbarVEnabled = true;
			myGuiControlScrollablePanel.Position = m_currentPosition;
			myGuiControlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlScrollablePanel.Size = new Vector2(base.Size.Value.X - 0.112f, base.Size.Value.Y / 2f - m_currentPosition.Y - 0.145f);
			myGuiControlScrollablePanel.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
			myGuiControlScrollablePanel.ScrolledAreaPadding = new MyGuiBorderThickness(0.005f);
			Controls.Add(myGuiControlScrollablePanel);
			myGuiControlParent.Size = new Vector2(myGuiControlScrollablePanel.Size.X, (float)m_players.Count * (myGuiControlLabel.Size.Y / 2f + m_padding) + myGuiControlLabel.Size.Y / 2f);
			Vector2 value = new Vector2((0f - myGuiControlScrollablePanel.Size.X) / 2f, (0f - myGuiControlParent.Size.Y) / 2f + myGuiControlLabel.Size.Y / 2f);
			foreach (KeyValuePair<string, float> player in m_players)
			{
				StringBuilder stringBuilder = new StringBuilder(player.Key);
				if (player.Value >= 0f)
				{
					stringBuilder.Append(": ");
					MyValueFormatter.AppendTimeInBestUnit((int)player.Value, stringBuilder);
				}
				MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(value, null, stringBuilder.ToString());
				value.Y += myGuiControlLabel2.Size.Y / 2f + m_padding;
				myGuiControlParent.Controls.Add(myGuiControlLabel2);
			}
		}

		protected void DrawButtons()
		{
			float x = m_currentPosition.X;
			m_btSettings = new MyGuiControlButton(m_currentPosition, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ServerDetails_Settings));
			m_btSettings.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_Settings));
			m_btSettings.PositionX += m_btSettings.Size.X / 2f;
			m_currentPosition.X += m_btSettings.Size.X + m_padding / 4f;
			m_btSettings.ButtonClicked += SettingButtonClick;
			Controls.Add(m_btSettings);
			m_btMods = new MyGuiControlButton(m_currentPosition, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.WorldSettings_Mods));
			m_btMods.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_Mods));
			m_btMods.PositionX += m_btMods.Size.X / 2f;
			m_currentPosition.X += m_btMods.Size.X + m_padding / 4f;
			m_btMods.ButtonClicked += ModsButtonClick;
			m_btMods.Enabled = MyPlatformGameSettings.IsModdingAllowed;
			Controls.Add(m_btMods);
			m_btPlayers = new MyGuiControlButton(m_currentPosition, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenCaptionPlayers));
			m_btPlayers.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerDetails_Players));
			m_btPlayers.PositionX += m_btPlayers.Size.X / 2f;
			m_currentPosition.X += m_btPlayers.Size.X + m_padding / 4f;
			m_btPlayers.ButtonClicked += PlayersButtonClick;
			Controls.Add(m_btPlayers);
			m_currentPosition.X = x;
			m_currentPosition.Y += m_btSettings.Size.Y + m_padding / 2f;
		}

		public override string GetFriendlyName()
		{
			return "ServerDetails";
		}

		/// <summary>
		///     Uses reflection to find all relevant settings then returns a localized descriptor with the value for each
		/// </summary>
		/// <returns></returns>
		protected SortedList<string, object> LoadSessionSettings(VRage.Game.Game game)
		{
			if (Settings == null)
			{
				return null;
			}
			SortedList<string, object> result = new SortedList<string, object>();
			FieldInfo[] fields = typeof(MyObjectBuilder_SessionSettings).GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				GameRelationAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<GameRelationAttribute>(fieldInfo);
				if (customAttribute == null || (customAttribute.RelatedTo != 0 && customAttribute.RelatedTo != game))
				{
					continue;
				}
<<<<<<< HEAD
				DisplayAttribute customAttribute2 = CustomAttributeExtensions.GetCustomAttribute<DisplayAttribute>((MemberInfo)fieldInfo);
=======
				DisplayAttribute customAttribute2 = ((MemberInfo)fieldInfo).GetCustomAttribute<DisplayAttribute>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (customAttribute2 != null && !string.IsNullOrEmpty(customAttribute2.get_Name()))
				{
					string text = "ServerDetails_" + fieldInfo.Name;
					if (!(MyTexts.GetString(text) == text))
					{
						result.Add(text, fieldInfo.GetValue(Settings));
					}
				}
			}
			AddAdditionalSettings(ref result);
			return result;
		}

		private void AddAdditionalSettings(ref SortedList<string, object> result)
		{
			if (Settings != null)
			{
				result.Add(MyTexts.GetString(MyCommonTexts.ServerDetails_PCU_Initial), (object)MyObjectBuilder_SessionSettings.GetInitialPCU(Settings));
			}
		}

		private void ConnectButtonClick(MyGuiControlButton obj)
		{
			ParseIPAndConnect();
		}

		/// <summary>
		/// Tries to connect to loaded server
		/// </summary>
		/// <returns></returns>
		private void ParseIPAndConnect()
		{
			if (!MySandboxGame.Config.ExperimentalMode && m_server.ExperimentalMode)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: MyTexts.Get(MyCommonTexts.MultiplayerErrorExperimental)));
				return;
			}
			try
			{
				MyGameService.OnPingServerResponded += MySandboxGame.Static.ServerResponded;
				MyGameService.OnPingServerFailedToRespond += MySandboxGame.Static.ServerFailedToRespond;
				MyGameService.PingServer(m_server.Server.ConnectionString);
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				MyGuiSandbox.Show(MyTexts.Get(MyCommonTexts.MultiplayerJoinIPError), MyCommonTexts.MessageBoxCaptionError);
			}
		}

		private void CloseButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CloseScreen();
		}

		private void TestFavoritesGameList()
		{
			MyGameService.OnFavoritesServerListResponded += OnFavoritesServerListResponded;
			MyGameService.OnFavoritesServersCompleteResponse += OnFavoritesServersCompleteResponse;
			MyGameService.RequestFavoritesServerList(new MySessionSearchFilter());
			m_loadingWheel.Visible = true;
		}

		private void OnFavoritesServerListResponded(object sender, int server)
		{
			MyCachedServerItem myCachedServerItem = new MyCachedServerItem(MyGameService.GetFavoritesServerDetails(server));
<<<<<<< HEAD
			MyCachedServerItem server2 = m_server;
			if (myCachedServerItem.Server.ConnectionString == server2.Server.ConnectionString)
			{
				m_serverIsFavorited = true;
				RecreateControls(constructor: false);
				m_loadingWheel.Visible = false;
=======
			if (myCachedServerItem != null)
			{
				MyCachedServerItem server2 = m_server;
				if (myCachedServerItem.Server.ConnectionString == server2.Server.ConnectionString)
				{
					m_serverIsFavorited = true;
					RecreateControls(constructor: false);
					m_loadingWheel.Visible = false;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void OnFavoritesServersCompleteResponse(object sender, MyMatchMakingServerResponse response)
		{
			CloseFavoritesRequest();
		}

		private void CloseFavoritesRequest()
		{
			MyGameService.OnFavoritesServerListResponded -= OnFavoritesServerListResponded;
			MyGameService.OnFavoritesServersCompleteResponse -= OnFavoritesServersCompleteResponse;
			MyGameService.CancelFavoritesServersRequest();
			m_loadingWheel.Visible = false;
		}

		private void FavoriteButtonClick(MyGuiControlButton myGuiControlButton)
		{
			MyGameService.AddFavoriteGame(m_server.Server);
			m_serverIsFavorited = true;
			RecreateControls(constructor: false);
		}

		private void UnFavoriteButtonClick(MyGuiControlButton myGuiControlButton)
		{
			MyGameService.RemoveFavoriteGame(m_server.Server);
			MyGuiScreenJoinGame firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenJoinGame>();
			if (firstScreenOfType.m_selectedPage.Name == "PageFavoritesPanel")
			{
				firstScreenOfType.RemoveFavoriteServer(m_server);
			}
			m_serverIsFavorited = false;
			RecreateControls(constructor: false);
		}

		private void JoinButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CloseScreen();
		}

		private void SettingButtonClick(MyGuiControlButton myGuiControlButton)
		{
			m_currentPage = DetailsPageEnum.Settings;
			RecreateControls(constructor: false);
		}

		private void ModsButtonClick(MyGuiControlButton myGuiControlButton)
		{
			m_currentPage = DetailsPageEnum.Mods;
			if (m_server.Mods != null && m_server.Mods.Count > 0)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, BeginModResultAction, EndModResultAction));
			}
			else if (m_server.Mods != null && m_server.Mods.Count == 0)
			{
				m_mods = new List<MyWorkshopItem>();
				RecreateControls(constructor: false);
			}
			else
			{
				RecreateControls(constructor: false);
			}
		}

		private void PlayersButtonClick(MyGuiControlButton myGuiControlButton)
		{
			m_currentPage = DetailsPageEnum.Players;
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, BeginPlayerResultAction, EndPlayerResultAction));
		}

		private IMyAsyncResult BeginPlayerResultAction()
		{
			return new LoadPlayersResult(m_server);
		}

		private void EndPlayerResultAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			LoadPlayersResult loadPlayersResult = (LoadPlayersResult)result;
			m_players = loadPlayersResult.Players;
			screen.CloseScreen();
			m_loadingWheel.Visible = false;
			RecreateControls(constructor: false);
		}

		private IMyAsyncResult BeginModResultAction()
		{
			return new LoadModsResult(m_server);
		}

		private void EndModResultAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			LoadModsResult loadModsResult = (LoadModsResult)result;
			m_mods = loadModsResult.ServerMods;
			screen.CloseScreen();
			m_loadingWheel.Visible = false;
			RecreateControls(constructor: false);
		}

		protected void AddSeparator(MyGuiControlParent parent, Vector2 localPos, float size = 1f)
		{
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.Size = new Vector2(1f, 0.01f);
			myGuiControlSeparatorList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			myGuiControlSeparatorList.AddHorizontal(Vector2.Zero, size);
			myGuiControlSeparatorList.Position = new Vector2(localPos.X, localPos.Y - 0.02f);
			myGuiControlSeparatorList.Alpha = 0.4f;
			parent.Controls.Add(myGuiControlSeparatorList);
		}

		protected MyGuiControlLabel AddLabel(MyStringId description, object value)
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_currentPosition, null, $"{MyTexts.GetString(description)}: {value}");
			m_currentPosition.Y += myGuiControlLabel.Size.Y / 2f + m_padding;
			Controls.Add(myGuiControlLabel);
			return myGuiControlLabel;
		}

		protected MyGuiControlLabel AddLabel(MyStringId description)
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_currentPosition, null, MyTexts.GetString(description));
			m_currentPosition.Y += myGuiControlLabel.Size.Y / 2f + m_padding;
			Controls.Add(myGuiControlLabel);
			return myGuiControlLabel;
		}

		protected MyGuiControlMultilineText AddMultilineText(string text, float size)
		{
			return AddMultilineText(new StringBuilder(text), size);
		}

		protected MyGuiControlMultilineText AddMultilineText(StringBuilder text, float size)
		{
			m_currentPosition.Y -= m_padding / 2f;
			m_currentPosition.X += 0.003f;
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(m_currentPosition, new Vector2(base.Size.Value.X - 0.112f, size), null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_currentPosition.X -= 0.003f;
			myGuiControlMultilineText.Text = text;
			myGuiControlMultilineText.Position += myGuiControlMultilineText.Size / 2f;
			MyGuiControlCompositePanel control = new MyGuiControlCompositePanel
			{
				Position = m_currentPosition,
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER,
				Size = myGuiControlMultilineText.Size,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			myGuiControlMultilineText.Size = new Vector2(myGuiControlMultilineText.Size.X / 1.01f, myGuiControlMultilineText.Size.Y / 1.09f);
			m_currentPosition.Y += myGuiControlMultilineText.Size.Y + m_padding * 1.5f;
			Controls.Add(control);
			Controls.Add(myGuiControlMultilineText);
			return myGuiControlMultilineText;
		}

		protected MyGuiControlCheckbox AddCheckbox(MyStringId text, Action<MyGuiControlCheckbox> onClick, MyStringId? tooltip = null)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(m_currentPosition, null, tooltip.HasValue ? MyTexts.GetString(tooltip.Value) : string.Empty);
			myGuiControlCheckbox.PositionX += myGuiControlCheckbox.Size.X / 2f;
			Controls.Add(myGuiControlCheckbox);
			if (onClick != null)
			{
				myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, onClick);
			}
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_currentPosition, null, MyTexts.GetString(text));
			myGuiControlLabel.PositionX += myGuiControlCheckbox.Size.X + m_padding;
			Controls.Add(myGuiControlLabel);
			m_currentPosition.Y += myGuiControlCheckbox.Size.Y;
			return myGuiControlCheckbox;
		}
	}
}
