using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
=======
using Sandbox.Engine.Platform;
using Sandbox.Game.Components;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents.Systems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
<<<<<<< HEAD
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entities;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders;
=======
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entities;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;
using VRage.Sync;

namespace Sandbox.Game.Entities.Cube
{
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyFunctionalBlock),
		typeof(Sandbox.ModAPI.Ingame.IMyFunctionalBlock)
	})]
<<<<<<< HEAD
	public class MyFunctionalBlock : MyTerminalBlock, IMyTieredUpdateBlock, IMyUpdateTimer, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
=======
	public class MyFunctionalBlock : MyTerminalBlock, IMyTieredUpdateBlock, IMyUpdateTimer, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		protected sealed class OnChangeTextRequest_003C_003ESystem_Int32_0023System_String : ICallSite<MyFunctionalBlock, int, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyFunctionalBlock @this, in int panelIndex, in string text, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeTextRequest(panelIndex, text);
			}
		}

		protected sealed class OnUpdateSpriteCollection_003C_003ESystem_Int32_0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyFunctionalBlock, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyFunctionalBlock @this, in int panelIndex, in MySerializableSpriteCollection sprites, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnUpdateSpriteCollection(panelIndex, sprites);
			}
		}

		protected sealed class OnRemoveSelectedImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyFunctionalBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyFunctionalBlock @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSelectedImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnSelectImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyFunctionalBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyFunctionalBlock @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSelectImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnChangeOpenSuccess_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyFunctionalBlock, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyFunctionalBlock @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeOpenRequest_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyFunctionalBlock, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyFunctionalBlock @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeDescription_003C_003ESystem_String_0023System_Boolean : ICallSite<MyFunctionalBlock, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyFunctionalBlock @this, in string description, in bool isPublic, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeDescription(description, isPublic);
			}
		}

		protected class m_enabled_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType enabled;
				ISyncType result = (enabled = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyFunctionalBlock)P_0).m_enabled = (Sync<bool, SyncDirection.BothWays>)enabled;
				return result;
			}
		}

		private class Sandbox_Game_Entities_Cube_MyFunctionalBlock_003C_003EActor : IActivator, IActivator<MyFunctionalBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyFunctionalBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFunctionalBlock CreateInstance()
			{
				return new MyFunctionalBlock();
			}

			MyFunctionalBlock IActivator<MyFunctionalBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyTimerComponent m_timer;

		private bool m_isTimerInRestartMode;

		protected MySoundPair m_baseIdleSound = new MySoundPair();

		protected MySoundPair m_actionSound = new MySoundPair();

		protected MyMultiTextPanelComponent m_multiPanel;

		protected MyGuiScreenTextPanel m_textBoxMultiPanel;

		private bool m_isTextPanelOpen;

		protected readonly Sync<bool, SyncDirection.BothWays> m_enabled;

		public virtual bool UseGenericLcd => true;

		/// <summary>
		/// Gets sound emitter. This value is null on dedicated server.
		/// </summary>
		internal MyEntity3DSoundEmitter SoundEmitter => m_soundEmitter;

		public virtual bool IsTieredUpdateSupported => false;

<<<<<<< HEAD
		/// <summary>
		/// Allows force update when block is turned off (Enabled false). Force Update prevents users from using exploit for generating energy/gas.
		/// It's overridden and turned off for production blocks (Refinery, Assembler, MyProductionBlock base class), where forcing has opposite effect.
		/// </summary>
		public virtual bool AllowTimerForceUpdate => true;

		public new MyFunctionalBlockDefinition BlockDefinition => base.BlockDefinition as MyFunctionalBlockDefinition;

		public virtual bool Enabled
=======
		public virtual bool AllowTimerForceUpdate => true;

		public bool Enabled
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			get
			{
				return m_enabled;
			}
			set
			{
				m_enabled.Value = value;
			}
		}

<<<<<<< HEAD
		public int SurfaceCount
		{
			get
			{
				if (m_multiPanel == null)
				{
					return 0;
				}
				return m_multiPanel.SurfaceCount;
			}
		}

		public MyMultiTextPanelComponent MultiTextPanel => m_multiPanel;

		public MyTextPanelComponent PanelComponent
		{
			get
			{
				if (m_multiPanel == null)
				{
					return null;
				}
				return m_multiPanel.PanelComponent;
			}
		}

		public bool IsTextPanelOpen
		{
			get
			{
				return m_isTextPanelOpen;
			}
			set
			{
				if (m_isTextPanelOpen != value)
				{
					m_isTextPanelOpen = value;
					RaisePropertiesChanged();
				}
			}
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool Sandbox.ModAPI.IMyFunctionalBlock.IsUpdateTimerCreated => m_timer != null;

		bool Sandbox.ModAPI.IMyFunctionalBlock.IsUpdateTimerEnabled
		{
			get
			{
				if (m_timer == null)
				{
					return false;
				}
				return m_timer.TimerEnabled;
			}
		}

		public event Action<MyTerminalBlock> EnabledChanged;

		public event Action<MyFunctionalBlock> UpdateTimerTriggered;

		event Action<Sandbox.ModAPI.IMyTerminalBlock> Sandbox.ModAPI.IMyFunctionalBlock.EnabledChanged
		{
			add
			{
				EnabledChanged += GetDelegate(value);
			}
			remove
			{
				EnabledChanged -= GetDelegate(value);
			}
		}

		event Action<Sandbox.ModAPI.IMyFunctionalBlock> Sandbox.ModAPI.IMyFunctionalBlock.UpdateTimerTriggered
		{
			add
			{
				UpdateTimerTriggered += GetDelegate(value);
			}
			remove
			{
				UpdateTimerTriggered -= GetDelegate(value);
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			if (base.Components.Contains(typeof(MyTriggerAggregate)))
			{
				base.Components.Remove(typeof(MyTriggerAggregate));
			}
			base.OnRemovedFromScene(source);
		}

		private void EnabledSyncChanged()
		{
			if (!base.Closed)
			{
				if (!Enabled && m_timer != null && AllowTimerForceUpdate)
				{
					UpdateTimer(forceUpdate: true);
				}
				UpdateIsWorking();
				OnEnabledChanged();
			}
		}

		public MyFunctionalBlock()
		{
			CreateTerminalControls();
			m_enabled.ValueChanged += delegate
			{
				EnabledSyncChanged();
			};
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyFunctionalBlock>())
			{
				base.CreateTerminalControls();
				MyTerminalControlOnOffSwitch<MyFunctionalBlock> myTerminalControlOnOffSwitch = new MyTerminalControlOnOffSwitch<MyFunctionalBlock>("OnOff", MySpaceTexts.BlockAction_Toggle);
				myTerminalControlOnOffSwitch.Getter = (MyFunctionalBlock x) => x.Enabled;
				myTerminalControlOnOffSwitch.Setter = delegate(MyFunctionalBlock x, bool v)
				{
					x.Enabled = v;
				};
				myTerminalControlOnOffSwitch.EnableToggleAction();
				myTerminalControlOnOffSwitch.EnableOnOffActions();
				MyTerminalControlFactory.AddControl(0, myTerminalControlOnOffSwitch);
				MyTerminalControlFactory.AddControl(1, new MyTerminalControlSeparator<MyFunctionalBlock>());
				MyMultiTextPanelComponent.CreateTerminalControls<MyFunctionalBlock>();
			}
		}

		protected override bool CheckIsWorking()
		{
			if (Enabled)
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			MyObjectBuilder_FunctionalBlock myObjectBuilder_FunctionalBlock = (MyObjectBuilder_FunctionalBlock)objectBuilder;
<<<<<<< HEAD
			m_enabled.SetLocalValue(myObjectBuilder_FunctionalBlock.Enabled);
			base.IsWorkingChanged += CubeBlock_IsWorkingChanged;
			m_baseIdleSound = BlockDefinition.PrimarySound;
			m_actionSound = BlockDefinition.ActionSound;
			if (BlockDefinition.ScreenAreas != null && BlockDefinition.ScreenAreas.Count > 0)
			{
				InitLcdComponent(myObjectBuilder_FunctionalBlock.GetTextPanelsData());
			}
			SetDetailedInfoDirty();
		}

		protected void InitLcdComponent(List<MySerializedTextPanelData> textPanels)
		{
			if (UseGenericLcd && m_multiPanel == null)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
				MyRenderComponentScreenAreas myRenderComponentScreenAreas;
				if ((myRenderComponentScreenAreas = base.Render as MyRenderComponentScreenAreas) != null)
				{
					myRenderComponentScreenAreas.IsUpdateModelPropertiesEnabled = true;
				}
				m_multiPanel = new MyMultiTextPanelComponent(this, BlockDefinition.ScreenAreas, textPanels, useOnlineTexture: false);
				m_multiPanel.Init(SendAddImagesToSelectionRequest, SendRemoveSelectedImageRequest, ChangeTextRequest, UpdateSpriteCollection);
			}
=======
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			}
			m_enabled.SetLocalValue(myObjectBuilder_FunctionalBlock.Enabled);
			base.IsWorkingChanged += CubeBlock_IsWorkingChanged;
			m_baseIdleSound = base.BlockDefinition.PrimarySound;
			m_actionSound = base.BlockDefinition.ActionSound;
			SetDetailedInfoDirty();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void CubeBlock_IsWorkingChanged(MyCubeBlock obj)
		{
			if (base.IsWorking)
			{
				OnStartWorking();
			}
			else
			{
				OnStopWorking();
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_FunctionalBlock myObjectBuilder_FunctionalBlock = (MyObjectBuilder_FunctionalBlock)base.GetObjectBuilderCubeBlock(copy);
			if (m_multiPanel != null)
			{
				myObjectBuilder_FunctionalBlock.TextPanelsNew = m_multiPanel.Serialize();
			}
			myObjectBuilder_FunctionalBlock.Enabled = Enabled;
			return myObjectBuilder_FunctionalBlock;
		}

		protected virtual void OnEnabledChanged()
		{
			if (base.IsWorking)
			{
				OnStartWorking();
			}
			else
			{
				OnStopWorking();
			}
			this.EnabledChanged.InvokeIfNotNull(this);
			RaisePropertiesChanged();
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (m_soundEmitter != null && SilenceInChange)
			{
				SilenceInChange = m_soundEmitter.FastUpdate(IsSilenced);
				if (!SilenceInChange && !UsedUpdateEveryFrame && !base.HasDamageEffect)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
			}
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (m_soundEmitter != null && MySector.MainCamera != null)
			{
				UpdateSoundEmitters();
			}
		}

		public virtual void UpdateSoundEmitters()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Update();
			}
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (m_timer != null && m_timer.TimerType == MyTimerTypes.Frame10 && Sync.IsServer)
			{
				UpdateTimer(forceUpdate: false);
			}
<<<<<<< HEAD
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateAfterSimulation(base.IsWorking);
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (m_timer != null && m_timer.TimerType == MyTimerTypes.Frame100 && Sync.IsServer)
			{
				UpdateTimer(forceUpdate: false);
			}
		}

		protected virtual void OnStartWorking()
		{
			if (base.InScene && base.CubeGrid.Physics != null && m_soundEmitter != null && m_baseIdleSound != null && m_baseIdleSound != MySoundPair.Empty)
			{
				m_soundEmitter.PlaySound(m_baseIdleSound, stopPrevious: true);
			}
		}

		protected virtual void OnStopWorking()
		{
			if (m_soundEmitter != null && (BlockDefinition.DamagedSound == null || m_soundEmitter.SoundId != BlockDefinition.DamagedSound.SoundId))
			{
				m_soundEmitter.StopSound(forced: false);
			}
		}

		protected override void Closing()
		{
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			base.Closing();
		}

		public override void SetDamageEffect(bool show)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				base.SetDamageEffect(show);
				_ = m_soundEmitter;
			}
		}

		public override void StopDamageEffect(bool stopSound = true)
		{
			base.StopDamageEffect(stopSound);
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
		}

		public virtual int GetBlockSpecificState()
		{
			return -1;
		}

		public void ChangeTier()
		{
			TiersChanged();
		}

		protected virtual void TiersChanged()
		{
		}

		/// <summary>
		/// Creates timer for block logic
		/// </summary>
		/// <param name="startingTimeInFrames">starting timer when block is created for Normal tiers</param>
		/// <param name="timerType"></param>
		/// <param name="start">if timer should start first and then count</param>
		public void CreateUpdateTimer(uint startingTimeInFrames, MyTimerTypes timerType, bool start = false)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			if (!base.Components.TryGet<MyTimerComponent>(out m_timer))
			{
				m_timer = new MyTimerComponent();
				m_timer.IsSessionUpdateEnabled = false;
				m_timer.SetType(timerType);
				m_timer.SetTimer(startingTimeInFrames, OnTimerTick, start, repeat: true);
				base.Components.Add(m_timer);
			}
			else
			{
				m_timer.SetType(timerType);
				m_timer.IsSessionUpdateEnabled = false;
				if (MyTimerComponentSystem.Static != null)
				{
					MyTimerComponentSystem.Static.Unregister(m_timer);
				}
				m_timer.EventToTrigger = OnTimerTick;
			}
			m_isTimerInRestartMode = start;
		}

		private void OnTimerTick(MyEntityComponentContainer obj)
		{
			if (base.CubeGrid == null || !base.CubeGrid.IsPreview)
			{
				DoUpdateTimerTick();
				this.UpdateTimerTriggered.InvokeIfNotNull(this);
			}
		}

		public uint GetFramesFromLastTrigger()
		{
			return m_timer.FramesFromLastTrigger;
		}

		public void ChangeTimerTick(uint timeTickInFrames)
		{
			m_timer.ChangeTimerTick(timeTickInFrames);
			if (timeTickInFrames == 0)
			{
				m_timer.Pause();
			}
		}

		private void UpdateTimer(bool forceUpdate)
		{
			if (GetTimerEnabledState())
			{
				if (!m_timer.TimerEnabled)
				{
					if (m_timer.TimerTickInFrames != 0)
					{
						m_timer.Resume(m_isTimerInRestartMode);
					}
					else
					{
						m_timer.Pause();
					}
				}
			}
			else if (m_timer.TimerEnabled)
			{
				m_timer.Pause();
			}
			m_timer.Update(forceUpdate);
		}

		public virtual bool GetTimerEnabledState()
		{
			return false;
		}

		/// <summary>
		/// This method is called only on server when Update Timer is triggered
		/// </summary>
		public virtual void DoUpdateTimerTick()
		{
		}

		protected uint GetTimerTime(int index)
		{
			if (index < 0 || BlockDefinition == null || BlockDefinition.TieredUpdateTimes == null || BlockDefinition.TieredUpdateTimes.Count <= index)
			{
				return GetDefaultTimeForUpdateTimer(index);
			}
			return BlockDefinition.TieredUpdateTimes[index];
		}

		protected virtual uint GetDefaultTimeForUpdateTimer(int index)
		{
			return 0u;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (m_multiPanel != null)
			{
				if (m_multiPanel.SurfaceCount > 0)
				{
					base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
				m_multiPanel.AddToScene();
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateScreen();
		}

		public void UpdateScreen()
		{
			m_multiPanel?.UpdateAfterSimulation(base.IsWorking);
		}

		private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyFunctionalBlock x) => x.OnSelectImageRequest, panelIndex, selection);
		}

		private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyFunctionalBlock x) => x.OnRemoveSelectedImageRequest, panelIndex, selection);
		}

		private void ChangeTextRequest(int panelIndex, string text)
		{
			MyMultiplayer.RaiseEvent(this, (MyFunctionalBlock x) => x.OnChangeTextRequest, panelIndex, text);
		}

		[Event(null, 535)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnChangeTextRequest(int panelIndex, [Nullable] string text)
		{
			m_multiPanel?.ChangeText(panelIndex, text);
		}

		private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyFunctionalBlock x) => x.OnUpdateSpriteCollection, panelIndex, sprites);
			}
		}

		[Event(null, 551)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		[DistanceRadius(32f)]
		private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);
		}

		public Sandbox.ModAPI.Ingame.IMyTextSurface GetSurface(int index)
		{
			if (m_multiPanel == null)
			{
				return null;
			}
			return m_multiPanel.GetSurface(index);
		}

		[Event(null, 562)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.RemoveItems(panelIndex, selection);
			}
		}

		[Event(null, 571)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnSelectImageRequest(int panelIndex, int[] selection)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectItems(panelIndex, selection);
			}
		}

		public void SelectPanel(List<MyGuiControlListbox.Item> selectedItems)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectPanel((int)selectedItems[0].UserData);
			}
			RaisePropertiesChanged();
		}

		public void OpenWindow(bool isEditable, bool sync, bool isPublic)
		{
			if (sync)
			{
				SendChangeOpenMessage(isOpen: true, isEditable, Sync.MyId, isPublic);
				return;
			}
			CreateTextBox(isEditable, new StringBuilder(PanelComponent.Text.ToString()), isPublic);
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
			MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_textBoxMultiPanel);
		}

		private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
		{
			string displayNameText = DisplayNameText;
			string displayName = PanelComponent.DisplayName;
			string description2 = description.ToString();
			bool editable = isEditable;
			m_textBoxMultiPanel = new MyGuiScreenTextPanel(displayNameText, "", displayName, description2, OnClosedPanelTextBox, null, null, editable);
		}

		public void OnClosedPanelTextBox(ResultEnum result)
		{
			if (m_textBoxMultiPanel != null)
			{
				if (m_textBoxMultiPanel.Description.Text.Length > 100000)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: OnClosedPanelMessageBox, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText)));
				}
				else
				{
					CloseWindow(isPublic: true);
				}
			}
		}

		public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				m_textBoxMultiPanel.Description.Text.Remove(100000, m_textBoxMultiPanel.Description.Text.Length - 100000);
				CloseWindow(isPublic: true);
			}
			else
			{
				CreateTextBox(isEditable: true, m_textBoxMultiPanel.Description.Text, isPublic: true);
				MyScreenManager.AddScreen(m_textBoxMultiPanel);
			}
		}

		[Event(null, 641)]
		[Reliable]
		[Broadcast]
		private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			OnChangeOpen(isOpen, editable, user, isPublic);
		}

		private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0uL, bool isPublic = false)
		{
			MyMultiplayer.RaiseEvent(this, (MyFunctionalBlock x) => x.OnChangeOpenRequest, isOpen, editable, user, isPublic);
		}

		[Event(null, 652)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			if (!(Sync.IsServer && IsTextPanelOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user, isPublic);
				MyMultiplayer.RaiseEvent(this, (MyFunctionalBlock x) => x.OnChangeOpenSuccess, isOpen, editable, user, isPublic);
			}
		}

		private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			IsTextPanelOpen = isOpen;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && user == Sync.MyId && isOpen)
			{
				OpenWindow(editable, sync: false, isPublic);
			}
		}

		private void CloseWindow(bool isPublic)
		{
			MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = null;
			foreach (MySlimBlock cubeBlock in base.CubeGrid.CubeBlocks)
			{
				if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == base.EntityId)
				{
					SendChangeDescriptionMessage(m_textBoxMultiPanel.Description.Text, isPublic);
					SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
					break;
				}
			}
		}

		private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
		{
			if (base.CubeGrid.IsPreview || !base.CubeGrid.SyncFlag)
			{
				PanelComponent.Text = description;
			}
			else if (description.CompareTo(PanelComponent.Text) != 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyFunctionalBlock x) => x.OnChangeDescription, description.ToString(), isPublic);
			}
		}

		[Event(null, 706)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void OnChangeDescription(string description, bool isPublic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Clear().Append(description);
			PanelComponent.Text = stringBuilder;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public void ChangeTier()
		{
			TiersChanged();
		}

		protected virtual void TiersChanged()
		{
		}

		public void CreateUpdateTimer(uint startingTimeInFrames, MyTimerTypes timerType, bool start = false)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			if (!base.Components.TryGet<MyTimerComponent>(out m_timer))
			{
				m_timer = new MyTimerComponent();
				m_timer.IsSessionUpdateEnabled = false;
				m_timer.SetType(timerType);
				m_timer.SetTimer(startingTimeInFrames, OnTimerTick, start, repeat: true);
				base.Components.Add(m_timer);
			}
			else
			{
				m_timer.SetType(timerType);
				m_timer.IsSessionUpdateEnabled = false;
				if (MyTimerComponentSystem.Static != null)
				{
					MyTimerComponentSystem.Static.Unregister(m_timer);
				}
				m_timer.EventToTrigger = OnTimerTick;
			}
			m_isTimerInRestartMode = start;
		}

		private void OnTimerTick(MyEntityComponentContainer obj)
		{
			if (base.CubeGrid == null || !base.CubeGrid.IsPreview)
			{
				DoUpdateTimerTick();
				this.UpdateTimerTriggered.InvokeIfNotNull(this);
			}
		}

		public uint GetFramesFromLastTrigger()
		{
			return m_timer.FramesFromLastTrigger;
		}

		public void ChangeTimerTick(uint timeTickInFrames)
		{
			m_timer.ChangeTimerTick(timeTickInFrames);
			if (timeTickInFrames == 0)
			{
				m_timer.Pause();
			}
		}

		private void UpdateTimer(bool forceUpdate)
		{
			if (GetTimerEnabledState())
			{
				if (!m_timer.TimerEnabled)
				{
					if (m_timer.TimerTickInFrames != 0)
					{
						m_timer.Resume(m_isTimerInRestartMode);
					}
					else
					{
						m_timer.Pause();
					}
				}
			}
			else if (m_timer.TimerEnabled)
			{
				m_timer.Pause();
			}
			m_timer.Update(forceUpdate);
		}

		public virtual bool GetTimerEnabledState()
		{
			return false;
		}

		public virtual void DoUpdateTimerTick()
		{
		}

		protected uint GetTimerTime(int index)
		{
			if (index < 0 || base.BlockDefinition == null || base.BlockDefinition.TieredUpdateTimes == null || base.BlockDefinition.TieredUpdateTimes.Count <= index)
			{
				return GetDefaultTimeForUpdateTimer(index);
			}
			return base.BlockDefinition.TieredUpdateTimes[index];
		}

		protected virtual uint GetDefaultTimeForUpdateTimer(int index)
		{
			return 0u;
		}

		private Action<MyTerminalBlock> GetDelegate(Action<Sandbox.ModAPI.IMyTerminalBlock> value)
		{
			return (Action<MyTerminalBlock>)Delegate.CreateDelegate(typeof(Action<MyTerminalBlock>), value.Target, value.Method);
		}

		void Sandbox.ModAPI.Ingame.IMyFunctionalBlock.RequestEnable(bool enable)
		{
			Enabled = enable;
		}

		uint Sandbox.ModAPI.IMyFunctionalBlock.GetFramesFromLastTrigger()
		{
			return GetFramesFromLastTrigger();
		}

		private Action<MyFunctionalBlock> GetDelegate(Action<Sandbox.ModAPI.IMyFunctionalBlock> value)
		{
			return (Action<MyFunctionalBlock>)Delegate.CreateDelegate(typeof(Action<MyFunctionalBlock>), value.Target, value.Method);
		}
	}
}
