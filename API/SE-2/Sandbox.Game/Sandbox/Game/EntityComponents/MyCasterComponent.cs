using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Weapons.Guns;
using Sandbox.Game.WorldEnvironment;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	/// <summary>
	/// Component used for finding object by casting. It is possible to create this component with different types of casting:
	/// Box, Raycast, Shape
	/// </summary>
	public class MyCasterComponent : MyEntityComponentBase
	{
		private class Sandbox_Game_EntityComponents_MyCasterComponent_003C_003EActor
		{
		}

		/// <summary>
		/// Indicates block that is hit by raycast.
		/// </summary>
		private MySlimBlock m_hitBlock;

		/// <summary>
		/// Indicates grid that was hit by raycast.
		/// </summary>
		private MyCubeGrid m_hitCubeGrid;

		/// <summary>
		/// Indicates character that was hit by raycast.
		/// </summary>
		private MyCharacter m_hitCharacter;

		/// <summary>
		/// Indicates destroyable object that was hit by raycast
		/// </summary>
		private IMyDestroyableObject m_hitDestroaybleObj;

		/// <summary>
		/// Indicates floating object that was hit by raycast.
		/// </summary>
		private MyFloatingObject m_hitFloatingObject;

		/// <summary>
		/// Indicates Environment Sector hit.
		/// </summary>
		private MyEnvironmentSector m_hitEnvironmentSector;

		/// <summary>
		/// Indicates Specific item of Environment Sector hit.
		/// </summary>
		private int m_environmentItem;

		/// <summary>
		/// Indicates exact hit position of raycast.
		/// </summary>
		private Vector3D m_hitPosition;

		/// <summary>
		/// Indicates distance to block that is hit by raycast.
		/// </summary>
		private double m_distanceToHitSq;

		/// <summary>
		/// Raycaster used for finding hit block.
		/// </summary>
		private MyDrillSensorBase m_caster;

		/// <summary>
		/// Point of reference to which closest object is found.
		/// </summary>
		private Vector3D m_pointOfReference;

		/// <summary>
		/// Indicates if point of reference is set.
		/// </summary>
		private bool m_isPointOfRefSet;

		public override string ComponentTypeDebugString => "MyBlockInfoComponent";

		/// <summary>
		/// Gets block that is hit by a raycaster.
		/// </summary>
		public MySlimBlock HitBlock => m_hitBlock;

		public MyCubeGrid HitCubeGrid => m_hitCubeGrid;

		public Vector3D HitPosition => m_hitPosition;

		public IMyDestroyableObject HitDestroyableObj => m_hitDestroaybleObj;

		public MyFloatingObject HitFloatingObject => m_hitFloatingObject;

		public MyEnvironmentSector HitEnvironmentSector => m_hitEnvironmentSector;

		public int EnvironmentItem => m_environmentItem;

		public MyCharacter HitCharacter => m_hitCharacter;

		public double DistanceToHitSq => m_distanceToHitSq;

		public Vector3D PointOfReference => m_pointOfReference;

		public MyDrillSensorBase Caster => m_caster;

		public MyCasterComponent(MyDrillSensorBase caster)
		{
			m_caster = caster;
		}

		public override void Init(MyComponentDefinitionBase definition)
		{
			base.Init(definition);
		}

		public void OnWorldPosChanged(ref MatrixD newTransform)
		{
			MatrixD worldMatrix = newTransform;
			m_caster.OnWorldPositionChanged(ref worldMatrix);
			Dictionary<long, MyDrillSensorBase.DetectionInfo> entitiesInRange = m_caster.EntitiesInRange;
			float num = float.MaxValue;
			MyEntity myEntity = null;
			int environmentItem = 0;
			if (!m_isPointOfRefSet)
			{
				m_pointOfReference = worldMatrix.Translation;
			}
			if (entitiesInRange != null && entitiesInRange.Count > 0)
			{
				foreach (MyDrillSensorBase.DetectionInfo value in entitiesInRange.Values)
				{
					float num2 = (float)Vector3D.DistanceSquared(value.DetectionPoint, m_pointOfReference);
					if (value.Entity.Physics != null && value.Entity.Physics.Enabled && num2 < num)
					{
						myEntity = value.Entity;
						environmentItem = value.ItemId;
						m_distanceToHitSq = num2;
						m_hitPosition = value.DetectionPoint;
						num = num2;
					}
				}
			}
			m_hitCubeGrid = myEntity as MyCubeGrid;
			m_hitBlock = null;
			m_hitDestroaybleObj = myEntity as IMyDestroyableObject;
			m_hitFloatingObject = myEntity as MyFloatingObject;
			m_hitCharacter = myEntity as MyCharacter;
			m_hitEnvironmentSector = myEntity as MyEnvironmentSector;
			m_environmentItem = environmentItem;
			if (m_hitCubeGrid != null)
			{
				MatrixD worldMatrixNormalizedInv = m_hitCubeGrid.PositionComp.WorldMatrixNormalizedInv;
				Vector3 vector = Vector3.Normalize(m_hitPosition - newTransform.Translation);
				Vector3D vector3D = Vector3D.Transform(m_hitPosition - vector * ((m_hitCubeGrid.GridSizeEnum == MyCubeSize.Large) ? 0.05f : (-0.007f)), worldMatrixNormalizedInv);
				m_hitCubeGrid.FixTargetCube(out var cube, vector3D / m_hitCubeGrid.GridSize);
				m_hitBlock = m_hitCubeGrid.GetCubeBlock(cube);
			}
		}

		public float GetCastLength()
		{
			return ((Vector3)(m_caster.Center - m_caster.FrontPoint)).Length();
		}

		public void SetPointOfReference(Vector3D pointOfRef)
		{
			m_pointOfReference = pointOfRef;
			m_isPointOfRefSet = true;
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
		}
	}
}
