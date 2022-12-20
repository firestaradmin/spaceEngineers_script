using System;
using System.ComponentModel;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Render.Particles;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Game
{
	[GenerateActivator]
	public class MyParticleEffect
	{
		private class VRage_Game_MyParticleEffect_003C_003EActor : IActivator, IActivator<MyParticleEffect>
		{
			private sealed override object CreateInstance()
			{
				return new MyParticleEffect();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyParticleEffect CreateInstance()
			{
				return new MyParticleEffect();
			}

			MyParticleEffect IActivator<MyParticleEffect>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyParticleEffectData m_data;

		private MyParticleEffectState m_state;

		private float m_elapsedTime;

		private MyTimeSpan m_lastTime;

		public float UserScale
		{
			get
			{
				return m_state.UserScale;
			}
			set
			{
				if (m_state.UserScale != value)
				{
					m_state.UserScale = value;
					m_state.TransformDirty = true;
					ScheduleUpdateInRender();
				}
			}
		}

		public bool Autodelete
		{
			get
			{
				return m_state.Autodelete;
			}
			set
			{
				if (m_state.Autodelete != value)
				{
					m_state.Autodelete = value;
					m_state.Dirty = true;
					ScheduleUpdateInRender();
				}
			}
		}

		[Obsolete("Use UserScale instead.")]
		public float UserEmitterScale
		{
			get
			{
				return UserScale;
			}
			set
			{
				UserScale = value;
			}
		}

		public float DistanceMax => m_data.DistanceMax;

		public float DurationMax => m_data.DurationMax;

		public bool Loop => m_data.Loop;

		public float UserBirthMultiplier
		{
			get
			{
				return m_state.UserBirthMultiplier;
			}
			set
			{
				if (m_state.UserBirthMultiplier != value)
				{
					m_state.UserBirthMultiplier = value;
					m_state.UserDirty = true;
					ScheduleUpdateInRender();
				}
			}
		}

		public float UserVelocityMultiplier
		{
			get
			{
				return m_state.UserVelocityMultiplier;
			}
			set
			{
				if (m_state.UserVelocityMultiplier != value)
				{
					m_state.UserVelocityMultiplier = value;
					m_state.UserDirty = true;
					ScheduleUpdateInRender();
				}
			}
		}

		public float UserColorIntensityMultiplier
		{
			get
			{
				return m_state.UserColorIntensityMultiplier;
			}
			set
			{
				if (m_state.UserColorIntensityMultiplier != value)
				{
					m_state.UserColorIntensityMultiplier = value;
					m_state.UserDirty = true;
					ScheduleUpdateInRender();
				}
			}
		}

		public float UserLifeMultiplier
		{
			get
			{
				return m_state.UserLifeMultiplier;
			}
			set
			{
				if (m_state.UserLifeMultiplier != value)
				{
					m_state.UserLifeMultiplier = value;
					m_state.UserDirty = true;
					ScheduleUpdateInRender();
				}
			}
		}

		public float CameraSoftRadiusMultiplier
		{
			get
			{
				return m_state.CameraSoftRadiusMultiplier;
			}
			set
			{
				if (m_state.CameraSoftRadiusMultiplier != value)
				{
					m_state.CameraSoftRadiusMultiplier = value;
					m_state.Dirty = true;
					ScheduleUpdateInRender();
				}
			}
		}

		public float SoftParticleDistanceScaleMultiplier
		{
			get
			{
				return m_state.SoftParticleDistanceScaleMultiplier;
			}
			set
			{
				if (m_state.SoftParticleDistanceScaleMultiplier != value)
				{
					m_state.SoftParticleDistanceScaleMultiplier = value;
					m_state.Dirty = true;
					ScheduleUpdateInRender();
				}
			}
		}

		public float UserRadiusMultiplier
		{
			get
			{
				return m_state.UserRadiusMultiplier;
			}
			set
			{
				if (m_state.UserRadiusMultiplier != value)
				{
					m_state.UserDirty = true;
					m_state.UserRadiusMultiplier = value;
					ScheduleUpdateInRender();
				}
			}
		}

		public float UserFadeMultiplier
		{
			get
			{
				return m_state.UserFadeMultiplier;
			}
			set
			{
				if (m_state.UserFadeMultiplier != value)
				{
					m_state.UserDirty = true;
					m_state.UserFadeMultiplier = value;
					ScheduleUpdateInRender();
				}
			}
		}

		public Vector4 UserColorMultiplier
		{
			get
			{
				return m_state.UserColorMultiplier;
			}
			set
			{
				if (m_state.UserColorMultiplier != value)
				{
					m_state.UserDirty = true;
					m_state.UserColorMultiplier = value;
					ScheduleUpdateInRender();
				}
			}
		}

		public Vector3 Velocity
		{
			set
			{
				if (!m_state.Velocity.HasValue || m_state.Velocity != value)
				{
					m_state.Velocity = value;
					ScheduleUpdateInRender();
				}
			}
		}

		[Browsable(false)]
		public MatrixD WorldMatrix
		{
			get
			{
				return m_state.WorldMatrix;
			}
			set
			{
				if (!value.EqualsFast(ref m_state.WorldMatrix, 0.001))
				{
					m_state.WorldMatrix = value;
					m_state.TransformDirty = true;
					ScheduleUpdateInRender();
				}
			}
		}

		[Browsable(false)]
		public bool IsStopped => m_state.IsStopped;

		public bool IsEmittingStopped => m_state.IsEmittingStopped;

		public uint Id => m_state.ID;

		public MyParticleEffectData Data => m_data;
<<<<<<< HEAD

		public event Action<MyParticleEffect> OnDelete;

		public void SetDirty()
		{
			m_state.Dirty = true;
		}

		public float GetElapsedTime()
		{
			return m_elapsedTime;
		}

=======

		public event Action<MyParticleEffect> OnDelete;

		public void SetDirty()
		{
			m_state.Dirty = true;
		}

		public float GetElapsedTime()
		{
			return m_elapsedTime;
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void Init(MyParticleEffectData data, ref MatrixD effectMatrix, uint parentId)
		{
			m_lastTime = MyParticlesManager.CurrentTime;
			m_data = data;
			m_state = new MyParticleEffectState
			{
				ID = MyRenderProxy.CreateParticleEffect(m_data, data.Name),
				Dirty = true,
				AnimDirty = true,
				ParentID = parentId,
				UserBirthMultiplier = 1f,
				UserRadiusMultiplier = 1f,
				UserVelocityMultiplier = 1f,
				UserColorIntensityMultiplier = 1f,
				UserLifeMultiplier = 1f,
				CameraSoftRadiusMultiplier = 1f,
				SoftParticleDistanceScaleMultiplier = 1f,
				UserColorMultiplier = Vector4.One,
				UserScale = 1f,
				WorldMatrix = effectMatrix,
				IsStopped = false,
				IsSimulationPaused = false,
				IsEmittingStopped = false,
				InstantStop = false,
				Timer = 0f,
				Autodelete = true,
				UserFadeMultiplier = 1f
			};
			m_elapsedTime = 0f;
			Update();
		}

		/// <summary>
		/// This method stops and deletes effect completely
		/// </summary>
		public void Stop(bool instant = true)
		{
			m_state.IsStopped = true;
			m_state.IsEmittingStopped = true;
			m_state.IsSimulationPaused = false;
			m_state.InstantStop = instant;
			m_state.Dirty = true;
			m_elapsedTime = 0f;
			ScheduleUpdateInRender();
		}

		/// <summary>
		/// This method restores effect
		/// </summary>
		public void Play()
		{
			m_lastTime = MyParticlesManager.CurrentTime;
			m_state.IsStopped = false;
			m_state.IsSimulationPaused = false;
			m_state.IsEmittingStopped = false;
			m_state.Dirty = true;
			m_state.Timer = 0f;
			ScheduleUpdateInRender();
		}

		/// <summary>
		/// This methods freezes effect and particles
		/// </summary>
		public void Pause()
		{
			m_state.IsSimulationPaused = true;
			m_state.IsEmittingStopped = true;
			m_state.Dirty = true;
			ScheduleUpdateInRender();
		}

		/// <summary>
		/// This method stops generating any new particles
		/// </summary>
		public void StopEmitting(float timeout = 0f)
		{
			m_state.IsEmittingStopped = true;
			m_state.Timer = timeout;
			m_state.Dirty = true;
			ScheduleUpdateInRender();
		}

		/// <summary>
		/// This method stops all lights
		/// </summary>
		public void StopLights()
		{
			m_state.StopLights = true;
			ScheduleUpdateInRender();
		}

		[Obsolete("SetTranslation(Vector3D) is deprecated, please use SetTranslation(ref Vector3D) instead.")]
		public void SetTranslation(Vector3D trans)
		{
			SetTranslation(ref trans);
		}

		public void SetTranslation(ref Vector3D trans)
		{
			if (!trans.Equals(m_state.WorldMatrix.Translation, 0.001))
			{
				m_state.WorldMatrix.Translation = trans;
				m_state.TransformDirty = true;
				ScheduleUpdateInRender();
			}
		}

		public void Close()
		{
			OnRemoved();
			MyRenderProxy.RemoveRenderObject(m_state.ID, MyRenderProxy.ObjectType.ParticleEffect);
		}

		public void OnRemoved()
		{
			this.OnDelete.InvokeIfNotNull(this);
			this.OnDelete = null;
		}

		private void ScheduleUpdateInRender()
		{
			MyParticlesManager.ScheduleUpdate(this);
		}

		public void Update()
		{
			if (!m_state.IsStopped && !m_state.IsSimulationPaused)
			{
				float num = (float)(MyParticlesManager.CurrentTime - m_lastTime).Seconds;
				m_lastTime = MyParticlesManager.CurrentTime;
				m_elapsedTime += num;
			}
			MyParticleEffectState data = m_state;
			bool isSimulationPaused = data.IsSimulationPaused;
			bool isEmittingStopped = data.IsEmittingStopped;
			if (isSimulationPaused != data.IsSimulationPaused || isEmittingStopped != data.IsEmittingStopped || m_state.Dirty || m_state.AnimDirty || m_state.TransformDirty || m_state.UserDirty)
			{
				MyRenderProxy.UpdateParticleEffect(ref data);
			}
			m_state.Dirty = (m_state.AnimDirty = (m_state.TransformDirty = (m_state.UserDirty = false)));
		}

		public override string ToString()
		{
			return m_data.Name;
		}

		public string GetName()
		{
			return m_data.Name;
		}

		public void AssertUnload()
		{
		}

		public void Clear()
		{
			OnRemoved();
			m_data = null;
		}
	}
}
