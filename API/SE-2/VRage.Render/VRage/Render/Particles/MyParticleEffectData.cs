using System.Collections.Generic;
using System.ComponentModel;
using VRage.Network;

namespace VRage.Render.Particles
{
	[GenerateActivator]
	public class MyParticleEffectData
	{
		private const int m_version = 0;

		private int m_particleID;

		private string m_name;

		private float m_length = 90f;

		private bool m_loop;

		private bool m_enabled = true;

		private float m_durationMin;

		private float m_durationMax;

		private bool m_dirty;

		private readonly List<MyParticleGPUGenerationData> m_generations = new List<MyParticleGPUGenerationData>();

		private readonly List<MyParticleLightData> m_particleLights = new List<MyParticleLightData>();

		private int m_showOnlyThisGeneration = -1;

		private float m_priority;

		[Description("Unique ID of the particle effect.")]
		public int ID
		{
			get
			{
				return m_particleID;
			}
			set
			{
				SetID(value);
			}
		}

		[Description("Maximum distance to render and simulate effect.")]
		public float DistanceMax { get; set; } = 3000f;


		[Description("Length to display in Effect life time ruler.")]
		public float Length
		{
			get
			{
				return m_length;
			}
			set
			{
				m_length = value;
			}
		}

		[Description("Minimum length effect can have when started or looped.")]
		public float DurationMin
		{
			get
			{
				return m_durationMin;
			}
			set
			{
				m_durationMin = value;
			}
		}

		[Description("Maximum length effect can have when started or looped. Set to zero if minimum should be used only.")]
		public float DurationMax
		{
			get
			{
				return m_durationMax;
			}
			set
			{
				m_durationMax = value;
			}
		}

		[Description("When lifetime of the effect reaches duration, effect is either stopped or looped.")]
		public bool Loop
		{
			get
			{
				return m_loop;
			}
			set
			{
				m_loop = value;
			}
		}

		[Description("Only enabled effects are available in the game.")]
		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				m_enabled = value;
			}
		}

		[Description("Only enabled effects are available in the game.")]
		public float Priority
		{
			get
			{
				return m_priority;
			}
			set
			{
				m_priority = value;
			}
		}

		[Browsable(false)]
		public int ShowOnlyThisGeneration => m_showOnlyThisGeneration;

		[Description("Unique name of the particle effect.")]
		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				SetName(value);
			}
		}

		[Browsable(false)]
		public object Tag { get; set; }

		public bool IsDirty()
		{
			return m_dirty;
		}

		public void SetDirty()
		{
			m_dirty = true;
		}

		public void ClearDirty()
		{
			m_dirty = false;
		}

		public void Start(int particleID, string particleName)
		{
			m_particleID = particleID;
			m_name = particleName;
		}

		public MyParticleEffectData Duplicate()
		{
			MyParticleEffectData myParticleEffectData = new MyParticleEffectData();
			myParticleEffectData.Start(0, Name);
			myParticleEffectData.m_length = m_length;
			myParticleEffectData.DurationMin = m_durationMin;
			myParticleEffectData.DurationMax = m_durationMax;
			myParticleEffectData.Loop = m_loop;
			myParticleEffectData.Priority = m_priority;
			foreach (MyParticleGPUGenerationData generation2 in m_generations)
			{
				MyParticleGPUGenerationData generation = generation2.Duplicate(myParticleEffectData);
				myParticleEffectData.AddGeneration(generation, loading: true);
			}
			foreach (MyParticleLightData particleLight2 in m_particleLights)
			{
				MyParticleLightData particleLight = particleLight2.Duplicate(myParticleEffectData);
				myParticleEffectData.AddParticleLight(particleLight, loading: true);
			}
			return myParticleEffectData;
		}

		public void SetShowOnlyThisGeneration(MyParticleGPUGenerationData generation)
		{
			for (int i = 0; i < m_generations.Count; i++)
			{
				if (m_generations[i] == generation)
				{
					SetShowOnlyThisGeneration(i);
					return;
				}
			}
			SetShowOnlyThisGeneration(-1);
		}

		public void SetShowOnlyThisGeneration(int generationIndex)
		{
			m_showOnlyThisGeneration = generationIndex;
			for (int i = 0; i < m_generations.Count; i++)
			{
				m_generations[i].Show = generationIndex < 0 || i == generationIndex;
			}
		}

		public int GetID()
		{
			return m_particleID;
		}

		public void SetID(int id)
		{
			if (m_particleID != id)
			{
				int particleID = m_particleID;
				m_particleID = id;
				if (particleID != 0)
				{
					MyParticleEffectsLibrary.UpdateID(particleID);
				}
			}
		}

		public void SetName(string name)
		{
			if (m_name != name)
			{
				string name2 = m_name;
				m_name = name;
				if (name2 != null)
				{
					MyParticleEffectsLibrary.UpdateName(name2);
				}
			}
		}

		public void AddGeneration(MyParticleGPUGenerationData generation, bool loading)
		{
			m_generations.Add(generation);
			if (!loading)
			{
				MyParticleEffectsLibrary.Recreate(this);
			}
		}

		public void RemoveGeneration(int index)
		{
			MyParticleGPUGenerationData item = m_generations[index];
			m_generations.Remove(item);
			MyParticleEffectsLibrary.Recreate(this);
		}

		public void RemoveGeneration(MyParticleGPUGenerationData generation)
		{
			int index = m_generations.IndexOf(generation);
			RemoveGeneration(index);
		}

		public List<MyParticleGPUGenerationData> GetGenerations()
		{
			return m_generations;
		}

		public void AddParticleLight(MyParticleLightData particleLight, bool loading)
		{
			m_particleLights.Add(particleLight);
			if (!loading)
			{
				MyParticleEffectsLibrary.Recreate(this);
			}
		}

		public void RemoveParticleLight(int index)
		{
			MyParticleLightData item = m_particleLights[index];
			m_particleLights.Remove(item);
			MyParticleEffectsLibrary.Recreate(this);
		}

		public void RemoveParticleLight(MyParticleLightData particleLight)
		{
			int index = m_particleLights.IndexOf(particleLight);
			RemoveParticleLight(index);
		}

		public List<MyParticleLightData> GetParticleLights()
		{
			return m_particleLights;
		}

		public override string ToString()
		{
			return Name + " (" + ID + ")";
		}
	}
}
