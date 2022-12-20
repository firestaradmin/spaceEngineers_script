using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ParallelTasks;
using SharpDX;
using SharpDX.Direct3D11;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Import;
using VRage.Library;
using VRage.Library.Collections;
using VRage.Library.Extensions;
using VRage.Network;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Utils;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Import;
using VRageRender.Messages;
using VRageRender.Vertex;
using VRageRender.Voxels;

namespace VRageRender
{
	internal static class MyMeshes
	{
		public class MyVoxelRenderDataProcessorProvider : Singleton<MyVoxelRenderDataProcessorProvider>, IMyVoxelRenderDataProcessorProvider
		{
			public static MyConcurrentPool<MyVoxelRenderDataProcessor> VoxelRenderDataProcessorPool = new MyConcurrentPool<MyVoxelRenderDataProcessor>();

			public IMyVoxelRenderDataProcessor GetRenderDataProcessor(int vertexCount, int indexCount, int partsCount)
			{
				MyVoxelRenderDataProcessor myVoxelRenderDataProcessor = VoxelRenderDataProcessorPool.Get();
				myVoxelRenderDataProcessor.Init(vertexCount, indexCount, partsCount);
				return myVoxelRenderDataProcessor;
			}
		}

		[GenerateActivator]
		public class MyVoxelRenderDataProcessor : IMyVoxelRenderDataProcessor
		{
			private class VRageRender_MyMeshes_003C_003EMyVoxelRenderDataProcessor_003C_003EActor : IActivator, IActivator<MyVoxelRenderDataProcessor>
			{
				private sealed override object CreateInstance()
				{
					return new MyVoxelRenderDataProcessor();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyVoxelRenderDataProcessor CreateInstance()
				{
					return new MyVoxelRenderDataProcessor();
				}

				MyVoxelRenderDataProcessor IActivator<MyVoxelRenderDataProcessor>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private bool m_shortIndices;

			private NativeArray m_indices;

			private NativeArray m_normals;

			private NativeArray m_vertices;

			private MyVoxelMeshPartIndex[] m_parts;

			private int partIndex;

			private int runningIndexOffset;

			private int runningVertexOffset;

			public void Init(int vertexCount, int indexCount, int partsCount)
			{
				partIndex = 0;
				runningIndexOffset = 0;
				runningVertexOffset = 0;
				m_shortIndices = vertexCount <= 65535;
				int num = (m_shortIndices ? 2 : 4);
				m_parts = new MyVoxelMeshPartIndex[partsCount];
				AllocBuffer(ref m_indices, num * indexCount);
				AllocBuffer(ref m_normals, MyVertexFormatVoxelNormal.STRIDE * vertexCount);
				AllocBuffer(ref m_vertices, MyVertexFormatVoxelPosition.STRIDE * vertexCount);
			}

			public unsafe void AddPart(MyList<MyVertexFormatVoxelSingleData> vertices, ushort* sourceIndices, int indicesCount, MyVoxelMaterialTriple material)
			{
				m_parts[partIndex].StartIndex = runningIndexOffset;
				m_parts[partIndex].IndexCount = indicesCount;
				m_parts[partIndex].Materials = material;
				fixed (MyVertexFormatVoxelSingleData* sourcePointer = vertices.GetInternalArray())
				{
					SplitVoxelVertexStream(sourcePointer, (MyVertexFormatVoxelPosition*)((byte*)(void*)m_vertices.Ptr + (long)runningVertexOffset * (long)sizeof(MyVertexFormatVoxelPosition)), (MyVertexFormatVoxelNormal*)((byte*)(void*)m_normals.Ptr + (long)runningVertexOffset * (long)sizeof(MyVertexFormatVoxelNormal)), vertices.Count);
				}
				if (m_shortIndices)
				{
					ushort* ptr = (ushort*)((byte*)(void*)m_indices.Ptr + (long)runningIndexOffset * 2L);
					int num = 0;
					while (num < indicesCount)
					{
						*ptr = (ushort)(*sourceIndices + runningVertexOffset);
						num++;
						ptr++;
						sourceIndices++;
					}
				}
				else
				{
					uint* ptr2 = (uint*)((byte*)(void*)m_indices.Ptr + (long)runningIndexOffset * 4L);
					int num2 = 0;
					while (num2 < indicesCount)
					{
						*ptr2 = (uint)(*sourceIndices + runningVertexOffset);
						num2++;
						ptr2++;
						sourceIndices++;
					}
				}
				runningIndexOffset += indicesCount;
				runningVertexOffset += vertices.Count;
				partIndex++;
			}

			public void GetDataAndDispose(ref MyVoxelRenderCellData data)
			{
				data.Parts = m_parts;
				data.Normals = m_normals;
				data.Indices = m_indices;
				data.Vertices = m_vertices;
				data.ShortIndices = m_shortIndices;
				m_parts = null;
				m_normals = null;
				m_indices = null;
				m_vertices = null;
				MyVoxelRenderDataProcessorProvider.VoxelRenderDataProcessorPool.Return(this);
			}

			private unsafe static void SplitVoxelVertexStream(MyVertexFormatVoxelSingleData* sourcePointer, MyVertexFormatVoxelPosition* destinationPointer0, MyVertexFormatVoxelNormal* destinationPointer1, int elementsToCopy)
			{
				MyVertexFormatVoxelPosition* ptr = destinationPointer0;
				MyVertexFormatVoxelNormal* ptr2 = destinationPointer1;
				MyVertexFormatVoxelSingleData* ptr3 = sourcePointer;
				for (int i = 0; i < elementsToCopy; i++)
				{
					*(long*)ptr = *(long*)ptr3;
					*(long*)((byte*)ptr + 8) = *(long*)((byte*)ptr3 + 8);
					*(long*)ptr2 = *(long*)((byte*)ptr3 + 2L * 8L);
					ptr++;
					ptr2++;
					ptr3++;
				}
			}
		}

		[GenerateActivator]
		private class MyDeviceUpdateBatch : IMyUpdateBatch, IMyVoxelUpdateBatch, IDisposable
		{
			private class VRageRender_MyMeshes_003C_003EMyDeviceUpdateBatch_003C_003EActor : IActivator, IActivator<MyDeviceUpdateBatch>
			{
				private sealed override object CreateInstance()
				{
					return new MyDeviceUpdateBatch();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyDeviceUpdateBatch CreateInstance()
				{
					return new MyDeviceUpdateBatch();
				}

				MyDeviceUpdateBatch IActivator<MyDeviceUpdateBatch>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private Task m_task;

			private int m_finishedBuffers;

			private int m_allocatedBuffers;

			private int m_buffersToConsume;

			private readonly IDeviceWriteBuffer m_writeBuffer = MyDeviceWriteBuffer.CreateWriteBuffer(10485760);

			private readonly List<(MyGenericBuffer Buffer, NativeArray Data, int Bytes, int Stride)> m_data = new List<(MyGenericBuffer, NativeArray, int, int)>();

			private bool m_isOpen;

			private Action m_AllocBuffers;

			public MyDeviceUpdateBatch()
			{
				m_AllocBuffers = AllocBuffers;
			}

			public void Open()
			{
				m_isOpen = true;
			}

			public MyGenericBuffer Add(NativeArray data, int bytes, int stride)
			{
				if (data == null)
				{
					return null;
				}
				if (bytes <= 0)
				{
					return null;
				}
				MyGenericBuffer myGenericBuffer = MeshBufferAllocator.PreAllocate(bytes);
				m_data.Add((myGenericBuffer, data, bytes, stride));
				Interlocked.Increment(ref m_buffersToConsume);
				if (m_buffersToConsume > m_allocatedBuffers + 5 && m_task.IsComplete)
				{
					m_task = Parallel.Start(WorkPriority.Normal, m_AllocBuffers);
				}
				return myGenericBuffer;
			}

			private void AllocBuffers()
			{
				do
				{
					int num = Interlocked.Increment(ref m_allocatedBuffers) - 1;
					if (num >= m_buffersToConsume)
					{
						break;
					}
					var (myGenericBuffer, _, _, stride) = m_data[num];
					MeshBufferAllocator.AllocateResource(myGenericBuffer);
					myGenericBuffer.SetStride(stride);
				}
				while (Interlocked.Increment(ref m_finishedBuffers) != m_buffersToConsume);
			}

			public void Commit(bool allowEmpty = false)
			{
				MyRenderContext rC = MyRender11.RC;
				int num = 0;
				foreach (var datum in m_data)
				{
					int item = datum.Bytes;
					num += item;
				}
				if (num > 0)
				{
					var (myMapping, source, myQuery, num2) = m_writeBuffer.Alloc(num);
					foreach (var datum2 in m_data)
					{
						NativeArray item2 = datum2.Data;
						int item3 = datum2.Bytes;
						myMapping.WriteAndPosition(item2, item3);
						NativeArray array = item2;
						ReturnBuffer(ref array);
					}
					myMapping.Unmap();
					if (m_finishedBuffers != m_buffersToConsume)
					{
						AllocBuffers();
					}
					m_task.WaitOrExecute(blocking: true);
					foreach (var (myGenericBuffer, _, num3, _) in m_data)
					{
						if (myGenericBuffer != null)
						{
							_ = myGenericBuffer.BufferSize;
							rC.CopySubresourceRegion(source, 0, new ResourceRegion
							{
								Left = num2,
								Right = num2 + num3,
								Top = 0,
								Bottom = 1,
								Front = 0,
								Back = 1
							}, myGenericBuffer, 0);
						}
						num2 += num3;
					}
					rC.End((Query)myQuery);
				}
				m_data.Clear();
				m_task = default(Task);
				m_finishedBuffers = 0;
				m_buffersToConsume = 0;
				m_allocatedBuffers = 0;
				m_isOpen = false;
			}

			public void Dispose()
			{
				m_writeBuffer.Dispose();
			}
		}

		internal static NativeArrayAllocator BufferAllocator = new NativeArrayAllocator(MyManagers.Buffers.BufferMemoryTacker.RegisterSubsystem("MeshBuffers"));

		internal static MyGenericBufferAllocator MeshBufferAllocator = new MyGenericBufferAllocator(createQuery: false, new BufferDescription(0, ResourceUsage.Default, BindFlags.VertexBuffer | BindFlags.IndexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 16), MyManagers.Buffers.BufferMemoryTacker.RegisterSubsystem("Mesh GPU Buffers"));

		private static readonly Dictionary<MyStringId, MeshId> MeshNameIndex = new Dictionary<MyStringId, MeshId>(MyStringId.Comparer);

		private static readonly Dictionary<MyStringId, MeshId> RuntimeMeshNameIndex = new Dictionary<MyStringId, MeshId>(MyStringId.Comparer);

		internal static readonly MyFreelist<MyMeshInfo> MeshInfos = new MyFreelist<MyMeshInfo>(4096);

		internal static readonly MyFreelist<MyLodMeshInfo> LodMeshInfos = new MyFreelist<MyLodMeshInfo>(4096);

		private static readonly Dictionary<MyLodMesh, LodMeshId> LodMeshIndex = new Dictionary<MyLodMesh, LodMeshId>(MyLodMesh.Comparer);

		internal static MyMeshBuffers[] LodMeshBuffers = new MyMeshBuffers[4096];

		internal static readonly MyFreelist<MyMeshPartInfo1> Parts = new MyFreelist<MyMeshPartInfo1>(8192);

		internal static readonly MyFreelist<MyMeshSectionInfo1> Sections = new MyFreelist<MyMeshSectionInfo1>(8192);

		private static readonly Dictionary<MeshId, MyVoxelCellInfo> MeshVoxelInfo = new Dictionary<MeshId, MyVoxelCellInfo>(MeshId.Comparer);

		private static readonly Dictionary<MyMeshPart, VoxelPartId> VoxelPartIndex = new Dictionary<MyMeshPart, VoxelPartId>(MyMeshPart.Comparer);

		private static readonly Dictionary<MyMeshPart, MeshPartId> PartIndex = new Dictionary<MyMeshPart, MeshPartId>(MyMeshPart.Comparer);

		private static readonly Dictionary<MyMeshSection, MeshSectionId> SectionIndex = new Dictionary<MyMeshSection, MeshSectionId>(MyMeshSection.Comparer);

		internal static readonly MyFreelist<MyVoxelPartInfo1> VoxelParts = new MyFreelist<MyVoxelPartInfo1>(2048);

		private static readonly Dictionary<MeshId, MyRuntimeMeshPersistentInfo> InterSessionData = new Dictionary<MeshId, MyRuntimeMeshPersistentInfo>(MeshId.Comparer);

		private static HashSet<MeshId>[] State;

		internal static VertexLayoutId VoxelLayout = VertexLayoutId.NULL;

		private static ushort[] m_tmpShortIndices;

<<<<<<< HEAD
		private static readonly Dictionary<MyMeshMaterialId, List<int>> MergableParts = new Dictionary<MyMeshMaterialId, List<int>>(MyMeshMaterialId.Comparer);
=======
		private static readonly Dictionary<MyMeshMaterialId, List<int>> MergableParts = new Dictionary<MyMeshMaterialId, List<int>>(new MyMeshMaterialId.CustomMergingEqualityComparer());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private static readonly Dictionary<MyMeshMaterialId, List<int>> NonMergableParts = new Dictionary<MyMeshMaterialId, List<int>>();

		private static readonly Dictionary<MyMeshMaterialId, List<int>> TempParts = new Dictionary<MyMeshMaterialId, List<int>>();

		private static int m_mergedPartCounter;

		private const string ERROR_MODEL_PATH = "Models/Debug/Error.mwm";

		private static MyDeviceUpdateBatch m_deviceUpdateBatchCache;

		private static MyDeviceUpdateBatch m_meshUpdateBatch;

		private static MyDeviceUpdateBatch DeviceUpdateBatch
		{
			get
			{
				if (m_deviceUpdateBatchCache == null)
				{
					m_deviceUpdateBatchCache = new MyDeviceUpdateBatch();
				}
				m_deviceUpdateBatchCache.Open();
				return m_deviceUpdateBatchCache;
			}
		}

		internal static void Init()
		{
			int num = Enum.GetNames(typeof(MyMeshState)).Length;
			State = new HashSet<MeshId>[num];
			for (int i = 0; i < num; i++)
			{
<<<<<<< HEAD
				State[i] = new HashSet<MeshId>(MeshId.Comparer);
=======
				State[i] = new HashSet<MeshId>((IEqualityComparer<MeshId>)MeshId.Comparer);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			for (int j = 0; j < LodMeshBuffers.Length; j++)
			{
				LodMeshBuffers[j] = MyMeshBuffers.Empty;
			}
			VoxelLayout = MyVertexLayouts.GetLayout(new MyVertexInputComponent(MyVertexInputComponentType.VOXEL_POSITION_MAT), new MyVertexInputComponent(MyVertexInputComponentType.VOXEL_NORMAL, 1));
			m_mergedPartCounter = 0;
		}

		internal static bool Exists(string name)
		{
			MyStringId key = X.TEXT_(name);
			if (!MeshNameIndex.ContainsKey(key))
			{
				return RuntimeMeshNameIndex.ContainsKey(key);
			}
			return true;
		}

		internal static bool IsVoxelMesh(MeshId mesh)
		{
			return MeshVoxelInfo.ContainsKey(mesh);
		}

		internal static MyVoxelCellInfo GetVoxelInfo(MeshId mesh)
		{
			return MeshVoxelInfo[mesh];
		}

		internal static LodMeshId GetLodMesh(MeshId mesh, int lod)
		{
			MyLodMesh myLodMesh = default(MyLodMesh);
			myLodMesh.Mesh = mesh;
			myLodMesh.Lod = lod;
			MyLodMesh key = myLodMesh;
			return LodMeshIndex[key];
		}

		internal static bool TryGetLodMesh(MeshId mesh, int lod, out LodMeshId lodMeshId)
		{
			MyLodMesh myLodMesh = default(MyLodMesh);
			myLodMesh.Mesh = mesh;
			myLodMesh.Lod = lod;
			MyLodMesh key = myLodMesh;
			return LodMeshIndex.TryGetValue(key, out lodMeshId);
		}

		internal static MeshPartId GetMeshPart(MeshId mesh, int lod, int part)
		{
			return PartIndex[new MyMeshPart
			{
				Mesh = mesh,
				Lod = lod,
				Part = part
			}];
		}

		internal static bool TryGetMeshPart(MeshId mesh, int lod, int part, out MeshPartId partId)
		{
			return PartIndex.TryGetValue(new MyMeshPart
			{
				Mesh = mesh,
				Lod = lod,
				Part = part
			}, out partId);
		}

		internal static MeshSectionId GetMeshSection(MeshId mesh, int lod, string section)
		{
			return SectionIndex[new MyMeshSection
			{
				Mesh = mesh,
				Lod = lod,
				Section = section
			}];
		}

		internal static bool TryGetMeshSection(MeshId mesh, int lod, string section, out MeshSectionId sectionId)
		{
			return SectionIndex.TryGetValue(new MyMeshSection
			{
				Mesh = mesh,
				Lod = lod,
				Section = section
			}, out sectionId);
		}

		internal static VoxelPartId GetVoxelPart(MeshId mesh, int part)
		{
			return VoxelPartIndex[new MyMeshPart
			{
				Mesh = mesh,
				Lod = 0,
				Part = part
			}];
		}

		private static void InitState(MeshId id, MyMeshState state)
		{
			State[(int)state].Add(id);
		}

		private static void MoveState(MeshId id, MyMeshState from, MyMeshState to)
		{
			State[(int)from].Remove(id);
			State[(int)to].Add(id);
		}

		private static bool CheckState(MeshId id, MyMeshState state)
		{
			return State[(int)state].Contains(id);
		}

		internal static void ClearState(MeshId id)
		{
			HashSet<MeshId>[] state = State;
			for (int i = 0; i < state.Length; i++)
			{
				state[i].Remove(id);
			}
		}

		internal static MeshId GetMeshId(MyStringId nameKey, float rescale, IMyVoxelUpdateBatch updateBatch = null)
		{
			if (RuntimeMeshNameIndex.ContainsKey(nameKey))
			{
				return RuntimeMeshNameIndex[nameKey];
			}
			if (!MeshNameIndex.ContainsKey(nameKey))
			{
				MeshId meshId = default(MeshId);
				meshId.Index = MeshInfos.Allocate();
				MeshId meshId2 = meshId;
				MeshNameIndex[nameKey] = meshId2;
				MeshInfos.Data[meshId2.Index] = new MyMeshInfo
				{
					Name = nameKey.ToString(),
					NameKey = nameKey,
					LodsNum = -1,
					Rescale = rescale
				};
				InitState(meshId2, MyMeshState.WAITING);
				LoadMesh(meshId2, updateBatch);
				MoveState(meshId2, MyMeshState.WAITING, MyMeshState.LOADED);
			}
			return MeshNameIndex[nameKey];
		}

		internal static void RemoveMesh(MeshId model)
		{
			int lodsNum = model.Info.LodsNum;
			for (int i = 0; i < lodsNum; i++)
			{
				LodMeshId lodMesh = GetLodMesh(model, i);
				DisposeLodMeshBuffers(lodMesh);
				int partsNum = LodMeshInfos.Data[lodMesh.Index].PartsNum;
				for (int j = 0; j < partsNum; j++)
				{
					MeshPartId meshPart = GetMeshPart(model, i, j);
					Parts.Free(meshPart.Index);
				}
				DisposeLodMeshInfos(lodMesh);
				LodMeshIndex.Remove(new MyLodMesh
				{
					Mesh = model,
					Lod = i
				});
			}
			if (model.Info.NameKey != MyStringId.NullOrEmpty)
			{
				MeshNameIndex.Remove(model.Info.NameKey);
				RuntimeMeshNameIndex.Remove(model.Info.NameKey);
			}
			MeshInfos.Free(model.Index);
		}

<<<<<<< HEAD
		internal static void RemoveMesh(MyStringId nameKey)
		{
			if (RuntimeMeshNameIndex.TryGetValue(nameKey, out var value) || MeshNameIndex.TryGetValue(nameKey, out value))
			{
				RemoveMesh(value);
			}
		}

		internal static void OnSessionEnd()
		{
			bool flag = true;
			MeshId[] array = RuntimeMeshNameIndex.Values.ToArray();
=======
		internal static void OnSessionEnd()
		{
			bool flag = true;
			MeshId[] array = Enumerable.ToArray<MeshId>((IEnumerable<MeshId>)RuntimeMeshNameIndex.Values);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			for (int i = 0; i < array.Length; i++)
			{
				MeshId model = array[i];
				if (!(model.Info.RuntimeGenerated && !model.Info.Dynamic && flag))
				{
					RemoveMesh(model);
				}
			}
<<<<<<< HEAD
			array = MeshVoxelInfo.Keys.ToArray();
=======
			array = Enumerable.ToArray<MeshId>((IEnumerable<MeshId>)MeshVoxelInfo.Keys);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			for (int i = 0; i < array.Length; i++)
			{
				RemoveVoxelCell(array[i]);
			}
			MeshVoxelInfo.Clear();
			VoxelParts.Clear();
			VoxelPartIndex.Clear();
			RuntimeMeshNameIndex.Clear();
			for (int j = 0; j < Enum.GetNames(typeof(MyMeshState)).Length; j++)
			{
				State[j].Clear();
			}
		}

		private static LodMeshId NewLodMesh(MeshId mesh, int lod)
		{
			LodMeshId lodMeshId = default(LodMeshId);
			lodMeshId.Index = LodMeshInfos.Allocate();
			LodMeshId lodMeshId2 = lodMeshId;
			LodMeshInfos.Data[lodMeshId2.Index] = default(MyLodMeshInfo);
			int lod2 = ((!IsVoxelMesh(mesh)) ? lod : 0);
			MyLodMesh myLodMesh = default(MyLodMesh);
			myLodMesh.Mesh = mesh;
			myLodMesh.Lod = lod2;
			MyLodMesh key = myLodMesh;
			LodMeshIndex[key] = lodMeshId2;
			MyArrayHelpers.Reserve(ref LodMeshBuffers, lodMeshId2.Index + 1);
			LodMeshBuffers[lodMeshId2.Index] = MyMeshBuffers.Empty;
			return lodMeshId2;
		}

		private static MeshPartId NewMeshPart(MeshId mesh, int lod, int part)
		{
			MeshPartId meshPartId = default(MeshPartId);
			meshPartId.Index = Parts.Allocate();
			MeshPartId meshPartId2 = meshPartId;
			Parts.Data[meshPartId2.Index] = default(MyMeshPartInfo1);
			PartIndex[new MyMeshPart
			{
				Mesh = mesh,
				Lod = lod,
				Part = part
			}] = meshPartId2;
			return meshPartId2;
		}

		private static MeshSectionId NewMeshSection(MeshId mesh, int lod, string section)
		{
			MeshSectionId meshSectionId = default(MeshSectionId);
			meshSectionId.Index = Sections.Allocate();
			MeshSectionId meshSectionId2 = meshSectionId;
			Sections.Data[meshSectionId2.Index] = default(MyMeshSectionInfo1);
			SectionIndex[new MyMeshSection
			{
				Mesh = mesh,
				Lod = lod,
				Section = section
			}] = meshSectionId2;
			return meshSectionId2;
		}

		private static LodMeshId StoreLodMeshWithParts(MeshId mesh, int lodIndex, MyLodMeshInfo lodMeshInfo, MyMeshPartInfo1[] parts)
		{
			LodMeshId result = NewLodMesh(mesh, lodIndex);
			LodMeshInfos.Data[result.Index] = lodMeshInfo;
			if (parts != null)
			{
				for (int i = 0; i < parts.Length; i++)
				{
					MeshPartId meshPartId = NewMeshPart(mesh, lodIndex, i);
					Parts.Data[meshPartId.Index] = parts[i];
				}
			}
			return result;
		}

		private static void StoreLodMeshSections(MeshId mesh, int lodIndex, ref MyMeshSectionInfo1[] sections)
		{
			if (sections != null)
			{
				for (int i = 0; i < sections.Length; i++)
				{
					string name = sections[i].Name;
					MeshSectionId meshSectionId = NewMeshSection(mesh, lodIndex, name);
					Sections.Data[meshSectionId.Index] = sections[i];
				}
			}
		}

		internal static MeshId CreateRuntimeMesh(MyStringId nameKey, int parts, bool dynamic)
		{
			MeshId meshId = default(MeshId);
			meshId.Index = MeshInfos.Allocate();
			MeshId meshId2 = meshId;
			RuntimeMeshNameIndex[nameKey] = meshId2;
			MeshInfos.Data[meshId2.Index] = new MyMeshInfo
			{
				Name = nameKey.ToString(),
				NameKey = nameKey,
				LodsNum = 1,
				Dynamic = dynamic,
				RuntimeGenerated = true
			};
			MyLodMeshInfo myLodMeshInfo = default(MyLodMeshInfo);
			myLodMeshInfo.PartsNum = parts;
			MyLodMeshInfo lodMeshInfo = myLodMeshInfo;
			MyMeshPartInfo1[] parts2 = new MyMeshPartInfo1[parts];
			StoreLodMeshWithParts(meshId2, 0, lodMeshInfo, parts2);
			return meshId2;
		}

		internal static void RefreshMaterialIds(MeshId mesh)
		{
			MyRuntimeSectionInfo[] sections = InterSessionData[mesh].Sections;
			for (int i = 0; i < sections.Length; i++)
			{
				MeshPartId meshPartId = PartIndex[new MyMeshPart
				{
					Mesh = mesh,
					Lod = 0,
					Part = i
				}];
				Parts.Data[meshPartId.Index].Material = MyMeshMaterials1.GetMaterialId(sections[i].MaterialName);
			}
		}

		internal static void UpdateRuntimeMesh(MeshId mesh, ushort[] indices, MyVertexFormatPositionH4[] stream0, MyVertexFormatTexcoordNormalTangentTexindices[] stream1, MyRuntimeSectionInfo[] sections, BoundingBox aabb)
		{
			InterSessionData[mesh] = new MyRuntimeMeshPersistentInfo
			{
				Sections = sections
			};
			LodMeshId lodMeshId = LodMeshIndex[new MyLodMesh
			{
				Mesh = mesh,
				Lod = 0
			}];
			long num = stream0.LongLength;
			long num2 = indices.LongLength;
			FillIndexData(ref LodMeshInfos.Data[lodMeshId.Index].Data, indices, (int)num2);
			FillStream0Data(ref LodMeshInfos.Data[lodMeshId.Index].Data, stream0, (int)num);
			FillStream1Data(ref LodMeshInfos.Data[lodMeshId.Index].Data, stream1, (int)num);
			LodMeshInfos.Data[lodMeshId.Index].BoundingBox = aabb;
			MyVertexInputComponent[] components = new MyVertexInputComponent[4]
			{
				new MyVertexInputComponent(MyVertexInputComponentType.POSITION_PACKED),
				new MyVertexInputComponent(MyVertexInputComponentType.NORMAL, 1),
				new MyVertexInputComponent(MyVertexInputComponentType.TANGENT_SIGN_OF_BITANGENT, 1),
				new MyVertexInputComponent(MyVertexInputComponentType.TEXCOORD0_H, 1)
			};
			LodMeshInfos.Data[lodMeshId.Index].Data.VertexLayout = MyVertexLayouts.GetLayout(components);
			for (int i = 0; i < sections.Length; i++)
			{
				MeshPartId meshPartId = PartIndex[new MyMeshPart
				{
					Mesh = mesh,
					Lod = 0,
					Part = i
				}];
				Parts.Data[meshPartId.Index].StartIndex = sections[i].IndexStart;
				Parts.Data[meshPartId.Index].IndexCount = sections[i].TriCount * 3;
				Parts.Data[meshPartId.Index].Material = MyMeshMaterials1.GetMaterialId(sections[i].MaterialName);
			}
			LodMeshInfos.Data[lodMeshId.Index].IndicesNum = (int)num2;
			LodMeshInfos.Data[lodMeshId.Index].VerticesNum = (int)num;
			if (LodMeshInfos.Data[lodMeshId.Index].VerticesNum > 0)
			{
				MoveData(lodMeshId);
			}
		}

		private static void MoveData(LodMeshId lodMeshId, MyDeviceUpdateBatch updateBatch = null)
		{
			MyLodMeshInfo myLodMeshInfo = LodMeshInfos.Data[lodMeshId.Index];
			if (!myLodMeshInfo.NullLodMesh)
			{
				DisposeLodMeshBuffers(lodMeshId);
				MoveData(lodMeshId.Info.VerticesNum, lodMeshId.Info.IndicesNum, ref LodMeshInfos.Data[lodMeshId.Index].Data, ref LodMeshBuffers[lodMeshId.Index], myLodMeshInfo.FileName, updateBatch);
			}
		}

		private static void MoveData(int vertexCount, int indexCount, ref MyMeshRawData rawData, ref MyMeshBuffers meshBuffer, string assetName, MyDeviceUpdateBatch updateBatch = null)
		{
			bool flag = false;
			if (updateBatch == null)
			{
				flag = true;
				updateBatch = DeviceUpdateBatch;
			}
			meshBuffer.VB0 = AddToBatch(ref rawData.VertexStream0, vertexCount, rawData.Stride0, updateBatch);
			if (rawData.Stride1 > 0)
			{
				meshBuffer.VB1 = AddToBatch(ref rawData.VertexStream1, vertexCount, rawData.Stride1, updateBatch);
			}
			int bufferStride2 = ((rawData.IndicesFmt == MyIndexBufferFormat.UInt) ? 4 : 2);
			MyGenericBuffer myGenericBuffer = AddToBatch(ref rawData.Indices, indexCount, bufferStride2, updateBatch);
			if (myGenericBuffer != null)
			{
				myGenericBuffer.Format = rawData.IndicesFmt;
			}
			meshBuffer.IB = myGenericBuffer;
			if (flag)
			{
				updateBatch.Commit();
			}
<<<<<<< HEAD
			MyGenericBuffer AddToBatch(ref NativeArray data, int elementCount, int bufferStride, MyDeviceUpdateBatch batch)
=======
			static MyGenericBuffer AddToBatch(ref NativeArray data, int elementCount, int bufferStride, MyDeviceUpdateBatch batch)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				int bytes = elementCount * bufferStride;
				MyGenericBuffer result = batch.Add(data, bytes, bufferStride);
				data = null;
				return result;
			}
		}

		internal static MeshId CreateVoxelCell(Vector3I coord, int lod)
		{
			MeshId meshId = default(MeshId);
			meshId.Index = MeshInfos.Allocate();
			MeshId meshId2 = meshId;
			MeshVoxelInfo[meshId2] = new MyVoxelCellInfo
			{
				Coord = coord,
				Lod = lod
			};
			string name = $"VoxelCell {coord} Lod {lod}";
			MeshInfos.Data[meshId2.Index] = new MyMeshInfo
			{
				Name = name,
				NameKey = MyStringId.NullOrEmpty,
				LodsNum = 1,
				Dynamic = false,
				RuntimeGenerated = true
			};
			MyLodMeshInfo myLodMeshInfo = default(MyLodMeshInfo);
			myLodMeshInfo.PartsNum = 0;
			myLodMeshInfo.Name = name;
			MyLodMeshInfo lodMeshInfo = myLodMeshInfo;
			MyMeshPartInfo1[] parts = new MyMeshPartInfo1[0];
			LodMeshId lodMeshId = StoreLodMeshWithParts(meshId2, lod, lodMeshInfo, parts);
			LodMeshInfos.Data[lodMeshId.Index].Data.VertexLayout = VoxelLayout;
			return meshId2;
		}

		internal static void RemoveVoxelCell(MeshId id)
		{
			if (MeshVoxelInfo.ContainsKey(id))
			{
				MyLodMesh myLodMesh = default(MyLodMesh);
				myLodMesh.Mesh = id;
				myLodMesh.Lod = 0;
				MyLodMesh key = myLodMesh;
				if (LodMeshIndex.ContainsKey(key))
				{
					LodMeshId lodMeshId = LodMeshIndex[key];
					ResizeVoxelParts(id, lodMeshId, 0);
					DisposeLodMeshBuffers(lodMeshId);
					DisposeLodMeshInfos(lodMeshId);
					LodMeshIndex.Remove(key);
				}
				MeshInfos.Free(id.Index);
				MeshVoxelInfo.Remove(id);
			}
		}

		/// <summary>
		/// Update the contents of a designated voxel mesh.
		/// </summary>
		internal static void UpdateVoxelCell(MeshId mesh, ref MyVoxelRenderCellData data, IMyVoxelUpdateBatch updateBatch)
		{
			for (int i = 0; i < data.Parts.Length; i++)
			{
				if (data.Parts[i].Materials.I0 != byte.MaxValue && !MyVoxelMaterials.IsMaterialValid(data.Parts[i].Materials.I0))
				{
					data.Parts[i].Materials.I0 = 0;
				}
				if (data.Parts[i].Materials.I1 != byte.MaxValue && !MyVoxelMaterials.IsMaterialValid(data.Parts[i].Materials.I1))
				{
					data.Parts[i].Materials.I1 = 0;
				}
				if (data.Parts[i].Materials.I2 != byte.MaxValue && !MyVoxelMaterials.IsMaterialValid(data.Parts[i].Materials.I2))
				{
					data.Parts[i].Materials.I2 = 0;
				}
			}
			MyLodMesh myLodMesh = default(MyLodMesh);
			myLodMesh.Mesh = mesh;
			myLodMesh.Lod = 0;
			MyLodMesh key = myLodMesh;
			if (LodMeshIndex.TryGetValue(key, out var value))
			{
				ResizeVoxelParts(mesh, value, data.Parts.Length);
				CalculateMultimaterialStart(data.Parts, out var multimaterialOffset);
				SplitVoxelStreams(ref data, ref LodMeshInfos.Data[value.Index].Data);
				PrepareVoxelMaterials(mesh, ref data);
				ref MyLodMeshInfo reference = ref LodMeshInfos[value.Index];
				reference.BoundingBox = data.CellBounds;
				reference.VerticesNum = data.VertexCount;
				reference.IndicesNum = data.IndexCount;
				reference.MultimaterialOffset = multimaterialOffset;
				reference.PartOffsets = data.Parts;
				if (updateBatch != null)
				{
					UpdateVoxelCellOnDevice(mesh, updateBatch);
				}
			}
		}

		public static void UpdateVoxelCellOnDevice(MeshId mesh, IMyVoxelUpdateBatch updateBatch = null)
		{
			MoveData(GetLodMesh(mesh, 0), (MyDeviceUpdateBatch)updateBatch);
		}

		private static void CalculateMultimaterialStart(MyVoxelMeshPartIndex[] parts, out int multimaterialOffset)
		{
			multimaterialOffset = parts[parts.Length - 1].StartIndex + parts[parts.Length - 1].IndexCount;
			for (int i = 0; i < parts.Length; i++)
			{
				if (parts[i].Materials.MultiMaterial)
				{
					multimaterialOffset = parts[i].StartIndex;
					break;
				}
			}
		}

		private unsafe static void CopyIndices(uint* source, void* destination, int destinationStride, int count)
		{
			switch (destinationStride)
			{
			case 2:
			{
				for (int i = 0; i < count; i++)
				{
					*(ushort*)((byte*)destination + (long)i * 2L) = (ushort)source[i];
				}
				break;
			}
			case 4:
				System.Buffer.MemoryCopy(source, destination, count, count);
				break;
			}
		}

		private static void SplitVoxelStreams(ref MyVoxelRenderCellData data, ref MyMeshRawData renderData)
		{
			ReturnBuffer(ref renderData.Indices);
			ReturnBuffer(ref renderData.VertexStream0);
			ReturnBuffer(ref renderData.VertexStream1);
			renderData.Indices = data.Indices;
			renderData.VertexStream1 = data.Normals;
			renderData.VertexStream0 = data.Vertices;
			renderData.Stride1 = MyVertexFormatVoxelNormal.STRIDE;
			renderData.Stride0 = MyVertexFormatVoxelPosition.STRIDE;
			renderData.IndicesFmt = (data.ShortIndices ? MyIndexBufferFormat.UShort : MyIndexBufferFormat.UInt);
		}

		private static void PrepareVoxelMaterials(MeshId mesh, ref MyVoxelRenderCellData data)
		{
			for (int i = 0; i < data.Parts.Length; i++)
			{
				MyVoxelMaterialTriple materials = data.Parts[i].Materials;
				VoxelPartId voxelPartId = VoxelPartIndex[new MyMeshPart
				{
					Mesh = mesh,
					Lod = 0,
					Part = i
				}];
				VoxelParts.Data[voxelPartId.Index] = new MyVoxelPartInfo1
				{
					IndexCount = data.Parts[i].IndexCount,
					StartIndex = data.Parts[i].StartIndex,
					BaseVertex = 0,
					MaterialTriple = materials
				};
			}
		}

		public static void UnloadMeshData(MeshId mesh)
		{
			MyLodMesh myLodMesh = default(MyLodMesh);
			myLodMesh.Mesh = mesh;
			myLodMesh.Lod = 0;
			MyLodMesh key = myLodMesh;
			LodMeshId lodMeshId = LodMeshIndex[key];
			if (!LodMeshInfos.Data[lodMeshId.Index].NullLodMesh)
			{
				DisposeLodMeshBuffers(lodMeshId);
			}
		}

		public static void DisposeLodMeshInfos(LodMeshId lodMeshId)
		{
			ReturnBuffer(ref LodMeshInfos.Data[lodMeshId.Index].Data.Indices);
			ReturnBuffer(ref LodMeshInfos.Data[lodMeshId.Index].Data.VertexStream0);
			ReturnBuffer(ref LodMeshInfos.Data[lodMeshId.Index].Data.VertexStream1);
			LodMeshInfos.Data[lodMeshId.Index].Data = default(MyMeshRawData);
			LodMeshInfos.Free(lodMeshId.Index);
		}

		public static void ReturnBuffer(ref NativeArray array)
		{
			if (array != null)
			{
				BufferAllocator.Dispose(array);
				array = null;
			}
		}

		private static void AllocBuffer(ref NativeArray destinationData, int size)
		{
			if (destinationData == null || destinationData.Size < size)
			{
				ReturnBuffer(ref destinationData);
				destinationData = BufferAllocator.Allocate(size);
			}
		}

		private unsafe static void FillStreamData(ref NativeArray destinationData, void* sourcePointer, int elementCount, int elementStride)
		{
			int num = elementCount * elementStride;
			AllocBuffer(ref destinationData, num);
			Utilities.CopyMemory(destinationData.Ptr, new IntPtr(sourcePointer), num);
		}

		private unsafe static void FillStream0Data(ref MyMeshRawData rawData, void* sourcePointer, int vertexCount, int vertexStride)
		{
			rawData.Stride0 = vertexStride;
			FillStreamData(ref rawData.VertexStream0, sourcePointer, vertexCount, vertexStride);
		}

		private unsafe static void FillStream0Data(ref MyMeshRawData rawData, MyVertexFormatPositionH4[] vertices, int vertexCount)
		{
			fixed (MyVertexFormatPositionH4* ptr = vertices)
			{
				void* sourcePointer = ptr;
				FillStream0Data(ref rawData, sourcePointer, vertexCount, MyVertexFormatPositionH4.STRIDE);
			}
		}

		private unsafe static void FillStream0Data(ref MyMeshRawData rawData, HalfVector4[] vertices, int vertexCount)
		{
			fixed (HalfVector4* ptr = vertices)
			{
				void* sourcePointer = ptr;
				FillStream0Data(ref rawData, sourcePointer, vertexCount, sizeof(HalfVector4));
			}
		}

		private unsafe static void FillStream0Data(ref MyMeshRawData rawData, MyVertexFormatPositionSkinning[] vertices, int vertexCount)
		{
			fixed (MyVertexFormatPositionSkinning* ptr = vertices)
			{
				void* sourcePointer = ptr;
				FillStream0Data(ref rawData, sourcePointer, vertexCount, MyVertexFormatPositionSkinning.STRIDE);
			}
		}

		private unsafe static void FillStream1Data(ref MyMeshRawData rawData, void* sourcePointer, int vertexCount, int vertexStride)
		{
			rawData.Stride1 = vertexStride;
			FillStreamData(ref rawData.VertexStream1, sourcePointer, vertexCount, vertexStride);
		}

		private unsafe static void FillStream1Data(ref MyMeshRawData rawData, MyVertexFormatTexcoordNormalTangentTexindices[] vertices, int vertexCount)
		{
			fixed (MyVertexFormatTexcoordNormalTangentTexindices* ptr = vertices)
			{
				void* sourcePointer = ptr;
				FillStream1Data(ref rawData, sourcePointer, vertexCount, MyVertexFormatTexcoordNormalTangentTexindices.STRIDE);
			}
		}

		private unsafe static void FillIndexData(ref MyMeshRawData rawData, ushort[] indices, int indexCapacity)
		{
			rawData.IndicesFmt = MyIndexBufferFormat.UShort;
			fixed (ushort* ptr = indices)
			{
				void* sourcePointer = ptr;
				FillStreamData(ref rawData.Indices, sourcePointer, indexCapacity, 2);
			}
		}

		private unsafe static void FillIndexData(ref MyMeshRawData rawData, uint[] indices, int indexCapacity)
		{
			rawData.IndicesFmt = MyIndexBufferFormat.UInt;
			fixed (uint* ptr = indices)
			{
				void* sourcePointer = ptr;
				FillStreamData(ref rawData.Indices, sourcePointer, indexCapacity, 4);
			}
		}

		private static void ResizeVoxelParts(MeshId mesh, LodMeshId lod, int num)
		{
			int partsNum = LodMeshInfos.Data[lod.Index].PartsNum;
			MyMeshPart key;
			if (partsNum < num)
			{
				for (int i = partsNum; i < num; i++)
				{
					VoxelPartId voxelPartId = default(VoxelPartId);
					voxelPartId.Index = VoxelParts.Allocate();
					VoxelPartId value = voxelPartId;
					Dictionary<MyMeshPart, VoxelPartId> voxelPartIndex = VoxelPartIndex;
					key = new MyMeshPart
					{
						Mesh = mesh,
						Lod = 0,
						Part = i
					};
					voxelPartIndex[key] = value;
				}
			}
			else if (partsNum > num)
			{
				for (int j = num; j < partsNum; j++)
				{
					Dictionary<MyMeshPart, VoxelPartId> voxelPartIndex2 = VoxelPartIndex;
					key = new MyMeshPart
					{
						Mesh = mesh,
						Lod = 0,
						Part = j
					};
					VoxelPartId voxelPartId2 = voxelPartIndex2[key];
					VoxelParts.Free(voxelPartId2.Index);
					Dictionary<MyMeshPart, VoxelPartId> voxelPartIndex3 = VoxelPartIndex;
					key = new MyMeshPart
					{
						Mesh = mesh,
						Lod = 0,
						Part = j
					};
					voxelPartIndex3.Remove(key);
				}
			}
			LodMeshInfos.Data[lod.Index].PartsNum = num;
			LodMeshInfos.Data[lod.Index].PartOffsets = new MyVoxelMeshPartIndex[num];
		}

		private static void DisposeLodMeshBuffers(LodMeshId lodMeshId)
		{
			ref MyMeshBuffers reference = ref LodMeshBuffers[lodMeshId.Index];
			DisposeBuffer(reference.IB);
			DisposeBuffer(reference.VB0);
			DisposeBuffer(reference.VB1);
			reference = MyMeshBuffers.Empty;
<<<<<<< HEAD
			void DisposeBuffer(IBuffer buffer)
=======
			static void DisposeBuffer(IBuffer buffer)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (buffer != null)
				{
					MeshBufferAllocator.Dispose((MyGenericBuffer)buffer);
				}
			}
		}

		private static void LoadMesh(MeshId id, IMyVoxelUpdateBatch updateBatch = null)
		{
			string name = MeshInfos.Data[id.Index].Name;
			float rescale = MeshInfos.Data[id.Index].Rescale;
			MyLodMeshInfo myLodMeshInfo = default(MyLodMeshInfo);
			myLodMeshInfo.Name = name;
			myLodMeshInfo.FileName = name;
			MyLodMeshInfo lodMeshInfo = myLodMeshInfo;
			MeshInfos.Data[id.Index].Loaded = true;
			if (!LoadMwm(ref lodMeshInfo, out var parts, out var sections, out var lodDescriptors, rescale))
			{
				lodMeshInfo.FileName = "Models/Debug/Error.mwm";
				LoadMwm(ref lodMeshInfo, out parts, out sections, out lodDescriptors, rescale);
			}
			MeshInfos.Data[id.Index].FileExists = true;
			StoreLodMeshWithParts(id, 0, lodMeshInfo, parts);
			StoreLodMeshSections(id, 0, ref sections);
			int num = 1;
			if (lodDescriptors != null)
			{
				for (int i = 0; i < lodDescriptors.Length; i++)
				{
					string modelAbsoluteFilePath = lodDescriptors[i].GetModelAbsoluteFilePath(name);
					if (modelAbsoluteFilePath == null)
					{
						string msg = "Missing lod file: " + lodDescriptors[i].Model;
						MyRender11.Log.WriteLine(msg);
						continue;
					}
					myLodMeshInfo = default(MyLodMeshInfo);
					myLodMeshInfo.FileName = modelAbsoluteFilePath;
					myLodMeshInfo.LodDistance = lodDescriptors[i].Distance;
					myLodMeshInfo.NullLodMesh = modelAbsoluteFilePath == null;
					MyLodMeshInfo lodMeshInfo2 = myLodMeshInfo;
					if (LoadMwm(ref lodMeshInfo2, out var parts2, out var sections2, rescale))
					{
						StoreLodMeshWithParts(id, num, lodMeshInfo2, parts2);
						StoreLodMeshSections(id, num, ref sections2);
						num++;
					}
				}
			}
			MeshInfos.Data[id.Index].LodsNum = num;
			for (int j = 0; j < num; j++)
			{
				MoveData(LodMeshIndex[new MyLodMesh
				{
					Mesh = id,
					Lod = j
				}], (MyDeviceUpdateBatch)updateBatch);
			}
		}

		public static string GetContentPath(string file)
		{
			string result = null;
			if (Path.IsPathRooted(file) && file.ToLower().Contains("models"))
			{
				result = file.Substring(0, file.ToLower().IndexOf("models"));
			}
			return result;
		}

		internal static void Load()
		{
<<<<<<< HEAD
			foreach (MeshId item in State[0].ToList())
=======
			foreach (MeshId item in Enumerable.ToList<MeshId>((IEnumerable<MeshId>)State[0]))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				LoadMesh(item);
				MoveState(item, MyMeshState.WAITING, MyMeshState.LOADED);
			}
		}

		internal static void OnDeviceReset()
		{
			foreach (LodMeshId value in LodMeshIndex.Values)
			{
				MoveData(value);
			}
		}

		public static void OnDeviceEnd()
		{
			m_deviceUpdateBatchCache?.Dispose();
			m_deviceUpdateBatchCache = null;
			MyVoxelRenderDataProcessorProvider.VoxelRenderDataProcessorPool.Clean();
		}

		public static IMyUpdateBatch OpenMeshUpdateBatch()
		{
			if (m_meshUpdateBatch == null)
			{
				m_meshUpdateBatch = new MyDeviceUpdateBatch();
			}
			m_meshUpdateBatch.Open();
			return m_meshUpdateBatch;
		}

		public static IMyUpdateBatch OpenVoxelBatch()
		{
			return DeviceUpdateBatch;
		}

		public static void Preload(List<string> models)
		{
			foreach (string model in models)
			{
				GetMeshId(X.TEXT_(model), 1f, MyRender11.DeferStateChangeBatch);
			}
			MyMeshMaterials1.OnResourcesGathering(preloadTextures: true);
		}

		private static bool LoadMwm(ref MyLodMeshInfo lodMeshInfo, out MyMeshPartInfo1[] parts, out MyMeshSectionInfo1[] sections, float rescale)
		{
			MyLODDescriptor[] lodDescriptors;
			return LoadMwm(ref lodMeshInfo, out parts, out sections, out lodDescriptors, rescale);
		}

		private static bool LoadMwm(ref MyLodMeshInfo lodMeshInfo, out MyMeshPartInfo1[] parts, out MyMeshSectionInfo1[] sections, out MyLODDescriptor[] lodDescriptors, float rescale)
		{
			parts = null;
			sections = null;
			lodDescriptors = null;
			MyModelImporter myModelImporter = new MyModelImporter();
			string fileName = lodMeshInfo.FileName;
			if (fileName == null)
			{
				return true;
			}
			string text = (Path.IsPathRooted(fileName) ? fileName : Path.Combine(MyFileSystem.ContentPath, fileName));
			if (!MyFileSystem.FileExists(text))
			{
				MyRender11.Log.WriteLine($"Mesh asset {fileName} missing");
				return false;
			}
			MyMeshData myMeshData = new MyMeshData();
			myMeshData.DoImport(myModelImporter, text);
			lodDescriptors = (MyLODDescriptor[])myMeshData.Lods.Clone();
			HalfVector4[] positions = myMeshData.Positions;
			if (rescale != 1f)
			{
				for (int i = 0; i < positions.Length; i++)
				{
					positions[i] = VF_Packer.PackPosition(VF_Packer.UnpackPosition(positions[i]) * rescale);
				}
			}
			int num = (lodMeshInfo.VerticesNum = positions.Length);
			if (positions.Length == 0)
			{
				MyRender11.Log.WriteLine($"Mesh asset {fileName} has no vertices");
				myModelImporter.Clear();
				return false;
			}
			Byte4[] normals = myMeshData.Normals;
			Byte4[] tangents = myMeshData.Tangents;
			Byte4[] bitangents = myMeshData.Bitangents;
			Byte4[] array = (myMeshData.StoredTangents = new Byte4[myMeshData.VerticesNum]);
			if (tangents.Length != 0 && bitangents.Length != 0)
			{
				for (int j = 0; j < myMeshData.VerticesNum; j++)
				{
					Vector3 v = VF_Packer.UnpackNormal(normals[j].PackedValue);
					Vector3 vector = VF_Packer.UnpackNormal(tangents[j].PackedValue);
					Vector3 v2 = VF_Packer.UnpackNormal(bitangents[j].PackedValue);
					Vector4 tangentW = new Vector4(vector.X, vector.Y, vector.Z, 0f);
					tangentW.W = ((!(vector.Cross(v).Dot(v2) < 0f)) ? 1 : (-1));
					array[j] = VF_Packer.PackTangentSignB4(ref tangentW);
				}
			}
			float patternScale = myMeshData.PatternScale;
			HalfVector2[] texcoords = myMeshData.Texcoords;
			if (patternScale != 1f && texcoords.Length != 0)
			{
				for (int k = 0; k < texcoords.Length; k++)
				{
					texcoords[k] = new HalfVector2(texcoords[k].ToVector2() / patternScale);
				}
			}
			CreatePartInfos(fileName, GetContentPath(fileName), myMeshData, ref lodMeshInfo, out parts, out sections);
			FillMeshData(myMeshData, ref lodMeshInfo, out var rawData);
			lodMeshInfo.Data = rawData;
			lodMeshInfo.BoundingBox = myMeshData.BoundindBox;
			lodMeshInfo.BoundingSphere = myMeshData.BoundingSphere;
			float num2 = myMeshData.BoundindBox.Size.Length();
			lodMeshInfo.TriangleDensity = (float)lodMeshInfo.TrianglesNum / (num2 * num2);
			myModelImporter.Clear();
			return true;
		}

		private static void CreatePartInfos(string assetName, string contentPath, MyMeshData meshData, ref MyLodMeshInfo lodMeshInfo, out MyMeshPartInfo1[] parts, out MyMeshSectionInfo1[] sections)
		{
			List<MyMeshPartInfo> partInfos = meshData.PartInfos;
			MergableParts.Clear();
			NonMergableParts.Clear();
			TempParts.Clear();
			for (int i = 0; i < partInfos.Count; i++)
			{
				MyMaterialDescriptor materialDesc = partInfos[i].m_MaterialDesc;
				if (materialDesc == null)
				{
					MyRender11.Log.WriteLine($"Mesh asset {assetName} has no material in part {i}");
				}
				else if (materialDesc.Facing == MyFacingEnum.Impostor)
				{
					if (materialDesc.Textures.ContainsKey("NormalGlossTexture"))
					{
						MyRender11.Log.WriteLine(string.Format("The impostor model {0} defines the normalGloss texture. Overwriting it with custom extension: {1}. Path to model: {2}", assetName, "_normal_depth", contentPath));
					}
					else
					{
						string text = materialDesc.Textures["ColorMetalTexture"];
						string extension = Path.GetExtension(text);
						materialDesc.Textures["NormalGlossTexture"] = text.Substring(0, text.Length - extension.Length) + "_normal_depth" + extension;
					}
				}
				MyMeshMaterialId myMeshMaterialId = MyMeshMaterials1.NullMaterialId;
				bool flag = false;
				if (MyRender11.Settings.UseGeometryArrayTextures && materialDesc != null)
				{
					MyMeshMaterialInfo info = MyMeshMaterials1.ConvertImportDescToMeshMaterialInfo(materialDesc, contentPath, assetName);
					if (MyManagers.GeometryTextureSystem.IsMaterialAcceptableForTheSystem(info))
					{
						MyManagers.GeometryTextureSystem.ValidateMaterialTextures(info);
						myMeshMaterialId = MyManagers.GeometryTextureSystem.GetOrCreateMaterialId(info);
						flag = true;
					}
				}
				if (!flag)
				{
					myMeshMaterialId = MyMeshMaterials1.GetMaterialId(materialDesc, contentPath, assetName);
				}
				if (materialDesc != null)
				{
					meshData.Material(materialDesc.MaterialName, myMeshMaterialId);
				}
				Dictionary<MyMeshMaterialId, List<int>> dictionary = (flag ? MergableParts : NonMergableParts);
				if (!dictionary.ContainsKey(myMeshMaterialId))
				{
					dictionary[myMeshMaterialId] = new List<int>();
				}
				dictionary[myMeshMaterialId].Add(i);
			}
			foreach (KeyValuePair<MyMeshMaterialId, List<int>> mergablePart in MergableParts)
			{
				if (mergablePart.Value.Count == 1)
				{
					TempParts[mergablePart.Key] = mergablePart.Value;
				}
			}
			foreach (KeyValuePair<MyMeshMaterialId, List<int>> tempPart in TempParts)
			{
				MergableParts.Remove(tempPart.Key);
				NonMergableParts[tempPart.Key] = tempPart.Value;
			}
			meshData.MatsIndices = new Dictionary<string, Tuple<int, int, int>>();
			CreateMergedPartInfos(assetName, MergableParts, NonMergableParts, meshData, out parts);
			sections = null;
			if (meshData.SectionInfos != null && meshData.SectionInfos.Count > 0)
			{
				CreateSections(meshData, parts, assetName, out sections);
			}
			lodMeshInfo.PartsNum = parts.Length;
			if (meshData.SectionInfos != null && meshData.SectionInfos.Count > 0)
			{
				lodMeshInfo.SectionNames = new string[meshData.SectionInfos.Count];
				for (int j = 0; j < meshData.SectionInfos.Count; j++)
				{
					lodMeshInfo.SectionNames[j] = meshData.SectionInfos[j].Name;
				}
			}
			else
			{
				lodMeshInfo.SectionNames = new string[0];
			}
			lodMeshInfo.IndicesNum = meshData.NewIndices.Count;
			lodMeshInfo.TrianglesNum = meshData.NewIndices.Count / 3;
		}

		private static void CreateMergedPartInfos(string assetName, Dictionary<MyMeshMaterialId, List<int>> mergablePartGroups, Dictionary<MyMeshMaterialId, List<int>> nonMergablePartGroups, MyMeshData meshData, out MyMeshPartInfo1[] parts)
		{
			meshData.NewIndices = new MyList<uint>();
			List<MyMeshPartInfo1> list = new List<MyMeshPartInfo1>();
			foreach (KeyValuePair<MyMeshMaterialId, List<int>> mergablePartGroup in mergablePartGroups)
			{
				if (!AddMergableGroup(assetName, mergablePartGroup.Key, mergablePartGroup.Value, meshData, list))
				{
					nonMergablePartGroups[mergablePartGroup.Key] = mergablePartGroups[mergablePartGroup.Key];
				}
			}
			foreach (KeyValuePair<MyMeshMaterialId, List<int>> nonMergablePartGroup in nonMergablePartGroups)
			{
				if (!AddNonMergableGroup(assetName, nonMergablePartGroup.Key, nonMergablePartGroup.Value, meshData, list))
				{
					string msg = $"Could not load part '{nonMergablePartGroup.Key.Info.Name.ToString()}' of model '{assetName}'.";
					MyRender11.Log.WriteLine(msg);
				}
			}
			parts = list.ToArray();
		}

		private static bool AddMergableGroup(string assetName, MyMeshMaterialId groupMaterial, List<int> groupParts, MyMeshData meshData, List<MyMeshPartInfo1> outputParts)
		{
			List<MyMeshPartInfo> partInfos = meshData.PartInfos;
			bool doOffset = true;
			if (groupMaterial == MyMeshMaterials1.NullMaterialId)
			{
				doOffset = false;
			}
			else
			{
				MyFacingEnum facing = groupMaterial.Info.Facing;
				if (facing != MyFacingEnum.Full && facing != MyFacingEnum.Impostor)
				{
					doOffset = false;
				}
			}
			List<int> list = new List<int>();
			foreach (int groupPart in groupParts)
			{
				MyMeshPartInfo myMeshPartInfo = partInfos[groupPart];
				list.AddRange(myMeshPartInfo.m_indices);
			}
			int[] bonesMapping = null;
			bool flag = true;
			if (meshData.IsAnimated)
			{
				flag = CreatePartAnimations(list, meshData, out bonesMapping);
			}
			if (!flag)
			{
				MyRender11.Log.WriteLine($"Model asset {assetName} has more than {60} bones in parth with {groupMaterial.Info.Name} material. Skipping part merging for this material.");
				return false;
			}
			MyMeshPartInfo1 item = AppendPartIndices(meshData, list, doOffset);
			item.BonesMapping = bonesMapping;
			MyMeshMaterialInfo desc = groupMaterial.Info;
			desc.Name = X.TEXT_("Merged::" + m_mergedPartCounter);
			m_mergedPartCounter++;
			item.Material = MyMeshMaterials1.GetMaterialId(ref desc);
			item.UnusedMaterials = new HashSet<string>();
			foreach (int groupPart2 in groupParts)
			{
				MyMeshMaterialId materialId = MyMeshMaterials1.GetMaterialId(partInfos[groupPart2].m_MaterialDesc.MaterialName);
				item.UnusedMaterials.Add(materialId.Info.Name.String);
			}
			int num = meshData.NewIndices.Count - list.Count;
			foreach (int groupPart3 in groupParts)
			{
				MyMeshPartInfo myMeshPartInfo2 = partInfos[groupPart3];
				if (myMeshPartInfo2.m_MaterialDesc != null)
				{
					meshData.MatsIndices[myMeshPartInfo2.m_MaterialDesc.MaterialName] = new Tuple<int, int, int>(outputParts.Count, num, item.BaseVertex);
				}
				num += myMeshPartInfo2.m_indices.Count;
			}
			outputParts.Add(item);
			return true;
		}

		private static bool AddNonMergableGroup(string assetName, MyMeshMaterialId groupMaterial, List<int> groupParts, MyMeshData meshData, List<MyMeshPartInfo1> outputParts)
		{
			List<MyMeshPartInfo> partInfos = meshData.PartInfos;
			bool doOffset = true;
			if (groupMaterial == MyMeshMaterials1.NullMaterialId)
			{
				doOffset = false;
			}
			else
			{
				MyFacingEnum facing = groupMaterial.Info.Facing;
				if (facing != MyFacingEnum.Full && facing != MyFacingEnum.Impostor)
				{
					doOffset = false;
				}
			}
			foreach (int groupPart in groupParts)
			{
				MyMeshPartInfo myMeshPartInfo = partInfos[groupPart];
				List<int> indices = myMeshPartInfo.m_indices;
				int[] bonesMapping = null;
				bool flag = true;
				if (meshData.IsAnimated)
				{
					flag = CreatePartAnimations(indices, meshData, out bonesMapping);
				}
				if (!flag)
				{
					string msg = $"Model asset {assetName} has more than {60} bones in parth with {groupMaterial.Info.Name} material. Split model into more meshes or assign multiple materials to meshes.Disabling animations for this model.";
					MyRender11.Log.WriteLine(msg);
					meshData.BoneIndices = new Vector4I[0];
					meshData.IsAnimated = false;
				}
				MyMeshPartInfo1 item = AppendPartIndices(meshData, indices, doOffset);
				item.BonesMapping = bonesMapping;
				item.Material = groupMaterial;
				if (myMeshPartInfo.m_MaterialDesc != null)
				{
					meshData.MatsIndices[myMeshPartInfo.m_MaterialDesc.MaterialName] = new Tuple<int, int, int>(outputParts.Count, meshData.NewIndices.Count - myMeshPartInfo.m_indices.Count, item.BaseVertex);
				}
				outputParts.Add(item);
			}
			return true;
		}

		private static bool CreatePartAnimations(List<int> partIndices, MyMeshData meshData, out int[] bonesMapping)
		{
			bonesMapping = null;
			MyModelBone[] bones = meshData.Bones;
			Vector4I[] boneIndices = meshData.BoneIndices;
			Vector4[] boneWeights = meshData.BoneWeights;
			if (boneIndices.Length == 0 || bones.Length <= 60)
			{
				return true;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			int num = partIndices.Count / 3;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					int num2 = partIndices[i * 3 + j];
					Vector4 vector = boneWeights[num2];
					Vector4I vector4I = boneIndices[num2];
					if (vector.X > 0f)
					{
						dictionary[vector4I.X] = 1;
					}
					if (vector.Y > 0f)
					{
						dictionary[vector4I.Y] = 1;
					}
					if (vector.Z > 0f)
					{
						dictionary[vector4I.Z] = 1;
					}
					if (vector.W > 0f)
					{
						dictionary[vector4I.W] = 1;
					}
				}
			}
			if (dictionary.Count > 60)
			{
				return false;
			}
			List<int> list = new List<int>(dictionary.Keys);
			list.Sort();
			if (list.Count == 0 || list[list.Count - 1] < 60)
			{
				return true;
			}
			for (int k = 0; k < list.Count; k++)
			{
				dictionary[list[k]] = k;
			}
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			for (int l = 0; l < num; l++)
			{
				for (int m = 0; m < 3; m++)
				{
					int num3 = partIndices[l * 3 + m];
					if (!dictionary2.ContainsKey(num3))
					{
						Vector4 vector2 = boneWeights[num3];
						Vector4I vector4I2 = boneIndices[num3];
						if (vector2.X > 0f)
						{
							vector4I2.X = dictionary[vector4I2.X];
						}
						if (vector2.Y > 0f)
						{
							vector4I2.Y = dictionary[vector4I2.Y];
						}
						if (vector2.Z > 0f)
						{
							vector4I2.Z = dictionary[vector4I2.Z];
						}
						if (vector2.W > 0f)
						{
							vector4I2.W = dictionary[vector4I2.W];
						}
						boneIndices[num3] = vector4I2;
						dictionary2[num3] = 1;
					}
				}
			}
			bonesMapping = list.ToArray();
			return true;
		}

		private static MyMeshPartInfo1 AppendPartIndices(MyMeshData meshData, List<int> partIndices, bool doOffset)
		{
			MyList<uint> newIndices = meshData.NewIndices;
			int count = newIndices.Count;
			int count2 = partIndices.Count;
			uint num = (uint)partIndices[0];
			uint num2 = 0u;
			foreach (int partIndex in partIndices)
			{
				newIndices.Add((uint)partIndex);
				num = Math.Min(num, (uint)partIndex);
			}
			for (int i = count; i < count + count2; i++)
			{
				newIndices[i] -= num;
				num2 = Math.Max(num2, newIndices[i]);
			}
			Vector3 zero = Vector3.Zero;
			if (doOffset)
			{
				Vector3[] array = new Vector3[partIndices.Count];
				for (int j = 0; j < partIndices.Count; j++)
				{
					HalfVector4 position = meshData.Positions[partIndices[j]];
					Vector3 vector = PositionPacker.UnpackPosition(ref position);
					zero += vector;
					array[j] = vector;
				}
				zero /= (float)partIndices.Count;
				for (int k = 0; k < partIndices.Count; k++)
				{
					Vector3 position2 = array[k];
					position2 -= zero;
					meshData.Positions[partIndices[k]] = PositionPacker.PackPosition(ref position2);
				}
			}
			MyMeshPartInfo1 myMeshPartInfo = default(MyMeshPartInfo1);
			myMeshPartInfo.IndexCount = count2;
			myMeshPartInfo.StartIndex = count;
			myMeshPartInfo.BaseVertex = (int)num;
			myMeshPartInfo.CenterOffset = zero;
			MyMeshPartInfo1 result = myMeshPartInfo;
			if (num2 > meshData.MaxIndex)
			{
				meshData.MaxIndex = num2;
			}
			return result;
		}

		private static void CreateSections(MyMeshData meshData, MyMeshPartInfo1[] parts, string assetName, out MyMeshSectionInfo1[] sections)
		{
			List<MyMeshSectionInfo> sectionInfos = meshData.SectionInfos;
			sections = new MyMeshSectionInfo1[sectionInfos.Count];
			int[] array = new int[parts.Length];
			for (int i = 0; i < sectionInfos.Count; i++)
			{
				MyMeshSectionInfo myMeshSectionInfo = sectionInfos[i];
				MyMeshSectionPartInfo1[] array2 = new MyMeshSectionPartInfo1[myMeshSectionInfo.Meshes.Count];
				int num = 0;
				foreach (MyMeshSectionMeshInfo mesh in myMeshSectionInfo.Meshes)
				{
					if (!meshData.MatsIndices.TryGetValue(mesh.MaterialName, out var value))
					{
						MyRender11.Log.WriteLine($"Section references material that is not present and sections wont be loaded. Section: {myMeshSectionInfo.Name}, Material_Name:{mesh.MaterialName}");
						sections = null;
						meshData.SectionInfos = null;
						return;
					}
					MyMeshMaterialId material = meshData.Material(mesh.MaterialName);
					array2[num] = new MyMeshSectionPartInfo1
					{
						IndexCount = mesh.IndexCount,
						StartIndex = mesh.StartIndex + value.Item2,
						BaseVertex = value.Item3,
						PartIndex = value.Item1,
						PartSubmeshIndex = array[value.Item1],
						Material = material
					};
					array[value.Item1]++;
					num++;
				}
				sections[i] = new MyMeshSectionInfo1
				{
					Name = myMeshSectionInfo.Name,
					Meshes = array2
				};
			}
			for (int j = 0; j < parts.Length; j++)
			{
				parts[j].SectionSubmeshCount = array[j];
			}
		}

		private unsafe static void FillMeshData(MyMeshData meshData, ref MyLodMeshInfo lodMeshInfo, out MyMeshRawData rawData)
		{
			rawData = default(MyMeshRawData);
			int num = meshData.Positions.Length;
			if (meshData.MaxIndex <= 65535)
			{
				MyArrayHelpers.InitOrReserveNoCopy(ref m_tmpShortIndices, lodMeshInfo.IndicesNum);
				fixed (uint* source = meshData.NewIndices.GetInternalArray())
				{
					fixed (ushort* ptr = m_tmpShortIndices)
					{
						void* destination = ptr;
						CopyIndices(source, destination, 2, lodMeshInfo.IndicesNum);
					}
				}
				FillIndexData(ref rawData, m_tmpShortIndices, lodMeshInfo.IndicesNum);
			}
			else
			{
				FillIndexData(ref rawData, meshData.NewIndices.GetInternalArray(), lodMeshInfo.IndicesNum);
			}
			int num2 = 0;
			num2 = (meshData.IsAnimated ? (num2 + 3) : (num2 + 1));
			if (meshData.ValidStreams)
			{
				num2 += 3;
				if (MyRender11.Settings.UseGeometryArrayTextures)
				{
					num2++;
				}
			}
			List<MyVertexInputComponent> list = new List<MyVertexInputComponent>(num2);
			if (!meshData.IsAnimated)
			{
				FillStream0Data(ref rawData, meshData.Positions, num);
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.POSITION_PACKED));
			}
			else
			{
				MyVertexFormatPositionSkinning[] array = new MyVertexFormatPositionSkinning[num];
				fixed (MyVertexFormatPositionSkinning* ptr2 = array)
				{
					for (int i = 0; i < num; i++)
					{
						ptr2[i].Position = meshData.Positions[i];
						ptr2[i].BoneIndices = new Byte4(meshData.BoneIndices[i].X, meshData.BoneIndices[i].Y, meshData.BoneIndices[i].Z, meshData.BoneIndices[i].W);
						ptr2[i].BoneWeights = new HalfVector4(meshData.BoneWeights[i]);
					}
				}
				FillStream0Data(ref rawData, array, num);
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.POSITION_PACKED));
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.BLEND_WEIGHTS));
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.BLEND_INDICES));
			}
			if (meshData.ValidStreams)
			{
				MyVertexFormatTexcoordNormalTangentTexindices[] array2 = new MyVertexFormatTexcoordNormalTangentTexindices[num];
				fixed (MyVertexFormatTexcoordNormalTangentTexindices* ptr3 = array2)
				{
					for (int j = 0; j < num; j++)
					{
						ptr3[j].Normal = meshData.Normals[j];
						ptr3[j].Tangent = meshData.StoredTangents[j];
						ptr3[j].Texcoord = meshData.Texcoords[j];
						ptr3[j].TexIndices = (Byte4)meshData.TexIndices[j];
					}
				}
				FillStream1Data(ref rawData, array2, array2.Length);
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.NORMAL, 1));
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.TANGENT_SIGN_OF_BITANGENT, 1));
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.TEXCOORD0_H, 1));
				if (MyRender11.Settings.UseGeometryArrayTextures)
				{
					list.Add(new MyVertexInputComponent(MyVertexInputComponentType.TEXINDICES, 1));
				}
			}
			list.Capacity = list.Count;
			rawData.VertexLayout = MyVertexLayouts.GetLayout(list.ToArray());
		}
	}
}
