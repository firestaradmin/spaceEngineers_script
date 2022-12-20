using VRage.Render11.Resources;

namespace VRage.Render11.RenderContext
{
	internal class MyAllShaderStages
	{
		private MyVertexStage m_vertexStage;

		private MyGeometryStage m_geometryStage;

		private MyPixelStage m_pixelStage;

		private MyComputeStage m_computeStage;

		internal MyAllShaderStages(MyVertexStage vertexStage, MyGeometryStage geometryStage, MyPixelStage pixelStage, MyComputeStage computeStage)
		{
			m_vertexStage = vertexStage;
			m_geometryStage = geometryStage;
			m_pixelStage = pixelStage;
			m_computeStage = computeStage;
		}

		internal void SetConstantBuffer(int slot, IConstantBuffer constantBuffer)
		{
			m_vertexStage.SetConstantBuffer(slot, constantBuffer);
			m_pixelStage.SetConstantBuffer(slot, constantBuffer);
			m_computeStage.SetConstantBuffer(slot, constantBuffer);
		}

		internal void SetSrv(int slot, ISrvBindable srv)
		{
			m_vertexStage.SetSrv(slot, srv);
			m_pixelStage.SetSrv(slot, srv);
			m_computeStage.SetSrv(slot, srv);
		}
	}
}
