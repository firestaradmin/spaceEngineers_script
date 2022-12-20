using System;
<<<<<<< HEAD
using Sandbox.Game.Components;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.World;
using VRage.Audio;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.Entities.Character
{
	public class MyCharacterBreath
	{
		public enum State
		{
			Calm,
			Heated,
			VeryHeated,
			NoBreath,
			Choking
		}

		private readonly string BREATH_CALM = "PlayVocBreath1L";

		private readonly string BREATH_HEAVY = "PlayVocBreath2L";

		private readonly string OXYGEN_CHOKE_NORMAL = "PlayChokeA";

		private readonly string OXYGEN_CHOKE_LOW = "PlayChokeB";

		private readonly string OXYGEN_CHOKE_CRITICAL = "PlayChokeC";

		private const float CHOKE_TRESHOLD_LOW = 55f;

		private const float CHOKE_TRESHOLD_CRITICAL = 25f;

		private const float STAMINA_DRAIN_TIME_RUN = 25f;

		private const float STAMINA_DRAIN_TIME_SPRINT = 8f;

		private const float STAMINA_RECOVERY_EXHAUSTED_TO_CALM = 5f;

		private const float STAMINA_RECOVERY_CALM_TO_ZERO = 15f;

		private const float STAMINA_AMOUNT_RUN = 0.0100000007f;

		private const float STAMINA_AMOUNT_SPRINT = 0.03125f;

		private const float STAMINA_AMOUNT_MAX = 20f;

		private IMySourceVoice m_sound;

		private MyCharacter m_character;

		private MyTimeSpan m_lastChange;

		private State m_state;

		private float m_staminaDepletion;

		private MySoundPair m_breathCalm;

		private MySoundPair m_breathHeavy;

		private MySoundPair m_oxygenChokeNormal;

		private MySoundPair m_oxygenChokeLow;

		private MySoundPair m_oxygenChokeCritical;

		public State CurrentState
		{
			get
			{
				return m_state;
			}
			set
			{
				m_state = value;
			}
		}

		public MyCharacterBreath(MyCharacter character)
		{
			CurrentState = State.NoBreath;
			m_character = character;
			string cueName = (string.IsNullOrEmpty(character.Definition.BreathCalmSoundName) ? BREATH_CALM : character.Definition.BreathCalmSoundName);
			m_breathCalm = new MySoundPair(cueName);
			string cueName2 = (string.IsNullOrEmpty(character.Definition.BreathHeavySoundName) ? BREATH_HEAVY : character.Definition.BreathHeavySoundName);
			m_breathHeavy = new MySoundPair(cueName2);
			string cueName3 = (string.IsNullOrEmpty(character.Definition.OxygenChokeNormalSoundName) ? OXYGEN_CHOKE_NORMAL : character.Definition.OxygenChokeNormalSoundName);
			m_oxygenChokeNormal = new MySoundPair(cueName3);
			string cueName4 = (string.IsNullOrEmpty(character.Definition.OxygenChokeLowSoundName) ? OXYGEN_CHOKE_LOW : character.Definition.OxygenChokeLowSoundName);
			m_oxygenChokeLow = new MySoundPair(cueName4);
			string cueName5 = (string.IsNullOrEmpty(character.Definition.OxygenChokeCriticalSoundName) ? OXYGEN_CHOKE_CRITICAL : character.Definition.OxygenChokeCriticalSoundName);
			m_oxygenChokeCritical = new MySoundPair(cueName5);
		}

		public void ForceUpdate()
		{
			if (m_character != null && m_character.StatComp != null && m_character.StatComp.Health != null && MySession.Static != null && MySession.Static.LocalCharacter == m_character)
			{
				SetHealth(m_character.StatComp.Health.Value);
			}
		}

		private void SetHealth(float health)
		{
			if (health <= 0f)
			{
				CurrentState = State.NoBreath;
			}
			Update(force: true);
		}

		public void Update(bool force = false)
		{
			if (MySession.Static == null || MySession.Static.LocalCharacter != m_character || MySession.Static.LocalCharacter != null)
			{
				return;
			}
			if (CurrentState == State.Heated)
			{
				m_staminaDepletion = Math.Min(m_staminaDepletion + 0.0100000007f, 20f);
			}
			else if (CurrentState == State.VeryHeated)
			{
				m_staminaDepletion = Math.Min(m_staminaDepletion + 0.03125f, 20f);
			}
			else
			{
				m_staminaDepletion = Math.Max(m_staminaDepletion - 0.0166666675f, 0f);
			}
			if (CurrentState == State.NoBreath)
			{
				if (m_sound != null)
				{
					m_sound.Stop();
					m_sound = null;
				}
				return;
			}
			float num = 0f;
			MyCharacter character = m_character;
			int num2;
			if (character == null)
			{
				num2 = 0;
			}
			else
			{
				MyCharacterStatComponent statComp = character.StatComp;
				if (statComp == null)
				{
					num2 = 0;
				}
				else
				{
					MyEntityStat health = statComp.Health;
					if (health == null)
					{
						num2 = 0;
					}
					else
					{
						_ = health.Value;
						num2 = 1;
					}
				}
			}
			if (num2 != 0)
			{
				num = m_character.StatComp.Health.Value;
			}
			if (CurrentState == State.Choking)
			{
				if (num >= 55f && (m_sound == null || !m_sound.IsPlaying || m_sound.CueEnum != m_oxygenChokeNormal.SoundId))
				{
					PlaySound(m_oxygenChokeNormal.SoundId, useCrossfade: false);
				}
				else if (num >= 25f && num < 55f && (m_sound == null || !m_sound.IsPlaying || m_sound.CueEnum != m_oxygenChokeLow.SoundId))
				{
					PlaySound(m_oxygenChokeLow.SoundId, useCrossfade: false);
				}
				else if (num > 0f && num < 25f && (m_sound == null || !m_sound.IsPlaying || m_sound.CueEnum != m_oxygenChokeCritical.SoundId))
				{
					PlaySound(m_oxygenChokeCritical.SoundId, useCrossfade: false);
				}
			}
			else
			{
				if (CurrentState != 0 && CurrentState != State.Heated && CurrentState != State.VeryHeated)
				{
					return;
				}
				if (m_staminaDepletion < 15f && num > 20f)
				{
					if (!m_breathCalm.SoundId.IsNull && (m_sound == null || !m_sound.IsPlaying || m_sound.CueEnum != m_breathCalm.SoundId))
					{
						PlaySound(m_breathCalm.SoundId, useCrossfade: true);
					}
					else if (m_sound != null && m_sound.IsPlaying && m_breathCalm.SoundId.IsNull)
					{
						m_sound.Stop(force: true);
					}
				}
				else if (!m_breathHeavy.SoundId.IsNull && (m_sound == null || !m_sound.IsPlaying || m_sound.CueEnum != m_breathHeavy.SoundId))
				{
					PlaySound(m_breathHeavy.SoundId, useCrossfade: true);
				}
				else if (m_sound != null && m_sound.IsPlaying && m_breathHeavy.SoundId.IsNull)
				{
					m_sound.Stop(force: true);
				}
			}
		}

		private void PlaySound(MyCueId soundId, bool useCrossfade)
		{
			if (m_sound != null && m_sound.IsPlaying && useCrossfade)
			{
				IMyAudioEffect myAudioEffect = MyAudio.Static.ApplyEffect(m_sound, MyStringHash.GetOrCompute("CrossFade"), new MyCueId[1] { soundId }, 2000f);
				m_sound = myAudioEffect.OutputSound;
				return;
			}
			if (m_sound != null)
			{
				m_sound.Stop(force: true);
			}
			m_sound = MyAudio.Static.PlaySound(soundId);
		}

		public void Close()
		{
			if (m_sound != null)
			{
				m_sound.Stop(force: true);
			}
		}
	}
}
