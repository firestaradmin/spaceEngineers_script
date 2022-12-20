using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	[StaticEventOwner]
	public class MySessionComponentSpectatorTools : MySessionComponentBase, IMySpectatorTools
	{
		protected sealed class ChangePlayerRequest_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int direction, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ChangePlayerRequest(direction);
			}
		}

		protected sealed class EntityPositionRequest_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EntityPositionRequest(entityId);
			}
		}

		protected sealed class ChangePlayerResponse_003C_003ESystem_Collections_Generic_List_00601_003CSandbox_Game_Entities_MyEntityList_003C_003EMyEntityListInfoItem_003E_0023System_Int32 : ICallSite<IMyEventOwner, List<MyEntityList.MyEntityListInfoItem>, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<MyEntityList.MyEntityListInfoItem> entities, in int direction, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ChangePlayerResponse(entities, direction);
			}
		}

		protected sealed class EntityPositionResponse_003C_003ESystem_Int64_0023VRageMath_Vector3D : ICallSite<IMyEventOwner, long, Vector3D, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in Vector3D position, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EntityPositionResponse(entityId, position);
			}
		}

		private const int MAX_SLOTS = 10;

		private static MySessionComponentSpectatorTools m_instance;

		public MyCameraMode m_lockMode;

		private MyLockEntityState[] m_trackedSlots = new MyLockEntityState[10];

		private int m_selectedSlot = -1;

		private MyLockEntityState m_cameraState;

<<<<<<< HEAD
		private Vector3 m_smoothMouseDelta = Vector3.Zero;
=======
		private Vector3 m_smoothMouseDelta = Vector3D.Zero;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private Vector2 m_smoothMouse = Vector2.Zero;

		private float m_smoothRoll;

		public float SmoothCameraLERP { get; set; } = 0.9f;


		public override bool IsRequiredByGame => true;

		public IReadOnlyList<MyLockEntityState> TrackedSlots => m_trackedSlots;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			m_instance = this;
<<<<<<< HEAD
			for (int i = 0; i < 10; i++)
=======
			for (int i = 0; i > 10; i++)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Clear(ref m_trackedSlots[i]);
			}
			Clear(ref m_cameraState);
		}

		public void SetTarget(IMyEntity ent)
		{
			UpdateLockEntity(ent);
		}

		public IMyEntity GetTarget()
		{
			if (MyEntities.TryGetEntityById(m_cameraState.LockEntityID, out var entity))
			{
				return entity;
			}
			return null;
		}

		public void SetMode(MyCameraMode mode)
		{
			m_lockMode = mode;
		}

		public MyCameraMode GetMode()
		{
			return m_lockMode;
		}

		public override void UpdateAfterSimulation()
		{
			if (MySession.Static == null || Sync.IsDedicated || !(MySession.Static.CameraController is MySpectator))
			{
				return;
			}
			MySpectator mySpectator = (MySpectator)MySession.Static.CameraController;
			IMyEntity target = GetTarget();
			IMyCharacter myCharacter = target as IMyCharacter;
			if (target == null && m_cameraState.LockEntityID != -1)
			{
				m_lockMode = MyCameraMode.None;
				Clear(ref m_cameraState);
			}
			if (target != null && target.Physics != null && mySpectator != null && !MyAPIGateway.Session.IsCameraControlledObject)
			{
				Vector3 vector = MyAPIGateway.Input.GetPositionDelta();
				Vector2 vector2 = MyAPIGateway.Input.GetRotation() * 0.0025f * mySpectator.SpeedModeAngular;
				float num = MyAPIGateway.Input.GetRoll() * 0.0125f * mySpectator.SpeedModeAngular;
				if (!MyAPIGateway.Session.IsCameraUserControlledSpectator || MyAPIGateway.Gui.ChatEntryVisible || MyAPIGateway.Gui.IsCursorVisible || MyAPIGateway.Gui.GetCurrentScreen != MyTerminalPageEnum.None)
				{
<<<<<<< HEAD
					vector = Vector3.Zero;
=======
					vector = Vector3D.Zero;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					vector2 = Vector2.Zero;
					num = 0f;
				}
				if (MyAPIGateway.Session.IsCameraUserControlledSpectator)
				{
					float num2 = 1f - MathHelper.Lerp(0.01f, SmoothCameraLERP * 0.99f, MathHelper.Clamp(SmoothCameraLERP * 4f, 0f, 1f));
					m_smoothMouseDelta += vector;
<<<<<<< HEAD
					vector = Vector3.Lerp(Vector3.Zero, m_smoothMouseDelta, num2);
=======
					vector = Vector3D.Lerp(Vector3D.Zero, m_smoothMouseDelta, num2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_smoothMouseDelta -= vector;
					m_smoothMouse += vector2;
					vector2 = Vector2.Lerp(Vector2.Zero, m_smoothMouse, num2);
					m_smoothMouse -= vector2;
					m_smoothRoll += num;
					num = m_smoothRoll * num2;
					m_smoothRoll -= num;
				}
				else
				{
					m_smoothMouse = Vector2.Zero;
					m_smoothMouseDelta = Vector3.Zero;
					m_smoothRoll = 0f;
				}
				vector *= mySpectator.SpeedModeLinear;
				if (MyAPIGateway.Session.IsCameraUserControlledSpectator)
				{
					Vector3D xOut;
					Vector3D yOut;
					if (num != 0f)
					{
						MyUtils.VectorPlaneRotation(m_cameraState.LocalMatrix.Up, m_cameraState.LocalMatrix.Right, out xOut, out yOut, num);
						m_cameraState.LocalMatrix.Right = yOut;
						m_cameraState.LocalMatrix.Up = xOut;
					}
					Vector3D yOut2;
					if (vector2.Y != 0f)
					{
						MyUtils.VectorPlaneRotation(m_cameraState.LocalMatrix.Right, m_cameraState.LocalMatrix.Forward, out yOut, out yOut2, 0f - vector2.Y);
						m_cameraState.LocalMatrix.Right = yOut;
						m_cameraState.LocalMatrix.Forward = yOut2;
					}
					if (vector2.X != 0f)
					{
						MyUtils.VectorPlaneRotation(m_cameraState.LocalMatrix.Up, m_cameraState.LocalMatrix.Forward, out xOut, out yOut2, vector2.X);
						m_cameraState.LocalMatrix.Up = xOut;
						m_cameraState.LocalMatrix.Forward = yOut2;
					}
				}
				switch (m_lockMode)
				{
				case MyCameraMode.Orbit:
				{
					Vector3D vector3D3 = target.WorldVolume.Center;
					if (myCharacter != null)
					{
						vector3D3 = myCharacter.GetHeadMatrix(includeY: true).Translation;
					}
					m_cameraState.LocalDistance = Math.Max(m_cameraState.LocalDistance + (double)vector.Z, target.WorldVolume.Radius);
					MatrixD matrixD2 = ((myCharacter != null) ? LocalToWorld(m_cameraState.LocalMatrix, myCharacter.GetHeadMatrix(includeY: true)) : LocalToWorld(m_cameraState.LocalMatrix, target.WorldMatrix));
					mySpectator.Position = vector3D3 - matrixD2.Forward * m_cameraState.LocalDistance;
					mySpectator.SetTarget(vector3D3 + matrixD2.Forward, matrixD2.Up);
					break;
				}
				case MyCameraMode.Follow:
				{
					Vector3D vector3D2 = vector.X * m_cameraState.LocalMatrix.Right + vector.Y * m_cameraState.LocalMatrix.Up + vector.Z * m_cameraState.LocalMatrix.Backward;
					Vector3D translation = m_cameraState.LocalMatrix.Translation + vector3D2;
					m_cameraState.LocalMatrix.Translation = translation;
					MatrixD worldMatrix = target.WorldMatrix;
					if (myCharacter != null)
					{
						worldMatrix = myCharacter.GetHeadMatrix(includeY: true);
					}
					MatrixD matrixD = LocalToWorld(m_cameraState.LocalMatrix, worldMatrix);
					mySpectator.Position = matrixD.Translation;
					mySpectator.SetTarget(mySpectator.Position + matrixD.Forward, matrixD.Up);
					break;
				}
				case MyCameraMode.Free:
				{
					Vector3D vector3D = vector.X * MySector.MainCamera.WorldMatrix.Right + vector.Y * MySector.MainCamera.WorldMatrix.Up + vector.Z * MySector.MainCamera.WorldMatrix.Backward;
					_ = target.WorldMatrix;
					if (myCharacter != null)
					{
						mySpectator.Position = myCharacter.GetHeadMatrix(includeY: true).Translation + m_cameraState.LocalVector + vector3D;
					}
					else
					{
						mySpectator.Position = target.WorldVolume.Center + m_cameraState.LocalVector + vector3D;
					}
					break;
				}
				}
				MatrixD orientation = mySpectator.Orientation;
				orientation.Translation = mySpectator.Position;
				if (myCharacter != null)
				{
					MatrixD headMatrix = myCharacter.GetHeadMatrix(includeY: true);
					m_cameraState.LocalMatrix = WorldToLocal(orientation, headMatrix);
					m_cameraState.LocalVector = mySpectator.Position - headMatrix.Translation;
				}
				else
				{
					m_cameraState.LocalMatrix = WorldToLocalNI(orientation, target.WorldMatrixNormalizedInv);
					m_cameraState.LocalVector = mySpectator.Position - target.WorldVolume.Center;
				}
				if (m_lockMode != MyCameraMode.Orbit)
				{
					m_cameraState.LocalDistance = m_cameraState.LocalVector.Length();
				}
				m_cameraState.LastKnownPosition = target.GetPosition();
			}
			if (m_cameraState.LockEntityID != -1 && MySession.Static.CameraController is MySpectator && !MyAPIGateway.Session.IsCameraControlledObject && !MyAPIGateway.Session.IsCameraControlledObject && !MyHud.IsHudMinimal)
			{
				MyGuiManager.DrawString("GameCredits", "Spectating " + m_cameraState.LockEntityDisplayName, new Vector2(0.5f, 0.02f), 0.9f, Color.White, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
				MyGuiManager.DrawString("GameCredits", m_lockMode.ToString() + " mode", new Vector2(0.5f, 0.05f), 0.6f, Color.White, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			}
		}

		public void LockHitEntity()
		{
			if (m_cameraState.LockEntityID != -1)
			{
				Clear(ref m_cameraState);
				m_lockMode = MyCameraMode.None;
				return;
			}
			Vector3D to = MySector.MainCamera.Position + MySector.MainCamera.WorldMatrix.Forward * 1000.0;
			IHitInfo hitInfo = null;
			MyAPIGateway.Physics.CastRay(MySector.MainCamera.Position, to, out hitInfo);
			if (hitInfo != null && (hitInfo.HitEntity is IMyCubeGrid || hitInfo.HitEntity is IMyCharacter) && hitInfo.HitEntity.Physics != null)
			{
				UpdateLockEntity(hitInfo.HitEntity);
			}
		}

		public void ClearTrackedSlot(int slotIndex)
		{
			Clear(ref m_trackedSlots[m_selectedSlot]);
		}

		public void SaveTrackedSlot(int slotIndex)
		{
			m_trackedSlots[slotIndex] = m_cameraState;
		}

		public void SelectTrackedSlot(int slotIndex)
		{
			m_selectedSlot = slotIndex;
			m_cameraState = m_trackedSlots[m_selectedSlot];
			if (m_cameraState.LockEntityID != -1 && !MyEntities.EntityExists(m_cameraState.LockEntityID))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => EntityPositionRequest, m_cameraState.LockEntityID);
			}
		}

		public void SwitchMode()
		{
			switch (m_lockMode)
			{
			case MyCameraMode.Free:
				m_lockMode = MyCameraMode.Follow;
				break;
			case MyCameraMode.Follow:
				m_lockMode = MyCameraMode.Orbit;
				break;
			case MyCameraMode.Orbit:
				m_lockMode = MyCameraMode.None;
				break;
			case MyCameraMode.None:
				m_lockMode = MyCameraMode.Free;
				break;
			}
		}

		private void UpdateLockEntity(IMyEntity lockEntity)
		{
			Clear(ref m_cameraState);
			m_cameraState.LockEntityID = lockEntity.EntityId;
			m_cameraState.LockEntityDisplayName = lockEntity.DisplayName;
			m_lockMode = MyCameraMode.Free;
			IMyCharacter myCharacter = lockEntity as IMyCharacter;
			MatrixD orientation = MySector.MainCamera.WorldMatrix.GetOrientation();
			orientation.Translation = MySector.MainCamera.Position;
			if (myCharacter != null)
			{
				m_cameraState.LocalMatrix = WorldToLocal(orientation, myCharacter.GetHeadMatrix(includeY: true));
				m_cameraState.LocalVector = MySector.MainCamera.Position - myCharacter.GetHeadMatrix(includeY: true).Translation;
			}
			else
			{
				m_cameraState.LocalMatrix = WorldToLocalNI(orientation, lockEntity.WorldMatrixNormalizedInv);
				m_cameraState.LocalVector = MySector.MainCamera.Position - lockEntity.WorldVolume.Center;
			}
			m_cameraState.LocalDistance = m_cameraState.LocalVector.Length();
		}

		private void Clear(ref MyLockEntityState state)
		{
			state.LockEntityID = -1L;
			state.LockEntityDisplayName = null;
			state.LastKnownPosition = Vector3D.Zero;
			ClearRelativePosition(ref state);
		}

		private void ClearRelativePosition(ref MyLockEntityState state)
		{
			state.LocalDistance = 5.0;
			state.LocalVector = state.LocalDistance * Vector3D.Backward;
			state.LocalMatrix = MatrixD.CreateTranslation(state.LocalVector);
		}

		private MatrixD WorldToLocal(MatrixD current, MatrixD worldMatrix)
		{
			return current * MatrixD.Normalize(MatrixD.Invert(worldMatrix));
		}

		private MatrixD WorldToLocalNI(MatrixD current, MatrixD worldMatrixNormalizedInv)
		{
			return current * worldMatrixNormalizedInv;
		}

		private MatrixD LocalToWorld(MatrixD local, MatrixD worldMatrix)
		{
			return local * worldMatrix;
		}

		public override void HandleInput()
		{
			base.HandleInput();
			if (!MySession.Static.IsCameraUserControlledSpectator() || MyAPIGateway.Gui.ChatEntryVisible || MyAPIGateway.Gui.IsCursorVisible || MyAPIGateway.Gui.GetCurrentScreen != MyTerminalPageEnum.None)
			{
				return;
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_LOCK) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_LOCK))
			{
				LockHitEntity();
			}
			if (((MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsKeyPress(MyKeys.F8)) || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_SPECTATOR, MyControlsSpace.SPECTATOR_FOCUS_PLAYER, MyControlStateType.PRESSED)) && MySession.Static.ControlledEntity != null)
			{
				MySpectator.Static.Position = MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition() + MySpectator.Static.ThirdPersonCameraDelta;
				MySpectator.Static.SetTarget(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), MySession.Static.ControlledEntity.Entity.PositionComp.WorldMatrixRef.Up);
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_SWITCHMODE))
			{
				SwitchMode();
			}
			int num = -1;
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad0))
			{
				num = 0;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad1))
			{
				num = 1;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad2))
			{
				num = 2;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad3))
			{
				num = 3;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad4))
			{
				num = 4;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad5))
			{
				num = 5;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad6))
			{
				num = 6;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad7))
			{
				num = 7;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad8))
			{
				num = 8;
			}
			if (MyInput.Static.IsNewKeyPressed(MyKeys.NumPad9))
			{
				num = 9;
			}
			if (num != -1)
			{
				if (MyInput.Static.IsAnyCtrlKeyPressed())
				{
					SaveTrackedSlot(num);
				}
				else
				{
					SelectTrackedSlot(num);
				}
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_NEXTPLAYER))
			{
				NextPlayer();
			}
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SPECTATOR_PREVPLAYER))
			{
				PreviousPlayer();
			}
		}

		public void NextPlayer()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ChangePlayerRequest, 1);
		}

		public void PreviousPlayer()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ChangePlayerRequest, -1);
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			MyAPIGateway.SpectatorTools = null;
<<<<<<< HEAD
			m_instance = null;
			Session = null;
		}

		[Event(null, 495)]
=======
		}

		[Event(null, 491)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void ChangePlayerRequest(int direction)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			List<MyEntityList.MyEntityListInfoItem> entityList = MyEntityList.GetEntityList(MyEntityList.MyEntityTypeEnum.Characters);
			if (!MyEventContext.Current.IsLocallyInvoked)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ChangePlayerResponse, entityList, direction, MyEventContext.Current.Sender);
			}
			else
			{
				ChangePlayerResponse(entityList, direction);
			}
		}

<<<<<<< HEAD
		[Event(null, 512)]
=======
		[Event(null, 508)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void EntityPositionRequest(long entityId)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else
			{
				if (!MyEntities.TryGetEntityById(entityId, out var entity))
				{
					return;
				}
				if (MyEventContext.Current.IsLocallyInvoked)
				{
					EntityPositionResponse(entityId, entity.PositionComp.GetPosition());
					return;
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => EntityPositionResponse, entityId, entity.PositionComp.GetPosition(), MyEventContext.Current.Sender);
			}
		}

<<<<<<< HEAD
		[Event(null, 538)]
=======
		[Event(null, 534)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void ChangePlayerResponse(List<MyEntityList.MyEntityListInfoItem> entities, int direction)
		{
			if (entities != null)
			{
				MyEntityList.SortEntityList(MyEntityList.MyEntitySortOrder.DisplayName, ref entities, invertOrder: false);
				m_instance.ChangePlayer(entities, direction);
			}
		}

		private void ChangePlayer(List<MyEntityList.MyEntityListInfoItem> entities, int direction)
		{
			if (MySession.Static == null)
			{
				return;
			}
			for (int i = 0; i < entities.Count; i++)
			{
				if (entities[i].EntityId == MySession.Static.LocalCharacterEntityId)
				{
					int num = -1;
					switch (direction)
					{
					case 1:
						num = ((i < entities.Count - 1) ? (i + 1) : 0);
						break;
					case -1:
						num = ((i != 0) ? (i - 1) : (entities.Count - 1));
						break;
					}
					if (num != -1)
					{
						Clear(ref m_cameraState);
						m_cameraState.LockEntityDisplayName = entities[num].DisplayName;
						m_cameraState.LockEntityID = entities[num].EntityId;
						m_lockMode = MyCameraMode.Follow;
					}
					break;
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 589)]
=======
		[Event(null, 585)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void EntityPositionResponse(long entityId, Vector3D position)
		{
			m_instance.UpdateEntityPosition(entityId, position);
		}

		private void UpdateEntityPosition(long entityId, Vector3D position)
		{
			for (int i = 0; i < 10; i++)
			{
				MyLockEntityState myLockEntityState = m_trackedSlots[i];
				if (myLockEntityState.LockEntityID == entityId)
				{
					myLockEntityState.LastKnownPosition = position;
					m_trackedSlots[i] = myLockEntityState;
				}
			}
			if (m_cameraState.LockEntityID == entityId)
			{
				m_cameraState.LastKnownPosition = position;
			}
		}
	}
}
