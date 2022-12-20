using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.Entities.Cube;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;
using VRage.Sync;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_ButtonPanel))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyButtonPanel),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel)
	})]
	public class MyButtonPanel : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyButtonPanel, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider
	{
		protected sealed class ActivateButton_003C_003ESystem_Int32_0023System_Int64 : ICallSite<MyButtonPanel, int, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in int index, in long userId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ActivateButton(index, userId);
			}
		}

		protected sealed class NotifyActivationFailed_003C_003E : ICallSite<MyButtonPanel, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.NotifyActivationFailed();
			}
		}

		protected sealed class SetButtonName_003C_003ESystem_String_0023System_Int32 : ICallSite<MyButtonPanel, string, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in string name, in int position, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetButtonName(name, position);
			}
		}

		protected sealed class SendToolbarItemChanged_003C_003ESandbox_Game_Entities_Blocks_ToolbarItem_0023System_Int32 : ICallSite<MyButtonPanel, ToolbarItem, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in ToolbarItem sentItem, in int index, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SendToolbarItemChanged(sentItem, index);
			}
		}

		protected sealed class OnChangeTextRequest_003C_003ESystem_Int32_0023System_String : ICallSite<MyButtonPanel, int, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in int panelIndex, in string text, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeTextRequest(panelIndex, text);
			}
		}

		protected sealed class OnUpdateSpriteCollection_003C_003ESystem_Int32_0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyButtonPanel, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in int panelIndex, in MySerializableSpriteCollection sprites, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnUpdateSpriteCollection(panelIndex, sprites);
			}
		}

		protected sealed class OnRemoveSelectedImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyButtonPanel, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSelectedImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnSelectImageRequest_003C_003ESystem_Int32_0023System_Int32_003C_0023_003E : ICallSite<MyButtonPanel, int, int[], DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in int panelIndex, in int[] selection, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSelectImageRequest(panelIndex, selection);
			}
		}

		protected sealed class OnChangeOpenSuccess_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyButtonPanel, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeOpenRequest_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyButtonPanel, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
			}
		}

		protected sealed class OnChangeDescription_003C_003ESystem_Int32_0023System_String_0023System_Boolean : ICallSite<MyButtonPanel, int, string, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyButtonPanel @this, in int panelIndex, in string description, in bool isPublic, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeDescription(panelIndex, description, isPublic);
			}
		}

		protected class m_anyoneCanUse_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType anyoneCanUse;
				ISyncType result = (anyoneCanUse = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyButtonPanel)P_0).m_anyoneCanUse = (Sync<bool, SyncDirection.BothWays>)anyoneCanUse;
				return result;
			}
		}

		private const string DETECTOR_NAME = "panel";

		private List<string> m_buttonEmissiveNames;

		private readonly Sync<bool, SyncDirection.BothWays> m_anyoneCanUse;

		private int m_selectedButton = -1;

		private MyHudNotification m_activationFailedNotification = new MyHudNotification(MySpaceTexts.Notification_ActivationFailed, 2500, "Red");

		private static List<MyToolbar> m_openedToolbars;

		private static bool m_shouldSetOtherToolbars;

		private SerializableDictionary<int, string> m_customButtonNames = new SerializableDictionary<int, string>();

		private List<MyUseObjectPanelButton> m_buttonsUseObjects = new List<MyUseObjectPanelButton>();

		private StringBuilder m_emptyName = new StringBuilder("");

		private bool m_syncing;

		private bool m_isTextPanelOpen;

		private MyMultiTextPanelComponent m_multiPanel;

		private MyGuiScreenTextPanel m_textBoxMultiPanel;

		private static StringBuilder m_helperSB = new StringBuilder();

		public MyToolbar Toolbar { get; set; }

		public new MyButtonPanelDefinition BlockDefinition => base.BlockDefinition as MyButtonPanelDefinition;

		public bool AnyoneCanUse
		{
			get
			{
				return m_anyoneCanUse;
			}
			set
			{
				m_anyoneCanUse.Value = value;
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

		int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount
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

		MyMultiTextPanelComponent IMyMultiTextPanelComponentOwner.MultiTextPanel => m_multiPanel;

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

		private event Action<int> ButtonPressed;

		event Action<int> SpaceEngineers.Game.ModAPI.IMyButtonPanel.ButtonPressed
		{
			add
			{
				ButtonPressed += value;
			}
			remove
			{
				ButtonPressed -= value;
			}
		}

		Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(int index)
		{
			if (m_multiPanel == null)
			{
				return null;
			}
			return m_multiPanel.GetSurface(index);
		}

		public MyButtonPanel()
		{
			CreateTerminalControls();
			m_openedToolbars = new List<MyToolbar>();
			base.Render = new MyRenderComponentScreenAreas(this);
		}

		protected override void CreateTerminalControls()
		{
			if (MyTerminalControlFactory.AreControlsCreated<MyButtonPanel>())
<<<<<<< HEAD
			{
				return;
			}
			base.CreateTerminalControls();
			MyTerminalControlCheckbox<MyButtonPanel> obj = new MyTerminalControlCheckbox<MyButtonPanel>("AnyoneCanUse", MySpaceTexts.BlockPropertyText_AnyoneCanUse, MySpaceTexts.BlockPropertyDescription_AnyoneCanUse)
			{
=======
			{
				return;
			}
			base.CreateTerminalControls();
			MyTerminalControlCheckbox<MyButtonPanel> obj = new MyTerminalControlCheckbox<MyButtonPanel>("AnyoneCanUse", MySpaceTexts.BlockPropertyText_AnyoneCanUse, MySpaceTexts.BlockPropertyDescription_AnyoneCanUse)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Getter = (MyButtonPanel x) => x.AnyoneCanUse,
				Setter = delegate(MyButtonPanel x, bool v)
				{
					x.AnyoneCanUse = v;
				}
			};
			obj.EnableAction();
			MyTerminalControlFactory.AddControl(obj);
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyButtonPanel>("Open Toolbar", MySpaceTexts.BlockPropertyTitle_SensorToolbarOpen, MySpaceTexts.BlockPropertyDescription_SensorToolbarOpen, delegate(MyButtonPanel self)
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
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<MyButtonPanel>("ButtonText", MySpaceTexts.BlockPropertyText_ButtonList, MySpaceTexts.Blank)
			{
				ListContent = delegate(MyButtonPanel x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
				{
					x.FillListContent(list1, list2);
				},
				ItemSelected = delegate(MyButtonPanel x, List<MyGuiControlListbox.Item> y)
				{
					x.SelectButtonToName(y);
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MyButtonPanel>("ButtonName", MySpaceTexts.BlockPropertyText_ButtonName, MySpaceTexts.Blank)
			{
				Getter = (MyButtonPanel x) => x.GetButtonName(),
				Setter = delegate(MyButtonPanel x, StringBuilder v)
				{
					x.SetCustomButtonName(v);
				},
				SupportsMultipleBlocks = false
			});
<<<<<<< HEAD
=======
			MyMultiTextPanelComponent.CreateTerminalControls<MyButtonPanel>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, 0.0001f, () => (!Enabled || !base.IsFunctional) ? 0f : 0.0001f, this);
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			myResourceSinkComponent.IsPoweredChanged += ComponentStack_IsFunctionalChanged;
			base.ResourceSink = myResourceSinkComponent;
			base.Init(builder, cubeGrid);
			m_buttonEmissiveNames = new List<string>(BlockDefinition.ButtonCount);
			for (int i = 1; i <= BlockDefinition.ButtonCount; i++)
			{
				m_buttonEmissiveNames.Add($"Emissive{i}");
			}
			MyObjectBuilder_ButtonPanel myObjectBuilder_ButtonPanel = builder as MyObjectBuilder_ButtonPanel;
			Toolbar = new MyToolbar(MyToolbarType.ButtonPanel, Math.Min(BlockDefinition.ButtonCount, 9), BlockDefinition.ButtonCount / 9 + 1);
			Toolbar.DrawNumbers = false;
			Toolbar.GetSymbol = delegate(int slot)
			{
				ColoredIcon result = default(ColoredIcon);
				if (Toolbar.SlotToIndex(slot) < BlockDefinition.ButtonCount)
				{
					result.Icon = BlockDefinition.ButtonSymbols[Toolbar.SlotToIndex(slot) % BlockDefinition.ButtonSymbols.Length];
					Vector4 color = BlockDefinition.ButtonColors[Toolbar.SlotToIndex(slot) % BlockDefinition.ButtonColors.Length];
					color.W = 1f;
					result.Color = color;
				}
				return result;
			};
			Toolbar.Init(myObjectBuilder_ButtonPanel.Toolbar, this);
			Toolbar.ItemChanged += Toolbar_ItemChanged;
			m_anyoneCanUse.SetLocalValue(myObjectBuilder_ButtonPanel.AnyoneCanUse);
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.ResourceSink.Update();
			if (myObjectBuilder_ButtonPanel.CustomButtonNames != null)
			{
				foreach (int key in myObjectBuilder_ButtonPanel.CustomButtonNames.Dictionary.Keys)
				{
					m_customButtonNames.Dictionary.Add(key, MyStatControlText.SubstituteTexts(myObjectBuilder_ButtonPanel.CustomButtonNames[key]));
				}
			}
			if (BlockDefinition.ScreenAreas != null && BlockDefinition.ScreenAreas.Count > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
				m_multiPanel = new MyMultiTextPanelComponent(this, BlockDefinition.ScreenAreas, myObjectBuilder_ButtonPanel.TextPanels);
				m_multiPanel.Init(SendAddImagesToSelectionRequest, SendRemoveSelectedImageRequest, ChangeTextRequest, UpdateSpriteCollection);
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			base.UseObjectsComponent.GetInteractiveObjects(m_buttonsUseObjects);
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
		}

		protected override bool CheckIsWorking()
		{
			if (base.CheckIsWorking())
			{
				return base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);
			}
			return false;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			UpdateEmissivity();
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			UpdateEmissivity();
			UpdateScreen();
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (base.Components.TryGet<MyContainerDropComponent>(out var component))
			{
				component.UpdateSound();
			}
		}

		private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index, bool isGamepad)
		{
			if (m_syncing)
<<<<<<< HEAD
			{
				return;
			}
			ToolbarItem arg = ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex));
			UpdateButtonEmissivity(index.ItemIndex);
			MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.SendToolbarItemChanged, arg, index.ItemIndex);
			if (m_shouldSetOtherToolbars)
			{
=======
			{
				return;
			}
			ToolbarItem arg = ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex));
			UpdateButtonEmissivity(index.ItemIndex);
			MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.SendToolbarItemChanged, arg, index.ItemIndex);
			if (m_shouldSetOtherToolbars)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
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
			MyToolbarItem itemAtIndex = Toolbar.GetItemAtIndex(index.ItemIndex);
			if (itemAtIndex != null)
			{
				string arg2 = itemAtIndex.DisplayName.ToString();
				MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.SetButtonName, arg2, index.ItemIndex);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.SetButtonName, MyTexts.GetString(MySpaceTexts.NotificationHintNoAction), index.ItemIndex);
			}
		}

		private void UpdateEmissivity()
		{
			for (int i = 0; i < BlockDefinition.ButtonCount; i++)
			{
				UpdateButtonEmissivity(i);
			}
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (base.InScene)
			{
				UpdateEmissivity();
			}
			if (m_multiPanel != null)
			{
				m_multiPanel.Reset();
			}
			UpdateScreen();
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity();
			m_buttonsUseObjects.Clear();
			base.UseObjectsComponent.GetInteractiveObjects(m_buttonsUseObjects);
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			UpdateEmissivity();
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			base.ResourceSink.Update();
			UpdateEmissivity();
		}

		private void UpdateButtonEmissivity(int index)
		{
			if (!base.InScene)
			{
				return;
			}
			Vector4 vector = BlockDefinition.ButtonColors[index % BlockDefinition.ButtonColors.Length];
			if (Toolbar.GetItemAtIndex(index) == null)
			{
				vector = BlockDefinition.UnassignedButtonColor;
			}
			float emissivity = vector.W;
			if (!base.IsWorking)
			{
				if (base.IsFunctional)
				{
					vector = Color.Red.ToVector4();
					emissivity = 0f;
				}
				else
				{
					vector = Color.Black.ToVector4();
					emissivity = 0f;
				}
			}
			MyEntity.UpdateNamedEmissiveParts(base.Render.RenderObjectIDs[0], m_buttonEmissiveNames[index], new Color(vector.X, vector.Y, vector.Z), emissivity);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_ButtonPanel myObjectBuilder_ButtonPanel = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_ButtonPanel;
			myObjectBuilder_ButtonPanel.Toolbar = Toolbar.GetObjectBuilder();
			myObjectBuilder_ButtonPanel.AnyoneCanUse = AnyoneCanUse;
			myObjectBuilder_ButtonPanel.CustomButtonNames = m_customButtonNames;
			if (m_multiPanel != null)
			{
				myObjectBuilder_ButtonPanel.TextPanels = m_multiPanel.Serialize();
			}
			return myObjectBuilder_ButtonPanel;
		}

		public void PressButton(int i)
		{
			if (this.ButtonPressed != null)
			{
				this.ButtonPressed(i);
			}
		}

<<<<<<< HEAD
		[Event(null, 332)]
=======
		[Event(null, 379)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access)]
		public void ActivateButton(int index, long userId)
		{
			MyEntities.TryGetEntityById(userId, out MyCharacter entity, allowClosed: false);
			long playerId = entity?.GetPlayerIdentityId() ?? 0;
			if (MyVisualScriptLogicProvider.ButtonPressedTerminalName != null)
			{
				MyVisualScriptLogicProvider.ButtonPressedTerminalName(base.CustomName.ToString(), index, playerId, base.EntityId);
			}
			if (MyVisualScriptLogicProvider.ButtonPressedEntityName != null)
			{
<<<<<<< HEAD
				MyVisualScriptLogicProvider.ButtonPressedEntityName(base.Name, index, playerId, base.EntityId);
=======
				MyVisualScriptLogicProvider.ButtonPressedEntityName(Name, index, playerId, base.EntityId);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			Toolbar.UpdateItem(index);
			bool num = Toolbar.ActivateItemAtIndex(index);
			PressButton(index);
			if (!num)
			{
				MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.NotifyActivationFailed, MyEventContext.Current.Sender);
			}
		}

<<<<<<< HEAD
		[Event(null, 351)]
=======
		[Event(null, 398)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private void NotifyActivationFailed()
		{
			MyHud.Notifications.Add(m_activationFailedNotification);
		}

		protected override void Closing()
		{
			base.Closing();
			foreach (MyUseObjectPanelButton buttonsUseObject in m_buttonsUseObjects)
			{
				buttonsUseObject.RemoveButtonMarker();
			}
			if (m_multiPanel != null)
			{
				m_multiPanel.SetRender(null);
			}
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (m_multiPanel != null)
			{
				UpdateScreen();
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (m_multiPanel != null && m_multiPanel.SurfaceCount > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
			if (m_multiPanel != null)
			{
				m_multiPanel.AddToScene();
			}
		}

		public void FillListContent(ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
		{
			string @string = MyTexts.GetString(MySpaceTexts.BlockPropertyText_Button);
			for (int i = 0; i < m_buttonsUseObjects.Count; i++)
			{
				m_helperSB.Clear().Append(@string + " " + (i + 1));
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(m_helperSB, null, null, i);
				listBoxContent.Add(item);
				if (i == m_selectedButton)
				{
					listBoxSelectedItems.Add(item);
				}
			}
		}

		public void SelectButtonToName(List<MyGuiControlListbox.Item> imageIds)
		{
			if (imageIds.Count > 0)
			{
				m_selectedButton = (int)imageIds[0].UserData;
				RaisePropertiesChanged();
			}
		}

		public StringBuilder GetButtonName()
		{
			if (m_selectedButton == -1)
			{
				return m_emptyName;
			}
			string value = null;
			if (!m_customButtonNames.Dictionary.TryGetValue(m_selectedButton, out value))
			{
				MyToolbarItem itemAtIndex = Toolbar.GetItemAtIndex(m_selectedButton);
				if (itemAtIndex != null)
				{
					return itemAtIndex.DisplayName;
				}
				return m_emptyName;
			}
			return new StringBuilder(value);
		}

<<<<<<< HEAD
		[Event(null, 414)]
=======
		[Event(null, 489)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void SetButtonName(string name, int position)
		{
			string value = null;
			if (name == null)
			{
				m_customButtonNames.Dictionary.Remove(position);
			}
			else if (m_customButtonNames.Dictionary.TryGetValue(position, out value))
			{
				m_customButtonNames.Dictionary[position] = name.ToString();
			}
			else
			{
				m_customButtonNames.Dictionary.Add(position, name.ToString());
			}
		}

		public bool IsButtonAssigned(int pos)
		{
			return Toolbar.GetItemAtIndex(pos) != null;
		}

		public bool HasCustomButtonName(int pos)
		{
			return m_customButtonNames.Dictionary.ContainsKey(pos);
		}

		public void SetCustomButtonName(string name, int pos)
		{
			MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.SetButtonName, name, pos);
		}

		public void SetCustomButtonName(StringBuilder name)
		{
			if (m_selectedButton != -1)
			{
				MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.SetButtonName, name.ToString(), m_selectedButton);
			}
		}

		public string GetCustomButtonName(int pos)
		{
			string value = null;
			if (!m_customButtonNames.Dictionary.TryGetValue(pos, out value))
			{
				MyToolbarItem itemAtIndex = Toolbar.GetItemAtIndex(pos);
				if (itemAtIndex != null)
				{
					return itemAtIndex.DisplayName.ToString();
				}
				return MyTexts.GetString(MySpaceTexts.NotificationHintNoAction);
			}
			return value;
		}

<<<<<<< HEAD
		[Event(null, 471)]
=======
		[Event(null, 546)]
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
			UpdateButtonEmissivity(index);
			m_syncing = false;
		}

		string SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.GetButtonName(int index)
		{
			return GetCustomButtonName(index);
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.SetCustomButtonName(int index, string name)
		{
			SetCustomButtonName(name, index);
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.ClearCustomButtonName(int index)
		{
			SetCustomButtonName(null, index);
		}

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.HasCustomButtonName(int index)
		{
			return HasCustomButtonName(index);
		}

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel.IsButtonAssigned(int index)
		{
			return IsButtonAssigned(index);
		}

		public void UpdateScreen()
		{
			m_multiPanel?.UpdateScreen(base.IsWorking);
		}

		private void ChangeTextRequest(int panelIndex, string text)
		{
			MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.OnChangeTextRequest, panelIndex, text);
		}

		[Event(null, 605)]
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
				MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.OnUpdateSpriteCollection, panelIndex, sprites);
			}
		}

		[Event(null, 621)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		[DistanceRadius(32f)]
		private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);
		}

		private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.OnSelectImageRequest, panelIndex, selection);
		}

		private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.OnRemoveSelectedImageRequest, panelIndex, selection);
		}

		[Event(null, 637)]
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

		[Event(null, 648)]
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

		void IMyMultiTextPanelComponentOwner.SelectPanel(List<MyGuiControlListbox.Item> panelItems)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectPanel((int)panelItems[0].UserData);
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

		[Event(null, 736)]
		[Reliable]
		[Broadcast]
		private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			OnChangeOpen(isOpen, editable, user, isPublic);
		}

		private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0uL, bool isPublic = false)
		{
			MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.OnChangeOpenRequest, isOpen, editable, user, isPublic);
		}

		[Event(null, 747)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			if (!(Sync.IsServer && IsTextPanelOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user, isPublic);
				MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.OnChangeOpenSuccess, isOpen, editable, user, isPublic);
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
						SendChangeDescriptionMessage(m_textBoxMultiPanel.Description.Text, isPublic);
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

		private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
		{
			if (base.CubeGrid.IsPreview || !base.CubeGrid.SyncFlag)
			{
				PanelComponent.Text = description;
			}
			else if (description.CompareTo(PanelComponent.Text) != 0)
			{
				MyMultiplayer.RaiseEvent(this, (MyButtonPanel x) => x.OnChangeDescription, m_multiPanel.SelectedPanelIndex, description.ToString(), isPublic);
			}
		}

		[Event(null, 805)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void OnChangeDescription(int panelIndex, string description, bool isPublic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Clear().Append(description);
			m_multiPanel.GetPanelComponent(panelIndex).Text = stringBuilder;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}
	}
}
