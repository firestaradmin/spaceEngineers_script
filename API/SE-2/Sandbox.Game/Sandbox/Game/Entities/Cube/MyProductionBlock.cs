using System;
using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	/// <summary>
	/// Common base for Assembler and Refinery blocks
	/// </summary>
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyProductionBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyProductionBlock)
	})]
	public abstract class MyProductionBlock : MyFunctionalBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyProductionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProductionBlock, IMyInventoryOwner
	{
		public struct QueueItem
		{
			public MyFixedPoint Amount;

			public MyBlueprintDefinitionBase Blueprint;

			public uint ItemId;
		}

		protected sealed class OnAddQueueItemRequest_003C_003ESystem_Int32_0023VRage_ObjectBuilders_SerializableDefinitionId_0023VRage_MyFixedPoint : ICallSite<MyProductionBlock, int, SerializableDefinitionId, MyFixedPoint, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProductionBlock @this, in int idx, in SerializableDefinitionId defId, in MyFixedPoint ammount, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnAddQueueItemRequest(idx, defId, ammount);
			}
		}

		protected sealed class OnAddQueueItemSuccess_003C_003ESystem_Int32_0023VRage_ObjectBuilders_SerializableDefinitionId_0023VRage_MyFixedPoint : ICallSite<MyProductionBlock, int, SerializableDefinitionId, MyFixedPoint, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProductionBlock @this, in int idx, in SerializableDefinitionId defId, in MyFixedPoint ammount, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnAddQueueItemSuccess(idx, defId, ammount);
			}
		}

		protected sealed class OnMoveQueueItemCallback_003C_003ESystem_UInt32_0023System_Int32 : ICallSite<MyProductionBlock, uint, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProductionBlock @this, in uint srcItemId, in int dstIdx, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnMoveQueueItemCallback(srcItemId, dstIdx);
			}
		}

		protected sealed class ClearQueueRequest_003C_003E : ICallSite<MyProductionBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProductionBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ClearQueueRequest();
			}
		}

		protected sealed class OnRemoveQueueItemRequest_003C_003ESystem_Int32_0023VRage_MyFixedPoint_0023System_Single : ICallSite<MyProductionBlock, int, MyFixedPoint, float, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProductionBlock @this, in int idx, in MyFixedPoint amount, in float progress, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveQueueItemRequest(idx, amount, progress);
			}
		}

		protected sealed class OnRemoveQueueItem_003C_003ESystem_Int32_0023VRage_MyFixedPoint_0023System_Single : ICallSite<MyProductionBlock, int, MyFixedPoint, float, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyProductionBlock @this, in int idx, in MyFixedPoint amount, in float progress, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveQueueItem(idx, amount, progress);
			}
		}

		protected class m_isProducing_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isProducing;
				ISyncType result = (isProducing = new Sync<bool, SyncDirection.FromServer>(P_1, P_2));
				((MyProductionBlock)P_0).m_isProducing = (Sync<bool, SyncDirection.FromServer>)isProducing;
				return result;
			}
		}

		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = (useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyProductionBlock)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		private readonly uint TIMER_NORMAL_IN_FRAMES = 60u;

		private readonly uint TIMER_TIER1_IN_FRAMES = 120u;

		private readonly uint TIMER_TIER2_IN_FRAMES = 240u;

		protected static Dictionary<MyDefinitionId, MyFixedPoint> m_tmpInventoryCounts = new Dictionary<MyDefinitionId, MyFixedPoint>();

		private MyInventoryAggregate m_inventoryAggregate;

		protected uint? m_realProductionStart;

		private MyInventory m_inputInventory;

		private MyInventory m_outputInventory;

<<<<<<< HEAD
		private IMyConveyorEndpoint m_multilineConveyorEndpoint;

		/// <summary>
		/// Use NextItemID
		/// </summary>
		private uint m_nextItemId;

		protected readonly Sync<bool, SyncDirection.FromServer> m_isProducing;

		protected readonly Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		protected QueueItem? m_currentQueueItem;
=======
		private string m_string;

		private IMyConveyorEndpoint m_multilineConveyorEndpoint;

		private uint m_nextItemId;

		protected readonly Sync<bool, SyncDirection.FromServer> m_isProducing;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		protected MySoundPair m_processSound = new MySoundPair();

<<<<<<< HEAD
		protected List<QueueItem> m_queue;

=======
		protected QueueItem? m_currentQueueItem;

		protected MySoundPair m_processSound = new MySoundPair();

		protected List<QueueItem> m_queue;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public Action OnQueueItemRemoved;

		protected MyProductionBlockDefinition ProductionBlockDefinition => (MyProductionBlockDefinition)base.BlockDefinition;

		public MyInventoryAggregate InventoryAggregate
		{
			get
			{
				return m_inventoryAggregate;
			}
			set
			{
				if (value != null)
				{
					base.Components.Remove<MyInventoryBase>();
					base.Components.Add((MyInventoryBase)value);
				}
			}
		}

		public MyInventory InputInventory
		{
			get
			{
				return m_inputInventory;
			}
			protected set
			{
				if (!InventoryAggregate.ChildList.Contains(value))
				{
					if (m_inputInventory != null)
					{
						InventoryAggregate.ChildList.RemoveComponent(m_inputInventory);
					}
					InventoryAggregate.AddComponent(value);
				}
			}
		}

		public MyInventory OutputInventory
		{
			get
			{
				return m_outputInventory;
			}
			protected set
			{
				if (!InventoryAggregate.ChildList.Contains(value))
				{
					if (m_outputInventory != null)
					{
						InventoryAggregate.ChildList.RemoveComponent(m_outputInventory);
					}
					InventoryAggregate.AddComponent(value);
				}
			}
		}

		public bool IsQueueEmpty => m_queue.Count == 0;

		/// <summary>
		/// Set by subclasses indicating whether they are working on something or not.
		/// Determines required power input.
		/// </summary>
		public bool IsProducing
		{
			get
			{
				return m_isProducing.Value;
			}
			protected set
			{
				if (value != m_isProducing.Value && Sync.IsServer)
				{
					m_isProducing.Value = value;
					IsProducing_ValueChanged(null);
				}
			}
		}

		/// <summary>
		/// Do not make changes to values in this enumerable. Consider it constant reference.
		/// </summary>
		public IEnumerable<QueueItem> Queue => m_queue;

		/// <summary>
		/// Gets next item id (auto increments)
		/// </summary>
		public uint NextItemId => m_nextItemId++;

		public bool UseConveyorSystem
		{
			get
			{
				return m_useConveyorSystem;
			}
			set
			{
				m_useConveyorSystem.Value = value;
			}
		}

		public override bool AllowTimerForceUpdate => false;

		int IMyInventoryOwner.InventoryCount => base.InventoryCount;

		long IMyInventoryOwner.EntityId => base.EntityId;

		bool IMyInventoryOwner.HasInventory => base.HasInventory;

		bool IMyInventoryOwner.UseConveyorSystem
		{
			get
			{
				return UseConveyorSystem;
			}
			set
			{
				UseConveyorSystem = value;
			}
		}

		public IMyConveyorEndpoint ConveyorEndpoint => m_multilineConveyorEndpoint;

		VRage.Game.ModAPI.IMyInventory Sandbox.ModAPI.IMyProductionBlock.InputInventory => InputInventory;

		VRage.Game.ModAPI.IMyInventory Sandbox.ModAPI.IMyProductionBlock.OutputInventory => OutputInventory;

		VRage.Game.ModAPI.Ingame.IMyInventory Sandbox.ModAPI.Ingame.IMyProductionBlock.InputInventory => InputInventory;

		VRage.Game.ModAPI.Ingame.IMyInventory Sandbox.ModAPI.Ingame.IMyProductionBlock.OutputInventory => OutputInventory;

		public event Action<MyProductionBlock> QueueChanged;

		public event Action StartedProducing;

		public event Action StoppedProducing;

		/// <summary>
		/// Production Block class constructor
		/// </summary>
		public MyProductionBlock()
		{
			CreateTerminalControls();
			m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			m_queue = new List<QueueItem>();
			m_isProducing.ValueChanged += IsProducing_ValueChanged;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
			IsProducing = false;
			base.Components.ComponentAdded += OnComponentAdded;
			base.Render = new MyRenderComponentScreenAreas(this);
		}

		private void IsProducing_ValueChanged(SyncBase obj)
		{
			if ((bool)m_isProducing)
			{
				OnStartProducing();
			}
			else
			{
				OnStopProducing();
			}
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.Closed)
				{
					UpdatePower();
				}
			}, "IsProducing");
		}

		private void IsProducing_ValueChanged(SyncBase obj)
		{
			if ((bool)m_isProducing)
			{
				OnStartProducing();
			}
			else
			{
				OnStopProducing();
			}
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.Closed)
				{
					UpdatePower();
				}
			}, "IsProducing");
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyProductionBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyProductionBlock> obj = new MyTerminalControlOnOffSwitch<MyProductionBlock>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = (MyProductionBlock x) => x.UseConveyorSystem,
					Setter = delegate(MyProductionBlock x, bool v)
					{
						x.UseConveyorSystem = v;
					}
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(ProductionBlockDefinition.ResourceSinkGroup, ProductionBlockDefinition.OperationalPowerConsumption, ComputeRequiredPower, this);
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_ProductionBlock myObjectBuilder_ProductionBlock = (MyObjectBuilder_ProductionBlock)objectBuilder;
			if (InventoryAggregate == null)
			{
				InventoryAggregate = new MyInventoryAggregate();
			}
			if (InputInventory == null)
			{
				InputInventory = new MyInventory(ProductionBlockDefinition.InventoryMaxVolume, ProductionBlockDefinition.InventorySize, MyInventoryFlags.CanReceive);
				if (myObjectBuilder_ProductionBlock.InputInventory != null)
				{
					InputInventory.Init(myObjectBuilder_ProductionBlock.InputInventory);
				}
			}
			if (OutputInventory == null)
			{
				OutputInventory = new MyInventory(ProductionBlockDefinition.InventoryMaxVolume, ProductionBlockDefinition.InventorySize, MyInventoryFlags.CanSend);
				if (myObjectBuilder_ProductionBlock.OutputInventory != null)
				{
					OutputInventory.Init(myObjectBuilder_ProductionBlock.OutputInventory);
				}
			}
			m_nextItemId = myObjectBuilder_ProductionBlock.NextItemId;
			_ = m_nextItemId;
			base.IsWorkingChanged += CubeBlock_IsWorkingChanged;
			base.ResourceSink.Update();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			if (myObjectBuilder_ProductionBlock.Queue != null)
			{
				m_queue.Clear();
				if (m_queue.Capacity < myObjectBuilder_ProductionBlock.Queue.Length)
				{
					m_queue.Capacity = myObjectBuilder_ProductionBlock.Queue.Length;
				}
				for (int i = 0; i < myObjectBuilder_ProductionBlock.Queue.Length; i++)
				{
					MyObjectBuilder_ProductionBlock.QueueItem itemOb = myObjectBuilder_ProductionBlock.Queue[i];
					QueueItem item = DeserializeQueueItem(itemOb);
					if (item.Blueprint != null)
					{
						m_queue.Add(item);
					}
					else
					{
						MySandboxGame.Log.WriteLine($"Could not add item into production block's queue: Blueprint {itemOb.Id} was not found.");
					}
				}
				UpdatePower();
			}
			m_useConveyorSystem.SetLocalValue(myObjectBuilder_ProductionBlock.UseConveyorSystem);
			CreateUpdateTimer(GetTimerTime(0), MyTimerTypes.Frame10);
		}

		protected virtual void UpdatePower()
		{
			if (base.ResourceSink != null)
			{
				base.ResourceSink.Update();
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_ProductionBlock myObjectBuilder_ProductionBlock = (MyObjectBuilder_ProductionBlock)base.GetObjectBuilderCubeBlock(copy);
			myObjectBuilder_ProductionBlock.UseConveyorSystem = m_useConveyorSystem;
			myObjectBuilder_ProductionBlock.NextItemId = m_nextItemId;
			if (m_queue.Count > 0)
			{
				myObjectBuilder_ProductionBlock.Queue = new MyObjectBuilder_ProductionBlock.QueueItem[m_queue.Count];
				for (int i = 0; i < m_queue.Count; i++)
				{
					myObjectBuilder_ProductionBlock.Queue[i].Id = m_queue[i].Blueprint.Id;
					myObjectBuilder_ProductionBlock.Queue[i].Amount = m_queue[i].Amount;
					myObjectBuilder_ProductionBlock.Queue[i].ItemId = m_queue[i].ItemId;
				}
			}
			else
			{
				myObjectBuilder_ProductionBlock.Queue = null;
			}
			return myObjectBuilder_ProductionBlock;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		public bool CanUseBlueprint(MyBlueprintDefinitionBase blueprint)
		{
			foreach (MyBlueprintClassDefinition blueprintClass in ProductionBlockDefinition.BlueprintClasses)
			{
				if (blueprintClass.ContainsBlueprint(blueprint))
				{
					return true;
				}
			}
			return false;
		}

		protected void InitializeInventoryCounts(bool inputInventory = true)
		{
			m_tmpInventoryCounts.Clear();
			foreach (MyPhysicalInventoryItem item in inputInventory ? InputInventory.GetItems() : OutputInventory.GetItems())
			{
				MyFixedPoint value = 0;
				MyDefinitionId key = new MyDefinitionId(item.Content.TypeId, item.Content.SubtypeId);
				m_tmpInventoryCounts.TryGetValue(key, out value);
				m_tmpInventoryCounts[key] = value + item.Amount;
			}
		}

		/// <summary>
		/// Sends request to server to add item to queue. (Can be also called on server. In that case it will be local)
		/// </summary>
		/// <param name="blueprint"></param>
		/// <param name="ammount"></param>
		/// <param name="idx">index to insert (-1 = last).</param>
		public virtual void AddQueueItemRequest(MyBlueprintDefinitionBase blueprint, MyFixedPoint ammount, int idx = -1)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && this is MyAssembler)
			{
				((MyAssembler)this).UpdateCurrentState(blueprint);
			}
			SerializableDefinitionId arg = blueprint.Id;
			MyMultiplayer.RaiseEvent(this, (MyProductionBlock x) => x.OnAddQueueItemRequest, idx, arg, ammount);
		}

<<<<<<< HEAD
		[Event(null, 430)]
=======
		[Event(null, 433)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnAddQueueItemRequest(int idx, SerializableDefinitionId defId, MyFixedPoint ammount)
		{
			MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(defId);
			if (blueprintDefinition != null)
			{
				InsertQueueItem(idx, blueprintDefinition, ammount);
				MyMultiplayer.RaiseEvent(this, (MyProductionBlock x) => x.OnAddQueueItemSuccess, idx, defId, ammount);
			}
		}

<<<<<<< HEAD
		[Event(null, 443)]
=======
		[Event(null, 446)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void OnAddQueueItemSuccess(int idx, SerializableDefinitionId defId, MyFixedPoint ammount)
		{
			InsertQueueItem(idx, MyDefinitionManager.Static.GetBlueprintDefinition(defId), ammount);
		}

		/// <summary>
		/// Sends request to server to move queue item. (Can be also called on server. In that case it will be local)
		/// </summary>
		/// <param name="srcItemId"></param>
		/// <param name="dstIdx"></param>
		public void MoveQueueItemRequest(uint srcItemId, int dstIdx)
		{
			MyMultiplayer.RaiseEvent(this, (MyProductionBlock x) => x.OnMoveQueueItemCallback, srcItemId, dstIdx);
		}

<<<<<<< HEAD
		[Event(null, 460)]
=======
		[Event(null, 463)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnMoveQueueItemCallback(uint srcItemId, int dstIdx)
		{
			MoveQueueItem(srcItemId, dstIdx);
		}

<<<<<<< HEAD
		[Event(null, 466)]
=======
		[Event(null, 469)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		protected void ClearQueueRequest()
		{
			for (int num = m_queue.Count - 1; num >= 0; num--)
			{
				if (RemoveQueueItemTests(num))
				{
					MyFixedPoint arg = -1;
					MyMultiplayer.RaiseEvent(this, (MyProductionBlock x) => x.OnRemoveQueueItem, num, arg, 0f);
				}
			}
		}

		/// <summary>
		/// Sends request to server to remove item from queue. (Can be also called on server. In that case it will be local)
		/// </summary>
		/// <param name="idx"></param>
		/// <param name="amount"></param>
		/// <param name="progress"></param>
		public void RemoveQueueItemRequest(int idx, MyFixedPoint amount, float progress = 0f)
		{
			MyMultiplayer.RaiseEvent(this, (MyProductionBlock x) => x.OnRemoveQueueItemRequest, idx, amount, progress);
		}

<<<<<<< HEAD
		[Event(null, 492)]
=======
		[Event(null, 495)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnRemoveQueueItemRequest(int idx, MyFixedPoint amount, float progress)
		{
			if (RemoveQueueItemTests(idx))
			{
				MyMultiplayer.RaiseEvent(this, (MyProductionBlock x) => x.OnRemoveQueueItem, idx, amount, progress);
			}
		}

		private bool RemoveQueueItemTests(int idx)
		{
			if (!m_queue.IsValidIndex(idx) && idx != -1)
			{
				MySandboxGame.Log.WriteLine("Invalid queue index in the remove item message!");
				return false;
			}
			return true;
		}

<<<<<<< HEAD
		[Event(null, 517)]
=======
		[Event(null, 520)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private void OnRemoveQueueItem(int idx, MyFixedPoint amount, float progress)
		{
			if (amount >= 0)
			{
				RemoveFirstQueueItem(idx, amount, progress);
				OnQueueItemRemoved.InvokeIfNotNull();
			}
			else
			{
				RemoveQueueItem(idx);
			}
		}

		protected virtual void OnQueueChanged()
		{
			if (this.QueueChanged != null)
			{
				this.QueueChanged(this);
			}
		}

		private QueueItem DeserializeQueueItem(MyObjectBuilder_ProductionBlock.QueueItem itemOb)
		{
			QueueItem result = default(QueueItem);
			result.Amount = itemOb.Amount;
			if (MyDefinitionManager.Static.HasBlueprint(itemOb.Id))
			{
				result.Blueprint = MyDefinitionManager.Static.GetBlueprintDefinition(itemOb.Id);
			}
			else
			{
				result.Blueprint = MyDefinitionManager.Static.TryGetBlueprintDefinitionByResultId(itemOb.Id);
			}
			result.ItemId = (itemOb.ItemId.HasValue ? itemOb.ItemId.Value : NextItemId);
			return result;
		}

		/// <summary>
		/// Removes specified amount from the first item in the queue. This is called by 
		/// subclasses (eg. assembler puts an item into its current production, refinery
		/// processes some ore, etc.) in their own specific situations.
		/// </summary>
		/// <param name="amount">Amount to be removed.</param>
		/// <param name="progress">Current progress of he processed item.</param>
		protected void RemoveFirstQueueItemAnnounce(MyFixedPoint amount, float progress = 0f)
		{
			RemoveQueueItemRequest(0, amount, progress);
		}

		protected virtual void RemoveFirstQueueItem(int index, MyFixedPoint amount, float progress = 0f)
		{
			if (!m_queue.IsValidIndex(index))
			{
				return;
			}
			QueueItem value = m_queue[index];
			amount = MathHelper.Clamp(amount, 0, value.Amount);
			value.Amount -= amount;
			m_queue[index] = value;
			if (value.Amount <= 0)
			{
				MyAssembler myAssembler = this as MyAssembler;
				if (myAssembler != null && Sync.IsServer)
				{
					myAssembler.CurrentProgress = 0f;
				}
				m_queue.RemoveAt(index);
			}
			UpdatePower();
			OnQueueChanged();
		}

		public void InsertQueueItemRequest(int idx, MyBlueprintDefinitionBase blueprint)
		{
			InsertQueueItemRequest(idx, blueprint, 1);
		}

		public void InsertQueueItemRequest(int idx, MyBlueprintDefinitionBase blueprint, MyFixedPoint amount)
		{
			AddQueueItemRequest(blueprint, amount, idx);
		}

		protected virtual void InsertQueueItem(int idx, MyBlueprintDefinitionBase blueprint, MyFixedPoint amount)
		{
			if (!CanUseBlueprint(blueprint))
			{
				return;
			}
			QueueItem queueItem = default(QueueItem);
			queueItem.Amount = amount;
			queueItem.Blueprint = blueprint;
			bool num = m_queue.Count == 0;
			if (m_queue.IsValidIndex(idx) && m_queue[idx].Blueprint == queueItem.Blueprint)
			{
				queueItem.Amount += m_queue[idx].Amount;
				queueItem.ItemId = m_queue[idx].ItemId;
				if (m_currentQueueItem.HasValue && m_queue[idx].ItemId == m_currentQueueItem.Value.ItemId)
				{
					m_currentQueueItem = queueItem;
				}
				m_queue[idx] = queueItem;
			}
			else if (m_queue.Count > 0 && (idx >= m_queue.Count || idx == -1) && m_queue[m_queue.Count - 1].Blueprint == queueItem.Blueprint)
			{
				queueItem.Amount += m_queue[m_queue.Count - 1].Amount;
				queueItem.ItemId = m_queue[m_queue.Count - 1].ItemId;
				if (m_currentQueueItem.HasValue && m_queue[m_queue.Count - 1].ItemId == m_currentQueueItem.Value.ItemId)
				{
					m_currentQueueItem = queueItem;
				}
				m_queue[m_queue.Count - 1] = queueItem;
			}
			else
			{
				if (idx == -1)
				{
					idx = m_queue.Count;
				}
				if (idx > m_queue.Count)
				{
					MyLog.Default.WriteLine("Production block.InsertQueueItem: Index out of bounds, desync!");
					idx = m_queue.Count;
				}
				queueItem.ItemId = NextItemId;
				m_queue.Insert(idx, queueItem);
			}
			if (num && Sync.IsServer)
			{
				m_realProductionStart = GetFramesFromLastTrigger();
			}
			UpdatePower();
			OnQueueChanged();
		}

		public QueueItem GetQueueItem(int idx)
		{
			return m_queue[idx];
		}

		public QueueItem? TryGetQueueItem(int idx)
		{
			if (!m_queue.IsValidIndex(idx))
			{
				return null;
			}
			return m_queue[idx];
		}

		public QueueItem? TryGetQueueItemById(uint itemId)
		{
			for (int i = 0; i < m_queue.Count; i++)
			{
				if (m_queue[i].ItemId == itemId)
				{
					return m_queue[i];
				}
			}
			return null;
		}

		protected virtual void RemoveQueueItem(int itemIdx)
		{
			if (itemIdx >= m_queue.Count)
			{
				MyLog.Default.WriteLine("Production block.RemoveQueueItem: Index out of bounds!");
				return;
			}
			m_queue.RemoveAt(itemIdx);
			UpdatePower();
			OnQueueChanged();
		}

		protected virtual void MoveQueueItem(uint queueItemId, int targetIdx)
		{
			for (int i = 0; i < m_queue.Count; i++)
			{
				if (m_queue[i].ItemId != queueItemId)
				{
					continue;
				}
				QueueItem item = m_queue[i];
				targetIdx = Math.Min(m_queue.Count - 1, targetIdx);
				if (i == targetIdx)
				{
					return;
				}
				m_queue.RemoveAt(i);
				int num = -1;
				if (m_queue.IsValidIndex(targetIdx - 1) && m_queue[targetIdx - 1].Blueprint == item.Blueprint)
				{
					num = targetIdx - 1;
				}
				if (m_queue.IsValidIndex(targetIdx) && m_queue[targetIdx].Blueprint == item.Blueprint)
				{
					num = targetIdx;
				}
				if (num != -1)
				{
					QueueItem value = m_queue[num];
					value.Amount += item.Amount;
					m_queue[num] = value;
				}
				else
				{
					m_queue.Insert(targetIdx, item);
				}
				break;
			}
			OnQueueChanged();
		}

		public QueueItem? TryGetFirstQueueItem()
		{
			return TryGetQueueItem(0);
		}

		public void ClearQueue(bool sendEvent = true)
		{
			if (Sync.IsServer)
			{
				ClearQueueRequest();
				if (sendEvent)
				{
					OnQueueChanged();
				}
			}
		}

		/// <summary>
		/// Swaps this and other queue. This operation is not synchronized. Those using
		/// this method should take care of proper synchronization.
		/// </summary>
		protected void SwapQueue(ref List<QueueItem> otherQueue)
		{
			List<QueueItem> queue = m_queue;
			m_queue = otherQueue;
			otherQueue = queue;
			OnQueueChanged();
		}

		protected override void Closing()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			base.Closing();
		}

		public override bool GetTimerEnabledState()
		{
			if (base.IsWorking)
			{
<<<<<<< HEAD
				return Enabled;
=======
				return base.Enabled;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return false;
		}

		public override MyInventoryBase GetInventoryBase(int index = 0)
		{
			return index switch
			{
				0 => InputInventory, 
				1 => OutputInventory, 
				_ => throw new InvalidBranchException(), 
			};
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(InputInventory);
			ReleaseInventory(OutputInventory);
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(InputInventory, damageContent: true);
			ReleaseInventory(OutputInventory, damageContent: true);
			base.OnDestroy();
		}

		private void OnComponentAdded(Type type, MyEntityComponentBase component)
		{
			MyInventoryAggregate myInventoryAggregate = component as MyInventoryAggregate;
			if (myInventoryAggregate == null)
			{
				return;
			}
			m_inventoryAggregate = myInventoryAggregate;
			m_inventoryAggregate.BeforeRemovedFromContainer += OnInventoryAggregateRemoved;
			m_inventoryAggregate.OnAfterComponentAdd += OnInventoryAddedToAggregate;
			m_inventoryAggregate.OnBeforeComponentRemove += OnBeforeInventoryRemovedFromAggregate;
			foreach (MyComponentBase item in m_inventoryAggregate.ChildList.Reader)
			{
				MyInventory inventory = item as MyInventory;
				OnInventoryAddedToAggregate(myInventoryAggregate, inventory);
			}
		}

		protected virtual void OnInventoryAddedToAggregate(MyInventoryAggregate aggregate, MyInventoryBase inventory)
		{
			if (m_inputInventory == null)
			{
				m_inputInventory = inventory as MyInventory;
			}
			else if (m_outputInventory == null)
			{
				m_outputInventory = inventory as MyInventory;
			}
		}

		private void OnInventoryAggregateRemoved(MyEntityComponentBase component)
		{
			m_inputInventory = null;
			m_outputInventory = null;
			m_inventoryAggregate.BeforeRemovedFromContainer -= OnInventoryAggregateRemoved;
			m_inventoryAggregate.OnAfterComponentAdd -= OnInventoryAddedToAggregate;
			m_inventoryAggregate.OnBeforeComponentRemove -= OnBeforeInventoryRemovedFromAggregate;
			m_inventoryAggregate = null;
		}

		protected virtual void OnBeforeInventoryRemovedFromAggregate(MyInventoryAggregate aggregate, MyInventoryBase inventory)
		{
			if (inventory == m_inputInventory)
			{
				m_inputInventory = null;
			}
			else if (inventory == m_outputInventory)
			{
				m_outputInventory = null;
			}
		}

		protected override void OnEnabledChanged()
		{
			UpdatePower();
			base.OnEnabledChanged();
			if (base.IsWorking && IsProducing)
			{
				OnStartProducing();
			}
		}

		private float ComputeRequiredPower()
		{
			if (!Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			if (!IsProducing || IsQueueEmpty)
			{
				return ProductionBlockDefinition.StandbyPowerConsumption;
			}
			return GetOperationalPowerConsumption();
		}

		protected virtual float GetOperationalPowerConsumption()
		{
			return ProductionBlockDefinition.OperationalPowerConsumption;
		}

		private void Receiver_IsPoweredChanged()
		{
			if (!base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				IsProducing = false;
			}
			UpdateIsWorking();
		}

		private void CubeBlock_IsWorkingChanged(MyCubeBlock block)
		{
			if (base.IsWorking && IsProducing)
			{
				OnStartProducing();
			}
		}

		protected void OnStartProducing()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.PlaySound(m_processSound, stopPrevious: true);
			}
			this.StartedProducing.InvokeIfNotNull();
		}

		protected void OnStopProducing()
		{
			if (m_soundEmitter != null)
			{
				if (base.IsWorking)
				{
					m_soundEmitter.StopSound(forced: false);
					m_soundEmitter.PlaySound(m_baseIdleSound, stopPrevious: false, skipIntro: true);
				}
				else
				{
					m_soundEmitter.StopSound(forced: false);
				}
			}
			this.StoppedProducing.InvokeIfNotNull();
		}

		public void FixInputOutputInventories(MyInventoryConstraint inputInventoryConstraint, MyInventoryConstraint outputInventoryConstraint)
		{
			if (m_inventoryAggregate.InventoryCount != 2)
			{
				MyInventoryAggregate component = MyInventoryAggregate.FixInputOutputInventories(m_inventoryAggregate, inputInventoryConstraint, outputInventoryConstraint);
				base.Components.Remove<MyInventoryBase>();
				m_outputInventory = null;
				m_inputInventory = null;
				base.Components.Add((MyInventoryBase)component);
			}
		}

		protected override void TiersChanged()
		{
			switch (base.CubeGrid.PlayerPresenceTier)
			{
			case MyUpdateTiersPlayerPresence.Normal:
				ChangeTimerTick(GetTimerTime(0));
				break;
			case MyUpdateTiersPlayerPresence.Tier1:
				ChangeTimerTick(GetTimerTime(1));
				break;
			case MyUpdateTiersPlayerPresence.Tier2:
				ChangeTimerTick(GetTimerTime(2));
				break;
			}
		}

		protected override uint GetDefaultTimeForUpdateTimer(int index)
		{
<<<<<<< HEAD
			switch (index)
			{
			case 0:
				return TIMER_NORMAL_IN_FRAMES;
			case 1:
				return TIMER_TIER1_IN_FRAMES;
			case 2:
				return TIMER_TIER2_IN_FRAMES;
			default:
				return 0u;
			}
=======
			return index switch
			{
				0 => TIMER_NORMAL_IN_FRAMES, 
				1 => TIMER_TIER1_IN_FRAMES, 
				2 => TIMER_TIER2_IN_FRAMES, 
				_ => 0u, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return this.GetInventory(index);
		}

		public virtual PullInformation GetPullInformation()
		{
			return new PullInformation
			{
				OwnerID = base.OwnerId,
				Inventory = InputInventory,
				Constraint = InputInventory.Constraint
			};
		}

		public virtual PullInformation GetPushInformation()
		{
			return new PullInformation
			{
				OwnerID = base.OwnerId,
				Inventory = OutputInventory,
				Constraint = OutputInventory.Constraint
			};
		}

		public virtual bool AllowSelfPulling()
		{
			return false;
		}

		public void InitializeConveyorEndpoint()
		{
			m_multilineConveyorEndpoint = new MyMultilineConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_multilineConveyorEndpoint));
		}

		bool Sandbox.ModAPI.IMyProductionBlock.CanUseBlueprint(MyDefinitionBase blueprint)
		{
			return CanUseBlueprint(blueprint as MyBlueprintDefinition);
		}

		void Sandbox.ModAPI.IMyProductionBlock.AddQueueItem(MyDefinitionBase blueprint, MyFixedPoint amount)
		{
			AddQueueItemRequest(blueprint as MyBlueprintDefinition, amount);
		}

		void Sandbox.ModAPI.IMyProductionBlock.InsertQueueItem(int idx, MyDefinitionBase blueprint, MyFixedPoint amount)
		{
			InsertQueueItemRequest(idx, blueprint as MyBlueprintDefinition, amount);
		}

		List<MyProductionQueueItem> Sandbox.ModAPI.IMyProductionBlock.GetQueue()
		{
			List<MyProductionQueueItem> list = new List<MyProductionQueueItem>(m_queue.Count);
			foreach (QueueItem item2 in m_queue)
			{
				MyProductionQueueItem item = default(MyProductionQueueItem);
				item.Amount = item2.Amount;
				item.Blueprint = item2.Blueprint;
				item.ItemId = item2.ItemId;
				list.Add(item);
			}
			return list;
		}

		bool Sandbox.ModAPI.Ingame.IMyProductionBlock.CanUseBlueprint(MyDefinitionId blueprint)
		{
			MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(blueprint);
			if (blueprintDefinition == null)
			{
				return false;
			}
			return CanUseBlueprint(blueprintDefinition);
		}

		void Sandbox.ModAPI.Ingame.IMyProductionBlock.AddQueueItem(MyDefinitionId blueprint, MyFixedPoint amount)
		{
			MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(blueprint);
			AddQueueItemRequest(blueprintDefinition, amount);
		}

		void Sandbox.ModAPI.Ingame.IMyProductionBlock.AddQueueItem(MyDefinitionId blueprint, decimal amount)
		{
			MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(blueprint);
			AddQueueItemRequest(blueprintDefinition, (MyFixedPoint)amount);
		}

		void Sandbox.ModAPI.Ingame.IMyProductionBlock.AddQueueItem(MyDefinitionId blueprint, double amount)
		{
			MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(blueprint);
			AddQueueItemRequest(blueprintDefinition, (MyFixedPoint)amount);
		}

		void Sandbox.ModAPI.Ingame.IMyProductionBlock.InsertQueueItem(int idx, MyDefinitionId blueprint, MyFixedPoint amount)
		{
			MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(blueprint);
			InsertQueueItemRequest(idx, blueprintDefinition, amount);
		}

		void Sandbox.ModAPI.Ingame.IMyProductionBlock.InsertQueueItem(int idx, MyDefinitionId blueprint, decimal amount)
		{
			MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(blueprint);
			InsertQueueItemRequest(idx, blueprintDefinition, (MyFixedPoint)amount);
		}

		void Sandbox.ModAPI.Ingame.IMyProductionBlock.InsertQueueItem(int idx, MyDefinitionId blueprint, double amount)
		{
			MyBlueprintDefinitionBase blueprintDefinition = MyDefinitionManager.Static.GetBlueprintDefinition(blueprint);
			InsertQueueItemRequest(idx, blueprintDefinition, (MyFixedPoint)amount);
		}

		void Sandbox.ModAPI.Ingame.IMyProductionBlock.RemoveQueueItem(int idx, MyFixedPoint amount)
		{
			RemoveQueueItemRequest(idx, amount);
		}

		void Sandbox.ModAPI.Ingame.IMyProductionBlock.RemoveQueueItem(int idx, decimal amount)
		{
			RemoveQueueItemRequest(idx, (MyFixedPoint)amount);
		}

		void Sandbox.ModAPI.Ingame.IMyProductionBlock.RemoveQueueItem(int idx, double amount)
		{
			RemoveQueueItemRequest(idx, (MyFixedPoint)amount);
		}

		void Sandbox.ModAPI.Ingame.IMyProductionBlock.ClearQueue()
		{
			ClearQueueRequest();
		}

		void Sandbox.ModAPI.Ingame.IMyProductionBlock.GetQueue(List<MyProductionItem> items)
		{
			items.Clear();
			for (int i = 0; i < m_queue.Count; i++)
			{
				QueueItem queueItem = m_queue[i];
				items.Add(new MyProductionItem(queueItem.ItemId, queueItem.Blueprint.Id, queueItem.Amount));
			}
		}
	}
}
