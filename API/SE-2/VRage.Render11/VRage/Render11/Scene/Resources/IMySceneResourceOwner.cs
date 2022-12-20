using System;
using System.Collections.Generic;

namespace VRage.Render11.Scene.Resources
{
	public interface IMySceneResourceOwner
	{
		event Action OnResourcesChanged;

		IEnumerable<ResourceInfo> GetResources();
	}
}
