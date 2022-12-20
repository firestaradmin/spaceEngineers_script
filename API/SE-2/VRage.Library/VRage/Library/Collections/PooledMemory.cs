using System;
using System.Diagnostics;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Represents a type-safe segment of memory that is borrowed from a pool.
	/// </summary>
	/// <seealso cref="T:VRage.Library.Collections.PoolManager" />
	/// <typeparam name="T"></typeparam>
	[DebuggerDisplay("{ToString(),raw}")]
	[DebuggerTypeProxy(typeof(PooledMemory<>.DebugView))]
	public readonly struct PooledMemory<T>
	{
		private sealed class DebugView
		{
			private readonly T[] _array;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public T[] Items => _array;

			public DebugView(PooledMemory<T> memory)
			{
				_array = memory.Span.ToArray();
			}
		}

		internal readonly T[] Array;

		public readonly int Length;

		public Span<T> Span => new Span<T>(Array, 0, Length);

		public bool IsEmpty => Length == 0;

		public ref T this[int index]
		{
			get
			{
				if (index < 0 || index >= Length)
				{
					throw new ArgumentOutOfRangeException();
				}
				return ref Array[index];
			}
		}

		internal PooledMemory(T[] array, int length)
		{
			Array = array;
			Length = length;
		}

		public static implicit operator Memory<T>(PooledMemory<T> pooled)
		{
			return new Memory<T>(pooled.Array, 0, pooled.Length);
		}

		public static implicit operator ReadOnlyMemory<T>(PooledMemory<T> pooled)
		{
			return new ReadOnlyMemory<T>(pooled.Array, 0, pooled.Length);
		}

		public static implicit operator Span<T>(PooledMemory<T> pooled)
		{
			return pooled.Span;
		}

		public static implicit operator ReadOnlySpan<T>(PooledMemory<T> pooled)
		{
			return pooled.Span;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			if (typeof(T) != typeof(char))
			{
				return $"PooledMemory<{typeof(T).Name}>[{Length}]";
			}
			return Span.ToString();
		}
	}
}
