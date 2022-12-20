using System.Collections.Generic;
using System.ComponentModel;
using VRage.Network;
using VRage.Render.Particles;
using VRage.Render11.Common;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.Particles
{
	[GenerateActivator]
	internal class MyRenderParticleEffect
	{
		private class VRage_Render11_Particles_MyRenderParticleEffect_003C_003EActor : IActivator, IActivator<MyRenderParticleEffect>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderParticleEffect();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderParticleEffect CreateInstance()
			{
				return new MyRenderParticleEffect();
			}

			MyRenderParticleEffect IActivator<MyRenderParticleEffect>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly List<MyParticleGPUGeneration> m_generations = new List<MyParticleGPUGeneration>();

		private readonly List<MyParticleLight> m_particleLights = new List<MyParticleLight>();

		private MyParticleEffectData m_data;

		private MyParticleEffectState m_state;

		private float m_durationActual;

		public bool CalculateDeltaMatrix;

		public MatrixD DeltaMatrix;

		public uint RenderCounter;

		private const int GRAVITY_UPDATE_DELAY = 200;

		private int m_updateCounter;

		private float m_elapsedTime;

		private float m_loopTime;

		private float m_lastTime;

		private float m_timer;

		private MatrixD m_lastWorldMatrix;

		private float m_startDelay;

		private bool m_genUpdated;

		private bool m_lightsUpdated;

		[Browsable(false)]
		public MatrixD WorldMatrix => m_state.WorldMatrix;

		public bool Autodelete => m_state.Autodelete;

		public string Name { get; private set; }

		public bool IsStopped => m_state.IsStopped;

		public bool IsSimulationPaused => m_state.IsSimulationPaused;

		public bool IsEmittingStopped => m_state.IsEmittingStopped;

		public float UserBirthMultiplier => m_state.UserBirthMultiplier;

		public float UserVelocityMultiplier => m_state.UserVelocityMultiplier;

		public float UserColorIntensityMultiplier => m_state.UserColorIntensityMultiplier;

		public float UserLifeMultiplier => m_state.UserLifeMultiplier;

		public float UserFadeMultiplier => m_state.UserFadeMultiplier;

		public float CameraSoftRadiusMultiplier => m_state.CameraSoftRadiusMultiplier;

		public float SoftParticleDistanceScaleMultiplier => m_state.SoftParticleDistanceScaleMultiplier;

		public Vector4 UserColorMultiplier => m_state.UserColorMultiplier;

		public uint ParentID => m_state.ParentID;

		public float UserRadiusMultiplier => m_state.UserRadiusMultiplier;

		public Vector3 Gravity { get; private set; }

		public float Priority => m_data.Priority;

		private void SetRandomDuration()
		{
			m_durationActual = ((m_data.DurationMax > m_data.DurationMin) ? MyUtils.GetRandomFloat(m_data.DurationMin, m_data.DurationMax) : m_data.DurationMin);
		}

		public float GetScale()
		{
			return m_state.UserScale;
		}

		public float GetElapsedTime()
		{
			return m_elapsedTime;
		}

		public float GetLoopTime()
		{
			return m_loopTime;
		}

		public MatrixD GetDeltaMatrix()
		{
			MatrixD matrix = MatrixD.Invert(m_lastWorldMatrix);
			MatrixD.Multiply(ref matrix, ref m_state.WorldMatrix, out DeltaMatrix);
			return DeltaMatrix;
		}

		public override string ToString()
		{
			return m_data.Name;
		}

		public void Init(uint id, MyParticleEffectData data, string name)
		{
			m_data = data;
			Name = name;
			SetRandomDuration();
			DeltaMatrix = MatrixD.Identity;
			CalculateDeltaMatrix = false;
			RenderCounter = 0u;
			m_updateCounter = 0;
			m_startDelay = 0f;
			m_lightsUpdated = false;
			m_genUpdated = false;
			Recreate();
		}

		private void Recreate()
		{
			Clear(forceInstant: true);
			foreach (MyParticleGPUGenerationData generation in m_data.GetGenerations())
			{
				MyManagers.ParticleEffectsManager.GPUGenerationsPool.AllocateOrCreate(out var item);
				item.Init(this, generation);
				m_generations.Add(item);
			}
			foreach (MyParticleLightData particleLight in m_data.GetParticleLights())
			{
				MyManagers.ParticleEffectsManager.LightsPool.AllocateOrCreate(out var item2);
				item2.Init(this, particleLight);
				m_particleLights.Add(item2);
			}
		}

		public void OnRemove(bool forceInstant, bool outputMessage = true)
		{
			if (outputMessage)
			{
				MyRenderProxy.ParticleEffectRemoved(m_state.ID);
			}
			Clear(forceInstant);
		}

		private void Clear(bool forceInstant)
		{
			bool instant = forceInstant || m_state.InstantStop;
			foreach (MyParticleGPUGeneration generation in m_generations)
			{
				generation.Done(instant);
				MyParticleGPUGeneration item = generation;
				if (generation != null)
				{
					MyManagers.ParticleEffectsManager.GPUGenerationsPool.Deallocate(item);
				}
			}
			m_generations.Clear();
			foreach (MyParticleLight particleLight in m_particleLights)
			{
				particleLight.Done();
				MyManagers.ParticleEffectsManager.LightsPool.Deallocate(particleLight);
			}
			m_particleLights.Clear();
			m_elapsedTime = 0f;
			m_loopTime = 0f;
		}

		public void UpdateState(ref MyParticleEffectState state)
		{
			if (state.TransformDirty)
			{
				m_lastWorldMatrix = m_state.WorldMatrix;
			}
			if (m_state.StopLights != state.StopLights)
			{
				foreach (MyParticleLight particleLight in m_particleLights)
				{
					particleLight.Enabled = !state.StopLights;
				}
			}
			m_state = state;
			if (m_state.Dirty)
			{
				if (!state.IsEmittingStopped && !state.IsStopped)
				{
					m_elapsedTime = state.ElapsedTime;
				}
				m_loopTime = m_elapsedTime;
			}
			if (m_timer == 0f && m_state.IsEmittingStopped)
			{
				m_timer = m_state.Timer;
			}
			else
			{
				m_timer = 0f;
			}
			if (m_state.Dirty)
			{
				foreach (MyParticleGPUGeneration generation in m_generations)
				{
					generation.SetDirty();
				}
			}
			else if (m_state.AnimDirty)
			{
				foreach (MyParticleGPUGeneration generation2 in m_generations)
				{
					generation2.SetAnimDirty();
				}
			}
			else if (m_state.TransformDirty)
			{
				foreach (MyParticleGPUGeneration generation3 in m_generations)
				{
					generation3.SetTransformDirty();
				}
			}
			else
			{
				if (!m_state.UserDirty)
				{
					return;
				}
				foreach (MyParticleGPUGeneration generation4 in m_generations)
				{
					generation4.SetUserDirty();
				}
			}
		}

		public void ClearEffectDataDirty()
		{
			m_data.ClearDirty();
		}

		public bool Update()
		{
			if (!m_data.Enabled)
			{
				return m_state.IsStopped;
			}
			if (WorldMatrix == MatrixD.Zero)
			{
				return true;
			}
			if (m_data.IsDirty())
			{
				Recreate();
			}
			if (ParentID == uint.MaxValue)
			{
				float num = 100f;
				if (m_updateCounter == 0)
				{
					Vector3 vector = MyRender11.CalculateGravityInPoint(m_state.WorldMatrix.Translation);
					float num2 = vector.Length();
					if (num2 > num)
					{
						vector = vector / num2 * num;
					}
					Gravity = vector;
				}
				m_updateCounter++;
				if (m_updateCounter > 200)
				{
					m_updateCounter = 0;
				}
			}
			if (m_state.Velocity.HasValue)
			{
				Vector3D translation = m_state.WorldMatrix.Translation;
				translation += m_state.Velocity.Value * (m_elapsedTime - m_lastTime);
				m_state.WorldMatrix.Translation = translation;
				foreach (MyParticleGPUGeneration generation in m_generations)
				{
					generation.SetTransformDirty();
				}
			}
			if (!m_state.IsSimulationPaused && (m_startDelay <= 0f || !m_lightsUpdated))
			{
				foreach (MyParticleLight particleLight in m_particleLights)
				{
					particleLight.Update();
				}
				m_lightsUpdated = true;
			}
			return UpdateLife();
		}

		private bool UpdateLife()
		{
			m_lastTime = m_elapsedTime;
			if (m_timer > 0f)
			{
				m_timer -= MyCommon.GetLastFrameDelta();
				if (m_timer <= 0f)
				{
					m_timer = 0f;
					return true;
				}
			}
			if (m_state.IsStopped)
			{
				return true;
			}
			m_startDelay -= MyCommon.GetLastFrameDelta();
			if (m_startDelay > 0f)
			{
				return false;
			}
			m_elapsedTime += MyCommon.GetLastFrameDelta();
			m_loopTime += MyCommon.GetLastFrameDelta();
			if (m_data.Loop && m_loopTime >= m_durationActual)
			{
				m_loopTime = 0f;
				SetRandomDuration();
			}
			if (m_durationActual > 0f)
			{
				return m_loopTime > m_durationActual;
			}
			return false;
		}

		public void Draw()
		{
			if (m_startDelay > 0f && m_genUpdated)
			{
				return;
			}
			foreach (MyParticleGPUGeneration generation in m_generations)
			{
				generation.Draw();
			}
			m_genUpdated = true;
		}

		public void StopEmitting()
		{
			if (!m_state.IsEmittingStopped)
			{
				m_state.IsEmittingStopped = true;
				m_state.Timer = m_durationActual;
				m_state.Dirty = true;
				UpdateState(ref m_state);
			}
		}
	}
}
