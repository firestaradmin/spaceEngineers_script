using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_ConveyorSorter))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyConveyorSorter),
		typeof(Sandbox.ModAPI.Ingame.IMyConveyorSorter)
	})]
	public class MyConveyorSorter : MyFunctionalBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyConveyorSorter, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyConveyorSorter, IMyInventoryOwner
	{
		protected sealed class DoChangeBlWl_003C_003ESystem_Boolean : ICallSite<MyConveyorSorter, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyConveyorSorter @this, in bool IsWl, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DoChangeBlWl(IsWl);
			}
		}

		protected sealed class DoChangeListId_003C_003EVRage_ObjectBuilders_SerializableDefinitionId_0023System_Boolean : ICallSite<MyConveyorSorter, SerializableDefinitionId, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyConveyorSorter @this, in SerializableDefinitionId id, in bool add, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DoChangeListId(id, add);
			}
		}

		protected sealed class DoChangeListType_003C_003ESystem_Byte_0023System_Boolean : ICallSite<MyConveyorSorter, byte, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyConveyorSorter @this, in byte type, in bool add, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DoChangeListType(type, add);
			}
		}

		protected sealed class DoSetupFilter_003C_003ESandbox_ModAPI_Ingame_MyConveyorSorterMode_0023System_Collections_Generic_List_00601_003CSandbox_ModAPI_Ingame_MyInventoryItemFilter_003E : ICallSite<MyConveyorSorter, MyConveyorSorterMode, List<MyInventoryItemFilter>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyConveyorSorter @this, in MyConveyorSorterMode mode, in List<MyInventoryItemFilter> items, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DoSetupFilter(mode, items);
			}
		}

		protected class m_drainAll_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType drainAll;
				ISyncType result = (drainAll = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyConveyorSorter)P_0).m_drainAll = (Sync<bool, SyncDirection.BothWays>)drainAll;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyConveyorSorter_003C_003EActor : IActivator, IActivator<MyConveyorSorter>
		{
			private sealed override object CreateInstance()
			{
				return new MyConveyorSorter();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyConveyorSorter CreateInstance()
			{
				return new MyConveyorSorter();
			}

			MyConveyorSorter IActivator<MyConveyorSorter>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly int MAX_ITEMS_TO_PULL_IN_ONE_TICK = 10;

		private MyStringHash m_prevColor = MyStringHash.NullOrEmpty;

		private readonly MyInventoryConstraint m_inventoryConstraint = new MyInventoryConstraint(string.Empty);

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private readonly Sync<bool, SyncDirection.BothWays> m_drainAll;

		private MyConveyorSorterDefinition m_conveyorSorterDefinition;

		private int m_pushRequestFrameCounter;

		private int m_pullRequestFrameCounter;

		private static readonly StringBuilder m_helperSB;

		private static MyTerminalControlOnOffSwitch<MyConveyorSorter> drainAll;

		private static MyTerminalControlCombobox<MyConveyorSorter> blacklistWhitelist;

		private static MyTerminalControlListbox<MyConveyorSorter> currentList;

		private static MyTerminalControlButton<MyConveyorSorter> removeFromSelectionButton;

		private static MyTerminalControlListbox<MyConveyorSorter> candidates;

		private static MyTerminalControlButton<MyConveyorSorter> addToSelectionButton;

		private static readonly Dictionary<byte, Tuple<MyObjectBuilderType, StringBuilder>> CandidateTypes;

		private static readonly Dictionary<MyObjectBuilderType, byte> CandidateTypesToId;

		private bool m_allowCurrentListUpdate = true;

		private List<MyGuiControlListbox.Item> m_selectedForDelete;

		private List<MyGuiControlListbox.Item> m_selectedForAdd;

		public bool IsWhitelist
		{
			get
			{
				return m_inventoryConstraint.IsWhitelist;
			}
			private set
			{
				if (m_inventoryConstraint.IsWhitelist != value)
				{
					m_inventoryConstraint.IsWhitelist = value;
					base.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
				}
			}
		}

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public bool DrainAll
		{
			get
			{
				return m_drainAll;
			}
			set
			{
				m_drainAll.Value = value;
			}
		}

		public new MyConveyorSorterDefinition BlockDefinition => (MyConveyorSorterDefinition)base.BlockDefinition;

		private bool UseConveyorSystem
		{
			get
			{
				return true;
			}
			set
			{
				throw new InvalidOperationException();
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
				throw new NotImplementedException();
			}
		}

		MyConveyorSorterMode Sandbox.ModAPI.Ingame.IMyConveyorSorter.Mode
		{
			get
			{
				if (!m_inventoryConstraint.IsWhitelist)
				{
					return MyConveyorSorterMode.Blacklist;
				}
				return MyConveyorSorterMode.Whitelist;
			}
		}

		public bool IsAllowed(MyDefinitionId itemId)
		{
			if (!Enabled || !base.IsFunctional || !base.IsWorking || !base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return false;
			}
			return m_inventoryConstraint.Check(itemId);
		}

		public MyConveyorSorter()
		{
			CreateTerminalControls();
			m_drainAll.ValueChanged += delegate
			{
				DoChangeDrainAll();
			};
		}

		static MyConveyorSorter()
		{
			m_helperSB = new StringBuilder();
			CandidateTypes = new Dictionary<byte, Tuple<MyObjectBuilderType, StringBuilder>>();
			CandidateTypesToId = new Dictionary<MyObjectBuilderType, byte>();
			byte b = 0;
			CandidateTypes.Add(b = (byte)(b + 1), new Tuple<MyObjectBuilderType, StringBuilder>(typeof(MyObjectBuilder_AmmoMagazine), MyTexts.Get(MySpaceTexts.DisplayName_ConvSorterTypes_Ammo)));
			CandidateTypes.Add(b = (byte)(b + 1), new Tuple<MyObjectBuilderType, StringBuilder>(typeof(MyObjectBuilder_Component), MyTexts.Get(MySpaceTexts.DisplayName_ConvSorterTypes_Component)));
			CandidateTypes.Add(b = (byte)(b + 1), new Tuple<MyObjectBuilderType, StringBuilder>(typeof(MyObjectBuilder_PhysicalGunObject), MyTexts.Get(MySpaceTexts.DisplayName_ConvSorterTypes_HandTool)));
			CandidateTypes.Add(b = (byte)(b + 1), new Tuple<MyObjectBuilderType, StringBuilder>(typeof(MyObjectBuilder_Ingot), MyTexts.Get(MySpaceTexts.DisplayName_ConvSorterTypes_Ingot)));
			CandidateTypes.Add(b = (byte)(b + 1), new Tuple<MyObjectBuilderType, StringBuilder>(typeof(MyObjectBuilder_Ore), MyTexts.Get(MySpaceTexts.DisplayName_ConvSorterTypes_Ore)));
			foreach (KeyValuePair<byte, Tuple<MyObjectBuilderType, StringBuilder>> candidateType in CandidateTypes)
			{
				CandidateTypesToId.Add(candidateType.Value.Item1, candidateType.Key);
			}
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyConveyorSorter>())
			{
				base.CreateTerminalControls();
				drainAll = new MyTerminalControlOnOffSwitch<MyConveyorSorter>("DrainAll", MySpaceTexts.Terminal_DrainAll);
				drainAll.Getter = (MyConveyorSorter block) => block.DrainAll;
				drainAll.Setter = delegate(MyConveyorSorter block, bool val)
				{
					block.DrainAll = val;
				};
				drainAll.EnableToggleAction();
				MyTerminalControlFactory.AddControl(drainAll);
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyConveyorSorter>());
				blacklistWhitelist = new MyTerminalControlCombobox<MyConveyorSorter>("blacklistWhitelist", MySpaceTexts.BlockPropertyTitle_ConveyorSorterFilterMode, MySpaceTexts.Blank);
				blacklistWhitelist.ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> block)
				{
					FillBlWlCombo(block);
				};
				blacklistWhitelist.Getter = (MyConveyorSorter block) => block.IsWhitelist ? 1 : 0;
				blacklistWhitelist.Setter = delegate(MyConveyorSorter block, long val)
				{
					block.ChangeBlWl(val == 1);
				};
				blacklistWhitelist.SetSerializerBit();
				blacklistWhitelist.SupportsMultipleBlocks = false;
				MyTerminalControlFactory.AddControl(blacklistWhitelist);
				currentList = new MyTerminalControlListbox<MyConveyorSorter>("CurrentList", MySpaceTexts.BlockPropertyTitle_ConveyorSorterFilterItemsList, MySpaceTexts.Blank, multiSelect: true);
				currentList.ListContent = delegate(MyConveyorSorter block, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
				{
					block.FillCurrentList(list1, list2);
				};
				currentList.ItemSelected = delegate(MyConveyorSorter block, List<MyGuiControlListbox.Item> val)
				{
					block.SelectFromCurrentList(val);
				};
				currentList.SupportsMultipleBlocks = false;
				MyTerminalControlFactory.AddControl(currentList);
				removeFromSelectionButton = new MyTerminalControlButton<MyConveyorSorter>("removeFromSelectionButton", MySpaceTexts.BlockPropertyTitle_ConveyorSorterRemove, MySpaceTexts.Blank, delegate(MyConveyorSorter block)
				{
					block.RemoveFromCurrentList();
				});
				removeFromSelectionButton.Enabled = (MyConveyorSorter x) => x.m_selectedForDelete != null && x.m_selectedForDelete.Count > 0;
				removeFromSelectionButton.SupportsMultipleBlocks = false;
				MyTerminalControlFactory.AddControl(removeFromSelectionButton);
				candidates = new MyTerminalControlListbox<MyConveyorSorter>("candidatesList", MySpaceTexts.BlockPropertyTitle_ConveyorSorterCandidatesList, MySpaceTexts.Blank, multiSelect: true);
				candidates.ListContent = delegate(MyConveyorSorter block, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
				{
					block.FillCandidatesList(list1, list2);
				};
				candidates.ItemSelected = delegate(MyConveyorSorter block, List<MyGuiControlListbox.Item> val)
				{
					block.SelectCandidate(val);
				};
				candidates.SupportsMultipleBlocks = false;
				MyTerminalControlFactory.AddControl(candidates);
				addToSelectionButton = new MyTerminalControlButton<MyConveyorSorter>("addToSelectionButton", MySpaceTexts.BlockPropertyTitle_ConveyorSorterAdd, MySpaceTexts.Blank, delegate(MyConveyorSorter x)
				{
					x.AddToCurrentList();
				});
				addToSelectionButton.SupportsMultipleBlocks = false;
				addToSelectionButton.Enabled = (MyConveyorSorter x) => x.m_selectedForAdd != null && x.m_selectedForAdd.Count > 0;
				MyTerminalControlFactory.AddControl(addToSelectionButton);
			}
		}

		private static void FillBlWlCombo(List<MyTerminalControlComboBoxItem> list)
		{
			MyTerminalControlComboBoxItem item = new MyTerminalControlComboBoxItem
			{
				Key = 0L,
				Value = MySpaceTexts.BlockPropertyTitle_ConveyorSorterFilterModeBlacklist
			};
			list.Add(item);
			item = new MyTerminalControlComboBoxItem
			{
				Key = 1L,
				Value = MySpaceTexts.BlockPropertyTitle_ConveyorSorterFilterModeWhitelist
			};
			list.Add(item);
		}

		private void FillCurrentList(ICollection<MyGuiControlListbox.Item> content, ICollection<MyGuiControlListbox.Item> selectedItems)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyObjectBuilderType> enumerator = m_inventoryConstraint.ConstrainedTypes.GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (CandidateTypesToId.TryGetValue(constrainedType, out var value) && CandidateTypes.TryGetValue(value, out var value2))
				{
					MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(value2.Item2, value2.Item2.ToString(), null, value);
					content.Add(item);
=======
				while (enumerator.MoveNext())
				{
					MyObjectBuilderType current = enumerator.get_Current();
					if (CandidateTypesToId.TryGetValue(current, out var value) && CandidateTypes.TryGetValue(value, out var value2))
					{
						MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(value2.Item2, null, null, value);
						content.Add(item);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
<<<<<<< HEAD
				if (MyDefinitionManager.Static.TryGetPhysicalItemDefinition(constrainedId, out var definition))
				{
					m_helperSB.Clear().Append(definition.DisplayNameText);
				}
				else
=======
				((IDisposable)enumerator).Dispose();
			}
			Enumerator<MyDefinitionId> enumerator2 = m_inventoryConstraint.ConstrainedIds.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyDefinitionId current2 = enumerator2.get_Current();
					if (MyDefinitionManager.Static.TryGetPhysicalItemDefinition(current2, out var definition))
					{
						m_helperSB.Clear().Append(definition.DisplayNameText);
					}
					else
					{
						m_helperSB.Clear().Append(current2.ToString());
					}
					MyGuiControlListbox.Item item2 = new MyGuiControlListbox.Item(m_helperSB, null, null, current2);
					content.Add(item2);
				}
<<<<<<< HEAD
				MyGuiControlListbox.Item item2 = new MyGuiControlListbox.Item(m_helperSB, m_helperSB.ToString(), null, constrainedId);
				content.Add(item2);
=======
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void SelectFromCurrentList(List<MyGuiControlListbox.Item> val)
		{
			m_selectedForDelete = val;
			removeFromSelectionButton.UpdateVisual();
		}

		private void ModifyCurrentList(ref List<MyGuiControlListbox.Item> list, bool Add)
		{
			m_allowCurrentListUpdate = false;
			if (list != null)
			{
				foreach (MyGuiControlListbox.Item item in list)
				{
					MyDefinitionId? myDefinitionId = item.UserData as MyDefinitionId?;
					if (myDefinitionId.HasValue)
					{
						ChangeListId(myDefinitionId.Value, Add);
						continue;
					}
					byte? b = item.UserData as byte?;
					if (b.HasValue)
					{
						ChangeListType(b.Value, Add);
					}
				}
			}
			m_allowCurrentListUpdate = true;
			currentList.UpdateVisual();
			addToSelectionButton.UpdateVisual();
			removeFromSelectionButton.UpdateVisual();
		}

		private void RemoveFromCurrentList()
		{
			ModifyCurrentList(ref m_selectedForDelete, Add: false);
		}

		private void FillCandidatesList(ICollection<MyGuiControlListbox.Item> content, ICollection<MyGuiControlListbox.Item> selectedItems)
		{
			foreach (KeyValuePair<byte, Tuple<MyObjectBuilderType, StringBuilder>> candidateType in CandidateTypes)
			{
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(candidateType.Value.Item2, null, null, candidateType.Key);
				content.Add(item);
			}
			foreach (MyDefinitionBase item3 in (IEnumerable<MyDefinitionBase>)Enumerable.OrderBy<MyDefinitionBase, string>((IEnumerable<MyDefinitionBase>)MyDefinitionManager.Static.GetAllDefinitions(), (Func<MyDefinitionBase, string>)((MyDefinitionBase x) => sorter(x))))
			{
				if (item3.Public)
				{
					MyPhysicalItemDefinition myPhysicalItemDefinition = item3 as MyPhysicalItemDefinition;
					if (myPhysicalItemDefinition != null && item3.Public && myPhysicalItemDefinition.CanSpawnFromScreen)
					{
						m_helperSB.Clear().Append(item3.DisplayNameText);
						MyGuiControlListbox.Item item2 = new MyGuiControlListbox.Item(m_helperSB, m_helperSB.ToString(), null, myPhysicalItemDefinition.Id);
						content.Add(item2);
					}
				}
			}
		}

		private string sorter(MyDefinitionBase def)
		{
			return (def as MyPhysicalItemDefinition)?.DisplayNameText;
		}

		private void SelectCandidate(List<MyGuiControlListbox.Item> val)
		{
			m_selectedForAdd = val;
			addToSelectionButton.UpdateVisual();
		}

		private void AddToCurrentList()
		{
			ModifyCurrentList(ref m_selectedForAdd, Add: true);
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0f, detailedInfo);
			detailedInfo.Append("\n");
		}

		internal void DoChangeDrainAll()
		{
			DrainAll = m_drainAll;
			drainAll.UpdateVisual();
		}

		public void ChangeBlWl(bool IsWl)
		{
			MyMultiplayer.RaiseEvent(this, (MyConveyorSorter x) => x.DoChangeBlWl, IsWl);
		}

		[Event(null, 345)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void DoChangeBlWl(bool IsWl)
		{
			IsWhitelist = IsWl;
			blacklistWhitelist.UpdateVisual();
		}

		private void ChangeListId(SerializableDefinitionId id, bool wasAdded)
		{
			MyMultiplayer.RaiseEvent(this, (MyConveyorSorter x) => x.DoChangeListId, id, wasAdded);
		}

		[Event(null, 357)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void DoChangeListId(SerializableDefinitionId id, bool add)
		{
			if (add)
			{
				m_inventoryConstraint.Add(id);
			}
			else
			{
				m_inventoryConstraint.Remove(id);
			}
			base.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
			if (m_allowCurrentListUpdate)
			{
				currentList.UpdateVisual();
			}
		}

		private void ChangeListType(byte type, bool wasAdded)
		{
			MyMultiplayer.RaiseEvent(this, (MyConveyorSorter x) => x.DoChangeListType, type, wasAdded);
		}

		[Event(null, 377)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void DoChangeListType(byte type, bool add)
		{
			if (CandidateTypes.TryGetValue(type, out var value))
			{
				if (add)
				{
					m_inventoryConstraint.AddObjectBuilderType(value.Item1);
				}
				else
				{
					m_inventoryConstraint.RemoveObjectBuilderType(value.Item1);
				}
				base.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
				if (m_allowCurrentListUpdate)
				{
					currentList.UpdateVisual();
				}
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			m_conveyorSorterDefinition = (MyConveyorSorterDefinition)MyDefinitionManager.Static.GetCubeBlockDefinition(objectBuilder.GetId());
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(m_conveyorSorterDefinition.ResourceSinkGroup, BlockDefinition.PowerInput, UpdatePowerInput, this);
			myResourceSinkComponent.IsPoweredChanged += IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_ConveyorSorter myObjectBuilder_ConveyorSorter = (MyObjectBuilder_ConveyorSorter)objectBuilder;
			m_drainAll.SetLocalValue(myObjectBuilder_ConveyorSorter.DrainAll);
			IsWhitelist = myObjectBuilder_ConveyorSorter.IsWhiteList;
			Enumerator<SerializableDefinitionId> enumerator = myObjectBuilder_ConveyorSorter.DefinitionIds.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					SerializableDefinitionId current = enumerator.get_Current();
					m_inventoryConstraint.Add(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			foreach (byte definitionType in myObjectBuilder_ConveyorSorter.DefinitionTypes)
			{
				if (CandidateTypes.TryGetValue(definitionType, out var value))
				{
					m_inventoryConstraint.AddObjectBuilderType(value.Item1);
				}
			}
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
			}
			MyInventory inventory = this.GetInventory();
			if (inventory == null)
			{
				inventory = new MyInventory(m_conveyorSorterDefinition.InventorySize.Volume, m_conveyorSorterDefinition.InventorySize, MyInventoryFlags.CanSend);
				base.Components.Add((MyInventoryBase)inventory);
				inventory.Init(myObjectBuilder_ConveyorSorter.Inventory);
			}
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
			base.ResourceSink.Update();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			MyObjectBuilder_ConveyorSorter myObjectBuilder_ConveyorSorter = (MyObjectBuilder_ConveyorSorter)base.GetObjectBuilderCubeBlock(copy);
			myObjectBuilder_ConveyorSorter.DrainAll = DrainAll;
			myObjectBuilder_ConveyorSorter.IsWhiteList = IsWhitelist;
			Enumerator<MyDefinitionId> enumerator = m_inventoryConstraint.ConstrainedIds.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDefinitionId current = enumerator.get_Current();
					myObjectBuilder_ConveyorSorter.DefinitionIds.Add((SerializableDefinitionId)current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			Enumerator<MyObjectBuilderType> enumerator2 = m_inventoryConstraint.ConstrainedTypes.GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (CandidateTypesToId.TryGetValue(constrainedType, out var value))
=======
				while (enumerator2.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyObjectBuilderType current2 = enumerator2.get_Current();
					if (CandidateTypesToId.TryGetValue(current2, out var value))
					{
						myObjectBuilder_ConveyorSorter.DefinitionTypes.Add(value);
					}
				}
				return myObjectBuilder_ConveyorSorter;
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			UpdateEmissivity();
		}

		private float UpdatePowerInput()
		{
			if (!Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			return BlockDefinition.PowerInput;
		}

		protected override void OnEnabledChanged()
		{
			base.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
			base.ResourceSink.Update();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
			base.OnEnabledChanged();
		}

		private void UpdateEmissivity()
		{
			if (base.IsFunctional)
			{
				bool flag = CheckIsWorking() && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
				if (m_prevColor != MyCubeBlock.m_emissiveNames.Working && flag)
				{
					SetEmissiveStateWorking();
					m_prevColor = MyCubeBlock.m_emissiveNames.Working;
				}
				else if (m_prevColor != MyCubeBlock.m_emissiveNames.Disabled && !flag)
				{
					SetEmissiveStateDisabled();
					m_prevColor = MyCubeBlock.m_emissiveNames.Disabled;
				}
			}
			else if (m_prevColor != MyCubeBlock.m_emissiveNames.Damaged)
			{
				SetEmissiveStateDamaged();
				m_prevColor = MyCubeBlock.m_emissiveNames.Damaged;
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

		private void IsPoweredChanged()
		{
			base.ResourceSink.Update();
			UpdateIsWorking();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
			UpdateEmissivity();
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentRemoved(inventory);
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
<<<<<<< HEAD
			if (!Sync.IsServer || !DrainAll || !Enabled || !base.IsFunctional || !base.IsWorking)
=======
			if (!Sync.IsServer || !DrainAll || !base.Enabled || !base.IsFunctional || !base.IsWorking)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			m_pushRequestFrameCounter++;
			if (m_pushRequestFrameCounter >= 4)
			{
				m_pushRequestFrameCounter = 0;
				MyInventory inventory = this.GetInventory();
				if (inventory.GetItemsCount() > 0)
<<<<<<< HEAD
				{
					MyGridConveyorSystem.PushAnyRequest(this, inventory);
				}
			}
			m_pullRequestFrameCounter++;
			if (m_pullRequestFrameCounter >= 10)
			{
				m_pullRequestFrameCounter = 0;
				MyInventory inventory2 = this.GetInventory();
				_ = inventory2.VolumeFillFactor;
				if (inventory2.VolumeFillFactor < 0.99f && MyGridConveyorSystem.PullAllRequestForSorter(this, inventory2, m_inventoryConstraint, MAX_ITEMS_TO_PULL_IN_ONE_TICK))
				{
=======
				{
					MyGridConveyorSystem.PushAnyRequest(this, inventory);
				}
			}
			m_pullRequestFrameCounter++;
			if (m_pullRequestFrameCounter >= 10)
			{
				m_pullRequestFrameCounter = 0;
				MyInventory inventory2 = this.GetInventory();
				_ = inventory2.VolumeFillFactor;
				if (inventory2.VolumeFillFactor < 0.99f && MyGridConveyorSystem.PullAllRequestForSorter(this, inventory2, m_inventoryConstraint, MAX_ITEMS_TO_PULL_IN_ONE_TICK))
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_pullRequestFrameCounter = 10;
				}
			}
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

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
			UpdateEmissivity();
		}

<<<<<<< HEAD
		[Event(null, 634)]
=======
		[Event(null, 633)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void DoSetupFilter(MyConveyorSorterMode mode, List<MyInventoryItemFilter> items)
		{
			IsWhitelist = mode == MyConveyorSorterMode.Whitelist;
			m_inventoryConstraint.Clear();
			if (items != null)
			{
				m_allowCurrentListUpdate = false;
				try
				{
					foreach (MyInventoryItemFilter item in items)
					{
						if (item.AllSubTypes)
						{
							m_inventoryConstraint.AddObjectBuilderType(item.ItemId.TypeId);
						}
						else
						{
							m_inventoryConstraint.Add(item.ItemId);
						}
					}
				}
				finally
				{
					m_allowCurrentListUpdate = true;
				}
			}
			base.CubeGrid.GridSystems.ConveyorSystem.FlagForRecomputation();
			currentList.UpdateVisual();
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return this.GetInventory(index);
		}

		public PullInformation GetPullInformation()
		{
			return new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId,
				Constraint = m_inventoryConstraint
			};
		}

		public PullInformation GetPushInformation()
		{
			return new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId,
				Constraint = new MyInventoryConstraint("Empty constraint")
			};
		}

		public bool AllowSelfPulling()
		{
			return false;
		}

		void Sandbox.ModAPI.Ingame.IMyConveyorSorter.GetFilterList(List<MyInventoryItemFilter> items)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			items.Clear();
			Enumerator<MyObjectBuilderType> enumerator = m_inventoryConstraint.ConstrainedTypes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyObjectBuilderType current = enumerator.get_Current();
					items.Add(new MyInventoryItemFilter(new MyDefinitionId(current), allSubTypes: true));
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			Enumerator<MyDefinitionId> enumerator2 = m_inventoryConstraint.ConstrainedIds.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					MyDefinitionId current2 = enumerator2.get_Current();
					items.Add(new MyInventoryItemFilter(current2));
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
		}

		void Sandbox.ModAPI.Ingame.IMyConveyorSorter.SetFilter(MyConveyorSorterMode mode, List<MyInventoryItemFilter> items)
		{
			MyMultiplayer.RaiseEvent(this, (MyConveyorSorter x) => x.DoSetupFilter, mode, items);
		}

		void Sandbox.ModAPI.Ingame.IMyConveyorSorter.AddItem(MyInventoryItemFilter item)
		{
			if (item.AllSubTypes)
			{
				if (CandidateTypesToId.TryGetValue(item.ItemId.TypeId, out var value))
				{
					ChangeListType(value, wasAdded: true);
				}
			}
			else
			{
				ChangeListId(item.ItemId, wasAdded: true);
			}
		}

		void Sandbox.ModAPI.Ingame.IMyConveyorSorter.RemoveItem(MyInventoryItemFilter item)
		{
			if (item.AllSubTypes)
			{
				if (CandidateTypesToId.TryGetValue(item.ItemId.TypeId, out var value))
				{
					ChangeListType(value, wasAdded: false);
				}
			}
			else
			{
				ChangeListId(item.ItemId, wasAdded: false);
			}
		}
	}
}
