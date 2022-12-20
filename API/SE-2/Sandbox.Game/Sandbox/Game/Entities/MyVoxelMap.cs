using System.Text.RegularExpressions;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Voxels;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyEntityType(typeof(MyObjectBuilder_VoxelMap), true)]
	public class MyVoxelMap : MyVoxelBase, IMyVoxelMap, IMyVoxelBase, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
	{
		private class Sandbox_Game_Entities_MyVoxelMap_003C_003EActor : IActivator, IActivator<MyVoxelMap>
		{
			private sealed override object CreateInstance()
			{
				return new MyVoxelMap();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyVoxelMap CreateInstance()
			{
				return new MyVoxelMap();
			}

			MyVoxelMap IActivator<MyVoxelMap>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override VRage.Game.Voxels.IMyStorage Storage
		{
			get
			{
				return base.m_storage;
			}
			set
			{
				bool flag = false;
				if (base.m_storage != null)
				{
					flag = true;
					base.m_storage.RangeChanged -= storage_RangeChanged;
				}
				base.m_storage = value;
				base.m_storage.RangeChanged += storage_RangeChanged;
				m_storageMax = base.m_storage.Size;
				if (flag)
				{
					base.m_storage.NotifyChanged(m_storageMin, m_storageMax, MyStorageDataTypeFlags.ContentAndMaterial);
				}
			}
		}

		internal new MyVoxelPhysicsBody Physics
		{
			get
			{
				return base.Physics as MyVoxelPhysicsBody;
			}
			set
			{
				base.Physics = value;
			}
		}

		public override MyVoxelBase RootVoxel => this;

		public bool IsStaticForCluster
		{
			get
			{
				return Physics.IsStaticForCluster;
			}
			set
			{
				Physics.IsStaticForCluster = value;
			}
		}

		public MyVoxelMap()
		{
			((MyPositionComponent)base.PositionComp).WorldPositionChanged = base.WorldPositionChanged;
			base.Render = new MyRenderComponentVoxelMap();
			base.Render.DrawOutsideViewDistance = true;
			AddDebugRenderComponent(new MyDebugRenderComponentVoxelMap(this));
		}

		public override void Init(MyObjectBuilder_EntityBase builder)
		{
			MyObjectBuilder_VoxelMap myObjectBuilder_VoxelMap = (MyObjectBuilder_VoxelMap)builder;
			if (myObjectBuilder_VoxelMap == null)
			{
				return;
			}
			base.m_storage = MyStorageBase.Load(myObjectBuilder_VoxelMap.StorageName, cache: false);
			if (base.m_storage != null)
			{
				Init(builder, base.m_storage);
				if (myObjectBuilder_VoxelMap.ContentChanged.HasValue)
				{
					base.ContentChanged = myObjectBuilder_VoxelMap.ContentChanged.Value;
				}
				else
				{
					base.ContentChanged = true;
				}
			}
		}

		public override void Init(MyObjectBuilder_EntityBase builder, VRage.Game.Voxels.IMyStorage storage)
		{
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			base.SyncFlag = true;
			base.Init(builder);
			base.Init(null, null, null, null);
			MyObjectBuilder_VoxelMap myObjectBuilder_VoxelMap = (MyObjectBuilder_VoxelMap)builder;
			if (myObjectBuilder_VoxelMap == null)
			{
				return;
			}
			if (myObjectBuilder_VoxelMap.MutableStorage)
			{
				base.StorageName = myObjectBuilder_VoxelMap.StorageName;
			}
			else
			{
				base.StorageName = GetNewStorageName(myObjectBuilder_VoxelMap.StorageName, base.EntityId);
			}
			base.m_storage = storage;
			base.m_storage.RangeChanged += storage_RangeChanged;
			m_storageMax = base.m_storage.Size;
			base.CreatedByUser = myObjectBuilder_VoxelMap.CreatedByUser;
			if (myObjectBuilder_VoxelMap != null && myObjectBuilder_VoxelMap.BoulderItemId.HasValue && myObjectBuilder_VoxelMap.BoulderSectorId.HasValue && myObjectBuilder_VoxelMap.BoulderPlanetId.HasValue)
			{
				MyBoulderInformation value = default(MyBoulderInformation);
				value.PlanetId = myObjectBuilder_VoxelMap.BoulderPlanetId.Value;
				value.SectorId = myObjectBuilder_VoxelMap.BoulderSectorId.Value;
				value.ItemId = myObjectBuilder_VoxelMap.BoulderItemId.Value;
				BoulderInfo = value;
			}
			else if (MyFakes.ENABLE_BOULDER_NAME_PARSING && myObjectBuilder_VoxelMap != null && !string.IsNullOrEmpty(myObjectBuilder_VoxelMap.StorageName))
			{
<<<<<<< HEAD
				Match match = new Regex("P\\((.*)\\)S\\((\\d+)\\)A\\((.*)__(\\d+)\\)").Match(myObjectBuilder_VoxelMap.StorageName);
				if (match.Success)
				{
					GroupCollection groups = match.Groups;
					if (groups.Count >= 4)
					{
						bool flag = true;
						MyBoulderInformation value2 = default(MyBoulderInformation);
						MyPlanet myPlanet = MyEntities.GetEntityByName(groups[1].Value) as MyPlanet;
=======
				Match val = new Regex("P\\((.*)\\)S\\((\\d+)\\)A\\((.*)__(\\d+)\\)").Match(myObjectBuilder_VoxelMap.StorageName);
				if (((Group)val).get_Success())
				{
					GroupCollection groups = val.get_Groups();
					if (groups.get_Count() >= 4)
					{
						bool flag = true;
						MyBoulderInformation value2 = default(MyBoulderInformation);
						MyPlanet myPlanet = MyEntities.GetEntityByName(((Capture)groups.get_Item(1)).get_Value()) as MyPlanet;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (myPlanet != null)
						{
							value2.PlanetId = myPlanet.EntityId;
						}
						else
						{
							flag = false;
						}
<<<<<<< HEAD
						value2.SectorId = long.Parse(groups[2].Value);
						value2.ItemId = int.Parse(groups[4].Value);
=======
						value2.SectorId = long.Parse(((Capture)groups.get_Item(2)).get_Value());
						value2.ItemId = int.Parse(((Capture)groups.get_Item(4)).get_Value());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (flag)
						{
							BoulderInfo = value2;
						}
					}
				}
			}
			else
			{
				BoulderInfo = null;
			}
			InitVoxelMap(MatrixD.CreateWorld(myObjectBuilder_VoxelMap.PositionAndOrientation.Value.Position + Vector3D.TransformNormal((Vector3D)base.m_storage.Size / 2.0, base.WorldMatrix), base.WorldMatrix.Forward, base.WorldMatrix.Up), base.m_storage.Size);
		}

		public static string GetNewStorageName(string storageName, long entityId)
		{
			return $"{storageName}-{entityId}";
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
		}

		public override bool IsOverlapOverThreshold(BoundingBoxD worldAabb, float thresholdPercentage)
		{
			if (base.m_storage == null)
			{
				if (MyEntities.GetEntityByIdOrDefault(base.EntityId) != this)
				{
<<<<<<< HEAD
					MyDebug.FailRelease("Voxel map was deleted!", "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Entities\\MyVoxelMap.cs", 180);
				}
				else
				{
					MyDebug.FailRelease("Voxel map is still in world but has null storage!", "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Entities\\MyVoxelMap.cs", 182);
=======
					MyDebug.FailRelease("Voxel map was deleted!", "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Entities\\MyVoxelMap.cs", 180);
				}
				else
				{
					MyDebug.FailRelease("Voxel map is still in world but has null storage!", "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Entities\\MyVoxelMap.cs", 182);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				return false;
			}
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref worldAabb.Min, out var voxelCoord);
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref worldAabb.Max, out var voxelCoord2);
			voxelCoord += base.StorageMin;
			voxelCoord2 += base.StorageMin;
			Storage.ClampVoxelCoord(ref voxelCoord);
			Storage.ClampVoxelCoord(ref voxelCoord2);
			if (MyVoxelBase.m_tempStorage == null)
			{
				MyVoxelBase.m_tempStorage = new MyStorageData();
			}
			MyVoxelBase.m_tempStorage.Resize(voxelCoord, voxelCoord2);
			Storage.ReadRange(MyVoxelBase.m_tempStorage, MyStorageDataTypeFlags.Content, 0, voxelCoord, voxelCoord2);
			double num = 0.00392156862745098;
			double num2 = 1.0;
			double num3 = 0.0;
			double volume = worldAabb.Volume;
			Vector3I voxelCoord3 = default(Vector3I);
			voxelCoord3.Z = voxelCoord.Z;
			Vector3I p = default(Vector3I);
			p.Z = 0;
			while (voxelCoord3.Z <= voxelCoord2.Z)
			{
				voxelCoord3.Y = voxelCoord.Y;
				p.Y = 0;
				while (voxelCoord3.Y <= voxelCoord2.Y)
				{
					voxelCoord3.X = voxelCoord.X;
					p.X = 0;
					while (voxelCoord3.X <= voxelCoord2.X)
					{
						MyVoxelCoordSystems.VoxelCoordToWorldAABB(PositionLeftBottomCorner, ref voxelCoord3, out var worldAABB);
						if (worldAabb.Intersects(worldAABB))
						{
							double num4 = (double)(int)MyVoxelBase.m_tempStorage.Content(ref p) * num * num2;
							double volume2 = worldAabb.Intersect(worldAABB).Volume;
							num3 += num4 * volume2;
						}
						voxelCoord3.X++;
						p.X++;
					}
					voxelCoord3.Y++;
					p.Y++;
				}
				voxelCoord3.Z++;
				p.Z++;
			}
			return num3 / volume >= (double)thresholdPercentage;
		}

		public override bool GetIntersectionWithSphere(ref BoundingSphereD sphere)
		{
			try
			{
				if (!base.PositionComp.WorldAABB.Intersects(ref sphere))
				{
					return false;
				}
				BoundingSphere localSphere = new BoundingSphere(sphere.Center - PositionLeftBottomCorner, (float)sphere.Radius);
				return Storage.GetGeometry().Intersects(ref localSphere);
			}
			finally
			{
			}
		}

		public override bool GetIntersectionWithAABB(ref BoundingBoxD aabb)
		{
			try
			{
				if (!base.PositionComp.WorldAABB.Intersects(ref aabb))
				{
					return false;
				}
				BoundingSphere localSphere = new BoundingSphere(aabb.Center - PositionLeftBottomCorner, (float)aabb.HalfExtents.Length());
				return Storage.GetGeometry().Intersects(ref localSphere);
			}
			finally
			{
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			if (Physics != null)
			{
				Physics.UpdateBeforeSimulation10();
			}
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (Physics != null)
			{
				Physics.UpdateAfterSimulation10();
			}
		}

		protected override void BeforeDelete()
		{
			base.BeforeDelete();
			base.m_storage = null;
			MySession.Static.VoxelMaps.RemoveVoxelMap(this);
		}

		/// <summary>
		/// Invalidates voxel cache
		/// </summary>
		/// <param name="minChanged">Inclusive min</param>
		/// <param name="maxChanged">Inclusive max</param>
		/// <param name="dataChanged"></param>
		private void storage_RangeChanged(Vector3I minChanged, Vector3I maxChanged, MyStorageDataTypeFlags dataChanged)
		{
			if ((dataChanged & MyStorageDataTypeFlags.Content) != 0 && Physics != null)
			{
				Physics.InvalidateRange(minChanged, maxChanged);
			}
			MyRenderComponentVoxelMap myRenderComponentVoxelMap;
			if ((myRenderComponentVoxelMap = base.Render as MyRenderComponentVoxelMap) != null)
			{
				myRenderComponentVoxelMap.InvalidateRange(minChanged, maxChanged);
			}
			OnRangeChanged(minChanged, maxChanged, dataChanged);
			base.ContentChanged = true;
		}

		public override string GetFriendlyName()
		{
			return "MyVoxelMap";
		}

		public override void Init(string storageName, VRage.Game.Voxels.IMyStorage storage, MatrixD worldMatrix, bool useVoxelOffset = true)
		{
			m_storageMax = storage.Size;
			base.Init(storageName, storage, worldMatrix, useVoxelOffset);
			base.m_storage.RangeChanged += storage_RangeChanged;
		}

		protected override void InitVoxelMap(MatrixD worldMatrix, Vector3I size, bool useOffset = true)
		{
			base.InitVoxelMap(worldMatrix, size, useOffset);
			((MyStorageBase)Storage).InitWriteCache(8);
			Physics = new MyVoxelPhysicsBody(this, 3f, 3f, base.DelayRigidBodyCreation);
			Physics.Enabled = true;
		}

		public override int GetOrePriority()
		{
			return 1;
		}

		void IMyVoxelMap.Close()
		{
			Close();
		}

		bool IMyVoxelMap.DoOverlapSphereTest(float sphereRadius, Vector3D spherePos)
		{
			return DoOverlapSphereTest(sphereRadius, spherePos);
		}

		void IMyVoxelMap.ClampVoxelCoord(ref Vector3I voxelCoord)
		{
			Storage.ClampVoxelCoord(ref voxelCoord);
		}

		bool IMyVoxelMap.GetIntersectionWithSphere(ref BoundingSphereD sphere)
		{
			return GetIntersectionWithSphere(ref sphere);
		}

		MyObjectBuilder_EntityBase IMyVoxelMap.GetObjectBuilder(bool copy)
		{
			return GetObjectBuilder(copy);
		}

		float IMyVoxelMap.GetVoxelContentInBoundingBox(BoundingBoxD worldAabb, out float cellCount)
		{
			cellCount = 0f;
			return 0f;
		}

		Vector3I IMyVoxelMap.GetVoxelCoordinateFromMeters(Vector3D pos)
		{
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(PositionLeftBottomCorner, ref pos, out var voxelCoord);
			return voxelCoord;
		}

		void IMyVoxelMap.Init(MyObjectBuilder_EntityBase builder)
		{
			Init(builder);
		}
	}
}
