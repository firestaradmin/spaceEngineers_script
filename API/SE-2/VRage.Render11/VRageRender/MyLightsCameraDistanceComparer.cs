using System.Collections.Generic;
using VRage.Render11.Scene.Components;

namespace VRageRender
{
	internal class MyLightsCameraDistanceComparer : IComparer<MyLightComponent>
	{
		public int Compare(MyLightComponent x, MyLightComponent y)
		{
			int num = x.ViewerDistanceSquared.CompareTo(y.ViewerDistanceSquared);
			if (num == 0)
			{
				return x.LastSpotShadowIndex.CompareTo(y.LastSpotShadowIndex);
			}
			return num;
		}
	}
}
