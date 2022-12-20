using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SharpDX.Direct3D;
using VRage.FileSystem;
using VRage.Library.Utils;
using VRage.Utils;
using VRageRender;

namespace VRage.Render11.Shader
{
	internal class MyShaderCacheGenerator
	{
		private struct CompileData
		{
			public string Filepath;

			public List<ShaderMacro> Macros;

			public MyShaderProfile Profile;

			public string Descriptor;

			public string VertexLayoutString;

			public float Progress;

			public OnShaderCacheProgressDelegate OnShaderCacheProgress;
		}

		public class IntComparer : IComparer<int>
		{
			public int Compare(int x, int y)
			{
				return -y.CompareTo(x);
			}
		}

		private const string CacheGeneratorFile = "CacheGenerator.xml";

		private const string ANNOTATION_DEFINE = "define";

		private const string ANNOTATION_DEFINE_MANDATORY = "defineMandatory";

		private const string ANNOTATION_SKIP = "@skipCache";

		private static readonly ShaderMacro[] m_globalMacros = new ShaderMacro[3]
		{
			new ShaderMacro("MS_SAMPLE_COUNT", 2),
			new ShaderMacro("MS_SAMPLE_COUNT", 4),
			new ShaderMacro("MS_SAMPLE_COUNT", 8)
		};

		private static readonly List<CompileData> m_dataList = new List<CompileData>();

		private static readonly IntComparer m_intComparer = new IntComparer();

		private static bool m_fastBuild;

		internal static void Generate(bool clean, bool fastBuild, OnShaderCacheProgressDelegate onShaderCacheProgress)
		{
<<<<<<< HEAD
			m_fastBuild = fastBuild;
			Directory.CreateDirectory(Path.Combine(MyFileSystem.UserDataPath, "ShaderCachePdb"));
			Directory.CreateDirectory(Path.Combine(MyFileSystem.UserDataPath, "ShaderCachePreprocessed"));
			string path = Path.Combine(MyFileSystem.UserDataPath, "ShaderCache");
			Directory.CreateDirectory(path);
			if (clean)
			{
				string[] files = Directory.GetFiles(path, "*.cache");
=======
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			m_fastBuild = fastBuild;
			Directory.CreateDirectory(Path.Combine(MyFileSystem.UserDataPath, "ShaderCachePdb"));
			Directory.CreateDirectory(Path.Combine(MyFileSystem.UserDataPath, "ShaderCachePreprocessed"));
			string text = Path.Combine(MyFileSystem.UserDataPath, "ShaderCache");
			Directory.CreateDirectory(text);
			if (clean)
			{
				string[] files = Directory.GetFiles(text, "*.cache");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				for (int i = 0; i < files.Length; i++)
				{
					File.Delete(files[i]);
				}
			}
<<<<<<< HEAD
			string text = null;
			CacheGenerator cacheGenerator = null;
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(CacheGenerator));
				text = Path.Combine(MyShaderCompiler.ShadersPath, "CacheGenerator.xml");
				TextReader textReader = new StreamReader(text);
				cacheGenerator = xmlSerializer.Deserialize(textReader) as CacheGenerator;
			}
			catch (Exception inner)
			{
				throw new FileLoadException("File " + text + " not found or invalid: ", inner);
			}
			if (cacheGenerator == null)
			{
				throw new FileLoadException("File " + text + " not found or invalid: ");
=======
			string text2 = null;
			CacheGenerator cacheGenerator = null;
			try
			{
				XmlSerializer val = new XmlSerializer(typeof(CacheGenerator));
				text2 = Path.Combine(MyShaderCompiler.ShadersPath, "CacheGenerator.xml");
				TextReader textReader = new StreamReader(text2);
				cacheGenerator = val.Deserialize(textReader) as CacheGenerator;
			}
			catch (Exception inner)
			{
				throw new FileLoadException("File " + text2 + " not found or invalid: ", inner);
			}
			if (cacheGenerator == null)
			{
				throw new FileLoadException("File " + text2 + " not found or invalid: ");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			GenerateInternal(cacheGenerator, onShaderCacheProgress);
			GenerateMaterials(cacheGenerator, onShaderCacheProgress);
			ExecuteDataList(0, m_dataList.Count);
		}

		private static bool CheckAnnotation(string source, int idx, string define)
		{
			return source.Substring(idx, define.Length) == define;
		}

		private static void GenerateInternal(CacheGenerator generatorDesc, OnShaderCacheProgressDelegate onShaderCacheProgress)
		{
			string text = Path.Combine(MyShaderCompiler.ShadersPath);
			string[] shadersRecursively = GetShadersRecursively(text, generatorDesc.Ignores);
			List<string[]> list = new List<string[]>();
			List<bool> list2 = new List<bool>();
			for (int i = 0; i < shadersRecursively.Length; i++)
			{
				float progress = (float)i * 100f / (float)shadersRecursively.Length;
				string text2 = shadersRecursively[i];
				string descriptor = text2.Remove(0, text.Length);
<<<<<<< HEAD
				using (StreamReader streamReader = new StreamReader(text2))
				{
					string text3 = streamReader.ReadToEnd();
					bool flag = false;
					if (text3.IndexOf("@skipCache") != -1)
					{
						continue;
					}
					for (MyShaderProfile myShaderProfile = MyShaderProfile.vs_5_0; myShaderProfile < MyShaderProfile.count; myShaderProfile++)
					{
						string value = MyShaderCompiler.ProfileEntryPoint(myShaderProfile);
						if (!text3.Contains(value))
						{
							continue;
						}
						list.Clear();
						list2.Clear();
						int num = 0;
						while (true)
						{
							int num2 = text3.IndexOf('@', num);
							if (num2 == -1)
							{
								break;
							}
							int num3 = -1;
							bool item = false;
							num2++;
							if (CheckAnnotation(text3, num2, "defineMandatory"))
							{
								num3 = "defineMandatory".Length;
								item = true;
							}
							else if (CheckAnnotation(text3, num2, "define"))
							{
								num3 = "define".Length;
							}
							if (num3 == -1)
							{
								num++;
								continue;
							}
							num2 += num3;
							num = text3.IndexOf("\n", num2);
							if (num == -1)
							{
								num = text3.Length;
							}
							string[] item2 = text3.Substring(num2, num - num2).Trim().Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
							list.Add(item2);
							list2.Add(item);
						}
						int[] array = new int[list.Count];
						for (int j = 0; j < list.Count; j++)
						{
							array[j] = (list2[j] ? 1 : 0);
						}
						bool flag2 = false;
						while (!flag2)
						{
							List<ShaderMacro> list3 = new List<ShaderMacro>();
							for (int k = 0; k < list.Count; k++)
							{
								if (array[k] > 0)
								{
									string[] array2 = list[k][array[k] - 1].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
									if (array2.Length > 1)
									{
										list3.Add(new ShaderMacro(array2[0].Trim(), array2[1].Trim()));
									}
									else
									{
										list3.Add(new ShaderMacro(array2[0].Trim(), null));
									}
								}
							}
							PreCompile(text2, list3, myShaderProfile, descriptor, "", progress, onShaderCacheProgress);
							flag = true;
							int num4 = array.Length - 1;
							flag2 = array.Length == 0;
							while (num4 >= 0)
							{
								array[num4]++;
								if (array[num4] <= list[num4].Length)
								{
									break;
								}
								array[num4] = (list2[num4] ? 1 : 0);
								if (num4 == 0)
								{
									flag2 = true;
								}
								num4--;
							}
						}
					}
					if (!flag)
					{
						onShaderCacheProgress?.Invoke(m_dataList.Count, text2, "", "", "", "", "No entry point found.", importantMessage: true);
					}
=======
				using StreamReader streamReader = new StreamReader(text2);
				string text3 = streamReader.ReadToEnd();
				bool flag = false;
				if (text3.IndexOf("@skipCache") != -1)
				{
					continue;
				}
				for (MyShaderProfile myShaderProfile = MyShaderProfile.vs_5_0; myShaderProfile < MyShaderProfile.count; myShaderProfile++)
				{
					string value = MyShaderCompiler.ProfileEntryPoint(myShaderProfile);
					if (!text3.Contains(value))
					{
						continue;
					}
					list.Clear();
					list2.Clear();
					int num = 0;
					while (true)
					{
						int num2 = text3.IndexOf('@', num);
						if (num2 == -1)
						{
							break;
						}
						int num3 = -1;
						bool item = false;
						num2++;
						if (CheckAnnotation(text3, num2, "defineMandatory"))
						{
							num3 = "defineMandatory".Length;
							item = true;
						}
						else if (CheckAnnotation(text3, num2, "define"))
						{
							num3 = "define".Length;
						}
						if (num3 == -1)
						{
							num++;
							continue;
						}
						num2 += num3;
						num = text3.IndexOf("\n", num2);
						if (num == -1)
						{
							num = text3.Length;
						}
						string[] item2 = text3.Substring(num2, num - num2).Trim().Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
						list.Add(item2);
						list2.Add(item);
					}
					int[] array = new int[list.Count];
					for (int j = 0; j < list.Count; j++)
					{
						array[j] = (list2[j] ? 1 : 0);
					}
					bool flag2 = false;
					while (!flag2)
					{
						List<ShaderMacro> list3 = new List<ShaderMacro>();
						for (int k = 0; k < list.Count; k++)
						{
							if (array[k] > 0)
							{
								string[] array2 = list[k][array[k] - 1].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
								if (array2.Length > 1)
								{
									list3.Add(new ShaderMacro(array2[0].Trim(), array2[1].Trim()));
								}
								else
								{
									list3.Add(new ShaderMacro(array2[0].Trim(), null));
								}
							}
						}
						PreCompile(text2, list3, myShaderProfile, descriptor, "", progress, onShaderCacheProgress);
						flag = true;
						int num4 = array.Length - 1;
						flag2 = array.Length == 0;
						while (num4 >= 0)
						{
							array[num4]++;
							if (array[num4] <= list[num4].Length)
							{
								break;
							}
							array[num4] = (list2[num4] ? 1 : 0);
							if (num4 == 0)
							{
								flag2 = true;
							}
							num4--;
						}
					}
				}
				if (!flag)
				{
					onShaderCacheProgress?.Invoke(m_dataList.Count, text2, "", "", "", "", "No entry point found.", importantMessage: true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private static void GenerateMaterials(CacheGenerator generatorDesc, OnShaderCacheProgressDelegate onShaderCacheProgress)
		{
			int num = generatorDesc.Materials.Length * generatorDesc.Passes.Length * generatorDesc.Combos.Length;
			int num2 = 0;
			for (int i = 0; i < generatorDesc.Materials.Length; i++)
			{
				MyStringId orCompute = MyStringId.GetOrCompute(generatorDesc.Materials[i].Id);
				MyShaderUnifiedFlags myShaderUnifiedFlags = ParseFlags(generatorDesc.Materials[i].FlagNames);
				MyShaderUnifiedFlags myShaderUnifiedFlags2 = ParseFlags(generatorDesc.Materials[i].UnsupportedFlagNames);
				string[] array = ParsePasses(generatorDesc.Materials[i].UnsupportedPasses);
				for (int j = 0; j < generatorDesc.Passes.Length; j++)
				{
					string id = generatorDesc.Passes[j].Id;
					if (array != null && array.Contains(id))
					{
						continue;
					}
					MyStringId orCompute2 = MyStringId.GetOrCompute(id);
					MyShaderUnifiedFlags myShaderUnifiedFlags3 = ParseFlags(generatorDesc.Passes[j].FlagNames);
					MyShaderUnifiedFlags myShaderUnifiedFlags4 = ParseFlags(generatorDesc.Passes[j].UnsupportedFlagNames);
					for (int k = 0; k < generatorDesc.Combos.Length; k++)
					{
						num2++;
						float progress = (float)num2 * 100f / (float)num;
						if (string.IsNullOrEmpty(generatorDesc.Combos[k].Material) && string.IsNullOrEmpty(generatorDesc.Combos[k].Pass))
						{
							GenerateCombo(orCompute, orCompute2, myShaderUnifiedFlags | myShaderUnifiedFlags3, myShaderUnifiedFlags2 | myShaderUnifiedFlags4, generatorDesc.Combos[k].ComboList1, generatorDesc.Combos[k].ComboList2, progress, onShaderCacheProgress);
						}
					}
				}
			}
			for (int l = 0; l < generatorDesc.Combos.Length; l++)
			{
				if (!GetMaterialFlags(generatorDesc, generatorDesc.Combos[l].Material, out var additional, out var unsupported, out var unsupportedPasses))
				{
					continue;
				}
				string material = generatorDesc.Combos[l].Material;
				if (string.IsNullOrEmpty(material))
				{
					continue;
				}
				if (!string.IsNullOrEmpty(generatorDesc.Combos[l].Pass))
				{
					num2++;
					float progress2 = (float)num2 * 100f / (float)num;
					MyStringId orCompute3 = MyStringId.GetOrCompute(material);
					MyStringId orCompute4 = MyStringId.GetOrCompute(generatorDesc.Combos[l].Pass);
					GenerateCombo(orCompute3, orCompute4, MyShaderUnifiedFlags.NONE, MyShaderUnifiedFlags.NONE, generatorDesc.Combos[l].ComboList1, generatorDesc.Combos[l].ComboList2, progress2, onShaderCacheProgress);
					continue;
				}
				CacheGenerator.Pass[] passes = generatorDesc.Passes;
				foreach (CacheGenerator.Pass pass in passes)
				{
					if (unsupportedPasses == null || !unsupportedPasses.Contains(pass.Id))
					{
						num2++;
						float progress3 = (float)num2 * 100f / (float)num;
						MyShaderUnifiedFlags myShaderUnifiedFlags5 = ParseFlags(pass.FlagNames);
						MyShaderUnifiedFlags myShaderUnifiedFlags6 = ParseFlags(pass.UnsupportedFlagNames);
						MyStringId orCompute5 = MyStringId.GetOrCompute(material);
						MyStringId orCompute6 = MyStringId.GetOrCompute(pass.Id);
						GenerateCombo(orCompute5, orCompute6, myShaderUnifiedFlags5 | additional, unsupported | myShaderUnifiedFlags6, generatorDesc.Combos[l].ComboList1, generatorDesc.Combos[l].ComboList2, progress3, onShaderCacheProgress);
					}
				}
			}
		}

		private static void GenerateCombo(MyStringId materialId, MyStringId passId, MyShaderUnifiedFlags additionalFlags, MyShaderUnifiedFlags unsupportedFlags, CacheGenerator.Combo[] comboList1, CacheGenerator.Combo[] comboList2, float progress, OnShaderCacheProgressDelegate onShaderCacheProgress)
		{
			if (comboList1 == null || comboList1.Length == 0)
			{
				comboList1 = new CacheGenerator.Combo[1]
				{
					new CacheGenerator.Combo()
				};
			}
			if (comboList2 == null || comboList2.Length == 0)
			{
				comboList2 = new CacheGenerator.Combo[1]
				{
					new CacheGenerator.Combo()
				};
			}
			for (int i = 0; i < comboList1.Length; i++)
			{
				MyVertexInputComponentType[] array = comboList1[i].VertexInput;
				if (array == null)
				{
					array = new MyVertexInputComponentType[0];
				}
				int[] array2 = comboList1[i].VertexInputOrder;
				if (array2 == null || array2.Length != array.Length)
				{
					array2 = new int[array.Length];
					for (int j = 0; j < array.Length; j++)
					{
						array2[j] = j;
					}
				}
				MyShaderUnifiedFlags myShaderUnifiedFlags = ParseFlags(comboList1[i].FlagNames);
				if ((myShaderUnifiedFlags & unsupportedFlags) != 0)
				{
					continue;
				}
				for (int k = 0; k < comboList2.Length; k++)
				{
					MyVertexInputComponentType[] array3 = comboList2[k].VertexInput;
					if (array3 == null)
					{
						array3 = new MyVertexInputComponentType[0];
					}
					int[] array4 = comboList2[k].VertexInputOrder;
					if (array4 == null || array4.Length != array3.Length)
					{
						array4 = new int[array3.Length];
						for (int l = 0; l < array3.Length; l++)
						{
							array4[l] = l;
						}
					}
<<<<<<< HEAD
					MyVertexInputComponentType[] array5 = array.Concat(array3).ToArray();
					Array.Sort(array2.Concat(array4).ToArray(), array5, m_intComparer);
=======
					MyVertexInputComponentType[] array5 = Enumerable.ToArray<MyVertexInputComponentType>(Enumerable.Concat<MyVertexInputComponentType>((IEnumerable<MyVertexInputComponentType>)array, (IEnumerable<MyVertexInputComponentType>)array3));
					Array.Sort(Enumerable.ToArray<int>(Enumerable.Concat<int>((IEnumerable<int>)array2, (IEnumerable<int>)array4)), array5, m_intComparer);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					VertexLayoutId layout = ((array5.Length == 0) ? MyVertexLayouts.Empty : MyVertexLayouts.GetLayout(array5));
					MyShaderUnifiedFlags myShaderUnifiedFlags2 = ParseFlags(comboList2[k].FlagNames) | myShaderUnifiedFlags | additionalFlags;
					List<ShaderMacro> list = new List<ShaderMacro>();
					list.Add(MyMaterialShaders.GetRenderingPassMacro(passId.String));
					MyMaterialShaders.AddMaterialShaderFlagMacrosTo(list, myShaderUnifiedFlags2);
					list.AddRange(layout.Info.Macros);
					MyMaterialShaders.GetMaterialSources(materialId, out var info);
					if ((myShaderUnifiedFlags2 & unsupportedFlags) == 0)
					{
						string @string = layout.Info.Components.GetString();
						string shaderDescriptor = MyMaterialShaders.GetShaderDescriptor(info.VertexShaderFilename, materialId.String, passId.String, layout);
						PreCompile(info.VertexShaderFilepath, list, MyShaderProfile.vs_5_0, shaderDescriptor, @string, progress, onShaderCacheProgress);
						string shaderDescriptor2 = MyMaterialShaders.GetShaderDescriptor(info.PixelShaderFilename, materialId.String, passId.String, layout);
						PreCompile(info.PixelShaderFilepath, list, MyShaderProfile.ps_5_0, shaderDescriptor2, @string, progress, onShaderCacheProgress);
					}
				}
			}
		}

		private static string[] GetShadersRecursively(string shaderPath, string[] ignoresConf)
		{
			List<string> list = new List<string>();
			string[] array = ignoresConf;
			foreach (string path in array)
			{
				string localPath = new Uri(Path.Combine(shaderPath, path)).LocalPath;
				list.Add(localPath);
			}
			List<string> list2 = new List<string>();
			array = PathUtils.GetFilesRecursively(shaderPath, "*.hlsl");
			foreach (string text in array)
			{
				bool flag = false;
				foreach (string item in list)
				{
					if (text.StartsWith(item))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					list2.Add(text);
				}
			}
			return list2.ToArray();
		}

		private static void PreCompile(string filepath, List<ShaderMacro> macros, MyShaderProfile profile, string descriptor, string vertexLayoutString, float progress, OnShaderCacheProgressDelegate onShaderCacheProgress)
		{
			CompileData compileData = default(CompileData);
			compileData.Filepath = filepath;
			compileData.Macros = macros;
			compileData.Profile = profile;
			compileData.Descriptor = descriptor;
			compileData.VertexLayoutString = vertexLayoutString;
			compileData.Progress = progress;
			compileData.OnShaderCacheProgress = onShaderCacheProgress;
			CompileData item = compileData;
			m_dataList.Add(item);
		}

		private static void ExecuteDataList(int from, int to)
		{
<<<<<<< HEAD
			ParallelOptions parallelOptions = new ParallelOptions();
			parallelOptions.MaxDegreeOfParallelism = 8;
			Parallel.For(from, to, parallelOptions, PreCompile);
=======
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			ParallelOptions val = new ParallelOptions();
			val.set_MaxDegreeOfParallelism(8);
			Parallel.For(from, to, val, (Action<int>)PreCompile);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void PreCompile(int index)
		{
			if (index >= m_dataList.Count)
			{
				return;
			}
			CompileData compileData = m_dataList[index];
			ShaderMacro[] macros = compileData.Macros.ToArray();
			MyTimeSpan myTimeSpan = new MyTimeSpan(Stopwatch.GetTimestamp());
			bool wasCached;
			string compileLog;
			string hash;
			byte[] array = MyShaderCompiler.Compile(compileData.Filepath, macros, compileData.Profile, compileData.Descriptor, !m_fastBuild, invalidateCache: false, out wasCached, out compileLog, out hash, savePdb: true, savePreprocessed: true);
			double seconds = (new MyTimeSpan(Stopwatch.GetTimestamp()) - myTimeSpan).Seconds;
			if (seconds > 15.0)
			{
				MyRender11.Log.WriteLine(seconds + "s: " + compileData.Descriptor + "\nVertexLayout: " + compileData.VertexLayoutString + "\nMacros: " + macros.GetString() + "\n");
			}
			if (compileData.OnShaderCacheProgress != null)
			{
				if (wasCached)
				{
					compileData.OnShaderCacheProgress(m_dataList.Count, compileData.Descriptor, hash, MyShaderCompiler.ProfileToString(compileData.Profile), compileData.VertexLayoutString, macros.GetString(), "skipped", importantMessage: false);
				}
				else if (compileLog != null)
				{
					compileData.OnShaderCacheProgress(m_dataList.Count, compileData.Descriptor, hash, MyShaderCompiler.ProfileToString(compileData.Profile), compileData.VertexLayoutString, macros.GetString(), ((array == null) ? "errors:\n" : "warnings:\n") + compileLog, array == null);
				}
				else
				{
					compileData.OnShaderCacheProgress(m_dataList.Count, compileData.Descriptor, hash, MyShaderCompiler.ProfileToString(compileData.Profile), compileData.VertexLayoutString, macros.GetString(), "", importantMessage: false);
				}
			}
		}

		private static bool GetMaterialFlags(CacheGenerator descriptor, string materialId, out MyShaderUnifiedFlags additional, out MyShaderUnifiedFlags unsupported, out string[] unsupportedPasses)
		{
			additional = MyShaderUnifiedFlags.NONE;
			unsupported = MyShaderUnifiedFlags.NONE;
			unsupportedPasses = null;
<<<<<<< HEAD
			CacheGenerator.Material material = descriptor.Materials.FirstOrDefault((CacheGenerator.Material x) => x.Id == materialId);
=======
			CacheGenerator.Material material = Enumerable.FirstOrDefault<CacheGenerator.Material>((IEnumerable<CacheGenerator.Material>)descriptor.Materials, (Func<CacheGenerator.Material, bool>)((CacheGenerator.Material x) => x.Id == materialId));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (material == null)
			{
				return false;
			}
			additional = ParseFlags(material.FlagNames);
			unsupported = ParseFlags(material.UnsupportedFlagNames);
			unsupportedPasses = ParsePasses(material.UnsupportedPasses);
			return true;
		}

		private static MyShaderUnifiedFlags ParseFlags(string flagNames)
		{
			if (string.IsNullOrEmpty(flagNames) || flagNames.Trim() == "")
			{
				return MyShaderUnifiedFlags.NONE;
			}
			MyShaderUnifiedFlags myShaderUnifiedFlags = MyShaderUnifiedFlags.NONE;
			string[] array = flagNames.Split(new char[1] { '|' });
			for (int i = 0; i < array.Length; i++)
			{
				if (Enum.TryParse<MyShaderUnifiedFlags>(array[i], out var result))
				{
					myShaderUnifiedFlags |= result;
				}
				else
				{
					Console.Write("Unknown flag: " + array[i]);
				}
			}
			return myShaderUnifiedFlags;
		}

		private static string[] ParsePasses(string passNames)
		{
			if (string.IsNullOrEmpty(passNames) || passNames.Trim() == "")
			{
				return null;
			}
			return passNames.Split(new char[1] { '|' });
		}
	}
}
