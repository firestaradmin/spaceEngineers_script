using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Culling.Occlusion;
using VRage.Render11.Scene;
using VRage.Render11.Scene.Components;
using VRageMath;
using VRageRender.Lights;
using VRageRender.Messages;

namespace VRageRender
{
	internal static class MyFlareRenderer
	{
		private struct Data
		{
			public MyFlareDesc Desc;

			public MyFlareOcclusionData Query;

			public MyActor Owner;
		}

		private static readonly MyFreelist<Data> m_flares = new MyFreelist<Data>(256);

		private static readonly HashSet<FlareId> m_distantFlares = new HashSet<FlareId>();

		internal static FlareId Set(FlareId flareId, MyActor owner, MyFlareDesc desc)
		{
			if (desc.Enabled && desc.Glares.Length != 0)
			{
				if (flareId == FlareId.NULL)
				{
					FlareId flareId2 = default(FlareId);
					flareId2.Index = m_flares.Allocate();
					flareId = flareId2;
					if (desc.Type == MyGlareTypeEnum.Distant)
					{
						m_distantFlares.Add(flareId);
					}
				}
				else if (desc.Type == MyGlareTypeEnum.Distant)
				{
					if (m_flares.Data[flareId.Index].Desc.Type != MyGlareTypeEnum.Distant)
					{
						m_distantFlares.Add(flareId);
					}
				}
				else if (desc.Type != MyGlareTypeEnum.Distant && m_flares.Data[flareId.Index].Desc.Type == MyGlareTypeEnum.Distant)
				{
					m_distantFlares.Remove(flareId);
				}
				if (m_flares.Data[flareId.Index].Query == null)
				{
					m_flares.Data[flareId.Index].Query = MyManagers.FlareOcclusionRenderer.Get(flareId, desc.Glares[0].Material.ToString());
				}
				m_flares.Data[flareId.Index].Desc = desc;
				m_flares.Data[flareId.Index].Owner = owner;
			}
			else if (flareId != FlareId.NULL)
			{
				Remove(flareId);
				flareId = FlareId.NULL;
			}
			return flareId;
		}

		internal static void Remove(FlareId flareId)
		{
			if (flareId != FlareId.NULL)
			{
				if (m_flares.Data[flareId.Index].Query != null)
				{
					MyManagers.FlareOcclusionRenderer.Remove(flareId);
					m_flares.Data[flareId.Index].Query = null;
				}
				m_flares.Free(flareId.Index);
				m_distantFlares.Remove(flareId);
			}
		}

		internal static void Draw(IEnumerable<MyLightComponent> visibleLights, float intensityMultiplier)
		{
			bool flag = MyManagers.Ansel.Is360Capturing && MyManagers.Ansel.FramesSinceStartOfCapturing == 0;
			if (!(!MyManagers.Ansel.IsCaptureRunning || flag))
			{
				return;
			}
			foreach (MyLightComponent visibleLight in visibleLights)
			{
				Draw(visibleLight, intensityMultiplier, flag);
			}
		}

		private static void Draw(MyLightComponent light, float intensityMultiplier, bool anselException)
		{
			if (light.FlareId != FlareId.NULL && (!anselException || light.Data.Glare.Type == MyGlareTypeEnum.Distant))
			{
				MatrixD worldMatrix = light.Owner.WorldMatrix;
				Draw(light.FlareId, worldMatrix.Translation, worldMatrix.Forward, light.Data.SpotLightOn ? new Vector4(light.Data.SpotLight.Light.Color / light.Data.SpotIntensity, 1f) : new Vector4(light.Data.PointLight.Color / light.Data.PointIntensity, 1f), intensityMultiplier);
			}
		}

		private static void Draw(FlareId flareId, Vector3D position, Vector3D forward, Vector4 colorFactor, float intensityMultiplier)
		{
			Data data = m_flares.Data[flareId.Index];
			if (data.Query == null)
			{
				return;
			}
			float occlusionFactor = data.Query.OcclusionFactor;
			float num = data.Desc.Intensity * intensityMultiplier;
			if (num <= 1E-05f)
			{
				return;
			}
			if (data.Desc.Type == MyGlareTypeEnum.Directional)
			{
				float num2 = Vector3.Dot(Vector3.Normalize(MyRender11.Environment.Matrices.CameraPosition - position), forward);
				num2 -= 0.3f;
				if (num2 <= 0f)
				{
					return;
				}
				num2 *= 2f;
				num2 *= num2;
				num2 *= num2;
				float num3 = MathHelper.Saturate(num2);
				num *= num3;
			}
			Vector3 position2 = MyRender11.Environment.Matrices.CameraPosition - position;
			float num4 = data.Desc.MaxDistance * data.Desc.MaxDistance;
			float num5 = position2.LengthSquared();
			if (num5 > num4)
			{
				return;
			}
			float num6 = num4 * 0.8f * 0.8f;
			float num7 = 1f;
			if (num5 > num6)
			{
				num7 = 1f - (num5 - num6) / (num4 - num6);
				float num8 = num7 * num7;
				num *= num8 * num8;
				if (num <= 1E-05f)
				{
					return;
				}
			}
			data.Query.Visible = true;
			Vector2 vector = data.Desc.SizeMultiplier * num7;
			float amount = 0f;
			Vector2 vector2 = Vector2.One;
			Vector4 vector3 = Vector4.One;
			bool flag = true;
			MySubGlare[] glares = data.Desc.Glares;
			for (int i = 0; i < glares.Length; i++)
			{
				MySubGlare mySubGlare = glares[i];
				float num9 = ConvertOcclusionToIntensity(mySubGlare.OcclusionToIntensityCurve, occlusionFactor);
				if (num9 <= 1E-05f)
				{
					continue;
				}
				if (flag)
				{
					vector3 = Vector4.Transform(position2, MyRender11.Environment.Matrices.ViewProjectionAt0);
					vector3 /= vector3.W;
					vector2 = new Vector2(vector3.X, vector3.Y);
					amount = MathExt.Saturate(vector2.Length());
					flag = false;
				}
				float num10 = ((mySubGlare.Type != SubGlareType.Anamorphic && mySubGlare.Type != SubGlareType.AnamorphicInverted) ? MathHelper.Lerp(mySubGlare.ScreenIntensityMultiplierCenter, mySubGlare.ScreenIntensityMultiplierEdge, amount) : MathHelper.Lerp(mySubGlare.ScreenIntensityMultiplierCenter, mySubGlare.ScreenIntensityMultiplierEdge, Math.Abs(vector2.X)));
				Vector4 color = mySubGlare.Color * Vector4.Lerp(colorFactor, colorFactor.ToGray(), 0.5f) * (num * num10 * num9);
				if (color.W <= 1E-05f)
				{
					continue;
				}
				Vector3D origin;
				if (mySubGlare.ScreenCenterDistance.X != 0f || mySubGlare.ScreenCenterDistance.Y != 0f)
				{
					if (MyManagers.Ansel.Is360Capturing)
					{
						continue;
					}
					Vector2 vector4 = vector2 - vector2 * mySubGlare.ScreenCenterDistance;
					Vector3 vector5 = Vector3.Transform(new Vector3(vector4.X, vector4.Y, vector3.Z), MyRender11.Environment.Matrices.InvViewProjectionAt0);
					origin = MyRender11.Environment.Matrices.CameraPosition - (Vector3D)vector5;
				}
				else
				{
					origin = position;
				}
				Vector2 vector6 = mySubGlare.Size * (mySubGlare.FixedSize ? new Vector2(1f, 1f) : vector) * 10f;
				color.X /= 4f;
				color.Y /= 4f;
				color.Z /= 4f;
				switch (mySubGlare.Type)
				{
				case SubGlareType.Oriented:
					MyBillboardsHelper11.AddBillboardOriented(mySubGlare.Material, color, origin, MyRender11.Environment.Matrices.InvViewAt0.Left, MyRender11.Environment.Matrices.InvViewAt0.Up, vector6.X, vector6.Y, MyBillboard.BlendTypeEnum.AdditiveTop);
					break;
				case SubGlareType.Rotated:
					MyBillboardsHelper11.AddBillboardRotated(mySubGlare.Material, color, origin, MyRender11.Environment.Matrices.CameraPosition, vector6.X, vector6.Y, MyBillboard.BlendTypeEnum.AdditiveTop);
					break;
				case SubGlareType.Anamorphic:
					MyBillboardsHelper11.AddBillboardOriented(mySubGlare.Material, color, origin, MyRender11.Environment.Matrices.InvViewAt0.Left, MyRender11.Environment.Matrices.InvViewAt0.Up, Math.Max(vector6.X * (1f - Math.Abs(vector2.X)), 0.0001f), vector6.Y, MyBillboard.BlendTypeEnum.AdditiveTop);
					break;
				case SubGlareType.AnamorphicInverted:
					MyBillboardsHelper11.AddBillboardOriented(mySubGlare.Material, color, origin, MyRender11.Environment.Matrices.InvViewAt0.Left, MyRender11.Environment.Matrices.InvViewAt0.Up, Math.Max(vector6.X * Math.Abs(vector2.X), 0.0001f), vector6.Y, MyBillboard.BlendTypeEnum.AdditiveTop);
					break;
				}
			}
		}

		private static float ConvertOcclusionToIntensity(MySubGlare.KeyPoint[] curve, float occlusionFactor)
		{
			if (curve != null && curve.Length != 0)
			{
				if (occlusionFactor < curve[0].Occlusion)
				{
					return curve[0].Intensity;
				}
				if (occlusionFactor > curve[curve.Length - 1].Occlusion)
				{
					return curve[curve.Length - 1].Intensity;
				}
				for (int i = 1; i < curve.Length; i++)
				{
					if (occlusionFactor < curve[i].Occlusion)
					{
						float num = (occlusionFactor - curve[i - 1].Occlusion) / (curve[i].Occlusion - curve[i - 1].Occlusion);
						return curve[i - 1].Intensity + (curve[i].Intensity - curve[i - 1].Intensity) * num;
					}
				}
				return curve[curve.Length - 1].Intensity;
			}
			float num2 = occlusionFactor;
			num2 = 1f - num2 * num2;
			num2 -= 0.2f;
			return num2 * 1.25f;
		}

		public static void AddDistantFlares(MyList<MyLightComponent> visibleLights)
		{
<<<<<<< HEAD
			foreach (FlareId distantFlare in m_distantFlares)
			{
				MyLightComponent light = m_flares.Data[distantFlare.Index].Owner.GetLight();
				if (!visibleLights.Contains(light))
				{
					visibleLights.Add(m_flares.Data[distantFlare.Index].Owner.GetLight());
				}
			}
=======
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<FlareId> enumerator = m_distantFlares.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					FlareId current = enumerator.get_Current();
					MyLightComponent light = m_flares.Data[current.Index].Owner.GetLight();
					if (!visibleLights.Contains(light))
					{
						visibleLights.Add(m_flares.Data[current.Index].Owner.GetLight());
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
