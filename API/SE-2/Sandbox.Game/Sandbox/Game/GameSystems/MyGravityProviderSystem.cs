using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using VRage.Collections;
using VRage.Game.Components;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 666)]
	public class MyGravityProviderSystem : MySessionComponentBase
	{
		private class GravityCollector
		{
			public Vector3 Gravity;

			private readonly Func<int, bool> CollectAction;

			private Vector3D WorldPoint;

			private MyDynamicAABBTreeD Tree;

			public GravityCollector()
			{
				CollectAction = CollectCallback;
			}

			public void Collect(MyDynamicAABBTreeD tree, ref Vector3D worldPoint)
			{
				Tree = tree;
				WorldPoint = worldPoint;
				tree.QueryPoint(CollectAction, ref worldPoint);
			}

			private bool CollectCallback(int proxyId)
			{
				IMyGravityProvider userData = Tree.GetUserData<IMyGravityProvider>(proxyId);
				if (userData.IsWorking && userData.IsPositionInRange(WorldPoint))
				{
					Gravity += userData.GetWorldGravity(WorldPoint);
				}
				return true;
			}
		}

		public const float G = 9.81f;

		private static Dictionary<IMyGravityProvider, int> m_proxyIdMap = new Dictionary<IMyGravityProvider, int>();

		private static MyDynamicAABBTreeD m_artificialGravityGenerators = new MyDynamicAABBTreeD(Vector3D.One * 10.0, 10.0);

		private static ConcurrentCachingList<IMyGravityProvider> m_naturalGravityGenerators = new ConcurrentCachingList<IMyGravityProvider>();

		[ThreadStatic]
		private static GravityCollector m_gravityCollector;

		protected override void UnloadData()
		{
			base.UnloadData();
			m_naturalGravityGenerators.ApplyChanges();
			if (m_proxyIdMap.Count <= 0)
			{
				_ = m_naturalGravityGenerators.Count;
				_ = 0;
			}
			m_proxyIdMap.Clear();
			m_artificialGravityGenerators.Clear();
			m_naturalGravityGenerators.ClearImmediate();
		}

		public static bool IsGravityReady()
		{
			return !m_artificialGravityGenerators.IsRootNull();
		}

		public static Vector3 CalculateTotalGravityInPoint(Vector3D worldPoint)
		{
			float naturalGravityMultiplier;
			Vector3 vector = CalculateNaturalGravityInPoint(worldPoint, out naturalGravityMultiplier);
			Vector3 vector2 = CalculateArtificialGravityInPoint(worldPoint, CalculateArtificialGravityStrengthMultiplier(naturalGravityMultiplier));
			return vector + vector2;
		}

		public static Vector3 CalculateArtificialGravityInPoint(Vector3D worldPoint, float gravityMultiplier = 1f)
		{
			if (gravityMultiplier == 0f)
			{
				return Vector3.Zero;
			}
			if (m_gravityCollector == null)
			{
				m_gravityCollector = new GravityCollector();
			}
			m_gravityCollector.Gravity = Vector3.Zero;
			m_gravityCollector.Collect(m_artificialGravityGenerators, ref worldPoint);
			return m_gravityCollector.Gravity * gravityMultiplier;
		}

		public static Vector3 CalculateNaturalGravityInPoint(Vector3D worldPoint)
		{
			float naturalGravityMultiplier;
			return CalculateNaturalGravityInPoint(worldPoint, out naturalGravityMultiplier);
		}

		public static Vector3 CalculateNaturalGravityInPoint(Vector3D worldPoint, out float naturalGravityMultiplier)
		{
			naturalGravityMultiplier = 0f;
			Vector3 zero = Vector3.Zero;
			m_naturalGravityGenerators.ApplyChanges();
			foreach (IMyGravityProvider naturalGravityGenerator in m_naturalGravityGenerators)
			{
				if (naturalGravityGenerator.IsPositionInRange(worldPoint))
				{
					Vector3 worldGravity = naturalGravityGenerator.GetWorldGravity(worldPoint);
					float gravityMultiplier = naturalGravityGenerator.GetGravityMultiplier(worldPoint);
					if (gravityMultiplier > naturalGravityMultiplier)
					{
						naturalGravityMultiplier = gravityMultiplier;
					}
					zero += worldGravity;
				}
			}
			return zero;
		}

		public static Vector3 CalculateNaturalGravityInPoint(Vector3D worldPoint, List<IMyGravityProvider> cache)
		{
			Vector3 zero = Vector3.Zero;
			foreach (IMyGravityProvider item in cache)
			{
				if (item.IsWorking && item.IsPositionInRange(worldPoint))
				{
					zero += item.GetWorldGravity(worldPoint);
				}
			}
			return zero;
		}

		public static List<IMyGravityProvider> GetOverlappingGravityProviders(BoundingBoxD with, List<IMyGravityProvider> cache)
		{
			m_naturalGravityGenerators.ApplyChanges();
			foreach (IMyGravityProvider naturalGravityGenerator in m_naturalGravityGenerators)
			{
				naturalGravityGenerator.GetProxyAABB(out var aabb);
				if (with.Intersects(aabb))
				{
					cache.Add(naturalGravityGenerator);
				}
			}
			return cache;
		}

		public static float CalculateHighestNaturalGravityMultiplierInPoint(Vector3D worldPoint)
		{
			float num = 0f;
			m_naturalGravityGenerators.ApplyChanges();
			foreach (IMyGravityProvider naturalGravityGenerator in m_naturalGravityGenerators)
			{
				if (naturalGravityGenerator.IsPositionInRange(worldPoint))
				{
					float gravityMultiplier = naturalGravityGenerator.GetGravityMultiplier(worldPoint);
					if (gravityMultiplier > num)
					{
						num = gravityMultiplier;
					}
				}
			}
			return num;
		}

		public static float CalculateArtificialGravityStrengthMultiplier(float naturalGravityMultiplier)
		{
			return MathHelper.Clamp(1f - naturalGravityMultiplier * 2f, 0f, 1f);
		}

		/// <summary>
		/// Returns the planet that has the most influential gravity well in the given world point.
		/// The most influential gravity well is defined as the planet that has the highest gravity in the point and
		/// if no such planet is found, it returns the planet, whose gravity well is the closest to the given point.
		/// </summary>
		/// <param name="worldPosition">Position to test for the strongest gravity well</param>
		/// <param name="nearestProvider"></param>
		/// <returns>Planet that has the most influential gravity well in the given world point</returns>
		public static double GetStrongestNaturalGravityWell(Vector3D worldPosition, out IMyGravityProvider nearestProvider)
		{
			double num = double.MinValue;
			nearestProvider = null;
			m_naturalGravityGenerators.ApplyChanges();
			foreach (IMyGravityProvider naturalGravityGenerator in m_naturalGravityGenerators)
			{
				float num2 = naturalGravityGenerator.GetWorldGravity(worldPosition).Length();
				if ((double)num2 > num)
				{
					num = num2;
					nearestProvider = naturalGravityGenerator;
				}
			}
			return num;
		}

		/// <summary>
		/// This quickly checks if a given position is in any natural gravity.
		/// </summary>
		/// <param name="position">Position to check</param>
		/// <param name="sphereSize">Sphere size to test with.</param>
		/// <returns>True if there is natural gravity at this position, false otherwise.</returns>
		public static bool IsPositionInNaturalGravity(Vector3D position, double sphereSize = 0.0)
		{
			sphereSize = MathHelper.Max(sphereSize, 0.0);
			m_naturalGravityGenerators.ApplyChanges();
			foreach (IMyGravityProvider naturalGravityGenerator in m_naturalGravityGenerators)
			{
				if (naturalGravityGenerator != null && naturalGravityGenerator.IsPositionInRange(position))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Checks if the specified trajectory intersects any natural gravity wells.
		/// </summary>
		/// <param name="start">Starting point of the trajectory.</param>
		/// <param name="end">Destination of the trajectory.</param>
		/// <param name="raySize">Size of the ray to test with. (Cylinder test)</param>
		/// DI: Do you mean capsule?
		/// <returns></returns>
		public static bool DoesTrajectoryIntersectNaturalGravity(Vector3D start, Vector3D end, double raySize = 0.0)
		{
			Vector3D value = start - end;
			if (Vector3D.IsZero(value))
			{
				return IsPositionInNaturalGravity(start, raySize);
			}
			RayD rayD = new RayD(start, Vector3.Normalize(value));
			raySize = MathHelper.Max(raySize, 0.0);
			m_naturalGravityGenerators.ApplyChanges();
			foreach (IMyGravityProvider naturalGravityGenerator in m_naturalGravityGenerators)
			{
				if (naturalGravityGenerator == null)
				{
					continue;
				}
				MySphericalNaturalGravityComponent mySphericalNaturalGravityComponent = naturalGravityGenerator as MySphericalNaturalGravityComponent;
				if (mySphericalNaturalGravityComponent != null)
				{
<<<<<<< HEAD
					BoundingSphereD sphere = new BoundingSphereD(mySphericalNaturalGravityComponent.Position, (double)mySphericalNaturalGravityComponent.GravityLimit + raySize);
					if (rayD.Intersects(sphere).HasValue)
=======
					BoundingSphereD boundingSphereD = new BoundingSphereD(mySphericalNaturalGravityComponent.Position, (double)mySphericalNaturalGravityComponent.GravityLimit + raySize);
					if (ray.Intersects(boundingSphereD).HasValue)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						return true;
					}
				}
			}
			return false;
		}

		public static void AddGravityGenerator(IMyGravityProvider gravityGenerator)
		{
			if (!m_proxyIdMap.ContainsKey(gravityGenerator))
			{
				gravityGenerator.GetProxyAABB(out var aabb);
				int value = m_artificialGravityGenerators.AddProxy(ref aabb, gravityGenerator, 0u);
				m_proxyIdMap.Add(gravityGenerator, value);
			}
		}

		public static void RemoveGravityGenerator(IMyGravityProvider gravityGenerator)
		{
			if (m_proxyIdMap.TryGetValue(gravityGenerator, out var value))
			{
				m_artificialGravityGenerators.RemoveProxy(value);
				m_proxyIdMap.Remove(gravityGenerator);
			}
		}

		public static void OnGravityGeneratorMoved(IMyGravityProvider gravityGenerator, ref Vector3 velocity)
		{
			if (m_proxyIdMap.TryGetValue(gravityGenerator, out var value))
			{
				gravityGenerator.GetProxyAABB(out var aabb);
				m_artificialGravityGenerators.MoveProxy(value, ref aabb, velocity);
			}
		}

		public static void AddNaturalGravityProvider(IMyGravityProvider gravityGenerator)
		{
			m_naturalGravityGenerators.Add(gravityGenerator);
		}

		public static void RemoveNaturalGravityProvider(IMyGravityProvider gravityGenerator)
		{
			m_naturalGravityGenerators.Remove(gravityGenerator);
		}
	}
}
