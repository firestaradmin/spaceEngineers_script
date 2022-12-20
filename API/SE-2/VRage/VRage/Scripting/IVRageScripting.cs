using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using VRage.Utils;

namespace VRage.Scripting
{
	public interface IVRageScripting
	{
		bool IsRuntimeCompilationSupported { get; }

		IVSTAssemblyProvider VSTAssemblyProvider { get; }

		void Initialize(Thread updateThread, IEnumerable<string> referencedAssemblies, Type[] referencedTypes, string[] symbols, string diagnosticsPath, bool enableScriptsPDBs);

		IMyWhitelistBatch OpenWhitelistBatch();

		void ClearWhitelist();

		Script GetIngameScript(string code, string className, string inheritance);

		Task<Assembly> CompileAsync(MyApiTarget target, string assemblyName, IEnumerable<Script> scripts, out List<Message> diagnostics, string friendlyName, bool enableDebugInformation = false);

		bool ReportIncorrectBehaviour(MyStringId ruleId);

		IEnumerable<MyTuple<string, MyStringId>> GetWatchdogWarnings();

		IMyIngameScripting GetModAPIScriptingHandle();
	}
}
