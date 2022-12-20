using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Localization;
using VRage.GameServices;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenNewWorkshopGame : MyGuiScreenBase
	{
		private class MyWorkshopItemComparer : IComparer<MyWorkshopItem>
		{
			private Func<MyWorkshopItem, MyWorkshopItem, int> comparator;

			public MyWorkshopItemComparer(Func<MyWorkshopItem, MyWorkshopItem, int> comp)
			{
				comparator = comp;
			}

			public int Compare(MyWorkshopItem x, MyWorkshopItem y)
			{
				if (comparator != null)
				{
					return comparator(x, y);
				}
				return 0;
			}
		}

		/// <summary>
		/// Workshop worlds
		/// </summary>
		private class LoadListResult : IMyAsyncResult
		{
			public (MyGameServiceCallResult, string) Result;

			/// <summary>
			/// List of worlds user is subscribed to, or null if there was an error
			/// during operation.
			/// </summary>
			public List<MyWorkshopItem> SubscribedWorlds;

			public bool IsCompleted => Task.IsComplete;

			public Task Task { get; private set; }

			public LoadListResult()
			{
				Task = Parallel.Start(delegate
				{
					LoadListAsync(out SubscribedWorlds);
				});
			}

			private void LoadListAsync(out List<MyWorkshopItem> list)
			{
				if (!MyGameService.IsActive || !MyGameService.IsOnline)
				{
					Result = (MyGameServiceCallResult.NoUser, MyGameService.GetDefaultUGC().ServiceName);
					list = null;
					return;
				}
				List<MyWorkshopItem> list2 = new List<MyWorkshopItem>();
				Result = MyWorkshop.GetSubscribedWorldsBlocking(list2);
				list = list2;
				List<MyWorkshopItem> list3 = new List<MyWorkshopItem>();
				(MyGameServiceCallResult, string) subscribedScenariosBlocking = MyWorkshop.GetSubscribedScenariosBlocking(list3);
				if (list3.Count > 0)
				{
					list.InsertRange(list.Count, list3);
				}
				SubscribedWorlds = list;
				MyWorkshop.TryUpdateWorldsBlocking(SubscribedWorlds, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo());
				if (Result.Item1 == MyGameServiceCallResult.OK)
				{
					Result = subscribedScenariosBlocking;
				}
			}
		}

		private class WorldDataLoaderInfo : WorkData
		{
			public MyObjectBuilder_Checkpoint World;

			public MyWorkshopItem WorkshopItem;
<<<<<<< HEAD

			public string Author = "";

			public bool IsMultiplayer;

=======

			public string Author = "";

			public bool IsMultiplayer;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public bool IsSkipped;
		}

		private MyGuiControlScreenSwitchPanel m_screenSwitchPanel;

		private MyGuiBlueprintScreen_Reworked.SortOption m_sort;

		private bool m_showThumbnails = true;

		private MyGuiControlButton m_buttonRefresh;

		private MyGuiControlButton m_buttonSorting;

		private MyGuiControlButton m_buttonToggleThumbnails;

		private MyGuiControlImage m_iconRefresh;

		private MyGuiControlImage m_iconSorting;

		private MyGuiControlImage m_iconToggleThumbnails;

		private MyGuiControlSearchBox m_searchBox;

		private MyGuiControlList m_worldList;

		private MyGuiControlRadioButtonGroup m_worldTypesGroup;

		private MyObjectBuilder_Checkpoint m_selectedWorld;

		private MyGuiControlContentButton m_selectedButton;

		private MyWorkshopItem m_selectedWorkshopItem;

		private readonly object m_selectedWorldLock = new object();

		private List<MyWorkshopItem> SubscribedWorlds;

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

		private MyGuiControlButton m_buttonRateUp;

		private MyGuiControlButton m_buttonRateDown;

		private MyGuiControlImage m_iconRateUp;

		private MyGuiControlImage m_iconRateDown;

		private MyGuiControlMultilineText m_noSubscribedItemsText;

		private MyGuiControlPanel m_noSubscribedItemsPanel;

		private MyGuiControlMultilineText m_descriptionMultilineText;

		private MyGuiControlPanel m_descriptionPanel;

		private MyGuiControlLabel m_workshopError;

		private MyGuiControlRotatingWheel m_asyncLoadingWheel;

		private float MARGIN_TOP = 0.22f;

		private float MARGIN_BOTTOM = 50f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;

		private float MARGIN_LEFT_INFO = 15f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private float MARGIN_RIGHT = 81f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private float MARGIN_LEFT_LIST = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		private bool m_displayTabScenario;

		private bool m_displayTabWorkshop;

		private bool m_displayTabCustom;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_workshopButton;

		private int m_maxPlayers;

		private MyGuiControlButton m_buttonGroup;

		private int m_groupSelection;

		private MyGuiControlImage m_iconGroupSelection;

		private bool m_workshopPermitted;

		public MyGuiScreenNewWorkshopGame(bool displayTabScenario = true, bool displayTabWorkshop = true, bool displayTabCustom = true)
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
			return "New Workshop Game";
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
			m_screenSwitchPanel = new MyGuiControlScreenSwitchPanel(this, MyTexts.Get(MyCommonTexts.WorkshopScreen_Description), m_displayTabScenario, m_displayTabWorkshop, m_displayTabCustom);
			Controls.Add(m_screenSwitchPanel);
			InitWorldList();
			InitRightSide();
			m_asyncLoadingWheel = new MyGuiControlRotatingWheel(new Vector2(m_size.Value.X / 2f - 0.077f, (0f - m_size.Value.Y) / 2f + 0.108f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.2f);
			m_asyncLoadingWheel.Visible = false;
			Controls.Add(m_asyncLoadingWheel);
			RefreshList();
			base.FocusedControl = m_searchBox.TextBox;
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

		private void InitWorldList()
		{
			float num = 0.31f;
			float x = 90f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			Vector2 position = -m_size.Value / 2f + new Vector2(x, num);
			int num2 = 0;
			m_buttonRefresh = CreateToolbarButton(num2++, MyCommonTexts.WorldSettings_Tooltip_Refresh, OnRefreshClicked);
			if (MyGameService.WorkshopService.GetAggregates().Count > 1)
			{
				string tooltip = string.Empty;
				if (MyGameService.WorkshopService.GetAggregates().Count > 1)
				{
					tooltip = string.Format(MyTexts.Get(MySpaceTexts.WorldSettings_Tooltip_ButGrouping).ToString(), MyGameService.WorkshopService.GetAggregates()[0].ServiceName, MyGameService.WorkshopService.GetAggregates()[1].ServiceName);
				}
				m_buttonGroup = CreateToolbarButton(num2++, tooltip, OnButton_GroupSelection);
				m_iconGroupSelection = CreateToolbarButtonIcon(m_buttonGroup, "");
				SetIconForGroupSelection();
			}
			m_buttonSorting = CreateToolbarButton(num2++, MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButSort, OnSortingClicked);
			m_buttonToggleThumbnails = CreateToolbarButton(num2++, MyCommonTexts.WorldSettings_Tooltip_ToggleThumbnails, OnToggleThumbnailsClicked);
<<<<<<< HEAD
			MyGuiControlButton myGuiControlButton = CreateToolbarButton(num2++, MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButOpenWorkshop, OnOpenWorkshopClicked);
			CreateToolbarButtonIcon(myGuiControlButton, "Textures\\GUI\\Icons\\Browser\\WorkshopBrowser.dds");
			myGuiControlButton.Enabled = MyGameService.IsActive;
=======
			MyGuiControlButton button = CreateToolbarButton(num2++, MySpaceTexts.ScreenBlueprintsRew_Tooltip_ButOpenWorkshop, OnOpenWorkshopClicked);
			CreateToolbarButtonIcon(button, "Textures\\GUI\\Icons\\Browser\\WorkshopBrowser.dds");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_iconRefresh = CreateToolbarButtonIcon(m_buttonRefresh, "Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
			m_iconSorting = CreateToolbarButtonIcon(m_buttonSorting, "");
			SetIconForSorting();
			m_iconToggleThumbnails = CreateToolbarButtonIcon(m_buttonToggleThumbnails, "");
			SetIconForHideThumbnails();
			m_worldTypesGroup = new MyGuiControlRadioButtonGroup();
			m_worldTypesGroup.SelectedChanged += WorldSelectionChanged;
			m_worldTypesGroup.MouseDoubleClick += WorldDoubleClick;
			m_worldList = new MyGuiControlList
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = position,
				Size = new Vector2(MyGuiConstants.LISTBOX_WIDTH, m_size.Value.Y - num - 0.048f)
			};
			Controls.Add(m_worldList);
			m_searchBox = new MyGuiControlSearchBox(new Vector2(-0.382f, -0.22f), new Vector2(m_worldList.Size.X, 0.032f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_searchBox.OnTextChanged += OnSearchTextChange;
			Controls.Add(m_searchBox);
		}

		private void OnButton_GroupSelection(MyGuiControlButton button)
		{
			m_groupSelection++;
			if (m_groupSelection > MyGameService.WorkshopService.GetAggregates().Count)
			{
				m_groupSelection = 0;
			}
			UpdateGroupSelection();
		}

		private void UpdateGroupSelection()
		{
			SetIconForGroupSelection();
			ClearItems();
			FillItems();
			TrySelectFirstItem();
		}

		private void SetIconForGroupSelection()
		{
			if (m_groupSelection == 0)
			{
				m_iconGroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\WNG_Service_Mixed.png");
			}
			else
			{
				m_iconGroupSelection.SetTexture("Textures\\GUI\\Icons\\Blueprints\\BP_" + MyGameService.WorkshopService.GetAggregates()[m_groupSelection - 1].ServiceName + ".png");
			}
		}

		private MyGuiControlButton CreateToolbarButton(int index, MyStringId tooltip, Action<MyGuiControlButton> onClick)
		{
			return CreateToolbarButton(index, MyTexts.Get(tooltip).ToString(), onClick);
		}

		private MyGuiControlButton CreateToolbarButton(int index, string tooltip, Action<MyGuiControlButton> onClick)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(new Vector2(-0.366f, -0.261f) + ((index > 0) ? (new Vector2(m_buttonRefresh.Size.X, 0f) * index) : Vector2.Zero), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, onClick)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
				VisualStyle = MyGuiControlButtonStyleEnum.Rectangular,
				ShowTooltipWhenDisabled = true,
				Size = new Vector2(0.029f, 0.03358333f)
			};
			myGuiControlButton.SetToolTip(tooltip);
			Controls.Add(myGuiControlButton);
			return myGuiControlButton;
		}

		private MyGuiControlImage CreateToolbarButtonIcon(MyGuiControlButton button, string texture)
		{
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(null, null, null, null, new string[1] { texture });
			AdjustButtonForIcon(button, myGuiControlImage);
			float num = 0.95f * Math.Min(button.Size.X, button.Size.Y);
			myGuiControlImage.Size = new Vector2(num * 0.75f, num);
			myGuiControlImage.Position = button.Position + new Vector2(-0.0016f, 0.018f);
			Controls.Add(myGuiControlImage);
			return myGuiControlImage;
		}

		private MyGuiControlButton CreateRateButton(bool positive)
		{
			return new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, onButtonClick: positive ? new Action<MyGuiControlButton>(OnRateUpClicked) : new Action<MyGuiControlButton>(OnRateDownClicked), size: new Vector2(0.03f));
		}

		private MyGuiControlImage CreateRateIcon(MyGuiControlButton button, string texture)
		{
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(null, null, null, null, new string[1] { texture });
			AdjustButtonForIcon(button, myGuiControlImage);
			myGuiControlImage.Size = button.Size * 0.6f;
			return myGuiControlImage;
		}

		private void AdjustButtonForIcon(MyGuiControlButton button, MyGuiControlImage icon)
		{
			button.Size = new Vector2(button.Size.X, button.Size.X * 4f / 3f);
			button.HighlightChanged += delegate(MyGuiControlBase x)
			{
				icon.ColorMask = (x.HasHighlight ? MyGuiConstants.HIGHLIGHT_TEXT_COLOR : Vector4.One);
			};
		}

		private void SetIconForSorting()
		{
			switch (m_sort)
			{
			case MyGuiBlueprintScreen_Reworked.SortOption.None:
				m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\NoSorting.png");
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
				m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\Alphabetical.png");
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
				m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ByCreationDate.png");
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
				m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\ByUpdateDate.png");
				break;
			default:
				m_iconSorting.SetTexture("Textures\\GUI\\Icons\\Blueprints\\NoSorting.png");
				break;
			}
		}

		private void SetIconForHideThumbnails()
		{
			m_iconToggleThumbnails.SetTexture(m_showThumbnails ? "Textures\\GUI\\Icons\\Blueprints\\ThumbnailsON.png" : "Textures\\GUI\\Icons\\Blueprints\\ThumbnailsOFF.png");
		}

		private void WorldSelectionChanged(MyGuiControlRadioButtonGroup args)
		{
			MyGuiControlContentButton myGuiControlContentButton = args.SelectedButton as MyGuiControlContentButton;
			if (myGuiControlContentButton == null || myGuiControlContentButton.UserData == null)
			{
				return;
			}
			MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem> myTuple = (MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>)myGuiControlContentButton.UserData;
			lock (m_selectedWorldLock)
			{
				m_selectedWorld = myTuple.Item1;
				m_selectedWorkshopItem = myTuple.Item2;
			}
			if (m_selectedButton != null)
			{
				m_selectedButton.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
			}
			m_selectedButton = myGuiControlContentButton;
			m_selectedButton.HighlightType = MyGuiControlHighlightType.CUSTOM;
			m_selectedButton.HasHighlight = true;
			string text = null;
			StringBuilder stringBuilder = null;
			MyLocalizationContext myLocalizationContext = MyLocalization.Static[m_selectedWorld.SessionName];
			if (myLocalizationContext != null)
			{
				StringBuilder stringBuilder2 = myLocalizationContext["Name"];
				if (stringBuilder2 != null)
				{
					text = stringBuilder2.ToString();
				}
				stringBuilder = myLocalizationContext["Description"];
			}
			if (string.IsNullOrEmpty(text))
			{
				text = m_selectedWorld.SessionName;
			}
			if (stringBuilder == null)
			{
				stringBuilder = new StringBuilder(m_selectedWorld.Description);
			}
			m_nameText.IsAutoEllipsisEnabled = true;
			m_nameText.SetMaxWidth(0.31f);
			m_nameText.SetToolTip(text);
			m_nameLabel.SetToolTip(text);
			m_nameText.Text = text;
			m_nameText.DoEllipsisAndScaleAdjust(RecalculateSize: false, float.PositiveInfinity, resetEllipsis: true);
			m_ratingDisplay.Value = (int)Math.Round(m_selectedWorkshopItem.Score * 10f);
			int myRating = m_selectedWorkshopItem.MyRating;
			m_buttonRateUp.Checked = myRating == 1;
			m_buttonRateDown.Checked = myRating == -1;
			m_descriptionMultilineText.Text = stringBuilder;
			m_descriptionMultilineText.SetScrollbarPageV(0f);
			m_descriptionMultilineText.SetScrollbarPageH(0f);
			WorldDataLoaderInfo worldDataLoaderInfo = new WorldDataLoaderInfo
			{
				World = myTuple.Item1,
				WorkshopItem = myTuple.Item2
			};
			WorldDataLoaded(worldDataLoaderInfo);
			m_asyncLoadingWheel.Visible = true;
			Parallel.Start(LoadWorldData, WorldDataLoaded, worldDataLoaderInfo);
		}

		private void LoadWorldData(WorkData data)
		{
			WorldDataLoaderInfo worldDataLoaderInfo = (WorldDataLoaderInfo)data;
			lock (m_selectedWorldLock)
			{
				if (worldDataLoaderInfo.WorkshopItem != m_selectedWorkshopItem || worldDataLoaderInfo.World != m_selectedWorld)
				{
					worldDataLoaderInfo.IsSkipped = true;
					return;
				}
			}
			string path = Path.Combine(worldDataLoaderInfo.WorkshopItem.Folder + "\\Sandbox.sbc");
			if (!MyFileSystem.FileExists(path))
			{
				return;
			}
			if (MyObjectBuilderSerializer.DeserializeXML(path, out MyObjectBuilder_Checkpoint checkpoint))
			{
				MyObjectBuilder_Identity myObjectBuilder_Identity = checkpoint.Identities.Find((MyObjectBuilder_Identity x) => x.CharacterEntityId == checkpoint.ControlledObject);
				if (myObjectBuilder_Identity != null)
				{
					worldDataLoaderInfo.Author = myObjectBuilder_Identity.DisplayName;
				}
				worldDataLoaderInfo.IsMultiplayer = checkpoint.OnlineMode != MyOnlineModeEnum.OFFLINE;
			}
		}

		private void WorldDataLoaded(WorkData data)
		{
			WorldDataLoaderInfo worldDataLoaderInfo = (WorldDataLoaderInfo)data;
			if (worldDataLoaderInfo.IsSkipped)
			{
				return;
			}
			lock (m_selectedWorldLock)
			{
				if (worldDataLoaderInfo.WorkshopItem != m_selectedWorkshopItem || worldDataLoaderInfo.World != m_selectedWorld)
				{
					return;
				}
			}
			if (worldDataLoaderInfo.IsMultiplayer)
			{
				m_onlineMode.Enabled = true;
			}
			else
			{
				m_onlineMode.Enabled = false;
				m_onlineMode.SelectItemByIndex(0);
			}
			m_authorText.Text = worldDataLoaderInfo.Author;
			m_maxPlayersSlider.Enabled = m_onlineMode.Enabled && m_onlineMode.GetSelectedIndex() > 0 && m_maxPlayers > 2;
			m_asyncLoadingWheel.Visible = false;
		}

		private void InitRightSide()
		{
			int num = 5;
			Vector2 topLeft = -m_size.Value / 2f + new Vector2(MARGIN_LEFT_LIST + m_worldList.Size.X + MARGIN_LEFT_INFO + 0.012f, MARGIN_TOP - 0.011f);
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
			m_ratingDisplay = new MyGuiControlRating(8)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			m_buttonRateUp = CreateRateButton(positive: true);
			m_iconRateUp = CreateRateIcon(m_buttonRateUp, "Textures\\GUI\\Icons\\Blueprints\\like_test.png");
			m_buttonRateDown = CreateRateButton(positive: false);
			m_iconRateDown = CreateRateIcon(m_buttonRateDown, "Textures\\GUI\\Icons\\Blueprints\\dislike_test.png");
			m_onlineModeLabel = new MyGuiControlLabel
			{
				Text = MyTexts.GetString(MyCommonTexts.WorldSettings_OnlineMode),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
			};
			m_onlineMode = new MyGuiControlCombobox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			m_onlineMode.AddItem(0L, MyCommonTexts.WorldSettings_OnlineModeOffline);
			m_onlineMode.AddItem(3L, MyCommonTexts.WorldSettings_OnlineModePrivate);
			m_onlineMode.AddItem(2L, MyCommonTexts.WorldSettings_OnlineModeFriends);
			m_onlineMode.AddItem(1L, MyCommonTexts.WorldSettings_OnlineModePublic);
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
			m_tableLayout.Add(m_buttonRateUp, MyAlignH.Right, MyAlignV.Center, 4, 1);
			m_tableLayout.Add(m_iconRateUp, MyAlignH.Center, MyAlignV.Center, 4, 1);
			m_tableLayout.Add(m_buttonRateDown, MyAlignH.Right, MyAlignV.Center, 4, 1);
			m_tableLayout.Add(m_iconRateDown, MyAlignH.Center, MyAlignV.Center, 4, 1);
			m_nameText.PositionX -= 0.001f;
			m_nameText.Size += new Vector2(0.002f, 0f);
			m_onlineMode.PositionX -= 0.002f;
			m_onlineMode.PositionY -= 0.005f;
			m_maxPlayersSlider.PositionX -= 0.003f;
			m_buttonRateUp.PositionX -= 0.05f;
			m_buttonRateDown.PositionX -= 0.007f;
			m_iconRateUp.Position = m_buttonRateUp.Position + new Vector2(-0.0015f, -0.002f) - new Vector2(m_buttonRateUp.Size.X / 2f, 0f);
			m_iconRateDown.Position = m_buttonRateDown.Position + new Vector2(-0.0015f, -0.002f) - new Vector2(m_buttonRateDown.Size.X / 2f, 0f);
			m_tableLayout.AddWithSize(m_descriptionPanel, MyAlignH.Left, MyAlignV.Top, 5, 0, 1, 2);
			m_tableLayout.AddWithSize(m_descriptionMultilineText, MyAlignH.Left, MyAlignV.Top, 5, 0, 1, 2);
			m_descriptionMultilineText.PositionY += 0.012f;
			float num6 = 0.01f;
			m_descriptionPanel.Position = new Vector2(m_descriptionPanel.PositionX - num6, m_descriptionPanel.PositionY - num6 + 0.012f);
			m_descriptionPanel.Size = new Vector2(m_descriptionPanel.Size.X, m_descriptionPanel.Size.Y + MyGuiConstants.BACK_BUTTON_SIZE.Y + 0.015f);
			m_descriptionMultilineText.Size = new Vector2(m_descriptionMultilineText.Size.X, m_descriptionMultilineText.Size.Y + MyGuiConstants.BACK_BUTTON_SIZE.Y);
			Vector2 value2 = m_size.Value / 2f;
			value2.X -= MARGIN_RIGHT + 0.004f;
			value2.Y -= MARGIN_BOTTOM + 0.004f;
			Vector2 bACK_BUTTON_SIZE = MyGuiConstants.BACK_BUTTON_SIZE;
			_ = MyGuiConstants.GENERIC_BUTTON_SPACING;
			_ = MyGuiConstants.GENERIC_BUTTON_SPACING;
			m_okButton = new MyGuiControlButton(value2, MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Start), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnOkButtonClicked);
			m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_Start));
			m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			Controls.Add(m_okButton);
			m_workshopButton = new MyGuiControlButton(m_okButton.Position - new Vector2(m_okButton.Size.X + 0.01f, 0f), MyGuiControlButtonStyleEnum.Default, bACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.ScreenLoadSubscribedWorldOpenInWorkshop), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnOpenInWorkshopClicked);
			m_workshopButton.SetToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorkshopOpenInWorkshop), MyGameService.Service.ServiceName));
			m_workshopButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			Controls.Add(m_workshopButton);
			base.CloseButtonEnabled = true;
			m_noSubscribedItemsPanel = new MyGuiControlCompositePanel
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			m_tableLayout.AddWithSize(m_noSubscribedItemsPanel, MyAlignH.Left, MyAlignV.Top, 0, 0, 6, 2);
			m_noSubscribedItemsPanel.Position = new Vector2(m_descriptionPanel.Position.X, m_worldList.Position.Y);
			m_noSubscribedItemsPanel.Size = new Vector2(m_descriptionPanel.Size.X, m_worldList.Size.Y - 1.63f * MyGuiConstants.BACK_BUTTON_SIZE.Y);
			m_noSubscribedItemsText = new MyGuiControlMultilineText(null, null, null, "Blue", 684f / 925f)
			{
				Size = new Vector2(m_descriptionMultilineText.Size.X, m_descriptionMultilineText.Size.Y * 2f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			m_tableLayout.AddWithSize(m_noSubscribedItemsText, MyAlignH.Left, MyAlignV.Top, 0, 0, 6, 2);
			m_noSubscribedItemsText.Position = m_noSubscribedItemsPanel.Position + new Vector2(num6);
			m_noSubscribedItemsText.Size = m_noSubscribedItemsPanel.Size - new Vector2(2f * num6);
			m_noSubscribedItemsText.Clear();
			m_noSubscribedItemsText.AppendText(MyTexts.GetString(MySpaceTexts.ToolTipNewGame_NoWorkshopWorld), "Blue", m_noSubscribedItemsText.TextScale, Vector4.One);
			m_noSubscribedItemsText.ScrollbarOffsetV = 1f;
			m_noSubscribedItemsPanel.Visible = false;
			m_noSubscribedItemsText.Visible = false;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(m_nameLabel.Position.X, m_okButton.Position.Y - bACK_BUTTON_SIZE.Y / 2f));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.GamepadHelpTextId = MySpaceTexts.NewGameWorkshop_Help_Screen;
		}

		private void OnLinkClicked(MyGuiControlBase sender, string url)
		{
			MyGuiSandbox.OpenUrlWithFallback(url, "Space Engineers Steam Workshop");
		}

		private void OnOpenInWorkshopClicked(MyGuiControlButton button)
		{
			if (m_selectedWorld != null)
			{
				MyGuiSandbox.OpenUrlWithFallback(m_selectedWorkshopItem.GetItemUrl(), m_selectedWorkshopItem.ServiceName + " Workshop");
			}
		}

		private void OnRateUpClicked(MyGuiControlButton button)
		{
			UpdateRateState(positive: true);
		}

		private void OnRateDownClicked(MyGuiControlButton button)
		{
			UpdateRateState(positive: false);
		}

		private void UpdateRateState(bool positive)
		{
			if (m_selectedWorkshopItem != null)
			{
				m_selectedWorkshopItem.Rate(positive);
				m_selectedWorkshopItem.ChangeRatingValue(positive);
				m_buttonRateUp.Checked = positive;
				m_buttonRateDown.Checked = !positive;
			}
		}

		private void m_onlineMode_ItemSelected()
		{
			m_maxPlayersSlider.Enabled = m_onlineMode.Enabled && m_onlineMode.GetSelectedIndex() > 0 && m_maxPlayers > 2;
		}

		private void OnRefreshClicked(MyGuiControlButton button)
		{
			ClearItems();
			RefreshList();
		}

		private void OnSortingClicked(MyGuiControlButton button)
		{
			switch (m_sort)
			{
			case MyGuiBlueprintScreen_Reworked.SortOption.None:
				m_sort = MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical;
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
				m_sort = MyGuiBlueprintScreen_Reworked.SortOption.CreationDate;
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
				m_sort = MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate;
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
				m_sort = MyGuiBlueprintScreen_Reworked.SortOption.None;
				break;
			}
			SetIconForSorting();
			ClearItems();
			OnSuccess();
		}

		private void OnOpenWorkshopClicked(MyGuiControlButton button)
		{
			MyWorkshop.OpenWorkshopBrowser(MySteamConstants.TAG_WORLDS);
		}

		private void OnToggleThumbnailsClicked(MyGuiControlButton button)
		{
			m_showThumbnails = !m_showThumbnails;
			SetIconForHideThumbnails();
			foreach (MyGuiControlBase control in m_worldList.Controls)
			{
				(control as MyGuiControlContentButton)?.SetPreviewVisibility(m_showThumbnails);
			}
			m_worldList.Recalculate();
		}

		private void OnSearchTextChange(string message)
		{
			ApplyFiltering();
			TrySelectFirstItem();
		}

		private void OnOkButtonClicked(MyGuiControlButton myGuiControlButton)
		{
			StartSelectedWorld();
		}

		private void WorldDoubleClick(MyGuiControlRadioButton obj)
		{
			StartSelectedWorld();
		}

		private void StartSelectedWorld()
		{
			if (m_selectedWorld == null || m_worldTypesGroup.SelectedButton == null || m_worldTypesGroup.SelectedButton.UserData == null)
<<<<<<< HEAD
			{
				return;
			}
			MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem> world = (MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>)m_worldTypesGroup.SelectedButton.UserData;
			if (m_onlineMode.GetSelectedIndex() != 0)
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
								DownloadSelectedWorld(world);
								break;
							case PermissionResult.Error:
								MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, default(MyStringId), MyMessageBoxStyleEnum.Info);
								break;
							}
						});
						break;
					case PermissionResult.Error:
						MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, default(MyStringId), MyMessageBoxStyleEnum.Info);
						break;
					}
				});
			}
			else
			{
=======
			{
				return;
			}
			MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem> world = (MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>)m_worldTypesGroup.SelectedButton.UserData;
			if (m_onlineMode.GetSelectedIndex() != 0)
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
								DownloadSelectedWorld(world);
								break;
							case PermissionResult.Error:
								MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, default(MyStringId), MyMessageBoxStyleEnum.Info);
								break;
							}
						});
						break;
					case PermissionResult.Error:
						MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, default(MyStringId), MyMessageBoxStyleEnum.Info);
						break;
					}
				});
			}
			else
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				DownloadSelectedWorld(world);
			}
		}

		private void DownloadSelectedWorld(MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem> world)
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, beginActionLoadSaves, endActionLoadSaves, world));
		}

		private IMyAsyncResult beginActionLoadSaves()
		{
			return new MyLoadWorldInfoListResult();
		}

		private void endActionLoadSaves(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			screen.CloseScreen();
			MyWorkshopItem world = ((MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>)screen.UserData).Item2;
			string sessionUniqueName = MyUtils.StripInvalidChars(world.Title);
			bool flag = false;
			string sessionSavesPath = MyLocalCache.GetSessionSavesPath(sessionUniqueName, contentFolder: false, createIfNotExists: false);
			if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
			{
				List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(MyCloudHelper.LocalToCloudWorldPath(sessionSavesPath));
				flag = cloudFiles != null && cloudFiles.Count > 0;
			}
			else
			{
				flag = Directory.Exists(sessionSavesPath);
<<<<<<< HEAD
			}
			if (flag)
			{
				OverwriteWorldDialog(world);
				return;
			}
=======
			}
			if (flag)
			{
				OverwriteWorldDialog(world);
				return;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyWorkshop.CreateWorldInstanceAsync(world, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo(), overwrite: false, delegate(bool success, string sessionPath)
			{
				if (success)
				{
					OnSuccessStart(sessionPath, world);
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded)));
				}
			});
		}

		private void OverwriteWorldDialog(MyWorkshopItem world)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextWorldExistsDownloadOverwrite), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum callbackReturn)
			{
				if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					MyWorkshop.CreateWorldInstanceAsync(world, MyWorkshop.MyWorkshopPathInfo.CreateWorldInfo(), overwrite: true, delegate(bool success, string sessionPath)
					{
						if (success)
						{
							OnSuccessStart(sessionPath, world);
						}
						else
						{
							MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded)));
						}
					});
				}
			}));
		}

		private void OnSuccessStart(string sessionPath, MyWorkshopItem workshopItem)
		{
			MySessionLoader.LoadSingleplayerSession(sessionPath, delegate
			{
				MySessionLoader.LastLoadedSessionWorkshopItem = workshopItem;
				MySession.Static.OnReady += delegate
				{
					MySessionLoader.LastLoadedSessionWorkshopItem = null;
				};
				MySession.Static.CurrentPath = sessionPath;
				MyAsyncSaving.DelayedSaveAfterLoad(sessionPath);
			});
		}

		private void OnCancelButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CloseScreen();
		}

		private void AddWorldButton(MyObjectBuilder_Checkpoint world, MyWorkshopItem workshopItem)
		{
			string text = world.SessionName;
			MyLocalizationContext myLocalizationContext = MyLocalization.Static[world.SessionName];
			if (myLocalizationContext != null)
			{
				StringBuilder stringBuilder = myLocalizationContext["Name"];
				if (stringBuilder != null)
				{
					text = stringBuilder.ToString();
				}
			}
			MyGuiControlContentButton myGuiControlContentButton = new MyGuiControlContentButton(text, GetImagePath(workshopItem))
			{
				UserData = new MyTuple<MyObjectBuilder_Checkpoint, MyWorkshopItem>(world, workshopItem),
				Key = m_worldTypesGroup.Count
			};
			myGuiControlContentButton.FocusHighlightAlsoSelects = true;
			myGuiControlContentButton.SetModType(MyBlueprintTypeEnum.WORKSHOP, workshopItem.ServiceName);
			myGuiControlContentButton.SetPreviewVisibility(m_showThumbnails);
			myGuiControlContentButton.SetTooltip(text);
			m_worldTypesGroup.Add(myGuiControlContentButton);
			m_worldList.Controls.Add(myGuiControlContentButton);
		}

		private string GetImagePath(MyWorkshopItem workshopItem)
		{
			return Path.Combine(workshopItem.Folder + "\\" + MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);
		}

		private void SortItems(List<MyWorkshopItem> list)
		{
			MyWorkshopItemComparer myWorkshopItemComparer = null;
			switch (m_sort)
			{
			case MyGuiBlueprintScreen_Reworked.SortOption.Alphabetical:
				myWorkshopItemComparer = new MyWorkshopItemComparer((MyWorkshopItem x, MyWorkshopItem y) => x.Title.CompareTo(y.Title));
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.CreationDate:
				myWorkshopItemComparer = new MyWorkshopItemComparer((MyWorkshopItem x, MyWorkshopItem y) => x.TimeCreated.CompareTo(y.TimeCreated));
				break;
			case MyGuiBlueprintScreen_Reworked.SortOption.UpdateDate:
				myWorkshopItemComparer = new MyWorkshopItemComparer((MyWorkshopItem x, MyWorkshopItem y) => x.TimeUpdated.CompareTo(y.TimeUpdated));
				break;
			}
			if (myWorkshopItemComparer != null)
			{
				list.Sort(myWorkshopItemComparer);
			}
		}

		private void ApplyFiltering()
		{
			bool flag = false;
			string[] array = new string[0];
			if (m_searchBox != null)
			{
				flag = m_searchBox.SearchText != "";
				array = m_searchBox.SearchText.Split(new char[1] { ' ' });
			}
			foreach (MyGuiControlBase control in m_worldList.Controls)
			{
				MyGuiControlContentButton myGuiControlContentButton = control as MyGuiControlContentButton;
				if (myGuiControlContentButton == null)
<<<<<<< HEAD
				{
					continue;
				}
				bool visible = true;
				if (flag)
				{
=======
				{
					continue;
				}
				bool visible = true;
				if (flag)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					string text = myGuiControlContentButton.Title.ToLower();
					string[] array2 = array;
					foreach (string text2 in array2)
					{
						if (!text.Contains(text2.ToLower()))
						{
							visible = false;
							break;
						}
					}
				}
				control.Visible = visible;
			}
			m_worldList.SetScrollBarPage();
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
				Size = new Vector2(m_worldList.Size.X, myGuiControlLabel.Size.Y),
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
			m_worldList.Controls.Add(myGuiControlParent);
		}

		private IMyAsyncResult beginAction()
		{
			return new LoadListResult();
		}

		private void endAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			LoadListResult loadListResult = (LoadListResult)result;
			SubscribedWorlds = loadListResult.SubscribedWorlds;
			UpdateWorkshopError(loadListResult.Result.Item1, loadListResult.Result.Item2);
			OnSuccess();
			screen.CloseScreen();
			bool flag = SubscribedWorlds != null && SubscribedWorlds.Count != 0;
			m_noSubscribedItemsPanel.Visible = !flag;
			m_noSubscribedItemsText.Visible = !flag;
			m_nameLabel.Visible = flag;
			m_nameText.Visible = flag;
			m_authorLabel.Visible = flag;
			m_authorText.Visible = flag;
			m_ratingDisplay.Visible = flag;
			m_ratingLabel.Visible = flag;
			m_buttonRateUp.Visible = flag;
			m_buttonRateDown.Visible = flag;
			m_iconRateUp.Visible = flag;
			m_iconRateDown.Visible = flag;
			m_descriptionMultilineText.Visible = flag;
			m_onlineModeLabel.Visible = flag;
			m_onlineMode.Visible = flag;
			m_maxPlayersLabel.Visible = flag;
			m_maxPlayersSlider.Visible = flag;
			m_descriptionPanel.Visible = flag;
		}

		private void PreloadItemTextures()
		{
			List<string> list = new List<string>();
			foreach (MyWorkshopItem subscribedWorld in SubscribedWorlds)
			{
				string imagePath = GetImagePath(subscribedWorld);
				if (MyFileSystem.FileExists(imagePath))
				{
					list.Add(imagePath);
				}
			}
			MyRenderProxy.PreloadTextures(list, TextureType.GUIWithoutPremultiplyAlpha);
		}

		private void ClearItems()
		{
			m_selectedWorld = null;
			m_worldList.Clear();
			m_worldTypesGroup.Clear();
		}

		private void FillItems()
		{
<<<<<<< HEAD
			if (SubscribedWorlds == null)
			{
				MyLog.Default.Warning("Can not load Subscribed Worlds because Steam is not running.");
				return;
			}
			foreach (MyWorkshopItem subscribedWorld in SubscribedWorlds)
			{
=======
			foreach (MyWorkshopItem subscribedWorld in SubscribedWorlds)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (m_groupSelection <= 0 || !(subscribedWorld.ServiceName != MyGameService.WorkshopService.GetAggregates()[m_groupSelection - 1].ServiceName))
				{
					MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = new MyObjectBuilder_Checkpoint();
					myObjectBuilder_Checkpoint.SessionName = subscribedWorld.Title;
					myObjectBuilder_Checkpoint.Description = subscribedWorld.Description;
					myObjectBuilder_Checkpoint.WorkshopId = subscribedWorld.Id;
					myObjectBuilder_Checkpoint.WorkshopServiceName = subscribedWorld.ServiceName;
					AddWorldButton(myObjectBuilder_Checkpoint, subscribedWorld);
				}
			}
		}

		private void TrySelectFirstItem()
		{
			if (!m_worldTypesGroup.SelectedIndex.HasValue && m_worldList.Controls.Count > 0)
			{
				m_worldTypesGroup.SelectByIndex(0);
			}
		}

		private void OnSuccess()
		{
			if (SubscribedWorlds != null)
			{
				SortItems(SubscribedWorlds);
				PreloadItemTextures();
				FillItems();
			}
			TrySelectFirstItem();
		}

		private void RefreshList()
		{
			ClearItems();
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, beginAction, endAction));
		}

		private void UpdateWorkshopError(MyGameServiceCallResult result, string serviceName)
		{
			if (result != MyGameServiceCallResult.OK)
			{
				string workshopErrorText = MyWorkshop.GetWorkshopErrorText(result, serviceName, m_workshopPermitted);
				SetWorkshopErrorText(workshopErrorText);
			}
			else
			{
				SetWorkshopErrorText("", visible: false);
			}
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (hasFocus)
			{
				m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_workshopButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			return result;
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

		public override void OnScreenOrderChanged(MyGuiScreenBase oldLast, MyGuiScreenBase newLast)
		{
			base.OnScreenOrderChanged(oldLast, newLast);
			CheckUGCServices();
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
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
				{
					OnRefreshClicked(null);
				}
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.VIEW))
				{
					OnOpenInWorkshopClicked(null);
				}
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU))
				{
					OnOpenWorkshopClicked(null);
				}
			}
		}
	}
}
