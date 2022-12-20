using System;
using System.Threading;
using ParallelTasks;

namespace VRage.Library.Threading
{
	/// <summary>
	/// A struct which implements a spin lock.
	/// </summary>
	public struct SpinLock
	{
		private Thread owner;

		private int recursion;

		/// <summary>
		/// Enters the lock. The calling thread will spin wait until it gains ownership of the lock.
		/// </summary>
		public void Enter()
		{
<<<<<<< HEAD
			Thread currentThread = Thread.CurrentThread;
=======
			Thread currentThread = Thread.get_CurrentThread();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (owner == currentThread)
			{
				Interlocked.Increment(ref recursion);
				return;
			}
			MySpinWait mySpinWait = default(MySpinWait);
			while (Interlocked.CompareExchange(ref owner, currentThread, null) != null)
			{
				mySpinWait.SpinOnce();
			}
			Interlocked.Increment(ref recursion);
		}

		/// <summary>
		/// Tries to enter the lock.
		/// </summary>
		/// <returns><c>true</c> if the lock was successfully taken; else <c>false</c>.</returns>
		public bool TryEnter()
		{
<<<<<<< HEAD
			Thread currentThread = Thread.CurrentThread;
=======
			Thread currentThread = Thread.get_CurrentThread();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (owner == currentThread)
			{
				Interlocked.Increment(ref recursion);
				return true;
			}
			bool flag = Interlocked.CompareExchange(ref owner, currentThread, null) == null;
			if (flag)
			{
				Interlocked.Increment(ref recursion);
			}
			return flag;
		}

		/// <summary>
		/// Exits the lock. This allows other threads to take ownership of the lock.
		/// </summary>
		public void Exit()
		{
<<<<<<< HEAD
			Thread currentThread = Thread.CurrentThread;
=======
			Thread currentThread = Thread.get_CurrentThread();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (currentThread == owner)
			{
				Interlocked.Decrement(ref recursion);
				if (recursion == 0)
				{
					owner = null;
				}
				return;
			}
			throw new InvalidOperationException("Exit cannot be called by a thread which does not currently own the lock.");
		}
	}
}
