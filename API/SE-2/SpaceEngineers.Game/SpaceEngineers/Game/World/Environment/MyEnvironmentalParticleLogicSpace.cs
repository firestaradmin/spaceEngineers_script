using System;
using System.Collections.Generic;
using Sandbox;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Library.Utils;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.World.Environment
{
	[MyEnvironmentalParticleLogicType(typeof(MyObjectBuilder_EnvironmentalParticleLogicSpace), true)]
	internal class MyEnvironmentalParticleLogicSpace : MyEnvironmentalParticleLogic
	{
		private int m_lastParticleSpawn;

		private float m_particlesLeftToSpawn;

		private bool m_isPlanetary;

<<<<<<< HEAD
		private MyRandom m_random = new MyRandom(256);

		public MyEntity ControlledEntity { get; private set; }

		public Vector3 ControlledVelocity { get; private set; }

=======
		public MyEntity ControlledEntity { get; private set; }

		public Vector3 ControlledVelocity { get; private set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool ShouldDrawParticles { get; private set; }

		public override void Init(MyObjectBuilder_EnvironmentalParticleLogic builder)
		{
			base.Init(builder);
			_ = builder is MyObjectBuilder_EnvironmentalParticleLogicSpace;
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			ShouldDrawParticles = false;
			try
			{
				ControlledEntity = GetControlledEntity();
				if (ControlledEntity == null)
				{
					return;
				}
				ControlledVelocity = ControlledEntity.Physics?.LinearVelocity ?? Vector3.Zero;
				if (ControlledVelocity.LengthSquared() < 100f || IsInGridAABB())
				{
					return;
				}
				ShouldDrawParticles = true;
				float particleSpawnDistance = base.ParticleSpawnDistance;
				double num = Math.PI / 2.0;
				float num2 = 1f;
				Vector3 value = ControlledVelocity - 8.5f * Vector3.Normalize(ControlledVelocity);
				float num3 = 4f * particleSpawnDistance * particleSpawnDistance * num2;
				m_isPlanetary = IsNearPlanet();
				if (m_isPlanetary && !MyFakes.ENABLE_STARDUST_ON_PLANET)
<<<<<<< HEAD
				{
					return;
				}
				m_particlesLeftToSpawn += (0.25f + m_random.NextFloat() * 1.25f) * value.Length() * num3 * base.ParticleDensity * 16f;
				if (m_particlesLeftToSpawn < 1f)
				{
					return;
				}
				double num4 = num / 2.0;
				double num5 = num4 + num;
				double num6 = num4 + (double)m_random.NextFloat() * (num5 - num4);
				double num7 = num4 + (double)m_random.NextFloat() * (num5 - num4);
=======
				{
					return;
				}
				m_particlesLeftToSpawn += (0.25f + MyRandom.Instance.NextFloat() * 1.25f) * value.Length() * num3 * base.ParticleDensity * 16f;
				if (m_particlesLeftToSpawn < 1f)
				{
					return;
				}
				double num4 = num / 2.0;
				double num5 = num4 + num;
				double num6 = num4 + (double)MyRandom.Instance.NextFloat() * (num5 - num4);
				double num7 = num4 + (double)MyRandom.Instance.NextFloat() * (num5 - num4);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				float num8 = 6f;
				while (m_particlesLeftToSpawn-- >= 1f)
				{
					float num9 = (float)Math.PI / 180f;
					if (Math.Abs(num6 - Math.PI / 2.0) < (double)(num8 * num9) && Math.Abs(num7 - Math.PI / 2.0) < (double)(num8 * num9))
<<<<<<< HEAD
					{
						num6 += (double)((float)Math.Sign(m_random.NextFloat()) * num8 * num9);
						num7 += (double)((float)Math.Sign(m_random.NextFloat()) * num8 * num9);
					}
					float num10 = (float)Math.Sin(num7);
					float num11 = (float)Math.Cos(num7);
					float num12 = (float)Math.Sin(num6);
					float num13 = (float)Math.Cos(num6);
					Vector3 upVector = MySector.MainCamera.UpVector;
					Vector3 vector = Vector3.Normalize(value);
					Vector3 vector2 = Vector3.Cross(vector, -upVector);
					if (Vector3.IsZero(vector2))
					{
						vector2 = Vector3.CalculatePerpendicularVector(vector);
					}
					else
					{
						vector2.Normalize();
					}
=======
					{
						num6 += (double)((float)Math.Sign(MyRandom.Instance.NextFloat()) * num8 * num9);
						num7 += (double)((float)Math.Sign(MyRandom.Instance.NextFloat()) * num8 * num9);
					}
					float num10 = (float)Math.Sin(num7);
					float num11 = (float)Math.Cos(num7);
					float num12 = (float)Math.Sin(num6);
					float num13 = (float)Math.Cos(num6);
					Vector3 upVector = MySector.MainCamera.UpVector;
					Vector3 vector = Vector3.Normalize(value);
					Vector3 vector2 = Vector3.Cross(vector, -upVector);
					if (Vector3.IsZero(vector2))
					{
						vector2 = Vector3.CalculatePerpendicularVector(vector);
					}
					else
					{
						vector2.Normalize();
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					Vector3 vector3 = Vector3.Cross(vector, vector2);
					Vector3 position = MySector.MainCamera.Position + particleSpawnDistance * (vector3 * num11 + vector2 * num10 * num13 + vector * num10 * num12);
					Spawn(position);
					m_lastParticleSpawn = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				}
			}
			finally
			{
			}
		}

		public override void UpdateAfterSimulation()
		{
			if (!ShouldDrawParticles)
			{
				DeactivateAll();
				m_particlesLeftToSpawn = 0f;
			}
			base.UpdateAfterSimulation();
		}

		public override void Draw()
		{
			base.Draw();
			if (!ShouldDrawParticles)
			{
				return;
			}
			Vector3 directionNormalized = -Vector3.Normalize(ControlledVelocity);
			float num = 0.025f;
			float num2 = (float)MathHelper.Clamp(ControlledVelocity.Length() / 50f, 0.0, 1.0);
			float num3 = 1f;
			float num4 = 1f;
			if (m_isPlanetary)
			{
				num3 = 1.5f;
				num4 = 3f;
			}
			foreach (MyEnvironmentalParticle activeParticle in m_activeParticles)
			{
				if (activeParticle.Active)
				{
					if (m_isPlanetary)
					{
						MyTransparentGeometry.AddLineBillboard(activeParticle.MaterialPlanet, activeParticle.ColorPlanet, activeParticle.Position, directionNormalized, num2 * num4, num * num3, MyBillboard.BlendTypeEnum.LDR);
					}
					else
					{
						MyTransparentGeometry.AddLineBillboard(activeParticle.Material, activeParticle.Color, activeParticle.Position, directionNormalized, num2 * num4, num * num3, MyBillboard.BlendTypeEnum.LDR);
					}
				}
			}
		}

		private bool IsInGridAABB()
		{
			bool result = false;
			BoundingSphereD boundingSphere = new BoundingSphereD(MySector.MainCamera.Position, 0.10000000149011612);
			List<MyEntity> list = null;
			try
			{
				list = MyEntities.GetEntitiesInSphere(ref boundingSphere);
				foreach (MyEntity item in list)
				{
					MyCubeGrid myCubeGrid;
					if ((myCubeGrid = item as MyCubeGrid) != null && myCubeGrid.GridSizeEnum != MyCubeSize.Small)
					{
						return true;
					}
				}
				return result;
			}
			finally
			{
				list?.Clear();
			}
		}

		private MyEntity GetControlledEntity()
		{
			MyEntity myEntity = MySession.Static.ControlledEntity as MyEntity;
			if (myEntity == null || MySession.Static.IsCameraUserControlledSpectator())
			{
				return null;
			}
			MyRemoteControl myRemoteControl;
			MyCockpit myCockpit;
			if ((myRemoteControl = myEntity as MyRemoteControl) != null)
			{
				myEntity = myRemoteControl.GetTopMostParent();
			}
			else if ((myCockpit = myEntity as MyCockpit) != null)
			{
				myEntity = myCockpit.GetTopMostParent();
			}
			return myEntity;
		}

		private bool IsNearPlanet()
		{
			if (ControlledEntity == null)
			{
				return false;
			}
			return !Vector3.IsZero(MyGravityProviderSystem.CalculateNaturalGravityInPoint(ControlledEntity.PositionComp.GetPosition()));
		}
	}
}
