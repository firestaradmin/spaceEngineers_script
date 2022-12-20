using System;
using VRage.Library.Utils;
using VRageMath;

namespace VRageRender
{
	internal class MyGravity
	{
		public readonly uint ParentID;

		public MyTimeSpan NextTime;

		public Vector3 Gravity;

		private readonly MyTimeSpan GRAVITY_UPDATE_DELAY = MyTimeSpan.FromSeconds(1.5);

		public MyGravity(uint id)
		{
			ParentID = id;
		}

		public void UpdateGravity(ref Vector3D wPos, out Vector3 gravityVec)
		{
			if (MyCommon.FrameTime > NextTime)
			{
				Vector3 vector = MyRender11.CalculateGravityInPoint(wPos);
				float num = vector.LengthSquared();
				if (num > 10000f)
				{
					float num2 = (float)Math.Sqrt(num);
					vector = vector / num2 * 100f;
				}
				Gravity = vector;
				NextTime = MyCommon.FrameTime + GRAVITY_UPDATE_DELAY;
			}
			gravityVec = Gravity;
		}
	}
}
