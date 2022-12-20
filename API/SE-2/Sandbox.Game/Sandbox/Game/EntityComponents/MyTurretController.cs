using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Entities.UseObject;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.ClientStates;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	public class MyTurretController : IMyUsableEntity, IMyEventProxy, IMyEventOwner
	{
		protected sealed class RequestUseMessageStatic_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64_0023System_Int64 : ICallSite<IMyEventOwner, UseActionEnum, long, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in UseActionEnum useAction, in long usedById, in long entityId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestUseMessageStatic(useAction, usedById, entityId);
			}
		}

		protected sealed class RequestUseMessage_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64 : ICallSite<MyTurretController, UseActionEnum, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretController @this, in UseActionEnum useAction, in long usedById, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RequestUseMessage(useAction, usedById);
			}
		}

		protected sealed class UseSuccessStatic_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64_0023VRage_Game_Entity_UseObject_UseActionResult_0023System_Int64 : ICallSite<IMyEventOwner, UseActionEnum, long, UseActionResult, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in UseActionEnum useAction, in long usedById, in UseActionResult useResult, in long entityId, in DBNull arg5, in DBNull arg6)
			{
				UseSuccessStatic(useAction, usedById, useResult, entityId);
			}
		}

		protected sealed class UseSuccessCallback_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64_0023VRage_Game_Entity_UseObject_UseActionResult : ICallSite<MyTurretController, UseActionEnum, long, UseActionResult, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretController @this, in UseActionEnum useAction, in long usedById, in UseActionResult useResult, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.UseSuccessCallback(useAction, usedById, useResult);
			}
		}

		protected sealed class UseFailureStatic_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64_0023VRage_Game_Entity_UseObject_UseActionResult_0023System_Int64 : ICallSite<IMyEventOwner, UseActionEnum, long, UseActionResult, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in UseActionEnum useAction, in long usedById, in UseActionResult useResult, in long entityId, in DBNull arg5, in DBNull arg6)
			{
				UseFailureStatic(useAction, usedById, useResult, entityId);
			}
		}

		protected sealed class UseFailureCallback_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64_0023VRage_Game_Entity_UseObject_UseActionResult : ICallSite<MyTurretController, UseActionEnum, long, UseActionResult, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretController @this, in UseActionEnum useAction, in long usedById, in UseActionResult useResult, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.UseFailureCallback(useAction, usedById, useResult);
			}
		}

		protected bool m_isControlled;

		private readonly MyCubeBlock m_cubeBlock;

		private IMyControllableEntity m_previousControlledEntity;

		public long? SavedPreviousControlledEntityId { get; set; }

		public MyCharacter CockpitPilot { get; private set; }

		public IMyControllableEntity PreviousControlledEntity
		{
			get
			{
				if (SavedPreviousControlledEntityId.HasValue && TryFindSavedEntity())
				{
					SavedPreviousControlledEntityId = null;
					this.OnCameraOverlayUpdate.InvokeIfNotNull();
				}
				return m_previousControlledEntity;
			}
			set
			{
				if (value == m_previousControlledEntity)
				{
					return;
				}
				if (m_previousControlledEntity != null)
				{
					m_previousControlledEntity.Entity.OnMarkForClose -= Entity_OnPreviousMarkForClose;
					if (CockpitPilot != null)
					{
						CockpitPilot.OnMarkForClose -= Entity_OnPreviousMarkForClose;
					}
				}
				m_previousControlledEntity = value;
				if (m_previousControlledEntity == null)
				{
					return;
				}
				AddPreviousControllerEvents();
				if (PreviousControlledEntity is MyCockpit)
				{
					CockpitPilot = (PreviousControlledEntity as MyCockpit).Pilot;
					if (CockpitPilot != null)
					{
						CockpitPilot.OnMarkForClose += Entity_OnPreviousMarkForClose;
					}
				}
			}
		}

		public bool IsControlled
		{
			get
			{
				if (PreviousControlledEntity == null)
				{
					return m_isControlled;
				}
				return true;
			}
			set
			{
				m_isControlled = value;
			}
		}

		public bool IsPlayerControlled
		{
			get
			{
				if (IsControlled)
				{
					return true;
				}
				if (Sync.Players.GetControllingPlayer(m_cubeBlock) != null)
				{
					return true;
				}
				return false;
			}
		}

		public bool IsControlledByLocalPlayer
		{
			get
			{
				if (IsControlled && ControllerInfo.Controller != null)
				{
					return ControllerInfo.IsLocallyControlled();
				}
				return false;
			}
		}

		public MyCharacter Pilot
		{
			get
			{
				MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					return myCharacter;
				}
				return CockpitPilot;
			}
		}

		public MyControllerInfo ControllerInfo { get; } = new MyControllerInfo();


		/// <summary>
		/// Used to serialize user input.
		/// </summary>
		public MyGridClientState LastNetMoveState { get; set; }

		public bool LastNetCanControl { get; set; }

		public bool LastNetRotateShip { get; set; }

		public event Action OnCameraOverlayUpdate;

		public event Action OnRotationUpdate;

		public event Action<Vector3, Vector2, float, bool, bool> OnMoveAndRotationUpdate;

		public event Action<bool> OnControlReleased;

		public event Action<IMyControllableEntity> OnControlAcquired;

		public MyTurretController(MyCubeBlock cubeBlock)
		{
			m_cubeBlock = cubeBlock;
		}

		public void UpdatePlayerControllers()
		{
			if (SavedPreviousControlledEntityId.HasValue)
			{
				MySession.Static.Players.UpdatePlayerControllers(m_cubeBlock.EntityId);
				if (SavedPreviousControlledEntityId.HasValue)
				{
					TryFindSavedEntity();
					SavedPreviousControlledEntityId = null;
				}
			}
		}

		public bool CanControl()
		{
			if (!m_cubeBlock.IsWorking)
			{
				return false;
			}
			if (IsPlayerControlled)
			{
				return false;
			}
			MyCockpit myCockpit = MySession.Static.ControlledEntity as MyCockpit;
			if (myCockpit != null)
			{
				if (myCockpit is MyCryoChamber)
				{
					return false;
				}
				return MyAntennaSystem.Static.CheckConnection(myCockpit.CubeGrid, m_cubeBlock.CubeGrid, myCockpit.ControllerInfo.Controller.Player);
			}
			MyCharacter myCharacter = MySession.Static.ControlledEntity as MyCharacter;
			if (myCharacter != null)
			{
				return MyAntennaSystem.Static.CheckConnection(myCharacter, m_cubeBlock.CubeGrid, myCharacter.ControllerInfo.Controller.Player);
			}
			return false;
		}

		public void RequestControl()
		{
			if (MyFakes.ENABLE_TURRET_CONTROL && CanControl())
			{
				if (MyGuiScreenTerminal.IsOpen)
				{
					MyGuiScreenTerminal.Hide();
				}
				MySession.Static.GameFocusManager.Clear();
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestUseMessageStatic, UseActionEnum.Manipulate, MySession.Static.ControlledEntity.Entity.EntityId, m_cubeBlock.EntityId);
			}
		}

		public void SerializeControls(BitStream stream)
		{
			LastNetMoveState.Serialize(stream);
			stream.WriteBool(LastNetCanControl);
			stream.WriteBool(LastNetRotateShip);
		}

		public void DeserializeControls(BitStream stream, bool outOfOrder)
		{
			LastNetMoveState = new MyGridClientState(stream);
			LastNetCanControl = stream.ReadBool();
			LastNetRotateShip = stream.ReadBool();
			if (LastNetMoveState.Valid)
			{
				this.OnMoveAndRotationUpdate.InvokeIfNotNull(LastNetMoveState.Move, LastNetMoveState.Rotation, LastNetMoveState.Roll, LastNetCanControl, LastNetRotateShip);
			}
		}

		public MyCharacter GetUser()
		{
			if (PreviousControlledEntity != null)
			{
				if (PreviousControlledEntity is MyCockpit)
				{
					return (PreviousControlledEntity as MyCockpit).Pilot;
				}
				MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					return myCharacter;
				}
			}
			return null;
		}

		public bool IsInRangeAndPlayerHasAccess()
		{
			if (ControllerInfo.Controller == null)
			{
				return false;
			}
			MyTerminalBlock myTerminalBlock = PreviousControlledEntity as MyTerminalBlock;
			if (myTerminalBlock == null)
			{
				MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					return MyAntennaSystem.Static.CheckConnection(myCharacter, m_cubeBlock.CubeGrid, ControllerInfo.Controller.Player);
				}
				return true;
			}
			MyCubeGrid cubeGrid = myTerminalBlock.SlimBlock.CubeGrid;
			return MyAntennaSystem.Static.CheckConnection(cubeGrid, m_cubeBlock.CubeGrid, ControllerInfo.Controller.Player);
		}

		public MyDataReceiver GetFirstRadioReceiver()
		{
			MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
			if (myCharacter != null)
			{
				return myCharacter.RadioReceiver;
			}
			HashSet<MyDataReceiver> output = new HashSet<MyDataReceiver>();
			MyAntennaSystem.Static.GetEntityReceivers(m_cubeBlock.CubeGrid, ref output, 0L);
			if (output.Count > 0)
			{
				return output.FirstElement();
			}
			return null;
		}

		public UseActionResult CanUse(UseActionEnum actionEnum, IMyControllableEntity user)
		{
			if (m_cubeBlock.IsWorking)
			{
				if (IsPlayerControlled)
				{
					return UseActionResult.UsedBySomeoneElse;
				}
				return UseActionResult.OK;
			}
			return UseActionResult.AccessDenied;
		}

		public void RemoveUsers(bool local)
		{
		}

		[Event(null, 333)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private static void RequestUseMessageStatic(UseActionEnum useAction, long usedById, long entityId)
		{
			(MyEntities.GetEntityById(entityId) as IMyTurretControllerControllable).TurretController.RequestUseMessage(useAction, usedById);
		}

		[Event(null, 340)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		public void RequestUseMessage(UseActionEnum useAction, long usedById)
		{
			MyEntity entity;
			bool num = MyEntities.TryGetEntityById<MyEntity>(usedById, out entity, allowClosed: false);
			IMyControllableEntity user = entity as IMyControllableEntity;
			UseActionResult useActionResult = UseActionResult.OK;
			if (num && (useActionResult = ((IMyUsableEntity)this).CanUse(useAction, user)) == UseActionResult.OK)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UseSuccessStatic, useAction, usedById, useActionResult, m_cubeBlock.EntityId);
				UseSuccessCallback(useAction, usedById, useActionResult);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UseFailureStatic, useAction, usedById, useActionResult, m_cubeBlock.EntityId);
			}
		}

		[Event(null, 362)]
		[Reliable]
		[Broadcast]
		private static void UseSuccessStatic(UseActionEnum useAction, long usedById, UseActionResult useResult, long entityId)
		{
			(MyEntities.GetEntityById(entityId) as IMyTurretControllerControllable)?.TurretController.UseSuccessCallback(useAction, usedById, useResult);
		}

		[Event(null, 369)]
		[Reliable]
		[Broadcast]
		public void UseSuccessCallback(UseActionEnum useAction, long usedById, UseActionResult useResult)
		{
			if (!MyEntities.TryGetEntityById<MyEntity>(usedById, out MyEntity entity, allowClosed: false))
			{
				return;
			}
			IMyControllableEntity myControllableEntity = entity as IMyControllableEntity;
			if (myControllableEntity != null)
			{
				MyRelationsBetweenPlayerAndBlock relations = MyRelationsBetweenPlayerAndBlock.NoOwnership;
				MyCubeBlock cubeBlock = m_cubeBlock;
				if (cubeBlock != null && myControllableEntity.ControllerInfo.Controller != null)
				{
					relations = cubeBlock.GetUserRelationToOwner(myControllableEntity.ControllerInfo.Controller.Player.Identity.IdentityId);
				}
				if (relations.IsFriendly() || MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
				{
					sync_UseSuccess(useAction, myControllableEntity);
				}
				else
				{
					sync_UseFailed(useAction, useResult, myControllableEntity);
				}
			}
		}

		[Event(null, 399)]
		[Reliable]
		[Client]
		private static void UseFailureStatic(UseActionEnum useAction, long usedById, UseActionResult useResult, long entityId)
		{
			(MyEntities.GetEntityById(entityId) as IMyTurretControllerControllable).TurretController.UseFailureCallback(useAction, usedById, useResult);
		}

		[Event(null, 406)]
		[Reliable]
		[Client]
		public void UseFailureCallback(UseActionEnum useAction, long usedById, UseActionResult useResult)
		{
			MyEntities.TryGetEntityById<MyEntity>(usedById, out MyEntity entity, allowClosed: false);
			IMyControllableEntity user = entity as IMyControllableEntity;
			sync_UseFailed(useAction, useResult, user);
		}

		private void sync_UseFailed(UseActionEnum action, UseActionResult actionResult, IMyControllableEntity user)
		{
			if (user != null && user.ControllerInfo.IsLocallyHumanControlled())
			{
				switch (actionResult)
				{
				case UseActionResult.UsedBySomeoneElse:
					MyHud.Notifications.Add(new MyHudNotification(MyCommonTexts.AlreadyUsedBySomebodyElse, 2500, "Red"));
					break;
				case UseActionResult.MissingDLC:
					MySession.Static.CheckDLCAndNotify(m_cubeBlock.BlockDefinition);
					break;
				default:
					MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
					break;
				}
			}
		}

		private void sync_UseSuccess(UseActionEnum action, IMyControllableEntity user)
		{
			PreviousControlledEntity = user;
			this.OnControlAcquired.InvokeIfNotNull(user);
		}

		private bool TryFindSavedEntity()
		{
			if (ControllerInfo.Controller != null && MyEntities.TryGetEntityById(SavedPreviousControlledEntityId.Value, out var entity))
			{
				PreviousControlledEntity = (IMyControllableEntity)entity;
				if (m_previousControlledEntity is MyCockpit)
				{
					CockpitPilot = (m_previousControlledEntity as MyCockpit).Pilot;
				}
				this.OnRotationUpdate.InvokeIfNotNull();
				return true;
			}
			return false;
		}

		private void Entity_OnPreviousMarkForClose(MyEntity obj)
		{
			this.OnControlReleased.InvokeIfNotNull(arg1: true);
		}

		private void AddPreviousControllerEvents()
		{
			m_previousControlledEntity.Entity.OnMarkForClose += Entity_OnPreviousMarkForClose;
			MyTerminalBlock myTerminalBlock = m_previousControlledEntity.Entity as MyTerminalBlock;
			if (myTerminalBlock != null)
			{
				myTerminalBlock.IsWorkingChanged += PreviousCubeBlock_IsWorkingChanged;
			}
		}

		private void PreviousCubeBlock_IsWorkingChanged(MyCubeBlock obj)
		{
			if (!obj.IsWorking && !obj.Closed && !obj.MarkedForClose)
			{
				this.OnControlReleased.InvokeIfNotNull(arg1: false);
			}
		}
	}
}
