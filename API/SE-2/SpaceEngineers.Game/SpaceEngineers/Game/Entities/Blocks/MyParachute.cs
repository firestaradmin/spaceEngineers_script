using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Parachute))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyParachute),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)
	})]
	public class MyParachute : MyDoorBase, SpaceEngineers.Game.ModAPI.IMyParachute, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyParachute, IMyConveyorEndpointBlock
	{
		protected sealed class AutoDeployRequest_003C_003ESystem_Boolean_0023System_Int64 : ICallSite<MyParachute, bool, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyParachute @this, in bool autodeploy, in long identityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.AutoDeployRequest(autodeploy, identityId);
			}
		}

		protected sealed class DeployHeightRequest_003C_003ESystem_Single_0023System_Int64 : ICallSite<MyParachute, float, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyParachute @this, in float deployHeight, in long identityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DeployHeightRequest(deployHeight, identityId);
			}
		}

		protected sealed class DoDeployChute_003C_003E : ICallSite<MyParachute, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyParachute @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.DoDeployChute();
			}
		}

		protected class m_autoDeploy_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType autoDeploy;
				ISyncType result = (autoDeploy = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyParachute)P_0).m_autoDeploy = (Sync<bool, SyncDirection.BothWays>)autoDeploy;
				return result;
			}
		}

		protected class m_deployHeight_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType deployHeight;
				ISyncType result = (deployHeight = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyParachute)P_0).m_deployHeight = (Sync<float, SyncDirection.BothWays>)deployHeight;
				return result;
			}
		}

		private const float MIN_DEPLOY_HEIGHT = 10f;

		private const float MAX_DEPLOY_HEIGHT = 10000f;

		private const double DENSITY_OF_AIR_IN_ONE_ATMO = 1.225;

		private const float NO_DRAG_SPEED_SQRD = 0.1f;

		private const float NO_DRAG_SPEED_RANGE = 20f;

		private bool m_stateChange;

		private List<MyEntity3DSoundEmitter> m_emitter = new List<MyEntity3DSoundEmitter>();

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		protected readonly Sync<bool, SyncDirection.BothWays> m_autoDeploy;

		protected readonly Sync<float, SyncDirection.BothWays> m_deployHeight;

		private MyPlanet m_nearPlanetCache;

		private MyEntitySubpart m_parachuteSubpart;

		private Vector3 m_lastParachuteVelocityVector = Vector3.Zero;

		private Vector3 m_lastParachuteScale = Vector3.Zero;

		private Vector3 m_gravityCache = Vector3.Zero;

		private Vector3D m_chuteScale = Vector3D.Zero;

		private Vector3D? m_closestPointCache;

		private int m_parachuteAnimationState;

		private int m_cutParachuteTimer;

		private bool m_canDeploy;

		private bool m_canCheckAutoDeploy;

		private bool m_atmosphereDirty = true;

		private float m_minAtmosphere = 0.2f;

		private float m_dragCoefficient = 1f;

		private float m_atmosphereDensityCache;

		private MyFixedPoint m_requiredItemsInInventory = 0;

		private Quaternion m_lastParachuteRotation = Quaternion.Identity;

		private Matrix m_lastParachuteLocalMatrix = Matrix.Identity;

		private MatrixD m_lastParachuteWorldMatrix = MatrixD.Identity;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		DoorStatus SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.Status
		{
			get
			{
				if ((bool)m_open)
				{
					return DoorStatus.Open;
				}
				return DoorStatus.Closed;
			}
		}

		public float OpenRatio
		{
			get
			{
				if ((bool)m_open)
<<<<<<< HEAD
				{
					return 1f;
				}
				return 0f;
			}
		}

		float SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.AutoDeployHeight
		{
			get
			{
				return DeployHeight;
			}
			set
			{
				if (!float.IsNaN(value))
				{
					DeployHeight = value;
=======
				{
					return 1f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		public bool AutoDeploy
		{
			get
			{
				return m_autoDeploy;
			}
			set
			{
				if ((bool)m_autoDeploy != value)
				{
					m_autoDeploy.Value = value;
				}
			}
		}

		public float DeployHeight
		{
			get
			{
				return m_deployHeight;
			}
			set
			{
				value = MathHelper.Clamp(value, 10f, 10000f);
				if ((float)m_deployHeight != value)
				{
					m_deployHeight.Value = value;
				}
			}
		}

		public float DragCoefficient => m_dragCoefficient;

		public bool CanDeploy
		{
			get
			{
<<<<<<< HEAD
				if (base.IsWorking && Enabled)
=======
				if (base.IsWorking && base.Enabled)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return m_canDeploy;
				}
				return false;
			}
			set
			{
				m_canDeploy = value;
			}
		}

		public float Atmosphere
		{
			get
			{
				if (!m_atmosphereDirty)
				{
					return m_atmosphereDensityCache;
				}
				m_atmosphereDirty = false;
				if (m_nearPlanetCache == null)
				{
					return m_atmosphereDensityCache = 0f;
				}
				return m_atmosphereDensityCache = m_nearPlanetCache.GetAirDensity(base.WorldMatrix.Translation);
			}
		}

		private new MyParachuteDefinition BlockDefinition => (MyParachuteDefinition)base.BlockDefinition;

		private event Action<bool> DoorStateChanged;

		event Action<bool> SpaceEngineers.Game.ModAPI.IMyParachute.DoorStateChanged
		{
			add
			{
				DoorStateChanged += value;
			}
			remove
			{
				DoorStateChanged -= value;
			}
		}

		private event Action<bool> ParachuteStateChanged;

		event Action<bool> SpaceEngineers.Game.ModAPI.IMyParachute.ParachuteStateChanged
		{
			add
			{
				ParachuteStateChanged += value;
			}
			remove
			{
				ParachuteStateChanged -= value;
			}
		}

		public MyParachute()
		{
			m_emitter.Clear();
			m_open.ValueChanged += delegate
			{
				OnStateChange();
			};
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.OpenDoor()
		{
			if (base.IsWorking && !m_open)
			{
				((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)this).ToggleDoor();
			}
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.CloseDoor()
		{
			if (base.IsWorking && (bool)m_open)
			{
				((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)this).ToggleDoor();
			}
		}

		void SpaceEngineers.Game.ModAPI.Ingame.IMyParachute.ToggleDoor()
		{
			if (base.IsWorking)
			{
				SetOpenRequest(!base.Open, base.OwnerId);
			}
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			UpdateEmissivity();
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		private void UpdateEmissivity()
		{
			if (Enabled && base.ResourceSink != null && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				MyCubeBlock.UpdateEmissiveParts(base.Render.RenderObjectIDs[0], 1f, Color.Green, Color.White);
				OnStateChange();
			}
			else
			{
				MyCubeBlock.UpdateEmissiveParts(base.Render.RenderObjectIDs[0], 0f, Color.Red, Color.White);
			}
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyParachute>())
			{
				base.CreateTerminalControls();
				MyTerminalControlCheckbox<MyParachute> obj = new MyTerminalControlCheckbox<MyParachute>("AutoDeploy", MySpaceTexts.Parachute_AutoDeploy, MySpaceTexts.Parachute_AutoDeployTooltip, MySpaceTexts.Parachute_AutoDeployOn, MySpaceTexts.Parachute_AutoDeployOff)
				{
					Getter = (MyParachute x) => x.AutoDeploy,
					Setter = delegate(MyParachute x, bool v)
					{
						x.SetAutoDeployRequest(v, x.OwnerId);
					}
				};
				obj.EnableAction();
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlSlider<MyParachute> myTerminalControlSlider = new MyTerminalControlSlider<MyParachute>("AutoDeployHeight", MySpaceTexts.Parachute_DeployHeightTitle, MySpaceTexts.Parachute_DeployHeightTooltip, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true);
				myTerminalControlSlider.Getter = (MyParachute x) => x.DeployHeight;
				myTerminalControlSlider.Setter = delegate(MyParachute x, float v)
				{
					x.SetDeployHeightRequest(v, x.OwnerId);
				};
				myTerminalControlSlider.Writer = delegate(MyParachute b, StringBuilder v)
				{
					v.Append($"{b.DeployHeight:N0} m");
				};
				myTerminalControlSlider.SetLogLimits(10f, 10000f);
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			}
		}

		public void SetAutoDeployRequest(bool autodeploy, long identityId)
		{
			MyMultiplayer.RaiseEvent(this, (MyParachute x) => x.AutoDeployRequest, autodeploy, identityId);
		}

<<<<<<< HEAD
		[Event(null, 273)]
=======
		[Event(null, 257)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void AutoDeployRequest(bool autodeploy, long identityId)
		{
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = GetUserRelationToOwner(identityId);
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			MyPlayer myPlayer = ((myIdentity != null && myIdentity.Character != null) ? MyPlayer.GetPlayerFromCharacter(myIdentity.Character) : null);
			bool flag = false;
			if (myPlayer != null && !userRelationToOwner.IsFriendly() && MySession.Static.RemoteAdminSettings.TryGetValue(myPlayer.Client.SteamUserId, out var value))
			{
				flag = value.HasFlag(AdminSettingsEnum.UseTerminals);
			}
			if (userRelationToOwner.IsFriendly() || flag)
			{
				AutoDeploy = autodeploy;
			}
		}

		public void SetDeployHeightRequest(float deployHeight, long identityId)
		{
			MyMultiplayer.RaiseEvent(this, (MyParachute x) => x.DeployHeightRequest, deployHeight, identityId);
		}

<<<<<<< HEAD
		[Event(null, 299)]
=======
		[Event(null, 283)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void DeployHeightRequest(float deployHeight, long identityId)
		{
			MyRelationsBetweenPlayerAndBlock userRelationToOwner = GetUserRelationToOwner(identityId);
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			MyPlayer myPlayer = ((myIdentity != null && myIdentity.Character != null) ? MyPlayer.GetPlayerFromCharacter(myIdentity.Character) : null);
			bool flag = false;
			if (myPlayer != null && !userRelationToOwner.IsFriendly() && MySession.Static.RemoteAdminSettings.TryGetValue(myPlayer.Client.SteamUserId, out var value))
			{
				flag = value.HasFlag(AdminSettingsEnum.UseTerminals);
			}
			if (userRelationToOwner.IsFriendly() || flag)
			{
				DeployHeight = deployHeight;
			}
		}

		private void OnStateChange()
		{
			base.ResourceSink.Update();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME;
			if ((bool)m_open)
			{
				this.DoorStateChanged?.Invoke(m_open);
			}
			m_stateChange = true;
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		public override void OnBuildSuccess(long builtBy, bool instantBuild)
		{
			base.ResourceSink.Update();
			base.OnBuildSuccess(builtBy, instantBuild);
		}

		public override void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.PowerConsumptionMoving, UpdatePowerInput, this);
			base.ResourceSink = myResourceSinkComponent;
			base.Init(builder, cubeGrid);
			MyObjectBuilder_Parachute myObjectBuilder_Parachute = (MyObjectBuilder_Parachute)builder;
			m_open.Value = myObjectBuilder_Parachute.Open;
			m_deployHeight.ValidateRange(10f, 10000f);
			m_deployHeight.Value = myObjectBuilder_Parachute.DeployHeight;
			m_autoDeploy.Value = myObjectBuilder_Parachute.AutoDeploy;
			m_parachuteAnimationState = myObjectBuilder_Parachute.ParachuteState;
			if (m_parachuteAnimationState > 50)
			{
				m_parachuteAnimationState = 0;
			}
			m_dragCoefficient = BlockDefinition.DragCoefficient;
			m_minAtmosphere = BlockDefinition.MinimumAtmosphereLevel;
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			myResourceSinkComponent.Update();
			OnStateChange();
			InitializeConveyorEndpoint();
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.ResourceSink.Update();
			MyInventory myInventory = this.GetInventory();
			MyComponentDefinition componentDefinition = MyDefinitionManager.Static.GetComponentDefinition(BlockDefinition.MaterialDefinitionId);
			if (myInventory == null)
			{
				Vector3 one = Vector3.One;
				myInventory = new MyInventory(componentDefinition.Volume * (float)BlockDefinition.MaterialDeployCost, one, MyInventoryFlags.CanReceive);
				base.Components.Add((MyInventoryBase)myInventory);
			}
			inventory_ContentsChanged(myInventory);
			myInventory.ContentsChanged += inventory_ContentsChanged;
			MyInventoryConstraint myInventoryConstraint = new MyInventoryConstraint(MySpaceTexts.Parachute_ConstraintItem);
			myInventoryConstraint.Add(BlockDefinition.MaterialDefinitionId);
			myInventoryConstraint.Icon = MyGuiConstants.TEXTURE_ICON_FILTER_COMPONENT;
			myInventory.Constraint = myInventoryConstraint;
<<<<<<< HEAD
			if (m_parachuteSubpart == null && m_parachuteAnimationState != 0)
			{
				m_parachuteSubpart = LoadSubpartFromName(BlockDefinition.ParachuteSubpartName);
				m_parachuteSubpart.Render.Visible = true;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}

		private MyEntitySubpart LoadSubpartFromName(string name)
		{
			if (base.Subparts.TryGetValue(name, out var value))
			{
				return value;
			}
			value = new MyEntitySubpart();
			string model = Path.Combine(Path.GetDirectoryName(base.Model.AssetName), name) + ".mwm";
			value.Render.EnableColorMaskHsv = base.Render.EnableColorMaskHsv;
			value.Render.ColorMaskHsv = base.Render.ColorMaskHsv;
			value.Render.TextureChanges = base.Render.TextureChanges;
			value.Render.MetalnessColorable = base.Render.MetalnessColorable;
			value.Init(null, model, this, null);
			base.Subparts[name] = value;
			if (base.InScene)
			{
				value.OnAddedToScene(this);
			}
			return value;
		}

		private void InitSubparts()
		{
			if (base.CubeGrid.CreatePhysics)
			{
				m_emitter.Clear();
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Parachute obj = (MyObjectBuilder_Parachute)base.GetObjectBuilderCubeBlock(copy);
			obj.Open = m_open;
			obj.AutoDeploy = m_autoDeploy;
			obj.DeployHeight = m_deployHeight;
			obj.ParachuteState = m_parachuteAnimationState;
			return obj;
		}

		protected float UpdatePowerInput()
		{
			if (!Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			return BlockDefinition.PowerConsumptionIdle;
		}

		private void StartSound(int emitterId, MySoundPair cuePair)
		{
			if (m_emitter[emitterId].Sound == null || !m_emitter[emitterId].Sound.IsPlaying || (!(m_emitter[emitterId].SoundId == cuePair.Arcade) && !(m_emitter[emitterId].SoundId == cuePair.Realistic)))
			{
				m_emitter[emitterId].StopSound(forced: true);
				m_emitter[emitterId].PlaySingleSound(cuePair);
			}
		}

		public override void UpdateSoundEmitters()
		{
			for (int i = 0; i < m_emitter.Count; i++)
			{
				if (m_emitter[i] != null)
				{
					m_emitter[i].Update();
				}
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			UpdateNearPlanet();
		}

		public override void UpdateBeforeSimulation()
		{
			if ((bool)m_open || (m_parachuteAnimationState > 0 && m_parachuteAnimationState < 50))
			{
				UpdateParachute();
			}
			else
			{
				UpdateCutChute();
				if (m_parachuteSubpart != null && m_parachuteSubpart.Render.RenderObjectIDs[0] != uint.MaxValue)
				{
					m_parachuteSubpart.Render.Visible = false;
				}
			}
			if (Sync.IsServer && m_canCheckAutoDeploy)
			{
				CheckAutoDeploy();
			}
			if (m_stateChange)
			{
				base.ResourceSink.Update();
				RaisePropertiesChanged();
				if (!m_open)
				{
					this.DoorStateChanged.InvokeIfNotNull(m_open);
				}
				m_stateChange = false;
			}
			base.UpdateBeforeSimulation();
		}

		public override void UpdateBeforeSimulation10()
		{
			m_atmosphereDirty = true;
			if (base.CubeGrid.Physics != null)
			{
				m_gravityCache = GetNaturalGravity();
				m_canCheckAutoDeploy = false;
				if (AutoDeploy && CanDeploy && base.CubeGrid.Physics.LinearVelocity.LengthSquared() > 2f && Vector3.Dot(m_gravityCache, base.CubeGrid.Physics.LinearVelocity) > 0.6f)
				{
					m_canCheckAutoDeploy = TryGetClosestPointInAtmosphere(out m_closestPointCache);
				}
			}
			base.UpdateBeforeSimulation10();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.CubeGrid.Physics != null)
<<<<<<< HEAD
			{
				UpdateParachutePosition();
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (Sync.IsServer && !CanDeploy)
			{
				AttemptPullRequiredInventoryItems();
			}
		}

=======
			{
				UpdateParachutePosition();
			}
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			if (Sync.IsServer && !CanDeploy)
			{
				AttemptPullRequiredInventoryItems();
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void AttemptPullRequiredInventoryItems()
		{
			if (BlockDefinition.MaterialDeployCost > m_requiredItemsInInventory)
			{
				base.CubeGrid.GridSystems.ConveyorSystem.PullItem(BlockDefinition.MaterialDefinitionId, BlockDefinition.MaterialDeployCost - m_requiredItemsInInventory, this, this.GetInventory(), remove: false, calcImmediately: false);
			}
		}

		private bool CheckDeployChute()
		{
			if (base.CubeGrid.Physics == null || !CanDeploy || m_parachuteAnimationState > 0 || Atmosphere < m_minAtmosphere)
			{
				return false;
			}
			if (!MySession.Static.CreativeMode)
			{
				MyInventory inventory = this.GetInventory();
				if (!(inventory.GetItemAmount(BlockDefinition.MaterialDefinitionId) >= BlockDefinition.MaterialDeployCost))
				{
					CanDeploy = false;
					return false;
				}
				inventory.RemoveItemsOfType(BlockDefinition.MaterialDeployCost, BlockDefinition.MaterialDefinitionId);
			}
			MyMultiplayer.RaiseEvent(this, (MyParachute x) => x.DoDeployChute);
			return true;
		}

<<<<<<< HEAD
		[Event(null, 606)]
=======
		[Event(null, 582)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private void DoDeployChute()
		{
			m_parachuteAnimationState = 1;
			m_lastParachuteRotation = Quaternion.Identity;
			m_lastParachuteScale = Vector3.Zero;
			m_cutParachuteTimer = 0;
			if (m_parachuteSubpart == null)
			{
				m_parachuteSubpart = LoadSubpartFromName(BlockDefinition.ParachuteSubpartName);
			}
			m_parachuteSubpart.Render.Visible = true;
			if (this.ParachuteStateChanged != null)
			{
				this.ParachuteStateChanged(obj: true);
			}
		}

		private void RemoveChute()
		{
			m_parachuteAnimationState = 0;
			if (m_parachuteSubpart != null)
			{
				m_parachuteSubpart.Render.Visible = false;
				if (Sync.IsServer)
				{
					((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)this).CloseDoor();
				}
			}
		}

		/// <summary>
		/// Called from game update. only called when door is opened fully closing or opening.
		/// </summary>
		private void UpdateParachute()
		{
			if (base.CubeGrid.Physics == null)
			{
				return;
			}
			if (m_parachuteAnimationState > 50)
			{
				if (!Sync.IsServer || !CanDeploy || !m_open || !CheckDeployChute())
				{
					UpdateCutChute();
				}
				return;
			}
			if (m_parachuteAnimationState == 0 && Sync.IsServer && CanDeploy && (bool)m_open)
			{
				CheckDeployChute();
			}
			if (m_parachuteAnimationState > 0 && m_parachuteAnimationState < 50)
			{
				m_parachuteAnimationState++;
			}
			Vector3 zero = Vector3.Zero;
			bool flag = false;
			float num = base.CubeGrid.Physics.LinearVelocity.LengthSquared();
			if (num > 2f)
			{
				zero = base.CubeGrid.Physics.LinearVelocity;
				m_cutParachuteTimer = 0;
			}
			else if (0.1f > num)
			{
				flag = true;
				zero = Vector3.Lerp(m_lastParachuteVelocityVector, -m_gravityCache, 0.05f);
				if (Vector3.DistanceSquared(zero, -m_gravityCache) < 0.0025f)
				{
					m_cutParachuteTimer++;
					if (m_cutParachuteTimer > 60)
					{
						if (Sync.IsServer)
						{
							((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)this).CloseDoor();
						}
						UpdateCutChute();
						return;
					}
				}
			}
			else
			{
				flag = true;
				zero = base.CubeGrid.Physics.LinearVelocity;
			}
			double num2 = 10.0 * (double)(Atmosphere - BlockDefinition.ReefAtmosphereLevel) * ((double)m_parachuteAnimationState / 50.0);
			if (num2 <= 0.5 || double.IsNaN(num2))
			{
				num2 = 0.5;
			}
			else
			{
				num2 = Math.Log(num2 - 0.99) + 5.0;
				if (num2 < 0.5 || double.IsNaN(num2))
				{
					num2 = 0.5;
				}
			}
			m_chuteScale.Z = ((m_parachuteAnimationState != 0) ? (Math.Log((double)m_parachuteAnimationState / 1.5) * (double)base.CubeGrid.GridSize * 20.0) : 0.0);
			m_chuteScale.X = (m_chuteScale.Y = num2 * (double)BlockDefinition.RadiusMultiplier * (double)base.CubeGrid.GridSize);
			m_lastParachuteVelocityVector = zero;
<<<<<<< HEAD
			Vector3D vector3D = ((!Vector3D.IsZero(zero)) ? Vector3D.Normalize(zero) : base.PositionComp.WorldMatrixRef.Up);
=======
			Vector3D vector3D = ((!Vector3D.IsZero(zero)) ? Vector3D.Normalize(zero) : base.PositionComp.WorldMatrix.Up);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Quaternion quaternion = Quaternion.CreateFromRotationMatrix(Matrix.CreateFromDir(vector3D, new Vector3(0f, 1f, 0f)).GetOrientation());
			quaternion = Quaternion.Lerp(m_lastParachuteRotation, quaternion, 0.02f);
			m_chuteScale = Vector3D.Lerp(m_lastParachuteScale, m_chuteScale, 0.02);
			double num3 = m_chuteScale.X / 2.0;
			m_lastParachuteScale = m_chuteScale;
			m_lastParachuteRotation = quaternion;
			MatrixD matrixD = MatrixD.Invert(base.WorldMatrix);
			m_lastParachuteWorldMatrix = MatrixD.CreateFromTransformScale(m_lastParachuteRotation, base.WorldMatrix.Translation + base.WorldMatrix.Up * ((double)base.CubeGrid.GridSize / 2.0), m_lastParachuteScale);
			MatrixD m = m_lastParachuteWorldMatrix * matrixD;
			m_lastParachuteLocalMatrix = m;
			if (!(num3 <= 0.0 || flag) && !(zero.LengthSquared() <= 1f))
			{
				Vector3D value = -vector3D;
				double num4 = Math.PI * num3 * num3;
				double num5 = 2.5 * ((double)Atmosphere * 1.225) * (double)zero.LengthSquared() * num4 * (double)DragCoefficient;
				if (num5 > 0.0 && !base.CubeGrid.Physics.IsStatic)
				{
					base.CubeGrid.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_FORCE, Vector3D.Multiply(value, num5), base.WorldMatrix.Translation, Vector3.Zero);
				}
			}
		}

		private void UpdateParachutePosition()
		{
			if (m_parachuteSubpart != null && m_parachuteAnimationState > 0)
			{
				m_parachuteSubpart.PositionComp.SetLocalMatrix(ref m_lastParachuteLocalMatrix);
			}
		}

		/// <summary>
		/// Called each tick when door is closed or called from UpdateParachutePosition if the door is opening/closing/fullyopen after being closed. 
		/// </summary>
		private void UpdateCutChute()
		{
			if (base.CubeGrid.Physics == null || m_parachuteAnimationState == 0)
			{
				return;
			}
			if (m_parachuteAnimationState > 100)
			{
				RemoveChute();
				return;
			}
			if (m_parachuteAnimationState < 50)
			{
				m_parachuteAnimationState = 50;
			}
			if (m_parachuteAnimationState == 50)
			{
				this.ParachuteStateChanged.InvokeIfNotNull(arg1: false);
			}
			m_parachuteAnimationState++;
			if (m_parachuteSubpart != null)
			{
				m_lastParachuteWorldMatrix.Translation += m_gravityCache * 0.05f;
				MatrixD m = m_lastParachuteWorldMatrix * MatrixD.Invert(base.WorldMatrix);
				Matrix localMatrix = m;
				m_parachuteSubpart.PositionComp.SetLocalMatrix(ref localMatrix);
			}
		}

		private void CheckAutoDeploy()
		{
			if (m_closestPointCache.HasValue && Vector3D.DistanceSquared(m_closestPointCache.Value, base.WorldMatrix.Translation) < (double)(DeployHeight * DeployHeight))
			{
				((SpaceEngineers.Game.ModAPI.Ingame.IMyParachute)this).OpenDoor();
			}
		}

		private void UpdateNearPlanet()
		{
			BoundingBoxD box = base.PositionComp.WorldAABB;
			m_nearPlanetCache = MyGamePruningStructure.GetClosestPlanet(ref box);
		}

		protected override void Closing()
		{
			for (int i = 0; i < m_emitter.Count; i++)
			{
				if (m_emitter[i] != null)
				{
					m_emitter[i].StopSound(forced: true);
				}
			}
			base.Closing();
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			InitSubparts();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			UpdateEmissivity();
		}

		private void inventory_ContentsChanged(MyInventoryBase obj)
		{
			if (MySession.Static.CreativeMode)
			{
				CanDeploy = true;
				return;
			}
			m_requiredItemsInInventory = obj.GetItemAmount(BlockDefinition.MaterialDefinitionId);
			if (m_requiredItemsInInventory >= BlockDefinition.MaterialDeployCost)
			{
				CanDeploy = true;
			}
			else
			{
				CanDeploy = false;
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		public PullInformation GetPullInformation()
		{
			return new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId,
				ItemDefinition = BlockDefinition.MaterialDefinitionId
			};
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}

		private bool TryGetClosestPointInAtmosphere(out Vector3D? closestPoint)
		{
			if (TryGetClosestPoint(out closestPoint) && m_minAtmosphere > Atmosphere)
			{
				return false;
			}
			return true;
		}

		public bool TryGetClosestPoint(out Vector3D? closestPoint)
		{
			closestPoint = null;
			BoundingBoxD box = base.PositionComp.WorldAABB;
			m_nearPlanetCache = MyGamePruningStructure.GetClosestPlanet(ref box);
			if (m_nearPlanetCache == null)
			{
				return false;
			}
			Vector3D globalPos = base.CubeGrid.Physics.CenterOfMassWorld;
			closestPoint = m_nearPlanetCache.GetClosestSurfacePointGlobal(ref globalPos);
			return true;
		}

		public Vector3D GetVelocity()
		{
			MyPhysicsComponentBase myPhysicsComponentBase = ((base.Parent != null) ? base.Parent.Physics : null);
			if (myPhysicsComponentBase != null)
			{
				return new Vector3D(myPhysicsComponentBase.GetVelocityAtPoint(base.PositionComp.GetPosition()));
			}
			return Vector3D.Zero;
		}

		public Vector3D GetNaturalGravity()
		{
			return MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.WorldMatrix.Translation);
		}

		public Vector3D GetArtificialGravity()
		{
			return MyGravityProviderSystem.CalculateArtificialGravityInPoint(base.WorldMatrix.Translation);
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

		public Vector3D GetTotalGravity()
		{
			return MyGravityProviderSystem.CalculateTotalGravityInPoint(base.WorldMatrix.Translation);
		}

		public override void ContactCallbackInternal()
		{
			base.ContactCallbackInternal();
		}

		public override bool EnableContactCallbacks()
		{
			return false;
		}

		public override bool IsClosing()
		{
			return false;
		}
	}
}
