using System;
using System.Collections.Generic;
using System.Linq;
using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Audio;
using VRage.Data.Audio;
using VRage.Game.Components;
using VRage.GameServices;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.VoiceChat
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	[StaticEventOwner]
	public class MyVoiceChatSessionComponent : MySessionComponentBase
	{
		private class SendBuffer : IBitSerializable
		{
			public byte[] VoiceDataBuffer;

			public int NumElements;

			public long SenderUserId;

			public bool Serialize(BitStream stream, bool validate, bool acceptAndSetValue = true)
			{
				if (stream.Reading)
				{
					SenderUserId = stream.ReadInt64();
					NumElements = stream.ReadInt32();
					ArrayExtensions.EnsureCapacity(ref VoiceDataBuffer, NumElements);
					stream.ReadBytes(VoiceDataBuffer, 0, NumElements);
				}
				else
				{
					stream.WriteInt64(SenderUserId);
					stream.WriteInt32(NumElements);
					stream.WriteBytes(VoiceDataBuffer, 0, NumElements);
				}
				return true;
			}

			public static implicit operator BitReaderWriter(SendBuffer buffer)
			{
				return new BitReaderWriter(buffer);
			}
		}

		private struct ReceivedData
		{
			public MyList<byte> UncompressedBuffer;

			public MyTimeSpan ReceivedDataTimestamp;

			public MyTimeSpan SpeakerTimestamp;

			public MyTimeSpan LastPlaybackSubmissionTimestamp;

			public byte[] GetDataForPlayback()
			{
				byte[] result = UncompressedBuffer.ToArray();
				UncompressedBuffer.Clear();
				ReceivedDataTimestamp = MyTimeSpan.Zero;
				LastPlaybackSubmissionTimestamp = MySandboxGame.Static.TotalTime;
				return result;
			}

			public void ClearSpeakerTimestamp()
			{
				SpeakerTimestamp = MyTimeSpan.Zero;
			}
		}

		protected sealed class SendVoice_003C_003EVRage_Library_Collections_BitReaderWriter : ICallSite<IMyEventOwner, BitReaderWriter, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in BitReaderWriter data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendVoice(data);
			}
		}

		protected sealed class SendVoicePlayer_003C_003ESystem_UInt64_0023VRage_Library_Collections_BitReaderWriter : ICallSite<IMyEventOwner, ulong, BitReaderWriter, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong user, in BitReaderWriter data, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendVoicePlayer(user, data);
			}
		}

		private bool m_recording;

		private Dictionary<ulong, MyEntity3DSoundEmitter> m_voices;

		private Dictionary<ulong, ReceivedData> m_receivedVoiceData;

		private List<ulong> m_keys;

		private IMyVoiceChatLogic m_voiceChatLogic;

		private bool m_enabled;

		private const uint UNCOMPRESSED_SIZE = 22528u;

		private Dictionary<ulong, bool> m_debugSentVoice = new Dictionary<ulong, bool>();

		private Dictionary<ulong, MyTuple<int, TimeSpan>> m_debugReceivedVoice = new Dictionary<ulong, MyTuple<int, TimeSpan>>();

		private static SendBuffer Recievebuffer = new SendBuffer();

		private Task m_dataRecordingTask;

		private MyVoiceResult m_lastRecordingResult = MyVoiceResult.NoData;

		private byte[] m_voiceDataBufferCache;

		private Action m_recordVoiceDataFunc;

		private Queue<byte> m_rememberedVoiceBytes = new Queue<byte>(22528);

		private int m_inactiveRecordingFrames;

		private static List<byte[]> RecievedBuffers = new List<byte[]>();

		public static MyVoiceChatSessionComponent Static { get; private set; }

		public bool IsRecording => m_recording;

		public override bool IsRequiredByGame => MyPerGameSettings.VoiceChatEnabled;

		public event Action<ulong, bool> OnPlayerMutedStateChanged;

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
			MyGameService.InitializeVoiceRecording();
			m_voiceChatLogic = Activator.CreateInstance(MyPerGameSettings.VoiceChatLogic) as IMyVoiceChatLogic;
			m_recording = false;
			m_voiceDataBufferCache = new byte[22528];
			m_voices = new Dictionary<ulong, MyEntity3DSoundEmitter>();
			m_receivedVoiceData = new Dictionary<ulong, ReceivedData>();
			m_keys = new List<ulong>();
			Sync.Players.PlayerRemoved += Players_PlayerRemoved;
			Sync.Players.PlayersChanged += OnOnlinePlayersChanged;
			m_enabled = MyAudio.Static.EnableVoiceChat;
			MyAudio.Static.VoiceChatEnabled += MyAudio_VoiceChatEnabled;
			MyHud.VoiceChat.VisibilityChanged += VoiceChat_VisibilityChanged;
			MyGameService.OnOverlayActivated += OnOverlayActivated;
			ICollection<MyPlayer> onlinePlayers = Sync.Players.GetOnlinePlayers();
			foreach (MyPlayer item in onlinePlayers)
			{
				ulong steamId = item.Id.SteamId;
				if (MySandboxGame.Config.MutedPlayers.Contains(steamId))
				{
					MyGameService.SetPlayerMuted(steamId, muted: true);
				}
			}
			if (onlinePlayers.Count > 0)
			{
				MyGameService.UpdateMutedPlayersFromCloud();
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			if (m_recording)
			{
				StopRecording();
			}
<<<<<<< HEAD
			ulong[] array = m_voices.Keys.ToArray();
=======
			ulong[] array = Enumerable.ToArray<ulong>((IEnumerable<ulong>)m_voices.Keys);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (ulong playerId in array)
			{
				DisposePlayerEmitter(playerId);
			}
			m_voiceChatLogic?.Dispose();
			m_voiceChatLogic = null;
			MyGameService.DisposeVoiceRecording();
			Static = null;
			m_receivedVoiceData = null;
			m_voices = null;
			m_keys = null;
			Sync.Players.PlayerRemoved -= Players_PlayerRemoved;
			Sync.Players.PlayersChanged -= OnOnlinePlayersChanged;
			MyAudio.Static.VoiceChatEnabled -= MyAudio_VoiceChatEnabled;
			MyHud.VoiceChat.VisibilityChanged -= VoiceChat_VisibilityChanged;
			MyGameService.OnOverlayActivated -= OnOverlayActivated;
		}

		private void OnOverlayActivated(bool enabled)
		{
			if (enabled)
			{
				return;
			}
			MyGameService.UpdateMutedPlayersFromCloud(delegate
			{
				if (base.Loaded && MySession.Static != null)
				{
					foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
					{
						if (!onlinePlayer.IsLocalPlayer)
						{
							ulong steamId = onlinePlayer.Id.SteamId;
							bool muted = MyGameService.IsPlayerMutedInCloud(steamId);
							SetPlayerMuted(steamId, muted);
						}
					}
				}
			});
		}

		private void OnOnlinePlayersChanged(bool connected, MyPlayer.PlayerId player)
		{
			ulong steamId = player.SteamId;
			if (connected && MySandboxGame.Config.MutedPlayers.Contains(steamId))
			{
				MyGameService.SetPlayerMuted(steamId, muted: true);
			}
		}

		private void Players_PlayerRemoved(MyPlayer.PlayerId pid)
		{
			if (pid.SerialId == 0)
			{
				ulong steamId = pid.SteamId;
				if (m_receivedVoiceData.ContainsKey(steamId))
				{
					m_receivedVoiceData.Remove(steamId);
				}
				DisposePlayerEmitter(steamId);
<<<<<<< HEAD
			}
		}

		private void DisposePlayerEmitter(ulong playerId)
		{
			if (m_voices.TryGetValue(playerId, out var value))
			{
				value.StopSound(forced: true, cleanUp: true, cleanupSound: true);
				m_voices.Remove(playerId);
			}
		}

=======
			}
		}

		private void DisposePlayerEmitter(ulong playerId)
		{
			if (m_voices.TryGetValue(playerId, out var value))
			{
				value.StopSound(forced: true, cleanUp: true, cleanupSound: true);
				m_voices.Remove(playerId);
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void MyAudio_VoiceChatEnabled(bool isEnabled)
		{
			m_enabled = isEnabled;
			if (m_enabled)
			{
				if (MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION && !m_recording)
				{
					StartRecording();
				}
				return;
			}
			if (m_recording)
			{
				m_recording = false;
				StopRecording();
			}
<<<<<<< HEAD
			ulong[] array = m_voices.Keys.ToArray();
=======
			ulong[] array = Enumerable.ToArray<ulong>((IEnumerable<ulong>)m_voices.Keys);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (ulong playerId in array)
			{
				DisposePlayerEmitter(playerId);
			}
			m_voices.Clear();
			m_receivedVoiceData.Clear();
		}

		private void VoiceChat_VisibilityChanged(bool isVisible)
		{
			if (!MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION && m_recording != isVisible)
			{
				if (m_recording)
				{
					m_recording = false;
					StopRecording();
				}
				else
				{
					StartRecording();
				}
			}
		}

		public void StartRecording()
		{
			if (m_enabled)
			{
				m_recording = true;
				MyGameService.StartVoiceRecording();
				if (!MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION)
				{
					MyHud.VoiceChat.Show();
				}
			}
		}

		public void StopRecording()
		{
			if (m_enabled)
			{
				MyGameService.StopVoiceRecording();
				MyHud.VoiceChat.Hide();
				DummyUpdate();
			}
		}

		public void ClearDebugData()
		{
			m_debugSentVoice.Clear();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (m_enabled && IsCharacterValid(MySession.Static.LocalCharacter))
			{
				if (m_recording)
				{
					UpdateRecording();
				}
				else if (MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION)
				{
					StartRecording();
				}
				UpdatePlayback();
			}
		}

		private static bool IsCharacterValid(MyCharacter character)
		{
			if (character != null && !character.IsDead)
			{
				return !character.MarkedForClose;
			}
			return false;
		}

		private void VoiceMessageReceived(SendBuffer data)
		{
			if (!m_enabled)
			{
				return;
			}
			MyMultiplayerBase @static = MyMultiplayer.Static;
			if (@static != null && @static.IsVoiceChatAvailable && IsCharacterValid(MySession.Static.LocalCharacter))
<<<<<<< HEAD
			{
				ulong senderUserId = (ulong)data.SenderUserId;
				if (MyGameService.Networking.Chat.IsVoiceChatAvailableForUserId(senderUserId, MyMultiplayer.Static.IsCrossMember(senderUserId)) && MyGameService.GetPlayerMutedState(senderUserId) != MyPlayerChatState.Muted)
				{
					ProcessBuffer(data.VoiceDataBuffer.Span(0, data.NumElements), senderUserId);
				}
			}
		}

		private void PlayVoice(byte[] uncompressedBuffer, ulong playerId, MySoundDimensions dimension, float maxDistance)
		{
			if (uncompressedBuffer.Length == 0)
			{
				return;
			}
			MyCharacter character;
			if (m_voices.TryGetValue(playerId, out var value) && (character = value.Entity as MyCharacter) != null && !IsCharacterValid(character))
			{
				DisposePlayerEmitter(playerId);
			}
			if (!m_voices.TryGetValue(playerId, out value))
			{
				MyCharacter character2 = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(playerId)).Character;
				if (!IsCharacterValid(character2))
				{
					return;
				}
				value = new MyEntity3DSoundEmitter(character2);
				m_voices[playerId] = value;
=======
			{
				ulong senderUserId = (ulong)data.SenderUserId;
				if (MyGameService.Networking.Chat.IsVoiceChatAvailableForUserId(senderUserId, MyMultiplayer.Static.IsCrossMember(senderUserId)) && MyGameService.GetPlayerMutedState(senderUserId) != MyPlayerChatState.Muted)
				{
					ProcessBuffer(data.VoiceDataBuffer.Span(0, data.NumElements), senderUserId);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_voices[playerId].PlaySound(uncompressedBuffer, MyAudio.Static.VolumeVoiceChat, maxDistance, dimension);
		}

<<<<<<< HEAD
		private void ProcessBuffer(Span<byte> compressedData, ulong sender)
		{
			if (!m_receivedVoiceData.TryGetValue(sender, out var value))
			{
				ReceivedData receivedData = default(ReceivedData);
				receivedData.UncompressedBuffer = new MyList<byte>();
				receivedData.ReceivedDataTimestamp = MyTimeSpan.Zero;
				receivedData.SpeakerTimestamp = MyTimeSpan.Zero;
				value = receivedData;
			}
			try
			{
				Span<byte> data = m_voiceChatLogic.Decompress(compressedData, (long)sender);
				value.UncompressedBuffer.Insert(data);
			}
			catch
			{
			}
			MyTimeSpan totalTime = MySandboxGame.Static.TotalTime;
			if (value.ReceivedDataTimestamp == MyTimeSpan.Zero)
			{
				value.ReceivedDataTimestamp = totalTime;
			}
			value.SpeakerTimestamp = totalTime;
			m_receivedVoiceData[sender] = value;
		}

		private void UpdatePlayback()
		{
			if (m_voiceChatLogic == null)
			{
=======
		private void PlayVoice(byte[] uncompressedBuffer, ulong playerId, MySoundDimensions dimension, float maxDistance)
		{
			if (uncompressedBuffer.Length == 0)
			{
				return;
			}
			MyCharacter character;
			if (m_voices.TryGetValue(playerId, out var value) && (character = value.Entity as MyCharacter) != null && !IsCharacterValid(character))
			{
				DisposePlayerEmitter(playerId);
			}
			if (!m_voices.TryGetValue(playerId, out value))
			{
				MyCharacter character2 = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(playerId)).Character;
				if (!IsCharacterValid(character2))
				{
					return;
				}
				value = new MyEntity3DSoundEmitter(character2);
				m_voices[playerId] = value;
			}
			m_voices[playerId].PlaySound(uncompressedBuffer, MyAudio.Static.VolumeVoiceChat, maxDistance, dimension);
		}

		private void ProcessBuffer(Span<byte> compressedData, ulong sender)
		{
			if (!m_receivedVoiceData.TryGetValue(sender, out var value))
			{
				ReceivedData receivedData = default(ReceivedData);
				receivedData.UncompressedBuffer = new MyList<byte>();
				receivedData.ReceivedDataTimestamp = MyTimeSpan.Zero;
				receivedData.SpeakerTimestamp = MyTimeSpan.Zero;
				value = receivedData;
			}
			try
			{
				Span<byte> data = m_voiceChatLogic.Decompress(compressedData, (long)sender);
				value.UncompressedBuffer.Insert(data);
			}
			catch
			{
			}
			MyTimeSpan totalTime = MySandboxGame.Static.TotalTime;
			if (value.ReceivedDataTimestamp == MyTimeSpan.Zero)
			{
				value.ReceivedDataTimestamp = totalTime;
			}
			value.SpeakerTimestamp = totalTime;
			m_receivedVoiceData[sender] = value;
		}

		private void UpdatePlayback()
		{
			if (m_voiceChatLogic == null)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return;
			}
			MyTimeSpan totalTime = MySandboxGame.Static.TotalTime;
			float num = 1000f;
			m_keys.AddRange(m_receivedVoiceData.Keys);
			foreach (ulong key in m_keys)
			{
				bool flag = false;
				ReceivedData value = m_receivedVoiceData[key];
				if (value.ReceivedDataTimestamp != MyTimeSpan.Zero)
<<<<<<< HEAD
				{
					MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(key));
					if (playerById != null && m_voiceChatLogic.ShouldPlayVoice(playerById, value.ReceivedDataTimestamp, value.LastPlaybackSubmissionTimestamp, out var dimension, out var maxDistance) && MyGameService.GetPlayerMutedState(playerById.Id.SteamId) != MyPlayerChatState.Muted)
					{
						PlayVoice(value.GetDataForPlayback(), key, dimension, maxDistance);
						flag = true;
					}
				}
				if (value.SpeakerTimestamp != MyTimeSpan.Zero && (totalTime - value.SpeakerTimestamp).Milliseconds > (double)num)
				{
					value.ClearSpeakerTimestamp();
					flag = true;
				}
				if (flag)
				{
=======
				{
					MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(key));
					if (playerById != null && m_voiceChatLogic.ShouldPlayVoice(playerById, value.ReceivedDataTimestamp, value.LastPlaybackSubmissionTimestamp, out var dimension, out var maxDistance) && MyGameService.GetPlayerMutedState(playerById.Id.SteamId) != MyPlayerChatState.Muted)
					{
						PlayVoice(value.GetDataForPlayback(), key, dimension, maxDistance);
						flag = true;
					}
				}
				if (value.SpeakerTimestamp != MyTimeSpan.Zero && (totalTime - value.SpeakerTimestamp).Milliseconds > (double)num)
				{
					value.ClearSpeakerTimestamp();
					flag = true;
				}
				if (flag)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_receivedVoiceData[key] = value;
				}
			}
			m_keys.Clear();
		}

		private void RecordVoiceDataParallel()
		{
			uint size2;
			MyVoiceResult myVoiceResult = MyGameService.GetAvailableVoice(ref m_voiceDataBufferCache, out size2);
			int size = (int)size2;
			byte[] array = null;
			if (myVoiceResult == MyVoiceResult.OK)
			{
				array = MyGameService.GetVoiceDataFormat();
				if (MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION)
				{
					if (m_voiceChatLogic.ShouldSendData(m_voiceDataBufferCache, size, array, out var bytesToRemember))
					{
						TrimOldData(bytesToRemember);
					}
					else
					{
						myVoiceResult = MyVoiceResult.NoData;
						TrimOldData(bytesToRemember);
						SaveOldData(0);
<<<<<<< HEAD
					}
				}
			}
			if (myVoiceResult == MyVoiceResult.OK)
			{
				ConsumeOldData();
				SendBuffer sendBuffer = new SendBuffer
				{
					SenderUserId = (long)MySession.Static.LocalHumanPlayer.Id.SteamId
				};
				int num = 0;
				bool isServer = Sync.IsServer;
				while (true)
				{
					m_voiceChatLogic.Compress(m_voiceDataBufferCache.Span(num, size - num), array, out var consumedBytes, out var packet, out var packetSize);
					if (consumedBytes == 0)
					{
						break;
					}
					num += consumedBytes;
					sendBuffer.NumElements = packetSize;
					sendBuffer.VoiceDataBuffer = packet;
					if (packetSize <= 0)
					{
						continue;
					}
					if (MyFakes.VOICE_CHAT_ECHO)
					{
						RecievedBuffers.Add(packet.Span(0, packetSize).ToArray());
					}
=======
					}
				}
			}
			if (myVoiceResult == MyVoiceResult.OK)
			{
				ConsumeOldData();
				SendBuffer sendBuffer = new SendBuffer
				{
					SenderUserId = (long)MySession.Static.LocalHumanPlayer.Id.SteamId
				};
				int num = 0;
				bool isServer = Sync.IsServer;
				while (true)
				{
					m_voiceChatLogic.Compress(m_voiceDataBufferCache.Span(num, size - num), array, out var consumedBytes, out var packet, out var packetSize);
					if (consumedBytes == 0)
					{
						break;
					}
					num += consumedBytes;
					sendBuffer.NumElements = packetSize;
					sendBuffer.VoiceDataBuffer = packet;
					if (packetSize <= 0)
					{
						continue;
					}
					if (MyFakes.VOICE_CHAT_ECHO)
					{
						RecievedBuffers.Add(packet.Span(0, packetSize).ToArray());
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (isServer)
					{
						SendVoice(sendBuffer);
						continue;
					}
					MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action<BitReaderWriter>>)((IMyEventOwner x) => SendVoice), (BitReaderWriter)sendBuffer, default(EndpointId), (Vector3D?)null);
				}
				SaveOldData(num);
			}
			m_lastRecordingResult = myVoiceResult;
			void ConsumeOldData()
			{
<<<<<<< HEAD
				int count = m_rememberedVoiceBytes.Count;
=======
				int count = m_rememberedVoiceBytes.get_Count();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (count > 0)
				{
					int num2 = size + count;
					ArrayExtensions.EnsureCapacity(ref m_voiceDataBufferCache, num2);
					Array.Copy(m_voiceDataBufferCache, 0, m_voiceDataBufferCache, count, size);
					m_rememberedVoiceBytes.CopyTo(m_voiceDataBufferCache, 0);
					m_rememberedVoiceBytes.Clear();
					size = num2;
				}
			}
			void SaveOldData(int i)
			{
				byte[] voiceDataBufferCache = m_voiceDataBufferCache;
				while (i < size)
				{
					m_rememberedVoiceBytes.Enqueue(voiceDataBufferCache[i]);
					i++;
				}
			}
			void TrimOldData(int maxBytesToRemember)
			{
				int num3 = Math.Max(0, maxBytesToRemember - size);
<<<<<<< HEAD
				while (m_rememberedVoiceBytes.Count > num3)
=======
				while (m_rememberedVoiceBytes.get_Count() > num3)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					m_rememberedVoiceBytes.Dequeue();
				}
			}
		}

		private void UpdateRecording()
		{
			if (Sync.IsDedicated)
			{
				return;
			}
			if (m_recordVoiceDataFunc == null)
			{
				m_recordVoiceDataFunc = RecordVoiceDataParallel;
			}
			else
			{
				m_dataRecordingTask.WaitOrExecute();
			}
			m_dataRecordingTask = Parallel.Start(m_recordVoiceDataFunc);
			MyVoiceResult lastRecordingResult = m_lastRecordingResult;
			if (lastRecordingResult == MyVoiceResult.OK)
			{
				if (MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION)
				{
					m_inactiveRecordingFrames = 0;
					if (!MyHud.VoiceChat.Visible)
					{
						MyHud.VoiceChat.Show();
					}
				}
				return;
			}
			if (MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION && MyHud.VoiceChat.Visible)
			{
				m_inactiveRecordingFrames++;
				if ((float)m_inactiveRecordingFrames > 30f)
				{
					MyHud.VoiceChat.Hide();
				}
			}
			if (lastRecordingResult == MyVoiceResult.NotRecording)
			{
				m_recording = false;
			}
			else
			{
				_ = 3;
			}
		}

		public void SetPlayerMuted(ulong playerId, bool muted)
		{
			HashSet<ulong> mutedPlayers = MySandboxGame.Config.MutedPlayers;
			if ((!muted) ? mutedPlayers.Remove(playerId) : mutedPlayers.Add(playerId))
			{
				MySandboxGame.Config.MutedPlayers = mutedPlayers;
				MySandboxGame.Config.Save();
				MyGameService.SetPlayerMuted(playerId, muted);
				this.OnPlayerMutedStateChanged.InvokeIfNotNull(playerId, muted);
			}
		}

<<<<<<< HEAD
		[Event(null, 711)]
=======
		[Event(null, 712)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Server]
		private static void SendVoice(BitReaderWriter data)
		{
			if (Static != null && data.ReadData(Recievebuffer, validate: false))
			{
				SendVoice(Recievebuffer);
			}
		}

		private static void SendVoice(SendBuffer receiveBuffer)
		{
			ulong senderUserId = (ulong)receiveBuffer.SenderUserId;
			MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(senderUserId));
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Id.SerialId == 0 && onlinePlayer.Id.SteamId != senderUserId && IsCharacterValid(onlinePlayer.Character) && Static.m_voiceChatLogic.ShouldSendVoice(playerById, onlinePlayer))
				{
					MyMultiplayer.RaiseStaticEvent((Func<IMyEventOwner, Action<ulong, BitReaderWriter>>)((IMyEventOwner x) => SendVoicePlayer), onlinePlayer.Id.SteamId, (BitReaderWriter)receiveBuffer, new EndpointId(onlinePlayer.Id.SteamId), (Vector3D?)null);
					if (MyFakes.ENABLE_VOICE_CHAT_DEBUGGING)
					{
						Static.m_debugSentVoice[onlinePlayer.Id.SteamId] = true;
					}
				}
				else if (MyFakes.ENABLE_VOICE_CHAT_DEBUGGING)
				{
					Static.m_debugSentVoice[onlinePlayer.Id.SteamId] = false;
				}
			}
		}

		private void DummyUpdate()
		{
			foreach (byte[] recievedBuffer in RecievedBuffers)
			{
				ProcessBuffer(recievedBuffer, MySession.Static.LocalHumanPlayer.Id.SteamId);
			}
		}

<<<<<<< HEAD
		[Event(null, 757)]
=======
		[Event(null, 758)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Client]
		private static void SendVoicePlayer(ulong user, BitReaderWriter data)
		{
			data.ReadData(Recievebuffer, validate: false);
			Static.VoiceMessageReceived(Recievebuffer);
		}

		public override void Draw()
		{
			base.Draw();
			if (m_receivedVoiceData == null)
			{
				return;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_VOICE_CHAT && MyFakes.ENABLE_VOICE_CHAT_DEBUGGING)
			{
				DebugDraw();
			}
			BoundingSphereD boundingSphereD = new BoundingSphereD(MySector.MainCamera.Position, 500.0);
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Character == null || onlinePlayer.IsLocalPlayer)
				{
					continue;
				}
				MyPositionComponentBase positionComp = onlinePlayer.Character.PositionComp;
				MatrixD worldMatrixRef = positionComp.WorldMatrixRef;
				if (boundingSphereD.Contains(worldMatrixRef.Translation) == ContainmentType.Disjoint)
				{
					continue;
				}
				ulong steamId = onlinePlayer.Id.SteamId;
				MyPlayerChatState playerMutedState = MyGameService.GetPlayerMutedState(steamId);
				if (playerMutedState != MyPlayerChatState.Muted && (playerMutedState == MyPlayerChatState.Talking || (m_receivedVoiceData.TryGetValue(steamId, out var value) && value.SpeakerTimestamp != MyTimeSpan.Zero)))
				{
					Vector3D position = worldMatrixRef.Translation + positionComp.LocalAABB.Height * worldMatrixRef.Up + worldMatrixRef.Up * 0.20000000298023224;
					MyGuiPaddedTexture tEXTURE_VOICE_CHAT = MyGuiConstants.TEXTURE_VOICE_CHAT;
					MatrixD matrix = MySector.MainCamera.ViewMatrix * MySector.MainCamera.ProjectionMatrix;
					Vector3D vector3D = Vector3D.Transform(position, matrix);
					if (vector3D.Z < 1.0)
					{
						Vector2 hudPos = new Vector2((float)vector3D.X, (float)vector3D.Y);
						hudPos = hudPos * 0.5f + 0.5f * Vector2.One;
						hudPos.Y = 1f - hudPos.Y;
						MyGuiManager.DrawSpriteBatch(tEXTURE_VOICE_CHAT.Texture, MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos), tEXTURE_VOICE_CHAT.SizeGui * 0.5f, Color.White, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM);
					}
				}
			}
		}

		private void DebugDraw()
		{
			Vector2 screenCoord = new Vector2(300f, 100f);
			MyRenderProxy.DebugDrawText2D(screenCoord, "Sent voice to:", Color.White, 1f);
			screenCoord.Y += 30f;
			foreach (KeyValuePair<ulong, bool> item in m_debugSentVoice)
			{
				string text = string.Format("id: {0} => {1}", item.Key, item.Value ? "SENT" : "NOT");
				MyRenderProxy.DebugDrawText2D(screenCoord, text, Color.White, 1f);
				screenCoord.Y += 30f;
			}
			MyRenderProxy.DebugDrawText2D(screenCoord, "Received voice from:", Color.White, 1f);
			screenCoord.Y += 30f;
			foreach (KeyValuePair<ulong, MyTuple<int, TimeSpan>> item2 in m_debugReceivedVoice)
			{
				string text2 = $"id: {item2.Key} => size: {item2.Value.Item1} (timestamp {item2.Value.Item2.ToString()})";
				MyRenderProxy.DebugDrawText2D(screenCoord, text2, Color.White, 1f);
				screenCoord.Y += 30f;
			}
			MyRenderProxy.DebugDrawText2D(screenCoord, "Uncompressed buffers:", Color.White, 1f);
			screenCoord.Y += 30f;
			foreach (KeyValuePair<ulong, ReceivedData> receivedVoiceDatum in m_receivedVoiceData)
			{
				string text3 = $"id: {receivedVoiceDatum.Key} => size: {receivedVoiceDatum.Value.UncompressedBuffer.Count}";
				MyRenderProxy.DebugDrawText2D(screenCoord, text3, Color.White, 1f);
				screenCoord.Y += 30f;
			}
		}
	}
}
