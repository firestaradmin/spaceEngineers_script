using System.Collections.Generic;
using VRage.Render.Scene;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal struct MyShadowmapQuery
	{
		internal IDsvBindable DepthBuffer;

		internal IDsvBindable DepthBufferRo;

		internal MyViewport Viewport;

		internal MyProjectionInfo ProjectionInfo;

		internal Vector3 ProjectionDir;

		internal float ProjectionFactor;

		internal MyViewType ViewType;

		internal int ViewIndex;

		internal HashSet<uint> IgnoredEntities;
	}
}
