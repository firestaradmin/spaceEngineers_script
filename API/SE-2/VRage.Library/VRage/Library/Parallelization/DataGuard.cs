using System;
using System.Runtime.InteropServices;

namespace VRage.Library.Parallelization
{
	/// <summary>
	/// This utility construct reports incorrect data access/synchronization
	/// Correct access is only asserted, not ensured in any way. No locking or spinning is performed in order to achieve any kind syncing!
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct DataGuard
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct AccessToken : IDisposable
		{
			void IDisposable.Dispose()
			{
			}
		}

<<<<<<< HEAD
=======
		private static bool stop;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public AccessToken Read(string reason = "")
		{
			return Shared(reason);
		}

		public AccessToken Shared(string reason = "")
		{
			return default(AccessToken);
		}

		public AccessToken Write(string reason = "")
		{
			return Exclusive(reason);
		}

		public AccessToken Exclusive(string reason = "")
		{
			return default(AccessToken);
		}
	}
}
