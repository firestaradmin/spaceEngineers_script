using System;

namespace SpaceEngineers.Game.VoiceChat
{
	public sealed class OpusEncoder : OpusDevice
	{
		private struct FrameSizes
		{
			public const int Count = 6;

			public unsafe fixed int Bytes[6];
		}

		public enum FormatT : byte
		{
			SHORT = 2,
			FLOAT = 4
		}

		private FrameSizes Frames;

		public int Channels { get; }

		public int TargetBitRate { get; }

		public FormatT Format { get; }

		public int SamplesPerSecond { get; }

<<<<<<< HEAD
		public CodingMode Mode { get; }

		public int MaxPacketSize { get; }

		public unsafe OpusEncoder(int samplesPerSecond, FormatT format, int? targetBitRate = null, int maxPacketSize = 1276, int channels = 1, CodingMode mode = CodingMode.Voip)
=======
		public new Application Application { get; }

		public int MaxPacketSize { get; }

		public unsafe OpusEncoder(int samplesPerSecond, FormatT format, int? targetBitRate = null, int maxPacketSize = 1276, int channels = 1, Application application = Application.Voip)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			switch (samplesPerSecond)
			{
			default:
				throw new Exception("Unsupported sampling frequency " + samplesPerSecond);
			case 8000:
			case 12000:
			case 16000:
			case 24000:
			case 48000:
			{
				Format = format;
				Channels = channels;
<<<<<<< HEAD
				Mode = mode;
				MaxPacketSize = maxPacketSize;
				SamplesPerSecond = samplesPerSecond;
				int error = default(int);
				Device = Native.opus_encoder_create(samplesPerSecond, channels, (int)mode, &error);
=======
				Application = application;
				MaxPacketSize = maxPacketSize;
				SamplesPerSecond = samplesPerSecond;
				int error = default(int);
				Device = Native.opus_encoder_create(samplesPerSecond, channels, (int)application, &error);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				CheckError(error, "opus_encoder_create");
				if (targetBitRate.HasValue)
				{
					TargetBitRate = targetBitRate.Value;
					error = Native.opus_encoder_ctl(Device, Ctl.SET_BITRATE_REQUEST, TargetBitRate);
					CheckError(error, "opus_encoder_ctl SET_BITRATE_REQUEST");
				}
				else
				{
					error = Native.opus_encoder_ctl(Device, Ctl.GET_BITRATE_REQUEST, out var value);
					CheckError(error, "opus_encoder_ctl GET_BITRATE_REQUEST");
					TargetBitRate = value;
				}
				ref int fixedElementField = ref Frames.Bytes[0];
				fixedElementField = (int)format * channels * (samplesPerSecond / 1000 * 60);
				Frames.Bytes[1] = (int)format * channels * (samplesPerSecond / 1000 * 40);
				Frames.Bytes[2] = (int)format * channels * (samplesPerSecond / 1000 * 20);
				Frames.Bytes[3] = (int)format * channels * (samplesPerSecond / 1000 * 10);
				Frames.Bytes[4] = (int)format * channels * (samplesPerSecond / 1000 * 5);
				Frames.Bytes[5] = (int)format * channels * (int)((double)(samplesPerSecond / 1000) * 2.5);
				break;
			}
			}
		}

		public unsafe void Encode(Span<byte> data, out int consumedBytes, out byte[] packet, out int packetSize)
		{
			int num = 0;
			for (int i = 0; i < 6; i++)
			{
				int num2 = Frames.Bytes[i];
				if (num2 <= data.Length)
				{
					num = num2;
					break;
				}
			}
			consumedBytes = num;
			if (num > 0)
			{
				byte[] tempBuffer = GetTempBuffer(MaxPacketSize);
				fixed (byte* pcm = data)
				{
					fixed (byte* dataOut = tempBuffer)
					{
						int frame_size = num / Channels / (int)Format;
						if (Format == FormatT.SHORT)
						{
							packetSize = Native.opus_encode(Device, pcm, frame_size, dataOut, MaxPacketSize);
						}
						else
						{
							packetSize = Native.opus_encode_float(Device, pcm, frame_size, dataOut, MaxPacketSize);
						}
						if (packetSize < 0)
						{
							CheckError(packetSize, "opus_encode");
						}
						if (packetSize >= 2)
						{
							packet = tempBuffer;
							return;
						}
					}
				}
			}
			packetSize = 0;
			packet = null;
		}

		protected override void DisposeDevice(IntPtr device)
		{
			Native.opus_encoder_destroy(device);
		}
	}
}
