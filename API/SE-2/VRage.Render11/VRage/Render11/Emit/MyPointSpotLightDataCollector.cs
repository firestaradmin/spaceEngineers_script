using VRage.Render.Scene;
using VRage.Render11.Culling;
using VRage.Render11.Scene.Components;

namespace VRage.Render11.Emit
{
	internal class MyPointSpotLightDataCollector : CullDataCollector
	{
		private MyLightComponent m_pointLight;

		private MyLightComponent m_spotLight;

		/// <inheritdoc />
		internal override void Init(MyCullResults results, MyChildCullTreeData data)
		{
			base.Init(results, data);
			m_pointLight = results.PointLights[0];
			m_spotLight = results.SpotLights[0];
		}

		/// <inheritdoc />
		internal override void Collect(MyCullResultsBase results, bool contained)
		{
			MyCullResults obj = (MyCullResults)results;
			obj.PointLights.Add(m_pointLight);
			obj.SpotLights.Add(m_spotLight);
		}
	}
}
