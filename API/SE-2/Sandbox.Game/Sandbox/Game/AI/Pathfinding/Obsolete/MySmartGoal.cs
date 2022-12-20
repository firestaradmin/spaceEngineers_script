using System;
using System.Collections.Generic;
using VRage.Algorithms;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
	public class MySmartGoal : IMyHighLevelPrimitiveObserver
	{
<<<<<<< HEAD
=======
		private MyNavigationPrimitive m_end;

		private MyHighLevelPrimitive m_hlEnd;

		private bool m_hlEndIsApproximate;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private Vector3D m_destinationCenter;

		private static readonly Func<MyNavigationPrimitive, float> m_hlPathfindingHeuristic = HlHeuristic;

		private static readonly Func<MyNavigationPrimitive, float> m_hlTerminationCriterion = HlCriterion;

		private static MySmartGoal m_pathfindingStatic;

		private readonly HashSet<MyHighLevelPrimitive> m_ignoredPrimitives;

		public IMyDestinationShape Destination { get; }

		public MyEntity EndEntity { get; private set; }

		public Func<MyNavigationPrimitive, float> PathfindingHeuristic { get; }

		public Func<MyNavigationPrimitive, float> TerminationCriterion { get; }

		public bool IsValid { get; private set; }

		public MySmartGoal(IMyDestinationShape goal, MyEntity entity = null)
		{
			Destination = goal;
			m_destinationCenter = goal.GetDestination();
			EndEntity = entity;
			if (EndEntity != null)
			{
				Destination.SetRelativeTransform(EndEntity.PositionComp.WorldMatrixNormalizedInv);
				EndEntity.OnClosing += m_endEntity_OnClosing;
			}
			PathfindingHeuristic = Heuristic;
			TerminationCriterion = Criterion;
			m_ignoredPrimitives = new HashSet<MyHighLevelPrimitive>();
			IsValid = true;
		}

		public void Invalidate()
		{
<<<<<<< HEAD
=======
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (EndEntity != null)
			{
				EndEntity.OnClosing -= m_endEntity_OnClosing;
				EndEntity = null;
			}
<<<<<<< HEAD
			foreach (MyHighLevelPrimitive ignoredPrimitive in m_ignoredPrimitives)
			{
				ignoredPrimitive.Parent.StopObservingPrimitive(ignoredPrimitive, this);
=======
			Enumerator<MyHighLevelPrimitive> enumerator = m_ignoredPrimitives.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyHighLevelPrimitive current = enumerator.get_Current();
					current.Parent.StopObservingPrimitive(current, this);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_ignoredPrimitives.Clear();
			IsValid = false;
		}

		public bool ShouldReinitPath()
		{
			return TargetMoved();
		}

		public void Reinit()
		{
			if (EndEntity != null)
			{
				Destination.UpdateWorldTransform(EndEntity.WorldMatrix);
				m_destinationCenter = Destination.GetDestination();
			}
		}

		public MyPath<MyNavigationPrimitive> FindHighLevelPath(MyPathfinding pathfinding, MyHighLevelPrimitive startPrimitive)
		{
			m_pathfindingStatic = this;
			MyPath<MyNavigationPrimitive> result = pathfinding.FindPath(startPrimitive, m_hlPathfindingHeuristic, m_hlTerminationCriterion, null, returnClosest: false);
			pathfinding.LastHighLevelTimestamp = pathfinding.GetCurrentTimestamp();
			m_pathfindingStatic = null;
			return result;
		}

		public MyPath<MyNavigationPrimitive> FindPath(MyPathfinding pathfinding, MyNavigationPrimitive startPrimitive)
		{
			throw new NotImplementedException();
		}

		public void IgnoreHighLevel(MyHighLevelPrimitive primitive)
		{
			if (!m_ignoredPrimitives.Contains(primitive))
			{
				primitive.Parent.ObservePrimitive(primitive, this);
				m_ignoredPrimitives.Add(primitive);
			}
		}

		private bool TargetMoved()
		{
			return Vector3D.DistanceSquared(m_destinationCenter, Destination.GetDestination()) > 4.0;
		}

		private void m_endEntity_OnClosing(MyEntity obj)
		{
			EndEntity = null;
			IsValid = false;
		}

		private float Heuristic(MyNavigationPrimitive primitive)
		{
			return (float)Vector3D.Distance(primitive.WorldPosition, m_destinationCenter);
		}

		private float Criterion(MyNavigationPrimitive primitive)
		{
			return Destination.PointAdmissibility(primitive.WorldPosition, 2f);
		}

		private static float HlHeuristic(MyNavigationPrimitive primitive)
		{
			return (float)Vector3D.RectangularDistance(primitive.WorldPosition, m_pathfindingStatic.m_destinationCenter) * 2f;
		}

		private static float HlCriterion(MyNavigationPrimitive primitive)
		{
			MyHighLevelPrimitive myHighLevelPrimitive = primitive as MyHighLevelPrimitive;
			if (myHighLevelPrimitive == null || m_pathfindingStatic.m_ignoredPrimitives.Contains(myHighLevelPrimitive))
			{
				return float.PositiveInfinity;
			}
			float num = m_pathfindingStatic.Destination.PointAdmissibility(primitive.WorldPosition, 8.7f);
			if (num < float.PositiveInfinity)
			{
				return num * 4f;
			}
			IMyHighLevelComponent component = myHighLevelPrimitive.GetComponent();
			if (component == null)
			{
				return float.PositiveInfinity;
			}
			if (!component.IsFullyExplored)
			{
				return (float)Vector3D.RectangularDistance(primitive.WorldPosition, m_pathfindingStatic.m_destinationCenter) * 8f;
			}
			return float.PositiveInfinity;
		}

		public void DebugDraw()
		{
<<<<<<< HEAD
			Destination.DebugDraw();
			foreach (MyHighLevelPrimitive ignoredPrimitive in m_ignoredPrimitives)
			{
				MyRenderProxy.DebugDrawSphere(ignoredPrimitive.WorldPosition, 0.5f, Color.Red, 1f, depthRead: false);
=======
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Destination.DebugDraw();
			Enumerator<MyHighLevelPrimitive> enumerator = m_ignoredPrimitives.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyRenderProxy.DebugDrawSphere(enumerator.get_Current().WorldPosition, 0.5f, Color.Red, 1f, depthRead: false);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
