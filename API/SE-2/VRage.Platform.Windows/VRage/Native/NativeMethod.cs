using System;

namespace VRage.Native
{
	public static class NativeMethod
	{
		public unsafe static IntPtr CalculateAddress(IntPtr instance, int methodOffset)
		{
			return *(IntPtr*)instance.ToPointer() + methodOffset * sizeof(void*);
		}
	}
}
