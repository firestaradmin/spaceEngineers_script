using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.ObjectBuilders.ObjectBuilders;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_TargetDummyBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyTargetDummyBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyTargetDummyBlock)
	})]
	public class MyTargetDummyBlock : MyFunctionalBlock, Sandbox.ModAPI.IMyTargetDummyBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, IMyInventoryOwner, IMyConveyorEndpointBlock
	{
		private class MySubpartState
		{
			public string Name;

			public bool IsCritical;

			public MyEntitySubpart Subpart;

			public MyTargetDummyBlock Block;

			private float m_integrityCurrent;

			public float IntegrityMax = 1f;

			public float IntegrityCurrent
			{
				get
				{
					return m_integrityCurrent;
				}
				set
				{
					float num = MathHelper.Clamp(value, 0f, IntegrityMax);
					if (num == m_integrityCurrent)
					{
						return;
					}
					float integrityCurrent = m_integrityCurrent;
					m_integrityCurrent = num;
					if (!Sync.IsServer)
					{
						return;
					}
					if (integrityCurrent > 0f && num <= 0f)
					{
						if (IsCritical)
						{
							Block.SubpartDestroyed(Subpart);
							Block.DestroyAllSubparts();
						}
						else
						{
							Block.SubpartDestroyed(Subpart);
						}
						if (Block.CanSyncState)
						{
							Block.SynchronizeSubparts();
						}
					}
					else if (integrityCurrent <= 0f && num > 0f)
					{
						Block.SubpartRestored(Subpart);
						if (Block.CanSyncState)
						{
							Block.SynchronizeSubparts();
						}
					}
				}
			}

			public void Damage(float damage)
			{
				IntegrityCurrent -= damage;
			}
		}

		protected sealed class SendToolbarItemChanged_003C_003ESandbox_Game_Entities_Blocks_ToolbarItem_0023System_Int32 : ICallSite<MyTargetDummyBlock, ToolbarItem, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTargetDummyBlock @this, in ToolbarItem sentItem, in int index, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SendToolbarItemChanged(sentItem, index);
			}
		}

		protected sealed class SendStates_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Single_003E : ICallSite<MyTargetDummyBlock, List<float>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTargetDummyBlock @this, in List<float> subpartHealths, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SendStates(subpartHealths);
			}
		}

		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = (useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyTargetDummyBlock)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		protected class m_restorationDelay_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType restorationDelay;
				ISyncType result = (restorationDelay = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyTargetDummyBlock)P_0).m_restorationDelay = (Sync<float, SyncDirection.BothWays>)restorationDelay;
				return result;
			}
		}

		protected class m_enableRestoration_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType enableRestoration;
				ISyncType result = (enableRestoration = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyTargetDummyBlock)P_0).m_enableRestoration = (Sync<bool, SyncDirection.BothWays>)enableRestoration;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyTargetDummyBlock_003C_003EActor : IActivator, IActivator<MyTargetDummyBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyTargetDummyBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTargetDummyBlock CreateInstance()
			{
				return new MyTargetDummyBlock();
			}

			MyTargetDummyBlock IActivator<MyTargetDummyBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly int TOOLBAR_ITEM_COUNT = 2;

		private static readonly float PULL_AMOUNT_CONSTANT = 4f;

		protected readonly Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		private readonly Sync<float, SyncDirection.BothWays> m_restorationDelay;

		private readonly Sync<bool, SyncDirection.BothWays> m_enableRestoration;

		private bool m_canSyncState = true;

		private List<MyEntitySubpart> m_subparts = new List<MyEntitySubpart>();

		private List<string> m_subpartNames = new List<string>();

		private Dictionary<long, MySubpartState> m_subpartStates = new Dictionary<long, MySubpartState>();

		private MyTimeSpan m_restorationTime;

		private bool m_areSubpartsInitialized;

		private bool m_isDestroyed = true;

		private MyMultilineConveyorEndpoint m_endpoint;

		public bool? LoadIsDestroyed;

		public int LoadTimeToRestore;

		private List<MyObjectBuilder_TargetDummyBlock.MySubpartSavedState> LoadSubpartState;

		private List<ToolbarItem> m_items;

		private static List<MyToolbar> m_openedToolbars = new List<MyToolbar>();

		private static bool m_shouldSetOtherToolbars;

		private bool m_syncing;

		private float m_currentFillThreshold;

		public bool CanSyncState => m_canSyncState;

		public float MinRestorationDelay => Definition.MinRegenerationTimeInS;

		public float MaxRestorationDelay => Definition.MaxRegenerationTimeInS;

		public MyTargetDummyBlockDefinition Definition => base.BlockDefinition as MyTargetDummyBlockDefinition;

		public MyToolbar Toolbar { get; set; }

		public float RestorationDelay
		{
			get
			{
				return m_restorationDelay;
			}
			protected set
			{
				if ((float)m_restorationDelay != value)
				{
					m_restorationDelay.Value = value;
				}
			}
		}

		public bool EnableRestoration
		{
			get
			{
				return m_enableRestoration;
			}
			set
			{
				if ((bool)m_enableRestoration != value)
				{
					m_enableRestoration.Value = value;
				}
			}
		}

		private bool IsDestroyed
		{
			get
			{
				return m_isDestroyed;
			}
			set
			{
				if (m_isDestroyed == value)
				{
					return;
				}
				m_isDestroyed = value;
				if (m_isDestroyed)
				{
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
					m_restorationTime = MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds) + MyTimeSpan.FromSeconds((float)m_restorationDelay);
					ActivateActionDestruction();
					return;
				}
				MyInventory myInventory = GetInventoryBase(0) as MyInventory;
				if (myInventory == null || m_currentFillThreshold < myInventory.VolumeFillFactor)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
				}
			}
		}

		public IMyConveyorEndpoint ConveyorEndpoint => m_endpoint;

		bool IMyInventoryOwner.UseConveyorSystem
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

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyTargetDummyBlockDefinition definition = Definition;
			m_endpoint = new MyMultilineConveyorEndpoint(this);
			m_useConveyorSystem.SetLocalValue(newValue: true);
			m_items = new List<ToolbarItem>(TOOLBAR_ITEM_COUNT);
			for (int i = 0; i < TOOLBAR_ITEM_COUNT; i++)
			{
				m_items.Add(new ToolbarItem
				{
					EntityID = 0L
				});
			}
			Toolbar = new MyToolbar(MyToolbarType.ButtonPanel, TOOLBAR_ITEM_COUNT, 1);
			Toolbar.DrawNumbers = false;
			MyObjectBuilder_TargetDummyBlock myObjectBuilder_TargetDummyBlock;
			if ((myObjectBuilder_TargetDummyBlock = objectBuilder as MyObjectBuilder_TargetDummyBlock) != null)
			{
				m_useConveyorSystem.SetLocalValue(myObjectBuilder_TargetDummyBlock.UseConveyorSystem);
<<<<<<< HEAD
				m_restorationDelay.ValidateRange(MinRestorationDelay, MaxRestorationDelay);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_restorationDelay.SetLocalValue(myObjectBuilder_TargetDummyBlock.RestorationDelay);
				m_enableRestoration.SetLocalValue(myObjectBuilder_TargetDummyBlock.EnableRestoration);
				if (myObjectBuilder_TargetDummyBlock.IsDestroyed.HasValue)
				{
					LoadIsDestroyed = myObjectBuilder_TargetDummyBlock.IsDestroyed;
					LoadTimeToRestore = myObjectBuilder_TargetDummyBlock.TimeToRestore;
					LoadSubpartState = myObjectBuilder_TargetDummyBlock.SubpartState;
				}
				Toolbar.Init(myObjectBuilder_TargetDummyBlock.Toolbar, this);
				for (int j = 0; j < 2; j++)
				{
					MyToolbarItem itemAtIndex = Toolbar.GetItemAtIndex(j);
					if (itemAtIndex != null)
					{
						m_items.RemoveAt(j);
						m_items.Insert(j, ToolbarItem.FromItem(itemAtIndex));
					}
				}
			}
			MyDefinitionManager.Static.GetCubeSize(definition.CubeSize);
			float inventoryMaxVolume = definition.InventoryMaxVolume;
			Vector3 inventorySize = definition.InventorySize;
			MyInventory myInventory = this.GetInventory();
			if (myInventory == null)
			{
				myInventory = new MyInventory(inventoryMaxVolume, inventorySize, MyInventoryFlags.CanSend);
				base.Components.Add((MyInventoryBase)myInventory);
				myInventory.Constraint = definition.InventoryConstraint;
			}
			myInventory.ContentsChanged += ContentChangedCallback;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			m_enableRestoration.ValueChanged += EnableRestorationChangedCallback;
			m_restorationDelay.ValueChanged += RestorationDelayChangedCallback;
			Toolbar.ItemChanged += Toolbar_ItemChanged;
			CreateTerminalControls();
			if (LoadSubpartState == null)
			{
				LoadSubpartState = new List<MyObjectBuilder_TargetDummyBlock.MySubpartSavedState>();
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (base.CubeGrid.Physics == null)
			{
				return;
			}
			Vector3 linearVelocity = base.CubeGrid.Physics.LinearVelocity;
			Vector3 angularVelocity = base.CubeGrid.Physics.AngularVelocity;
			Vector3D centerOfMassWorld = base.CubeGrid.Physics.CenterOfMassWorld;
			foreach (MyEntitySubpart subpart in m_subparts)
			{
				if (subpart.Physics != null && subpart.Physics.Enabled)
				{
					Vector3 vector = Vector3.Cross(base.CubeGrid.Physics.AngularVelocity, subpart.Physics.CenterOfMassWorld - centerOfMassWorld);
					Vector3 vector2 = linearVelocity + vector;
					if (subpart.Physics.LinearVelocity != vector2)
					{
						subpart.Physics.LinearVelocity = vector2;
					}
					if (subpart.Physics.AngularVelocity != angularVelocity)
					{
						subpart.Physics.AngularVelocity = angularVelocity;
					}
				}
			}
		}

		protected override void CreateTerminalControls()
		{
			if (MyTerminalControlFactory.AreControlsCreated<MyTargetDummyBlock>())
			{
				return;
			}
			base.CreateTerminalControls();
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyTargetDummyBlock>("Open Toolbar", MySpaceTexts.BlockPropertyTitle_SensorToolbarOpen, MySpaceTexts.BlockPropertyDescription_SensorToolbarOpen, delegate(MyTargetDummyBlock self)
			{
				m_openedToolbars.Add(self.Toolbar);
				if (MyGuiScreenToolbarConfigBase.Static == null)
				{
					m_shouldSetOtherToolbars = true;
					MyToolbarComponent.CurrentToolbar = self.Toolbar;
					MyGuiScreenBase myGuiScreenBase = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, self, null);
					MyToolbarComponent.AutoUpdate = false;
					myGuiScreenBase.Closed += delegate
					{
						MyToolbarComponent.AutoUpdate = true;
						m_openedToolbars.Clear();
					};
					MyGuiSandbox.AddScreen(myGuiScreenBase);
				}
			}));
			MyTerminalControlSlider<MyTargetDummyBlock> myTerminalControlSlider = new MyTerminalControlSlider<MyTargetDummyBlock>("Delay", MySpaceTexts.BlockPropertyTitle_RegenerationDelay, MySpaceTexts.BlockPropertyDescription_RegenerationDelay);
			myTerminalControlSlider.SetLimits((MyTargetDummyBlock block) => MinRestorationDelay, (MyTargetDummyBlock block) => MaxRestorationDelay);
			myTerminalControlSlider.DefaultValue = 5f;
			myTerminalControlSlider.Getter = (MyTargetDummyBlock x) => x.RestorationDelay;
			myTerminalControlSlider.Setter = delegate(MyTargetDummyBlock x, float v)
			{
				x.RestorationDelay = v;
			};
			myTerminalControlSlider.Writer = delegate(MyTargetDummyBlock x, StringBuilder result)
			{
				result.AppendFormatedDecimal("", x.RestorationDelay, 0, " s");
			};
			myTerminalControlSlider.EnableActions();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			MyTerminalControlFactory.AddControl(new MyTerminalControlOnOffSwitch<MyTargetDummyBlock>("Enable Restoration", MySpaceTexts.BlockPropertyTitle_EnableRegeneration, MySpaceTexts.BlockPropertyDescription_EnableRegeneration)
			{
				Getter = (MyTargetDummyBlock x) => x.EnableRestoration,
				Setter = delegate(MyTargetDummyBlock x, bool v)
				{
					x.EnableRestoration = v;
				}
			});
		}

		private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index, bool isGamepad)
		{
			if (m_syncing)
			{
				return;
			}
			ToolbarItem toolbarItem = ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex));
			ToolbarItem toolbarItem2 = m_items[index.ItemIndex];
			if ((toolbarItem.EntityID == 0L && toolbarItem2.EntityID == 0L) || (toolbarItem.EntityID != 0L && toolbarItem2.EntityID != 0L && toolbarItem.Equals(toolbarItem2)))
			{
				return;
			}
			m_items.RemoveAt(index.ItemIndex);
			m_items.Insert(index.ItemIndex, toolbarItem);
			MyMultiplayer.RaiseEvent(this, (MyTargetDummyBlock x) => x.SendToolbarItemChanged, toolbarItem, index.ItemIndex);
			if (!m_shouldSetOtherToolbars)
			{
				return;
			}
			m_shouldSetOtherToolbars = false;
			foreach (MyToolbar openedToolbar in m_openedToolbars)
			{
				if (openedToolbar != self)
				{
					openedToolbar.SetItemAtIndex(index.ItemIndex, self.GetItemAtIndex(index.ItemIndex));
				}
			}
			m_shouldSetOtherToolbars = true;
		}

<<<<<<< HEAD
		[Event(null, 389)]
=======
		[Event(null, 386)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void SendToolbarItemChanged(ToolbarItem sentItem, int index)
		{
			m_syncing = true;
			MyToolbarItem item = null;
			if (sentItem.EntityID != 0L)
			{
				item = ToolbarItem.ToItem(sentItem);
			}
			Toolbar.SetItemAtIndex(index, item);
			m_syncing = false;
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			InitializationUpdate();
		}

		private void InitializationUpdate()
		{
			if (base.IsWorking)
			{
				InitializeSubparts();
			}
			else
			{
				UninitializeSubparts();
			}
		}

		private void ActivateActionDestruction()
		{
			if (base.IsFunctional)
			{
				Toolbar.UpdateItem(1);
				if (Sync.IsServer)
				{
					Toolbar.ActivateItemAtSlot(1, checkIfWantsToBeActivated: false, playActivationSound: false);
				}
			}
		}

		private void ActivateActionHit()
		{
			if (base.IsFunctional)
			{
				Toolbar.UpdateItem(0);
				if (Sync.IsServer)
				{
					Toolbar.ActivateItemAtSlot(0, checkIfWantsToBeActivated: false, playActivationSound: false);
				}
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_TargetDummyBlock myObjectBuilder_TargetDummyBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_TargetDummyBlock;
			myObjectBuilder_TargetDummyBlock.RestorationDelay = m_restorationDelay.Value;
			myObjectBuilder_TargetDummyBlock.EnableRestoration = m_enableRestoration.Value;
			myObjectBuilder_TargetDummyBlock.UseConveyorSystem = m_useConveyorSystem.Value;
			myObjectBuilder_TargetDummyBlock.SubpartState = LoadSubpartState;
			if (m_areSubpartsInitialized)
			{
				if (IsDestroyed)
				{
					myObjectBuilder_TargetDummyBlock.IsDestroyed = true;
					myObjectBuilder_TargetDummyBlock.TimeToRestore = (int)(m_restorationTime - MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds)).Seconds;
				}
				else
				{
					myObjectBuilder_TargetDummyBlock.IsDestroyed = false;
					myObjectBuilder_TargetDummyBlock.SubpartState = new List<MyObjectBuilder_TargetDummyBlock.MySubpartSavedState>();
					foreach (KeyValuePair<long, MySubpartState> subpartState in m_subpartStates)
					{
						myObjectBuilder_TargetDummyBlock.SubpartState.Add(new MyObjectBuilder_TargetDummyBlock.MySubpartSavedState
						{
							SubpartName = subpartState.Value.Name,
							SubpartHealth = subpartState.Value.IntegrityCurrent
						});
					}
				}
			}
			myObjectBuilder_TargetDummyBlock.Toolbar = Toolbar.GetObjectBuilder();
			return myObjectBuilder_TargetDummyBlock;
		}

		private void EnableRestorationChangedCallback(SyncBase obj)
		{
			if ((bool)m_enableRestoration && m_isDestroyed)
			{
				m_restorationTime = MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds) + MyTimeSpan.FromSeconds((float)m_restorationDelay);
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
		}

		private void RestorationDelayChangedCallback(SyncBase obj)
		{
			if (m_isDestroyed)
			{
				m_restorationTime = MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds) + MyTimeSpan.FromSeconds((float)m_restorationDelay);
			}
		}

		private void ContentChangedCallback(MyInventoryBase obj)
		{
			if (m_isDestroyed)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (!base.IsWorking)
			{
				return;
			}
			InitializeSubparts();
			if (!LoadIsDestroyed.HasValue)
			{
				return;
			}
			if (LoadIsDestroyed.Value)
			{
				m_isDestroyed = true;
				m_restorationTime = MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds) + MyTimeSpan.FromSeconds(LoadTimeToRestore);
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
				foreach (MyEntitySubpart subpart in m_subparts)
				{
					HideSubpart(subpart);
				}
			}
			else
			{
				RestoreAllSubparts();
				foreach (MyObjectBuilder_TargetDummyBlock.MySubpartSavedState item in LoadSubpartState)
				{
					foreach (KeyValuePair<long, MySubpartState> subpartState in m_subpartStates)
					{
						if (subpartState.Value.Name == item.SubpartName)
						{
							subpartState.Value.IntegrityCurrent = item.SubpartHealth;
							break;
						}
					}
				}
				if (!Sync.IsServer)
				{
					ResetAllSubparts();
				}
			}
			LoadIsDestroyed = null;
			LoadTimeToRestore = 0;
			LoadSubpartState = null;
		}

		public override void RefreshModels(string modelPath, string modelCollisionPath)
		{
			base.RefreshModels(modelPath, modelCollisionPath);
			if (m_areSubpartsInitialized)
			{
				ReinitializeSubpartsOnModelChange();
			}
		}

		private void ReinitializeSubpartsOnModelChange()
		{
			Dictionary<string, float> dictionary = new Dictionary<string, float>();
			foreach (KeyValuePair<long, MySubpartState> subpartState in m_subpartStates)
			{
				dictionary.Add(subpartState.Value.Name, subpartState.Value.IntegrityCurrent);
			}
			bool isDestroyed = m_isDestroyed;
			MyTimeSpan restorationTime = m_restorationTime;
			UninitializeSubparts();
			InitializeSubparts();
			DisableStateSyncing();
			foreach (KeyValuePair<long, MySubpartState> subpartState2 in m_subpartStates)
			{
				if (dictionary.TryGetValue(subpartState2.Value.Name, out var value))
				{
					subpartState2.Value.IntegrityCurrent = value;
				}
			}
			EnableStateSyncing();
			m_isDestroyed = isDestroyed;
			m_restorationTime = restorationTime;
			ResetAllSubparts();
		}

		private void InitializeSubparts()
		{
			if (m_areSubpartsInitialized)
			{
				return;
			}
			m_areSubpartsInitialized = true;
			GatherSubparts(this);
			BuildSubpartStates();
			foreach (MyEntitySubpart subpart in m_subparts)
			{
				InitializeSubpart(subpart);
				if (subpart.Physics != null && !subpart.Physics.Enabled)
				{
					subpart.Physics.Activate();
				}
			}
			m_isDestroyed = true;
			bool flag = true;
			if (MySession.Static.CreativeMode && m_restorationTime <= MyTimeSpan.Zero)
			{
				m_restorationTime = MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds);
				flag = false;
			}
			else
			{
				m_restorationTime = MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds) + MyTimeSpan.FromSeconds((float)m_restorationDelay);
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			if (!flag)
			{
				return;
			}
			foreach (MyEntitySubpart subpart2 in m_subparts)
			{
				HideSubpart(subpart2);
			}
		}

		private void UninitializeSubparts()
		{
			if (m_areSubpartsInitialized)
			{
				m_areSubpartsInitialized = false;
				m_subparts.Clear();
				m_subpartNames.Clear();
				m_subpartStates.Clear();
			}
		}

		private void GatherSubparts(MyEntity ent)
		{
			Dictionary<string, MyEntitySubpart>.Enumerator enumerator = ent.Subparts.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, MyEntitySubpart> current = enumerator.Current;
				m_subparts.Add(current.Value);
				m_subpartNames.Add(current.Key);
				GatherSubparts(current.Value);
			}
		}

		private void BuildSubpartStates()
		{
			MyTargetDummyBlockDefinition definition = Definition;
			if (definition == null)
			{
				return;
			}
			for (int i = 0; i < m_subparts.Count; i++)
			{
				MyEntitySubpart myEntitySubpart = m_subparts[i];
				string text = m_subpartNames[i];
				float integrityMax = 1f;
				bool isCritical = false;
				if (definition.SubpartDefinitions.ContainsKey(text))
				{
					MyTargetDummyBlockDefinition.MyDummySubpartDescription myDummySubpartDescription = definition.SubpartDefinitions[text];
					integrityMax = myDummySubpartDescription.Health;
					isCritical = myDummySubpartDescription.IsCritical;
				}
				MySubpartState value = new MySubpartState
				{
					Subpart = myEntitySubpart,
					Block = this,
					Name = text,
					IsCritical = isCritical,
					IntegrityCurrent = 0f,
					IntegrityMax = integrityMax
				};
				m_subpartStates.Add(myEntitySubpart.EntityId, value);
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			CheckInventoryFill();
			if (!m_isDestroyed)
			{
				return;
			}
			if (!m_enableRestoration)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
			else
			{
				if (!(m_restorationTime < MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds)) || !base.IsFunctional)
				{
					return;
				}
				if (MySession.Static.CreativeMode)
				{
					RestoreAllSubparts();
					return;
				}
				MyTargetDummyBlockDefinition definition = Definition;
				MyInventoryBase inventoryBase = GetInventoryBase(0);
				if (inventoryBase.GetItemAmount(definition.ConstructionItem).ToIntSafe() >= definition.ConstructionItemAmount)
				{
					MyFixedPoint amount = definition.ConstructionItemAmount;
					inventoryBase.RemoveItemsOfType(amount, definition.ConstructionItem);
					RestoreAllSubparts();
				}
			}
		}

		private void CheckInventoryFill()
		{
			MyInventory myInventory = GetInventoryBase(0) as MyInventory;
			MyTargetDummyBlockDefinition definition = Definition;
			if (myInventory.VolumeFillFactor <= definition.MinFillFactor)
			{
				m_currentFillThreshold = definition.MaxFillFactor;
			}
			if (m_currentFillThreshold > myInventory.VolumeFillFactor)
			{
				base.CubeGrid.GridSystems.ConveyorSystem.PullItem(definition.ConstructionItem, (MyFixedPoint)(PULL_AMOUNT_CONSTANT * (float)definition.ConstructionItemAmount), this, myInventory, remove: false, calcImmediately: false);
			}
			else if (!m_isDestroyed)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
			if (myInventory.VolumeFillFactor >= definition.MaxFillFactor)
			{
				m_currentFillThreshold = definition.MinFillFactor;
			}
		}

		private void InitializeSubpart(MyEntity subpart)
		{
			bool flag = !Sync.IsServer;
			if (subpart.Physics == null && subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0)
			{
				HkShape shape = subpart.ModelCollision.HavokCollisionShapes[0];
				MyPhysicsBody myPhysicsBody = (MyPhysicsBody)(subpart.Physics = new MyPhysicsBody(subpart, RigidBodyFlag.RBF_KINEMATIC | RigidBodyFlag.RBF_UNLOCKED_SPEEDS));
				Vector3 zero = Vector3.Zero;
				HkMassProperties value = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(subpart.PositionComp.LocalAABB.HalfExtents, 0f);
				subpart.Physics.IsPhantom = false;
				value.Volume = subpart.PositionComp.LocalAABB.Volume();
				subpart.GetPhysicsBody().CreateFromCollisionObject(shape, zero, subpart.WorldMatrix, value, 6);
				((MyPhysicsBody)subpart.Physics).IsSubpart = true;
			}
			if (subpart.Physics != null)
			{
				if (!flag)
				{
					subpart.Physics.Enabled = true;
				}
				else
				{
					subpart.Physics.Enabled = true;
				}
			}
		}

<<<<<<< HEAD
		public override bool ReceivedDamage(float damage, MyStringHash damageType, long attackerId, long realHitEntityId, bool shouldDetonateAmmo = true)
=======
		public override bool ReceivedDamage(float damage, MyStringHash damageType, long attackerId, long realHitEntityId)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			bool result = base.ReceivedDamage(damage, damageType, attackerId, realHitEntityId);
			if (m_subpartStates.ContainsKey(realHitEntityId))
			{
				if (Sync.IsServer)
				{
					ActivateActionHit();
					m_subpartStates[realHitEntityId].Damage(damage);
				}
				result = false;
			}
			return result;
		}

		private void SubpartDestroyed(MyEntitySubpart subpart)
		{
			HideSubpart(subpart);
			if (IsDestroyed)
			{
				return;
			}
			bool flag = false;
			foreach (KeyValuePair<long, MySubpartState> subpartState in m_subpartStates)
			{
				if (subpartState.Value.IntegrityCurrent > 0f)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				IsDestroyed = true;
			}
		}

		private void SubpartRestored(MyEntitySubpart subpart)
		{
			bool visible = subpart.Render.Visible;
			subpart.Render.Visible = true;
			if (subpart.Physics != null)
			{
				subpart.Physics.Enabled = true;
			}
<<<<<<< HEAD
			if (visible)
			{
				return;
			}
			MyTargetDummyBlockDefinition definition = Definition;
			MatrixD effectMatrix = subpart.PositionComp.LocalMatrixRef;
			Vector3D worldPosition = subpart.PositionComp.GetPosition();
			if (!base.CubeGrid.IsPreview)
			{
=======
			if (!visible)
			{
				MyTargetDummyBlockDefinition definition = Definition;
				MatrixD effectMatrix = subpart.PositionComp.LocalMatrixRef;
				Vector3D worldPosition = subpart.PositionComp.GetPosition();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyParticlesManager.TryCreateParticleEffect(definition.RegenerationEffectName, ref effectMatrix, ref worldPosition, base.Render.GetRenderObjectID(), out var effect);
				if (effect != null)
				{
					effect.UserBirthMultiplier = definition.RegenerationEffectMultiplier;
				}
				if (m_soundEmitter != null)
				{
					m_soundEmitter.PlaySound(definition.RegenerationSound, stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, true);
				}
			}
		}

		private void HideSubpart(MyEntitySubpart subpart)
		{
			bool num = !subpart.Render.Visible;
			subpart.Render.Visible = false;
			if (subpart.Physics != null)
			{
				subpart.Physics.Enabled = false;
			}
<<<<<<< HEAD
			if (num)
			{
				return;
			}
			MyTargetDummyBlockDefinition definition = Definition;
			MatrixD effectMatrix = subpart.PositionComp.LocalMatrixRef;
			Vector3D worldPosition = subpart.PositionComp.GetPosition();
			if (!base.CubeGrid.IsPreview)
			{
=======
			if (!num)
			{
				MyTargetDummyBlockDefinition definition = Definition;
				MatrixD effectMatrix = subpart.PositionComp.LocalMatrixRef;
				Vector3D worldPosition = subpart.PositionComp.GetPosition();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyParticlesManager.TryCreateParticleEffect(definition.DestructionEffectName, ref effectMatrix, ref worldPosition, base.Render.GetRenderObjectID(), out var effect);
				if (effect != null)
				{
					effect.UserBirthMultiplier = definition.DestructionEffectMultiplier;
				}
				if (m_soundEmitter != null)
				{
					m_soundEmitter.PlaySound(definition.DestructionSound, stopPrevious: false, skipIntro: false, force2D: false, alwaysHearOnRealistic: false, skipToEnd: false, true);
				}
			}
		}

		private void RestoreAllSubparts()
		{
			DisableStateSyncing();
			foreach (KeyValuePair<long, MySubpartState> subpartState in m_subpartStates)
			{
				subpartState.Value.IntegrityCurrent = subpartState.Value.IntegrityMax;
			}
			EnableStateSyncing();
			if (Sync.IsServer)
			{
				SynchronizeSubparts();
			}
			IsDestroyed = false;
		}

		private void DestroyAllSubparts()
		{
			if (IsDestroyed)
			{
				return;
			}
			IsDestroyed = true;
			DisableStateSyncing();
			foreach (KeyValuePair<long, MySubpartState> subpartState in m_subpartStates)
			{
				subpartState.Value.IntegrityCurrent = 0f;
			}
			EnableStateSyncing();
			if (Sync.IsServer)
			{
				SynchronizeSubparts();
			}
		}

		private void EnableStateSyncing()
		{
			m_canSyncState = true;
		}

		private void DisableStateSyncing()
		{
			m_canSyncState = false;
		}

		private void ResetAllSubparts()
		{
			foreach (KeyValuePair<long, MySubpartState> subpartState in m_subpartStates)
			{
				bool flag = subpartState.Value.IntegrityCurrent > 0f;
				MyEntitySubpart subpart = subpartState.Value.Subpart;
				subpart.Render.Visible = flag;
				if (subpart.Physics != null)
				{
					subpart.Physics.Enabled = flag;
				}
			}
		}

		private void SynchronizeSubparts()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			List<float> list = new List<float>();
			foreach (MyEntitySubpart subpart in m_subparts)
			{
				list.Add(m_subpartStates[subpart.EntityId].IntegrityCurrent);
			}
			MyMultiplayer.RaiseEvent(this, (MyTargetDummyBlock x) => x.SendStates, list);
		}

<<<<<<< HEAD
		[Event(null, 950)]
=======
		[Event(null, 940)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void SendStates(List<float> subpartHealths)
		{
			for (int i = 0; i < m_subparts.Count; i++)
			{
				if (subpartHealths[i] > 0f && !m_subparts[i].Render.Visible)
				{
					SubpartRestored(m_subparts[i]);
				}
				else if (subpartHealths[i] <= 0f && m_subparts[i].Render.Visible)
				{
					HideSubpart(m_subparts[i]);
				}
			}
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return this.GetInventory(index);
		}

		public void InitializeConveyorEndpoint()
		{
			m_endpoint = new MyMultilineConveyorEndpoint(this);
		}

		public bool AllowSelfPulling()
		{
			return false;
		}

		public PullInformation GetPullInformation()
		{
			return new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId,
				Constraint = new MyInventoryConstraint("Empty constraint")
			};
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		[SpecialName]
		int IMyInventoryOwner.get_InventoryCount()
		{
			return base.InventoryCount;
		}

		[SpecialName]
		bool IMyInventoryOwner.get_HasInventory()
		{
			return base.HasInventory;
		}
	}
}
