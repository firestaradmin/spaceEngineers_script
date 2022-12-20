using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.EntityComponents
{
	public class MyResourceDistributorComponent : MyEntityComponentBase, IMyResourceDistributorComponent
	{
		private struct MyPhysicalDistributionGroup
		{
			public IMyConveyorEndpoint FirstEndpoint;

			public HashSet<MyResourceSinkComponent>[] SinksByPriority;

			public HashSet<MyResourceSourceComponent>[] SourcesByPriority;

			public List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> SinkSourcePairs;

			public MySinkGroupData[] SinkDataByPriority;

			public MySourceGroupData[] SourceDataByPriority;

			public MyTuple<MySinkGroupData, MySourceGroupData> InputOutputData;

			public MyList<int> StockpilingStorage;

			public MyList<int> OtherStorage;

			public float MaxAvailableResources;

			public MyResourceStateEnum ResourceState;

			public MyPhysicalDistributionGroup(MyDefinitionId typeId, IMyConveyorEndpointBlock block)
			{
				SinksByPriority = null;
				SourcesByPriority = null;
				SinkSourcePairs = null;
				FirstEndpoint = null;
				SinkDataByPriority = null;
				SourceDataByPriority = null;
				StockpilingStorage = null;
				OtherStorage = null;
				InputOutputData = default(MyTuple<MySinkGroupData, MySourceGroupData>);
				MaxAvailableResources = 0f;
				ResourceState = MyResourceStateEnum.NoPower;
				AllocateData();
				Init(typeId, block);
			}

			public MyPhysicalDistributionGroup(MyDefinitionId typeId, MyResourceSinkComponent tempConnectedSink)
			{
				SinksByPriority = null;
				SourcesByPriority = null;
				SinkSourcePairs = null;
				FirstEndpoint = null;
				SinkDataByPriority = null;
				SourceDataByPriority = null;
				StockpilingStorage = null;
				OtherStorage = null;
				InputOutputData = default(MyTuple<MySinkGroupData, MySourceGroupData>);
				MaxAvailableResources = 0f;
				ResourceState = MyResourceStateEnum.NoPower;
				AllocateData();
				InitFromTempConnected(typeId, tempConnectedSink);
			}

			public MyPhysicalDistributionGroup(MyDefinitionId typeId, MyResourceSourceComponent tempConnectedSource)
			{
				SinksByPriority = null;
				SourcesByPriority = null;
				SinkSourcePairs = null;
				FirstEndpoint = null;
				SinkDataByPriority = null;
				SourceDataByPriority = null;
				StockpilingStorage = null;
				OtherStorage = null;
				InputOutputData = default(MyTuple<MySinkGroupData, MySourceGroupData>);
				MaxAvailableResources = 0f;
				ResourceState = MyResourceStateEnum.NoPower;
				AllocateData();
				InitFromTempConnected(typeId, tempConnectedSource);
			}

			public void Init(MyDefinitionId typeId, IMyConveyorEndpointBlock block)
			{
				ClearData();
				FirstEndpoint = block.ConveyorEndpoint;
				Add(typeId, block);
			}

			public void InitFromTempConnected(MyDefinitionId typeId, MyResourceSinkComponent tempConnectedSink)
			{
				ClearData();
				IMyConveyorEndpointBlock myConveyorEndpointBlock;
				if (FirstEndpoint == null && (myConveyorEndpointBlock = tempConnectedSink.TemporaryConnectedEntity as IMyConveyorEndpointBlock) != null)
				{
					FirstEndpoint = myConveyorEndpointBlock.ConveyorEndpoint;
				}
				AddTempConnected(typeId, tempConnectedSink);
			}

			public void InitFromTempConnected(MyDefinitionId typeId, MyResourceSourceComponent tempConnectedSource)
			{
				ClearData();
				IMyConveyorEndpointBlock myConveyorEndpointBlock;
				if ((myConveyorEndpointBlock = tempConnectedSource.TemporaryConnectedEntity as IMyConveyorEndpointBlock) != null)
				{
					FirstEndpoint = myConveyorEndpointBlock.ConveyorEndpoint;
				}
				AddTempConnected(typeId, tempConnectedSource);
			}

			public void Add(MyDefinitionId typeId, IMyConveyorEndpointBlock endpoint)
			{
				if (FirstEndpoint == null)
				{
					FirstEndpoint = endpoint.ConveyorEndpoint;
				}
				MyEntityComponentContainer components = (endpoint as IMyEntity).Components;
				MyResourceSinkComponent myResourceSinkComponent = components.Get<MyResourceSinkComponent>();
				MyResourceSourceComponent myResourceSourceComponent = components.Get<MyResourceSourceComponent>();
				bool flag = myResourceSinkComponent != null && Enumerable.Contains<MyDefinitionId>((IEnumerable<MyDefinitionId>)myResourceSinkComponent.AcceptedResources, typeId);
				bool flag2 = myResourceSourceComponent != null && Enumerable.Contains<MyDefinitionId>((IEnumerable<MyDefinitionId>)myResourceSourceComponent.ResourceTypes, typeId);
				if (flag && flag2)
				{
					SinkSourcePairs.Add(new MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>(myResourceSinkComponent, myResourceSourceComponent));
				}
				else if (flag)
				{
					SinksByPriority[GetPriority(myResourceSinkComponent)].Add(myResourceSinkComponent);
				}
				else if (flag2)
				{
					SourcesByPriority[GetPriority(myResourceSourceComponent)].Add(myResourceSourceComponent);
				}
			}

			public void AddTempConnected(MyDefinitionId typeId, MyResourceSinkComponent tempConnectedSink)
			{
				if (tempConnectedSink != null && Enumerable.Contains<MyDefinitionId>((IEnumerable<MyDefinitionId>)tempConnectedSink.AcceptedResources, typeId))
				{
					SinksByPriority[GetPriority(tempConnectedSink)].Add(tempConnectedSink);
				}
			}

			public void AddTempConnected(MyDefinitionId typeId, MyResourceSourceComponent tempConnectedSource)
			{
				if (tempConnectedSource != null && Enumerable.Contains<MyDefinitionId>((IEnumerable<MyDefinitionId>)tempConnectedSource.ResourceTypes, typeId))
				{
					SourcesByPriority[GetPriority(tempConnectedSource)].Add(tempConnectedSource);
				}
			}

			private void AllocateData()
			{
				FirstEndpoint = null;
				SinksByPriority = new HashSet<MyResourceSinkComponent>[m_sinkGroupPrioritiesTotal];
				SourcesByPriority = new HashSet<MyResourceSourceComponent>[m_sourceGroupPrioritiesTotal];
				SinkSourcePairs = new List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>();
				SinkDataByPriority = new MySinkGroupData[m_sinkGroupPrioritiesTotal];
				SourceDataByPriority = new MySourceGroupData[m_sourceGroupPrioritiesTotal];
				StockpilingStorage = new MyList<int>();
				OtherStorage = new MyList<int>();
				for (int i = 0; i < m_sinkGroupPrioritiesTotal; i++)
				{
					SinksByPriority[i] = new HashSet<MyResourceSinkComponent>();
				}
				for (int j = 0; j < m_sourceGroupPrioritiesTotal; j++)
				{
					SourcesByPriority[j] = new HashSet<MyResourceSourceComponent>();
				}
			}

			public void ClearData()
			{
				HashSet<MyResourceSinkComponent>[] sinksByPriority = SinksByPriority;
				for (int i = 0; i < sinksByPriority.Length; i++)
				{
					sinksByPriority[i].Clear();
				}
				HashSet<MyResourceSourceComponent>[] sourcesByPriority = SourcesByPriority;
				for (int i = 0; i < sourcesByPriority.Length; i++)
				{
					sourcesByPriority[i].Clear();
				}
				SinkSourcePairs.Clear();
				StockpilingStorage.ClearFast();
				OtherStorage.ClearFast();
				FirstEndpoint = null;
<<<<<<< HEAD
			}
		}

		private struct MyElectricalDistributionGroup
		{
			public List<long> GridsEntityIds;

			public HashSet<MyResourceSinkComponent>[] SinksByPriority;

			public HashSet<MyResourceSourceComponent>[] SourcesByPriority;

			public List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> SinkSourcePairs;

			public MySinkGroupData[] SinkDataByPriority;

			public MySourceGroupData[] SourceDataByPriority;

			public MyList<int> StockpilingStorage;

			public MyList<int> OtherStorage;

			public MyTuple<MySinkGroupData, MySourceGroupData> InputOutputData;

			public bool RemainingFuelTimeDirty;

			public float RemainingFuelTime;

			public float MaxAvailableResource;

			public List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> InputOutputList;

			public MyResourceStateEnum ResourceState;

			private MyResourceDistributorComponent parentComponent;

			public MyElectricalDistributionGroup(MyDefinitionId typeId, List<long> gridsEntityIds, MyResourceDistributorComponent parent)
			{
				GridsEntityIds = gridsEntityIds;
				SinksByPriority = null;
				SourcesByPriority = null;
				SinkSourcePairs = null;
				SinkDataByPriority = null;
				SourceDataByPriority = null;
				StockpilingStorage = null;
				OtherStorage = null;
				InputOutputData = default(MyTuple<MySinkGroupData, MySourceGroupData>);
				InputOutputList = new List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>();
				MaxAvailableResource = 0f;
				ResourceState = MyResourceStateEnum.NoPower;
				RemainingFuelTime = 0f;
				RemainingFuelTimeDirty = true;
				parentComponent = parent;
				AllocateData();
			}

			private void AllocateData()
			{
				SinksByPriority = new HashSet<MyResourceSinkComponent>[m_sinkGroupPrioritiesTotal];
				SourcesByPriority = new HashSet<MyResourceSourceComponent>[m_sourceGroupPrioritiesTotal];
				SinkSourcePairs = new List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>();
				SinkDataByPriority = new MySinkGroupData[m_sinkGroupPrioritiesTotal];
				SourceDataByPriority = new MySourceGroupData[m_sourceGroupPrioritiesTotal];
				InputOutputList = new List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>();
				StockpilingStorage = new MyList<int>();
				OtherStorage = new MyList<int>();
				for (int i = 0; i < m_sinkGroupPrioritiesTotal; i++)
				{
					SinksByPriority[i] = new HashSet<MyResourceSinkComponent>();
				}
				for (int j = 0; j < m_sourceGroupPrioritiesTotal; j++)
				{
					SourcesByPriority[j] = new HashSet<MyResourceSourceComponent>();
				}
			}

			public void Add(MyDefinitionId typeId, MyResourceSinkComponent sink)
			{
				if (sink != null && sink.AcceptedResources.Contains(typeId))
				{
					SinksByPriority[GetPriority(sink)].Add(sink);
				}
			}

			public void Add(MyDefinitionId typeId, MyResourceSourceComponent source)
			{
				if (SourcesByPriority != null && source.ResourceTypes.Contains(typeId))
				{
					SourcesByPriority[GetPriority(source)].Add(source);
				}
			}

			public void ClearData()
			{
				SinksByPriority = new HashSet<MyResourceSinkComponent>[m_sinkGroupPrioritiesTotal];
				SourcesByPriority = new HashSet<MyResourceSourceComponent>[m_sourceGroupPrioritiesTotal];
				SinkSourcePairs.Clear();
				StockpilingStorage.ClearFast();
				OtherStorage.ClearFast();
			}

			public float GetPowerInUse()
			{
				float num = 0f;
				MySinkGroupData[] sinkDataByPriority = SinkDataByPriority;
				for (int i = 0; i < sinkDataByPriority.Length; i++)
				{
					MySinkGroupData mySinkGroupData = sinkDataByPriority[i];
					if (mySinkGroupData.RemainingAvailableResource >= mySinkGroupData.RequiredInput)
					{
						num += mySinkGroupData.RequiredInput;
						continue;
					}
					if (!mySinkGroupData.IsAdaptible)
					{
						break;
					}
					num += mySinkGroupData.RemainingAvailableResource;
				}
				num = ((!(InputOutputData.Item1.RemainingAvailableResource > InputOutputData.Item1.RequiredInput)) ? (num + InputOutputData.Item1.RemainingAvailableResource) : (num + InputOutputData.Item1.RequiredInput));
				for (int j = 0; j < SourcesByPriority.Length; j++)
				{
					MySourceGroupData mySourceGroupData = SourceDataByPriority[j];
					if (!(mySourceGroupData.UsageRatio <= 0f) && mySourceGroupData.InfiniteCapacity)
					{
						num -= mySourceGroupData.UsageRatio * mySourceGroupData.MaxAvailableResource;
					}
				}
				return num;
			}

			public float ComputeRemainingElectricityTime()
			{
				MyDefinitionId typeId = ElectricityId;
				parentComponent.GetTypeIndex(ref typeId);
				if (MaxAvailableResource == 0f)
				{
					return 0f;
				}
				float powerInUse = GetPowerInUse();
				bool flag = false;
				bool flag2 = false;
				float num = 0f;
				for (int i = 0; i < SourcesByPriority.Length; i++)
				{
					MySourceGroupData mySourceGroupData = SourceDataByPriority[i];
					if (mySourceGroupData.UsageRatio <= 0f)
					{
						continue;
					}
					if (mySourceGroupData.InfiniteCapacity)
					{
						flag = true;
						continue;
					}
					foreach (MyResourceSourceComponent item in SourcesByPriority[i])
					{
						if (item.Enabled && item.ProductionEnabledByType(typeId) && item.CountTowardsRemainingEnergyTime)
						{
							flag2 = true;
							num += item.RemainingCapacityByType(typeId);
						}
					}
				}
				if (InputOutputData.Item2.UsageRatio > 0f)
				{
					foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in InputOutputList)
					{
						if (inputOutput.Item2.Enabled && inputOutput.Item2.ProductionEnabledByType(typeId))
						{
							flag2 = true;
							num += inputOutput.Item2.RemainingCapacityByType(typeId);
						}
					}
				}
				if (flag && !flag2)
				{
					return float.PositiveInfinity;
				}
				if (powerInUse > 0f)
				{
					return num / powerInUse;
				}
				return float.PositiveInfinity;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		/// <summary>
		/// Some precomputed data for each priority group.
		/// </summary>
		private struct MySinkGroupData
		{
			public bool IsAdaptible;

			public float RequiredInput;

			public float RequiredInputCumulative;

			public float RemainingAvailableResource;

			public override string ToString()
			{
				return $"IsAdaptible: {IsAdaptible}, RequiredInput: {RequiredInput}, RemainingAvailableResource: {RemainingAvailableResource}";
			}
		}

		private struct MySourceGroupData
		{
			public float MaxAvailableResource;

			public float UsageRatio;

			public bool InfiniteCapacity;

			public int ActiveCount;

			public override string ToString()
			{
				return $"MaxAvailableResource: {MaxAvailableResource}, UsageRatio: {UsageRatio}";
			}
		}

		private class PerTypeData
		{
			private bool m_needsRecompute;

			public MyDefinitionId TypeId;

			public MySinkGroupData[] SinkDataByPriority;

			public MySourceGroupData[] SourceDataByPriority;

			public MyTuple<MySinkGroupData, MySourceGroupData> InputOutputData;

			public HashSet<MyResourceSinkComponent>[] SinksByPriority;

			public HashSet<MyResourceSourceComponent>[] SourcesByPriority;

			public List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> InputOutputList;

			public MyList<int> StockpilingStorageIndices;

			public MyList<int> OtherStorageIndices;

			public List<MyPhysicalDistributionGroup> PhysicalDistributionGroups;

			public List<MyElectricalDistributionGroup> ElectricalDistributionGroups;

			public int DistributionGroupsInUse;

			public bool GroupsDirty;

			public int SourceCount;

			public float RemainingFuelTime;

			public bool RemainingFuelTimeDirty;

			public float MaxAvailableResource;

			public MyMultipleEnabledEnum SourcesEnabled;

			public bool SourcesEnabledDirty;

			public MyResourceStateEnum ResourceState;

			private bool m_gridsForUpdateValid;

			private MyCubeGrid m_gridForUpdate;

			private bool m_gridUpdateScheduled;

			private Action m_UpdateGridsCallback;

			public bool NeedsRecompute
			{
				get
				{
					return m_needsRecompute;
				}
				set
				{
					if (m_needsRecompute != value)
					{
						m_needsRecompute = value;
						if (m_needsRecompute)
						{
							ScheduleGridUpdate();
						}
					}
				}
			}

			public PerTypeData()
			{
				m_UpdateGridsCallback = UpdateGrids;
				ElectricalDistributionGroups = new List<MyElectricalDistributionGroup>();
			}

			public void InvalidateGridForUpdateCache()
			{
				m_gridForUpdate = null;
				m_gridsForUpdateValid = false;
			}

			private void ScheduleGridUpdate()
			{
				if (!m_gridUpdateScheduled)
				{
					m_gridUpdateScheduled = true;
					MySandboxGame.Static.Invoke(m_UpdateGridsCallback, "UpdateResourcesOnGrids");
				}
			}

			private void UpdateGrids()
			{
				m_gridUpdateScheduled = false;
				if (!m_gridsForUpdateValid)
				{
					m_gridsForUpdateValid = true;
					m_gridForUpdate = FindGridForUpdate();
				}
				MyCubeGrid gridForUpdate = m_gridForUpdate;
				if (gridForUpdate != null && !gridForUpdate.Closed)
				{
					m_gridForUpdate.GridSystems?.ResourceDistributor?.Schedule();
				}
				MyCubeGrid FindGridForUpdate()
				{
<<<<<<< HEAD
=======
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					//IL_0075: Unknown result type (might be due to invalid IL or missing references)
					//IL_007a: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					HashSet<MyResourceSourceComponent>[] sourcesByPriority = SourcesByPriority;
					for (int i = 0; i < sourcesByPriority.Length; i++)
					{
						Enumerator<MyResourceSourceComponent> enumerator = sourcesByPriority[i].GetEnumerator();
						try
						{
<<<<<<< HEAD
							MyCubeBlock myCubeBlock;
							MyCubeGrid myCubeGrid = (((myCubeBlock = item.Entity as MyCubeBlock) != null) ? myCubeBlock.CubeGrid : null);
							if (myCubeGrid != null)
							{
								return myCubeGrid;
							}
=======
							while (enumerator.MoveNext())
							{
								MyCubeBlock myCubeBlock;
								MyCubeGrid myCubeGrid = (((myCubeBlock = enumerator.get_Current().Entity as MyCubeBlock) != null) ? myCubeBlock.CubeGrid : null);
								if (myCubeGrid != null)
								{
									return myCubeGrid;
								}
							}
						}
						finally
						{
							((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					HashSet<MyResourceSinkComponent>[] sinksByPriority = SinksByPriority;
					for (int i = 0; i < sinksByPriority.Length; i++)
					{
						Enumerator<MyResourceSinkComponent> enumerator2 = sinksByPriority[i].GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								MyCubeBlock myCubeBlock2;
								MyCubeGrid myCubeGrid2 = (((myCubeBlock2 = enumerator2.get_Current().Entity as MyCubeBlock) != null) ? myCubeBlock2.CubeGrid : null);
								if (myCubeGrid2 != null)
								{
									return myCubeGrid2;
								}
							}
						}
						finally
						{
<<<<<<< HEAD
							MyCubeBlock myCubeBlock2;
							MyCubeGrid myCubeGrid2 = (((myCubeBlock2 = item2.Entity as MyCubeBlock) != null) ? myCubeBlock2.CubeGrid : null);
							if (myCubeGrid2 != null)
							{
								return myCubeGrid2;
							}
=======
							((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					return null;
				}
			}

			public void ClearData()
			{
<<<<<<< HEAD
				if (PhysicalDistributionGroups != null)
				{
					for (int i = 0; i < PhysicalDistributionGroups.Count; i++)
					{
						MyPhysicalDistributionGroup value = PhysicalDistributionGroups[i];
						value.ClearData();
						PhysicalDistributionGroups[i] = value;
					}
					for (int j = 0; j < ElectricalDistributionGroups.Count; j++)
					{
						MyElectricalDistributionGroup value2 = ElectricalDistributionGroups[j];
						value2.ClearData();
						ElectricalDistributionGroups[j] = value2;
=======
				if (DistributionGroups != null)
				{
					for (int i = 0; i < DistributionGroups.Count; i++)
					{
						MyPhysicalDistributionGroup value = DistributionGroups[i];
						value.ClearData();
						DistributionGroups[i] = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					DistributionGroupsInUse = 0;
				}
				HashSet<MyResourceSinkComponent>[] sinksByPriority = SinksByPriority;
<<<<<<< HEAD
				for (int k = 0; k < sinksByPriority.Length; k++)
				{
					sinksByPriority[k].Clear();
				}
				HashSet<MyResourceSourceComponent>[] sourcesByPriority = SourcesByPriority;
				for (int k = 0; k < sourcesByPriority.Length; k++)
				{
					sourcesByPriority[k].Clear();
				}
				InputOutputList.Clear();
				InvalidateGridForUpdateCache();
			}

			public void Add(MyDefinitionId typeId, MyResourceSourceComponent source)
			{
				SourcesByPriority[GetPriority(source)].Add(source);
			}

			public void Add(MyDefinitionId typeId, MyResourceSinkComponent sink)
			{
				SinksByPriority[GetPriority(sink)].Add(sink);
=======
				for (int j = 0; j < sinksByPriority.Length; j++)
				{
					sinksByPriority[j].Clear();
				}
				HashSet<MyResourceSourceComponent>[] sourcesByPriority = SourcesByPriority;
				for (int j = 0; j < sourcesByPriority.Length; j++)
				{
					sourcesByPriority[j].Clear();
				}
				InputOutputList.Clear();
				InvalidateGridForUpdateCache();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private class Sandbox_Game_EntityComponents_MyResourceDistributorComponent_003C_003EActor
		{
		}

		public static readonly MyDefinitionId ElectricityId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Electricity");

		public static readonly MyDefinitionId HydrogenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Hydrogen");

		public static readonly MyDefinitionId OxygenId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen");

		private int m_typeGroupCount;

		private bool m_forceRecalculation;

		private readonly List<PerTypeData> m_dataPerType = new List<PerTypeData>();

<<<<<<< HEAD
		protected readonly HashSet<MyDefinitionId> m_initializedTypes = new HashSet<MyDefinitionId>(MyDefinitionId.Comparer);
=======
		protected readonly HashSet<MyDefinitionId> m_initializedTypes = new HashSet<MyDefinitionId>((IEqualityComparer<MyDefinitionId>)MyDefinitionId.Comparer);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private readonly Dictionary<MyDefinitionId, int> m_typeIdToIndex = new Dictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);

		private readonly Dictionary<MyDefinitionId, bool> m_typeIdToConveyorConnectionRequired = new Dictionary<MyDefinitionId, bool>(MyDefinitionId.Comparer);

		private readonly HashSet<MyDefinitionId> m_typesToRemove = new HashSet<MyDefinitionId>((IEqualityComparer<MyDefinitionId>)MyDefinitionId.Comparer);

		private readonly MyConcurrentHashSet<MyResourceSinkComponent> m_sinksToAdd = new MyConcurrentHashSet<MyResourceSinkComponent>();

		private readonly MyConcurrentHashSet<MyTuple<MyResourceSinkComponent, bool>> m_sinksToRemove = new MyConcurrentHashSet<MyTuple<MyResourceSinkComponent, bool>>();

		private readonly MyConcurrentHashSet<MyResourceSourceComponent> m_sourcesToAdd = new MyConcurrentHashSet<MyResourceSourceComponent>();

		private readonly MyConcurrentHashSet<MyResourceSourceComponent> m_sourcesToRemove = new MyConcurrentHashSet<MyResourceSourceComponent>();

		private readonly MyConcurrentDictionary<MyDefinitionId, int> m_changedTypes = new MyConcurrentDictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);

		private readonly List<string> m_changesDebug = new List<string>();

		/// <summary>
		/// For debugging purposes. Enables trace messages and watches for this instance.
		/// </summary>
		public static bool ShowTrace = false;

		public string DebugName;

		private static int m_typeGroupCountTotal = -1;

		private static int m_sinkGroupPrioritiesTotal = -1;

		private static int m_sourceGroupPrioritiesTotal = -1;

		private static readonly Dictionary<MyDefinitionId, int> m_typeIdToIndexTotal = new Dictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);

		private static readonly Dictionary<MyDefinitionId, bool> m_typeIdToConveyorConnectionRequiredTotal = new Dictionary<MyDefinitionId, bool>(MyDefinitionId.Comparer);

		private static readonly Dictionary<MyStringHash, int> m_sourceSubtypeToPriority = new Dictionary<MyStringHash, int>(MyStringHash.Comparer);

		private static readonly Dictionary<MyStringHash, int> m_sinkSubtypeToPriority = new Dictionary<MyStringHash, int>(MyStringHash.Comparer);

		private static readonly Dictionary<MyStringHash, bool> m_sinkSubtypeToAdaptability = new Dictionary<MyStringHash, bool>(MyStringHash.Comparer);

		private MyResourceStateEnum m_electricityState;

		private bool m_updateInProgress;

		private bool m_recomputeInProgress;

		public MyMultipleEnabledEnum SourcesEnabled => SourcesEnabledByType(Enumerable.First<MyDefinitionId>((IEnumerable<MyDefinitionId>)m_typeIdToIndexTotal.Keys));

		public MyResourceStateEnum ResourceState => ResourceStateByType(Enumerable.First<MyDefinitionId>((IEnumerable<MyDefinitionId>)m_typeIdToIndexTotal.Keys));

		public static int SinkGroupPrioritiesTotal => m_sinkGroupPrioritiesTotal;

		public static DictionaryReader<MyStringHash, int> SinkSubtypesToPriority => new DictionaryReader<MyStringHash, int>(m_sinkSubtypeToPriority);

		public override string ComponentTypeDebugString => "Resource Distributor";

<<<<<<< HEAD
		public event Action<bool> OnPowerGenerationChanged;

		/// <summary>
		/// Event raised when any element of this system is changed, requiring the system to update.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public event Action SystemChanged;

		internal static void InitializeMappings()
		{
			lock (m_typeIdToIndexTotal)
			{
				if (m_sinkGroupPrioritiesTotal >= 0 || m_sourceGroupPrioritiesTotal >= 0)
				{
					return;
				}
				ListReader<MyResourceDistributionGroupDefinition> definitionsOfType = MyDefinitionManager.Static.GetDefinitionsOfType<MyResourceDistributionGroupDefinition>();
<<<<<<< HEAD
				IOrderedEnumerable<MyResourceDistributionGroupDefinition> orderedEnumerable = definitionsOfType.OrderBy((MyResourceDistributionGroupDefinition def) => def.Priority);
=======
				IOrderedEnumerable<MyResourceDistributionGroupDefinition> obj = Enumerable.OrderBy<MyResourceDistributionGroupDefinition, int>((IEnumerable<MyResourceDistributionGroupDefinition>)definitionsOfType, (Func<MyResourceDistributionGroupDefinition, int>)((MyResourceDistributionGroupDefinition def) => def.Priority));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (definitionsOfType.Count > 0)
				{
					m_sinkGroupPrioritiesTotal = 0;
					m_sourceGroupPrioritiesTotal = 0;
				}
<<<<<<< HEAD
				foreach (MyResourceDistributionGroupDefinition item in orderedEnumerable)
=======
				foreach (MyResourceDistributionGroupDefinition item in (IEnumerable<MyResourceDistributionGroupDefinition>)obj)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					if (!item.IsSource)
					{
						m_sinkSubtypeToPriority.Add(item.Id.SubtypeId, m_sinkGroupPrioritiesTotal++);
						m_sinkSubtypeToAdaptability.Add(item.Id.SubtypeId, item.IsAdaptible);
					}
					else
					{
						m_sourceSubtypeToPriority.Add(item.Id.SubtypeId, m_sourceGroupPrioritiesTotal++);
					}
				}
				m_sinkGroupPrioritiesTotal = Math.Max(m_sinkGroupPrioritiesTotal, 1);
				m_sourceGroupPrioritiesTotal = Math.Max(m_sourceGroupPrioritiesTotal, 1);
				m_sinkSubtypeToPriority.Add(MyStringHash.NullOrEmpty, m_sinkGroupPrioritiesTotal - 1);
				m_sinkSubtypeToAdaptability.Add(MyStringHash.NullOrEmpty, value: false);
				m_sourceSubtypeToPriority.Add(MyStringHash.NullOrEmpty, m_sourceGroupPrioritiesTotal - 1);
				m_typeGroupCountTotal = 0;
				m_typeIdToIndexTotal.Add(ElectricityId, m_typeGroupCountTotal++);
				m_typeIdToConveyorConnectionRequiredTotal.Add(ElectricityId, value: false);
				foreach (MyGasProperties item2 in MyDefinitionManager.Static.GetDefinitionsOfType<MyGasProperties>())
				{
					m_typeIdToIndexTotal.Add(item2.Id, m_typeGroupCountTotal++);
					m_typeIdToConveyorConnectionRequiredTotal.Add(item2.Id, value: true);
				}
			}
		}

		public static void Clear()
		{
			m_typeGroupCountTotal = -1;
			m_sinkGroupPrioritiesTotal = -1;
			m_sourceGroupPrioritiesTotal = -1;
			m_typeIdToIndexTotal.Clear();
			m_typeIdToConveyorConnectionRequiredTotal.Clear();
			m_sourceSubtypeToPriority.Clear();
			m_sinkSubtypeToPriority.Clear();
			m_sinkSubtypeToAdaptability.Clear();
		}

		private void InitializeNewType(ref MyDefinitionId typeId)
		{
			m_typeIdToIndex.Add(typeId, m_typeGroupCount++);
			m_typeIdToConveyorConnectionRequired.Add(typeId, IsConveyorConnectionRequiredTotal(ref typeId));
			HashSet<MyResourceSinkComponent>[] array = new HashSet<MyResourceSinkComponent>[m_sinkGroupPrioritiesTotal];
			HashSet<MyResourceSourceComponent>[] array2 = new HashSet<MyResourceSourceComponent>[m_sourceGroupPrioritiesTotal];
			List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> inputOutputList = new List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new HashSet<MyResourceSinkComponent>();
			}
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = new HashSet<MyResourceSourceComponent>();
			}
			List<MyPhysicalDistributionGroup> physicalDistributionGroups = null;
			int distributionGroupsInUse = 0;
			MySinkGroupData[] sinkDataByPriority = null;
			MySourceGroupData[] sourceDataByPriority = null;
			MyList<int> stockpilingStorageIndices = null;
			MyList<int> otherStorageIndices = null;
			if (IsConveyorConnectionRequired(ref typeId))
			{
				physicalDistributionGroups = new List<MyPhysicalDistributionGroup>();
			}
			else
			{
				sinkDataByPriority = new MySinkGroupData[m_sinkGroupPrioritiesTotal];
				sourceDataByPriority = new MySourceGroupData[m_sourceGroupPrioritiesTotal];
				stockpilingStorageIndices = new MyList<int>();
				otherStorageIndices = new MyList<int>();
			}
			m_dataPerType.Add(new PerTypeData
			{
				TypeId = typeId,
				SinkDataByPriority = sinkDataByPriority,
				SourceDataByPriority = sourceDataByPriority,
				InputOutputData = default(MyTuple<MySinkGroupData, MySourceGroupData>),
				SinksByPriority = array,
				SourcesByPriority = array2,
				InputOutputList = inputOutputList,
				StockpilingStorageIndices = stockpilingStorageIndices,
				OtherStorageIndices = otherStorageIndices,
				PhysicalDistributionGroups = physicalDistributionGroups,
				ElectricalDistributionGroups = new List<MyElectricalDistributionGroup>(),
				DistributionGroupsInUse = distributionGroupsInUse,
				NeedsRecompute = false,
				GroupsDirty = true,
				SourceCount = 0,
				RemainingFuelTime = 0f,
				RemainingFuelTimeDirty = true,
				MaxAvailableResource = 0f,
				SourcesEnabled = MyMultipleEnabledEnum.NoObjects,
				SourcesEnabledDirty = true,
				ResourceState = MyResourceStateEnum.NoPower
			});
			m_initializedTypes.Add(typeId);
			RaiseSystemChanged();
		}

		public MyResourceDistributorComponent(string debugName)
		{
			InitializeMappings();
			DebugName = debugName;
			m_changesDebug.Clear();
		}

		public void MarkForUpdate()
		{
			m_forceRecalculation = true;
		}

		private void RemoveTypesFromChanges(ListReader<MyDefinitionId> types)
		{
			foreach (MyDefinitionId item in types)
			{
				if (m_changedTypes.TryGetValue(item, out var value))
				{
					m_changedTypes[item] = Math.Max(0, value - 1);
				}
			}
		}

		public void AddSink(MyResourceSinkComponent sink)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
			{
				m_changesDebug.Add($"+Sink: {((sink.Entity != null) ? sink.Entity.ToString() : sink.Group.ToString())}");
			}
			MyTuple<MyResourceSinkComponent, bool> value = default(MyTuple<MyResourceSinkComponent, bool>);
			lock (m_sinksToRemove)
			{
				foreach (MyTuple<MyResourceSinkComponent, bool> item in m_sinksToRemove)
				{
					if (item.Item1 == sink)
					{
						value = item;
						break;
					}
				}
				if (value.Item1 != null)
				{
					m_sinksToRemove.Remove(value);
					RemoveTypesFromChanges(value.Item1.AcceptedResources);
					return;
				}
			}
			lock (m_sinksToAdd)
			{
				m_sinksToAdd.Add(sink);
				foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
				{
					MyDefinitionId typeId = acceptedResource;
					if (m_initializedTypes.Contains(typeId))
					{
						GetSinksOfType(ref typeId, sink.Group);
					}
				}
			}
			foreach (MyDefinitionId acceptedResource2 in sink.AcceptedResources)
			{
				if (!m_changedTypes.TryGetValue(acceptedResource2, out var value2))
				{
					m_changedTypes.Add(acceptedResource2, 1);
				}
				else
				{
					m_changedTypes[acceptedResource2] = value2 + 1;
				}
			}
			RaiseSystemChanged();
		}

		public void RemoveSink(MyResourceSinkComponent sink, bool resetSinkInput = true, bool markedForClose = false)
		{
			if (markedForClose)
			{
				return;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
			{
				m_changesDebug.Add($"-Sink: {((sink.Entity != null) ? sink.Entity.ToString() : sink.Group.ToString())}");
			}
			lock (m_sinksToAdd)
			{
				if (m_sinksToAdd.Contains(sink))
				{
					m_sinksToAdd.Remove(sink);
					RemoveTypesFromChanges(sink.AcceptedResources);
					return;
				}
			}
			lock (m_sinksToRemove)
			{
				m_sinksToRemove.Add(MyTuple.Create(sink, resetSinkInput));
			}
			foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
			{
				if (!m_changedTypes.TryGetValue(acceptedResource, out var value))
				{
					m_changedTypes.Add(acceptedResource, 1);
				}
				else
				{
					m_changedTypes[acceptedResource] = value + 1;
				}
			}
			RaiseSystemChanged();
		}

		public void AddSource(MyResourceSourceComponent source)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
			{
				m_changesDebug.Add($"+Source: {((source.Entity != null) ? source.Entity.ToString() : source.Group.ToString())}");
			}
			lock (m_sourcesToRemove)
			{
				if (m_sourcesToRemove.Contains(source))
				{
					m_sourcesToRemove.Remove(source);
					RemoveTypesFromChanges(source.ResourceTypes);
					return;
				}
			}
			lock (m_sourcesToAdd)
			{
				m_sourcesToAdd.Add(source);
			}
			foreach (MyDefinitionId resourceType in source.ResourceTypes)
			{
				if (!m_changedTypes.TryGetValue(resourceType, out var value))
				{
					m_changedTypes.Add(resourceType, 1);
				}
				else
				{
					m_changedTypes[resourceType] = value + 1;
				}
			}
			RaiseSystemChanged();
		}

		public void RemoveSource(MyResourceSourceComponent source)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
			{
				m_changesDebug.Add($"-Source: {((source.Entity != null) ? source.Entity.ToString() : source.Group.ToString())}");
			}
			lock (m_sourcesToAdd)
			{
				if (m_sourcesToAdd.Contains(source))
				{
					m_sourcesToAdd.Remove(source);
					RemoveTypesFromChanges(source.ResourceTypes);
					return;
				}
			}
			lock (m_sourcesToRemove)
			{
				m_sourcesToRemove.Add(source);
			}
			foreach (MyDefinitionId resourceType in source.ResourceTypes)
			{
				if (!m_changedTypes.TryGetValue(resourceType, out var value))
				{
					m_changedTypes.Add(resourceType, 1);
				}
				else
				{
					m_changedTypes[resourceType] = value + 1;
				}
			}
			RaiseSystemChanged();
		}

		private void AddSinkLazy(MyResourceSinkComponent sink)
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
			{
				MyDefinitionId typeId = acceptedResource;
				if (!m_initializedTypes.Contains(typeId))
				{
					InitializeNewType(ref typeId);
				}
				int typeIndex = GetTypeIndex(ref typeId);
				PerTypeData perTypeData = m_dataPerType[typeIndex];
				HashSet<MyResourceSinkComponent> sinksOfType = GetSinksOfType(ref typeId, sink.Group);
				if (sinksOfType == null)
				{
					continue;
				}
<<<<<<< HEAD
=======
				int typeIndex = GetTypeIndex(ref typeId);
				PerTypeData perTypeData = m_dataPerType[typeIndex];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyResourceSourceComponent myResourceSourceComponent = null;
				if (sink.Container != null)
				{
					HashSet<MyResourceSourceComponent>[] sourcesByPriority = perTypeData.SourcesByPriority;
<<<<<<< HEAD
					foreach (HashSet<MyResourceSourceComponent> hashSet in sourcesByPriority)
					{
						foreach (MyResourceSourceComponent item in hashSet)
						{
							if (item.Container != null && item.Container.Get<MyResourceSinkComponent>() == sink)
							{
								perTypeData.InputOutputList.Add(MyTuple.Create(sink, item));
								myResourceSourceComponent = item;
								break;
							}
=======
					foreach (HashSet<MyResourceSourceComponent> val in sourcesByPriority)
					{
						Enumerator<MyResourceSourceComponent> enumerator2 = val.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								MyResourceSourceComponent current = enumerator2.get_Current();
								if (current.Container != null && current.Container.Get<MyResourceSinkComponent>() == sink)
								{
									perTypeData.InputOutputList.Add(MyTuple.Create(sink, current));
									myResourceSourceComponent = current;
									break;
								}
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
						if (myResourceSourceComponent != null)
						{
							val.Remove(myResourceSourceComponent);
							perTypeData.InvalidateGridForUpdateCache();
							break;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						if (myResourceSourceComponent != null)
						{
							hashSet.Remove(myResourceSourceComponent);
							perTypeData.InvalidateGridForUpdateCache();
							break;
						}
					}
				}
				if (myResourceSourceComponent == null)
				{
					sinksOfType.Add(sink);
					perTypeData.InvalidateGridForUpdateCache();
				}
				perTypeData.NeedsRecompute = true;
				perTypeData.GroupsDirty = true;
				perTypeData.RemainingFuelTimeDirty = true;
			}
			sink.RequiredInputChanged += Sink_RequiredInputChanged;
			sink.ResourceAvailable += Sink_IsResourceAvailable;
			sink.OnAddType += Sink_OnAddType;
			sink.OnRemoveType += Sink_OnRemoveType;
		}

		private void RemoveSinkLazy(MyResourceSinkComponent sink, bool resetSinkInput = true)
		{
			foreach (MyDefinitionId acceptedResource in sink.AcceptedResources)
			{
				MyDefinitionId typeId = acceptedResource;
				HashSet<MyResourceSinkComponent> sinksOfType = GetSinksOfType(ref typeId, sink.Group);
				if (sinksOfType == null)
				{
					continue;
				}
				int typeIndex = GetTypeIndex(ref typeId);
				PerTypeData perTypeData = m_dataPerType[typeIndex];
<<<<<<< HEAD
				if (typeId == ElectricityId)
				{
					int electricalGroupIndex = GetElectricalGroupIndex(ref typeId, sink.Grid);
					if (!TryGetTypeIndex(typeId, out typeIndex) || !m_sinkSubtypeToPriority.TryGetValue(sink.Group, out var value))
					{
						continue;
					}
					int num = -1;
					for (int i = 0; i < m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex].InputOutputList.Count; i++)
					{
						if (m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex].InputOutputList[i].Item1 == sink)
						{
							num = i;
							break;
						}
					}
					if (num != -1)
					{
						MyResourceSourceComponent item = m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex].InputOutputList[num].Item2;
						m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex].InputOutputList.RemoveAtFast(num);
						m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex].SourcesByPriority[GetPriority(item)].Add(item);
						m_dataPerType[typeIndex].InvalidateGridForUpdateCache();
					}
					m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex].SinksByPriority[value].Remove(sink);
				}
				if (!sinksOfType.Remove(sink))
				{
					int num2 = -1;
					for (int j = 0; j < perTypeData.InputOutputList.Count; j++)
					{
						if (perTypeData.InputOutputList[j].Item1 == sink)
						{
							num2 = j;
							break;
						}
					}
					if (num2 != -1)
					{
						MyResourceSourceComponent item2 = perTypeData.InputOutputList[num2].Item2;
						perTypeData.InputOutputList.RemoveAtFast(num2);
						perTypeData.SourcesByPriority[GetPriority(item2)].Add(item2);
=======
				if (!sinksOfType.Remove(sink))
				{
					int num = -1;
					for (int i = 0; i < perTypeData.InputOutputList.Count; i++)
					{
						if (perTypeData.InputOutputList[i].Item1 == sink)
						{
							num = i;
							break;
						}
					}
					if (num != -1)
					{
						MyResourceSourceComponent item = perTypeData.InputOutputList[num].Item2;
						perTypeData.InputOutputList.RemoveAtFast(num);
						perTypeData.SourcesByPriority[GetPriority(item)].Add(item);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						perTypeData.InvalidateGridForUpdateCache();
					}
				}
				else
				{
					perTypeData.InvalidateGridForUpdateCache();
				}
				perTypeData.NeedsRecompute = true;
				perTypeData.GroupsDirty = true;
				perTypeData.RemainingFuelTimeDirty = true;
				if (resetSinkInput)
				{
					sink.SetInputFromDistributor(typeId, 0f, IsAdaptible(sink));
				}
			}
			sink.OnRemoveType -= Sink_OnRemoveType;
			sink.OnAddType -= Sink_OnAddType;
			sink.RequiredInputChanged -= Sink_RequiredInputChanged;
			sink.ResourceAvailable -= Sink_IsResourceAvailable;
		}

		private void AddSourceLazy(MyResourceSourceComponent source)
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			foreach (MyDefinitionId resourceType in source.ResourceTypes)
			{
				MyDefinitionId typeId = resourceType;
				if (!m_initializedTypes.Contains(typeId))
				{
					InitializeNewType(ref typeId);
				}
				HashSet<MyResourceSourceComponent> sourcesOfType = GetSourcesOfType(ref typeId, source.Group);
				if (sourcesOfType == null)
				{
					continue;
				}
				int typeIndex = GetTypeIndex(ref typeId);
				PerTypeData perTypeData = m_dataPerType[typeIndex];
				MyResourceSinkComponent myResourceSinkComponent = null;
				if (source.Container != null)
				{
					HashSet<MyResourceSinkComponent>[] sinksByPriority = perTypeData.SinksByPriority;
<<<<<<< HEAD
					foreach (HashSet<MyResourceSinkComponent> hashSet in sinksByPriority)
					{
						foreach (MyResourceSinkComponent item in hashSet)
						{
							if (item.Container != null && item.Container.Get<MyResourceSourceComponent>() == source)
							{
								perTypeData.InputOutputList.Add(MyTuple.Create(item, source));
								myResourceSinkComponent = item;
								break;
							}
=======
					foreach (HashSet<MyResourceSinkComponent> val in sinksByPriority)
					{
						Enumerator<MyResourceSinkComponent> enumerator2 = val.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								MyResourceSinkComponent current = enumerator2.get_Current();
								if (current.Container != null && current.Container.Get<MyResourceSourceComponent>() == source)
								{
									perTypeData.InputOutputList.Add(MyTuple.Create(current, source));
									myResourceSinkComponent = current;
									break;
								}
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
						if (myResourceSinkComponent != null)
						{
							val.Remove(myResourceSinkComponent);
							perTypeData.InvalidateGridForUpdateCache();
							break;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						if (myResourceSinkComponent != null)
						{
							hashSet.Remove(myResourceSinkComponent);
							perTypeData.InvalidateGridForUpdateCache();
							break;
						}
					}
				}
				if (myResourceSinkComponent == null)
				{
					sourcesOfType.Add(source);
					perTypeData.InvalidateGridForUpdateCache();
				}
				perTypeData.NeedsRecompute = true;
				perTypeData.GroupsDirty = true;
				perTypeData.SourceCount++;
				if (perTypeData.SourceCount == 1)
				{
					perTypeData.SourcesEnabled = ((!source.Enabled) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
				}
				else if ((perTypeData.SourcesEnabled == MyMultipleEnabledEnum.AllEnabled && !source.Enabled) || (perTypeData.SourcesEnabled == MyMultipleEnabledEnum.AllDisabled && source.Enabled))
				{
					perTypeData.SourcesEnabled = MyMultipleEnabledEnum.Mixed;
				}
				perTypeData.RemainingFuelTimeDirty = true;
			}
			source.HasCapacityRemainingChanged += source_HasRemainingCapacityChanged;
			source.MaxOutputChanged += source_MaxOutputChanged;
			source.ProductionEnabledChanged += source_ProductionEnabledChanged;
		}

		private void RemoveSourceLazy(MyResourceSourceComponent source)
		{
			foreach (MyDefinitionId resourceType in source.ResourceTypes)
			{
				MyDefinitionId typeId = resourceType;
				HashSet<MyResourceSourceComponent> sourcesOfType = GetSourcesOfType(ref typeId, source.Group);
				if (sourcesOfType == null)
				{
					continue;
				}
				int typeIndex = GetTypeIndex(ref typeId);
				PerTypeData perTypeData = m_dataPerType[typeIndex];
<<<<<<< HEAD
				if (typeId == ElectricityId)
				{
					int electricalGroupIndex = GetElectricalGroupIndex(ref typeId, source.Grid);
					if (!TryGetTypeIndex(typeId, out typeIndex) || !m_sourceSubtypeToPriority.TryGetValue(source.Group, out var _))
					{
						continue;
					}
					int num = -1;
					for (int i = 0; i < m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex].InputOutputList.Count; i++)
					{
						if (m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex].InputOutputList[i].Item2 == source)
						{
							num = i;
							break;
						}
					}
					if (num != -1)
					{
						MyResourceSinkComponent item = m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex].InputOutputList[num].Item1;
						m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex].InputOutputList.RemoveAtFast(num);
						m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex].SinksByPriority[GetPriority(item)].Add(item);
						m_dataPerType[typeIndex].InvalidateGridForUpdateCache();
					}
				}
				if (!sourcesOfType.Remove(source))
				{
					int num2 = -1;
					for (int j = 0; j < perTypeData.InputOutputList.Count; j++)
					{
						if (perTypeData.InputOutputList[j].Item2 == source)
						{
							num2 = j;
							break;
						}
					}
					if (num2 != -1)
					{
						MyResourceSinkComponent item2 = perTypeData.InputOutputList[num2].Item1;
						perTypeData.InputOutputList.RemoveAtFast(num2);
						perTypeData.SinksByPriority[GetPriority(item2)].Add(item2);
=======
				if (!sourcesOfType.Remove(source))
				{
					int num = -1;
					for (int i = 0; i < perTypeData.InputOutputList.Count; i++)
					{
						if (perTypeData.InputOutputList[i].Item2 == source)
						{
							num = i;
							break;
						}
					}
					if (num != -1)
					{
						MyResourceSinkComponent item = perTypeData.InputOutputList[num].Item1;
						perTypeData.InputOutputList.RemoveAtFast(num);
						perTypeData.SinksByPriority[GetPriority(item)].Add(item);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						perTypeData.InvalidateGridForUpdateCache();
					}
				}
				else
				{
					perTypeData.InvalidateGridForUpdateCache();
				}
				perTypeData.NeedsRecompute = true;
				perTypeData.GroupsDirty = true;
				perTypeData.SourceCount--;
				if (perTypeData.SourceCount == 0)
				{
					perTypeData.SourcesEnabled = MyMultipleEnabledEnum.NoObjects;
				}
				else if (perTypeData.SourceCount == 1)
				{
					MyResourceSourceComponent firstSourceOfType = GetFirstSourceOfType(ref typeId);
					if (firstSourceOfType != null)
					{
						ChangeSourcesState(typeId, (!firstSourceOfType.Enabled) ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled, MySession.Static.LocalPlayerId);
					}
					else
					{
						perTypeData.SourceCount--;
						perTypeData.SourcesEnabled = MyMultipleEnabledEnum.NoObjects;
					}
				}
				else if (perTypeData.SourcesEnabled == MyMultipleEnabledEnum.Mixed)
				{
					perTypeData.SourcesEnabledDirty = true;
				}
				perTypeData.RemainingFuelTimeDirty = true;
			}
			source.ProductionEnabledChanged -= source_ProductionEnabledChanged;
			source.MaxOutputChanged -= source_MaxOutputChanged;
			source.HasCapacityRemainingChanged -= source_HasRemainingCapacityChanged;
		}

		public int GetSourceCount(MyDefinitionId resourceTypeId, MyStringHash sourceGroupType)
		{
			int num = 0;
			if (!TryGetTypeIndex(ref resourceTypeId, out var typeIndex))
			{
				return 0;
			}
			int num2 = m_sourceSubtypeToPriority[sourceGroupType];
			foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in m_dataPerType[typeIndex].InputOutputList)
			{
				if (inputOutput.Item2.Group == sourceGroupType && inputOutput.Item2.CurrentOutputByType(resourceTypeId) > 0f)
				{
					num++;
				}
			}
			return m_dataPerType[typeIndex].SourceDataByPriority[num2].ActiveCount + num;
		}

		public void UpdateBeforeSimulation()
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			CheckDistributionSystemChanges();
			foreach (MyDefinitionId key in m_typeIdToIndex.Keys)
			{
				MyDefinitionId typeId = key;
				if (m_forceRecalculation || NeedsRecompute(ref typeId))
				{
					RecomputeResourceDistribution(ref typeId, updateChanges: false);
				}
			}
			m_forceRecalculation = false;
			Enumerator<MyDefinitionId> enumerator2 = m_typesToRemove.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					MyDefinitionId typeId2 = enumerator2.get_Current();
					RemoveType(ref typeId2);
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			_ = ShowTrace;
		}

		public virtual void UpdateBeforeSimulation100()
		{
			MyResourceStateEnum myResourceStateEnum = ResourceStateByType(ElectricityId);
			if (m_electricityState != myResourceStateEnum)
			{
				if (PowerStateIsOk(myResourceStateEnum) != PowerStateIsOk(m_electricityState))
				{
					ConveyorSystem_OnPoweredChanged();
				}
				bool flag = PowerStateWorks(myResourceStateEnum);
				if (this.OnPowerGenerationChanged != null && flag != PowerStateWorks(m_electricityState))
				{
					this.OnPowerGenerationChanged(flag);
				}
				ConveyorSystem_OnPoweredChanged();
				m_electricityState = myResourceStateEnum;
			}
		}

		protected bool PowerStateIsOk(MyResourceStateEnum state)
		{
			return state == MyResourceStateEnum.Ok;
		}

		protected bool PowerStateWorks(MyResourceStateEnum state)
		{
			return state != MyResourceStateEnum.NoPower;
		}

		/// <summary>
		/// Computes number of groups that have enough energy to work.
		/// </summary>
		public void UpdateHud(MyHudSinkGroupInfo info)
		{
			bool flag = true;
			int num = 0;
			int i = 0;
<<<<<<< HEAD
			MyCubeBlock myCubeBlock;
			if (!TryGetTypeIndex(ElectricityId, out var typeIndex) || (myCubeBlock = MySession.Static.ControlledEntity as MyCubeBlock) == null || myCubeBlock.CubeGrid == null)
=======
			if (!TryGetTypeIndex(ElectricityId, out var typeIndex))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			MyCubeGrid cubeGrid = myCubeBlock.CubeGrid;
			MyDefinitionId typeId = ElectricityId;
			int electricalGroupIndex = GetElectricalGroupIndex(ref typeId, cubeGrid);
			for (MyElectricalDistributionGroup myElectricalDistributionGroup = m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex]; i < myElectricalDistributionGroup.SinkDataByPriority.Length; i++)
			{
				if (flag && myElectricalDistributionGroup.SinkDataByPriority[i].RemainingAvailableResource < myElectricalDistributionGroup.SinkDataByPriority[i].RequiredInput && !myElectricalDistributionGroup.SinkDataByPriority[i].IsAdaptible)
				{
					flag = false;
				}
				if (flag)
				{
					num++;
				}
				info.SetGroupDeficit(i, Math.Max(myElectricalDistributionGroup.SinkDataByPriority[i].RequiredInput - myElectricalDistributionGroup.SinkDataByPriority[i].RemainingAvailableResource, 0f));
			}
			info.WorkingGroupCount = num;
		}

		public void ChangeSourcesState(MyDefinitionId resourceTypeId, MyMultipleEnabledEnum state, long playerId, MyCubeGrid onlyForThisGrid = null)
		{
<<<<<<< HEAD
=======
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (m_recomputeInProgress || !TryGetTypeIndex(resourceTypeId, out var typeIndex) || m_dataPerType[typeIndex].SourcesEnabled == state || m_dataPerType[typeIndex].SourcesEnabled == MyMultipleEnabledEnum.NoObjects)
			{
				return;
			}
			m_recomputeInProgress = true;
			m_dataPerType[typeIndex].SourcesEnabled = state;
			bool newValue = state == MyMultipleEnabledEnum.AllEnabled;
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(playerId);
			bool flag = MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals);
			if (MySession.Static.RemoteAdminSettings.TryGetValue(MySession.Static.Players.TryGetSteamId(playerId), out var value))
			{
				flag |= value.HasFlag(AdminSettingsEnum.UseTerminals);
			}
			HashSet<MyResourceSourceComponent>[] sourcesByPriority = m_dataPerType[typeIndex].SourcesByPriority;
			for (int i = 0; i < sourcesByPriority.Length; i++)
			{
<<<<<<< HEAD
				foreach (MyResourceSourceComponent item2 in sourcesByPriority[i])
				{
					MyCubeBlock myCubeBlock;
					if (onlyForThisGrid != null && (myCubeBlock = item2.Entity as MyCubeBlock) != null && myCubeBlock.CubeGrid?.EntityId != onlyForThisGrid.EntityId)
					{
						continue;
					}
					if (!flag && playerId >= 0 && item2.Entity != null)
					{
						MyFunctionalBlock myFunctionalBlock = item2.Entity as MyFunctionalBlock;
						if (myFunctionalBlock != null && myFunctionalBlock.OwnerId != 0L && myFunctionalBlock.OwnerId != playerId)
						{
							MyOwnershipShareModeEnum shareMode = myFunctionalBlock.IDModule.ShareMode;
							IMyFaction myFaction2 = MySession.Static.Factions.TryGetPlayerFaction(myFunctionalBlock.OwnerId);
							if (shareMode == MyOwnershipShareModeEnum.None || (shareMode == MyOwnershipShareModeEnum.Faction && (myFaction == null || (myFaction2 != null && myFaction.FactionId != myFaction2.FactionId))))
=======
				Enumerator<MyResourceSourceComponent> enumerator = sourcesByPriority[i].GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyResourceSourceComponent current = enumerator.get_Current();
						if (!flag && playerId >= 0 && current.Entity != null)
						{
							MyFunctionalBlock myFunctionalBlock = current.Entity as MyFunctionalBlock;
							if (myFunctionalBlock != null && myFunctionalBlock.OwnerId != 0L && myFunctionalBlock.OwnerId != playerId)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								continue;
							}
						}
					}
					if (playerId == -2)
					{
						MyTerminalBlock myTerminalBlock = item2.Container.Entity as MyTerminalBlock;
						if (myTerminalBlock != null)
						{
<<<<<<< HEAD
							string text = myTerminalBlock.CustomName.ToString();
							if (text != "Special Content" && text != "Special Content Power")
							{
								continue;
							}
						}
=======
							MyTerminalBlock myTerminalBlock = current.Container.Entity as MyTerminalBlock;
							if (myTerminalBlock != null)
							{
								string text = myTerminalBlock.CustomName.ToString();
								if (text != "Special Content" && text != "Special Content Power")
								{
									continue;
								}
							}
						}
						current.MaxOutputChanged -= source_MaxOutputChanged;
						current.SetEnabled(newValue, fireEvents: false);
						current.MaxOutputChanged += source_MaxOutputChanged;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					item2.MaxOutputChanged -= source_MaxOutputChanged;
					item2.SetEnabled(newValue, fireEvents: false);
					item2.MaxOutputChanged += source_MaxOutputChanged;
				}
			}
			foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in m_dataPerType[typeIndex].InputOutputList)
			{
				MyResourceSourceComponent item = inputOutput.Item2;
				MyCubeBlock myCubeBlock2;
				if (onlyForThisGrid != null && (myCubeBlock2 = item.Entity as MyCubeBlock) != null && myCubeBlock2.CubeGrid?.EntityId != onlyForThisGrid.EntityId)
				{
					continue;
				}
<<<<<<< HEAD
				if (!flag && playerId >= 0 && item.Entity != null)
				{
=======
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in m_dataPerType[typeIndex].InputOutputList)
			{
				MyResourceSourceComponent item = inputOutput.Item2;
				if (!flag && playerId >= 0 && item.Entity != null)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyFunctionalBlock myFunctionalBlock2 = item.Entity as MyFunctionalBlock;
					if (myFunctionalBlock2 != null && myFunctionalBlock2.OwnerId != 0L && myFunctionalBlock2.OwnerId != playerId)
					{
						MyOwnershipShareModeEnum shareMode2 = myFunctionalBlock2.IDModule.ShareMode;
						IMyFaction myFaction3 = MySession.Static.Factions.TryGetPlayerFaction(myFunctionalBlock2.OwnerId);
						if (shareMode2 == MyOwnershipShareModeEnum.None || (shareMode2 == MyOwnershipShareModeEnum.Faction && (myFaction == null || (myFaction3 != null && myFaction.FactionId != myFaction3.FactionId))))
						{
							continue;
						}
					}
				}
				item.MaxOutputChanged -= source_MaxOutputChanged;
				item.SetEnabled(newValue, fireEvents: false);
				item.MaxOutputChanged += source_MaxOutputChanged;
			}
			m_dataPerType[typeIndex].SourcesEnabledDirty = false;
			m_dataPerType[typeIndex].NeedsRecompute = true;
			m_recomputeInProgress = false;
		}

		private float ComputeRemainingFuelTime(MyDefinitionId resourceTypeId)
		{
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				int typeIndex = GetTypeIndex(ref resourceTypeId);
				if (m_dataPerType[typeIndex].MaxAvailableResource == 0f)
				{
					return 0f;
				}
				float num = 0f;
				MySinkGroupData[] sinkDataByPriority = m_dataPerType[typeIndex].SinkDataByPriority;
				for (int i = 0; i < sinkDataByPriority.Length; i++)
				{
					MySinkGroupData mySinkGroupData = sinkDataByPriority[i];
					if (mySinkGroupData.RemainingAvailableResource >= mySinkGroupData.RequiredInput)
					{
						num += mySinkGroupData.RequiredInput;
						continue;
					}
					if (!mySinkGroupData.IsAdaptible)
					{
						break;
					}
					num += mySinkGroupData.RemainingAvailableResource;
				}
				num = ((!(m_dataPerType[typeIndex].InputOutputData.Item1.RemainingAvailableResource > m_dataPerType[typeIndex].InputOutputData.Item1.RequiredInput)) ? (num + m_dataPerType[typeIndex].InputOutputData.Item1.RemainingAvailableResource) : (num + m_dataPerType[typeIndex].InputOutputData.Item1.RequiredInput));
				bool flag = false;
				bool flag2 = false;
				float num2 = 0f;
				for (int j = 0; j < m_dataPerType[typeIndex].SourcesByPriority.Length; j++)
				{
					MySourceGroupData mySourceGroupData = m_dataPerType[typeIndex].SourceDataByPriority[j];
					if (mySourceGroupData.UsageRatio <= 0f)
					{
						continue;
					}
					if (mySourceGroupData.InfiniteCapacity)
					{
						flag = true;
						num -= mySourceGroupData.UsageRatio * mySourceGroupData.MaxAvailableResource;
						continue;
					}
<<<<<<< HEAD
					foreach (MyResourceSourceComponent item in m_dataPerType[typeIndex].SourcesByPriority[j])
					{
						if (item.Enabled && item.ProductionEnabledByType(resourceTypeId) && item.CountTowardsRemainingEnergyTime)
						{
							flag2 = true;
							num2 += item.RemainingCapacityByType(resourceTypeId);
=======
					Enumerator<MyResourceSourceComponent> enumerator = m_dataPerType[typeIndex].SourcesByPriority[j].GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							MyResourceSourceComponent current = enumerator.get_Current();
							if (current.Enabled && current.ProductionEnabledByType(resourceTypeId) && current.CountTowardsRemainingEnergyTime)
							{
								flag2 = true;
								num2 += current.RemainingCapacityByType(resourceTypeId);
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				if (m_dataPerType[typeIndex].InputOutputData.Item2.UsageRatio > 0f)
				{
					foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in m_dataPerType[typeIndex].InputOutputList)
					{
						if (inputOutput.Item2.Enabled && inputOutput.Item2.ProductionEnabledByType(resourceTypeId))
						{
							flag2 = true;
							num2 += inputOutput.Item2.RemainingCapacityByType(resourceTypeId);
						}
					}
				}
				if (flag && !flag2)
				{
					return float.PositiveInfinity;
				}
				if (num > 0f)
				{
					return num2 / num;
				}
				return float.PositiveInfinity;
			}
			finally
			{
			}
		}

		private void RefreshSourcesEnabled(MyDefinitionId resourceTypeId)
		{
<<<<<<< HEAD
=======
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!TryGetTypeIndex(ref resourceTypeId, out var typeIndex))
			{
				return;
			}
			m_dataPerType[typeIndex].SourcesEnabledDirty = false;
			if (m_dataPerType[typeIndex].SourceCount == 0)
			{
				m_dataPerType[typeIndex].SourcesEnabled = MyMultipleEnabledEnum.NoObjects;
				return;
			}
			bool flag = true;
			bool flag2 = true;
			HashSet<MyResourceSourceComponent>[] sourcesByPriority = m_dataPerType[typeIndex].SourcesByPriority;
			for (int i = 0; i < sourcesByPriority.Length; i++)
			{
				Enumerator<MyResourceSourceComponent> enumerator = sourcesByPriority[i].GetEnumerator();
				try
				{
<<<<<<< HEAD
					flag = flag && item.Enabled;
					flag2 = flag2 && !item.Enabled;
					if (!flag && !flag2)
=======
					while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyResourceSourceComponent current = enumerator.get_Current();
						flag = flag && current.Enabled;
						flag2 = flag2 && !current.Enabled;
						if (!flag && !flag2)
						{
							m_dataPerType[typeIndex].SourcesEnabled = MyMultipleEnabledEnum.Mixed;
							return;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in m_dataPerType[typeIndex].InputOutputList)
			{
				flag = flag && inputOutput.Item2.Enabled;
				flag2 = flag2 && !inputOutput.Item2.Enabled;
				if (!flag && !flag2)
				{
					m_dataPerType[typeIndex].SourcesEnabled = MyMultipleEnabledEnum.Mixed;
					return;
				}
			}
			m_dataPerType[typeIndex].SourcesEnabled = (flag2 ? MyMultipleEnabledEnum.AllDisabled : MyMultipleEnabledEnum.AllEnabled);
		}

		internal void CubeGrid_OnFatBlockAddedOrRemoved(MyCubeBlock fatblock)
		{
			IMyConveyorEndpointBlock obj = fatblock as IMyConveyorEndpointBlock;
			IMyConveyorSegmentBlock myConveyorSegmentBlock = fatblock as IMyConveyorSegmentBlock;
			if (obj == null && myConveyorSegmentBlock == null)
			{
				return;
			}
			foreach (PerTypeData item in m_dataPerType)
			{
				item.GroupsDirty = true;
				item.NeedsRecompute = true;
			}
		}

		private void CheckDistributionSystemChanges()
		{
			if (m_sinksToRemove.Count > 0)
			{
				lock (m_sinksToRemove)
				{
					MyTuple<MyResourceSinkComponent, bool>[] array = Enumerable.ToArray<MyTuple<MyResourceSinkComponent, bool>>((IEnumerable<MyTuple<MyResourceSinkComponent, bool>>)m_sinksToRemove);
					for (int i = 0; i < array.Length; i++)
					{
						MyTuple<MyResourceSinkComponent, bool> myTuple = array[i];
						RemoveSinkLazy(myTuple.Item1, myTuple.Item2);
						foreach (MyDefinitionId acceptedResource in myTuple.Item1.AcceptedResources)
						{
							m_changedTypes[acceptedResource] = Math.Max(0, m_changedTypes[acceptedResource] - 1);
						}
					}
					m_sinksToRemove.Clear();
				}
			}
			if (m_sourcesToRemove.Count > 0)
			{
				lock (m_sourcesToRemove)
				{
					foreach (MyResourceSourceComponent item in m_sourcesToRemove)
					{
						RemoveSourceLazy(item);
						foreach (MyDefinitionId resourceType in item.ResourceTypes)
						{
							m_changedTypes[resourceType] = Math.Max(0, m_changedTypes[resourceType] - 1);
						}
					}
					m_sourcesToRemove.Clear();
				}
			}
			if (m_sourcesToAdd.Count > 0)
			{
				lock (m_sourcesToAdd)
				{
					foreach (MyResourceSourceComponent item2 in m_sourcesToAdd)
					{
						AddSourceLazy(item2);
						foreach (MyDefinitionId resourceType2 in item2.ResourceTypes)
						{
							m_changedTypes[resourceType2] = Math.Max(0, m_changedTypes[resourceType2] - 1);
						}
					}
					m_sourcesToAdd.Clear();
				}
			}
			if (m_sinksToAdd.Count <= 0)
<<<<<<< HEAD
			{
				return;
			}
			lock (m_sinksToAdd)
			{
=======
			{
				return;
			}
			lock (m_sinksToAdd)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				foreach (MyResourceSinkComponent item3 in m_sinksToAdd)
				{
					AddSinkLazy(item3);
					foreach (MyDefinitionId acceptedResource2 in item3.AcceptedResources)
					{
						m_changedTypes[acceptedResource2] = Math.Max(0, m_changedTypes[acceptedResource2] - 1);
					}
				}
				m_sinksToAdd.Clear();
			}
		}

		public void RecomputeResourceDistribution(ref MyDefinitionId typeId, bool updateChanges = true)
		{
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			if (m_recomputeInProgress)
			{
				return;
			}
			m_recomputeInProgress = true;
			if (updateChanges && !m_updateInProgress)
			{
				m_updateInProgress = true;
				CheckDistributionSystemChanges();
				m_updateInProgress = false;
			}
			int typeIndex = GetTypeIndex(ref typeId);
<<<<<<< HEAD
			if (m_dataPerType.Count <= typeIndex)
=======
			PerTypeData perTypeData = m_dataPerType[typeIndex];
			if (perTypeData.SinksByPriority.Length == 0 && perTypeData.SourcesByPriority.Length == 0 && perTypeData.InputOutputList.Count == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_typesToRemove.Add(typeId);
				m_recomputeInProgress = false;
				return;
			}
<<<<<<< HEAD
			PerTypeData perTypeData = m_dataPerType[typeIndex];
			if (perTypeData.SinksByPriority.Length == 0 && perTypeData.SourcesByPriority.Length == 0 && perTypeData.InputOutputList.Count == 0)
			{
				m_typesToRemove.Add(typeId);
				m_recomputeInProgress = false;
				return;
=======
			bool flag;
			if (MySession.Static.SimplifiedSimulation)
			{
				flag = false;
				if (PowerStateWorks(m_electricityState) || typeId == ElectricityId)
				{
					foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in perTypeData.InputOutputList)
					{
						MyResourceSourceComponent item = inputOutput.Item2;
						if (!item.Enabled || !(item.MaxOutputByType(typeId) > 0f))
						{
							continue;
						}
						flag = true;
						goto IL_0177;
					}
					HashSet<MyResourceSourceComponent>[] sourcesByPriority = perTypeData.SourcesByPriority;
					for (int i = 0; i < sourcesByPriority.Length; i++)
					{
						Enumerator<MyResourceSourceComponent> enumerator2 = sourcesByPriority[i].GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								MyResourceSourceComponent current = enumerator2.get_Current();
								if (!current.Enabled || !(current.MaxOutputByType(typeId) > 0f))
								{
									continue;
								}
								flag = true;
								goto IL_0177;
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
				}
				goto IL_0177;
			}
			if (!IsConveyorConnectionRequired(ref typeId))
			{
				ComputeInitialDistributionData(ref typeId, perTypeData.SinkDataByPriority, perTypeData.SourceDataByPriority, ref perTypeData.InputOutputData, perTypeData.SinksByPriority, perTypeData.SourcesByPriority, perTypeData.InputOutputList, perTypeData.StockpilingStorageIndices, perTypeData.OtherStorageIndices, out perTypeData.MaxAvailableResource);
				perTypeData.ResourceState = RecomputeResourceDistributionPartial(ref typeId, 0, perTypeData.SinkDataByPriority, perTypeData.SourceDataByPriority, ref perTypeData.InputOutputData, perTypeData.SinksByPriority, perTypeData.SourcesByPriority, perTypeData.InputOutputList, perTypeData.StockpilingStorageIndices, perTypeData.OtherStorageIndices, perTypeData.MaxAvailableResource);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			bool flag;
			if (MySession.Static.SimplifiedSimulation)
			{
				flag = false;
				if (PowerStateWorks(m_electricityState) || typeId == ElectricityId)
				{
					foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in perTypeData.InputOutputList)
					{
						MyResourceSourceComponent item = inputOutput.Item2;
						if (item == null || !item.Enabled || !(item.MaxOutputByType(typeId) > 0f))
						{
							continue;
						}
						flag = true;
						goto IL_01a3;
					}
					HashSet<MyResourceSourceComponent>[] sourcesByPriority = perTypeData.SourcesByPriority;
					for (int i = 0; i < sourcesByPriority.Length; i++)
					{
						foreach (MyResourceSourceComponent item3 in sourcesByPriority[i])
						{
							if (!item3.Enabled || !(item3.MaxOutputByType(typeId) > 0f))
							{
								continue;
							}
							flag = true;
							goto IL_01a3;
						}
					}
				}
				goto IL_01a3;
			}
			if (typeId == ElectricityId)
			{
				if (perTypeData.GroupsDirty)
				{
					perTypeData.GroupsDirty = false;
<<<<<<< HEAD
					RecreateElectricallDistributionGroups(ref typeId, perTypeData.SinksByPriority, perTypeData.SourcesByPriority, perTypeData.InputOutputList);
				}
				perTypeData.MaxAvailableResource = 0f;
				for (int j = 0; j < perTypeData.ElectricalDistributionGroups.Count; j++)
				{
					MyElectricalDistributionGroup value = perTypeData.ElectricalDistributionGroups[j];
					ComputeInitialDistributionData(ref typeId, value.SinkDataByPriority, value.SourceDataByPriority, ref value.InputOutputData, value.SinksByPriority, value.SourcesByPriority, value.SinkSourcePairs, value.StockpilingStorage, value.OtherStorage, out value.MaxAvailableResource);
					value.ResourceState = RecomputeResourceDistributionPartial(ref typeId, 0, value.SinkDataByPriority, value.SourceDataByPriority, ref value.InputOutputData, value.SinksByPriority, value.SourcesByPriority, value.SinkSourcePairs, value.StockpilingStorage, value.OtherStorage, value.MaxAvailableResource);
					perTypeData.MaxAvailableResource += value.MaxAvailableResource;
					perTypeData.ElectricalDistributionGroups[j] = value;
=======
					perTypeData.DistributionGroupsInUse = 0;
					RecreatePhysicalDistributionGroups(ref typeId, perTypeData.SinksByPriority, perTypeData.SourcesByPriority, perTypeData.InputOutputList);
				}
				perTypeData.MaxAvailableResource = 0f;
				for (int j = 0; j < perTypeData.DistributionGroupsInUse; j++)
				{
					MyPhysicalDistributionGroup value = perTypeData.DistributionGroups[j];
					ComputeInitialDistributionData(ref typeId, value.SinkDataByPriority, value.SourceDataByPriority, ref value.InputOutputData, value.SinksByPriority, value.SourcesByPriority, value.SinkSourcePairs, value.StockpilingStorage, value.OtherStorage, out value.MaxAvailableResources);
					value.ResourceState = RecomputeResourceDistributionPartial(ref typeId, 0, value.SinkDataByPriority, value.SourceDataByPriority, ref value.InputOutputData, value.SinksByPriority, value.SourcesByPriority, value.SinkSourcePairs, value.StockpilingStorage, value.OtherStorage, value.MaxAvailableResources);
					perTypeData.MaxAvailableResource += value.MaxAvailableResources;
					perTypeData.DistributionGroups[j] = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				MyResourceStateEnum resourceState;
				if (perTypeData.MaxAvailableResource == 0f)
				{
					resourceState = MyResourceStateEnum.NoPower;
				}
				else
				{
					resourceState = MyResourceStateEnum.Ok;
<<<<<<< HEAD
					for (int k = 0; k < perTypeData.ElectricalDistributionGroups.Count; k++)
					{
						if (perTypeData.ElectricalDistributionGroups[k].ResourceState == MyResourceStateEnum.OverloadAdaptible)
=======
					for (int k = 0; k < perTypeData.DistributionGroupsInUse; k++)
					{
						if (perTypeData.DistributionGroups[k].ResourceState == MyResourceStateEnum.OverloadAdaptible)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							resourceState = MyResourceStateEnum.OverloadAdaptible;
							break;
						}
<<<<<<< HEAD
						if (perTypeData.ElectricalDistributionGroups[k].ResourceState == MyResourceStateEnum.OverloadBlackout)
=======
						if (perTypeData.DistributionGroups[k].ResourceState == MyResourceStateEnum.OverloadBlackout)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							resourceState = MyResourceStateEnum.OverloadAdaptible;
							break;
						}
					}
				}
				perTypeData.ResourceState = resourceState;
<<<<<<< HEAD
			}
			else if (!IsConveyorConnectionRequired(ref typeId))
			{
				ComputeInitialDistributionData(ref typeId, perTypeData.SinkDataByPriority, perTypeData.SourceDataByPriority, ref perTypeData.InputOutputData, perTypeData.SinksByPriority, perTypeData.SourcesByPriority, perTypeData.InputOutputList, perTypeData.StockpilingStorageIndices, perTypeData.OtherStorageIndices, out perTypeData.MaxAvailableResource);
				perTypeData.ResourceState = RecomputeResourceDistributionPartial(ref typeId, 0, perTypeData.SinkDataByPriority, perTypeData.SourceDataByPriority, ref perTypeData.InputOutputData, perTypeData.SinksByPriority, perTypeData.SourcesByPriority, perTypeData.InputOutputList, perTypeData.StockpilingStorageIndices, perTypeData.OtherStorageIndices, perTypeData.MaxAvailableResource);
			}
			else
			{
				if (perTypeData.GroupsDirty)
				{
					perTypeData.GroupsDirty = false;
					perTypeData.DistributionGroupsInUse = 0;
					RecreatePhysicalDistributionGroups(ref typeId, perTypeData.SinksByPriority, perTypeData.SourcesByPriority, perTypeData.InputOutputList);
				}
				perTypeData.MaxAvailableResource = 0f;
				for (int l = 0; l < perTypeData.DistributionGroupsInUse; l++)
				{
					MyPhysicalDistributionGroup value2 = perTypeData.PhysicalDistributionGroups[l];
					ComputeInitialDistributionData(ref typeId, value2.SinkDataByPriority, value2.SourceDataByPriority, ref value2.InputOutputData, value2.SinksByPriority, value2.SourcesByPriority, value2.SinkSourcePairs, value2.StockpilingStorage, value2.OtherStorage, out value2.MaxAvailableResources);
					value2.ResourceState = RecomputeResourceDistributionPartial(ref typeId, 0, value2.SinkDataByPriority, value2.SourceDataByPriority, ref value2.InputOutputData, value2.SinksByPriority, value2.SourcesByPriority, value2.SinkSourcePairs, value2.StockpilingStorage, value2.OtherStorage, value2.MaxAvailableResources);
					perTypeData.MaxAvailableResource += value2.MaxAvailableResources;
					perTypeData.PhysicalDistributionGroups[l] = value2;
				}
				MyResourceStateEnum resourceState2;
				if (perTypeData.MaxAvailableResource == 0f)
				{
					resourceState2 = MyResourceStateEnum.NoPower;
				}
				else
				{
					resourceState2 = MyResourceStateEnum.Ok;
					for (int m = 0; m < perTypeData.DistributionGroupsInUse; m++)
					{
						if (perTypeData.PhysicalDistributionGroups[m].ResourceState == MyResourceStateEnum.OverloadAdaptible)
						{
							resourceState2 = MyResourceStateEnum.OverloadAdaptible;
							break;
						}
						if (perTypeData.PhysicalDistributionGroups[m].ResourceState == MyResourceStateEnum.OverloadBlackout)
						{
							resourceState2 = MyResourceStateEnum.OverloadAdaptible;
							break;
						}
					}
				}
				perTypeData.ResourceState = resourceState2;
			}
			goto IL_068f;
			IL_01a3:
			HashSet<MyResourceSinkComponent>[] sinksByPriority = perTypeData.SinksByPriority;
			for (int i = 0; i < sinksByPriority.Length; i++)
			{
				foreach (MyResourceSinkComponent item4 in sinksByPriority[i])
				{
					float newResourceInput = (flag ? item4.RequiredInputByType(typeId) : 0f);
					item4.SetInputFromDistributor(typeId, newResourceInput, isAdaptible: true);
				}
			}
			foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput2 in perTypeData.InputOutputList)
			{
				MyResourceSinkComponent item2 = inputOutput2.Item1;
				float num = 0f;
				if (item2 != null)
				{
					num = (flag ? item2.RequiredInputByType(typeId) : 0f);
					item2.SetInputFromDistributor(typeId, num, isAdaptible: true);
				}
			}
			float num2 = (flag ? float.PositiveInfinity : 0f);
			perTypeData.RemainingFuelTimeDirty = false;
			perTypeData.RemainingFuelTime = num2;
			perTypeData.ResourceState = ((!flag) ? MyResourceStateEnum.NoPower : MyResourceStateEnum.Ok);
			MySinkGroupData[] sinkDataByPriority = perTypeData.SinkDataByPriority;
			for (int n = 0; n < ((sinkDataByPriority != null) ? sinkDataByPriority.Length : 0); n++)
			{
				sinkDataByPriority[n].RemainingAvailableResource = num2;
			}
			goto IL_068f;
			IL_068f:
=======
			}
			goto IL_04bc;
			IL_0177:
			HashSet<MyResourceSinkComponent>[] sinksByPriority = perTypeData.SinksByPriority;
			for (int i = 0; i < sinksByPriority.Length; i++)
			{
				Enumerator<MyResourceSinkComponent> enumerator3 = sinksByPriority[i].GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						MyResourceSinkComponent current2 = enumerator3.get_Current();
						float newResourceInput = (flag ? current2.RequiredInputByType(typeId) : 0f);
						current2.SetInputFromDistributor(typeId, newResourceInput, isAdaptible: true);
					}
				}
				finally
				{
					((IDisposable)enumerator3).Dispose();
				}
			}
			foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput2 in perTypeData.InputOutputList)
			{
				MyResourceSinkComponent item2 = inputOutput2.Item1;
				float newResourceInput2 = (flag ? item2.RequiredInputByType(typeId) : 0f);
				item2.SetInputFromDistributor(typeId, newResourceInput2, isAdaptible: true);
			}
			float num = (flag ? float.PositiveInfinity : 0f);
			perTypeData.RemainingFuelTimeDirty = false;
			perTypeData.RemainingFuelTime = num;
			perTypeData.ResourceState = ((!flag) ? MyResourceStateEnum.NoPower : MyResourceStateEnum.Ok);
			MySinkGroupData[] sinkDataByPriority = perTypeData.SinkDataByPriority;
			for (int l = 0; l < ((sinkDataByPriority != null) ? sinkDataByPriority.Length : 0); l++)
			{
				sinkDataByPriority[l].RemainingAvailableResource = num;
			}
			goto IL_04bc;
			IL_04bc:
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			perTypeData.NeedsRecompute = false;
			m_recomputeInProgress = false;
		}

		private void RecreatePhysicalDistributionGroups(ref MyDefinitionId typeId, HashSet<MyResourceSinkComponent>[] allSinksByPriority, HashSet<MyResourceSourceComponent>[] allSourcesByPriority, List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> allSinkSources)
		{
<<<<<<< HEAD
			for (int i = 0; i < allSourcesByPriority.Length; i++)
			{
				foreach (MyResourceSourceComponent item in allSourcesByPriority[i])
=======
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < allSourcesByPriority.Length; i++)
			{
				Enumerator<MyResourceSourceComponent> enumerator = allSourcesByPriority[i].GetEnumerator();
				try
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					while (enumerator.MoveNext())
					{
						MyResourceSourceComponent current = enumerator.get_Current();
						if (current.Entity == null)
						{
							if (current.TemporaryConnectedEntity != null)
							{
								SetEntityGroupForTempConnected(ref typeId, current);
							}
						}
						else
						{
							SetEntityGroup(ref typeId, current.Entity);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			for (int i = 0; i < allSinksByPriority.Length; i++)
			{
<<<<<<< HEAD
				foreach (MyResourceSinkComponent item2 in allSinksByPriority[i])
=======
				Enumerator<MyResourceSinkComponent> enumerator2 = allSinksByPriority[i].GetEnumerator();
				try
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					while (enumerator2.MoveNext())
					{
						MyResourceSinkComponent current2 = enumerator2.get_Current();
						if (current2.Entity == null)
						{
							if (current2.TemporaryConnectedEntity != null)
							{
								SetEntityGroupForTempConnected(ref typeId, current2);
							}
						}
						else
						{
							SetEntityGroup(ref typeId, current2.Entity);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
			foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> allSinkSource in allSinkSources)
			{
				if (allSinkSource.Item1.Entity != null)
				{
					SetEntityGroup(ref typeId, allSinkSource.Item1.Entity);
				}
			}
		}

		private void SetEntityGroup(ref MyDefinitionId typeId, IMyEntity entity)
		{
			IMyConveyorEndpointBlock myConveyorEndpointBlock = entity as IMyConveyorEndpointBlock;
			if (myConveyorEndpointBlock == null)
			{
				return;
			}
			int typeIndex = GetTypeIndex(ref typeId);
			bool flag = false;
			for (int i = 0; i < m_dataPerType[typeIndex].DistributionGroupsInUse; i++)
			{
				if (MyGridConveyorSystem.Reachable(m_dataPerType[typeIndex].PhysicalDistributionGroups[i].FirstEndpoint, myConveyorEndpointBlock.ConveyorEndpoint))
				{
					MyPhysicalDistributionGroup value = m_dataPerType[typeIndex].PhysicalDistributionGroups[i];
					value.Add(typeId, myConveyorEndpointBlock);
					m_dataPerType[typeIndex].PhysicalDistributionGroups[i] = value;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (++m_dataPerType[typeIndex].DistributionGroupsInUse > m_dataPerType[typeIndex].PhysicalDistributionGroups.Count)
				{
					m_dataPerType[typeIndex].PhysicalDistributionGroups.Add(new MyPhysicalDistributionGroup(typeId, myConveyorEndpointBlock));
					return;
				}
				MyPhysicalDistributionGroup value2 = m_dataPerType[typeIndex].PhysicalDistributionGroups[m_dataPerType[typeIndex].DistributionGroupsInUse - 1];
				value2.Init(typeId, myConveyorEndpointBlock);
				m_dataPerType[typeIndex].PhysicalDistributionGroups[m_dataPerType[typeIndex].DistributionGroupsInUse - 1] = value2;
			}
		}

		private void SetEntityGroupForTempConnected(ref MyDefinitionId typeId, MyResourceSinkComponent sink)
		{
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			IMyConveyorEndpointBlock myConveyorEndpointBlock = sink.TemporaryConnectedEntity as IMyConveyorEndpointBlock;
			int typeIndex = GetTypeIndex(ref typeId);
			bool flag = false;
			int num = 0;
			while (num < m_dataPerType[typeIndex].DistributionGroupsInUse)
			{
				if (myConveyorEndpointBlock == null || !MyGridConveyorSystem.Reachable(m_dataPerType[typeIndex].PhysicalDistributionGroups[num].FirstEndpoint, myConveyorEndpointBlock.ConveyorEndpoint))
				{
					bool flag2 = false;
					if (myConveyorEndpointBlock == null)
					{
						HashSet<MyResourceSourceComponent>[] sourcesByPriority = m_dataPerType[typeIndex].PhysicalDistributionGroups[num].SourcesByPriority;
						for (int i = 0; i < sourcesByPriority.Length; i++)
						{
							Enumerator<MyResourceSourceComponent> enumerator = sourcesByPriority[i].GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									MyResourceSourceComponent current = enumerator.get_Current();
									if (sink.TemporaryConnectedEntity == current.TemporaryConnectedEntity)
									{
										flag2 = true;
										break;
									}
								}
							}
							finally
							{
								((IDisposable)enumerator).Dispose();
							}
							if (flag2)
							{
								break;
							}
						}
					}
					if (!flag2)
					{
						num++;
						continue;
					}
				}
				MyPhysicalDistributionGroup value = m_dataPerType[typeIndex].PhysicalDistributionGroups[num];
				value.AddTempConnected(typeId, sink);
				m_dataPerType[typeIndex].PhysicalDistributionGroups[num] = value;
				flag = true;
				break;
			}
			if (!flag)
			{
				if (++m_dataPerType[typeIndex].DistributionGroupsInUse > m_dataPerType[typeIndex].PhysicalDistributionGroups.Count)
				{
					m_dataPerType[typeIndex].PhysicalDistributionGroups.Add(new MyPhysicalDistributionGroup(typeId, sink));
					return;
				}
				MyPhysicalDistributionGroup value2 = m_dataPerType[typeIndex].PhysicalDistributionGroups[m_dataPerType[typeIndex].DistributionGroupsInUse - 1];
				value2.InitFromTempConnected(typeId, sink);
				m_dataPerType[typeIndex].PhysicalDistributionGroups[m_dataPerType[typeIndex].DistributionGroupsInUse - 1] = value2;
			}
		}

		private void SetEntityGroupForTempConnected(ref MyDefinitionId typeId, MyResourceSourceComponent source)
		{
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			IMyConveyorEndpointBlock myConveyorEndpointBlock = source.TemporaryConnectedEntity as IMyConveyorEndpointBlock;
			int typeIndex = GetTypeIndex(ref typeId);
			bool flag = false;
			int num = 0;
			while (num < m_dataPerType[typeIndex].DistributionGroupsInUse)
			{
				if (myConveyorEndpointBlock == null || !MyGridConveyorSystem.Reachable(m_dataPerType[typeIndex].PhysicalDistributionGroups[num].FirstEndpoint, myConveyorEndpointBlock.ConveyorEndpoint))
				{
					bool flag2 = false;
					if (myConveyorEndpointBlock == null)
					{
						HashSet<MyResourceSinkComponent>[] sinksByPriority = m_dataPerType[typeIndex].PhysicalDistributionGroups[num].SinksByPriority;
						for (int i = 0; i < sinksByPriority.Length; i++)
						{
							Enumerator<MyResourceSinkComponent> enumerator = sinksByPriority[i].GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									MyResourceSinkComponent current = enumerator.get_Current();
									if (source.TemporaryConnectedEntity == current.TemporaryConnectedEntity)
									{
										flag2 = true;
										break;
									}
								}
							}
							finally
							{
								((IDisposable)enumerator).Dispose();
							}
							if (flag2)
							{
								break;
							}
						}
					}
					if (!flag2)
					{
						num++;
						continue;
					}
				}
				MyPhysicalDistributionGroup value = m_dataPerType[typeIndex].PhysicalDistributionGroups[num];
				value.AddTempConnected(typeId, source);
				m_dataPerType[typeIndex].PhysicalDistributionGroups[num] = value;
				flag = true;
				break;
			}
			if (!flag)
			{
				if (++m_dataPerType[typeIndex].DistributionGroupsInUse > m_dataPerType[typeIndex].PhysicalDistributionGroups.Count)
				{
					m_dataPerType[typeIndex].PhysicalDistributionGroups.Add(new MyPhysicalDistributionGroup(typeId, source));
					return;
				}
				MyPhysicalDistributionGroup value2 = m_dataPerType[typeIndex].PhysicalDistributionGroups[m_dataPerType[typeIndex].DistributionGroupsInUse - 1];
				value2.InitFromTempConnected(typeId, source);
				m_dataPerType[typeIndex].PhysicalDistributionGroups[m_dataPerType[typeIndex].DistributionGroupsInUse - 1] = value2;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="typeId"></param>
		/// <param name="sinkDataByPriority"></param>
		/// <param name="sourceDataByPriority"></param>
		/// <param name="sinkSourceData"></param>
		/// <param name="sinksByPriority"></param>
		/// <param name="sourcesByPriority"></param>
		/// <param name="sinkSourcePairs"></param>
		/// <param name="stockpilingStorageList">Indices into sinkSourcePairs</param>
		/// <param name="otherStorageList">Indices into sinkSourcePairs</param>
		/// <param name="maxAvailableResource"></param>
		private static void ComputeInitialDistributionData(ref MyDefinitionId typeId, MySinkGroupData[] sinkDataByPriority, MySourceGroupData[] sourceDataByPriority, ref MyTuple<MySinkGroupData, MySourceGroupData> sinkSourceData, HashSet<MyResourceSinkComponent>[] sinksByPriority, HashSet<MyResourceSourceComponent>[] sourcesByPriority, List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> sinkSourcePairs, MyList<int> stockpilingStorageList, MyList<int> otherStorageList, out float maxAvailableResource)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			maxAvailableResource = 0f;
			for (int i = 0; i < sourceDataByPriority.Length; i++)
			{
				HashSet<MyResourceSourceComponent> obj = sourcesByPriority[i];
				MySourceGroupData mySourceGroupData = sourceDataByPriority[i];
				mySourceGroupData.MaxAvailableResource = 0f;
				Enumerator<MyResourceSourceComponent> enumerator = obj.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyResourceSourceComponent current = enumerator.get_Current();
						if (current.Enabled && current.HasCapacityRemainingByType(typeId))
						{
							mySourceGroupData.MaxAvailableResource += current.MaxOutputByType(typeId);
							mySourceGroupData.InfiniteCapacity = current.IsInfiniteCapacity;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				maxAvailableResource += mySourceGroupData.MaxAvailableResource;
				sourceDataByPriority[i] = mySourceGroupData;
			}
			float num = 0f;
			for (int j = 0; j < sinksByPriority.Length; j++)
			{
				float num2 = 0f;
				bool flag = true;
				Enumerator<MyResourceSinkComponent> enumerator2 = sinksByPriority[j].GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MyResourceSinkComponent current2 = enumerator2.get_Current();
						num2 += current2.RequiredInputByType(typeId);
						flag = flag && IsAdaptible(current2);
					}
				}
				finally
				{
<<<<<<< HEAD
					num2 += item2.RequiredInputByType(typeId);
					flag = flag && IsAdaptible(item2);
=======
					((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				sinkDataByPriority[j].RequiredInput = num2;
				sinkDataByPriority[j].IsAdaptible = flag;
				num += num2;
				sinkDataByPriority[j].RequiredInputCumulative = num;
			}
			PrepareSinkSourceData(ref typeId, ref sinkSourceData, sinkSourcePairs, stockpilingStorageList, otherStorageList);
			maxAvailableResource += sinkSourceData.Item2.MaxAvailableResource;
		}

		private static void PrepareSinkSourceData(ref MyDefinitionId typeId, ref MyTuple<MySinkGroupData, MySourceGroupData> sinkSourceData, List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> sinkSourcePairs, MyList<int> stockpilingStorageList, MyList<int> otherStorageList)
		{
			stockpilingStorageList.Clear();
			otherStorageList.Clear();
			sinkSourceData.Item1.IsAdaptible = true;
			sinkSourceData.Item1.RequiredInput = 0f;
			sinkSourceData.Item1.RequiredInputCumulative = 0f;
			sinkSourceData.Item2.MaxAvailableResource = 0f;
			for (int i = 0; i < sinkSourcePairs.Count; i++)
			{
				MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> myTuple = sinkSourcePairs[i];
				bool flag = myTuple.Item2.ProductionEnabledByType(typeId);
				bool num = myTuple.Item2.Enabled && !flag && myTuple.Item1.RequiredInputByType(typeId) > 0f;
				sinkSourceData.Item1.IsAdaptible = sinkSourceData.Item1.IsAdaptible && IsAdaptible(myTuple.Item1);
				sinkSourceData.Item1.RequiredInput += myTuple.Item1.RequiredInputByType(typeId);
				if (num)
				{
					sinkSourceData.Item1.RequiredInputCumulative += myTuple.Item1.RequiredInputByType(typeId);
				}
				sinkSourceData.Item2.InfiniteCapacity = float.IsInfinity(myTuple.Item2.RemainingCapacityByType(typeId));
				if (num)
				{
					stockpilingStorageList.Add(i);
					continue;
				}
				otherStorageList.Add(i);
				if (myTuple.Item2.Enabled && flag)
				{
					sinkSourceData.Item2.MaxAvailableResource += myTuple.Item2.MaxOutputByType(typeId);
				}
			}
		}

		/// <summary>
		/// Recomputes power distribution in subset of all priority groups (in range
		/// from startPriorityIdx until the end). Passing index 0 recomputes all priority groups.
		/// </summary>
		private static MyResourceStateEnum RecomputeResourceDistributionPartial(ref MyDefinitionId typeId, int startPriorityIdx, MySinkGroupData[] sinkDataByPriority, MySourceGroupData[] sourceDataByPriority, ref MyTuple<MySinkGroupData, MySourceGroupData> sinkSourceData, HashSet<MyResourceSinkComponent>[] sinksByPriority, HashSet<MyResourceSourceComponent>[] sourcesByPriority, List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> sinkSourcePairs, MyList<int> stockpilingStorageList, MyList<int> otherStorageList, float availableResource)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_073d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0742: Unknown result type (might be due to invalid IL or missing references)
			float num = availableResource;
			int i;
			for (i = startPriorityIdx; i < sinksByPriority.Length; i++)
			{
				sinkDataByPriority[i].RemainingAvailableResource = availableResource;
				Enumerator<MyResourceSinkComponent> enumerator;
				if (sinkDataByPriority[i].RequiredInput <= availableResource)
				{
					availableResource -= sinkDataByPriority[i].RequiredInput;
					enumerator = sinksByPriority[i].GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							MyResourceSinkComponent current = enumerator.get_Current();
							current.SetInputFromDistributor(typeId, current.RequiredInputByType(typeId), sinkDataByPriority[i].IsAdaptible);
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					continue;
				}
				if (sinkDataByPriority[i].IsAdaptible && availableResource > 0f)
				{
					enumerator = sinksByPriority[i].GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							MyResourceSinkComponent current2 = enumerator.get_Current();
							current2.SetInputFromDistributor(newResourceInput: current2.RequiredInputByType(typeId) / sinkDataByPriority[i].RequiredInput * availableResource, resourceTypeId: typeId, isAdaptible: true);
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					availableResource = 0f;
					continue;
				}
<<<<<<< HEAD
				foreach (MyResourceSinkComponent item10 in sinksByPriority[i])
				{
					item10.SetInputFromDistributor(typeId, 0f, sinkDataByPriority[i].IsAdaptible);
				}
=======
				enumerator = sinksByPriority[i].GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						enumerator.get_Current().SetInputFromDistributor(typeId, 0f, sinkDataByPriority[i].IsAdaptible);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				sinkDataByPriority[i].RemainingAvailableResource = availableResource;
			}
			for (; i < sinkDataByPriority.Length; i++)
			{
				sinkDataByPriority[i].RemainingAvailableResource = 0f;
				Enumerator<MyResourceSinkComponent> enumerator = sinksByPriority[i].GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						enumerator.get_Current().SetInputFromDistributor(typeId, 0f, sinkDataByPriority[i].IsAdaptible);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			float num2 = num - availableResource + ((startPriorityIdx != 0) ? (sinkDataByPriority[0].RemainingAvailableResource - sinkDataByPriority[startPriorityIdx].RemainingAvailableResource) : 0f);
			float num3 = Math.Max(num - num2, 0f);
			float num4 = num3;
			if (stockpilingStorageList.Count > 0)
			{
				float requiredInputCumulative = sinkSourceData.Item1.RequiredInputCumulative;
				if (requiredInputCumulative <= num4)
				{
					num4 -= requiredInputCumulative;
					foreach (int stockpilingStorage in stockpilingStorageList)
					{
						MyResourceSinkComponent item = sinkSourcePairs[stockpilingStorage].Item1;
						item.SetInputFromDistributor(typeId, item.RequiredInputByType(typeId), isAdaptible: true);
					}
					sinkSourceData.Item1.RemainingAvailableResource = num4;
				}
				else
				{
					foreach (int stockpilingStorage2 in stockpilingStorageList)
					{
						MyResourceSinkComponent item2 = sinkSourcePairs[stockpilingStorage2].Item1;
						item2.SetInputFromDistributor(newResourceInput: item2.RequiredInputByType(typeId) / requiredInputCumulative * num3, resourceTypeId: typeId, isAdaptible: true);
					}
					num4 = 0f;
					sinkSourceData.Item1.RemainingAvailableResource = num4;
				}
			}
			float num5 = num3 - num4;
			float num6 = Math.Max(num - (sinkSourceData.Item2.MaxAvailableResource - sinkSourceData.Item2.MaxAvailableResource * sinkSourceData.Item2.UsageRatio) - num2 - num5, 0f);
			float num7 = num6;
			if (otherStorageList.Count > 0)
			{
				float num8 = sinkSourceData.Item1.RequiredInput - sinkSourceData.Item1.RequiredInputCumulative;
				if (num8 <= num7)
				{
					num7 -= num8;
					for (int j = 0; j < otherStorageList.Count; j++)
					{
						int index = otherStorageList[j];
						MyResourceSinkComponent item3 = sinkSourcePairs[index].Item1;
						item3.SetInputFromDistributor(typeId, item3.RequiredInputByType(typeId), isAdaptible: true);
					}
					sinkSourceData.Item1.RemainingAvailableResource = num7;
				}
				else
				{
					for (int k = 0; k < otherStorageList.Count; k++)
					{
						int index2 = otherStorageList[k];
						MyResourceSinkComponent item4 = sinkSourcePairs[index2].Item1;
						item4.SetInputFromDistributor(newResourceInput: item4.RequiredInputByType(typeId) / num8 * num7, resourceTypeId: typeId, isAdaptible: true);
					}
					num7 = 0f;
					sinkSourceData.Item1.RemainingAvailableResource = num7;
				}
			}
			float num9 = num6 - num7;
			float num10 = num5 + num2;
			if (sinkSourceData.Item2.MaxAvailableResource > 0f)
			{
				float num11 = num10;
				sinkSourceData.Item2.UsageRatio = Math.Min(1f, num11 / sinkSourceData.Item2.MaxAvailableResource);
				num10 -= Math.Min(num11, sinkSourceData.Item2.MaxAvailableResource);
			}
			else
			{
				sinkSourceData.Item2.UsageRatio = 0f;
			}
			num5 = num3 - num4;
			num6 = Math.Max(num - (sinkSourceData.Item2.MaxAvailableResource - sinkSourceData.Item2.MaxAvailableResource * sinkSourceData.Item2.UsageRatio) - num2 - num5, 0f);
			num7 = num6;
			if (otherStorageList.Count > 0)
			{
				float num12 = sinkSourceData.Item1.RequiredInput - sinkSourceData.Item1.RequiredInputCumulative;
				if (num12 <= num7)
				{
					num7 -= num12;
					for (int l = 0; l < otherStorageList.Count; l++)
					{
						int index3 = otherStorageList[l];
						MyResourceSinkComponent item5 = sinkSourcePairs[index3].Item1;
						item5.SetInputFromDistributor(typeId, item5.RequiredInputByType(typeId), isAdaptible: true);
					}
					sinkSourceData.Item1.RemainingAvailableResource = num7;
				}
				else
				{
					for (int m = 0; m < otherStorageList.Count; m++)
					{
						int index4 = otherStorageList[m];
						MyResourceSinkComponent item6 = sinkSourcePairs[index4].Item1;
						item6.SetInputFromDistributor(newResourceInput: item6.RequiredInputByType(typeId) / num12 * num7, resourceTypeId: typeId, isAdaptible: true);
					}
					num7 = 0f;
					sinkSourceData.Item1.RemainingAvailableResource = num7;
				}
			}
			sinkSourceData.Item2.ActiveCount = 0;
			for (int n = 0; n < otherStorageList.Count; n++)
			{
				int index5 = otherStorageList[n];
				MyResourceSourceComponent item7 = sinkSourcePairs[index5].Item2;
				if (item7.Enabled && item7.ProductionEnabledByType(typeId) && item7.HasCapacityRemainingByType(typeId))
				{
					sinkSourceData.Item2.ActiveCount++;
					item7.SetOutputByType(typeId, sinkSourceData.Item2.UsageRatio * item7.MaxOutputByType(typeId));
				}
			}
			int num13 = 0;
			float num14 = num10 + num9;
			for (; num13 < sourcesByPriority.Length; num13++)
			{
				if (sourceDataByPriority[num13].MaxAvailableResource > 0f)
				{
					float num15 = Math.Max(num14, 0f);
					sourceDataByPriority[num13].UsageRatio = Math.Min(1f, num15 / sourceDataByPriority[num13].MaxAvailableResource);
					num14 -= Math.Min(num15, sourceDataByPriority[num13].MaxAvailableResource);
				}
				else
				{
					sourceDataByPriority[num13].UsageRatio = 0f;
				}
				sourceDataByPriority[num13].ActiveCount = 0;
				Enumerator<MyResourceSourceComponent> enumerator3 = sourcesByPriority[num13].GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						MyResourceSourceComponent current5 = enumerator3.get_Current();
						if (current5.Enabled && current5.HasCapacityRemainingByType(typeId))
						{
							sourceDataByPriority[num13].ActiveCount++;
							current5.SetOutputByType(typeId, sourceDataByPriority[num13].UsageRatio * current5.MaxOutputByType(typeId));
						}
					}
				}
				finally
				{
					((IDisposable)enumerator3).Dispose();
				}
			}
			if (num == 0f)
			{
				return MyResourceStateEnum.NoPower;
			}
			if (sinkDataByPriority[m_sinkGroupPrioritiesTotal - 1].RequiredInputCumulative > num)
			{
				MySinkGroupData mySinkGroupData = Enumerable.Last<MySinkGroupData>((IEnumerable<MySinkGroupData>)sinkDataByPriority);
				if (mySinkGroupData.IsAdaptible && mySinkGroupData.RemainingAvailableResource != 0f)
				{
					return MyResourceStateEnum.OverloadAdaptible;
				}
				return MyResourceStateEnum.OverloadBlackout;
			}
			return MyResourceStateEnum.Ok;
		}

		/// <summary>
		/// Creates groupings of sinks and sources by grids they are on. Needs to be shuffled into chunks of connected grids
		/// </summary>
		private void RecreateElectricallDistributionGroups(ref MyDefinitionId typeId, HashSet<MyResourceSinkComponent>[] allSinksByPriority, HashSet<MyResourceSourceComponent>[] allSourcesByPriority, List<MyTuple<MyResourceSinkComponent, MyResourceSourceComponent>> sinkSourcePairs)
		{
			List<MyCubeGrid> grids = new List<MyCubeGrid>();
			int typeIndex = GetTypeIndex(ref typeId);
			m_dataPerType[typeIndex].ElectricalDistributionGroups.Clear();
			HashSet<MyResourceSourceComponent>[] array = allSourcesByPriority;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (MyResourceSourceComponent item in array[i])
				{
					MyCubeGrid grid = item.Grid;
					if (grid != null && !grids.Contains(grid))
					{
						grids.Add(grid);
					}
				}
			}
			HashSet<MyResourceSinkComponent>[] array2 = allSinksByPriority;
			for (int i = 0; i < array2.Length; i++)
			{
				foreach (MyResourceSinkComponent item2 in array2[i])
				{
					MyCubeGrid grid2 = item2.Grid;
					if (grid2 != null && !grids.Contains(grid2))
					{
						grids.Add(grid2);
					}
				}
			}
			CreateEmptyElectricalGroups(ref typeId, ref grids);
			array = allSourcesByPriority;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (MyResourceSourceComponent item3 in array[i])
				{
					int electricalGroupIndex = GetElectricalGroupIndex(ref typeId, item3.Grid);
					m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex].Add(typeId, item3);
					m_dataPerType[typeIndex].Add(typeId, item3);
				}
			}
			array2 = allSinksByPriority;
			for (int i = 0; i < array2.Length; i++)
			{
				foreach (MyResourceSinkComponent item4 in array2[i])
				{
					int electricalGroupIndex2 = GetElectricalGroupIndex(ref typeId, item4.Grid);
					m_dataPerType[typeIndex].ElectricalDistributionGroups[electricalGroupIndex2].Add(typeId, item4);
					m_dataPerType[typeIndex].Add(typeId, item4);
				}
			}
			foreach (MyTuple<MyResourceSinkComponent, MyResourceSourceComponent> inputOutput in m_dataPerType[typeIndex].InputOutputList)
			{
				MyCubeGrid grid3 = inputOutput.Item1.Grid;
				if (grid3 == null)
				{
					grid3 = inputOutput.Item2.Grid;
				}
				int index = 0;
				if (grid3 != null)
				{
					index = GetElectricalGroupIndex(ref typeId, grid3);
				}
				m_dataPerType[typeIndex].ElectricalDistributionGroups[index].InputOutputList.Add(inputOutput);
				m_dataPerType[typeIndex].ElectricalDistributionGroups[index].SinkSourcePairs.Add(inputOutput);
			}
		}

		/// <summary>
		/// WARNING! `grids` will loose all elements
		/// Creates Empty Electrical Groups, for specified grids.
		/// </summary>
		private void CreateEmptyElectricalGroups(ref MyDefinitionId typeId, ref List<MyCubeGrid> grids)
		{
			List<MyCubeGrid> list = new List<MyCubeGrid>();
			int typeIndex = GetTypeIndex(ref typeId);
			while (grids.Count > 0)
			{
				MyCubeGrid nodeInGroup = grids[0];
				MyCubeGridGroups.Static.Electrical.GetGroupNodes(nodeInGroup, list);
				List<long> list2 = new List<long>();
				foreach (MyCubeGrid item in list)
				{
					grids.Remove(item);
					list2.Add(item.EntityId);
				}
				list.Clear();
				m_dataPerType[typeIndex].ElectricalDistributionGroups.Add(new MyElectricalDistributionGroup(typeId, list2, this));
			}
		}

		/// <summary>
		/// Mostly debug method to verify that all members of the group have 
		/// same ability to adapt as given sink.
		/// </summary>
		private bool MatchesAdaptability(HashSet<MyResourceSinkComponent> group, MyResourceSinkComponent referenceSink)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			bool flag = IsAdaptible(referenceSink);
			Enumerator<MyResourceSinkComponent> enumerator = group.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (IsAdaptible(enumerator.get_Current()) != flag)
					{
						return false;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return true;
		}

		private bool MatchesInfiniteCapacity(HashSet<MyResourceSourceComponent> group, MyResourceSourceComponent producer)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyResourceSourceComponent> enumerator = group.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyResourceSourceComponent current = enumerator.get_Current();
					if (producer.IsInfiniteCapacity != current.IsInfiniteCapacity)
					{
						return false;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return true;
		}

		[Conditional("DEBUG")]
		private void UpdateTrace()
		{
			for (int i = 0; i < m_typeGroupCount; i++)
			{
				for (int j = 0; j < m_dataPerType[i].DistributionGroupsInUse; j++)
				{
					MyPhysicalDistributionGroup myPhysicalDistributionGroup = m_dataPerType[i].PhysicalDistributionGroups[j];
					for (int k = 0; k < myPhysicalDistributionGroup.SinkSourcePairs.Count; k++)
					{
					}
				}
			}
		}

		private HashSet<MyResourceSinkComponent> GetSinksOfType(ref MyDefinitionId typeId, MyStringHash groupType)
		{
			if (!TryGetTypeIndex(typeId, out var typeIndex) || !m_sinkSubtypeToPriority.TryGetValue(groupType, out var value))
			{
				return null;
			}
			return m_dataPerType[typeIndex].SinksByPriority[value];
		}

		private HashSet<MyResourceSourceComponent> GetSourcesOfType(ref MyDefinitionId typeId, MyStringHash groupType)
		{
			if (!TryGetTypeIndex(typeId, out var typeIndex) || !m_sourceSubtypeToPriority.TryGetValue(groupType, out var value))
			{
				return null;
			}
			return m_dataPerType[typeIndex].SourcesByPriority[value];
		}

		private MyResourceSourceComponent GetFirstSourceOfType(ref MyDefinitionId resourceTypeId)
		{
			int typeIndex = GetTypeIndex(ref resourceTypeId);
			for (int i = 0; i < m_dataPerType[typeIndex].SourcesByPriority.Length; i++)
			{
				HashSet<MyResourceSourceComponent> val = m_dataPerType[typeIndex].SourcesByPriority[i];
				if (val.get_Count() > 0)
				{
					return val.FirstElement<MyResourceSourceComponent>();
				}
			}
			return null;
		}

		public MyMultipleEnabledEnum SourcesEnabledByType(MyDefinitionId resourceTypeId)
		{
			if (!TryGetTypeIndex(ref resourceTypeId, out var typeIndex))
			{
				return MyMultipleEnabledEnum.NoObjects;
			}
			if (m_dataPerType[typeIndex].SourcesEnabledDirty)
			{
				RefreshSourcesEnabled(resourceTypeId);
			}
			return m_dataPerType[typeIndex].SourcesEnabled;
		}

		public List<List<long>> GetElectricalGridsGroups()
		{
			List<List<long>> list = new List<List<long>>();
			MyDefinitionId typeId = ElectricityId;
			int typeIndex = GetTypeIndex(ref typeId);
			foreach (MyElectricalDistributionGroup electricalDistributionGroup in m_dataPerType[typeIndex].ElectricalDistributionGroups)
			{
				list.Add(electricalDistributionGroup.GridsEntityIds);
			}
			return list;
		}

		/// <summary>
		/// Specify grid when asking for electricity.
		/// </summary>
		public float RemainingFuelTimeByType(MyDefinitionId resourceTypeId, MyCubeGrid grid = null)
		{
			if (!TryGetTypeIndex(ref resourceTypeId, out var typeIndex))
<<<<<<< HEAD
			{
				return 0f;
			}
			if (resourceTypeId == ElectricityId && grid != null)
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyElectricalDistributionGroup myElectricalDistributionGroup = m_dataPerType[typeIndex].ElectricalDistributionGroups[GetElectricalGroupIndex(ref resourceTypeId, grid)];
				if (myElectricalDistributionGroup.GridsEntityIds.Contains(grid.EntityId))
				{
					if (!myElectricalDistributionGroup.RemainingFuelTimeDirty)
					{
						return myElectricalDistributionGroup.RemainingFuelTime;
					}
					myElectricalDistributionGroup.RemainingFuelTime = myElectricalDistributionGroup.ComputeRemainingElectricityTime();
					return myElectricalDistributionGroup.RemainingFuelTime;
				}
				return 0f;
			}
			if (!m_dataPerType[typeIndex].RemainingFuelTimeDirty)
			{
				return m_dataPerType[typeIndex].RemainingFuelTime;
			}
			m_dataPerType[typeIndex].RemainingFuelTime = ComputeRemainingFuelTime(resourceTypeId);
			return m_dataPerType[typeIndex].RemainingFuelTime;
		}

		private bool NeedsRecompute(ref MyDefinitionId typeId)
		{
			if (!m_changedTypes.TryGetValue(typeId, out var value) || value <= 0)
			{
				if (TryGetTypeIndex(ref typeId, out var typeIndex))
				{
					return m_dataPerType[typeIndex].NeedsRecompute;
				}
				return false;
			}
			return true;
		}

		private bool NeedsRecompute(ref MyDefinitionId typeId, int typeIndex)
		{
			if (m_typeGroupCount <= 0 || typeIndex < 0 || m_dataPerType.Count <= typeIndex || !m_dataPerType[typeIndex].NeedsRecompute)
			{
				if (m_changedTypes.TryGetValue(typeId, out var value))
				{
					return value > 0;
				}
				return false;
			}
			return true;
		}

		/// <summary>
		/// Specify grid when asking for electricity. Electricity is shared only between certain grids.
		/// </summary>
		public MyResourceStateEnum ResourceStateByType(MyDefinitionId typeId, bool withRecompute = true, MyCubeGrid grid = null)
		{
			int typeIndex = GetTypeIndex(ref typeId);
			if (withRecompute && NeedsRecompute(ref typeId) && !MyEntities.IsAsyncUpdateInProgress)
			{
				RecomputeResourceDistribution(ref typeId);
			}
			if (typeId == ElectricityId && grid != null)
			{
				return m_dataPerType[typeIndex].ElectricalDistributionGroups[GetElectricalGroupIndex(ref typeId, grid)].ResourceState;
			}
			if (!withRecompute && (typeIndex < 0 || typeIndex >= m_dataPerType.Count))
			{
				return MyResourceStateEnum.NoPower;
			}
			return m_dataPerType[typeIndex].ResourceState;
		}

		private bool TryGetTypeIndex(MyDefinitionId typeId, out int typeIndex)
		{
			return TryGetTypeIndex(ref typeId, out typeIndex);
		}

		private bool TryGetTypeIndex(ref MyDefinitionId typeId, out int typeIndex)
		{
			typeIndex = 0;
			if (m_typeGroupCount == 0)
			{
				return false;
			}
			if (m_typeGroupCount > 1)
			{
				return m_typeIdToIndex.TryGetValue(typeId, out typeIndex);
			}
			return true;
		}

		private int GetTypeIndex(ref MyDefinitionId typeId)
		{
			int result = 0;
			if (m_typeGroupCount > 1)
			{
				result = m_typeIdToIndex[typeId];
			}
			return result;
		}

		public int GetElectricalGroupIndex(ref MyDefinitionId typeId, MyCubeGrid grid)
		{
			int num = 0;
			int typeIndex = GetTypeIndex(ref typeId);
			if (grid == null)
			{
				if (m_dataPerType[typeIndex].ElectricalDistributionGroups.Count == 0)
				{
					m_dataPerType[typeIndex].ElectricalDistributionGroups.Add(new MyElectricalDistributionGroup(typeId, new List<long>(), this));
				}
				return 0;
			}
			foreach (MyElectricalDistributionGroup electricalDistributionGroup in m_dataPerType[typeIndex].ElectricalDistributionGroups)
			{
				if (electricalDistributionGroup.GridsEntityIds.Contains(grid.EntityId))
				{
					return num;
				}
				num++;
			}
			m_dataPerType[typeIndex].ElectricalDistributionGroups.Add(new MyElectricalDistributionGroup(typeId, new List<long> { grid.EntityId }, this));
			return GetElectricalGroupIndex(ref typeId, grid);
		}

		private bool IsAlreadyGrouped(MyCubeGrid grid, List<List<MyCubeGrid>> gridGroups)
		{
			foreach (List<MyCubeGrid> gridGroup in gridGroups)
			{
				foreach (MyCubeGrid item in gridGroup)
				{
					if (item.EntityId == grid.EntityId)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static int GetTypeIndexTotal(ref MyDefinitionId typeId)
		{
			int result = 0;
			if (m_typeGroupCountTotal > 1)
			{
				result = m_typeIdToIndexTotal[typeId];
			}
			return result;
		}

		public static bool IsConveyorConnectionRequiredTotal(MyDefinitionId typeId)
		{
			return IsConveyorConnectionRequiredTotal(ref typeId);
		}

		public static bool IsConveyorConnectionRequiredTotal(ref MyDefinitionId typeId)
		{
			try
			{
				return m_typeIdToConveyorConnectionRequiredTotal[typeId];
			}
			catch (Exception)
			{
				StringBuilder stringBuilder = new StringBuilder(string.Concat("SLIME: IsConveyorConnectionRequiredTotal: ", typeId, " -> "));
				foreach (KeyValuePair<MyDefinitionId, bool> item in m_typeIdToConveyorConnectionRequiredTotal)
				{
					string value = string.Concat(item.Key, "/", item.Value.ToString(), " ");
					stringBuilder.Append(value);
				}
				throw new Exception(stringBuilder.ToString());
			}
		}

		private bool IsConveyorConnectionRequired(ref MyDefinitionId typeId)
		{
			try
			{
				return m_typeIdToConveyorConnectionRequired[typeId];
			}
			catch (Exception)
			{
				StringBuilder stringBuilder = new StringBuilder(string.Concat("SLIME: IsConveyorConnectionRequiredTotal: ", typeId, " -> "));
				foreach (KeyValuePair<MyDefinitionId, bool> item in m_typeIdToConveyorConnectionRequired)
				{
					string.Concat(item.Key, "/", item.Value.ToString(), " ");
				}
				throw new Exception(stringBuilder.ToString());
			}
		}

		internal static int GetPriority(MyResourceSinkComponent sink)
		{
			return m_sinkSubtypeToPriority[sink.Group];
		}

		internal static int GetPriority(MyResourceSourceComponent source)
		{
			return m_sourceSubtypeToPriority[source.Group];
		}

		private static bool IsAdaptible(MyResourceSinkComponent sink)
		{
			return m_sinkSubtypeToAdaptability[sink.Group];
		}

		private void RemoveType(ref MyDefinitionId typeId)
		{
			if (TryGetTypeIndex(ref typeId, out var typeIndex))
			{
				m_dataPerType.RemoveAt(typeIndex);
				m_initializedTypes.Remove(typeId);
				m_typeGroupCount--;
				m_typeIdToIndex.Remove(typeId);
				m_typeIdToConveyorConnectionRequired.Remove(typeId);
				RaiseSystemChanged();
<<<<<<< HEAD
			}
		}

		private void RaiseSystemChanged()
		{
			this.SystemChanged?.Invoke();
		}

		public void SetDataDirty(MyDefinitionId typeId)
		{
			if (TryGetTypeIndex(ref typeId, out var typeIndex))
			{
				m_dataPerType[typeIndex].GroupsDirty = true;
				m_dataPerType[typeIndex].RemainingFuelTimeDirty = true;
				m_dataPerType[typeIndex].SourcesEnabledDirty = true;
			}
		}

		public void SetNeedRecompute(MyDefinitionId typeId)
		{
			if (TryGetTypeIndex(ref typeId, out var typeIndex))
			{
				m_dataPerType[typeIndex].NeedsRecompute = true;
			}
		}

		public void SetNeedRecomputeAll()
		{
			foreach (PerTypeData item in m_dataPerType)
			{
				item.NeedsRecompute = true;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void RaiseSystemChanged()
		{
			this.SystemChanged?.Invoke();
		}

		private void Sink_OnAddType(MyResourceSinkComponent sink, MyDefinitionId resourceType)
		{
			RefreshSink(sink);
		}

		private void Sink_OnRemoveType(MyResourceSinkComponent sink, MyDefinitionId resourceType)
		{
			RefreshSink(sink);
		}

		private void RefreshSink(MyResourceSinkComponent sink)
		{
			if (MyEntities.IsAsyncUpdateInProgress)
			{
				MyEntities.InvokeLater(delegate
				{
					RefreshSink(sink);
				});
			}
			else
			{
				RemoveSinkLazy(sink, resetSinkInput: false);
				CheckDistributionSystemChanges();
				AddSinkLazy(sink);
			}
		}

		private void Sink_RequiredInputChanged(MyDefinitionId changedResourceTypeId, MyResourceSinkComponent changedSink, float oldRequirement, float newRequirement)
		{
			if (m_typeIdToIndex.ContainsKey(changedResourceTypeId) && m_sinkSubtypeToPriority.ContainsKey(changedSink.Group))
			{
				int typeIndex = GetTypeIndex(ref changedResourceTypeId);
				if (TryGetTypeIndex(changedResourceTypeId, out typeIndex))
				{
					m_dataPerType[typeIndex].NeedsRecompute = true;
				}
			}
		}

		private float Sink_IsResourceAvailable(MyDefinitionId resourceTypeId, MyResourceSinkComponent receiver)
		{
			int typeIndex = GetTypeIndex(ref resourceTypeId);
			int priority = GetPriority(receiver);
			if (IsConveyorConnectionRequired(ref resourceTypeId) || resourceTypeId == ElectricityId)
			{
				if (resourceTypeId != ElectricityId && !(receiver.Entity is IMyConveyorEndpointBlock))
				{
					return 0f;
				}
				int i;
				for (i = 0; i < m_dataPerType[typeIndex].DistributionGroupsInUse && !m_dataPerType[typeIndex].PhysicalDistributionGroups[i].SinksByPriority[priority].Contains(receiver); i++)
				{
				}
				if (i == m_dataPerType[typeIndex].DistributionGroupsInUse)
				{
					return 0f;
				}
				return m_dataPerType[typeIndex].PhysicalDistributionGroups[i].SinkDataByPriority[priority].RemainingAvailableResource - m_dataPerType[typeIndex].PhysicalDistributionGroups[i].SinkDataByPriority[priority].RequiredInput;
			}
			float remainingAvailableResource = m_dataPerType[typeIndex].SinkDataByPriority[priority].RemainingAvailableResource;
			float requiredInput = m_dataPerType[typeIndex].SinkDataByPriority[priority].RequiredInput;
			return remainingAvailableResource - requiredInput;
		}

		private void source_HasRemainingCapacityChanged(MyDefinitionId changedResourceTypeId, MyResourceSourceComponent source)
		{
			int typeIndex = GetTypeIndex(ref changedResourceTypeId);
			m_dataPerType[typeIndex].NeedsRecompute = true;
			m_dataPerType[typeIndex].RemainingFuelTimeDirty = true;
		}

		public void ConveyorSystem_OnPoweredChanged()
		{
			MySandboxGame.Static.Invoke(delegate
			{
				for (int i = 0; i < m_dataPerType.Count; i++)
				{
					m_dataPerType[i].GroupsDirty = true;
					m_dataPerType[i].NeedsRecompute = true;
					m_dataPerType[i].RemainingFuelTimeDirty = true;
					m_dataPerType[i].SourcesEnabledDirty = true;
				}
			}, "ConveyorSystem_OnPoweredChanged");
		}

		private void source_MaxOutputChanged(MyDefinitionId changedResourceTypeId, float oldOutput, MyResourceSourceComponent obj)
		{
			int typeIndex = GetTypeIndex(ref changedResourceTypeId);
			m_dataPerType[typeIndex].NeedsRecompute = true;
			m_dataPerType[typeIndex].RemainingFuelTimeDirty = true;
			m_dataPerType[typeIndex].SourcesEnabledDirty = true;
			if (m_dataPerType[typeIndex].SourceCount == 1 && !MyEntities.IsAsyncUpdateInProgress)
			{
				RecomputeResourceDistribution(ref changedResourceTypeId);
			}
		}

		private void source_ProductionEnabledChanged(MyDefinitionId changedResourceTypeId, MyResourceSourceComponent obj)
		{
			int typeIndex = GetTypeIndex(ref changedResourceTypeId);
			m_dataPerType[typeIndex].NeedsRecompute = true;
			m_dataPerType[typeIndex].RemainingFuelTimeDirty = true;
			m_dataPerType[typeIndex].SourcesEnabledDirty = true;
			if (m_dataPerType[typeIndex].SourceCount == 1 && !MyEntities.IsAsyncUpdateInProgress)
			{
				RecomputeResourceDistribution(ref changedResourceTypeId);
			}
		}

		public float MaxAvailableResourceByType(MyDefinitionId resourceTypeId, MyCubeGrid grid = null)
		{
			if (!TryGetTypeIndex(ref resourceTypeId, out var typeIndex))
			{
				return 0f;
			}
			if (resourceTypeId == ElectricityId && grid != null)
			{
				MyElectricalDistributionGroup myElectricalDistributionGroup = m_dataPerType[typeIndex].ElectricalDistributionGroups[GetElectricalGroupIndex(ref resourceTypeId, grid)];
				if (myElectricalDistributionGroup.GridsEntityIds.Contains(grid.EntityId))
				{
					return myElectricalDistributionGroup.MaxAvailableResource;
				}
			}
			return m_dataPerType[typeIndex].MaxAvailableResource;
		}

		public float TotalRequiredInputByType(MyDefinitionId resourceTypeId, MyCubeGrid grid = null)
		{
<<<<<<< HEAD
			if (TryGetTypeIndex(ref resourceTypeId, out var typeIndex))
=======
			if (!TryGetTypeIndex(ref resourceTypeId, out var typeIndex))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (resourceTypeId == ElectricityId && grid != null)
				{
					return m_dataPerType[typeIndex].ElectricalDistributionGroups[GetElectricalGroupIndex(ref resourceTypeId, grid)].GetPowerInUse();
				}
				return m_dataPerType[typeIndex].SinkDataByPriority.Last().RequiredInputCumulative;
			}
<<<<<<< HEAD
			return 0f;
=======
			return Enumerable.Last<MySinkGroupData>((IEnumerable<MySinkGroupData>)m_dataPerType[typeIndex].SinkDataByPriority).RequiredInputCumulative;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void DebugDraw(MyEntity entity)
		{
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			if (!MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
			{
				return;
			}
			double num = 6.5 * 0.045;
			Vector3D vector3D = entity.WorldMatrix.Translation;
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				myCubeGrid.GetPhysicalGroupAABB();
				vector3D = myCubeGrid.GetPhysicalGroupAABB().Center;
			}
			Vector3D position = MySector.MainCamera.Position;
			Vector3D up = MySector.MainCamera.WorldMatrix.Up;
			Vector3D right = MySector.MainCamera.WorldMatrix.Right;
			double val = Vector3D.Distance(vector3D, position);
			float num2 = (float)Math.Atan(6.5 / Math.Max(val, 0.001));
			if (num2 <= 0.27f)
			{
				return;
			}
			MyRenderProxy.DebugDrawText3D(vector3D, entity.ToString(), Color.Yellow, num2, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
<<<<<<< HEAD
			if (m_initializedTypes == null || m_initializedTypes.Count == 0)
=======
			if (m_initializedTypes == null || m_initializedTypes.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			Vector3D vector3D2 = vector3D;
			int num3 = -1;
<<<<<<< HEAD
			foreach (MyDefinitionId initializedType in m_initializedTypes)
			{
				vector3D2 = vector3D + num3 * up * num;
				DebugDrawResource(initializedType, vector3D2, right, num2);
				num3--;
			}
			while (m_changesDebug.Count > 10)
			{
				m_changesDebug.RemoveAt(0);
			}
			num3--;
			vector3D2 = vector3D + num3 * up * num;
			MyRenderProxy.DebugDrawText3D(vector3D2 + right * 0.064999997615814209, "Recent changes:", Color.LightYellow, num2, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			num3--;
			foreach (string item in m_changesDebug)
			{
				vector3D2 = vector3D + num3 * up * num;
				MyRenderProxy.DebugDrawText3D(vector3D2 + right * 0.064999997615814209, item, Color.White, num2, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				num3--;
=======
			Enumerator<MyDefinitionId> enumerator = m_initializedTypes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDefinitionId current = enumerator.get_Current();
					vector3D2 = vector3D + num3 * up * num;
					DebugDrawResource(current, vector3D2, right, num2);
					num3--;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			while (m_changesDebug.Count > 10)
			{
				m_changesDebug.RemoveAt(0);
			}
			num3--;
			vector3D2 = vector3D + num3 * up * num;
			MyRenderProxy.DebugDrawText3D(vector3D2 + right * 0.064999997615814209, "Recent changes:", Color.LightYellow, num2, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			num3--;
			foreach (string item in m_changesDebug)
			{
				vector3D2 = vector3D + num3 * up * num;
				MyRenderProxy.DebugDrawText3D(vector3D2 + right * 0.064999997615814209, item, Color.White, num2, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				num3--;
			}
		}

		private void DebugDrawResource(MyDefinitionId resourceId, Vector3D origin, Vector3D rightVector, float textSize)
		{
			Vector3D vector3D = 0.05000000074505806 * rightVector;
			Vector3D worldCoord = origin + vector3D + rightVector * 0.014999999664723873;
			int value = 0;
			string text = resourceId.SubtypeName;
			if (m_typeIdToIndex.TryGetValue(resourceId, out value))
			{
				PerTypeData perTypeData = m_dataPerType[value];
				int num = 0;
				HashSet<MyResourceSinkComponent>[] sinksByPriority = perTypeData.SinksByPriority;
				foreach (HashSet<MyResourceSinkComponent> val in sinksByPriority)
				{
					num += val.get_Count();
				}
				text = $"{resourceId.SubtypeName} Sources:{perTypeData.SourceCount} Sinks:{num} Available:{perTypeData.MaxAvailableResource} State:{perTypeData.ResourceState}";
			}
			MyRenderProxy.DebugDrawLine3D(origin, origin + vector3D, Color.White, Color.White, depthRead: false);
			MyRenderProxy.DebugDrawText3D(worldCoord, text, Color.White, textSize, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
		}

		public void ClearData()
		{
			foreach (PerTypeData item in m_dataPerType)
			{
				item.ClearData();
			}
			m_sinksToAdd.Clear();
			m_sourcesToAdd.Clear();
			m_sinksToRemove.Clear();
			m_sourcesToRemove.Clear();
<<<<<<< HEAD
			this.OnPowerGenerationChanged = null;
		}

		public float MaxAvailableResourceByType(MyDefinitionId resourceTypeId, IMyCubeGrid grid)
		{
			return MaxAvailableResourceByType(resourceTypeId, (MyCubeGrid)grid);
		}

		public float TotalRequiredInputByType(MyDefinitionId resourceTypeId, IMyCubeGrid grid)
		{
			return TotalRequiredInputByType(resourceTypeId, (MyCubeGrid)grid);
=======
			OnPowerGenerationChanged = null;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
