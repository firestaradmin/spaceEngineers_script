using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct3D11;
using VRage;
using VRage.Library.Extensions;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender.Messages;
using VRageRender.Vertex;

namespace VRageRender
{
	internal static class MyInstancing
	{
		private static readonly List<MyDecalPositionUpdate> m_tmpDecalsUpdate = new List<MyDecalPositionUpdate>();

		private static readonly Dictionary<uint, InstancingId> m_idIndex = new Dictionary<uint, InstancingId>();

		private static MyActor m_instanceActor;

		internal static readonly MyFreelist<MyInstancingInfo> Instancings = new MyFreelist<MyInstancingInfo>(128);

		internal static MyInstancingData[] Data = new MyInstancingData[128];

		public static int Count => Instancings.Size;

		internal static InstancingId Get(uint GID)
		{
			return m_idIndex.Get(GID, InstancingId.NULL);
		}

		internal unsafe static void Create(uint GID, uint parentGID, MyRenderInstanceBufferType type, string debugName)
		{
			InstancingId instancingId = default(InstancingId);
			instancingId.Index = Instancings.Allocate();
			InstancingId value = instancingId;
			Instancings.Data[value.Index] = new MyInstancingInfo
			{
				ID = GID,
				ParentID = parentGID,
				Type = type,
				DebugName = debugName,
				Refs = new List<uint> { uint.MaxValue }
			};
			MyArrayHelpers.Reserve(ref Data, value.Index + 1);
			Data[value.Index] = default(MyInstancingData);
			if (type == MyRenderInstanceBufferType.Cube)
			{
				Instancings.Data[value.Index].Layout = MyVertexLayouts.GetLayout(new MyVertexInputComponent(MyVertexInputComponentType.CUBE_INSTANCE, 2, MyVertexInputComponentFreq.PER_INSTANCE));
				Instancings.Data[value.Index].Stride = sizeof(MyVertexFormatCubeInstance);
			}
			else
			{
				Instancings.Data[value.Index].Layout = MyVertexLayouts.GetLayout(new MyVertexInputComponent(MyVertexInputComponentType.GENERIC_INSTANCE, 2, MyVertexInputComponentFreq.PER_INSTANCE));
				Instancings.Data[value.Index].Stride = sizeof(MyVertexFormatGenericInstance);
			}
			m_idIndex.Add(GID, value);
		}

		private static void RemoveResource(InstancingId id)
		{
			if (Data[id.Index].VB != null)
			{
				MyManagers.Buffers.Dispose(Data[id.Index].VB);
			}
			Data[id.Index].VB = null;
		}

		internal static void AddRef(uint GID, uint refId)
		{
			InstancingId instancingId = m_idIndex.Get(GID, InstancingId.NULL);
			if (instancingId != InstancingId.NULL)
			{
				instancingId.Info.Refs.Add(refId);
			}
		}

		internal static void Remove(uint GID, uint refId, bool checkConsistency = true)
		{
			InstancingId instancingId = m_idIndex.Get(GID, InstancingId.NULL);
			if (instancingId != InstancingId.NULL)
			{
				Remove(GID, instancingId, refId);
			}
		}

		internal static void Remove(uint GID, InstancingId id, uint refId)
		{
			id.Info.Refs.Remove(refId);
			if (id.Info.Refs.Count == 0)
			{
				RemoveInternal(GID, id);
			}
		}

		private static void RemoveInternal(uint GID, InstancingId id)
		{
			RemoveResource(id);
			m_idIndex.Remove(GID);
			Instancings.Free(id.Index);
		}

		private static void DisposeInstanceActor()
		{
			if (m_instanceActor != null)
			{
				if (!m_instanceActor.IsDestroyed)
				{
					m_instanceActor.Destroy();
				}
				m_instanceActor = null;
			}
		}

		internal static void OnSessionEnd()
		{
<<<<<<< HEAD
			KeyValuePair<uint, InstancingId>[] array = m_idIndex.ToArray();
=======
			KeyValuePair<uint, InstancingId>[] array = Enumerable.ToArray<KeyValuePair<uint, InstancingId>>((IEnumerable<KeyValuePair<uint, InstancingId>>)m_idIndex);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<uint, InstancingId> keyValuePair = array[i];
				keyValuePair.Value.Info.Refs.Clear();
				RemoveInternal(keyValuePair.Key, keyValuePair.Value);
			}
			DisposeInstanceActor();
		}

		internal static void UpdateGeneric(InstancingId id, MyInstanceData[] instanceData, int capacity)
		{
			capacity = instanceData.Length;
			if (capacity != Instancings.Data[id.Index].TotalCapacity)
			{
				Vector3[] array = new Vector3[capacity];
				bool[] array2 = new bool[capacity];
				for (int i = 0; i < capacity; i++)
				{
					array[i] = new Vector3(instanceData[i].m_row0.ToVector4().W, instanceData[i].m_row1.ToVector4().W, instanceData[i].m_row2.ToVector4().W);
					array2[i] = true;
				}
				Instancings.Data[id.Index].VisibleCapacity = capacity;
				Instancings.Data[id.Index].Positions = array;
				Instancings.Data[id.Index].VisibilityMask = array2;
				Instancings.Data[id.Index].NonVisibleInstanceCount = 0;
			}
			Instancings.Data[id.Index].TotalCapacity = capacity;
			Instancings.Data[id.Index].InstanceData = instanceData;
			RebuildGeneric(id);
		}

		internal unsafe static void RebuildGeneric(InstancingId instancingId)
		{
			if (Instancings.Data[instancingId.Index].InstanceData == null)
			{
				return;
			}
			MyInstancingInfo info = instancingId.Info;
			int num = Instancings.Data[instancingId.Index].InstanceData.Length;
			for (int i = 0; i < Instancings.Data[instancingId.Index].TotalCapacity; i++)
			{
				if (!Instancings.Data[instancingId.Index].VisibilityMask[i])
				{
					num--;
				}
			}
			int size = info.Stride * num;
			MyArrayHelpers.InitOrReserve(ref Instancings.Data[instancingId.Index].Data, size);
			fixed (MyInstanceData* ptr = Instancings.Data[instancingId.Index].InstanceData)
			{
				void* value = ptr;
				fixed (byte* ptr2 = Instancings.Data[instancingId.Index].Data)
				{
					void* value2 = ptr2;
					int num2 = 0;
					for (int j = 0; j < Instancings.Data[instancingId.Index].TotalCapacity; j++)
					{
						if (Instancings.Data[instancingId.Index].VisibilityMask[j])
						{
							Utilities.CopyMemory(new IntPtr(value2) + num2 * info.Stride, new IntPtr(value) + j * info.Stride, info.Stride);
							num2++;
						}
					}
				}
			}
			Instancings.Data[instancingId.Index].VisibleCapacity = num;
			UpdateVertexBuffer(instancingId);
		}

		internal unsafe static void UpdateCube(InstancingId id, List<MyCubeInstanceData> instanceData, List<MyCubeInstanceDecalData> decals, int capacity)
		{
			UpdateDecalPositions(id, instanceData, decals);
			int size = id.Info.Stride * capacity;
			MyArrayHelpers.InitOrReserve(ref Instancings.Data[id.Index].Data, size);
			bool flag = false;
			fixed (byte* ptr = Instancings.Data[id.Index].Data)
			{
				void* ptr2 = ptr;
				MyVertexFormatCubeInstance* ptr3 = (MyVertexFormatCubeInstance*)ptr2;
				for (int i = 0; i < instanceData.Count; i++)
				{
					flag |= instanceData[i].EnableSkinning;
					instanceData[i].RetrieveBones(ptr3[i].bones);
					ptr3[i].translationRotation = instanceData[i].m_translationAndRot;
					Vector4 vector = (ptr3[i].colorMaskHSV = instanceData[i].ColorMaskHSV);
				}
			}
			Instancings.Data[id.Index].EnabledSkinning = flag;
			Instancings.Data[id.Index].VisibleCapacity = capacity;
			UpdateVertexBuffer(id);
		}

		private static void UpdateDecalPositions(InstancingId id, List<MyCubeInstanceData> instanceData, List<MyCubeInstanceDecalData> decals)
		{
			m_tmpDecalsUpdate.Clear();
			for (int i = 0; i < decals.Count; i++)
			{
				MyCubeInstanceDecalData myCubeInstanceDecalData = decals[i];
				MyCubeInstanceData myCubeInstanceData = instanceData[myCubeInstanceDecalData.InstanceIndex];
				if (!myCubeInstanceData.EnableSkinning)
				{
					break;
				}
				if (MyScreenDecals.GetDecalTopoData(myCubeInstanceDecalData.DecalId, out var data))
				{
					Matrix localMatrix;
					Matrix matrix = myCubeInstanceData.ConstructDeformedCubeInstanceMatrix(ref data.BoneIndices, ref data.BoneWeights, out localMatrix);
					Matrix.Invert(ref localMatrix, out var result);
					Matrix transform = data.MatrixBinding * result * matrix;
					m_tmpDecalsUpdate.Add(new MyDecalPositionUpdate
					{
						ID = myCubeInstanceDecalData.DecalId,
						Transform = transform
					});
				}
			}
			MyScreenDecals.UpdateDecals(m_tmpDecalsUpdate);
		}

		internal unsafe static void UpdateVertexBuffer(InstancingId id)
		{
			MyInstancingInfo info = id.Info;
			if (info.VisibleCapacity == 0)
			{
				return;
			}
			fixed (byte* value = info.Data)
			{
				IVertexBuffer vB = Data[id.Index].VB;
				if (vB == null)
				{
					Data[id.Index].VB = MyManagers.Buffers.CreateVertexBuffer(info.DebugName, info.VisibleCapacity, info.Stride, new IntPtr(value), ResourceUsage.Dynamic);
					return;
				}
				if (vB.ElementCount < info.VisibleCapacity || vB.Description.StructureByteStride != info.Stride)
				{
					MyManagers.Buffers.Resize(Data[id.Index].VB, info.VisibleCapacity, info.Stride, new IntPtr(value));
					return;
				}
				MyMapping myMapping = MyMapping.MapDiscard(MyImmediateRC.RC, vB);
				myMapping.WriteAndPosition(info.Data, info.VisibleCapacity * info.Stride);
				myMapping.Unmap();
			}
		}

		internal static void OnDeviceReset()
		{
			foreach (InstancingId value in m_idIndex.Values)
			{
				RemoveResource(value);
				UpdateVertexBuffer(value);
			}
			DisposeInstanceActor();
		}
	}
}
