using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.XAudio2;
using SharpDX.XAudio2.Fx;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.Utils;

namespace VRage.Audio
{
	public class MyCueBank : IDisposable
	{
		public enum CuePart
		{
			Start,
			Loop,
			End
		}

		private static MyStringId MUSIC_CATEGORY = MyStringId.GetOrCompute("Music");

		private XAudio2 m_audioEngine;

		public Dictionary<MyCueId, MySoundData> m_cues;

		private MyWaveBank m_waveBank;

		private VoiceSendDescriptor[] m_hudVoiceDescriptors;

		private VoiceSendDescriptor[] m_gameVoiceDescriptors;

		private VoiceSendDescriptor[] m_musicVoiceDescriptors;

		private Dictionary<MyWaveFormat, MySourceVoicePool> m_voiceHudPools;

		private Dictionary<MyWaveFormat, MySourceVoicePool> m_voiceSoundPools;

		private Dictionary<MyWaveFormat, MySourceVoicePool> m_voiceMusicPools;

		private Dictionary<MyStringId, Dictionary<MyStringId, MyCueId>> m_musicTransitionCues;

		private Dictionary<MyStringId, List<MyCueId>> m_musicTracks;

		private List<MyStringId> m_categories;

		private Reverb m_reverb;

		public bool UseSameSoundLimiter { get; set; }

		public bool DisablePooling { get; set; }

		public bool CacheLoaded { get; set; }

		public Dictionary<MyCueId, MySoundData>.ValueCollection CueDefinitions => m_cues.Values;

		public bool ApplyReverb { get; set; }

		public MyCueBank(XAudio2 audioEngine, ListReader<MySoundData> cues, VoiceSendDescriptor[] gameDesc, VoiceSendDescriptor[] hudDesc, VoiceSendDescriptor[] musicDesc, bool cacheLoaded)
		{
			ApplyReverb = false;
			CacheLoaded = cacheLoaded;
			m_audioEngine = audioEngine;
			if (cues.Count > 0)
			{
				m_cues = new Dictionary<MyCueId, MySoundData>(cues.Count, MyCueId.Comparer);
				InitTransitionCues();
				InitCues(cues);
				InitCategories();
				InitWaveBank();
				InitVoicePools(gameDesc, hudDesc, musicDesc);
				m_reverb = new Reverb(audioEngine);
			}
		}

		public void SetAudioEngine(XAudio2 audioEngine)
		{
			m_audioEngine = audioEngine;
			foreach (MySourceVoicePool value in m_voiceHudPools.Values)
			{
				value.SetAudioEngine(audioEngine);
			}
			foreach (MySourceVoicePool value2 in m_voiceSoundPools.Values)
			{
				value2.SetAudioEngine(audioEngine);
			}
			foreach (MySourceVoicePool value3 in m_voiceMusicPools.Values)
			{
				value3.SetAudioEngine(audioEngine);
			}
		}

		public void SetAudioEngine(XAudio2 audioEngine, VoiceSendDescriptor[] gameAudioVoiceDesc, VoiceSendDescriptor[] hudAudioVoiceDesc, VoiceSendDescriptor[] musicAudioVoiceDesc)
		{
			m_audioEngine = audioEngine;
			foreach (MySourceVoicePool value in m_voiceHudPools.Values)
			{
				value.SetAudioEngine(null);
				value.Dispose();
			}
			foreach (MySourceVoicePool value2 in m_voiceSoundPools.Values)
			{
				value2.SetAudioEngine(null);
				value2.Dispose();
			}
			foreach (MySourceVoicePool value3 in m_voiceMusicPools.Values)
			{
				value3.SetAudioEngine(null);
				value3.Dispose();
			}
			if (gameAudioVoiceDesc != null && hudAudioVoiceDesc != null)
			{
				InitVoicePools(gameAudioVoiceDesc, hudAudioVoiceDesc, musicAudioVoiceDesc);
			}
		}

		private void InitCues(ListReader<MySoundData> cues)
		{
			foreach (MySoundData item in cues)
			{
				MyCueId myCueId = new MyCueId(item.SubtypeId);
				m_cues[myCueId] = item;
				if (item.Category == MUSIC_CATEGORY)
				{
					AddMusicCue(item.MusicTrack.TransitionCategory, item.MusicTrack.MusicCategory, myCueId);
				}
			}
		}

		private void InitCategories()
		{
			m_categories = new List<MyStringId>();
			foreach (KeyValuePair<MyCueId, MySoundData> cue in m_cues)
			{
				if (!m_categories.Contains(cue.Value.Category))
				{
					m_categories.Add(cue.Value.Category);
				}
			}
		}

		private void InitWaveBank()
		{
			m_waveBank = new MyWaveBank();
			foreach (KeyValuePair<MyCueId, MySoundData> cue in m_cues)
			{
				if (cue.Value.Waves == null)
				{
					continue;
				}
				foreach (MyAudioWave wave in cue.Value.Waves)
				{
					m_waveBank.Add(cue.Value, wave, CacheLoaded);
				}
			}
		}

		private void InitVoicePools(VoiceSendDescriptor[] gameDesc, VoiceSendDescriptor[] hudDesc, VoiceSendDescriptor[] musicDesc)
		{
			m_hudVoiceDescriptors = hudDesc;
			m_gameVoiceDescriptors = gameDesc;
			m_musicVoiceDescriptors = musicDesc;
			m_voiceHudPools = new Dictionary<MyWaveFormat, MySourceVoicePool>();
			m_voiceSoundPools = new Dictionary<MyWaveFormat, MySourceVoicePool>();
			m_voiceMusicPools = new Dictionary<MyWaveFormat, MySourceVoicePool>();
		}

		private MySourceVoicePool GetVoicePool(MyVoicePoolType poolType, MyWaveFormat format)
		{
			Dictionary<MyWaveFormat, MySourceVoicePool> dictionary = null;
			VoiceSendDescriptor[] desc = null;
			switch (poolType)
			{
			case MyVoicePoolType.Hud:
				dictionary = m_voiceHudPools;
				desc = m_hudVoiceDescriptors;
				break;
			case MyVoicePoolType.Sound:
				dictionary = m_voiceSoundPools;
				desc = m_gameVoiceDescriptors;
				break;
			case MyVoicePoolType.Music:
				dictionary = m_voiceMusicPools;
				desc = m_musicVoiceDescriptors;
				break;
			}
			MySourceVoicePool value = null;
			lock (m_audioEngine)
			{
				if (dictionary != null)
				{
					if (!dictionary.TryGetValue(format, out value))
					{
						value = new MySourceVoicePool(m_audioEngine, format.WaveFormat, this, desc);
						value.UseSameSoundLimiter = UseSameSoundLimiter;
						dictionary.Add(format, value);
						return value;
					}
					return value;
				}
				return value;
			}
		}

		public void SetSameSoundLimiter()
		{
			if (m_voiceHudPools != null)
			{
				foreach (MySourceVoicePool value in m_voiceHudPools.Values)
				{
					value.UseSameSoundLimiter = UseSameSoundLimiter;
				}
			}
			if (m_voiceSoundPools != null)
			{
				foreach (MySourceVoicePool value2 in m_voiceSoundPools.Values)
				{
					value2.UseSameSoundLimiter = UseSameSoundLimiter;
				}
			}
			if (m_voiceMusicPools == null)
			{
				return;
			}
			foreach (MySourceVoicePool value3 in m_voiceMusicPools.Values)
			{
				value3.UseSameSoundLimiter = UseSameSoundLimiter;
			}
		}

		private void InitTransitionCues()
		{
			m_musicTransitionCues = new Dictionary<MyStringId, Dictionary<MyStringId, MyCueId>>(MyStringId.Comparer);
			m_musicTracks = new Dictionary<MyStringId, List<MyCueId>>(MyStringId.Comparer);
		}

		private void AddMusicCue(MyStringId musicTransition, MyStringId category, MyCueId cueId)
		{
			if (!m_musicTransitionCues.ContainsKey(musicTransition))
			{
				m_musicTransitionCues[musicTransition] = new Dictionary<MyStringId, MyCueId>(MyStringId.Comparer);
			}
			if (!m_musicTransitionCues[musicTransition].ContainsKey(category))
			{
				m_musicTransitionCues[musicTransition].Add(category, cueId);
			}
			if (!m_musicTracks.ContainsKey(category))
			{
				m_musicTracks.Add(category, new List<MyCueId>());
			}
			m_musicTracks[category].Add(cueId);
		}

		public Dictionary<MyStringId, List<MyCueId>> GetMusicCues()
		{
			return m_musicTracks;
		}

		public void Update()
		{
			UpdatePools(m_voiceHudPools);
			UpdatePools(m_voiceSoundPools);
			UpdatePools(m_voiceMusicPools);
			void UpdatePools(Dictionary<MyWaveFormat, MySourceVoicePool> pools)
			{
				if (pools != null)
				{
					foreach (KeyValuePair<MyWaveFormat, MySourceVoicePool> pool in pools)
					{
						pool.Value?.Update();
					}
				}
			}
		}

		public void ClearSounds()
		{
			ClearPools(m_voiceHudPools);
			ClearPools(m_voiceSoundPools);
			ClearPools(m_voiceMusicPools);
			void ClearPools(Dictionary<MyWaveFormat, MySourceVoicePool> pools)
			{
				foreach (KeyValuePair<MyWaveFormat, MySourceVoicePool> pool in pools)
				{
					pool.Value.StopAll();
				}
			}
		}

		public void Dispose()
		{
			if (m_waveBank != null)
			{
				m_waveBank.Dispose();
			}
			if (m_reverb != null)
			{
				m_reverb.Dispose();
			}
			m_reverb = null;
			ClearSounds();
			DisposePools(m_voiceHudPools);
			DisposePools(m_voiceSoundPools);
			DisposePools(m_voiceMusicPools);
			m_cues.Clear();
			void DisposePools(Dictionary<MyWaveFormat, MySourceVoicePool> pools)
			{
				foreach (KeyValuePair<MyWaveFormat, MySourceVoicePool> pool in pools)
				{
					pool.Value.Dispose();
				}
				pools.Clear();
			}
		}

		public MyStringId? GetRandomTransitionEnum()
		{
			if (m_musicTransitionCues == null)
			{
				return null;
			}
			return m_musicTransitionCues.Keys.ElementAt(MyUtils.GetRandomInt(m_musicTransitionCues.Count));
		}

		public MyStringId GetRandomTransitionCategory(ref MyStringId transitionEnum, ref MyStringId noRandom)
		{
			if (!m_musicTransitionCues.ContainsKey(transitionEnum))
			{
				do
				{
					transitionEnum = GetRandomTransitionEnum().Value;
				}
				while (transitionEnum == noRandom && m_musicTransitionCues.Count > 1);
			}
			int randomInt = MyUtils.GetRandomInt(m_musicTransitionCues[transitionEnum].Count);
			int num = 0;
			foreach (KeyValuePair<MyStringId, MyCueId> item in m_musicTransitionCues[transitionEnum])
			{
				if (num == randomInt)
				{
					return item.Key;
				}
				num++;
			}
			throw new InvalidBranchException();
		}

		public bool IsValidTransitionCategory(MyStringId transitionEnum, MyStringId category)
		{
			if (m_musicTransitionCues != null && m_musicTransitionCues.ContainsKey(transitionEnum))
			{
				if (!(category == MyStringId.NullOrEmpty))
				{
					return m_musicTransitionCues[transitionEnum].ContainsKey(category);
				}
				return true;
			}
			return false;
		}

		public MyCueId GetTransitionCue(MyStringId transitionEnum, MyStringId category)
		{
			return m_musicTransitionCues[transitionEnum][category];
		}

		public MySoundData GetCue(MyCueId cueId)
		{
			if (!m_cues.ContainsKey(cueId) && cueId.Hash != MyStringHash.NullOrEmpty)
			{
				MyLog.Default.WriteLine("Cue was not found: " + cueId, LoggingOptions.AUDIO);
			}
			MySoundData value = null;
			m_cues.TryGetValue(cueId, out value);
			return value;
		}

		public List<MyStringId> GetCategories()
		{
			return m_categories;
		}

		private MyInMemoryWave GetRandomWave(MySoundData cue, MySoundDimensions type, out int waveNumber, out CuePart part, int tryIgnoreWaveNumber = -1)
		{
			int num = 0;
			foreach (MyAudioWave wave2 in cue.Waves)
			{
				if (wave2.Type == type)
				{
					num++;
				}
			}
			if (num == 0)
			{
				waveNumber = 0;
				part = CuePart.Start;
				return null;
			}
			waveNumber = MyUtils.GetRandomInt(num);
			if (num > 2 && waveNumber == tryIgnoreWaveNumber)
			{
				waveNumber = (waveNumber + 1) % num;
			}
			MyInMemoryWave wave = GetWave(cue, type, waveNumber, CuePart.Start);
			if (wave != null)
			{
				part = CuePart.Start;
			}
			else
			{
				wave = GetWave(cue, type, waveNumber, CuePart.Loop);
				part = CuePart.Loop;
			}
			return wave;
		}

		private MyInMemoryWave GetWave(MySoundData cue, MySoundDimensions dim, int waveNumber, CuePart cuePart)
		{
			if (m_waveBank == null)
			{
				return null;
			}
			foreach (MyAudioWave wave in cue.Waves)
			{
				if (wave.Type != dim)
				{
					continue;
				}
				if (waveNumber == 0)
				{
					switch (cuePart)
					{
					case CuePart.Start:
						return cue.StreamSound ? m_waveBank.GetStreamedWave(wave.Start, cue, dim) : m_waveBank.GetWave(wave.Start);
					case CuePart.Loop:
						return cue.StreamSound ? m_waveBank.GetStreamedWave(wave.Loop, cue, dim) : m_waveBank.GetWave(wave.Loop);
					case CuePart.End:
						return cue.StreamSound ? m_waveBank.GetStreamedWave(wave.End, cue, dim) : m_waveBank.GetWave(wave.End);
					}
				}
				waveNumber--;
			}
			return null;
		}

		private MySourceVoice GetVoice(MyCueId cueId, MyInMemoryWave wave, CuePart part, MyVoicePoolType poolType)
		{
			MySourceVoice mySourceVoice = GetVoicePool(poolType, new MyWaveFormat
			{
				Encoding = wave.WaveFormat.Encoding,
				Channels = wave.WaveFormat.Channels,
				SampleRate = wave.WaveFormat.SampleRate,
				WaveFormat = wave.WaveFormat
			}).NextAvailable();
			if (mySourceVoice == null)
			{
				return null;
			}
			mySourceVoice.Flush();
			mySourceVoice.SubmitSourceBuffer(cueId, wave, part);
			return mySourceVoice;
		}

		internal MySourceVoice GetVoice(MyCueId cueId, out int waveNumber, MySoundDimensions type = MySoundDimensions.D2, int tryIgnoreWaveNumber = -1, MyVoicePoolType poolType = MyVoicePoolType.Sound)
		{
			waveNumber = -1;
			if (m_audioEngine == null)
			{
				return null;
			}
			MySoundData cue = GetCue(cueId);
			if (cue == null || cue.Waves == null || cue.Waves.Count == 0)
			{
				return null;
			}
			MyInMemoryWave randomWave = GetRandomWave(cue, type, out waveNumber, out var part, tryIgnoreWaveNumber);
			if (randomWave == null && type == MySoundDimensions.D2)
			{
				type = MySoundDimensions.D3;
				randomWave = GetRandomWave(cue, type, out waveNumber, out part, tryIgnoreWaveNumber);
			}
			if (randomWave == null)
			{
				return null;
			}
			MySourceVoice voice = GetVoice(cueId, randomWave, part, poolType);
			if (voice == null)
			{
				return null;
			}
			if (cue.Loopable)
			{
				randomWave = GetWave(cue, type, waveNumber, CuePart.Loop);
				if (randomWave != null)
				{
					if (voice.Owner.WaveFormat.Encoding == randomWave.WaveFormat.Encoding)
					{
						voice.SubmitSourceBuffer(cueId, randomWave, CuePart.Loop);
					}
					else
					{
						MyLog.Default.WriteLine($"Inconsistent encodings: '{cueId}', got '{randomWave.WaveFormat.Encoding}', expected '{voice.Owner.WaveFormat.Encoding}', part = '{CuePart.Loop}'");
					}
				}
				randomWave = GetWave(cue, type, waveNumber, CuePart.End);
				if (randomWave != null)
				{
					if (voice.Owner.WaveFormat.Encoding == randomWave.WaveFormat.Encoding)
					{
						voice.SubmitSourceBuffer(cueId, randomWave, CuePart.End);
					}
					else
					{
						MyLog.Default.WriteLine($"Inconsistent encodings: '{cueId}', got '{randomWave.WaveFormat.Encoding}', expected '{voice.Owner.WaveFormat.Encoding}', part = '{CuePart.End}'");
					}
				}
			}
			return voice;
		}

		public void WriteDebugInfo(StringBuilder stringBuilder)
		{
			if (m_voiceHudPools != null || m_voiceSoundPools != null || m_voiceMusicPools != null)
			{
				stringBuilder.Append("Playing: ");
				WritePlayingDebugPools(m_voiceHudPools);
				WritePlayingDebugPools(m_voiceSoundPools);
				WritePlayingDebugPools(m_voiceMusicPools);
				stringBuilder.AppendLine("");
				stringBuilder.Append("Not playing: ");
				WritePauseDebugPools(m_voiceHudPools);
				WritePauseDebugPools(m_voiceSoundPools);
				WritePauseDebugPools(m_voiceMusicPools);
			}
			void WritePauseDebugPools(Dictionary<MyWaveFormat, MySourceVoicePool> pools)
			{
				if (pools != null)
				{
					foreach (KeyValuePair<MyWaveFormat, MySourceVoicePool> pool in pools)
					{
						pool.Value.WritePausedDebugInfo(stringBuilder);
					}
				}
			}
			void WritePlayingDebugPools(Dictionary<MyWaveFormat, MySourceVoicePool> pools)
			{
				if (pools != null)
				{
					foreach (KeyValuePair<MyWaveFormat, MySourceVoicePool> pool2 in pools)
					{
						pool2.Value.WritePlayingDebugInfo(stringBuilder);
					}
				}
			}
		}
	}
}
