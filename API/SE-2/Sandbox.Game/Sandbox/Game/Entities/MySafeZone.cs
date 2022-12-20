using System;
using System.Collections.Generic;
using System.Linq;
using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
<<<<<<< HEAD
using Sandbox.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Audio;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components;
using VRage.Groups;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
	[MyEntityType(typeof(MyObjectBuilder_SafeZone), true)]
	public class MySafeZone : MyEntity, IMyEventProxy, IMyEventOwner, IMyProjectileDetector
	{
		/// <summary>
		/// Must be sorted in order from lower to upper access
		/// </summary>
		private enum SubgridCheckResult
		{
			NotSafe,
			NeedExtraCheck,
			Safe,
			Admin
		}

		protected sealed class InsertEntity_Implementation_003C_003ESystem_Int64_0023System_Boolean : ICallSite<MySafeZone, long, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySafeZone @this, in long entityId, in bool addedOrRemoved, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.InsertEntity_Implementation(entityId, addedOrRemoved);
			}
		}

		protected sealed class InsertEntities_Implementation_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Int64_003E : ICallSite<MySafeZone, List<long>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySafeZone @this, in List<long> list, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.InsertEntities_Implementation(list);
			}
		}

		protected sealed class RemoveEntity_Implementation_003C_003ESystem_Int64_0023System_Boolean : ICallSite<MySafeZone, long, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySafeZone @this, in long entityId, in bool addedOrRemoved, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RemoveEntity_Implementation(entityId, addedOrRemoved);
			}
		}

		private class Sandbox_Game_Entities_MySafeZone_003C_003EActor : IActivator, IActivator<MySafeZone>
		{
			private sealed override object CreateInstance()
			{
				return new MySafeZone();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySafeZone CreateInstance()
			{
				return new MySafeZone();
			}

			MySafeZone IActivator<MySafeZone>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private const string MODEL_SPHERE = "Models\\Environment\\SafeZone\\SafeZone.mwm";

		private const string MODEL_BOX = "Models\\Environment\\SafeZone\\SafeZoneBox.mwm";

		public static readonly float MAX_RADIUS = 500f;

		public static readonly float MIN_RADIUS = 10f;

		public float Radius;

		protected MyConcurrentHashSet<long> m_containedEntities = new MyConcurrentHashSet<long>();

		public List<MyFaction> Factions = new List<MyFaction>();

		public List<long> Players = new List<long>();

		public HashSet<long> Entities = new HashSet<long>();

		private long m_safezoneBlockId;

		private List<long> m_entitiesToSend = new List<long>();

		private List<long> m_entitiesToAdd = new List<long>();

		private MyHudNotification m_safezoneEnteredNotification = new MyHudNotification(MyCommonTexts.SafeZone_Entered, 2000, "White");

		private MyHudNotification m_safezoneLeftNotification = new MyHudNotification(MyCommonTexts.SafeZone_Left, 2000, "White");

		private Dictionary<MyStringHash, MyTextureChange> m_texturesDefinitions;

		private MySafeZoneSettingsDefinition m_safeZoneSettings;

		private Color m_animatedColor;

		private TimeSpan m_blendTimer;

		private bool m_isAnimating;

<<<<<<< HEAD
		private HashSet<IMyEntity> m_RemoveEntityPhantomTaskList = new HashSet<IMyEntity>();

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool Enabled { get; set; }

		/// <summary>
		/// Entity id of the safezone block associated with the safezone. If 0 than safezone is not associated to any block.
		/// </summary>
		public long SafeZoneBlockId => m_safezoneBlockId;

		public MySafeZoneAccess AccessTypePlayers { get; set; }

		public MySafeZoneAccess AccessTypeFactions { get; set; }

		public MySafeZoneAccess AccessTypeGrids { get; set; }

		public MySafeZoneAccess AccessTypeFloatingObjects { get; set; }

		public MySafeZoneAction AllowedActions { get; set; }

		public MySafeZoneShape Shape { get; set; }

		public Color ModelColor { get; private set; }

		public MyStringHash CurrentTexture { get; private set; }

		public MyTextureChange DisabledTexture { get; private set; }

		public bool IsVisible { get; set; }

		public Vector3 Size { get; set; }
<<<<<<< HEAD

		public bool IsStatic => SafeZoneBlockId != 0;

		public IMyEntity HitEntity => this;

		public BoundingBoxD DetectorAABB => base.PositionComp.WorldAABB;
=======

		public bool IsStatic => SafeZoneBlockId != 0;

		public bool IsActionAllowed(MyEntity entity, MySafeZoneAction action, long sourceEntityId = 0L)
		{
			if (!Enabled)
			{
				return true;
			}
			if (entity == null)
			{
				return false;
			}
			if (!m_containedEntities.Contains(entity.EntityId))
			{
				return true;
			}
			if (sourceEntityId != 0L && MyEntities.TryGetEntityById(sourceEntityId, out var entity2) && !IsSafe(entity2.GetTopMostParent()))
			{
				return false;
			}
			return AllowedActions.HasFlag(action);
		}

		private bool IsOutside(BoundingBoxD aabb)
		{
			bool flag = false;
			if (Shape == MySafeZoneShape.Sphere)
			{
				return !new BoundingSphereD(base.PositionComp.GetPosition(), Radius).Intersects(aabb);
			}
			return !new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrixRef).Intersects(ref aabb);
		}

		private bool IsOutside(MyEntity entity)
		{
			bool flag = false;
			MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(entity.PositionComp.LocalAABB, entity.PositionComp.WorldMatrixRef);
			if (Shape == MySafeZoneShape.Sphere)
			{
				BoundingSphereD sphere = new BoundingSphereD(base.PositionComp.GetPosition(), Radius);
				return !other.Intersects(ref sphere);
			}
			return !new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrixRef).Intersects(ref other);
		}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public bool IsDetectorEnabled
		{
			get
			{
				if (Enabled)
				{
					return (AllowedActions & MySafeZoneAction.Shooting) == 0;
				}
<<<<<<< HEAD
				return false;
			}
=======
			}
			if (num == 1)
			{
				return m_containedEntities.Contains(entityId);
			}
			return false;
		}

		public bool IsEmpty()
		{
			return m_containedEntities.Count == 0;
		}

		public bool IsActionAllowed(BoundingBoxD aabb, MySafeZoneAction action, long sourceEntityId = 0L)
		{
			if (!Enabled)
			{
				return true;
			}
			if (IsOutside(aabb))
			{
				return true;
			}
			if (sourceEntityId != 0L && MyEntities.TryGetEntityById(sourceEntityId, out var entity) && !IsSafe(entity.GetTopMostParent()))
			{
				return false;
			}
			return (AllowedActions & action) == action;
		}

		public bool IsActionAllowed(Vector3D point, MySafeZoneAction action, long sourceEntityId = 0L)
		{
			if (!Enabled)
			{
				return true;
			}
			bool flag = false;
			if ((Shape != 0) ? (!new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrixRef).Contains(ref point)) : (new BoundingSphereD(base.PositionComp.GetPosition(), Radius).Contains(point) != ContainmentType.Contains))
			{
				return true;
			}
			if (sourceEntityId != 0L && MyEntities.TryGetEntityById(sourceEntityId, out var entity) && !IsSafe(entity.GetTopMostParent()))
			{
				return false;
			}
			return (AllowedActions & action) == action;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public MySafeZone()
		{
			base.SyncFlag = true;
		}

		protected override void Closing()
		{
			MySessionComponentSafeZones.RemoveSafeZone(this);
			base.Closing();
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
			if (m_texturesDefinitions == null)
			{
				IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
				if (allDefinitions == null)
				{
					MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
				}
				else
				{
					m_texturesDefinitions = new Dictionary<MyStringHash, MyTextureChange>();
					foreach (MySafeZoneTexturesDefinition item in allDefinitions)
					{
						if (item.Id.SubtypeName == "Disabled")
						{
							DisabledTexture = item.Texture;
						}
						m_texturesDefinitions.Add(item.DisplayTextId, item.Texture);
					}
					if (m_texturesDefinitions.Count == 0)
					{
						MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
					}
				}
			}
			if (m_safeZoneSettings == null)
			{
				MySafeZoneSettingsDefinition definition = MyDefinitionManager.Static.GetDefinition<MySafeZoneSettingsDefinition>("SafeZoneSettings");
				if (definition == null)
				{
					MyLog.Default.Error("Safe Zone Settings definition for safe zone are missing. Without it, safezone wont work propertly.");
					m_safeZoneSettings = new MySafeZoneSettingsDefinition();
				}
				else
				{
					m_safeZoneSettings = definition;
				}
			}
			CurrentTexture = MyStringHash.NullOrEmpty;
			MyRenderComponentSafeZone myRenderComponentSafeZone = (MyRenderComponentSafeZone)(base.Render = new MyRenderComponentSafeZone());
			base.Render.PersistentFlags &= ~MyPersistentEntityFlags2.CastShadows;
			base.Render.EnableColorMaskHsv = true;
			base.Render.FadeIn = (base.Render.FadeOut = true);
			base.Save = true;
			base.NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
			MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = (MyObjectBuilder_SafeZone)objectBuilder;
			InitInternal(myObjectBuilder_SafeZone, insertEntities: false);
			Init(null, "Models\\Environment\\SafeZone\\SafeZone.mwm", null, null);
			if (Shape == MySafeZoneShape.Sphere)
			{
				base.PositionComp.LocalAABB = new BoundingBox(new Vector3(0f - Radius), new Vector3(Radius));
			}
			else
			{
				base.PositionComp.LocalAABB = new BoundingBox(-Size / 2f, Size / 2f);
			}
			MySessionComponentSafeZones.AddSafeZone(this);
			if (base.PositionComp != null)
			{
				base.PositionComp.OnPositionChanged += PositionComp_OnPositionChanged;
			}
			base.DisplayName = myObjectBuilder_SafeZone.DisplayName;
			m_safezoneBlockId = myObjectBuilder_SafeZone.SafeZoneBlockId;
		}

		internal void InitInternal(MyObjectBuilder_SafeZone ob, bool insertEntities = true)
		{
			float num = MIN_RADIUS;
			if (ob.Radius.IsValid())
			{
				num = MathHelper.Clamp(ob.Radius, MIN_RADIUS, MAX_RADIUS);
			}
			bool flag = num != Radius;
			Radius = num;
			bool enabled = Enabled;
			bool flag2 = Enabled != ob.Enabled;
			Enabled = ob.Enabled;
			AccessTypePlayers = ob.AccessTypePlayers;
			AccessTypeFactions = ob.AccessTypeFactions;
			AccessTypeGrids = ob.AccessTypeGrids;
			AccessTypeFloatingObjects = ob.AccessTypeFloatingObjects;
			AllowedActions = ob.AllowedActions;
			if (MySession.Static.AppVersionFromSave < 1198000 && Sync.IsServer && (AllowedActions & MySafeZoneAction.Building) != 0)
			{
				AllowedActions |= MySafeZoneAction.BuildingProjections;
			}
			bool flag3 = Size != ob.Size;
			Size = ob.Size;
			bool flag4 = Shape != ob.Shape;
			Shape = ob.Shape;
			IsVisible = ob.IsVisible;
			Color color = new Color(ob.ModelColor);
			bool num2 = color != ModelColor;
			ModelColor = color;
			MyStringHash orCompute = MyStringHash.GetOrCompute(ob.Texture);
			bool flag5 = false;
			if (m_texturesDefinitions.TryGetValue(orCompute, out var _))
			{
				flag5 = CurrentTexture != orCompute;
				CurrentTexture = orCompute;
			}
			bool flag6 = false;
			if (ob.PositionAndOrientation.HasValue)
			{
				MatrixD other = ob.PositionAndOrientation.Value.GetMatrix();
				flag6 = !base.PositionComp.WorldMatrixRef.EqualsFast(ref other, 0.01);
				base.PositionComp.SetWorldMatrix(ref other);
			}
			if (ob.Factions != null)
			{
				Factions = Enumerable.ToList<MyFaction>(Enumerable.Where<MyFaction>((IEnumerable<MyFaction>)Enumerable.ToList<long>((IEnumerable<long>)ob.Factions).ConvertAll((long x) => (MyFaction)MySession.Static.Factions.TryGetFactionById(x)), (Func<MyFaction, bool>)((MyFaction x) => x != null)));
			}
			if (ob.Players != null)
			{
				Players = Enumerable.ToList<long>((IEnumerable<long>)ob.Players);
			}
			if (ob.Entities != null)
			{
				Entities = new HashSet<long>((IEnumerable<long>)ob.Entities);
			}
			if (flag || flag4 || flag3 || flag6)
			{
				RecreatePhysics(insertEntities, triggerNotification: false);
				flag2 = false;
			}
			if (flag2 && insertEntities)
			{
				StartEnableAnimation(enabled);
				InsertContainingEntities(triggerNotification: false);
			}
			if (num2 || (flag2 && insertEntities) || flag5 || flag4)
			{
				RefreshGraphics();
			}
			if (!Sync.IsServer && ob.ContainedEntities != null)
			{
				m_entitiesToAdd.AddRange(ob.ContainedEntities);
			}
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_SafeZone myObjectBuilder_SafeZone = (MyObjectBuilder_SafeZone)base.GetObjectBuilder(copy);
			myObjectBuilder_SafeZone.Radius = Radius;
			myObjectBuilder_SafeZone.Size = Size;
			myObjectBuilder_SafeZone.Shape = Shape;
			myObjectBuilder_SafeZone.Enabled = Enabled;
			myObjectBuilder_SafeZone.AccessTypePlayers = AccessTypePlayers;
			myObjectBuilder_SafeZone.AccessTypeFactions = AccessTypeFactions;
			myObjectBuilder_SafeZone.AccessTypeGrids = AccessTypeGrids;
			myObjectBuilder_SafeZone.AccessTypeFloatingObjects = AccessTypeFloatingObjects;
			myObjectBuilder_SafeZone.AllowedActions = AllowedActions;
			myObjectBuilder_SafeZone.DisplayName = base.DisplayName;
			myObjectBuilder_SafeZone.ModelColor = ModelColor.ToVector3();
			myObjectBuilder_SafeZone.Texture = CurrentTexture.String;
			myObjectBuilder_SafeZone.Factions = Factions.ConvertAll((MyFaction x) => x.FactionId).ToArray();
			myObjectBuilder_SafeZone.Players = Players.ToArray();
			myObjectBuilder_SafeZone.Entities = Enumerable.ToArray<long>((IEnumerable<long>)Entities);
			myObjectBuilder_SafeZone.SafeZoneBlockId = m_safezoneBlockId;
			if (Sync.IsServer && m_containedEntities.Count > 0)
			{
				myObjectBuilder_SafeZone.ContainedEntities = Enumerable.ToArray<long>((IEnumerable<long>)m_containedEntities);
			}
			myObjectBuilder_SafeZone.IsVisible = IsVisible;
			return myObjectBuilder_SafeZone;
		}

		public void RecreatePhysics(bool insertEntities = true, bool triggerNotification = true)
		{
			if (base.Physics != null)
			{
				base.Physics.Close();
				base.Physics = null;
			}
			if (Shape == MySafeZoneShape.Sphere)
			{
				base.PositionComp.LocalAABB = new BoundingBox(new Vector3(0f - Radius), new Vector3(Radius));
				UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZone.mwm", new Vector3(Radius));
			}
			else
			{
				base.PositionComp.LocalAABB = new BoundingBox(-Size / 2f, Size / 2f);
				UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZoneBox.mwm", Size * 0.5f);
			}
			if (insertEntities)
			{
				m_containedEntities.Clear();
				MySessionComponentSafeZones.EntityToSafezonesCache.ClearForSafezone(this);
			}
			if (Sync.IsServer)
			{
				HkBvShape hkBvShape = CreateFieldShape();
				base.Physics = new MyPhysicsBody(this, RigidBodyFlag.RBF_STATIC);
				base.Physics.IsPhantom = true;
				((MyPhysicsBody)base.Physics).CreateFromCollisionObject(hkBvShape, base.PositionComp.LocalVolume.Center, base.WorldMatrix);
				hkBvShape.Base.RemoveReference();
				base.Physics.Enabled = true;
				if (insertEntities)
				{
					InsertContainingEntities(triggerNotification);
				}
			}
			if (!Sync.IsDedicated)
			{
				RefreshGraphics();
			}
		}

		private void StartEnableAnimation(bool lastEnabled)
		{
			m_blendTimer = TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMilliseconds(m_safeZoneSettings.EnableAnimationTimeMs);
			m_animatedColor = Color.Black;
			m_isAnimating = true;
			MyRenderComponentSafeZone myRenderComponentSafeZone;
			if ((myRenderComponentSafeZone = base.Render as MyRenderComponentSafeZone) != null)
			{
				myRenderComponentSafeZone.AddTransitionObject(GetTextureChange(lastEnabled));
				myRenderComponentSafeZone.UpdateTransitionObjColor(lastEnabled ? ModelColor : Color.White);
			}
		}

		private void UpdateRenderObject(string modelName, Vector3 scale)
		{
			MyRenderComponentSafeZone myRenderComponentSafeZone;
			if ((myRenderComponentSafeZone = base.Render as MyRenderComponentSafeZone) != null)
			{
				myRenderComponentSafeZone.SwitchModel(modelName);
				myRenderComponentSafeZone.ChangeScale(scale);
			}
		}

		public void RefreshGraphics()
		{
			MyRenderComponentSafeZone myRenderComponentSafeZone;
			if (!Sync.IsDedicated && (myRenderComponentSafeZone = base.Render as MyRenderComponentSafeZone) != null)
			{
				Color newColor = (m_isAnimating ? m_animatedColor : (Enabled ? ModelColor : Color.White));
				myRenderComponentSafeZone.ChangeColor(newColor);
				myRenderComponentSafeZone.InvalidateRenderObjects();
				myRenderComponentSafeZone.TextureChanges = GetTextureChange(Enabled);
			}
		}

		private Dictionary<string, MyTextureChange> GetTextureChange(bool enabled)
		{
			if (enabled)
			{
				MyTextureChange value = m_texturesDefinitions[CurrentTexture];
				return new Dictionary<string, MyTextureChange> { { "SafeZoneShield_Material", value } };
			}
			return new Dictionary<string, MyTextureChange> { { "SafeZoneShield_Material", DisabledTexture } };
		}

		private void InsertContainingEntities(bool triggerNotification = true)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			List<MyEntity> list = null;
			if (Shape == MySafeZoneShape.Sphere)
			{
				BoundingSphereD boundingSphere = new BoundingSphereD(base.PositionComp.WorldMatrixRef.Translation, Radius);
				list = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
			}
			else
			{
				MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrixRef);
				list = MyEntities.GetEntitiesInOBB(ref obb);
			}
			foreach (MyEntity item in list)
			{
				MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrixRef);
				MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(item.PositionComp.LocalAABB, item.PositionComp.WorldMatrixRef);
				if (myOrientedBoundingBoxD.Contains(ref other) == ContainmentType.Contains && InsertEntityInternal(item, addedOrRemoved: false, triggerNotification))
				{
					m_entitiesToSend.Add(item.EntityId);
				}
			}
			SendInsertedEntities(m_entitiesToSend);
			list.Clear();
			m_entitiesToSend.Clear();
		}

		internal void InsertEntity(MyEntity entity)
		{
			if (Shape == MySafeZoneShape.Box)
			{
				MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrixRef);
				MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(entity.PositionComp.LocalAABB, entity.PositionComp.WorldMatrixRef);
				if (myOrientedBoundingBoxD.Contains(ref other) != ContainmentType.Contains)
				{
					return;
				}
			}
			else
			{
				BoundingSphereD sphere = new BoundingSphereD(base.PositionComp.WorldMatrixRef.Translation, Radius);
				if (new MyOrientedBoundingBoxD(entity.PositionComp.LocalAABB, entity.PositionComp.WorldMatrixRef).Contains(ref sphere) != ContainmentType.Contains)
				{
					return;
				}
			}
			if (InsertEntityInternal(entity, addedOrRemoved: false))
			{
				SendInsertedEntity(entity.EntityId, addedOrRemoved: false);
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (Sync.IsServer)
			{
				InsertContainingEntities();
			}
			else
			{
				m_containedEntities.Clear();
				MySessionComponentSafeZones.EntityToSafezonesCache.ClearForSafezone(this);
				foreach (long item in m_entitiesToAdd)
				{
					InsertEntity_Implementation(item, addedOrRemoved: false);
				}
				m_entitiesToAdd.Clear();
			}
			if (!Sync.IsDedicated)
			{
				if (Shape == MySafeZoneShape.Sphere)
				{
					UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZone.mwm", new Vector3(Radius));
				}
				else
				{
					UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZoneBox.mwm", Size * 0.5f);
				}
				RefreshGraphics();
			}
		}

		private HkBvShape CreateFieldShape()
		{
			return new HkBvShape(childShape: new HkPhantomCallbackShape(phantom_Enter, phantom_Leave), boundingVolumeShape: GetHkShape(), policy: HkReferencePolicy.TakeOwnership);
		}

		protected HkShape GetHkShape()
		{
			if (Shape == MySafeZoneShape.Sphere)
			{
				return new HkSphereShape(Radius);
			}
			return new HkBoxShape(Size / 2f);
		}

		private void phantom_Enter(HkPhantomCallbackShape sender, HkRigidBody body)
		{
			MyEntity myEntity = body.GetEntity(0u) as MyEntity;
			bool addedOrRemoved = MySessionComponentSafeZones.IsRecentlyAddedOrRemoved(myEntity);
			if (InsertEntityInternal(myEntity, addedOrRemoved))
			{
				SendInsertedEntity(myEntity.EntityId, addedOrRemoved);
			}
		}

		private bool InsertEntityInternal(MyEntity entity, bool addedOrRemoved, bool triggerNotification = true)
		{
			if (entity != null)
			{
				MyEntity topEntity = entity.GetTopMostParent();
				if (topEntity.Physics == null)
				{
					return false;
				}
				if (topEntity is MySafeZone)
				{
					return false;
				}
				if (topEntity.Physics.ShapeChangeInProgress)
				{
					return false;
				}
				if (!m_containedEntities.Contains(topEntity.EntityId))
				{
					m_containedEntities.Add(topEntity.EntityId);
					MySessionComponentSafeZones.EntityToSafezonesCache.Add(topEntity.EntityId, this);
					if (triggerNotification)
					{
						UpdatePlayerNotification(topEntity, addedOrRemoved);
					}
					MySandboxGame.Static.Invoke(delegate
					{
						if (topEntity.Physics != null && topEntity.Physics.HasRigidBody && !topEntity.Physics.IsStatic)
						{
							((MyPhysicsBody)topEntity.Physics).RigidBody.Activate();
						}
					}, "MyGravityGeneratorBase/Activate physics");
					MyCubeGrid myCubeGrid;
					if ((myCubeGrid = entity as MyCubeGrid) != null)
					{
						foreach (MyShipController fatBlock in myCubeGrid.GetFatBlocks<MyShipController>())
						{
							if (!(fatBlock is MyRemoteControl) && fatBlock.Pilot != null && fatBlock.Pilot.GetTopMostParent() == topEntity && InsertEntityInternal(fatBlock.Pilot, addedOrRemoved))
							{
								SendInsertedEntity(fatBlock.Pilot.EntityId, addedOrRemoved);
							}
						}
					}
					return true;
				}
			}
			return false;
		}

		private void UpdatePlayerNotification(MyEntity topEntity, bool addedOrRemoved)
		{
			if (Enabled && MySession.Static.ControlledEntity != null && ((MyEntity)MySession.Static.ControlledEntity).GetTopMostParent() == topEntity && !addedOrRemoved)
			{
				if (!IsSafe((MyEntity)MySession.Static.ControlledEntity))
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
					return;
				}
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				MyHud.Notifications.Add(m_safezoneEnteredNotification);
			}
		}

		internal bool RemoveEntityInternal(MyEntity entity, bool addedOrRemoved)
		{
			MySessionComponentSafeZones.EntityToSafezonesCache.Remove(entity.EntityId, this);
			bool num = m_containedEntities.Remove(entity.EntityId);
			if (num)
			{
				RemoveEntityLocal(entity, addedOrRemoved);
			}
			return num;
		}

		private void RemoveEntityLocal(MyEntity entity, bool addedOrRemoved)
		{
			if (Enabled && MySession.Static != null && MySession.Static.ControlledEntity != null && ((MyEntity)MySession.Static.ControlledEntity).GetTopMostParent() == entity && IsSafe(entity) && !addedOrRemoved && (!(entity is MyCharacter) || !((entity as MyCharacter).IsUsing is MyCockpit)))
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
				MyHud.Notifications.Add(m_safezoneLeftNotification);
			}
		}

		private void entity_OnClose(MyEntity obj)
		{
			if (base.PositionComp != null)
			{
				base.PositionComp.OnPositionChanged -= PositionComp_OnPositionChanged;
			}
			if (RemoveEntityInternal(obj, addedOrRemoved: true))
			{
				SendRemovedEntity(obj.EntityId, addedOrRemoved: true);
			}
		}

		private void PositionComp_OnPositionChanged(MyPositionComponentBase obj)
		{
			if (Shape == MySafeZoneShape.Sphere)
			{
				UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZone.mwm", new Vector3(Radius));
			}
			else
			{
				UpdateRenderObject("Models\\Environment\\SafeZone\\SafeZoneBox.mwm", Size * 0.5f);
			}
			RefreshGraphics();
		}

		private void phantom_Leave(HkPhantomCallbackShape sender, HkRigidBody body)
		{
			IMyEntity entity = body.GetEntity(0u);
			if (entity != null)
			{
				RemoveEntityPhantom(body, entity);
			}
		}

		private void RemoveEntityPhantom(HkRigidBody body, IMyEntity entity)
		{
			MyEntity topEntity = entity.GetTopMostParent() as MyEntity;
<<<<<<< HEAD
			if (topEntity != entity || topEntity.Physics == null || topEntity.Physics.ShapeChangeInProgress)
			{
				return;
			}
			bool addedOrRemoved = MySessionComponentSafeZones.IsRecentlyAddedOrRemoved(topEntity) || !entity.InScene;
			if (m_RemoveEntityPhantomTaskList.Contains(entity))
			{
				return;
			}
			m_RemoveEntityPhantomTaskList.Add(entity);
=======
			if (topEntity.Physics == null || topEntity.Physics.ShapeChangeInProgress || topEntity != entity)
			{
				return;
			}
			bool addedOrRemoved = MySessionComponentSafeZones.IsRecentlyAddedOrRemoved(topEntity) || !entity.InScene;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Vector3D position1 = entity.Physics.ClusterToWorld(body.Position);
			Quaternion rotation1 = Quaternion.CreateFromRotationMatrix(body.GetRigidBodyMatrix());
			MySandboxGame.Static.Invoke(delegate
			{
<<<<<<< HEAD
				m_RemoveEntityPhantomTaskList.Remove(entity);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (base.Physics != null)
				{
					if (entity.MarkedForClose)
					{
						if (RemoveEntityInternal(topEntity, addedOrRemoved))
						{
							SendRemovedEntity(topEntity.EntityId, addedOrRemoved);
						}
					}
					else
					{
						MyCharacter myCharacter = entity as MyCharacter;
						bool flag = (myCharacter?.IsDead ?? false) || body.IsDisposed || !entity.Physics.IsInWorld;
						if (entity.Physics != null && !flag)
						{
							position1 = entity.Physics.ClusterToWorld(body.Position);
							rotation1 = Quaternion.CreateFromRotationMatrix(body.GetRigidBodyMatrix());
						}
						Vector3D translation = base.PositionComp.GetPosition();
						MatrixD matrix = base.PositionComp.GetOrientation();
						Quaternion rotation2 = Quaternion.CreateFromRotationMatrix(in matrix);
						HkShape shape = HkShape.Empty;
						if (entity.Physics != null)
						{
							MyPhysicsBody myPhysicsBody;
							if (entity.Physics.RigidBody != null)
							{
								shape = entity.Physics.RigidBody.GetShape();
							}
							else if ((myPhysicsBody = entity.Physics as MyPhysicsBody) != null && myCharacter != null && myPhysicsBody.CharacterProxy != null)
							{
								shape = myPhysicsBody.CharacterProxy.GetHitRigidBody().GetShape();
							}
						}
						if (flag || !shape.IsValid || !MyPhysics.IsPenetratingShapeShape(shape, ref position1, ref rotation1, base.Physics.RigidBody.GetShape(), ref translation, ref rotation2))
						{
							if (RemoveEntityInternal(topEntity, addedOrRemoved))
							{
								SendRemovedEntity(topEntity.EntityId, addedOrRemoved);
								MyCubeGrid myCubeGrid;
								if ((myCubeGrid = topEntity as MyCubeGrid) != null)
								{
									foreach (MyShipController fatBlock in myCubeGrid.GetFatBlocks<MyShipController>())
									{
										if (!(fatBlock is MyRemoteControl) && fatBlock.Pilot != null && fatBlock.Pilot != topEntity && RemoveEntityInternal(fatBlock.Pilot, addedOrRemoved))
										{
											SendRemovedEntity(fatBlock.Pilot.EntityId, addedOrRemoved);
										}
									}
								}
							}
							topEntity.OnClose -= entity_OnClose;
						}
					}
				}
			}, "Phantom leave");
		}

		private void SendInsertedEntity(long entityId, bool addedOrRemoved)
		{
			if (base.IsReadyForReplication)
			{
				MyMultiplayer.RaiseEvent(this, (MySafeZone x) => x.InsertEntity_Implementation, entityId, addedOrRemoved);
			}
		}

		private void SendInsertedEntities(List<long> list)
		{
			if (base.IsReadyForReplication)
			{
				MyMultiplayer.RaiseEvent(this, (MySafeZone x) => x.InsertEntities_Implementation, list);
			}
		}

		private void SendRemovedEntity(long entityId, bool addedOrRemoved)
		{
			if (base.IsReadyForReplication)
			{
				MyMultiplayer.RaiseEvent(this, (MySafeZone x) => x.RemoveEntity_Implementation, entityId, addedOrRemoved);
			}
		}

<<<<<<< HEAD
		[Event(null, 896)]
=======
		[Event(null, 920)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[BroadcastExcept]
		private void InsertEntity_Implementation(long entityId, bool addedOrRemoved)
		{
			MySessionComponentSafeZones.EntityToSafezonesCache.Add(entityId, this);
			if (!m_containedEntities.Contains(entityId))
			{
				m_containedEntities.Add(entityId);
				if (MyEntities.TryGetEntityById(entityId, out var entity))
				{
					UpdatePlayerNotification(entity, addedOrRemoved);
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 912)]
=======
		[Event(null, 935)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[BroadcastExcept]
		private void InsertEntities_Implementation(List<long> list)
		{
			foreach (long item in list)
			{
				InsertEntity_Implementation(item, addedOrRemoved: false);
			}
		}

<<<<<<< HEAD
		[Event(null, 921)]
=======
		[Event(null, 944)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[BroadcastExcept]
		private void RemoveEntity_Implementation(long entityId, bool addedOrRemoved)
		{
			MySessionComponentSafeZones.EntityToSafezonesCache.Remove(entityId, this);
			if (m_containedEntities.Contains(entityId))
			{
				m_containedEntities.Remove(entityId);
				if (MyEntities.TryGetEntityById(entityId, out var entity))
				{
					RemoveEntityLocal(entity, addedOrRemoved);
				}
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (m_isAnimating)
			{
				TimeSpan timeSpan = m_blendTimer - TimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
				if (timeSpan.Ticks < 0)
				{
					m_isAnimating = false;
					MyRenderComponentSafeZone myRenderComponentSafeZone;
					if ((myRenderComponentSafeZone = base.Render as MyRenderComponentSafeZone) != null)
					{
						myRenderComponentSafeZone.RemoveTransitionObject();
					}
				}
				else
				{
					float amount = 1f - (float)(timeSpan.TotalMilliseconds / (double)(float)m_safeZoneSettings.EnableAnimationTimeMs);
					Color value = (Enabled ? ModelColor : Color.White);
					Color value2 = (Enabled ? Color.White : ModelColor);
					m_animatedColor = Color.Lerp(Color.Black, value, amount);
					RefreshGraphics();
					MyRenderComponentSafeZone myRenderComponentSafeZone2;
					if ((myRenderComponentSafeZone2 = base.Render as MyRenderComponentSafeZone) != null)
					{
						Color color = Color.Lerp(value2, Color.Black, amount);
						myRenderComponentSafeZone2.UpdateTransitionObjColor(color);
					}
				}
			}
			if (!Sync.IsServer || !Enabled)
			{
				return;
			}
			foreach (long containedEntity in m_containedEntities)
			{
				if (!MyEntities.TryGetEntityById(containedEntity, out var entity) || entity.Physics.IsKinematic || entity.Physics.IsStatic || IsSafe(entity))
				{
					continue;
				}
				MyAmmoBase myAmmoBase = entity as MyAmmoBase;
				if (myAmmoBase != null)
				{
					myAmmoBase.MarkForDestroy();
					continue;
				}
				MyMeteor myMeteor;
				if ((myMeteor = entity as MyMeteor) != null)
				{
					myMeteor.GameLogic.MarkForClose();
					continue;
<<<<<<< HEAD
				}
				Vector3D vector3D = entity.PositionComp.GetPosition() - base.PositionComp.GetPosition();
				if (vector3D.LengthSquared() > 0.10000000149011612)
				{
					vector3D.Normalize();
				}
=======
				}
				Vector3D vector3D = entity.PositionComp.GetPosition() - base.PositionComp.GetPosition();
				if (vector3D.LengthSquared() > 0.10000000149011612)
				{
					vector3D.Normalize();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				else
				{
					vector3D = Vector3.Up;
				}
<<<<<<< HEAD
				Vector3 value3 = vector3D * entity.Physics.Mass * 1000.0;
				entity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, value3, null, null);
=======
				Vector3D vector3D2 = vector3D * entity.Physics.Mass * 1000.0;
				entity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, vector3D2, null, null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private bool IsSafe(MyEntity entity)
		{
			MyFloatingObject obj = entity as MyFloatingObject;
			MyInventoryBagEntity myInventoryBagEntity = entity as MyInventoryBagEntity;
			if (obj != null || myInventoryBagEntity != null)
			{
				if (Entities.Contains(entity.EntityId))
				{
					return AccessTypeFloatingObjects == MySafeZoneAccess.Whitelist;
				}
				return AccessTypeFloatingObjects != MySafeZoneAccess.Whitelist;
			}
			MyEntity topMostParent = entity.GetTopMostParent();
			IMyComponentOwner<MyIDModule> myComponentOwner = topMostParent as IMyComponentOwner<MyIDModule>;
			if (myComponentOwner != null && myComponentOwner.GetComponent(out var component))
			{
				ulong num = MySession.Static.Players.TryGetSteamId(component.Owner);
				if (num != 0L && CheckAdminIgnoreSafezones(num))
				{
					return true;
				}
				if (AccessTypePlayers == MySafeZoneAccess.Whitelist)
				{
					if (Players.Contains(component.Owner))
					{
						return true;
					}
				}
				else if (Players.Contains(component.Owner))
				{
					return false;
				}
				MyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(component.Owner) as MyFaction;
				if (myFaction != null)
				{
					if (AccessTypeFactions == MySafeZoneAccess.Whitelist)
					{
						if (Factions.Contains(myFaction))
						{
							return true;
						}
					}
					else if (Factions.Contains(myFaction))
					{
						return false;
					}
				}
				return AccessTypePlayers == MySafeZoneAccess.Blacklist;
			}
			MyCubeGrid myCubeGrid = topMostParent as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyGroupsBase<MyCubeGrid> groups = MyCubeGridGroups.Static.GetGroups(GridLinkTypeEnum.Mechanical);
				SubgridCheckResult subgridCheckResult = SubgridCheckResult.NotSafe;
				foreach (MyCubeGrid groupNode in groups.GetGroupNodes(myCubeGrid))
				{
					SubgridCheckResult subgridCheckResult2 = IsSubGridSafe(groupNode);
					switch (subgridCheckResult2)
					{
					case SubgridCheckResult.NotSafe:
						if (subgridCheckResult != SubgridCheckResult.Admin)
						{
							subgridCheckResult = SubgridCheckResult.NotSafe;
						}
						break;
					case SubgridCheckResult.NeedExtraCheck:
					case SubgridCheckResult.Safe:
					case SubgridCheckResult.Admin:
						if (subgridCheckResult2 > subgridCheckResult)
						{
							subgridCheckResult = subgridCheckResult2;
						}
						break;
					}
				}
				return subgridCheckResult >= SubgridCheckResult.Safe;
			}
			if ((entity is MyAmmoBase || entity is MyMeteor) && (AllowedActions & MySafeZoneAction.Shooting) == 0)
			{
				return false;
			}
			return true;
		}

		private SubgridCheckResult IsSubGridSafe(MyCubeGrid cubeGrid)
		{
			if (cubeGrid.BigOwners != null && cubeGrid.BigOwners.Count > 0)
			{
				foreach (long bigOwner in cubeGrid.BigOwners)
				{
					ulong num = MySession.Static.Players.TryGetSteamId(bigOwner);
					if (num != 0L && CheckAdminIgnoreSafezones(num))
					{
						return SubgridCheckResult.Admin;
					}
				}
			}
			if (AccessTypeGrids == MySafeZoneAccess.Whitelist)
			{
				if (Entities.Contains(cubeGrid.EntityId))
				{
					return SubgridCheckResult.Safe;
				}
			}
			else if (Entities.Contains(cubeGrid.EntityId))
			{
				return SubgridCheckResult.NeedExtraCheck;
			}
			if (cubeGrid.BigOwners.Count > 0)
			{
				foreach (long bigOwner2 in cubeGrid.BigOwners)
				{
					MyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(bigOwner2) as MyFaction;
					if (myFaction != null)
					{
						if (AccessTypeFactions == MySafeZoneAccess.Whitelist)
						{
							return (!Factions.Contains(myFaction)) ? SubgridCheckResult.NeedExtraCheck : SubgridCheckResult.Safe;
						}
						return (!Factions.Contains(myFaction)) ? SubgridCheckResult.Safe : SubgridCheckResult.NotSafe;
					}
				}
<<<<<<< HEAD
			}
			if (AccessTypeGrids != MySafeZoneAccess.Blacklist)
=======
				return AccessTypeGrids == MySafeZoneAccess.Blacklist;
			}
			if ((entity is MyAmmoBase || entity is MyMeteor) && (AllowedActions & MySafeZoneAction.Shooting) == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return SubgridCheckResult.NeedExtraCheck;
			}
			return SubgridCheckResult.Safe;
		}

		public static bool CheckAdminIgnoreSafezones(ulong id)
		{
			AdminSettingsEnum adminSettingsEnum = AdminSettingsEnum.None;
			if (id == Sync.MyId)
			{
				adminSettingsEnum = MySession.Static.AdminSettings;
			}
			else if (MySession.Static.RemoteAdminSettings.ContainsKey(id))
			{
				adminSettingsEnum = MySession.Static.RemoteAdminSettings[id];
			}
			if ((adminSettingsEnum & AdminSettingsEnum.IgnoreSafeZones) != 0)
			{
				return true;
			}
			return false;
		}

		public bool IsActionAllowed(MyEntity entity, MySafeZoneAction action, long sourceEntityId = 0L)
		{
			if (!Enabled)
			{
				return true;
			}
			if (entity == null)
			{
				return false;
			}
			if (!m_containedEntities.Contains(entity.EntityId))
			{
				return true;
			}
			if (sourceEntityId != 0L && MyEntities.TryGetEntityById(sourceEntityId, out var entity2) && !IsSafe(entity2.GetTopMostParent()))
			{
				return false;
			}
			return AllowedActions.HasFlag(action);
		}

		private bool IsOutside(BoundingBoxD aabb)
		{
			bool flag = false;
			if (Shape == MySafeZoneShape.Sphere)
			{
				return !new BoundingSphereD(base.PositionComp.GetPosition(), Radius).Intersects(aabb);
			}
			return !new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrixRef).Intersects(ref aabb);
		}

		private bool IsOutside(MyEntity entity)
		{
			bool flag = false;
			MyOrientedBoundingBoxD other = new MyOrientedBoundingBoxD(entity.PositionComp.LocalAABB, entity.PositionComp.WorldMatrixRef);
			if (Shape == MySafeZoneShape.Sphere)
			{
				BoundingSphereD sphere = new BoundingSphereD(base.PositionComp.GetPosition(), Radius);
				return !other.Intersects(ref sphere);
			}
			return !new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrixRef).Intersects(ref other);
		}

		public bool IsEntityInsideAlone(long entityId)
		{
			int num = 0;
			foreach (long containedEntity in m_containedEntities)
			{
				MyEntity entity = null;
				MyEntities.TryGetEntityById(containedEntity, out entity);
				if (!(entity is MyVoxelPhysics))
				{
					num++;
				}
			}
			if (num == 1)
			{
				return m_containedEntities.Contains(entityId);
			}
			return false;
		}

		public bool IsEmpty()
		{
			return m_containedEntities.Count == 0;
		}

		public bool IsActionAllowed(BoundingBoxD aabb, MySafeZoneAction action, long sourceEntityId = 0L)
		{
			if (!Enabled)
			{
				return true;
			}
			if (IsOutside(aabb))
			{
				return true;
			}
			if (sourceEntityId != 0L && MyEntities.TryGetEntityById(sourceEntityId, out var entity) && !IsSafe(entity.GetTopMostParent()))
			{
				return false;
			}
			return (AllowedActions & action) == action;
		}

		public bool IsActionAllowed(Vector3D point, MySafeZoneAction action, long sourceEntityId = 0L)
		{
			if (!Enabled)
			{
				return true;
			}
			bool flag = false;
			if ((Shape != 0) ? (!new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrixRef).Contains(ref point)) : (new BoundingSphereD(base.PositionComp.GetPosition(), Radius).Contains(point) != ContainmentType.Contains))
			{
				return true;
			}
			if (sourceEntityId != 0L && MyEntities.TryGetEntityById(sourceEntityId, out var entity) && !IsSafe(entity.GetTopMostParent()))
			{
				return false;
			}
			return (AllowedActions & action) == action;
		}

		public void AddContainedToList()
		{
			foreach (long containedEntity in m_containedEntities)
			{
				if (!MyEntities.TryGetEntityById(containedEntity, out var entity))
				{
					continue;
				}
				IMyComponentOwner<MyIDModule> myComponentOwner = entity as IMyComponentOwner<MyIDModule>;
				if (myComponentOwner != null && myComponentOwner.GetComponent(out var component))
				{
					if (!Players.Contains(component.Owner))
					{
						Players.Add(component.Owner);
					}
				}
				else if (!Entities.Contains(entity.EntityId))
				{
					Entities.Add(entity.EntityId);
				}
			}
		}

		public override bool GetIntersectionWithLine(ref LineD line, out Vector3D? v, bool useCollisionModel = true, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			v = null;
			RayD ray = new RayD(line.From, line.Direction);
			if (Shape == MySafeZoneShape.Sphere)
			{
				if (new BoundingSphereD(base.PositionComp.GetPosition(), Radius).IntersectRaySphere(ray, out var tmin, out var _))
				{
					v = line.From + line.Direction * tmin;
					return line.Length >= tmin;
				}
			}
			else
			{
				double? num = new MyOrientedBoundingBoxD(base.PositionComp.LocalAABB, base.PositionComp.WorldMatrixRef).Intersects(ref ray);
				if (num.HasValue && num.Value <= line.Length)
				{
					v = line.From + line.Direction * num.Value;
					return true;
				}
			}
			return false;
		}

		public bool GetDetectorIntersectionWithLine(ref LineD line, out Vector3D? hit)
		{
			return GetIntersectionWithLine(ref line, out hit, useCollisionModel: true, IntersectionFlags.ALL_TRIANGLES);
		}
	}
}
