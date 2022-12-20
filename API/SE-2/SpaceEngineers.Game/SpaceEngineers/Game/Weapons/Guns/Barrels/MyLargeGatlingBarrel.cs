using System;
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
	internal class MyLargeGatlingBarrel : MyLargeBarrelBase
	{
<<<<<<< HEAD
=======
		private Vector3D m_muzzleFlashPosition;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private float m_rotationTimeout;

		private float m_lastRotation;

<<<<<<< HEAD
		private int m_shotsLeftInBurst;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public int ShotsInBurst => m_gunBase.ShotsInBurst;

		public MyLargeGatlingBarrel()
		{
			m_rotationTimeout = 2000f + MyUtils.GetRandomFloat(-500f, 500f);
		}

		public override void Init(MyEntity entity, MyLargeTurretBase turretBase)
		{
			base.Init(entity, turretBase);
			m_shotsLeftInBurst = ShotsInBurst;
			if (!m_gunBase.HasDummies)
			{
				Vector3 position = 2.0 * entity.PositionComp.WorldMatrixRef.Forward;
				m_gunBase.AddMuzzleMatrix(MyAmmoType.HighSpeed, Matrix.CreateTranslation(position));
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
<<<<<<< HEAD
			if (!Sync.IsDedicated)
			{
				float amount = 1f - MathHelper.Clamp((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot) / m_rotationTimeout, 0f, 1f);
				amount = MathHelper.SmoothStep(0f, 1f, amount);
				float num = amount * ((float)Math.PI * 4f) * 0.0166666675f;
				if (num != 0f && m_lastRotation != num)
=======
			if (Sync.IsDedicated)
			{
				return;
			}
			float amount = 1f - MathHelper.Clamp((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot) / m_rotationTimeout, 0f, 1f);
			amount = MathHelper.SmoothStep(0f, 1f, amount);
			float num = amount * ((float)Math.PI * 4f) * 0.0166666675f;
			if (num != 0f)
			{
				Matrix localMatrix = Matrix.CreateRotationZ(num) * base.Entity.PositionComp.LocalMatrixRef;
				base.Entity.PositionComp.SetLocalMatrix(ref localMatrix);
			}
			if (m_shotSmoke != null)
			{
				m_shotSmoke.WorldMatrix = m_gunBase.GetMuzzleWorldMatrix();
				if (m_smokeToGenerate > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					m_lastRotation = num;
					Matrix localMatrix = Matrix.CreateRotationZ(num) * base.Entity.PositionComp.LocalMatrixRef;
					base.Entity.PositionComp.SetLocalMatrix(ref localMatrix);
				}
				UpdateReloadNotification();
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
			if (!DoesTimingAllowsShooting() || !base.StartShooting() || (m_turretBase != null && !m_turretBase.TargetingSystem.IsTargetVisible()) || MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot < m_gunBase.ShootIntervalInMiliseconds)
			{
				return false;
			}
			if (m_lateStartRandom > m_currentLateStart && !m_dontTimeOffsetNextShot)
			{
				m_currentLateStart++;
				return false;
			}
			m_dontTimeOffsetNextShot = false;
			if (m_shotsLeftInBurst <= 0 && ShotsInBurst != 0)
			{
				return false;
			}
<<<<<<< HEAD
=======
			m_muzzleFlashLength = MyUtils.GetRandomFloat(4f, 6f);
			m_muzzleFlashRadius = MyUtils.GetRandomFloat(1.2f, 2f);
			if (m_turretBase.IsControlledByLocalPlayer)
			{
				m_muzzleFlashRadius *= 0.33f;
			}
			if (!Sync.IsDedicated)
			{
				IncreaseSmoke();
				m_muzzleFlashPosition = m_gunBase.GetMuzzleWorldPosition();
				if (m_shotSmoke == null)
				{
					if (m_smokeToGenerate > 0)
					{
						MyParticlesManager.TryCreateParticleEffect("Smoke_LargeGunShot", MatrixD.CreateTranslation(m_muzzleFlashPosition), out m_shotSmoke);
					}
				}
				else if (m_shotSmoke.IsEmittingStopped)
				{
					m_shotSmoke.Play();
				}
				if (m_muzzleFlash == null)
				{
					MyParticlesManager.TryCreateParticleEffect("Muzzle_Flash_Large", MatrixD.CreateTranslation(m_muzzleFlashPosition), out m_muzzleFlash);
				}
				m_shotSmoke?.SetTranslation(ref m_muzzleFlashPosition);
				m_muzzleFlash?.SetTranslation(ref m_muzzleFlashPosition);
				GetWeaponBase().PlayShootingSound();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Shoot(base.Entity.PositionComp.GetPosition());
			GetWeaponBase().PlayShootingSound();
			if (ShotsInBurst > 0)
			{
				m_shotsLeftInBurst--;
				if (m_shotsLeftInBurst <= 0)
				{
					m_reloadCompletionTime = MySandboxGame.TotalGamePlayTimeInMilliseconds + m_gunBase.ReloadTime;
					m_turretBase.OnReloadStarted(m_gunBase.ReloadTime);
					m_shotsLeftInBurst = ShotsInBurst;
					m_currentLateStart = 0;
				}
			}
			m_lastTimeShoot = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			return true;
		}

		public override void StopShooting()
		{
			base.StopShooting();
			base.GunBase.StopShoot();
			m_currentLateStart = 0;
<<<<<<< HEAD
=======
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
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
