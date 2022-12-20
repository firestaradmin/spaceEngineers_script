using System.Collections.Generic;
using SharpDX.XAudio2;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.Utils;

namespace VRage.Audio
{
	internal class MyEffectBank
	{
		private Dictionary<MyStringHash, MyAudioEffect> m_effects = new Dictionary<MyStringHash, MyAudioEffect>(MyStringHash.Comparer);

		private List<MyEffectInstance> m_activeEffects = new List<MyEffectInstance>();

		private XAudio2 m_engine;

		public MyEffectBank(ListReader<MyAudioEffect> effects, XAudio2 engine)
		{
			foreach (MyAudioEffect item in effects)
			{
				m_effects[item.EffectId] = item;
			}
			m_engine = engine;
		}

		public MyEffectInstance CreateEffect(IMySourceVoice input, MyStringHash effect, MySourceVoice[] cues = null, float? duration = null)
		{
			if (!m_effects.ContainsKey(effect))
			{
				return null;
			}
			MyEffectInstance myEffectInstance = new MyEffectInstance(m_effects[effect], input, cues, duration, m_engine);
			m_activeEffects.Add(myEffectInstance);
			return myEffectInstance;
		}

		public void Update(int ms)
		{
			for (int num = m_activeEffects.Count - 1; num >= 0; num--)
			{
				if (m_activeEffects[num].Finished)
				{
					m_activeEffects.RemoveAt(num);
				}
				else if (m_activeEffects[num].AutoUpdate)
				{
					m_activeEffects[num].Update(ms);
				}
			}
		}
	}
}
