using System;
using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage.Collections;
using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRage.Render11.Common;
using VRage.Render11.Culling;
using VRage.Render11.Resources;
using VRage.Render11.Scene;
using VRage.Render11.Scene.Components;
using VRage.Render11.Sprites;
using VRageMath;
using VRageRender.Vertex;

namespace VRageRender
{
	internal class MyDebugRenderer : MyImmediateRC
	{
		private static MyPixelShaders.Id m_baseColorShader;

		private static MyPixelShaders.Id m_albedoShader;

		private static MyPixelShaders.Id m_normalShader;

		private static MyPixelShaders.Id m_normalViewShader;

		private static MyPixelShaders.Id m_glossinessShader;

		private static MyPixelShaders.Id m_metalnessShader;

		private static MyPixelShaders.Id m_aoShader;

		private static MyPixelShaders.Id m_emissiveShader;

		private static MyPixelShaders.Id m_ambientDiffuseShader;

		private static MyPixelShaders.Id m_ambientSpecularShader;

		private static MyPixelShaders.Id m_edgeDebugShader;

		private static MyPixelShaders.Id m_shadowsDebugShader;

		private static MyPixelShaders.Id m_shadowSplitsDebugShader;

		private static MyPixelShaders.Id m_NDotLShader;

		private static MyPixelShaders.Id m_lodShader;

		private static MyPixelShaders.Id m_depthShader;

		private static MyComputeShaders.Id m_depthReprojectionShader;

		private static MyPixelShaders.Id m_stencilShader;

		private static MyPixelShaders.Id m_rtShader;

		private static MyVertexShaders.Id m_screenVertexShader;

		private static MyPixelShaders.Id m_blitTextureShader;

		private static MyPixelShaders.Id m_blitTextureDepthShader;

		private static MyPixelShaders.Id m_displayHdrIntensity;

		private static MyPixelShaders.Id m_blitTexture3DShader;

		private static MyPixelShaders.Id m_blitTextureArrayShader;

		private static MyInputLayouts.Id m_inputLayout;

		private static IVertexBuffer m_quadBuffer;

		private static readonly Vector4[] m_lodColors = new Vector4[16]
		{
			new Vector4(1f, 0f, 0f, 1f),
			new Vector4(0f, 1f, 0f, 1f),
			new Vector4(0f, 0f, 1f, 1f),
			new Vector4(1f, 1f, 0f, 1f),
			new Vector4(0f, 1f, 1f, 1f),
			new Vector4(1f, 0f, 1f, 1f),
			new Vector4(0.5f, 0f, 1f, 1f),
			new Vector4(0.5f, 1f, 0f, 1f),
			new Vector4(1f, 0f, 0.5f, 1f),
			new Vector4(0f, 1f, 0.5f, 1f),
			new Vector4(1f, 0.5f, 0f, 1f),
			new Vector4(0f, 0.5f, 1f, 1f),
			new Vector4(0.5f, 1f, 1f, 1f),
			new Vector4(1f, 0.5f, 1f, 1f),
			new Vector4(1f, 1f, 0.5f, 1f),
			new Vector4(0.5f, 0.5f, 1f, 1f)
		};

		internal static MyPixelShaders.Id BlitTextureShader => m_blitTextureShader;

		internal static void Init()
		{
			m_screenVertexShader = MyVertexShaders.Create("Debug/DebugBaseColor.hlsl");
			m_baseColorShader = MyPixelShaders.Create("Debug/DebugBaseColor.hlsl");
			m_albedoShader = MyPixelShaders.Create("Debug/DebugAlbedo.hlsl");
			m_normalShader = MyPixelShaders.Create("Debug/DebugNormal.hlsl");
			m_normalViewShader = MyPixelShaders.Create("Debug/DebugNormalView.hlsl");
			m_glossinessShader = MyPixelShaders.Create("Debug/DebugGlossiness.hlsl");
			m_metalnessShader = MyPixelShaders.Create("Debug/DebugMetalness.hlsl");
			m_aoShader = MyPixelShaders.Create("Debug/DebugAmbientOcclusion.hlsl");
			m_emissiveShader = MyPixelShaders.Create("Debug/DebugEmissive.hlsl");
			m_ambientDiffuseShader = MyPixelShaders.Create("Debug/DebugAmbientDiffuse.hlsl");
			m_ambientSpecularShader = MyPixelShaders.Create("Debug/DebugAmbientSpecular.hlsl");
			m_edgeDebugShader = MyPixelShaders.Create("Debug/DebugEdge.hlsl");
			m_shadowsDebugShader = MyPixelShaders.Create("Debug/DebugShadows.hlsl");
			m_shadowSplitsDebugShader = MyPixelShaders.Create("Debug/DebugShadowSplits.hlsl");
			m_NDotLShader = MyPixelShaders.Create("Debug/DebugNDotL.hlsl");
			m_lodShader = MyPixelShaders.Create("Debug/DebugLOD.hlsl");
			m_depthShader = MyPixelShaders.Create("Debug/DebugDepth.hlsl");
			m_depthReprojectionShader = MyComputeShaders.Create("Debug/DebugDepthReprojection.hlsl");
			m_stencilShader = MyPixelShaders.Create("Debug/DebugStencil.hlsl");
			m_rtShader = MyPixelShaders.Create("Debug/DebugRt.hlsl");
			m_displayHdrIntensity = MyPixelShaders.Create("Debug/DisplayHdrIntensity.hlsl", new ShaderMacro[1]
			{
				new ShaderMacro("BLIT", null)
			});
			m_blitTextureShader = MyPixelShaders.Create("Debug/DebugBlitTexture.hlsl");
			m_blitTexture3DShader = MyPixelShaders.Create("Debug/DebugBlitTexture3D.hlsl");
			m_blitTextureArrayShader = MyPixelShaders.Create("Debug/DebugBlitTextureArray.hlsl");
			m_blitTextureDepthShader = MyPixelShaders.Create("Debug/DebugBlitTextureDepth.hlsl");
			m_inputLayout = MyInputLayouts.Create(m_screenVertexShader.InfoId, MyVertexLayouts.GetLayout(MyVertexInputComponentType.POSITION2, MyVertexInputComponentType.TEXCOORD0));
			m_quadBuffer = MyManagers.Buffers.CreateVertexBuffer("MyDebugRenderer quad", 6, MyVertexFormatPosition2Texcoord.STRIDE, null, ResourceUsage.Dynamic, isStreamOutput: false, isGlobal: true);
		}

		internal static void DrawQuad(float x, float y, float w, float h, MyVertexShaders.Id? customVertexShader = null)
		{
			MyVertexShaders.Id id = (customVertexShader.HasValue ? customVertexShader.Value : m_screenVertexShader);
			MyImmediateRC.RC.VertexShader.Set(id);
			MyImmediateRC.RC.SetInputLayout(m_inputLayout);
			MyMapping myMapping = MyMapping.MapDiscard(m_quadBuffer);
			MyVertexFormatPosition2Texcoord data = new MyVertexFormatPosition2Texcoord(new Vector2(x, y), new Vector2(0f, 0f));
			myMapping.WriteAndPosition(ref data);
			data = new MyVertexFormatPosition2Texcoord(new Vector2(x + w, y + h), new Vector2(1f, 1f));
			myMapping.WriteAndPosition(ref data);
			data = new MyVertexFormatPosition2Texcoord(new Vector2(x, y + h), new Vector2(0f, 1f));
			myMapping.WriteAndPosition(ref data);
			data = new MyVertexFormatPosition2Texcoord(new Vector2(x, y), new Vector2(0f, 0f));
			myMapping.WriteAndPosition(ref data);
			data = new MyVertexFormatPosition2Texcoord(new Vector2(x + w, y), new Vector2(1f, 0f));
			myMapping.WriteAndPosition(ref data);
			data = new MyVertexFormatPosition2Texcoord(new Vector2(x + w, y + h), new Vector2(1f, 1f));
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			MyImmediateRC.RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			MyImmediateRC.RC.SetVertexBuffer(0, m_quadBuffer);
			MyImmediateRC.RC.Draw(6, 0);
		}

		internal static void DrawAtmosphereTransmittance(uint ID)
		{
			IUavTexture transmittanceLut = MyAtmosphereRenderer.AtmosphereLUT[ID].TransmittanceLut;
			MyImmediateRC.RC.PixelShader.Set(m_blitTextureShader);
			MyImmediateRC.RC.PixelShader.SetSrv(0, transmittanceLut);
			DrawQuad(256f, 0f, 256f, 64f);
			MyImmediateRC.RC.PixelShader.SetSrv(0, null);
		}

		internal static void DrawSrvDepthTexture(float x, float y, float w, float h, ISrvBindable tex)
		{
			MyImmediateRC.RC.PixelShader.Set(m_blitTextureDepthShader);
			MyImmediateRC.RC.PixelShader.SetSrv(0, tex);
			DrawQuad(x, y, w, h);
		}

		internal static void DrawSrvTexture(float x, float y, float w, float h, ISrvBindable tex, bool intensities)
		{
			if (intensities)
			{
				MyImmediateRC.RC.PixelShader.Set(m_displayHdrIntensity);
				MyImmediateRC.RC.PixelShader.SetSrv(5, tex);
				DrawQuad(x, y, w, h);
			}
			else
			{
				MyImmediateRC.RC.PixelShader.Set(m_blitTextureShader);
				MyImmediateRC.RC.PixelShader.SetSrv(0, tex);
				DrawQuad(x, y, w, h);
			}
		}

		internal static void DrawShadowmapTextures(int quadStartX, int quadStartY, int quadSize)
		{
			uint num = 0u;
<<<<<<< HEAD
			using (List<MyShadows.ShadowMap>.Enumerator enumerator = MyManagers.Shadows.Shadowmaps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DrawSrvDepthTexture(tex: enumerator.Current.Texture, x: quadStartX + (quadSize + quadStartX / 2) * num, y: quadStartY, w: quadSize, h: quadSize * MyRender11.ViewportResolution.Y / MyRender11.ViewportResolution.X);
					num++;
				}
=======
			using List<MyShadows.ShadowMap>.Enumerator enumerator = MyManagers.Shadows.Shadowmaps.GetEnumerator();
			while (enumerator.MoveNext())
			{
				DrawSrvDepthTexture(tex: enumerator.Current.Texture, x: quadStartX + (quadSize + quadStartX / 2) * num, y: quadStartY, w: quadSize, h: quadSize * MyRender11.ViewportResolution.Y / MyRender11.ViewportResolution.X);
				num++;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		internal static void DrawArrayTextures(IDepthArrayTexture tex, int quadStartX, int quadStartY, int quadSize)
		{
			for (uint num = 0u; num < tex.NumSlices; num++)
			{
				DrawArrayTextureIndex(tex, num, quadStartX + (quadSize + quadStartX / 2) * num, quadStartY, quadSize, quadSize * MyRender11.ViewportResolution.Y / MyRender11.ViewportResolution.X);
			}
		}

		private static void DrawArrayTextureIndex(ISrvBindable textureArray, uint index, float x, float y, float width, float height)
		{
			MyImmediateRC.RC.PixelShader.Set(m_blitTextureArrayShader);
			MyImmediateRC.RC.PixelShader.SetSrv(0, textureArray);
			IConstantBuffer materialCB = MyImmediateRC.RC.GetMaterialCB(4);
			MyImmediateRC.RC.PixelShader.SetConstantBuffer(5, materialCB);
			float data = index;
			MyMapping myMapping = MyMapping.MapDiscard(materialCB);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
			DrawQuad(x, y, width, height);
			MyImmediateRC.RC.PixelShader.SetSrv(0, null);
		}

		internal static void DrawParticles(ISrvBindable particleRenderTarget, int quadStartX, int quadStartY, int quadSize)
		{
			MyImmediateRC.RC.PixelShader.Set(m_blitTextureShader);
			MyImmediateRC.RC.AllShaderStages.SetSrv(0, particleRenderTarget);
			MyImmediateRC.RC.SetBlendState(MyBlendStateManager.BlendAlphaPremult);
			DrawQuad(quadStartX, quadStartY, quadSize, quadSize * MyRender11.ViewportResolution.Y / MyRender11.ViewportResolution.X);
			MyImmediateRC.RC.SetBlendState(null);
			MyImmediateRC.RC.PixelShader.SetSrv(0, null);
		}

		internal static void DrawEnvProbe(IRtvArrayTexture colorTexture, int colorMipLevel, bool intensities)
		{
			if (colorMipLevel >= colorTexture.MipLevels)
			{
				colorMipLevel = colorTexture.MipLevels - 1;
			}
			DrawSrvTexture(512f, 256f, 256f, 256f, colorTexture.SubresourceSrv(0, colorMipLevel), intensities);
			DrawSrvTexture(0f, 256f, 256f, 256f, colorTexture.SubresourceSrv(1, colorMipLevel), intensities);
			DrawSrvTexture(256f, 0f, 256f, 256f, colorTexture.SubresourceSrv(2, colorMipLevel), intensities);
			DrawSrvTexture(256f, 512f, 256f, 256f, colorTexture.SubresourceSrv(3, colorMipLevel), intensities);
			DrawSrvTexture(256f, 256f, 256f, 256f, colorTexture.SubresourceSrv(4, colorMipLevel), intensities);
			DrawSrvTexture(768f, 256f, 256f, 256f, colorTexture.SubresourceSrv(5, colorMipLevel), intensities);
		}

		internal static void Draw(IRtvBindable renderTarget, IRtvTexture ambientOcclusion)
		{
			MyImmediateRC.RC.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			MyImmediateRC.RC.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			MyImmediateRC.RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			MyImmediateRC.RC.SetViewport(0f, 0f, MyRender11.ViewportResolution.X, MyRender11.ViewportResolution.Y);
			MyImmediateRC.RC.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			MyImmediateRC.RC.SetRtv(renderTarget);
			MyImmediateRC.RC.PixelShader.SetSrvs(0, MyGBuffer.Main);
			MyImmediateRC.RC.SetBlendState(null);
			if (MyRender11.Settings.DisplayGbufferColor)
			{
				MyImmediateRC.RC.PixelShader.Set(m_baseColorShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayGbufferAlbedo)
			{
				MyImmediateRC.RC.PixelShader.Set(m_albedoShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayGbufferNormal)
			{
				MyImmediateRC.RC.PixelShader.Set(m_normalShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayGbufferNormalView)
			{
				MyImmediateRC.RC.PixelShader.Set(m_normalViewShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayGbufferGlossiness)
			{
				MyImmediateRC.RC.PixelShader.Set(m_glossinessShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayGbufferMetalness)
			{
				MyImmediateRC.RC.PixelShader.Set(m_metalnessShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayGbufferAO)
			{
				MyImmediateRC.RC.PixelShader.Set(m_aoShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayEmissive)
			{
				MyImmediateRC.RC.PixelShader.Set(m_emissiveShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayAmbientDiffuse)
			{
				MyImmediateRC.RC.PixelShader.Set(m_ambientDiffuseShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayAmbientSpecular)
			{
				MyImmediateRC.RC.PixelShader.Set(m_ambientSpecularShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayEdgeMask)
			{
				MyImmediateRC.RC.PixelShader.Set(m_edgeDebugShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayShadowsWithDebug || MyRender11.Settings.DisplayShadowSplitsWithDebug)
			{
				IBorrowedUavTexture borrowedUavTexture = MyManagers.Shadows.ShadowCascades.PostProcess(MyImmediateRC.RC);
				MyImmediateRC.RC.PixelShader.SetConstantBuffer(4, MyManagers.Shadows.ShadowCascades.CascadeConstantBuffer);
				MyImmediateRC.RC.PixelShader.SetSrv(19, borrowedUavTexture);
				MyImmediateRC.RC.SetRtv(renderTarget);
				MyImmediateRC.RC.PixelShader.SetSrvs(0, MyGBuffer.Main);
				MyImmediateRC.RC.PixelShader.Set(MyRender11.Settings.DisplayShadowsWithDebug ? m_shadowsDebugShader : m_shadowSplitsDebugShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
				MyImmediateRC.RC.PixelShader.SetSrv(19, null);
				borrowedUavTexture.Release();
			}
			else if (MyRender11.Settings.DisplayNDotL)
			{
				MyImmediateRC.RC.PixelShader.Set(m_NDotLShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayGbufferLOD)
			{
				MyImmediateRC.RC.PixelShader.Set(m_lodShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayMipmap)
			{
				MyImmediateRC.RC.PixelShader.Set(m_baseColorShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayDepth)
			{
				MyImmediateRC.RC.PixelShader.Set(m_depthShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayReprojectedDepth)
			{
				IBorrowedUavTexture borrowedUavTexture2 = MyManagers.RwTexturesPool.BorrowUav("DebugRender.DepthReprojection", Format.R32_Float);
				MyRender11.RC.ClearUav(borrowedUavTexture2, default(RawInt4));
				MyImmediateRC.RC.ComputeShader.SetConstantBuffer(0, MyCommon.FrameConstants);
				MyImmediateRC.RC.ComputeShader.SetSrv(0, MyGBuffer.Main.DepthStencil.SrvDepth);
				MyImmediateRC.RC.ComputeShader.SetUav(0, borrowedUavTexture2);
				MyImmediateRC.RC.ComputeShader.Set(m_depthReprojectionShader);
				int threadGroupCountX = Align(MyRender11.ResolutionI.X, 32) / 32;
				int threadGroupCountY = Align(MyRender11.ResolutionI.Y, 32) / 32;
				MyImmediateRC.RC.Dispatch(threadGroupCountX, threadGroupCountY, 1);
				MyImmediateRC.RC.ComputeShader.SetSrv(0, null);
				MyImmediateRC.RC.ComputeShader.SetUav(0, null);
				MyImmediateRC.RC.PixelShader.SetSrv(0, borrowedUavTexture2);
				MyImmediateRC.RC.PixelShader.Set(m_depthShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
				MyImmediateRC.RC.PixelShader.SetSrv(0, MyGBuffer.Main.DepthStencil.SrvDepth);
			}
			else if (MyRender11.Settings.DisplayStencil)
			{
				MyImmediateRC.RC.PixelShader.Set(m_stencilShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayAO)
			{
				MyImmediateRC.RC.PixelShader.SetSrv(0, ambientOcclusion);
				MyImmediateRC.RC.PixelShader.SetSampler(0, MySamplerStateManager.Linear);
				MyImmediateRC.RC.PixelShader.Set(m_rtShader);
				MyScreenPass.DrawFullscreenQuad(MyImmediateRC.RC);
			}
			else if (MyRender11.Settings.DisplayEnvProbe && MyManagers.EnvironmentProbe.CloseCubemapFinal != null)
			{
				DrawEnvProbe(MyManagers.EnvironmentProbe.CloseCubemapFinal, MyRender11.Settings.DisplayEnvProbeMipLevel, MyRender11.Settings.DisplayEnvProbeIntensities);
			}
			else if (MyRender11.Settings.DisplayEnvProbeFar && MyManagers.EnvironmentProbe.FarCubemapFinal != null)
			{
				DrawEnvProbe(MyManagers.EnvironmentProbe.FarCubemapFinal, MyRender11.Settings.DisplayEnvProbeMipLevel, MyRender11.Settings.DisplayEnvProbeIntensities);
			}
			else if (MyRender11.Settings.DisplayEnvProbeOriginal && MyManagers.EnvironmentProbe.CloseCubemapOriginal != null)
			{
				DrawEnvProbe(MyManagers.EnvironmentProbe.CloseCubemapOriginal, MyRender11.Settings.DisplayEnvProbeMipLevel, MyRender11.Settings.DisplayEnvProbeIntensities);
			}
			else if (MyRender11.Settings.DisplayEnvProbeFarOriginal && MyManagers.EnvironmentProbe.FarCubemapOriginal != null)
			{
				DrawEnvProbe(MyManagers.EnvironmentProbe.FarCubemapOriginal, MyRender11.Settings.DisplayEnvProbeMipLevel, MyRender11.Settings.DisplayEnvProbeIntensities);
			}
			if (MyRender11.Settings.DisplayNormals)
			{
				MyHBAO.DrawNormalsTexture(MyImmediateRC.RC);
			}
			if (MyRender11.Settings.DrawSpotShadowTextures)
			{
				if (MyRender11.Settings.ZoomCascadeTextureIndex >= 0 && MyManagers.Shadows.Shadowmaps.Count > 0)
				{
					int index = Math.Min(MyRender11.Settings.ZoomCascadeTextureIndex, MyManagers.Shadows.Shadowmaps.Count);
					DrawSrvDepthTexture(0f, 0f, MyRender11.ViewportResolution.X, MyRender11.ViewportResolution.Y, MyManagers.Shadows.Shadowmaps[index].Texture);
				}
				DrawShadowmapTextures(100, 100, 200);
			}
			if (MyRender11.Settings.DrawCascadeShadowTextures)
			{
				if (MyRender11.Settings.ZoomCascadeTextureIndex >= 0)
				{
					DrawArrayTextureIndex(MyManagers.Shadows.ShadowCascades.CascadeShadowmapArray, (uint)MyRender11.Settings.ZoomCascadeTextureIndex, 0f, 0f, MyRender11.ViewportResolution.X, MyRender11.ViewportResolution.Y);
				}
				DrawArrayTextures(MyManagers.Shadows.ShadowCascades.CascadeShadowmapArray, 100, 100, 200);
			}
			if (MyRender11.Settings.DisplayIDs || MyRender11.Settings.DisplayAabbs || MyRender11.Settings.DisplayTreeAabbs)
			{
				DrawHierarchyDebug();
			}
			if (!MyRender11.Settings.DebugRenderClipmapCells)
			{
				return;
			}
			MyLinesBatch myLinesBatch = MyLinesRenderer.CreateBatch();
			foreach (MyVoxelCellComponent item in MyComponentFactory<MyVoxelCellComponent>.GetAll())
			{
				if (item.IsVisible)
				{
					BoundingBox bb = new BoundingBox(item.Owner.WorldAabb.Min - MyRender11.Environment.Matrices.CameraPosition, item.Owner.WorldAabb.Max - MyRender11.Environment.Matrices.CameraPosition);
					myLinesBatch.AddBoundingBox(bb, new Color(m_lodColors[item.Lod]));
				}
			}
			myLinesBatch.Commit();
		}

		private static int Align(int value, int alignment)
		{
			return (value + (alignment - 1)) & ~(alignment - 1);
		}

		private static void DrawHierarchyDebug()
		{
<<<<<<< HEAD
=======
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MatrixD matrix = MyRender11.Environment.Matrices.ViewProjectionD;
			StringBuilder stringBuilder = new StringBuilder();
			MyLinesBatch batch = MyLinesRenderer.CreateBatch();
			if (MyRender11.Settings.DisplayIDs)
			{
<<<<<<< HEAD
				foreach (MyActor item in MyActorFactory.GetAll())
				{
					MyMergeGroupLeafComponent mergeGroupLeaf = item.GetMergeGroupLeaf();
					MyRenderableComponent renderable = item.GetRenderable();
					Vector3 vector;
					uint iD;
					if (renderable != null)
					{
						vector = renderable.Owner.WorldMatrix.Translation;
						iD = renderable.Owner.ID;
					}
					else
					{
						if (mergeGroupLeaf == null)
						{
							continue;
						}
						vector = mergeGroupLeaf.Owner.WorldMatrix.Translation;
						iD = mergeGroupLeaf.Owner.ID;
					}
					Vector3 vector2 = Vector3D.Transform(vector, ref matrix);
					vector2.X = vector2.X * 0.5f + 0.5f;
					vector2.Y = vector2.Y * -0.5f + 0.5f;
					if (vector2.Z > 0f && vector2.Z < 1f)
					{
						stringBuilder.AppendFormat("{0}", iD);
						MyDebugTextHelpers.DrawText(new Vector2(vector2.X, vector2.Y) * MyRender11.ViewportResolution, stringBuilder, Color.DarkCyan, 0.5f);
					}
					stringBuilder.Clear();
=======
				Enumerator<MyActor> enumerator = MyActorFactory.GetAll().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyActor current = enumerator.get_Current();
						MyMergeGroupLeafComponent mergeGroupLeaf = current.GetMergeGroupLeaf();
						MyRenderableComponent renderable = current.GetRenderable();
						Vector3 vector;
						uint iD;
						if (renderable != null)
						{
							vector = renderable.Owner.WorldMatrix.Translation;
							iD = renderable.Owner.ID;
						}
						else
						{
							if (mergeGroupLeaf == null)
							{
								continue;
							}
							vector = mergeGroupLeaf.Owner.WorldMatrix.Translation;
							iD = mergeGroupLeaf.Owner.ID;
						}
						Vector3 vector2 = Vector3D.Transform(vector, ref matrix);
						vector2.X = vector2.X * 0.5f + 0.5f;
						vector2.Y = vector2.Y * -0.5f + 0.5f;
						if (vector2.Z > 0f && vector2.Z < 1f)
						{
							stringBuilder.AppendFormat("{0}", iD);
							MyDebugTextHelpers.DrawText(new Vector2(vector2.X, vector2.Y) * MyRender11.ViewportResolution, stringBuilder, Color.DarkCyan, 0.5f);
						}
						stringBuilder.Clear();
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			if (MyRender11.Settings.DisplayTreeAabbs)
			{
				MyScene11.DynamicRenderablesDBVH.GetAll(delegate(Action<MyCullResults, bool> x, BoundingBoxD bbD)
				{
					DrawActor((x.Target as MyActorComponent).DebugColor, bbD, batch);
				});
				MyScene11.DynamicRenderablesFarDBVH.GetAll(delegate(Action<MyCullResults, bool> x, BoundingBoxD bbD)
				{
					DrawActor((x.Target as MyActorComponent).DebugColor, bbD, batch);
				});
				if (MyRender11.Settings.DrawGroups)
				{
					MyScene11.ManualCullTree.GetAll(delegate(MyManualCullTreeData userData, BoundingBoxD bbD)
					{
						Matrix m = userData.Actor.WorldMatrix;
<<<<<<< HEAD
						m.Translation -= (Vector3)MyRender11.Environment.Matrices.CameraPosition;
						foreach (KeyValuePair<int, MyBruteCullData> item2 in userData.BruteCull)
						{
							Color color = item2.Value.UserData.DebugColor();
							BoundingBoxD bb3 = new BoundingBoxD(item2.Value.Aabb.Data[16], item2.Value.Aabb.Data[0]);
=======
						m.Translation -= MyRender11.Environment.Matrices.CameraPosition;
						foreach (KeyValuePair<int, MyBruteCullData> item in userData.BruteCull)
						{
							Color color = item.Value.UserData.DebugColor();
							BoundingBoxD bb3 = new BoundingBoxD(item.Value.Aabb.Data[16], item.Value.Aabb.Data[0]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							batch.AddBoundingBox(bb3, color, m);
						}
						BoundingBox bb4 = (BoundingBox)bbD.Translate(-MyRender11.Environment.Matrices.CameraPosition);
						batch.AddBoundingBox(bb4, Color.Red);
					});
				}
			}
			if (MyRender11.Settings.DisplayAabbs)
			{
				HashSetReader<MyActor> all = MyActorFactory.GetAll();
<<<<<<< HEAD
				foreach (MyActor item3 in all)
				{
					if (!item3.IsRoot() && item3.IsVisible && item3.HasLocalAabb)
					{
						BoundingBoxD boundingBoxD = item3.LocalAabb.Transform(item3.WorldMatrix);
						boundingBoxD.Translate(-MyRender11.Environment.Matrices.CameraPosition);
						BoundingBox bb = (BoundingBox)boundingBoxD;
						Color debugColor = item3.GetDebugColor();
						batch.AddBoundingBox(bb, debugColor);
					}
				}
				foreach (MyActor item4 in all)
				{
					if (item4.IsRoot() && item4.IsVisible)
					{
						BoundingBoxD worldAabb = item4.WorldAabb;
						worldAabb.Translate(-MyRender11.Environment.Matrices.CameraPosition);
						BoundingBox bb2 = (BoundingBox)worldAabb;
						batch.AddBoundingBox(bb2, Color.Red);
					}
				}
=======
				Enumerator<MyActor> enumerator = all.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyActor current2 = enumerator.get_Current();
						if (!current2.IsRoot() && current2.IsVisible && current2.HasLocalAabb)
						{
							BoundingBoxD boundingBoxD = current2.LocalAabb.Transform(current2.WorldMatrix);
							boundingBoxD.Translate(-MyRender11.Environment.Matrices.CameraPosition);
							BoundingBox bb = (BoundingBox)boundingBoxD;
							Color debugColor = current2.GetDebugColor();
							batch.AddBoundingBox(bb, debugColor);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				enumerator = all.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyActor current3 = enumerator.get_Current();
						if (current3.IsRoot() && current3.IsVisible)
						{
							BoundingBoxD worldAabb = current3.WorldAabb;
							worldAabb.Translate(-MyRender11.Environment.Matrices.CameraPosition);
							BoundingBox bb2 = (BoundingBox)worldAabb;
							batch.AddBoundingBox(bb2, Color.Red);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			batch.Commit();
		}

		private static void DrawActor(Color color, BoundingBoxD bbD, MyLinesBatch batch)
		{
			BoundingBox bb = (BoundingBox)bbD.Translate(-MyRender11.Environment.Matrices.CameraPosition);
			batch.AddBoundingBox(bb, color);
		}
	}
}
