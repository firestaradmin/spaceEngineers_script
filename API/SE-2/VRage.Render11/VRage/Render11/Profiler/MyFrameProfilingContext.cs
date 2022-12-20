using System.Collections.Generic;
using VRageRender;

namespace VRage.Render11.Profiler
{
	[PooledObject(2)]
	internal class MyFrameProfilingContext : IPooledObject
	{
		internal readonly Queue<MyIssuedQuery> Issued = new Queue<MyIssuedQuery>(128);

		/// <inheritdoc />
		void IPooledObject.Cleanup()
		{
			Issued.Clear();
		}
	}
}
