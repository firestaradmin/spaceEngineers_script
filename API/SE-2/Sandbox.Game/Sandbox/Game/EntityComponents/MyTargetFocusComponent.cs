using System;
using System.Collections.Generic;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	[StaticEventOwner]
	[MyComponentType(typeof(MyTargetFocusComponent))]
	[MyComponentBuilder(typeof(MyObjectBuilder_TargetFocusComponent), true)]
	public class MyTargetFocusComponent : MyGameLogicComponent
	{
		private class Sandbox_Game_EntityComponents_MyTargetFocusComponent_003C_003EActor : IActivator, IActivator<MyTargetFocusComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyTargetFocusComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTargetFocusComponent CreateInstance()
			{
				return new MyTargetFocusComponent();
			}

			MyTargetFocusComponent IActivator<MyTargetFocusComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static double MinLockAngleCosSquared;

		private static readonly List<long> m_idCache = new List<long>();

		private HashSet<MyCubeGrid> m_supergridsInRange = new HashSet<MyCubeGrid>();

		private MyShipController m_shipController;

		private MyCubeGrid m_currentTarget;

		private List<MyEntity> m_entitiesInRange = new List<MyEntity>();

		private Dictionary<MyCubeGrid, MyCubeGrid> m_gridToMasterMap = new Dictionary<MyCubeGrid, MyCubeGrid>();

		private BoundingSphereD m_rangeBoundingSphere;

		private MatrixD m_aimMatrix;

		/// <summary>
		///  key - gridId
		///  value
		///  if value more 0 - target is visible
		///  if value less 0 - target is invisible
		///  if value = 0 - must refresh raycast
		///  </summary>
		private Dictionary<long, int> m_raycastCache = new Dictionary<long, int>();

		private const int RAYCAST_INTERVAL = 110;

		public double FocusSearchMaxDistance { get; private set; }

		public double FocusSearchMaxDistanceSquared { get; private set; }

		private IMyTargetingCapableBlock ControlledBlock => MySession.Static.ControlledEntity as IMyTargetingCapableBlock;

		public MyEntity CurrentTarget
		{
			get
			{
				if (!IsTargetLockingEnabledInController)
				{
					return null;
				}
				return m_currentTarget;
			}
		}

		public long? GridId => m_shipController?.CubeGrid?.EntityId;

		public bool IsLocallyControlled => base.Entity.EntityId == MySession.Static.LocalCharacter?.EntityId;

		public bool IsTargetLockingEnabledInController => ControlledBlock?.IsTargetLockingEnabled() ?? false;

		public override string ComponentTypeDebugString => "Target focus";

		private MyTargetLockingComponent TargetLockingComponent => (base.Entity as MyCharacter)?.Components.Get<MyTargetLockingComponent>();

		public override bool IsSerialized()
		{
			return false;
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			MyTargetFocusComponentDefinition myTargetFocusComponentDefinition = definition as MyTargetFocusComponentDefinition;
			FocusSearchMaxDistance = myTargetFocusComponentDefinition.FocusSearchMaxDistance;
			FocusSearchMaxDistanceSquared = FocusSearchMaxDistance * FocusSearchMaxDistance;
			double num = Math.Cos(myTargetFocusComponentDefinition.AngularToleranceFromCrosshair * Math.PI / 180.0);
			MinLockAngleCosSquared = num * num;
			m_rangeBoundingSphere = new BoundingSphereD(Vector3D.Zero, FocusSearchMaxDistance);
		}

		private bool IsValidTarget(MyEntity entity)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return false;
			}
			if (myCubeGrid.EntityId == GridId)
			{
				return false;
			}
			if (myCubeGrid.Physics == null)
			{
				return false;
			}
			return true;
		}

		private static double GetTargetCosSquaredSigned(MatrixD shooterMatrix, MyCubeGrid target)
		{
			Vector3D vector3D = MyTargetingHelper.Instance.GetLockingPosition(target) - shooterMatrix.Translation;
			double num = vector3D.Dot(shooterMatrix.Forward);
			double num2 = num * num / vector3D.LengthSquared();
			if (num < 0.0)
			{
				num2 *= -1.0;
			}
			return num2;
		}

		public bool IsTargetInRange(MatrixD shooterMatrix, MyCubeGrid target, out double targetCosSquaredSigned, bool checkAngle = true)
		{
			if (target.MarkedForClose)
			{
				targetCosSquaredSigned = double.MinValue;
				return false;
			}
			targetCosSquaredSigned = GetTargetCosSquaredSigned(shooterMatrix, target);
			if (targetCosSquaredSigned < MinLockAngleCosSquared && checkAngle)
			{
				return false;
			}
			if ((target.Physics.CenterOfMassWorld - shooterMatrix.Translation).LengthSquared() > FocusSearchMaxDistanceSquared)
			{
				return false;
			}
			return true;
		}

		private bool IsVisibleUsingRaycast(MyCubeGrid target)
		{
			MyCubeBlock myCubeBlock = (base.Entity as MyCharacter)?.IsUsing as MyCubeBlock;
			if (myCubeBlock == null)
			{
				return false;
			}
			Vector3D lockingPosition = MyTargetingHelper.Instance.GetLockingPosition(target);
			Vector3D translation = myCubeBlock.WorldMatrix.Translation;
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			MyPhysics.CastRay(lockingPosition, translation, list, 15);
			return MyTargetingHelper.Instance.IsVisible(myCubeBlock.CubeGrid, target, list);
		}

		public void UpdateAimVector()
		{
			MatrixD? overridingFocusMatrix = MySession.Static.CameraController.GetOverridingFocusMatrix();
			if (overridingFocusMatrix.HasValue)
			{
				m_aimMatrix = overridingFocusMatrix.Value;
				return;
			}
			IMyTargetingCapableBlock controlledBlock = ControlledBlock;
			m_aimMatrix = controlledBlock.GetWorldMatrix();
		}

		private void Callback(MyCubeGrid target, List<MyPhysics.HitInfo> hits)
		{
			MyCubeBlock myCubeBlock = (base.Entity as MyCharacter)?.IsUsing as MyCubeBlock;
			if (myCubeBlock != null)
			{
				bool flag = MyTargetingHelper.Instance.IsVisible(myCubeBlock.CubeGrid, target, hits);
				m_raycastCache[target.EntityId] = (flag ? 1 : (-1)) * 110;
			}
		}

		private void RecomputeFocusTarget()
		{
			UpdateAimVector();
			MyCubeGrid currentTarget = null;
			double num = 0.0;
			foreach (MyCubeGrid item in m_supergridsInRange)
			{
				if (!item.IsPreview && item.Physics != null && IsTargetInRange(m_aimMatrix, item, out var targetCosSquaredSigned) && IsVisibleUsingRaycast(item) && targetCosSquaredSigned > num)
				{
					currentTarget = item;
					num = targetCosSquaredSigned;
				}
			}
			UpdateCache();
			m_currentTarget = currentTarget;
		}

		private void UpdateCache()
		{
			foreach (long key in m_raycastCache.Keys)
			{
				m_idCache.Add(key);
			}
			foreach (long item in m_idCache)
			{
				int num = m_raycastCache[item];
				if (num == 0)
				{
					m_raycastCache.Remove(item);
				}
				else
				{
					m_raycastCache[item] = ((num > 0) ? (num - 10) : (num + 10));
				}
			}
			m_idCache.Clear();
		}

		public void LookupSuperGridsInRange()
		{
			LookupSuperGridsInRangeInternal();
		}

		private void LookupSuperGridsInRangeInternal()
		{
			UpdateAimVector();
			m_rangeBoundingSphere.Center = m_aimMatrix.Translation;
			m_entitiesInRange.Clear();
			MyGamePruningStructure.GetAllEntitiesInSphere(ref m_rangeBoundingSphere, m_entitiesInRange);
			m_gridToMasterMap.Clear();
			m_supergridsInRange.Clear();
			foreach (MyEntity item in m_entitiesInRange)
			{
				MyCubeGrid myCubeGrid = item as MyCubeGrid;
				if (myCubeGrid == null)
				{
					continue;
				}
				m_gridToMasterMap.TryGetValue(myCubeGrid, out var value);
				if (value != null)
				{
					continue;
				}
				value = myCubeGrid;
				if (value.IsPreview || value.Physics == null)
				{
					continue;
				}
				float mass = value.Physics.Mass;
				List<MyCubeGrid> connectedGrids = myCubeGrid.GetConnectedGrids(GridLinkTypeEnum.Physical);
				bool flag = false;
				foreach (MyCubeGrid item2 in connectedGrids)
				{
					if (item2.IsPowered)
					{
						if (item2.Physics.Mass == mass && item2.EntityId < value.EntityId)
						{
							value = item2;
							mass = item2.Physics.Mass;
						}
						if (item2.Physics.Mass > mass)
						{
							value = item2;
							mass = item2.Physics.Mass;
						}
						flag = true;
					}
				}
				if (!flag)
				{
					continue;
				}
				m_supergridsInRange.Add(value);
				foreach (MyCubeGrid item3 in connectedGrids)
				{
					m_gridToMasterMap.Add(item3, value);
				}
			}
			m_gridToMasterMap.TryGetValue((ControlledBlock as MyCubeBlock).CubeGrid, out var value2);
			m_supergridsInRange.Remove(value2);
		}

		public override void UpdateAfterSimulation10()
		{
			IMyTargetingCapableBlock myTargetingCapableBlock;
			if (IsLocallyControlled && IsTargetLockingEnabledInController && ((myTargetingCapableBlock = MySession.Static.ControlledEntity as IMyTargetingCapableBlock) == null || !myTargetingCapableBlock.IsShipToolSelected()))
			{
				RecomputeFocusTarget();
			}
		}

		public override void UpdateAfterSimulation100()
		{
			if (IsLocallyControlled && ControlledBlock != null && IsTargetLockingEnabledInController)
			{
				LookupSuperGridsInRange();
			}
		}

		public void OnLockRequest()
		{
			TargetLockingComponent.OnTargetRequest(CurrentTarget as MyCubeGrid);
		}
	}
}
