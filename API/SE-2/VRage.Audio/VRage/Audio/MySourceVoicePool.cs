using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using VRage.Collections;
using VRage.Utils;

namespace VRage.Audio
{
	internal class MySourceVoicePool
	{
		private struct FadeoutData
		{
			public const float TargetVolume = 0.01f;

			public const float VolumeMultiplierPerStep = 0.65f;

			public int RemainingSteps;

			public readonly MySourceVoice Voice;

			public FadeoutData(MySourceVoice voice)
			{
				if (voice == null || voice.Voice == null)
				{
					string msg = "FadeoutData initialized with " + ((voice == null) ? "MySourceVoice == null." : "MySourceVoice having SourceVoice null.");
					MyLog.Default.WriteLine(msg);
					Voice = voice;
					RemainingSteps = 0;
					return;
				}
				Voice = voice;
				voice.Voice.GetVolume(out var volumeRef);
				if (volumeRef <= 0.01f)
				{
					RemainingSteps = 0;
				}
				else
				{
					RemainingSteps = (int)Math.Log(0.01f / volumeRef, 0.64999997615814209);
				}
			}
		}

		private XAudio2 m_audioEngine;

		private readonly WaveFormat m_waveFormat;

		private MyCueBank m_owner;

		private readonly MyConcurrentQueue<MySourceVoice> m_voicesToRecycle;

		private readonly MyConcurrentQueue<MySourceVoice> m_availableVoices;

		private readonly List<FadeoutData> m_fadingOutVoices;

		private readonly VoiceSendDescriptor[] m_desc;

		public bool UseSameSoundLimiter;

		private int m_currentCount;

		private const int MAX_COUNT = 128;

		private readonly ConcurrentDictionary<MySourceVoice, byte> m_allVoices = new ConcurrentDictionary<MySourceVoice, byte>();

		private readonly List<MySourceVoice> m_voicesToRemove = new List<MySourceVoice>();

		private readonly List<MySourceVoice> m_distancedVoices = new List<MySourceVoice>();

		private readonly List<MySourceVoice> m_voicesToDispose = new List<MySourceVoice>();

		public WaveFormat WaveFormat => m_waveFormat;

		public event AudioEngineChanged OnAudioEngineChanged;

		public MySourceVoicePool(XAudio2 audioEngine, WaveFormat waveformat, MyCueBank owner, VoiceSendDescriptor[] desc = null)
		{
			m_audioEngine = audioEngine;
			m_waveFormat = waveformat;
			m_owner = owner;
			m_voicesToRecycle = new MyConcurrentQueue<MySourceVoice>(20);
			m_availableVoices = new MyConcurrentQueue<MySourceVoice>(128);
			m_fadingOutVoices = new List<FadeoutData>();
			m_currentCount = 0;
			m_desc = desc;
		}

		public void SetAudioEngine(XAudio2 audioEngine)
		{
			if (m_audioEngine != audioEngine)
			{
				this.OnAudioEngineChanged?.Invoke();
				m_audioEngine = audioEngine;
				m_voicesToRecycle.Clear();
				m_availableVoices.Clear();
				m_fadingOutVoices.Clear();
				m_currentCount = 0;
			}
		}

		internal MySourceVoice NextAvailable()
		{
			if (m_audioEngine == null)
			{
				return null;
			}
			MySourceVoice instance = null;
			if ((m_owner.DisablePooling || !m_availableVoices.TryDequeue(out instance)) && m_allVoices.Count < 128)
			{
				instance = new MySourceVoice(this, m_audioEngine, m_waveFormat);
				if (m_desc != null)
				{
					instance.SetOutputVoices(m_desc);
				}
				m_allVoices.TryAdd(instance, 0);
				m_currentCount++;
			}
			return instance;
		}

		public void OnStopPlaying(MySourceVoice voice)
		{
			m_currentCount--;
			m_voicesToRecycle.Enqueue(voice);
		}

		public void Update()
		{
			if (m_owner == null || m_audioEngine == null)
			{
				return;
			}
			if (m_owner.DisablePooling)
			{
				foreach (MySourceVoice item in m_voicesToDispose)
				{
					item.Dispose();
				}
				m_voicesToDispose.Clear();
				MySourceVoice instance;
				while (m_voicesToRecycle.TryDequeue(out instance))
				{
					instance.CleanupBeforeDispose();
					m_voicesToDispose.Add(instance);
				}
			}
			else
			{
				MySourceVoice instance2;
				while (m_voicesToRecycle.TryDequeue(out instance2))
				{
					instance2.Emitter = null;
					m_availableVoices.Enqueue(instance2);
				}
			}
			int num = 0;
			while (num < m_fadingOutVoices.Count)
			{
				FadeoutData value = m_fadingOutVoices[num];
				MySourceVoice voice = value.Voice;
				if (!voice.IsValid)
				{
					m_fadingOutVoices.RemoveAt(num);
				}
				else
				{
					if (value.RemainingSteps == 0)
					{
						voice.Voice.Stop();
						voice.Voice.FlushSourceBuffers();
						m_fadingOutVoices.RemoveAt(num);
						continue;
					}
					voice.Voice.GetVolume(out var volumeRef);
					voice.Voice.SetVolume(volumeRef * 0.65f);
					value.RemainingSteps--;
					m_fadingOutVoices[num] = value;
				}
				num++;
			}
			m_voicesToRemove.Clear();
			foreach (MySourceVoice key in m_allVoices.Keys)
			{
				if (!key.IsValid)
				{
					m_voicesToRemove.Add(key);
				}
			}
			while (m_voicesToRemove.Count > 0)
			{
				m_allVoices.Remove(m_voicesToRemove[0]);
				m_voicesToRemove.RemoveAt(0);
			}
			if (!UseSameSoundLimiter)
			{
				return;
			}
			m_distancedVoices.Clear();
			foreach (MySourceVoice key2 in m_allVoices.Keys)
			{
				m_distancedVoices.Add(key2);
			}
			m_distancedVoices.Sort((MySourceVoice x, MySourceVoice y) => x.DistanceToListener.CompareTo(y.DistanceToListener));
			while (m_distancedVoices.Count > 0)
			{
				MyCueId cueEnum = m_distancedVoices[0].CueEnum;
				int num2 = 0;
				int num3 = MyAudio.Static.GetCue(cueEnum)?.SoundLimit ?? 0;
				for (int i = 0; i < m_distancedVoices.Count; i++)
				{
					if (m_distancedVoices[i].CueEnum.Equals(cueEnum))
					{
						num2++;
						if (num3 > 0 && num2 > num3)
						{
							m_distancedVoices[i].Silent = true;
						}
						else
						{
							m_distancedVoices[i].Silent = false;
						}
						m_distancedVoices.RemoveAt(i);
						i--;
					}
				}
			}
		}

		public void AddToFadeoutList(MySourceVoice voice)
		{
			m_fadingOutVoices.Add(new FadeoutData(voice));
		}

		public void StopAll()
		{
			foreach (MySourceVoice key in m_allVoices.Keys)
			{
				key.Stop(force: true);
			}
		}

		public void Dispose()
		{
			m_availableVoices.Clear();
			m_fadingOutVoices.Clear();
			m_currentCount = 0;
			m_audioEngine = null;
			m_owner = null;
			foreach (MySourceVoice key in m_allVoices.Keys)
			{
				key.CleanupBeforeDispose();
				m_voicesToDispose.Add(key);
			}
			m_allVoices.Clear();
			foreach (MySourceVoice item in m_voicesToDispose)
			{
				item.Dispose();
			}
			m_voicesToDispose.Clear();
		}

		public void WritePlayingDebugInfo(StringBuilder stringBuilder)
		{
		}

		public void WritePausedDebugInfo(StringBuilder stringBuilder)
		{
		}
	}
}
