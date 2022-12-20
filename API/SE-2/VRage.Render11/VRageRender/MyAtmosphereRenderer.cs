using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Library.Utils;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender.Messages;

namespace VRageRender
{
	internal static class MyAtmosphereRenderer
	{
		internal static bool Enabled = true;

		private static MyComputeShaders.Id m_precomputeDensity;

		private static MyPixelShaders.Id m_ps;

		private static MyPixelShaders.Id m_psPerSample;

		private static MyPixelShaders.Id m_psEnvPerSample;

		private static MyPixelShaders.Id m_psEnv;

		private static MyVertexShaders.Id m_proxyVs;

		private static MyInputLayouts.Id m_proxyIL;

		private static IConstantBuffer m_cb;

		private static MeshId? m_sphereMesh;

		private static readonly Dictionary<uint, MyAtmosphere> m_atmospheres = new Dictionary<uint, MyAtmosphere>();

		internal static readonly Dictionary<uint, AtmosphereLuts> AtmosphereLUT = new Dictionary<uint, AtmosphereLuts>();

		private static readonly List<uint> m_removeTmp = new List<uint>();

		public static int Count => m_atmospheres.Count;

		internal static void Init(IMyUpdateBatch meshBatch)
		{
			ReloadShaders();
			m_sphereMesh = MyMeshes.GetMeshId(X.TEXT_("Models/Debug/Sphere.mwm"), 1f, meshBatch);
		}

		public static void OnSessionEnd()
		{
			while (m_atmospheres.Count > 0)
			{
<<<<<<< HEAD
				RemoveAtmosphere(m_atmospheres.Keys.First(), fadeOut: false);
=======
				RemoveAtmosphere(Enumerable.First<uint>((IEnumerable<uint>)m_atmospheres.Keys), fadeOut: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public static void OnDeviceEnd()
		{
			if (m_sphereMesh.HasValue)
			{
				MyMeshes.RemoveMesh(m_sphereMesh.Value);
				m_sphereMesh = null;
			}
		}

		private static void AllocateLuts(ref AtmosphereLuts luts)
		{
			if (luts.TransmittanceLut == null)
			{
				luts.TransmittanceLut = MyManagers.RwTextures.CreateUav("AtmosphereLuts.TransmittanceLut", 512, 128, Format.R32G32_Float);
			}
		}

		internal static void UpdateSettings(uint id, MyAtmosphereSettings settings)
		{
			if (settings.MieColorScattering.X == 0f)
			{
				settings.MieColorScattering = new Vector3(settings.MieScattering);
			}
			if (settings.Intensity == 0f)
			{
				settings.Intensity = 1f;
			}
			if (settings.AtmosphereTopModifier == 0f)
			{
				settings.AtmosphereTopModifier = 1f;
			}
			if (settings.SeaLevelModifier == 0f)
			{
				settings.SeaLevelModifier = 1f;
			}
			if (settings.RayleighHeightSpace == 0f)
			{
				settings.RayleighHeightSpace = settings.RayleighHeight;
			}
			if (settings.RayleighTransitionModifier == 0f)
			{
				settings.RayleighTransitionModifier = 1f;
			}
			if (m_atmospheres.TryGetValue(id, out var value))
			{
				value.Settings = settings;
				value.LutsPrecomputed = false;
				m_atmospheres[id] = value;
			}
		}

		private static void Precompute1(MyRenderContext rc, AtmosphereLuts luts)
		{
			rc.ComputeShader.SetUav(0, luts.TransmittanceLut);
			rc.ComputeShader.Set(m_precomputeDensity);
			rc.Dispatch(64, 16, 1);
			rc.ComputeShader.SetUav(0, null);
		}

		internal static void CreateAtmosphere(uint ID, MatrixD worldMatrix, float planetRadius, float atmosphereRadius, Vector3 rayleighScattering, float rayleighHeightScale, Vector3 mieScattering, float mieHeightScale, float planetScaleFactor, float atmosphereScaleFactor, bool fadeIn)
		{
			m_atmospheres.Add(ID, new MyAtmosphere
			{
				WorldMatrix = worldMatrix,
				PlanetRadius = planetRadius,
				AtmosphereRadius = atmosphereRadius,
				BetaRayleighScattering = rayleighScattering,
				BetaMieScattering = mieScattering,
				HeightScaleRayleighMie = new Vector2(rayleighHeightScale, mieHeightScale),
				PlanetScaleFactor = planetScaleFactor,
				AtmosphereScaleFactor = atmosphereScaleFactor,
				FadeInStart = (fadeIn ? MyCommon.FrameTime : MyTimeSpan.Zero),
				FadeOutStart = MyTimeSpan.Zero,
				LutsPrecomputed = false
			});
			AtmosphereLuts luts = default(AtmosphereLuts);
			AllocateLuts(ref luts);
			AtmosphereLUT[ID] = luts;
		}

		internal static void RecomputeAtmospheres()
		{
<<<<<<< HEAD
			foreach (KeyValuePair<uint, MyAtmosphere> item in m_atmospheres.ToList())
=======
			foreach (KeyValuePair<uint, MyAtmosphere> item in Enumerable.ToList<KeyValuePair<uint, MyAtmosphere>>((IEnumerable<KeyValuePair<uint, MyAtmosphere>>)m_atmospheres))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyAtmosphere value = item.Value;
				value.LutsPrecomputed = false;
				m_atmospheres[item.Key] = value;
			}
		}

		internal static void RemoveAtmosphere(uint ID, bool fadeOut)
		{
			if (fadeOut)
			{
				if (m_atmospheres.TryGetValue(ID, out var value))
				{
					value.FadeOutStart = MyCommon.FrameTime;
					m_atmospheres[ID] = value;
				}
				return;
			}
			m_atmospheres.Remove(ID);
			if (AtmosphereLUT.ContainsKey(ID))
			{
				IUavTexture texture = AtmosphereLUT[ID].TransmittanceLut;
				MyManagers.RwTextures.DisposeTex(ref texture);
			}
			AtmosphereLUT.Remove(ID);
		}

		internal static void Update()
		{
			foreach (KeyValuePair<uint, MyAtmosphere> atmosphere in m_atmospheres)
			{
				if (atmosphere.Value.FadeOutStart > MyTimeSpan.Zero && (MyCommon.FrameTime - atmosphere.Value.FadeOutStart).Seconds >= (double)MyCommon.LoddingSettings.Global.MaxTransitionInSeconds)
				{
					m_removeTmp.Add(atmosphere.Key);
				}
			}
			foreach (uint item in m_removeTmp)
			{
				RemoveAtmosphere(item, fadeOut: false);
			}
			m_removeTmp.Clear();
		}

		internal static void RenderGBuffer(MyRenderContext rc)
		{
			if (m_atmospheres.Count == 0 || !Enabled)
			{
				return;
			}
			Vector3D cameraPosition = MyRender11.Environment.Matrices.CameraPosition;
<<<<<<< HEAD
			IOrderedEnumerable<KeyValuePair<uint, MyAtmosphere>> orderedEnumerable = m_atmospheres.OrderByDescending((KeyValuePair<uint, MyAtmosphere> x) => (x.Value.WorldMatrix.Translation - cameraPosition).LengthSquared());
			rc.SetScreenViewport();
			foreach (KeyValuePair<uint, MyAtmosphere> item in orderedEnumerable)
=======
			IOrderedEnumerable<KeyValuePair<uint, MyAtmosphere>> obj = Enumerable.OrderByDescending<KeyValuePair<uint, MyAtmosphere>, double>((IEnumerable<KeyValuePair<uint, MyAtmosphere>>)m_atmospheres, (Func<KeyValuePair<uint, MyAtmosphere>, double>)((KeyValuePair<uint, MyAtmosphere> x) => (x.Value.WorldMatrix.Translation - cameraPosition).LengthSquared()));
			rc.SetScreenViewport();
			foreach (KeyValuePair<uint, MyAtmosphere> item in (IEnumerable<KeyValuePair<uint, MyAtmosphere>>)obj)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (MyRender11.DebugOverrides.Atmosphere)
				{
					rc.SetRtv(MyGBuffer.Main.DepthStencil.DsvRo, MyGBuffer.Main.LBuffer);
					rc.PixelShader.SetSrvs(0, MyGBuffer.Main);
					RenderBegin(rc);
					RenderOne(rc, cameraPosition, ref MyRender11.Environment.Matrices.ViewProjectionAt0, m_ps, m_psPerSample, item.Key, MyRender11.Environment.Planet.AtmosphereIntensityMultiplier, 0f);
					RenderEnd(rc);
				}
				if (MyRender11.DebugOverrides.Clouds)
				{
					MyCloudRenderer.Render(rc, item.Key);
				}
			}
		}

		internal static void RenderEnvProbe(MyRenderContext rc, Vector3D cameraPosition, ref Matrix viewProj, uint atmosphereId)
		{
			RenderBegin(rc);
			RenderOne(rc, cameraPosition, ref viewProj, m_psEnv, m_psEnvPerSample, atmosphereId, MyRender11.Environment.Planet.AtmosphereIntensityAmbientMultiplier, MyRender11.Environment.Planet.AtmosphereDesaturationFactorForward);
			RenderEnd(rc);
		}

		private static void RenderBegin(MyRenderContext rc)
		{
			MyMeshBuffers buffers = MyMeshes.GetLodMesh(m_sphereMesh.Value, 0).Buffers;
			rc.SetVertexBuffer(0, buffers.VB0);
			rc.SetIndexBuffer(buffers.IB);
			rc.VertexShader.Set(m_proxyVs);
			rc.SetInputLayout(m_proxyIL);
			rc.SetRasterizerState(null);
			rc.SetBlendState(MyBlendStateManager.BlendAtmosphere);
			rc.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstants);
			rc.AllShaderStages.SetConstantBuffer(1, m_cb);
			rc.PixelShader.SetSampler(15, MySamplerStateManager.Shadowmap);
		}

		private static void RenderEnd(MyRenderContext rc)
		{
			rc.PixelShader.SetSrvs(5, null, null);
			rc.SetRasterizerState(null);
			rc.SetDepthStencilState(null);
			rc.SetRtvNull();
		}

		private static void RenderOne(MyRenderContext rc, Vector3D cameraPosition, ref Matrix viewProj, MyPixelShaders.Id ps, MyPixelShaders.Id psPerSample, uint atmosphereId, float intensityMultiplier, float desaturationFactor)
		{
			if (!Enabled)
			{
				return;
			}
			MyAtmosphere myAtmosphere = m_atmospheres[atmosphereId];
			AtmosphereLuts luts = AtmosphereLUT[atmosphereId];
			FillAtmosphereConstants(cameraPosition, myAtmosphere, intensityMultiplier, desaturationFactor, out var constants);
			MatrixD m = myAtmosphere.WorldMatrix;
			m.Translation -= cameraPosition;
			Matrix matrix = (Matrix)m * viewProj;
			constants.WorldViewProj = Matrix.Transpose(matrix);
			MyMapping myMapping = MyMapping.MapDiscard(rc, m_cb);
			myMapping.WriteAndPosition(ref constants);
			myMapping.Unmap();
			if (!myAtmosphere.LutsPrecomputed)
			{
				Precompute1(rc, luts);
				myAtmosphere.LutsPrecomputed = true;
				m_atmospheres[atmosphereId] = myAtmosphere;
			}
			rc.PixelShader.SetSrvs(5, luts.TransmittanceLut);
			bool flag = m.Translation.Length() < (double)(myAtmosphere.AtmosphereRadius * myAtmosphere.PlanetScaleFactor);
			if (flag)
			{
				rc.SetRasterizerState(MyRasterizerStateManager.InvTriRasterizerState);
				if (MyRender11.MultisamplingEnabled)
				{
					rc.SetDepthStencilState(MyDepthStencilStateManager.TestEdgeStencil);
				}
				else
				{
					rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
				}
			}
			else
			{
				rc.SetRasterizerState(null);
				if (MyRender11.MultisamplingEnabled)
				{
					rc.SetDepthStencilState(MyDepthStencilStateManager.TestDepthAndEdgeStencil);
				}
				else
				{
					rc.SetDepthStencilState(MyDepthStencilStateManager.DepthTestReadOnly);
				}
			}
			rc.PixelShader.Set(ps);
			int indicesNum = MyMeshes.GetLodMesh(m_sphereMesh.Value, 0).Info.IndicesNum;
			rc.DrawIndexed(indicesNum, 0, 0);
			if (MyRender11.MultisamplingEnabled)
			{
				if (flag)
				{
					rc.SetDepthStencilState(MyDepthStencilStateManager.TestEdgeStencil, 128);
				}
				else
				{
					rc.SetDepthStencilState(MyDepthStencilStateManager.TestDepthAndEdgeStencil, 128);
				}
				rc.PixelShader.Set(psPerSample);
				rc.DrawIndexed(indicesNum, 0, 0);
			}
		}

		internal static bool IsValid(uint id)
		{
			return m_atmospheres.ContainsKey(id);
		}

		internal static uint? GetNearestAtmosphereId(Vector3D cameraPosition)
		{
			double num = double.MaxValue;
			uint? result = null;
			foreach (KeyValuePair<uint, MyAtmosphere> atmosphere in m_atmospheres)
			{
				double num2 = (cameraPosition - atmosphere.Value.WorldMatrix.Translation).LengthSquared();
				if (num2 < num)
				{
					result = atmosphere.Key;
					num = num2;
				}
			}
			return result;
		}

		private static void FillAtmosphereConstants(Vector3D cameraPosition, MyAtmosphere atmosphere, float intensityMultiplier, float desaturationFactor, out AtmosphereConstants constants)
		{
<<<<<<< HEAD
			Vector3 planetCentre = atmosphere.WorldMatrix.Translation - cameraPosition;
			double num = planetCentre.Length();
=======
			Vector3D vector3D = atmosphere.WorldMatrix.Translation - cameraPosition;
			double num = vector3D.Length();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			double num2 = atmosphere.AtmosphereRadius * atmosphere.Settings.AtmosphereTopModifier * atmosphere.PlanetScaleFactor * atmosphere.Settings.RayleighTransitionModifier;
			float amount = 0f;
			if (num > num2)
			{
				amount = ((!(num > num2 * 2.0)) ? ((float)((num - num2) / num2)) : 1f);
			}
			float x = MathHelper.Lerp(atmosphere.Settings.RayleighHeight, atmosphere.Settings.RayleighHeightSpace, amount);
			float num3 = Math.Min(1f, (float)(MyCommon.FrameTime - atmosphere.FadeInStart).Seconds / MyCommon.LoddingSettings.Global.MaxTransitionInSeconds);
			MyTimeSpan myTimeSpan = MyCommon.FrameTime - atmosphere.FadeOutStart;
			float val = ((atmosphere.FadeOutStart.Seconds > 0.0) ? (1f - (float)myTimeSpan.Seconds / MyCommon.LoddingSettings.Global.MaxTransitionInSeconds) : 1f);
			float num4 = num3 * Math.Max(0f, val);
			constants = new AtmosphereConstants
			{
<<<<<<< HEAD
				PlanetCentre = planetCentre,
=======
				PlanetCentre = vector3D,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				AtmosphereRadius = atmosphere.AtmosphereRadius * atmosphere.Settings.AtmosphereTopModifier,
				GroundRadius = atmosphere.PlanetRadius * 1.01f * atmosphere.Settings.SeaLevelModifier,
				BetaRayleighScattering = atmosphere.BetaRayleighScattering / atmosphere.Settings.RayleighScattering,
				BetaMieScattering = atmosphere.BetaMieScattering / atmosphere.Settings.MieColorScattering,
				HeightScaleRayleighMie = atmosphere.HeightScaleRayleighMie * new Vector2(x, atmosphere.Settings.MieHeight),
				MieG = atmosphere.Settings.MieG,
				PlanetScaleFactor = atmosphere.PlanetScaleFactor,
				AtmosphereScaleFactor = atmosphere.AtmosphereScaleFactor,
				Intensity = atmosphere.Settings.Intensity * intensityMultiplier * num4,
				FogIntensity = atmosphere.Settings.FogIntensity,
				DesaturationFactor = desaturationFactor
			};
		}

		internal unsafe static void ReloadShaders()
		{
			ShaderMacro[] macros = new ShaderMacro[1] { MyRender11.GetQualityMacro(MyRender11.Settings.User.AtmosphereShaderQuality) };
			ShaderMacro[] macros2 = new ShaderMacro[2]
			{
				MyRender11.GetQualityMacro(MyRender11.Settings.User.AtmosphereShaderQuality),
				MyRender11.ShaderSampleFrequencyDefine()[0]
			};
			m_ps = MyPixelShaders.Create("Transparent/Atmosphere/AtmosphereGBuffer.hlsl", macros);
			m_psPerSample = MyPixelShaders.Create("Transparent/Atmosphere/AtmosphereGBuffer.hlsl", macros2);
			m_psEnv = MyPixelShaders.Create("Transparent/Atmosphere/AtmosphereEnv.hlsl", macros);
			m_psEnvPerSample = MyPixelShaders.Create("Transparent/Atmosphere/AtmosphereEnv.hlsl", macros2);
			m_precomputeDensity = MyComputeShaders.Create("Transparent/Atmosphere/AtmospherePrecompute.hlsl");
			m_proxyVs = MyVertexShaders.Create("Transparent/Atmosphere/AtmosphereVS.hlsl");
			m_proxyIL = MyInputLayouts.Create(m_proxyVs.InfoId, MyVertexLayouts.GetLayout(default(MyVertexInputComponentType)));
			m_cb = MyManagers.Buffers.CreateConstantBuffer("CommonObjectCB" + sizeof(AtmosphereConstants), sizeof(AtmosphereConstants), null, ResourceUsage.Dynamic, isGlobal: true);
		}
	}
}
