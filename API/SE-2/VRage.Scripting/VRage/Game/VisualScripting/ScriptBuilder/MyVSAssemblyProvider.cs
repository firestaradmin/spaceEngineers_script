using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using VRage.FileSystem;
using VRage.Plugins;

namespace VRage.Game.VisualScripting.ScriptBuilder
{
	public static class MyVSAssemblyProvider
	{
		public struct Arguments
		{
			public readonly IEnumerable<string> SourceFiles;

			public readonly string BaseContentPath;

			public readonly Assembly ReferenceProvider;

			public string OutputPath;

			public bool GenerateDummy;

			public bool LoadAssembly;

			public bool ThrowException;

			public Arguments(IEnumerable<string> sourceFiles, string baseContentPath, Assembly referenceProvider)
			{
				SourceFiles = sourceFiles;
				BaseContentPath = baseContentPath;
				ReferenceProvider = referenceProvider;
				OutputPath = null;
				GenerateDummy = false;
				LoadAssembly = true;
				ThrowException = false;
			}
		}

		private static MyVSPreprocessor m_defaultPreprocessor = new MyVSPreprocessor();

		private static MyVSCompiler m_defaultCompiler;

		private static Assembly m_assembly;

		private static readonly Regex m_cleanupSelector = new Regex("[^A-Za-z_-]*");

		public static void Init(IEnumerable<string> fileNames, string localModPath)
		{
			Arguments args = new Arguments(fileNames, localModPath, MyPlugins.GameAssembly);
			Init(in args);
		}

		public static void Init(in Arguments args)
		{
			if (MyVSCompiler.DependencyCollector == null)
			{
				MyVSCompiler.DependencyCollector = new MyDependencyCollector();
				MyVSCompiler.DependencyCollector.CollectReferences(args.ReferenceProvider);
			}
			m_defaultPreprocessor.Clear();
			foreach (string sourceFile in args.SourceFiles)
			{
				m_defaultPreprocessor.AddFile(sourceFile, args.BaseContentPath);
			}
			List<string> list = new List<string>();
			string[] fileSet = m_defaultPreprocessor.FileSet;
			MyVisualScriptBuilder myVisualScriptBuilder = new MyVisualScriptBuilder();
			string[] array = fileSet;
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = (myVisualScriptBuilder.ScriptFilePath = array[i]);
				if (myVisualScriptBuilder.Load() && myVisualScriptBuilder.Build())
				{
					list.Add(Path.Combine(Path.GetTempPath(), myVisualScriptBuilder.ScriptName + ".cs"));
					File.WriteAllText(list[list.Count - 1], myVisualScriptBuilder.Syntax);
				}
			}
			if (args.GenerateDummy)
			{
				GenerateDummy(out var path);
				list.Add(path);
			}
			m_defaultCompiler = new MyVSCompiler((args.OutputPath != null) ? Path.GetFileNameWithoutExtension(args.OutputPath) : "MyVSDefaultAssembly", list)
			{
				ThrowOnFailure = args.ThrowException
			};
			if (fileSet.Length != 0)
			{
				bool flag = m_defaultCompiler.Compile();
				if (flag)
				{
					if (!string.IsNullOrEmpty(args.OutputPath))
					{
						flag &= m_defaultCompiler.WriteAssembly(args.OutputPath);
					}
					if (args.LoadAssembly)
					{
						flag &= m_defaultCompiler.LoadAssembly(args.OutputPath);
					}
				}
			}
			m_assembly = m_defaultCompiler.Assembly;
			foreach (string item in list)
			{
				File.Delete(item);
			}
		}

		public static string MakeAssemblyName(string scenarioName)
		{
			return "VS_" + m_cleanupSelector.Replace(scenarioName, "");
		}

		private static void GenerateDummy(out string path)
		{
<<<<<<< HEAD
			string arg = string.Join("\n", m_defaultPreprocessor.ClassNames.Select((string x) => $"           new {x}();"));
			string contents = $"namespace VisualScripting.CustomScripts\r\n{{\r\n    public class __Dummy\r\n    {{\r\n        public __Dummy()\r\n        {{\r\n{arg}\r\n        }}\r\n    }}\r\n}}";
			path = Path.Combine(Path.GetTempPath(), "__Dummy.cs");
			File.WriteAllText(path, contents);
=======
			string arg = string.Join("\n", Enumerable.Select<string, string>((IEnumerable<string>)m_defaultPreprocessor.ClassNames, (Func<string, string>)((string x) => $"           new {x}();")));
			string text = $"namespace VisualScripting.CustomScripts\r\n{{\r\n    public class __Dummy\r\n    {{\r\n        public __Dummy()\r\n        {{\r\n{arg}\r\n        }}\r\n    }}\r\n}}";
			path = Path.Combine(Path.GetTempPath(), "__Dummy.cs");
			File.WriteAllText(path, text);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static bool TryLoad(string name, bool checkExists)
		{
<<<<<<< HEAD
			if (checkExists && !MyFileSystem.FileExists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name + ".dll")))
=======
			if (checkExists && !MyFileSystem.FileExists(Path.Combine(AppDomain.get_CurrentDomain().get_BaseDirectory(), name + ".dll")))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return false;
			}
			m_assembly = Assembly.Load(name);
			return true;
		}

		public static Type GetType(string typeName)
		{
			if (m_assembly == null)
			{
				return null;
			}
			return m_assembly.GetType(typeName);
		}

		public static Assembly GetAssembly()
		{
			return m_assembly;
		}
	}
}
