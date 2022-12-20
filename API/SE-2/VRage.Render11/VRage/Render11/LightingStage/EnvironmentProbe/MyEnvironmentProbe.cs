using System;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage.Library.Utils;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Scene.Components;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace VRage.Render11.LightingStage.EnvironmentProbe
{
	internal class MyEnvironmentProbe : IManager, IManagerUnloadData, IManagerDevice
	{
		private const bool BLEND_USING_FACTOR = false;

		internal IRtvArrayTexture CloseCubemapFinal;

		internal IRtvArrayTexture CloseCubemapOriginal;

		internal IRtvArrayTexture FarCubemapFinal;

		internal IRtvArrayTexture FarCubemapOriginal;

		private int m_state;

		private MyTimeSpan m_lastUpdateTime;

<<<<<<< HEAD
=======
		private Vector3D m_position;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private IRtvArrayTexture m_workCloseCubemapPrefiltered;

		private IRtvArrayTexture m_workCloseCubemapPrefilteredAlt;

		private IRtvArrayTexture m_workFarCubemapPrefiltered;

		private IRtvArrayTexture m_workFarCubemapPrefilteredAlt;

		private IDepthArrayTexture m_workCubemapDepth;

		private bool m_blendDone;

		private float m_timeOut;

		private uint? m_nearestAtmosphereId;

		private bool m_blendClose;

		private bool m_initWorkProbes;

		public static float LastAmbient { get; private set; }

		internal void UpdateCullQuery(MyRenderContext rc, MyCullQueries cullQueries)
		{
			if (CloseCubemapFinal == null || CloseCubemapFinal.Size.X != MyRender11.Environment.Data.EnvMapResolution)
			{
				CreateResources(rc, MyRender11.Environment.Data.EnvMapResolution);
				m_lastUpdateTime = MyTimeSpan.Zero;
				m_state = 0;
			}
			if (CloseCubemapFinal == null)
			{
				return;
			}
<<<<<<< HEAD
			if (m_lastUpdateTime == MyTimeSpan.Zero || MyRender11.Environment.Data.EnvironmentProbe.TimeOut == 0f || MyRender11.DebugOverrides.DisableEnvironmentLight)
=======
			if (m_lastUpdateTime == MyTimeSpan.Zero || MyRender11.Environment.Data.EnvironmentProbe.TimeOut == 0f)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_blendDone = false;
				AddAllProbes(rc, cullQueries);
				return;
			}
			if (m_state < 0)
			{
				m_initWorkProbes = true;
				m_state = 0;
			}
			if (m_state == 0)
			{
				m_timeOut = MyRender11.Environment.Data.EnvironmentProbe.TimeOut;
				m_lastUpdateTime = MyCommon.FrameTime;
				m_blendDone = false;
			}
			if (m_state < 6)
			{
				AddProbe(rc, m_state, cullQueries);
				m_state++;
			}
		}

		internal void FinalizeEnvProbes(MyRenderContext rc)
		{
			if (CloseCubemapFinal == null)
			{
				return;
			}
			if (m_initWorkProbes)
			{
				InitWorkProbes(rc);
				m_initWorkProbes = false;
			}
<<<<<<< HEAD
			if (m_lastUpdateTime == MyTimeSpan.Zero || MyRender11.Environment.Data.EnvironmentProbe.TimeOut == 0f || MyRender11.DebugOverrides.DisableEnvironmentLight)
=======
			if (m_lastUpdateTime == MyTimeSpan.Zero || MyRender11.Environment.Data.EnvironmentProbe.TimeOut == 0f)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				PostprocessProbes(rc);
				m_lastUpdateTime = MyCommon.FrameTime;
				m_state = -1;
				return;
			}
			if (m_state >= 6 && m_state < 12)
			{
				int nProbe = m_state - 6;
				PostprocessProbe(rc, nProbe);
				RenderTransparent(rc, nProbe, CloseCubemapOriginal, FarCubemapOriginal);
				m_state++;
			}
			else if (m_state == 12)
			{
				rc.GenerateMips(FarCubemapOriginal);
				m_state++;
			}
			else if (m_state == 13)
			{
				rc.GenerateMips(CloseCubemapOriginal);
				m_state++;
			}
			else if (m_state >= 14 && m_state < 20)
			{
				int nProbe2 = m_state - 14;
				int skipIBLevels = MyRender11.Environment.Data.EnvironmentLight.SkipIBLevels;
				Prefilter(rc, nProbe2, skipIBLevels, m_workCloseCubemapPrefiltered, m_workFarCubemapPrefiltered);
				m_state++;
			}
			else if (m_state >= 20)
			{
				MyTimeSpan myTimeSpan = m_lastUpdateTime + MyTimeSpan.FromSeconds(m_timeOut);
				if (MyCommon.FrameTime >= myTimeSpan)
				{
					m_state = 0;
				}
				else
				{
					m_state++;
				}
			}
			float num = (float)(MyCommon.FrameTime - m_lastUpdateTime).Seconds;
			float num2 = Math.Min(1f, num / m_timeOut);
			if (num2 == 1f)
			{
				m_blendDone = true;
			}
			float val = 1f / MyCommon.GetLastFrameDelta() * m_timeOut;
			float num3 = Math.Max(10f, val);
			float blendStrength = 1f / num3;
			BlendAllProbes(rc, useFactor: false, blendStrength, num2);
		}

		private void PostprocessProbes(MyRenderContext rc)
		{
			for (int i = 0; i < 6; i++)
			{
				PostprocessProbe(rc, i);
				RenderTransparent(rc, i, CloseCubemapOriginal, FarCubemapOriginal);
			}
			BuildMipMaps(rc);
			for (int j = 0; j < 6; j++)
			{
				Prefilter(rc, j, MyRender11.Environment.Data.EnvironmentLight.SkipIBLevels, CloseCubemapFinal, FarCubemapFinal);
			}
		}

		private void RenderTransparent(MyRenderContext rc, int nProbe, IRtvArrayTexture dest0, IRtvArrayTexture dest1)
		{
			ISrvBindable lastFrameSource = dest0.SubresourceSrv(nProbe);
			ISrvBindable lastFrame = m_workCloseCubemapPrefiltered.SubresourceSrv(nProbe);
			Matrix viewMatrix = CubeFaceViewMatrix(Vector3.Zero, nProbe);
			Matrix projMatrix = GetProjectionMatrixInfinite();
			Vector3D position = MyRender11.Environment.Matrices.CameraPosition;
			MyTransparentRendering.RenderForward(rc, dest0.SubresourceRtv(nProbe), dest1.SubresourceRtv(nProbe), m_workCubemapDepth.SubresourceDsvRo(nProbe), ref viewMatrix, ref projMatrix, ref position, m_workCubemapDepth.SubresourceSrv(nProbe), m_nearestAtmosphereId, lastFrameSource, lastFrame);
			rc.PixelShader.SetSrv(11, null);
			rc.PixelShader.SetSrv(17, null);
		}

		private void BuildMipMaps(MyRenderContext rc)
		{
			rc.GenerateMips(FarCubemapOriginal);
			rc.GenerateMips(CloseCubemapOriginal);
		}

		private void Prefilter(MyRenderContext rc, int nProbe, int skipMipLevels, IRtvArrayTexture closeCubemap, IRtvArrayTexture farCubemap)
		{
			MyEnvProbeProcessing.Prefilter(rc, nProbe, skipMipLevels, CloseCubemapOriginal, closeCubemap);
			MyEnvProbeProcessing.Prefilter(rc, nProbe, skipMipLevels, FarCubemapOriginal, farCubemap);
		}

		private void InitWorkProbes(MyRenderContext rc)
		{
			rc.CopyResource(CloseCubemapFinal, m_workCloseCubemapPrefiltered);
			rc.CopyResource(FarCubemapFinal, m_workFarCubemapPrefiltered);
		}

		private void AddAllProbes(MyRenderContext rc, MyCullQueries cullQueries)
		{
			for (int i = 0; i < 6; i++)
			{
				AddProbe(rc, i, cullQueries);
			}
		}

		internal void UpdateProbe()
		{
			if (m_state <= 0)
			{
				m_lastUpdateTime = MyCommon.FrameTime;
				m_nearestAtmosphereId = MyAtmosphereRenderer.GetNearestAtmosphereId(MyRender11.Environment.Matrices.CameraPosition);
				LastAmbient = GatherLightAmbient() * MyRender11.Environment.Data.EnvironmentProbe.AmbientScale;
				if (LastAmbient < MyRender11.Environment.Data.EnvironmentProbe.AmbientMinClamp)
				{
					LastAmbient = 0f;
				}
				else if (LastAmbient < MyRender11.Environment.Data.EnvironmentProbe.AmbientMinClamp * 2f)
				{
					LastAmbient = MathHelper.Lerp(0f, LastAmbient, LastAmbient / MyRender11.Environment.Data.EnvironmentProbe.AmbientMinClamp - 1f);
				}
				else
				{
					LastAmbient = Math.Min(LastAmbient, MyRender11.Environment.Data.EnvironmentProbe.AmbientMaxClamp);
				}
			}
		}

		private float GatherLightAmbient()
		{
			float ambientLightsGatherRadius = MyRender11.Environment.Data.EnvironmentLight.AmbientLightsGatherRadius;
			float num = ambientLightsGatherRadius * ambientLightsGatherRadius;
			Vector3D cameraPosition = MyRender11.Environment.Matrices.CameraPosition;
			MyCullResults myCullResults = MyLightsRendering.GatherLights(cameraPosition, ambientLightsGatherRadius);
			float num2 = 0f;
			int num3 = 0;
			foreach (MyLightComponent pointLight in myCullResults.PointLights)
			{
				float num4 = 0f;
				if (pointLight.Data.PointLightOn && pointLight.Data.PointLight.Range > 0.5f)
				{
					UpdateRenderLightData data = pointLight.Data;
					float num5 = (float)(pointLight.Owner.WorldMatrix.Translation - cameraPosition).LengthSquared();
					float num6 = ambientLightsGatherRadius + data.PointLight.Range;
					if (num5 < num6 * num6)
					{
						float num7 = (float)Math.Sqrt(num5);
						float num8 = 1f / (1f + 0.25f * data.PointLight.Falloff);
						float num9 = (data.PointLight.Color.X + data.PointLight.Color.Y + data.PointLight.Color.Z) / 3f;
						float num10 = data.PointLight.Range * data.PointLight.Range / num;
						float num11 = 1f;
						if (num7 > ambientLightsGatherRadius)
						{
							num11 = 1f - (num7 - ambientLightsGatherRadius) / data.PointLight.Range;
						}
						num4 = num11 * num8 * num10 * num9;
						num3++;
					}
				}
				num2 += num4;
			}
			foreach (MyLightComponent spotLight in myCullResults.SpotLights)
			{
				float num12 = 0f;
				if (spotLight.Data.SpotLightOn)
				{
					UpdateRenderLightData data2 = spotLight.Data;
					float num13 = data2.SpotLight.Light.Range / 2f + ambientLightsGatherRadius * 2f;
					Vector3D vector3D = spotLight.Owner.WorldMatrix.Translation - spotLight.Owner.WorldMatrix.Forward * ambientLightsGatherRadius - cameraPosition;
					float num14 = (float)vector3D.LengthSquared();
					if (num14 < num13 * num13)
					{
						float num15 = (float)Math.Sqrt(num14);
						if (MathHelper.Saturate(0.0 - (vector3D / num15).Dot(spotLight.Owner.WorldMatrix.Forward)) > (double)data2.SpotLight.ApertureCos)
						{
							float num16 = 1f / (1f + 0.25f * data2.SpotLight.Light.Falloff);
							float num17 = (data2.SpotLight.Light.Color.X + data2.SpotLight.Light.Color.Y + data2.SpotLight.Light.Color.Z) / 3f / 2f;
							num12 = MathHelper.Saturate(1f - (num15 - ambientLightsGatherRadius) / num13) * num16 * num17;
							num3++;
						}
					}
				}
				num2 += num12;
			}
			MyLightsRendering.GatherLightsClear();
			return num2;
		}

		private void AddProbe(MyRenderContext rc, int nProbe, MyCullQueries cullQueries)
		{
			rc.ClearDsv(m_workCubemapDepth.SubresourceDsv(nProbe), DepthStencilClearFlags.Depth, 0f, 0);
			Vector3D viewOrigin = MyRender11.Environment.Matrices.CameraPosition;
			Matrix offsetedViewProjection = PrepareLocalRenderMatrix(MyRender11.Environment.Matrices.CameraPosition - viewOrigin, nProbe);
			Matrix matrix = PrepareLocalCullingMatrix(MyRender11.Environment.Matrices.CameraPosition - viewOrigin, nProbe, MyRender11.Environment.Data.EnvironmentProbe.DrawDistance);
			MatrixD viewProjection = MatrixD.CreateTranslation(-viewOrigin) * matrix;
			Matrix projection = GetProjectionMatrixInfinite();
			MyViewport viewport = new MyViewport(CloseCubemapOriginal.Size);
			cullQueries.AddForwardPass(nProbe, ref offsetedViewProjection, ref viewProjection, ref viewOrigin, ref projection, ref viewport, m_workCubemapDepth.SubresourceDsv(nProbe), m_workCubemapDepth.SubresourceSrv(nProbe), CloseCubemapOriginal.SubresourceRtv(nProbe), FarCubemapOriginal.SubresourceRtv(nProbe));
		}

		private void PostprocessProbe(MyRenderContext rc, int nProbe)
		{
			PostprocessProbe(rc, nProbe, FarCubemapOriginal);
			PostprocessProbe(rc, nProbe, CloseCubemapOriginal);
		}

		private void PostprocessProbe(MyRenderContext rc, int nProbe, IRtvArrayTexture source)
		{
			Matrix viewMatrix = CubeFaceViewMatrix(Vector3.Zero, nProbe);
			Matrix projMatrix = GetProjectionMatrixInfinite();
			MyEnvProbeProcessing.RunForwardPostprocess(rc, source.SubresourceRtv(nProbe), m_workCubemapDepth.SubresourceSrv(nProbe), ref viewMatrix, ref projMatrix);
		}

		private void BlendAllProbes(MyRenderContext rc, bool useFactor, float blendStrength, float blendFactor)
		{
			m_blendClose = !m_blendClose;
			if (m_blendClose)
			{
				MyEnvProbeProcessing.Blend(rc, useFactor, CloseCubemapFinal, m_workCloseCubemapPrefiltered, m_workCloseCubemapPrefilteredAlt, blendStrength, blendFactor);
			}
			else
			{
				MyEnvProbeProcessing.Blend(rc, useFactor, FarCubemapFinal, m_workFarCubemapPrefiltered, m_workFarCubemapPrefilteredAlt, blendStrength, blendFactor);
			}
		}

		private static Matrix CubeFaceViewMatrix(Vector3 pos, int faceId)
		{
			Matrix result = Matrix.Identity;
			switch (faceId)
			{
			case 0:
				result = Matrix.CreateLookAt(pos, pos + Vector3.Left, Vector3.Up);
				break;
			case 1:
				result = Matrix.CreateLookAt(pos, pos + Vector3.Right, Vector3.Up);
				break;
			case 2:
				result = Matrix.CreateLookAt(pos, pos + Vector3.Up, -Vector3.Backward);
				break;
			case 3:
				result = Matrix.CreateLookAt(pos, pos + Vector3.Down, -Vector3.Forward);
				break;
			case 4:
				result = Matrix.CreateLookAt(pos, pos + Vector3.Backward, Vector3.Up);
				break;
			case 5:
				result = Matrix.CreateLookAt(pos, pos + Vector3.Forward, Vector3.Up);
				break;
			}
			return result;
		}

		private static Matrix GetProjectionMatrix(float farPlane)
		{
			return Matrix.CreatePerspectiveFovRhComplementary((float)Math.PI / 2f, 1f, 0.01f, farPlane);
		}

		private static Matrix GetProjectionMatrixInfinite()
		{
			return Matrix.CreatePerspectiveFovRhInfiniteComplementary((float)Math.PI / 2f, 1f, 0.01f);
		}

		private static Matrix PrepareLocalRenderMatrix(Vector3 pos, int faceId)
		{
			Matrix projectionMatrixInfinite = GetProjectionMatrixInfinite();
			return CubeFaceViewMatrix(pos, faceId) * projectionMatrixInfinite;
		}

		private static Matrix PrepareLocalCullingMatrix(Vector3 pos, int faceId, float farPlane)
		{
			Matrix projectionMatrix = GetProjectionMatrix(farPlane);
			return CubeFaceViewMatrix(pos, faceId) * projectionMatrix;
		}

		private void CreateResources(MyRenderContext rc, int resolution)
		{
			DisposeResources();
			if (resolution == 0)
			{
				return;
			}
			int num = 0;
			for (int num2 = resolution; num2 != 1; num2 = num2 / 2 + num2 % 2)
			{
				num++;
			}
			Format format = Format.R11G11B10_Float;
			MyArrayTextureManager arrayTextures = MyManagers.ArrayTextures;
			CloseCubemapFinal = arrayTextures.CreateRtvCube("MyEnvironmentProbe.CloseCubemap", resolution, format, num);
			m_workCloseCubemapPrefiltered = arrayTextures.CreateRtvCube("MyEnvironmentProbe.WorkCloseCubemapPrefiltered", resolution, format, num);
			FarCubemapFinal = arrayTextures.CreateRtvCube("MyEnvironmentProbe.FarCubemapPrefiltered", resolution, format, num);
			m_workFarCubemapPrefiltered = arrayTextures.CreateRtvCube("MyEnvironmentProbe.WorkFarCubemapPrefiltered", resolution, format, num);
			CloseCubemapOriginal = arrayTextures.CreateRtvCube("MyEnvironmentProbe.CloseCubemapOriginal", resolution, format, num, generateMipMaps: true);
			FarCubemapOriginal = arrayTextures.CreateRtvCube("MyEnvironmentProbe.FarCubemapOriginal", resolution, format, num, generateMipMaps: true);
			m_workCubemapDepth = arrayTextures.CreateDepthCube("MyEnvironmentProbe.WorkCubemapDepth", resolution, Format.R24G8_Typeless, Format.R24_UNorm_X8_Typeless, Format.D24_UNorm_S8_UInt);
			for (int i = 0; i < m_workCubemapDepth.NumSlices; i++)
			{
				rc.ClearDsv(m_workCubemapDepth.SubresourceDsv(i), DepthStencilClearFlags.Depth, 0f, 0);
			}
			for (int j = 0; j < CloseCubemapFinal.NumSlices; j++)
			{
				for (int k = 0; k < CloseCubemapFinal.MipLevels; k++)
				{
					rc.ClearRtv(CloseCubemapFinal.SubresourceRtv(j, k), default(RawColor4));
					rc.ClearRtv(m_workCloseCubemapPrefiltered.SubresourceRtv(j, k), default(RawColor4));
					rc.ClearRtv(FarCubemapFinal.SubresourceRtv(j, k), default(RawColor4));
					rc.ClearRtv(m_workFarCubemapPrefiltered.SubresourceRtv(j, k), default(RawColor4));
					rc.ClearRtv(CloseCubemapOriginal.SubresourceRtv(j, k), default(RawColor4));
					rc.ClearRtv(FarCubemapOriginal.SubresourceRtv(j, k), default(RawColor4));
				}
			}
		}

		private void DisposeResources()
		{
			MyArrayTextureManager arrayTextures = MyManagers.ArrayTextures;
			arrayTextures.DisposeTex(ref CloseCubemapFinal);
			arrayTextures.DisposeTex(ref m_workCloseCubemapPrefiltered);
			arrayTextures.DisposeTex(ref m_workCloseCubemapPrefilteredAlt);
			arrayTextures.DisposeTex(ref FarCubemapFinal);
			arrayTextures.DisposeTex(ref m_workFarCubemapPrefiltered);
			arrayTextures.DisposeTex(ref m_workFarCubemapPrefilteredAlt);
			arrayTextures.DisposeTex(ref CloseCubemapOriginal);
			arrayTextures.DisposeTex(ref FarCubemapOriginal);
			arrayTextures.DisposeTex(ref m_workCubemapDepth);
		}

		void IManagerUnloadData.OnUnloadData()
		{
			m_lastUpdateTime = MyTimeSpan.Zero;
		}

		public void OnDeviceInit()
		{
		}

		public void OnDeviceReset()
		{
		}

		public void OnDeviceEnd()
		{
			DisposeResources();
		}
	}
}
