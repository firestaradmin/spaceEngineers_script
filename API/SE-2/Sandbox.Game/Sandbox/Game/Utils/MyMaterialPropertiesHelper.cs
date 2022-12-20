using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Utils
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyMaterialPropertiesHelper : MySessionComponentBase
	{
		public static class CollisionType
		{
			public static MyStringId Start = MyStringId.GetOrCompute("Start");

			public static MyStringId Hit = MyStringId.GetOrCompute("Hit");

			public static MyStringId Walk = MyStringId.GetOrCompute("Walk");

			public static MyStringId Run = MyStringId.GetOrCompute("Run");

			public static MyStringId Sprint = MyStringId.GetOrCompute("Sprint");
		}

		private struct MaterialProperties
		{
			public MySoundPair Sound;

			public string ParticleEffectName;

			public List<MyPhysicalMaterialDefinition.ImpactSounds> ImpactSoundCues;

			public MaterialProperties(MySoundPair soundCue, string particleEffectName, List<MyPhysicalMaterialDefinition.ImpactSounds> impactSounds)
			{
				Sound = soundCue;
				ParticleEffectName = particleEffectName;
				ImpactSoundCues = impactSounds;
			}
		}

		public static MyMaterialPropertiesHelper Static;

		private Dictionary<MyStringId, Dictionary<MyStringHash, Dictionary<MyStringHash, MaterialProperties>>> m_materialDictionary = new Dictionary<MyStringId, Dictionary<MyStringHash, Dictionary<MyStringHash, MaterialProperties>>>(MyStringId.Comparer);

		private HashSet<MyStringHash> m_loaded = new HashSet<MyStringHash>((IEqualityComparer<MyStringHash>)MyStringHash.Comparer);

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
			foreach (MyPhysicalMaterialDefinition physicalMaterialDefinition in MyDefinitionManager.Static.GetPhysicalMaterialDefinitions())
			{
				LoadMaterialProperties(physicalMaterialDefinition);
			}
			foreach (MyPhysicalMaterialDefinition physicalMaterialDefinition2 in MyDefinitionManager.Static.GetPhysicalMaterialDefinitions())
			{
				LoadMaterialSoundsInheritance(physicalMaterialDefinition2);
			}
		}

		private void LoadMaterialSoundsInheritance(MyPhysicalMaterialDefinition material)
		{
			MyStringHash subtypeId = material.Id.SubtypeId;
			if (!m_loaded.Add(subtypeId) || !(material.InheritFrom != MyStringHash.NullOrEmpty))
			{
				return;
			}
			if (MyDefinitionManager.Static.TryGetDefinition<MyPhysicalMaterialDefinition>(new MyDefinitionId(typeof(MyObjectBuilder_PhysicalMaterialDefinition), material.InheritFrom), out var definition))
			{
				if (!m_loaded.Contains(material.InheritFrom))
				{
					LoadMaterialSoundsInheritance(definition);
				}
				foreach (KeyValuePair<MyStringId, MySoundPair> generalSound in definition.GeneralSounds)
				{
					material.GeneralSounds[generalSound.Key] = generalSound.Value;
				}
			}
			foreach (MyStringId key in m_materialDictionary.Keys)
			{
				if (!m_materialDictionary[key].ContainsKey(subtypeId))
				{
					m_materialDictionary[key][subtypeId] = new Dictionary<MyStringHash, MaterialProperties>(MyStringHash.Comparer);
				}
				MaterialProperties? materialProperties = null;
				if (!m_materialDictionary[key].ContainsKey(material.InheritFrom))
				{
					continue;
				}
				foreach (KeyValuePair<MyStringHash, MaterialProperties> item in m_materialDictionary[key][material.InheritFrom])
				{
					if (item.Key == material.InheritFrom)
					{
						materialProperties = item.Value;
					}
					else if (m_materialDictionary[key][subtypeId].ContainsKey(item.Key))
					{
						if (!m_materialDictionary[key][item.Key].ContainsKey(subtypeId))
						{
							m_materialDictionary[key][item.Key][subtypeId] = item.Value;
						}
					}
					else
					{
						m_materialDictionary[key][subtypeId][item.Key] = item.Value;
						m_materialDictionary[key][item.Key][subtypeId] = item.Value;
					}
				}
				if (materialProperties.HasValue)
				{
					m_materialDictionary[key][subtypeId][subtypeId] = materialProperties.Value;
					m_materialDictionary[key][subtypeId][material.InheritFrom] = materialProperties.Value;
					m_materialDictionary[key][material.InheritFrom][subtypeId] = materialProperties.Value;
				}
			}
		}

		private void LoadMaterialProperties(MyPhysicalMaterialDefinition material)
		{
			MyStringHash subtypeId = material.Id.SubtypeId;
			foreach (KeyValuePair<MyStringId, Dictionary<MyStringHash, MyPhysicalMaterialDefinition.CollisionProperty>> collisionProperty in material.CollisionProperties)
			{
				MyStringId key = collisionProperty.Key;
				if (!m_materialDictionary.ContainsKey(key))
				{
					m_materialDictionary[key] = new Dictionary<MyStringHash, Dictionary<MyStringHash, MaterialProperties>>(MyStringHash.Comparer);
				}
				if (!m_materialDictionary[key].ContainsKey(subtypeId))
				{
					m_materialDictionary[key][subtypeId] = new Dictionary<MyStringHash, MaterialProperties>(MyStringHash.Comparer);
				}
				foreach (KeyValuePair<MyStringHash, MyPhysicalMaterialDefinition.CollisionProperty> item in collisionProperty.Value)
				{
					m_materialDictionary[key][subtypeId][item.Key] = new MaterialProperties(item.Value.Sound, item.Value.ParticleEffect, item.Value.ImpactSoundCues);
					if (!m_materialDictionary[key].ContainsKey(item.Key))
					{
						m_materialDictionary[key][item.Key] = new Dictionary<MyStringHash, MaterialProperties>(MyStringHash.Comparer);
					}
					if (!m_materialDictionary[key][item.Key].ContainsKey(subtypeId))
					{
						m_materialDictionary[key][item.Key][subtypeId] = new MaterialProperties(item.Value.Sound, item.Value.ParticleEffect, item.Value.ImpactSoundCues);
					}
				}
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_materialDictionary.Clear();
			Session = null;
			Static = null;
		}

		public bool TryCreateCollisionEffect(MyStringId type, Vector3D position, Vector3 normal, MyStringHash material1, MyStringHash material2, IMyEntity entity)
		{
			string collisionEffect = GetCollisionEffect(type, material1, material2);
			if (collisionEffect != null)
			{
				MatrixD effectMatrix = MatrixD.CreateWorld(position, normal, Vector3.CalculatePerpendicularVector(normal));
				MyParticleEffect effect;
				if (entity != null && !(entity is MyVoxelBase) && !(entity is MySafeZone))
				{
					MyEntity myEntity = entity as MyEntity;
					effectMatrix *= myEntity.PositionComp.WorldMatrixNormalizedInv;
					return MyParticlesManager.TryCreateParticleEffect(collisionEffect, ref effectMatrix, ref position, myEntity.Render.RenderObjectIDs[0], out effect);
				}
				return MyParticlesManager.TryCreateParticleEffect(collisionEffect, ref effectMatrix, ref position, uint.MaxValue, out effect);
			}
			return false;
		}

		public string GetCollisionEffect(MyStringId type, MyStringHash materialType1, MyStringHash materialType2)
		{
			string result = null;
			if (m_materialDictionary.TryGetValue(type, out var value) && value.TryGetValue(materialType1, out var value2) && value2.TryGetValue(materialType2, out var value3))
			{
				result = value3.ParticleEffectName;
			}
			return result;
		}

		public MySoundPair GetCollisionCue(MyStringId type, MyStringHash materialType1, MyStringHash materialType2)
		{
			if (m_materialDictionary.TryGetValue(type, out var value) && value.TryGetValue(materialType1, out var value2) && value2.TryGetValue(materialType2, out var value3))
			{
				return value3.Sound;
			}
			return MySoundPair.Empty;
		}

		public MySoundPair GetCollisionCueWithMass(MyStringId type, MyStringHash materialType1, MyStringHash materialType2, ref float volume, float? mass = null, float velocity = 0f)
		{
			if (m_materialDictionary.TryGetValue(type, out var value) && value.TryGetValue(materialType1, out var value2) && value2.TryGetValue(materialType2, out var value3))
			{
				if (!mass.HasValue || value3.ImpactSoundCues == null || value3.ImpactSoundCues.Count == 0)
				{
					return value3.Sound;
				}
				int num = -1;
				float num2 = -1f;
				for (int i = 0; i < value3.ImpactSoundCues.Count; i++)
				{
					if (mass >= value3.ImpactSoundCues[i].Mass && value3.ImpactSoundCues[i].Mass > num2 && velocity >= value3.ImpactSoundCues[i].minVelocity)
					{
						num = i;
						num2 = value3.ImpactSoundCues[i].Mass;
					}
				}
				if (num >= 0)
				{
					volume = 0.25f + 0.75f * MyMath.Clamp((velocity - value3.ImpactSoundCues[num].minVelocity) / (value3.ImpactSoundCues[num].maxVolumeVelocity - value3.ImpactSoundCues[num].minVelocity), 0f, 1f);
					return value3.ImpactSoundCues[num].SoundCue;
				}
				return value3.Sound;
			}
			return MySoundPair.Empty;
		}
	}
}
