using System;
using VRageMath;

namespace VRageRender
{
	public class MyTriangleBillboard : MyBillboard
	{
		public Vector2 UV0;

		public Vector2 UV1;

		public Vector2 UV2;

		public Vector3 Normal0;

		[Obsolete("Not used at all")]
		public Vector3 Normal1;

		[Obsolete("Not used at all")]
		public Vector3 Normal2;
	}
}
