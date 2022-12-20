using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Wheel))]
	public class MyWheel : MyMotorRotor, Sandbox.ModAPI.IMyWheel, Sandbox.ModAPI.IMyMotorRotor, Sandbox.ModAPI.IMyAttachableTopBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock, Sandbox.ModAPI.Ingame.IMyMotorRotor, Sandbox.ModAPI.Ingame.IMyWheel, IMyTrackTrails
	{
		[Serializable]
		protected struct TrailContactProperties
		{
			protected class Sandbox_Game_Entities_Blocks_MyWheel_003C_003ETrailContactProperties_003C_003EContactEntityId_003C_003EAccessor : IMemberAccessor<TrailContactProperties, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TrailContactProperties owner, in long value)
				{
					owner.ContactEntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TrailContactProperties owner, out long value)
				{
					value = owner.ContactEntityId;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyWheel_003C_003ETrailContactProperties_003C_003EContactPosition_003C_003EAccessor : IMemberAccessor<TrailContactProperties, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TrailContactProperties owner, in Vector3 value)
				{
					owner.ContactPosition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TrailContactProperties owner, out Vector3 value)
				{
					value = owner.ContactPosition;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyWheel_003C_003ETrailContactProperties_003C_003EContactNormal_003C_003EAccessor : IMemberAccessor<TrailContactProperties, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TrailContactProperties owner, in Vector3 value)
				{
					owner.ContactNormal = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TrailContactProperties owner, out Vector3 value)
				{
					value = owner.ContactNormal;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyWheel_003C_003ETrailContactProperties_003C_003EPhysicalMaterial_003C_003EAccessor : IMemberAccessor<TrailContactProperties, MyStringHash>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TrailContactProperties owner, in MyStringHash value)
				{
					owner.PhysicalMaterial = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TrailContactProperties owner, out MyStringHash value)
				{
					value = owner.PhysicalMaterial;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyWheel_003C_003ETrailContactProperties_003C_003EVoxelMaterial_003C_003EAccessor : IMemberAccessor<TrailContactProperties, MyStringHash>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TrailContactProperties owner, in MyStringHash value)
				{
					owner.VoxelMaterial = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TrailContactProperties owner, out MyStringHash value)
				{
					value = owner.VoxelMaterial;
				}
			}

			public long ContactEntityId;

			public Vector3 ContactPosition;

			public Vector3 ContactNormal;

			public MyStringHash PhysicalMaterial;

			public MyStringHash VoxelMaterial;
		}

		[Serializable]
		private struct ParticleData
		{
			protected class Sandbox_Game_Entities_Blocks_MyWheel_003C_003EParticleData_003C_003EEffectName_003C_003EAccessor : IMemberAccessor<ParticleData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ParticleData owner, in string value)
				{
					owner.EffectName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ParticleData owner, out string value)
				{
					value = owner.EffectName;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyWheel_003C_003EParticleData_003C_003EPositionRelative_003C_003EAccessor : IMemberAccessor<ParticleData, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ParticleData owner, in Vector3 value)
				{
					owner.PositionRelative = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ParticleData owner, out Vector3 value)
				{
					value = owner.PositionRelative;
				}
			}

			protected class Sandbox_Game_Entities_Blocks_MyWheel_003C_003EParticleData_003C_003ENormal_003C_003EAccessor : IMemberAccessor<ParticleData, Vector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ParticleData owner, in Vector3 value)
				{
					owner.Normal = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ParticleData owner, out Vector3 value)
				{
					value = owner.Normal;
				}
			}

			public string EffectName;

			public Vector3 PositionRelative;

			public Vector3 Normal;
		}

		protected class m_particleData_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType particleData;
				ISyncType result = (particleData = new Sync<ParticleData, SyncDirection.FromServer>(P_1, P_2));
				((MyWheel)P_0).m_particleData = (Sync<ParticleData, SyncDirection.FromServer>)particleData;
				return result;
			}
		}

		protected class m_contactPointTrail_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType contactPointTrail;
				ISyncType result = (contactPointTrail = new Sync<TrailContactProperties, SyncDirection.FromServer>(P_1, P_2));
				((MyWheel)P_0).m_contactPointTrail = (Sync<TrailContactProperties, SyncDirection.FromServer>)contactPointTrail;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyWheel_003C_003EActor : IActivator, IActivator<MyWheel>
		{
			private sealed override object CreateInstance()
			{
				return new MyWheel();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWheel CreateInstance()
			{
				return new MyWheel();
			}

			MyWheel IActivator<MyWheel>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly MyStringHash m_wheelStringHash = MyStringHash.GetOrCompute("Wheel");

		private float m_terrainMaterialDistance = 0.95f;

		private float m_xDecalOffset;

		private float m_yDecalOffset;

		private MyWheelModelsDefinition m_cachedModelsDefinition;

		private Vector3D m_wheelCenterToTrail = Vector3D.Zero;

		private const float DECALSIZE_SPACING_FACTOR = 1.95f;

		private const float GROUND_NORMAL_SMOOTH_FACTOR = 1.5f;

		public Vector3 LastUsedGroundNormal;

		private int m_modelSwapCountUp;

		private bool m_usesAlternativeModel;

		public bool m_isSuspensionMounted;

		private int m_slipCountdown;

		private int m_staticHitCount;

		private int m_contactCountdown;

		private float m_frictionCollector;

		private Vector3 m_lastFrameImpuse;

		private ConcurrentNormalAggregator m_contactNormals = new ConcurrentNormalAggregator(10);

		private readonly Sync<ParticleData, SyncDirection.FromServer> m_particleData;

		private readonly Sync<TrailContactProperties, SyncDirection.FromServer> m_contactPointTrail;

		private static Dictionary<MyCubeGrid, Queue<MyTuple<DateTime, string>>> activityLog = new Dictionary<MyCubeGrid, Queue<MyTuple<DateTime, string>>>();

		private bool m_eachUpdateCallbackRegistered;

		public float Friction { get; set; }

		public ulong LastContactFrameNumber { get; private set; }

		private new MyRenderComponentWheel Render
		{
			get
			{
				return base.Render as MyRenderComponentWheel;
			}
			set
			{
				base.Render = value;
			}
		}

		public ulong FramesSinceLastContact => MySandboxGame.Static.SimulationFrameCounter - LastContactFrameNumber;

		public DateTime LastContactTime { get; set; }

		private MyWheelModelsDefinition WheelModelsDefinition
		{
			get
			{
				if (m_cachedModelsDefinition == null)
				{
					string subtypeName = base.BlockDefinition.Id.SubtypeName;
					DictionaryReader<string, MyWheelModelsDefinition> wheelModelDefinitions = MyDefinitionManager.Static.GetWheelModelDefinitions();
					if (!wheelModelDefinitions.TryGetValue(subtypeName, out m_cachedModelsDefinition))
					{
						MyDefinitionManager.Static.AddMissingWheelModelDefinition(subtypeName);
						m_cachedModelsDefinition = wheelModelDefinitions[subtypeName];
					}
				}
				return m_cachedModelsDefinition;
			}
		}

		public bool IsConsideredInContactWithStaticSurface
		{
			get
			{
				if (m_staticHitCount > 0)
				{
					return true;
				}
				return m_contactCountdown > 0;
			}
		}

		public MyTrailProperties LastTrail { get; set; }

		public MyWheel()
		{
			Friction = 1.5f;
			base.IsWorkingChanged += MyWheel_IsWorkingChanged;
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				Render = new MyRenderComponentWheel();
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			base.Init(builder, cubeGrid);
			MyObjectBuilder_Wheel myObjectBuilder_Wheel = builder as MyObjectBuilder_Wheel;
			if (myObjectBuilder_Wheel != null && !myObjectBuilder_Wheel.YieldLastComponent)
			{
				SlimBlock.DisableLastComponentYield();
			}
			if (Sync.IsServer)
			{
				m_particleData.Value = new ParticleData
				{
					EffectName = "",
					PositionRelative = Vector3.Zero,
					Normal = Vector3.Forward
				};
				m_contactPointTrail.Value = new TrailContactProperties
				{
					ContactEntityId = -1L
				};
				if (!Sync.IsDedicated)
				{
					m_contactPointTrail.ValueChanged += ProcessTrails;
				}
			}
			else
			{
				m_particleData.ValueChanged += m_particleData_ValueChanged;
				m_contactPointTrail.ValueChanged += ProcessTrails;
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		private void m_particleData_ValueChanged(SyncBase obj)
		{
			LastContactTime = DateTime.UtcNow;
			string effectName = m_particleData.Value.EffectName;
			Vector3D position = base.PositionComp.WorldMatrixRef.Translation + m_particleData.Value.PositionRelative;
			Vector3 normal = m_particleData.Value.Normal;
			if (Render != null)
			{
				Render.TrySpawnParticle(effectName, ref position, ref normal);
				Render.UpdateParticle(ref position, ref normal);
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_CubeBlock objectBuilderCubeBlock = base.GetObjectBuilderCubeBlock(copy);
			MyObjectBuilder_Wheel myObjectBuilder_Wheel = objectBuilderCubeBlock as MyObjectBuilder_Wheel;
			if (myObjectBuilder_Wheel != null)
			{
				myObjectBuilder_Wheel.YieldLastComponent = SlimBlock.YieldLastComponent;
			}
			return objectBuilderCubeBlock;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (base.CubeGrid.Physics != null)
			{
				base.CubeGrid.Physics.RigidBody.CallbackLimit = (MySession.Static.SimplifiedSimulation ? 1 : 2);
				base.CubeGrid.Physics.RigidBody.CollisionAddedCallback += RigidBody_CollisionAddedCallback;
				base.CubeGrid.Physics.RigidBody.CollisionRemovedCallback += RigidBody_CollisionRemovedCallback;
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (base.CubeGrid.Physics != null)
			{
				base.CubeGrid.Physics.RigidBody.CollisionAddedCallback -= RigidBody_CollisionAddedCallback;
				base.CubeGrid.Physics.RigidBody.CollisionRemovedCallback -= RigidBody_CollisionRemovedCallback;
			}
		}

		private bool IsAcceptableContact(HkRigidBody rb)
		{
			object userObject = rb.UserObject;
			if (userObject == null)
			{
				return false;
			}
			if (userObject == base.CubeGrid.Physics)
			{
				return false;
			}
			if (userObject is MyVoxelPhysicsBody)
			{
				return true;
			}
			MyGridPhysics myGridPhysics = userObject as MyGridPhysics;
			if (myGridPhysics != null && myGridPhysics.IsStatic)
			{
				return true;
			}
			return false;
		}

		private void RigidBody_CollisionAddedCallback(ref HkCollisionEvent e)
		{
			_ = base.CubeGrid.Physics;
			if (IsAcceptableContact(e.BodyA) || IsAcceptableContact(e.BodyB))
			{
				m_contactCountdown = 30;
				Interlocked.Increment(ref m_staticHitCount);
				RegisterPerFrameUpdate();
			}
		}

		private void RigidBody_CollisionRemovedCallback(ref HkCollisionEvent e)
		{
			_ = base.CubeGrid.Physics;
			if ((IsAcceptableContact(e.BodyA) || IsAcceptableContact(e.BodyB)) && Interlocked.Decrement(ref m_staticHitCount) < 0)
			{
				Interlocked.Increment(ref m_staticHitCount);
			}
		}

		private void MyWheel_IsWorkingChanged(MyCubeBlock obj)
		{
			if (base.Stator != null)
			{
				base.Stator.UpdateIsWorking();
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (Render != null)
			{
				Render.UpdatePosition();
			}
		}

		public override void ContactPointCallback(ref MyGridContactInfo value)
		{
			Vector3 contactNormal = value.Event.ContactPoint.Normal;
			m_contactNormals.PushNext(ref contactNormal);
			MyVoxelMaterialDefinition voxelSurfaceMaterial = value.VoxelSurfaceMaterial;
			if (voxelSurfaceMaterial != null)
			{
				m_frictionCollector = voxelSurfaceMaterial.Friction;
			}
			float num = Friction;
			if (m_isSuspensionMounted && value.CollidingEntity is MyCubeGrid && value.OtherBlock != null && value.OtherBlock.FatBlock == null)
			{
				num *= 0.07f;
				m_frictionCollector = 0.7f;
			}
			HkContactPointProperties contactProperties = value.Event.ContactProperties;
			contactProperties.Friction = num;
			contactProperties.Restitution = 0.5f;
			value.EnableParticles = false;
			value.RubberDeformation = true;
			ulong simulationFrameCounter = MySandboxGame.Static.SimulationFrameCounter;
			Vector3D contactPosition = value.ContactPosition;
<<<<<<< HEAD
			if (Sync.IsServer && voxelSurfaceMaterial != null && CanProcessTrails(value, voxelSurfaceMaterial))
=======
			if (Sync.IsServer && CanProcessTrails(value, voxelSurfaceMaterial))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				TrailContactProperties trailContactProperties = default(TrailContactProperties);
				trailContactProperties.ContactEntityId = value.CollidingEntity.EntityId;
				trailContactProperties.ContactNormal = value.Event.ContactPoint.Normal;
				trailContactProperties.ContactPosition = value.ContactPosition;
				trailContactProperties.VoxelMaterial = voxelSurfaceMaterial.Id.SubtypeId;
				trailContactProperties.PhysicalMaterial = voxelSurfaceMaterial.MaterialTypeNameHash;
				TrailContactProperties value2 = trailContactProperties;
				m_contactPointTrail.Value = value2;
			}
			else
			{
				LastTrail = null;
			}
			if (simulationFrameCounter == LastContactFrameNumber)
			{
				return;
			}
			LastContactFrameNumber = simulationFrameCounter;
			if (m_contactNormals.GetAvgNormalCached(out var normal))
			{
				contactNormal = normal;
			}
			string effectName = null;
			if (value.CollidingEntity is MyVoxelBase && MyFakes.ENABLE_DRIVING_PARTICLES)
			{
				if (voxelSurfaceMaterial != null)
				{
					MyStringHash materialTypeNameHash = voxelSurfaceMaterial.MaterialTypeNameHash;
					effectName = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, m_wheelStringHash, materialTypeNameHash);
				}
			}
			else if (value.CollidingEntity is MyCubeGrid && MyFakes.ENABLE_DRIVING_PARTICLES)
			{
				MyStringHash materialAt = (value.CollidingEntity as MyCubeGrid).Physics.GetMaterialAt(contactPosition);
				effectName = MyMaterialPropertiesHelper.Static.GetCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Start, m_wheelStringHash, materialAt);
			}
			MySandboxGame.Static.Invoke(delegate
			{
				UpdateEffect(effectName, contactPosition, contactNormal);
			}, " MyWheel.ContactPointCallback");
			RegisterPerFrameUpdate();
		}

		private bool CanProcessTrails(MyGridContactInfo value, MyVoxelMaterialDefinition voxelSurfaceMaterial)
		{
			if (base.CubeGrid.Physics != null && voxelSurfaceMaterial != null && !MyDebugDrawSettings.DEBUG_DRAW_DISABLE_TRACKTRAILS)
			{
				return value.CollidingEntity is MyVoxelBase;
			}
			return false;
		}

		private void ProcessTrails(SyncBase obj)
		{
			if (Sync.IsDedicated)
			{
				return;
<<<<<<< HEAD
			}
			TrailContactProperties value = m_contactPointTrail.Value;
			if (value.ContactEntityId == -1 || Math.Abs(Vector3D.Dot(value.ContactNormal, base.WorldMatrix.Down)) > 0.3)
			{
				return;
			}
			Vector3D vector3D = -Vector3D.Normalize(value.ContactNormal);
			Vector3D vector3D2 = Vector3D.Dot(base.WorldMatrix.Down, vector3D) * base.WorldMatrix.Down;
			vector3D = Vector3D.Normalize(vector3D - vector3D2);
			Vector3D forwardDirection = Vector3D.Cross(vector3D, base.WorldMatrix.Down);
			Vector3D vector3D3 = base.WorldMatrix.Translation - Vector3D.Dot(base.WorldMatrix.Translation - value.ContactPosition, vector3D) * vector3D;
			IReadOnlyList<MyDecalMaterial> decalMaterials = null;
			bool flag = false;
			flag = MyDecalMaterials.TryGetDecalMaterial(base.BlockDefinition.Id.SubtypeId.String, value.PhysicalMaterial.String, out decalMaterials, value.VoxelMaterial);
			if (!flag)
			{
				flag = MyDecalMaterials.TryGetDecalMaterial(base.BlockDefinition.Id.SubtypeId.String, "GenericMaterial", out decalMaterials, value.VoxelMaterial);
			}
			if (flag && decalMaterials != null && decalMaterials.Count > 0 && decalMaterials[0] != null)
			{
				if (decalMaterials[0].Spacing > 0f)
				{
					m_terrainMaterialDistance = decalMaterials[0].MinSize * decalMaterials[0].Spacing;
				}
				else
				{
					m_terrainMaterialDistance = decalMaterials[0].MinSize * 1.95f;
				}
				m_xDecalOffset = decalMaterials[0].XOffset;
				m_yDecalOffset = decalMaterials[0].YOffset;
				m_wheelCenterToTrail = vector3D3 - base.WorldMatrix.Translation;
				if (LastTrail == null || value.VoxelMaterial != LastTrail.VoxelMaterial)
				{
					LastTrail = new MyTrailProperties
					{
						EntityId = value.ContactEntityId,
						PhysicalMaterial = decalMaterials[0].Target,
						VoxelMaterial = value.VoxelMaterial,
						Position = vector3D3,
						Normal = vector3D,
						ForwardDirection = forwardDirection
					};
				}
				Vector3D vec = vector3D3 - LastTrail.Position;
				vec = Vector3D.ProjectOnPlane(ref vec, ref LastTrail.ForwardDirection);
				double num = vec.LengthSquared();
				if (num > (double)m_terrainMaterialDistance && num < (double)(m_terrainMaterialDistance * 4f))
				{
					vec = Vector3D.Normalize(vec) * Math.Sqrt(m_terrainMaterialDistance);
				}
				LastTrail.Position += vec;
			}
			else if (LastTrail != null)
			{
				LastTrail.PhysicalMaterial = value.PhysicalMaterial;
				LastTrail.VoxelMaterial = value.VoxelMaterial;
			}
=======
			}
			TrailContactProperties value = m_contactPointTrail.Value;
			if (value.ContactEntityId == -1 || Math.Abs(Vector3D.Dot(value.ContactNormal, base.WorldMatrix.Down)) > 0.3)
			{
				return;
			}
			Vector3D vector3D = -Vector3D.Normalize(value.ContactNormal);
			Vector3D vector3D2 = Vector3D.Dot(base.WorldMatrix.Down, vector3D) * base.WorldMatrix.Down;
			vector3D = Vector3D.Normalize(vector3D - vector3D2);
			Vector3D forwardDirection = Vector3D.Cross(vector3D, base.WorldMatrix.Down);
			Vector3D vector3D3 = base.WorldMatrix.Translation - Vector3D.Dot(base.WorldMatrix.Translation - value.ContactPosition, vector3D) * vector3D;
			IReadOnlyList<MyDecalMaterial> decalMaterials = null;
			bool flag = false;
			flag = MyDecalMaterials.TryGetDecalMaterial(base.BlockDefinition.Id.SubtypeId.String, value.PhysicalMaterial.String, out decalMaterials, value.VoxelMaterial);
			if (!flag)
			{
				flag = MyDecalMaterials.TryGetDecalMaterial(base.BlockDefinition.Id.SubtypeId.String, "GenericMaterial", out decalMaterials, value.VoxelMaterial);
			}
			if (flag && decalMaterials != null && decalMaterials.Count > 0 && decalMaterials[0] != null)
			{
				if (decalMaterials[0].Spacing > 0f)
				{
					m_terrainMaterialDistance = decalMaterials[0].MinSize * decalMaterials[0].Spacing;
				}
				else
				{
					m_terrainMaterialDistance = decalMaterials[0].MinSize * 1.95f;
				}
				m_xDecalOffset = decalMaterials[0].XOffset;
				m_yDecalOffset = decalMaterials[0].YOffset;
				m_wheelCenterToTrail = vector3D3 - base.WorldMatrix.Translation;
				if (LastTrail == null || value.VoxelMaterial != LastTrail.VoxelMaterial)
				{
					LastTrail = new MyTrailProperties
					{
						EntityId = value.ContactEntityId,
						PhysicalMaterial = decalMaterials[0].Target,
						VoxelMaterial = value.VoxelMaterial,
						Position = vector3D3,
						Normal = vector3D,
						ForwardDirection = forwardDirection
					};
				}
				Vector3D vec = vector3D3 - LastTrail.Position;
				vec = Vector3D.ProjectOnPlane(ref vec, ref LastTrail.ForwardDirection);
				double num = vec.LengthSquared();
				if (num > (double)m_terrainMaterialDistance && num < (double)(m_terrainMaterialDistance * 4f))
				{
					vec = Vector3D.Normalize(vec) * Math.Sqrt(m_terrainMaterialDistance);
				}
				LastTrail.Position += vec;
			}
			else if (LastTrail != null)
			{
				LastTrail.PhysicalMaterial = value.PhysicalMaterial;
				LastTrail.VoxelMaterial = value.VoxelMaterial;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void CheckTrail()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_DISABLE_TRACKTRAILS || LastTrail == null)
			{
				return;
			}
			Vector3D vector3D = base.WorldMatrix.Translation + m_wheelCenterToTrail - LastTrail.Position;
			double num = vector3D.LengthSquared();
			if (!(num > (double)m_terrainMaterialDistance))
			{
				return;
			}
			double num2 = Math.Floor(Math.Sqrt(num / (double)m_terrainMaterialDistance));
			if (num2 > 1.0 && num2 < 16.0)
			{
				Vector3D displacement = Vector3D.Normalize(vector3D) * m_terrainMaterialDistance;
				for (int i = 0; (double)i < num2; i++)
				{
					AddTrails(displacement);
				}
			}
			else
			{
				AddTrails(vector3D);
			}
		}

		public void AddTrails(Vector3D displacement)
		{
			if (!(displacement.LengthSquared() < (double)m_terrainMaterialDistance))
			{
				Vector3D vector3D = Vector3D.Normalize(Vector3D.Cross(Vector3D.Normalize(displacement), base.WorldMatrix.Down));
				Vector3D vector3D2 = Vector3.Normalize(Vector3D.Cross(base.WorldMatrix.Down, m_wheelCenterToTrail));
				float num = Math.Sign(Vector3.Dot(vector3D2, displacement));
				AddTrails(LastTrail.Position + displacement, Vector3.Normalize(LastTrail.Normal * 0.20000000298023224 + 0.800000011920929 * vector3D * num), vector3D2, LastTrail.EntityId, LastTrail.PhysicalMaterial, LastTrail.VoxelMaterial);
			}
		}

		public void AddTrails(MyTrailProperties properties)
		{
			AddTrails(properties.Position, properties.Normal, properties.ForwardDirection, properties.EntityId, properties.PhysicalMaterial, properties.VoxelMaterial);
		}

		public void AddTrails(Vector3D position, Vector3D normal, Vector3D forwardDirection, long entityId, MyStringHash physicalMaterial, MyStringHash voxelMaterial)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_DISABLE_TRACKTRAILS || LastTrail == null || (LastTrail.Position - position).LengthSquared() < (double)m_terrainMaterialDistance)
			{
				return;
			}
			Vector3D vector3D = position - base.WorldMatrix.Up * m_xDecalOffset;
			MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(vector3D + normal, vector3D - normal, 28);
			if (hitInfo.HasValue)
			{
				vector3D = hitInfo.Value.Position;
				Vector3D vector3D2 = Vector3D.Cross(base.WorldMatrix.Down, hitInfo.Value.HkHitInfo.Normal);
				forwardDirection = vector3D2;
				normal -= Vector3D.Dot(normal, vector3D2);
				normal += hitInfo.Value.HkHitInfo.Normal * 1.5f;
				normal = Vector3D.Normalize(normal);
				LastTrail.Position = position;
				LastTrail.Normal = normal;
				LastTrail.ForwardDirection = forwardDirection;
				LastTrail.EntityId = entityId;
				LastTrail.PhysicalMaterial = physicalMaterial;
				LastTrail.VoxelMaterial = voxelMaterial;
				MyHitInfo myHitInfo = default(MyHitInfo);
				myHitInfo.Position = vector3D;
				myHitInfo.Normal = normal;
				MyHitInfo hitInfo2 = myHitInfo;
				MyEntity entityById = MyEntities.GetEntityById(entityId);
				if (entityById != null)
				{
					MyDecals.HandleAddDecal(entityById, hitInfo2, forwardDirection, physicalMaterial, base.BlockDefinition.Id.SubtypeId, null, 30f, voxelMaterial, isTrail: true);
				}
			}
		}

		private void UpdateEffect(string effectName, Vector3D contactPosition, Vector3 contactNormal)
		{
			if (Render != null)
			{
				if (effectName != null)
				{
					Render.TrySpawnParticle(effectName, ref contactPosition, ref contactNormal);
				}
				Render.UpdateParticle(ref contactPosition, ref contactNormal);
			}
			if (effectName != null && Sync.IsServer)
			{
				m_particleData.Value = new ParticleData
				{
					EffectName = effectName,
					PositionRelative = contactPosition - base.PositionComp.WorldMatrixRef.Translation,
					Normal = contactNormal
				};
			}
		}

		private bool SteeringLogic()
		{
			if (!base.IsFunctional)
			{
				return false;
			}
			MyGridPhysics physics = base.CubeGrid.Physics;
			if (physics == null)
			{
				return false;
			}
			if (base.Stator != null && MyFixedGrids.IsRooted(base.Stator.CubeGrid))
			{
				return false;
			}
			if (m_slipCountdown > 0)
			{
				m_slipCountdown--;
			}
			if (m_staticHitCount == 0)
			{
				if (m_contactCountdown <= 0)
				{
					return false;
				}
				m_contactCountdown--;
				if (m_contactCountdown == 0)
				{
					m_frictionCollector = 0f;
					m_contactNormals.Clear();
					return false;
				}
			}
			Vector3 value = physics.LinearVelocity;
			if (MyUtils.IsZero(ref value) || !physics.IsActive)
			{
				return false;
			}
			MatrixD worldMatrix = base.WorldMatrix;
			Vector3D centerOfMassWorld = physics.CenterOfMassWorld;
			if (!m_contactNormals.GetAvgNormal(out var normal))
			{
				return false;
			}
			LastUsedGroundNormal = normal;
			Vector3 guideVector = worldMatrix.Up;
			Vector3 guideVector2 = Vector3.Cross(normal, guideVector);
			value = Vector3.ProjectOnPlane(ref value, ref normal);
			Vector3 vector = Vector3.ProjectOnVector(ref value, ref guideVector2);
			Vector3 value2 = vector - value;
			if (MyUtils.IsZero(ref value2))
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			float num = 6f * m_frictionCollector;
			Vector3 vec = Vector3.ProjectOnVector(ref value2, ref guideVector);
			float num2 = vec.Length();
			bool flag3 = num2 > num;
			if (flag3 || m_slipCountdown != 0)
			{
				float num3 = 1f / num2;
				num3 *= num;
				vec *= num3;
				flag = true;
				vec *= 1f - MyPhysicsConfig.WheelSlipCutAwayRatio;
				if (flag3)
				{
					m_slipCountdown = MyPhysicsConfig.WheelSlipCountdown;
				}
			}
			else if ((double)num2 < 0.1)
			{
				flag2 = true;
			}
			if (!flag2)
			{
				if (vec.LengthSquared() < 0.001f)
				{
					return !flag2;
				}
				vec *= 1f - (1f - m_frictionCollector) * MyPhysicsConfig.WheelSurfaceMaterialSteerRatio;
				Vector3 vec2 = vec;
				Vector3 vector2 = Vector3.ProjectOnPlane(ref vec2, ref normal);
				MyMechanicalConnectionBlockBase stator = base.Stator;
				MyPhysicsBody myPhysicsBody = null;
				if (stator != null)
				{
					myPhysicsBody = base.Stator.CubeGrid.Physics;
				}
				vector2 *= 0.1f;
				if (myPhysicsBody == null)
				{
					vector2 *= physics.Mass;
					physics.ApplyImpulse(vector2, centerOfMassWorld);
				}
				else
				{
					Vector3D vector3D = Vector3D.Zero;
					MyMotorSuspension myMotorSuspension = stator as MyMotorSuspension;
					if (myMotorSuspension != null)
					{
						vector2 *= MyMath.Clamp(myMotorSuspension.Friction * 2f, 0f, 1f);
						myMotorSuspension.GetCoMVectors(out var adjustmentVector);
						vector3D = Vector3D.TransformNormal(-adjustmentVector, stator.CubeGrid.WorldMatrix);
					}
					Vector3D vector3D2 = centerOfMassWorld + vector3D;
					float wheelImpulseBlending = MyPhysicsConfig.WheelImpulseBlending;
					vector2 = (m_lastFrameImpuse = m_lastFrameImpuse * wheelImpulseBlending + vector2 * (1f - wheelImpulseBlending));
					vector2 *= myPhysicsBody.Mass;
					myPhysicsBody.ApplyImpulse(vector2, vector3D2);
					if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS)
					{
						MyRenderProxy.DebugDrawArrow3DDir(vector3D2, -vector3D, Color.Red);
						MyRenderProxy.DebugDrawSphere(vector3D2, 0.1f, Color.Yellow, 1f, depthRead: false);
					}
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS)
			{
				MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld, value, Color.Yellow);
				MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld, vector, Color.Blue);
				MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld, vec, Color.MediumPurple);
				MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld + value, value2, Color.Red);
				MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld + guideVector, normal, Color.AliceBlue);
				MyRenderProxy.DebugDrawArrow3DDir(centerOfMassWorld, Vector3.ProjectOnPlane(ref vec, ref normal), flag ? Color.DarkRed : Color.IndianRed);
				if (m_slipCountdown > 0)
				{
					MyRenderProxy.DebugDrawText3D(centerOfMassWorld + guideVector * 2f, "Drift", Color.Red, 1f, depthRead: false);
				}
				MyRenderProxy.DebugDrawText3D(centerOfMassWorld + guideVector * 1.2f, m_staticHitCount.ToString(), Color.Red, 1f, depthRead: false);
			}
			return !flag2;
		}

		public override void UpdateBeforeSimulation()
		{
			CheckTrail();
			base.UpdateBeforeSimulation();
			SwapModelLogic();
			bool flag = SteeringLogic();
			if (!flag && m_contactCountdown == 0)
			{
				m_lastFrameImpuse = Vector3.Zero;
				if ((Render == null || (Render != null && !Render.UpdateNeeded)) && !base.HasDamageEffect)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_WHEEL_PHYSICS)
			{
				MatrixD worldMatrix = base.WorldMatrix;
				MyRenderProxy.DebugDrawCross(worldMatrix.Translation, worldMatrix.Up, worldMatrix.Forward, flag ? Color.Green : Color.Red);
			}
		}

		public override string CalculateCurrentModel(out Matrix orientation)
		{
			string result = base.CalculateCurrentModel(out orientation);
			if (base.CubeGrid.Physics == null)
			{
				return result;
			}
			if (base.Stator == null || !base.IsFunctional)
			{
				return result;
			}
			if (m_usesAlternativeModel)
			{
				return WheelModelsDefinition.AlternativeModel;
			}
			return result;
		}

		private void SwapModelLogic()
		{
			if (!MyFakes.WHEEL_ALTERNATIVE_MODELS_ENABLED || base.Stator == null || !base.IsFunctional)
			{
				if (m_usesAlternativeModel)
				{
					m_usesAlternativeModel = false;
					UpdateVisual();
				}
				return;
			}
			float angularVelocityThreshold = WheelModelsDefinition.AngularVelocityThreshold;
			float observerAngularVelocityDiff = GetObserverAngularVelocityDiff();
			bool num = m_usesAlternativeModel && observerAngularVelocityDiff + 5f < angularVelocityThreshold;
			bool flag = !m_usesAlternativeModel && observerAngularVelocityDiff - 5f > angularVelocityThreshold;
			if (num || flag)
			{
				m_modelSwapCountUp++;
				if (m_modelSwapCountUp >= 5)
				{
					m_usesAlternativeModel = !m_usesAlternativeModel;
					UpdateVisual();
				}
			}
			else
			{
				m_modelSwapCountUp = 0;
			}
		}

		private float GetObserverAngularVelocityDiff()
		{
			MyGridPhysics physics = base.CubeGrid.Physics;
			if (physics != null && physics.LinearVelocity.LengthSquared() > 16f)
			{
				IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
				if (controlledEntity != null)
				{
					MyEntity entity = controlledEntity.Entity;
					if (entity != null)
					{
						MyPhysicsComponentBase physics2 = entity.GetTopMostParent().Physics;
						if (physics2 != null)
						{
							return (physics.AngularVelocity - physics2.AngularVelocity).Length();
						}
					}
				}
			}
			return 0f;
		}

		public static void WheelExplosionLog(MyCubeGrid grid, MyTerminalBlock block, string message)
		{
		}

		public static void DumpActivityLog()
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			lock (activityLog)
			{
				foreach (KeyValuePair<MyCubeGrid, Queue<MyTuple<DateTime, string>>> item2 in activityLog)
				{
					MyCubeGrid key = item2.Key;
					MyLog.Default.WriteLine("GRID: " + key.DisplayName);
					Enumerator<MyTuple<DateTime, string>> enumerator2 = item2.Value.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MyTuple<DateTime, string> current2 = enumerator2.get_Current();
							MyLog @default = MyLog.Default;
							DateTime item = current2.Item1;
							@default.WriteLine("[" + item.ToString("dd/MM hh:mm:ss:FFF") + "] " + current2.Item2);
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					MyLog.Default.Flush();
				}
				activityLog.Clear();
			}
		}

		public override void Attach(MyMechanicalConnectionBlockBase parent)
		{
			base.Attach(parent);
			m_isSuspensionMounted = base.Stator is MyMotorSuspension;
		}

		public override void Detach(bool isWelding)
		{
			m_isSuspensionMounted = false;
			base.Detach(isWelding);
		}

		private void RegisterPerFrameUpdate()
		{
			if ((base.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) == 0 && !m_eachUpdateCallbackRegistered)
			{
				m_eachUpdateCallbackRegistered = true;
				MySandboxGame.Static.Invoke(delegate
				{
					m_eachUpdateCallbackRegistered = false;
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				}, "WheelEachUpdate");
			}
		}
	}
}
