using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ParallelTasks;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 800)]
	[StaticEventOwner]
	public class MyFloatingObjects : MySessionComponentBase
	{
		private class MyFloatingObjectComparer : IEqualityComparer<MyFloatingObject>
		{
			public bool Equals(MyFloatingObject x, MyFloatingObject y)
			{
				return x.EntityId == y.EntityId;
			}

			public int GetHashCode(MyFloatingObject obj)
			{
				return (int)obj.EntityId;
			}
		}

		private class MyFloatingObjectTimestampComparer : IComparer<MyFloatingObject>
		{
			public int Compare(MyFloatingObject x, MyFloatingObject y)
			{
				if (x.CreationTime != y.CreationTime)
				{
					return y.CreationTime.CompareTo(x.CreationTime);
				}
				return y.EntityId.CompareTo(x.EntityId);
			}
		}

		private class MyFloatingObjectsSynchronizationComparer : IComparer<MyFloatingObject>
		{
			public int Compare(MyFloatingObject x, MyFloatingObject y)
			{
				return x.ClosestDistanceToAnyPlayerSquared.CompareTo(y.ClosestDistanceToAnyPlayerSquared);
			}
		}

		private struct StabilityInfo
		{
			public MyPositionAndOrientation PositionAndOr;

			public StabilityInfo(MyPositionAndOrientation posAndOr)
			{
				PositionAndOr = posAndOr;
			}
		}

		protected sealed class RequestSpawnCreative_Implementation_003C_003EVRage_Game_MyObjectBuilder_FloatingObject : ICallSite<IMyEventOwner, MyObjectBuilder_FloatingObject, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_FloatingObject obj, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestSpawnCreative_Implementation(obj);
			}
		}

		private static MyFloatingObjectComparer m_entityComparer = new MyFloatingObjectComparer();

		private static MyFloatingObjectTimestampComparer m_comparer = new MyFloatingObjectTimestampComparer();

		private static SortedSet<MyFloatingObject> m_floatingOres = new SortedSet<MyFloatingObject>((IComparer<MyFloatingObject>)m_comparer);

		private static SortedSet<MyFloatingObject> m_floatingItems = new SortedSet<MyFloatingObject>((IComparer<MyFloatingObject>)m_comparer);

		private static List<MyVoxelBase> m_tmpResultList = new List<MyVoxelBase>();

		private static List<MyFloatingObject> m_synchronizedFloatingObjects = new List<MyFloatingObject>();

		private static List<MyFloatingObject> m_floatingObjectsToSyncCreate = new List<MyFloatingObject>();

		private static MyFloatingObjectsSynchronizationComparer m_synchronizationComparer = new MyFloatingObjectsSynchronizationComparer();

		private static int m_updateCounter = 0;

		private static int m_checkObjectInsideVoxel = 0;

		private static List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>> m_itemsToSpawnNextUpdate = new List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>>();

		private static readonly MyConcurrentPool<List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>>> m_itemsToSpawnPool = new MyConcurrentPool<List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>>>();

		public override Type[] Dependencies => new Type[1] { typeof(MyDebris) };

		public static int FloatingOreCount => m_floatingOres.get_Count();

		public static int FloatingItemCount => m_floatingItems.get_Count();

		internal static bool CanSpawn(MyPhysicalInventoryItem ob)
		{
			if (!MySession.Static.SimplifiedSimulation)
			{
				return true;
			}
			if (!(ob.Content is MyObjectBuilder_Component) && !(ob.Content is MyObjectBuilder_GasContainerObject))
			{
				return ob.Content is MyObjectBuilder_PhysicalGunObject;
			}
			return true;
		}

		public override void LoadData()
		{
			base.LoadData();
			MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(0);
			Spawn(new MyPhysicalInventoryItem(1, MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>(voxelMaterialDefinition.MinedOre)), default(BoundingSphereD), null, voxelMaterialDefinition, delegate(MyEntity item)
			{
				item.Close();
			});
		}

		public override void UpdateAfterSimulation()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			CheckObjectInVoxel();
			m_updateCounter++;
			if (m_updateCounter > 100)
			{
				ReduceFloatingObjects();
			}
			if (m_itemsToSpawnNextUpdate.Count > 0)
			{
				List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>> tmp = m_itemsToSpawnNextUpdate;
				m_itemsToSpawnNextUpdate = m_itemsToSpawnPool.Get();
				if (MySandboxGame.Config.SyncRendering)
				{
					MyEntityIdentifier.PrepareSwapData();
					MyEntityIdentifier.SwapPerThreadData();
				}
				Parallel.Start(delegate
				{
					SpawnInventoryItems(tmp);
					tmp.Clear();
					m_itemsToSpawnPool.Return(tmp);
				});
				if (MySandboxGame.Config.SyncRendering)
				{
					MyEntityIdentifier.ClearSwapDataAndRestore();
				}
			}
			base.UpdateAfterSimulation();
			if (m_updateCounter > 100)
			{
				OptimizeFloatingObjects();
			}
			if (m_updateCounter > 100)
			{
				m_updateCounter = 0;
			}
		}

		private void OptimizeFloatingObjects()
		{
			ReduceFloatingObjects();
			OptimizeQualityType();
		}

		private void CheckObjectInVoxel()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			if (m_checkObjectInsideVoxel >= m_synchronizedFloatingObjects.Count)
			{
				m_checkObjectInsideVoxel = 0;
			}
			if (m_synchronizedFloatingObjects.Count > 0)
			{
				MyFloatingObject myFloatingObject = m_synchronizedFloatingObjects[m_checkObjectInsideVoxel];
				BoundingBoxD aabb = myFloatingObject.PositionComp.LocalAABB;
				MatrixD aabbWorldTransform = myFloatingObject.PositionComp.WorldMatrixRef;
				BoundingBoxD box = myFloatingObject.PositionComp.WorldAABB;
				using (m_tmpResultList.GetClearToken())
				{
					MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, m_tmpResultList);
					bool flag = false;
					foreach (MyVoxelBase tmpResult in m_tmpResultList)
					{
						if (tmpResult != null && !tmpResult.MarkedForClose && !(tmpResult is MyVoxelPhysics) && tmpResult.AreAllAabbCornersInside(ref aabbWorldTransform, aabb))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						myFloatingObject.NumberOfFramesInsideVoxel++;
						if (myFloatingObject.NumberOfFramesInsideVoxel > 5)
						{
							RemoveFloatingObject(myFloatingObject);
						}
					}
					else
					{
						myFloatingObject.NumberOfFramesInsideVoxel = 0;
					}
				}
			}
			m_checkObjectInsideVoxel++;
		}

		/// <summary>
		/// Spawning of inventory items is delayed to UpdateAfterSimulation
		/// </summary>
		private void SpawnInventoryItems(List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>> itemsList)
		{
			for (int i = 0; i < itemsList.Count; i++)
			{
				Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D> item = itemsList[i];
				item.Item1.Spawn(item.Item1.Amount, item.Item2, null, delegate(MyEntity entity)
				{
					entity.Physics.LinearVelocity = item.Item3;
					entity.Physics.ApplyImpulse(MyUtils.GetRandomVector3Normalized() * entity.Physics.Mass / 5f, entity.PositionComp.GetPosition());
				});
			}
			itemsList.Clear();
		}

		public static void Spawn(MyPhysicalInventoryItem item, Vector3D position, Vector3D forward, Vector3D up, MyPhysicsComponentBase motionInheritedFrom = null, Action<MyEntity> completionCallback = null)
		{
			if (!MyEntities.IsInsideWorld(position))
			{
				return;
			}
			Vector3D forward2 = forward;
			Vector3D up2 = up;
			Vector3D vector3D = Vector3D.Cross(up, forward);
			MyPhysicalItemDefinition definition = null;
			if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalItemDefinition>(item.Content.GetObjectId(), out definition))
			{
				if (definition.RotateOnSpawnX)
				{
					forward2 = up;
					up2 = -forward;
				}
				if (definition.RotateOnSpawnY)
				{
					forward2 = vector3D;
				}
				if (definition.RotateOnSpawnZ)
				{
					up2 = -vector3D;
				}
			}
			Spawn(item, MatrixD.CreateWorld(position, forward2, up2), motionInheritedFrom, completionCallback);
		}

		public static void Spawn(MyPhysicalInventoryItem item, MatrixD worldMatrix, MyPhysicsComponentBase motionInheritedFrom, Action<MyEntity> completionCallback)
		{
			if (!CanSpawn(item) || !MyEntities.IsInsideWorld(worldMatrix.Translation))
			{
				return;
			}
			MyObjectBuilder_FloatingObject myObjectBuilder_FloatingObject = PrepareBuilder(ref item);
			myObjectBuilder_FloatingObject.PositionAndOrientation = new MyPositionAndOrientation(worldMatrix);
			MyEntities.CreateFromObjectBuilderParallel(myObjectBuilder_FloatingObject, addToScene: true, delegate(MyEntity entity)
			{
				if (entity != null && entity.Physics != null)
				{
					entity.Physics.ForceActivate();
					ApplyPhysics(entity, motionInheritedFrom);
					if (MyVisualScriptLogicProvider.ItemSpawned != null)
					{
						MyVisualScriptLogicProvider.ItemSpawned(item.Content.TypeId.ToString(), item.Content.SubtypeName, entity.EntityId, item.Amount.ToIntSafe(), worldMatrix.Translation);
					}
					if (completionCallback != null)
					{
						completionCallback(entity);
					}
				}
			});
		}

		internal static MyEntity Spawn(MyPhysicalInventoryItem item, BoundingBoxD box, MyPhysicsComponentBase motionInheritedFrom = null)
		{
			if (!CanSpawn(item))
			{
				return null;
			}
			MyEntity myEntity = MyEntities.CreateFromObjectBuilder(PrepareBuilder(ref item), fadeIn: false);
			if (myEntity != null)
			{
				float radius = myEntity.PositionComp.LocalVolume.Radius;
<<<<<<< HEAD
				Vector3D value = box.Size / 2.0 - new Vector3(radius);
				value = Vector3D.Max(value, Vector3D.Zero);
				box = new BoundingBoxD(box.Center - value, box.Center + value);
=======
				Vector3D vector3D = box.Size / 2.0 - new Vector3(radius);
				vector3D = Vector3.Max(vector3D, Vector3.Zero);
				box = new BoundingBoxD(box.Center - vector3D, box.Center + vector3D);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Vector3D randomPosition = MyUtils.GetRandomPosition(ref box);
				AddToPos(myEntity, randomPosition, motionInheritedFrom);
				myEntity.Physics.ForceActivate();
				if (MyVisualScriptLogicProvider.ItemSpawned != null)
				{
					MyVisualScriptLogicProvider.ItemSpawned(item.Content.TypeId.ToString(), item.Content.SubtypeName, myEntity.EntityId, item.Amount.ToIntSafe(), randomPosition);
				}
			}
			return myEntity;
		}

		public static void Spawn(MyPhysicalInventoryItem item, BoundingSphereD sphere, MyPhysicsComponentBase motionInheritedFrom, MyVoxelMaterialDefinition voxelMaterial, Action<MyEntity> OnDone)
		{
			if (!CanSpawn(item))
			{
				return;
			}
			MyEntities.CreateFromObjectBuilderParallel(PrepareBuilder(ref item), addToScene: false, delegate(MyEntity entity)
			{
				if (voxelMaterial.DamagedMaterial != MyStringHash.NullOrEmpty)
				{
					voxelMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition(voxelMaterial.DamagedMaterial.ToString());
				}
				((MyFloatingObject)entity).VoxelMaterial = voxelMaterial;
				float radius = entity.PositionComp.LocalVolume.Radius;
				double val = sphere.Radius - (double)radius;
				val = Math.Max(val, 0.0);
				sphere = new BoundingSphereD(sphere.Center, val);
				Vector3D randomBorderPosition = MyUtils.GetRandomBorderPosition(ref sphere);
				AddToPos(entity, randomBorderPosition, motionInheritedFrom);
				if (MyVisualScriptLogicProvider.ItemSpawned != null)
				{
					MyVisualScriptLogicProvider.ItemSpawned(item.Content.TypeId.ToString(), item.Content.SubtypeName, entity.EntityId, item.Amount.ToIntSafe(), randomBorderPosition);
				}
				OnDone(entity);
			});
		}

		public static void Spawn(MyPhysicalItemDefinition itemDefinition, Vector3D translation, Vector3D forward, Vector3D up, int amount = 1, float scale = 1f)
		{
			MyObjectBuilder_PhysicalObject content = MyObjectBuilderSerializer.CreateNewObject(itemDefinition.Id.TypeId, itemDefinition.Id.SubtypeName) as MyObjectBuilder_PhysicalObject;
			Spawn(new MyPhysicalInventoryItem(amount, content, scale), translation, forward, up);
		}

		public static void EnqueueInventoryItemSpawn(MyPhysicalInventoryItem inventoryItem, BoundingBoxD boundingBox, Vector3D inheritedVelocity)
		{
			m_itemsToSpawnNextUpdate.Add(Tuple.Create(inventoryItem, boundingBox, inheritedVelocity));
		}

		private static MyObjectBuilder_FloatingObject PrepareBuilder(ref MyPhysicalInventoryItem item)
		{
			MyObjectBuilder_FloatingObject myObjectBuilder_FloatingObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_FloatingObject>();
			myObjectBuilder_FloatingObject.Item = item.GetObjectBuilder();
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(item.Content);
			myObjectBuilder_FloatingObject.ModelVariant = (physicalItemDefinition.HasModelVariants ? MyUtils.GetRandomInt(physicalItemDefinition.Models.Length) : 0);
			myObjectBuilder_FloatingObject.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
			return myObjectBuilder_FloatingObject;
		}

		private static void AddToPos(MyEntity thrownEntity, Vector3D pos, MyPhysicsComponentBase motionInheritedFrom)
		{
			Vector3 randomVector3Normalized = MyUtils.GetRandomVector3Normalized();
			Vector3 randomVector3Normalized2 = MyUtils.GetRandomVector3Normalized();
			while (randomVector3Normalized == randomVector3Normalized2)
			{
				randomVector3Normalized2 = MyUtils.GetRandomVector3Normalized();
			}
			randomVector3Normalized2 = Vector3.Cross(Vector3.Cross(randomVector3Normalized, randomVector3Normalized2), randomVector3Normalized);
			thrownEntity.WorldMatrix = MatrixD.CreateWorld(pos, randomVector3Normalized, randomVector3Normalized2);
			MyEntities.Add(thrownEntity);
			ApplyPhysics(thrownEntity, motionInheritedFrom);
		}

		private static void ApplyPhysics(MyEntity thrownEntity, MyPhysicsComponentBase motionInheritedFrom)
		{
			if (thrownEntity.Physics != null && motionInheritedFrom != null)
			{
				thrownEntity.Physics.LinearVelocity = motionInheritedFrom.LinearVelocity;
				thrownEntity.Physics.AngularVelocity = motionInheritedFrom.AngularVelocity;
			}
		}

		private void OptimizeQualityType()
		{
			for (int i = 0; i < m_synchronizedFloatingObjects.Count; i++)
			{
				_ = m_synchronizedFloatingObjects[i];
			}
		}

		internal static void RegisterFloatingObject(MyFloatingObject obj)
		{
			if (obj != null && !obj.WasRemovedFromWorld)
			{
				obj.CreationTime = Stopwatch.GetTimestamp();
				if (obj.VoxelMaterial != null)
				{
					m_floatingOres.Add(obj);
				}
				else
				{
					m_floatingItems.Add(obj);
				}
				if (Sync.IsServer)
				{
					AddToSynchronization(obj);
				}
			}
		}

		internal static void UnregisterFloatingObject(MyFloatingObject obj)
		{
			if (obj.VoxelMaterial != null)
			{
				m_floatingOres.Remove(obj);
			}
			else
			{
				m_floatingItems.Remove(obj);
			}
			if (Sync.IsServer)
			{
				RemoveFromSynchronization(obj);
			}
			obj.WasRemovedFromWorld = true;
		}

		public static void AddFloatingObjectAmount(MyFloatingObject obj, MyFixedPoint amount)
		{
			MyPhysicalInventoryItem item = obj.Item;
			item.Amount += amount;
			obj.Item = item;
			obj.Amount.Value = item.Amount;
			obj.UpdateInternalState();
		}

		public static void RemoveFloatingObject(MyFloatingObject obj, bool sync)
		{
			if (sync)
			{
				if (Sync.IsServer)
				{
					RemoveFloatingObject(obj);
				}
				else
				{
					obj.SendCloseRequest();
				}
			}
			else
			{
				RemoveFloatingObject(obj);
			}
		}

		public static void RemoveFloatingObject(MyFloatingObject obj)
		{
			RemoveFloatingObject(obj, MyFixedPoint.MaxValue);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="amount">MyFixedPoint.MaxValue to remove object</param>
		internal static void RemoveFloatingObject(MyFloatingObject obj, MyFixedPoint amount)
		{
			if (!(amount <= 0))
			{
				if (amount < obj.Item.Amount)
				{
					obj.Amount.Value -= amount;
					obj.RefreshDisplayName();
				}
				else
				{
					obj.Render.FadeOut = false;
					obj.Close();
					obj.WasRemovedFromWorld = true;
				}
			}
		}

		public static void ReduceFloatingObjects()
		{
			int num = m_floatingOres.get_Count() + m_floatingItems.get_Count();
			int num2 = Math.Max(MySession.Static.MaxFloatingObjects / 5, 4);
			while (num > MySession.Static.MaxFloatingObjects)
			{
<<<<<<< HEAD
				SortedSet<MyFloatingObject> sortedSet = ((m_floatingOres.Count <= num2 && m_floatingItems.Count != 0) ? m_floatingItems : m_floatingOres);
				if (sortedSet.Count > 0)
=======
				SortedSet<MyFloatingObject> val = ((m_floatingOres.get_Count() <= num2 && m_floatingItems.get_Count() != 0) ? m_floatingItems : m_floatingOres);
				if (val.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyFloatingObject myFloatingObject = Enumerable.Last<MyFloatingObject>((IEnumerable<MyFloatingObject>)val);
					val.Remove(myFloatingObject);
					if (Sync.IsServer)
					{
						RemoveFloatingObject(myFloatingObject);
					}
				}
				num--;
			}
		}

		private static void AddToSynchronization(MyFloatingObject floatingObject)
		{
			m_floatingObjectsToSyncCreate.Add(floatingObject);
			m_synchronizedFloatingObjects.Add(floatingObject);
			floatingObject.OnClose += floatingObject_OnClose;
		}

		private static void floatingObject_OnClose(MyEntity obj)
		{
		}

		private static void RemoveFromSynchronization(MyFloatingObject floatingObject)
		{
			floatingObject.OnClose -= floatingObject_OnClose;
			m_synchronizedFloatingObjects.Remove(floatingObject);
			m_floatingObjectsToSyncCreate.Remove(floatingObject);
		}

		/// <summary>
		/// This is used mainly for compactibility issues, it takes the builder of an entity of old object representation and creates a floating object builder for it
		/// </summary>
		public static MyObjectBuilder_FloatingObject ChangeObjectBuilder(MyComponentDefinition componentDef, MyObjectBuilder_EntityBase entityOb)
		{
			MyObjectBuilder_PhysicalObject content = MyObjectBuilderSerializer.CreateNewObject(componentDef.Id.TypeId, componentDef.Id.SubtypeName) as MyObjectBuilder_PhysicalObject;
			Vector3 up = entityOb.PositionAndOrientation.Value.Up;
			Vector3 forward = entityOb.PositionAndOrientation.Value.Forward;
			Vector3D position = entityOb.PositionAndOrientation.Value.Position;
			MyPhysicalInventoryItem item = new MyPhysicalInventoryItem(1, content);
			MyObjectBuilder_FloatingObject myObjectBuilder_FloatingObject = PrepareBuilder(ref item);
			myObjectBuilder_FloatingObject.PositionAndOrientation = new MyPositionAndOrientation(position, forward, up);
			myObjectBuilder_FloatingObject.EntityId = entityOb.EntityId;
			return myObjectBuilder_FloatingObject;
		}

		/// <summary>
		/// Players are allowed to spawn any object in creative
		/// </summary>
		public static void RequestSpawnCreative(MyObjectBuilder_FloatingObject obj)
		{
			if (MySession.Static.HasCreativeRights || MySession.Static.CreativeMode)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestSpawnCreative_Implementation, obj);
			}
		}

<<<<<<< HEAD
		[Event(null, 659)]
=======
		[Event(null, 654)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void RequestSpawnCreative_Implementation(MyObjectBuilder_FloatingObject obj)
		{
			if (MySession.Static.CreativeMode || MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				MyEntities.CreateFromObjectBuilderAndAdd(obj, fadeIn: false);
			}
			else
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
		}
	}
}
