using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.ParticleEffects;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.Entity.EntityComponents;
using VRage.Game.Entity.UseObject;
using VRage.Game.Graphics;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Render.Particles;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Entities
{
	public class MyCubeBlock : MyEntity, IMyComponentOwner<MyIDModule>, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.IMyUpgradableBlock, Sandbox.ModAPI.Ingame.IMyUpgradableBlock
	{
		private class MethodDataIsConnectedTo
		{
			public List<MyCubeBlockDefinition.MountPoint> MyMountPoints = new List<MyCubeBlockDefinition.MountPoint>();

			public List<MyCubeBlockDefinition.MountPoint> OtherMountPoints = new List<MyCubeBlockDefinition.MountPoint>();

			public void Clear()
			{
				MyMountPoints.Clear();
				OtherMountPoints.Clear();
			}
		}

		public class AttachedUpgradeModule
		{
			public Sandbox.ModAPI.IMyUpgradeModule Block;

			public int SlotCount = 1;

			public bool Compatible = true;

			public AttachedUpgradeModule(Sandbox.ModAPI.IMyUpgradeModule block)
			{
				Block = block;
			}

			public AttachedUpgradeModule(Sandbox.ModAPI.IMyUpgradeModule block, int slotCount, bool compatible)
			{
				Block = block;
				SlotCount = slotCount;
				Compatible = compatible;
			}
		}

		private class DetonationData
		{
			public bool HasDetonated;

			public float DamageThreshold = 10f;

			public float DetonateChance = 0.25f;

			public float ExplosionRadiusMin = 0.1f;

			public float ExplosionRadiusMax = 30f;

			public float ExplosionAmmoVolumeMin = 1f;

			public float ExplosionAmmoVolumeMax = 100000f;

			public float ExplosionDamagePerLiter = 0.05f;

			public float ExplosionDamageMax = 5000f;

			public float CachedAmmoMass;

			public float CachedAmmoVolume;
		}

		public struct EmissiveNames
		{
			public MyStringHash Working;

			public MyStringHash Disabled;

			public MyStringHash Warning;

			public MyStringHash Damaged;

			public MyStringHash Alternative;

			public MyStringHash Locked;

			public MyStringHash Autolock;

			public MyStringHash Constraint;

			public EmissiveNames(bool ignore)
			{
				Working = MyStringHash.GetOrCompute("Working");
				Disabled = MyStringHash.GetOrCompute("Disabled");
				Damaged = MyStringHash.GetOrCompute("Damaged");
				Alternative = MyStringHash.GetOrCompute("Alternative");
				Locked = MyStringHash.GetOrCompute("Locked");
				Autolock = MyStringHash.GetOrCompute("Autolock");
				Warning = MyStringHash.GetOrCompute("Warning");
				Constraint = MyStringHash.GetOrCompute("Constraint");
			}
		}

		public class MyBlockPosComponent : MyPositionComponent
		{
			private class Sandbox_Game_Entities_MyCubeBlock_003C_003EMyBlockPosComponent_003C_003EActor : IActivator, IActivator<MyBlockPosComponent>
			{
				private sealed override object CreateInstance()
				{
					return new MyBlockPosComponent();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyBlockPosComponent CreateInstance()
				{
					return new MyBlockPosComponent();
				}

				MyBlockPosComponent IActivator<MyBlockPosComponent>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			protected override void OnWorldPositionChanged(object source, bool updateChildren, bool forceUpdateAllChildren)
			{
				base.OnWorldPositionChanged(source, updateChildren, forceUpdateAllChildren);
				(base.Container.Entity as MyCubeBlock).WorldPositionChanged(source);
			}
		}

		private class Sandbox_Game_Entities_MyCubeBlock_003C_003EActor : IActivator, IActivator<MyCubeBlock>
		{
			private sealed override object CreateInstance()
			{
				return new MyCubeBlock();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCubeBlock CreateInstance()
			{
				return new MyCubeBlock();
			}

			MyCubeBlock IActivator<MyCubeBlock>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected static readonly string DUMMY_SUBBLOCK_ID;

		private static List<MyCubeBlockDefinition.MountPoint> m_tmpMountPoints;

		private static List<MyCubeBlockDefinition.MountPoint> m_tmpBlockMountPoints;

		private static List<MyCubeBlockDefinition.MountPoint> m_tmpOtherBlockMountPoints;

		public MyEntity3DSoundEmitter m_soundEmitter;

		private bool m_shouldDetonateAmmo;

		private DetonationData m_detonationData;

		protected static EmissiveNames m_emissiveNames;

		public Dictionary<long, AttachedUpgradeModule> CurrentAttachedUpgradeModules;

		private MyResourceSinkComponent m_sinkComp;

		public bool IsBeingRemoved;

		protected List<MyCubeBlockEffect> m_activeEffects;

		private bool? m_setDamagedEffectDelayed = false;

		private bool m_checkConnectionAllowed;

		private int m_numberInGrid;

		public MySlimBlock SlimBlock;

		public bool IsSilenced;

		public bool SilenceInChange;

		public bool UsedUpdateEveryFrame;

		/// <summary>
		/// Detectors contains inverted matrices
		/// </summary>
		private MyIDModule m_IDModule;

		/// <summary>
		/// Map from dummy name to subblock (subgrid, note that after grid split the subblock instance will be the same)
		/// </summary>
		protected Dictionary<string, MySlimBlock> SubBlocks;

		/// <summary>
		/// Loaded subblocks from object builder. Cached for getting grid entity when loaded.
		/// </summary>
		private List<MyObjectBuilder_CubeBlock.MySubBlockId> m_loadedSubBlocks;

		private static MethodDataIsConnectedTo m_methodDataIsConnectedTo;

		protected bool m_forceBlockDestructible;

		private MyParticleEffect m_damageEffect;

		private bool m_wasUpdatedEachFrame;

		private MyUpgradableBlockComponent m_upgradeComponent;

		private Dictionary<string, float> m_upgradeValues;

		private MyStringHash m_skinSubtypeId;

		public virtual bool IsBeingHacked => false;

		public bool UsesEmissivePreset { get; private set; }

		protected static bool AllowExperimentalValues => MySession.Static.IsRunningExperimental;

		public virtual MyCubeBlockHighlightModes HighlightMode => MyCubeBlockHighlightModes.Default;

		public new MyPhysicsBody Physics
		{
			get
			{
				return base.Physics as MyPhysicsBody;
			}
			set
			{
				base.Physics = value;
			}
		}

		public long OwnerId
		{
			get
			{
				if (IDModule != null)
				{
					return IDModule.Owner;
				}
				return 0L;
			}
		}

		public long BuiltBy
		{
			get
			{
				if (SlimBlock != null)
				{
					return SlimBlock.BuiltBy;
				}
				return 0L;
			}
		}

		public MyResourceSinkComponent ResourceSink
		{
			get
			{
				return m_sinkComp;
			}
			protected set
			{
				if (ContainsDebugRenderComponent(typeof(MyDebugRenderComponentDrawPowerReciever)))
				{
					RemoveDebugRenderComponent(typeof(MyDebugRenderComponentDrawPowerReciever));
				}
				if (base.Components.Contains(typeof(MyResourceSinkComponent)))
				{
					base.Components.Remove<MyResourceSinkComponent>();
				}
				base.Components.Add(value);
				AddDebugRenderComponent(new MyDebugRenderComponentDrawPowerReciever(value, this));
				m_sinkComp = value;
			}
		}

		public MyCubeBlockDefinition BlockDefinition => SlimBlock.BlockDefinition;

		public Vector3I Min => SlimBlock.Min;

		public Vector3I Max => SlimBlock.Max;

		public MyBlockOrientation Orientation => SlimBlock.Orientation;

		public Vector3I Position => SlimBlock.Position;

		public MyCubeGrid CubeGrid => SlimBlock.CubeGrid;

		public MyUseObjectsComponentBase UseObjectsComponent => base.Components.Get<MyUseObjectsComponentBase>();

		public bool CheckConnectionAllowed
		{
			get
			{
				return m_checkConnectionAllowed;
			}
			set
			{
				m_checkConnectionAllowed = value;
				this.CheckConnectionChanged?.Invoke(this);
			}
		}

		public int NumberInGrid
		{
			get
			{
				return m_numberInGrid;
			}
			set
			{
				m_numberInGrid = value;
			}
		}

		/// <summary>
		/// Shortcut to component stack property.
		/// </summary>
		public bool IsFunctional => SlimBlock.ComponentStack.IsFunctional;

		public bool IsBuilt => SlimBlock.ComponentStack.IsBuilt;

		public virtual float DisassembleRatio => BlockDefinition.DisassembleRatio;

		public bool IsWorking { get; private set; }

		public MyIDModule IDModule => m_IDModule;

		public bool IsSubBlock => SubBlockName != null;

<<<<<<< HEAD
		/// <summary>
		/// Name of subblock (key in the owner's subblocks map).
		/// </summary>
		public string SubBlockName { get; internal set; }

		/// <summary>
		/// If the block is subblock then OwnerBlock is set to block which owns (spawns) subblocks (subgrids)
		/// </summary>
=======
		public string SubBlockName { get; internal set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MySlimBlock OwnerBlock { get; internal set; }

		public string DefinitionDisplayNameText => BlockDefinition.DisplayNameText;

		public bool ForceBlockDestructible
		{
			get
			{
				if (MyFakes.ENABLE_VR_FORCE_BLOCK_DESTRUCTIBLE)
				{
					return m_forceBlockDestructible;
				}
				return false;
			}
		}

		protected bool HasDamageEffect => m_damageEffect != null;

		public Dictionary<string, float> UpgradeValues
		{
			get
			{
				if (m_upgradeValues == null)
				{
					m_upgradeValues = new Dictionary<string, float>();
				}
				return m_upgradeValues;
			}
		}

		SerializableDefinitionId VRage.Game.ModAPI.Ingame.IMyCubeBlock.BlockDefinition => BlockDefinition.Id;

		VRage.Game.ModAPI.IMyCubeGrid VRage.Game.ModAPI.IMyCubeBlock.CubeGrid => CubeGrid;

		VRage.Game.ModAPI.Ingame.IMyCubeGrid VRage.Game.ModAPI.Ingame.IMyCubeBlock.CubeGrid => CubeGrid;

		bool VRage.Game.ModAPI.Ingame.IMyCubeBlock.CheckConnectionAllowed
		{
			get
			{
				return CheckConnectionAllowed;
			}
			set
			{
				CheckConnectionAllowed = value;
			}
		}

		float VRage.Game.ModAPI.Ingame.IMyCubeBlock.Mass => GetMass();

		VRage.Game.ModAPI.IMySlimBlock VRage.Game.ModAPI.IMyCubeBlock.SlimBlock => SlimBlock;

		uint Sandbox.ModAPI.Ingame.IMyUpgradableBlock.UpgradeCount => (uint)UpgradeValues.Count;

		MyResourceSinkComponentBase VRage.Game.ModAPI.IMyCubeBlock.ResourceSink
		{
			get
			{
				return ResourceSink;
			}
			set
			{
				ResourceSink = (MyResourceSinkComponent)value;
			}
		}

		public event Action<MyCubeBlock> CheckConnectionChanged;

		public event Action<MyCubeBlock> IsWorkingChanged;

		public event Func<bool> CanContinueBuildCheck;

		public event Action OnUpgradeValuesChanged;

		event Action<VRage.Game.ModAPI.IMyCubeBlock> VRage.Game.ModAPI.IMyCubeBlock.IsWorkingChanged
		{
			add
			{
				IsWorkingChanged += GetDelegate(value);
			}
			remove
			{
				IsWorkingChanged -= GetDelegate(value);
			}
		}

		public string GetOwnerFactionTag()
		{
			if (IDModule == null)
			{
				return "";
			}
			if (IDModule.Owner == 0L)
			{
				return "";
			}
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(IDModule.Owner);
			if (myFaction == null)
			{
				return "";
			}
			return myFaction.Tag;
		}

		public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long identityId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
		{
			if (!MyFakes.SHOW_FACTIONS_GUI)
			{
				return MyRelationsBetweenPlayerAndBlock.NoOwnership;
			}
			if (IDModule == null)
			{
				return MyRelationsBetweenPlayerAndBlock.NoOwnership;
			}
			return IDModule.GetUserRelationToOwner(identityId, defaultNoUser);
		}

		public MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner()
		{
			if (!MyFakes.SHOW_FACTIONS_GUI)
			{
				return MyRelationsBetweenPlayerAndBlock.NoOwnership;
			}
			if (IDModule == null)
			{
				return MyRelationsBetweenPlayerAndBlock.NoOwnership;
			}
			if (MySession.Static.LocalHumanPlayer != null)
			{
				return IDModule.GetUserRelationToOwner(MySession.Static.LocalHumanPlayer.Identity.IdentityId);
			}
			return MyRelationsBetweenPlayerAndBlock.Neutral;
		}

		/// <summary>
		/// Whether the two blocks are friendly. This relation is base on their owners and is symmetrical
		/// </summary>
		public bool FriendlyWithBlock(MyCubeBlock block)
		{
			if (GetUserRelationToOwner(block.OwnerId) == MyRelationsBetweenPlayerAndBlock.Enemies)
			{
				return false;
			}
			if (block.GetUserRelationToOwner(OwnerId) == MyRelationsBetweenPlayerAndBlock.Enemies)
			{
				return false;
			}
			return true;
		}

		public void UpdateIsWorking()
		{
			bool flag = CheckIsWorking();
			bool flag2 = flag != IsWorking;
			IsWorking = flag;
			if (flag2 && this.IsWorkingChanged != null)
			{
				this.IsWorkingChanged(this);
			}
			if (UsesEmissivePreset && flag2)
			{
				CheckEmissiveState();
			}
		}

		protected virtual bool CheckIsWorking()
		{
			return IsFunctional;
		}

		public bool CanContinueBuild()
		{
			if (this.CanContinueBuildCheck == null)
			{
				return true;
			}
			bool flag = true;
			Delegate[] invocationList = this.CanContinueBuildCheck.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				Func<bool> func = invocationList[i] as Func<bool>;
				flag &= func();
			}
			return flag;
		}

		public IMyUseObject GetInteractiveObject(uint shapeKey)
		{
			if (!IsFunctional)
			{
				return null;
			}
			return UseObjectsComponent.GetInteractiveObject(shapeKey);
		}

		public void ReleaseInventory(MyInventory inventory, bool damageContent = false)
		{
			if (inventory == null || !Sync.IsServer)
			{
				return;
			}
			MyEntityInventorySpawnComponent component = null;
			if (base.Components.TryGet<MyEntityInventorySpawnComponent>(out component))
			{
				component.SpawnInventoryContainer();
				MyInventory component2 = new MyInventory(inventory.MaxVolume, inventory.MaxMass, Vector3.One, inventory.GetFlags());
				base.Components.Add((MyInventoryBase)component2);
				return;
			}
			foreach (MyPhysicalInventoryItem item in inventory.GetItems())
			{
<<<<<<< HEAD
				if (m_shouldDetonateAmmo && (item.GetItemDefinition() is MyAmmoDefinition || item.GetItemDefinition() is MyAmmoMagazineDefinition || item.Content.SubtypeId == MyStringHash.GetOrCompute("Explosives")))
				{
					continue;
				}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyPhysicalInventoryItem inventoryItem = item;
				if (damageContent && item.Content.TypeId == typeof(MyObjectBuilder_Component))
				{
					inventoryItem.Amount *= (MyFixedPoint)MyDefinitionManager.Static.GetComponentDefinition(item.Content.GetId()).DropProbability;
					inventoryItem.Amount = MyFixedPoint.Floor(inventoryItem.Amount);
					if (inventoryItem.Amount == 0)
					{
						continue;
					}
				}
				MyFloatingObjects.EnqueueInventoryItemSpawn(inventoryItem, base.PositionComp.WorldAABB, (CubeGrid.Physics != null) ? CubeGrid.Physics.GetVelocityAtPoint(base.PositionComp.GetPosition()) : Vector3.Zero);
			}
			inventory.Clear();
		}

		/// <summary>
		/// Called by constraint owner
		/// </summary>
		protected virtual void OnConstraintAdded(GridLinkTypeEnum type, VRage.ModAPI.IMyEntity attachedEntity)
		{
			MyCubeGrid myCubeGrid = attachedEntity as MyCubeGrid;
			if (myCubeGrid != null && !MyCubeGridGroups.Static.GetGroups(type).LinkExists(base.EntityId, CubeGrid, myCubeGrid))
			{
				MyCubeGridGroups.Static.CreateLink(type, base.EntityId, CubeGrid, myCubeGrid);
			}
		}

		/// <summary>
		/// Called by constraint owner
		/// </summary>
		protected virtual void OnConstraintRemoved(GridLinkTypeEnum type, VRage.ModAPI.IMyEntity detachedEntity)
		{
			MyCubeGrid myCubeGrid = detachedEntity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyCubeGridGroups.Static.BreakLink(type, base.EntityId, CubeGrid, myCubeGrid);
			}
		}

		static MyCubeBlock()
		{
			DUMMY_SUBBLOCK_ID = "subblock_";
			m_tmpMountPoints = new List<MyCubeBlockDefinition.MountPoint>();
			m_tmpBlockMountPoints = new List<MyCubeBlockDefinition.MountPoint>();
			m_tmpOtherBlockMountPoints = new List<MyCubeBlockDefinition.MountPoint>();
			m_emissiveNames = new EmissiveNames(ignore: true);
			m_methodDataIsConnectedTo = new MethodDataIsConnectedTo();
		}

		public MyCubeBlock()
		{
			base.Render.ShadowBoxLod = true;
			base.NeedsWorldMatrix = false;
			base.InvalidateOnMove = false;
		}

		public override void InitComponents()
		{
			if (base.Render == null)
			{
				base.Render = new MyRenderComponentCubeBlock();
			}
			if (base.PositionComp == null)
			{
				base.PositionComp = new MyBlockPosComponent();
			}
			base.InitComponents();
		}

		public void Init()
		{
			base.PositionComp.LocalAABB = new BoundingBox(new Vector3((0f - SlimBlock.CubeGrid.GridSize) / 2f), new Vector3(SlimBlock.CubeGrid.GridSize / 2f));
			base.Components.Add((MyUseObjectsComponentBase)new MyUseObjectsComponent());
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			}
			if (BlockDefinition.CubeDefinition != null)
			{
				SlimBlock.Orientation = MyCubeGridDefinitions.GetTopologyUniqueOrientation(BlockDefinition.CubeDefinition.CubeTopology, Orientation);
			}
			CalcLocalMatrix(out var localMatrix, out var currModel);
			if (!string.IsNullOrEmpty(currModel))
			{
				Init(null, currModel, null, null);
				OnModelChange();
			}
			base.Render.EnableColorMaskHsv = true;
			base.Render.FadeIn = CubeGrid.Render.FadeIn;
			base.Render.SkipIfTooSmall = false;
			CheckConnectionAllowed = false;
			base.PositionComp.SetLocalMatrix(ref localMatrix, CubeGrid);
			base.Save = false;
			if (CubeGrid.CreatePhysics)
			{
				UseObjectsComponent.LoadDetectorsFromModel();
			}
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			if (base.Subparts == null || base.Subparts.Count <= 0)
			{
<<<<<<< HEAD
				bool flag = false;
				foreach (MyEntitySubpart value in base.Subparts.Values)
				{
					if ((!(value.Render is MyParentedSubpartRenderComponent) && value.InvalidateOnMove) || value.NeedsWorldMatrix)
					{
						flag = true;
						break;
					}
				}
				if (flag)
=======
				return;
			}
			bool flag = false;
			foreach (MyEntitySubpart value in base.Subparts.Values)
			{
				if ((!(value.Render is MyParentedSubpartRenderComponent) && value.InvalidateOnMove) || value.NeedsWorldMatrix)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					flag = true;
					break;
				}
			}
<<<<<<< HEAD
			if (DisplayNameText == null)
			{
				DisplayNameText = BlockDefinition.DisplayNameText;
=======
			if (flag)
			{
				base.NeedsWorldMatrix = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void GetLocalMatrix(out Matrix localMatrix)
		{
			SlimBlock.GetLocalMatrix(out localMatrix);
		}

		public void CalcLocalMatrix(out Matrix localMatrix, out string currModel)
		{
			GetLocalMatrix(out localMatrix);
			currModel = SlimBlock.CalculateCurrentModel(out var orientation);
			orientation.Translation = localMatrix.Translation;
			localMatrix = orientation;
		}

		public virtual void Init(MyObjectBuilder_CubeBlock builder, MyCubeGrid cubeGrid)
		{
			if (builder.EntityId == 0L)
			{
				base.EntityId = MyEntityIdentifier.AllocateId();
			}
			else if (builder.EntityId != 0L)
			{
				base.EntityId = builder.EntityId;
			}
			if (string.IsNullOrEmpty(builder.Name))
			{
<<<<<<< HEAD
				base.Name = base.EntityId.ToString();
			}
			else
			{
				base.Name = builder.Name;
=======
				Name = base.EntityId.ToString();
			}
			else
			{
				Name = builder.Name;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			NumberInGrid = cubeGrid.BlockCounter.GetNextNumber(builder.GetId());
			base.Render.ColorMaskHsv = builder.ColorMaskHSV;
			UpdateSkin();
			base.Render.FadeIn = cubeGrid.Render.FadeIn;
			if (MyFakes.ENABLE_SUBBLOCKS && BlockDefinition.SubBlockDefinitions != null && BlockDefinition.SubBlockDefinitions.Count > 0)
			{
				if (builder.SubBlocks != null && builder.SubBlocks.Length != 0)
				{
					m_loadedSubBlocks = new List<MyObjectBuilder_CubeBlock.MySubBlockId>();
					MyObjectBuilder_CubeBlock.MySubBlockId[] subBlocks = builder.SubBlocks;
					foreach (MyObjectBuilder_CubeBlock.MySubBlockId item in subBlocks)
					{
						m_loadedSubBlocks.Add(item);
					}
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
				else if (Sync.IsServer)
				{
					m_loadedSubBlocks = new List<MyObjectBuilder_CubeBlock.MySubBlockId>();
					SpawnSubBlocks();
					base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
			}
			UsesEmissivePreset = BlockDefinition.EmissiveColorPreset != MyStringHash.NullOrEmpty && MyEmissiveColorPresets.ContainsPreset(BlockDefinition.EmissiveColorPreset);
			base.Components.InitComponents(builder.TypeId, builder.SubtypeId, builder.ComponentContainer);
			base.Init(null);
			base.Render.PersistentFlags |= MyPersistentEntityFlags2.CastShadows;
			Init();
			AddDebugRenderComponent(new MyDebugRenderComponentCubeBlock(this));
			InitOwnership(builder);
			if (MyFakes.ENABLE_AMMO_DETONATION)
			{
				MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(builder.GetId());
				MyGameDefinition gameDefinition = MySession.Static.GameDefinition;
				m_detonationData = new DetonationData
				{
					DamageThreshold = cubeBlockDefinition.DamageThreshold,
					DetonateChance = cubeBlockDefinition.DetonateChance,
					ExplosionAmmoVolumeMin = gameDefinition.ExplosionAmmoVolumeMin,
					ExplosionAmmoVolumeMax = gameDefinition.ExplosionAmmoVolumeMax,
					ExplosionRadiusMin = gameDefinition.ExplosionRadiusMin,
					ExplosionRadiusMax = gameDefinition.ExplosionRadiusMax,
					ExplosionDamagePerLiter = gameDefinition.ExplosionDamagePerLiter,
					ExplosionDamageMax = gameDefinition.ExplosionDamageMax
				};
				MyInventory inventory = this.GetInventory();
				if (inventory != null)
				{
					inventory.InventoryContentChanged += CacheItem;
					CacheInventory(inventory);
				}
			}
		}

		protected override void Components_ComponentAdded(Type t, MyEntityComponentBase c)
		{
			base.Components_ComponentAdded(t, c);
			IMyGunObject<MyDeviceBase> gun;
			if ((gun = c as IMyGunObject<MyDeviceBase>) != null)
			{
				CubeGrid.GridSystems.WeaponSystem.Register(gun);
			}
		}

		protected override void Components_ComponentRemoved(Type t, MyEntityComponentBase c)
		{
			base.Components_ComponentRemoved(t, c);
			IMyGunObject<MyDeviceBase> gun;
			if ((gun = c as IMyGunObject<MyDeviceBase>) != null)
			{
				CubeGrid?.GridSystems?.WeaponSystem?.Unregister(gun);
			}
		}

		private void InitOwnership(MyObjectBuilder_CubeBlock builder)
		{
			MyEntityOwnershipComponent myEntityOwnershipComponent = base.Components.Get<MyEntityOwnershipComponent>();
			bool flag = BlockDefinition.ContainsComputer();
			if (UseObjectsComponent != null)
			{
				flag = flag || UseObjectsComponent.GetDetectors("ownership").Count > 0;
			}
			if (flag)
			{
				m_IDModule = new MyIDModule();
				if (MySession.Static.Settings.ResetOwnership && Sync.IsServer)
				{
					m_IDModule.Owner = 0L;
					m_IDModule.ShareMode = MyOwnershipShareModeEnum.None;
				}
				else
				{
					if (builder.ShareMode == (MyOwnershipShareModeEnum)(-1))
					{
						builder.ShareMode = MyOwnershipShareModeEnum.None;
					}
					MyEntityIdentifier.ID_OBJECT_TYPE idObjectType = MyEntityIdentifier.GetIdObjectType(builder.Owner);
					if (builder.Owner != 0L && idObjectType != MyEntityIdentifier.ID_OBJECT_TYPE.NPC && idObjectType != MyEntityIdentifier.ID_OBJECT_TYPE.SPAWN_GROUP && !Sync.Players.HasIdentity(builder.Owner))
					{
						builder.Owner = 0L;
					}
					m_IDModule.Owner = builder.Owner;
					m_IDModule.ShareMode = builder.ShareMode;
				}
			}
			if (myEntityOwnershipComponent != null && builder.Owner != 0L)
			{
				myEntityOwnershipComponent.OwnerId = builder.Owner;
				myEntityOwnershipComponent.ShareMode = MyOwnershipShareModeEnum.None;
			}
		}

		public sealed override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			return base.GetObjectBuilder(copy);
		}

		public virtual MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = MyCubeBlockFactory.CreateObjectBuilder(this);
			myObjectBuilder_CubeBlock.ColorMaskHSV = base.Render.ColorMaskHsv;
			myObjectBuilder_CubeBlock.SkinSubtypeId = m_skinSubtypeId.String;
			myObjectBuilder_CubeBlock.EntityId = base.EntityId;
			myObjectBuilder_CubeBlock.Min = Min;
			myObjectBuilder_CubeBlock.Owner = 0L;
			myObjectBuilder_CubeBlock.ShareMode = MyOwnershipShareModeEnum.None;
			myObjectBuilder_CubeBlock.Name = base.Name;
			if (m_IDModule != null)
			{
				myObjectBuilder_CubeBlock.Owner = m_IDModule.Owner;
				myObjectBuilder_CubeBlock.ShareMode = m_IDModule.ShareMode;
			}
			if (MyFakes.ENABLE_SUBBLOCKS && SubBlocks != null && SubBlocks.Count != 0)
			{
				myObjectBuilder_CubeBlock.SubBlocks = new MyObjectBuilder_CubeBlock.MySubBlockId[SubBlocks.Count];
				int num = 0;
				foreach (KeyValuePair<string, MySlimBlock> subBlock in SubBlocks)
				{
					myObjectBuilder_CubeBlock.SubBlocks[num].SubGridId = subBlock.Value.CubeGrid.EntityId;
					myObjectBuilder_CubeBlock.SubBlocks[num].SubGridName = subBlock.Key;
					myObjectBuilder_CubeBlock.SubBlocks[num].SubBlockPosition = subBlock.Value.Min;
					num++;
				}
			}
			myObjectBuilder_CubeBlock.ComponentContainer = base.Components.Serialize(copy);
			if (copy)
			{
				myObjectBuilder_CubeBlock.Name = null;
			}
			return myObjectBuilder_CubeBlock;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			UpdateIsWorking();
			if (UsesEmissivePreset)
			{
				CheckEmissiveState();
			}
			MyCubeBlockDefinition.BuildProgressModel[] buildProgressModels = BlockDefinition.BuildProgressModels;
			for (int i = 0; i < buildProgressModels.Length; i++)
			{
				MyRenderProxy.PreloadModel(buildProgressModels[i].File);
			}
			if (MyFakes.SHOW_DAMAGE_EFFECTS && CubeGrid.Physics != null && SlimBlock != null && !BlockDefinition.RatioEnoughForDamageEffect(SlimBlock.BuildIntegrity / SlimBlock.MaxIntegrity) && BlockDefinition.RatioEnoughForDamageEffect(SlimBlock.Integrity / SlimBlock.MaxIntegrity))
			{
				SetDamageEffect(show: true);
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			StopDamageEffect();
			base.OnRemovedFromScene(source);
		}

		/// <summary>
		/// Returns true if this block can connect to another block (of the given type) in the given position.
		/// This is called only if CheckConnectionAllowed == true.
		/// If this method would return true for any position, set CheckConnectionAllowed to false to avoid
		/// unnecessary overhead. It is the block's responsibility to call CubeGrid.UpdateBlockNeighbors every time the
		/// conditions that are checked by this method change.
		/// </summary>
		public virtual bool ConnectionAllowed(ref Vector3I otherBlockPos, ref Vector3I faceNormal, MyCubeBlockDefinition def)
		{
			if (MyFakes.ENABLE_FRACTURE_COMPONENT && base.Components.Has<MyFractureComponentBase>())
			{
				MyFractureComponentCubeBlock fractureComponent = GetFractureComponent();
				if (fractureComponent == null || fractureComponent.MountPoints == null)
				{
					return true;
				}
				m_tmpBlockMountPoints.Clear();
				MyCubeGrid.TransformMountPoints(m_tmpBlockMountPoints, BlockDefinition, fractureComponent.MountPoints, ref SlimBlock.Orientation);
				MySlimBlock cubeBlock = CubeGrid.GetCubeBlock(otherBlockPos);
				if (cubeBlock == null)
				{
					return true;
				}
				Vector3I positionA = Position;
				m_tmpMountPoints.Clear();
				if (cubeBlock.FatBlock is MyCompoundCubeBlock)
				{
					foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
					{
						MyFractureComponentCubeBlock fractureComponent2 = block.GetFractureComponent();
						MyCubeBlockDefinition.MountPoint[] array = null;
						array = ((fractureComponent2 == null) ? block.BlockDefinition.GetBuildProgressModelMountPoints(block.BuildLevelRatio) : fractureComponent2.MountPoints);
						m_tmpOtherBlockMountPoints.Clear();
						MyCubeGrid.TransformMountPoints(m_tmpOtherBlockMountPoints, block.BlockDefinition, array, ref block.Orientation);
						m_tmpMountPoints.AddRange(m_tmpOtherBlockMountPoints);
					}
				}
				else
				{
					MyCubeBlockDefinition.MountPoint[] array2 = null;
					MyFractureComponentCubeBlock fractureComponent3 = cubeBlock.GetFractureComponent();
					MyCubeGrid.TransformMountPoints(mountPoints: (fractureComponent3 == null) ? def.GetBuildProgressModelMountPoints(cubeBlock.BuildLevelRatio) : fractureComponent3.MountPoints, outMountPoints: m_tmpMountPoints, def: def, orientation: ref cubeBlock.Orientation);
				}
				bool result = MyCubeGrid.CheckMountPointsForSide(m_tmpBlockMountPoints, ref SlimBlock.Orientation, ref positionA, BlockDefinition.Id, ref faceNormal, m_tmpMountPoints, ref cubeBlock.Orientation, ref otherBlockPos, def.Id);
				m_tmpMountPoints.Clear();
				m_tmpBlockMountPoints.Clear();
				m_tmpOtherBlockMountPoints.Clear();
				return result;
			}
			return true;
		}

		/// <summary>
		/// Whether connection is allowed to any of the positions between otherBlockMinPos and otherBlockMaxPos (both inclusive).
		/// Default implementation calls ConnectionAllowed(ref Vector3I otherBlockPos, ref Vector3I faceNormal) in a for loop.
		/// Override this in a subclass if this is not needed (for example, because all calls would return the same value for the same face)
		/// </summary>
		public virtual bool ConnectionAllowed(ref Vector3I otherBlockMinPos, ref Vector3I otherBlockMaxPos, ref Vector3I faceNormal, MyCubeBlockDefinition def)
		{
			Vector3I otherBlockPos = otherBlockMinPos;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref otherBlockMinPos, ref otherBlockMaxPos);
			while (vector3I_RangeIterator.IsValid())
			{
				if (ConnectionAllowed(ref otherBlockPos, ref faceNormal, def))
				{
					return true;
				}
				vector3I_RangeIterator.GetNext(out otherBlockPos);
			}
			return false;
		}

		protected virtual void WorldPositionChanged(object source)
		{
		}

		protected override void Closing()
		{
			if (UseObjectsComponent.DetectorPhysics != null)
			{
				UseObjectsComponent.ClearPhysics();
			}
			if (MyFakes.ENABLE_SUBBLOCKS && SubBlocks != null)
			{
				foreach (KeyValuePair<string, MySlimBlock> subBlock in SubBlocks)
				{
					MySlimBlock value = subBlock.Value;
					if (value.FatBlock != null)
					{
						value.FatBlock.OwnerBlock = null;
						value.FatBlock.SubBlockName = null;
						value.FatBlock.OnClosing -= SubBlock_OnClosing;
					}
				}
			}
			SetDamageEffect(show: false);
			if (SlimBlock != null)
			{
				SlimBlock.CleanUp();
			}
			base.Closing();
		}

		public virtual bool SetEmissiveStateWorking()
		{
			if (base.Render != null && base.Render.RenderObjectIDs.Length != 0)
			{
				return SetEmissiveState(m_emissiveNames.Working, base.Render.RenderObjectIDs[0]);
			}
			return false;
		}

		public virtual bool SetEmissiveStateDisabled()
		{
			if (base.Render != null && base.Render.RenderObjectIDs.Length != 0)
			{
				return SetEmissiveState(m_emissiveNames.Disabled, base.Render.RenderObjectIDs[0]);
			}
			return false;
		}

		public virtual bool SetEmissiveStateDamaged()
		{
			if (base.Render != null && base.Render.RenderObjectIDs.Length != 0)
			{
				return SetEmissiveState(m_emissiveNames.Damaged, base.Render.RenderObjectIDs[0]);
			}
			return false;
		}

		public virtual void CheckEmissiveState(bool force = false)
		{
			if (IsWorking)
			{
				SetEmissiveStateWorking();
			}
			else if (IsFunctional)
			{
				SetEmissiveStateDisabled();
			}
			else
			{
				SetEmissiveStateDamaged();
			}
		}

		public bool SetEmissiveState(MyStringHash state, uint renderObjectId, string namedPart = null)
		{
			if (renderObjectId != uint.MaxValue && MyEmissiveColorPresets.LoadPresetState(BlockDefinition.EmissiveColorPreset, state, out var result))
			{
				if (string.IsNullOrEmpty(namedPart))
				{
					byte b = 0;
					while (true)
					{
						string defaultEmissiveParts = GetDefaultEmissiveParts(b);
						if (string.IsNullOrEmpty(defaultEmissiveParts))
						{
							break;
						}
						MyEntity.UpdateNamedEmissiveParts(renderObjectId, defaultEmissiveParts, result.EmissiveColor, result.Emissivity);
						b = (byte)(b + 1);
					}
				}
				else
				{
					MyEntity.UpdateNamedEmissiveParts(renderObjectId, namedPart, result.EmissiveColor, result.Emissivity);
				}
				return true;
			}
			return false;
		}

		public static void UpdateEmissiveParts(uint renderObjectId, float emissivity, Color emissivePartColor, Color displayPartColor)
		{
			if (renderObjectId != uint.MaxValue)
			{
				MyEntity.UpdateNamedEmissiveParts(renderObjectId, "Emissive", emissivePartColor, emissivity);
				MyEntity.UpdateNamedEmissiveParts(renderObjectId, "Display", displayPartColor, emissivity);
			}
		}

		protected virtual string GetDefaultEmissiveParts(byte index)
		{
			return index switch
			{
				0 => "Emissive", 
				1 => "Display", 
				_ => null, 
			};
		}

		private bool UpdateSkin()
		{
			bool num = m_skinSubtypeId != SlimBlock.SkinSubtypeId;
			if (num)
			{
				m_skinSubtypeId = SlimBlock.SkinSubtypeId;
				MyDefinitionManager.MyAssetModifiers myAssetModifiers = default(MyDefinitionManager.MyAssetModifiers);
				if (m_skinSubtypeId != MyStringHash.NullOrEmpty)
				{
					myAssetModifiers = MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(m_skinSubtypeId);
				}
				base.Render.TextureChanges = myAssetModifiers.SkinTextureChanges;
				base.Render.MetalnessColorable = myAssetModifiers.MetalnessColorable;
			}
			MyAssetModifierDefinition assetModifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId(typeof(MyObjectBuilder_AssetModifierDefinition), m_skinSubtypeId));
			if (assetModifierDefinition != null && assetModifierDefinition.DefaultColor.HasValue)
			{
				SlimBlock.ColorMaskHSV = assetModifierDefinition.DefaultColor.Value.ColorToHSVDX11();
				base.Render.ColorMaskHsv = SlimBlock.ColorMaskHSV;
			}
			return num;
		}

		public virtual void UpdateVisual()
		{
			bool flag = UpdateSkin();
			Matrix orientation;
			string text = SlimBlock.CalculateCurrentModel(out orientation);
			bool flag2 = base.Model != null && base.Model.AssetName != text;
			if (flag2 || base.Render.ColorMaskHsv != SlimBlock.ColorMaskHSV || flag || base.Render.Transparency != SlimBlock.Dithering)
			{
				base.Render.ColorMaskHsv = SlimBlock.ColorMaskHSV;
				m_skinSubtypeId = SlimBlock.SkinSubtypeId;
				MyDefinitionManager.MyAssetModifiers myAssetModifiers = default(MyDefinitionManager.MyAssetModifiers);
				if (m_skinSubtypeId != MyStringHash.NullOrEmpty)
				{
					myAssetModifiers = MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(m_skinSubtypeId);
				}
				base.Render.TextureChanges = myAssetModifiers.SkinTextureChanges;
				base.Render.MetalnessColorable = myAssetModifiers.MetalnessColorable;
				base.Render.Transparency = SlimBlock.Dithering;
				Vector3D translation = base.WorldMatrix.Translation;
				MatrixD worldMatrix = orientation * CubeGrid.WorldMatrix;
				worldMatrix.Translation = translation;
				base.PositionComp.SetWorldMatrix(ref worldMatrix, null, forceUpdate: true);
				RefreshModels(text, null);
				base.Render.RemoveRenderObjects();
				base.Render.AddRenderObjects();
				if (CubeGrid.CreatePhysics && flag2)
				{
					UseObjectsComponent.LoadDetectorsFromModel();
				}
				OnModelChange();
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (MyFakes.ENABLE_SUBBLOCKS && m_loadedSubBlocks != null)
			{
				InitSubBlocks();
			}
			if (m_setDamagedEffectDelayed.HasValue)
			{
				SetDamageEffect(m_setDamagedEffectDelayed.Value);
				m_setDamagedEffectDelayed = null;
			}
		}

		public override void UpdateBeforeSimulation10()
		{
			base.UpdateBeforeSimulation10();
			if (MyFakes.ENABLE_SUBBLOCKS && m_loadedSubBlocks != null)
			{
				InitSubBlocks();
			}
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Update();
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			if (m_activeEffects == null || !MyPerGameSettings.UseNewDamageEffects)
			{
				return;
			}
			for (int i = 0; i < m_activeEffects.Count; i++)
			{
				if (m_activeEffects[i].CanBeDeleted)
				{
					m_activeEffects[i].Stop();
					m_activeEffects.RemoveAt(i);
					i--;
				}
				else
				{
					m_activeEffects[i].Update();
				}
			}
		}

		/// <summary>
		/// Method called when a block has been built (after adding to the grid).
		/// This is called right after placing the block and it doesn't matter whether
		/// it is fully built (creative mode) or is only construction site.
		/// Note that it is not called for blocks which do not create FatBlock at that moment.
		/// </summary>
		public virtual void OnBuildSuccess(long builtBy, bool instantBuild)
		{
		}

		/// <summary>
		/// Method called when user removes a cube block from grid. Useful when block
		/// has to remove some other attached block (like motors).
		/// </summary>
		public virtual void OnRemovedByCubeBuilder()
		{
			SetFadeOut(state: false);
			if (MyFakes.ENABLE_SUBBLOCKS && SubBlocks != null)
			{
				foreach (KeyValuePair<string, MySlimBlock> subBlock in SubBlocks)
				{
					MySlimBlock value = subBlock.Value;
					value.CubeGrid.RemoveBlock(value, updatePhysics: true);
				}
			}
			SetDamageEffect(show: false);
			UnsubscribeEvents();
		}

		/// <summary>
		/// Called at the end of registration from grid systems (after block has been registered).
		/// </summary>
		public virtual void OnRegisteredToGridSystems()
		{
			if (m_upgradeComponent != null)
			{
				m_upgradeComponent.Refresh(this);
			}
		}

		/// <summary>
		/// Called at the end of unregistration from grid systems (after block has been unregistered).
		/// </summary>
		public virtual void OnUnregisteredFromGridSystems()
		{
		}

		/// <summary>
		/// Return true when contact is valid
		/// </summary>
		public virtual void ContactPointCallback(ref MyGridContactInfo value)
		{
		}

		/// <summary>
		/// Called when block is destroyed before being removed from grid
		/// </summary>
		public virtual void OnDestroy()
		{
			SetDamageEffect(show: false);
			if (MyFakes.ENABLE_AMMO_DETONATION && m_shouldDetonateAmmo && m_detonationData != null && m_detonationData.CachedAmmoMass > 0f && m_detonationData.CachedAmmoVolume > 0f)
			{
				DetonateAmmo();
				UnsubscribeEvents();
			}
			else
			{
				ReleaseInventory(this.GetInventory(), damageContent: true);
			}
		}

		/// <summary>
		/// Called when the model referred by the block is changed
		/// </summary>
		public virtual void OnModelChange()
		{
			if (UsesEmissivePreset)
			{
				CheckEmissiveState(force: true);
			}
		}

		public virtual string CalculateCurrentModel(out Matrix orientation)
		{
			Orientation.GetMatrix(out orientation);
			return BlockDefinition.Model;
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			UpdateIsWorking();
			CubeGrid.UpdateOwnership(OwnerId, IsFunctional);
			if (UsesEmissivePreset)
			{
				CheckEmissiveState();
			}
			if (MyVisualScriptLogicProvider.BlockFunctionalityChanged != null)
			{
				BlockFunctionalityChangedEvent blockFunctionalityChanged = MyVisualScriptLogicProvider.BlockFunctionalityChanged;
				long entityId = base.EntityId;
				long entityId2 = CubeGrid.EntityId;
				string name = base.Name;
				string name2 = CubeGrid.Name;
				MyObjectBuilderType typeId = SlimBlock.BlockDefinition.Id.TypeId;
				blockFunctionalityChanged(entityId, entityId2, name, name2, typeId.ToString(), SlimBlock.BlockDefinition.Id.SubtypeName, IsFunctional);
			}
		}

		internal virtual void OnIntegrityChanged(float buildIntegrity, float integrity, bool setOwnership, long owner, MyOwnershipShareModeEnum sharing = MyOwnershipShareModeEnum.Faction)
		{
			if (!BlockDefinition.ContainsComputer())
			{
				return;
			}
			MyEntityOwnershipComponent myEntityOwnershipComponent = base.Components.Get<MyEntityOwnershipComponent>();
			if (setOwnership)
			{
				if (m_IDModule.Owner == 0L && Sync.IsServer)
				{
					CubeGrid.ChangeOwnerRequest(CubeGrid, this, owner, sharing);
				}
				if (myEntityOwnershipComponent != null && myEntityOwnershipComponent.OwnerId == 0L && Sync.IsServer)
				{
					CubeGrid.ChangeOwnerRequest(CubeGrid, this, owner, sharing);
				}
				return;
			}
			if (m_IDModule.Owner != 0L && Sync.IsServer)
			{
				sharing = MyOwnershipShareModeEnum.None;
				CubeGrid.ChangeOwnerRequest(CubeGrid, this, 0L, sharing);
			}
			if (myEntityOwnershipComponent != null && myEntityOwnershipComponent.OwnerId != 0L && Sync.IsServer)
			{
				sharing = MyOwnershipShareModeEnum.None;
				CubeGrid.ChangeOwnerRequest(CubeGrid, this, 0L, sharing);
			}
		}

		public void ChangeBlockOwnerRequest(long playerId, MyOwnershipShareModeEnum shareMode)
		{
			CubeGrid.ChangeOwnerRequest(CubeGrid, this, playerId, shareMode);
		}

		public bool SetEffect(string effectName, bool stopPrevious = false)
		{
			return SetEffect(effectName, 0f, stopPrevious, ignoreParameter: true);
		}

		public bool SetEffect(string effectName, float parameter, bool stopPrevious = false, bool ignoreParameter = false, bool removeSameNameEffects = false)
		{
			if (BlockDefinition == null || BlockDefinition.Effects == null)
			{
				return false;
			}
			int num = -1;
			for (int i = 0; i < BlockDefinition.Effects.Length; i++)
			{
				if (effectName.Equals(BlockDefinition.Effects[i].Name) && (ignoreParameter || (parameter >= BlockDefinition.Effects[i].ParameterMin && parameter <= BlockDefinition.Effects[i].ParameterMax)))
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return false;
			}
			if (m_activeEffects == null)
			{
				m_activeEffects = new List<MyCubeBlockEffect>();
			}
			for (int i = 0; i < m_activeEffects.Count; i++)
			{
				if (m_activeEffects[i].EffectId == num)
				{
					if (stopPrevious)
					{
						m_activeEffects[i].Stop();
						m_activeEffects.RemoveAt(i);
						break;
					}
					return false;
				}
			}
			if (removeSameNameEffects)
			{
				RemoveEffect(effectName, num);
			}
			if (m_activeEffects.Count == 0)
			{
				m_wasUpdatedEachFrame = (base.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) != 0;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
			m_activeEffects.Add(new MyCubeBlockEffect(num, BlockDefinition.Effects[num], this));
			return true;
		}

		public int RemoveEffect(string effectName, int exception = -1)
		{
			if (BlockDefinition == null || BlockDefinition.Effects == null || m_activeEffects == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < BlockDefinition.Effects.Length; i++)
			{
				if (!effectName.Equals(BlockDefinition.Effects[i].Name))
				{
					continue;
				}
				for (int j = 0; j < m_activeEffects.Count; j++)
				{
					if (m_activeEffects[j].EffectId == i && i != exception)
					{
						m_activeEffects[j].Stop();
						m_activeEffects.RemoveAt(j);
						num++;
					}
				}
			}
			if (m_activeEffects.Count == 0 && !m_wasUpdatedEachFrame)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
			return num;
		}

		public virtual void SetDamageEffectDelayed(bool show)
		{
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			m_setDamagedEffectDelayed = true;
		}

		public virtual void SetDamageEffect(bool show)
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			if (MyPerGameSettings.UseNewDamageEffects && show)
			{
				SetEffect("Damage", SlimBlock.Integrity / SlimBlock.MaxIntegrity, stopPrevious: false, ignoreParameter: false, removeSameNameEffects: true);
			}
			bool flag = m_activeEffects != null && MyPerGameSettings.UseNewDamageEffects && m_activeEffects.Count > 0;
			if (MyPerGameSettings.UseNewDamageEffects && !show)
			{
				RemoveEffect("Damage");
			}
			if (MyFakes.SHOW_DAMAGE_EFFECTS && (!string.IsNullOrEmpty(BlockDefinition.DamageEffectName) || BlockDefinition.DamageEffectID.HasValue))
			{
				if (!show && m_damageEffect != null)
				{
					m_damageEffect.Stop(instant: false);
					m_damageEffect.StopLights();
					if (CubeGrid.Physics != null)
					{
						m_damageEffect.Velocity = CubeGrid.Physics.LinearVelocity;
					}
					m_damageEffect.OnDelete -= OnDamageEffectDeleted;
					m_damageEffect = null;
				}
<<<<<<< HEAD
				if (show && m_damageEffect == null && !flag && MySandboxGame.Static.EnableDamageEffects)
=======
				m_damageEffect.OnDelete -= OnDamageEffectDeleted;
				m_damageEffect = null;
			}
			if (!show || m_damageEffect != null || flag || !MySandboxGame.Static.EnableDamageEffects)
			{
				return;
			}
			string text = BlockDefinition.DamageEffectName;
			if (string.IsNullOrEmpty(text) && BlockDefinition.DamageEffectID.HasValue)
			{
				MyParticleEffectsLibrary.GetById().TryGetValue(BlockDefinition.DamageEffectID.Value, out var value);
				if (value != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					string text = BlockDefinition.DamageEffectName;
					if (string.IsNullOrEmpty(text) && BlockDefinition.DamageEffectID.HasValue)
					{
						MyParticleEffectsLibrary.GetById().TryGetValue(BlockDefinition.DamageEffectID.Value, out var value);
						if (value != null)
						{
							text = value.Name;
						}
					}
					MatrixD effectMatrix = GetDamageLocalMatrix();
					Vector3D worldPosition = base.PositionComp.WorldMatrixRef.Translation;
					if (MyParticlesManager.TryCreateParticleEffect(text, ref effectMatrix, ref worldPosition, base.Render.ParentIDs[0], out m_damageEffect))
					{
						m_damageEffect.UserScale = base.Model.BoundingBox.Perimeter * 0.018f;
						m_damageEffect.OnDelete += OnDamageEffectDeleted;
					}
				}
			}
<<<<<<< HEAD
			if (BlockDefinition.DamagedSound != null && m_soundEmitter != null)
			{
				if (show)
				{
					m_soundEmitter.PlaySound(BlockDefinition.DamagedSound, stopPrevious: true);
				}
				else if (m_soundEmitter.SoundId == BlockDefinition.DamagedSound.SoundId)
				{
					m_soundEmitter.StopSound(forced: false);
				}
=======
			MatrixD effectMatrix = GetDamageLocalMatrix();
			Vector3D worldPosition = base.PositionComp.WorldMatrixRef.Translation;
			if (MyParticlesManager.TryCreateParticleEffect(text, ref effectMatrix, ref worldPosition, base.Render.ParentIDs[0], out m_damageEffect))
			{
				m_damageEffect.UserScale = base.Model.BoundingBox.Perimeter * 0.018f;
				m_damageEffect.OnDelete += OnDamageEffectDeleted;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public virtual void StopDamageEffect(bool stopSound = true)
		{
			if (MyPerGameSettings.UseNewDamageEffects)
			{
				RemoveEffect("Damage");
			}
			if (MyFakes.SHOW_DAMAGE_EFFECTS && (!string.IsNullOrEmpty(BlockDefinition.DamageEffectName) || BlockDefinition.DamageEffectID.HasValue) && m_damageEffect != null)
			{
				m_damageEffect.StopEmitting(10f);
				m_damageEffect.StopLights();
				if (CubeGrid.Physics != null)
				{
					m_damageEffect.Velocity = CubeGrid.Physics.LinearVelocity;
				}
				m_damageEffect.OnDelete -= OnDamageEffectDeleted;
				m_damageEffect = null;
			}
			if (stopSound && m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: false);
			}
		}

		private MatrixD GetDamageWorldMatrix()
		{
			return MatrixD.CreateTranslation(0.85f * base.PositionComp.LocalVolume.Center) * base.WorldMatrix;
		}

		private MatrixD GetDamageLocalMatrix()
		{
			MatrixD matrixD = MatrixD.CreateTranslation(0.85f * base.PositionComp.LocalVolume.Center);
			if (BlockDefinition.DamageEffectOffset.HasValue && !BlockDefinition.DamageEffectOffset.Value.Equals(Vector3.Zero))
			{
				matrixD.Translation += BlockDefinition.DamageEffectOffset.Value;
			}
			if (base.PositionComp == null)
			{
				return matrixD;
			}
			return matrixD * base.PositionComp.LocalMatrixRef;
		}

		private void OnDamageEffectDeleted(MyParticleEffect effect)
		{
			if (effect == m_damageEffect)
			{
				SetDamageEffect(show: false);
			}
		}

		public void ChangeOwner(long owner, MyOwnershipShareModeEnum shareMode)
		{
			MyEntityOwnershipComponent myEntityOwnershipComponent = base.Components.Get<MyEntityOwnershipComponent>();
			if (myEntityOwnershipComponent != null)
			{
				if (owner != myEntityOwnershipComponent.OwnerId || shareMode != myEntityOwnershipComponent.ShareMode)
				{
					long ownerId = myEntityOwnershipComponent.OwnerId;
					myEntityOwnershipComponent.OwnerId = owner;
					myEntityOwnershipComponent.ShareMode = shareMode;
					if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
					{
						CubeGrid.ChangeOwner(this, ownerId, owner);
					}
					OnOwnershipChanged();
				}
			}
			else if (IDModule != null && (owner != m_IDModule.Owner || shareMode != m_IDModule.ShareMode))
			{
				long owner2 = m_IDModule.Owner;
				m_IDModule.Owner = owner;
				m_IDModule.ShareMode = shareMode;
				if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
				{
					CubeGrid.ChangeOwner(this, owner2, owner);
				}
				OnOwnershipChanged();
			}
		}

		protected virtual void OnOwnershipChanged()
		{
		}

		bool IMyComponentOwner<MyIDModule>.GetComponent(out MyIDModule component)
		{
			component = m_IDModule;
			if (m_IDModule != null)
			{
				return IsFunctional;
			}
			return false;
		}

		/// <summary>
		/// Notifies about grid change with old grid in parameter (new grid is available in property).
		/// </summary>
		public virtual void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			if (MyFakes.ENABLE_FRACTURE_COMPONENT && base.Components.Has<MyFractureComponentBase>())
			{
				GetFractureComponent()?.OnCubeGridChanged();
			}
		}

		public virtual void OnTeleport()
		{
		}

		internal virtual void OnAddedNeighbours()
		{
		}

		internal virtual void OnRemovedNeighbours()
		{
		}

		internal virtual void OnTransformed(ref MatrixI transform)
		{
		}

		internal virtual void UpdateWorldMatrix()
		{
			GetLocalMatrix(out var localMatrix);
			MatrixD worldMatrix = localMatrix;
			base.PositionComp.SetWorldMatrix(ref worldMatrix, null, forceUpdate: true);
		}

		private void InitSubBlocks()
		{
			if (!MyFakes.ENABLE_SUBBLOCKS || m_loadedSubBlocks == null)
			{
				return;
			}
			bool flag = AllSubBlocksInitialized();
			bool flag2 = m_loadedSubBlocks.Count == 0 && Sync.IsServer && flag;
			if (!flag)
			{
				for (int num = m_loadedSubBlocks.Count - 1; num >= 0; num--)
				{
					MyObjectBuilder_CubeBlock.MySubBlockId mySubBlockId = m_loadedSubBlocks[num];
					if (MyEntities.TryGetEntityById(mySubBlockId.SubGridId, out var entity))
					{
						MyCubeGrid myCubeGrid = entity as MyCubeGrid;
						if (myCubeGrid != null)
						{
							MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(mySubBlockId.SubBlockPosition);
							if (cubeBlock != null)
							{
								AddSubBlock(mySubBlockId.SubGridName, cubeBlock);
							}
						}
						m_loadedSubBlocks.RemoveAt(num);
					}
				}
			}
			if (AllSubBlocksInitialized())
			{
				m_loadedSubBlocks = null;
				if (flag2 || !flag)
				{
					SubBlocksInitialized(flag2);
				}
			}
		}

		protected bool AllSubBlocksInitialized()
		{
			if (BlockDefinition.SubBlockDefinitions == null || BlockDefinition.SubBlockDefinitions.Count == 0)
			{
				return false;
			}
			if (SubBlocks != null && SubBlocks.Count != 0)
			{
				if (SubBlocks.Count != BlockDefinition.SubBlockDefinitions.Count && m_loadedSubBlocks != null)
				{
					return m_loadedSubBlocks.Count == 0;
				}
				return true;
			}
			return false;
		}

		protected void AddSubBlock(string dummyName, MySlimBlock subblock)
		{
			if (SubBlocks == null)
			{
				SubBlocks = new Dictionary<string, MySlimBlock>();
			}
			if (SubBlocks.TryGetValue(dummyName, out var value))
			{
				if (subblock == value)
				{
					return;
				}
				RemoveSubBlock(dummyName, removeFromGrid: false);
			}
			SubBlocks.Add(dummyName, subblock);
			subblock.FatBlock.SubBlockName = dummyName;
			subblock.FatBlock.OwnerBlock = SlimBlock;
			subblock.FatBlock.OnClosing += SubBlock_OnClosing;
		}

		private void SpawnSubBlocks()
		{
			if (!MyFakes.ENABLE_SUBBLOCKS || !CubeGrid.CreatePhysics)
<<<<<<< HEAD
			{
				return;
			}
			foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(BlockDefinition.Model).Dummies)
			{
				if (!GetSubBlockDataFromDummy(BlockDefinition, dummy.Key, dummy.Value, useOffset: true, out var subBlockDefinition, out var subBlockMatrix, out var _))
				{
					continue;
				}
				string dummyName = dummy.Key.Substring(DUMMY_SUBBLOCK_ID.Length);
				GetLocalMatrix(out var localMatrix);
				MatrixD m = subBlockMatrix * localMatrix * CubeGrid.WorldMatrix;
				Matrix m2 = m;
				MySlimBlock mySlimBlock = null;
				MyCubeGrid myCubeGrid = MyCubeBuilder.SpawnDynamicGrid(subBlockDefinition, null, m2, new Vector3(0f, -1f, 0f), MyStringHash.NullOrEmpty, 0L, MyCubeBuilder.SpawnFlags.Default, 0L);
				if (myCubeGrid != null)
				{
=======
			{
				return;
			}
			foreach (KeyValuePair<string, MyModelDummy> dummy in MyModels.GetModelOnlyDummies(BlockDefinition.Model).Dummies)
			{
				if (!GetSubBlockDataFromDummy(BlockDefinition, dummy.Key, dummy.Value, useOffset: true, out var subBlockDefinition, out var subBlockMatrix, out var _))
				{
					continue;
				}
				string dummyName = dummy.Key.Substring(DUMMY_SUBBLOCK_ID.Length);
				GetLocalMatrix(out var localMatrix);
				MatrixD m = subBlockMatrix * localMatrix * CubeGrid.WorldMatrix;
				Matrix m2 = m;
				MySlimBlock mySlimBlock = null;
				MyCubeGrid myCubeGrid = MyCubeBuilder.SpawnDynamicGrid(subBlockDefinition, null, m2, new Vector3(0f, -1f, 0f), MyStringHash.NullOrEmpty, 0L, MyCubeBuilder.SpawnFlags.Default, 0L);
				if (myCubeGrid != null)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					mySlimBlock = myCubeGrid.GetCubeBlock(Vector3I.Zero);
					if (mySlimBlock != null && mySlimBlock.FatBlock != null)
					{
						AddSubBlock(dummyName, mySlimBlock);
					}
				}
			}
		}

		/// <summary>
		/// Function called when all subblocks have been initialized.
		/// </summary>
		/// <param name="spawned">true if subblocks have been just spawned on server, otherwise false (load)</param>
		protected virtual void SubBlocksInitialized(bool spawned)
		{
		}

		protected virtual void OnSubBlockClosing(MySlimBlock subBlock)
		{
			subBlock.FatBlock.OnClosing -= SubBlock_OnClosing;
			if (SubBlocks != null)
			{
				SubBlocks.Remove(subBlock.FatBlock.SubBlockName);
			}
		}

		private void SubBlock_OnClosing(MyEntity obj)
		{
			MyCubeBlock subblock = obj as MyCubeBlock;
			if (subblock != null)
			{
				KeyValuePair<string, MySlimBlock> keyValuePair = Enumerable.FirstOrDefault<KeyValuePair<string, MySlimBlock>>((IEnumerable<KeyValuePair<string, MySlimBlock>>)SubBlocks, (Func<KeyValuePair<string, MySlimBlock>, bool>)((KeyValuePair<string, MySlimBlock> p) => p.Value == subblock.SlimBlock));
				if (keyValuePair.Value != null)
				{
					OnSubBlockClosing(keyValuePair.Value);
				}
			}
		}

		/// <summary>
		/// Removes subblock with the given name from the block. 
		/// </summary>
		protected bool RemoveSubBlock(string subBlockName, bool removeFromGrid = true)
		{
			if (SubBlocks == null)
			{
				return false;
			}
			if (SubBlocks.TryGetValue(subBlockName, out var value))
			{
				if (removeFromGrid)
				{
					value.CubeGrid.RemoveBlock(value, updatePhysics: true);
				}
				if (SubBlocks.Remove(subBlockName))
				{
					if (value.FatBlock != null)
					{
						value.FatBlock.OwnerBlock = null;
						value.FatBlock.SubBlockName = null;
					}
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns block offset in spawned grid.
		/// </summary>
		public static Vector3 GetBlockGridOffset(MyCubeBlockDefinition blockDefinition)
		{
			float cubeSize = MyDefinitionManager.Static.GetCubeSize(blockDefinition.CubeSize);
			Vector3 zero = Vector3.Zero;
			if (blockDefinition.Size.X % 2 == 0)
			{
				zero.X = cubeSize / 2f;
			}
			if (blockDefinition.Size.Y % 2 == 0)
			{
				zero.Y = cubeSize / 2f;
			}
			if (blockDefinition.Size.Z % 2 == 0)
			{
				zero.Z = cubeSize / 2f;
			}
			return zero;
		}

		/// <summary>
		/// Returns subblock data from dummy, subblock matrix can be offset (according to useOffset parameter) so the dummy position output is also provided.
		/// </summary>
		/// <returns>true when dummy is subblock otherwise false</returns>
		public static bool GetSubBlockDataFromDummy(MyCubeBlockDefinition ownerBlockDefinition, string dummyName, MyModelDummy dummy, bool useOffset, out MyCubeBlockDefinition subBlockDefinition, out MatrixD subBlockMatrix, out Vector3 dummyPosition)
		{
			subBlockDefinition = null;
			subBlockMatrix = MatrixD.Identity;
			dummyPosition = Vector3.Zero;
			if (!dummyName.ToLower().StartsWith(DUMMY_SUBBLOCK_ID))
			{
				return false;
			}
			if (ownerBlockDefinition.SubBlockDefinitions == null)
			{
				return false;
			}
			string key = dummyName.Substring(DUMMY_SUBBLOCK_ID.Length);
			if (!ownerBlockDefinition.SubBlockDefinitions.TryGetValue(key, out var value))
			{
				return false;
			}
			MyDefinitionManager.Static.TryGetCubeBlockDefinition(value, out subBlockDefinition);
			if (subBlockDefinition == null)
			{
				return false;
			}
			subBlockMatrix = MatrixD.Normalize(dummy.Matrix);
			Vector3I intVector = Base6Directions.GetIntVector(Base6Directions.GetClosestDirection(subBlockMatrix.Forward));
			double num = Vector3D.Dot(subBlockMatrix.Forward, (Vector3D)intVector);
			if (Math.Abs(1.0 - num) <= 1E-08)
			{
				subBlockMatrix.Forward = intVector;
			}
			Vector3I intVector2 = Base6Directions.GetIntVector(Base6Directions.GetClosestDirection(subBlockMatrix.Right));
			double num2 = Vector3D.Dot(subBlockMatrix.Right, (Vector3D)intVector2);
			if (Math.Abs(1.0 - num2) <= 1E-08)
			{
				subBlockMatrix.Right = intVector2;
			}
			Vector3I intVector3 = Base6Directions.GetIntVector(Base6Directions.GetClosestDirection(subBlockMatrix.Up));
			double num3 = Vector3D.Dot(subBlockMatrix.Up, (Vector3D)intVector3);
			if (Math.Abs(1.0 - num3) <= 1E-08)
			{
				subBlockMatrix.Up = intVector3;
			}
			dummyPosition = subBlockMatrix.Translation;
			if (useOffset)
			{
				Vector3 blockGridOffset = GetBlockGridOffset(subBlockDefinition);
				subBlockMatrix.Translation -= Vector3D.TransformNormal(blockGridOffset, subBlockMatrix);
			}
			return true;
		}

		public virtual float GetMass()
		{
			Matrix orientation;
			if (MyDestructionData.Static != null)
			{
				return MyDestructionData.Static.GetBlockMass(SlimBlock.CalculateCurrentModel(out orientation), BlockDefinition);
			}
			return BlockDefinition.Mass;
		}

		public virtual BoundingBox GetGeometryLocalBox()
		{
			if (base.Model != null)
			{
				return base.Model.BoundingBox;
			}
			return new BoundingBox(new Vector3((0f - CubeGrid.GridSize) / 2f), new Vector3(CubeGrid.GridSize / 2f));
		}

		public DictionaryReader<string, MySlimBlock> GetSubBlocks()
		{
			return new DictionaryReader<string, MySlimBlock>(SubBlocks);
		}

		public bool TryGetSubBlock(string name, out MySlimBlock block)
		{
			if (SubBlocks == null)
			{
				block = null;
				return false;
			}
			return SubBlocks.TryGetValue(name, out block);
		}

		public MyUpgradableBlockComponent GetComponent()
		{
			if (m_upgradeComponent == null)
			{
				m_upgradeComponent = new MyUpgradableBlockComponent(this);
			}
			return m_upgradeComponent;
		}

		public void AddUpgradeValue(string name, float defaultValue)
		{
			if (UpgradeValues.TryGetValue(name, out var value))
			{
				if (value != defaultValue)
				{
					MyLog.Default.WriteLine("ERROR while adding upgraded block " + DisplayNameText.ToString() + ". Duplicate with different default value found!");
				}
			}
			else
			{
				UpgradeValues.Add(name, defaultValue);
			}
		}

		public void CommitUpgradeValues()
		{
			this.OnUpgradeValuesChanged?.Invoke();
		}

		public virtual void CreateRenderer(MyPersistentEntityFlags2 persistentFlags, Vector3 colorMaskHsv, object modelStorage)
		{
			m_skinSubtypeId = MyStringHash.NullOrEmpty;
			base.Render = new MyRenderComponentCubeBlock();
			base.Render.ColorMaskHsv = colorMaskHsv;
			base.Render.ShadowBoxLod = true;
			base.Render.EnableColorMaskHsv = true;
			base.Render.SkipIfTooSmall = false;
			base.Render.PersistentFlags |= persistentFlags | MyPersistentEntityFlags2.CastShadows;
			base.Render.ModelStorage = modelStorage;
			base.Render.FadeIn = CubeGrid.Render.FadeIn;
			UpdateSkin();
		}

		public MyFractureComponentCubeBlock GetFractureComponent()
		{
			MyFractureComponentCubeBlock result = null;
			if (MyFakes.ENABLE_FRACTURE_COMPONENT)
			{
				result = base.Components.Get<MyFractureComponentBase>() as MyFractureComponentCubeBlock;
			}
			return result;
		}

		public override void RefreshModels(string modelPath, string modelCollisionPath)
		{
			MyModels.GetModelOnlyData(modelPath)?.Rescale(CubeGrid.GridScale);
			if (modelCollisionPath != null)
			{
				MyModels.GetModelOnlyData(modelCollisionPath)?.Rescale(CubeGrid.GridScale);
			}
			base.RefreshModels(modelPath, modelCollisionPath);
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
			CubeGrid.RegisterInventory(this);
			if (inventory != null)
			{
				inventory.ContentsChanged += Inventory_ContentsChanged;
			}
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
			CubeGrid.UnregisterInventory(this);
			if (inventory != null)
			{
				inventory.ContentsChanged -= Inventory_ContentsChanged;
			}
		}

		private void Inventory_ContentsChanged(MyInventoryBase obj)
		{
			CubeGrid.SetInventoryMassDirty();
			CubeGrid.RaiseInventoryChanged(obj);
		}

		public override bool GetIntersectionWithLine(ref LineD line, out MyIntersectionResultLineTriangleEx? t, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
		{
			t = null;
			if (base.ModelCollision == null)
			{
				return false;
			}
			if (BlockDefinition == null)
			{
				return false;
			}
			Orientation.GetMatrix(out var result);
			Vector3.TransformNormal(ref BlockDefinition.ModelOffset, ref result, out var result2);
			result.Translation = Position * CubeGrid.GridSize + result2;
			MatrixD customInvMatrix = MatrixD.Invert(base.WorldMatrix);
			t = base.ModelCollision.GetTrianglePruningStructure().GetIntersectionWithLine(this, ref line, ref customInvMatrix, flags);
			if (!t.HasValue && base.Subparts != null)
			{
				foreach (KeyValuePair<string, MyEntitySubpart> subpart in base.Subparts)
				{
					if (subpart.Value != null && subpart.Value.ModelCollision != null)
					{
						customInvMatrix = MatrixD.Invert(subpart.Value.WorldMatrix);
						t = subpart.Value.ModelCollision.GetTrianglePruningStructure().GetIntersectionWithLine(this, ref line, ref customInvMatrix, flags);
						if (t.HasValue)
						{
							break;
						}
					}
				}
			}
			return t.HasValue;
		}

		public virtual void DisableUpdates()
		{
			base.NeedsUpdate &= ~(MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME);
		}

		protected static void ClampExperimentalValue(ref float? value, float maxSafeValue)
		{
			if (!AllowExperimentalValues && value > maxSafeValue)
			{
				value = maxSafeValue;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Block received damage from any source.
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="damageType"></param>
		/// <param name="attackerId"></param>
		/// <param name="realHitEntityId"></param>
		/// <param name="shouldDetonateAmmo"></param>
		/// <returns> Returns true if damage should be inflicted upon the block normally.</returns>
		public virtual bool ReceivedDamage(float damage, MyStringHash damageType, long attackerId, long realHitEntityId, bool shouldDetonateAmmo = true)
		{
			m_shouldDetonateAmmo = shouldDetonateAmmo;
			return true;
		}

		private void CacheInventory(MyInventory inventory)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			foreach (MyPhysicalInventoryItem item in inventory.GetItems())
			{
				CacheItem(inventory, item, item.Amount);
			}
		}

		private void CacheItem(MyInventoryBase inventoryBase, MyPhysicalInventoryItem item, MyFixedPoint amount)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			int num = amount.ToIntSafe();
			if (num != 0)
			{
				MyDefinitionId objectId = item.Content.GetObjectId();
				MyDefinitionId id = item.Content.GetId();
				if (id.TypeId == typeof(MyObjectBuilder_AmmoDefinition) || id.TypeId == typeof(MyObjectBuilder_AmmoMagazine))
				{
					CacheAmmo(objectId, num);
				}
			}
		}

		private void CacheAmmo(MyDefinitionId myDefinitionId, int amount)
		{
			if (Sync.IsServer)
			{
				MyInventoryItemAdapter @static = MyInventoryItemAdapter.Static;
				@static.Adapt(myDefinitionId);
				m_detonationData.CachedAmmoMass += (float)amount * @static.Mass;
				m_detonationData.CachedAmmoVolume += (float)amount * @static.Volume;
			}
		}

		private void DetonateAmmo()
		{
			if (!m_detonationData.HasDetonated)
			{
				float ammoVolume = m_detonationData.CachedAmmoVolume * 1000f;
				float num = CalculateAmmoExplosionRadius(m_detonationData.CachedAmmoMass, ammoVolume);
				float damage = CalculateAmmoExplosionDamage(m_detonationData.CachedAmmoMass, ammoVolume);
				string text = CalculateAmmoExplosionParticleEffect(m_detonationData.CachedAmmoMass, ammoVolume);
				if (num <= 0f)
				{
					num = 1f;
				}
				BoundingSphereD explosionSphere = new BoundingSphereD(base.PositionComp.GetPosition(), num);
				MyEntity ownerEntity = null;
				MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
				myExplosionInfo.PlayerDamage = 0f;
				myExplosionInfo.Damage = damage;
				myExplosionInfo.ExplosionType = MyExplosionTypeEnum.CUSTOM;
				myExplosionInfo.ExplosionSphere = explosionSphere;
				myExplosionInfo.LifespanMiliseconds = 700;
				myExplosionInfo.ParticleScale = 1f;
				myExplosionInfo.OwnerEntity = ownerEntity;
				myExplosionInfo.ExplosionFlags = MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION | MyExplosionFlags.CREATE_PARTICLE_DEBRIS;
				myExplosionInfo.VoxelCutoutScale = 0.3f;
				myExplosionInfo.PlaySound = true;
				myExplosionInfo.ApplyForceAndDamage = true;
				myExplosionInfo.KeepAffectedBlocks = true;
				myExplosionInfo.CustomEffect = (string.IsNullOrEmpty(BlockDefinition.AmmoExplosionEffect) ? text : BlockDefinition.AmmoExplosionEffect);
				myExplosionInfo.CustomSound = new MySoundPair(BlockDefinition.AmmoExplosionSound);
				myExplosionInfo.ShouldDetonateAmmo = false;
				MyExplosionInfo explosionInfo = myExplosionInfo;
				explosionInfo.StrengthImpulse = 0.7f;
				if (Physics != null)
				{
					explosionInfo.Velocity = Physics.LinearVelocity;
				}
				MyExplosions.AddExplosion(ref explosionInfo);
				m_detonationData.HasDetonated = true;
			}
		}

		private float CalculateAmmoExplosionRadius(float ammoMass, float ammoVolume)
		{
			float num = MyMath.Clamp((ammoVolume - m_detonationData.ExplosionAmmoVolumeMin) / m_detonationData.ExplosionAmmoVolumeMax, 0f, 1f);
			return m_detonationData.ExplosionRadiusMin + num * (m_detonationData.ExplosionRadiusMax - m_detonationData.ExplosionRadiusMin);
		}

		private float CalculateAmmoExplosionDamage(float ammoMass, float ammoVolume)
		{
			return Math.Min(m_detonationData.ExplosionDamageMax, ammoVolume * m_detonationData.ExplosionDamagePerLiter);
		}

		private string CalculateAmmoExplosionParticleEffect(float ammoMass, float ammoVolume)
		{
			float num = 125f;
			float num2 = 16000f;
			float num3 = 100000f;
			if (ammoVolume < num)
			{
				return "Explosion_AmmunitionTiny";
			}
			if (ammoVolume < num2)
			{
				return "Explosion_AmmunitionSmall";
			}
			if (ammoVolume < num3)
			{
				return "Explosion_AmmunitionMedium";
			}
			return "Explosion_AmmunitionLarge";
		}

		private void UnsubscribeEvents()
		{
			MyInventory inventory = this.GetInventory();
			if (inventory != null)
			{
				inventory.InventoryContentChanged -= CacheItem;
			}
		}

=======
		public virtual bool ReceivedDamage(float damage, MyStringHash damageType, long attackerId, long realHitEntityId)
		{
			return true;
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void Init(MyObjectBuilder_CubeBlock builder, VRage.Game.ModAPI.IMyCubeGrid cubeGrid)
		{
			if (cubeGrid is MyCubeGrid)
			{
				Init(builder, cubeGrid as MyCubeGrid);
			}
		}

		private Action<MyCubeBlock> GetDelegate(Action<VRage.Game.ModAPI.IMyCubeBlock> value)
		{
			return (Action<MyCubeBlock>)Delegate.CreateDelegate(typeof(Action<MyCubeBlock>), value.Target, value.Method);
		}

		void VRage.Game.ModAPI.IMyCubeBlock.CalcLocalMatrix(out Matrix localMatrix, out string currModel)
		{
			CalcLocalMatrix(out localMatrix, out currModel);
		}

		string VRage.Game.ModAPI.IMyCubeBlock.CalculateCurrentModel(out Matrix orientation)
		{
			return CalculateCurrentModel(out orientation);
		}

		MyObjectBuilder_CubeBlock VRage.Game.ModAPI.IMyCubeBlock.GetObjectBuilderCubeBlock(bool copy)
		{
			return GetObjectBuilderCubeBlock(copy);
		}

		MyRelationsBetweenPlayerAndBlock VRage.Game.ModAPI.Ingame.IMyCubeBlock.GetPlayerRelationToOwner()
		{
			return GetPlayerRelationToOwner();
		}

		MyRelationsBetweenPlayerAndBlock VRage.Game.ModAPI.Ingame.IMyCubeBlock.GetUserRelationToOwner(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser)
		{
			return GetUserRelationToOwner(playerId, defaultNoUser);
		}

		void VRage.Game.ModAPI.IMyCubeBlock.Init()
		{
			Init();
		}

		void VRage.Game.ModAPI.IMyCubeBlock.Init(MyObjectBuilder_CubeBlock builder, VRage.Game.ModAPI.IMyCubeGrid cubeGrid)
		{
			Init(builder, cubeGrid);
		}

		void VRage.Game.ModAPI.IMyCubeBlock.OnBuildSuccess(long builtBy)
		{
			OnBuildSuccess(builtBy, instantBuild: false);
		}

		void VRage.Game.ModAPI.IMyCubeBlock.OnBuildSuccess(long builtBy, bool instantBuild)
		{
			OnBuildSuccess(builtBy, instantBuild);
		}

		void VRage.Game.ModAPI.IMyCubeBlock.OnDestroy()
		{
			OnDestroy();
		}

		void VRage.Game.ModAPI.IMyCubeBlock.OnModelChange()
		{
			OnModelChange();
		}

		void VRage.Game.ModAPI.IMyCubeBlock.OnRegisteredToGridSystems()
		{
			OnRegisteredToGridSystems();
		}

		void VRage.Game.ModAPI.IMyCubeBlock.OnRemovedByCubeBuilder()
		{
			OnRemovedByCubeBuilder();
		}

		void VRage.Game.ModAPI.IMyCubeBlock.OnUnregisteredFromGridSystems()
		{
			OnUnregisteredFromGridSystems();
		}

		string VRage.Game.ModAPI.IMyCubeBlock.RaycastDetectors(Vector3D worldFrom, Vector3D worldTo)
		{
			return base.Components.Get<MyUseObjectsComponentBase>().RaycastDetectors(worldFrom, worldTo);
		}

		void VRage.Game.ModAPI.IMyCubeBlock.ReloadDetectors(bool refreshNetworks)
		{
			base.Components.Get<MyUseObjectsComponentBase>().LoadDetectorsFromModel();
		}

		void VRage.Game.ModAPI.Ingame.IMyCubeBlock.UpdateIsWorking()
		{
			UpdateIsWorking();
		}

		void VRage.Game.ModAPI.Ingame.IMyCubeBlock.UpdateVisual()
		{
			UpdateVisual();
		}

		void VRage.Game.ModAPI.IMyCubeBlock.SetDamageEffect(bool start)
		{
			SetDamageEffect(start);
		}

		void Sandbox.ModAPI.Ingame.IMyUpgradableBlock.GetUpgrades(out Dictionary<string, float> upgrades)
		{
			upgrades = new Dictionary<string, float>();
			foreach (KeyValuePair<string, float> upgradeValue in UpgradeValues)
			{
				upgrades.Add(upgradeValue.Key, upgradeValue.Value);
			}
		}

		bool VRage.Game.ModAPI.IMyCubeBlock.SetEffect(string effectName, bool stopPrevious)
		{
			return SetEffect(effectName, stopPrevious);
		}

		bool VRage.Game.ModAPI.IMyCubeBlock.SetEffect(string effectName, float parameter, bool stopPrevious, bool ignoreParameter, bool removeSameNameEffects)
		{
			return SetEffect(effectName, parameter, stopPrevious, ignoreParameter, removeSameNameEffects);
		}

		int VRage.Game.ModAPI.IMyCubeBlock.RemoveEffect(string effectName, int exception)
		{
			return RemoveEffect(effectName, exception);
		}
	}
}
