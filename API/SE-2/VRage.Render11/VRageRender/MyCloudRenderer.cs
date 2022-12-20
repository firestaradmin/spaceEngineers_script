using System;
using System.Collections.Generic;
using VRage.Library.Utils;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRageMath;
using VRageRender.Messages;

namespace VRageRender
{
	internal static class MyCloudRenderer
	{
		private class CloudLayerComparer : IComparer<KeyValuePair<uint, MyCloudLayer>>
		{
			public int Compare(KeyValuePair<uint, MyCloudLayer> x, KeyValuePair<uint, MyCloudLayer> y)
			{
				return GetValue(in y).CompareTo(GetValue(in x));
			}

			private static double GetValue(in KeyValuePair<uint, MyCloudLayer> layer)
			{
				KeyValuePair<uint, MyCloudLayer> keyValuePair = layer;
				MyCloudLayer value = keyValuePair.Value;
				Vector3D vector3D = value.Settings.CenterPoint - MyRender11.Environment.Matrices.CameraPosition;
				Vector3D vector3D2 = -Vector3D.Normalize(vector3D);
				return (vector3D + vector3D2 * value.Settings.Altitude).LengthSquared();
			}
		}

		private static MyVertexShaders.Id m_proxyVs;

		private static MyPixelShaders.Id m_cloudPs;

		private static MyInputLayouts.Id m_proxyIL;

		private static readonly Dictionary<uint, MyCloudLayer> m_cloudLayers = new Dictionary<uint, MyCloudLayer>();

		private static readonly Dictionary<uint, MyModifiableCloudLayerData> m_modifiableCloudLayerData = new Dictionary<uint, MyModifiableCloudLayerData>();

		private static KeyValuePair<uint, MyCloudLayer>[] m_cloudLayersSorted = Array.Empty<KeyValuePair<uint, MyCloudLayer>>();

		private static CloudLayerComparer m_comparer = new CloudLayerComparer();

		private static readonly List<uint> m_removeTmp = new List<uint>();

		internal static bool DrawFog = false;

		public static int Count => m_cloudLayers.Count;

		internal static void Init()
		{
			m_proxyVs = MyVertexShaders.Create("Transparent/Clouds/Clouds.hlsl");
			m_cloudPs = MyPixelShaders.Create("Transparent/Clouds/Clouds.hlsl");
			m_proxyIL = MyInputLayouts.Create(m_proxyVs.InfoId, MyVertexLayouts.GetLayout(new MyVertexInputComponent(MyVertexInputComponentType.POSITION_PACKED, 0), new MyVertexInputComponent(MyVertexInputComponentType.NORMAL, 1), new MyVertexInputComponent(MyVertexInputComponentType.TANGENT_SIGN_OF_BITANGENT, 1), new MyVertexInputComponent(MyVertexInputComponentType.TEXCOORD0_H, 1)));
		}

		internal static void CreateCloudLayer(ref MyCloudLayerSettingsRender settings)
		{
			MyTextureStreamingManager textures = MyManagers.Textures;
			MeshId meshId = MyMeshes.GetMeshId(X.TEXT_(settings.Model), 1f, MyRender11.DeferStateChangeBatch);
			MyStreamedTexturePin permanentTexture;
			MyStreamedTexturePin permanentTexture2;
			if (settings.Textures != null && settings.Textures.Count > 0)
			{
				string name = settings.Textures[0].Insert(settings.Textures[0].LastIndexOf('.'), "_alphamask");
				permanentTexture = textures.GetPermanentTexture(name, MyFileTextureEnum.ALPHAMASK);
				string name2 = settings.Textures[0].Insert(settings.Textures[0].LastIndexOf('.'), "_cm");
				permanentTexture2 = textures.GetPermanentTexture(name2, MyFileTextureEnum.COLOR_METAL);
			}
			else
			{
				permanentTexture = textures.GetPermanentTexture(MyMeshes.GetMeshPart(meshId, 0, 0).Info.Material.Info.Alphamask_Texture, MyFileTextureEnum.ALPHAMASK);
				permanentTexture2 = textures.GetPermanentTexture(MyMeshes.GetMeshPart(meshId, 0, 0).Info.Material.Info.ColorMetal_Texture, MyFileTextureEnum.COLOR_METAL);
			}
			m_cloudLayers.Add(settings.ID, new MyCloudLayer
			{
				Mesh = meshId,
				AlphaTextureHandle = permanentTexture,
				ColorTextureHandle = permanentTexture2,
				AlphaTexture = permanentTexture.Texture,
				ColorTexture = permanentTexture2.Texture,
				Settings = settings,
				FadeInStart = (settings.FadeIn ? MyCommon.FrameTime : MyTimeSpan.Zero)
			});
			m_modifiableCloudLayerData.Add(settings.ID, new MyModifiableCloudLayerData
			{
				RadiansAroundAxis = settings.InitialRotation,
				LastGameplayFrameUpdate = MyRender11.GameplayFrameCounter
			});
		}

		internal static void RemoveCloud(uint ID, bool fadeOut)
		{
			if (fadeOut)
			{
				if (m_cloudLayers.TryGetValue(ID, out var value))
				{
					value.FadeOutStart = MyCommon.FrameTime;
					m_cloudLayers[ID] = value;
				}
			}
			else
			{
				MyCloudLayer myCloudLayer = m_cloudLayers[ID];
				myCloudLayer.ColorTextureHandle.Dispose();
				myCloudLayer.AlphaTextureHandle.Dispose();
				m_cloudLayers.Remove(ID);
				m_modifiableCloudLayerData.Remove(ID);
			}
		}

		internal static void Update()
		{
			foreach (KeyValuePair<uint, MyCloudLayer> cloudLayer in m_cloudLayers)
			{
				if (cloudLayer.Value.FadeOutStart > MyTimeSpan.Zero && (MyCommon.FrameTime - cloudLayer.Value.FadeOutStart).Seconds >= (double)MyCommon.LoddingSettings.Global.MaxTransitionInSeconds)
				{
					m_removeTmp.Add(cloudLayer.Key);
				}
			}
			foreach (uint item in m_removeTmp)
			{
				RemoveCloud(item, fadeOut: false);
			}
			m_removeTmp.Clear();
		}

		internal unsafe static void Render(MyRenderContext rc, uint atmosphereID)
		{
			if (m_cloudLayers.Count == 0)
			{
				return;
			}
			rc.SetScreenViewport();
			rc.VertexShader.Set(m_proxyVs);
			rc.PixelShader.Set(m_cloudPs);
			rc.SetInputLayout(m_proxyIL);
			rc.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
			rc.SetBlendState(MyBlendStateManager.BlendTransparent);
			rc.SetDepthStencilState(MyDepthStencilStateManager.DepthTestReadOnly);
			IConstantBuffer objectCB = rc.GetObjectCB(sizeof(CloudsConstants));
			rc.AllShaderStages.SetConstantBuffer(1, objectCB);
			rc.PixelShader.SetSamplers(0, MySamplerStateManager.CloudSampler);
			rc.SetRtv(MyGBuffer.Main.DepthStencil.DsvRo, MyGBuffer.Main.LBuffer);
			int count = m_cloudLayers.Count;
			if (m_cloudLayersSorted.Length < count)
			{
				Array.Resize(ref m_cloudLayersSorted, Math.Max(count, Math.Max(m_cloudLayersSorted.Length * 2, 4)));
			}
			((ICollection<KeyValuePair<uint, MyCloudLayer>>)m_cloudLayers).CopyTo(m_cloudLayersSorted, 0);
			Array.Sort(m_cloudLayersSorted, 0, count, m_comparer);
			for (int i = 0; i < m_cloudLayersSorted.Length; i++)
			{
				ref KeyValuePair<uint, MyCloudLayer> reference = ref m_cloudLayersSorted[i];
				if (reference.Value.Settings.AtmosphereID != atmosphereID || !m_modifiableCloudLayerData.TryGetValue(reference.Key, out var value))
				{
					continue;
				}
				float num = Math.Min(1f, (float)(MyCommon.FrameTime - reference.Value.FadeInStart).Seconds / MyCommon.LoddingSettings.Global.MaxTransitionInSeconds);
				MyTimeSpan myTimeSpan = MyCommon.FrameTime - reference.Value.FadeOutStart;
				float val = ((reference.Value.FadeOutStart.Seconds > 0.0) ? (1f - (float)myTimeSpan.Seconds / MyCommon.LoddingSettings.Global.MaxTransitionInSeconds) : 1f);
				float num2 = num * Math.Max(0f, val);
				Vector3D centerPoint = reference.Value.Settings.CenterPoint;
				Vector3D cameraPosition = MyRender11.Environment.Matrices.CameraPosition;
				double num3 = (centerPoint - cameraPosition).Length();
				float w = 1f;
				double num4 = (num3 - reference.Value.Settings.MinScaledAltitude) / (reference.Value.Settings.MaxPlanetHillRadius - reference.Value.Settings.MinScaledAltitude);
				if (reference.Value.Settings.FadeOutRelativeAltitudeStart > reference.Value.Settings.FadeOutRelativeAltitudeEnd)
				{
					w = (float)MathHelper.Clamp(1.0 - (reference.Value.Settings.FadeOutRelativeAltitudeStart - num4) / (reference.Value.Settings.FadeOutRelativeAltitudeStart - reference.Value.Settings.FadeOutRelativeAltitudeEnd), 0.0, 1.0);
				}
				else if (reference.Value.Settings.FadeOutRelativeAltitudeStart < reference.Value.Settings.FadeOutRelativeAltitudeEnd)
				{
					w = (float)MathHelper.Clamp(1.0 - (num4 - reference.Value.Settings.FadeOutRelativeAltitudeStart) / (reference.Value.Settings.FadeOutRelativeAltitudeEnd - reference.Value.Settings.FadeOutRelativeAltitudeStart), 0.0, 1.0);
				}
				Vector4 color = reference.Value.Settings.Color * num2 * new Vector4(MyRender11.Environment.Planet.CloudsIntensityMultiplier, MyRender11.Environment.Planet.CloudsIntensityMultiplier, MyRender11.Environment.Planet.CloudsIntensityMultiplier, w);
				if (color.W == 0f)
				{
					continue;
				}
				int gameplayFrameCounter = MyRender11.GameplayFrameCounter;
				float num5 = reference.Value.Settings.AngularVelocity * (float)(gameplayFrameCounter - value.LastGameplayFrameUpdate) / 10f;
				value.RadiansAroundAxis += num5;
				if (value.RadiansAroundAxis >= Math.PI * 2.0)
				{
					value.RadiansAroundAxis -= Math.PI * 2.0;
				}
				value.LastGameplayFrameUpdate = gameplayFrameCounter;
				double num6 = reference.Value.Settings.Altitude;
				if (reference.Value.Settings.ScalingEnabled)
				{
					double num7 = reference.Value.Settings.Altitude * 0.95;
					if (num3 > num7)
					{
						num6 = MathHelper.Clamp(num6 * (1.0 - MathHelper.Clamp((num3 - num7) / (num7 * 1.5), 0.0, 1.0)), reference.Value.Settings.MinScaledAltitude, reference.Value.Settings.Altitude);
					}
				}
				MatrixD matrix = MatrixD.CreateScale(num6) * MatrixD.CreateFromAxisAngle(reference.Value.Settings.RotationAxis, (float)value.RadiansAroundAxis);
				matrix.Translation = reference.Value.Settings.CenterPoint;
				matrix.Translation -= MyRender11.Environment.Matrices.CameraPosition;
				CloudsConstants cloudsConstants = default(CloudsConstants);
				MatrixD m = MatrixD.Transpose(matrix);
				cloudsConstants.World = m;
				MatrixD m2 = MatrixD.Transpose(MyRender11.Environment.Matrices.ViewProjectionAt0);
				cloudsConstants.ViewProj = m2;
				cloudsConstants.Color = color;
				CloudsConstants data = cloudsConstants;
				MyMapping myMapping = MyMapping.MapDiscard(rc, objectCB);
				myMapping.WriteAndPosition(ref data);
				myMapping.Unmap();
				LodMeshId lodMesh = MyMeshes.GetLodMesh(reference.Value.Mesh, 0);
				MyMeshBuffers buffers = lodMesh.Buffers;
				rc.SetVertexBuffer(0, buffers.VB0);
				rc.SetVertexBuffer(1, buffers.VB1);
				rc.SetIndexBuffer(buffers.IB);
				rc.PixelShader.SetSrv(0, reference.Value.ColorTexture);
				rc.PixelShader.SetSrv(1, reference.Value.AlphaTexture);
				rc.DrawIndexed(lodMesh.Info.IndicesNum, 0, 0);
			}
			rc.PixelShader.SetSrv(0, null);
			rc.PixelShader.SetSrv(1, null);
			rc.PixelShader.SetSrv(2, null);
			rc.SetDepthStencilState(null);
			rc.SetBlendState(null);
			rc.SetRasterizerState(null);
			rc.SetRtvNull();
		}
	}
}
