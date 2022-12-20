using VRage.Render.Scene;
using VRage.Render11.Culling;

namespace VRage.Render11.Emit
{
	internal abstract class CullDataCollector : MyChildCullTreeData
	{
		protected CullDataCollector()
		{
			Add = Collect;
		}

		internal virtual void Init(MyCullResults results, MyChildCullTreeData data)
		{
			Remove = data.Remove;
			DebugColor = data.DebugColor;
			FarCull = data.FarCull;
		}

		internal abstract void Collect(MyCullResultsBase results, bool contained);
	}
}
