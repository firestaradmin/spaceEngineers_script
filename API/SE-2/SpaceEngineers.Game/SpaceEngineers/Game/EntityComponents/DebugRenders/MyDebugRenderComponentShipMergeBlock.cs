using System;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using SpaceEngineers.Game.Entities.Blocks;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.EntityComponents.DebugRenders
{
	public class MyDebugRenderComponentShipMergeBlock : MyDebugRenderComponent
	{
		private MyShipMergeBlock m_shipMergeBlock;

		public MyDebugRenderComponentShipMergeBlock(MyShipMergeBlock shipConnector)
			: base(shipConnector)
		{
			m_shipMergeBlock = shipConnector;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_CONNECTORS_AND_MERGE_BLOCKS)
			{
				Matrix matrix = m_shipMergeBlock.PositionComp.WorldMatrixRef;
				MyRenderProxy.DebugDrawLine3D(m_shipMergeBlock.Physics.RigidBody.Position, m_shipMergeBlock.Physics.RigidBody.Position + m_shipMergeBlock.WorldMatrix.Right, Color.Green, Color.Green, depthRead: false);
				Vector3 position = m_shipMergeBlock.Position * m_shipMergeBlock.CubeGrid.GridSize;
				MatrixD m = m_shipMergeBlock.WorldMatrix;
				MyRenderProxy.DebugDrawSphere(Vector3.Transform(position, Matrix.Invert(m)), 1f, Color.Green, 1f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(m_shipMergeBlock.WorldMatrix.Translation, 0.2f, m_shipMergeBlock.InConstraint ? Color.Yellow : Color.Orange, 1f, depthRead: false);
				if (m_shipMergeBlock.InConstraint)
				{
					MyRenderProxy.DebugDrawSphere(m_shipMergeBlock.Other.WorldMatrix.Translation, 0.2f, Color.Yellow, 1f, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(m_shipMergeBlock.WorldMatrix.Translation, m_shipMergeBlock.Other.WorldMatrix.Translation, Color.Yellow, Color.Yellow, depthRead: false);
				}
				MyRenderProxy.DebugDrawLine3D(matrix.Translation, matrix.Translation + m_shipMergeBlock.CubeGrid.WorldMatrix.GetDirectionVector(Base6Directions.GetDirection(m_shipMergeBlock.PositionComp.LocalMatrixRef.Right)), Color.Red, Color.Red, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(matrix.Translation, matrix.Translation + m_shipMergeBlock.CubeGrid.WorldMatrix.GetDirectionVector(Base6Directions.GetDirection(m_shipMergeBlock.PositionComp.LocalMatrixRef.Up)), Color.Green, Color.Green, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(matrix.Translation, matrix.Translation + m_shipMergeBlock.CubeGrid.WorldMatrix.GetDirectionVector(Base6Directions.GetDirection(m_shipMergeBlock.PositionComp.LocalMatrixRef.Backward)), Color.Blue, Color.Blue, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(matrix.Translation, matrix.Translation + m_shipMergeBlock.CubeGrid.WorldMatrix.GetDirectionVector(m_shipMergeBlock.OtherRight), Color.Violet, Color.Violet, depthRead: false);
				MyRenderProxy.DebugDrawText3D(matrix.Translation, "Bodies: " + m_shipMergeBlock.GridCount, Color.White, 1f, depthRead: false);
				if (m_shipMergeBlock.Other != null)
				{
					MyRenderProxy.DebugDrawText3D(text: ((float)Math.Exp((0.0 - ((matrix.Translation - m_shipMergeBlock.Other.WorldMatrix.Translation).Length() - (double)m_shipMergeBlock.CubeGrid.GridSize)) * 6.0)).ToString("0.00"), worldCoord: matrix.Translation + m_shipMergeBlock.CubeGrid.WorldMatrix.GetDirectionVector(Base6Directions.GetDirection(m_shipMergeBlock.PositionComp.LocalMatrixRef.Up)) * 0.5, color: Color.Red, scale: 1f, depthRead: false);
				}
			}
		}
	}
}
