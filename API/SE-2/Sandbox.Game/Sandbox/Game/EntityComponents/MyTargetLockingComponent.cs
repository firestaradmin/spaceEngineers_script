using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	[StaticEventOwner]
	[MyComponentType(typeof(MyTargetLockingComponent))]
	[MyComponentBuilder(typeof(MyObjectBuilder_TargetLockingComponent), true)]
	public class MyTargetLockingComponent : MyGameLogicComponent, IMyEventProxy, IMyEventOwner
	{
		protected sealed class OnTargetRequestServer_003C_003ESystem_Int64_0023System_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long shooterId, in long controlledBlockId, in long targetId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnTargetRequestServer(shooterId, controlledBlockId, targetId);
			}
		}

		protected sealed class RequestReleaseTargetLock_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long shooterId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestReleaseTargetLock(shooterId);
			}
		}

		protected sealed class LooseLockOnClient_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long shooterId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				LooseLockOnClient(shooterId);
			}
		}

		protected sealed class OnTargetLostClient_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long shooterId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnTargetLostClient(shooterId);
			}
		}

		private class Sandbox_Game_EntityComponents_MyTargetLockingComponent_003C_003EActor : IActivator, IActivator<MyTargetLockingComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyTargetLockingComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTargetLockingComponent CreateInstance()
			{
				return new MyTargetLockingComponent();
			}

			MyTargetLockingComponent IActivator<MyTargetLockingComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public const int OnLockToolbarPosition = 0;

		public const int OnUnLockToolbarPosition = 1;

		public const int LOST_GRID_RETRY_ATTEMPTS = 30;

		private bool m_isListenerAttached;

		private IMyTargetingCapableBlock m_controlledBlock;

		private bool? m_currentTargetIsVisible;

		private int m_refreshTimerIsVisible;

		private double m_lastOnLockEventTime;

		private IMySourceVoice m_lockingProgressSound;

		private long m_previousTargetId;

		private bool m_lastFrameTargetLocked;

		private int m_lastSampleTimestamp;

		public MyTargetLockingComponentDefinition Definition { get; private set; }

		private MyCubeGrid Shooter
		{
			get
			{
				MyCubeBlock myCubeBlock = (base.Entity as MyCharacter)?.IsUsing as MyCubeBlock;
				IMyTargetingCapableBlock myTargetingCapableBlock = myCubeBlock as IMyTargetingCapableBlock;
				if (myTargetingCapableBlock != null && myTargetingCapableBlock.IsTargetLockingEnabled())
				{
					return myCubeBlock?.CubeGrid;
				}
				return null;
			}
		}

		private MyCharacter.MyTargetData TargetData
		{
			get
			{
				return (base.Entity as MyCharacter)?.TargetLockData?.Value ?? MyCharacter.MyTargetData.EmptyTargetData;
			}
			set
			{
				MyCharacter myCharacter = base.Entity as MyCharacter;
				if (myCharacter != null && myCharacter.TargetLockData != null)
				{
					myCharacter.TargetLockData.Value = value;
				}
			}
		}

		public bool IsTargetLocked => TargetData.IsTargetLocked;

		public bool IsLosingLock
		{
			get
			{
				if (IsTargetLocked)
				{
					return TargetData.LockingProgress < 1f;
				}
				return false;
			}
		}

		public MyCubeGrid Target => MyEntities.GetEntityById(TargetData.TargetId) as MyCubeGrid;

		private bool IsLocking
		{
			get
			{
				if (Target != null)
				{
					return Shooter != null;
				}
				return false;
			}
		}

		private MyTargetFocusComponent TargetFocusComponent => (base.Entity as MyCharacter)?.Components.Get<MyTargetFocusComponent>();

		public float LockingProgressPercent => 100f * TargetData.LockingProgress;

		public float LockingProgressMilliseconds => LockingProgressPercent / 1000f;

		public float LockingTimeRemainingMilliseconds
		{
			get
			{
				int timeToLockInMillis = GetTimeToLockInMillis(Shooter, Target);
				return MathHelper.Max((1f - TargetData.LockingProgress) * (float)timeToLockInMillis, 0f);
			}
		}

		public float GetTimeToLockIsSec(MyCubeGrid shooter, MyCubeGrid target)
		{
			double num = (target.PositionComp.GetPosition() - shooter.PositionComp.GetPosition()).Length() / TargetFocusComponent.FocusSearchMaxDistance;
			float num2 = ((target.GridSizeEnum == MyCubeSize.Small) ? Definition.LockingModifierSmallGrid : Definition.LockingModifierLargeGrid);
			return (float)MathHelper.Min(MathHelper.Max(num * (double)Definition.LockingModifierDistance * (double)num2, Definition.LockingTimeMin), Definition.LockingTimeMax);
		}

		public int GetTimeToLockInMillis(MyCubeGrid shooter, MyCubeGrid target)
		{
			return (int)(1000f * GetTimeToLockIsSec(shooter, target));
		}

		public bool CanTarget(MyCubeGrid target)
		{
			return CanTarget(m_controlledBlock, target, checkAngle: false);
		}

		public bool CanTarget(IMyTargetingCapableBlock controlledBlock, MyCubeGrid target, bool checkAngle)
		{
			if (Shooter == null || target == null || controlledBlock == null || controlledBlock.IsShipToolSelected())
			{
				return false;
			}
			MatrixD shooterMatrix = controlledBlock.GetWorldMatrix();
			MyCharacter myCharacter = base.Entity as MyCharacter;
			if (myCharacter != null && MySession.Static.Players.GetCameraData(myCharacter.ControlSteamId, out var type, out var entityId) && type != 0 && entityId != 0L)
			{
				MyEntity entityById = MyEntities.GetEntityById(entityId);
				MyCameraBlock myCameraBlock;
				MyCockpit myCockpit;
				if ((myCameraBlock = entityById as MyCameraBlock) != null)
				{
					shooterMatrix = myCameraBlock.WorldMatrix;
				}
				else if ((myCockpit = entityById as MyCockpit) != null)
				{
					shooterMatrix = MatrixD.CreateFromQuaternion(myCockpit.CameraRotation);
					shooterMatrix.Translation = myCockpit.CameraPosition;
				}
			}
			double targetCosSquaredSigned;
			return TargetFocusComponent.IsTargetInRange(shooterMatrix, target, out targetCosSquaredSigned, checkAngle);
		}

		private void LazyAttachListeners()
		{
			if (!m_isListenerAttached)
			{
				Sync<MyCharacter.MyTargetData, SyncDirection.FromServer> sync = (base.Entity as MyCharacter)?.TargetLockData;
				if (sync != null && base.Entity is MyCharacter)
				{
					sync.ValueChanged -= OnTargetInfoChanged;
					sync.ValueChanged += OnTargetInfoChanged;
					m_isListenerAttached = true;
				}
			}
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
			Definition = definition as MyTargetLockingComponentDefinition;
		}

		public void OnTargetRequest(MyCubeGrid target)
		{
			LazyAttachListeners();
			if (Target == null)
			{
				PlaySoundLockingStarted();
			}
			else
			{
				PlaySoundLockingComplete(lockSuccess: false);
			}
			MyCubeBlock myCubeBlock = MySession.Static.ControlledEntity as MyCubeBlock;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnTargetRequestServer, base.Entity.EntityId, myCubeBlock?.EntityId ?? 0, target?.EntityId ?? 0);
		}

		[Event(null, 205)]
		[Reliable]
		[Server]
		private static void OnTargetRequestServer(long shooterId, long controlledBlockId, long targetId)
		{
			ulong value = MyEventContext.Current.Sender.Value;
			MyCharacter myCharacter = MyEntities.GetEntityById(shooterId) as MyCharacter;
			if (value != myCharacter?.ControlSteamId || myCharacter == null)
			{
				return;
			}
			myCharacter.TargetLockingComp.OnTargetRequestInternal(controlledBlockId, targetId, out var releaseTargetOnClient);
			if (releaseTargetOnClient)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => LooseLockOnClient, shooterId, MyEventContext.Current.Sender);
			}
		}

		private void OnTargetRequestInternal(long controlledBlockId, long targetId, out bool releaseTargetOnClient)
		{
			releaseTargetOnClient = false;
			if (Target != null)
			{
				if (TargetData.IsTargetLocked)
				{
					releaseTargetOnClient = true;
				}
				ReleaseTargetLock();
				return;
			}
			LazyAttachListeners();
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(targetId) as MyCubeGrid;
			IMyTargetingCapableBlock myTargetingCapableBlock = MyEntities.GetEntityById(controlledBlockId) as IMyTargetingCapableBlock;
			if (myTargetingCapableBlock == null || myCubeGrid == null || !CanTarget(myTargetingCapableBlock, myCubeGrid, checkAngle: true))
			{
				TargetData = MyCharacter.MyTargetData.EmptyTargetData;
				OnTargetLostServer();
				return;
			}
			TargetData = new MyCharacter.MyTargetData
			{
				TargetId = targetId,
				IsTargetLocked = false,
				LockingProgress = 0f,
				ControlledBlockId = controlledBlockId
			};
			m_lastSampleTimestamp = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_controlledBlock = myTargetingCapableBlock;
		}

		public void OnControlledEntityChanged(IMyControllableEntity oldEntity, IMyControllableEntity newEntity)
		{
			if (m_lockingProgressSound != null)
			{
				m_lockingProgressSound.Stop();
			}
			if (!Sync.IsServer)
			{
				return;
			}
			IMyTargetingCapableBlock myTargetingCapableBlock = newEntity as IMyTargetingCapableBlock;
			MyEntities.GetEntityById(TargetData.ControlledBlockId);
			if (myTargetingCapableBlock == null)
			{
				MyCockpit myCockpit;
				if ((myCockpit = oldEntity as MyCockpit) != null)
				{
					myCockpit.TargetData = ((TargetData.TargetId != 0L) ? TargetData : MyCharacter.MyTargetData.EmptyTargetData);
				}
				ReleaseTargetLock();
				return;
			}
			MyCockpit myCockpit2;
			if ((myCockpit2 = oldEntity as MyCockpit) != null)
			{
				myCockpit2.TargetData = MyCharacter.MyTargetData.EmptyTargetData;
			}
			MyCockpit myCockpit3 = myTargetingCapableBlock as MyCockpit;
			MyCharacter.MyTargetData targetData;
			if (myCockpit3 != null && myCockpit3.TargetData.TargetId != 0L)
			{
				if (MyEntities.TryGetEntityById(myCockpit3.TargetData.TargetId, out var _))
				{
					targetData = new MyCharacter.MyTargetData
					{
						TargetId = myCockpit3.TargetData.TargetId,
						IsTargetLocked = myCockpit3.TargetData.IsTargetLocked,
						LockingProgress = myCockpit3.TargetData.LockingProgress,
						ControlledBlockId = (myTargetingCapableBlock as MyEntity).EntityId
					};
					TargetData = targetData;
					m_controlledBlock = myTargetingCapableBlock;
					return;
				}
				myCockpit3.TargetData = MyCharacter.MyTargetData.EmptyTargetData;
			}
			MyCharacter.MyTargetData targetData2 = TargetData;
			targetData = (TargetData = new MyCharacter.MyTargetData
			{
				TargetId = targetData2.TargetId,
				IsTargetLocked = targetData2.IsTargetLocked,
				LockingProgress = targetData2.LockingProgress,
				ControlledBlockId = (myTargetingCapableBlock as MyEntity).EntityId
			});
			m_controlledBlock = myTargetingCapableBlock;
		}

		public void ReleaseTargetLockRequest()
		{
			if (Sync.IsServer)
			{
				ReleaseTargetLock();
				return;
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestReleaseTargetLock, base.Entity.EntityId);
		}

		[Event(null, 325)]
		[Reliable]
		[Server]
		public static void RequestReleaseTargetLock(long shooterId)
		{
			ulong value = MyEventContext.Current.Sender.Value;
			MyCharacter myCharacter = MyEntities.GetEntityById(shooterId) as MyCharacter;
			if (value == myCharacter?.ControlSteamId && myCharacter != null)
			{
				myCharacter.TargetLockingComp.ReleaseTargetLock();
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => LooseLockOnClient, shooterId, MyEventContext.Current.Sender);
			}
		}

		public void ReleaseTargetLock()
		{
			if (TargetData.IsTargetLocked)
			{
				m_previousTargetId = TargetData.TargetId;
			}
			if (Sync.IsServer)
			{
				TargetData = new MyCharacter.MyTargetData
				{
					TargetId = 0L,
					IsTargetLocked = false,
					LockingProgress = 0f,
					ControlledBlockId = 0L
				};
			}
			m_lastSampleTimestamp = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_controlledBlock = null;
			m_lockingProgressSound?.Stop();
			m_lockingProgressSound = null;
		}

		private void OnTargetLostServer()
		{
			ulong? num = (base.Entity as MyCharacter)?.ControlInfo.SteamId;
			if (num.HasValue)
			{
				MyMultiplayer.RaiseStaticEvent(targetEndpoint: new EndpointId(num.Value), action: (IMyEventOwner x) => OnTargetLostClient, arg2: base.Entity.EntityId);
			}
		}

		[Event(null, 376)]
		[Reliable]
		[Client]
		private static void LooseLockOnClient(long shooterId)
		{
			_ = MyEventContext.Current;
			(MyEntities.GetEntityById(shooterId) as MyCharacter)?.TargetLockingComp.ReleaseTargetLock();
		}

		[Event(null, 384)]
		[Reliable]
		[Client]
		private static void OnTargetLostClient(long shooterId)
		{
			_ = MyEventContext.Current;
			(MyEntities.GetEntityById(shooterId) as MyCharacter)?.TargetLockingComp.PlaySoundLockingComplete(lockSuccess: false);
		}

		/// <summary>
		/// Should be called in UpdateBeforeSimulation1
		/// We can let characters to have update 1 as there are only few of them in world
		/// </summary>
		public void UpdateCanTarget()
		{
			if (IsLocking && Sync.IsServer && (m_refreshTimerIsVisible++ % 100 != 0 || m_currentTargetIsVisible.HasValue))
			{
				MyCubeBlock myCubeBlock = (base.Entity as MyCharacter)?.IsUsing as MyCubeBlock;
				if (myCubeBlock == null)
				{
					m_currentTargetIsVisible = false;
				}
				else if (Target == null)
				{
					m_currentTargetIsVisible = false;
				}
				else if (!CanTarget(Target))
				{
					m_currentTargetIsVisible = false;
				}
				else
				{
					MyTargetingHelper.Instance.RaycastCheck(myCubeBlock, Target, Callback);
				}
			}
		}

		private void Callback(MyCubeGrid grid, List<MyPhysics.HitInfo> hits)
		{
			if (grid == Target)
			{
				MyCubeBlock myCubeBlock = (base.Entity as MyCharacter)?.IsUsing as MyCubeBlock;
				if (myCubeBlock == null)
				{
					m_currentTargetIsVisible = false;
				}
				else
				{
					m_currentTargetIsVisible = MyTargetingHelper.Instance.IsVisible(myCubeBlock.CubeGrid, grid, hits);
				}
			}
		}

		public void Update()
		{
			if (Shooter == null)
			{
				if (Target != null)
				{
					ReleaseTargetLockRequest();
				}
				else if (m_lockingProgressSound != null)
				{
					m_lockingProgressSound?.Stop();
					m_lockingProgressSound = null;
				}
				return;
			}
			MyCubeGrid controlledGrid = MySession.Static.ControlledGrid;
			MyCubeBlock myCubeBlock = MySession.Static.ControlledEntity as MyCubeBlock;
			if (controlledGrid != null && myCubeBlock != null && (!controlledGrid.IsPowered || !myCubeBlock.IsWorking))
			{
				return;
			}
			if (IsTargetLocked && Target == null)
			{
				ReleaseTargetLockRequest();
			}
			IMyTargetingCapableBlock myTargetingCapableBlock;
			if ((myTargetingCapableBlock = MySession.Static.ControlledEntity as IMyTargetingCapableBlock) != null && (myTargetingCapableBlock.IsShipToolSelected() || !myTargetingCapableBlock.IsTargetLockingEnabled()))
			{
				ReleaseTargetLockRequest();
			}
			else
			{
				if (!IsLocking || !Sync.IsServer || !m_currentTargetIsVisible.HasValue)
				{
					return;
				}
				int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				float num = GetTimeToLockInMillis(Shooter, Target);
				float num2 = (float)(totalGamePlayTimeInMilliseconds - m_lastSampleTimestamp) / num;
				m_lastSampleTimestamp = totalGamePlayTimeInMilliseconds;
				if (!m_currentTargetIsVisible.Value)
				{
					num2 *= -1f;
				}
				MyCharacter.MyTargetData targetData = TargetData;
				float num3 = ((!m_currentTargetIsVisible.Value || !IsTargetLocked) ? (targetData.LockingProgress + num2) : 1f);
				bool isTargetLocked = targetData.IsTargetLocked;
				if (num3 <= 0f)
				{
					ReleaseTargetLock();
					return;
				}
				if (num3 >= 1f)
				{
					num3 = 1f;
					if (!targetData.IsTargetLocked)
					{
						isTargetLocked = true;
					}
				}
				TargetData = new MyCharacter.MyTargetData
				{
					TargetId = targetData.TargetId,
					IsTargetLocked = isTargetLocked,
					LockingProgress = num3,
					ControlledBlockId = targetData.ControlledBlockId
				};
			}
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
		}

		public override void Close()
		{
			Sync<MyCharacter.MyTargetData, SyncDirection.FromServer> sync = (base.Entity as MyCharacter)?.TargetLockData;
			if (sync != null && base.Entity is MyCharacter)
			{
				sync.ValueChanged -= OnTargetInfoChanged;
				m_isListenerAttached = false;
			}
		}

		public void OnTargetInfoChanged(SyncBase obj)
		{
			m_currentTargetIsVisible = null;
			if (m_lastFrameTargetLocked != TargetData.IsTargetLocked)
			{
				m_lastFrameTargetLocked = TargetData.IsTargetLocked;
				if (TargetData.IsTargetLocked)
				{
					OnLockAcquired();
				}
				else
				{
					OnLockLost();
				}
			}
		}

		public void OnLockLost()
		{
			((base.Entity as MyCharacter)?.IsUsing as IMyTargetingCapableBlock)?.SetLockedTarget(null);
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(m_previousTargetId) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				foreach (MyCockpit fatBlock in myCubeGrid.GetFatBlocks<MyCockpit>())
				{
					if (fatBlock.OnLockedToolbar.ItemCount > 1)
					{
						fatBlock.OnLockedToolbar.UpdateItem(1);
						fatBlock.OnLockedToolbar.ActivateItemAtIndex(1);
					}
				}
			}
			PlaySoundLockLost();
		}

		public void OnLockAcquired()
		{
			IMyTargetingCapableBlock myTargetingCapableBlock = (base.Entity as MyCharacter)?.IsUsing as IMyTargetingCapableBlock;
			if (myTargetingCapableBlock == null)
			{
				return;
			}
			myTargetingCapableBlock.SetLockedTarget(Target);
			if (IsTargetLocked && Target != null && m_lastOnLockEventTime < MySession.Static.ElapsedGameTime.TotalSeconds)
			{
				m_lastOnLockEventTime = MySession.Static.ElapsedGameTime.TotalSeconds;
				foreach (MyCockpit fatBlock in Target.GetFatBlocks<MyCockpit>())
				{
					if (fatBlock.OnLockedToolbar.ItemCount > 0)
					{
						fatBlock.OnLockedToolbar.UpdateItem(0);
						fatBlock.OnLockedToolbar.ActivateItemAtIndex(0);
					}
				}
			}
			PlaySoundLockingComplete(lockSuccess: true);
		}

		private void PlaySoundLockingStarted()
		{
			if (m_lockingProgressSound == null)
			{
				m_lockingProgressSound = MyGuiAudio.PlaySound(MyGuiSounds.HudLockingProgress);
			}
		}

		private void PlaySoundLockingComplete(bool lockSuccess, bool forceSound = false)
		{
			if (m_lockingProgressSound != null || forceSound)
			{
				m_lockingProgressSound?.Stop();
				m_lockingProgressSound = null;
				MyGuiAudio.PlaySound(lockSuccess ? MyGuiSounds.HudLockingSuccess : MyGuiSounds.HudLockingLost);
			}
		}

		private void PlaySoundLockLost()
		{
			PlaySoundLockingComplete(lockSuccess: false, forceSound: true);
		}
	}
}
