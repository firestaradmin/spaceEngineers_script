using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_TerminalBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyTerminalBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyTerminalBlock)
	})]
	public class MyTerminalBlock : MySyncedBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock
	{
		public enum AccessRightsResult
		{
			Granted,
			Enemies,
			MissingDLC,
			Other,
			None
		}

		protected sealed class SetCustomNameEvent_003C_003ESystem_String : ICallSite<MyTerminalBlock, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTerminalBlock @this, in string name, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetCustomNameEvent(name);
			}
		}

		protected sealed class OnCustomDataChanged_003C_003ESystem_String : ICallSite<MyTerminalBlock, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTerminalBlock @this, in string data, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnCustomDataChanged(data);
			}
		}

		protected sealed class OnChangeOpenRequest_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64 : ICallSite<MyTerminalBlock, bool, bool, ulong, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTerminalBlock @this, in bool isOpen, in bool editable, in ulong user, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenRequest(isOpen, editable, user);
			}
		}

		protected sealed class OnChangeOpenSuccess_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64 : ICallSite<MyTerminalBlock, bool, bool, ulong, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTerminalBlock @this, in bool isOpen, in bool editable, in ulong user, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenSuccess(isOpen, editable, user);
			}
		}

		protected class m_showOnHUD_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType showOnHUD;
				ISyncType result = (showOnHUD = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyTerminalBlock)P_0).m_showOnHUD = (Sync<bool, SyncDirection.BothWays>)showOnHUD;
				return result;
			}
		}

		protected class m_showInTerminal_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType showInTerminal;
				ISyncType result = (showInTerminal = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyTerminalBlock)P_0).m_showInTerminal = (Sync<bool, SyncDirection.BothWays>)showInTerminal;
				return result;
			}
		}

		protected class m_showInToolbarConfig_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType showInToolbarConfig;
				ISyncType result = (showInToolbarConfig = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyTerminalBlock)P_0).m_showInToolbarConfig = (Sync<bool, SyncDirection.BothWays>)showInToolbarConfig;
				return result;
			}
		}

		protected class m_showInInventory_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType showInInventory;
				ISyncType result = (showInInventory = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyTerminalBlock)P_0).m_showInInventory = (Sync<bool, SyncDirection.BothWays>)showInInventory;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyTerminalBlock_003C_003EActor : IActivator, IActivator<MyTerminalBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyTerminalBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTerminalBlock CreateInstance()
			{
				return new MyTerminalBlock();
			}

			MyTerminalBlock IActivator<MyTerminalBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly Guid m_storageGuid = new Guid("74DE02B3-27F9-4960-B1C4-27351F2B06D1");

		private const int DATA_CHARACTER_LIMIT = 64000;

		private Sync<bool, SyncDirection.BothWays> m_showOnHUD;

		private Sync<bool, SyncDirection.BothWays> m_showInTerminal;

		private Sync<bool, SyncDirection.BothWays> m_showInToolbarConfig;

		private Sync<bool, SyncDirection.BothWays> m_showInInventory;

		private bool m_isBeingHackedPrevValue;

		private MyGuiScreenTextPanel m_textBox;

		protected bool m_textboxOpen;

		private ulong m_currentUser;

		/// <summary>
		/// Name in terminal
		/// </summary>
		private StringBuilder m_customName;

		private StringBuilder m_defaultCustomName;

		public int? HackAttemptTime;

		public bool IsAccessibleForProgrammableBlock = true;

		private bool m_detailedInfoDirty;

		private readonly StringBuilder m_detailedInfo = new StringBuilder();

		private static FastResourceLock m_createControlsLock = new FastResourceLock();

<<<<<<< HEAD
		public StringBuilder CustomName
		{
			get
			{
				if (m_customName == null)
				{
					return m_defaultCustomName;
				}
				return m_customName;
			}
			private set
			{
				m_customName = value;
			}
		}

		public override string DisplayNameText
		{
			get
			{
				return CustomName.ToString();
			}
			set
			{
				CustomName = new StringBuilder(value);
			}
		}
=======
		public StringBuilder CustomName { get; private set; }

		public StringBuilder CustomNameWithFaction { get; private set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public StringBuilder CustomNameWithFaction { get; private set; }

		public string CustomData
		{
			get
			{
				if (base.Storage == null || !base.Storage.TryGetValue(m_storageGuid, out var value))
				{
					return string.Empty;
				}
				return value;
			}
			set
			{
				SetCustomData_Internal(value, sync: true);
			}
		}

		public bool ShowOnHUD
		{
			get
			{
				return m_showOnHUD;
			}
			set
			{
				if ((bool)m_showOnHUD != value && CanShowOnHud)
				{
					m_showOnHUD.Value = value;
					RaiseShowOnHUDChanged();
				}
			}
		}

		public bool ShowInTerminal
		{
			get
			{
				return m_showInTerminal;
			}
			set
			{
				if ((bool)m_showInTerminal != value)
				{
					m_showInTerminal.Value = value;
					RaiseShowInTerminalChanged();
				}
			}
		}

		public bool ShowInInventory
		{
			get
			{
				return m_showInInventory;
			}
			set
			{
				if ((bool)m_showInInventory != value)
				{
					m_showInInventory.Value = value;
					RaiseShowInInventoryChanged();
				}
			}
		}

		public bool ShowInToolbarConfig
		{
			get
			{
				return m_showInToolbarConfig;
			}
			set
			{
				if ((bool)m_showInToolbarConfig != value)
				{
					m_showInToolbarConfig.Value = value;
					RaiseShowInToolbarConfigChanged();
				}
			}
		}

		public override bool IsBeingHacked
		{
			get
			{
				if (!HackAttemptTime.HasValue)
				{
					return false;
				}
				bool flag = MySandboxGame.TotalSimulationTimeInMilliseconds - HackAttemptTime.Value < 1000;
				if (flag != m_isBeingHackedPrevValue)
				{
					m_isBeingHackedPrevValue = flag;
					RaiseIsBeingHackedChanged();
				}
				return flag;
			}
		}

		/// <summary>
		/// Detailed text in terminal (on right side)
		/// </summary>
		public StringBuilder DetailedInfo
		{
			get
			{
				if (m_detailedInfoDirty)
				{
					m_detailedInfoDirty = false;
					UpdateDetailedInfo(m_detailedInfo);
				}
				return m_detailedInfo;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Moddable part of detailed text in terminal.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public StringBuilder CustomInfo { get; private set; }

		public bool HasUnsafeValues { get; private set; }

		public bool IsOpenedInTerminal { get; set; }

		protected virtual bool CanShowOnHud => true;

		string Sandbox.ModAPI.Ingame.IMyTerminalBlock.CustomName
		{
			get
			{
				return CustomName.ToString();
			}
			set
			{
				SetCustomName(value);
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTerminalBlock.CustomNameWithFaction => CustomNameWithFaction.ToString();

		string Sandbox.ModAPI.Ingame.IMyTerminalBlock.DetailedInfo => DetailedInfo.ToString();

		string Sandbox.ModAPI.Ingame.IMyTerminalBlock.CustomInfo => CustomInfo.ToString();

		public event Action<MyTerminalBlock> CustomDataChanged;

		public event Action<MyTerminalBlock> CustomNameChanged;

		public event Action<MyTerminalBlock> PropertiesChanged;

		public event Action<MyTerminalBlock> OwnershipChanged;

		public event Action<MyTerminalBlock> VisibilityChanged;

		public event Action<MyTerminalBlock> ShowOnHUDChanged;

		public event Action<MyTerminalBlock> ShowInTerminalChanged;

		public event Action<MyTerminalBlock> ShowInIventoryChanged;

		public event Action<MyTerminalBlock> ShowInToolbarConfigChanged;

		public event Action<MyTerminalBlock> IsBeingHackedChanged;

		public event Action<MyTerminalBlock, StringBuilder> AppendingCustomInfo;

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.CustomNameChanged
		{
			add
			{
				CustomNameChanged += GetDelegate(value);
			}
			remove
			{
				CustomNameChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.OwnershipChanged
		{
			add
			{
				OwnershipChanged += GetDelegate(value);
			}
			remove
			{
				OwnershipChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.PropertiesChanged
		{
			add
			{
				PropertiesChanged += GetDelegate(value);
			}
			remove
			{
				PropertiesChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.ShowOnHUDChanged
		{
			add
			{
				ShowOnHUDChanged += GetDelegate(value);
			}
			remove
			{
				ShowOnHUDChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.VisibilityChanged
		{
			add
			{
				VisibilityChanged += GetDelegate(value);
			}
			remove
			{
				VisibilityChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyTerminalBlock, StringBuilder> Sandbox.ModAPI.IMyTerminalBlock.AppendingCustomInfo
		{
			add
			{
				AppendingCustomInfo += GetDelegate(value);
			}
			remove
			{
				AppendingCustomInfo -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyTerminalBlock.CustomDataChanged
		{
			add
			{
				CustomDataChanged += GetDelegate(value);
			}
			remove
			{
				CustomDataChanged -= GetDelegate(value);
			}
		}

		private void SetCustomData_Internal(string value, bool sync)
		{
			if (base.Storage == null)
			{
				base.Storage = new MyModStorageComponent();
				base.Components.Add(base.Storage);
			}
			if (value.Length > 64000)
			{
				value = value.Substring(0, 64000);
			}
			if (!base.Storage.TryGetValue(m_storageGuid, out var value2) || value2 != value)
			{
				base.Storage[m_storageGuid] = value;
				if (sync)
				{
					RaiseCustomDataChanged();
				}
				else
				{
					this.CustomDataChanged?.Invoke(this);
				}
			}
		}

		public MyTerminalBlock()
		{
			using (m_createControlsLock.AcquireExclusiveUsing())
			{
				CreateTerminalControls();
			}
			CustomInfo = new StringBuilder();
			CustomNameWithFaction = new StringBuilder();
			m_defaultCustomName = new StringBuilder();
			base.SyncType.PropertyChanged += delegate
			{
				RaisePropertiesChanged();
			};
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_TerminalBlock myObjectBuilder_TerminalBlock = (MyObjectBuilder_TerminalBlock)objectBuilder;
			m_defaultCustomName.Clear().Append(base.BlockDefinition.DisplayNameText);
			if (base.NumberInGrid > 1)
			{
				m_defaultCustomName.Append(" ").Append(base.NumberInGrid);
			}
			if (myObjectBuilder_TerminalBlock.CustomName != null && myObjectBuilder_TerminalBlock.CustomName.Length > 0)
			{
				CustomName = new StringBuilder(myObjectBuilder_TerminalBlock.CustomName);
			}
			if (Sync.IsServer && Sync.Clients != null)
			{
				Sync.Clients.ClientRemoved += ClientRemoved;
			}
			m_showOnHUD.ValueChanged += m_showOnHUD_ValueChanged;
			m_showOnHUD.SetLocalValue(myObjectBuilder_TerminalBlock.ShowOnHUD);
			m_showInTerminal.SetLocalValue(myObjectBuilder_TerminalBlock.ShowInTerminal);
			m_showInInventory.SetLocalValue(myObjectBuilder_TerminalBlock.ShowInInventory);
			m_showInToolbarConfig.SetLocalValue(myObjectBuilder_TerminalBlock.ShowInToolbarConfig);
			AddDebugRenderComponent(new MyDebugRenderComponentTerminal(this));
		}

		private void m_showOnHUD_ValueChanged(SyncBase obj)
		{
			_ = base.CubeGrid;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (this is IMyControllableEntity)
			{
				MyPlayerCollection.UpdateControl(this);
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			if (HasUnsafeValues)
			{
				base.CubeGrid.UnregisterUnsafeBlock(this);
			}
			base.OnRemovedFromScene(source);
		}

		protected override void Closing()
		{
			base.Closing();
			if (Sync.IsServer && Sync.Clients != null)
			{
				Sync.Clients.ClientRemoved -= ClientRemoved;
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_TerminalBlock obj = (MyObjectBuilder_TerminalBlock)base.GetObjectBuilderCubeBlock(copy);
			obj.CustomName = m_customName?.ToString();
			obj.ShowOnHUD = ShowOnHUD;
			obj.ShowInTerminal = ShowInTerminal;
			obj.ShowInInventory = ShowInInventory;
			obj.ShowInToolbarConfig = ShowInToolbarConfig;
			return obj;
		}

		public void NotifyTerminalValueChanged(ITerminalControl control)
		{
		}

		public void RefreshCustomInfo()
		{
			CustomInfo.Clear();
			this.AppendingCustomInfo?.Invoke(this, CustomInfo);
		}

		public void SetCustomName(string text)
		{
			UpdateCustomName(text);
			MyMultiplayer.RaiseEvent(this, (MyTerminalBlock x) => x.SetCustomNameEvent, text);
		}

		public void UpdateCustomName(string text)
		{
			if (CustomName.CompareUpdate(text))
			{
				RaiseCustomNameChanged();
				RaiseShowOnHUDChanged();
				DisplayNameText = text;
			}
		}

		public void SetCustomName(StringBuilder text)
		{
			UpdateCustomName(text);
			MyMultiplayer.RaiseEvent(this, (MyTerminalBlock x) => x.SetCustomNameEvent, text.ToString());
		}

		[Event(null, 380)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		public void SetCustomNameEvent(string name)
		{
			UpdateCustomName(name);
		}

		public void UpdateCustomName(StringBuilder text)
		{
			if (CustomName.CompareUpdate(text))
			{
				DisplayNameText = text.ToString();
				RaiseCustomNameChanged();
				RaiseShowOnHUDChanged();
			}
		}

		/// <summary>
		/// Call this when you change the name
		/// </summary>
		private void RaiseCustomNameChanged()
		{
			this.CustomNameChanged?.Invoke(this);
		}

		/// <summary>
		/// Call this when you change detailed info or other terminal properties
		/// </summary>
		public void RaisePropertiesChanged()
		{
			this.PropertiesChanged?.Invoke(this);
		}

		public void SetDetailedInfoDirty()
		{
			m_detailedInfoDirty = true;
		}

		/// <summary>
		/// Call this when you change the properties that modify the visibility of this block's controls
		/// </summary>
		protected void RaiseVisibilityChanged()
		{
			this.VisibilityChanged?.Invoke(this);
		}

		protected void RaiseShowOnHUDChanged()
		{
			this.ShowOnHUDChanged?.Invoke(this);
		}

		protected void RaiseShowInTerminalChanged()
		{
			this.ShowInTerminalChanged?.Invoke(this);
		}

		protected void RaiseShowInInventoryChanged()
		{
			this.ShowInIventoryChanged?.Invoke(this);
		}

		protected void RaiseShowInToolbarConfigChanged()
		{
			this.ShowInToolbarConfigChanged?.Invoke(this);
		}

		protected void RaiseIsBeingHackedChanged()
		{
			this.IsBeingHackedChanged?.Invoke(this);
		}

		/// <summary>
		/// Is true when you dont see "(access denied)" near block name
		/// </summary>
		public bool CanLocalPlayerChangeValue()
		{
			return CanPlayerChangeValue(MySession.Static.LocalPlayerId);
		}

		/// <summary>
		/// Is true when you dont see "(access denied)" near block name
		/// </summary>
		public bool CanPlayerChangeValue(long identityId)
		{
			if (base.IDModule == null)
			{
				return HasPlayerAccessToBlockWithoutOwnership(identityId);
			}
			return HasPlayerAccess(identityId);
		}

		/// <summary>
		/// Don't forget to check `block.IDModule == null` first
		/// </summary>
		/// <returns></returns>
		public bool HasLocalPlayerAccessToBlockWithoutOwnership()
		{
			return HasPlayerAccessToBlockWithoutOwnership(MySession.Static.LocalPlayerId);
		}

		/// <summary>
		/// Don't forget to check `block.IDModule == null` first
		/// </summary>
		/// <returns></returns>
		public bool HasPlayerAccessToBlockWithoutOwnership(long identityId)
		{
			if (HasLocalPlayerAdminUseTerminals())
			{
				return true;
			}
			if (base.CubeGrid == null || base.IDModule != null)
			{
				return false;
			}
			List<long> bigOwners = base.CubeGrid.BigOwners;
			if (bigOwners != null && bigOwners.Count == 0)
			{
				return true;
			}
			if (base.CubeGrid.BigOwners != null)
			{
				foreach (long item in base.CubeGrid?.BigOwners)
				{
					MyRelationsBetweenPlayers relationPlayerPlayer = MyIDModule.GetRelationPlayerPlayer(item, identityId, MyRelationsBetweenFactions.Neutral, MyRelationsBetweenPlayers.Neutral);
					if (relationPlayerPlayer == MyRelationsBetweenPlayers.Allies || relationPlayerPlayer == MyRelationsBetweenPlayers.Self)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool HasLocalPlayerAccess()
		{
			return HasPlayerAccess(MySession.Static.LocalPlayerId);
		}

		public bool HasPlayerAccess(long identityId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
		{
			return HasPlayerAccessReason(identityId, defaultNoUser) == AccessRightsResult.Granted;
		}

		public AccessRightsResult HasPlayerAccessReason(long identityId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
		{
			if (!MyFakes.SHOW_FACTIONS_GUI)
			{
				return AccessRightsResult.Other;
			}
			if (HasAdminUseTerminals(identityId))
			{
				return AccessRightsResult.Granted;
			}
			if (!GetUserRelationToOwner(identityId, defaultNoUser).IsFriendly())
			{
				return AccessRightsResult.Enemies;
			}
			return AccessRightsResult.Granted;
		}

		internal bool HasLocalPlayerAdminUseTerminals()
		{
			return HasAdminUseTerminals(MySession.Static.LocalPlayerId);
		}

		internal bool HasAdminUseTerminals(long identityId)
		{
			ulong key = MySession.Static.Players.TryGetSteamId(identityId);
			AdminSettingsEnum value2;
			if (Sync.IsServer)
			{
				if (MySession.Static.RemoteAdminSettings.TryGetValue(key, out var value) && (value & AdminSettingsEnum.UseTerminals) != 0)
				{
					return true;
				}
			}
			else if (identityId == MySession.Static.LocalPlayerId)
			{
				if ((MySession.Static.AdminSettings & AdminSettingsEnum.UseTerminals) != 0)
				{
					return true;
				}
			}
			else if (MySession.Static.RemoteAdminSettings.TryGetValue(key, out value2) && (value2 & AdminSettingsEnum.UseTerminals) != 0)
			{
				return true;
			}
			return false;
		}

		public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
		{
			CustomNameWithFaction.Clear();
			if (!string.IsNullOrEmpty(GetOwnerFactionTag()))
			{
				CustomNameWithFaction.Append(GetOwnerFactionTag());
				CustomNameWithFaction.Append(".");
			}
			CustomNameWithFaction.AppendStringBuilder(CustomName);
			m_hudParams.Clear();
			m_hudParams.Add(new MyHudEntityParams
			{
				FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_ALL,
				Text = CustomNameWithFaction,
				Owner = ((base.IDModule != null) ? base.IDModule.Owner : 0),
				Share = ((base.IDModule != null) ? base.IDModule.ShareMode : MyOwnershipShareModeEnum.None),
				Entity = this,
				BlinkingTime = ((allowBlink && IsBeingHacked) ? 10 : 0)
			});
			return m_hudParams;
		}

		protected override void OnOwnershipChanged()
		{
			base.OnOwnershipChanged();
			RaiseOwnershipChanged();
			RaiseShowOnHUDChanged();
			RaisePropertiesChanged();
		}

		private void RaiseOwnershipChanged()
		{
			if (this.OwnershipChanged != null)
			{
				this.OwnershipChanged(this);
			}
		}

		/// <summary>
		/// Display name for terminal. Override this to alter how the block's name is displayed in the terminal.
		/// </summary>
		/// <param name="result"></param>
		public virtual void GetTerminalName(StringBuilder result)
		{
			result.AppendStringBuilder(CustomName);
		}

		protected void PrintUpgradeModuleInfo(StringBuilder output)
		{
			if (GetComponent().ConnectionPositions.get_Count() == 0)
			{
				return;
			}
			int num = 0;
			if (CurrentAttachedUpgradeModules != null)
			{
				foreach (AttachedUpgradeModule value in CurrentAttachedUpgradeModules.Values)
				{
					num += value.SlotCount;
				}
			}
			output.Append(MyTexts.Get(MyCommonTexts.Module_UsedSlots).ToString() + num + " / " + GetComponent().ConnectionPositions.get_Count() + "\n");
			if (CurrentAttachedUpgradeModules != null)
			{
				int num2 = 0;
				foreach (AttachedUpgradeModule value2 in CurrentAttachedUpgradeModules.Values)
				{
					num2 += ((value2.Block != null && value2.Block.IsWorking) ? 1 : 0);
				}
				output.Append(MyTexts.Get(MyCommonTexts.Module_Attached).ToString() + CurrentAttachedUpgradeModules.Count);
				if (num2 != CurrentAttachedUpgradeModules.Count)
				{
					output.Append(" (" + num2 + MyTexts.Get(MyCommonTexts.Module_Functioning).ToString());
				}
				output.Append("\n");
				foreach (AttachedUpgradeModule value3 in CurrentAttachedUpgradeModules.Values)
				{
					if (value3.Block != null)
					{
						output.Append(" - " + value3.Block.DisplayNameText + ((!value3.Block.IsFunctional) ? MyTexts.Get(MyCommonTexts.Module_Damaged).ToString() : ((!value3.Compatible) ? MyTexts.Get(MyCommonTexts.Module_Incompatible).ToString() : (value3.Block.Enabled ? "" : MyTexts.Get(MyCommonTexts.Module_Off).ToString()))));
					}
					else
					{
						output.Append(MyTexts.Get(MyCommonTexts.Module_Unknown).ToString());
					}
					output.Append("\n");
				}
			}
			output.AppendFormat("\n");
		}

		protected void FixSingleInventory()
		{
			if (!base.Components.TryGet<MyInventoryBase>(out var component))
			{
				return;
			}
			MyInventoryAggregate myInventoryAggregate = component as MyInventoryAggregate;
			MyInventory myInventory = null;
			if (myInventoryAggregate != null)
			{
				foreach (MyComponentBase item in myInventoryAggregate.ChildList.Reader)
				{
					MyInventory myInventory2 = item as MyInventory;
					if (myInventory2 != null)
					{
						if (myInventory == null)
<<<<<<< HEAD
						{
							myInventory = myInventory2;
						}
						else if (myInventory.GetItemsCount() < myInventory2.GetItemsCount())
						{
							myInventory = myInventory2;
=======
						{
							myInventory = myInventory2;
						}
						else if (myInventory.GetItemsCount() < myInventory2.GetItemsCount())
						{
							myInventory = myInventory2;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
			}
			if (myInventory != null)
			{
				base.Components.Remove<MyInventoryBase>();
				base.Components.Add((MyInventoryBase)myInventory);
			}
		}

		/// <summary>
		/// Control creation was moved from the static ctor into this static function.  Control creation should still be static, but static ctors
		/// only ever get called once, which means we can never modify these controls (remove), since they will be removed forever.  All classes
		/// that inherit MyTerminalBlock should put terminal control creation in a function called CreateTerminalControls, as MyTerminalControlFactory 
		/// will properly ensure their base classes' controls are added in.  I can't make this virtual because terminal controls don't deal with instances
		/// directly (this should probably change)
		///
		/// GR: Had to change this from static due to parallelization issues with multuple threads. Now it should run only once.
		/// </summary>
		protected virtual void CreateTerminalControls()
		{
			if (MyTerminalControlFactory.AreControlsCreated<MyTerminalBlock>())
			{
				return;
			}
			MyTerminalControlFactory.AddControl(new MyTerminalControlOnOffSwitch<MyTerminalBlock>("ShowInTerminal", MySpaceTexts.Terminal_ShowInTerminal, MySpaceTexts.Terminal_ShowInTerminalToolTip)
			{
				Getter = (MyTerminalBlock x) => x.m_showInTerminal,
				Setter = delegate(MyTerminalBlock x, bool v)
				{
					x.ShowInTerminal = v;
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlOnOffSwitch<MyTerminalBlock>("ShowInInventory", MySpaceTexts.Terminal_ShowInInventory, MySpaceTexts.Terminal_ShowInInventoryToolTip, null, null, 0.25f, is_AutoEllipsisEnabled: true, is_AutoScaleEnabled: true)
			{
				Getter = (MyTerminalBlock x) => x.m_showInInventory,
				Setter = delegate(MyTerminalBlock x, bool v)
				{
					x.ShowInInventory = v;
				},
				Visible = (MyTerminalBlock x) => x.HasInventory
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlOnOffSwitch<MyTerminalBlock>("ShowInToolbarConfig", MySpaceTexts.Terminal_ShowInToolbarConfig, MySpaceTexts.Terminal_ShowInToolbarConfigToolTip, null, null, 0.25f, is_AutoEllipsisEnabled: true, is_AutoScaleEnabled: true)
			{
				Getter = (MyTerminalBlock x) => x.m_showInToolbarConfig,
				Setter = delegate(MyTerminalBlock x, bool v)
				{
					x.ShowInToolbarConfig = v;
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MyTerminalBlock>("Name", MyCommonTexts.Name, MySpaceTexts.Blank)
			{
				Getter = (MyTerminalBlock x) => x.CustomName,
				Setter = delegate(MyTerminalBlock x, StringBuilder v)
				{
					x.SetCustomName(v);
				},
				SupportsMultipleBlocks = false
			});
			MyTerminalControlOnOffSwitch<MyTerminalBlock> myTerminalControlOnOffSwitch = new MyTerminalControlOnOffSwitch<MyTerminalBlock>("ShowOnHUD", MySpaceTexts.Terminal_ShowOnHUD, MySpaceTexts.Terminal_ShowOnHUDToolTip);
			myTerminalControlOnOffSwitch.Getter = (MyTerminalBlock x) => x.ShowOnHUD;
			myTerminalControlOnOffSwitch.Setter = delegate(MyTerminalBlock x, bool v)
			{
				x.ShowOnHUD = v;
			};
			myTerminalControlOnOffSwitch.EnableToggleAction();
			myTerminalControlOnOffSwitch.EnableOnOffActions();
			myTerminalControlOnOffSwitch.Visible = (MyTerminalBlock x) => x.CanShowOnHud;
			myTerminalControlOnOffSwitch.Enabled = (MyTerminalBlock x) => x.CanShowOnHud;
			MyTerminalAction<MyTerminalBlock>[] actions = myTerminalControlOnOffSwitch.Actions;
			for (int i = 0; i < actions.Length; i++)
			{
				actions[i].Enabled = (MyTerminalBlock x) => x.CanShowOnHud;
			}
			MyTerminalControlFactory.AddControl(myTerminalControlOnOffSwitch);
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyTerminalBlock>("CustomData", MySpaceTexts.Terminal_CustomData, MySpaceTexts.Terminal_CustomDataTooltip, CustomDataClicked)
			{
				Enabled = (MyTerminalBlock x) => !x.m_textboxOpen,
				SupportsMultipleBlocks = false
			});
		}

		protected static void CustomDataClicked(MyTerminalBlock myTerminalBlock)
		{
			myTerminalBlock.OpenWindow(isEditable: true, sync: true);
		}

		private void RaiseCustomDataChanged()
		{
			MyMultiplayer.RaiseEvent(this, (MyTerminalBlock x) => x.OnCustomDataChanged, CustomData);
		}

		[Event(null, 766)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		private void OnCustomDataChanged(string data)
		{
			SetCustomData_Internal(data, sync: false);
		}

		private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0uL)
		{
			MyMultiplayer.RaiseEvent(this, (MyTerminalBlock x) => x.OnChangeOpenRequest, isOpen, editable, user);
		}

		[Event(null, 777)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user)
		{
			if (!(Sync.IsServer && m_textboxOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user);
				MyMultiplayer.RaiseEvent(this, (MyTerminalBlock x) => x.OnChangeOpenSuccess, isOpen, editable, user);
			}
		}

		[Event(null, 788)]
		[Reliable]
		[Broadcast]
		private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user)
		{
			OnChangeOpen(isOpen, editable, user);
		}

		private void OnChangeOpen(bool isOpen, bool editable, ulong user)
		{
			m_textboxOpen = isOpen;
			m_currentUser = user;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && user == Sync.MyId && isOpen)
			{
				OpenWindow(editable, sync: false);
			}
		}

		private void CreateTextBox(bool isEditable, string description)
		{
			string missionTitle = CustomName.ToString();
			string @string = MyTexts.GetString(MySpaceTexts.Terminal_CustomData);
			bool editable = isEditable;
			m_textBox = new MyGuiScreenTextPanel(missionTitle, "", @string, description, OnClosedTextBox, null, null, editable);
		}

		public void OpenWindow(bool isEditable, bool sync)
		{
			if (sync)
			{
				SendChangeOpenMessage(isOpen: true, isEditable, Sync.MyId);
				return;
			}
			CreateTextBox(isEditable, CustomData);
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
			MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_textBox);
		}

		public void OnClosedTextBox(ResultEnum result)
		{
			if (m_textBox != null)
			{
				CloseWindow();
			}
		}

		public void OnClosedMessageBox(ResultEnum result)
		{
			if (result == ResultEnum.OK)
			{
				CloseWindow();
				return;
			}
			CreateTextBox(isEditable: true, m_textBox.Description.Text.ToString());
			MyScreenManager.AddScreen(m_textBox);
		}

		private void CloseWindow()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = null;
			Enumerator<MySlimBlock> enumerator = base.CubeGrid.CubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					if (current.FatBlock != null && current.FatBlock.EntityId == base.EntityId)
					{
						CustomData = m_textBox.Description.Text.ToString();
						SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
						break;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private void ClientRemoved(ulong steamId)
		{
			if (steamId == m_currentUser)
			{
				SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
			}
		}

		protected void OnUnsafeSettingsChanged()
		{
			MySandboxGame.Static.Invoke("", this, delegate(object x)
			{
				OnUnsafeSettingsChangedInternal(x);
			});
		}

		private static void OnUnsafeSettingsChangedInternal(object o)
		{
			MyTerminalBlock myTerminalBlock = (MyTerminalBlock)o;
			if (myTerminalBlock.MarkedForClose)
			{
				return;
			}
			bool flag = myTerminalBlock.HasUnsafeSettingsCollector();
			if (myTerminalBlock.HasUnsafeValues != flag)
			{
				myTerminalBlock.HasUnsafeValues = flag;
				if (flag)
				{
					myTerminalBlock.CubeGrid.RegisterUnsafeBlock(myTerminalBlock);
				}
				else
				{
					myTerminalBlock.CubeGrid.UnregisterUnsafeBlock(myTerminalBlock);
				}
			}
		}

		protected virtual bool HasUnsafeSettingsCollector()
		{
			return false;
		}

		protected virtual void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			detailedInfo.Clear();
		}

		public override string ToString()
		{
			return base.ToString() + " " + CustomName;
		}

		public virtual void OnOpenedInTerminal(bool state)
		{
		}

		private Action<MyTerminalBlock> GetDelegate(Action<Sandbox.ModAPI.IMyTerminalBlock> value)
		{
			return (Action<MyTerminalBlock>)Delegate.CreateDelegate(typeof(Action<MyTerminalBlock>), value.Target, value.Method);
		}

		private Action<MyTerminalBlock, StringBuilder> GetDelegate(Action<Sandbox.ModAPI.IMyTerminalBlock, StringBuilder> value)
		{
			return (Action<MyTerminalBlock, StringBuilder>)Delegate.CreateDelegate(typeof(Action<MyTerminalBlock, StringBuilder>), value.Target, value.Method);
		}

		bool Sandbox.ModAPI.IMyTerminalBlock.IsInSameLogicalGroupAs(Sandbox.ModAPI.IMyTerminalBlock other)
		{
			return base.CubeGrid.IsInSameLogicalGroupAs(other.CubeGrid);
		}

		bool Sandbox.ModAPI.IMyTerminalBlock.IsSameConstructAs(Sandbox.ModAPI.IMyTerminalBlock other)
		{
			return base.CubeGrid.IsSameConstructAs(other.CubeGrid);
		}

		void Sandbox.ModAPI.Ingame.IMyTerminalBlock.GetActions(List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect)
		{
			((IMyTerminalActionsHelper)MyTerminalControlFactoryHelper.Static).GetActions(GetType(), resultList, collect);
		}

		void Sandbox.ModAPI.Ingame.IMyTerminalBlock.SearchActionsOfName(string name, List<Sandbox.ModAPI.Interfaces.ITerminalAction> resultList, Func<Sandbox.ModAPI.Interfaces.ITerminalAction, bool> collect)
		{
			((IMyTerminalActionsHelper)MyTerminalControlFactoryHelper.Static).SearchActionsOfName(name, GetType(), resultList, collect);
		}

		Sandbox.ModAPI.Interfaces.ITerminalAction Sandbox.ModAPI.Ingame.IMyTerminalBlock.GetActionWithName(string name)
		{
			return ((IMyTerminalActionsHelper)MyTerminalControlFactoryHelper.Static).GetActionWithName(name, GetType());
		}

		public ITerminalProperty GetProperty(string id)
		{
			return ((IMyTerminalActionsHelper)MyTerminalControlFactoryHelper.Static).GetProperty(id, GetType());
		}

		public void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null)
		{
			((IMyTerminalActionsHelper)MyTerminalControlFactoryHelper.Static).GetProperties(GetType(), resultList, collect);
		}

		bool Sandbox.ModAPI.Ingame.IMyTerminalBlock.IsSameConstructAs(Sandbox.ModAPI.Ingame.IMyTerminalBlock other)
		{
			return ((VRage.Game.ModAPI.Ingame.IMyCubeGrid)base.CubeGrid).IsSameConstructAs(other.CubeGrid);
		}
	}
}
