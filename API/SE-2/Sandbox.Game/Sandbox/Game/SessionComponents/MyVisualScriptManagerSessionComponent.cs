using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Debugging;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Debugging;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.SessionComponents;
using VRage.Game.VisualScripting;
using VRage.Game.VisualScripting.Missions;
using VRage.Generics;
using VRage.Scripting;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation, 1000, typeof(MyObjectBuilder_VisualScriptManagerSessionComponent), null, false)]
	public class MyVisualScriptManagerSessionComponent : MySessionComponentBase
	{
		public static readonly string WAYPOINT_NAME_PREFIX = "Waypoint_";

		private static bool m_firstUpdate = true;

		private CachingList<IMyLevelScript> m_levelScripts;

		private MyObjectBuilder_VisualScriptManagerSessionComponent m_objectBuilder;

		private MyVSStateMachineManager m_smManager;

		private readonly Dictionary<string, string> m_relativePathsToAbsolute = new Dictionary<string, string>();

		private readonly List<string> m_stateMachineDefinitionFilePaths = new List<string>();

		private string[] m_runningLevelScriptNames;

		private string[] m_failedLevelScriptExceptionTexts;

		private HashSet<string> m_worldOutlineFolders = new HashSet<string>();

		private Dictionary<long, MyUIString> m_UIStrings = new Dictionary<long, MyUIString>();

		private StringBuilder m_UIStringBuilder = new StringBuilder();

		private Dictionary<string, MyGuiScreenBoard> m_boardScreens = new Dictionary<string, MyGuiScreenBoard>();

		private int m_updateCounter;

		private const int LIVE_DEBUGGING_DELAY = 120;

		private int m_liveDebuggingCounter;

		private bool m_hadClients;

		public bool IsActive => m_levelScripts != null;

		public CachingList<IMyLevelScript> LevelScripts => m_levelScripts;

		public MyVSStateMachineManager SMManager => m_smManager;

		public MyObjectBuilder_Questlog QuestlogData
		{
			get
			{
				if (m_objectBuilder != null)
				{
					return m_objectBuilder.Questlog;
				}
				return null;
			}
			set
			{
				if (m_objectBuilder != null)
				{
					m_objectBuilder.Questlog = value;
				}
			}
		}

		public MyObjectBuilder_ExclusiveHighlights ExclusiveHighlightsData
		{
			get
			{
				if (m_objectBuilder != null)
				{
					return m_objectBuilder.ExclusiveHighlights;
				}
				return null;
			}
			set
			{
				if (m_objectBuilder != null)
				{
					m_objectBuilder.ExclusiveHighlights = value;
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Array of running level script names.
		/// </summary>
		public string[] RunningLevelScriptNames => m_runningLevelScriptNames;

		/// <summary>
		/// Array of exceptions raised when the scripts were running.
		/// The name of the script is at the same index. (RunningLevelScriptNames)
		/// </summary>
		public string[] FailedLevelScriptExceptionTexts => m_failedLevelScriptExceptionTexts;

		/// <summary>
		/// Path a campaign repository in case of modded campaign.
		/// </summary>
=======
		public string[] RunningLevelScriptNames => m_runningLevelScriptNames;

		public string[] FailedLevelScriptExceptionTexts => m_failedLevelScriptExceptionTexts;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public string CampaignModPath { get; set; }

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyVSStateMachineFinalNode.Finished = (Action<string, string, bool, bool>)Delegate.Combine(MyVSStateMachineFinalNode.Finished, new Action<string, string, bool, bool>(OnSMFinished));
			MyObjectBuilder_VisualScriptManagerSessionComponent myObjectBuilder_VisualScriptManagerSessionComponent = (m_objectBuilder = (MyObjectBuilder_VisualScriptManagerSessionComponent)sessionComponent);
			if (myObjectBuilder_VisualScriptManagerSessionComponent.BoardScreens != null)
			{
				m_boardScreens.Clear();
				MyObjectBuilder_BoardScreen[] boardScreens = myObjectBuilder_VisualScriptManagerSessionComponent.BoardScreens;
				foreach (MyObjectBuilder_BoardScreen myObjectBuilder_BoardScreen in boardScreens)
				{
					Vector2 screenTextLeftTopPosition = MyGuiManager.GetScreenTextLeftTopPosition();
					MyGuiScreenBoard myGuiScreenBoard = new MyGuiScreenBoard(myObjectBuilder_BoardScreen.Coords, screenTextLeftTopPosition, myObjectBuilder_BoardScreen.Size);
					myGuiScreenBoard.Init(myObjectBuilder_BoardScreen);
					m_boardScreens.Add(myObjectBuilder_BoardScreen.Id, myGuiScreenBoard);
					if (!Sync.IsDedicated)
					{
						MyScreenManager.AddScreen(myGuiScreenBoard);
					}
				}
			}
			if (!Session.IsServer)
			{
				return;
			}
			m_firstUpdate = myObjectBuilder_VisualScriptManagerSessionComponent.FirstRun;
			m_worldOutlineFolders.Clear();
			if (myObjectBuilder_VisualScriptManagerSessionComponent.WorldOutlineFolders != null)
			{
				string[] worldOutlineFolders = myObjectBuilder_VisualScriptManagerSessionComponent.WorldOutlineFolders;
<<<<<<< HEAD
				foreach (string item in worldOutlineFolders)
				{
					m_worldOutlineFolders.Add(item);
=======
				foreach (string text in worldOutlineFolders)
				{
					m_worldOutlineFolders.Add(text);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			FixFilepathsPlatform();
		}

		private void OnSMFinished(string machineName, string transitionName, bool showCredits, bool closeSession)
		{
			_ = MyCampaignManager.Static.ActiveCampaignName;
			MyVisualScriptLogicProvider.SessionClose(10000, showCredits, closeSession);
		}

		public override void BeforeStart()
		{
			if (m_objectBuilder == null || !Session.IsServer)
			{
				return;
			}
			m_relativePathsToAbsolute.Clear();
			m_stateMachineDefinitionFilePaths.Clear();
			string text = null;
			if (m_objectBuilder.LevelScriptFiles != null)
			{
				string[] levelScriptFiles = m_objectBuilder.LevelScriptFiles;
				foreach (string text2 in levelScriptFiles)
				{
					MyContentPath myContentPath = Path.Combine(CampaignModPath ?? MyFileSystem.ContentPath, text2);
					if (myContentPath.GetExitingFilePath() != null)
					{
						m_relativePathsToAbsolute.Add(text2, myContentPath.GetExitingFilePath());
						if (text2.StartsWith("Scenarios"))
						{
							text = text2.Substring("Scenarios\\".Length);
							int length = text.IndexOf('\\');
							text = text.Substring(0, length);
						}
					}
					else
					{
						MyLog.Default.WriteLine(text2 + " Level Script was not found.");
					}
				}
			}
			if (m_objectBuilder.StateMachines != null)
			{
				string[] levelScriptFiles = m_objectBuilder.StateMachines;
				foreach (string text3 in levelScriptFiles)
				{
					MyContentPath myContentPath2 = Path.Combine(CampaignModPath ?? MyFileSystem.ContentPath, text3);
					if (myContentPath2.GetExitingFilePath() != null)
					{
						if (!m_relativePathsToAbsolute.ContainsKey(text3))
						{
							m_stateMachineDefinitionFilePaths.Add(myContentPath2.GetExitingFilePath());
						}
						m_relativePathsToAbsolute.Add(text3, myContentPath2.GetExitingFilePath());
					}
					else
					{
						MyLog.Default.WriteLine(text3 + " Mission File was not found.");
					}
				}
			}
			bool flag = false;
			if (Session.Mods != null)
			{
				foreach (MyObjectBuilder_Checkpoint.ModItem mod in Session.Mods)
				{
					string text4 = mod.GetPath();
					if (!MyFileSystem.DirectoryExists(text4))
					{
						text4 = Path.Combine(MyFileSystem.ModsPath, mod.Name);
						if (!MyFileSystem.DirectoryExists(text4))
						{
							text4 = null;
						}
					}
					if (string.IsNullOrEmpty(text4))
					{
						continue;
					}
					foreach (string file in MyFileSystem.GetFiles(text4, "*", MySearchOption.AllDirectories))
					{
						string extension = Path.GetExtension(file);
						string key = MyFileSystem.MakeRelativePath(Path.Combine(text4, "VisualScripts"), file);
						if (extension == ".vs" || extension == ".vsc")
						{
							if (m_relativePathsToAbsolute.ContainsKey(key))
							{
								m_relativePathsToAbsolute[key] = file;
								flag = true;
							}
							else
							{
								m_relativePathsToAbsolute.Add(key, file);
							}
						}
					}
				}
			}
			if (m_relativePathsToAbsolute.Count == 0)
			{
				return;
			}
			bool isRuntimeCompilationSupported = MyVRage.Platform.Scripting.IsRuntimeCompilationSupported;
			IVSTAssemblyProvider vSTAssemblyProvider = MyVRage.Platform.Scripting.VSTAssemblyProvider;
			bool flag2 = false;
			if (!flag)
			{
				string assemblyName = ((text != null) ? MyVisualScriptingAssemblyHelper.MakeAssemblyName(text) : "VisualScripts");
				try
				{
					flag2 = vSTAssemblyProvider.TryLoad(assemblyName, isRuntimeCompilationSupported);
				}
				catch (FileNotFoundException)
				{
				}
				catch (Exception ex2)
				{
					MyLog.Default.Error("Cannot load visual script assembly: " + ex2.Message);
				}
			}
			if ((flag || !flag2) && MyVRage.Platform.Scripting.IsRuntimeCompilationSupported)
			{
				vSTAssemblyProvider.Init(m_relativePathsToAbsolute.Values, CampaignModPath);
			}
			else if (!flag2)
			{
				MyLog.Default.Error("Precompiled mod scripts assembly was not loaded. Scripts will not function.");
				return;
			}
			m_levelScripts = new CachingList<IMyLevelScript>();
<<<<<<< HEAD
			HashSet<string> scriptNames = new HashSet<string>(m_objectBuilder.LevelScriptFiles?.Select(Path.GetFileNameWithoutExtension) ?? Enumerable.Empty<string>());
=======
			string[] levelScriptFiles2 = m_objectBuilder.LevelScriptFiles;
			HashSet<string> scriptNames = new HashSet<string>(((levelScriptFiles2 != null) ? Enumerable.Select<string, string>((IEnumerable<string>)levelScriptFiles2, (Func<string, string>)Path.GetFileNameWithoutExtension) : null) ?? Enumerable.Empty<string>());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			vSTAssemblyProvider.GetLevelScriptInstances(scriptNames).ForEach(delegate(IMyLevelScript script)
			{
				m_levelScripts.Add(script);
			});
			m_levelScripts.ApplyAdditions();
<<<<<<< HEAD
			m_runningLevelScriptNames = m_levelScripts.Select((IMyLevelScript x) => x.GetType().Name).ToArray();
=======
			m_runningLevelScriptNames = Enumerable.ToArray<string>(Enumerable.Select<IMyLevelScript, string>((IEnumerable<IMyLevelScript>)m_levelScripts, (Func<IMyLevelScript, string>)((IMyLevelScript x) => x.GetType().Name)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_failedLevelScriptExceptionTexts = new string[m_runningLevelScriptNames.Length];
			m_smManager = new MyVSStateMachineManager();
			foreach (string stateMachineDefinitionFilePath in m_stateMachineDefinitionFilePaths)
			{
				m_smManager.AddMachine(stateMachineDefinitionFilePath);
			}
			if (m_objectBuilder != null && m_objectBuilder.ScriptStateMachineManager != null)
			{
				foreach (MyObjectBuilder_ScriptStateMachineManager.CursorStruct activeStateMachine in m_objectBuilder.ScriptStateMachineManager.ActiveStateMachines)
				{
					m_smManager.Restore(activeStateMachine.StateMachineName, activeStateMachine.Cursors);
				}
			}
			ConvertOldWaypoints();
			UpdateFoldersFromWaypoints();
		}

		private void ConvertOldWaypoints()
		{
			List<MyEntity> list = new List<MyEntity>();
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				if (IsOldWaypoint(entity))
				{
					list.Add(entity);
				}
			}
			foreach (MyEntity item in list)
			{
				MyEntities.Close(item);
				MyEntities.DeleteRememberedEntities();
				MyEntities.Add(CreateWaypoint(item.Name, item.EntityId, item.PositionComp.WorldMatrixRef));
			}
		}

		private bool IsOldWaypoint(MyEntity ent)
		{
			if (ent.Name == null)
			{
				return false;
			}
			if (ent.Name.Length < WAYPOINT_NAME_PREFIX.Length || !WAYPOINT_NAME_PREFIX.Equals(ent.Name.Substring(0, WAYPOINT_NAME_PREFIX.Length)))
			{
				return false;
			}
			if (ent is MyWaypoint)
			{
				return false;
			}
			return true;
		}

		private void UpdateFoldersFromWaypoints()
		{
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				MyWaypoint myWaypoint = entity as MyWaypoint;
				if (myWaypoint != null)
				{
					CreateFoldersFromPath(myWaypoint.Path);
				}
			}
		}

		public override void UpdateBeforeSimulation()
		{
			if (!Session.IsServer)
			{
				return;
			}
			if (m_smManager != null)
			{
				m_smManager.Update();
			}
			if (m_levelScripts == null)
			{
				return;
			}
			if (VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate != null)
			{
				VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate();
			}
			if (m_updateCounter % 10 == 0 && VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate10 != null)
			{
				VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate10();
			}
			if (m_updateCounter % 100 == 0 && VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate100 != null)
			{
				VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate100();
			}
			if (m_updateCounter % 1000 == 0 && VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate1000 != null)
			{
				VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate1000();
			}
			if (++m_updateCounter > 1000)
			{
				m_updateCounter = 0;
			}
			foreach (IMyLevelScript levelScript in m_levelScripts)
			{
				try
				{
					if (m_firstUpdate)
					{
						levelScript.GameStarted();
					}
					else
					{
						levelScript.Update();
					}
				}
				catch (Exception ex)
				{
					string name = levelScript.GetType().Name;
					for (int i = 0; i < m_runningLevelScriptNames.Length; i++)
					{
						if (m_runningLevelScriptNames[i] == name)
						{
							m_runningLevelScriptNames[i] += " - failed";
							m_failedLevelScriptExceptionTexts[i] = ex.ToString();
						}
					}
					m_levelScripts.Remove(levelScript);
				}
			}
			m_levelScripts.ApplyRemovals();
			m_firstUpdate = false;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			LiveDebugging();
			foreach (MyUIString value in m_UIStrings.Values)
			{
				m_UIStringBuilder.Clear();
				m_UIStringBuilder.Append(value.Text);
				MyGuiManager.DrawString(value.Font, m_UIStringBuilder.ToString(), value.NormalizedCoord, value.Scale, null, value.DrawAlign);
			}
		}

		public override void LoadData()
		{
			base.LoadData();
			if (!MySessionComponentExtDebug.Static.IsHandlerRegistered(LiveDebugging_ReceivedMessageHandler))
			{
				MySessionComponentExtDebug.Static.ReceivedMsg += LiveDebugging_ReceivedMessageHandler;
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			DisposeRunningScripts();
			SendDisconnectMessage();
			m_UIStrings.Clear();
<<<<<<< HEAD
			MyVSStateMachineFinalNode.Finished = (Action<string, string, bool, bool>)Delegate.Remove(MyVSStateMachineFinalNode.Finished, new Action<string, string, bool, bool>(OnSMFinished));
			Session = null;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void DisposeRunningScripts()
		{
			if (m_levelScripts == null)
			{
				return;
			}
			foreach (IMyLevelScript levelScript in m_levelScripts)
			{
				levelScript.GameFinished();
				levelScript.Dispose();
			}
			m_smManager.Dispose();
			m_smManager = null;
			m_levelScripts.Clear();
			m_levelScripts.ApplyRemovals();
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			if (!Session.IsServer)
			{
				return null;
			}
			m_objectBuilder.ScriptStateMachineManager = m_smManager?.GetObjectBuilder();
			m_objectBuilder.FirstRun = m_firstUpdate;
			m_objectBuilder.Questlog = MyHud.Questlog.GetObjectBuilder();
			MyHighlightSystem component = MySession.Static.GetComponent<MyHighlightSystem>();
			if (component != null)
			{
				m_objectBuilder.ExclusiveHighlights = component.GetExclusiveHighlightsObjectBuilder();
			}
<<<<<<< HEAD
			m_objectBuilder.BoardScreens = m_boardScreens.Select((KeyValuePair<string, MyGuiScreenBoard> x) => x.Value.GetBoardObjectBuilder(x.Key)).ToArray();
=======
			m_objectBuilder.BoardScreens = Enumerable.ToArray<MyObjectBuilder_BoardScreen>(Enumerable.Select<KeyValuePair<string, MyGuiScreenBoard>, MyObjectBuilder_BoardScreen>((IEnumerable<KeyValuePair<string, MyGuiScreenBoard>>)m_boardScreens, (Func<KeyValuePair<string, MyGuiScreenBoard>, MyObjectBuilder_BoardScreen>)((KeyValuePair<string, MyGuiScreenBoard> x) => x.Value.GetBoardObjectBuilder(x.Key))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return m_objectBuilder;
		}

		public void Reset()
		{
			if (m_smManager != null)
			{
				m_smManager.Dispose();
			}
			m_firstUpdate = true;
		}

		private void FixFilepathsPlatform()
		{
			if (m_objectBuilder.LevelScriptFiles != null)
			{
				for (int i = 0; i < m_objectBuilder.LevelScriptFiles.Length; i++)
				{
					string filePath = m_objectBuilder.LevelScriptFiles[i];
					string text = FixFilepathPlatform(filePath);
					m_objectBuilder.LevelScriptFiles[i] = text;
				}
			}
			if (m_objectBuilder.StateMachines != null)
			{
				for (int j = 0; j < m_objectBuilder.StateMachines.Length; j++)
				{
					string filePath2 = m_objectBuilder.StateMachines[j];
					string text2 = FixFilepathPlatform(filePath2);
					m_objectBuilder.StateMachines[j] = text2;
				}
			}
		}

		private string FixFilepathPlatform(string filePath)
		{
			if (!string.IsNullOrEmpty(((MyContentPath)Path.Combine(CampaignModPath ?? MyFileSystem.ContentPath, filePath)).GetExitingFilePath()))
			{
				return filePath;
			}
			string text = filePath;
			int num = filePath.LastIndexOf("Scripts\\");
			if (num != -1)
			{
				text = filePath.Substring(0, num) + "PC\\Scripts\\" + filePath.Substring(num + 8, filePath.Length - (num + 8));
			}
			if (!string.IsNullOrEmpty(((MyContentPath)Path.Combine(CampaignModPath ?? MyFileSystem.ContentPath, text)).GetExitingFilePath()))
			{
				return text;
			}
			return filePath;
		}

		public void CreateUIString(long id, MyUIString UIString)
		{
			m_UIStrings[id] = UIString;
		}

		public void RemoveUIString(long id)
		{
			m_UIStrings.Remove(id);
		}

		public void CreateBoardScreen(string boardId, float normalizedPosX, float normalizedPosY, float normalizedSizeX, float normalizedSizeY)
		{
			Vector2 screenTextLeftTopPosition = MyGuiManager.GetScreenTextLeftTopPosition();
			MyGuiScreenBoard myGuiScreenBoard = new MyGuiScreenBoard(new Vector2(normalizedPosX, normalizedPosY), screenTextLeftTopPosition, new Vector2(normalizedSizeX, normalizedSizeY));
			if (!Sync.IsDedicated)
			{
				MyScreenManager.AddScreen(myGuiScreenBoard);
			}
			m_boardScreens[boardId] = myGuiScreenBoard;
		}

		public void RemoveBoardScreen(string boardId)
		{
			if (m_boardScreens.TryGetValue(boardId, out var value))
			{
				if (!Sync.IsDedicated)
				{
					MyScreenManager.RemoveScreen(value);
				}
				m_boardScreens.Remove(boardId);
			}
		}

		public void AddColumn(string boardId, string columnId, MyGuiScreenBoard.MyColumn column)
		{
			if (m_boardScreens.TryGetValue(boardId, out var value))
			{
				value.AddColumn(columnId, column);
			}
		}

		public void AddColumn(string boardId, string columnId, float width, string headerText, MyGuiDrawAlignEnum headerDrawAlign, MyGuiDrawAlignEnum columnDrawAlign)
		{
			if (m_boardScreens.TryGetValue(boardId, out var value))
			{
				value.AddColumn(columnId, width, headerText, headerDrawAlign, columnDrawAlign);
			}
		}

		public void RemoveColumn(string boardId, string columnId)
		{
			if (m_boardScreens.TryGetValue(boardId, out var value))
			{
				value.RemoveColumn(boardId);
			}
		}

		public void AddRow(string boardId, string rowId)
		{
			if (m_boardScreens.TryGetValue(boardId, out var value))
			{
				value.AddRow(rowId);
			}
		}

		public void RemoveRow(string boardId, string rowId)
		{
			if (m_boardScreens.TryGetValue(boardId, out var value))
			{
				value.RemoveRow(rowId);
			}
		}

		public void SetCell(string boardId, string rowId, string columnId, string text)
		{
			if (m_boardScreens.TryGetValue(boardId, out var value))
			{
				value.SetCell(rowId, columnId, text);
			}
		}

		public void SetRowRanking(string boardId, string rowId, int ranking)
		{
			if (m_boardScreens.TryGetValue(boardId, out var value))
			{
				value.SetRowRanking(rowId, ranking);
			}
		}

		public void SortByColumn(string boardId, string columnId, bool ascending)
		{
			if (m_boardScreens.TryGetValue(boardId, out var value))
			{
				value.SortByColumn(columnId, ascending);
			}
		}

		public void SortByRanking(string boardId, bool ascending)
		{
			if (m_boardScreens.TryGetValue(boardId, out var value))
			{
				value.SortByRanking(ascending);
			}
		}

		public void ShowOrderInColumn(string boardId, string columnId)
		{
			if (m_boardScreens.TryGetValue(boardId, out var value))
			{
				value.ShowOrderInColumn(columnId);
			}
		}

		public void SetColumnVisibility(string boardId, string columnId, bool visible)
		{
			if (m_boardScreens.TryGetValue(boardId, out var value))
			{
				value.SetColumnVisibility(columnId, visible);
			}
		}

		private void LiveDebugging()
		{
			if (!Session.IsServer)
			{
				return;
			}
			if (m_liveDebuggingCounter == 0)
			{
				m_liveDebuggingCounter = 120;
				SendStatusMessage();
				if (!m_hadClients && MySessionComponentExtDebug.Static.HasClients)
				{
					MyDebugDrawSettings.ENABLE_DEBUG_DRAW = true;
					MyDebugDrawSettings.DEBUG_DRAW_WAYPOINTS = true;
				}
				m_hadClients = MySessionComponentExtDebug.Static.HasClients;
			}
			if (MyVRage.Platform.Scripting.VSTAssemblyProvider.DebugEnabled && m_smManager?.RunningMachines != null)
			{
				foreach (MyVSStateMachine runningMachine in m_smManager.RunningMachines)
				{
					MyVisualScriptingDebug.LogStateMachine(runningMachine);
				}
				SendLoggedNodesMessage();
			}
			MyVisualScriptingDebug.Clear();
			m_liveDebuggingCounter--;
		}

		private void SendStatusMessage()
		{
			VSStatusMsg vSStatusMsg = default(VSStatusMsg);
			vSStatusMsg.World = MySession.Static.CurrentPath;
			vSStatusMsg.VSComponent = (MyObjectBuilder_VisualScriptManagerSessionComponent)GetObjectBuilder();
			VSStatusMsg msg = vSStatusMsg;
			MySessionComponentExtDebug.Static.SendMessageToClients(msg);
		}

		private void SendDisconnectMessage()
		{
			MySessionComponentExtDebug.Static.SendMessageToClients(default(VSDisconnectMsg));
		}

		private void LiveDebugging_ReceivedMessageHandler(MyExternalDebugStructures.CommonMsgHeader messageHeader, byte[] messageData)
		{
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSReqEntitiesMsg>(ref messageHeader, messageData, out var _))
			{
				SendEntitiesToClients();
			}
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSReqTriggersMsg>(ref messageHeader, messageData, out var _))
			{
				SendAllTriggers();
			}
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSReqEntityIdsMsg>(ref messageHeader, messageData, out var _))
			{
				SendEntityIDs();
			}
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSReqWaypointCreateMsg>(ref messageHeader, messageData, out var _))
			{
				CreateWaypoint();
			}
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSReqWaypointDeleteMsg>(ref messageHeader, messageData, out var outMsg5))
			{
				DeleteWaypoint(outMsg5.Id);
			}
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSFolderDeletedMsg>(ref messageHeader, messageData, out var outMsg6))
			{
				DeleteFolder(outMsg6.Path);
			}
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSFolderCreatedMsg>(ref messageHeader, messageData, out var outMsg7))
			{
				CreateFoldersFromPath(outMsg7.Path);
			}
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSWaypointRenamedMsg>(ref messageHeader, messageData, out var outMsg8))
			{
				RenameWaypoint(outMsg8.Id, outMsg8.Name);
			}
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSFolderRenamedMsg>(ref messageHeader, messageData, out var outMsg9))
			{
				RenameFolder(outMsg9.OldPath, outMsg9.NewPath);
			}
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSWaypointPathUpdatedMsg>(ref messageHeader, messageData, out var outMsg10) && MyEntities.TryGetEntityById(outMsg10.Id, out MyWaypoint entity, allowClosed: false))
			{
				entity.Path = outMsg10.Path;
			}
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSWaypointVisibilityUpdatedMsg>(ref messageHeader, messageData, out var outMsg11) && MyEntities.TryGetEntityById(outMsg11.Id, out MyWaypoint entity2, allowClosed: false))
			{
				entity2.Visible = outMsg11.Visible;
			}
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSWaypointFreezeUpdatedMsg>(ref messageHeader, messageData, out var outMsg12) && MyEntities.TryGetEntityById(outMsg12.Id, out MyWaypoint entity3, allowClosed: false))
			{
				entity3.Freeze = outMsg12.Freeze;
			}
			if (MyExternalDebugStructures.ReadMessageFromPtr<VSSaveWorldMsg>(ref messageHeader, messageData, out var _) && MySession.Static != null && !MyAsyncSaving.InProgress)
			{
				MyAsyncSaving.Start(delegate
				{
					MySector.ResetEyeAdaptation = true;
				});
			}
			if (!MyExternalDebugStructures.ReadMessageFromPtr<VSActivatesStatesMsg>(ref messageHeader, messageData, out var outMsg14) || MySession.Static == null)
			{
				return;
			}
			foreach (MyVSStateMachine runningMachine in SMManager.RunningMachines)
			{
				if (!(runningMachine.Name == outMsg14.SMName))
				{
					continue;
				}
				foreach (MyStateMachineCursor activeCursor in runningMachine.ActiveCursors)
				{
					runningMachine.DeleteCursor(activeCursor.Id);
				}
				string[] activeStates = outMsg14.ActiveStates;
				foreach (string nodeName in activeStates)
				{
					runningMachine.CreateCursor(nodeName);
				}
				break;
			}
		}

		public void SendEntitiesToClients()
		{
<<<<<<< HEAD
			IEnumerable<MyWaypoint> enumerable = MyEntities.GetEntities().OfType<MyWaypoint>();
=======
			IEnumerable<MyWaypoint> enumerable = Enumerable.OfType<MyWaypoint>((IEnumerable)MyEntities.GetEntities());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			List<MyObjectBuilder_Waypoint> list = new List<MyObjectBuilder_Waypoint>();
			foreach (MyWaypoint item in enumerable)
			{
				list.Add((MyObjectBuilder_Waypoint)item.GetObjectBuilder());
			}
			VSEntitiesMsg vSEntitiesMsg = default(VSEntitiesMsg);
<<<<<<< HEAD
			vSEntitiesMsg.Folders = m_worldOutlineFolders.ToArray();
=======
			vSEntitiesMsg.Folders = Enumerable.ToArray<string>((IEnumerable<string>)m_worldOutlineFolders);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			vSEntitiesMsg.Waypoints = list;
			VSEntitiesMsg msg = vSEntitiesMsg;
			MySessionComponentExtDebug.Static.SendMessageToClients(msg);
		}

		public static MyWaypoint CreateWaypoint(string name, long id, MatrixD worldMatrix)
		{
			MyObjectBuilder_Waypoint myObjectBuilder_Waypoint = new MyObjectBuilder_Waypoint();
			myObjectBuilder_Waypoint.Name = name;
			myObjectBuilder_Waypoint.EntityId = id;
			myObjectBuilder_Waypoint.PositionAndOrientation = new MyPositionAndOrientation(worldMatrix.Translation, worldMatrix.Forward, worldMatrix.Up);
			myObjectBuilder_Waypoint.Visible = true;
			myObjectBuilder_Waypoint.Freeze = false;
			MyWaypoint myWaypoint = MyEntityFactory.CreateEntity<MyWaypoint>(myObjectBuilder_Waypoint);
			myWaypoint.Init(myObjectBuilder_Waypoint);
			myWaypoint.Components.Remove<MyPhysicsComponentBase>();
			return myWaypoint;
		}

		public static void CreateWaypoint()
		{
			int num = 0;
			string name;
			MyEntity entity;
			do
			{
				name = WAYPOINT_NAME_PREFIX + num++;
			}
			while (MyEntities.TryGetEntityByName(name, out entity));
			MatrixD worldMatrix = MyAPIGateway.Session.Camera.WorldMatrix;
			MyWaypoint myWaypoint = CreateWaypoint(name, MyEntityIdentifier.AllocateId(), worldMatrix);
			MyEntities.Add(myWaypoint);
			if (MyGuiScreenScriptingTools.Static != null)
			{
				MyGuiScreenScriptingTools.Static.OnWaypointAdded(myWaypoint);
			}
			VSWaypointCreatedMsg vSWaypointCreatedMsg = default(VSWaypointCreatedMsg);
			vSWaypointCreatedMsg.Waypoint = (MyObjectBuilder_Waypoint)myWaypoint.GetObjectBuilder();
			VSWaypointCreatedMsg msg = vSWaypointCreatedMsg;
			MySessionComponentExtDebug.Static.SendMessageToClients(msg);
		}

		private void DeleteWaypoint(long id)
		{
			if (MyEntities.TryGetEntityById(id, out var entity))
			{
				entity.Close();
			}
		}

		private void DeleteFolder(string path)
		{
<<<<<<< HEAD
			m_worldOutlineFolders.RemoveWhere((string x) => x.StartsWith(path));
=======
			m_worldOutlineFolders.RemoveWhere((Predicate<string>)((string x) => x.StartsWith(path)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void CreateFoldersFromPath(string path)
		{
			for (int num = path.LastIndexOf('\\'); num != -1; num = path.LastIndexOf('\\'))
			{
				m_worldOutlineFolders.Add(path);
				path = path.Remove(num);
			}
			if (!string.IsNullOrEmpty(path))
			{
				m_worldOutlineFolders.Add(path);
			}
		}

		private void RenameWaypoint(long id, string name)
		{
			if (MyEntities.TryGetEntityById(id, out MyWaypoint entity, allowClosed: false))
			{
				entity.Name = name;
				MyEntities.SetEntityName(entity, possibleRename: false);
				if (MyGuiScreenScriptingTools.Static != null)
				{
					MyGuiScreenScriptingTools.Static.UpdateWaypointList();
					MyGuiScreenScriptingTools.Static.UpdateTriggerList();
				}
			}
		}

		private void RenameFolder(string oldPath, string newPath)
		{
<<<<<<< HEAD
			List<string> list = new List<string>();
			foreach (string worldOutlineFolder in m_worldOutlineFolders)
			{
				if (worldOutlineFolder.StartsWith(oldPath))
				{
					list.Add(worldOutlineFolder.Replace(oldPath, newPath));
				}
			}
=======
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			List<string> list = new List<string>();
			Enumerator<string> enumerator = m_worldOutlineFolders.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					if (current.StartsWith(oldPath))
					{
						list.Add(current.Replace(oldPath, newPath));
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			DeleteFolder(oldPath);
			foreach (string item in list)
			{
				m_worldOutlineFolders.Add(item);
			}
		}

		private void SendAllTriggers()
		{
			List<MyTriggerComponent> allTriggers = MySessionComponentTriggerSystem.Static.GetAllTriggers();
<<<<<<< HEAD
			List<MyObjectBuilder_AreaTrigger> list = (from x in allTriggers.ConvertAll((MyTriggerComponent x) => (MyObjectBuilder_TriggerBase)x.Serialize())
				where x is MyObjectBuilder_AreaTrigger
				select x).Cast<MyObjectBuilder_AreaTrigger>().ToList();
=======
			List<MyObjectBuilder_AreaTrigger> list = Enumerable.ToList<MyObjectBuilder_AreaTrigger>(Enumerable.Cast<MyObjectBuilder_AreaTrigger>((IEnumerable)Enumerable.Where<MyObjectBuilder_TriggerBase>((IEnumerable<MyObjectBuilder_TriggerBase>)allTriggers.ConvertAll((MyTriggerComponent x) => (MyObjectBuilder_TriggerBase)x.Serialize()), (Func<MyObjectBuilder_TriggerBase, bool>)((MyObjectBuilder_TriggerBase x) => x is MyObjectBuilder_AreaTrigger))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			List<long> list2 = allTriggers.ConvertAll((MyTriggerComponent x) => x.Entity.EntityId);
			VSTriggersMsg vSTriggersMsg = default(VSTriggersMsg);
			vSTriggersMsg.Triggers = list.ToArray();
			vSTriggersMsg.Parents = list2.ToArray();
			VSTriggersMsg msg = vSTriggersMsg;
			MySessionComponentExtDebug.Static.SendMessageToClients(msg);
		}

		private void SendEntityIDs()
		{
			List<SimpleEntityInfo> list = new List<SimpleEntityInfo>();
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				SimpleEntityInfo simpleEntityInfo = default(SimpleEntityInfo);
				simpleEntityInfo.Id = entity.EntityId;
				simpleEntityInfo.Name = entity.Name;
				simpleEntityInfo.DisplayName = entity.DisplayName;
				simpleEntityInfo.Type = entity.GetType().Name;
				SimpleEntityInfo item = simpleEntityInfo;
				list.Add(item);
			}
			VSEntityIdsMsg vSEntityIdsMsg = default(VSEntityIdsMsg);
			vSEntityIdsMsg.Data = list.ToArray();
			VSEntityIdsMsg msg = vSEntityIdsMsg;
			MySessionComponentExtDebug.Static.SendMessageToClients(msg);
		}

		private void SendLoggedNodesMessage()
		{
			VSLoggedNodesMsg vSLoggedNodesMsg = default(VSLoggedNodesMsg);
			vSLoggedNodesMsg.Time = MySandboxGame.TotalTimeInMilliseconds;
<<<<<<< HEAD
			vSLoggedNodesMsg.Nodes = MyVisualScriptingDebug.LoggedNodes.ToArray();
			vSLoggedNodesMsg.StateMachines = MyVisualScriptingDebug.StateMachines.ToArray();
=======
			vSLoggedNodesMsg.Nodes = Enumerable.ToArray<MyDebuggingNodeLog>((IEnumerable<MyDebuggingNodeLog>)MyVisualScriptingDebug.LoggedNodes);
			vSLoggedNodesMsg.StateMachines = Enumerable.ToArray<MyDebuggingStateMachine>((IEnumerable<MyDebuggingStateMachine>)MyVisualScriptingDebug.StateMachines);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			VSLoggedNodesMsg msg = vSLoggedNodesMsg;
			MySessionComponentExtDebug.Static.SendMessageToClients(msg);
		}
	}
}
