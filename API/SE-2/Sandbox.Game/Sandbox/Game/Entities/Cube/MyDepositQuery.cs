using System;
using System.Collections.Generic;
using ParallelTasks;
using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.Voxels;
using VRage.Generics;
using VRage.Profiler;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	internal class MyDepositQuery : IPrioritizedWork, IWork
	{
		private struct MaterialPositionData
		{
			public Vector3 Sum;

			public int Count;
		}

		private static readonly MyObjectsPool<MyDepositQuery> m_instancePool = new MyObjectsPool<MyDepositQuery>(16);

		[ThreadStatic]
		private static MyStorageData m_cache;

		[ThreadStatic]
		private static MaterialPositionData[] m_materialData;

		private List<MyEntityOreDeposit> m_result;

		private List<Vector3I> m_emptyCells;

		private readonly Action m_onComplete;

		private bool m_canceled;

		private static MyStorageData Cache
		{
			get
			{
				if (m_cache == null)
				{
					m_cache = new MyStorageData();
				}
				return m_cache;
			}
		}

		private static MaterialPositionData[] MaterialData
		{
			get
			{
				if (m_materialData == null)
				{
					m_materialData = new MaterialPositionData[256];
				}
				return m_materialData;
			}
		}

		public Vector3I Min { get; set; }

		public Vector3I Max { get; set; }

		public MyVoxelBase VoxelMap { get; set; }

		public long DetectorId { get; set; }

		public Action<List<MyEntityOreDeposit>, List<Vector3I>> CompletionCallback { get; set; }

		WorkPriority IPrioritizedWork.Priority => WorkPriority.VeryLow;

		WorkOptions IWork.Options => Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Block, "OreDetector");

		public static void Start(Vector3I min, Vector3I max, long detectorId, MyVoxelBase voxelMap, Action<List<MyEntityOreDeposit>, List<Vector3I>> completionCallback)
		{
			MyDepositQuery item = null;
			m_instancePool.AllocateOrCreate(out item);
			if (item != null)
			{
				item.Min = min;
				item.Max = max;
				item.DetectorId = detectorId;
				item.VoxelMap = voxelMap;
				item.CompletionCallback = completionCallback;
				MyOreDetectorSessionComponent.Static.Track(item);
				Parallel.Start(item, item.m_onComplete);
			}
		}

		public MyDepositQuery()
		{
			m_onComplete = OnComplete;
		}

		private void OnComplete()
		{
			CompletionCallback.InvokeIfNotNull(m_result, m_emptyCells);
			m_result = null;
			VoxelMap = null;
			CompletionCallback = null;
			m_instancePool.Deallocate(this);
			MyOreDetectorSessionComponent.Static.Untrack(this);
		}

		void IWork.DoWork(WorkData workData)
		{
			try
			{
				if (m_result == null)
				{
					m_result = new List<MyEntityOreDeposit>();
					m_emptyCells = new List<Vector3I>();
				}
				if (m_canceled)
				{
					return;
				}
				MyStorageData cache = Cache;
				cache.Resize(new Vector3I(8));
				IMyStorage storage = VoxelMap.Storage;
				if (storage == null)
				{
					return;
				}
<<<<<<< HEAD
				using (StoragePin storagePin = storage.Pin())
				{
					if (!storagePin.Valid)
					{
						return;
					}
					Vector3I cell = default(Vector3I);
					cell.Z = Min.Z;
					while (cell.Z <= Max.Z)
					{
						cell.Y = Min.Y;
						while (cell.Y <= Max.Y)
						{
							cell.X = Min.X;
							while (cell.X <= Max.X)
							{
								if (m_canceled)
								{
									return;
								}
								ProcessCell(cache, storage, cell, DetectorId);
								cell.X++;
							}
							cell.Y++;
						}
						cell.Z++;
=======
				using StoragePin storagePin = storage.Pin();
				if (!storagePin.Valid)
				{
					return;
				}
				Vector3I cell = default(Vector3I);
				cell.Z = Min.Z;
				while (cell.Z <= Max.Z)
				{
					cell.Y = Min.Y;
					while (cell.Y <= Max.Y)
					{
						cell.X = Min.X;
						while (cell.X <= Max.X)
						{
							if (m_canceled)
							{
								return;
							}
							ProcessCell(cache, storage, cell, DetectorId);
							cell.X++;
						}
						cell.Y++;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					cell.Z++;
				}
			}
			finally
			{
			}
		}

		private void ProcessCell(MyStorageData cache, IMyStorage storage, Vector3I cell, long detectorId)
		{
			Vector3I vector3I = cell << 3;
			Vector3I lodVoxelRangeMax = vector3I + 7;
			storage.ReadRange(cache, MyStorageDataTypeFlags.Content, 2, vector3I, lodVoxelRangeMax);
			if (!cache.ContainsVoxelsAboveIsoLevel())
			{
				return;
			}
			MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.PreciseOrePositions;
			storage.ReadRange(cache, MyStorageDataTypeFlags.Material, 2, vector3I, lodVoxelRangeMax, ref requestFlags);
			MaterialPositionData[] materialData = MaterialData;
			Vector3I p = default(Vector3I);
			p.Z = 0;
			while (p.Z < 8)
			{
				p.Y = 0;
				while (p.Y < 8)
				{
					p.X = 0;
					while (p.X < 8)
					{
						int linearIdx = cache.ComputeLinear(ref p);
						if (cache.Content(linearIdx) > 127)
						{
							byte b = cache.Material(linearIdx);
<<<<<<< HEAD
							Vector3 vector = (p + vector3I) * 4f + 2f;
							materialData[b].Sum += vector;
=======
							Vector3D vector3D = (p + vector3I) * 4f + 2f;
							ref Vector3 sum = ref materialData[b].Sum;
							sum += vector3D;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							materialData[b].Count++;
						}
						p.X++;
					}
					p.Y++;
				}
				p.Z++;
			}
			MyEntityOreDeposit myEntityOreDeposit = null;
			for (int i = 0; i < materialData.Length; i++)
			{
				if (materialData[i].Count == 0)
				{
					continue;
				}
				MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition((byte)i);
				if (voxelMaterialDefinition != null && voxelMaterialDefinition.IsRare)
				{
					if (myEntityOreDeposit == null)
					{
						myEntityOreDeposit = new MyEntityOreDeposit(VoxelMap, cell, detectorId);
					}
					List<MyEntityOreDeposit.Data> materials = myEntityOreDeposit.Materials;
					Vector3D value = materialData[i].Sum / materialData[i].Count - VoxelMap.SizeInMetresHalf;
					MatrixD matrix = VoxelMap.WorldMatrix;
					materials.Add(new MyEntityOreDeposit.Data(voxelMaterialDefinition, Vector3D.Transform(value, Quaternion.CreateFromRotationMatrix(in matrix))));
				}
			}
			if (myEntityOreDeposit != null)
			{
				m_result.Add(myEntityOreDeposit);
			}
			else
			{
				m_emptyCells.Add(cell);
			}
			Array.Clear(materialData, 0, materialData.Length);
		}

		public void Cancel()
		{
			m_canceled = true;
		}
	}
}
