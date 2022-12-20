using System.Collections.Generic;
using SharpDX.Direct3D;
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
	internal class MyTransparentForDecalsRenderPass : MyRenderPass
	{
		private MyViewport m_viewport;

		private IDepthStencil m_outputDepth;

		private IDepthStencil m_inputFilterDepth;

		private IRtvTexture m_gbuffer1;

		public void Init(bool isUsedDeferredRC, int passId, MyViewport viewport, IDepthStencil outputDepth, IDepthStencil inputFilterDepth, IRtvTexture gbuffer1, MyRenderData renderData, MyRenderContext rc)
		{
			m_viewport = viewport;
			m_outputDepth = outputDepth;
			m_inputFilterDepth = inputFilterDepth;
			m_gbuffer1 = gbuffer1;
			InitInternal(passId, "TransparentForDecals", isUsedDeferredRC, renderData, null, rc);
		}

		protected override void BeginDraw(MyRenderContext RC)
		{
			RC.PixelShader.SetSrvs(0, null, null, null, null, null);
			RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			RC.SetViewport(ref m_viewport);
			RC.SetRtv(m_outputDepth.Dsv, m_gbuffer1);
			RC.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			RC.VertexShader.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			RC.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			RC.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			RC.PixelShader.SetSrv(28, MyGeneratedTextureManager.Dithering8x8Tex);
			RC.PixelShader.SetSrv(29, MyGeneratedTextureManager.RandomTex);
			RC.PixelShader.SetSrv(0, m_inputFilterDepth.SrvDepth);
			RC.VertexShader.SetConstantBuffer(2, MyRenderPassUtils.GetPlaceholderObjectCB(RC, 255u));
			RC.PixelShader.SetConstantBuffer(2, MyRenderPassUtils.GetPlaceholderTransparentMaterialCB(RC));
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
			List<MyPreprocessedPart> transparentForDecalsParts = itGroup.Lod.PreprocessedParts.TransparentForDecalsParts;
			if (transparentForDecalsParts == null)
			{
				return;
			}
			foreach (MyPreprocessedPart item in transparentForDecalsParts)
			{
				RC.SetVertexBuffer(0, lod.VB0);
				RC.SetVertexBuffer(1, lod.VB1);
				RC.SetIndexBuffer(lod.IB);
				RC.SetBlendState(null);
				RC.SetRasterizerState(null);
				RC.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
				MyShaderBundle shaderBundle = item.GetShaderBundle(itGroup.State, itGroup.MetalnessColorable);
				RC.SetInputLayout(shaderBundle.InputLayout);
				RC.VertexShader.Set(shaderBundle.VertexShader);
				RC.PixelShader.Set(shaderBundle.PixelShader);
				int instancesCount = itGroup.InstancesCount;
				int offsetInInstanceBuffer = itGroup.OffsetInInstanceBuffer;
				RC.DrawIndexedInstanced(item.IndicesCount, instancesCount, item.IndexStart, 0, offsetInInstanceBuffer);
				m_stats.Triangles += item.IndicesCount / 3 * instancesCount;
				m_stats.Draws++;
			}
		}

		protected override void EndDraw(MyRenderContext RC)
		{
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
					item.Init(materialName, lod, part.IndexOffset, part.IndicesCount, VRage.Render11.GeometryStage2.Materials.MyTransparentMaterials.GetMaterial(part.MaterialName, part.Technique), MyRenderPassType.TransparentForDecals);
					parts.Add(item);
				}
			}
		}
	}
}
