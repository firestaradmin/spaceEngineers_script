using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EmptyKeys.UserInterface.Mvvm;
using Multiplayer;
using Sandbox;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game;
using Sandbox.Game.AI.Pathfinding.Obsolete;
using Sandbox.Game.AI.Pathfinding.RecastDetour;
using Sandbox.Game.Entities;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.AI;
using SpaceEngineers.Game.GUI;
using SpaceEngineers.Game.VoiceChat;
using VRage;
using VRage.Analytics;
using VRage.Data.Audio;
using VRage.FileSystem;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;
using World;

namespace SpaceEngineers.Game
{
	public class SpaceEngineersGame : MySandboxGame
	{
<<<<<<< HEAD
		public const int SE_VERSION = 1200025;
=======
		public const int SE_VERSION = 1198033;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private Vector2I m_initializedScreenSize;

		public SpaceEngineersGame(string[] commandlineArgs)
			: base(commandlineArgs, (IntPtr)0)
		{
			MySandboxGame.GameCustomInitialization = new MySpaceGameCustomInitialization();
			FillCredits();
		}

		private void OnRenderInitialized(Vector2I size)
		{
			OnScreenSize = (Action<Vector2I>)Delegate.Remove(OnScreenSize, new Action<Vector2I>(OnRenderInitialized));
			m_initializedScreenSize = size;
			MySandboxGame.m_windowCreatedEvent.Set();
		}

		protected override void InitializeRender(IntPtr windowHandle)
		{
			OnScreenSize = (Action<Vector2I>)Delegate.Combine(OnScreenSize, new Action<Vector2I>(OnRenderInitialized));
			base.InitializeRender(windowHandle);
			StartIntroVideo();
		}

		private void StartIntroVideo()
		{
			if (MyPlatformGameSettings.ENABLE_LOGOS && MyPlatformGameSettings.ENABLE_LOGOS_ASAP)
			{
				ProcessInvoke();
				MyRenderProxy.Settings.RenderThreadHighPriority = true;
				MyRenderProxy.SwitchRenderSettings(MyRenderProxy.Settings);
				string videoFile = Path.Combine(MyFileSystem.ContentPath, "Videos\\KSH.wmv");
				base.IntroVideoId = MyRenderProxy.PlayVideo(videoFile, 1f);
				MyRenderProxy.UpdateVideo(base.IntroVideoId);
				MyRenderProxy.DrawVideo(base.IntroVideoId, new Rectangle(0, 0, m_initializedScreenSize.X, m_initializedScreenSize.Y), Color.White, MyVideoRectangleFitMode.AutoFit, ignoreBounds: true);
				MyScreenManager.AddScreen(MyGuiScreenInitialLoading.Instance);
				MyRenderProxy.AfterUpdate(null);
				MyRenderProxy.BeforeUpdate();
				MyVRage.Platform.Windows.Window.ShowAndFocus();
			}
		}

		public static void SetupBasicGameInfo()
		{
<<<<<<< HEAD
			MyPerGameSettings.BasicGameInfo.GameVersion = 1200025;
=======
			MyPerGameSettings.BasicGameInfo.GameVersion = 1198033;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyPerGameSettings.BasicGameInfo.GameName = "Space Engineers";
			MyPerGameSettings.BasicGameInfo.GameNameSafe = "SpaceEngineers";
			MyPerGameSettings.BasicGameInfo.ApplicationName = "SpaceEngineers";
			MyPerGameSettings.BasicGameInfo.GameAcronym = "SE";
			MyPerGameSettings.BasicGameInfo.MinimumRequirementsWeb = "http://www.spaceengineersgame.com";
			MyPerGameSettings.BasicGameInfo.SplashScreenImage = "..\\Content\\Textures\\Logo\\splashscreen.png";
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyPerGameSettings.BasicGameInfo.AnalyticId = MyPerGameSettings.BasicGameInfo.GameAcronym + "DS";
			}
			else
			{
				MyPerGameSettings.BasicGameInfo.AnalyticId = MyPerGameSettings.BasicGameInfo.GameAcronym;
			}
		}

		public static void SetupPerGameSettings()
		{
			MyPerGameSettings.Game = GameEnum.SE_GAME;
			MyPerGameSettings.GameIcon = "SpaceEngineers.ico";
			MyPerGameSettings.EnableGlobalGravity = false;
			MyPerGameSettings.GameModAssembly = "SpaceEngineers.Game.dll";
			MyPerGameSettings.GameModObjBuildersAssembly = "SpaceEngineers.ObjectBuilders.dll";
			MyPerGameSettings.OffsetVoxelMapByHalfVoxel = true;
			MyPerGameSettings.EnablePregeneratedAsteroidHack = true;
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MySandboxGame.ConfigDedicated = new MyConfigDedicated<MyObjectBuilder_SessionSettings>("SpaceEngineers-Dedicated.cfg");
			}
			MySandboxGame.GameCustomInitialization = new MySpaceGameCustomInitialization();
			MyPerGameSettings.ShowObfuscationStatus = false;
			MyPerGameSettings.UseNewDamageEffects = true;
			MyPerGameSettings.EnableResearch = true;
			MyPerGameSettings.UseVolumeLimiter = MyFakes.ENABLE_NEW_SOUNDS && MyFakes.ENABLE_REALISTIC_LIMITER;
			MyPerGameSettings.UseSameSoundLimiter = true;
			MyPerGameSettings.UseMusicController = true;
			MyPerGameSettings.UseReverbEffect = true;
			MyPerGameSettings.Destruction = false;
			MyMusicTrack value = default(MyMusicTrack);
			value.TransitionCategory = MyStringId.GetOrCompute("NoRandom");
			value.MusicCategory = MyStringId.GetOrCompute("MusicMenu");
			MyPerGameSettings.MainMenuTrack = value;
			MyPerGameSettings.BallFriendlyPhysics = false;
			if (MyFakes.ENABLE_CESTMIR_PATHFINDING)
			{
				MyPerGameSettings.PathfindingType = typeof(MyPathfinding);
			}
			else
			{
				MyPerGameSettings.PathfindingType = typeof(MyRDPathfinding);
			}
			MyPerGameSettings.BotFactoryType = typeof(MySpaceBotFactory);
			MyPerGameSettings.ControlMenuInitializerType = typeof(MySpaceControlMenuInitializer);
			MyPerGameSettings.EnableScenarios = true;
			MyPerGameSettings.EnableJumpDrive = true;
			MyPerGameSettings.EnableShipSoundSystem = true;
			MyFakes.ENABLE_PLANETS_JETPACK_LIMIT_IN_CREATIVE = true;
			MyFakes.ENABLE_DRIVING_PARTICLES = true;
			MyPerGameSettings.EnablePathfinding = false;
			MyPerGameSettings.CharacterGravityMultiplier = 2f;
			MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_AXIS_HELPERS = true;
			MyPerGameSettings.EnableRagdollInJetpack = true;
			MyPerGameSettings.GUI.OptionsScreen = typeof(MyGuiScreenOptionsSpace);
			MyPerGameSettings.GUI.PerformanceWarningScreen = typeof(MyGuiScreenPerformanceWarnings);
			MyPerGameSettings.GUI.CreateFactionScreen = typeof(MyGuiScreenCreateOrEditFactionSpace);
			MyPerGameSettings.GUI.MainMenu = typeof(MyGuiScreenMainMenu);
			MyPerGameSettings.DefaultGraphicsRenderer = MySandboxGame.DirectX11RendererKey;
			MyPerGameSettings.EnableWelderAutoswitch = true;
			MyPerGameSettings.CompatHelperType = typeof(MySpaceSessionCompatHelper);
			MyPerGameSettings.GUI.MainMenuBackgroundVideos = new string[10] { "Videos\\Background01_720p.wmv", "Videos\\Background02_720p.wmv", "Videos\\Background03_720p.wmv", "Videos\\Background04_720p.wmv", "Videos\\Background05_720p.wmv", "Videos\\Background09_720p.wmv", "Videos\\Background10_720p.wmv", "Videos\\Background11_720p.wmv", "Videos\\Background12_720p.wmv", "Videos\\Background13_720p.wmv" };
			MyPerGameSettings.VoiceChatEnabled = true;
			MyPerGameSettings.VoiceChatLogic = typeof(MyVoiceChatLogic);
			MyPerGameSettings.ClientStateType = typeof(MySpaceClientState);
			MyVoxelPhysicsBody.UseLod1VoxelPhysics = false;
			MyPerGameSettings.EnableAi = true;
			MyPerGameSettings.EnablePathfinding = true;
			MyPerGameSettings.UpdateOrchestratorType = typeof(MyParallelEntityUpdateOrchestrator);
			MyFakesLocal.SetupLocalPerGameSettings();
		}

		private static void FillCredits()
		{
			MyCreditsDepartment myCreditsDepartment = new MyCreditsDepartment("{LOCG:Department_ExecutiveProducer}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment);
			myCreditsDepartment.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment.Persons.Add(new MyCreditsPerson("MAREK ROSA"));
			MyCreditsDepartment myCreditsDepartment2 = new MyCreditsDepartment("{LOCG:Department_LeadProducer}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment2);
			myCreditsDepartment2.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment2.Persons.Add(new MyCreditsPerson("PETR MINARIK"));
			MyCreditsDepartment myCreditsDepartment3 = new MyCreditsDepartment("{LOCG:Department_TeamOperations}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment3);
			myCreditsDepartment3.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment3.Persons.Add(new MyCreditsPerson("VLADISLAV POLGAR"));
			MyCreditsDepartment myCreditsDepartment4 = new MyCreditsDepartment("{LOCG:Department_LiveOpsProducer}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment4);
			myCreditsDepartment4.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment4.Persons.Add(new MyCreditsPerson("LUCIE HORNOFOVA"));
			MyCreditsDepartment myCreditsDepartment5 = new MyCreditsDepartment("{LOCG:Department_TechnicalDirector}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment5);
			myCreditsDepartment5.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment5.Persons.Add(new MyCreditsPerson("JAN \"CENDA\" HLOUSEK"));
			MyCreditsDepartment myCreditsDepartment6 = new MyCreditsDepartment("{LOCG:Department_LiveOpsTechLead}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment6);
			myCreditsDepartment6.Persons = new List<MyCreditsPerson>();
<<<<<<< HEAD
			myCreditsDepartment6.Persons.Add(new MyCreditsPerson("FILIP DUSEK"));
			MyCreditsDepartment myCreditsDepartment7 = new MyCreditsDepartment("{LOCG:Department_LeadProgrammers}");
=======
			myCreditsDepartment6.Persons.Add(new MyCreditsPerson("PETR BERANEK"));
			myCreditsDepartment6.Persons.Add(new MyCreditsPerson("MARTIN PAVLICEK"));
			myCreditsDepartment6.Persons.Add(new MyCreditsPerson("DANIEL ILHA"));
			myCreditsDepartment6.Persons.Add(new MyCreditsPerson("MIRO FARKAS"));
			myCreditsDepartment6.Persons.Add(new MyCreditsPerson("GRZEGORZ ZADROGA"));
			myCreditsDepartment6.Persons.Add(new MyCreditsPerson("SANDRA LENARDOVA"));
			MyCreditsDepartment myCreditsDepartment7 = new MyCreditsDepartment("{LOCG:Department_LeadDesigner}");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment7);
			myCreditsDepartment7.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment7.Persons.Add(new MyCreditsPerson("FILIP DUSEK"));
			myCreditsDepartment7.Persons.Add(new MyCreditsPerson("JAN \"CENDA\" HLOUSEK"));
			myCreditsDepartment7.Persons.Add(new MyCreditsPerson("PETR MINARIK"));
			MyCreditsDepartment myCreditsDepartment8 = new MyCreditsDepartment("{LOCG:Department_Programmers}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment8);
			myCreditsDepartment8.Persons = new List<MyCreditsPerson>();
<<<<<<< HEAD
			myCreditsDepartment8.Persons.Add(new MyCreditsPerson("PETR BERANEK"));
			myCreditsDepartment8.Persons.Add(new MyCreditsPerson("DANIEL ILHA"));
			myCreditsDepartment8.Persons.Add(new MyCreditsPerson("JIRI MAREK"));
			myCreditsDepartment8.Persons.Add(new MyCreditsPerson("MARTIN PAVLICEK"));
			myCreditsDepartment8.Persons.Add(new MyCreditsPerson("JAKUB SADIL"));
			myCreditsDepartment8.Persons.Add(new MyCreditsPerson("GREGORY SMIRNOV"));
			MyCreditsDepartment myCreditsDepartment9 = new MyCreditsDepartment("{LOCG:Department_LiveOpsProgrammers}");
=======
			myCreditsDepartment8.Persons.Add(new MyCreditsPerson("ALES KOZAK"));
			MyCreditsDepartment myCreditsDepartment9 = new MyCreditsDepartment("{LOCG:Department_LeadArtist}");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment9);
			myCreditsDepartment9.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment9.Persons.Add(new MyCreditsPerson("JESSE SKY KING"));
			myCreditsDepartment9.Persons.Add(new MyCreditsPerson("JIRI MAREK"));
			myCreditsDepartment9.Persons.Add(new MyCreditsPerson("MATE MESZAROS"));
			myCreditsDepartment9.Persons.Add(new MyCreditsPerson("JAKUB SADIL"));
			myCreditsDepartment9.Persons.Add(new MyCreditsPerson("GREGORY SMIRNOV"));
			MyCreditsDepartment myCreditsDepartment10 = new MyCreditsDepartment("{LOCG:Department_LeadDesigner}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment10);
			myCreditsDepartment10.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment10.Persons.Add(new MyCreditsPerson("JOACHIM KOOLHOF"));
			MyCreditsDepartment myCreditsDepartment11 = new MyCreditsDepartment("{LOCG:Department_Designers}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment11);
			myCreditsDepartment11.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment11.Persons.Add(new MyCreditsPerson("ALES KOZAK"));
			MyCreditsDepartment myCreditsDepartment12 = new MyCreditsDepartment("{LOCG:Department_LiveOpsDesigners}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment12);
			myCreditsDepartment12.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment12.Persons.Add(new MyCreditsPerson("SANDER DE VISSER"));
			myCreditsDepartment12.Persons.Add(new MyCreditsPerson("MIKKO SAARIJARVI"));
			MyCreditsDepartment myCreditsDepartment13 = new MyCreditsDepartment("{LOCG:Department_LeadArtist}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment13);
			myCreditsDepartment13.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment13.Persons.Add(new MyCreditsPerson("NATIQ AGHAYEV"));
			MyCreditsDepartment myCreditsDepartment14 = new MyCreditsDepartment("{LOCG:Department_Artists}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment14);
			myCreditsDepartment14.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment14.Persons.Add(new MyCreditsPerson("JAN GOLMIC"));
			myCreditsDepartment14.Persons.Add(new MyCreditsPerson("JAN TRAUSKE"));
			myCreditsDepartment14.Persons.Add(new MyCreditsPerson("VLADIMIR VDOVIN"));
			MyCreditsDepartment myCreditsDepartment15 = new MyCreditsDepartment("{LOCG:Department_SoundDesign}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment15);
			myCreditsDepartment15.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment15.Persons.Add(new MyCreditsPerson("LUKAS TVRDON"));
			MyCreditsDepartment myCreditsDepartment16 = new MyCreditsDepartment("{LOCG:Department_Music}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment16);
			myCreditsDepartment16.Persons = new List<MyCreditsPerson>();
<<<<<<< HEAD
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("KAREL ANTONIN"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("ANNA KALHAUSOVA (cello)"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("MARIE SVOBODOVA (vocals)"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Colossus On Fire - Dave Rodriguez"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Designing The Fleet - Limnull"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Entropy - Epicarmina"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("EXODUS - Gordon Saverimuthu"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Growing Wild - Timo Letsch"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Into the Asteroid Field - Lynxi Ft Umbria"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Last Day on Triton - Aaron Schxfer"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Sands of the Slave Princess - Windflower Falls"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Sector 347 - Jan Altherr"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Space Angels - Musicfonts"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Space Elevator - Gordon Saverimuthu"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Space Engineers Main Theme - KhydroDjent"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Space Pirates - Bart Zeal Ruben Isarin"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("SpaceItOut - SWAUSAGE"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("The City Lies - Exelan"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("Timeless Space - Marcin Klysewicz"));
			MyCreditsDepartment myCreditsDepartment17 = new MyCreditsDepartment("{LOCG:Department_Video}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment17);
			myCreditsDepartment17.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("JOEL \"XOCLIW\" WILCOX"));
			MyCreditsDepartment myCreditsDepartment18 = new MyCreditsDepartment("{LOCG:Department_LeadTester}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment18);
			myCreditsDepartment18.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment18.Persons.Add(new MyCreditsPerson("ONDREJ NAHALKA"));
			MyCreditsDepartment myCreditsDepartment19 = new MyCreditsDepartment("{LOCG:Department_Testers}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment19);
			myCreditsDepartment19.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("ALES KOZAK"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("ARTUR KUSHUKOV"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("JAN HRIVNAC"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("JAN PETRZILKA"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("KATERINA CERVENA"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("LAURA KNIGHT"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("ONDREJ BORZ"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("VOJTECH NEORAL"));
			MyCreditsDepartment myCreditsDepartment20 = new MyCreditsDepartment("{LOCG:Department_CommunityPr}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment20);
			myCreditsDepartment20.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("ERIN TRUITT"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("JOEL \"XOCLIW\" WILCOX"));
			MyCreditsDepartment myCreditsDepartment21 = new MyCreditsDepartment("Frostbite scenario");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment21);
			myCreditsDepartment21.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Petr Minarik"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Jan Vanecek"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Joachim Koolhof"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Mikko Saari"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Timothy Gatton"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Pepijn van Duijn"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Jesse Baule"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Dusan Repik"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Joel Wilcox"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Natiq Aghayev"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Jan Trauske"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Jan Golmic"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Kristiaan Renaerts"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Pavel Konfrst"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Lukas Tvrdon"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Satoko Yamaoko"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Nicole Draper"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Victor Hugo Monaco"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Chris Bayne a.k.a DirectedEnergy"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Lela Kovalenko a.k.a.Naburine"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Nathan \"Silverbane\" Steen"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Skyler \"Gorhamian\" Gorham"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Jacob \"wearsglasses\" Ruttenberg"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Yang Yafang"));
			MyCreditsDepartment myCreditsDepartment22 = new MyCreditsDepartment("{LOCG:Department_Office}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment22);
			myCreditsDepartment22.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("MARIANNA HIRCAKOVA"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("PETR KREJCI"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("LUCIE KRESTOVA"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("VACLAV NOVOTNY"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("TOMAS STROUHAL"));
			MyCreditsDepartment myCreditsDepartment23 = new MyCreditsDepartment("{LOCG:Department_CommunityManagers}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment23);
			myCreditsDepartment23.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment23.Persons.Add(new MyCreditsPerson("Dr Vagax"));
			myCreditsDepartment23.Persons.Add(new MyCreditsPerson("Conrad Larson"));
			myCreditsDepartment23.Persons.Add(new MyCreditsPerson("Dan2D3D"));
			myCreditsDepartment23.Persons.Add(new MyCreditsPerson("RayvenQ"));
			myCreditsDepartment23.Persons.Add(new MyCreditsPerson("Redphoenix"));
			myCreditsDepartment23.Persons.Add(new MyCreditsPerson("TodesRitter"));
			MyCreditsDepartment myCreditsDepartment24 = new MyCreditsDepartment("{LOCG:Department_ModContributors}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment24);
			myCreditsDepartment24.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Tyrsis"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Daniel \"Gwindalmir\" Osborne"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Morten \"Malware\" Aune Lyrstad"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Arindel"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Darth Biomech"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Night Lone"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Mexmer"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("JD.Horx"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("John \"Jimmacle\" Gross"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Logan Tyran"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Joshua \"Whiplash141\" Banks"));
			MyCreditsDepartment myCreditsDepartment25 = new MyCreditsDepartment("{LOCG:Department_Translators}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment25);
			myCreditsDepartment25.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Damian \"Truzaku\" Komarek"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Julian Tomaszewski"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("George Grivas"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Олег \"AaLeSsHhKka\" Цюпка"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Maxim \"Ma)(imuM\" Lyashuk"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Axazel"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Baly94"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Dyret"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("gon.gged"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Huberto"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("HunterNephilim"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("nintendo22"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Quellix"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("raviool"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Dr. Bell"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Dominik Frydl"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Daniel Hloušek"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Andre Camara Marchi"));
			myCreditsDepartment25.Persons.Add(new MyCreditsPerson("Ociotek Traducciones"));
			myCreditsDepartment25.LogoTexture = "Textures\\Logo\\TranslatorsCN.dds";
			myCreditsDepartment25.LogoScale = 0.85f;
			myCreditsDepartment25.LogoTextureSize = MyRenderProxy.GetTextureSize(myCreditsDepartment25.LogoTexture);
			myCreditsDepartment25.LogoOffsetPost = 0.11f;
			MyCreditsDepartment myCreditsDepartment26 = new MyCreditsDepartment("{LOCG:Department_SpecialThanks}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment26);
			myCreditsDepartment26.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("ABDULAZIZ ALDIGS"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("DUSAN ANDRAS"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("ONDREJ ANGELOVIC"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("IVAN BARAN"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("ANTON \"TOTAL\" BAUER"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("ALES BRICH"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("JOAO CARIAS"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("THEO ESCAMEZ"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("ALEX FLOREA"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("JAN GOLMIC"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("CESTMIR HOUSKA"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("JAKUB HRNCIR"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("LUKAS CHRAPEK"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("LUKAS JANDIK"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("MARKETA JAROSOVA"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("MARTIN KOCISEK"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("JOELLEN KOESTER"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("GREGORY KONTADAKIS"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("MARKO KORHONEN"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("TOMAS KOSEK"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("RADOVAN KOTRLA"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("MARTIN KROSLAK"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("MICHAL KUCIS"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("DANIEL LEIMBACH"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("RADKA LISA"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("PERCY LIU"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("GEORGE MAMAKOS"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("BRANT MARTIN"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("JAN NEKVAPIL"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("MAREK OBRSAL"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("PAVEL OCOVAJ"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("PREMYSL PASKA"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("ONDREJ PETRZILKA"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("FRANCESKO PRETTO"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("TOMAS PSENICKA"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("DOMINIK RAGANCIK"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("TOMAS RAMPAS"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("DUSAN REPIK"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("VILEM SOULAK"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("RASTKO STANOJEVIC"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("SLOBODAN STEVIC"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("TIM TOXOPEUS"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("JAN VEBERSIK"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("LUKAS VILIM"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("MATEJ VLK"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("ADAM WILLIAMS"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("CHARLES WINTERS"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("MICHAL WROBEL"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("MICHAL ZAK"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("MICHAL ZAVADAK"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("MIRO FARKAS"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("GRZEGORZ ZADROGA"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("SANDRA LENARDOVA"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("JESSE BAULE"));
			myCreditsDepartment26.Persons.Add(new MyCreditsPerson("KRISTIAAN RENAERTS"));
			MyCreditsDepartment myCreditsDepartment27 = new MyCreditsDepartment("{LOCG:Department_MoreInfo}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment27);
			myCreditsDepartment27.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment27.Persons.Add(new MyCreditsPerson("{LOCG:Person_Web}"));
			myCreditsDepartment27.Persons.Add(new MyCreditsPerson("{LOCG:Person_FB}"));
			myCreditsDepartment27.Persons.Add(new MyCreditsPerson("{LOCG:Person_Twitter}"));
			MyCreditsDepartment myCreditsDepartment28 = new MyCreditsDepartment("{LOCG:Department_Licenses}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment28);
			myCreditsDepartment28.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("C# / XNA  port of Bullet - Copyright (c) 2011 Mark Neale <xexuxjy@hotmail.com>"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Bullet Physics Library - Copyright (c) 2003-2008 Erwin Coumans http://www.bulletphysics.com/"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Empty Keys UI - Copyright (c) 2020 Empty Keys"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("mod.io - Copyright (c) 2020 mod.io"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("FXAA - Copyright (c) 2010 NVIDIA Corporation. All rights reserved."));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("HBAO+ - Copyright (c) 2010 NVIDIA Corporation. All rights reserved."));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("StringBuilderExt - Copyright (c) Gavin Pugh 2010 - Released under the zlib license"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("SharpDX - Copyright (c) 2010-2012 SharpDX - Alexandre Mutel"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Parallel Tasks - Microsoft Public License (Ms-PL), http://paralleltasks.codeplex.com/"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Perlin's Improved Noise - http://mrl.nyu.edu/~perlin/noise/"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Trace Tool - Common Public License Version 1.0 (CPL)"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("GImpact - Copyright  (c) 2006 , Francisco León."));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("RestSharp - Copyright © 2009-2020 John Sheehan, Andrew Young, Alexey Zimarev and RestSharp community"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("LitJson - Unlicensed"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Fody - Copyright (c) Simon Cropp"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("ProtoBuf - Copyright 2008 Google Inc.  All rights reserved."));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("ProtoBuf.Net - Copyright 2008 Marc Gravell"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("ImageSharp - Copyright (c) Six Labors"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Opus codec - Copyright (c) 2010-2011 Xiph.Org Foundation, Skype Limited; Written by Jean-Marc Valin and Koen Vos"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("RecastDetour - Copyright (c) 2009-2010 Mikko Mononen memon@inside.org"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Open Asset Import Library - Copyright (c) 2006-2010, Assimp Development Team"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Steamworks - Copyright (c) 1996-2014, Valve Corporation, All rights reserved."));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Steamworks.NET - Copyright (c) 2013-2019 Riley Labrecque"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Epic Online Services - Copyright Epic Games, Inc. All Rights Reserved."));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Xamarin - Copyright 2011-2016 Xamarin, Copyright 2016-2019 Microsoft Inc"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Havok - (C) Copyright 1999-2014 Telekinesys Research Limited t/a Havok. All Rights Reserved."));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Telerik - Copyright © 2020, Progress Software Corporation and/or its subsidiaries or affiliates. All Rights Reserved."));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("DirectShow Net Library - Copyright (C) 2007 directshownet"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("GameAnalytics - Copyright 2020 GameAnalytics"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("NLog - Copyright (c) 2004-2020 Jaroslaw Kowalski <jaak@jkowalski.net>, Kim Christensen, Julian Verdurmen"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("XamlBehaviors - Copyright (c) 2015 Microsoft"));
			myCreditsDepartment28.Persons.Add(new MyCreditsPerson("Newtonsoft.Json - Copyright (c) 2007 James Newton-King"));
=======
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("JESSE BAULE"));
			myCreditsDepartment16.Persons.Add(new MyCreditsPerson("JOEL \"XOCLIW\" WILCOX"));
			MyCreditsDepartment myCreditsDepartment17 = new MyCreditsDepartment("Frostbite scenario");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment17);
			myCreditsDepartment17.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Petr Minarik"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Jan Vanecek"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Joachim Koolhof"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Mikko Saari"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Timothy Gatton"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Pepijn van Duijn"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Jesse Baule"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Dusan Repik"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Joel Wilcox"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Natiq Aghayev"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Jan Trauske"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Jan Golmic"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Kristiaan Renaerts"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Pavel Konfrst"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Lukas Tvrdon"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Satoko Yamaoko"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Nicole Draper"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Victor Hugo Monaco"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Chris Bayne a.k.a DirectedEnergy"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Lela Kovalenko a.k.a.Naburine"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Nathan \"Silverbane\" Steen"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Skyler \"Gorhamian\" Gorham"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Jacob \"wearsglasses\" Ruttenberg"));
			myCreditsDepartment17.Persons.Add(new MyCreditsPerson("Yang Yafang"));
			MyCreditsDepartment myCreditsDepartment18 = new MyCreditsDepartment("{LOCG:Department_Office}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment18);
			myCreditsDepartment18.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment18.Persons.Add(new MyCreditsPerson("MARIANNA HIRCAKOVA"));
			myCreditsDepartment18.Persons.Add(new MyCreditsPerson("PETR KREJCI"));
			myCreditsDepartment18.Persons.Add(new MyCreditsPerson("LUCIE KRESTOVA"));
			myCreditsDepartment18.Persons.Add(new MyCreditsPerson("VACLAV NOVOTNY"));
			myCreditsDepartment18.Persons.Add(new MyCreditsPerson("TOMAS STROUHAL"));
			MyCreditsDepartment myCreditsDepartment19 = new MyCreditsDepartment("{LOCG:Department_CommunityManagers}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment19);
			myCreditsDepartment19.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("Dr Vagax"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("Conrad Larson"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("Dan2D3D"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("RayvenQ"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("Redphoenix"));
			myCreditsDepartment19.Persons.Add(new MyCreditsPerson("TodesRitter"));
			MyCreditsDepartment myCreditsDepartment20 = new MyCreditsDepartment("{LOCG:Department_ModContributors}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment20);
			myCreditsDepartment20.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Tyrsis"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Daniel \"Phoenix84\" Osborne"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Morten \"Malware\" Aune Lyrstad"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Arindel"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Darth Biomech"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Night Lone"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Mexmer"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("JD.Horx"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("John \"Jimmacle\" Gross"));
			myCreditsDepartment20.Persons.Add(new MyCreditsPerson("Logan Tyran"));
			MyCreditsDepartment myCreditsDepartment21 = new MyCreditsDepartment("{LOCG:Department_Translators}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment21);
			myCreditsDepartment21.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Damian \"Truzaku\" Komarek"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Julian Tomaszewski"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("George Grivas"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Олег \"AaLeSsHhKka\" Цюпка"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Maxim \"Ma)(imuM\" Lyashuk"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Axazel"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Baly94"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Dyret"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("gon.gged"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Huberto"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("HunterNephilim"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("nintendo22"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Quellix"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("raviool"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Dr. Bell"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Dominik Frydl"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Daniel Hloušek"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Andre Camara Marchi"));
			myCreditsDepartment21.Persons.Add(new MyCreditsPerson("Ociotek Traducciones"));
			myCreditsDepartment21.LogoTexture = "Textures\\Logo\\TranslatorsCN.dds";
			myCreditsDepartment21.LogoScale = 0.85f;
			myCreditsDepartment21.LogoTextureSize = MyRenderProxy.GetTextureSize(myCreditsDepartment21.LogoTexture);
			myCreditsDepartment21.LogoOffsetPost = 0.11f;
			MyCreditsDepartment myCreditsDepartment22 = new MyCreditsDepartment("{LOCG:Department_SpecialThanks}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment22);
			myCreditsDepartment22.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("ABDULAZIZ ALDIGS"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("DUSAN ANDRAS"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("ONDREJ ANGELOVIC"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("IVAN BARAN"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("ANTON \"TOTAL\" BAUER"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("ALES BRICH"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("JOAO CARIAS"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("THEO ESCAMEZ"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("ALEX FLOREA"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("JAN GOLMIC"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("CESTMIR HOUSKA"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("JAKUB HRNCIR"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("LUKAS CHRAPEK"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("LUKAS JANDIK"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("MARKETA JAROSOVA"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("MARTIN KOCISEK"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("JOELLEN KOESTER"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("GREGORY KONTADAKIS"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("MARKO KORHONEN"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("TOMAS KOSEK"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("RADOVAN KOTRLA"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("MARTIN KROSLAK"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("MICHAL KUCIS"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("DANIEL LEIMBACH"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("RADKA LISA"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("PERCY LIU"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("GEORGE MAMAKOS"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("BRANT MARTIN"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("JAN NEKVAPIL"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("MAREK OBRSAL"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("PAVEL OCOVAJ"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("PREMYSL PASKA"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("ONDREJ PETRZILKA"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("FRANCESKO PRETTO"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("TOMAS PSENICKA"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("DOMINIK RAGANCIK"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("TOMAS RAMPAS"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("DUSAN REPIK"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("VILEM SOULAK"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("RASTKO STANOJEVIC"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("SLOBODAN STEVIC"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("TIM TOXOPEUS"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("JAN VEBERSIK"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("LUKAS VILIM"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("MATEJ VLK"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("ADAM WILLIAMS"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("CHARLES WINTERS"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("MICHAL WROBEL"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("MICHAL ZAK"));
			myCreditsDepartment22.Persons.Add(new MyCreditsPerson("MICHAL ZAVADAK"));
			MyCreditsDepartment myCreditsDepartment23 = new MyCreditsDepartment("{LOCG:Department_MoreInfo}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment23);
			myCreditsDepartment23.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment23.Persons.Add(new MyCreditsPerson("{LOCG:Person_Web}"));
			myCreditsDepartment23.Persons.Add(new MyCreditsPerson("{LOCG:Person_FB}"));
			myCreditsDepartment23.Persons.Add(new MyCreditsPerson("{LOCG:Person_Twitter}"));
			MyCreditsDepartment myCreditsDepartment24 = new MyCreditsDepartment("{LOCG:Department_Licenses}");
			MyPerGameSettings.Credits.Departments.Add(myCreditsDepartment24);
			myCreditsDepartment24.Persons = new List<MyCreditsPerson>();
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Empty Keys UI - Copyright (c) 2018 Empty Keys"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("mod.io - Copyright (c) 2020 mod.io"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("FXAA - Copyright (c) 2010 NVIDIA Corporation. All rights reserved."));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("HBAO+ - Copyright (c) 2010 NVIDIA Corporation. All rights reserved."));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("StringBuilderExt - Copyright (c) Gavin Pugh 2010 - Released under the zlib license"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("SharpDX - Copyright (c) 2010-2012 SharpDX - Alexandre Mutel"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Parallel Tasks - Microsoft Public License (Ms-PL), http://paralleltasks.codeplex.com/"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Perlin's Improved Noise - http://mrl.nyu.edu/~perlin/noise/"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Trace Tool - Common Public License Version 1.0 (CPL)"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("GImpact - Copyright  (c) 2006 , Francisco León."));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("RestSharp - Copyright © 2009-2020 John Sheehan, Andrew Young, Alexey Zimarev and RestSharp community"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("LitJson - Unlicensed"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Fody - Copyright (c) Simon Cropp"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("ProtoBuf - Copyright 2008 Google Inc.  All rights reserved."));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("ProtoBuf.Net - Copyright 2008 Marc Gravell"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("ImageSharp - Copyright (c) Six Labors"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Opus codec - Copyright (c) 2010-2011 Xiph.Org Foundation, Skype Limited; Written by Jean-Marc Valin and Koen Vos"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("RecastDetour - Copyright (c) 2009-2010 Mikko Mononen memon@inside.org"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Open Asset Import Library - Copyright (c) 2006-2010, Assimp Development Team"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Steamworks - Copyright (c) 1996-2014, Valve Corporation, All rights reserved."));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Steamworks.NET - Copyright (c) 2013-2019 Riley Labrecque"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Epic Online Services - Copyright Epic Games, Inc. All Rights Reserved."));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Xamarin - Copyright 2011-2016 Xamarin, Copyright 2016-2019 Microsoft Inc"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Havok - (C) Copyright 1999-2014 Telekinesys Research Limited t/a Havok. All Rights Reserved."));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Telerik - Copyright © 2020, Progress Software Corporation and/or its subsidiaries or affiliates. All Rights Reserved."));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("DirectShow Net Library - Copyright (C) 2007 directshownet"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("GameAnalytics - Copyright 2020 GameAnalytics"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("NLog - Copyright (c) 2004-2020 Jaroslaw Kowalski <jaak@jkowalski.net>, Kim Christensen, Julian Verdurmen"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("XamlBehaviors - Copyright (c) 2015 Microsoft"));
			myCreditsDepartment24.Persons.Add(new MyCreditsPerson("Newtonsoft.Json - Copyright (c) 2007 James Newton-King"));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyCreditsNotice myCreditsNotice = new MyCreditsNotice();
			myCreditsNotice.LogoScale = 0.8f;
			myCreditsNotice.LogoTexture = "Textures\\Logo\\vrage_logo_2_0_small.dds";
			myCreditsNotice.LogoTextureSize = MyRenderProxy.GetTextureSize(myCreditsNotice.LogoTexture);
			myCreditsNotice.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_01}"));
			myCreditsNotice.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_02}"));
			myCreditsNotice.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_03}"));
			myCreditsNotice.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_04}"));
			MyPerGameSettings.Credits.CreditNotices.Add(myCreditsNotice);
			MyCreditsNotice myCreditsNotice2 = new MyCreditsNotice();
			myCreditsNotice2.LogoTexture = "Textures\\Logo\\havok.dds";
			myCreditsNotice2.LogoScale = 0.65f;
			myCreditsNotice2.LogoTextureSize = MyRenderProxy.GetTextureSize(myCreditsNotice2.LogoTexture);
			myCreditsNotice2.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_05}"));
			myCreditsNotice2.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_06}"));
			myCreditsNotice2.CreditNoticeLines.Add(new StringBuilder("{LOCG:NoticeLine_07}"));
			MyPerGameSettings.Credits.CreditNotices.Add(myCreditsNotice2);
			SetupSecrets();
		}

		private static void SetupSecrets()
		{
			MyPerGameSettings.GA_Public_GameKey = "27bae5ba5219bcd64ddbf83113eabb30";
			MyPerGameSettings.GA_Public_SecretKey = "d04e0431f97f90fae73b9d6ea99fc9746695bd11";
			MyPerGameSettings.GA_Dev_GameKey = "3a6b6ebdc48552beba3efe173488d8ba";
			MyPerGameSettings.GA_Dev_SecretKey = "caecaaa4a91f6b2598cf8ffb931b3573f20b4343";
			MyPerGameSettings.GA_Pirate_GameKey = "41827f7c8bfed902495e0e27cb57c495";
			MyPerGameSettings.GA_Pirate_SecretKey = "493b7cb3f0a472f940c0ba0c38efbb49e902cbec";
			MyPerGameSettings.GA_Other_GameKey = "4f02769277e62b4344da70967e99a2a0";
			MyPerGameSettings.GA_Other_SecretKey = "7fa773c228ce9534181adcfebf30d18bc6807d2b";
		}

		protected override void CheckGraphicsCard(MyRenderMessageVideoAdaptersResponse msgVideoAdapters)
		{
			base.CheckGraphicsCard(msgVideoAdapters);
			MyAdapterInfo myAdapterInfo = msgVideoAdapters.Adapters[MyVideoSettingsManager.CurrentDeviceSettings.AdapterOrdinal];
			MyPerformanceSettings defaults = MyGuiScreenOptionsGraphics.GetPreset(myAdapterInfo.Quality);
			if (myAdapterInfo.VRAM < 512000000)
			{
				defaults.RenderSettings.TextureQuality = (defaults.RenderSettings.VoxelTextureQuality = MyTextureQuality.LOW);
			}
			else if (myAdapterInfo.VRAM < 2000000000 && myAdapterInfo.Quality == MyRenderPresetEnum.HIGH)
			{
				defaults.RenderSettings.TextureQuality = (defaults.RenderSettings.VoxelTextureQuality = MyTextureQuality.MEDIUM);
			}
			bool force = myAdapterInfo.Quality > MyRenderPresetEnum.CUSTOM;
			MyVideoSettingsManager.UpdateRenderSettingsFromConfig(ref defaults, force);
		}

		public static void SetupAnalytics()
		{
			MyLog.Default.WriteLine("SpaceEngineersGame.SetupAnalytics - START");
			string projectId = "27bae5ba5219bcd64ddbf83113eabb30:d04e0431f97f90fae73b9d6ea99fc9746695bd11";
<<<<<<< HEAD
			IMyAnalytics myAnalytics = MyVRage.Platform.InitAnalytics(projectId, 1200025.ToString());
=======
			IMyAnalytics myAnalytics = MyVRage.Platform.InitAnalytics(projectId, 1198033.ToString());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (myAnalytics != null)
			{
				MySpaceAnalytics.Instance.RegisterAnalyticsTracker(myAnalytics);
			}
			string text;
			string apiKeyId;
			string apiKey;
			if (MyGameService.BranchName == null || MyGameService.BranchName == "RETAIL" || MyGameService.BranchName == "default")
			{
				text = "se-events-live";
				apiKeyId = "_NLm-3MBTFi_5jOj-49_";
				apiKey = "vs5rglJCQBSKUoUoGFEpeA";
			}
			else
			{
				text = "se-events-dev";
				apiKeyId = "QGXc-3MBVmkVU1r8Ck-4";
				apiKey = "3H_nScelS8WqrGpvlP6s3Q";
			}
			string eventStoragePath = Path.Combine(MyFileSystem.TempPath, "ElasticAnalyticsEvents");
			int maxStoredEvents = 1000;
			string apiUrl = "https://9de1fbe0eed74ab49772fa5324e02c8c.eu-central-1.aws.cloud.es.io:9243/" + text + "/_doc";
			MySpaceAnalytics.Instance.RegisterAnalyticsTracker(new MyElasticAnalytics(apiUrl, apiKeyId, apiKey, eventStoragePath, maxStoredEvents));
			string text2 = "se-heartbeat";
			string apiKeyId2 = "m4DAFngBTFi_5jOjnt_q";
			string apiKey2 = "54U_nUvSSYmuXTkEYtdR3g";
			MyElasticAnalytics heartbeatTracker = new MyElasticAnalytics("https://9de1fbe0eed74ab49772fa5324e02c8c.eu-central-1.aws.cloud.es.io:9243/" + text2 + "/_doc", apiKeyId2, apiKey2, null, 0);
			MySpaceAnalytics.Instance.RegisterHeartbeatTracker(heartbeatTracker, TimeSpan.FromMinutes(5.0));
			MyVRage.Platform.Render.OnSuspending += MySpaceAnalytics.Instance.OnSuspending;
			MyVRage.Platform.Render.OnResuming += MySpaceAnalytics.Instance.OnResuming;
			MyLog.Default.WriteLine("SpaceEngineersGame.SetupAnalytics - END");
		}

		protected override void InitServices()
		{
			base.InitServices();
			ServiceManager.Instance.AddService((IMyGuiScreenFactoryService)new MyGuiScreenFactoryService());
		}
	}
}
