using System.Collections.Generic;
using SharpDX.Direct3D;
using VRage.Render11.Common;
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
	internal class MyTransparentRenderPass : MyRenderPass
	{
		private MyViewport m_viewport;

		private bool m_flareOccludersOnly;

		private ISrvBindable m_lastFrameSrv;

		private ISrvBindable m_depthSrv;

		public void Init(bool isUsedDeferredRC, int passId, MyViewport viewport, MyRenderData singleRenderData, bool flareOccludersOnly, ISrvBindable lastFrameSrv, ISrvBindable depthSrv, MyRenderContext rc = null)
		{
			m_lastFrameSrv = lastFrameSrv;
			m_depthSrv = depthSrv;
			m_viewport = viewport;
			m_flareOccludersOnly = flareOccludersOnly;
			InitInternal(passId, "Transparent", isUsedDeferredRC, singleRenderData, null, rc);
		}

		protected override void BeginDraw(MyRenderContext RC)
		{
			RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			RC.SetViewport(ref m_viewport);
			RC.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			RC.VertexShader.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			RC.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			RC.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			RC.PixelShader.SetSrv(28, MyGeneratedTextureManager.Dithering8x8Tex);
			RC.PixelShader.SetSrv(29, MyGeneratedTextureManager.RandomTex);
			IConstantBuffer placeholderObjectCB = MyRenderPassUtils.GetPlaceholderObjectCB(RC, 255u);
			RC.VertexShader.SetConstantBuffer(2, placeholderObjectCB);
			IRtvArrayTexture closeCubemapFinal = MyManagers.EnvironmentProbe.CloseCubemapFinal;
			IRtvArrayTexture farCubemapFinal = MyManagers.EnvironmentProbe.FarCubemapFinal;
			RC.PixelShader.SetSrv(11, closeCubemapFinal);
			RC.PixelShader.SetSrv(17, farCubemapFinal);
			RC.PixelShader.SetSrv(16, MyCommon.GetAmbientBrdfLut());
			RC.PixelShader.SetSampler(15, MySamplerStateManager.Shadowmap);
			RC.PixelShader.SetSrv(15, MyManagers.Shadows.ShadowCascades.CascadeShadowmapArray);
			RC.PixelShader.SetSrv(18, m_lastFrameSrv);
			RC.PixelShader.SetSrv(21, m_depthSrv);
			if (m_flareOccludersOnly)
			{
				RC.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
				RC.PixelShader.Set(null);
			}
			else
			{
				RC.SetDepthStencilState(MyDepthStencilStateManager.DepthTestReadOnly);
			}
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
			List<MyPreprocessedPart> transparentParts = itGroup.Lod.PreprocessedParts.TransparentParts;
			if (transparentParts == null)
			{
				return;
			}
			RC.SetVertexBuffer(0, lod.VB0);
			RC.SetVertexBuffer(1, lod.VB1);
			RC.SetIndexBuffer(lod.IB);
			MyRenderMaterialBindings[] transparentParts2 = itGroup.LodInstance.TransparentParts;
			for (int i = 0; i < transparentParts.Count; i++)
			{
				if (!m_flareOccludersOnly || transparentParts[i].Material.IsFlareOccluder)
				{
					MyShaderBundle shaderBundle = transparentParts[i].GetShaderBundle(itGroup.State, itGroup.MetalnessColorable);
					RC.SetInputLayout(shaderBundle.InputLayout);
					RC.VertexShader.Set(shaderBundle.VertexShader);
					if (!m_flareOccludersOnly)
					{
						RC.PixelShader.Set(shaderBundle.PixelShader);
					}
					RC.PixelShader.SetSrvs(0, transparentParts2[i].Srvs);
					IConstantBuffer transparentMaterialCB = MyRenderPassUtils.GetTransparentMaterialCB(RC, transparentParts[i].Material.Material, itGroup.Lod.BoundingBox.HalfExtents.X);
					RC.PixelShader.SetConstantBuffer(3, transparentMaterialCB);
					RC.VertexShader.SetConstantBuffer(3, transparentMaterialCB);
					RC.SetRasterizerState(MyRender11.Settings.Wireframe ? MyRasterizerStateManager.WireframeRasterizerState : (transparentParts[i].Material.Material.TriangleFaceCulling ? null : MyRasterizerStateManager.NocullRasterizerState));
					int instancesCount = itGroup.InstancesCount;
					int offsetInInstanceBuffer = itGroup.OffsetInInstanceBuffer;
					RC.DrawIndexedInstanced(transparentParts[i].IndicesCount, instancesCount, transparentParts[i].IndexStart, 0, offsetInInstanceBuffer);
					m_stats.Triangles += transparentParts[i].IndicesCount / 3 * instancesCount;
					m_stats.Draws++;
				}
			}
		}

		protected override void EndDraw(MyRenderContext RC)
		{
			RC.SetRasterizerState(null);
			RC.PixelShader.SetSrv(18, null);
			RC.PixelShader.SetSrv(21, null);
		}

		public static void PreprocessData(out List<MyPreprocessedPart> parts, MyMwmData mwmData, MyLod lod)
		{
			parts = null;
			foreach (MyMwmDataPart part in mwmData.Parts)
			{
				string materialName = part.MaterialName;
				if (part.Technique.IsTransparent())
				{
					if (parts == null)
					{
						parts = new List<MyPreprocessedPart>();
					}
					MyPreprocessedPart item = default(MyPreprocessedPart);
					item.Init(materialName, lod, part.IndexOffset, part.IndicesCount, VRage.Render11.GeometryStage2.Materials.MyTransparentMaterials.GetMaterial(materialName, part.Technique), MyRenderPassType.Transparent);
					parts.Add(item);
				}
			}
		}
	}
}
