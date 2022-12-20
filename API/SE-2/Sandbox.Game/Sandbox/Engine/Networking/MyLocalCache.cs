using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
<<<<<<< HEAD
=======
using System.Xml.Linq;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Definitions;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GUI;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.GameServices;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Engine.Networking
{
	public class MyLocalCache
	{
		private class SameWorldComparer : EqualityComparer<Tuple<string, MyWorldInfo>>
		{
			private static SameWorldComparer m_static;

			public static SameWorldComparer Static
			{
				get
				{
					if (m_static == null)
					{
						m_static = new SameWorldComparer();
					}
					return m_static;
				}
			}

			public override bool Equals(Tuple<string, MyWorldInfo> t1, Tuple<string, MyWorldInfo> t2)
			{
				return Path.GetFileName(t1.Item1).Equals(Path.GetFileName(t2.Item1));
			}
<<<<<<< HEAD
=======

			public override int GetHashCode(Tuple<string, MyWorldInfo> t)
			{
				return Path.GetFileName(t.Item1).GetHashCode();
			}
		}

		private class SameCloudWorldComparer : EqualityComparer<Tuple<string, MyWorldInfo>>
		{
			private static SameCloudWorldComparer m_static;

			public static SameCloudWorldComparer Static
			{
				get
				{
					if (m_static == null)
					{
						m_static = new SameCloudWorldComparer();
					}
					return m_static;
				}
			}

			public override bool Equals(Tuple<string, MyWorldInfo> t1, Tuple<string, MyWorldInfo> t2)
			{
				return t1.Item1.Equals(t2.Item1);
			}

			public override int GetHashCode(Tuple<string, MyWorldInfo> t)
			{
				return Path.GetFileName(t.Item1).GetHashCode();
			}
		}

		public const string CHECKPOINT_FILE = "Sandbox.sbc";

		private const string WORLD_CONFIGURATION_FILE = "Sandbox_config.sbc";
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

			public override int GetHashCode(Tuple<string, MyWorldInfo> t)
			{
				return Path.GetFileName(t.Item1).GetHashCode();
			}
		}

		private class SameCloudWorldComparer : EqualityComparer<Tuple<string, MyWorldInfo>>
		{
			private static SameCloudWorldComparer m_static;

			public static SameCloudWorldComparer Static
			{
				get
				{
					if (m_static == null)
					{
						m_static = new SameCloudWorldComparer();
					}
					return m_static;
				}
			}

<<<<<<< HEAD
			public override bool Equals(Tuple<string, MyWorldInfo> t1, Tuple<string, MyWorldInfo> t2)
			{
				return t1.Item1.Equals(t2.Item1);
			}
=======
		private const string INVENTORY_FILE = "ActiveInventory.sbl";
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

			public override int GetHashCode(Tuple<string, MyWorldInfo> t)
			{
				return Path.GetFileName(t.Item1).GetHashCode();
			}
		}

		public const string CHECKPOINT_FILE = "Sandbox.sbc";

<<<<<<< HEAD
		private const string WORLD_CONFIGURATION_FILE = "Sandbox_config.sbc";

		private const string LAST_SESSION_FILE = "LastSession.sbl";

		private const string INVENTORY_FILE = "ActiveInventory.sbl";

		public static MyObjectBuilder_LastSession LastSessionOverride;

		public static string LastSessionPath => Path.Combine(MyFileSystem.SavesPath, "LastSession.sbl");

		public static string LastSessionCloudPath => "Session/cloud/LastSession.sbl";

=======
		public static string LastSessionPath => Path.Combine(MyFileSystem.SavesPath, "LastSession.sbl");

		public static string LastSessionCloudPath => "Session/cloud/LastSession.sbl";

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static string ContentSessionsPath => "Worlds";

		private static string GetSectorPath(string sessionPath, Vector3I sectorPosition)
		{
			if (!sessionPath.EndsWith("/"))
			{
				sessionPath += "/";
			}
			return sessionPath + GetSectorName(sectorPosition) + ".sbs";
		}

		private static string GetSectorName(Vector3I sectorPosition)
		{
			return string.Format("{0}_{1}_{2}_{3}_", "SANDBOX", sectorPosition.X, sectorPosition.Y, sectorPosition.Z);
		}

		public static string GetSessionSavesPath(string sessionUniqueName, bool contentFolder, bool createIfNotExists = true, bool isCloud = false)
		{
			string text;
			if (isCloud)
			{
				text = "Worlds/cloud/" + sessionUniqueName;
				if (!text.EndsWith("/"))
				{
					text += "/";
				}
				return text;
			}
			text = ((!contentFolder) ? Path.Combine(MyFileSystem.SavesPath, sessionUniqueName) : Path.Combine(MyFileSystem.ContentPath, ContentSessionsPath, sessionUniqueName));
			if (createIfNotExists)
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		private static MyWorldInfo LoadWorldInfoFromCloud(string containerName)
		{
			try
			{
<<<<<<< HEAD
				using (Stream stream = new MemoryStream(MyGameService.LoadFromCloud(MyCloudHelper.Combine(containerName, "Sandbox.sbc"))).UnwrapGZip())
				{
					MyWorldInfo myWorldInfo = LoadWorldInfo(stream);
					myWorldInfo.StorageSize = MyCloudHelper.GetStorageSize(containerName);
					return myWorldInfo;
				}
=======
				using Stream stream = new MemoryStream(MyGameService.LoadFromCloud(MyCloudHelper.Combine(containerName, "Sandbox.sbc"))).UnwrapGZip();
				MyWorldInfo myWorldInfo = LoadWorldInfo(stream);
				myWorldInfo.StorageSize = MyCloudHelper.GetStorageSize(containerName);
				return myWorldInfo;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine(ex);
				return new MyWorldInfo
<<<<<<< HEAD
=======
				{
					IsCorrupted = true
				};
			}
		}

		public static string GetSessionPathFromScenario(string path, bool forceConsoleCompatible, out bool isCompatible)
		{
			string[] files = Directory.GetFiles(path, "*.scf");
			isCompatible = true;
			if (files.Length != 0 && MyObjectBuilderSerializer.DeserializeXML(files[0], out MyObjectBuilder_VSFiles objectBuilder))
			{
				return GetSessionPathFromScenarioObjectBuilder(objectBuilder.Campaign, path, forceConsoleCompatible, out isCompatible);
			}
			return null;
		}

		public static string GetSessionPathFromScenarioObjectBuilder(MyObjectBuilder_Campaign ob, string path, bool forceConsoleCompatible, out bool isCompatible)
		{
			isCompatible = true;
			if (ob.SupportedPlatforms != null && ob.SupportedPlatforms.Length != 0)
			{
				if (MyPlatformGameSettings.CONSOLE_COMPATIBLE || forceConsoleCompatible)
				{
					MyObjectBuilder_Campaign.MySupportedPlatform platform2 = Enumerable.FirstOrDefault<MyObjectBuilder_Campaign.MySupportedPlatform>((IEnumerable<MyObjectBuilder_Campaign.MySupportedPlatform>)ob.SupportedPlatforms, (Func<MyObjectBuilder_Campaign.MySupportedPlatform, bool>)((MyObjectBuilder_Campaign.MySupportedPlatform x) => x.Name == "XBox"));
					MyObjectBuilder_CampaignSM myObjectBuilder_CampaignSM = Enumerable.FirstOrDefault<MyObjectBuilder_CampaignSM>((IEnumerable<MyObjectBuilder_CampaignSM>)ob.StateMachines, (Func<MyObjectBuilder_CampaignSM, bool>)((MyObjectBuilder_CampaignSM x) => x.Name == platform2.StateMachine));
					if (myObjectBuilder_CampaignSM != null)
					{
						return Path.Combine(path, myObjectBuilder_CampaignSM.Nodes[0].SaveFilePath);
					}
					isCompatible = false;
					return null;
				}
				MyObjectBuilder_Campaign.MySupportedPlatform platform = Enumerable.FirstOrDefault<MyObjectBuilder_Campaign.MySupportedPlatform>((IEnumerable<MyObjectBuilder_Campaign.MySupportedPlatform>)ob.SupportedPlatforms, (Func<MyObjectBuilder_Campaign.MySupportedPlatform, bool>)((MyObjectBuilder_Campaign.MySupportedPlatform x) => x.Name == "PC"));
				MyObjectBuilder_CampaignSM myObjectBuilder_CampaignSM2 = Enumerable.FirstOrDefault<MyObjectBuilder_CampaignSM>((IEnumerable<MyObjectBuilder_CampaignSM>)ob.StateMachines, (Func<MyObjectBuilder_CampaignSM, bool>)((MyObjectBuilder_CampaignSM x) => x.Name == platform.StateMachine));
				if (myObjectBuilder_CampaignSM2 != null)
				{
					return Path.Combine(path, myObjectBuilder_CampaignSM2.Nodes[0].SaveFilePath);
				}
				isCompatible = false;
				return null;
			}
			return null;
		}

		public static MyWorldInfo GetWorldInfoFromScenario(string sessionPath, out bool isCompatible)
		{
			string text = sessionPath;
			if (text.ToLower().EndsWith(".sbc"))
			{
				text = Path.GetDirectoryName(text);
			}
			string[] files = Directory.GetFiles(text, "*.scf");
			isCompatible = true;
			if (files.Length != 0 && MyObjectBuilderSerializer.DeserializeXML(files[0], out MyObjectBuilder_VSFiles objectBuilder))
			{
				MyWorldInfo myWorldInfo = new MyWorldInfo();
				myWorldInfo.SessionName = MyStatControlText.SubstituteTexts(objectBuilder.Campaign.Name);
				myWorldInfo.Description = objectBuilder.Campaign.Description;
				myWorldInfo.StorageSize = DirectoryExtensions.GetStorageSize(text);
				myWorldInfo.SessionPath = GetSessionPathFromScenarioObjectBuilder(objectBuilder.Campaign, text, forceConsoleCompatible: false, out isCompatible);
				if (!isCompatible)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					IsCorrupted = true
				};
			}
		}

		public static string GetSessionPathFromScenario(string path, bool forceConsoleCompatible, out bool isCompatible, string pathToContentDir = null)
		{
			string[] files = Directory.GetFiles(path, "*.scf");
			isCompatible = true;
			if (files.Length != 0 && MyObjectBuilderSerializer.DeserializeXML(files[0], out MyObjectBuilder_VSFiles objectBuilder))
			{
				return GetSessionPathFromScenarioObjectBuilder(objectBuilder.Campaign, (pathToContentDir != null) ? pathToContentDir : path, forceConsoleCompatible, out isCompatible);
			}
			return null;
		}

		public static string GetSessionPathFromScenarioObjectBuilder(MyObjectBuilder_Campaign ob, string path, bool forceConsoleCompatible, out bool isCompatible)
		{
			isCompatible = true;
			MyObjectBuilder_CampaignSM myObjectBuilder_CampaignSM = null;
			if (ob.StateMachines != null)
			{
				if (ob.StateMachines.Length == 0)
				{
					isCompatible = false;
					return null;
				}
<<<<<<< HEAD
				ob.StateMachine = ob.StateMachines[0];
				if (ob.SupportedPlatforms != null && ob.SupportedPlatforms.Length != 0)
				{
					if (MyPlatformGameSettings.CONSOLE_COMPATIBLE || forceConsoleCompatible)
					{
						MyObjectBuilder_Campaign.MySupportedPlatform platform2 = ob.SupportedPlatforms.FirstOrDefault((MyObjectBuilder_Campaign.MySupportedPlatform x) => x.Name == "XBox");
						myObjectBuilder_CampaignSM = ob.StateMachines.FirstOrDefault((MyObjectBuilder_CampaignSM x) => x.Name == platform2.StateMachine);
					}
					else
					{
						MyObjectBuilder_Campaign.MySupportedPlatform platform = ob.SupportedPlatforms.FirstOrDefault((MyObjectBuilder_Campaign.MySupportedPlatform x) => x.Name == "PC");
						myObjectBuilder_CampaignSM = ob.StateMachines.FirstOrDefault((MyObjectBuilder_CampaignSM x) => x.Name == platform.StateMachine);
					}
				}
				else
				{
					myObjectBuilder_CampaignSM = ob.StateMachine;
				}
			}
			else
			{
				myObjectBuilder_CampaignSM = ob.StateMachine;
			}
			if (myObjectBuilder_CampaignSM != null)
			{
				return Path.Combine(path, myObjectBuilder_CampaignSM.Nodes[0].SaveFilePath);
			}
			isCompatible = false;
			return null;
		}

		public static MyWorldInfo GetWorldInfoFromScenario(string sessionPath, out bool isCompatible)
		{
			string text = sessionPath;
			if (text.ToLower().EndsWith(".sbc"))
			{
				text = Path.GetDirectoryName(text);
			}
			string[] files = Directory.GetFiles(text, "*.scf");
			isCompatible = true;
			if (files.Length != 0 && MyObjectBuilderSerializer.DeserializeXML(files[0], out MyObjectBuilder_VSFiles objectBuilder))
			{
				MyWorldInfo myWorldInfo = new MyWorldInfo();
				myWorldInfo.SessionName = MyStatControlText.SubstituteTexts(objectBuilder.Campaign.Name);
				myWorldInfo.Description = objectBuilder.Campaign.Description;
				myWorldInfo.StorageSize = DirectoryExtensions.GetStorageSize(text);
				myWorldInfo.SessionPath = GetSessionPathFromScenarioObjectBuilder(objectBuilder.Campaign, text, forceConsoleCompatible: false, out isCompatible);
				if (!isCompatible)
				{
					return null;
				}
				if (!string.IsNullOrEmpty(myWorldInfo.SessionPath))
				{
					MyWorldInfo myWorldInfo2 = LoadWorldInfoFromCheckpoint(myWorldInfo.SessionPath);
					myWorldInfo.IsExperimental = myWorldInfo2.IsExperimental;
				}
				ulong sizeInBytes;
				MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = LoadCheckpoint(myWorldInfo.SessionPath, out sizeInBytes);
				if (myObjectBuilder_Checkpoint != null)
				{
					MyObjectBuilder_CampaignSessionComponent myObjectBuilder_CampaignSessionComponent = myObjectBuilder_Checkpoint.SessionComponents.OfType<MyObjectBuilder_CampaignSessionComponent>().FirstOrDefault();
					if (myObjectBuilder_CampaignSessionComponent != null)
					{
						myWorldInfo.IsCampaign |= MyCampaignManager.Static != null && MyCampaignManager.Static.IsCampaign(myObjectBuilder_CampaignSessionComponent);
					}
				}
				return myWorldInfo;
			}
			return null;
		}

		public static MyWorldInfo LoadWorldInfoFromFile(string sessionPath)
		{
			bool isCompatible;
			MyWorldInfo worldInfoFromScenario = GetWorldInfoFromScenario(sessionPath, out isCompatible);
			if (worldInfoFromScenario != null || !isCompatible)
			{
				return worldInfoFromScenario;
			}
			return LoadWorldInfoFromCheckpoint(sessionPath);
		}

		private static MyWorldInfo LoadWorldInfoFromCheckpoint(string sessionPath)
		{
			try
			{
				if (sessionPath.ToLower().EndsWith(".sbc"))
				{
					sessionPath = Path.GetDirectoryName(sessionPath);
				}
				string path = Path.Combine(sessionPath, "Sandbox.sbc");
				if (!File.Exists(path))
				{
					return null;
				}
				using (Stream stream = MyFileSystem.OpenRead(path).UnwrapGZip())
				{
					MyWorldInfo myWorldInfo = LoadWorldInfo(stream);
					myWorldInfo.StorageSize = DirectoryExtensions.GetStorageSize(sessionPath);
					return myWorldInfo;
				}
=======
				if (!string.IsNullOrEmpty(myWorldInfo.SessionPath))
				{
					MyWorldInfo myWorldInfo2 = LoadWorldInfoFromCheckpoint(myWorldInfo.SessionPath);
					myWorldInfo.IsExperimental = myWorldInfo2.IsExperimental;
				}
				ulong sizeInBytes;
				MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = LoadCheckpoint(myWorldInfo.SessionPath, out sizeInBytes);
				if (myObjectBuilder_Checkpoint != null)
				{
					MyObjectBuilder_CampaignSessionComponent myObjectBuilder_CampaignSessionComponent = Enumerable.FirstOrDefault<MyObjectBuilder_CampaignSessionComponent>(Enumerable.OfType<MyObjectBuilder_CampaignSessionComponent>((IEnumerable)myObjectBuilder_Checkpoint.SessionComponents));
					if (myObjectBuilder_CampaignSessionComponent != null)
					{
						myWorldInfo.IsCampaign |= MyCampaignManager.Static != null && MyCampaignManager.Static.IsCampaign(myObjectBuilder_CampaignSessionComponent);
					}
				}
				return myWorldInfo;
			}
			return null;
		}

		public static MyWorldInfo LoadWorldInfoFromFile(string sessionPath)
		{
			bool isCompatible;
			MyWorldInfo worldInfoFromScenario = GetWorldInfoFromScenario(sessionPath, out isCompatible);
			if (worldInfoFromScenario != null || !isCompatible)
			{
				return worldInfoFromScenario;
			}
			return LoadWorldInfoFromCheckpoint(sessionPath);
		}

		private static MyWorldInfo LoadWorldInfoFromCheckpoint(string sessionPath)
		{
			try
			{
				if (sessionPath.ToLower().EndsWith(".sbc"))
				{
					sessionPath = Path.GetDirectoryName(sessionPath);
				}
				string text = Path.Combine(sessionPath, "Sandbox.sbc");
				if (!File.Exists(text))
				{
					return null;
				}
				using Stream stream = MyFileSystem.OpenRead(text).UnwrapGZip();
				MyWorldInfo myWorldInfo = LoadWorldInfo(stream);
				myWorldInfo.StorageSize = DirectoryExtensions.GetStorageSize(sessionPath);
				return myWorldInfo;
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine(ex);
				return new MyWorldInfo
				{
					IsCorrupted = true
				};
			}
		}

		private static MyWorldInfo LoadWorldInfo(Stream stream)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			MyWorldInfo myWorldInfo = new MyWorldInfo();
			try
			{
				XDocument val = null;
				XmlReaderSettings val2 = new XmlReaderSettings();
				val2.set_CheckCharacters(false);
				XmlReaderSettings val3 = val2;
				XmlReader val4 = XmlReader.Create(stream, val3);
				try
				{
					val = XDocument.Load(val4);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				XElement root = val.get_Root();
				XElement val5 = ((XContainer)root).Element(XName.op_Implicit("SessionName"));
				XElement val6 = ((XContainer)root).Element(XName.op_Implicit("Description"));
				XElement val7 = ((XContainer)root).Element(XName.op_Implicit("LastSaveTime"));
				((XContainer)root).Element(XName.op_Implicit("WorldID"));
				XElement val8 = ((XContainer)root).Element(XName.op_Implicit("WorkshopId"));
				XElement val9 = ((XContainer)root).Element(XName.op_Implicit("WorkshopServiceName"));
				XElement val10 = ((XContainer)root).Element(XName.op_Implicit("WorkshopId1"));
				XElement val11 = ((XContainer)root).Element(XName.op_Implicit("WorkshopServiceName1"));
				XElement val12 = ((XContainer)root).Element(XName.op_Implicit("Briefing"));
				XElement obj = ((XContainer)root).Element(XName.op_Implicit("Settings"));
				XElement val13 = ((obj != null) ? ((XContainer)((XContainer)root).Element(XName.op_Implicit("Settings"))).Element(XName.op_Implicit("ScenarioEditMode")) : null);
				XElement val14 = ((obj != null) ? ((XContainer)((XContainer)root).Element(XName.op_Implicit("Settings"))).Element(XName.op_Implicit("ExperimentalMode")) : null);
				XElement val15 = ((obj != null) ? ((XContainer)((XContainer)root).Element(XName.op_Implicit("Settings"))).Element(XName.op_Implicit("HasPlanets")) : null);
				XElement obj2 = ((XContainer)root).Element(XName.op_Implicit("SessionComponents"));
				object obj3;
				if (obj2 == null)
				{
					obj3 = null;
				}
				else
				{
					IEnumerable<XElement> enumerable = ((XContainer)obj2).Elements(XName.op_Implicit("MyObjectBuilder_SessionComponent"));
					obj3 = ((enumerable != null) ? Enumerable.FirstOrDefault<XElement>(enumerable, (Func<XElement, bool>)delegate(XElement e)
					{
						XAttribute firstAttribute = e.get_FirstAttribute();
						return ((firstAttribute != null) ? firstAttribute.get_Value() : null) == "MyObjectBuilder_CampaignSessionComponent";
					}) : null);
				}
				XElement val16 = (XElement)obj3;
				XElement obj4 = ((val16 != null) ? ((XContainer)val16).Element(XName.op_Implicit("Mod")) : null);
				if (val14 != null)
				{
					bool.TryParse(val14.get_Value(), out myWorldInfo.IsExperimental);
				}
				if (val5 != null)
				{
					myWorldInfo.SessionName = MyStatControlText.SubstituteTexts(val5.get_Value());
				}
				if (val6 != null)
				{
					myWorldInfo.Description = val6.get_Value();
				}
				if (val7 != null)
				{
					DateTime.TryParse(val7.get_Value(), out myWorldInfo.LastSaveTime);
				}
				List<WorkshopId> list = new List<WorkshopId>();
				if (val8 != null && ulong.TryParse(val8.get_Value(), out var result))
				{
					list.Add(new WorkshopId(result, ((val9 != null) ? val9.get_Value() : null) ?? MyGameService.GetDefaultUGC().ServiceName));
				}
				if (val10 != null && ulong.TryParse(val10.get_Value(), out result))
				{
					list.Add(new WorkshopId(result, val11.get_Value()));
				}
				myWorldInfo.WorkshopIds = list.ToArray();
				if (val12 != null)
				{
					myWorldInfo.Briefing = val12.get_Value();
				}
				if (val13 != null)
				{
					bool.TryParse(val13.get_Value(), out myWorldInfo.ScenarioEditMode);
				}
				if (val15 != null)
				{
					bool.TryParse(val15.get_Value(), out myWorldInfo.HasPlanets);
				}
				object s;
				if (obj4 == null)
				{
					s = null;
				}
				else
				{
					XElement obj5 = ((XContainer)obj4).Element(XName.op_Implicit("PublishedFileId"));
					s = ((obj5 != null) ? obj5.get_Value() : null);
				}
				ulong.TryParse((string)s, out var result2);
				int isCampaign;
				if (val16 != null)
				{
					MyCampaignManager @static = MyCampaignManager.Static;
					if (@static == null)
					{
						isCampaign = 0;
					}
					else
					{
						XElement obj6 = ((XContainer)val16).Element(XName.op_Implicit("CampaignName"));
						string campaignName = ((obj6 != null) ? obj6.get_Value() : null);
						object obj7;
						if (val16 == null)
						{
							obj7 = null;
						}
						else
						{
							XElement obj8 = ((XContainer)val16).Element(XName.op_Implicit("IsVanilla"));
							obj7 = ((obj8 != null) ? obj8.get_Value().ToLower() : null);
						}
						isCampaign = (@static.IsCampaign(campaignName, (string)obj7 == "true", result2) ? 1 : 0);
					}
				}
				else
				{
					isCampaign = 0;
				}
				myWorldInfo.IsCampaign = (byte)isCampaign != 0;
				return myWorldInfo;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine(ex);
				return new MyWorldInfo
				{
					IsCorrupted = true
				};
			}
		}

		private static string TryReadAttributeValue(XmlReader reader)
		{
			reader.Read();
			if (reader.NodeType == XmlNodeType.Text && reader.Value != null)
			{
				return reader.Value;
			}
			return null;
		}

		private static MyWorldInfo LoadWorldInfo(Stream stream)
		{
			MyWorldInfo worldInfo = new MyWorldInfo();
			XmlReaderSettings settings = new XmlReaderSettings
			{
				CheckCharacters = false
			};
			string text = null;
			string text2 = null;
			string text3 = null;
			string serviceName = null;
			using (XmlReader xmlReader = XmlReader.Create(stream, settings))
			{
				xmlReader.MoveToContent();
				while (xmlReader.Read())
				{
					if (xmlReader.NodeType != XmlNodeType.Element)
					{
						continue;
					}
					string name = xmlReader.Name;
					string text4 = TryReadAttributeValue(xmlReader);
					switch (name)
					{
					case "SessionName":
						worldInfo.SessionName = ((text4 != null) ? MyStatControlText.SubstituteTexts(text4) : "");
						break;
					case "Description":
						worldInfo.Description = ((text4 != null) ? text4 : "");
						break;
					case "LastSaveTime":
						if (text4 != null)
						{
							DateTime.TryParse(text4, out worldInfo.LastSaveTime);
						}
						break;
					case "WorkshopId":
						text = text4;
						break;
					case "WorkshopId1":
						text2 = text4;
						break;
					case "WorkshopServiceName":
						text3 = text4;
						break;
					case "WorkshopServiceName1":
						serviceName = text4;
						break;
					case "Briefing":
						worldInfo.Briefing = ((text4 != null) ? text4 : "");
						break;
					case "Settings":
						LoadWorldInfoSettings(xmlReader, ref worldInfo);
						break;
					case "SessionComponents":
						LoadWorldInfoComponents(xmlReader, ref worldInfo);
						break;
					}
				}
			}
			List<WorkshopId> list = new List<WorkshopId>();
			if (text != null && ulong.TryParse(text, out var result))
			{
				list.Add(new WorkshopId(result, text3 ?? MyGameService.GetDefaultUGC().ServiceName));
			}
			if (text2 != null && ulong.TryParse(text, out result))
			{
				list.Add(new WorkshopId(result, serviceName));
			}
			worldInfo.WorkshopIds = list.ToArray();
			return worldInfo;
		}

		private static void LoadWorldInfoSettings(XmlReader reader, ref MyWorldInfo worldInfo)
		{
			do
			{
				if (reader.NodeType != XmlNodeType.Element)
				{
					continue;
				}
				string name = reader.Name;
				string text = TryReadAttributeValue(reader);
				switch (name)
				{
				case "ScenarioEditMode":
					if (text != null)
					{
						bool.TryParse(text, out worldInfo.ScenarioEditMode);
					}
					break;
				case "ExperimentalMode":
					if (text != null)
					{
						bool.TryParse(text, out worldInfo.IsExperimental);
					}
					break;
				case "HasPlanets":
					if (text != null)
					{
						bool.TryParse(text, out worldInfo.HasPlanets);
					}
					break;
				}
			}
			while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || !(reader.Name == "Settings")));
		}

		private static void LoadWorldInfoComponents(XmlReader reader, ref MyWorldInfo worldInfo)
		{
			int depth = reader.Depth;
			while (reader.Depth >= depth)
			{
				if (reader.NodeType == XmlNodeType.Element && reader.Name == "MyObjectBuilder_SessionComponent" && reader.GetAttribute("xsi:type") == "MyObjectBuilder_CampaignSessionComponent")
				{
					do
					{
						if (reader.NodeType == XmlNodeType.Element)
						{
							string name = reader.Name;
							string text = TryReadAttributeValue(reader);
							if (name == "CampaignName")
							{
								if (text != null)
								{
									worldInfo.IsCampaign = true;
								}
								return;
							}
						}
						reader.Read();
					}
					while (reader.NodeType != XmlNodeType.EndElement || !(reader.Name == "MyObjectBuilder_SessionComponent"));
				}
				if (!reader.Read())
				{
					break;
				}
				reader.MoveToContent();
				if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "SessionComponents")
				{
					break;
				}
			}
		}

		public static MyObjectBuilder_Checkpoint LoadCheckpointFromCloud(string fileName, out ulong sizeInBytes, MyGameModeEnum? forceGameMode = null, MyOnlineModeEnum? forceOnlineMode = null)
		{
			sizeInBytes = 0uL;
			string text = MyCloudHelper.Combine(fileName, "Sandbox.sbc");
			byte[] array = MyGameService.LoadFromCloud(text);
			if (array == null)
			{
				return null;
			}
			MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Checkpoint>(array, out var objectBuilder, out sizeInBytes);
			if (objectBuilder != null && string.IsNullOrEmpty(objectBuilder.SessionName))
			{
				objectBuilder.SessionName = Path.GetFileNameWithoutExtension(text);
			}
			ulong sizeInBytes2;
			MyObjectBuilder_WorldConfiguration myObjectBuilder_WorldConfiguration = LoadWorldConfigurationFromCloud(fileName, out sizeInBytes2);
			if (myObjectBuilder_WorldConfiguration != null)
			{
				MyLog.Default.WriteLineAndConsole("Sandbox world configuration file found, overriding checkpoint settings.");
<<<<<<< HEAD
				if (objectBuilder != null)
				{
					objectBuilder.Settings = myObjectBuilder_WorldConfiguration.Settings;
					objectBuilder.Mods = myObjectBuilder_WorldConfiguration.Mods;
					if (!string.IsNullOrEmpty(myObjectBuilder_WorldConfiguration.SessionName))
					{
						objectBuilder.SessionName = myObjectBuilder_WorldConfiguration.SessionName;
					}
=======
				objectBuilder.Settings = myObjectBuilder_WorldConfiguration.Settings;
				objectBuilder.Mods = myObjectBuilder_WorldConfiguration.Mods;
				if (!string.IsNullOrEmpty(myObjectBuilder_WorldConfiguration.SessionName))
				{
					objectBuilder.SessionName = myObjectBuilder_WorldConfiguration.SessionName;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				sizeInBytes += sizeInBytes2;
			}
			if (objectBuilder != null)
			{
				MySession.PerformPlatformPatchBeforeLoad(objectBuilder, forceGameMode, forceOnlineMode);
				CheckExperimental(objectBuilder);
<<<<<<< HEAD
=======
			}
			return objectBuilder;
		}

		public static MyObjectBuilder_Checkpoint LoadCheckpoint(string sessionDirectory, out ulong sizeInBytes, MyGameModeEnum? forceGameMode = null, MyOnlineModeEnum? forceOnlineMode = null)
		{
			sizeInBytes = 0uL;
			if (sessionDirectory.ToLower().EndsWith(".sbc"))
			{
				sessionDirectory = Path.GetDirectoryName(sessionDirectory);
			}
			string text = Path.Combine(sessionDirectory, "Sandbox.sbc");
			if (!File.Exists(text))
			{
				return null;
			}
			MyObjectBuilder_Checkpoint objectBuilder = null;
			MyObjectBuilderSerializer.DeserializeXML(text, out objectBuilder, out sizeInBytes);
			if (objectBuilder != null)
			{
				if (string.IsNullOrEmpty(objectBuilder.SessionName))
				{
					objectBuilder.SessionName = Path.GetFileNameWithoutExtension(text);
				}
				ulong sizeInBytes2 = 0uL;
				MyObjectBuilder_WorldConfiguration myObjectBuilder_WorldConfiguration = LoadWorldConfiguration(sessionDirectory, out sizeInBytes2);
				if (myObjectBuilder_WorldConfiguration != null)
				{
					MyLog.Default.WriteLineAndConsole("Sandbox world configuration file found, overriding checkpoint settings.");
					objectBuilder.Settings = myObjectBuilder_WorldConfiguration.Settings;
					objectBuilder.Mods = myObjectBuilder_WorldConfiguration.Mods;
					if (!string.IsNullOrEmpty(myObjectBuilder_WorldConfiguration.SessionName))
					{
						objectBuilder.SessionName = myObjectBuilder_WorldConfiguration.SessionName;
					}
					sizeInBytes += sizeInBytes2;
				}
				MySession.PerformPlatformPatchBeforeLoad(objectBuilder, forceGameMode, forceOnlineMode);
				CheckExperimental(objectBuilder);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return objectBuilder;
		}

<<<<<<< HEAD
		public static MyObjectBuilder_Checkpoint LoadCheckpoint(string sessionDirectory, out ulong sizeInBytes, MyGameModeEnum? forceGameMode = null, MyOnlineModeEnum? forceOnlineMode = null)
		{
			sizeInBytes = 0uL;
			if (sessionDirectory.ToLower().EndsWith(".sbc"))
			{
				sessionDirectory = Path.GetDirectoryName(sessionDirectory);
			}
			string path = Path.Combine(sessionDirectory, "Sandbox.sbc");
			if (!File.Exists(path))
			{
				return null;
			}
			MyObjectBuilder_Checkpoint objectBuilder = null;
			MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder, out sizeInBytes);
			if (objectBuilder != null)
			{
				if (string.IsNullOrEmpty(objectBuilder.SessionName))
				{
					objectBuilder.SessionName = Path.GetFileNameWithoutExtension(path);
				}
				ulong sizeInBytes2 = 0uL;
				MyObjectBuilder_WorldConfiguration myObjectBuilder_WorldConfiguration = LoadWorldConfiguration(sessionDirectory, out sizeInBytes2);
				if (myObjectBuilder_WorldConfiguration != null)
				{
					MyLog.Default.WriteLineAndConsole("Sandbox world configuration file found, overriding checkpoint settings.");
					objectBuilder.Settings = myObjectBuilder_WorldConfiguration.Settings;
					objectBuilder.Mods = myObjectBuilder_WorldConfiguration.Mods;
					if (!string.IsNullOrEmpty(myObjectBuilder_WorldConfiguration.SessionName))
					{
						objectBuilder.SessionName = myObjectBuilder_WorldConfiguration.SessionName;
					}
					sizeInBytes += sizeInBytes2;
				}
				MySession.PerformPlatformPatchBeforeLoad(objectBuilder, forceGameMode, forceOnlineMode);
				CheckExperimental(objectBuilder);
			}
			return objectBuilder;
		}

		private static void CheckExperimental(MyObjectBuilder_Checkpoint checkpoint)
		{
=======
		private static void CheckExperimental(MyObjectBuilder_Checkpoint checkpoint)
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyObjectBuilder_SessionSettings settings = checkpoint.Settings;
			if (settings != null && !settings.ExperimentalMode && (checkpoint.IsSettingsExperimental(remoteSetting: false) || (MySandboxGame.ConfigDedicated != null && MySandboxGame.ConfigDedicated.Plugins != null && MySandboxGame.ConfigDedicated.Plugins.Count != 0) || (MySandboxGame.Config.ExperimentalMode && MySandboxGame.ConfigDedicated == null)))
			{
				settings.ExperimentalMode = true;
			}
		}

		public static MyObjectBuilder_WorldConfiguration LoadWorldConfiguration(string sessionPath)
		{
			ulong sizeInBytes = 0uL;
			MyObjectBuilder_WorldConfiguration myObjectBuilder_WorldConfiguration = LoadWorldConfiguration(sessionPath, out sizeInBytes);
			if (myObjectBuilder_WorldConfiguration != null)
			{
				return myObjectBuilder_WorldConfiguration;
			}
			MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = LoadCheckpoint(sessionPath, out sizeInBytes);
			if (myObjectBuilder_Checkpoint != null)
			{
				return new MyObjectBuilder_WorldConfiguration
				{
					LastSaveTime = myObjectBuilder_Checkpoint.LastSaveTime,
					Mods = myObjectBuilder_Checkpoint.Mods,
					SessionName = myObjectBuilder_Checkpoint.SessionName,
					Settings = myObjectBuilder_Checkpoint.Settings,
					SubtypeName = myObjectBuilder_Checkpoint.SubtypeName
				};
			}
			return null;
		}

		public static MyObjectBuilder_WorldConfiguration LoadWorldConfiguration(string sessionPath, out ulong sizeInBytes)
		{
			if (sessionPath.ToLower().EndsWith(".sbc"))
			{
				sessionPath = Path.GetDirectoryName(sessionPath);
			}
<<<<<<< HEAD
			else if (sessionPath.ToLower().EndsWith(".scf"))
			{
				sessionPath = Path.GetDirectoryName(sessionPath);
				foreach (string file in MyFileSystem.GetFiles(sessionPath))
				{
					if (file.Contains("Sandbox_config.sbc"))
					{
						sessionPath = Path.GetDirectoryName(file);
					}
				}
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			string text = Path.Combine(sessionPath, "Sandbox_config.sbc");
			if (!File.Exists(text))
			{
				sizeInBytes = 0uL;
				return null;
			}
			MyLog.Default.WriteLineAndConsole("Loading Sandbox world configuration file " + text);
			MyObjectBuilder_WorldConfiguration objectBuilder = null;
			MyObjectBuilderSerializer.DeserializeXML(text, out objectBuilder, out sizeInBytes);
			MySession.PerformPlatformPatchBeforeLoad(objectBuilder, null, null);
			return objectBuilder;
		}

		private static MyObjectBuilder_WorldConfiguration LoadWorldConfigurationFromCloud(string fileName, out ulong sizeInBytes)
		{
			sizeInBytes = 0uL;
			string text = MyCloudHelper.Combine(fileName, "Sandbox_config.sbc");
			MyLog.Default.WriteLineAndConsole("Loading Sandbox world configuration file " + text);
			byte[] array = MyGameService.LoadFromCloud(text);
			if (array == null)
			{
				return null;
			}
			MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_WorldConfiguration>(array, out var objectBuilder, out sizeInBytes);
			MySession.PerformPlatformPatchBeforeLoad(objectBuilder, null, null);
			return objectBuilder;
		}

		public static MyObjectBuilder_Sector LoadSector(string sessionPath, Vector3I sectorPosition, bool allowXml, out ulong sizeInBytes, out bool needsXml)
		{
			return LoadSector(GetSectorPath(sessionPath, sectorPosition), allowXml, out sizeInBytes, out needsXml);
		}

		private static MyObjectBuilder_Sector LoadSector(string path, bool allowXml, out ulong sizeInBytes, out bool needsXml)
		{
			sizeInBytes = 0uL;
			needsXml = false;
			MyObjectBuilder_Sector objectBuilder = null;
			string path2 = path + "B5";
			if (MyFileSystem.FileExists(path2))
			{
				MyObjectBuilderSerializer.DeserializePB<MyObjectBuilder_Sector>(path2, out objectBuilder, out sizeInBytes);
				if (objectBuilder == null || objectBuilder.SectorObjects == null)
				{
					if (allowXml)
					{
						MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder, out sizeInBytes);
						if (objectBuilder != null)
						{
							MyObjectBuilderSerializer.SerializePB(path2, compress: false, objectBuilder);
						}
					}
					else
					{
						needsXml = true;
					}
				}
			}
			else if (allowXml)
			{
				MyObjectBuilderSerializer.DeserializeXML(path, out objectBuilder, out sizeInBytes);
				if (!MyFileSystem.FileExists(path2))
				{
					MyObjectBuilderSerializer.SerializePB(path + "B5", compress: false, objectBuilder);
				}
			}
			else
			{
				needsXml = true;
			}
			if (objectBuilder == null)
			{
				MySandboxGame.Log.WriteLine("Incorrect save data");
				return null;
			}
			return objectBuilder;
		}

		public static MyObjectBuilder_CubeGrid LoadCubeGrid(string sessionPath, string fileName, out ulong sizeInBytes)
		{
			MyObjectBuilderSerializer.DeserializeXML(Path.Combine(sessionPath, fileName), out MyObjectBuilder_CubeGrid objectBuilder, out sizeInBytes);
			if (objectBuilder == null)
			{
				MySandboxGame.Log.WriteLine("Incorrect save data");
				return null;
			}
			return objectBuilder;
		}

		public static bool SaveSector(MyObjectBuilder_Sector sector, string sessionPath, Vector3I sectorPosition, out ulong sizeInBytes, List<MyCloudFile> fileList)
		{
			string sectorPath = GetSectorPath(sessionPath, sectorPosition);
			bool result = MyObjectBuilderSerializer.SerializeXML(sectorPath, MyPlatformGameSettings.GAME_SAVES_COMPRESSED_BY_DEFAULT, sector, out sizeInBytes);
			fileList.Add(new MyCloudFile(sectorPath));
			string text = sectorPath + "B5";
			MyObjectBuilderSerializer.SerializePB(text, MyPlatformGameSettings.GAME_SAVES_COMPRESSED_BY_DEFAULT, sector, out sizeInBytes);
			fileList.Add(new MyCloudFile(text));
			return result;
		}

		public static CloudResult SaveCheckpointToCloud(MyObjectBuilder_Checkpoint checkpoint, string cloudPath)
		{
			List<MyCloudFile> list = new List<MyCloudFile>();
			SaveCheckpoint(checkpoint, MyFileSystem.TempPath, list);
			return MyGameService.SaveToCloud(cloudPath, list);
		}

		public static bool SaveCheckpoint(MyObjectBuilder_Checkpoint checkpoint, string sessionPath, List<MyCloudFile> fileList = null)
		{
			ulong sizeInBytes;
			return SaveCheckpoint(checkpoint, sessionPath, out sizeInBytes, fileList);
		}

		public static bool SaveCheckpoint(MyObjectBuilder_Checkpoint checkpoint, string sessionPath, out ulong sizeInBytes, List<MyCloudFile> fileList)
		{
			string text = Path.Combine(sessionPath, "Sandbox.sbc");
			bool num = MyObjectBuilderSerializer.SerializeXML(text, MyPlatformGameSettings.GAME_SAVES_COMPRESSED_BY_DEFAULT, checkpoint, out sizeInBytes);
			fileList?.Add(new MyCloudFile(text));
			MyObjectBuilder_WorldConfiguration myObjectBuilder_WorldConfiguration = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_WorldConfiguration>();
			myObjectBuilder_WorldConfiguration.Settings = checkpoint.Settings;
			myObjectBuilder_WorldConfiguration.Mods = checkpoint.Mods;
			myObjectBuilder_WorldConfiguration.SessionName = checkpoint.SessionName;
			myObjectBuilder_WorldConfiguration.LastSaveTime = checkpoint.LastSaveTime;
			ulong sizeInBytes2 = 0uL;
			bool result = num & SaveWorldConfiguration(myObjectBuilder_WorldConfiguration, sessionPath, out sizeInBytes2, fileList);
			sizeInBytes += sizeInBytes2;
			return result;
		}

<<<<<<< HEAD
		/// <summary>
		/// Save world configuration. This method is used from the game and from dedicated server gui
		/// </summary>
		/// <param name="configuration">configuration</param>
		/// <param name="sessionPath">path</param>
		/// <param name="fileList">file list for cloud</param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static bool SaveWorldConfiguration(MyObjectBuilder_WorldConfiguration configuration, string sessionPath, List<MyCloudFile> fileList)
		{
			ulong sizeInBytes;
			return SaveWorldConfiguration(configuration, sessionPath, out sizeInBytes, fileList);
		}

		private static bool SaveWorldConfiguration(MyObjectBuilder_WorldConfiguration configuration, string sessionPath, out ulong sizeInBytes, List<MyCloudFile> fileList)
		{
			string text = Path.Combine(sessionPath, "Sandbox_config.sbc");
			MyLog.Default.WriteLineAndConsole("Saving Sandbox world configuration file " + text);
			fileList?.Add(new MyCloudFile(text));
			return MyObjectBuilderSerializer.SerializeXML(text, compress: false, configuration, out sizeInBytes);
		}

		public static bool SaveRespawnShip(MyObjectBuilder_CubeGrid cubegrid, string sessionPath, string fileName, out ulong sizeInBytes)
		{
			return MyObjectBuilderSerializer.SerializeXML(Path.Combine(sessionPath, fileName), MyPlatformGameSettings.GAME_SAVES_COMPRESSED_BY_DEFAULT, cubegrid, out sizeInBytes);
		}

		public static List<Tuple<string, MyWorldInfo>> GetAvailableWorldInfos(List<string> customPaths = null)
		{
			MySandboxGame.Log.WriteLine("Loading available saves - START");
			List<Tuple<string, MyWorldInfo>> result = new List<Tuple<string, MyWorldInfo>>();
			using (MySandboxGame.Log.IndentUsing(LoggingOptions.ALL))
			{
				if (customPaths == null)
				{
					GetWorldInfoFromDirectory(MyFileSystem.SavesPath, result);
				}
				else
				{
					foreach (string customPath in customPaths)
					{
						GetWorldInfoFromDirectory(customPath, result);
					}
				}
			}
			MySandboxGame.Log.WriteLine("Loading available saves - END");
			return result;
		}

		public static List<Tuple<string, MyWorldInfo>> GetAvailableWorldInfosFromCloud(List<string> customPaths = null)
		{
			MySandboxGame.Log.WriteLine("Loading available saves - START");
			List<Tuple<string, MyWorldInfo>> list = new List<Tuple<string, MyWorldInfo>>();
			using (MySandboxGame.Log.IndentUsing(LoggingOptions.ALL))
			{
				if (customPaths == null)
				{
					GetWorldInfoFromCloud(string.Empty, list);
				}
				else
				{
					foreach (string customPath in customPaths)
					{
						GetWorldInfoFromCloud(customPath, list);
					}
				}
			}
<<<<<<< HEAD
			list = list.Distinct(SameCloudWorldComparer.Static).ToList();
=======
			list = Enumerable.ToList<Tuple<string, MyWorldInfo>>(Enumerable.Distinct<Tuple<string, MyWorldInfo>>((IEnumerable<Tuple<string, MyWorldInfo>>)list, (IEqualityComparer<Tuple<string, MyWorldInfo>>)SameCloudWorldComparer.Static));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MySandboxGame.Log.WriteLine("Loading available saves - END");
			return list;
		}

		private static List<Tuple<string, MyWorldInfo>> GetAvailableInfosFromDirectory(string worldCategory, string worldDirectoryPath)
		{
			string text = "Loading available " + worldCategory;
			MySandboxGame.Log.WriteLine(text + " - START");
			List<Tuple<string, MyWorldInfo>> result = new List<Tuple<string, MyWorldInfo>>();
			using (MySandboxGame.Log.IndentUsing(LoggingOptions.ALL))
			{
				GetWorldInfoFromDirectory(Path.Combine(MyFileSystem.ContentPath, worldDirectoryPath), result);
			}
			MySandboxGame.Log.WriteLine(text + " - END");
			return result;
		}

		public static void GetWorldInfoFromDirectory(string path, List<Tuple<string, MyWorldInfo>> result)
		{
			bool flag = Directory.Exists(path);
			MySandboxGame.Log.WriteLine($"GetWorldInfoFromDirectory (Exists: {flag}) '{path}'");
			if (!flag)
			{
				return;
			}
			string[] directories = Directory.GetDirectories(path, "*", (SearchOption)0);
			foreach (string text in directories)
			{
				MyWorldInfo myWorldInfo = LoadWorldInfoFromFile(text);
				string item = text;
				if (myWorldInfo != null)
				{
					if (string.IsNullOrEmpty(myWorldInfo.SessionName))
					{
						myWorldInfo.SessionName = Path.GetFileName(text);
					}
					if (!string.IsNullOrEmpty(myWorldInfo.SessionPath))
					{
						item = myWorldInfo.SessionPath;
					}
				}
				result.Add(Tuple.Create(item, myWorldInfo));
			}
		}

		public static void GetWorldInfoFromCloud(string path, List<Tuple<string, MyWorldInfo>> result)
		{
			MySandboxGame.Log.WriteLine($"GetWorldInfoFromCloud '{path}'");
			List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(GetSessionSavesPath(path, contentFolder: false, createIfNotExists: false, isCloud: true));
			if (cloudFiles == null)
			{
				return;
			}
			foreach (MyCloudFileInfo item in cloudFiles)
			{
				if (item.Name.EndsWith("Sandbox.sbc"))
				{
					string text = item.Name.Replace("Sandbox.sbc", "");
					MyWorldInfo myWorldInfo = LoadWorldInfoFromCloud(text);
					if (myWorldInfo != null && string.IsNullOrEmpty(myWorldInfo.SessionName))
					{
						myWorldInfo.SessionName = Path.GetFileName(text);
					}
					result.Add(Tuple.Create(text, myWorldInfo));
				}
			}
		}

		public static string GetLastSessionPath()
		{
			return CheckLastSession(GetLastSession());
		}

		private static string CheckLastSession(MyObjectBuilder_LastSession lastSession)
		{
			if (lastSession == null)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(lastSession.Path))
			{
				string text = Path.Combine(lastSession.IsContentWorlds ? MyFileSystem.ContentPath : MyFileSystem.SavesPath, lastSession.Path);
				if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD || Directory.Exists(text))
				{
					return text;
				}
			}
			return null;
		}

		public static void UpdateLastSessionFromCloud()
		{
			if (MyPlatformGameSettings.GAME_LAST_SESSION_TO_CLOUD)
			{
				byte[] array = MyGameService.LoadFromCloud(LastSessionCloudPath);
				if (array != null)
				{
					File.WriteAllBytes(LastSessionPath, array);
				}
				else
				{
					File.Delete(LastSessionPath);
				}
			}
		}

		public static MyObjectBuilder_LastSession GetLastSession()
		{
			if (LastSessionOverride != null && CheckLastSession(LastSessionOverride) != null)
			{
				return LastSessionOverride;
			}
			if (!File.Exists(LastSessionPath))
			{
				return null;
			}
			MyObjectBuilder_LastSession objectBuilder = null;
			MyObjectBuilderSerializer.DeserializeXML(LastSessionPath, out objectBuilder);
			return objectBuilder;
		}

		public static bool SaveLastSessionInfo(string sessionPath, bool isOnline, bool isLobby, string gameName, string serverIP, int serverPort)
		{
			MyObjectBuilder_LastSession myObjectBuilder_LastSession = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_LastSession>();
			myObjectBuilder_LastSession.IsOnline = isOnline;
			myObjectBuilder_LastSession.IsLobby = isLobby;
			if (isOnline)
			{
				if (isLobby)
				{
					myObjectBuilder_LastSession.GameName = gameName;
					myObjectBuilder_LastSession.ServerIP = serverIP;
				}
				else
				{
					myObjectBuilder_LastSession.GameName = gameName;
					myObjectBuilder_LastSession.ServerIP = serverIP;
					myObjectBuilder_LastSession.ServerPort = serverPort;
				}
			}
			else if (sessionPath != null)
			{
				myObjectBuilder_LastSession.Path = sessionPath;
				myObjectBuilder_LastSession.GameName = gameName;
				myObjectBuilder_LastSession.IsContentWorlds = sessionPath.StartsWith(MyFileSystem.ContentPath, StringComparison.InvariantCultureIgnoreCase);
			}
			ulong sizeInBytes;
			bool num = MyObjectBuilderSerializer.SerializeXML(LastSessionPath, compress: false, myObjectBuilder_LastSession, out sizeInBytes);
			if (num && MyPlatformGameSettings.GAME_LAST_SESSION_TO_CLOUD)
			{
				MyGameService.SaveToCloud(LastSessionCloudPath, File.ReadAllBytes(LastSessionPath));
			}
			return num;
		}

		public static bool SaveLastSessionInfo(string sessionPath, bool isOnline, bool isLobby, string gameName, string serverConnectionString)
		{
			MyObjectBuilder_LastSession myObjectBuilder_LastSession = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_LastSession>();
			myObjectBuilder_LastSession.IsOnline = isOnline;
			myObjectBuilder_LastSession.IsLobby = isLobby;
			if (isOnline)
			{
				myObjectBuilder_LastSession.GameName = gameName;
				myObjectBuilder_LastSession.ConnectionString = serverConnectionString;
			}
			else if (sessionPath != null)
			{
				myObjectBuilder_LastSession.Path = sessionPath;
				myObjectBuilder_LastSession.GameName = gameName;
				myObjectBuilder_LastSession.IsContentWorlds = sessionPath.StartsWith(MyFileSystem.ContentPath, StringComparison.InvariantCultureIgnoreCase);
			}
			ulong sizeInBytes;
			bool num = MyObjectBuilderSerializer.SerializeXML(LastSessionPath, compress: false, myObjectBuilder_LastSession, out sizeInBytes);
			if (num && MyPlatformGameSettings.GAME_LAST_SESSION_TO_CLOUD)
			{
				MyGameService.SaveToCloud(LastSessionCloudPath, File.ReadAllBytes(LastSessionPath));
			}
			return num;
		}

		public static void ClearLastSessionInfo()
		{
			string lastSessionPath = LastSessionPath;
			if (File.Exists(lastSessionPath))
<<<<<<< HEAD
			{
				File.Delete(lastSessionPath);
			}
			if (MyPlatformGameSettings.GAME_LAST_SESSION_TO_CLOUD)
			{
=======
			{
				File.Delete(lastSessionPath);
			}
			if (MyPlatformGameSettings.GAME_LAST_SESSION_TO_CLOUD)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyGameService.DeleteFromCloud(LastSessionCloudPath);
			}
		}

		private static string GetInventoryFile(MyCharacter character)
		{
<<<<<<< HEAD
			long index = 0L;
			if (character.EntityId != MySession.Static.LocalCharacter?.EntityId)
			{
				index = character.EntityId;
			}
			return GetInventoryFile(index);
		}

		private static string GetInventoryFile(long index)
		{
			string text = Path.GetFileNameWithoutExtension("ActiveInventory.sbl");
			string path = MyFileSystem.SavesPath;
			if (index > 0)
			{
				text = text + "-" + index;
=======
			return GetInventoryFile(Enumerable.ToList<MyCharacter>(MySession.Static.SavedCharacters).IndexOf(character));
		}

		private static string GetInventoryFile(int index)
		{
			string text = Path.GetFileNameWithoutExtension("ActiveInventory.sbl");
			string path = MyFileSystem.SavesPath;
			if (index > 0)
			{
				text += index;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				path = MySession.Static.CurrentPath;
			}
			text += Path.GetExtension("ActiveInventory.sbl");
			return Path.Combine(path, text);
		}

		private static MyObjectBuilder_SkinInventory GetInventoryBuilder(string filename)
		{
			if (!MyFileSystem.FileExists(filename))
			{
				return null;
<<<<<<< HEAD
			}
			if (!MyObjectBuilderSerializer.DeserializeXML(filename, out MyObjectBuilder_SkinInventory objectBuilder))
			{
				return null;
			}
=======
			}
			if (!MyObjectBuilderSerializer.DeserializeXML(filename, out MyObjectBuilder_SkinInventory objectBuilder))
			{
				return null;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return objectBuilder;
		}

		public static void PreloadLocalInventoryConfig()
		{
			if (!MyGameService.IsActive)
			{
				return;
			}
			List<MyGameInventoryItem> list = new List<MyGameInventoryItem>();
			try
			{
<<<<<<< HEAD
				MyObjectBuilder_SkinInventory inventoryBuilder = GetInventoryBuilder(GetInventoryFile(0L));
=======
				MyObjectBuilder_SkinInventory inventoryBuilder = GetInventoryBuilder(GetInventoryFile(0));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (inventoryBuilder == null)
				{
					return;
				}
				if (inventoryBuilder.Character != null)
				{
					foreach (ulong itemId2 in inventoryBuilder.Character)
					{
<<<<<<< HEAD
						MyGameInventoryItem myGameInventoryItem = MyGameService.InventoryItems.FirstOrDefault((MyGameInventoryItem i) => i.ID == itemId2);
=======
						MyGameInventoryItem myGameInventoryItem = Enumerable.FirstOrDefault<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)MyGameService.InventoryItems, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.ID == itemId2));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (myGameInventoryItem != null)
						{
							list.Add(myGameInventoryItem);
						}
					}
				}
				if (inventoryBuilder.Tools != null)
				{
					foreach (ulong itemId in inventoryBuilder.Tools)
					{
<<<<<<< HEAD
						MyGameInventoryItem myGameInventoryItem2 = MyGameService.InventoryItems.FirstOrDefault((MyGameInventoryItem i) => i.ID == itemId);
=======
						MyGameInventoryItem myGameInventoryItem2 = Enumerable.FirstOrDefault<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)MyGameService.InventoryItems, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.ID == itemId));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (myGameInventoryItem2 != null)
						{
							list.Add(myGameInventoryItem2);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				return;
			}
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			List<string> list5 = new List<string>();
			foreach (MyGameInventoryItem item in list)
			{
				foreach (KeyValuePair<string, MyTextureChange> skinTextureChange in MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(item.ItemDefinition.AssetModifierId).SkinTextureChanges)
				{
					if (!string.IsNullOrEmpty(skinTextureChange.Value.ColorMetalFileName))
					{
						list2.Add(skinTextureChange.Value.ColorMetalFileName);
					}
					if (!string.IsNullOrEmpty(skinTextureChange.Value.NormalGlossFileName))
					{
						list3.Add(skinTextureChange.Value.NormalGlossFileName);
					}
					if (!string.IsNullOrEmpty(skinTextureChange.Value.ExtensionsFileName))
					{
						list4.Add(skinTextureChange.Value.ExtensionsFileName);
					}
					if (!string.IsNullOrEmpty(skinTextureChange.Value.AlphamaskFileName))
					{
						list5.Add(skinTextureChange.Value.AlphamaskFileName);
					}
				}
			}
			if (list2.Count > 0)
			{
				MyRenderProxy.PreloadTextures(list2, TextureType.ColorMetal);
			}
			if (list3.Count > 0)
			{
				MyRenderProxy.PreloadTextures(list3, TextureType.NormalGloss);
			}
			if (list4.Count > 0)
			{
				MyRenderProxy.PreloadTextures(list4, TextureType.Extensions);
			}
			if (list5.Count > 0)
			{
				MyRenderProxy.PreloadTextures(list5, TextureType.AlphaMask);
			}
		}

		public static void LoadInventoryConfig(MyCharacter character, bool setModel = true, bool setColor = true)
		{
			if (character == null)
			{
				throw new ArgumentNullException("character");
			}
			if (!MyGameService.IsActive)
			{
				return;
			}
			MyObjectBuilder_SkinInventory inventoryBuilder = GetInventoryBuilder(GetInventoryFile(character));
			if (inventoryBuilder == null)
			{
				ResetAllInventorySlots(character);
				return;
			}
			if (inventoryBuilder.Character != null && MyGameService.InventoryItems != null)
			{
				List<MyGameInventoryItem> list = new List<MyGameInventoryItem>();
				List<MyGameInventoryItemSlot> list2 = Enumerable.ToList<MyGameInventoryItemSlot>(Enumerable.Cast<MyGameInventoryItemSlot>((IEnumerable)Enum.GetValues(typeof(MyGameInventoryItemSlot))));
				list2.Remove(MyGameInventoryItemSlot.None);
				foreach (ulong itemId in inventoryBuilder.Character)
				{
					MyGameInventoryItem myGameInventoryItem = Enumerable.FirstOrDefault<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)MyGameService.InventoryItems, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.ID == itemId));
					if (myGameInventoryItem != null)
					{
						myGameInventoryItem.UsingCharacters.Add(character.EntityId);
						list.Add(myGameInventoryItem);
						list2.Remove(myGameInventoryItem.ItemDefinition.ItemSlot);
					}
				}
<<<<<<< HEAD
				foreach (ulong itemId2 in inventoryBuilder.Tools)
				{
					MyGameInventoryItem myGameInventoryItem2 = MyGameService.InventoryItems.FirstOrDefault((MyGameInventoryItem i) => i.ID == itemId2);
					if (myGameInventoryItem2 != null)
					{
						myGameInventoryItem2.UsingCharacters.Add(character.EntityId);
						list.Add(myGameInventoryItem2);
						list2.Remove(myGameInventoryItem2.ItemDefinition.ItemSlot);
					}
				}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (character.Components.TryGet<MyAssetModifierComponent>(out var comp))
				{
					MyGameService.GetItemsCheckData(list, delegate(byte[] checkDataResult)
					{
						comp.TryAddAssetModifier(checkDataResult);
					});
					foreach (MyGameInventoryItemSlot item in list2)
					{
						comp.ResetSlot(item);
					}
				}
			}
			else
			{
				ResetAllInventorySlots(character);
			}
			if (setModel && !string.IsNullOrEmpty(inventoryBuilder.Model))
			{
				character.ModelName = inventoryBuilder.Model;
			}
			if (setColor)
			{
				character.ColorMask = inventoryBuilder.Color;
			}
		}

		public static void ResetAllInventorySlots(MyCharacter character)
		{
			if (!character.Components.TryGet<MyAssetModifierComponent>(out var component))
<<<<<<< HEAD
			{
				return;
			}
			foreach (MyGameInventoryItemSlot value in Enum.GetValues(typeof(MyGameInventoryItemSlot)))
			{
=======
			{
				return;
			}
			foreach (MyGameInventoryItemSlot value in Enum.GetValues(typeof(MyGameInventoryItemSlot)))
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (value != 0)
				{
					component.ResetSlot(value);
				}
			}
		}

		public static void LoadInventoryConfig(MyCharacter character, MyEntity toolEntity, MyAssetModifierComponent skinComponent)
		{
			if (toolEntity == null)
			{
				throw new ArgumentNullException("toolEntity");
			}
			if (skinComponent == null)
			{
				throw new ArgumentNullException("skinComponent");
			}
			if (!MyGameService.IsActive)
			{
				return;
			}
			string inventoryFile = GetInventoryFile(character);
			if (!MyFileSystem.FileExists(inventoryFile) || !MyObjectBuilderSerializer.DeserializeXML(inventoryFile, out MyObjectBuilder_SkinInventory objectBuilder) || objectBuilder.Tools == null || MyGameService.InventoryItems == null)
			{
				return;
			}
			IMyHandheldGunObject<MyDeviceBase> myHandheldGunObject = toolEntity as IMyHandheldGunObject<MyDeviceBase>;
			MyPhysicalItemDefinition physicalItemDefinition = myHandheldGunObject.PhysicalItemDefinition;
			MyGameInventoryItemSlot myGameInventoryItemSlot = MyGameInventoryItemSlot.None;
			if (myHandheldGunObject is MyHandDrill)
			{
				myGameInventoryItemSlot = MyGameInventoryItemSlot.Drill;
			}
			else if (myHandheldGunObject is MyAutomaticRifleGun)
			{
				myGameInventoryItemSlot = MyGameInventoryItemSlot.Rifle;
			}
			else if (myHandheldGunObject is MyWelder)
			{
				myGameInventoryItemSlot = MyGameInventoryItemSlot.Welder;
			}
			else if (myHandheldGunObject is MyAngleGrinder)
			{
				myGameInventoryItemSlot = MyGameInventoryItemSlot.Grinder;
			}
			if (myGameInventoryItemSlot == MyGameInventoryItemSlot.None)
			{
				return;
			}
			List<MyGameInventoryItem> list = new List<MyGameInventoryItem>();
			foreach (ulong itemId in objectBuilder.Tools)
			{
<<<<<<< HEAD
				MyGameInventoryItem myGameInventoryItem = MyGameService.InventoryItems.FirstOrDefault((MyGameInventoryItem i) => i.ID == itemId);
=======
				MyGameInventoryItem myGameInventoryItem = Enumerable.FirstOrDefault<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)MyGameService.InventoryItems, (Func<MyGameInventoryItem, bool>)((MyGameInventoryItem i) => i.ID == itemId));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (myGameInventoryItem != null && physicalItemDefinition != null && (physicalItemDefinition == null || myGameInventoryItem.ItemDefinition.ItemSlot == myGameInventoryItemSlot))
				{
					myGameInventoryItem.UsingCharacters.Add(character.EntityId);
					list.Add(myGameInventoryItem);
				}
			}
			MyGameService.GetItemsCheckData(list, delegate(byte[] checkDataResult)
			{
				skinComponent.TryAddAssetModifier(checkDataResult);
			});
		}

		public static bool GetCharacterInfoFromInventoryConfig(ref string model, ref Color color)
		{
			if (!MyGameService.IsActive)
			{
				return false;
			}
			string path = Path.Combine(MyFileSystem.SavesPath, "ActiveInventory.sbl");
			if (!MyFileSystem.FileExists(path))
			{
				return false;
			}
			if (!MyObjectBuilderSerializer.DeserializeXML(path, out MyObjectBuilder_SkinInventory objectBuilder))
			{
				return false;
			}
			model = objectBuilder.Model;
			color = new Color(objectBuilder.Color.X, objectBuilder.Color.Y, objectBuilder.Color.Z);
			return true;
		}

		public static void SaveInventoryConfig(MyCharacter character)
		{
			if (character == null || !MyGameService.IsActive)
			{
				return;
			}
			MyObjectBuilder_SkinInventory myObjectBuilder_SkinInventory = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SkinInventory>();
			myObjectBuilder_SkinInventory.Character = new List<ulong>();
			myObjectBuilder_SkinInventory.Color = character.ColorMask;
			myObjectBuilder_SkinInventory.Model = character.ModelName;
			myObjectBuilder_SkinInventory.Tools = new List<ulong>();
			if (MyGameService.InventoryItems != null)
			{
				foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
				{
					if (inventoryItem.UsingCharacters.Contains(character.EntityId))
					{
						switch (inventoryItem.ItemDefinition.ItemSlot)
						{
						case MyGameInventoryItemSlot.None:
						case MyGameInventoryItemSlot.Face:
						case MyGameInventoryItemSlot.Helmet:
						case MyGameInventoryItemSlot.Gloves:
						case MyGameInventoryItemSlot.Boots:
						case MyGameInventoryItemSlot.Suit:
							myObjectBuilder_SkinInventory.Character.Add(inventoryItem.ID);
							break;
						case MyGameInventoryItemSlot.Rifle:
						case MyGameInventoryItemSlot.Welder:
						case MyGameInventoryItemSlot.Grinder:
						case MyGameInventoryItemSlot.Drill:
							myObjectBuilder_SkinInventory.Tools.Add(inventoryItem.ID);
							break;
						}
					}
				}
			}
			MyObjectBuilderSerializer.SerializeXML(GetInventoryFile(character), compress: false, myObjectBuilder_SkinInventory, out var _);
		}
	}
}
