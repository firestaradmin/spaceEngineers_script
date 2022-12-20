using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using ProtoBuf;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ModAPI;

namespace Sandbox.ModAPI
{
	public class MyAPIUtilities : IMyUtilities, IMyGamePaths
	{
		private const string STORAGE_FOLDER = "Storage";

		public static readonly MyAPIUtilities Static;

		private Dictionary<long, List<Action<object>>> m_registeredListeners = new Dictionary<long, List<Action<object>>>();

		public Dictionary<string, object> Variables = new Dictionary<string, object>();

		IMyConfigDedicated IMyUtilities.ConfigDedicated => MySandboxGame.ConfigDedicated;

		string IMyGamePaths.ContentPath => MyFileSystem.ContentPath;

		string IMyGamePaths.ModsPath => MyFileSystem.ModsPath;

		string IMyGamePaths.UserDataPath => MyFileSystem.UserDataPath;

		string IMyGamePaths.SavesPath => MyFileSystem.SavesPath;

		string IMyGamePaths.ModScopeName => StripDllExtIfNecessary(Assembly.GetCallingAssembly().ManifestModule.ScopeName);

		IMyGamePaths IMyUtilities.GamePaths => this;

		bool IMyUtilities.IsDedicated => Sandbox.Engine.Platform.Game.IsDedicated;

		public event MessageEnteredDel MessageEntered;

		public event MessageEnteredSenderDel MessageEnteredSender;

		public event Action<ulong, string> MessageRecieved;

		event MessageEnteredDel IMyUtilities.MessageEntered
		{
			add
			{
				MessageEntered += value;
			}
			remove
			{
				MessageEntered -= value;
			}
		}

		event Action<ulong, string> IMyUtilities.MessageRecieved
		{
			add
			{
				MessageRecieved += value;
			}
			remove
			{
				MessageRecieved -= value;
			}
		}

		static MyAPIUtilities()
		{
			Static = new MyAPIUtilities();
		}

		string IMyUtilities.GetTypeName(Type type)
		{
			return type.Name;
		}

		void IMyUtilities.ShowNotification(string message, int disappearTimeMs, string font)
		{
			MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.CustomText, disappearTimeMs, font);
			myHudNotification.SetTextFormatArguments(message);
			MyHud.Notifications.Add(myHudNotification);
		}

		IMyHudNotification IMyUtilities.CreateNotification(string message, int disappearTimeMs, string font)
		{
			MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.CustomText, disappearTimeMs, font);
			myHudNotification.SetTextFormatArguments(message);
			return myHudNotification;
		}

		void IMyUtilities.ShowMessage(string sender, string messageText)
		{
			MyHud.Chat.ShowMessage(sender, messageText);
		}

		void IMyUtilities.SendMessage(string messageText)
		{
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.SendChatMessage(messageText, ChatChannel.Global, 0L);
			}
		}

		public void EnterMessage(ulong sender, string messageText, ref bool sendToOthers)
		{
			this.MessageEntered?.Invoke(messageText, ref sendToOthers);
			this.MessageEnteredSender?.Invoke(sender, messageText, ref sendToOthers);
		}

		public void EnterMessageSender(ulong sender, string messageText, ref bool sendToOthers)
		{
			this.MessageEnteredSender?.Invoke(sender, messageText, ref sendToOthers);
		}

		public void RecieveMessage(ulong senderSteamId, string message)
		{
			this.MessageRecieved?.Invoke(senderSteamId, message);
		}

		private string StripDllExtIfNecessary(string name)
		{
			string text = ".dll";
			if (name.EndsWith(text, StringComparison.InvariantCultureIgnoreCase))
			{
				return name.Substring(0, name.Length - text.Length);
			}
			return name;
		}

		TextReader IMyUtilities.ReadFileInModLocation(string file, MyObjectBuilder_Checkpoint.ModItem modItem)
		{
			if (file.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string fullPath = Path.GetFullPath(Path.Combine(modItem.GetPath(), file));
			if (fullPath.StartsWith(modItem.GetPath()))
			{
				string text = Path.Combine(modItem.GetPath(), "Data", "Scripts");
				if (fullPath.StartsWith(text))
				{
					throw new FileNotFoundException("Access to protected location '" + text + "' not allowed.", fullPath);
				}
				Stream stream = MyFileSystem.OpenRead(fullPath);
				if (stream != null)
				{
					return new StreamReader(stream);
				}
			}
			throw new FileNotFoundException();
		}

		TextReader IMyUtilities.ReadFileInGameContent(string file)
		{
			if (file.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string fullPath = Path.GetFullPath(Path.Combine(MyFileSystem.ContentPath, file));
			if (fullPath.StartsWith(MyFileSystem.ContentPath))
			{
				Stream stream = MyFileSystem.OpenRead(fullPath);
				if (stream != null)
				{
					return new StreamReader(stream);
				}
			}
			throw new FileNotFoundException();
		}

		TextReader IMyUtilities.ReadFileInGlobalStorage(string file)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string text = Path.Combine(MyFileSystem.UserDataPath, "Storage");
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				Stream stream = MyFileSystem.OpenRead(path);
				if (stream != null)
				{
					return new StreamReader(stream);
				}
			}
			throw new FileNotFoundException();
		}

		TextReader IMyUtilities.ReadFileInLocalStorage(string file, Type callingType)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string text = Path.Combine(MyFileSystem.UserDataPath, "Storage", StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName));
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				Stream stream = MyFileSystem.OpenRead(path);
				if (stream != null)
				{
					return new StreamReader(stream);
				}
			}
			throw new FileNotFoundException();
		}

		TextReader IMyUtilities.ReadFileInWorldStorage(string file, Type callingType)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string text = Path.Combine(MySession.Static.CurrentPath, "Storage", StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName));
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				Stream stream = MyFileSystem.OpenRead(path);
				if (stream != null)
				{
					return new StreamReader(stream);
				}
			}
			throw new FileNotFoundException();
		}

		TextWriter IMyUtilities.WriteFileInGlobalStorage(string file)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string text = Path.Combine(MyFileSystem.UserDataPath, "Storage");
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				Stream stream = MyFileSystem.OpenWrite(path);
				if (stream != null)
				{
					return new StreamWriter(stream);
				}
			}
			throw new FileNotFoundException();
		}

		TextWriter IMyUtilities.WriteFileInLocalStorage(string file, Type callingType)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string text = Path.Combine(MyFileSystem.UserDataPath, "Storage", StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName));
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				Stream stream = MyFileSystem.OpenWrite(path);
				if (stream != null)
				{
					return new StreamWriter(stream);
				}
			}
			throw new FileNotFoundException();
		}

		TextWriter IMyUtilities.WriteFileInWorldStorage(string file, Type callingType)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string text = Path.Combine(MySession.Static.CurrentPath, "Storage", StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName));
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				Stream stream = MyFileSystem.OpenWrite(path);
				if (stream != null)
				{
					return new StreamWriter(stream);
				}
			}
			throw new FileNotFoundException();
		}

		string IMyUtilities.SerializeToXML<T>(T objToSerialize)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			XmlSerializer val = new XmlSerializer(objToSerialize.GetType());
			StringWriter val2 = new StringWriter();
			val.Serialize((TextWriter)(object)val2, (object)objToSerialize);
			return ((object)val2).ToString();
		}

		T IMyUtilities.SerializeFromXML<T>(string xml)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			if (string.IsNullOrEmpty(xml))
			{
				return default(T);
			}
			XmlSerializer val = new XmlSerializer(typeof(T));
			StringReader val2 = new StringReader(xml);
			try
			{
				XmlReader val3 = XmlReader.Create((TextReader)(object)val2);
				try
				{
					return (T)val.Deserialize(val3);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}

		byte[] IMyUtilities.SerializeToBinary<T>(T obj)
		{
			using MemoryStream memoryStream = new MemoryStream();
			Serializer.Serialize((Stream)memoryStream, obj);
			return memoryStream.ToArray();
		}

		T IMyUtilities.SerializeFromBinary<T>(byte[] data)
		{
			using MemoryStream source = new MemoryStream(data);
			return Serializer.Deserialize<T>(source);
		}

		void IMyUtilities.InvokeOnGameThread(Action action, string invokerName, int StartAt, int RepeatTimes)
		{
			if (MySandboxGame.Static != null)
			{
				MySandboxGame.Static.Invoke(action, invokerName, StartAt, RepeatTimes);
			}
		}

		bool IMyUtilities.FileExistsInModLocation(string file, MyObjectBuilder_Checkpoint.ModItem modItem)
		{
			if (file.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				return false;
			}
			string fullPath = Path.GetFullPath(Path.Combine(modItem.GetPath(), file));
			if (fullPath.StartsWith(modItem.GetPath()))
			{
				string value = Path.Combine(modItem.GetPath(), "Data", "Scripts");
				if (fullPath.StartsWith(value))
				{
					return false;
				}
				return File.Exists(fullPath);
			}
			return false;
		}

		bool IMyUtilities.FileExistsInGameContent(string file)
		{
			if (file.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				return false;
			}
			string fullPath = Path.GetFullPath(Path.Combine(MyFileSystem.ContentPath, file));
			if (fullPath.StartsWith(MyFileSystem.ContentPath))
			{
				return File.Exists(fullPath);
			}
			return false;
		}

		bool IMyUtilities.FileExistsInGlobalStorage(string file)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				return false;
			}
			string text = Path.Combine(MyFileSystem.UserDataPath, "Storage");
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				return File.Exists(path);
			}
			return false;
		}

		bool IMyUtilities.FileExistsInLocalStorage(string file, Type callingType)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				return false;
			}
			string text = Path.Combine(MyFileSystem.UserDataPath, "Storage", StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName));
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				return File.Exists(path);
			}
			return false;
		}

		bool IMyUtilities.FileExistsInWorldStorage(string file, Type callingType)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				return false;
			}
			string text = Path.Combine(MySession.Static.CurrentPath, "Storage", StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName));
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				return File.Exists(path);
			}
			return false;
		}

		void IMyUtilities.DeleteFileInLocalStorage(string file, Type callingType)
		{
			if (((IMyUtilities)this).FileExistsInLocalStorage(file, callingType))
			{
				string text = Path.Combine(MyFileSystem.UserDataPath, "Storage", StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName));
				string path = Path.Combine(text, file);
				if (Path.GetFullPath(path).StartsWith(text))
				{
					File.Delete(path);
				}
			}
		}

		void IMyUtilities.DeleteFileInWorldStorage(string file, Type callingType)
		{
			if (((IMyUtilities)this).FileExistsInLocalStorage(file, callingType))
			{
				string text = Path.Combine(MySession.Static.CurrentPath, "Storage", StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName));
				string path = Path.Combine(text, file);
				if (Path.GetFullPath(path).StartsWith(text))
				{
					File.Delete(path);
				}
			}
		}

		void IMyUtilities.DeleteFileInGlobalStorage(string file)
		{
			if (((IMyUtilities)this).FileExistsInGlobalStorage(file))
			{
				string text = Path.Combine(MyFileSystem.UserDataPath, "Storage");
				string path = Path.Combine(text, file);
				if (Path.GetFullPath(path).StartsWith(text))
				{
					File.Delete(path);
				}
			}
		}

		void IMyUtilities.ShowMissionScreen(string screenTitle, string currentObjectivePrefix, string currentObjective, string screenDescription, Action<ResultEnum> callback, string okButtonCaption)
		{
			MyScreenManager.AddScreen(new MyGuiScreenMission(screenTitle, currentObjectivePrefix, currentObjective, screenDescription, callback, okButtonCaption));
		}

		IMyHudObjectiveLine IMyUtilities.GetObjectiveLine()
		{
			return MyHud.ObjectiveLine;
		}

		BinaryReader IMyUtilities.ReadBinaryFileInModLocation(string file, MyObjectBuilder_Checkpoint.ModItem modItem)
		{
			if (file.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string fullPath = Path.GetFullPath(Path.Combine(modItem.GetPath(), file));
			if (fullPath.StartsWith(modItem.GetPath()))
			{
				string text = Path.Combine(modItem.GetPath(), "Data", "Scripts");
				if (fullPath.StartsWith(text))
				{
					throw new FileNotFoundException("Access to protected location '" + text + "' not allowed.", fullPath);
				}
				Stream stream = MyFileSystem.OpenRead(fullPath);
				if (stream != null)
				{
					return new BinaryReader(stream);
				}
			}
			throw new FileNotFoundException();
		}

		BinaryReader IMyUtilities.ReadBinaryFileInGameContent(string file)
		{
			if (file.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string fullPath = Path.GetFullPath(Path.Combine(MyFileSystem.ContentPath, file));
			if (fullPath.StartsWith(MyFileSystem.ContentPath))
			{
				Stream stream = MyFileSystem.OpenRead(fullPath);
				if (stream != null)
				{
					return new BinaryReader(stream);
				}
			}
			throw new FileNotFoundException();
		}

		BinaryReader IMyUtilities.ReadBinaryFileInGlobalStorage(string file)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string text = Path.Combine(MyFileSystem.UserDataPath, "Storage");
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				Stream stream = MyFileSystem.OpenRead(path);
				if (stream != null)
				{
					return new BinaryReader(stream);
				}
			}
			throw new FileNotFoundException();
		}

		BinaryReader IMyUtilities.ReadBinaryFileInLocalStorage(string file, Type callingType)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string text = Path.Combine(MyFileSystem.UserDataPath, "Storage", StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName));
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				Stream stream = MyFileSystem.OpenRead(path);
				if (stream != null)
				{
					return new BinaryReader(stream);
				}
			}
			throw new FileNotFoundException();
		}

		BinaryReader IMyUtilities.ReadBinaryFileInWorldStorage(string file, Type callingType)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string text = Path.Combine(MySession.Static.CurrentPath, "Storage", StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName));
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				Stream stream = MyFileSystem.OpenRead(path);
				if (stream != null)
				{
					return new BinaryReader(stream);
				}
			}
			throw new FileNotFoundException();
		}

		BinaryWriter IMyUtilities.WriteBinaryFileInGlobalStorage(string file)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string text = Path.Combine(MyFileSystem.UserDataPath, "Storage");
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				Stream stream = MyFileSystem.OpenWrite(path);
				if (stream != null)
				{
					return new BinaryWriter(stream);
				}
			}
			throw new FileNotFoundException();
		}

		BinaryWriter IMyUtilities.WriteBinaryFileInLocalStorage(string file, Type callingType)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string text = Path.Combine(MyFileSystem.UserDataPath, "Storage", StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName));
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				Stream stream = MyFileSystem.OpenWrite(path);
				if (stream != null)
				{
					return new BinaryWriter(stream);
				}
			}
			throw new FileNotFoundException();
		}

		BinaryWriter IMyUtilities.WriteBinaryFileInWorldStorage(string file, Type callingType)
		{
			if (file.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				throw new FileNotFoundException();
			}
			string text = Path.Combine(MySession.Static.CurrentPath, "Storage", StripDllExtIfNecessary(callingType.Assembly.ManifestModule.ScopeName));
			string path = Path.Combine(text, file);
			if (Path.GetFullPath(path).StartsWith(text))
			{
				Stream stream = MyFileSystem.OpenWrite(path);
				if (stream != null)
				{
					return new BinaryWriter(stream);
				}
			}
			throw new FileNotFoundException();
		}

		void IMyUtilities.SetVariable<T>(string name, T value)
		{
			Variables.Remove(name);
			Variables.Add(name, value);
		}

		bool IMyUtilities.GetVariable<T>(string name, out T value)
		{
			value = default(T);
			if (Variables.TryGetValue(name, out var value2) && value2 is T)
			{
				value = (T)value2;
				return true;
			}
			return false;
		}

		bool IMyUtilities.RemoveVariable(string name)
		{
			return Variables.Remove(name);
		}

		public void RegisterMessageHandler(long id, Action<object> messageHandler)
		{
			if (m_registeredListeners.TryGetValue(id, out var value))
			{
				value.Add(messageHandler);
				return;
			}
			m_registeredListeners[id] = new List<Action<object>> { messageHandler };
		}

		public void UnregisterMessageHandler(long id, Action<object> messageHandler)
		{
			if (m_registeredListeners.TryGetValue(id, out var value))
			{
				value.Remove(messageHandler);
			}
		}

		public void SendModMessage(long id, object payload)
		{
			if (!m_registeredListeners.TryGetValue(id, out var value))
			{
				return;
			}
			foreach (Action<object> item in value)
			{
				item(payload);
			}
		}
	}
}
