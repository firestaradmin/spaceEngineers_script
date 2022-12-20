using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.Network;
using VRage.Plugins;

namespace Sandbox.Engine.Multiplayer
{
	public static class MyReplicationLayerExtensions
	{
		public static void RegisterFromGameAssemblies(this MyReplicationLayerBase layer)
		{
			Assembly[] array = new Assembly[5]
			{
				typeof(MySandboxGame).Assembly,
				typeof(MyRenderProfiler).Assembly,
				MyPlugins.GameAssembly,
				MyPlugins.SandboxAssembly,
				MyPlugins.SandboxGameAssembly
			};
			layer.RegisterFromAssembly(Enumerable.Distinct<Assembly>(Enumerable.Where<Assembly>((IEnumerable<Assembly>)array, (Func<Assembly, bool>)((Assembly s) => s != null))));
		}
	}
}
