using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRageMath;

namespace Sandbox.Game.Entities.Planet
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 500)]
	public class MyPlanets : MySessionComponentBase
	{
		private readonly List<MyPlanet> m_planets = new List<MyPlanet>();

		private readonly List<BoundingBoxD> m_planetAABBsCache = new List<BoundingBoxD>();

		private readonly Dictionary<MyDefinitionId, int> m_knownPlanetTypes = new Dictionary<MyDefinitionId, int>(MyDefinitionId.Comparer);

		public static MyPlanets Static { get; private set; }

		public event Action<MyPlanet> OnPlanetAdded;

		public event Action<MyPlanet> OnPlanetRemoved;

		public override void LoadData()
		{
			Static = this;
			base.LoadData();
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}

		public static void Register(MyPlanet myPlanet)
		{
			Static.m_planets.Add(myPlanet);
			Static.m_planetAABBsCache.Clear();
			Static.OnPlanetAdded.InvokeIfNotNull(myPlanet);
		}

		public static void UnRegister(MyPlanet myPlanet)
		{
			Static.m_planets.Remove(myPlanet);
			Static.m_planetAABBsCache.Clear();
			Static.OnPlanetRemoved.InvokeIfNotNull(myPlanet);
			lock (Static.m_knownPlanetTypes)
			{
				Static.m_knownPlanetTypes.TryGetValue(myPlanet.Generator.Id, out var value);
				value--;
				if (value == 0)
				{
					Static.m_knownPlanetTypes.Remove(myPlanet.Generator.Id);
				}
				else
				{
					Static.m_knownPlanetTypes[myPlanet.Generator.Id] = value;
				}
			}
		}

		public static List<MyPlanet> GetPlanets()
		{
			return Static?.m_planets;
		}

		public MyPlanet GetClosestPlanet(Vector3D position)
		{
			List<MyPlanet> planets = m_planets;
			if (planets.Count == 0)
			{
				return null;
			}
			return planets.MinBy((MyPlanet x) => (float)((Vector3D.DistanceSquared(x.PositionComp.GetPosition(), position) - (double)(x.AtmosphereRadius * x.AtmosphereRadius)) / 1000.0));
		}

		public ListReader<BoundingBoxD> GetPlanetAABBs()
		{
			if (m_planetAABBsCache.Count == 0)
			{
				foreach (MyPlanet planet in m_planets)
				{
					m_planetAABBsCache.Add(planet.PositionComp.WorldAABB);
				}
			}
			return m_planetAABBsCache;
		}

		public bool CanSpawnPlanet(MyPlanetGeneratorDefinition planetType, bool register, out string errorMessage)
		{
			if (!MyFakes.ENABLE_PLANETS)
			{
				errorMessage = MyTexts.GetString(MySpaceTexts.Notification_PlanetsNotSupported);
				return false;
			}
			lock (m_knownPlanetTypes)
			{
				if (!m_knownPlanetTypes.ContainsKey(planetType.Id) && m_knownPlanetTypes.Count + 1 > Session.SessionSettings.MaxPlanets)
				{
					errorMessage = string.Format(MyTexts.GetString(MySpaceTexts.Notification_PlanetNotWhitelisted), planetType.Id.SubtypeId.String);
					return false;
				}
				if (register)
				{
					m_knownPlanetTypes.TryGetValue(planetType.Id, out var value);
					value++;
					m_knownPlanetTypes[planetType.Id] = value;
				}
			}
			errorMessage = null;
			return true;
		}
	}
}
