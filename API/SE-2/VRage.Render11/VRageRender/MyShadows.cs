using System.Collections.Generic;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.LightingStage;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Scene.Components;
using VRageMath;

namespace VRageRender
{
	internal class MyShadows : IManager, IManagerDevice
	{
		internal struct ShadowMap
		{
			public IDepthTexture Texture;

			public MyLightComponent Light;
		}

		private readonly List<MyShadowmapQuery> m_shadowmapQueries = new List<MyShadowmapQuery>();

		private readonly List<ShadowMap> m_shadowmapsPool = new List<ShadowMap>();

		private readonly List<MyLightComponent> m_tempShadowLights = new List<MyLightComponent>();

		private readonly MyLightsCameraDistanceComparer m_lightSorter = new MyLightsCameraDistanceComparer();

		private int m_usedShadowMaps;

		internal MyShadowCascades ShadowCascades { get; private set; }

		internal ListReader<ShadowMap> Shadowmaps => m_shadowmapsPool;

		public int CascadeCount => ShadowCascades?.CascadeCount ?? 0;

		public int CascadeResolution => ShadowCascades?.CascadeResolution ?? 0;

		internal MyShadows()
		{
		}

		private void Init(MyRenderContext rc, int numberOfCascades, int cascadeResolution)
		{
			if (ShadowCascades == null)
			{
				ShadowCascades = new MyShadowCascades(rc, numberOfCascades, cascadeResolution);
			}
			else
			{
				ShadowCascades.Reset(rc, numberOfCascades, cascadeResolution);
			}
		}

		internal void Reset(MyRenderContext rc, int numberOfCascades, int cascadeResolution)
		{
			UnloadResources();
			Init(rc, numberOfCascades, cascadeResolution);
		}

		private void UnloadResources()
		{
			for (int i = 0; i < m_shadowmapsPool.Count; i++)
			{
				IDepthTexture texture = m_shadowmapsPool[i].Texture;
				MyManagers.RwTextures.DisposeTex(ref texture);
			}
			m_shadowmapsPool.Clear();
			if (ShadowCascades != null)
			{
				ShadowCascades.UnloadResources();
			}
		}

		internal ListReader<MyShadowmapQuery> PrepareQueries(MyRenderContext rc)
		{
			m_shadowmapQueries.Clear();
			if (MyRender11.DebugOverrides.SpotLights)
			{
				PrepareSpotlights();
			}
			ShadowCascades.PrepareQueries(rc, m_shadowmapQueries);
			for (int i = 0; i < m_shadowmapQueries.Count; i++)
			{
				rc.ClearDsv(m_shadowmapQueries[i].DepthBuffer, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1f, 0);
			}
			return m_shadowmapQueries;
		}

		internal IDepthTexture GetShadowMap(MyLightComponent light)
		{
			for (int i = 0; i < m_usedShadowMaps; i++)
			{
				if (m_shadowmapsPool[i].Light == light)
				{
					return m_shadowmapsPool[i].Texture;
				}
			}
			return null;
		}

		internal int SetVisibleLights(MyCullQuery cullQuery)
		{
			MyLightsRendering.PostprocessCulling(cullQuery);
			MyFlareRenderer.AddDistantFlares(cullQuery.Results.PointLights);
			m_tempShadowLights.Clear();
			MyList<MyLightComponent> spotLights = cullQuery.Results.SpotLights;
			bool flag = MyRender11.Settings.User.ShadowQuality.ReflectorShadowDistance() > 0f;
			foreach (MyLightComponent item in spotLights)
			{
				if (item.Data.Glare.Enabled && !item.Data.PointLightOn)
				{
					cullQuery.Results.PointLights.Add(item);
				}
				if (flag && item.Data.CastShadows && item.Data.SpotLightOn)
				{
					m_tempShadowLights.Add(item);
				}
			}
			m_tempShadowLights.Sort(m_lightSorter);
			return spotLights.Count;
		}

		private void PrepareSpotlights()
		{
			int num = MyRender11.Settings.User.ShadowGPUQuality.ShadowSpotResolution();
			int num2 = 0;
			MatrixD matrixD = MatrixD.CreateTranslation(MyRender11.Environment.Matrices.CameraPosition);
			foreach (MyLightComponent tempShadowLight in m_tempShadowLights)
			{
				tempShadowLight.LastSpotShadowIndex = int.MaxValue;
				if (tempShadowLight.Owner != null && !tempShadowLight.Owner.IsDestroyed && num2 < 4)
				{
					if (m_shadowmapsPool.Count <= num2)
					{
						m_shadowmapsPool.Add(new ShadowMap
						{
							Light = tempShadowLight,
							Texture = MyManagers.RwTextures.CreateDepth("ShadowmapsPool.Item", num, num, Format.R32_Typeless, Format.R32_Float, Format.D32_Float)
						});
					}
					ShadowMap value = m_shadowmapsPool[num2];
					value.Light = tempShadowLight;
					tempShadowLight.LastSpotShadowIndex = num2;
					m_shadowmapsPool[num2] = value;
					MatrixD spotlightViewProjection = tempShadowLight.SpotlightViewProjection;
					MatrixD matrixD2 = matrixD * spotlightViewProjection;
					MyShadowmapQuery myShadowmapQuery = default(MyShadowmapQuery);
					myShadowmapQuery.DepthBuffer = m_shadowmapsPool[num2].Texture;
					myShadowmapQuery.DepthBufferRo = m_shadowmapsPool[num2].Texture.DsvRo;
					myShadowmapQuery.Viewport = new MyViewport(num, num);
					myShadowmapQuery.ViewType = MyViewType.ShadowProjection;
					myShadowmapQuery.ViewIndex = num2;
					myShadowmapQuery.ProjectionInfo = new MyProjectionInfo
					{
						WorldCameraOffsetPosition = MyRender11.Environment.Matrices.CameraPosition,
						WorldToProjection = spotlightViewProjection,
						LocalToProjection = matrixD2,
						LocalToProjectionExtruded = matrixD2,
						Projection = tempShadowLight.SpotlightProjection,
						ViewOrigin = tempShadowLight.Owner.WorldMatrix.Translation
					};
					myShadowmapQuery.IgnoredEntities = tempShadowLight.GetEntitiesIgnoringShadow();
					MyShadowmapQuery item = myShadowmapQuery;
					m_shadowmapQueries.Add(item);
					num2++;
				}
			}
			m_usedShadowMaps = num2;
		}

		/// <inheritdoc />
		public void OnDeviceInit()
		{
		}

		/// <inheritdoc />
		public void OnDeviceReset()
		{
		}

		/// <inheritdoc />
		public void OnDeviceEnd()
		{
			UnloadResources();
		}
	}
}
