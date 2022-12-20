using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.ModAPI;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity.UseObject;
using VRage.Game.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Scripting;
using VRage.Utils;

namespace Sandbox.Game.World
{
	public class MyScriptManager
	{
		public static MyScriptManager Static;

		public readonly Dictionary<MyModContext, HashSet<MyStringId>> ScriptsPerMod = new Dictionary<MyModContext, HashSet<MyStringId>>();

		public Dictionary<MyStringId, Assembly> Scripts = new Dictionary<MyStringId, Assembly>(MyStringId.Comparer);

		public Dictionary<Type, HashSet<Type>> EntityScripts = new Dictionary<Type, HashSet<Type>>();

		public Dictionary<Tuple<Type, string>, HashSet<Type>> SubEntityScripts = new Dictionary<Tuple<Type, string>, HashSet<Type>>();

		public Dictionary<string, Type> StatScripts = new Dictionary<string, Type>();

		public Dictionary<MyStringId, Type> InGameScripts = new Dictionary<MyStringId, Type>(MyStringId.Comparer);

		public Dictionary<MyStringId, StringBuilder> InGameScriptsCode = new Dictionary<MyStringId, StringBuilder>(MyStringId.Comparer);

		public Dictionary<Type, MyModContext> TypeToModMap = new Dictionary<Type, MyModContext>();

<<<<<<< HEAD
		private List<string> m_errors = new List<string>();

		private List<string> m_cachedFiles = new List<string>();

		/// <summary>
		/// Usings that will be added to scripts before compilation. Those usings will replace default ones.
		/// </summary>
=======
		private List<string> m_cachedFiles = new List<string>();

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static string CompatibilityUsings = "using VRage;\r\nusing VRage.Game.Components;\r\nusing VRage.ObjectBuilders;\r\nusing VRage.ModAPI;\r\nusing VRage.Game.ModAPI;\r\nusing Sandbox.Common.ObjectBuilders;\r\nusing VRage.Game;\r\nusing Sandbox.ModAPI;\r\nusing VRage.Game.ModAPI.Interfaces;\r\nusing SpaceEngineers.Game.ModAPI;\r\n#line 1\r\n";

		private static Dictionary<string, string> m_compatibilityChanges = new Dictionary<string, string>
		{
			{ "using VRage.Common.Voxels;", "" },
			{ "VRage.Common.Voxels.", "" },
			{ "Sandbox.ModAPI.IMyEntity", "VRage.ModAPI.IMyEntity" },
			{ "Sandbox.Common.ObjectBuilders.MyObjectBuilder_EntityBase", "VRage.ObjectBuilders.MyObjectBuilder_EntityBase" },
			{ "Sandbox.Common.MyEntityUpdateEnum", "VRage.ModAPI.MyEntityUpdateEnum" },
			{ "using Sandbox.Common.ObjectBuilders.Serializer;", "" },
			{ "Sandbox.Common.ObjectBuilders.Serializer.", "" },
			{ "Sandbox.Common.MyMath", "VRageMath.MyMath" },
			{ "Sandbox.Common.ObjectBuilders.VRageData.SerializableVector3I", "VRage.SerializableVector3I" },
			{ "VRage.Components", "VRage.Game.Components" },
			{ "using Sandbox.Common.ObjectBuilders.VRageData;", "" },
			{ "Sandbox.Common.ObjectBuilders.MyOnlineModeEnum", "VRage.Game.MyOnlineModeEnum" },
			{ "Sandbox.Common.ObjectBuilders.Definitions.MyDamageType", "VRage.Game.MyDamageType" },
			{ "Sandbox.Common.ObjectBuilders.VRageData.SerializableBlockOrientation", "VRage.Game.SerializableBlockOrientation" },
			{ "Sandbox.Common.MySessionComponentDescriptor", "VRage.Game.Components.MySessionComponentDescriptor" },
			{ "Sandbox.Common.MyUpdateOrder", "VRage.Game.Components.MyUpdateOrder" },
			{ "Sandbox.Common.MySessionComponentBase", "VRage.Game.Components.MySessionComponentBase" },
			{ "Sandbox.Common.MyFontEnum", "VRage.Game.MyFontEnum" },
			{ "Sandbox.Common.MyRelationsBetweenPlayerAndBlock", "VRage.Game.MyRelationsBetweenPlayerAndBlock" },
			{ "Sandbox.Common.Components", "VRage.Game.Components" },
			{ "using Sandbox.Common.Input;", "" },
			{ "using Sandbox.Common.ModAPI;", "" }
		};

		private Dictionary<string, string> m_scriptsToSave = new Dictionary<string, string>();

		public void LoadData()
		{
			MySandboxGame.Log.WriteLine("MyScriptManager.LoadData() - START");
			MySandboxGame.Log.IncreaseIndent();
			Static = this;
			Scripts.Clear();
			EntityScripts.Clear();
			SubEntityScripts.Clear();
			TryAddEntityScripts(MyModContext.BaseGame, MyPlugins.SandboxAssembly);
			TryAddEntityScripts(MyModContext.BaseGame, MyPlugins.SandboxGameAssembly);
			if (MySession.Static.CurrentPath != null)
			{
				LoadScripts(MySession.Static.CurrentPath, MyModContext.BaseGame);
			}
			if (MySession.Static.Mods != null)
			{
				bool isServer = Sync.IsServer;
				foreach (MyObjectBuilder_Checkpoint.ModItem mod in MySession.Static.Mods)
				{
					bool flag = false;
					if (mod.IsModData())
					{
						ListReader<string> tags = mod.GetModData().Tags;
<<<<<<< HEAD
						if (tags.Contains(MySteamConstants.TAG_SERVER_SCRIPTS) && !isServer)
						{
							continue;
						}
						flag = tags.Contains(MySteamConstants.TAG_NO_SCRIPTS);
					}
					MyModContext myModContext = (MyModContext)mod.GetModContext();
=======
						if (Enumerable.Contains<string>((IEnumerable<string>)tags, MySteamConstants.TAG_SERVER_SCRIPTS) && !isServer)
						{
							continue;
						}
						flag = Enumerable.Contains<string>((IEnumerable<string>)tags, MySteamConstants.TAG_NO_SCRIPTS);
					}
					MyModContext myModContext = new MyModContext();
					myModContext.Init(mod);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					try
					{
						LoadScripts(mod.GetPath(), myModContext);
					}
					catch (MyLoadingRuntimeCompilationNotSupportedException)
					{
						if (flag)
						{
							MyVRage.Platform.Scripting.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_RuntimeScripts);
							continue;
						}
						throw;
					}
					catch (Exception ex2)
					{
						MyLog.Default.WriteLine($"Fatal error compiling {myModContext.ModServiceName}:{myModContext.ModId} - {myModContext.ModName}. This item is likely not a mod and should be removed from the mod list.");
						MyLog.Default.WriteLine(ex2);
						throw;
					}
				}
			}
			foreach (Assembly value in Scripts.Values)
			{
				if (MyFakes.ENABLE_TYPES_FROM_MODS)
				{
					MyGlobalTypeMetadata.Static.RegisterAssembly(value);
				}
				MySandboxGame.Log.WriteLine($"Script loaded: {value.FullName}");
			}
			MyTextSurfaceScriptFactory.LoadScripts();
			MyUseObjectFactory.RegisterAssemblyTypes(Scripts.Values.ToArray());
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("MyScriptManager.LoadData() - END");
		}

		private void LoadScripts(string path, MyModContext mod = null)
		{
			if (!MyFakes.ENABLE_SCRIPTS)
			{
				return;
			}
			string text = Path.Combine(path, "Data", "Scripts");
			string[] array;
			try
			{
<<<<<<< HEAD
				array = MyFileSystem.GetFiles(text, "*.cs").ToArray();
=======
				array = Enumerable.ToArray<string>(MyFileSystem.GetFiles(text, "*.cs"));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (Exception)
			{
				MySandboxGame.Log.WriteLine("Failed to load scripts from: " + path);
				return;
			}
			if (array.Length == 0)
			{
				return;
			}
			if (!MyVRage.Platform.Scripting.IsRuntimeCompilationSupported)
			{
				throw new MyLoadingRuntimeCompilationNotSupportedException();
			}
			bool zipped = MyZipFileProvider.IsZipFile(path);
<<<<<<< HEAD
			string[] array2 = array.First().Split(new char[1] { '\\' });
			int num = text.Split(new char[1] { '\\' }).Length;
			if (num >= array2.Length)
			{
				MySandboxGame.Log.WriteLine(string.Format("\nWARNING: Mod \"{0}\" contains misplaced .cs files ({2}). Scripts are supposed to be at {1}.\n", path, text, array.First()));
=======
			string[] array2 = Enumerable.First<string>((IEnumerable<string>)array).Split(new char[1] { '\\' });
			int num = text.Split(new char[1] { '\\' }).Length;
			if (num >= array2.Length)
			{
				MySandboxGame.Log.WriteLine(string.Format("\nWARNING: Mod \"{0}\" contains misplaced .cs files ({2}). Scripts are supposed to be at {1}.\n", path, text, Enumerable.First<string>((IEnumerable<string>)array)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return;
			}
			List<string> list = new List<string>();
			string text2 = array2[num];
			string[] array3 = array;
			foreach (string text3 in array3)
			{
				array2 = text3.Split(new char[1] { '\\' });
<<<<<<< HEAD
				if (!(array2[array2.Length - 1].Split(new char[1] { '.' }).Last() != "cs"))
=======
				if (!(Enumerable.Last<string>((IEnumerable<string>)array2[array2.Length - 1].Split(new char[1] { '.' })) != "cs"))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					int num2 = Array.IndexOf(array2, "Scripts") + 1;
					if (array2[num2] == text2)
					{
						list.Add(text3);
						continue;
					}
					Compile(list, GetAssemblyName(mod, text2), zipped, mod);
					list.Clear();
					text2 = array2[num];
					list.Add(text3);
				}
			}
			Compile(list.ToArray(), Path.Combine(MyFileSystem.ModsPath, GetAssemblyName(mod, text2)), zipped, mod);
			list.Clear();
		}

		private string GetAssemblyName(MyModContext mod, string scriptDir)
		{
			string text = mod?.ModId + "_" + scriptDir;
			if (mod?.ModServiceName.ToLower() != "steam")
			{
				text = mod?.ModServiceName + "_" + text;
			}
			return text;
		}

		private void Compile(IEnumerable<string> scriptFiles, string assemblyName, bool zipped, MyModContext context)
		{
			if (zipped)
			{
				string text = Path.Combine(Path.GetTempPath(), MyPerGameSettings.BasicGameInfo.GameNameSafe, Path.GetFileName(assemblyName));
				if (Directory.Exists(text))
				{
					Directory.Delete(text, true);
				}
				foreach (string scriptFile in scriptFiles)
				{
					try
					{
						string text2 = Path.Combine(text, Path.GetFileName(scriptFile));
						using (StreamReader streamReader = new StreamReader(MyFileSystem.OpenRead(scriptFile)))
						{
							using StreamWriter streamWriter = new StreamWriter(MyFileSystem.OpenWrite(text2));
							streamWriter.Write(streamReader.ReadToEnd());
						}
						m_cachedFiles.Add(text2);
					}
					catch (Exception ex)
					{
						MySandboxGame.Log.WriteLine(ex);
						MyDefinitionErrors.Add(context, $"Cannot load {Path.GetFileName(scriptFile)}", TErrorSeverity.Error);
						MyDefinitionErrors.Add(context, ex.Message, TErrorSeverity.Error);
					}
				}
				scriptFiles = m_cachedFiles;
			}
			List<Message> diagnostics;
<<<<<<< HEAD
			Assembly result = MyVRage.Platform.Scripting.CompileAsync(MyApiTarget.Mod, assemblyName, scriptFiles.Select((string file) => new Script(file, UpdateCompatibility(file))), out diagnostics, context.ModName).Result;
=======
			Assembly result = MyVRage.Platform.Scripting.CompileAsync(MyApiTarget.Mod, assemblyName, Enumerable.Select<string, Script>(scriptFiles, (Func<string, Script>)((string file) => new Script(file, UpdateCompatibility(file)))), out diagnostics, context.ModName).Result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (result != null)
			{
				AddAssembly(context, MyStringId.GetOrCompute(assemblyName), result);
			}
			else
			{
				MyDefinitionErrors.Add(context, $"Compilation of {assemblyName} failed:", TErrorSeverity.Error);
				MySandboxGame.Log.IncreaseIndent();
				foreach (Message item in diagnostics)
				{
					MyDefinitionErrors.Add(context, item.Text, (!item.IsError) ? TErrorSeverity.Warning : TErrorSeverity.Error);
					_ = item.IsError;
				}
				MySandboxGame.Log.DecreaseIndent();
				m_errors.Clear();
			}
			m_cachedFiles.Clear();
		}

		public static string UpdateCompatibility(string filename)
		{
			using (Stream stream = MyFileSystem.OpenRead(filename))
			{
				if (stream != null)
				{
					using (StreamReader streamReader = new StreamReader(stream))
					{
						string text = streamReader.ReadToEnd();
						text = text.Insert(0, CompatibilityUsings);
						foreach (KeyValuePair<string, string> compatibilityChange in m_compatibilityChanges)
						{
							text = text.Replace(compatibilityChange.Key, compatibilityChange.Value);
						}
						return text;
					}
				}
			}
			return null;
		}

		private void AddAssembly(MyModContext context, MyStringId myStringId, Assembly assembly)
		{
			if (Scripts.ContainsKey(myStringId))
			{
				MySandboxGame.Log.WriteLine($"Script already in list {myStringId.ToString()}");
				return;
			}
			if (!ScriptsPerMod.TryGetValue(context, out var value))
			{
				value = new HashSet<MyStringId>();
				ScriptsPerMod.Add(context, value);
			}
			value.Add(myStringId);
			Scripts.Add(myStringId, assembly);
			Type[] types = assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				MyConsole.AddCommand(new MyCommandScript(types[i]));
			}
			TryAddEntityScripts(context, assembly);
			AddStatScripts(assembly);
		}

		private void TryAddEntityScripts(MyModContext context, Assembly assembly)
		{
			Type typeFromHandle = typeof(MyGameLogicComponent);
			Type typeFromHandle2 = typeof(MyObjectBuilder_Base);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(MyEntityComponentDescriptor), inherit: false);
				if (customAttributes == null || customAttributes.Length == 0)
				{
					continue;
				}
<<<<<<< HEAD
				TypeToModMap.Add(type, context);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyEntityComponentDescriptor myEntityComponentDescriptor = (MyEntityComponentDescriptor)customAttributes[0];
				try
				{
					if (!myEntityComponentDescriptor.EntityUpdate.HasValue)
					{
						MyDefinitionErrors.Add(context, "**WARNING!**\r\nScript for " + myEntityComponentDescriptor.EntityBuilderType.Name + " is using the obsolete MyEntityComponentDescriptor overload!\r\nYou must use the 3 parameter overload to properly register script updates!\r\nThis script will most likely not work as intended!\r\n**WARNING!**", TErrorSeverity.Warning);
					}
					if (myEntityComponentDescriptor.EntityBuilderSubTypeNames != null && myEntityComponentDescriptor.EntityBuilderSubTypeNames.Length != 0)
					{
						string[] entityBuilderSubTypeNames = myEntityComponentDescriptor.EntityBuilderSubTypeNames;
						foreach (string item in entityBuilderSubTypeNames)
						{
							if (typeFromHandle.IsAssignableFrom(type) && typeFromHandle2.IsAssignableFrom(myEntityComponentDescriptor.EntityBuilderType))
							{
								Tuple<Type, string> key = new Tuple<Type, string>(myEntityComponentDescriptor.EntityBuilderType, item);
								if (!SubEntityScripts.TryGetValue(key, out var value))
								{
									value = new HashSet<Type>();
									SubEntityScripts.Add(key, value);
								}
								else
								{
									MyDefinitionErrors.Add(context, "Possible entity type script logic collision", TErrorSeverity.Notice);
								}
								value.Add(type);
							}
						}
					}
					else if (typeFromHandle.IsAssignableFrom(type) && typeFromHandle2.IsAssignableFrom(myEntityComponentDescriptor.EntityBuilderType))
					{
						if (!EntityScripts.TryGetValue(myEntityComponentDescriptor.EntityBuilderType, out var value2))
						{
							value2 = new HashSet<Type>();
							EntityScripts.Add(myEntityComponentDescriptor.EntityBuilderType, value2);
						}
						else
						{
							MyDefinitionErrors.Add(context, "Possible entity type script logic collision", TErrorSeverity.Notice);
						}
						value2.Add(type);
					}
				}
<<<<<<< HEAD
				catch (Exception ex)
				{
					MySandboxGame.Log.WriteLine("Exception during loading of type : " + type.Name);
					MySandboxGame.Log.WriteLine(ex);
=======
				catch (Exception)
				{
					MySandboxGame.Log.WriteLine("Exception during loading of type : " + type.Name);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private void AddStatScripts(Assembly assembly)
		{
			Type typeFromHandle = typeof(MyStatLogic);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(MyStatLogicDescriptor), inherit: false);
				if (customAttributes != null && customAttributes.Length != 0)
				{
					string componentName = ((MyStatLogicDescriptor)customAttributes[0]).ComponentName;
					if (typeFromHandle.IsAssignableFrom(type) && !StatScripts.ContainsKey(componentName))
					{
						StatScripts.Add(componentName, type);
					}
				}
			}
		}

		protected void UnloadData()
		{
			Scripts.Clear();
			InGameScripts.Clear();
			InGameScriptsCode.Clear();
			EntityScripts.Clear();
			m_scriptsToSave.Clear();
			MyTextSurfaceScriptFactory.UnloadScripts();
		}

		public void SaveData()
		{
			WriteScripts(MySession.Static.CurrentPath);
		}

		private void ReadScripts(string path)
		{
			string text = Path.Combine(path, "Data", "Scripts");
			IEnumerable<string> files = MyFileSystem.GetFiles(text, "*.cs");
			try
			{
				if (Enumerable.Count<string>(files) == 0)
				{
					return;
				}
			}
			catch (Exception)
			{
				MySandboxGame.Log.WriteLine($"Failed to load scripts from: {path}");
				return;
			}
			foreach (string item in files)
			{
				try
				{
					using StreamReader streamReader = new StreamReader(MyFileSystem.OpenRead(item));
					m_scriptsToSave.Add(item.Substring(text.Length + 1), streamReader.ReadToEnd());
				}
				catch (Exception ex2)
				{
					MySandboxGame.Log.WriteLine(ex2);
				}
			}
		}

		private void WriteScripts(string path)
		{
			try
			{
				string arg = Path.Combine(path, "Data", "Scripts");
				foreach (KeyValuePair<string, string> item in m_scriptsToSave)
				{
					using StreamWriter streamWriter = new StreamWriter(MyFileSystem.OpenWrite($"{arg}\\{item.Key}"));
					streamWriter.Write(item.Value);
				}
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine(ex);
			}
		}

		public void Init(MyObjectBuilder_ScriptManager scriptBuilder)
		{
			if (scriptBuilder != null)
			{
				MyAPIUtilities.Static.Variables = scriptBuilder.variables.Dictionary;
			}
			LoadData();
		}

		public MyObjectBuilder_ScriptManager GetObjectBuilder()
		{
			return new MyObjectBuilder_ScriptManager
			{
				variables = 
				{
					Dictionary = MyAPIUtilities.Static.Variables
				}
			};
		}

		public Type GetScriptType(MyModContext context, string qualifiedTypeName)
		{
<<<<<<< HEAD
=======
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!ScriptsPerMod.TryGetValue(context, out var value))
			{
				return null;
			}
			Enumerator<MyStringId> enumerator = value.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyStringId current = enumerator.get_Current();
					Type type = Scripts[current].GetType(qualifiedTypeName);
					if (type != null)
					{
						return type;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return null;
		}
	}
}
