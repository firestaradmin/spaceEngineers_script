using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ParallelTasks;
using VRage.Library;
using VRage.Library.Memory;

namespace VRage
{
	public static class MyDebug
	{
		public static readonly List<TraceListener> Listeners = new List<TraceListener>();

		public static readonly NativeArrayAllocator DebugMemoryAllocator = new NativeArrayAllocator(Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("Debug"));

		[Conditional("NEVER")]
		public static void Assert(bool condition, string message = null)
		{
		}

		[Conditional("NEVER")]
		public static void Fail(string message)
		{
		}

		public static void WriteLine(string message, [CallerFilePath] string file = null, [CallerLineNumber] int line = -1)
		{
			foreach (TraceListener listener in Listeners)
			{
				IAdvancedDebugListener advancedDebugListener;
				if ((advancedDebugListener = listener as IAdvancedDebugListener) != null)
				{
					advancedDebugListener.WriteLine(message, file, line);
				}
				else
				{
					listener.WriteLine(message);
				}
			}
		}

		[DebuggerStepThrough]
		public static void AssertRelease(bool condition, string message = null, [CallerFilePath] string file = null, [CallerLineNumber] int line = -1)
		{
			if (!condition)
			{
				FailRelease(message, file, line);
			}
		}

		[DebuggerStepThrough]
		public static void FailRelease(string message, [CallerFilePath] string file = null, [CallerLineNumber] int line = -1)
		{
			foreach (TraceListener listener in Listeners)
			{
				IAdvancedDebugListener advancedDebugListener;
				if ((advancedDebugListener = listener as IAdvancedDebugListener) != null)
				{
					advancedDebugListener.Fail(message, null, file, line);
				}
				else
				{
					listener.Fail(message);
				}
			}
		}
	}
}
