using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
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
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_OxygenTank))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyGasTank),
		typeof(Sandbox.ModAPI.Ingame.IMyGasTank)
	})]
	public class MyGasTank : MyFunctionalBlock, IMyGasBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyGasTank, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyGasTank, IMyInventoryOwner, Sandbox.Game.Entities.Interfaces.IMyGasTank
	{
		protected sealed class OnStockipleModeCallback_003C_003ESystem_Boolean : ICallSite<MyGasTank, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyGasTank @this, in bool newStockpileMode, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnStockipleModeCallback(newStockpileMode);
			}
		}

		protected sealed class OnFilledRatioCallback_003C_003ESystem_Double : ICallSite<MyGasTank, double, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyGasTank @this, in double newFilledRatio, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnFilledRatioCallback(newFilledRatio);
			}
		}

		protected sealed class OnRefillCallback_003C_003E : ICallSite<MyGasTank, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyGasTank @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRefillCallback();
			}
		}

		protected class m_autoRefill_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType autoRefill;
				ISyncType result = (autoRefill = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyGasTank)P_0).m_autoRefill = (Sync<bool, SyncDirection.BothWays>)autoRefill;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyGasTank_003C_003EActor : IActivator, IActivator<MyGasTank>
		{
			private sealed override object CreateInstance()
			{
				return new MyGasTank();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGasTank CreateInstance()
			{
				return new MyGasTank();
			}

			MyGasTank IActivator<MyGasTank>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

<<<<<<< HEAD
=======
		private readonly uint TIMER_NORMAL_IN_FRAMES = 100u;

		private readonly uint TIMER_TIER1_IN_FRAMES = 300u;

		private readonly uint TIMER_TIER2_IN_FRAMES = 600u;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static readonly string[] m_emissiveTextureNames = new string[4] { "Emissive0", "Emissive1", "Emissive2", "Emissive3" };

		private Color m_prevColor = Color.White;

		private int m_prevFillCount = -1;

		private readonly Sync<bool, SyncDirection.BothWays> m_autoRefill;

		private const float m_maxFillPerSecond = 0.05f;

		private bool m_syncNeeded;

		private double m_oldInLitres;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private bool m_isStockpiling;

		private double m_FilledRatio;

		private float m_leakPercent;

		private bool m_isEmptyDamageEffectStarted;

		private MyParticleEffect m_emptyDamageEffect;

		private MyResourceSourceComponent m_sourceComp;

		private readonly MyDefinitionId m_oxygenGasId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen");

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public bool IsStockpiling
		{
			get
			{
				return m_isStockpiling;
			}
			private set
			{
				SetStockpilingState(value);
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyGasTank.Stockpile
		{
			get
			{
				return IsStockpiling;
			}
			set
			{
				ChangeStockpileMode(value);
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyGasTank.AutoRefillBottles
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

		public bool CanStore
		{
			get
			{
				if (((MySession.Static != null && MySession.Static.Settings.EnableOxygen) || BlockDefinition.StoredGasId != m_oxygenGasId) && base.IsWorking && Enabled)
				{
					return base.IsFunctional;
				}
				return false;
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

		public new MyGasTankDefinition BlockDefinition => (MyGasTankDefinition)base.BlockDefinition;

		private float GasOutputPerSecond
		{
			get
			{
				if (!SourceComp.ProductionEnabledByType(BlockDefinition.StoredGasId))
				{
					return 0f;
				}
				return SourceComp.CurrentOutputByType(BlockDefinition.StoredGasId);
			}
		}

		private float GasInputPerSecond => base.ResourceSink.CurrentInputByType(BlockDefinition.StoredGasId);

		private float GasOutputPerUpdate => GasOutputPerSecond * 0.0166666675f;

		private float GasInputPerUpdate => GasInputPerSecond * 0.0166666675f;

		public float Capacity => BlockDefinition.Capacity;

		public float GasCapacity => Capacity;

		public double FilledRatio
		{
			get
			{
				return m_FilledRatio;
			}
			private set
			{
				if (m_FilledRatio != value)
				{
					FilledRatioChanged.InvokeIfNotNull();
				}
				m_FilledRatio = value;
			}
		}

		public Action FilledRatioChanged { get; set; }

		public bool CanPressurizeRoom => false;

		public override bool IsTieredUpdateSupported => true;

		int IMyInventoryOwner.InventoryCount => base.InventoryCount;

		long IMyInventoryOwner.EntityId => base.EntityId;

		bool IMyInventoryOwner.HasInventory => base.HasInventory;

		bool IMyInventoryOwner.UseConveyorSystem
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public MyGasTank()
		{
			CreateTerminalControls();
			SourceComp = new MyResourceSourceComponent();
			base.ResourceSink = new MyResourceSinkComponent(2);
			m_autoRefill.ValueChanged += delegate
			{
				OnAutoRefillChanged();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyGasTank>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyGasTank> obj = new MyTerminalControlOnOffSwitch<MyGasTank>("Stockpile", MySpaceTexts.BlockPropertyTitle_Stockpile, MySpaceTexts.BlockPropertyDescription_Stockpile)
				{
					Getter = (MyGasTank x) => x.IsStockpiling,
					Setter = delegate(MyGasTank x, bool v)
					{
						x.ChangeStockpileMode(v);
					}
				};
				obj.EnableToggleAction();
				obj.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlButton<MyGasTank> obj2 = new MyTerminalControlButton<MyGasTank>("Refill", MySpaceTexts.BlockPropertyTitle_Refill, MySpaceTexts.BlockPropertyTitle_Refill, OnRefillButtonPressed)
				{
					Enabled = (MyGasTank x) => x.CanRefill()
				};
				obj2.EnableAction();
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlCheckbox<MyGasTank> obj3 = new MyTerminalControlCheckbox<MyGasTank>("Auto-Refill", MySpaceTexts.BlockPropertyTitle_AutoRefill, MySpaceTexts.BlockPropertyTitle_AutoRefill)
				{
					Getter = (MyGasTank x) => x.m_autoRefill,
					Setter = delegate(MyGasTank x, bool v)
					{
						x.m_autoRefill.Value = v;
					}
				};
				obj3.EnableAction();
				MyTerminalControlFactory.AddControl(obj3);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_OxygenTank myObjectBuilder_OxygenTank = (MyObjectBuilder_OxygenTank)objectBuilder;
			InitializeConveyorEndpoint();
			if (Sync.IsServer)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
				if (this.GetInventory() != null)
				{
					this.GetInventory().Constraint = BlockDefinition.InputInventoryConstraint;
				}
			}
			MyInventory myInventory = this.GetInventory();
			if (myInventory == null)
			{
				myInventory = new MyInventory(BlockDefinition.InventoryMaxVolume, BlockDefinition.InventorySize, MyInventoryFlags.CanReceive);
				myInventory.Constraint = BlockDefinition.InputInventoryConstraint;
				base.Components.Add((MyInventoryBase)myInventory);
				myInventory.Init(myObjectBuilder_OxygenTank.Inventory);
			}
			myInventory.ContentsChanged += MyGasTank_ContentsChanged;
			m_autoRefill.SetLocalValue(myObjectBuilder_OxygenTank.AutoRefill);
			List<MyResourceSourceInfo> sourceResourceData = new List<MyResourceSourceInfo>
			{
				new MyResourceSourceInfo
				{
					ResourceTypeId = BlockDefinition.StoredGasId,
					DefinedOutput = 0.05f * BlockDefinition.Capacity
				}
			};
			SourceComp.Init(BlockDefinition.ResourceSourceGroup, sourceResourceData);
			SourceComp.OutputChanged += Source_OutputChanged;
			SourceComp.Enabled = Enabled;
			IsStockpiling = myObjectBuilder_OxygenTank.IsStockpiling;
			List<MyResourceSinkInfo> list = new List<MyResourceSinkInfo>();
			MyResourceSinkInfo item = new MyResourceSinkInfo
			{
				ResourceTypeId = MyResourceDistributorComponent.ElectricityId,
				MaxRequiredInput = BlockDefinition.OperationalPowerConsumption,
				RequiredInputFunc = ComputeRequiredPower
			};
			list.Add(item);
			item = new MyResourceSinkInfo
			{
				ResourceTypeId = BlockDefinition.StoredGasId,
				MaxRequiredInput = Capacity,
				RequiredInputFunc = ComputeRequiredGas
			};
			list.Add(item);
			List<MyResourceSinkInfo> sinkData = list;
			base.ResourceSink.Init(BlockDefinition.ResourceSinkGroup, sinkData, this);
			base.ResourceSink.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
			base.ResourceSink.CurrentInputChanged += Sink_CurrentInputChanged;
			float num = myObjectBuilder_OxygenTank.FilledRatio;
			if (MySession.Static.CreativeMode && num == 0f)
			{
				num = 0.5f;
			}
			ChangeFilledRatio(MathHelper.Clamp(num, 0f, 1f));
			base.ResourceSink.Update();
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.IsWorkingChanged += MyOxygenTank_IsWorkingChanged;
			m_leakPercent = BlockDefinition.LeakPercent;
		}

		private void MyGasTank_ContentsChanged(MyInventoryBase obj)
		{
			if ((bool)m_autoRefill && CanRefill())
			{
				RefillBottles();
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_OxygenTank obj = (MyObjectBuilder_OxygenTank)base.GetObjectBuilderCubeBlock(copy);
			obj.IsStockpiling = IsStockpiling;
			obj.FilledRatio = (float)FilledRatio;
			obj.AutoRefill = m_autoRefill;
			return obj;
		}

		public void RefillBottles()
		{
			List<MyPhysicalInventoryItem> items = this.GetInventory().GetItems();
			bool flag = false;
			double num = FilledRatio;
			foreach (MyPhysicalInventoryItem item in items)
			{
				if (num <= 0.0)
				{
					break;
				}
				MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
				if (myObjectBuilder_GasContainerObject != null && !(myObjectBuilder_GasContainerObject.GasLevel >= 1f))
				{
					MyOxygenContainerDefinition myOxygenContainerDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(myObjectBuilder_GasContainerObject) as MyOxygenContainerDefinition;
					float num2 = myObjectBuilder_GasContainerObject.GasLevel * myOxygenContainerDefinition.Capacity;
					float val = (float)(num * (double)Capacity);
					float num3 = Math.Min(myOxygenContainerDefinition.Capacity - num2, val);
					myObjectBuilder_GasContainerObject.GasLevel = Math.Min((num2 + num3) / myOxygenContainerDefinition.Capacity, 1f);
					num = Math.Max(num - (double)(num3 / Capacity), 0.0);
					flag = true;
				}
			}
			if (flag)
			{
<<<<<<< HEAD
				m_syncNeeded = true;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				ChangeFilledRatio(num, Sync.IsServer);
			}
		}

		private static void OnRefillButtonPressed(MyGasTank tank)
		{
			if (tank.IsWorking)
			{
				tank.SendRefillRequest();
			}
		}

		void Sandbox.ModAPI.Ingame.IMyGasTank.RefillBottles()
		{
			if (base.IsWorking)
			{
				SendRefillRequest();
			}
		}

		private bool CanRefill()
		{
			if (!CanStore || !base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) || FilledRatio == 0.0)
			{
				return false;
			}
			foreach (MyPhysicalInventoryItem item in this.GetInventory().GetItems())
			{
				MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
				if (myObjectBuilder_GasContainerObject != null && myObjectBuilder_GasContainerObject.GasLevel < 1f)
				{
					return true;
				}
			}
			return false;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (Sync.IsServer && !MySession.Static.SimplifiedSimulation)
<<<<<<< HEAD
			{
				double totalTransfer = GasInputPerUpdate - GasOutputPerUpdate;
				ExecuteGasTransfer(totalTransfer);
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (!base.IsFunctional && FilledRatio > 0.0)
			{
				ChangeFilledRatio(Math.Max(0.0, FilledRatio - (double)m_leakPercent), Sync.IsServer);
			}
			if (Sync.IsServer && !MySession.Static.SimplifiedSimulation)
			{
				if (GetTimerEnabledState() && (bool)m_autoRefill && CanRefill())
				{
					RefillBottles();
				}
				if (m_syncNeeded)
				{
					m_syncNeeded = false;
					this.GetInventory().OnContentsChanged();
				}
=======
			{
				double totalTransfer = GasInputPerUpdate - GasOutputPerUpdate;
				ExecuteGasTransfer(totalTransfer);
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (Sync.IsServer && !MySession.Static.SimplifiedSimulation && GetTimerEnabledState() && (bool)m_autoRefill && CanRefill())
			{
				RefillBottles();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public override bool GetTimerEnabledState()
		{
<<<<<<< HEAD
			if (Enabled && base.IsWorking)
=======
			if (base.Enabled && base.IsWorking)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return !MySession.Static.SimplifiedSimulation;
			}
			return false;
		}

		private void ExecuteGasTransfer(double totalTransfer)
		{
			if (totalTransfer != 0.0)
			{
				Transfer(totalTransfer);
				base.ResourceSink.Update();
				SourceComp.OnProductionEnabledChanged(BlockDefinition.StoredGasId);
			}
			else if (!base.HasDamageEffect)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override void DoUpdateTimerTick()
		{
			base.DoUpdateTimerTick();
			uint framesFromLastTrigger = GetFramesFromLastTrigger();
			double totalTransfer = (GasInputPerSecond - GasOutputPerSecond) * ((float)framesFromLastTrigger / 60f);
			ExecuteGasTransfer(totalTransfer);
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
			if ((!MySession.Static.Settings.EnableOxygen && BlockDefinition.StoredGasId == m_oxygenGasId) || !Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			if (!(SourceComp.CurrentOutputByType(BlockDefinition.StoredGasId) > 0f) && !(base.ResourceSink.CurrentInputByType(BlockDefinition.StoredGasId) > 0f))
			{
				return BlockDefinition.StandbyPowerConsumption;
			}
			return BlockDefinition.OperationalPowerConsumption;
		}

		private float ComputeRequiredGas()
		{
			if (!CanStore)
			{
				return 0f;
			}
			double num = (1.0 - FilledRatio) * 60.0 * (double)SourceComp.ProductionToCapacityMultiplierByType(BlockDefinition.StoredGasId);
			float num2 = SourceComp.CurrentOutputByType(BlockDefinition.StoredGasId);
			return Math.Min((float)num * Capacity + num2, 0.05f * Capacity);
		}

		private void Inventory_ContentsChanged(MyInventoryBase obj)
		{
			RaisePropertiesChanged();
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			m_prevColor = Color.White;
			m_prevFillCount = -1;
			UpdateEmissivity();
			SetDetailedInfoDirty();
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.Closed)
				{
					UpdateIsWorking();
					UpdateEmissivity();
				}
			}, "MyGasTank::PowerReceiver_IsPoweredChanged");
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			SourceComp.Enabled = CanStore;
			base.ResourceSink.Update();
			if (MySession.Static.CreativeMode)
			{
				SourceComp.SetRemainingCapacityByType(BlockDefinition.StoredGasId, Capacity);
			}
			else
			{
				SourceComp.SetRemainingCapacityByType(BlockDefinition.StoredGasId, (float)(FilledRatio * (double)Capacity));
			}
			if (base.CubeGrid != null && base.CubeGrid.GridSystems != null && base.CubeGrid.GridSystems.ResourceDistributor != null)
			{
				base.CubeGrid.GridSystems.ResourceDistributor.ConveyorSystem_OnPoweredChanged();
			}
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
			UpdateEmissivity();
		}

		private void MyOxygenTank_IsWorkingChanged(MyCubeBlock obj)
		{
			SourceComp.Enabled = CanStore;
			SetStockpilingState(m_isStockpiling);
			UpdateEmissivity();
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			SourceComp.Enabled = CanStore;
			base.ResourceSink.Update();
			UpdateEmissivity();
		}

		private void Source_OutputChanged(MyDefinitionId changedResourceId, float oldOutput, MyResourceSourceComponent source)
		{
			if (!(changedResourceId != BlockDefinition.StoredGasId) && Sync.IsServer)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		private void Sink_CurrentInputChanged(MyDefinitionId resourceTypeId, float oldInput, MyResourceSinkComponent sink)
		{
			if (!(resourceTypeId != BlockDefinition.StoredGasId) && Sync.IsServer)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public override void SetDamageEffect(bool show)
		{
			if (FilledRatio > 0.0)
			{
				base.SetDamageEffect(show);
			}
			else
			{
				if (Sandbox.Engine.Platform.Game.IsDedicated)
				{
					return;
				}
				if (MyFakes.SHOW_DAMAGE_EFFECTS && !string.IsNullOrEmpty(BlockDefinition.EmptyDamageEffectName))
				{
					if (!show && m_emptyDamageEffect != null)
					{
						m_emptyDamageEffect.Stop(instant: false);
						m_emptyDamageEffect.StopLights();
						if (base.CubeGrid.Physics != null)
						{
							m_emptyDamageEffect.Velocity = base.CubeGrid.Physics.LinearVelocity;
						}
						m_emptyDamageEffect.OnDelete -= OnEmptyDamageEffectDeleted;
						m_emptyDamageEffect = null;
					}
					if (show && m_emptyDamageEffect == null && MySandboxGame.Static.EnableDamageEffects)
					{
						string emptyDamageEffectName = BlockDefinition.EmptyDamageEffectName;
						MatrixD matrixD = MatrixD.CreateTranslation(0.85f * base.PositionComp.LocalVolume.Center);
						MatrixD effectMatrix = ((base.PositionComp != null) ? (matrixD * base.PositionComp.LocalMatrixRef) : matrixD);
						Vector3D worldPosition = base.PositionComp.WorldMatrixRef.Translation;
						if (MyParticlesManager.TryCreateParticleEffect(emptyDamageEffectName, ref effectMatrix, ref worldPosition, base.Render.ParentIDs[0], out m_emptyDamageEffect))
						{
							m_emptyDamageEffect.UserScale = base.Model.BoundingBox.Perimeter * 0.018f;
							m_emptyDamageEffect.OnDelete += OnEmptyDamageEffectDeleted;
						}
					}
				}
				if (BlockDefinition.EmptyDamagedSound != null && m_soundEmitter != null)
				{
					if (show)
					{
						m_soundEmitter.PlaySound(BlockDefinition.EmptyDamagedSound, stopPrevious: true);
					}
					else if (m_soundEmitter.SoundId == BlockDefinition.EmptyDamagedSound.SoundId)
					{
						m_soundEmitter.StopSound(forced: false);
					}
				}
			}
		}

		private void OnEmptyDamageEffectDeleted(MyParticleEffect effect)
		{
			if (effect == m_emptyDamageEffect)
			{
				SetDamageEffect(show: false);
			}
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity();
		}

		public override bool SetEmissiveStateWorking()
		{
			return false;
		}

		public override bool SetEmissiveStateDamaged()
		{
			return false;
		}

		public override bool SetEmissiveStateDisabled()
		{
			return false;
		}

		private void UpdateEmissivity()
		{
			Color color = Color.Red;
			bool flag = true;
			MyEmissiveColorStateResult result;
			if (CanStore)
			{
				if (IsStockpiling)
				{
					color = Color.Teal;
					flag = false;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Alternative, out result))
					{
						color = result.EmissiveColor;
					}
				}
				else if (FilledRatio <= 9.9999997473787516E-06)
				{
					color = Color.Yellow;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Warning, out result))
					{
						color = result.EmissiveColor;
					}
				}
				else
				{
					color = Color.Green;
					if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Working, out result))
					{
						color = result.EmissiveColor;
					}
				}
			}
			else if (base.IsFunctional)
			{
				flag = false;
				if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Disabled, out result))
				{
					color = result.EmissiveColor;
				}
			}
			else if (MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, MyCubeBlock.m_emissiveNames.Damaged, out result))
			{
				color = result.EmissiveColor;
			}
			SetEmissive(color, flag ? ((float)FilledRatio) : 1f);
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
			detailedInfo.Append("\n");
			if (!MySession.Static.Settings.EnableOxygen && !(BlockDefinition.StoredGasId != m_oxygenGasId))
			{
				detailedInfo.Append((object)MyTexts.Get(MySpaceTexts.Oxygen_Disabled));
			}
			else
			{
				detailedInfo.Append(string.Format(MyTexts.GetString(MySpaceTexts.Oxygen_Filled), (FilledRatio * 100.0).ToString("F1"), MyValueFormatter.GetFormatedFloat((int)(FilledRatio * (double)Capacity), 0, ""), MyValueFormatter.GetFormatedFloat(Capacity, 0, "")));
			}
		}

		private void SetEmissive(Color color, float fill)
		{
			int num = (int)(fill * (float)m_emissiveTextureNames.Length);
			if (base.Render.RenderObjectIDs[0] != uint.MaxValue && (!(color == m_prevColor) || num != m_prevFillCount))
			{
				for (int i = 0; i < m_emissiveTextureNames.Length; i++)
				{
					MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_emissiveTextureNames[i], (i < num) ? color : Color.Black, 1f);
				}
				m_prevColor = color;
				m_prevFillCount = num;
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			m_prevFillCount = -1;
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory(), damageContent: true);
			float num = CalculateGasExplosionDamage((float)FilledRatio * Capacity);
			if (num > 0f)
			{
				double radius = CalculateGasExplosionRadius(FilledRatio * (double)Capacity);
				BoundingSphereD explosionSphere = new BoundingSphereD(base.PositionComp.GetPosition(), radius);
				MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
				myExplosionInfo.PlayerDamage = 0f;
				myExplosionInfo.Damage = num;
				myExplosionInfo.ExplosionType = MyExplosionTypeEnum.CUSTOM;
				myExplosionInfo.ExplosionSphere = explosionSphere;
				myExplosionInfo.LifespanMiliseconds = 700;
				myExplosionInfo.ParticleScale = 1f;
				myExplosionInfo.OwnerEntity = null;
				myExplosionInfo.ExplosionFlags = MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION | MyExplosionFlags.CREATE_PARTICLE_DEBRIS;
				myExplosionInfo.VoxelCutoutScale = 1f;
				myExplosionInfo.PlaySound = true;
				myExplosionInfo.ApplyForceAndDamage = true;
				myExplosionInfo.KeepAffectedBlocks = true;
				myExplosionInfo.CustomEffect = BlockDefinition.GasExplosionEffect;
				myExplosionInfo.CustomSound = new MySoundPair(BlockDefinition.GasExplosionSound);
				MyExplosionInfo explosionInfo = myExplosionInfo;
				explosionInfo.StrengthImpulse = 0.7f;
				if (base.Physics != null)
				{
					explosionInfo.Velocity = base.Physics.LinearVelocity;
				}
				MyExplosions.AddExplosion(ref explosionInfo);
			}
			base.OnDestroy();
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
			MyInventory myInventory = inventory as MyInventory;
			if (myInventory != null)
			{
<<<<<<< HEAD
				myInventory.ContentsChanged += Inventory_ContentsChanged;
=======
				myInventory.ContentsChanged += m_inventory_ContentsChanged;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
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
			return CanStore;
		}

		private void SetStockpilingState(bool newState)
		{
			m_isStockpiling = newState;
			SourceComp.SetProductionEnabledByType(BlockDefinition.StoredGasId, !m_isStockpiling && CanStore);
			base.ResourceSink.Update();
			RaisePropertiesChanged();
		}

		internal void Transfer(double transferAmount)
		{
			if (transferAmount > 0.0)
			{
				Fill(transferAmount);
			}
			else if (transferAmount < 0.0)
			{
				Drain(0.0 - transferAmount);
			}
		}

		internal void Fill(double amount)
		{
			if (amount != 0.0)
			{
				ChangeFilledRatio(Math.Min(1.0, FilledRatio + amount / (double)Capacity), Sync.IsServer);
			}
		}

		internal void Drain(double amount)
		{
			if (amount != 0.0)
			{
				ChangeFilledRatio(Math.Max(0.0, FilledRatio - amount / (double)Capacity), Sync.IsServer);
			}
		}

		internal bool ChangeFilledRatio(double newFilledRatio, bool updateSync = false)
		{
			double filledRatio = FilledRatio;
			if (filledRatio != newFilledRatio || MySession.Static.CreativeMode)
			{
<<<<<<< HEAD
				double num = ((Capacity == 0f) ? 1f : (Capacity / 10000f));
				double num2 = Math.Round(newFilledRatio * (double)Capacity / num, 0);
				if (updateSync && m_oldInLitres != num2)
				{
					m_oldInLitres = Math.Round(newFilledRatio * (double)Capacity / num, 0);
=======
				double num = Math.Round(filledRatio * (double)Capacity, 1);
				double num2 = Math.Round(newFilledRatio * (double)Capacity, 1);
				if (updateSync && num != num2)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					ChangeFillRatioAmount(newFilledRatio);
					return false;
				}
				FilledRatio = newFilledRatio;
				if (MySession.Static.CreativeMode && newFilledRatio > filledRatio)
				{
					SourceComp.SetRemainingCapacityByType(BlockDefinition.StoredGasId, Capacity);
				}
				else
				{
					SourceComp.SetRemainingCapacityByType(BlockDefinition.StoredGasId, (float)(FilledRatio * (double)Capacity));
				}
				base.ResourceSink.Update();
				UpdateEmissivity();
				SetDetailedInfoDirty();
				RaisePropertiesChanged();
<<<<<<< HEAD
				UpdateDamageEffect();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return true;
			}
			return false;
		}

		public double GetOxygenLevel()
		{
			return FilledRatio;
		}

		public bool IsResourceStorage(MyDefinitionId resourceDefinition)
		{
			return Enumerable.Any<MyDefinitionId>((IEnumerable<MyDefinitionId>)SourceComp.ResourceTypes, (Func<MyDefinitionId, bool>)((MyDefinitionId x) => x == resourceDefinition));
		}

		private double CalculateGasExplosionRadius(double volume)
		{
			double num = (double)(0f - BlockDefinition.GasExplosionMaxRadius) * ((double)(BlockDefinition.GasExplosionNeededVolumeToReachMaxRadius / 40f) - volume) / ((double)(BlockDefinition.GasExplosionNeededVolumeToReachMaxRadius / 40f) + volume);
			if (num <= 0.0)
			{
				num = 1.0;
			}
			return num;
		}

		private float CalculateGasExplosionDamage(float volume)
		{
			return volume * BlockDefinition.GasExplosionDamageMultiplier;
		}

		private void UpdateDamageEffect()
		{
			if (base.IsFunctional)
			{
				m_isEmptyDamageEffectStarted = false;
			}
			else if (FilledRatio <= 0.0 && !m_isEmptyDamageEffectStarted)
			{
				m_isEmptyDamageEffectStarted = true;
				StopDamageEffect();
				SetDamageEffect(show: true);
			}
		}

		public void ChangeStockpileMode(bool newStockpileMode)
		{
			MyMultiplayer.RaiseEvent(this, (MyGasTank x) => x.OnStockipleModeCallback, newStockpileMode);
			UpdateEmissivity();
		}

<<<<<<< HEAD
		[Event(null, 953)]
=======
		[Event(null, 843)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnStockipleModeCallback(bool newStockpileMode)
		{
			IsStockpiling = newStockpileMode;
		}

		private void OnAutoRefillChanged()
		{
			if (Sync.IsServer && (bool)m_autoRefill && CanRefill())
			{
				RefillBottles();
			}
		}

		public void ChangeFillRatioAmount(double newFilledRatio)
		{
			MyMultiplayer.RaiseEvent(this, (MyGasTank x) => x.OnFilledRatioCallback, newFilledRatio);
		}

<<<<<<< HEAD
		[Event(null, 972)]
=======
		[Event(null, 862)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnFilledRatioCallback(double newFilledRatio)
		{
			ChangeFilledRatio(newFilledRatio);
		}

		public void SendRefillRequest()
		{
			MyMultiplayer.RaiseEvent(this, (MyGasTank x) => x.OnRefillCallback);
		}

<<<<<<< HEAD
		[Event(null, 983)]
=======
		[Event(null, 873)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnRefillCallback()
		{
			RefillBottles();
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

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}
	}
}
