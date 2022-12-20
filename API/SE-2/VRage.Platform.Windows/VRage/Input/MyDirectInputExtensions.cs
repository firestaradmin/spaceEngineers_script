using SharpDX;
using SharpDX.DirectInput;
using VRage.Native;

namespace VRage.Input
{
	internal static class MyDirectInputExtensions
	{
		public static Result TryAcquire(this Device device)
		{
			return NativeCall<int>.Method(device.NativePointer, 7);
		}
	}
}
