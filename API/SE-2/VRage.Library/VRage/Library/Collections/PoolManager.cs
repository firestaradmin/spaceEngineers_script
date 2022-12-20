using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Library.Collections
{
	/// <summary>
	/// A simple thread safe manager for all sorts of pooled objects.
	///
	/// This 
	/// </summary>
	public static class PoolManager
	{
		public struct ReturnHandle<TObject> : IDisposable where TObject : new()
		{
			private TObject m_obj;

			public ReturnHandle(TObject data)
			{
				this = default(ReturnHandle<TObject>);
				m_obj = data;
			}

			public void Dispose()
			{
				Return(ref m_obj);
			}
		}

		public struct ArrayReturnHandle<TElement> : IDisposable
		{
			private TElement[] m_array;

			public ArrayReturnHandle(TElement[] data)
			{
				m_array = data;
			}

			public void Dispose()
			{
				ReturnBorrowedArray(ref m_array);
			}
		}

		private static readonly ConcurrentDictionary<Type, IConcurrentPool> Pools = new ConcurrentDictionary<Type, IConcurrentPool>();

		/// <summary>
		/// Preallocate a number of elements for a given object pool.
		///
		/// This call only produces an effect if the pool has not yet been initialized.
		/// </summary>
		/// <typeparam name="TPooled">The type of the pooled object.</typeparam>
		/// <param name="size">The number of elements to preallocate, this can be zero.</param>
		public static void Preallocate<TPooled>(int size) where TPooled : new()
		{
			Type typeFromHandle = typeof(TPooled);
			if (!Pools.ContainsKey(typeFromHandle))
			{
<<<<<<< HEAD
				Pools[typeFromHandle] = GetPool<TPooled>(typeFromHandle, size);
=======
				Pools.set_Item(typeFromHandle, GetPool<TPooled>(typeFromHandle, size));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		/// <summary>
		/// Get an instance from a pool.
		///
		/// Pooled resources represent data structures that are cached to prevent unnecessary memory allocations in time critical moments.
		///
		/// Pooled resources must always be return to the pool when no longer necessary.
		/// </summary>
		/// <typeparam name="TPooled">The type of the object to retrieve.</typeparam>
		/// <returns>Retrieve an instance of an object from the appropriate pool, if no pool exists one is created for the type.</returns>
		public static TPooled Get<TPooled>() where TPooled : new()
		{
			Type typeFromHandle = typeof(TPooled);
<<<<<<< HEAD
			if (!Pools.TryGetValue(typeFromHandle, out var value))
			{
				value = (Pools[typeFromHandle] = GetPool<TPooled>(typeFromHandle));
			}
			return (TPooled)value.Get();
=======
			IConcurrentPool pool = default(IConcurrentPool);
			if (!Pools.TryGetValue(typeFromHandle, ref pool))
			{
				Pools.set_Item(typeFromHandle, pool = GetPool<TPooled>(typeFromHandle));
			}
			return (TPooled)pool.Get();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// Get an instance from a pool.
		///
		/// Pooled resources represent data structures that are cached to prevent unnecessary memory allocations in time critical moments.
		///
		/// Pooled resources must always be return to the pool when no longer necessary.
		/// </summary>
		/// <typeparam name="TPooled">The type of the object to retrieve.</typeparam>
		/// <returns>Retrieve an instance of an object from the appropriate pool, if no pool exists one is created for the type.</returns>
		public static ReturnHandle<TPooled> Get<TPooled>(out TPooled poolObject) where TPooled : new()
		{
			Type typeFromHandle = typeof(TPooled);
<<<<<<< HEAD
			if (!Pools.TryGetValue(typeFromHandle, out var value))
			{
				value = (Pools[typeFromHandle] = GetPool<TPooled>(typeFromHandle));
			}
			poolObject = (TPooled)value.Get();
=======
			IConcurrentPool pool = default(IConcurrentPool);
			if (!Pools.TryGetValue(typeFromHandle, ref pool))
			{
				Pools.set_Item(typeFromHandle, pool = GetPool<TPooled>(typeFromHandle));
			}
			poolObject = (TPooled)pool.Get();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return new ReturnHandle<TPooled>(poolObject);
		}

		private static IConcurrentPool GetPool<TPooled>(Type type, int preallocated = 0) where TPooled : new()
		{
			Type typeFromHandle = typeof(ICollection<>);
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeFromHandle)
				{
					Type type3 = typeof(MyConcurrentCollectionPool<, >).MakeGenericType(type, type2.GetGenericArguments()[0]);
					return (IConcurrentPool)Activator.CreateInstance(type3, preallocated);
				}
			}
			return new MyConcurrentPool<TPooled>(preallocated);
		}

		/// <summary>
		/// Return an object to it's pool.
		///
		/// Note that if no pool has been allocated for the object type this call does nothing.
		///
		/// If this type of behaviour is undesirable one may preallocate the pool <see cref="M:VRage.Library.Collections.PoolManager.Preallocate``1(System.Int32)" />.
		/// In this case a pre-allocation of size 0 ensures the pool exists but does not allocate any instances.
		/// </summary>
		/// <typeparam name="TPooled">The type of the object to return to the pool.</typeparam>
		/// <param name="obj">An object to return to a pool.</param>
		public static void Return<TPooled>(ref TPooled obj) where TPooled : new()
		{
			Type typeFromHandle = typeof(TPooled);
<<<<<<< HEAD
			if (Pools.TryGetValue(typeFromHandle, out var value))
			{
				value.Return(obj);
=======
			IConcurrentPool concurrentPool = default(IConcurrentPool);
			if (Pools.TryGetValue(typeFromHandle, ref concurrentPool))
			{
				concurrentPool.Return(obj);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			obj = default(TPooled);
		}

		/// <summary>
		/// Borrow an array.
		/// </summary>
		/// <remarks>The array can be returned using the provided handle or <see cref="M:VRage.Library.Collections.PoolManager.ReturnBorrowedArray``1(``0[]@)" /></remarks>
		/// <typeparam name="TElement">The array element type.</typeparam>
		/// <param name="size">The minimum size of the requested array, this can be smaller than the actual length.</param>
		/// <param name="array">The borrowed array, the length of it may be longer than the requested size.</param>
		/// <returns>A handle that can be used to return the array once it's use is no longer required.</returns>
		public static ArrayReturnHandle<TElement> BorrowArray<TElement>(int size, out TElement[] array)
		{
			array = ArrayPool<TElement>.Shared.Rent(size);
			return new ArrayReturnHandle<TElement>(array);
		}

		/// <summary>
		/// Return an array that was borrowed via <see cref="M:VRage.Library.Collections.PoolManager.BorrowArray``1(System.Int32,``0[]@)" />
		/// </summary>
		/// <typeparam name="TElement">The array element type.</typeparam>
		/// <param name="array"></param>
		public static void ReturnBorrowedArray<TElement>(ref TElement[] array)
		{
			ArrayPool<TElement>.Shared.Return(array);
			array = null;
		}

		/// <summary>
		/// Borrow a memory segment.
		/// </summary>
		/// <remarks>The memory can be returned using the provided handle or <see cref="M:VRage.Library.Collections.PoolManager.ReturnBorrowedArray``1(``0[]@)" /></remarks>
		/// <typeparam name="TElement">The memory element type.</typeparam>
		/// <param name="size">The minimum size of the requested memory, this can be smaller than the actual length.</param>
		/// <param name="memory">The borrowed memory, the length of it may be longer than the requested size.</param>
		/// <returns>A handle that can be used to return the memory once it's use is no longer required.</returns>
		public static ArrayReturnHandle<TElement> BorrowMemory<TElement>(int size, out PooledMemory<TElement> memory)
		{
			TElement[] array = ArrayPool<TElement>.Shared.Rent(size);
			memory = new PooledMemory<TElement>(array, size);
			return new ArrayReturnHandle<TElement>(array);
		}

		/// <summary>
		/// Return a memory segment that was borrowed via <see cref="M:VRage.Library.Collections.PoolManager.BorrowMemory``1(System.Int32,VRage.Library.Collections.PooledMemory{``0}@)" />
		/// </summary>
		/// <typeparam name="TElement">The memory element type.</typeparam>
		/// <param name="memory"></param>
		public static void ReturnBorrowedMemory<TElement>(ref PooledMemory<TElement> memory)
		{
			ArrayPool<TElement>.Shared.Return(memory.Array);
			memory = default(PooledMemory<TElement>);
		}

		/// <summary>
		/// Borrow a span of memory containing elements of the requested type.
		/// </summary>
		/// <typeparam name="TElement">The array element type.</typeparam>
		/// <param name="size">The length of the requested span.</param>
		/// <param name="span">The borrowed span.</param>
		/// <returns>A token used to return the borrowed memory span.</returns>
		public static ArrayReturnHandle<TElement> BorrowSpan<TElement>(int size, out Span<TElement> span)
		{
			TElement[] array = ArrayPool<TElement>.Shared.Rent(size);
			span = new Span<TElement>(array, 0, size);
			return new ArrayReturnHandle<TElement>(array);
		}
	}
}
