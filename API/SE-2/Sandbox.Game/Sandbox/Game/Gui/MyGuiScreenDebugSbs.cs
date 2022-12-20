using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "Sbs")]
	[StaticEventOwner]
	internal class MyGuiScreenDebugSbs : MyGuiScreenDebugBase
	{
		private const float TWO_BUTTON_XOFFSET = 0.05f;

		public MyGuiScreenDebugSbs()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Sbs Recreating", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddVerticalSpacing(0.01f * m_scale);
			AddButton("Generate World sbsB5", delegate
			{
				RegenerateWorlds(delete: false);
			});
			AddButton("ReGenerate World sbsB5", delegate
			{
				RegenerateWorlds(delete: true);
			});
			AddVerticalSpacing();
			AddButton("ReGenerate Prefab sbsB5", delegate
			{
				RegeneratePrefabs(delete: true);
			});
		}

		public static void RegenerateWorlds(bool delete)
		{
<<<<<<< HEAD
			HashSet<string> hashSet = new HashSet<string>();
			AddWorldToTask(Path.Combine(MyFileSystem.ContentPath, "InventoryScenes"), hashSet, delete);
			AddWorldToTask(Path.Combine(MyFileSystem.ContentPath, "Scenarios"), hashSet, delete);
			AddWorldToTask(Path.Combine(MyFileSystem.ContentPath, "CustomWorlds"), hashSet, delete);
			Parallel.ForEach(hashSet, delegate(string file)
=======
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			HashSet<string> val = new HashSet<string>();
			AddWorldToTask(Path.Combine(MyFileSystem.ContentPath, "InventoryScenes"), val, delete);
			AddWorldToTask(Path.Combine(MyFileSystem.ContentPath, "Scenarios"), val, delete);
			AddWorldToTask(Path.Combine(MyFileSystem.ContentPath, "CustomWorlds"), val, delete);
			Parallel.ForEach<string>((IEnumerable<string>)val, (Action<string>)delegate(string file)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				ulong fileSize = 0uL;
				string text = file + "B5";
				MyObjectBuilder_Sector objectBuilder = null;
				MyObjectBuilderSerializer.DeserializeXML(file, out objectBuilder, out fileSize);
				MyObjectBuilderSerializer.SerializePB(text, compress: false, objectBuilder);
<<<<<<< HEAD
				MyDebug.WriteLine("File saved - " + text, "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Screens\\DebugScreens\\Game\\MyGuiScreenDebugSbs.cs", 66);
=======
				MyDebug.WriteLine("File saved - " + text, "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Screens\\DebugScreens\\Game\\MyGuiScreenDebugSbs.cs", 66);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			});
		}

		public static void RegeneratePrefabs(bool delete)
		{
<<<<<<< HEAD
			IEnumerable<string> files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Data"), "*.sbc", MySearchOption.AllDirectories);
			int count = 0;
			Parallel.ForEach(files, delegate(string file)
=======
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			IEnumerable<string> files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Data"), "*.sbc", MySearchOption.AllDirectories);
			int count = 0;
			Parallel.ForEach<string>(files, (Action<string>)delegate(string file)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyObjectBuilder_Definitions myObjectBuilder_Definitions = CheckPrefabs(file);
				if (myObjectBuilder_Definitions != null)
				{
					try
					{
						RebuildPrefabs(myObjectBuilder_Definitions);
<<<<<<< HEAD
						MyDebug.WriteLine("File saved - " + file + "B5", "E:\\Repo1\\Sources\\Sandbox.Game\\Game\\Screens\\DebugScreens\\Game\\MyGuiScreenDebugSbs.cs", 83);
=======
						MyDebug.WriteLine("File saved - " + file + "B5", "E:\\Repo3\\Sources\\Sandbox.Game\\Game\\Screens\\DebugScreens\\Game\\MyGuiScreenDebugSbs.cs", 83);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						Interlocked.Increment(ref count);
					}
					catch (Exception)
					{
					}
				}
			});
		}

		private static void AddWorldToTask(string topDirectory, HashSet<string> task, bool delete)
		{
			foreach (string file in MyFileSystem.GetFiles(topDirectory, "*.sbs", MySearchOption.AllDirectories))
			{
				if (file.EndsWith("B5"))
				{
					continue;
				}
<<<<<<< HEAD
				string path = file + "B5";
				if (MyFileSystem.FileExists(path))
				{
					if (delete)
					{
						File.Delete(path);
=======
				string text = file + "B5";
				if (MyFileSystem.FileExists(text))
				{
					if (delete)
					{
						File.Delete(text);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						task.Add(file);
					}
				}
				else
				{
					task.Add(file);
				}
			}
		}

		private static void RebuildPrefabs(MyObjectBuilder_Definitions builder)
		{
			MyObjectBuilder_PrefabDefinition[] prefabs = builder.Prefabs;
			foreach (MyObjectBuilder_PrefabDefinition obj in prefabs)
			{
<<<<<<< HEAD
				string path = obj.PrefabPath + "B5";
				if (MyFileSystem.FileExists(path))
				{
					File.Delete(path);
=======
				string text = obj.PrefabPath + "B5";
				if (MyFileSystem.FileExists(text))
				{
					File.Delete(text);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				MyObjectBuilder_Definitions myObjectBuilder_Definitions = LoadWithProtobuffers<MyObjectBuilder_Definitions>(obj.PrefabPath);
				MyObjectBuilder_PrefabDefinition[] prefabs2 = myObjectBuilder_Definitions.Prefabs;
				foreach (MyObjectBuilder_PrefabDefinition myObjectBuilder_PrefabDefinition in prefabs2)
				{
					if (myObjectBuilder_PrefabDefinition.CubeGrid != null)
					{
						myObjectBuilder_PrefabDefinition.CubeGrids = new MyObjectBuilder_CubeGrid[1] { myObjectBuilder_PrefabDefinition.CubeGrid };
					}
				}
<<<<<<< HEAD
				MyObjectBuilderSerializer.SerializePB(path, compress: false, myObjectBuilder_Definitions);
=======
				MyObjectBuilderSerializer.SerializePB(text, compress: false, myObjectBuilder_Definitions);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private static void CheckXmlForPrefabs(string file, ref List<MyObjectBuilder_PrefabDefinition> prefabs, Stream readStream)
		{
<<<<<<< HEAD
			using (XmlReader xmlReader = XmlReader.Create(readStream))
			{
				while (xmlReader.Read())
				{
					if (!xmlReader.IsStartElement())
					{
						continue;
					}
					if (xmlReader.Name == "SpawnGroups")
					{
						break;
					}
					if (xmlReader.Name == "Prefabs")
					{
						prefabs = new List<MyObjectBuilder_PrefabDefinition>();
						while (xmlReader.ReadToFollowing("Prefab"))
						{
							ReadPrefabHeader(file, ref prefabs, xmlReader);
=======
			XmlReader val = XmlReader.Create(readStream);
			try
			{
				while (val.Read())
				{
					if (!val.IsStartElement())
					{
						continue;
					}
					if (val.get_Name() == "SpawnGroups")
					{
						break;
					}
					if (val.get_Name() == "Prefabs")
					{
						prefabs = new List<MyObjectBuilder_PrefabDefinition>();
						while (val.ReadToFollowing("Prefab"))
						{
							ReadPrefabHeader(file, ref prefabs, val);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						break;
					}
				}
			}
<<<<<<< HEAD
=======
			finally
			{
				((IDisposable)val)?.Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void ReadPrefabHeader(string file, ref List<MyObjectBuilder_PrefabDefinition> prefabs, XmlReader reader)
		{
<<<<<<< HEAD
=======
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Invalid comparison between Unknown and I4
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyObjectBuilder_PrefabDefinition myObjectBuilder_PrefabDefinition = new MyObjectBuilder_PrefabDefinition();
			myObjectBuilder_PrefabDefinition.PrefabPath = file;
			reader.ReadToFollowing("Id");
			bool flag = false;
<<<<<<< HEAD
			if (reader.AttributeCount >= 2)
			{
				for (int i = 0; i < reader.AttributeCount; i++)
				{
					reader.MoveToAttribute(i);
					string name = reader.Name;
=======
			if (reader.get_AttributeCount() >= 2)
			{
				for (int i = 0; i < reader.get_AttributeCount(); i++)
				{
					reader.MoveToAttribute(i);
					string name = reader.get_Name();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (!(name == "Type"))
					{
						if (name == "Subtype")
						{
<<<<<<< HEAD
							myObjectBuilder_PrefabDefinition.Id.SubtypeId = reader.Value;
=======
							myObjectBuilder_PrefabDefinition.Id.SubtypeId = reader.get_Value();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					else
					{
<<<<<<< HEAD
						myObjectBuilder_PrefabDefinition.Id.TypeIdString = reader.Value;
=======
						myObjectBuilder_PrefabDefinition.Id.TypeIdString = reader.get_Value();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						flag = true;
					}
				}
			}
			if (!flag)
			{
				while (reader.Read())
				{
					if (reader.IsStartElement())
					{
<<<<<<< HEAD
						string name = reader.Name;
=======
						string name = reader.get_Name();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (!(name == "TypeId"))
						{
							if (name == "SubtypeId")
							{
								reader.Read();
<<<<<<< HEAD
								myObjectBuilder_PrefabDefinition.Id.SubtypeId = reader.Value;
=======
								myObjectBuilder_PrefabDefinition.Id.SubtypeId = reader.get_Value();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
						}
						else
						{
							reader.Read();
<<<<<<< HEAD
							myObjectBuilder_PrefabDefinition.Id.TypeIdString = reader.Value;
						}
					}
					else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Id")
=======
							myObjectBuilder_PrefabDefinition.Id.TypeIdString = reader.get_Value();
						}
					}
					else if ((int)reader.get_NodeType() == 15 && reader.get_Name() == "Id")
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						break;
					}
				}
			}
			prefabs.Add(myObjectBuilder_PrefabDefinition);
		}

		private static MyObjectBuilder_Definitions CheckPrefabs(string file)
		{
			List<MyObjectBuilder_PrefabDefinition> prefabs = null;
			using (Stream stream = MyFileSystem.OpenRead(file))
			{
				if (stream != null)
				{
<<<<<<< HEAD
					using (Stream stream2 = stream.UnwrapGZip())
					{
						if (stream2 != null)
						{
							CheckXmlForPrefabs(file, ref prefabs, stream2);
						}
=======
					using Stream stream2 = stream.UnwrapGZip();
					if (stream2 != null)
					{
						CheckXmlForPrefabs(file, ref prefabs, stream2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			MyObjectBuilder_Definitions myObjectBuilder_Definitions = null;
			if (prefabs != null)
			{
				myObjectBuilder_Definitions = new MyObjectBuilder_Definitions();
				myObjectBuilder_Definitions.Prefabs = prefabs.ToArray();
			}
			return myObjectBuilder_Definitions;
		}

		private static T LoadWithProtobuffers<T>(string path) where T : MyObjectBuilder_Base
		{
			T objectBuilder = null;
			MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder);
			return objectBuilder;
		}
	}
}
