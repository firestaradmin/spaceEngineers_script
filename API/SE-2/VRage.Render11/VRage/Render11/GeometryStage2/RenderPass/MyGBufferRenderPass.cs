using System.Collections.Generic;
using SharpDX.Direct3D;
using VRage.Library.Collections;
using VRage.Render11.GeometryStage2.Common;
using VRage.Render11.GeometryStage2.Materials;
using VRage.Render11.GeometryStage2.Model;
using VRage.Render11.GeometryStage2.Model.Preprocess;
using VRage.Render11.GeometryStage2.Rendering;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace VRage.Render11.GeometryStage2.RenderPass
{
	[PooledObject(2)]
	internal class MyGBufferRenderPass : MyRenderPass
	{
		private MyViewport m_viewport;

		private MyGBuffer m_gbuffer;

		private IGBufferSrvStrategy m_srvStrategy;

		public void Init(bool isUsedDeferredRC, int passId, MyViewport viewport, MyGBuffer gbuffer, IGBufferSrvStrategy srvStrategy, string debugName, MyRenderData singleRenderData, MyList<MyRenderData> multiRenderData)
		{
			m_viewport = viewport;
			m_gbuffer = gbuffer;
			m_srvStrategy = srvStrategy;
			InitInternal(passId, debugName, isUsedDeferredRC, singleRenderData, multiRenderData);
		}

		protected override void BeginDraw(MyRenderContext RC)
		{
			RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			RC.SetViewport(ref m_viewport);
			RC.SetRtvs(m_gbuffer, MyDepthStencilAccess.ReadWrite);
			RC.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			RC.VertexShader.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			RC.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			RC.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			RC.PixelShader.SetSrv(28, MyGeneratedTextureManager.Dithering8x8Tex);
			RC.PixelShader.SetSrv(29, MyGeneratedTextureManager.RandomTex);
			IConstantBuffer placeholderObjectCB = MyRenderPassUtils.GetPlaceholderObjectCB(RC, 255u);
			RC.VertexShader.SetConstantBuffer(2, placeholderObjectCB);
			RC.PixelShader.SetConstantBuffer(2, placeholderObjectCB);
		}

		protected override void SetRenderData(MyRenderContext RC, IVertexBuffer vbInstanceBuffer, Matrix viewProjMatrix)
		{
			MyRenderPassUtils.FillConstantBuffer(RC, MyCommon.ProjectionConstants, Matrix.Transpose(viewProjMatrix));
			RC.VertexShader.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			RC.SetVertexBuffer(2, vbInstanceBuffer);
		}

		protected override void DrawInstanceLodGroup(MyRenderContext RC, MyInstanceLodGroup itGroup)
		{
			MyLod lod = itGroup.Lod;
			RC.SetVertexBuffer(0, lod.VB0);
			RC.SetVertexBuffer(1, lod.VB1);
			RC.SetIndexBuffer(lod.IB);
			if (MyRender11.Settings.Wireframe)
			{
				RC.SetDepthStencilState(MyDepthStencilStateManager.DepthTestWrite);
				RC.SetBlendState(null);
				RC.SetRasterizerState(MyRasterizerStateManager.NocullWireframeRasterizerState);
			}
			List<MyPreprocessedPart> gBufferParts = lod.PreprocessedParts.GBufferParts;
			MyRenderMaterialBindings[] gBufferParts2 = itGroup.LodInstance.GBufferParts;
			for (int i = 0; i < gBufferParts.Count; i++)
			{
				if (!MyRender11.Settings.Wireframe)
				{
					RC.SetDepthStencilState(gBufferParts[i].DepthStencilState);
					RC.SetRasterizerState(gBufferParts[i].RasterizerState);
					RC.SetBlendState(gBufferParts[i].BlendState);
				}
				MyShaderBundle shaderBundle = gBufferParts[i].GetShaderBundle(itGroup.State, itGroup.MetalnessColorable);
				RC.SetInputLayout(shaderBundle.InputLayout);
				RC.VertexShader.Set(shaderBundle.VertexShader);
				RC.PixelShader.Set(shaderBundle.PixelShader);
				RC.PixelShader.SetSrvs(0, m_srvStrategy.GetSrvs(RC, gBufferParts2[i], lod.LodNum));
				int instancesCount = itGroup.InstancesCount;
				int startInstanceLocation = itGroup.OffsetInInstanceBuffer + (gBufferParts[i].InstanceMaterialOffsetWithinLod + 1) * instancesCount;
				RC.DrawIndexedInstanced(gBufferParts[i].IndicesCount, instancesCount, gBufferParts[i].IndexStart, 0, startInstanceLocation);
				m_stats.Triangles += gBufferParts[i].IndicesCount / 3 * instancesCount;
				m_stats.Draws++;
			}
		}

		protected override void EndDraw(MyRenderContext RC)
		{
		}

		private static IBlendState GetAlphamaskBlendState(string cmFilepath, string ngFilepath, string extFilepath, bool isPremultipliedAlpha)
		{
			bool flag = !string.IsNullOrEmpty(cmFilepath);
			bool flag2 = !string.IsNullOrEmpty(ngFilepath);
			bool flag3 = !string.IsNullOrEmpty(extFilepath);
			if (flag && flag2 && flag3)
			{
				if (isPremultipliedAlpha)
				{
					return MyBlendStateManager.BlendDecalNormalColorExt;
				}
				return MyBlendStateManager.BlendDecalNormalColorExtNoPremult;
			}
			if (flag && flag2 && !flag3)
			{
				if (isPremultipliedAlpha)
				{
					return MyBlendStateManager.BlendDecalNormalColor;
				}
				return MyBlendStateManager.BlendDecalNormalColorNoPremult;
			}
			if (!flag && flag2 && !flag3)
			{
				if (isPremultipliedAlpha)
				{
					return MyBlendStateManager.BlendDecalNormal;
				}
				return MyBlendStateManager.BlendDecalNormalNoPremult;
			}
			if (flag && !flag2 && !flag3)
			{
				if (isPremultipliedAlpha)
				{
					return MyBlendStateManager.BlendDecalColor;
				}
				return MyBlendStateManager.BlendDecalColorNoPremult;
			}
			MyRenderProxy.Error("Unknown alphamask texture pattern");
			return null;
		}

		private static void InitPartForDecal(ref MyPreprocessedPart part, string cmFilepath, string ngFilepath, string extFilepath, bool isPremultipliedAlpha, bool isCutout)
		{
			part.DepthStencilState = MyDepthStencilStateManager.DepthTestReadOnly;
			part.BlendState = (isCutout ? null : GetAlphamaskBlendState(cmFilepath, ngFilepath, extFilepath, isPremultipliedAlpha));
			part.RasterizerState = null;
		}

		public static void PreprocessData(out List<MyPreprocessedPart> parts, MyMwmData mwmData, MyLod lod)
		{
			parts = new List<MyPreprocessedPart>();
			foreach (MyMwmDataPart part2 in mwmData.Parts)
			{
				if (!part2.Technique.IsTransparent())
				{
					MyPreprocessedPart part = default(MyPreprocessedPart);
					part.Init(part2.MaterialName, lod, part2.IndexOffset, part2.IndicesCount, MyModelMaterials.GetMaterial(part2), MyRenderPassType.GBuffer);
					MyMeshDrawTechnique technique = part2.Technique;
					string colorMetalFilepath = part2.ColorMetalFilepath;
					string normalGlossFilepath = part2.NormalGlossFilepath;
					string extensionFilepath = part2.ExtensionFilepath;
					switch (technique)
					{
					case MyMeshDrawTechnique.MESH:
						part.DepthStencilState = MyDepthStencilStateManager.DefaultDepthState;
						part.BlendState = null;
						part.RasterizerState = null;
						break;
					case MyMeshDrawTechnique.DECAL:
						InitPartForDecal(ref part, colorMetalFilepath, normalGlossFilepath, extensionFilepath, isPremultipliedAlpha: true, isCutout: false);
						break;
					case MyMeshDrawTechnique.DECAL_NOPREMULT:
						InitPartForDecal(ref part, colorMetalFilepath, normalGlossFilepath, extensionFilepath, isPremultipliedAlpha: false, isCutout: false);
						break;
					case MyMeshDrawTechnique.DECAL_CUTOUT:
						InitPartForDecal(ref part, colorMetalFilepath, normalGlossFilepath, extensionFilepath, isPremultipliedAlpha: true, isCutout: true);
						break;
					case MyMeshDrawTechnique.ALPHA_MASKED:
						part.DepthStencilState = MyDepthStencilStateManager.DefaultDepthState;
						part.BlendState = null;
						part.RasterizerState = MyRasterizerStateManager.NocullRasterizerState;
						break;
					case MyMeshDrawTechnique.ALPHA_MASKED_SINGLE_SIDED:
						part.DepthStencilState = MyDepthStencilStateManager.DefaultDepthState;
						part.BlendState = null;
						part.RasterizerState = null;
						break;
					default:
						MyRenderProxy.Error("Material is not resolved");
						break;
					}
					parts.Add(part);
				}
			}
		}
	}
}
