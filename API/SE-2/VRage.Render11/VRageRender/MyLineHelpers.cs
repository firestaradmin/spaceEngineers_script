using System.Collections.Generic;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Vertex;

namespace VRageRender
{
	internal class MyLineHelpers
	{
		private static void AddTriangle(List<ushort> I, int i0, int i1, int i2)
		{
			I.Add((ushort)i0);
			I.Add((ushort)i1);
			I.Add((ushort)i2);
		}

		internal static ushort[] GenerateIndices(int verticesNum)
		{
			List<ushort> list = new List<ushort>();
			ushort num = 0;
			AddTriangle(list, num, num + 1, num + 2);
			AddTriangle(list, num, num + 2, num + 3);
			while (num < verticesNum - 4)
			{
				AddTriangle(list, num, num + 4, num + 5);
				AddTriangle(list, num, num + 5, num + 1);
				AddTriangle(list, num + 1, num + 5, num + 6);
				AddTriangle(list, num + 1, num + 6, num + 2);
				AddTriangle(list, num + 2, num + 6, num + 7);
				AddTriangle(list, num + 2, num + 7, num + 3);
				AddTriangle(list, num + 3, num + 7, num + 4);
				AddTriangle(list, num + 3, num + 4, num);
				num = (ushort)(num + 4);
			}
			AddTriangle(list, num + 2, num + 1, num);
			AddTriangle(list, num + 3, num + 2, num);
			return list.ToArray();
		}

		internal static BoundingBoxD GetBoundingBox(ref Vector3D worldPointA, ref Vector3D worldPointB)
		{
			Vector3D vector3D = (worldPointA + worldPointB) * 0.5;
			Vector3D point = worldPointA - vector3D;
			Vector3D point2 = worldPointB - vector3D;
			BoundingBoxD result = BoundingBoxD.CreateInvalid();
			result.Include(ref point);
			result.Include(ref point2);
			result.Inflate(0.25);
			return result;
		}

		internal unsafe static void GenerateVertexData(ref Vector3D worldPointA, ref Vector3D worldPointB, out MyVertexFormatPositionH4[] stream0, out MyVertexFormatTexcoordNormalTangentTexindices[] stream1)
		{
			Vector3D vector3D = (worldPointA + worldPointB) * 0.5;
			Vector3 vector = worldPointA - vector3D;
			Vector3 vector2 = worldPointB - vector3D;
			float num = (vector - vector2).Length() * 10f;
			Vector3 value = vector2 - vector;
			Vector3.Normalize(ref value, out value);
			value.CalculatePerpendicularVector(out var result);
			Vector3.Cross(ref value, ref result, out var result2);
			Vector3 vector3 = result * 0.025f;
			Vector3 vector4 = result2 * 0.025f;
			List<MyVertexFormatPositionH4> list = new List<MyVertexFormatPositionH4>();
			List<MyVertexFormatTexcoordNormalTangentTexindices> list2 = new List<MyVertexFormatTexcoordNormalTangentTexindices>();
			Byte4 texIndices = new Byte4(0f, 0f, 0f, 0f);
			Vector3* ptr = stackalloc Vector3[2];
			*ptr = vector;
			ptr[1] = vector2;
			for (int i = 0; i < 2; i++)
			{
				float x = ((float)i - 0.5f) * num;
				list.Add(new MyVertexFormatPositionH4(ptr[i] + vector3));
				list2.Add(new MyVertexFormatTexcoordNormalTangentTexindices(new Vector2(x, 0f), result, Vector3.Cross(value, result), texIndices));
				list.Add(new MyVertexFormatPositionH4(ptr[i] + vector4));
				list2.Add(new MyVertexFormatTexcoordNormalTangentTexindices(new Vector2(x, 0.33333f), result2, Vector3.Cross(value, result2), texIndices));
				list.Add(new MyVertexFormatPositionH4(ptr[i] - vector3));
				list2.Add(new MyVertexFormatTexcoordNormalTangentTexindices(new Vector2(x, 0.66667f), -result, Vector3.Cross(value, -result), texIndices));
				list.Add(new MyVertexFormatPositionH4(ptr[i] - vector4));
				list2.Add(new MyVertexFormatTexcoordNormalTangentTexindices(new Vector2(x, 1f), -result2, Vector3.Cross(value, -result2), texIndices));
			}
			stream0 = list.ToArray();
			stream1 = list2.ToArray();
		}
	}
}
