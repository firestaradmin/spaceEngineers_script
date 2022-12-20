using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;
using VRage.Collections;
using VRage.Render11.Common;
using VRageRender;

namespace VRage.Render11.Resources
{
	internal static class MyDeviceWriteBuffer
	{
		private class CircularWriter : IDeviceWriteBuffer, IDisposable
		{
			private int freeBegin;

			private int freeEnd;

			private int bufferSize;

			private MyGenericBuffer m_buffer;

			private readonly Queue<(MyQuery, int)> m_queries = new Queue<(MyQuery, int)>();

			public CircularWriter(int initialSize)
			{
				ReallocBuffer(initialSize);
			}

			private void ReallocBuffer(int size)
			{
				if (m_buffer != null)
				{
					m_writeBufferAllocator.Dispose(m_buffer);
				}
				bufferSize = size;
				m_buffer = m_writeBufferAllocator.Allocate(bufferSize);
				freeBegin = 0;
				freeEnd = bufferSize;
<<<<<<< HEAD
				while (m_queries.Count > 0)
=======
				while (m_queries.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyQueryFactory.RelaseEventQuery(m_queries.Dequeue().Item1);
				}
			}

			public bool Update()
			{
				bool result = false;
<<<<<<< HEAD
				while (m_queries.Count > 0)
=======
				while (m_queries.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					(MyQuery, int) tuple = m_queries.Peek();
					if (!MyRender11.RCForQueries.GetData<int>((Query)tuple.Item1, AsynchronousFlags.DoNotFlush, out var result2) || result2 <= 0)
					{
						break;
					}
					result = true;
					freeEnd = tuple.Item2;
					if (freeBegin == freeEnd)
					{
						freeBegin = 0;
						freeEnd = bufferSize;
					}
					m_queries.Dequeue();
					MyQueryFactory.RelaseEventQuery(tuple.Item1);
				}
				return result;
			}

			public (MyMapping, IBuffer, MyQuery, int Offset) Alloc(int bytes)
			{
				bytes = (bytes + 15) / 16 * 16;
				int num = -1;
				if (freeBegin < freeEnd)
				{
					if (freeBegin + bytes <= freeEnd)
					{
						num = freeBegin;
						freeBegin += bytes;
					}
				}
				else if (freeBegin + bytes <= bufferSize)
				{
					num = freeBegin;
					freeBegin += bytes;
				}
				else if (bytes <= freeEnd)
				{
					num = 0;
					freeBegin = bytes;
				}
				if (num == -1)
				{
					if (!Update())
					{
						ReallocBuffer(bufferSize * 2);
					}
					return Alloc(bytes);
				}
				MyQuery myQuery = MyQueryFactory.CreateEventQuery();
				m_queries.Enqueue((myQuery, num + bytes));
				MyMapping item = MyMapping.Map(MyRender11.RC, m_buffer, bufferSize, MapMode.WriteNoOverwrite);
				item.Offset(num);
				return (item, m_buffer, myQuery, num);
			}

			public void Dispose()
			{
				m_writeBufferAllocator.Dispose(m_buffer);
				m_buffer = null;
			}
		}

		private class BufferedWriter : IDeviceWriteBuffer, IDisposable
		{
			private readonly MyConcurrentBufferPool<MyGenericBuffer> m_meshWriteOutBufferPool = new MyConcurrentBufferPool<MyGenericBuffer>("MyMeshHWBufferPool", m_writeBufferAllocator);

			private readonly Queue<MyGenericBuffer> m_buffersToReturn = new Queue<MyGenericBuffer>();

			public (MyMapping, IBuffer, MyQuery, int Offset) Alloc(int bytes)
			{
				RecycleFreeBuffers();
				MyGenericBuffer myGenericBuffer = m_meshWriteOutBufferPool.Get(bytes);
				if (myGenericBuffer.Query == null)
				{
					myGenericBuffer.Query = MyQueryFactory.CreateEventQuery();
				}
				m_buffersToReturn.Enqueue(myGenericBuffer);
				return (MyMapping.Map(MyRender11.RC, myGenericBuffer, bytes, MapMode.WriteDiscard), myGenericBuffer, myGenericBuffer.Query, 0);
			}

			private void RecycleFreeBuffers()
			{
<<<<<<< HEAD
				while (m_buffersToReturn.Count > 0)
=======
				while (m_buffersToReturn.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyGenericBuffer myGenericBuffer = m_buffersToReturn.Peek();
					if (MyImmediateRC.RC.GetData<int>((Query)myGenericBuffer.Query, AsynchronousFlags.DoNotFlush, out var result) && result > 0)
					{
						m_meshWriteOutBufferPool.Return(myGenericBuffer);
						m_buffersToReturn.Dequeue();
						continue;
					}
					break;
				}
			}

			public void Dispose()
			{
<<<<<<< HEAD
				foreach (MyGenericBuffer item in m_buffersToReturn)
				{
					m_meshWriteOutBufferPool.Return(item);
=======
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				Enumerator<MyGenericBuffer> enumerator = m_buffersToReturn.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyGenericBuffer current = enumerator.get_Current();
						m_meshWriteOutBufferPool.Return(current);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				m_buffersToReturn.Clear();
			}
		}

		private static readonly MyGenericBufferAllocator m_writeBufferAllocator = new MyGenericBufferAllocator(createQuery: false, new BufferDescription(0, ResourceUsage.Dynamic, BindFlags.VertexBuffer | BindFlags.IndexBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 16), MyManagers.Buffers.BufferMemoryTacker.RegisterSubsystem("MyDeviceWriteBuffers"));

		public static IDeviceWriteBuffer CreateWriteBuffer(int initialSize)
		{
			if (MyVRage.Platform.System.IsDeprecatedOS)
			{
				return new BufferedWriter();
			}
			return new CircularWriter(initialSize);
		}
	}
}
