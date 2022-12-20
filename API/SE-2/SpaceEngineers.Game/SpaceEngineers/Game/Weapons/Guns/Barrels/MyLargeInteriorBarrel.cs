using Sandbox;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.Weapons.Guns.Barrels;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.Weapons.Guns.Barrels
{
	internal class MyLargeInteriorBarrel : MyLargeBarrelBase
	{
		public override void Init(MyEntity entity, MyLargeTurretBase turretBase)
		{
			base.Init(entity, turretBase);
			if (!m_gunBase.HasDummies)
			{
				Vector3 position = -base.Entity.PositionComp.WorldMatrixRef.Forward * 0.800000011920929;
				m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, Matrix.CreateTranslation(position));
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (Sync.IsDedicated)
			{
				return;
			}
			if (m_shotSmoke != null)
			{
				m_shotSmoke.WorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
				if (m_smokeToGenerate > 0)
				{
					m_shotSmoke.UserBirthMultiplier = m_smokeToGenerate;
				}
				else
				{
					m_shotSmoke.Stop(instant: false);
					m_shotSmoke = null;
				}
			}
			if (m_muzzleFlash != null)
			{
				if (m_smokeToGenerate == 0)
				{
					m_muzzleFlash.Stop();
					m_muzzleFlash = null;
				}
				else
				{
					m_muzzleFlash.WorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
				}
			}
		}

		public override void Draw()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawLine3D(m_entity.PositionComp.GetPosition(), m_entity.PositionComp.GetPosition() + m_entity.WorldMatrix.Forward, Color.Green, Color.GreenYellow, depthRead: false);
				if (GetWeaponBase().Target != null)
				{
					MyRenderProxy.DebugDrawSphere(GetWeaponBase().Target.PositionComp.GetPosition(), 0.4f, Color.Green, 1f, depthRead: false);
				}
			}
		}

		public override bool StartShooting()
		{
			if (m_lateStartRandom > m_currentLateStart && !m_dontTimeOffsetNextShot)
			{
				m_currentLateStart++;
				return false;
			}
			m_dontTimeOffsetNextShot = false;
			if (!base.StartShooting())
			{
				return false;
			}
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot < m_gunBase.ShootIntervalInMiliseconds)
			{
				return false;
			}
			if (Sync.IsServer && m_turretBase != null && !m_turretBase.TargetingSystem.IsTargetVisible())
			{
				return false;
			}
			if (!Sync.IsDedicated)
			{
				m_muzzleFlashLength = MyUtils.GetRandomFloat(1f, 2f);
				m_muzzleFlashRadius = MyUtils.GetRandomFloat(0.3f, 0.5f);
				if (m_turretBase.IsControlledByLocalPlayer)
				{
					m_muzzleFlashLength *= 0.33f;
					m_muzzleFlashRadius *= 0.33f;
				}
				IncreaseSmoke();
				if (m_shotSmoke == null)
				{
					if (m_smokeToGenerate > 0)
					{
<<<<<<< HEAD
						MatrixD effectMatrix = m_gunBase.GetMuzzleWorldMatrix();
						Vector3D worldPosition = effectMatrix.Translation;
						MyParticlesManager.TryCreateParticleEffect("Smoke_LargeGunShot", ref effectMatrix, ref worldPosition, uint.MaxValue, out m_shotSmoke);
=======
						MyParticlesManager.TryCreateParticleEffect("Smoke_LargeGunShot", m_gunBase.GetMuzzleWorldMatrix(), out m_shotSmoke);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				else if (m_shotSmoke.IsEmittingStopped)
				{
					m_shotSmoke.Play();
				}
				if (m_muzzleFlash == null)
				{
<<<<<<< HEAD
					MatrixD effectMatrix2 = m_gunBase.GetMuzzleWorldMatrix();
					Vector3D worldPosition2 = effectMatrix2.Translation;
					MyParticlesManager.TryCreateParticleEffect("Muzzle_Flash_Large", ref effectMatrix2, ref worldPosition2, uint.MaxValue, out m_muzzleFlash);
=======
					MyParticlesManager.TryCreateParticleEffect("Muzzle_Flash_Large", m_gunBase.GetMuzzleWorldMatrix(), out m_muzzleFlash);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				if (m_shotSmoke != null)
				{
					m_shotSmoke.WorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
				}
				if (m_muzzleFlash != null)
				{
					m_muzzleFlash.WorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
				}
				GetWeaponBase().PlayShootingSound();
			}
			Shoot(base.Entity.PositionComp.GetPosition());
			m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			return true;
		}

		public override void StopShooting()
		{
			base.StopShooting();
			m_currentLateStart = 0;
			if (m_muzzleFlash != null)
			{
				m_muzzleFlash.Stop();
				m_muzzleFlash = null;
			}
		}

		public override void Close()
		{
			if (m_shotSmoke != null)
			{
				m_shotSmoke.Stop();
				m_shotSmoke = null;
			}
			if (m_muzzleFlash != null)
			{
				m_muzzleFlash.Stop();
				m_muzzleFlash = null;
			}
		}
	}
}
