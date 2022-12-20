using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Collections.ObjectModel;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Linq;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyTerminalProductionController : MyTerminalController
	{
		private enum AssemblerMode
		{
			Assembling,
			Disassembling
		}

		public static readonly int BLUEPRINT_GRID_ROWS = 7;

		public static readonly int QUEUE_GRID_ROWS = 2;

		public static readonly int INVENTORY_GRID_ROWS = 3;

		private static readonly Vector4 ERROR_ICON_COLOR_MASK = new Vector4(1f, 0.5f, 0.5f, 1f);

		private static readonly Vector4 COLOR_MASK_WHITE = Color.White.ToVector4();

		private static readonly MyTimeSpan TIME_EPSILON = MyTimeSpan.FromMilliseconds(35.0);

		private static StringBuilder m_textCache = new StringBuilder();

		private static Dictionary<MyDefinitionId, MyFixedPoint> m_requiredCountCache = new Dictionary<MyDefinitionId, MyFixedPoint>(MyDefinitionId.Comparer);

		private static List<MyBlueprintDefinitionBase.ProductionInfo> m_blueprintCache = new List<MyBlueprintDefinitionBase.ProductionInfo>();

		private IMyGuiControlsParent m_controlsParent;

		private MyGridTerminalSystem m_terminalSystem;

		private Dictionary<int, MyAssembler> m_assemblersByKey = new Dictionary<int, MyAssembler>();

		private int m_assemblerKeyCounter;

		private MyTimeSpan m_productionStartTime;

		private MyGuiControlSearchBox m_blueprintsSearchBox;

		private MyGuiControlCombobox m_comboboxAssemblers;

		private MyGuiControlGrid m_blueprintsGrid;

		private MyAssembler m_selectedAssembler;

		private MyGuiControlRadioButtonGroup m_blueprintButtonGroup = new MyGuiControlRadioButtonGroup();

		private MyGuiControlRadioButtonGroup m_modeButtonGroup = new MyGuiControlRadioButtonGroup();

		private MyGuiControlGrid m_queueGrid;

		private MyGuiControlGrid m_inventoryGrid;

		private MyGuiControlComponentList m_materialsList;

		private MyGuiControlScrollablePanel m_blueprintsArea;

		private MyGuiControlScrollablePanel m_queueArea;

		private MyGuiControlScrollablePanel m_inventoryArea;

		private MyGuiControlBase m_blueprintsBgPanel;

		private MyGuiControlBase m_blueprintsLabel;

		private MyGuiControlCheckbox m_repeatCheckbox;

		private MyGuiControlCheckbox m_slaveCheckbox;

		private MyGuiControlButton m_disassembleAllButton;

		private MyGuiControlButton m_controlPanelButton;

		private MyGuiControlButton m_inventoryButton;

		private MyGuiControlLabel m_materialsLabel;

		private MyDragAndDropInfo m_dragAndDropInfo;

		private MyGuiControlGridDragAndDrop m_dragAndDrop;

		private StringBuilder m_incompleteAssemblerName = new StringBuilder();

		private MyGuiControlRadioButton m_assemblingButton;

		private MyGuiControlRadioButton m_disassemblingButton;

		private int m_queueRemoveStackTimeMs;

		private int m_queueRemoveStackIndex;

		private HashSet<MyDefinitionId> m_blueprintUnbuildabilityCache = new HashSet<MyDefinitionId>();

		private bool m_isAnimationNeeded;

<<<<<<< HEAD
=======
		private bool m_hadItemsInQueueLastUpdate;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private const int QUEUE_REMOVE_STACK_WAIT_MS = 600;

		private MyAssembler SelectedAssembler
		{
			get
			{
				return m_selectedAssembler;
			}
			set
			{
				if (m_selectedAssembler != value)
				{
					m_selectedAssembler = value;
				}
			}
		}

		private AssemblerMode CurrentAssemblerMode => (AssemblerMode)m_modeButtonGroup.SelectedButton.Key;

		public void Init(IMyGuiControlsParent controlsParent, MyCubeGrid grid, MyCubeBlock currentBlock)
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			if (grid == null)
			{
				ShowError(MySpaceTexts.ScreenTerminalError_ShipNotConnected, controlsParent);
				return;
			}
			grid.OnTerminalOpened();
			m_assemblerKeyCounter = 0;
			m_assemblersByKey.Clear();
			Enumerator<MyTerminalBlock> enumerator = grid.GridSystems.TerminalSystem.Blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyAssembler myAssembler = enumerator.get_Current() as MyAssembler;
					if (myAssembler != null && myAssembler.HasLocalPlayerAccess())
					{
						m_assemblersByKey.Add(m_assemblerKeyCounter++, myAssembler);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_controlsParent = controlsParent;
			m_terminalSystem = grid.GridSystems.TerminalSystem;
			m_blueprintsArea = (MyGuiControlScrollablePanel)controlsParent.Controls.GetControlByName("BlueprintsScrollableArea");
			m_blueprintsSearchBox = (MyGuiControlSearchBox)controlsParent.Controls.GetControlByName("BlueprintsSearchBox");
			m_queueArea = (MyGuiControlScrollablePanel)controlsParent.Controls.GetControlByName("QueueScrollableArea");
			m_inventoryArea = (MyGuiControlScrollablePanel)controlsParent.Controls.GetControlByName("InventoryScrollableArea");
			m_blueprintsBgPanel = controlsParent.Controls.GetControlByName("BlueprintsBackgroundPanel");
			m_blueprintsLabel = controlsParent.Controls.GetControlByName("BlueprintsLabel");
			m_comboboxAssemblers = (MyGuiControlCombobox)controlsParent.Controls.GetControlByName("AssemblersCombobox");
			m_blueprintsGrid = (MyGuiControlGrid)m_blueprintsArea.ScrolledControl;
			m_queueGrid = (MyGuiControlGrid)m_queueArea.ScrolledControl;
			m_inventoryGrid = (MyGuiControlGrid)m_inventoryArea.ScrolledControl;
			m_materialsList = (MyGuiControlComponentList)controlsParent.Controls.GetControlByName("MaterialsList");
			m_repeatCheckbox = (MyGuiControlCheckbox)controlsParent.Controls.GetControlByName("RepeatCheckbox");
			m_slaveCheckbox = (MyGuiControlCheckbox)controlsParent.Controls.GetControlByName("SlaveCheckbox");
			m_disassembleAllButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("DisassembleAllButton");
			m_controlPanelButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("ControlPanelButton");
			m_inventoryButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("InventoryButton");
			m_materialsLabel = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("RequiredLabel");
			m_controlPanelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ProductionScreen_TerminalControlScreen));
			m_inventoryButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ProductionScreen_TerminalInventoryScreen));
			m_assemblingButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("AssemblingButton");
			m_assemblingButton.VisualStyle = MyGuiControlRadioButtonStyleEnum.TerminalAssembler;
			m_assemblingButton.Key = 0;
<<<<<<< HEAD
			m_assemblingButton.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			m_disassemblingButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("DisassemblingButton");
			m_disassemblingButton.VisualStyle = MyGuiControlRadioButtonStyleEnum.TerminalAssembler;
			m_disassemblingButton.Key = 1;
			m_disassemblingButton.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			m_modeButtonGroup.Add(m_assemblingButton);
			m_modeButtonGroup.Add(m_disassemblingButton);
			foreach (KeyValuePair<int, MyAssembler> item in m_assemblersByKey.OrderBy(delegate(KeyValuePair<int, MyAssembler> x)
=======
			m_disassemblingButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("DisassemblingButton");
			m_disassemblingButton.VisualStyle = MyGuiControlRadioButtonStyleEnum.TerminalAssembler;
			m_disassemblingButton.Key = 1;
			m_modeButtonGroup.Add(m_assemblingButton);
			m_modeButtonGroup.Add(m_disassemblingButton);
			foreach (KeyValuePair<int, MyAssembler> item in (IEnumerable<KeyValuePair<int, MyAssembler>>)Enumerable.OrderBy<KeyValuePair<int, MyAssembler>, int>((IEnumerable<KeyValuePair<int, MyAssembler>>)m_assemblersByKey, (Func<KeyValuePair<int, MyAssembler>, int>)delegate(KeyValuePair<int, MyAssembler> x)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyAssembler value2 = x.Value;
				return (value2 == currentBlock) ? (-1) : (((!value2.IsFunctional) ? 10000 : 0) + value2.GUIPriority);
			}))
			{
				MyAssembler value = item.Value;
				if (!value.IsFunctional)
				{
					m_incompleteAssemblerName.Clear();
					m_incompleteAssemblerName.AppendStringBuilder(value.CustomName);
					m_incompleteAssemblerName.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockIncomplete));
					m_comboboxAssemblers.AddItem(item.Key, m_incompleteAssemblerName);
				}
				else
				{
					m_comboboxAssemblers.AddItem(item.Key, value.CustomName);
				}
			}
			m_comboboxAssemblers.ItemSelected += Assemblers_ItemSelected;
			m_comboboxAssemblers.SetToolTip(MyTexts.GetString(MySpaceTexts.ProductionScreen_AssemblerList));
			m_comboboxAssemblers.SelectItemByIndex(0);
			m_dragAndDrop = new MyGuiControlGridDragAndDrop(MyGuiConstants.DRAG_AND_DROP_BACKGROUND_COLOR, MyGuiConstants.DRAG_AND_DROP_TEXT_COLOR, 0.7f, MyGuiConstants.DRAG_AND_DROP_TEXT_OFFSET, supportIcon: true);
			controlsParent.Controls.Add(m_dragAndDrop);
			m_dragAndDrop.DrawBackgroundTexture = false;
			m_dragAndDrop.ItemDropped += dragDrop_OnItemDropped;
			m_blueprintsGrid.GamepadHelpTextId = MySpaceTexts.ToolTipTerminalProduction_AddToQueueGamepad;
			RefreshBlueprints();
			Assemblers_ItemSelected();
			RegisterEvents();
			if (m_assemblersByKey.Count == 0)
			{
				ShowError(MySpaceTexts.ScreenTerminalError_NoAssemblers, controlsParent);
			}
			m_queueGrid.GamepadHelpTextId = MySpaceTexts.TerminalProduction_Help_QueueGrid;
		}

		private void UpdateBlueprintClassGui()
		{
			foreach (MyGuiControlRadioButton item in m_blueprintButtonGroup)
			{
				m_controlsParent.Controls.Remove(item);
			}
			m_blueprintButtonGroup.Clear();
			float xOffset = 0f;
			if (SelectedAssembler.BlockDefinition is MyProductionBlockDefinition)
			{
				List<MyBlueprintClassDefinition> blueprintClasses = (SelectedAssembler.BlockDefinition as MyProductionBlockDefinition).BlueprintClasses;
				for (int i = 0; i < blueprintClasses.Count; i++)
				{
					bool selected = i == 0 || blueprintClasses[i].Id.SubtypeName == "Components" || blueprintClasses[i].Id.SubtypeName == "BasicComponents";
					AddBlueprintClassButton(blueprintClasses[i], ref xOffset, selected);
				}
			}
		}

		private void AddBlueprintClassButton(MyBlueprintClassDefinition classDef, ref float xOffset, bool selected = false)
		{
			if (classDef != null)
			{
				MyGuiControlRadioButton myGuiControlRadioButton = new MyGuiControlRadioButton(m_blueprintsLabel.Position + new Vector2(xOffset, m_blueprintsLabel.Size.Y + 0.012f), new Vector2(46f, 46f) / MyGuiConstants.GUI_OPTIMAL_SIZE);
				xOffset += myGuiControlRadioButton.Size.X;
				myGuiControlRadioButton.Icon = new MyGuiHighlightTexture
				{
					Normal = classDef.Icons[0],
					Highlight = classDef.HighlightIcon,
					Focus = classDef.FocusIcon,
					SizePx = new Vector2(46f, 46f)
				};
				myGuiControlRadioButton.UserData = classDef;
				myGuiControlRadioButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				myGuiControlRadioButton.SetTooltip(classDef.DescriptionText);
				m_blueprintButtonGroup.Add(myGuiControlRadioButton);
				m_controlsParent.Controls.Add(myGuiControlRadioButton);
				myGuiControlRadioButton.Selected = selected;
			}
		}

		private static void ShowError(MyStringId errorText, IMyGuiControlsParent controlsParent)
		{
			foreach (MyGuiControlBase control in controlsParent.Controls)
			{
				control.Visible = false;
			}
			MyGuiControlLabel myGuiControlLabel = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("ErrorMessage");
			if (myGuiControlLabel == null)
			{
				myGuiControlLabel = MyGuiScreenTerminal.CreateErrorLabel(errorText, "ErrorMessage");
			}
			myGuiControlLabel.TextEnum = errorText;
			if (!controlsParent.Controls.Contains(myGuiControlLabel))
			{
				controlsParent.Controls.Add(myGuiControlLabel);
			}
		}

		private static void HideError(IMyGuiControlsParent controlsParent)
		{
			controlsParent.Controls.RemoveControlByName("ErrorMessage");
			foreach (MyGuiControlBase control in controlsParent.Controls)
			{
				control.Visible = true;
			}
		}

		private void RegisterEvents()
		{
			foreach (KeyValuePair<int, MyAssembler> item in m_assemblersByKey)
			{
				item.Value.CustomNameChanged += assembler_CustomNameChanged;
			}
			m_terminalSystem.BlockAdded += TerminalSystem_BlockAdded;
			m_terminalSystem.BlockRemoved += TerminalSystem_BlockRemoved;
			m_blueprintButtonGroup.SelectedChanged += blueprintButtonGroup_SelectedChanged;
			m_modeButtonGroup.SelectedChanged += modeButtonGroup_SelectedChanged;
			m_blueprintsSearchBox.OnTextChanged += OnSearchTextChanged;
			m_blueprintsGrid.ItemClicked += blueprintsGrid_ItemClicked;
			m_blueprintsGrid.MouseOverIndexChanged += blueprintsGrid_MouseOverIndexChanged;
			m_blueprintsGrid.ItemSelected += blueprintsGrid_FocusChanged;
			m_blueprintsGrid.ItemAccepted += blueprintsGrid_ItemClicked;
			m_inventoryGrid.ItemClicked += inventoryGrid_ItemClicked;
			m_inventoryGrid.MouseOverIndexChanged += inventoryGrid_MouseOverIndexChanged;
			m_inventoryGrid.ItemSelected += inventoryGrid_FocusChanged;
			m_inventoryGrid.ItemAccepted += inventoryGrid_ItemClicked;
			m_repeatCheckbox.IsCheckedChanged = repeatCheckbox_IsCheckedChanged;
			m_slaveCheckbox.IsCheckedChanged = slaveCheckbox_IsCheckedChanged;
			m_queueGrid.ItemClicked += queueGrid_ItemClicked;
			m_queueGrid.ItemDragged += queueGrid_ItemDragged;
			m_queueGrid.ItemAccepted += queueGrid_ItemClicked;
			m_queueGrid.MouseOverIndexChanged += queueGrid_MouseOverIndexChanged;
			m_queueGrid.ItemSelected += queueGrid_FocusChanged;
			MyGuiControlGrid queueGrid = m_queueGrid;
			queueGrid.ItemControllerAction = (Func<MyGuiControlGrid, int, MyGridItemAction, bool, bool>)Delegate.Combine(queueGrid.ItemControllerAction, new Func<MyGuiControlGrid, int, MyGridItemAction, bool, bool>(queueGrid_ItemControllerAction));
			m_controlPanelButton.ButtonClicked += controlPanelButton_ButtonClicked;
			m_inventoryButton.ButtonClicked += inventoryButton_ButtonClicked;
			m_disassembleAllButton.ButtonClicked += disassembleAllButton_ButtonClicked;
		}

		private bool queueGrid_ItemControllerAction(MyGuiControlGrid sender, int index, MyGridItemAction action, bool pressed)
		{
			if (action == MyGridItemAction.Button_A)
			{
				if (pressed)
				{
					m_queueRemoveStackIndex = index;
					m_queueRemoveStackTimeMs = MySandboxGame.TotalGamePlayTimeInMilliseconds + 600;
					return true;
				}
				m_queueRemoveStackTimeMs = -1;
				bool num = MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED);
				bool flag = MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED);
				int num2 = ((!num) ? 1 : 10) * ((!flag) ? 1 : 100);
				SelectedAssembler.RemoveQueueItemRequest(index, num2);
				return true;
<<<<<<< HEAD
			}
			return false;
		}

		private void UnregisterEvents()
		{
			if (m_controlsParent == null)
			{
				return;
			}
			foreach (KeyValuePair<int, MyAssembler> item in m_assemblersByKey)
			{
				item.Value.CustomNameChanged -= assembler_CustomNameChanged;
			}
			if (m_terminalSystem != null)
			{
				m_terminalSystem.BlockAdded -= TerminalSystem_BlockAdded;
				m_terminalSystem.BlockRemoved -= TerminalSystem_BlockRemoved;
			}
=======
			}
			return false;
		}

		private void UnregisterEvents()
		{
			if (m_controlsParent == null)
			{
				return;
			}
			foreach (KeyValuePair<int, MyAssembler> item in m_assemblersByKey)
			{
				item.Value.CustomNameChanged -= assembler_CustomNameChanged;
			}
			if (m_terminalSystem != null)
			{
				m_terminalSystem.BlockAdded -= TerminalSystem_BlockAdded;
				m_terminalSystem.BlockRemoved -= TerminalSystem_BlockRemoved;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_blueprintButtonGroup.SelectedChanged -= blueprintButtonGroup_SelectedChanged;
			m_modeButtonGroup.SelectedChanged -= modeButtonGroup_SelectedChanged;
			m_blueprintsSearchBox.OnTextChanged -= OnSearchTextChanged;
			m_blueprintsGrid.ItemClicked -= blueprintsGrid_ItemClicked;
			m_blueprintsGrid.MouseOverIndexChanged -= blueprintsGrid_MouseOverIndexChanged;
			m_blueprintsGrid.ItemSelected -= blueprintsGrid_FocusChanged;
			m_blueprintsGrid.ItemAccepted -= blueprintsGrid_ItemClicked;
			m_inventoryGrid.ItemClicked -= inventoryGrid_ItemClicked;
			m_inventoryGrid.MouseOverIndexChanged -= inventoryGrid_MouseOverIndexChanged;
			m_inventoryGrid.ItemSelected -= inventoryGrid_FocusChanged;
			m_inventoryGrid.ItemAccepted -= inventoryGrid_ItemClicked;
			m_repeatCheckbox.IsCheckedChanged = null;
			m_slaveCheckbox.IsCheckedChanged = null;
			m_queueGrid.ItemClicked -= queueGrid_ItemClicked;
			m_queueGrid.ItemDragged -= queueGrid_ItemDragged;
			m_queueGrid.MouseOverIndexChanged -= queueGrid_MouseOverIndexChanged;
			m_queueGrid.ItemSelected -= queueGrid_FocusChanged;
			m_queueGrid.ItemAccepted -= queueGrid_ItemClicked;
			m_controlPanelButton.ButtonClicked -= controlPanelButton_ButtonClicked;
			m_inventoryButton.ButtonClicked -= inventoryButton_ButtonClicked;
			m_disassembleAllButton.ButtonClicked -= disassembleAllButton_ButtonClicked;
		}

		private void RegisterAssemblerEvents(MyAssembler assembler)
		{
			if (assembler != null)
			{
				assembler.CurrentModeChanged += assembler_CurrentModeChanged;
				assembler.QueueChanged += assembler_QueueChanged;
				assembler.CurrentProgressChanged += assembler_CurrentProgressChanged;
				assembler.CurrentStateChanged += assembler_CurrentStateChanged;
				assembler.InputInventory.ContentsChanged += InputInventory_ContentsChanged;
				assembler.OutputInventory.ContentsChanged += OutputInventory_ContentsChanged;
			}
		}

		private void UnregisterAssemblerEvents(MyAssembler assembler)
		{
			if (assembler != null)
			{
				SelectedAssembler.CurrentModeChanged -= assembler_CurrentModeChanged;
				SelectedAssembler.QueueChanged -= assembler_QueueChanged;
				SelectedAssembler.CurrentProgressChanged -= assembler_CurrentProgressChanged;
				SelectedAssembler.CurrentStateChanged -= assembler_CurrentStateChanged;
				if (assembler.InputInventory != null)
				{
					assembler.InputInventory.ContentsChanged -= InputInventory_ContentsChanged;
				}
				if (SelectedAssembler.OutputInventory != null)
				{
					SelectedAssembler.OutputInventory.ContentsChanged -= OutputInventory_ContentsChanged;
				}
			}
		}

		internal void Close()
		{
			UnregisterEvents();
			UnregisterAssemblerEvents(SelectedAssembler);
			m_assemblersByKey.Clear();
			m_blueprintButtonGroup.Clear();
			m_modeButtonGroup.Clear();
			SelectedAssembler = null;
			m_controlsParent = null;
			m_terminalSystem = null;
			m_comboboxAssemblers = null;
			m_dragAndDrop = null;
			m_dragAndDropInfo = null;
		}

		private void SelectAndShowAssembler(MyAssembler assembler)
		{
			UnregisterAssemblerEvents(SelectedAssembler);
			SelectedAssembler = assembler;
			RegisterAssemblerEvents(assembler);
			RefreshRepeatMode(assembler.RepeatEnabled);
			RefreshSlaveMode(assembler.IsSlave);
			SelectModeButton(assembler);
			UpdateBlueprintClassGui();
			m_blueprintsSearchBox.SearchText = string.Empty;
			RefreshQueue(resetScroll: true);
			RefreshInventory();
			RefreshProgress();
			RefreshAssemblerModeView();
		}

		private void RefreshInventory()
		{
			int? selectedIndex = m_inventoryGrid.SelectedIndex;
			m_inventoryGrid.Clear();
			foreach (MyPhysicalInventoryItem item in SelectedAssembler.OutputInventory.GetItems())
			{
				m_inventoryGrid.Add(MyGuiControlInventoryOwner.CreateInventoryGridItem(item));
			}
			int count = SelectedAssembler.OutputInventory.GetItems().Count;
			m_inventoryGrid.RowsCount = Math.Max(1 + count / m_inventoryGrid.ColumnsCount, INVENTORY_GRID_ROWS);
			if (selectedIndex.HasValue)
			{
				m_inventoryGrid.SelectedIndex = Math.Max(Math.Min(selectedIndex.Value, count - 1), 0);
			}
		}

		private void RefreshQueue(bool resetScroll = false)
		{
			float num = m_queueArea.ScrollbarVPosition;
			int? selectedIndex = m_queueGrid.SelectedIndex;
			int num2 = 0;
			for (int i = 0; i < m_queueGrid.Items.Count && m_queueGrid.Items[i] != null; i++)
			{
				num2++;
			}
			m_queueGrid.Clear();
			int num3 = 0;
			foreach (MyProductionBlock.QueueItem item in SelectedAssembler.Queue)
			{
				m_textCache.Clear().Append((int)item.Amount).Append('x');
				MyGuiGridItem myGuiGridItem = new MyGuiGridItem(item.Blueprint.Icons, null, item.Blueprint.DisplayNameText, item);
				myGuiGridItem.AddText(m_textCache, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
				if (MyFakes.SHOW_PRODUCTION_QUEUE_ITEM_IDS)
				{
					m_textCache.Clear().Append((int)item.ItemId);
					myGuiGridItem.AddText(m_textCache, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				}
				m_queueGrid.Add(myGuiGridItem);
				num3++;
			}
			if (num2 == 0 && m_queueGrid.Items.Count > 0 && m_queueGrid.Items[0] != null)
			{
				m_productionStartTime = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			}
			m_queueGrid.RowsCount = Math.Max(1 + num3 / m_queueGrid.ColumnsCount, QUEUE_GRID_ROWS);
			CheckIfAnimationIsNeeded();
			if (m_isAnimationNeeded)
			{
				RefreshProgress();
			}
			if (resetScroll || m_queueGrid.RowsCount <= 2)
			{
				num = 0f;
			}
			if (m_queueArea.ScrollbarVPosition != num)
			{
				m_queueArea.ScrollbarVPosition = num;
			}
			if (selectedIndex.HasValue && m_queueGrid.Items.Count > selectedIndex.Value)
			{
				m_queueGrid.SelectedIndex = selectedIndex;
			}
		}

		private void RefreshBlueprints()
		{
			if (m_blueprintButtonGroup.SelectedButton == null)
			{
				return;
			}
			MyBlueprintClassDefinition myBlueprintClassDefinition = m_blueprintButtonGroup.SelectedButton.UserData as MyBlueprintClassDefinition;
			if (myBlueprintClassDefinition == null)
			{
				return;
			}
			m_blueprintsGrid.Clear();
			bool flag = !string.IsNullOrEmpty(m_blueprintsSearchBox.SearchText);
			string text = (MyInput.Static.IsJoystickLastUsed ? ("\n" + MyTexts.GetString(MySpaceTexts.ToolTipTerminalProduction_ItemInfoGamepad)) : string.Empty);
			int num = 0;
			foreach (MyBlueprintDefinitionBase item2 in myBlueprintClassDefinition)
			{
				if (item2.Public && (!flag || System.StringExtensions.Contains(item2.DisplayNameText, m_blueprintsSearchBox.SearchText, StringComparison.OrdinalIgnoreCase)))
				{
					MyToolTips myToolTips = new MyToolTips();
					myToolTips.AddToolTip(item2.DisplayNameText + text, MyPlatformGameSettings.ITEM_TOOLTIP_SCALE);
					MyGuiGridItem item = new MyGuiGridItem(item2.Icons, null, myToolTips, item2);
					m_blueprintsGrid.Add(item);
					num++;
				}
			}
			m_blueprintsGrid.RowsCount = Math.Max(1 + num / m_blueprintsGrid.ColumnsCount, BLUEPRINT_GRID_ROWS);
			RefreshBlueprintGridColors();
		}

		private void RefreshBlueprintGridColors()
		{
			m_blueprintUnbuildabilityCache.Clear();
			SelectedAssembler.InventoryOwnersDirty = true;
			for (int i = 0; i < m_blueprintsGrid.RowsCount; i++)
			{
				for (int j = 0; j < m_blueprintsGrid.ColumnsCount; j++)
				{
					MyGuiGridItem myGuiGridItem = m_blueprintsGrid.TryGetItemAt(i, j);
					if (myGuiGridItem == null)
					{
						continue;
					}
					MyBlueprintDefinitionBase myBlueprintDefinitionBase = myGuiGridItem.UserData as MyBlueprintDefinitionBase;
					if (myBlueprintDefinitionBase == null)
					{
						continue;
					}
					myGuiGridItem.IconColorMask = Vector4.One;
					if (SelectedAssembler == null)
					{
						continue;
					}
					AddComponentPrerequisites(myBlueprintDefinitionBase, 1, m_requiredCountCache);
					bool flag = false;
					if (CurrentAssemblerMode == AssemblerMode.Assembling)
					{
						foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> item in m_requiredCountCache)
						{
							if (!SelectedAssembler.CheckConveyorResources(item.Value, item.Key))
							{
								myGuiGridItem.IconColorMask = ERROR_ICON_COLOR_MASK;
								flag = true;
								break;
							}
						}
					}
					else if (CurrentAssemblerMode == AssemblerMode.Disassembling && !SelectedAssembler.CheckConveyorResources(null, myBlueprintDefinitionBase.Results[0].Id))
					{
						myGuiGridItem.IconColorMask = ERROR_ICON_COLOR_MASK;
						flag = true;
					}
					m_requiredCountCache.Clear();
					if (flag)
					{
						m_blueprintUnbuildabilityCache.Add(myBlueprintDefinitionBase.Id);
					}
				}
			}
		}

		private void CheckIfAnimationIsNeeded()
		{
			m_isAnimationNeeded = true;
		}

		private void RefreshProgress()
		{
			if (SelectedAssembler == null)
			{
				return;
			}
			int currentItemIndex = SelectedAssembler.CurrentItemIndex;
			MyGuiGridItem myGuiGridItem = m_queueGrid.TryGetItemAt(currentItemIndex);
			bool flag = SelectedAssembler.CubeGrid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId) == MyResourceStateEnum.Ok;
			MyBlueprintDefinitionBase blueprint = null;
			if (myGuiGridItem != null)
			{
				blueprint = ((MyProductionBlock.QueueItem)myGuiGridItem.UserData).Blueprint;
			}
			MyAssembler.StateEnum currentState = SelectedAssembler.GetCurrentState(blueprint);
			bool flag2 = currentState == MyAssembler.StateEnum.Ok;
			bool flag3 = currentState == MyAssembler.StateEnum.Ok || currentState == MyAssembler.StateEnum.MissingItems;
			for (int i = 0; i < m_queueGrid.GetItemsCount(); i++)
			{
				myGuiGridItem = m_queueGrid.TryGetItemAt(i);
				if (myGuiGridItem == null)
				{
					break;
				}
				if (i < currentItemIndex)
				{
					myGuiGridItem.IconColorMask = ERROR_ICON_COLOR_MASK;
					myGuiGridItem.OverlayColorMask = COLOR_MASK_WHITE;
<<<<<<< HEAD
					myGuiGridItem.ToolTip.ToolTips.Clear();
=======
					((Collection<MyColoredText>)(object)myGuiGridItem.ToolTip.ToolTips).Clear();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					myGuiGridItem.ToolTip.AddToolTip(GetAssemblerStateText(MyAssembler.StateEnum.MissingItems), MyPlatformGameSettings.ITEM_TOOLTIP_SCALE, "Red");
				}
				else
				{
					if (i == currentItemIndex || currentItemIndex == -1)
					{
						if (currentItemIndex == -1)
						{
							flag2 = false;
						}
						myGuiGridItem.IconColorMask = (flag2 ? COLOR_MASK_WHITE : ERROR_ICON_COLOR_MASK);
						myGuiGridItem.OverlayColorMask = COLOR_MASK_WHITE;
					}
					else
					{
						myGuiGridItem.IconColorMask = (flag3 ? COLOR_MASK_WHITE : ERROR_ICON_COLOR_MASK);
						myGuiGridItem.OverlayColorMask = COLOR_MASK_WHITE;
					}
<<<<<<< HEAD
					myGuiGridItem.ToolTip.ToolTips.Clear();
=======
					((Collection<MyColoredText>)(object)myGuiGridItem.ToolTip.ToolTips).Clear();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (i == currentItemIndex && flag2)
					{
						MyProductionBlock.QueueItem queueItem = (MyProductionBlock.QueueItem)myGuiGridItem.UserData;
						float currentProgress = SelectedAssembler.CurrentProgress;
						MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
						float currentBlueprintProductionTime = SelectedAssembler.GetCurrentBlueprintProductionTime();
						float num = 0f;
						if (!m_blueprintUnbuildabilityCache.Contains(queueItem.Blueprint.Id))
						{
							MyTimeSpan myTimeSpan2 = SelectedAssembler.GetLastProgressUpdateTime();
							if (myTimeSpan2 < m_productionStartTime - TIME_EPSILON)
							{
								myTimeSpan2 = m_productionStartTime;
							}
							float num2 = ((currentBlueprintProductionTime <= 0f) ? 0f : ((float)(myTimeSpan - myTimeSpan2).Milliseconds / currentBlueprintProductionTime));
							float num3 = currentProgress + (flag ? num2 : 0f);
							int num4 = (int)queueItem.Amount - (int)num3;
							num = ((num4 > 0) ? (num3 % 1f) : 1f);
							myGuiGridItem.ClearText(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
							myGuiGridItem.AddText(Math.Max(num4, 1) + "x", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
						}
						myGuiGridItem.OverlayPercent = num;
<<<<<<< HEAD
						myGuiGridItem.ToolTip.ToolTips.Clear();
						m_textCache.Clear().AppendFormat("{0}: {1}%", queueItem.Blueprint.DisplayNameText, (int)(num * 100f));
						myGuiGridItem.ToolTip.AddToolTip(m_textCache.ToString(), MyPlatformGameSettings.ITEM_TOOLTIP_SCALE);
					}
					MyBlueprintDefinitionBase blueprint2 = ((MyProductionBlock.QueueItem)myGuiGridItem.UserData).Blueprint;
					MyAssembler.StateEnum currentState2 = SelectedAssembler.GetCurrentState(blueprint2);
					if (currentState2 != 0)
					{
						string assemblerStateText = GetAssemblerStateText(currentState2);
						myGuiGridItem.IconColorMask = ERROR_ICON_COLOR_MASK;
						myGuiGridItem.OverlayColorMask = COLOR_MASK_WHITE;
						myGuiGridItem.ToolTip.AddToolTip(assemblerStateText, MyPlatformGameSettings.ITEM_TOOLTIP_SCALE, "Red");
=======
						((Collection<MyColoredText>)(object)myGuiGridItem.ToolTip.ToolTips).Clear();
						m_textCache.Clear().AppendFormat("{0}: {1}%", queueItem.Blueprint.DisplayNameText, (int)(num * 100f));
						myGuiGridItem.ToolTip.AddToolTip(m_textCache.ToString(), MyPlatformGameSettings.ITEM_TOOLTIP_SCALE);
					}
					if (!flag2)
					{
						GetAssemblerStateText(currentState);
						myGuiGridItem.ToolTip.AddToolTip(GetAssemblerStateText(currentState), MyPlatformGameSettings.ITEM_TOOLTIP_SCALE, "Red");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				if (MyInput.Static.IsJoystickLastUsed)
				{
					myGuiGridItem.ToolTip.AddToolTip(MyTexts.GetString(MySpaceTexts.ToolTipTerminalProduction_ProductionQueue_ItemInfoGamepad), MyPlatformGameSettings.ITEM_TOOLTIP_SCALE);
				}
			}
		}

		private void RefreshAssemblerModeView()
		{
			bool flag = CurrentAssemblerMode == AssemblerMode.Assembling;
			bool repeatEnabled = SelectedAssembler.RepeatEnabled;
			m_blueprintsArea.Enabled = true;
			m_blueprintsBgPanel.Enabled = true;
			m_blueprintsLabel.Enabled = true;
			foreach (MyGuiControlRadioButton item in m_blueprintButtonGroup)
			{
				item.Enabled = true;
			}
			m_materialsLabel.Text = (flag ? MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_RequiredAndAvailable) : MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_GainedAndAvailable));
			m_queueGrid.Enabled = flag || !repeatEnabled;
			m_disassembleAllButton.Visible = !flag && !repeatEnabled;
			RefreshBlueprintGridColors();
		}

		private void RefreshRepeatMode(bool repeatModeEnabled)
		{
			if (SelectedAssembler.IsSlave && repeatModeEnabled)
			{
				RefreshSlaveMode(slaveModeEnabled: false);
			}
			SelectedAssembler.CurrentModeChanged -= assembler_CurrentModeChanged;
			SelectedAssembler.RequestRepeatEnabled(repeatModeEnabled);
			SelectedAssembler.CurrentModeChanged += assembler_CurrentModeChanged;
			m_repeatCheckbox.IsCheckedChanged = null;
			m_repeatCheckbox.IsChecked = SelectedAssembler.RepeatEnabled;
			m_repeatCheckbox.IsCheckedChanged = repeatCheckbox_IsCheckedChanged;
			m_repeatCheckbox.Visible = SelectedAssembler.SupportsAdvancedFunctions;
		}

		private void RefreshSlaveMode(bool slaveModeEnabled)
		{
			if (SelectedAssembler.RepeatEnabled && slaveModeEnabled)
			{
				RefreshRepeatMode(repeatModeEnabled: false);
			}
			if (SelectedAssembler.DisassembleEnabled)
			{
				m_slaveCheckbox.Enabled = false;
				m_slaveCheckbox.Visible = false;
			}
			if (!SelectedAssembler.DisassembleEnabled)
			{
				m_slaveCheckbox.Enabled = true;
				m_slaveCheckbox.Visible = true;
			}
			SelectedAssembler.CurrentModeChanged -= assembler_CurrentModeChanged;
			SelectedAssembler.IsSlave = slaveModeEnabled;
			SelectedAssembler.CurrentModeChanged += assembler_CurrentModeChanged;
			m_slaveCheckbox.IsCheckedChanged = null;
			m_slaveCheckbox.IsChecked = SelectedAssembler.IsSlave;
			m_slaveCheckbox.IsCheckedChanged = slaveCheckbox_IsCheckedChanged;
			if (!SelectedAssembler.SupportsAdvancedFunctions)
			{
				m_slaveCheckbox.Visible = false;
			}
		}

		private void EnqueueBlueprint(MyBlueprintDefinitionBase blueprint, MyFixedPoint amount)
		{
			m_blueprintCache.Clear();
			blueprint.GetBlueprints(m_blueprintCache);
			foreach (MyBlueprintDefinitionBase.ProductionInfo item in m_blueprintCache)
			{
				SelectedAssembler.InsertQueueItemRequest(-1, item.Blueprint, item.Amount * amount);
			}
			m_blueprintCache.Clear();
		}

		private void ShowBlueprintComponents(MyBlueprintDefinitionBase blueprint, MyFixedPoint amount)
		{
			m_materialsList.Clear();
			if (blueprint != null)
			{
				AddComponentPrerequisites(blueprint, amount, m_requiredCountCache);
				FillMaterialList(m_requiredCountCache);
				m_requiredCountCache.Clear();
			}
		}

		private void FillMaterialList(Dictionary<MyDefinitionId, MyFixedPoint> materials)
		{
			bool flag = CurrentAssemblerMode == AssemblerMode.Disassembling;
			foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> material in materials)
			{
				MyFixedPoint itemAmount = SelectedAssembler.InventoryAggregate.GetItemAmount(material.Key);
				string font = ((flag || material.Value <= itemAmount) ? "White" : "Red");
				m_materialsList.Add(material.Key, (double)material.Value, (double)itemAmount, font);
			}
		}

		private void AddComponentPrerequisites(MyBlueprintDefinitionBase blueprint, MyFixedPoint multiplier, Dictionary<MyDefinitionId, MyFixedPoint> outputAmounts)
		{
			MyFixedPoint myFixedPoint = (MyFixedPoint)(1f / ((SelectedAssembler != null) ? SelectedAssembler.GetEfficiencyMultiplierForBlueprint(blueprint) : MySession.Static.AssemblerEfficiencyMultiplier));
			MyBlueprintDefinitionBase.Item[] prerequisites = blueprint.Prerequisites;
			for (int i = 0; i < prerequisites.Length; i++)
			{
				MyBlueprintDefinitionBase.Item item = prerequisites[i];
				if (!outputAmounts.ContainsKey(item.Id))
				{
					outputAmounts[item.Id] = 0;
				}
				outputAmounts[item.Id] += item.Amount * multiplier * myFixedPoint;
			}
		}

		private void StartDragging(MyDropHandleType dropHandlingType, MyGuiControlGrid gridControl, ref MyGuiControlGrid.EventArgs args)
		{
			m_dragAndDropInfo = new MyDragAndDropInfo();
			m_dragAndDropInfo.Grid = gridControl;
			m_dragAndDropInfo.ItemIndex = args.ItemIndex;
			MyGuiGridItem itemAt = m_dragAndDropInfo.Grid.GetItemAt(m_dragAndDropInfo.ItemIndex);
			m_dragAndDrop.StartDragging(dropHandlingType, args.Button, itemAt, m_dragAndDropInfo, includeTooltip: false);
		}

		private void SelectModeButton(MyAssembler assembler)
		{
			bool supportsAdvancedFunctions = assembler.SupportsAdvancedFunctions;
			foreach (MyGuiControlRadioButton item in m_modeButtonGroup)
			{
				item.Enabled = supportsAdvancedFunctions;
			}
			AssemblerMode key = (assembler.DisassembleEnabled ? AssemblerMode.Disassembling : AssemblerMode.Assembling);
			m_modeButtonGroup.SelectByKey((int)key);
		}

		private void RefreshMaterialsPreview(bool isFocused)
		{
			int num = 0;
			try
			{
				m_materialsList.Clear();
				num = 1;
				if (isFocused)
<<<<<<< HEAD
				{
					num = 2;
					if (m_blueprintsGrid.SelectedItem != null)
					{
						num = 3;
						ShowBlueprintComponents((MyBlueprintDefinitionBase)m_blueprintsGrid.SelectedItem.UserData, 1);
						num = 4;
					}
					else if (m_inventoryGrid.SelectedItem != null && CurrentAssemblerMode == AssemblerMode.Disassembling)
					{
						num = 5;
						MyPhysicalInventoryItem myPhysicalInventoryItem = (MyPhysicalInventoryItem)m_inventoryGrid.SelectedItem.UserData;
						num = 6;
						if (MyDefinitionManager.Static.HasBlueprint(myPhysicalInventoryItem.Content.GetId()))
						{
							num = 7;
							ShowBlueprintComponents(MyDefinitionManager.Static.GetBlueprintDefinition(myPhysicalInventoryItem.Content.GetId()), 1);
							num = 8;
						}
						num = 9;
					}
					else if (m_queueGrid.SelectedItem != null)
					{
						num = 10;
						MyProductionBlock.QueueItem queueItem = (MyProductionBlock.QueueItem)m_queueGrid.SelectedItem.UserData;
						num = 11;
						ShowBlueprintComponents(queueItem.Blueprint, queueItem.Amount);
						num = 12;
					}
					else if (SelectedAssembler != null)
					{
						num = 13;
						foreach (MyProductionBlock.QueueItem item in SelectedAssembler.Queue)
						{
							num = 14;
							AddComponentPrerequisites(item.Blueprint, item.Amount, m_requiredCountCache);
							num = 15;
						}
						FillMaterialList(m_requiredCountCache);
						num = 16;
					}
					num = 17;
				}
				else
				{
					num = 18;
					if (m_blueprintsGrid.MouseOverItem != null)
					{
						num = 19;
						ShowBlueprintComponents((MyBlueprintDefinitionBase)m_blueprintsGrid.MouseOverItem.UserData, 1);
						num = 20;
					}
					else if (m_inventoryGrid.MouseOverItem != null && CurrentAssemblerMode == AssemblerMode.Disassembling)
					{
						num = 21;
						MyPhysicalInventoryItem myPhysicalInventoryItem2 = (MyPhysicalInventoryItem)m_inventoryGrid.MouseOverItem.UserData;
						num = 22;
						if (MyDefinitionManager.Static.HasBlueprint(myPhysicalInventoryItem2.Content.GetId()))
						{
							num = 23;
							ShowBlueprintComponents(MyDefinitionManager.Static.GetBlueprintDefinition(myPhysicalInventoryItem2.Content.GetId()), 1);
							num = 24;
						}
						num = 25;
					}
					else if (m_queueGrid.MouseOverItem != null)
					{
						num = 26;
						MyProductionBlock.QueueItem queueItem2 = (MyProductionBlock.QueueItem)m_queueGrid.MouseOverItem.UserData;
						num = 27;
						ShowBlueprintComponents(queueItem2.Blueprint, queueItem2.Amount);
						num = 28;
					}
					else if (SelectedAssembler != null)
					{
						num = 29;
						foreach (MyProductionBlock.QueueItem item2 in SelectedAssembler.Queue)
						{
							num = 30;
							AddComponentPrerequisites(item2.Blueprint, item2.Amount, m_requiredCountCache);
							num = 31;
						}
						num = 32;
						FillMaterialList(m_requiredCountCache);
						num = 33;
					}
					num = 34;
				}
=======
				{
					num = 2;
					if (m_blueprintsGrid.SelectedItem != null)
					{
						num = 3;
						ShowBlueprintComponents((MyBlueprintDefinitionBase)m_blueprintsGrid.SelectedItem.UserData, 1);
						num = 4;
					}
					else if (m_inventoryGrid.SelectedItem != null && CurrentAssemblerMode == AssemblerMode.Disassembling)
					{
						num = 5;
						MyPhysicalInventoryItem myPhysicalInventoryItem = (MyPhysicalInventoryItem)m_inventoryGrid.SelectedItem.UserData;
						num = 6;
						if (MyDefinitionManager.Static.HasBlueprint(myPhysicalInventoryItem.Content.GetId()))
						{
							num = 7;
							ShowBlueprintComponents(MyDefinitionManager.Static.GetBlueprintDefinition(myPhysicalInventoryItem.Content.GetId()), 1);
							num = 8;
						}
						num = 9;
					}
					else if (m_queueGrid.SelectedItem != null)
					{
						num = 10;
						MyProductionBlock.QueueItem queueItem = (MyProductionBlock.QueueItem)m_queueGrid.SelectedItem.UserData;
						num = 11;
						ShowBlueprintComponents(queueItem.Blueprint, queueItem.Amount);
						num = 12;
					}
					else if (SelectedAssembler != null)
					{
						num = 13;
						foreach (MyProductionBlock.QueueItem item in SelectedAssembler.Queue)
						{
							num = 14;
							AddComponentPrerequisites(item.Blueprint, item.Amount, m_requiredCountCache);
							num = 15;
						}
						FillMaterialList(m_requiredCountCache);
						num = 16;
					}
					num = 17;
				}
				else
				{
					num = 18;
					if (m_blueprintsGrid.MouseOverItem != null)
					{
						num = 19;
						ShowBlueprintComponents((MyBlueprintDefinitionBase)m_blueprintsGrid.MouseOverItem.UserData, 1);
						num = 20;
					}
					else if (m_inventoryGrid.MouseOverItem != null && CurrentAssemblerMode == AssemblerMode.Disassembling)
					{
						num = 21;
						MyPhysicalInventoryItem myPhysicalInventoryItem2 = (MyPhysicalInventoryItem)m_inventoryGrid.MouseOverItem.UserData;
						num = 22;
						if (MyDefinitionManager.Static.HasBlueprint(myPhysicalInventoryItem2.Content.GetId()))
						{
							num = 23;
							ShowBlueprintComponents(MyDefinitionManager.Static.GetBlueprintDefinition(myPhysicalInventoryItem2.Content.GetId()), 1);
							num = 24;
						}
						num = 25;
					}
					else if (m_queueGrid.MouseOverItem != null)
					{
						num = 26;
						MyProductionBlock.QueueItem queueItem2 = (MyProductionBlock.QueueItem)m_queueGrid.MouseOverItem.UserData;
						num = 27;
						ShowBlueprintComponents(queueItem2.Blueprint, queueItem2.Amount);
						num = 28;
					}
					else if (SelectedAssembler != null)
					{
						num = 29;
						foreach (MyProductionBlock.QueueItem item2 in SelectedAssembler.Queue)
						{
							num = 30;
							AddComponentPrerequisites(item2.Blueprint, item2.Amount, m_requiredCountCache);
							num = 31;
						}
						num = 32;
						FillMaterialList(m_requiredCountCache);
						num = 33;
					}
					num = 34;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				num = 35;
				m_requiredCountCache.Clear();
				num = 36;
			}
			catch
			{
				MyLog.Default.WriteLine("Crash in RefreshMaterialsPreview line " + num);
				throw;
			}
		}

		private static string GetAssemblerStateText(MyAssembler.StateEnum state)
		{
			MyStringId id = MySpaceTexts.Blank;
			switch (state)
			{
			case MyAssembler.StateEnum.Ok:
				id = MySpaceTexts.Blank;
				break;
			case MyAssembler.StateEnum.Disabled:
				id = MySpaceTexts.AssemblerState_Disabled;
				break;
			case MyAssembler.StateEnum.NotWorking:
				id = MySpaceTexts.AssemblerState_NotWorking;
				break;
			case MyAssembler.StateEnum.MissingItems:
				id = MySpaceTexts.AssemblerState_MissingItems;
				break;
			case MyAssembler.StateEnum.NotEnoughPower:
				id = MySpaceTexts.AssemblerState_NotEnoughPower;
				break;
			case MyAssembler.StateEnum.InventoryFull:
				id = MySpaceTexts.AssemblerState_InventoryFull;
				break;
			}
			return MyTexts.GetString(id);
		}

		private void blueprintButtonGroup_SelectedChanged(MyGuiControlRadioButtonGroup obj)
		{
			RefreshBlueprints();
		}

		private void Assemblers_ItemSelected()
		{
			if (m_assemblersByKey.Count > 0 && m_assemblersByKey.ContainsKey((int)m_comboboxAssemblers.GetSelectedKey()))
			{
				SelectAndShowAssembler(m_assemblersByKey[(int)m_comboboxAssemblers.GetSelectedKey()]);
			}
		}

		private void assembler_CurrentModeChanged(MyAssembler assembler)
		{
			SelectModeButton(assembler);
			RefreshRepeatMode(assembler.RepeatEnabled);
			RefreshSlaveMode(assembler.IsSlave);
			RefreshProgress();
			RefreshAssemblerModeView();
			RefreshMaterialsPreview(isFocused: false);
		}

		private void assembler_QueueChanged(MyProductionBlock block)
		{
			RefreshQueue();
			RefreshMaterialsPreview(isFocused: false);
			RefreshProgress();
		}

		private void assembler_CurrentProgressChanged(MyAssembler assembler)
		{
			RefreshProgress();
			CheckIfAnimationIsNeeded();
		}

		private void assembler_CurrentStateChanged(MyAssembler obj)
		{
			RefreshProgress();
			CheckIfAnimationIsNeeded();
		}

		private void InputInventory_ContentsChanged(MyInventoryBase obj)
		{
			if (CurrentAssemblerMode == AssemblerMode.Assembling)
			{
				RefreshBlueprintGridColors();
			}
			RefreshMaterialsPreview(isFocused: false);
		}

		private void OutputInventory_ContentsChanged(MyInventoryBase obj)
		{
			RefreshInventory();
			RefreshMaterialsPreview(isFocused: false);
		}

		private void OnSearchTextChanged(string text)
		{
			RefreshBlueprints();
		}

		private void blueprintsGrid_ItemClicked(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			MyGuiGridItem itemAt = control.GetItemAt(args.ItemIndex);
			if (itemAt != null)
			{
				MyBlueprintDefinitionBase blueprint = (MyBlueprintDefinitionBase)itemAt.UserData;
				int num = 1;
				if (MyInput.Static.IsAnyCtrlKeyPressed() || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED))
				{
					num = ((!MyInput.Static.IsAnyShiftKeyPressed() && !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED)) ? (num * 10) : (num * 1000));
				}
				else if (MyInput.Static.IsAnyShiftKeyPressed() || MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED))
				{
					num *= 100;
				}
				EnqueueBlueprint(blueprint, num);
			}
		}

		private void inventoryGrid_ItemClicked(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			if (CurrentAssemblerMode == AssemblerMode.Assembling)
			{
				return;
			}
			MyGuiGridItem itemAt = control.GetItemAt(args.ItemIndex);
			if (itemAt != null)
			{
				MyPhysicalInventoryItem myPhysicalInventoryItem = (MyPhysicalInventoryItem)itemAt.UserData;
				MyBlueprintDefinitionBase myBlueprintDefinitionBase = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(myPhysicalInventoryItem.Content.GetId());
				if (myBlueprintDefinitionBase != null)
				{
					int num = ((!MyInput.Static.IsAnyShiftKeyPressed()) ? 1 : 100) * ((!MyInput.Static.IsAnyCtrlKeyPressed()) ? 1 : 10);
					EnqueueBlueprint(myBlueprintDefinitionBase, num);
				}
			}
		}

		private void queueGrid_ItemClicked(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			if ((CurrentAssemblerMode != AssemblerMode.Disassembling || !SelectedAssembler.RepeatEnabled) && args.Button == MySharedButtonsEnum.Secondary)
			{
				SelectedAssembler.RemoveQueueItemRequest(args.ItemIndex, -1);
			}
		}

		private void queueGrid_ItemDragged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			StartDragging(MyDropHandleType.MouseRelease, control, ref args);
		}

		private void dragDrop_OnItemDropped(object sender, MyDragAndDropEventArgs eventArgs)
		{
			if (SelectedAssembler != null && eventArgs.DropTo != null && eventArgs.DragFrom.Grid == eventArgs.DropTo.Grid)
			{
				MyProductionBlock.QueueItem queueItem = (MyProductionBlock.QueueItem)eventArgs.Item.UserData;
				SelectedAssembler.MoveQueueItemRequest(queueItem.ItemId, eventArgs.DropTo.ItemIndex);
			}
			m_dragAndDropInfo = null;
		}

		private void blueprintsGrid_MouseOverIndexChanged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			RefreshMaterialsPreview(isFocused: false);
		}

		private void blueprintsGrid_FocusChanged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			RefreshMaterialsPreview(isFocused: true);
		}

		private void inventoryGrid_MouseOverIndexChanged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			if (CurrentAssemblerMode != 0)
			{
				RefreshMaterialsPreview(isFocused: false);
			}
		}

		private void inventoryGrid_FocusChanged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			if (CurrentAssemblerMode != 0)
			{
				RefreshMaterialsPreview(isFocused: true);
			}
		}

		private void queueGrid_MouseOverIndexChanged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			RefreshMaterialsPreview(isFocused: false);
		}

		private void queueGrid_FocusChanged(MyGuiControlGrid control, MyGuiControlGrid.EventArgs args)
		{
			RefreshMaterialsPreview(isFocused: true);
		}

		private void modeButtonGroup_SelectedChanged(MyGuiControlRadioButtonGroup obj)
		{
			SelectedAssembler.CurrentModeChanged -= assembler_CurrentModeChanged;
			bool flag = obj.SelectedButton.Key == 1;
			SelectedAssembler.RequestDisassembleEnabled(flag);
			if (flag)
			{
				m_slaveCheckbox.Enabled = false;
				m_slaveCheckbox.Visible = false;
			}
			if (!flag && SelectedAssembler.SupportsAdvancedFunctions)
			{
				m_slaveCheckbox.Enabled = true;
				m_slaveCheckbox.Visible = true;
			}
			SelectedAssembler.CurrentModeChanged += assembler_CurrentModeChanged;
			m_repeatCheckbox.IsCheckedChanged = null;
			m_repeatCheckbox.IsChecked = SelectedAssembler.RepeatEnabled;
			m_repeatCheckbox.IsCheckedChanged = repeatCheckbox_IsCheckedChanged;
			m_slaveCheckbox.IsCheckedChanged = null;
			m_slaveCheckbox.IsChecked = SelectedAssembler.IsSlave;
			m_slaveCheckbox.IsCheckedChanged = slaveCheckbox_IsCheckedChanged;
			RefreshProgress();
			RefreshAssemblerModeView();
			m_queueArea.ScrollbarVPosition = 0f;
		}

		private void repeatCheckbox_IsCheckedChanged(MyGuiControlCheckbox control)
		{
			RefreshRepeatMode(control.IsChecked);
			RefreshAssemblerModeView();
		}

		private void slaveCheckbox_IsCheckedChanged(MyGuiControlCheckbox control)
		{
			RefreshSlaveMode(control.IsChecked);
			RefreshAssemblerModeView();
		}

		private void controlPanelButton_ButtonClicked(MyGuiControlButton control)
		{
			MyGuiScreenTerminal.SwitchToControlPanelBlock(SelectedAssembler);
		}

		private void inventoryButton_ButtonClicked(MyGuiControlButton control)
		{
			MyGuiScreenTerminal.SwitchToInventory(SelectedAssembler);
		}

		private void TerminalSystem_BlockAdded(MyTerminalBlock obj)
		{
			MyAssembler myAssembler = obj as MyAssembler;
			if (myAssembler != null)
			{
				if (m_assemblersByKey.Count == 0)
				{
					HideError(m_controlsParent);
				}
				int num = m_assemblerKeyCounter++;
				m_assemblersByKey.Add(num, myAssembler);
				m_comboboxAssemblers.AddItem(num, myAssembler.CustomName);
				if (m_assemblersByKey.Count == 1)
				{
					m_comboboxAssemblers.SelectItemByIndex(0);
				}
				myAssembler.CustomNameChanged += assembler_CustomNameChanged;
			}
		}

		private void TerminalSystem_BlockRemoved(MyTerminalBlock obj)
		{
			MyAssembler myAssembler = obj as MyAssembler;
			if (myAssembler == null)
			{
				return;
			}
			myAssembler.CustomNameChanged -= assembler_CustomNameChanged;
			int? num = null;
			foreach (KeyValuePair<int, MyAssembler> item in m_assemblersByKey)
			{
				if (item.Value == myAssembler)
				{
					num = item.Key;
					break;
				}
			}
			if (num.HasValue)
			{
				m_assemblersByKey.Remove(num.Value);
				m_comboboxAssemblers.RemoveItem(num.Value);
			}
			if (myAssembler == SelectedAssembler)
			{
				if (m_assemblersByKey.Count > 0)
				{
					m_comboboxAssemblers.SelectItemByIndex(0);
				}
				else
				{
					ShowError(MySpaceTexts.ScreenTerminalError_NoAssemblers, m_controlsParent);
				}
			}
		}

		private void assembler_CustomNameChanged(MyTerminalBlock block)
		{
			foreach (KeyValuePair<int, MyAssembler> item in m_assemblersByKey)
			{
				if (item.Value == block)
				{
					m_comboboxAssemblers.TryGetItemByKey(item.Key).Value.Clear().AppendStringBuilder(block.CustomName);
				}
			}
		}

		private void disassembleAllButton_ButtonClicked(MyGuiControlButton obj)
		{
			if (CurrentAssemblerMode == AssemblerMode.Disassembling && !SelectedAssembler.RepeatEnabled)
			{
				SelectedAssembler.RequestDisassembleAll();
			}
		}

		public override void UpdateBeforeDraw(MyGuiScreenBase screen)
		{
			base.UpdateBeforeDraw(screen);
			CheckIfAnimationIsNeeded();
			if (m_isAnimationNeeded)
			{
				RefreshProgress();
				if (!Sync.IsServer)
				{
					CheckIfAnimationIsNeeded();
				}
			}
			if (!m_dirtyDraw)
			{
				return;
			}
			if (MyInput.Static.IsJoystickLastUsed)
			{
				m_dirtyDraw = false;
			}
			if (MyInput.Static.IsJoystickLastUsed)
			{
				if (m_assemblingButton != null && m_assemblingButton.Selected)
				{
					screen.GamepadHelpTextId = MySpaceTexts.TerminalProduction_Help_ScreenAssembling;
				}
				else if (m_disassemblingButton != null && m_disassemblingButton.Selected)
				{
					screen.GamepadHelpTextId = MySpaceTexts.TerminalProduction_Help_ScreenDisassembling;
				}
				else
				{
					screen.GamepadHelpTextId = MySpaceTexts.TerminalProduction_Help_Screen;
				}
				MyBlueprintClassDefinition myBlueprintClassDefinition;
				if (m_blueprintButtonGroup.SelectedButton != null && (myBlueprintClassDefinition = m_blueprintButtonGroup.SelectedButton.UserData as MyBlueprintClassDefinition) != null)
				{
					string @string = MyTexts.GetString(MySpaceTexts.TerminalProduction_Help_BlueprintFilter);
					@string = (screen.GamepadHelpText = string.Format(@string, myBlueprintClassDefinition.DisplayNameText));
				}
				screen.UpdateGamepadHelp(screen.FocusedControl);
			}
		}

		public override void HandleInput()
		{
			base.HandleInput();
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_Y) && m_disassemblingButton != null && m_assemblingButton != null && m_disassemblingButton.Enabled)
			{
				if (m_assemblingButton.Selected)
				{
					m_disassemblingButton.Selected = true;
				}
				else if (m_disassemblingButton.Selected)
				{
					m_assemblingButton.Selected = true;
				}
				m_dirtyDraw = true;
			}
			if (SelectedAssembler != null && !m_comboboxAssemblers.IsOpen)
<<<<<<< HEAD
			{
				if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
				{
					MyGuiScreenTerminal.SwitchToInventory(SelectedAssembler);
				}
				if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
				{
					MyGuiScreenTerminal.SwitchToControlPanelBlock(SelectedAssembler);
				}
			}
			if (m_blueprintButtonGroup != null && m_blueprintButtonGroup.Count > 1 && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
			{
				int num = (m_blueprintButtonGroup.SelectedIndex.HasValue ? m_blueprintButtonGroup.SelectedIndex.Value : (-1));
				num++;
				if (num >= m_blueprintButtonGroup.Count)
				{
					num = 0;
				}
				m_blueprintButtonGroup.SelectByIndex(num);
				m_dirtyDraw = true;
			}
			if (m_queueGrid != null && SelectedAssembler != null && m_queueRemoveStackTimeMs != -1 && m_queueRemoveStackTimeMs <= MySandboxGame.TotalGamePlayTimeInMilliseconds)
			{
				m_queueRemoveStackTimeMs = -1;
				if (m_queueGrid.SelectedIndex == m_queueRemoveStackIndex)
				{
					SelectedAssembler.RemoveQueueItemRequest(m_queueGrid.SelectedIndex.Value, -1);
				}
			}
			QueueReorderHandleInput();
			if (m_controlPanelButton != null)
			{
				m_controlPanelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			if (m_inventoryButton != null)
			{
				m_inventoryButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
		}

		private void QueueReorderHandleInput()
		{
			if (m_queueGrid == null || SelectedAssembler == null)
			{
				return;
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_LEFT) && m_queueGrid.HasFocus && m_queueGrid.SelectedIndex.HasValue && m_queueGrid.SelectedItem != null)
			{
=======
			{
				if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
				{
					MyGuiScreenTerminal.SwitchToInventory(SelectedAssembler);
				}
				if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
				{
					MyGuiScreenTerminal.SwitchToControlPanelBlock(SelectedAssembler);
				}
			}
			if (m_blueprintButtonGroup != null && m_blueprintButtonGroup.Count > 1 && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
			{
				int num = (m_blueprintButtonGroup.SelectedIndex.HasValue ? m_blueprintButtonGroup.SelectedIndex.Value : (-1));
				num++;
				if (num >= m_blueprintButtonGroup.Count)
				{
					num = 0;
				}
				m_blueprintButtonGroup.SelectByIndex(num);
				m_dirtyDraw = true;
			}
			if (m_queueGrid != null && SelectedAssembler != null && m_queueRemoveStackTimeMs != -1 && m_queueRemoveStackTimeMs <= MySandboxGame.TotalGamePlayTimeInMilliseconds)
			{
				m_queueRemoveStackTimeMs = -1;
				if (m_queueGrid.SelectedIndex == m_queueRemoveStackIndex)
				{
					SelectedAssembler.RemoveQueueItemRequest(m_queueGrid.SelectedIndex.Value, -1);
				}
			}
			QueueReorderHandleInput();
			if (m_controlPanelButton != null)
			{
				m_controlPanelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			if (m_inventoryButton != null)
			{
				m_inventoryButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
		}

		private void QueueReorderHandleInput()
		{
			if (m_queueGrid == null || SelectedAssembler == null)
			{
				return;
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_LEFT) && m_queueGrid.HasFocus && m_queueGrid.SelectedIndex.HasValue && m_queueGrid.SelectedItem != null)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				int value = m_queueGrid.SelectedIndex.Value;
				MyProductionBlock.QueueItem queueItem = (MyProductionBlock.QueueItem)m_queueGrid.SelectedItem.UserData;
				if (value - 1 >= 0)
				{
					SelectedAssembler.MoveQueueItemRequest(queueItem.ItemId, value - 1);
				}
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_RIGHT) && m_queueGrid.HasFocus && m_queueGrid.SelectedIndex.HasValue && m_queueGrid.SelectedItem != null)
			{
				int value2 = m_queueGrid.SelectedIndex.Value;
				MyProductionBlock.QueueItem queueItem2 = (MyProductionBlock.QueueItem)m_queueGrid.SelectedItem.UserData;
				if (value2 + 1 < m_queueGrid.Items.Count)
				{
					SelectedAssembler.MoveQueueItemRequest(queueItem2.ItemId, value2 + 1);
				}
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_UP) && m_queueGrid.HasFocus && m_queueGrid.SelectedIndex.HasValue && m_queueGrid.SelectedItem != null)
			{
				int value3 = m_queueGrid.SelectedIndex.Value;
				MyProductionBlock.QueueItem queueItem3 = (MyProductionBlock.QueueItem)m_queueGrid.SelectedItem.UserData;
				if (value3 - 5 >= 0)
				{
					SelectedAssembler.MoveQueueItemRequest(queueItem3.ItemId, value3 - 5);
				}
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_DOWN) && m_queueGrid.HasFocus && m_queueGrid.SelectedIndex.HasValue && m_queueGrid.SelectedItem != null)
			{
				int value4 = m_queueGrid.SelectedIndex.Value;
				MyProductionBlock.QueueItem queueItem4 = (MyProductionBlock.QueueItem)m_queueGrid.SelectedItem.UserData;
				if (value4 + 5 < m_queueGrid.Items.Count)
				{
					SelectedAssembler.MoveQueueItemRequest(queueItem4.ItemId, value4 + 5);
				}
			}
		}
	}
}
