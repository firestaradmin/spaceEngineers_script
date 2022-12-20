using System;
using System.Collections.Generic;
using SharpDX.XAudio2;
using VRage.Data.Audio;

namespace VRage.Audio
{
	internal class MyEffectInstance : IMyAudioEffect
	{
		private class SoundData
		{
			public MySourceVoice Sound;

			public float Pivot;

			public int CurrentEffect;

			public float OrigVolume;

			public float OrigFrequency;

			public FilterParameters CurrentFilter;
		}

		private const int FINISHED_OP_SET = 1;

		private static FilterParameters m_defaultFilter = new FilterParameters
		{
			Type = FilterType.LowPassFilter,
			Frequency = 1f,
			OneOverQ = 1f
		};

		private bool m_autoUpdate = true;

		private bool m_ended;

		private MyAudioEffect m_effect;

		private List<SoundData> m_sounds = new List<SoundData>();

		private float m_elapsed;

		private float m_scale = 1f;

		private float m_duration;

		private XAudio2 m_engine;

		public bool AutoUpdate
		{
			get
			{
				return m_autoUpdate;
			}
			set
			{
				m_autoUpdate = value;
			}
		}

		public bool Finished => m_ended;

		public IMySourceVoice OutputSound
		{
			get
			{
				if (m_effect.ResultEmitterIdx >= m_sounds.Count)
				{
					return null;
				}
				return m_sounds[m_effect.ResultEmitterIdx].Sound;
			}
		}

		public event Action<MyEffectInstance> OnEffectEnded;

		public MyEffectInstance(MyAudioEffect effect, IMySourceVoice input, MySourceVoice[] cues, float? duration, XAudio2 engine)
		{
			m_engine = engine;
			m_effect = effect;
			MySourceVoice mySourceVoice = input as MySourceVoice;
			if (mySourceVoice != null && mySourceVoice.IsValid && mySourceVoice.Voice != null && mySourceVoice.Voice.IsValid())
			{
				SoundData item = new SoundData
				{
					Sound = mySourceVoice,
					Pivot = 0f,
					CurrentEffect = 0,
					OrigVolume = mySourceVoice.Volume,
					OrigFrequency = mySourceVoice.FrequencyRatio
				};
				m_sounds.Add(item);
			}
			foreach (MySourceVoice mySourceVoice2 in cues)
			{
				mySourceVoice2.Start(skipIntro: false);
				m_sounds.Add(new SoundData
				{
					Sound = mySourceVoice2,
					Pivot = 0f,
					CurrentEffect = 0,
					OrigVolume = mySourceVoice2.Volume,
					OrigFrequency = mySourceVoice2.FrequencyRatio
				});
			}
			if (OutputSound != null)
			{
				IMySourceVoice outputSound = OutputSound;
				outputSound.StoppedPlaying = (Action<IMySourceVoice>)Delegate.Combine(outputSound.StoppedPlaying, new Action<IMySourceVoice>(EffectFinished));
			}
			ComputeDurationAndScale(duration);
			Update(0);
		}

		private void ComputeDurationAndScale(float? duration)
		{
			float num = 0f;
			foreach (List<MyAudioEffect.SoundEffect> soundsEffect in m_effect.SoundsEffects)
			{
				float num2 = 0f;
				foreach (MyAudioEffect.SoundEffect item in soundsEffect)
				{
					num2 += item.Duration;
				}
				if (num2 > num)
				{
					num = num2;
				}
			}
			if (num > 0f && duration.HasValue)
			{
				m_scale = duration.Value / num;
			}
			m_duration = num * m_scale;
		}

		public void Update(int stepMs)
		{
			if (m_ended)
			{
				return;
			}
			m_elapsed += stepMs;
			bool flag = true;
			for (int i = 0; i < m_sounds.Count; i++)
			{
				SoundData soundData = m_sounds[i];
				MySourceVoice sound = soundData.Sound;
				if (sound == null || !sound.IsValid || !soundData.Sound.IsPlaying || soundData.CurrentEffect >= m_effect.SoundsEffects[i].Count)
				{
					continue;
				}
				MyAudioEffect.SoundEffect effect = m_effect.SoundsEffects[i][soundData.CurrentEffect];
				float num = ((!(effect.Duration > 0f)) ? 0f : ((m_elapsed - soundData.Pivot) / (effect.Duration * m_scale)));
				if (num > 1f)
				{
					if (effect.StopAfter)
					{
						soundData.Sound.Stop();
						continue;
					}
					soundData.CurrentEffect++;
					soundData.Pivot += effect.Duration * m_scale;
					i--;
					if (effect.Filter != MyAudioEffect.FilterType.None)
					{
						soundData.Sound.Voice.SetFilterParameters(m_defaultFilter);
					}
				}
				else
				{
					UpdateVolume(soundData, effect, num);
					UpdateFilter(soundData, effect);
					flag = false;
				}
			}
			if (flag)
			{
				EffectFinished();
			}
		}

		private static void UpdateVolume(SoundData sData, MyAudioEffect.SoundEffect effect, float effPosition)
		{
			if (effect.VolumeCurve != null)
			{
				sData.Sound.SetVolume(sData.OrigVolume * effect.VolumeCurve.Evaluate(effPosition));
			}
		}

		private static void UpdateFilter(SoundData sData, MyAudioEffect.SoundEffect effect)
		{
			MySourceVoice sound = sData.Sound;
			if ((sound.IsValid || !sound.Voice.IsDisposed) && effect.Filter != MyAudioEffect.FilterType.None)
			{
				sData.CurrentFilter = new FilterParameters
				{
					Frequency = effect.Frequency,
					OneOverQ = effect.OneOverQ,
					Type = (FilterType)effect.Filter
				};
				sound.Voice.SetFilterParameters(sData.CurrentFilter);
			}
		}

		public void SetPosition(float msecs)
		{
			m_elapsed = msecs;
			Update(0);
		}

		public void SetPositionRelative(float position)
		{
			m_elapsed = m_duration * position;
			Update(0);
		}

		private void EffectFinished(IMySourceVoice _ = null)
		{
			if (m_ended)
			{
				return;
			}
			m_ended = true;
			for (int i = 0; i < m_sounds.Count; i++)
			{
				if (m_sounds[i].Sound != null && m_sounds[i].Sound.IsValid && m_sounds[i].Sound.Voice != null && m_sounds[i].Sound.Voice.IsValid())
				{
					m_sounds[i].Sound.Voice.SetFilterParameters(m_defaultFilter, 1);
					if (i != m_effect.ResultEmitterIdx)
					{
						m_sounds[i].Sound.Stop();
					}
				}
			}
			if (m_engine != null && !m_engine.IsDisposed)
			{
				m_engine.CommitChanges(1);
			}
			if (OutputSound != null)
			{
				IMySourceVoice outputSound = OutputSound;
				outputSound.StoppedPlaying = (Action<IMySourceVoice>)Delegate.Remove(outputSound.StoppedPlaying, new Action<IMySourceVoice>(EffectFinished));
			}
			this.OnEffectEnded?.Invoke(this);
		}
	}
}
