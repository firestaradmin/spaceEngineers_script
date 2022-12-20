using System;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Media;
using VRage.Audio;

namespace VRage.UserInterface.Media
{
	public class MySound : SoundBase
	{
		private GuiSounds sound;

		public override float Volume { get; set; }

		public override SoundState State => SoundState.Stopped;

		public MySound(object nativeSound)
			: base(nativeSound)
		{
			Enum.TryParse<GuiSounds>(nativeSound.ToString(), out sound);
		}

		public override void Pause()
		{
		}

		public override void Play()
		{
			if (Engine.Instance != null)
			{
				MyAudioDevice myAudioDevice = Engine.Instance.AudioDevice as MyAudioDevice;
				if (myAudioDevice?.GuiAudio != null)
				{
					myAudioDevice.GuiAudio.PlaySound(sound);
				}
			}
		}

		public override void Stop()
		{
		}
	}
}
