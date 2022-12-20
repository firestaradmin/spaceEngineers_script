using System;

namespace VRage.Audio
{
	public interface IMySourceVoice
	{
		bool IsValid { get; }

		Action<IMySourceVoice> StoppedPlaying { get; set; }

		bool IsPlaying { get; }

		float FrequencyRatio { get; set; }

		bool IsLoopable { get; }

		MyCueId CueEnum { get; }

		bool IsBuffered { get; }

		bool IsPaused { get; }

		float Volume { get; }

		float VolumeMultiplier { get; set; }

		void Start(bool skipIntro, bool skipToEnd = false);

		void Stop(bool force = false);

		void StartBuffered();

		void SubmitBuffer(byte[] buffer);

		void Pause();

		void Resume();

		void SetVolume(float value);

		void Destroy();
	}
}
