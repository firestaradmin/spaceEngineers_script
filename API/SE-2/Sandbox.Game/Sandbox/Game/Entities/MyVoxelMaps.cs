using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
<<<<<<< HEAD
using Sandbox.Game.Gui;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.Voxels;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public class MyVoxelMaps : IMyVoxelMaps
	{
		private readonly Dictionary<long, MyVoxelBase> m_voxelMapsByEntityId = new Dictionary<long, MyVoxelBase>();

		private readonly List<MyVoxelBase> m_tmpVoxelMapsList = new List<MyVoxelBase>();

		private static MyShapeBox m_boxVoxelShape = new MyShapeBox();

		private static MyShapeCapsule m_capsuleShape = new MyShapeCapsule();

		private static MyShapeSphere m_sphereShape = new MyShapeSphere();

		private static MyShapeRamp m_rampShape = new MyShapeRamp();

		private static readonly List<MyVoxelBase> m_voxelsTmpStorage = new List<MyVoxelBase>();

		public DictionaryValuesReader<long, MyVoxelBase> Instances => m_voxelMapsByEntityId;

		int IMyVoxelMaps.VoxelMaterialCount => MyDefinitionManager.Static.VoxelMaterialCount;

		public void Clear()
		{
			foreach (KeyValuePair<long, MyVoxelBase> item in m_voxelMapsByEntityId)
			{
				item.Value.Close();
			}
			m_voxelMapsByEntityId.Clear();
		}

		public bool Exist(MyVoxelBase voxelMap)
		{
			return m_voxelMapsByEntityId.ContainsKey(voxelMap.EntityId);
		}

		public void RemoveVoxelMap(MyVoxelBase voxelMap)
		{
			m_voxelMapsByEntityId.Remove(voxelMap.EntityId);
		}

		public MyVoxelBase GetOverlappingWithSphere(ref BoundingSphereD sphere)
		{
			MyVoxelBase result = null;
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref sphere, m_tmpVoxelMapsList);
			foreach (MyVoxelBase tmpVoxelMaps in m_tmpVoxelMapsList)
			{
				if (tmpVoxelMaps.DoOverlapSphereTest((float)sphere.Radius, sphere.Center))
				{
					result = tmpVoxelMaps;
					break;
				}
			}
			m_tmpVoxelMapsList.Clear();
			return result;
		}

		public void GetAllOverlappingWithSphere(ref BoundingSphereD sphere, List<MyVoxelBase> voxels)
		{
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref sphere, voxels);
		}

		public List<MyVoxelBase> GetAllOverlappingWithSphere(ref BoundingSphereD sphere)
		{
			List<MyVoxelBase> result = new List<MyVoxelBase>();
			MyGamePruningStructure.GetAllVoxelMapsInSphere(ref sphere, result);
			return result;
		}

		public void Add(MyVoxelBase voxelMap)
		{
			if (!Exist(voxelMap))
			{
				m_voxelMapsByEntityId.Add(voxelMap.EntityId, voxelMap);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Return reference to voxel map that intersects the box. If not voxel map found, null is returned.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyVoxelBase GetVoxelMapWhoseBoundingBoxIntersectsBox(ref BoundingBoxD boundingBox, MyVoxelBase ignoreVoxelMap)
		{
			MyVoxelBase result = null;
			double num = double.MaxValue;
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (!value.MarkedForClose && !value.Closed && value != ignoreVoxelMap && value.IsBoxIntersectingBoundingBoxOfThisVoxelMap(ref boundingBox))
				{
					double num2 = Vector3D.DistanceSquared(value.PositionComp.WorldAABB.Center, boundingBox.Center);
					if (num2 < num)
					{
						num = num2;
						result = value;
					}
				}
			}
			return result;
		}

		public bool GetVoxelMapsWhoseBoundingBoxesIntersectBox(ref BoundingBoxD boundingBox, MyVoxelBase ignoreVoxelMap, List<MyVoxelBase> voxelList)
		{
			int num = 0;
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (!value.MarkedForClose && !value.Closed && value != ignoreVoxelMap && value.IsBoxIntersectingBoundingBoxOfThisVoxelMap(ref boundingBox))
				{
					voxelList.Add(value);
					num++;
				}
			}
			return num > 0;
		}

		public MyVoxelBase TryGetVoxelMapByNameStart(string name)
		{
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (value.StorageName != null && value.StorageName.StartsWith(name))
				{
					return value;
				}
			}
			return null;
		}

		public MyVoxelBase TryGetVoxelMapByName(string name)
		{
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (value.StorageName == name)
				{
					return value;
				}
			}
			return null;
		}

		public MyVoxelBase TryGetVoxelBaseById(long id)
		{
			if (!m_voxelMapsByEntityId.ContainsKey(id))
			{
				return null;
			}
			return m_voxelMapsByEntityId[id];
		}

		public Dictionary<string, byte[]> GetVoxelMapsArray(bool includeChanged)
		{
			Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (value.Storage != null && (includeChanged || (!value.ContentChanged && !value.BeforeContentChanged)) && value.Save && !dictionary.ContainsKey(value.StorageName))
				{
					byte[] outCompressedData = null;
					value.Storage.Save(out outCompressedData);
					dictionary.Add(value.StorageName, outCompressedData);
				}
			}
			return dictionary;
		}

		public Dictionary<string, byte[]> GetVoxelMapsData(bool includeChanged, bool compressed, Dictionary<string, VRage.Game.Voxels.IMyStorage> voxelStorageNameCache = null)
		{
			Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (value.Storage == null || (!includeChanged && (value.ContentChanged || value.BeforeContentChanged)) || !value.Save || dictionary.ContainsKey(value.StorageName))
				{
					continue;
				}
				byte[] outCompressedData = null;
				if (value.Storage.AreDataCached)
				{
					if (compressed != value.Storage.AreDataCachedCompressed)
					{
						continue;
					}
					value.Storage.Save(out outCompressedData);
				}
				else
				{
					if (compressed)
					{
						continue;
					}
					outCompressedData = value.Storage.GetVoxelData();
				}
				dictionary.Add(value.StorageName, outCompressedData);
				voxelStorageNameCache?.Add(value.StorageName, value.Storage);
			}
			return dictionary;
		}

		public void DebugDraw(MyVoxelDebugDrawMode drawMode)
		{
			foreach (MyVoxelBase value in m_voxelMapsByEntityId.Values)
			{
				if (!(value is MyVoxelPhysics))
				{
					MatrixD worldMatrix = value.WorldMatrix;
					worldMatrix.Translation = value.PositionLeftBottomCorner;
					value.Storage.DebugDraw(ref worldMatrix, drawMode);
				}
			}
		}

		public void GetCacheStats(out int cachedChuncks, out int pendingCachedChuncks)
		{
			cachedChuncks = (pendingCachedChuncks = 0);
			foreach (KeyValuePair<long, MyVoxelBase> item in m_voxelMapsByEntityId)
			{
				if (!(item.Value is MyVoxelPhysics))
				{
					MyOctreeStorage myOctreeStorage = item.Value.Storage as MyOctreeStorage;
					if (myOctreeStorage != null)
					{
						cachedChuncks += myOctreeStorage.CachedChunksCount;
						pendingCachedChuncks += myOctreeStorage.PendingCachedChunksCount;
					}
				}
			}
		}

		internal void GetAllIds(ref List<long> list)
		{
			foreach (long key in m_voxelMapsByEntityId.Keys)
			{
				list.Add(key);
			}
		}

		void IMyVoxelMaps.Clear()
		{
			Clear();
		}

		bool IMyVoxelMaps.Exist(IMyVoxelBase voxelMap)
		{
			return Exist(voxelMap as MyVoxelBase);
		}

		IMyVoxelBase IMyVoxelMaps.GetOverlappingWithSphere(ref BoundingSphereD sphere)
		{
			m_voxelsTmpStorage.Clear();
			GetAllOverlappingWithSphere(ref sphere, m_voxelsTmpStorage);
			if (m_voxelsTmpStorage.Count == 0)
			{
				return null;
			}
			return m_voxelsTmpStorage[0];
		}

		IMyVoxelBase IMyVoxelMaps.GetVoxelMapWhoseBoundingBoxIntersectsBox(ref BoundingBoxD boundingBox, IMyVoxelBase ignoreVoxelMap)
		{
			return GetVoxelMapWhoseBoundingBoxIntersectsBox(ref boundingBox, ignoreVoxelMap as MyVoxelBase);
		}

		void IMyVoxelMaps.GetInstances(List<IMyVoxelBase> voxelMaps, Func<IMyVoxelBase, bool> collect)
		{
			foreach (MyVoxelBase instance in Instances)
			{
				if (collect == null || collect(instance))
				{
					voxelMaps.Add(instance);
				}
			}
		}

		VRage.ModAPI.IMyStorage IMyVoxelMaps.CreateStorage(Vector3I size)
		{
			return new MyOctreeStorage(null, size);
		}

		IMyVoxelMap IMyVoxelMaps.CreateVoxelMap(string storageName, VRage.ModAPI.IMyStorage storage, Vector3D position, long voxelMapId)
		{
			MyVoxelMap myVoxelMap = new MyVoxelMap();
			myVoxelMap.EntityId = voxelMapId;
			myVoxelMap.Init(storageName, storage as VRage.Game.Voxels.IMyStorage, position);
			MyEntities.Add(myVoxelMap);
			return myVoxelMap;
		}

		IMyVoxelMap IMyVoxelMaps.CreateVoxelMapFromStorageName(string storageName, string prefabVoxelMapName, Vector3D position)
		{
			MyStorageBase myStorageBase = MyStorageBase.LoadFromFile(MyWorldGenerator.GetVoxelPrefabPath(prefabVoxelMapName));
			if (myStorageBase == null)
			{
				return null;
			}
			myStorageBase.DataProvider = MyCompositeShapeProvider.CreateAsteroidShape(0, (float)myStorageBase.Size.AbsMax() * 1f);
			return MyWorldGenerator.AddVoxelMap(storageName, myStorageBase, position, 0L);
		}

		IMyVoxelMap IMyVoxelMaps.CreatePredefinedVoxelMap(string storageName, string voxelMaterial, MatrixD matrix, bool useVoxelOffset)
		{
			if (!MySession.Static.IsServer)
			{
				return null;
			}
			MyStorageBase myStorageBase = MyGuiScreenDebugSpawnMenu.CreatePredefinedDataStorage(storageName, voxelMaterial);
			if (myStorageBase == null)
			{
				return null;
			}
			return MyWorldGenerator.AddVoxelMap(storageName, myStorageBase, matrix, 0L, lazyPhysics: false, useVoxelOffset);
		}

		IMyVoxelMap IMyVoxelMaps.CreateProceduralVoxelMap(int seed, float radius, MatrixD matrix)
		{
			if (!MySession.Static.IsServer)
			{
				return null;
			}
			string text = MyGuiScreenDebugSpawnMenu.MakeStorageName("ProcAsteroid-" + seed + "r" + radius);
			MyStorageBase myStorageBase = MyGuiScreenDebugSpawnMenu.CreateProceduralAsteroidStorage(seed, radius);
			if (myStorageBase == null)
			{
				return null;
			}
			MyVoxelMap myVoxelMap = new MyVoxelMap();
			myVoxelMap.CreatedByUser = false;
			myVoxelMap.Save = true;
			myVoxelMap.AsteroidName = text;
			myVoxelMap.Init(text, myStorageBase, matrix.Translation - myStorageBase.Size * 0.5f);
			myVoxelMap.WorldMatrix = matrix;
			MyEntities.Add(myVoxelMap);
			MyEntities.RaiseEntityCreated(myVoxelMap);
			return myVoxelMap;
		}

		VRage.ModAPI.IMyStorage IMyVoxelMaps.CreateStorage(byte[] data)
		{
			bool isOldFormat = false;
			return MyStorageBase.Load(data, out isOldFormat);
		}

		IMyVoxelShapeBox IMyVoxelMaps.GetBoxVoxelHand()
		{
			return m_boxVoxelShape;
		}

		IMyVoxelShapeCapsule IMyVoxelMaps.GetCapsuleVoxelHand()
		{
			return m_capsuleShape;
		}

		IMyVoxelShapeSphere IMyVoxelMaps.GetSphereVoxelHand()
		{
			return m_sphereShape;
		}

		IMyVoxelShapeRamp IMyVoxelMaps.GetRampVoxelHand()
		{
			return m_rampShape;
		}

		void IMyVoxelMaps.PaintInShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape, byte materialIdx)
		{
			MyVoxelGenerator.RequestPaintInShape(voxelMap, voxelShape, materialIdx);
		}

		void IMyVoxelMaps.CutOutShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape)
		{
			MyVoxelGenerator.RequestCutOutShape(voxelMap, voxelShape);
		}

		void IMyVoxelMaps.FillInShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape, byte materialIdx)
		{
			MyVoxelGenerator.RequestFillInShape(voxelMap, voxelShape, materialIdx);
		}

		void IMyVoxelMaps.RevertShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape)
		{
			MyVoxelGenerator.RequestRevertShape(voxelMap, voxelShape);
		}

		void IMyVoxelMaps.MakeCrater(IMyVoxelBase voxelMap, BoundingSphereD sphere, Vector3 direction, byte materialIdx)
		{
			MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(materialIdx);
			MyVoxelGenerator.MakeCrater((MyVoxelBase)voxelMap, sphere, direction, voxelMaterialDefinition);
		}

		IMyVoxelBase IMyVoxelMaps.SpawnPlanet(string planetName, float size, int seed, Vector3D pos)
		{
			if (!MySession.Static.IsServer)
			{
				return null;
			}
			string text = planetName + "-" + seed + "d" + size;
			MyGuiScreenDebugSpawnMenu.MakeStorageName(text);
			long entityId = MyRandom.Instance.NextLong();
			MyPlanet myPlanet = MyWorldGenerator.AddPlanet(text, planetName, planetName, pos, seed, size, fadeIn: true, entityId, addGPS: false, userCreated: true);
			if (myPlanet == null)
			{
				return null;
			}
			MyGuiScreenDebugSpawnMenu.SpawnPlanetClientModApi(planetName, text, size, seed, pos, entityId);
			return myPlanet;
		}
	}
}
