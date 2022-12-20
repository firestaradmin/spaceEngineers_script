using VRage.Render.Scene;
using VRage.Render11.Culling;

namespace VRage.Render11.Emit
{
	internal class MyCullProxyDataCollector : CullDataCollector
	{
		private MyCullProxy m_cullProxy;

		/// <inheritdoc />
		internal override void Init(MyCullResults results, MyChildCullTreeData data)
		{
			base.Init(results, data);
			m_cullProxy = results.CullProxies[0];
		}

		/// <inheritdoc />
		internal override void Collect(MyCullResultsBase results, bool contained)
		{
			MyCullResults obj = (MyCullResults)results;
			obj.CullProxies.Add(m_cullProxy);
			obj.CullProxiesContained.Add(contained);
		}
	}
}
