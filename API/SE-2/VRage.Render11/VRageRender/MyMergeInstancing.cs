using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SharpDX.Direct3D11;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal class MyMergeInstancing
	{
		internal MyMeshTableSrv m_meshTable;

		internal ISrvBuffer m_indirectionBuffer;

		internal ISrvBuffer m_instanceBuffer;

		internal ISrvBindable[] m_srvs = new ISrvBindable[2];

		private Dictionary<uint, MyInstanceInfo> m_entities = new Dictionary<uint, MyInstanceInfo>();

		private bool m_instancesDataDirty;

		private bool m_tableDirty;

		private MyPackedPool<MyInstancingTableEntry> m_instancingTable = new MyPackedPool<MyInstancingTableEntry>(64);

		private MyFreelist<MyPerInstanceData> m_perInstance = new MyFreelist<MyPerInstanceData>(16);

		private MyFreelist<MyInstanceEntityInfo> m_entityInfos = new MyFreelist<MyInstanceEntityInfo>(16);

		internal int VerticesNum => m_meshTable.PageSize * m_instancingTable.Size;

		public bool TableDirty => m_tableDirty;

		public int TablePageSize => m_meshTable.PageSize;

		internal MyMergeInstancing(MyMeshTableSrv meshTable)
		{
			m_meshTable = meshTable;
			m_instancesDataDirty = false;
			m_tableDirty = false;
		}

		internal bool IsMergable(MeshId model)
		{
			if (model.Info.LodsNum == 1 && MyMeshes.GetLodMesh(model, 0).Info.PartsNum == 1)
			{
				return m_meshTable.IsMergable(model);
			}
			return false;
		}

		internal void AddEntity(MyActor actor, MeshId model)
		{
			uint iD = actor.ID;
			int num = m_perInstance.Allocate();
			m_entityInfos.Allocate();
			m_entities[iD] = new MyInstanceInfo
			{
				InstanceIndex = num,
				PageHandles = new List<MyPackedPoolHandle>()
			};
			int num2 = -1;
			MyMeshTableEntry key = MyMeshTableSrv.MakeKey(model);
			foreach (int item in m_meshTable.Pages(key))
			{
				if (num2 == -1)
				{
					num2 = item;
				}
				MyPackedPoolHandle myPackedPoolHandle = m_instancingTable.Allocate();
				m_instancingTable.Data[m_instancingTable.AsIndex(myPackedPoolHandle)] = new MyInstancingTableEntry
				{
					InstanceId = num,
					InnerMeshId = item
				};
				m_entities[iD].PageHandles.Add(myPackedPoolHandle);
			}
			m_perInstance.Data[num] = MyPerInstanceData.FromWorldMatrix(ref MatrixD.Zero, 0);
			m_entityInfos.Data[num] = new MyInstanceEntityInfo
			{
				EntityId = iD,
				PageOffset = num2
			};
			m_tableDirty = true;
		}

		internal void UpdateEntity(MyActor actor, int depthBias)
		{
			uint iD = actor.ID;
			MatrixD matrix = actor.WorldMatrix;
			m_perInstance.Data[m_entities[iD].InstanceIndex] = MyPerInstanceData.FromWorldMatrix(ref matrix, depthBias);
			m_instancesDataDirty = true;
		}

		internal void RemoveEntity(MyActor actor)
		{
			uint iD = actor.ID;
			MyInstanceInfo myInstanceInfo = m_entities[iD];
			m_perInstance.Free(myInstanceInfo.InstanceIndex);
			m_entityInfos.Free(myInstanceInfo.InstanceIndex);
			foreach (MyPackedPoolHandle pageHandle in m_entities[iD].PageHandles)
			{
				m_instancingTable.Free(pageHandle);
			}
			m_tableDirty = true;
		}

		public MyInstanceEntityInfo[] GetEntityInfos(out int filledSize)
		{
			filledSize = m_entityInfos.FilledSize;
			return m_entityInfos.Data;
		}

		internal void OnDeviceReset()
		{
			if (m_indirectionBuffer != null)
			{
				MyManagers.Buffers.Dispose(m_indirectionBuffer);
			}
			m_indirectionBuffer = null;
			if (m_instanceBuffer != null)
			{
				MyManagers.Buffers.Dispose(m_instanceBuffer);
			}
			m_instanceBuffer = null;
			Array.Clear(m_srvs, 0, m_srvs.Length);
			m_tableDirty = true;
			m_instancesDataDirty = true;
		}

		internal unsafe void MoveToGPU()
		{
			if (m_tableDirty)
			{
				MyInstancingTableEntry[] data = m_instancingTable.Data;
				fixed (MyInstancingTableEntry* ptr = data)
				{
					void* value = ptr;
					CreateResizeOrFill("MyMergeInstancing/Tables", ref m_indirectionBuffer, data.Length, data, new IntPtr(value));
					m_srvs[0] = m_indirectionBuffer;
				}
				m_tableDirty = false;
			}
			if (m_instancesDataDirty)
			{
				MyPerInstanceData[] data2 = m_perInstance.Data;
				fixed (MyPerInstanceData* ptr2 = data2)
				{
					void* value2 = ptr2;
					CreateResizeOrFill("MyMergeInstancing instances", ref m_instanceBuffer, data2.Length, data2, new IntPtr(value2));
					m_srvs[1] = m_instanceBuffer;
				}
				m_instancesDataDirty = false;
			}
		}

		private void CreateResizeOrFill<TDataElement>(string name, ref ISrvBuffer buffer, int size, TDataElement[] data, IntPtr rawData) where TDataElement : struct
		{
			if (buffer != null && buffer.ElementCount < size)
			{
				MyManagers.Buffers.Resize(buffer, size, -1, rawData);
			}
			if (buffer == null)
			{
				buffer = MyManagers.Buffers.CreateSrv(name, size, Marshal.SizeOf<TDataElement>(), rawData, ResourceUsage.Dynamic);
				return;
			}
			MyMapping myMapping = MyMapping.MapDiscard(MyImmediateRC.RC, buffer);
			myMapping.WriteAndPosition(data, data.Length * Marshal.SizeOf<TDataElement>());
			myMapping.Unmap();
		}
	}
}
