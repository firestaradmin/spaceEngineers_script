using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using VRage.FileSystem;
using VRage.Render11.Shader;

namespace VRageRender
{
	public static class MyShaderCompiler
	{
		private class MyIncludeProcessor : Include, ICallbackable, IDisposable
		{
			private readonly string m_basePath;

			public IDisposable Shadow { get; set; }

			internal MyIncludeProcessor(string filepath)
			{
				string basePath = null;
				if (filepath != null)
				{
					basePath = Path.GetDirectoryName(filepath);
				}
				m_basePath = basePath;
			}

			public void Close(Stream stream)
			{
				stream.Close();
			}

			public Stream Open(IncludeType type, string fileName, Stream parentStream)
			{
				string text = fileName;
				if (type == IncludeType.Local)
				{
					string text2 = null;
					FileStream fileStream = parentStream as FileStream;
					if (fileStream != null)
					{
						text2 = Path.GetDirectoryName(fileStream.Name);
					}
					else if (m_basePath != null)
					{
						text2 = m_basePath;
					}
					if (text2 != null)
					{
						text = Path.Combine(text2, fileName);
						if (MyFileSystem.FileExists(text))
						{
							return new FileStream(text, FileMode.Open, FileAccess.Read);
						}
					}
					if (m_basePath != null)
					{
						goto IL_0091;
					}
				}
				for (int num = Includes.Count - 1; num >= 0; num--)
				{
					text = Path.Combine(Includes[num], fileName);
					if (MyFileSystem.FileExists(text))
					{
						return new FileStream(text, FileMode.Open, FileAccess.Read);
					}
				}
				goto IL_0091;
				IL_0091:
				string text3 = "Include not found: " + text;
				MyRender11.Log.WriteLine(text3);
				throw new Exception(text3);
			}

			public void Dispose()
			{
			}
		}

		private static ShaderMacro[] m_globalShaderMacros;

		private const bool DUMP_CODE = false;

		private static string m_shadersPath;

		private static List<string> m_includes;

		[ThreadStatic]
		private static List<ShaderMacro> m_macroList;

		private static ShaderMacro[] GlobalShaderMacros
		{
			get
			{
				if (m_globalShaderMacros == null)
				{
					m_globalShaderMacros = new ShaderMacro[0];
				}
				return m_globalShaderMacros;
			}
		}

		public static string ShadersPath
		{
			get
			{
				if (m_shadersPath == null)
				{
					m_shadersPath = Path.Combine(MyFileSystem.ShadersBasePath, "Shaders");
				}
				return m_shadersPath;
			}
		}

		private static IReadOnlyList<string> Includes
		{
			get
			{
				if (m_includes == null)
				{
					m_includes = new List<string>(new string[1] { ShadersPath });
				}
				return m_includes;
			}
		}

		internal static void Init()
		{
		}

		internal static byte[] Compile(ref MyShaderCompilationInfo info, bool invalidateCache = false)
		{
			string text = Path.Combine(ShadersPath, info.File.ToString());
			if (!File.Exists(text))
			{
				string text2 = "ERROR: Shaders Compile - can not find file: " + text;
				MyRender11.Log.WriteLine(text2);
				Debugger.Break();
				if (Debugger.IsAttached)
				{
					Compile(ref info, invalidateCache: true);
				}
				throw new MyRenderException(text2);
			}
			List<ShaderMacro> list = new List<ShaderMacro>();
			if (info.Macros != null)
			{
				list.AddRange(info.Macros);
			}
			byte[] array = Compile(text, list.ToArray(), info.Profile, info.File.ToString(), invalidateCache);
			if (array == null)
			{
				string text3 = string.Concat("Failed to compile ", info.File, " @ profile ", info.Profile, " with defines ", list.GetString());
				MyRender11.Log.WriteLine(text3);
				if (!Debugger.IsAttached)
				{
					throw new MyRenderException(text3);
				}
				array = Compile(ref info, invalidateCache: true);
			}
			return array;
		}

		internal static byte[] Compile(string filepath, ShaderMacro[] macros, MyShaderProfile profile, string sourceDescriptor, bool invalidateCache)
		{
			bool wasCached;
			string compileLog;
			string hash;
			byte[] array = Compile(filepath, macros, profile, sourceDescriptor, optimize: false, invalidateCache, out wasCached, out compileLog, out hash, savePdb: false, savePreprocessed: false);
			if (!wasCached)
			{
				string msg = string.Concat("WARNING: Shader was not precompiled - ", sourceDescriptor, " @ profile ", profile, " with defines ", macros.GetString(), "(", hash, ")");
				MyRender11.Log.WriteLine(msg);
			}
			if (!string.IsNullOrEmpty(compileLog))
			{
				string arg = sourceDescriptor + " " + ProfileToString(profile) + " " + macros.GetString();
				if (array == null)
				{
					string msg2 = $"Compilation of shader {arg} errors:\n{compileLog}";
					MyRender11.Log.WriteLine(msg2);
					Debugger.Break();
				}
			}
			return array;
		}

		private static void FillGlobalMacros(List<ShaderMacro> macros, bool optimize)
		{
		}

		internal static byte[] Compile(string filepath, ShaderMacro[] macros, MyShaderProfile profile, string sourceDescriptor, bool optimize, bool invalidateCache, out bool wasCached, out string compileLog, out string hash, bool savePdb, bool savePreprocessed)
		{
			hash = "";
			ShaderMacro[] globalShaderMacros = GlobalShaderMacros;
			if (m_macroList == null)
			{
				m_macroList = new List<ShaderMacro>();
			}
			else
			{
				m_macroList.Clear();
			}
			m_macroList.AddRange(globalShaderMacros);
			m_macroList.AddRange(macros);
			FillGlobalMacros(m_macroList, optimize);
			macros = m_macroList.ToArray();
			string entryPoint = ProfileEntryPoint(profile);
			string profile2 = ProfileToString(profile);
			wasCached = false;
			compileLog = null;
			string errors;
			string text = PreprocessShader(filepath, macros, out errors);
			if (text == null)
			{
				compileLog = errors;
				return null;
			}
			if (!invalidateCache && MyShaderCache.TryFetch(text, profile, out var cache, out hash))
			{
				m_macroList?.Clear();
				wasCached = true;
				return cache;
			}
			try
			{
				string text2 = string.Concat(sourceDescriptor, " ", profile, " ", macros.GetString());
				CompilationResult compilationResult = ShaderBytecode.Compile(File.ReadAllText(filepath), entryPoint, profile2, (optimize ? ShaderFlags.OptimizationLevel3 : ShaderFlags.OptimizationLevel1) | ShaderFlags.Debug, EffectFlags.None, macros, new MyIncludeProcessor(filepath), filepath);
				if (compilationResult.Message != null)
				{
					compileLog = compilationResult.Message;
				}
				ShaderBytecode shaderBytecode = compilationResult.Bytecode;
				if (shaderBytecode != null && shaderBytecode.Data.Length != 0)
				{
					if (savePreprocessed)
					{
						File.WriteAllText(MyShaderCache.GetPreprocessedFilepath(MyFileSystem.UserDataPath, "ShaderCachePreprocessed", hash), text);
					}
					if (optimize)
					{
						ShaderBytecode shaderBytecode2 = shaderBytecode.Strip(StripFlags.CompilerStripReflectionData | StripFlags.CompilerStripDebugInformation | StripFlags.CompilerStripTestBlobs | StripFlags.CompilerStripPrivateData);
						shaderBytecode.Dispose();
						shaderBytecode = shaderBytecode2;
					}
					MyShaderCache.Store(text, profile, shaderBytecode.Data);
				}
				return shaderBytecode?.Data;
			}
			catch (Exception ex)
			{
				compileLog = ex.Message;
			}
			finally
			{
				m_macroList?.Clear();
				MyShaderCache.Release(hash);
			}
			return null;
		}

		private static string PreprocessShader(string filepath, ShaderMacro[] macros, out string errors)
		{
			try
			{
				MyIncludeProcessor include = new MyIncludeProcessor(filepath);
				return ShaderBytecode.PreprocessFromFile(filepath, macros, include, out errors);
			}
			catch (Exception ex)
			{
				errors = ex.Message;
				return null;
			}
		}

		public static ShaderMacro[] ConcatenateMacros(ShaderMacro[] sm1, ShaderMacro[] sm2)
		{
			ShaderMacro[] array = new ShaderMacro[sm1.Length + sm2.Length];
			sm1.CopyTo(array, 0);
			sm2.CopyTo(array, sm1.Length);
			return array;
		}

		internal static string ProfileToString(MyShaderProfile val)
		{
<<<<<<< HEAD
			switch (val)
			{
			case MyShaderProfile.vs_5_0:
				return "vs_5_0";
			case MyShaderProfile.ps_5_0:
				return "ps_5_0";
			case MyShaderProfile.gs_5_0:
				return "gs_5_0";
			case MyShaderProfile.cs_5_0:
				return "cs_5_0";
			default:
				throw new Exception();
			}
=======
			return val switch
			{
				MyShaderProfile.vs_5_0 => "vs_5_0", 
				MyShaderProfile.ps_5_0 => "ps_5_0", 
				MyShaderProfile.gs_5_0 => "gs_5_0", 
				MyShaderProfile.cs_5_0 => "cs_5_0", 
				_ => throw new Exception(), 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal static string ProfileEntryPoint(MyShaderProfile val)
		{
<<<<<<< HEAD
			switch (val)
			{
			case MyShaderProfile.vs_5_0:
				return "__vertex_shader";
			case MyShaderProfile.ps_5_0:
				return "__pixel_shader";
			case MyShaderProfile.gs_5_0:
				return "__geometry_shader";
			case MyShaderProfile.cs_5_0:
				return "__compute_shader";
			default:
				throw new Exception();
			}
=======
			return val switch
			{
				MyShaderProfile.vs_5_0 => "__vertex_shader", 
				MyShaderProfile.ps_5_0 => "__pixel_shader", 
				MyShaderProfile.gs_5_0 => "__geometry_shader", 
				MyShaderProfile.cs_5_0 => "__compute_shader", 
				_ => throw new Exception(), 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
