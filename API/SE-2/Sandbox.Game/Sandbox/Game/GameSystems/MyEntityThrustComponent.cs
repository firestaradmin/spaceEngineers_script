using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	public abstract class MyEntityThrustComponent : MyEntityComponentBase
	{
		private class DirectionComparer : IEqualityComparer<Vector3I>
		{
			public bool Equals(Vector3I x, Vector3I y)
			{
				return x == y;
			}

			public int GetHashCode(Vector3I obj)
			{
				return obj.X + 8 * obj.Y + 64 * obj.Z;
			}
		}

		public class FuelTypeData
		{
			public Dictionary<Vector3I, HashSet<MyEntity>> ThrustsByDirection;

			public Dictionary<Vector3I, float> MaxRequirementsByDirection;

			public float CurrentRequiredFuelInput;

			public Vector3 MaxNegativeThrust;

			public Vector3 MaxPositiveThrust;

			public float MinRequiredPowerInput;

			public float MaxRequiredPowerInput;

			public int ThrustCount;

			public float Efficiency;

			public float EnergyDensity;

			public Vector3 CurrentThrust;

			public Vector3 ThrustOverride;

			public float ThrustOverridePower;
		}

		public class MyConveyorConnectedGroup
		{
			public readonly List<FuelTypeData> DataByFuelType;

			public readonly MyResourceSinkComponent ResourceSink;

			public int ThrustCount;

			public Vector3 MaxNegativeThrust;

			public Vector3 MaxPositiveThrust;

			public Vector3 ThrustOverride;

			public float ThrustOverridePower;

			public readonly List<MyDefinitionId> FuelTypes;

			public readonly Dictionary<MyDefinitionId, int> FuelTypeToIndex;

			public long LastPowerUpdate;

			public IMyConveyorEndpoint FirstEndpoint;

			public MyConveyorConnectedGroup(IMyConveyorEndpointBlock endpointBlock)
			{
				FirstEndpoint = endpointBlock.ConveyorEndpoint;
				DataByFuelType = new List<FuelTypeData>();
				ResourceSink = new MyResourceSinkComponent();
				LastPowerUpdate = MySession.Static.GameplayFrameCounter;
				FuelTypes = new List<MyDefinitionId>();
				FuelTypeToIndex = new Dictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);
			}

			public bool TryGetTypeIndex(ref MyDefinitionId fuelId, out int typeIndex)
			{
				typeIndex = 0;
				if (FuelTypeToIndex.Count > 1 && !FuelTypeToIndex.TryGetValue(fuelId, out typeIndex))
				{
					return false;
				}
				return FuelTypeToIndex.Count > 0;
			}

			public int GetTypeIndex(ref MyDefinitionId fuelId)
			{
				int result = 0;
				if (FuelTypeToIndex.Count > 1 && FuelTypeToIndex.TryGetValue(fuelId, out var value))
				{
					result = value;
				}
				return result;
			}
		}

		private static float MAX_DISTANCE_RELATIVE_DAMPENING = 100f;

		private static float MAX_DISTANCE_RELATIVE_DAMPENING_SQ = MAX_DISTANCE_RELATIVE_DAMPENING * MAX_DISTANCE_RELATIVE_DAMPENING;

		private static readonly DirectionComparer m_directionComparer = new DirectionComparer();

		protected float m_lastPlanetaryInfluence = -1f;

		protected bool m_lastPlanetaryInfluenceHasAtmosphere;

		protected float m_lastPlanetaryGravityMagnitude;

		private int m_nextPlanetaryInfluenceRecalculation = -1;

		private const int m_maxInfluenceRecalculationInterval = 10000;

		private Vector3 m_maxNegativeThrust;

		private Vector3 m_maxPositiveThrust;

		protected readonly List<FuelTypeData> m_dataByFuelType = new List<FuelTypeData>();

		private readonly MyResourceSinkComponent m_resourceSink;

		private Vector3 m_totalMaxNegativeThrust;

		private Vector3 m_totalMaxPositiveThrust;

		protected readonly List<MyDefinitionId> m_fuelTypes = new List<MyDefinitionId>();

		private readonly Dictionary<MyDefinitionId, int> m_fuelTypeToIndex = new Dictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);

		protected readonly List<MyConveyorConnectedGroup> m_connectedGroups = new List<MyConveyorConnectedGroup>();

		protected MyResourceSinkComponent m_lastSink;

		protected FuelTypeData m_lastFuelTypeData;

		protected MyConveyorConnectedGroup m_lastGroup;

		protected Vector3 m_totalThrustOverride;

		protected float m_totalThrustOverridePower;

		private readonly List<MyConveyorConnectedGroup> m_groupsToTrySplit = new List<MyConveyorConnectedGroup>();

		private bool m_mergeAllGroupsDirty = true;

		[ThreadStatic]
		private static List<int> m_tmpGroupIndicesPerThread;

		[ThreadStatic]
		private static List<MyTuple<MyEntity, Vector3I>> m_tmpEntitiesWithDirectionsPerThread;

		[ThreadStatic]
		private static List<MyConveyorConnectedGroup> m_tmpGroupsPerThread;

		protected readonly MyConcurrentQueue<MyTuple<MyEntity, Vector3I, Func<bool>>> m_thrustEntitiesPending = new MyConcurrentQueue<MyTuple<MyEntity, Vector3I, Func<bool>>>();

		protected readonly HashSet<MyEntity> m_thrustEntitiesRemovedBeforeRegister = new HashSet<MyEntity>();

		private MyConcurrentQueue<IMyConveyorEndpointBlock> m_conveyorEndpointsPending = new MyConcurrentQueue<IMyConveyorEndpointBlock>();

		private MyConcurrentQueue<IMyConveyorSegmentBlock> m_conveyorSegmentsPending = new MyConcurrentQueue<IMyConveyorSegmentBlock>();

		/// <summary>
		/// True whenever thrust was added or removed.
		/// </summary>
		protected bool m_thrustsChanged;

		private Vector3 m_controlThrust;

		private bool m_lastControlThrustChanged;

		private bool m_controlThrustChanged;

		private long m_lastPowerUpdate;

		private Vector3? m_maxThrustOverride;

		private bool m_secondFrameUpdate;

		private bool m_dampenersEnabledLastFrame = true;

		private bool m_recalculateConveyorsFired;

		private int m_counter;

		private bool m_enabled;

		private Vector3 m_autoPilotControlThrust;

		protected ListReader<MyConveyorConnectedGroup> ConnectedGroups => new ListReader<MyConveyorConnectedGroup>(m_connectedGroups);

		private static List<int> m_tmpGroupIndices => MyUtils.Init(ref m_tmpGroupIndicesPerThread);

		private static List<MyTuple<MyEntity, Vector3I>> m_tmpEntitiesWithDirections => MyUtils.Init(ref m_tmpEntitiesWithDirectionsPerThread);

		private static List<MyConveyorConnectedGroup> m_tmpGroups => MyUtils.Init(ref m_tmpGroupsPerThread);

		protected bool ControlThrustChanged
		{
			get
			{
				return m_controlThrustChanged;
			}
			set
			{
				m_controlThrustChanged = value;
			}
		}

		public Vector3? MaxThrustOverride
		{
			get
			{
				if (!MyFakes.ENABLE_VR_REMOTE_CONTROL_WAYPOINTS_FAST_MOVEMENT)
				{
					return null;
				}
				return m_maxThrustOverride;
			}
			set
			{
				m_maxThrustOverride = value;
			}
		}

		public new MyEntity Entity => base.Entity as MyEntity;

		public float MaxRequiredPowerInput { get; private set; }

		public float MinRequiredPowerInput { get; private set; }

<<<<<<< HEAD
		/// <summary>
		/// For now just the maximum slowdown factor of any thruster registered to the component
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public float SlowdownFactor { get; set; }

		public int ThrustCount { get; private set; }

		public bool DampenersEnabled { get; set; }

		/// <summary>
		/// Torque and thrust wanted by player (from input).
		/// </summary>
		public Vector3 ControlThrust
		{
			get
			{
				return m_controlThrust;
			}
			set
			{
				if (value != m_controlThrust)
				{
					m_controlThrustChanged = true;
					m_controlThrust = value;
					OnControlTrustChanged();
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Final thrust (clamped by available power, added anti-gravity, slowdown).
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public Vector3 FinalThrust { get; private set; }

		/// <summary>
		/// Thrust wanted by AutoPilot
		/// </summary>
		public Vector3 AutoPilotControlThrust
		{
			get
			{
				return m_autoPilotControlThrust;
			}
			set
			{
				lock (m_dataByFuelType)
				{
					if (value != m_autoPilotControlThrust)
					{
						m_autoPilotControlThrust = value;
						m_controlThrustChanged = true;
						OnControlTrustChanged();
					}
				}
			}
		}

		public bool AutopilotEnabled { get; set; }

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

		public bool IsDirty
		{
			get
			{
				if (!m_thrustsChanged)
				{
					return m_controlThrustChanged;
				}
				return true;
			}
		}

		public override string ComponentTypeDebugString => "Thrust Component";

		public bool HasPower => m_resourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

		public bool HasThrust
		{
			get
			{
				if (Vector3.IsZero(m_totalMaxPositiveThrust))
				{
					return !Vector3.IsZero(m_totalMaxNegativeThrust);
				}
				return true;
			}
		}

		private int InitializeType(MyDefinitionId fuelType, List<FuelTypeData> dataByTypeList, List<MyDefinitionId> fuelTypeList, Dictionary<MyDefinitionId, int> fuelTypeToIndex, MyResourceSinkComponent resourceSink)
		{
			dataByTypeList.Add(new FuelTypeData
			{
				ThrustsByDirection = new Dictionary<Vector3I, HashSet<MyEntity>>(6, m_directionComparer),
				MaxRequirementsByDirection = new Dictionary<Vector3I, float>(6, m_directionComparer),
				CurrentRequiredFuelInput = 0.0001f,
				Efficiency = 0f,
				EnergyDensity = 0f
			});
			int typeIndex = dataByTypeList.Count - 1;
			fuelTypeToIndex.Add(fuelType, typeIndex);
			fuelTypeList.Add(fuelType);
			Vector3I[] intDirections = Base6Directions.IntDirections;
			foreach (Vector3I key in intDirections)
			{
				dataByTypeList[typeIndex].ThrustsByDirection[key] = new HashSet<MyEntity>();
			}
			MyResourceSinkInfo myResourceSinkInfo = default(MyResourceSinkInfo);
			myResourceSinkInfo.ResourceTypeId = fuelType;
			myResourceSinkInfo.MaxRequiredInput = 0f;
			myResourceSinkInfo.RequiredInputFunc = () => RequiredFuelInput(dataByTypeList[typeIndex]);
			MyResourceSinkInfo sinkData = myResourceSinkInfo;
			if (fuelTypeList.Count == 1)
			{
				resourceSink.Init(MyStringHash.GetOrCompute("Thrust"), sinkData, null);
				resourceSink.IsPoweredChanged += Sink_IsPoweredChanged;
				resourceSink.CurrentInputChanged += Sink_CurrentInputChanged;
				AddSinkToSystems(resourceSink, base.Container.Entity as MyCubeGrid);
			}
			else
			{
				resourceSink.AddType(ref sinkData);
			}
			return typeIndex;
		}

		protected MyEntityThrustComponent()
		{
			MyResourceDistributorComponent.InitializeMappings();
			m_resourceSink = new MyResourceSinkComponent();
		}

		public virtual void Init()
		{
			Enabled = true;
			ThrustCount = 0;
			DampenersEnabled = true;
			m_lastPowerUpdate = MySession.Static.GameplayFrameCounter;
		}

		public virtual void Register(MyEntity entity, Vector3I forwardVector, Func<bool> onRegisteredCallback = null)
		{
			MyDefinitionId typeId = FuelType(entity);
			int num = -1;
			int typeIndex = -1;
			IMyConveyorEndpointBlock myConveyorEndpointBlock = entity as IMyConveyorEndpointBlock;
			Dictionary<Vector3I, HashSet<MyEntity>> thrustsByDirection;
			MyResourceSinkComponent resourceSink;
			if (MyResourceDistributorComponent.IsConveyorConnectionRequiredTotal(ref typeId) && myConveyorEndpointBlock != null)
			{
				FindConnectedGroups(myConveyorEndpointBlock, m_connectedGroups, m_tmpGroupIndices);
				MyConveyorConnectedGroup myConveyorConnectedGroup;
				if (m_tmpGroupIndices.Count >= 1)
				{
					if (m_tmpGroupIndices.Count > 1)
					{
						MergeGroups(m_connectedGroups, m_tmpGroupIndices);
					}
					num = m_tmpGroupIndices[0];
					myConveyorConnectedGroup = m_connectedGroups[num];
				}
				else
				{
					myConveyorConnectedGroup = new MyConveyorConnectedGroup(myConveyorEndpointBlock);
					m_connectedGroups.Add(myConveyorConnectedGroup);
					num = m_connectedGroups.Count - 1;
				}
				if (!myConveyorConnectedGroup.TryGetTypeIndex(ref typeId, out typeIndex))
				{
					typeIndex = InitializeType(typeId, myConveyorConnectedGroup.DataByFuelType, myConveyorConnectedGroup.FuelTypes, myConveyorConnectedGroup.FuelTypeToIndex, myConveyorConnectedGroup.ResourceSink);
					if (myConveyorConnectedGroup.FuelTypes.Count == 1)
					{
						entity.Components.Add(myConveyorConnectedGroup.ResourceSink);
					}
				}
				myConveyorConnectedGroup.ThrustCount++;
				myConveyorConnectedGroup.DataByFuelType[typeIndex].ThrustCount++;
				thrustsByDirection = myConveyorConnectedGroup.DataByFuelType[typeIndex].ThrustsByDirection;
				resourceSink = myConveyorConnectedGroup.ResourceSink;
				m_tmpGroupIndices.Clear();
			}
			else
			{
				if (!TryGetTypeIndex(ref typeId, out typeIndex))
				{
					typeIndex = InitializeType(typeId, m_dataByFuelType, m_fuelTypes, m_fuelTypeToIndex, m_resourceSink);
					if (m_fuelTypes.Count == 1)
					{
						entity.Components.Add(m_resourceSink);
					}
				}
				else
				{
					entity.Components.Remove<MyResourceSinkComponent>();
				}
				thrustsByDirection = m_dataByFuelType[typeIndex].ThrustsByDirection;
				resourceSink = m_resourceSink;
				m_dataByFuelType[typeIndex].ThrustCount++;
			}
			m_lastSink = resourceSink;
			m_lastGroup = ((num == -1) ? null : m_connectedGroups[num]);
			m_lastFuelTypeData = ((num == -1) ? m_dataByFuelType[typeIndex] : m_connectedGroups[num].DataByFuelType[typeIndex]);
			thrustsByDirection[forwardVector].Add(entity);
			ThrustCount++;
			MarkDirty();
		}

		protected virtual bool RegisterLazy(MyEntity entity, Vector3I forwardVector, Func<bool> onRegisteredCallback)
		{
			return true;
		}

		public bool IsRegistered(MyEntity entity, Vector3I forwardVector)
		{
			bool result = false;
			MyDefinitionId typeId = FuelType(entity);
			IMyConveyorEndpointBlock myConveyorEndpointBlock = entity as IMyConveyorEndpointBlock;
			if (MyResourceDistributorComponent.IsConveyorConnectionRequiredTotal(ref typeId) && myConveyorEndpointBlock != null)
			{
				foreach (MyConveyorConnectedGroup connectedGroup in m_connectedGroups)
				{
					if (connectedGroup.TryGetTypeIndex(ref typeId, out var typeIndex) && connectedGroup.DataByFuelType[typeIndex].ThrustsByDirection[forwardVector].Contains(entity))
					{
						return true;
					}
				}
				return result;
			}
			if (TryGetTypeIndex(ref typeId, out var typeIndex2))
			{
				result = m_dataByFuelType[typeIndex2].ThrustsByDirection[forwardVector].Contains(entity);
			}
			return result;
		}

		public virtual void Unregister(MyEntity entity, Vector3I forwardVector)
		{
			if (entity == null || Entity == null || Entity.MarkedForClose)
			{
				return;
			}
			if (!IsRegistered(entity, forwardVector))
			{
				m_thrustEntitiesRemovedBeforeRegister.Add(entity);
				return;
			}
			Dictionary<Vector3I, HashSet<MyEntity>> dictionary = null;
			int num = 0;
			MyResourceSinkComponent myResourceSinkComponent = null;
			MyDefinitionId typeId = FuelType(entity);
			List<FuelTypeData> list = null;
			int num2 = -1;
			int typeIndex = -1;
			IMyConveyorEndpointBlock myConveyorEndpointBlock = entity as IMyConveyorEndpointBlock;
			if (MyResourceDistributorComponent.IsConveyorConnectionRequiredTotal(ref typeId) && myConveyorEndpointBlock != null)
			{
				MyConveyorConnectedGroup myConveyorConnectedGroup = TrySplitGroup(myConveyorEndpointBlock);
				if (!myConveyorConnectedGroup.TryGetTypeIndex(ref typeId, out typeIndex))
				{
					return;
				}
				if (myConveyorConnectedGroup.DataByFuelType[typeIndex].ThrustsByDirection[forwardVector].Contains(entity))
				{
					num = --myConveyorConnectedGroup.ThrustCount;
					myResourceSinkComponent = myConveyorConnectedGroup.ResourceSink;
					dictionary = myConveyorConnectedGroup.DataByFuelType[typeIndex].ThrustsByDirection;
					list = myConveyorConnectedGroup.DataByFuelType;
					for (int i = 0; i < m_connectedGroups.Count; i++)
					{
						if (m_connectedGroups[i] == myConveyorConnectedGroup)
						{
							num2 = i;
							break;
						}
					}
				}
			}
			else
			{
				if (!TryGetTypeIndex(ref typeId, out typeIndex))
				{
					return;
				}
				myResourceSinkComponent = m_resourceSink;
				dictionary = m_dataByFuelType[typeIndex].ThrustsByDirection;
				list = m_dataByFuelType;
				num = 0;
				foreach (FuelTypeData item in m_dataByFuelType)
				{
					num += item.ThrustCount;
				}
				num = Math.Max(num - 1, 0);
			}
			if (dictionary == null)
			{
				return;
			}
			MyConveyorConnectedGroup myConveyorConnectedGroup2 = ((num2 != -1) ? m_connectedGroups[num2] : null);
			MoveSinkToNewEntity(entity, list, typeIndex, num, myResourceSinkComponent, myConveyorConnectedGroup2);
			dictionary[forwardVector].Remove(entity);
			myResourceSinkComponent.SetMaxRequiredInputByType(typeId, myResourceSinkComponent.MaxRequiredInputByType(typeId) - PowerAmountToFuel(ref typeId, MaxPowerConsumption(entity), myConveyorConnectedGroup2));
			if (--list[typeIndex].ThrustCount == 0)
			{
				list.RemoveAtFast(typeIndex);
				if (myConveyorConnectedGroup2 != null)
				{
					myConveyorConnectedGroup2.FuelTypes.RemoveAtFast(typeIndex);
					myConveyorConnectedGroup2.FuelTypeToIndex.Remove(typeId);
				}
				else
				{
					m_fuelTypes.RemoveAtFast(typeIndex);
					m_fuelTypeToIndex.Remove(typeId);
				}
			}
			if (num == 0)
			{
				RemoveSinkFromSystems(myResourceSinkComponent, base.Container.Entity as MyCubeGrid);
				if (num2 != -1)
				{
					m_connectedGroups.RemoveAt(num2);
				}
			}
			(entity as MyThrust)?.ClearThrustComponent();
			ThrustCount--;
		}

		private void MoveSinkToNewEntity(MyEntity entity, List<FuelTypeData> fuelData, int typeIndex, int thrustsLeftInGroup, MyResourceSinkComponent resourceSink, MyConveyorConnectedGroup containingGroup)
		{
<<<<<<< HEAD
=======
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!(base.Container.Entity is MyCubeGrid) || entity.Components.Get<MyResourceSinkComponent>() != resourceSink)
			{
				return;
			}
			entity.Components.Remove<MyResourceSinkComponent>();
			if (thrustsLeftInGroup <= 0)
<<<<<<< HEAD
			{
				return;
			}
			foreach (HashSet<MyEntity> value in fuelData[typeIndex].ThrustsByDirection.Values)
			{
				if (value.Count <= 0)
=======
			{
				return;
			}
			foreach (HashSet<MyEntity> value in fuelData[typeIndex].ThrustsByDirection.Values)
			{
				if (value.get_Count() <= 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					continue;
				}
				bool flag = false;
<<<<<<< HEAD
				foreach (MyEntity item in value)
				{
					if (item != entity)
					{
						item.Components.Add(resourceSink);
						AddSinkToSystems(resourceSink, Entity as MyCubeGrid);
						flag = true;
						if (containingGroup != null)
						{
							containingGroup.FirstEndpoint = (item as IMyConveyorEndpointBlock).ConveyorEndpoint;
=======
				Enumerator<MyEntity> enumerator2 = value.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MyEntity current2 = enumerator2.get_Current();
						if (current2 != entity)
						{
							current2.Components.Add(resourceSink);
							AddSinkToSystems(resourceSink, Entity as MyCubeGrid);
							flag = true;
							if (containingGroup != null)
							{
								containingGroup.FirstEndpoint = (current2 as IMyConveyorEndpointBlock).ConveyorEndpoint;
							}
							break;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						break;
					}
				}
<<<<<<< HEAD
=======
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (flag)
				{
					break;
				}
			}
		}

		private void MergeGroups(List<MyConveyorConnectedGroup> groups, List<int> connectedGroupIndices)
		{
<<<<<<< HEAD
			if (connectedGroupIndices == null || groups == null)
			{
				return;
			}
=======
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			int num = int.MinValue;
			int num2 = int.MinValue;
			foreach (int connectedGroupIndex in connectedGroupIndices)
			{
				MyConveyorConnectedGroup myConveyorConnectedGroup = groups[connectedGroupIndex];
				if (myConveyorConnectedGroup.ThrustCount > num2)
				{
					num = connectedGroupIndex;
					num2 = myConveyorConnectedGroup.ThrustCount;
				}
			}
			MyConveyorConnectedGroup myConveyorConnectedGroup2 = groups[num];
			foreach (int connectedGroupIndex2 in connectedGroupIndices)
			{
				if (connectedGroupIndex2 == num)
<<<<<<< HEAD
				{
					continue;
				}
				MyConveyorConnectedGroup myConveyorConnectedGroup3 = groups[connectedGroupIndex2];
				foreach (MyDefinitionId fuelType in myConveyorConnectedGroup3.FuelTypes)
				{
=======
				{
					continue;
				}
				MyConveyorConnectedGroup myConveyorConnectedGroup3 = groups[connectedGroupIndex2];
				foreach (MyDefinitionId fuelType in myConveyorConnectedGroup3.FuelTypes)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyDefinitionId fuelId = fuelType;
					if (!myConveyorConnectedGroup2.TryGetTypeIndex(ref fuelId, out var typeIndex))
					{
						typeIndex = InitializeType(fuelType, myConveyorConnectedGroup2.DataByFuelType, myConveyorConnectedGroup2.FuelTypes, myConveyorConnectedGroup2.FuelTypeToIndex, myConveyorConnectedGroup2.ResourceSink);
					}
					FuelTypeData fuelTypeData = myConveyorConnectedGroup2.DataByFuelType[typeIndex];
					FuelTypeData fuelTypeData2 = myConveyorConnectedGroup3.DataByFuelType[typeIndex];
					fuelTypeData.MaxRequiredPowerInput += fuelTypeData2.MaxRequiredPowerInput;
					fuelTypeData.MinRequiredPowerInput += fuelTypeData2.MinRequiredPowerInput;
					fuelTypeData.CurrentRequiredFuelInput += fuelTypeData2.CurrentRequiredFuelInput;
					fuelTypeData.MaxNegativeThrust += fuelTypeData2.MaxNegativeThrust;
					fuelTypeData.MaxPositiveThrust += fuelTypeData2.MaxPositiveThrust;
					fuelTypeData.ThrustOverride += fuelTypeData2.ThrustOverride;
					fuelTypeData.ThrustOverridePower += fuelTypeData2.ThrustOverridePower;
					fuelTypeData.ThrustCount += fuelTypeData2.ThrustCount;
					Vector3I[] intDirections = Base6Directions.IntDirections;
					foreach (Vector3I key in intDirections)
					{
						if (fuelTypeData2.MaxRequirementsByDirection.TryGetValue(key, out var value))
						{
							if (fuelTypeData.MaxRequirementsByDirection.TryGetValue(key, out var value2))
<<<<<<< HEAD
							{
								fuelTypeData.MaxRequirementsByDirection[key] = value2 + value;
							}
							else
							{
								fuelTypeData.MaxRequirementsByDirection[key] = value;
							}
						}
						if (!fuelTypeData.ThrustsByDirection.ContainsKey(key))
						{
							fuelTypeData.ThrustsByDirection[key] = new HashSet<MyEntity>();
						}
						HashSet<MyEntity> hashSet = fuelTypeData.ThrustsByDirection[key];
						if (!fuelTypeData2.ThrustsByDirection.ContainsKey(key))
						{
							continue;
						}
						foreach (MyEntity item in fuelTypeData2.ThrustsByDirection[key])
						{
							hashSet.Add(item);
							item.Components.Remove<MyResourceSinkComponent>();
=======
							{
								fuelTypeData.MaxRequirementsByDirection[key] = value2 + value;
							}
							else
							{
								fuelTypeData.MaxRequirementsByDirection[key] = value;
							}
						}
						if (!fuelTypeData.ThrustsByDirection.ContainsKey(key))
						{
							fuelTypeData.ThrustsByDirection[key] = new HashSet<MyEntity>();
						}
						HashSet<MyEntity> val = fuelTypeData.ThrustsByDirection[key];
						if (!fuelTypeData2.ThrustsByDirection.ContainsKey(key))
						{
							continue;
						}
						Enumerator<MyEntity> enumerator3 = fuelTypeData2.ThrustsByDirection[key].GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								MyEntity current4 = enumerator3.get_Current();
								val.Add(current4);
								current4.Components.Remove<MyResourceSinkComponent>();
							}
						}
						finally
						{
							((IDisposable)enumerator3).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					myConveyorConnectedGroup2.ThrustCount += myConveyorConnectedGroup3.ThrustCount;
					myConveyorConnectedGroup2.ThrustOverride += myConveyorConnectedGroup3.ThrustOverride;
					myConveyorConnectedGroup2.ThrustOverridePower += myConveyorConnectedGroup3.ThrustOverridePower;
					myConveyorConnectedGroup2.MaxNegativeThrust += myConveyorConnectedGroup3.MaxNegativeThrust;
					myConveyorConnectedGroup2.MaxPositiveThrust += myConveyorConnectedGroup3.MaxPositiveThrust;
					RemoveSinkFromSystems(myConveyorConnectedGroup3.ResourceSink, base.Container.Entity as MyCubeGrid);
				}
			}
			connectedGroupIndices.Sort();
			for (int num3 = connectedGroupIndices.Count - 1; num3 >= 0; num3--)
			{
				if (connectedGroupIndices[num3] != num)
				{
					if (connectedGroupIndices[num3] < num)
					{
						num--;
					}
					groups.RemoveAtFast(connectedGroupIndices[num3]);
					connectedGroupIndices.RemoveAt(num3);
				}
			}
			myConveyorConnectedGroup2.ResourceSink.Update();
			connectedGroupIndices[0] = num;
		}

		public void MergeAllGroupsDirty()
		{
			m_mergeAllGroupsDirty = true;
		}

		private void TryMergeAllGroups()
		{
			if (m_connectedGroups == null || m_connectedGroups.Count == 0)
			{
				return;
			}
			int num = 0;
			do
			{
				MyConveyorConnectedGroup myConveyorConnectedGroup = m_connectedGroups[num];
				IMyConveyorEndpointBlock myConveyorEndpointBlock = ((myConveyorConnectedGroup.FirstEndpoint != null) ? (myConveyorConnectedGroup.FirstEndpoint.CubeBlock as IMyConveyorEndpointBlock) : null);
				if (myConveyorEndpointBlock != null)
				{
					FindConnectedGroups(myConveyorEndpointBlock, m_connectedGroups, m_tmpGroupIndices);
					if (m_tmpGroupIndices.Count > 1)
					{
						MergeGroups(m_connectedGroups, m_tmpGroupIndices);
						num--;
					}
					m_tmpGroupIndices.Clear();
					num++;
				}
			}
			while (num < m_connectedGroups.Count);
		}

		private static void FindConnectedGroups(IMyConveyorSegmentBlock block, List<MyConveyorConnectedGroup> groups, List<int> outConnectedGroupIndices)
		{
			if (groups == null || outConnectedGroupIndices == null || outConnectedGroupIndices.Count == 0 || block.ConveyorSegment.ConveyorLine == null)
			{
				return;
			}
			IMyConveyorEndpoint myConveyorEndpoint = block.ConveyorSegment.ConveyorLine.GetEndpoint(0) ?? block.ConveyorSegment.ConveyorLine.GetEndpoint(1);
			if (myConveyorEndpoint == null)
			{
				return;
			}
			for (int i = 0; i < groups.Count; i++)
			{
				if (MyGridConveyorSystem.Reachable(groups[i].FirstEndpoint, myConveyorEndpoint))
				{
					outConnectedGroupIndices.Add(i);
				}
			}
		}

		private static void FindConnectedGroups(IMyConveyorEndpointBlock block, List<MyConveyorConnectedGroup> groups, List<int> outConnectedGroupIndices)
		{
			if (groups == null || block?.ConveyorEndpoint == null || outConnectedGroupIndices == null || outConnectedGroupIndices.Count == 0)
			{
				return;
			}
			for (int i = 0; i < groups.Count; i++)
			{
				MyConveyorConnectedGroup myConveyorConnectedGroup = groups[i];
				if (myConveyorConnectedGroup.FirstEndpoint != null && MyGridConveyorSystem.Reachable(myConveyorConnectedGroup.FirstEndpoint, block.ConveyorEndpoint) && MyGridConveyorSystem.Reachable(block.ConveyorEndpoint, myConveyorConnectedGroup.FirstEndpoint))
				{
					outConnectedGroupIndices.Add(i);
				}
			}
		}

		/// <summary>
		/// Tries to split the group containing the given block at the position of the block. Leaves the block in one of the new groups (or the old one if no splits happened) and returns that group
		/// If conveyorEndpointBlock, it uses groupOverride for the group instead.
		/// </summary>
		private MyConveyorConnectedGroup TrySplitGroup(IMyConveyorEndpointBlock conveyorEndpointBlock, MyConveyorConnectedGroup groupOverride = null)
		{
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			MyConveyorConnectedGroup myConveyorConnectedGroup = null;
			MyEntity myEntity = conveyorEndpointBlock as MyEntity;
			myConveyorConnectedGroup = groupOverride ?? FindEntityGroup(myEntity);
			if (myConveyorConnectedGroup == null)
			{
				return null;
			}
			if (conveyorEndpointBlock != null && conveyorEndpointBlock.ConveyorEndpoint == myConveyorConnectedGroup.FirstEndpoint)
			{
				if (myConveyorConnectedGroup.ThrustCount == 1)
				{
					return myConveyorConnectedGroup;
				}
				foreach (FuelTypeData item3 in myConveyorConnectedGroup.DataByFuelType)
				{
					bool flag = false;
					foreach (HashSet<MyEntity> value in item3.ThrustsByDirection.Values)
					{
						Enumerator<MyEntity> enumerator3 = value.GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								MyEntity current2 = enumerator3.get_Current();
								if (current2 != myEntity)
								{
									myConveyorConnectedGroup.FirstEndpoint = (current2 as IMyConveyorEndpointBlock).ConveyorEndpoint;
									myEntity.Components.Remove<MyResourceSinkComponent>();
									current2.Components.Add(myConveyorConnectedGroup.ResourceSink);
									AddSinkToSystems(myConveyorConnectedGroup.ResourceSink, Entity as MyCubeGrid);
									flag = true;
									break;
								}
							}
						}
						finally
						{
							((IDisposable)enumerator3).Dispose();
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			Vector3I[] intDirections = Base6Directions.IntDirections;
			foreach (Vector3I vector3I in intDirections)
			{
				for (int j = 0; j < myConveyorConnectedGroup.FuelTypes.Count; j++)
				{
					FuelTypeData fuelTypeData = myConveyorConnectedGroup.DataByFuelType[j];
					Enumerator<MyEntity> enumerator3 = fuelTypeData.ThrustsByDirection[vector3I].GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							MyEntity current3 = enumerator3.get_Current();
							IMyConveyorEndpoint conveyorEndpoint = (current3 as IMyConveyorEndpointBlock).ConveyorEndpoint;
							if (myEntity != current3 && !MyGridConveyorSystem.Reachable(conveyorEndpoint, myConveyorConnectedGroup.FirstEndpoint))
							{
								MyDefinitionId fuelType = FuelType(current3);
								myConveyorConnectedGroup.ResourceSink.SetMaxRequiredInputByType(fuelType, myConveyorConnectedGroup.ResourceSink.MaxRequiredInputByType(fuelType) - PowerAmountToFuel(ref fuelType, MaxPowerConsumption(current3), myConveyorConnectedGroup));
								fuelTypeData.ThrustCount--;
								myConveyorConnectedGroup.ThrustCount--;
								m_tmpEntitiesWithDirections.Add(new MyTuple<MyEntity, Vector3I>(current3, vector3I));
							}
						}
					}
					finally
					{
						((IDisposable)enumerator3).Dispose();
					}
					foreach (MyTuple<MyEntity, Vector3I> tmpEntitiesWithDirection in m_tmpEntitiesWithDirections)
					{
						fuelTypeData.ThrustsByDirection[tmpEntitiesWithDirection.Item2].Remove(tmpEntitiesWithDirection.Item1);
						RemoveFromGroup(tmpEntitiesWithDirection.Item1, myConveyorConnectedGroup);
					}
				}
			}
			foreach (MyTuple<MyEntity, Vector3I> tmpEntitiesWithDirection2 in m_tmpEntitiesWithDirections)
			{
				MyEntity item = tmpEntitiesWithDirection2.Item1;
				Vector3I item2 = tmpEntitiesWithDirection2.Item2;
				MyDefinitionId fuelId = FuelType(item);
				bool flag2 = false;
				foreach (MyConveyorConnectedGroup tmpGroup in m_tmpGroups)
				{
					if (MyGridConveyorSystem.Reachable((item as IMyConveyorEndpointBlock).ConveyorEndpoint, tmpGroup.FirstEndpoint))
					{
						if (!tmpGroup.TryGetTypeIndex(ref fuelId, out var typeIndex))
						{
							typeIndex = InitializeType(fuelId, tmpGroup.DataByFuelType, tmpGroup.FuelTypes, tmpGroup.FuelTypeToIndex, tmpGroup.ResourceSink);
						}
						FuelTypeData fuelTypeData2 = tmpGroup.DataByFuelType[typeIndex];
						fuelTypeData2.ThrustsByDirection[item2].Add(item);
						AddToGroup(item, tmpGroup);
						fuelTypeData2.ThrustCount++;
						tmpGroup.ThrustCount++;
						tmpGroup.ResourceSink.SetMaxRequiredInputByType(fuelId, tmpGroup.ResourceSink.MaxRequiredInputByType(fuelId) + PowerAmountToFuel(ref fuelId, MaxPowerConsumption(item), tmpGroup));
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					MyConveyorConnectedGroup myConveyorConnectedGroup2 = new MyConveyorConnectedGroup(item as IMyConveyorEndpointBlock);
					m_tmpGroups.Add(myConveyorConnectedGroup2);
					m_connectedGroups.Add(myConveyorConnectedGroup2);
					int index = InitializeType(fuelId, myConveyorConnectedGroup2.DataByFuelType, myConveyorConnectedGroup2.FuelTypes, myConveyorConnectedGroup2.FuelTypeToIndex, myConveyorConnectedGroup2.ResourceSink);
					item.Components.Add(myConveyorConnectedGroup2.ResourceSink);
					FuelTypeData fuelTypeData3 = myConveyorConnectedGroup2.DataByFuelType[index];
					int typeIndex2 = myConveyorConnectedGroup.GetTypeIndex(ref fuelId);
					fuelTypeData3.Efficiency = myConveyorConnectedGroup.DataByFuelType[typeIndex2].Efficiency;
					fuelTypeData3.EnergyDensity = myConveyorConnectedGroup.DataByFuelType[typeIndex2].EnergyDensity;
					fuelTypeData3.ThrustsByDirection[item2].Add(item);
					AddToGroup(item, myConveyorConnectedGroup2);
					fuelTypeData3.ThrustCount++;
					myConveyorConnectedGroup2.ThrustCount++;
					myConveyorConnectedGroup2.ResourceSink.SetMaxRequiredInputByType(fuelId, myConveyorConnectedGroup2.ResourceSink.MaxRequiredInputByType(fuelId) + PowerAmountToFuel(ref fuelId, MaxPowerConsumption(item), myConveyorConnectedGroup2));
				}
			}
			m_tmpGroups.Clear();
			m_tmpEntitiesWithDirections.Clear();
			return myConveyorConnectedGroup;
		}

		private static void AddSinkToSystems(MyResourceSinkComponent resourceSink, MyCubeGrid cubeGrid)
		{
			if (cubeGrid != null)
			{
				MyCubeGridSystems gridSystems = cubeGrid.GridSystems;
				if (gridSystems != null && gridSystems.ResourceDistributor != null)
				{
					gridSystems.ResourceDistributor.AddSink(resourceSink);
				}
			}
		}

		private static void RemoveSinkFromSystems(MyResourceSinkComponentBase resourceSink, MyCubeGrid cubeGrid)
		{
			if (cubeGrid != null)
			{
				MyCubeGridSystems gridSystems = cubeGrid.GridSystems;
				if (gridSystems != null && gridSystems.ResourceDistributor != null)
				{
					gridSystems.ResourceDistributor.RemoveSink(resourceSink as MyResourceSinkComponent);
				}
			}
		}

		private void Sink_CurrentInputChanged(MyDefinitionId resourceTypeId, float oldInput, MyResourceSinkComponent sink)
		{
			m_controlThrustChanged = true;
		}

		private void Sink_IsPoweredChanged()
		{
			MarkDirty();
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			MyPlanets.Static.OnPlanetAdded += OnPlanetAddedOrRemoved;
			MyPlanets.Static.OnPlanetRemoved += OnPlanetAddedOrRemoved;
<<<<<<< HEAD
			Entity.OnTeleported += OnTeleport;
=======
			Entity.OnTeleport += OnTeleport;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MarkDirty();
			MyCubeGrid myCubeGrid = Entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				myCubeGrid.OnBlockAdded += CubeGrid_OnBlockAdded;
				myCubeGrid.GridSystems.ConveyorSystem.OnBeforeRemoveSegmentBlock += ConveyorSystem_OnBeforeRemoveSegmentBlock;
				myCubeGrid.GridSystems.ConveyorSystem.OnBeforeRemoveEndpointBlock += ConveyorSystem_OnBeforeRemoveEndpointBlock;
				myCubeGrid.GridSystems.ConveyorSystem.ResourceSink.IsPoweredChanged += ConveyorSystem_OnPoweredChanged;
			}
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			MyPlanets.Static.OnPlanetAdded -= OnPlanetAddedOrRemoved;
			MyPlanets.Static.OnPlanetRemoved -= OnPlanetAddedOrRemoved;
			if (base.Container.Entity != null)
			{
				((MyEntity)base.Container.Entity).OnTeleported -= OnTeleport;
			}
			foreach (MyConveyorConnectedGroup connectedGroup in m_connectedGroups)
			{
				RemoveSinkFromSystems(connectedGroup.ResourceSink, base.Container.Entity as MyCubeGrid);
			}
			RemoveSinkFromSystems(m_resourceSink, base.Container.Entity as MyCubeGrid);
			MyCubeGrid myCubeGrid = Entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				myCubeGrid.OnBlockAdded -= CubeGrid_OnBlockAdded;
				myCubeGrid.GridSystems.ConveyorSystem.OnBeforeRemoveSegmentBlock -= ConveyorSystem_OnBeforeRemoveSegmentBlock;
				myCubeGrid.GridSystems.ConveyorSystem.OnBeforeRemoveEndpointBlock -= ConveyorSystem_OnBeforeRemoveEndpointBlock;
				myCubeGrid.GridSystems.ConveyorSystem.ResourceSink.IsPoweredChanged -= ConveyorSystem_OnPoweredChanged;
			}
		}

		public virtual void UpdateBeforeSimulation(bool updateDampeners, MyEntity relativeDampeningEntity)
		{
			if (Entity == null)
			{
				return;
			}
			if (Entity.InScene)
			{
				UpdateConveyorSystemChanges();
				if (!m_recalculateConveyorsFired)
				{
					m_recalculateConveyorsFired = true;
					m_mergeAllGroupsDirty = true;
				}
			}
			if (ThrustCount == 0)
			{
				Entity.Components.Remove<MyEntityThrustComponent>();
				return;
			}
			if (MySession.Static.GameplayFrameCounter >= m_nextPlanetaryInfluenceRecalculation)
			{
				RecalculatePlanetaryInfluence();
			}
			if (m_thrustsChanged)
			{
				RecomputeThrustParameters();
				if (Entity is MyCubeGrid && Entity.Physics != null && !Entity.Physics.RigidBody.IsActive)
				{
					(Entity as MyCubeGrid).ActivatePhysics();
				}
			}
			if (Enabled && Entity.Physics != null)
			{
				MatrixD worldMatrixNormalizedInv = Entity.PositionComp.WorldMatrixNormalizedInv;
				Vector3 normal = ((relativeDampeningEntity != null && relativeDampeningEntity.Physics != null) ? (relativeDampeningEntity.Physics.LinearVelocity + 30f * relativeDampeningEntity.Physics.LinearAcceleration * 0.0166666675f) : Vector3.Zero);
				normal = Vector3.TransformNormal(normal, worldMatrixNormalizedInv);
				if (normal.LengthSquared() > 0f && Entity is MyCubeGrid && Entity.Physics != null && !Entity.Physics.RigidBody.IsActive)
				{
					(Entity as MyCubeGrid).ActivatePhysics();
				}
				UpdateThrusts(updateDampeners, normal);
				if (m_thrustsChanged)
				{
					RecomputeThrustParameters();
				}
			}
			if (!DampenersEnabled && m_dampenersEnabledLastFrame)
			{
				foreach (MyConveyorConnectedGroup connectedGroup in m_connectedGroups)
				{
					if (connectedGroup.DataByFuelType.Count > 0)
					{
						TurnOffThrusterFlame(connectedGroup.DataByFuelType);
					}
				}
				if (m_dataByFuelType.Count > 0)
				{
					TurnOffThrusterFlame(m_dataByFuelType);
				}
			}
			m_dampenersEnabledLastFrame = DampenersEnabled;
			m_thrustsChanged = false;
		}

		private void TurnOffThrusterFlame(List<FuelTypeData> dataByFuelType)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			foreach (FuelTypeData item in dataByFuelType)
			{
				foreach (KeyValuePair<Vector3I, HashSet<MyEntity>> item2 in item.ThrustsByDirection)
				{
					Enumerator<MyEntity> enumerator3 = item2.Value.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							MyThrust myThrust = enumerator3.get_Current() as MyThrust;
							if (myThrust != null && myThrust.ThrustOverride <= 0f)
							{
								myThrust.CurrentStrength = 0f;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator3).Dispose();
					}
				}
			}
		}

		private void RecomputeThrustParameters()
		{
			m_secondFrameUpdate = true;
			if (!m_thrustsChanged && m_secondFrameUpdate)
			{
				m_secondFrameUpdate = false;
			}
			Vector3 zero = Vector3.Zero;
			float num = 0f;
			Vector3 maxPositiveThrust = default(Vector3);
			Vector3 maxNegativeThrust = default(Vector3);
			Vector3 totalMaxNegativeThrust = default(Vector3);
			Vector3 totalMaxPositiveThrust = default(Vector3);
			MaxRequiredPowerInput = 0f;
			MinRequiredPowerInput = 0f;
			foreach (FuelTypeData item in m_dataByFuelType)
			{
				RecomputeTypeThrustParameters(item);
				MaxRequiredPowerInput += item.MaxRequiredPowerInput;
				MinRequiredPowerInput += item.MinRequiredPowerInput;
				maxPositiveThrust += item.MaxPositiveThrust;
				maxNegativeThrust += item.MaxNegativeThrust;
				zero += item.ThrustOverride;
				num += item.ThrustOverridePower;
			}
			totalMaxNegativeThrust += m_maxNegativeThrust;
			totalMaxPositiveThrust += m_maxPositiveThrust;
			foreach (MyConveyorConnectedGroup connectedGroup in m_connectedGroups)
			{
				connectedGroup.MaxPositiveThrust = default(Vector3);
				connectedGroup.MaxNegativeThrust = default(Vector3);
				connectedGroup.ThrustOverride = default(Vector3);
				connectedGroup.ThrustOverridePower = 0f;
				foreach (FuelTypeData item2 in connectedGroup.DataByFuelType)
				{
					RecomputeTypeThrustParameters(item2);
					MaxRequiredPowerInput += item2.MaxRequiredPowerInput;
					MinRequiredPowerInput += item2.MinRequiredPowerInput;
					connectedGroup.MaxPositiveThrust += item2.MaxPositiveThrust;
					connectedGroup.MaxNegativeThrust += item2.MaxNegativeThrust;
					connectedGroup.ThrustOverride += item2.ThrustOverride;
					connectedGroup.ThrustOverridePower += item2.ThrustOverridePower;
				}
				totalMaxNegativeThrust += connectedGroup.MaxNegativeThrust;
				totalMaxPositiveThrust += connectedGroup.MaxPositiveThrust;
			}
			m_totalThrustOverride = zero;
			m_totalThrustOverridePower = num;
			m_maxPositiveThrust = maxPositiveThrust;
			m_maxNegativeThrust = maxNegativeThrust;
			m_totalMaxNegativeThrust = totalMaxNegativeThrust;
			m_totalMaxPositiveThrust = totalMaxPositiveThrust;
		}

		public float GetMaxThrustInDirection(Base6Directions.Direction direction)
		{
			return direction switch
			{
				Base6Directions.Direction.Up => m_maxPositiveThrust.Y, 
				Base6Directions.Direction.Right => m_maxPositiveThrust.X, 
				Base6Directions.Direction.Backward => m_maxNegativeThrust.Z, 
				Base6Directions.Direction.Left => m_maxNegativeThrust.X, 
				Base6Directions.Direction.Down => m_maxNegativeThrust.Y, 
				_ => m_maxPositiveThrust.Z, 
			};
		}

		private float GetMaxRequirementsByDirection(FuelTypeData fuelData, Vector3I direction)
		{
			if (fuelData.MaxRequirementsByDirection.TryGetValue(direction, out var value))
			{
				return value;
			}
			return 0f;
		}

		private void RecomputeTypeThrustParameters(FuelTypeData fuelData)
		{
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			fuelData.MaxRequiredPowerInput = 0f;
			fuelData.MinRequiredPowerInput = 0f;
			fuelData.MaxPositiveThrust = default(Vector3);
			fuelData.MaxNegativeThrust = default(Vector3);
			fuelData.MaxRequirementsByDirection.Clear();
			fuelData.ThrustOverride = default(Vector3);
			fuelData.ThrustOverridePower = 0f;
			fuelData.CurrentRequiredFuelInput = 0f;
			foreach (KeyValuePair<Vector3I, HashSet<MyEntity>> item in fuelData.ThrustsByDirection)
			{
				if (!fuelData.MaxRequirementsByDirection.ContainsKey(item.Key))
				{
					fuelData.MaxRequirementsByDirection[item.Key] = 0f;
				}
				float num = 0f;
				Enumerator<MyEntity> enumerator2 = item.Value.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MyEntity current2 = enumerator2.get_Current();
						if (!RecomputeOverriddenParameters(current2, fuelData) && IsUsed(current2))
						{
							float num2 = ForceMagnitude(current2, m_lastPlanetaryInfluence, m_lastPlanetaryInfluenceHasAtmosphere);
							float num3 = CalculateForceMultiplier(current2, m_lastPlanetaryInfluence, m_lastPlanetaryInfluenceHasAtmosphere);
							float num4 = CalculateConsumptionMultiplier(current2, m_lastPlanetaryGravityMagnitude);
							if (current2 is MyThrust && !(current2 as MyThrust).IsPowered)
							{
								fuelData.MaxPositiveThrust += 0f;
								fuelData.MaxNegativeThrust += 0f;
							}
							else
							{
								fuelData.MaxPositiveThrust += Vector3.Clamp(-item.Key * num2, Vector3.Zero, Vector3.PositiveInfinity);
								fuelData.MaxNegativeThrust += -Vector3.Clamp(-item.Key * num2, Vector3.NegativeInfinity, Vector3.Zero);
							}
							num += MaxPowerConsumption(current2) * num3 * num4;
							fuelData.MinRequiredPowerInput += MinPowerConsumption(current2) * num4;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
				fuelData.MaxRequirementsByDirection[item.Key] += num;
			}
			fuelData.MaxRequiredPowerInput += Math.Max(GetMaxRequirementsByDirection(fuelData, Vector3I.Forward), GetMaxRequirementsByDirection(fuelData, Vector3I.Backward));
			fuelData.MaxRequiredPowerInput += Math.Max(GetMaxRequirementsByDirection(fuelData, Vector3I.Left), GetMaxRequirementsByDirection(fuelData, Vector3I.Right));
			fuelData.MaxRequiredPowerInput += Math.Max(GetMaxRequirementsByDirection(fuelData, Vector3I.Up), GetMaxRequirementsByDirection(fuelData, Vector3I.Down));
		}

		protected virtual void UpdateThrusts(bool applyDampeners, Vector3 dampeningVelocity)
		{
			for (int i = 0; i < m_dataByFuelType.Count; i++)
			{
				FuelTypeData fuelData = m_dataByFuelType[i];
				if (AutopilotEnabled)
				{
					ComputeAiThrust(AutoPilotControlThrust, fuelData);
				}
				else
				{
					ComputeBaseThrust(ref m_controlThrust, fuelData, applyDampeners, dampeningVelocity);
				}
			}
			for (int j = 0; j < m_connectedGroups.Count; j++)
			{
				MyConveyorConnectedGroup myConveyorConnectedGroup = m_connectedGroups[j];
				for (int k = 0; k < myConveyorConnectedGroup.DataByFuelType.Count; k++)
				{
					FuelTypeData fuelData2 = myConveyorConnectedGroup.DataByFuelType[k];
					if (AutopilotEnabled)
					{
						ComputeAiThrust(AutoPilotControlThrust, fuelData2);
					}
					else
					{
						ComputeBaseThrust(ref m_controlThrust, fuelData2, applyDampeners, dampeningVelocity);
					}
				}
			}
			FinalThrust = default(Vector3);
			bool flag = !MySession.Static.SimplifiedSimulation;
			Vector3 thrust = default(Vector3);
			for (int l = 0; l < m_dataByFuelType.Count; l++)
			{
				MyDefinitionId fuelType = m_fuelTypes[l];
				FuelTypeData fuelTypeData = m_dataByFuelType[l];
				if (flag)
				{
					UpdatePowerAndThrustStrength(fuelTypeData.CurrentThrust, fuelType, null, updateThrust: true);
				}
				Vector3 vector = m_maxPositiveThrust + m_maxNegativeThrust;
				thrust.X = ((vector.X != 0f) ? (fuelTypeData.CurrentThrust.X * (fuelTypeData.MaxPositiveThrust.X + fuelTypeData.MaxNegativeThrust.X) / vector.X) : 0f);
				thrust.Y = ((vector.Y != 0f) ? (fuelTypeData.CurrentThrust.Y * (fuelTypeData.MaxPositiveThrust.Y + fuelTypeData.MaxNegativeThrust.Y) / vector.Y) : 0f);
				thrust.Z = ((vector.Z != 0f) ? (fuelTypeData.CurrentThrust.Z * (fuelTypeData.MaxPositiveThrust.Z + fuelTypeData.MaxNegativeThrust.Z) / vector.Z) : 0f);
				Vector3 vector2 = ApplyThrustModifiers(ref fuelType, ref thrust, ref m_totalThrustOverride, m_resourceSink);
				FinalThrust += vector2;
			}
			Vector3 thrust2 = default(Vector3);
			for (int m = 0; m < m_connectedGroups.Count; m++)
			{
				MyConveyorConnectedGroup myConveyorConnectedGroup2 = m_connectedGroups[m];
				for (int n = 0; n < myConveyorConnectedGroup2.DataByFuelType.Count; n++)
				{
					MyDefinitionId fuelType2 = myConveyorConnectedGroup2.FuelTypes[n];
					FuelTypeData fuelTypeData2 = myConveyorConnectedGroup2.DataByFuelType[n];
					if (flag && ((Entity.Physics.RigidBody?.IsActive ?? true) || m_thrustsChanged || m_lastControlThrustChanged))
					{
						UpdatePowerAndThrustStrength(fuelTypeData2.CurrentThrust, fuelType2, myConveyorConnectedGroup2, updateThrust: true);
					}
					Vector3 vector3 = myConveyorConnectedGroup2.MaxPositiveThrust + myConveyorConnectedGroup2.MaxNegativeThrust;
					thrust2.X = ((vector3.X != 0f) ? (fuelTypeData2.CurrentThrust.X * (fuelTypeData2.MaxPositiveThrust.X + fuelTypeData2.MaxNegativeThrust.X) / vector3.X) : 0f);
					thrust2.Y = ((vector3.Y != 0f) ? (fuelTypeData2.CurrentThrust.Y * (fuelTypeData2.MaxPositiveThrust.Y + fuelTypeData2.MaxNegativeThrust.Y) / vector3.Y) : 0f);
					thrust2.Z = ((vector3.Z != 0f) ? (fuelTypeData2.CurrentThrust.Z * (fuelTypeData2.MaxPositiveThrust.Z + fuelTypeData2.MaxNegativeThrust.Z) / vector3.Z) : 0f);
					Vector3 vector4 = ApplyThrustModifiers(ref fuelType2, ref thrust2, ref myConveyorConnectedGroup2.ThrustOverride, myConveyorConnectedGroup2.ResourceSink);
					FinalThrust += vector4;
				}
			}
			m_lastControlThrustChanged = m_controlThrustChanged;
			m_controlThrustChanged = false;
		}

		public Vector3 GetAutoPilotThrustForDirection(Vector3 direction)
		{
			foreach (FuelTypeData item in m_dataByFuelType)
			{
				ComputeAiThrust(AutoPilotControlThrust, item);
			}
			foreach (MyConveyorConnectedGroup connectedGroup in m_connectedGroups)
			{
				foreach (FuelTypeData item2 in connectedGroup.DataByFuelType)
				{
					ComputeAiThrust(AutoPilotControlThrust, item2);
				}
			}
			Vector3 result = default(Vector3);
			Vector3 thrust = default(Vector3);
			for (int i = 0; i < m_dataByFuelType.Count; i++)
			{
				MyDefinitionId fuelType = m_fuelTypes[i];
				FuelTypeData fuelTypeData = m_dataByFuelType[i];
				UpdatePowerAndThrustStrength(fuelTypeData.CurrentThrust, fuelType, null, updateThrust: false);
				Vector3 vector = m_maxPositiveThrust + m_maxNegativeThrust;
				thrust.X = ((vector.X != 0f) ? (fuelTypeData.CurrentThrust.X * (fuelTypeData.MaxPositiveThrust.X + fuelTypeData.MaxNegativeThrust.X) / vector.X) : 0f);
				thrust.Y = ((vector.Y != 0f) ? (fuelTypeData.CurrentThrust.Y * (fuelTypeData.MaxPositiveThrust.Y + fuelTypeData.MaxNegativeThrust.Y) / vector.Y) : 0f);
				thrust.Z = ((vector.Z != 0f) ? (fuelTypeData.CurrentThrust.Z * (fuelTypeData.MaxPositiveThrust.Z + fuelTypeData.MaxNegativeThrust.Z) / vector.Z) : 0f);
				result += ApplyThrustModifiers(ref fuelType, ref thrust, ref m_totalThrustOverride, m_resourceSink);
			}
			Vector3 thrust2 = default(Vector3);
			foreach (MyConveyorConnectedGroup connectedGroup2 in m_connectedGroups)
			{
				for (int j = 0; j < connectedGroup2.DataByFuelType.Count; j++)
				{
					MyDefinitionId fuelType2 = connectedGroup2.FuelTypes[j];
					FuelTypeData fuelTypeData2 = connectedGroup2.DataByFuelType[j];
					UpdatePowerAndThrustStrength(fuelTypeData2.CurrentThrust, fuelType2, connectedGroup2, updateThrust: false);
					Vector3 vector2 = connectedGroup2.MaxPositiveThrust + connectedGroup2.MaxNegativeThrust;
					thrust2.X = ((vector2.X != 0f) ? (fuelTypeData2.CurrentThrust.X * (fuelTypeData2.MaxPositiveThrust.X + fuelTypeData2.MaxNegativeThrust.X) / vector2.X) : 0f);
					thrust2.Y = ((vector2.Y != 0f) ? (fuelTypeData2.CurrentThrust.Y * (fuelTypeData2.MaxPositiveThrust.Y + fuelTypeData2.MaxNegativeThrust.Y) / vector2.Y) : 0f);
					thrust2.Z = ((vector2.Z != 0f) ? (fuelTypeData2.CurrentThrust.Z * (fuelTypeData2.MaxPositiveThrust.Z + fuelTypeData2.MaxNegativeThrust.Z) / vector2.Z) : 0f);
					result += ApplyThrustModifiers(ref fuelType2, ref thrust2, ref connectedGroup2.ThrustOverride, connectedGroup2.ResourceSink);
				}
			}
			m_lastControlThrustChanged = m_controlThrustChanged;
			m_controlThrustChanged = false;
			return result;
		}

		private void ComputeBaseThrust(ref Vector3 controlThrust, FuelTypeData fuelData, bool applyDampeners, Vector3 dampeningVelocity)
		{
			MyPhysicsComponentBase physics = Entity.Physics;
			if (physics == null)
			{
				fuelData.CurrentThrust = Vector3.Zero;
				return;
			}
			Matrix matrix = Entity.PositionComp.WorldMatrixNormalizedInv;
			Vector3 vector = physics.Gravity * 0.5f;
			Vector3 vector2 = Vector3.TransformNormal((applyDampeners ? physics.LinearVelocity : Vector3.Zero) + vector, matrix);
			if (vector2.LengthSquared() < 1.00000011E-06f)
			{
				vector2 = Vector3.Zero;
			}
			Vector3 vector3 = dampeningVelocity;
			_ = Vector3.Zero;
			Vector3 vector4 = Vector3.Zero;
			Vector3 vector5 = vector3 - vector2;
			if (!Vector3.IsZero(vector5) || !Vector3.IsZero(controlThrust) || !Vector3.IsZero(fuelData.ThrustOverride))
			{
				Vector3 zero = Vector3.Zero;
				if (DampenersEnabled)
				{
					Vector3 value = Vector3.Clamp(controlThrust, -Vector3.One, Vector3.One);
					value = Vector3.Abs(value);
					zero = (Vector3.One - value) * Vector3.IsZeroVector(fuelData.ThrustOverride);
					Vector3 zero2 = Vector3.Zero;
					if (vector2.X > vector3.X)
					{
						zero2.X = m_totalMaxNegativeThrust.X;
					}
					else if (vector2.X < vector3.X)
					{
						zero2.X = m_totalMaxPositiveThrust.X;
					}
					if (vector2.Y > vector3.Y)
					{
						zero2.Y = m_totalMaxNegativeThrust.Y;
					}
					else if (vector2.Y < vector3.Y)
					{
						zero2.Y = m_totalMaxPositiveThrust.Y;
					}
					if (vector2.Z > vector3.Z)
					{
						zero2.Z = m_totalMaxNegativeThrust.Z;
					}
					else if (vector2.Z < vector3.Z)
					{
						zero2.Z = m_totalMaxPositiveThrust.Z;
					}
					Vector3 zero3 = Vector3.Zero;
					if (vector2.X > vector3.X)
					{
						zero3.X = fuelData.MaxNegativeThrust.X;
					}
					else if (vector2.X < vector3.X)
					{
						zero3.X = fuelData.MaxPositiveThrust.X;
					}
					if (vector2.Y > vector3.Y)
					{
						zero3.Y = fuelData.MaxNegativeThrust.Y;
					}
					else if (vector2.Y < vector3.Y)
					{
						zero3.Y = fuelData.MaxPositiveThrust.Y;
					}
					if (vector2.Z > vector3.Z)
					{
						zero3.Z = fuelData.MaxNegativeThrust.Z;
					}
					else if (vector2.Z < vector3.Z)
					{
						zero3.Z = fuelData.MaxPositiveThrust.Z;
					}
					Vector3 vector6 = zero3 / zero2;
					if (!vector6.X.IsValid())
					{
						vector6.X = 1f;
					}
					if (!vector6.Y.IsValid())
					{
						vector6.Y = 1f;
					}
					if (!vector6.Z.IsValid())
					{
						vector6.Z = 1f;
					}
					zero *= vector6;
					vector4 = vector5 / 0.5f * CalculateMass() * zero;
				}
			}
			Vector3 vector7 = Vector3.Zero;
			if (!Vector3.IsZero(fuelData.MaxPositiveThrust) || !Vector3.IsZero(fuelData.MaxNegativeThrust))
			{
				Vector3 vector8 = Vector3.Clamp(controlThrust, Vector3.Zero, Vector3.One);
				vector7 = Vector3.Clamp(controlThrust, -Vector3.One, Vector3.Zero) * fuelData.MaxNegativeThrust + vector8 * fuelData.MaxPositiveThrust;
				vector7 = Vector3.Clamp(vector7, -fuelData.MaxNegativeThrust, fuelData.MaxPositiveThrust);
			}
			vector7 = Vector3.Clamp(vector7 + vector4, -fuelData.MaxNegativeThrust * SlowdownFactor, fuelData.MaxPositiveThrust * SlowdownFactor);
			if (!Vector3.IsZero(vector7))
			{
				m_controlThrustChanged = true;
				m_lastControlThrustChanged = m_controlThrustChanged;
			}
			fuelData.CurrentThrust = vector7;
		}

		private void ComputeAiThrust(Vector3 direction, FuelTypeData fuelData)
		{
			MatrixD m = Entity.PositionComp.WorldMatrixNormalizedInv.GetOrientation();
			Matrix matrix = m;
			Vector3 vector = Vector3.Clamp(direction, Vector3.Zero, Vector3.One);
			Vector3 vector2 = Vector3.Clamp(direction, -Vector3.One, Vector3.Zero);
			Vector3 vector3 = Vector3.Clamp(-Vector3.Transform(Entity.Physics.Gravity, ref matrix) * Entity.Physics.Mass, Vector3.Zero, Vector3.PositiveInfinity);
			Vector3 vector4 = Vector3.Clamp(-Vector3.Transform(Entity.Physics.Gravity, ref matrix) * Entity.Physics.Mass, Vector3.NegativeInfinity, Vector3.Zero);
			Vector3 vector5 = (MaxThrustOverride.HasValue ? (MaxThrustOverride.Value * Vector3I.Sign(fuelData.MaxPositiveThrust)) : fuelData.MaxPositiveThrust);
			Vector3 vector6 = (MaxThrustOverride.HasValue ? (MaxThrustOverride.Value * Vector3I.Sign(fuelData.MaxNegativeThrust)) : fuelData.MaxNegativeThrust);
			Vector3 vector7 = Vector3.Clamp(vector5 - vector3, Vector3.Zero, Vector3.PositiveInfinity);
			Vector3 vector8 = Vector3.Clamp(vector6 + vector4, Vector3.Zero, Vector3.PositiveInfinity);
			Vector3 vector9 = vector7 * vector;
			Vector3 vector10 = vector8 * -vector2;
			float num = Math.Max(vector9.Max(), vector10.Max());
			Vector3 vector11 = Vector3.Zero;
			if (num > 0.001f)
			{
				Vector3 vector12 = vector * vector9;
				Vector3 vector13 = -vector2 * vector10;
				Vector3 vector14 = vector7 / vector12;
				Vector3 vector15 = vector8 / vector13;
				if (!vector14.X.IsValid())
				{
					vector14.X = 1f;
				}
				if (!vector14.Y.IsValid())
				{
					vector14.Y = 1f;
				}
				if (!vector14.Z.IsValid())
				{
					vector14.Z = 1f;
				}
				if (!vector15.X.IsValid())
				{
					vector15.X = 1f;
				}
				if (!vector15.Y.IsValid())
				{
					vector15.Y = 1f;
				}
				if (!vector15.Z.IsValid())
				{
					vector15.Z = 1f;
				}
				vector11 = -vector13 * vector15 + vector12 * vector14;
				vector11 += vector3 + vector4;
				vector11 = Vector3.Clamp(vector11, -vector6, vector5);
			}
			float num2 = (MyFakes.ENABLE_VR_REMOTE_CONTROL_WAYPOINTS_FAST_MOVEMENT ? 0.25f : 0.5f);
			Vector3 vector16 = Vector3.Transform(Entity.Physics.LinearVelocity + Entity.Physics.Gravity / 2f, ref matrix);
<<<<<<< HEAD
			Vector3D vector3D;
			if (!Vector3.IsZero(direction))
			{
				Vector3 direction2 = Vector3.Normalize(direction);
				vector3D = Vector3.Reject(vector16, direction2);
			}
			else
			{
				vector3D = vector16;
			}
			Vector3D vector3D2 = -vector3D / num2 * Entity.Physics.Mass;
			vector11 = Vector3.Clamp(vector11 + vector3D2, -vector6 * SlowdownFactor, vector5 * SlowdownFactor);
=======
			Vector3D vector3D2;
			if (!Vector3.IsZero(direction))
			{
				Vector3D vector3D = Vector3.Normalize(direction);
				vector3D2 = Vector3.Reject(vector16, vector3D);
			}
			else
			{
				vector3D2 = vector16;
			}
			Vector3D vector3D3 = -vector3D2 / num2 * Entity.Physics.Mass;
			vector11 = Vector3.Clamp(vector11 + vector3D3, -vector6 * SlowdownFactor, vector5 * SlowdownFactor);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!Vector3.IsZero(vector11))
			{
				m_controlThrustChanged = true;
				m_lastControlThrustChanged = m_controlThrustChanged;
			}
			fuelData.CurrentThrust = vector11;
		}

		private void FlipNegativeInfinity(ref Vector3 v)
		{
			if (float.IsNegativeInfinity(v.X))
			{
				v.X = float.PositiveInfinity;
			}
			if (float.IsNegativeInfinity(v.Y))
			{
				v.Y = float.PositiveInfinity;
			}
			if (float.IsNegativeInfinity(v.Z))
			{
				v.Z = float.PositiveInfinity;
			}
		}

		protected virtual Vector3 ApplyThrustModifiers(ref MyDefinitionId fuelType, ref Vector3 thrust, ref Vector3 thrustOverride, MyResourceSinkComponentBase resourceSink)
		{
			thrust += thrustOverride;
			thrust *= resourceSink.SuppliedRatioByType(fuelType);
			thrust *= MyFakes.THRUST_FORCE_RATIO;
			return thrust;
		}

		private void UpdatePowerAndThrustStrength(Vector3 thrust, MyDefinitionId fuelType, MyConveyorConnectedGroup group, bool updateThrust)
		{
			if (!m_controlThrustChanged && !m_lastControlThrustChanged)
			{
				return;
			}
			MyResourceSinkComponent resourceSink;
			FuelTypeData fuelTypeData;
			float num;
			if (group == null)
			{
				int typeIndex = GetTypeIndex(ref fuelType);
				resourceSink = m_resourceSink;
				fuelTypeData = m_dataByFuelType[typeIndex];
				num = m_totalThrustOverridePower;
				m_lastPowerUpdate = MySession.Static.GameplayFrameCounter;
			}
			else
			{
				int typeIndex = group.GetTypeIndex(ref fuelType);
				resourceSink = group.ResourceSink;
				fuelTypeData = group.DataByFuelType[typeIndex];
				num = group.ThrustOverridePower;
				group.LastPowerUpdate = MySession.Static.GameplayFrameCounter;
			}
			Vector3 vector = Vector3.Zero;
			Vector3 vector2 = Vector3.Zero;
			if (!Vector3.IsZero(thrust) || num != 0f)
			{
				vector = thrust / (fuelTypeData.MaxPositiveThrust + 1E-07f);
				vector2 = -thrust / (fuelTypeData.MaxNegativeThrust + 1E-07f);
				vector = Vector3.Clamp(vector, Vector3.Zero, Vector3.One);
				vector2 = Vector3.Clamp(vector2, Vector3.Zero, Vector3.One);
				float num2 = 0f;
				if (Enabled)
				{
					num2 += ((vector.X > 0f) ? (vector.X * GetMaxPowerRequirement(fuelTypeData, ref Vector3I.Left)) : 0f);
					num2 += ((vector.Y > 0f) ? (vector.Y * GetMaxPowerRequirement(fuelTypeData, ref Vector3I.Down)) : 0f);
					num2 += ((vector.Z > 0f) ? (vector.Z * GetMaxPowerRequirement(fuelTypeData, ref Vector3I.Forward)) : 0f);
					num2 += ((vector2.X > 0f) ? (vector2.X * GetMaxPowerRequirement(fuelTypeData, ref Vector3I.Right)) : 0f);
					num2 += ((vector2.Y > 0f) ? (vector2.Y * GetMaxPowerRequirement(fuelTypeData, ref Vector3I.Up)) : 0f);
					num2 += ((vector2.Z > 0f) ? (vector2.Z * GetMaxPowerRequirement(fuelTypeData, ref Vector3I.Backward)) : 0f);
					num2 += num;
					num2 = Math.Max(num2, fuelTypeData.MinRequiredPowerInput);
				}
				SetRequiredFuelInput(ref fuelType, PowerAmountToFuel(ref fuelType, num2, group), group);
				if (num2 > 1E-05f)
				{
					resourceSink.Update();
				}
			}
			else
			{
				SetRequiredFuelInput(ref fuelType, PowerAmountToFuel(ref fuelType, fuelTypeData.MinRequiredPowerInput, group), group);
				resourceSink.Update();
			}
			if (updateThrust)
			{
				UpdateThrustStrength(fuelTypeData.ThrustsByDirection[Vector3I.Left], vector.X);
				UpdateThrustStrength(fuelTypeData.ThrustsByDirection[Vector3I.Down], vector.Y);
				UpdateThrustStrength(fuelTypeData.ThrustsByDirection[Vector3I.Forward], vector.Z);
				UpdateThrustStrength(fuelTypeData.ThrustsByDirection[Vector3I.Right], vector2.X);
				UpdateThrustStrength(fuelTypeData.ThrustsByDirection[Vector3I.Up], vector2.Y);
				UpdateThrustStrength(fuelTypeData.ThrustsByDirection[Vector3I.Backward], vector2.Z);
			}
		}

		private void RecalculatePlanetaryInfluence()
		{
			BoundingBoxD box = Entity.PositionComp.WorldAABB;
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(ref box);
			float num = 0f;
			if (closestPlanet != null)
			{
				num = closestPlanet.GetAirDensity(box.Center);
				m_lastPlanetaryInfluenceHasAtmosphere = closestPlanet.HasAtmosphere;
				m_lastPlanetaryGravityMagnitude = closestPlanet.Components.Get<MyGravityProviderComponent>().GetGravityMultiplier(Entity.PositionComp.WorldMatrixRef.Translation);
				m_nextPlanetaryInfluenceRecalculation = MySession.Static.GameplayFrameCounter + Math.Min(100, 10000);
			}
			else
			{
				m_lastPlanetaryInfluenceHasAtmosphere = false;
				m_lastPlanetaryGravityMagnitude = 0f;
				m_nextPlanetaryInfluenceRecalculation = MySession.Static.GameplayFrameCounter + Math.Min(1000, 10000);
			}
			if (m_lastPlanetaryInfluence != num)
			{
				MarkDirty();
				m_lastPlanetaryInfluence = num;
			}
		}

		private void UpdateConveyorSystemChanges()
		{
			while (m_thrustEntitiesPending.Count > 0)
			{
				MyTuple<MyEntity, Vector3I, Func<bool>> myTuple = m_thrustEntitiesPending.Dequeue();
				if (IsThrustEntityType(myTuple.Item1))
				{
					if (m_thrustEntitiesRemovedBeforeRegister.Contains(myTuple.Item1))
					{
						m_thrustEntitiesRemovedBeforeRegister.Remove(myTuple.Item1);
					}
					else
					{
						RegisterLazy(myTuple.Item1, myTuple.Item2, myTuple.Item3);
					}
				}
			}
			while (m_conveyorSegmentsPending.Count > 0)
			{
				FindConnectedGroups(m_conveyorSegmentsPending.Dequeue(), m_connectedGroups, m_tmpGroupIndices);
				if (m_tmpGroupIndices.Count > 1)
				{
					MergeGroups(m_connectedGroups, m_tmpGroupIndices);
				}
				m_tmpGroupIndices.Clear();
			}
			while (m_conveyorEndpointsPending.Count > 0)
			{
				FindConnectedGroups(m_conveyorEndpointsPending.Dequeue(), m_connectedGroups, m_tmpGroupIndices);
				if (m_tmpGroupIndices.Count > 1)
				{
					MergeGroups(m_connectedGroups, m_tmpGroupIndices);
				}
				m_tmpGroupIndices.Clear();
			}
			foreach (MyConveyorConnectedGroup item in m_groupsToTrySplit)
			{
				TrySplitGroup(null, item);
			}
			m_groupsToTrySplit.Clear();
			if (m_mergeAllGroupsDirty)
			{
				TryMergeAllGroups();
				m_mergeAllGroupsDirty = false;
			}
		}

		private void ConveyorSystem_OnPoweredChanged()
		{
			MergeAllGroupsDirty();
		}

		/// <summary>
		/// Finds the resource sink that should handle the power consumption of thrustEntity
		/// </summary>
		public MyResourceSinkComponent ResourceSink(MyEntity thrustEntity)
		{
			MyConveyorConnectedGroup myConveyorConnectedGroup = FindEntityGroup(thrustEntity);
			if (myConveyorConnectedGroup != null)
			{
				return myConveyorConnectedGroup.ResourceSink;
			}
			return m_resourceSink;
		}

		public void ResourceSinks(HashSet<MyResourceSinkComponent> outResourceSinks)
		{
			if (m_resourceSink != null)
			{
				outResourceSinks.Add(m_resourceSink);
			}
			foreach (MyConveyorConnectedGroup connectedGroup in m_connectedGroups)
			{
				if (connectedGroup.ResourceSink != null)
				{
					outResourceSinks.Add(connectedGroup.ResourceSink);
				}
			}
		}

		private MyConveyorConnectedGroup FindEntityGroup(MyEntity thrustEntity)
		{
			MyConveyorConnectedGroup myConveyorConnectedGroup = null;
			if (!IsThrustEntityType(thrustEntity))
			{
				IMyConveyorEndpoint myConveyorEndpoint = null;
				IMyConveyorEndpointBlock myConveyorEndpointBlock = thrustEntity as IMyConveyorEndpointBlock;
				IMyConveyorSegmentBlock myConveyorSegmentBlock = thrustEntity as IMyConveyorSegmentBlock;
				if (myConveyorEndpointBlock != null)
				{
					myConveyorEndpoint = myConveyorEndpointBlock.ConveyorEndpoint;
				}
				else if (myConveyorSegmentBlock != null && myConveyorSegmentBlock.ConveyorSegment.ConveyorLine != null)
				{
					myConveyorEndpoint = myConveyorSegmentBlock.ConveyorSegment.ConveyorLine.GetEndpoint(0) ?? myConveyorSegmentBlock.ConveyorSegment.ConveyorLine.GetEndpoint(1);
				}
				if (myConveyorEndpoint != null)
				{
					for (int i = 0; i < m_connectedGroups.Count; i++)
					{
						MyConveyorConnectedGroup myConveyorConnectedGroup2 = m_connectedGroups[i];
						if (MyGridConveyorSystem.Reachable(myConveyorConnectedGroup2.FirstEndpoint, myConveyorEndpoint))
						{
							myConveyorConnectedGroup = myConveyorConnectedGroup2;
							break;
						}
					}
				}
			}
			else
			{
				MyDefinitionId fuelId = FuelType(thrustEntity);
				if (MyResourceDistributorComponent.IsConveyorConnectionRequiredTotal(fuelId))
				{
					foreach (MyConveyorConnectedGroup connectedGroup in m_connectedGroups)
					{
						if (connectedGroup.TryGetTypeIndex(ref fuelId, out var typeIndex))
						{
							foreach (HashSet<MyEntity> value in connectedGroup.DataByFuelType[typeIndex].ThrustsByDirection.Values)
							{
								if (value.Contains(thrustEntity))
								{
									myConveyorConnectedGroup = connectedGroup;
									break;
								}
							}
							if (myConveyorConnectedGroup != null)
							{
								return myConveyorConnectedGroup;
							}
						}
					}
					return myConveyorConnectedGroup;
				}
			}
			return myConveyorConnectedGroup;
		}

		protected float GetMaxPowerRequirement(FuelTypeData typeData, ref Vector3I direction)
		{
			return typeData.MaxRequirementsByDirection[direction];
		}

		public virtual void MarkDirty(bool recomputePlanetaryInfluence = false)
		{
			m_thrustsChanged = true;
			m_controlThrustChanged = true;
			m_nextPlanetaryInfluenceRecalculation = 0;
		}

		private static float RequiredFuelInput(FuelTypeData typeData)
		{
			return typeData.CurrentRequiredFuelInput;
		}

		internal void SetRequiredFuelInput(ref MyDefinitionId fuelType, float newFuelInput, MyConveyorConnectedGroup group)
		{
			int typeIndex = 0;
			if ((group != null || TryGetTypeIndex(ref fuelType, out typeIndex)) && (group == null || group.TryGetTypeIndex(ref fuelType, out typeIndex)))
			{
				((group != null) ? group.DataByFuelType : m_dataByFuelType)[typeIndex].CurrentRequiredFuelInput = newFuelInput;
			}
		}

		protected float PowerAmountToFuel(ref MyDefinitionId fuelType, float powerAmount, MyConveyorConnectedGroup group)
		{
			int typeIndex = 0;
			if (group == null && !TryGetTypeIndex(ref fuelType, out typeIndex))
			{
				return 0f;
			}
			if (group != null && !group.TryGetTypeIndex(ref fuelType, out typeIndex))
			{
				return 0f;
			}
			List<FuelTypeData> list = ((group != null) ? group.DataByFuelType : m_dataByFuelType);
			return powerAmount / (list[typeIndex].Efficiency * list[typeIndex].EnergyDensity);
		}

		private bool TryGetTypeIndex(ref MyDefinitionId fuelId, out int typeIndex)
		{
			typeIndex = 0;
			if (m_fuelTypeToIndex.Count > 1 && !m_fuelTypeToIndex.TryGetValue(fuelId, out typeIndex))
			{
				return false;
			}
			return m_fuelTypeToIndex.Count > 0;
		}

		public bool IsThrustPoweredByType(MyEntity thrustEntity, ref MyDefinitionId fuelId)
		{
			return ResourceSink(thrustEntity).IsPoweredByType(fuelId);
		}

		protected int GetTypeIndex(ref MyDefinitionId fuelId)
		{
			int result = 0;
			if (m_fuelTypeToIndex.Count > 1 && m_fuelTypeToIndex.TryGetValue(fuelId, out var value))
			{
				result = value;
			}
			return result;
		}

		private void CubeGrid_OnBlockAdded(MySlimBlock addedBlock)
		{
			MyCubeBlock fatBlock = addedBlock.FatBlock;
			if (fatBlock != null)
			{
				IMyConveyorEndpointBlock myConveyorEndpointBlock = fatBlock as IMyConveyorEndpointBlock;
				IMyConveyorSegmentBlock myConveyorSegmentBlock = fatBlock as IMyConveyorSegmentBlock;
				if (myConveyorEndpointBlock != null && !IsThrustEntityType(myConveyorEndpointBlock as MyEntity))
				{
					m_conveyorEndpointsPending.Enqueue(myConveyorEndpointBlock);
				}
				else if (myConveyorSegmentBlock != null)
				{
					m_conveyorSegmentsPending.Enqueue(myConveyorSegmentBlock);
				}
			}
		}

		private void ConveyorSystem_OnBeforeRemoveSegmentBlock(IMyConveyorSegmentBlock conveyorSegmentBlock)
		{
			if (conveyorSegmentBlock != null)
			{
				MyConveyorConnectedGroup myConveyorConnectedGroup = FindEntityGroup(conveyorSegmentBlock as MyEntity);
				if (myConveyorConnectedGroup != null)
				{
					m_groupsToTrySplit.Add(myConveyorConnectedGroup);
				}
			}
		}

		private void ConveyorSystem_OnBeforeRemoveEndpointBlock(IMyConveyorEndpointBlock conveyorEndpointBlock)
		{
			if (conveyorEndpointBlock != null && IsThrustEntityType(conveyorEndpointBlock as MyEntity))
			{
				MyConveyorConnectedGroup myConveyorConnectedGroup = FindEntityGroup(conveyorEndpointBlock as MyEntity);
				if (myConveyorConnectedGroup != null)
				{
					m_groupsToTrySplit.Add(myConveyorConnectedGroup);
				}
			}
		}

		protected virtual void OnControlTrustChanged()
		{
		}

		protected abstract void UpdateThrustStrength(HashSet<MyEntity> entities, float thrustForce);

		protected abstract bool RecomputeOverriddenParameters(MyEntity thrustEntity, FuelTypeData fuelData);

		protected abstract bool IsUsed(MyEntity thrustEntity);

		protected abstract float ForceMagnitude(MyEntity thrustEntity, float planetaryInfluence, bool inAtmosphere);

		protected abstract float CalculateForceMultiplier(MyEntity thrustEntity, float planetaryInfluence, bool inAtmosphere);

		protected abstract float CalculateConsumptionMultiplier(MyEntity thrustEntity, float naturalGravityStrength);

		protected abstract float MaxPowerConsumption(MyEntity thrustEntity);

		protected abstract float MinPowerConsumption(MyEntity thrustEntity);

		protected abstract MyDefinitionId FuelType(MyEntity thrustEntity);

		protected abstract bool IsThrustEntityType(MyEntity thrustEntity);

		protected abstract void RemoveFromGroup(MyEntity thrustEntity, MyConveyorConnectedGroup group);

		protected abstract void AddToGroup(MyEntity thrustEntity, MyConveyorConnectedGroup group);

		public float GetLastThrustMultiplier(MyEntity thrustEntity)
		{
			return CalculateForceMultiplier(thrustEntity, m_lastPlanetaryInfluence, m_lastPlanetaryInfluenceHasAtmosphere);
		}

		protected virtual float CalculateMass()
		{
			return Entity.Physics.Mass;
		}

		public bool HasThrustersInAllDirections(MyDefinitionId fuelId)
		{
			if (m_fuelTypeToIndex.TryGetValue(fuelId, out var value))
			{
				FuelTypeData fuelTypeData = m_dataByFuelType[value];
<<<<<<< HEAD
				return true & (fuelTypeData.ThrustsByDirection[Vector3I.Backward].Count > 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Forward].Count > 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Up].Count > 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Down].Count > 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Left].Count > 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Right].Count > 0);
=======
				return true & (fuelTypeData.ThrustsByDirection[Vector3I.Backward].get_Count() > 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Forward].get_Count() > 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Up].get_Count() > 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Down].get_Count() > 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Left].get_Count() > 0) & (fuelTypeData.ThrustsByDirection[Vector3I.Right].get_Count() > 0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return false;
		}

		private void OnPlanetAddedOrRemoved(MyPlanet planet)
		{
			if (Entity != null)
			{
				BoundingBoxD boundingBox = Entity.PositionComp.WorldAABB;
				if (planet.IntersectsWithGravityFast(ref boundingBox))
				{
					MarkDirty(recomputePlanetaryInfluence: true);
				}
			}
		}

		private void OnTeleport(MyEntity entity)
		{
			MarkDirty(recomputePlanetaryInfluence: true);
		}

		public static void UpdateRelativeDampeningEntity(IMyControllableEntity controlledEntity, MyEntity dampeningEntity)
		{
			if (Sync.IsServer && dampeningEntity != null && dampeningEntity.PositionComp.WorldAABB.DistanceSquared(controlledEntity.Entity.PositionComp.GetPosition()) > (double)MAX_DISTANCE_RELATIVE_DAMPENING_SQ)
			{
				controlledEntity.RelativeDampeningEntity = null;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyPlayerCollection.ClearDampeningEntity, controlledEntity.Entity.EntityId);
			}
		}

		public virtual void SetRelativeDampeningEntity(MyEntity entity)
		{
		}
	}
}
