using System;
using Steamworks;
using VRage.GameServices;

namespace VRage.Steam
{
	internal class MySteamMicrophone : IMyMicrophoneService
	{
		private byte[] VoiceFormat = new byte[18]
		{
			1, 0, 1, 0, 0, 0, 0, 0, 128, 187,
			0, 0, 2, 0, 16, 0, 0, 0
		};

		private byte[] m_readBuffer = new byte[8192];

		public bool FiltersSilence => true;

		private uint DataSampleRate
		{
			get
			{
				uint num = SteamUser.GetVoiceOptimalSampleRate();
				if (num > 24000)
				{
					num = 24000u;
				}
				return num;
			}
		}

		public void InitializeVoiceRecording()
		{
		}

		public void DisposeVoiceRecording()
		{
		}

		public void StartVoiceRecording()
		{
			SteamUser.StartVoiceRecording();
		}

		public void StopVoiceRecording()
		{
			SteamUser.StopVoiceRecording();
		}

		public byte[] GetVoiceDataFormat()
		{
			byte[] voiceFormat = VoiceFormat;
			uint dataSampleRate = DataSampleRate;
			for (int i = 0; i < 4; i++)
			{
				voiceFormat[4 + i] = (byte)((dataSampleRate >> i * 8) & 0xFFu);
			}
			return voiceFormat;
		}

		public MyVoiceResult GetAvailableVoice(ref byte[] buffer, out uint size)
		{
			uint pcbCompressed;
			EVoiceResult eVoiceResult = SteamUser.GetAvailableVoice(out pcbCompressed);
			size = 0u;
			if (eVoiceResult == EVoiceResult.k_EVoiceResultOK)
			{
				ArrayExtensions.EnsureCapacity(ref m_readBuffer, (int)pcbCompressed);
				eVoiceResult = SteamUser.GetVoice(bWantCompressed: true, m_readBuffer, (uint)m_readBuffer.Length, out pcbCompressed);
				if (eVoiceResult == EVoiceResult.k_EVoiceResultOK)
				{
					size = pcbCompressed * 2;
					do
					{
						ArrayExtensions.EnsureCapacity(ref buffer, (int)size, 2f);
						eVoiceResult = SteamUser.DecompressVoice(m_readBuffer, pcbCompressed, buffer, (uint)buffer.Length, out size, DataSampleRate);
					}
					while (eVoiceResult == EVoiceResult.k_EVoiceResultBufferTooSmall);
				}
			}
			return (MyVoiceResult)eVoiceResult;
		}
	}
}
