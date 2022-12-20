using System.Collections.Generic;
using Havok;
using Sandbox.Engine.Physics;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Entities.Character
{
	internal class MyMagneticBootsOnGridSmoothing
	{
		private MyCharacter m_character;

		private List<HkHitInfo> m_hkHitInfos = new List<HkHitInfo>();

		private List<Vector3D> m_rayCastOffsets = new List<Vector3D>();

		private MyPhysics.HitInfo m_hitInfo;

		private float m_minFraction;

		private float m_maxFraction;

		private const float MIN_NORMAL_DOT = 0.99999f;

		private const float MAX_HIT_FRACTION_DIFF = 0.03f;

		public Vector3 SupportNormal { get; internal set; }

		public MyMagneticBootsOnGridSmoothing(MyCharacter character)
		{
			m_character = character;
		}

		public bool CanUseRayCastNormal()
		{
			m_hkHitInfos.Clear();
			m_minFraction = float.MaxValue;
			m_maxFraction = float.MinValue;
			if (!AreRayCastsNormalsCompatible())
			{
				return false;
			}
			return true;
		}

		private bool AreRayCastsNormalsCompatible()
		{
			float dEFAULT_GROUND_SEARCH_DISTANCE = MyConstants.DEFAULT_GROUND_SEARCH_DISTANCE;
			Vector3D vector3D = m_character.PositionComp.GetPosition() + m_character.PositionComp.WorldMatrixRef.Up * 0.5;
			Vector3D vector3D2 = vector3D + m_character.PositionComp.WorldMatrixRef.Down * dEFAULT_GROUND_SEARCH_DISTANCE;
			m_rayCastOffsets.Clear();
			m_rayCastOffsets.Add(m_character.PositionComp.WorldMatrixRef.Forward * 0.10000000149011612 + m_character.PositionComp.WorldMatrixRef.Left * 0.30000001192092896);
			m_rayCastOffsets.Add(m_character.PositionComp.WorldMatrixRef.Forward * 0.10000000149011612 + m_character.PositionComp.WorldMatrixRef.Right * 0.30000001192092896);
			m_rayCastOffsets.Add(m_character.PositionComp.WorldMatrixRef.Backward * 0.10000000149011612 + m_character.PositionComp.WorldMatrixRef.Left * 0.20000000298023224);
			m_rayCastOffsets.Add(m_character.PositionComp.WorldMatrixRef.Backward * 0.10000000149011612 + m_character.PositionComp.WorldMatrixRef.Right * 0.20000000298023224);
			foreach (Vector3D rayCastOffset in m_rayCastOffsets)
			{
				RayCastGround(vector3D + rayCastOffset, vector3D2 + rayCastOffset);
				if (m_hkHitInfos.Count >= 2)
				{
					if (!IsRayCastNormalDotCompatible())
					{
						return false;
					}
					if (!IsHitFractionCompatible())
					{
						return false;
					}
				}
			}
			SupportNormal = m_hkHitInfos[0].Normal;
			return true;
		}

		private bool IsHitFractionCompatible()
		{
			if (m_minFraction > m_hitInfo.HkHitInfo.HitFraction)
			{
				m_minFraction = m_hitInfo.HkHitInfo.HitFraction;
			}
			if (m_maxFraction < m_hitInfo.HkHitInfo.HitFraction)
			{
				m_maxFraction = m_hitInfo.HkHitInfo.HitFraction;
			}
			return m_maxFraction - m_minFraction <= 0.03f;
		}

		private void RayCastGround(Vector3D from, Vector3D to)
		{
			MyPhysics.CastRay(from, to, out m_hitInfo, 18u, ignoreConvexShape: true);
			m_hkHitInfos.Add(m_hitInfo.HkHitInfo);
		}

		private bool IsRayCastNormalDotCompatible()
		{
			return 0.99999f <= m_hitInfo.HkHitInfo.Normal.Dot(m_hkHitInfos[0].Normal);
		}
	}
}
