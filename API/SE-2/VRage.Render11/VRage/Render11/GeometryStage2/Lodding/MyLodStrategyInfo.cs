using VRage.Library.Collections;
using VRageRender.Import;

namespace VRage.Render11.GeometryStage2.Lodding
{
	internal struct MyLodStrategyInfo
	{
		public MyList<float> LodSwitchingDistances;

		public static float HisteresisRatio = 0.1f;

		public bool IsEmpty
		{
			get
			{
				if (LodSwitchingDistances != null)
				{
					return LodSwitchingDistances.Count == 0;
				}
				return true;
			}
		}

		public void Init(MyLODDescriptor[] lodDescriptors)
		{
			LodSwitchingDistances = new MyList<float>(lodDescriptors.Length);
			foreach (MyLODDescriptor myLODDescriptor in lodDescriptors)
			{
				LodSwitchingDistances.Add(myLODDescriptor.Distance);
			}
		}

		public int GetLodsCount()
		{
			if (LodSwitchingDistances == null)
			{
				return 1;
			}
			return LodSwitchingDistances.Count + 1;
		}

		public void ReduceLodsCount(int newLodsCount)
		{
			LodSwitchingDistances.SetSize(newLodsCount - 1);
		}

		public int GetTheBestLodWithHisteresis(float distance, int currentLod)
		{
			for (int i = 0; i < LodSwitchingDistances.Count; i++)
			{
				float num = LodSwitchingDistances[i];
				float num2 = num * HisteresisRatio;
				num2 = ((currentLod > i) ? (0f - num2) : num2);
				if (num + num2 > distance)
				{
					return i;
				}
			}
			return LodSwitchingDistances.Count;
		}
	}
}
