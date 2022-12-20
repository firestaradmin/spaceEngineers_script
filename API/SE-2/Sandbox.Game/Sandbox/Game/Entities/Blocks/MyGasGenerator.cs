using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
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
using VRage.Sync;
using VRage.Utils;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_OxygenGenerator))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyGasGenerator),
		typeof(Sandbox.ModAPI.Ingame.IMyGasGenerator)
	})]
	public class MyGasGenerator : MyFunctionalBlock, IMyGasBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyGasGenerator, Sandbox.ModAPI.Ingame.IMyGasGenerator, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, IMyInventoryOwner, IMyEventProxy, IMyEventOwner
	{
		protected sealed class OnRefillCallback_003C_003E : ICallSite<MyGasGenerator, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyGasGenerator @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRefillCallback();
			}
		}

		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = (useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyGasGenerator)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		protected class m_autoRefill_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType autoRefill;
				ISyncType result = (autoRefill = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyGasGenerator)P_0).m_autoRefill = (Sync<bool, SyncDirection.BothWays>)autoRefill;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyGasGenerator_003C_003EActor : IActivator, IActivator<MyGasGenerator>
		{
			private sealed override object CreateInstance()
			{
				return new MyGasGenerator();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGasGenerator CreateInstance()
			{
				return new MyGasGenerator();
			}

			MyGasGenerator IActivator<MyGasGenerator>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly int NUMBER_PULLS_BOTTLES = 3;

		private readonly uint TIMER_NORMAL_IN_FRAMES = 300u;

		private readonly uint TIMER_TIER1_IN_FRAMES = 600u;

		private readonly uint TIMER_TIER2_IN_FRAMES = 1200u;

		private float m_productionCapacityMultiplier = 1f;

		private float m_powerConsumptionMultiplier = 1f;

		private float m_actualCheckFillValue;

		private float m_iceAmount;

		private int m_numberOfPullsForBottles = NUMBER_PULLS_BOTTLES;

		private readonly Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		private readonly Sync<bool, SyncDirection.BothWays> m_autoRefill;

		private bool m_isProducing;

		private MyInventoryConstraint m_oreConstraint;

		private MyInventoryConstraint m_containersConstraint;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private MyResourceSourceComponent m_sourceComp;

		private new MyOxygenGeneratorDefinition BlockDefinition => (MyOxygenGeneratorDefinition)base.BlockDefinition;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

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

		public bool CanPressurizeRoom => false;

		public bool CanProduce
		{
			get
			{
<<<<<<< HEAD
				if (((MySession.Static != null && MySession.Static.Settings.EnableOxygen) || !BlockDefinition.IsOxygenOnly) && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.IsWorking && Enabled)
=======
				if (((MySession.Static != null && MySession.Static.Settings.EnableOxygen) || !BlockDefinition.IsOxygenOnly) && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.IsWorking && base.Enabled)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return base.IsFunctional;
				}
				return false;
			}
		}

		public bool AutoRefill
		{
			get
			{
				return m_autoRefill;
			}
			set
			{
				m_autoRefill.Value = value;
			}
		}

		public MyResourceSourceComponent SourceComp
		{
			get
			{
				return m_sourceComp;
			}
			set
			{
				if (base.Components.Contains(typeof(MyResourceSourceComponent)))
				{
					base.Components.Remove<MyResourceSourceComponent>();
				}
				base.Components.Add(value);
				m_sourceComp = value;
			}
		}

		public override bool IsTieredUpdateSupported => true;

		float Sandbox.ModAPI.IMyGasGenerator.ProductionCapacityMultiplier
		{
			get
			{
				return m_productionCapacityMultiplier;
			}
			set
			{
				m_productionCapacityMultiplier = value;
				if (m_productionCapacityMultiplier < 0.01f)
				{
					m_productionCapacityMultiplier = 0.01f;
				}
			}
		}

		float Sandbox.ModAPI.IMyGasGenerator.PowerConsumptionMultiplier
		{
			get
			{
				return m_powerConsumptionMultiplier;
			}
			set
			{
				m_powerConsumptionMultiplier = value;
				if (m_powerConsumptionMultiplier < 0.01f)
				{
					m_powerConsumptionMultiplier = 0.01f;
				}
				if (base.ResourceSink != null)
				{
					base.ResourceSink.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, BlockDefinition.OperationalPowerConsumption * m_powerConsumptionMultiplier);
					base.ResourceSink.Update();
				}
			}
		}

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

		public MyGasGenerator()
		{
			CreateTerminalControls();
			SourceComp = new MyResourceSourceComponent(2);
			base.ResourceSink = new MyResourceSinkComponent();
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyGasGenerator>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyGasGenerator> obj = new MyTerminalControlOnOffSwitch<MyGasGenerator>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = (MyGasGenerator x) => x.UseConveyorSystem,
					Setter = delegate(MyGasGenerator x, bool v)
					{
						x.UseConveyorSystem = v;
					}
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlButton<MyGasGenerator> obj2 = new MyTerminalControlButton<MyGasGenerator>("Refill", MySpaceTexts.BlockPropertyTitle_Refill, MySpaceTexts.BlockPropertyTitle_Refill, OnRefillButtonPressed)
				{
					Enabled = (MyGasGenerator x) => x.CanRefill()
				};
				obj2.EnableAction();
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlCheckbox<MyGasGenerator> obj3 = new MyTerminalControlCheckbox<MyGasGenerator>("Auto-Refill", MySpaceTexts.BlockPropertyTitle_AutoRefill, MySpaceTexts.BlockPropertyTitle_AutoRefill)
				{
					Getter = (MyGasGenerator x) => x.AutoRefill,
					Setter = delegate(MyGasGenerator x, bool v)
					{
						x.AutoRefill = v;
					}
				};
				obj3.EnableAction();
				MyTerminalControlFactory.AddControl(obj3);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			base.SyncFlag = true;
			List<MyResourceSourceInfo> list = new List<MyResourceSourceInfo>();
			foreach (MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo producedGase in BlockDefinition.ProducedGases)
			{
				list.Add(new MyResourceSourceInfo
				{
					ResourceTypeId = producedGase.Id,
					DefinedOutput = BlockDefinition.IceConsumptionPerSecond * producedGase.IceToGasRatio * (MySession.Static.CreativeMode ? 10f : 1f),
					ProductionToCapacityMultiplier = 1f
				});
			}
			SourceComp.Init(BlockDefinition.ResourceSourceGroup, list);
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_OxygenGenerator myObjectBuilder_OxygenGenerator = objectBuilder as MyObjectBuilder_OxygenGenerator;
			InitializeConveyorEndpoint();
			m_useConveyorSystem.SetLocalValue(myObjectBuilder_OxygenGenerator.UseConveyorSystem);
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
			MyInventory myInventory = this.GetInventory();
			if (myInventory == null)
			{
				myInventory = new MyInventory(BlockDefinition.InventoryMaxVolume, BlockDefinition.InventorySize, MyInventoryFlags.CanReceive);
				myInventory.Constraint = BlockDefinition.InputInventoryConstraint;
				base.Components.Add((MyInventoryBase)myInventory);
			}
			else
			{
				myInventory.Constraint = BlockDefinition.InputInventoryConstraint;
			}
			ResetActualCheckFillValue(myInventory);
			m_oreConstraint = new MyInventoryConstraint(myInventory.Constraint.Description, myInventory.Constraint.Icon, myInventory.Constraint.IsWhitelist);
			m_containersConstraint = new MyInventoryConstraint(myInventory.Constraint.Description, myInventory.Constraint.Icon, myInventory.Constraint.IsWhitelist);
<<<<<<< HEAD
			foreach (MyDefinitionId constrainedId in myInventory.Constraint.ConstrainedIds)
			{
				if (constrainedId.TypeId == typeof(MyObjectBuilder_Ore))
=======
			Enumerator<MyDefinitionId> enumerator2 = myInventory.Constraint.ConstrainedIds.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyDefinitionId current2 = enumerator2.get_Current();
					if (current2.TypeId == typeof(MyObjectBuilder_Ore))
					{
						m_oreConstraint.Add(current2);
					}
					else if (current2.TypeId == typeof(MyObjectBuilder_GasContainerObject) || current2.TypeId == typeof(MyObjectBuilder_OxygenContainerObject))
					{
						m_containersConstraint.Add(current2);
					}
				}
				else if (constrainedId.TypeId == typeof(MyObjectBuilder_GasContainerObject) || constrainedId.TypeId == typeof(MyObjectBuilder_OxygenContainerObject))
				{
					m_containersConstraint.Add(constrainedId);
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
			}
			AutoRefill = myObjectBuilder_OxygenGenerator.AutoRefill;
			SourceComp.Enabled = Enabled;
			if (Sync.IsServer)
			{
				SourceComp.OutputChanged += Source_OutputChanged;
			}
			base.ResourceSink.Init(BlockDefinition.ResourceSinkGroup, new MyResourceSinkInfo
			{
				ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
				MaxRequiredInput = BlockDefinition.OperationalPowerConsumption,
				RequiredInputFunc = ComputeRequiredPower
			}, this);
			base.ResourceSink.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
			myInventory.Init(myObjectBuilder_OxygenGenerator.Inventory);
			base.ResourceSink.Update();
			SetDetailedInfoDirty();
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += MyGasGenerator_IsWorkingChanged;
			CreateUpdateTimer(GetTimerTime(0), MyTimerTypes.Frame10);
			m_iceAmount = IceAmount();
		}

		private void ResetActualCheckFillValue(MyInventory inventory)
		{
			if (BlockDefinition.InventoryFillFactorMin > inventory.VolumeFillFactor)
			{
				m_actualCheckFillValue = BlockDefinition.InventoryFillFactorMax;
			}
			else
			{
				m_actualCheckFillValue = BlockDefinition.InventoryFillFactorMin;
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (base.CubeGrid != null && Sync.IsServer && !MySession.Static.SimplifiedSimulation)
			{
				MyCubeGrid cubeGrid = base.CubeGrid;
				cubeGrid.OnAnyBlockInventoryChanged = (Action<MyInventoryBase>)Delegate.Combine(cubeGrid.OnAnyBlockInventoryChanged, new Action<MyInventoryBase>(OnAnyBlockInventoryChanged));
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (base.CubeGrid != null && Sync.IsServer && !MySession.Static.SimplifiedSimulation)
			{
				MyCubeGrid cubeGrid = base.CubeGrid;
				cubeGrid.OnAnyBlockInventoryChanged = (Action<MyInventoryBase>)Delegate.Remove(cubeGrid.OnAnyBlockInventoryChanged, new Action<MyInventoryBase>(OnAnyBlockInventoryChanged));
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_OxygenGenerator obj = (MyObjectBuilder_OxygenGenerator)base.GetObjectBuilderCubeBlock(copy);
			obj.UseConveyorSystem = m_useConveyorSystem;
			obj.AutoRefill = AutoRefill;
			return obj;
		}

		public void RefillBottles()
		{
			List<MyPhysicalInventoryItem> items = this.GetInventory().GetItems();
			foreach (MyDefinitionId resourceType in SourceComp.ResourceTypes)
			{
				MyDefinitionId gasId = resourceType;
				double num = 0.0;
				if (MySession.Static.CreativeMode)
				{
					num = 3.4028234663852886E+38;
				}
				else
				{
					foreach (MyPhysicalInventoryItem item in items)
					{
						if (!(item.Content is MyObjectBuilder_GasContainerObject))
						{
							num += IceToGas(ref gasId, (float)item.Amount) * (double)((Sandbox.ModAPI.IMyGasGenerator)this).ProductionCapacityMultiplier;
						}
					}
				}
				double num2 = 0.0;
				foreach (MyPhysicalInventoryItem item2 in items)
				{
					if (num <= 0.0)
					{
						return;
					}
					MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item2.Content as MyObjectBuilder_GasContainerObject;
					if (myObjectBuilder_GasContainerObject != null && !(myObjectBuilder_GasContainerObject.GasLevel >= 1f))
					{
						MyOxygenContainerDefinition myOxygenContainerDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(myObjectBuilder_GasContainerObject) as MyOxygenContainerDefinition;
						if (!(myOxygenContainerDefinition.StoredGasId != resourceType))
						{
							float num3 = myObjectBuilder_GasContainerObject.GasLevel * myOxygenContainerDefinition.Capacity;
							double num4 = Math.Min(myOxygenContainerDefinition.Capacity - num3, num);
							myObjectBuilder_GasContainerObject.GasLevel = (float)Math.Min(((double)num3 + num4) / (double)myOxygenContainerDefinition.Capacity, 1.0);
							num2 += num4;
							num -= num4;
						}
					}
				}
				if (num2 > 0.0)
				{
					ConsumeFuel(ref gasId, num2);
				}
			}
		}

		private static void OnRefillButtonPressed(MyGasGenerator generator)
		{
			if (generator.IsWorking)
			{
				generator.SendRefillRequest();
			}
		}

		private bool CanRefill()
		{
			if (!CanProduce)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			foreach (MyPhysicalInventoryItem item in this.GetInventory().GetItems())
			{
				MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
				if (myObjectBuilder_GasContainerObject != null)
<<<<<<< HEAD
				{
					if (myObjectBuilder_GasContainerObject.GasLevel < 1f)
					{
						flag = true;
					}
				}
				else if (item.Content is MyObjectBuilder_Ore)
				{
=======
				{
					if (myObjectBuilder_GasContainerObject.GasLevel < 1f)
					{
						flag = true;
					}
				}
				else if (item.Content is MyObjectBuilder_Ore)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					flag2 = true;
				}
			}
			return flag && flag2;
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			SetRemainingCapacities();
			base.ResourceSink.Update();
			SetEmissiveStateWorking();
			CheckProducigState();
			if (MyFakes.ENABLE_OXYGEN_SOUNDS)
			{
				UpdateSounds();
			}
<<<<<<< HEAD
=======
			CheckProducigState();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!m_isProducing && !base.HasDamageEffect)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		private void CheckProducigState()
		{
			m_isProducing = false;
			foreach (MyDefinitionId resourceType in SourceComp.ResourceTypes)
			{
				m_isProducing |= SourceComp.CurrentOutputByType(resourceType) > 0f;
			}
		}

		private void UpdateSounds()
		{
<<<<<<< HEAD
			if (m_soundEmitter == null || Sandbox.Engine.Platform.Game.IsDedicated || base.CubeGrid == null || base.CubeGrid.IsPreview || base.CubeGrid.Physics == null)
=======
			if (m_soundEmitter == null || Sandbox.Engine.Platform.Game.IsDedicated)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			if (base.IsWorking)
			{
				if (m_isProducing)
				{
					if (m_soundEmitter.SoundId != BlockDefinition.GenerateSound.Arcade && m_soundEmitter.SoundId != BlockDefinition.GenerateSound.Realistic)
					{
						m_soundEmitter.PlaySound(BlockDefinition.GenerateSound, stopPrevious: true);
					}
				}
				else if (m_soundEmitter.SoundId != BlockDefinition.IdleSound.Arcade && m_soundEmitter.SoundId != BlockDefinition.IdleSound.Realistic && (m_soundEmitter.SoundId == BlockDefinition.GenerateSound.Arcade || m_soundEmitter.SoundId == BlockDefinition.GenerateSound.Realistic) && m_soundEmitter.Loop)
				{
					m_soundEmitter.StopSound(forced: false);
				}
				if (!m_soundEmitter.IsPlaying)
				{
					m_soundEmitter.PlaySound(BlockDefinition.IdleSound, stopPrevious: true);
				}
			}
			else if (m_soundEmitter.IsPlaying)
			{
				m_soundEmitter.StopSound(forced: false);
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
<<<<<<< HEAD
			if (Sync.IsServer && !MySession.Static.SimplifiedSimulation && Enabled && base.IsWorking && !MySession.Static.SimplifiedSimulation)
=======
			if (Sync.IsServer && !MySession.Static.SimplifiedSimulation && base.Enabled && base.IsWorking && !MySession.Static.SimplifiedSimulation)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if ((bool)m_useConveyorSystem)
				{
					GetIceFromConveyorSystem();
					GetBottlesFromConveyorSystem();
				}
				if (AutoRefill && CanRefill())
				{
					RefillBottles();
				}
			}
			if (base.HasDamageEffect)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		private void GetBottlesFromConveyorSystem()
		{
			if (m_numberOfPullsForBottles != 0)
			{
				MyInventory inventory = this.GetInventory();
				if (inventory.VolumeFillFactor < 0.6f && base.CubeGrid.GridSystems.ConveyorSystem.PullItems(m_containersConstraint, 2, this, inventory) == 0)
				{
					m_numberOfPullsForBottles--;
				}
			}
		}

		private void OnAnyBlockInventoryChanged(MyInventoryBase inv)
		{
			if (inv != null && inv.Entity != this)
			{
				m_numberOfPullsForBottles = NUMBER_PULLS_BOTTLES;
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.CheckIsWorking())
			{
				return base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
			}
			return false;
		}

		private float ComputeRequiredPower()
		{
<<<<<<< HEAD
			if ((!MySession.Static.Settings.EnableOxygen && BlockDefinition.IsOxygenOnly) || !Enabled || !base.IsFunctional)
=======
			if ((!MySession.Static.Settings.EnableOxygen && BlockDefinition.IsOxygenOnly) || !base.Enabled || !base.IsFunctional)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return 0f;
			}
			bool flag = false;
			foreach (MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo producedGase in BlockDefinition.ProducedGases)
			{
				flag = flag || (SourceComp.CurrentOutputByType(producedGase.Id) > 0f && (MySession.Static.Settings.EnableOxygen || producedGase.Id != MyOxygenGeneratorDefinition.OxygenGasId));
			}
			return (flag ? BlockDefinition.OperationalPowerConsumption : BlockDefinition.StandbyPowerConsumption) * m_powerConsumptionMultiplier;
		}

		private void SetRemainingCapacities()
		{
			foreach (MyDefinitionId resourceType in SourceComp.ResourceTypes)
			{
				MyDefinitionId gasId = resourceType;
				m_sourceComp.SetRemainingCapacityByType(resourceType, (float)IceToGas(ref gasId, m_iceAmount));
			}
			if (MySession.Static != null && !MySession.Static.Settings.EnableOxygen)
			{
				m_sourceComp.SetMaxOutputByType(MyOxygenGeneratorDefinition.OxygenGasId, 0f);
			}
		}

		private float IceAmount()
		{
			if (MySession.Static.CreativeMode)
			{
				return 10000f;
			}
			List<MyPhysicalInventoryItem> items = this.GetInventory().GetItems();
			MyFixedPoint myFixedPoint = 0;
			foreach (MyPhysicalInventoryItem item in items)
			{
				if (!(item.Content is MyObjectBuilder_GasContainerObject))
				{
					myFixedPoint += item.Amount;
				}
			}
			return (float)myFixedPoint;
		}

		private void Inventory_ContentsChanged(MyInventoryBase obj)
		{
			m_iceAmount = IceAmount();
			SetRemainingCapacities();
			RaisePropertiesChanged();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		private void MyGasGenerator_IsWorkingChanged(MyCubeBlock obj)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.Closed)
				{
					SourceComp.Enabled = CanProduce;
					SetEmissiveStateWorking();
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				}
			}, "MyGasGenerator_IsWorkingChanged");
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			SourceComp.Enabled = CanProduce;
			base.ResourceSink.Update();
			if (base.CubeGrid.GridSystems.ResourceDistributor != null)
			{
				base.CubeGrid.GridSystems.ResourceDistributor.ConveyorSystem_OnPoweredChanged();
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			SourceComp.Enabled = CanProduce;
			base.ResourceSink.Update();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		private void Source_OutputChanged(MyDefinitionId changedResourceId, float oldOutput, MyResourceSourceComponent source)
		{
			if (!BlockDefinition.ProducedGases.TrueForAll((MyOxygenGeneratorDefinition.MyGasGeneratorResourceInfo info) => info.Id != changedResourceId))
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		protected override void Closing()
		{
			base.Closing();
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			SetEmissiveStateWorking();
		}

		public override bool SetEmissiveStateWorking()
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return false;
			}
			if (CanProduce)
			{
				MyInventory inventory = this.GetInventory();
				if (inventory == null)
				{
					return false;
				}
				if (!inventory.FindItem((MyPhysicalInventoryItem item) => !(item.Content is MyObjectBuilder_GasContainerObject)).HasValue)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, base.Render.RenderObjectIDs[0]);
				}
				if (m_isProducing)
				{
					return SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, base.Render.RenderObjectIDs[0]);
				}
				return SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
			}
			return false;
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			if (!MySession.Static.Settings.EnableOxygen)
			{
				detailedInfo.Append("\n");
				detailedInfo.Append("Oxygen disabled in world settings!");
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			CheckEmissiveState(force: true);
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory(), damageContent: true);
			base.OnDestroy();
		}

		public void SetInventory(MyInventory inventory, int index)
		{
			throw new NotImplementedException("TODO Dusan inventory sync");
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
			MyInventory myInventory = inventory as MyInventory;
			if (myInventory != null)
			{
				myInventory.ContentsChanged += Inventory_ContentsChanged;
			}
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentRemoved(inventory);
			MyInventory myInventory = inventory as MyInventory;
			if (myInventory != null)
			{
				myInventory.ContentsChanged -= Inventory_ContentsChanged;
			}
		}

		bool IMyGasBlock.IsWorking()
		{
			return CanProduce;
		}

		public override bool GetTimerEnabledState()
		{
<<<<<<< HEAD
			if (Enabled && base.IsWorking && m_isProducing)
=======
			if (base.Enabled && base.IsWorking && m_isProducing)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return !MySession.Static.SimplifiedSimulation;
			}
			return false;
		}

		public override void DoUpdateTimerTick()
		{
			base.DoUpdateTimerTick();
			if (MySession.Static.CreativeMode || !base.IsWorking)
			{
				return;
			}
			foreach (MyDefinitionId resourceType in SourceComp.ResourceTypes)
			{
				MyDefinitionId gasId = resourceType;
				uint framesFromLastTrigger = GetFramesFromLastTrigger();
				double gasAmount = GasOutputPerSecond(ref gasId) * (double)((float)framesFromLastTrigger / 60f);
				ConsumeFuel(ref gasId, gasAmount);
			}
		}

		private void GetIceFromConveyorSystem()
		{
<<<<<<< HEAD
=======
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyInventory inventory = this.GetInventory();
			if (inventory == null)
			{
				return;
			}
			if (m_actualCheckFillValue > inventory.VolumeFillFactor)
			{
<<<<<<< HEAD
				foreach (MyDefinitionId constrainedId in m_oreConstraint.ConstrainedIds)
				{
					float num = BlockDefinition.IceConsumptionPerSecond * 60f * BlockDefinition.FuelPullAmountFromConveyorInMinutes;
					base.CubeGrid.GridSystems.ConveyorSystem.PullItem(constrainedId, (MyFixedPoint)num, this, inventory, remove: false, calcImmediately: false);
				}
				if (m_actualCheckFillValue == BlockDefinition.InventoryFillFactorMin)
				{
=======
				Enumerator<MyDefinitionId> enumerator = m_oreConstraint.ConstrainedIds.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyDefinitionId current = enumerator.get_Current();
						float num = BlockDefinition.IceConsumptionPerSecond * 60f * BlockDefinition.FuelPullAmountFromConveyorInMinutes;
						base.CubeGrid.GridSystems.ConveyorSystem.PullItem(current, (MyFixedPoint)num, this, inventory, remove: false, calcImmediately: false);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				if (m_actualCheckFillValue == BlockDefinition.InventoryFillFactorMin)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_actualCheckFillValue = BlockDefinition.InventoryFillFactorMax;
				}
			}
			else if (m_actualCheckFillValue == BlockDefinition.InventoryFillFactorMax)
			{
				m_actualCheckFillValue = BlockDefinition.InventoryFillFactorMin;
			}
		}

		private void ConsumeFuel(ref MyDefinitionId gasTypeId, double gasAmount)
		{
			if (gasAmount <= 0.0 || !Sync.IsServer || MySession.Static.CreativeMode)
			{
				return;
			}
			double num = GasToIce(ref gasTypeId, gasAmount);
			if (num <= 0.0)
			{
				return;
			}
			MyInventory inventory = this.GetInventory();
			if (inventory == null)
			{
				return;
			}
			List<MyPhysicalInventoryItem> items = inventory.GetItems();
			if (items.Count <= 0)
			{
				return;
			}
			int num2 = 0;
			while (num2 < items.Count)
			{
				MyPhysicalInventoryItem myPhysicalInventoryItem = items[num2];
				if (myPhysicalInventoryItem.Content is MyObjectBuilder_GasContainerObject)
				{
					num2++;
					continue;
				}
				if (num < (double)(float)myPhysicalInventoryItem.Amount)
				{
					MyFixedPoint value = MyFixedPoint.Max((MyFixedPoint)num, MyFixedPoint.SmallestPossibleValue);
					inventory.RemoveItems(myPhysicalInventoryItem.ItemId, value);
					break;
				}
				num -= (double)(float)myPhysicalInventoryItem.Amount;
				inventory.RemoveItems(myPhysicalInventoryItem.ItemId);
			}
		}

		private double GasOutputPerSecond(ref MyDefinitionId gasId)
		{
			Sandbox.ModAPI.IMyGasGenerator myGasGenerator;
			if ((myGasGenerator = this) != null)
			{
				return SourceComp.CurrentOutputByType(gasId) * myGasGenerator.ProductionCapacityMultiplier;
			}
			return 0.0;
		}

		private double GasOutputPerUpdate(ref MyDefinitionId gasId)
		{
			return GasOutputPerSecond(ref gasId) * 0.01666666753590107;
		}

		private double IceToGas(ref MyDefinitionId gasId, double iceAmount)
		{
			return iceAmount * IceToGasRatio(ref gasId);
		}

		private double GasToIce(ref MyDefinitionId gasId, double gasAmount)
		{
			return gasAmount / IceToGasRatio(ref gasId);
		}

		private double IceToGasRatio(ref MyDefinitionId gasId)
		{
			return SourceComp.DefinedOutputByType(gasId) / BlockDefinition.IceConsumptionPerSecond;
		}

		public void SendRefillRequest()
		{
			MyMultiplayer.RaiseEvent(this, (MyGasGenerator x) => x.OnRefillCallback);
		}

		[Event(null, 887)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnRefillCallback()
		{
			RefillBottles();
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

		public PullInformation GetPullInformation()
		{
			PullInformation obj = new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId
			};
			obj.Constraint = obj.Inventory.Constraint;
			return obj;
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}
	}
}
