using System.Collections.Generic;
using System.Linq;
using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.WorldEnvironment;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons.Guns
{
	public class MyDrillSensorRayCast : MyDrillSensorBase
	{
		private readonly List<MyLineSegmentOverlapResult<MyEntity>> m_raycastResults = new List<MyLineSegmentOverlapResult<MyEntity>>();

		private readonly float m_rayLength;

		private readonly float m_originOffset;

		private Vector3D m_origin;

		private readonly List<MyPhysics.HitInfo> m_hits;

		private bool m_parallelRaycastRunning;

		public MyDrillSensorRayCast(float originOffset, float rayLength, MyDefinitionBase drillDefinition)
		{
			m_rayLength = rayLength;
			m_originOffset = originOffset;
			m_hits = new List<MyPhysics.HitInfo>();
			m_drillDefinition = drillDefinition;
		}

		public override void OnWorldPositionChanged(ref MatrixD worldMatrix)
		{
			if (!m_parallelRaycastRunning)
			{
				Vector3D forward = worldMatrix.Forward;
				m_origin = worldMatrix.Translation + forward * m_originOffset;
				base.FrontPoint = m_origin + m_rayLength * forward;
				base.Center = m_origin;
			}
		}

		protected override void ReadEntitiesInRange()
		{
			if (!m_parallelRaycastRunning)
			{
				m_parallelRaycastRunning = true;
				m_hits.Clear();
				Vector3D to = base.FrontPoint;
				if (MyFakes.USE_PARALLEL_TOOL_RAYCAST)
				{
					MyPhysics.CastRayParallel(ref m_origin, ref to, m_hits, 24, RayCastResult);
					return;
				}
				MyPhysics.CastRay(m_origin, to, m_hits, 24);
				ProcessToolRaycast();
				m_parallelRaycastRunning = false;
			}
		}

		private void RayCastResult(List<MyPhysics.HitInfo> list)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				ProcessToolRaycast();
				m_parallelRaycastRunning = false;
			}, "Tool Sensor Raycast");
		}

		private void ProcessToolRaycast()
		{
			m_entitiesInRange.Clear();
			DetectionInfo value = default(DetectionInfo);
			bool flag = false;
			foreach (MyPhysics.HitInfo hit in m_hits)
			{
				HkHitInfo hkHitInfo = hit.HkHitInfo;
				if (hkHitInfo.Body == null)
				{
					continue;
				}
				IMyEntity hitEntity = hkHitInfo.GetHitEntity();
				if (hitEntity == null)
				{
					continue;
				}
				IMyEntity topMostParent = hitEntity.GetTopMostParent();
<<<<<<< HEAD
				if (IgnoredEntities.Contains(topMostParent))
=======
				if (Enumerable.Contains<IMyEntity>((IEnumerable<IMyEntity>)IgnoredEntities, topMostParent))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					continue;
				}
				Vector3D position = hit.Position;
				MyCubeGrid myCubeGrid = topMostParent as MyCubeGrid;
				if (myCubeGrid != null)
				{
					if (myCubeGrid.GridSizeEnum == MyCubeSize.Large)
					{
						position += hit.HkHitInfo.Normal * -0.08f;
					}
					else
					{
						position += hit.HkHitInfo.Normal * -0.02f;
					}
				}
				if (m_entitiesInRange.TryGetValue(topMostParent.EntityId, out value))
				{
<<<<<<< HEAD
					double num = Vector3D.DistanceSquared(value.DetectionPoint, m_origin);
					double num2 = Vector3D.DistanceSquared(position, m_origin);
=======
					float num = Vector3.DistanceSquared(value.DetectionPoint, m_origin);
					float num2 = Vector3.DistanceSquared(position, m_origin);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (num > num2)
					{
						m_entitiesInRange[topMostParent.EntityId] = new DetectionInfo(topMostParent as MyEntity, position);
					}
				}
				else
				{
					m_entitiesInRange[topMostParent.EntityId] = new DetectionInfo(topMostParent as MyEntity, position);
				}
				if (hitEntity is MyEnvironmentSector && !flag)
				{
					MyEnvironmentSector myEnvironmentSector = hitEntity as MyEnvironmentSector;
					uint shapeKey = hkHitInfo.GetShapeKey(0);
					int itemFromShapeKey = myEnvironmentSector.GetItemFromShapeKey(shapeKey);
					if (myEnvironmentSector.DataView.Items[itemFromShapeKey].ModelIndex >= 0)
					{
						flag = true;
						m_entitiesInRange[hitEntity.EntityId] = new DetectionInfo(myEnvironmentSector, position, itemFromShapeKey);
					}
				}
			}
			LineD ray = new LineD(m_origin, base.FrontPoint);
			using (m_raycastResults.GetClearToken())
			{
				MyGamePruningStructure.GetAllEntitiesInRay(ref ray, m_raycastResults);
				foreach (MyLineSegmentOverlapResult<MyEntity> raycastResult in m_raycastResults)
				{
					if (raycastResult.Element == null)
					{
						continue;
					}
					MyEntity topMostParent2 = raycastResult.Element.GetTopMostParent();
					if (IgnoredEntities.Contains(topMostParent2))
					{
						continue;
					}
					MyCubeBlock myCubeBlock = raycastResult.Element as MyCubeBlock;
					if (myCubeBlock == null)
					{
						continue;
					}
					Vector3D vector3D = default(Vector3D);
					if (myCubeBlock.SlimBlock.BlockDefinition.HasPhysics)
					{
						continue;
					}
					MatrixD matrix = myCubeBlock.PositionComp.WorldMatrixNormalizedInv;
<<<<<<< HEAD
					Vector3 vector = Vector3D.Transform(m_origin, ref matrix);
					Vector3D vector3D2 = Vector3D.Transform(base.FrontPoint, ref matrix);
					float? num3 = new Ray(vector, Vector3.Normalize(vector3D2 - vector)).Intersects(myCubeBlock.PositionComp.LocalAABB) + 0.01f;
=======
					Vector3D vector3D2 = Vector3D.Transform(m_origin, ref matrix);
					Vector3D vector3D3 = Vector3D.Transform(base.FrontPoint, ref matrix);
					float? num3 = new Ray(vector3D2, Vector3.Normalize(vector3D3 - vector3D2)).Intersects(myCubeBlock.PositionComp.LocalAABB) + 0.01f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (!num3.HasValue || !(num3 <= m_rayLength))
					{
						continue;
					}
					vector3D = m_origin + Vector3D.Normalize(base.FrontPoint - m_origin) * num3.Value;
					if (m_entitiesInRange.TryGetValue(topMostParent2.EntityId, out value))
					{
<<<<<<< HEAD
						if (Vector3D.DistanceSquared(value.DetectionPoint, m_origin) > Vector3D.DistanceSquared(vector3D, m_origin))
=======
						if (Vector3.DistanceSquared(value.DetectionPoint, m_origin) > Vector3.DistanceSquared(vector3D, m_origin))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							m_entitiesInRange[topMostParent2.EntityId] = new DetectionInfo(topMostParent2, vector3D);
						}
					}
					else
					{
						m_entitiesInRange[topMostParent2.EntityId] = new DetectionInfo(topMostParent2, vector3D);
					}
				}
			}
		}

		public override void DebugDraw()
		{
			MyRenderProxy.DebugDrawLine3D(m_origin, base.FrontPoint, Color.Red, Color.Blue, depthRead: false);
		}
	}
}
