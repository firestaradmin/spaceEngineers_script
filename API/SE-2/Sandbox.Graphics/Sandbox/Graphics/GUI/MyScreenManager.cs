using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using VRage;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Graphics.GUI
{
	public static class MyScreenManager
	{
		private static readonly FastResourceLock lockObject;

		public static int TotalGamePlayTimeInMilliseconds;

		private static MyGuiScreenBase m_lastScreenWithFocus;

		private static List<MyGuiScreenBase> m_screens;

		private static List<MyGuiScreenBase> m_screensToRemove;

		private static List<MyGuiScreenBase> m_screensToAdd;

		public static Func<StringBuilder, string> OnValidateText;

		private static bool m_inputToNonFocusedScreens;

		private static List<MyGuiScreenBase> m_screensToDraw;

		private static List<MyGuiScreenBase> m_screensToDraw;

		private static StringBuilder m_sb;

		public static MyGuiScreenBase LastScreenWithFocus => m_lastScreenWithFocus;

		public static Thread UpdateThread { get; set; }

		public static MyGuiControlBase FocusedControl => GetScreenWithFocus()?.FocusedControl;

		public static bool InputToNonFocusedScreens
		{
			get
			{
				return m_inputToNonFocusedScreens;
			}
			set
			{
				m_inputToNonFocusedScreens = value;
			}
		}

		/// <summary>
		/// Corrently active screens.
		/// </summary>
		public static IEnumerable<MyGuiScreenBase> Screens => m_screens;

		public static event Action<MyGuiScreenBase> ScreenAdded;

		public static event Action<MyGuiScreenBase> ScreenRemoved;

		public static event Action EndOfDraw;

<<<<<<< HEAD
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object" /> class.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		static MyScreenManager()
		{
			lockObject = new FastResourceLock();
			m_inputToNonFocusedScreens = false;
<<<<<<< HEAD
=======
			m_wasInputToNonFocusedScreens = false;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_screensToDraw = new List<MyGuiScreenBase>();
			m_sb = new StringBuilder(512);
			MyLog.Default.WriteLine("MyScreenManager()");
			m_screens = new List<MyGuiScreenBase>();
			m_screensToRemove = new List<MyGuiScreenBase>();
			m_screensToAdd = new List<MyGuiScreenBase>();
		}

		/// <summary>
		/// Loads the data.
		/// </summary>
		public static void LoadData()
		{
			m_screens.Clear();
			using (lockObject.AcquireExclusiveUsing())
			{
				m_screensToRemove.Clear();
			}
			m_screensToAdd.Clear();
		}

		public static void LoadContent()
		{
			MyLog.Default.WriteLine("MyGuiManager.LoadContent() - START");
			MyLog.Default.IncreaseIndent();
			foreach (MyGuiScreenBase screen in m_screens)
			{
				screen.LoadContent();
			}
			MyLog.Default.DecreaseIndent();
			MyLog.Default.WriteLine("MyGuiManager.LoadContent() - END");
		}

		public static void RecreateControls()
		{
			if (m_screens != null)
			{
				for (int i = 0; i < m_screens.Count; i++)
				{
					m_screens[i].RecreateControls(constructor: false);
				}
			}
		}

		public static void CloseScreen(Type screenType)
		{
			if (m_screens == null)
			{
				return;
			}
			for (int i = 0; i < m_screens.Count; i++)
			{
				if (m_screens[i].GetType() == screenType)
				{
					m_screens[i].CloseScreen();
				}
			}
		}

		public static void CloseScreenNow(Type screenType)
		{
			if (m_screens == null)
			{
				return;
			}
			for (int i = 0; i < m_screens.Count; i++)
			{
				if (m_screens[i].GetType() == screenType)
				{
					m_screens[i].CloseScreenNow();
				}
			}
		}

		/// <summary>
		/// Clears the old focus, this gets around an issue where the input does not always get cleared between frames, causing screens to handle input when they shouldn't.
		/// </summary>
		public static void ClearLastScreenWithFocus()
		{
			m_lastScreenWithFocus = null;
		}

		public static int GetScreensCount()
		{
			return m_screens.Count;
		}

		public static void GetControlsUnderMouseCursor(List<MyGuiControlBase> outControls, bool visibleOnly)
		{
			foreach (MyGuiScreenBase screen in m_screens)
			{
				if (screen.State == MyGuiScreenState.OPENED)
				{
					screen.GetControlsUnderMouseCursor(MyGuiManager.MouseCursorPosition, outControls, visibleOnly);
				}
			}
		}

		public static void UnloadContent()
		{
			foreach (MyGuiScreenBase screen in m_screens)
			{
				if (screen.IsFirstForUnload())
				{
					screen.UnloadContent();
				}
			}
			foreach (MyGuiScreenBase screen2 in m_screens)
			{
				if (!screen2.IsFirstForUnload())
				{
					screen2.UnloadContent();
				}
			}
		}

		public static void AddScreen(MyGuiScreenBase screen)
		{
			CheckThread();
			screen.Closed += delegate(MyGuiScreenBase sender, bool isUnloading)
			{
				RemoveScreen(sender);
			};
			if (MyInput.Static != null)
			{
				MyInput.Static.JoystickAsMouse = screen.JoystickAsMouse;
			}
			m_screensToAdd.Add(screen);
		}

		public static void InsertScreen(MyGuiScreenBase screen, int index)
		{
			CheckThread();
			index = MyUtils.GetClampInt(index, 0, m_screens.Count - 1);
			screen.Closed += delegate(MyGuiScreenBase sender, bool isUnloading)
			{
				RemoveScreen(sender);
			};
			m_screens.Insert(index, screen);
			if (!screen.IsLoaded)
			{
				screen.State = MyGuiScreenState.OPENING;
				screen.LoadData();
				screen.LoadContent();
			}
		}

		public static void AddScreenNow(MyGuiScreenBase screen)
		{
			CheckThread();
			screen.Closed += delegate(MyGuiScreenBase sender, bool isUnloading)
			{
				RemoveScreen(sender);
			};
			GetScreenWithFocus()?.HideTooltips();
			MyGuiScreenBase myGuiScreenBase = ((m_screens.Count <= 0) ? null : m_screens[m_screens.Count - 1]);
			MyGuiScreenBase myGuiScreenBase2 = null;
			if (screen.CanHideOthers)
			{
				myGuiScreenBase2 = GetPreviousScreen(null, (MyGuiScreenBase x) => x.CanBeHidden, (MyGuiScreenBase x) => x.CanHideOthers);
			}
			if (myGuiScreenBase2 != null && myGuiScreenBase2.State != MyGuiScreenState.CLOSING)
			{
				myGuiScreenBase2.HideScreen();
			}
			if (!screen.IsLoaded)
			{
				screen.State = MyGuiScreenState.OPENING;
				screen.LoadData();
				screen.LoadContent();
			}
			if (screen.IsAlwaysFirst())
			{
				m_screens.Insert(0, screen);
			}
			else if (screen.IsTopMostScreen())
			{
				m_screens.Add(screen);
			}
			else
			{
				m_screens.Insert(GetIndexOfLastNonTopScreen(), screen);
			}
			MyGuiScreenBase myGuiScreenBase3 = ((m_screens.Count <= 0) ? null : m_screens[m_screens.Count - 1]);
			NotifyScreenAdded(screen);
			if (myGuiScreenBase != myGuiScreenBase3)
			{
				myGuiScreenBase?.OnScreenOrderChanged(myGuiScreenBase, myGuiScreenBase3);
				myGuiScreenBase3?.OnScreenOrderChanged(myGuiScreenBase, myGuiScreenBase3);
			}
		}

		public static void RemoveScreen(MyGuiScreenBase screen)
		{
			CheckThread();
			if (!IsAnyScreenOpening())
			{
				MyGuiScreenBase previousScreen = GetPreviousScreen(screen, (MyGuiScreenBase x) => x.CanBeHidden, (MyGuiScreenBase x) => x.CanHideOthers);
				if (previousScreen != null && (previousScreen.State == MyGuiScreenState.HIDDEN || previousScreen.State == MyGuiScreenState.HIDING))
				{
					previousScreen.UnhideScreen();
					MyInput.Static.JoystickAsMouse = previousScreen.JoystickAsMouse;
				}
			}
			using (lockObject.AcquireExclusiveUsing())
			{
				m_screensToRemove.Add(screen);
			}
		}

		public static void CheckThread()
		{
<<<<<<< HEAD
			if (UpdateThread == null || UpdateThread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId)
=======
			if (UpdateThread == null || UpdateThread.get_ManagedThreadId() != Thread.get_CurrentThread().get_ManagedThreadId())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyLog.Default.Error($"Thread unsafe access to GUI screens: {new StackTrace(2, fNeedFileInfo: true)}");
			}
		}

		public static MyGuiScreenBase GetTopHiddenScreen()
		{
			MyGuiScreenBase result = null;
			for (int num = GetScreensCount() - 1; num > 0; num--)
			{
				MyGuiScreenBase myGuiScreenBase = m_screens[num];
				if (myGuiScreenBase.State == MyGuiScreenState.HIDDEN || myGuiScreenBase.State == MyGuiScreenState.HIDING)
				{
					result = myGuiScreenBase;
					break;
				}
			}
			return result;
		}

		public static MyGuiScreenBase GetPreviousScreen(MyGuiScreenBase screen, Predicate<MyGuiScreenBase> condition, Predicate<MyGuiScreenBase> terminatingCondition)
		{
			MyGuiScreenBase result = null;
			int num = -1;
			if (screen == null)
			{
				num = GetScreensCount();
			}
			for (int num2 = GetScreensCount() - 1; num2 > 0; num2--)
			{
				MyGuiScreenBase myGuiScreenBase = m_screens[num2];
				if (screen == myGuiScreenBase)
				{
					num = num2;
				}
				if (num2 < num && myGuiScreenBase.State != MyGuiScreenState.CLOSED && myGuiScreenBase.State != MyGuiScreenState.CLOSED && myGuiScreenBase.State != MyGuiScreenState.CLOSING)
				{
					if (condition(myGuiScreenBase))
					{
						result = myGuiScreenBase;
						break;
					}
					if (terminatingCondition(myGuiScreenBase))
					{
						break;
					}
				}
			}
			return result;
		}

		public static void RemoveAllScreensExcept(MyGuiScreenBase dontRemove)
		{
			foreach (MyGuiScreenBase screen in m_screens)
			{
				if (screen != dontRemove)
				{
					RemoveScreen(screen);
				}
			}
		}

		public static void RemoveScreenByType(Type screenType)
		{
			foreach (MyGuiScreenBase screen in m_screens)
			{
				if (screenType.IsAssignableFrom(screen.GetType()))
				{
					RemoveScreen(screen);
				}
			}
		}

		public static void CloseAllScreensExcept(MyGuiScreenBase dontRemove)
		{
			for (int num = m_screens.Count - 1; num >= 0; num--)
			{
				MyGuiScreenBase myGuiScreenBase = m_screens[num];
				if (myGuiScreenBase != dontRemove && myGuiScreenBase.CanCloseInCloseAllScreenCalls())
				{
					myGuiScreenBase.CloseScreen();
				}
			}
		}

		public static void CloseAllScreensNowExcept(MyGuiScreenBase dontRemove, bool isUnloading = false)
		{
			for (int num = m_screens.Count - 1; num >= 0; num--)
			{
				MyGuiScreenBase myGuiScreenBase = m_screens[num];
				if (myGuiScreenBase != dontRemove && myGuiScreenBase.CanCloseInCloseAllScreenCalls())
				{
					myGuiScreenBase.CloseScreenNow(isUnloading);
				}
			}
			foreach (MyGuiScreenBase item in m_screensToAdd)
			{
				item.UnloadContent();
			}
			m_screensToAdd.Clear();
		}

		public static void CloseAllScreensExceptThisOneAndAllTopMost(MyGuiScreenBase dontRemove)
		{
			for (int num = m_screens.Count - 1; num >= 0; num--)
			{
				MyGuiScreenBase myGuiScreenBase = m_screens[num];
				if (myGuiScreenBase != dontRemove && myGuiScreenBase.CanCloseInCloseAllScreenCalls() && !myGuiScreenBase.IsTopMostScreen())
				{
					myGuiScreenBase.CloseScreen();
				}
			}
		}

		public static void CloseNowAllBelow(MyGuiScreenBase targetScreen)
		{
			bool flag = false;
			foreach (MyGuiScreenBase screen in m_screens)
			{
				if (screen == targetScreen)
				{
					flag = true;
				}
				else if (flag)
				{
					screen.CloseScreenNow();
				}
			}
		}

		public static void HandleInput()
		{
			try
			{
				if (m_screens == null || m_screens.Count <= 0)
				{
					return;
				}
				MyGuiScreenBase screenWithFocus = GetScreenWithFocus();
				if (m_inputToNonFocusedScreens)
				{
					bool flag = false;
					for (int num = m_screens.Count - 1; num >= 0; num--)
					{
						if (m_screens.Count > num)
						{
							MyGuiScreenBase myGuiScreenBase = m_screens[num];
							if (myGuiScreenBase != null)
							{
								if (myGuiScreenBase.CanShareInput())
								{
									myGuiScreenBase.HandleInput(m_lastScreenWithFocus != screenWithFocus);
									flag = true;
								}
								else if (!flag && myGuiScreenBase == screenWithFocus)
								{
									myGuiScreenBase.HandleInput(m_lastScreenWithFocus != screenWithFocus);
								}
							}
						}
					}
					m_inputToNonFocusedScreens &= flag;
				}
				else
				{
					foreach (MyGuiScreenBase screen in m_screens)
					{
						if (screen != screenWithFocus)
						{
							screen.InputLost();
						}
					}
					if (screenWithFocus != null)
					{
						switch (screenWithFocus.State)
						{
						case MyGuiScreenState.OPENING:
						case MyGuiScreenState.OPENED:
						case MyGuiScreenState.UNHIDING:
							screenWithFocus.HandleInput(m_lastScreenWithFocus != screenWithFocus);
							break;
						}
					}
				}
				m_lastScreenWithFocus = screenWithFocus;
				if (screenWithFocus != null && screenWithFocus.State == MyGuiScreenState.OPENED && MyVRage.Platform.ImeProcessor != null)
				{
					MyVRage.Platform.ImeProcessor.RecaptureTopScreen(screenWithFocus);
				}
			}
			finally
			{
			}
		}

		public static void HandleInputAfterSimulation()
		{
			for (int num = m_screens.Count - 1; num >= 0; num--)
			{
				m_screens[num].HandleInputAfterSimulation();
			}
		}

		private static bool IsAnyScreenInTransition()
		{
			bool flag = false;
			if (m_screens.Count > 0)
			{
				for (int num = m_screens.Count - 1; num >= 0; num--)
				{
					flag = IsScreenTransitioning(m_screens[num]);
					if (flag)
					{
						break;
					}
				}
			}
			return flag;
		}

		public static bool IsAnyScreenOpening()
		{
			bool flag = false;
			if (m_screens.Count > 0)
			{
				for (int num = m_screens.Count - 1; num >= 0; num--)
				{
					flag = m_screens[num].State == MyGuiScreenState.OPENING;
					if (flag)
					{
						break;
					}
				}
			}
			return flag;
		}

		private static bool IsScreenTransitioning(MyGuiScreenBase screen)
		{
			if (screen.State != MyGuiScreenState.CLOSING && screen.State != 0)
			{
				if (screen.State != MyGuiScreenState.HIDING)
				{
					return screen.State == MyGuiScreenState.UNHIDING;
				}
				return true;
			}
			return true;
		}

		public static bool IsScreenOfTypeOpen(Type screenType)
		{
			foreach (MyGuiScreenBase screen in m_screens)
			{
				if (screen.GetType() == screenType && screen.State == MyGuiScreenState.OPENED)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsScreenOfTypeOpen(string debugNamePath)
		{
			foreach (MyGuiScreenBase screen in m_screens)
			{
				if (screen.DebugNamePath == debugNamePath && screen.State == MyGuiScreenState.OPENED)
				{
					return true;
				}
			}
			return false;
		}

		public static bool ExistsScreenOfType(Type screenType)
		{
			foreach (MyGuiScreenBase screen in m_screens)
			{
				if (screen.GetType() == screenType)
				{
					return true;
				}
			}
			return false;
		}

		public static void Update(int totalTimeInMS)
		{
			TotalGamePlayTimeInMilliseconds = totalTimeInMS;
			RemoveScreens();
			AddScreens();
			RemoveScreens();
			MyGuiScreenBase screenWithFocus = GetScreenWithFocus();
			for (int i = 0; i < m_screens.Count; i++)
			{
				MyGuiScreenBase myGuiScreenBase = m_screens[i];
				myGuiScreenBase.Update(myGuiScreenBase == screenWithFocus);
			}
			if (m_screens.Count > 0 && m_screens[m_screens.Count - 1].State == MyGuiScreenState.HIDDEN)
			{
				m_screens[m_screens.Count - 1].UnhideScreen();
			}
		}

		private static int GetIndexOfLastNonTopScreen()
		{
			int result = 0;
			for (int i = 0; i < m_screens.Count; i++)
			{
				MyGuiScreenBase myGuiScreenBase = m_screens[i];
				if (myGuiScreenBase.IsTopMostScreen() || myGuiScreenBase.IsTopScreen())
				{
					break;
				}
				result = i + 1;
			}
			return result;
		}

		private static void AddScreens()
		{
			MyGuiScreenBase myGuiScreenBase = ((m_screens.Count <= 0) ? null : m_screens[m_screens.Count - 1]);
			for (int i = 0; i < m_screensToAdd.Count; i++)
			{
				MyGuiScreenBase myGuiScreenBase2 = m_screensToAdd[i];
				GetScreenWithFocus()?.HideTooltips();
				MyGuiScreenBase myGuiScreenBase3 = null;
				if (myGuiScreenBase2.CanHideOthers)
				{
					do
					{
						myGuiScreenBase3 = GetPreviousScreen(myGuiScreenBase3, (MyGuiScreenBase x) => x.CanBeHidden, (MyGuiScreenBase x) => x.CanHideOthers);
					}
					while (myGuiScreenBase3 != null && myGuiScreenBase3 != null && myGuiScreenBase3.State == MyGuiScreenState.CLOSED);
				}
				if (myGuiScreenBase3 != null && myGuiScreenBase3.State != MyGuiScreenState.CLOSING)
				{
					myGuiScreenBase3.HideScreen();
				}
				if (!myGuiScreenBase2.IsLoaded)
				{
					myGuiScreenBase2.State = MyGuiScreenState.OPENING;
					myGuiScreenBase2.LoadData();
					myGuiScreenBase2.LoadContent();
				}
				if (myGuiScreenBase2.IsAlwaysFirst())
				{
					m_screens.Insert(0, myGuiScreenBase2);
				}
				else if (myGuiScreenBase2.IsTopMostScreen())
				{
					m_screens.Add(myGuiScreenBase2);
				}
				else
				{
					m_screens.Insert(GetIndexOfLastNonTopScreen(), myGuiScreenBase2);
				}
				NotifyScreenAdded(myGuiScreenBase2);
			}
			m_screensToAdd.Clear();
			MyGuiScreenBase myGuiScreenBase4 = ((m_screens.Count <= 0) ? null : m_screens[m_screens.Count - 1]);
			if (myGuiScreenBase != myGuiScreenBase4)
			{
				myGuiScreenBase?.OnScreenOrderChanged(myGuiScreenBase, myGuiScreenBase4);
				myGuiScreenBase4?.OnScreenOrderChanged(myGuiScreenBase, myGuiScreenBase4);
			}
		}

		public static bool IsScreenOnTop(MyGuiScreenBase screen)
		{
			int num = GetIndexOfLastNonTopScreen() - 1;
			if (num < 0 || num >= m_screens.Count)
			{
				return false;
			}
			if (m_screensToAdd.Count > 0)
			{
				return false;
			}
			return m_screens[num] == screen;
		}

		private static void RemoveScreens()
		{
			MyGuiScreenBase myGuiScreenBase = ((m_screens.Count <= 0) ? null : m_screens[m_screens.Count - 1]);
			using (lockObject.AcquireExclusiveUsing())
			{
				bool flag = false;
				foreach (MyGuiScreenBase item in m_screensToRemove)
				{
					if (item.IsLoaded)
					{
						item.UnloadContent();
						item.UnloadData();
					}
					item.OnRemoved();
					m_screens.Remove(item);
					flag = true;
					for (int num = m_screensToAdd.Count - 1; num >= 0; num--)
					{
						if (m_screensToAdd[num] == item)
						{
							m_screensToAdd.RemoveAt(num);
						}
					}
					NotifyScreenRemoved(item);
				}
				m_screensToRemove.Clear();
				if (flag)
				{
					MyGuiScreenBase screenWithFocus = GetScreenWithFocus();
					if (screenWithFocus != null && (screenWithFocus.State == MyGuiScreenState.HIDDEN || screenWithFocus.State == MyGuiScreenState.HIDING))
					{
						screenWithFocus.UnhideScreen();
					}
				}
			}
			MyGuiScreenBase myGuiScreenBase2 = ((m_screens.Count <= 0) ? null : m_screens[m_screens.Count - 1]);
			if (myGuiScreenBase != myGuiScreenBase2)
			{
				myGuiScreenBase?.OnScreenOrderChanged(myGuiScreenBase, myGuiScreenBase2);
				myGuiScreenBase2?.OnScreenOrderChanged(myGuiScreenBase, myGuiScreenBase2);
			}
		}

		public static MyGuiScreenBase GetScreenWithFocus()
		{
			MyGuiScreenBase result = null;
			if (m_screens != null && m_screens.Count > 0)
			{
				for (int num = m_screens.Count - 1; num >= 0; num--)
				{
					MyGuiScreenBase myGuiScreenBase = m_screens[num];
					if (myGuiScreenBase != null && (myGuiScreenBase.State == MyGuiScreenState.OPENED || IsScreenTransitioning(myGuiScreenBase)) && myGuiScreenBase.CanHaveFocus)
					{
						result = myGuiScreenBase;
						break;
					}
				}
			}
			return result;
		}

		public static void Draw()
		{
			MyGuiScreenBase screenWithFocus = GetScreenWithFocus();
			bool previousCanHideOthers;
			MyGuiScreenBase screenFade = FindFirstFade(screenWithFocus, out previousCanHideOthers);
			PrepareDraw(screenWithFocus, previousCanHideOthers);
			DrawScreens(screenFade);
			DrawTooltips(screenWithFocus);
			MyScreenManager.EndOfDraw?.Invoke();
		}

		private static MyGuiScreenBase FindFirstFade(MyGuiScreenBase screenWithFocus, out bool previousCanHideOthers)
		{
			MyGuiScreenBase result = null;
			previousCanHideOthers = false;
			for (int num = m_screens.Count - 1; num >= 0; num--)
			{
				MyGuiScreenBase myGuiScreenBase = m_screens[num];
				if (myGuiScreenBase == null)
<<<<<<< HEAD
				{
					MyLog.Default.Error("Null screen in list of screens.");
				}
				else
				{
					bool enabledBackgroundFade = myGuiScreenBase.EnabledBackgroundFade;
					bool flag = false;
					if (screenWithFocus == myGuiScreenBase || myGuiScreenBase.GetDrawScreenEvenWithoutFocus() || !previousCanHideOthers)
					{
						if (myGuiScreenBase.State != MyGuiScreenState.CLOSED && enabledBackgroundFade)
						{
							flag = true;
						}
					}
					else if (IsScreenTransitioning(myGuiScreenBase) && enabledBackgroundFade)
					{
						flag = true;
					}
					if (flag)
					{
						result = myGuiScreenBase;
						break;
					}
					previousCanHideOthers = myGuiScreenBase.CanHideOthers;
				}
=======
				{
					MyLog.Default.Error("Null screen in list of screens.");
				}
				bool enabledBackgroundFade = myGuiScreenBase.EnabledBackgroundFade;
				bool flag = false;
				if (screenWithFocus == myGuiScreenBase || myGuiScreenBase.GetDrawScreenEvenWithoutFocus() || !previousCanHideOthers)
				{
					if (myGuiScreenBase.State != MyGuiScreenState.CLOSED && enabledBackgroundFade)
					{
						flag = true;
					}
				}
				else if (IsScreenTransitioning(myGuiScreenBase) && enabledBackgroundFade)
				{
					flag = true;
				}
				if (flag)
				{
					result = myGuiScreenBase;
					break;
				}
				previousCanHideOthers = myGuiScreenBase.CanHideOthers;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return result;
		}

		private static void PrepareDraw(MyGuiScreenBase screenWithFocus, bool previousCanHideOthers)
		{
			for (int i = 0; i < m_screens.Count; i++)
			{
				MyGuiScreenBase myGuiScreenBase = m_screens[i];
				bool flag = false;
				if (screenWithFocus == myGuiScreenBase || myGuiScreenBase.GetDrawScreenEvenWithoutFocus() || !previousCanHideOthers)
				{
					if (myGuiScreenBase.State != MyGuiScreenState.CLOSED && myGuiScreenBase.State != MyGuiScreenState.HIDDEN)
					{
						flag = true;
					}
				}
				else if (!myGuiScreenBase.CanBeHidden)
				{
					flag = true;
				}
				else if (IsScreenTransitioning(myGuiScreenBase))
				{
					flag = true;
				}
				if (flag)
				{
					m_screensToDraw.Add(myGuiScreenBase);
					myGuiScreenBase.PrepareDraw();
<<<<<<< HEAD
				}
			}
		}

		private static void DrawScreens(MyGuiScreenBase screenFade)
		{
			for (int i = 0; i < m_screensToDraw.Count; i++)
			{
				MyGuiScreenBase myGuiScreenBase = m_screensToDraw[i];
				if (myGuiScreenBase == screenFade)
				{
					MyGuiManager.DrawSpriteBatch("Textures\\Gui\\Screens\\screen_background_fade.dds", MyGuiManager.GetFullscreenRectangle(), myGuiScreenBase.BackgroundFadeColor, ignoreBounds: true, waitTillLoaded: true);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				myGuiScreenBase.Draw();
			}
<<<<<<< HEAD
=======
		}

		private static void DrawScreens(MyGuiScreenBase screenFade)
		{
			for (int i = 0; i < m_screensToDraw.Count; i++)
			{
				MyGuiScreenBase myGuiScreenBase = m_screensToDraw[i];
				if (myGuiScreenBase == screenFade)
				{
					MyGuiManager.DrawSpriteBatch("Textures\\Gui\\Screens\\screen_background_fade.dds", MyGuiManager.GetFullscreenRectangle(), myGuiScreenBase.BackgroundFadeColor, ignoreBounds: true, waitTillLoaded: true);
				}
				myGuiScreenBase.Draw();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_screensToDraw.Clear();
		}

		private static void DrawTooltips(MyGuiScreenBase screenWithFocus)
		{
			if (screenWithFocus != null)
			{
				List<MyGuiControlBase> visibleControls = screenWithFocus.Controls.GetVisibleControls();
				for (int num = visibleControls.Count - 1; num >= 0; num--)
				{
					visibleControls[num].ShowToolTip();
				}
			}
		}

		private static void NotifyScreenAdded(MyGuiScreenBase screen)
		{
			if (MyScreenManager.ScreenAdded != null)
			{
				MyScreenManager.ScreenAdded(screen);
			}
		}

		private static void NotifyScreenRemoved(MyGuiScreenBase screen)
		{
			if (MyScreenManager.ScreenRemoved != null)
			{
				MyScreenManager.ScreenRemoved(screen);
			}
		}

		public static StringBuilder GetGuiScreensForDebug()
		{
			m_sb.Clear();
			m_sb.ConcatFormat("{0}{1}{2}", "GUI screens: [", m_screens.Count, "]: ");
			MyGuiScreenBase screenWithFocus = GetScreenWithFocus();
			for (int i = 0; i < m_screens.Count; i++)
			{
				MyGuiScreenBase myGuiScreenBase = m_screens[i];
				if (screenWithFocus == myGuiScreenBase)
				{
					m_sb.Append("[F]");
				}
				m_sb.Append(myGuiScreenBase.GetFriendlyName());
				m_sb.Append((i < m_screens.Count - 1) ? ", " : "");
			}
			return m_sb;
		}

		public static T GetFirstScreenOfType<T>() where T : MyGuiScreenBase
		{
<<<<<<< HEAD
			return m_screens.OfType<T>().FirstOrDefault() ?? m_screensToAdd.OfType<T>().FirstOrDefault();
=======
			return Enumerable.FirstOrDefault<T>(Enumerable.OfType<T>((IEnumerable)m_screens)) ?? Enumerable.FirstOrDefault<T>(Enumerable.OfType<T>((IEnumerable)m_screensToAdd));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static string ValidateText(StringBuilder text)
		{
			return OnValidateText?.Invoke(text);
		}
	}
}
