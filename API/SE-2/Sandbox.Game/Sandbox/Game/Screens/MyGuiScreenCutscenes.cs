using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Collections.ObjectModel;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Text;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenCutscenes : MyGuiScreenDebugBase
	{
		private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);

		private static readonly float HIDDEN_PART_RIGHT = 0.04f;

		private static readonly float ITEM_HORIZONTAL_PADDING = 0.01f;

		private static readonly float ITEM_VERTICAL_PADDING = 0.005f;

		private static readonly Vector2 BUTTON_SIZE = new Vector2(0.06f, 0.03f);

		private static readonly Vector2 ITEM_SIZE = new Vector2(0.06f, 0.02f);

		public static MyGuiScreenCutscenes Static;

		private Dictionary<string, Cutscene> m_cutscenes;

		private Cutscene m_cutsceneCurrent;

		private int m_selectedCutsceneNodeIndex = -1;

		private bool m_cutscenePlaying;

		private MyGuiControlCombobox m_cutsceneSelection;

		private MyGuiControlButton m_cutsceneDeleteButton;

		private MyGuiControlButton m_cutscenePlayButton;

		private MyGuiControlButton m_cutsceneRevertButton;

		private MyGuiControlButton m_cutsceneSaveButton;

		private MyGuiControlTextbox m_cutscenePropertyStartEntity;

		private MyGuiControlTextbox m_cutscenePropertyStartLookAt;

		private MyGuiControlCombobox m_cutscenePropertyNextCutscene;

		private MyGuiControlTextbox m_cutscenePropertyStartingFOV;

		private MyGuiControlCheckbox m_cutscenePropertyCanBeSkipped;

		private MyGuiControlCheckbox m_cutscenePropertyFireEventsDuringSkip;

		private MyGuiControlListbox m_cutsceneNodes;

		private MyGuiControlButton m_cutsceneNodeButtonAdd;

		private MyGuiControlButton m_cutsceneNodeButtonMoveUp;

		private MyGuiControlButton m_cutsceneNodeButtonMoveDown;

		private MyGuiControlButton m_cutsceneNodeButtonDelete;

		private MyGuiControlButton m_cutsceneNodeButtonDeleteAll;

		private MyGuiControlTextbox m_cutsceneNodePropertyTime;

		private MyGuiControlTextbox m_cutsceneNodePropertyMoveTo;

		private MyGuiControlTextbox m_cutsceneNodePropertyMoveToInstant;

		private MyGuiControlTextbox m_cutsceneNodePropertyRotateLike;

		private MyGuiControlTextbox m_cutsceneNodePropertyRotateLikeInstant;

		private MyGuiControlTextbox m_cutsceneNodePropertyRotateTowards;

		private MyGuiControlTextbox m_cutsceneNodePropertyRotateTowardsInstant;

		private MyGuiControlTextbox m_cutsceneNodePropertyRotateTowardsLock;

		private MyGuiControlTextbox m_cutsceneNodePropertyAttachAll;

		private MyGuiControlTextbox m_cutsceneNodePropertyAttachPosition;

		private MyGuiControlTextbox m_cutsceneNodePropertyAttachRotation;

		private MyGuiControlTextbox m_cutsceneNodePropertyEvent;

		private MyGuiControlTextbox m_cutsceneNodePropertyEventDelay;

		private MyGuiControlTextbox m_cutsceneNodePropertyFOVChange;

		private MyGuiControlTextbox m_cutsceneNodePropertyWaypoints;

		public MyGuiScreenCutscenes()
			: base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - SCREEN_SIZE.X * 0.5f + HIDDEN_PART_RIGHT, 0.5f), SCREEN_SIZE, MyGuiConstants.SCREEN_BACKGROUND_COLOR, isTopMostScreen: false)
		{
			base.CanBeHidden = true;
			base.CanHideOthers = false;
			m_canCloseInCloseAllScreenCalls = true;
			m_canShareInput = true;
			m_isTopScreen = false;
			m_isTopMostScreen = false;
			Static = this;
			MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFreeMouse);
			MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
			MyDebugDrawSettings.DEBUG_DRAW_CUTSCENES = true;
			MyDebugDrawSettings.DEBUG_DRAW_WAYPOINTS = true;
			RecreateControls(constructor: true);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (!MyToolbarComponent.IsToolbarControlShown)
			{
				MyToolbarComponent.IsToolbarControlShown = true;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyInput.Static.IsNewKeyPressed(MyKeys.F11))
			{
				CloseScreenWithSave();
				return;
			}
			base.HandleInput(receivedFocusInThisUpdate);
			if (MySpectatorCameraController.Static.SpectatorCameraMovement != MySpectatorCameraMovementEnum.FreeMouse)
			{
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.FreeMouse;
			}
			foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
			{
				if (!(screen is MyGuiScreenScriptingTools) && !(screen is MyGuiScreenCutscenes))
				{
					screen.HandleInput(receivedFocusInThisUpdate);
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
			MyDebugDrawSettings.DEBUG_DRAW_CUTSCENES = false;
			MyDebugDrawSettings.DEBUG_DRAW_WAYPOINTS = false;
			MyGuiScreenGamePlay.DisableInput = MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning;
			Static = null;
			return base.CloseScreen(isUnloading);
		}

		public override bool Update(bool hasFocus)
		{
			UpdateCutscenes();
			return base.Update(hasFocus);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			float y = (SCREEN_SIZE.Y - 1f) / 2f;
			Vector2 vector = new Vector2(0.02f, 0f);
			string text = MyTexts.Get(MySpaceTexts.ScriptingToolsCutscenes).ToString();
			MyGuiControlLabel myGuiControlLabel = AddCaption(text, Color.White.ToVector4(), vector + new Vector2(0f - HIDDEN_PART_RIGHT, y));
			m_currentPosition.Y = myGuiControlLabel.PositionY + myGuiControlLabel.Size.Y + ITEM_VERTICAL_PADDING;
			RecreateControlsCutscenes();
		}

		private void RecreateControlsCutscenes()
		{
			m_cutscenes = MySession.Static.GetComponent<MySessionComponentCutscenes>().GetCutscenes();
			m_currentPosition.Y += ITEM_SIZE.Y;
			PositionControls(new MyGuiControlBase[2]
			{
				CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_New).ToString(), CreateNewCutsceneClicked),
				CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_ClearAllCutscenes).ToString(), ClearAllCutscenesClicked)
			});
			m_cutsceneSelection = CreateComboBox();
			foreach (Cutscene value in m_cutscenes.Values)
			{
				m_cutsceneSelection.AddItem(value.Name.GetHashCode64(), value.Name);
			}
			m_cutsceneSelection.ItemSelected += m_cutsceneSelection_ItemSelected;
			PositionControls(new MyGuiControlBase[2]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Selected).ToString()),
				m_cutsceneSelection
			});
			m_cutsceneDeleteButton = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Delete).ToString(), DeleteCurrentCutsceneClicked);
			m_cutscenePlayButton = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Play).ToString(), WatchCutsceneClicked);
			m_cutscenePlayButton.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Play_Extended).ToString());
			m_cutsceneSaveButton = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Save).ToString(), SaveCutsceneClicked);
			m_cutsceneRevertButton = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Revert).ToString(), RevertCutsceneClicked);
			PositionControls(new MyGuiControlBase[4] { m_cutscenePlayButton, m_cutsceneSaveButton, m_cutsceneRevertButton, m_cutsceneDeleteButton });
			m_currentPosition.Y += ITEM_SIZE.Y / 2f;
			m_cutscenePropertyNextCutscene = CreateComboBox();
			m_cutscenePropertyNextCutscene.ItemSelected += CutscenePropertyNextCutscene_ItemSelected;
			PositionControls(new MyGuiControlBase[2]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_New).ToString()),
				m_cutscenePropertyNextCutscene
			});
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_PosRot).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookRot).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_FOV).ToString())
			});
			m_cutscenePropertyStartEntity = CreateTextbox("", CutscenePropertyStartEntity_TextChanged);
			m_cutscenePropertyStartEntity.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_PosRot_Extended).ToString());
			m_cutscenePropertyStartLookAt = CreateTextbox("", CutscenePropertyStartLookAt_TextChanged);
			m_cutscenePropertyStartLookAt.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookRot_Extended).ToString());
			m_cutscenePropertyStartingFOV = CreateTextbox("", CutscenePropertyStartingFOV_TextChanged);
			m_cutscenePropertyStartingFOV.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_FOV_Extended).ToString());
			PositionControls(new MyGuiControlBase[3] { m_cutscenePropertyStartEntity, m_cutscenePropertyStartLookAt, m_cutscenePropertyStartingFOV });
			m_cutscenePropertyCanBeSkipped = CreateCheckbox(CutscenePropertyCanBeSkippedChanged, isChecked: true);
			m_cutscenePropertyCanBeSkipped.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Skippable).ToString());
			m_cutscenePropertyFireEventsDuringSkip = CreateCheckbox(CutscenePropertyFireEventsDuringSkipChanged, isChecked: true);
			m_cutscenePropertyFireEventsDuringSkip.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_SkipWarning).ToString());
			PositionControls(new MyGuiControlBase[4]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_CanSkip).ToString()),
				m_cutscenePropertyCanBeSkipped,
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Events).ToString()),
				m_cutscenePropertyFireEventsDuringSkip
			});
			m_currentPosition.Y += ITEM_SIZE.Y;
			m_cutsceneNodeButtonAdd = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_AddNode).ToString(), CutsceneNodeButtonAddClicked);
			m_cutsceneNodeButtonDelete = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Delete).ToString(), CutsceneNodeButtonDeleteClicked);
			m_cutsceneNodeButtonDeleteAll = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_ClearAll).ToString(), CutsceneNodeButtonDeleteAllClicked);
			m_cutsceneNodeButtonMoveUp = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveUp).ToString(), CutsceneNodeButtonMoveUpClicked);
			m_cutsceneNodeButtonMoveDown = CreateButton(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveDown).ToString(), CutsceneNodeButtonMoveDownClicked);
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Nodes).ToString()),
				m_cutsceneNodeButtonAdd,
				m_cutsceneNodeButtonDeleteAll
			});
			PositionControls(new MyGuiControlBase[4]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_CurrentNode).ToString()),
				m_cutsceneNodeButtonMoveUp,
				m_cutsceneNodeButtonMoveDown,
				m_cutsceneNodeButtonDelete
			});
			m_cutsceneNodes = CreateListBox();
			m_cutsceneNodes.VisibleRowsCount = 5;
			m_cutsceneNodes.Size = new Vector2(0f, 0.12f);
			m_cutsceneNodes.ItemsSelected += m_cutsceneNodes_ItemsSelected;
			PositionControl(m_cutsceneNodes);
			m_cutsceneNodePropertyTime = CreateTextbox("", CutsceneNodePropertyTime_TextChanged);
			m_cutsceneNodePropertyTime.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Time_Extended).ToString());
			m_cutsceneNodePropertyEvent = CreateTextbox("", CutsceneNodePropertyEvent_TextChanged);
			m_cutsceneNodePropertyEvent.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Event_Extended).ToString());
			m_cutsceneNodePropertyEventDelay = CreateTextbox("", CutsceneNodePropertyEventDelay_TextChanged);
			m_cutsceneNodePropertyEventDelay.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_EventDelay_Extended).ToString());
			m_cutsceneNodePropertyFOVChange = CreateTextbox("", CutsceneNodePropertyFOV_TextChanged);
			m_cutsceneNodePropertyFOVChange.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_FOVChange_Extended).ToString());
			PositionControls(new MyGuiControlBase[4]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Time).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Event).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_EventDelay).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_FOVChange).ToString())
			});
			PositionControls(new MyGuiControlBase[4] { m_cutsceneNodePropertyTime, m_cutsceneNodePropertyEvent, m_cutsceneNodePropertyEventDelay, m_cutsceneNodePropertyFOVChange });
			m_currentPosition.Y += ITEM_SIZE.Y / 2f;
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Action).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_OverTime).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Instant).ToString())
			});
			m_cutsceneNodePropertyMoveTo = CreateTextbox("", CutsceneNodePropertyMoveTo_TextChanged);
			m_cutsceneNodePropertyMoveTo.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveTo_Extended1).ToString());
			m_cutsceneNodePropertyMoveToInstant = CreateTextbox("", CutsceneNodePropertyMoveToInstant_TextChanged);
			m_cutsceneNodePropertyMoveToInstant.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveTo_Extended2).ToString());
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_MoveTo).ToString()),
				m_cutsceneNodePropertyMoveTo,
				m_cutsceneNodePropertyMoveToInstant
			});
			m_cutsceneNodePropertyRotateLike = CreateTextbox("", CutsceneNodePropertyRotateLike_TextChanged);
			m_cutsceneNodePropertyRotateLike.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_RotateLike_Extended1).ToString());
			m_cutsceneNodePropertyRotateLikeInstant = CreateTextbox("", CutsceneNodePropertyRotateLikeInstant_TextChanged);
			m_cutsceneNodePropertyRotateLikeInstant.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_RotateLike_Extended2).ToString());
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_RotateLike).ToString()),
				m_cutsceneNodePropertyRotateLike,
				m_cutsceneNodePropertyRotateLikeInstant
			});
			m_cutsceneNodePropertyRotateTowards = CreateTextbox("", CutsceneNodePropertyLookAt_TextChanged);
			m_cutsceneNodePropertyRotateTowards.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookAt_Extended1).ToString());
			m_cutsceneNodePropertyRotateTowardsInstant = CreateTextbox("", CutsceneNodePropertyLookAtInstant_TextChanged);
			m_cutsceneNodePropertyRotateTowardsInstant.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookAt_Extended2).ToString());
			PositionControls(new MyGuiControlBase[3]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_LookAt).ToString()),
				m_cutsceneNodePropertyRotateTowards,
				m_cutsceneNodePropertyRotateTowardsInstant
			});
			m_currentPosition.Y += ITEM_SIZE.Y;
			m_cutsceneNodePropertyRotateTowardsLock = CreateTextbox("", CutsceneNodePropertyLockRotationTo_TextChanged);
			m_cutsceneNodePropertyRotateTowardsLock.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Track_Extended1).ToString());
			m_cutsceneNodePropertyAttachAll = CreateTextbox("", CutsceneNodePropertyAttachTo_TextChanged);
			m_cutsceneNodePropertyAttachAll.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Track_Extended2).ToString());
			m_cutsceneNodePropertyAttachPosition = CreateTextbox("", CutsceneNodePropertyAttachPositionTo_TextChanged);
			m_cutsceneNodePropertyAttachPosition.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Track_Extended3).ToString());
			m_cutsceneNodePropertyAttachRotation = CreateTextbox("", CutsceneNodePropertyAttachRotationTo_TextChanged);
			m_cutsceneNodePropertyAttachRotation.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Track_Extended4).ToString());
			PositionControls(new MyGuiControlBase[4]
			{
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_TrackLook).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_TrackPosRot).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_TrackPos).ToString()),
				CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_TrackRot).ToString())
			});
			PositionControls(new MyGuiControlBase[4] { m_cutsceneNodePropertyRotateTowardsLock, m_cutsceneNodePropertyAttachAll, m_cutsceneNodePropertyAttachPosition, m_cutsceneNodePropertyAttachRotation });
			m_currentPosition.Y += ITEM_SIZE.Y / 2f;
			m_cutsceneNodePropertyWaypoints = CreateTextbox("", CutsceneNodePropertyWaypoints_TextChanged);
			m_cutsceneNodePropertyWaypoints.SetToolTip(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Waypoints_Extended).ToString());
			PositionControl(CreateLabel(MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_Waypoints).ToString()));
			PositionControl(m_cutsceneNodePropertyWaypoints);
			m_cutsceneCurrent = null;
			m_selectedCutsceneNodeIndex = -1;
			m_cutsceneSaveButton.Enabled = false;
			if (m_cutscenes.Count > 0)
			{
				m_cutsceneSelection.SelectItemByIndex(0);
			}
			else
			{
				UpdateCutsceneFields();
			}
		}

		private void CutsceneTextbox_FocusChanged(MyGuiControlBase arg1, bool focused)
		{
			MyGuiScreenGamePlay.DisableInput = focused;
		}

		private void UpdateCutscenes()
		{
			if (m_cutscenePlaying && !MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning)
			{
				StopCutscene();
			}
		}

		private void StopCutscene()
		{
			base.State = MyGuiScreenState.OPENED;
			MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
			MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFreeMouse);
			m_cutscenePlaying = false;
		}

		private void UpdateCutsceneFields()
		{
			string name = ((m_cutsceneSelection.GetSelectedIndex() >= 0) ? m_cutsceneSelection.GetSelectedValue().ToString() : "");
			m_cutsceneCurrent = null;
			Cutscene cutsceneCopy = MySession.Static.GetComponent<MySessionComponentCutscenes>().GetCutsceneCopy(name);
			bool flag = cutsceneCopy != null;
			m_cutsceneDeleteButton.Enabled = flag;
			m_cutscenePlayButton.Enabled = flag;
			m_cutsceneSaveButton.Enabled = false;
			m_cutsceneRevertButton.Enabled = false;
			m_cutscenePropertyNextCutscene.Enabled = flag;
			m_cutscenePropertyNextCutscene.ClearItems();
			m_cutscenePropertyNextCutscene.AddItem(0L, MyTexts.Get(MyCommonTexts.Cutscene_Tooltip_None));
			m_cutscenePropertyNextCutscene.SelectItemByIndex(0);
			m_cutscenePropertyStartEntity.Enabled = flag;
			m_cutscenePropertyStartLookAt.Enabled = flag;
			m_cutscenePropertyStartingFOV.Enabled = flag;
			m_cutscenePropertyCanBeSkipped.Enabled = flag;
			m_cutscenePropertyFireEventsDuringSkip.Enabled = flag;
			m_cutsceneNodes.ClearItems();
			if (flag)
			{
				m_cutscenePropertyStartEntity.Text = cutsceneCopy.StartEntity;
				m_cutscenePropertyStartLookAt.Text = cutsceneCopy.StartLookAt;
				m_cutscenePropertyStartingFOV.Text = cutsceneCopy.StartingFOV.ToString();
				m_cutscenePropertyCanBeSkipped.IsChecked = cutsceneCopy.CanBeSkipped;
				m_cutscenePropertyFireEventsDuringSkip.IsChecked = cutsceneCopy.FireEventsDuringSkip;
				foreach (string key in m_cutscenes.Keys)
				{
					if (!key.Equals(cutsceneCopy.Name))
					{
						m_cutscenePropertyNextCutscene.AddItem(key.GetHashCode64(), key);
						if (key.Equals(cutsceneCopy.NextCutscene))
						{
							m_cutscenePropertyNextCutscene.SelectItemByKey(key.GetHashCode64());
						}
					}
				}
				if (cutsceneCopy.SequenceNodes != null)
				{
					for (int i = 0; i < cutsceneCopy.SequenceNodes.Count; i++)
					{
						m_cutsceneNodes.Add(new MyGuiControlListbox.Item(new StringBuilder(i + 1 + ": " + cutsceneCopy.SequenceNodes[i].GetNodeSummary()), cutsceneCopy.SequenceNodes[i].GetNodeDescription()));
					}
				}
			}
			m_cutsceneCurrent = cutsceneCopy;
			UpdateCutsceneNodeFields();
		}

		private void CutsceneChanged()
		{
			m_cutsceneSaveButton.Enabled = m_cutsceneCurrent != null;
			m_cutsceneRevertButton.Enabled = m_cutsceneCurrent != null;
			if (m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneNodes.ItemsSelected -= m_cutsceneNodes_ItemsSelected;
<<<<<<< HEAD
				m_cutsceneNodes.Items.RemoveAt(m_selectedCutsceneNodeIndex);
				m_cutsceneNodes.Items.Insert(m_selectedCutsceneNodeIndex, new MyGuiControlListbox.Item(new StringBuilder(m_selectedCutsceneNodeIndex + 1 + ": " + m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].GetNodeSummary()), m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].GetNodeDescription()));
=======
				((Collection<MyGuiControlListbox.Item>)(object)m_cutsceneNodes.Items).RemoveAt(m_selectedCutsceneNodeIndex);
				((Collection<MyGuiControlListbox.Item>)(object)m_cutsceneNodes.Items).Insert(m_selectedCutsceneNodeIndex, new MyGuiControlListbox.Item(new StringBuilder(m_selectedCutsceneNodeIndex + 1 + ": " + m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].GetNodeSummary()), m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].GetNodeDescription()));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				SelectListboxItemAtIndex(m_cutsceneNodes, m_selectedCutsceneNodeIndex);
				m_cutsceneNodes.ItemsSelected += m_cutsceneNodes_ItemsSelected;
			}
		}

		private void UpdateCutsceneNodeFields()
		{
			bool flag = m_cutsceneCurrent != null && m_cutsceneNodes.SelectedItems.Count > 0;
			m_cutsceneNodeButtonMoveUp.Enabled = flag;
			m_cutsceneNodeButtonMoveDown.Enabled = flag;
			m_cutsceneNodeButtonDelete.Enabled = flag;
			m_cutsceneNodePropertyTime.Enabled = flag;
			m_cutsceneNodePropertyMoveTo.Enabled = flag;
			m_cutsceneNodePropertyMoveToInstant.Enabled = flag;
			m_cutsceneNodePropertyRotateLike.Enabled = flag;
			m_cutsceneNodePropertyRotateLikeInstant.Enabled = flag;
			m_cutsceneNodePropertyRotateTowards.Enabled = flag;
			m_cutsceneNodePropertyRotateTowardsInstant.Enabled = flag;
			m_cutsceneNodePropertyEvent.Enabled = flag;
			m_cutsceneNodePropertyEventDelay.Enabled = flag;
			m_cutsceneNodePropertyFOVChange.Enabled = flag;
			m_cutsceneNodePropertyRotateTowardsLock.Enabled = flag;
			m_cutsceneNodePropertyAttachAll.Enabled = flag;
			m_cutsceneNodePropertyAttachPosition.Enabled = flag;
			m_cutsceneNodePropertyAttachRotation.Enabled = flag;
			m_cutsceneNodePropertyWaypoints.Enabled = flag;
			if (!flag)
			{
				return;
			}
			m_selectedCutsceneNodeIndex = GetListboxSelectedIndex(m_cutsceneNodes);
<<<<<<< HEAD
			m_cutsceneNodeButtonMoveUp.Enabled = m_selectedCutsceneNodeIndex > 0 && m_cutsceneNodes.Items.Count > 1;
			m_cutsceneNodeButtonMoveDown.Enabled = m_selectedCutsceneNodeIndex < m_cutsceneNodes.Items.Count - 1;
=======
			m_cutsceneNodeButtonMoveUp.Enabled = m_selectedCutsceneNodeIndex > 0 && ((Collection<MyGuiControlListbox.Item>)(object)m_cutsceneNodes.Items).Count > 1;
			m_cutsceneNodeButtonMoveDown.Enabled = m_selectedCutsceneNodeIndex < ((Collection<MyGuiControlListbox.Item>)(object)m_cutsceneNodes.Items).Count - 1;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_cutsceneNodePropertyTime.Text = Math.Max(m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Time, 0f).ToString();
			m_cutsceneNodePropertyMoveTo.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].MoveTo != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].MoveTo : "");
			m_cutsceneNodePropertyMoveToInstant.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].SetPositionTo != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].SetPositionTo : "");
			m_cutsceneNodePropertyRotateLike.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].RotateLike != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].RotateLike : "");
			m_cutsceneNodePropertyRotateLikeInstant.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].SetRotationLike != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].SetRotationLike : "");
			m_cutsceneNodePropertyRotateTowards.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].RotateTowards != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].RotateTowards : "");
			m_cutsceneNodePropertyRotateTowardsInstant.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LookAt != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LookAt : "");
			m_cutsceneNodePropertyEvent.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Event != null) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Event : "");
			m_cutsceneNodePropertyEventDelay.Text = Math.Max(m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].EventDelay, 0f).ToString();
			m_cutsceneNodePropertyFOVChange.Text = Math.Max(m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].ChangeFOVTo, 0f).ToString();
			m_cutsceneNodePropertyRotateTowardsLock.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LockRotationTo == null) ? "" : ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LockRotationTo.Length > 0) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LockRotationTo : "X"));
			m_cutsceneNodePropertyAttachAll.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachTo == null) ? "" : ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachTo.Length > 0) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachTo : "X"));
			m_cutsceneNodePropertyAttachPosition.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachPositionTo == null) ? "" : ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachPositionTo.Length > 0) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachPositionTo : "X"));
			m_cutsceneNodePropertyAttachRotation.Text = ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachRotationTo == null) ? "" : ((m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachRotationTo.Length > 0) ? m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachRotationTo : "X"));
			if (m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints.Count; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(";");
					}
					stringBuilder.Append(m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints[i].Name);
				}
				m_cutsceneNodePropertyWaypoints.Text = stringBuilder.ToString();
			}
			else
			{
				m_cutsceneNodePropertyWaypoints.Text = "";
			}
		}

		private void CloseScreenWithSave()
		{
			if (m_cutsceneSaveButton.Enabled)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO_CANCEL, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Caption), messageText: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						SaveCutsceneClicked(m_cutsceneSaveButton);
					}
					if (result == MyGuiScreenMessageBox.ResultEnum.YES || result == MyGuiScreenMessageBox.ResultEnum.NO)
					{
						CloseScreen();
					}
				}));
			}
			else
			{
				CloseScreen();
			}
		}

		private void m_cutsceneSelection_ItemSelected()
		{
			if (m_cutsceneSaveButton.Enabled)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO_CANCEL, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Text), messageText: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Caption), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						SaveCutsceneClicked(m_cutsceneSaveButton);
					}
					if (result == MyGuiScreenMessageBox.ResultEnum.YES || result == MyGuiScreenMessageBox.ResultEnum.NO)
					{
						UpdateCutsceneFields();
					}
				}));
			}
			else
			{
				UpdateCutsceneFields();
			}
		}

		private void WatchCutsceneClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneSelection.GetSelectedValue() != null && !m_cutscenePlaying)
			{
				MyDebugDrawSettings.ENABLE_DEBUG_DRAW = false;
				MySession.Static.GetComponent<MySessionComponentCutscenes>().PlayCutscene(m_cutsceneCurrent, registerEvents: false);
				base.State = MyGuiScreenState.HIDDEN;
				m_cutscenePlaying = true;
			}
			else
			{
				MySession.Static.GetComponent<MySessionComponentCutscenes>().CutsceneEnd(releaseCamera: true, copyToSpectator: true);
				StopCutscene();
			}
		}

		private void SaveCutsceneClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneCurrent != null)
			{
				m_cutscenes[m_cutsceneCurrent.Name] = m_cutsceneCurrent;
				m_cutsceneSaveButton.Enabled = false;
				m_cutsceneRevertButton.Enabled = false;
			}
		}

		private void RevertCutsceneClicked(MyGuiControlButton myGuiControlButton)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_Revert_Caption), messageText: MyTexts.Get(MyCommonTexts.Cutscene_Revert_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
				if (result == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					UpdateCutsceneFields();
				}
			}));
		}

		private void ClearAllCutscenesClicked(MyGuiControlButton myGuiControlButton)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_DeleteAll_Caption), messageText: MyTexts.Get(MyCommonTexts.Cutscene_DeleteAll_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
				if (result == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					m_cutscenes.Clear();
					m_cutsceneSelection.ClearItems();
					UpdateCutsceneFields();
				}
			}));
		}

		private void DeleteCurrentCutsceneClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneSelection.GetSelectedIndex() < 0)
			{
				return;
			}
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_Delete_Caption), messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.Cutscene_Delete_Text), m_cutsceneSelection.GetItemByIndex(m_cutsceneSelection.GetSelectedIndex()).Value), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
				if (result == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					m_cutscenes.Remove(m_cutsceneSelection.GetItemByIndex(m_cutsceneSelection.GetSelectedIndex()).Value.ToString());
					m_cutsceneSelection.RemoveItemByIndex(m_cutsceneSelection.GetSelectedIndex());
					if (m_cutscenes.Count > 0)
					{
						m_cutsceneSelection.SelectItemByIndex(0);
					}
					else
					{
						UpdateCutsceneFields();
					}
				}
			}));
		}

		private void CreateNewCutsceneClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneSaveButton.Enabled)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO_CANCEL, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Caption), messageText: MyTexts.Get(MyCommonTexts.Cutscene_Unsaved_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						SaveCutsceneClicked(m_cutsceneSaveButton);
					}
					if (result == MyGuiScreenMessageBox.ResultEnum.YES || result == MyGuiScreenMessageBox.ResultEnum.NO)
					{
						NewCutscene();
					}
				}));
			}
			else
			{
				NewCutscene();
			}
		}

		private void NewCutscene()
		{
			MyGuiSandbox.AddScreen(new ValueGetScreenWithCaption(MyTexts.Get(MyCommonTexts.Cutscene_New_Caption).ToString(), "", delegate(string text)
			{
				if (m_cutscenes.ContainsKey(text))
				{
					return false;
				}
				Cutscene value = new Cutscene
				{
					Name = text
				};
				m_cutscenes.Add(text, value);
				long hashCode = text.GetHashCode64();
				m_cutsceneSelection.AddItem(hashCode, text);
				m_cutsceneSelection.SelectItemByKey(hashCode);
				return true;
			}));
		}

		private void CutscenePropertyCanBeSkippedChanged(MyGuiControlCheckbox checkbox)
		{
			if (m_cutsceneCurrent != null)
			{
				m_cutsceneCurrent.CanBeSkipped = checkbox.IsChecked;
				CutsceneChanged();
			}
		}

		private void CutscenePropertyFireEventsDuringSkipChanged(MyGuiControlCheckbox checkbox)
		{
			if (m_cutsceneCurrent != null)
			{
				m_cutsceneCurrent.FireEventsDuringSkip = checkbox.IsChecked;
				CutsceneChanged();
			}
		}

		private void CutscenePropertyStartEntity_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null)
			{
				m_cutsceneCurrent.StartEntity = obj.Text;
				CutsceneChanged();
			}
		}

		private void CutscenePropertyStartLookAt_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null)
			{
				m_cutsceneCurrent.StartLookAt = obj.Text;
				CutsceneChanged();
			}
		}

		private void CutscenePropertyStartingFOV_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null)
			{
				if (float.TryParse(obj.Text, out var result))
				{
					m_cutsceneCurrent.StartingFOV = result;
				}
				else
				{
					m_cutsceneCurrent.StartingFOV = 70f;
				}
				CutsceneChanged();
			}
		}

		private void CutscenePropertyNextCutscene_ItemSelected()
		{
			if (m_cutsceneCurrent != null)
			{
				if (m_cutscenePropertyNextCutscene.GetSelectedKey() == 0L)
				{
					m_cutsceneCurrent.NextCutscene = null;
				}
				else
				{
					m_cutsceneCurrent.NextCutscene = m_cutscenePropertyNextCutscene.GetSelectedValue().ToString();
				}
				CutsceneChanged();
			}
		}

		private void m_cutsceneNodes_ItemsSelected(MyGuiControlListbox obj)
		{
			bool enabled = m_cutsceneSaveButton.Enabled;
			m_selectedCutsceneNodeIndex = GetListboxSelectedIndex(m_cutsceneNodes);
			UpdateCutsceneNodeFields();
			m_cutsceneSaveButton.Enabled = enabled;
			m_cutsceneRevertButton.Enabled = enabled;
		}

		private void CutsceneNodeButtonAddClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneCurrent != null)
			{
				if (m_cutsceneCurrent.SequenceNodes == null)
				{
					m_cutsceneCurrent.SequenceNodes = new List<CutsceneSequenceNode>();
				}
				CutsceneSequenceNode cutsceneSequenceNode = new CutsceneSequenceNode();
				m_cutsceneCurrent.SequenceNodes.Add(cutsceneSequenceNode);
				m_cutsceneNodes.Add(new MyGuiControlListbox.Item(new StringBuilder(m_cutsceneCurrent.SequenceNodes.Count + ": " + cutsceneSequenceNode.GetNodeSummary()), cutsceneSequenceNode.GetNodeDescription()));
				SelectListboxItemAtIndex(m_cutsceneNodes, m_cutsceneCurrent.SequenceNodes.Count - 1);
				CutsceneChanged();
			}
		}

		private void CutsceneNodeButtonDeleteAllClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneCurrent == null)
			{
				return;
			}
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_DeleteAllNodes_Caption), messageText: MyTexts.Get(MyCommonTexts.Cutscene_DeleteAllNodes_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
				if (result == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					m_cutsceneCurrent.SequenceNodes.Clear();
					m_cutsceneCurrent.SequenceNodes = null;
					m_cutsceneNodes.ClearItems();
					UpdateCutsceneNodeFields();
					m_cutsceneNodes.ScrollToolbarToTop();
					CutsceneChanged();
				}
			}));
		}

		private void CutsceneNodeButtonDeleteClicked(MyGuiControlButton myGuiControlButton)
		{
			if (m_cutsceneCurrent == null)
			{
				return;
			}
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.Cutscene_DeleteNode_Caption), messageText: MyTexts.Get(MyCommonTexts.Cutscene_DeleteNode_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
				if (result == MyGuiScreenMessageBox.ResultEnum.YES && m_selectedCutsceneNodeIndex >= 0)
				{
					m_cutsceneCurrent.SequenceNodes.RemoveAt(m_selectedCutsceneNodeIndex);
<<<<<<< HEAD
					m_cutsceneNodes.Items.RemoveAt(m_selectedCutsceneNodeIndex);
=======
					((Collection<MyGuiControlListbox.Item>)(object)m_cutsceneNodes.Items).RemoveAt(m_selectedCutsceneNodeIndex);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					SelectListboxItemAtIndex(m_cutsceneNodes, m_selectedCutsceneNodeIndex);
					CutsceneChanged();
				}
			}));
		}

		private void CutsceneNodeButtonMoveUpClicked(MyGuiControlButton myGuiControlButton)
		{
			int listboxSelectedIndex = GetListboxSelectedIndex(m_cutsceneNodes);
			if (m_cutsceneCurrent != null && listboxSelectedIndex >= 0)
			{
				CutsceneSequenceNode item = m_cutsceneCurrent.SequenceNodes[listboxSelectedIndex];
				m_cutsceneCurrent.SequenceNodes.RemoveAt(listboxSelectedIndex);
				m_cutsceneCurrent.SequenceNodes.Insert(listboxSelectedIndex - 1, item);
<<<<<<< HEAD
				MyGuiControlListbox.Item item2 = m_cutsceneNodes.Items[listboxSelectedIndex];
				m_cutsceneNodes.Items.RemoveAt(listboxSelectedIndex);
				m_cutsceneNodes.Items.Insert(listboxSelectedIndex - 1, item2);
=======
				MyGuiControlListbox.Item item2 = ((Collection<MyGuiControlListbox.Item>)(object)m_cutsceneNodes.Items)[listboxSelectedIndex];
				((Collection<MyGuiControlListbox.Item>)(object)m_cutsceneNodes.Items).RemoveAt(listboxSelectedIndex);
				((Collection<MyGuiControlListbox.Item>)(object)m_cutsceneNodes.Items).Insert(listboxSelectedIndex - 1, item2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				SelectListboxItemAtIndex(m_cutsceneNodes, listboxSelectedIndex - 1);
				CutsceneChanged();
			}
		}

		private void CutsceneNodeButtonMoveDownClicked(MyGuiControlButton myGuiControlButton)
		{
			int listboxSelectedIndex = GetListboxSelectedIndex(m_cutsceneNodes);
			if (m_cutsceneCurrent != null && listboxSelectedIndex >= 0)
			{
				CutsceneSequenceNode item = m_cutsceneCurrent.SequenceNodes[listboxSelectedIndex];
				m_cutsceneCurrent.SequenceNodes.RemoveAt(listboxSelectedIndex);
				m_cutsceneCurrent.SequenceNodes.Insert(listboxSelectedIndex + 1, item);
<<<<<<< HEAD
				MyGuiControlListbox.Item item2 = m_cutsceneNodes.Items[listboxSelectedIndex];
				m_cutsceneNodes.Items.RemoveAt(listboxSelectedIndex);
				m_cutsceneNodes.Items.Insert(listboxSelectedIndex + 1, item2);
=======
				MyGuiControlListbox.Item item2 = ((Collection<MyGuiControlListbox.Item>)(object)m_cutsceneNodes.Items)[listboxSelectedIndex];
				((Collection<MyGuiControlListbox.Item>)(object)m_cutsceneNodes.Items).RemoveAt(listboxSelectedIndex);
				((Collection<MyGuiControlListbox.Item>)(object)m_cutsceneNodes.Items).Insert(listboxSelectedIndex + 1, item2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				SelectListboxItemAtIndex(m_cutsceneNodes, listboxSelectedIndex + 1);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyMoveTo_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].MoveTo = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyMoveToInstant_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].SetPositionTo = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyRotateLike_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].RotateLike = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyRotateLikeInstant_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].SetRotationLike = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyLookAt_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].RotateTowards = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyLookAtInstant_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LookAt = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyEvent_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Event = ((obj.Text.Length > 0) ? obj.Text : null);
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyTime_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				if (float.TryParse(obj.Text, out var result))
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Time = Math.Max(0f, result);
				}
				else
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Time = 0f;
				}
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyFOV_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				if (float.TryParse(obj.Text, out var result))
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].ChangeFOVTo = Math.Max(0f, result);
				}
				else
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].ChangeFOVTo = 0f;
				}
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyEventDelay_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				if (float.TryParse(obj.Text, out var result))
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].EventDelay = Math.Max(0f, result);
				}
				else
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].EventDelay = 0f;
				}
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyAttachTo_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachTo = ((obj.Text.Length <= 0) ? null : ((obj.Text.Length > 1 || !obj.Text.ToUpper().Equals("X")) ? obj.Text : ""));
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyAttachPositionTo_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachPositionTo = ((obj.Text.Length <= 0) ? null : ((obj.Text.Length > 1 || !obj.Text.ToUpper().Equals("X")) ? obj.Text : ""));
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyAttachRotationTo_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].AttachRotationTo = ((obj.Text.Length <= 0) ? null : ((obj.Text.Length > 1 || !obj.Text.ToUpper().Equals("X")) ? obj.Text : ""));
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyLockRotationTo_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent != null && m_selectedCutsceneNodeIndex >= 0)
			{
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].LockRotationTo = ((obj.Text.Length <= 0) ? null : ((obj.Text.Length > 1 || !obj.Text.ToUpper().Equals("X")) ? obj.Text : ""));
				CutsceneChanged();
			}
		}

		private void CutsceneNodePropertyWaypoints_TextChanged(MyGuiControlTextbox obj)
		{
			if (m_cutsceneCurrent == null || m_selectedCutsceneNodeIndex < 0)
			{
				return;
			}
			bool flag = obj.Text.Length == 0;
			if (!flag)
			{
				string[] array = obj.Text.Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length != 0)
				{
					if (m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints == null)
					{
						m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints = new List<CutsceneSequenceNodeWaypoint>();
					}
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints.Clear();
					string[] array2 = array;
					foreach (string name in array2)
					{
						CutsceneSequenceNodeWaypoint cutsceneSequenceNodeWaypoint = new CutsceneSequenceNodeWaypoint();
						cutsceneSequenceNodeWaypoint.Name = name;
						m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints.Add(cutsceneSequenceNodeWaypoint);
					}
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints != null)
				{
					m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints.Clear();
				}
				m_cutsceneCurrent.SequenceNodes[m_selectedCutsceneNodeIndex].Waypoints = null;
			}
			CutsceneChanged();
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
			myGuiControlTextbox.FocusChanged += CutsceneTextbox_FocusChanged;
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
<<<<<<< HEAD
			for (int i = 0; i < listbox.Items.Count; i++)
			{
				if (listbox.Items[i] == listbox.SelectedItems[0])
=======
			for (int i = 0; i < ((Collection<MyGuiControlListbox.Item>)(object)listbox.Items).Count; i++)
			{
				if (((Collection<MyGuiControlListbox.Item>)(object)listbox.Items)[i] == listbox.SelectedItems[0])
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return i;
				}
			}
			return -1;
		}

		private void SelectListboxItemAtIndex(MyGuiControlListbox listbox, int index)
		{
			List<bool> list = new List<bool>();
			for (int i = 0; i < m_cutsceneCurrent.SequenceNodes.Count; i++)
			{
				list.Add(i == index);
			}
			m_cutsceneNodes.ChangeSelection(list);
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
