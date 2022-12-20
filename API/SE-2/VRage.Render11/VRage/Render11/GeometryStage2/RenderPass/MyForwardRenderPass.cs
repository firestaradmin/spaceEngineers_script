using System.Collections.Generic;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using VRage.Library.Collections;
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
	internal class MyForwardRenderPass : MyRenderPass
	{
		private MyViewport m_viewport;

		private IDsvBindable m_dsv;

		private readonly RenderTargetView[] m_rtvs = new RenderTargetView[2];

		private IGBufferSrvStrategy m_srvStrategy;

		private IConstantBuffer m_cbForward;

		private ISrvBindable m_srvDepth;

		public void Init(bool isUsedDeferredRC, int passId, MyViewport viewport, RenderTargetView[] rtvs, IDsvBindable dsv, ISrvBindable srvDepth, IGBufferSrvStrategy srvStrategy, string debugName, MyRenderData singleRenderData, MyList<MyRenderData> multiRenderData)
		{
			m_viewport = viewport;
			m_rtvs[0] = rtvs[0];
			m_rtvs[1] = rtvs[1];
			m_dsv = dsv;
			m_srvDepth = srvDepth;
			m_srvStrategy = srvStrategy;
			InitInternal(passId, debugName, isUsedDeferredRC, singleRenderData, multiRenderData);
		}

		protected override void BeginDraw(MyRenderContext RC)
		{
			RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			RC.SetViewport(ref m_viewport);
			RC.SetRtvs(m_dsv, m_rtvs);
			RC.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			RC.AllShaderStages.SetConstantBuffer(0, MyCommon.FrameConstants);
			RC.AllShaderStages.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			RC.PixelShader.SetSrv(28, MyGeneratedTextureManager.Dithering8x8Tex);
			RC.PixelShader.SetSrv(29, MyGeneratedTextureManager.RandomTex);
			IConstantBuffer placeholderObjectCB = MyRenderPassUtils.GetPlaceholderObjectCB(RC, 255u);
			RC.VertexShader.SetConstantBuffer(2, placeholderObjectCB);
			RC.PixelShader.SetConstantBuffer(2, placeholderObjectCB);
			m_cbForward = MyCommon.GetForwardConstants(m_RC, m_singleRenderData.ProjectionMatrix);
			RC.PixelShader.SetConstantBuffer(7, m_cbForward);
			RC.AllShaderStages.SetConstantBuffer(4, MyManagers.Shadows.ShadowCascades.CascadeConstantBufferOld);
			RC.PixelShader.SetSampler(15, MySamplerStateManager.Shadowmap);
			RC.PixelShader.SetSrv(15, MyManagers.Shadows.ShadowCascades.CascadeShadowmapArrayOld);
		}

		protected override void SetRenderData(MyRenderContext RC, IVertexBuffer vbInstanceBuffer, Matrix viewProjMatrix)
		{
			MyRenderPassUtils.FillConstantBuffer(RC, MyCommon.ProjectionConstants, Matrix.Transpose(viewProjMatrix));
			RC.AllShaderStages.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			RC.SetVertexBuffer(2, vbInstanceBuffer);
		}

		protected override void DrawInstanceLodGroup(MyRenderContext RC, MyInstanceLodGroup itGroup)
		{
			MyLod lod = itGroup.Lod;
			List<MyPreprocessedPart> forwardParts = lod.PreprocessedParts.ForwardParts;
			MyRenderMaterialBindings[] forwardParts2 = itGroup.LodInstance.ForwardParts;
			for (int i = 0; i < forwardParts.Count; i++)
			{
				RC.SetVertexBuffer(0, lod.VB0);
				RC.SetVertexBuffer(1, lod.VB1);
				RC.SetIndexBuffer(lod.IB);
				if (MyRender11.Settings.Wireframe)
				{
					RC.SetDepthStencilState(MyDepthStencilStateManager.DepthTestWrite);
					RC.SetBlendState(null);
					RC.SetRasterizerState(MyRasterizerStateManager.NocullWireframeRasterizerState);
				}
				else
				{
					RC.SetDepthStencilState(forwardParts[i].DepthStencilState);
					RC.SetRasterizerState(forwardParts[i].RasterizerState);
					RC.SetBlendState(forwardParts[i].BlendState);
				}
				MyShaderBundle shaderBundle = forwardParts[i].GetShaderBundle(itGroup.State, itGroup.MetalnessColorable);
				RC.SetInputLayout(shaderBundle.InputLayout);
				RC.VertexShader.Set(shaderBundle.VertexShader);
				RC.PixelShader.Set(shaderBundle.PixelShader);
				RC.PixelShader.SetSrvs(0, m_srvStrategy.GetSrvs(RC, forwardParts2[i], lod.LodNum));
				int instancesCount = itGroup.InstancesCount;
				int startInstanceLocation = itGroup.OffsetInInstanceBuffer + (forwardParts[i].InstanceMaterialOffsetWithinLod + 1) * itGroup.InstancesCount;
				RC.DrawIndexedInstanced(forwardParts[i].IndicesCount, instancesCount, forwardParts[i].IndexStart, 0, startInstanceLocation);
				m_stats.Triangles += forwardParts[i].IndicesCount / 3 * instancesCount;
				m_stats.Draws++;
			}
		}

		protected override void EndDraw(MyRenderContext RC)
		{
			MyCommon.MyScreenLayout myScreenLayout = default(MyCommon.MyScreenLayout);
			myScreenLayout.Offset = Vector2I.Zero;
			myScreenLayout.Resolution = new Vector2(m_viewport.Width, m_viewport.Height);
			MyCommon.MyScreenLayout layout = myScreenLayout;
			MyCommon.ReturnForwardConstants(m_cbForward);
			m_cbForward = null;
			RC.ResetTargets();
			MyManagers.Shadows.ShadowCascades.Gather(RC, ref layout, m_srvDepth, base.ViewId);
		}

		public static void PreprocessData(out List<MyPreprocessedPart> parts, MyMwmData mwmData, MyLod lod)
		{
			parts = new List<MyPreprocessedPart>();
			foreach (MyMwmDataPart part in mwmData.Parts)
			{
				if (!part.Technique.IsTransparent() && part.Technique == MyMeshDrawTechnique.MESH)
				{
					MyPreprocessedPart item = default(MyPreprocessedPart);
					item.Init(part.MaterialName, lod, part.IndexOffset, part.IndicesCount, MyModelMaterials.GetMaterial(part), MyRenderPassType.Forward);
					item.DepthStencilState = MyDepthStencilStateManager.DefaultDepthState;
					item.BlendState = null;
					item.RasterizerState = null;
					parts.Add(item);
				}
			}
		}
	}
}
