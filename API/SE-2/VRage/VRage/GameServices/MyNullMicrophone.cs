using System;

namespace VRage.GameServices
{
	public class MyNullMicrophone : IMyMicrophoneService
	{
		public bool FiltersSilence => false;

		public void InitializeVoiceRecording()
		{
		}

		public void DisposeVoiceRecording()
		{
		}

		public void StartVoiceRecording()
		{
		}

		public void StopVoiceRecording()
		{
		}

		public byte[] GetVoiceDataFormat()
		{
			throw new NotImplementedException();
		}

		public MyVoiceResult GetAvailableVoice(ref byte[] buffer, out uint size)
		{
			size = 0u;
			return MyVoiceResult.NoData;
		}
	}
}
