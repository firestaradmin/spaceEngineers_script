using SharpDX.Direct3D11;

namespace VRage.Render11.RenderContext
{
	internal class MyPixelStage : MyCommonStage
	{
		private PixelShader m_pixelShader;

		internal override void ClearState()
		{
			base.ClearState();
			m_pixelShader = null;
		}

		public void Set(PixelShader shader)
		{
			if (shader != m_pixelShader)
			{
				m_pixelShader = shader;
				m_deviceContext.PixelShader.Set(shader);
				m_statistics.SetPixelShaders++;
			}
		}
	}
}
