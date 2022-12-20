using System;
using System.Collections.Generic;
using VRage.Render11.Common;
using VRage.Render11.Resources.Textures;

namespace VRage.Render11.Scene.Resources
{
	internal class MySceneResourceCompiler : IMySceneResourceCompiler
	{
		public (MyCompiledResourceBundle, bool) Compile(IEnumerable<ResourceInfo> resources)
		{
			Dictionary<IMyStreamedTextureBase, int> dictionary = new Dictionary<IMyStreamedTextureBase, int>();
			foreach (ResourceInfo resource2 in resources)
			{
				IMySceneResource resource = resource2.Resource;
				IMyStreamedTextureBase myStreamedTextureBase;
				if (resource != null && (myStreamedTextureBase = resource as IMyStreamedTextureBase) != null)
				{
					IMyStreamedTextureBase key = myStreamedTextureBase;
					dictionary[key] = dictionary.GetValueOrDefault(key, 0) + resource2.UseCount;
					continue;
				}
				throw new Exception("Unknown resource " + resource2.Resource);
			}
			(MyTextureStreamingManager.CompiledBatch, bool) tuple = MyManagers.Textures.CompileBatch(dictionary);
			return (new MyCompiledResourceBundle(tuple.Item1), tuple.Item2);
		}
	}
}
