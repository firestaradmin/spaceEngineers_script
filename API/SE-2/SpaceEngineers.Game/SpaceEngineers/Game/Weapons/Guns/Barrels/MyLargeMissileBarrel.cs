using Sandbox;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.Weapons.Guns.Barrels;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace SpaceEngineers.Game.Weapons.Guns.Barrels
{
	internal class MyLargeMissileBarrel : MyLargeBarrelBase
	{
		private int m_nextShootTime;

		private int m_shotsLeftInBurst;

		private MyEntity3DSoundEmitter m_soundEmitter;

		public int ShotsInBurst => m_gunBase.ShotsInBurst;

		public MyLargeMissileBarrel()
		{
			m_soundEmitter = new MyEntity3DSoundEmitter(m_entity);
		}

		public override void Init(MyEntity entity, MyLargeTurretBase turretBase)
		{
			base.Init(entity, turretBase);
			if (!m_gunBase.HasDummies)
			{
				Matrix identity = Matrix.Identity;
<<<<<<< HEAD
				identity.Translation += (Vector3)(entity.PositionComp.WorldMatrixRef.Forward * 3.0);
=======
				identity.Translation += entity.PositionComp.WorldMatrixRef.Forward * 3.0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_gunBase.AddMuzzleMatrix(MyAmmoType.Missile, identity);
			}
			m_shotsLeftInBurst = ShotsInBurst;
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Entity = turretBase;
			}
		}

		public void Init(Matrix localMatrix, MyLargeTurretBase parentObject)
		{
			m_shotsLeftInBurst = ShotsInBurst;
		}

		public override bool StartShooting()
		{
			if (m_turretBase == null || m_turretBase.Parent == null || m_turretBase.Parent.Physics == null)
			{
				return false;
			}
			if (Sync.IsServer)
			{
				if (m_lateStartRandom > m_currentLateStart && !m_dontTimeOffsetNextShot && !m_turretBase.IsControlled)
				{
					m_currentLateStart++;
					return false;
				}
				m_dontTimeOffsetNextShot = false;
			}
			if (DoesTimingAllowsShooting() && (m_shotsLeftInBurst > 0 || ShotsInBurst == 0))
			{
				MyEntity target = m_turretBase.Target;
				m_turretBase.TargetingSystem.TargetPrediction.UsePrediction = !IsControlledByPlayer();
				if (m_turretBase.IsControlled || (target != null && m_turretBase.TargetingSystem.IsTargetVisible()))
				{
					if (Sync.IsServer)
					{
						m_turretBase.IsReloadStarted = false;
						GetWeaponBase().RemoveAmmoPerShot();
					}
					if (IsControlledByPlayer())
					{
						m_gunBase.Shoot(m_turretBase.Parent.Physics.LinearVelocity, null, this);
					}
					else if (!m_turretBase.IsControlledByLocalPlayer && m_turretBase.TargetingSystem.Target != null)
					{
						Vector3D aimPosition = m_turretBase.TargetingSystem.AimPosition;
						if (!m_turretBase.TargetingSystem.TargetPrediction.IsLastPredictedCoordinatesInRange)
						{
							return false;
						}
						m_gunBase.Shoot(m_turretBase.Parent.Physics.LinearVelocity, aimPosition, null, this);
					}
					else
					{
						m_gunBase.Shoot(m_turretBase.Parent.Physics.LinearVelocity, null, this);
					}
					m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
					m_nextShootTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + m_gunBase.ShootIntervalInMiliseconds;
					if (ShotsInBurst > 0)
					{
						m_shotsLeftInBurst--;
						if (m_shotsLeftInBurst <= 0)
						{
							m_nextShootTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + m_gunBase.ReloadTime;
							m_reloadCompletionTime = m_nextShootTime;
							m_turretBase.ReloadTimeHandledByBarrel = true;
							m_turretBase.ReloadCompletionTime = m_reloadCompletionTime;
							m_turretBase.OnReloadStarted(m_gunBase.ReloadTime);
							if (Sync.IsServer)
							{
								m_turretBase.IsReloadStarted = true;
							}
							m_shotsLeftInBurst = ShotsInBurst;
						}
					}
				}
				m_turretBase.TargetingSystem.TargetPrediction.UsePrediction = false;
			}
			return true;
		}

		public override bool DoesTimingAllowsShooting()
		{
			if (base.DoesTimingAllowsShooting())
			{
				return m_nextShootTime <= MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
			return false;
		}

		public override void StopShooting()
		{
			base.StopShooting();
			m_currentLateStart = 0;
			m_soundEmitter.StopSound(forced: true);
		}

		private void StartSound()
		{
			m_gunBase.StartShootSound(m_soundEmitter);
		}

		public override void Close()
		{
			base.Close();
			m_soundEmitter.StopSound(forced: true);
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (!Sync.IsDedicated)
			{
				UpdateReloadNotification();
			}
		}
	}
}
