using System;
<<<<<<< HEAD
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
=======
using Sandbox.Definitions;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Weapons.Guns.Barrels
{
	public abstract class MyLargeBarrelBase
	{
		protected MyGunBase m_gunBase;

		protected Matrix m_renderLocal;

		protected int m_lastTimeShoot;

		protected int m_lateStartRandom;

		protected int m_currentLateStart;

		private float m_barrelElevationMin;

		private float m_barrelSinElevationMin;

		protected MyParticleEffect m_shotSmoke;

		protected MyParticleEffect m_muzzleFlash;

		protected bool m_dontTimeOffsetNextShot;

		protected int m_smokeLastTime;

		protected int m_smokeToGenerate;

		protected float m_muzzleFlashLength;

		protected float m_muzzleFlashRadius;

		private bool m_isPreview;

<<<<<<< HEAD
		private bool m_isVisibleOutsidePreview = true;
=======
		private bool m_isVisibleOutsidePreview;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		protected MyEntity m_entity;

		protected MyLargeTurretBase m_turretBase;

		protected int m_nextNotificationTime;

		protected MyHudNotification m_reloadNotification;

		protected int m_reloadCompletionTime;

		public MyGunBase GunBase => m_gunBase;

		public MyModelDummy CameraDummy { get; private set; }

		public int LateTimeRandom
		{
			get
			{
				return m_lateStartRandom;
			}
			set
			{
				m_lateStartRandom = value;
			}
		}

		public float BarrelElevationMin
		{
			get
			{
				return m_barrelElevationMin;
			}
			protected set
			{
				m_barrelElevationMin = value;
				m_barrelSinElevationMin = (float)Math.Sin(m_barrelSinElevationMin);
			}
		}

		public float BarrelSinElevationMin => m_barrelSinElevationMin;

		public bool NeedsPerFrameUpdate => m_smokeToGenerate > 0;

		public MyEntity Entity => m_entity;

		public void ResetCurrentLateStart()
		{
			m_currentLateStart = 0;
		}

		public void DontTimeOffsetNextShot()
		{
			m_dontTimeOffsetNextShot = true;
		}

		public MyLargeBarrelBase()
		{
			m_lastTimeShoot = 0;
			BarrelElevationMin = -0.6f;
		}

		public virtual void Draw()
		{
		}

		public virtual void Init(MyEntity entity, MyLargeTurretBase turretBase)
		{
			m_entity = entity;
			m_turretBase = turretBase;
			m_gunBase = turretBase.GunBase;
			m_lateStartRandom = turretBase.LateStartRandom;
			if (m_entity.Model != null)
			{
				if (m_entity.Model.Dummies.ContainsKey("camera"))
				{
					CameraDummy = m_entity.Model.Dummies["camera"];
				}
				m_gunBase.LoadDummies(m_entity.Model.Dummies, m_turretBase.GetDummyNames());
			}
			m_entity.OnClose += m_entity_OnClose;
			UpdatePreviewState(turretBase.IsPreview);
		}

		protected void UpdateReloadNotification()
		{
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds > m_nextNotificationTime)
			{
				m_reloadNotification = null;
			}
			if (!m_gunBase.HasEnoughAmmunition() && MySession.Static.SurvivalMode)
			{
				MyHud.Notifications.Remove(m_reloadNotification);
				m_reloadNotification = null;
			}
			else if (!m_turretBase.IsControlledByLocalPlayer)
			{
				if (m_reloadNotification != null)
				{
					MyHud.Notifications.Remove(m_reloadNotification);
					m_reloadNotification = null;
				}
			}
			else if (m_reloadCompletionTime > MySandboxGame.TotalGamePlayTimeInMilliseconds)
			{
				ShowReloadNotification(m_reloadCompletionTime - MySandboxGame.TotalGamePlayTimeInMilliseconds);
			}
		}

		/// <summary>
		/// Will show the reload notification for the specified duration.
		/// </summary>
		/// <param name="duration">The time in MS it should show reloading.</param>
		protected void ShowReloadNotification(int duration)
		{
			int num = MySandboxGame.TotalGamePlayTimeInMilliseconds + duration;
			if (m_reloadNotification == null)
			{
				duration = Math.Max(0, duration - 250);
				if (duration != 0)
				{
					m_reloadNotification = new MyHudNotification(MySpaceTexts.LargeMissileTurretReloadingNotification, duration, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
					MyHud.Notifications.Add(m_reloadNotification);
					m_nextNotificationTime = num;
				}
			}
			else
			{
				int timeStep = num - m_nextNotificationTime;
				m_reloadNotification.AddAliveTime(timeStep);
				m_nextNotificationTime = num;
			}
		}

		protected void UpdateReloadNotification()
		{
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds > m_nextNotificationTime)
			{
				m_reloadNotification = null;
			}
			if (!m_gunBase.HasEnoughAmmunition() && MySession.Static.SurvivalMode)
			{
				MyHud.Notifications.Remove(m_reloadNotification);
				m_reloadNotification = null;
			}
			else if (!m_turretBase.IsControlledByLocalPlayer)
			{
				if (m_reloadNotification != null)
				{
					MyHud.Notifications.Remove(m_reloadNotification);
					m_reloadNotification = null;
				}
			}
			else if (m_reloadCompletionTime > MySandboxGame.TotalGamePlayTimeInMilliseconds)
			{
				ShowReloadNotification(m_reloadCompletionTime - MySandboxGame.TotalGamePlayTimeInMilliseconds);
			}
		}

		protected void ShowReloadNotification(int duration)
		{
			int num = MySandboxGame.TotalGamePlayTimeInMilliseconds + duration;
			if (m_reloadNotification == null)
			{
				duration = Math.Max(0, duration - 250);
				if (duration != 0)
				{
					m_reloadNotification = new MyHudNotification(MySpaceTexts.LargeMissileTurretReloadingNotification, duration, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
					MyHud.Notifications.Add(m_reloadNotification);
					m_nextNotificationTime = num;
				}
			}
			else
			{
				int timeStep = num - m_nextNotificationTime;
				m_reloadNotification.AddAliveTime(timeStep);
				m_nextNotificationTime = num;
			}
		}

		private void m_entity_OnClose(MyEntity obj)
		{
			if (m_shotSmoke != null)
			{
				MyParticlesManager.RemoveParticleEffect(m_shotSmoke);
				m_shotSmoke = null;
			}
			if (m_muzzleFlash != null)
			{
				MyParticlesManager.RemoveParticleEffect(m_muzzleFlash);
				m_muzzleFlash = null;
			}
		}

		public virtual bool StartShooting()
		{
			m_turretBase.Render.NeedsDrawFromParent = true;
			return true;
		}

		public virtual void StopShooting()
		{
			m_turretBase.Render.NeedsDrawFromParent = false;
			GetWeaponBase().StopShootingSound();
		}

		protected MyLargeTurretBase GetWeaponBase()
		{
			return m_turretBase;
		}

		protected void Shoot(Vector3 muzzlePosition)
		{
			if (m_turretBase.Parent.Physics != null)
			{
				_ = (Vector3)m_entity.WorldMatrix.Forward;
				Vector3 linearVelocity = m_turretBase.Parent.Physics.LinearVelocity;
				GetWeaponBase().RemoveAmmoPerShot();
				m_gunBase.Shoot(linearVelocity, null, this);
			}
		}

		private void DrawCrossHair()
		{
		}

		public bool IsControlledByPlayer()
		{
			return MySession.Static.ControlledEntity == m_turretBase;
		}

		protected void IncreaseSmoke()
		{
			m_smokeToGenerate += 19;
			m_smokeToGenerate = MyUtils.GetClampInt(m_smokeToGenerate, 0, 50);
		}

		protected void DecreaseSmoke()
		{
			m_smokeToGenerate--;
			m_smokeToGenerate = MyUtils.GetClampInt(m_smokeToGenerate, 0, 50);
		}

		public virtual void UpdateAfterSimulation()
		{
			DecreaseSmoke();
			if (!Sync.IsDedicated)
			{
				if ((float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastTimeShoot) > m_gunBase.ReleaseTimeAfterFire)
				{
					m_gunBase.RemoveOldEffects();
				}
				else
				{
					m_gunBase.UpdateEffects();
				}
			}
		}

		public void IsPreviewChanged(bool isPreview)
		{
			if (m_isPreview != isPreview)
			{
<<<<<<< HEAD
				UpdatePreviewState(isPreview);
			}
		}

		private void UpdatePreviewState(bool isPreview)
		{
			m_isPreview = isPreview;
			if (isPreview)
			{
				m_isVisibleOutsidePreview = Entity.Render.Visible;
				Entity.Render.Visible = false;
			}
			else
			{
				Entity.Render.Visible = m_isVisibleOutsidePreview;
			}
		}

=======
				m_isPreview = isPreview;
				if (isPreview)
				{
					m_isVisibleOutsidePreview = Entity.Render.Visible;
					Entity.Render.Visible = false;
				}
				else
				{
					Entity.Render.Visible = m_isVisibleOutsidePreview;
				}
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void RemoveSmoke()
		{
			m_smokeToGenerate = 0;
		}

		public virtual void Close()
		{
<<<<<<< HEAD
			m_gunBase.RemoveAllEffects();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void WorldPositionChanged(ref Matrix renderLocal)
		{
			m_gunBase.WorldMatrix = Entity.PositionComp.WorldMatrixRef;
			m_renderLocal = renderLocal;
		}

		public virtual bool DoesTimingAllowsShooting()
		{
			if (!m_dontTimeOffsetNextShot)
			{
				return m_reloadCompletionTime <= MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
			return true;
		}
	}
}
