using System.Collections.Generic;

namespace VRage.Render.Particles
{
	public static class MyParticleEffectsLibrary
	{
		private static IParticleManager m_manager;

		private static readonly Dictionary<string, MyParticleEffectData> m_dataString = new Dictionary<string, MyParticleEffectData>();

		private static readonly Dictionary<int, MyParticleEffectData> m_dataId = new Dictionary<int, MyParticleEffectData>();

		public static void Init(IParticleManager manager)
		{
			m_manager = manager;
		}

		public static void Add(MyParticleEffectData data)
		{
			if (m_dataString.TryGetValue(data.Name, out var value))
			{
				Remove(value);
			}
			m_dataString[data.Name] = data;
			m_dataId[data.ID] = data;
		}

		public static bool Exists(string name)
		{
			return m_dataString.ContainsKey(name);
		}

		public static MyParticleEffectData Get(string name)
		{
			if (m_dataString.TryGetValue(name, out var value))
			{
				return value;
			}
			return null;
		}

		public static void UpdateID(int id)
		{
			m_dataId.TryGetValue(id, out var value);
			if (value != null)
			{
				m_dataId.Remove(id);
				m_dataId.Add(value.ID, value);
			}
		}

		public static void UpdateName(string name)
		{
			m_dataString.TryGetValue(name, out var value);
			if (value != null)
			{
				m_dataString.Remove(name);
				m_dataString.Add(value.Name, value);
			}
		}

		public static void Remove(string name)
		{
			m_dataString.TryGetValue(name, out var value);
			if (value != null)
			{
				m_manager?.RemoveParticleEffects(value);
				m_dataString.Remove(value.Name);
				m_dataId.Remove(value.ID);
			}
		}

		public static void Remove(MyParticleEffectData data)
		{
			Remove(data.Name);
		}

		public static IEnumerable<int> GetIDs()
		{
			return m_dataId.Keys;
		}

		public static IEnumerable<string> GetNames()
		{
			return m_dataString.Keys;
		}

		public static IReadOnlyDictionary<int, MyParticleEffectData> GetById()
		{
			return m_dataId;
		}

		public static IReadOnlyDictionary<string, MyParticleEffectData> GetByName()
		{
			return m_dataString;
		}

		public static bool GetID(string name, out int id)
		{
			if (m_dataString.TryGetValue(name, out var value))
			{
				id = value.GetID();
				return true;
			}
			id = -1;
			return false;
		}

		public static bool GetName(int id, out string name)
		{
			if (m_dataId.TryGetValue(id, out var value))
			{
				name = value.Name;
				return true;
			}
			name = string.Empty;
			return false;
		}

		public static void Close()
		{
			if (m_manager != null)
			{
				foreach (MyParticleEffectData value in m_dataString.Values)
				{
					m_manager.RemoveParticleEffects(value);
				}
			}
			m_dataString.Clear();
			m_dataId.Clear();
			m_manager = null;
		}

		public static void Recreate(MyParticleEffectData data)
		{
			if (m_manager != null)
			{
				m_manager.RecreateParticleEffects(data);
			}
		}
	}
}
