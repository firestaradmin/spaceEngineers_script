using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlTabControl))]
	public class MyGuiControlTabControl : MyGuiControlParent
	{
		public bool ShowGamepadHelp = true;
<<<<<<< HEAD
=======

		private Dictionary<int, MyGuiControlTabPage> m_pages = new Dictionary<int, MyGuiControlTabPage>();

		private string m_selectedTexture;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private Dictionary<int, MyGuiControlTabPage> m_pages = new Dictionary<int, MyGuiControlTabPage>();

		private int m_selectedPage;

		private Vector2 m_tabButtonSize;

		private float m_tabButtonScale = 1f;

		private bool m_helpDirty = true;

		private bool m_drawHelp = true;

		public int SelectedPage
		{
			get
			{
				return m_selectedPage;
			}
			set
			{
				if (m_pages.ContainsKey(m_selectedPage))
				{
					m_pages[m_selectedPage].Visible = false;
				}
				m_selectedPage = value;
				if (this.OnPageChanged != null)
				{
					this.OnPageChanged();
				}
				if (m_pages.ContainsKey(m_selectedPage))
				{
					m_pages[m_selectedPage].Visible = true;
				}
			}
		}

		public Vector2 TabButtonSize
		{
			get
			{
				return m_tabButtonSize;
			}
			private set
			{
				m_tabButtonSize = value;
			}
		}

		public float TabButtonScale
		{
			get
			{
				return m_tabButtonScale;
			}
			set
			{
				m_tabButtonScale = value;
				RefreshInternals();
			}
		}

		public int PagesCount => m_pages.Count;

		public DictionaryValuesReader<int, MyGuiControlTabPage> Pages => m_pages;

		public Vector2 TabPosition { get; private set; }

		public Vector2 TabSize { get; private set; }

		public Vector2 ButtonsOffset { get; set; }

		public event Action OnPageChanged;

		public MyGuiControlTabControl()
			: this(null, null, null)
		{
		}

		public MyGuiControlTabControl(Vector2? position = null, Vector2? size = null, Vector4? colorMask = null)
			: base(position, size, colorMask)
		{
			RefreshInternals();
		}

		public override void Init(MyObjectBuilder_GuiControlBase builder)
		{
			base.Init(builder);
			_ = (MyObjectBuilder_GuiControlTabControl)builder;
			RecreatePages();
			HideTabs();
			SelectedPage = 0;
		}

		public void RecreatePages()
		{
			foreach (MyGuiControlBase control in base.Controls)
			{
				control.EnabledChanged = (Action<MyGuiControlBase>)Delegate.Remove(control.EnabledChanged, new Action<MyGuiControlBase>(PageEnabled));
			}
			m_pages.Clear();
			foreach (MyGuiControlTabPage control2 in base.Controls)
			{
				control2.Visible = false;
				m_pages.Add(control2.PageKey, control2);
				control2.EnabledChanged = (Action<MyGuiControlBase>)Delegate.Combine(control2.EnabledChanged, new Action<MyGuiControlBase>(PageEnabled));
			}
			m_helpDirty = true;
			RefreshInternals();
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			return (MyObjectBuilder_GuiControlTabControl)base.GetObjectBuilder();
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			MyGuiCompositeTexture tEXTURE_BUTTON_DEFAULT_NORMAL = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL;
			MyGuiCompositeTexture tEXTURE_BUTTON_DEFAULT_HIGHLIGHT = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_HIGHLIGHT;
			_ = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_FOCUS;
			MyGuiCompositeTexture tEXTURE_BUTTON_DEFAULT_ACTIVE = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_ACTIVE;
			_ = m_pages.Count;
			int num = 0;
			Vector2 vector = GetPositionAbsoluteTopLeft() + ButtonsOffset;
			if (ShowGamepadHelp && MyInput.Static.IsJoystickLastUsed)
			{
				DrawGamepadHelp(vector, transitionAlpha);
			}
			foreach (int key in m_pages.Keys)
			{
				MyGuiControlTabPage tabSubControl = GetTabSubControl(key);
				if (!tabSubControl.IsTabVisible)
				{
					continue;
				}
				bool flag = GetMouseOverTab() == key;
				bool flag2 = SelectedPage == key;
				bool flag3 = base.Enabled && tabSubControl.Enabled;
				Color value = MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, flag3, transitionAlpha);
				MyGuiCompositeTexture myGuiCompositeTexture;
				string font;
				if (flag3)
				{
					if (flag)
					{
						myGuiCompositeTexture = tEXTURE_BUTTON_DEFAULT_HIGHLIGHT;
						font = "White";
					}
					else if (flag2)
					{
						myGuiCompositeTexture = tEXTURE_BUTTON_DEFAULT_ACTIVE;
						font = "Blue";
					}
					else
					{
						myGuiCompositeTexture = tEXTURE_BUTTON_DEFAULT_NORMAL;
						font = "Blue";
					}
				}
				else
				{
					myGuiCompositeTexture = tEXTURE_BUTTON_DEFAULT_NORMAL;
					font = "Blue";
<<<<<<< HEAD
				}
				myGuiCompositeTexture.Draw(vector, TabButtonSize, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, flag3, transitionAlpha), m_tabButtonScale);
				StringBuilder text = tabSubControl.Text;
				if (text != null)
				{
					Vector2 normalizedCoord = vector + TabButtonSize / 2f;
					MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
					MyGuiManager.DrawString(font, text.ToString(), normalizedCoord, tabSubControl.TextScale, value, drawAlign);
				}
=======
				}
				myGuiCompositeTexture.Draw(vector, TabButtonSize, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, flag3, transitionAlpha), m_tabButtonScale);
				StringBuilder text = tabSubControl.Text;
				if (text != null)
				{
					Vector2 normalizedCoord = vector + TabButtonSize / 2f;
					MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
					MyGuiManager.DrawString(font, text.ToString(), normalizedCoord, tabSubControl.TextScale, value, drawAlign);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				vector.X += TabButtonSize.X;
				num++;
			}
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
		}

		public override MyGuiControlBase HandleInput()
		{
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT) || (MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.Tab)))
			{
				int selectedPage = SelectedPage;
				int num = selectedPage;
				do
				{
					num = (num + PagesCount - 1) % PagesCount;
				}
				while (!m_pages[num].Enabled && selectedPage != num);
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				if (selectedPage != num)
				{
					SelectedPage = num;
					return this;
				}
				return null;
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT) || (MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyShiftKeyPressed() && MyInput.Static.IsNewKeyPressed(MyKeys.Tab)))
			{
				int selectedPage2 = SelectedPage;
				int num2 = selectedPage2;
				do
				{
					num2 = (num2 + 1) % PagesCount;
				}
				while (!m_pages[num2].Enabled && selectedPage2 != num2);
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				if (selectedPage2 != num2)
				{
					SelectedPage = num2;
					return this;
				}
				return null;
			}
			int mouseOverTab = GetMouseOverTab();
			if (mouseOverTab != -1 && GetTabSubControl(mouseOverTab).Enabled && MyInput.Static.IsNewPrimaryButtonPressed())
			{
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				SelectedPage = mouseOverTab;
				return this;
			}
			return base.HandleInput();
		}

		public override void ShowToolTip()
		{
			foreach (KeyValuePair<int, MyGuiControlTabPage> page in m_pages)
			{
				if (page.Value.IsTabVisible)
				{
					page.Value.ShowToolTip();
				}
			}
			int mouseOverTab = GetMouseOverTab();
			foreach (KeyValuePair<int, MyGuiControlTabPage> page2 in m_pages)
			{
				if (page2.Key == mouseOverTab && page2.Value.m_toolTip != null)
				{
					page2.Value.m_toolTip.Draw(MyGuiManager.MouseCursorPosition);
					return;
				}
			}
			base.ShowToolTip();
		}

		public MyGuiControlTabPage GetTabSubControl(int key)
		{
			if (!m_pages.ContainsKey(key))
			{
				Dictionary<int, MyGuiControlTabPage> pages = m_pages;
				Vector2? position = TabPosition;
				Vector2? size = TabSize;
				Vector4? color = base.ColorMask;
				pages[key] = new MyGuiControlTabPage(key, position, size, color)
				{
					Visible = false,
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
				};
				base.Controls.Add(m_pages[key]);
				m_helpDirty = true;
				MyGuiControlTabPage myGuiControlTabPage = m_pages[key];
				myGuiControlTabPage.EnabledChanged = (Action<MyGuiControlBase>)Delegate.Combine(myGuiControlTabPage.EnabledChanged, new Action<MyGuiControlBase>(PageEnabled));
			}
			return m_pages[key];
		}

		private void PageEnabled(MyGuiControlBase obj)
		{
			m_helpDirty = true;
		}

<<<<<<< HEAD
		/// <summary>
		///
		/// </summary>
		/// <returns>Mouse over tab index or -1 when none of them.</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private int GetMouseOverTab()
		{
			_ = m_pages.Keys.Count;
			int num = 0;
			Vector2 vector = GetPositionAbsoluteTopLeft() + ButtonsOffset;
			foreach (KeyValuePair<int, MyGuiControlTabPage> page in m_pages)
			{
				if (page.Value.IsTabVisible)
				{
					int key = page.Key;
					Vector2 vector2 = vector;
					Vector2 vector3 = vector2 + TabButtonSize;
					if (MyGuiManager.MouseCursorPosition.X >= vector2.X && MyGuiManager.MouseCursorPosition.X <= vector3.X && MyGuiManager.MouseCursorPosition.Y >= vector2.Y && MyGuiManager.MouseCursorPosition.Y <= vector3.Y)
					{
						return key;
					}
					vector.X += TabButtonSize.X;
					num++;
				}
			}
			return -1;
		}

		private void RefreshInternals()
		{
			Vector2 minSizeGui = MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui;
			minSizeGui *= m_tabButtonScale;
			TabButtonSize = new Vector2(Math.Min(base.Size.X / (float)m_pages.Count, minSizeGui.X), minSizeGui.Y);
			TabPosition = base.Size * -0.5f + new Vector2(0f, TabButtonSize.Y);
			TabSize = base.Size - new Vector2(0f, TabButtonSize.Y);
			RefreshPageParameters();
		}

		private void RefreshPageParameters()
		{
			foreach (MyGuiControlTabPage value in m_pages.Values)
			{
				value.Position = TabPosition;
				value.Size = TabSize;
				value.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			}
		}

		private void HideTabs()
		{
			foreach (KeyValuePair<int, MyGuiControlTabPage> page in m_pages)
			{
				page.Value.Visible = false;
			}
		}

		private void RecomputeHelp()
		{
			m_helpDirty = false;
			if (m_pages.Count <= 1)
			{
				m_drawHelp = false;
				return;
			}
			int num = 0;
			foreach (KeyValuePair<int, MyGuiControlTabPage> page in m_pages)
			{
				if (page.Value.Enabled)
				{
					num++;
				}
			}
			m_drawHelp = num > 1;
		}

		private void DrawGamepadHelp(Vector2 topLeft, float transitionAlpha)
		{
			if (m_helpDirty)
			{
				RecomputeHelp();
			}
			if (m_drawHelp)
			{
				MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
				Vector2 normalizedCoord = topLeft;
				normalizedCoord.Y += TabButtonSize.Y / 2f;
				normalizedCoord.X -= TabButtonSize.X / 6f;
				Vector2 normalizedCoord2 = topLeft;
				normalizedCoord2.Y = normalizedCoord.Y;
				normalizedCoord2.X += (float)PagesCount * TabButtonSize.X + TabButtonSize.X / 8f;
				Color value = MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, enabled: true, transitionAlpha);
				MyGuiManager.DrawString("Blue", MyTexts.Get(MyCommonTexts.Gamepad_Help_TabControl_Left).ToString(), normalizedCoord, 1f, value, drawAlign);
				MyGuiManager.DrawString("Blue", MyTexts.Get(MyCommonTexts.Gamepad_Help_TabControl_Right).ToString(), normalizedCoord2, 1f, value, drawAlign);
			}
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshInternals();
		}

		public void MoveToNextTab()
		{
			if (m_pages.Count == 0)
			{
				return;
			}
			int selectedPage = SelectedPage;
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			foreach (KeyValuePair<int, MyGuiControlTabPage> page in m_pages)
			{
				num2 = Math.Min(num2, page.Key);
				if (page.Key > selectedPage && page.Key < num)
				{
					num = page.Key;
				}
			}
			SelectedPage = ((num != int.MaxValue) ? num : num2);
		}

		public void MoveToPreviousTab()
		{
			if (m_pages.Count == 0)
			{
				return;
			}
			int selectedPage = SelectedPage;
			int num = int.MinValue;
			int num2 = int.MinValue;
			foreach (KeyValuePair<int, MyGuiControlTabPage> page in m_pages)
			{
				num2 = Math.Max(num2, page.Key);
				if (page.Key < selectedPage && page.Key > num)
				{
					num = page.Key;
				}
			}
			SelectedPage = ((num != int.MinValue) ? num : num2);
		}

		public override MyGuiControlGridDragAndDrop GetDragAndDropHandlingNow()
		{
			if (m_selectedPage > -1)
			{
				return m_pages[m_selectedPage].GetDragAndDropHandlingNow();
			}
			return null;
		}
	}
}
