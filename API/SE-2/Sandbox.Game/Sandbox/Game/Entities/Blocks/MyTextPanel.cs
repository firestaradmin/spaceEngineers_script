using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.Entity.UseObject;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_TextPanel))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyTextPanel),
		typeof(Sandbox.ModAPI.Ingame.IMyTextPanel)
	})]
	public class MyTextPanel : MyFunctionalBlock, IMyTextPanelComponentOwner, IMyTextPanelProvider, Sandbox.ModAPI.IMyTextPanel, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.IMyTextSurface, Sandbox.ModAPI.Ingame.IMyTextSurface, Sandbox.ModAPI.Ingame.IMyTextPanel, Sandbox.ModAPI.IMyTextSurfaceProvider, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider
	{
		protected sealed class SetSelectedRotationIndex_003C_003ESystem_Int32 : ICallSite<MyTextPanel, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in int newIndex, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SetSelectedRotationIndex(newIndex);
			}
		}

		protected sealed class OnRemoveSelectedImageRequest_003C_003ESystem_Int32_003C_0023_003E : ICallSite<MyTextPanel, int[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in int[] selection, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnRemoveSelectedImageRequest(selection);
			}
		}

		protected sealed class OnSelectImageRequest_003C_003ESystem_Int32_003C_0023_003E : ICallSite<MyTextPanel, int[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in int[] selection, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSelectImageRequest(selection);
			}
		}

		protected sealed class OnUpdateSpriteCollection_003C_003EVRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyTextPanel, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in MySerializableSpriteCollection sprites, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnUpdateSpriteCollection(sprites);
			}
		}

		protected new sealed class OnChangeDescription_003C_003ESystem_String_0023System_Boolean : ICallSite<MyTextPanel, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in string description, in bool isPublic, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeDescription(description, isPublic);
			}
		}

		protected sealed class OnChangeTitle_003C_003ESystem_String_0023System_Boolean : ICallSite<MyTextPanel, string, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in string title, in bool isPublic, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeTitle(title, isPublic);
			}
		}

		protected new sealed class OnChangeOpenRequest_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyTextPanel, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
			}
		}

		protected new sealed class OnChangeOpenSuccess_003C_003ESystem_Boolean_0023System_Boolean_0023System_UInt64_0023System_Boolean : ICallSite<MyTextPanel, bool, bool, ulong, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTextPanel @this, in bool isOpen, in bool editable, in ulong user, in bool isPublic, in DBNull arg5, in DBNull arg6)
			{
				@this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
			}
		}

		private class Sandbox_Game_Entities_Blocks_MyTextPanel_003C_003EActor : IActivator, IActivator<MyTextPanel>
		{
			private sealed override object CreateInstance()
			{
				return new MyTextPanel();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTextPanel CreateInstance()
			{
				return new MyTextPanel();
			}

			MyTextPanel IActivator<MyTextPanel>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public const double MAX_DRAW_DISTANCE = 200.0;

		private readonly StringBuilder m_publicDescription = new StringBuilder();

		private readonly StringBuilder m_publicTitle = new StringBuilder();

		private readonly StringBuilder m_privateDescription = new StringBuilder();

		private readonly StringBuilder m_privateTitle = new StringBuilder();

		private bool m_isTextPanelOpen;

		private ulong m_userId;

		private MyGuiScreenTextPanel m_textBox;

		protected MySessionComponentPanels m_panelsComponent;
<<<<<<< HEAD
=======

		private List<MyTextPanelComponent> m_panelComponents = new List<MyTextPanelComponent>();

		private int m_selectedRotationIndex;

		private int m_newSelectedRotationIndex;

		private int m_previousUpdateTime;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private List<MyTextPanelComponent> m_panelComponents = new List<MyTextPanelComponent>();

<<<<<<< HEAD
		private int m_selectedRotationIndex;

		private int m_newSelectedRotationIndex;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected MyTextPanelComponent m_activePanelComponent;

		private bool m_isEditingPublic;

		private float m_maxRenderDistanceSquared;

		private StringBuilder m_publicTitleHelper = new StringBuilder();

		private StringBuilder m_privateTitleHelper = new StringBuilder();

		private StringBuilder m_publicDescriptionHelper = new StringBuilder();

		private StringBuilder m_privateDescriptionHelper = new StringBuilder();

		private bool m_descriptionPrivateDirty;

		private bool m_descriptionPublicDirty;

<<<<<<< HEAD
		public override bool UseGenericLcd => false;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public int PanelTexturesByteCount => m_activePanelComponent.TextureByteCount;

		public Vector3D WorldPosition => base.PositionComp.WorldMatrixRef.Translation;

		public int RangeIndex { get; set; }

		public ContentType ContentType
		{
			get
			{
				return PanelComponent.ContentType;
			}
			set
			{
				PanelComponent.ContentType = value;
			}
		}

		public ShowTextOnScreenFlag ShowTextFlag
		{
			get
			{
				return PanelComponent.ShowTextFlag;
			}
			set
			{
				PanelComponent.ShowTextFlag = value;
			}
		}

		public bool ShowTextOnScreen => PanelComponent.ShowTextOnScreen;

<<<<<<< HEAD
		public new MyTextPanelComponent PanelComponent => m_activePanelComponent;
=======
		public MyTextPanelComponent PanelComponent => m_activePanelComponent;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public StringBuilder PublicDescription
		{
			get
			{
				return m_publicDescription;
			}
			set
			{
				if (m_publicDescription.CompareUpdate(value))
				{
					base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
					m_activePanelComponent.Text = m_publicDescription;
				}
				if (m_publicDescriptionHelper != value)
				{
					m_publicDescriptionHelper.Clear().Append((object)value);
				}
			}
		}

		public StringBuilder PublicTitle
		{
			get
			{
				return m_publicTitle;
			}
			set
			{
				m_publicTitle.CompareUpdate(value);
				if (m_publicTitleHelper != value)
				{
					m_publicTitleHelper.Clear().Append((object)value);
				}
			}
		}

		public StringBuilder PrivateTitle
		{
			get
			{
				return m_privateTitle;
			}
			set
			{
				m_privateTitle.CompareUpdate(value);
				if (m_privateTitleHelper != value)
				{
					m_privateTitleHelper.Clear().Append((object)value);
				}
			}
		}

		public StringBuilder PrivateDescription
		{
			get
			{
				return m_privateDescription;
			}
			set
			{
				if (m_privateDescription.CompareUpdate(value))
				{
					base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
				if (m_privateDescriptionHelper != value)
				{
					m_privateDescriptionHelper.Clear().Append((object)value);
				}
			}
		}

		public new bool IsTextPanelOpen
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

		public ulong UserId
		{
			get
			{
				return m_userId;
			}
			set
			{
				m_userId = value;
			}
		}

		public Vector2 SurfaceSize => m_activePanelComponent.SurfaceSize;

		public Vector2 TextureSize => m_activePanelComponent.TextureSize;

		internal new MyRenderComponentScreenAreas Render
		{
			get
			{
				return base.Render as MyRenderComponentScreenAreas;
			}
			set
			{
				base.Render = value;
			}
		}

		public new MyTextPanelDefinition BlockDefinition => (MyTextPanelDefinition)base.BlockDefinition;

		public int SelectedRotationIndex
		{
			get
			{
				return m_selectedRotationIndex;
			}
			set
			{
				SetSelectedRotationIndex(value);
			}
		}

		public float FontSize
		{
			get
			{
				return m_activePanelComponent.FontSize;
			}
			set
			{
				m_activePanelComponent.FontSize = (float)Math.Round(value, 3);
			}
		}

		public Color FontColor
		{
			get
			{
				return m_activePanelComponent.FontColor;
			}
			set
			{
				m_activePanelComponent.FontColor = value;
			}
		}

		public Color BackgroundColor
		{
			get
			{
				return m_activePanelComponent.BackgroundColor;
			}
			set
			{
				m_activePanelComponent.BackgroundColor = value;
			}
		}

		public byte BackgroundAlpha
		{
			get
			{
				return m_activePanelComponent.BackgroundAlpha;
			}
			set
			{
				m_activePanelComponent.BackgroundAlpha = value;
			}
		}

		public float ChangeInterval
		{
			get
			{
				return m_activePanelComponent.ChangeInterval;
			}
			set
			{
				m_activePanelComponent.ChangeInterval = (float)Math.Round(value, 3);
			}
		}

		ShowTextOnScreenFlag Sandbox.ModAPI.Ingame.IMyTextPanel.ShowOnScreen => ShowTextFlag;

		bool Sandbox.ModAPI.Ingame.IMyTextPanel.ShowText => ShowTextOnScreen;

		string Sandbox.ModAPI.Ingame.IMyTextSurface.CurrentlyShownImage
		{
			get
			{
				if (PanelComponent.SelectedTexturesToDraw.Count == 0)
				{
					return null;
				}
				if (PanelComponent.CurrentSelectedTexture >= PanelComponent.SelectedTexturesToDraw.Count)
				{
					return PanelComponent.SelectedTexturesToDraw[0].Id.SubtypeName;
				}
				return PanelComponent.SelectedTexturesToDraw[PanelComponent.CurrentSelectedTexture].Id.SubtypeName;
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.Font
		{
			get
			{
				return PanelComponent.Font.SubtypeName;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(value) != null)
				{
					PanelComponent.Font = MyDefinitionManager.Static.GetDefinition<MyFontDefinition>(value).Id;
				}
			}
		}

		TextAlignment Sandbox.ModAPI.Ingame.IMyTextSurface.Alignment
		{
			get
			{
				if (m_activePanelComponent == null)
				{
					return TextAlignment.LEFT;
				}
				return m_activePanelComponent.Alignment;
			}
			set
			{
				if (m_activePanelComponent != null)
				{
					m_activePanelComponent.Alignment = value;
				}
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.Script
		{
			get
			{
				if (m_activePanelComponent == null)
				{
					return string.Empty;
				}
				return m_activePanelComponent.Script;
			}
			set
			{
				if (m_activePanelComponent != null)
				{
					m_activePanelComponent.Script = value;
				}
			}
		}

		ContentType Sandbox.ModAPI.Ingame.IMyTextSurface.ContentType
		{
			get
			{
				return ContentType;
			}
			set
			{
				ContentType = value;
			}
		}

		Vector2 Sandbox.ModAPI.Ingame.IMyTextSurface.SurfaceSize => SurfaceSize;

		Vector2 Sandbox.ModAPI.Ingame.IMyTextSurface.TextureSize => TextureSize;

		bool Sandbox.ModAPI.Ingame.IMyTextSurface.PreserveAspectRatio
		{
			get
			{
				if (m_activePanelComponent == null)
				{
					return false;
				}
				return m_activePanelComponent.PreserveAspectRatio;
			}
			set
			{
				if (m_activePanelComponent != null)
				{
					m_activePanelComponent.PreserveAspectRatio = value;
				}
			}
		}

		float Sandbox.ModAPI.Ingame.IMyTextSurface.TextPadding
		{
			get
			{
				if (m_activePanelComponent == null)
				{
					return 0f;
				}
				return m_activePanelComponent.TextPadding;
			}
			set
			{
				if (m_activePanelComponent != null)
				{
					m_activePanelComponent.TextPadding = value;
				}
			}
		}

		Color Sandbox.ModAPI.Ingame.IMyTextSurface.ScriptBackgroundColor
		{
			get
			{
				if (m_activePanelComponent == null)
				{
					return Color.White;
				}
				return m_activePanelComponent.ScriptBackgroundColor;
			}
			set
			{
				if (m_activePanelComponent != null)
				{
					m_activePanelComponent.ScriptBackgroundColor = value;
				}
			}
		}

		Color Sandbox.ModAPI.Ingame.IMyTextSurface.ScriptForegroundColor
		{
			get
			{
				if (m_activePanelComponent == null)
				{
					return Color.White;
				}
				return m_activePanelComponent.ScriptForegroundColor;
			}
			set
			{
				if (m_activePanelComponent != null)
				{
					m_activePanelComponent.ScriptForegroundColor = value;
				}
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.Name
		{
			get
			{
				if (m_activePanelComponent == null)
				{
					return null;
				}
				return m_activePanelComponent.Name;
			}
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.DisplayName
		{
			get
			{
				if (m_activePanelComponent == null)
				{
					return null;
				}
				return m_activePanelComponent.DisplayName;
			}
		}

		int Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.SurfaceCount => 1;

		public MyTextPanel()
		{
			CreateTerminalControls();
			m_isTextPanelOpen = false;
			m_privateDescription = new StringBuilder();
			m_privateTitle = new StringBuilder();
			Render = new MyRenderComponentTextPanel(this);
			Render.NeedsDraw = false;
			base.NeedsWorldMatrix = true;
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (base.IsFunctional)
			{
				m_activePanelComponent.UpdateAfterSimulation(base.IsWorking, IsInRange());
			}
			m_activePanelComponent?.UpdateModApiText();
			UpdateModApiText();
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (IsBeingHacked)
			{
				m_descriptionPrivateDirty = false;
				PrivateDescription.Clear();
				SendChangeDescriptionMessage(PrivateDescription, isPublic: false);
			}
			base.ResourceSink.Update();
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			SetDetailedInfoDirty();
			UpdateIsWorking();
			if (Render != null)
			{
				UpdateScreen();
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

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStartWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override void OnStopWorking()
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			ComponentStack_IsFunctionalChanged();
			PanelComponent.Reset();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			MyCubeGridRenderCell orAddCell = base.CubeGrid.RenderData.GetOrAddCell(base.Position * base.CubeGrid.GridSize);
			if (orAddCell.ParentCullObject != uint.MaxValue)
			{
				Render.SetParent(0, orAddCell.ParentCullObject, base.PositionComp.LocalMatrixRef);
			}
			PanelComponent.SetRender(Render);
			if (m_newSelectedRotationIndex != m_selectedRotationIndex)
			{
				SetSelectedRotationIndex(m_newSelectedRotationIndex);
			}
			HideInactivePanelComponents();
			UpdateScreen();
		}

		protected override void CreateTerminalControls()
		{
			if (MyTerminalControlFactory.AreControlsCreated<MyTextPanel>())
			{
				return;
			}
			base.CreateTerminalControls();
			MyTerminalControlFactory.AddControl(new MyTerminalControlTextbox<MyTextPanel>("Title", MySpaceTexts.BlockPropertyTitle_TextPanelPublicTitle, MySpaceTexts.Blank)
			{
				Getter = (MyTextPanel x) => x.PublicTitle,
				Setter = delegate(MyTextPanel x, StringBuilder v)
				{
					x.SendChangeTitleMessage(v, isPublic: true);
				},
				SupportsMultipleBlocks = false
			});
			MyTerminalControlSlider<MyTextPanel> myTerminalControlSlider = new MyTerminalControlSlider<MyTextPanel>("Rotate", MyCommonTexts.ScriptingTools_Rotation, MyCommonTexts.ScriptingTools_Rotation);
			myTerminalControlSlider.SetLimits((MyTextPanel block) => 0f, (MyTextPanel block) => 270f);
			myTerminalControlSlider.DefaultValue = 0f;
			myTerminalControlSlider.Getter = (MyTextPanel x) => x.m_selectedRotationIndex * 90;
			myTerminalControlSlider.Setter = delegate(MyTextPanel x, float v)
			{
				int num = Convert.ToInt32(v / 90f);
				if (num != x.m_selectedRotationIndex)
				{
					x.SendSelectRotationIndexRequest(num);
				}
			};
			myTerminalControlSlider.Writer = delegate(MyTextPanel x, StringBuilder result)
			{
				result.AppendInt32(x.m_selectedRotationIndex * 90).Append("°");
			};
			myTerminalControlSlider.EnableActions(0.25f);
			myTerminalControlSlider.Visible = (MyTextPanel x) => x.m_panelComponents.Count == 4;
			MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyTextPanel>());
			MyTextPanelComponent.CreateTerminalControls<MyTextPanel>();
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, () => (!Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), this);
			base.ResourceSink = myResourceSinkComponent;
			m_panelsComponent = MySession.Static.GetComponent<MySessionComponentPanels>();
			base.OnClosing += delegate
			{
				m_panelsComponent.Remove(this);
			};
			RangeIndex = -1;
			m_maxRenderDistanceSquared = BlockDefinition.MaxScreenRenderDistance * BlockDefinition.MaxScreenRenderDistance;
<<<<<<< HEAD
			MyObjectBuilder_TextPanel myObjectBuilder_TextPanel = objectBuilder as MyObjectBuilder_TextPanel;
=======
			MyObjectBuilder_TextPanel myObjectBuilder_TextPanel = (MyObjectBuilder_TextPanel)objectBuilder;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (BlockDefinition.ScreenAreas != null && BlockDefinition.ScreenAreas.Count == 4)
			{
				for (int i = 0; i < BlockDefinition.ScreenAreas.Count; i++)
				{
<<<<<<< HEAD
					MyTextPanelComponent myTextPanelComponent = new MyTextPanelComponent(i, this, BlockDefinition.ScreenAreas[i].Name, BlockDefinition.ScreenAreas[i].DisplayName, BlockDefinition.ScreenAreas[i].TextureResolution, BlockDefinition.ScreenAreas.Count, BlockDefinition.ScreenAreas[i].ScreenWidth, BlockDefinition.ScreenAreas[i].ScreenHeight);
=======
					MyTextPanelComponent myTextPanelComponent = new MyTextPanelComponent(i, this, BlockDefinition.ScreenAreas[i].Name, BlockDefinition.ScreenAreas[i].DisplayName, BlockDefinition.ScreenAreas[i].TextureResolution, BlockDefinition.ScreenAreas[i].ScreenWidth, BlockDefinition.ScreenAreas[i].ScreenHeight);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					base.SyncType.Append(myTextPanelComponent);
					myTextPanelComponent.Init(myObjectBuilder_TextPanel?.Sprites ?? default(MySerializableSpriteCollection), null, SendAddImagesToSelectionRequest, SendRemoveSelectedImageRequest, ChangeTextRequest, UpdateSpriteCollection);
					m_panelComponents.Add(myTextPanelComponent);
				}
				m_activePanelComponent = m_panelComponents[0];
				base.SyncType.Append(m_panelComponents);
<<<<<<< HEAD
				if (myObjectBuilder_TextPanel != null && myObjectBuilder_TextPanel.SelectedRotationIndex.HasValue && myObjectBuilder_TextPanel.SelectedRotationIndex.Value > 0 && myObjectBuilder_TextPanel.SelectedRotationIndex.Value < m_panelComponents.Count)
=======
				if (myObjectBuilder_TextPanel.SelectedRotationIndex.HasValue && myObjectBuilder_TextPanel.SelectedRotationIndex.Value > 0 && myObjectBuilder_TextPanel.SelectedRotationIndex.Value < m_panelComponents.Count)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					m_selectedRotationIndex = (m_newSelectedRotationIndex = myObjectBuilder_TextPanel.SelectedRotationIndex.Value);
					m_activePanelComponent = m_panelComponents[m_selectedRotationIndex];
				}
			}
			else
			{
<<<<<<< HEAD
				m_activePanelComponent = new MyTextPanelComponent(0, this, BlockDefinition.PanelMaterialName, BlockDefinition.PanelMaterialName, BlockDefinition.TextureResolution, BlockDefinition.ScreenAreas.Count, BlockDefinition.ScreenWidth, BlockDefinition.ScreenHeight);
=======
				m_activePanelComponent = new MyTextPanelComponent(0, this, BlockDefinition.PanelMaterialName, BlockDefinition.PanelMaterialName, BlockDefinition.TextureResolution, BlockDefinition.ScreenWidth, BlockDefinition.ScreenHeight);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				base.SyncType.Append(m_activePanelComponent);
				m_activePanelComponent.Init(myObjectBuilder_TextPanel?.Sprites ?? default(MySerializableSpriteCollection), null, SendAddImagesToSelectionRequest, SendRemoveSelectedImageRequest, ChangeTextRequest, UpdateSpriteCollection);
				m_panelComponents.Add(m_activePanelComponent);
			}
			if (myObjectBuilder_TextPanel != null)
			{
				InitTextPanelComponent(m_activePanelComponent, myObjectBuilder_TextPanel);
			}
			base.Init(objectBuilder, cubeGrid);
			base.ResourceSink.Update();
			base.ResourceSink.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
			base.ResourceSink.RequiredInputChanged += PowerReceiver_RequiredInputChanged;
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
		}

		protected void InitTextPanelComponent(MyTextPanelComponent component, MyObjectBuilder_TextPanel ob)
		{
			if (ob == null)
			{
				return;
			}
			PrivateTitle.Append(ob.Title);
			PrivateDescription.Append(ob.Description);
			PublicDescription.Append(MyStatControlText.SubstituteTexts(ob.PublicDescription));
			PublicTitle.Append(ob.PublicTitle);
			if (Sync.IsServer && Sync.Clients != null)
			{
				Sync.Clients.ClientRemoved += TextPanel_ClientRemoved;
			}
			component.CurrentSelectedTexture = ob.CurrentShownTexture;
			MyTextPanelComponent.ContentMetadata contentMetadata = default(MyTextPanelComponent.ContentMetadata);
			contentMetadata.ContentType = ob.ContentType;
			contentMetadata.BackgroundColor = ob.BackgroundColor;
			contentMetadata.ChangeInterval = MathHelper.Clamp(ob.ChangeInterval, 0f, BlockDefinition.MaxChangingSpeed);
			contentMetadata.PreserveAspectRatio = ob.PreserveAspectRatio;
			contentMetadata.TextPadding = ob.TextPadding;
			MyTextPanelComponent.ContentMetadata content = contentMetadata;
			MyTextPanelComponent.FontData fontData = default(MyTextPanelComponent.FontData);
			fontData.Alignment = (TextAlignment)ob.Alignment;
			fontData.Size = MathHelper.Clamp(ob.FontSize, BlockDefinition.MinFontSize, BlockDefinition.MaxFontSize);
			fontData.TextColor = ob.FontColor;
			MyTextPanelComponent.FontData font = fontData;
			MyTextPanelComponent.ScriptData scriptData = default(MyTextPanelComponent.ScriptData);
			scriptData.Script = ob.SelectedScript ?? string.Empty;
			scriptData.CustomizeScript = ob.CustomizeScripts;
			scriptData.BackgroundColor = ob.ScriptBackgroundColor;
			scriptData.ForegroundColor = ob.ScriptForegroundColor;
			MyTextPanelComponent.ScriptData script = scriptData;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			Render.NeedsDrawFromParent = true;
			if (!ob.Font.IsNull())
			{
				font.Name = ob.Font.SubtypeName;
			}
			if (ob.SelectedImages != null)
			{
				foreach (string selectedImage in ob.SelectedImages)
				{
					foreach (MyLCDTextureDefinition definition in component.Definitions)
					{
						if (definition.Id.SubtypeName == selectedImage)
						{
							component.SelectedTexturesToDraw.Add(definition);
							break;
						}
					}
				}
				component.CurrentSelectedTexture = Math.Min(component.CurrentSelectedTexture, component.SelectedTexturesToDraw.Count);
				RaisePropertiesChanged();
			}
			if (ob.Version == 0)
			{
				if (ob.ContentType == ContentType.NONE && ((ob.SelectedImages != null && ob.SelectedImages.Count > 0) || ob.ShowText != 0 || ob.PublicDescription != string.Empty))
				{
					if (ob.ShowText != 0)
					{
						component.SelectedTexturesToDraw.Clear();
					}
					else
					{
						PublicDescription.Clear();
					}
					content.ContentType = ContentType.TEXT_AND_IMAGE;
				}
				else if (ob.ContentType == ContentType.IMAGE)
				{
					content.ContentType = ContentType.TEXT_AND_IMAGE;
				}
				else
				{
					content.ContentType = ob.ContentType;
				}
			}
			component.SetLocalValues(content, font, script);
			component.Text = PublicDescription;
		}

		private void PowerReceiver_RequiredInputChanged(MyDefinitionId resourceTypeId, MyResourceSinkComponent receiver, float oldRequirement, float newRequirement)
		{
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_TextPanel myObjectBuilder_TextPanel = (MyObjectBuilder_TextPanel)base.GetObjectBuilderCubeBlock(copy);
			myObjectBuilder_TextPanel.Description = m_privateDescription.ToString();
			myObjectBuilder_TextPanel.Title = m_privateTitle.ToString();
			myObjectBuilder_TextPanel.PublicDescription = m_publicDescription.ToString();
			myObjectBuilder_TextPanel.PublicTitle = m_publicTitle.ToString();
			myObjectBuilder_TextPanel.ChangeInterval = ChangeInterval;
			myObjectBuilder_TextPanel.Font = PanelComponent.Font;
			myObjectBuilder_TextPanel.FontSize = FontSize;
			myObjectBuilder_TextPanel.FontColor = FontColor;
			myObjectBuilder_TextPanel.BackgroundColor = BackgroundColor;
			myObjectBuilder_TextPanel.CurrentShownTexture = PanelComponent.CurrentSelectedTexture;
			myObjectBuilder_TextPanel.ShowText = ShowTextOnScreenFlag.NONE;
			myObjectBuilder_TextPanel.Alignment = (TextAlignmentEnum)PanelComponent.Alignment;
			myObjectBuilder_TextPanel.ContentType = ((PanelComponent.ContentType == ContentType.IMAGE) ? ContentType.TEXT_AND_IMAGE : PanelComponent.ContentType);
			myObjectBuilder_TextPanel.SelectedScript = PanelComponent.Script;
			myObjectBuilder_TextPanel.CustomizeScripts = PanelComponent.CustomizeScripts;
			myObjectBuilder_TextPanel.ScriptBackgroundColor = PanelComponent.ScriptBackgroundColor;
			myObjectBuilder_TextPanel.ScriptForegroundColor = PanelComponent.ScriptForegroundColor;
			myObjectBuilder_TextPanel.TextPadding = PanelComponent.TextPadding;
			myObjectBuilder_TextPanel.PreserveAspectRatio = PanelComponent.PreserveAspectRatio;
			myObjectBuilder_TextPanel.Version = 1;
			if (PanelComponent.SelectedTexturesToDraw.Count > 0)
			{
				myObjectBuilder_TextPanel.SelectedImages = new List<string>();
				foreach (MyLCDTextureDefinition item in PanelComponent.SelectedTexturesToDraw)
				{
					myObjectBuilder_TextPanel.SelectedImages.Add(item.Id.SubtypeName);
				}
			}
			myObjectBuilder_TextPanel.Sprites = PanelComponent.ExternalSprites;
			myObjectBuilder_TextPanel.SelectedRotationIndex = m_selectedRotationIndex;
			return myObjectBuilder_TextPanel;
		}

		public void Use(UseActionEnum actionEnum, VRage.ModAPI.IMyEntity entity)
		{
			if (m_isTextPanelOpen)
			{
				return;
			}
			MyCharacter myCharacter = entity as MyCharacter;
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = GetUserRelationToOwner(myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId);
			if (base.OwnerId == 0L || MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
			{
				OnOwnerUse(actionEnum, myCharacter);
				return;
			}
			switch (userRelationToOwner)
			{
			case MyRelationsBetweenPlayerAndBlock.Neutral:
			case MyRelationsBetweenPlayerAndBlock.Enemies:
			case MyRelationsBetweenPlayerAndBlock.Friends:
				if (MySession.Static.Factions.TryGetPlayerFaction(myCharacter.ControllerInfo.Controller.Player.Identity.IdentityId) == MySession.Static.Factions.TryGetPlayerFaction(base.IDModule.Owner) && actionEnum == UseActionEnum.Manipulate)
				{
					OnFactionUse(actionEnum, myCharacter);
				}
				else
				{
					OnEnemyUse(actionEnum, myCharacter);
				}
				break;
			case MyRelationsBetweenPlayerAndBlock.NoOwnership:
			case MyRelationsBetweenPlayerAndBlock.FactionShare:
				OnFactionUse(actionEnum, myCharacter);
				break;
			case MyRelationsBetweenPlayerAndBlock.Owner:
				OnOwnerUse(actionEnum, myCharacter);
				break;
			}
		}

		private void OnEnemyUse(UseActionEnum actionEnum, MyCharacter user)
		{
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				OpenWindow(isEditable: false, sync: true, isPublic: true);
				break;
			case UseActionEnum.OpenTerminal:
				MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
				break;
			}
		}

		private void OnFactionUse(UseActionEnum actionEnum, MyCharacter user)
		{
			bool flag = false;
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = GetUserRelationToOwner(user.GetPlayerIdentityId());
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				if (userRelationToOwner == MyRelationsBetweenPlayerAndBlock.FactionShare)
				{
					OpenWindow(isEditable: true, sync: true, isPublic: true);
				}
				else
				{
					OpenWindow(isEditable: false, sync: true, isPublic: true);
				}
				break;
			case UseActionEnum.OpenTerminal:
				if (userRelationToOwner == MyRelationsBetweenPlayerAndBlock.FactionShare)
				{
					MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, this);
				}
				else
				{
					flag = true;
				}
				break;
			}
			if (user.ControllerInfo.Controller.Player == MySession.Static.LocalHumanPlayer && flag)
			{
				MyHud.Notifications.Add(MyNotificationSingletons.TextPanelReadOnly);
			}
		}

		private void OnOwnerUse(UseActionEnum actionEnum, MyCharacter user)
		{
			switch (actionEnum)
			{
			case UseActionEnum.Manipulate:
				OpenWindow(isEditable: true, sync: true, isPublic: true);
				break;
			case UseActionEnum.OpenTerminal:
				MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, this);
				break;
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (PanelComponent != null)
			{
				PanelComponent.SetRender(null);
			}
			if (m_panelComponents == null)
			{
				return;
			}
			foreach (MyTextPanelComponent panelComponent in m_panelComponents)
			{
				panelComponent?.SetRender(null);
			}
		}

		protected override void Closing()
		{
			base.Closing();
			if (Sync.IsServer && Sync.Clients != null)
			{
				Sync.Clients.ClientRemoved -= TextPanel_ClientRemoved;
			}
		}

		private void TextPanel_ClientRemoved(ulong playerId)
		{
			if (playerId == m_userId)
			{
				SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
			}
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
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0f, detailedInfo);
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (m_activePanelComponent != null)
			{
				m_activePanelComponent.Reset();
			}
			if (base.ResourceSink != null)
			{
				UpdateScreen();
			}
			if (CheckIsWorking() && ShowTextOnScreen)
			{
				Render.UpdateModelProperties();
			}
			HideInactivePanelComponents();
		}

		public new void OpenWindow(bool isEditable, bool sync, bool isPublic)
		{
			if (sync)
			{
				SendChangeOpenMessage(isOpen: true, isEditable, Sync.MyId, isPublic);
				return;
			}
			m_isEditingPublic = isPublic;
			CreateTextBox(isEditable, isPublic ? PublicDescription : PrivateDescription, isPublic);
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
			MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_textBox);
		}

		private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
		{
			string missionTitle = (isPublic ? m_publicTitle.ToString() : m_privateTitle.ToString());
			string description2 = description.ToString();
			bool editable = isEditable;
			m_textBox = new MyGuiScreenTextPanel(missionTitle, "", "", description2, OnClosedPanelTextBox, null, null, editable);
		}

		public new void OnClosedPanelTextBox(ResultEnum result)
		{
			if (m_textBox != null)
			{
				if (m_textBox.Description.Text.Length > 100000)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: OnClosedPanelMessageBox, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText)));
				}
				else
				{
					CloseWindow(m_isEditingPublic);
				}
			}
		}

		public new void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				m_textBox.Description.Text.Remove(100000, m_textBox.Description.Text.Length - 100000);
				CloseWindow(m_isEditingPublic);
			}
			else
			{
				CreateTextBox(isEditable: true, m_textBox.Description.Text, m_isEditingPublic);
				MyScreenManager.AddScreen(m_textBox);
			}
		}

		private void CloseWindow(bool isPublic)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = null;
			MySession.Static.Gpss.ScanText(m_textBox.Description.Text.ToString(), PublicTitle);
			Enumerator<MySlimBlock> enumerator = base.CubeGrid.CubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
<<<<<<< HEAD
					if (isPublic)
					{
						m_descriptionPublicDirty = false;
					}
					else
					{
						m_descriptionPrivateDirty = false;
					}
					SendChangeDescriptionMessage(m_textBox.Description.Text, isPublic);
					SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
					break;
=======
					MySlimBlock current = enumerator.get_Current();
					if (current.FatBlock != null && current.FatBlock.EntityId == base.EntityId)
					{
						if (isPublic)
						{
							m_descriptionPublicDirty = false;
						}
						else
						{
							m_descriptionPrivateDirty = false;
						}
						SendChangeDescriptionMessage(m_textBox.Description.Text, isPublic);
						SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
						break;
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public new void UpdateScreen()
		{
			if (m_activePanelComponent != null)
			{
				m_activePanelComponent.UpdateAfterSimulation(CheckIsWorking(), IsInRange());
			}
		}

		private bool IsInRange()
		{
<<<<<<< HEAD
=======
			if (m_activePanelComponent != null)
			{
				m_activePanelComponent.UpdateAfterSimulation(CheckIsWorking(), IsInRange());
			}
		}

		private bool IsInRange()
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (IsContentStatic())
			{
				m_panelsComponent.Remove(this);
				return true;
			}
			return m_panelsComponent.IsInRange(this, m_maxRenderDistanceSquared);
		}

		private bool IsContentStatic()
		{
			if (m_activePanelComponent != null)
			{
				return m_activePanelComponent.IsStatic;
			}
			return false;
		}

<<<<<<< HEAD
		[Event(null, 896)]
=======
		[Event(null, 885)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void SetSelectedRotationIndex(int newIndex)
		{
			if (newIndex != m_selectedRotationIndex)
			{
				m_activePanelComponent.GetLocalValues(out var content, out var font, out var script);
				m_panelComponents[newIndex].SetLocalValues(content, font, script);
				m_panelComponents[newIndex].Text = PublicDescription;
				m_panelComponents[newIndex].SelectedTexturesToDraw.Clear();
				m_panelComponents[newIndex].SelectedTexturesToDraw.AddRange(m_activePanelComponent.SelectedTexturesToDraw);
<<<<<<< HEAD
				m_activePanelComponent.ChangeRenderTexture(m_selectedRotationIndex, null, isForced: false);
=======
				m_activePanelComponent.ChangeRenderTexture(m_selectedRotationIndex, null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_activePanelComponent.ReleaseTexture(useEmptyTexture: false);
				m_activePanelComponent = m_panelComponents[newIndex];
				m_selectedRotationIndex = newIndex;
				m_newSelectedRotationIndex = newIndex;
				RaisePropertiesChanged();
			}
		}

		private void HideInactivePanelComponents()
		{
			if (!base.IsFunctional)
<<<<<<< HEAD
			{
				return;
			}
			for (int i = 0; i < m_panelComponents.Count; i++)
			{
=======
			{
				return;
			}
			for (int i = 0; i < m_panelComponents.Count; i++)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (m_panelComponents[i] != null)
				{
					m_panelComponents[i].SetRender(Render);
					if (i != m_selectedRotationIndex && !System.StringExtensions.Contains(m_activePanelComponent.Name, "TransparentScreenArea", StringComparison.Ordinal))
					{
						m_panelComponents[i].RemoveTexture(i);
					}
				}
			}
		}

		private void SendSelectRotationIndexRequest(int selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.SetSelectedRotationIndex, selection);
		}

		private void SendRemoveSelectedImageRequest(Sandbox.ModAPI.IMyTextSurface panel, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnRemoveSelectedImageRequest, selection);
		}

<<<<<<< HEAD
		[Event(null, 944)]
=======
		[Event(null, 933)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveSelectedImageRequest(int[] selection)
		{
			PanelComponent.RemoveItems(selection);
		}

		private void SendAddImagesToSelectionRequest(Sandbox.ModAPI.IMyTextSurface panel, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnSelectImageRequest, selection);
		}

<<<<<<< HEAD
		[Event(null, 955)]
=======
		[Event(null, 944)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnSelectImageRequest(int[] selection)
		{
			PanelComponent.SelectItems(selection);
		}

		private void ChangeTextRequest(MyTextPanelComponent panel, string text)
		{
			MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnChangeDescription, text, arg3: true);
		}

		private void UpdateSpriteCollection(MyTextPanelComponent panel, MySerializableSpriteCollection sprites)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnUpdateSpriteCollection, sprites);
			}
		}

<<<<<<< HEAD
		[Event(null, 974)]
=======
		[Event(null, 963)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		[DistanceRadius(32f)]
		private void OnUpdateSpriteCollection(MySerializableSpriteCollection sprites)
		{
			foreach (MyTextPanelComponent panelComponent in m_panelComponents)
			{
				panelComponent?.UpdateSpriteCollection(sprites);
			}
		}

<<<<<<< HEAD
		[Event(null, 983)]
=======
		[Event(null, 972)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public new void OnChangeDescription(string description, bool isPublic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Clear().Append(description);
			if (isPublic)
			{
				PublicDescription = stringBuilder;
			}
			else
			{
				PrivateDescription = stringBuilder;
			}
		}

<<<<<<< HEAD
		[Event(null, 998)]
=======
		[Event(null, 987)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnChangeTitle(string title, bool isPublic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Clear().Append(title);
			if (isPublic)
			{
				PublicTitle = stringBuilder;
			}
			else
			{
				PrivateTitle = stringBuilder;
			}
		}

		private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0uL, bool isPublic = false)
		{
			MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnChangeOpenRequest, isOpen, editable, user, isPublic);
		}

<<<<<<< HEAD
		[Event(null, 1018)]
=======
		[Event(null, 1007)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			if (!(Sync.IsServer && IsTextPanelOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user, isPublic);
				MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnChangeOpenSuccess, isOpen, editable, user, isPublic);
			}
		}

<<<<<<< HEAD
		[Event(null, 1029)]
=======
		[Event(null, 1018)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			OnChangeOpen(isOpen, editable, user, isPublic);
		}

		private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			IsTextPanelOpen = isOpen;
			UserId = user;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && user == Sync.MyId && isOpen)
			{
				OpenWindow(editable, sync: false, isPublic);
			}
		}

		private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
		{
			if (base.CubeGrid.IsPreview || !base.CubeGrid.SyncFlag)
			{
				if (isPublic)
				{
					PublicDescription = description;
				}
				else
				{
					PrivateDescription = description;
				}
			}
			else if (!(description.CompareTo(PublicDescription) == 0 && isPublic) && (description.CompareTo(PrivateDescription) != 0 || isPublic))
			{
				MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnChangeDescription, description.ToString(), isPublic);
			}
		}

		private void SendChangeTitleMessage(StringBuilder title, bool isPublic)
		{
			if (base.CubeGrid.IsPreview || !base.CubeGrid.SyncFlag)
			{
				if (isPublic)
				{
					PublicTitle = title;
				}
				else
				{
					PrivateTitle = title;
				}
			}
			else if (!(title.CompareTo(PublicTitle) == 0 && isPublic) && (title.CompareTo(PrivateTitle) != 0 || isPublic))
			{
				if (isPublic)
				{
					PublicTitle = title;
				}
				else
				{
					PrivateTitle = title;
				}
				MyMultiplayer.RaiseEvent(this, (MyTextPanel x) => x.OnChangeTitle, title.ToString(), isPublic);
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextPanel.ShowPrivateTextOnScreen()
		{
			ShowTextFlag = ShowTextOnScreenFlag.PRIVATE;
		}

		void Sandbox.ModAPI.Ingame.IMyTextPanel.ShowPublicTextOnScreen()
		{
			ContentType = ContentType.TEXT_AND_IMAGE;
		}

		void Sandbox.ModAPI.Ingame.IMyTextPanel.ShowTextureOnScreen()
		{
			ContentType = ContentType.TEXT_AND_IMAGE;
		}

		void Sandbox.ModAPI.Ingame.IMyTextPanel.SetShowOnScreen(ShowTextOnScreenFlag set)
		{
			ShowTextFlag = set;
		}

		string Sandbox.ModAPI.Ingame.IMyTextPanel.GetPublicTitle()
		{
			return m_publicTitleHelper.ToString();
		}

		bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePublicTitle(string value, bool append)
		{
			if (m_isTextPanelOpen)
			{
				return false;
			}
			if (!append)
			{
				m_publicTitleHelper.Clear();
			}
			m_publicTitleHelper.Append(value);
			SendChangeTitleMessage(m_publicTitleHelper, isPublic: true);
			return true;
		}

		bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePublicText(string value, bool append)
		{
			return ((Sandbox.ModAPI.Ingame.IMyTextSurface)this).WriteText(value, append);
		}

		string Sandbox.ModAPI.Ingame.IMyTextPanel.GetPublicText()
		{
			return ((Sandbox.ModAPI.Ingame.IMyTextSurface)this).GetText();
		}

		bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePublicText(StringBuilder value, bool append)
		{
			return ((Sandbox.ModAPI.Ingame.IMyTextSurface)this).WriteText(value, append);
		}

		void Sandbox.ModAPI.Ingame.IMyTextPanel.ReadPublicText(StringBuilder buffer, bool append)
		{
			((Sandbox.ModAPI.Ingame.IMyTextSurface)this).ReadText(buffer, append);
		}

		bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePrivateTitle(string value, bool append)
		{
			if (m_isTextPanelOpen)
			{
				return false;
			}
			if (!append)
			{
				m_privateTitleHelper.Clear();
			}
			m_privateTitleHelper.Append(value);
			SendChangeTitleMessage(m_privateTitleHelper, isPublic: false);
			return true;
		}

		string Sandbox.ModAPI.Ingame.IMyTextPanel.GetPrivateTitle()
		{
			return m_privateTitle.ToString();
		}

		private void UpdateModApiText()
		{
			if (m_descriptionPrivateDirty)
			{
				m_descriptionPrivateDirty = false;
				SendChangeDescriptionMessage(m_privateDescriptionHelper, isPublic: false);
			}
			if (m_descriptionPublicDirty)
			{
				m_descriptionPublicDirty = false;
				SendChangeDescriptionMessage(m_publicDescriptionHelper, isPublic: true);
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyTextPanel.WritePrivateText(string value, bool append)
		{
			if (m_isTextPanelOpen)
			{
				return false;
			}
			if (!append)
			{
				m_privateDescriptionHelper.Clear();
			}
			m_privateDescriptionHelper.Append(value);
			m_descriptionPrivateDirty = true;
			return true;
		}

		string Sandbox.ModAPI.Ingame.IMyTextPanel.GetPrivateText()
		{
			return m_privateDescription.ToString();
		}

		bool Sandbox.ModAPI.Ingame.IMyTextSurface.WriteText(string value, bool append)
		{
			if (m_isTextPanelOpen)
			{
				return false;
			}
			if (!append)
			{
				m_publicDescriptionHelper.Clear();
			}
			if (value.Length + m_publicDescriptionHelper.Length > 100000)
			{
				value = value.Remove(100000 - m_publicDescriptionHelper.Length);
			}
			m_publicDescriptionHelper.Append(value);
			m_descriptionPublicDirty = true;
			return true;
		}

		string Sandbox.ModAPI.Ingame.IMyTextSurface.GetText()
		{
			return m_publicDescription.ToString();
		}

		bool Sandbox.ModAPI.Ingame.IMyTextSurface.WriteText(StringBuilder value, bool append)
		{
			if (m_isTextPanelOpen)
			{
				return false;
			}
			if (!append)
			{
				m_publicDescriptionHelper.Clear();
			}
			m_publicDescriptionHelper.Append((object)value);
			m_descriptionPublicDirty = true;
			return true;
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.ReadText(StringBuilder buffer, bool append)
		{
			if (!append)
			{
				buffer.Clear();
			}
			buffer.AppendStringBuilder(m_publicDescription);
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.AddImageToSelection(string id, bool checkExistence)
		{
			if (id == null)
			{
				return;
			}
			for (int i = 0; i < PanelComponent.Definitions.Count; i++)
			{
				if (!(PanelComponent.Definitions[i].Id.SubtypeName == id))
				{
					continue;
				}
				if (checkExistence)
				{
					for (int j = 0; j < PanelComponent.SelectedTexturesToDraw.Count; j++)
					{
						if (PanelComponent.SelectedTexturesToDraw[j].Id.SubtypeName == id)
						{
							return;
						}
					}
				}
				SendAddImagesToSelectionRequest(this, new int[1] { i });
				break;
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.AddImagesToSelection(List<string> ids, bool checkExistence)
		{
			if (ids == null)
<<<<<<< HEAD
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (string id in ids)
			{
=======
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (string id in ids)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				for (int i = 0; i < PanelComponent.Definitions.Count; i++)
				{
					if (!(PanelComponent.Definitions[i].Id.SubtypeName == id))
					{
						continue;
					}
					bool flag = false;
					if (checkExistence)
					{
						for (int j = 0; j < PanelComponent.SelectedTexturesToDraw.Count; j++)
						{
							if (PanelComponent.SelectedTexturesToDraw[j].Id.SubtypeName == id)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						list.Add(i);
					}
					break;
				}
			}
			if (list.Count > 0)
			{
				SendAddImagesToSelectionRequest(this, list.ToArray());
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.RemoveImageFromSelection(string id, bool removeDuplicates)
		{
			if (id == null)
			{
				return;
			}
			List<int> list = new List<int>();
			for (int i = 0; i < PanelComponent.Definitions.Count; i++)
			{
				if (!(PanelComponent.Definitions[i].Id.SubtypeName == id))
				{
					continue;
				}
				if (removeDuplicates)
				{
					for (int j = 0; j < PanelComponent.SelectedTexturesToDraw.Count; j++)
					{
						if (PanelComponent.SelectedTexturesToDraw[j].Id.SubtypeName == id)
						{
							list.Add(i);
						}
					}
				}
				else
				{
					list.Add(i);
				}
				break;
			}
			if (list.Count > 0)
			{
				SendRemoveSelectedImageRequest(this, list.ToArray());
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.RemoveImagesFromSelection(List<string> ids, bool removeDuplicates)
		{
			if (ids == null)
<<<<<<< HEAD
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (string id in ids)
			{
				for (int i = 0; i < PanelComponent.Definitions.Count; i++)
				{
					if (!(PanelComponent.Definitions[i].Id.SubtypeName == id))
					{
						continue;
					}
					if (removeDuplicates)
					{
=======
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (string id in ids)
			{
				for (int i = 0; i < PanelComponent.Definitions.Count; i++)
				{
					if (!(PanelComponent.Definitions[i].Id.SubtypeName == id))
					{
						continue;
					}
					if (removeDuplicates)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						for (int j = 0; j < PanelComponent.SelectedTexturesToDraw.Count; j++)
						{
							if (PanelComponent.SelectedTexturesToDraw[j].Id.SubtypeName == id)
							{
								list.Add(i);
							}
						}
					}
					else
					{
						list.Add(i);
					}
					break;
				}
			}
			if (list.Count > 0)
			{
				SendRemoveSelectedImageRequest(this, list.ToArray());
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.ClearImagesFromSelection()
		{
			if (PanelComponent.SelectedTexturesToDraw.Count == 0)
			{
				return;
			}
			List<int> list = new List<int>();
			for (int i = 0; i < PanelComponent.SelectedTexturesToDraw.Count; i++)
			{
				for (int j = 0; j < PanelComponent.Definitions.Count; j++)
				{
					if (PanelComponent.Definitions[j].Id.SubtypeName == PanelComponent.SelectedTexturesToDraw[i].Id.SubtypeName)
					{
						list.Add(j);
						break;
					}
				}
			}
			SendRemoveSelectedImageRequest(this, list.ToArray());
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.GetSelectedImages(List<string> output)
		{
			foreach (MyLCDTextureDefinition item in PanelComponent.SelectedTexturesToDraw)
			{
				output.Add(item.Id.SubtypeName);
			}
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.GetFonts(List<string> fonts)
		{
			if (fonts == null)
			{
				return;
			}
			foreach (MyFontDefinition definition in MyDefinitionManager.Static.GetDefinitions<MyFontDefinition>())
			{
				fonts.Add(definition.Id.SubtypeName);
			}
		}

		public void GetSprites(List<string> sprites)
		{
			PanelComponent.GetSprites(sprites);
		}

		void Sandbox.ModAPI.Ingame.IMyTextSurface.GetScripts(List<string> scripts)
		{
			if (m_activePanelComponent != null)
			{
				m_activePanelComponent.GetScripts(scripts);
			}
		}

		MySpriteDrawFrame Sandbox.ModAPI.Ingame.IMyTextSurface.DrawFrame()
		{
			if (m_activePanelComponent != null)
			{
				return m_activePanelComponent.DrawFrame();
			}
			return new MySpriteDrawFrame(null);
		}

		public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale)
		{
			return MyGuiManager.MeasureStringRaw(font, text, scale);
		}

		public Vector2 MeasureStringInPixels(string text, string font, float scale)
		{
			return MyGuiManager.MeasureStringRaw(font, text, scale);
		}

		Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(int index)
		{
			if (index != 0)
			{
				return null;
			}
			return m_activePanelComponent;
		}
	}
}
