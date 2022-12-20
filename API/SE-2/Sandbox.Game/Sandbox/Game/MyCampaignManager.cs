using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components.Session;
using VRage.Game.Localization;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Campaign;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.GameServices;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game
{
	public class MyCampaignManager
	{
		private const string CAMPAIGN_CONTENT_RELATIVE_PATH = "Campaigns";

		private readonly string m_scenariosContentRelativePath = "Scenarios";

		private readonly string m_scenarioFileExtension = "*.scf";

		private const string CAMPAIGN_DEBUG_RELATIVE_PATH = "Worlds\\Campaigns";

		private static MyCampaignManager m_instance;

		private string m_activeCampaignName;

		private MyObjectBuilder_Campaign m_activeCampaign;

		private readonly Dictionary<string, List<MyObjectBuilder_Campaign>> m_campaignsByNames = new Dictionary<string, List<MyObjectBuilder_Campaign>>();

		private readonly List<string> m_activeCampaignLevelNames = new List<string>();

		private Dictionary<string, MyLocalization.MyBundle> m_campaignMenuLocalizationBundle = new Dictionary<string, MyLocalization.MyBundle>();

		private readonly HashSet<MyLocalizationContext> m_campaignLocContexts = new HashSet<MyLocalizationContext>();

		private MyLocalization.MyBundle? m_currentMenuBundle;

		private readonly List<MyWorkshopItem> m_subscribedCampaignItems = new List<MyWorkshopItem>();

		private Task m_refreshTask;

		public static Action AfterCampaignLocalizationsLoaded;

		public static MyCampaignManager Static
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new MyCampaignManager();
				}
				return m_instance;
			}
		}

		public (MyGameServiceCallResult, string) RefreshSubscribedModDataResult { get; private set; }

		public IEnumerable<MyObjectBuilder_Campaign> Campaigns
		{
			get
			{
				List<MyObjectBuilder_Campaign> list = new List<MyObjectBuilder_Campaign>();
				foreach (List<MyObjectBuilder_Campaign> value in m_campaignsByNames.Values)
				{
					list.AddRange(value);
				}
				return list;
			}
		}

		public IEnumerable<string> CampaignNames => m_campaignsByNames.Keys;

		public IEnumerable<string> ActiveCampaignLevels => m_activeCampaignLevelNames;

		public string ActiveCampaignName => m_activeCampaignName;

		public MyObjectBuilder_Campaign ActiveCampaign => m_activeCampaign;

		public string ActiveCampaingPlatform { get; private set; }

		public bool IsCampaignRunning
		{
			get
			{
				if (MySession.Static == null)
				{
					return false;
				}
				return MySession.Static.GetComponent<MyCampaignSessionComponent>()?.Running ?? false;
			}
		}

		public bool IsScenarioRunning
		{
			get
			{
				if (MySession.Static == null)
				{
					return false;
				}
				return MySession.Static.GetComponent<MyCampaignSessionComponent>()?.IsScenarioRunning ?? false;
			}
		}

		public IEnumerable<string> LocalizationLanguages
		{
			get
			{
				if (m_activeCampaign == null)
				{
					return null;
				}
				return m_activeCampaign.LocalizationLanguages;
			}
		}

		public bool IsNewCampaignLevelLoading { get; private set; }

		public static event Action OnActiveCampaignChanged;

		public event Action OnCampaignFinished;

		/// <summary>
		/// Loads campaign data to storage. 
		/// </summary>
		public void Init()
		{
			MyLocalization.Static.InitLoader(LoadCampaignLocalization);
			MySandboxGame.Log.WriteLine("MyCampaignManager.Constructor() - START");
			foreach (string file in MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, m_scenariosContentRelativePath), m_scenarioFileExtension, MySearchOption.AllDirectories))
			{
				if (MyObjectBuilderSerializer.DeserializeXML(file, out MyObjectBuilder_VSFiles objectBuilder) && objectBuilder.Campaign != null)
				{
					objectBuilder.Campaign.IsVanilla = true;
					objectBuilder.Campaign.IsLocalMod = false;
					objectBuilder.Campaign.CampaignPath = file;
					objectBuilder.Campaign.IsDebug = Path.GetDirectoryName(file).ToLower().EndsWith("_test");
					LoadCampaignData(objectBuilder.Campaign);
				}
			}
			foreach (string file2 in MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, "Worlds\\Campaigns"), "*.vs", MySearchOption.TopDirectoryOnly))
			{
				if (MyObjectBuilderSerializer.DeserializeXML(file2, out MyObjectBuilder_VSFiles objectBuilder2) && objectBuilder2.Campaign != null)
				{
					objectBuilder2.Campaign.IsVanilla = true;
					objectBuilder2.Campaign.IsLocalMod = false;
					objectBuilder2.Campaign.IsDebug = true;
					LoadCampaignData(objectBuilder2.Campaign);
				}
			}
			MySandboxGame.Log.WriteLine("MyCampaignManager.Constructor() - END");
		}

		public Task RefreshModData()
		{
			if (!m_refreshTask.IsComplete)
			{
				return m_refreshTask;
			}
			return m_refreshTask = Parallel.Start(delegate
			{
				RefreshLocalModData();
				if (MyGameService.IsActive && MyGameService.IsOnline)
				{
					RefreshSubscribedModData();
				}
			});
		}

		private void RefreshLocalModData()
		{
			string[] directories = Directory.GetDirectories(MyFileSystem.ModsPath);
			foreach (List<MyObjectBuilder_Campaign> value in m_campaignsByNames.Values)
			{
				value.RemoveAll((MyObjectBuilder_Campaign campaign) => campaign.IsLocalMod);
			}
			string[] array = directories;
			foreach (string localModPath in array)
			{
				RegisterLocalModData(localModPath);
			}
		}

		private void RegisterLocalModData(string localModPath)
		{
			foreach (string file in MyFileSystem.GetFiles(Path.Combine(localModPath, "Campaigns"), "*.vs", MySearchOption.TopDirectoryOnly))
			{
				LoadScenarioFile(file);
			}
			foreach (string file2 in MyFileSystem.GetFiles(Path.Combine(localModPath, m_scenariosContentRelativePath), m_scenarioFileExtension, MySearchOption.AllDirectories))
			{
				LoadScenarioFile(file2);
			}
		}

		private void LoadScenarioFile(string modFile)
		{
			if (MyObjectBuilderSerializer.DeserializeXML(modFile, out MyObjectBuilder_VSFiles objectBuilder) && objectBuilder.Campaign != null)
			{
				objectBuilder.Campaign.IsVanilla = false;
				objectBuilder.Campaign.IsLocalMod = true;
				objectBuilder.Campaign.ModFolderPath = GetModFolderPath(modFile);
				LoadCampaignData(objectBuilder.Campaign);
			}
		}

		/// <summary>
		/// Removes unsubscribed items and adds newly subscribed items
		/// </summary>
		private void RefreshSubscribedModData()
		{
			(MyGameServiceCallResult, string) subscribedCampaignsBlocking = MyWorkshop.GetSubscribedCampaignsBlocking(m_subscribedCampaignItems);
			List<MyObjectBuilder_Campaign> list = new List<MyObjectBuilder_Campaign>();
			foreach (List<MyObjectBuilder_Campaign> campaignList in m_campaignsByNames.Values)
			{
				foreach (MyObjectBuilder_Campaign item in campaignList)
				{
					if (item.PublishedFileId == 0L)
					{
						continue;
					}
					bool flag = false;
					for (int i = 0; i < m_subscribedCampaignItems.Count; i++)
					{
						MyWorkshopItem myWorkshopItem = m_subscribedCampaignItems[i];
						if (myWorkshopItem.Id == item.PublishedFileId && myWorkshopItem.ServiceName == item.PublishedServiceName)
						{
							m_subscribedCampaignItems.RemoveAtFast(i);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						list.Add(item);
					}
				}
				list.ForEach(delegate(MyObjectBuilder_Campaign campaignToRemove)
				{
					campaignList.Remove(campaignToRemove);
				});
				list.Clear();
<<<<<<< HEAD
			}
			MyWorkshop.DownloadModsBlockingUGC(m_subscribedCampaignItems, null);
			foreach (MyWorkshopItem subscribedCampaignItem in m_subscribedCampaignItems)
			{
				RegisterWorshopModDataUGC(subscribedCampaignItem);
			}
=======
			}
			MyWorkshop.DownloadModsBlockingUGC(m_subscribedCampaignItems, null);
			foreach (MyWorkshopItem subscribedCampaignItem in m_subscribedCampaignItems)
			{
				RegisterWorshopModDataUGC(subscribedCampaignItem);
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			RefreshSubscribedModDataResult = subscribedCampaignsBlocking;
		}

		private void RegisterWorshopModDataUGC(MyWorkshopItem mod)
		{
			string folder = mod.Folder;
			IEnumerable<string> files = MyFileSystem.GetFiles(folder, "*.vs", MySearchOption.AllDirectories);
			LoadScenarioMod(mod, files);
			IEnumerable<string> files2 = MyFileSystem.GetFiles(folder, m_scenarioFileExtension, MySearchOption.AllDirectories);
			LoadScenarioMod(mod, files2);
		}

		private void LoadScenarioMod(MyWorkshopItem mod, IEnumerable<string> visualScriptingFiles)
		{
			foreach (string visualScriptingFile in visualScriptingFiles)
			{
				if (MyObjectBuilderSerializer.DeserializeXML(visualScriptingFile, out MyObjectBuilder_VSFiles objectBuilder) && objectBuilder.Campaign != null)
				{
					objectBuilder.Campaign.IsVanilla = false;
					objectBuilder.Campaign.IsLocalMod = false;
					objectBuilder.Campaign.PublishedFileId = mod.Id;
					objectBuilder.Campaign.PublishedServiceName = mod.ServiceName;
					objectBuilder.Campaign.ModFolderPath = GetModFolderPath(visualScriptingFile);
					LoadCampaignData(objectBuilder.Campaign);
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Runs publish process for active campaign.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void PublishActive(string[] tags, string[] serviceNames)
		{
			WorkshopId[] workshopIds = MyWorkshop.FilterWorkshopIds(MyWorkshop.GetWorkshopIdFromMod(m_activeCampaign.ModFolderPath) ?? new WorkshopId[1]
			{
				new WorkshopId(0uL, MyGameService.GetDefaultUGC().ServiceName)
			}, serviceNames);
			MyWorkshop.PublishModAsync(m_activeCampaign.ModFolderPath, m_activeCampaign.Name, m_activeCampaign.Description, workshopIds, tags, MyPublishedFileVisibility.Public, OnPublishFinished);
		}

<<<<<<< HEAD
		/// <summary>
		/// Called when campaign gets uploaded to steam workshop 
		/// </summary>
		/// <param name="publishSuccess"></param>
		/// <param name="publishResult"></param>
		/// <param name="publishResultServiceName"></param>
		/// <param name="publishedFiles"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void OnPublishFinished(bool publishSuccess, MyGameServiceCallResult publishResult, string publishResultServiceName, MyWorkshopItem[] publishedFiles)
		{
			if (publishedFiles.Length != 0)
			{
				MyWorkshop.GenerateModInfoLocal(m_activeCampaign.ModFolderPath, publishedFiles, Sync.MyId);
			}
			MyWorkshop.ReportPublish(publishedFiles, publishResult, publishResultServiceName);
		}

		/// <summary>
		/// Takes the path the campaign file and returns path to mod folder 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private string GetModFolderPath(string path)
		{
			int num = path.IndexOf("Campaigns", StringComparison.InvariantCulture);
			if (num == -1)
			{
				num = path.IndexOf(m_scenariosContentRelativePath, StringComparison.InvariantCulture);
			}
			return path.Remove(num - 1);
		}

		/// <summary>
		/// Universal campaign loading process 
		/// </summary>
		/// <param name="campaignOb"></param>
		private void LoadCampaignData(MyObjectBuilder_Campaign campaignOb)
		{
<<<<<<< HEAD
=======
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!MyDLCs.IsDLCSupported(campaignOb.DLC))
			{
				return;
			}
			bool isCompatible = true;
			MyLocalCache.GetSessionPathFromScenarioObjectBuilder(campaignOb, "", forceConsoleCompatible: false, out isCompatible);
			if (!isCompatible)
			{
				return;
			}
			if (campaignOb.PublishedFileId != 0L && campaignOb.PublishedServiceName == null)
			{
				campaignOb.PublishedServiceName = MyGameService.GetDefaultUGC().ServiceName;
			}
			if (m_campaignsByNames.ContainsKey(campaignOb.Name))
			{
				List<MyObjectBuilder_Campaign> list = m_campaignsByNames[campaignOb.Name];
				foreach (MyObjectBuilder_Campaign item2 in list)
				{
					if (item2.IsLocalMod == campaignOb.IsLocalMod && item2.IsMultiplayer == campaignOb.IsMultiplayer && item2.IsVanilla == campaignOb.IsVanilla && item2.PublishedFileId == campaignOb.PublishedFileId && item2.PublishedServiceName == campaignOb.PublishedServiceName)
					{
						return;
					}
				}
				list.Add(campaignOb);
			}
			else
			{
				m_campaignsByNames.Add(campaignOb.Name, new List<MyObjectBuilder_Campaign>());
				m_campaignsByNames[campaignOb.Name].Add(campaignOb);
			}
			if (string.IsNullOrEmpty(campaignOb.DescriptionLocalizationFile))
			{
				return;
			}
			FileInfo val = new FileInfo(Path.Combine(campaignOb.ModFolderPath ?? MyFileSystem.ContentPath, campaignOb.DescriptionLocalizationFile));
			if (!((FileSystemInfo)val).get_Exists())
			{
				return;
			}
<<<<<<< HEAD
			string[] files = Directory.GetFiles(fileInfo.Directory.FullName, Path.GetFileNameWithoutExtension(fileInfo.Name) + "*.sbl", SearchOption.TopDirectoryOnly);
=======
			string[] files = Directory.GetFiles(((FileSystemInfo)val.get_Directory()).get_FullName(), Path.GetFileNameWithoutExtension(((FileSystemInfo)val).get_Name()) + "*.sbl", (SearchOption)0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			string text = (string.IsNullOrEmpty(campaignOb.ModFolderPath) ? campaignOb.Name : Path.Combine(campaignOb.ModFolderPath, campaignOb.Name));
			MyLocalization.MyBundle value = new MyLocalization.MyBundle
			{
				BundleId = MyStringId.GetOrCompute(text),
				FilePaths = new List<string>()
			};
			string[] array = files;
			foreach (string item in array)
			{
				if (!value.FilePaths.Contains(item))
				{
					value.FilePaths.Add(item);
				}
			}
			if (m_campaignMenuLocalizationBundle.ContainsKey(text))
			{
				m_campaignMenuLocalizationBundle[text] = value;
			}
			else
			{
				m_campaignMenuLocalizationBundle.Add(text, value);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Starts new session with campaign data 
		/// </summary>
		/// <param name="relativePath"></param>
		/// <param name="afterLoad"></param>
		/// <param name="campaignDirectoryName"></param>
		/// <param name="campaignName"></param>
		/// <param name="maxPlayers"></param>
		/// <param name="onlineMode"></param>
		/// <param name="runAsInstance"></param>
		public CloudResult LoadSessionFromActiveCampaign(string relativePath, Action afterLoad = null, string campaignDirectoryName = null, string campaignName = null, MyOnlineModeEnum onlineMode = MyOnlineModeEnum.OFFLINE, int maxPlayers = 0, bool runAsInstance = true)
		{
=======
		public CloudResult LoadSessionFromActiveCampaign(string relativePath, Action afterLoad = null, string campaignDirectoryName = null, string campaignName = null, MyOnlineModeEnum onlineMode = MyOnlineModeEnum.OFFLINE, int maxPlayers = 0, bool runAsInstance = true)
		{
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyLog.Default.WriteLine("LoadSessionFromActiveCampaign");
			string path;
			if (m_activeCampaign.IsVanilla || m_activeCampaign.IsDebug)
			{
				path = Path.Combine(MyFileSystem.ContentPath, relativePath);
				if (!MyFileSystem.FileExists(path))
				{
					MySandboxGame.Log.WriteLine("ERROR: Missing vanilla world file in campaign: " + m_activeCampaignName);
					return CloudResult.Failed;
				}
			}
			else
			{
				path = Path.Combine(m_activeCampaign.ModFolderPath, relativePath);
				if (!MyFileSystem.FileExists(path))
				{
					path = Path.Combine(MyFileSystem.ContentPath, relativePath);
					if (!MyFileSystem.FileExists(path))
					{
						MySandboxGame.Log.WriteLine("ERROR: Missing world file in campaign: " + m_activeCampaignName);
						return CloudResult.Failed;
					}
				}
			}
<<<<<<< HEAD
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(path));
			string text = directoryInfo.FullName;
=======
			DirectoryInfo val = new DirectoryInfo(Path.GetDirectoryName(path));
			string text = ((FileSystemInfo)val).get_FullName();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			string text2 = null;
			if (runAsInstance)
			{
				bool flag = true;
				if (string.IsNullOrEmpty(campaignDirectoryName))
				{
					campaignDirectoryName = ActiveCampaignName + " " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
					flag = false;
				}
				if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
				{
<<<<<<< HEAD
					string text3 = directoryInfo.Name;
=======
					string text3 = ((FileSystemInfo)val).get_Name();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					while (true)
					{
						text2 = campaignDirectoryName + " " + text3;
						List<MyCloudFileInfo> list = null;
						if (flag)
						{
							list = MyGameService.GetCloudFiles(text2);
						}
						if (list == null || list.Count == 0)
						{
							break;
						}
<<<<<<< HEAD
						text3 = directoryInfo.Name + " " + MyUtils.GetRandomInt(int.MaxValue).ToString("########");
=======
						text3 = ((FileSystemInfo)val).get_Name() + " " + MyUtils.GetRandomInt(int.MaxValue).ToString("########");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					text = Path.Combine(MyFileSystem.SavesPath, text2);
					if (MyFileSystem.DirectoryExists(text))
					{
<<<<<<< HEAD
						Directory.Delete(text, recursive: true);
=======
						Directory.Delete(text, true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				else
				{
<<<<<<< HEAD
					text = Path.Combine(MyFileSystem.SavesPath, campaignDirectoryName, directoryInfo.Name);
					while (MyFileSystem.DirectoryExists(text))
					{
						text = Path.Combine(MyFileSystem.SavesPath, campaignDirectoryName, directoryInfo.Name + " " + MyUtils.GetRandomInt(int.MaxValue).ToString("########"));
					}
				}
				MyUtils.CopyDirectory(directoryInfo.FullName, text);
=======
					text = Path.Combine(MyFileSystem.SavesPath, campaignDirectoryName, ((FileSystemInfo)val).get_Name());
					while (MyFileSystem.DirectoryExists(text))
					{
						text = Path.Combine(MyFileSystem.SavesPath, campaignDirectoryName, ((FileSystemInfo)val).get_Name() + " " + MyUtils.GetRandomInt(int.MaxValue).ToString("########"));
					}
				}
				MyUtils.CopyDirectory(((FileSystemInfo)val).get_FullName(), text);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (m_activeCampaign != null)
				{
					string path2 = Path.Combine(text, Path.GetFileName(path));
					if (MyFileSystem.FileExists(path2) && MyObjectBuilderSerializer.DeserializeXML(path2, out MyObjectBuilder_Checkpoint objectBuilder))
					{
						foreach (MyObjectBuilder_SessionComponent sessionComponent in objectBuilder.SessionComponents)
						{
							MyObjectBuilder_LocalizationSessionComponent myObjectBuilder_LocalizationSessionComponent;
							if ((myObjectBuilder_LocalizationSessionComponent = sessionComponent as MyObjectBuilder_LocalizationSessionComponent) != null)
							{
								myObjectBuilder_LocalizationSessionComponent.CampaignModFolderName = m_activeCampaign.ModFolderPath;
								break;
							}
						}
						MyObjectBuilderSerializer.SerializeXML(path2, compress: false, objectBuilder);
					}
				}
				if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
				{
					CloudResult cloudResult = MyCloudHelper.UploadFiles(MyCloudHelper.LocalToCloudWorldPath(text), text, MyPlatformGameSettings.GAME_SAVES_COMPRESSED_BY_DEFAULT);
					if (cloudResult != 0)
					{
						return cloudResult;
					}
				}
			}
			if (!string.IsNullOrEmpty(MyLanguage.CurrentCultureName))
			{
				afterLoad = (Action)Delegate.Combine(afterLoad, (Action)delegate
				{
					MyLocalizationSessionComponent component = MySession.Static.GetComponent<MyLocalizationSessionComponent>();
					component.LoadCampaignLocalization(m_activeCampaign.LocalizationPaths, m_activeCampaign.ModFolderPath);
					component.ReloadLanguageBundles();
					AfterCampaignLocalizationsLoaded.InvokeIfNotNull();
				});
			}
			afterLoad = (Action)Delegate.Combine(afterLoad, (Action)delegate
			{
				MySession.Static.Save();
				IsNewCampaignLevelLoading = false;
			});
			IsNewCampaignLevelLoading = true;
			if (MyLocalization.Static != null)
			{
				MyLocalization.Static.DisposeAll();
			}
			if (Sync.IsDedicated)
			{
				MyWorkshop.CancelToken cancelToken = new MyWorkshop.CancelToken();
				MySessionLoader.LoadDedicatedSession(text, cancelToken, afterLoad);
			}
			else
			{
				MySessionLoader.LoadSingleplayerSession(text, afterLoad, campaignName, onlineMode, maxPlayers, text2);
			}
			return CloudResult.Ok;
		}

		public void LoadCampaignLocalization()
		{
			if (MySession.Static != null)
			{
				MyLocalizationSessionComponent component = MySession.Static.GetComponent<MyLocalizationSessionComponent>();
				if (component != null && m_activeCampaign != null)
				{
					component.LoadCampaignLocalization(m_activeCampaign.LocalizationPaths, m_activeCampaign.ModFolderPath);
					component.ReloadLanguageBundles();
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		///  Changes the manager state to given campaign.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="isVanilla"></param>
		/// <param name="publisherFileId">0 is default value or local mod value.</param>
		/// <param name="publisherServiceName"></param>
		/// <param name="localModFolder"></param>
		/// <param name="platform"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool SwitchCampaign(string name, bool isVanilla = true, ulong publisherFileId = 0uL, string publisherServiceName = null, string localModFolder = null, string platform = null)
		{
			if (m_campaignsByNames.ContainsKey(name))
			{
				ActiveCampaingPlatform = platform;
				foreach (MyObjectBuilder_Campaign item in m_campaignsByNames[name])
				{
					if (item.IsVanilla == isVanilla && item.IsLocalMod == (localModFolder != null && publisherFileId == 0) && item.PublishedFileId == publisherFileId && item.PublishedServiceName == publisherServiceName)
					{
						m_activeCampaign = item;
						m_activeCampaignName = name;
						m_activeCampaignLevelNames.Clear();
						MyLog.Default.WriteLine("Switching active campaign: " + m_activeCampaign?.Name);
						MyObjectBuilder_CampaignSMNode[] nodes = m_activeCampaign.GetStateMachine(platform).Nodes;
						foreach (MyObjectBuilder_CampaignSMNode myObjectBuilder_CampaignSMNode in nodes)
						{
							m_activeCampaignLevelNames.Add(myObjectBuilder_CampaignSMNode.Name);
						}
						MyCampaignManager.OnActiveCampaignChanged.InvokeIfNotNull();
<<<<<<< HEAD
						LoadCampaignLocalization();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						return true;
					}
					if (publisherFileId == 0L && item.PublishedFileId != 0L)
					{
						publisherFileId = item.PublishedFileId;
						return true;
					}
				}
			}
			if (publisherFileId != 0L)
			{
				if (DownloadCampaign(publisherFileId, publisherServiceName))
				{
					return SwitchCampaign(name, isVanilla, publisherFileId, publisherServiceName, localModFolder, platform);
				}
			}
			else if (!isVanilla && localModFolder != null && MyFileSystem.DirectoryExists(localModFolder))
			{
				RegisterLocalModData(localModFolder);
				return SwitchCampaign(name, isVanilla, publisherFileId, publisherServiceName, localModFolder, platform);
<<<<<<< HEAD
			}
			return false;
		}

		public void SetExperimentalCampaign(MyObjectBuilder_Checkpoint checkpoint)
		{
			MyObjectBuilder_CampaignSessionComponent myObjectBuilder_CampaignSessionComponent = checkpoint.SessionComponents.OfType<MyObjectBuilder_CampaignSessionComponent>().FirstOrDefault();
			if (myObjectBuilder_CampaignSessionComponent != null && myObjectBuilder_CampaignSessionComponent.IsVanilla)
			{
				checkpoint.Settings.ExperimentalMode = true;
				myObjectBuilder_CampaignSessionComponent.IsVanilla = false;
			}
		}

		public bool IsCampaign(MyObjectBuilder_CampaignSessionComponent ob)
		{
			return IsCampaign(ob.CampaignName, ob.IsVanilla, ob.Mod.PublishedFileId);
		}

		public bool IsCampaign(string campaignName, bool isVanilla, ulong modPublishedFileId)
		{
			if (!string.IsNullOrEmpty(campaignName) && m_campaignsByNames.ContainsKey(campaignName) && isVanilla)
			{
				return modPublishedFileId == 0;
			}
			return false;
		}

		public bool HasStartedAsCampaign(string campaignName)
		{
			if (!string.IsNullOrEmpty(campaignName))
			{
				return m_campaignsByNames.ContainsKey(campaignName);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return false;
		}

<<<<<<< HEAD
=======
		public void SetExperimentalCampaign(MyObjectBuilder_Checkpoint checkpoint)
		{
			MyObjectBuilder_CampaignSessionComponent myObjectBuilder_CampaignSessionComponent = Enumerable.FirstOrDefault<MyObjectBuilder_CampaignSessionComponent>(Enumerable.OfType<MyObjectBuilder_CampaignSessionComponent>((IEnumerable)checkpoint.SessionComponents));
			if (myObjectBuilder_CampaignSessionComponent != null && myObjectBuilder_CampaignSessionComponent.IsVanilla)
			{
				checkpoint.Settings.ExperimentalMode = true;
				myObjectBuilder_CampaignSessionComponent.IsVanilla = false;
			}
		}

		public bool IsCampaign(MyObjectBuilder_CampaignSessionComponent ob)
		{
			return IsCampaign(ob.CampaignName, ob.IsVanilla, ob.Mod.PublishedFileId);
		}

		public bool IsCampaign(string campaignName, bool isVanilla, ulong modPublishedFileId)
		{
			if (!string.IsNullOrEmpty(campaignName) && m_campaignsByNames.ContainsKey(campaignName) && isVanilla)
			{
				return modPublishedFileId == 0;
			}
			return false;
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void Unload()
		{
			m_activeCampaign = null;
			m_activeCampaignName = null;
			m_activeCampaignLevelNames.Clear();
		}

<<<<<<< HEAD
		/// <summary>
		/// Tries to download and register mod of given fileId.
		/// </summary>
		/// <param name="publisherFileId"></param>
		/// <param name="publisherServiceName"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool DownloadCampaign(ulong publisherFileId, string publisherServiceName)
		{
			MyWorkshop.ResultData resultData = default(MyWorkshop.ResultData);
			resultData.Success = false;
			MyWorkshop.ResultData resultData2 = resultData;
			MyWorkshopItem myWorkshopItem = MyGameService.CreateWorkshopItem(publisherServiceName);
			myWorkshopItem.Id = publisherFileId;
			resultData2 = MyWorkshop.DownloadModsBlockingUGC(new List<MyWorkshopItem>(new MyWorkshopItem[1] { myWorkshopItem }), null);
			if (resultData2.Success && resultData2.Mods.Count != 0)
			{
				RegisterWorshopModDataUGC(resultData2.Mods[0]);
				return true;
			}
			return false;
		}

		public void ReloadMenuLocalization(string name)
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			if (m_currentMenuBundle.HasValue)
			{
				MyLocalization.Static.UnloadBundle(m_currentMenuBundle.Value.BundleId);
				m_campaignLocContexts.Clear();
			}
			if (!m_campaignMenuLocalizationBundle.ContainsKey(name))
			{
				return;
			}
			m_currentMenuBundle = m_campaignMenuLocalizationBundle[name];
			if (!m_currentMenuBundle.HasValue)
			{
				return;
			}
			MyLocalization.Static.LoadBundle(m_currentMenuBundle.Value, m_campaignLocContexts, disposableContexts: false);
<<<<<<< HEAD
			foreach (MyLocalizationContext campaignLocContext in m_campaignLocContexts)
			{
				campaignLocContext.Switch(MyLanguage.CurrentCultureName);
			}
		}

		/// <summary>
		/// starts new campaign 
		/// </summary>
		/// <param name="campaignName"></param> 
		/// <param name="maxPlayers"></param>
		/// <param name="onlineMode"></param>
		/// <param name="platform"></param>
		/// <param name="runAsInstance"></param>
		public CloudResult RunNewCampaign(string campaignName, MyOnlineModeEnum onlineMode, int maxPlayers, string platform, bool runAsInstance = true)
		{
			if (m_activeCampaign == null)
			{
				return CloudResult.Failed;
			}
			MyObjectBuilder_CampaignSM stateMachine = m_activeCampaign.GetStateMachine(platform);
			MyObjectBuilder_CampaignSMNode myObjectBuilder_CampaignSMNode = ((stateMachine != null) ? FindStartingState(stateMachine) : null);
			if (myObjectBuilder_CampaignSMNode != null)
			{
				int num = (MySandboxGame.Config.ExperimentalMode ? stateMachine.MaxLobbyPlayersExperimental : stateMachine.MaxLobbyPlayers);
				if (num > 0 && !Sync.IsDedicated)
				{
					if (num == 1)
					{
						onlineMode = MyOnlineModeEnum.OFFLINE;
					}
					else
					{
						maxPlayers = num;
					}
=======
			Enumerator<MyLocalizationContext> enumerator = m_campaignLocContexts.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Switch(MyLanguage.CurrentCultureName);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				return LoadSessionFromActiveCampaign(myObjectBuilder_CampaignSMNode.SaveFilePath, null, null, campaignName, onlineMode, maxPlayers, runAsInstance);
			}
<<<<<<< HEAD
			return CloudResult.Failed;
		}

		public void RunCampaign(string path, bool runAsInstance = true, string platform = null)
		{
			MyObjectBuilder_Campaign myObjectBuilder_Campaign = null;
			if (MyObjectBuilderSerializer.DeserializeXML(path, out MyObjectBuilder_VSFiles objectBuilder) && objectBuilder.Campaign != null)
			{
				objectBuilder.Campaign.IsVanilla = false;
				objectBuilder.Campaign.IsLocalMod = true;
				LoadCampaignData(objectBuilder.Campaign);
				myObjectBuilder_Campaign = objectBuilder.Campaign;
			}
			if (myObjectBuilder_Campaign != null && (!Sync.IsDedicated || objectBuilder.Campaign.IsMultiplayer))
			{
				ActiveCampaingPlatform = platform;
				if (Static.SwitchCampaign(myObjectBuilder_Campaign.Name, myObjectBuilder_Campaign.IsVanilla, myObjectBuilder_Campaign.PublishedFileId, myObjectBuilder_Campaign.PublishedServiceName, myObjectBuilder_Campaign.ModFolderPath = GetModFolderPath(path)))
				{
					MyOnlineModeEnum onlineMode = (myObjectBuilder_Campaign.IsMultiplayer ? MyOnlineModeEnum.PUBLIC : MyOnlineModeEnum.OFFLINE);
					int maxPlayers = myObjectBuilder_Campaign.MaxPlayers;
					Static.RunNewCampaign(myObjectBuilder_Campaign.Name, onlineMode, maxPlayers, platform, runAsInstance);
				}
=======
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public CloudResult RunNewCampaign(string campaignName, MyOnlineModeEnum onlineMode, int maxPlayers, string platform, bool runAsInstance = true)
		{
			if (m_activeCampaign == null)
			{
				return CloudResult.Failed;
			}
			MyObjectBuilder_CampaignSM stateMachine = m_activeCampaign.GetStateMachine(platform);
			MyObjectBuilder_CampaignSMNode myObjectBuilder_CampaignSMNode = ((stateMachine != null) ? FindStartingState(stateMachine) : null);
			if (myObjectBuilder_CampaignSMNode != null)
			{
				int num = (MySandboxGame.Config.ExperimentalMode ? stateMachine.MaxLobbyPlayersExperimental : stateMachine.MaxLobbyPlayers);
				if (num > 0 && !Sync.IsDedicated)
				{
					if (num == 1)
					{
						onlineMode = MyOnlineModeEnum.OFFLINE;
					}
					else
					{
						maxPlayers = num;
					}
				}
				return LoadSessionFromActiveCampaign(myObjectBuilder_CampaignSMNode.SaveFilePath, null, null, campaignName, onlineMode, maxPlayers, runAsInstance);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return CloudResult.Failed;
		}

<<<<<<< HEAD
		/// <summary>
		/// Finds starting state of the campaign SM. For purposes of first load.
		/// </summary>
		/// <returns></returns>
		private MyObjectBuilder_CampaignSMNode FindStartingState(MyObjectBuilder_CampaignSM stateMachine)
		{
=======
		public void RunCampaign(string path, bool runAsInstance = true, string platform = null)
		{
			MyObjectBuilder_Campaign myObjectBuilder_Campaign = null;
			if (MyObjectBuilderSerializer.DeserializeXML(path, out MyObjectBuilder_VSFiles objectBuilder) && objectBuilder.Campaign != null)
			{
				objectBuilder.Campaign.IsVanilla = false;
				objectBuilder.Campaign.IsLocalMod = true;
				LoadCampaignData(objectBuilder.Campaign);
				myObjectBuilder_Campaign = objectBuilder.Campaign;
			}
			if (myObjectBuilder_Campaign != null && (!Sync.IsDedicated || objectBuilder.Campaign.IsMultiplayer))
			{
				ActiveCampaingPlatform = platform;
				if (Static.SwitchCampaign(myObjectBuilder_Campaign.Name, myObjectBuilder_Campaign.IsVanilla, myObjectBuilder_Campaign.PublishedFileId, myObjectBuilder_Campaign.PublishedServiceName, myObjectBuilder_Campaign.ModFolderPath = GetModFolderPath(path)))
				{
					MyOnlineModeEnum onlineMode = (myObjectBuilder_Campaign.IsMultiplayer ? MyOnlineModeEnum.PUBLIC : MyOnlineModeEnum.OFFLINE);
					int maxPlayers = myObjectBuilder_Campaign.MaxPlayers;
					Static.RunNewCampaign(myObjectBuilder_Campaign.Name, onlineMode, maxPlayers, platform, runAsInstance);
				}
			}
		}

		private MyObjectBuilder_CampaignSMNode FindStartingState(MyObjectBuilder_CampaignSM stateMachine)
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			bool flag = false;
			MyObjectBuilder_CampaignSMNode[] nodes = stateMachine.Nodes;
			foreach (MyObjectBuilder_CampaignSMNode myObjectBuilder_CampaignSMNode in nodes)
			{
				MyObjectBuilder_CampaignSMTransition[] transitions = stateMachine.Transitions;
				for (int j = 0; j < transitions.Length; j++)
				{
					if (transitions[j].To == myObjectBuilder_CampaignSMNode.Name)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					flag = false;
					continue;
				}
				return myObjectBuilder_CampaignSMNode;
			}
			return null;
		}

		/// <summary>
		/// Called from MyCampaignSessionComponent when campaign finished. Do not use anywhere else.
		/// </summary>
		public void NotifyCampaignFinished()
		{
			this.OnCampaignFinished?.Invoke();
		}
	}
}
