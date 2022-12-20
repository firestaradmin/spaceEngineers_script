using VRage.Render.Scene;
using VRage.Render11.Culling;
using VRage.Render11.GeometryStage2.Instancing;

namespace VRage.Render11.Emit
{
	internal class MyInstanceDataCollector : CullDataCollector
	{
		private MyInstance m_instance;

		/// <inheritdoc />
		internal override void Init(MyCullResults results, MyChildCullTreeData data)
		{
			base.Init(results, data);
			m_instance = results.Instances[0];
		}

		/// <inheritdoc />
		internal override void Collect(MyCullResultsBase results, bool contained)
		{
			((MyCullResults)results).Instances.Add(m_instance);
		}
	}
}
