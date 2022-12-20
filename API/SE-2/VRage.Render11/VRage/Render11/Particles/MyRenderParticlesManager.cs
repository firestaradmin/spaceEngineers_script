using System.Collections.Generic;
using VRage.Generics;
using VRage.Render.Particles;
using VRage.Render11.Common;

namespace VRage.Render11.Particles
{
	internal class MyRenderParticlesManager : IManager
	{
		private readonly MyObjectsPool<MyRenderParticleEffect> m_effectsPool = new MyObjectsPool<MyRenderParticleEffect>(2048);

		public readonly MyObjectsPool<MyParticleGPUGeneration> GPUGenerationsPool = new MyObjectsPool<MyParticleGPUGeneration>(4096);

		public readonly MyObjectsPool<MyParticleLight> LightsPool = new MyObjectsPool<MyParticleLight>(512);

		private readonly Dictionary<uint, MyRenderParticleEffect> m_effects = new Dictionary<uint, MyRenderParticleEffect>();

		private readonly List<uint> m_effectsToDelete = new List<uint>();

		private bool m_unloading;

		public void Create(uint id, MyParticleEffectData data, string name)
		{
			m_effectsPool.AllocateOrCreate(out var item);
			item.Init(id, data, name);
			m_effects.Add(id, item);
		}

		public void Remove(uint id, bool forceInstant, bool output = true)
		{
			if (m_effects.TryGetValue(id, out var value))
			{
				value.OnRemove(forceInstant, !m_unloading && output);
				m_effectsPool.Deallocate(value);
				m_effects.Remove(id);
			}
		}

		public MyRenderParticleEffect Get(uint id)
		{
			if (!m_effects.TryGetValue(id, out var value))
			{
				return null;
			}
			return value;
		}

		public void UnloadData()
		{
			m_unloading = true;
			Update();
			m_effectsToDelete.AddRange(m_effects.Keys);
			foreach (uint item in m_effectsToDelete)
			{
				Remove(item, forceInstant: true);
			}
			m_effectsToDelete.Clear();
			m_unloading = false;
		}

		public void Update()
		{
			foreach (KeyValuePair<uint, MyRenderParticleEffect> effect in m_effects)
			{
				MyRenderParticleEffect value = effect.Value;
				if (value.Update())
				{
					if (value.Autodelete)
					{
						m_effectsToDelete.Add(effect.Key);
					}
					else
					{
						value.StopEmitting();
					}
				}
			}
			foreach (uint item in m_effectsToDelete)
			{
				Remove(item, forceInstant: false);
			}
			m_effectsToDelete.Clear();
			foreach (KeyValuePair<uint, MyRenderParticleEffect> effect2 in m_effects)
			{
				effect2.Value.ClearEffectDataDirty();
			}
		}

		public void Draw()
		{
			foreach (KeyValuePair<uint, MyRenderParticleEffect> effect in m_effects)
			{
				effect.Value.Draw();
			}
		}
	}
}
