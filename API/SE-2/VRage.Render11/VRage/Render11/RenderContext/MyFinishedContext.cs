using SharpDX.Direct3D11;
using VRage.Render11.Profiler;

namespace VRage.Render11.RenderContext
{
	internal struct MyFinishedContext
	{
		public CommandList CommandList;

		public MyFrameProfilingContext ProfilingQueries;

		public MyRenderContextStatistics Statistics;
	}
}
