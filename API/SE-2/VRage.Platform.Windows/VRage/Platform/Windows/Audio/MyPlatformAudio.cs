using SharpDX.Multimedia;
using SharpDX.XAudio2;
using VRage.Audio;

namespace VRage.Platform.Windows.Audio
{
	internal class MyPlatformAudio : IMyPlatformAudio
	{
		private XAudio2 m_audioEngine;

		public int DeviceCount => m_audioEngine.DeviceCount;

		public XAudio2 InitAudioEngine()
		{
			m_audioEngine = new XAudio2(XAudio2Version.Version27);
			return m_audioEngine;
		}

		public void DisposeAudioEngine()
		{
			m_audioEngine.Dispose();
			m_audioEngine = null;
		}

		public DeviceDetails GetDeviceDetails(int deviceIndex, bool _)
		{
			return m_audioEngine.GetDeviceDetails(deviceIndex);
		}

		public MasteringVoice CreateMasteringVoice(int deviceIndex)
		{
			return new MasteringVoice(m_audioEngine, 0, 0, deviceIndex);
		}

		public void InitializeAudioBuffer(AudioBuffer audioBuffer, WaveFormat format)
		{
		}
	}
}
