using System;
using System.Threading;
using VRage.Render11.GeometryStage2.Common;
using VRageMath;
using VRageRender;

namespace VRage.Render11.GeometryStage2.Lodding
{
	internal class MyLodStrategy
	{
		private MyLodStrategyInfo m_lodStrategyInfo;

		private int m_currentLod;

		private int m_transitionLod;

		private float m_transition;

		private float m_transitionStartedAtDistance;

		private int m_maxLod;

		private MyInstanceLodState m_explicitState;

		private float m_explicitStateData;

		private long m_updatedAtFrameId;

		private const long MISSING_FRAMES_FOR_IMMEDIATE_LOD_SWITCH = 100L;

		private static float m_minTransitionInSeconds = 0.25f;

		private static float m_maxTransitionInSeconds = 1f;

		private static float m_transitionDeadZoneConst = 1f;

		private static float m_transitionDeadZoneDistanceMult = 0.1f;

		private static float m_objectDistanceAdd = 0f;

		private static float m_objectDistanceMult = 1f;

<<<<<<< HEAD
=======
		private bool m_fadeOut;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public int CurrentLod => m_currentLod;

		public int TransitionLod => m_transitionLod;

		public float Transition => m_transition;

		public MyInstanceLodState ExplicitLodState => m_explicitState;

		public float ExplicitStateData => m_explicitStateData;

		public int MaxLod => m_maxLod;

		public event Action OnTransitionEndOnce;

		public static void SetSettings(ref MyGlobalLoddingSettings globalLodding)
		{
			MyLodStrategyInfo.HisteresisRatio = globalLodding.HisteresisRatio;
			m_objectDistanceAdd = globalLodding.ObjectDistanceAdd;
			m_objectDistanceMult = globalLodding.ObjectDistanceMult;
			m_minTransitionInSeconds = globalLodding.MinTransitionInSeconds;
			m_maxTransitionInSeconds = globalLodding.MaxTransitionInSeconds;
			m_transitionDeadZoneConst = globalLodding.TransitionDeadZoneConst;
			m_transitionDeadZoneDistanceMult = globalLodding.TransitionDeadZoneDistanceMult;
		}

		private static int GetTheBestLod(MyLodStrategyInfo strategyInfo, float distance)
		{
			if (strategyInfo.IsEmpty)
			{
				return 0;
			}
			for (int i = 0; i < strategyInfo.LodSwitchingDistances.Count; i++)
			{
				if (strategyInfo.LodSwitchingDistances[i] > distance)
				{
					return i;
				}
			}
			return strategyInfo.LodSwitchingDistances.Count;
		}

		private void SmoothTransition(MyLodStrategyInfo strategyInfo, float timeDeltaSeconds, float distance)
		{
			if (m_transition == 0f)
			{
				m_transitionLod = strategyInfo.GetTheBestLodWithHisteresis(distance, m_currentLod);
				if (m_currentLod == m_transitionLod)
				{
					m_transitionLod = -1;
					return;
				}
				m_transitionStartedAtDistance = distance;
				int transitionLod = m_transitionLod;
				m_transitionLod = m_currentLod;
				m_currentLod = transitionLod;
			}
			float num = timeDeltaSeconds / m_maxTransitionInSeconds;
			float num2 = timeDeltaSeconds / m_minTransitionInSeconds;
			float num3 = Math.Abs(m_transitionStartedAtDistance - distance);
			float num4 = Math.Max(m_transitionDeadZoneConst, m_transitionDeadZoneDistanceMult * num3);
			if (num3 <= num4)
			{
				m_transition += num;
			}
			else
			{
				m_transition += num2;
			}
			if (m_transition >= 1f)
			{
				EndTransition();
			}
		}

		private void EndTransition()
		{
			m_transition = 0f;
			m_transitionLod = -1;
			if (this.OnTransitionEndOnce != null)
			{
				this.OnTransitionEndOnce();
				this.OnTransitionEndOnce = null;
			}
		}

		public void Init(MyLodStrategyInfo lodStrategyInfo)
		{
			m_lodStrategyInfo = lodStrategyInfo;
			m_currentLod = 0;
			m_transitionLod = -1;
			m_transition = 0f;
			m_transitionStartedAtDistance = 0f;
			m_updatedAtFrameId = 0L;
<<<<<<< HEAD
=======
			m_fadeOut = false;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			this.OnTransitionEndOnce = null;
			m_maxLod = ((!lodStrategyInfo.IsEmpty) ? (lodStrategyInfo.GetLodsCount() - 1) : 0);
		}

		public void CopyFrom(MyLodStrategy strategy)
		{
			m_currentLod = strategy.m_currentLod;
			m_transitionLod = strategy.m_transitionLod;
			m_transition = strategy.m_transition;
			m_transitionStartedAtDistance = strategy.m_transitionStartedAtDistance;
			m_explicitState = strategy.m_explicitState;
			m_explicitStateData = strategy.m_explicitStateData;
			m_updatedAtFrameId = strategy.m_updatedAtFrameId;
		}

		public void UpdateExplicitly(int lodNum)
		{
			if (!m_lodStrategyInfo.IsEmpty)
			{
				int val = m_lodStrategyInfo.GetLodsCount() - 1;
				m_currentLod = Math.Min(lodNum, val);
				EndTransition();
			}
		}

		public void UpdateWithoutTransition(ref Vector3 cameraTranslation, float scale, MyLodStrategyPreprocessor preprocessor)
		{
			long frameCounter = MyCommon.FrameCounter;
			if (!m_lodStrategyInfo.IsEmpty)
			{
				float num = cameraTranslation.Length();
				num /= scale;
				num *= preprocessor.DistanceMult;
				num += m_objectDistanceAdd;
				num *= m_objectDistanceMult;
				m_currentLod = GetTheBestLod(m_lodStrategyInfo, num);
				EndTransition();
				m_updatedAtFrameId = frameCounter;
			}
		}

		public void UpdateSmoothly(ref Vector3 cameraTranslation, float scale, MyLodStrategyPreprocessor preprocessor)
		{
			if (m_lodStrategyInfo.IsEmpty && m_transition == 0f)
			{
				return;
			}
			long frameCounter = MyCommon.FrameCounter;
			if (Interlocked.Exchange(ref m_updatedAtFrameId, frameCounter) != frameCounter)
			{
				float num = cameraTranslation.Length();
				num /= scale;
				num *= preprocessor.DistanceMult;
				num += m_objectDistanceAdd;
				num *= m_objectDistanceMult;
				if (100 < frameCounter - m_updatedAtFrameId)
				{
					m_currentLod = GetTheBestLod(m_lodStrategyInfo, num);
					EndTransition();
				}
				else
				{
					float lastFrameDelta = MyCommon.GetLastFrameDelta();
					SmoothTransition(m_lodStrategyInfo, lastFrameDelta, num);
				}
			}
		}

		public void SetExplicitLodState(MyInstanceLodState state, float stateData)
		{
			m_explicitState = state;
			m_explicitStateData = stateData;
		}

		public void StartTransition(float distance, bool fadeIn)
		{
			distance *= MyLodStrategyPreprocessor.Perform().DistanceMult;
			distance += m_objectDistanceAdd;
			distance *= m_objectDistanceMult;
			m_transitionStartedAtDistance = distance;
			m_transition = 0.001f;
			m_updatedAtFrameId = MyCommon.FrameCounter;
			if (fadeIn)
			{
				m_transitionLod = -1;
				m_currentLod = GetTheBestLod(m_lodStrategyInfo, distance);
			}
			else
			{
				m_transitionLod = m_currentLod;
				m_currentLod = -1;
<<<<<<< HEAD
=======
				m_fadeOut = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
