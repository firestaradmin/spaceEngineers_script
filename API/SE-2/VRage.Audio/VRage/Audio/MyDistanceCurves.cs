using SharpDX.X3DAudio;

namespace VRage.Audio
{
	internal static class MyDistanceCurves
	{
		internal class DistanceCurve
		{
			public CurvePoint[] Points { get; set; }

			public DistanceCurve(params CurvePoint[] points)
			{
				Points = points;
			}
		}

		internal static DistanceCurve CURVE_LINEAR;

		internal static DistanceCurve CURVE_QUADRATIC;

		internal static DistanceCurve CURVE_INVQUADRATIC;

		internal static DistanceCurve CURVE_CUSTOM_1;

		internal static DistanceCurve[] Curves;

		static MyDistanceCurves()
		{
			CurvePoint[] array = new CurvePoint[2];
			CurvePoint curvePoint = new CurvePoint
			{
				Distance = 0f,
				DspSetting = 1f
			};
			array[0] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 1f,
				DspSetting = 0f
			};
			array[1] = curvePoint;
			CURVE_LINEAR = new DistanceCurve(array);
			CurvePoint[] array2 = new CurvePoint[5];
			curvePoint = new CurvePoint
			{
				Distance = 0f,
				DspSetting = 1f
			};
			array2[0] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 0.25f,
				DspSetting = 0.9375f
			};
			array2[1] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 0.5f,
				DspSetting = 0.75f
			};
			array2[2] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 0.75f,
				DspSetting = 0.4375f
			};
			array2[3] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 1f,
				DspSetting = 0f
			};
			array2[4] = curvePoint;
			CURVE_QUADRATIC = new DistanceCurve(array2);
			CurvePoint[] array3 = new CurvePoint[5];
			curvePoint = new CurvePoint
			{
				Distance = 0f,
				DspSetting = 1f
			};
			array3[0] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 0.25f,
				DspSetting = 0.5625f
			};
			array3[1] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 0.5f,
				DspSetting = 0.25f
			};
			array3[2] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 0.75f,
				DspSetting = 0.0625f
			};
			array3[3] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 1f,
				DspSetting = 0f
			};
			array3[4] = curvePoint;
			CURVE_INVQUADRATIC = new DistanceCurve(array3);
			CurvePoint[] array4 = new CurvePoint[6];
			curvePoint = new CurvePoint
			{
				Distance = 0f,
				DspSetting = 1f
			};
			array4[0] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 0.038462f,
				DspSetting = 0.979592f
			};
			array4[1] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 0.384615f,
				DspSetting = 0.938776f
			};
			array4[2] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 0.576923f,
				DspSetting = 0.928571f
			};
			array4[3] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 0.769231f,
				DspSetting = 0.826531f
			};
			array4[4] = curvePoint;
			curvePoint = new CurvePoint
			{
				Distance = 1f,
				DspSetting = 0f
			};
			array4[5] = curvePoint;
			CURVE_CUSTOM_1 = new DistanceCurve(array4);
			Curves = new DistanceCurve[4] { CURVE_LINEAR, CURVE_QUADRATIC, CURVE_INVQUADRATIC, CURVE_CUSTOM_1 };
		}
	}
}
