<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using SharpDX.Direct3D;
using VRage.Render11.GeometryStage2.Common;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.Materials;
using VRage.Render11.GeometryStage2.Model;
using VRage.Render11.GeometryStage2.Model.Preprocess;
using VRage.Render11.GeometryStage2.Rendering;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Scene.Components;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace VRage.Render11.GeometryStage2.SpecialPass
{
	internal class MyHighlightSpecialPass
	{
		private unsafe static IConstantBuffer GetObjectCB(MyRenderContext RC, MyInstanceComponent instance, Vector2 stateData)
		{
			instance.UpdateWorldMatrix();
			instance.GetMatrixCols(out var m);
			Matrix identity = Matrix.Identity;
			identity.SetRow(0, m.Col0);
			identity.SetRow(1, m.Col1);
			identity.SetRow(2, m.Col2);
			identity = Matrix.Transpose(identity);
			int num = sizeof(MyObjectDataCommon);
			num += sizeof(MyObjectDataNonVoxel);
			IConstantBuffer objectCB = RC.GetObjectCB(num);
			MyMapping myMapping = MyMapping.MapDiscard(RC, objectCB);
			MyObjectDataNonVoxel data = default(MyObjectDataNonVoxel);
			myMapping.WriteAndPosition(ref data);
			MyObjectDataCommon myObjectDataCommon = default(MyObjectDataCommon);
			myObjectDataCommon.LocalMatrix = identity;
			myObjectDataCommon.ColorMul = Vector3.One;
			myObjectDataCommon.KeyColor = new Vector3(0f, -1f, 0f);
			myObjectDataCommon.CustomAlpha = stateData;
			MyObjectDataCommon data2 = myObjectDataCommon;
			myMapping.WriteAndPosition(ref data2);
			myMapping.Unmap();
			return objectCB;
		}

		public static void DrawInstanceComponent(MyInstanceComponent instance, HashSet<MyHighlightDesc> highlightDescs)
		{
<<<<<<< HEAD
=======
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyRenderContext rC = MyRender11.RC;
			MyMapping myMapping = MyMapping.MapDiscard(MyCommon.ProjectionConstants);
			Matrix viewProjectionAt = MyRender11.Environment.Matrices.ViewProjectionAt0;
			viewProjectionAt = Matrix.Transpose(viewProjectionAt);
			myMapping.WriteAndPosition(ref viewProjectionAt);
			myMapping.Unmap();
			rC.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			rC.VertexShader.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			rC.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			rC.PixelShader.SetSrv(28, MyGeneratedTextureManager.Dithering8x8Tex);
			rC.PixelShader.SetSrv(29, MyGeneratedTextureManager.RandomTex);
			rC.SetDepthStencilState(MyDepthStencilStateManager.WriteHighlightStencil, 64);
			rC.SetBlendState(null);
			rC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			rC.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
			rC.SetScreenViewport();
			rC.PixelShader.SetConstantBuffer(4, MyCommon.HighlightConstants);
			MyLod highlightLod = instance.GetHighlightLod();
			MyInstanceLodState state = MyInstanceLodState.Solid;
			rC.SetIndexBuffer(highlightLod.IB);
			rC.SetVertexBuffer(0, highlightLod.VB0);
			IConstantBuffer objectCB = GetObjectCB(rC, instance, Vector2.Zero);
			rC.VertexShader.SetConstantBuffer(2, objectCB);
			rC.PixelShader.SetConstantBuffer(2, objectCB);
<<<<<<< HEAD
			foreach (MyHighlightDesc highlightDesc in highlightDescs)
			{
				MyHighlightDesc desc = highlightDesc;
				MyHighlight.WriteHighlightConstants(ref desc);
				if (string.IsNullOrEmpty(highlightDesc.SectionName))
				{
					List<MyPreprocessedPart> highlightParts = highlightLod.PreprocessedParts.HighlightParts;
					MyRenderMaterialBindings[] highlightParts2 = instance.GetHighlightLodInstance().HighlightParts;
					for (int i = 0; i < highlightParts.Count; i++)
					{
						DrawHighlightedPart(rC, highlightParts[i], highlightParts2[i], state, pixelShader: true);
					}
					continue;
				}
				Dictionary<string, MyPreprocessedSection> highlightSections = highlightLod.PreprocessedParts.HighlightSections;
				if (highlightSections == null || !highlightSections.ContainsKey(highlightDesc.SectionName))
				{
					continue;
				}
				foreach (MyPreprocessedPart part in highlightSections[highlightDesc.SectionName].Parts)
				{
					DrawHighlightedPart(rC, part, default(MyRenderMaterialBindings), state, pixelShader: true);
				}
			}
=======
			Enumerator<MyHighlightDesc> enumerator = highlightDescs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyHighlightDesc current = enumerator.get_Current();
					MyHighlightDesc desc = current;
					MyHighlight.WriteHighlightConstants(ref desc);
					if (string.IsNullOrEmpty(current.SectionName))
					{
						List<MyPreprocessedPart> highlightParts = highlightLod.PreprocessedParts.HighlightParts;
						MyRenderMaterialBindings[] highlightParts2 = instance.GetHighlightLodInstance().HighlightParts;
						for (int i = 0; i < highlightParts.Count; i++)
						{
							DrawHighlightedPart(rC, highlightParts[i], highlightParts2[i], state, pixelShader: true);
						}
						continue;
					}
					Dictionary<string, MyPreprocessedSection> highlightSections = highlightLod.PreprocessedParts.HighlightSections;
					if (highlightSections == null || !highlightSections.ContainsKey(current.SectionName))
					{
						continue;
					}
					foreach (MyPreprocessedPart part in highlightSections[current.SectionName].Parts)
					{
						DrawHighlightedPart(rC, part, default(MyRenderMaterialBindings), state, pixelShader: true);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void DrawHighlightedPart(MyRenderContext RC, MyPreprocessedPart part, MyRenderMaterialBindings materialInstance, MyInstanceLodState state, bool pixelShader)
		{
			MyShaderBundle shaderBundle = part.GetShaderBundle(state, metalnessColorable: false);
			RC.SetInputLayout(shaderBundle.InputLayout);
			RC.VertexShader.Set(shaderBundle.VertexShader);
			if (pixelShader)
			{
				RC.PixelShader.Set(shaderBundle.PixelShader);
			}
			RC.PixelShader.SetSrv(3, materialInstance.SrvAlphamask);
			RC.DrawIndexed(part.IndicesCount, part.IndexStart, 0);
		}

		public static void DrawOverlappingInstance(MyRenderContext RC, MyInstanceComponent instance)
		{
			MyMapping myMapping = MyMapping.MapDiscard(MyCommon.ProjectionConstants);
			Matrix viewProjectionAt = MyRender11.Environment.Matrices.ViewProjectionAt0;
			viewProjectionAt = Matrix.Transpose(viewProjectionAt);
			myMapping.WriteAndPosition(ref viewProjectionAt);
			myMapping.Unmap();
			RC.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			RC.VertexShader.SetConstantBuffer(1, MyCommon.ProjectionConstants);
			RC.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			RC.PixelShader.SetSrv(28, MyGeneratedTextureManager.Dithering8x8Tex);
			RC.PixelShader.SetSrv(29, MyGeneratedTextureManager.RandomTex);
			RC.SetDepthStencilState(MyDepthStencilStateManager.WriteOverlappingHighlightStencil, 128);
			RC.SetBlendState(null);
			RC.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			RC.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
			RC.SetScreenViewport();
			RC.PixelShader.SetConstantBuffer(4, MyCommon.HighlightConstants);
			MyLod highlightLod = instance.GetHighlightLod();
			RC.SetIndexBuffer(highlightLod.IB);
			RC.SetVertexBuffer(0, highlightLod.VB0);
			IConstantBuffer objectCB = GetObjectCB(RC, instance, Vector2.Zero);
			RC.VertexShader.SetConstantBuffer(2, objectCB);
			RC.PixelShader.SetConstantBuffer(2, objectCB);
			RC.PixelShader.Set(null);
			List<MyPreprocessedPart> highlightParts = highlightLod.PreprocessedParts.HighlightParts;
			MyRenderMaterialBindings[] highlightParts2 = instance.GetHighlightLodInstance().HighlightParts;
			for (int i = 0; i < highlightParts.Count; i++)
			{
				DrawHighlightedPart(RC, highlightParts[i], highlightParts2[i], MyInstanceLodState.Solid, pixelShader: false);
			}
		}

		public static void PreprocessData(out List<MyPreprocessedPart> parts, out Dictionary<string, MyPreprocessedSection> sections, MyMwmData mwmData, MyLod lod)
		{
			parts = new List<MyPreprocessedPart>();
			foreach (MyMwmDataPart part in mwmData.Parts)
			{
				MyPreprocessedPart item = default(MyPreprocessedPart);
				item.Init(part.MaterialName, lod, part.IndexOffset, part.IndicesCount, MyModelMaterials.GetMaterial(part), MyRenderPassType.Highlight);
				parts.Add(item);
			}
			sections = new Dictionary<string, MyPreprocessedSection>();
			if (mwmData.SectionInfos == null)
			{
				return;
			}
			foreach (MyMeshSectionInfo sectionInfo in mwmData.SectionInfos)
			{
				string name = sectionInfo.Name;
				MyPreprocessedSection myPreprocessedSection = new MyPreprocessedSection();
				myPreprocessedSection.Init(MyRenderPassType.Highlight, lod, name, sectionInfo, mwmData.Parts, parts);
				sections.Add(name, myPreprocessedSection);
			}
		}
	}
}
