using VRage.Render.Scene;
using VRage.Render11.Culling;
using VRage.Render11.Scene.Components;

namespace VRage.Render11.Emit
{
	internal class MyPointLightDataCollector : CullDataCollector
	{
		private MyLightComponent m_pointLight;

		/// <inheritdoc />
		internal override void Init(MyCullResults results, MyChildCullTreeData data)
		{
			base.Init(results, data);
			m_pointLight = results.PointLights[0];
		}

		/// <inheritdoc />
		internal override void Collect(MyCullResultsBase results, bool contained)
		{
			((MyCullResults)results).PointLights.Add(m_pointLight);
		}
	}
}
