using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Helpers.InputRecording;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Models;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyTestingToolHelper
	{
		public enum MyTestingToolHelperStateOuterEnum
		{
			Disabled,
			Idle,
			Action_1
		}

		public enum MySpawningCycleMicroEnum
		{
			ReloadAndClear,
			CheckIfLoaded,
			FindBlock,
			FoundBlock,
			CreateBlock,
			SetCamera,
			TakeScreenshot,
			SaveScene,
			PrepareTestCase,
			StartReplay,
			StopReplay,
			FinalSave,
			Final
		}

		public enum MyViewsEnum
		{
			Fr,
			Ba,
			Le,
			Ri,
			To,
			Bo
		}

		protected class MyBlockTestGenerationState
		{
			public bool IsFinalPhase;

			public int SessionOrder;

			public int SessionOrder_Max;

			public int CurrentLOD;

			public int ScreenshotCount;

			public int ConstructionPhasesMaxIndex;

			public int CurrentConstructionPhase;

			public int LastTimeStamp = 200;

			public string CurrentBlockName = string.Empty;

			public List<string> UsedKeys = new List<string>();

			public DateTime TestStart = DateTime.UtcNow;

			public Vector3I BlockSize = Vector3I.Zero;

			public MyCubeSize BlockGrid;

			public MyViewsEnum CurrentView;

			public MyCubeBlockDefinition currentBlock;

			public MyCameraSnapshot CameraSnapshot;

			public MyBlockSnapshot BlockSnapshot;

			public List<MySnapshot> Snapshots;

			public string BasePath = string.Empty;

			public string ResultSaveName = string.Empty;

			public string ResultTestCaseName = string.Empty;

			public string SourceSaveWithBlockPath = string.Empty;

			public string ResultTestCaseSavePath = string.Empty;
		}

		public MySlimBlock PhotoBlock;

		public string SelectedCategory = "LargeBlocks";

		public bool IgnoreConstructionPhase;

		private const int MAX_VIEWS = 5;

		private const int MAX_LODS = 5;

		private const int DEFAULT_LODS = 3;

		private const int TIMESTAMP = 400;

		private bool m_syncRendering;

		private bool m_smallBlock;

		private bool m_isSaving;

		private bool m_savingFinished;

		private bool m_isLoading;

		private bool m_loadingFinished;

		private MyTestingToolHelperStateOuterEnum m_stateOuter;

		private MyBlockTestGenerationState m_blockTestGenerationState;

		private static MyTestingToolHelper m_instance = null;

		private Dictionary<MyViewsEnum, Vector3D> myDirection = new Dictionary<MyViewsEnum, Vector3D>
		{
			{
				MyViewsEnum.Fr,
				Vector3D.Forward
			},
			{
				MyViewsEnum.Ba,
				Vector3D.Backward
			},
			{
				MyViewsEnum.Le,
				Vector3D.Left
			},
			{
				MyViewsEnum.Ri,
				Vector3D.Right
			},
			{
				MyViewsEnum.To,
				Vector3D.Up
			},
			{
				MyViewsEnum.Bo,
				Vector3D.Down
			}
		};

		private int m_stateInner;

		private MySpawningCycleMicroEnum m_stateMicro;

		private List<string> m_blocksInCategoryList = new List<string>();

		private int m_timer;

		private uint m_renderEntityId;

		public long timerRepetitions;

		internal Dictionary<string, MyGuiBlockCategoryDefinition> m_categories;

		public static float ScreenshotDistanceMultiplier { get; set; } = 1.6f;


		public static int m_timer_Max { get; set; } = 100;


		public static bool IsSmallGridSelected { get; set; } = true;


		public static bool IsLargeGridSelected { get; set; } = true;


		public static string CurrentTestPath { get; set; }

		public static MyTestingToolHelper Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new MyTestingToolHelper();
				}
				return m_instance;
			}
		}

		public MyTestingToolHelperStateOuterEnum StateOuter
		{
			get
			{
				return m_stateOuter;
			}
			set
			{
				if (CanChangeOuterStateTo(value))
				{
					m_stateOuter = value;
					m_stateInner = 0;
					m_stateMicro = MySpawningCycleMicroEnum.ReloadAndClear;
					OnStateOuterUpdate();
				}
			}
		}

		public bool IsEnabled { get; private set; }

		public bool IsIdle { get; private set; }

		public bool NeedsUpdate { get; private set; }

		private MyLODDescriptor[] CurrentLODs
		{
			get
			{
				MyLODDescriptor[] result = new MyLODDescriptor[0];
				if (PhotoBlock.FatBlock != null && PhotoBlock.FatBlock.Model != null && PhotoBlock.FatBlock.Model.LODs != null)
				{
					result = PhotoBlock.FatBlock.Model.LODs;
				}
				else if (Enumerable.Count<MyCubeBlockDefinition.BuildProgressModel>((IEnumerable<MyCubeBlockDefinition.BuildProgressModel>)m_blockTestGenerationState.currentBlock.BuildProgressModels) > m_blockTestGenerationState.CurrentConstructionPhase)
				{
					result = MyModels.GetModelOnlyData(m_blockTestGenerationState.currentBlock.BuildProgressModels[m_blockTestGenerationState.CurrentConstructionPhase].File).LODs;
				}
				return result;
			}
		}

		public static string ScreenshotsDir
		{
			get
			{
				string path = "SpaceEngineers";
				return Path.Combine(Environment.GetFolderPath((SpecialFolder)26), path, "Screenshots");
			}
		}

		private static string UserSaveFolder
		{
			get
			{
				bool flag = false;
				string text = "SpaceEngineers";
				return Path.Combine(Environment.GetFolderPath((SpecialFolder)26), text + (flag ? "Dedicated" : ""), "Saves");
			}
		}

		private static string TestCasesDir
		{
			get
			{
				bool flag = false;
				string text = "SpaceEngineers";
				return Path.Combine(Environment.GetFolderPath((SpecialFolder)26), text + (flag ? "Dedicated" : ""), "TestCases");
			}
		}

		public static string GameLogPath
		{
			get
			{
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				bool flag = false;
				string text = "SpaceEngineers";
				string text2 = Path.Combine(Environment.GetFolderPath((SpecialFolder)26), text + (flag ? "Dedicated" : ""));
				string text3 = Path.Combine(text2, text + (flag ? "Dedicated" : "") + ".log");
				if (File.Exists(text3))
				{
					return text3;
				}
				return ((FileSystemInfo)Enumerable.First<FileInfo>((IEnumerable<FileInfo>)Enumerable.ToList<FileInfo>((IEnumerable<FileInfo>)Enumerable.OrderByDescending<FileInfo, DateTime>((IEnumerable<FileInfo>)new DirectoryInfo(text2).GetFiles(), (Func<FileInfo, DateTime>)((FileInfo f) => ((FileSystemInfo)f).get_LastWriteTime()))))).get_FullName().ToString();
			}
		}

		public static string ConfigFile
		{
			get
			{
				string text = "SpaceEngineers";
				return Path.Combine(Environment.GetFolderPath((SpecialFolder)26), text, text + ".cfg");
			}
		}

		public static string TestResultFilename => "result";

		public static string LastTestRunResultFilename => "last_test_run";

		public bool CanChangeOuterStateTo(MyTestingToolHelperStateOuterEnum value)
		{
			if (m_stateOuter == value)
			{
				return false;
			}
			switch (m_stateOuter)
			{
			case MyTestingToolHelperStateOuterEnum.Disabled:
				return false;
			case MyTestingToolHelperStateOuterEnum.Idle:
				return true;
			case MyTestingToolHelperStateOuterEnum.Action_1:
				if (value == MyTestingToolHelperStateOuterEnum.Idle || value == MyTestingToolHelperStateOuterEnum.Disabled)
				{
					return true;
				}
				return false;
			default:
				return true;
			}
		}

		private void ChangeStateOuter_Force(MyTestingToolHelperStateOuterEnum value)
		{
			if (m_stateOuter != value)
			{
				m_stateOuter = value;
				OnStateOuterUpdate();
			}
		}

		private int GetLODCount()
		{
			int num = Enumerable.Count<MyLODDescriptor>((IEnumerable<MyLODDescriptor>)CurrentLODs);
			if (num == 0)
			{
				return 3;
			}
			return num;
		}

		public MyTestingToolHelper()
		{
			ChangeStateOuter_Force(MyTestingToolHelperStateOuterEnum.Idle);
		}

		public void Update()
		{
			if (!NeedsUpdate)
			{
				return;
			}
			m_timer--;
			if (m_timer < 0)
			{
				timerRepetitions++;
				m_timer = m_timer_Max;
				switch (StateOuter)
				{
				case MyTestingToolHelperStateOuterEnum.Action_1:
					Update_Action1();
					break;
				case MyTestingToolHelperStateOuterEnum.Disabled:
				case MyTestingToolHelperStateOuterEnum.Idle:
					break;
				}
			}
		}

		private void Update_Action1()
		{
			switch (m_stateInner)
			{
			case 0:
				Action1_State0_PrepareBase();
				break;
			case 1:
				Action1_State1_SpawningCyclePreparation();
				break;
			case 2:
				Action1_State2_SpawningCycle();
				break;
			case 3:
				Action1_State3_Finish();
				break;
			}
		}

		public void OnStateOuterUpdate()
		{
			IsEnabled = m_stateOuter != MyTestingToolHelperStateOuterEnum.Disabled;
			IsIdle = m_stateOuter == MyTestingToolHelperStateOuterEnum.Idle;
			NeedsUpdate = IsEnabled && !IsIdle;
		}

		public void Action_SpawnBlockSaveTestReload()
		{
			FillCategoryWithBlocks();
			StateOuter = MyTestingToolHelperStateOuterEnum.Action_1;
		}

		public void Action_Test()
		{
			MyCubeBlockDefinition large = MyDefinitionManager.Static.GetDefinitionGroup("Monolith").Large;
			MyCubeBuilder component = MySession.Static.GetComponent<MyCubeBuilder>();
			if (component != null)
			{
				MatrixD worldMatrixAdd = MatrixD.Identity;
				component.AddBlocksToBuildQueueOrSpawn(large, ref worldMatrixAdd, new Vector3I(0, 0, 0), new Vector3I(0, 0, 0), new Vector3I(0, 0, 0), Quaternion.Identity);
			}
		}

		public void ReloadAndClearScene()
		{
			m_blockTestGenerationState.SessionOrder++;
			m_blockTestGenerationState.TestStart = DateTime.UtcNow;
			Load(m_blockTestGenerationState.BasePath);
			m_blockTestGenerationState.ResultTestCaseName = string.Empty;
			m_blockTestGenerationState.ScreenshotCount = 0;
			m_blockTestGenerationState.CurrentLOD = 0;
			m_blockTestGenerationState.CurrentView = Enumerable.First<MyViewsEnum>((IEnumerable<MyViewsEnum>)myDirection.Keys);
			m_blockTestGenerationState.CurrentConstructionPhase = 0;
			m_blockTestGenerationState.Snapshots = new List<MySnapshot>();
			m_blockTestGenerationState.IsFinalPhase = false;
			m_renderEntityId = 0u;
			m_timer = 0;
			timerRepetitions = 0L;
			if (PhotoBlock != null)
			{
				PhotoBlock.CubeGrid.Delete();
				PhotoBlock = null;
			}
			MySector.Lodding.CurrentSettings.Global.EnableLodSelection = true;
			MySector.Lodding.CurrentSettings.Global.IsUpdateEnabled = true;
		}

		private Vector3D Up(MyViewsEnum view)
		{
			return view switch
			{
				MyViewsEnum.To => Vector3D.Forward, 
				MyViewsEnum.Bo => Vector3D.Backward, 
				_ => Vector3D.Up, 
			};
		}

		private void FillCategoryWithBlocks()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<string> enumerator = MyDefinitionManager.Static.GetCategories()[SelectedCategory].ItemIds.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.get_Current();
					string item = current.Substring(current.IndexOf("/") + 1);
					m_blocksInCategoryList.Add(item);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private void Action1_State0_PrepareBase()
		{
			MySession.Static.Name = MySession.Static.Name.Replace(":", "-") + "_BTests";
			SaveAs(MySession.Static.Name);
			m_stateInner = 1;
		}

		private void Action1_State1_SpawningCyclePreparation()
		{
			if (!m_isSaving && m_savingFinished)
			{
				m_blockTestGenerationState = new MyBlockTestGenerationState();
				m_blockTestGenerationState.BasePath = MySession.Static.CurrentPath;
				m_blockTestGenerationState.SessionOrder = -1;
				m_blockTestGenerationState.SessionOrder_Max = MyDefinitionManager.Static.GetDefinitionPairNames().Count * 2 * myDirection.Keys.Count;
				m_blockTestGenerationState.UsedKeys.Clear();
				m_blockTestGenerationState.CurrentBlockName = string.Empty;
				m_blockTestGenerationState.CurrentView = Enumerable.First<MyViewsEnum>((IEnumerable<MyViewsEnum>)myDirection.Keys);
				m_smallBlock = false;
				m_stateInner = 2;
			}
		}

		private void Action1_State2_SpawningCycle()
		{
			switch (m_stateMicro)
			{
			case MySpawningCycleMicroEnum.ReloadAndClear:
				CurrentTestPath = null;
				ReloadAndClearScene();
				m_stateMicro = MySpawningCycleMicroEnum.CheckIfLoaded;
				break;
			case MySpawningCycleMicroEnum.CheckIfLoaded:
				if (ConsumeLoadingCompletion())
				{
					m_stateMicro = MySpawningCycleMicroEnum.FindBlock;
				}
				break;
			case MySpawningCycleMicroEnum.FindBlock:
				if (!m_smallBlock)
				{
					bool flag = false;
					if (SelectedCategory != null)
					{
						foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
						{
							List<string> blocksInCategoryList = m_blocksInCategoryList;
							MyStringHash subtypeId = MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName).Any.Id.SubtypeId;
							if (blocksInCategoryList.Contains(subtypeId.ToString()) && !m_blockTestGenerationState.UsedKeys.Contains(definitionPairName))
							{
								m_blockTestGenerationState.CurrentBlockName = definitionPairName;
								m_smallBlock = false;
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						m_stateInner = 3;
						break;
					}
				}
				m_stateMicro = MySpawningCycleMicroEnum.FoundBlock;
				break;
			case MySpawningCycleMicroEnum.FoundBlock:
			{
				MySession.Static.LocalCharacter.Render.Visible = false;
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(m_blockTestGenerationState.CurrentBlockName);
				MyCubeBlockDefinition myCubeBlockDefinition = null;
				if (definitionGroup != null)
				{
					if (m_smallBlock && IsSmallGridSelected)
					{
						myCubeBlockDefinition = definitionGroup.Small;
					}
					else
					{
						if (IsLargeGridSelected)
						{
							myCubeBlockDefinition = definitionGroup.Large;
						}
						m_blockTestGenerationState.UsedKeys.Add(m_blockTestGenerationState.CurrentBlockName);
					}
				}
				if (myCubeBlockDefinition != null && myCubeBlockDefinition.Public)
				{
					m_blockTestGenerationState.currentBlock = myCubeBlockDefinition;
					m_blockTestGenerationState.ConstructionPhasesMaxIndex = Enumerable.Count<MyCubeBlockDefinition.BuildProgressModel>((IEnumerable<MyCubeBlockDefinition.BuildProgressModel>)m_blockTestGenerationState.currentBlock.BuildProgressModels) - 1;
					MySession.Static.SetCameraController(MyCameraControllerEnum.SpectatorFixed);
					m_stateMicro = MySpawningCycleMicroEnum.SaveScene;
				}
				else
				{
					m_stateMicro = MySpawningCycleMicroEnum.FindBlock;
				}
				m_smallBlock = !m_smallBlock;
				break;
			}
			case MySpawningCycleMicroEnum.CreateBlock:
				if (PhotoBlock == null)
				{
					PrepareRenderEntity(m_blockTestGenerationState.currentBlock);
					m_blockTestGenerationState.BlockSize = m_blockTestGenerationState.currentBlock.Size;
					m_blockTestGenerationState.BlockGrid = m_blockTestGenerationState.currentBlock.CubeSize;
				}
				else
				{
					PhotoBlock.SetBuildLevel(m_blockTestGenerationState.CurrentConstructionPhase);
					SetBlockLOD(m_blockTestGenerationState.CurrentLOD);
				}
				m_stateMicro = MySpawningCycleMicroEnum.SetCamera;
				break;
			case MySpawningCycleMicroEnum.SetCamera:
			{
				float cubeSize = MyDefinitionManager.Static.GetCubeSize(m_blockTestGenerationState.BlockGrid);
				if (m_blockTestGenerationState.CurrentView != 0)
				{
					MySpectator.Static.Position = myDirection[m_blockTestGenerationState.CurrentView] * ScreenshotDistanceMultiplier * cubeSize * m_blockTestGenerationState.BlockSize.AbsMax();
				}
				else
				{
					MySpectator.Static.Position = new Vector3D(0.8, 0.8, 0.8) * ScreenshotDistanceMultiplier * cubeSize * m_blockTestGenerationState.BlockSize.AbsMax();
				}
				SetCameraForTesting(MySpectator.Static.Position, m_blockTestGenerationState.CurrentView);
				PhotoBlock.SetBuildLevel(m_blockTestGenerationState.CurrentConstructionPhase);
				SetBlockLOD(m_blockTestGenerationState.CurrentLOD);
				CreateCompleteSnapshot();
				m_stateMicro = MySpawningCycleMicroEnum.TakeScreenshot;
				break;
			}
			case MySpawningCycleMicroEnum.TakeScreenshot:
			{
				MyDefinitionManager.Static.GetDefinitionGroup(m_blockTestGenerationState.CurrentBlockName);
				CreateCompleteSnapshot(TakeScreenShot: true);
				string path = "TestingToolResult" + m_blockTestGenerationState.ScreenshotCount + ".png";
				MyGuiSandbox.TakeScreenshot(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y, Path.Combine(Path.Combine(MyFileSystem.UserDataPath, "Screenshots"), path), ignoreSprites: true, showNotification: false);
				m_blockTestGenerationState.ScreenshotCount++;
				if (m_blockTestGenerationState.CurrentLOD < GetLODCount() - 1)
				{
					m_blockTestGenerationState.CurrentLOD++;
					m_stateMicro = MySpawningCycleMicroEnum.SetCamera;
					break;
				}
				m_blockTestGenerationState.CurrentLOD = 0;
				if (m_blockTestGenerationState.CurrentConstructionPhase <= m_blockTestGenerationState.ConstructionPhasesMaxIndex)
				{
					if (m_blockTestGenerationState.CurrentConstructionPhase == m_blockTestGenerationState.ConstructionPhasesMaxIndex)
					{
						m_blockTestGenerationState.CurrentConstructionPhase = int.MaxValue;
					}
					else
					{
						m_blockTestGenerationState.CurrentConstructionPhase++;
					}
					m_stateMicro = MySpawningCycleMicroEnum.SetCamera;
				}
				else if (m_blockTestGenerationState.CurrentView == Enumerable.Last<MyViewsEnum>((IEnumerable<MyViewsEnum>)myDirection.Keys))
				{
					m_stateMicro = MySpawningCycleMicroEnum.PrepareTestCase;
				}
				else
				{
					m_blockTestGenerationState.CurrentView++;
					m_blockTestGenerationState.CurrentConstructionPhase = 0;
					m_blockTestGenerationState.CurrentLOD = 0;
					m_stateMicro = MySpawningCycleMicroEnum.SetCamera;
				}
				break;
			}
			case MySpawningCycleMicroEnum.SaveScene:
				m_blockTestGenerationState.ResultSaveName = MySession.Static.Name + "_" + m_blockTestGenerationState.CurrentBlockName + (m_smallBlock ? "_L" : "_S");
				m_blockTestGenerationState.ResultTestCaseName = m_blockTestGenerationState.ResultSaveName + (m_syncRendering ? "-sync" : "-async");
				SaveAs(m_blockTestGenerationState.ResultSaveName);
				m_stateMicro = MySpawningCycleMicroEnum.CreateBlock;
				break;
			case MySpawningCycleMicroEnum.PrepareTestCase:
				PrepareTestCase();
				m_stateMicro = MySpawningCycleMicroEnum.StartReplay;
				break;
			case MySpawningCycleMicroEnum.StartReplay:
				MySessionComponentReplay.Static.StopRecording();
				MySessionComponentReplay.Static.StartReplay();
				m_stateMicro = MySpawningCycleMicroEnum.StopReplay;
				break;
			case MySpawningCycleMicroEnum.StopReplay:
				MySessionComponentReplay.Static.StopReplay();
				m_stateMicro = MySpawningCycleMicroEnum.FinalSave;
				break;
			case MySpawningCycleMicroEnum.FinalSave:
				SaveAs(Path.Combine(MyFileSystem.SavesPath, "..", "TestingToolSave"));
				m_stateMicro = MySpawningCycleMicroEnum.Final;
				break;
			case MySpawningCycleMicroEnum.Final:
			{
				if (!ConsumeSavingCompletion())
				{
					break;
				}
				CopyScreenshots(m_blockTestGenerationState.ResultTestCaseName, m_blockTestGenerationState.TestStart, isAddCase: true);
				CopyLastGamelog(m_blockTestGenerationState.ResultTestCaseName, "result.log");
				CopyLastSave(m_blockTestGenerationState.ResultTestCaseName, "result");
				MyInputRecording myInputRecording = new MyInputRecording
				{
					Name = Path.Combine(TestCasesDir, m_blockTestGenerationState.ResultTestCaseName),
					Description = m_blockTestGenerationState.SourceSaveWithBlockPath,
					Session = MyInputRecordingSession.Specific
				};
				myInputRecording.SetStartingScreenDimensions(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
				foreach (MySnapshot snapshot in m_blockTestGenerationState.Snapshots)
				{
					myInputRecording.AddSnapshot(snapshot);
				}
				myInputRecording.Save();
				m_stateMicro = MySpawningCycleMicroEnum.ReloadAndClear;
				break;
			}
			}
		}

		public bool PrepareRenderEntity(MyCubeBlockDefinition blockD, MyBlockSnapshot blockSnapshot = null)
		{
			if (PhotoBlock == null)
			{
				MyCubeGrid.MyBlockVisuals myBlockVisuals = new MyCubeGrid.MyBlockVisuals(MyPlayer.SelectedColor.PackHSVToUint(), MyStringHash.GetOrCompute(MyPlayer.SelectedArmorSkin));
				Vector3 color = ColorExtensions.UnpackHSVFromUint(myBlockVisuals.ColorMaskHSV);
				ulong value = MyEventContext.Current.Sender.Value;
				MyStringHash skinId = MySession.Static.GetComponent<MySessionComponentGameInventory>()?.ValidateArmor(myBlockVisuals.SkinId, value) ?? MyStringHash.NullOrEmpty;
				new MyCubeBuilder();
				MatrixD worldMatrix = MatrixD.CreateWorld(Vector3D.Zero, Vector3.Forward, Vector3.Up);
				PhotoBlock = MyCubeBuilder.SpawnStaticGrid_nonParalel(blockD, MySession.Static.LocalHumanPlayer.Character.Entity, worldMatrix, color, skinId, MyCubeBuilder.SpawnFlags.Default, 0L);
			}
			if (blockSnapshot == null)
			{
				PhotoBlock.SetBuildLevel(m_blockTestGenerationState.CurrentConstructionPhase);
				SetBlockLOD(m_blockTestGenerationState.CurrentLOD);
			}
			else
			{
				int buildLevel = 0;
				if (blockSnapshot.Stage.HasValue)
				{
					buildLevel = blockSnapshot.Stage.Value;
				}
				PhotoBlock.SetBuildLevel(buildLevel);
				if (blockSnapshot.LOD.HasValue)
				{
					SetBlockLOD(blockSnapshot.LOD.Value);
				}
			}
			return true;
		}

		public void SetCameraForTesting(Vector3D cameraPosition, MyViewsEnum view)
		{
			MySpectator.Static.Position = cameraPosition;
			MySpectator.Static.SetTarget(Vector3D.Zero, Up(view));
			MyRenderProxy.SetSettingsDirty();
		}

		public void SetBlockLOD(int LODIndex)
		{
			MySector.Lodding.CurrentSettings.Global.EnableLodSelection = true;
			MySector.Lodding.CurrentSettings.Global.LodSelection = LODIndex;
			MyRenderProxy.UpdateNewLoddingSettings(MySector.Lodding.CurrentSettings);
		}

		private void PrepareTestCase()
		{
			if (m_syncRendering)
			{
				AddSyncRenderingToCfg(m_syncRendering);
			}
			m_blockTestGenerationState.SourceSaveWithBlockPath = MySession.Static.CurrentPath;
			m_blockTestGenerationState.ResultTestCaseSavePath = Path.Combine(TestCasesDir, m_blockTestGenerationState.ResultTestCaseName, Path.GetFileName(m_blockTestGenerationState.SourceSaveWithBlockPath));
			Directory.CreateDirectory(Path.Combine(TestCasesDir, m_blockTestGenerationState.ResultTestCaseName));
			DirectoryCopy(MySession.Static.CurrentPath, m_blockTestGenerationState.ResultTestCaseSavePath, copySubDirs: true);
			File.Copy(ConfigFile, Path.Combine(TestCasesDir, m_blockTestGenerationState.ResultTestCaseName, "SpaceEngineers.cfg"), true);
		}

		private MySnapshot CreateSnapshot(int timestampIncrease = 400)
		{
			MySnapshot mySnapshot = new MySnapshot();
			mySnapshot.MouseSnapshot = new MyMouseSnapshot();
			mySnapshot.JoystickSnapshot = new MyJoystickStateSnapshot
			{
				AccelerationSliders = new int[2],
				ForceSliders = new int[2],
				PointOfViewControllers = new int[2],
				Sliders = new int[2],
				VelocitySliders = new int[2]
			};
			mySnapshot.TimerFrames = m_timer_Max;
			mySnapshot.TimerRepetitions = timerRepetitions;
			mySnapshot.KeyboardSnapshot = new List<byte>();
			mySnapshot.MouseCursorPosition = default(Vector2);
			mySnapshot.KeyboardSnapshotText = new List<char>();
			mySnapshot.SnapshotTimestamp = m_blockTestGenerationState.LastTimeStamp + timestampIncrease;
			mySnapshot.BlockSnapshot = new MyBlockSnapshot();
			mySnapshot.CameraSnapshot = new MyCameraSnapshot();
			MySnapshot mySnapshot2 = mySnapshot;
			m_blockTestGenerationState.LastTimeStamp = mySnapshot2.SnapshotTimestamp;
			return mySnapshot2;
		}

		private void CreateCompleteSnapshot(bool TakeScreenShot = false)
		{
			MyBlockSnapshot myBlockSnapshot = new MyBlockSnapshot();
			if (PhotoBlock.BlockDefinition != null)
			{
				myBlockSnapshot.CurrentBlockName = PhotoBlock.BlockDefinition.BlockPairName;
			}
			if (m_blockTestGenerationState != null)
			{
				myBlockSnapshot.Grid = m_blockTestGenerationState.BlockGrid;
				myBlockSnapshot.LOD = m_blockTestGenerationState.CurrentLOD;
				myBlockSnapshot.Stage = m_blockTestGenerationState.CurrentConstructionPhase;
				myBlockSnapshot.CurrentBlockName = PhotoBlock.BlockDefinition.BlockPairName;
			}
			MyPositionAndOrientation myPositionAndOrientation = default(MyPositionAndOrientation);
			myPositionAndOrientation.Position = MySpectator.Static.Position;
			myPositionAndOrientation.Orientation = Quaternion.CreateFromForwardUp(MySpectator.Static.Target, Up(m_blockTestGenerationState.CurrentView));
			MyPositionAndOrientation value = myPositionAndOrientation;
			MyCameraSnapshot cameraSnapshot = new MyCameraSnapshot
			{
				CameraPosition = value,
				TakeScreenShot = TakeScreenShot,
				View = m_blockTestGenerationState.CurrentView
			};
			AddSnapshot(myBlockSnapshot, cameraSnapshot);
		}

		private void AddSnapshot(MyBlockSnapshot blockSnapshot = null, MyCameraSnapshot cameraSnapshot = null, int timestamp = 400)
		{
			if (blockSnapshot == null)
			{
				blockSnapshot = new MyBlockSnapshot();
			}
			if (cameraSnapshot == null)
			{
				cameraSnapshot = new MyCameraSnapshot();
			}
			MySnapshot mySnapshot = CreateSnapshot(timestamp);
			mySnapshot.BlockSnapshot = blockSnapshot;
			mySnapshot.CameraSnapshot = cameraSnapshot;
			m_blockTestGenerationState.Snapshots.Add(mySnapshot);
		}

		public void Action1_State3_Finish()
		{
			m_blockTestGenerationState = null;
			StateOuter = MyTestingToolHelperStateOuterEnum.Idle;
		}

		private void ClearTimer()
		{
			m_timer = 0;
		}

		private bool SaveAs(string name)
		{
			if (m_isSaving)
			{
				return false;
			}
			m_isSaving = true;
			MyAsyncSaving.Start(delegate
			{
				OnSaveAsComplete();
			}, name);
			return true;
		}

		public void OnSaveAsComplete()
		{
			m_savingFinished = true;
			m_isSaving = false;
		}

		private bool FakeSaveCompletion()
		{
			if (m_isSaving)
			{
				return false;
			}
			m_savingFinished = true;
			return true;
		}

		private bool ConsumeSavingCompletion()
		{
			if (m_isSaving || !m_savingFinished)
			{
				return false;
			}
			m_savingFinished = false;
			return true;
		}

		private bool Load(string path)
		{
			if (m_isLoading)
			{
				return false;
			}
			m_isLoading = true;
			MySessionLoader.LoadSingleplayerSession(path, delegate
			{
				OnLoadComplete();
			});
			return true;
		}

		public void OnLoadComplete()
		{
			m_loadingFinished = true;
			m_isLoading = false;
		}

		private bool FakeLoadCompletion()
		{
			if (m_isLoading)
			{
				return false;
			}
			m_loadingFinished = true;
			return true;
		}

		private bool ConsumeLoadingCompletion()
		{
			if (m_isLoading || !m_loadingFinished)
			{
				return false;
			}
			m_loadingFinished = false;
			return true;
		}

		public static void CopyScreenshots(string testFolder, DateTime startTime, bool isAddCase = false)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			FileInfo[] array = Enumerable.ToArray<FileInfo>((IEnumerable<FileInfo>)Enumerable.OrderBy<FileInfo, DateTime>((IEnumerable<FileInfo>)new DirectoryInfo(ScreenshotsDir).GetFiles(), (Func<FileInfo, DateTime>)((FileInfo file) => ((FileSystemInfo)file).get_CreationTime())));
			List<string> list = new List<string>();
			int num = 0;
			FileInfo[] array2 = array;
			foreach (FileInfo val in array2)
			{
				try
				{
					if (((FileSystemInfo)val).get_LastWriteTime() >= startTime)
					{
						list.Add(((FileSystemInfo)val).get_FullName());
						File.Copy(((FileSystemInfo)val).get_FullName(), Path.Combine(TestCasesDir, testFolder, LastTestRunResultFilename + num + ".png"), true);
						if (isAddCase)
						{
							File.Copy(((FileSystemInfo)val).get_FullName(), Path.Combine(TestCasesDir, testFolder, TestResultFilename + num + ".png"), true);
						}
						File.Delete(((FileSystemInfo)val).get_FullName());
						num++;
					}
				}
				catch
				{
				}
			}
			string text = Path.Combine(ScreenshotsDir, "TestingToolResult.png");
			if (File.Exists(text))
			{
				File.Copy(text, Path.Combine(TestCasesDir, testFolder, LastTestRunResultFilename + ".png"), true);
				File.Delete(text);
			}
		}

		public static void CopyLastGamelog(string testFolder, string resultType)
		{
			File.Copy(GameLogPath, Path.Combine(TestCasesDir, testFolder, resultType), true);
		}

		public void CopyLastSave(string testCasePath, string resultName)
		{
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			string text = Path.Combine(UserSaveFolder, "TestingToolSave");
			string path = Path.Combine(TestCasesDir, testCasePath);
			if (File.Exists(Path.Combine(text, "Sandbox.sbc")))
			{
				File.Copy(Path.Combine(text, "Sandbox.sbc"), Path.Combine(path, resultName + ".sbc"), true);
				File.Copy(Path.Combine(text, "SANDBOX_0_0_0_.sbs"), Path.Combine(path, resultName + ".sbs"), true);
			}
			if (Directory.Exists(Path.Combine(text)))
			{
				new DirectoryInfo(Path.Combine(text)).Delete(true);
			}
		}

		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			DirectoryInfo val = new DirectoryInfo(sourceDirName);
			if (!((FileSystemInfo)val).get_Exists())
			{
				throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
			}
			DirectoryInfo[] directories = val.GetDirectories();
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}
			FileInfo[] files = val.GetFiles();
			foreach (FileInfo val2 in files)
			{
				string text = Path.Combine(destDirName, ((FileSystemInfo)val2).get_Name());
				val2.CopyTo(text, true);
			}
			if (copySubDirs)
			{
				DirectoryInfo[] array = directories;
				foreach (DirectoryInfo val3 in array)
				{
					string destDirName2 = Path.Combine(destDirName, ((FileSystemInfo)val3).get_Name());
					DirectoryCopy(((FileSystemInfo)val3).get_FullName(), destDirName2, copySubDirs);
				}
			}
		}

		public static void AddSyncRenderingToCfg(bool value, string cfgPath = null)
		{
			string[] array = File.ReadAllLines(ConfigFile);
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				flag = array[i].Contains("SyncRendering");
				if (flag)
				{
					array[i + 1] = "      <Value xsi:type=\"xsd:string\">" + value + "</Value>";
					break;
				}
			}
			if (!flag)
			{
				List<string> list = Enumerable.ToList<string>((IEnumerable<string>)array);
				list.Insert(list.Count - 2, "    <item>");
				list.Insert(list.Count - 2, "      <Key xsi:type=\"xsd:string\">SyncRendering</Key>");
				list.Insert(list.Count - 2, "      <Value xsi:type=\"xsd:string\">" + value + "</Value>");
				list.Insert(list.Count - 2, "    </item>");
				array = list.ToArray();
			}
			File.Delete((cfgPath != null) ? cfgPath : ConfigFile);
			File.WriteAllLines((cfgPath != null) ? cfgPath : ConfigFile, array);
		}
	}
}
