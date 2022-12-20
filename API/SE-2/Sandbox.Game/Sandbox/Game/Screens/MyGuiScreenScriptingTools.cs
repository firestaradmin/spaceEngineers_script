using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Collections.ObjectModel;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Globalization;
using System.Linq;
using System.Text;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Debugging;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI.DebugInputComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Entity;
<<<<<<< HEAD
=======
using VRage.Game.ModAPI.Interfaces;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Game.ObjectBuilders.Gui;
using VRage.Game.SessionComponents;
using VRage.Game.VisualScripting.Missions;
using VRage.Generics;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenScriptingTools : MyGuiScreenDebugBase
	{
		private enum ScriptingToolsScreen
		{
			Transformation,
			Cutscenes
		}

		private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);

		private static readonly float HIDDEN_PART_RIGHT = 0.04f;

		private static readonly float ITEM_HORIZONTAL_PADDING = 0.01f;

		private static readonly float ITEM_VERTICAL_PADDING = 0.005f;

		private static readonly Vector2 BUTTON_SIZE = new Vector2(0.06f, 0.03f);

		private static readonly Vector2 ITEM_SIZE = new Vector2(0.06f, 0.02f);

		public static MyGuiScreenScriptingTools Static;
<<<<<<< HEAD
=======

		private static uint m_entityCounter = 0u;

		private IMyCameraController m_previousCameraController;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private MyGuiControlButton m_setTriggerSizeButton;

		private MyGuiControlButton m_growTriggerButton;

		private MyGuiControlButton m_shrinkTriggerButton;

		private MyGuiControlListbox m_triggersListBox;

		private MyGuiControlListbox m_waypointsListBox;

		private MyGuiControlListbox m_smListBox;

		private MyGuiControlListbox m_levelScriptListBox;

		private MyGuiControlTextbox m_selectedTriggerNameBox;

		private MyGuiControlTextbox m_selectedEntityNameBox;

		private MyGuiControlTextbox m_selectedFunctionalBlockNameBox;

		private MyEntity m_selectedFunctionalBlock;

		private bool m_disablePicking;

		private readonly MyTriggerManipulator m_triggerManipulator;

		private readonly MyEntityTransformationSystem m_transformSys;

		private List<MyWaypoint> m_waypoints = new List<MyWaypoint>();

		private int m_selectedAxis;

		public MyGuiScreenScriptingTools()
			: base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - SCREEN_SIZE.X * 0.5f + HIDDEN_PART_RIGHT, 0.5f), SCREEN_SIZE, MyGuiConstants.SCREEN_BACKGROUND_COLOR, isTopMostScreen: false)
		{
			base.CanBeHidden = true;
			base.CanHideOthers = false;
			m_canCloseInCloseAllScreenCalls = true;
			m_canShareInput = true;
			m_isTopScreen = false;
			m_isTopMostScreen = false;
			Static = this;
			m_triggerManipulator = new MyTriggerManipulator((MyTriggerComponent trigger) => trigger is MyAreaTriggerComponent);
			m_transformSys = MySession.Static.GetComponent<MyEntityTransformationSystem>();
			m_transformSys.ControlledEntityChanged += TransformSysOnControlledEntityChanged;
			m_transformSys.RayCasted += TransformSysOnRayCasted;
			MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFreeMouse);
			MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
			MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER = true;
			MyDebugDrawSettings.DEBUG_DRAW_WAYPOINTS = true;
			MyDebugDrawSettings.DEBUG_DRAW_CUTSCENES = true;
			RecreateControls(constructor: true);
			InitializeWaypointList();
			UpdateWaypointList();
			UpdateTriggerList();
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (m_transformSys.DisablePicking)
			{
				m_transformSys.DisablePicking = false;
			}
			if (MyInput.Static.IsNewPrimaryButtonPressed())
			{
				Vector2 normalizedCoordinateFromScreenCoordinate = MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(MyInput.Static.GetMousePosition());
				Vector2 vector = GetPosition() - SCREEN_SIZE * 0.5f;
				if (normalizedCoordinateFromScreenCoordinate.X > vector.X)
				{
					m_transformSys.DisablePicking = true;
				}
			}
			if (!MyToolbarComponent.IsToolbarControlShown)
			{
				MyToolbarComponent.IsToolbarControlShown = true;
			}
			base.FocusedControl = null;
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape))
			{
				CloseScreen();
				return;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F11))
			{
				CloseScreen();
				MyScreenManager.AddScreen(new MyGuiScreenCutscenes());
				return;
			}
			base.HandleInput(receivedFocusInThisUpdate);
			if (MySpectatorCameraController.Static.SpectatorCameraMovement != MySpectatorCameraMovementEnum.FreeMouse)
			{
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.FreeMouse;
			}
			foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
			{
				if (!(screen is MyGuiScreenScriptingTools))
				{
					screen.HandleInput(receivedFocusInThisUpdate);
				}
			}
			HandleShortcuts();
		}

		private void HandleShortcuts()
		{
			if (!MyInput.Static.IsAnyShiftKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyAltKeyPressed())
			{
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Add))
				{
					GrowTriggerOnClick(null);
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Subtract))
				{
					ShrinkTriggerOnClick(null);
				}
			}
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
			MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.UserControlled;
			if (MySession.Static.ControlledEntity != null)
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, MySession.Static.ControlledEntity.Entity);
			}
			MyDebugDrawSettings.ENABLE_DEBUG_DRAW = false;
			MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER = false;
			MyDebugDrawSettings.DEBUG_DRAW_WAYPOINTS = false;
			MyDebugDrawSettings.DEBUG_DRAW_CUTSCENES = false;
			m_transformSys.Active = false;
			MyGuiScreenGamePlay.DisableInput = MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning;
			Static = null;
			return base.CloseScreen(isUnloading);
		}

		public override bool Update(bool hasFocus)
		{
			if (MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinition != null || MyInput.Static.IsRightMousePressed())
			{
				base.DrawMouseCursor = false;
			}
			else
			{
				base.DrawMouseCursor = true;
			}
			m_triggerManipulator.CurrentPosition = MyAPIGateway.Session.Camera.Position;
			MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
			MySession.Static.GetComponent<MySessionComponentScriptSharedStorage>();
			if (component == null)
			{
				return false;
			}
			if (component.FailedLevelScriptExceptionTexts != null)
			{
				for (int i = 0; i < component.FailedLevelScriptExceptionTexts.Length; i++)
				{
					string text = component.FailedLevelScriptExceptionTexts[i];
<<<<<<< HEAD
					if (text != null && (bool)m_levelScriptListBox.Items[i].UserData)
					{
						m_levelScriptListBox.Items[i].Text.Append(" - failed");
						m_levelScriptListBox.Items[i].ColorMask = MyTerminalControlPanel.RED_TEXT_COLOR;
						m_levelScriptListBox.Items[i].ToolTip.AddToolTip(text, 0.7f, "Red");
=======
					if (text != null && (bool)((Collection<MyGuiControlListbox.Item>)(object)m_levelScriptListBox.Items)[i].UserData)
					{
						((Collection<MyGuiControlListbox.Item>)(object)m_levelScriptListBox.Items)[i].Text.Append(" - failed");
						((Collection<MyGuiControlListbox.Item>)(object)m_levelScriptListBox.Items)[i].ColorMask = MyTerminalControlPanel.RED_TEXT_COLOR;
						((Collection<MyGuiControlListbox.Item>)(object)m_levelScriptListBox.Items)[i].ToolTip.AddToolTip(text, 0.7f, "Red");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			if (component.SMManager != null && component.SMManager.RunningMachines != null)
			{
				foreach (MyVSStateMachine stateMachine in component.SMManager.RunningMachines)
				{
					int num = m_smListBox.Items.FindIndex((MyGuiControlListbox.Item item) => (MyVSStateMachine)item.UserData == stateMachine);
					if (num == -1)
					{
						m_smListBox.Add(new MyGuiControlListbox.Item(new StringBuilder(stateMachine.Name), userData: stateMachine, toolTip: MyTexts.Get(MyCommonTexts.Scripting_Tooltip_Cursors).ToString()));
<<<<<<< HEAD
						num = m_smListBox.Items.Count - 1;
					}
					MyGuiControlListbox.Item item2 = m_smListBox.Items[num];
					for (int num2 = item2.ToolTip.ToolTips.Count - 1; num2 >= 0; num2--)
					{
						MyColoredText myColoredText = item2.ToolTip.ToolTips[num2];
=======
						num = ((Collection<MyGuiControlListbox.Item>)(object)m_smListBox.Items).Count - 1;
					}
					MyGuiControlListbox.Item item2 = ((Collection<MyGuiControlListbox.Item>)(object)m_smListBox.Items)[num];
					for (int num2 = ((Collection<MyColoredText>)(object)item2.ToolTip.ToolTips).Count - 1; num2 >= 0; num2--)
					{
						MyColoredText myColoredText = ((Collection<MyColoredText>)(object)item2.ToolTip.ToolTips)[num2];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						bool flag = false;
						foreach (MyStateMachineCursor activeCursor in stateMachine.ActiveCursors)
						{
							if (myColoredText.Text.CompareTo(activeCursor.Node.Name) == 0)
							{
								flag = true;
								break;
							}
						}
						if (!flag && num2 != 0)
						{
<<<<<<< HEAD
							item2.ToolTip.ToolTips.RemoveAtFast(num2);
=======
							((IList<MyColoredText>)item2.ToolTip.ToolTips).RemoveAtFast(num2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					if (stateMachine.ActiveCursors == null)
					{
						continue;
					}
					foreach (MyStateMachineCursor activeCursor2 in stateMachine.ActiveCursors)
					{
						bool flag2 = false;
<<<<<<< HEAD
						for (int num3 = item2.ToolTip.ToolTips.Count - 1; num3 >= 0; num3--)
						{
							if (item2.ToolTip.ToolTips[num3].Text.CompareTo(activeCursor2.Node.Name) == 0)
=======
						for (int num3 = ((Collection<MyColoredText>)(object)item2.ToolTip.ToolTips).Count - 1; num3 >= 0; num3--)
						{
							if (((Collection<MyColoredText>)(object)item2.ToolTip.ToolTips)[num3].Text.CompareTo(activeCursor2.Node.Name) == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							item2.ToolTip.AddToolTip(activeCursor2.Node.Name);
						}
					}
				}
			}
			if (true)
			{
				MyVisualScriptingDebugInputComponent.DrawVariables();
			}
			return base.Update(hasFocus);
		}

		public void UpdateTriggerList()
		{
			List<MyTriggerComponent> allTriggers = MySessionComponentTriggerSystem.Static.GetAllTriggers();
<<<<<<< HEAD
			m_triggersListBox.Items.Clear();
=======
			((Collection<MyGuiControlListbox.Item>)(object)m_triggersListBox.Items).Clear();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MyTriggerComponent item2 in allTriggers)
			{
				MyGuiControlListbox.Item item = CreateTriggerListItem(item2);
				if (item != null)
				{
					m_triggersListBox.Add(item);
				}
			}
		}

		private MyGuiControlListbox.Item CreateTriggerListItem(MyTriggerComponent trigger)
		{
			MyAreaTriggerComponent myAreaTriggerComponent = trigger as MyAreaTriggerComponent;
			if (myAreaTriggerComponent == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder("Trigger: ");
			stringBuilder.Append(myAreaTriggerComponent.Name).Append(" Entity: ");
			stringBuilder.Append(string.IsNullOrEmpty(myAreaTriggerComponent.Entity.Name) ? ((MyEntity)myAreaTriggerComponent.Entity).DisplayNameText : myAreaTriggerComponent.Entity.Name);
			return new MyGuiControlListbox.Item(stringBuilder, "Double click to rename trigger", null, myAreaTriggerComponent);
		}

		private void InitializeWaypointList()
		{
			m_waypoints.Clear();
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				if (entity is MyWaypoint)
				{
					m_waypoints.Add(entity as MyWaypoint);
				}
			}
		}

		public void UpdateWaypointList()
		{
			if (m_waypointsListBox == null)
			{
				return;
			}
			ObservableCollection<MyGuiControlListbox.Item> items = m_waypointsListBox.Items;
			for (int i = 0; i < ((Collection<MyGuiControlListbox.Item>)(object)items).Count; i++)
			{
<<<<<<< HEAD
				MyWaypoint item2 = (MyWaypoint)items[i].UserData;
=======
				MyWaypoint item2 = (MyWaypoint)((Collection<MyGuiControlListbox.Item>)(object)items)[i].UserData;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!m_waypoints.Contains(item2))
				{
					((IList<MyGuiControlListbox.Item>)items).RemoveAtFast(i);
				}
			}
			foreach (MyWaypoint waypoint in m_waypoints)
			{
				int num = m_waypointsListBox.Items.FindIndex((MyGuiControlListbox.Item item) => (MyWaypoint)item.UserData == waypoint);
				if (num < 0)
				{
					StringBuilder stringBuilder = new StringBuilder("Waypoint: ");
					stringBuilder.Append(waypoint.Name);
					m_waypointsListBox.Add(new MyGuiControlListbox.Item(stringBuilder, waypoint.Name, null, waypoint));
				}
				else
				{
<<<<<<< HEAD
					m_waypointsListBox.Items[num].Text.Clear().Append("Waypoint: " + waypoint.Name);
				}
			}
			List<MyGuiControlListbox.Item> list = m_waypointsListBox.Items.OrderBy((MyGuiControlListbox.Item x) => x.Text.ToString()).ToList();
			for (int j = 0; j < list.Count; j++)
			{
				items.Move(items.IndexOf(list[j]), j);
=======
					((Collection<MyGuiControlListbox.Item>)(object)m_waypointsListBox.Items)[num].Text.Clear().Append("Waypoint: " + waypoint.Name);
				}
			}
			List<MyGuiControlListbox.Item> list = Enumerable.ToList<MyGuiControlListbox.Item>((IEnumerable<MyGuiControlListbox.Item>)Enumerable.OrderBy<MyGuiControlListbox.Item, string>((IEnumerable<MyGuiControlListbox.Item>)m_waypointsListBox.Items, (Func<MyGuiControlListbox.Item, string>)((MyGuiControlListbox.Item x) => x.Text.ToString())));
			for (int j = 0; j < list.Count; j++)
			{
				((ObservableCollection<MyGuiControlListbox.Item>)items).Move(((Collection<MyGuiControlListbox.Item>)(object)items).IndexOf(list[j]), j);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			float y = (SCREEN_SIZE.Y - 1f) / 2f;
			Vector2 vector = new Vector2(0.02f, 0f);
			string text = MyTexts.Get(MySpaceTexts.ScriptingToolsTransformations).ToString();
			MyGuiControlLabel myGuiControlLabel = AddCaption(text, Color.White.ToVector4(), vector + new Vector2(0f - HIDDEN_PART_RIGHT, y));
			m_currentPosition.Y = myGuiControlLabel.PositionY + myGuiControlLabel.Size.Y + ITEM_VERTICAL_PADDING;
			m_transformSys.Active = true;
			m_canShareInput = true;
			MyGuiScreenGamePlay.DisableInput = false;
			SelectCoordsWorld(world: false);
			SelectOperation(MyEntityTransformationSystem.OperationMode.Translation);
			RecreateControlsTransformation();
		}

		private void SelectOperation(MyEntityTransformationSystem.OperationMode mode)
		{
			m_transformSys.ChangeOperationMode(mode);
		}

		private void SelectCoordsWorld(bool world)
		{
			m_transformSys.ChangeCoordSystem(world);
		}

		private void DeselectEntityOnClicked(MyGuiControlButton myGuiControlButton)
		{
			m_transformSys.SetControlledEntity(null);
		}

		private void RecreateControlsTransformation()
		{
			MyGuiControlCombobox transformCombo = CreateComboBox();
			transformCombo.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScriptingTools_Translation));
			transformCombo.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScriptingTools_Rotation));
			transformCombo.SelectItemByIndex(0);
			transformCombo.ItemSelected += delegate
			{
				SelectOperation((MyEntityTransformationSystem.OperationMode)transformCombo.GetSelectedKey());
			};
			MyGuiControlCombobox myGuiControlCombobox = CreateComboBox();
			myGuiControlCombobox.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScriptingTools_Coords_Local));
			myGuiControlCombobox.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScriptingTools_Coords_World));
			myGuiControlCombobox.SelectItemByIndex(0);
			myGuiControlCombobox.ItemSelected += delegate
			{
				SelectCoordsWorld(((int)transformCombo.GetSelectedKey() != 0) ? true : false);
			};
			PositionControls(new MyGuiControlBase[2] { myGuiControlCombobox, transformCombo });
			m_selectedEntityNameBox = CreateTextbox("");
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.GetString(MySpaceTexts.SelectedEntity) + ": "),
				m_selectedEntityNameBox,
				CreateButton(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_ButtonRename), RenameSelectedEntityOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_Rename1))
			});
			m_selectedFunctionalBlockNameBox = CreateTextbox("");
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.GetString(MySpaceTexts.SelectedBlock) + ": "),
				m_selectedFunctionalBlockNameBox,
				CreateButton(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_ButtonRename), RenameFunctionalBlockOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_Rename2))
			});
			PositionControls(new MyGuiControlBase[3]
			{
				CreateButton(MyTexts.GetString(MySpaceTexts.SpawnEntity), SpawnWaypointClicked, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SpawnEnt)),
				CreateButton(MyTexts.GetString(MyCommonTexts.Snap), SnapEntityToCameraClick, "Snap entity to camera"),
				CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_SetPosition), SetPositionOnClicked, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SetPosition))
			});
			PositionControls(new MyGuiControlBase[3]
			{
				CreateButton(MyTexts.GetString(MyCommonTexts.Select), SelectEntityOnClick, "Select entity you are standing in"),
				CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_DeselectEntity), DeselectEntityOnClicked, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_DeselectEnt)),
				CreateButton(MyTexts.GetString(MySpaceTexts.DeleteEntity), DeleteEntityOnClicked, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_DeleteEnt))
			});
			m_waypointsListBox = CreateListBox();
			m_waypointsListBox.Size = new Vector2(0f, 0.148f);
			m_waypointsListBox.ItemClicked += WaypointsListBoxOnItemDoubleClicked;
			PositionControl(m_waypointsListBox);
			PositionControl(CreateLabel(MyTexts.GetString(MySpaceTexts.Triggers)));
			PositionControls(new MyGuiControlBase[2]
			{
				CreateButton("Attach spherical trigger", AttachSphericalTriggerOnClick),
				CreateButton("Attach box trigger", AttachBoxTriggerOnClick)
			});
			PositionControls(new MyGuiControlBase[2]
			{
				CreateButton(MyTexts.GetString(MyCommonTexts.Snap), SnapTriggerToCameraOrEntityOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_TriggerSnap)),
				CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_SetPosition), SetPositionOnClicked, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SetPosition))
			});
			m_growTriggerButton = CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_Grow), GrowTriggerOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SizeGrow));
			m_shrinkTriggerButton = CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_Shrink), ShrinkTriggerOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SizeShrink));
			m_setTriggerSizeButton = CreateButton(MyTexts.GetString(MyCommonTexts.Size), SetSizeOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_SizeSet));
			MyGuiControlCombobox axisCombo = CreateComboBox();
			axisCombo.AddItem(-1L, "All");
			axisCombo.AddItem(0L, "X");
			axisCombo.AddItem(1L, "Y");
			axisCombo.AddItem(2L, "Z");
			axisCombo.SelectItemByIndex(0);
			axisCombo.ItemSelected += delegate
			{
				m_selectedAxis = (int)axisCombo.GetSelectedKey();
			};
			PositionControls(new MyGuiControlBase[4] { axisCombo, m_growTriggerButton, m_setTriggerSizeButton, m_shrinkTriggerButton });
			PositionControls(new MyGuiControlBase[3]
			{
				CreateButton(MyTexts.GetString(MyCommonTexts.Select), SelectTriggerOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_TriggerSelect)),
				CreateButton(MyTexts.GetString(MyCommonTexts.ScriptingTools_DeselectEntity), DeselectEntityOnClicked, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_DeselectEnt)),
				CreateButton(MyTexts.GetString(MyCommonTexts.Delete), DeleteTriggerOnClick, MyTexts.GetString(MyCommonTexts.ScriptingTools_Tooltip_TriggerDelete))
			});
			m_selectedTriggerNameBox = CreateTextbox(MyTexts.GetString(MySpaceTexts.TriggerNotSelected));
			PositionControls(new MyGuiControlBase[2]
			{
				CreateLabel(MyTexts.GetString(MySpaceTexts.SelectedTrigger) + ":"),
				m_selectedTriggerNameBox
			});
			m_triggersListBox = CreateListBox();
			m_triggersListBox.Size = new Vector2(0f, 0.14f);
			m_triggersListBox.ItemClicked += TriggersListBoxOnItemClicked;
			m_triggersListBox.ItemDoubleClicked += TriggersListBox_ItemDoubleClicked;
			PositionControl(m_triggersListBox);
			PositionControl(CreateLabel(MyTexts.Get(MySpaceTexts.RunningLevelScripts).ToString()));
			m_levelScriptListBox = CreateListBox();
			m_levelScriptListBox.Size = new Vector2(0f, 0.07f);
			PositionControl(m_levelScriptListBox);
			MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
			if (component != null && component.RunningLevelScriptNames != null)
			{
				string[] runningLevelScriptNames = component.RunningLevelScriptNames;
				foreach (string value in runningLevelScriptNames)
				{
					m_levelScriptListBox.Add(new MyGuiControlListbox.Item(new StringBuilder(value), null, null, false));
				}
			}
			PositionControl(CreateLabel(MyTexts.Get(MySpaceTexts.RunningStateMachines).ToString()));
			m_smListBox = CreateListBox();
			m_smListBox.Size = new Vector2(0f, 0.07f);
			PositionControl(m_smListBox);
			m_smListBox.ItemSize = new Vector2(SCREEN_SIZE.X, ITEM_SIZE.Y);
		}

		private void SwitchPageToTransformation(MyGuiControlButton myGuiControlButton)
		{
		}

		private void TransformSysOnRayCasted(LineD ray)
		{
			if (m_transformSys.ControlledEntity == null || m_disablePicking)
			{
				return;
			}
			MyHighlightData data;
			if (m_selectedFunctionalBlock != null)
			{
				MyHighlightSystem component = MySession.Static.GetComponent<MyHighlightSystem>();
				if (component != null)
				{
					data = new MyHighlightData
					{
						EntityId = m_selectedFunctionalBlock.EntityId,
						PlayerId = -1L,
						Thickness = -1
					};
					component.RequestHighlightChange(data);
				}
				m_selectedFunctionalBlock = null;
			}
			MyCubeGrid myCubeGrid = m_transformSys.ControlledEntity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				Vector3I? vector3I = myCubeGrid.RayCastBlocks(ray.From, ray.To);
				if (vector3I.HasValue)
				{
					MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(vector3I.Value);
					if (cubeBlock.FatBlock != null)
					{
						m_selectedFunctionalBlock = cubeBlock.FatBlock;
					}
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (m_selectedFunctionalBlock != null)
			{
				stringBuilder.Append(string.IsNullOrEmpty(m_selectedFunctionalBlock.Name) ? m_selectedFunctionalBlock.DisplayNameText : m_selectedFunctionalBlock.Name);
				MyHighlightSystem component2 = MySession.Static.GetComponent<MyHighlightSystem>();
				if (component2 != null)
				{
					data = new MyHighlightData
					{
						EntityId = m_selectedFunctionalBlock.EntityId,
						IgnoreUseObjectData = true,
						OutlineColor = Color.Blue,
						PulseTimeInFrames = 120uL,
						Thickness = 3,
						PlayerId = -1L
					};
					component2.RequestHighlightChange(data);
				}
			}
			if (m_selectedFunctionalBlockNameBox != null)
			{
				m_selectedFunctionalBlockNameBox.SetText(stringBuilder);
			}
		}

		private void RenameFunctionalBlockOnClick(MyGuiControlButton myGuiControlButton)
		{
			if (m_selectedFunctionalBlock == null)
			{
				return;
			}
			m_disablePicking = true;
			m_transformSys.DisablePicking = true;
			ValueGetScreenWithCaption valueGetScreenWithCaption = new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.EntityRename).ToString() + ": " + m_selectedFunctionalBlock.DisplayNameText, "", delegate(string text)
			{
				if (MyEntities.TryGetEntityByName(text, out var _))
				{
					return false;
				}
				m_selectedFunctionalBlock.Name = text;
				MyEntities.SetEntityName(m_selectedFunctionalBlock);
				m_selectedFunctionalBlockNameBox.SetText(new StringBuilder(text));
				return true;
			});
			valueGetScreenWithCaption.Closed += delegate
			{
				m_disablePicking = false;
				m_transformSys.DisablePicking = false;
			};
			MyGuiSandbox.AddScreen(valueGetScreenWithCaption);
		}

		private void RenameSelectedEntityOnClick(MyGuiControlButton myGuiControlButton)
		{
			if (m_transformSys.ControlledEntity == null)
			{
				return;
			}
			m_disablePicking = true;
			m_transformSys.DisablePicking = true;
			MyEntity selectedEntity = m_transformSys.ControlledEntity;
			ValueGetScreenWithCaption valueGetScreenWithCaption = new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.EntityRename).ToString() + ": " + m_transformSys.ControlledEntity.DisplayNameText, string.IsNullOrEmpty(m_transformSys.ControlledEntity.Name) ? "" : m_transformSys.ControlledEntity.Name, delegate(string text)
<<<<<<< HEAD
			{
				if (MyEntities.TryGetEntityByName(text, out var _))
				{
					return false;
				}
				selectedEntity.Name = text;
				MyEntities.SetEntityName(selectedEntity);
				m_selectedEntityNameBox.SetText(new StringBuilder(text));
				InitializeWaypointList();
				UpdateWaypointList();
				UpdateTriggerList();
				VSWaypointRenamedMsg vSWaypointRenamedMsg = default(VSWaypointRenamedMsg);
				vSWaypointRenamedMsg.Id = selectedEntity.EntityId;
				vSWaypointRenamedMsg.Name = text;
				VSWaypointRenamedMsg msg = vSWaypointRenamedMsg;
				MySessionComponentExtDebug.Static.SendMessageToClients(msg);
				return true;
			});
			valueGetScreenWithCaption.Closed += delegate
			{
				m_disablePicking = false;
				m_transformSys.DisablePicking = false;
			};
			MyGuiSandbox.AddScreen(valueGetScreenWithCaption);
		}

		private void DeleteEntityOnClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_transformSys.ControlledEntity == null)
			{
				return;
			}
			MyWaypoint wp = m_transformSys.ControlledEntity as MyWaypoint;
			if (wp != null && m_waypoints.Contains(wp))
			{
				m_waypoints.Remove(wp);
				UpdateWaypointList();
				VSWaypointDeletedMsg vSWaypointDeletedMsg = default(VSWaypointDeletedMsg);
				vSWaypointDeletedMsg.Id = wp.EntityId;
				VSWaypointDeletedMsg msg = vSWaypointDeletedMsg;
				MySessionComponentExtDebug.Static.SendMessageToClients(msg);
				MySessionComponentCutscenes component = MySession.Static.GetComponent<MySessionComponentCutscenes>();
				if (component != null)
				{
					foreach (KeyValuePair<string, Cutscene> cutscene in component.GetCutscenes())
					{
						foreach (CutsceneSequenceNode sequenceNode in cutscene.Value.SequenceNodes)
						{
							CutsceneSequenceNodeWaypoint cutsceneSequenceNodeWaypoint = sequenceNode.Waypoints.FirstOrDefault((CutsceneSequenceNodeWaypoint x) => x.Name == wp.Name);
							if (cutsceneSequenceNodeWaypoint != null)
							{
								sequenceNode.Waypoints.Remove(cutsceneSequenceNodeWaypoint);
							}
						}
					}
				}
			}
			m_transformSys.ControlledEntity.Close();
			m_transformSys.SetControlledEntity(null);
			MyEntities.DeleteRememberedEntities();
			UpdateTriggerList();
		}

		private void AttachSphericalTriggerOnClick(MyGuiControlButton myGuiControlButton)
		{
			AttachTriggerOnClick(MyTriggerComponent.TriggerType.Sphere);
		}

		private void AttachBoxTriggerOnClick(MyGuiControlButton myGuiControlButton)
		{
			AttachTriggerOnClick(MyTriggerComponent.TriggerType.OBB);
		}

		private void AttachTriggerOnClick(MyTriggerComponent.TriggerType triggerType)
		{
			if (m_transformSys.ControlledEntity == null)
			{
				return;
			}
=======
			{
				if (MyEntities.TryGetEntityByName(text, out var _))
				{
					return false;
				}
				selectedEntity.Name = text;
				MyEntities.SetEntityName(selectedEntity);
				m_selectedEntityNameBox.SetText(new StringBuilder(text));
				InitializeWaypointList();
				UpdateWaypointList();
				UpdateTriggerList();
				VSWaypointRenamedMsg vSWaypointRenamedMsg = default(VSWaypointRenamedMsg);
				vSWaypointRenamedMsg.Id = selectedEntity.EntityId;
				vSWaypointRenamedMsg.Name = text;
				VSWaypointRenamedMsg msg = vSWaypointRenamedMsg;
				MySessionComponentExtDebug.Static.SendMessageToClients(msg);
				return true;
			});
			valueGetScreenWithCaption.Closed += delegate
			{
				m_disablePicking = false;
				m_transformSys.DisablePicking = false;
			};
			MyGuiSandbox.AddScreen(valueGetScreenWithCaption);
		}

		private void DeleteEntityOnClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_transformSys.ControlledEntity == null)
			{
				return;
			}
			MyWaypoint wp = m_transformSys.ControlledEntity as MyWaypoint;
			if (wp != null && m_waypoints.Contains(wp))
			{
				m_waypoints.Remove(wp);
				UpdateWaypointList();
				VSWaypointDeletedMsg vSWaypointDeletedMsg = default(VSWaypointDeletedMsg);
				vSWaypointDeletedMsg.Id = wp.EntityId;
				VSWaypointDeletedMsg msg = vSWaypointDeletedMsg;
				MySessionComponentExtDebug.Static.SendMessageToClients(msg);
				MySessionComponentCutscenes component = MySession.Static.GetComponent<MySessionComponentCutscenes>();
				if (component != null)
				{
					foreach (KeyValuePair<string, Cutscene> cutscene in component.GetCutscenes())
					{
						foreach (CutsceneSequenceNode sequenceNode in cutscene.Value.SequenceNodes)
						{
							CutsceneSequenceNodeWaypoint cutsceneSequenceNodeWaypoint = Enumerable.FirstOrDefault<CutsceneSequenceNodeWaypoint>((IEnumerable<CutsceneSequenceNodeWaypoint>)sequenceNode.Waypoints, (Func<CutsceneSequenceNodeWaypoint, bool>)((CutsceneSequenceNodeWaypoint x) => x.Name == wp.Name));
							if (cutsceneSequenceNodeWaypoint != null)
							{
								sequenceNode.Waypoints.Remove(cutsceneSequenceNodeWaypoint);
							}
						}
					}
				}
			}
			m_transformSys.ControlledEntity.Close();
			m_transformSys.SetControlledEntity(null);
			MyEntities.DeleteRememberedEntities();
			UpdateTriggerList();
		}

		private void AttachSphericalTriggerOnClick(MyGuiControlButton myGuiControlButton)
		{
			AttachTriggerOnClick(MyTriggerComponent.TriggerType.Sphere);
		}

		private void AttachBoxTriggerOnClick(MyGuiControlButton myGuiControlButton)
		{
			AttachTriggerOnClick(MyTriggerComponent.TriggerType.OBB);
		}

		private void AttachTriggerOnClick(MyTriggerComponent.TriggerType triggerType)
		{
			if (m_transformSys.ControlledEntity == null)
			{
				return;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyEntity selectedEntity = ((m_selectedFunctionalBlock != null) ? m_selectedFunctionalBlock : m_transformSys.ControlledEntity);
			MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption("Set name of trigger to be attached to " + (string.IsNullOrEmpty(selectedEntity.Name) ? selectedEntity.DisplayNameText : selectedEntity.Name), "", delegate(string text)
			{
				MyAreaTriggerComponent myAreaTriggerComponent = new MyAreaTriggerComponent(text)
				{
					TriggerAreaType = triggerType
				};
				m_triggerManipulator.SelectedTrigger = myAreaTriggerComponent;
				if (!selectedEntity.Components.Contains(typeof(MyTriggerAggregate)))
				{
					selectedEntity.Components.Add(typeof(MyTriggerAggregate), new MyTriggerAggregate());
				}
				selectedEntity.Components.Get<MyTriggerAggregate>().AddComponent(m_triggerManipulator.SelectedTrigger);
				myAreaTriggerComponent.Radius = 2.0;
				Matrix m = Matrix.Normalize(selectedEntity.PositionComp.WorldMatrixRef);
				myAreaTriggerComponent.OBB = new MyOrientedBoundingBoxD(m);
				myAreaTriggerComponent.Center = MyAPIGateway.Session.Camera.Position;
				myAreaTriggerComponent.CustomDebugColor = Color.Yellow;
				DeselectEntity();
				UpdateTriggerList();
				m_triggersListBox.SelectedItems.Clear();
				m_triggersListBox.SelectByUserData(myAreaTriggerComponent);
				return true;
			}));
		}

		private void RenameTriggerOnClick(MyAreaTriggerComponent trigger)
		{
			MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.EntityRename).ToString() + ": " + trigger.Name, trigger.Name, delegate(string text)
			{
				m_triggerManipulator.SelectedTrigger = trigger;
				trigger.Name = text;
				UpdateTriggerList();
				m_selectedTriggerNameBox.SetText(new StringBuilder(text));
				return true;
			}));
		}

		private void DeselectEntity()
		{
			m_transformSys.SetControlledEntity(null);
			m_waypointsListBox.SelectedItems.Clear();
		}

		private void DeselectTrigger()
		{
			m_triggerManipulator.SelectedTrigger = null;
			if (m_selectedTriggerNameBox != null)
			{
				m_selectedTriggerNameBox.SetText(new StringBuilder());
			}
			if (m_triggersListBox != null)
			{
				m_triggersListBox.SelectedItems.Clear();
			}
		}

		private void DeleteTriggerOnClick(MyGuiControlButton myGuiControlButton)
		{
			if (m_triggerManipulator.SelectedTrigger != null)
			{
				if (m_triggerManipulator.SelectedTrigger.Entity != null)
				{
					m_triggerManipulator.SelectedTrigger.Entity.Components.Remove(typeof(MyTriggerAggregate), m_triggerManipulator.SelectedTrigger);
				}
				m_triggerManipulator.SelectedTrigger = null;
				m_selectedEntityNameBox.SetText(new StringBuilder());
				UpdateTriggerList();
			}
		}

		private void SnapTriggerToCameraOrEntityOnClick(MyGuiControlButton myGuiControlButton)
		{
			if (m_triggerManipulator.SelectedTrigger != null)
			{
				MyAreaTriggerComponent myAreaTriggerComponent = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
				if (m_transformSys.ControlledEntity != null)
				{
					myAreaTriggerComponent.Center = m_transformSys.ControlledEntity.PositionComp.GetPosition();
				}
				else
				{
					myAreaTriggerComponent.Center = MyAPIGateway.Session.Camera.Position;
				}
			}
		}

		private void SnapEntityToCameraClick(MyGuiControlButton myGuiControlButton)
		{
			if (m_transformSys.ControlledEntity != null)
			{
				MatrixD worldMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
				m_transformSys.ControlledEntity.PositionComp.SetWorldMatrix(ref worldMatrix);
			}
		}

		private void TransformSysOnControlledEntityChanged(MyEntity oldEntity, MyEntity newEntity)
		{
			if (m_disablePicking)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (newEntity != null)
			{
				stringBuilder.Clear().Append(string.IsNullOrEmpty(newEntity.Name) ? newEntity.DisplayName : newEntity.Name);
				DeselectTrigger();
				if (newEntity is MyWaypoint && !m_waypoints.Contains(newEntity as MyWaypoint))
				{
					m_waypointsListBox.SelectedItems.Clear();
				}
			}
			if (m_selectedEntityNameBox != null)
			{
				m_selectedEntityNameBox.SetText(stringBuilder);
			}
			TransformSysOnRayCasted(m_transformSys.LastRay);
		}

		private void TriggersListBoxOnItemClicked(MyGuiControlListbox listBox)
		{
			if (m_triggersListBox.SelectedItems.Count != 0)
			{
				MyAreaTriggerComponent selectedTrigger = (MyAreaTriggerComponent)m_triggersListBox.SelectedItems[0].UserData;
				m_triggerManipulator.SelectedTrigger = selectedTrigger;
				if (m_triggerManipulator.SelectedTrigger != null)
				{
					MyAreaTriggerComponent myAreaTriggerComponent = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
					m_selectedTriggerNameBox.SetText(new StringBuilder(myAreaTriggerComponent.Name));
				}
				DeselectEntity();
			}
		}

		private void TriggersListBox_ItemDoubleClicked(MyGuiControlListbox obj)
		{
			if (m_triggersListBox.SelectedItems.Count != 0)
			{
				MyAreaTriggerComponent selectedTrigger = (MyAreaTriggerComponent)m_triggersListBox.SelectedItems[0].UserData;
				m_triggerManipulator.SelectedTrigger = selectedTrigger;
				if (m_triggerManipulator.SelectedTrigger != null)
				{
					MyAreaTriggerComponent trigger = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
					RenameTriggerOnClick(trigger);
				}
				DeselectEntity();
			}
		}

		private void WaypointsListBoxOnItemDoubleClicked(MyGuiControlListbox listBox)
		{
			if (m_waypointsListBox.SelectedItems.Count != 0)
			{
				MyEntity controlledEntity = (MyEntity)m_waypointsListBox.SelectedItems[0].UserData;
				m_transformSys.SetControlledEntity(controlledEntity);
				DeselectTrigger();
			}
		}

		private void SetSizeOnClick(MyGuiControlButton button)
		{
			if (m_triggerManipulator.SelectedTrigger == null)
<<<<<<< HEAD
			{
				return;
			}
			MyAreaTriggerComponent areaTrigger = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
			MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.SetTriggerSizeDialog).ToString(), areaTrigger.Radius.ToString(CultureInfo.InvariantCulture), delegate(string text)
			{
				if (!float.TryParse(text, out var result))
				{
					return false;
				}
				if (m_selectedAxis == -1 || areaTrigger.TriggerAreaType == MyTriggerComponent.TriggerType.Sphere)
				{
=======
			{
				return;
			}
			MyAreaTriggerComponent areaTrigger = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
			MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption(MyTexts.Get(MySpaceTexts.SetTriggerSizeDialog).ToString(), areaTrigger.Radius.ToString(CultureInfo.InvariantCulture), delegate(string text)
			{
				if (!float.TryParse(text, out var result))
				{
					return false;
				}
				if (m_selectedAxis == -1 || areaTrigger.TriggerAreaType == MyTriggerComponent.TriggerType.Sphere)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					areaTrigger.Radius = result;
				}
				else
				{
					switch (m_selectedAxis)
					{
					case 0:
						areaTrigger.SizeX = result;
						break;
					case 1:
						areaTrigger.SizeY = result;
						break;
					case 2:
						areaTrigger.SizeZ = result;
						break;
					}
				}
				return true;
			}));
		}

		private void SetPositionOnClicked(MyGuiControlButton button)
		{
			if (m_transformSys.ControlledEntity == null)
			{
				return;
			}
			MyEntity entity = m_transformSys.ControlledEntity;
			Vector3D position = entity.PositionComp.GetPosition();
			MyGuiSandbox.AddScreen(new Vector3GetScreenWithCaption(MyTexts.GetString(MySpaceTexts.SetEntityPositionDialog), position.X.ToString(), position.Y.ToString(), position.Z.ToString(), delegate(string text1, string text2, string text3)
			{
				if (!double.TryParse(text1, out var result) || !double.TryParse(text2, out var result2) || !double.TryParse(text3, out var result3))
				{
					return false;
				}
				MatrixD worldMatrix = entity.WorldMatrix;
				worldMatrix.Translation = new Vector3D(result, result2, result3);
				entity.WorldMatrix = worldMatrix;
				return true;
			}));
		}

		private void ShrinkTriggerOnClick(MyGuiControlButton button)
		{
			if (m_triggerManipulator.SelectedTrigger == null)
			{
				return;
			}
			MyAreaTriggerComponent myAreaTriggerComponent = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
			if (!(myAreaTriggerComponent.Radius > 0.20000000298023224))
			{
				return;
			}
			if (m_selectedAxis == -1 || myAreaTriggerComponent.TriggerAreaType == MyTriggerComponent.TriggerType.Sphere)
			{
				myAreaTriggerComponent.Radius -= 0.20000000298023224;
				return;
			}
			switch (m_selectedAxis)
			{
			case 0:
				myAreaTriggerComponent.SizeX -= 0.20000000298023224;
				break;
			case 1:
				myAreaTriggerComponent.SizeY -= 0.20000000298023224;
				break;
			case 2:
				myAreaTriggerComponent.SizeZ -= 0.20000000298023224;
				break;
			}
		}

		private void GrowTriggerOnClick(MyGuiControlButton button)
		{
			if (m_triggerManipulator.SelectedTrigger == null)
<<<<<<< HEAD
			{
				return;
			}
			MyAreaTriggerComponent myAreaTriggerComponent = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
			if (m_selectedAxis == -1 || myAreaTriggerComponent.TriggerAreaType == MyTriggerComponent.TriggerType.Sphere)
			{
				myAreaTriggerComponent.Radius += 0.20000000298023224;
				return;
			}
			switch (m_selectedAxis)
			{
=======
			{
				return;
			}
			MyAreaTriggerComponent myAreaTriggerComponent = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
			if (m_selectedAxis == -1 || myAreaTriggerComponent.TriggerAreaType == MyTriggerComponent.TriggerType.Sphere)
			{
				myAreaTriggerComponent.Radius += 0.20000000298023224;
				return;
			}
			switch (m_selectedAxis)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			case 0:
				myAreaTriggerComponent.SizeX += 0.20000000298023224;
				break;
			case 1:
				myAreaTriggerComponent.SizeY += 0.20000000298023224;
				break;
			case 2:
				myAreaTriggerComponent.SizeZ += 0.20000000298023224;
				break;
			}
		}

		private void SelectTriggerOnClick(MyGuiControlButton button)
		{
			m_triggerManipulator.SelectClosest(MyAPIGateway.Session.Camera.Position);
			if (m_triggerManipulator.SelectedTrigger != null)
			{
				MyAreaTriggerComponent myAreaTriggerComponent = (MyAreaTriggerComponent)m_triggerManipulator.SelectedTrigger;
				m_selectedTriggerNameBox.SetText(new StringBuilder(myAreaTriggerComponent.Name));
			}
		}

		private void SelectEntityOnClick(MyGuiControlButton button)
		{
			BoundingSphereD sphere = new BoundingSphereD(MyAPIGateway.Session.Camera.Position, 1.0);
			List<MyEntity> list = new List<MyEntity>();
			MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, list);
			if (list.Count > 0)
			{
				m_transformSys.SetControlledEntity(list[0]);
				return;
			}
			foreach (MyWaypoint waypoint in m_waypoints)
			{
				if (sphere.Contains(waypoint.PositionComp.GetPosition()) == ContainmentType.Contains)
				{
					m_transformSys.SetControlledEntity(waypoint);
					break;
				}
			}
		}

		private void SpawnWaypointClicked(MyGuiControlButton myGuiControlButton)
		{
			MyVisualScriptManagerSessionComponent.CreateWaypoint();
		}

		internal void OnWaypointAdded(MyWaypoint waypoint)
		{
			m_transformSys.SetControlledEntity(waypoint);
			m_waypoints.Add(waypoint);
			UpdateWaypointList();
			m_waypointsListBox.SelectedItems.Clear();
			m_waypointsListBox.SelectByUserData(waypoint);
		}

		private void DisableTransformationOnCheckedChanged(MyGuiControlCheckbox checkbox)
		{
			m_transformSys.DisableTransformation = checkbox.IsChecked;
		}

		private MyGuiControlCheckbox CreateCheckbox(Action<MyGuiControlCheckbox> onCheckedChanged, bool isChecked, string tooltip = null)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(null, null, null, isChecked, MyGuiControlCheckboxStyleEnum.Debug, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			if (!string.IsNullOrEmpty(tooltip))
			{
				myGuiControlCheckbox.SetTooltip(tooltip);
			}
			myGuiControlCheckbox.Size = ITEM_SIZE;
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, onCheckedChanged);
			Controls.Add(myGuiControlCheckbox);
			return myGuiControlCheckbox;
		}

		private MyGuiControlTextbox CreateTextbox(string text, Action<MyGuiControlTextbox> textChanged = null)
		{
			MyGuiControlTextbox myGuiControlTextbox = new MyGuiControlTextbox(null, text, 512, null, 0.8f, MyGuiControlTextboxType.Normal, MyGuiControlTextboxStyleEnum.Debug);
			myGuiControlTextbox.Enabled = false;
			myGuiControlTextbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlTextbox.Size = ITEM_SIZE;
			myGuiControlTextbox.TextChanged += textChanged;
			Controls.Add(myGuiControlTextbox);
			return myGuiControlTextbox;
		}

		private MyGuiControlLabel CreateLabel(string text)
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, ITEM_SIZE, text, null, 0.8f, "Debug", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			Controls.Add(myGuiControlLabel);
			return myGuiControlLabel;
		}

		private MyGuiControlListbox CreateListBox()
		{
			MyGuiControlListbox myGuiControlListbox = new MyGuiControlListbox(null, MyGuiControlListboxStyleEnum.Blueprints)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Size = new Vector2(1f, 0.15f)
			};
			myGuiControlListbox.MultiSelect = false;
			myGuiControlListbox.Enabled = true;
			myGuiControlListbox.ItemSize = new Vector2(SCREEN_SIZE.X, ITEM_SIZE.Y);
			myGuiControlListbox.TextScale = 0.6f;
			myGuiControlListbox.VisibleRowsCount = 7;
			Controls.Add(myGuiControlListbox);
			return myGuiControlListbox;
		}

		private MyGuiControlCombobox CreateComboBox()
		{
			MyGuiControlCombobox myGuiControlCombobox = new MyGuiControlCombobox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Size = BUTTON_SIZE,
				Position = new Vector2(m_buttonXOffset, m_currentPosition.Y)
			};
			myGuiControlCombobox.Enabled = true;
			Controls.Add(myGuiControlCombobox);
			return myGuiControlCombobox;
		}

		private MyGuiControlButton CreateButton(string text, Action<MyGuiControlButton> onClick, string tooltip = null)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(new Vector2(m_buttonXOffset, m_currentPosition.Y), MyGuiControlButtonStyleEnum.Rectangular, null, Color.Yellow.ToVector4(), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, new StringBuilder(text), 0.8f * MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * m_scale, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, onClick);
			if (!string.IsNullOrEmpty(tooltip))
			{
				myGuiControlButton.SetTooltip(tooltip);
			}
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlButton.Size = BUTTON_SIZE;
			Controls.Add(myGuiControlButton);
			return myGuiControlButton;
		}

		private int GetListboxSelectedIndex(MyGuiControlListbox listbox)
		{
			if (listbox.SelectedItems.Count == 0)
			{
				return -1;
			}
			for (int i = 0; i < ((Collection<MyGuiControlListbox.Item>)(object)listbox.Items).Count; i++)
			{
				if (((Collection<MyGuiControlListbox.Item>)(object)listbox.Items)[i] == listbox.SelectedItems[0])
				{
					return i;
				}
			}
			return -1;
		}

		private void PositionControl(MyGuiControlBase control)
		{
			float x = SCREEN_SIZE.X - HIDDEN_PART_RIGHT - ITEM_HORIZONTAL_PADDING * 2f;
			Vector2 size = control.Size;
			control.Position = new Vector2(m_currentPosition.X - SCREEN_SIZE.X / 2f + ITEM_HORIZONTAL_PADDING, m_currentPosition.Y + ITEM_VERTICAL_PADDING);
			control.Size = new Vector2(x, size.Y);
			m_currentPosition.Y += control.Size.Y + ITEM_VERTICAL_PADDING;
		}

		private void PositionControls(MyGuiControlBase[] controls)
		{
			float num = (SCREEN_SIZE.X - HIDDEN_PART_RIGHT - ITEM_HORIZONTAL_PADDING * 2f) / (float)controls.Length - 0.001f * (float)controls.Length;
			float num2 = num + 0.001f * (float)controls.Length;
			float num3 = 0f;
			for (int i = 0; i < controls.Length; i++)
			{
				MyGuiControlBase myGuiControlBase = controls[i];
				if (!(myGuiControlBase is MyGuiControlCheckbox))
				{
					myGuiControlBase.Size = new Vector2(num, myGuiControlBase.Size.Y);
				}
				else
				{
					myGuiControlBase.Size = new Vector2(BUTTON_SIZE.Y);
				}
				myGuiControlBase.PositionX = m_currentPosition.X + num2 * (float)i - SCREEN_SIZE.X / 2f + ITEM_HORIZONTAL_PADDING;
				myGuiControlBase.PositionY = m_currentPosition.Y + ITEM_VERTICAL_PADDING;
				if (myGuiControlBase.Size.Y > num3)
				{
					num3 = myGuiControlBase.Size.Y;
				}
			}
			m_currentPosition.Y += num3 + ITEM_VERTICAL_PADDING;
		}
	}
}
