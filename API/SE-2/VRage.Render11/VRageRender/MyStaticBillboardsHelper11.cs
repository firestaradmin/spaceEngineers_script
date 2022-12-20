using VRage.Render.Scene;
using VRage.Utils;
using VRageMath;

namespace VRageRender
{
	public class MyStaticBillboardsHelper11 : IMyBillboardsHelper
	{
		public void AddPointBillboard(MyStringId material, Vector4 color, Vector3D origin, float radius, float angle, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, int customViewProjection = -1, float intensity = 1f)
		{
			MyBillboardsHelper11.AddPointBillboard(material, color, origin, radius, angle, blendType, customViewProjection, intensity);
		}

		public void AddLineBillboard(MyStringId material, Vector4 color, Vector3D origin, Vector3 directionNormalized, float length, float thickness, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, int customViewProjection = -1, float intensity = 1f)
		{
			MyBillboardsHelper11.AddLineBillboard(material, color, origin, directionNormalized, length, thickness, blendType, customViewProjection, intensity);
		}

		public void AddBillboardOriented(MyStringId material, Vector4 color, Vector3D origin, Vector3 leftVector, Vector3 upVector, float radiusX, float radiusY, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float softParticleDistanceScale = 1f, int customViewProjection = -1)
		{
			MyBillboardsHelper11.AddBillboardOriented(material, color, origin, leftVector, upVector, radiusX, radiusY, blendType, softParticleDistanceScale, customViewProjection);
		}
	}
}
