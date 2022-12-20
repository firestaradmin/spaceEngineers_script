namespace VRage.GameServices
{
	public interface IMyMicrophoneService
	{
		bool FiltersSilence { get; }

		void InitializeVoiceRecording();

		void DisposeVoiceRecording();

		void StartVoiceRecording();

		void StopVoiceRecording();

		byte[] GetVoiceDataFormat();

		MyVoiceResult GetAvailableVoice(ref byte[] buffer, out uint size);
	}
}
