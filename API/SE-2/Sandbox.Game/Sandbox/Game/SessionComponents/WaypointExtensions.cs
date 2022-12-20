using System;
using Sandbox.Game.Entities;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	internal static class WaypointExtensions
	{
		public static MyEntity GetWaypoint(string name)
		{
			if (MyEntities.TryGetEntityByName(name, out var entity))
			{
				return entity;
			}
			return null;
		}

		public static MatrixD GetWorldMatrix(this CutsceneSequenceNodeWaypoint waypoint)
		{
			MatrixD result = MatrixD.Identity;
			MyEntity waypoint2 = GetWaypoint(waypoint.Name);
			if (waypoint2 != null)
			{
				result = waypoint2.PositionComp.WorldMatrixRef;
			}
			return result;
		}

		public static Vector3D GetPosition(this CutsceneSequenceNodeWaypoint waypoint)
		{
			return GetWaypoint(waypoint.Name)?.PositionComp.GetPosition() ?? Vector3D.Zero;
		}

		public static Vector3D GetBezierPosition(this CutsceneSequenceNode cutsceneNode, float timeRatio)
		{
			Vector3D result = Vector3D.Zero;
			if (cutsceneNode.Waypoints.Count > 2)
			{
				float num = 1f / (float)(cutsceneNode.Waypoints.Count - 1);
				int num2 = (int)Math.Floor(timeRatio / num);
				float num3 = (timeRatio - (float)num2 * num) / num;
				if (num2 == cutsceneNode.Waypoints.Count - 1)
				{
					result = cutsceneNode.Waypoints[cutsceneNode.Waypoints.Count - 1].GetPosition();
				}
				else
				{
					if (num2 > cutsceneNode.Waypoints.Count - 2)
					{
						num2 = cutsceneNode.Waypoints.Count - 2;
					}
					result = ((num2 == 0) ? MathHelper.CalculateBezierPoint(num3, cutsceneNode.Waypoints[num2].GetPosition(), cutsceneNode.Waypoints[num2].GetPosition(), cutsceneNode.Waypoints[num2 + 1].GetPosition() - (cutsceneNode.Waypoints[num2 + 2].GetPosition() - cutsceneNode.Waypoints[num2].GetPosition()) / 4.0, cutsceneNode.Waypoints[num2 + 1].GetPosition()) : ((num2 <= cutsceneNode.Waypoints.Count - 3) ? MathHelper.CalculateBezierPoint(num3, cutsceneNode.Waypoints[num2].GetPosition(), cutsceneNode.Waypoints[num2].GetPosition() + (cutsceneNode.Waypoints[num2 + 1].GetPosition() - cutsceneNode.Waypoints[num2 - 1].GetPosition()) / 4.0, cutsceneNode.Waypoints[num2 + 1].GetPosition() - (cutsceneNode.Waypoints[num2 + 2].GetPosition() - cutsceneNode.Waypoints[num2].GetPosition()) / 4.0, cutsceneNode.Waypoints[num2 + 1].GetPosition()) : MathHelper.CalculateBezierPoint(num3, cutsceneNode.Waypoints[num2].GetPosition(), cutsceneNode.Waypoints[num2].GetPosition() + (cutsceneNode.Waypoints[num2 + 1].GetPosition() - cutsceneNode.Waypoints[num2 - 1].GetPosition()) / 4.0, cutsceneNode.Waypoints[num2 + 1].GetPosition(), cutsceneNode.Waypoints[num2 + 1].GetPosition())));
				}
			}
			return result;
		}

		public static MatrixD GetBezierOrientation(this CutsceneSequenceNode cutsceneNode, float timeRatio)
		{
			if (cutsceneNode.Waypoints.Count > 2)
			{
				Vector3 forward = Vector3.Forward;
				Vector3 up = Vector3.Up;
				float num = 1f / (float)(cutsceneNode.Waypoints.Count - 1);
				int num2 = (int)Math.Floor(timeRatio / num);
				float num3 = (timeRatio - (float)num2 * num) / num;
				if (num2 == cutsceneNode.Waypoints.Count - 1)
				{
					forward = cutsceneNode.Waypoints[cutsceneNode.Waypoints.Count - 1].GetWorldMatrix().Forward;
					up = cutsceneNode.Waypoints[cutsceneNode.Waypoints.Count - 1].GetWorldMatrix().Up;
				}
				else
				{
					if (num2 > cutsceneNode.Waypoints.Count - 2)
					{
						num2 = cutsceneNode.Waypoints.Count - 2;
					}
					if (num2 == 0)
					{
						forward = MathHelper.CalculateBezierPoint(num3, cutsceneNode.Waypoints[num2].GetWorldMatrix().Forward, cutsceneNode.Waypoints[num2].GetWorldMatrix().Forward, cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Forward - (cutsceneNode.Waypoints[num2 + 2].GetWorldMatrix().Forward - cutsceneNode.Waypoints[num2].GetWorldMatrix().Forward) / 4.0, cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Forward);
						up = MathHelper.CalculateBezierPoint(num3, cutsceneNode.Waypoints[num2].GetWorldMatrix().Up, cutsceneNode.Waypoints[num2].GetWorldMatrix().Up, cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Up - (cutsceneNode.Waypoints[num2 + 2].GetWorldMatrix().Up - cutsceneNode.Waypoints[num2].GetWorldMatrix().Up) / 4.0, cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Up);
					}
					else if (num2 > cutsceneNode.Waypoints.Count - 3)
					{
						forward = MathHelper.CalculateBezierPoint(num3, cutsceneNode.Waypoints[num2].GetWorldMatrix().Forward, cutsceneNode.Waypoints[num2].GetWorldMatrix().Forward + (cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Forward - cutsceneNode.Waypoints[num2 - 1].GetWorldMatrix().Forward) / 4.0, cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Forward, cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Forward);
						up = MathHelper.CalculateBezierPoint(num3, cutsceneNode.Waypoints[num2].GetWorldMatrix().Up, cutsceneNode.Waypoints[num2].GetWorldMatrix().Up + (cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Up - cutsceneNode.Waypoints[num2 - 1].GetWorldMatrix().Up) / 4.0, cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Up, cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Up);
					}
					else
					{
						forward = MathHelper.CalculateBezierPoint(num3, cutsceneNode.Waypoints[num2].GetWorldMatrix().Forward, cutsceneNode.Waypoints[num2].GetWorldMatrix().Forward + (cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Forward - cutsceneNode.Waypoints[num2 - 1].GetWorldMatrix().Forward) / 4.0, cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Forward - (cutsceneNode.Waypoints[num2 + 2].GetWorldMatrix().Forward - cutsceneNode.Waypoints[num2].GetWorldMatrix().Forward) / 4.0, cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Forward);
						up = MathHelper.CalculateBezierPoint(num3, cutsceneNode.Waypoints[num2].GetWorldMatrix().Up, cutsceneNode.Waypoints[num2].GetWorldMatrix().Up + (cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Up - cutsceneNode.Waypoints[num2 - 1].GetWorldMatrix().Up) / 4.0, cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Up - (cutsceneNode.Waypoints[num2 + 2].GetWorldMatrix().Up - cutsceneNode.Waypoints[num2].GetWorldMatrix().Up) / 4.0, cutsceneNode.Waypoints[num2 + 1].GetWorldMatrix().Up);
					}
				}
				return MatrixD.CreateWorld(Vector3D.Zero, forward, up);
			}
			return MatrixD.Identity;
		}
	}
}
