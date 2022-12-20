using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VRage.Library
{
	public abstract class NativeArray : IDisposable
	{
		private IntPtr m_ptr;

		public readonly int Size;

		public bool IsDisposed => m_ptr == IntPtr.Zero;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public IntPtr Ptr => m_ptr;

		protected NativeArray(int size)
		{
			Size = size;
			m_ptr = Marshal.AllocHGlobal(size);
		}

		[Conditional("DEBUG")]
		public void CheckDisposed()
		{
		}

		[Conditional("DEBUG")]
		public void UpdateAllocationTrace()
		{
		}

		/// <summary>
		/// Get the contents of this array as a span.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public unsafe Span<T> AsSpan<T>(int length)
		{
			if (length * Unsafe.SizeOf<T>() > Size)
			{
				throw new ArgumentException("Requested length is too long for the native array.");
			}
			return new Span<T>(Ptr.ToPointer(), length);
		}

		/// <summary>
		/// Get the contents of this array as a byte span.
		/// </summary>
		/// <returns></returns>
		public unsafe Span<byte> AsSpan(int length = -1)
		{
			if (length == -1)
			{
				length = Size;
			}
			if (length > Size)
			{
				throw new ArgumentException("Requested length is too long for the native array.");
			}
			return new Span<byte>(Ptr.ToPointer(), length);
		}

		public virtual void Dispose()
		{
			Marshal.FreeHGlobal(Ptr);
			m_ptr = IntPtr.Zero;
		}
	}
}
