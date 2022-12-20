using System;
using System.Collections.Generic;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.EntityComponents
{
	public class MyResourceSinkComponent : MyResourceSinkComponentBase
	{
		private struct PerTypeData
		{
			public float CurrentInput;

			public float RequiredInput;

			/// Theoretical maximum of required input. This can be different from RequiredInput, but
			/// it has to be &gt;= RequiredInput. It is used to check whether current power supply can meet
			/// demand under stress.
			public float MaxRequiredInput;

			public float SuppliedRatio;

			public Func<float> RequiredInputFunc;

			public bool IsPowered;
		}

		private class Sandbox_Game_EntityComponents_MyResourceSinkComponent_003C_003EActor
		{
		}

		private MyEntity m_tmpConnectedEntity;

		public MyCubeBlock ParentBlock;

		private MyCubeGrid m_grid;

		private PerTypeData[] m_dataPerType;

		private readonly Dictionary<MyDefinitionId, int> m_resourceTypeToIndex = new Dictionary<MyDefinitionId, int>(1, MyDefinitionId.Comparer);

		private readonly List<MyDefinitionId> m_resourceIds = new List<MyDefinitionId>(1);

		[ThreadStatic]
		private static List<MyResourceSinkInfo> m_singleHelperList;

		/// <summary>
		/// Higher priority groups get more resources than lower priority ones.
		/// If there are not enough resources for everything, lower priority groups
		/// are turned off first.
		/// </summary>
		internal MyStringHash Group;

		public override IMyEntity TemporaryConnectedEntity
		{
			get
			{
				return m_tmpConnectedEntity;
			}
			set
			{
				m_tmpConnectedEntity = (MyEntity)value;
			}
		}

		public new MyEntity Entity => base.Entity as MyEntity;

		/// <summary>
		/// Grid of assigned entity. If no entity is assigned gets manualy set grid
		/// </summary>
		public MyCubeGrid Grid
		{
			get
			{
				if (m_grid != null)
				{
					return m_grid;
				}
				if (ParentBlock != null)
				{
					return ParentBlock.CubeGrid;
				}
				MyCubeGrid result;
				if (Entity?.Parent != null && (result = Entity?.Parent as MyCubeGrid) != null)
				{
					return result;
				}
				MyCubeGrid result2;
				if (Entity != null && (result2 = Entity as MyCubeGrid) != null)
				{
					return result2;
				}
				MyCubeBlock myCubeBlock;
				if (Entity != null && (myCubeBlock = Entity as MyCubeBlock) != null)
				{
					return myCubeBlock.CubeGrid;
				}
				MyCubeBlock myCubeBlock2;
				if (TemporaryConnectedEntity != null && (myCubeBlock2 = TemporaryConnectedEntity as MyCubeBlock) != null)
				{
					return myCubeBlock2.CubeGrid;
				}
				return m_grid;
			}
			set
			{
				m_grid = value;
			}
		}

		[Obsolete]
		public float MaxRequiredInput
		{
			get
			{
				return MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId);
			}
			set
			{
				SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, value);
			}
		}

		[Obsolete]
		public float RequiredInput => RequiredInputByType(MyResourceDistributorComponent.ElectricityId);

		[Obsolete]
		public float SuppliedRatio => SuppliedRatioByType(MyResourceDistributorComponent.ElectricityId);

		[Obsolete]
		public float CurrentInput => CurrentInputByType(MyResourceDistributorComponent.ElectricityId);

		[Obsolete]
		public bool IsPowered => IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

		public override ListReader<MyDefinitionId> AcceptedResources => new ListReader<MyDefinitionId>(m_resourceIds);

		public override string ComponentTypeDebugString => "Resource Sink";

		public event MyRequiredResourceChangeDelegate RequiredInputChanged;

		public event MyResourceAvailableDelegate ResourceAvailable;

		public event MyCurrentResourceInputChangedDelegate CurrentInputChanged;

		public event Action IsPoweredChanged;

		public event Action<MyResourceSinkComponent, MyDefinitionId> OnAddType;

		public event Action<MyResourceSinkComponent, MyDefinitionId> OnRemoveType;

		public MyResourceSinkComponent(int initialAllocationSize = 1)
		{
			AllocateData(initialAllocationSize);
		}

		public void Init(MyStringHash group, float maxRequiredInput, Func<float> requiredInputFunc, MyCubeBlock parent)
		{
			using (MyUtils.ReuseCollection(ref m_singleHelperList))
			{
				m_singleHelperList.Add(new MyResourceSinkInfo
				{
					ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
					MaxRequiredInput = maxRequiredInput,
					RequiredInputFunc = requiredInputFunc
				});
				Init(group, m_singleHelperList, parent);
			}
		}

		public void Init(MyStringHash group, MyResourceSinkInfo sinkData, MyCubeBlock parent)
		{
			using (MyUtils.ReuseCollection(ref m_singleHelperList))
			{
				m_singleHelperList.Add(sinkData);
				Init(group, m_singleHelperList, parent);
			}
		}

		public void Init(MyStringHash group, MyResourceSinkInfo sinkData)
		{
			Init(group, sinkData, null);
		}

		public void Init(MyStringHash group, List<MyResourceSinkInfo> sinkData, MyCubeBlock parent)
		{
			Group = group;
			ParentBlock = parent;
			if (sinkData != null && m_dataPerType.Length != sinkData.Count)
			{
				AllocateData(sinkData.Count);
			}
			m_resourceTypeToIndex.Clear();
			m_resourceIds.Clear();
			ClearAllCallbacks();
			if (sinkData != null)
			{
				int num = 0;
				for (int i = 0; i < sinkData.Count; i++)
				{
					m_resourceTypeToIndex.Add(sinkData[i].ResourceTypeId, num++);
					m_resourceIds.Add(sinkData[i].ResourceTypeId);
					m_dataPerType[num - 1].MaxRequiredInput = sinkData[i].MaxRequiredInput;
					m_dataPerType[num - 1].RequiredInputFunc = sinkData[i].RequiredInputFunc;
				}
			}
		}

		public void ClearAllData()
		{
			for (int i = 0; i < m_dataPerType.Length; i++)
			{
				m_dataPerType[i].IsPowered = false;
				m_dataPerType[i].SuppliedRatio = 0f;
			}
		}

		public void ClearAllData()
		{
			for (int i = 0; i < m_dataPerType.Length; i++)
			{
				m_dataPerType[i].IsPowered = false;
				m_dataPerType[i].SuppliedRatio = 0f;
			}
		}

		public void AddType(ref MyResourceSinkInfo sinkData)
		{
			if (!m_resourceIds.Contains(sinkData.ResourceTypeId) && !m_resourceTypeToIndex.ContainsKey(sinkData.ResourceTypeId))
			{
				PerTypeData[] array = new PerTypeData[m_resourceIds.Count + 1];
				for (int i = 0; i < m_dataPerType.Length; i++)
				{
					array[i] = m_dataPerType[i];
				}
				m_dataPerType = array;
				m_dataPerType[m_dataPerType.Length - 1] = new PerTypeData
				{
					MaxRequiredInput = sinkData.MaxRequiredInput,
					RequiredInputFunc = sinkData.RequiredInputFunc
				};
				m_resourceIds.Add(sinkData.ResourceTypeId);
				m_resourceTypeToIndex.Add(sinkData.ResourceTypeId, m_dataPerType.Length - 1);
				if (this.OnAddType != null)
				{
					this.OnAddType(this, sinkData.ResourceTypeId);
				}
			}
		}

		public void RemoveType(ref MyDefinitionId resourceType)
		{
			if (!m_resourceIds.Contains(resourceType))
			{
				return;
			}
			if (this.OnRemoveType != null)
			{
				this.OnRemoveType(this, resourceType);
			}
			PerTypeData[] array = new PerTypeData[m_resourceIds.Count - 1];
			int typeIndex = GetTypeIndex(resourceType);
			int num = 0;
			int num2 = 0;
			while (num2 < m_dataPerType.Length)
			{
				if (num2 != typeIndex)
				{
					array[num] = m_dataPerType[num2];
				}
				num2++;
				num++;
			}
			m_dataPerType = array;
			m_resourceIds.Remove(resourceType);
			m_resourceTypeToIndex.Remove(resourceType);
		}

		private void AllocateData(int allocationSize)
		{
			m_dataPerType = new PerTypeData[allocationSize];
		}

		/// <summary>
		/// This should be called only from MyResourceDistributor.
		/// </summary>
		public override void SetInputFromDistributor(MyDefinitionId resourceTypeId, float newResourceInput, bool isAdaptible, bool fireEvents = true)
		{
			int typeIndex = GetTypeIndex(resourceTypeId);
			float currentInput = m_dataPerType[typeIndex].CurrentInput;
			float requiredInput = m_dataPerType[typeIndex].RequiredInput;
			bool flag;
			float suppliedRatio;
			if (newResourceInput > 0f || requiredInput == 0f)
			{
				flag = isAdaptible || newResourceInput >= requiredInput;
				suppliedRatio = ((!(requiredInput > 0f)) ? 1f : (newResourceInput / requiredInput));
			}
			else
			{
				flag = false;
				suppliedRatio = 0f;
			}
			bool flag2 = !newResourceInput.IsEqual(m_dataPerType[typeIndex].CurrentInput, 1E-06f);
			bool flag3 = flag != m_dataPerType[typeIndex].IsPowered;
			m_dataPerType[typeIndex].IsPowered = flag;
			m_dataPerType[typeIndex].SuppliedRatio = suppliedRatio;
			m_dataPerType[typeIndex].CurrentInput = newResourceInput;
			if (!fireEvents)
			{
				return;
			}
			if (flag2 && this.CurrentInputChanged != null)
			{
				if (MyEntities.IsAsyncUpdateInProgress)
				{
					float oldInputCopy = currentInput;
					MyDefinitionId resourceTypeIdCopy = resourceTypeId;
					MyCurrentResourceInputChangedDelegate handler = this.CurrentInputChanged;
					MyEntities.InvokeLater(delegate
					{
						handler?.Invoke(resourceTypeIdCopy, oldInputCopy, this);
					});
				}
				else
				{
					this.CurrentInputChanged(resourceTypeId, currentInput, this);
				}
			}
			if (flag3 && this.IsPoweredChanged != null)
			{
				if (MyEntities.IsAsyncUpdateInProgress)
<<<<<<< HEAD
				{
					MyEntities.InvokeLater(this.IsPoweredChanged);
				}
				else
				{
=======
				{
					MyEntities.InvokeLater(this.IsPoweredChanged);
				}
				else
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					this.IsPoweredChanged.InvokeIfNotNull();
				}
			}
		}

		public float ResourceAvailableByType(MyDefinitionId resourceTypeId)
		{
			float num = CurrentInputByType(resourceTypeId);
			if (this.ResourceAvailable != null)
			{
				num += this.ResourceAvailable(resourceTypeId, this);
			}
			return num;
		}

		public override bool IsPowerAvailable(MyDefinitionId resourceTypeId, float power)
		{
			return ResourceAvailableByType(resourceTypeId) >= power;
		}

		public void Update()
		{
			foreach (MyDefinitionId key in m_resourceTypeToIndex.Keys)
			{
				SetRequiredInputByType(key, m_dataPerType[GetTypeIndex(key)].RequiredInputFunc());
			}
		}

		public override float MaxRequiredInputByType(MyDefinitionId resourceTypeId)
		{
			return m_dataPerType[GetTypeIndex(resourceTypeId)].MaxRequiredInput;
		}

		public override void SetMaxRequiredInputByType(MyDefinitionId resourceTypeId, float newMaxRequiredInput)
		{
			m_dataPerType[GetTypeIndex(resourceTypeId)].MaxRequiredInput = newMaxRequiredInput;
		}

		public override float CurrentInputByType(MyDefinitionId resourceTypeId)
		{
			return m_dataPerType[GetTypeIndex(resourceTypeId)].CurrentInput;
		}

		public override float RequiredInputByType(MyDefinitionId resourceTypeId)
		{
			return m_dataPerType[GetTypeIndex(resourceTypeId)].RequiredInput;
		}

		public override bool IsPoweredByType(MyDefinitionId resourceTypeId)
		{
			return m_dataPerType[GetTypeIndex(resourceTypeId)].IsPowered;
		}

		public override void SetRequiredInputByType(MyDefinitionId resourceTypeId, float newRequiredInput)
		{
			int typeIndex = GetTypeIndex(resourceTypeId);
			if (m_dataPerType[typeIndex].RequiredInput != newRequiredInput)
			{
				float requiredInput = m_dataPerType[typeIndex].RequiredInput;
				m_dataPerType[typeIndex].RequiredInput = newRequiredInput;
				if (this.RequiredInputChanged != null)
				{
					this.RequiredInputChanged(resourceTypeId, this, requiredInput, newRequiredInput);
				}
			}
		}

		/// <summary>
		/// Change the required input function (callback) for given type of resource. It does not call it immediatelly to update required input value.
		/// </summary>
		public override Func<float> SetRequiredInputFuncByType(MyDefinitionId resourceTypeId, Func<float> newRequiredInputFunc)
		{
			int typeIndex = GetTypeIndex(resourceTypeId);
			Func<float> requiredInputFunc = m_dataPerType[typeIndex].RequiredInputFunc;
			m_dataPerType[typeIndex].RequiredInputFunc = newRequiredInputFunc;
			return requiredInputFunc;
		}

		public override float SuppliedRatioByType(MyDefinitionId resourceTypeId)
		{
			return m_dataPerType[GetTypeIndex(resourceTypeId)].SuppliedRatio;
		}

		protected int GetTypeIndex(MyDefinitionId resourceTypeId)
		{
			int value = 0;
			m_resourceTypeToIndex.TryGetValue(resourceTypeId, out value);
			return value;
		}

		public void DebugDraw(Matrix worldMatrix)
		{
			if (!MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
			{
				return;
			}
			double num = 2.5 * 0.045;
			Vector3D vector3D = worldMatrix.Translation + worldMatrix.Up;
			Vector3D position = MySector.MainCamera.Position;
			Vector3D up = MySector.MainCamera.WorldMatrix.Up;
			Vector3D right = MySector.MainCamera.WorldMatrix.Right;
			double val = Vector3D.Distance(vector3D, position);
			double num2 = Math.Atan(2.5 / Math.Max(val, 0.001));
			if (num2 <= 0.27000001072883606)
			{
				return;
			}
			if (Entity != null)
			{
				MyRenderProxy.DebugDrawText3D(vector3D, Entity.ToString(), Color.Yellow, (float)num2, depthRead: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			}
			if (m_resourceIds == null || m_resourceIds.Count == 0)
			{
				return;
			}
			Vector3D vector3D2 = vector3D;
			int num3 = -1;
			foreach (MyDefinitionId resourceId in m_resourceIds)
			{
				vector3D2 = vector3D + num3 * up * num;
				DebugDrawResource(resourceId, vector3D2, right, (float)num2);
				num3--;
			}
		}

		private void DebugDrawResource(MyDefinitionId resourceId, Vector3D origin, Vector3D rightVector, float textSize)
		{
			Vector3D vector3D = 0.05000000074505806 * rightVector;
			Vector3D worldCoord = origin + vector3D + rightVector * 0.014999999664723873;
			int value = 0;
			string text = resourceId.SubtypeName;
			if (m_resourceTypeToIndex.TryGetValue(resourceId, out value))
			{
				PerTypeData perTypeData = m_dataPerType[value];
				text = $"{resourceId.SubtypeName} Required:{perTypeData.RequiredInput} Current:{perTypeData.CurrentInput} Ratio:{perTypeData.SuppliedRatio}";
			}
			MyRenderProxy.DebugDrawLine3D(origin, origin + vector3D, Color.White, Color.White, depthRead: false);
			MyRenderProxy.DebugDrawText3D(worldCoord, text, Color.White, textSize, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
		}

		private void ClearAllCallbacks()
		{
			this.RequiredInputChanged = null;
			this.ResourceAvailable = null;
			this.CurrentInputChanged = null;
			this.IsPoweredChanged = null;
			this.OnAddType = null;
			this.OnRemoveType = null;
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			ClearAllCallbacks();
		}
	}
}
