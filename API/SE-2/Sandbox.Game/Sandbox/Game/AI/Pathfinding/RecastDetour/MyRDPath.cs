using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.RecastDetour
{
	public class MyRDPath : IMyPath
	{
		private readonly MyRDPathfinding m_pathfinding;

		private List<Vector3D> m_pathPoints;

		private int m_currentPointIndex;

		private readonly MyPlanet m_planet;

		public IMyDestinationShape Destination { get; }

		public IMyEntity EndEntity => null;

		public bool IsValid { get; private set; }

		public bool PathCompleted { get; private set; }

		public bool IsWaitingForTileGeneration { get; private set; }

		public MyRDPath(MyRDPathfinding pathfinding, Vector3D begin, IMyDestinationShape destination)
		{
			m_pathPoints = new List<Vector3D>();
			m_pathfinding = pathfinding;
			Destination = destination;
			m_currentPointIndex = 0;
			m_planet = GetClosestPlanet(begin);
			IsValid = m_planet != null;
		}

		public void Invalidate()
		{
			IsValid = false;
		}

		public bool GetNextTarget(Vector3D position, out Vector3D target, out float targetRadius, out IMyEntity relativeEntity)
		{
			target = Vector3D.Zero;
			relativeEntity = null;
			targetRadius = 0.8f;
			if (!IsValid)
			{
				return false;
			}
			if (m_pathPoints.Count == 0 || PathCompleted || !IsValid)
			{
				m_pathPoints = m_pathfinding.GetPath(m_planet, position, Destination.GetDestination(), out var allTilesGenerated);
<<<<<<< HEAD
				if (m_pathPoints == null)
				{
					return false;
				}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				IsWaitingForTileGeneration = !allTilesGenerated;
				if (m_pathPoints.Count < 2)
				{
					return false;
				}
				m_currentPointIndex = 1;
			}
			_ = m_currentPointIndex;
			_ = m_pathPoints.Count - 1;
			target = m_pathPoints[m_currentPointIndex];
			if (Math.Abs(Vector3.Distance(target, position)) < targetRadius)
			{
				if (m_currentPointIndex == m_pathPoints.Count - 1)
				{
					PathCompleted = true;
					return false;
				}
				m_currentPointIndex++;
				target = m_pathPoints[m_currentPointIndex];
			}
			return true;
		}

		public void ReInit(Vector3D position)
		{
		}

		public void DebugDraw()
		{
			if (m_pathPoints.Count > 0)
			{
				for (int i = 0; i < m_pathPoints.Count - 1; i++)
				{
					Vector3D pointFrom = m_pathPoints[i];
					Vector3D vector3D = m_pathPoints[i + 1];
					MyRenderProxy.DebugDrawLine3D(pointFrom, vector3D, Color.Blue, Color.Red, depthRead: true);
					MyRenderProxy.DebugDrawSphere(vector3D, 0.3f, Color.Yellow);
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Returns the planet closest to the given position
		/// </summary>
		/// <param name="position">3D Point from where the search is started</param>
		/// <returns>The closest planet</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static MyPlanet GetClosestPlanet(Vector3D position)
		{
			BoundingBoxD box = new BoundingBoxD(position - 100.0, position + 100f);
			return MyGamePruningStructure.GetClosestPlanet(ref box);
		}
	}
}
