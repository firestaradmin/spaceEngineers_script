using System;
using Sandbox.Game.World;
using VRage.Data.Audio;
using VRage.Library.Utils;

namespace Sandbox.Game.VoiceChat
{
	public interface IMyVoiceChatLogic : IDisposable
	{
		bool ShouldSendData(byte[] data, int dataSize, Span<byte> format, out int bytesToRemember);

		bool ShouldSendVoice(MyPlayer sender, MyPlayer receiver);

		bool ShouldPlayVoice(MyPlayer player, MyTimeSpan timestamp, MyTimeSpan lastPlaybackSubmission, out MySoundDimensions dimension, out float maxDistance);

		void Compress(Span<byte> data, Span<byte> formatBytes, out int consumedBytes, out byte[] packet, out int packetSize);

		Span<byte> Decompress(Span<byte> packet, long sender);
	}
}
