using System;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Common;
using VRage.Render11.GeometryStage2.Materials;
using VRage.Render11.GeometryStage2.Rendering;
using VRage.Render11.Resources;

namespace VRage.Render11.GeometryStage2.Model.Preprocess
{
	internal struct MyPreprocessedPart
	{
		private MyShaderBundle[] m_shaderBundles;

		public int InstanceMaterialOffsetWithinLod;

		public IDepthStencilState DepthStencilState;

		public IBlendState BlendState;

		public IRasterizerState RasterizerState;

		private static int[] m_lodStates;

		public string Name { get; private set; }

		public MyLod Lod { get; private set; }

		public int IndexStart { get; private set; }

		public int IndicesCount { get; private set; }

		public MyRenderMaterial Material { get; private set; }

		static MyPreprocessedPart()
		{
			m_lodStates = (int[])Enum.GetValues(typeof(MyInstanceLodState));
		}

		public void Init(string name, MyLod lod, int indexStart, int indicesCount, MyRenderMaterial material, MyRenderPassType passType)
		{
			Name = name;
			Lod = lod;
			IndexStart = indexStart;
			IndicesCount = indicesCount;
			Material = material;
			m_shaderBundles = new MyShaderBundle[m_lodStates.Length * 2];
			for (int i = 0; i < 2; i++)
			{
				bool metalnessColorable = i > 0;
				int[] lodStates = m_lodStates;
				foreach (int num in lodStates)
				{
					m_shaderBundles[num + i * m_lodStates.Length] = MyManagers.ShaderBundles.GetShaderBundle(passType, material.Technique, (MyInstanceLodState)num, material.IsColorMetalTexture, material.IsNormalGlossTexture, material.IsExtensionTexture, metalnessColorable);
				}
			}
			InstanceMaterialOffsetWithinLod = -1;
			DepthStencilState = null;
			BlendState = null;
			RasterizerState = null;
		}

		public MyShaderBundle GetShaderBundle(MyInstanceLodState state, bool metalnessColorable)
		{
			return m_shaderBundles[(int)(state + (metalnessColorable ? m_lodStates.Length : 0))];
		}
	}
}
