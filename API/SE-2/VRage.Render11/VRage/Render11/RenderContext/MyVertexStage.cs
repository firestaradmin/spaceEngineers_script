using SharpDX.Direct3D11;

namespace VRage.Render11.RenderContext
{
	internal class MyVertexStage : MyCommonStage
	{
		private VertexShader m_vertexShader;

		internal override void ClearState()
		{
			base.ClearState();
			m_vertexShader = null;
		}

		internal void Set(VertexShader shader)
		{
			if (shader != m_vertexShader)
			{
				m_vertexShader = shader;
				m_deviceContext.VertexShader.Set(shader);
				m_statistics.SetVertexShaders++;
			}
		}
	}
}
