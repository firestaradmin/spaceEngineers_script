using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace VRage.Scripting
{
	public static class IVRageScriptingFunctions
	{
		public static Task<Assembly> CompileIngameScriptAsync(this IVRageScripting thiz, string assemblyName, string program, out List<Message> diagnostics, string friendlyName, string typeName, string baseType)
		{
			Script ingameScript = thiz.GetIngameScript(program, typeName, baseType);
			return thiz.CompileAsync(MyApiTarget.Ingame, assemblyName, new Script[1] { ingameScript }, out diagnostics, friendlyName);
		}
	}
}
