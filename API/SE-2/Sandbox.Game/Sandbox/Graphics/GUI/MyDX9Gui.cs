using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sandbox.AppCode;
using Sandbox.Common;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.GUI.DebugInputComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Input;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Graphics.GUI
{
	public class MyDX9Gui : IMyGuiSandbox
	{
		private struct LogoItem
		{
			public Type Screen;

			public string[] Args;

			public uint Id;
		}

		private static MyGuiScreenDebugBase m_currentDebugScreen;

		private static bool m_lookAroundEnabled;

		private MyGuiScreenMessageBox m_currentModErrorsMessageBox;

		private MyGuiScreenDebugBase m_currentStatisticsScreen;

		private bool m_debugScreensEnabled = true;

		private StringBuilder m_debugText = new StringBuilder();

		private Vector2 m_oldVisPos;

		private Vector2 m_oldNonVisPos;

		private bool m_oldMouseVisibilityState;

		private bool m_cameraControllerMovementAllowed;

		private Vector2 m_gameLogoSize = new Vector2(351f / 800f, 0.1975f) * 0.8f;

		private bool m_wasInputToNonFocusedScreens;

		private StringBuilder m_inputSharingText;

		private StringBuilder m_renderOverloadedText = new StringBuilder("WARNING: Render is overloaded, optimize your scene!");

		private bool m_shapeRenderingMessageBoxShown;

		internal List<MyDebugComponent> UserDebugInputComponents = new List<MyDebugComponent>();

<<<<<<< HEAD
		public string GameLogoTexture = "Textures\\GUI\\GameLogoLarge.dds";

		public static bool LookaroundEnabled => m_lookAroundEnabled;

		public Action<float, Vector2> DrawGameLogoHandler { get; set; }
=======
		internal List<MyDebugComponent> UserDebugInputComponents = new List<MyDebugComponent>();

		public string GameLogoTexture = "Textures\\GUI\\GameLogoLarge.dds";

		public static bool LookaroundEnabled => m_lookAroundEnabled;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public Action<float, Vector2> DrawGameLogoHandler { get; set; }

<<<<<<< HEAD
=======
		public Vector2 MouseCursorPosition => MyGuiManager.GetNormalizedMousePosition(MyInput.Static.GetMousePosition(), MyInput.Static.GetMouseAreaSize());

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool IsDebugScreenEnabled()
		{
			return m_debugScreensEnabled;
		}

<<<<<<< HEAD
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object" /> class.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyDX9Gui()
		{
			MySandboxGame.Log.WriteLine("MyGuiManager()");
			DrawGameLogoHandler = DrawGameLogo;
			if (MyFakes.ALT_AS_DEBUG_KEY)
			{
				m_inputSharingText = new StringBuilder("WARNING: Sharing input enabled (release ALT to disable it)");
			}
			else
			{
				m_inputSharingText = new StringBuilder("WARNING: Sharing input enabled (release Scroll Lock to disable it)");
			}
			MyGuiScreenBase.EnableSlowTransitionAnimations = MyFakes.ENABLE_SLOW_WINDOW_TRANSITION_ANIMATIONS;
			UserDebugInputComponents.Add(new MyGlobalInputComponent());
			UserDebugInputComponents.Add(new MyCharacterInputComponent());
			UserDebugInputComponents.Add(new MyPetaInputComponent());
			UserDebugInputComponents.Add(new MyVisualScriptingDebugInputComponent());
			UserDebugInputComponents.Add(new MyRendererStatsComponent());
			UserDebugInputComponents.Add(new MyTestersInputComponent());
			UserDebugInputComponents.Add(new MyAsteroidsDebugInputComponent());
			UserDebugInputComponents.Add(new MyPlanetsDebugInputComponent());
			UserDebugInputComponents.Add(new MyRenderDebugInputComponent());
			UserDebugInputComponents.Add(new MyComponentsDebugInputComponent());
			UserDebugInputComponents.Add(new MyResearchDebugInputComponent());
			UserDebugInputComponents.Add(new MyAIDebugInputComponent());
			UserDebugInputComponents.Add(new MyMartinInputComponent());
			UserDebugInputComponents.Add(new MyTomasInputComponent());
			UserDebugInputComponents.Add(new MyOndraInputComponent());
			UserDebugInputComponents.Add(new MyHonzaInputComponent());
			UserDebugInputComponents.Add(new MyCestmirDebugInputComponent());
			UserDebugInputComponents.Add(new MyAlexDebugInputComponent());
			UserDebugInputComponents.Add(new MyMichalDebugInputComponent());
			UserDebugInputComponents.Add(new MyAlesDebugInputComponent());
			UserDebugInputComponents.Add(new MyTomasDDebugInputComponent());
			LoadDebugInputsFromConfig();
		}

		public void SetMouseCursorVisibility(bool visible, bool changePosition = true)
		{
			if (m_oldMouseVisibilityState && visible != m_oldMouseVisibilityState)
			{
				m_oldVisPos = MyInput.Static.GetMousePosition();
				m_oldMouseVisibilityState = visible;
			}
			if (!m_oldMouseVisibilityState && visible != m_oldMouseVisibilityState)
			{
				m_oldNonVisPos = MyInput.Static.GetMousePosition();
				m_oldMouseVisibilityState = visible;
				if (changePosition)
				{
					MyInput.Static.SetMousePosition((int)m_oldVisPos.X, (int)m_oldVisPos.Y);
				}
			}
			MySandboxGame.Static.SetMouseVisible(visible);
		}

<<<<<<< HEAD
		/// <summary>
		/// Loads the data.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void LoadData()
		{
			MyScreenManager.LoadData();
			MyGuiManager.LoadData();
			MyLanguage.CurrentLanguage = MySandboxGame.Config.Language;
			if (MyFakes.SHOW_AUDIO_DEV_SCREEN)
			{
				MyGuiScreenDebugAudio screen = new MyGuiScreenDebugAudio();
				AddScreen(screen);
			}
		}

		public void LoadContent()
		{
			MySandboxGame.Log.WriteLine("MyGuiManager.LoadContent() - START");
			MySandboxGame.Log.IncreaseIndent();
			MyGuiManager.SetMouseCursorTexture("Textures\\GUI\\MouseCursor.dds");
			MyGuiManager.LoadContent();
			MyGuiManager.CurrentLanguage = MySandboxGame.Config.Language;
			MyScreenManager.LoadContent();
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("MyGuiManager.LoadContent() - END");
		}

		public void UnloadContent()
		{
			MyScreenManager.UnloadContent();
		}

		public void SwitchDebugScreensEnabled()
		{
			m_debugScreensEnabled = !m_debugScreensEnabled;
		}

		public bool HandleRenderProfilerInput()
		{
			return MyRenderProfiler.HandleInput();
		}

		public void AddScreen(MyGuiScreenBase screen)
		{
			MyScreenManager.AddScreen(screen);
		}

		public void InsertScreen(MyGuiScreenBase screen, int index)
		{
			MyScreenManager.InsertScreen(screen, index);
		}

		public void RemoveScreen(MyGuiScreenBase screen)
		{
			MyScreenManager.RemoveScreen(screen);
		}

		/// <summary>
		/// Sends input (keyboard/mouse) to screen which has focus (top-most)
		/// </summary>
		public void HandleInput()
		{
			try
			{
				if (MySandboxGame.Static.PauseInput || HandleRenderProfilerInput())
<<<<<<< HEAD
				{
					return;
				}
				MyTexts.SetGlobalVariantSelector(MyInput.Static.IsJoystickLastUsed ? MyTexts.GAMEPAD_VARIANT_ID : MyStringId.NullOrEmpty);
				if (MyInput.Static.IsAnyAltKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.F4))
				{
					if (MySpaceAnalytics.Instance != null)
					{
						if (MySession.Static != null)
						{
							MySpaceAnalytics.Instance.ReportWorldEnd();
						}
						MySpaceAnalytics.Instance.ReportSessionEnd("Alt+F4");
					}
					MySandboxGame.ExitThreadSafe();
					return;
				}
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SCREENSHOT))
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					TakeScreenshot();
				}
				bool flag = MyInput.Static.IsNewKeyPressed(MyKeys.F12);
				if ((MyInput.Static.IsNewKeyPressed(MyKeys.F2) || flag) && MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyAltKeyPressed())
				{
					if (MySession.Static != null && MySession.Static.CreativeMode)
					{
						if (flag)
						{
=======
				{
					return;
				}
				MyTexts.SetGlobalVariantSelector(MyInput.Static.IsJoystickLastUsed ? MyTexts.GAMEPAD_VARIANT_ID : MyStringId.NullOrEmpty);
				if (MyInput.Static.IsAnyAltKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.F4))
				{
					if (MySpaceAnalytics.Instance != null)
					{
						if (MySession.Static != null)
						{
							MySpaceAnalytics.Instance.ReportWorldEnd();
						}
						MySpaceAnalytics.Instance.ReportSessionEnd("Alt+F4");
					}
					MySandboxGame.ExitThreadSafe();
					return;
				}
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.SCREENSHOT))
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
					TakeScreenshot();
				}
				bool flag = MyInput.Static.IsNewKeyPressed(MyKeys.F12);
				if ((MyInput.Static.IsNewKeyPressed(MyKeys.F2) || flag) && MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyAltKeyPressed())
				{
					if (MySession.Static != null && MySession.Static.CreativeMode)
					{
						if (flag)
						{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							MyDebugDrawSettings.DEBUG_DRAW_PHYSICS = !MyDebugDrawSettings.DEBUG_DRAW_PHYSICS;
							if (!m_shapeRenderingMessageBoxShown)
							{
								m_shapeRenderingMessageBoxShown = true;
								AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("PHYSICS SHAPES"), messageText: new StringBuilder("Enabled physics shapes rendering. This feature is for modders only and is not part of the gameplay.")));
							}
						}
					}
					else
					{
						AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("MODDING HELPER KEYS"), messageText: new StringBuilder("Use of helper key combinations for modders is only allowed in creative mode.")));
					}
					return;
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.H) && MyInput.Static.IsAnyCtrlKeyPressed())
				{
					MyGeneralStats.ToggleProfiler();
				}
				if (MyInput.Static.IsNewKeyPressed(MyKeys.F11) && MyInput.Static.IsAnyShiftKeyPressed() && !MyInput.Static.IsAnyCtrlKeyPressed())
				{
					SwitchTimingScreen();
				}
				if (MyFakes.ENABLE_MISSION_SCREEN && MyInput.Static.IsNewKeyPressed(MyKeys.U))
				{
					MyScreenManager.AddScreen(new MyGuiScreenMission());
				}
				if (!MyFakes.ENABLE_F12_MENU && Sync.MultiplayerActive && m_currentDebugScreen is MyGuiScreenDebugOfficial)
				{
					RemoveScreen(m_currentDebugScreen);
					m_currentDebugScreen = null;
				}
				bool flag2 = false;
				if ((MySession.Static != null && MySession.Static.CreativeMode) || MyFakes.ENABLE_F12_MENU)
				{
					F12Handling();
				}
				if (MyFakes.ENABLE_F12_MENU)
				{
					if (MyInput.Static.IsNewKeyPressed(MyKeys.F11) && !MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed())
					{
						SwitchStatisticsScreen();
					}
					if (MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsAnyAltKeyPressed() && MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.Home))
					{
						throw new InvalidOperationException("Controlled crash");
					}
					if (MyInput.Static.IsNewKeyPressed(MyKeys.Pause) && MyInput.Static.IsAnyShiftKeyPressed())
					{
						GC.Collect(GC.MaxGeneration);
					}
					if (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.F2))
					{
						if (MyInput.Static.IsAnyAltKeyPressed() && MyInput.Static.IsAnyShiftKeyPressed())
						{
							MyDefinitionManager.Static.ReloadParticles();
						}
						else if (MyInput.Static.IsAnyShiftKeyPressed())
						{
							MyDefinitionManager.Static.ReloadDecalMaterials();
							MyRenderProxy.ReloadTextures();
						}
						else if (MyInput.Static.IsAnyAltKeyPressed())
						{
							MyRenderProxy.ReloadModels();
						}
						else
						{
							MyRenderProxy.ReloadEffects();
						}
					}
					flag2 = HandleDebugInput();
				}
				if (!flag2)
				{
					MyScreenManager.HandleInput();
				}
			}
			finally
			{
			}
		}

		private void F12Handling()
		{
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F12))
			{
				if (MyFakes.ENABLE_F12_MENU)
				{
					ShowDeveloperDebugScreen();
				}
				else if (m_currentDebugScreen is MyGuiScreenDebugDeveloper)
				{
					RemoveScreen(m_currentDebugScreen);
					m_currentDebugScreen = null;
				}
			}
			if (MyFakes.ALT_AS_DEBUG_KEY)
			{
				MyScreenManager.InputToNonFocusedScreens = MyInput.Static.IsAnyAltKeyPressed() && !MyInput.Static.IsKeyPress(MyKeys.Tab);
			}
			else
			{
				MyScreenManager.InputToNonFocusedScreens = MyInput.Static.IsKeyPress(MyKeys.ScrollLock) && !MyInput.Static.IsKeyPress(MyKeys.Tab);
			}
			if (MyScreenManager.InputToNonFocusedScreens != m_wasInputToNonFocusedScreens)
			{
				if (MyScreenManager.InputToNonFocusedScreens && m_currentDebugScreen != null)
				{
					SetMouseCursorVisibility(MyScreenManager.InputToNonFocusedScreens);
				}
				m_wasInputToNonFocusedScreens = MyScreenManager.InputToNonFocusedScreens;
			}
		}

		public static void SwitchModDebugScreen()
		{
			if (!MyFakes.ENABLE_F12_MENU && Sync.MultiplayerActive)
			{
				return;
			}
			if (m_currentDebugScreen != null)
			{
				if (m_currentDebugScreen is MyGuiScreenDebugOfficial)
				{
					m_currentDebugScreen.CloseScreen();
					m_currentDebugScreen = null;
				}
			}
			else
			{
				ShowModDebugScreen();
			}
		}

		private static void ShowModDebugScreen()
		{
			if (m_currentDebugScreen == null)
			{
				m_currentDebugScreen = new MyGuiScreenDebugOfficial();
				MyScreenManager.AddScreen(m_currentDebugScreen);
				m_currentDebugScreen.Closed += delegate
				{
					m_currentDebugScreen = null;
				};
			}
			else if (m_currentDebugScreen is MyGuiScreenDebugOfficial)
			{
				m_currentDebugScreen.RecreateControls(constructor: false);
			}
		}

		private void ShowModErrorsMessageBox()
		{
			ListReader<MyDefinitionErrors.Error> errors = MyDefinitionErrors.GetErrors();
			if (m_currentModErrorsMessageBox != null)
			{
				RemoveScreen(m_currentModErrorsMessageBox);
			}
			StringBuilder stringBuilder = new StringBuilder(MyTexts.GetString(MyCommonTexts.MessageBoxErrorModLoadingFailure));
			stringBuilder.Append("\n");
<<<<<<< HEAD
			HashSet<string> hashSet = new HashSet<string>();
			foreach (MyDefinitionErrors.Error item in errors)
			{
				string modName = item.ModName;
				if (item.Severity == TErrorSeverity.Critical && !string.IsNullOrWhiteSpace(modName) && hashSet.Add(modName))
=======
			HashSet<string> val = new HashSet<string>();
			foreach (MyDefinitionErrors.Error item in errors)
			{
				string modName = item.ModName;
				if (item.Severity == TErrorSeverity.Critical && !string.IsNullOrWhiteSpace(modName) && val.Add(modName))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					stringBuilder.Append("\n");
					stringBuilder.Append(modName);
				}
			}
			stringBuilder.Append("\n");
			m_currentModErrorsMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, stringBuilder, MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
			Vector2 value = m_currentModErrorsMessageBox.Size.Value;
			value.Y *= 1.5f;
			m_currentModErrorsMessageBox.Size = value;
			m_currentModErrorsMessageBox.RecreateControls(constructor: false);
			AddScreen(m_currentModErrorsMessageBox);
		}

		public void ShowModErrors()
		{
			if (MyFakes.ENABLE_F12_MENU || !Sync.MultiplayerActive)
			{
				ShowModDebugScreen();
			}
			else
			{
				ShowModErrorsMessageBox();
			}
		}

		private void ShowDeveloperDebugScreen()
		{
			if (!(m_currentDebugScreen is MyGuiScreenDebugOfficial) && !(m_currentDebugScreen is MyGuiScreenDebugDeveloper))
			{
				if (m_currentDebugScreen != null)
				{
					RemoveScreen(m_currentDebugScreen);
				}
				MyGuiScreenDebugDeveloper currentDebugScreen = new MyGuiScreenDebugDeveloper();
				AddScreen(m_currentDebugScreen = currentDebugScreen);
				m_currentDebugScreen.Closed += delegate
				{
					m_currentDebugScreen = null;
				};
			}
		}

		/// <summary>
		/// Sends input (keyboard/mouse) to screen which has focus (top-most)
		/// </summary>
		public void HandleInputAfterSimulation()
		{
			if (MySession.Static == null)
			{
				return;
			}
			bool flag = MyScreenManager.GetScreenWithFocus() == MyGuiScreenGamePlay.Static && MyGuiScreenGamePlay.Static != null && !MyScreenManager.InputToNonFocusedScreens;
			bool flag2 = MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED) || (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.PrimaryLookaround);
			bool flag3 = MySession.Static.ControlledEntity != null && !flag && m_cameraControllerMovementAllowed != flag;
			MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
			if (MySandboxGame.IsPaused && screenWithFocus is MyGuiScreenGamePlay && !MyScreenManager.InputToNonFocusedScreens)
			{
				if (MySession.Static.GetCameraControllerEnum() != 0 && MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.SpectatorDelta && MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.SpectatorFixed && MySession.Static.GetCameraControllerEnum() != MyCameraControllerEnum.SpectatorOrbit)
				{
					return;
				}
			}
			else if (flag2)
			{
				if (flag)
				{
					float roll = MyInput.Static.GetRoll();
					Vector2 rotationIndicator = (MyInput.Static.IsJoystickLastUsed ? MyInput.Static.GetLookAroundRotation() : MyInput.Static.GetRotation());
					MySession.Static.CameraController.Rotate(rotationIndicator, roll);
					if (!m_lookAroundEnabled && flag3)
					{
						MySession.Static.ControlledEntity.MoveAndRotateStopped();
					}
				}
				if (flag3)
				{
					MySession.Static.CameraController.RotateStopped();
				}
			}
			else if (MySession.Static.CameraController is MySpectatorCameraController && MySpectatorCameraController.Static.SpectatorCameraMovement == MySpectatorCameraMovementEnum.ConstantDelta && flag)
			{
				float roll2 = MyInput.Static.GetRoll();
				Vector2 rotation = MyInput.Static.GetRotation();
				Vector3 positionDelta = MyInput.Static.GetPositionDelta();
				MySpectatorCameraController.Static.MoveAndRotate(positionDelta, rotation, roll2);
			}
			MyScreenManager.HandleInputAfterSimulation();
			if (flag3)
			{
				MySession.Static.ControlledEntity.MoveAndRotateStopped();
			}
			m_cameraControllerMovementAllowed = flag;
			m_lookAroundEnabled = flag2;
		}

		private void SwitchTimingScreen()
		{
			MyRenderProxy.IsPlayerSpaceMaster = true;
			if (MySession.Static != null)
			{
				MyRenderProxy.IsPlayerSpaceMaster = MySession.Static.IsUserSpaceMaster(MyGameService.UserId);
			}
			if (!(m_currentStatisticsScreen is MyGuiScreenDebugTiming))
			{
				if (m_currentStatisticsScreen != null)
				{
					RemoveScreen(m_currentStatisticsScreen);
				}
				AddScreen(m_currentStatisticsScreen = new MyGuiScreenDebugTiming());
			}
			else if (MyRenderProxy.DrawRenderStats == MyRenderProxy.MyStatsState.SimpleTimingStats)
			{
				MyRenderProxy.DrawRenderStats = MyRenderProxy.MyStatsState.ComplexTimingStats;
			}
			else
			{
				MyRenderProxy.DrawRenderStats = MyRenderProxy.MyStatsState.MoveNext;
			}
		}

		private void SwitchStatisticsScreen()
		{
			MyGuiScreenDebugStatistics myGuiScreenDebugStatistics;
			if ((myGuiScreenDebugStatistics = m_currentStatisticsScreen as MyGuiScreenDebugStatistics) != null)
			{
				if (!myGuiScreenDebugStatistics.Cycle())
				{
					RemoveScreen(m_currentStatisticsScreen);
					m_currentStatisticsScreen = null;
				}
			}
			else
			{
				if (m_currentStatisticsScreen != null)
				{
					RemoveScreen(m_currentStatisticsScreen);
				}
				AddScreen(m_currentStatisticsScreen = new MyGuiScreenDebugStatistics());
			}
		}

		private void SwitchInputScreen()
		{
			if (!(m_currentStatisticsScreen is MyGuiScreenDebugInput))
			{
				if (m_currentStatisticsScreen != null)
				{
					RemoveScreen(m_currentStatisticsScreen);
				}
				AddScreen(m_currentStatisticsScreen = new MyGuiScreenDebugInput());
			}
			else
			{
				RemoveScreen(m_currentStatisticsScreen);
				m_currentStatisticsScreen = null;
			}
		}

		public void Update(int totalTimeInMS)
		{
			MyScreenManager.Update(totalTimeInMS);
			MyScreenManager.GetScreenWithFocus();
			bool gameFocused = MySandboxGame.Static.IsActive && ((MyExternalAppBase.Static == null && MyVRage.Platform.Windows.Window.IsActive) || (MyExternalAppBase.Static != null && !MyExternalAppBase.IsEditorActive));
			if (MyRenderProxy.DrawRenderStats == MyRenderProxy.MyStatsState.Last)
			{
				RemoveScreen(m_currentStatisticsScreen);
				m_currentStatisticsScreen = null;
			}
			MyInput.Static.Update(gameFocused);
			MyGuiManager.Update(totalTimeInMS);
			MyGuiManager.MouseCursorPosition = MouseCursorPosition;
			MyGuiManager.TotalTimeInMilliseconds = MySandboxGame.TotalTimeInMilliseconds;
		}

		private void DrawMouseCursor(string mouseCursorTexture)
		{
			if (mouseCursorTexture != null)
			{
				Vector2 normalizedSize = MyGuiManager.GetNormalizedSize(new Vector2(64f), 1f);
				MyGuiManager.DrawSpriteBatch(mouseCursorTexture, MouseCursorPosition, normalizedSize, new Color(MyGuiConstants.MOUSE_CURSOR_COLOR), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, useFullClientArea: false, waitTillLoaded: false);
			}
		}

		private string GetMouseOverTexture(MyGuiScreenBase screen)
		{
			if (screen != null)
			{
				MyGuiControlBase mouseOverControl = screen.GetMouseOverControl();
				if (mouseOverControl != null)
				{
					return mouseOverControl.GetMouseCursorTexture() ?? MyGuiManager.GetMouseCursorTexture();
				}
			}
			return MyGuiManager.GetMouseCursorTexture();
		}

		/// <summary>
		/// Draw all screens
		/// </summary>
		public void Draw()
		{
			MyScreenManager.Draw();
			m_debugText.Clear();
			if (MyFakes.ENABLE_F12_MENU && MySandboxGame.Config.DebugComponentsInfo != 0)
			{
				float num = 0f;
				int num2 = 0;
				bool flag = false;
				MyDebugComponent.ResetFrame();
				foreach (MyDebugComponent userDebugInputComponent in UserDebugInputComponents)
				{
					if (userDebugInputComponent.Enabled)
					{
						if (num == 0f)
						{
							m_debugText.AppendLine("Debug input:");
							m_debugText.AppendLine();
							num += 0.063f;
						}
						m_debugText.ConcatFormat("{0} (Ctrl + numPad{1})", UserDebugInputComponents[num2].GetName(), num2);
						m_debugText.AppendLine();
						num += 0.0265f;
						if (MySession.Static != null)
						{
							userDebugInputComponent.DispatchUpdate();
						}
						userDebugInputComponent.Draw();
						flag = true;
					}
					num2++;
				}
				if (flag)
				{
					MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Controls\\rectangle_dark_center.dds", new Vector2(MyGuiManager.GetMaxMouseCoord().X, 0f), new Vector2(MyGuiManager.MeasureString("White", m_debugText, 1f).X + 0.012f, num), new Color(0, 0, 0, 130), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
					MyGuiManager.DrawString("White", m_debugText.ToString(), new Vector2(MyGuiManager.GetMaxMouseCoord().X - 0.01f, 0f), 1f, Color.White, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
				}
			}
			bool flag2 = MyVideoSettingsManager.IsHardwareCursorUsed();
			MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
			if ((screenWithFocus != null && screenWithFocus.GetDrawMouseCursor()) || (MyScreenManager.InputToNonFocusedScreens && MyScreenManager.GetScreensCount() > 1))
			{
				SetMouseCursorVisibility(flag2 && !MyInput.Static.IsJoystickLastUsed, changePosition: false);
				if (!flag2 || MyFakes.FORCE_SOFTWARE_MOUSE_DRAW)
				{
					DrawMouseCursor(GetMouseOverTexture(screenWithFocus));
				}
			}
			else if (flag2 && screenWithFocus != null)
			{
				SetMouseCursorVisibility(screenWithFocus.GetDrawMouseCursor());
			}
		}

		public void BackToIntroLogos(Action afterLogosAction)
		{
			MyScreenManager.CloseAllScreensNowExcept(null, isUnloading: true);
			uint introVideoId = MySandboxGame.Static.IntroVideoId;
			LogoItem[] array = new LogoItem[3];
			LogoItem logoItem = new LogoItem
			{
				Screen = typeof(MyGuiScreenIntroVideo),
				Args = ((introVideoId != 0) ? null : new string[1] { "Videos\\KSH.wmv" }),
				Id = introVideoId
			};
			array[0] = logoItem;
			logoItem = (array[1] = new LogoItem
			{
				Screen = typeof(MyGuiScreenLogo),
				Args = new string[1] { "Textures\\Logo\\vrage_logo_2_0_white.dds" }
			});
			logoItem = (array[2] = new LogoItem
			{
				Screen = typeof(MyGuiScreenLogo),
				Args = new string[1] { "Textures\\Logo\\se.dds" }
			});
			MyGuiScreenBase myGuiScreenBase = null;
			LogoItem[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				LogoItem logoItem2 = array2[i];
				List<string> list;
				if (logoItem2.Args != null)
				{
					list = new List<string>();
					string[] args = logoItem2.Args;
					foreach (string text in args)
					{
						if (MyFileSystem.FileExists(Path.Combine(MyFileSystem.ContentPath, text)))
						{
							list.Add(text);
						}
					}
					if (list.Count == 0)
					{
						continue;
					}
				}
				else
				{
					list = null;
				}
				MyGuiScreenBase myGuiScreenBase2 = (MyGuiScreenBase)Activator.CreateInstance(logoItem2.Screen, (list == null) ? new object[2] { null, logoItem2.Id } : new object[2]
				{
					list.ToArray(),
					logoItem2.Id
				});
				if (myGuiScreenBase != null)
				{
					AddCloseHandler(myGuiScreenBase, myGuiScreenBase2, afterLogosAction);
				}
				else
				{
					AddScreen(myGuiScreenBase2);
				}
				myGuiScreenBase = myGuiScreenBase2;
			}
			if (myGuiScreenBase != null)
			{
				myGuiScreenBase.Closed += delegate
				{
					afterLogosAction();
				};
			}
			else
			{
				afterLogosAction();
			}
		}

		private void AddCloseHandler(MyGuiScreenBase previousScreen, MyGuiScreenBase logoScreen, Action afterLogosAction)
		{
			previousScreen.Closed += delegate(MyGuiScreenBase screen, bool isUnloading)
			{
				if (!screen.Cancelled)
				{
					AddScreen(logoScreen);
				}
				else
				{
					afterLogosAction();
				}
			};
		}

		public void BackToMainMenu()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.MainMenu));
		}

		public float GetDefaultTextScaleWithLanguage()
		{
			return 0.8f * MyGuiManager.LanguageTextScale;
		}

		public void TakeScreenshot(int width, int height, string saveToPath = null, bool ignoreSprites = false, bool showNotification = true)
		{
			TakeScreenshot(saveToPath, ignoreSprites, new Vector2(width, height) / MySandboxGame.ScreenSize, showNotification);
		}

		public void TakeScreenshot(string saveToPath = null, bool ignoreSprites = false, Vector2? sizeMultiplier = null, bool showNotification = true)
		{
			if (!sizeMultiplier.HasValue)
			{
				sizeMultiplier = new Vector2(MySandboxGame.Config.ScreenshotSizeMultiplier);
			}
			MyRenderProxy.TakeScreenshot(sizeMultiplier.Value, saveToPath, debug: false, ignoreSprites, showNotification);
		}

		public Vector2 GetNormalizedCoordsAndPreserveOriginalSize(int width, int height)
		{
			return new Vector2((float)width / (float)MySandboxGame.ScreenSize.X, (float)height / (float)MySandboxGame.ScreenSize.Y);
		}

		public void DrawGameLogo(float transitionAlpha, Vector2 position)
		{
			Color color = Color.White * transitionAlpha;
			MyGuiManager.DrawSpriteBatch(GameLogoTexture, position, new Vector2(351f / 800f, 0.1975f), color, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
		}

		public void DrawBadge(string texture, float transitionAlpha, Vector2 position, Vector2 size)
		{
			Color color = Color.White * transitionAlpha;
			MyGuiManager.DrawSpriteBatch(texture, position, size, color, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
		}

		private bool HandleDebugInput()
		{
			if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				int num = -1;
				for (int i = 0; i < 10; i++)
				{
					if (MyInput.Static.IsNewKeyPressed((MyKeys)(96 + i)))
					{
						num = i;
						if (MyInput.Static.IsAnyAltKeyPressed())
						{
							num += 10;
						}
						break;
					}
				}
				if (num > -1 && num < UserDebugInputComponents.Count)
				{
					MyDebugComponent myDebugComponent = UserDebugInputComponents[num];
					myDebugComponent.Enabled = !myDebugComponent.Enabled;
					SaveDebugInputsToConfig();
					return false;
				}
			}
			bool flag = false;
			foreach (MyDebugComponent userDebugInputComponent in UserDebugInputComponents)
			{
				if (userDebugInputComponent.Enabled && !MyInput.Static.IsAnyAltKeyPressed())
				{
					flag = userDebugInputComponent.HandleInput() || flag;
				}
				if (flag)
				{
					return flag;
				}
			}
			return flag;
		}

		private void SaveDebugInputsToConfig()
		{
			MySandboxGame.Config.DebugInputComponents.Dictionary.Clear();
			SerializableDictionary<string, MyConfig.MyDebugInputData> debugInputComponents = MySandboxGame.Config.DebugInputComponents;
			foreach (MyDebugComponent userDebugInputComponent in UserDebugInputComponents)
			{
				string name = userDebugInputComponent.GetName();
				debugInputComponents.Dictionary.TryGetValue(name, out var value);
				value.Enabled = userDebugInputComponent.Enabled;
				value.Data = userDebugInputComponent.InputData;
				debugInputComponents[name] = value;
			}
			MySandboxGame.Config.Save();
		}

		private void LoadDebugInputsFromConfig()
		{
			foreach (KeyValuePair<string, MyConfig.MyDebugInputData> item in MySandboxGame.Config.DebugInputComponents.Dictionary)
			{
				for (int i = 0; i < UserDebugInputComponents.Count; i++)
				{
					if (UserDebugInputComponents[i].GetName() == item.Key)
					{
						UserDebugInputComponents[i].Enabled = item.Value.Enabled;
						try
						{
							UserDebugInputComponents[i].InputData = item.Value.Data;
						}
						catch
						{
						}
					}
				}
			}
		}
	}
}
