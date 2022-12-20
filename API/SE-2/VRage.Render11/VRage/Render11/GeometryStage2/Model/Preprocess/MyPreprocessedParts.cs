using System.Collections.Generic;
using VRage.Render11.GeometryStage2.RenderPass;
using VRage.Render11.GeometryStage2.SpecialPass;

namespace VRage.Render11.GeometryStage2.Model.Preprocess
{
	internal class MyPreprocessedParts
	{
		public List<MyPreprocessedPart> GBufferParts;

		public List<MyPreprocessedPart> ForwardParts;

		public List<MyPreprocessedPart> DepthParts;

		public List<MyPreprocessedPart> TransparentParts;

		public List<MyPreprocessedPart> TransparentForDecalsParts;

		public List<MyPreprocessedPart> HighlightParts;

		public Dictionary<string, MyPreprocessedSection> HighlightSections;

		public void Init(MyMwmData mwmData, MyLod parentLod)
		{
			MyGBufferRenderPass.PreprocessData(out GBufferParts, mwmData, parentLod);
			MyForwardRenderPass.PreprocessData(out ForwardParts, mwmData, parentLod);
			MyDepthRenderPass.PreprocessData(out DepthParts, mwmData, parentLod);
			MyTransparentRenderPass.PreprocessData(out TransparentParts, mwmData, parentLod);
			MyTransparentForDecalsRenderPass.PreprocessData(out TransparentForDecalsParts, mwmData, parentLod);
			MyHighlightSpecialPass.PreprocessData(out HighlightParts, out HighlightSections, mwmData, parentLod);
		}

		public void AddInstanceMaterial(string materialName, int instanceMaterialOffsetWithinLod)
		{
			for (int i = 0; i < GBufferParts.Count; i++)
			{
				if (GBufferParts[i].Name == materialName)
				{
					MyPreprocessedPart value = GBufferParts[i];
					value.InstanceMaterialOffsetWithinLod = instanceMaterialOffsetWithinLod;
					GBufferParts[i] = value;
				}
			}
			for (int j = 0; j < ForwardParts.Count; j++)
			{
				if (ForwardParts[j].Name == materialName)
				{
					MyPreprocessedPart value2 = ForwardParts[j];
					value2.InstanceMaterialOffsetWithinLod = instanceMaterialOffsetWithinLod;
					ForwardParts[j] = value2;
				}
			}
		}
	}
}
