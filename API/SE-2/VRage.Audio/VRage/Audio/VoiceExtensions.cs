using System;
using SharpDX.XAudio2;

namespace VRage.Audio
{
	internal static class VoiceExtensions
	{
		public static bool IsValid(this SourceVoice self)
		{
			if (!self.IsDisposed)
			{
				return self.NativePointer != IntPtr.Zero;
			}
			return false;
		}
	}
}
