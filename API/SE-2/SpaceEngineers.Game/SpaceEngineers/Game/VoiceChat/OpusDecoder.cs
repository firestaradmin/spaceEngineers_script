using System;

namespace SpaceEngineers.Game.VoiceChat
{
	public sealed class OpusDecoder : OpusDevice
	{
		public int Channels { get; }

		public int SamplesPerSecond { get; }

		public unsafe OpusDecoder(int samplesPerSecond = 48000, int channels = 1)
		{
			Channels = 1;
			SamplesPerSecond = samplesPerSecond;
			int error = default(int);
			Device = Native.opus_decoder_create(samplesPerSecond, channels, &error);
			CheckError(error, "opus_decoder_create");
		}

		public unsafe Span<byte> Decode(Span<byte> packet)
		{
			fixed (byte* ptr = packet)
			{
				int num = Native.opus_packet_get_nb_samples(ptr, packet.Length, SamplesPerSecond);
				int num2 = 2 * num * Channels;
				byte[] tempBuffer = GetTempBuffer(num2);
				fixed (byte* pcmOut = tempBuffer)
				{
					int num3 = Native.opus_decode(Device, ptr, packet.Length, pcmOut, num, 0);
					if (num3 != num)
					{
						CheckError(num3, "opus_decode");
					}
				}
				return tempBuffer.Span(0, num2);
			}
		}

		protected override void DisposeDevice(IntPtr device)
		{
			Native.opus_decoder_destroy(device);
		}
	}
}
