using VRageRender;

namespace VRage.Render11.GeometryStage2.Lodding
{
	internal struct MyLodStrategyPreprocessor
	{
		public readonly float DistanceMult;

		private MyLodStrategyPreprocessor(float distanceMult)
		{
			DistanceMult = distanceMult;
		}

		public static MyLodStrategyPreprocessor Perform()
		{
			return new MyLodStrategyPreprocessor(MyCommon.LODCoefficient);
		}
	}
}
