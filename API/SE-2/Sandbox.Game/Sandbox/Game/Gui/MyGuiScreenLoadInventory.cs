using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Collections.ObjectModel;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Linq;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;
using VRageRender.Utils;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenLoadInventory : MyGuiScreenBase
	{
		private enum InventoryItemAction
		{
			Apply,
			Sell,
			Trade,
			Recycle,
			Delete,
			Buy
		}

		private enum TabState
		{
			Character,
			Tools
		}

		private enum LowerTabState
		{
			Coloring,
			Recycling
		}

		private struct CategoryButton
		{
			public MyStringId Tooltip;

			public MyGameInventoryItemSlot Slot;

			public string ImageName;

			public string ButtonText;

			public CategoryButton(MyStringId tooltip, MyGameInventoryItemSlot slot, string imageName = null, string buttonText = null)
			{
				Tooltip = tooltip;
				Slot = slot;
				ImageName = imageName;
				ButtonText = buttonText;
			}
		}

		public static readonly string IMAGE_PREFIX = "IN_";
<<<<<<< HEAD
=======

		private static readonly bool SKIN_STORE_FEATURES_ENABLED = false;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private static readonly bool SKIN_STORE_FEATURES_ENABLED = false;

		private readonly string m_equipCheckbox = "equipCheckbox";

		private Vector2 m_itemsTableSize;

		private MyGuiControlButton m_openStoreButton;

		private MyGuiControlButton m_refreshButton;

		private MyGuiControlButton m_characterButton;

		private MyGuiControlButton m_toolsButton;

		private MyGuiControlButton m_recyclingButton;

		private bool m_inGame;

		private TabState m_activeTabState;

		private LowerTabState m_activeLowTabState;

		private string m_rotatingWheelTexture;

		private MyGuiControlRotatingWheel m_wheel;

		private MyEntityRemoteController m_entityController;

		private List<MyGuiControlCheckbox> m_itemCheckboxes;

		private bool m_itemCheckActive;

		private MyGuiControlCombobox m_modelPicker;

		private MyGuiControlSlider m_sliderHue;

		private MyGuiControlSlider m_sliderSaturation;

		private MyGuiControlSlider m_sliderValue;

		private string m_selectedModel;

		private Vector3 m_selectedHSV;

		private Dictionary<string, int> m_displayModels;

		private Dictionary<int, string> m_models;

		private string m_storedModel;

		private Vector3 m_storedHSV;

		private bool m_colorOrModelChanged;

		private MyGameInventoryItemSlot m_filteredSlot;

		private MyGuiControlContextMenu m_contextMenu;

		private MyGuiControlImageButton m_contextMenuLastButton;

		private bool m_hideDuplicatesEnabled;

		private bool m_showOnlyDuplicatesEnabled;

		private MyGuiControlParent m_itemsTableParent;

		private List<MyGameInventoryItem> m_userItems;

		private List<MyPhysicalInventoryItem> m_allTools = new List<MyPhysicalInventoryItem>();

		private MyGuiControlCombobox m_toolPicker;

		private string m_selectedTool;

		private MyGuiControlButton m_OkButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlButton m_craftButton;

		private MyGuiControlCombobox m_rarityPicker;

		private MyGameInventoryItem m_lastCraftedItem;

		private MyGuiControlButton m_coloringButton;

		private bool m_audioSet;

		private bool? m_savedStateAnselEnabled;

		private List<CategoryButton> m_categoryButtonsData;

		private int m_currentCategoryIndex;

		private MyGuiControlStackPanel m_categoryButtonLayout;

		private MyGuiControlCheckbox m_duplicateCheckboxRecycle;

		private bool m_focusButton;

		private MyGuiControlWrapPanel m_itemsTable;

		private const int PAGE_COUNT = 2;

		public static event MyLookChangeDelegate LookChanged;

		public MyGuiScreenLoadInventory()
			: base(new Vector2(0.32f, 0.05f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.65f, 0.9f), isTopMostScreen: true, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = false;
		}

		public MyGuiScreenLoadInventory(bool inGame = false, HashSet<string> customCharacterNames = null)
			: base(new Vector2(0.32f, 0.05f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.65f, 0.9f), isTopMostScreen: true, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = false;
			Initialize(inGame, customCharacterNames);
		}

		public void Initialize(bool inGame, HashSet<string> customCharacterNames)
		{
			m_inGame = inGame;
			m_audioSet = inGame;
			m_rotatingWheelTexture = "Textures\\GUI\\screens\\screen_loading_wheel_loading_screen.dds";
			base.Align = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_filteredSlot = MyGameInventoryItemSlot.None;
			IsHitTestVisible = true;
			MyGameService.CheckItemDataReady += MyGameService_CheckItemDataReady;
			m_storedModel = ((MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.ModelName : string.Empty);
			InitModels(customCharacterNames);
			m_entityController = new MyEntityRemoteController(MySession.Static.LocalCharacter);
			m_entityController.LockRotationAxis(GlobalAxis.Y | GlobalAxis.Z);
			m_allTools = m_entityController.GetInventoryTools();
			RecreateControls(constructor: true);
			UpdateSliderTooltips();
			MyScreenManager.GetFirstScreenOfType<MyGuiScreenIntroVideo>()?.HideScreen();
			if (!inGame)
			{
				MyLocalCache.LoadInventoryConfig(MySession.Static.LocalCharacter);
			}
			EquipTool();
			UpdateCheckboxes();
			m_isTopMostScreen = false;
			switch (m_activeTabState)
			{
			case TabState.Character:
				m_currentCategoryIndex = -1;
				break;
			case TabState.Tools:
				m_currentCategoryIndex = 0;
				break;
			}
			MyScreenManager.GetFirstScreenOfType<MyGuiScreenGamePlay>()?.HideScreen();
		}

		private void InitModels(HashSet<string> customCharacterNames)
		{
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			m_displayModels = new Dictionary<string, int>();
			m_models = new Dictionary<int, string>();
			int value = 0;
			if (customCharacterNames == null)
			{
				foreach (MyCharacterDefinition character in MyDefinitionManager.Static.Characters)
				{
					if ((character.UsableByPlayer || (!MySession.Static.SurvivalMode && m_inGame && MySession.Static.IsRunningExperimental)) && character.Public)
					{
						string displayName = GetDisplayName(character.Name);
						m_displayModels[displayName] = value;
						m_models[value++] = character.Name;
					}
				}
				return;
			}
			DictionaryValuesReader<string, MyCharacterDefinition> characters = MyDefinitionManager.Static.Characters;
			Enumerator<string> enumerator2 = customCharacterNames.GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (characters.TryGetValue(customCharacterName, out var result) && (!MySession.Static.SurvivalMode || result.UsableByPlayer) && result.Public)
=======
				while (enumerator2.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					string current2 = enumerator2.get_Current();
					if (characters.TryGetValue(current2, out var result) && (!MySession.Static.SurvivalMode || result.UsableByPlayer) && result.Public)
					{
						string displayName2 = GetDisplayName(result.Name);
						m_displayModels[displayName2] = value;
						m_models[value++] = result.Name;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
		}

		private string GetDisplayName(string name)
		{
			return MyTexts.GetString(name);
		}

		private void SubscribeValueChangedEventsToSliders(bool subsribe)
		{
			if (subsribe)
			{
				if (m_sliderHue != null)
				{
					MyGuiControlSlider sliderHue = m_sliderHue;
					sliderHue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderHue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
				}
				if (m_sliderSaturation != null)
				{
					MyGuiControlSlider sliderSaturation = m_sliderSaturation;
					sliderSaturation.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderSaturation.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
				}
				if (m_sliderValue != null)
				{
					MyGuiControlSlider sliderValue = m_sliderValue;
					sliderValue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderValue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
				}
			}
			else
			{
				if (m_sliderHue != null)
				{
					MyGuiControlSlider sliderHue2 = m_sliderHue;
					sliderHue2.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderHue2.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
				}
				if (m_sliderSaturation != null)
				{
					MyGuiControlSlider sliderSaturation2 = m_sliderSaturation;
					sliderSaturation2.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderSaturation2.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
				}
				if (m_sliderValue != null)
				{
					MyGuiControlSlider sliderValue2 = m_sliderValue;
					sliderValue2.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderValue2.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// add horizontal line under caption
		/// </summary>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MyGuiControlSeparatorList Prepare_MyGuiControlSeparatorList()
		{
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.072f), 0.5385f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.147f), 0.5385f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.228f), 0.5385f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.548f), 0.5385f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.629f), 0.5385f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.778f), 0.5385f);
			return myGuiControlSeparatorList;
		}

		private MyGuiControlStackPanel Prepare_sideStackPanel()
		{
			return new MyGuiControlStackPanel
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_COMPOSITE_ROUND_ALL,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
		}

		private MyGuiControlStackPanel Prepare_lowTabPanel()
		{
			return new MyGuiControlStackPanel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Margin = new Thickness(0.016f, 0.009f, 0.015f, 0.015f),
				Orientation = MyGuiOrientation.Horizontal
			};
		}

		private MyGuiControlButton Prepare_coloringButton()
		{
			MyGuiControlButton myGuiControlButton = MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryColoring, MyCommonTexts.ScreenLoadInventoryColoringFilterTooltip, OnViewTabColoring);
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.ToolbarButton;
			myGuiControlButton.CanHaveFocus = false;
			myGuiControlButton.Margin = new Thickness(0.0415f, 0.0285f, 0.0025f, 0f);
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				myGuiControlButton.Checked = true;
				myGuiControlButton.Selected = true;
			}
			return myGuiControlButton;
		}

		private MyGuiControlStackPanel Prepare_tabsPanel()
		{
			return new MyGuiControlStackPanel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Margin = new Thickness(0.016f, 0f, 0.015f, 0.015f),
				Orientation = MyGuiOrientation.Horizontal
			};
		}

		private MyGuiControlScrollablePanel Prepare_scrollPanel()
		{
			return new MyGuiControlScrollablePanel(m_itemsTableParent)
			{
				CompleteScissor = true,
				ScrollBarOffset = new Vector2(0.005f, 0f),
				ScrollbarVEnabled = true,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Size = m_itemsTableSize,
				Position = m_itemsTable.Position
			};
		}

		private void Prepare_m_recyclingButton()
		{
			m_recyclingButton = MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryRecycling, MyCommonTexts.ScreenLoadInventoryRecyclingFilterTooltip, OnViewTabRecycling);
			m_recyclingButton.VisualStyle = MyGuiControlButtonStyleEnum.ToolbarButton;
			m_recyclingButton.CanHaveFocus = false;
			m_recyclingButton.Margin = new Thickness(0.0025f, 0.0285f, 0.0025f, 0f);
			if (m_activeLowTabState == LowerTabState.Recycling)
			{
				m_recyclingButton.Checked = true;
				m_recyclingButton.Selected = true;
			}
		}

		private void Prepare_m_characterButton()
		{
			m_characterButton = MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryCharacter, MyCommonTexts.ScreenLoadInventoryCharacterFilterTooltip, OnViewTabCharacter);
			m_characterButton.VisualStyle = MyGuiControlButtonStyleEnum.ToolbarButton;
			m_characterButton.CanHaveFocus = false;
			m_characterButton.Margin = new Thickness(0.0415f, 0.0285f, 0.0025f, 0f);
			m_categoryButtonsData = new List<CategoryButton>();
			if (m_activeTabState == TabState.Character)
			{
				base.FocusedControl = m_characterButton;
				m_characterButton.Checked = true;
				m_characterButton.Selected = true;
				m_categoryButtonsData.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryHelmetTooltip, MyGameInventoryItemSlot.Helmet, "Textures\\GUI\\Icons\\Skins\\Categories\\helmet.png", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryHelmet)));
				m_categoryButtonsData.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventorySuitTooltip, MyGameInventoryItemSlot.Suit, "Textures\\GUI\\Icons\\Skins\\Categories\\suit.png", MyTexts.GetString(MyCommonTexts.ScreenLoadInventorySuit)));
				m_categoryButtonsData.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryGlovesTooltip, MyGameInventoryItemSlot.Gloves, "Textures\\GUI\\Icons\\Skins\\Categories\\glove.png", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryGloves)));
				m_categoryButtonsData.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryBootsTooltip, MyGameInventoryItemSlot.Boots, "Textures\\GUI\\Icons\\Skins\\Categories\\boot.png", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryBoots)));
			}
		}

		private void Prepare_m_toolsButton_And_m_categoryButtonsData()
		{
			m_toolsButton = MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryTools, MyCommonTexts.ScreenLoadInventoryToolsFilterTooltip, OnViewTools);
			m_toolsButton.CanHaveFocus = false;
			m_toolsButton.VisualStyle = MyGuiControlButtonStyleEnum.ToolbarButton;
			m_toolsButton.Margin = new Thickness(0.0025f, 0.0285f, 0f, 0f);
			if (m_activeTabState == TabState.Tools)
			{
				base.FocusedControl = m_toolsButton;
				m_toolsButton.Checked = true;
				m_toolsButton.Selected = true;
				m_categoryButtonsData.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryWelderTooltip, MyGameInventoryItemSlot.Welder, "Textures\\GUI\\Icons\\WeaponWelder.dds", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryWelder)));
				m_categoryButtonsData.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryGrinderTooltip, MyGameInventoryItemSlot.Grinder, "Textures\\GUI\\Icons\\WeaponGrinder.dds", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryGrinder)));
				m_categoryButtonsData.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryDrillTooltip, MyGameInventoryItemSlot.Drill, "Textures\\GUI\\Icons\\WeaponDrill.dds", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryDrill)));
				m_categoryButtonsData.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryRifleTooltip, MyGameInventoryItemSlot.Rifle, "Textures\\GUI\\Icons\\WeaponAutomaticRifle.dds", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryRifle)));
			}
		}

		private void Prepare_m_categoryButtonLayout(MyGuiControlImageButton.StyleDefinition basicButtonStyle, Vector2 basicButtonSize)
		{
			m_categoryButtonLayout = new MyGuiControlStackPanel();
			m_categoryButtonLayout.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_categoryButtonLayout.Margin = new Thickness(0.055f, 0.05f, 0f, 0f);
			m_categoryButtonLayout.Orientation = MyGuiOrientation.Horizontal;
			foreach (CategoryButton categoryButtonsDatum in m_categoryButtonsData)
			{
				MyGuiControlImageButton myGuiControlImageButton = MakeImageButton(Vector2.Zero, basicButtonSize, basicButtonStyle, categoryButtonsDatum.Tooltip, OnCategoryClicked);
				myGuiControlImageButton.UserData = categoryButtonsDatum.Slot;
				myGuiControlImageButton.Margin = new Thickness(0f, 0f, 0.004f, 0f);
				m_categoryButtonLayout.Add(myGuiControlImageButton);
			}
		}

		private void Prepare_LowerTabState_Coloring_ModelPanel(MyGuiControlStackPanel modelPanel)
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.PlayerCharacterModel));
			myGuiControlLabel.Margin = new Thickness(0.045f, -0.03f, 0.005f, 0f);
			m_modelPicker = new MyGuiControlCombobox();
			m_modelPicker.Size = new Vector2(0.225f, 1f);
			m_modelPicker.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_modelPicker.Margin = new Thickness(0.005f, -0.03f, 0.005f, 0f);
			m_modelPicker.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipCharacterScreen_Model));
			foreach (KeyValuePair<string, int> displayModel in m_displayModels)
			{
				m_modelPicker.AddItem(displayModel.Value, new StringBuilder(displayModel.Key));
			}
			m_selectedModel = MySession.Static.LocalCharacter.ModelName;
			if (m_displayModels.ContainsKey(GetDisplayName(m_selectedModel)))
			{
				m_modelPicker.SelectItemByKey(m_displayModels[GetDisplayName(m_selectedModel)]);
			}
			else if (m_displayModels.Count > 0)
			{
<<<<<<< HEAD
				m_modelPicker.SelectItemByKey(m_displayModels.First().Value);
=======
				m_modelPicker.SelectItemByKey(Enumerable.First<KeyValuePair<string, int>>((IEnumerable<KeyValuePair<string, int>>)m_displayModels).Value);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_modelPicker.ItemSelected += OnItemSelected;
			if (m_activeTabState == TabState.Character || m_activeTabState == TabState.Tools)
			{
				modelPanel.Add(myGuiControlLabel);
				modelPanel.Add(m_modelPicker);
				Controls.Add(myGuiControlLabel);
				Controls.Add(m_modelPicker);
			}
		}

		private void TabState_Filter()
		{
			m_toolPicker = new MyGuiControlCombobox();
			m_toolPicker.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_toolPicker.Margin = new Thickness(0.015f, 0.01f, 0.01f, 0.01f);
			foreach (MyPhysicalInventoryItem allTool in m_allTools)
			{
				if (m_entityController.GetToolSlot(allTool.Content.SubtypeName) == m_filteredSlot)
				{
					MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(allTool.Content);
					if (physicalItemDefinition != null)
					{
						m_toolPicker.AddItem(allTool.ItemId, new StringBuilder(physicalItemDefinition.DisplayNameText));
					}
					else if (allTool.Content != null)
					{
						m_toolPicker.AddItem(allTool.ItemId, new StringBuilder(allTool.Content.SubtypeName));
					}
				}
			}
			if (string.IsNullOrEmpty(m_selectedTool))
			{
				if (m_toolPicker.GetItemsCount() > 0)
				{
					m_toolPicker.SelectItemByIndex(0);
					uint key = (uint)m_toolPicker.GetSelectedKey();
<<<<<<< HEAD
					MyPhysicalInventoryItem myPhysicalInventoryItem = m_allTools.FirstOrDefault((MyPhysicalInventoryItem t) => t.ItemId == key);
=======
					MyPhysicalInventoryItem myPhysicalInventoryItem = Enumerable.FirstOrDefault<MyPhysicalInventoryItem>((IEnumerable<MyPhysicalInventoryItem>)m_allTools, (Func<MyPhysicalInventoryItem, bool>)((MyPhysicalInventoryItem t) => t.ItemId == key));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (myPhysicalInventoryItem.Content != null)
					{
						m_selectedTool = myPhysicalInventoryItem.Content.SubtypeName;
					}
				}
			}
			else
			{
<<<<<<< HEAD
				MyPhysicalInventoryItem myPhysicalInventoryItem2 = m_allTools.FirstOrDefault((MyPhysicalInventoryItem t) => t.Content.SubtypeName == m_selectedTool);
=======
				MyPhysicalInventoryItem myPhysicalInventoryItem2 = Enumerable.FirstOrDefault<MyPhysicalInventoryItem>((IEnumerable<MyPhysicalInventoryItem>)m_allTools, (Func<MyPhysicalInventoryItem, bool>)((MyPhysicalInventoryItem t) => t.Content.SubtypeName == m_selectedTool));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_toolPicker.SelectItemByKey(myPhysicalInventoryItem2.ItemId);
			}
			m_toolPicker.ItemSelected += m_toolPicker_ItemSelected;
		}

		private void Prepare_LowerTabState_Coloring(MyGuiControlStackPanel modelPanel)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(null, null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			myGuiControlCheckbox.IsChecked = m_hideDuplicatesEnabled;
			myGuiControlCheckbox.IsCheckedChanged = OnHideDuplicates;
			myGuiControlCheckbox.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipCharacterScreen_HideDuplicates));
			myGuiControlCheckbox.Margin = new Thickness(0.005f, -0.03f, 0.005f, 0.01f);
			modelPanel.Add(myGuiControlCheckbox);
			Controls.Add(myGuiControlCheckbox);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryHideDuplicates));
			myGuiControlLabel.Margin = new Thickness(0f, -0.03f, 0.005f, 0.01f);
			modelPanel.Add(myGuiControlLabel);
			Controls.Add(myGuiControlLabel);
		}

		private void Prepare_LowerTabState_NotColoring(MyGuiControlStackPanel modelPanel)
		{
			m_duplicateCheckboxRecycle = new MyGuiControlCheckbox(null, null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_duplicateCheckboxRecycle.IsChecked = m_showOnlyDuplicatesEnabled;
			m_duplicateCheckboxRecycle.IsCheckedChanged = OnShowOnlyDuplicates;
			m_duplicateCheckboxRecycle.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipCharacterScreen_ShowOnlyDuplicates));
			m_duplicateCheckboxRecycle.Margin = new Thickness(0.039f, -0.03f, 0.01f, 0.01f);
			modelPanel.Add(m_duplicateCheckboxRecycle);
			Controls.Add(m_duplicateCheckboxRecycle);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryShowOnlyDuplicates));
			myGuiControlLabel.Margin = new Thickness(0.005f, -0.03f, 0.01f, 0.01f);
			modelPanel.Add(myGuiControlLabel);
			Controls.Add(myGuiControlLabel);
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryCurrencyCurrent), MyGameService.RecycleTokens), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: false, 0.1f, isAutoScaleEnabled: true);
			myGuiControlLabel2.Margin = new Thickness(0.19f, -0.05f, 0.01f, 0.01f);
			modelPanel.Add(myGuiControlLabel2);
			Controls.Add(myGuiControlLabel2);
		}

		private void Prepare_LowerTabState_Sliders(MyGuiControlStackPanel colorPanel)
		{
			m_sliderHue = new MyGuiControlSlider(null, 0f, 360f, 0.177f, null, null, null, 0, 0.8f, 0f, "White", string.Empty, MyGuiControlSliderStyleEnum.Hue, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, intValue: true);
			m_sliderHue.Margin = new Thickness(0.055f, -0.0425f, 0f, 0f);
			m_sliderHue.Enabled = m_activeTabState == TabState.Character;
			colorPanel.Add(m_sliderHue);
			Controls.Add(m_sliderHue);
			m_sliderSaturation = new MyGuiControlSlider(null, 0f, 1f, 0.177f, 0f, null, null, 1, 0.8f, 0f, "White", string.Empty, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_sliderSaturation.Margin = new Thickness(0.0052f, -0.0425f, 0f, 0f);
			m_sliderSaturation.Enabled = m_activeTabState == TabState.Character;
			colorPanel.Add(m_sliderSaturation);
			Controls.Add(m_sliderSaturation);
			m_sliderValue = new MyGuiControlSlider(null, 0f, 1f, 0.177f, 0f, null, null, 1, 0.8f, 0f, "White", string.Empty, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_sliderValue.Margin = new Thickness(0.006f, -0.0425f, 0f, 0f);
			m_sliderValue.Enabled = m_activeTabState == TabState.Character;
			colorPanel.Add(m_sliderValue);
			Controls.Add(m_sliderValue);
		}

		private void Prepare_categoryButton(MyGuiControlImageButton control, int i, MyGuiControlImageButton.StyleDefinition selectedButtonStyle)
		{
			Controls.Add(control);
			if (m_filteredSlot != 0 && m_filteredSlot == (MyGameInventoryItemSlot)control.UserData)
			{
				control.ApplyStyle(selectedButtonStyle);
				control.Checked = true;
				control.Selected = true;
			}
			control.CanHaveFocus = false;
			control.Size = new Vector2(0.1f, 0.1f);
			float num = control.Position.X;
			if (m_categoryButtonsData == null || m_categoryButtonsData.Count <= i)
			{
				return;
			}
			if (!string.IsNullOrEmpty(m_categoryButtonsData[i].ImageName))
			{
				MyGuiControlImage myGuiControlImage = new MyGuiControlImage(size: new Vector2(0.03f, 0.04f), position: control.Position + new Vector2(0.005f, 0.001f), backgroundColor: null, backgroundTexture: null, textures: new string[1] { m_categoryButtonsData[i].ImageName }, toolTip: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				Controls.Add(myGuiControlImage);
				num += myGuiControlImage.Size.X;
			}
			if (string.IsNullOrEmpty(m_categoryButtonsData[i].ButtonText))
			{
				return;
			}
			MyGuiControlLabel buttonLabel = new MyGuiControlLabel(size: control.Size, position: new Vector2((num + control.Position.X + control.Size.X) / 2f, control.Position.Y + control.Size.Y / 2.18f), text: m_categoryButtonsData[i].ButtonText, colorMask: null, textScale: 0.8f, font: "Blue", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: true, maxWidth: control.Size.X - 0.04f, isAutoScaleEnabled: true);
			Controls.Add(buttonLabel);
			control.HighlightChanged += delegate(MyGuiControlBase x)
			{
				if (x.HasHighlight)
				{
					buttonLabel.ColorMask = MyGuiConstants.HIGHLIGHT_TEXT_COLOR;
				}
				else
				{
					buttonLabel.ColorMask = Vector4.One;
				}
			};
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			SubscribeValueChangedEventsToSliders(subsribe: false);
			MyGuiControlImageButton.StyleDefinition basicButtonStyle = new MyGuiControlImageButton.StyleDefinition
			{
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_HIGHLIGHT
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_ACTIVE
				},
				Active = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_ACTIVE
				},
				Focus = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_FOCUS
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_NORMAL
				}
			};
			Vector2 basicButtonSize = new Vector2(0.14f, 0.05f);
			MyGuiControlImageButton.StyleDefinition selectedButtonStyle = new MyGuiControlImageButton.StyleDefinition
			{
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_HIGHLIGHT
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_ACTIVE
				},
				Active = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_ACTIVE
				},
				Focus = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_FOCUS
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_NORMAL
				}
			};
			Controls.Add(Prepare_MyGuiControlSeparatorList());
			MyGuiControlStackPanel myGuiControlStackPanel = Prepare_sideStackPanel();
			MyGuiControlStackPanel myGuiControlStackPanel2 = Prepare_lowTabPanel();
			MyGuiControlStackPanel myGuiControlStackPanel3 = Prepare_tabsPanel();
			m_coloringButton = Prepare_coloringButton();
			myGuiControlStackPanel2.Add(m_coloringButton);
			Controls.Add(m_coloringButton);
			if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
			{
				Prepare_m_recyclingButton();
				myGuiControlStackPanel2.Add(m_recyclingButton);
				Controls.Add(m_recyclingButton);
			}
			Prepare_m_characterButton();
			myGuiControlStackPanel3.Add(m_characterButton);
			Controls.Add(m_characterButton);
			Prepare_m_toolsButton_And_m_categoryButtonsData();
			myGuiControlStackPanel3.Add(m_toolsButton);
			Controls.Add(m_toolsButton);
			Prepare_m_categoryButtonLayout(basicButtonStyle, basicButtonSize);
			if (m_modelPicker != null)
			{
				m_modelPicker.ItemSelected -= OnItemSelected;
			}
			MyGuiControlStackPanel myGuiControlStackPanel4 = new MyGuiControlStackPanel();
			myGuiControlStackPanel4.Orientation = MyGuiOrientation.Horizontal;
			myGuiControlStackPanel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlStackPanel4.Margin = new Thickness(0.014f);
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				Prepare_LowerTabState_Coloring_ModelPanel(myGuiControlStackPanel4);
			}
			if (m_activeTabState == TabState.Tools && m_filteredSlot != 0)
			{
				TabState_Filter();
			}
			m_itemsTableSize = new Vector2(0.582f, 0.29f);
			m_itemsTableParent = new MyGuiControlParent(null, new Vector2(m_itemsTableSize.X, 0.1f));
			m_itemsTableParent.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_itemsTableParent.SkipForMouseTest = true;
			m_itemsTable = CreateItemsTable(m_itemsTableParent);
			myGuiControlStackPanel.Add(m_itemsTable);
			myGuiControlStackPanel.Add(myGuiControlStackPanel2);
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				Prepare_LowerTabState_Coloring(myGuiControlStackPanel4);
			}
			else
			{
				Prepare_LowerTabState_NotColoring(myGuiControlStackPanel4);
			}
			MyGuiControlStackPanel myGuiControlStackPanel5 = new MyGuiControlStackPanel();
			myGuiControlStackPanel5.Orientation = MyGuiOrientation.Horizontal;
			myGuiControlStackPanel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlStackPanel5.Margin = new Thickness(0.018f);
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				bool enabled = m_OkButton == null || (m_OkButton != null && m_OkButton.Enabled);
				m_OkButton = MakeButton(Vector2.Zero, MyCommonTexts.Ok, MyCommonTexts.ScreenLoadInventoryOkTooltip, OnOkClick);
				m_OkButton.Enabled = enabled;
				m_OkButton.Margin = new Thickness(0.0395f, -0.029f, 0.0075f, 0f);
				myGuiControlStackPanel5.Add(m_OkButton);
			}
			else
			{
				m_craftButton = MakeButton(Vector2.Zero, MyCommonTexts.CraftButton, MyCommonTexts.ScreenLoadInventoryCraftTooltip, OnCraftClick);
				m_craftButton.Enabled = true;
				m_craftButton.Margin = new Thickness(0.0395f, -0.029f, 0.0075f, 0f);
				myGuiControlStackPanel5.Add(m_craftButton);
				uint craftingCost = MyGameService.GetCraftingCost(MyGameInventoryItemQuality.Common);
				m_craftButton.Text = string.Format(MyTexts.GetString(MyCommonTexts.CraftButton), craftingCost);
				m_craftButton.Enabled = MyGameService.RecycleTokens >= craftingCost;
			}
			m_cancelButton = MakeButton(Vector2.Zero, MyCommonTexts.Cancel, MyCommonTexts.ScreenLoadInventoryCancelTooltip, OnCancelClick);
			m_cancelButton.Margin = new Thickness(0f, -0.029f, 0.0075f, 0.03f);
			myGuiControlStackPanel5.Add(m_cancelButton);
			if (SKIN_STORE_FEATURES_ENABLED)
			{
				m_openStoreButton = MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryBrowseItems, MyCommonTexts.ScreenLoadInventoryBrowseItems, OnOpenStore);
				myGuiControlStackPanel5.Add(m_openStoreButton);
			}
			else
			{
				m_refreshButton = MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadSubscribedWorldRefresh, MyCommonTexts.ScreenLoadInventoryRefreshTooltip, OnRefreshClick);
				m_refreshButton.Margin = new Thickness(0f, -0.029f, 0f, 0f);
				myGuiControlStackPanel5.Add(m_refreshButton);
			}
			m_wheel = new MyGuiControlRotatingWheel(Vector2.Zero, MyGuiConstants.ROTATING_WHEEL_COLOR, 0.2f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, m_rotatingWheelTexture, manualRotationUpdate: false, MyPerGameSettings.GUI.MultipleSpinningWheels);
			m_wheel.ManualRotationUpdate = false;
			m_wheel.Margin = new Thickness(0.21f, 0.047f, 0f, 0f);
			Elements.Add(m_wheel);
			myGuiControlStackPanel3.Add(m_wheel);
			MyGuiControlStackPanel myGuiControlStackPanel6 = new MyGuiControlStackPanel();
			myGuiControlStackPanel6.Orientation = MyGuiOrientation.Horizontal;
			myGuiControlStackPanel6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				Prepare_LowerTabState_Sliders(myGuiControlStackPanel6);
			}
			else
			{
				CreateRecyclerUI(myGuiControlStackPanel6);
			}
			GridLength[] columns = new GridLength[1]
			{
				new GridLength(1f)
			};
			GridLength[] rows = new GridLength[7]
			{
				new GridLength(0.6f),
				new GridLength(0.5f),
				new GridLength(0.8f),
				new GridLength(4.6f),
				new GridLength(0.6f),
				new GridLength(0.6f),
				new GridLength(0.8f)
			};
			MyGuiControlLayoutGrid myGuiControlLayoutGrid = new MyGuiControlLayoutGrid(columns, rows);
			myGuiControlLayoutGrid.Size = new Vector2(0.65f, 0.9f);
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel3, 0, 1);
			myGuiControlLayoutGrid.Add(m_categoryButtonLayout, 0, 2);
			if (MyGameService.InventoryItems != null && MyGameService.InventoryItems.Count == 0)
			{
				MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryNoItem));
				myGuiControlLabel.Margin = new Thickness(0.015f);
				myGuiControlLayoutGrid.Add(myGuiControlLabel, 0, 3);
				Elements.Add(myGuiControlLabel);
			}
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel, 0, 3);
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel4, 0, 4);
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel6, 0, 5);
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel5, 0, 6);
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenCaptionInventory), Vector4.One, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			myGuiControlLabel2.Name = "CaptionLabel";
			myGuiControlLabel2.Font = "ScreenCaption";
			Elements.Add(myGuiControlLabel2);
			myGuiControlLayoutGrid.Add(myGuiControlLabel2, 0, 0);
			GridLength[] columns2 = new GridLength[1]
			{
				new GridLength(1f)
			};
			GridLength[] rows2 = new GridLength[1]
			{
				new GridLength(1f)
			};
			MyGuiControlLayoutGrid myGuiControlLayoutGrid2 = new MyGuiControlLayoutGrid(columns2, rows2);
			myGuiControlLayoutGrid2.Size = new Vector2(1f, 1f);
			myGuiControlLayoutGrid2.Add(myGuiControlLayoutGrid, 0, 0);
			myGuiControlLayoutGrid2.UpdateMeasure();
			m_itemsTableParent.Size = new Vector2(m_itemsTableSize.X, m_itemsTable.Size.Y);
			m_itemsTable.Size = m_itemsTableSize;
			myGuiControlLayoutGrid2.UpdateArrange();
			Controls.Add(Prepare_scrollPanel());
			myGuiControlLabel2.Position = new Vector2(myGuiControlLabel2.Position.X + base.Size.Value.X / 2f, myGuiControlLabel2.Position.Y + MyGuiConstants.SCREEN_CAPTION_DELTA_Y / 3f + 0.023f);
			foreach (MyGuiControlBase control in myGuiControlStackPanel5.GetControls())
			{
				Controls.Add(control);
			}
			List<MyGuiControlBase> controls = m_categoryButtonLayout.GetControls();
			for (int i = 0; i < controls.Count; i++)
			{
				MyGuiControlImageButton myGuiControlImageButton = controls[i] as MyGuiControlImageButton;
				if (myGuiControlImageButton != null)
				{
					Prepare_categoryButton(myGuiControlImageButton, i, selectedButtonStyle);
				}
			}
			m_wheel.Visible = false;
			base.CloseButtonEnabled = true;
			m_storedHSV = MySession.Static.LocalCharacter.ColorMask;
			m_selectedHSV = m_storedHSV;
			m_sliderHue.Value = m_selectedHSV.X * 360f;
			m_sliderSaturation.Value = MathHelper.Clamp(m_selectedHSV.Y + MyColorPickerConstants.SATURATION_DELTA, 0f, 1f);
			m_sliderValue.Value = MathHelper.Clamp(m_selectedHSV.Z + MyColorPickerConstants.VALUE_DELTA - MyColorPickerConstants.VALUE_COLORIZE_DELTA, 0f, 1f);
			SubscribeValueChangedEventsToSliders(subsribe: true);
			m_contextMenu = new MyGuiControlContextMenu();
			m_contextMenu.CreateNewContextMenu();
			if (SKIN_STORE_FEATURES_ENABLED && MyGameService.IsOverlayEnabled)
			{
				StringBuilder stringBuilder = MyTexts.Get(MyCommonTexts.ScreenLoadInventoryBuyItem);
				m_contextMenu.AddItem(stringBuilder, stringBuilder.ToString(), "", InventoryItemAction.Buy);
			}
			StringBuilder stringBuilder2 = MyTexts.Get(MyCommonTexts.ScreenLoadInventorySellItem);
			if (MyGameService.IsOverlayEnabled)
			{
				m_contextMenu.AddItem(stringBuilder2, stringBuilder2.ToString(), "", InventoryItemAction.Sell);
			}
			StringBuilder stringBuilder3 = MyTexts.Get(MyCommonTexts.ScreenLoadInventoryRecycleItem);
			string.Format(stringBuilder3.ToString(), 0);
			m_contextMenu.AddItem(stringBuilder3, string.Empty, "", InventoryItemAction.Recycle);
			m_contextMenu.ItemClicked += m_contextMenu_ItemClicked;
			Controls.Add(m_contextMenu);
			m_contextMenu.Deactivate();
			if (constructor)
			{
				m_colorOrModelChanged = false;
			}
			Vector2 vector = default(Vector2);
			if (m_OkButton != null)
			{
				vector = m_OkButton.Position;
			}
			else if (m_craftButton != null)
			{
				vector = m_craftButton.Position;
			}
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2(vector.X, vector.Y + minSizeGui.Y / 2f));
			myGuiControlLabel3.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel3);
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				base.GamepadHelpTextId = MySpaceTexts.CharacterSkinInventory_Help_ScreenOK;
			}
			else
			{
				base.GamepadHelpTextId = MySpaceTexts.CharacterSkinInventory_Help_ScreenCraft;
			}
			UpdateSliderTooltips();
			UpdateGamepadHelp(base.FocusedControl);
			m_focusButton = true;
		}

		private void FocusButton(MyGuiScreenBase obj)
		{
			if (MyScreenManager.GetScreenWithFocus() != this || m_itemsTable == null)
			{
				return;
			}
			MyGuiControlBase firstVisible = m_itemsTable.GetFirstVisible();
			MyGuiControlLayoutGrid myGuiControlLayoutGrid;
			if (firstVisible == null || (myGuiControlLayoutGrid = firstVisible as MyGuiControlLayoutGrid) == null)
			{
				return;
			}
			foreach (MyGuiControlBase item in myGuiControlLayoutGrid.GetControlsAt(0, 0))
			{
				if (item is MyGuiControlImageButton)
				{
					base.FocusedControl = item;
					break;
				}
			}
		}

		private void OnViewTabColoring(MyGuiControlButton obj)
		{
			m_activeLowTabState = LowerTabState.Coloring;
			EquipTool();
			RecreateControls(constructor: false);
			UpdateCheckboxes();
		}

		private void CreateRecyclerUI(MyGuiControlStackPanel panel)
		{
			GridLength[] columns = new GridLength[3]
			{
				new GridLength(1.4f),
				new GridLength(0.6f),
				new GridLength(0.8f)
			};
			GridLength[] rows = new GridLength[1]
			{
				new GridLength(1f)
			};
			MyGuiControlLayoutGrid myGuiControlLayoutGrid = new MyGuiControlLayoutGrid(columns, rows);
			myGuiControlLayoutGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlLayoutGrid.Margin = new Thickness(0.055f, -0.035f, 0f, 0f);
			myGuiControlLayoutGrid.Size = new Vector2(0.65f, 0.1f);
			MyGuiControlStackPanel myGuiControlStackPanel = new MyGuiControlStackPanel();
			myGuiControlStackPanel.Orientation = MyGuiOrientation.Horizontal;
			myGuiControlStackPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlStackPanel.Margin = new Thickness(0f);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenLoadInventorySelectRarity));
			myGuiControlLabel.Margin = new Thickness(0f, 0f, 0.01f, 0f);
			myGuiControlStackPanel.Add(myGuiControlLabel);
			Controls.Add(myGuiControlLabel);
			m_rarityPicker = new MyGuiControlCombobox();
			m_rarityPicker.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_rarityPicker.Size = new Vector2(0.15f, 0f);
			foreach (object value3 in Enum.GetValues(typeof(MyGameInventoryItemQuality)))
			{
				m_rarityPicker.AddItem((int)value3, MyTexts.GetString(MyStringId.GetOrCompute(value3.ToString())));
			}
			m_rarityPicker.SelectItemByIndex(0);
			m_rarityPicker.ItemSelected += m_rarityPicker_ItemSelected;
			myGuiControlStackPanel.Add(m_rarityPicker);
			Controls.Add(m_rarityPicker);
			if (m_lastCraftedItem != null)
			{
				Vector2 value = new Vector2(0.07f, 0.09f);
				MyGuiControlImage myGuiControlImage = null;
				if (string.IsNullOrEmpty(m_lastCraftedItem.ItemDefinition.BackgroundColor))
				{
					myGuiControlImage = new MyGuiControlImage(null, value, null, null, new string[1] { "Textures\\GUI\\Controls\\grid_item_highlight.dds" }, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
					Controls.Add(myGuiControlImage);
				}
				else
				{
					Vector4 value2 = (string.IsNullOrEmpty(m_lastCraftedItem.ItemDefinition.BackgroundColor) ? Vector4.One : ColorExtensions.HexToVector4(m_lastCraftedItem.ItemDefinition.BackgroundColor));
					myGuiControlImage = new MyGuiControlImage(null, new Vector2(value.X - 0.004f, value.Y - 0.002f), value2, null, new string[1] { "Textures\\GUI\\blank.dds" });
					Controls.Add(myGuiControlImage);
				}
				string[] array = new string[1] { "Textures\\GUI\\Blank.dds" };
				if (!string.IsNullOrEmpty(m_lastCraftedItem.ItemDefinition.IconTexture))
				{
					array[0] = m_lastCraftedItem.ItemDefinition.IconTexture;
				}
				MyGuiControlImage control = new MyGuiControlImage(null, new Vector2(0.06f, 0.08f), null, null, array);
				Controls.Add(control);
				MyGuiControlLabel control2 = new MyGuiControlLabel(null, null, m_lastCraftedItem.ItemDefinition.Name, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				Controls.Add(control2);
				MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryCraftedLabel), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
				myGuiControlLabel2.Margin = new Thickness(0f, -0.035f, 0f, 0f);
				Controls.Add(myGuiControlLabel2);
				myGuiControlLayoutGrid.Add(myGuiControlImage, 1, 0);
				myGuiControlLayoutGrid.Add(control, 1, 0);
				myGuiControlLayoutGrid.Add(control2, 2, 0);
				myGuiControlLayoutGrid.Add(myGuiControlLabel2, 2, 0);
			}
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel, 0, 0);
			panel.Add(myGuiControlLayoutGrid);
		}

		private void OnOkClick(MyGuiControlButton obj)
		{
			if (m_colorOrModelChanged && MyGuiScreenLoadInventory.LookChanged != null && MySession.Static != null)
			{
				MyGuiScreenLoadInventory.LookChanged();
			}
			if (MySession.Static.LocalCharacter.Definition.UsableByPlayer)
			{
				MyLocalCache.SaveInventoryConfig(MySession.Static.LocalCharacter);
			}
			CloseScreen();
		}

		private void OnCancelClick(MyGuiControlButton obj)
		{
			Cancel();
			CloseScreen();
		}

		private void OnCraftClick(MyGuiControlButton obj)
		{
			MyGameService.ItemsAdded -= MyGameService_ItemsAdded;
			MyGameService.ItemsAdded += MyGameService_ItemsAdded;
			if (MyGameService.CraftSkin((MyGameInventoryItemQuality)m_rarityPicker.GetSelectedKey()))
			{
				RotatingWheelShow();
			}
			else
			{
				MyGameService.ItemsAdded -= MyGameService_ItemsAdded;
			}
		}

		private void MyGameService_ItemsAdded(object sender, MyGameItemsEventArgs e)
		{
			if (e.NewItems != null && e.NewItems.Count > 0)
			{
				m_lastCraftedItem = e.NewItems[0];
				m_lastCraftedItem.IsNew = true;
				MyGameService.ItemsAdded -= MyGameService_ItemsAdded;
				RefreshUI();
			}
			RotatingWheelHide();
		}

		private static void Cancel()
		{
			if (MyGameService.InventoryItems != null)
			{
				foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
				{
					inventoryItem.UsingCharacters.Remove(MySession.Static.LocalCharacter.EntityId);
				}
			}
			MyLocalCache.LoadInventoryConfig(MySession.Static.LocalCharacter);
		}

		private void m_toolPicker_ItemSelected()
		{
			MyPhysicalInventoryItem myPhysicalInventoryItem = Enumerable.FirstOrDefault<MyPhysicalInventoryItem>((IEnumerable<MyPhysicalInventoryItem>)m_allTools, (Func<MyPhysicalInventoryItem, bool>)((MyPhysicalInventoryItem t) => t.ItemId == m_toolPicker.GetSelectedKey()));
			if (myPhysicalInventoryItem.Content != null)
			{
				m_selectedTool = myPhysicalInventoryItem.Content.SubtypeName;
				EquipTool();
			}
		}

		private void m_rarityPicker_ItemSelected()
		{
			uint craftingCost = MyGameService.GetCraftingCost((MyGameInventoryItemQuality)m_rarityPicker.GetSelectedIndex());
			m_craftButton.Text = string.Format(MyTexts.GetString(MyCommonTexts.CraftButton), craftingCost);
			m_craftButton.Enabled = MyGameService.RecycleTokens >= craftingCost;
		}

		private void OnHideDuplicates(MyGuiControlCheckbox obj)
		{
			m_hideDuplicatesEnabled = obj.IsChecked;
			RefreshUI();
		}

		private void OnShowOnlyDuplicates(MyGuiControlCheckbox obj)
		{
			m_showOnlyDuplicatesEnabled = obj.IsChecked;
			RefreshUI();
		}

		private void m_contextMenu_ItemClicked(MyGuiControlContextMenu contextMenu, MyGuiControlContextMenu.EventArgs selectedItem)
		{
			switch ((InventoryItemAction)selectedItem.UserData)
			{
			case InventoryItemAction.Apply:
				hiddenButton_ButtonClicked(m_contextMenuLastButton);
				break;
			case InventoryItemAction.Sell:
				OpenUserInventory();
				break;
			case InventoryItemAction.Recycle:
				RecycleItemRequest();
				break;
			case InventoryItemAction.Delete:
				DeleteItemRequest();
				break;
			case InventoryItemAction.Buy:
				OpenCurrentItemInStore();
				break;
			case InventoryItemAction.Trade:
				break;
			}
		}

		private void OnViewTools(MyGuiControlButton obj)
		{
			m_activeTabState = TabState.Tools;
			m_filteredSlot = MyGameInventoryItemSlot.Welder;
			m_selectedTool = string.Empty;
			m_currentCategoryIndex = 0;
			RefreshUI();
		}

		private void OnViewTabCharacter(MyGuiControlButton obj)
		{
			m_activeTabState = TabState.Character;
			m_filteredSlot = MyGameInventoryItemSlot.None;
			m_selectedTool = string.Empty;
			m_currentCategoryIndex = -1;
			EquipTool();
			RecreateControls(constructor: false);
			UpdateCheckboxes();
		}

		private void OnViewTabRecycling(MyGuiControlButton obj)
		{
			m_activeLowTabState = LowerTabState.Recycling;
			EquipTool();
			RecreateControls(constructor: false);
			UpdateCheckboxes();
		}

		private void OnItemSelected()
		{
			Cancel();
			int key = (int)m_modelPicker.GetSelectedKey();
			m_selectedModel = m_models[key];
			ChangeCharacter(m_selectedModel, m_selectedHSV);
			RecreateControls(constructor: false);
			MyLocalCache.ResetAllInventorySlots(MySession.Static.LocalCharacter);
			RefreshItems();
		}

		private void ChangeCharacter(string model, Vector3 colorMaskHSV, bool resetToDefault = true)
		{
			m_colorOrModelChanged = true;
			MySession.Static.LocalCharacter.ChangeModelAndColor(model, colorMaskHSV, resetToDefault, MySession.Static.LocalPlayerId);
		}

		public static void ResetOnFinish(string model, bool resetToDefault)
		{
			MyGuiScreenLoadInventory firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoadInventory>();
			if (firstScreenOfType != null && !(firstScreenOfType.m_selectedModel == MySession.Static.LocalCharacter.ModelName))
			{
				if (resetToDefault)
				{
					firstScreenOfType.ResetOnFinishInternal(model);
				}
				firstScreenOfType.RecreateControls(constructor: false);
				MyLocalCache.ResetAllInventorySlots(MySession.Static.LocalCharacter);
				firstScreenOfType.RefreshItems();
			}
		}

		private void ResetOnFinishInternal(string model)
		{
			if (model == "Default_Astronaut" || model == "Default_Astronaut_Female")
			{
				MyLocalCache.LoadInventoryConfig(MySession.Static.LocalCharacter, setModel: false);
				return;
			}
			int key = (int)m_modelPicker.GetSelectedKey();
			m_selectedModel = m_models[key];
			Cancel();
			RecreateControls(constructor: false);
			MyLocalCache.ResetAllInventorySlots(MySession.Static.LocalCharacter);
			RefreshItems();
		}

		private void OnValueChange(MyGuiControlSlider sender)
		{
			UpdateSliderTooltips();
			m_selectedHSV.X = m_sliderHue.Value / 360f;
			m_selectedHSV.Y = m_sliderSaturation.Value - MyColorPickerConstants.SATURATION_DELTA;
			m_selectedHSV.Z = m_sliderValue.Value - MyColorPickerConstants.VALUE_DELTA + MyColorPickerConstants.VALUE_COLORIZE_DELTA;
			ChangeCharacter(m_selectedModel, m_selectedHSV, resetToDefault: false);
		}

		private void UpdateSliderTooltips()
		{
			((Collection<MyColoredText>)(object)m_sliderHue.Tooltips.ToolTips).Clear();
			m_sliderHue.Tooltips.AddToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryHue), m_sliderHue.Value));
			((Collection<MyColoredText>)(object)m_sliderSaturation.Tooltips.ToolTips).Clear();
			m_sliderSaturation.Tooltips.AddToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventorySaturation), m_sliderSaturation.Value.ToString("P1")));
			((Collection<MyColoredText>)(object)m_sliderValue.Tooltips.ToolTips).Clear();
			m_sliderValue.Tooltips.AddToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryValue), m_sliderValue.Value.ToString("P1")));
		}

		private MyGuiControlWrapPanel CreateItemsTable(MyGuiControlParent parent)
		{
			Vector2 vector = new Vector2(0.07f, 0.09f);
			MyGuiControlWrapPanel myGuiControlWrapPanel = new MyGuiControlWrapPanel(vector);
			myGuiControlWrapPanel.Size = m_itemsTableSize;
			myGuiControlWrapPanel.Margin = new Thickness(0.018f, 0.044f, 0f, 0f);
			myGuiControlWrapPanel.InnerOffset = new Vector2(0.005f, 0.0065f);
			myGuiControlWrapPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			MyGuiControlImageButton.StyleDefinition style = new MyGuiControlImageButton.StyleDefinition
			{
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture("Textures\\Gui\\Screens\\screen_background_fade.dds")
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture("Textures\\Gui\\Screens\\screen_background_fade.dds")
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture("Textures\\Gui\\Screens\\screen_background_fade.dds")
				}
			};
			MyGuiControlCheckbox.StyleDefinition style2 = new MyGuiControlCheckbox.StyleDefinition
			{
				NormalCheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_GREEN_CHECKED,
				NormalUncheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_BLANK,
				HighlightCheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_BLANK,
				HighlightUncheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_BLANK
			};
			m_itemCheckboxes = new List<MyGuiControlCheckbox>();
			m_userItems = GetInventoryItems();
			if (SKIN_STORE_FEATURES_ENABLED)
			{
				List<MyGameInventoryItem> storeItems = GetStoreItems(m_userItems);
				m_userItems.AddRange(storeItems);
			}
			for (int i = 0; i < m_userItems.Count; i++)
			{
				MyGameInventoryItem myGameInventoryItem = m_userItems[i];
				if (myGameInventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.None || myGameInventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Emote || myGameInventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor || (m_filteredSlot != 0 && m_filteredSlot != myGameInventoryItem.ItemDefinition.ItemSlot))
				{
					continue;
				}
				if (m_filteredSlot == MyGameInventoryItemSlot.None)
				{
					MyGameInventoryItemSlot itemSlot = myGameInventoryItem.ItemDefinition.ItemSlot;
					switch (m_activeTabState)
					{
					case TabState.Character:
						if (itemSlot == MyGameInventoryItemSlot.Grinder || itemSlot == MyGameInventoryItemSlot.Rifle || itemSlot == MyGameInventoryItemSlot.Welder || itemSlot == MyGameInventoryItemSlot.Drill)
						{
							continue;
						}
						break;
					case TabState.Tools:
						if (itemSlot == MyGameInventoryItemSlot.Helmet || itemSlot == MyGameInventoryItemSlot.Gloves || itemSlot == MyGameInventoryItemSlot.Suit || itemSlot == MyGameInventoryItemSlot.Boots)
						{
							continue;
						}
						break;
					}
				}
				GridLength[] columns = new GridLength[2]
				{
					new GridLength(1f),
					new GridLength(1f)
				};
				GridLength[] rows = new GridLength[2]
				{
					new GridLength(1f),
					new GridLength(0f)
				};
				MyGuiControlLayoutGrid myGuiControlLayoutGrid = new MyGuiControlLayoutGrid(columns, rows);
				myGuiControlLayoutGrid.Size = vector;
				myGuiControlLayoutGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
<<<<<<< HEAD
				MyGuiControlImageButton myGuiControlImageButton = new MyGuiControlImageButton("Button", myGuiControlLayoutGrid.Position, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, onButtonClick: hiddenButton_ButtonClicked, toolTip: myGameInventoryItem.ItemDefinition.Name);
				if (myGameInventoryItem.ItemDefinition.Tradable == "1")
				{
					myGuiControlImageButton.ButtonRightClicked += hiddenButton_ButtonRightClicked;
				}
=======
				MyGuiControlImageButton myGuiControlImageButton = new MyGuiControlImageButton("Button", myGuiControlLayoutGrid.Position, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, onButtonClick: hiddenButton_ButtonClicked, onButtonRightClick: hiddenButton_ButtonRightClicked, toolTip: myGameInventoryItem.ItemDefinition.Name);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!MyInput.Static.IsJoystickLastUsed)
				{
					myGuiControlImageButton.Tooltips.AddToolTip(string.Empty);
				}
				if (!MyInput.Static.IsJoystickLastUsed && myGameInventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Helmet)
				{
					MyControl gameControl = MyInput.Static.GetGameControl(MyControlsSpace.HELMET);
					string toolTip = string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryToggleHelmet), gameControl.GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
					myGuiControlImageButton.Tooltips.AddToolTip(toolTip);
				}
				if (!MyInput.Static.IsJoystickLastUsed)
				{
					myGuiControlImageButton.Tooltips.AddToolTip(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryLeftClickTip));
					myGuiControlImageButton.Tooltips.AddToolTip(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryRightClickTip));
				}
				myGuiControlImageButton.ApplyStyle(style);
				myGuiControlImageButton.Size = myGuiControlLayoutGrid.Size;
				myGuiControlImageButton.HighlightChanged += delegate(MyGuiControlBase x)
				{
					ChangeButtonBorder(x);
				};
				myGuiControlImageButton.FocusChanged += delegate(MyGuiControlBase x, bool y)
				{
					ChangeButtonBorder(x);
				};
				parent.Controls.Add(myGuiControlImageButton);
				MyGuiControlImage myGuiControlImage = null;
				if (string.IsNullOrEmpty(myGameInventoryItem.ItemDefinition.BackgroundColor))
				{
					myGuiControlImage = new MyGuiControlImage(null, vector, null, null, new string[1] { "Textures\\GUI\\Controls\\grid_item_highlight.dds" }, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					parent.Controls.Add(myGuiControlImage);
				}
				else
				{
					Vector4 value = (string.IsNullOrEmpty(myGameInventoryItem.ItemDefinition.BackgroundColor) ? Vector4.One : ColorExtensions.HexToVector4(myGameInventoryItem.ItemDefinition.BackgroundColor));
					myGuiControlImage = new MyGuiControlImage(null, new Vector2(vector.X - 0.004f, vector.Y - 0.002f), value, null, new string[1] { "Textures\\GUI\\blank.dds" }, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					parent.Controls.Add(myGuiControlImage);
					myGuiControlImage.Margin = new Thickness(0.0023f, 0.001f, 0f, 0f);
				}
				myGuiControlImage.Name = IMAGE_PREFIX;
				string[] array = new string[1] { "Textures\\GUI\\Blank.dds" };
				if (!string.IsNullOrEmpty(myGameInventoryItem.ItemDefinition.IconTexture))
				{
					array[0] = myGameInventoryItem.ItemDefinition.IconTexture;
				}
				MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(null, new Vector2(0.06f, 0.08f), null, null, array, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				myGuiControlImage2.Margin = new Thickness(0.005f, 0.005f, 0f, 0f);
				parent.Controls.Add(myGuiControlImage2);
				MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(null, null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				myGuiControlCheckbox.Name = m_equipCheckbox;
				myGuiControlCheckbox.ApplyStyle(style2);
				myGuiControlCheckbox.Margin = new Thickness(0.005f, 0.005f, 0.01f, 0.01f);
				myGuiControlCheckbox.IsHitTestVisible = false;
				myGuiControlCheckbox.CanHaveFocus = false;
				parent.Controls.Add(myGuiControlCheckbox);
				myGuiControlCheckbox.UserData = myGameInventoryItem;
				myGuiControlImageButton.UserData = myGuiControlLayoutGrid;
				m_itemCheckboxes.Add(myGuiControlCheckbox);
				myGuiControlLayoutGrid.Add(myGuiControlImage, 0, 0);
				myGuiControlLayoutGrid.Add(myGuiControlImageButton, 0, 0);
				myGuiControlLayoutGrid.Add(myGuiControlImage2, 0, 0);
				myGuiControlLayoutGrid.Add(myGuiControlCheckbox, 1, 0);
				if (myGameInventoryItem.IsNew)
				{
					MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage(null, new Vector2(0.0175f, 0.023f), null, null, new string[1] { "Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png" }, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					myGuiControlImage3.Margin = new Thickness(0.01f, -0.035f, 0f, 0f);
					parent.Controls.Add(myGuiControlImage3);
					myGuiControlLayoutGrid.Add(myGuiControlImage3, 1, 1);
				}
				myGuiControlWrapPanel.Add(myGuiControlLayoutGrid);
				parent.Controls.Add(myGuiControlLayoutGrid);
			}
			return myGuiControlWrapPanel;
		}

		private void ChangeButtonBorder(MyGuiControlBase obj)
		{
			if (obj.HasHighlight)
			{
				MyGuiControlImage myGuiControlImage = null;
				foreach (MyGuiControlBase item in (obj.UserData as MyGuiControlLayoutGrid).GetControlsAt(0, 0))
				{
					if (item.Name.Length >= IMAGE_PREFIX.Length && item.Name.Substring(0, IMAGE_PREFIX.Length) == IMAGE_PREFIX)
					{
						myGuiControlImage = item as MyGuiControlImage;
						break;
					}
				}
				if (myGuiControlImage != null)
				{
					myGuiControlImage.ColorMask = MyGuiConstants.HIGHLIGHT_BACKGROUND_COLOR;
				}
				return;
			}
			if (obj.HasFocus)
			{
				MyGuiControlImage myGuiControlImage2 = null;
				foreach (MyGuiControlBase item2 in (obj.UserData as MyGuiControlLayoutGrid).GetControlsAt(0, 0))
				{
					if (item2.Name.Length >= IMAGE_PREFIX.Length && item2.Name.Substring(0, IMAGE_PREFIX.Length) == IMAGE_PREFIX)
					{
						myGuiControlImage2 = item2 as MyGuiControlImage;
						break;
					}
				}
				if (myGuiControlImage2 != null)
				{
					myGuiControlImage2.ColorMask = MyGuiConstants.FOCUS_BACKGROUND_COLOR;
				}
				return;
			}
			List<MyGuiControlBase> controlsAt = (obj.UserData as MyGuiControlLayoutGrid).GetControlsAt(1, 0);
			if (controlsAt == null || controlsAt.Count <= 0)
			{
				return;
			}
			MyGameInventoryItem myGameInventoryItem = controlsAt[0].UserData as MyGameInventoryItem;
			if (myGameInventoryItem == null)
			{
				return;
			}
			MyGuiControlImage myGuiControlImage3 = null;
			foreach (MyGuiControlBase item3 in (obj.UserData as MyGuiControlLayoutGrid).GetControlsAt(0, 0))
			{
				if (item3.Name.Length >= IMAGE_PREFIX.Length && item3.Name.Substring(0, IMAGE_PREFIX.Length) == IMAGE_PREFIX)
				{
					myGuiControlImage3 = item3 as MyGuiControlImage;
					break;
				}
			}
			if (myGuiControlImage3 != null)
			{
				myGuiControlImage3.ColorMask = (string.IsNullOrEmpty(myGameInventoryItem.ItemDefinition.BackgroundColor) ? Vector4.One : ColorExtensions.HexToVector4(myGameInventoryItem.ItemDefinition.BackgroundColor));
			}
		}

		private static List<MyGameInventoryItem> GetStoreItems(List<MyGameInventoryItem> userItems)
		{
			List<MyGameInventoryItemDefinition> list = new List<MyGameInventoryItemDefinition>(MyGameService.Definitions.Values);
			List<MyGameInventoryItem> list2 = new List<MyGameInventoryItem>();
			foreach (MyGameInventoryItemDefinition item in list)
			{
				if (item.DefinitionType == MyGameInventoryItemDefinitionType.item && item.ItemSlot != 0 && !item.Hidden && !item.IsStoreHidden && Enumerable.FirstOrDefault<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)userItems, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.ItemDefinition.ID == item.ID)) == null)
				{
					MyGameInventoryItem item2 = new MyGameInventoryItem
					{
						ID = 0uL,
						IsStoreFakeItem = true,
						ItemDefinition = item,
						Quantity = 1
					};
					list2.Add(item2);
				}
			}
			return Enumerable.ToList<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)Enumerable.OrderBy<MyGameInventoryItem, string>((IEnumerable<MyGameInventoryItem>)list2, (Func<MyGameInventoryItem, string>)((MyGameInventoryItem i) => i.ItemDefinition.Name)));
		}

		private List<MyGameInventoryItem> GetInventoryItems()
		{
			List<MyGameInventoryItem> list = new List<MyGameInventoryItem>(MyGameService.InventoryItems);
			List<MyGameInventoryItem> list2 = null;
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				if (list.Count > 0 && m_hideDuplicatesEnabled)
				{
					list2 = new List<MyGameInventoryItem>();
<<<<<<< HEAD
					list2.AddRange(list.Where((MyGameInventoryItem i) => i.IsNew));
					list2.AddRange(list.Where((MyGameInventoryItem i) => i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId)));
					foreach (MyGameInventoryItem item in list)
					{
						if (!item.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId) && list2.FirstOrDefault((MyGameInventoryItem i) => i.ItemDefinition.AssetModifierId == item.ItemDefinition.AssetModifierId) == null)
=======
					list2.AddRange(Enumerable.Where<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)list, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.IsNew)));
					list2.AddRange(Enumerable.Where<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)list, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))));
					foreach (MyGameInventoryItem item in list)
					{
						if (!item.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId) && Enumerable.FirstOrDefault<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)list2, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.ItemDefinition.AssetModifierId == item.ItemDefinition.AssetModifierId)) == null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							list2.Add(item);
						}
					}
<<<<<<< HEAD
					return (from i in list2
						orderby i.IsNew descending, i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId) descending, i.ItemDefinition.Name
						select i).ToList();
				}
				return (from i in list
					orderby i.IsNew descending, i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId) descending, i.ItemDefinition.Name
					select i).ToList();
=======
					return Enumerable.ToList<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)Enumerable.ThenBy<MyGameInventoryItem, string>(Enumerable.ThenByDescending<MyGameInventoryItem, bool>(Enumerable.OrderByDescending<MyGameInventoryItem, bool>((IEnumerable<MyGameInventoryItem>)list2, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.IsNew)), (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))), (Func<MyGameInventoryItem, string>)((MyGameInventoryItem i) => i.ItemDefinition.Name)));
				}
				return Enumerable.ToList<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)Enumerable.ThenBy<MyGameInventoryItem, string>(Enumerable.ThenByDescending<MyGameInventoryItem, bool>(Enumerable.OrderByDescending<MyGameInventoryItem, bool>((IEnumerable<MyGameInventoryItem>)list, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.IsNew)), (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))), (Func<MyGameInventoryItem, string>)((MyGameInventoryItem i) => i.ItemDefinition.Name)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (list.Count > 0 && m_showOnlyDuplicatesEnabled)
			{
				HashSet<string> val = new HashSet<string>();
				list2 = new List<MyGameInventoryItem>();
				foreach (MyGameInventoryItem item2 in list)
				{
					if (!item2.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))
					{
						if (!val.Contains(item2.ItemDefinition.AssetModifierId))
						{
							val.Add(item2.ItemDefinition.AssetModifierId);
						}
						else
						{
							list2.Add(item2);
						}
					}
				}
<<<<<<< HEAD
				return (from i in list2
					orderby i.IsNew descending, i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId) descending, i.ItemDefinition.Name
					select i).ToList();
			}
			list2 = new List<MyGameInventoryItem>();
			list2.AddRange(list.Where((MyGameInventoryItem i) => !i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId)));
			return (from i in list2
				orderby i.IsNew descending, i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId) descending, i.ItemDefinition.Name
				select i).ToList();
=======
				return Enumerable.ToList<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)Enumerable.ThenBy<MyGameInventoryItem, string>(Enumerable.ThenByDescending<MyGameInventoryItem, bool>(Enumerable.OrderByDescending<MyGameInventoryItem, bool>((IEnumerable<MyGameInventoryItem>)list2, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.IsNew)), (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))), (Func<MyGameInventoryItem, string>)((MyGameInventoryItem i) => i.ItemDefinition.Name)));
			}
			list2 = new List<MyGameInventoryItem>();
			list2.AddRange(Enumerable.Where<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)list, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => !i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))));
			return Enumerable.ToList<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)Enumerable.ThenBy<MyGameInventoryItem, string>(Enumerable.ThenByDescending<MyGameInventoryItem, bool>(Enumerable.OrderByDescending<MyGameInventoryItem, bool>((IEnumerable<MyGameInventoryItem>)list2, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.IsNew)), (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))), (Func<MyGameInventoryItem, string>)((MyGameInventoryItem i) => i.ItemDefinition.Name)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void hiddenButton_ButtonClicked(MyGuiControlImageButton obj)
		{
			MyGuiControlLayoutGrid myGuiControlLayoutGrid = obj.UserData as MyGuiControlLayoutGrid;
			if (myGuiControlLayoutGrid != null)
			{
				MyGuiControlCheckbox myGuiControlCheckbox = Enumerable.FirstOrDefault<MyGuiControlBase>((IEnumerable<MyGuiControlBase>)myGuiControlLayoutGrid.GetAllControls(), (Func<MyGuiControlBase, bool>)((MyGuiControlBase c) => c.Name.StartsWith(m_equipCheckbox))) as MyGuiControlCheckbox;
				if (myGuiControlCheckbox != null)
				{
					myGuiControlCheckbox.IsChecked = !myGuiControlCheckbox.IsChecked;
				}
			}
		}

		private void hiddenButton_ButtonRightClicked(MyGuiControlImageButton obj)
		{
			m_contextMenuLastButton = obj;
			MyGuiControlListbox.Item item = Enumerable.FirstOrDefault<MyGuiControlListbox.Item>((IEnumerable<MyGuiControlListbox.Item>)m_contextMenu.Items, (Func<MyGuiControlListbox.Item, bool>)((MyGuiControlListbox.Item i) => i.UserData != null && (InventoryItemAction)i.UserData == InventoryItemAction.Recycle));
			if (item != null)
			{
				MyGameInventoryItem currentItem = GetCurrentItem();
				if (currentItem != null)
				{
					string value = string.Format(MyTexts.Get(MyCommonTexts.ScreenLoadInventoryRecycleItem).ToString(), MyGameService.GetRecyclingReward(currentItem.ItemDefinition.ItemQuality));
					item.Text = new StringBuilder(value);
				}
			}
			m_contextMenu.Activate();
			base.FocusedControl = m_contextMenu.GetInnerList();
			obj.BlockAutofocusOnHandlingOnce = true;
		}

		private void OnCategoryClicked(MyGuiControlImageButton obj)
		{
			if (obj.UserData == null)
			{
				return;
			}
			MyGameInventoryItemSlot myGameInventoryItemSlot = (MyGameInventoryItemSlot)obj.UserData;
			if (myGameInventoryItemSlot == m_filteredSlot)
			{
				if (m_activeTabState == TabState.Character)
				{
					m_filteredSlot = MyGameInventoryItemSlot.None;
				}
			}
			else
			{
				m_filteredSlot = myGameInventoryItemSlot;
			}
			m_selectedTool = string.Empty;
			RecreateControls(constructor: false);
			EquipTool();
			UpdateCheckboxes();
		}

		private void EquipTool()
		{
			if (m_filteredSlot != 0 && m_activeTabState == TabState.Tools)
			{
				long key = m_toolPicker.GetSelectedKey();
				MyPhysicalInventoryItem myPhysicalInventoryItem = Enumerable.FirstOrDefault<MyPhysicalInventoryItem>((IEnumerable<MyPhysicalInventoryItem>)m_allTools, (Func<MyPhysicalInventoryItem, bool>)((MyPhysicalInventoryItem t) => t.ItemId == key));
				if (myPhysicalInventoryItem.Content != null)
				{
					m_entityController.ActivateCharacterToolbarItem(new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), myPhysicalInventoryItem.Content.SubtypeName));
				}
				foreach (MyGameInventoryItem userItem in m_userItems)
				{
					if (userItem.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId) && userItem.ItemDefinition.ItemSlot == m_filteredSlot)
					{
						m_itemCheckActive = true;
						MyGameService.GetItemCheckData(userItem, null);
						break;
					}
				}
			}
			else
			{
				m_entityController.ActivateCharacterToolbarItem(default(MyDefinitionId));
			}
		}

		private void OnItemCheckChanged(MyGuiControlCheckbox sender)
		{
			if (sender == null)
			{
				return;
			}
			MyGameInventoryItem myGameInventoryItem = sender.UserData as MyGameInventoryItem;
			if (myGameInventoryItem == null)
			{
				return;
			}
			if (sender.IsChecked)
			{
				m_itemCheckActive = true;
				MyGameService.GetItemCheckData(myGameInventoryItem, null);
				return;
			}
			m_itemCheckActive = false;
			myGameInventoryItem.UsingCharacters.Remove(MySession.Static.LocalCharacter.EntityId);
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter == null)
			{
				return;
			}
			if (localCharacter != null)
			{
				MyAssetModifierComponent component;
				switch (myGameInventoryItem.ItemDefinition.ItemSlot)
				{
				case MyGameInventoryItemSlot.Face:
				case MyGameInventoryItemSlot.Helmet:
				case MyGameInventoryItemSlot.Gloves:
				case MyGameInventoryItemSlot.Boots:
				case MyGameInventoryItemSlot.Suit:
					if (localCharacter.Components.TryGet<MyAssetModifierComponent>(out component))
					{
						component.ResetSlot(myGameInventoryItem.ItemDefinition.ItemSlot);
					}
					break;
				case MyGameInventoryItemSlot.Rifle:
				case MyGameInventoryItemSlot.Welder:
				case MyGameInventoryItemSlot.Grinder:
				case MyGameInventoryItemSlot.Drill:
				{
					MyEntity myEntity = localCharacter.CurrentWeapon as MyEntity;
					if (myEntity != null && myEntity.Components.TryGet<MyAssetModifierComponent>(out component))
					{
						component.ResetSlot(myGameInventoryItem.ItemDefinition.ItemSlot);
					}
					break;
				}
				}
			}
			UpdateOKButton();
		}

		private void MyGameService_CheckItemDataReady(object sender, MyGameItemsEventArgs itemArgs)
		{
			if (itemArgs.NewItems == null || itemArgs.NewItems.Count == 0)
			{
				return;
			}
			MyGameInventoryItem myGameInventoryItem = itemArgs.NewItems[0];
			UseItem(myGameInventoryItem, itemArgs.CheckData);
			foreach (MyGameInventoryItem item in new List<MyGameInventoryItem>(MyGameService.InventoryItems))
			{
				if (item != null && item.ID != myGameInventoryItem.ID && item.ItemDefinition.ItemSlot == myGameInventoryItem.ItemDefinition.ItemSlot)
				{
					item.UsingCharacters.Remove(MySession.Static.LocalCharacter.EntityId);
				}
			}
			UpdateCheckboxes();
			UpdateOKButton();
		}

		private void UpdateOKButton()
		{
			bool flag = true;
			foreach (MyGameInventoryItem userItem in m_userItems)
			{
				if (userItem.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId))
				{
					flag &= !userItem.IsStoreFakeItem;
				}
			}
			m_OkButton.Enabled = flag;
		}

		private void UpdateCheckboxes()
		{
			if (!MySession.Static.LocalCharacter.Components.TryGet<MyAssetModifierComponent>(out var _))
			{
				return;
			}
			foreach (MyGuiControlCheckbox itemCheckbox in m_itemCheckboxes)
			{
				MyGameInventoryItem myGameInventoryItem = itemCheckbox.UserData as MyGameInventoryItem;
				if (myGameInventoryItem != null)
				{
					itemCheckbox.IsCheckedChanged = null;
					itemCheckbox.IsChecked = myGameInventoryItem.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId);
					itemCheckbox.IsCheckedChanged = OnItemCheckChanged;
				}
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenLoadInventory";
		}

		private void OpenCurrentItemInStore()
		{
			MyGameInventoryItem currentItem = GetCurrentItem();
			if (currentItem != null)
			{
				MyGuiSandbox.OpenUrlWithFallback(string.Format(MySteamConstants.URL_INVENTORY_BUY_ITEM_FORMAT, MyGameService.AppId, currentItem.ItemDefinition.ID), "Buy Item");
			}
		}

		private void OpenUserInventory()
		{
			MyGuiSandbox.OpenUrlWithFallback(string.Format(MySteamConstants.URL_INVENTORY, MyGameService.UserId, MyGameService.AppId), "User Inventory");
		}

		private void OnOpenStore(MyGuiControlButton obj)
		{
			MyGuiSandbox.OpenUrlWithFallback(string.Format(MySteamConstants.URL_INVENTORY_BROWSE_ALL_ITEMS, MyGameService.AppId), "Store");
		}

		private MyGameInventoryItem GetCurrentItem()
		{
			if (m_contextMenuLastButton == null)
			{
				return null;
			}
			MyGuiControlLayoutGrid myGuiControlLayoutGrid = m_contextMenuLastButton.UserData as MyGuiControlLayoutGrid;
			if (myGuiControlLayoutGrid == null)
			{
				return null;
			}
			MyGuiControlCheckbox myGuiControlCheckbox = Enumerable.FirstOrDefault<MyGuiControlBase>((IEnumerable<MyGuiControlBase>)myGuiControlLayoutGrid.GetAllControls(), (Func<MyGuiControlBase, bool>)((MyGuiControlBase c) => c.Name.StartsWith(m_equipCheckbox))) as MyGuiControlCheckbox;
			if (myGuiControlCheckbox == null)
			{
				return null;
			}
			return myGuiControlCheckbox.UserData as MyGameInventoryItem;
		}

		private void RecycleItemRequest()
		{
			if (GetCurrentItem() != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.ScreenLoadInventoryRecycleItemMessageTitle), messageText: MyTexts.Get(MyCommonTexts.ScreenLoadInventoryRecycleItemMessageText), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: OnRecycleItem));
			}
		}

		private void OnRecycleItem(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MyGameInventoryItem currentItem = GetCurrentItem();
				if (currentItem != null && MyGameService.RecycleItem(currentItem))
				{
					RemoveItemFromUI(currentItem);
				}
			}
			base.State = MyGuiScreenState.OPENING;
		}

		private void DeleteItemRequest()
		{
			if (GetCurrentItem() != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.ScreenLoadInventoryDeleteItemMessageTitle), messageText: MyTexts.Get(MyCommonTexts.ScreenLoadInventoryDeleteItemMessageText), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: DeleteItemRequestMessageHandler));
			}
		}

		private void DeleteItemRequestMessageHandler(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MyGameInventoryItem currentItem = GetCurrentItem();
				if (currentItem != null)
				{
					MyGameService.ConsumeItem(currentItem);
					RemoveItemFromUI(currentItem);
				}
			}
			base.State = MyGuiScreenState.OPENING;
		}

		private void RemoveItemFromUI(MyGameInventoryItem item)
		{
			MyGuiControlLayoutGrid myGuiControlLayoutGrid = m_contextMenuLastButton.UserData as MyGuiControlLayoutGrid;
			if (myGuiControlLayoutGrid != null)
			{
				foreach (MyGuiControlBase allControl in myGuiControlLayoutGrid.GetAllControls())
				{
					allControl.Visible = false;
					allControl.Enabled = false;
				}
			}
			m_contextMenuLastButton = null;
			if (MySession.Static.LocalCharacter != null && item.UsingCharacters.Contains(MySession.Static.LocalCharacter.EntityId) && MySession.Static.LocalCharacter.Components.TryGet<MyAssetModifierComponent>(out var component))
			{
				component.ResetSlot(item.ItemDefinition.ItemSlot);
			}
			MyLocalCache.SaveInventoryConfig(MySession.Static.LocalCharacter);
		}

		private void OnRefreshClick(MyGuiControlButton obj)
		{
			if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
			{
				m_refreshButton.Enabled = false;
				RotatingWheelShow();
				RefreshItems();
			}
		}

		private void UseItem(MyGameInventoryItem item, byte[] checkData)
		{
			if (MySession.Static.LocalCharacter == null)
			{
				return;
			}
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			item.IsNew = false;
			string assetModifierId = item.ItemDefinition.AssetModifierId;
			m_colorOrModelChanged = true;
			MyAssetModifierComponent component;
			switch (item.ItemDefinition.ItemSlot)
			{
			case MyGameInventoryItemSlot.Face:
			case MyGameInventoryItemSlot.Helmet:
			case MyGameInventoryItemSlot.Gloves:
			case MyGameInventoryItemSlot.Boots:
			case MyGameInventoryItemSlot.Suit:
				if (localCharacter.Components.TryGet<MyAssetModifierComponent>(out component) && (MyFakes.OWN_ALL_ITEMS ? component.TryAddAssetModifier(assetModifierId) : component.TryAddAssetModifier(checkData)))
				{
					item.UsingCharacters.Add(MySession.Static.LocalCharacter.EntityId);
					m_entityController.PlayRandomCharacterAnimation();
				}
				break;
			case MyGameInventoryItemSlot.Rifle:
			case MyGameInventoryItemSlot.Welder:
			case MyGameInventoryItemSlot.Grinder:
			case MyGameInventoryItemSlot.Drill:
			{
				MyEntity myEntity = localCharacter.CurrentWeapon as MyEntity;
				if (myEntity != null && myEntity.Components.TryGet<MyAssetModifierComponent>(out component) && (MyFakes.OWN_ALL_ITEMS ? component.TryAddAssetModifier(assetModifierId) : component.TryAddAssetModifier(checkData)))
				{
					item.UsingCharacters.Add(MySession.Static.LocalCharacter.EntityId);
				}
				break;
			}
			case MyGameInventoryItemSlot.Emote:
			case MyGameInventoryItemSlot.Armor:
				break;
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (base.State != MyGuiScreenState.CLOSING && base.State != MyGuiScreenState.CLOSED)
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFixed);
			}
			if (MyInput.Static.IsNewPrimaryButtonPressed() && m_contextMenu.IsActiveControl && !m_contextMenu.IsMouseOver)
			{
				m_contextMenu.Deactivate();
			}
			if (m_focusButton && hasFocus)
			{
				m_focusButton = false;
				FocusButton(null);
			}
			base.Update(hasFocus);
			if (!m_audioSet && MySandboxGame.IsGameReady && MyAudio.Static != null && MyRenderProxy.VisibleObjectsRead != null && MyRenderProxy.VisibleObjectsRead.get_Count() > 0)
			{
				SetAudioVolumes();
				m_audioSet = true;
			}
			if (m_OkButton != null)
			{
				m_OkButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			if (m_craftButton != null)
			{
				m_craftButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_refreshButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			return true;
		}

		private static void SetAudioVolumes()
		{
			MyAudio.Static.StopMusic();
			MyAudio.Static.ChangeGlobalVolume(1f, 5f);
			if (MyPerGameSettings.UseMusicController && MyFakes.ENABLE_MUSIC_CONTROLLER && MySandboxGame.Config.EnableDynamicMusic && !Sandbox.Engine.Platform.Game.IsDedicated && MyMusicController.Static == null)
			{
				MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
			}
			MyAudio.Static.MusicAllowed = MyMusicController.Static == null;
			if (MyMusicController.Static != null)
			{
				MyMusicController.Static.Active = true;
				return;
			}
			MyAudio.Static.PlayMusic(new MyMusicTrack
			{
				TransitionCategory = MyStringId.GetOrCompute("Default")
			});
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (m_entityController != null)
			{
				bool isMouseOverAnyControl = IsMouseOver() || m_lastHandlingControl != null;
				m_entityController.Update(isMouseOverAnyControl);
				if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.VIEW))
				{
					(m_entityController.GetEntity() as IMyControllableEntity)?.SwitchHelmet();
				}
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.PAGE_DOWN))
			{
				MyGuiControlBase myGuiControlBase = base.FocusedControl;
				bool flag = myGuiControlBase == m_itemsTableParent;
				while (!flag && myGuiControlBase != null)
				{
					if (myGuiControlBase.Owner == m_itemsTableParent)
					{
						flag = true;
						break;
					}
					myGuiControlBase = myGuiControlBase.Owner as MyGuiControlBase;
				}
				if (flag)
				{
					switch (m_activeLowTabState)
					{
					case LowerTabState.Coloring:
						base.FocusedControl = m_modelPicker;
						break;
					case LowerTabState.Recycling:
						base.FocusedControl = m_duplicateCheckboxRecycle;
						break;
					}
				}
			}
			if (receivedFocusInThisUpdate)
			{
				return;
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				if (m_activeLowTabState == LowerTabState.Coloring)
				{
					OnOkClick(null);
				}
				else
				{
					OnCraftClick(null);
				}
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
			{
				OnRefreshClick(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
			{
				SwitchCategory();
			}
			if (!MyPlatformGameSettings.LIMITED_MAIN_MENU && MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
			{
				SwitchAction();
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
			{
				SwitchMainTab();
			}
		}

		public override bool Draw()
		{
			bool result = base.Draw();
			DrawScene();
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
				Vector2 value = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.ToolbarButton).SizeOverride.Value;
				Vector2 positionAbsoluteTopLeft;
				Vector2 vector = (positionAbsoluteTopLeft = m_characterButton.GetPositionAbsoluteTopLeft());
				positionAbsoluteTopLeft.Y += value.Y / 2f;
				positionAbsoluteTopLeft.X -= value.X / 6f;
				Vector2 normalizedCoord = vector;
				normalizedCoord.Y = positionAbsoluteTopLeft.Y;
				Color value2 = MyGuiControlBase.ApplyColorMaskModifiers(MyGuiConstants.LABEL_TEXT_COLOR, enabled: true, m_transitionAlpha);
				normalizedCoord.X += 2f * value.X + value.X / 8f;
				MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Left), positionAbsoluteTopLeft, 1f, value2, drawAlign);
				MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Right), normalizedCoord, 1f, value2, drawAlign);
			}
			return result;
		}

		private void DrawScene()
		{
			if (MySession.Static == null)
			{
				return;
			}
			if ((MySession.Static.CameraController == null || !MySession.Static.CameraController.IsInFirstPersonView) && MyThirdPersonSpectator.Static != null)
			{
				MyThirdPersonSpectator.Static.Update();
			}
			if (MySector.MainCamera != null)
			{
				MySession.Static.CameraController.ControlCamera(MySector.MainCamera);
				MySector.MainCamera.Update(0.0166666675f);
				MySector.MainCamera.UploadViewMatrixToRender();
			}
			MySector.UpdateSunLight();
			MyRenderProxy.UpdateGameplayFrame(MySession.Static.GameplayFrameCounter);
			MyRenderFogSettings myRenderFogSettings = default(MyRenderFogSettings);
			myRenderFogSettings.FogMultiplier = MySector.FogProperties.FogMultiplier;
			myRenderFogSettings.FogColor = MySector.FogProperties.FogColor;
			myRenderFogSettings.FogDensity = MySector.FogProperties.FogDensity / 100f;
			myRenderFogSettings.FogSkybox = MySector.FogProperties.FogSkybox;
			myRenderFogSettings.FogAtmo = MySector.FogProperties.FogAtmo;
			MyRenderFogSettings settings = myRenderFogSettings;
			MyRenderProxy.UpdateFogSettings(ref settings);
			MyRenderPlanetSettings myRenderPlanetSettings = default(MyRenderPlanetSettings);
			myRenderPlanetSettings.AtmosphereIntensityMultiplier = MySector.PlanetProperties.AtmosphereIntensityMultiplier;
			myRenderPlanetSettings.AtmosphereIntensityAmbientMultiplier = MySector.PlanetProperties.AtmosphereIntensityAmbientMultiplier;
			myRenderPlanetSettings.AtmosphereDesaturationFactorForward = MySector.PlanetProperties.AtmosphereDesaturationFactorForward;
			myRenderPlanetSettings.CloudsIntensityMultiplier = MySector.PlanetProperties.CloudsIntensityMultiplier;
			MyRenderPlanetSettings settings2 = myRenderPlanetSettings;
			MyRenderProxy.UpdatePlanetSettings(ref settings2);
			MyRenderProxy.UpdateSSAOSettings(ref MySector.SSAOSettings);
			MyRenderProxy.UpdateHBAOSettings(ref MySector.HBAOSettings);
			MyEnvironmentData data = MySector.SunProperties.EnvironmentData;
			data.Skybox = ((!string.IsNullOrEmpty(MySession.Static.CustomSkybox)) ? MySession.Static.CustomSkybox : MySector.EnvironmentDefinition.EnvironmentTexture);
			data.SkyboxOrientation = MySector.EnvironmentDefinition.EnvironmentOrientation.ToQuaternion();
			data.EnvironmentLight.SunLightDirection = -MySector.SunProperties.SunDirectionNormalized;
			MyRenderProxy.UpdateRenderEnvironment(ref data, MySector.ResetEyeAdaptation);
			MySector.ResetEyeAdaptation = false;
			MyRenderProxy.UpdateEnvironmentMap();
			if (MyVideoSettingsManager.CurrentGraphicsSettings.PostProcessingEnabled != MyPostprocessSettingsWrapper.AllEnabled || MyPostprocessSettingsWrapper.IsDirty)
			{
				if (MyVideoSettingsManager.CurrentGraphicsSettings.PostProcessingEnabled)
				{
					MyPostprocessSettingsWrapper.SetWardrobePostProcessing();
				}
				else
				{
					MyPostprocessSettingsWrapper.ReducePostProcessing();
				}
			}
			MyRenderProxy.SwitchPostprocessSettings(ref MyPostprocessSettingsWrapper.Settings);
			if (MyRenderProxy.SettingsDirty)
			{
				MyRenderProxy.SwitchRenderSettings(MyRenderProxy.Settings);
			}
			MyRenderProxy.Draw3DScene();
			using (Stats.Generic.Measure("GamePrepareDraw"))
			{
				if (MySession.Static != null)
				{
					MySession.Static.Draw();
				}
			}
		}

		protected override void Canceling()
		{
			Cancel();
			base.Canceling();
		}

		protected override void OnHide()
		{
			base.OnHide();
			DrawScene();
		}

		protected override void OnClosed()
		{
			if (MyGameService.IsActive)
			{
				MyGameService.InventoryRefreshed -= MySteamInventory_Refreshed;
			}
			MyGuiControlSlider sliderHue = m_sliderHue;
			sliderHue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderHue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderSaturation = m_sliderSaturation;
			sliderSaturation.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderSaturation.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderValue = m_sliderValue;
			sliderValue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderValue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGameService.CheckItemDataReady -= MyGameService_CheckItemDataReady;
			base.OnClosed();
			if (!m_inGame)
			{
				MyVRage.Platform.Ansel.IsSessionEnabled = false;
			}
			else if (m_savedStateAnselEnabled.HasValue)
			{
				MyVRage.Platform.Ansel.IsSessionEnabled = m_savedStateAnselEnabled.Value;
				m_savedStateAnselEnabled = null;
			}
			MyScreenManager.GetFirstScreenOfType<MyGuiScreenGamePlay>()?.UnhideScreen();
		}

		protected override void OnShow()
		{
			m_savedStateAnselEnabled = MyVRage.Platform.Ansel.IsSessionEnabled;
			MyVRage.Platform.Ansel.IsSessionEnabled = false;
			if (MySector.MainCamera != null && !m_inGame)
			{
				MySector.MainCamera.FieldOfViewDegrees = 55f;
			}
			if (MyGameService.IsActive)
			{
				MyGameService.InventoryRefreshed += MySteamInventory_Refreshed;
			}
			base.OnShow();
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			MyScreenManager.GetFirstScreenOfType<MyGuiScreenIntroVideo>()?.UnhideScreen();
			return base.CloseScreen(isUnloading);
		}

		private void RefreshItems()
		{
			if (MyGameService.IsActive)
			{
				MyGameService.GetAllInventoryItems();
			}
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, MyStringId toolTip, Action<MyGuiControlButton> onClick)
		{
			return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(text), toolTip: MyTexts.GetString(toolTip), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: onClick)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
		}

		private MyGuiControlImageButton MakeImageButton(Vector2 position, Vector2 size, MyGuiControlImageButton.StyleDefinition style, MyStringId toolTip, Action<MyGuiControlImageButton> onClick)
		{
			MyGuiControlImageButton myGuiControlImageButton = new MyGuiControlImageButton("Button", position, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyTexts.GetString(toolTip), null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, onClick);
			if (style != null)
			{
				myGuiControlImageButton.ApplyStyle(style);
			}
			myGuiControlImageButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlImageButton.Size = size;
			return myGuiControlImageButton;
		}

		private void MySteamInventory_Refreshed(object sender, EventArgs e)
		{
			if (m_itemCheckActive)
			{
				m_itemCheckActive = false;
			}
			else
			{
				RefreshUI();
			}
		}

		private void RefreshUI()
		{
			RecreateControls(constructor: false);
			EquipTool();
			UpdateCheckboxes();
		}

		private void RotatingWheelShow()
		{
			m_wheel.ManualRotationUpdate = true;
			m_wheel.Visible = true;
		}

		private void RotatingWheelHide()
		{
			m_wheel.ManualRotationUpdate = false;
			m_wheel.Visible = false;
		}

		private void SwitchCategory()
		{
			m_currentCategoryIndex++;
			if (m_currentCategoryIndex >= m_categoryButtonsData.Count)
			{
				m_currentCategoryIndex = 0;
			}
			List<MyGuiControlBase> controls = m_categoryButtonLayout.GetControls();
			if (m_currentCategoryIndex < controls.Count)
			{
				MyGuiControlImageButton obj = controls[m_currentCategoryIndex] as MyGuiControlImageButton;
				OnCategoryClicked(obj);
			}
			FocusButton(null);
		}

		private void SwitchMainTab()
		{
			if (m_activeTabState == TabState.Character)
			{
				OnViewTools(null);
			}
			else
			{
				OnViewTabCharacter(null);
			}
			FocusButton(null);
		}

		private void SwitchAction()
		{
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				OnViewTabRecycling(null);
			}
			else
			{
				OnViewTabColoring(null);
			}
		}
	}
}
