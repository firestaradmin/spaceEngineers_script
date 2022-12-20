using System;
using VRage.Utils;
using VRageMath;

namespace VRageRender
{
	public static class MyBillboardsHelper11
	{
		public static void AddPointBillboard(MyStringId material, Vector4 color, Vector3D origin, float radius, float angle, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, int customViewProjection = -1, float intensity = 1f)
		{
			if (MyRender11.DebugOverrides.BillboardsDynamic)
			{
				MyBillboard myBillboard = MyBillboardRenderer.AddBillboardOnce();
				if (myBillboard != null)
				{
					myBillboard.BlendType = blendType;
					myBillboard.UVOffset = Vector2.Zero;
					myBillboard.UVSize = Vector2.One;
					MyQuadD quad = default(MyQuadD);
					CreateBillboard(myBillboard, ref quad, material, ref color, ref origin, 1f, default(Vector2), 0f, intensity);
					myBillboard.Position0 = origin;
					myBillboard.Position2 = new Vector3D(radius, angle, 0.0);
					myBillboard.LocalType = MyBillboard.LocalTypeEnum.Point;
					myBillboard.CustomViewProjection = customViewProjection;
				}
			}
		}

		public static void AddLineBillboard(MyStringId material, Vector4 color, Vector3D origin, Vector3 directionNormalized, float length, float thickness, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, int customViewProjection = -1, float intensity = 1f)
		{
			if (MyRender11.DebugOverrides.BillboardsDynamic)
			{
				MyBillboard myBillboard = MyBillboardRenderer.AddBillboardOnce();
				if (myBillboard != null)
				{
					myBillboard.BlendType = blendType;
					myBillboard.UVOffset = Vector2.Zero;
					myBillboard.UVSize = Vector2.One;
					MyQuadD quad = default(MyQuadD);
					CreateBillboard(myBillboard, ref quad, material, ref color, ref origin, 1f, default(Vector2), 0f, intensity);
					myBillboard.Position0 = origin;
					myBillboard.Position1 = directionNormalized;
					myBillboard.Position2 = new Vector3D(length, thickness, 0.0);
					myBillboard.LocalType = MyBillboard.LocalTypeEnum.Line;
					myBillboard.CustomViewProjection = customViewProjection;
				}
			}
		}

		public static void AddBillboardOriented(MyStringId material, Vector4 color, Vector3D origin, Vector3 leftVector, Vector3 upVector, float radius, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float softParticleDistanceScale = 1f, int customViewProjection = -1)
		{
			AddBillboardOriented(material, color, origin, leftVector, upVector, radius, radius, blendType, softParticleDistanceScale, customViewProjection);
		}

		public static void AddBillboardOriented(MyStringId material, Vector4 color, Vector3D origin, Vector3 leftVector, Vector3 upVector, float radiusX, float radiusY, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float softParticleDistanceScale = 1f, int customViewProjection = -1)
		{
			if (MyRender11.DebugOverrides.BillboardsDynamic)
			{
				MyBillboard myBillboard = MyBillboardRenderer.AddBillboardOnce();
				if (myBillboard != null)
				{
					myBillboard.CustomViewProjection = customViewProjection;
					myBillboard.BlendType = blendType;
					myBillboard.LocalType = MyBillboard.LocalTypeEnum.Custom;
					MyUtils.GetBillboardQuadOriented(out var quad, ref origin, radiusX, radiusY, ref leftVector, ref upVector);
					CreateBillboard(myBillboard, ref quad, material, ref color, ref origin, softParticleDistanceScale);
				}
			}
		}

		public static void AddBillboardRotated(MyStringId material, Vector4 color, Vector3D origin, Vector3D cameraPosition, float radius, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float softParticleDistanceScale = 1f, int customViewProjection = -1)
		{
			AddBillboardRotated(material, color, origin, cameraPosition, radius, radius, blendType, softParticleDistanceScale, customViewProjection);
		}

		public static void AddBillboardRotated(MyStringId material, Vector4 color, Vector3D origin, Vector3D cameraPosition, float radiusX, float radiusY, MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard, float softParticleDistanceScale = 1f, int customViewProjection = -1)
		{
			if (MyRender11.DebugOverrides.BillboardsDynamic)
			{
				MyBillboard myBillboard = MyBillboardRenderer.AddBillboardOnce();
				if (myBillboard != null)
				{
					myBillboard.CustomViewProjection = customViewProjection;
					myBillboard.BlendType = blendType;
					myBillboard.LocalType = MyBillboard.LocalTypeEnum.Custom;
					MyUtils.GetBillboardQuadAdvancedRotated(out var quad, origin, radiusX, radiusY, 0f, cameraPosition);
					CreateBillboard(myBillboard, ref quad, material, ref color, ref origin, softParticleDistanceScale);
				}
			}
		}

		private static void CreateBillboard(MyBillboard billboard, ref MyQuadD quad, MyStringId material, ref Vector4 color, ref Vector3D origin, float softParticleDistanceScale = 1f, Vector2 uvOffset = default(Vector2), float reflectivity = 0f, float intensity = 1f)
		{
			if (!MyTransparentMaterials.ContainsMaterial(material))
			{
				material = MyTransparentMaterials.ErrorMaterial.Id;
				color = Vector4.One;
			}
			billboard.Material = material;
			billboard.Position0 = quad.Point0;
			billboard.Position1 = quad.Point1;
			billboard.Position2 = quad.Point2;
			billboard.Position3 = quad.Point3;
			billboard.UVOffset = uvOffset;
			billboard.UVSize = Vector2.One;
			billboard.DistanceSquared = (float)Vector3D.DistanceSquared(MyRender11.Environment.Matrices.CameraPosition, origin);
			billboard.Color = color;
			billboard.Reflectivity = reflectivity;
			billboard.ColorIntensity = intensity;
			billboard.ParentID = uint.MaxValue;
			MyTransparentMaterial material2 = MyTransparentMaterials.GetMaterial(billboard.Material);
			if (material2.AlphaMistingEnable)
			{
				billboard.Color *= MathHelper.Clamp(((float)Math.Sqrt(billboard.DistanceSquared) - material2.AlphaMistingStart) / (material2.AlphaMistingEnd - material2.AlphaMistingStart), 0f, 1f);
			}
			billboard.Color *= material2.Color;
			billboard.SoftParticleDistanceScale = softParticleDistanceScale;
		}
	}
}
