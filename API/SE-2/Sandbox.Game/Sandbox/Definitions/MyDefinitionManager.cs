using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using ParallelTasks;
using Sandbox.AppCode;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.AI.Pathfinding.Obsolete;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Data;
using VRage.Data.Audio;
using VRage.FileSystem;
using VRage.Filesystem.FindFilesRegEx;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.Definitions.Animation;
using VRage.Game.Factions.Definitions;
using VRage.Game.Graphics;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Render.Particles;
using VRage.Stats;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;
using VRageRender.Messages;
using VRageRender.Utils;

namespace Sandbox.Definitions
{
	[PreloadRequired]
	public class MyDefinitionManager : MyDefinitionManagerBase
	{
		internal class DefinitionDictionary<V> : Dictionary<MyDefinitionId, V> where V : MyDefinitionBase
		{
			public DefinitionDictionary(int capacity)
				: base(capacity, (IEqualityComparer<MyDefinitionId>)MyDefinitionId.Comparer)
			{
			}

			public void AddDefinitionSafe<T>(T definition, MyModContext context, string file, bool checkDuplicates = false) where T : V
			{
				if (definition.Id.TypeId != MyObjectBuilderType.Invalid)
				{
					if ((checkDuplicates || context.IsBaseGame) && ContainsKey(definition.Id))
					{
						string msg = string.Concat("Duplicate definition ", definition.Id, " in ", file);
						MyLog.Default.WriteLine(msg);
					}
					base[definition.Id] = (V)definition;
				}
				else
				{
					MyDefinitionErrors.Add(context, "Invalid definition id", TErrorSeverity.Error);
				}
			}

			public void Merge(DefinitionDictionary<V> other)
			{
				foreach (KeyValuePair<MyDefinitionId, V> item in other)
				{
					if (item.Value.Enabled)
					{
						base[item.Key] = item.Value;
					}
					else
					{
						Remove(item.Key);
					}
				}
			}
		}

		internal class DefinitionSet : MyDefinitionSet
		{
			private static DefinitionDictionary<MyDefinitionBase> m_helperDict = new DefinitionDictionary<MyDefinitionBase>(100);

			internal float[] m_cubeSizes;

			internal float[] m_cubeSizesOriginal;

			internal string[] m_basePrefabNames;

			internal DefinitionDictionary<MyCubeBlockDefinition>[] m_uniqueCubeBlocksBySize;

			internal DefinitionDictionary<MyDefinitionBase> m_definitionsById;

			internal DefinitionDictionary<MyBlueprintDefinitionBase> m_blueprintsById;

			internal DefinitionDictionary<MyHandItemDefinition> m_handItemsById;

			internal DefinitionDictionary<MyPhysicalItemDefinition> m_physicalItemsByHandItemId;

			internal DefinitionDictionary<MyHandItemDefinition> m_handItemsByPhysicalItemId;

			internal Dictionary<string, MyPhysicalMaterialDefinition> m_physicalMaterialsByName = new Dictionary<string, MyPhysicalMaterialDefinition>();

			internal Dictionary<string, MyVoxelMaterialDefinition> m_voxelMaterialsByName;

			internal Dictionary<byte, MyVoxelMaterialDefinition> m_voxelMaterialsByIndex;

			internal int m_voxelMaterialRareCount;

			internal List<MyPhysicalItemDefinition> m_physicalGunItemDefinitions;

			internal List<MyPhysicalItemDefinition> m_physicalConsumableItemDefinitions;

			internal DefinitionDictionary<MyWeaponDefinition> m_weaponDefinitionsById;

			internal DefinitionDictionary<MyAmmoDefinition> m_ammoDefinitionsById;

			internal List<MySpawnGroupDefinition> m_spawnGroupDefinitions;

			internal DefinitionDictionary<MyContainerTypeDefinition> m_containerTypeDefinitions;

			internal List<MyScenarioDefinition> m_scenarioDefinitions;

			internal Dictionary<string, MyCharacterDefinition> m_characters;

			internal Dictionary<string, Dictionary<string, MyAnimationDefinition>> m_animationsBySkeletonType;

			internal DefinitionDictionary<MyBlueprintClassDefinition> m_blueprintClasses;

			internal List<MyGuiBlockCategoryDefinition> m_categoryClasses;

			internal Dictionary<string, MyGuiBlockCategoryDefinition> m_categories;

			internal HashSet<BlueprintClassEntry> m_blueprintClassEntries;

			internal HashSet<EnvironmentItemsEntry> m_environmentItemsEntries;

			internal HashSet<MyComponentBlockEntry> m_componentBlockEntries;

			public HashSet<MyDefinitionId> m_componentBlocks;

			public Dictionary<MyDefinitionId, MyCubeBlockDefinition> m_componentIdToBlock;

			internal DefinitionDictionary<MyBlueprintDefinitionBase> m_blueprintsByResultId;

			internal Dictionary<string, MyPrefabDefinition> m_prefabs;

			internal Dictionary<string, MyRespawnShipDefinition> m_respawnShips;

			internal Dictionary<string, MyDropContainerDefinition> m_dropContainers;

			internal Dictionary<string, MyBlockVariantGroup> m_blockVariantGroups;

			internal DefinitionDictionary<MyAssetModifierDefinition> m_assetModifiers;

			internal Dictionary<MyStringHash, MyAssetModifiers> m_assetModifiersForRender;

			internal Dictionary<string, MyWheelModelsDefinition> m_wheelModels;

			internal Dictionary<string, MyAsteroidGeneratorDefinition> m_asteroidGenerators;

			/// <summary>
			/// Block pairs Small,Large
			/// </summary>
			internal Dictionary<string, MyCubeBlockDefinitionGroup> m_blockGroups;

			internal Dictionary<string, Vector2I> m_blockPositions;

			internal DefinitionDictionary<MyAudioDefinition> m_sounds;

			internal DefinitionDictionary<MyShipSoundsDefinition> m_shipSounds;

			internal MyShipSoundSystemDefinition m_shipSoundSystem = new MyShipSoundSystemDefinition();

			internal DefinitionDictionary<MyBehaviorDefinition> m_behaviorDefinitions;

			public Dictionary<string, MyVoxelMapStorageDefinition> m_voxelMapStorages;

			public readonly Dictionary<int, List<MyDefinitionId>> m_channelEnvironmentItemsDefs = new Dictionary<int, List<MyDefinitionId>>();

			internal List<MyCharacterName> m_characterNames;

			internal DefinitionDictionary<MyPlanetGeneratorDefinition> m_planetGeneratorDefinitions;

			internal DefinitionDictionary<MyComponentGroupDefinition> m_componentGroups;

			internal Dictionary<MyDefinitionId, MyTuple<int, MyComponentGroupDefinition>> m_componentGroupMembers;

			internal DefinitionDictionary<MyPlanetPrefabDefinition> m_planetPrefabDefinitions;

			internal Dictionary<string, Dictionary<string, MyGroupedIds>> m_groupedIds;

			internal DefinitionDictionary<MyPirateAntennaDefinition> m_pirateAntennaDefinitions;

			internal MyDestructionDefinition m_destructionDefinition;

			internal Dictionary<string, MyCubeBlockDefinition> m_mapMultiBlockDefToCubeBlockDef = new Dictionary<string, MyCubeBlockDefinition>();

			internal Dictionary<string, MyFactionDefinition> m_factionDefinitionsByTag = new Dictionary<string, MyFactionDefinition>(StringComparer.InvariantCultureIgnoreCase);

			internal DefinitionDictionary<MyGridCreateToolDefinition> m_gridCreateDefinitions;

			internal DefinitionDictionary<MyComponentDefinitionBase> m_entityComponentDefinitions;

			internal DefinitionDictionary<MyContainerDefinition> m_entityContainers;

			internal MyLootBagDefinition m_lootBagDefinition;

			internal DefinitionDictionary<MyMainMenuInventorySceneDefinition> m_mainMenuInventoryScenes;

			internal DefinitionDictionary<MyResearchGroupDefinition> m_researchGroupsDefinitions;

			internal DefinitionDictionary<MyResearchBlockDefinition> m_researchBlocksDefinitions;

			internal DefinitionDictionary<MyRadialMenu> m_radialMenuDefinitions;

			internal MyOffensiveWordsDefinition m_offensiveWordsDefinition;

			internal DefinitionDictionary<MyContractTypeDefinition> m_contractTypesDefinitions;

			internal DefinitionDictionary<MyFactionNameDefinition> m_factionNameDefinitions;

			internal DefinitionDictionary<MyWeatherEffectDefinition> m_weatherEffectsDefinitions;

<<<<<<< HEAD
			internal DefinitionDictionary<MyControllerSchemeDefinition> m_controllerSchemeDefinitions;

			internal DefinitionDictionary<MyTargetingGroupDefinition> m_targetingGroupDefinitions;
=======
			internal DefinitionDictionary<MyChatBotResponseDefinition> m_chatBotResponseDefinitions;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

			public DefinitionSet()
			{
				Clear();
			}

			public void Clear(bool unload = false)
			{
				base.Clear();
				m_cubeSizes = new float[typeof(MyCubeSize).GetEnumValues().Length];
				m_cubeSizesOriginal = new float[typeof(MyCubeSize).GetEnumValues().Length];
				m_basePrefabNames = new string[m_cubeSizes.Length * 4];
				m_definitionsById = new DefinitionDictionary<MyDefinitionBase>(100);
				m_voxelMaterialsByName = new Dictionary<string, MyVoxelMaterialDefinition>(10);
				m_voxelMaterialsByIndex = new Dictionary<byte, MyVoxelMaterialDefinition>(10);
				m_voxelMaterialRareCount = 0;
				m_physicalGunItemDefinitions = new List<MyPhysicalItemDefinition>(10);
				m_physicalConsumableItemDefinitions = new List<MyPhysicalItemDefinition>(10);
				m_weaponDefinitionsById = new DefinitionDictionary<MyWeaponDefinition>(10);
				m_ammoDefinitionsById = new DefinitionDictionary<MyAmmoDefinition>(10);
				m_blockPositions = new Dictionary<string, Vector2I>(10);
				m_uniqueCubeBlocksBySize = new DefinitionDictionary<MyCubeBlockDefinition>[m_cubeSizes.Length];
				for (int i = 0; i < m_cubeSizes.Length; i++)
				{
					m_uniqueCubeBlocksBySize[i] = new DefinitionDictionary<MyCubeBlockDefinition>(10);
				}
				m_blueprintsById = new DefinitionDictionary<MyBlueprintDefinitionBase>(10);
				m_spawnGroupDefinitions = new List<MySpawnGroupDefinition>(10);
				m_containerTypeDefinitions = new DefinitionDictionary<MyContainerTypeDefinition>(10);
				m_handItemsById = new DefinitionDictionary<MyHandItemDefinition>(10);
				m_physicalItemsByHandItemId = new DefinitionDictionary<MyPhysicalItemDefinition>(m_handItemsById.Count);
				m_handItemsByPhysicalItemId = new DefinitionDictionary<MyHandItemDefinition>(m_handItemsById.Count);
				m_scenarioDefinitions = new List<MyScenarioDefinition>(10);
				m_characters = new Dictionary<string, MyCharacterDefinition>();
				m_animationsBySkeletonType = new Dictionary<string, Dictionary<string, MyAnimationDefinition>>();
				if (m_blueprintClasses != null)
				{
					foreach (MyBlueprintClassDefinition value in m_blueprintClasses.Values)
					{
						value.ClearBlueprints();
					}
				}
				m_blueprintClasses = new DefinitionDictionary<MyBlueprintClassDefinition>(10);
				m_blueprintClassEntries = new HashSet<BlueprintClassEntry>();
				m_blueprintsByResultId = new DefinitionDictionary<MyBlueprintDefinitionBase>(10);
				m_assetModifiers = new DefinitionDictionary<MyAssetModifierDefinition>(10);
				m_mainMenuInventoryScenes = new DefinitionDictionary<MyMainMenuInventorySceneDefinition>(10);
				m_environmentItemsEntries = new HashSet<EnvironmentItemsEntry>();
				m_componentBlockEntries = new HashSet<MyComponentBlockEntry>();
				m_componentBlocks = new HashSet<MyDefinitionId>((IEqualityComparer<MyDefinitionId>)MyDefinitionId.Comparer);
				m_componentIdToBlock = new Dictionary<MyDefinitionId, MyCubeBlockDefinition>(MyDefinitionId.Comparer);
				m_categoryClasses = new List<MyGuiBlockCategoryDefinition>(25);
				m_categories = new Dictionary<string, MyGuiBlockCategoryDefinition>(25);
				m_prefabs = new Dictionary<string, MyPrefabDefinition>();
				m_respawnShips = new Dictionary<string, MyRespawnShipDefinition>();
				m_dropContainers = new Dictionary<string, MyDropContainerDefinition>();
				if (m_blockVariantGroups != null)
				{
					foreach (MyBlockVariantGroup value2 in m_blockVariantGroups.Values)
					{
						value2.CleanUp();
					}
				}
				m_blockVariantGroups = new Dictionary<string, MyBlockVariantGroup>();
				m_wheelModels = new Dictionary<string, MyWheelModelsDefinition>();
				m_asteroidGenerators = new Dictionary<string, MyAsteroidGeneratorDefinition>();
				m_sounds = new DefinitionDictionary<MyAudioDefinition>(10);
				m_shipSounds = new DefinitionDictionary<MyShipSoundsDefinition>(10);
				m_behaviorDefinitions = new DefinitionDictionary<MyBehaviorDefinition>(10);
				m_voxelMapStorages = new Dictionary<string, MyVoxelMapStorageDefinition>(64);
				m_characterNames = new List<MyCharacterName>(32);
				m_planetGeneratorDefinitions = new DefinitionDictionary<MyPlanetGeneratorDefinition>(5);
				m_componentGroups = new DefinitionDictionary<MyComponentGroupDefinition>(4);
				m_componentGroupMembers = new Dictionary<MyDefinitionId, MyTuple<int, MyComponentGroupDefinition>>();
				m_planetPrefabDefinitions = new DefinitionDictionary<MyPlanetPrefabDefinition>(5);
				m_groupedIds = new Dictionary<string, Dictionary<string, MyGroupedIds>>();
				m_pirateAntennaDefinitions = new DefinitionDictionary<MyPirateAntennaDefinition>(4);
				m_destructionDefinition = new MyDestructionDefinition();
				m_mapMultiBlockDefToCubeBlockDef = new Dictionary<string, MyCubeBlockDefinition>();
				m_factionDefinitionsByTag.Clear();
				m_gridCreateDefinitions = new DefinitionDictionary<MyGridCreateToolDefinition>(3);
				m_entityComponentDefinitions = new DefinitionDictionary<MyComponentDefinitionBase>(10);
				m_entityContainers = new DefinitionDictionary<MyContainerDefinition>(10);
				if (unload)
				{
					m_physicalMaterialsByName = new Dictionary<string, MyPhysicalMaterialDefinition>();
				}
				m_lootBagDefinition = null;
				m_researchBlocksDefinitions = new DefinitionDictionary<MyResearchBlockDefinition>(250);
				m_researchGroupsDefinitions = new DefinitionDictionary<MyResearchGroupDefinition>(30);
				m_radialMenuDefinitions = new DefinitionDictionary<MyRadialMenu>(10);
				m_offensiveWordsDefinition = new MyOffensiveWordsDefinition();
				m_contractTypesDefinitions = new DefinitionDictionary<MyContractTypeDefinition>(30);
				m_factionNameDefinitions = new DefinitionDictionary<MyFactionNameDefinition>(30);
				m_weatherEffectsDefinitions = new DefinitionDictionary<MyWeatherEffectDefinition>(30);
<<<<<<< HEAD
				m_controllerSchemeDefinitions = new DefinitionDictionary<MyControllerSchemeDefinition>(300);
				m_targetingGroupDefinitions = new DefinitionDictionary<MyTargetingGroupDefinition>(10);
=======
				m_chatBotResponseDefinitions = new DefinitionDictionary<MyChatBotResponseDefinition>(300);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			public void OverrideBy(DefinitionSet definitionSet, bool isBasic)
			{
<<<<<<< HEAD
				base.OverrideBy(definitionSet, isBasic);
=======
				//IL_0395: Unknown result type (might be due to invalid IL or missing references)
				//IL_039a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0416: Unknown result type (might be due to invalid IL or missing references)
				//IL_041b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0486: Unknown result type (might be due to invalid IL or missing references)
				//IL_048b: Unknown result type (might be due to invalid IL or missing references)
				base.OverrideBy(definitionSet);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				foreach (KeyValuePair<MyDefinitionId, MyGridCreateToolDefinition> gridCreateDefinition in definitionSet.m_gridCreateDefinitions)
				{
					m_gridCreateDefinitions[gridCreateDefinition.Key] = gridCreateDefinition.Value;
				}
				for (int i = 0; i < definitionSet.m_cubeSizes.Length; i++)
				{
					float num = definitionSet.m_cubeSizes[i];
					if (num != 0f)
					{
						m_cubeSizes[i] = num;
						m_cubeSizesOriginal[i] = definitionSet.m_cubeSizesOriginal[i];
					}
				}
				for (int j = 0; j < definitionSet.m_basePrefabNames.Length; j++)
				{
					if (!string.IsNullOrEmpty(definitionSet.m_basePrefabNames[j]))
					{
						m_basePrefabNames[j] = definitionSet.m_basePrefabNames[j];
					}
				}
				m_definitionsById.Merge(definitionSet.m_definitionsById);
				foreach (KeyValuePair<string, MyVoxelMaterialDefinition> item in definitionSet.m_voxelMaterialsByName)
				{
					m_voxelMaterialsByName[item.Key] = item.Value;
				}
				foreach (KeyValuePair<string, MyPhysicalMaterialDefinition> item2 in definitionSet.m_physicalMaterialsByName)
				{
					m_physicalMaterialsByName[item2.Key] = item2.Value;
				}
				MergeDefinitionLists(m_physicalGunItemDefinitions, definitionSet.m_physicalGunItemDefinitions);
				MergeDefinitionLists(m_physicalConsumableItemDefinitions, definitionSet.m_physicalConsumableItemDefinitions);
				foreach (KeyValuePair<string, Vector2I> blockPosition in definitionSet.m_blockPositions)
				{
					m_blockPositions[blockPosition.Key] = blockPosition.Value;
				}
				for (int k = 0; k < definitionSet.m_uniqueCubeBlocksBySize.Length; k++)
				{
					foreach (KeyValuePair<MyDefinitionId, MyCubeBlockDefinition> item3 in definitionSet.m_uniqueCubeBlocksBySize[k])
					{
						m_uniqueCubeBlocksBySize[k][item3.Key] = item3.Value;
					}
				}
				m_blueprintsById.Merge(definitionSet.m_blueprintsById);
				MergeDefinitionLists(m_spawnGroupDefinitions, definitionSet.m_spawnGroupDefinitions);
				m_containerTypeDefinitions.Merge(definitionSet.m_containerTypeDefinitions);
				m_handItemsById.Merge(definitionSet.m_handItemsById);
				MergeDefinitionLists(m_scenarioDefinitions, definitionSet.m_scenarioDefinitions);
				foreach (KeyValuePair<string, MyCharacterDefinition> character in definitionSet.m_characters)
				{
					if (character.Value.Enabled)
					{
						m_characters[character.Key] = character.Value;
					}
					else
					{
						m_characters.Remove(character.Key);
					}
				}
				m_blueprintClasses.Merge(definitionSet.m_blueprintClasses);
				foreach (MyGuiBlockCategoryDefinition categoryClass in definitionSet.m_categoryClasses)
				{
					m_categoryClasses.Add(categoryClass);
					string name = categoryClass.Name;
					MyGuiBlockCategoryDefinition value = null;
					if (!m_categories.TryGetValue(name, out value))
					{
						m_categories.Add(name, categoryClass);
					}
					else
					{
						value.ItemIds.UnionWith((IEnumerable<string>)categoryClass.ItemIds);
					}
				}
				Enumerator<BlueprintClassEntry> enumerator8 = definitionSet.m_blueprintClassEntries.GetEnumerator();
				try
				{
					while (enumerator8.MoveNext())
					{
						BlueprintClassEntry current8 = enumerator8.get_Current();
						if (m_blueprintClassEntries.Contains(current8))
						{
							if (!current8.Enabled)
							{
								m_blueprintClassEntries.Remove(current8);
							}
						}
						else if (current8.Enabled)
						{
							m_blueprintClassEntries.Add(current8);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator8).Dispose();
				}
				m_blueprintsByResultId.Merge(definitionSet.m_blueprintsByResultId);
				Enumerator<EnvironmentItemsEntry> enumerator9 = definitionSet.m_environmentItemsEntries.GetEnumerator();
				try
				{
					while (enumerator9.MoveNext())
					{
						EnvironmentItemsEntry current9 = enumerator9.get_Current();
						if (m_environmentItemsEntries.Contains(current9))
						{
							if (!current9.Enabled)
							{
								m_environmentItemsEntries.Remove(current9);
							}
						}
						else if (current9.Enabled)
						{
							m_environmentItemsEntries.Add(current9);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator9).Dispose();
				}
				Enumerator<MyComponentBlockEntry> enumerator10 = definitionSet.m_componentBlockEntries.GetEnumerator();
				try
				{
					while (enumerator10.MoveNext())
					{
						MyComponentBlockEntry current10 = enumerator10.get_Current();
						if (m_componentBlockEntries.Contains(current10))
						{
							if (!current10.Enabled)
							{
								m_componentBlockEntries.Remove(current10);
							}
						}
						else if (current10.Enabled)
						{
							m_componentBlockEntries.Add(current10);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator10).Dispose();
				}
				foreach (KeyValuePair<string, MyPrefabDefinition> prefab in definitionSet.m_prefabs)
				{
					if (prefab.Value.Enabled)
					{
						m_prefabs[prefab.Key] = prefab.Value;
					}
					else
					{
						m_prefabs.Remove(prefab.Key);
					}
				}
				foreach (KeyValuePair<string, MyRespawnShipDefinition> respawnShip in definitionSet.m_respawnShips)
				{
					if (respawnShip.Value.Enabled)
					{
						m_respawnShips[respawnShip.Key] = respawnShip.Value;
					}
					else
					{
						m_respawnShips.Remove(respawnShip.Key);
					}
				}
				foreach (KeyValuePair<string, MyDropContainerDefinition> dropContainer in definitionSet.m_dropContainers)
				{
					if (dropContainer.Value.Enabled)
					{
						m_dropContainers[dropContainer.Key] = dropContainer.Value;
					}
					else
					{
						m_dropContainers.Remove(dropContainer.Key);
					}
				}
				foreach (KeyValuePair<string, MyBlockVariantGroup> blockVariantGroup in definitionSet.m_blockVariantGroups)
				{
					if (blockVariantGroup.Value.Enabled)
					{
						m_blockVariantGroups[blockVariantGroup.Key] = blockVariantGroup.Value;
					}
					else
					{
						m_blockVariantGroups.Remove(blockVariantGroup.Key);
					}
				}
				foreach (KeyValuePair<string, MyWheelModelsDefinition> wheelModel in definitionSet.m_wheelModels)
				{
					if (wheelModel.Value.Enabled)
					{
						m_wheelModels[wheelModel.Key] = wheelModel.Value;
					}
					else
					{
						m_wheelModels.Remove(wheelModel.Key);
					}
				}
				foreach (KeyValuePair<string, MyAsteroidGeneratorDefinition> asteroidGenerator in definitionSet.m_asteroidGenerators)
				{
					if (asteroidGenerator.Value.Enabled)
					{
						m_asteroidGenerators[asteroidGenerator.Key] = asteroidGenerator.Value;
					}
					else
					{
						m_asteroidGenerators.Remove(asteroidGenerator.Key);
					}
				}
				foreach (KeyValuePair<MyDefinitionId, MyAssetModifierDefinition> assetModifier in definitionSet.m_assetModifiers)
				{
					if (assetModifier.Value.Enabled)
					{
						m_assetModifiers[assetModifier.Key] = assetModifier.Value;
					}
					else
					{
						m_assetModifiers.Remove(assetModifier.Key);
					}
				}
				foreach (KeyValuePair<MyDefinitionId, MyMainMenuInventorySceneDefinition> mainMenuInventoryScene in definitionSet.m_mainMenuInventoryScenes)
				{
					if (mainMenuInventoryScene.Value.Enabled)
					{
						m_mainMenuInventoryScenes[mainMenuInventoryScene.Key] = mainMenuInventoryScene.Value;
					}
					else
					{
						m_mainMenuInventoryScenes.Remove(mainMenuInventoryScene.Key);
					}
				}
				foreach (KeyValuePair<string, Dictionary<string, MyAnimationDefinition>> item4 in definitionSet.m_animationsBySkeletonType)
				{
					foreach (KeyValuePair<string, MyAnimationDefinition> item5 in item4.Value)
					{
						if (item5.Value.Enabled)
						{
							if (!m_animationsBySkeletonType.ContainsKey(item4.Key))
							{
								m_animationsBySkeletonType[item4.Key] = new Dictionary<string, MyAnimationDefinition>();
							}
							m_animationsBySkeletonType[item4.Key][item5.Value.Id.SubtypeName] = item5.Value;
						}
						else
						{
							m_animationsBySkeletonType[item4.Key].Remove(item5.Value.Id.SubtypeName);
						}
					}
				}
				foreach (KeyValuePair<MyDefinitionId, MyAudioDefinition> sound in definitionSet.m_sounds)
				{
					m_sounds[sound.Key] = sound.Value;
				}
				m_weaponDefinitionsById.Merge(definitionSet.m_weaponDefinitionsById);
				m_ammoDefinitionsById.Merge(definitionSet.m_ammoDefinitionsById);
				m_behaviorDefinitions.Merge(definitionSet.m_behaviorDefinitions);
				foreach (KeyValuePair<string, MyVoxelMapStorageDefinition> voxelMapStorage in definitionSet.m_voxelMapStorages)
				{
					m_voxelMapStorages[voxelMapStorage.Key] = voxelMapStorage.Value;
				}
				foreach (MyCharacterName characterName in definitionSet.m_characterNames)
				{
					m_characterNames.Add(characterName);
				}
				m_componentGroups.Merge(definitionSet.m_componentGroups);
				foreach (KeyValuePair<MyDefinitionId, MyPlanetGeneratorDefinition> planetGeneratorDefinition in definitionSet.m_planetGeneratorDefinitions)
				{
					if (planetGeneratorDefinition.Value.Enabled)
					{
						m_planetGeneratorDefinitions[planetGeneratorDefinition.Key] = planetGeneratorDefinition.Value;
					}
					else
					{
						m_planetGeneratorDefinitions.Remove(planetGeneratorDefinition.Key);
					}
				}
				foreach (KeyValuePair<MyDefinitionId, MyPlanetPrefabDefinition> planetPrefabDefinition in definitionSet.m_planetPrefabDefinitions)
				{
					if (planetPrefabDefinition.Value.Enabled)
					{
						m_planetPrefabDefinitions[planetPrefabDefinition.Key] = planetPrefabDefinition.Value;
					}
					else
					{
						m_planetPrefabDefinitions.Remove(planetPrefabDefinition.Key);
					}
				}
				foreach (KeyValuePair<string, Dictionary<string, MyGroupedIds>> groupedId in definitionSet.m_groupedIds)
				{
					if (m_groupedIds.ContainsKey(groupedId.Key))
					{
						Dictionary<string, MyGroupedIds> dictionary = m_groupedIds[groupedId.Key];
						foreach (KeyValuePair<string, MyGroupedIds> item6 in groupedId.Value)
						{
							dictionary[item6.Key] = item6.Value;
						}
					}
					else
					{
						m_groupedIds[groupedId.Key] = groupedId.Value;
					}
				}
				m_pirateAntennaDefinitions.Merge(definitionSet.m_pirateAntennaDefinitions);
				if (definitionSet.m_destructionDefinition != null && definitionSet.m_destructionDefinition.Enabled)
				{
					m_destructionDefinition.Merge(definitionSet.m_destructionDefinition);
				}
				foreach (KeyValuePair<string, MyCubeBlockDefinition> item7 in definitionSet.m_mapMultiBlockDefToCubeBlockDef)
				{
					if (m_mapMultiBlockDefToCubeBlockDef.ContainsKey(item7.Key))
					{
						m_mapMultiBlockDefToCubeBlockDef.Remove(item7.Key);
					}
					m_mapMultiBlockDefToCubeBlockDef.Add(item7.Key, item7.Value);
				}
				m_entityComponentDefinitions.Merge(definitionSet.m_entityComponentDefinitions);
				m_entityContainers.Merge(definitionSet.m_entityContainers);
				m_lootBagDefinition = definitionSet.m_lootBagDefinition;
				m_researchBlocksDefinitions.Merge(definitionSet.m_researchBlocksDefinitions);
				m_researchGroupsDefinitions.Merge(definitionSet.m_researchGroupsDefinitions);
				m_radialMenuDefinitions.Merge(definitionSet.m_radialMenuDefinitions);
				m_offensiveWordsDefinition.Blacklist.AddRange(definitionSet.m_offensiveWordsDefinition.Blacklist);
				m_contractTypesDefinitions.Merge(definitionSet.m_contractTypesDefinitions);
				m_factionNameDefinitions.Merge(definitionSet.m_factionNameDefinitions);
				m_weatherEffectsDefinitions.Merge(definitionSet.m_weatherEffectsDefinitions);
<<<<<<< HEAD
				if (isBasic)
				{
					m_controllerSchemeDefinitions.Merge(definitionSet.m_controllerSchemeDefinitions);
				}
				m_targetingGroupDefinitions.Merge(definitionSet.m_targetingGroupDefinitions);
=======
				m_chatBotResponseDefinitions.Merge(definitionSet.m_chatBotResponseDefinitions);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}

			private static void MergeDefinitionLists<T>(List<T> output, List<T> input) where T : MyDefinitionBase
			{
				m_helperDict.Clear();
				foreach (T item in output)
				{
					m_helperDict[item.Id] = item;
				}
				foreach (T item2 in input)
				{
					if (item2.Enabled)
					{
						m_helperDict[item2.Id] = item2;
					}
					else
					{
						m_helperDict.Remove(item2.Id);
					}
				}
				output.Clear();
				foreach (MyDefinitionBase value in m_helperDict.Values)
				{
					output.Add((T)value);
				}
				m_helperDict.Clear();
			}
		}

		private class SoundsData : WorkData
		{
			public ListReader<MySoundData> SoundData { get; set; }

			public ListReader<MyAudioEffect> EffectData { get; set; }
		}

		public struct MyAssetModifiers
		{
			public Dictionary<string, MyTextureChange> SkinTextureChanges;

			public bool MetalnessColorable;
		}

		private Dictionary<string, DefinitionSet> m_modDefinitionSets = new Dictionary<string, DefinitionSet>();

		private DefinitionSet m_currentLoadingSet;

		private const string DUPLICATE_ENTRY_MESSAGE = "Duplicate entry of '{0}'";

		private const string UNKNOWN_ENTRY_MESSAGE = "Unknown type '{0}'";

		private const string WARNING_ON_REDEFINITION_MESSAGE = "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'";

		private bool m_transparentMaterialsInitialized;

		private List<MyObjectBuilder_DecalDefinition> m_decalObjectBuilders = new List<MyObjectBuilder_DecalDefinition>();

		private ConcurrentDictionary<string, MyObjectBuilder_Definitions> m_preloadedDefinitionBuilders = new ConcurrentDictionary<string, MyObjectBuilder_Definitions>();

		private FastResourceLock m_voxelMaterialsLock = new FastResourceLock();

		private Lazy<List<MyVoxelMapStorageDefinition>> m_voxelMapStorageDefinitionsForProceduralRemovals;

		private Lazy<List<MyVoxelMapStorageDefinition>> m_voxelMapStorageDefinitionsForProceduralAdditions;

		private Lazy<List<MyVoxelMapStorageDefinition>> m_voxelMapStorageDefinitionsForProceduralPrimaryAdditions;

		private Lazy<List<MyDefinitionBase>> m_inventoryItemDefinitions;

		private static Dictionary<string, bool> m_directoryExistCache;

		public new static MyDefinitionManager Static => MyDefinitionManagerBase.Static as MyDefinitionManager;

		private new DefinitionSet m_definitions => (DefinitionSet)base.m_definitions;

		/// Return the definition set currently being loaded.
		internal DefinitionSet LoadingSet => m_currentLoadingSet;

		public bool Loading { get; private set; }

		public MyEnvironmentDefinition EnvironmentDefinition => MySector.EnvironmentDefinition;

		public DictionaryValuesReader<string, MyCharacterDefinition> Characters => new DictionaryValuesReader<string, MyCharacterDefinition>(m_definitions.m_characters);

		public MyShipSoundSystemDefinition GetShipSoundSystemDefinition => m_definitions.m_shipSoundSystem;

		public int VoxelMaterialCount
		{
			get
			{
				using (m_voxelMaterialsLock.AcquireSharedUsing())
				{
					return m_definitions.m_voxelMaterialsByName.Count;
				}
			}
		}

		public int VoxelMaterialRareCount => m_definitions.m_voxelMaterialRareCount;

		public MyDestructionDefinition DestructionDefinition => m_definitions.m_destructionDefinition;

		public override MyDefinitionSet GetLoadingSet()
		{
			return LoadingSet;
		}

		static MyDefinitionManager()
		{
			m_directoryExistCache = new Dictionary<string, bool>();
			MyDefinitionManagerBase.Static = new MyDefinitionManager();
		}

		private MyDefinitionManager()
		{
			Loading = false;
			base.m_definitions = new DefinitionSet();
			m_voxelMapStorageDefinitionsForProceduralRemovals = new Lazy<List<MyVoxelMapStorageDefinition>>(() => Enumerable.ToList<MyVoxelMapStorageDefinition>(Enumerable.Where<MyVoxelMapStorageDefinition>((IEnumerable<MyVoxelMapStorageDefinition>)m_definitions.m_voxelMapStorages.Values, (Func<MyVoxelMapStorageDefinition, bool>)((MyVoxelMapStorageDefinition x) => x.UseForProceduralRemovals))), LazyThreadSafetyMode.PublicationOnly);
			m_voxelMapStorageDefinitionsForProceduralAdditions = new Lazy<List<MyVoxelMapStorageDefinition>>(() => Enumerable.ToList<MyVoxelMapStorageDefinition>(Enumerable.Where<MyVoxelMapStorageDefinition>((IEnumerable<MyVoxelMapStorageDefinition>)m_definitions.m_voxelMapStorages.Values, (Func<MyVoxelMapStorageDefinition, bool>)((MyVoxelMapStorageDefinition x) => x.UseForProceduralAdditions))), LazyThreadSafetyMode.PublicationOnly);
			m_voxelMapStorageDefinitionsForProceduralPrimaryAdditions = new Lazy<List<MyVoxelMapStorageDefinition>>(() => Enumerable.ToList<MyVoxelMapStorageDefinition>(Enumerable.Where<MyVoxelMapStorageDefinition>((IEnumerable<MyVoxelMapStorageDefinition>)m_definitions.m_voxelMapStorages.Values, (Func<MyVoxelMapStorageDefinition, bool>)((MyVoxelMapStorageDefinition x) => x.UseAsPrimaryProceduralAdditionShape))), LazyThreadSafetyMode.PublicationOnly);
			m_inventoryItemDefinitions = new Lazy<List<MyDefinitionBase>>(() => Enumerable.ToList<MyDefinitionBase>(Enumerable.Where<MyDefinitionBase>((IEnumerable<MyDefinitionBase>)m_definitions.m_definitionsById.Values, (Func<MyDefinitionBase, bool>)delegate(MyDefinitionBase x)
			{
				Type type = x.Id.TypeId;
				return type == typeof(MyObjectBuilder_Ore) || type == typeof(MyObjectBuilder_Ingot) || type == typeof(MyObjectBuilder_Component) || type == typeof(MyObjectBuilder_AmmoMagazine) || type == typeof(MyObjectBuilder_PhysicalGunObject) || type == typeof(MyObjectBuilder_GasContainerObject) || type == typeof(MyObjectBuilder_OxygenContainerObject);
<<<<<<< HEAD
			}).ToList(), LazyThreadSafetyMode.PublicationOnly);
=======
			})), LazyThreadSafetyMode.PublicationOnly);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void PreloadDefinitions()
		{
			MySandboxGame.Log.WriteLine("MyDefinitionManager.PreloadDefinitions() - START");
			m_definitions.Clear();
			using (MySandboxGame.Log.IndentUsing())
			{
				if (!m_modDefinitionSets.ContainsKey(""))
				{
					m_modDefinitionSets.Add("", new DefinitionSet());
				}
				DefinitionSet definitionSet = m_modDefinitionSets[""];
				LoadDefinitions(MyModContext.BaseGame, definitionSet, failOnDebug: false, isPreload: true);
			}
			MySandboxGame.Log.WriteLine("MyDefinitionManager.PreloadDefinitions() - END");
		}

		/// <summary>
		/// Get the set of object builders that need additional data preloaded for session.
		/// </summary>
		/// <returns></returns>
		public List<Tuple<MyObjectBuilder_Definitions, string>> GetSessionPreloadDefinitions()
		{
			MySandboxGame.Log.WriteLine("MyDefinitionManager.GetSessionPreloadDefinitions() - START");
			List<Tuple<MyObjectBuilder_Definitions, string>> result = null;
			using (MySandboxGame.Log.IndentUsing())
			{
				if (MyFakes.ENABLE_PRELOAD_DEFINITIONS)
				{
					HashSet<string> preloadSet = GetPreloadSet(MyObjectBuilder_PreloadFileInfo.PreloadType.SessionPreload);
					result = GetDefinitionBuilders(MyModContext.BaseGame, preloadSet);
				}
			}
			MySandboxGame.Log.WriteLine("MyDefinitionManager.GetSessionPreloadDefinitions() - END");
			return result;
		}

		/// <summary>
		/// Load and return all object builders needed for a session.
		/// </summary>
		/// <returns></returns>
		public List<Tuple<MyObjectBuilder_Definitions, string>> GetAllSessionPreloadObjectBuilders()
		{
			MySandboxGame.Log.WriteLine("MyDefinitionManager.GetSessionPreloadDefinitions() - START");
			List<Tuple<MyObjectBuilder_Definitions, string>> result = null;
			using (MySandboxGame.Log.IndentUsing())
			{
				if (MyFakes.ENABLE_PRELOAD_DEFINITIONS)
				{
					result = GetDefinitionBuilders(MyModContext.BaseGame);
				}
			}
			MySandboxGame.Log.WriteLine("MyDefinitionManager.GetSessionPreloadDefinitions() - END");
			return result;
		}

		public void LoadScenarios()
		{
			MySandboxGame.Log.WriteLine("MyDefinitionManager.LoadScenarios() - START");
			MySandboxGame.WaitForPreload();
			using (MySandboxGame.Log.IndentUsing())
			{
				MyDataIntegrityChecker.ResetHash();
				if (!m_modDefinitionSets.ContainsKey(""))
				{
					m_modDefinitionSets.Add("", new DefinitionSet());
				}
				DefinitionSet definitionSet = m_modDefinitionSets[""];
				foreach (MyScenarioDefinition scenarioDefinition in m_definitions.m_scenarioDefinitions)
				{
					definitionSet.m_definitionsById.Remove(scenarioDefinition.Id);
				}
				foreach (MyScenarioDefinition scenarioDefinition2 in m_definitions.m_scenarioDefinitions)
				{
					m_definitions.m_definitionsById.Remove(scenarioDefinition2.Id);
				}
				m_definitions.m_scenarioDefinitions.Clear();
				LoadScenarios(MyModContext.BaseGame, definitionSet);
			}
			MySandboxGame.Log.WriteLine("MyDefinitionManager.LoadScenarios() - END");
		}

		public void ReloadDecalMaterials()
		{
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = Load<MyObjectBuilder_Definitions>(Path.Combine(MyModContext.BaseGame.ModPathData, "Decals.sbc"));
			if (myObjectBuilder_Definitions.Decals != null)
			{
				InitDecals(MyModContext.BaseGame, myObjectBuilder_Definitions.Decals);
			}
			if (myObjectBuilder_Definitions.DecalGlobals != null)
			{
				InitDecalGlobals(MyModContext.BaseGame, myObjectBuilder_Definitions.DecalGlobals);
			}
		}

		public void LoadData(List<MyObjectBuilder_Checkpoint.ModItem> mods)
		{
			MySandboxGame.Log.WriteLine("MyDefinitionManager.LoadData() - START");
			MySandboxGame.WaitForPreload();
			UnloadData();
			Loading = true;
			LoadScenarios();
			using (MySandboxGame.Log.IndentUsing())
			{
				if (!m_modDefinitionSets.ContainsKey(""))
				{
					m_modDefinitionSets.Add("", new DefinitionSet());
				}
				DefinitionSet item = m_modDefinitionSets[""];
				List<MyModContext> list = new List<MyModContext>();
				List<DefinitionSet> list2 = new List<DefinitionSet>();
				list.Add(MyModContext.BaseGame);
				list2.Add(item);
				foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
				{
					MyModContext myModContext = (MyModContext)mod.GetModContext();
					if (!m_modDefinitionSets.ContainsKey(myModContext.ModPath))
					{
						DefinitionSet definitionSet = new DefinitionSet();
						m_modDefinitionSets.Add(myModContext.ModPath, definitionSet);
						list.Add(myModContext);
						list2.Add(definitionSet);
					}
				}
				MySandboxGame.Log.WriteLine($"List of used mods ({mods.Count}) - START");
				MySandboxGame.Log.IncreaseIndent();
				foreach (MyObjectBuilder_Checkpoint.ModItem mod2 in mods)
				{
					MySandboxGame.Log.WriteLine($"Id = {mod2.PublishedServiceName}:{mod2.PublishedFileId}, Filename = '{mod2.Name}', Name = '{mod2.FriendlyName}'");
				}
				MySandboxGame.Log.DecreaseIndent();
				MySandboxGame.Log.WriteLine("List of used mods - END");
				LoadDefinitions(list, list2);
				if (MySandboxGame.Static != null)
				{
					LoadPostProcess();
				}
				if (MyFakes.TEST_MODELS && MyExternalAppBase.Static == null)
				{
					long timestamp = Stopwatch.GetTimestamp();
					TestCubeBlockModels();
					_ = (double)(Stopwatch.GetTimestamp() - timestamp) / (double)Stopwatch.Frequency;
				}
				if (MyFakes.ENABLE_ALL_IN_SURVIVAL)
				{
					foreach (MyPhysicalItemDefinition physicalGunItemDefinition in m_definitions.m_physicalGunItemDefinitions)
					{
						physicalGunItemDefinition.AvailableInSurvival = true;
					}
					foreach (MyPhysicalItemDefinition physicalConsumableItemDefinition in m_definitions.m_physicalConsumableItemDefinitions)
					{
						physicalConsumableItemDefinition.AvailableInSurvival = true;
					}
					foreach (MyBehaviorDefinition value2 in m_definitions.m_behaviorDefinitions.Values)
					{
						value2.AvailableInSurvival = true;
					}
					foreach (MyBehaviorDefinition value3 in m_definitions.m_behaviorDefinitions.Values)
					{
						value3.AvailableInSurvival = true;
					}
					foreach (MyCharacterDefinition value4 in m_definitions.m_characters.Values)
					{
						value4.AvailableInSurvival = true;
					}
				}
				foreach (MyEnvironmentItemsDefinition environmentItemClassDefinition in Static.GetEnvironmentItemClassDefinitions())
				{
					List<MyDefinitionId> value = null;
					if (!m_definitions.m_channelEnvironmentItemsDefs.TryGetValue(environmentItemClassDefinition.Channel, out value))
					{
						value = new List<MyDefinitionId>();
						m_definitions.m_channelEnvironmentItemsDefs[environmentItemClassDefinition.Channel] = value;
					}
					value.Add(environmentItemClassDefinition.Id);
				}
			}
			Loading = false;
			MySandboxGame.Log.WriteLine("MyDefinitionManager.LoadData() - END");
		}

		private void TestCubeBlockModels()
		{
			Parallel.ForEach(GetDefinitionPairNames(), delegate(string pair)
			{
				MyCubeBlockDefinitionGroup definitionGroup = GetDefinitionGroup(pair);
				TestCubeBlockModel(definitionGroup.Small);
				TestCubeBlockModel(definitionGroup.Large);
			});
		}

		private void TestCubeBlockModel(MyCubeBlockDefinition block)
		{
			if (block != null)
			{
				if (block.Model != null)
				{
					TestCubeBlockModel(block.Model);
				}
				MyCubeBlockDefinition.BuildProgressModel[] buildProgressModels = block.BuildProgressModels;
				foreach (MyCubeBlockDefinition.BuildProgressModel buildProgressModel in buildProgressModels)
				{
					TestCubeBlockModel(buildProgressModel.File);
				}
			}
		}

		private void TestCubeBlockModel(string file)
		{
			Path.Combine(MyFileSystem.ContentPath, file);
			MyModel modelOnlyData = MyModels.GetModelOnlyData(file);
			if (MyFakes.TEST_MODELS_WRONG_TRIANGLES)
			{
				int trianglesCount = modelOnlyData.GetTrianglesCount();
				for (int i = 0; i < trianglesCount; i++)
				{
					MyTriangleVertexIndices triangle = modelOnlyData.GetTriangle(i);
					if (MyUtils.IsWrongTriangle(modelOnlyData.GetVertex(triangle.I0), modelOnlyData.GetVertex(triangle.I1), modelOnlyData.GetVertex(triangle.I2)))
					{
						break;
					}
				}
			}
			if (modelOnlyData.LODs != null)
			{
				MyLODDescriptor[] lODs = modelOnlyData.LODs;
				foreach (MyLODDescriptor myLODDescriptor in lODs)
				{
					TestCubeBlockModel(myLODDescriptor.Model);
				}
			}
			modelOnlyData.UnloadData();
		}

		private HashSet<string> GetPreloadSet(MyObjectBuilder_PreloadFileInfo.PreloadType preloadType)
		{
			string path = Path.Combine(MyModContext.BaseGame.ModPathData, "DefinitionsToPreload.sbc");
			if (!MyFileSystem.FileExists(path))
			{
				return null;
			}
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = Load<MyObjectBuilder_Definitions>(path);
			if (myObjectBuilder_Definitions?.Definitions == null)
			{
				return null;
			}
			HashSet<string> val = new HashSet<string>();
			bool isDedicated = Sandbox.Engine.Platform.Game.IsDedicated;
			MyObjectBuilder_PreloadFileInfo[] definitionFiles = ((MyObjectBuilder_DefinitionsToPreload)myObjectBuilder_Definitions.Definitions[0]).DefinitionFiles;
			foreach (MyObjectBuilder_PreloadFileInfo myObjectBuilder_PreloadFileInfo in definitionFiles)
			{
				if ((myObjectBuilder_PreloadFileInfo.Type & preloadType) != 0 && (!isDedicated || myObjectBuilder_PreloadFileInfo.LoadOnDedicated))
				{
					val.Add(myObjectBuilder_PreloadFileInfo.Name);
				}
			}
			return val;
		}

		private List<Tuple<MyObjectBuilder_Definitions, string>> GetDefinitionBuilders(MyModContext context, HashSet<string> preloadSet = null)
		{
			ConcurrentBag<Tuple<MyObjectBuilder_Definitions, string>> definitionBuilders = new ConcurrentBag<Tuple<MyObjectBuilder_Definitions, string>>();
			IEnumerable<string> enumerable = MyFileSystem.GetFiles(context.ModPathData, "*.sbc", MySearchOption.AllDirectories);
			string path = Path.Combine(context.ModPathData, "../DataPlatform");
			if (MyFileSystem.DirectoryExists(path))
			{
				enumerable = Enumerable.Concat<string>(enumerable, MyFileSystem.GetFiles(path, "*.sbc", MySearchOption.AllDirectories));
			}
			enumerable = Enumerable.Where<string>(enumerable, (Func<string, bool>)((string f) => f.EndsWith(".sbc")));
			ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();
			Parallel.ForEach(enumerable, delegate(string file)
			{
				if ((preloadSet == null || preloadSet.Contains(Path.GetFileName(file))) && !(Path.GetFileName(file) == "DefinitionsToPreload.sbc"))
				{
					MyObjectBuilder_Definitions myObjectBuilder_Definitions = null;
					if (context == MyModContext.BaseGame && preloadSet == null && m_preloadedDefinitionBuilders.TryGetValue(file, ref myObjectBuilder_Definitions))
					{
						definitionBuilders.Add(new Tuple<MyObjectBuilder_Definitions, string>(myObjectBuilder_Definitions, file));
					}
					else
					{
						context.CurrentFile = file;
						try
						{
<<<<<<< HEAD
							value = CheckPrefabs(file);
=======
							myObjectBuilder_Definitions = CheckPrefabs(file);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						catch (Exception ex)
						{
							FailModLoading(context, -1, 0, ex);
							exceptions.Enqueue(ex);
						}
						if (myObjectBuilder_Definitions == null)
						{
							myObjectBuilder_Definitions = Load<MyObjectBuilder_Definitions>(file);
						}
						if (myObjectBuilder_Definitions == null)
						{
							FailModLoading(context);
						}
						else
						{
							Tuple<MyObjectBuilder_Definitions, string> tuple = new Tuple<MyObjectBuilder_Definitions, string>(myObjectBuilder_Definitions, file);
							definitionBuilders.Add(tuple);
							if (context == MyModContext.BaseGame && preloadSet == null && MyFakes.ENABLE_PRELOAD_DEFINITIONS)
							{
								m_preloadedDefinitionBuilders.TryAdd(file, myObjectBuilder_Definitions);
							}
						}
					}
				}
			}, WorkPriority.VeryLow);
			if (exceptions.get_Count() > 0)
			{
				throw new AggregateException((IEnumerable<Exception>)exceptions);
			}
			List<Tuple<MyObjectBuilder_Definitions, string>> list = Enumerable.ToList<Tuple<MyObjectBuilder_Definitions, string>>((IEnumerable<Tuple<MyObjectBuilder_Definitions, string>>)definitionBuilders);
			list.Sort((Tuple<MyObjectBuilder_Definitions, string> x, Tuple<MyObjectBuilder_Definitions, string> y) => x.Item2.CompareTo(y.Item2));
			return list;
		}

		private void LoadDefinitions(MyModContext context, DefinitionSet definitionSet, bool failOnDebug = true, bool isPreload = false)
		{
			HashSet<string> val = null;
			if (isPreload)
			{
				val = GetPreloadSet(MyObjectBuilder_PreloadFileInfo.PreloadType.MainMenu);
				if (val == null)
				{
					return;
				}
			}
			if (!MyFileSystem.DirectoryExists(context.ModPathData))
			{
				return;
			}
			m_currentLoadingSet = definitionSet;
			definitionSet.Context = context;
			m_transparentMaterialsInitialized = false;
<<<<<<< HEAD
			List<Tuple<MyObjectBuilder_Definitions, string>> definitionBuilders = GetDefinitionBuilders(context, hashSet);
=======
			List<Tuple<MyObjectBuilder_Definitions, string>> definitionBuilders = GetDefinitionBuilders(context, val);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (definitionBuilders == null)
			{
				return;
			}
			Action<MyObjectBuilder_Definitions, MyModContext, DefinitionSet, bool>[] array = new Action<MyObjectBuilder_Definitions, MyModContext, DefinitionSet, bool>[6] { CompatPhase, LoadPhase1, LoadPhase2, LoadPhase3, LoadPhase4, LoadPhase5 };
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					foreach (Tuple<MyObjectBuilder_Definitions, string> item in definitionBuilders)
					{
						context.CurrentFile = item.Item2;
						array[i](item.Item1, context, definitionSet, failOnDebug);
					}
				}
				catch (Exception innerException)
				{
					FailModLoading(context, i, array.Length, innerException);
					return;
				}
				MergeDefinitions();
			}
			AfterLoad(context, definitionSet);
		}

		private void LoadDefinitions(List<MyModContext> contexts, List<DefinitionSet> definitionSets, bool failOnDebug = true, bool isPreload = false)
		{
			HashSet<string> val = null;
			if (isPreload)
			{
				val = GetPreloadSet(MyObjectBuilder_PreloadFileInfo.PreloadType.MainMenu);
				if (val == null)
				{
					return;
				}
			}
			List<List<Tuple<MyObjectBuilder_Definitions, string>>> list = new List<List<Tuple<MyObjectBuilder_Definitions, string>>>();
			for (int i = 0; i < contexts.Count; i++)
			{
				if (!MyFileSystem.DirectoryExists(contexts[i].ModPathData))
				{
					list.Add(null);
					continue;
				}
				definitionSets[i].Context = contexts[i];
				m_transparentMaterialsInitialized = false;
				List<Tuple<MyObjectBuilder_Definitions, string>> definitionBuilders = GetDefinitionBuilders(contexts[i], val);
				list.Add(definitionBuilders);
				if (definitionBuilders == null)
				{
					return;
				}
			}
			Action<MyObjectBuilder_Definitions, MyModContext, DefinitionSet, bool>[] array = new Action<MyObjectBuilder_Definitions, MyModContext, DefinitionSet, bool>[6] { CompatPhase, LoadPhase1, LoadPhase2, LoadPhase3, LoadPhase4, LoadPhase5 };
			for (int j = 0; j < array.Length; j++)
			{
				for (int k = 0; k < contexts.Count; k++)
				{
					m_currentLoadingSet = definitionSets[k];
					try
					{
						foreach (Tuple<MyObjectBuilder_Definitions, string> item in list[k])
						{
							contexts[k].CurrentFile = item.Item2;
							array[j](item.Item1, contexts[k], definitionSets[k], failOnDebug);
						}
					}
					catch (Exception innerException)
					{
						FailModLoading(contexts[k], j, array.Length, innerException);
						continue;
					}
					MergeDefinitions();
				}
			}
			for (int l = 0; l < contexts.Count; l++)
			{
				AfterLoad(contexts[l], definitionSets[l]);
			}
			m_directoryExistCache.Clear();
		}

		private void AfterLoad(MyModContext context, DefinitionSet definitionSet)
		{
			MyDefinitionPostprocessor.Bundle bundle = default(MyDefinitionPostprocessor.Bundle);
			bundle.Context = context;
			bundle.Set = m_currentLoadingSet;
			MyDefinitionPostprocessor.Bundle definitions = bundle;
			foreach (MyDefinitionPostprocessor postProcessor in MyDefinitionManagerBase.m_postProcessors)
			{
				if (definitionSet.Definitions.TryGetValue(postProcessor.DefinitionType, out definitions.Definitions))
				{
					postProcessor.AfterLoaded(ref definitions);
				}
			}
		}

		[Conditional("DEBUG")]
		private void CheckEntityComponents()
		{
			if (m_definitions.m_entityComponentDefinitions == null)
<<<<<<< HEAD
			{
				return;
			}
			foreach (KeyValuePair<MyDefinitionId, MyComponentDefinitionBase> entityComponentDefinition in m_definitions.m_entityComponentDefinitions)
			{
=======
			{
				return;
			}
			foreach (KeyValuePair<MyDefinitionId, MyComponentDefinitionBase> entityComponentDefinition in m_definitions.m_entityComponentDefinitions)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				try
				{
					MyComponentFactory.CreateInstanceByTypeId(entityComponentDefinition.Key.TypeId)?.Init(entityComponentDefinition.Value);
				}
				catch (Exception)
				{
				}
			}
		}

		[Conditional("DEBUG")]
		private void CheckComponentContainers()
		{
			if (m_definitions.m_entityContainers == null)
<<<<<<< HEAD
			{
				return;
			}
			foreach (KeyValuePair<MyDefinitionId, MyContainerDefinition> entityContainer in m_definitions.m_entityContainers)
			{
=======
			{
				return;
			}
			foreach (KeyValuePair<MyDefinitionId, MyContainerDefinition> entityContainer in m_definitions.m_entityContainers)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				foreach (MyContainerDefinition.DefaultComponent defaultComponent in entityContainer.Value.DefaultComponents)
				{
					try
					{
						MyComponentFactory.CreateInstanceByTypeId(defaultComponent.BuilderType);
					}
					catch (Exception)
					{
					}
				}
			}
		}

		private static void FailModLoading(MyModContext context, int phase = -1, int phaseNum = 0, Exception innerException = null)
		{
<<<<<<< HEAD
			string text = ((innerException != null) ? (", Following Error occured:" + Environment.NewLine + innerException.Message + Environment.NewLine + innerException.Source + Environment.NewLine + innerException.StackTrace) : "");
=======
			string text = ((innerException != null) ? (", Following Error occured:" + MyEnvironment.NewLine + innerException.Message + MyEnvironment.NewLine + innerException.Source + MyEnvironment.NewLine + innerException.StackTrace) : "");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (phase == -1)
			{
				MyDefinitionErrors.Add(context, "MOD SKIPPED, Cannot load definition file" + text, TErrorSeverity.Critical);
			}
			else
			{
				MyDefinitionErrors.Add(context, string.Format("MOD PARTIALLY SKIPPED, LOADED ONLY {0}/{1} PHASES" + text, phase + 1, phaseNum), TErrorSeverity.Critical);
			}
			if (context.IsBaseGame)
			{
				throw new MyLoadingException(string.Format(MyTexts.GetString(MySpaceTexts.LoadingError_ModifiedOriginalContent), context.CurrentFile, MySession.GameServiceName), innerException);
			}
		}

		private static MyObjectBuilder_Definitions CheckPrefabs(string file)
		{
			List<MyObjectBuilder_PrefabDefinition> prefabs = null;
			using (Stream stream = MyFileSystem.OpenRead(file))
			{
				if (stream != null)
				{
					using Stream stream2 = stream.UnwrapGZip();
					if (stream2 != null)
					{
						CheckXmlForPrefabs(file, ref prefabs, stream2);
					}
				}
			}
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = null;
			if (prefabs != null)
			{
				myObjectBuilder_Definitions = new MyObjectBuilder_Definitions();
				myObjectBuilder_Definitions.Prefabs = prefabs.ToArray();
			}
			return myObjectBuilder_Definitions;
		}

		private static void CheckXmlForPrefabs(string file, ref List<MyObjectBuilder_PrefabDefinition> prefabs, Stream readStream)
		{
			XmlReader val = XmlReader.Create(readStream);
			try
			{
<<<<<<< HEAD
				while (xmlReader.Read())
				{
					if (!xmlReader.IsStartElement())
					{
						continue;
					}
					if (xmlReader.Name == "SpawnGroups")
					{
						break;
					}
					if (xmlReader.Name == "Prefabs")
					{
						prefabs = new List<MyObjectBuilder_PrefabDefinition>();
						while (xmlReader.ReadToFollowing("Prefab"))
						{
							ReadPrefabHeader(file, ref prefabs, xmlReader);
=======
				while (val.Read())
				{
					if (!val.IsStartElement())
					{
						continue;
					}
					if (val.get_Name() == "SpawnGroups")
					{
						break;
					}
					if (val.get_Name() == "Prefabs")
					{
						prefabs = new List<MyObjectBuilder_PrefabDefinition>();
						while (val.ReadToFollowing("Prefab"))
						{
							ReadPrefabHeader(file, ref prefabs, val);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						break;
					}
				}
<<<<<<< HEAD
=======
			}
			finally
			{
				((IDisposable)val)?.Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private static void ReadPrefabHeader(string file, ref List<MyObjectBuilder_PrefabDefinition> prefabs, XmlReader reader)
		{
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Invalid comparison between Unknown and I4
			MyObjectBuilder_PrefabDefinition myObjectBuilder_PrefabDefinition = new MyObjectBuilder_PrefabDefinition();
			myObjectBuilder_PrefabDefinition.PrefabPath = file;
			reader.ReadToFollowing("Id");
			bool flag = false;
			if (reader.get_AttributeCount() >= 2)
			{
				for (int i = 0; i < reader.get_AttributeCount(); i++)
				{
					reader.MoveToAttribute(i);
					string name = reader.get_Name();
					if (!(name == "Type"))
					{
						if (name == "Subtype")
						{
							myObjectBuilder_PrefabDefinition.Id.SubtypeId = reader.get_Value();
						}
					}
					else
					{
						myObjectBuilder_PrefabDefinition.Id.TypeIdString = reader.get_Value();
						flag = true;
					}
				}
			}
			if (!flag)
			{
				while (reader.Read())
				{
					if (reader.IsStartElement())
					{
						string name = reader.get_Name();
						if (!(name == "TypeId"))
						{
							if (name == "SubtypeId")
							{
								reader.Read();
								myObjectBuilder_PrefabDefinition.Id.SubtypeId = reader.get_Value();
							}
						}
						else
						{
							reader.Read();
							myObjectBuilder_PrefabDefinition.Id.TypeIdString = reader.get_Value();
						}
					}
					else if ((int)reader.get_NodeType() == 15 && reader.get_Name() == "Id")
					{
						break;
					}
				}
			}
			prefabs.Add(myObjectBuilder_PrefabDefinition);
		}

		private void CompatPhase(MyObjectBuilder_Definitions objBuilder, MyModContext context, DefinitionSet definitionSet, bool failOnDebug)
		{
			MyObjectBuilder_DefinitionBase[] fonts = objBuilder.Fonts;
			InitDefinitionsCompat(context, fonts);
		}

		private void LoadPhase1(MyObjectBuilder_Definitions objBuilder, MyModContext context, DefinitionSet definitionSet, bool failOnDebug)
		{
			if (objBuilder.Definitions != null)
			{
				MyObjectBuilder_DefinitionBase[] definitions = objBuilder.Definitions;
				foreach (MyObjectBuilder_DefinitionBase builder in definitions)
				{
					MyDefinitionBase def = InitDefinition<MyDefinitionBase>(context, builder);
					m_currentLoadingSet.AddDefinition(def);
				}
			}
			if (objBuilder.GridCreators != null)
			{
				MySandboxGame.Log.WriteLine("Loading grid creators");
				InitGridCreators(context, definitionSet.m_gridCreateDefinitions, definitionSet.m_definitionsById, objBuilder.GridCreators, failOnDebug);
			}
			if (objBuilder.Ammos != null)
			{
				MySandboxGame.Log.WriteLine("Loading ammo definitions");
				InitAmmos(context, definitionSet.m_ammoDefinitionsById, objBuilder.Ammos, failOnDebug);
			}
			if (objBuilder.AmmoMagazines != null)
			{
				MySandboxGame.Log.WriteLine("Loading ammo magazines");
				InitAmmoMagazines(context, definitionSet.m_definitionsById, objBuilder.AmmoMagazines, failOnDebug);
			}
			if (objBuilder.Animations != null)
			{
				MySandboxGame.Log.WriteLine("Loading animations");
				InitAnimations(context, definitionSet.m_definitionsById, objBuilder.Animations, definitionSet.m_animationsBySkeletonType, failOnDebug);
			}
			if (objBuilder.CategoryClasses != null)
			{
				MySandboxGame.Log.WriteLine("Loading category classes");
				InitCategoryClasses(context, definitionSet.m_categoryClasses, objBuilder.CategoryClasses, failOnDebug);
			}
			if (objBuilder.Debris != null)
			{
				MySandboxGame.Log.WriteLine("Loading debris");
				InitDebris(context, definitionSet.m_definitionsById, objBuilder.Debris, failOnDebug);
			}
			if (objBuilder.Edges != null)
			{
				MySandboxGame.Log.WriteLine("Loading edges");
				InitEdges(context, definitionSet.m_definitionsById, objBuilder.Edges, failOnDebug);
			}
			if (objBuilder.Factions != null)
			{
				MySandboxGame.Log.WriteLine("Loading factions");
				InitDefinitionsGeneric<MyObjectBuilder_FactionDefinition, MyFactionDefinition>(context, definitionSet.m_definitionsById, objBuilder.Factions, failOnDebug);
			}
			if (objBuilder.BlockPositions != null)
			{
				MySandboxGame.Log.WriteLine("Loading block positions");
				InitBlockPositions(definitionSet.m_blockPositions, objBuilder.BlockPositions, failOnDebug);
			}
			if (objBuilder.BlueprintClasses != null)
			{
				MySandboxGame.Log.WriteLine("Loading blueprint classes");
				InitBlueprintClasses(context, definitionSet.m_blueprintClasses, objBuilder.BlueprintClasses, failOnDebug);
			}
			if (objBuilder.BlueprintClassEntries != null)
			{
				MySandboxGame.Log.WriteLine("Loading blueprint class entries");
				InitBlueprintClassEntries(context, definitionSet.m_blueprintClassEntries, objBuilder.BlueprintClassEntries, failOnDebug);
			}
			if (objBuilder.Blueprints != null)
			{
				MySandboxGame.Log.WriteLine("Loading blueprints");
				InitBlueprints(context, definitionSet.m_blueprintsById, definitionSet.m_blueprintsByResultId, objBuilder.Blueprints, failOnDebug);
			}
			if (objBuilder.Components != null)
			{
				MySandboxGame.Log.WriteLine("Loading components");
				InitComponents(context, definitionSet.m_definitionsById, objBuilder.Components, failOnDebug);
			}
			if (objBuilder.Configuration != null)
			{
				MySandboxGame.Log.WriteLine("Loading configuration");
				Check(failOnDebug, "Configuration", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitConfiguration(definitionSet, objBuilder.Configuration);
			}
			if (objBuilder.ContainerTypes != null)
			{
				MySandboxGame.Log.WriteLine("Loading container types");
				InitContainerTypes(context, definitionSet.m_containerTypeDefinitions, objBuilder.ContainerTypes, failOnDebug);
			}
			if (objBuilder.Environments != null)
			{
				MySandboxGame.Log.WriteLine("Loading environment definition");
				Check(failOnDebug, "Environment", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitEnvironment(context, definitionSet, objBuilder.Environments, failOnDebug);
			}
			if (objBuilder.DroneBehaviors != null)
			{
				MySandboxGame.Log.WriteLine("Loading drone behaviors");
				Check(failOnDebug, "DroneBehaviors", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				LoadDroneBehaviorPresets(context, definitionSet, objBuilder.DroneBehaviors, failOnDebug);
			}
			if (objBuilder.EnvironmentItemsEntries != null)
			{
				MySandboxGame.Log.WriteLine("Loading environment items entries");
				InitEnvironmentItemsEntries(context, definitionSet.m_environmentItemsEntries, objBuilder.EnvironmentItemsEntries, failOnDebug);
			}
			if (objBuilder.GlobalEvents != null)
			{
				MySandboxGame.Log.WriteLine("Loading event definitions");
				InitGlobalEvents(context, definitionSet.m_definitionsById, objBuilder.GlobalEvents, failOnDebug);
			}
			if (objBuilder.HandItems != null)
			{
				InitHandItems(context, definitionSet.m_handItemsById, objBuilder.HandItems, failOnDebug);
			}
			if (objBuilder.VoxelHands != null)
			{
				InitVoxelHands(context, definitionSet.m_definitionsById, objBuilder.VoxelHands, failOnDebug);
			}
			if (objBuilder.AssetModifiers != null)
			{
				InitAssetModifiers(context, definitionSet.m_assetModifiers, objBuilder.AssetModifiers, failOnDebug);
			}
			if (objBuilder.MainMenuInventoryScenes != null)
			{
				InitMainMenuInventoryScenes(context, definitionSet.m_mainMenuInventoryScenes, objBuilder.MainMenuInventoryScenes, failOnDebug);
			}
			if (objBuilder.PrefabThrowers != null && MyFakes.ENABLE_PREFAB_THROWER)
			{
				DefinitionDictionary<MyDefinitionBase> definitionsById = definitionSet.m_definitionsById;
				MyObjectBuilder_DefinitionBase[] definitions = objBuilder.PrefabThrowers;
				InitPrefabThrowers(context, definitionsById, definitions, failOnDebug);
			}
			if (objBuilder.PhysicalItems != null)
			{
				MySandboxGame.Log.WriteLine("Loading physical items");
				InitPhysicalItems(context, definitionSet.m_definitionsById, definitionSet.m_physicalGunItemDefinitions, definitionSet.m_physicalConsumableItemDefinitions, objBuilder.PhysicalItems, failOnDebug);
			}
			if (objBuilder.TransparentMaterials != null)
			{
				MySandboxGame.Log.WriteLine("Loading transparent material properties");
				InitTransparentMaterials(context, definitionSet.m_definitionsById, objBuilder.TransparentMaterials);
			}
			if (objBuilder.VoxelMaterials != null && MySandboxGame.Static != null)
			{
				MySandboxGame.Log.WriteLine("Loading voxel material definitions");
				InitVoxelMaterials(context, definitionSet.m_voxelMaterialsByName, objBuilder.VoxelMaterials, failOnDebug);
			}
			if (objBuilder.Characters != null)
			{
				MySandboxGame.Log.WriteLine("Loading character definitions");
				InitCharacters(context, definitionSet.m_characters, definitionSet.m_definitionsById, objBuilder.Characters, failOnDebug);
			}
			if (objBuilder.CompoundBlockTemplates != null)
			{
				MySandboxGame.Log.WriteLine("Loading compound block template definitions");
				InitDefinitionsGeneric<MyObjectBuilder_CompoundBlockTemplateDefinition, MyCompoundBlockTemplateDefinition>(context, definitionSet.m_definitionsById, objBuilder.CompoundBlockTemplates, failOnDebug);
			}
			if (objBuilder.Sounds != null)
			{
				MySandboxGame.Log.WriteLine("Loading sound definitions");
				InitSounds(context, definitionSet.m_sounds, objBuilder.Sounds, failOnDebug);
			}
			if (objBuilder.MultiBlocks != null)
			{
				MySandboxGame.Log.WriteLine("Loading multi cube block definitions");
				InitDefinitionsGeneric<MyObjectBuilder_MultiBlockDefinition, MyMultiBlockDefinition>(context, definitionSet.m_definitionsById, objBuilder.MultiBlocks, failOnDebug);
			}
			if (objBuilder.SoundCategories != null)
			{
				MySandboxGame.Log.WriteLine("Loading sound categories");
				InitSoundCategories(context, definitionSet.m_definitionsById, objBuilder.SoundCategories, failOnDebug);
			}
			if (objBuilder.ShipSoundGroups != null)
			{
				MySandboxGame.Log.WriteLine("Loading ship sound groups");
				InitShipSounds(context, definitionSet.m_shipSounds, objBuilder.ShipSoundGroups, failOnDebug);
			}
			if (objBuilder.ShipSoundSystem != null)
			{
				MySandboxGame.Log.WriteLine("Loading ship sound groups");
				InitShipSoundSystem(context, ref definitionSet.m_shipSoundSystem, objBuilder.ShipSoundSystem, failOnDebug);
			}
			if (objBuilder.LCDTextures != null)
			{
				MySandboxGame.Log.WriteLine("Loading LCD texture categories");
				InitLCDTextureCategories(context, definitionSet, definitionSet.m_definitionsById, objBuilder.LCDTextures, failOnDebug);
			}
			if (objBuilder.AIBehaviors != null)
			{
				MySandboxGame.Log.WriteLine("Loading behaviors");
				DefinitionDictionary<MyBehaviorDefinition> behaviorDefinitions = definitionSet.m_behaviorDefinitions;
				MyObjectBuilder_DefinitionBase[] definitions = objBuilder.AIBehaviors;
				InitAIBehaviors(context, behaviorDefinitions, definitions, failOnDebug);
			}
			if (objBuilder.VoxelMapStorages != null)
			{
				MySandboxGame.Log.WriteLine("Loading voxel map storage definitions");
				InitVoxelMapStorages(context, definitionSet.m_voxelMapStorages, objBuilder.VoxelMapStorages, failOnDebug);
			}
			if (objBuilder.Bots != null)
			{
				MySandboxGame.Log.WriteLine("Loading agent definitions");
				InitBots(context, definitionSet.m_definitionsById, objBuilder.Bots, failOnDebug);
			}
			if (objBuilder.PhysicalMaterials != null)
			{
				MySandboxGame.Log.WriteLine("Loading physical material properties");
				InitPhysicalMaterials(context, definitionSet.m_definitionsById, objBuilder.PhysicalMaterials);
			}
			if (objBuilder.AiCommands != null)
			{
				MySandboxGame.Log.WriteLine("Loading bot commands");
				InitBotCommands(context, definitionSet.m_definitionsById, objBuilder.AiCommands, failOnDebug);
			}
			if (objBuilder.BlockNavigationDefinitions != null)
			{
				MySandboxGame.Log.WriteLine("Loading navigation definitions");
				InitNavigationDefinitions(context, definitionSet.m_definitionsById, objBuilder.BlockNavigationDefinitions, failOnDebug);
			}
			if (objBuilder.Cuttings != null)
			{
				MySandboxGame.Log.WriteLine("Loading cutting definitions");
				DefinitionDictionary<MyDefinitionBase> definitionsById2 = definitionSet.m_definitionsById;
				MyObjectBuilder_DefinitionBase[] definitions = objBuilder.Cuttings;
				InitGenericObjects(context, definitionsById2, definitions, failOnDebug);
			}
			if (objBuilder.ControllerSchemas != null)
			{
				MySandboxGame.Log.WriteLine("Loading controller schemas definitions");
				InitControllerSchemas(context, definitionSet.m_definitionsById, objBuilder.ControllerSchemas, failOnDebug);
			}
			if (objBuilder.CurveDefinitions != null)
			{
				MySandboxGame.Log.WriteLine("Loading curve definitions");
				InitCurves(context, definitionSet.m_definitionsById, objBuilder.CurveDefinitions, failOnDebug);
			}
			if (objBuilder.CharacterNames != null)
			{
				MySandboxGame.Log.WriteLine("Loading character names");
				InitCharacterNames(context, definitionSet.m_characterNames, objBuilder.CharacterNames, failOnDebug);
			}
			if (objBuilder.DecalGlobals != null)
			{
				MySandboxGame.Log.WriteLine("Loading decal global definitions");
				Check(failOnDebug, "DecalGlobals", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitDecalGlobals(context, objBuilder.DecalGlobals, failOnDebug);
			}
			if (objBuilder.EmissiveColors != null)
			{
				MySandboxGame.Log.WriteLine("Loading emissive color definitions");
				Check(failOnDebug, "EmissiveColors", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitEmissiveColors(context, objBuilder.EmissiveColors, failOnDebug);
			}
			if (objBuilder.EmissiveColorStatePresets != null)
			{
				MySandboxGame.Log.WriteLine("Loading emissive color default states");
				Check(failOnDebug, "EmissiveColorPresets", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitEmissiveColorPresets(context, objBuilder.EmissiveColorStatePresets, failOnDebug);
			}
			if (objBuilder.Decals != null)
			{
				MySandboxGame.Log.WriteLine("Loading decal definitions");
				Check(failOnDebug, "Decals", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitDecals(context, objBuilder.Decals, failOnDebug);
			}
			if (objBuilder.PlanetGeneratorDefinitions != null)
			{
				MySandboxGame.Log.WriteLine("Loading planet definition " + context.ModName);
				Check(failOnDebug, "Planet", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitPlanetGeneratorDefinitions(context, definitionSet, objBuilder.PlanetGeneratorDefinitions, failOnDebug);
			}
			if (objBuilder.StatDefinitions != null)
			{
				MySandboxGame.Log.WriteLine("Loading stat definitions");
				Check(failOnDebug, "Stat", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				DefinitionDictionary<MyDefinitionBase> definitionsById3 = definitionSet.m_definitionsById;
				MyObjectBuilder_DefinitionBase[] definitions = objBuilder.StatDefinitions;
				InitGenericObjects(context, definitionsById3, definitions, failOnDebug);
			}
			if (objBuilder.GasProperties != null)
			{
				MySandboxGame.Log.WriteLine("Loading gas property definitions");
				Check(failOnDebug, "Gas", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				DefinitionDictionary<MyDefinitionBase> definitionsById4 = definitionSet.m_definitionsById;
				MyObjectBuilder_DefinitionBase[] definitions = objBuilder.GasProperties;
				InitGenericObjects(context, definitionsById4, definitions, failOnDebug);
			}
			if (objBuilder.ResourceDistributionGroups != null)
			{
				MySandboxGame.Log.WriteLine("Loading resource distribution groups");
				Check(failOnDebug, "DistributionGroup", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				DefinitionDictionary<MyDefinitionBase> definitionsById5 = definitionSet.m_definitionsById;
				MyObjectBuilder_DefinitionBase[] definitions = objBuilder.ResourceDistributionGroups;
				InitGenericObjects(context, definitionsById5, definitions, failOnDebug);
			}
			if (objBuilder.ComponentGroups != null)
			{
				MySandboxGame.Log.WriteLine("Loading component group definitions");
				Check(failOnDebug, "Component groups", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitComponentGroups(context, definitionSet.m_componentGroups, objBuilder.ComponentGroups, failOnDebug);
			}
			if (objBuilder.ComponentBlocks != null)
			{
				MySandboxGame.Log.WriteLine("Loading component block definitions");
				InitComponentBlocks(context, definitionSet.m_componentBlockEntries, objBuilder.ComponentBlocks, failOnDebug);
			}
			if (objBuilder.PlanetPrefabs != null)
			{
				MySandboxGame.Log.WriteLine("Loading planet prefabs");
				Check(failOnDebug, "Planet prefabs", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitPlanetPrefabDefinitions(context, ref definitionSet.m_planetPrefabDefinitions, objBuilder.PlanetPrefabs, failOnDebug);
			}
			if (objBuilder.EnvironmentGroups != null)
			{
				MySandboxGame.Log.WriteLine("Loading environment groups");
				Check(failOnDebug, "Environment groups", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitGroupedIds(context, "EnvGroups", definitionSet.m_groupedIds, objBuilder.EnvironmentGroups, failOnDebug);
			}
			if (objBuilder.PirateAntennas != null)
			{
				MySandboxGame.Log.WriteLine("Loading pirate antennas");
				Check(failOnDebug, "Pirate antennas", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitDefinitionsGeneric(context, definitionSet.m_pirateAntennaDefinitions, objBuilder.PirateAntennas, failOnDebug);
			}
			if (objBuilder.Destruction != null)
			{
				MySandboxGame.Log.WriteLine("Loading destruction definition");
				Check(failOnDebug, "Destruction", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitDestruction(context, ref definitionSet.m_destructionDefinition, objBuilder.Destruction, failOnDebug);
			}
			if (objBuilder.EntityComponents != null)
			{
				MySandboxGame.Log.WriteLine("Loading entity components");
				Check(failOnDebug, "Entity components", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitDefinitionsGeneric(context, definitionSet.m_entityComponentDefinitions, objBuilder.EntityComponents, failOnDebug);
			}
			if (objBuilder.EntityContainers != null)
			{
				MySandboxGame.Log.WriteLine("Loading component containers");
				Check(failOnDebug, "Entity containers", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitDefinitionsGeneric(context, definitionSet.m_entityContainers, objBuilder.EntityContainers, failOnDebug);
			}
			if (objBuilder.ShadowTextureSets != null)
			{
				MySandboxGame.Log.WriteLine("Loading shadow textures definitions");
				Check(failOnDebug, "Text shadow sets", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitShadowTextureSets(context, objBuilder.ShadowTextureSets, failOnDebug);
			}
			if (objBuilder.Flares != null)
			{
				MySandboxGame.Log.WriteLine("Loading flare definitions");
				Check(failOnDebug, "Flares", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitFlares(context, definitionSet.m_definitionsById, objBuilder.Flares, failOnDebug);
			}
			if (objBuilder.ResearchGroups != null)
			{
				MySandboxGame.Log.WriteLine("Loading research groups definitions");
				Check(failOnDebug, "Research Groups", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitResearchGroups(context, ref definitionSet.m_researchGroupsDefinitions, objBuilder.ResearchGroups, failOnDebug);
			}
			if (objBuilder.ResearchBlocks != null)
			{
				MySandboxGame.Log.WriteLine("Loading research blocks definitions");
				Check(failOnDebug, "Research Blocks", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitResearchBlocks(context, ref definitionSet.m_researchBlocksDefinitions, objBuilder.ResearchBlocks, failOnDebug);
			}
			if (objBuilder.ContractTypes != null)
			{
				MySandboxGame.Log.WriteLine("Loading Contract Types");
				InitContractTypes(context, ref definitionSet.m_contractTypesDefinitions, objBuilder.ContractTypes, failOnDebug);
			}
			if (objBuilder.FactionNames != null)
			{
				MySandboxGame.Log.WriteLine("Loading Faction Names");
				InitFactionNames(context, ref definitionSet.m_factionNameDefinitions, objBuilder.FactionNames, failOnDebug);
			}
			if (objBuilder.WeatherEffects != null)
			{
				MySandboxGame.Log.WriteLine("Loading Weather Effects");
				InitWeatherEffects(context, ref definitionSet.m_weatherEffectsDefinitions, objBuilder.WeatherEffects, failOnDebug);
			}
			if (objBuilder.OffensiveWords != null)
			{
				MySandboxGame.Log.WriteLine("Loading OffensiveWords definitions");
				Check(failOnDebug, "Offensive Words", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitOffensiveWords(context, ref definitionSet.m_offensiveWordsDefinition, objBuilder.OffensiveWords, failOnDebug);
			}
<<<<<<< HEAD
			if (objBuilder.ControllerSchemes != null)
			{
				MySandboxGame.Log.WriteLine("Loading gamepad controls");
				InitControllerSchema(context, ref definitionSet.m_controllerSchemeDefinitions, objBuilder.ControllerSchemes, failOnDebug);
			}
			if (objBuilder.TargetingGroups != null)
			{
				MySandboxGame.Log.WriteLine("Loading gamepad controls");
				InitDefinitionsGeneric(context, definitionSet.m_targetingGroupDefinitions, objBuilder.TargetingGroups, failOnDebug);
=======
			if (objBuilder.ChatBot != null)
			{
				MySandboxGame.Log.WriteLine("Loading chat bot responses");
				InitChatBot(context, ref definitionSet.m_chatBotResponseDefinitions, objBuilder.ChatBot, failOnDebug);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void LoadPhase2(MyObjectBuilder_Definitions objBuilder, MyModContext context, DefinitionSet definitionSet, bool failOnDebug)
		{
			if (objBuilder.ParticleEffects != null)
			{
				MySandboxGame.Log.WriteLine("Loading particle effect definitions");
				InitParticleEffects(context, definitionSet.m_definitionsById, objBuilder.ParticleEffects, failOnDebug);
			}
			if (objBuilder.EnvironmentItems != null)
			{
				MySandboxGame.Log.WriteLine("Loading environment item definitions");
				InitDefinitionsEnvItems(context, definitionSet.m_definitionsById, objBuilder.EnvironmentItems, failOnDebug);
			}
			if (objBuilder.EnvironmentItemsDefinitions != null)
			{
				MySandboxGame.Log.WriteLine("Loading environment items definitions");
				InitDefinitionsGeneric<MyObjectBuilder_EnvironmentItemsDefinition, MyEnvironmentItemsDefinition>(context, definitionSet.m_definitionsById, objBuilder.EnvironmentItemsDefinitions, failOnDebug);
			}
			if (objBuilder.MaterialProperties != null)
			{
				MySandboxGame.Log.WriteLine("Loading physical material properties");
				InitMaterialProperties(context, definitionSet.m_definitionsById, objBuilder.MaterialProperties);
			}
			if (objBuilder.Weapons != null)
			{
				MySandboxGame.Log.WriteLine("Loading weapon definitions");
				InitWeapons(context, definitionSet.m_weaponDefinitionsById, objBuilder.Weapons, failOnDebug);
			}
			if (objBuilder.AudioEffects != null)
			{
				MySandboxGame.Log.WriteLine("Audio effects definitions");
				InitAudioEffects(context, definitionSet.m_definitionsById, objBuilder.AudioEffects, failOnDebug);
			}
		}

		private void LoadPhase3(MyObjectBuilder_Definitions objBuilder, MyModContext context, DefinitionSet definitionSet, bool failOnDebug)
		{
			if (objBuilder.CubeBlocks != null)
			{
				MySandboxGame.Log.WriteLine("Loading cube blocks");
				InitCubeBlocks(context, objBuilder.CubeBlocks);
				ToDefinitions(context, definitionSet.m_definitionsById, definitionSet.m_uniqueCubeBlocksBySize, objBuilder.CubeBlocks, failOnDebug);
				MySandboxGame.Log.WriteLine("Created block definitions");
				DefinitionDictionary<MyCubeBlockDefinition>[] uniqueCubeBlocksBySize = definitionSet.m_uniqueCubeBlocksBySize;
				foreach (DefinitionDictionary<MyCubeBlockDefinition> cubeBlocks in uniqueCubeBlocksBySize)
				{
					PrepareBlockBlueprints(context, definitionSet.m_blueprintsById, cubeBlocks);
				}
			}
		}

		private void LoadPhase4(MyObjectBuilder_Definitions objBuilder, MyModContext context, DefinitionSet definitionSet, bool failOnDebug)
		{
			if (objBuilder.Prefabs != null && MySandboxGame.Static != null)
			{
				MySandboxGame.Log.WriteLine("Loading prefab: " + context.CurrentFile);
				InitPrefabs(context, definitionSet.m_prefabs, objBuilder.Prefabs, failOnDebug);
			}
			if (MyFakes.ENABLE_GENERATED_INTEGRITY_FIX)
			{
				DefinitionDictionary<MyCubeBlockDefinition>[] uniqueCubeBlocksBySize = definitionSet.m_uniqueCubeBlocksBySize;
				foreach (DefinitionDictionary<MyCubeBlockDefinition> cubeBlocks in uniqueCubeBlocksBySize)
				{
					FixGeneratedBlocksIntegrity(cubeBlocks);
				}
			}
			if (objBuilder.BlockVariantGroups != null && MySandboxGame.Static != null)
			{
				MySandboxGame.Log.WriteLine("Loading block variant groups");
				InitBlockVariantGroups(context, definitionSet.m_blockVariantGroups, objBuilder.BlockVariantGroups, failOnDebug);
			}
		}

		private void LoadPhase5(MyObjectBuilder_Definitions objBuilder, MyModContext context, DefinitionSet definitionSet, bool failOnDebug)
		{
			if (objBuilder.SpawnGroups != null && MySandboxGame.Static != null)
			{
				MySandboxGame.Log.WriteLine("Loading spawn groups");
				InitSpawnGroups(context, definitionSet.m_spawnGroupDefinitions, definitionSet.m_definitionsById, objBuilder.SpawnGroups);
			}
			if (objBuilder.RespawnShips != null && MySandboxGame.Static != null)
			{
				MySandboxGame.Log.WriteLine("Loading respawn ships");
				InitRespawnShips(context, definitionSet.m_respawnShips, objBuilder.RespawnShips, failOnDebug);
			}
			if (objBuilder.DropContainers != null && MySandboxGame.Static != null)
			{
				MySandboxGame.Log.WriteLine("Loading drop containers");
				InitDropContainers(context, definitionSet.m_dropContainers, objBuilder.DropContainers, failOnDebug);
			}
			if (objBuilder.RadialMenus != null)
			{
				MySandboxGame.Log.WriteLine("Loading radial menu definitions");
				Check(failOnDebug, "Radial menu", failOnDebug, "WARNING: Unexpected behaviour may occur due to redefinition of '{0}'");
				InitRadialMenus(context, definitionSet.m_radialMenuDefinitions, objBuilder.RadialMenus, failOnDebug);
			}
			if (objBuilder.WheelModels != null && MySandboxGame.Static != null)
			{
				MySandboxGame.Log.WriteLine("Loading wheel speeds");
				InitWheelModels(context, definitionSet.m_wheelModels, objBuilder.WheelModels, failOnDebug);
			}
			if (objBuilder.AsteroidGenerators != null && MySandboxGame.Static != null)
			{
				MySandboxGame.Log.WriteLine("Loading asteroid generators");
				InitAsteroidGenerators(context, definitionSet.m_asteroidGenerators, objBuilder.AsteroidGenerators, failOnDebug);
			}
		}

		private void LoadScenarios(MyModContext context, DefinitionSet definitionSet, bool failOnDebug = true)
		{
			string text = Path.Combine(context.ModPathData, "Scenarios.sbx");
			if (!MyFileSystem.FileExists(text))
			{
				return;
			}
			MyDataIntegrityChecker.HashInFile(text);
			MyObjectBuilder_ScenarioDefinitions myObjectBuilder_ScenarioDefinitions = Load<MyObjectBuilder_ScenarioDefinitions>(text);
			if (myObjectBuilder_ScenarioDefinitions == null)
			{
				MyDefinitionErrors.Add(context, "Scenarios: Cannot load definition file, see log for details", TErrorSeverity.Error);
				return;
			}
			if (myObjectBuilder_ScenarioDefinitions.Scenarios != null)
			{
				MySandboxGame.Log.WriteLine("Loading scenarios");
				InitScenarioDefinitions(context, definitionSet.m_definitionsById, definitionSet.m_scenarioDefinitions, myObjectBuilder_ScenarioDefinitions.Scenarios, failOnDebug);
			}
			MergeDefinitions();
		}

		private void LoadPostProcess()
		{
			InitVoxelMaterials();
			if (!m_transparentMaterialsInitialized)
			{
				CreateTransparentMaterials();
				m_transparentMaterialsInitialized = true;
			}
			InitBlockGroups();
			PostprocessComponentGroups();
			PostprocessComponentBlocks();
			PostprocessBlueprints();
			PostprocessBlockVariantGroups();
			PostprocessRadialMenus();
			AddEntriesToBlueprintClasses();
			AddEntriesToEnvironmentItemClasses();
			PairPhysicalAndHandItems();
			CheckWeaponRelatedDefinitions();
			SetShipSoundSystem();
			MoveNonPublicBlocksToSpecialCategory();
			if (MyAudio.Static != null)
			{
				ListReader<MySoundData> soundDataFromDefinitions = MyAudioExtensions.GetSoundDataFromDefinitions();
				ListReader<MyAudioEffect> effectData = MyAudioExtensions.GetEffectData();
				if (MyFakes.ENABLE_SOUNDS_ASYNC_PRELOAD)
				{
					SoundsData workData = new SoundsData
					{
						SoundData = soundDataFromDefinitions,
						EffectData = effectData,
						Priority = WorkPriority.VeryLow
					};
					Parallel.Start(LoadSoundAsync, OnLoadSoundsComplete, workData);
				}
				else
				{
					MyAudio.Static.CacheLoaded = false;
					MyAudio.Static.ReloadData(soundDataFromDefinitions, effectData);
				}
			}
			PostprocessPirateAntennas();
			InitMultiBlockDefinitions();
			CreateMapMultiBlockDefinitionToBlockDefinition();
			CreateLCDTexturesDefinitions();
			PostprocessDecals();
			PostprocessAllDefinitions();
			InitAssetModifiersForRender();
			AfterPostprocess();
		}

		private void CreateLCDTexturesDefinitions()
		{
			List<MyLCDTextureDefinition> list = new List<MyLCDTextureDefinition>();
			foreach (MyDefinitionBase value in m_definitions.m_definitionsById.Values)
			{
				MyPhysicalItemDefinition physDef;
				if ((physDef = value as MyPhysicalItemDefinition) != null && value.Icons != null && value.Icons.Length != 0)
				{
					MyLCDTextureDefinition item = CreateLCDTextureDefinition(physDef);
					list.Add(item);
				}
			}
			foreach (MyFactionIconsDefinition allDefinition in GetAllDefinitions<MyFactionIconsDefinition>())
			{
				if (allDefinition.Icons != null && allDefinition.Icons.Length != 0)
				{
					string[] icons = allDefinition.Icons;
					foreach (string text in icons)
					{
						MyLCDTextureDefinition myLCDTextureDefinition = new MyLCDTextureDefinition();
						myLCDTextureDefinition.Id = new MyDefinitionId(typeof(MyObjectBuilder_LCDTextureDefinition), text);
						myLCDTextureDefinition.Public = false;
						myLCDTextureDefinition.LocalizationId = allDefinition.DisplayNameString;
						myLCDTextureDefinition.SpritePath = text;
						myLCDTextureDefinition.Selectable = false;
						list.Add(myLCDTextureDefinition);
					}
				}
			}
			foreach (MyLCDTextureDefinition item2 in list)
			{
				if (!m_definitions.m_definitionsById.ContainsKey(item2.Id))
				{
					m_definitions.m_definitionsById.Add(item2.Id, item2);
					m_definitions.AddOrRelaceDefinition(item2);
				}
			}
		}

		private static MyLCDTextureDefinition CreateLCDTextureDefinition(MyPhysicalItemDefinition physDef)
		{
			return new MyLCDTextureDefinition
			{
				Id = new MyDefinitionId(typeof(MyObjectBuilder_LCDTextureDefinition), physDef.Id.ToString()),
				Public = false,
				LocalizationId = physDef.DisplayNameString,
				SpritePath = ((physDef.Icons.Length != 0) ? physDef.Icons[0] : string.Empty),
				Selectable = false
			};
		}

		private void InitAssetModifiersForRender()
		{
			m_definitions.m_assetModifiersForRender = new Dictionary<MyStringHash, MyAssetModifiers>();
			foreach (KeyValuePair<MyDefinitionId, MyAssetModifierDefinition> assetModifier in m_definitions.m_assetModifiers)
			{
				Dictionary<string, MyTextureChange> dictionary = new Dictionary<string, MyTextureChange>();
				foreach (MyObjectBuilder_AssetModifierDefinition.MyAssetTexture texture in assetModifier.Value.Textures)
				{
					if (!string.IsNullOrEmpty(texture.Location))
					{
						dictionary.TryGetValue(texture.Location, out var value);
						switch (texture.Type)
						{
						case MyTextureType.ColorMetal:
							value.ColorMetalFileName = texture.Filepath;
							break;
						case MyTextureType.NormalGloss:
							value.NormalGlossFileName = texture.Filepath;
							break;
						case MyTextureType.Extensions:
							value.ExtensionsFileName = texture.Filepath;
							break;
						case MyTextureType.Alphamask:
							value.AlphamaskFileName = texture.Filepath;
							break;
						}
						dictionary[texture.Location] = value;
					}
				}
				MyAssetModifiers myAssetModifiers = default(MyAssetModifiers);
				myAssetModifiers.MetalnessColorable = assetModifier.Value.MetalnessColorable;
				myAssetModifiers.SkinTextureChanges = dictionary;
				MyAssetModifiers value2 = myAssetModifiers;
				m_definitions.m_assetModifiersForRender.Add(assetModifier.Key.SubtypeId, value2);
			}
		}

		private void OnLoadSoundsComplete(WorkData workData)
		{
		}

		private void LoadSoundAsync(WorkData workData)
		{
			SoundsData soundsData = workData as SoundsData;
			if (soundsData != null)
			{
				MyAudio.Static.ReloadData(soundsData.SoundData, soundsData.EffectData);
			}
		}

		private void PostprocessAllDefinitions()
		{
			foreach (MyDefinitionBase value in m_definitions.m_definitionsById.Values)
			{
				value.Postprocess();
			}
		}

		private void AfterPostprocess()
		{
			foreach (MyDefinitionPostprocessor postProcessor in MyDefinitionManagerBase.m_postProcessors)
			{
				if (m_definitions.Definitions.TryGetValue(postProcessor.DefinitionType, out var value))
				{
					postProcessor.AfterPostprocess(m_definitions, value);
				}
			}
		}

		private void InitMultiBlockDefinitions()
		{
			if (!MyFakes.ENABLE_MULTIBLOCKS)
			{
				return;
			}
			foreach (MyMultiBlockDefinition multiBlockDefinition in GetMultiBlockDefinitions())
			{
				multiBlockDefinition.Min = Vector3I.MaxValue;
				multiBlockDefinition.Max = Vector3I.MinValue;
				MyMultiBlockDefinition.MyMultiBlockPartDefinition[] blockDefinitions = multiBlockDefinition.BlockDefinitions;
				foreach (MyMultiBlockDefinition.MyMultiBlockPartDefinition myMultiBlockPartDefinition in blockDefinitions)
				{
					if (Static.TryGetCubeBlockDefinition(myMultiBlockPartDefinition.Id, out var blockDefinition) && blockDefinition != null)
					{
						MatrixI transformation = new MatrixI(myMultiBlockPartDefinition.Forward, myMultiBlockPartDefinition.Up);
						Vector3I vector3I = Vector3I.Abs(Vector3I.TransformNormal(blockDefinition.Size - Vector3I.One, ref transformation));
						myMultiBlockPartDefinition.Max = myMultiBlockPartDefinition.Min + vector3I;
						multiBlockDefinition.Min = Vector3I.Min(multiBlockDefinition.Min, myMultiBlockPartDefinition.Min);
						multiBlockDefinition.Max = Vector3I.Max(multiBlockDefinition.Max, myMultiBlockPartDefinition.Max);
					}
				}
			}
		}

		private void CreateMapMultiBlockDefinitionToBlockDefinition()
		{
			if (!MyFakes.ENABLE_MULTIBLOCKS)
			{
				return;
			}
			ListReader<MyMultiBlockDefinition> multiBlockDefinitions = GetMultiBlockDefinitions();
<<<<<<< HEAD
			List<MyCubeBlockDefinition> list = m_definitions.m_definitionsById.Values.OfType<MyCubeBlockDefinition>().ToList();
=======
			List<MyCubeBlockDefinition> list = Enumerable.ToList<MyCubeBlockDefinition>(Enumerable.OfType<MyCubeBlockDefinition>((IEnumerable)m_definitions.m_definitionsById.Values));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MyMultiBlockDefinition item in multiBlockDefinitions)
			{
				foreach (MyCubeBlockDefinition item2 in list)
				{
					if (item2.MultiBlock == item.Id.SubtypeName)
					{
						if (!m_definitions.m_mapMultiBlockDefToCubeBlockDef.ContainsKey(item.Id.SubtypeName))
						{
							m_definitions.m_mapMultiBlockDefToCubeBlockDef.Add(item.Id.SubtypeName, item2);
						}
						break;
					}
				}
			}
		}

		public MyCubeBlockDefinition GetCubeBlockDefinitionForMultiBlock(string multiBlock)
		{
			if (m_definitions.m_mapMultiBlockDefToCubeBlockDef.TryGetValue(multiBlock, out var value))
			{
				return value;
			}
			return null;
		}

		private void PostprocessPirateAntennas()
		{
			foreach (MyPirateAntennaDefinition value in m_definitions.m_pirateAntennaDefinitions.Values)
			{
				value.Postprocess();
			}
		}

		private void PostprocessDecals()
		{
			new List<string>();
			Dictionary<string, List<MyDecalMaterialDesc>> dictionary = new Dictionary<string, List<MyDecalMaterialDesc>>();
			MyDecalMaterials.ClearMaterials();
			foreach (MyObjectBuilder_DecalDefinition decalObjectBuilder in m_decalObjectBuilders)
			{
				if (decalObjectBuilder.MaxSize < decalObjectBuilder.MinSize)
				{
					decalObjectBuilder.MaxSize = decalObjectBuilder.MinSize;
				}
				MyDecalMaterial myDecalMaterial = new MyDecalMaterial(decalObjectBuilder.Material, decalObjectBuilder.Transparent, MyStringHash.GetOrCompute(decalObjectBuilder.Target), MyStringHash.GetOrCompute(decalObjectBuilder.Source), decalObjectBuilder.MinSize, decalObjectBuilder.MaxSize, decalObjectBuilder.Depth, decalObjectBuilder.Rotation, null, decalObjectBuilder.XOffset, decalObjectBuilder.YOffset, decalObjectBuilder.Alpha, decalObjectBuilder.Spacing, decalObjectBuilder.RenderDistance);
				if (!dictionary.TryGetValue(myDecalMaterial.StringId, out var value))
				{
					value = new List<MyDecalMaterialDesc>();
					dictionary[myDecalMaterial.StringId] = value;
				}
				if (decalObjectBuilder.Blacklist != null && decalObjectBuilder.Blacklist.Length != 0)
				{
					List<MyStringHash> list = new List<MyStringHash>();
					string[] blacklist = decalObjectBuilder.Blacklist;
					foreach (string str in blacklist)
					{
						list.Add(MyStringHash.GetOrCompute(str));
					}
					myDecalMaterial.SetBlacklist(list);
				}
				value.Add(decalObjectBuilder.Material);
				MyDecalMaterials.AddDecalMaterial(myDecalMaterial);
<<<<<<< HEAD
			}
			MyRenderProxy.RegisterDecals(dictionary);
		}

		private void MoveNonPublicBlocksToSpecialCategory()
		{
			if (!MyFakes.ENABLE_NON_PUBLIC_BLOCKS)
			{
				return;
			}
			MyGuiBlockCategoryDefinition myGuiBlockCategoryDefinition = new MyGuiBlockCategoryDefinition
			{
				DescriptionString = "Non public blocks",
				DisplayNameString = "Non public",
				Enabled = true,
				Id = new MyDefinitionId(typeof(MyObjectBuilder_GuiBlockCategoryDefinition)),
				IsBlockCategory = true,
				IsShipCategory = false,
				Name = "Non public",
				Public = true,
				SearchBlocks = true,
				ShowAnimations = false,
				ItemIds = new HashSet<string>()
			};
			foreach (string definitionPairName in GetDefinitionPairNames())
			{
				MyCubeBlockDefinitionGroup definitionGroup = Static.GetDefinitionGroup(definitionPairName);
				myGuiBlockCategoryDefinition.ItemIds.Add(definitionGroup.Any.Id.ToString());
			}
=======
			}
			MyRenderProxy.RegisterDecals(dictionary);
		}

		private void MoveNonPublicBlocksToSpecialCategory()
		{
			if (!MyFakes.ENABLE_NON_PUBLIC_BLOCKS)
			{
				return;
			}
			MyGuiBlockCategoryDefinition myGuiBlockCategoryDefinition = new MyGuiBlockCategoryDefinition
			{
				DescriptionString = "Non public blocks",
				DisplayNameString = "Non public",
				Enabled = true,
				Id = new MyDefinitionId(typeof(MyObjectBuilder_GuiBlockCategoryDefinition)),
				IsBlockCategory = true,
				IsShipCategory = false,
				Name = "Non public",
				Public = true,
				SearchBlocks = true,
				ShowAnimations = false,
				ItemIds = new HashSet<string>()
			};
			foreach (string definitionPairName in GetDefinitionPairNames())
			{
				MyCubeBlockDefinitionGroup definitionGroup = Static.GetDefinitionGroup(definitionPairName);
				myGuiBlockCategoryDefinition.ItemIds.Add(definitionGroup.Any.Id.ToString());
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_definitions.m_categories.Add("NonPublic", myGuiBlockCategoryDefinition);
		}

		private void PairPhysicalAndHandItems()
		{
			foreach (KeyValuePair<MyDefinitionId, MyHandItemDefinition> item in m_definitions.m_handItemsById)
			{
				MyHandItemDefinition value = item.Value;
				MyPhysicalItemDefinition physicalItemDefinition = GetPhysicalItemDefinition(value.PhysicalItemId);
				Check(!m_definitions.m_physicalItemsByHandItemId.ContainsKey(value.Id), value.Id);
				Check(!m_definitions.m_handItemsByPhysicalItemId.ContainsKey(physicalItemDefinition.Id), physicalItemDefinition.Id);
				m_definitions.m_physicalItemsByHandItemId[value.Id] = physicalItemDefinition;
				m_definitions.m_handItemsByPhysicalItemId[physicalItemDefinition.Id] = value;
			}
		}

		private void CheckWeaponRelatedDefinitions()
		{
			foreach (MyWeaponDefinition value in m_definitions.m_weaponDefinitionsById.Values)
			{
				MyDefinitionId[] ammoMagazinesId = value.AmmoMagazinesId;
				foreach (MyDefinitionId myDefinitionId in ammoMagazinesId)
				{
					Check(m_definitions.m_definitionsById.ContainsKey(myDefinitionId), myDefinitionId, failOnDebug: true, "Unknown type '{0}'");
					MyAmmoMagazineDefinition ammoMagazineDefinition = GetAmmoMagazineDefinition(myDefinitionId);
					Check(m_definitions.m_ammoDefinitionsById.ContainsKey(ammoMagazineDefinition.AmmoDefinitionId), ammoMagazineDefinition.AmmoDefinitionId, failOnDebug: true, "Unknown type '{0}'");
					MyAmmoDefinition ammoDefinition = GetAmmoDefinition(ammoMagazineDefinition.AmmoDefinitionId);
					if (!value.HasSpecificAmmoData(ammoDefinition))
					{
						StringBuilder stringBuilder = new StringBuilder("Weapon definition lacks ammo data properties for given ammo definition: ");
						stringBuilder.Append(ammoDefinition.Id.SubtypeName);
						MyDefinitionErrors.Add(value.Context, stringBuilder.ToString(), TErrorSeverity.Critical);
					}
				}
			}
		}

		private void PostprocessComponentGroups()
		{
			foreach (KeyValuePair<MyDefinitionId, MyComponentGroupDefinition> componentGroup in m_definitions.m_componentGroups)
			{
				MyComponentGroupDefinition value = componentGroup.Value;
				value.Postprocess();
				if (value.IsValid)
				{
					int componentNumber = value.GetComponentNumber();
					for (int i = 1; i <= componentNumber; i++)
					{
						MyComponentDefinition componentDefinition = value.GetComponentDefinition(i);
						m_definitions.m_componentGroupMembers.Add(componentDefinition.Id, new MyTuple<int, MyComponentGroupDefinition>(i, value));
					}
				}
			}
		}

		private void PostprocessComponentBlocks()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyComponentBlockEntry> enumerator = m_definitions.m_componentBlockEntries.GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (!componentBlockEntry.Enabled)
				{
					continue;
				}
				MyObjectBuilderType type = MyObjectBuilderType.Parse(componentBlockEntry.Type);
				MyDefinitionId myDefinitionId = new MyDefinitionId(type, componentBlockEntry.Subtype);
				m_definitions.m_componentBlocks.Add(myDefinitionId);
				if (componentBlockEntry.Main)
				{
					MyCubeBlockDefinition blockDefinition = null;
					TryGetCubeBlockDefinition(myDefinitionId, out blockDefinition);
					if (blockDefinition.Components.Length == 1 && blockDefinition.Components[0].Count == 1)
=======
				while (enumerator.MoveNext())
				{
					MyComponentBlockEntry current = enumerator.get_Current();
					if (!current.Enabled)
					{
						continue;
					}
					MyObjectBuilderType type = MyObjectBuilderType.Parse(current.Type);
					MyDefinitionId myDefinitionId = new MyDefinitionId(type, current.Subtype);
					m_definitions.m_componentBlocks.Add(myDefinitionId);
					if (current.Main)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						m_definitions.m_componentIdToBlock[blockDefinition.Components[0].Definition.Id] = blockDefinition;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_definitions.m_componentBlockEntries.Clear();
		}

		private void PostprocessBlueprints()
		{
			CachingList<MyBlueprintDefinitionBase> cachingList = new CachingList<MyBlueprintDefinitionBase>();
			foreach (KeyValuePair<MyDefinitionId, MyBlueprintDefinitionBase> item in m_definitions.m_blueprintsById)
			{
				MyBlueprintDefinitionBase value = item.Value;
				if (value.PostprocessNeeded)
				{
					cachingList.Add(value);
				}
			}
			cachingList.ApplyAdditions();
			int num = -1;
			while (cachingList.Count != 0 && cachingList.Count != num)
			{
				num = cachingList.Count;
				foreach (MyBlueprintDefinitionBase item2 in cachingList)
				{
					_ = item2 is MyCompositeBlueprintDefinition;
					item2.Postprocess();
					if (!item2.PostprocessNeeded)
					{
						cachingList.Remove(item2);
					}
				}
				cachingList.ApplyRemovals();
			}
			if (cachingList.Count == 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder("Following blueprints could not be post-processed: ");
			foreach (MyBlueprintDefinitionBase item3 in cachingList)
			{
				stringBuilder.Append(item3.Id.ToString());
				stringBuilder.Append(", ");
			}
			MyDefinitionErrors.Add(MyModContext.BaseGame, stringBuilder.ToString(), TErrorSeverity.Error);
		}

		private void PostprocessBlockVariantGroups()
		{
			foreach (KeyValuePair<string, MyBlockVariantGroup> blockVariantGroup in m_definitions.m_blockVariantGroups)
			{
				blockVariantGroup.Value.Postprocess();
			}
		}

		private void PostprocessRadialMenus()
		{
			MyRadialMenu myRadialMenu = m_definitions.m_radialMenuDefinitions[new MyDefinitionId(typeof(MyObjectBuilder_RadialMenu), "Toolbar")];
<<<<<<< HEAD
			HashSet<MyBlockVariantGroup> hashSet = new HashSet<MyBlockVariantGroup>();
=======
			HashSet<MyBlockVariantGroup> val = new HashSet<MyBlockVariantGroup>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MyRadialMenuSection currentSection in myRadialMenu.CurrentSections)
			{
				foreach (MyRadialMenuItemCubeBlock item5 in currentSection.Items)
				{
<<<<<<< HEAD
					hashSet.Add(item5.BlockVariantGroup);
				}
			}
			int num = 0;
=======
					val.Add(item5.BlockVariantGroup);
				}
			}
			int num = 1;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			int num2 = -1;
			int num3 = -1;
			int num4 = -1;
			foreach (var (_, myBlockVariantGroup2) in m_definitions.m_blockVariantGroups)
			{
<<<<<<< HEAD
				if (!hashSet.Contains(myBlockVariantGroup2))
				{
					if ((num4 == -1 && myBlockVariantGroup2.AvailableInSurvival) || num2 == -1 || num3 == -1)
					{
						num++;
					}
					if (num3 == -1)
					{
						MyRadialMenuSection item = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute(string.Format(MyTexts.GetString(MySpaceTexts.RadialMenuSectionTitle_Modded), num)));
=======
				if (!val.Contains(myBlockVariantGroup2))
				{
					if (num3 == -1)
					{
						MyRadialMenuSection item = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute(string.Format(MyTexts.GetString(MySpaceTexts.RadialMenuSectionTitle_Modded), num++)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						myRadialMenu.SectionsComplete.Add(item);
						num3 = myRadialMenu.SectionsComplete.IndexOf(item);
					}
					if (num2 == -1)
<<<<<<< HEAD
					{
						MyRadialMenuSection item2 = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute(string.Format(MyTexts.GetString(MySpaceTexts.RadialMenuSectionTitle_Modded), num)));
						myRadialMenu.SectionsCreative.Add(item2);
						num2 = myRadialMenu.SectionsCreative.IndexOf(item2);
					}
					if (num4 == -1 && myBlockVariantGroup2.AvailableInSurvival)
					{
						MyRadialMenuSection item3 = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute(string.Format(MyTexts.GetString(MySpaceTexts.RadialMenuSectionTitle_Modded), num)));
=======
					{
						MyRadialMenuSection item2 = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute(string.Format(MyTexts.GetString(MySpaceTexts.RadialMenuSectionTitle_Modded), num++)));
						myRadialMenu.SectionsCreative.Add(item2);
						num2 = myRadialMenu.SectionsCreative.IndexOf(item2);
					}
					if (num4 == -1 && myBlockVariantGroup2.AvailableInSurvival)
					{
						MyRadialMenuSection item3 = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute(string.Format(MyTexts.GetString(MySpaceTexts.RadialMenuSectionTitle_Modded), num++)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						myRadialMenu.SectionsSurvival.Add(item3);
						num4 = myRadialMenu.SectionsSurvival.IndexOf(item3);
					}
					MyRadialMenuItemCubeBlock myRadialMenuItemCubeBlock2 = new MyRadialMenuItemCubeBlock();
					myRadialMenuItemCubeBlock2.Init(new MyObjectBuilder_RadialMenuItemCubeBlock
					{
						Id = new SerializableDefinitionId(typeof(MyObjectBuilder_RadialMenuItemCubeBlock), myBlockVariantGroup2.Id.SubtypeName),
						Icons = new List<string>()
					});
<<<<<<< HEAD
					myRadialMenuItemCubeBlock2.Icons.Add(myBlockVariantGroup2.PrimaryGUIBlock.Icons.FirstOrDefault() ?? string.Empty);
=======
					myRadialMenuItemCubeBlock2.Icons.Add(Enumerable.FirstOrDefault<string>((IEnumerable<string>)myBlockVariantGroup2.PrimaryGUIBlock.Icons) ?? string.Empty);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (num3 != -1)
					{
						myRadialMenu.SectionsComplete[num3].Items.Add(myRadialMenuItemCubeBlock2);
					}
					if (num2 != -1)
					{
						myRadialMenu.SectionsCreative[num2].Items.Add(myRadialMenuItemCubeBlock2);
					}
					if (myBlockVariantGroup2.AvailableInSurvival && num4 != -1)
					{
						myRadialMenu.SectionsSurvival[num4].Items.Add(myRadialMenuItemCubeBlock2);
					}
					if (myRadialMenu.SectionsComplete[num3].Items.Count >= 8)
<<<<<<< HEAD
					{
						num3 = -1;
=======
					{
						num3 = -1;
					}
					if (myRadialMenu.SectionsCreative[num2].Items.Count >= 8)
					{
						num2 = -1;
					}
					if (myBlockVariantGroup2.AvailableInSurvival && myRadialMenu.SectionsSurvival[num4].Items.Count >= 8)
					{
						num4 = -1;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					if (myRadialMenu.SectionsCreative[num2].Items.Count >= 8)
					{
						num2 = -1;
					}
					if (myBlockVariantGroup2.AvailableInSurvival && myRadialMenu.SectionsSurvival[num4].Items.Count >= 8)
					{
						num4 = -1;
					}
				}
			}
			List<MyDefinitionId> list = new List<MyDefinitionId>();
			foreach (var (item4, myRadialMenu3) in m_definitions.m_radialMenuDefinitions)
			{
				myRadialMenu3.Postprocess();
				if (myRadialMenu3.SectionsComplete.Count == 0)
				{
					list.Add(item4);
				}
			}
<<<<<<< HEAD
=======
			List<MyDefinitionId> list = new List<MyDefinitionId>();
			foreach (var (item4, myRadialMenu3) in m_definitions.m_radialMenuDefinitions)
			{
				myRadialMenu3.Postprocess();
				if (myRadialMenu3.SectionsComplete.Count == 0)
				{
					list.Add(item4);
				}
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MyDefinitionId item6 in list)
			{
				m_definitions.m_radialMenuDefinitions.Remove(item6);
			}
		}

		private void AddEntriesToBlueprintClasses()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<BlueprintClassEntry> enumerator = m_definitions.m_blueprintClassEntries.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					BlueprintClassEntry current = enumerator.get_Current();
					if (current.Enabled)
					{
						MyBlueprintClassDefinition value = null;
						MyBlueprintDefinitionBase myBlueprintDefinitionBase = null;
						MyDefinitionId key = new MyDefinitionId(typeof(MyObjectBuilder_BlueprintClassDefinition), current.Class);
						m_definitions.m_blueprintClasses.TryGetValue(key, out value);
						myBlueprintDefinitionBase = FindBlueprintByClassEntry(current);
						if (myBlueprintDefinitionBase != null)
						{
							value?.AddBlueprint(myBlueprintDefinitionBase);
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_definitions.m_blueprintClassEntries.Clear();
			foreach (KeyValuePair<MyDefinitionId, MyDefinitionBase> item in m_definitions.m_definitionsById)
			{
				(item.Value as MyProductionBlockDefinition)?.LoadPostProcess();
			}
		}

		private MyBlueprintDefinitionBase FindBlueprintByClassEntry(BlueprintClassEntry blueprintClassEntry)
		{
			if (blueprintClassEntry.TypeId.IsNull)
			{
				MyBlueprintDefinitionBase value = null;
				MyDefinitionId key = new MyDefinitionId(typeof(MyObjectBuilder_BlueprintDefinition), blueprintClassEntry.BlueprintSubtypeId);
				m_definitions.m_blueprintsById.TryGetValue(key, out value);
				if (value == null)
				{
					key = new MyDefinitionId(typeof(MyObjectBuilder_CompositeBlueprintDefinition), blueprintClassEntry.BlueprintSubtypeId);
					m_definitions.m_blueprintsById.TryGetValue(key, out value);
				}
				return value;
			}
			MyDefinitionId blueprintId = new MyDefinitionId(blueprintClassEntry.TypeId, blueprintClassEntry.BlueprintSubtypeId);
			return GetBlueprintDefinition(blueprintId);
		}

		private void AddEntriesToEnvironmentItemClasses()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<EnvironmentItemsEntry> enumerator = m_definitions.m_environmentItemsEntries.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
<<<<<<< HEAD
					MyEnvironmentItemsDefinition definition = null;
					MyDefinitionId defId = new MyDefinitionId(MyObjectBuilderType.Parse(environmentItemsEntry.Type), environmentItemsEntry.Subtype);
					if (!TryGetDefinition<MyEnvironmentItemsDefinition>(defId, out definition))
=======
					EnvironmentItemsEntry current = enumerator.get_Current();
					if (current.Enabled)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyEnvironmentItemsDefinition definition = null;
						MyDefinitionId defId = new MyDefinitionId(MyObjectBuilderType.Parse(current.Type), current.Subtype);
						if (!TryGetDefinition<MyEnvironmentItemsDefinition>(defId, out definition))
						{
							string message = "Environment items definition " + defId.ToString() + " not found!";
							MyDefinitionErrors.Add(MyModContext.BaseGame, message, TErrorSeverity.Warning);
						}
						else if (FindEnvironmentItemByEntry(definition, current) != null)
						{
							MyStringHash orCompute = MyStringHash.GetOrCompute(current.ItemSubtype);
							definition.AddItemDefinition(orCompute, current.Frequency, recompute: false);
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			foreach (MyEnvironmentItemsDefinition item in GetDefinitionsOfType<MyEnvironmentItemsDefinition>())
			{
				item.RecomputeFrequencies();
			}
			m_definitions.m_environmentItemsEntries.Clear();
		}

		private MyEnvironmentItemDefinition FindEnvironmentItemByEntry(MyEnvironmentItemsDefinition itemsDefinition, EnvironmentItemsEntry envItemEntry)
		{
			MyDefinitionId defId = new MyDefinitionId(itemsDefinition.ItemDefinitionType, envItemEntry.ItemSubtype);
			MyEnvironmentItemDefinition definition = null;
			TryGetDefinition<MyEnvironmentItemDefinition>(defId, out definition);
			return definition;
		}

		private void InitBlockGroups()
		{
			List<string> list = new List<string>();
			foreach (var (item, myBlockVariantGroup2) in m_definitions.m_blockVariantGroups)
<<<<<<< HEAD
			{
				myBlockVariantGroup2.ResolveBlocks();
				if (myBlockVariantGroup2.Blocks.IsNullOrEmpty())
				{
					list.Add(item);
				}
			}
			foreach (string item2 in list)
			{
=======
			{
				myBlockVariantGroup2.ResolveBlocks();
				if (myBlockVariantGroup2.Blocks.IsNullOrEmpty())
				{
					list.Add(item);
				}
			}
			foreach (string item2 in list)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_definitions.m_blockVariantGroups.Remove(item2);
			}
			m_definitions.m_blockGroups = new Dictionary<string, MyCubeBlockDefinitionGroup>();
			for (int i = 0; i < m_definitions.m_cubeSizes.Length; i++)
			{
				foreach (KeyValuePair<MyDefinitionId, MyCubeBlockDefinition> item3 in m_definitions.m_uniqueCubeBlocksBySize[i])
				{
					MyCubeBlockDefinition value = item3.Value;
					MyCubeBlockDefinitionGroup value2 = null;
					if (!m_definitions.m_blockGroups.TryGetValue(value.BlockPairName, out value2))
					{
						value2 = new MyCubeBlockDefinitionGroup();
						m_definitions.m_blockGroups.Add(value.BlockPairName, value2);
					}
					value2[(MyCubeSize)i] = value;
				}
			}
			foreach (MyBlockVariantGroup value3 in m_definitions.m_blockVariantGroups.Values)
			{
				MyCubeBlockDefinition[] blocks = value3.Blocks;
				for (int j = 0; j < blocks.Length; j++)
				{
					blocks[j].BlockVariantsGroup = value3;
				}
			}
			for (int k = 0; k < m_definitions.m_cubeSizes.Length; k++)
			{
				foreach (MyCubeBlockDefinition value4 in m_definitions.m_uniqueCubeBlocksBySize[k].Values)
				{
					if (value4.BlockVariantsGroup != null || value4.BlockStages == null)
<<<<<<< HEAD
					{
						continue;
					}
					IEnumerable<MyCubeBlockDefinition> enumerable = from x in value4.BlockStages.Select(delegate(MyDefinitionId x)
						{
							MyCubeBlockDefinition obj = (MyCubeBlockDefinition)m_definitions.m_definitionsById.Get(x);
							if (obj == null)
							{
								MyLog.Default.Error($"Stage block {x} doesn't exist");
							}
							return obj;
						})
						where x != null
						select x;
					MyBlockVariantGroup group = enumerable.Select((MyCubeBlockDefinition x) => x.BlockVariantsGroup).FirstOrDefault((MyBlockVariantGroup x) => x != null);
					if (group == null)
					{
						if (!string.IsNullOrEmpty(value4.BlockPairName))
						{
							MyCubeBlockDefinitionGroup definitionGroup = GetDefinitionGroup(value4.BlockPairName);
							MyCubeSize[] values = MyEnum<MyCubeSize>.Values;
							foreach (MyCubeSize size in values)
							{
								MyBlockVariantGroup myBlockVariantGroup3;
								if ((myBlockVariantGroup3 = definitionGroup[size]?.BlockVariantsGroup) != null)
								{
									group = myBlockVariantGroup3;
									break;
								}
							}
						}
						if (group == null)
						{
							string text2 = value4.Id.SubtypeName;
							while (m_definitions.m_blockVariantGroups.ContainsKey(text2))
							{
								text2 += "_";
							}
							group = new MyBlockVariantGroup
							{
=======
					{
						continue;
					}
					IEnumerable<MyCubeBlockDefinition> enumerable = Enumerable.Where<MyCubeBlockDefinition>(Enumerable.Select<MyDefinitionId, MyCubeBlockDefinition>((IEnumerable<MyDefinitionId>)value4.BlockStages, (Func<MyDefinitionId, MyCubeBlockDefinition>)delegate(MyDefinitionId x)
					{
						MyCubeBlockDefinition obj = (MyCubeBlockDefinition)m_definitions.m_definitionsById.Get(x);
						if (obj == null)
						{
							MyLog.Default.Error($"Stage block {x} doesn't exist");
						}
						return obj;
					}), (Func<MyCubeBlockDefinition, bool>)((MyCubeBlockDefinition x) => x != null));
					MyBlockVariantGroup group = Enumerable.FirstOrDefault<MyBlockVariantGroup>(Enumerable.Select<MyCubeBlockDefinition, MyBlockVariantGroup>(enumerable, (Func<MyCubeBlockDefinition, MyBlockVariantGroup>)((MyCubeBlockDefinition x) => x.BlockVariantsGroup)), (Func<MyBlockVariantGroup, bool>)((MyBlockVariantGroup x) => x != null));
					if (group == null)
					{
						if (!string.IsNullOrEmpty(value4.BlockPairName))
						{
							MyCubeBlockDefinitionGroup definitionGroup = GetDefinitionGroup(value4.BlockPairName);
							MyCubeSize[] values = MyEnum<MyCubeSize>.Values;
							foreach (MyCubeSize size in values)
							{
								MyBlockVariantGroup myBlockVariantGroup3;
								if ((myBlockVariantGroup3 = definitionGroup[size]?.BlockVariantsGroup) != null)
								{
									group = myBlockVariantGroup3;
									break;
								}
							}
						}
						if (group == null)
						{
							string text2 = value4.Id.SubtypeName;
							while (m_definitions.m_blockVariantGroups.ContainsKey(text2))
							{
								text2 += "_";
							}
							group = new MyBlockVariantGroup
							{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								Id = new MyDefinitionId(typeof(MyObjectBuilder_BlockVariantGroup), text2)
							};
							m_definitions.m_blockVariantGroups.Add(text2, group);
						}
					}
					List<MyCubeBlockDefinition> blocksInGroup = new List<MyCubeBlockDefinition>();
					if (group.Blocks != null)
					{
						blocksInGroup.AddRange(group.Blocks);
					}
					blocksInGroup.Add(value4);
					value4.BlockVariantsGroup = group;
					foreach (MyCubeBlockDefinition item4 in enumerable)
					{
						if (string.IsNullOrEmpty(item4.BlockPairName))
						{
							AddBlock(item4);
							continue;
						}
						MyCubeBlockDefinitionGroup definitionGroup2 = Static.GetDefinitionGroup(item4.BlockPairName);
						MyCubeSize[] values = MyEnum<MyCubeSize>.Values;
						foreach (MyCubeSize size2 in values)
						{
							if (definitionGroup2[size2] != null)
							{
								AddBlock(definitionGroup2[size2]);
							}
						}
					}
					group.Blocks = blocksInGroup.ToArray();
					void AddBlock(MyCubeBlockDefinition b)
					{
						if (b.BlockVariantsGroup == null)
						{
							b.BlockVariantsGroup = group;
							blocksInGroup.Add(b);
						}
					}
				}
			}
			foreach (MyBlockVariantGroup value5 in m_definitions.m_blockVariantGroups.Values)
			{
				MyCubeBlockDefinition[] blocks = value5.Blocks;
				foreach (MyCubeBlockDefinition myCubeBlockDefinition in blocks)
				{
					if (!string.IsNullOrEmpty(myCubeBlockDefinition.BlockPairName))
					{
						MyCubeBlockDefinitionGroup myCubeBlockDefinitionGroup = m_definitions.m_blockGroups[myCubeBlockDefinition.BlockPairName];
						MyCubeSize size3 = ((myCubeBlockDefinition.CubeSize == MyCubeSize.Large) ? MyCubeSize.Small : MyCubeSize.Large);
						MyCubeBlockDefinition myCubeBlockDefinition2 = myCubeBlockDefinitionGroup[size3];
						if (myCubeBlockDefinition2 != null && myCubeBlockDefinition2.Public && myCubeBlockDefinition.Public && myCubeBlockDefinition.BlockVariantsGroup != myCubeBlockDefinition2.BlockVariantsGroup)
						{
							MyLog.Default.Error($"Block-pair {myCubeBlockDefinition.Id} {myCubeBlockDefinition2.Id} is not in the same block-variant group");
						}
					}
				}
			}
		}

		public void InitVoxelMaterials()
		{
			MyRenderVoxelMaterialData[] array = new MyRenderVoxelMaterialData[m_definitions.m_voxelMaterialsByName.Count];
			MyVoxelMaterialDefinition.ResetIndexing();
			int num = 0;
			foreach (KeyValuePair<string, MyVoxelMaterialDefinition> item in m_definitions.m_voxelMaterialsByName)
			{
				MyVoxelMaterialDefinition value = item.Value;
				value.AssignIndex();
				m_definitions.m_voxelMaterialsByIndex[value.Index] = value;
				if (value.IsRare)
				{
					m_definitions.m_voxelMaterialRareCount++;
				}
				array[num++] = value.RenderParams;
			}
			MyRenderProxy.CreateRenderVoxelMaterials(array);
		}

		private void LoadVoxelMaterials(string path, MyModContext context, List<MyVoxelMaterialDefinition> res)
		{
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = Load<MyObjectBuilder_Definitions>(path);
			for (int i = 0; i < myObjectBuilder_Definitions.VoxelMaterials.Length; i++)
			{
				MyDx11VoxelMaterialDefinition myDx11VoxelMaterialDefinition = new MyDx11VoxelMaterialDefinition();
				myDx11VoxelMaterialDefinition.Init(myObjectBuilder_Definitions.VoxelMaterials[i], context);
				res.Add(myDx11VoxelMaterialDefinition);
			}
		}

		public void ReloadVoxelMaterials()
		{
			using (m_voxelMaterialsLock.AcquireExclusiveUsing())
			{
				MyModContext baseGame = MyModContext.BaseGame;
				MyVoxelMaterialDefinition.ResetIndexing();
				MySandboxGame.Log.WriteLine("ReloadVoxelMaterials");
				List<MyVoxelMaterialDefinition> list = new List<MyVoxelMaterialDefinition>();
				LoadVoxelMaterials(Path.Combine(baseGame.ModPathData, "VoxelMaterials_asteroids.sbc"), baseGame, list);
				LoadVoxelMaterials(Path.Combine(baseGame.ModPathData, "VoxelMaterials_planetary.sbc"), baseGame, list);
				if (m_definitions.m_voxelMaterialsByIndex == null)
				{
					m_definitions.m_voxelMaterialsByIndex = new Dictionary<byte, MyVoxelMaterialDefinition>();
				}
				else
				{
					m_definitions.m_voxelMaterialsByIndex.Clear();
				}
				if (m_definitions.m_voxelMaterialsByName == null)
				{
					m_definitions.m_voxelMaterialsByName = new Dictionary<string, MyVoxelMaterialDefinition>();
				}
				else
				{
					m_definitions.m_voxelMaterialsByName.Clear();
				}
				foreach (MyVoxelMaterialDefinition item in list)
				{
					item.AssignIndex();
					m_definitions.m_voxelMaterialsByIndex[item.Index] = item;
					m_definitions.m_voxelMaterialsByName[item.Id.SubtypeName] = item;
				}
			}
		}

		public void UnloadData(bool clearPreloaded = false)
		{
			m_modDefinitionSets.Clear();
			MyCubeBlockDefinition.ClearPreloadedConstructionModels();
			m_definitions.Clear(unload: true);
			m_definitions.m_channelEnvironmentItemsDefs.Clear();
			if (clearPreloaded)
			{
				m_preloadedDefinitionBuilders.Clear();
			}
			m_decalObjectBuilders.Clear();
		}

		private T Load<T>(string path) where T : MyObjectBuilder_Base
		{
			T objectBuilder = null;
			MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder);
			MyObjectBuilder_Definitions definitions;
			if ((definitions = objectBuilder as MyObjectBuilder_Definitions) != null)
			{
				FilterUnsupportedDLCs(definitions);
			}
			return objectBuilder;
		}

		private T LoadWithProtobuffers<T>(string path) where T : MyObjectBuilder_Base
		{
			T objectBuilder = null;
			string path2 = path + "B5";
			if (MyFileSystem.FileExists(path2))
			{
				MyObjectBuilderSerializer.DeserializePB<T>(path2, out objectBuilder);
				if (objectBuilder == null)
				{
					MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder);
					if (objectBuilder != null)
					{
						MyObjectBuilderSerializer.SerializePB(path2, compress: false, objectBuilder);
					}
				}
			}
			else
			{
				MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder);
				if (objectBuilder != null && !MyFileSystem.FileExists(path2))
				{
					MyObjectBuilderSerializer.SerializePB(path2, compress: false, objectBuilder);
				}
			}
			return objectBuilder;
		}

		private T Load<T>(string path, Type useType) where T : MyObjectBuilder_Base
		{
			MyObjectBuilder_Base objectBuilder = null;
			MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder, useType);
			if (objectBuilder == null)
			{
				return null;
			}
			return objectBuilder as T;
		}

		private void InitDefinitionsCompat(MyModContext context, MyObjectBuilder_DefinitionBase[] definitions)
		{
			if (definitions != null)
			{
				foreach (MyObjectBuilder_DefinitionBase builder in definitions)
				{
					MyDefinitionBase def = InitDefinition<MyDefinitionBase>(context, builder);
					m_currentLoadingSet.AddDefinition(def);
				}
			}
		}

		private static void InitAmmoMagazines(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_AmmoMagazineDefinition[] magazines, bool failOnDebug = true)
		{
			MyAmmoMagazineDefinition[] array = new MyAmmoMagazineDefinition[magazines.Length];
			for (int i = 0; i < magazines.Length; i++)
			{
				array[i] = InitDefinition<MyAmmoMagazineDefinition>(context, magazines[i]);
				Check(array[i].Id.TypeId == typeof(MyObjectBuilder_AmmoMagazine), array[i].Id.TypeId, failOnDebug, "Unknown type '{0}'");
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private static void InitShipSounds(MyModContext context, DefinitionDictionary<MyShipSoundsDefinition> output, MyObjectBuilder_ShipSoundsDefinition[] shipGroups, bool failOnDebug = true)
		{
			MyShipSoundsDefinition[] array = new MyShipSoundsDefinition[shipGroups.Length];
			for (int i = 0; i < shipGroups.Length; i++)
			{
				array[i] = InitDefinition<MyShipSoundsDefinition>(context, shipGroups[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private static void InitShipSoundSystem(MyModContext context, ref MyShipSoundSystemDefinition output, MyObjectBuilder_ShipSoundSystemDefinition shipSystem, bool failOnDebug = true)
		{
			MyShipSoundSystemDefinition myShipSoundSystemDefinition = (output = InitDefinition<MyShipSoundSystemDefinition>(context, shipSystem));
		}

		public void SetShipSoundSystem()
		{
			MyShipSoundComponent.ClearShipSounds();
			foreach (DefinitionSet value in m_modDefinitionSets.Values)
			{
				if (value.m_shipSounds == null || value.m_shipSounds.Count <= 0)
				{
					continue;
				}
				foreach (KeyValuePair<MyDefinitionId, MyShipSoundsDefinition> shipSound in value.m_shipSounds)
				{
					MyShipSoundComponent.AddShipSounds(shipSound.Value);
				}
				if (value.m_shipSoundSystem != null)
				{
					MyShipSoundComponent.SetDefinition(value.m_shipSoundSystem);
				}
			}
			MyShipSoundComponent.ActualizeGroups();
		}

		private static void InitAnimations(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_AnimationDefinition[] animations, Dictionary<string, Dictionary<string, MyAnimationDefinition>> animationsBySkeletonType, bool failOnDebug = true)
		{
			MyAnimationDefinition[] array = new MyAnimationDefinition[animations.Length];
			for (int i = 0; i < animations.Length; i++)
			{
				array[i] = InitDefinition<MyAnimationDefinition>(context, animations[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
				_ = context.IsBaseGame;
				Static.m_currentLoadingSet.AddOrRelaceDefinition(array[i]);
			}
			MyAnimationDefinition[] array2 = array;
			foreach (MyAnimationDefinition myAnimationDefinition in array2)
			{
				string[] supportedSkeletons = myAnimationDefinition.SupportedSkeletons;
				foreach (string key in supportedSkeletons)
				{
					if (!animationsBySkeletonType.ContainsKey(key))
					{
						animationsBySkeletonType.Add(key, new Dictionary<string, MyAnimationDefinition>());
					}
					animationsBySkeletonType[key][myAnimationDefinition.Id.SubtypeName] = myAnimationDefinition;
				}
			}
		}

		private static void InitDebris(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_DebrisDefinition[] debris, bool failOnDebug = true)
		{
			MyDebrisDefinition[] array = new MyDebrisDefinition[debris.Length];
			for (int i = 0; i < debris.Length; i++)
			{
				array[i] = InitDefinition<MyDebrisDefinition>(context, debris[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private static void InitEdges(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_EdgesDefinition[] edges, bool failOnDebug = true)
		{
			MyEdgesDefinition[] array = new MyEdgesDefinition[edges.Length];
			for (int i = 0; i < edges.Length; i++)
			{
				array[i] = InitDefinition<MyEdgesDefinition>(context, edges[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private void InitBlockPositions(Dictionary<string, Vector2I> outputBlockPositions, MyBlockPosition[] positions, bool failOnDebug = true)
		{
			foreach (MyBlockPosition myBlockPosition in positions)
			{
				Check(!outputBlockPositions.ContainsKey(myBlockPosition.Name), myBlockPosition.Name, failOnDebug);
				outputBlockPositions[myBlockPosition.Name] = myBlockPosition.Position;
			}
		}

		private void InitCategoryClasses(MyModContext context, List<MyGuiBlockCategoryDefinition> categories, MyObjectBuilder_GuiBlockCategoryDefinition[] classes, bool failOnDebug = true)
		{
			foreach (MyObjectBuilder_GuiBlockCategoryDefinition myObjectBuilder_GuiBlockCategoryDefinition in classes)
			{
				if (myObjectBuilder_GuiBlockCategoryDefinition.Public || MyFakes.ENABLE_NON_PUBLIC_CATEGORY_CLASSES)
				{
					MyGuiBlockCategoryDefinition item = InitDefinition<MyGuiBlockCategoryDefinition>(context, myObjectBuilder_GuiBlockCategoryDefinition);
					categories.Add(item);
				}
			}
		}

		private void InitSounds(MyModContext context, DefinitionDictionary<MyAudioDefinition> output, MyObjectBuilder_AudioDefinition[] classes, bool failOnDebug = true)
		{
			foreach (MyObjectBuilder_AudioDefinition myObjectBuilder_AudioDefinition in classes)
			{
				output[myObjectBuilder_AudioDefinition.Id] = InitDefinition<MyAudioDefinition>(context, myObjectBuilder_AudioDefinition);
			}
		}

		private void InitParticleEffects(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_ParticleEffect[] classes, bool failOnDebug = true)
		{
			if (!m_transparentMaterialsInitialized)
			{
				CreateTransparentMaterials();
				m_transparentMaterialsInitialized = true;
			}
<<<<<<< HEAD
			foreach (IGrouping<string, MyObjectBuilder_ParticleEffect> item in from x in classes
				group x by x.Id.SubtypeName)
			{
				item.Count();
				_ = 1;
			}
			foreach (IGrouping<int, MyObjectBuilder_ParticleEffect> item2 in from x in classes
				group x by x.ParticleId)
			{
				item2.Count();
=======
			foreach (IGrouping<string, MyObjectBuilder_ParticleEffect> item in Enumerable.GroupBy<MyObjectBuilder_ParticleEffect, string>((IEnumerable<MyObjectBuilder_ParticleEffect>)classes, (Func<MyObjectBuilder_ParticleEffect, string>)((MyObjectBuilder_ParticleEffect x) => x.Id.SubtypeName)))
			{
				Enumerable.Count<MyObjectBuilder_ParticleEffect>((IEnumerable<MyObjectBuilder_ParticleEffect>)item);
				_ = 1;
			}
			foreach (IGrouping<int, MyObjectBuilder_ParticleEffect> item2 in Enumerable.GroupBy<MyObjectBuilder_ParticleEffect, int>((IEnumerable<MyObjectBuilder_ParticleEffect>)classes, (Func<MyObjectBuilder_ParticleEffect, int>)((MyObjectBuilder_ParticleEffect x) => x.ParticleId)))
			{
				Enumerable.Count<MyObjectBuilder_ParticleEffect>((IEnumerable<MyObjectBuilder_ParticleEffect>)item2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				_ = 1;
			}
			foreach (MyObjectBuilder_ParticleEffect builder in classes)
			{
				MyParticleEffectData data = new MyParticleEffectData();
				MyParticleEffectDataSerializer.DeserializeFromObjectBuilder(data, builder);
				MyParticleEffectsLibrary.Add(data);
			}
		}

		private void InitSoundCategories(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_SoundCategoryDefinition[] categories, bool failOnDebug = true)
		{
			foreach (MyObjectBuilder_SoundCategoryDefinition myObjectBuilder_SoundCategoryDefinition in categories)
			{
				MySoundCategoryDefinition value = InitDefinition<MySoundCategoryDefinition>(context, myObjectBuilder_SoundCategoryDefinition);
				Check(!output.ContainsKey(myObjectBuilder_SoundCategoryDefinition.Id), myObjectBuilder_SoundCategoryDefinition.Id, failOnDebug);
				output[myObjectBuilder_SoundCategoryDefinition.Id] = value;
			}
		}

		private void InitLCDTextureCategories(MyModContext context, DefinitionSet definitions, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_LCDTextureDefinition[] categories, bool failOnDebug = true)
		{
			foreach (MyObjectBuilder_LCDTextureDefinition myObjectBuilder_LCDTextureDefinition in categories)
			{
				MyLCDTextureDefinition myLCDTextureDefinition = InitDefinition<MyLCDTextureDefinition>(context, myObjectBuilder_LCDTextureDefinition);
				Check(!output.ContainsKey(myObjectBuilder_LCDTextureDefinition.Id), myObjectBuilder_LCDTextureDefinition.Id, failOnDebug);
				output[myObjectBuilder_LCDTextureDefinition.Id] = myLCDTextureDefinition;
				definitions.AddOrRelaceDefinition(myLCDTextureDefinition);
			}
		}

		private void InitBlueprintClasses(MyModContext context, DefinitionDictionary<MyBlueprintClassDefinition> output, MyObjectBuilder_BlueprintClassDefinition[] classes, bool failOnDebug = true)
		{
			foreach (MyObjectBuilder_BlueprintClassDefinition myObjectBuilder_BlueprintClassDefinition in classes)
			{
				MyBlueprintClassDefinition value = InitDefinition<MyBlueprintClassDefinition>(context, myObjectBuilder_BlueprintClassDefinition);
				Check(!output.ContainsKey(myObjectBuilder_BlueprintClassDefinition.Id), myObjectBuilder_BlueprintClassDefinition.Id, failOnDebug);
				output[myObjectBuilder_BlueprintClassDefinition.Id] = value;
			}
		}

		private void InitBlueprintClassEntries(MyModContext context, HashSet<BlueprintClassEntry> output, BlueprintClassEntry[] entries, bool failOnDebug = true)
		{
			foreach (BlueprintClassEntry blueprintClassEntry in entries)
			{
				Check(!output.Contains(blueprintClassEntry), blueprintClassEntry, failOnDebug);
				output.Add(blueprintClassEntry);
			}
		}

		private void InitEnvironmentItemsEntries(MyModContext context, HashSet<EnvironmentItemsEntry> output, EnvironmentItemsEntry[] entries, bool failOnDebug = true)
		{
			foreach (EnvironmentItemsEntry environmentItemsEntry in entries)
			{
				Check(!output.Contains(environmentItemsEntry), environmentItemsEntry, failOnDebug);
				output.Add(environmentItemsEntry);
			}
		}

		private void InitBlueprints(MyModContext context, Dictionary<MyDefinitionId, MyBlueprintDefinitionBase> output, DefinitionDictionary<MyBlueprintDefinitionBase> blueprintsByResult, MyObjectBuilder_BlueprintDefinition[] blueprints, bool failOnDebug = true)
		{
			for (int i = 0; i < blueprints.Length; i++)
			{
				MyBlueprintDefinitionBase myBlueprintDefinitionBase = InitDefinition<MyBlueprintDefinitionBase>(context, blueprints[i]);
				Check(!output.ContainsKey(myBlueprintDefinitionBase.Id), myBlueprintDefinitionBase.Id, failOnDebug);
				output[myBlueprintDefinitionBase.Id] = myBlueprintDefinitionBase;
				if (myBlueprintDefinitionBase.Results.Length != 1)
				{
					continue;
				}
				bool flag = true;
				MyDefinitionId id = myBlueprintDefinitionBase.Results[0].Id;
				if (blueprintsByResult.TryGetValue(id, out var value))
				{
					if (value.IsPrimary == myBlueprintDefinitionBase.IsPrimary)
					{
						string text = (myBlueprintDefinitionBase.IsPrimary ? "primary" : "non-primary");
						string msg = string.Concat("Overriding ", text, " blueprint \"", value, "\" with ", text, " blueprint \"", myBlueprintDefinitionBase, "\"");
						MySandboxGame.Log.WriteLine(msg);
					}
					else if (value.IsPrimary)
					{
						flag = false;
					}
				}
				if (flag)
				{
					blueprintsByResult[id] = myBlueprintDefinitionBase;
				}
			}
		}

		private static void InitComponents(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_ComponentDefinition[] components, bool failOnDebug = true)
		{
			MyComponentDefinition[] array = new MyComponentDefinition[components.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyComponentDefinition>(context, components[i]);
				Check(array[i].Id.TypeId == typeof(MyObjectBuilder_Component), array[i].Id.TypeId, failOnDebug, "Unknown type '{0}'");
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
				if (!context.IsBaseGame)
				{
					MySandboxGame.Log.WriteLine("Loaded component: " + array[i].Id);
				}
			}
		}

		private void InitConfiguration(DefinitionSet definitionSet, MyObjectBuilder_Configuration configuration)
		{
			definitionSet.m_cubeSizes[1] = configuration.CubeSizes.Small;
			definitionSet.m_cubeSizes[0] = configuration.CubeSizes.Large;
			definitionSet.m_cubeSizesOriginal[1] = ((configuration.CubeSizes.SmallOriginal > 0f) ? configuration.CubeSizes.SmallOriginal : configuration.CubeSizes.Small);
			definitionSet.m_cubeSizesOriginal[0] = configuration.CubeSizes.Large;
			for (int i = 0; i < 2; i++)
			{
				bool flag = i == 0;
				MyObjectBuilder_Configuration.BaseBlockSettings baseBlockSettings = (flag ? configuration.BaseBlockPrefabs : configuration.BaseBlockPrefabsSurvival);
				AddBasePrefabName(definitionSet, MyCubeSize.Small, isStatic: true, flag, baseBlockSettings.SmallStatic);
				AddBasePrefabName(definitionSet, MyCubeSize.Small, isStatic: false, flag, baseBlockSettings.SmallDynamic);
				AddBasePrefabName(definitionSet, MyCubeSize.Large, isStatic: true, flag, baseBlockSettings.LargeStatic);
				AddBasePrefabName(definitionSet, MyCubeSize.Large, isStatic: false, flag, baseBlockSettings.LargeDynamic);
			}
			if (configuration.LootBag != null)
			{
				definitionSet.m_lootBagDefinition = new MyLootBagDefinition();
				definitionSet.m_lootBagDefinition.Init(configuration.LootBag);
			}
		}

		private static void InitContainerTypes(MyModContext context, DefinitionDictionary<MyContainerTypeDefinition> output, MyObjectBuilder_ContainerTypeDefinition[] containers, bool failOnDebug = true)
		{
			foreach (MyObjectBuilder_ContainerTypeDefinition myObjectBuilder_ContainerTypeDefinition in containers)
			{
				Check(!output.ContainsKey(myObjectBuilder_ContainerTypeDefinition.Id), myObjectBuilder_ContainerTypeDefinition.Id, failOnDebug);
				MyContainerTypeDefinition value = InitDefinition<MyContainerTypeDefinition>(context, myObjectBuilder_ContainerTypeDefinition);
				output[myObjectBuilder_ContainerTypeDefinition.Id] = value;
			}
		}

		private static void InitCubeBlocks(MyModContext context, MyObjectBuilder_CubeBlockDefinition[] cubeBlocks)
		{
			MyCubeBlockDefinition.ClearPreloadedConstructionModels();
			foreach (MyObjectBuilder_CubeBlockDefinition myObjectBuilder_CubeBlockDefinition in cubeBlocks)
			{
				myObjectBuilder_CubeBlockDefinition.BlockPairName = myObjectBuilder_CubeBlockDefinition.BlockPairName ?? myObjectBuilder_CubeBlockDefinition.DisplayName;
<<<<<<< HEAD
				if (myObjectBuilder_CubeBlockDefinition.Components.Where((MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent component) => component.Subtype == "Computer").Count() != 0)
=======
				if (Enumerable.Count<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent>(Enumerable.Where<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent>((IEnumerable<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent>)myObjectBuilder_CubeBlockDefinition.Components, (Func<MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent, bool>)((MyObjectBuilder_CubeBlockDefinition.CubeBlockComponent component) => component.Subtype == "Computer"))) != 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					Type producedType = MyCubeBlockFactory.GetProducedType(myObjectBuilder_CubeBlockDefinition.Id.TypeId);
					if (!producedType.IsSubclassOf(typeof(MyTerminalBlock)) && producedType != typeof(MyTerminalBlock))
					{
						string message = string.Format(MyTexts.GetString(MySpaceTexts.DefinitionError_BlockWithComputerNotTerminalBlock), myObjectBuilder_CubeBlockDefinition.DisplayName);
						MyDefinitionErrors.Add(context, message, TErrorSeverity.Error);
					}
				}
			}
		}

		private static void InitWeapons(MyModContext context, DefinitionDictionary<MyWeaponDefinition> output, MyObjectBuilder_WeaponDefinition[] weapons, bool failOnDebug)
		{
			MyWeaponDefinition[] array = new MyWeaponDefinition[weapons.Length];
			for (int i = 0; i < weapons.Length; i++)
			{
				array[i] = InitDefinition<MyWeaponDefinition>(context, weapons[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private void InitGridCreators(MyModContext context, DefinitionDictionary<MyGridCreateToolDefinition> gridCreateDefinitions, DefinitionDictionary<MyDefinitionBase> definitionsById, MyObjectBuilder_GridCreateToolDefinition[] gridCreators, bool failOnDebug)
		{
			foreach (MyObjectBuilder_GridCreateToolDefinition myObjectBuilder_GridCreateToolDefinition in gridCreators)
			{
				_ = gridCreateDefinitions.ContainsKey(myObjectBuilder_GridCreateToolDefinition.Id) && failOnDebug;
				MyGridCreateToolDefinition value = InitDefinition<MyGridCreateToolDefinition>(context, myObjectBuilder_GridCreateToolDefinition);
				gridCreateDefinitions[myObjectBuilder_GridCreateToolDefinition.Id] = value;
				definitionsById[myObjectBuilder_GridCreateToolDefinition.Id] = value;
			}
		}

		private static void InitAmmos(MyModContext context, DefinitionDictionary<MyAmmoDefinition> output, MyObjectBuilder_AmmoDefinition[] ammos, bool failOnDebug = true)
		{
			MyAmmoDefinition[] array = new MyAmmoDefinition[ammos.Length];
			for (int i = 0; i < ammos.Length; i++)
			{
				array[i] = InitDefinition<MyAmmoDefinition>(context, ammos[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private void FixGeneratedBlocksIntegrity(DefinitionDictionary<MyCubeBlockDefinition> cubeBlocks)
		{
			foreach (KeyValuePair<MyDefinitionId, MyCubeBlockDefinition> cubeBlock in cubeBlocks)
			{
				MyCubeBlockDefinition value = cubeBlock.Value;
				if (value.GeneratedBlockDefinitions == null)
<<<<<<< HEAD
				{
					continue;
				}
				MyDefinitionId[] generatedBlockDefinitions = value.GeneratedBlockDefinitions;
				foreach (MyDefinitionId defId in generatedBlockDefinitions)
				{
=======
				{
					continue;
				}
				MyDefinitionId[] generatedBlockDefinitions = value.GeneratedBlockDefinitions;
				foreach (MyDefinitionId defId in generatedBlockDefinitions)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (TryGetCubeBlockDefinition(defId, out var blockDefinition) && !(blockDefinition.GeneratedBlockType == MyStringId.GetOrCompute("pillar")))
					{
						blockDefinition.Components = value.Components;
						blockDefinition.MaxIntegrity = value.MaxIntegrity;
					}
				}
			}
		}

		private static void PrepareBlockBlueprints(MyModContext context, Dictionary<MyDefinitionId, MyBlueprintDefinitionBase> output, Dictionary<MyDefinitionId, MyCubeBlockDefinition> cubeBlocks, bool failOnDebug = true)
		{
			foreach (KeyValuePair<MyDefinitionId, MyCubeBlockDefinition> cubeBlock in cubeBlocks)
			{
				MyCubeBlockDefinition value = cubeBlock.Value;
				if (!context.IsBaseGame)
				{
					MySandboxGame.Log.WriteLine("Loading cube block: " + cubeBlock.Key);
				}
				if (!MyFakes.ENABLE_NON_PUBLIC_BLOCKS && !value.Public)
				{
					continue;
				}
				MyCubeBlockDefinition uniqueVersion = value.UniqueVersion;
				Check(!output.ContainsKey(value.Id), value.Id, failOnDebug);
				if (!output.ContainsKey(uniqueVersion.Id))
				{
					MyBlueprintDefinitionBase myBlueprintDefinitionBase = MakeBlueprintFromComponentStack(context, uniqueVersion);
					if (myBlueprintDefinitionBase != null)
					{
						output[myBlueprintDefinitionBase.Id] = myBlueprintDefinitionBase;
					}
				}
			}
		}

		private static void InitEnvironment(MyModContext context, DefinitionSet defSet, MyObjectBuilder_EnvironmentDefinition[] objBuilder, bool failOnDebug = true)
		{
			foreach (MyObjectBuilder_EnvironmentDefinition builder in objBuilder)
			{
				MyEnvironmentDefinition def = InitDefinition<MyEnvironmentDefinition>(context, builder);
				defSet.AddDefinition(def);
			}
		}

		private static void LoadDroneBehaviorPresets(MyModContext context, DefinitionSet defSet, MyObjectBuilder_DroneBehaviorDefinition[] objBuilder, bool failOnDebug = true)
		{
			foreach (MyObjectBuilder_DroneBehaviorDefinition obj in objBuilder)
			{
				MyDroneAIDataStatic.SavePreset(preset: new MyDroneAIData(obj), key: obj.Id.SubtypeId);
			}
		}

		private static void InitGlobalEvents(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_GlobalEventDefinition[] events, bool failOnDebug = true)
		{
			MyGlobalEventDefinition[] array = new MyGlobalEventDefinition[events.Length];
			for (int i = 0; i < events.Length; i++)
			{
				array[i] = InitDefinition<MyGlobalEventDefinition>(context, events[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private static void InitHandItems(MyModContext context, DefinitionDictionary<MyHandItemDefinition> output, MyObjectBuilder_HandItemDefinition[] items, bool failOnDebug = true)
		{
			MyHandItemDefinition[] array = new MyHandItemDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyHandItemDefinition>(context, items[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private static void InitAssetModifiers(MyModContext context, DefinitionDictionary<MyAssetModifierDefinition> output, MyObjectBuilder_AssetModifierDefinition[] items, bool failOnDebug = true)
		{
			MyAssetModifierDefinition[] array = new MyAssetModifierDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyAssetModifierDefinition>(context, items[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private void InitResearchGroups(MyModContext context, ref DefinitionDictionary<MyResearchGroupDefinition> output, MyObjectBuilder_ResearchGroupDefinition[] items, bool failOnDebug)
		{
			MyResearchGroupDefinition[] array = new MyResearchGroupDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyResearchGroupDefinition>(context, items[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		public MyResearchGroupDefinition GetResearchGroup(string subtype)
		{
			MyDefinitionId key = new MyDefinitionId(typeof(MyObjectBuilder_ResearchGroupDefinition), subtype);
			MyResearchGroupDefinition value = null;
			m_definitions.m_researchGroupsDefinitions.TryGetValue(key, out value);
			return value;
		}

		private void InitContractTypes(MyModContext context, ref DefinitionDictionary<MyContractTypeDefinition> output, MyObjectBuilder_ContractTypeDefinition[] items, bool failOnDebug)
		{
			MyContractTypeDefinition[] array = new MyContractTypeDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyContractTypeDefinition>(context, items[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		public MyContractTypeDefinition GetContractType(string subtype)
		{
			MyDefinitionId key = new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), subtype);
			MyContractTypeDefinition value = null;
			m_definitions.m_contractTypesDefinitions.TryGetValue(key, out value);
			return value;
		}

		private void InitWeatherEffects(MyModContext context, ref DefinitionDictionary<MyWeatherEffectDefinition> output, MyObjectBuilder_WeatherEffectDefinition[] items, bool failOnDebug)
		{
			MyWeatherEffectDefinition[] array = new MyWeatherEffectDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyWeatherEffectDefinition>(context, items[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

<<<<<<< HEAD
		private void InitControllerSchema(MyModContext context, ref DefinitionDictionary<MyControllerSchemeDefinition> output, MyObjectBuilder_ControllerSchemeDefinition[] items, bool failOnDebug)
		{
			MyControllerSchemeDefinition[] array = new MyControllerSchemeDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyControllerSchemeDefinition>(context, items[i]);
=======
		private void InitChatBot(MyModContext context, ref DefinitionDictionary<MyChatBotResponseDefinition> output, MyObjectBuilder_ChatBotResponseDefinition[] items, bool failOnDebug)
		{
			MyChatBotResponseDefinition[] array = new MyChatBotResponseDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyChatBotResponseDefinition>(context, items[i]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		public MyWeatherEffectDefinition GetWeatherEffect(string subtype)
		{
			MyDefinitionId key = new MyDefinitionId(typeof(MyObjectBuilder_WeatherEffectDefinition), subtype);
			MyWeatherEffectDefinition value = null;
			m_definitions.m_weatherEffectsDefinitions.TryGetValue(key, out value);
			return value;
		}

		public ListReader<MyWeatherEffectDefinition> GetWeatherDefinitions()
		{
<<<<<<< HEAD
			return new ListReader<MyWeatherEffectDefinition>(m_definitions.m_weatherEffectsDefinitions.Values.ToList());
=======
			return new ListReader<MyWeatherEffectDefinition>(Enumerable.ToList<MyWeatherEffectDefinition>((IEnumerable<MyWeatherEffectDefinition>)m_definitions.m_weatherEffectsDefinitions.Values));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public MyFactionNameDefinition GetFactionName(string subtype)
		{
			MyDefinitionId key = new MyDefinitionId(typeof(MyFactionNameDefinition), subtype);
			MyFactionNameDefinition value = null;
			m_definitions.m_factionNameDefinitions.TryGetValue(key, out value);
			return value;
		}

<<<<<<< HEAD
		public ListReader<MyControllerSchemeDefinition> GetControllerSchemesAll()
		{
			return new ListReader<MyControllerSchemeDefinition>(m_definitions.m_controllerSchemeDefinitions.Values.ToList());
		}

		public List<MyControllerSchemeDefinition> GetControllerSchemesSelectable()
		{
			List<MyControllerSchemeDefinition> list = new List<MyControllerSchemeDefinition>();
			foreach (KeyValuePair<MyDefinitionId, MyControllerSchemeDefinition> controllerSchemeDefinition in m_definitions.m_controllerSchemeDefinitions)
			{
				if (controllerSchemeDefinition.Value.IsSelectable)
				{
					list.Add(controllerSchemeDefinition.Value);
				}
			}
			return list;
		}

		public MyControllerSchemeDefinition GetControllerScheme(string subtype = "General")
		{
			MyDefinitionId key = new MyDefinitionId(typeof(MyObjectBuilder_ControllerSchemeDefinition), subtype);
			MyControllerSchemeDefinition value = null;
			m_definitions.m_controllerSchemeDefinitions.TryGetValue(key, out value);
			return value;
=======
		public ListReader<MyChatBotResponseDefinition> GetChatBotResponseDefinitions()
		{
			return new ListReader<MyChatBotResponseDefinition>(Enumerable.ToList<MyChatBotResponseDefinition>((IEnumerable<MyChatBotResponseDefinition>)m_definitions.m_chatBotResponseDefinitions.Values));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public DictionaryReader<MyDefinitionId, MyFactionNameDefinition> GetFactionNameDefinitions()
		{
			return new DictionaryReader<MyDefinitionId, MyFactionNameDefinition>(m_definitions.m_factionNameDefinitions);
		}

		public DictionaryValuesReader<MyDefinitionId, MyResearchGroupDefinition> GetResearchGroupDefinitions()
		{
			return new DictionaryValuesReader<MyDefinitionId, MyResearchGroupDefinition>(m_definitions.m_researchGroupsDefinitions);
		}

		private void InitFactionNames(MyModContext context, ref DefinitionDictionary<MyFactionNameDefinition> output, MyObjectBuilder_FactionNameDefinition[] items, bool failOnDebug)
		{
			MyFactionNameDefinition[] array = new MyFactionNameDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyFactionNameDefinition>(context, items[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private void InitResearchBlocks(MyModContext context, ref DefinitionDictionary<MyResearchBlockDefinition> output, MyObjectBuilder_ResearchBlockDefinition[] items, bool failOnDebug)
		{
			MyResearchBlockDefinition[] array = new MyResearchBlockDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyResearchBlockDefinition>(context, items[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		public MyResearchBlockDefinition GetResearchBlock(MyDefinitionId id)
		{
			MyResearchBlockDefinition value = null;
			m_definitions.m_researchBlocksDefinitions.TryGetValue(id, out value);
			return value;
		}

		public DictionaryValuesReader<MyDefinitionId, MyResearchBlockDefinition> GetResearchBlockDefinitions()
		{
			return new DictionaryValuesReader<MyDefinitionId, MyResearchBlockDefinition>(m_definitions.m_researchBlocksDefinitions);
		}

		private void InitMainMenuInventoryScenes(MyModContext context, DefinitionDictionary<MyMainMenuInventorySceneDefinition> output, MyObjectBuilder_MainMenuInventorySceneDefinition[] items, bool failOnDebug)
		{
			MyMainMenuInventorySceneDefinition[] array = new MyMainMenuInventorySceneDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyMainMenuInventorySceneDefinition>(context, items[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private static void InitVoxelHands(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_VoxelHandDefinition[] items, bool failOnDebug = true)
		{
			MyVoxelHandDefinition[] array = new MyVoxelHandDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyVoxelHandDefinition>(context, items[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private void InitPrefabThrowers(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_DefinitionBase[] items, bool failOnDebug)
		{
			MyPrefabThrowerDefinition[] array = new MyPrefabThrowerDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyPrefabThrowerDefinition>(context, items[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private void InitAIBehaviors(MyModContext context, DefinitionDictionary<MyBehaviorDefinition> output, MyObjectBuilder_DefinitionBase[] items, bool failOnDebug)
		{
			MyBehaviorDefinition[] array = new MyBehaviorDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyBehaviorDefinition>(context, items[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private void InitBots(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_BotDefinition[] bots, bool failOnDebug = true)
		{
			MyBotDefinition[] array = new MyBotDefinition[bots.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyBotDefinition>(context, bots[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private void InitBotCommands(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_AiCommandDefinition[] commands, bool failOnDebug = true)
		{
			MyAiCommandDefinition[] array = new MyAiCommandDefinition[commands.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyAiCommandDefinition>(context, commands[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private void InitNavigationDefinitions(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_BlockNavigationDefinition[] definitions, bool failOnDebug = true)
		{
			MyBlockNavigationDefinition[] array = new MyBlockNavigationDefinition[definitions.Length];
			for (int i = 0; i < definitions.Length; i++)
			{
				array[i] = InitDefinition<MyBlockNavigationDefinition>(context, definitions[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		private static void InitDestruction(MyModContext context, ref MyDestructionDefinition output, MyObjectBuilder_DestructionDefinition objBuilder, bool failOnDebug = true)
		{
			MyDestructionDefinition myDestructionDefinition = (output = InitDefinition<MyDestructionDefinition>(context, objBuilder));
		}

		private void InitDecals(MyModContext context, MyObjectBuilder_DecalDefinition[] objBuilders, bool failOnDebug = true)
		{
<<<<<<< HEAD
			if (context != null && !context.IsBaseGame && context.ModPath != null)
			{
				foreach (MyObjectBuilder_DecalDefinition myObjectBuilder_DecalDefinition in objBuilders)
				{
					if (myObjectBuilder_DecalDefinition != null)
					{
						if (myObjectBuilder_DecalDefinition.Material.ColorMetalTexture != null)
						{
							string text = Path.Combine(context.ModPath, myObjectBuilder_DecalDefinition.Material.ColorMetalTexture);
							myObjectBuilder_DecalDefinition.Material.ColorMetalTexture = (File.Exists(text) ? text : myObjectBuilder_DecalDefinition.Material.ColorMetalTexture);
						}
						if (myObjectBuilder_DecalDefinition.Material.AlphamaskTexture != null)
						{
							string text2 = Path.Combine(context.ModPath, myObjectBuilder_DecalDefinition.Material.AlphamaskTexture);
							myObjectBuilder_DecalDefinition.Material.AlphamaskTexture = (File.Exists(text2) ? text2 : myObjectBuilder_DecalDefinition.Material.AlphamaskTexture);
						}
						if (myObjectBuilder_DecalDefinition.Material.ExtensionsTexture != null)
						{
							string text3 = Path.Combine(context.ModPath, myObjectBuilder_DecalDefinition.Material.ExtensionsTexture);
							myObjectBuilder_DecalDefinition.Material.ExtensionsTexture = (File.Exists(text3) ? text3 : myObjectBuilder_DecalDefinition.Material.ExtensionsTexture);
						}
						if (myObjectBuilder_DecalDefinition.Material.NormalmapTexture != null)
						{
							string text4 = Path.Combine(context.ModPath, myObjectBuilder_DecalDefinition.Material.NormalmapTexture);
							myObjectBuilder_DecalDefinition.Material.NormalmapTexture = (File.Exists(text4) ? text4 : myObjectBuilder_DecalDefinition.Material.NormalmapTexture);
						}
					}
				}
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_decalObjectBuilders.AddArray(objBuilders);
		}

		private static void InitEmissiveColors(MyModContext context, MyObjectBuilder_EmissiveColorDefinition[] objBuilders, bool failOnDebug = true)
		{
			for (int i = 0; i < objBuilders.Length; i++)
			{
				MyEmissiveColors.AddEmissiveColor(MyStringHash.GetOrCompute(objBuilders[i].Id.SubtypeId), new Color(objBuilders[i].ColorDefinition.R, objBuilders[i].ColorDefinition.G, objBuilders[i].ColorDefinition.B, objBuilders[i].ColorDefinition.A));
			}
		}

		private static void InitEmissiveColorPresets(MyModContext context, MyObjectBuilder_EmissiveColorStatePresetDefinition[] objBuilders, bool failOnDebug = true)
		{
			for (int i = 0; i < objBuilders.Length; i++)
			{
				MyStringHash orCompute = MyStringHash.GetOrCompute(objBuilders[i].Id.SubtypeId);
				MyEmissiveColorPresets.AddPreset(orCompute);
				for (int j = 0; j < objBuilders[i].EmissiveStates.Length; j++)
				{
					MyEmissiveColorState state = new MyEmissiveColorState(objBuilders[i].EmissiveStates[j].EmissiveColorName, objBuilders[i].EmissiveStates[j].DisplayColorName, objBuilders[i].EmissiveStates[j].Emissivity);
					MyEmissiveColorPresets.AddPresetState(orCompute, MyStringHash.GetOrCompute(objBuilders[i].EmissiveStates[j].StateName), state);
				}
			}
		}

		private static void InitDecalGlobals(MyModContext context, MyObjectBuilder_DecalGlobalsDefinition objBuilder, bool failOnDebug = true)
		{
			MyDecalGlobals decalGlobals = default(MyDecalGlobals);
			decalGlobals.DecalQueueSize = objBuilder.DecalQueueSize;
			decalGlobals.DecalTrailsQueueSize = objBuilder.DecalTrailsQueueSize;
			MyRenderProxy.SetDecalGlobals(decalGlobals);
		}

		private static void InitShadowTextureSets(MyModContext context, MyObjectBuilder_ShadowTextureSetDefinition[] objBuilders, bool failOnDebug = true)
		{
			MyGuiTextShadows.ClearShadowTextures();
			foreach (MyObjectBuilder_ShadowTextureSetDefinition myObjectBuilder_ShadowTextureSetDefinition in objBuilders)
			{
				List<ShadowTexture> list = new List<ShadowTexture>();
				MyObjectBuilder_ShadowTexture[] shadowTextures = myObjectBuilder_ShadowTextureSetDefinition.ShadowTextures;
				foreach (MyObjectBuilder_ShadowTexture myObjectBuilder_ShadowTexture in shadowTextures)
				{
					list.Add(new ShadowTexture(myObjectBuilder_ShadowTexture.Texture, myObjectBuilder_ShadowTexture.MinWidth, myObjectBuilder_ShadowTexture.GrowFactorWidth, myObjectBuilder_ShadowTexture.GrowFactorHeight, myObjectBuilder_ShadowTexture.DefaultAlpha));
				}
				MyGuiTextShadows.AddTextureSet(myObjectBuilder_ShadowTextureSetDefinition.Id.SubtypeName, list);
			}
		}

		private static void InitFlares(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_FlareDefinition[] objBuilders, bool failOnDebug = true)
		{
			MyFlareDefinition[] array = new MyFlareDefinition[objBuilders.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyFlareDefinition>(context, objBuilders[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		public void SetDefaultNavDef(MyCubeBlockDefinition blockDefinition)
		{
			MyObjectBuilder_BlockNavigationDefinition defaultObjectBuilder = MyBlockNavigationDefinition.GetDefaultObjectBuilder(blockDefinition);
			TryGetDefinition<MyBlockNavigationDefinition>(defaultObjectBuilder.Id, out var definition);
			if (definition != null)
			{
				blockDefinition.NavigationDefinition = definition;
				return;
			}
			MyBlockNavigationDefinition.CreateDefaultTriangles(defaultObjectBuilder);
			MyBlockNavigationDefinition myBlockNavigationDefinition = InitDefinition<MyBlockNavigationDefinition>(blockDefinition.Context, defaultObjectBuilder);
			Check(!m_definitions.m_definitionsById.ContainsKey(defaultObjectBuilder.Id), defaultObjectBuilder.Id);
			m_definitions.m_definitionsById[defaultObjectBuilder.Id] = myBlockNavigationDefinition;
			blockDefinition.NavigationDefinition = myBlockNavigationDefinition;
		}

		private void InitVoxelMapStorages(MyModContext context, Dictionary<string, MyVoxelMapStorageDefinition> output, MyObjectBuilder_VoxelMapStorageDefinition[] items, bool failOnDebug)
		{
			foreach (MyObjectBuilder_VoxelMapStorageDefinition builder in items)
			{
				MyVoxelMapStorageDefinition myVoxelMapStorageDefinition = InitDefinition<MyVoxelMapStorageDefinition>(context, builder);
				if (myVoxelMapStorageDefinition.StorageFile != null)
				{
					string subtypeName = myVoxelMapStorageDefinition.Id.SubtypeName;
					Check(!output.ContainsKey(subtypeName), subtypeName, failOnDebug);
					output[subtypeName] = myVoxelMapStorageDefinition;
				}
			}
		}

		private MyHandItemDefinition[] LoadHandItems(string path, MyModContext context)
		{
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = Load<MyObjectBuilder_Definitions>(path);
			MyHandItemDefinition[] array = new MyHandItemDefinition[myObjectBuilder_Definitions.HandItems.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyHandItemDefinition>(context, myObjectBuilder_Definitions.HandItems[i]);
			}
			return array;
		}

		public void ReloadHandItems()
		{
			MyModContext baseGame = MyModContext.BaseGame;
			MySandboxGame.Log.WriteLine("Loading hand items");
			string path = Path.Combine(baseGame.ModPathData, "HandItems.sbc");
			MyHandItemDefinition[] array = LoadHandItems(path, baseGame);
			if (m_definitions.m_handItemsById == null)
			{
				m_definitions.m_handItemsById = new DefinitionDictionary<MyHandItemDefinition>(array.Length);
			}
			else
			{
				m_definitions.m_handItemsById.Clear();
			}
			MyHandItemDefinition[] array2 = array;
			foreach (MyHandItemDefinition myHandItemDefinition in array2)
			{
				m_definitions.m_handItemsById[myHandItemDefinition.Id] = myHandItemDefinition;
			}
		}

		public void ReloadParticles()
		{
			MyModContext baseGame = MyModContext.BaseGame;
			MySandboxGame.Log.WriteLine("Loading particles");
			string path = Path.Combine(baseGame.ModPathData, "Particles.sbc");
			if (!m_transparentMaterialsInitialized)
			{
				CreateTransparentMaterials();
				m_transparentMaterialsInitialized = true;
			}
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = Load<MyObjectBuilder_Definitions>(path);
			MyParticleEffectsLibrary.Close();
			MyObjectBuilder_ParticleEffect[] particleEffects = myObjectBuilder_Definitions.ParticleEffects;
			foreach (MyObjectBuilder_ParticleEffect builder in particleEffects)
			{
				MyParticleEffectData data = new MyParticleEffectData();
				MyParticleEffectDataSerializer.DeserializeFromObjectBuilder(data, builder);
				MyParticleEffectsLibrary.Add(data);
			}
		}

		public void SaveHandItems()
		{
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
			List<MyObjectBuilder_HandItemDefinition> list = new List<MyObjectBuilder_HandItemDefinition>();
			foreach (MyHandItemDefinition value in m_definitions.m_handItemsById.Values)
			{
				MyObjectBuilder_HandItemDefinition item = (MyObjectBuilder_HandItemDefinition)value.GetObjectBuilder();
				list.Add(item);
			}
			myObjectBuilder_Definitions.HandItems = list.ToArray();
			string filepath = Path.Combine(MyFileSystem.ContentPath, "Data", "HandItems.sbc");
			myObjectBuilder_Definitions.Save(filepath);
		}

		private static void InitPhysicalItems(MyModContext context, DefinitionDictionary<MyDefinitionBase> outputDefinitions, List<MyPhysicalItemDefinition> outputWeapons, List<MyPhysicalItemDefinition> outputConsumables, MyObjectBuilder_PhysicalItemDefinition[] items, bool failOnDebug = true)
		{
			MyPhysicalItemDefinition[] array = new MyPhysicalItemDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyPhysicalItemDefinition>(context, items[i]);
				Check(!outputDefinitions.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				if (array[i].Id.TypeId == typeof(MyObjectBuilder_PhysicalGunObject))
				{
					outputWeapons.Add(array[i]);
				}
				if (array[i].Id.TypeId == typeof(MyObjectBuilder_ConsumableItem))
				{
					outputConsumables.Add(array[i]);
				}
				outputDefinitions[array[i].Id] = array[i];
			}
		}

		private static void InitScenarioDefinitions(MyModContext context, DefinitionDictionary<MyDefinitionBase> outputDefinitions, List<MyScenarioDefinition> outputScenarios, MyObjectBuilder_ScenarioDefinition[] scenarios, bool failOnDebug = true)
		{
			MyScenarioDefinition[] array = new MyScenarioDefinition[scenarios.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyScenarioDefinition>(context, scenarios[i]);
				outputScenarios.Add(array[i]);
				Check(!outputDefinitions.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				outputDefinitions[array[i].Id] = array[i];
			}
		}

		private static void InitSpawnGroups(MyModContext context, List<MySpawnGroupDefinition> outputSpawnGroups, DefinitionDictionary<MyDefinitionBase> outputDefinitions, MyObjectBuilder_SpawnGroupDefinition[] spawnGroups)
		{
			foreach (MyObjectBuilder_SpawnGroupDefinition builder in spawnGroups)
			{
				MySpawnGroupDefinition mySpawnGroupDefinition = InitDefinition<MySpawnGroupDefinition>(context, builder);
				mySpawnGroupDefinition.Init(builder, context);
				if (mySpawnGroupDefinition.IsValid)
				{
					outputSpawnGroups.Add(mySpawnGroupDefinition);
					outputDefinitions.AddDefinitionSafe(mySpawnGroupDefinition, context, context.CurrentFile);
				}
				else
				{
					MySandboxGame.Log.WriteLine("Error loading spawn group " + mySpawnGroupDefinition.DisplayNameString);
					MyDefinitionErrors.Add(context, "Error loading spawn group " + mySpawnGroupDefinition.DisplayNameString, TErrorSeverity.Warning);
				}
			}
		}

		private static void InitRespawnShips(MyModContext context, Dictionary<string, MyRespawnShipDefinition> outputDefinitions, MyObjectBuilder_RespawnShipDefinition[] respawnShips, bool failOnDebug)
		{
			foreach (MyObjectBuilder_RespawnShipDefinition builder in respawnShips)
			{
				MyRespawnShipDefinition myRespawnShipDefinition = InitDefinition<MyRespawnShipDefinition>(context, builder);
				string subtypeName = myRespawnShipDefinition.Id.SubtypeName;
				Check(!outputDefinitions.ContainsKey(subtypeName), subtypeName, failOnDebug);
				outputDefinitions[subtypeName] = myRespawnShipDefinition;
			}
		}

		private static void InitDropContainers(MyModContext context, Dictionary<string, MyDropContainerDefinition> outputDefinitions, MyObjectBuilder_DropContainerDefinition[] dropContainers, bool failOnDebug)
		{
			foreach (MyObjectBuilder_DropContainerDefinition builder in dropContainers)
			{
				MyDropContainerDefinition myDropContainerDefinition = InitDefinition<MyDropContainerDefinition>(context, builder);
				string subtypeName = myDropContainerDefinition.Id.SubtypeName;
				Check(!outputDefinitions.ContainsKey(subtypeName), subtypeName, failOnDebug);
				outputDefinitions[subtypeName] = myDropContainerDefinition;
			}
		}

		private static void InitBlockVariantGroups(MyModContext context, Dictionary<string, MyBlockVariantGroup> outputDefinitions, MyObjectBuilder_BlockVariantGroup[] groupDefinitions, bool failOnDebug)
		{
			foreach (MyObjectBuilder_BlockVariantGroup builder in groupDefinitions)
			{
				MyBlockVariantGroup myBlockVariantGroup = InitDefinition<MyBlockVariantGroup>(context, builder);
				string subtypeName = myBlockVariantGroup.Id.SubtypeName;
				Check(!outputDefinitions.ContainsKey(subtypeName), subtypeName, failOnDebug);
				outputDefinitions[subtypeName] = myBlockVariantGroup;
			}
		}

		private static void InitWheelModels(MyModContext context, Dictionary<string, MyWheelModelsDefinition> outputDefinitions, MyObjectBuilder_WheelModelsDefinition[] wheelDefinitions, bool failOnDebug)
		{
			foreach (MyObjectBuilder_WheelModelsDefinition builder in wheelDefinitions)
			{
				MyWheelModelsDefinition myWheelModelsDefinition = InitDefinition<MyWheelModelsDefinition>(context, builder);
				string subtypeName = myWheelModelsDefinition.Id.SubtypeName;
				Check(!outputDefinitions.ContainsKey(subtypeName), subtypeName, failOnDebug);
				outputDefinitions[subtypeName] = myWheelModelsDefinition;
			}
		}

		private static void InitAsteroidGenerators(MyModContext context, Dictionary<string, MyAsteroidGeneratorDefinition> outputDefinitions, MyObjectBuilder_AsteroidGeneratorDefinition[] asteroidGenerators, bool failOnDebug)
		{
			foreach (MyObjectBuilder_AsteroidGeneratorDefinition myObjectBuilder_AsteroidGeneratorDefinition in asteroidGenerators)
			{
				if (!int.TryParse(myObjectBuilder_AsteroidGeneratorDefinition.Id.SubtypeId, out var _))
				{
					Check(conditionResult: false, myObjectBuilder_AsteroidGeneratorDefinition.Id.SubtypeId, failOnDebug, "Asteroid generator SubtypeId has to be number.");
					continue;
				}
				MyAsteroidGeneratorDefinition myAsteroidGeneratorDefinition = InitDefinition<MyAsteroidGeneratorDefinition>(context, myObjectBuilder_AsteroidGeneratorDefinition);
				string subtypeName = myAsteroidGeneratorDefinition.Id.SubtypeName;
				Check(!outputDefinitions.ContainsKey(subtypeName), subtypeName, failOnDebug);
				outputDefinitions[subtypeName] = myAsteroidGeneratorDefinition;
			}
		}

		private static void InitPrefabs(MyModContext context, Dictionary<string, MyPrefabDefinition> outputDefinitions, MyObjectBuilder_PrefabDefinition[] prefabs, bool failOnDebug)
		{
			foreach (MyObjectBuilder_PrefabDefinition myObjectBuilder_PrefabDefinition in prefabs)
			{
				MyPrefabDefinition myPrefabDefinition = InitDefinition<MyPrefabDefinition>(context, myObjectBuilder_PrefabDefinition);
				string subtypeName = myPrefabDefinition.Id.SubtypeName;
				Check(!outputDefinitions.ContainsKey(subtypeName), subtypeName, failOnDebug);
				outputDefinitions[subtypeName] = myPrefabDefinition;
				if (myObjectBuilder_PrefabDefinition.RespawnShip)
				{
					MyDefinitionErrors.Add(context, "Tag <RespawnShip /> is obsolete in prefabs. Use file \"RespawnShips.sbc\" instead.", TErrorSeverity.Warning);
				}
			}
		}

		private void InitControllerSchemas(MyModContext context, DefinitionDictionary<MyDefinitionBase> outputDefinitions, MyObjectBuilder_ControllerSchemaDefinition[] schemas, bool failOnDebug)
		{
			foreach (MyObjectBuilder_ControllerSchemaDefinition builder in schemas)
			{
				MyControllerSchemaDefinition myControllerSchemaDefinition = InitDefinition<MyControllerSchemaDefinition>(context, builder);
				MyDefinitionId id = myControllerSchemaDefinition.Id;
				Check(!outputDefinitions.ContainsKey(id), id, failOnDebug);
				outputDefinitions.AddDefinitionSafe(myControllerSchemaDefinition, context, "<ControllerSchema>");
			}
		}

		private void InitCurves(MyModContext context, DefinitionDictionary<MyDefinitionBase> outputDefinitions, MyObjectBuilder_CurveDefinition[] curves, bool failOnDebug)
		{
			foreach (MyObjectBuilder_CurveDefinition builder in curves)
			{
				MyCurveDefinition myCurveDefinition = InitDefinition<MyCurveDefinition>(context, builder);
				MyDefinitionId id = myCurveDefinition.Id;
				Check(!outputDefinitions.ContainsKey(id), id, failOnDebug);
				outputDefinitions.AddDefinitionSafe(myCurveDefinition, context, "<Curve>");
			}
		}

		private void InitCharacterNames(MyModContext context, List<MyCharacterName> output, MyCharacterName[] names, bool failOnDebug)
		{
			foreach (MyCharacterName item in names)
			{
				output.Add(item);
			}
		}

		private void InitAudioEffects(MyModContext context, DefinitionDictionary<MyDefinitionBase> outputDefinitions, MyObjectBuilder_AudioEffectDefinition[] audioEffects, bool failOnDebug)
		{
			foreach (MyObjectBuilder_AudioEffectDefinition builder in audioEffects)
			{
				MyAudioEffectDefinition myAudioEffectDefinition = InitDefinition<MyAudioEffectDefinition>(context, builder);
				MyDefinitionId id = myAudioEffectDefinition.Id;
				Check(!outputDefinitions.ContainsKey(id), id, failOnDebug);
				outputDefinitions.AddDefinitionSafe(myAudioEffectDefinition, context, "<AudioEffect>");
			}
		}

		private static void InitTransparentMaterials(MyModContext context, DefinitionDictionary<MyDefinitionBase> outputDefinitions, MyObjectBuilder_TransparentMaterialDefinition[] materials)
		{
			foreach (MyObjectBuilder_TransparentMaterialDefinition builder in materials)
			{
				MyTransparentMaterialDefinition myTransparentMaterialDefinition = InitDefinition<MyTransparentMaterialDefinition>(context, builder);
				myTransparentMaterialDefinition.Init(builder, context);
				outputDefinitions.AddDefinitionSafe(myTransparentMaterialDefinition, context, "<TransparentMaterials>");
			}
		}

		private void InitPhysicalMaterials(MyModContext context, DefinitionDictionary<MyDefinitionBase> outputDefinitions, MyObjectBuilder_PhysicalMaterialDefinition[] materials)
		{
			foreach (MyObjectBuilder_PhysicalMaterialDefinition myObjectBuilder_PhysicalMaterialDefinition in materials)
			{
				if (!TryGetDefinition<MyPhysicalMaterialDefinition>(myObjectBuilder_PhysicalMaterialDefinition.Id, out var definition))
				{
					definition = InitDefinition<MyPhysicalMaterialDefinition>(context, myObjectBuilder_PhysicalMaterialDefinition);
					outputDefinitions.AddDefinitionSafe(definition, context, "<PhysicalMaterials>");
				}
				else
				{
					definition.Init(myObjectBuilder_PhysicalMaterialDefinition, context);
				}
				m_definitions.m_physicalMaterialsByName[definition.Id.SubtypeName] = definition;
			}
		}

		private void InitMaterialProperties(MyModContext context, DefinitionDictionary<MyDefinitionBase> outputDefinitions, MyObjectBuilder_MaterialPropertiesDefinition[] properties)
		{
			foreach (MyObjectBuilder_MaterialPropertiesDefinition myObjectBuilder_MaterialPropertiesDefinition in properties)
			{
				if (TryGetDefinition<MyPhysicalMaterialDefinition>(myObjectBuilder_MaterialPropertiesDefinition.Id, out var definition))
				{
					definition.Init(myObjectBuilder_MaterialPropertiesDefinition, context);
				}
			}
		}

		private static void CreateTransparentMaterials()
		{
			List<string> list = new List<string>();
			HashSet<string> val = new HashSet<string>();
			foreach (MyTransparentMaterialDefinition transparentMaterialDefinition in Static.GetTransparentMaterialDefinitions())
			{
				MyTransparentMaterials.AddMaterial(new MyTransparentMaterial(MyStringId.GetOrCompute(transparentMaterialDefinition.Id.SubtypeName), transparentMaterialDefinition.TextureType, transparentMaterialDefinition.Texture, transparentMaterialDefinition.GlossTexture, transparentMaterialDefinition.SoftParticleDistanceScale, transparentMaterialDefinition.CanBeAffectedByLights, transparentMaterialDefinition.AlphaMistingEnable, transparentMaterialDefinition.Color, transparentMaterialDefinition.ColorAdd, transparentMaterialDefinition.ShadowMultiplier, transparentMaterialDefinition.LightMultiplier, transparentMaterialDefinition.IsFlareOccluder, transparentMaterialDefinition.TriangleFaceCulling, transparentMaterialDefinition.UseAtlas, transparentMaterialDefinition.AlphaMistingStart, transparentMaterialDefinition.AlphaMistingEnd, transparentMaterialDefinition.AlphaSaturation, transparentMaterialDefinition.Reflectivity, transparentMaterialDefinition.AlphaCutout, transparentMaterialDefinition.TargetSize, transparentMaterialDefinition.Fresnel, transparentMaterialDefinition.ReflectionShadow, transparentMaterialDefinition.Gloss, transparentMaterialDefinition.GlossTextureAdd, transparentMaterialDefinition.SpecularColorFactor));
				if (transparentMaterialDefinition.TextureType == MyTransparentMaterialTextureType.FileTexture && !string.IsNullOrEmpty(transparentMaterialDefinition.Texture) && Path.GetFileNameWithoutExtension(transparentMaterialDefinition.Texture).StartsWith("Atlas_"))
				{
					list.Add(transparentMaterialDefinition.Texture);
					val.Add(transparentMaterialDefinition.Texture);
				}
			}
<<<<<<< HEAD
			MyRenderProxy.AddToParticleTextureArray(hashSet);
=======
			MyRenderProxy.AddToParticleTextureArray(val);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyTransparentMaterials.Update();
		}

		private static void InitVoxelMaterials(MyModContext context, Dictionary<string, MyVoxelMaterialDefinition> output, MyObjectBuilder_VoxelMaterialDefinition[] materials, bool failOnDebug = true)
		{
			MyVoxelMaterialDefinition[] array = new MyVoxelMaterialDefinition[materials.Length];
			for (int i = 0; i < materials.Length; i++)
			{
				array[i] = InitDefinition<MyVoxelMaterialDefinition>(context, materials[i]);
				Check(!output.ContainsKey(array[i].Id.SubtypeName), array[i].Id.SubtypeName, failOnDebug);
				output[array[i].Id.SubtypeName] = array[i];
				if (!context.IsBaseGame)
				{
					MySandboxGame.Log.WriteLine("Loaded voxel material: " + array[i].Id.SubtypeName);
				}
			}
		}

		private static void InitCharacters(MyModContext context, Dictionary<string, MyCharacterDefinition> outputCharacters, DefinitionDictionary<MyDefinitionBase> outputDefinitions, MyObjectBuilder_CharacterDefinition[] characters, bool failOnDebug = true)
		{
			MyCharacterDefinition[] array = new MyCharacterDefinition[characters.Length];
			for (int i = 0; i < characters.Length; i++)
			{
				if (typeof(MyObjectBuilder_CharacterDefinition).IsAssignableFrom(characters[i].Id.TypeId))
				{
					characters[i].Id.TypeId = typeof(MyObjectBuilder_Character);
				}
				array[i] = InitDefinition<MyCharacterDefinition>(context, characters[i]);
				if (array[i].Id.TypeId.IsNull)
				{
					MySandboxGame.Log.WriteLine("Invalid character Id found in mod !");
					MyDefinitionErrors.Add(context, "Invalid character Id found in mod ! ", TErrorSeverity.Error);
					continue;
				}
				Check(!outputCharacters.ContainsKey(array[i].Name), array[i].Name, failOnDebug);
				outputCharacters[array[i].Name] = array[i];
				Check(!outputDefinitions.ContainsKey(characters[i].Id), array[i].Name, failOnDebug);
				outputDefinitions[characters[i].Id] = array[i];
			}
		}

		private static void InitDefinitionsEnvItems(MyModContext context, DefinitionDictionary<MyDefinitionBase> outputDefinitions, MyObjectBuilder_EnvironmentItemDefinition[] items, bool failOnDebug = true)
		{
			MyEnvironmentItemDefinition[] array = new MyEnvironmentItemDefinition[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyEnvironmentItemDefinition>(context, items[i]);
				array[i].PhysicalMaterial = MyDestructionData.GetPhysicalMaterial(array[i], items[i].PhysicalMaterial);
				Check(!outputDefinitions.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				outputDefinitions[array[i].Id] = array[i];
			}
		}

		private static void InitDefinitionsGeneric<OBDefType, DefType>(MyModContext context, DefinitionDictionary<MyDefinitionBase> outputDefinitions, OBDefType[] items, bool failOnDebug = true) where OBDefType : MyObjectBuilder_DefinitionBase where DefType : MyDefinitionBase
		{
			DefType[] array = new DefType[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<DefType>(context, items[i]);
				Check(!outputDefinitions.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				outputDefinitions[array[i].Id] = array[i];
			}
		}

		private static void InitDefinitionsGeneric<OBDefType, DefType>(MyModContext context, DefinitionDictionary<DefType> outputDefinitions, OBDefType[] items, bool failOnDebug = true) where OBDefType : MyObjectBuilder_DefinitionBase where DefType : MyDefinitionBase
		{
			DefType[] array = new DefType[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<DefType>(context, items[i]);
				Check(!outputDefinitions.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				outputDefinitions[array[i].Id] = array[i];
			}
		}

		private void InitPlanetGeneratorDefinitions(MyModContext context, DefinitionSet defset, MyObjectBuilder_PlanetGeneratorDefinition[] planets, bool failOnDebug)
		{
			foreach (MyObjectBuilder_PlanetGeneratorDefinition builder in planets)
			{
				MyPlanetGeneratorDefinition myPlanetGeneratorDefinition = InitDefinition<MyPlanetGeneratorDefinition>(context, builder);
				if (!context.IsBaseGame)
				{
					foreach (MyCloudLayerSettings cloudLayer in myPlanetGeneratorDefinition.CloudLayers)
					{
						for (int j = 0; j < cloudLayer.Textures.Count; j++)
						{
							cloudLayer.Textures[j] = context.ModPath + "\\" + cloudLayer.Textures[j];
						}
					}
				}
				if (myPlanetGeneratorDefinition.Enabled)
				{
					defset.AddOrRelaceDefinition(myPlanetGeneratorDefinition);
				}
				else
				{
					defset.RemoveDefinition(ref myPlanetGeneratorDefinition.Id);
				}
			}
		}

		private static void InitComponentGroups(MyModContext context, DefinitionDictionary<MyComponentGroupDefinition> output, MyObjectBuilder_ComponentGroupDefinition[] objects, bool failOnDebug = true)
		{
			for (int i = 0; i < objects.Length; i++)
			{
				MyComponentGroupDefinition myComponentGroupDefinition = InitDefinition<MyComponentGroupDefinition>(context, objects[i]);
				Check(!output.ContainsKey(myComponentGroupDefinition.Id), myComponentGroupDefinition.Id, failOnDebug);
				output[myComponentGroupDefinition.Id] = myComponentGroupDefinition;
			}
		}

		private void InitComponentBlocks(MyModContext context, HashSet<MyComponentBlockEntry> output, MyComponentBlockEntry[] objects, bool failOnDebug = true)
		{
			foreach (MyComponentBlockEntry myComponentBlockEntry in objects)
			{
				Check(!output.Contains(myComponentBlockEntry), myComponentBlockEntry, failOnDebug);
				output.Add(myComponentBlockEntry);
			}
		}

		private void InitPlanetPrefabDefinitions(MyModContext context, ref DefinitionDictionary<MyPlanetPrefabDefinition> m_planetDefinitions, MyObjectBuilder_PlanetPrefabDefinition[] planets, bool failOnDebug)
		{
			foreach (MyObjectBuilder_PlanetPrefabDefinition builder in planets)
			{
				MyPlanetPrefabDefinition myPlanetPrefabDefinition = InitDefinition<MyPlanetPrefabDefinition>(context, builder);
				MyDefinitionId id = myPlanetPrefabDefinition.Id;
				if (myPlanetPrefabDefinition.Enabled)
				{
					m_planetDefinitions[id] = myPlanetPrefabDefinition;
				}
				else
				{
					m_planetDefinitions.Remove(id);
				}
			}
		}

		private void InitGroupedIds(MyModContext context, string setName, Dictionary<string, Dictionary<string, MyGroupedIds>> output, MyGroupedIds[] groups, bool failOnDebug)
		{
			if (!output.TryGetValue(setName, out var value))
			{
				value = new Dictionary<string, MyGroupedIds>();
				output.Add(setName, value);
			}
			foreach (MyGroupedIds myGroupedIds in groups)
			{
				value[myGroupedIds.Tag] = myGroupedIds;
			}
		}

		private void InitRadialMenus(MyModContext context, Dictionary<MyDefinitionId, MyRadialMenu> output, MyObjectBuilder_RadialMenu[] items, bool failOnDebug)
		{
			MyRadialMenu[] array = new MyRadialMenu[items.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = InitDefinition<MyRadialMenu>(context, items[i]);
				Check(!output.ContainsKey(array[i].Id), array[i].Id, failOnDebug);
				output[array[i].Id] = array[i];
			}
		}

		public MyRadialMenu GetRadialMenuDefinition(string subtype)
		{
			MyDefinitionId key = new MyDefinitionId(typeof(MyObjectBuilder_RadialMenu), subtype);
			MyRadialMenu value = null;
			m_definitions.m_radialMenuDefinitions.TryGetValue(key, out value);
			return value;
		}

		private void InitOffensiveWords(MyModContext context, ref MyOffensiveWordsDefinition output, MyObjectBuilder_OffensiveWords[] items, bool failOnDebug)
		{
			for (int i = 0; i < items.Length; i++)
			{
				MyOffensiveWordsDefinition myOffensiveWordsDefinition = (output = InitDefinition<MyOffensiveWordsDefinition>(context, items[i]));
			}
		}

		public MyOffensiveWordsDefinition GetOffensiveWordsForPlatform()
		{
			return m_definitions.m_offensiveWordsDefinition;
		}

		public bool IsComponentBlock(MyDefinitionId blockDefinitionId)
		{
			return m_definitions.m_componentBlocks.Contains(blockDefinitionId);
		}

		public MyDefinitionId GetComponentId(MyCubeBlockDefinition blockDefinition)
		{
			MyCubeBlockDefinition.Component[] components = blockDefinition.Components;
			if (components == null || components.Length == 0)
			{
				return default(MyDefinitionId);
			}
			return components[0].Definition.Id;
		}

		public MyDefinitionId GetComponentId(MyDefinitionId defId)
		{
			MyCubeBlockDefinition blockDefinition = null;
			if (!TryGetCubeBlockDefinition(defId, out blockDefinition))
			{
				return default(MyDefinitionId);
			}
			return GetComponentId(blockDefinition);
		}

		public MyCubeBlockDefinition TryGetComponentBlockDefinition(MyDefinitionId componentDefId)
		{
			MyCubeBlockDefinition value = null;
			m_definitions.m_componentIdToBlock.TryGetValue(componentDefId, out value);
			return value;
		}

		public MyCubeBlockDefinition GetComponentBlockDefinition(MyDefinitionId componentDefId)
		{
			MyCubeBlockDefinition value = null;
			m_definitions.m_componentIdToBlock.TryGetValue(componentDefId, out value);
			return value;
		}

		public string GetRandomCharacterName()
		{
			if (m_definitions.m_characterNames.Count == 0)
			{
				return "";
			}
			int randomInt = MyUtils.GetRandomInt(m_definitions.m_characterNames.Count);
			return m_definitions.m_characterNames[randomInt].Name;
		}

		public MyAudioDefinition GetSoundDefinition(MyStringHash subtypeId)
		{
			if (m_definitions.m_sounds.TryGetValue(new MyDefinitionId(typeof(MyObjectBuilder_AudioDefinition), subtypeId), out var value))
			{
				return value;
			}
			return null;
		}

		public DictionaryValuesReader<MyDefinitionId, MyHandItemDefinition> GetHandItemDefinitions()
		{
			return new DictionaryValuesReader<MyDefinitionId, MyHandItemDefinition>(m_definitions.m_handItemsById);
		}

		public MyHandItemDefinition TryGetHandItemDefinition(ref MyDefinitionId id)
		{
			m_definitions.m_handItemsById.TryGetValue(id, out var value);
			return value;
		}

		public ListReader<MyVoxelHandDefinition> GetVoxelHandDefinitions()
		{
			return new ListReader<MyVoxelHandDefinition>(Enumerable.ToList<MyVoxelHandDefinition>(Enumerable.OfType<MyVoxelHandDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MyPrefabThrowerDefinition> GetPrefabThrowerDefinitions()
		{
			return new ListReader<MyPrefabThrowerDefinition>(Enumerable.ToList<MyPrefabThrowerDefinition>(Enumerable.OfType<MyPrefabThrowerDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public DictionaryReader<MyDefinitionId, MyContractTypeDefinition> GetContractTypeDefinitions()
		{
			return new DictionaryReader<MyDefinitionId, MyContractTypeDefinition>(m_definitions.m_contractTypesDefinitions);
		}

		private static MyBlueprintDefinitionBase MakeBlueprintFromComponentStack(MyModContext context, MyCubeBlockDefinition cubeBlockDefinition)
		{
			MyCubeBlockFactory.GetProducedType(cubeBlockDefinition.Id.TypeId);
			MyObjectBuilder_CompositeBlueprintDefinition myObjectBuilder_CompositeBlueprintDefinition = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_CompositeBlueprintDefinition>();
			myObjectBuilder_CompositeBlueprintDefinition.Id = new SerializableDefinitionId(typeof(MyObjectBuilder_BlueprintDefinition), cubeBlockDefinition.Id.ToString().Replace("MyObjectBuilder_", ""));
			Dictionary<MyDefinitionId, MyFixedPoint> dictionary = new Dictionary<MyDefinitionId, MyFixedPoint>();
			MyCubeBlockDefinition.Component[] components = cubeBlockDefinition.Components;
			foreach (MyCubeBlockDefinition.Component component in components)
			{
				MyDefinitionId id = component.Definition.Id;
				if (!dictionary.ContainsKey(id))
				{
					dictionary[id] = 0;
				}
				dictionary[id] += (MyFixedPoint)component.Count;
			}
			myObjectBuilder_CompositeBlueprintDefinition.Blueprints = new BlueprintItem[dictionary.Count];
			int num = 0;
			foreach (KeyValuePair<MyDefinitionId, MyFixedPoint> item in dictionary)
			{
				MyBlueprintDefinitionBase myBlueprintDefinitionBase = null;
				if ((myBlueprintDefinitionBase = Static.TryGetBlueprintDefinitionByResultId(item.Key)) == null)
				{
					MyDefinitionErrors.Add(context, "Could not find component blueprint for " + item.Key.ToString(), TErrorSeverity.Error);
					return null;
				}
				myObjectBuilder_CompositeBlueprintDefinition.Blueprints[num] = new BlueprintItem
				{
					Id = new SerializableDefinitionId(myBlueprintDefinitionBase.Id.TypeId, myBlueprintDefinitionBase.Id.SubtypeName),
					Amount = item.Value.ToString()
				};
				num++;
			}
			myObjectBuilder_CompositeBlueprintDefinition.Icons = cubeBlockDefinition.Icons;
			myObjectBuilder_CompositeBlueprintDefinition.DisplayName = (cubeBlockDefinition.DisplayNameEnum.HasValue ? cubeBlockDefinition.DisplayNameEnum.Value.ToString() : cubeBlockDefinition.DisplayNameText);
			myObjectBuilder_CompositeBlueprintDefinition.Public = cubeBlockDefinition.Public;
			return InitDefinition<MyBlueprintDefinitionBase>(context, myObjectBuilder_CompositeBlueprintDefinition);
		}

		public MyObjectBuilder_DefinitionBase GetObjectBuilder(MyDefinitionBase definition)
		{
			return MyDefinitionManagerBase.GetObjectFactory().CreateObjectBuilder<MyObjectBuilder_DefinitionBase>(definition);
		}

		private static void Check<T>(bool conditionResult, T identifier, bool failOnDebug = true, string messageFormat = "Duplicate entry of '{0}'")
		{
			if (!conditionResult)
			{
				string msg = string.Format(messageFormat, identifier.ToString());
				MySandboxGame.Log.WriteLine(msg);
			}
		}

		private void MergeDefinitions()
		{
			m_definitions.Clear();
			foreach (KeyValuePair<string, DefinitionSet> modDefinitionSet in m_modDefinitionSets)
			{
				m_definitions.OverrideBy(modDefinitionSet.Value, modDefinitionSet.Value.Context == null || modDefinitionSet.Value.Context.IsBaseGame);
			}
		}

		private static void InitGenericObjects(MyModContext context, DefinitionDictionary<MyDefinitionBase> output, MyObjectBuilder_DefinitionBase[] objects, bool failOnDebug = true)
		{
			for (int i = 0; i < objects.Length; i++)
			{
				MyDefinitionBase myDefinitionBase = InitDefinition<MyDefinitionBase>(context, objects[i]);
				Check(!output.ContainsKey(myDefinitionBase.Id), myDefinitionBase.Id, failOnDebug);
				output[myDefinitionBase.Id] = myDefinitionBase;
			}
		}

		public MyGridCreateToolDefinition GetGridCreator(MyStringHash name)
		{
			m_definitions.m_gridCreateDefinitions.TryGetValue(new MyDefinitionId(typeof(MyObjectBuilder_GridCreateToolDefinition), name), out var value);
			return value;
		}

		public IEnumerable<MyGridCreateToolDefinition> GetGridCreatorDefinitions()
		{
			return m_definitions.m_gridCreateDefinitions.Values;
		}

		public void GetBaseBlockPrefabName(MyCubeSize size, bool isStatic, bool isCreative, out string prefabName)
		{
			prefabName = m_definitions.m_basePrefabNames[ComputeBasePrefabIndex(size, isStatic, isCreative)];
		}

		private void AddBasePrefabName(DefinitionSet definitionSet, MyCubeSize size, bool isStatic, bool isCreative, string prefabName)
		{
			if (!string.IsNullOrEmpty(prefabName))
			{
				definitionSet.m_basePrefabNames[ComputeBasePrefabIndex(size, isStatic, isCreative)] = prefabName;
			}
		}

		private static int ComputeBasePrefabIndex(MyCubeSize size, bool isStatic, bool isCreative)
		{
			return (int)size * 4 + (isStatic ? 2 : 0) + (isCreative ? 1 : 0);
		}

		public MyCubeBlockDefinitionGroup GetDefinitionGroup(string groupName)
		{
			return m_definitions.m_blockGroups[groupName];
		}

		public MyCubeBlockDefinitionGroup TryGetDefinitionGroup(string groupName)
		{
			if (!m_definitions.m_blockGroups.ContainsKey(groupName))
			{
				return null;
			}
			return m_definitions.m_blockGroups[groupName];
		}

		public DictionaryKeysReader<string, MyCubeBlockDefinitionGroup> GetDefinitionPairNames()
		{
			return new DictionaryKeysReader<string, MyCubeBlockDefinitionGroup>(m_definitions.m_blockGroups);
		}

		public Dictionary<string, MyCubeBlockDefinitionGroup> GetDefinitionPairs()
		{
			return m_definitions.m_blockGroups;
		}

		public bool TryGetDefinition<T>(MyDefinitionId defId, out T definition) where T : MyDefinitionBase
		{
			if (!defId.TypeId.IsNull)
			{
				definition = GetDefinition<T>(defId);
				if (definition != null)
				{
					return true;
				}
				if (m_definitions.m_definitionsById.TryGetValue(defId, out var value))
				{
					definition = value as T;
					return definition != null;
				}
			}
			definition = null;
			return false;
		}

		public MyDefinitionBase GetDefinition(MyDefinitionId id)
		{
			MyDefinitionBase definition = GetDefinition<MyDefinitionBase>(id);
			if (definition != null)
			{
				return definition;
			}
			CheckDefinition(ref id);
			if (m_definitions.m_definitionsById.TryGetValue(id, out var value))
			{
				return value;
			}
			return new MyDefinitionBase();
		}

		public Vector2I GetCubeBlockScreenPosition(MyCubeBlockDefinitionGroup blockDefinitionGroup)
		{
			string blockPairName = blockDefinitionGroup.Any.BlockPairName;
			if (m_definitions.m_blockPositions.TryGetValue(blockPairName, out var value))
			{
				return value;
			}
			if (blockDefinitionGroup.Any.BlockVariantsGroup != null && m_definitions.m_blockPositions.TryGetValue(blockDefinitionGroup.Any.BlockVariantsGroup.PrimaryGUIBlock.BlockPairName, out value))
			{
				return value;
			}
			return new Vector2I(-1, -1);
		}

		public bool TryGetCubeBlockDefinition(MyDefinitionId defId, out MyCubeBlockDefinition blockDefinition)
		{
			if (!m_definitions.m_definitionsById.TryGetValue(defId, out var value))
			{
				blockDefinition = null;
				return false;
			}
			blockDefinition = value as MyCubeBlockDefinition;
			return blockDefinition != null;
		}

		public MyCubeBlockDefinition GetCubeBlockDefinition(MyObjectBuilder_CubeBlock builder)
		{
			return GetCubeBlockDefinition(builder.GetId());
		}

		public MyCubeBlockDefinition GetCubeBlockDefinition(MyDefinitionId id)
		{
			CheckDefinition<MyCubeBlockDefinition>(ref id);
			if (m_definitions.m_definitionsById.ContainsKey(id))
			{
				return m_definitions.m_definitionsById[id] as MyCubeBlockDefinition;
			}
			return null;
		}

		public MyComponentDefinition GetComponentDefinition(MyDefinitionId id)
		{
			CheckDefinition<MyComponentDefinition>(ref id);
			if (m_definitions.m_definitionsById.ContainsKey(id))
			{
				return m_definitions.m_definitionsById[id] as MyComponentDefinition;
			}
			return null;
		}

		public void GetDefinedEntityComponents(ref List<MyDefinitionId> definedComponents)
		{
			foreach (KeyValuePair<MyDefinitionId, MyComponentDefinitionBase> entityComponentDefinition in m_definitions.m_entityComponentDefinitions)
			{
				definedComponents.Add(entityComponentDefinition.Key);
			}
		}

		public bool TryGetComponentDefinition(MyDefinitionId id, out MyComponentDefinition definition)
		{
			MyDefinitionBase value = (definition = null);
			if (m_definitions.m_definitionsById.TryGetValue(id, out value))
			{
				definition = value as MyComponentDefinition;
				return definition != null;
			}
			return false;
		}

		public MyBlueprintDefinitionBase TryGetBlueprintDefinitionByResultId(MyDefinitionId resultId)
		{
			return m_definitions.m_blueprintsByResultId.GetValueOrDefault(resultId);
		}

		public bool TryGetBlueprintDefinitionByResultId(MyDefinitionId resultId, out MyBlueprintDefinitionBase definition)
		{
			return m_definitions.m_blueprintsByResultId.TryGetValue(resultId, out definition);
		}

		public bool HasBlueprint(MyDefinitionId blueprintId)
		{
			return m_definitions.m_blueprintsById.ContainsKey(blueprintId);
		}

		public MyBlueprintDefinitionBase GetBlueprintDefinition(MyDefinitionId blueprintId)
		{
			if (!m_definitions.m_blueprintsById.ContainsKey(blueprintId))
			{
				MySandboxGame.Log.WriteLine($"No blueprint with Id '{blueprintId}'");
				return null;
			}
			return m_definitions.m_blueprintsById[blueprintId];
		}

		public MyBlueprintClassDefinition GetBlueprintClass(string className)
		{
			MyBlueprintClassDefinition value = null;
			MyDefinitionId key = new MyDefinitionId(typeof(MyObjectBuilder_BlueprintClassDefinition), className);
			m_definitions.m_blueprintClasses.TryGetValue(key, out value);
			return value;
		}

		public bool TryGetIngotBlueprintDefinition(MyObjectBuilder_Base oreBuilder, out MyBlueprintDefinitionBase ingotBlueprint)
		{
			return TryGetIngotBlueprintDefinition(oreBuilder.GetId(), out ingotBlueprint);
		}

		public Dictionary<string, MyGuiBlockCategoryDefinition> GetCategories()
		{
			return m_definitions.m_categories;
		}

		public bool TryGetIngotBlueprintDefinition(MyDefinitionId oreId, out MyBlueprintDefinitionBase ingotBlueprint)
		{
			foreach (MyBlueprintDefinitionBase item in GetBlueprintClass("Ingots"))
			{
				if (!(item.InputItemType != typeof(MyObjectBuilder_Ore)) && item.Prerequisites[0].Id.SubtypeId == oreId.SubtypeId)
				{
					ingotBlueprint = item;
					return true;
				}
			}
			ingotBlueprint = null;
			return false;
		}

		public bool TryGetComponentBlueprintDefinition(MyDefinitionId componentId, out MyBlueprintDefinitionBase componentBlueprint)
		{
			foreach (MyBlueprintDefinitionBase item in GetBlueprintClass("Components"))
			{
				if (!(item.InputItemType != typeof(MyObjectBuilder_Ingot)) && item.Results[0].Id.SubtypeId == componentId.SubtypeId)
				{
					componentBlueprint = item;
					return true;
				}
			}
			componentBlueprint = null;
			return false;
		}

		public DictionaryValuesReader<MyDefinitionId, MyBlueprintDefinitionBase> GetBlueprintDefinitions()
		{
			return new DictionaryValuesReader<MyDefinitionId, MyBlueprintDefinitionBase>(m_definitions.m_blueprintsById);
		}

		public MyAssetModifierDefinition GetAssetModifierDefinition(MyDefinitionId id)
		{
			MyAssetModifierDefinition value = null;
			m_definitions.m_assetModifiers.TryGetValue(id, out value);
			return value;
		}

		public MyAssetModifiers GetAssetModifierDefinitionForRender(string skinId)
		{
			return GetAssetModifierDefinitionForRender(MyStringHash.GetOrCompute(skinId));
		}

		public MyAssetModifiers GetAssetModifierDefinitionForRender(MyStringHash skinId)
		{
			m_definitions.m_assetModifiersForRender.TryGetValue(skinId, out var value);
			return value;
		}

		public DictionaryValuesReader<MyDefinitionId, MyMainMenuInventorySceneDefinition> GetMainMenuInventoryScenes()
		{
			return new DictionaryValuesReader<MyDefinitionId, MyMainMenuInventorySceneDefinition>(m_definitions.m_mainMenuInventoryScenes);
		}

		public DictionaryValuesReader<MyDefinitionId, MyAssetModifierDefinition> GetAssetModifierDefinitions()
		{
			return new DictionaryValuesReader<MyDefinitionId, MyAssetModifierDefinition>(m_definitions.m_assetModifiers);
		}

		public DictionaryReader<MyStringHash, MyAssetModifiers> GetAssetModifierDefinitionsForRender()
		{
			return new DictionaryReader<MyStringHash, MyAssetModifiers>(m_definitions.m_assetModifiersForRender);
		}

		public DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> GetAllDefinitions()
		{
			return new DictionaryValuesReader<MyDefinitionId, MyDefinitionBase>(m_definitions.m_definitionsById);
		}

		public ListReader<MyPhysicalItemDefinition> GetWeaponDefinitions()
		{
			return new ListReader<MyPhysicalItemDefinition>(m_definitions.m_physicalGunItemDefinitions);
		}

		public ListReader<MyPhysicalItemDefinition> GetConsumableDefinitions()
		{
			return new ListReader<MyPhysicalItemDefinition>(m_definitions.m_physicalConsumableItemDefinitions);
		}

		public ListReader<MySpawnGroupDefinition> GetSpawnGroupDefinitions()
		{
			return new ListReader<MySpawnGroupDefinition>(m_definitions.m_spawnGroupDefinitions);
		}

		public ListReader<MyScenarioDefinition> GetScenarioDefinitions()
		{
			return new ListReader<MyScenarioDefinition>(m_definitions.m_scenarioDefinitions);
		}

		public ListReader<MyAnimationDefinition> GetAnimationDefinitions()
		{
			return new ListReader<MyAnimationDefinition>(Enumerable.ToList<MyAnimationDefinition>(Enumerable.OfType<MyAnimationDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public IEnumerable<MyEmoteDefinition> GetEmoteDefinitions()
		{
			foreach (Dictionary<MyStringHash, MyDefinitionBase> value in m_definitions.Definitions.Values)
			{
				IEnumerable<MyEmoteDefinition> enumerable = value.Values.OfType<MyEmoteDefinition>();
				if (enumerable.Count() > 0)
				{
					return enumerable;
				}
			}
			return new List<MyEmoteDefinition>();
		}

		public Dictionary<string, MyAnimationDefinition> GetAnimationDefinitions(string skeleton)
		{
			return m_definitions.m_animationsBySkeletonType[skeleton];
		}

		public ListReader<MyDebrisDefinition> GetDebrisDefinitions()
		{
			return new ListReader<MyDebrisDefinition>(Enumerable.ToList<MyDebrisDefinition>(Enumerable.OfType<MyDebrisDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MyTransparentMaterialDefinition> GetTransparentMaterialDefinitions()
		{
			return new ListReader<MyTransparentMaterialDefinition>(Enumerable.ToList<MyTransparentMaterialDefinition>(Enumerable.OfType<MyTransparentMaterialDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MyPhysicalMaterialDefinition> GetPhysicalMaterialDefinitions()
		{
			return new ListReader<MyPhysicalMaterialDefinition>(Enumerable.ToList<MyPhysicalMaterialDefinition>(Enumerable.OfType<MyPhysicalMaterialDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MyEdgesDefinition> GetEdgesDefinitions()
		{
			return new ListReader<MyEdgesDefinition>(Enumerable.ToList<MyEdgesDefinition>(Enumerable.OfType<MyEdgesDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MyPhysicalItemDefinition> GetPhysicalItemDefinitions()
		{
			return new ListReader<MyPhysicalItemDefinition>(Enumerable.ToList<MyPhysicalItemDefinition>(Enumerable.OfType<MyPhysicalItemDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MyEnvironmentItemDefinition> GetEnvironmentItemDefinitions()
		{
			return new ListReader<MyEnvironmentItemDefinition>(Enumerable.ToList<MyEnvironmentItemDefinition>(Enumerable.OfType<MyEnvironmentItemDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MyEnvironmentItemsDefinition> GetEnvironmentItemClassDefinitions()
		{
			return new ListReader<MyEnvironmentItemsDefinition>(Enumerable.ToList<MyEnvironmentItemsDefinition>(Enumerable.OfType<MyEnvironmentItemsDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MyCompoundBlockTemplateDefinition> GetCompoundBlockTemplateDefinitions()
		{
			return new ListReader<MyCompoundBlockTemplateDefinition>(Enumerable.ToList<MyCompoundBlockTemplateDefinition>(Enumerable.OfType<MyCompoundBlockTemplateDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public DictionaryValuesReader<MyDefinitionId, MyAudioDefinition> GetSoundDefinitions()
		{
			return m_definitions.m_sounds;
		}

		internal ListReader<MyAudioEffectDefinition> GetAudioEffectDefinitions()
		{
			return new ListReader<MyAudioEffectDefinition>(Enumerable.ToList<MyAudioEffectDefinition>(Enumerable.OfType<MyAudioEffectDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MyMultiBlockDefinition> GetMultiBlockDefinitions()
		{
			return new ListReader<MyMultiBlockDefinition>(Enumerable.ToList<MyMultiBlockDefinition>(Enumerable.OfType<MyMultiBlockDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MySoundCategoryDefinition> GetSoundCategoryDefinitions()
		{
			return new ListReader<MySoundCategoryDefinition>(Enumerable.ToList<MySoundCategoryDefinition>(Enumerable.OfType<MySoundCategoryDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MyLCDTextureDefinition> GetLCDTexturesDefinitions()
		{
			return new ListReader<MyLCDTextureDefinition>(Enumerable.ToList<MyLCDTextureDefinition>(Enumerable.OfType<MyLCDTextureDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MyBehaviorDefinition> GetBehaviorDefinitions()
		{
			return new ListReader<MyBehaviorDefinition>(Enumerable.ToList<MyBehaviorDefinition>((IEnumerable<MyBehaviorDefinition>)m_definitions.m_behaviorDefinitions.Values));
		}

		public ListReader<MyBotDefinition> GetBotDefinitions()
		{
			return new ListReader<MyBotDefinition>(Enumerable.ToList<MyBotDefinition>(Enumerable.OfType<MyBotDefinition>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<T> GetDefinitionsOfType<T>() where T : MyDefinitionBase
		{
			return new ListReader<T>(Enumerable.ToList<T>(Enumerable.OfType<T>((IEnumerable)m_definitions.m_definitionsById.Values)));
		}

		public ListReader<MyVoxelMapStorageDefinition> GetVoxelMapStorageDefinitions()
		{
			return new ListReader<MyVoxelMapStorageDefinition>(Enumerable.ToList<MyVoxelMapStorageDefinition>((IEnumerable<MyVoxelMapStorageDefinition>)m_definitions.m_voxelMapStorages.Values));
		}

		public bool TryGetVoxelMapStorageDefinition(string name, out MyVoxelMapStorageDefinition definition)
		{
			return m_definitions.m_voxelMapStorages.TryGetValue(name, out definition);
		}

		public ListReader<MyVoxelMapStorageDefinition> GetVoxelMapStorageDefinitionsForProceduralRemovals()
		{
			return m_voxelMapStorageDefinitionsForProceduralRemovals.Value;
		}

		public ListReader<MyVoxelMapStorageDefinition> GetVoxelMapStorageDefinitionsForProceduralAdditions()
		{
			return m_voxelMapStorageDefinitionsForProceduralAdditions.Value;
		}

		public ListReader<MyVoxelMapStorageDefinition> GetVoxelMapStorageDefinitionsForProceduralPrimaryAdditions()
		{
			return m_voxelMapStorageDefinitionsForProceduralPrimaryAdditions.Value;
		}

		public ListReader<MyDefinitionBase> GetInventoryItemDefinitions()
		{
			return m_inventoryItemDefinitions.Value;
		}

		public MyScenarioDefinition GetScenarioDefinition(MyDefinitionId id)
		{
			CheckDefinition<MyScenarioDefinition>(ref id);
			return (MyScenarioDefinition)m_definitions.m_definitionsById[id];
		}

		public MyEdgesDefinition GetEdgesDefinition(MyDefinitionId id)
		{
			CheckDefinition<MyEdgesDefinition>(ref id);
			return (MyEdgesDefinition)m_definitions.m_definitionsById[id];
		}

		public void RegisterFactionDefinition(MyFactionDefinition definition)
		{
			if (Loading)
			{
				if (m_definitions.m_factionDefinitionsByTag.ContainsKey(definition.Tag))
				{
					string msg = "Faction with tag " + definition.Tag + " is already registered in the definition manager. Overwriting...";
					MySandboxGame.Log.WriteLine(msg);
				}
				m_definitions.m_factionDefinitionsByTag.Add(definition.Tag, definition);
			}
		}

		public MyFactionDefinition TryGetFactionDefinition(string tag)
		{
			m_definitions.m_factionDefinitionsByTag.TryGetValue(tag, out var value);
			return value;
		}

		/// <summary>
		/// Gets default factions.
		/// </summary>
		/// <returns>List of default factions.</returns>
		public List<MyFactionDefinition> GetDefaultFactions()
		{
			List<MyFactionDefinition> list = new List<MyFactionDefinition>();
			foreach (MyFactionDefinition value in m_definitions.m_factionDefinitionsByTag.Values)
			{
				if (value.IsDefault)
				{
					list.Add(value);
				}
			}
			return list;
		}

		/// <summary>
		/// Gets all factions from definition.
		/// </summary>
		/// <returns>List of factions.</returns>
		public List<MyFactionDefinition> GetFactionsFromDefinition()
		{
			List<MyFactionDefinition> list = new List<MyFactionDefinition>();
			foreach (MyFactionDefinition value in m_definitions.m_factionDefinitionsByTag.Values)
			{
				list.Add(value);
			}
			return list;
		}

		public MyContainerTypeDefinition GetContainerTypeDefinition(string containerName)
		{
			if (!m_definitions.m_containerTypeDefinitions.TryGetValue(new MyDefinitionId(typeof(MyObjectBuilder_ContainerTypeDefinition), containerName), out var value))
			{
				return null;
			}
			return value;
		}

		public MyContainerTypeDefinition GetContainerTypeDefinition(MyDefinitionId id)
		{
			if (!m_definitions.m_containerTypeDefinitions.TryGetValue(id, out var value))
			{
				return null;
			}
			return value;
		}

		public MySpawnGroupDefinition GetSpawnGroupDefinition(int index)
		{
			return m_definitions.m_spawnGroupDefinitions[index];
		}

		public bool HasRespawnShip(string id)
		{
			return m_definitions.m_respawnShips.ContainsKey(id);
		}

		public MyRespawnShipDefinition GetRespawnShipDefinition(string id)
		{
			m_definitions.m_respawnShips.TryGetValue(id, out var value);
			if (value == null)
			{
				return null;
			}
			if (value.Prefab == null)
			{
				return null;
			}
			return value;
		}

		public MyPrefabDefinition GetPrefabDefinition(string id)
		{
			m_definitions.m_prefabs.TryGetValue(id, out var value);
			return value;
		}

		public void ReloadPrefabsFromFile(string filePath)
		{
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = LoadWithProtobuffers<MyObjectBuilder_Definitions>(filePath);
			if (myObjectBuilder_Definitions.Prefabs != null)
			{
				MyObjectBuilder_PrefabDefinition[] prefabs = myObjectBuilder_Definitions.Prefabs;
				foreach (MyObjectBuilder_PrefabDefinition myObjectBuilder_PrefabDefinition in prefabs)
				{
					GetPrefabDefinition(myObjectBuilder_PrefabDefinition.Id.SubtypeId)?.InitLazy(myObjectBuilder_PrefabDefinition);
				}
			}
		}

		public DictionaryReader<string, MyPrefabDefinition> GetPrefabDefinitions()
		{
			return m_definitions.m_prefabs;
		}

		public DictionaryReader<string, MyWheelModelsDefinition> GetWheelModelDefinitions()
		{
			return m_definitions.m_wheelModels;
		}

		public DictionaryReader<string, MyRespawnShipDefinition> GetRespawnShipDefinitions()
		{
			return m_definitions.m_respawnShips;
		}

		public DictionaryReader<string, MyDropContainerDefinition> GetDropContainerDefinitions()
		{
			return m_definitions.m_dropContainers;
		}

		public DictionaryReader<string, MyBlockVariantGroup> GetBlockVariantGroupDefinitions()
		{
			return m_definitions.m_blockVariantGroups;
		}

		public DictionaryReader<string, MyAsteroidGeneratorDefinition> GetAsteroidGeneratorDefinitions()
		{
			return m_definitions.m_asteroidGenerators;
		}

		public void AddMissingWheelModelDefinition(string wheelType)
		{
			MyLog.Default.WriteLine("Missing wheel models definition in WheelModels.sbc for " + wheelType);
			m_definitions.m_wheelModels[wheelType] = new MyWheelModelsDefinition
			{
				AngularVelocityThreshold = float.MaxValue
			};
		}

		public MyDropContainerDefinition GetDropContainerDefinition(string id)
		{
			m_definitions.m_dropContainers.TryGetValue(id, out var value);
			if (value == null)
			{
				return null;
			}
			if (value.Prefab == null)
			{
				return null;
			}
			return value;
		}

		public string GetFirstRespawnShip()
		{
			if (m_definitions.m_respawnShips.Count > 0)
			{
				return Enumerable.FirstOrDefault<KeyValuePair<string, MyRespawnShipDefinition>>((IEnumerable<KeyValuePair<string, MyRespawnShipDefinition>>)m_definitions.m_respawnShips).Value.Id.SubtypeName;
			}
			return null;
		}

		public MyGlobalEventDefinition GetEventDefinition(MyDefinitionId id)
		{
			CheckDefinition<MyGlobalEventDefinition>(ref id);
			MyDefinitionBase value = null;
			m_definitions.m_definitionsById.TryGetValue(id, out value);
			return (MyGlobalEventDefinition)value;
		}

		public bool TryGetPhysicalItemDefinition(MyDefinitionId id, out MyPhysicalItemDefinition definition)
		{
			if (!TryGetDefinition<MyDefinitionBase>(id, out var definition2))
			{
				definition = null;
				return false;
			}
			definition = definition2 as MyPhysicalItemDefinition;
			return definition != null;
		}

		public MyPhysicalItemDefinition TryGetPhysicalItemDefinition(MyDefinitionId id)
		{
			if (!TryGetDefinition<MyDefinitionBase>(id, out var definition))
			{
				return null;
			}
			return definition as MyPhysicalItemDefinition;
		}

		public MyPhysicalItemDefinition GetPhysicalItemDefinition(MyObjectBuilder_Base objectBuilder)
		{
			return GetPhysicalItemDefinition(objectBuilder.GetId());
		}

		public MyAmmoDefinition GetAmmoDefinition(MyDefinitionId id)
		{
			return m_definitions.m_ammoDefinitionsById[id];
		}

		public MyPhysicalItemDefinition GetPhysicalItemDefinition(MyDefinitionId id)
		{
			if (!m_definitions.m_definitionsById.ContainsKey(id))
			{
				MyLog.Default.Critical(new StringBuilder($"Definition of \"{id.ToString()}\" is missing."));
				foreach (MyDefinitionBase value2 in m_definitions.m_definitionsById.Values)
				{
					if (value2 is MyPhysicalItemDefinition)
					{
						return value2 as MyPhysicalItemDefinition;
					}
				}
			}
			CheckDefinition<MyPhysicalItemDefinition>(ref id);
			if (m_definitions.m_definitionsById.TryGetValue(id, out var _))
			{
				return m_definitions.m_definitionsById[id] as MyPhysicalItemDefinition;
			}
			return null;
		}

		public void TryGetDefinitionsByTypeId(MyObjectBuilderType typeId, HashSet<MyDefinitionId> definitions)
		{
			foreach (MyDefinitionId key in m_definitions.m_definitionsById.Keys)
			{
				if (key.TypeId == typeId && !definitions.Contains(key))
				{
					definitions.Add(key);
				}
			}
		}

		public MyEnvironmentItemDefinition GetEnvironmentItemDefinition(MyDefinitionId id)
		{
			CheckDefinition<MyEnvironmentItemDefinition>(ref id);
			return m_definitions.m_definitionsById[id] as MyEnvironmentItemDefinition;
		}

		public MyCompoundBlockTemplateDefinition GetCompoundBlockTemplateDefinition(MyDefinitionId id)
		{
			CheckDefinition<MyCompoundBlockTemplateDefinition>(ref id);
			return m_definitions.m_definitionsById[id] as MyCompoundBlockTemplateDefinition;
		}

		public MyAmmoMagazineDefinition GetAmmoMagazineDefinition(MyDefinitionId id)
		{
			CheckDefinition<MyAmmoMagazineDefinition>(ref id);
			return m_definitions.m_definitionsById[id] as MyAmmoMagazineDefinition;
		}

		public MyShipSoundsDefinition GetShipSoundsDefinition(MyDefinitionId id)
		{
			CheckDefinition<MyShipSoundsDefinition>(ref id);
			return m_definitions.m_definitionsById[id] as MyShipSoundsDefinition;
		}

		public MyWeaponDefinition GetWeaponDefinition(MyDefinitionId id)
		{
			return m_definitions.m_weaponDefinitionsById[id];
		}

		public bool TryGetWeaponDefinition(MyDefinitionId defId, out MyWeaponDefinition definition)
		{
			if (!defId.TypeId.IsNull && m_definitions.m_weaponDefinitionsById.TryGetValue(defId, out var value))
			{
				definition = value;
				return definition != null;
			}
			definition = null;
			return false;
		}

		public MyBehaviorDefinition GetBehaviorDefinition(MyDefinitionId id)
		{
			return m_definitions.m_behaviorDefinitions[id];
		}

		public MyBotDefinition GetBotDefinition(MyDefinitionId id)
		{
			CheckDefinition<MyBotDefinition>(ref id);
			if (m_definitions.m_definitionsById.ContainsKey(id))
			{
				return m_definitions.m_definitionsById[id] as MyBotDefinition;
			}
			return null;
		}

		public bool TryGetBotDefinition(MyDefinitionId id, out MyBotDefinition botDefinition)
		{
			if (m_definitions.m_definitionsById.ContainsKey(id))
			{
				botDefinition = m_definitions.m_definitionsById[id] as MyBotDefinition;
				return true;
			}
			botDefinition = null;
			return false;
		}

		public MyAnimationDefinition TryGetAnimationDefinition(string animationSubtypeName)
		{
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_AnimationDefinition), animationSubtypeName);
			CheckDefinition<MyAnimationDefinition>(ref id);
			MyDefinitionBase value = null;
			m_definitions.m_definitionsById.TryGetValue(id, out value);
			return value as MyAnimationDefinition;
		}

		public string GetAnimationDefinitionCompatibility(string animationSubtypeName)
		{
			if (Static.TryGetDefinition<MyDefinitionBase>(new MyDefinitionId(typeof(MyObjectBuilder_AnimationDefinition), animationSubtypeName), out var _))
			{
				return animationSubtypeName;
			}
			foreach (MyAnimationDefinition animationDefinition in Static.GetAnimationDefinitions())
			{
				if (animationDefinition.AnimationModel == animationSubtypeName)
				{
					return animationDefinition.Id.SubtypeName;
				}
			}
			return animationSubtypeName;
		}

		public MyMultiBlockDefinition GetMultiBlockDefinition(MyDefinitionId id)
		{
			CheckDefinition<MyMultiBlockDefinition>(ref id);
			return m_definitions.m_definitionsById[id] as MyMultiBlockDefinition;
		}

		public MyMultiBlockDefinition TryGetMultiBlockDefinition(MyDefinitionId id)
		{
			if (!m_definitions.m_definitionsById.ContainsKey(id))
			{
				MySandboxGame.Log.WriteLine($"No multiblock definition '{id}'");
				return null;
			}
			return m_definitions.m_definitionsById[id] as MyMultiBlockDefinition;
		}

		public MyPhysicalItemDefinition GetPhysicalItemForHandItem(MyDefinitionId handItemId)
		{
			if (!m_definitions.m_physicalItemsByHandItemId.ContainsKey(handItemId))
			{
				return null;
			}
			return m_definitions.m_physicalItemsByHandItemId[handItemId];
		}

		public MyHandItemDefinition TryGetHandItemForPhysicalItem(MyDefinitionId physicalItemId)
		{
			if (!m_definitions.m_handItemsByPhysicalItemId.ContainsKey(physicalItemId))
			{
				MySandboxGame.Log.WriteLine($"No hand item for physical item '{physicalItemId}'");
				return null;
			}
			return m_definitions.m_handItemsByPhysicalItemId[physicalItemId];
		}

		public bool HandItemExistsFor(MyDefinitionId physicalItemId)
		{
			return m_definitions.m_handItemsByPhysicalItemId.ContainsKey(physicalItemId);
		}

		public MyDefinitionId? ItemIdFromWeaponId(MyDefinitionId weaponDefinition)
		{
			MyDefinitionId? result = null;
			if (weaponDefinition.TypeId != typeof(MyObjectBuilder_PhysicalGunObject))
			{
				MyPhysicalItemDefinition physicalItemForHandItem = Static.GetPhysicalItemForHandItem(weaponDefinition);
				if (physicalItemForHandItem != null)
				{
					result = physicalItemForHandItem.Id;
				}
			}
			else
			{
				result = weaponDefinition;
			}
			return result;
		}

		public float GetCubeSize(MyCubeSize gridSize)
		{
			return m_definitions.m_cubeSizes[(uint)gridSize];
		}

		public float GetCubeSizeOriginal(MyCubeSize gridSize)
		{
			return m_definitions.m_cubeSizesOriginal[(uint)gridSize];
		}

		public List<MyTargetingGroupDefinition> GetTargetingGroupDefinitions(List<MyTargetingGroupDefinition> list = null)
		{
			if (list == null)
			{
				list = new List<MyTargetingGroupDefinition>();
			}
			foreach (KeyValuePair<MyDefinitionId, MyTargetingGroupDefinition> targetingGroupDefinition in m_definitions.m_targetingGroupDefinitions)
			{
				list.Add(targetingGroupDefinition.Value);
			}
			list.Sort((MyTargetingGroupDefinition a, MyTargetingGroupDefinition b) => FloatComparer.Instance.Compare(a.SortOrder, b.SortOrder));
			return list;
		}

		public bool HasDefaultTargetingGroup(MyObjectBuilderType type, MyStringHash hash)
		{
			foreach (KeyValuePair<MyDefinitionId, MyTargetingGroupDefinition> targetingGroupDefinition in m_definitions.m_targetingGroupDefinitions)
			{
				if (targetingGroupDefinition.Value.DefaultBlockTypes.Contains(type) && (hash == MyStringHash.NullOrEmpty || targetingGroupDefinition.Value.Id.SubtypeId == hash))
				{
					return true;
				}
			}
			return false;
		}

		public MyLootBagDefinition GetLootBagDefinition()
		{
			return m_definitions.m_lootBagDefinition;
		}

		public MyPhysicalMaterialDefinition GetPhysicalMaterialDefinition(MyDefinitionId id)
		{
			CheckDefinition<MyPhysicalMaterialDefinition>(ref id);
			return m_definitions.m_definitionsById[id] as MyPhysicalMaterialDefinition;
		}

		public MyPhysicalMaterialDefinition GetPhysicalMaterialDefinition(string name)
		{
			MyPhysicalMaterialDefinition value = null;
			m_definitions.m_physicalMaterialsByName.TryGetValue(name, out value);
			return value;
		}

		public void GetOreTypeNames(out string[] outNames)
		{
			List<string> list = new List<string>();
			foreach (MyDefinitionBase value in m_definitions.m_definitionsById.Values)
			{
				if (value.Id.TypeId == typeof(MyObjectBuilder_Ore))
				{
					list.Add(value.Id.SubtypeName);
				}
			}
			outNames = list.ToArray();
		}

		private void CheckDefinition(ref MyDefinitionId id)
		{
			CheckDefinition<MyDefinitionBase>(ref id);
		}

		public MyEnvironmentItemsDefinition GetRandomEnvironmentClass(int channel)
		{
			MyEnvironmentItemsDefinition definition = null;
			List<MyDefinitionId> value = null;
			m_definitions.m_channelEnvironmentItemsDefs.TryGetValue(channel, out value);
			if (value == null)
			{
				return definition;
			}
			int index = MyRandom.Instance.Next(0, value.Count);
			MyDefinitionId defId = value[index];
			Static.TryGetDefinition<MyEnvironmentItemsDefinition>(defId, out definition);
			return definition;
		}

		public ListReader<MyDefinitionId> GetEnvironmentItemsDefinitions(int channel)
		{
			List<MyDefinitionId> value = null;
			m_definitions.m_channelEnvironmentItemsDefs.TryGetValue(channel, out value);
			return value;
		}

		private void CheckDefinition<T>(ref MyDefinitionId id) where T : MyDefinitionBase
		{
			try
			{
				MyDefinitionBase value = GetDefinition<T>(id.SubtypeId);
				if (value == null && !m_definitions.m_definitionsById.TryGetValue(id, out value))
				{
					string msg = $"No definition '{id}'. Maybe a mistake in XML?";
					MySandboxGame.Log.WriteLine(msg);
				}
				else if (!(value is T))
				{
					string msg2 = $"Definition '{id}' is not of desired type.";
					MySandboxGame.Log.WriteLine(msg2);
				}
			}
			catch (KeyNotFoundException)
			{
			}
		}

		public IEnumerable<MyPlanetGeneratorDefinition> GetPlanetsGeneratorsDefinitions()
		{
			return m_definitions.GetDefinitionsOfType<MyPlanetGeneratorDefinition>();
		}

		public DictionaryValuesReader<MyDefinitionId, MyPlanetPrefabDefinition> GetPlanetsPrefabsDefinitions()
		{
			return new DictionaryValuesReader<MyDefinitionId, MyPlanetPrefabDefinition>(m_definitions.m_planetPrefabDefinitions);
		}

		public DictionaryValuesReader<string, MyGroupedIds> GetGroupedIds(string superGroup)
		{
			return new DictionaryValuesReader<string, MyGroupedIds>(m_definitions.m_groupedIds[superGroup]);
		}

		public DictionaryValuesReader<MyDefinitionId, MyPirateAntennaDefinition> GetPirateAntennaDefinitions()
		{
			return new DictionaryValuesReader<MyDefinitionId, MyPirateAntennaDefinition>(m_definitions.m_pirateAntennaDefinitions);
		}

		public MyComponentGroupDefinition GetComponentGroup(MyDefinitionId groupDefId)
		{
			MyComponentGroupDefinition value = null;
			m_definitions.m_componentGroups.TryGetValue(groupDefId, out value);
			return value;
		}

		public MyComponentGroupDefinition GetGroupForComponent(MyDefinitionId componentDefId, out int amount)
		{
			if (m_definitions.m_componentGroupMembers.TryGetValue(componentDefId, out var value))
			{
				amount = value.Item1;
				return value.Item2;
			}
			amount = 0;
			return null;
		}

		public bool TryGetEntityComponentDefinition(MyDefinitionId componentId, out MyComponentDefinitionBase definition)
		{
			return m_definitions.m_entityComponentDefinitions.TryGetValue(componentId, out definition);
		}

		public MyComponentDefinitionBase GetEntityComponentDefinition(MyDefinitionId componentId)
		{
			return m_definitions.m_entityComponentDefinitions[componentId];
		}

		public ListReader<MyComponentDefinitionBase> GetEntityComponentDefinitions()
		{
			return GetEntityComponentDefinitions<MyComponentDefinitionBase>();
		}

		public ListReader<T> GetEntityComponentDefinitions<T>()
		{
			return new ListReader<T>(Enumerable.ToList<T>(Enumerable.OfType<T>((IEnumerable)m_definitions.m_entityComponentDefinitions.Values)));
		}

		public bool TryGetContainerDefinition(MyDefinitionId containerId, out MyContainerDefinition definition)
		{
			return m_definitions.m_entityContainers.TryGetValue(containerId, out definition);
		}

		public MyContainerDefinition GetContainerDefinition(MyDefinitionId containerId)
		{
			return m_definitions.m_entityContainers[containerId];
		}

		public void GetDefinedEntityContainers(ref List<MyDefinitionId> definedContainers)
		{
			foreach (KeyValuePair<MyDefinitionId, MyContainerDefinition> entityContainer in m_definitions.m_entityContainers)
			{
				definedContainers.Add(entityContainer.Key);
			}
		}

		internal void SetEntityContainerDefinition(MyContainerDefinition newDefinition)
		{
			if (m_definitions != null && m_definitions.m_entityContainers != null)
			{
				if (!m_definitions.m_entityContainers.ContainsKey(newDefinition.Id))
				{
					m_definitions.m_entityContainers.Add(newDefinition.Id, newDefinition);
				}
				else
				{
					m_definitions.m_entityContainers[newDefinition.Id] = newDefinition;
				}
			}
		}

		public MyVoxelMaterialDefinition GetVoxelMaterialDefinition(byte materialIndex)
		{
			using (m_voxelMaterialsLock.AcquireSharedUsing())
			{
				MyVoxelMaterialDefinition value = null;
				m_definitions.m_voxelMaterialsByIndex.TryGetValue(materialIndex, out value);
				return value;
			}
		}

		public MyVoxelMaterialDefinition GetVoxelMaterialDefinition(string name)
		{
			using (m_voxelMaterialsLock.AcquireSharedUsing())
			{
				MyVoxelMaterialDefinition value = null;
				m_definitions.m_voxelMaterialsByName.TryGetValue(name, out value);
				return value;
			}
		}

		internal byte GetVoxelMaterialDefinitionIndex(string name)
		{
			using (m_voxelMaterialsLock.AcquireSharedUsing())
			{
				foreach (KeyValuePair<byte, MyVoxelMaterialDefinition> item in m_definitions.m_voxelMaterialsByIndex)
				{
					MyStringHash subtypeId = item.Value.Id.SubtypeId;
					if (subtypeId.ToString() == name)
					{
						return item.Key;
					}
				}
			}
			return 0;
		}

		public bool TryGetVoxelMaterialDefinition(string name, out MyVoxelMaterialDefinition definition)
		{
			using (m_voxelMaterialsLock.AcquireSharedUsing())
			{
				return m_definitions.m_voxelMaterialsByName.TryGetValue(name, out definition);
			}
		}

		public DictionaryValuesReader<string, MyVoxelMaterialDefinition> GetVoxelMaterialDefinitions()
		{
			using (m_voxelMaterialsLock.AcquireSharedUsing())
			{
				return m_definitions.m_voxelMaterialsByName;
			}
		}

		public MyVoxelMaterialDefinition GetDefaultVoxelMaterialDefinition()
		{
			return m_definitions.m_voxelMaterialsByIndex[0];
		}

		private static void ToDefinitions(MyModContext context, DefinitionDictionary<MyDefinitionBase> outputDefinitions, DefinitionDictionary<MyCubeBlockDefinition>[] outputCubeBlocks, MyObjectBuilder_CubeBlockDefinition[] cubeBlocks, bool failOnDebug = true)
		{
			foreach (MyObjectBuilder_CubeBlockDefinition builder in cubeBlocks)
			{
				MyCubeBlockDefinition myCubeBlockDefinition = InitDefinition<MyCubeBlockDefinition>(context, builder);
				myCubeBlockDefinition.UniqueVersion = myCubeBlockDefinition;
				outputCubeBlocks[(uint)myCubeBlockDefinition.CubeSize][myCubeBlockDefinition.Id] = myCubeBlockDefinition;
				Check(!outputDefinitions.ContainsKey(myCubeBlockDefinition.Id), myCubeBlockDefinition.Id, failOnDebug);
				outputDefinitions[myCubeBlockDefinition.Id] = myCubeBlockDefinition;
				if (!context.IsBaseGame)
				{
					MySandboxGame.Log.WriteLine("Created definition for: " + myCubeBlockDefinition.DisplayNameText);
				}
			}
		}

		private static T InitDefinition<T>(MyModContext context, MyObjectBuilder_DefinitionBase builder) where T : MyDefinitionBase
		{
			T val = MyDefinitionManagerBase.GetObjectFactory().CreateInstance<T>(builder.GetType());
			val.Context = new MyModContext();
			val.Context.Init(context);
			if (!context.IsBaseGame)
			{
				UpdateModableContent(val.Context, builder);
			}
			val.Init(builder, val.Context);
			if (MyFakes.ENABLE_ALL_IN_SURVIVAL)
			{
				val.AvailableInSurvival = true;
			}
			return val;
		}

		private static void UpdateModableContent(MyModContext context, MyObjectBuilder_DefinitionBase builder)
		{
			using (Stats.Generic.Measure("UpdateModableContent", (MyStatTypeEnum)41))
			{
				FieldInfo[] fields = builder.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
				foreach (FieldInfo field in fields)
				{
					ProcessField(context, builder, field);
				}
			}
		}

		private static void ProcessField(MyModContext context, object fieldOwnerInstance, FieldInfo field, bool includeMembers = true)
		{
			string[] array = Enumerable.ToArray<string>(Enumerable.SelectMany<ModdableContentFileAttribute, string>(Enumerable.Cast<ModdableContentFileAttribute>((IEnumerable)field.GetCustomAttributes(typeof(ModdableContentFileAttribute), inherit: true)), (Func<ModdableContentFileAttribute, IEnumerable<string>>)((ModdableContentFileAttribute s) => Enumerable.Select<string, string>((IEnumerable<string>)s.FileExtensions, (Func<string, string>)((string ex) => "." + ex)))));
			if (array.Length != 0 && field.FieldType == typeof(string))
			{
				string contentFile = (string)field.GetValue(fieldOwnerInstance);
				object[] extensions = array;
				ProcessContentFilePath(context, ref contentFile, extensions, logNoExtensions: true);
				field.SetValue(fieldOwnerInstance, contentFile);
			}
			else if (field.FieldType == typeof(string[]))
			{
				string[] array2 = (string[])field.GetValue(fieldOwnerInstance);
				if (array2 != null)
				{
					for (int i = 0; i < array2.Length; i++)
					{
						ref string contentFile2 = ref array2[i];
						object[] extensions = array;
						ProcessContentFilePath(context, ref contentFile2, extensions, logNoExtensions: false);
					}
					field.SetValue(fieldOwnerInstance, array2);
				}
			}
			else if (array.Length != 0 && field.FieldType == typeof(List<string>))
			{
				List<string> list = (List<string>)field.GetValue(fieldOwnerInstance);
				if (list == null)
				{
					return;
				}
				for (int j = 0; j < list.Count; j++)
				{
					string contentFile3 = list[j];
					object[] extensions = array;
					ProcessContentFilePath(context, ref contentFile3, extensions, logNoExtensions: false);
					if (contentFile3 != null)
					{
						list[j] = contentFile3;
					}
				}
				field.SetValue(fieldOwnerInstance, list);
			}
			else
			{
				if (!includeMembers || (!field.FieldType.IsClass && (!field.FieldType.IsValueType || field.FieldType.IsPrimitive)))
				{
					return;
				}
				object value = field.GetValue(fieldOwnerInstance);
				IEnumerable enumerable = value as IEnumerable;
				if (enumerable != null)
				{
					foreach (object item in enumerable)
					{
						FieldInfo[] fields = item.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
						if (fields.Length != 0)
						{
							FieldInfo[] array3 = fields;
							foreach (FieldInfo field2 in array3)
							{
								ProcessField(context, item, field2, includeMembers: false);
							}
						}
					}
				}
				else if (value != null)
				{
					ProcessSubfields(context, field, value);
				}
			}
		}

		private static void ProcessContentFilePath(MyModContext context, ref string contentFile, object[] extensions, bool logNoExtensions)
		{
			if (string.IsNullOrEmpty(contentFile))
			{
				return;
			}
			string extension = Path.GetExtension(contentFile);
			if (extensions.IsNullOrEmpty())
			{
				if (logNoExtensions)
				{
					MyDefinitionErrors.Add(context, "List of supported file extensions not found. (Internal error)", TErrorSeverity.Warning);
				}
				return;
			}
			if (string.IsNullOrEmpty(extension))
			{
				MyDefinitionErrors.Add(context, "File does not have a proper extension: " + contentFile, TErrorSeverity.Warning);
				return;
			}
			if (!extensions.Contains(extension))
			{
				MyDefinitionErrors.Add(context, "File extension of: " + contentFile + " is not supported.", TErrorSeverity.Warning);
				return;
			}
			string text = Path.Combine(context.ModPath, contentFile);
			if (!m_directoryExistCache.TryGetValue(text, out var value))
			{
<<<<<<< HEAD
				value = MyFileSystem.DirectoryExists(Path.GetDirectoryName(text)) && MyFileSystem.GetFiles(Path.GetDirectoryName(text), Path.GetFileName(text), MySearchOption.TopDirectoryOnly).Any();
=======
				value = MyFileSystem.DirectoryExists(Path.GetDirectoryName(text)) && Enumerable.Any<string>(MyFileSystem.GetFiles(Path.GetDirectoryName(text), Path.GetFileName(text), MySearchOption.TopDirectoryOnly));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_directoryExistCache.Add(text, value);
			}
			if (value)
			{
				contentFile = text;
			}
			else if (!MyFileSystem.FileExists(Path.Combine(MyFileSystem.ContentPath, contentFile)))
			{
				if (contentFile.EndsWith(".mwm"))
				{
					MyDefinitionErrors.Add(context, "Resource not found, setting to error model. Resource path: " + text, TErrorSeverity.Error);
					contentFile = "Models\\Debug\\Error.mwm";
				}
				else
				{
					MyDefinitionErrors.Add(context, "Resource not found, setting to null. Resource path: " + text, TErrorSeverity.Error);
					contentFile = null;
				}
			}
		}

		private static void ProcessSubfields(MyModContext context, FieldInfo field, object instance)
		{
			FieldInfo[] fields = field.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo field2 in fields)
			{
				ProcessField(context, instance, field2);
			}
		}

		public void Save(string filePattern = "*.*")
		{
			Regex val = FindFilesPatternToRegex.Convert(filePattern);
			Dictionary<string, List<MyDefinitionBase>> dictionary = new Dictionary<string, List<MyDefinitionBase>>();
			foreach (KeyValuePair<MyDefinitionId, MyDefinitionBase> item in m_definitions.m_definitionsById)
			{
				if (string.IsNullOrEmpty(item.Value.Context.CurrentFile))
				{
					continue;
				}
				string fileName = Path.GetFileName(item.Value.Context.CurrentFile);
<<<<<<< HEAD
				if (regex.IsMatch(fileName))
=======
				if (val.IsMatch(fileName))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					List<MyDefinitionBase> list = null;
					if (!dictionary.ContainsKey(item.Value.Context.CurrentFile))
					{
						dictionary.Add(item.Value.Context.CurrentFile, list = new List<MyDefinitionBase>());
					}
					else
					{
						list = dictionary[item.Value.Context.CurrentFile];
					}
					list.Add(item.Value);
				}
			}
			foreach (KeyValuePair<string, List<MyDefinitionBase>> item2 in dictionary)
			{
				MyObjectBuilder_Definitions myObjectBuilder_Definitions = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Definitions>();
				List<MyObjectBuilder_DefinitionBase> list2 = new List<MyObjectBuilder_DefinitionBase>();
				foreach (MyDefinitionBase item3 in item2.Value)
				{
					MyObjectBuilder_DefinitionBase objectBuilder = item3.GetObjectBuilder();
					list2.Add(objectBuilder);
				}
				myObjectBuilder_Definitions.CubeBlocks = Enumerable.ToArray<MyObjectBuilder_CubeBlockDefinition>(Enumerable.OfType<MyObjectBuilder_CubeBlockDefinition>((IEnumerable)list2));
				MyObjectBuilderSerializer.SerializeXML(item2.Key, compress: false, myObjectBuilder_Definitions);
			}
		}

		private void FilterUnsupportedDLCs(MyObjectBuilder_Definitions definitions)
		{
			List<int> indicesToRemove = null;
			FilterDefinitions<MyObjectBuilder_DefinitionBase>(ref definitions.Definitions);
			FilterDefinitions<MyObjectBuilder_GridCreateToolDefinition>(ref definitions.GridCreators);
			FilterDefinitions<MyObjectBuilder_AmmoMagazineDefinition>(ref definitions.AmmoMagazines);
			FilterDefinitions<MyObjectBuilder_BlueprintDefinition>(ref definitions.Blueprints);
			FilterDefinitions<MyObjectBuilder_ComponentDefinition>(ref definitions.Components);
			FilterDefinitions<MyObjectBuilder_ContainerTypeDefinition>(ref definitions.ContainerTypes);
			FilterDefinitions<MyObjectBuilder_CubeBlockDefinition>(ref definitions.CubeBlocks);
			FilterDefinitions<MyObjectBuilder_EnvironmentDefinition>(ref definitions.Environments);
			FilterDefinitions<MyObjectBuilder_GlobalEventDefinition>(ref definitions.GlobalEvents);
			FilterDefinitions<MyObjectBuilder_HandItemDefinition>(ref definitions.HandItems);
			FilterDefinitions<MyObjectBuilder_PhysicalItemDefinition>(ref definitions.PhysicalItems);
			FilterDefinitions<MyObjectBuilder_SpawnGroupDefinition>(ref definitions.SpawnGroups);
			FilterDefinitions<MyObjectBuilder_TransparentMaterialDefinition>(ref definitions.TransparentMaterials);
			FilterDefinitions<MyObjectBuilder_VoxelMaterialDefinition>(ref definitions.VoxelMaterials);
			FilterDefinitions<MyObjectBuilder_CharacterDefinition>(ref definitions.Characters);
			FilterDefinitions<MyObjectBuilder_AnimationDefinition>(ref definitions.Animations);
			FilterDefinitions<MyObjectBuilder_DebrisDefinition>(ref definitions.Debris);
			FilterDefinitions<MyObjectBuilder_EdgesDefinition>(ref definitions.Edges);
			FilterDefinitions<MyObjectBuilder_FactionDefinition>(ref definitions.Factions);
			FilterDefinitions<MyObjectBuilder_PrefabDefinition>(ref definitions.Prefabs);
			FilterDefinitions<MyObjectBuilder_BlueprintClassDefinition>(ref definitions.BlueprintClasses);
			FilterDefinitions<MyObjectBuilder_EnvironmentItemDefinition>(ref definitions.EnvironmentItems);
			FilterDefinitions<MyObjectBuilder_CompoundBlockTemplateDefinition>(ref definitions.CompoundBlockTemplates);
			FilterDefinitions<MyObjectBuilder_RespawnShipDefinition>(ref definitions.RespawnShips);
			FilterDefinitions<MyObjectBuilder_DropContainerDefinition>(ref definitions.DropContainers);
			FilterDefinitions<MyObjectBuilder_WheelModelsDefinition>(ref definitions.WheelModels);
			FilterDefinitions<MyObjectBuilder_AsteroidGeneratorDefinition>(ref definitions.AsteroidGenerators);
			FilterDefinitions<MyObjectBuilder_GuiBlockCategoryDefinition>(ref definitions.CategoryClasses);
			FilterDefinitions<MyObjectBuilder_ShipBlueprintDefinition>(ref definitions.ShipBlueprints);
			FilterDefinitions<MyObjectBuilder_WeaponDefinition>(ref definitions.Weapons);
			FilterDefinitions<MyObjectBuilder_AmmoDefinition>(ref definitions.Ammos);
			FilterDefinitions<MyObjectBuilder_AudioDefinition>(ref definitions.Sounds);
			FilterDefinitions<MyObjectBuilder_AssetModifierDefinition>(ref definitions.AssetModifiers);
			FilterDefinitions<MyObjectBuilder_MainMenuInventorySceneDefinition>(ref definitions.MainMenuInventoryScenes);
			FilterDefinitions<MyObjectBuilder_VoxelHandDefinition>(ref definitions.VoxelHands);
			FilterDefinitions<MyObjectBuilder_MultiBlockDefinition>(ref definitions.MultiBlocks);
			FilterDefinitions<MyObjectBuilder_PrefabThrowerDefinition>(ref definitions.PrefabThrowers);
			FilterDefinitions<MyObjectBuilder_SoundCategoryDefinition>(ref definitions.SoundCategories);
			FilterDefinitions<MyObjectBuilder_ShipSoundsDefinition>(ref definitions.ShipSoundGroups);
			FilterDefinitions<MyObjectBuilder_DroneBehaviorDefinition>(ref definitions.DroneBehaviors);
			FilterDefinitions<MyObjectBuilder_ParticleEffect>(ref definitions.ParticleEffects);
			FilterDefinitions<MyObjectBuilder_BehaviorTreeDefinition>(ref definitions.AIBehaviors);
			FilterDefinitions<MyObjectBuilder_VoxelMapStorageDefinition>(ref definitions.VoxelMapStorages);
			FilterDefinitions<MyObjectBuilder_LCDTextureDefinition>(ref definitions.LCDTextures);
			FilterDefinitions<MyObjectBuilder_BotDefinition>(ref definitions.Bots);
			FilterDefinitions<MyObjectBuilder_PhysicalMaterialDefinition>(ref definitions.PhysicalMaterials);
			FilterDefinitions<MyObjectBuilder_AiCommandDefinition>(ref definitions.AiCommands);
			FilterDefinitions<MyObjectBuilder_BlockNavigationDefinition>(ref definitions.BlockNavigationDefinitions);
			FilterDefinitions<MyObjectBuilder_CuttingDefinition>(ref definitions.Cuttings);
			FilterDefinitions<MyObjectBuilder_MaterialPropertiesDefinition>(ref definitions.MaterialProperties);
			FilterDefinitions<MyObjectBuilder_ControllerSchemaDefinition>(ref definitions.ControllerSchemas);
			FilterDefinitions<MyObjectBuilder_CurveDefinition>(ref definitions.CurveDefinitions);
			FilterDefinitions<MyObjectBuilder_AudioEffectDefinition>(ref definitions.AudioEffects);
			FilterDefinitions<MyObjectBuilder_EnvironmentItemsDefinition>(ref definitions.EnvironmentItemsDefinitions);
			FilterDefinitions<MyObjectBuilder_DecalDefinition>(ref definitions.Decals);
			FilterDefinitions<MyObjectBuilder_EmissiveColorDefinition>(ref definitions.EmissiveColors);
			FilterDefinitions<MyObjectBuilder_EmissiveColorStatePresetDefinition>(ref definitions.EmissiveColorStatePresets);
			FilterDefinitions<MyObjectBuilder_PlanetGeneratorDefinition>(ref definitions.PlanetGeneratorDefinitions);
			FilterDefinitions<MyObjectBuilder_EntityStatDefinition>(ref definitions.StatDefinitions);
			FilterDefinitions<MyObjectBuilder_GasProperties>(ref definitions.GasProperties);
			FilterDefinitions<MyObjectBuilder_ResourceDistributionGroup>(ref definitions.ResourceDistributionGroups);
			FilterDefinitions<MyObjectBuilder_ComponentGroupDefinition>(ref definitions.ComponentGroups);
			FilterDefinitions<MyObjectBuilder_PlanetPrefabDefinition>(ref definitions.PlanetPrefabs);
			FilterDefinitions<MyObjectBuilder_PirateAntennaDefinition>(ref definitions.PirateAntennas);
			FilterDefinitions<MyObjectBuilder_ComponentDefinitionBase>(ref definitions.EntityComponents);
			FilterDefinitions<MyObjectBuilder_ContainerDefinition>(ref definitions.EntityContainers);
			FilterDefinitions<MyObjectBuilder_ShadowTextureSetDefinition>(ref definitions.ShadowTextureSets);
			FilterDefinitions<MyObjectBuilder_FontDefinition>(ref definitions.Fonts);
			FilterDefinitions<MyObjectBuilder_FlareDefinition>(ref definitions.Flares);
			FilterDefinitions<MyObjectBuilder_ResearchBlockDefinition>(ref definitions.ResearchBlocks);
			FilterDefinitions<MyObjectBuilder_ResearchGroupDefinition>(ref definitions.ResearchGroups);
			FilterDefinitions<MyObjectBuilder_ContractTypeDefinition>(ref definitions.ContractTypes);
			FilterDefinitions<MyObjectBuilder_FactionNameDefinition>(ref definitions.FactionNames);
			FilterDefinitions<MyObjectBuilder_RadialMenu>(ref definitions.RadialMenus);
			FilterDefinitions<MyObjectBuilder_OffensiveWords>(ref definitions.OffensiveWords);
			FilterDefinitions<MyObjectBuilder_BlockVariantGroup>(ref definitions.BlockVariantGroups);
			FilterDefinitions<MyObjectBuilder_WeatherEffectDefinition>(ref definitions.WeatherEffects);
<<<<<<< HEAD
			FilterDefinitions<MyObjectBuilder_ControllerSchemeDefinition>(ref definitions.ControllerSchemes);
			FilterDefinitions<MyObjectBuilder_TargetingGroupDefinition>(ref definitions.TargetingGroups);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			void FilterDefinitions<T>(ref T[] definitionArray) where T : MyObjectBuilder_DefinitionBase
			{
				if (definitionArray != null)
				{
					for (int i = 0; i < definitionArray.Length; i++)
					{
						string[] dLCs = definitionArray[i].DLCs;
						if (dLCs != null)
						{
							for (int j = 0; j < dLCs.Length; j++)
							{
								if (!MyDLCs.IsDLCSupported(dLCs[j]))
								{
									if (indicesToRemove == null)
									{
										indicesToRemove = new List<int>();
									}
									indicesToRemove.Add(i);
									break;
								}
							}
						}
					}
					if (indicesToRemove != null && indicesToRemove.Count > 0)
					{
						definitionArray = definitionArray.Without(indicesToRemove);
						indicesToRemove.Clear();
					}
				}
			}
		}
	}
}
