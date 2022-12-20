using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Common.ObjectBuilders.Definitions;
=======
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Groups;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyTerminalInventoryController : MyTerminalController
	{
		private struct MyGamepadTransferCollection
		{
			public MyPhysicalInventoryItem Item;

			public MyInventory From;

			public MyInventory To;

			public MyGuiControlGrid ToGrid;

			public int IndexFrom;

			public int IndexTo;
		}

		private struct QueueComponent
		{
			public MyDefinitionId Id;

			public int Count;
		}

		private enum MyBuildPlannerAction
		{
			None,
			DefaultWithdraw,
			WithdrawKeep1,
			WithdrawKeep10,
			AddProduction1,
			AddProduction10
		}

		public static readonly MyTimeSpan TRANSFER_TIMER_TIME = MyTimeSpan.FromMilliseconds(600.0);

		private static int m_persistentRadioSelectionLeft = 0;

		private static int m_persistentRadioSelectionRight = 0;

		private static readonly Vector2 m_controlListFullSize = new Vector2(0.437f, 0.618f);

		private static readonly Vector2 m_controlListSizeWithSearch = new Vector2(0.437f, 0.569f);

		private static readonly Vector2 m_leftControlListPosition = new Vector2(-0.452f, -0.276f);

		private static readonly Vector2 m_rightControlListPosition = new Vector2(0.4555f, -0.276f);

		private static readonly Vector2 m_leftControlListPosWithSearch = new Vector2(-0.452f, -0.227f);

		private static readonly Vector2 m_rightControlListPosWithSearch = new Vector2(0.4555f, -0.227f);

		private bool m_isTransferTimerActive;

<<<<<<< HEAD
		private bool m_isJoystickLastUsed;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MyTimeSpan m_transferTimer;

		private MyGamepadTransferCollection m_transferData;

		private MyGuiControlList m_leftOwnersControl;

		private MyGuiControlRadioButton m_leftSuitButton;

		private MyGuiControlRadioButton m_leftGridButton;

		private MyGuiControlLabel m_leftFilterGamepadHelp;

		private MyGuiControlRadioButton m_leftFilterShipButton;

		private MyGuiControlRadioButton m_leftFilterStorageButton;

		private MyGuiControlRadioButton m_leftFilterSystemButton;

		private MyGuiControlRadioButton m_leftFilterEnergyButton;

		private MyGuiControlRadioButton m_leftFilterAllButton;

		private MyGuiControlRadioButtonGroup m_leftTypeGroup;

		private MyGuiControlRadioButtonGroup m_leftFilterGroup;

		private MyGuiControlList m_rightOwnersControl;

		private MyGuiControlRadioButton m_rightSuitButton;

		private MyGuiControlRadioButton m_rightGridButton;

		private MyGuiControlLabel m_rightFilterGamepadHelp;

		private MyGuiControlRadioButton m_rightFilterShipButton;

		private MyGuiControlRadioButton m_rightFilterStorageButton;

		private MyGuiControlRadioButton m_rightFilterSystemButton;

		private MyGuiControlRadioButton m_rightFilterEnergyButton;

		private MyGuiControlRadioButton m_rightFilterAllButton;

		private MyGuiControlRadioButtonGroup m_rightTypeGroup;

		private MyGuiControlRadioButtonGroup m_rightFilterGroup;

		private MyGuiControlButton m_throwOutButton;

		private MyGuiControlButton m_withdrawButton;

		private MyGuiControlButton m_depositAllButton;

		private MyGuiControlButton m_addToProductionButton;

		private MyGuiControlButton m_selectedToProductionButton;

		private MyDragAndDropInfo m_dragAndDropInfo;

		private MyGuiControlGridDragAndDrop m_dragAndDrop;

		private List<MyGuiControlGrid> m_controlsDisabledWhileDragged;

		private MyEntity m_userAsEntity;

		private MyEntity m_interactedAsEntity;

		private MyEntity m_userAsOwner;

		private MyEntity m_interactedAsOwner;

		private List<MyEntity> m_interactedGridOwners = new List<MyEntity>();

		private List<MyEntity> m_interactedGridOwnersMechanical = new List<MyEntity>();

		private List<IMyConveyorEndpoint> m_reachableInventoryOwners = new List<IMyConveyorEndpoint>();

		private List<MyGridConveyorSystem> m_registeredConveyorSystems = new List<MyGridConveyorSystem>();

		private List<MyGridConveyorSystem> m_registeredConveyorMechanicalSystems = new List<MyGridConveyorSystem>();

		private MyGuiControlInventoryOwner m_focusedOwnerControl;

		private MyGuiControlGrid m_focusedGridControl;

		private MyPhysicalInventoryItem? m_selectedInventoryItem;

		private MyInventory m_selectedInventory;

		private MyInventoryOwnerTypeEnum? m_leftFilterType;

		private MyInventoryOwnerTypeEnum? m_rightFilterType;

		private MyGridColorHelper m_colorHelper;

		private MyGuiControlSearchBox m_searchBoxLeft;

		private MyGuiControlSearchBox m_searchBoxRight;

		private MyGuiControlCheckbox m_hideEmptyLeft;

		private MyGuiControlLabel m_hideEmptyLeftLabel;

		private MyGuiControlCheckbox m_hideEmptyRight;

		private MyGuiControlLabel m_hideEmptyRightLabel;

		private MyGuiControlGrid m_leftFocusedInventory;

		private MyGuiControlGrid m_rightFocusedInventory;

		private Predicate<IMyConveyorEndpoint> m_endpointPredicate;

		private IMyConveyorEndpointBlock m_interactedEndpointBlock;

		public int LeftFilterTypeIndex
		{
			get
			{
				if (MySession.Static != null && MySession.Static.LocalHumanPlayer != null)
				{
					return MySession.Static.LocalHumanPlayer.LeftFilterTypeIndex;
				}
				return 0;
			}
			set
			{
				if (MySession.Static != null && MySession.Static.LocalHumanPlayer != null)
				{
					MySession.Static.LocalHumanPlayer.LeftFilterTypeIndex = value;
				}
			}
		}

		public int RightFilterTypeIndex
		{
			get
			{
				if (MySession.Static != null && MySession.Static.LocalHumanPlayer != null)
				{
					return MySession.Static.LocalHumanPlayer.RightFilterTypeIndex;
				}
				return 0;
			}
			set
			{
				if (MySession.Static != null && MySession.Static.LocalHumanPlayer != null)
				{
					MySession.Static.LocalHumanPlayer.RightFilterTypeIndex = value;
				}
			}
		}

		public MyGuiControlRadioButtonStyleEnum LeftFilter
		{
			get
			{
				if (MySession.Static != null && MySession.Static.LocalHumanPlayer != null)
				{
					return MySession.Static.LocalHumanPlayer.LeftFilter;
				}
				return MyGuiControlRadioButtonStyleEnum.FilterCharacter;
			}
			set
			{
				if (MySession.Static != null && MySession.Static.LocalHumanPlayer != null)
				{
					MySession.Static.LocalHumanPlayer.LeftFilter = value;
				}
			}
		}

		public MyGuiControlRadioButtonStyleEnum RightFilter
		{
			get
			{
				if (MySession.Static != null && MySession.Static.LocalHumanPlayer != null)
				{
					return MySession.Static.LocalHumanPlayer.RightFilter;
				}
				return MyGuiControlRadioButtonStyleEnum.FilterCharacter;
			}
			set
			{
				if (MySession.Static != null && MySession.Static.LocalHumanPlayer != null)
				{
					MySession.Static.LocalHumanPlayer.RightFilter = value;
				}
			}
		}

		private MyGuiControlInventoryOwner FocusedOwnerControl
		{
			get
			{
				return m_focusedOwnerControl;
			}
			set
			{
				if (m_focusedOwnerControl != value)
				{
					m_focusedOwnerControl = value;
<<<<<<< HEAD
				}
			}
		}

		private MyGuiControlGrid FocusedGridControl
		{
			get
			{
				return m_focusedGridControl;
			}
			set
			{
				if (m_focusedGridControl != value)
				{
					m_focusedGridControl = value;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

<<<<<<< HEAD
=======
		private MyGuiControlGrid FocusedGridControl
		{
			get
			{
				return m_focusedGridControl;
			}
			set
			{
				if (m_focusedGridControl != value)
				{
					m_focusedGridControl = value;
				}
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MyGuiControlGrid LeftFocusedInventory
		{
			get
			{
				return m_leftFocusedInventory;
			}
			set
			{
				LeftFocusChanged(value);
			}
		}

		private MyGuiControlGrid RightFocusedInventory
		{
			get
			{
				return m_rightFocusedInventory;
			}
			set
			{
				RightFocusChanged(value);
			}
		}

		private void LeftFocusChanged(MyGuiControlGrid grid)
		{
			if (m_leftFocusedInventory != null && m_leftFocusedInventory != grid)
			{
				m_leftFocusedInventory.BorderSize = 1;
				m_leftFocusedInventory.BorderColor = MyGuiConstants.ACTIVE_BACKGROUND_COLOR;
			}
			m_leftFocusedInventory = grid;
			LeftFocusChangeBorder();
		}

		private void LeftFocusChangeBorder()
		{
			if (m_leftFocusedInventory != null)
			{
				m_leftFocusedInventory.BorderSize = 3;
				m_leftFocusedInventory.BorderColor = (m_leftFocusedInventory.HasFocus ? MyGuiConstants.FOCUS_BACKGROUND_COLOR : MyGuiConstants.ACTIVE_BACKGROUND_COLOR);
			}
		}

		private void RightFocusChanged(MyGuiControlGrid grid)
		{
			if (m_rightFocusedInventory != null && m_rightFocusedInventory != grid)
			{
				m_rightFocusedInventory.BorderSize = 1;
				m_rightFocusedInventory.BorderColor = MyGuiConstants.ACTIVE_BACKGROUND_COLOR;
			}
			m_rightFocusedInventory = grid;
			RightFocusChangeBorder();
		}

		private void RightFocusChangeBorder()
		{
			if (m_rightFocusedInventory != null)
			{
				m_rightFocusedInventory.BorderSize = 3;
				m_rightFocusedInventory.BorderColor = (m_rightFocusedInventory.HasFocus ? MyGuiConstants.FOCUS_BACKGROUND_COLOR : MyGuiConstants.ACTIVE_BACKGROUND_COLOR);
			}
		}

		public MyTerminalInventoryController()
		{
			m_leftTypeGroup = new MyGuiControlRadioButtonGroup();
			m_leftFilterGroup = new MyGuiControlRadioButtonGroup();
			m_rightTypeGroup = new MyGuiControlRadioButtonGroup();
			m_rightFilterGroup = new MyGuiControlRadioButtonGroup();
			m_controlsDisabledWhileDragged = new List<MyGuiControlGrid>();
			m_endpointPredicate = EndpointPredicate;
		}

		public void Refresh()
		{
<<<<<<< HEAD
=======
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyCubeGrid myCubeGrid = ((m_interactedAsEntity != null) ? (m_interactedAsEntity.Parent as MyCubeGrid) : null);
			m_interactedGridOwners.Clear();
			if (myCubeGrid != null)
			{
				Enumerator<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node> enumerator = MyCubeGridGroups.Static.Logical.GetGroup(myCubeGrid).Nodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node current = enumerator.get_Current();
						GetGridInventories(current.NodeData, m_interactedGridOwners, m_interactedAsEntity, MySession.Static.LocalPlayerId);
						current.NodeData.GridSystems.ConveyorSystem.BlockAdded += ConveyorSystem_BlockAdded;
						current.NodeData.GridSystems.ConveyorSystem.BlockRemoved += ConveyorSystem_BlockRemoved;
						m_registeredConveyorSystems.Add(current.NodeData.GridSystems.ConveyorSystem);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			m_interactedGridOwnersMechanical.Clear();
			if (myCubeGrid != null)
			{
				Enumerator<MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node> enumerator2 = MyCubeGridGroups.Static.Mechanical.GetGroup(myCubeGrid).Nodes.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node current2 = enumerator2.get_Current();
						GetGridInventories(current2.NodeData, m_interactedGridOwnersMechanical, m_interactedAsEntity, MySession.Static.LocalPlayerId);
						current2.NodeData.GridSystems.ConveyorSystem.BlockAdded += ConveyorSystemMechanical_BlockAdded;
						current2.NodeData.GridSystems.ConveyorSystem.BlockRemoved += ConveyorSystemMechanical_BlockRemoved;
						m_registeredConveyorMechanicalSystems.Add(current2.NodeData.GridSystems.ConveyorSystem);
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
			m_leftTypeGroup.SelectedIndex = m_persistentRadioSelectionLeft;
			m_rightTypeGroup.SelectedIndex = m_persistentRadioSelectionRight;
			m_leftFilterGroup.SelectedIndex = LeftFilterTypeIndex;
			m_rightFilterGroup.SelectedIndex = RightFilterTypeIndex;
			LeftTypeGroup_SelectedChanged(m_leftTypeGroup);
			RightTypeGroup_SelectedChanged(m_rightTypeGroup);
			SetLeftFilter(m_leftFilterType);
			SetRightFilter(m_rightFilterType);
			SetFilterGamepadHelp(m_leftTypeGroup, m_leftFilterGroup, m_leftFilterGamepadHelp);
			SetFilterGamepadHelp(m_rightTypeGroup, m_rightFilterGroup, m_rightFilterGamepadHelp);
		}

		public void Init(IMyGuiControlsParent controlsParent, MyEntity thisEntity, MyEntity interactedEntity, MyGridColorHelper colorHelper, MyGuiScreenBase screen)
		{
			//IL_092c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0931: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a09: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0e: Unknown result type (might be due to invalid IL or missing references)
			m_userAsEntity = thisEntity;
			m_interactedAsEntity = interactedEntity;
			m_colorHelper = colorHelper;
			screen.FocusChanged += ParentsFocusChanged;
			m_leftOwnersControl = (MyGuiControlList)controlsParent.Controls.GetControlByName("LeftInventory");
			m_rightOwnersControl = (MyGuiControlList)controlsParent.Controls.GetControlByName("RightInventory");
			m_leftSuitButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftSuitButton");
			m_leftGridButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftGridButton");
			m_leftFilterGamepadHelp = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("LeftFilterGamepadHelp");
			m_leftFilterShipButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftFilterShipButton");
			m_leftFilterStorageButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftFilterStorageButton");
			m_leftFilterSystemButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftFilterSystemButton");
			m_leftFilterEnergyButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftFilterEnergyButton");
			m_leftFilterAllButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("LeftFilterAllButton");
			m_rightSuitButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightSuitButton");
			m_rightGridButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightGridButton");
			m_rightFilterGamepadHelp = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("RightFilterGamepadHelp");
			m_rightFilterShipButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightFilterShipButton");
			m_rightFilterStorageButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightFilterStorageButton");
			m_rightFilterSystemButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightFilterSystemButton");
			m_rightFilterEnergyButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightFilterEnergyButton");
			m_rightFilterAllButton = (MyGuiControlRadioButton)controlsParent.Controls.GetControlByName("RightFilterAllButton");
			m_throwOutButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("ThrowOutButton");
			m_withdrawButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("WithdrawButton");
			m_depositAllButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("DepositAllButton");
			m_addToProductionButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("AddToProductionButton");
			m_selectedToProductionButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("SelectedToProductionButton");
			if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.BuildPlanner != null)
			{
				m_withdrawButton.Enabled = MySession.Static.LocalCharacter.BuildPlanner.Count > 0;
				m_addToProductionButton.Enabled = MySession.Static.LocalCharacter.BuildPlanner.Count > 0;
			}
			else
			{
				m_withdrawButton.Enabled = false;
				m_addToProductionButton.Enabled = false;
			}
			m_selectedToProductionButton.Enabled = false;
			m_hideEmptyLeft = (MyGuiControlCheckbox)controlsParent.Controls.GetControlByName("CheckboxHideEmptyLeft");
			m_hideEmptyLeftLabel = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("LabelHideEmptyLeft");
			m_hideEmptyRight = (MyGuiControlCheckbox)controlsParent.Controls.GetControlByName("CheckboxHideEmptyRight");
			m_hideEmptyRightLabel = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("LabelHideEmptyRight");
			m_searchBoxLeft = (MyGuiControlSearchBox)controlsParent.Controls.GetControlByName("BlockSearchLeft");
			m_searchBoxRight = (MyGuiControlSearchBox)controlsParent.Controls.GetControlByName("BlockSearchRight");
			m_hideEmptyLeft.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_HideEmpty);
			m_hideEmptyRight.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_HideEmpty);
			m_hideEmptyLeft.Visible = false;
			m_hideEmptyLeftLabel.Visible = false;
			m_hideEmptyRight.Visible = true;
			m_hideEmptyRightLabel.Visible = true;
			m_searchBoxLeft.Visible = false;
			m_searchBoxRight.Visible = false;
			MyGuiControlCheckbox hideEmptyLeft = m_hideEmptyLeft;
			hideEmptyLeft.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(hideEmptyLeft.IsCheckedChanged, new Action<MyGuiControlCheckbox>(HideEmptyLeft_Checked));
			MyGuiControlCheckbox hideEmptyRight = m_hideEmptyRight;
			hideEmptyRight.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(hideEmptyRight.IsCheckedChanged, new Action<MyGuiControlCheckbox>(HideEmptyRight_Checked));
			m_searchBoxLeft.OnTextChanged += BlockSearchLeft_TextChanged;
			m_searchBoxRight.OnTextChanged += BlockSearchRight_TextChanged;
			m_leftSuitButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowCharacter);
			m_leftGridButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowConnected);
			m_leftGridButton.ShowTooltipWhenDisabled = true;
			m_rightSuitButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowInteracted);
			m_rightGridButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowConnected);
			m_rightGridButton.ShowTooltipWhenDisabled = true;
			m_leftFilterAllButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterAll);
			m_leftFilterEnergyButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterEnergy);
			m_leftFilterShipButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterShip);
			m_leftFilterStorageButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterStorage);
			m_leftFilterSystemButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterSystem);
			m_rightFilterAllButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterAll);
			m_rightFilterEnergyButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterEnergy);
			m_rightFilterShipButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterShip);
			m_rightFilterStorageButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterStorage);
			m_rightFilterSystemButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_FilterSystem);
			m_throwOutButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ThrowOut);
			m_throwOutButton.ShowTooltipWhenDisabled = true;
			m_throwOutButton.CueEnum = GuiSounds.None;
			MyControl gameControl = MyInput.Static.GetGameControl(MyControlsSpace.BUILD_PLANNER);
			m_withdrawButton.SetToolTip(string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_Withdraw).ToString(), gameControl));
			m_withdrawButton.ShowTooltipWhenDisabled = true;
			m_withdrawButton.CueEnum = GuiSounds.None;
			m_withdrawButton.DrawCrossTextureWhenDisabled = false;
			m_depositAllButton.SetToolTip(string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_Deposit).ToString(), gameControl));
			m_depositAllButton.ShowTooltipWhenDisabled = true;
			m_depositAllButton.CueEnum = GuiSounds.None;
			m_depositAllButton.DrawCrossTextureWhenDisabled = false;
			m_addToProductionButton.SetToolTip(string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_AddComponents).ToString(), gameControl));
			m_addToProductionButton.ShowTooltipWhenDisabled = true;
			m_addToProductionButton.CueEnum = GuiSounds.None;
			m_addToProductionButton.DrawCrossTextureWhenDisabled = false;
			m_selectedToProductionButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_AddSelectedComponent);
			m_selectedToProductionButton.ShowTooltipWhenDisabled = true;
			m_selectedToProductionButton.CueEnum = GuiSounds.None;
			m_selectedToProductionButton.DrawCrossTextureWhenDisabled = false;
			m_leftTypeGroup.Add(m_leftSuitButton);
			m_leftTypeGroup.Add(m_leftGridButton);
			m_rightTypeGroup.Add(m_rightSuitButton);
			m_rightTypeGroup.Add(m_rightGridButton);
			m_leftFilterGroup.Add(m_leftFilterAllButton);
			m_leftFilterGroup.Add(m_leftFilterEnergyButton);
			m_leftFilterGroup.Add(m_leftFilterShipButton);
			m_leftFilterGroup.Add(m_leftFilterStorageButton);
			m_leftFilterGroup.Add(m_leftFilterSystemButton);
			m_rightFilterGroup.Add(m_rightFilterAllButton);
			m_rightFilterGroup.Add(m_rightFilterEnergyButton);
			m_rightFilterGroup.Add(m_rightFilterShipButton);
			m_rightFilterGroup.Add(m_rightFilterStorageButton);
			m_rightFilterGroup.Add(m_rightFilterSystemButton);
			m_throwOutButton.DrawCrossTextureWhenDisabled = false;
			m_dragAndDrop = new MyGuiControlGridDragAndDrop(MyGuiConstants.DRAG_AND_DROP_BACKGROUND_COLOR, MyGuiConstants.DRAG_AND_DROP_TEXT_COLOR, 0.7f, MyGuiConstants.DRAG_AND_DROP_TEXT_OFFSET, supportIcon: true);
			controlsParent.Controls.Add(m_dragAndDrop);
			m_dragAndDrop.DrawBackgroundTexture = false;
			m_throwOutButton.ButtonClicked += ThrowOutButton_OnButtonClick;
			m_withdrawButton.ButtonClicked += WithdrawButton_ButtonClicked;
			m_depositAllButton.ButtonClicked += DepositAllButton_ButtonClicked;
			m_addToProductionButton.ButtonClicked += AddToProductionButton_ButtonClicked;
			m_selectedToProductionButton.ButtonClicked += selectedToProductionButton_ButtonClicked;
			m_dragAndDrop.ItemDropped += dragDrop_OnItemDropped;
			MyEntity myEntity = ((m_userAsEntity != null && m_userAsEntity.HasInventory) ? m_userAsEntity : null);
			if (myEntity != null)
			{
				m_userAsOwner = myEntity;
			}
			MyEntity myEntity2 = ((m_interactedAsEntity != null && m_interactedAsEntity.HasInventory) ? m_interactedAsEntity : null);
			if (myEntity2 != null)
			{
				m_interactedAsOwner = myEntity2;
			}
			MyCubeGrid myCubeGrid = ((m_interactedAsEntity != null) ? (m_interactedAsEntity.Parent as MyCubeGrid) : null);
			m_interactedGridOwners.Clear();
			if (myCubeGrid != null)
			{
				Enumerator<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node> enumerator = MyCubeGridGroups.Static.Logical.GetGroup(myCubeGrid).Nodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node current = enumerator.get_Current();
						GetGridInventories(current.NodeData, m_interactedGridOwners, m_interactedAsEntity, MySession.Static.LocalPlayerId);
						current.NodeData.GridSystems.ConveyorSystem.BlockAdded += ConveyorSystem_BlockAdded;
						current.NodeData.GridSystems.ConveyorSystem.BlockRemoved += ConveyorSystem_BlockRemoved;
						m_registeredConveyorSystems.Add(current.NodeData.GridSystems.ConveyorSystem);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			m_interactedGridOwnersMechanical.Clear();
			if (myCubeGrid != null)
			{
				Enumerator<MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node> enumerator2 = MyCubeGridGroups.Static.Mechanical.GetGroup(myCubeGrid).Nodes.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node current2 = enumerator2.get_Current();
						GetGridInventories(current2.NodeData, m_interactedGridOwnersMechanical, m_interactedAsEntity, MySession.Static.LocalPlayerId);
						current2.NodeData.GridSystems.ConveyorSystem.BlockAdded += ConveyorSystemMechanical_BlockAdded;
						current2.NodeData.GridSystems.ConveyorSystem.BlockRemoved += ConveyorSystemMechanical_BlockRemoved;
						m_registeredConveyorMechanicalSystems.Add(current2.NodeData.GridSystems.ConveyorSystem);
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
			if (m_interactedAsEntity is MyCharacter || m_interactedAsEntity is MyInventoryBagEntity)
			{
				m_persistentRadioSelectionRight = 0;
			}
			if (LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterCharacter)
			{
				m_leftTypeGroup.SelectedIndex = 0;
			}
			else if (LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
			{
				m_leftTypeGroup.SelectedIndex = 1;
			}
<<<<<<< HEAD
			if (m_interactedAsEntity is MyInventoryBagEntity)
			{
				RightFilter = MyGuiControlRadioButtonStyleEnum.FilterCharacter;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (RightFilter == MyGuiControlRadioButtonStyleEnum.FilterCharacter)
			{
				m_rightTypeGroup.SelectedIndex = 0;
			}
			else if (RightFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
			{
				m_rightTypeGroup.SelectedIndex = 1;
			}
			LeftTypeGroup_SelectedChanged(m_leftTypeGroup);
			RightTypeGroup_SelectedChanged(m_rightTypeGroup);
			m_leftFilterGroup.SelectByIndex(LeftFilterTypeIndex);
			m_rightFilterGroup.SelectByIndex(RightFilterTypeIndex);
			m_leftTypeGroup.SelectedChanged += LeftTypeGroup_SelectedChanged;
			m_rightTypeGroup.SelectedChanged += RightTypeGroup_SelectedChanged;
			m_leftFilterAllButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					LeftFilterTypeIndex = 0;
					SetLeftFilter(null);
				}
			};
			m_leftFilterEnergyButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					LeftFilterTypeIndex = 1;
					SetLeftFilter(MyInventoryOwnerTypeEnum.Energy);
				}
			};
			m_leftFilterStorageButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					LeftFilterTypeIndex = 3;
					SetLeftFilter(MyInventoryOwnerTypeEnum.Storage);
				}
			};
			m_leftFilterSystemButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					LeftFilterTypeIndex = 4;
					SetLeftFilter(MyInventoryOwnerTypeEnum.System);
				}
			};
			m_leftFilterShipButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					LeftFilterTypeIndex = 2;
					SetLeftFilter(null);
				}
			};
			m_rightFilterAllButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					RightFilterTypeIndex = 0;
					SetRightFilter(null);
				}
			};
			m_rightFilterEnergyButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					RightFilterTypeIndex = 1;
					SetRightFilter(MyInventoryOwnerTypeEnum.Energy);
				}
			};
			m_rightFilterStorageButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					RightFilterTypeIndex = 3;
					SetRightFilter(MyInventoryOwnerTypeEnum.Storage);
				}
			};
			m_rightFilterSystemButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					RightFilterTypeIndex = 4;
					SetRightFilter(MyInventoryOwnerTypeEnum.System);
				}
			};
			m_rightFilterShipButton.SelectedChanged += delegate(MyGuiControlRadioButton button)
			{
				if (button.Selected)
				{
					RightFilterTypeIndex = 2;
					SetRightFilter(null);
				}
			};
			m_leftFilterGroup.SelectByIndex(LeftFilterTypeIndex);
			m_rightFilterGroup.SelectByIndex(RightFilterTypeIndex);
			if (m_interactedAsEntity == null)
			{
				m_persistentRadioSelectionLeft = 0;
				m_leftGridButton.Enabled = false;
				m_leftGridButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowConnectedDisabled);
				m_rightGridButton.Enabled = false;
				m_rightGridButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ShowConnectedDisabled);
			}
			RefreshSelectedInventoryItem();
		}

		private void ParentsFocusChanged(MyGuiControlBase prev, MyGuiControlBase next)
		{
			LeftFocusChangeBorder();
			RightFocusChangeBorder();
		}

		private void selectedToProductionButton_ButtonClicked(MyGuiControlButton obj)
		{
			if (m_selectedInventoryItem.HasValue && m_interactedAsEntity != null)
			{
				Queue<QueueComponent> obj2 = new Queue<QueueComponent>();
				int count = 1;
				if (MyInput.Static.IsJoystickLastUsed)
				{
					count = (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED) ? ((!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED)) ? 10 : 1000) : ((!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED)) ? 1 : 100));
				}
				else if (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed())
				{
					count = 100;
				}
				else if (MyInput.Static.IsAnyCtrlKeyPressed())
				{
					count = 10;
				}
				QueueComponent queueComponent = new QueueComponent
				{
					Id = m_selectedInventoryItem.Value.Content.GetId(),
					Count = count
				};
				obj2.Enqueue(queueComponent);
				AddComponentsToProduction(obj2, m_interactedAsEntity);
			}
		}

		private static bool FilterAssemblerFunc(Sandbox.ModAPI.IMyTerminalBlock block)
		{
			if (!(block is Sandbox.ModAPI.IMyAssembler))
			{
				return false;
			}
			if (block != null && !block.IsWorking)
			{
				return false;
			}
			MyEntity myEntity = block as MyEntity;
			if (myEntity == null || !myEntity.HasInventory)
			{
				return false;
			}
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = block.GetUserRelationToOwner(MySession.Static.LocalPlayerId);
			if (userRelationToOwner != MyRelationsBetweenPlayerAndBlock.Owner && userRelationToOwner != MyRelationsBetweenPlayerAndBlock.FactionShare && userRelationToOwner != 0)
			{
				return false;
			}
			return true;
		}

		private static int SortAssemberBlockFunc(Sandbox.ModAPI.IMyTerminalBlock x, Sandbox.ModAPI.IMyTerminalBlock y)
		{
			MyAssemblerDefinition myAssemblerDefinition = x.SlimBlock?.BlockDefinition as MyAssemblerDefinition;
			if (myAssemblerDefinition == null)
			{
				return 0;
			}
			return (y.SlimBlock?.BlockDefinition as MyAssemblerDefinition)?.AssemblySpeed.CompareTo(myAssemblerDefinition.AssemblySpeed) ?? 1;
		}

		private void AddToProductionButton_ButtonClicked(MyGuiControlButton obj)
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyBuildPlannerAction myBuildPlannerAction = MyBuildPlannerAction.None;
				myBuildPlannerAction = ((!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED)) ? MyBuildPlannerAction.AddProduction1 : MyBuildPlannerAction.AddProduction10);
				int num = 0;
				switch (myBuildPlannerAction)
				{
				case MyBuildPlannerAction.AddProduction1:
				{
					int value2 = 1;
					if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
					{
						num = AddComponentsToProduction(m_interactedAsEntity, value2);
					}
					else
					{
						MyGuiScreenGamePlay.ShowEmptyBuildPlannerNotification();
					}
					break;
				}
				case MyBuildPlannerAction.AddProduction10:
				{
					int value = 10;
					if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
					{
						num = AddComponentsToProduction(m_interactedAsEntity, value);
					}
					else
					{
						MyGuiScreenGamePlay.ShowEmptyBuildPlannerNotification();
					}
					break;
				}
				}
				if (num > 0)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: new StringBuilder(string.Format(MyTexts.GetString(MySpaceTexts.NotificationPutToProductionFailed), num))));
				}
				return;
			}
			int num2 = 1;
			if (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed())
			{
				num2 = 100;
			}
			else if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				num2 = 10;
			}
<<<<<<< HEAD
			Queue<QueueComponent> queue = new Queue<QueueComponent>();
=======
			Queue<QueueComponent> val = new Queue<QueueComponent>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			while (num2 > 0)
			{
				foreach (MyIdentity.BuildPlanItem item in MySession.Static.LocalCharacter.BuildPlanner)
				{
					foreach (MyIdentity.BuildPlanItem.Component component in item.Components)
					{
						if (component.ComponentDefinition != null)
						{
							val.Enqueue(new QueueComponent
							{
								Id = component.ComponentDefinition.Id,
								Count = component.Count
							});
						}
					}
				}
				num2--;
			}
<<<<<<< HEAD
			int num3 = AddComponentsToProduction(queue, m_interactedAsEntity);
=======
			int num3 = AddComponentsToProduction(val, m_interactedAsEntity);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (num3 > 0)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: new StringBuilder(string.Format(MyTexts.GetString(MySpaceTexts.NotificationPutToProductionFailed), num3))));
			}
		}

		public static int AddComponentsToProduction(MyEntity interactedEntity, int? persistentMultiple)
		{
			Queue<QueueComponent> val = new Queue<QueueComponent>();
			foreach (MyIdentity.BuildPlanItem item in MySession.Static.LocalCharacter.BuildPlanner)
			{
				for (int num = ((!persistentMultiple.HasValue) ? 1 : persistentMultiple.Value); num > 0; num--)
				{
					foreach (MyIdentity.BuildPlanItem.Component component in item.Components)
					{
						if (component.ComponentDefinition != null)
						{
<<<<<<< HEAD
							queue.Enqueue(new QueueComponent
=======
							val.Enqueue(new QueueComponent
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								Id = component.ComponentDefinition.Id,
								Count = component.Count
							});
						}
					}
				}
			}
			return AddComponentsToProduction(val, interactedEntity);
		}

		private static int AddComponentsToProduction(Queue<QueueComponent> queuedComponents, MyEntity interactedEntity)
		{
			if (interactedEntity == null)
			{
				return 0;
			}
			MyCubeGrid myCubeGrid = interactedEntity.Parent as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return 0;
			}
			MyGridTerminalSystem terminalSystem = myCubeGrid.GridSystems.TerminalSystem;
			int num = 0;
			List<Sandbox.ModAPI.IMyTerminalBlock> list = new List<Sandbox.ModAPI.IMyTerminalBlock>();
			((Sandbox.ModAPI.IMyGridTerminalSystem)terminalSystem).GetBlocksOfType<Sandbox.ModAPI.IMyTerminalBlock>(list, (Func<Sandbox.ModAPI.IMyTerminalBlock, bool>)FilterAssemblerFunc);
			List<Sandbox.ModAPI.IMyAssembler> list2 = Enumerable.ToList<Sandbox.ModAPI.IMyAssembler>(Enumerable.Cast<Sandbox.ModAPI.IMyAssembler>((IEnumerable)list));
			list2.SortNoAlloc(SortAssemberBlockFunc);
			queuedComponents.get_Count();
			while (queuedComponents.get_Count() > 0)
			{
				QueueComponent queueComponent = queuedComponents.Dequeue();
				bool flag = false;
				foreach (Sandbox.ModAPI.IMyAssembler item in list2)
				{
					if (item.Mode != MyAssemblerMode.Disassembly && item.UseConveyorSystem && !item.CooperativeMode)
					{
						Sandbox.ModAPI.IMyProductionBlock myProductionBlock = item;
						MyBlueprintDefinitionBase myBlueprintDefinitionBase = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(queueComponent.Id);
						if (myBlueprintDefinitionBase != null && myProductionBlock.CanUseBlueprint(myBlueprintDefinitionBase))
						{
							myProductionBlock.AddQueueItem(myBlueprintDefinitionBase, queueComponent.Count);
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					num++;
				}
			}
			return num;
		}

		private MyInventory[] GetSourceInventories()
		{
			ObservableCollection<MyGuiControlBase>.Enumerator enumerator = m_leftOwnersControl.Controls.GetEnumerator();
			List<MyInventory> list = new List<MyInventory>();
			MyGuiControlInventoryOwner myGuiControlInventoryOwner = null;
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.Visible)
				{
					continue;
				}
				myGuiControlInventoryOwner = enumerator.Current as MyGuiControlInventoryOwner;
				if (myGuiControlInventoryOwner == null || !myGuiControlInventoryOwner.Enabled)
				{
					continue;
				}
				MyEntity inventoryOwner = myGuiControlInventoryOwner.InventoryOwner;
				for (int i = 0; i < inventoryOwner.InventoryCount; i++)
				{
					MyInventory inventory = inventoryOwner.GetInventory(i);
					if (inventory != null)
					{
						list.Add(inventory);
					}
				}
			}
			return list.ToArray();
		}

		private void DepositAllButton_ButtonClicked(MyGuiControlButton obj)
		{
			int num = depositAllFrom(GetSourceInventories(), m_interactedAsEntity, GetAvailableInventories);
			if (num > 0)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: new StringBuilder(string.Format(MyTexts.GetString(MySpaceTexts.NotificationDepositFailed), num))));
			}
		}

		public static int DepositAll(MyInventory srcInventory, MyEntity interactedEntity)
		{
			return depositAllFrom(new MyInventory[1] { srcInventory }, interactedEntity, GetAvailableInventoriesStatic);
		}

		private static bool ShouldStoreMagazine(MyObjectBuilder_AmmoMagazine magazine)
		{
			if (magazine.SubtypeName == "NATO_25x184mm")
			{
				return true;
			}
			return false;
		}

		private static int depositAllFrom(MyInventory[] srcInventories, MyEntity interactedEntity, Action<MyEntity, MyDefinitionId, List<MyInventory>, MyEntity, bool> getInventoriesMethod)
		{
			int num = 0;
			Dictionary<MyInventory, Dictionary<MyDefinitionId, MyFixedPoint>> dictionary = new Dictionary<MyInventory, Dictionary<MyDefinitionId, MyFixedPoint>>();
			MyInventory[] array = srcInventories;
			foreach (MyInventory myInventory in array)
			{
				foreach (MyPhysicalInventoryItem item in myInventory.GetItems())
				{
					MyObjectBuilder_AmmoMagazine magazine;
					if (item.Content is MyObjectBuilder_Ore || item.Content is MyObjectBuilder_Ingot || item.Content is MyObjectBuilder_Component || ((magazine = item.Content as MyObjectBuilder_AmmoMagazine) != null && ShouldStoreMagazine(magazine)))
					{
						if (!dictionary.ContainsKey(myInventory))
						{
							dictionary[myInventory] = new Dictionary<MyDefinitionId, MyFixedPoint>();
						}
						if (!dictionary[myInventory].ContainsKey(item.Content.GetId()))
						{
							dictionary[myInventory][item.Content.GetId()] = 0;
						}
						dictionary[myInventory][item.Content.GetId()] += item.Amount;
					}
				}
			}
			array = srcInventories;
			foreach (MyInventory myInventory2 in array)
			{
				if (!dictionary.ContainsKey(myInventory2))
<<<<<<< HEAD
				{
					continue;
				}
				Dictionary<MyInventory, MyFixedPoint> dictionary2 = new Dictionary<MyInventory, MyFixedPoint>();
				List<MyInventory> list = new List<MyInventory>();
				foreach (MyDefinitionId item2 in dictionary[myInventory2].Keys.ToList())
				{
=======
				{
					continue;
				}
				Dictionary<MyInventory, MyFixedPoint> dictionary2 = new Dictionary<MyInventory, MyFixedPoint>();
				List<MyInventory> list = new List<MyInventory>();
				foreach (MyDefinitionId item2 in Enumerable.ToList<MyDefinitionId>((IEnumerable<MyDefinitionId>)dictionary[myInventory2].Keys))
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					getInventoriesMethod(myInventory2.Owner, item2, list, interactedEntity, arg5: false);
					if (list.Count == 0)
					{
						num++;
						continue;
					}
					MyFixedPoint myFixedPoint = 0;
					foreach (MyInventory item3 in list)
					{
						if (myInventory2 == item3)
						{
							continue;
						}
						if (!dictionary2.ContainsKey(item3))
						{
							dictionary2.Add(item3, item3.MaxVolume - item3.CurrentVolume);
						}
						MyFixedPoint myFixedPoint2 = dictionary2[item3];
						if (myFixedPoint2 == 0)
						{
							continue;
						}
						MyInventory.GetItemVolumeAndMass(item2, out var _, out var itemVolume);
						MyFixedPoint myFixedPoint3 = dictionary[myInventory2][item2] * itemVolume;
						MyFixedPoint myFixedPoint4 = myFixedPoint3;
						MyFixedPoint myFixedPoint5 = dictionary[myInventory2][item2];
						if (myFixedPoint2 < myFixedPoint3)
						{
							myFixedPoint4 = myFixedPoint2;
							MyInventoryItemAdapter @static = MyInventoryItemAdapter.Static;
							@static.Adapt(item2);
							if (@static.HasIntegralAmounts)
							{
								myFixedPoint5 = (MyFixedPoint)(Math.Round((double)myFixedPoint4 * 1000.0 / (double)itemVolume) / 1000.0);
								myFixedPoint5 = MyFixedPoint.Floor(myFixedPoint5);
							}
							else
							{
								MyFixedPoint myFixedPoint6 = (MyFixedPoint)((double)myFixedPoint4 / (double)itemVolume);
								if (Math.Abs((float)myFixedPoint6 - (float)myFixedPoint5) > 0.001f)
								{
									myFixedPoint5 = myFixedPoint6;
								}
							}
						}
						if (myFixedPoint5 > 0)
						{
							MyInventory.TransferByPlanner(myInventory2, item3, item2, MyItemFlags.None, myFixedPoint5);
							dictionary[myInventory2][item2] -= myFixedPoint5;
							dictionary2[item3] -= myFixedPoint4;
						}
						myFixedPoint += myFixedPoint5;
<<<<<<< HEAD
=======
					}
					if (myFixedPoint == 0)
					{
						num++;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					if (myFixedPoint == 0)
					{
						num++;
					}
				}
			}
			return num;
		}

		private void WithdrawButton_ButtonClicked(MyGuiControlButton obj)
		{
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyBuildPlannerAction myBuildPlannerAction = MyBuildPlannerAction.None;
				myBuildPlannerAction = (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED) ? MyBuildPlannerAction.WithdrawKeep1 : ((!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED)) ? MyBuildPlannerAction.DefaultWithdraw : MyBuildPlannerAction.WithdrawKeep10));
				HashSet<MyInventory> usedTargetInventories = new HashSet<MyInventory>();
				List<MyIdentity.BuildPlanItem.Component> list = null;
<<<<<<< HEAD
				MyInventory[] sourceInventories = GetSourceInventories();
				switch (myBuildPlannerAction)
				{
				case MyBuildPlannerAction.DefaultWithdraw:
					list = ProcessWithdraw(m_interactedAsEntity, sourceInventories, ref usedTargetInventories, null);
					MySession.Static.LocalCharacter.CleanFinishedBuildPlanner();
					break;
				case MyBuildPlannerAction.WithdrawKeep1:
					list = ProcessWithdraw(multiplier: 1, owner: m_interactedAsEntity, inventories: sourceInventories, usedTargetInventories: ref usedTargetInventories);
					break;
				case MyBuildPlannerAction.WithdrawKeep10:
					list = ProcessWithdraw(multiplier: 10, owner: m_interactedAsEntity, inventories: sourceInventories, usedTargetInventories: ref usedTargetInventories);
=======
				switch (myBuildPlannerAction)
				{
				case MyBuildPlannerAction.DefaultWithdraw:
					list = ProcessWithdraw(m_interactedAsEntity, MySession.Static.LocalCharacter.GetInventory(), ref usedTargetInventories, null);
					MySession.Static.LocalCharacter.CleanFinishedBuildPlanner();
					break;
				case MyBuildPlannerAction.WithdrawKeep1:
					list = ProcessWithdraw(multiplier: 1, owner: m_interactedAsEntity, inventory: MySession.Static.LocalCharacter.GetInventory(), usedTargetInventories: ref usedTargetInventories);
					break;
				case MyBuildPlannerAction.WithdrawKeep10:
					list = ProcessWithdraw(multiplier: 10, owner: m_interactedAsEntity, inventory: MySession.Static.LocalCharacter.GetInventory(), usedTargetInventories: ref usedTargetInventories);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					break;
				}
				if (list != null && list.Count > 0)
				{
					string missingComponentsText = GetMissingComponentsText(list);
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: new StringBuilder(missingComponentsText)));
				}
				if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
				{
					m_withdrawButton.Enabled = true;
					m_addToProductionButton.Enabled = true;
					return;
				}
				if (m_withdrawButton.HasFocus || m_addToProductionButton.HasFocus)
				{
					IMyGuiControlsOwner myGuiControlsOwner = obj;
					while (myGuiControlsOwner.Owner != null)
					{
						myGuiControlsOwner = myGuiControlsOwner.Owner;
					}
					MyGuiScreenBase screen;
					if ((screen = myGuiControlsOwner as MyGuiScreenBase) != null)
					{
						RefocusInventories(screen, MySession.Static.LocalCharacter.GetInventory(), m_interactedAsEntity, usedTargetInventories);
					}
				}
				m_withdrawButton.Enabled = false;
				m_addToProductionButton.Enabled = false;
				return;
			}
			MyInventory[] sourceInventories2 = GetSourceInventories();
			int? persistentMultiple = null;
			if (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed())
			{
				persistentMultiple = 100;
			}
			else if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				persistentMultiple = 10;
			}
			else if (MyInput.Static.IsAnyShiftKeyPressed())
			{
				persistentMultiple = 1;
			}
			HashSet<MyInventory> usedTargetInventories2 = new HashSet<MyInventory>();
<<<<<<< HEAD
			List<MyIdentity.BuildPlanItem.Component> missingComponents = WithdrawToInventories(sourceInventories2, GetAvailableInventories, m_interactedAsEntity, ref usedTargetInventories2, persistentMultiple);
=======
			List<MyIdentity.BuildPlanItem.Component> missingComponents = WithdrawToInventories(sourceInventories, GetAvailableInventories, m_interactedAsEntity, ref usedTargetInventories2, persistentMultiple);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (persistentMultiple.HasValue)
			{
				return;
			}
			if (MySession.Static.LocalCharacter.BuildPlanner.Count > 0)
			{
				m_withdrawButton.Enabled = true;
				m_addToProductionButton.Enabled = true;
				string missingComponentsText2 = GetMissingComponentsText(missingComponents);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: new StringBuilder(missingComponentsText2)));
				return;
			}
			if (m_withdrawButton.HasFocus || m_addToProductionButton.HasFocus)
			{
				IMyGuiControlsOwner myGuiControlsOwner2 = obj;
				while (myGuiControlsOwner2.Owner != null)
				{
					myGuiControlsOwner2 = myGuiControlsOwner2.Owner;
				}
				MyGuiScreenBase screen2;
				if ((screen2 = myGuiControlsOwner2 as MyGuiScreenBase) != null)
				{
					RefocusInventories(screen2, MySession.Static.LocalCharacter.GetInventory(), m_interactedAsEntity, usedTargetInventories2);
				}
			}
			m_withdrawButton.Enabled = false;
			m_addToProductionButton.Enabled = false;
		}

		private void RefocusInventories(MyGuiScreenBase screen, MyInventory characterInventory, MyEntity interactedEntity, HashSet<MyInventory> actualSourceInventories)
		{
			if (screen == null)
			{
				return;
			}
			HashSet<MyInventory> sourceInventories = new HashSet<MyInventory>();
			for (int i = 0; i < interactedEntity.InventoryCount; i++)
			{
				sourceInventories.Add(interactedEntity.GetInventory(i));
			}
			MyGuiControlGrid interactedEntityGrid = null;
			MyGuiControlGrid actualInteractedEntityGrid = null;
			if (!SiftThroughInventories(m_leftOwnersControl) && !SiftThroughInventories(m_rightOwnersControl))
			{
				if (interactedEntityGrid != null)
				{
					screen.FocusedControl = interactedEntityGrid;
				}
<<<<<<< HEAD
				else if (actualInteractedEntityGrid != null)
=======
				else if (interactedEntityGrid != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					screen.FocusedControl = actualInteractedEntityGrid;
				}
				else
				{
					ForceSelectFirstInList(m_leftOwnersControl);
				}
			}
			bool SiftThroughInventories(MyGuiControlList list)
			{
				foreach (MyGuiControlBase control in list.Controls)
				{
					MyGuiControlInventoryOwner myGuiControlInventoryOwner = control as MyGuiControlInventoryOwner;
					if (myGuiControlInventoryOwner != null && myGuiControlInventoryOwner.Visible)
					{
						foreach (MyGuiControlGrid contentGrid in myGuiControlInventoryOwner.ContentGrids)
						{
							if (contentGrid.UserData == characterInventory)
							{
								screen.FocusedControl = contentGrid;
								return true;
							}
<<<<<<< HEAD
							if (actualSourceInventories.Contains(contentGrid.UserData))
							{
								actualInteractedEntityGrid = contentGrid;
								if (sourceInventories.Contains(contentGrid.UserData))
=======
							if (Enumerable.Contains<object>((IEnumerable<object>)actualSourceInventories, contentGrid.UserData))
							{
								actualInteractedEntityGrid = contentGrid;
								if (Enumerable.Contains<object>((IEnumerable<object>)sourceInventories, contentGrid.UserData))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								{
									interactedEntityGrid = contentGrid;
								}
							}
						}
					}
				}
				return false;
			}
		}

<<<<<<< HEAD
		private static List<MyIdentity.BuildPlanItem.Component> ProcessWithdraw(MyEntity owner, MyInventory[] inventories, ref HashSet<MyInventory> usedTargetInventories, int? multiplier)
=======
		private static List<MyIdentity.BuildPlanItem.Component> ProcessWithdraw(MyEntity owner, MyInventory inventory, ref HashSet<MyInventory> usedTargetInventories, int? multiplier)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			if (MySession.Static.LocalCharacter.BuildPlanner.Count == 0)
			{
				MyGuiScreenGamePlay.ShowEmptyBuildPlannerNotification();
				return null;
			}
<<<<<<< HEAD
			List<MyIdentity.BuildPlanItem.Component> list = Withdraw(owner, inventories, ref usedTargetInventories, multiplier);
=======
			List<MyIdentity.BuildPlanItem.Component> list = Withdraw(owner, inventory, ref usedTargetInventories, multiplier);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (list.Count == 0)
			{
				MyHud.Notifications.Add(MyNotificationSingletons.WithdrawSuccessful);
			}
			else
			{
				string missingComponentsText = GetMissingComponentsText(list);
				MyHud.Notifications.Add(MyNotificationSingletons.WithdrawFailed).SetTextFormatArguments(missingComponentsText);
			}
			return list;
		}

		public static string GetMissingComponentsText(List<MyIdentity.BuildPlanItem.Component> missingComponents)
		{
			string text = "";
			switch (missingComponents.Count)
			{
			case 0:
				return string.Empty;
			case 1:
				return string.Format(MyTexts.Get(MySpaceTexts.NotificationWithdrawFailed1).ToString(), missingComponents[0].Count, missingComponents[0].ComponentDefinition.DisplayNameText);
			case 2:
				return string.Format(MyTexts.Get(MySpaceTexts.NotificationWithdrawFailed2).ToString(), missingComponents[0].Count, missingComponents[0].ComponentDefinition.DisplayNameText, missingComponents[1].Count, missingComponents[1].ComponentDefinition.DisplayNameText);
			case 3:
				return string.Format(MyTexts.Get(MySpaceTexts.NotificationWithdrawFailed3).ToString(), missingComponents[0].Count, missingComponents[0].ComponentDefinition.DisplayNameText, missingComponents[1].Count, missingComponents[1].ComponentDefinition.DisplayNameText, missingComponents[2].Count, missingComponents[2].ComponentDefinition.DisplayNameText);
			default:
			{
				int num = 0;
				for (int i = 3; i < missingComponents.Count; i++)
				{
					num += missingComponents[i].Count;
				}
				return string.Format(MyTexts.Get(MySpaceTexts.NotificationWithdrawFailed4More).ToString(), missingComponents[0].Count, missingComponents[0].ComponentDefinition.DisplayNameText, missingComponents[1].Count, missingComponents[1].ComponentDefinition.DisplayNameText, missingComponents[2].Count, missingComponents[2].ComponentDefinition.DisplayNameText, num);
			}
			}
		}

<<<<<<< HEAD
		public static List<MyIdentity.BuildPlanItem.Component> Withdraw(MyEntity interactedEntity, MyInventory[] inventories, ref HashSet<MyInventory> usedTargetInventories, int? persistentMultiple)
		{
			return WithdrawToInventories(inventories.ToArray(), GetAvailableInventoriesStatic, interactedEntity, ref usedTargetInventories, persistentMultiple);
=======
		public static List<MyIdentity.BuildPlanItem.Component> Withdraw(MyEntity interactedEntity, MyInventory toInventory, ref HashSet<MyInventory> usedTargetInventories, int? persistentMultiple)
		{
			return WithdrawToInventories(new MyInventory[1] { toInventory }, GetAvailableInventoriesStatic, interactedEntity, ref usedTargetInventories, persistentMultiple);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void GetAvailableInventoriesStatic(MyEntity destinationEntity, MyDefinitionId id, List<MyInventory> availableInventories, MyEntity interactedEntity, bool requireAmount)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			availableInventories.Clear();
			List<MyEntity> list = new List<MyEntity>();
			MyCubeBlock myCubeBlock = interactedEntity as MyCubeBlock;
			MyInventoryBagEntity item;
			if (myCubeBlock != null)
			{
<<<<<<< HEAD
				foreach (MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node node in MyCubeGridGroups.Static.Mechanical.GetGroup(myCubeBlock.CubeGrid).Nodes)
				{
					GetGridInventories(node.NodeData, list, destinationEntity, MySession.Static.LocalPlayerId);
				}
				if (myCubeBlock.HasInventory && !list.Contains(myCubeBlock))
				{
					list.Add(myCubeBlock);
=======
				Enumerator<MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node> enumerator = MyCubeGridGroups.Static.Mechanical.GetGroup(myCubeGrid).Nodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						GetGridInventories(enumerator.get_Current().NodeData, list, interactedEntity, MySession.Static.LocalPlayerId);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			else if ((item = interactedEntity as MyInventoryBagEntity) != null)
			{
				list.Add(item);
			}
			IMyConveyorEndpointBlock myConveyorEndpointBlock = ((!(destinationEntity is MyCharacter)) ? (destinationEntity as IMyConveyorEndpointBlock) : (interactedEntity as IMyConveyorEndpointBlock));
			foreach (MyEntity item2 in list)
			{
<<<<<<< HEAD
				int num = 0;
				for (int i = 0; i < item2.InventoryCount; i++)
				{
					if (requireAmount && item2.GetInventory(i).GetItemAmount(id) <= 0)
					{
						num++;
					}
				}
				if (num == item2.InventoryCount)
				{
					continue;
				}
				if (item2 == myCubeBlock && myCubeBlock.IsFunctional)
				{
					for (int j = 0; j < item2.InventoryCount; j++)
					{
						MyInventory inventory = item2.GetInventory(j);
						if (inventory.CheckConstraint(id) && (!requireAmount || inventory.GetItemAmount(id) > 0))
						{
							availableInventories.Add(inventory);
						}
=======
				IMyConveyorEndpointBlock myConveyorEndpointBlock = item as IMyConveyorEndpointBlock;
				IMyConveyorEndpointBlock myConveyorEndpointBlock2 = interactedEntity as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock == null || myConveyorEndpointBlock2 == null || (myConveyorEndpointBlock2 is IMyCubeBlock && !((IMyCubeBlock)myConveyorEndpointBlock2).IsFunctional) || (myConveyorEndpointBlock is IMyCubeBlock && !((IMyCubeBlock)myConveyorEndpointBlock).IsFunctional) || (myConveyorEndpointBlock != myConveyorEndpointBlock2 && !MyGridConveyorSystem.Reachable(myConveyorEndpointBlock.ConveyorEndpoint, myConveyorEndpointBlock2.ConveyorEndpoint, MySession.Static.LocalPlayerId, id, EndpointPredicateStatic)))
				{
					continue;
				}
				for (int i = 0; i < item.InventoryCount; i++)
				{
					MyInventory inventory = item.GetInventory(i);
					if (inventory.CheckConstraint(id) && (!requireAmount || inventory.GetItemAmount(id) > 0))
					{
						availableInventories.Add(inventory);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					continue;
				}
				IMyConveyorEndpointBlock myConveyorEndpointBlock2 = item2 as IMyConveyorEndpointBlock;
				IMyCubeBlock myCubeBlock2;
				IMyCubeBlock myCubeBlock3;
				if (item2.EntityId == destinationEntity.EntityId || myConveyorEndpointBlock2 == null || myConveyorEndpointBlock == null || ((myCubeBlock2 = myConveyorEndpointBlock as IMyCubeBlock) != null && !myCubeBlock2.IsFunctional) || ((myCubeBlock3 = myConveyorEndpointBlock2 as IMyCubeBlock) != null && !myCubeBlock3.IsFunctional) || (myConveyorEndpointBlock2 != myConveyorEndpointBlock && !MyGridConveyorSystem.Reachable(myConveyorEndpointBlock2.ConveyorEndpoint, myConveyorEndpointBlock.ConveyorEndpoint, MySession.Static.LocalPlayerId, id, EndpointPredicateStatic)))
				{
					continue;
				}
				for (int k = 0; k < item2.InventoryCount; k++)
				{
					MyInventory inventory2 = item2.GetInventory(k);
					if (inventory2.CheckConstraint(id) && (!requireAmount || inventory2.GetItemAmount(id) > 0))
					{
						availableInventories.Add(inventory2);
					}
				}
			}
		}

		private static List<MyIdentity.BuildPlanItem.Component> WithdrawToInventories(MyInventory[] toInventories, Action<MyEntity, MyDefinitionId, List<MyInventory>, MyEntity, bool> getInventoriesMethod, MyEntity interactedEntity, ref HashSet<MyInventory> usedTargetInventories, int? persistentMultiple = null)
		{
			Dictionary<MyInventory, Dictionary<MyDefinitionId, int>> dictionary = new Dictionary<MyInventory, Dictionary<MyDefinitionId, int>>();
			List<MyInventory> list = new List<MyInventory>();
			IReadOnlyList<MyIdentity.BuildPlanItem> readOnlyList = MySession.Static.LocalCharacter.BuildPlanner;
			if (persistentMultiple.HasValue)
			{
				List<MyIdentity.BuildPlanItem> list2 = new List<MyIdentity.BuildPlanItem>();
				for (int num = persistentMultiple.Value; num > 0; num--)
				{
					foreach (MyIdentity.BuildPlanItem item2 in MySession.Static.LocalCharacter.BuildPlanner)
					{
						MyIdentity.BuildPlanItem item = item2.Clone();
						list2.Add(item);
					}
				}
				readOnlyList = list2;
			}
			foreach (MyInventory myInventory in toInventories)
			{
				MyFixedPoint myFixedPoint = myInventory.MaxVolume - myInventory.CurrentVolume;
				foreach (MyIdentity.BuildPlanItem item3 in readOnlyList)
				{
					foreach (MyIdentity.BuildPlanItem.Component component in item3.Components)
					{
						if (component.ComponentDefinition == null || component.ComponentDefinition.Volume <= 0f || !myInventory.CheckConstraint(component.ComponentDefinition.Id))
						{
							continue;
						}
<<<<<<< HEAD
						getInventoriesMethod?.Invoke(myInventory.Owner, component.ComponentDefinition.Id, list, interactedEntity, arg5: true);
=======
						getInventoriesMethod(myInventory.Owner, component.ComponentDefinition.Id, list, interactedEntity, arg5: true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						foreach (MyInventory item4 in list)
						{
							MyFixedPoint itemAmount = item4.GetItemAmount(component.ComponentDefinition.Id);
							if (!Sync.IsServer)
							{
								if (dictionary.ContainsKey(item4) && dictionary[item4].ContainsKey(component.ComponentDefinition.Id))
								{
									int num2 = dictionary[item4][component.ComponentDefinition.Id];
									itemAmount -= (MyFixedPoint)num2;
								}
								if (itemAmount == 0)
								{
									continue;
								}
							}
							MyFixedPoint myFixedPoint2 = itemAmount;
							if (itemAmount > component.Count)
							{
								myFixedPoint2 = component.Count;
							}
							float num3 = (float)myFixedPoint2 * component.ComponentDefinition.Volume;
							if (num3 > (float)myFixedPoint)
							{
								num3 = (float)myFixedPoint;
								myFixedPoint2 = (int)(num3 / component.ComponentDefinition.Volume);
							}
							if (!Sync.IsServer)
							{
								if (!dictionary.ContainsKey(item4))
								{
									dictionary.Add(item4, new Dictionary<MyDefinitionId, int>());
								}
								if (!dictionary[item4].ContainsKey(component.ComponentDefinition.Id))
								{
									dictionary[item4].Add(component.ComponentDefinition.Id, 0);
								}
								dictionary[item4][component.ComponentDefinition.Id] += (int)myFixedPoint2;
<<<<<<< HEAD
							}
							MyInventory.TransferByPlanner(item4, myInventory, component.ComponentDefinition.Id, MyItemFlags.None, myFixedPoint2);
							if (usedTargetInventories != null && !usedTargetInventories.Contains(item4))
							{
								usedTargetInventories.Add(item4);
							}
=======
							}
							MyInventory.TransferByPlanner(item4, myInventory, component.ComponentDefinition.Id, MyItemFlags.None, myFixedPoint2);
							if (usedTargetInventories != null && !usedTargetInventories.Contains(item4))
							{
								usedTargetInventories.Add(item4);
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							myFixedPoint -= (MyFixedPoint)num3;
							item3.IsInProgress = true;
							component.Count -= (int)myFixedPoint2;
							break;
						}
					}
					item3.Components.RemoveAll((MyIdentity.BuildPlanItem.Component x) => x.Count == 0);
				}
				if (!persistentMultiple.HasValue)
				{
					MySession.Static.LocalCharacter.CleanFinishedBuildPlanner();
				}
			}
			List<MyIdentity.BuildPlanItem.Component> list3 = new List<MyIdentity.BuildPlanItem.Component>();
			foreach (MyIdentity.BuildPlanItem item5 in readOnlyList)
			{
				foreach (MyIdentity.BuildPlanItem.Component component2 in item5.Components)
				{
					list3.Add(component2);
				}
			}
			return list3;
		}

		private void GetAvailableInventories(MyEntity inventoryOwner, MyDefinitionId id, List<MyInventory> availableInventories, MyEntity interactedEntity, bool requireAmount)
		{
			ObservableCollection<MyGuiControlBase>.Enumerator enumerator = m_rightOwnersControl.Controls.GetEnumerator();
			availableInventories.Clear();
			MyGuiControlInventoryOwner myGuiControlInventoryOwner = null;
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.Visible)
				{
					continue;
				}
				myGuiControlInventoryOwner = enumerator.Current as MyGuiControlInventoryOwner;
				if (myGuiControlInventoryOwner == null || !myGuiControlInventoryOwner.Enabled)
				{
					continue;
				}
				if ((inventoryOwner != m_userAsOwner && inventoryOwner != m_interactedAsOwner) || (myGuiControlInventoryOwner.InventoryOwner != m_userAsOwner && myGuiControlInventoryOwner.InventoryOwner != m_interactedAsOwner))
				{
					int num = 0;
					for (int i = 0; i < myGuiControlInventoryOwner.InventoryOwner.InventoryCount; i++)
					{
						if (requireAmount && myGuiControlInventoryOwner.InventoryOwner.GetInventory(i).GetItemAmount(id) <= 0)
						{
							num++;
						}
					}
					if (num == myGuiControlInventoryOwner.InventoryOwner.InventoryCount)
					{
						continue;
					}
					bool flag = inventoryOwner is MyCharacter;
					bool flag2 = myGuiControlInventoryOwner.InventoryOwner is MyCharacter;
					IMyConveyorEndpointBlock myConveyorEndpointBlock = ((inventoryOwner == null) ? null : ((flag ? m_interactedAsOwner : inventoryOwner) as IMyConveyorEndpointBlock));
					IMyConveyorEndpointBlock myConveyorEndpointBlock2 = ((myGuiControlInventoryOwner.InventoryOwner == null) ? null : ((flag2 ? m_interactedAsOwner : myGuiControlInventoryOwner.InventoryOwner) as IMyConveyorEndpointBlock));
					if (myConveyorEndpointBlock == null || myConveyorEndpointBlock2 == null)
					{
						continue;
					}
					try
					{
						MyGridConveyorSystem.AppendReachableEndpoints(myConveyorEndpointBlock.ConveyorEndpoint, MySession.Static.LocalPlayerId, m_reachableInventoryOwners, id, m_endpointPredicate);
						if (!m_reachableInventoryOwners.Contains(myConveyorEndpointBlock2.ConveyorEndpoint))
						{
							continue;
						}
					}
					finally
					{
						m_reachableInventoryOwners.Clear();
					}
					if (!MyGridConveyorSystem.Reachable(myConveyorEndpointBlock.ConveyorEndpoint, myConveyorEndpointBlock2.ConveyorEndpoint))
					{
						continue;
					}
				}
				MyEntity inventoryOwner2 = myGuiControlInventoryOwner.InventoryOwner;
				for (int j = 0; j < inventoryOwner2.InventoryCount; j++)
				{
					MyInventory inventory = inventoryOwner2.GetInventory(j);
					if (inventory.CheckConstraint(id) && (!requireAmount || inventory.GetItemAmount(id) > 0))
					{
						availableInventories.Add(inventory);
					}
				}
			}
		}

		public void Close()
		{
			foreach (MyGridConveyorSystem registeredConveyorSystem in m_registeredConveyorSystems)
			{
				registeredConveyorSystem.BlockAdded -= ConveyorSystem_BlockAdded;
				registeredConveyorSystem.BlockRemoved -= ConveyorSystem_BlockRemoved;
			}
			m_registeredConveyorSystems.Clear();
			foreach (MyGridConveyorSystem registeredConveyorMechanicalSystem in m_registeredConveyorMechanicalSystems)
			{
				registeredConveyorMechanicalSystem.BlockAdded -= ConveyorSystemMechanical_BlockAdded;
				registeredConveyorMechanicalSystem.BlockRemoved -= ConveyorSystemMechanical_BlockRemoved;
			}
			m_registeredConveyorMechanicalSystems.Clear();
			m_leftTypeGroup.Clear();
			m_leftFilterGroup.Clear();
			m_rightTypeGroup.Clear();
			m_rightFilterGroup.Clear();
			m_controlsDisabledWhileDragged.Clear();
			m_leftOwnersControl = null;
			m_leftSuitButton = null;
			m_leftGridButton = null;
			m_leftFilterStorageButton = null;
			m_leftFilterSystemButton = null;
			m_leftFilterEnergyButton = null;
			m_leftFilterAllButton = null;
			m_leftFilterShipButton = null;
			m_rightOwnersControl = null;
			m_rightSuitButton = null;
			m_rightGridButton = null;
			m_rightFilterShipButton = null;
			m_rightFilterStorageButton = null;
			m_rightFilterSystemButton = null;
			m_rightFilterEnergyButton = null;
			m_rightFilterAllButton = null;
			m_throwOutButton = null;
			m_dragAndDrop = null;
			m_dragAndDropInfo = null;
			FocusedOwnerControl = null;
			FocusedGridControl = null;
			m_selectedInventory = null;
			MyGuiControlCheckbox hideEmptyLeft = m_hideEmptyLeft;
			hideEmptyLeft.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(hideEmptyLeft.IsCheckedChanged, new Action<MyGuiControlCheckbox>(HideEmptyLeft_Checked));
			MyGuiControlCheckbox hideEmptyRight = m_hideEmptyRight;
			hideEmptyRight.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(hideEmptyRight.IsCheckedChanged, new Action<MyGuiControlCheckbox>(HideEmptyRight_Checked));
			m_searchBoxLeft.OnTextChanged -= BlockSearchLeft_TextChanged;
			m_searchBoxRight.OnTextChanged -= BlockSearchRight_TextChanged;
			m_hideEmptyLeft = null;
			m_hideEmptyLeftLabel = null;
			m_hideEmptyRight = null;
			m_hideEmptyRightLabel = null;
			m_searchBoxLeft = null;
			m_searchBoxRight = null;
		}

		public void SetSearch(string text, bool interactedSide = true)
		{
			MyGuiControlSearchBox myGuiControlSearchBox = (interactedSide ? m_searchBoxRight : m_searchBoxLeft);
			if (myGuiControlSearchBox != null)
			{
				myGuiControlSearchBox.SearchText = text;
			}
			if (interactedSide)
			{
				SetRightFilter(null);
			}
			else
			{
				SetLeftFilter(null);
			}
		}

		private void StartDragging(MyDropHandleType dropHandlingType, MyGuiControlGrid gridControl, ref MyGuiControlGrid.EventArgs args)
		{
			m_dragAndDropInfo = new MyDragAndDropInfo();
			m_dragAndDropInfo.Grid = gridControl;
			m_dragAndDropInfo.ItemIndex = args.ItemIndex;
			DisableInvalidWhileDragging();
			MyGuiGridItem itemAt = m_dragAndDropInfo.Grid.GetItemAt(m_dragAndDropInfo.ItemIndex);
			m_dragAndDrop.StartDragging(dropHandlingType, args.Button, itemAt, m_dragAndDropInfo, includeTooltip: false);
		}

		private void DisableInvalidWhileDragging()
		{
			MyGuiGridItem itemAt = m_dragAndDropInfo.Grid.GetItemAt(m_dragAndDropInfo.ItemIndex);
			if (itemAt != null)
			{
				MyPhysicalInventoryItem item = (MyPhysicalInventoryItem)itemAt.UserData;
				MyInventory srcInventory = (MyInventory)m_dragAndDropInfo.Grid.UserData;
				DisableUnacceptingInventoryControls(item, m_leftOwnersControl);
				DisableUnacceptingInventoryControls(item, m_rightOwnersControl);
				DisableUnreachableInventoryControls(srcInventory, item, m_leftOwnersControl);
				DisableUnreachableInventoryControls(srcInventory, item, m_rightOwnersControl);
			}
		}

		private void DisableUnacceptingInventoryControls(MyPhysicalInventoryItem item, MyGuiControlList list)
		{
			foreach (MyGuiControlBase visibleControl in list.Controls.GetVisibleControls())
			{
				if (!visibleControl.Enabled)
				{
					continue;
				}
				MyGuiControlInventoryOwner myGuiControlInventoryOwner = (MyGuiControlInventoryOwner)visibleControl;
				MyEntity inventoryOwner = myGuiControlInventoryOwner.InventoryOwner;
				for (int i = 0; i < inventoryOwner.InventoryCount; i++)
				{
					if (!inventoryOwner.GetInventory(i).CanItemsBeAdded(0, item.Content.GetId()))
					{
						myGuiControlInventoryOwner.ContentGrids[i].Enabled = false;
						m_controlsDisabledWhileDragged.Add(myGuiControlInventoryOwner.ContentGrids[i]);
					}
				}
			}
		}

		private bool EndpointPredicate(IMyConveyorEndpoint endpoint)
		{
			if (endpoint.CubeBlock == null || !endpoint.CubeBlock.HasInventory)
			{
				return endpoint.CubeBlock == m_interactedEndpointBlock;
			}
			return true;
		}

		private static bool EndpointPredicateStatic(IMyConveyorEndpoint endpoint)
		{
			if (endpoint.CubeBlock != null)
			{
				return endpoint.CubeBlock.HasInventory;
			}
			return false;
		}

		private void DisableUnreachableInventoryControls(MyInventory srcInventory, MyPhysicalInventoryItem item, MyGuiControlList list)
		{
			bool flag = srcInventory.Owner == m_userAsOwner;
			bool flag2 = srcInventory.Owner == m_interactedAsOwner;
			MyEntity owner = srcInventory.Owner;
			IMyConveyorEndpointBlock myConveyorEndpointBlock = null;
			if (flag)
			{
				myConveyorEndpointBlock = m_interactedAsEntity as IMyConveyorEndpointBlock;
			}
			else if (owner != null)
			{
				myConveyorEndpointBlock = owner as IMyConveyorEndpointBlock;
			}
			IMyConveyorEndpointBlock myConveyorEndpointBlock2 = null;
			if (m_interactedAsEntity != null)
			{
				myConveyorEndpointBlock2 = m_interactedAsEntity as IMyConveyorEndpointBlock;
			}
			if (myConveyorEndpointBlock != null)
			{
				long localPlayerId = MySession.Static.LocalPlayerId;
				m_interactedEndpointBlock = myConveyorEndpointBlock2;
				MyGridConveyorSystem.AppendReachableEndpoints(myConveyorEndpointBlock.ConveyorEndpoint, localPlayerId, m_reachableInventoryOwners, item.Content.GetId(), m_endpointPredicate);
			}
			foreach (MyGuiControlBase visibleControl in list.Controls.GetVisibleControls())
			{
				if (!visibleControl.Enabled)
<<<<<<< HEAD
				{
					continue;
				}
				MyGuiControlInventoryOwner myGuiControlInventoryOwner = (MyGuiControlInventoryOwner)visibleControl;
				MyEntity inventoryOwner = myGuiControlInventoryOwner.InventoryOwner;
				IMyConveyorEndpoint item2 = null;
				IMyConveyorEndpointBlock myConveyorEndpointBlock3 = inventoryOwner as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock3 != null)
				{
=======
				{
					continue;
				}
				MyGuiControlInventoryOwner myGuiControlInventoryOwner = (MyGuiControlInventoryOwner)visibleControl;
				MyEntity inventoryOwner = myGuiControlInventoryOwner.InventoryOwner;
				IMyConveyorEndpoint item2 = null;
				IMyConveyorEndpointBlock myConveyorEndpointBlock3 = inventoryOwner as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock3 != null)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					item2 = myConveyorEndpointBlock3.ConveyorEndpoint;
				}
				bool num = inventoryOwner == owner;
				bool flag3 = (flag && inventoryOwner == m_interactedAsOwner) || (flag2 && inventoryOwner == m_userAsOwner);
				bool num2 = !num && !flag3;
				bool flag4 = !m_reachableInventoryOwners.Contains(item2);
				bool flag5 = myConveyorEndpointBlock2 != null && m_reachableInventoryOwners.Contains(myConveyorEndpointBlock2.ConveyorEndpoint);
				bool flag6 = inventoryOwner == m_userAsOwner && flag5;
				if (!(num2 && flag4) || flag6)
				{
					continue;
				}
				for (int i = 0; i < inventoryOwner.InventoryCount; i++)
				{
					if (myGuiControlInventoryOwner.ContentGrids[i].Enabled)
					{
						myGuiControlInventoryOwner.ContentGrids[i].Enabled = false;
						m_controlsDisabledWhileDragged.Add(myGuiControlInventoryOwner.ContentGrids[i]);
					}
				}
			}
			m_reachableInventoryOwners.Clear();
		}

		private static void GetGridInventories(MyCubeGrid grid, List<MyEntity> outputInventories, MyEntity interactedEntity, long identityId)
		{
			grid?.GridSystems.ConveyorSystem.GetGridInventories(interactedEntity, outputInventories, identityId);
		}

		private void CreateInventoryControlInList(MyEntity owner, MyGuiControlList listControl)
		{
			List<MyEntity> list = new List<MyEntity>();
			if (owner != null)
			{
				list.Add(owner);
			}
			CreateInventoryControlsInList(list, listControl);
		}

		private void CreateInventoryControlsInList(List<MyEntity> owners, MyGuiControlList listControl, MyInventoryOwnerTypeEnum? filterType = null)
		{
			if (listControl.Controls.Contains(FocusedOwnerControl))
			{
				FocusedOwnerControl = null;
			}
			List<MyGuiControlBase> list = new List<MyGuiControlBase>();
			foreach (MyEntity owner in owners)
			{
				if (owner == null || !owner.HasInventory || (filterType.HasValue && owner.InventoryOwnerType() != filterType))
				{
					continue;
				}
				Vector4 labelColorMask = Color.White.ToVector4();
				if (owner is MyCubeBlock)
				{
					Color? gridColor = m_colorHelper.GetGridColor((owner as MyCubeBlock).CubeGrid);
					labelColorMask = (gridColor.HasValue ? gridColor.Value.ToVector4() : Vector4.One);
				}
				MyGuiControlInventoryOwner myGuiControlInventoryOwner = new MyGuiControlInventoryOwner(owner, labelColorMask);
				myGuiControlInventoryOwner.Size = new Vector2(listControl.Size.X - 0.05f, myGuiControlInventoryOwner.Size.Y);
				myGuiControlInventoryOwner.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				foreach (MyGuiControlGrid contentGrid in myGuiControlInventoryOwner.ContentGrids)
				{
					contentGrid.FocusChanged += grid_focusChanged;
					contentGrid.ItemSelected += grid_ItemSelected;
					contentGrid.ItemDragged += grid_ItemDragged;
					contentGrid.ItemDoubleClicked += grid_ItemDoubleClicked;
					contentGrid.ItemClicked += grid_ItemClicked;
					contentGrid.ItemAccepted += grid_ItemDoubleClicked;
					contentGrid.ItemReleased += grid_ItemReleased;
					contentGrid.ReleasedWithoutItem += grid_ReleasedWithoutItem;
					contentGrid.ItemControllerAction = (Func<MyGuiControlGrid, int, MyGridItemAction, bool, bool>)Delegate.Combine(contentGrid.ItemControllerAction, new Func<MyGuiControlGrid, int, MyGridItemAction, bool, bool>(grid_ItemControllerAction));
				}
				myGuiControlInventoryOwner.SizeChanged += inventoryControl_SizeChanged;
				myGuiControlInventoryOwner.InventoryContentsChanged += ownerControl_InventoryContentsChanged;
				if (owner is MyCubeBlock)
				{
					myGuiControlInventoryOwner.Enabled = (owner as MyCubeBlock).IsFunctional;
				}
				if (owner == m_interactedAsOwner || owner == m_userAsOwner)
				{
					list.Insert(0, myGuiControlInventoryOwner);
				}
				else
				{
					list.Add(myGuiControlInventoryOwner);
				}
			}
			list.SortNoAlloc(CompareGuiControlInventoryOwners);
			listControl.InitControls(list);
		}

<<<<<<< HEAD
		private int CompareGuiControlInventoryOwners(MyGuiControlBase x, MyGuiControlBase y)
		{
			MyGuiControlInventoryOwner myGuiControlInventoryOwner = x as MyGuiControlInventoryOwner;
			MyGuiControlInventoryOwner myGuiControlInventoryOwner2 = y as MyGuiControlInventoryOwner;
			if (myGuiControlInventoryOwner == null)
			{
				if (myGuiControlInventoryOwner2 == null)
				{
					return 0;
				}
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			if (myGuiControlInventoryOwner.InventoryOwner == m_interactedAsOwner || myGuiControlInventoryOwner.InventoryOwner == m_userAsOwner)
			{
				return -1;
			}
			if (myGuiControlInventoryOwner2.InventoryOwner == m_interactedAsOwner || myGuiControlInventoryOwner2.InventoryOwner == m_userAsOwner)
			{
				return 1;
			}
			return string.Compare(myGuiControlInventoryOwner.InventoryOwner.DisplayNameText, myGuiControlInventoryOwner2.InventoryOwner.DisplayNameText);
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void grid_focusChanged(MyGuiControlBase sender, bool focus)
		{
			if (!focus)
			{
				return;
			}
			MyGuiControlGrid myGuiControlGrid2 = (FocusedGridControl = sender as MyGuiControlGrid);
			FocusedOwnerControl = (MyGuiControlInventoryOwner)myGuiControlGrid2.Owner;
			RefreshSelectedInventoryItem();
			if (m_focusedOwnerControl != null)
			{
				if (m_selectedInventoryItem.HasValue && CanTransferItem(m_selectedInventoryItem.Value, m_focusedGridControl, out var _, out var _))
				{
					m_focusedOwnerControl.ResetGamepadHelp(m_userAsOwner, canTransfer: true);
				}
				else
				{
					m_focusedOwnerControl.ResetGamepadHelp(m_userAsOwner, canTransfer: false);
				}
			}
		}

		private bool grid_ItemControllerAction(MyGuiControlGrid sender, int index, MyGridItemAction action, bool pressed)
		{
			return ActivateItemGamepad(sender, index, action, pressed);
		}

		private void grid_ReleasedWithoutItem(MyGuiControlGrid obj)
		{
			FocusedGridControl = obj;
			FocusedOwnerControl = (MyGuiControlInventoryOwner)obj.Owner;
			RefreshSelectedInventoryItem();
		}

		private void ShowAmountTransferDialog(MyPhysicalInventoryItem inventoryItem, Action<float> onConfirmed)
		{
			MyFixedPoint amount = inventoryItem.Amount;
			MyObjectBuilderType typeId = inventoryItem.Content.TypeId;
			int minMaxDecimalDigits = 0;
			bool parseAsInteger = true;
			if (typeId == typeof(MyObjectBuilder_Ore) || typeId == typeof(MyObjectBuilder_Ingot))
			{
				minMaxDecimalDigits = 2;
				parseAsInteger = false;
			}
			MyGuiScreenDialogAmount dialog = new MyGuiScreenDialogAmount(0f, (float)amount, MyCommonTexts.DialogAmount_AddAmountCaption, minMaxDecimalDigits, parseAsInteger, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity);
			dialog.OnConfirmed += onConfirmed;
			if (m_interactedAsEntity != null)
			{
				Action<MyEntity> entityCloseAction = null;
				entityCloseAction = delegate
				{
					dialog.CloseScreen();
				};
				m_interactedAsEntity.OnClose += entityCloseAction;
				dialog.Closed += delegate
				{
					if (m_interactedAsEntity != null)
					{
						m_interactedAsEntity.OnClose -= entityCloseAction;
					}
				};
			}
			MyGuiSandbox.AddScreen(dialog);
		}

		private bool CanTransferItem(MyPhysicalInventoryItem item, MyGuiControlGrid sender, out MyInventory srcInventory, out MyInventory dstInventory)
		{
			srcInventory = null;
			dstInventory = null;
			if (sender == null || sender.Owner == null)
			{
				return false;
			}
			MyGuiControlInventoryOwner myGuiControlInventoryOwner = sender.Owner as MyGuiControlInventoryOwner;
			ObservableCollection<MyGuiControlBase>.Enumerator enumerator = ((myGuiControlInventoryOwner.Owner == m_leftOwnersControl) ? m_rightOwnersControl : m_leftOwnersControl).Controls.GetEnumerator();
			MyGuiControlInventoryOwner myGuiControlInventoryOwner2 = null;
			myGuiControlInventoryOwner2 = ((myGuiControlInventoryOwner.Owner == m_leftOwnersControl) ? (RightFocusedInventory?.Owner as MyGuiControlInventoryOwner) : (LeftFocusedInventory?.Owner as MyGuiControlInventoryOwner));
			if (myGuiControlInventoryOwner2 == null)
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Visible)
					{
						myGuiControlInventoryOwner2 = enumerator.Current as MyGuiControlInventoryOwner;
						break;
					}
				}
			}
			if (myGuiControlInventoryOwner2 == null || !myGuiControlInventoryOwner2.Enabled)
			{
				return false;
			}
			if ((myGuiControlInventoryOwner.InventoryOwner != m_userAsOwner && myGuiControlInventoryOwner.InventoryOwner != m_interactedAsOwner) || (myGuiControlInventoryOwner2.InventoryOwner != m_userAsOwner && myGuiControlInventoryOwner2.InventoryOwner != m_interactedAsOwner))
			{
				bool flag = myGuiControlInventoryOwner.InventoryOwner is MyCharacter;
				bool flag2 = myGuiControlInventoryOwner2.InventoryOwner is MyCharacter;
				IMyConveyorEndpointBlock myConveyorEndpointBlock = ((myGuiControlInventoryOwner.InventoryOwner == null) ? null : ((flag ? m_interactedAsOwner : myGuiControlInventoryOwner.InventoryOwner) as IMyConveyorEndpointBlock));
				IMyConveyorEndpointBlock myConveyorEndpointBlock2 = ((myGuiControlInventoryOwner2.InventoryOwner == null) ? null : ((flag2 ? m_interactedAsOwner : myGuiControlInventoryOwner2.InventoryOwner) as IMyConveyorEndpointBlock));
				if (myConveyorEndpointBlock == null || myConveyorEndpointBlock2 == null)
				{
					return false;
				}
				try
				{
					MyGridConveyorSystem.AppendReachableEndpoints(myConveyorEndpointBlock.ConveyorEndpoint, MySession.Static.LocalPlayerId, m_reachableInventoryOwners, item.Content.GetId(), m_endpointPredicate);
					if (!m_reachableInventoryOwners.Contains(myConveyorEndpointBlock2.ConveyorEndpoint))
					{
						return false;
					}
				}
				finally
				{
					m_reachableInventoryOwners.Clear();
				}
				if (!MyGridConveyorSystem.Reachable(myConveyorEndpointBlock.ConveyorEndpoint, myConveyorEndpointBlock2.ConveyorEndpoint))
				{
					return false;
				}
			}
			MyEntity inventoryOwner = myGuiControlInventoryOwner2.InventoryOwner;
			_ = myGuiControlInventoryOwner.InventoryOwner;
			srcInventory = (MyInventory)sender.UserData;
			dstInventory = ((myGuiControlInventoryOwner.Owner == m_leftOwnersControl) ? (RightFocusedInventory?.UserData as MyInventory) : (LeftFocusedInventory?.UserData as MyInventory));
			if (dstInventory == null)
			{
				for (int i = 0; i < inventoryOwner.InventoryCount; i++)
				{
					MyInventory inventory = inventoryOwner.GetInventory(i);
					if (inventory.CheckConstraint(item.Content.GetId()))
					{
						dstInventory = inventory;
						break;
					}
				}
			}
			else if (!dstInventory.CheckConstraint(item.Content.GetId()))
			{
				return false;
			}
			if (dstInventory == null)
			{
				return false;
			}
			return true;
		}

		private bool TransferToOppositeFirst(MyPhysicalInventoryItem item, MyGuiControlGrid sender)
		{
			if (!CanTransferItem(item, sender, out var srcInventory, out var dstInventory))
			{
				return false;
			}
			MyFixedPoint value = item.Amount;
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED))
			{
				value = ((!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED)) ? ((MyFixedPoint)10) : ((MyFixedPoint)1000));
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED))
			{
				value = 100;
			}
			List<MyPhysicalInventoryItem> items = srcInventory.GetItems();
			int value2 = sender.SelectedIndex.Value;
			if (value2 >= 0 && value2 < items.Count)
			{
				MyInventory.TransferByUser(srcInventory, dstInventory, items[value2].ItemId, -1, value);
				return true;
			}
			return false;
		}

		private void SetLeftFilter(MyInventoryOwnerTypeEnum? filterType)
		{
			if (LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterCharacter)
			{
				m_leftFilterType = null;
			}
			else
<<<<<<< HEAD
			{
				m_leftFilterType = filterType;
			}
			m_leftFilterType = filterType;
			if (LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
			{
=======
			{
				m_leftFilterType = filterType;
			}
			m_leftFilterType = filterType;
			if (LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				CreateInventoryControlsInList((LeftFilterTypeIndex == 2) ? m_interactedGridOwnersMechanical : m_interactedGridOwners, m_leftOwnersControl, m_leftFilterType);
				m_searchBoxLeft.SearchText = m_searchBoxLeft.SearchText;
			}
			LeftFocusedInventory = ((m_leftOwnersControl.Controls.Count > 0) ? (m_leftOwnersControl.Controls[0] as MyGuiControlInventoryOwner).ContentGrids[0] : null);
			RefreshSelectedInventoryItem();
			ForceSelectSearchBox(m_searchBoxLeft);
			LeftFilterTypeIndex = m_leftFilterGroup.SelectedIndex.Value;
		}

		private void SetRightFilter(MyInventoryOwnerTypeEnum? filterType)
		{
			if (RightFilter == MyGuiControlRadioButtonStyleEnum.FilterCharacter)
<<<<<<< HEAD
			{
				m_rightFilterType = null;
			}
			else
			{
				m_rightFilterType = filterType;
			}
=======
			{
				m_rightFilterType = null;
			}
			else
			{
				m_rightFilterType = filterType;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (RightFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
			{
				CreateInventoryControlsInList((RightFilterTypeIndex == 2) ? m_interactedGridOwnersMechanical : m_interactedGridOwners, m_rightOwnersControl, m_rightFilterType);
				m_searchBoxRight.SearchText = m_searchBoxRight.SearchText;
			}
			RightFocusedInventory = ((m_rightOwnersControl.Controls.Count > 0) ? (m_rightOwnersControl.Controls[0] as MyGuiControlInventoryOwner).ContentGrids[0] : null);
			RefreshSelectedInventoryItem();
			ForceSelectSearchBox(m_searchBoxRight);
			RightFilterTypeIndex = m_rightFilterGroup.SelectedIndex.Value;
		}

		private void RefreshSelectedInventoryItem()
		{
			if (FocusedGridControl != null)
			{
				m_selectedInventory = (MyInventory)FocusedGridControl.UserData;
				MyGuiGridItem selectedItem = FocusedGridControl.SelectedItem;
				m_selectedInventoryItem = ((selectedItem != null) ? ((MyPhysicalInventoryItem?)selectedItem.UserData) : null);
				if (FocusedGridControl?.Owner?.Owner == m_leftOwnersControl)
				{
					LeftFocusedInventory = FocusedGridControl;
				}
				else if (FocusedGridControl?.Owner?.Owner == m_rightOwnersControl)
				{
					RightFocusedInventory = FocusedGridControl;
				}
			}
			else
			{
				m_selectedInventory = null;
				m_selectedInventoryItem = null;
			}
			if (m_throwOutButton != null)
			{
				m_throwOutButton.Enabled = m_selectedInventoryItem.HasValue && m_selectedInventoryItem.HasValue && FocusedOwnerControl != null && FocusedOwnerControl.InventoryOwner == m_userAsOwner;
				if (m_throwOutButton.Enabled)
				{
					m_throwOutButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ThrowOut);
				}
				else
				{
					m_throwOutButton.SetToolTip(MySpaceTexts.ToolTipTerminalInventory_ThrowOutDisabled);
				}
			}
			if (m_selectedToProductionButton != null)
			{
				if (m_selectedInventoryItem.HasValue && m_selectedInventoryItem.Value.Content != null && m_interactedAsEntity != null)
				{
					MyDefinitionId id = m_selectedInventoryItem.Value.Content.GetId();
					MyBlueprintDefinitionBase myBlueprintDefinitionBase = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(id);
					m_selectedToProductionButton.Enabled = myBlueprintDefinitionBase != null;
				}
				else
				{
					m_selectedToProductionButton.Enabled = false;
				}
			}
			if (m_depositAllButton != null)
			{
				m_depositAllButton.Enabled = m_interactedAsEntity != null;
			}
			if (MySession.Static == null || MySession.Static.LocalCharacter == null)
			{
				return;
			}
			if (m_addToProductionButton != null)
			{
				MyGuiControlButton addToProductionButton = m_addToProductionButton;
				int enabled;
				if (m_interactedAsEntity != null && m_interactedAsEntity.Parent is MyCubeGrid)
				{
					IReadOnlyList<MyIdentity.BuildPlanItem> buildPlanner = MySession.Static.LocalCharacter.BuildPlanner;
					enabled = ((buildPlanner != null && buildPlanner.Count > 0) ? 1 : 0);
				}
				else
				{
					enabled = 0;
				}
				addToProductionButton.Enabled = (byte)enabled != 0;
			}
			if (m_withdrawButton != null)
			{
				MyGuiControlButton withdrawButton = m_withdrawButton;
				int enabled2;
				if (m_interactedAsEntity != null)
				{
					IReadOnlyList<MyIdentity.BuildPlanItem> buildPlanner2 = MySession.Static.LocalCharacter.BuildPlanner;
					enabled2 = ((buildPlanner2 != null && buildPlanner2.Count > 0) ? 1 : 0);
				}
				else
				{
					enabled2 = 0;
				}
				withdrawButton.Enabled = (byte)enabled2 != 0;
			}
		}

		private MyCubeGrid GetInteractedGrid()
		{
			if (m_interactedAsEntity == null)
			{
				return null;
			}
			return m_interactedAsEntity.Parent as MyCubeGrid;
		}

		private void ForceSelectSearchBox(MyGuiControlSearchBox searchBox)
		{
			IMyGuiControlsOwner myGuiControlsOwner = searchBox;
			while (myGuiControlsOwner.Owner != null)
			{
				myGuiControlsOwner = myGuiControlsOwner.Owner;
			}
			MyGuiScreenBase myGuiScreenBase = myGuiControlsOwner as MyGuiScreenBase;
			if (myGuiScreenBase != null)
			{
				myGuiScreenBase.FocusedControl = searchBox.TextBox;
			}
		}

		private void ForceSelectFirstInList(MyGuiControlList list)
<<<<<<< HEAD
		{
			if (list.Controls.Count <= 0)
			{
				return;
			}
			IMyGuiControlsOwner myGuiControlsOwner = list;
			while (myGuiControlsOwner.Owner != null)
			{
				myGuiControlsOwner = myGuiControlsOwner.Owner;
			}
			MyGuiScreenBase myGuiScreenBase = myGuiControlsOwner as MyGuiScreenBase;
			if (myGuiScreenBase == null)
			{
				return;
			}
			foreach (MyGuiControlBase control in list.Controls)
			{
				MyGuiControlInventoryOwner myGuiControlInventoryOwner;
				if ((myGuiControlInventoryOwner = control as MyGuiControlInventoryOwner) != null && myGuiControlInventoryOwner.Visible && myGuiControlInventoryOwner.ContentGrids.Count > 0)
				{
					myGuiScreenBase.FocusedControl = myGuiControlInventoryOwner.ContentGrids[0];
					break;
				}
			}
		}

		private void ApplyTypeGroupSelectionChange(MyGuiControlRadioButtonGroup obj, MyGuiControlList targetControlList, MyInventoryOwnerTypeEnum? filterType, MyGuiControlRadioButtonGroup filterButtonGroup, MyGuiControlCheckbox showEmpty, MyGuiControlLabel showEmptyLabel, MyGuiControlSearchBox searchBox, bool isLeftControllist)
		{
=======
		{
			if (list.Controls.Count <= 0)
			{
				return;
			}
			IMyGuiControlsOwner myGuiControlsOwner = list;
			while (myGuiControlsOwner.Owner != null)
			{
				myGuiControlsOwner = myGuiControlsOwner.Owner;
			}
			MyGuiScreenBase myGuiScreenBase = myGuiControlsOwner as MyGuiScreenBase;
			if (myGuiScreenBase == null)
			{
				return;
			}
			foreach (MyGuiControlBase control in list.Controls)
			{
				MyGuiControlInventoryOwner myGuiControlInventoryOwner;
				if ((myGuiControlInventoryOwner = control as MyGuiControlInventoryOwner) != null && myGuiControlInventoryOwner.Visible && myGuiControlInventoryOwner.ContentGrids.Count > 0)
				{
					myGuiScreenBase.FocusedControl = myGuiControlInventoryOwner.ContentGrids[0];
					break;
				}
			}
		}

		private void ApplyTypeGroupSelectionChange(MyGuiControlRadioButtonGroup obj, MyGuiControlList targetControlList, MyInventoryOwnerTypeEnum? filterType, MyGuiControlRadioButtonGroup filterButtonGroup, MyGuiControlCheckbox showEmpty, MyGuiControlLabel showEmptyLabel, MyGuiControlSearchBox searchBox, bool isLeftControllist)
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			bool flag = false;
			switch (obj.SelectedButton.VisualStyle)
			{
			case MyGuiControlRadioButtonStyleEnum.FilterCharacter:
				flag = false;
				showEmpty.Visible = false;
				showEmptyLabel.Visible = false;
				searchBox.Visible = false;
				targetControlList.Position = (isLeftControllist ? new Vector2(-0.46f, -0.276f) : new Vector2(0.4595f, -0.276f));
				targetControlList.Size = m_controlListFullSize;
				if (targetControlList == m_leftOwnersControl)
				{
					CreateInventoryControlInList(m_userAsOwner, targetControlList);
				}
				else
				{
					CreateInventoryControlInList(m_interactedAsOwner, targetControlList);
				}
				ForceSelectFirstInList(targetControlList);
				break;
			case MyGuiControlRadioButtonStyleEnum.FilterGrid:
			{
				flag = true;
				bool flag2 = ((targetControlList == m_leftOwnersControl) ? (LeftFilterTypeIndex == 2) : (RightFilterTypeIndex == 2));
				CreateInventoryControlsInList(flag2 ? m_interactedGridOwnersMechanical : m_interactedGridOwners, targetControlList, filterType);
				showEmpty.Visible = true;
				showEmptyLabel.Visible = true;
				searchBox.Visible = true;
				searchBox.SearchText = searchBox.SearchText;
				targetControlList.Position = (isLeftControllist ? new Vector2(-0.46f, -0.227f) : new Vector2(0.4595f, -0.227f));
				targetControlList.Size = m_controlListSizeWithSearch;
				ForceSelectSearchBox(searchBox);
				break;
			}
			}
			foreach (MyGuiControlRadioButton item in filterButtonGroup)
			{
				bool visible = (item.Enabled = flag);
				item.Visible = visible;
			}
			if (isLeftControllist)
			{
				LeftFilter = obj.SelectedButton.VisualStyle;
			}
			else
			{
				RightFilter = obj.SelectedButton.VisualStyle;
			}
			RefreshSelectedInventoryItem();
		}

		private void ConveyorSystem_BlockAdded(MyCubeBlock obj)
		{
			m_interactedGridOwners.Add(obj);
			if (LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterCharacter)
			{
				LeftTypeGroup_SelectedChanged(m_leftTypeGroup);
			}
			if (RightFilter == MyGuiControlRadioButtonStyleEnum.FilterCharacter)
			{
				RightTypeGroup_SelectedChanged(m_rightTypeGroup);
			}
			if (m_dragAndDropInfo != null)
			{
				ClearDisabledControls();
				DisableInvalidWhileDragging();
			}
		}

		private void ConveyorSystem_BlockRemoved(MyCubeBlock obj)
		{
			m_interactedGridOwners.Remove(obj);
			UpdateSelection();
			if (m_dragAndDropInfo != null)
			{
				ClearDisabledControls();
				DisableInvalidWhileDragging();
			}
		}

		private void ConveyorSystemMechanical_BlockAdded(MyCubeBlock obj)
		{
			m_interactedGridOwnersMechanical.Add(obj);
			if (LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
			{
				LeftTypeGroup_SelectedChanged(m_leftTypeGroup);
			}
			if (RightFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
			{
				RightTypeGroup_SelectedChanged(m_rightTypeGroup);
			}
			if (m_dragAndDropInfo != null)
			{
				ClearDisabledControls();
				DisableInvalidWhileDragging();
			}
		}

		private void ConveyorSystemMechanical_BlockRemoved(MyCubeBlock obj)
		{
			m_interactedGridOwnersMechanical.Remove(obj);
			UpdateSelection();
			if (m_dragAndDropInfo != null)
			{
				ClearDisabledControls();
				DisableInvalidWhileDragging();
			}
		}

		private void UpdateSelection()
		{
			if (LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid || RightFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
			{
				InvalidateBeforeDraw();
			}
		}

		public override void UpdateBeforeDraw(MyGuiScreenBase screen)
		{
			base.UpdateBeforeDraw(screen);
			if (m_dirtyDraw)
			{
				m_dirtyDraw = false;
				if (LeftFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
				{
					LeftTypeGroup_SelectedChanged(m_leftTypeGroup);
				}
				if (RightFilter == MyGuiControlRadioButtonStyleEnum.FilterGrid)
				{
					RightTypeGroup_SelectedChanged(m_rightTypeGroup);
				}
			}
		}

		private void LeftTypeGroup_SelectedChanged(MyGuiControlRadioButtonGroup obj)
		{
			ApplyTypeGroupSelectionChange(obj, m_leftOwnersControl, m_leftFilterType, m_leftFilterGroup, m_hideEmptyLeft, m_hideEmptyLeftLabel, m_searchBoxLeft, isLeftControllist: true);
			m_leftOwnersControl.SetScrollBarPage();
			if (obj.SelectedIndex.HasValue)
			{
				m_persistentRadioSelectionLeft = obj.SelectedIndex.Value;
			}
			if (!CheckFocusedInventoryVisibilityLeft())
			{
				SelectFirstLeftInventory();
			}
			if (m_dragAndDropInfo != null)
			{
				DisableInvalidWhileDragging();
			}
		}

		private void RightTypeGroup_SelectedChanged(MyGuiControlRadioButtonGroup obj)
		{
			ApplyTypeGroupSelectionChange(obj, m_rightOwnersControl, m_rightFilterType, m_rightFilterGroup, m_hideEmptyRight, m_hideEmptyRightLabel, m_searchBoxRight, isLeftControllist: false);
			m_rightOwnersControl.SetScrollBarPage();
			if (obj.SelectedIndex.HasValue)
			{
				m_persistentRadioSelectionRight = obj.SelectedIndex.Value;
			}
			if (!CheckFocusedInventoryVisibilityRight())
			{
				SelectFirstRightInventory();
			}
			if (m_dragAndDropInfo != null)
			{
				DisableInvalidWhileDragging();
			}
		}

		private void ThrowOutButton_OnButtonClick(MyGuiControlButton sender)
		{
			MyEntity inventoryOwner = FocusedOwnerControl.InventoryOwner;
			if (m_selectedInventoryItem.HasValue && inventoryOwner != null && FocusedOwnerControl.InventoryOwner == m_userAsOwner)
			{
				MyPhysicalInventoryItem value = m_selectedInventoryItem.Value;
				if (FocusedGridControl.SelectedIndex.HasValue)
				{
					m_selectedInventory.DropItem(FocusedGridControl.SelectedIndex.Value, value.Amount);
				}
			}
			RefreshSelectedInventoryItem();
		}

		private void interactedObjectButton_OnButtonClick(MyGuiControlButton sender)
		{
			CreateInventoryControlInList(m_interactedAsOwner, m_rightOwnersControl);
		}

		private void grid_ItemSelected(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			MyGuiControlGrid myGuiControlGrid2 = (FocusedGridControl = sender);
			FocusedOwnerControl = (MyGuiControlInventoryOwner)myGuiControlGrid2.Owner;
			RefreshSelectedInventoryItem();
		}

		private void grid_ItemDragged(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			if (!MyInput.Static.IsAnyShiftKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed())
			{
				StartDragging(MyDropHandleType.MouseRelease, sender, ref eventArgs);
			}
		}

		private void grid_ItemDoubleClicked(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			if (!MyInput.Static.IsAnyShiftKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed())
			{
				MyPhysicalInventoryItem item = (MyPhysicalInventoryItem)sender.GetItemAt(eventArgs.ItemIndex).UserData;
				TransferToOppositeFirst(item, sender);
				RefreshSelectedInventoryItem();
			}
		}

		private void grid_ItemClicked(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			bool flag = MyInput.Static.IsAnyCtrlKeyPressed();
			bool flag2 = MyInput.Static.IsAnyShiftKeyPressed();
			if (flag || flag2)
			{
				MyPhysicalInventoryItem item = (MyPhysicalInventoryItem)sender.GetItemAt(eventArgs.ItemIndex).UserData;
				item.Amount = MyFixedPoint.Min(((!flag2) ? 1 : 100) * ((!flag) ? 1 : 10), item.Amount);
				TransferToOppositeFirst(item, sender);
				RefreshSelectedInventoryItem();
			}
			else if (((MyPhysicalInventoryItem)sender.GetItemAt(eventArgs.ItemIndex).UserData).Content != null)
			{
				MyInventory myInventory = FocusedGridControl.UserData as MyInventory;
<<<<<<< HEAD
				MyShipController myShipController;
				if (myInventory != null && !(myInventory.Owner is MyCharacter) && myInventory.Owner == MySession.Static.ControlledEntity && (myShipController = myInventory.Owner as MyShipController) != null)
				{
					_ = myShipController.Pilot;
=======
				MyCharacter myCharacter = null;
				if (myInventory != null)
				{
					myCharacter = myInventory.Owner as MyCharacter;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private void grid_ItemReleased(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			ActivateItemKeyboard(sender, eventArgs.ItemIndex, eventArgs.Button);
		}

		private void ActivateItemKeyboard(MyGuiControlGrid sender, int index, MySharedButtonsEnum button)
		{
			MyPhysicalInventoryItem item = (MyPhysicalInventoryItem)sender.GetItemAt(index).UserData;
			if (item.Content == null)
			{
				return;
			}
			MyInventory myInventory = sender.UserData as MyInventory;
			MyCharacter myCharacter = null;
			if (myInventory == null)
			{
				return;
			}
			myCharacter = myInventory.Owner as MyCharacter;
			if (myCharacter == null && item.Content.GetType() != typeof(MyObjectBuilder_Datapad))
			{
				MyShipController myShipController;
<<<<<<< HEAD
				if (myInventory.Owner == MySession.Static.ControlledEntity && (myShipController = myInventory.Owner as MyShipController) != null)
=======
				if (myCharacter == null && myInventory.Owner == MySession.Static.ControlledEntity && (myShipController = myInventory.Owner as MyShipController) != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					myCharacter = myShipController.Pilot;
				}
			}
			else
			{
				MyUsableItemHelper.ItemActivatedGridKeyboard(item, myInventory, myCharacter, button);
			}
		}

		private bool ActivateItemGamepad(MyGuiControlGrid sender, int index, MyGridItemAction action, bool pressed)
		{
<<<<<<< HEAD
			if (sender.GetItemAt(index) == null)
			{
=======
			ActivateItemKeyboard(sender, eventArgs.ItemIndex, eventArgs.Button);
		}

		private void ActivateItemKeyboard(MyGuiControlGrid sender, int index, MySharedButtonsEnum button)
		{
			MyPhysicalInventoryItem item = (MyPhysicalInventoryItem)sender.GetItemAt(index).UserData;
			if (item.Content != null)
			{
				MyInventory myInventory = sender.UserData as MyInventory;
				MyCharacter myCharacter = null;
				if (myInventory != null)
				{
					myCharacter = myInventory.Owner as MyCharacter;
				}
				MyShipController myShipController;
				if (myCharacter == null && myInventory.Owner == MySession.Static.ControlledEntity && (myShipController = myInventory.Owner as MyShipController) != null)
				{
					myCharacter = myShipController.Pilot;
				}
				if (myCharacter != null)
				{
					MyUsableItemHelper.ItemActivatedGridKeyboard(item, myInventory, myCharacter, button);
				}
			}
		}

		private bool ActivateItemGamepad(MyGuiControlGrid sender, int index, MyGridItemAction action, bool pressed)
		{
			if (sender.GetItemAt(index) == null)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return false;
			}
			MyPhysicalInventoryItem item = (MyPhysicalInventoryItem)sender.GetItemAt(index).UserData;
			if (action == MyGridItemAction.Button_A)
			{
				if (pressed)
<<<<<<< HEAD
				{
					if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED) && !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED) && (RightFocusedInventory != null || LeftFocusedInventory != null))
					{
						if (RightFocusedInventory == null)
						{
							RightFocusedInventory = LeftFocusedInventory;
						}
						if (LeftFocusedInventory == null)
						{
							LeftFocusedInventory = RightFocusedInventory;
						}
						m_transferTimer = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
						m_isTransferTimerActive = true;
						m_transferData.Item = item;
						m_transferData.ToGrid = (((sender.Owner as MyGuiControlInventoryOwner).Owner == m_leftOwnersControl) ? RightFocusedInventory : LeftFocusedInventory);
						m_transferData.From = (MyInventory)sender.UserData;
						m_transferData.To = (MyInventory)m_transferData.ToGrid.UserData;
						m_transferData.IndexFrom = index;
						m_transferData.IndexTo = m_transferData.ToGrid.GetFirstEmptySlotIndex();
						return true;
					}
				}
				else
=======
				{
					if (!MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_LEFT, MyControlStateType.PRESSED) && !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyControlStateType.PRESSED) && (RightFocusedInventory != null || LeftFocusedInventory != null))
					{
						if (RightFocusedInventory == null)
						{
							RightFocusedInventory = LeftFocusedInventory;
						}
						if (LeftFocusedInventory == null)
						{
							LeftFocusedInventory = RightFocusedInventory;
						}
						m_transferTimer = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
						m_isTransferTimerActive = true;
						m_transferData.Item = item;
						m_transferData.ToGrid = (((sender.Owner as MyGuiControlInventoryOwner).Owner == m_leftOwnersControl) ? RightFocusedInventory : LeftFocusedInventory);
						m_transferData.From = (MyInventory)sender.UserData;
						m_transferData.To = (MyInventory)m_transferData.ToGrid.UserData;
						m_transferData.IndexFrom = index;
						m_transferData.IndexTo = m_transferData.ToGrid.GetFirstEmptySlotIndex();
						return true;
					}
				}
				else
				{
					m_isTransferTimerActive = false;
					MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
					if (TransferToOppositeFirst(item, sender))
					{
						RefreshSelectedInventoryItem();
						return true;
					}
				}
			}
			if (item.Content == null)
			{
				return false;
			}
			if (pressed)
			{
				MyInventory myInventory = sender.UserData as MyInventory;
				MyCharacter myCharacter = null;
				if (myInventory != null)
				{
					myCharacter = myInventory.Owner as MyCharacter;
				}
				MyShipController myShipController;
				if (myCharacter == null && myInventory.Owner == MySession.Static.ControlledEntity && (myShipController = myInventory.Owner as MyShipController) != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					m_isTransferTimerActive = false;
					MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
					if (TransferToOppositeFirst(item, sender))
					{
						RefreshSelectedInventoryItem();
						return true;
					}
				}
			}
			if (item.Content == null)
			{
				return false;
			}
			if (pressed)
			{
				MyInventory myInventory = sender.UserData as MyInventory;
				MyCharacter myCharacter = null;
				if (myInventory != null)
				{
<<<<<<< HEAD
					myCharacter = myInventory.Owner as MyCharacter;
					if (myCharacter != null)
					{
						return MyUsableItemHelper.ItemActivatedGridGamepad(item, myInventory, myCharacter, action);
					}
					MyShipController myShipController;
					if (myInventory.Owner == MySession.Static.ControlledEntity && (myShipController = myInventory.Owner as MyShipController) != null)
					{
						myCharacter = myShipController.Pilot;
					}
=======
					return MyUsableItemHelper.ItemActivatedGridGamepad(item, myInventory, myCharacter, action);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			return false;
		}

		private void dragDrop_OnItemDropped(object sender, MyDragAndDropEventArgs eventArgs)
		{
			if (eventArgs.DropTo != null)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
				MyPhysicalInventoryItem inventoryItem = (MyPhysicalInventoryItem)eventArgs.Item.UserData;
				MyGuiControlGrid grid = eventArgs.DragFrom.Grid;
				MyGuiControlGrid dstGrid = eventArgs.DropTo.Grid;
				_ = (MyGuiControlInventoryOwner)grid.Owner;
				if (!(dstGrid.Owner is MyGuiControlInventoryOwner))
				{
					return;
				}
				MyInventory srcInventory = (MyInventory)grid.UserData;
				MyInventory dstInventory = (MyInventory)dstGrid.UserData;
				if (grid == dstGrid)
				{
					if (eventArgs.DragButton == MySharedButtonsEnum.Secondary)
					{
						ShowAmountTransferDialog(inventoryItem, delegate(float amount)
						{
							if (amount != 0f && srcInventory.IsItemAt(eventArgs.DragFrom.ItemIndex))
							{
								inventoryItem.Amount = (MyFixedPoint)amount;
								CorrectItemAmount(ref inventoryItem);
								MyInventory.TransferByUser(srcInventory, srcInventory, inventoryItem.ItemId, eventArgs.DropTo.ItemIndex, inventoryItem.Amount);
								if (dstGrid.IsValidIndex(eventArgs.DropTo.ItemIndex))
								{
									dstGrid.SelectedIndex = eventArgs.DropTo.ItemIndex;
								}
								else
								{
									dstGrid.SelectLastItem();
								}
								RefreshSelectedInventoryItem();
							}
						});
					}
					else
					{
						MyInventory.TransferByUser(srcInventory, srcInventory, inventoryItem.ItemId, eventArgs.DropTo.ItemIndex);
						if (dstGrid.IsValidIndex(eventArgs.DropTo.ItemIndex))
						{
							dstGrid.SelectedIndex = eventArgs.DropTo.ItemIndex;
						}
						else
						{
							dstGrid.SelectLastItem();
						}
						RefreshSelectedInventoryItem();
					}
				}
				else if (eventArgs.DragButton == MySharedButtonsEnum.Secondary)
				{
					ShowAmountTransferDialog(inventoryItem, delegate(float amount)
					{
						if (amount != 0f && srcInventory.IsItemAt(eventArgs.DragFrom.ItemIndex))
						{
							inventoryItem.Amount = (MyFixedPoint)amount;
							CorrectItemAmount(ref inventoryItem);
							MyInventory.TransferByUser(srcInventory, dstInventory, inventoryItem.ItemId, eventArgs.DropTo.ItemIndex, inventoryItem.Amount);
							RefreshSelectedInventoryItem();
						}
					});
				}
				else
				{
					MyInventory.TransferByUser(srcInventory, dstInventory, inventoryItem.ItemId, eventArgs.DropTo.ItemIndex);
					RefreshSelectedInventoryItem();
				}
			}
			else
			{
				MyGuiControlGridDragAndDrop myGuiControlGridDragAndDrop = (MyGuiControlGridDragAndDrop)sender;
				if (m_throwOutButton.Enabled && (myGuiControlGridDragAndDrop.IsEmptySpace() || IsDroppedOnThrowOutButton(myGuiControlGridDragAndDrop)))
				{
					ThrowOutButton_OnButtonClick(m_throwOutButton);
				}
			}
			ClearDisabledControls();
			m_dragAndDropInfo = null;
		}

		private void ClearDisabledControls()
		{
			foreach (MyGuiControlGrid item in m_controlsDisabledWhileDragged)
			{
				item.Enabled = true;
			}
			m_controlsDisabledWhileDragged.Clear();
		}

		private bool IsDroppedOnThrowOutButton(MyGuiControlGridDragAndDrop dragDrop)
		{
			foreach (MyGuiControlBase dropToControl in dragDrop.DropToControls)
			{
				if (dropToControl.Name.Equals("ThrowOutButton", StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		private static void CorrectItemAmount(ref MyPhysicalInventoryItem dragItem)
		{
			_ = dragItem.Content.TypeId;
		}

		private void inventoryControl_SizeChanged(MyGuiControlBase obj)
		{
			((MyGuiControlList)obj.Owner).Recalculate();
		}

		private void ownerControl_InventoryContentsChanged(MyGuiControlInventoryOwner control)
		{
			if (control == FocusedOwnerControl)
			{
				RefreshSelectedInventoryItem();
			}
			UpdateDisabledControlsWhileDragging(control);
		}

		private void UpdateDisabledControlsWhileDragging(MyGuiControlInventoryOwner control)
		{
			if (m_controlsDisabledWhileDragged.Count == 0)
			{
				return;
			}
			MyEntity inventoryOwner = control.InventoryOwner;
			for (int i = 0; i < inventoryOwner.InventoryCount; i++)
			{
				MyGuiControlGrid myGuiControlGrid = control.ContentGrids[i];
				if (m_controlsDisabledWhileDragged.Contains(myGuiControlGrid) && myGuiControlGrid.Enabled)
				{
					myGuiControlGrid.Enabled = false;
				}
			}
		}

		private void HideEmptyLeft_Checked(MyGuiControlCheckbox obj)
		{
			if (m_leftFilterType != MyInventoryOwnerTypeEnum.Character)
			{
				SearchInList(m_searchBoxLeft.TextBox, m_leftOwnersControl, obj.IsChecked);
				CheckFocusedInventoryVisibilityLeft();
			}
		}

		private bool CheckFocusedInventoryVisibilityLeft()
		{
			if (LeftFocusedInventory == null || LeftFocusedInventory.Owner == null || !LeftFocusedInventory.Owner.Visible)
			{
				LeftFocusedInventory = null;
				return false;
			}
			return true;
		}

		private void SelectFirstLeftInventory()
		{
			foreach (MyGuiControlBase control in m_leftOwnersControl.Controls)
			{
				if (control.Visible)
				{
					MyGuiControlInventoryOwner myGuiControlInventoryOwner = control as MyGuiControlInventoryOwner;
					if (myGuiControlInventoryOwner.ContentGrids.Count > 0)
					{
						LeftFocusedInventory = myGuiControlInventoryOwner.ContentGrids[0];
					}
				}
			}
		}

		private void SelectFirstRightInventory()
		{
			foreach (MyGuiControlBase control in m_rightOwnersControl.Controls)
			{
				if (control.Visible)
				{
					MyGuiControlInventoryOwner myGuiControlInventoryOwner = control as MyGuiControlInventoryOwner;
					if (myGuiControlInventoryOwner.ContentGrids.Count > 0)
					{
						RightFocusedInventory = myGuiControlInventoryOwner.ContentGrids[0];
					}
				}
			}
		}

		private void HideEmptyRight_Checked(MyGuiControlCheckbox obj)
		{
			if (m_rightFilterType != MyInventoryOwnerTypeEnum.Character)
			{
				SearchInList(m_searchBoxRight.TextBox, m_rightOwnersControl, obj.IsChecked);
				CheckFocusedInventoryVisibilityRight();
			}
		}

		private bool CheckFocusedInventoryVisibilityRight()
		{
			if (RightFocusedInventory == null || RightFocusedInventory.Owner == null || !RightFocusedInventory.Owner.Visible)
			{
				RightFocusedInventory = null;
				return false;
			}
			return true;
		}

		private void BlockSearchLeft_TextChanged(string obj)
		{
			if (m_leftFilterType == MyInventoryOwnerTypeEnum.Character)
			{
				return;
			}
			SearchInList(m_searchBoxLeft.TextBox, m_leftOwnersControl, m_hideEmptyLeft.IsChecked);
			MyGuiControlInventoryOwner myGuiControlInventoryOwner = null;
			foreach (MyGuiControlBase control in m_leftOwnersControl.Controls)
			{
				MyGuiControlInventoryOwner myGuiControlInventoryOwner2;
				if (control.Visible && (myGuiControlInventoryOwner2 = control as MyGuiControlInventoryOwner) != null)
				{
					myGuiControlInventoryOwner = myGuiControlInventoryOwner2;
					break;
				}
			}
			if (myGuiControlInventoryOwner != null && myGuiControlInventoryOwner.ContentGrids.Count > 0)
			{
				FocusedGridControl = myGuiControlInventoryOwner.ContentGrids[0];
				FocusedOwnerControl = myGuiControlInventoryOwner;
				RefreshSelectedInventoryItem();
			}
		}

		private void BlockSearchRight_TextChanged(string obj)
		{
			if (m_rightFilterType == MyInventoryOwnerTypeEnum.Character)
			{
				return;
			}
			SearchInList(m_searchBoxRight.TextBox, m_rightOwnersControl, m_hideEmptyRight.IsChecked);
			MyGuiControlInventoryOwner myGuiControlInventoryOwner = null;
			foreach (MyGuiControlBase control in m_rightOwnersControl.Controls)
			{
				MyGuiControlInventoryOwner myGuiControlInventoryOwner2;
				if (control.Visible && (myGuiControlInventoryOwner2 = control as MyGuiControlInventoryOwner) != null)
				{
					myGuiControlInventoryOwner = myGuiControlInventoryOwner2;
					break;
				}
			}
			if (myGuiControlInventoryOwner != null && myGuiControlInventoryOwner.ContentGrids.Count > 0)
			{
				FocusedGridControl = myGuiControlInventoryOwner.ContentGrids[0];
				FocusedOwnerControl = myGuiControlInventoryOwner;
				RefreshSelectedInventoryItem();
			}
		}

		private void SearchInList(MyGuiControlTextbox searchText, MyGuiControlList list, bool hideEmpty)
		{
			if (searchText.Text != "")
			{
				string[] array = searchText.Text.ToLower().Split(new char[1] { ' ' });
				foreach (MyGuiControlBase control in list.Controls)
				{
					MyEntity inventoryOwner = (control as MyGuiControlInventoryOwner).InventoryOwner;
					string text = inventoryOwner.DisplayNameText.ToString().ToLower();
					bool flag = true;
					bool flag2 = true;
					string[] array2 = array;
					foreach (string value in array2)
					{
						if (!text.Contains(value))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						for (int j = 0; j < inventoryOwner.InventoryCount; j++)
						{
							foreach (MyPhysicalInventoryItem item in inventoryOwner.GetInventory(j).GetItems())
							{
								bool flag3 = true;
								MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(item.Content);
								if (physicalItemDefinition == null || string.IsNullOrEmpty(physicalItemDefinition.DisplayNameText))
								{
									continue;
								}
								string text2 = physicalItemDefinition.DisplayNameText.ToString().ToLower();
								array2 = array;
								foreach (string value2 in array2)
								{
									if (!text2.Contains(value2))
									{
										flag3 = false;
										break;
									}
								}
								if (flag3)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								break;
							}
						}
					}
					if (flag)
					{
						for (int k = 0; k < inventoryOwner.InventoryCount; k++)
						{
							if (inventoryOwner.GetInventory(k).CurrentMass != 0)
							{
								flag2 = false;
								break;
							}
						}
						control.Visible = ((!(hideEmpty && flag2)) ? true : false);
					}
					else
					{
						control.Visible = false;
					}
				}
			}
			else
			{
				foreach (MyGuiControlBase control2 in list.Controls)
				{
					bool flag4 = true;
					MyEntity inventoryOwner2 = (control2 as MyGuiControlInventoryOwner).InventoryOwner;
					for (int l = 0; l < inventoryOwner2.InventoryCount; l++)
					{
						if (inventoryOwner2.GetInventory(l).CurrentMass != 0)
						{
							flag4 = false;
							break;
						}
					}
					if (hideEmpty && flag4)
					{
						control2.Visible = false;
					}
					else
					{
						control2.Visible = true;
					}
				}
			}
			list.SetScrollBarPage();
		}

		private void MoveItems(int direction, MyGuiControlGrid grid, int index)
		{
			MyPhysicalInventoryItem dragItem = (MyPhysicalInventoryItem)grid.GetItemAt(index).UserData;
			MyInventory myInventory = (MyInventory)grid.UserData;
			CorrectItemAmount(ref dragItem);
			int num = index;
			switch (direction)
			{
			case 0:
				num = index - grid.ColumnsCount;
				break;
			case 1:
				num = index - 1;
				break;
			case 2:
				num = index + 1;
				break;
			case 3:
				num = index + grid.ColumnsCount;
				break;
			}
			MyInventory.TransferByUser(myInventory, myInventory, dragItem.ItemId, num, dragItem.Amount);
			if (!grid.IsValidIndex(num))
			{
				grid.SelectLastItem();
			}
			RefreshSelectedInventoryItem();
		}

		public override void HandleInput()
		{
			base.HandleInput();
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				selectedToProductionButton_ButtonClicked(null);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.VIEW))
<<<<<<< HEAD
			{
				DepositAllButton_ButtonClicked(null);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MENU) && m_throwOutButton.Enabled)
			{
				ThrowOutButton_OnButtonClick(null);
			}
			MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			if (m_isTransferTimerActive && m_transferTimer + TRANSFER_TIMER_TIME <= myTimeSpan)
			{
				OpenTransferDialog();
				m_isTransferTimerActive = false;
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
			{
				SwitchFilter(m_leftTypeGroup, m_leftFilterGroup, m_leftFilterGamepadHelp);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
			{
				SwitchFilter(m_rightTypeGroup, m_rightFilterGroup, m_rightFilterGamepadHelp);
			}
			if (MyInput.Static.IsJoystickLastUsed != m_isJoystickLastUsed)
			{
				MyControl gameControl = MyInput.Static.GetGameControl(MyControlsSpace.BUILD_PLANNER);
				m_isJoystickLastUsed = MyInput.Static.IsJoystickLastUsed;
				if (m_throwOutButton != null)
				{
					m_throwOutButton.Visible = !m_isJoystickLastUsed;
				}
				if (m_withdrawButton != null)
				{
					m_withdrawButton.SetTooltip(m_isJoystickLastUsed ? string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_Withdraw_Controller).ToString(), gameControl) : string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_Withdraw).ToString(), gameControl));
				}
				if (m_selectedToProductionButton != null)
				{
					m_selectedToProductionButton.Visible = !m_isJoystickLastUsed;
				}
				if (m_addToProductionButton != null)
				{
					m_addToProductionButton.SetTooltip(m_isJoystickLastUsed ? string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_AddComponents_Controller).ToString(), gameControl) : string.Format(MyTexts.Get(MySpaceTexts.ToolTipTerminalInventory_AddComponents).ToString(), gameControl));
				}
				if (m_depositAllButton != null)
				{
					m_depositAllButton.Visible = !m_isJoystickLastUsed;
				}
			}
			MyGuiControlLabel leftFilterGamepadHelp = m_leftFilterGamepadHelp;
			bool visible = (m_rightFilterGamepadHelp.Visible = MyInput.Static.IsJoystickLastUsed);
			leftFilterGamepadHelp.Visible = visible;
			int? num = null;
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_UP))
			{
				num = 0;
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_LEFT))
			{
				num = 1;
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_RIGHT))
			{
				num = 2;
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_DOWN))
			{
				num = 3;
			}
			if (!num.HasValue)
			{
				return;
			}
			MyEntity inventoryOwner = FocusedOwnerControl.InventoryOwner;
			if (m_selectedInventoryItem.HasValue && inventoryOwner != null)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
				_ = m_selectedInventoryItem.Value;
				if (FocusedGridControl.SelectedIndex.HasValue)
				{
					MoveItems(num.Value, FocusedGridControl, FocusedGridControl.SelectedIndex.Value);
				}
			}
			RefreshSelectedInventoryItem();
		}

		private void SwitchFilter(MyGuiControlRadioButtonGroup typeGroup, MyGuiControlRadioButtonGroup filterGroup, MyGuiControlLabel gamepadHelp)
		{
			int num = (typeGroup.SelectedIndex.HasValue ? typeGroup.SelectedIndex.Value : 0);
			if (num != typeGroup.Count - 1 && m_interactedAsEntity != null)
			{
				typeGroup.SelectByIndex(num + 1);
			}
			else
			{
				int num2 = (filterGroup.SelectedIndex.HasValue ? filterGroup.SelectedIndex.Value : 0);
				if (num2 != filterGroup.Count - 1)
				{
=======
			{
				DepositAllButton_ButtonClicked(null);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MENU) && m_throwOutButton.Enabled)
			{
				ThrowOutButton_OnButtonClick(null);
			}
			MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			if (m_isTransferTimerActive && m_transferTimer + TRANSFER_TIMER_TIME <= myTimeSpan)
			{
				OpenTransferDialog();
				m_isTransferTimerActive = false;
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
			{
				SwitchFilter(m_leftTypeGroup, m_leftFilterGroup, m_leftFilterGamepadHelp);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
			{
				SwitchFilter(m_rightTypeGroup, m_rightFilterGroup, m_rightFilterGamepadHelp);
			}
			if (m_throwOutButton != null)
			{
				m_throwOutButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			if (m_selectedToProductionButton != null)
			{
				m_selectedToProductionButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			if (m_depositAllButton != null)
			{
				m_depositAllButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			MyGuiControlLabel leftFilterGamepadHelp = m_leftFilterGamepadHelp;
			bool visible = (m_rightFilterGamepadHelp.Visible = MyInput.Static.IsJoystickLastUsed);
			leftFilterGamepadHelp.Visible = visible;
			int? num = null;
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_UP))
			{
				num = 0;
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_LEFT))
			{
				num = 1;
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_RIGHT))
			{
				num = 2;
			}
			else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.MOVE_ITEM_DOWN))
			{
				num = 3;
			}
			if (!num.HasValue)
			{
				return;
			}
			MyEntity inventoryOwner = FocusedOwnerControl.InventoryOwner;
			if (m_selectedInventoryItem.HasValue && inventoryOwner != null)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
				_ = m_selectedInventoryItem.Value;
				if (FocusedGridControl.SelectedIndex.HasValue)
				{
					MoveItems(num.Value, FocusedGridControl, FocusedGridControl.SelectedIndex.Value);
				}
			}
			RefreshSelectedInventoryItem();
		}

		private void SwitchFilter(MyGuiControlRadioButtonGroup typeGroup, MyGuiControlRadioButtonGroup filterGroup, MyGuiControlLabel gamepadHelp)
		{
			int num = (typeGroup.SelectedIndex.HasValue ? typeGroup.SelectedIndex.Value : 0);
			if (num != typeGroup.Count - 1 && m_interactedAsEntity != null)
			{
				typeGroup.SelectByIndex(num + 1);
			}
			else
			{
				int num2 = (filterGroup.SelectedIndex.HasValue ? filterGroup.SelectedIndex.Value : 0);
				if (num2 != filterGroup.Count - 1)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					filterGroup.SelectByIndex(num2 + 1);
				}
				else
				{
					filterGroup.SelectByIndex(0);
					typeGroup.SelectByIndex(0);
				}
			}
			SetFilterGamepadHelp(typeGroup, filterGroup, gamepadHelp);
		}

		private void SetFilterGamepadHelp(MyGuiControlRadioButtonGroup typeGroup, MyGuiControlRadioButtonGroup filterGroup, MyGuiControlLabel gamepadHelp)
		{
			string text = MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_ActiveFilter) + "\n";
			switch (typeGroup.SelectedButton.VisualStyle)
			{
			case MyGuiControlRadioButtonStyleEnum.FilterCharacter:
				gamepadHelp.Text = text + MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_Character);
				return;
			case MyGuiControlRadioButtonStyleEnum.FilterGrid:
				text += MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_ShipOrStation);
				break;
			}
			text += "\n";
			switch (filterGroup.SelectedButton.VisualStyle)
			{
			case MyGuiControlRadioButtonStyleEnum.FilterAll:
				text += MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_AllInventories);
				break;
			case MyGuiControlRadioButtonStyleEnum.FilterEnergy:
				text += MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_EnergyInventories);
				break;
			case MyGuiControlRadioButtonStyleEnum.FilterShip:
				text += MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_CurrentShip);
				break;
			case MyGuiControlRadioButtonStyleEnum.FilterStorage:
				text += MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_StorageInventories);
				break;
			case MyGuiControlRadioButtonStyleEnum.FilterSystem:
				text += MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_FilterGamepadHelp_SystemInventories);
				break;
			}
			gamepadHelp.Text = text;
		}

		private void OpenTransferDialog()
		{
			if (!CanTransferItem(m_transferData.Item, m_transferData.ToGrid, out var _, out var _))
			{
				return;
			}
			ShowAmountTransferDialog(m_transferData.Item, delegate(float amount)
			{
				if (amount != 0f && m_transferData.From.IsItemAt(m_transferData.IndexFrom))
				{
					m_transferData.Item.Amount = (MyFixedPoint)amount;
					CorrectItemAmount(ref m_transferData.Item);
					MyInventory.TransferByUser(m_transferData.From, m_transferData.To, m_transferData.Item.ItemId, m_transferData.IndexTo, m_transferData.Item.Amount);
					if (m_transferData.ToGrid.IsValidIndex(m_transferData.IndexTo))
					{
						m_transferData.ToGrid.SelectedIndex = m_transferData.IndexTo;
					}
					else
					{
						m_transferData.ToGrid.SelectLastItem();
					}
					RefreshSelectedInventoryItem();
				}
			});
		}

		public MyGuiControlGrid GetDefaultFocus()
		{
			return LeftFocusedInventory;
		}
	}
}
