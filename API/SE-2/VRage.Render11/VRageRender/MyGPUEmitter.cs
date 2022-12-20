using System;
using VRage.Library.Utils;
using VRage.Render.Particles;
using VRage.Render.Scene;
using VRageMath;

namespace VRageRender
{
	internal class MyGPUEmitter : IComparable
	{
		internal int BufferIndex;

		internal MyGPUEmitterData EmitterData;

		internal bool MotionInterpolation = true;

		private string m_debugName;

		private float m_particlesEmittedFraction;

		private MyTimeSpan m_dieAt;

		private bool m_justAdded;

		private Vector3D m_lastWorldPosition;

		private MyGravity m_gravity;

		private bool m_farAway;

		private bool m_dying;

		private bool m_first;

<<<<<<< HEAD
		private bool m_isLastRotationStored;

		private Matrix3x3 m_lastRotation;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyGPUEmitter(string debugName)
		{
			m_debugName = debugName;
			BufferIndex = -1;
			m_dieAt = MyTimeSpan.MaxValue;
			m_justAdded = true;
			m_first = true;
		}

		public void UpdateData()
		{
			if (m_farAway || m_dying)
			{
				EmitterData.Data.Flags |= GPUEmitterFlags.FreezeEmit;
				EmitterData.ParticlesPerFrame = 0f;
			}
			if (m_first)
			{
				UpdateLastPosition();
				m_first = false;
			}
		}

		public void Remove()
		{
			MyGPUEmitters.FreeBufferIndex(BufferIndex);
			BufferIndex = -1;
			if (!m_farAway)
			{
				MyGPUEmitters.Remove(this);
			}
		}

		public void TryRemove(bool instant, bool distance = false)
		{
			if (!distance || !m_dying)
			{
				m_farAway = distance;
				m_dying = !distance;
				if (BufferIndex == -1 && !distance)
				{
					Remove();
					return;
				}
				if (instant)
				{
					EmitterData.Data.Flags |= GPUEmitterFlags.Dead;
					return;
				}
				EmitterData.Data.Flags |= GPUEmitterFlags.FreezeEmit;
				m_dieAt = MyCommon.FrameTime + MyTimeSpan.FromSeconds(EmitterData.Data.ParticleLifeSpan);
			}
		}

		private void UpdateGravity(out Vector3 gravityVec)
		{
			if (Math.Abs(EmitterData.GravityFactor) > 0.001f)
			{
				uint parentID = EmitterData.ParentID;
				if (m_gravity == null || parentID != m_gravity.ParentID)
				{
					m_gravity = MyGPUEmitters.GetGravity(parentID);
				}
				m_gravity.UpdateGravity(ref m_lastWorldPosition, out gravityVec);
				gravityVec *= EmitterData.GravityFactor;
			}
			else
			{
				gravityVec = Vector3.Zero;
			}
		}

		public int CompareTo(object obj)
		{
			MyGPUEmitter myGPUEmitter = obj as MyGPUEmitter;
			double num = Vector3D.DistanceSquared(myGPUEmitter.m_lastWorldPosition, MyGPUEmitters.CameraPosition) * (double)myGPUEmitter.EmitterData.PrioritySqr;
			if (!(Vector3D.DistanceSquared(m_lastWorldPosition, MyGPUEmitters.CameraPosition) * (double)EmitterData.PrioritySqr < num))
			{
				return 1;
			}
			return -1;
		}

		public void Update(ref MyGPUEmitterLayoutData data)
		{
			if (MyCommon.FrameTime > m_dieAt)
			{
				EmitterData.Data.Flags |= GPUEmitterFlags.Dead;
			}
			if (BufferIndex != -1)
			{
				if ((EmitterData.Data.Flags & GPUEmitterFlags.FreezeEmit) == 0)
				{
					float num = MyCommon.GetLastFrameDelta() * MyGPUEmitters.ParticleCountMultiplier * EmitterData.ParticlesPerSecond + m_particlesEmittedFraction;
					EmitterData.Data.NumParticlesToEmitThisFrame = (int)num + (int)EmitterData.ParticlesPerFrame;
					m_particlesEmittedFraction = num - (float)(int)num;
					EmitterData.ParticlesPerFrame -= (int)EmitterData.ParticlesPerFrame;
				}
				else
				{
					EmitterData.Data.NumParticlesToEmitThisFrame = 0;
				}
				data = EmitterData.Data;
				uint textureIndex = MyGPUEmitters.GetTextureIndex(EmitterData.AtlasTexture);
				data.TextureIndex1 |= textureIndex;
				Vector3D position = EmitterData.WorldPosition;
				if (EmitterData.ParentID != uint.MaxValue)
				{
					MyActor myActor = MyIDTracker<MyActor>.FindByID(EmitterData.ParentID);
					if (myActor != null)
					{
						MatrixD matrix = myActor.WorldMatrix;
						Vector3D.Transform(ref position, ref matrix, out position);
						Matrix3x3 matrix2 = matrix.Rotation;
						matrix2.Transpose();
						Matrix3x3.Multiply(ref matrix2, ref data.Rotation, out data.Rotation);
<<<<<<< HEAD
						m_isLastRotationStored = true;
						m_lastRotation = matrix2;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else
					{
						position = m_lastWorldPosition;
<<<<<<< HEAD
						if (m_isLastRotationStored)
						{
							Matrix3x3.Multiply(ref m_lastRotation, ref data.Rotation, out data.Rotation);
						}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				Vector3 vector = position - MyRender11.Environment.Matrices.CameraPosition;
				if (EmitterData.CameraBias != 0f)
				{
					Vector3 vector2 = vector;
					vector2.Normalize();
					vector -= vector2 * EmitterData.CameraBias;
				}
				data.Position = vector;
				Vector3 positionDelta = Vector3.Zero;
				if (!m_justAdded && MotionInterpolation)
				{
					positionDelta = position - m_lastWorldPosition;
				}
				data.PositionDelta = positionDelta;
				m_justAdded = false;
				m_lastWorldPosition = position;
				UpdateGravity(out data.Gravity);
				if (!m_farAway && vector.LengthSquared() > EmitterData.DistanceMaxSqr)
				{
					EmitterData.Data.Flags |= GPUEmitterFlags.FreezeEmit;
					TryRemove(instant: false, distance: true);
				}
			}
			else
			{
				if (!m_farAway)
				{
					return;
				}
				Vector3D position2 = EmitterData.WorldPosition;
				if (EmitterData.ParentID != uint.MaxValue)
				{
					MyActor myActor2 = MyIDTracker<MyActor>.FindByID(EmitterData.ParentID);
					if (myActor2 != null)
					{
						MatrixD matrix3 = myActor2.WorldMatrix;
						Vector3D.Transform(ref position2, ref matrix3, out position2);
					}
				}
				if (((Vector3)(position2 - MyRender11.Environment.Matrices.CameraPosition)).LengthSquared() < EmitterData.DistanceMaxSqr * 0.9f)
				{
					EmitterData.Data.Flags &= ~GPUEmitterFlags.FreezeEmit;
					EmitterData.Data.Flags &= ~GPUEmitterFlags.Dead;
					m_dieAt = MyTimeSpan.MaxValue;
					m_farAway = false;
				}
			}
		}

		public void UpdateLastPosition()
		{
			Vector3D position = EmitterData.WorldPosition;
			if (EmitterData.ParentID != uint.MaxValue)
			{
				MyActor myActor = MyIDTracker<MyActor>.FindByID(EmitterData.ParentID);
				if (myActor != null)
				{
					MatrixD matrix = myActor.WorldMatrix;
					Vector3D.Transform(ref position, ref matrix, out position);
				}
			}
			m_lastWorldPosition = position;
		}
	}
}
