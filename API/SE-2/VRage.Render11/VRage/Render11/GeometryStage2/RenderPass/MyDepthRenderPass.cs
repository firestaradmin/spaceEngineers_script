using System.Collections.Generic;
using SharpDX.Direct3D;
using VRage.Library.Collections;
using VRage.Render11.Culling;
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
	internal class MyDepthRenderPass : MyRenderPass
	{
		private enum MyDepthPartType
		{
			NOT_DRAWN,
			SOLID,
			ALPHA_MASKED,
			TRANSPARENT
		}

		private MyViewport m_viewport;

		private IDsvBindable m_dsv;

		private bool m_isCascade;

		private IRasterizerState m_rasterizer;

		public void Init(bool isUsedDeferredRC, int passId, MyViewport viewport, IDsvBindable dsv, bool isCascade, string debugName, MyRenderData singleRenderData, MyList<MyRenderData> multiRenderData, MyCullQuery cullQuery)
		{
			m_viewport = viewport;
			m_dsv = dsv;
			m_isCascade = isCascade;
			m_rasterizer = cullQuery.Rasterizer;
			InitInternal(passId, debugName, isUsedDeferredRC, singleRenderData, multiRenderData);
		}

		protected override void BeginDraw(MyRenderContext RC)
		{
			RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			RC.SetViewport(ref m_viewport);
			RC.SetRtv(m_dsv);
			RC.SetRasterizerState(m_rasterizer);
			RC.SetDepthStencilState(null);
			RC.SetBlendState(null);
			RC.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			RC.VertexShader.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			RC.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			RC.PixelShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			RC.PixelShader.SetSrv(28, MyGeneratedTextureManager.Dithering8x8Tex);
			RC.PixelShader.SetSrv(29, MyGeneratedTextureManager.RandomTex);
			IConstantBuffer placeholderObjectCB = MyRenderPassUtils.GetPlaceholderObjectCB(RC, 0u);
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
			RC.SetIndexBuffer(lod.IB);
			List<MyPreprocessedPart> depthParts = itGroup.Lod.PreprocessedParts.DepthParts;
			MyRenderMaterialBindings[] depthParts2 = itGroup.LodInstance.DepthParts;
			for (int i = 0; i < depthParts.Count; i++)
			{
				MyShaderBundle shaderBundle = depthParts[i].GetShaderBundle(itGroup.State, itGroup.MetalnessColorable);
				RC.SetInputLayout(shaderBundle.InputLayout);
				RC.VertexShader.Set(shaderBundle.VertexShader);
				RC.PixelShader.Set(shaderBundle.PixelShader);
				RC.SetRasterizerState(depthParts[i].RasterizerState ?? m_rasterizer);
				if (depthParts2[i].Srvs != null)
				{
					RC.PixelShader.SetSrvs(0, depthParts2[i].Srvs);
				}
				int instancesCount = itGroup.InstancesCount;
				int offsetInInstanceBuffer = itGroup.OffsetInInstanceBuffer;
				RC.DrawIndexedInstanced(depthParts[i].IndicesCount, instancesCount, depthParts[i].IndexStart, 0, offsetInInstanceBuffer);
				m_stats.Triangles += depthParts[i].IndicesCount / 3 * instancesCount;
				m_stats.Draws++;
			}
		}

		protected override void EndDraw(MyRenderContext RC)
		{
		}

		private static MyDepthPartType GetDepthPartType(string materialName, MyMeshDrawTechnique technique)
		{
			switch (technique)
			{
			case MyMeshDrawTechnique.DECAL:
			case MyMeshDrawTechnique.DECAL_NOPREMULT:
			case MyMeshDrawTechnique.DECAL_CUTOUT:
				return MyDepthPartType.NOT_DRAWN;
			case MyMeshDrawTechnique.ALPHA_MASKED:
			case MyMeshDrawTechnique.ALPHA_MASKED_SINGLE_SIDED:
				return MyDepthPartType.ALPHA_MASKED;
			default:
				if (technique.IsTransparent())
				{
					return MyDepthPartType.TRANSPARENT;
				}
				return MyDepthPartType.SOLID;
			}
		}

		private static void AddDepthSegment(List<MyPreprocessedPart> parts, MyMwmDataPart prevPart, MyLod lod, MyDepthPartType prevType, int indexStart, int indicesCount)
		{
			switch (prevType)
			{
			case MyDepthPartType.SOLID:
			{
				MyRenderMaterial myRenderMaterial = default(MyRenderMaterial);
				myRenderMaterial.DebugMaterialName = "Depth placeholder";
				myRenderMaterial.Technique = MyMeshDrawTechnique.MESH;
				MyRenderMaterial material2 = myRenderMaterial;
				if (indicesCount != 0)
				{
					MyPreprocessedPart item2 = default(MyPreprocessedPart);
					string name = "Unnamed part";
					item2.Init(name, lod, indexStart, indicesCount, material2, MyRenderPassType.Depth);
					parts.Add(item2);
				}
				break;
			}
			case MyDepthPartType.ALPHA_MASKED:
			{
				MyRenderMaterial material = MyModelMaterials.GetMaterial(prevPart);
				if (indicesCount != 0)
				{
					MyPreprocessedPart item = default(MyPreprocessedPart);
					string materialName = prevPart.MaterialName;
					item.Init(materialName, lod, indexStart, indicesCount, material, MyRenderPassType.Depth);
					parts.Add(item);
				}
				break;
			}
			default:
				_ = 3;
				break;
			case MyDepthPartType.NOT_DRAWN:
				break;
			}
		}

		public static void PreprocessData(out List<MyPreprocessedPart> parts, MyMwmData mwmData, MyLod lod)
		{
			int num = 0;
			int num2 = 0;
			parts = new List<MyPreprocessedPart>();
			MyDepthPartType myDepthPartType = MyDepthPartType.NOT_DRAWN;
			MyMwmDataPart prevPart = null;
			foreach (MyMwmDataPart part in mwmData.Parts)
			{
				string materialName = part.MaterialName;
				MyDepthPartType depthPartType = GetDepthPartType(materialName, part.Technique);
				if (myDepthPartType != depthPartType || myDepthPartType == MyDepthPartType.ALPHA_MASKED)
				{
					AddDepthSegment(parts, part, lod, myDepthPartType, num, num2);
					num += num2;
					num2 = 0;
				}
				num2 += part.IndicesCount;
				myDepthPartType = GetDepthPartType(materialName, part.Technique);
				prevPart = part;
			}
			AddDepthSegment(parts, prevPart, lod, myDepthPartType, num, num2);
		}
	}
}
