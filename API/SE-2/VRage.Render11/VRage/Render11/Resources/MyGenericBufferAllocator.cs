using System;
using SharpDX.Direct3D11;
using VRage.Collections;
using VRage.Library.Memory;
using VRage.Render11.Common;

namespace VRage.Render11.Resources
{
	internal class MyGenericBufferAllocator : IMyElementAllocator<MyGenericBuffer>
	{
		private MyMemorySystem m_memoryTracker;

		private readonly bool m_createQuery;

		private BufferDescription m_bufferDescription;

		public bool ExplicitlyDisposeAllElements => false;

		public MyGenericBufferAllocator(bool createQuery, BufferDescription bufferDescription, MyMemorySystem memoryTracker)
		{
			m_createQuery = createQuery;
			m_memoryTracker = memoryTracker;
			m_bufferDescription = bufferDescription;
		}

		public MyGenericBuffer PreAllocate(int size)
		{
			return new MyGenericBuffer(size);
		}

		public void AllocateResource(MyGenericBuffer buffer)
		{
			int bufferSize = buffer.BufferSize;
			BufferDescription description = m_bufferDescription;
			description.SizeInBytes = bufferSize;
			buffer.Init("MeshBuffer" + bufferSize, ref description, IntPtr.Zero, m_memoryTracker);
			if (m_createQuery)
			{
				buffer.Query = MyQueryFactory.CreateEventQuery();
			}
		}

		public MyGenericBuffer Allocate(int size)
		{
			MyGenericBuffer myGenericBuffer = PreAllocate(size);
			AllocateResource(myGenericBuffer);
			return myGenericBuffer;
		}

		public void Dispose(MyGenericBuffer instance)
		{
			if (instance.Query != null)
			{
				MyQueryFactory.RelaseEventQuery(instance.Query);
			}
			instance.DisposeInternal();
		}

		public void Init(MyGenericBuffer instance)
		{
		}

		public int GetBytes(MyGenericBuffer instance)
		{
			return instance.BufferSize;
		}

		public int GetBucketId(MyGenericBuffer instance)
		{
			return instance.BufferSize;
		}
	}
}
