using System;
using System.Collections.Generic;
using System.Text;
using Sandbox;
using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.GUI
{
	internal class MyGuiScreenPerformanceWarnings : MyGuiScreenBase
	{
		/// <summary>
		/// Each of the performance problems on the screen
		/// </summary>
		internal class WarningLine
		{
			public MySimpleProfiler.PerformanceWarning Warning;

			private MyGuiControlLabel m_name;

			private MyGuiControlMultilineText m_description;

			public MyGuiControlParent Parent;

			private MyGuiControlSeparatorList m_separator;

			private MyGuiControlLabel m_time;

			public string Name
			{
				get
				{
					if (m_name != null)
					{
						return m_name.Text;
					}
					return string.Empty;
				}
			}

			public string Time
			{
				get
				{
					if (m_time != null)
					{
						return m_time.Text;
					}
					return string.Empty;
				}
			}

			public WarningLine(MySimpleProfiler.PerformanceWarning warning, MyGuiScreenPerformanceWarnings screen)
			{
				Parent = new MyGuiControlParent();
				string displayName = warning.Block.DisplayName;
				string text = displayName;
				m_name = new MyGuiControlLabel(new Vector2(-0.33f, 0f), null, text, null, 0.8f, "Red");
				if (m_name.Size.X > 0.14f)
				{
					m_name.Text = Truncate(displayName, 15, "..");
				}
				m_name.SetToolTip(displayName);
				m_description = new MyGuiControlMultilineText();
				m_description.Position = new Vector2(-0.18f, 0f);
				m_description.Size = new Vector2(0.45f, 0.2f);
				if (MyPlatformGameSettings.ENABLE_TRASH_REMOVAL_SETTING || string.IsNullOrEmpty(warning.Block.DescriptionSimple.String) || !MyTexts.Exists(warning.Block.DescriptionSimple))
				{
					m_description.Text = new StringBuilder(string.IsNullOrEmpty(warning.Block.Description.String) ? "" : MyTexts.GetString(warning.Block.Description));
				}
				else
				{
					m_description.Text = MyTexts.Get(warning.Block.DescriptionSimple);
				}
				m_description.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_description.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_description.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_description.Size = new Vector2(0.45f, m_description.TextSize.Y);
				Parent.Size = new Vector2(Parent.Size.X, m_description.Size.Y);
				m_separator = new MyGuiControlSeparatorList();
				m_separator.AddVertical(new Vector2(-0.19f, (0f - Parent.Size.Y) / 2f - 0.006f), Parent.Size.Y + 0.016f);
				m_separator.AddVertical(new Vector2(0.26f, (0f - Parent.Size.Y) / 2f - 0.006f), Parent.Size.Y + 0.016f);
				m_time = new MyGuiControlLabel(new Vector2(0.33f, 0f), null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
				switch (warning.Block.Type)
				{
				case MySimpleProfiler.ProfilingBlockType.GPU:
				case MySimpleProfiler.ProfilingBlockType.RENDER:
					screen.m_areaTitleGraphics.Warnings.Add(this);
					break;
				case MySimpleProfiler.ProfilingBlockType.BLOCK:
					screen.m_areaTitleBlocks.Warnings.Add(this);
					break;
				case MySimpleProfiler.ProfilingBlockType.MOD:
				case MySimpleProfiler.ProfilingBlockType.OTHER:
					screen.m_areaTitleOther.Warnings.Add(this);
					break;
				}
				Warning = warning;
			}

			public WarningLine(string name, string description, DateTime? time = null)
			{
				Parent = new MyGuiControlParent();
				m_name = new MyGuiControlLabel(new Vector2(-0.33f, 0f), null, name, null, 0.8f, "Red");
				if (m_name.Size.X > 0.14f)
				{
					m_name.Text = Truncate(name, 15, "..");
				}
				m_name.SetToolTip(name);
				m_name.ShowTooltipWhenDisabled = true;
				m_description = new MyGuiControlMultilineText();
				m_description.Position = new Vector2(-0.18f, 0f);
				m_description.Size = new Vector2(0.45f, 0.2f);
				m_description.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_description.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_description.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				MyWikiMarkupParser.ParseText(description, ref m_description);
				m_description.Size = new Vector2(0.45f, m_description.TextSize.Y);
				MyWikiMarkupParser.ParseText(description, ref m_description);
				m_description.OnLinkClicked += OnLinkClicked;
				m_separator = new MyGuiControlSeparatorList();
				Parent.Size = new Vector2(Parent.Size.X, m_description.Size.Y);
				m_separator.AddVertical(new Vector2(-0.19f, (0f - Parent.Size.Y) / 2f - 0.006f), Parent.Size.Y + 0.016f);
				m_separator.AddVertical(new Vector2(0.35f, (0f - Parent.Size.Y) / 2f - 0.006f), Parent.Size.Y + 0.016f);
				m_time = new MyGuiControlLabel(new Vector2(0.33f, 0f), null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
				if (time.HasValue)
				{
					m_separator.AddVertical(new Vector2(0.26f, (0f - Parent.Size.Y) / 2f - 0.006f), Parent.Size.Y + 0.016f);
					TimeSpan timeSpan = DateTime.Now - time.Value;
					m_time.Text = $"{timeSpan.Hours}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
				}
			}

			public void Prepare()
			{
				Parent.Position = Vector2.Zero;
				if (Warning != null)
				{
					TimeSpan timeSpan = TimeSpan.FromSeconds((int)((float)Warning.Time * 0.0166666675f));
					m_time.Text = $"{timeSpan.Hours}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
				}
				if (Parent.Controls.Count == 0)
				{
					Parent.Controls.Add(m_name);
					Parent.Controls.Add(m_description);
					Parent.Controls.Add(m_separator);
					Parent.Controls.Add(m_time);
				}
			}

			private string Truncate(string input, int maxLenght, string tooLongSuffix)
			{
				if (input.Length < maxLenght)
				{
					return input;
				}
				return input.Substring(0, maxLenght - tooLongSuffix.Length) + tooLongSuffix;
			}

			private void OnLinkClicked(MyGuiControlBase sender, string url)
			{
				MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm);
			}
		}

		/// <summary>
		/// Used to contain each of the areas (graphics, blocks, other). Also holds the headings.
		/// </summary>
		internal class WarningArea
		{
			internal List<WarningLine> Warnings;

			private MyGuiControlParent m_header;

			private MyGuiControlPanel m_titleBackground;

			private MyGuiControlLabel m_title;

			private MyGuiControlLabel m_lastOccurence;

			private MyGuiControlSeparatorList m_separator;

			private MyGuiControlButton m_refButton;

			public MyGuiControlButton Button => m_refButton;

			public WarningArea(MyStringId name, MySessionComponentWarningSystem.Category areaType, bool refButton, bool unsafeGrid, bool serverMessage)
			{
				Warnings = new List<WarningLine>();
				m_header = new MyGuiControlParent();
				m_titleBackground = new MyGuiControlPanel(null, null, null, "Textures\\GUI\\Controls\\item_highlight_dark.dds");
				m_title = new MyGuiControlLabel(null, null, MyTexts.GetString(name));
				m_separator = new MyGuiControlSeparatorList();
				m_separator.AddHorizontal(new Vector2(-0.45f, 0.018f), 0.9f);
				m_title.Position = new Vector2(-0.33f, 0f);
				m_titleBackground.Size = new Vector2(m_titleBackground.Size.X, 0.035f);
				m_header.Size = new Vector2(m_header.Size.X, m_titleBackground.Size.Y);
				if (!unsafeGrid)
				{
					m_lastOccurence = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.PerformanceWarningLastOccurrence), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
					m_lastOccurence.Position = new Vector2(0.33f, 0f);
				}
				if (!refButton || MySession.Static == null)
				{
					return;
				}
				switch (areaType)
				{
				case MySessionComponentWarningSystem.Category.Graphics:
					if (!MyPlatformGameSettings.LIMITED_MAIN_MENU)
					{
						m_refButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenCaptionGraphicsOptions), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, delegate
						{
							MyGuiSandbox.AddScreen(new MyGuiScreenOptionsGraphics());
						});
					}
					break;
				case MySessionComponentWarningSystem.Category.Blocks:
<<<<<<< HEAD
					if (!MyPlatformGameSettings.ENABLE_TRASH_REMOVAL_SETTING && !MyFakes.FORCE_ADD_TRASH_REMOVAL_MENU)
					{
						break;
					}
					m_refButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Cleanup), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, delegate
					{
						if (MySession.Static == null || MySession.Static.Name != "Skins scene")
						{
							MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
						}
					});
=======
					if (MyPlatformGameSettings.ENABLE_TRASH_REMOVAL_SETTING || MyFakes.FORCE_ADD_TRASH_REMOVAL_MENU)
					{
						m_refButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_Cleanup), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, delegate
						{
							MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
						});
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					break;
				case MySessionComponentWarningSystem.Category.Other:
					m_refButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.ToolbarButton, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenCaptionGraphicsOptions), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, delegate
					{
						MyGuiSandbox.AddScreen(new MyGuiScreenOptionsGame());
					});
					break;
				}
			}

			/// <summary>
			/// Add this area into a list
			/// </summary>
			public void Add(MyGuiControlList list, bool showAll)
			{
				m_header.Position = Vector2.Zero;
				if (m_header.Controls.Count == 0)
				{
					m_header.Controls.Add(m_titleBackground);
					m_header.Controls.Add(m_title);
					m_header.Controls.Add(m_separator);
					if (m_lastOccurence != null)
					{
						m_header.Controls.Add(m_lastOccurence);
					}
				}
				bool flag = false;
				Warnings.Sort(delegate(WarningLine x, WarningLine y)
				{
					if (x.Warning == null && y.Warning == null)
					{
						return string.Compare(x.Name, y.Name);
					}
					if (x.Warning == null)
					{
						return -y.Warning.Time;
					}
					if (y.Warning == null)
					{
						return x.Warning.Time;
					}
					return (y.Warning.Time == 0 && x.Warning.Time == 0) ? string.Compare(x.Name, y.Name) : (x.Warning.Time - y.Warning.Time);
				});
				foreach (WarningLine warning in Warnings)
				{
					if (warning.Warning == null || (float)warning.Warning.Time < 300f || showAll)
					{
						if (!flag)
						{
							list.Controls.Add(m_header);
							flag = true;
						}
						warning.Prepare();
						list.Controls.Add(warning.Parent);
					}
				}
				if (flag)
				{
					if (m_refButton != null)
					{
						list.Controls.Add(m_refButton);
					}
					MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
					myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.45f, 0f), 0.9f);
					myGuiControlSeparatorList.Size = new Vector2(1f, 0.005f);
					myGuiControlSeparatorList.ColorMask = new Vector4(0f, 0f, 0f, 0f);
					list.Controls.Add(myGuiControlSeparatorList);
				}
			}
		}

		private MyGuiControlList m_warningsList;

		private MyGuiControlCheckbox m_showWarningsCheckBox;

		private MyGuiControlCheckbox m_showAllCheckBox;

		private MyGuiControlCheckbox m_showAllBlockLimitsCheckBox;

		private MyGuiControlButton m_okButton;

		private Dictionary<MyStringId, WarningLine> m_warningLines = new Dictionary<MyStringId, WarningLine>();

		internal WarningArea m_areaTitleGraphics = new WarningArea(MyCommonTexts.PerformanceWarningIssuesGraphics, MySessionComponentWarningSystem.Category.Graphics, refButton: true, unsafeGrid: false, serverMessage: false);

		internal WarningArea m_areaTitleBlocks = new WarningArea(MyCommonTexts.PerformanceWarningIssuesBlocks, MySessionComponentWarningSystem.Category.Blocks, refButton: true, unsafeGrid: false, serverMessage: false);

		internal WarningArea m_areaTitleOther = new WarningArea(MyCommonTexts.PerformanceWarningIssuesOther, MySessionComponentWarningSystem.Category.Other, refButton: false, unsafeGrid: false, serverMessage: false);

		internal WarningArea m_areaTitleUnsafeGrids = new WarningArea(MyCommonTexts.PerformanceWarningIssuesUnsafeGrids, MySessionComponentWarningSystem.Category.UnsafeGrids, refButton: false, unsafeGrid: true, serverMessage: false);

		internal WarningArea m_areaTitleBlockLimits = new WarningArea(MyCommonTexts.PerformanceWarningIssuesBlockBuildingLimits, MySessionComponentWarningSystem.Category.BlockLimits, refButton: false, unsafeGrid: true, serverMessage: false);

		internal WarningArea m_areaTitleServer = new WarningArea(MyCommonTexts.PerformanceWarningIssuesServer, MySessionComponentWarningSystem.Category.Server, refButton: false, unsafeGrid: false, serverMessage: true);

		internal WarningArea m_areaTitlePerformance = new WarningArea(MyCommonTexts.PerformanceWarningIssues, MySessionComponentWarningSystem.Category.Performance, refButton: false, unsafeGrid: false, serverMessage: false);

		internal WarningArea m_areaTitleGeneral = new WarningArea(MyCommonTexts.PerformanceWarningIssuesGeneral, MySessionComponentWarningSystem.Category.General, refButton: false, unsafeGrid: false, serverMessage: false);

		private int m_refreshFrameCounter = 120;

		private static bool m_showAll;

		private static bool m_showAllBlockLimits;

		private MyGuiControlMultilineText m_screenDescText;

		private MySessionComponentWarningSystem.Category? m_storedButtonCategory;

		public MyGuiScreenPerformanceWarnings()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.8436f, 0.97f))
		{
			base.EnabledBackgroundFade = true;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenPerformanceWarnings";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyTexts.GetString(MyCommonTexts.PerformanceWarningHelpHeader), null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.87f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.87f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.87f / 2f, m_size.Value.Y / 2f - 0.847f), m_size.Value.X * 0.87f);
			Controls.Add(myGuiControlSeparatorList);
			m_warningsList = new MyGuiControlList(new Vector2(0f, -0.05f), new Vector2(0.731f, 0.685f));
			string @string = MyTexts.GetString(MyCommonTexts.ScreenOptionsGame_EnablePerformanceWarnings);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(-0.365f, 0.329f), null, @string);
			m_showWarningsCheckBox = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipGameOptionsEnablePerformanceWarnings), position: new Vector2(myGuiControlLabel.Position.X + myGuiControlLabel.Size.X + 0.01f, 0.329f), color: null, isChecked: false, visualStyle: MyGuiControlCheckboxStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_showWarningsCheckBox.IsChecked = MySandboxGame.Config.EnablePerformanceWarnings;
			MyGuiControlCheckbox showWarningsCheckBox = m_showWarningsCheckBox;
			showWarningsCheckBox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(showWarningsCheckBox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(ShowWarningsChanged));
			@string = MyTexts.GetString(MyCommonTexts.PerformanceWarningShowAll);
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2(m_showWarningsCheckBox.PositionX + 0.04f, 0.329f), null, @string);
			m_showAllCheckBox = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipPerformanceWarningShowAll), position: new Vector2(myGuiControlLabel2.Position.X + myGuiControlLabel2.Size.X + 0.01f, 0.329f), color: null, isChecked: false, visualStyle: MyGuiControlCheckboxStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_showAllCheckBox.IsChecked = m_showAll;
			MyGuiControlCheckbox showAllCheckBox = m_showAllCheckBox;
			showAllCheckBox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(showAllCheckBox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(KeepInListChanged));
			@string = MyTexts.GetString(MyCommonTexts.PerformanceWarningShowAllBlockLimits);
			MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2(m_showAllCheckBox.PositionX + 0.04f, 0.329f), null, @string);
			m_showAllBlockLimitsCheckBox = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MyCommonTexts.ToolTipPerformanceWarningShowAllBlockLimits), position: new Vector2(myGuiControlLabel3.Position.X + myGuiControlLabel3.Size.X + 0.01f, 0.329f), color: null, isChecked: false, visualStyle: MyGuiControlCheckboxStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_showAllBlockLimitsCheckBox.IsChecked = m_showAllBlockLimits;
			MyGuiControlCheckbox showAllBlockLimitsCheckBox = m_showAllBlockLimitsCheckBox;
			showAllBlockLimitsCheckBox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(showAllBlockLimitsCheckBox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(KeepInListChangedBlockLimits));
			m_screenDescText = new MyGuiControlMultilineText(contents: new StringBuilder(MyTexts.GetString(MyCommonTexts.PerformanceWarningInfoText)), position: new Vector2(-0.365f, 0.381f), size: new Vector2(0.4f, 0.2f));
			m_screenDescText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_screenDescText.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_screenDescText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_okButton = new MyGuiControlButton(new Vector2(0.281f, 0.415f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Close), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Close));
			m_okButton.ButtonClicked += OnOkButtonClicked;
			Controls.Add(m_warningsList);
			Controls.Add(myGuiControlLabel);
			Controls.Add(m_showWarningsCheckBox);
			Controls.Add(myGuiControlLabel2);
			Controls.Add(m_showAllCheckBox);
			Controls.Add(myGuiControlLabel3);
			Controls.Add(m_showAllBlockLimitsCheckBox);
			Controls.Add(m_screenDescText);
			Controls.Add(m_okButton);
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(new Vector2(m_screenDescText.PositionX, m_okButton.Position.Y - minSizeGui.Y / 2f));
			myGuiControlLabel4.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel4);
			base.GamepadHelpTextId = MySpaceTexts.PerformanceWarnings_Help_Screen;
			base.FocusedControl = m_showWarningsCheckBox;
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
		}

		private void PersistentButtonsSelectionStore()
		{
			m_storedButtonCategory = null;
			if (base.FocusedControl != null)
			{
				if (base.FocusedControl == m_areaTitleOther.Button)
				{
					m_storedButtonCategory = MySessionComponentWarningSystem.Category.Other;
				}
				else if (base.FocusedControl == m_areaTitleServer.Button)
				{
					m_storedButtonCategory = MySessionComponentWarningSystem.Category.Server;
				}
				else if (base.FocusedControl == m_areaTitleBlocks.Button)
				{
					m_storedButtonCategory = MySessionComponentWarningSystem.Category.Blocks;
				}
				else if (base.FocusedControl == m_areaTitleGeneral.Button)
				{
					m_storedButtonCategory = MySessionComponentWarningSystem.Category.General;
				}
				else if (base.FocusedControl == m_areaTitleGraphics.Button)
				{
					m_storedButtonCategory = MySessionComponentWarningSystem.Category.Graphics;
				}
				else if (base.FocusedControl == m_areaTitleBlockLimits.Button)
				{
					m_storedButtonCategory = MySessionComponentWarningSystem.Category.BlockLimits;
				}
				else if (base.FocusedControl == m_areaTitlePerformance.Button)
				{
					m_storedButtonCategory = MySessionComponentWarningSystem.Category.Performance;
				}
			}
		}

		private void PersistentButtonsSelectionRestore()
		{
			if (!m_storedButtonCategory.HasValue)
			{
				return;
			}
			MySessionComponentWarningSystem.Category? storedButtonCategory = m_storedButtonCategory;
			if (!storedButtonCategory.HasValue)
			{
				return;
			}
			switch (storedButtonCategory.GetValueOrDefault())
			{
			case MySessionComponentWarningSystem.Category.Other:
				if (m_areaTitleOther.Button != null)
				{
					base.FocusedControl = m_areaTitleOther.Button;
				}
				break;
			case MySessionComponentWarningSystem.Category.Server:
				if (m_areaTitleServer.Button != null)
				{
					base.FocusedControl = m_areaTitleServer.Button;
				}
				break;
			case MySessionComponentWarningSystem.Category.Blocks:
				if (m_areaTitleBlocks.Button != null)
				{
					base.FocusedControl = m_areaTitleBlocks.Button;
				}
				break;
			case MySessionComponentWarningSystem.Category.General:
				if (m_areaTitleGeneral.Button != null)
				{
					base.FocusedControl = m_areaTitleGeneral.Button;
				}
				break;
			case MySessionComponentWarningSystem.Category.Graphics:
				if (m_areaTitleGraphics.Button != null)
				{
					base.FocusedControl = m_areaTitleGraphics.Button;
				}
				break;
			case MySessionComponentWarningSystem.Category.BlockLimits:
				if (m_areaTitleBlockLimits.Button != null)
				{
					base.FocusedControl = m_areaTitleBlockLimits.Button;
				}
				break;
			case MySessionComponentWarningSystem.Category.Performance:
				if (m_areaTitlePerformance.Button != null)
				{
					base.FocusedControl = m_areaTitlePerformance.Button;
				}
				break;
			case MySessionComponentWarningSystem.Category.UnsafeGrids:
				break;
			}
		}

		private void Refresh()
		{
			if ((float)m_refreshFrameCounter < 60f)
			{
				m_refreshFrameCounter++;
				return;
			}
			float value = (m_warningsList.GetScrollBar().Visible ? m_warningsList.GetScrollBar().Value : 0f);
			PersistentButtonsSelectionStore();
			m_warningsList.Controls.Clear();
			m_areaTitleOther.Warnings.Clear();
			m_areaTitleServer.Warnings.Clear();
			m_areaTitleBlocks.Warnings.Clear();
			m_areaTitleGeneral.Warnings.Clear();
			m_areaTitleGraphics.Warnings.Clear();
			m_areaTitleBlockLimits.Warnings.Clear();
			m_areaTitlePerformance.Warnings.Clear();
			CreateNonProfilerWarnings();
			CreateBlockLimitsWarnings();
			m_warningLines.Clear();
			foreach (MySimpleProfiler.PerformanceWarning value3 in MySimpleProfiler.CurrentWarnings.Values)
			{
				if (((float)value3.Time < 300f || m_showAll) && !m_warningLines.TryGetValue(value3.Block.DisplayStringId, out var value2))
				{
					value2 = new WarningLine(value3, this);
					m_warningLines.Add(value3.Block.DisplayStringId, value2);
				}
			}
			m_refreshFrameCounter = 0;
			if (MyPlatformGameSettings.LIMITED_MAIN_MENU)
			{
				m_areaTitleGraphics.Add(m_warningsList, m_showAll);
			}
			m_areaTitleBlocks.Add(m_warningsList, m_showAll);
			m_areaTitleOther.Add(m_warningsList, m_showAll);
			m_areaTitleUnsafeGrids.Add(m_warningsList, m_showAll);
			m_areaTitleBlockLimits.Add(m_warningsList, m_showAll);
			m_areaTitleServer.Add(m_warningsList, m_showAll);
			m_areaTitlePerformance.Add(m_warningsList, m_showAll);
			m_areaTitleGeneral.Add(m_warningsList, m_showAll);
			m_warningsList.GetScrollBar().Value = value;
			PersistentButtonsSelectionRestore();
		}

		private void CreateBlockLimitsWarnings()
		{
			if (MySession.Static == null)
			{
				return;
			}
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(MySession.Static.LocalPlayerId);
			if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
			{
				m_areaTitleBlockLimits.Warnings.Add(new WarningLine(MyTexts.GetString(MyCommonTexts.WorldSettings_BlockLimits), MyTexts.GetString(MyCommonTexts.Disabled)));
			}
			if (myIdentity == null)
			{
				return;
			}
			if (MySession.Static.MaxBlocksPerPlayer > 0 && (myIdentity.BlockLimits.BlocksBuilt >= myIdentity.BlockLimits.MaxBlocks || m_showAllBlockLimits))
			{
				m_areaTitleBlockLimits.Warnings.Add(new WarningLine(MyTexts.GetString(MyCommonTexts.PerformanceWarningBlocks), $"{myIdentity.BlockLimits.BlocksBuilt}/{myIdentity.BlockLimits.MaxBlocks} {MyTexts.GetString(MyCommonTexts.PerformanceWarningBlocksBuilt)}"));
			}
			MyBlockLimits blockLimits = myIdentity.BlockLimits;
			if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION)
			{
				MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(myIdentity.IdentityId);
				if (playerFaction != null)
				{
					blockLimits = playerFaction.BlockLimits;
				}
			}
			if (MySession.Static.TotalPCU > -1 && (blockLimits.PCU == 0 || m_showAllBlockLimits))
			{
				m_areaTitleBlockLimits.Warnings.Add(new WarningLine("PCU", $"{blockLimits.PCU} {MyTexts.GetString(MyCommonTexts.PerformanceWarningPCUAvailable)}"));
			}
			MyBlockLimits.MyTypeLimitData myTypeLimitData = default(MyBlockLimits.MyTypeLimitData);
			foreach (KeyValuePair<string, short> blockTypeLimit in MySession.Static.BlockTypeLimits)
			{
<<<<<<< HEAD
				myIdentity.BlockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit.Key, out var value);
=======
				myIdentity.BlockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit.Key, ref myTypeLimitData);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyCubeBlockDefinitionGroup myCubeBlockDefinitionGroup = MyDefinitionManager.Static.TryGetDefinitionGroup(blockTypeLimit.Key);
				if (myCubeBlockDefinitionGroup != null && myTypeLimitData != null && (myTypeLimitData.BlocksBuilt >= MySession.Static.GetBlockTypeLimit(blockTypeLimit.Key) || m_showAllBlockLimits))
				{
<<<<<<< HEAD
					m_areaTitleBlockLimits.Warnings.Add(new WarningLine(myCubeBlockDefinitionGroup.Any.DisplayNameText, $"{value.BlocksBuilt}/{MySession.Static.GetBlockTypeLimit(blockTypeLimit.Key)} {MyTexts.GetString(MyCommonTexts.PerformanceWarningBlocksBuilt)}"));
=======
					new WarningLine(myCubeBlockDefinitionGroup.Any.DisplayNameText, $"{myTypeLimitData.BlocksBuilt}/{MySession.Static.GetBlockTypeLimit(blockTypeLimit.Key)} {MyTexts.GetString(MyCommonTexts.PerformanceWarningBlocksBuilt)}", m_areaTitleBlockLimits);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private void CreateNonProfilerWarnings()
		{
			if (MySessionComponentWarningSystem.Static != null)
			{
				DateTime now = DateTime.Now;
				foreach (MySessionComponentWarningSystem.Warning currentWarning in MySessionComponentWarningSystem.Static.CurrentWarnings)
				{
					if (m_showAll || !currentWarning.Time.HasValue || currentWarning.Time.Value.Subtract(now).Seconds < 5)
					{
						GetWarningAreaForCategory(currentWarning.Category).Warnings.Add(new WarningLine(currentWarning.Title, currentWarning.Description, currentWarning.Time));
					}
				}
			}
			if (MyPlatformGameSettings.PUBLIC_BETA_MP_TEST)
			{
<<<<<<< HEAD
				m_areaTitleServer.Warnings.Add(new WarningLine(MyTexts.GetString(MyCommonTexts.PerformanceWarningIssues_BetaTest_Header), MyTexts.GetString(MyCommonTexts.PerformanceWarningIssues_BetaTest_Message)));
=======
				new WarningLine(MyTexts.GetString(MyCommonTexts.PerformanceWarningIssues_BetaTest_Header), MyTexts.GetString(MyCommonTexts.PerformanceWarningIssues_BetaTest_Message), m_areaTitleServer);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (MySession.Static != null && MySession.Static.MultiplayerLastMsg > 3.0)
			{
				string description = string.Format(MyTexts.GetString(MyCommonTexts.Multiplayer_LastMsg), (int)MySession.Static.MultiplayerLastMsg);
				m_areaTitleServer.Warnings.Add(new WarningLine(MyTexts.GetString(MyCommonTexts.PerformanceWarningIssuesServer_Response), description));
			}
			if (!MyGameService.IsOnline)
			{
<<<<<<< HEAD
				m_areaTitlePerformance.Warnings.Add(new WarningLine(string.Format(MyTexts.GetString(MyCommonTexts.GeneralWarningIssues_SteamOffline), MyGameService.Service.ServiceName), string.Format(MyTexts.GetString(MyCommonTexts.General_SteamOffline), MyGameService.Service.ServiceName)));
=======
				new WarningLine(string.Format(MyTexts.GetString(MyCommonTexts.GeneralWarningIssues_SteamOffline), MyGameService.Service.ServiceName), string.Format(MyTexts.GetString(MyCommonTexts.General_SteamOffline), MyGameService.Service.ServiceName), m_areaTitlePerformance);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if ((MySession.Static != null && MySession.Static.IsRunningExperimental) || MySandboxGame.Config.ExperimentalMode || MyDebugDrawSettings.DEBUG_DRAW_SERVER_WARNINGS)
			{
				m_areaTitlePerformance.Warnings.Add(new WarningLine(MyTexts.GetString(MyCommonTexts.GeneralWarningIssues_Experimental), MyTexts.GetString(MyCommonTexts.General_Experimental)));
			}
			if (!MyGameService.Service.GetInstallStatus(out var _))
			{
<<<<<<< HEAD
				m_areaTitleGeneral.Warnings.Add(new WarningLine(MyTexts.GetString(MyCommonTexts.GeneralWarningIssues_InstallInProgress), MyTexts.GetString(MyCommonTexts.General_InstallInProgress)));
=======
				new WarningLine(MyTexts.GetString(MyCommonTexts.GeneralWarningIssues_InstallInProgress), MyTexts.GetString(MyCommonTexts.General_InstallInProgress), m_areaTitleGeneral);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (MySession.Static == null)
			{
				return;
			}
			foreach (KeyValuePair<MyDLCs.MyDLC, int> usedUnownedDLC in MySession.Static.GetComponent<MySessionComponentDLC>().UsedUnownedDLCs)
			{
<<<<<<< HEAD
				m_areaTitleGeneral.Warnings.Add(new WarningLine(MyTexts.GetString(MyCommonTexts.PerformanceWarningTitle_PaidContent), string.Format(MyTexts.GetString(MyCommonTexts.PerformanceWarningIssuesPaidContent), MyTexts.GetString(usedUnownedDLC.Key.DisplayName))));
=======
				new WarningLine(MyTexts.GetString(MyCommonTexts.PerformanceWarningTitle_PaidContent), string.Format(MyTexts.GetString(MyCommonTexts.PerformanceWarningIssuesPaidContent), MyTexts.GetString(usedUnownedDLC.Key.DisplayName)), m_areaTitleGeneral);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void ShowWarningsChanged(MyGuiControlCheckbox obj)
		{
			MySandboxGame.Config.EnablePerformanceWarnings = obj.IsChecked;
		}

		private void KeepInListChanged(MyGuiControlCheckbox obj)
		{
			m_showAll = obj.IsChecked;
		}

		private void KeepInListChangedBlockLimits(MyGuiControlCheckbox obj)
		{
			m_showAllBlockLimits = obj.IsChecked;
			m_areaTitleBlockLimits.Warnings.Clear();
			CreateBlockLimitsWarnings();
		}

		private WarningArea GetWarningAreaForCategory(MySessionComponentWarningSystem.Category category)
		{
			return category switch
			{
				MySessionComponentWarningSystem.Category.BlockLimits => m_areaTitleBlockLimits, 
				MySessionComponentWarningSystem.Category.Blocks => m_areaTitleBlocks, 
				MySessionComponentWarningSystem.Category.General => m_areaTitleGeneral, 
				MySessionComponentWarningSystem.Category.Graphics => m_areaTitleGraphics, 
				MySessionComponentWarningSystem.Category.Other => m_areaTitleOther, 
				MySessionComponentWarningSystem.Category.Performance => m_areaTitlePerformance, 
				MySessionComponentWarningSystem.Category.Server => m_areaTitleServer, 
				MySessionComponentWarningSystem.Category.UnsafeGrids => m_areaTitleServer, 
				_ => m_areaTitleOther, 
			};
		}

		private void OnOkButtonClicked(MyGuiControlButton obj)
		{
			CloseScreen();
		}

		public override bool Update(bool hasFocus)
		{
			Refresh();
			if (MyInput.Static.IsJoystickLastUsed && !m_okButton.Visible)
			{
				UpdateGamepadHelp(base.FocusedControl);
			}
			m_screenDescText.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			return base.Update(hasFocus);
		}
	}
}
