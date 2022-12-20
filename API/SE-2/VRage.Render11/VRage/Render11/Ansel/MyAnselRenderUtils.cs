using System.Collections.Generic;
using VRageRender;

namespace VRage.Render11.Ansel
{
	internal static class MyAnselRenderUtils
	{
		private static readonly List<MyShadowmapQuery> m_emptyShadowmapQueries = new List<MyShadowmapQuery>();

		public static List<MyShadowmapQuery> EmptyShadowmapQueries => m_emptyShadowmapQueries;
	}
}
