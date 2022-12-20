using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Sandbox.Engine.Networking;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage;
using VRage.Input;
using VRage.Plugins;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public static class MyGuiSandbox
	{
		public static Regex urlRgx = new Regex("^(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?$");

		internal static IMyGuiSandbox Gui = new MyNullGui();

		private static Dictionary<Type, Type> m_createdScreenTypes = new Dictionary<Type, Type>();

		public static int TotalGamePlayTimeInMilliseconds;

		/// <summary>
		/// Event triggered on gui control created.
		/// </summary>
		public static Action<object> GuiControlCreated;

		/// <summary>
		/// Event triggered on gui control removed.
		/// </summary>
		public static Action<object> GuiControlRemoved;

		private static Regex[] WWW_WHITELIST = (Regex[])(object)new Regex[4]
		{
			new Regex("^(http[s]{0,1}://){0,1}[^/]*youtube.com/.*", (RegexOptions)1),
			new Regex("^(http[s]{0,1}://){0,1}[^/]*youtu.be/.*", (RegexOptions)1),
			new Regex("^(http[s]{0,1}://){0,1}[^/]*steamcommunity.com/.*", (RegexOptions)1),
			new Regex("^(http[s]{0,1}://){0,1}[^/]*forum[s]{0,1}.keenswh.com/.*", (RegexOptions)1)
		};

		private const int m_logLatency = 1800;

		private static int m_frame = 0;

		private static Stopwatch m_timer = new Stopwatch();

		private static double m_pastDrawTime;

		private static double m_pastUpdateTime;

		public static Vector2 MouseCursorPosition => Gui.MouseCursorPosition;

		public static Action<float, Vector2> DrawGameLogoHandler
		{
			get
			{
				return Gui.DrawGameLogoHandler;
			}
			set
			{
				Gui.DrawGameLogoHandler = value;
			}
		}

		public static void SetMouseCursorVisibility(bool visible, bool changePosition = true)
		{
			Gui.SetMouseCursorVisibility(visible, changePosition);
		}

		private static void AnselWarningMessage(bool pauseAllowed, bool spectatorEnabled)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				AnselWarningMessageInternal(pauseAllowed, spectatorEnabled);
			}, "AnselWarningMessage");
		}

		private static void AnselWarningMessageInternal(bool pauseAllowed, bool spectatorEnabled)
		{
			if (!pauseAllowed || !spectatorEnabled)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (!pauseAllowed)
				{
					stringBuilder.Append((object)MyTexts.Get(MyCommonTexts.MessageBoxTextAnselCannotPauseOnlineGame));
					stringBuilder.AppendLine("");
				}
				if (!spectatorEnabled)
				{
					stringBuilder.Append((object)MyTexts.Get(MyCommonTexts.MessageBoxTextAnselSpectatorDisabled));
					stringBuilder.AppendLine("");
				}
				stringBuilder.Append((object)MyTexts.Get(MyCommonTexts.MessageBoxTextAnselTimeout));
				AddScreen(CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.NONE_TIMEOUT, stringBuilder, MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), null, null, null, null, null, 4000));
			}
		}

		public static bool Ansel_IsSpectatorEnabled()
		{
			return MyGuiScreenGamePlay.SpectatorEnabled;
		}

		/// <summary>
		/// Loads the data.
		/// </summary>
		public static void LoadData(bool nullGui)
		{
			MyVRage.Platform.Ansel.WarningMessageDelegate += AnselWarningMessage;
			MyVRage.Platform.Ansel.IsSpectatorEnabledDelegate += Ansel_IsSpectatorEnabled;
			if (!nullGui)
			{
				Gui = new MyDX9Gui();
			}
			Gui.LoadData();
		}

		public static void LoadContent()
		{
			Gui.LoadContent();
		}

		public static bool IsUrlWhitelisted(string wwwLink)
		{
			Regex[] wWW_WHITELIST = WWW_WHITELIST;
			for (int i = 0; i < wWW_WHITELIST.Length; i++)
			{
				if (wWW_WHITELIST[i].IsMatch(wwwLink))
				{
					return true;
				}
			}
			return false;
		}

<<<<<<< HEAD
		/// <summary>
		/// Opens URL in Steam overlay or external browser.
		/// </summary>
		/// <param name="url">Url to open.</param>
		/// <param name="urlFriendlyName">Friendly name of URL to show in confirmation screen, e.g. Steam Workshop</param>
		/// <param name="useWhitelist"></param>
		/// <param name="onDone"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static void OpenUrlWithFallback(string url, string urlFriendlyName, bool useWhitelist = false, Action<bool> onDone = null)
		{
			if (useWhitelist && !IsUrlWhitelisted(url))
			{
				MySandboxGame.Log.WriteLine("URL NOT ALLOWED: " + url);
				onDone.InvokeIfNotNull(arg1: false);
			}
			else
			{
				StringBuilder confirmMessageBrowser = new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextOpenBrowser), urlFriendlyName, MySession.GameServiceName);
				OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm, confirmMessageBrowser, null, null, null, onDone);
			}
		}

		public static bool IsUrlValid(string url)
		{
			return urlRgx.IsMatch(url);
		}

		private static bool OpenSteamOverlay(string url)
		{
			if (MyGameService.IsOverlayBrowserAvailable)
			{
				MyGameService.OpenOverlayUrl(url);
				return true;
			}
			return false;
		}

<<<<<<< HEAD
		/// <summary>
		/// Opens URL in Steam overlay or external browser.
		/// </summary>
		/// <param name="url">Url to open.</param>
		/// <param name="openMode">How to open the url.</param>
		/// <param name="confirmMessageBrowser"></param>
		/// <param name="confirmCaptionBrowser"></param>
		/// <param name="confirmMessageOverlay"></param>
		/// <param name="confirmCaptionOverlay"></param>
		/// <param name="onDone"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static void OpenUrl(string url, UrlOpenMode openMode, StringBuilder confirmMessageBrowser = null, StringBuilder confirmCaptionBrowser = null, StringBuilder confirmMessageOverlay = null, StringBuilder confirmCaptionOverlay = null, Action<bool> onDone = null)
		{
			bool num = (openMode & UrlOpenMode.SteamOverlay) != 0;
			bool flag = (openMode & UrlOpenMode.ExternalBrowser) != 0;
			bool flag2 = (openMode & UrlOpenMode.ConfirmExternal) != 0;
			bool flag3 = false;
			if (num)
			{
				if (flag2 && confirmMessageOverlay != null)
				{
					if (MyGameService.IsOverlayBrowserAvailable)
					{
						StringBuilder messageCaption = confirmCaptionOverlay ?? MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm);
						AddScreen(CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, confirmMessageOverlay, messageCaption, null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum retval)
						{
							if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
							{
								OpenSteamOverlay(url);
								onDone.InvokeIfNotNull(arg1: true);
							}
							else
							{
								onDone.InvokeIfNotNull(arg1: false);
							}
						}));
						return;
					}
				}
				else
				{
					flag3 = OpenSteamOverlay(url);
				}
			}
			if (!flag3 && flag)
			{
				if (flag2)
				{
					AddScreen(CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: confirmCaptionBrowser ?? MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), messageText: confirmMessageBrowser ?? new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextOpenBrowser, url), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum retval)
					{
						if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							bool arg2 = OpenExternalBrowser(url);
							onDone.InvokeIfNotNull(arg2);
						}
						else
						{
							onDone.InvokeIfNotNull(arg1: false);
						}
					}));
				}
				else
				{
					bool arg = OpenExternalBrowser(url);
					onDone.InvokeIfNotNull(arg);
				}
			}
			else
			{
				onDone.InvokeIfNotNull(arg1: true);
			}
		}

		private static bool OpenExternalBrowser(string url)
		{
			if (!MyVRage.Platform.System.OpenUrl(url))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(MyTexts.GetString(MyCommonTexts.TitleFailedToStartInternetBrowser), url);
				StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.TitleFailedToStartInternetBrowser);
				AddScreen(CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, stringBuilder, messageCaption));
				return false;
			}
			return true;
		}

		public static void UnloadContent()
		{
			Gui.UnloadContent();
		}

		public static void SwitchDebugScreensEnabled()
		{
			Gui.SwitchDebugScreensEnabled();
		}

		public static void ShowModErrors()
		{
			Gui.ShowModErrors();
		}

		public static bool IsDebugScreenEnabled()
		{
			return Gui.IsDebugScreenEnabled();
		}

		public static MyGuiScreenBase CreateScreen(Type screenType, params object[] args)
		{
			return Activator.CreateInstance(screenType, args) as MyGuiScreenBase;
		}

		public static T CreateScreen<T>(params object[] args) where T : MyGuiScreenBase
		{
			Type value = null;
			if (!m_createdScreenTypes.TryGetValue(typeof(T), out value))
			{
				Type typeFromHandle = typeof(T);
				value = typeFromHandle;
				ChooseScreenType<T>(ref value, MyPlugins.GameAssembly);
				ChooseScreenType<T>(ref value, MyPlugins.SandboxAssembly);
				ChooseScreenType<T>(ref value, MyPlugins.UserAssemblies);
				m_createdScreenTypes[typeFromHandle] = value;
			}
			return Activator.CreateInstance(value, args) as T;
		}

		private static void ChooseScreenType<T>(ref Type createdType, Assembly[] assemblies) where T : MyGuiScreenBase
		{
			if (assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
				{
					ChooseScreenType<T>(ref createdType, assembly);
				}
			}
		}

		private static void ChooseScreenType<T>(ref Type createdType, Assembly assembly) where T : MyGuiScreenBase
		{
			if (assembly == null)
			{
				return;
			}
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (typeof(T).IsAssignableFrom(type))
				{
					createdType = type;
					break;
				}
			}
		}

		public static void AddScreen(MyGuiScreenBase screen)
		{
			Gui.AddScreen(screen);
			if (GuiControlCreated != null)
			{
				GuiControlCreated(screen);
			}
			screen.Closed += delegate(MyGuiScreenBase x, bool isUnloading)
			{
				if (GuiControlRemoved != null)
				{
					GuiControlRemoved(x);
				}
			};
			if (MyAPIGateway.GuiControlCreated != null)
			{
				MyAPIGateway.GuiControlCreated(screen);
			}
		}

		public static void InsertScreen(MyGuiScreenBase screen, int index)
		{
			Gui.InsertScreen(screen, index);
			if (GuiControlCreated != null)
			{
				GuiControlCreated(screen);
			}
			screen.Closed += delegate(MyGuiScreenBase x, bool isUnloading)
			{
				if (GuiControlRemoved != null)
				{
					GuiControlRemoved(x);
				}
			};
			if (MyAPIGateway.GuiControlCreated != null)
			{
				MyAPIGateway.GuiControlCreated(screen);
			}
		}

		public static void RemoveScreen(MyGuiScreenBase screen)
		{
			Gui.RemoveScreen(screen);
			if (GuiControlRemoved != null)
			{
				GuiControlRemoved(screen);
			}
		}

		public static void HandleInput()
		{
			Gui.HandleInput();
		}

		public static void HandleInputAfterSimulation()
		{
			Gui.HandleInputAfterSimulation();
		}

		public static void Update(int totalTimeInMS)
		{
			m_timer.Restart();
			Gui.Update(totalTimeInMS);
			m_timer.Stop();
<<<<<<< HEAD
			m_pastUpdateTime += m_timer.Elapsed.TotalMilliseconds;
=======
			m_pastUpdateTime += m_timer.get_Elapsed().TotalMilliseconds;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static void Draw()
		{
			m_timer.Restart();
			Gui.Draw();
			m_timer.Stop();
<<<<<<< HEAD
			m_pastDrawTime += m_timer.Elapsed.TotalMilliseconds;
=======
			m_pastDrawTime += m_timer.get_Elapsed().TotalMilliseconds;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (++m_frame == 1800)
			{
				m_frame = 0;
				MyLog.Default.WriteLine($"GUI Stats: Update {m_pastUpdateTime / 1800.0}, Draw {m_pastDrawTime / 1800.0}");
				m_pastUpdateTime = (m_pastDrawTime = 0.0);
			}
		}

		public static void BackToIntroLogos(Action afterLogosAction)
		{
			Gui.BackToIntroLogos(afterLogosAction);
		}

		public static void BackToMainMenu()
		{
			Gui.BackToMainMenu();
		}

		public static float GetDefaultTextScaleWithLanguage()
		{
			return Gui.GetDefaultTextScaleWithLanguage();
		}

		public static void TakeScreenshot(int width, int height, string saveToPath = null, bool ignoreSprites = false, bool showNotification = true)
		{
			Gui.TakeScreenshot(width, height, saveToPath, ignoreSprites, showNotification);
		}

		public static MyGuiScreenMessageBox CreateMessageBox(MyMessageBoxStyleEnum styleEnum = MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType buttonType = MyMessageBoxButtonsType.OK, StringBuilder messageText = null, StringBuilder messageCaption = null, MyStringId? okButtonText = null, MyStringId? cancelButtonText = null, MyStringId? yesButtonText = null, MyStringId? noButtonText = null, Action<MyGuiScreenMessageBox.ResultEnum> callback = null, int timeoutInMiliseconds = 0, MyGuiScreenMessageBox.ResultEnum focusedResult = MyGuiScreenMessageBox.ResultEnum.YES, bool canHideOthers = true, Vector2? size = null, bool useOpacity = true, Vector2? position = null, bool focusable = true, bool canBeHidden = false, Action onClosing = null)
		{
			return new MyGuiScreenMessageBox(styleEnum, buttonType, messageText, messageCaption, okButtonText ?? MyCommonTexts.Ok, cancelButtonText ?? MyCommonTexts.Cancel, yesButtonText ?? MyCommonTexts.Yes, noButtonText ?? MyCommonTexts.No, callback, timeoutInMiliseconds, focusedResult, canHideOthers, size, useOpacity ? MySandboxGame.Config.UIBkOpacity : 1f, useOpacity ? MySandboxGame.Config.UIOpacity : 1f, position, focusable, canBeHidden, onClosing);
		}

		public static void Show(StringBuilder text, MyStringId caption = default(MyStringId), MyMessageBoxStyleEnum type = MyMessageBoxStyleEnum.Error)
		{
			AddScreen(CreateMessageBox(type, MyMessageBoxButtonsType.OK, text, MyTexts.Get(caption)));
		}

		public static void Show(MyStringId text, MyStringId caption = default(MyStringId), MyMessageBoxStyleEnum type = MyMessageBoxStyleEnum.Error)
		{
			AddScreen(CreateMessageBox(type, MyMessageBoxButtonsType.OK, MyTexts.Get(text), MyTexts.Get(caption)));
		}

		public static void DrawGameLogo(float transitionAlpha, Vector2 position)
		{
			Gui.DrawGameLogo(transitionAlpha, position);
		}

		public static void DrawBadge(string texture, float transitionAlpha, Vector2 position, Vector2 size)
		{
			Gui.DrawBadge(texture, transitionAlpha, position, size);
		}

		public static string GetKeyName(MyStringId control)
		{
			MyControl gameControl = MyInput.Static.GetGameControl(control);
			if (gameControl != null)
			{
				return gameControl.GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
			}
			return "";
		}
	}
}
