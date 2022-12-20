using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRageMath.PackedVector;
using VRageRender;
using VRageRender.Vertex;

namespace VRage.Render11.GeometryStage2.Model
{
	internal static class MyLodUtils
	{
		public unsafe static IVertexBuffer CreateSimpleVB0(MyMwmData mwmData)
		{
			string name = "VB0-" + mwmData.MwmFilepath;
			if (!mwmData.IsSkinnedProperly)
			{
				MyVertexFormatPositionHTextureH[] array = new MyVertexFormatPositionHTextureH[mwmData.VerticesCount];
				for (int i = 0; i < mwmData.VerticesCount; i++)
				{
					array[i].Position = mwmData.Positions[i];
					array[i].Texcoord = mwmData.Texcoords[i];
				}
				fixed (MyVertexFormatPositionHTextureH* ptr = array)
				{
					void* value = ptr;
					return MyManagers.Buffers.CreateVertexBuffer(name, array.Length, MyVertexFormatPositionHTextureH.STRIDE, new IntPtr(value), ResourceUsage.Immutable, isStreamOutput: false, isGlobal: true);
				}
			}
			MyVertexFormatPositionSkinningTexcoord[] array2 = new MyVertexFormatPositionSkinningTexcoord[mwmData.VerticesCount];
			for (int j = 0; j < mwmData.VerticesCount; j++)
			{
				array2[j].Position = mwmData.Positions[j];
				array2[j].BoneIndices = new Byte4(mwmData.BoneIndices[j].X, mwmData.BoneIndices[j].Y, mwmData.BoneIndices[j].Z, mwmData.BoneIndices[j].W);
				array2[j].BoneWeights = new HalfVector4(mwmData.BoneWeights[j]);
				array2[j].Texcoord = mwmData.Texcoords[j];
			}
			fixed (MyVertexFormatPositionSkinningTexcoord* ptr2 = array2)
			{
				void* value2 = ptr2;
				return MyManagers.Buffers.CreateVertexBuffer(name, array2.Length, MyVertexFormatPositionSkinningTexcoord.STRIDE, new IntPtr(value2), ResourceUsage.Immutable, isStreamOutput: false, isGlobal: true);
			}
		}

		public unsafe static IVertexBuffer CreateSimpleVB1(MyMwmData mwmData)
		{
			string name = "VB1-" + mwmData.MwmFilepath;
			MyVertexFormatNormalTangent[] array = new MyVertexFormatNormalTangent[mwmData.VerticesCount];
			fixed (MyVertexFormatNormalTangent* ptr = array)
			{
				for (int i = 0; i < mwmData.VerticesCount; i++)
				{
					ptr[i].Normal = mwmData.Normals[i];
					ptr[i].Tangent = mwmData.Tangents[i];
				}
			}
			fixed (MyVertexFormatNormalTangent* ptr2 = array)
			{
				void* value = ptr2;
				return MyManagers.Buffers.CreateVertexBuffer(name, array.Length, MyVertexFormatNormalTangent.STRIDE, new IntPtr(value), ResourceUsage.Immutable, isStreamOutput: false, isGlobal: true);
			}
		}

		public unsafe static IIndexBuffer CreateSimpleIB(MyMwmData mwmData)
		{
			string name = "IB-" + mwmData.MwmFilepath;
			int num = int.MinValue;
			foreach (int index in mwmData.Indices)
			{
				num = Math.Max(index, num);
			}
			if (num < 65535)
			{
				List<ushort> list = new List<ushort>(mwmData.Indices.Count);
				foreach (int index2 in mwmData.Indices)
				{
					list.Add((ushort)index2);
				}
				fixed (ushort* ptr = list.ToArray())
				{
					void* value = ptr;
					return MyManagers.Buffers.CreateIndexBuffer(name, mwmData.Indices.Count, new IntPtr(value), MyIndexBufferFormat.UShort, ResourceUsage.Immutable, isGlobal: true);
				}
			}
			fixed (int* ptr2 = mwmData.Indices.ToArray())
			{
				void* value2 = ptr2;
				return MyManagers.Buffers.CreateIndexBuffer(name, mwmData.Indices.Count, new IntPtr(value2), MyIndexBufferFormat.UInt, ResourceUsage.Immutable, isGlobal: true);
			}
		}

		public static List<MyVertexInputComponent> CreateStandardVertexInputComponents(MyMwmData mwmData)
		{
			List<MyVertexInputComponent> list = new List<MyVertexInputComponent>();
			list.Add(new MyVertexInputComponent(MyVertexInputComponentType.POSITION_PACKED));
			if (mwmData.IsSkinnedProperly)
			{
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.BLEND_WEIGHTS));
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.BLEND_INDICES));
			}
			if (mwmData.IsValid2ndStream)
			{
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.NORMAL));
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.TANGENT_SIGN_OF_BITANGENT));
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.TEXCOORD0_H));
			}
			return list;
		}

		public static List<MyVertexInputComponent> CreateDepthVertexInputComponents(MyMwmData mwmData)
		{
			List<MyVertexInputComponent> list = new List<MyVertexInputComponent>();
			list.Add(new MyVertexInputComponent(MyVertexInputComponentType.POSITION_PACKED));
			if (mwmData.IsSkinnedProperly)
			{
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.BLEND_WEIGHTS));
				list.Add(new MyVertexInputComponent(MyVertexInputComponentType.BLEND_INDICES));
			}
			return list;
		}
	}
}
