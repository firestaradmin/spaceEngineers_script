using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Emit;
using VRage.Collections;
using VRage.Compiler;
using VRage.FileSystem;
using VRage.Scripting.Analyzers;
using VRage.Scripting.Rewriters;

namespace VRage.Scripting
{
	/// <summary>
	///     Provides a compiler for scripts, with support for a type whitelist and instruction counting.
	/// </summary>
	public class MyScriptCompiler
	{
		/// <summary>
		///     Retrieves the default script compiler.
		/// </summary>
		public static readonly MyScriptCompiler Static = new MyScriptCompiler();

		private readonly List<MetadataReference> m_metadataReferences = new List<MetadataReference>();

		private readonly MyScriptWhitelist m_whitelist;

		private readonly CSharpCompilationOptions m_debugCompilationOptions;

		private readonly CSharpCompilationOptions m_runtimeCompilationOptions;

		private readonly WhitelistDiagnosticAnalyzer m_ingameWhitelistDiagnosticAnalyzer;

		private readonly WhitelistDiagnosticAnalyzer m_modApiWhitelistDiagnosticAnalyzer;

		private readonly HashSet<string> m_assemblyLocations = new HashSet<string>();

		private readonly HashSet<string> m_implicitScriptNamespaces = new HashSet<string>();

		private readonly Dictionary<string, string> m_implicitTypeMappings = new Dictionary<string, string>();

		private readonly HashSet<string> m_ignoredWarnings = new HashSet<string>();

		private readonly HashSet<Type> m_unblockableIngameExceptions = new HashSet<Type>();

		private readonly HashSet<string> m_conditionalCompilationSymbols = new HashSet<string>();

		private readonly CSharpParseOptions m_conditionalParseOptions;

		/// <summary>
		///     Gets the assembly locations to be referenced by the scripts
		/// </summary>
		public HashSetReader<string> AssemblyLocations => m_assemblyLocations;

		/// <summary>
		///     Gets the namespaces that are to be added to the ingame script using list
		/// </summary>
		public HashSetReader<string> ImplicitIngameScriptNamespaces => m_implicitScriptNamespaces;

		public DictionaryReader<string, string> ImplicitTypeMappings => m_implicitTypeMappings;

		/// <summary>
		///     Gets the exception types that are to be made unblockable in ingame scripts
		/// </summary>
		public HashSetReader<Type> UnblockableIngameExceptions => m_unblockableIngameExceptions;

		/// <summary>
		///     Gets the conditional compilation symbols scripts are compiled with.
		/// </summary>
		public HashSetReader<string> ConditionalCompilationSymbols => m_conditionalCompilationSymbols;

		/// <summary>
		///     If this property is set, the compiler will write altered scripts and diagnostics to this
		///     folder.
		/// </summary>
		public string DiagnosticOutputPath { get; set; }

		/// <summary>
		///     Gets the whitelist being used for this compiler.
		/// </summary>
		public MyScriptWhitelist Whitelist => m_whitelist;

		/// <summary>
		///     Contains the diagnostic codes of warnings that should not be reported by the compiler.
		/// </summary>
		public HashSet<string> IgnoredWarnings => m_ignoredWarnings;

		/// <summary>
		///     Determines whether debug information is enabled on a global level. This decision can be made on a per-script
		///     fashion on each of the compile methods, but if this property is set to <c>true</c>, it will override any
		///     parameter value.
		/// </summary>
		public bool EnableDebugInformation { get; set; }

		public MyScriptCompiler()
		{
			AddReferencedAssemblies(GetType().Assembly.Location, typeof(int).Assembly.Location, typeof(XmlEntity).Assembly.Location, typeof(HashSet<>).Assembly.Location, typeof(Dictionary<, >).Assembly.Location, typeof(Uri).Assembly.Location);
			AddImplicitIngameNamespacesFromTypes(typeof(object), typeof(StringBuilder), typeof(IEnumerable), typeof(IEnumerable<>), typeof(Enumerable));
<<<<<<< HEAD
			AddImplicitTypeMappings(typeof(INotifyPropertyChanging), typeof(PropertyChangingEventHandler), typeof(PropertyChangingEventArgs), typeof(INotifyPropertyChanged), typeof(PropertyChangedEventHandler), typeof(PropertyChangedEventArgs));
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			AddUnblockableIngameExceptions(typeof(ScriptOutOfRangeException));
			m_debugCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, reportSuppressedDiagnostics: false, null, null, null, null, OptimizationLevel.Debug, checkOverflow: false, allowUnsafe: false, null, null, default(ImmutableArray<byte>), null, Platform.X64);
			m_runtimeCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, reportSuppressedDiagnostics: false, null, null, null, null, OptimizationLevel.Release, checkOverflow: false, allowUnsafe: false, null, null, default(ImmutableArray<byte>), null, Platform.X64);
			m_whitelist = new MyScriptWhitelist(this);
			m_ingameWhitelistDiagnosticAnalyzer = new WhitelistDiagnosticAnalyzer(m_whitelist, MyWhitelistTarget.Ingame);
			m_modApiWhitelistDiagnosticAnalyzer = new WhitelistDiagnosticAnalyzer(m_whitelist, MyWhitelistTarget.ModApi);
			m_conditionalParseOptions = new CSharpParseOptions(LanguageVersion.CSharp6, DocumentationMode.None);
			IgnoredWarnings.Add("CS0105");
		}

		/// <summary>
		///     Compiles a script.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="assemblyName"></param>
		/// <param name="scripts"></param>
		/// <param name="messages"></param>
		/// <param name="friendlyName"></param>
		/// <param name="enableDebugInformation"></param>
		/// <returns></returns>
		public async Task<Assembly> Compile(MyApiTarget target, string assemblyName, IEnumerable<Script> scripts, List<Message> messages, string friendlyName, bool enableDebugInformation = false)
		{
			if (friendlyName == null)
			{
				friendlyName = "<No Name>";
			}
			DiagnosticAnalyzer whitelistAnalyzer;
			Func<CSharpCompilation, SyntaxTree, SyntaxTree> syntaxTreeInjector;
			switch (target)
			{
			case MyApiTarget.None:
				whitelistAnalyzer = null;
				syntaxTreeInjector = null;
				break;
			case MyApiTarget.Mod:
			{
				int modId = MyModWatchdog.AllocateModId(friendlyName);
				whitelistAnalyzer = m_modApiWhitelistDiagnosticAnalyzer;
				syntaxTreeInjector = (CSharpCompilation c, SyntaxTree st) => InjectMod(c, st, modId);
				break;
			}
			case MyApiTarget.Ingame:
				syntaxTreeInjector = InjectInstructionCounter;
				whitelistAnalyzer = m_ingameWhitelistDiagnosticAnalyzer;
				break;
			default:
				throw new ArgumentOutOfRangeException("target", target, "Invalid compilation target");
			}
			CSharpCompilation compilation = CreateCompilation(assemblyName, scripts, enableDebugInformation);
			await WriteDiagnostics(target, assemblyName, compilation.SyntaxTrees).ConfigureAwait(continueOnCapturedContext: false);
			bool injectionFailed = false;
			CSharpCompilation compilationWithoutInjection = compilation;
			if (syntaxTreeInjector != null)
			{
				SyntaxTree[] newSyntaxTrees = null;
				try
				{
					ImmutableArray<SyntaxTree> syntaxTrees = compilation.SyntaxTrees;
					newSyntaxTrees = ((syntaxTrees.Length != 1) ? (await Task.WhenAll(syntaxTrees.Select((SyntaxTree x) => Task.Run(() => syntaxTreeInjector(compilation, x)))).ConfigureAwait(continueOnCapturedContext: false)) : new SyntaxTree[1] { syntaxTreeInjector(compilation, syntaxTrees[0]) });
				}
				catch
				{
					injectionFailed = true;
				}
				if (!injectionFailed)
				{
					await WriteDiagnostics(target, assemblyName, newSyntaxTrees, ".injected").ConfigureAwait(continueOnCapturedContext: false);
					compilation = compilation.RemoveAllSyntaxTrees().AddSyntaxTrees(newSyntaxTrees);
				}
			}
			CompilationWithAnalyzers analyticCompilation = null;
			if (whitelistAnalyzer != null)
			{
				analyticCompilation = compilation.WithAnalyzers(ImmutableArray.Create(whitelistAnalyzer));
				compilation = (CSharpCompilation)analyticCompilation.Compilation;
			}
			using MemoryStream pdbStream = new MemoryStream();
			using MemoryStream assemblyStream = new MemoryStream();
			bool loadPDBs = false;
			EmitResult emitResult = compilation.Emit(assemblyStream, pdbStream);
			bool success2 = emitResult.Success;
			MyBlacklistSyntaxVisitor myBlacklistSyntaxVisitor = new MyBlacklistSyntaxVisitor();
			ImmutableArray<SyntaxTree>.Enumerator enumerator = compilation.SyntaxTrees.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SyntaxTree current = enumerator.Current;
				myBlacklistSyntaxVisitor.SetSemanticModel(compilation.GetSemanticModel(current));
				myBlacklistSyntaxVisitor.Visit(current.GetRoot());
			}
			if (myBlacklistSyntaxVisitor.HasAnyResult())
			{
				myBlacklistSyntaxVisitor.GetResultMessages(messages);
				return null;
			}
			success2 = await EmitDiagnostics(analyticCompilation, emitResult, messages, success2).ConfigureAwait(continueOnCapturedContext: false);
			await WriteDiagnostics(target, assemblyName, messages, success2).ConfigureAwait(continueOnCapturedContext: false);
			pdbStream.Seek(0L, SeekOrigin.Begin);
			assemblyStream.Seek(0L, SeekOrigin.Begin);
			if (!injectionFailed)
			{
				if (success2)
				{
<<<<<<< HEAD
					bool loadPDBs = false;
					EmitResult emitResult = compilation.Emit(assemblyStream, pdbStream);
					bool success2 = emitResult.Success;
					MyBlacklistSyntaxVisitor myBlacklistSyntaxVisitor = new MyBlacklistSyntaxVisitor();
					ImmutableArray<SyntaxTree>.Enumerator enumerator = compilation.SyntaxTrees.GetEnumerator();
					while (enumerator.MoveNext())
					{
						SyntaxTree current = enumerator.Current;
						myBlacklistSyntaxVisitor.SetSemanticModel(compilation.GetSemanticModel(current));
						myBlacklistSyntaxVisitor.Visit(current.GetRoot());
					}
					if (myBlacklistSyntaxVisitor.HasAnyResult())
					{
						myBlacklistSyntaxVisitor.GetResultMessages(messages);
						return null;
					}
					success2 = await EmitDiagnostics(analyticCompilation, emitResult, messages, success2).ConfigureAwait(continueOnCapturedContext: false);
					await WriteDiagnostics(target, assemblyName, messages, success2).ConfigureAwait(continueOnCapturedContext: false);
					pdbStream.Seek(0L, SeekOrigin.Begin);
					assemblyStream.Seek(0L, SeekOrigin.Begin);
					if (!injectionFailed)
=======
					if (loadPDBs)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						return Assembly.Load(assemblyStream.ToArray(), pdbStream.ToArray());
					}
					return Assembly.Load(assemblyStream.ToArray());
				}
				emitResult = compilationWithoutInjection.Emit(assemblyStream);
				await EmitDiagnostics(analyticCompilation, emitResult, messages, success: false).ConfigureAwait(continueOnCapturedContext: false);
			}
			return null;
		}

		private static string MakeAssemblyName(string name)
		{
			if (name == null)
			{
				return "scripts.dll";
			}
			return Path.GetFileName(name);
		}

		private async Task<bool> EmitDiagnostics(CompilationWithAnalyzers analyticCompilation, EmitResult result, List<Message> messages, bool success)
		{
			messages.Clear();
			if (analyticCompilation != null)
			{
				AnalyzeDiagnostics(await analyticCompilation.GetAllDiagnosticsAsync().ConfigureAwait(continueOnCapturedContext: false), messages, ref success);
			}
			else
			{
				AnalyzeDiagnostics(result.Diagnostics, messages, ref success);
			}
			return success;
		}

		/// <summary>
		///     Injects instruction counter code into the given syntax tree.
		/// </summary>
		/// <param name="compilation"></param>
		/// <param name="tree"></param>
		/// <returns></returns>
		private SyntaxTree InjectInstructionCounter(CSharpCompilation compilation, SyntaxTree tree)
		{
			return new InstructionCountingRewriter(this, compilation, tree).Rewrite();
		}

		/// <summary>
		/// Injects perf counters and ProtoMember tags into Mod's syntax tree
		/// </summary>
		/// <returns></returns>
		private SyntaxTree InjectMod(CSharpCompilation compilation, SyntaxTree syntaxTree, int modId)
		{
			SyntaxTree syntaxTree2 = ProtoTagRewriter.Rewrite(compilation, syntaxTree);
			compilation = compilation.ReplaceSyntaxTree(syntaxTree, syntaxTree2);
			return PerfCountingRewriter.Rewrite(compilation, syntaxTree2, modId);
		}

		/// <summary>
		///     Analyzes the given diagnostics and places errors and warnings in the messages lists.
		/// </summary>
		/// <param name="diagnostics"></param>
		/// <param name="messages"></param>
		/// <param name="success"></param>
		private void AnalyzeDiagnostics(ImmutableArray<Diagnostic> diagnostics, List<Message> messages, ref bool success)
		{
			success = success && !diagnostics.Any((Diagnostic d) => d.Severity == DiagnosticSeverity.Error);
<<<<<<< HEAD
			foreach (Diagnostic item in from d in diagnostics
				where d.Severity >= DiagnosticSeverity.Warning
				orderby d.Severity descending
				select d)
=======
			foreach (Diagnostic item in (IEnumerable<Diagnostic>)Enumerable.OrderByDescending<Diagnostic, DiagnosticSeverity>(diagnostics.Where((Diagnostic d) => d.Severity >= DiagnosticSeverity.Warning), (Func<Diagnostic, DiagnosticSeverity>)((Diagnostic d) => d.Severity)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (item.Severity != DiagnosticSeverity.Warning || (success && !m_ignoredWarnings.Contains(item.Id)))
				{
					FileLinePositionSpan mappedLineSpan = item.Location.GetMappedLineSpan();
					string text = $"{mappedLineSpan.Path}({mappedLineSpan.StartLinePosition.Line + 1},{mappedLineSpan.StartLinePosition.Character}): {item.Severity}: {item.GetMessage()}";
					messages.Add(new Message(item.Severity == DiagnosticSeverity.Error, text));
				}
			}
		}

		private bool GetDiagnosticsOutputPath(MyApiTarget target, string assemblyName, out string outputPath)
		{
			outputPath = DiagnosticOutputPath;
			if (outputPath == null)
			{
				return false;
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			outputPath = Path.Combine(DiagnosticOutputPath, target.ToString(), Path.GetFileNameWithoutExtension(assemblyName));
			return true;
		}

		/// <summary>
		///     If diagnostic output is enabled, this method writes the log of a compilation.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="assemblyName"></param>
		/// <param name="messages"></param>
		/// <param name="success"></param>
		/// <returns></returns>
		private async Task WriteDiagnostics(MyApiTarget target, string assemblyName, IEnumerable<Message> messages, bool success)
		{
			if (!GetDiagnosticsOutputPath(target, assemblyName, out var outputPath))
			{
				return;
<<<<<<< HEAD
			}
			string path = Path.Combine(outputPath, "log.txt");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Success: " + success);
			stringBuilder.AppendLine();
			foreach (Message message in messages)
			{
				stringBuilder.AppendLine(message.ToString());
			}
			using (Stream stream = MyFileSystem.OpenWrite(path))
			{
				StreamWriter writer = new StreamWriter(stream);
				await writer.WriteAsync(stringBuilder.ToString()).ConfigureAwait(continueOnCapturedContext: false);
				await writer.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			string path = Path.Combine(outputPath, "log.txt");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Success: " + success);
			stringBuilder.AppendLine();
			foreach (Message message in messages)
			{
				stringBuilder.AppendLine(message.ToString());
			}
			using Stream stream = MyFileSystem.OpenWrite(path);
			StreamWriter writer = new StreamWriter(stream);
			await writer.WriteAsync(stringBuilder.ToString()).ConfigureAwait(continueOnCapturedContext: false);
			await writer.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
		}

		/// <summary>
		///     If diagnostics is enabled, this method writes
		/// </summary>
		/// <param name="target"></param>
		/// <param name="assemblyName"></param>
		/// <param name="syntaxTrees"></param>
		/// <param name="suffix"></param>
		/// <returns></returns>
		private async Task WriteDiagnostics(MyApiTarget target, string assemblyName, IList<SyntaxTree> syntaxTrees, string suffix = null)
		{
			if (!GetDiagnosticsOutputPath(target, assemblyName, out var outputPath))
			{
				return;
			}
			suffix = suffix ?? "";
			for (int i = 0; i < syntaxTrees.Count; i++)
			{
				SyntaxTree syntaxTree = syntaxTrees[i];
				SyntaxNode node = await syntaxTree.GetRootAsync().ConfigureAwait(continueOnCapturedContext: false);
				string text = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(syntaxTree.FilePath) + suffix + Path.GetExtension(syntaxTree.FilePath));
				if (!text.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
				{
					text += ".cs";
				}
				SyntaxTree normalizedTree = syntaxTree.WithRootAndOptions(node.NormalizeWhitespace(), syntaxTree.Options).WithFilePath(text);
				using (Stream stream = MyFileSystem.OpenWrite(text))
				{
					StreamWriter writer = new StreamWriter(stream);
					await writer.WriteAsync(normalizedTree.ToString()).ConfigureAwait(continueOnCapturedContext: false);
					await writer.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				if (syntaxTrees is Array)
				{
					syntaxTrees[i] = normalizedTree;
				}
			}
		}

		/// <summary>
		///     Creates a script compilation for the given script set.
		/// </summary>
		/// <param name="assemblyFileName"></param>
		/// <param name="scripts"></param>
		/// <param name="enableDebugInformation"></param>                        
		/// <returns></returns>
		internal CSharpCompilation CreateCompilation(string assemblyFileName, IEnumerable<Script> scripts, bool enableDebugInformation)
		{
			CSharpCompilationOptions options = ((enableDebugInformation || EnableDebugInformation) ? m_debugCompilationOptions : m_runtimeCompilationOptions);
			IEnumerable<SyntaxTree> syntaxTrees = null;
			if (scripts != null)
			{
				CSharpParseOptions parseOptions = m_conditionalParseOptions.WithPreprocessorSymbols(ConditionalCompilationSymbols);
				syntaxTrees = Enumerable.Select<Script, SyntaxTree>(scripts, (Func<Script, SyntaxTree>)((Script s) => CSharpSyntaxTree.ParseText(s.Code, parseOptions, s.Name, Encoding.UTF8)));
			}
			return CSharpCompilation.Create(MakeAssemblyName(assemblyFileName), syntaxTrees, m_metadataReferences, options);
		}

		/// <summary>
		///     Adds assemblyLocations to be referenced by scripts.
		/// </summary>
		/// <param name="assemblyLocations"></param>
		public void AddReferencedAssemblies(params string[] assemblyLocations)
		{
			foreach (string text in assemblyLocations)
			{
				if (text == null)
				{
					throw new ArgumentNullException("assemblyLocations");
				}
				if (m_assemblyLocations.Add(text))
				{
					m_metadataReferences.Add(MetadataReference.CreateFromFile(text));
				}
			}
		}

		/// <summary>
		///     Adds the given namespaces for automatic inclusion in the ingame script wrapper.
		///     **This method does NOT whitelist namespaces!
		/// </summary>
		/// <param name="types"></param>
		public void AddImplicitIngameNamespacesFromTypes(params Type[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
				m_implicitScriptNamespaces.Add(types[i].Namespace);
<<<<<<< HEAD
			}
		}

		/// <summary>
		/// Adds a type mapper using for inclusion in the ingame script wrapper.
		/// **This method does NOT whitelist types!
		/// </summary>
		/// <param name="type"></param>
		/// <param name="mappedName">The new name of the type. Leave as null to map to the type name.</param>
		public void AddImplicitTypeMapping(Type type, string mappedName = null)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			mappedName = mappedName ?? type.Name;
			if (string.IsNullOrEmpty(mappedName))
			{
				throw new ArgumentException("Value cannot be null or empty.", "mappedName");
			}
			m_implicitTypeMappings.Add(mappedName, type.FullName);
		}

		/// <summary>
		/// Adds a type mapper using for inclusion in the ingame script wrapper.
		/// **This method does NOT whitelist types!
		/// </summary>
		/// <param name="types"></param>
		public void AddImplicitTypeMappings(params Type[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				AddImplicitTypeMapping(types[i]);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		/// <summary>
		///     Adds the given exceptions to the unblockable list for ingame scripts. These exceptions
		///     will be added to try/catch expressions so they cannot be caught in-game.
		/// </summary>
		/// <param name="types"></param>
		public void AddUnblockableIngameExceptions(params Type[] types)
		{
			foreach (Type type in types)
			{
				if (type == null)
				{
					throw new ArgumentNullException("types");
				}
				if (!typeof(Exception).IsAssignableFrom(type))
				{
					throw new ArgumentException(type.FullName + " is not an exception", "types");
				}
				if (type.IsGenericType || type.IsGenericTypeDefinition)
				{
					throw new ArgumentException("Generic exceptions are not supported", "types");
				}
				m_unblockableIngameExceptions.Add(type);
			}
		}

		/// <summary>
		///     Adds a conditional compilation symbol
		/// </summary>
		/// <param name="symbols"></param>
		public void AddConditionalCompilationSymbols(params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				string text = symbols[i];
				if (text == null)
				{
					throw new ArgumentNullException("symbols");
				}
				if (!(text == string.Empty))
				{
					m_conditionalCompilationSymbols.Add(symbols[i]);
				}
			}
		}

		/// <summary>
		///     Creates a complete code file from an ingame script.
		/// </summary>
		/// <param name="code"></param>
		/// <param name="className"></param>
		/// <param name="inheritance"></param>
		/// <param name="modifiers"></param>
		/// <returns></returns>
		public Script GetIngameScript(string code, string className, string inheritance, string modifiers = "sealed")
		{
			if (string.IsNullOrEmpty(className))
			{
				throw new ArgumentException("Argument is null or empty", "className");
			}
<<<<<<< HEAD
			string text = string.Join(";\nusing ", m_implicitScriptNamespaces);
			string text2 = string.Join(";\nusing ", m_implicitTypeMappings.Select((KeyValuePair<string, string> t) => t.Key + " = " + t.Value));
			modifiers = modifiers ?? "";
			inheritance = (string.IsNullOrEmpty(inheritance) ? "" : (": " + inheritance));
			code = code ?? "";
			string empty = string.Empty;
			empty = "#line 1 \"" + className + "\"\n";
			return new Script(className, "using " + text + ";\nusing " + text2 + ";\npublic " + modifiers + " class " + className + " " + inheritance + "{\n" + empty + code + "\n}\n");
=======
			string text = string.Join(";\nusing ", (IEnumerable<string>)m_implicitScriptNamespaces);
			modifiers = modifiers ?? "";
			inheritance = (string.IsNullOrEmpty(inheritance) ? "" : (": " + inheritance));
			code = code ?? "";
			string text2 = "#line 1 \"{2}\"\n";
			return new Script(className, string.Format("using {0};\npublic {1} class {2} {3}{{\n" + text2 + "{4}\n}}\n", text, modifiers, className, inheritance, code));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
