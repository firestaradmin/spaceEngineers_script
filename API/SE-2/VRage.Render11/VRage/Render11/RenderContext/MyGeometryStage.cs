using SharpDX.Direct3D11;

namespace VRage.Render11.RenderContext
{
	internal class MyGeometryStage : MyCommonStage
	{
		private GeometryShader m_geometryShader;

		internal override void ClearState()
		{
			base.ClearState();
			m_geometryShader = null;
		}

		internal void Set(GeometryShader shader)
		{
			if (shader != m_geometryShader)
			{
				m_geometryShader = shader;
				m_deviceContext.GeometryShader.Set(shader);
				m_statistics.SetGeometryShaders++;
			}
		}
	}
}
