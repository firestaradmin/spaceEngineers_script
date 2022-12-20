using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using EmptyKeys.UserInterface.Mvvm;
using ParallelTasks;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Collections;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Engine.Networking
{
	public static class MyWorkshop
	{
		public struct Category
		{
			public string Id;

			public MyStringId LocalizableName;

			public bool IsVisibleForFilter;
		}

		public struct MyWorkshopPathInfo
		{
			public string Path;

			public string Suffix;

			public string NamePrefix;

			public static MyWorkshopPathInfo CreateWorldInfo()
			{
				MyWorkshopPathInfo result = default(MyWorkshopPathInfo);
				result.Path = m_workshopWorldsPath;
				result.Suffix = m_workshopWorldSuffix;
				result.NamePrefix = "Workshop";
				return result;
			}

			public static MyWorkshopPathInfo CreateScenarioInfo()
			{
				MyWorkshopPathInfo result = default(MyWorkshopPathInfo);
				result.Path = m_workshopScenariosPath;
				result.Suffix = m_workshopScenariosSuffix;
				result.NamePrefix = "Scenario";
				return result;
			}
		}

		private class CreateWorldResult : IMyAsyncResult
		{
			public string m_createdSessionPath;

			public Task Task { get; private set; }

			public bool Success { get; private set; }

			public Action<bool, string> Callback { get; private set; }

			public bool IsCompleted => Task.IsComplete;

			public CreateWorldResult(MyWorkshopItem world, MyWorkshopPathInfo pathInfo, Action<bool, string> callback, bool overwrite)
			{
				CreateWorldResult createWorldResult = this;
				Callback = callback;
				Task = Parallel.Start(delegate
				{
					createWorldResult.Success = TryCreateWorldInstanceBlocking(world, pathInfo, out createWorldResult.m_createdSessionPath, overwrite);
				});
			}
		}

		private class UpdateWorldsResult : IMyAsyncResult
		{
			public Task Task { get; private set; }

			public bool Success { get; private set; }

			public Action<bool> Callback { get; private set; }

			public bool IsCompleted => Task.IsComplete;

			public UpdateWorldsResult(List<MyWorkshopItem> worlds, MyWorkshopPathInfo pathInfo, Action<bool> callback)
			{
				UpdateWorldsResult updateWorldsResult = this;
				Callback = callback;
				Task = Parallel.Start(delegate
				{
					updateWorldsResult.Success = TryUpdateWorldsBlocking(worlds, pathInfo);
				});
			}
		}

		private class PublishItemResult : IMyAsyncResult
		{
			public MyGameServiceCallResult Result;

			public string ResultServiceName;

			public MyWorkshopItem[] PublishedItems;

			public Task Task { get; private set; }

			public bool IsCompleted => Task.IsComplete;

			public PublishItemResult(string localFolder, string publishedTitle, string publishedDescription, WorkshopId[] workshopIds, MyPublishedFileVisibility visibility, string[] tags, HashSet<string> ignoredExtensions, HashSet<string> ignoredPaths = null, uint[] requiredDLCs = null)
			{
				PublishItemResult publishItemResult = this;
				Task = Parallel.Start(delegate
				{
					MyWorkshopItem[] publishedItems;
					(MyGameServiceCallResult, string) tuple = PublishItemBlocking(localFolder, publishedTitle, publishedDescription, workshopIds, visibility, tags, ignoredExtensions, ignoredPaths, requiredDLCs, out publishedItems);
					publishItemResult.PublishedItems = publishedItems;
					publishItemResult.Result = tuple.Item1;
					publishItemResult.ResultServiceName = tuple.Item2;
				});
			}
		}

		public struct ResultData
		{
			public bool Success;

			public bool Cancel;

			public List<MyWorkshopItem> Mods;

			public List<MyWorkshopItem> MismatchMods;

			public ResultData(bool success, bool cancel)
			{
				Success = success;
				Cancel = cancel;
				Mods = new List<MyWorkshopItem>();
				MismatchMods = new List<MyWorkshopItem>();
			}
		}

		private class DownloadModsResult : IMyAsyncResult
		{
			public ResultData Result;

			public Action<bool> Callback;

			public Task Task { get; private set; }

			public bool IsCompleted => Task.IsComplete;

			public DownloadModsResult(List<MyObjectBuilder_Checkpoint.ModItem> mods, Action<bool> onFinishedCallback, CancelToken cancelToken)
			{
				DownloadModsResult downloadModsResult = this;
				Callback = onFinishedCallback;
				Task = Parallel.Start(delegate
				{
					downloadModsResult.Result = DownloadWorldModsBlockingInternal(mods, cancelToken);
					if (!downloadModsResult.Result.Cancel)
					{
						MySandboxGame.Static.Invoke(endActionDownloadMods, "DownloadModsResult::endActionDownloadMods");
					}
				});
			}
		}

		public class CancelToken
		{
			public bool Cancel;
		}

		private const int MOD_NAME_LIMIT = 25;

		private const string MOD_INFO_FILE = "modinfo.sbmi";

		private static MyWorkshopItemPublisher m_publisher;

		private static MyGuiScreenDownloadMods m_downloadScreen;

		private static DownloadModsResult m_downloadResult;

		private static readonly int m_syncUpdateTimeout = 500;

		private static readonly int m_dependenciesRequestTimeout = 30000;

		private static readonly string m_workshopWorldsDir = "WorkshopWorlds";

		private static readonly string m_workshopWorldsPath = Path.Combine(MyFileSystem.UserDataPath, m_workshopWorldsDir);

		private static readonly string m_workshopWorldSuffix = ".sbw";

		private static readonly string m_workshopBlueprintsPath = Path.Combine(MyFileSystem.UserDataPath, "Blueprints", "workshop");

		private static readonly string m_workshopScriptPath = Path.Combine(MyFileSystem.UserDataPath, "IngameScripts", "workshop");

		private static readonly string m_workshopModsPath = MyFileSystem.ModsPath;

		public static readonly string WorkshopModSuffix = "_legacy.bin";

		private static readonly string m_workshopScenariosPath = Path.Combine(MyFileSystem.UserDataPath, "Scenarios", "workshop");

		private static readonly string m_workshopScenariosSuffix = ".sbs";

		private static readonly string[] m_previewFileNames = new string[2]
		{
			"thumb.png",
			MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION
		};

		private const string ModMetadataFileName = "metadata.mod";

<<<<<<< HEAD
		private static readonly HashSet<string> m_ignoredExecutableExtensions = new HashSet<string>(new string[48]
=======
		private static readonly HashSet<string> m_ignoredExecutableExtensions = new HashSet<string>((IEnumerable<string>)new string[48]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			".action", ".apk", ".app", ".bat", ".bin", ".cmd", ".com", ".command", ".cpl", ".csh",
			".dll", ".exe", ".gadget", ".inf1", ".ins", ".inx", ".ipa", ".isu", ".job", ".jse",
			".ksh", ".lnk", ".msc", ".msi", ".msp", ".mst", ".osx", ".out", ".pif", ".paf",
			".prg", ".ps1", ".reg", ".rgs", ".run", ".sct", ".shb", ".shs", ".so", ".u3p",
			".vb", ".vbe", ".vbs", ".vbscript", ".workflow", ".ws", ".wsf", ".suo"
		});

<<<<<<< HEAD
		private static readonly HashSet<string> m_scriptExtensions = new HashSet<string> { ".cs" };

		private static readonly int m_bufferSize = 1048576;
=======
		private static readonly HashSet<string> m_scriptExtensions;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private static readonly int m_bufferSize;

		private static byte[] buffer;

		private static Category[] m_modCategories;

		private static Category[] m_worldCategories;

		private static Category[] m_blueprintCategories;

		private static Category[] m_scenarioCategories;

		private static Category[] m_scriptCategories;

<<<<<<< HEAD
		/// <summary>
		/// Do NOT change this value, as it would break worlds published to workshop!!!
		/// Tag for workshop items which contain world data.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public const string WORKSHOP_DEVELOPMENT_TAG = "development";

		public const string WORKSHOP_WORLD_TAG = "world";

		public const string WORKSHOP_CAMPAIGN_TAG = "campaign";

		public const string WORKSHOP_MOD_TAG = "mod";

		public const string WORKSHOP_BLUEPRINT_TAG = "blueprint";

		public const string WORKSHOP_SCENARIO_TAG = "scenario";

		public const string WORKSHOP_INGAMESCRIPT_TAG = "ingameScript";
<<<<<<< HEAD

		private static FastResourceLock m_modLock = new FastResourceLock();

		private static string ContentTag;

		private static Action SubscriptionChangedCallback;

=======

		private static FastResourceLock m_modLock;

		private static string ContentTag;

		private static Action SubscriptionChangedCallback;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> m_onPublishingFinished;

		private static MyGuiScreenProgressAsync m_asyncPublishScreen;

		private static CancelToken m_cancelTokenDownloadMods;

		public static Category[] ModCategories => m_modCategories;

		public static Category[] WorldCategories => m_worldCategories;

		public static Category[] BlueprintCategories => m_blueprintCategories;

		public static Category[] ScenarioCategories => m_scenarioCategories;

		public static Category[] ScriptCategories => m_scriptCategories;

		public static void Init(Category[] modCategories, Category[] worldCategories, Category[] blueprintCategories, Category[] scenarioCategories, Category[] scriptCategories)
		{
			m_modCategories = modCategories;
			m_worldCategories = worldCategories;
			m_blueprintCategories = blueprintCategories;
			m_scenarioCategories = scenarioCategories;
			m_scriptCategories = scriptCategories;
		}

		public static void PublishModAsync(string localModFolder, string publishedTitle, string publishedDescription, WorkshopId[] workshopIds, string[] tags, MyPublishedFileVisibility visibility, Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> callbackOnFinished = null)
		{
			m_onPublishingFinished = callbackOnFinished;
			HashSet<string> ignoredPaths = new HashSet<string>();
			ignoredPaths.Add("modinfo.sbmi");
			MyGuiSandbox.AddScreen(m_asyncPublishScreen = new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextUploadingWorld, null, () => new PublishItemResult(localModFolder, publishedTitle, publishedDescription, workshopIds, visibility, tags, m_ignoredExecutableExtensions, ignoredPaths), endActionPublish));
		}

		public static WorkshopId[] GetWorkshopIdFromLocalMod(string localModFolder, WorkshopId additionalWorkshopId)
		{
			return GetWorkshopIdFromLocalModInternal(Path.Combine(MyFileSystem.ModsPath, localModFolder, "modinfo.sbmi"), additionalWorkshopId);
		}

		public static WorkshopId[] GetWorkshopIdFromMod(string modFolder)
		{
			return GetWorkshopIdFromLocalModInternal(Path.Combine(modFolder, "modinfo.sbmi"), default(WorkshopId));
		}

		public static WorkshopId[] GetWorkshopIdFromLocalBlueprint(string localFolder, WorkshopId additionalWorkshopId)
		{
			return GetWorkshopIdFromLocalModInternal(Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, localFolder, "modinfo.sbmi"), additionalWorkshopId);
		}

		public static WorkshopId[] GetWorkshopIdFromLocalScript(string localFolder, WorkshopId additionalWorkshopId)
		{
			return GetWorkshopIdFromLocalModInternal(Path.Combine(MyBlueprintUtils.SCRIPT_FOLDER_LOCAL, localFolder, "modinfo.sbmi"), additionalWorkshopId);
		}

		private static WorkshopId[] GetWorkshopIdFromLocalModInternal(string modInfoPath, WorkshopId additionalWorkshopId)
		{
			if (File.Exists(modInfoPath) && MyObjectBuilderSerializer.DeserializeXML(modInfoPath, out MyObjectBuilder_ModInfo objectBuilder))
<<<<<<< HEAD
			{
				if (objectBuilder.WorkshopIds != null)
				{
					if (additionalWorkshopId.Id != 0L)
					{
						List<WorkshopId> list = objectBuilder.WorkshopIds.ToList();
						int num = list.FindIndex((WorkshopId x) => x.ServiceName == additionalWorkshopId.ServiceName);
						if (num != -1)
						{
							list[num] = additionalWorkshopId;
						}
						else
						{
							list.Add(additionalWorkshopId);
						}
						return list.ToArray();
					}
					return objectBuilder.WorkshopIds;
				}
				if (objectBuilder.WorkshopId != 0L)
				{
					string serviceName = MyGameService.GetDefaultUGC().ServiceName;
					if (additionalWorkshopId.Id != 0L)
					{
						if (!(additionalWorkshopId.ServiceName == serviceName))
						{
							return new WorkshopId[2]
							{
								new WorkshopId
								{
									Id = objectBuilder.WorkshopId,
									ServiceName = MyGameService.GetDefaultUGC().ServiceName
								},
								additionalWorkshopId
							};
						}
						return new WorkshopId[1] { additionalWorkshopId };
					}
					return new WorkshopId[1]
					{
						new WorkshopId(objectBuilder.WorkshopId, MyGameService.GetDefaultUGC().ServiceName)
					};
				}
			}
			if (additionalWorkshopId.Id != 0L)
			{
=======
			{
				if (objectBuilder.WorkshopIds != null)
				{
					if (additionalWorkshopId.Id != 0L)
					{
						List<WorkshopId> list = Enumerable.ToList<WorkshopId>((IEnumerable<WorkshopId>)objectBuilder.WorkshopIds);
						int num = list.FindIndex((WorkshopId x) => x.ServiceName == additionalWorkshopId.ServiceName);
						if (num != -1)
						{
							list[num] = additionalWorkshopId;
						}
						else
						{
							list.Add(additionalWorkshopId);
						}
						return list.ToArray();
					}
					return objectBuilder.WorkshopIds;
				}
				if (objectBuilder.WorkshopId != 0L)
				{
					string serviceName = MyGameService.GetDefaultUGC().ServiceName;
					if (additionalWorkshopId.Id != 0L)
					{
						if (!(additionalWorkshopId.ServiceName == serviceName))
						{
							return new WorkshopId[2]
							{
								new WorkshopId
								{
									Id = objectBuilder.WorkshopId,
									ServiceName = MyGameService.GetDefaultUGC().ServiceName
								},
								additionalWorkshopId
							};
						}
						return new WorkshopId[1] { additionalWorkshopId };
					}
					return new WorkshopId[1]
					{
						new WorkshopId(objectBuilder.WorkshopId, MyGameService.GetDefaultUGC().ServiceName)
					};
				}
			}
			if (additionalWorkshopId.Id != 0L)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return new WorkshopId[1] { additionalWorkshopId };
			}
			return new WorkshopId[0];
		}

		public static void PublishWorldAsync(string localWorldFolder, string publishedTitle, string publishedDescription, WorkshopId[] workshopIds, string[] tags, MyPublishedFileVisibility visibility, Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> callbackOnFinished = null)
		{
			m_onPublishingFinished = callbackOnFinished;
<<<<<<< HEAD
			HashSet<string> ignoredExtensions = new HashSet<string>(m_ignoredExecutableExtensions);
=======
			HashSet<string> ignoredExtensions = new HashSet<string>((IEnumerable<string>)m_ignoredExecutableExtensions);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ignoredExtensions.Add(".xmlcache");
			ignoredExtensions.Add(".png");
			HashSet<string> ignoredPaths = new HashSet<string>();
			ignoredPaths.Add("Backup");
			MyGuiSandbox.AddScreen(m_asyncPublishScreen = new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextUploadingWorld, null, () => new PublishItemResult(localWorldFolder, publishedTitle, publishedDescription, workshopIds, visibility, tags, ignoredExtensions, ignoredPaths), endActionPublish));
		}

		public static void PublishBlueprintAsync(string localWorldFolder, string publishedTitle, string publishedDescription, WorkshopId[] workshopIds, string[] tags, uint[] requiredDLCs, MyPublishedFileVisibility visibility, Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> callbackOnFinished = null)
		{
			m_onPublishingFinished = callbackOnFinished;
			MyGuiSandbox.AddScreen(m_asyncPublishScreen = new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextUploadingWorld, null, () => new PublishItemResult(localWorldFolder, publishedTitle, publishedDescription, workshopIds, visibility, tags, m_ignoredExecutableExtensions, null, requiredDLCs), endActionPublish));
		}

		public static void PublishScenarioAsync(string localWorldFolder, string publishedTitle, string publishedDescription, WorkshopId[] workshopIds, MyPublishedFileVisibility visibility, Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> callbackOnFinished = null)
		{
			m_onPublishingFinished = callbackOnFinished;
			string[] tags = new string[1] { "scenario" };
			MyGuiSandbox.AddScreen(m_asyncPublishScreen = new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextUploadingWorld, null, () => new PublishItemResult(localWorldFolder, publishedTitle, publishedDescription, workshopIds, visibility, tags, m_ignoredExecutableExtensions), endActionPublish));
		}

		public static void PublishIngameScriptAsync(string localWorldFolder, string publishedTitle, string publishedDescription, WorkshopId[] workshopIds, string[] tags, MyPublishedFileVisibility visibility, Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> callbackOnFinished = null)
		{
			m_onPublishingFinished = callbackOnFinished;
<<<<<<< HEAD
			HashSet<string> ignoredExtensions = new HashSet<string>(m_ignoredExecutableExtensions);
=======
			HashSet<string> ignoredExtensions = new HashSet<string>((IEnumerable<string>)m_ignoredExecutableExtensions);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ignoredExtensions.Add(".sbmi");
			MyGuiSandbox.AddScreen(m_asyncPublishScreen = new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextUploadingWorld, null, () => new PublishItemResult(localWorldFolder, publishedTitle, publishedDescription, workshopIds, visibility, tags, ignoredExtensions), endActionPublish));
		}

<<<<<<< HEAD
		/// <summary>
		/// Do NOT call this method from update thread. Use PublishWorldAsync or worker thread, otherwise it will block update.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static (MyGameServiceCallResult, string) PublishItemBlocking(string localFolder, string publishedTitle, string publishedDescription, WorkshopId[] workshopIds, MyPublishedFileVisibility visibility, string[] tags, HashSet<string> ignoredExtensions, HashSet<string> ignoredPaths, uint[] requiredDLCs, out MyWorkshopItem[] publishedItems)
		{
			MySandboxGame.Log.WriteLine("PublishItemBlocking - START");
			MySandboxGame.Log.IncreaseIndent();
			publishedItems = Array.Empty<MyWorkshopItem>();
			if (tags.Length == 0)
			{
				MySandboxGame.Log.WriteLine("Error: Can not publish with no tags!");
				MySandboxGame.Log.DecreaseIndent();
				MySandboxGame.Log.WriteLine("PublishItemBlocking - END");
				return (MyGameServiceCallResult.InvalidParam, null);
			}
			if (!MyGameService.IsActive && !MyGameService.IsOnline)
			{
				return (MyGameServiceCallResult.NoUser, null);
			}
			AutoResetEvent resetEvent = new AutoResetEvent(initialState: false);
			try
			{
				bool publishPossible = false;
				MyGameService.Service.RequestPermissions(Permissions.ShareContent, attemptResolution: true, delegate(bool granted)
				{
					if (granted)
					{
						MyGameService.Service.RequestPermissions(Permissions.UGC, attemptResolution: true, delegate(bool ugcGranted)
						{
							publishPossible = ugcGranted;
							resetEvent.Set();
						});
					}
					else
					{
						publishPossible = false;
						resetEvent.Set();
					}
				});
				resetEvent.WaitOne();
				if (!publishPossible)
				{
					return (MyGameServiceCallResult.PlatformPublishRestricted, null);
				}
<<<<<<< HEAD
			}
			finally
			{
				if (resetEvent != null)
				{
					((IDisposable)resetEvent).Dispose();
				}
			}
			List<MyWorkshopItem> list = new List<MyWorkshopItem>();
			(MyGameServiceCallResult, string) result = (MyGameServiceCallResult.OK, "");
			for (int i = 0; i < workshopIds.Length; i++)
			{
				WorkshopId workshopId = workshopIds[i];
				MyWorkshopItem publishedItem;
				MyGameServiceCallResult myGameServiceCallResult = PublishItemBlocking(localFolder, publishedTitle, publishedDescription, workshopId.Id, workshopId.ServiceName, visibility, tags, ignoredExtensions, ignoredPaths, requiredDLCs, out publishedItem);
				if (publishedItem != null)
				{
					list.Add(publishedItem);
				}
				if (myGameServiceCallResult != MyGameServiceCallResult.OK)
				{
					result = (myGameServiceCallResult, workshopId.ServiceName);
				}
			}
			if (list.Count > 0)
			{
				publishedItems = list.ToArray();
			}
			return result;
		}

		private static MyGameServiceCallResult PublishItemBlocking(string localFolder, string publishedTitle, string publishedDescription, ulong workshopId, string serviceName, MyPublishedFileVisibility visibility, string[] tags, HashSet<string> ignoredExtensions, HashSet<string> ignoredPaths, uint[] requiredDLCs, out MyWorkshopItem publishedItem)
		{
			publishedItem = null;
			MyWorkshopItemPublisher myWorkshopItemPublisher = MyGameService.CreateWorkshopPublisher(serviceName);
			if (myWorkshopItemPublisher == null)
			{
				return MyGameServiceCallResult.ServiceUnavailable;
			}
=======
			}
			finally
			{
				if (resetEvent != null)
				{
					((IDisposable)resetEvent).Dispose();
				}
			}
			List<MyWorkshopItem> list = new List<MyWorkshopItem>();
			(MyGameServiceCallResult, string) result = (MyGameServiceCallResult.OK, "");
			for (int i = 0; i < workshopIds.Length; i++)
			{
				WorkshopId workshopId = workshopIds[i];
				MyWorkshopItem publishedItem;
				MyGameServiceCallResult myGameServiceCallResult = PublishItemBlocking(localFolder, publishedTitle, publishedDescription, workshopId.Id, workshopId.ServiceName, visibility, tags, ignoredExtensions, ignoredPaths, requiredDLCs, out publishedItem);
				if (publishedItem != null)
				{
					list.Add(publishedItem);
				}
				if (myGameServiceCallResult != MyGameServiceCallResult.OK)
				{
					result = (myGameServiceCallResult, workshopId.ServiceName);
				}
			}
			if (list.Count > 0)
			{
				publishedItems = list.ToArray();
			}
			return result;
		}

		private static MyGameServiceCallResult PublishItemBlocking(string localFolder, string publishedTitle, string publishedDescription, ulong workshopId, string serviceName, MyPublishedFileVisibility visibility, string[] tags, HashSet<string> ignoredExtensions, HashSet<string> ignoredPaths, uint[] requiredDLCs, out MyWorkshopItem publishedItem)
		{
			publishedItem = null;
			MyWorkshopItemPublisher myWorkshopItemPublisher = MyGameService.CreateWorkshopPublisher(serviceName);
			if (myWorkshopItemPublisher == null)
			{
				return MyGameServiceCallResult.ServiceUnavailable;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (workshopId != 0L)
			{
				List<MyWorkshopItem> list = new List<MyWorkshopItem>();
				if (GetItemsBlockingUGC(new List<WorkshopId>
				{
					new WorkshopId(workshopId, serviceName)
				}, list))
				{
					if (list.Count != 0)
<<<<<<< HEAD
					{
						MyWorkshopItem myWorkshopItem = list.FirstOrDefault((MyWorkshopItem wi) => wi.Id == workshopId);
						if (myWorkshopItem != null)
						{
							publishedTitle = myWorkshopItem.Title;
							visibility = myWorkshopItem.Visibility;
						}
					}
					else
					{
=======
					{
						MyWorkshopItem myWorkshopItem = Enumerable.FirstOrDefault<MyWorkshopItem>((IEnumerable<MyWorkshopItem>)list, (Func<MyWorkshopItem, bool>)((MyWorkshopItem wi) => wi.Id == workshopId));
						if (myWorkshopItem != null)
						{
							publishedTitle = myWorkshopItem.Title;
						}
					}
					else
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						workshopId = 0uL;
					}
				}
				else
				{
					workshopId = 0uL;
				}
			}
			myWorkshopItemPublisher.Title = publishedTitle;
			myWorkshopItemPublisher.Description = publishedDescription;
			myWorkshopItemPublisher.Visibility = visibility;
			myWorkshopItemPublisher.Tags = new List<string>(tags);
			myWorkshopItemPublisher.Id = workshopId;
			string filename = Path.Combine(localFolder, "metadata.mod");
			MyModMetadata mod = MyModMetadataLoader.Load(filename);
			CheckAndFixModMetadata(ref mod);
			MyModMetadataLoader.Save(filename, mod);
			bool flag = CheckModFolder(ref localFolder, ignoredExtensions, ignoredPaths);
			myWorkshopItemPublisher.Folder = localFolder;
			myWorkshopItemPublisher.Metadata = mod;
			string[] previewFileNames = m_previewFileNames;
			foreach (string path in previewFileNames)
			{
				string text = Path.Combine(localFolder, path);
				if (File.Exists(text))
				{
					myWorkshopItemPublisher.Thumbnail = text;
					break;
				}
			}
			if (myWorkshopItemPublisher.Tags.Contains("mod"))
			{
				bool flag2 = false;
<<<<<<< HEAD
				foreach (string item3 in Directory.EnumerateFiles(myWorkshopItemPublisher.Folder, "*", SearchOption.AllDirectories))
				{
					if (m_scriptExtensions.Contains(Path.GetExtension(item3)))
=======
				foreach (string item in Directory.EnumerateFiles(myWorkshopItemPublisher.Folder, "*", (SearchOption)1))
				{
					if (m_scriptExtensions.Contains(Path.GetExtension(item)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					myWorkshopItemPublisher.Tags.Add(MySteamConstants.TAG_NO_SCRIPTS);
				}
			}
			try
			{
				MyGameServiceCallResult publishResult = MyGameServiceCallResult.Fail;
				AutoResetEvent resetEvent = new AutoResetEvent(initialState: false);
				try
				{
					m_publisher = myWorkshopItemPublisher;
					MyWorkshopItem publishedItemLocal = null;
					m_publisher.ItemPublished += delegate(MyGameServiceCallResult result, MyWorkshopItem item)
					{
						publishResult = result;
						if (result == MyGameServiceCallResult.OK)
						{
							MySandboxGame.Log.WriteLine("Published file update successful");
						}
						else
						{
							MySandboxGame.Log.WriteLine($"Error during publishing: {result}");
						}
						if (result == MyGameServiceCallResult.OK)
						{
							publishedItemLocal = item;
						}
						resetEvent.Set();
					};
					if (requiredDLCs != null)
					{
<<<<<<< HEAD
						foreach (uint item2 in requiredDLCs)
						{
							m_publisher.DLCs.Add(item2);
=======
						foreach (uint num in requiredDLCs)
						{
							m_publisher.DLCs.Add(num);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					m_publisher.Publish();
					resetEvent.WaitOne();
					publishedItem = publishedItemLocal;
				}
				finally
				{
					if (resetEvent != null)
					{
						((IDisposable)resetEvent).Dispose();
					}
				}
				return publishResult;
			}
			finally
			{
				m_publisher = null;
				if (flag && localFolder.StartsWith(Path.GetTempPath()))
				{
					Directory.Delete(localFolder, true);
				}
			}
		}

		private static bool CheckModFolder(ref string localFolder, HashSet<string> ignoredExtensions, HashSet<string> ignoredPaths)
		{
			if ((ignoredExtensions == null || ignoredExtensions.get_Count() == 0) && (ignoredPaths == null || ignoredPaths.get_Count() == 0))
			{
				return false;
			}
			string tempPath = Path.GetTempPath();
			string path = $"{Process.GetCurrentProcess().get_Id()}-{Path.GetFileName(localFolder)}";
			string text = Path.Combine(tempPath, path);
			if (Directory.Exists(text))
			{
				Directory.Delete(text, true);
			}
			localFolder = MyFileSystem.TerminatePath(localFolder);
			int sourcePathLength = localFolder.Length;
			MyFileSystem.CopyAll(localFolder, text, delegate(string s)
			{
				if (ignoredExtensions != null)
				{
					string extension = Path.GetExtension(s);
					if (extension != null && ignoredExtensions.Contains(extension))
					{
						return false;
					}
				}
				if (ignoredPaths != null)
				{
					string text2 = s.Substring(sourcePathLength);
					if (ignoredPaths.Contains(text2))
					{
						return false;
					}
				}
				return true;
			});
			localFolder = text;
			return true;
		}

		/// <summary>
		/// Tries to find mod metadata file and check compatibility with the game.
		/// </summary>
		/// <param name="localFullPath"></param>
		/// <returns></returns>
		public static MyModCompatibility CheckModCompatibility(string localFullPath)
		{
			if (string.IsNullOrWhiteSpace(localFullPath))
			{
				return MyModCompatibility.Unknown;
			}
			string text = Path.Combine(localFullPath, "metadata.mod");
			if (!MyFileSystem.FileExists(text))
			{
				return MyModCompatibility.Unknown;
			}
			return CheckModCompatibility(MyModMetadataLoader.Load(text));
		}

		/// <summary>
		/// Checks mod metadata and fixes what is not valid
		/// </summary>
		/// <param name="mod"></param>
		public static MyModCompatibility CheckModCompatibility(MyModMetadata mod)
		{
			return MyModCompatibility.Ok;
		}

		/// <summary>
		/// Checks mod meta-data and fixes what is not valid
		/// </summary>
		/// <param name="mod"></param>
		private static void CheckAndFixModMetadata(ref MyModMetadata mod)
		{
			if (mod == null)
			{
				mod = new MyModMetadata();
			}
			if (mod.ModVersion == null)
			{
				mod.ModVersion = new Version(1, 0);
			}
		}

		private static void endActionPublish(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			PublishItemResult publishItemResult = result as PublishItemResult;
			screen.CloseScreenNow();
			if (m_onPublishingFinished != null)
			{
				Action<bool, MyGameServiceCallResult, string, MyWorkshopItem[]> onPublishingFinished = m_onPublishingFinished;
				MyWorkshopItem[] publishedItems = publishItemResult.PublishedItems;
				onPublishingFinished(publishedItems != null && publishedItems.Length != 0, publishItemResult.Result, publishItemResult.ResultServiceName, publishItemResult.PublishedItems);
			}
			m_onPublishingFinished = null;
			m_asyncPublishScreen = null;
		}

<<<<<<< HEAD
		/// <summary>
		/// Do NOT call this method from update thread.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static (MyGameServiceCallResult, string) GetSubscribedWorldsBlocking(List<MyWorkshopItem> results)
		{
			MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedWorldsBlocking - START");
			try
			{
				return GetSubscribedItemsBlockingUGC(results, new string[1] { "world" });
			}
			finally
			{
				MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedWorldsBlocking - END");
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Do NOT call this method from update thread.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static (MyGameServiceCallResult, string) GetSubscribedCampaignsBlocking(List<MyWorkshopItem> results)
		{
			MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedWorldsBlocking - START");
			try
			{
				return GetSubscribedItemsBlockingUGC(results, new string[1] { "campaign" });
			}
			finally
			{
				MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedWorldsBlocking - END");
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Do NOT call this method from update thread.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static (MyGameServiceCallResult, string) GetSubscribedModsBlocking(List<MyWorkshopItem> results)
		{
			MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - START");
			try
			{
				return GetSubscribedItemsBlockingUGC(results, new string[1] { "mod" });
			}
			finally
			{
				MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - END");
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Do NOT call this method from update thread.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static (MyGameServiceCallResult, string) GetSubscribedScenariosBlocking(List<MyWorkshopItem> results)
		{
			MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedScenariosBlocking - START");
			try
			{
				return GetSubscribedItemsBlockingUGC(results, new string[1] { "scenario" });
			}
			finally
			{
				MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedScenariosBlocking - END");
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Do NOT call this method from update thread.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static (MyGameServiceCallResult, string) GetSubscribedBlueprintsBlocking(List<MyWorkshopItem> results)
		{
			MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - START");
			try
			{
				return GetSubscribedItemsBlockingUGC(results, new string[1] { "blueprint" });
			}
			finally
			{
				MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - END");
			}
		}

		public static (MyGameServiceCallResult, string) GetSubscribedIngameScriptsBlocking(List<MyWorkshopItem> results)
		{
			MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - START");
			try
			{
				return GetSubscribedItemsBlockingUGC(results, new string[1] { "ingameScript" });
			}
			finally
			{
				MySandboxGame.Log.WriteLine("MySteamWorkshop.GetSubscribedModsBlocking - END");
			}
		}

		private static Dictionary<string, List<ulong>> ToDictionary(IEnumerable<WorkshopId> workshopIds)
		{
			Dictionary<string, List<ulong>> dictionary = new Dictionary<string, List<ulong>>();
			foreach (WorkshopId workshopId in workshopIds)
			{
				string key = workshopId.ServiceName ?? MyGameService.GetDefaultUGC().ServiceName;
				if (!dictionary.TryGetValue(key, out var value))
				{
					dictionary.Add(key, value = new List<ulong>());
				}
				value.Add(workshopId.Id);
			}
			return dictionary;
		}

		public static void GetItemsAsync(List<WorkshopId> items, Action<bool, List<MyWorkshopItem>> onDone)
		{
			Parallel.Start(delegate
			{
				List<MyWorkshopItem> list = new List<MyWorkshopItem>();
				bool itemsBlockingUGC = GetItemsBlockingUGC(items, list);
				onDone.InvokeIfNotNull(itemsBlockingUGC, list);
			});
		}

		public static bool GetItemsBlockingUGC(List<WorkshopId> workshopIds, List<MyWorkshopItem> resultDestination)
		{
			if (!MyGameService.IsOnline && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return false;
			}
			if (workshopIds.Count == 0)
			{
				return true;
			}
			Dictionary<string, List<ulong>> dictionary = ToDictionary(workshopIds);
			MySandboxGame.Log.WriteLine($"MyWorkshop.GetItemsBlocking: getting {workshopIds.Count} items");
			resultDestination.Clear();
			foreach (KeyValuePair<string, List<ulong>> item in dictionary)
			{
				string text = item.Key ?? MyGameService.GetDefaultUGC().ServiceName;
				MyWorkshopQuery myWorkshopQuery = MyGameService.CreateWorkshopQuery(text);
				if (myWorkshopQuery == null)
				{
					MySandboxGame.Log.WriteLine("Unknown UGC service name: " + text);
					continue;
				}
				myWorkshopQuery.ItemIds = item.Value;
				AutoResetEvent resetEvent = new AutoResetEvent(initialState: false);
				try
				{
					myWorkshopQuery.QueryCompleted += delegate(MyGameServiceCallResult result)
					{
						if (result == MyGameServiceCallResult.OK)
						{
							MySandboxGame.Log.WriteLine("Mod query successful");
						}
						else
						{
							MySandboxGame.Log.WriteLine($"Error during mod query: {result}");
						}
						resetEvent.Set();
					};
					myWorkshopQuery.Run();
<<<<<<< HEAD
					if (MyFakes.FORCE_NO_WORKER)
					{
						while (!resetEvent.WaitOne(m_syncUpdateTimeout))
						{
							MyGameService.Update();
						}
					}
					else if (!resetEvent.WaitOne())
					{
						return false;
					}
				}
				finally
				{
					if (resetEvent != null)
					{
						((IDisposable)resetEvent).Dispose();
					}
				}
				if (myWorkshopQuery.Items != null)
				{
					resultDestination.AddRange(myWorkshopQuery.Items);
				}
			}
			return true;
		}

		private static (MyGameServiceCallResult, string) GetSubscribedItemsBlockingUGC(List<MyWorkshopItem> results, string[] tags)
		{
			(MyGameServiceCallResult, string) result = (MyGameServiceCallResult.OK, null);
			results.Clear();
			foreach (IMyUGCService aggregate in MyGameService.WorkshopService.GetAggregates())
			{
				if (aggregate.IsConsentGiven)
				{
					MyGameServiceCallResult subscribedItemsBlockingUGCInternal = GetSubscribedItemsBlockingUGCInternal(aggregate.ServiceName, results, tags);
					if (subscribedItemsBlockingUGCInternal != MyGameServiceCallResult.OK)
					{
						result = (subscribedItemsBlockingUGCInternal, aggregate.ServiceName);
					}
				}
			}
			return result;
		}

		private static MyGameServiceCallResult GetSubscribedItemsBlockingUGCInternal(string serviceName, List<MyWorkshopItem> results, IEnumerable<string> tags)
		{
			if (!MyGameService.IsActive || !MyGameService.IsOnline)
			{
				return MyGameServiceCallResult.NoUser;
			}
=======
					if (!resetEvent.WaitOne())
					{
						return false;
					}
				}
				finally
				{
					if (resetEvent != null)
					{
						((IDisposable)resetEvent).Dispose();
					}
				}
				if (myWorkshopQuery.Items != null)
				{
					resultDestination.AddRange(myWorkshopQuery.Items);
				}
			}
			return true;
		}

		private static (MyGameServiceCallResult, string) GetSubscribedItemsBlockingUGC(List<MyWorkshopItem> results, string[] tags)
		{
			(MyGameServiceCallResult, string) result = (MyGameServiceCallResult.OK, null);
			results.Clear();
			foreach (IMyUGCService aggregate in MyGameService.WorkshopService.GetAggregates())
			{
				if (aggregate.IsConsentGiven)
				{
					MyGameServiceCallResult subscribedItemsBlockingUGCInternal = GetSubscribedItemsBlockingUGCInternal(aggregate.ServiceName, results, tags);
					if (subscribedItemsBlockingUGCInternal != MyGameServiceCallResult.OK)
					{
						result = (subscribedItemsBlockingUGCInternal, aggregate.ServiceName);
					}
				}
			}
			return result;
		}

		private static MyGameServiceCallResult GetSubscribedItemsBlockingUGCInternal(string serviceName, List<MyWorkshopItem> results, IEnumerable<string> tags)
		{
			if (!MyGameService.IsActive)
			{
				return MyGameServiceCallResult.NoUser;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyWorkshopQuery myWorkshopQuery = MyGameService.CreateWorkshopQuery(serviceName);
			myWorkshopQuery.UserId = Sync.MyId;
			if (tags != null)
			{
				if (myWorkshopQuery.RequiredTags == null)
				{
					myWorkshopQuery.RequiredTags = new List<string>();
				}
				myWorkshopQuery.RequiredTags.AddRange(tags);
			}
			MyGameServiceCallResult queryResult = MyGameServiceCallResult.Fail;
			AutoResetEvent resetEvent = new AutoResetEvent(initialState: false);
			try
			{
				myWorkshopQuery.QueryCompleted += delegate(MyGameServiceCallResult result)
				{
					queryResult = result;
					if (result == MyGameServiceCallResult.OK)
					{
						MySandboxGame.Log.WriteLine("Query successful.");
					}
					else
					{
						MySandboxGame.Log.WriteLine($"Error during UGC query: {result}");
					}
					resetEvent.Set();
				};
				myWorkshopQuery.Run();
				if (MyFakes.FORCE_NO_WORKER)
				{
					while (!resetEvent.WaitOne(0))
					{
						MyGameService.Update();
					}
				}
				else if (!resetEvent.WaitOne())
				{
					return MyGameServiceCallResult.AccessDenied;
				}
			}
			finally
			{
				if (resetEvent != null)
				{
					((IDisposable)resetEvent).Dispose();
				}
			}
			if (myWorkshopQuery.Items != null)
			{
				results.AddRange(myWorkshopQuery.Items);
			}
			return queryResult;
		}

		public static void DownloadModsAsync(List<MyObjectBuilder_Checkpoint.ModItem> mods, Action<bool> onFinishedCallback, Action onCancelledCallback = null)
		{
			if (mods == null || mods.Count == 0)
			{
				onFinishedCallback(obj: true);
				return;
			}
			if (!Directory.Exists(m_workshopModsPath))
			{
				Directory.CreateDirectory(m_workshopModsPath);
			}
			m_cancelTokenDownloadMods = new CancelToken();
			MyGameService.Service.RequestPermissions(Permissions.UGC, attemptResolution: true, delegate(bool granted)
			{
				if (granted)
				{
					StartDownloadModsAsync(mods, onFinishedCallback, onCancelledCallback);
				}
				else
				{
					onCancelledCallback();
				}
			});
		}

		private static void StartDownloadModsAsync(List<MyObjectBuilder_Checkpoint.ModItem> mods, Action<bool> onFinishedCallback, Action onCancelledCallback = null)
		{
			m_downloadScreen = new MyGuiScreenDownloadMods(delegate
			{
				m_cancelTokenDownloadMods.Cancel = true;
				if (onCancelledCallback != null)
				{
					onCancelledCallback();
				}
			});
			m_downloadScreen.Closed += OnDownloadScreenClosed;
			m_downloadResult = new DownloadModsResult(mods, onFinishedCallback, m_cancelTokenDownloadMods);
			MyGuiSandbox.AddScreen(m_downloadScreen);
		}

		private static void FixModServiceName(List<MyObjectBuilder_Checkpoint.ModItem> mods)
		{
			for (int i = 0; i < mods.Count; i++)
			{
				MyObjectBuilder_Checkpoint.ModItem value = mods[i];
				if (string.IsNullOrEmpty(value.PublishedServiceName))
				{
					value.PublishedServiceName = MyGameService.GetDefaultUGC().ServiceName;
					mods[i] = value;
				}
			}
		}

		private static void OnDownloadScreenClosed(MyGuiScreenBase source, bool isUnloading)
		{
			if (m_cancelTokenDownloadMods != null)
			{
				m_cancelTokenDownloadMods.Cancel = true;
			}
		}

		private static void endActionDownloadMods()
		{
			m_downloadScreen.CloseScreen();
			if (!m_downloadResult.Result.Success)
			{
				MySandboxGame.Log.WriteLine($"Error downloading mods");
			}
			m_downloadResult.Callback(m_downloadResult.Result.Success);
		}

		public static ResultData DownloadModsBlockingUGC(List<MyWorkshopItem> mods, CancelToken cancelToken)
		{
			int num = 0;
			string numMods = mods.Count.ToString();
			CachingList<MyWorkshopItem> cachingList = new CachingList<MyWorkshopItem>();
			CachingList<MyWorkshopItem> cachingList2 = new CachingList<MyWorkshopItem>();
			List<KeyValuePair<MyWorkshopItem, string>> list = new List<KeyValuePair<MyWorkshopItem, string>>();
			bool flag = false;
			long timestamp = Stopwatch.GetTimestamp();
			double byteSize = 0.0;
			for (int i = 0; i < mods.Count; i++)
			{
				byteSize += (double)mods[i].Size;
			}
			string text = MyUtils.FormatByteSizePrefix(ref byteSize);
			text = byteSize.ToString("N1") + text + "B";
			double num2 = 0.0;
			foreach (MyWorkshopItem mod in mods)
			{
				if (!MyGameService.IsOnline)
				{
					flag = true;
					continue;
				}
				if (cancelToken != null && cancelToken.Cancel)
				{
					flag = true;
					continue;
				}
				UpdateDownloadScreen(num, numMods, list, text, num2, mod);
				if (!UpdateMod(mod))
				{
					MySandboxGame.Log.WriteLineAndConsole($"Mod failed: Id = {mod.Id}, title = '{mod.Title}'");
					cachingList.Add(mod);
					flag = true;
					if (cancelToken != null)
					{
						cancelToken.Cancel = true;
					}
					continue;
				}
				MySandboxGame.Log.WriteLineAndConsole($"Up to date mod: Id = {mod.Id}, title = '{mod.Title}'");
				if (m_downloadScreen == null)
				{
					continue;
				}
				using (m_modLock.AcquireExclusiveUsing())
				{
					num2 += (double)mod.Size;
					num++;
					list.RemoveAll((KeyValuePair<MyWorkshopItem, string> e) => e.Key == mod);
				}
			}
			long timestamp2 = Stopwatch.GetTimestamp();
			ResultData resultData;
			if (flag)
			{
				cachingList.ApplyChanges();
				if (cachingList.Count > 0)
				{
					foreach (MyWorkshopItem item in cachingList)
					{
						MySandboxGame.Log.WriteLineAndConsole($"Failed to download mod: Id = {item.Id}, title = '{item.Title}'");
					}
				}
				else if (cancelToken == null || !cancelToken.Cancel)
				{
					MySandboxGame.Log.WriteLineAndConsole($"Failed to download mods because service is not in Online Mode.");
				}
				else
				{
					MySandboxGame.Log.WriteLineAndConsole($"Failed to download mods because download was stopped.");
				}
				resultData = default(ResultData);
				return resultData;
			}
			cachingList2.ApplyChanges();
			resultData = default(ResultData);
			resultData.Success = true;
			resultData.MismatchMods = new List<MyWorkshopItem>(cachingList2);
			resultData.Mods = new List<MyWorkshopItem>(mods);
			ResultData result = resultData;
			double num3 = (double)(timestamp2 - timestamp) / (double)Stopwatch.Frequency;
			MySandboxGame.Log.WriteLineAndConsole($"Mod download time: {num3:0.00} seconds");
			return result;
		}

		private static void UpdateDownloadScreen(int counter, string numMods, List<KeyValuePair<MyWorkshopItem, string>> currentMods, string sizeStr, double runningTotal, MyWorkshopItem mod)
		{
			if (m_downloadScreen == null)
			{
				return;
			}
			string text2;
			if (mod.Title.Length <= 25)
			{
				text2 = mod.Title;
			}
			else
			{
				text2 = mod.Title.Substring(0, 25);
				int num = text2.LastIndexOf(' ');
				if (num != -1)
				{
					text2 = text2.Substring(0, num);
				}
				text2 += "...";
			}
			StringBuilder stringBuilder = new StringBuilder();
			using (m_modLock.AcquireExclusiveUsing())
			{
				double byteSize = runningTotal;
				string text3 = MyUtils.FormatByteSizePrefix(ref byteSize);
				double byteSize2 = mod.Size;
				string text4 = MyUtils.FormatByteSizePrefix(ref byteSize2);
				currentMods.Add(new KeyValuePair<MyWorkshopItem, string>(mod, text2 + " " + byteSize2.ToString("N1") + text4 + "B"));
				stringBuilder.AppendLine();
				foreach (KeyValuePair<MyWorkshopItem, string> currentMod in currentMods)
				{
					stringBuilder.AppendLine(currentMod.Value);
				}
				stringBuilder.AppendLine(MyTexts.GetString(MyCommonTexts.DownloadingMods_Completed) + counter + "/" + numMods + " : " + byteSize.ToString("N1") + text3 + "B/" + sizeStr);
			}
			MySandboxGame.Static.Invoke("MySteamWorkshop::set loading text", stringBuilder, delegate(object text)
			{
				m_downloadScreen.MessageText = (StringBuilder)text;
			});
		}

		public static bool DownloadScriptBlocking(MyWorkshopItem item)
		{
			if (!MyGameService.IsOnline)
			{
				return false;
			}
			if (!IsUpToDate(item))
			{
				if (!UpdateMod(item))
				{
					return false;
				}
			}
			else
			{
				MySandboxGame.Log.WriteLineAndConsole($"Up to date mod: Id = {item.Id}, title = '{item.Title}'");
			}
			return true;
		}

		public static bool DownloadBlueprintBlockingUGC(MyWorkshopItem item, bool check = true)
		{
			if (!check || !item.IsUpToDate())
			{
				if (!UpdateMod(item))
				{
					return false;
				}
			}
			else
			{
				MySandboxGame.Log.WriteLineAndConsole($"Up to date mod: Id = {item.Id}, title = '{item.Title}'");
			}
			return true;
		}

		public static bool IsUpToDate(MyWorkshopItem item)
		{
			if (!MyGameService.IsOnline)
			{
				return false;
			}
			item.UpdateState();
			return item.IsUpToDate();
		}

		public static ResultData DownloadWorldModsBlocking(List<MyObjectBuilder_Checkpoint.ModItem> mods, CancelToken cancelToken)
		{
			ResultData ret = default(ResultData);
			Task task = Parallel.Start(delegate
			{
				ret = DownloadWorldModsBlockingInternal(mods, cancelToken);
			});
			while (!task.IsComplete)
			{
				MyGameService.Update();
				Thread.Sleep(10);
			}
			return ret;
		}

<<<<<<< HEAD
		/// <summary>
		/// Do NOT call this method from update thread.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static ResultData DownloadWorldModsBlockingInternal(List<MyObjectBuilder_Checkpoint.ModItem> mods, CancelToken cancelToken)
		{
			ResultData resultData = default(ResultData);
			resultData.Success = true;
			if (!MyFakes.ENABLE_WORKSHOP_MODS)
			{
				if (cancelToken != null)
				{
					resultData.Cancel = cancelToken.Cancel;
				}
				return resultData;
			}
			MySandboxGame.Log.WriteLineAndConsole("Downloading world mods - START");
			MySandboxGame.Log.IncreaseIndent();
			if (mods != null && mods.Count > 0)
			{
				FixModServiceName(mods);
				List<WorkshopId> list = new List<WorkshopId>();
				foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
				{
					if (mod.PublishedFileId != 0L)
					{
						WorkshopId item = new WorkshopId(mod.PublishedFileId, mod.PublishedServiceName);
						if (!list.Contains(item))
						{
							list.Add(item);
						}
					}
					else if (Sandbox.Engine.Platform.Game.IsDedicated)
					{
						MySandboxGame.Log.WriteLineAndConsole("Local mods are not allowed in multiplayer.");
						MySandboxGame.Log.DecreaseIndent();
						return default(ResultData);
					}
				}
				list.Sort();
				bool flag = false;
				if (MyPlatformGameSettings.CONSOLE_COMPATIBLE)
				{
					foreach (WorkshopId item2 in list)
					{
						IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(item2.ServiceName);
						if (aggregate == null)
						{
							flag = true;
							MySandboxGame.Log.WriteLineAndConsole($"Can't download mod {item2.Id}. Service {item2.ServiceName} is not available");
						}
						else if (!aggregate.IsConsoleCompatible)
						{
							flag = true;
							MySandboxGame.Log.WriteLineAndConsole($"Can't download mod {item2.Id}. Service {aggregate.ServiceName} is not console compatible");
						}
					}
				}
				if (flag)
				{
					resultData.Success = false;
				}
				else if (Sandbox.Engine.Platform.Game.IsDedicated)
				{
					if (MySandboxGame.ConfigDedicated.AutodetectDependencies)
					{
						AddModDependencies(mods, list);
					}
					MyGameService.SetServerModTemporaryDirectory();
					resultData = DownloadModsBlocking(mods, resultData, list, cancelToken);
				}
				else
				{
					if (Sync.IsServer)
					{
						AddModDependencies(mods, list);
					}
					resultData = DownloadModsBlocking(mods, resultData, list, cancelToken);
				}
			}
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLineAndConsole("Downloading world mods - END");
			if (cancelToken != null)
			{
				resultData.Cancel |= cancelToken.Cancel;
			}
			return resultData;
		}

		private static void AddModDependencies(List<MyObjectBuilder_Checkpoint.ModItem> mods, List<WorkshopId> workshopIds)
		{
<<<<<<< HEAD
			HashSet<WorkshopId> hashSet = new HashSet<WorkshopId>();
			HashSet<WorkshopId> hashSet2 = new HashSet<WorkshopId>();
			foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
			{
				WorkshopId item = new WorkshopId(mod.PublishedFileId, mod.PublishedServiceName);
				hashSet2.Add(item);
				if (!mod.IsDependency && mod.PublishedFileId != 0L)
				{
					hashSet.Add(item);
				}
			}
			bool hasReferenceIssue;
			foreach (MyWorkshopItem item3 in GetModsDependencyHiearchy(hashSet, out hasReferenceIssue))
			{
				WorkshopId item2 = new WorkshopId(item3.Id, item3.ServiceName);
				if (hashSet2.Add(item2))
				{
					mods.Add(new MyObjectBuilder_Checkpoint.ModItem(item3.Id, item3.ServiceName, isDependency: true)
					{
						FriendlyName = item3.Title
					});
				}
				if (!workshopIds.Contains(item2))
				{
					workshopIds.Add(item2);
=======
			HashSet<WorkshopId> val = new HashSet<WorkshopId>();
			HashSet<WorkshopId> val2 = new HashSet<WorkshopId>();
			foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
			{
				WorkshopId workshopId = new WorkshopId(mod.PublishedFileId, mod.PublishedServiceName);
				val2.Add(workshopId);
				if (!mod.IsDependency && mod.PublishedFileId != 0L)
				{
					val.Add(workshopId);
				}
			}
			bool hasReferenceIssue;
			foreach (MyWorkshopItem item in GetModsDependencyHiearchy(val, out hasReferenceIssue))
			{
				WorkshopId workshopId2 = new WorkshopId(item.Id, item.ServiceName);
				if (val2.Add(workshopId2))
				{
					mods.Add(new MyObjectBuilder_Checkpoint.ModItem(item.Id, item.ServiceName, isDependency: true)
					{
						FriendlyName = item.Title
					});
				}
				if (!workshopIds.Contains(workshopId2))
				{
					workshopIds.Add(workshopId2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		public static List<MyWorkshopItem> GetModsDependencyHiearchy(HashSet<WorkshopId> workshopIds, out bool hasReferenceIssue)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			hasReferenceIssue = false;
			List<MyWorkshopItem> list = new List<MyWorkshopItem>();
<<<<<<< HEAD
			HashSet<WorkshopId> hashSet = new HashSet<WorkshopId>();
			List<WorkshopId> list2 = new List<WorkshopId>();
			Stack<WorkshopId> stack = new Stack<WorkshopId>();
			foreach (WorkshopId workshopId in workshopIds)
			{
				stack.Push(workshopId);
=======
			HashSet<WorkshopId> val = new HashSet<WorkshopId>();
			List<WorkshopId> list2 = new List<WorkshopId>();
			Stack<WorkshopId> val2 = new Stack<WorkshopId>();
			Enumerator<WorkshopId> enumerator = workshopIds.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					WorkshopId current = enumerator.get_Current();
					val2.Push(current);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			while (val2.get_Count() > 0)
			{
				while (val2.get_Count() > 0)
				{
<<<<<<< HEAD
					WorkshopId item = stack.Pop();
					if (!hashSet.Contains(item))
					{
						hashSet.Add(item);
						list2.Add(item);
=======
					WorkshopId workshopId = val2.Pop();
					if (!val.Contains(workshopId))
					{
						val.Add(workshopId);
						list2.Add(workshopId);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else
					{
						hasReferenceIssue = true;
<<<<<<< HEAD
						MyLog.Default.WriteLineAndConsole($"Reference issue detected (circular reference or wrong order) for mod {item.ServiceName}:{item.Id}");
=======
						MyLog.Default.WriteLineAndConsole($"Reference issue detected (circular reference or wrong order) for mod {workshopId.ServiceName}:{workshopId.Id}");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				if (list2.Count == 0)
				{
					continue;
				}
				List<MyWorkshopItem> modsInfo = GetModsInfo(list2);
				if (modsInfo != null)
				{
<<<<<<< HEAD
					foreach (MyWorkshopItem item2 in modsInfo)
					{
						list.Insert(0, item2);
						item2.UpdateDependencyBlocking();
						for (int num = item2.Dependencies.Count - 1; num >= 0; num--)
						{
							ulong id = item2.Dependencies[num];
							stack.Push(new WorkshopId(id, item2.ServiceName));
=======
					foreach (MyWorkshopItem item in modsInfo)
					{
						list.Insert(0, item);
						item.UpdateDependencyBlocking();
						for (int num = item.Dependencies.Count - 1; num >= 0; num--)
						{
							ulong id = item.Dependencies[num];
							val2.Push(new WorkshopId(id, item.ServiceName));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
				list2.Clear();
			}
			return list;
		}

		public static List<MyWorkshopItem> GetModsInfo(List<WorkshopId> workshopIds)
		{
			Dictionary<string, List<ulong>> dictionary = ToDictionary(workshopIds);
			List<MyWorkshopItem> list = null;
			foreach (KeyValuePair<string, List<ulong>> item in dictionary)
			{
				if (list == null)
				{
					list = GetModsInfo(item.Key, item.Value);
					continue;
				}
				List<MyWorkshopItem> modsInfo = GetModsInfo(item.Key, item.Value);
				if (modsInfo != null)
				{
					list.AddRange(modsInfo);
				}
			}
			return list;
		}

		public static List<MyWorkshopItem> GetModsInfo(string serviceName, List<ulong> workshopIds)
		{
			MyWorkshopQuery myWorkshopQuery = MyGameService.CreateWorkshopQuery(serviceName);
			if (myWorkshopQuery == null)
			{
				return null;
			}
			myWorkshopQuery.ItemIds = workshopIds;
			AutoResetEvent resetEvent = new AutoResetEvent(initialState: false);
			try
			{
				myWorkshopQuery.QueryCompleted += delegate(MyGameServiceCallResult result)
				{
					if (result == MyGameServiceCallResult.OK)
					{
						MySandboxGame.Log.WriteLine("Mod dependencies query successful");
					}
					else
					{
						MySandboxGame.Log.WriteLine($"Error during mod dependencies query: {result}");
					}
					resetEvent.Set();
				};
				myWorkshopQuery.Run();
<<<<<<< HEAD
				if (MyFakes.FORCE_NO_WORKER)
				{
					int num = m_dependenciesRequestTimeout / m_syncUpdateTimeout;
					while (!resetEvent.WaitOne(m_syncUpdateTimeout))
					{
						MyGameService.Update();
						num--;
						if (num < 0)
						{
							myWorkshopQuery.Dispose();
							return null;
						}
					}
				}
				else if (!resetEvent.WaitOne(m_dependenciesRequestTimeout))
=======
				if (!resetEvent.WaitOne(m_dependenciesRequestTimeout))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					myWorkshopQuery.Dispose();
					return null;
				}
			}
			finally
			{
				if (resetEvent != null)
				{
					((IDisposable)resetEvent).Dispose();
				}
			}
			List<MyWorkshopItem> items = myWorkshopQuery.Items;
			myWorkshopQuery.Dispose();
			return items;
		}

		private static ResultData DownloadModsBlocking(List<MyObjectBuilder_Checkpoint.ModItem> mods, ResultData ret, List<WorkshopId> workshopIds, CancelToken cancelToken)
		{
			List<MyWorkshopItem> toGet = new List<MyWorkshopItem>(workshopIds.Count);
			if (!GetItemsBlockingUGC(workshopIds, toGet))
			{
				MySandboxGame.Log.WriteLine("Could not obtain workshop item details");
				ret.Success = false;
			}
			else if (workshopIds.Count != toGet.Count)
			{
				MySandboxGame.Log.WriteLine($"Could not obtain all workshop item details, expected {workshopIds.Count}, got {toGet.Count}");
				ret.Success = false;
			}
			else
			{
				if (m_downloadScreen != null)
				{
					MySandboxGame.Static.Invoke(delegate
					{
						m_downloadScreen.MessageText = new StringBuilder(MyTexts.GetString(MyCommonTexts.ProgressTextDownloadingMods) + " 0 of " + toGet.Count);
					}, "DownloadModsBlocking");
				}
				ret = DownloadModsBlockingUGC(toGet, cancelToken);
				if (!ret.Success)
				{
					MySandboxGame.Log.WriteLine("Downloading mods failed");
				}
				else
				{
					MyObjectBuilder_Checkpoint.ModItem[] array = mods.ToArray();
					int i;
					for (i = 0; i < array.Length; i++)
					{
						MyWorkshopItem myWorkshopItem = toGet.Find((MyWorkshopItem x) => x.Id == array[i].PublishedFileId);
						if (myWorkshopItem != null)
						{
							array[i].FriendlyName = myWorkshopItem.Title;
							array[i].SetModData(myWorkshopItem);
						}
						else
						{
							array[i].FriendlyName = array[i].Name;
						}
					}
					mods.Clear();
					mods.AddRange(array);
				}
			}
			return ret;
		}

		/// <summary>
		/// Checks if mod is up-to-date
		/// </summary>
		/// <param name="mod"></param>
		/// <returns></returns>
		private static bool UpdateMod(MyWorkshopItem mod)
		{
			mod.UpdateState();
			if (mod.IsUpToDate())
			{
				return true;
			}
			MySandboxGame.Log.WriteLineAndConsole($"Downloading: Id = {mod.Id}, title = '{mod.Title}' ");
			AutoResetEvent resetEvent = new AutoResetEvent(initialState: false);
			try
			{
				MyWorkshopItem.DownloadItemResult value = delegate(MyGameServiceCallResult result, ulong id)
				{
					switch (result)
					{
					case MyGameServiceCallResult.OK:
						MySandboxGame.Log.WriteLineAndConsole("Mod download successful.");
						break;
					case MyGameServiceCallResult.Pending:
						return;
					default:
						MySandboxGame.Log.WriteLineAndConsole($"Error during downloading: {result}");
						break;
					}
					resetEvent.Set();
				};
				mod.ItemDownloaded += value;
				mod.Download();
				if (MyFakes.FORCE_NO_WORKER)
				{
					while (!resetEvent.WaitOne(m_syncUpdateTimeout))
					{
						MyGameService.Update();
					}
				}
				else
				{
					resetEvent.WaitOne();
				}
				mod.ItemDownloaded -= value;
			}
			finally
			{
				if (resetEvent != null)
				{
					((IDisposable)resetEvent).Dispose();
				}
			}
			return true;
		}

		public static WorkshopId[] ToWorkshopIds(this MyWorkshopItem[] items)
		{
<<<<<<< HEAD
			return items.ToList().ConvertAll((MyWorkshopItem x) => new WorkshopId(x.Id, x.ServiceName)).ToArray();
=======
			return Enumerable.ToList<MyWorkshopItem>((IEnumerable<MyWorkshopItem>)items).ConvertAll((MyWorkshopItem x) => new WorkshopId(x.Id, x.ServiceName)).ToArray();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static bool GenerateModInfoLocal(string modPath, MyWorkshopItem[] publishedFiles, ulong steamIDOwner)
		{
			return GenerateModInfo(Path.Combine(MyFileSystem.ModsPath, modPath), publishedFiles, steamIDOwner);
		}

		public static bool GenerateModInfo(string modPath, MyWorkshopItem[] publishedFiles, ulong steamIDOwner)
		{
			WorkshopId[] array = publishedFiles.ToWorkshopIds();
			string text = Path.Combine(modPath, "modinfo.sbmi");
			WorkshopId[] workshopIdFromLocalModInternal = GetWorkshopIdFromLocalModInternal(text, default(WorkshopId));
			MyObjectBuilder_ModInfo myObjectBuilder_ModInfo = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ModInfo>();
			myObjectBuilder_ModInfo.WorkshopId = 0uL;
			myObjectBuilder_ModInfo.SteamIDOwner = steamIDOwner;
			if (workshopIdFromLocalModInternal != null)
			{
<<<<<<< HEAD
				List<WorkshopId> list = workshopIdFromLocalModInternal.ToList();
=======
				List<WorkshopId> list = Enumerable.ToList<WorkshopId>((IEnumerable<WorkshopId>)workshopIdFromLocalModInternal);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				WorkshopId[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					WorkshopId workshopId = array2[i];
					int num = list.FindIndex((WorkshopId x) => x.ServiceName == workshopId.ServiceName);
					if (num != -1)
					{
						list[num] = workshopId;
					}
					else
					{
						list.Add(workshopId);
					}
				}
				myObjectBuilder_ModInfo.WorkshopIds = list.ToArray();
			}
			else
			{
				myObjectBuilder_ModInfo.WorkshopIds = array;
			}
			if (!MyObjectBuilderSerializer.SerializeXML(text, compress: false, myObjectBuilder_ModInfo))
			{
				MySandboxGame.Log.WriteLine($"Error creating modinfo: {array}, mod='{modPath}'");
				return false;
			}
			return true;
		}

		public static void CreateWorldInstanceAsync(MyWorkshopItem world, MyWorkshopPathInfo pathInfo, bool overwrite, Action<bool, string> callbackOnFinished = null)
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.ProgressTextCreatingWorld, null, () => new CreateWorldResult(world, pathInfo, callbackOnFinished, overwrite), endActionCreateWorldInstance));
		}

		private static void endActionCreateWorldInstance(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			screen.CloseScreen();
			CreateWorldResult createWorldResult = (CreateWorldResult)result;
			createWorldResult.Callback?.Invoke(createWorldResult.Success, createWorldResult.m_createdSessionPath);
		}

		public static void UpdateWorldsAsync(List<MyWorkshopItem> worlds, MyWorkshopPathInfo pathInfo, Action<bool> callbackOnFinished = null)
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, () => new UpdateWorldsResult(worlds, pathInfo, callbackOnFinished), endActionUpdateWorld));
		}

		private static void endActionUpdateWorld(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			screen.CloseScreen();
			UpdateWorldsResult updateWorldsResult = (UpdateWorldsResult)result;
			updateWorldsResult.Callback?.Invoke(updateWorldsResult.Success);
		}

		/// <summary>
		/// Do NOT call this method from update thread.
		/// </summary>
		public static bool TryUpdateWorldsBlocking(List<MyWorkshopItem> worlds, MyWorkshopPathInfo pathInfo)
		{
			if (!Directory.Exists(pathInfo.Path))
			{
				Directory.CreateDirectory(pathInfo.Path);
			}
			if (!MyGameService.IsOnline)
			{
				return false;
			}
			bool flag = true;
			foreach (MyWorkshopItem world in worlds)
			{
				flag &= UpdateMod(world);
			}
			return flag;
		}

		/// <summary>
		/// Do NOT call this method from update thread.
		/// </summary>
		public static bool TryCreateWorldInstanceBlocking(MyWorkshopItem world, MyWorkshopPathInfo pathInfo, out string sessionPath, bool overwrite)
		{
			string text = MyUtils.StripInvalidChars(world.Title);
			sessionPath = null;
			Path.Combine(pathInfo.Path, world.Id + pathInfo.Suffix);
			if (!MyGameService.IsOnline)
			{
				return false;
			}
			if (!UpdateMod(world))
			{
				return false;
			}
			sessionPath = MyLocalCache.GetSessionSavesPath(text, contentFolder: false, createIfNotExists: false);
			if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
			{
				if (overwrite)
				{
					MyCloudHelper.Delete(MyCloudHelper.LocalToCloudWorldPath(sessionPath));
				}
				if (Directory.Exists(sessionPath))
				{
<<<<<<< HEAD
					Directory.Delete(sessionPath, recursive: true);
=======
					Directory.Delete(sessionPath, true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				while (true)
				{
					List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(MyCloudHelper.LocalToCloudWorldPath(sessionPath));
					if (cloudFiles == null || cloudFiles.Count == 0)
					{
						break;
					}
					sessionPath = MyLocalCache.GetSessionSavesPath(text + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false);
				}
<<<<<<< HEAD
			}
			else
			{
				if (overwrite && Directory.Exists(sessionPath))
				{
					Directory.Delete(sessionPath, recursive: true);
				}
				while (Directory.Exists(sessionPath))
				{
					sessionPath = MyLocalCache.GetSessionSavesPath(text + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false);
				}
			}
			if (!Directory.Exists(sessionPath))
			{
=======
			}
			else
			{
				if (overwrite && Directory.Exists(sessionPath))
				{
					Directory.Delete(sessionPath, true);
				}
				while (Directory.Exists(sessionPath))
				{
					sessionPath = MyLocalCache.GetSessionSavesPath(text + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false);
				}
			}
			if (!Directory.Exists(sessionPath))
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Directory.CreateDirectory(sessionPath);
			}
			if (MyFileSystem.IsDirectory(world.Folder))
			{
				MyFileSystem.CopyAll(world.Folder, sessionPath);
			}
			else
			{
				MyZipArchive.ExtractToDirectory(world.Folder, sessionPath);
			}
			ulong sizeInBytes;
			MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes);
			if (myObjectBuilder_Checkpoint == null)
			{
				return false;
			}
			myObjectBuilder_Checkpoint.SessionName = $"({pathInfo.NamePrefix}) {world.Title}";
			myObjectBuilder_Checkpoint.LastSaveTime = DateTime.Now;
			myObjectBuilder_Checkpoint.WorkshopId = null;
			MyLocalCache.SaveCheckpoint(myObjectBuilder_Checkpoint, sessionPath);
			return true;
		}

		private static string GetErrorString(bool ioFailure, MyGameServiceCallResult result)
		{
			if (!ioFailure)
			{
				return result.ToString();
			}
			return "IO Failure";
		}

		public static bool CheckLocalModsAllowed(List<MyObjectBuilder_Checkpoint.ModItem> mods, bool allowLocalMods)
		{
			foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
			{
				if (mod.PublishedFileId == 0L && !allowLocalMods)
				{
					return false;
				}
			}
			return true;
		}

		public static bool CanRunOffline(List<MyObjectBuilder_Checkpoint.ModItem> mods)
		{
			foreach (MyObjectBuilder_Checkpoint.ModItem mod in mods)
			{
				if (mod.PublishedFileId != 0L)
				{
					string text = Path.Combine(MyFileSystem.ModsPath, mod.Name);
					if (!Directory.Exists(text) && !File.Exists(text))
					{
						return false;
					}
				}
			}
			return true;
		}

		public static string GetWorkshopErrorText(MyGameServiceCallResult result, string serviceName, bool workshopPermitted)
		{
<<<<<<< HEAD
			MyStringId id;
			switch (result)
			{
			case MyGameServiceCallResult.OK:
				id = MyStringId.NullOrEmpty;
				break;
			case MyGameServiceCallResult.NoUser:
				id = MySpaceTexts.WorkshopNoUser;
				break;
			case MyGameServiceCallResult.ParentalControlRestricted:
				id = (workshopPermitted ? MySpaceTexts.WorkshopRestricted : MySpaceTexts.WorkshopAgeRestricted);
				break;
			case MyGameServiceCallResult.PlatformRestricted:
				id = MySpaceTexts.WorkshopRestricted;
				break;
			case MyGameServiceCallResult.PlatformPublishRestricted:
				id = MySpaceTexts.WorkshopRestricted;
				break;
			case MyGameServiceCallResult.AccessDenied:
				id = MyCommonTexts.MessageBoxTextPublishFailed_AccessDenied;
				break;
			case MyGameServiceCallResult.FileNotFound:
				id = MyCommonTexts.MessageBoxTextPublishFailed_FileNotFound;
				break;
			default:
				id = MySpaceTexts.WorkshopError;
				break;
			}
			return ((!string.IsNullOrEmpty(serviceName)) ? (serviceName + ": ") : "") + MyTexts.Get(id);
=======
			return string.Concat(arg1: MyTexts.Get(result switch
			{
				MyGameServiceCallResult.OK => MyStringId.NullOrEmpty, 
				MyGameServiceCallResult.NoUser => MySpaceTexts.WorkshopNoUser, 
				MyGameServiceCallResult.ParentalControlRestricted => workshopPermitted ? MySpaceTexts.WorkshopRestricted : MySpaceTexts.WorkshopAgeRestricted, 
				MyGameServiceCallResult.PlatformRestricted => MySpaceTexts.WorkshopRestricted, 
				MyGameServiceCallResult.PlatformPublishRestricted => MySpaceTexts.WorkshopRestricted, 
				MyGameServiceCallResult.AccessDenied => MyCommonTexts.MessageBoxTextPublishFailed_AccessDenied, 
				MyGameServiceCallResult.FileNotFound => MyCommonTexts.MessageBoxTextPublishFailed_FileNotFound, 
				_ => MySpaceTexts.WorkshopError, 
			}), arg0: (!string.IsNullOrEmpty(serviceName)) ? (serviceName + ": ") : "");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static WorkshopId[] FilterWorkshopIds(WorkshopId[] publishedIds, string[] selectedServiceNames)
		{
<<<<<<< HEAD
			List<WorkshopId> workshopIds = publishedIds.ToList();
			List<string> list = selectedServiceNames.ToList();
=======
			List<WorkshopId> workshopIds = Enumerable.ToList<WorkshopId>((IEnumerable<WorkshopId>)publishedIds);
			List<string> list = Enumerable.ToList<string>((IEnumerable<string>)selectedServiceNames);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			int i = 0;
			while (i < workshopIds.Count)
			{
				if (list.FindIndex((string x) => x == workshopIds[i].ServiceName) == -1)
				{
					workshopIds.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			foreach (string serviceName in selectedServiceNames)
			{
				if (workshopIds.FindIndex((WorkshopId x) => x.ServiceName == serviceName) == -1)
				{
					workshopIds.Add(new WorkshopId(0uL, serviceName));
				}
			}
			return workshopIds.ToArray();
		}

		public static void ReportPublish(MyWorkshopItem[] publishedFiles, MyGameServiceCallResult result, string resultServiceName, Action onDone = null, int index = 0)
		{
			if (publishedFiles == null || index == publishedFiles.Length)
			{
				if (result != MyGameServiceCallResult.OK)
				{
					string workshopErrorText = GetWorkshopErrorText(result, resultServiceName, workshopPermitted: true);
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(workshopErrorText), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWorldPublishFailed), null, null, null, null, delegate
					{
						onDone.InvokeIfNotNull();
					}));
				}
				else
				{
					onDone.InvokeIfNotNull();
				}
			}
			else
			{
				MyWorkshopItem myWorkshopItem = publishedFiles[index++];
				MyGuiSandbox.OpenUrl(myWorkshopItem.GetItemUrl(), UrlOpenMode.SteamOrExternalWithConfirm, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextWorldPublishedBrowser), MyGameService.Service.ServiceName, myWorkshopItem.ServiceName), MyTexts.Get(MySpaceTexts.WorkshopItemPublished), new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextWorldPublished), MyGameService.Service.ServiceName, myWorkshopItem.ServiceName), MyTexts.Get(MySpaceTexts.WorkshopItemPublished), delegate
				{
					ReportPublish(publishedFiles, result, resultServiceName, onDone, index);
				});
			}
		}

		public static void OpenWorkshopBrowser(string contentTag, Action subscriptionChangedCallback = null)
		{
			MyGameService.Service.RequestPermissions(Permissions.UGC, attemptResolution: true, delegate(bool granted)
			{
				if (granted)
				{
					ContentTag = contentTag;
					SubscriptionChangedCallback = subscriptionChangedCallback;
					if (MyGameService.AtLeastOneUGCServiceConsented)
					{
						OpenWorkshopBrowserWindow();
					}
					else
					{
						MyModIoConsentViewModel viewModel = new MyModIoConsentViewModel(OpenWorkshopBrowserWindow);
						ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen(viewModel);
					}
				}
			});
		}

		private static void OpenWorkshopBrowserWindow()
		{
			MyWorkshopBrowserViewModel myWorkshopBrowserViewModel = new MyWorkshopBrowserViewModel();
			myWorkshopBrowserViewModel.AdditionalTag = ContentTag;
			ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen(myWorkshopBrowserViewModel);
		}
<<<<<<< HEAD
=======

		static MyWorkshop()
		{
			HashSet<string> obj = new HashSet<string>();
			obj.Add(".cs");
			m_scriptExtensions = obj;
			m_bufferSize = 1048576;
			buffer = new byte[m_bufferSize];
			m_modLock = new FastResourceLock();
		}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
