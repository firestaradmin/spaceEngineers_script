using System.Collections.Generic;

namespace VRage.Render11.Scene.Resources
{
	internal interface IMySceneResourceCompiler
	{
		(MyCompiledResourceBundle Batch, bool HasAllResources) Compile(IEnumerable<ResourceInfo> resources);
	}
}
