using System;
using System.Collections.Generic;
using Havok;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
<<<<<<< HEAD
using VRage;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Character.Components
{
	public abstract class MyCharacterDetectorComponent : MyCharacterComponent
	{
		private IMyEntity m_detectedEntity;

		protected IMyUseObject m_interactiveObject;

		protected static List<MyEntity> m_detectableEntities = new List<MyEntity>();

		protected MyHudNotification m_useObjectNotification;

		protected MyHudNotification m_showTerminalNotification;

		protected MyHudNotification m_openInventoryNotification;

		protected MyHudNotification m_buildPlannerNotification;

		protected bool m_usingContinuously;

		protected MyCharacterHitInfo CharHitInfo;

		public virtual IMyUseObject UseObject
		{
			get
			{
				return m_interactiveObject;
			}
			set
			{
				if (value != m_interactiveObject)
				{
					if (m_interactiveObject != null)
					{
						UseClose();
						m_interactiveObject.OnSelectionLost();
						InteractiveObjectRemoved();
					}
					m_interactiveObject = value;
					InteractiveObjectChanged();
				}
			}
		}

		public IMyEntity DetectedEntity
		{
			get
			{
				return m_detectedEntity;
			}
			protected set
			{
				if (m_detectedEntity != null)
				{
					m_detectedEntity.OnMarkForClose -= OnDetectedEntityMarkForClose;
				}
				m_detectedEntity = value;
				if (m_detectedEntity != null)
				{
					m_detectedEntity.OnMarkForClose += OnDetectedEntityMarkForClose;
				}
			}
		}

		public Vector3D HitPosition { get; protected set; }

		public Vector3 HitNormal { get; protected set; }

		public uint ShapeKey { get; protected set; }

		public Vector3D StartPosition { get; protected set; }

		public MyStringHash HitMaterial { get; protected set; }

		public HkRigidBody HitBody { get; protected set; }

		public object HitTag { get; protected set; }

		public static event Action<IMyUseObject> OnInteractiveObjectChanged;

		public static event Action<IMyUseObject> OnInteractiveObjectUsed;

		public override void UpdateAfterSimulation10()
		{
			if (MySession.Static.ControlledEntity != base.Character)
			{
				return;
			}
			if (m_useObjectNotification != null && !m_usingContinuously)
			{
				MyHud.Notifications.Add(m_useObjectNotification);
			}
			m_usingContinuously = false;
			if (!base.Character.IsSitting && !base.Character.IsDead)
			{
				MySandboxGame.Static.Invoke("MyCharacterDetectorComponent::DoDetection", this, delegate(object context)
				{
					MyCharacterDetectorComponent myCharacterDetectorComponent = (MyCharacterDetectorComponent)context;
					MyCharacter character = myCharacterDetectorComponent.Character;
					if (character != null)
					{
<<<<<<< HEAD
						myCharacterDetectorComponent.DoDetection(character.TargetFromCamera || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.SpectatorDelta || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.SpectatorFixed);
=======
						myCharacterDetectorComponent.DoDetection(character.TargetFromCamera);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				});
			}
			else if (MySession.Static.ControlledEntity == base.Character)
			{
				MyHud.SelectedObjectHighlight.RemoveHighlight();
			}
		}

		protected abstract void DoDetection(bool useHead);

		protected virtual void OnDetectedEntityMarkForClose(IMyEntity obj)
		{
			DetectedEntity = null;
			if (UseObject != null)
			{
				UseObject = null;
				MyHud.SelectedObjectHighlight.RemoveHighlight();
			}
		}

		protected void UseClose()
		{
			if (base.Character != null && UseObject != null && UseObject.IsActionSupported(UseActionEnum.Close))
			{
				UseObject.Use(UseActionEnum.Close, base.Character);
			}
		}

		protected void InteractiveObjectRemoved()
		{
			if (base.Character != null)
			{
				base.Character.RemoveNotification(ref m_useObjectNotification);
				base.Character.RemoveNotification(ref m_showTerminalNotification);
				base.Character.RemoveNotification(ref m_openInventoryNotification);
				base.Character.RemoveNotification(ref m_buildPlannerNotification);
			}
		}

		protected void InteractiveObjectChanged()
		{
			if (MySession.Static.ControlledEntity != base.Character)
			{
				return;
			}
			if (UseObject != null)
			{
				GetNotification(UseObject, UseActionEnum.Manipulate, ref m_useObjectNotification);
				GetNotification(UseObject, UseActionEnum.OpenTerminal, ref m_showTerminalNotification);
				GetNotification(UseObject, UseActionEnum.OpenInventory, ref m_openInventoryNotification);
				GetNotification(UseObject, UseActionEnum.BuildPlanner, ref m_buildPlannerNotification);
				MyStringId myStringId = ((m_useObjectNotification != null) ? m_useObjectNotification.Text : MySpaceTexts.Blank);
				MyStringId myStringId2 = ((m_showTerminalNotification != null) ? m_showTerminalNotification.Text : MySpaceTexts.Blank);
				MyStringId myStringId3 = ((m_openInventoryNotification != null) ? m_openInventoryNotification.Text : MySpaceTexts.Blank);
				if (myStringId != MySpaceTexts.Blank)
				{
					MyHud.Notifications.Add(m_useObjectNotification);
				}
				if (myStringId2 != MySpaceTexts.Blank && myStringId2 != myStringId)
				{
					MyHud.Notifications.Add(m_showTerminalNotification);
				}
				if (myStringId3 != MySpaceTexts.Blank && myStringId3 != myStringId2 && myStringId3 != myStringId)
				{
					MyHud.Notifications.Add(m_openInventoryNotification);
				}
				if (m_buildPlannerNotification != null)
				{
					MyHud.Notifications.Add(m_buildPlannerNotification);
				}
			}
			if (MyCharacterDetectorComponent.OnInteractiveObjectChanged != null)
			{
				MyCharacterDetectorComponent.OnInteractiveObjectChanged(UseObject);
			}
		}

		public void RaiseObjectUsed()
		{
			if (MyCharacterDetectorComponent.OnInteractiveObjectUsed != null)
			{
				MyCharacterDetectorComponent.OnInteractiveObjectUsed(UseObject);
			}
		}

		private void GetNotification(IMyUseObject useObject, UseActionEnum actionType, ref MyHudNotification notification)
		{
			if ((useObject.SupportedActions & actionType) == 0)
			{
				return;
			}
			MyActionDescription actionInfo = useObject.GetActionInfo(actionType);
			base.Character.RemoveNotification(ref notification);
			notification = new MyHudNotification(actionInfo.Text, 0, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, actionInfo.IsTextControlHint ? MyNotificationLevel.Control : MyNotificationLevel.Normal);
			if (!MyDebugDrawSettings.DEBUG_DRAW_JOYSTICK_CONTROL_HINTS && (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed))
			{
				notification.SetTextFormatArguments(actionInfo.FormatParams);
			}
			else if (actionInfo.ShowForGamepad)
			{
				if (actionInfo.JoystickText.HasValue)
				{
					notification.Text = actionInfo.JoystickText.Value;
				}
				if (actionInfo.JoystickFormatParams != null)
				{
					notification.SetTextFormatArguments(actionInfo.JoystickFormatParams);
				}
			}
			else
			{
				notification.Text = MyStringId.NullOrEmpty;
				notification = null;
			}
		}

		public void UseContinues()
		{
			MyHud.Notifications.Remove(m_useObjectNotification);
			m_usingContinuously = true;
		}

		public override void OnCharacterDead()
		{
			UseObject = null;
			base.OnCharacterDead();
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
<<<<<<< HEAD
			MySpectatorCameraController.Static.OnModeChanged += StaticOnOnModeChanged;
			MySession.Static.CameraAttachedToChanged += StaticOnCameraAttachedToChanged;
			base.Character.OnControlStateChanged += CharacterOnOnControlStateChanged;
			if (MySession.Static.LocalHumanPlayer != null)
			{
				MySession.Static.LocalHumanPlayer.Controller.ControlledEntityChanged += ControllerOnControlledEntityChanged;
			}
		}

		private void ControllerOnControlledEntityChanged(IMyControllableEntity arg1, IMyControllableEntity arg2)
		{
			CheckNeedsUpdates();
		}

		/// <inheritdoc />
		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			MySpectatorCameraController.Static.OnModeChanged -= StaticOnOnModeChanged;
			MySession.Static.CameraAttachedToChanged -= StaticOnCameraAttachedToChanged;
			base.Character.OnControlStateChanged -= CharacterOnOnControlStateChanged;
			if (MySession.Static.LocalHumanPlayer != null)
			{
				MySession.Static.LocalHumanPlayer.Controller.ControlledEntityChanged -= ControllerOnControlledEntityChanged;
			}
		}

		private void CheckNeedsUpdates()
		{
			MyCameraControllerEnum cameraControllerEnum = MySession.Static.GetCameraControllerEnum();
			base.NeedsUpdateAfterSimulation10 = MySession.Static.ControlledEntity == base.Character && !base.Character.ControllerInfo.IsRemotelyControlled() && (cameraControllerEnum == MyCameraControllerEnum.SpectatorDelta || cameraControllerEnum == MyCameraControllerEnum.SpectatorFixed || cameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator || cameraControllerEnum == MyCameraControllerEnum.Entity);
			if (!base.NeedsUpdateAfterSimulation10)
			{
				UseObject = null;
			}
		}

		private void CharacterOnOnControlStateChanged(bool hasControl)
		{
			CheckNeedsUpdates();
		}

		private void StaticOnCameraAttachedToChanged(IMyCameraController arg1, IMyCameraController arg2)
		{
			CheckNeedsUpdates();
		}

		private void StaticOnOnModeChanged(MySpectatorCameraMovementEnum oldValue, MySpectatorCameraMovementEnum newValue)
		{
			CheckNeedsUpdates();
=======
			base.Character.OnControlStateChanged += CharacterOnOnControlStateChanged;
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			base.Character.OnControlStateChanged -= CharacterOnOnControlStateChanged;
		}

		private void CharacterOnOnControlStateChanged(bool hasControl)
		{
			if (!hasControl)
			{
				base.NeedsUpdateAfterSimulation10 = false;
			}
			else
			{
				base.NeedsUpdateAfterSimulation10 = !base.Character.ControllerInfo.IsRemotelyControlled();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void OnRemovedFromScene()
		{
			UseObject = null;
			base.OnRemovedFromScene();
		}

		protected void GatherDetectorsInArea(Vector3D from)
		{
			BoundingSphereD sphere = new BoundingSphereD(from, MyConstants.DEFAULT_INTERACTIVE_DISTANCE);
			MyGamePruningStructure.GetAllEntitiesInSphere(ref sphere, m_detectableEntities);
		}

		protected void EnableDetectorsInArea(Vector3D from)
		{
			GatherDetectorsInArea(from);
			for (int i = 0; i < m_detectableEntities.Count; i++)
			{
				MyEntity myEntity = m_detectableEntities[i];
				MyCompoundCubeBlock myCompoundCubeBlock = myEntity as MyCompoundCubeBlock;
				if (myCompoundCubeBlock != null)
				{
					foreach (MySlimBlock block in myCompoundCubeBlock.GetBlocks())
					{
						if (block.FatBlock != null)
						{
							m_detectableEntities.Add(block.FatBlock);
						}
					}
				}
				if (myEntity.Components.TryGet<MyUseObjectsComponentBase>(out var component) && component.DetectorPhysics != null)
				{
					component.PositionChanged(component.Container.Get<MyPositionComponentBase>());
					component.DetectorPhysics.Enabled = true;
				}
			}
		}

		protected void DisableDetectors()
		{
			foreach (MyEntity detectableEntity in m_detectableEntities)
			{
				if (detectableEntity.Components.TryGet<MyUseObjectsComponentBase>(out var component) && component.DetectorPhysics != null)
				{
					component.DetectorPhysics.Enabled = false;
				}
			}
			m_detectableEntities.Clear();
		}

		protected static void HandleInteractiveObject(IMyUseObject interactive)
		{
			if (MyFakes.ENABLE_USE_NEW_OBJECT_HIGHLIGHT)
			{
				MyHud.SelectedObjectHighlight.Color = MySector.EnvironmentDefinition.ContourHighlightColor;
				if (interactive.InstanceID != -1 || interactive is MyFloatingObject || interactive.Owner is MyInventoryBagEntity)
				{
					MyHud.SelectedObjectHighlight.HighlightAttribute = null;
					MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.OutlineHighlight;
				}
				else
				{
					MyCharacter myCharacter = interactive as MyCharacter;
					if (myCharacter != null && myCharacter.IsDead)
					{
						MyHud.SelectedObjectHighlight.HighlightAttribute = null;
						MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.OutlineHighlight;
					}
					else
					{
						bool flag = false;
						IMyModelDummy dummy = interactive.Dummy;
						if (dummy != null && dummy.CustomData != null)
						{
							flag = dummy.CustomData.TryGetValue("highlight", out var value);
							string text = value as string;
							if (flag && text != null)
							{
								MyHud.SelectedObjectHighlight.HighlightAttribute = text;
								if (interactive.Owner is MyTextPanel)
								{
									MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.EdgeHighlight;
								}
								else
								{
									MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.OutlineHighlight;
								}
							}
							bool num = dummy.CustomData.TryGetValue("highlighttype", out value);
							string text2 = value as string;
							if (num && text2 != null)
							{
								if (text2 == "edge")
								{
									MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.EdgeHighlight;
								}
								else
								{
									MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.OutlineHighlight;
								}
							}
						}
						if (!flag)
						{
							MyHud.SelectedObjectHighlight.HighlightAttribute = null;
							MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.DummyHighlight;
						}
					}
				}
			}
			else
			{
				MyHud.SelectedObjectHighlight.HighlightAttribute = null;
				MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.DummyHighlight;
			}
			MyCubeBlock myCubeBlock = interactive.Owner as MyCubeBlock;
			if (myCubeBlock != null)
			{
				if (myCubeBlock.HighlightMode == MyCubeBlockHighlightModes.AlwaysCanUse)
				{
					MyHud.SelectedObjectHighlight.Color = MySector.EnvironmentDefinition.ContourHighlightColor;
				}
				else if ((myCubeBlock.HighlightMode == MyCubeBlockHighlightModes.Default && myCubeBlock.GetPlayerRelationToOwner() == MyRelationsBetweenPlayerAndBlock.Enemies) || myCubeBlock.HighlightMode == MyCubeBlockHighlightModes.AlwaysHostile)
				{
					MyHud.SelectedObjectHighlight.Color = MySector.EnvironmentDefinition.ContourHighlightColorAccessDenied;
				}
			}
			MyHud.SelectedObjectHighlight.Highlight(interactive);
		}
	}
}
