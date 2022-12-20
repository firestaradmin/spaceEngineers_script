using VRage.Library.Collections;
using VRage.Render11.GeometryStage2.Rendering;
using VRage.Render11.GeometryStage2.StaticGroup;
using VRageMath;
using VRageRender;

namespace VRage.Render11.GeometryStage2.PrepareGroupPass
{
	[PooledObject(2)]
	internal class MyPrepareGroupPass : IPrepareWork, IPooledObject
	{
		private MyList<MyStaticGroup> m_visibleGroups;

		private MyList<MyRenderData> m_outputRenderData;

		private MatrixD m_viewProj;

		private Matrix m_proj;

		private int m_passId;

		public int PassId => m_passId;

		public void Init(MyList<MyStaticGroup> visibleGroups, MyList<MyRenderData> outputRenderData, MatrixD viewProj, Matrix proj, int passId)
		{
			m_outputRenderData = outputRenderData;
			m_visibleGroups = visibleGroups;
			m_viewProj = viewProj;
			m_proj = proj;
			m_passId = passId;
		}

		public void DoWork()
		{
			m_outputRenderData.Clear();
			foreach (MyStaticGroup visibleGroup in m_visibleGroups)
			{
				m_outputRenderData.Add(visibleGroup.GetRenderData(m_viewProj, m_proj, m_passId));
			}
		}

		public void PostprocessWork()
		{
		}

		/// <inheritdoc />
		void IPooledObject.Cleanup()
		{
		}
	}
}
