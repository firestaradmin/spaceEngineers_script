<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
	public class MyGridCameraSystem : MyUpdateableGridSystem
	{
		public static float GAMEPAD_ZOOM_SPEED;

		private readonly List<MyCameraBlock> m_cameras;

		private readonly List<MyCameraBlock> m_relayedCameras;

		private MyCameraBlock m_currentCamera;

		private bool m_ignoreNextInput;

		private static MyHudCameraOverlay m_cameraOverlay;

		public int CameraCount => m_cameras.Count;

		public MyCameraBlock CurrentCamera => m_currentCamera;

		public static IMyCameraController PreviousNonCameraBlockController { get; set; }

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.BeforeSimulation;

		static MyGridCameraSystem()
		{
			GAMEPAD_ZOOM_SPEED = 0.02f;
		}

		public MyGridCameraSystem(MyCubeGrid grid)
			: base(grid)
		{
			m_cameras = new List<MyCameraBlock>();
			m_relayedCameras = new List<MyCameraBlock>();
		}

		public void Register(MyCameraBlock camera)
		{
			m_cameras.Add(camera);
		}

		public void Unregister(MyCameraBlock camera)
		{
			if (camera == m_currentCamera)
			{
				ResetCamera();
			}
			m_cameras.Remove(camera);
		}

		public void CheckCurrentCameraStillValid()
		{
			if (m_currentCamera != null && !m_currentCamera.IsWorking)
			{
				ResetCamera();
			}
		}

		public void SetAsCurrent(MyCameraBlock newCamera)
		{
			if (m_currentCamera != newCamera)
			{
				if (newCamera.BlockDefinition.OverlayTexture != null)
				{
					MyHudCameraOverlay.TextureName = newCamera.BlockDefinition.OverlayTexture;
					MyHudCameraOverlay.Enabled = true;
				}
				else
				{
					MyHudCameraOverlay.Enabled = false;
				}
				string shipName = "";
				if (MyAntennaSystem.Static != null)
				{
					shipName = MyAntennaSystem.Static.GetLogicalGroupRepresentative(base.Grid).DisplayName ?? "";
				}
				string displayNameText = newCamera.DisplayNameText;
				MyHud.CameraInfo.Enable(shipName, displayNameText);
				m_currentCamera = newCamera;
				m_ignoreNextInput = true;
				MySessionComponentVoxelHand.Static.Enabled = false;
				MySession.Static.GameFocusManager.Clear();
				Schedule();
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected override void Update()
		{
			if (m_currentCamera == null)
			{
				return;
			}
			if (MySession.Static.CameraController != m_currentCamera)
			{
				if (!(MySession.Static.CameraController is MyCameraBlock))
				{
					DisableCameraEffects();
				}
				ResetCurrentCamera();
				return;
			}
			if (m_ignoreNextInput)
			{
				m_ignoreNextInput = false;
				return;
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SWITCH_LEFT) && MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				SetPrev();
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SWITCH_RIGHT) && MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				SetNext();
			}
			if (MyInput.Static.DeltaMouseScrollWheelValue() != 0 && MyGuiScreenToolbarConfigBase.Static == null && !MyGuiScreenTerminal.IsOpen)
			{
				m_currentCamera.ChangeZoom(MyInput.Static.DeltaMouseScrollWheelValue());
			}
			MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MyStringId.NullOrEmpty;
			if (MyControllerHelper.IsControl(context, MyControlsSpace.CAMERA_ZOOM_IN, MyControlStateType.PRESSED))
			{
				m_currentCamera.ChangeZoomPrecise(0f - GAMEPAD_ZOOM_SPEED);
			}
			else if (MyControllerHelper.IsControl(context, MyControlsSpace.CAMERA_ZOOM_OUT, MyControlStateType.PRESSED))
			{
				m_currentCamera.ChangeZoomPrecise(GAMEPAD_ZOOM_SPEED);
			}
		}

		public void UpdateBeforeSimulation10()
		{
			if (m_currentCamera != null && !CameraIsInRangeAndPlayerHasAccessLocal(m_currentCamera))
			{
				ResetCamera();
			}
		}

		public static bool CameraIsInRangeAndPlayerHasAccessLocal(MyCameraBlock camera)
		{
			return CameraIsInRangeAndPlayerHasAccess(MySession.Static.ControlledEntity, camera);
		}

		public static bool CameraIsInRangeAndPlayerHasAccess(Sandbox.Game.Entities.IMyControllableEntity entity, MyCameraBlock camera)
		{
			if (entity != null)
			{
				MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(entity as MyEntity);
				if (controllingPlayer == null)
				{
					return false;
				}
				if (((IMyComponentOwner<MyIDModule>)camera).GetComponent(out MyIDModule component) && !camera.HasPlayerAccess(controllingPlayer.Identity.IdentityId) && component.Owner != 0L)
				{
					return false;
				}
				MyCharacter broadcastingEntity;
				if ((broadcastingEntity = entity as MyCharacter) != null)
				{
					return MyAntennaSystem.Static.CheckConnection(broadcastingEntity, camera.CubeGrid, controllingPlayer);
				}
				MyShipController myShipController;
				if ((myShipController = entity as MyShipController) != null)
				{
					return MyAntennaSystem.Static.CheckConnection(myShipController.CubeGrid, camera.CubeGrid, controllingPlayer);
				}
				MyCubeBlock myCubeBlock;
				if ((myCubeBlock = entity as MyCubeBlock) != null)
				{
					return MyAntennaSystem.Static.CheckConnection(myCubeBlock.CubeGrid, camera.CubeGrid, controllingPlayer);
				}
			}
			return false;
		}

		public void ResetCamera()
		{
			ResetCurrentCamera();
			DisableCameraEffects();
			bool flag = false;
			if (PreviousNonCameraBlockController != null)
			{
				MyShipController myShipController;
				if ((myShipController = PreviousNonCameraBlockController as MyShipController) != null)
				{
					myShipController.RefreshControlNotifications();
				}
				MyEntity myEntity = PreviousNonCameraBlockController as MyEntity;
				if (myEntity != null && !myEntity.Closed)
				{
					MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, myEntity);
					PreviousNonCameraBlockController = null;
					flag = true;
				}
			}
			if (!flag && MySession.Static.LocalCharacter != null)
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, MySession.Static.LocalCharacter);
			}
		}

		private void DisableCameraEffects()
		{
			MyHudCameraOverlay.Enabled = false;
			MyHud.CameraInfo.Disable();
			MySector.MainCamera.FieldOfView = MySandboxGame.Config.FieldOfView;
		}

		public void ResetCurrentCamera()
		{
			if (m_currentCamera != null)
			{
				m_currentCamera.OnExitView();
				m_currentCamera = null;
				DeSchedule();
			}
		}

		private void SetNext()
		{
			UpdateRelayedCameras();
			MyCameraBlock next = GetNext(m_currentCamera);
			if (next != null)
			{
				SetCamera(next);
			}
		}

		private void SetPrev()
		{
			UpdateRelayedCameras();
			MyCameraBlock prev = GetPrev(m_currentCamera);
			if (prev != null)
			{
				SetCamera(prev);
			}
		}

		private void SetCamera(MyCameraBlock newCamera)
		{
			if (newCamera != m_currentCamera)
			{
				if (m_cameras.Contains(newCamera))
				{
					SetAsCurrent(newCamera);
					newCamera.SetView();
					return;
				}
				MyHudCameraOverlay.Enabled = false;
				MyHud.CameraInfo.Disable();
				ResetCurrentCamera();
				newCamera.RequestSetView();
			}
		}

		private void UpdateRelayedCameras()
		{
<<<<<<< HEAD
			List<MyAntennaSystem.BroadcasterInfo> list = MyAntennaSystem.Static.GetConnectedGridsInfo(base.Grid).ToList();
=======
			List<MyAntennaSystem.BroadcasterInfo> list = Enumerable.ToList<MyAntennaSystem.BroadcasterInfo>((IEnumerable<MyAntennaSystem.BroadcasterInfo>)MyAntennaSystem.Static.GetConnectedGridsInfo(base.Grid));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			list.Sort((MyAntennaSystem.BroadcasterInfo b1, MyAntennaSystem.BroadcasterInfo b2) => b1.EntityId.CompareTo(b2.EntityId));
			m_relayedCameras.Clear();
			foreach (MyAntennaSystem.BroadcasterInfo item in list)
			{
				AddValidCamerasFromGridToRelayed(item.EntityId);
			}
			if (m_relayedCameras.Count == 0)
			{
				AddValidCamerasFromGridToRelayed(base.Grid);
			}
		}

		private void AddValidCamerasFromGridToRelayed(long gridId)
		{
			MyEntities.TryGetEntityById(gridId, out MyCubeGrid entity, allowClosed: false);
			if (entity != null)
			{
				AddValidCamerasFromGridToRelayed(entity);
			}
		}

		private void AddValidCamerasFromGridToRelayed(MyCubeGrid grid)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyTerminalBlock> enumerator = grid.GridSystems.TerminalSystem.Blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyCameraBlock myCameraBlock = enumerator.get_Current() as MyCameraBlock;
					if (myCameraBlock != null && myCameraBlock.IsWorking && myCameraBlock.HasLocalPlayerAccess())
					{
						m_relayedCameras.Add(myCameraBlock);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private MyCameraBlock GetNext(MyCameraBlock current)
		{
			if (m_relayedCameras.Count == 1)
			{
				return current;
			}
			int num = m_relayedCameras.IndexOf(current);
			if (num == -1)
			{
				ResetCamera();
				return null;
			}
			return m_relayedCameras[(num + 1) % m_relayedCameras.Count];
		}

		private MyCameraBlock GetPrev(MyCameraBlock current)
		{
			if (m_relayedCameras.Count == 1)
			{
				return current;
			}
			int num = m_relayedCameras.IndexOf(current);
			if (num == -1)
			{
				ResetCamera();
				return null;
			}
			int num2 = num - 1;
			if (num2 < 0)
			{
				num2 = m_relayedCameras.Count - 1;
			}
			return m_relayedCameras[num2];
		}
	}
}
