using System;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
	public class MyBasicObstacle : IMyObstacle
	{
		public MatrixD m_worldInv;

		public Vector3D m_halfExtents;

		private MyEntity m_entity;

		public bool Valid { get; private set; }

		public MyBasicObstacle(MyEntity entity)
		{
			m_entity = entity;
			m_entity.OnClosing += OnEntityClosing;
			Update();
			Valid = true;
		}

		public bool Contains(ref Vector3D point)
		{
			Vector3D.Transform(ref point, ref m_worldInv, out var result);
			if (Math.Abs(result.X) < m_halfExtents.X && Math.Abs(result.Y) < m_halfExtents.Y)
			{
				return Math.Abs(result.Z) < m_halfExtents.Z;
			}
			return false;
		}

		public void Update()
		{
			m_worldInv = m_entity.PositionComp.WorldMatrixNormalizedInv;
			m_halfExtents = m_entity.PositionComp.LocalAABB.Extents;
		}

		public void DebugDraw()
		{
			MatrixD matrixD = MatrixD.Invert(m_worldInv);
			MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(MatrixD.CreateScale(m_halfExtents) * matrixD), Color.Red, 0.3f, depthRead: false, smooth: false);
		}

		private void OnEntityClosing(MyEntity entity)
		{
			Valid = false;
			m_entity = null;
		}
	}
}
