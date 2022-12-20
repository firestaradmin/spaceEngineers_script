using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
	internal class MyDebugRenderComponentWaypoint : MyDebugRenderComponent
	{
		private MyWaypoint m_waypoint;

		public MyDebugRenderComponentWaypoint(MyWaypoint waypoint)
			: base(waypoint)
		{
			m_waypoint = waypoint;
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_WAYPOINTS && m_waypoint.Visible)
			{
				Color color = (m_waypoint.Freeze ? Color.Gray : Color.Yellow);
				MatrixD worldMatrixRef = m_waypoint.PositionComp.WorldMatrixRef;
				MyRenderProxy.DebugDrawAxis(worldMatrixRef, 0.5f, depthRead: false);
				Vector3D translation = worldMatrixRef.Translation;
				worldMatrixRef = -worldMatrixRef;
				worldMatrixRef.Translation = translation;
				MyRenderProxy.DebugDrawAxis(worldMatrixRef, 0.5f, depthRead: false, skipScale: false, persistent: false, color);
				MyRenderProxy.DebugDrawText3D(m_waypoint.PositionComp.GetPosition() + new Vector3D(0.05000000074505806), m_waypoint.Name, color, 0.7f, depthRead: false);
			}
		}
	}
}
