using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using VRage.Game.VisualScripting.ScriptBuilder;
using VRage.Game.VisualScripting.Utils;
using VRage.Utils;

namespace VRage.Scripting
{
	internal class MyVRageScriptingInternal : IVRageScripting
	{
		private class VSTAssemblyProviderAdapter : IVSTAssemblyProvider
		{
			public bool DebugEnabled
			{
				get
				{
					return MySyntaxFactory.DEBUG_MODE;
				}
				set
				{
					MySyntaxFactory.DEBUG_MODE = value;
				}
			}

			public bool TryLoad(string assemblyName, bool checkFileExists)
			{
				return MyVSAssemblyProvider.TryLoad(assemblyName, checkFileExists);
			}

			public void Init(IEnumerable<string> fileNames, string localModPath)
			{
				MyVSAssemblyProvider.Init(fileNames, localModPath);
			}

			public Assembly GetAssembly()
			{
				return MyVSAssemblyProvider.GetAssembly();
			}
		}

		public bool IsRuntimeCompilationSupported => true;

		public IVSTAssemblyProvider VSTAssemblyProvider { get; } = new VSTAssemblyProviderAdapter();


		public void Initialize(Thread updateThread, IEnumerable<string> referencedAssemblies, Type[] referencedTypes, string[] symbols, string diagnosticsPath, bool enableScriptsPDBs)
		{
			MyModWatchdog.Init(updateThread);
<<<<<<< HEAD
			MyScriptCompiler.Static.AddReferencedAssemblies(referencedAssemblies.ToArray());
=======
			MyScriptCompiler.Static.AddReferencedAssemblies(Enumerable.ToArray<string>(referencedAssemblies));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyScriptCompiler.Static.AddImplicitIngameNamespacesFromTypes(referencedTypes);
			MyScriptCompiler.Static.AddConditionalCompilationSymbols(symbols);
			if (!string.IsNullOrEmpty(diagnosticsPath))
			{
				MyScriptCompiler.Static.DiagnosticOutputPath = diagnosticsPath;
			}
			if (enableScriptsPDBs)
			{
				MyScriptCompiler.Static.EnableDebugInformation = true;
			}
		}

		public IMyWhitelistBatch OpenWhitelistBatch()
		{
			return MyScriptCompiler.Static.Whitelist.OpenBatch();
		}

		public void ClearWhitelist()
		{
			MyScriptCompiler.Static.Whitelist?.Clear();
		}

		public Script GetIngameScript(string code, string className, string inheritance)
		{
			return MyScriptCompiler.Static.GetIngameScript(code, className, inheritance);
		}

		public Task<Assembly> CompileAsync(MyApiTarget target, string assemblyName, IEnumerable<Script> scripts, out List<Message> diagnostics, string friendlyName, bool enableDebugInformation = false)
		{
			diagnostics = new List<Message>();
			return MyScriptCompiler.Static.Compile(target, assemblyName, scripts, diagnostics, friendlyName, enableDebugInformation);
		}

		public bool ReportIncorrectBehaviour(MyStringId ruleId)
		{
			return MyModWatchdog.ReportIncorrectBehaviour(ruleId);
		}

		public IEnumerable<MyTuple<string, MyStringId>> GetWatchdogWarnings()
		{
<<<<<<< HEAD
			return MyModWatchdog.Warnings.Select((KeyValuePair<long, MyTuple<string, MyStringId>> x) => x.Value);
=======
			return Enumerable.Select<KeyValuePair<long, MyTuple<string, MyStringId>>, MyTuple<string, MyStringId>>((IEnumerable<KeyValuePair<long, MyTuple<string, MyStringId>>>)MyModWatchdog.Warnings, (Func<KeyValuePair<long, MyTuple<string, MyStringId>>, MyTuple<string, MyStringId>>)((KeyValuePair<long, MyTuple<string, MyStringId>> x) => x.Value));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public IMyIngameScripting GetModAPIScriptingHandle()
		{
			return MyIngameScripting.Static;
		}
	}
}
