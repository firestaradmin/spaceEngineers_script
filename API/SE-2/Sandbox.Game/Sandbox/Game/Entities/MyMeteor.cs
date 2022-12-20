using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	[MyEntityType(typeof(MyObjectBuilder_Meteor), true)]
	public class MyMeteor : MyEntity, IMyDestroyableObject, IMyDecalProxy, IMyMeteor, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyEventProxy, IMyEventOwner
	{
		public class MyMeteorGameLogic : MyEntityGameLogic
		{
			private enum MeteorStatus
			{
				InAtmosphere,
				InSpace
			}

			public struct ContactProperties
			{
				public HkRigidBody CollidingBody;

				public Vector3D Position;

				public Vector3 Normal;

				public float Direction;

				public float SeparatingVelocity;
			}

			private class Sandbox_Game_Entities_MyMeteor_003C_003EMyMeteorGameLogic_003C_003EActor : IActivator, IActivator<MyMeteorGameLogic>
			{
				private sealed override object CreateInstance()
				{
					return new MyMeteorGameLogic();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyMeteorGameLogic CreateInstance()
				{
					return new MyMeteorGameLogic();
				}

				MyMeteorGameLogic IActivator<MyMeteorGameLogic>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private const int VISIBLE_RANGE_MAX_DISTANCE_SQUARED = 9000000;

			public MyPhysicalInventoryItem Item;

			private float m_integrity = 100f;

			private string[] m_particleEffectNames = new string[2];

			private MyParticleEffect m_dustEffect;

			private int m_timeCreated;

			private Vector3 m_particleVectorForward = Vector3.Zero;

			private Vector3 m_particleVectorUp = Vector3.Zero;

			private MeteorStatus m_meteorStatus = MeteorStatus.InSpace;

			private MyEntity3DSoundEmitter m_soundEmitter;

			private bool m_closeAfterSimulation;

			private MySoundPair m_meteorFly = new MySoundPair("MeteorFly");

			private MySoundPair m_meteorExplosion = new MySoundPair("MeteorExplosion");

			internal MyMeteor MeteorEntity
			{
				get
				{
					if (base.Container == null)
					{
						return null;
					}
					return base.Container.Entity as MyMeteor;
				}
			}

			public MyVoxelMaterialDefinition VoxelMaterial { get; set; }

			private bool InParticleVisibleRange
			{
				get
				{
					MyMeteor meteorEntity = MeteorEntity;
					if (meteorEntity != null)
					{
						return (MySector.MainCamera.Position - meteorEntity.WorldMatrix.Translation).LengthSquared() < 9000000.0;
					}
					string msg = "Error: MyMeteor.Container should not be null!";
					MyLog.Default.WriteLine(msg);
					return false;
				}
			}

			public float Integrity => m_integrity;

			public MyMeteorGameLogic()
			{
				m_soundEmitter = new MyEntity3DSoundEmitter(null);
			}

			public override void Init(MyObjectBuilder_EntityBase objectBuilder)
			{
				MeteorEntity.SyncFlag = true;
				base.Init(objectBuilder);
				MyObjectBuilder_Meteor myObjectBuilder_Meteor = (MyObjectBuilder_Meteor)objectBuilder;
				Item = new MyPhysicalInventoryItem(myObjectBuilder_Meteor.Item);
				m_particleEffectNames[0] = "Meteory_Fire_Atmosphere";
				m_particleEffectNames[1] = "Meteory_Fire_Space";
				InitInternal();
				MeteorEntity.Physics.LinearVelocity = myObjectBuilder_Meteor.LinearVelocity;
				MeteorEntity.Physics.AngularVelocity = myObjectBuilder_Meteor.AngularVelocity;
				m_integrity = myObjectBuilder_Meteor.Integrity;
			}

			private void InitInternal()
			{
				MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(Item.Content);
				MyObjectBuilder_Ore myObjectBuilder_Ore = Item.Content as MyObjectBuilder_Ore;
				string model = physicalItemDefinition.Model;
				float num = 1f;
				VoxelMaterial = null;
				if (myObjectBuilder_Ore != null)
				{
					foreach (MyVoxelMaterialDefinition voxelMaterialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
					{
						if (voxelMaterialDefinition.MinedOre == myObjectBuilder_Ore.SubtypeName)
						{
							VoxelMaterial = voxelMaterialDefinition;
							model = MyDebris.GetAmountBasedDebrisVoxel((float)Item.Amount);
							num = (float)Math.Pow((float)Item.Amount * physicalItemDefinition.Volume / MyDebris.VoxelDebrisModelVolume, 0.33300000429153442);
							break;
						}
					}
				}
				if (num < 0.15f)
				{
					num = 0.15f;
				}
				MyRenderComponentDebrisVoxel obj = MeteorEntity.Render as MyRenderComponentDebrisVoxel;
				obj.VoxelMaterialIndex = VoxelMaterial.Index;
				obj.TexCoordOffset = 5f;
				obj.TexCoordScale = 8f;
<<<<<<< HEAD
				MeteorEntity.Init(new StringBuilder("Meteor"), model, null, null);
				MeteorEntity.PositionComp.Scale = num;
				HkMassProperties value = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(MeteorEntity.PositionComp.LocalVolume.Radius, (float)(4.1887903296220665 * Math.Pow(MeteorEntity.PositionComp.LocalVolume.Radius, 3.0)) * 3.7f);
				HkSphereShape hkSphereShape = new HkSphereShape(MeteorEntity.PositionComp.LocalVolume.Radius);
				if (MeteorEntity.Physics != null)
				{
					MeteorEntity.Physics.Close();
				}
				MeteorEntity.Physics = new MyPhysicsBody(MeteorEntity, RigidBodyFlag.RBF_BULLET);
				MeteorEntity.Physics.ReportAllContacts = true;
				MeteorEntity.GetPhysicsBody().CreateFromCollisionObject(hkSphereShape, Vector3.Zero, MatrixD.Identity, value);
				MeteorEntity.Physics.Enabled = true;
				MeteorEntity.Physics.RigidBody.ContactPointCallbackEnabled = true;
				hkSphereShape.Base.RemoveReference();
				MeteorEntity.Physics.PlayCollisionCueEnabled = true;
=======
				Entity.Init(new StringBuilder("Meteor"), model, null, null);
				Entity.PositionComp.Scale = num;
				HkMassProperties value = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(Entity.PositionComp.LocalVolume.Radius, (float)(4.1887903296220665 * Math.Pow(Entity.PositionComp.LocalVolume.Radius, 3.0)) * 3.7f);
				HkSphereShape hkSphereShape = new HkSphereShape(Entity.PositionComp.LocalVolume.Radius);
				if (Entity.Physics != null)
				{
					Entity.Physics.Close();
				}
				Entity.Physics = new MyPhysicsBody(Entity, RigidBodyFlag.RBF_BULLET);
				Entity.Physics.ReportAllContacts = true;
				Entity.GetPhysicsBody().CreateFromCollisionObject(hkSphereShape, Vector3.Zero, MatrixD.Identity, value);
				Entity.Physics.Enabled = true;
				Entity.Physics.RigidBody.ContactPointCallbackEnabled = true;
				hkSphereShape.Base.RemoveReference();
				Entity.Physics.PlayCollisionCueEnabled = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_timeCreated = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
				StartLoopSound();
			}

			public override void OnAddedToScene()
			{
				base.OnAddedToScene();
				MeteorEntity.GetPhysicsBody().ContactPointCallback += RigidBody_ContactPointCallback;
			}

			public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
			{
				MyObjectBuilder_Meteor myObjectBuilder_Meteor = (MyObjectBuilder_Meteor)base.GetObjectBuilder(copy);
				if (MeteorEntity == null || MeteorEntity.Physics == null)
				{
					myObjectBuilder_Meteor.LinearVelocity = Vector3.One * 10f;
					myObjectBuilder_Meteor.AngularVelocity = Vector3.Zero;
				}
				else
				{
					myObjectBuilder_Meteor.LinearVelocity = MeteorEntity.Physics.LinearVelocity;
					myObjectBuilder_Meteor.AngularVelocity = MeteorEntity.Physics.AngularVelocity;
				}
				if (base.GameLogic != null)
				{
					myObjectBuilder_Meteor.Item = Item.GetObjectBuilder();
					myObjectBuilder_Meteor.Integrity = Integrity;
				}
				return myObjectBuilder_Meteor;
			}

			public override void OnAddedToContainer()
			{
				base.OnAddedToContainer();
				m_soundEmitter.Entity = base.Container.Entity as MyEntity;
			}

			public override void MarkForClose()
			{
				DestroyMeteor();
				base.MarkForClose();
			}

			public override void UpdateBeforeSimulation()
			{
				base.UpdateBeforeSimulation();
			}

			public override void UpdateAfterSimulation()
			{
				if (m_closeAfterSimulation)
				{
					CloseMeteorInternal();
					m_closeAfterSimulation = false;
				}
				base.UpdateAfterSimulation();
			}

			private MatrixD GetParticleWorldMatrix()
			{
				if (m_particleVectorUp != Vector3.Zero)
				{
<<<<<<< HEAD
					return MatrixD.CreateWorld(MeteorEntity.WorldMatrix.Translation, m_particleVectorForward, m_particleVectorUp);
=======
					return MatrixD.CreateWorld(Entity.WorldMatrix.Translation, m_particleVectorForward, m_particleVectorUp);
				}
				return Entity.WorldMatrix;
			}

			private void UpdateParticlePosition()
			{
				if (m_particleVectorUp != Vector3.Zero)
				{
					m_dustEffect.WorldMatrix = GetParticlePosition();
				}
				else
				{
					m_dustEffect.Stop();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				return MeteorEntity.WorldMatrix;
			}

			public override void UpdateBeforeSimulation100()
			{
				base.UpdateBeforeSimulation100();
				if (m_particleVectorUp == Vector3.Zero)
				{
					if (MeteorEntity.Physics.LinearVelocity != Vector3.Zero)
					{
						m_particleVectorUp = -Vector3.Normalize(MeteorEntity.Physics.LinearVelocity);
					}
					else
					{
						m_particleVectorUp = Vector3.Up;
					}
					m_particleVectorUp.CalculatePerpendicularVector(out m_particleVectorForward);
				}
				Vector3D position = MeteorEntity.PositionComp.GetPosition();
				MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
				MeteorStatus meteorStatus = m_meteorStatus;
				if (closestPlanet != null && closestPlanet.HasAtmosphere && closestPlanet.GetAirDensity(position) > 0.5f)
				{
					m_meteorStatus = MeteorStatus.InAtmosphere;
				}
				else
				{
					m_meteorStatus = MeteorStatus.InSpace;
				}
				if (meteorStatus != m_meteorStatus && m_dustEffect != null)
				{
					m_dustEffect.Stop();
					m_dustEffect = null;
				}
				if (m_dustEffect != null && !InParticleVisibleRange)
				{
					m_dustEffect.Stop();
					m_dustEffect = null;
				}
				if (m_dustEffect == null && InParticleVisibleRange)
				{
					MatrixD effectMatrix = MatrixD.Identity;
					if (m_particleVectorUp != Vector3.Zero)
					{
						effectMatrix = MatrixD.CreateWorld(Vector3D.Zero, m_particleVectorForward, m_particleVectorUp);
					}
					Vector3D worldPosition = MeteorEntity.WorldMatrix.Translation;
					if (MyParticlesManager.TryCreateParticleEffect(m_particleEffectNames[(int)m_meteorStatus], ref effectMatrix, ref worldPosition, MeteorEntity.Render.GetRenderObjectID(), out m_dustEffect))
					{
						m_dustEffect.UserScale = MeteorEntity.PositionComp.Scale.Value;
					}
				}
				m_soundEmitter.Update();
				if (Sync.IsServer && (float)(MySandboxGame.TotalGamePlayTimeInMilliseconds - m_timeCreated) > Math.Min(MAX_TRAJECTORY_LENGTH / SPEED, MAX_TRAJECTORY_LENGTH / MeteorEntity.Physics.LinearVelocity.Length()) * 1000f)
				{
					CloseMeteorInternal();
				}
			}

			private void CloseMeteorInternal()
			{
				if (MeteorEntity.Physics != null)
				{
					MeteorEntity.Physics.Enabled = false;
					MeteorEntity.Physics.Deactivate();
				}
				MarkForClose();
			}

			public override void Close()
			{
				if (m_dustEffect != null)
				{
					m_dustEffect.Stop();
					m_dustEffect = null;
				}
				base.Close();
			}

			private void RigidBody_ContactPointCallback(ref MyPhysics.MyContactPointEvent value)
			{
<<<<<<< HEAD
				if (!base.MarkedForClose && MeteorEntity.Physics.Enabled && !m_closeAfterSimulation)
				{
					VRage.ModAPI.IMyEntity other = value.ContactPointEvent.GetOtherEntity(MeteorEntity);
					ContactProperties props = new ContactProperties
					{
						Position = value.Position,
						CollidingBody = ((value.ContactPointEvent.Base.BodyA == MeteorEntity.Physics.RigidBody) ? value.ContactPointEvent.Base.BodyA : value.ContactPointEvent.Base.BodyB),
						Direction = ((!(value.ContactPointEvent.Base.BodyA == MeteorEntity.Physics.RigidBody)) ? 1 : (-1)),
=======
				if (!base.MarkedForClose && Entity.Physics.Enabled && !m_closeAfterSimulation)
				{
					VRage.ModAPI.IMyEntity other = value.ContactPointEvent.GetOtherEntity(Entity);
					ContactProperties props = new ContactProperties
					{
						Position = value.Position,
						CollidingBody = ((value.ContactPointEvent.Base.BodyA == Entity.Physics.RigidBody) ? value.ContactPointEvent.Base.BodyA : value.ContactPointEvent.Base.BodyB),
						Direction = ((!(value.ContactPointEvent.Base.BodyA == Entity.Physics.RigidBody)) ? 1 : (-1)),
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						Normal = value.Normal,
						SeparatingVelocity = value.ContactPointEvent.SeparatingVelocity
					};
					MyEntities.InvokeLater(delegate
					{
						ProcessCollision(props, other);
					});
				}
			}

			private void ProcessCollision(ContactProperties properties, VRage.ModAPI.IMyEntity other)
			{
				if (!MySessionComponentSafeZones.IsActionAllowed(properties.Position, MySafeZoneAction.Damage, 0L, 0uL))
				{
					m_closeAfterSimulation = Sync.IsServer;
					return;
				}
				if (Sync.IsServer)
				{
					if (other is MyCubeGrid)
					{
						MyCubeGrid myCubeGrid = other as MyCubeGrid;
						if (myCubeGrid.BlocksDestructionEnabled)
						{
							DestroyGrid(in properties, myCubeGrid);
						}
					}
					else if (other is MyCharacter)
					{
<<<<<<< HEAD
						(other as MyCharacter).DoDamage(50f * MeteorEntity.PositionComp.Scale.Value, MyDamageType.Environment, updateSync: true, MeteorEntity.EntityId);
					}
					else if (other is MyFloatingObject)
					{
						(other as MyFloatingObject).DoDamage(100f * MeteorEntity.PositionComp.Scale.Value, MyDamageType.Deformation, sync: true, MeteorEntity.EntityId);
=======
						(other as MyCharacter).DoDamage(50f * Entity.PositionComp.Scale.Value, MyDamageType.Environment, updateSync: true, Entity.EntityId);
					}
					else if (other is MyFloatingObject)
					{
						(other as MyFloatingObject).DoDamage(100f * Entity.PositionComp.Scale.Value, MyDamageType.Deformation, sync: true, Entity.EntityId);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else if (other is MyMeteor)
					{
						m_closeAfterSimulation = true;
						(other.GameLogic as MyMeteorGameLogic).m_closeAfterSimulation = true;
					}
					m_closeAfterSimulation = true;
				}
				if (other is MyVoxelBase)
				{
					CreateCrater(in properties, other as MyVoxelBase);
				}
			}

			private void DestroyMeteor()
			{
<<<<<<< HEAD
				MatrixD effectMatrix = GetParticleWorldMatrix();
				Vector3D worldPosition = effectMatrix.Translation;
				if (InParticleVisibleRange && MyParticlesManager.TryCreateParticleEffect("Meteorit_Smoke1AfterHit", ref effectMatrix, ref worldPosition, uint.MaxValue, out var effect))
=======
				if (InParticleVisibleRange && MyParticlesManager.TryCreateParticleEffect("Meteorit_Smoke1AfterHit", GetParticlePosition(), out var effect))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					effect.UserScale = MyUtils.GetRandomFloat(0.8f, 1.2f);
				}
				if (m_dustEffect != null)
				{
					m_dustEffect.StopEmitting(10f);
					m_dustEffect.StopLights();
					m_dustEffect = null;
				}
				PlayExplosionSound();
			}

			private void CreateCrater(in ContactProperties props, MyVoxelBase voxel)
			{
<<<<<<< HEAD
				if ((double)Math.Abs(Vector3.Normalize(-MeteorEntity.WorldMatrix.Forward).Dot(props.Normal)) < 0.1)
				{
					MatrixD effectMatrix = MeteorEntity.WorldMatrix;
					Vector3D worldPosition = effectMatrix.Translation;
					if (InParticleVisibleRange && MyParticlesManager.TryCreateParticleEffect("Meteorit_Smoke1AfterHit", ref effectMatrix, ref worldPosition, uint.MaxValue, out var effect))
=======
				if ((double)Math.Abs(Vector3.Normalize(-Entity.WorldMatrix.Forward).Dot(props.Normal)) < 0.1)
				{
					if (InParticleVisibleRange && MyParticlesManager.TryCreateParticleEffect("Meteorit_Smoke1AfterHit", Entity.WorldMatrix, out var effect))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						effect.UserScale = (float)MeteorEntity.PositionComp.WorldVolume.Radius * 2f;
					}
					m_particleVectorUp = Vector3.Zero;
					m_closeAfterSimulation = Sync.IsServer;
					return;
				}
				if (Sync.IsServer)
				{
<<<<<<< HEAD
					float num = MeteorEntity.PositionComp.Scale.Value * 5f;
					BoundingSphereD sphere = new BoundingSphereD(props.Position, num);
					Vector3 vector = ((!(props.SeparatingVelocity < 0f)) ? Vector3.Normalize(Vector3.Reflect(MeteorEntity.Physics.LinearVelocity, props.Normal)) : Vector3.Normalize(MeteorEntity.Physics.LinearVelocity));
					MyVoxelMaterialDefinition myVoxelMaterialDefinition = VoxelMaterial;
					int num2 = MyDefinitionManager.Static.GetVoxelMaterialDefinitions().Count() * 2;
=======
					float radius = Entity.PositionComp.Scale.Value * 5f;
					BoundingSphereD sphere = new BoundingSphere(props.Position, radius);
					Vector3 vector = ((!(props.SeparatingVelocity < 0f)) ? Vector3.Normalize(Vector3.Reflect(Entity.Physics.LinearVelocity, props.Normal)) : Vector3.Normalize(Entity.Physics.LinearVelocity));
					MyVoxelMaterialDefinition myVoxelMaterialDefinition = VoxelMaterial;
					int num = Enumerable.Count<MyVoxelMaterialDefinition>((IEnumerable<MyVoxelMaterialDefinition>)MyDefinitionManager.Static.GetVoxelMaterialDefinitions()) * 2;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					while (!myVoxelMaterialDefinition.IsRare || !myVoxelMaterialDefinition.SpawnsFromMeteorites || myVoxelMaterialDefinition.MinVersion > MySession.Static.Settings.VoxelGeneratorVersion || myVoxelMaterialDefinition.MaxVersion < MySession.Static.Settings.VoxelGeneratorVersion)
					{
						if (--num2 < 0)
						{
							myVoxelMaterialDefinition = VoxelMaterial;
							break;
						}
						myVoxelMaterialDefinition = Enumerable.ElementAt<MyVoxelMaterialDefinition>((IEnumerable<MyVoxelMaterialDefinition>)MyDefinitionManager.Static.GetVoxelMaterialDefinitions(), MyUtils.GetRandomInt(Enumerable.Count<MyVoxelMaterialDefinition>((IEnumerable<MyVoxelMaterialDefinition>)MyDefinitionManager.Static.GetVoxelMaterialDefinitions()) - 1));
					}
					voxel.CreateVoxelMeteorCrater(sphere.Center, (float)sphere.Radius, -vector, myVoxelMaterialDefinition);
					MyVoxelGenerator.MakeCrater(voxel, sphere, -vector, myVoxelMaterialDefinition);
				}
				m_soundEmitter.Entity = voxel;
				m_soundEmitter.SetPosition(MeteorEntity.PositionComp.GetPosition());
				m_closeAfterSimulation = Sync.IsServer;
			}

			private void DestroyGrid(in ContactProperties value, MyCubeGrid grid)
			{
				m_soundEmitter.Entity = grid;
<<<<<<< HEAD
				m_soundEmitter.SetPosition(MeteorEntity.PositionComp.GetPosition());
=======
				m_soundEmitter.SetPosition(Entity.PositionComp.GetPosition());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				grid.Physics.PerformMeteoriteDeformation(in value);
				m_closeAfterSimulation = Sync.IsServer;
			}

			private void StartLoopSound()
			{
				m_soundEmitter.PlaySingleSound(m_meteorFly);
			}

			private void StopLoopSound()
			{
				m_soundEmitter.StopSound(forced: true);
			}

			private void PlayExplosionSound()
			{
				m_soundEmitter.SetVelocity(Vector3.Zero);
				m_soundEmitter.SetPosition(MeteorEntity.PositionComp.GetPosition());
				m_soundEmitter.PlaySingleSound(m_meteorExplosion, stopPrevious: true);
			}

			public void DoDamage(float damage, MyStringHash damageType, bool sync, MyHitInfo? hitInfo, long attackerId)
			{
				if (sync)
				{
					if (Sync.IsServer)
					{
						MySyncDamage.DoDamageSynced(MeteorEntity, damage, damageType, attackerId);
					}
					return;
				}
				MyDamageInformation info = new MyDamageInformation(isDeformation: false, damage, damageType, attackerId);
				if (MeteorEntity == null)
				{
					return;
				}
				if (MeteorEntity.UseDamageSystem)
				{
					MyDamageSystem.Static.RaiseBeforeDamageApplied(MeteorEntity, ref info);
				}
				m_integrity -= info.Amount;
				if (MeteorEntity.UseDamageSystem)
				{
					MyDamageSystem.Static.RaiseAfterDamageApplied(MeteorEntity, info);
				}
				if (m_integrity <= 0f && Sync.IsServer)
				{
					m_closeAfterSimulation = Sync.IsServer;
					if (MeteorEntity.UseDamageSystem)
					{
						MyDamageSystem.Static.RaiseDestroyed(MeteorEntity, info);
					}
				}
			}

			public void OnDestroy()
			{
			}

			protected virtual HkShape GetPhysicsShape(HkMassProperties massProperties, float mass, float scale)
			{
				Vector3 halfExtents = (MeteorEntity.Render.GetModel().BoundingBox.Max - MeteorEntity.Render.GetModel().BoundingBox.Min) / 2f;
				massProperties = ((VoxelMaterial == null) ? HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(halfExtents, mass) : HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(MeteorEntity.Render.GetModel().BoundingSphere.Radius * scale, mass));
				return MyDebris.Static.GetDebrisShape(MeteorEntity.Render.GetModel(), HkShapeType.ConvexVertices, scale);
			}
		}

		private class Sandbox_Game_Entities_MyMeteor_003C_003EActor : IActivator, IActivator<MyMeteor>
		{
			private sealed override object CreateInstance()
			{
				return new MyMeteor();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMeteor CreateInstance()
			{
				return new MyMeteor();
			}

			MyMeteor IActivator<MyMeteor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly float MAX_TRAJECTORY_LENGTH = 10000f;

		private static readonly float SPEED = 90f;

		private MyMeteorGameLogic m_logic;

		private bool m_hasModifiableDamage;

		public new MyMeteorGameLogic GameLogic
		{
			get
			{
				return m_logic;
			}
			set
			{
				base.GameLogic = value;
			}
		}

		public override bool IsCCDForProjectiles => true;

		public float Integrity => GameLogic.Integrity;

		public bool UseDamageSystem => m_hasModifiableDamage;

		public MyMeteor()
		{
			base.Components.ComponentAdded += Components_ComponentAdded;
			GameLogic = new MyMeteorGameLogic();
			base.Render = new MyRenderComponentDebrisVoxel();
		}

		private void Components_ComponentAdded(Type arg1, MyComponentBase arg2)
		{
			if (arg1 == typeof(MyGameLogicComponent))
			{
				m_logic = arg2 as MyMeteorGameLogic;
			}
		}

		public static MyEntity SpawnRandom(Vector3D position, Vector3 direction)
		{
			string materialName = GetMaterialName();
			MyPhysicalInventoryItem item = new MyPhysicalInventoryItem(500 * (MyFixedPoint)MyUtils.GetRandomFloat(1f, 3f), MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>(materialName));
			return Spawn(ref item, position, direction * SPEED);
		}

		private static string GetMaterialName()
		{
			string text = "Stone";
			bool flag = false;
			MyVoxelMaterialDefinition myVoxelMaterialDefinition = null;
			foreach (MyVoxelMaterialDefinition voxelMaterialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
			{
				if (voxelMaterialDefinition.MinedOre == text)
				{
					flag = true;
					break;
				}
				myVoxelMaterialDefinition = voxelMaterialDefinition;
			}
			if (!flag && myVoxelMaterialDefinition != null)
			{
				text = myVoxelMaterialDefinition.MinedOre;
			}
			return text;
		}

		public static MyEntity Spawn(ref MyPhysicalInventoryItem item, Vector3D position, Vector3 speed)
		{
			return MyEntities.CreateFromObjectBuilderParallel(PrepareBuilder(ref item), addToScene: true, delegate(MyEntity x)
			{
				SetSpawnSettings(x, position, speed);
			});
		}

		private static void SetSpawnSettings(MyEntity meteorEntity, Vector3D position, Vector3 speed)
		{
			Vector3 vector = -MySector.DirectionToSunNormalized;
			Vector3 randomVector3Normalized = MyUtils.GetRandomVector3Normalized();
			while (vector == randomVector3Normalized)
			{
				randomVector3Normalized = MyUtils.GetRandomVector3Normalized();
			}
			randomVector3Normalized = Vector3.Cross(Vector3.Cross(vector, randomVector3Normalized), vector);
			meteorEntity.WorldMatrix = MatrixD.CreateWorld(position, vector, randomVector3Normalized);
			meteorEntity.Physics.RigidBody.MaxLinearVelocity = 500f;
			meteorEntity.Physics.LinearVelocity = speed;
			meteorEntity.Physics.AngularVelocity = MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(1.5f, 3f);
		}

		private static MyObjectBuilder_Meteor PrepareBuilder(ref MyPhysicalInventoryItem item)
		{
			MyObjectBuilder_Meteor myObjectBuilder_Meteor = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Meteor>();
			myObjectBuilder_Meteor.Item = item.GetObjectBuilder();
			myObjectBuilder_Meteor.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
			return myObjectBuilder_Meteor;
		}

		public void OnDestroy()
		{
			GameLogic.OnDestroy();
		}

<<<<<<< HEAD
		public bool DoDamage(float damage, MyStringHash damageType, bool sync, MyHitInfo? hitInfo, long attackerId, long realHitEntityId = 0L, bool shouldDetonateAmmo = true)
=======
		public bool DoDamage(float damage, MyStringHash damageType, bool sync, MyHitInfo? hitInfo, long attackerId, long realHitEntityId = 0L)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			GameLogic.DoDamage(damage, damageType, sync, hitInfo, attackerId);
			return true;
		}

<<<<<<< HEAD
		void IMyDecalProxy.AddDecals(ref MyHitInfo hitInfo, MyStringHash source, Vector3 forwardDirection, object customdata, IMyDecalHandler decalHandler, MyStringHash physicalMaterial, MyStringHash voxelMaterial, bool isTrail, MyDecalFlags flags, int aliveUntil, List<uint> ids)
=======
		void IMyDecalProxy.AddDecals(ref MyHitInfo hitInfo, MyStringHash source, Vector3 forwardDirection, object customdata, IMyDecalHandler decalHandler, MyStringHash physicalMaterial, MyStringHash voxelMaterial, bool isTrail)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			return GameLogic.GetObjectBuilder();
		}
	}
}
