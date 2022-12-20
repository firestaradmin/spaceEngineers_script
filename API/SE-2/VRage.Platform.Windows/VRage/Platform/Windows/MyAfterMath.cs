using System;
using NativeAftermath;

namespace VRage.Platform.Windows
{
	internal class MyAfterMath : IAfterMath
	{
		public int Init(IntPtr device)
		{
			return (int)NativeAftermath.MyAfterMath.Init(device);
		}

		public void Shutdown()
		{
			NativeAftermath.MyAfterMath.Shutdown();
		}

		public string GetInfo(IntPtr context)
		{
			ContextData info = NativeAftermath.MyAfterMath.GetInfo(context);
			return string.Concat(info.MarkerData, "|", info.Result, "|", info.Status, "|", info.ContextStatus);
		}

		public void SetEventMarker(IntPtr nativePointer, string tag)
		{
		}
	}
}
