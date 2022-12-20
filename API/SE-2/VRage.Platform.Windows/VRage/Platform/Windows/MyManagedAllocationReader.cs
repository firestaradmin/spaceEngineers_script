using System;
using System.Runtime.InteropServices;

namespace VRage.Platform.Windows
{
	public static class MyManagedAllocationReader
	{
		private const string NATIVE_CLR_PROFILER = "Native_CLR_Profiler";

		[ThreadStatic]
		private static IntPtr ThreadAllocationStampNativePtr;

		public static bool IsReady => GetModuleHandle("Native_CLR_Profiler") != IntPtr.Zero;

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procname);

		[DllImport("Native_CLR_Profiler")]
		private static extern ulong GetTotalAllocations();

		[DllImport("Native_CLR_Profiler")]
		private static extern IntPtr GetThreadAllocationPtr();

		public unsafe static ulong GetThreadAllocationStamp()
		{
			IntPtr intPtr = ThreadAllocationStampNativePtr;
			if (intPtr == IntPtr.Zero)
			{
				IntPtr moduleHandle = GetModuleHandle("Native_CLR_Profiler");
				if (moduleHandle == IntPtr.Zero)
				{
					throw new Exception("Native profiler is not attached!");
				}
				if (GetProcAddress(moduleHandle, "GetThreadAllocationPtr") == IntPtr.Zero)
				{
					throw new Exception("GetThreadAllocationPtr not found!");
				}
				intPtr = (ThreadAllocationStampNativePtr = GetThreadAllocationPtr());
			}
			return *(ulong*)(void*)intPtr;
		}

		public static ulong GetGlobalAllocationsStamp()
		{
			return GetTotalAllocations();
		}
	}
}
