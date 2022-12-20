using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.GameSystems;
using Sandbox.Game.VoiceChat;
using Sandbox.Game.World;
using VRage.Data.Audio;
using VRage.Library.Utils;
using VRageMath;

namespace SpaceEngineers.Game.VoiceChat
{
	public class MyVoiceChatLogic : IMyVoiceChatLogic, IDisposable
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private readonly struct WAVEFORMATEX
		{
			public readonly ushort FormatTag;

			public readonly ushort Channels;

			public readonly uint SamplesPerSec;

			public readonly uint AvgBytesPerSec;

			public readonly ushort BlockAlign;

			public readonly ushort BitsPerSample;

			public readonly ushort Size;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct WAVEFORMATEXTENSIBLE
		{
			public ushort ValidBitsPerSample;

			public uint ChannelMask;

			public Guid SubFormat;
		}

		private MyTimeSpan LastSentData = MyTimeSpan.Zero;

		private OpusEncoder m_encoder;

		private readonly Dictionary<long, OpusDecoder> m_decoders = new Dictionary<long, OpusDecoder>();

		public bool ShouldSendVoice(MyPlayer sender, MyPlayer receiver)
		{
			return MyAntennaSystem.Static.CheckConnection(sender.Identity, receiver.Identity);
		}

		public bool ShouldPlayVoice(MyPlayer player, MyTimeSpan timestamp, MyTimeSpan lastPlaybackSubmission, out MySoundDimensions dimension, out float maxDistance)
		{
			MyTimeSpan totalTime = MySandboxGame.Static.TotalTime;
			double num = MyFakes.VOICE_CHAT_PLAYBACK_DELAY;
			double num2 = Math.Max(500.0, num * 3.0);
			if ((totalTime - timestamp).Milliseconds > num || (totalTime - lastPlaybackSubmission).Milliseconds < num2)
			{
				dimension = (MyPlatformGameSettings.VOICE_CHAT_3D_SOUND ? MySoundDimensions.D3 : MySoundDimensions.D2);
				maxDistance = float.MaxValue;
				return true;
			}
			dimension = MySoundDimensions.D2;
			maxDistance = 0f;
			return false;
		}

		public bool ShouldSendData(byte[] data, int dataSize, Span<byte> format, out int bytesToRemember)
		{
			MyMultiplayerBase @static = MyMultiplayer.Static;
			if (@static == null || !@static.IsSomeoneElseConnected)
			{
				bytesToRemember = 0;
				return false;
			}
			if (MyGameService.IsMicrophoneFilteringSilence())
			{
				bytesToRemember = 0;
				return true;
			}
			return ShouldSendDataLogic(data, dataSize, format, out bytesToRemember);
		}

		public bool ShouldSendDataLogic(byte[] data, int dataSize, Span<byte> format, out int bytesToRemember)
		{
			float num = ComputeRMS(data, dataSize, format, 0.75f, out bytesToRemember);
			if (num < 0f)
			{
				bool flag = true;
				for (int i = 0; i < dataSize; i++)
				{
					flag &= data[i] == 0;
				}
				num = ((!flag) ? 1 : 0);
			}
			MyTimeSpan totalTime = MySandboxGame.Static.TotalTime;
			double milliseconds = (totalTime - LastSentData).Milliseconds;
			float num2 = MyMath.Clamp(MyFakes.VOICE_CHAT_MIC_SENSITIVITY, 0f, 1f);
			float num3 = 0.1f - 0.05f * ((num2 - 0.5f) * 2f);
			bool flag2 = milliseconds < 1000.0;
			if (num > num3 || (flag2 && num > num3 * 0.85f))
			{
				LastSentData = totalTime;
				return true;
			}
			if (flag2)
			{
				return true;
			}
			return false;
		}

		private static bool TryReadFormat(Span<byte> formatBytes, out WAVEFORMATEX format, out bool isIEEE_Float)
		{
			int num = Unsafe.SizeOf<WAVEFORMATEX>();
			if (formatBytes.Length >= num)
			{
				format = Unsafe.ReadUnaligned<WAVEFORMATEX>(ref formatBytes[0]);
				if (format.FormatTag == 65534)
				{
					if (formatBytes.Length < num + Unsafe.SizeOf<WAVEFORMATEXTENSIBLE>())
					{
						goto IL_0081;
					}
					isIEEE_Float = Unsafe.ReadUnaligned<WAVEFORMATEXTENSIBLE>(ref formatBytes[num]).SubFormat == new Guid(3, 0, 16, 128, 0, 0, 170, 0, 56, 155, 113);
				}
				else
				{
					isIEEE_Float = false;
				}
				return true;
			}
			goto IL_0081;
			IL_0081:
			isIEEE_Float = false;
			format = default(WAVEFORMATEX);
			return false;
		}

		private static float ComputeRMS(byte[] data, int dataSize, Span<byte> formatBytes, float secondsToSave, out int bytesToSave)
		{
			if (TryReadFormat(formatBytes, out var format, out var isIEEE_Float) && isIEEE_Float && format.BitsPerSample == 32)
			{
				float num = 0f;
				for (int i = 0; i < dataSize; i += 4)
				{
					float num2 = Unsafe.ReadUnaligned<float>(ref data[i]);
					num += num2 * num2;
				}
				int num3 = (int)(secondsToSave * (float)format.SamplesPerSec);
				bytesToSave = 4 * num3;
				return (float)Math.Sqrt(num / (float)(dataSize / 4));
			}
			bytesToSave = 0;
			return -1f;
		}

		public void Compress(Span<byte> data, Span<byte> formatBytes, out int consumedBytes, out byte[] packet, out int packetSize)
		{
			if (!TryReadFormat(formatBytes, out var format, out var isIEEE_Float) || (!isIEEE_Float && format.BitsPerSample != 16))
			{
				throw new Exception($"Unsupported format {format.FormatTag};{format.BitsPerSample};{format.Channels};{format.Size}");
			}
			if (m_encoder == null || isIEEE_Float != (m_encoder.Format == OpusEncoder.FormatT.FLOAT) || m_encoder.TargetBitRate != MyFakes.VOICE_CHAT_TARGET_BIT_RATE)
			{
				m_encoder?.Dispose();
				m_encoder = new OpusEncoder((int)format.SamplesPerSec, isIEEE_Float ? OpusEncoder.FormatT.FLOAT : OpusEncoder.FormatT.SHORT, (MyFakes.VOICE_CHAT_TARGET_BIT_RATE == 0) ? null : new int?(MyFakes.VOICE_CHAT_TARGET_BIT_RATE), 1000);
				MyFakes.VOICE_CHAT_TARGET_BIT_RATE = m_encoder.TargetBitRate;
			}
			m_encoder.Encode(data, out consumedBytes, out packet, out packetSize);
		}

		public Span<byte> Decompress(Span<byte> packet, long sender)
		{
			if (!m_decoders.TryGetValue(sender, out var value))
			{
				value = new OpusDecoder(24000);
				m_decoders.Add(sender, value);
			}
			return value.Decode(packet);
		}

		public void Dispose()
		{
			if (m_encoder != null)
			{
				m_encoder.Dispose();
				m_encoder = null;
			}
			foreach (KeyValuePair<long, OpusDecoder> decoder in m_decoders)
			{
				LinqExtensions.Deconstruct(decoder, out var _, out var v);
				v.Dispose();
			}
			m_decoders.Clear();
		}
	}
}
