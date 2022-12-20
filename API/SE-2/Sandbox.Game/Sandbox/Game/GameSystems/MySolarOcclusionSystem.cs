using System.Collections.Concurrent;
using System.Collections.Generic;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.World;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems
{
	public class MySolarOcclusionSystem : MyUpdateableGridSystem
	{
		public static readonly int TIMER_DELAY = 60;

		private static readonly float PLANET_SHADOW_MULTIPLIER = 20f;

		private static readonly float PLANET_RADIUS_SAFETY_OFFSET = 500f;

		private static readonly float PLANET_RADIUS_SAFETY_SCALE = 0.9f;

		private static readonly bool TEST_PLANET_INSIDES = true;

		public ConcurrentDictionary<long, IMySolarOccludable> m_occludables = new ConcurrentDictionary<long, IMySolarOccludable>();

		private bool m_isOccluded = true;

		private int m_innerTimer = TIMER_DELAY;

		private static bool DEBUG_DRAW = false;

		public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.AfterSimulation;

		public bool IsOccluded
		{
			get
			{
				return m_isOccluded;
			}
			private set
			{
				if (m_isOccluded != value)
				{
					m_isOccluded = value;
					base.Grid.SetSolarOcclusion(value);
				}
			}
		}

		public MySolarOcclusionSystem(MyCubeGrid grid)
			: base(grid)
		{
		}

		protected override void Update()
		{
			if (m_occludables.Count > 0)
			{
				if (m_innerTimer > 0)
				{
					m_innerTimer--;
					return;
				}
				IsOccluded = ComputeOcclusion();
				m_innerTimer = TIMER_DELAY;
			}
		}

		internal void RegisterOccludable(IMySolarOccludable occ)
		{
			if (m_occludables.Count == 0)
			{
				Schedule();
			}
			m_occludables.TryAdd(occ.GetEntityId(), occ);
		}

		internal void UnregisterOccludable(IMySolarOccludable occ)
		{
			m_occludables.Remove(occ.GetEntityId());
			if (m_occludables.Count <= 0)
			{
				DeSchedule();
			}
		}

		private bool ComputeOcclusion()
		{
			Vector3D position = base.Grid.PositionComp.GetPosition();
			List<MyPlanet> planets = MyPlanets.GetPlanets();
			if (DEBUG_DRAW || MyFakes.DrawSolarOcclusionOnce)
			{
				foreach (MyPlanet item in planets)
				{
					Vector3D position2 = item.PositionComp.GetPosition();
					float radius = item.Provider.Radius;
					float num = ((radius > PLANET_RADIUS_SAFETY_OFFSET) ? (radius - PLANET_RADIUS_SAFETY_OFFSET) : (radius * PLANET_RADIUS_SAFETY_SCALE));
					Vector3 vector = position - position2;
					Vector3 directionToSunNormalized = MySector.DirectionToSunNormalized;
					float num2 = Vector3.Dot(vector, directionToSunNormalized);
					MyFakes.DrawSolarOcclusionOnce = false;
					_ = Color.Red;
					if (!(num2 > 0f))
					{
						Vector3D vector3D = position2;
						Vector3 vector2 = PLANET_SHADOW_MULTIPLIER * radius * -directionToSunNormalized;
						Vector3 vector3 = num2 * directionToSunNormalized;
						MyRenderProxy.DebugDrawLine3D(vector3D, vector3D + vector2, Color.Red, Color.Red, depthRead: false, persistent: true);
						if ((MySector.DirectionToSunNormalized.X > 0f && vector2.X > vector3.X) || (MySector.DirectionToSunNormalized.X < 0f && vector2.X < vector3.X))
						{
							MyRenderProxy.DebugDrawLine3D(vector3D + vector2, vector3D + vector3, Color.Green, Color.Green, depthRead: false, persistent: true);
						}
						Vector3D vector3D2 = vector - directionToSunNormalized * num2;
						float num3 = (float)vector3D2.Length();
						Vector3D vector3D3 = vector3D2 / num3;
						MyRenderProxy.DebugDrawLine3D(vector3D + vector3, vector3D + vector3 + vector3D3 * num, Color.Red, Color.Red, depthRead: false, persistent: true);
						if (num3 > num)
						{
							MyRenderProxy.DebugDrawLine3D(vector3D + vector3 + vector3D3 * num, vector3D + vector3 + vector3D2, Color.Green, Color.Green, depthRead: false, persistent: true);
						}
					}
				}
			}
			foreach (MyPlanet item2 in planets)
			{
				Vector3D position3 = item2.PositionComp.GetPosition();
				float radius2 = item2.Provider.Radius;
				float num4 = ((radius2 > PLANET_RADIUS_SAFETY_OFFSET) ? (radius2 - PLANET_RADIUS_SAFETY_OFFSET) : (radius2 * PLANET_RADIUS_SAFETY_SCALE));
				Vector3 vector4 = position - position3;
				Vector3 directionToSunNormalized2 = MySector.DirectionToSunNormalized;
				float num5 = Vector3.Dot(vector4, directionToSunNormalized2);
				if (num5 > 0f)
				{
					if (TEST_PLANET_INSIDES && vector4.LengthSquared() < num4 * num4)
					{
						return true;
					}
					continue;
				}
				Vector3D vector3D4 = vector4 - directionToSunNormalized2 * num5;
				if (!(0f - num5 > PLANET_SHADOW_MULTIPLIER * radius2) && vector3D4.LengthSquared() < (double)(num4 * num4))
				{
					return true;
				}
			}
			return false;
		}
	}
}
