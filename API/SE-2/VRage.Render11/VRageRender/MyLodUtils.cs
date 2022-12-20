using System;
using VRageMath;

namespace VRageRender
{
	internal static class MyLodUtils
	{
		public const bool LOD_TRANSITION_DISTANCE = true;

		private const int MAX_LOD_COUNT = 8;

		private const float LodDistanceTransitionThreshold = 4f;

		private const float LodTransitionTime = 1f;

		private const float MinTranstionDistance = 4f;

		private static float[] m_lodTransitionVector;

		static MyLodUtils()
		{
			m_lodTransitionVector = new float[8];
			for (int i = 0; i < 8; i++)
			{
				m_lodTransitionVector[i] = GetLodTransitionBorder(i);
			}
		}

		public static float GetLodTransitionBorder(int lodIndex)
		{
			return Math.Max(4f * (float)Math.Pow(2.0, lodIndex), 4f);
		}

		public static float GetTransitionDelta(float distanceDelta, float currentState, int lodIndex)
		{
			float value = distanceDelta / m_lodTransitionVector[lodIndex] / 8f;
			value = Math.Max(currentState + MyCommon.GetLastFrameDelta() / MyCommon.LoddingSettings.Global.MaxTransitionInSeconds, MathHelper.Clamp(value, 0f, 1f));
			float val = Math.Abs(currentState - value);
			val = Math.Min(val, 0.25f);
			if (!val.IsValid())
			{
				val = 1f;
			}
			return val;
		}
	}
}
