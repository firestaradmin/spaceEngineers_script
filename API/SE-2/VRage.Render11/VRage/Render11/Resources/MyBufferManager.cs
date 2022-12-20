using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Collections;
using VRage.Generics;
using VRage.Library.Collections;
using VRage.Library.Memory;
using VRage.Render11.Common;
using VRage.Render11.Resources.Buffers;

namespace VRage.Render11.Resources
{
	internal class MyBufferManager : IManager, IManagerDevice, IManagerUnloadData
	{
		private interface IMyBufferPool
		{
			MyBufferStatistics Statistics { get; }

			int GlobalCount { get; }

			void DisposeAll();
		}

		private class MyBufferPool<T> : MyConcurrentObjectsPool<T>, IMyBufferPool where T : class, IBuffer, new()
		{
			private MyBufferStatistics m_statistics;

			private Action<T> m_actionDisposeInternal = delegate(T buffer)
			{
				buffer.DisposeInternal();
			};

			private int m_globalCount;

			public MyMemorySystem MemorySystem { get; }

			public MyBufferStatistics Statistics
			{
				get
				{
					m_statistics.TotalBuffersAllocated = Math.Max(m_statistics.TotalBuffersAllocated, m_statistics.ActiveBuffers);
					m_statistics.TotalBytesAllocated = Math.Max(m_statistics.TotalBytesAllocated, m_statistics.ActiveBytes);
					return m_statistics;
				}
				private set
				{
					m_statistics = value;
				}
			}

			public int GlobalCount => m_globalCount;

			public MyBufferPool(string name, int baseCapacity, MyMemorySystem memorySystem)
				: base(baseCapacity)
			{
				m_statistics.Name = name;
				MemorySystem = memorySystem;
			}

			public void DisposeAll()
			{
				ApplyActionOnAllActives(m_actionDisposeInternal);
				DeallocateAll();
			}

			public void LogAllocate(IBuffer buffer)
			{
				Interlocked.Increment(ref m_statistics.ActiveBuffers);
				Interlocked.Add(ref m_statistics.ActiveBytes, buffer.Description.SizeInBytes);
				if (buffer.IsGlobal)
				{
					Interlocked.Increment(ref m_globalCount);
				}
			}

			public void LogDispose(IBuffer buffer)
			{
				if (buffer.IsGlobal)
				{
					Interlocked.Decrement(ref m_globalCount);
				}
				m_statistics.ActiveBuffers--;
				m_statistics.ActiveBytes -= buffer.Description.SizeInBytes;
			}

			public string GetSessionBuffersString()
			{
				StringBuilder sb = new StringBuilder();
				ApplyActionOnAllActives(delegate(T x)
				{
					if (!x.IsGlobal)
					{
						sb.Append(x.Buffer.DebugName).Append("\n");
					}
				});
				return sb.ToString();
			}
		}

		private bool m_isDeviceInit;

		private readonly MyBufferPool<MySrvBuffer> m_srvBuffers;

		private readonly MyBufferPool<MyUavBuffer> m_uavBuffers;

		private readonly MyBufferPool<MySrvUavBuffer> m_srvUavBuffers;

		private readonly MyBufferPool<MyIndirectArgsBuffer> m_indirectArgsBuffers;

		private readonly MyBufferPool<MyReadBuffer> m_readBuffers;

		private readonly MyBufferPool<MyIndexBuffer> m_indexBuffers;

		private readonly MyBufferPool<MyVertexBuffer> m_vertexBuffers;

		private readonly MyBufferPool<MyConstantBuffer> m_constantBuffers;

		private readonly TypeSwitchRet<MyBufferInternal, IMyBufferPool> m_poolSwitch = new TypeSwitchRet<MyBufferInternal, IMyBufferPool>();

		public MyMemorySystem BufferMemoryTacker { get; } = MyManagers.MemoryTracker.RegisterSubsystem("Buffers");


		public MyBufferManager()
		{
			CreateBufferPool<MySrvBuffer>(out m_srvBuffers, "Srv");
			CreateBufferPool<MyUavBuffer>(out m_uavBuffers, "Uav");
			CreateBufferPool<MySrvUavBuffer>(out m_srvUavBuffers, "SrvUav");
			CreateBufferPool<MyIndirectArgsBuffer>(out m_indirectArgsBuffers, "Indirect");
			CreateBufferPool<MyReadBuffer>(out m_readBuffers, "Read");
			CreateBufferPool<MyIndexBuffer>(out m_indexBuffers, "Index");
			CreateBufferPool<MyVertexBuffer>(out m_vertexBuffers, "Vertex");
			CreateBufferPool<MyConstantBuffer>(out m_constantBuffers, "Constant");
			m_poolSwitch.Case<MySrvBuffer>(() => m_srvBuffers).Case<MyUavBuffer>(() => m_uavBuffers).Case<MySrvUavBuffer>(() => m_srvUavBuffers)
				.Case<MyIndirectArgsBuffer>(() => m_indirectArgsBuffers)
				.Case<MyReadBuffer>(() => m_readBuffers)
				.Case<MyIndexBuffer>(() => m_indexBuffers)
				.Case<MyVertexBuffer>(() => m_vertexBuffers)
				.Case<MyConstantBuffer>(() => m_constantBuffers);
			void CreateBufferPool<T>(out MyBufferPool<T> pool, string name) where T : class, IBuffer, new()
			{
				MyMemorySystem memorySystem = BufferMemoryTacker.RegisterSubsystem(name);
				pool = new MyBufferPool<T>(name, 64, memorySystem);
			}
		}

		private MyBufferPool<T> GetPool<T>() where T : MyBufferInternal, new()
		{
			return m_poolSwitch.Switch<T, MyBufferPool<T>>();
		}

		private IEnumerable<IMyBufferPool> GetAllPools()
		{
<<<<<<< HEAD
			return m_poolSwitch.Matches.Values.Select((Func<IMyBufferPool> poolFunc) => poolFunc());
=======
			return Enumerable.Select<Func<IMyBufferPool>, IMyBufferPool>((IEnumerable<Func<IMyBufferPool>>)m_poolSwitch.Matches.Values, (Func<Func<IMyBufferPool>, IMyBufferPool>)((Func<IMyBufferPool> poolFunc) => poolFunc()));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private TBuffer CreateInternal<TBuffer>(string name, ref BufferDescription description, IntPtr? initData, Action<TBuffer> initializer = null, bool isGlobal = false) where TBuffer : MyBufferInternal, new()
		{
			MyBufferPool<TBuffer> pool = GetPool<TBuffer>();
			pool.AllocateOrCreate(out var item);
			initializer?.Invoke(item);
			item.Init(name, ref description, initData, pool.MemorySystem, isGlobal);
			pool.LogAllocate(item);
			return item;
		}

		public ISrvBuffer CreateSrv(string name, int elements, int byteStride, IntPtr? initData = null, ResourceUsage usage = ResourceUsage.Default, bool isGlobal = false)
		{
			BufferDescription description = new BufferDescription(elements * byteStride, usage, BindFlags.ShaderResource, (usage == ResourceUsage.Dynamic) ? CpuAccessFlags.Write : CpuAccessFlags.None, ResourceOptionFlags.BufferStructured, byteStride);
			return CreateInternal<MySrvBuffer>(name, ref description, initData, null, isGlobal);
		}

		public IUavBuffer CreateUav(string name, int elements, int byteStride, IntPtr? initData = null, MyUavType uavType = MyUavType.Default, ResourceUsage usage = ResourceUsage.Default, bool isGlobal = false)
		{
			BufferDescription description = new BufferDescription(elements * byteStride, usage, BindFlags.ShaderResource | BindFlags.UnorderedAccess, (usage == ResourceUsage.Dynamic) ? CpuAccessFlags.Write : CpuAccessFlags.None, ResourceOptionFlags.BufferStructured, byteStride);
			return CreateInternal(name, ref description, initData, delegate(MyUavBuffer b)
			{
				b.UavType = uavType;
			}, isGlobal);
		}

		public ISrvUavBuffer CreateSrvUav(string name, int elements, int byteStride, IntPtr? initData = null, MyUavType uavType = MyUavType.Default, ResourceUsage usage = ResourceUsage.Default, bool isGlobal = false)
		{
			BufferDescription description = new BufferDescription(elements * byteStride, usage, BindFlags.ShaderResource | BindFlags.UnorderedAccess, (usage == ResourceUsage.Dynamic) ? CpuAccessFlags.Write : CpuAccessFlags.None, ResourceOptionFlags.BufferStructured, byteStride);
			return CreateInternal(name, ref description, initData, delegate(MySrvUavBuffer b)
			{
				b.UavType = uavType;
			}, isGlobal);
		}

		public IIndirectResourcesBuffer CreateIndirectArgsBuffer(string name, int elements, int byteStride, Format format = Format.R32_UInt, bool isGlobal = false)
		{
			BufferDescription description = new BufferDescription(elements * byteStride, ResourceUsage.Default, BindFlags.UnorderedAccess, CpuAccessFlags.None, ResourceOptionFlags.DrawIndirectArguments, byteStride);
			return CreateInternal(name, ref description, null, delegate(MyIndirectArgsBuffer b)
			{
				b.Format = format;
			}, isGlobal);
		}

		public IReadBuffer CreateRead(string name, int elements, int byteStride, bool isGlobal = false)
		{
			BufferDescription description = new BufferDescription(elements * byteStride, ResourceUsage.Staging, BindFlags.None, CpuAccessFlags.Read, ResourceOptionFlags.None, byteStride);
			return CreateInternal<MyReadBuffer>(name, ref description, null, null, isGlobal);
		}

		public IIndexBuffer CreateIndexBuffer(string name, int elements, IntPtr? initData = null, MyIndexBufferFormat format = MyIndexBufferFormat.UShort, ResourceUsage usage = ResourceUsage.Default, bool isGlobal = false)
		{
<<<<<<< HEAD
			int num;
			switch (format)
			{
			case MyIndexBufferFormat.UShort:
				num = 2;
				break;
			case MyIndexBufferFormat.UInt:
				num = 4;
				break;
			default:
				throw new NotImplementedException("Unsupported index buffer format.");
			}
=======
			int num = format switch
			{
				MyIndexBufferFormat.UShort => 2, 
				MyIndexBufferFormat.UInt => 4, 
				_ => throw new NotImplementedException("Unsupported index buffer format."), 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			BufferDescription description = new BufferDescription(elements * num, usage, BindFlags.IndexBuffer, (usage == ResourceUsage.Dynamic) ? CpuAccessFlags.Write : CpuAccessFlags.None, ResourceOptionFlags.None, num);
			return CreateInternal(name, ref description, initData, delegate(MyIndexBuffer b)
			{
				b.Format = format;
			}, isGlobal);
		}

		public IVertexBuffer CreateVertexBuffer(string name, int elements, int byteStride, IntPtr? initData = null, ResourceUsage usage = ResourceUsage.Default, bool isStreamOutput = false, bool isGlobal = false)
		{
			BufferDescription description = new BufferDescription(elements * byteStride, usage, BindFlags.VertexBuffer | (isStreamOutput ? BindFlags.StreamOutput : BindFlags.None), (usage == ResourceUsage.Dynamic) ? CpuAccessFlags.Write : CpuAccessFlags.None, ResourceOptionFlags.None, byteStride);
			return CreateInternal<MyVertexBuffer>(name, ref description, initData, null, isGlobal);
		}

		public IConstantBuffer CreateConstantBuffer(string name, int byteSize, IntPtr? initData = null, ResourceUsage usage = ResourceUsage.Default, bool isGlobal = false)
		{
			BufferDescription description = new BufferDescription(byteSize, usage, BindFlags.ConstantBuffer, (usage == ResourceUsage.Dynamic) ? CpuAccessFlags.Write : CpuAccessFlags.None, ResourceOptionFlags.None, 0);
			return CreateInternal<MyConstantBuffer>(name, ref description, initData, null, isGlobal);
		}

		private void ResizeInternal<TBuffer>(TBuffer buffer, int newElements, int newByteStride, IntPtr? newData, Action<TBuffer> initializer = null) where TBuffer : MyBufferInternal, new()
		{
			MyBufferPool<TBuffer> pool = GetPool<TBuffer>();
			BufferDescription description = buffer.Description;
			if (newByteStride < 0)
			{
				newByteStride = description.StructureByteStride;
			}
			description.SizeInBytes = newElements * newByteStride;
			description.StructureByteStride = newByteStride;
			if (buffer.ByteSize != description.SizeInBytes || buffer.Description.StructureByteStride != description.StructureByteStride || newData.HasValue)
			{
				pool.LogDispose(buffer);
				string name = buffer.Name;
				buffer.DisposeInternal();
				initializer?.Invoke(buffer);
				buffer.Init(name, ref description, newData, pool.MemorySystem, buffer.IsGlobal);
				pool.LogAllocate(buffer);
			}
		}

		public void Resize(ISrvBuffer buffer, int newElements, int newByteStride = -1, IntPtr? newData = null)
		{
			ResizeInternal(buffer as MySrvBuffer, newElements, newByteStride, newData);
		}

		public void Resize(IUavBindable buffer, int newElements, int newByteStride = -1, IntPtr? newData = null)
		{
			MyUavBuffer uavBuffer = buffer as MyUavBuffer;
			ResizeInternal(uavBuffer, newElements, newByteStride, newData, delegate(MyUavBuffer b)
			{
				b.UavType = uavBuffer.UavType;
			});
		}

		public void Resize(ISrvUavBindable buffer, int newElements, int newByteStride = -1, IntPtr? newData = null)
		{
			MySrvUavBuffer srvUavBuffer = buffer as MySrvUavBuffer;
			ResizeInternal(srvUavBuffer, newElements, newByteStride, newData, delegate(MySrvUavBuffer b)
			{
				b.UavType = srvUavBuffer.UavType;
			});
		}

		public void Resize(IIndirectResourcesBuffer buffer, int newElements, int newByteStride = -1, IntPtr? newData = null)
		{
			MyIndirectArgsBuffer indirectBuffer = buffer as MyIndirectArgsBuffer;
			ResizeInternal(indirectBuffer, newElements, newByteStride, newData, delegate(MyIndirectArgsBuffer b)
			{
				b.Format = indirectBuffer.Format;
			});
		}

		public void Resize(IReadBuffer buffer, int newElements, int newByteStride = -1, IntPtr? newData = null)
		{
			ResizeInternal(buffer as MyReadBuffer, newElements, newByteStride, newData);
		}

		public void Resize(IIndexBuffer buffer, int newElements, int newByteStride = -1, IntPtr? newData = null)
		{
			MyIndexBuffer indexBuffer = buffer as MyIndexBuffer;
			ResizeInternal(indexBuffer, newElements, newByteStride, newData, delegate(MyIndexBuffer b)
			{
				b.Format = indexBuffer.Format;
			});
		}

		public void Resize(IVertexBuffer buffer, int newElements, int newByteStride = -1, IntPtr? newData = null)
		{
			ResizeInternal(buffer as MyVertexBuffer, newElements, newByteStride, newData);
		}

		public void Resize(IConstantBuffer buffer, int newElements, int newByteStride = -1, IntPtr? newData = null)
		{
			ResizeInternal(buffer as MyConstantBuffer, newElements, newByteStride, newData);
		}

		public void DisposeInternal<TBuffer>(TBuffer buffer) where TBuffer : MyBufferInternal, new()
		{
			if (buffer != null && !buffer.IsReleased)
			{
				MyBufferPool<TBuffer> pool = GetPool<TBuffer>();
				pool.LogDispose(buffer);
				buffer.DisposeInternal();
				pool.Deallocate(buffer);
			}
		}

		public void Dispose(params ISrvBindable[] buffers)
		{
			if (buffers != null)
			{
				foreach (ISrvBindable srvBindable in buffers)
				{
					DisposeInternal(srvBindable as MySrvBuffer);
				}
			}
		}

		public void Dispose(params IUavBindable[] buffers)
		{
			if (buffers != null)
			{
				foreach (IUavBindable uavBindable in buffers)
				{
					DisposeInternal(uavBindable as MyUavBuffer);
				}
			}
		}

		public void Dispose(params ISrvUavBindable[] buffers)
		{
			if (buffers != null)
			{
				foreach (ISrvUavBindable srvUavBindable in buffers)
				{
					DisposeInternal(srvUavBindable as MySrvUavBuffer);
				}
			}
		}

		public void Dispose(params IIndirectResourcesBuffer[] buffers)
		{
			if (buffers != null)
			{
				foreach (IIndirectResourcesBuffer indirectResourcesBuffer in buffers)
				{
					DisposeInternal(indirectResourcesBuffer as MyIndirectArgsBuffer);
				}
			}
		}

		public void Dispose(params IReadBuffer[] buffers)
		{
			if (buffers != null)
			{
				foreach (IReadBuffer readBuffer in buffers)
				{
					DisposeInternal(readBuffer as MyReadBuffer);
				}
			}
		}

		public void Dispose(params IIndexBuffer[] buffers)
		{
			if (buffers != null)
			{
				foreach (IIndexBuffer indexBuffer in buffers)
				{
					DisposeInternal(indexBuffer as MyIndexBuffer);
				}
			}
		}

		public void Dispose(params IVertexBuffer[] buffers)
		{
			if (buffers != null)
			{
				foreach (IVertexBuffer vertexBuffer in buffers)
				{
					DisposeInternal(vertexBuffer as MyVertexBuffer);
				}
			}
		}

		public void Dispose(params IConstantBuffer[] buffers)
		{
			if (buffers != null)
			{
				foreach (IConstantBuffer constantBuffer in buffers)
				{
					DisposeInternal(constantBuffer as MyConstantBuffer);
				}
			}
		}

		public void OnDeviceInit()
		{
			m_isDeviceInit = true;
		}

		public void OnDeviceReset()
		{
			OnDeviceEnd();
			OnDeviceInit();
		}

		public void OnDeviceEnd()
		{
			m_isDeviceInit = false;
			foreach (IMyBufferPool allPool in GetAllPools())
			{
				allPool.DisposeAll();
			}
		}

		public IEnumerable<MyBufferStatistics> GetReport()
		{
			MyBufferStatistics totalStatistics = new MyBufferStatistics
			{
				Name = "Total"
			};
			foreach (IMyBufferPool allPool in GetAllPools())
			{
				totalStatistics.ActiveBytes += allPool.Statistics.ActiveBytes;
				totalStatistics.ActiveBuffers += allPool.Statistics.ActiveBuffers;
				yield return allPool.Statistics;
			}
			yield return totalStatistics;
		}

		public void OnSessionStart()
		{
		}

		public void OnUnloadData()
		{
		}
	}
}
