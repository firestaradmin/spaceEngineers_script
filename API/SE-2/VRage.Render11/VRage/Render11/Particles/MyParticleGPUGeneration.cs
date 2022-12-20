using System;
using VRage.Network;
using VRage.Render.Particles;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Particles
{
	[GenerateActivator]
	internal class MyParticleGPUGeneration : IComparable
	{
		private class VRage_Render11_Particles_MyParticleGPUGeneration_003C_003EActor : IActivator, IActivator<MyParticleGPUGeneration>
		{
			private sealed override object CreateInstance()
			{
				return new MyParticleGPUGeneration();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyParticleGPUGeneration CreateInstance()
			{
				return new MyParticleGPUGeneration();
			}

			MyParticleGPUGeneration IActivator<MyParticleGPUGeneration>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private string m_name;

		private MyParticleGPUGenerationData m_data;

		private MyRenderParticleEffect m_owner;

		private MyGPUEmitter m_emitter;

		private bool m_dirty;

		private bool m_animDirty;

		private bool m_transformDirty;

		private bool m_userDirty;

		private float m_lastTime;

		private bool m_animatedTimeValues;

		private float m_lastFramePPS;

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public void Init(MyRenderParticleEffect owner, MyParticleGPUGenerationData data)
		{
			m_owner = owner;
			m_name = data.Name;
			m_data = data;
			m_lastTime = 0f;
			m_dirty = true;
			m_emitter = MyGPUEmitters.Create(m_data.Name);
		}

		public void Done(bool instant)
		{
			Stop(instant);
		}

		private void Stop(bool instant)
		{
			m_owner = null;
			m_emitter.TryRemove(instant);
		}

		private void CalculateWorldMatrix(out MatrixD matrix)
		{
			Vector3 normal = (Vector3)m_data.Offset * m_owner.GetScale();
			matrix = m_owner.WorldMatrix;
			Vector3.TransformNormal(ref normal, ref matrix, out normal);
			matrix.Translation += normal;
		}

		private static uint GenerateTextureIndex(ref MyGPUEmitterData emitterData)
		{
			return (uint)(emitterData.AtlasFrameOffset | (emitterData.AtlasDimension.X << 18) | (emitterData.AtlasDimension.Y << 12));
		}

		private void FillData(ref MyGPUEmitterData emitterData)
		{
			if (m_owner.IsSimulationPaused)
			{
				emitterData.Data.Flags |= GPUEmitterFlags.FreezeSimulate;
			}
			else
			{
				emitterData.Data.Flags &= ~GPUEmitterFlags.FreezeSimulate;
			}
			CalculateWorldMatrix(out var matrix);
			emitterData.ParentID = m_owner.ParentID;
			emitterData.Data.Rotation = matrix.Rotation;
			emitterData.Data.Rotation.Transpose();
			emitterData.WorldPosition = matrix.Translation;
			emitterData.Data.Scale = m_owner.GetScale();
			m_data.FillData(m_owner.GetElapsedTime(), ref emitterData);
			emitterData.ParticlesPerSecond = GetParticlesPerSecond();
			emitterData.ParticlesPerFrame += GetParticlesPerFrame();
			Vector4 userColorMultiplier = m_owner.UserColorMultiplier;
			m_emitter.EmitterData.Data.Color0 *= userColorMultiplier;
			m_emitter.EmitterData.Data.Color1 *= userColorMultiplier;
			m_emitter.EmitterData.Data.Color2 *= userColorMultiplier;
			m_emitter.EmitterData.Data.Color3 *= userColorMultiplier;
			emitterData.CameraBias *= m_owner.GetScale();
			emitterData.Data.ParticleSize0 *= m_owner.UserRadiusMultiplier;
			emitterData.Data.ParticleSize1 *= m_owner.UserRadiusMultiplier;
			emitterData.Data.ParticleSize2 *= m_owner.UserRadiusMultiplier;
			emitterData.Data.ParticleSize3 *= m_owner.UserRadiusMultiplier;
			emitterData.Data.UserRadiusMultiplier = m_owner.UserRadiusMultiplier;
			emitterData.Data.UserVelocityMultiplier = m_owner.UserVelocityMultiplier;
			emitterData.Data.UserColorIntensityMultiplier = m_owner.UserColorIntensityMultiplier;
			emitterData.Data.UserLifeMultiplier = m_owner.UserLifeMultiplier;
			emitterData.Data.UserFadeMultiplier = m_owner.UserFadeMultiplier;
		}

		private float GetParticlesPerSecond()
		{
			if (m_data.Enabled.GetValue() && !m_owner.IsEmittingStopped)
			{
				m_data.ParticlesPerSecond.GetInterpolatedValue(m_owner.GetElapsedTime(), out var value);
				return value * m_owner.UserBirthMultiplier;
			}
			return 0f;
		}

		private float GetParticlesPerFrame()
		{
			if (m_data.ParticlesPerFrame.GetKeysCount() > 0 && m_data.Enabled.GetValue() && !m_owner.IsEmittingStopped)
			{
				m_data.ParticlesPerFrame.GetNextValue(m_lastTime, out var nextValue, out var nextTime, out var _);
				if (nextTime < m_lastTime || nextTime >= m_owner.GetLoopTime())
				{
					return 0f;
				}
				return nextValue * m_owner.UserBirthMultiplier;
			}
			return 0f;
		}

		public int CompareTo(object compareToObject)
		{
			return 0;
		}

		public MyRenderParticleEffect GetOwner()
		{
			return m_owner;
		}

		public void SetDirty()
		{
			m_dirty = true;
		}

		public void SetAnimDirty()
		{
			m_animDirty = true;
		}

		public void SetTransformDirty()
		{
			m_transformDirty = true;
		}

		public void SetUserDirty()
		{
			m_userDirty = true;
		}

		public void Draw()
		{
			if (m_dirty)
			{
				string atlasTexture = m_emitter.EmitterData.AtlasTexture;
				m_animatedTimeValues = m_data.FillDataComplete(ref m_emitter.EmitterData);
				m_emitter.MotionInterpolation = m_data.MotionInterpolation;
				m_emitter.EmitterData.Data.TextureIndex1 = GenerateTextureIndex(ref m_emitter.EmitterData);
				m_emitter.EmitterData.Data.CameraSoftRadius *= m_owner.CameraSoftRadiusMultiplier;
				m_emitter.EmitterData.Data.SoftParticleDistanceScale *= m_owner.SoftParticleDistanceScaleMultiplier;
				m_emitter.EmitterData.PrioritySqr = m_owner.Priority * m_owner.Priority;
				FillData(ref m_emitter.EmitterData);
				m_lastFramePPS = m_emitter.EmitterData.ParticlesPerSecond;
				MyGPUEmitters.UpdateTexture(m_emitter.EmitterData.AtlasTexture, atlasTexture);
				m_emitter.UpdateData();
			}
			else if (m_animatedTimeValues || m_animDirty)
			{
				FillData(ref m_emitter.EmitterData);
				m_lastFramePPS = m_emitter.EmitterData.ParticlesPerSecond;
				m_emitter.UpdateData();
				m_animDirty = false;
			}
			else if (m_transformDirty)
			{
				float particlesPerSecond = GetParticlesPerSecond();
				float particlesPerFrame = GetParticlesPerFrame();
				m_lastFramePPS = particlesPerSecond;
				CalculateWorldMatrix(out var matrix);
				m_emitter.EmitterData.WorldPosition = matrix.Translation;
				m_emitter.EmitterData.Data.Rotation = matrix.Rotation;
				m_emitter.EmitterData.Data.Rotation.Transpose();
				m_emitter.EmitterData.Data.Scale = m_owner.GetScale();
				m_emitter.EmitterData.ParticlesPerSecond = particlesPerSecond;
				m_emitter.EmitterData.ParticlesPerFrame += particlesPerFrame;
				m_emitter.UpdateData();
			}
			else if (m_userDirty)
			{
				m_emitter.EmitterData.ParticlesPerSecond = GetParticlesPerSecond();
				m_emitter.EmitterData.ParticlesPerFrame += GetParticlesPerFrame();
				Vector4 userColorMultiplier = m_owner.UserColorMultiplier;
				m_emitter.EmitterData.Data.Color0 *= userColorMultiplier;
				m_emitter.EmitterData.Data.Color1 *= userColorMultiplier;
				m_emitter.EmitterData.Data.Color2 *= userColorMultiplier;
				m_emitter.EmitterData.Data.Color3 *= userColorMultiplier;
				m_emitter.EmitterData.Data.UserRadiusMultiplier = m_owner.UserRadiusMultiplier;
				m_emitter.EmitterData.Data.UserVelocityMultiplier = m_owner.UserVelocityMultiplier;
				m_emitter.EmitterData.Data.UserColorIntensityMultiplier = m_owner.UserColorIntensityMultiplier;
				m_emitter.EmitterData.Data.UserLifeMultiplier = m_owner.UserLifeMultiplier;
				m_emitter.EmitterData.Data.UserFadeMultiplier = m_owner.UserFadeMultiplier;
			}
			else if (m_data.ParticlesPerSecond.GetKeysCount() > 1 || m_data.ParticlesPerFrame.GetKeysCount() > 0)
			{
				float particlesPerSecond2 = GetParticlesPerSecond();
				float particlesPerFrame2 = GetParticlesPerFrame();
				if (Math.Abs(m_lastFramePPS - particlesPerSecond2) > 0.5f || particlesPerFrame2 > 0f)
				{
					m_lastFramePPS = particlesPerSecond2;
					m_emitter.EmitterData.ParticlesPerSecond = particlesPerSecond2;
					m_emitter.EmitterData.ParticlesPerFrame += particlesPerFrame2;
					m_emitter.UpdateData();
				}
			}
			m_dirty = (m_animDirty = (m_transformDirty = (m_userDirty = false)));
			m_lastTime = m_owner.GetLoopTime();
		}
	}
}
