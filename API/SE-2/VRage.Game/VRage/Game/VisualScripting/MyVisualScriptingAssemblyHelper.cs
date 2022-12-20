using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using VRage.Scripting;

namespace VRage.Game.VisualScripting
{
	public static class MyVisualScriptingAssemblyHelper
	{
<<<<<<< HEAD
		private static readonly Regex m_assemblyNameCleaner = new Regex("[^A-Za-z_-]*", RegexOptions.Compiled);
=======
		private static readonly Regex m_assemblyNameCleaner = new Regex("[^A-Za-z_-]*", (RegexOptions)8);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public static string MakeAssemblyName(string scenarioName)
		{
			return "VS_" + m_assemblyNameCleaner.Replace(scenarioName, "");
		}

		public static Type GetType(this IVSTAssemblyProvider provider, string typeName)
		{
			return provider.GetAssembly()?.GetType(typeName);
		}

		public static List<IMyLevelScript> GetLevelScriptInstances(this IVSTAssemblyProvider provider, HashSet<string> scriptNames = null)
		{
			Assembly assembly = provider.GetAssembly();
			List<IMyLevelScript> list = new List<IMyLevelScript>();
			if (assembly != null)
			{
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (typeof(IMyLevelScript).IsAssignableFrom(type) && (scriptNames == null || scriptNames.Contains(type.Name)))
					{
						list.Add((IMyLevelScript)Activator.CreateInstance(type));
					}
				}
			}
			return list;
		}
	}
}
