using System;
using System.Collections.Generic;
using SharpDX;
using VRage.Library.Extensions;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageRender.Vertex;

namespace VRageRender
{
	internal class MyMeshTableSrv
	{
		internal static VertexLayoutId OneAndOnlySupportedVertexLayout;

		private int m_vertices;

		private int m_indices;

		private byte[] m_vertexStream0 = new byte[2048];

		private byte[] m_vertexStream1 = new byte[2048];

		private byte[] m_indexStream = new byte[2048];

		private unsafe static readonly int Stride0 = sizeof(MyVertexFormatPositionH4);

		private unsafe static readonly int Stride1 = sizeof(MyVertexFormatTexcoordNormalTangent);

		private static readonly int IndexStride = 4;

		private int m_indexPageSize;

		private int m_pagesUsed;

		private Dictionary<MyMeshTableEntry, MyMeshTableSrv_Entry> m_table = new Dictionary<MyMeshTableEntry, MyMeshTableSrv_Entry>();

		internal ISrvBuffer m_VB_positions;

		internal ISrvBuffer m_VB_rest;

		internal ISrvBuffer m_IB;

		private bool m_dirty;

		internal int PageSize => m_indexPageSize;

		internal static void Init()
		{
			OneAndOnlySupportedVertexLayout = MyVertexLayouts.GetLayout(new MyVertexInputComponent(MyVertexInputComponentType.POSITION_PACKED), new MyVertexInputComponent(MyVertexInputComponentType.NORMAL, 1), new MyVertexInputComponent(MyVertexInputComponentType.TANGENT_SIGN_OF_BITANGENT, 1), new MyVertexInputComponent(MyVertexInputComponentType.TEXCOORD0_H, 1));
		}

		internal bool IsMergable(MeshId model)
		{
			LodMeshId lodMesh = MyMeshes.GetLodMesh(model, 0);
			if (!MyMeshes.TryGetMeshPart(model, 0, 1, out var _) && lodMesh.Info.Data.VertexLayout == OneAndOnlySupportedVertexLayout && lodMesh.Info.IndicesNum > 0 && !model.Info.RuntimeGenerated)
			{
				return !model.Info.Dynamic;
			}
			return false;
		}

		internal MyMeshTableSrv(int pageSize = 36)
		{
			m_indexPageSize = pageSize;
			m_dirty = false;
		}

		internal int CalculateIndicesCapacity(int currentOffset, int indicesNum)
		{
			return currentOffset + (indicesNum + m_indexPageSize - 1) / m_indexPageSize * m_indexPageSize;
		}

		internal bool ContainsKey(MyMeshTableEntry key)
		{
			return m_table.ContainsKey(key);
		}

		internal static MyMeshTableEntry MakeKey(MeshId model)
		{
			MyMeshTableEntry result = default(MyMeshTableEntry);
			result.Model = model;
			result.Lod = 0;
			result.Part = 0;
			return result;
		}

		internal void OnSessionEnd()
		{
			Release();
			m_vertices = 0;
			m_indices = 0;
			m_vertexStream0 = new byte[2048];
			m_vertexStream1 = new byte[2048];
			m_indexStream = new byte[2048];
			m_pagesUsed = 0;
			m_table.Clear();
		}

		internal List<int> Pages(MyMeshTableEntry key)
		{
			return m_table[key].Pages;
		}

		internal unsafe void AddMesh(MeshId model)
		{
			MyMeshTableEntry myMeshTableEntry = default(MyMeshTableEntry);
			myMeshTableEntry.Model = model;
			myMeshTableEntry.Lod = 0;
			myMeshTableEntry.Part = 0;
			MyMeshTableEntry key = myMeshTableEntry;
			if (ContainsKey(key))
			{
				return;
			}
			int vertices = m_vertices;
			int indices = m_indices;
			MyLodMeshInfo info = MyMeshes.GetLodMesh(model, 0).Info;
			MyMeshRawData data = info.Data;
			int num = vertices + info.VerticesNum;
			int num2 = CalculateIndicesCapacity(indices, info.IndicesNum);
			m_vertices = num;
			m_indices = num2;
			MyArrayHelpers.Reserve(ref m_vertexStream0, num * Stride0, 1048576);
			MyArrayHelpers.Reserve(ref m_vertexStream1, num * Stride1, 1048576);
			MyArrayHelpers.Reserve(ref m_indexStream, num2 * IndexStride, 1048576);
			List<int> list = new List<int>();
			fixed (byte* ptr = m_vertexStream0)
			{
				Utilities.CopyMemory(new IntPtr(ptr + data.Stride0 * vertices), data.VertexStream0.Ptr, data.Stride0 * info.VerticesNum);
			}
			fixed (byte* ptr2 = m_vertexStream1)
			{
				Utilities.CopyMemory(new IntPtr(ptr2 + data.Stride1 * vertices), data.VertexStream1.Ptr, data.Stride1 * info.VerticesNum);
			}
			fixed (byte* ptr3 = m_indexStream)
			{
				void* ptr4 = ptr3;
				uint* ptr5 = (uint*)ptr4;
				ptr5 += indices;
				ushort* ptr6 = (ushort*)(void*)data.Indices.Ptr;
				for (int i = 0; i < info.IndicesNum; i += m_indexPageSize)
				{
					int num3 = Math.Min(i + m_indexPageSize, info.IndicesNum);
					for (int j = i; j < num3; j++)
					{
						ptr5[j] = (uint)(ptr6[j] + vertices);
					}
					list.Add(m_pagesUsed++);
				}
				if (info.IndicesNum % m_indexPageSize != 0)
				{
					int num4 = info.IndicesNum / m_indexPageSize;
					int num5 = info.IndicesNum % m_indexPageSize;
					uint num6 = ptr5[num4 * m_indexPageSize + num5 - 1];
					for (int k = num5; k < m_indexPageSize; k++)
					{
						ptr5[num4 * m_indexPageSize + k] = num6;
					}
				}
			}
			m_table.Add(key, new MyMeshTableSrv_Entry
			{
				Pages = list
			});
			m_dirty = true;
		}

		internal unsafe void MoveToGPU()
		{
			if (m_dirty)
			{
				Release();
				fixed (byte* ptr = m_vertexStream0)
				{
					void* value = ptr;
					m_VB_positions = MyManagers.Buffers.CreateSrv("MyMergeInstancing positions", m_vertices, Stride0, new IntPtr(value));
				}
				fixed (byte* ptr = m_vertexStream1)
				{
					void* value2 = ptr;
					m_VB_rest = MyManagers.Buffers.CreateSrv("MyMergeInstancing rest", m_vertices, Stride1, new IntPtr(value2));
				}
				fixed (byte* ptr = m_indexStream)
				{
					void* value3 = ptr;
					m_IB = MyManagers.Buffers.CreateSrv("MyMergeInstancing", m_indices, IndexStride, new IntPtr(value3));
				}
				m_dirty = false;
			}
		}

		internal void OnDeviceReset()
		{
			if (m_vertices > 0)
			{
				m_dirty = true;
			}
		}

		internal void Release()
		{
			if (m_VB_positions != null)
			{
				MyManagers.Buffers.Dispose(m_VB_positions);
				m_VB_positions = null;
				MyManagers.Buffers.Dispose(m_VB_rest);
				m_VB_rest = null;
				MyManagers.Buffers.Dispose(m_IB);
				m_IB = null;
			}
		}
	}
}
