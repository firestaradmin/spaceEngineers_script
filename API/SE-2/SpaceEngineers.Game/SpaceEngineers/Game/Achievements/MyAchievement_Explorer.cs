<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Achievements
{
	public class MyAchievement_Explorer : MySteamAchievementBase
	{
		private const uint CHECK_INTERVAL_S = 3u;

		private const uint PLANET_COUNT = 6u;

		private BitArray m_exploredPlanetData;

		private readonly int[] m_bitArrayConversionArray = new int[1];

		private int m_planetsDiscovered;

		private uint m_lastCheckS;

		private readonly Dictionary<MyStringHash, int> m_planetNamesToIndexes = new Dictionary<MyStringHash, int>();

		private bool m_globalConditionsMet;

		public override bool NeedsUpdate => m_globalConditionsMet;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_Explorer", "Explorer_ExplorePlanetsData", 6f);
		}

		public override void Init()
		{
			base.Init();
			if (!base.IsAchieved)
			{
				m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Alien"), 0);
				m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("EarthLike"), 1);
				m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Europa"), 2);
				m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Mars"), 3);
				m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Moon"), 4);
				m_planetNamesToIndexes.Add(MyStringHash.GetOrCompute("Titan"), 5);
			}
		}

		public override void SessionLoad()
		{
			m_globalConditionsMet = !MySession.Static.CreativeMode;
			m_lastCheckS = 0u;
		}

		public override void SessionUpdate()
		{
			if (MySession.Static.LocalCharacter == null || base.IsAchieved)
			{
				return;
			}
			uint num = (uint)MySession.Static.ElapsedPlayTime.TotalSeconds;
			if (num - m_lastCheckS <= 3 || MySession.Static.LocalCharacter == null)
			{
				return;
			}
			Vector3D position = MySession.Static.LocalCharacter.PositionComp.GetPosition();
			Vector3 vector = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
			m_lastCheckS = num;
			if (vector == Vector3.Zero)
			{
				return;
			}
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
<<<<<<< HEAD
			if (closestPlanet == null || !m_planetNamesToIndexes.TryGetValue(closestPlanet.Generator.Id.SubtypeId, out var value) || m_exploredPlanetData[value])
			{
				return;
			}
			m_exploredPlanetData[value] = true;
=======
			if (closestPlanet == null || !m_planetNamesToIndexes.TryGetValue(closestPlanet.Generator.Id.SubtypeId, out var value) || m_exploredPlanetData.get_Item(value))
			{
				return;
			}
			m_exploredPlanetData.set_Item(value, true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_planetsDiscovered = 0;
			for (int i = 0; (long)i < 6L; i++)
			{
				if (m_exploredPlanetData.get_Item(i))
				{
					m_planetsDiscovered++;
				}
			}
			StoreSteamData();
			if ((long)m_planetsDiscovered < 6L)
			{
				m_remoteAchievement.IndicateProgress((uint)m_planetsDiscovered);
			}
			else
			{
				NotifyAchieved();
			}
		}

		protected override void LoadStatValue()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			int statValueConditionBitField = m_remoteAchievement.StatValueConditionBitField;
			m_exploredPlanetData = new BitArray(new int[1] { statValueConditionBitField });
		}

		private void StoreSteamData()
		{
			m_exploredPlanetData.CopyTo((Array)m_bitArrayConversionArray, 0);
			m_remoteAchievement.StatValueConditionBitField = m_bitArrayConversionArray[0];
		}
	}
}
