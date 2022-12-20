using System;
using VRageMath;

namespace VRageRender
{
	public static class MyVector3Extensions
	{
		public static Vector3 Round(this Vector3 v)
		{
			return new Vector3((float)Math.Round(v.X), (float)Math.Round(v.Y), (float)Math.Round(v.Z));
		}

		public static Vector3D Round(this Vector3D v)
		{
			return new Vector3D(Math.Round(v.X), Math.Round(v.Y), Math.Round(v.Z));
		}

		public static Vector3 HsvToRgb(this Vector3 hsv)
		{
			float num = hsv.X * 360f;
			float y = hsv.Y;
			float z = hsv.Z;
			int num2 = Convert.ToInt32(Math.Floor(num / 60f)) % 6;
			float num3 = (float)((double)(num / 60f) - Math.Floor(num / 60f));
			float num4 = z;
			float num5 = z * (1f - y);
			float num6 = z * (1f - num3 * y);
			float num7 = z * (1f - (1f - num3) * y);
<<<<<<< HEAD
			switch (num2)
			{
			case 0:
				return new Vector3(num4, num7, num5);
			case 1:
				return new Vector3(num6, num4, num5);
			case 2:
				return new Vector3(num5, num4, num7);
			case 3:
				return new Vector3(num5, num6, num4);
			case 4:
				return new Vector3(num7, num5, num4);
			default:
				return new Vector3(num4, num5, num6);
			}
=======
			return num2 switch
			{
				0 => new Vector3(num4, num7, num5), 
				1 => new Vector3(num6, num4, num5), 
				2 => new Vector3(num5, num4, num7), 
				3 => new Vector3(num5, num6, num4), 
				4 => new Vector3(num7, num5, num4), 
				_ => new Vector3(num4, num5, num6), 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
