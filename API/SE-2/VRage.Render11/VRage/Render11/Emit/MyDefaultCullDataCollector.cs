using VRage.Render.Scene;
using VRage.Render11.Culling;

namespace VRage.Render11.Emit
{
	internal class MyDefaultCullDataCollector : CullDataCollector
	{
		private readonly MyCullResults m_baseResults = new MyCullResults();

		/// <inheritdoc />
		internal override void Init(MyCullResults results, MyChildCullTreeData data)
		{
			base.Init(results, data);
			m_baseResults.Append(results);
		}

		/// <inheritdoc />
		internal override void Collect(MyCullResultsBase results, bool contained)
		{
			((MyCullResults)results).Append(m_baseResults);
		}
	}
}
