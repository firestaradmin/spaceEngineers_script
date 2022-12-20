using System;
using System.Runtime.InteropServices;

namespace SpaceEngineers.Game.VoiceChat
{
	public abstract class OpusDevice : IDisposable
	{
		protected class Native
		{
			private const string OPUS_DLL = "Opus.dll";

			private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

			internal const int max_packet_size = 1276;

			[DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
			internal unsafe static extern IntPtr opus_encoder_create(int Fs, int channels, int application, int* error);

			[DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
			internal static extern void opus_encoder_destroy(IntPtr encoder);

			[DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
			internal unsafe static extern int opus_encode(IntPtr st, byte* pcm, int frame_size, byte* dataOut, int max_data_bytes);

			[DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
			internal unsafe static extern int opus_encode_float(IntPtr st, byte* pcm, int frame_size, byte* dataOut, int out_data_bytes);

			[DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
			internal unsafe static extern IntPtr opus_decoder_create(int Fs, int channels, int* error);

			[DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
			internal static extern void opus_decoder_destroy(IntPtr decoder);

			[DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
			internal unsafe static extern int opus_decode(IntPtr st, byte* data, int len, byte* pcmOut, int frame_size, int decode_fec);

			[DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
			internal static extern int opus_encoder_ctl(IntPtr st, Ctl request, int value);

			[DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
			internal static extern int opus_encoder_ctl(IntPtr st, Ctl request, out int value);

			[DllImport("Opus.dll", CallingConvention = CallingConvention.Cdecl)]
			internal unsafe static extern int opus_packet_get_nb_samples(byte* packet, int packetLen, int Fs);
		}

		public enum Ctl
		{
			SET_APPLICATION_REQUEST = 4000,
			GET_APPLICATION_REQUEST = 4001,
			SET_BITRATE_REQUEST = 4002,
			GET_BITRATE_REQUEST = 4003,
			SET_MAX_BANDWIDTH_REQUEST = 4004,
			GET_MAX_BANDWIDTH_REQUEST = 4005,
			SET_VBR_REQUEST = 4006,
			GET_VBR_REQUEST = 4007,
			SET_BANDWIDTH_REQUEST = 4008,
			GET_BANDWIDTH_REQUEST = 4009,
			SET_COMPLEXITY_REQUEST = 4010,
			GET_COMPLEXITY_REQUEST = 4011,
			SET_INBAND_FEC_REQUEST = 4012,
			GET_INBAND_FEC_REQUEST = 4013,
			SET_PACKET_LOSS_PERC_REQUEST = 4014,
			GET_PACKET_LOSS_PERC_REQUEST = 4015,
			SET_DTX_REQUEST = 4016,
			GET_DTX_REQUEST = 4017,
			SET_VBR_CONSTRAINT_REQUEST = 4020,
			GET_VBR_CONSTRAINT_REQUEST = 4021,
			SET_FORCE_CHANNELS_REQUEST = 4022,
			GET_FORCE_CHANNELS_REQUEST = 4023,
			SET_SIGNAL_REQUEST = 4024,
			GET_SIGNAL_REQUEST = 4025,
			GET_LOOKAHEAD_REQUEST = 4027,
			GET_SAMPLE_RATE_REQUEST = 4029,
			GET_FINAL_RANGE_REQUEST = 4031,
			GET_PITCH_REQUEST = 4033,
			SET_GAIN_REQUEST = 4034,
			GET_GAIN_REQUEST = 4045,
			SET_LSB_DEPTH_REQUEST = 4036,
			GET_LSB_DEPTH_REQUEST = 4037,
			GET_LAST_PACKET_DURATION_REQUEST = 4039,
			SET_EXPERT_FRAME_DURATION_REQUEST = 4040,
			GET_EXPERT_FRAME_DURATION_REQUEST = 4041,
			SET_PREDICTION_DISABLED_REQUEST = 4042,
			GET_PREDICTION_DISABLED_REQUEST = 4043,
			SET_PHASE_INVERSION_DISABLED_REQUEST = 4046,
			GET_PHASE_INVERSION_DISABLED_REQUEST = 4047,
			GET_IN_DTX_REQUEST = 4049
		}

		/// <summary>
		/// Supported coding modes.
		/// </summary>
<<<<<<< HEAD
		public enum CodingMode
=======
		public enum Application
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			/// <summary>
			/// Best for most VoIP/videoconference applications where listening quality and intelligibility matter most.
			/// </summary>
			Voip = 0x800,
			/// <summary>
			/// Best for broadcast/high-fidelity application where the decoded audio should be as close as possible to input.
			/// </summary>
			Audio = 2049,
			/// <summary>
			/// Only use when lowest-achievable latency is what matters most. Voice-optimized modes cannot be used.
			/// </summary>
			Restricted_LowLatency = 2051
		}

		public enum Errors
		{
			/// <summary>
			/// No error.
			/// </summary>
			OK = 0,
			/// <summary>
			/// One or more invalid/out of range arguments.
			/// </summary>
			BadArg = -1,
			/// <summary>
			/// The mode struct passed is invalid.
			/// </summary>
			BufferToSmall = -2,
			/// <summary>
			/// An internal error was detected.
			/// </summary>
			InternalError = -3,
			/// <summary>
			/// The compressed data passed is corrupted.
			/// </summary>
			InvalidPacket = -4,
			/// <summary>
			/// Invalid/unsupported request number.
			/// </summary>
			Unimplemented = -5,
			/// <summary>
			/// An encoder or decoder structure is invalid or already freed.
			/// </summary>
			InvalidState = -6,
			/// <summary>
			/// Memory allocation has failed.
			/// </summary>
			AllocFail = -7
		}

		public IntPtr Device;

		private byte[] TempBuffer;

		protected OpusDevice()
		{
			Device = IntPtr.Zero;
		}

		protected abstract void DisposeDevice(IntPtr device);

		protected void CheckError(int error, string message)
		{
			if (error != 0)
			{
				throw new Exception($"Error #{error} {message}");
			}
		}

		protected byte[] GetTempBuffer(int minSize)
		{
			byte[] tempBuffer = TempBuffer;
			int num = ((tempBuffer != null) ? tempBuffer.Length : 0);
			if (num < minSize)
			{
				int val = Math.Max((int)((float)num * 1.5f), minSize);
				val = Math.Max(val, 1024);
				Array.Resize(ref TempBuffer, val);
			}
			return TempBuffer;
		}

		public void Dispose()
		{
			if (Device != IntPtr.Zero)
			{
				DisposeDevice(Device);
				Device = IntPtr.Zero;
			}
		}
	}
}
