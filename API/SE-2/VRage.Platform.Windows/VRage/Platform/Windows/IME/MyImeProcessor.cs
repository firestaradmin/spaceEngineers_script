using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VRage.Collections;
using VRage.Input;
using VRage.Platform.Windows.Render;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Platform.Windows.IME
{
	internal sealed class MyImeProcessor : IMyImeProcessor
	{
		private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

		private struct KeyboardHookStruct
		{
			public int VirtualKeyCode;

			public int ScanCode;

			public int Flags;

			public int Time;

			public int ExtraInfo;
		}

		internal class MyDel
		{
			public IMyImeActiveControl control;

			public Action<IMyImeActiveControl> action;

			public MyDel(IMyImeActiveControl ctrl, Action<IMyImeActiveControl> a)
			{
				control = ctrl;
				action = a;
			}

			public void Invoke()
			{
				action(control);
			}
		}

		private const int WH_KEYBOARD_LL = 13;

		private static IntPtr hookID = IntPtr.Zero;

		private LowLevelKeyboardProc hookProc;

		private static MyImeProcessor instance;

		private bool m_isEnabled;

		private int m_textLimit;

		private int m_charsWritten;

		private bool m_isActive;

		private bool m_isComposing;

		private string m_compositionString = string.Empty;

		private string[] m_candidates;

		private IMyImeActiveControl m_activeTextElement;

		private MyGuiControlIme m_guiControlElement;

		private IVRageGuiScreen m_activeScreen;

		private IMyImeCandidateList m_candidateList;

		private readonly Type m_candidateListType;

		private readonly MyConcurrentQueue<MyDel> m_invokeQueue = new MyConcurrentQueue<MyDel>(32);

		public static MyImeProcessor Instance => instance;

		public bool IsEnabled => m_isEnabled;

		public bool IsActive => m_isActive;

		public bool IsTextElementConnected => m_activeTextElement != null;

		public MyGuiControlIme GuiImeControl
		{
			get
			{
				return m_guiControlElement;
			}
			set
			{
				if (m_isEnabled)
				{
					Deactivate();
				}
				m_guiControlElement = value;
				m_isEnabled = m_guiControlElement != null;
			}
		}

		public bool IsComposing => m_isComposing;

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		private static IntPtr SetHook(LowLevelKeyboardProc proc)
		{
			using (Process process = Process.GetCurrentProcess())
			{
				using (ProcessModule processModule = process.MainModule)
				{
					return SetWindowsHookEx(13, proc, GetModuleHandle(processModule.ModuleName), 0u);
				}
			}
		}

		[DllImport("imm32.dll")]
		public static extern IntPtr ImmGetContext(IntPtr hWnd);

		[DllImport("imm32.dll", EntryPoint = "ImmGetCandidateListW")]
		public static extern int ImmGetCandidateList(IntPtr himc, int deIndex, IntPtr lpCandidateList, int dwBufLen);

		[DllImport("imm32.dll")]
		public static extern int ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

		[DllImport("imm32.dll", CharSet = CharSet.Unicode)]
		public static extern int ImmGetCompositionStringW(IntPtr hIMC, int dwIndex, byte[] lpBuf, int dwBufLen);

		[DllImport("imm32.dll")]
		public static extern bool ImmNotifyIME(IntPtr hIMC, int dwAction, int dwIndex, int dwValue);

		[DllImport("imm32.dll")]
		public static extern int ImmGetCandidateWindow(IntPtr hIMC, int dwIndex, IntPtr lpCandidate);

		[DllImport("imm32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ImmSetCompositionWindow(IntPtr hIMC, ref CompositionForm form);

		[DllImport("imm32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ImmSetCandidateWindow(IntPtr hIMC, ref CANDIDATEFORM form);

		public static void CreateInstance(Type candidateListType)
		{
			instance = new MyImeProcessor(candidateListType);
		}

		public void Deactivate()
		{
			if (!m_isEnabled)
			{
				return;
			}
			if (m_activeScreen != null && m_candidateList.IsGuiControlEqual(m_activeScreen.FocusedControl))
			{
				m_activeScreen.FocusedControl = m_activeTextElement as IVRageGuiControl;
				return;
			}
			RejectComposition();
			m_isComposing = false;
			if (m_activeTextElement != null)
			{
				InvokeDeactivation();
			}
			m_isActive = false;
			m_activeTextElement = null;
			if (m_isEnabled)
			{
				m_guiControlElement.DeactivateIme();
			}
			m_candidateList.Deactivate();
		}

		public void Activate(IMyImeActiveControl textElement)
		{
			if (m_isEnabled)
			{
				InitializeGui();
				m_activeTextElement = textElement;
				m_textLimit = Math.Max(0, m_activeTextElement.GetMaxLength() - m_activeTextElement.GetTextLength() + m_activeTextElement.GetSelectionLength());
				m_charsWritten = 0;
				m_isActive = true;
				m_guiControlElement.ActivateIme();
			}
		}

		public void StopComposing()
		{
			EvtCompositionEnd();
		}

		private static bool SetCompositionWindow(IntPtr hwnd)
		{
			IntPtr hIMC = ImmGetContext(hwnd);
			CompositionForm form = new CompositionForm
			{
				dwStyle = 32,
				ptCurrentPos = 
				{
					x = 15000,
					y = 0
				},
				rcArea = 
				{
					left = 0,
					right = 0,
					top = 0,
					bottom = 0
				}
			};
			return ImmSetCompositionWindow(hIMC, ref form);
		}

		private string[] GetAllCandidateList(IntPtr hwnd)
		{
			string[] array = null;
			_ = string.Empty;
			try
			{
				IntPtr intPtr = ImmGetContext(hwnd);
				if (intPtr != IntPtr.Zero)
				{
					new CANDIDATELIST();
					IntPtr zero = IntPtr.Zero;
					int num = ImmGetCandidateList(intPtr, 0, zero, 0);
					if (num > 28)
					{
						CANDIDATELIST cANDIDATELIST = new CANDIDATELIST();
						IntPtr intPtr2 = Marshal.AllocHGlobal(num);
						ImmGetCandidateList(intPtr, 0, intPtr2, num);
						Marshal.PtrToStructure(intPtr2, cANDIDATELIST);
						byte[] array2 = new byte[num];
						Marshal.Copy(intPtr2, array2, 0, num);
						Marshal.FreeHGlobal(intPtr2);
						int dwOffset = cANDIDATELIST.dwOffset;
						string @string = Encoding.Unicode.GetString(array2, dwOffset, array2.Length - dwOffset - 2);
						char[] separator = "\0".ToCharArray();
						array = @string.Split(separator);
						ImmReleaseContext(hwnd, intPtr);
						if (!string.IsNullOrEmpty(@string))
						{
							return array;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return Array.Empty<string>();
		}

		private string CurrentCompStr(IntPtr handle)
		{
			IntPtr hIMC = ImmGetContext(handle);
			try
			{
				int dwIndex = 8;
				int num = ImmGetCompositionStringW(hIMC, dwIndex, null, 0);
				if (num > 0)
				{
					byte[] array = new byte[num];
					ImmGetCompositionStringW(hIMC, dwIndex, array, num);
					return Encoding.Unicode.GetString(array);
				}
				return string.Empty;
			}
			finally
			{
				ImmReleaseContext(handle, hIMC);
			}
		}

		private MyImeProcessor(Type candidateListType)
		{
			m_candidateListType = candidateListType;
			hookProc = HookKeyboardCallback;
			hookID = SetHook(hookProc);
		}

		private void InitializeGui()
		{
			if (m_candidateList == null)
			{
				m_candidateList = Activator.CreateInstance(m_candidateListType) as IMyImeCandidateList;
				if (m_candidateList != null)
				{
					m_candidateList.ItemClicked += CandidateClicked;
					m_candidateList.CreateNewContextMenu();
				}
			}
		}

		private void InvokeDeactivation()
		{
			if (m_activeTextElement != null)
			{
				QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
				{
					x.DeactivateIme();
				}));
			}
		}

		private void InvokeCharacter(bool compositionEnd, char character, bool check = false)
		{
			if (m_activeTextElement == null)
			{
				return;
			}
			if (!check)
			{
				QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
				{
					x.InsertChar(compositionEnd, character);
				}));
			}
			else if (m_charsWritten < m_textLimit)
			{
				QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
				{
					x.InsertChar(compositionEnd, character);
				}));
				m_charsWritten++;
			}
		}

		private void InvokeCharacterMultiple(bool compositionEnd, string chars, bool check = false)
		{
			if (m_activeTextElement == null || string.IsNullOrEmpty(chars))
			{
				return;
			}
			if (!check)
			{
				QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
				{
					x.InsertCharMultiple(compositionEnd, chars);
				}));
				return;
			}
			if (m_charsWritten + chars.Length <= m_textLimit)
			{
				QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
				{
					x.InsertCharMultiple(compositionEnd, chars);
				}));
				m_charsWritten += chars.Length;
				return;
			}
			string str = chars.Substring(0, m_textLimit - m_charsWritten);
			QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
			{
				x.InsertCharMultiple(compositionEnd, str);
			}));
			m_charsWritten += str.Length;
		}

		private void InvokeBackspace(bool compositionEnd, bool check = false)
		{
			if (m_activeTextElement == null)
			{
				return;
			}
			if (!check)
			{
				QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
				{
					x.KeypressBackspace(compositionEnd);
				}));
			}
			else if (m_charsWritten > 0)
			{
				QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
				{
					x.KeypressBackspace(compositionEnd);
				}));
			}
		}

		private void InvokeBackspaceMultiple(bool compositionEnd, int count, bool check = false)
		{
			if (m_activeTextElement == null || count <= 0)
			{
				return;
			}
			if (!check)
			{
				QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
				{
					x.KeypressBackspaceMultiple(compositionEnd, count);
				}));
				return;
			}
			int min = Math.Min(m_charsWritten, count);
			QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
			{
				x.KeypressBackspaceMultiple(compositionEnd, min);
			}));
			m_charsWritten -= min;
		}

		private void InvokeEnter(bool compositionEnd)
		{
			if (m_activeTextElement != null)
			{
				QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
				{
					x.KeypressEnter(compositionEnd);
				}));
			}
		}

		private void InvokeDelete(bool compositionEnd, char character, bool check = false)
		{
			if (m_activeTextElement != null)
			{
				QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
				{
					x.KeypressDelete(compositionEnd);
				}));
			}
		}

		private void InvokeRedo()
		{
			if (m_activeTextElement != null)
			{
				QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
				{
					x.KeypressRedo();
				}));
			}
		}

		private void InvokeUndo()
		{
			if (m_activeTextElement != null)
			{
				QueueInvoke(new MyDel(m_activeTextElement, delegate(IMyImeActiveControl x)
				{
					x.KeypressUndo();
				}));
			}
		}

		private IntPtr HookKeyboardCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0 && wParam == (IntPtr)256)
			{
				int num = Marshal.ReadInt32(lParam);
				if (num == 27 && IsComposing)
				{
					MyInput.Static.NegateEscapePress();
				}
			}
			return CallNextHookEx(hookID, nCode, wParam, lParam);
		}

		public bool WndProc(ref Message m)
		{
			switch (m.Msg)
			{
			case 258:
				if (m_activeTextElement == null)
				{
					break;
				}
				switch ((ushort)(int)m.WParam)
				{
				case 8:
					InvokeBackspace(compositionEnd: true);
					break;
				case 13:
					InvokeEnter(compositionEnd: true);
					break;
				case 25:
					InvokeRedo();
					break;
				case 26:
					InvokeUndo();
					break;
				default:
					if (!MyInput.Static.IsAnyCtrlKeyPressed())
					{
						InvokeCharacter(compositionEnd: true, (char)(int)m.WParam);
					}
					break;
				case 27:
					break;
				}
				return true;
			case 81:
				LanguageChanged();
				break;
			case 269:
				EvtCompositionStart();
				return false;
			case 271:
				EvtComposition();
				return true;
			case 270:
				EvtCompositionEnd();
				return false;
			case 641:
			case 643:
			case 644:
			case 645:
			case 648:
			case 656:
			case 657:
				return false;
			case 642:
			case 646:
				return true;
			case 7:
				MyPlatformRender.HandleFocusMessage(MyWindowFocusMessage.SetFocus);
				break;
			}
			return true;
		}

		public void LanguageChanged()
		{
			if (m_isComposing)
			{
				StopComposing();
				m_guiControlElement.DeactivateIme();
				m_guiControlElement.ActivateIme();
			}
		}

		private void EvtCompositionStart()
		{
			m_isComposing = true;
			if (m_isEnabled)
			{
				SetCompositionWindow(m_guiControlElement.Handle);
			}
			AddCandidListToActiveScreen();
			if (m_activeTextElement != null)
			{
				m_activeTextElement.IsImeActive = true;
				m_textLimit = Math.Max(0, m_activeTextElement.GetMaxLength() - m_activeTextElement.GetTextLength() + m_activeTextElement.GetSelectionLength());
				m_charsWritten = 0;
			}
			else
			{
				m_textLimit = (m_charsWritten = 0);
			}
			if (m_textLimit == 0)
			{
				EvtCompositionEnd();
				m_guiControlElement.DeactivateIme();
				m_guiControlElement.ActivateIme();
			}
		}

		private void EvtCompositionEnd()
		{
			RejectComposition();
			m_isComposing = false;
			if (m_activeTextElement != null)
			{
				InvokeDeactivation();
			}
			m_candidateList.Deactivate();
		}

		private void EvtComposition()
		{
			if (m_isComposing)
			{
				UpdateCandidateList();
			}
		}

		private void CandidateClicked(IMyImeCandidateList sender, int itemIndex)
		{
			RejectComposition(check: true);
			string text = m_candidates[itemIndex];
			InvokeCharacterMultiple(compositionEnd: true, text.Substring(0, text.Length - 1), check: true);
			if (text[text.Length - 1] != ' ')
			{
				InvokeCharacter(compositionEnd: true, text[text.Length - 1], check: true);
			}
			if (m_isEnabled)
			{
				m_guiControlElement.DeactivateIme();
				m_guiControlElement.ActivateIme();
			}
		}

		private void UpdateCandidateList()
		{
			IntPtr intPtr = IntPtr.Zero;
			if (m_isEnabled)
			{
				intPtr = m_guiControlElement.Handle;
			}
			if (!(intPtr != IntPtr.Zero))
			{
				return;
			}
			int length = m_compositionString.Length;
			if (m_activeTextElement != null)
			{
				string text = CurrentCompStr(intPtr);
				InvokeBackspaceMultiple(compositionEnd: false, m_compositionString.Length, check: true);
				if (!string.IsNullOrEmpty(text))
				{
					InvokeCharacterMultiple(compositionEnd: false, text, check: true);
				}
				m_compositionString = text;
			}
			string[] array = (m_candidates = GetAllCandidateList(intPtr));
			m_candidateList.Clear();
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length > num)
				{
					num = array[i].Length;
				}
				m_candidateList.AddItem(new StringBuilder().AppendFormat("{0}. {1}", i + 1, array[i]));
			}
			Vector2 newPosition;
			Vector2 carriagePosition;
			if (m_activeTextElement != null)
			{
				newPosition = m_activeTextElement.GetCornerPosition();
				carriagePosition = m_activeTextElement.GetCarriagePosition(length);
			}
			else
			{
				newPosition = new Vector2(0f, 0f);
				carriagePosition = new Vector2(0f, 0f);
			}
			ModifyCandidateListVisuals(array.Length, num, newPosition, carriagePosition);
			ModifyVisibility();
		}

		private void ModifyVisibility()
		{
			if (m_candidates.Length == 0)
			{
				m_candidateList.Deactivate();
			}
			else
			{
				m_candidateList.Activate(autoPositionOnMouseTip: false);
			}
		}

		private void ModifyCandidateListVisuals(int wordCount, int maxWordLength, Vector2 newPosition, Vector2 carriagePosition)
		{
			m_candidateList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			Vector2 vector = new Vector2(newPosition.X + carriagePosition.X, newPosition.Y + carriagePosition.Y);
			Vector2 listBoxSize = m_candidateList.GetListBoxSize();
			Vector2 vector2 = vector + listBoxSize;
			if (vector2.X > 1f)
			{
				vector.X = 1f - listBoxSize.X;
			}
			vector.X = Math.Max(0f, vector.X);
			if (vector2.Y > 1f && listBoxSize.Y < 0.5f)
			{
				vector.Y -= listBoxSize.Y + 0.03f;
			}
			m_candidateList.Position = vector;
		}

		public void CaretRepositionReaction()
		{
			ConfirmComposition();
			if (m_isComposing && m_isEnabled)
			{
				m_guiControlElement.DeactivateIme();
				m_guiControlElement.ActivateIme();
			}
		}

		public void UnregisterActiveScreen(IVRageGuiScreen screen)
		{
			if (m_activeScreen != null && m_activeScreen == screen)
			{
				if (m_activeScreen != null)
				{
					m_activeScreen.RemoveControl(m_candidateList);
				}
				m_activeScreen = null;
				Deactivate();
			}
		}

		public void RegisterActiveScreen(IVRageGuiScreen screen)
		{
			if (m_activeScreen != screen)
			{
				if (m_activeScreen != null)
				{
					UnregisterActiveScreen(m_activeScreen);
				}
				m_activeScreen = screen;
				AddCandidListToActiveScreen();
			}
		}

		public void RecaptureTopScreen(IVRageGuiScreen screenWithFocus)
		{
			if ((m_activeScreen == null || !m_activeScreen.IsOpened) && screenWithFocus != null)
			{
				RegisterActiveScreen(screenWithFocus);
			}
		}

		private void AddCandidListToActiveScreen()
		{
			InitializeGui();
			if (m_activeScreen != null && !m_activeScreen.ContainsControl(m_candidateList))
			{
				m_activeScreen.AddControl(m_candidateList);
				m_candidateList.Deactivate();
			}
		}

		private void RemoveCandidListFromActiveScreen()
		{
			m_isComposing = false;
			m_isActive = false;
			m_activeTextElement = null;
			m_textLimit = 0;
			m_charsWritten = 0;
			if (m_isEnabled)
			{
				m_guiControlElement.DeactivateIme();
			}
			if (m_activeScreen != null)
			{
				m_activeScreen.RemoveControl(m_candidateList);
			}
			m_candidateList.Deactivate();
		}

		private void RejectComposition(bool check = false)
		{
			if (m_activeTextElement != null)
			{
				InvokeBackspaceMultiple(compositionEnd: false, m_compositionString.Length, check);
			}
			m_compositionString = string.Empty;
		}

		private void ConfirmComposition()
		{
			m_compositionString = string.Empty;
		}

		private void QueueInvoke(MyDel del)
		{
			lock (m_invokeQueue)
			{
				m_invokeQueue.Enqueue(del);
			}
		}

		public void ProcessInvoke()
		{
			MyDel myDel;
			while (m_invokeQueue.TryDequeue(out myDel))
			{
				myDel.Invoke();
			}
		}
	}
}
