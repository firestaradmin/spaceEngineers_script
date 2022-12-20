using System;

namespace VRageRender
{
	internal struct MyBillboardDataArray
	{
		public MyBillboardData[] Data;

		public MyBillboardVertexData[] Vertex;

		public int Length => Data.Length;

		public MyBillboardDataArray(int size)
		{
			Data = new MyBillboardData[size];
			Vertex = new MyBillboardVertexData[size];
		}

		public void Resize(int size)
		{
			if (size != Length)
			{
				Array.Resize(ref Data, size);
				Array.Resize(ref Vertex, size);
			}
		}
	}
}
