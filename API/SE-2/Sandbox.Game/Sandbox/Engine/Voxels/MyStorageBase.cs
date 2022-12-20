using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Voxels.Storage;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
	public abstract class MyStorageBase : VRage.Game.Voxels.IMyStorage, VRage.ModAPI.IMyStorage
	{
		public enum MyAccessType
		{
			Read,
			Write,
			Delete
		}

		public class VoxelChunk
		{
			public const int SizeBits = 3;

			public const int Size = 8;

			public const int Volume = 512;

			public readonly Vector3I Coords;

			public byte MaxLod;

			public static readonly int TotalVolume = 841;

			public static readonly Vector3I SizeVector = new Vector3I(8);

			public static readonly Vector3I MaxVector = new Vector3I(7);

			public byte[] Material;

			public byte[] Content;

			public MyStorageDataTypeFlags Dirty;

			public MyStorageDataTypeFlags Cached;

			public int HitCount;

			internal int TreeProxy;

			public FastResourceLock Lock = new FastResourceLock();

			public VoxelChunk(Vector3I coords)
			{
				Coords = coords;
				Material = new byte[TotalVolume];
				Content = new byte[TotalVolume];
			}

			public unsafe void UpdateLodData(int lod)
			{
				for (int i = MaxLod + 1; i <= lod; i++)
				{
					UpdateLodDataInternal(i, Content, MyOctreeNode.ContentFilter);
					UpdateLodDataInternal(i, Material, MyOctreeNode.MaterialFilter);
				}
				MaxLod = (byte)lod;
			}

			private unsafe static void UpdateLodDataInternal(int lod, byte[] dataArray, MyOctreeNode.FilterFunction filter)
			{
				int num = 0;
				for (int i = 0; i < lod - 1; i++)
				{
					num += 512 >> i + i + i;
				}
				int num2 = 8 >> lod;
				int num3 = num2 * num2;
				int num4 = num3 * num2;
				int num5 = 8 >> lod - 1;
				int num6 = num5 * num5;
				int num7 = num6 * num5;
				ulong num8 = default(ulong);
				byte* ptr = (byte*)(&num8);
				fixed (byte* ptr2 = dataArray)
				{
					byte* ptr3 = ptr2 + num;
					byte* ptr4 = ptr3 + num7;
					for (int j = 0; j < num4; j += num3)
					{
						int num9 = j << 3;
						int num10 = (j << 3) + num6;
						for (int k = 0; k < num3; k += num2)
						{
							int num11 = k << 2;
							int num12 = (k << 2) + num5;
							for (int l = 0; l < num2; l++)
							{
								int num13 = l << 1;
								int num14 = (l << 1) + 1;
								*ptr = ptr3[num9 + num11 + num13];
								ptr[1] = ptr3[num9 + num11 + num14];
								ptr[2] = ptr3[num9 + num12 + num13];
								ptr[3] = ptr3[num9 + num12 + num14];
								ptr[4] = ptr3[num10 + num11 + num13];
								ptr[5] = ptr3[num10 + num11 + num14];
								ptr[6] = ptr3[num10 + num12 + num13];
								ptr[7] = ptr3[num10 + num12 + num14];
								ptr4[l + k + j] = filter(ptr, lod);
							}
						}
					}
				}
			}

			public MyStorageData MakeData()
			{
				return new MyStorageData(SizeVector, Content, Material);
			}

			public void ReadLod(MyStorageData target, MyStorageDataTypeFlags dataTypes, ref Vector3I targetOffset, int lodIndex, ref Vector3I min, ref Vector3I max)
			{
				if (lodIndex > MaxLod)
				{
					UpdateLodData(lodIndex);
				}
				if (dataTypes.Requests(MyStorageDataTypeEnum.Content))
				{
					ReadLod(target, MyStorageDataTypeEnum.Content, Content, targetOffset, lodIndex, min, max);
				}
				if (dataTypes.Requests(MyStorageDataTypeEnum.Material))
				{
					ReadLod(target, MyStorageDataTypeEnum.Material, Material, targetOffset, lodIndex, min, max);
				}
				HitCount++;
			}

			private unsafe void ReadLod(MyStorageData target, MyStorageDataTypeEnum dataType, byte[] dataArray, Vector3I tofft, int lod, Vector3I min, Vector3I max)
			{
				int num = 0;
				for (int i = 0; i < lod; i++)
				{
					num += 512 >> i + i + i;
				}
				int num2 = 8 >> lod;
				int num3 = num2 * num2;
				min.Y *= num2;
				min.Z *= num3;
				max.Y *= num2;
				max.Z *= num3;
				int stepX = target.StepX;
				int stepY = target.StepY;
				int stepZ = target.StepZ;
				tofft.Y *= stepY;
				tofft.Z *= stepZ;
				fixed (byte* ptr = dataArray)
				{
					fixed (byte* ptr3 = target[dataType])
					{
						byte* ptr2 = ptr + num;
						int num4 = min.Z;
						int num5 = tofft.Z;
						while (num4 <= max.Z)
						{
							int num6 = min.Y;
							int num7 = tofft.Y;
							while (num6 <= max.Y)
							{
								int num8 = min.X;
								int num9 = tofft.X;
								while (num8 <= max.X)
								{
									ptr3[num5 + num7 + num9] = ptr2[num4 + num6 + num8];
									num8++;
									num9 += stepX;
								}
								num6 += num2;
								num7 += stepY;
							}
							num4 += num3;
							num5 += stepZ;
						}
					}
				}
			}

			public void ExecuteOperator<TVoxelOperator>(ref TVoxelOperator voxelOperator, MyStorageDataTypeFlags dataTypes, ref Vector3I targetOffset, ref Vector3I min, ref Vector3I max) where TVoxelOperator : IVoxelOperator
			{
				if (dataTypes.Requests(MyStorageDataTypeEnum.Content))
				{
					ExecuteOperator(ref voxelOperator, MyStorageDataTypeEnum.Content, Content, targetOffset, min, max);
				}
				if (dataTypes.Requests(MyStorageDataTypeEnum.Material))
				{
					ExecuteOperator(ref voxelOperator, MyStorageDataTypeEnum.Material, Material, targetOffset, min, max);
				}
				Cached |= dataTypes;
				Dirty |= dataTypes;
				MaxLod = 0;
			}

			private void ExecuteOperator<TVoxelOperator>(ref TVoxelOperator voxelOperator, MyStorageDataTypeEnum dataType, byte[] dataArray, Vector3I tofft, Vector3I min, Vector3I max) where TVoxelOperator : IVoxelOperator
			{
				int num = 8;
				int num2 = num * num;
				min.Y *= num;
				min.Z *= num2;
				max.Y *= num;
				max.Z *= num2;
				int num3 = min.Z;
				Vector3I position = default(Vector3I);
				position.Z = tofft.Z;
				while (num3 <= max.Z)
				{
					int num4 = min.Y;
					position.Y = tofft.Y;
					while (num4 <= max.Y)
					{
						int num5 = min.X;
						position.X = tofft.X;
						while (num5 <= max.X)
						{
							voxelOperator.Op(ref position, dataType, ref dataArray[num3 + num4 + num5]);
							num5++;
							position.X++;
						}
						num4 += num;
						position.Y++;
					}
					num3 += num2;
					position.Z++;
				}
			}
		}

		public struct WriteCacheStats
		{
			public int QueuedWrites;

			public int CachedChunks;

			public IEnumerable<KeyValuePair<Vector3I, VoxelChunk>> Chunks;
		}

		private struct MyVoxelObjectDefinition
		{
			public readonly string FilePath;

			public readonly Dictionary<byte, byte> Changes;

			public MyVoxelObjectDefinition(string filePath, Dictionary<byte, byte> changes)
			{
				FilePath = filePath;
				Changes = changes;
			}

			public override int GetHashCode()
			{
				int num = 17;
				num = num * 486187739 + FilePath.GetHashCode();
				if (Changes != null)
				{
					foreach (KeyValuePair<byte, byte> change in Changes)
					{
						num = num * 486187739 + change.Key.GetHashCode();
						num = num * 486187739 + change.Value.GetHashCode();
					}
					return num;
				}
				return num;
			}
		}

		private const int ACCESS_GRID_LOD_DEFAULT = 10;

		private int m_accessGridLod;

		private int m_streamGridLod = 65535;

		private readonly ConcurrentDictionary<Vector3I, MyTimeSpan> m_access = new ConcurrentDictionary<Vector3I, MyTimeSpan>();

		private const int MaxChunksToDiscard = 10;

		private const int MaximumHitsForDiscard = 100;

		private MyConcurrentQueue<Vector3I> m_pendingChunksToWrite;

		private MyQueue<Vector3I> m_chunksbyAge;

		private MyConcurrentDictionary<Vector3I, VoxelChunk> m_cachedChunks;

		private MyDynamicAABBTree m_cacheMap;

		private FastResourceLock m_cacheLock;

		[ThreadStatic]
		private static List<VoxelChunk> m_tmpChunks;

		private const int WriteCacheCap = 1024;

		private const int MaxWriteJobWorkMillis = 6;

		private bool m_cachedWrites;

		public const int STORAGE_TYPE_VERSION_CELL = 2;

		private const string STORAGE_TYPE_NAME_CELL = "Cell";

		private const string STORAGE_TYPE_NAME_OCTREE = "Octree";

		protected const int STORAGE_TYPE_VERSION_OCTREE = 1;

		protected const int STORAGE_TYPE_VERSION_OCTREE_ACCESS = 2;

		private readonly object m_compressedDataLock = new object();

		private byte[] m_cachedData;

		private bool m_cachedDataCompressed;

		private bool m_setDataCacheAllowed;

		private readonly MyVoxelGeometry m_geometry = new MyVoxelGeometry();

		protected readonly FastResourceLock StorageLock = new FastResourceLock();

		public static bool UseStorageCache;

		private static readonly LRUCache<int, Lazy<MyStorageBase>> m_storageCache;

		private static int m_runningStorageId;

		private const int CLOSE_MASK = int.MinValue;

		private const int PIN_MASK = int.MaxValue;

		private int m_pinAndCloseMark;

		private int m_closed;

		public IEnumerator<KeyValuePair<Vector3I, MyTimeSpan>> AccessEnumerator => m_access.GetEnumerator();

		private static MyVoxelOperationsSessionComponent OperationsComponent => MySession.Static.GetComponent<MyVoxelOperationsSessionComponent>();

		public bool CachedWrites
		{
			get
			{
				if (m_cachedWrites)
				{
					return MyVoxelOperationsSessionComponent.EnableCache;
				}
				return false;
			}
			set
			{
				m_cachedWrites = value;
			}
		}

		public bool HasPendingWrites => m_pendingChunksToWrite.Count > 0;

		public bool HasCachedChunks => m_chunksbyAge.Count - m_pendingChunksToWrite.Count > 0;

		public int CachedChunksCount => m_cachedChunks.Count;

		public int PendingCachedChunksCount => m_pendingChunksToWrite.Count;

		public abstract IMyStorageDataProvider DataProvider { get; set; }

		public bool DeleteSupported => DataProvider != null;

		public bool Shared { get; internal set; }

		public uint StorageId { get; private set; }

		public MyVoxelGeometry Geometry => m_geometry;

		public Vector3I Size { get; protected set; }

		public bool AreDataCached
		{
			get
			{
				lock (m_compressedDataLock)
				{
					return m_cachedData != null;
				}
			}
		}

		public bool AreDataCachedCompressed => m_cachedDataCompressed;

		public bool Closed => Interlocked.CompareExchange(ref m_closed, 0, 0) != 0;

		public bool MarkedForClose => (Interlocked.CompareExchange(ref m_pinAndCloseMark, 0, 0) & int.MinValue) != 0;

		Vector3I VRage.ModAPI.IMyStorage.Size => Size;

		public event Action<Vector3I, Vector3I, MyStorageDataTypeFlags> RangeChanged;

		public void ConvertAccessCoordinates(ref Vector3I coord, out BoundingBoxD bb)
		{
			MyCellCoord myCellCoord = new MyCellCoord(m_accessGridLod, ref coord);
			Vector3I vector3I = myCellCoord.CoordInLod << myCellCoord.Lod;
			float num = 1f * (float)(1 << myCellCoord.Lod);
			Vector3 vector = vector3I * 1f + 0.5f * num;
			bb = new BoundingBoxD(vector - 0.5f * num, vector + 0.5f * num);
		}

		public void AccessDeleteFirst()
		{
			if (!m_access.get_IsEmpty())
			{
				Vector3I coord = Enumerable.First<KeyValuePair<Vector3I, MyTimeSpan>>((IEnumerable<KeyValuePair<Vector3I, MyTimeSpan>>)m_access).Key;
				AccessDelete(ref coord, MyStorageDataTypeFlags.ContentAndMaterial);
			}
		}

		public void MarkClear(ref Vector3I coord)
		{
			GetAccessRange(coord, out var _, out var _);
			MyCellCoord coord2 = new MyCellCoord(m_accessGridLod, coord);
			AccessRange(MyAccessType.Delete, MyStorageDataTypeEnum.Content, ref coord2);
			AccessRange(MyAccessType.Delete, MyStorageDataTypeEnum.Material, ref coord2);
		}

		public void AccessDelete(ref Vector3I coord, MyStorageDataTypeFlags dataType, bool notify = true)
		{
			GetAccessRange(coord, out var min, out var max);
			DeleteRange(dataType, min, max, notify);
			MyCellCoord coord2 = new MyCellCoord(m_accessGridLod, coord);
			if ((dataType & MyStorageDataTypeFlags.Content) != 0)
			{
				AccessRange(MyAccessType.Delete, MyStorageDataTypeEnum.Content, ref coord2);
			}
			if ((dataType & MyStorageDataTypeFlags.Material) != 0)
			{
				AccessRange(MyAccessType.Delete, MyStorageDataTypeEnum.Material, ref coord2);
			}
		}

		public void GetAccessRange(Vector3I coord, out Vector3I min, out Vector3I max)
		{
			int num = 1 << m_accessGridLod;
			min = coord << m_accessGridLod;
			max = min + num;
		}

		protected void AccessReset()
		{
			m_access.Clear();
			int num = 0;
			Vector3I size = Size;
			while (size != Vector3I.Zero)
			{
				size >>= 1;
				num++;
			}
			m_accessGridLod = Math.Min(num - 1, 10);
		}

		public void AccessRange(MyAccessType accessType, MyStorageDataTypeEnum dataType, ref MyCellCoord coord)
		{
			if (coord.Lod == m_accessGridLod && dataType == MyStorageDataTypeEnum.Content)
			{
				switch (accessType)
				{
				case MyAccessType.Write:
				{
					MyTimeSpan myTimeSpan = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
					m_access.set_Item(coord.CoordInLod, myTimeSpan);
					break;
				}
				case MyAccessType.Delete:
					m_access.Remove<Vector3I, MyTimeSpan>(coord.CoordInLod);
					break;
				}
			}
		}

		protected void ReadStorageAccess(Stream stream)
		{
			m_streamGridLod = stream.ReadUInt16();
		}

		protected void WriteStorageAccess(Stream stream)
		{
			stream.WriteNoAlloc((ushort)m_accessGridLod);
		}

		protected void LoadAccess(Stream stream, MyCellCoord coord)
		{
			if (coord.Lod == m_streamGridLod || coord.Lod == m_accessGridLod)
			{
				MyTimeSpan myTimeSpan = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
				MyTimeSpan myTimeSpan2 = myTimeSpan;
				if (coord.Lod == m_streamGridLod)
				{
					long num = stream.ReadUInt16();
					myTimeSpan2 = MyTimeSpan.FromSeconds(myTimeSpan.Seconds - (double)(num * 60));
				}
				if (coord.Lod == m_accessGridLod)
				{
					m_access.set_Item(coord.CoordInLod, myTimeSpan2);
				}
			}
		}

		protected void SaveAccess(Stream stream, MyCellCoord coord)
		{
			if (coord.Lod == m_accessGridLod)
			{
				MyTimeSpan myTimeSpan = MyTimeSpan.FromTicks(Stopwatch.GetTimestamp());
<<<<<<< HEAD
				if (!m_access.TryGetValue(coord.CoordInLod, out var value))
=======
				MyTimeSpan myTimeSpan2 = default(MyTimeSpan);
				if (!m_access.TryGetValue(coord.CoordInLod, ref myTimeSpan2))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					myTimeSpan2 = myTimeSpan;
				}
				long num = (long)((myTimeSpan - myTimeSpan2).Seconds / 60.0);
				if (num > 65535)
				{
					num = 65535L;
				}
				stream.WriteNoAlloc((ushort)num);
			}
		}

		private void DebugDrawAccess(ref MatrixD worldMatrix)
		{
			Color green = Color.Green;
			green.A = 64;
			using IMyDebugDrawBatchAabb myDebugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(worldMatrix, green);
			foreach (KeyValuePair<Vector3I, MyTimeSpan> item in m_access)
			{
<<<<<<< HEAD
				foreach (KeyValuePair<Vector3I, MyTimeSpan> item in m_access)
				{
					Vector3I coord = item.Key;
					ConvertAccessCoordinates(ref coord, out var bb);
					myDebugDrawBatchAabb.Add(ref bb);
				}
=======
				Vector3I coord = item.Key;
				ConvertAccessCoordinates(ref coord, out var bb);
				myDebugDrawBatchAabb.Add(ref bb);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void DebugDrawDelete(Vector3I coord, MyVoxelBase voxelCurrentBase)
		{
			ConvertAccessCoordinates(ref coord, out var bb);
			bb.TransformSlow(voxelCurrentBase.WorldMatrix);
			MyRenderProxy.DebugDrawAABB(bb, Color.Red, 0.3f, 1f, depthRead: true, shaded: false, persistent: true);
		}

		public void InitWriteCache(int prealloc = 128)
		{
			if (m_cachedChunks == null && OperationsComponent != null)
			{
				CachedWrites = true;
				m_cachedChunks = new MyConcurrentDictionary<Vector3I, VoxelChunk>(prealloc, Vector3I.Comparer);
				m_pendingChunksToWrite = new MyConcurrentQueue<Vector3I>(prealloc / 10);
				m_chunksbyAge = new MyQueue<Vector3I>(prealloc);
				m_cacheMap = new MyDynamicAABBTree(Vector3.Zero);
				m_cacheLock = new FastResourceLock();
				if (MyVoxelOperationsSessionComponent.EnableCache)
				{
					OperationsComponent.Add(this);
				}
			}
		}

		private void DeleteChunk(ref Vector3I coord, MyStorageDataTypeFlags dataToDelete)
		{
			if (m_cachedChunks.TryGetValue(coord, out var value))
			{
				if ((dataToDelete & value.Cached) == value.Cached)
				{
					m_cachedChunks.Remove(coord);
					m_cacheMap.RemoveProxy(value.TreeProxy);
				}
				else
				{
					value.Cached &= (MyStorageDataTypeFlags)(byte)(~(int)dataToDelete);
					value.Dirty &= (MyStorageDataTypeFlags)(byte)(~(int)dataToDelete);
				}
			}
		}

		private void WriteChunk(VoxelChunk chunk)
		{
			using (StorageLock.AcquireExclusiveUsing())
			{
				using (chunk.Lock.AcquireExclusiveUsing())
				{
					if (chunk.Dirty != 0)
					{
						Vector3I voxelRangeMin = chunk.Coords << 3;
						Vector3I voxelRangeMax = (chunk.Coords + 1 << 3) - 1;
						MyStorageData data = chunk.MakeData();
						MyStorageDataWriteOperator source = new MyStorageDataWriteOperator(data);
						WriteRangeInternal(ref source, chunk.Dirty, ref voxelRangeMin, ref voxelRangeMax);
						chunk.Dirty = MyStorageDataTypeFlags.None;
					}
				}
			}
		}

		private void GetChunk(ref Vector3I coord, out VoxelChunk chunk, MyStorageDataTypeFlags required)
		{
			using (m_cacheLock.AcquireExclusiveUsing())
			{
				if (!m_cachedChunks.TryGetValue(coord, out chunk))
				{
					chunk = new VoxelChunk(coord);
					Vector3I vector3I = coord << 3;
					Vector3I vector3I2 = (coord + 1 << 3) - 1;
					if (required != 0)
					{
						using (StorageLock.AcquireSharedUsing())
						{
							ReadDatForChunk(chunk, required);
						}
					}
					m_chunksbyAge.Enqueue(coord);
					m_cachedChunks.Add(coord, chunk);
					BoundingBox aabb = new BoundingBox(vector3I, vector3I2);
					chunk.TreeProxy = m_cacheMap.AddProxy(ref aabb, chunk, 0u);
				}
				else if ((chunk.Cached & required) != required)
				{
					using (StorageLock.AcquireSharedUsing())
					{
						ReadDatForChunk(chunk, required & ((MyStorageDataTypeFlags)(~(uint)chunk.Cached) & MyStorageDataTypeFlags.ContentAndMaterial));
					}
				}
			}
		}

		private void ReadDatForChunk(VoxelChunk chunk, MyStorageDataTypeFlags data)
		{
			using (chunk.Lock.AcquireExclusiveUsing())
			{
				Vector3I lodVoxelRangeMin = chunk.Coords << 3;
				Vector3I lodVoxelRangeMax = (chunk.Coords + 1 << 3) - 1;
				MyStorageData target = chunk.MakeData();
				MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.ConsiderContent;
				ReadRangeInternal(target, ref Vector3I.Zero, data, 0, ref lodVoxelRangeMin, ref lodVoxelRangeMax, ref requestFlags);
				chunk.Cached |= data;
				chunk.MaxLod = 0;
			}
		}

		private void DequeueDirtyChunk(out VoxelChunk chunk, out Vector3I coords)
		{
			coords = m_pendingChunksToWrite.Dequeue();
			m_cachedChunks.TryGetValue(coords, out chunk);
		}

		public void CleanCachedChunks()
		{
			int num = 0;
			int count = m_chunksbyAge.Count;
			for (int i = 0; i < count; i++)
			{
				if (num >= 10)
				{
					break;
				}
				Vector3I vector3I;
				bool flag;
				VoxelChunk value;
				using (m_cacheLock.AcquireExclusiveUsing())
				{
					vector3I = m_chunksbyAge.Dequeue();
					flag = m_cachedChunks.TryGetValue(vector3I, out value);
				}
				if (!flag)
				{
					continue;
				}
				if (value.Dirty == MyStorageDataTypeFlags.None && value.HitCount <= 100)
				{
					using (value.Lock.AcquireSharedUsing())
					{
						if (value.Dirty == MyStorageDataTypeFlags.None && value.HitCount <= 100)
						{
							using (m_cacheLock.AcquireExclusiveUsing())
							{
								m_cachedChunks.Remove(vector3I);
								m_cacheMap.RemoveProxy(value.TreeProxy);
							}
						}
						else
						{
							using (m_cacheLock.AcquireExclusiveUsing())
							{
								m_chunksbyAge.Enqueue(vector3I);
								value.HitCount = 0;
							}
						}
					}
				}
				else
				{
					using (m_cacheLock.AcquireExclusiveUsing())
					{
						m_chunksbyAge.Enqueue(vector3I);
						value.HitCount = 0;
					}
				}
			}
		}

		public bool WritePending(bool force = false)
		{
			Stopwatch val = Stopwatch.StartNew();
			using (m_cacheLock.AcquireSharedUsing())
			{
				while ((val.get_ElapsedMilliseconds() < 6 || force) && m_pendingChunksToWrite.Count > 0)
				{
					DequeueDirtyChunk(out var chunk, out var _);
					if (chunk != null && chunk.Dirty != 0)
					{
						WriteChunk(chunk);
					}
				}
			}
			return m_pendingChunksToWrite.Count == 0;
		}

		public void GetStats(out WriteCacheStats stats)
		{
			stats.CachedChunks = m_cachedChunks.Count;
			stats.QueuedWrites = m_pendingChunksToWrite.Count;
			stats.Chunks = m_cachedChunks;
		}

		private bool OverlapsAnyCachedCell(Vector3I voxelRangeMin, Vector3I voxelRangeMax)
		{
			BoundingBox bbox = new BoundingBox(voxelRangeMin, voxelRangeMax);
			using (m_cacheLock.AcquireSharedUsing())
			{
				return m_cacheMap.OverlapsAnyLeafBoundingBox(ref bbox);
			}
		}

		protected void FlushCache(ref BoundingBoxI box, int lodIndex)
		{
			if (!CachedWrites || m_cacheMap.GetLeafCount() == 0)
			{
				return;
			}
			if (m_tmpChunks == null)
			{
				m_tmpChunks = new List<VoxelChunk>();
			}
			BoundingBox bbox = new BoundingBox(box.Min << lodIndex, box.Max << lodIndex);
			using (m_cacheLock.AcquireSharedUsing())
			{
				m_cacheMap.OverlapAllBoundingBox(ref bbox, m_tmpChunks, 0u, clear: false);
			}
			foreach (VoxelChunk tmpChunk in m_tmpChunks)
			{
				if (tmpChunk.Dirty != 0)
				{
					WriteChunk(tmpChunk);
				}
			}
			m_tmpChunks.Clear();
		}

		public static MyStorageBase CreateAsteroidStorage(string asteroid)
		{
			if (string.IsNullOrEmpty(asteroid))
			{
				MyLog.Default.Error("Error: asteroid should not be null!");
				return null;
			}
			if (MyDefinitionManager.Static.TryGetVoxelMapStorageDefinition(asteroid, out var definition))
			{
				return LoadFromFile(Path.Combine(definition.Context.IsBaseGame ? MyFileSystem.ContentPath : definition.Context.ModPath, definition.StorageFile));
			}
			return null;
		}

		static MyStorageBase()
		{
			UseStorageCache = true;
			m_storageCache = new LRUCache<int, Lazy<MyStorageBase>>(512);
			m_runningStorageId = -1;
			MyVRage.RegisterExitCallback(ResetCache);
		}

		protected MyStorageBase()
		{
			StorageId = (uint)Interlocked.Increment(ref m_runningStorageId);
		}

		/// <inheritdoc cref="M:VRage.Game.Voxels.IMyStorage.NotifyChanged(VRageMath.Vector3I,VRageMath.Vector3I,VRage.Voxels.MyStorageDataTypeFlags)" />
		public void NotifyChanged(Vector3I voxelRangeMin, Vector3I voxelRangeMax, MyStorageDataTypeFlags changedData)
		{
			OnRangeChanged(voxelRangeMin, voxelRangeMax, changedData);
		}

		protected void OnRangeChanged(Vector3I voxelRangeMin, Vector3I voxelRangeMax, MyStorageDataTypeFlags changedData)
		{
			if (!Closed)
			{
				ResetDataCache();
				if (this.RangeChanged != null)
				{
					this.ClampVoxelCoord(ref voxelRangeMin);
					this.ClampVoxelCoord(ref voxelRangeMax);
					this.RangeChanged.InvokeIfNotNull(voxelRangeMin, voxelRangeMax, changedData);
				}
			}
		}

		public void ResetDataCache()
		{
			lock (m_compressedDataLock)
			{
				m_setDataCacheAllowed = false;
				m_cachedData = null;
			}
		}

		public void SetDataCache(byte[] data, bool compressed)
		{
			lock (m_compressedDataLock)
			{
				if (m_setDataCacheAllowed)
				{
					m_setDataCacheAllowed = false;
					m_cachedData = data;
					m_cachedDataCompressed = compressed;
				}
			}
		}

		private unsafe void ChangeMaterials(Dictionary<byte, byte> map)
		{
			int num = 0;
			if ((Size + 1).Size > 4194304)
			{
				MyLog.Default.Error("Cannot overwrite materials for a storage 4 MB or larger.");
				return;
			}
			Vector3I zero = Vector3I.Zero;
			Vector3I vector3I = Size - 1;
			MyStorageData myStorageData = new MyStorageData();
			myStorageData.Resize(Size);
			ReadRange(myStorageData, MyStorageDataTypeFlags.Material, 0, zero, vector3I);
			int sizeLinear = myStorageData.SizeLinear;
			fixed (byte* ptr = myStorageData[MyStorageDataTypeEnum.Material])
			{
				for (int i = 0; i < sizeLinear; i++)
				{
					if (map.TryGetValue(ptr[i], out var value))
					{
						ptr[i] = value;
						num++;
					}
				}
			}
			if (num > 0)
			{
				WriteRange(myStorageData, MyStorageDataTypeFlags.Material, zero, vector3I);
			}
		}

		public static MyStorageBase LoadFromFile(string absoluteFilePath, Dictionary<byte, byte> modifiers = null, bool cache = true)
		{
			if (absoluteFilePath.Contains(".vox") && absoluteFilePath.Contains(".vx2"))
			{
				int num = absoluteFilePath.LastIndexOf(".vx2");
				if (num != -1)
				{
					absoluteFilePath = absoluteFilePath.Remove(num);
					absoluteFilePath = Path.ChangeExtension(absoluteFilePath, "vx2");
				}
			}
			MyVoxelObjectDefinition definition = new MyVoxelObjectDefinition(absoluteFilePath, modifiers);
			int hashCode = definition.GetHashCode();
			if (cache && UseStorageCache)
			{
				Lazy<MyStorageBase> lazy;
				lock (m_storageCache)
<<<<<<< HEAD
				{
					lazy = m_storageCache.Read(hashCode);
					if (lazy == null)
					{
						lazy = new Lazy<MyStorageBase>(PerformLoad);
						m_storageCache.Write(hashCode, lazy);
					}
				}
				MyStorageBase value = lazy.Value;
				if (value != null)
				{
					value.Shared = true;
				}
=======
				{
					lazy = m_storageCache.Read(hashCode);
					if (lazy == null)
					{
						lazy = new Lazy<MyStorageBase>(PerformLoad);
						m_storageCache.Write(hashCode, lazy);
					}
				}
				MyStorageBase value = lazy.Value;
				if (value != null)
				{
					value.Shared = true;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return value;
			}
			return PerformLoad();
			MyStorageBase PerformLoad()
			{
				if (!MyFileSystem.FileExists(absoluteFilePath))
				{
					string text = Path.ChangeExtension(absoluteFilePath, "vox");
					MySandboxGame.Log.WriteLine($"Loading voxel storage from file '{text}'");
					if (!MyFileSystem.FileExists(text))
					{
						int num2 = absoluteFilePath.LastIndexOf(".vx2");
						if (num2 == -1)
						{
							return null;
						}
						text = absoluteFilePath.Remove(num2);
						if (!MyFileSystem.FileExists(text))
						{
							return null;
						}
					}
					UpdateFileFormat(text);
				}
				else
				{
					MySandboxGame.Log.WriteLine($"Loading voxel storage from file '{absoluteFilePath}'");
				}
				byte[] array = null;
				using (Stream stream = MyFileSystem.OpenRead(absoluteFilePath))
				{
					array = new byte[stream.Length];
					stream.Read(array, 0, array.Length);
				}
				bool isOldFormat;
				MyStorageBase myStorageBase = Load(array, out isOldFormat);
				if (definition.Changes != null)
				{
					myStorageBase.ChangeMaterials(definition.Changes);
				}
				return myStorageBase;
			}
		}

		public static void ResetCache()
		{
			lock (m_storageCache)
			{
				foreach (var (_, lazy2) in m_storageCache)
				{
					if (lazy2 != null && lazy2.IsValueCreated)
					{
						lazy2.Value?.Close();
					}
				}
				m_storageCache.Reset();
			}
		}

		public static MyStorageBase Load(string name, bool cache = true, bool local = false)
		{
			MyStorageBase myStorageBase = null;
			if (local || MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer)
			{
				return LoadFromFile(Path.IsPathRooted(name) ? name : Path.Combine(MySession.Static.CurrentPath, name + ".vx2"), null, cache);
			}
			bool isOldFormat = false;
			byte[] array = MyMultiplayer.Static.VoxelMapData.Read(name);
			if (array != null)
			{
				return Load(array, out isOldFormat);
			}
			throw new Exception($"Missing voxel map data! : {name}");
		}

		public static MyStorageBase Load(byte[] memoryBuffer, out bool isOldFormat)
		{
<<<<<<< HEAD
=======
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			bool compressed = false;
			MyStorageBase storage;
			using (MemoryStream memoryStream = new MemoryStream(memoryBuffer, writable: false))
			{
<<<<<<< HEAD
				using (Stream stream = memoryStream.UnwrapGZip())
				{
					compressed = stream != memoryStream;
					using (BufferedStream stream2 = new BufferedStream(stream, 32768))
					{
						Load(stream2, out storage, out isOldFormat);
					}
=======
				using Stream stream = memoryStream.UnwrapGZip();
				compressed = stream != memoryStream;
				BufferedStream val = new BufferedStream(stream, 32768);
				try
				{
					Load((Stream)(object)val, out storage, out isOldFormat);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			if (!isOldFormat)
			{
				storage.SetDataCache(memoryBuffer, compressed);
			}
			else
			{
				MySandboxGame.Log.WriteLine("Voxel storage was in old format. It is updated but needs to be saved.");
				storage.ResetDataCache();
			}
			return storage;
		}

		private static void Load(Stream stream, out MyStorageBase storage, out bool isOldFormat)
		{
			try
			{
				isOldFormat = false;
				string text = stream.ReadString();
				int fileVersion = stream.Read7BitEncodedInt();
				if (text == "Cell")
				{
					MyStorageBaseCompatibility myStorageBaseCompatibility = new MyStorageBaseCompatibility();
					storage = myStorageBaseCompatibility.Compatibility_LoadCellStorage(fileVersion, stream);
					isOldFormat = true;
					return;
				}
				if (text == "Octree")
				{
					storage = new MyOctreeStorage();
					storage.LoadInternal(fileVersion, stream, ref isOldFormat);
					storage.m_geometry.Init(storage);
					storage.m_setDataCacheAllowed = true;
					return;
				}
				throw new InvalidBranchException();
			}
			finally
			{
			}
		}

		public void Save(out byte[] outCompressedData)
		{
			if (CachedWrites)
			{
				WritePending(force: true);
			}
			try
			{
				using (StorageLock.AcquireSharedUsing())
				{
					lock (m_compressedDataLock)
					{
						if (m_cachedData == null)
						{
							m_cachedData = GetData(compressed: true);
							m_cachedDataCompressed = true;
						}
						outCompressedData = m_cachedData;
					}
				}
			}
			finally
			{
			}
		}

		private byte[] GetData(bool compressed)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			MemoryStream memoryStream;
			using (memoryStream = new MemoryStream(16384))
			{
				Stream stream = null;
				stream = ((!compressed) ? memoryStream : ((MemoryStream)new GZipStream((Stream)memoryStream, (CompressionMode)1)));
				BufferedStream val = new BufferedStream(stream, 16384);
				try
				{
					if (!(GetType() == typeof(MyOctreeStorage)))
					{
						throw new InvalidBranchException();
					}
					string text = "Octree";
					int value = 2;
					((Stream)(object)val).WriteNoAlloc(text);
					((Stream)(object)val).Write7BitEncodedInt(value);
					SaveInternal((Stream)(object)val);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				if (compressed)
				{
					stream.Dispose();
				}
			}
			return memoryStream.ToArray();
		}

		public byte[] GetVoxelData()
		{
			if (CachedWrites)
			{
				WritePending(force: true);
			}
			byte[] array = null;
			try
			{
				using (StorageLock.AcquireSharedUsing())
				{
					array = GetData(compressed: false);
					m_setDataCacheAllowed = true;
					return array;
				}
			}
			finally
			{
			}
		}

		/// <summary>
		/// Resets the data specified by flags to values from data provider, or default if no provider is assigned.
		/// </summary>
		/// <param name="dataToReset"></param>
		public void Reset(MyStorageDataTypeFlags dataToReset)
		{
			using (StorageLock.AcquireExclusiveUsing())
			{
				ResetDataCache();
				ResetInternal(dataToReset);
			}
<<<<<<< HEAD
			if (Thread.CurrentThread != MySandboxGame.Static.UpdateThread)
=======
			if (Thread.get_CurrentThread() != MySandboxGame.Static.UpdateThread)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MySandboxGame.Static.Invoke(delegate
				{
					if (!Closed)
					{
						OnRangeChanged(Vector3I.Zero, Size - 1, dataToReset);
					}
				}, "MyStorageBase.Reset: RangeChanged");
			}
			else
			{
				OnRangeChanged(Vector3I.Zero, Size - 1, dataToReset);
			}
		}

		public void OverwriteAllMaterials(MyVoxelMaterialDefinition material)
		{
			using (StorageLock.AcquireExclusiveUsing())
			{
				ResetDataCache();
				OverwriteAllMaterialsInternal(material);
			}
			OnRangeChanged(Vector3I.Zero, Size - 1, MyStorageDataTypeFlags.Material);
		}

		public void ExecuteOperationFast<TVoxelOperator>(ref TVoxelOperator voxelOperator, MyStorageDataTypeFlags dataToWrite, ref Vector3I voxelRangeMin, ref Vector3I voxelRangeMax, bool notifyRangeChanged, bool skipCache) where TVoxelOperator : struct, IVoxelOperator
		{
			try
			{
				ResetDataCache();
				if (CachedWrites && !skipCache && (m_pendingChunksToWrite.Count < 1024 || OverlapsAnyCachedCell(voxelRangeMin, voxelRangeMax)))
				{
					int num = 3;
					Vector3I vector3I = voxelRangeMin >> num;
					Vector3I vector3I2 = voxelRangeMax >> num;
					Vector3I coord = Vector3I.Zero;
					coord.Z = vector3I.Z;
					while (coord.Z <= vector3I2.Z)
					{
						coord.Y = vector3I.Y;
						while (coord.Y <= vector3I2.Y)
						{
							coord.X = vector3I.X;
							while (coord.X <= vector3I2.X)
							{
								Vector3I vector3I3 = coord << num;
								Vector3I value = coord << num;
								value = Vector3I.Max(value, voxelRangeMin);
								Vector3I value2 = (coord + 1 << num) - 1;
								value2 = Vector3I.Min(value2, voxelRangeMax);
								Vector3I targetOffset = value - voxelRangeMin;
								MyStorageDataTypeFlags required = dataToWrite;
								if ((value2 - value + 1).Size == 512 && voxelOperator.Flags == VoxelOperatorFlags.WriteAll)
								{
									required = MyStorageDataTypeFlags.None;
								}
								GetChunk(ref coord, out var chunk, required);
								value -= vector3I3;
								value2 -= vector3I3;
								using (chunk.Lock.AcquireExclusiveUsing())
								{
									bool num2 = chunk.Dirty != MyStorageDataTypeFlags.None;
									chunk.ExecuteOperator(ref voxelOperator, dataToWrite, ref targetOffset, ref value, ref value2);
									if (!num2)
									{
										m_pendingChunksToWrite.Enqueue(coord);
									}
								}
								coord.X++;
							}
							coord.Y++;
						}
						coord.Z++;
					}
				}
				else
				{
					if (skipCache)
					{
						BoundingBoxI box = new BoundingBoxI(voxelRangeMin, voxelRangeMax);
						FlushCache(ref box, 0);
					}
					using (StorageLock.AcquireExclusiveUsing())
					{
						WriteRangeInternal(ref voxelOperator, dataToWrite, ref voxelRangeMin, ref voxelRangeMax);
					}
				}
			}
			finally
			{
				if (notifyRangeChanged)
				{
					OnRangeChanged(voxelRangeMin, voxelRangeMax, dataToWrite);
				}
			}
		}

		public void WriteRange(MyStorageData source, MyStorageDataTypeFlags dataToWrite, Vector3I voxelRangeMin, Vector3I voxelRangeMax, bool notifyRangeChanged = true, bool skipCache = false)
		{
			MyStorageDataWriteOperator voxelOperator = new MyStorageDataWriteOperator(source);
			ExecuteOperationFast(ref voxelOperator, dataToWrite, ref voxelRangeMin, ref voxelRangeMax, notifyRangeChanged, skipCache);
		}

		public void Sweep(MyStorageDataTypeFlags dataToSweep)
		{
			try
			{
				ResetDataCache();
				if (CachedWrites)
				{
					using (m_cacheLock.AcquireExclusiveUsing())
					{
						while (m_cachedChunks.Count > 0)
						{
							Vector3I coord = m_cachedChunks.FirstPair().Key;
							DeleteChunk(ref coord, dataToSweep);
						}
					}
				}
				using (StorageLock.AcquireExclusiveUsing())
				{
					SweepInternal(dataToSweep);
				}
			}
			finally
			{
				OnRangeChanged(Vector3I.Zero, Size, dataToSweep);
			}
		}

		private void DeleteRange(MyStorageDataTypeFlags dataToDelete, Vector3I voxelRangeMin, Vector3I voxelRangeMax, bool notify)
		{
			try
			{
				ResetDataCache();
				if (CachedWrites && OverlapsAnyCachedCell(voxelRangeMin, voxelRangeMax))
				{
					using (m_cacheLock.AcquireExclusiveUsing())
					{
						int num = 3;
						Vector3I vector3I = voxelRangeMin >> num;
						Vector3I vector3I2 = voxelRangeMax >> num;
						Vector3I coord = Vector3I.Zero;
						coord.Z = vector3I.Z;
						while (coord.Z <= vector3I2.Z)
						{
							coord.Y = vector3I.Y;
							while (coord.Y <= vector3I2.Y)
							{
								coord.X = vector3I.X;
								while (coord.X <= vector3I2.X)
								{
									DeleteChunk(ref coord, dataToDelete);
									coord.X++;
								}
								coord.Y++;
							}
							coord.Z++;
						}
					}
				}
				using (StorageLock.AcquireExclusiveUsing())
				{
					DeleteRangeInternal(dataToDelete, ref voxelRangeMin, ref voxelRangeMax);
				}
			}
			finally
			{
				if (notify)
				{
					OnRangeChanged(voxelRangeMin, voxelRangeMax, dataToDelete);
				}
			}
		}

		public bool IsRangeModified(ref BoundingBoxI box)
		{
			if (DataProvider == null)
			{
				return true;
			}
			using (StorageLock.AcquireSharedUsing())
			{
				return IsModifiedInternal(ref box);
			}
		}

		public void ReadRange(MyStorageData target, MyStorageDataTypeFlags dataToRead, int lodIndex, Vector3I lodVoxelRangeMin, Vector3I lodVoxelRangeMax)
		{
			MyVoxelRequestFlags requestFlags = ((dataToRead == MyStorageDataTypeFlags.ContentAndMaterial) ? MyVoxelRequestFlags.ConsiderContent : ((MyVoxelRequestFlags)0));
			ReadRange(target, dataToRead, lodIndex, lodVoxelRangeMin, lodVoxelRangeMax, ref requestFlags);
		}

		private void ReadRangeAdviseCache(MyStorageData target, MyStorageDataTypeFlags dataToRead, Vector3I lodVoxelRangeMin, Vector3I lodVoxelRangeMax)
		{
			if (m_pendingChunksToWrite.Count > 1024)
			{
				ReadRange(target, dataToRead, 0, lodVoxelRangeMin, lodVoxelRangeMax);
			}
			else
			{
				if (!CachedWrites)
				{
					return;
				}
				int num = 3;
				Vector3I vector3I = lodVoxelRangeMin >> num;
				Vector3I vector3I2 = lodVoxelRangeMax >> num;
				Vector3I coord = Vector3I.Zero;
				coord.Z = vector3I.Z;
				while (coord.Z <= vector3I2.Z)
				{
					coord.Y = vector3I.Y;
					while (coord.Y <= vector3I2.Y)
					{
						coord.X = vector3I.X;
						while (coord.X <= vector3I2.X)
						{
							Vector3I vector3I3 = coord << num;
							Vector3I value = coord << num;
							value = Vector3I.Max(value, lodVoxelRangeMin);
							Vector3I targetOffset = value - lodVoxelRangeMin;
							Vector3I value2 = (coord + 1 << num) - 1;
							value2 = Vector3I.Min(value2, lodVoxelRangeMax);
							GetChunk(ref coord, out var chunk, dataToRead);
							value -= vector3I3;
							value2 -= vector3I3;
							using (chunk.Lock.AcquireSharedUsing())
							{
								chunk.ReadLod(target, dataToRead, ref targetOffset, 0, ref value, ref value2);
							}
							coord.X++;
						}
						coord.Y++;
					}
					coord.Z++;
				}
			}
		}

		public void ReadRange(MyStorageData target, MyStorageDataTypeFlags dataToRead, int lodIndex, Vector3I lodVoxelRangeMin, Vector3I lodVoxelRangeMax, ref MyVoxelRequestFlags requestFlags)
		{
			if ((dataToRead & MyStorageDataTypeFlags.Material) != 0 && (requestFlags & MyVoxelRequestFlags.SurfaceMaterial) == 0)
			{
				target.ClearMaterials(byte.MaxValue);
				requestFlags |= MyVoxelRequestFlags.EmptyData;
				if ((dataToRead & MyStorageDataTypeFlags.Content) != 0)
				{
					requestFlags |= MyVoxelRequestFlags.ConsiderContent;
				}
			}
			if ((dataToRead & MyStorageDataTypeFlags.Content) != 0)
			{
				target.ClearContent(0);
				requestFlags |= MyVoxelRequestFlags.EmptyData;
			}
			if (requestFlags.HasFlags(MyVoxelRequestFlags.AdviseCache) && lodIndex == 0 && CachedWrites)
			{
				ReadRangeAdviseCache(target, dataToRead, lodVoxelRangeMin, lodVoxelRangeMax);
				return;
			}
			if (CachedWrites && lodIndex <= 3 && m_cachedChunks.Count > 0)
			{
				if (m_tmpChunks == null)
				{
					m_tmpChunks = new List<VoxelChunk>();
				}
				int num = 3 - lodIndex;
				BoundingBox bbox = new BoundingBox(lodVoxelRangeMin << lodIndex, lodVoxelRangeMax << lodIndex);
				using (m_cacheLock.AcquireSharedUsing())
				{
					m_cacheMap.OverlapAllBoundingBox(ref bbox, m_tmpChunks, 0u, clear: false);
				}
				if (m_tmpChunks.Count > 0)
				{
					Vector3I vector3I = lodVoxelRangeMin >> num;
					Vector3I vector3I2 = lodVoxelRangeMax >> num;
					bool flag = false;
					if ((vector3I2 - vector3I + 1).Size > m_tmpChunks.Count)
					{
						using (StorageLock.AcquireSharedUsing())
						{
							ReadRangeInternal(target, ref Vector3I.Zero, dataToRead, lodIndex, ref lodVoxelRangeMin, ref lodVoxelRangeMax, ref requestFlags);
						}
						requestFlags &= ~(MyVoxelRequestFlags.EmptyData | MyVoxelRequestFlags.FullContent | MyVoxelRequestFlags.OneMaterial);
						flag = true;
					}
					for (int i = 0; i < m_tmpChunks.Count; i++)
					{
						VoxelChunk voxelChunk = m_tmpChunks[i];
						Vector3I coords = voxelChunk.Coords;
						Vector3I vector3I3 = coords << num;
						Vector3I value = coords << num;
						value = Vector3I.Max(value, lodVoxelRangeMin);
						Vector3I targetOffset = value - lodVoxelRangeMin;
						Vector3I value2 = (coords + 1 << num) - 1;
						value2 = Vector3I.Min(value2, lodVoxelRangeMax);
						value -= vector3I3;
						value2 -= vector3I3;
						if ((voxelChunk.Cached & dataToRead) != dataToRead && !flag)
						{
							using (StorageLock.AcquireSharedUsing())
							{
								if ((voxelChunk.Cached & dataToRead) != dataToRead)
								{
									ReadDatForChunk(voxelChunk, dataToRead);
								}
							}
						}
						using (voxelChunk.Lock.AcquireSharedUsing())
						{
							voxelChunk.ReadLod(target, (!flag) ? dataToRead : (dataToRead & voxelChunk.Cached), ref targetOffset, lodIndex, ref value, ref value2);
						}
					}
					m_tmpChunks.Clear();
					return;
				}
			}
			using (StorageLock.AcquireSharedUsing())
			{
				ReadRangeInternal(target, ref Vector3I.Zero, dataToRead, lodIndex, ref lodVoxelRangeMin, ref lodVoxelRangeMax, ref requestFlags);
			}
		}

		public abstract ContainmentType Intersect(ref BoundingBox box, bool lazy);

		public abstract bool IntersectInternal(ref LineD line);

		public void PinAndExecute(Action action)
		{
			using StoragePin storagePin = Pin();
			if (storagePin.Valid)
			{
				action.InvokeIfNotNull();
			}
		}

		public void PinAndExecute(Action<VRage.ModAPI.IMyStorage> action)
		{
			using StoragePin storagePin = Pin();
			if (storagePin.Valid)
			{
				action.InvokeIfNotNull(this);
			}
		}

		public ContainmentType Intersect(ref BoundingBoxI box, int lod, bool exhaustiveContainmentCheck = true)
		{
			FlushCache(ref box, lod);
			using (StorageLock.AcquireSharedUsing())
			{
				if (Closed)
				{
					return ContainmentType.Disjoint;
				}
				return IntersectInternal(ref box, lod, exhaustiveContainmentCheck);
			}
		}

		public bool Intersect(ref LineD line)
		{
			using (StorageLock.AcquireSharedUsing())
			{
				if (Closed)
				{
					return false;
				}
				return IntersectInternal(ref line);
			}
		}

		protected abstract ContainmentType IntersectInternal(ref BoundingBoxI box, int lod, bool exhaustiveContainmentCheck);

		protected abstract void LoadInternal(int fileVersion, Stream stream, ref bool isOldFormat);

		protected abstract void SaveInternal(Stream stream);

		protected abstract void ResetInternal(MyStorageDataTypeFlags dataToReset);

		protected abstract void OverwriteAllMaterialsInternal(MyVoxelMaterialDefinition material);

		protected abstract void WriteRangeInternal<TOperator>(ref TOperator source, MyStorageDataTypeFlags dataToWrite, ref Vector3I voxelRangeMin, ref Vector3I voxelRangeMax) where TOperator : struct, IVoxelOperator;

		protected abstract void DeleteRangeInternal(MyStorageDataTypeFlags dataToDelete, ref Vector3I voxelRangeMin, ref Vector3I voxelRangeMax);

		protected abstract void SweepInternal(MyStorageDataTypeFlags dataToSweep);

		protected abstract void ReadRangeInternal(MyStorageData target, ref Vector3I targetWriteRange, MyStorageDataTypeFlags dataToRead, int lodIndex, ref Vector3I lodVoxelRangeMin, ref Vector3I lodVoxelRangeMax, ref MyVoxelRequestFlags requestFlags);

		protected abstract bool IsModifiedInternal(ref BoundingBoxI range);

		public virtual void DebugDraw(ref MatrixD worldMatrix, MyVoxelDebugDrawMode mode)
		{
			if (mode == MyVoxelDebugDrawMode.Content_Access)
			{
				DebugDrawAccess(ref worldMatrix);
			}
		}

		private static void UpdateFileFormat(string originalVoxFile)
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			string path = Path.ChangeExtension(originalVoxFile, ".vx2");
			if (!File.Exists(originalVoxFile))
			{
				MySandboxGame.Log.Error("Voxel file '{0}' does not exist!", originalVoxFile);
				return;
			}
			if (Path.GetExtension(originalVoxFile) != ".vox")
			{
				MySandboxGame.Log.Warning("Unexpected voxel file extensions in path: '{0}'", originalVoxFile);
			}
			try
			{
				using MyCompressionFileLoad myCompressionFileLoad = new MyCompressionFileLoad(originalVoxFile);
				using Stream stream = MyFileSystem.OpenWrite(path);
				GZipStream val = new GZipStream(stream, (CompressionMode)1);
				try
				{
					BufferedStream val2 = new BufferedStream((Stream)(object)val);
					try
					{
						((Stream)(object)val2).WriteNoAlloc("Cell");
						((Stream)(object)val2).Write7BitEncodedInt(myCompressionFileLoad.GetInt32());
						byte[] array = new byte[16384];
						for (int bytes = myCompressionFileLoad.GetBytes(array.Length, array); bytes != 0; bytes = myCompressionFileLoad.GetBytes(array.Length, array))
						{
							((Stream)(object)val2).Write(array, 0, bytes);
						}
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.Error("While updating voxel storage '{0}' to new format: {1}", originalVoxFile, ex.Message);
			}
		}

		public virtual VRage.Game.Voxels.IMyStorage Copy()
		{
			return null;
		}

		protected void PerformClose()
		{
			if (Interlocked.CompareExchange(ref m_closed, 1, 0) == 0)
			{
				CloseInternal();
			}
		}

		public virtual void CloseInternal()
		{
			using (StorageLock.AcquireExclusiveUsing())
			{
				this.RangeChanged = null;
				DataProvider?.Close();
			}
			if (MyVoxelOperationsSessionComponent.EnableCache)
			{
				OperationsComponent?.Remove(this);
			}
		}

		public void Close()
		{
			int num = m_pinAndCloseMark;
			while (true)
			{
				if (((uint)num & 0x80000000u) != 0)
				{
					return;
				}
				int num2 = Interlocked.CompareExchange(ref m_pinAndCloseMark, num | int.MinValue, num);
				if (num == num2)
				{
					break;
				}
				num = num2;
			}
			if ((num & 0x7FFFFFFF) == 0)
			{
				PerformClose();
			}
		}

		public StoragePin Pin()
		{
			if ((Interlocked.Increment(ref m_pinAndCloseMark) & int.MinValue) == 0)
			{
				return new StoragePin(this);
			}
			if ((Interlocked.Decrement(ref m_pinAndCloseMark) & 0x7FFFFFFF) == 0)
			{
				PerformClose();
			}
			return new StoragePin(null);
		}

		public void Unpin()
		{
			if (Interlocked.Decrement(ref m_pinAndCloseMark) == int.MinValue)
			{
				PerformClose();
			}
		}

		public static string GetStoragePath(string storageName)
		{
			return Path.Combine(MySession.Static.CurrentPath, storageName + ".vx2");
		}

		void VRage.ModAPI.IMyStorage.ReadRange(MyStorageData target, MyStorageDataTypeFlags dataToRead, int lodIndex, Vector3I lodVoxelRangeMin, Vector3I lodVoxelRangeMax)
		{
			if ((uint)lodIndex < 16u)
			{
				ReadRange(target, dataToRead, lodIndex, lodVoxelRangeMin, lodVoxelRangeMax);
			}
		}

		void VRage.ModAPI.IMyStorage.WriteRange(MyStorageData source, MyStorageDataTypeFlags dataToWrite, Vector3I voxelRangeMin, Vector3I voxelRangeMax, bool notify, bool skipCache)
		{
			WriteRange(source, dataToWrite, voxelRangeMin, voxelRangeMax, notify, skipCache);
		}

		void VRage.ModAPI.IMyStorage.DeleteRange(MyStorageDataTypeFlags dataToWrite, Vector3I voxelRangeMin, Vector3I voxelRangeMax, bool notify)
		{
			DeleteRange(dataToWrite, voxelRangeMin, voxelRangeMax, notify);
		}

		void VRage.ModAPI.IMyStorage.ExecuteOperationFast<TVoxelOperator>(ref TVoxelOperator voxelOperator, MyStorageDataTypeFlags dataToWrite, ref Vector3I voxelRangeMin, ref Vector3I voxelRangeMax, bool notifyRangeChanged)
		{
			ExecuteOperationFast(ref voxelOperator, dataToWrite, ref voxelRangeMin, ref voxelRangeMax, notifyRangeChanged, skipCache: false);
		}

		void VRage.ModAPI.IMyStorage.NotifyRangeChanged(ref Vector3I voxelRangeMin, ref Vector3I voxelRangeMax, MyStorageDataTypeFlags dataChanged)
		{
			OnRangeChanged(voxelRangeMin, voxelRangeMax, dataChanged);
		}

		public void OverwriteAllMaterials(byte materialIndex)
		{
		}
	}
}
