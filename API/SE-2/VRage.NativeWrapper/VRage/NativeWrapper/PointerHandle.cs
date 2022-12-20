using System;
using System.Diagnostics;

namespace VRage.NativeWrapper
{
	public abstract class PointerHandle : IDisposable
	{
		protected bool TrackReferences = true;

		protected IntPtr m_handle;

		public bool IsDisposed { get; private set; }

		internal IntPtr NativeObject => m_handle;

		public void Dispose()
		{
			DisposeInternal(disposing: true);
			GC.SuppressFinalize(this);
		}

		internal PointerHandle()
		{
			m_handle = IntPtr.Zero;
		}

		internal PointerHandle(IntPtr ptr)
		{
			m_handle = ptr;
		}

		[Conditional("DEBUG")]
		protected void CheckDisposed()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
		}

		protected abstract void Dispose(bool disposing);

		private void DisposeInternal(bool disposing)
		{
			if (!IsDisposed)
			{
				Dispose(disposing);
				IsDisposed = true;
			}
		}

		~PointerHandle()
		{
			_ = TrackReferences;
			DisposeInternal(disposing: false);
		}
	}
}
