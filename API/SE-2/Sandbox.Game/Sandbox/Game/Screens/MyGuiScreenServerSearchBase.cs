using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	/// <summary>
	///     Provides filter options and controls that are common to both games.
	/// </summary>
	public class MyGuiScreenServerSearchBase : MyGuiScreenBase
	{
		protected enum SearchPageEnum
		{
			Settings,
			Advanced,
			Mods
		}

		private static List<MyWorkshopItem> m_subscribedMods;

		private static List<MyWorkshopItem> m_settingsMods;

		private static bool m_needsModRefresh = true;

		private MyGuiControlRotatingWheel m_loadingWheel;

		protected Vector2 m_currentPosition;

		protected MyGuiScreenJoinGame m_joinScreen;

		protected float m_padding = 0.02f;

		protected MyGuiControlScrollablePanel m_panel;

		protected MyGuiControlParent m_parent;

		protected MyGuiControlCheckbox m_advancedCheckbox;

		protected MyGuiControlButton m_searchButton;

		protected MyGuiControlButton m_settingsButton;

		protected MyGuiControlButton m_advancedButton;

		protected MyGuiControlButton m_modsButton;

		private MyGuiControlButton m_btnDefault;

		private SearchPageEnum m_currentPage;

		protected SearchPageEnum CurrentPage
		{
			get
			{
				return m_currentPage;
			}
			set
			{
				if (m_currentPage != value)
				{
					m_currentPage = value;
					if (m_currentPage == SearchPageEnum.Mods)
					{
						PrepareModPage();
					}
					else
					{
						RecreateControls(constructor: false);
					}
				}
			}
		}

		protected bool EnableAdvanced
		{
			get
			{
				if (FilterOptions.AdvancedFilter)
				{
					return m_joinScreen.EnableAdvancedSearch;
				}
				return false;
			}
		}

		protected Vector2 WindowSize => new Vector2(base.Size.Value.X - 0.1f, base.Size.Value.Y - m_settingsButton.Size.Y * 2f - m_padding * 16f);

		protected MyServerFilterOptions FilterOptions
		{
			get
			{
				return m_joinScreen.FilterOptions;
			}
			set
			{
				m_joinScreen.FilterOptions = value;
			}
		}

		public MyGuiScreenServerSearchBase(MyGuiScreenJoinGame joinScreen)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 0.9398855f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			m_joinScreen = joinScreen;
			CreateScreen();
		}

		private void CreateScreen()
		{
			base.CanHideOthers = true;
			base.CanBeHidden = true;
			base.EnabledBackgroundFade = true;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ServerSearch, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiControlSeparatorList myGuiControlSeparatorList3 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList3.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.15f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList3);
			m_currentPosition = new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f - 0.003f, m_size.Value.Y / 2f - 0.095f);
			float y = m_currentPosition.Y;
<<<<<<< HEAD
			m_settingsButton = AddButton(MyCommonTexts.ServerDetails_Settings, SettingsButtonClick);
			m_settingsButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Settings));
			m_currentPosition.Y = y;
			m_currentPosition.X += m_settingsButton.Size.X + m_padding / 3.6f;
			m_advancedButton = AddButton(MyCommonTexts.Advanced, AdvancedButtonClick);
=======
			m_settingsButton = AddButton(MyCommonTexts.ServerDetails_Settings, SettingsButtonClick, null, enabled: true, addToParent: false);
			m_settingsButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Settings));
			m_currentPosition.Y = y;
			m_currentPosition.X += m_settingsButton.Size.X + m_padding / 3.6f;
			m_advancedButton = AddButton(MyCommonTexts.Advanced, AdvancedButtonClick, null, enabled: true, addToParent: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_advancedButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Advanced));
			m_currentPosition.Y = y;
			m_currentPosition.X += m_settingsButton.Size.X + m_padding / 3.6f;
			Vector2 vector;
			if (MyPlatformGameSettings.IsModdingAllowed && MyFakes.ENABLE_WORKSHOP_MODS)
			{
<<<<<<< HEAD
				m_modsButton = AddButton(MyCommonTexts.WorldSettings_Mods, ModsButtonClick);
=======
				m_modsButton = AddButton(MyCommonTexts.WorldSettings_Mods, ModsButtonClick, null, enabled: true, addToParent: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_modsButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Mods));
				m_currentPosition.Y = y;
				m_currentPosition.X += m_settingsButton.Size.X + m_padding;
				vector = m_modsButton.Position;
			}
			else
			{
				vector = m_currentPosition;
			}
			m_loadingWheel = new MyGuiControlRotatingWheel(vector + new Vector2(0.137f, -0.004f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.2f);
			Controls.Add(m_loadingWheel);
			m_loadingWheel.Visible = false;
			m_btnDefault = new MyGuiControlButton(new Vector2(0f, 0f) - new Vector2(-0.003f, (0f - m_size.Value.Y) / 2f + 0.071f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ServerSearch_Defaults));
			m_btnDefault.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Defaults));
			m_btnDefault.ButtonClicked += DefaultSettingsClick;
			m_btnDefault.ButtonClicked += DefaultModsClick;
			m_btnDefault.Visible = !MyInput.Static.IsJoystickLastUsed;
			Controls.Add(m_btnDefault);
			m_searchButton = new MyGuiControlButton(new Vector2(0f, 0f) - new Vector2(0.18f, (0f - m_size.Value.Y) / 2f + 0.071f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenMods_SearchLabel));
			m_searchButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Search));
			m_searchButton.ButtonClicked += SearchClick;
			m_searchButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			Controls.Add(m_searchButton);
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(m_searchButton.Position.X - minSizeGui.X / 2f, m_searchButton.Position.Y));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.GamepadHelpTextId = MySpaceTexts.ServerSearch_Help_Screen;
			m_currentPosition = -WindowSize / 2f;
			switch (CurrentPage)
			{
			case SearchPageEnum.Settings:
				base.FocusedControl = m_settingsButton;
				m_settingsButton.Checked = true;
				m_settingsButton.Selected = true;
				m_currentPosition.Y += m_padding * 2f;
				DrawSettingsSelector();
				DrawTopControls();
				DrawMidControls();
				break;
			case SearchPageEnum.Mods:
				base.FocusedControl = m_modsButton;
				m_modsButton.Checked = true;
				m_modsButton.Selected = true;
				DrawModSelector();
				break;
			case SearchPageEnum.Advanced:
				base.FocusedControl = m_advancedButton;
				m_advancedButton.Checked = true;
				m_advancedButton.Selected = true;
				DrawAdvancedSelector();
				DrawBottomControls();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void DefaultSettingsClick(MyGuiControlButton myGuiControlButton)
		{
			FilterOptions.SetDefaults();
			RecreateControls(constructor: false);
		}

		private void DefaultModsClick(MyGuiControlButton myGuiControlButton)
		{
			FilterOptions.Mods.Clear();
			RecreateControls(constructor: false);
		}

		private void CancelButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CloseScreen();
		}

		private void PrepareModPage()
		{
			if (m_needsModRefresh)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, LoadModsBeginAction, LoadModsEndAction));
				m_needsModRefresh = false;
			}
			else
			{
				RecreateControls(constructor: false);
				m_loadingWheel.Visible = false;
			}
		}

		private void ModsButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CurrentPage = SearchPageEnum.Mods;
			base.FocusedControl = m_modsButton;
		}

		private void SettingsButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CurrentPage = SearchPageEnum.Settings;
			base.FocusedControl = m_settingsButton;
		}

		private void AdvancedButtonClick(MyGuiControlButton myGuiControlButton)
		{
			CurrentPage = SearchPageEnum.Advanced;
			base.FocusedControl = m_advancedButton;
		}

		private void DrawModSelector()
		{
			m_parent = new MyGuiControlParent();
			m_panel = new MyGuiControlScrollablePanel(m_parent);
			m_panel.ScrollbarVEnabled = true;
			m_panel.PositionX += 0.0075f;
			m_panel.PositionY += m_settingsButton.Size.Y / 2f + m_padding * 1.7f;
			m_panel.Size = new Vector2(base.Size.Value.X - 0.1f, base.Size.Value.Y - m_settingsButton.Size.Y * 2f - m_padding * 13.7f);
			Controls.Add(m_panel);
			m_advancedCheckbox = new MyGuiControlCheckbox(new Vector2(-0.0435f, -0.279f), null, MyTexts.GetString(MyCommonTexts.ServerSearch_EnableAdvancedTooltip));
			m_advancedCheckbox.IsChecked = FilterOptions.AdvancedFilter;
			MyGuiControlCheckbox advancedCheckbox = m_advancedCheckbox;
			advancedCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(advancedCheckbox.IsCheckedChanged, (Action<MyGuiControlCheckbox>)delegate(MyGuiControlCheckbox c)
			{
				FilterOptions.AdvancedFilter = c.IsChecked;
				RecreateControls(constructor: false);
			});
			m_advancedCheckbox.Enabled = m_joinScreen.EnableAdvancedSearch;
			Controls.Add(m_advancedCheckbox);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_advancedCheckbox.Position - new Vector2(m_advancedCheckbox.Size.X / 2f + m_padding * 10.45f, 0f), null, MyTexts.GetString(MyCommonTexts.ServerSearch_EnableAdvanced));
			myGuiControlLabel.SetToolTip(MyCommonTexts.ServerSearch_EnableAdvancedTooltip);
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlLabel.Enabled = m_joinScreen.EnableAdvancedSearch;
			Controls.Add(myGuiControlLabel);
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(new Vector2(0.246499985f, -0.279f));
			myGuiControlCheckbox.IsChecked = FilterOptions.ModsExclusive;
			myGuiControlCheckbox.SetToolTip(MyCommonTexts.ServerSearch_ExclusiveTooltip);
			myGuiControlCheckbox.IsCheckedChanged = delegate(MyGuiControlCheckbox c)
			{
				FilterOptions.ModsExclusive = c.IsChecked;
			};
			myGuiControlCheckbox.Enabled = true;
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(myGuiControlCheckbox.Position - new Vector2(myGuiControlCheckbox.Size.X / 2f + m_padding * 10.45f, 0f), null, MyTexts.GetString(MyCommonTexts.ServerSearch_Exclusive));
			myGuiControlLabel2.SetToolTip(MyCommonTexts.ServerSearch_ExclusiveTooltip);
			myGuiControlLabel2.Enabled = true;
			myGuiControlLabel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			Controls.Add(myGuiControlCheckbox);
			Controls.Add(myGuiControlLabel2);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.23f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Small, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ServerSearch_Clear));
			MyGuiControlCheckbox myGuiControlCheckbox2 = new MyGuiControlCheckbox(m_currentPosition);
			float num = myGuiControlCheckbox2.Size.Y * (float)(m_subscribedMods.Count + m_settingsMods.Count) + myGuiControlButton.Size.Y / 2f + m_padding;
			m_currentPosition = -m_panel.Size / 2f;
			m_currentPosition.Y = (0f - num) / 2f + myGuiControlCheckbox2.Size.Y / 2f - 0.005f;
			m_currentPosition.X -= 0.0225f;
			m_parent.Size = new Vector2(m_panel.Size.X, num);
			m_subscribedMods.Sort((MyWorkshopItem a, MyWorkshopItem b) => a.Title.CompareTo(b.Title));
			m_settingsMods.Sort((MyWorkshopItem a, MyWorkshopItem b) => a.Title.CompareTo(b.Title));
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlButton.ButtonClicked += DefaultModsClick;
			myGuiControlButton.Position = m_currentPosition + new Vector2(m_padding, (0f - m_padding) * 6f);
			m_parent.Controls.Add(myGuiControlButton);
			foreach (MyWorkshopItem mod2 in m_subscribedMods)
			{
				int num2 = Math.Min(mod2.Description.Length, 128);
				int num3 = mod2.Description.IndexOf("\n");
				if (num3 > 0)
				{
					num2 = Math.Min(num2, num3 - 1);
				}
				MyGuiControlCheckbox myGuiControlCheckbox3 = AddCheckbox(mod2.Title, delegate(MyGuiControlCheckbox c)
				{
					ModCheckboxClick(c, mod2.Id, mod2.ServiceName);
<<<<<<< HEAD
				}, mod2.Description.Substring(0, num2), null, enabled: true, isAutoEllipsisEnabled: true, isAutoScaleEnabled: true, 0.48f);
=======
				}, mod2.Description.Substring(0, num2), null, enabled: true, isAutoEllipsisEnabled: true, isAutoScaleEnabled: true, 0.5f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myGuiControlCheckbox3.IsChecked = FilterOptions.Mods.Contains(new WorkshopId(mod2.Id, mod2.ServiceName));
				myGuiControlCheckbox3.Enabled = FilterOptions.AdvancedFilter;
			}
			foreach (MyWorkshopItem mod in m_settingsMods)
			{
				int num4 = Math.Min(mod.Description.Length, 128);
				int num5 = mod.Description.IndexOf("\n");
				if (num5 > 0)
				{
					num4 = Math.Min(num4, num5 - 1);
				}
				MyGuiControlCheckbox myGuiControlCheckbox4 = AddCheckbox(mod.Title, delegate(MyGuiControlCheckbox c)
				{
					ModCheckboxClick(c, mod.Id, mod.ServiceName);
<<<<<<< HEAD
				}, mod.Description.Substring(0, num4), "DarkBlue", EnableAdvanced, isAutoEllipsisEnabled: true, isAutoScaleEnabled: true, 0.48f);
=======
				}, mod.Description.Substring(0, num4), "DarkBlue", EnableAdvanced, isAutoEllipsisEnabled: true, isAutoScaleEnabled: true, 0.5f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myGuiControlCheckbox4.IsChecked = FilterOptions.Mods.Contains(new WorkshopId(mod.Id, mod.ServiceName));
				myGuiControlCheckbox4.Enabled = FilterOptions.AdvancedFilter;
			}
		}

		private void DrawAdvancedSelector()
		{
			m_advancedCheckbox = new MyGuiControlCheckbox(new Vector2(-0.0435f, -0.279f), null, MyTexts.GetString(MyCommonTexts.ServerSearch_EnableAdvancedTooltip));
			m_advancedCheckbox.IsChecked = FilterOptions.AdvancedFilter;
			MyGuiControlCheckbox advancedCheckbox = m_advancedCheckbox;
			advancedCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(advancedCheckbox.IsCheckedChanged, (Action<MyGuiControlCheckbox>)delegate(MyGuiControlCheckbox c)
			{
				FilterOptions.AdvancedFilter = c.IsChecked;
				RecreateControls(constructor: false);
			});
			m_advancedCheckbox.Enabled = m_joinScreen.EnableAdvancedSearch;
			Controls.Add(m_advancedCheckbox);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_advancedCheckbox.Position - new Vector2(m_advancedCheckbox.Size.X / 2f + m_padding * 10.45f, 0f), null, MyTexts.GetString(MyCommonTexts.ServerSearch_EnableAdvanced));
			myGuiControlLabel.SetToolTip(MyCommonTexts.ServerSearch_EnableAdvancedTooltip);
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlLabel.Enabled = m_joinScreen.EnableAdvancedSearch;
			Controls.Add(myGuiControlLabel);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.23f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			m_currentPosition.Y += 0.07f;
		}

		private void DrawSettingsSelector()
		{
			m_currentPosition.Y = -0.279f;
			AddCheckboxDuo(new MyStringId?[2]
			{
				MyCommonTexts.WorldSettings_GameModeCreative,
				MyCommonTexts.WorldSettings_GameModeSurvival
			}, new Action<MyGuiControlCheckbox>[2]
			{
				delegate(MyGuiControlCheckbox c)
				{
					FilterOptions.CreativeMode = c.IsChecked;
				},
				delegate(MyGuiControlCheckbox c)
				{
					FilterOptions.SurvivalMode = c.IsChecked;
				}
			}, new MyStringId?[2]
			{
				MySpaceTexts.ToolTipJoinGameServerSearch_Creative,
				MySpaceTexts.ToolTipJoinGameServerSearch_Survival
			}, new bool[2] { FilterOptions.CreativeMode, FilterOptions.SurvivalMode });
			AddCheckboxDuo(new MyStringId?[2]
			{
				MyCommonTexts.MultiplayerCompatibleVersions,
				MyCommonTexts.MultiplayerJoinSameGameData
			}, new Action<MyGuiControlCheckbox>[2]
			{
				delegate(MyGuiControlCheckbox c)
				{
					FilterOptions.SameVersion = c.IsChecked;
				},
				delegate(MyGuiControlCheckbox c)
				{
					FilterOptions.SameData = c.IsChecked;
				}
			}, new MyStringId?[2]
			{
				MySpaceTexts.ToolTipJoinGameServerSearch_CompatibleVersions,
				MySpaceTexts.ToolTipJoinGameServerSearch_SameGameData
			}, new bool[2] { FilterOptions.SameVersion, FilterOptions.SameData });
			Vector2 currentPosition = m_currentPosition;
			AddCheckboxDuo(new string[2]
			{
				MyTexts.GetString(MyCommonTexts.MultiplayerJoinAllowedGroups),
				null
			}, new Action<MyGuiControlCheckbox>[1]
			{
				delegate(MyGuiControlCheckbox c)
				{
					FilterOptions.AllowedGroups = c.IsChecked;
				}
			}, new string[1] { string.Format(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_AllowedGroups), MyGameService.Service.ServiceName) }, new bool[1] { FilterOptions.AllowedGroups }, m_joinScreen.SupportsGroups);
			m_currentPosition = currentPosition;
			AddIndeterminateDuo(new MyStringId?[2]
			{
				null,
				MyCommonTexts.MultiplayerJoinHasPassword
			}, new Action<MyGuiControlIndeterminateCheckbox>[2]
			{
				null,
				delegate(MyGuiControlIndeterminateCheckbox c)
				{
					switch (c.State)
					{
					case CheckStateEnum.Checked:
						FilterOptions.HasPassword = true;
						break;
					case CheckStateEnum.Unchecked:
						FilterOptions.HasPassword = false;
						break;
					case CheckStateEnum.Indeterminate:
						FilterOptions.HasPassword = null;
						break;
					}
				}
			}, new MyStringId?[2]
			{
				null,
				MySpaceTexts.ToolTipJoinGameServerSearch_HasPassword
			}, new CheckStateEnum[2]
			{
				CheckStateEnum.Indeterminate,
				(!FilterOptions.HasPassword.HasValue || !FilterOptions.HasPassword.Value) ? (FilterOptions.HasPassword.HasValue ? CheckStateEnum.Unchecked : CheckStateEnum.Indeterminate) : CheckStateEnum.Checked
			});
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.325f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.409f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
			myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.835f / 2f, m_size.Value.Y / 2f - 0.657f), m_size.Value.X * 0.835f);
			Controls.Add(myGuiControlSeparatorList);
		}

		private void ModCheckboxClick(MyGuiControlCheckbox c, ulong modId, string serviceName)
		{
			if (c.IsChecked)
			{
				FilterOptions.Mods.Add(new WorkshopId(modId, serviceName));
			}
			else
			{
				FilterOptions.Mods.Remove(new WorkshopId(modId, serviceName));
			}
		}

		protected virtual void DrawTopControls()
		{
			m_currentPosition.Y = -0.0225f;
			AddNumericRangeOption(MyCommonTexts.MultiplayerJoinOnlinePlayers, delegate(SerializableRange r)
			{
				FilterOptions.PlayerCount = r;
			}, FilterOptions.PlayerCount, FilterOptions.CheckPlayer, delegate(MyGuiControlCheckbox c)
			{
				FilterOptions.CheckPlayer = c.IsChecked;
			});
			AddNumericRangeOption(MyCommonTexts.JoinGame_ColumnTitle_Mods, delegate(SerializableRange r)
			{
				FilterOptions.ModCount = r;
			}, FilterOptions.ModCount, FilterOptions.CheckMod, delegate(MyGuiControlCheckbox c)
			{
				FilterOptions.CheckMod = c.IsChecked;
			}, MyPlatformGameSettings.IsModdingAllowed);
			AddNumericRangeOption(MySpaceTexts.WorldSettings_ViewDistance, delegate(SerializableRange r)
			{
				FilterOptions.ViewDistance = r;
			}, FilterOptions.ViewDistance, FilterOptions.CheckDistance, delegate(MyGuiControlCheckbox c)
			{
				FilterOptions.CheckDistance = c.IsChecked;
			});
		}

		protected virtual void DrawMidControls()
		{
			Vector2 currentPosition = m_currentPosition;
			m_currentPosition.Y += m_padding * 1.32f;
			m_currentPosition.X += m_padding / 2.4f;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_currentPosition, null, MyTexts.GetString(MyCommonTexts.JoinGame_ColumnTitle_Ping));
			myGuiControlLabel.Enabled = m_joinScreen.EnableAdvancedSearch;
			Controls.Add(myGuiControlLabel);
			m_currentPosition.X += m_padding * 2.3f;
			MyGuiControlSlider myGuiControlSlider = new MyGuiControlSlider(m_currentPosition + new Vector2(0.215f, 0f), -1f, 1000f, 0.29f, FilterOptions.Ping, null, string.Empty, 1, 0f);
			myGuiControlSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			myGuiControlSlider.LabelDecimalPlaces = 0;
			myGuiControlSlider.IntValue = true;
			myGuiControlSlider.Size = new Vector2(0.45f - myGuiControlLabel.Size.X, 1f);
			myGuiControlSlider.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_Ping));
			myGuiControlSlider.PositionX += myGuiControlSlider.Size.X / 2f;
			myGuiControlSlider.Enabled = m_joinScreen.SupportsPing;
			Controls.Add(myGuiControlSlider);
			m_currentPosition.X += myGuiControlSlider.Size.X / 2f + m_padding * 14f;
			MyGuiControlLabel val = new MyGuiControlLabel(m_currentPosition, null, "<" + myGuiControlSlider.Value + "ms", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			myGuiControlSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider x)
			{
				val.Text = "<" + x.Value + "ms";
				FilterOptions.Ping = (int)x.Value;
			});
			val.Enabled = m_joinScreen.EnableAdvancedSearch;
			Controls.Add(val);
			m_currentPosition = currentPosition;
			m_currentPosition.Y += 0.04f;
		}

		protected virtual void DrawBottomControls()
		{
		}

		public override bool Draw()
		{
			base.Draw();
<<<<<<< HEAD
			if (MyInput.Static.IsJoystickLastUsed)
			{
				MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
				Vector2 positionAbsoluteTopLeft = m_settingsButton.GetPositionAbsoluteTopLeft();
				Vector2 size = m_settingsButton.Size;
				Vector2 normalizedCoord = positionAbsoluteTopLeft;
				normalizedCoord.Y += size.Y / 2f;
				normalizedCoord.X -= size.X / 6f;
				Vector2 normalizedCoord2 = positionAbsoluteTopLeft;
				normalizedCoord2.Y = normalizedCoord.Y;
				int num = ((m_modsButton == null) ? 2 : 3);
				Color value = MyGuiControlBase.ApplyColorMaskModifiers(MyGuiConstants.LABEL_TEXT_COLOR, enabled: true, m_transitionAlpha);
				normalizedCoord2.X += (float)num * size.X + size.X / 6f;
				MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Left), normalizedCoord, 1f, value, drawAlign);
				MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Right), normalizedCoord2, 1f, value, drawAlign);
			}
=======
			MyGuiDrawAlignEnum drawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			Vector2 positionAbsoluteTopLeft = m_settingsButton.GetPositionAbsoluteTopLeft();
			Vector2 size = m_settingsButton.Size;
			Vector2 normalizedCoord = positionAbsoluteTopLeft;
			normalizedCoord.Y += size.Y / 2f;
			normalizedCoord.X -= size.X / 6f;
			Vector2 normalizedCoord2 = positionAbsoluteTopLeft;
			normalizedCoord2.Y = normalizedCoord.Y;
			int num = ((m_modsButton == null) ? 2 : 3);
			Color value = MyGuiControlBase.ApplyColorMaskModifiers(MyGuiConstants.LABEL_TEXT_COLOR, enabled: true, m_transitionAlpha);
			normalizedCoord2.X += (float)num * size.X + size.X / 6f;
			MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Left), normalizedCoord, 1f, value, drawAlign);
			MyGuiManager.DrawString("Blue", MyTexts.GetString(MyCommonTexts.Gamepad_Help_TabControl_Right), normalizedCoord2, 1f, value, drawAlign);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return true;
		}

		private void SearchClick(MyGuiControlButton myGuiControlButton)
		{
			CloseScreen();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenServerSearchBase";
		}

		private IMyAsyncResult LoadModsBeginAction()
		{
			return new MyModsLoadListResult(FilterOptions.Mods);
		}

		private void LoadModsEndAction(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			MyModsLoadListResult obj = (MyModsLoadListResult)result;
			m_subscribedMods = obj.SubscribedMods;
			m_settingsMods = obj.SetMods;
			screen.CloseScreen();
			m_loadingWheel.Visible = false;
			RecreateControls(constructor: false);
		}

		protected MyGuiControlButton AddButton(MyStringId text, Action<MyGuiControlButton> onClick, MyStringId? tooltip = null, bool enabled = true)
		{
			Vector2? position = m_currentPosition;
			StringBuilder text2 = MyTexts.Get(text);
			string toolTip = (tooltip.HasValue ? MyTexts.GetString(tooltip.Value) : string.Empty);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.ToolbarButton, null, Color.Yellow.ToVector4(), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP, toolTip, text2, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, onClick);
			myGuiControlButton.Enabled = enabled;
			myGuiControlButton.PositionX += myGuiControlButton.Size.X / 2f;
<<<<<<< HEAD
=======
			if (addToParent)
			{
				Controls.Add(myGuiControlButton);
			}
			else
			{
				Controls.Add(myGuiControlButton);
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_currentPosition.Y += myGuiControlButton.Size.Y + m_padding;
			return myGuiControlButton;
		}

		protected void AddNumericRangeOption(MyStringId text, Action<SerializableRange> onEntry, SerializableRange currentRange, bool active, Action<MyGuiControlCheckbox> onEnable, bool enabled = true)
		{
			float num = 0.004f;
			float x = m_currentPosition.X;
			m_currentPosition.X = (0f - WindowSize.X) / 2f + m_padding * 12.6f;
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(m_currentPosition + new Vector2(0f, num), null, MyTexts.GetString(MyCommonTexts.ServerSearch_EnableNumericTooltip));
			myGuiControlCheckbox.PositionX += myGuiControlCheckbox.Size.X / 2f;
			myGuiControlCheckbox.IsChecked = active && enabled;
			myGuiControlCheckbox.Enabled = enabled;
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, onEnable);
			Controls.Add(myGuiControlCheckbox);
			m_currentPosition.X += myGuiControlCheckbox.Size.X / 2f + m_padding;
			MyGuiControlTextbox minText = new MyGuiControlTextbox(m_currentPosition, currentRange.Min.ToString(), 6, null, 0.8f, MyGuiControlTextboxType.DigitsOnly);
			minText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			minText.Size = new Vector2(0.12f, minText.Size.Y);
			minText.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_MinimumFilterValue));
			minText.Enabled = myGuiControlCheckbox.IsChecked;
			Controls.Add(minText);
			m_currentPosition.X += minText.Size.X / 1.5f + m_padding + 0.028f;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(m_currentPosition, null, "-");
			Controls.Add(myGuiControlLabel);
			m_currentPosition.X += myGuiControlLabel.Size.X / 2f + m_padding / 2f;
			MyGuiControlTextbox maxText = new MyGuiControlTextbox(m_currentPosition, float.IsInfinity(currentRange.Max) ? "-1" : currentRange.Max.ToString(), 6, null, 0.8f, MyGuiControlTextboxType.DigitsOnly);
			maxText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			maxText.Size = new Vector2(0.12f, maxText.Size.Y);
			maxText.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipJoinGameServerSearch_MaximumFilterValue));
			maxText.Enabled = myGuiControlCheckbox.IsChecked;
			Controls.Add(maxText);
			m_currentPosition.X += maxText.Size.X / 1.5f + m_padding + 0.01f;
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2(-0.27f, m_currentPosition.Y), null, MyTexts.GetString(text));
			myGuiControlLabel2.Enabled = true;
			Controls.Add(myGuiControlLabel2);
			m_currentPosition.X = x;
			m_currentPosition.Y += myGuiControlLabel2.Size.Y + m_padding + num;
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, (Action<MyGuiControlCheckbox>)delegate(MyGuiControlCheckbox c)
			{
				MyGuiControlTextbox myGuiControlTextbox = minText;
				bool enabled2 = (maxText.Enabled = c.IsChecked);
				myGuiControlTextbox.Enabled = enabled2;
			});
			if (onEntry == null)
<<<<<<< HEAD
			{
				return;
			}
			maxText.TextChanged += delegate
			{
=======
			{
				return;
			}
			maxText.TextChanged += delegate
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (float.TryParse(minText.Text, out var result3) && float.TryParse(maxText.Text, out var result4))
				{
					if (result4 == -1f)
					{
						result4 = float.PositiveInfinity;
					}
					if (result3 < 0f)
					{
						result3 = 0f;
					}
					onEntry(new SerializableRange(result3, result4));
				}
			};
			minText.TextChanged += delegate
			{
				if (float.TryParse(minText.Text, out var result) && float.TryParse(maxText.Text, out var result2))
				{
					if (result2 == -1f)
					{
						result2 = float.PositiveInfinity;
					}
					if (result < 0f)
					{
						result = 0f;
					}
					onEntry(new SerializableRange(result, result2));
				}
			};
		}

		protected MyGuiControlCheckbox AddCheckbox(MyStringId text, Action<MyGuiControlCheckbox> onClick, MyStringId? tooltip = null, string font = null, bool enabled = true)
		{
			return AddCheckbox(MyTexts.GetString(text), onClick, tooltip.HasValue ? MyTexts.GetString(tooltip.Value) : null, font, enabled);
		}

		protected MyGuiControlCheckbox AddCheckbox(string text, Action<MyGuiControlCheckbox> onClick, string tooltip = null, string font = null, bool enabled = true, bool isAutoEllipsisEnabled = false, bool isAutoScaleEnabled = false, float maxWidth = float.PositiveInfinity)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(m_currentPosition, null, tooltip ?? string.Empty);
			myGuiControlCheckbox.PositionX += myGuiControlCheckbox.Size.X / 2f + m_padding * 26f;
			m_parent.Controls.Add(myGuiControlCheckbox);
			if (onClick != null)
			{
				myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, onClick);
			}
			Vector2? position = m_currentPosition;
			string @string = MyTexts.GetString(text);
			bool isAutoScaleEnabled2 = isAutoScaleEnabled;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(position, null, @string, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, isAutoEllipsisEnabled, maxWidth, isAutoScaleEnabled2);
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlLabel.PositionX = myGuiControlCheckbox.PositionX - m_padding * 25.8f;
			if (!string.IsNullOrEmpty(tooltip))
			{
				myGuiControlLabel.SetToolTip(tooltip);
			}
			if (!string.IsNullOrEmpty(font))
			{
				myGuiControlLabel.Font = font;
			}
			m_parent.Controls.Add(myGuiControlLabel);
			m_currentPosition.Y += myGuiControlCheckbox.Size.Y;
			myGuiControlCheckbox.Enabled = enabled;
			myGuiControlLabel.Enabled = enabled;
			return myGuiControlCheckbox;
		}

		protected MyGuiControlCheckbox[] AddCheckboxDuo(MyStringId?[] text, Action<MyGuiControlCheckbox>[] onClick, MyStringId?[] tooltip, bool[] values)
		{
			string[] array = new string[text.Length];
			string[] array2 = new string[tooltip.Length];
			for (int i = 0; i < text.Length; i++)
			{
				array[i] = (text[i].HasValue ? MyTexts.GetString(text[i].Value) : string.Empty);
			}
			for (int j = 0; j < tooltip.Length; j++)
			{
				array2[j] = (tooltip[j].HasValue ? MyTexts.GetString(tooltip[j].Value) : string.Empty);
			}
			return AddCheckboxDuo(array, onClick, array2, values);
		}

		protected MyGuiControlCheckbox[] AddCheckboxDuo(string[] text, Action<MyGuiControlCheckbox>[] onClick, string[] tooltip, bool[] values, bool enabled = true)
		{
			MyGuiControlCheckbox[] array = new MyGuiControlCheckbox[2];
			float x = m_currentPosition.X;
			if (!string.IsNullOrEmpty(text[0]))
			{
				MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(m_currentPosition, null, (!string.IsNullOrEmpty(tooltip[0])) ? MyTexts.GetString(tooltip[0]) : string.Empty);
				myGuiControlCheckbox.PositionX = -0.0435f;
				myGuiControlCheckbox.IsChecked = values[0];
				myGuiControlCheckbox.Enabled = enabled;
				array[0] = myGuiControlCheckbox;
				if (onClick[0] != null)
				{
					myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, onClick[0]);
				}
				m_currentPosition.X = myGuiControlCheckbox.PositionX + myGuiControlCheckbox.Size.X / 2f + m_padding / 3f;
				MyGuiControlLabel control = new MyGuiControlLabel(myGuiControlCheckbox.Position - new Vector2(myGuiControlCheckbox.Size.X / 2f + m_padding * 10.45f, 0f), null, MyTexts.GetString(text[0]));
				Controls.Add(myGuiControlCheckbox);
				Controls.Add(control);
			}
			if (!string.IsNullOrEmpty(text[1]))
			{
				MyGuiControlCheckbox myGuiControlCheckbox2 = new MyGuiControlCheckbox(m_currentPosition, null, (!string.IsNullOrEmpty(tooltip[1])) ? MyTexts.GetString(tooltip[1]) : string.Empty);
				myGuiControlCheckbox2.PositionX = 0.262f;
				myGuiControlCheckbox2.IsChecked = values[1];
				myGuiControlCheckbox2.Enabled = enabled;
				array[1] = myGuiControlCheckbox2;
				if (onClick[1] != null)
				{
					myGuiControlCheckbox2.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox2.IsCheckedChanged, onClick[1]);
				}
				m_currentPosition.X = myGuiControlCheckbox2.PositionX + myGuiControlCheckbox2.Size.X / 2f + m_padding / 2f;
				MyGuiControlLabel control2 = new MyGuiControlLabel(myGuiControlCheckbox2.Position - new Vector2(myGuiControlCheckbox2.Size.X / 2f + m_padding * 10.45f, 0f), null, MyTexts.GetString(text[1]));
				Controls.Add(myGuiControlCheckbox2);
				Controls.Add(control2);
			}
			m_currentPosition.X = x;
<<<<<<< HEAD
			m_currentPosition.Y += array.First((MyGuiControlCheckbox c) => c != null).Size.Y / 2f + m_padding + 0.005f;
=======
			m_currentPosition.Y += Enumerable.First<MyGuiControlCheckbox>((IEnumerable<MyGuiControlCheckbox>)array, (Func<MyGuiControlCheckbox, bool>)((MyGuiControlCheckbox c) => c != null)).Size.Y / 2f + m_padding + 0.005f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return array;
		}

		protected MyGuiControlIndeterminateCheckbox[] AddIndeterminateDuo(MyStringId?[] text, Action<MyGuiControlIndeterminateCheckbox>[] onClick, MyStringId?[] tooltip, CheckStateEnum[] values, bool enabled = true, float maxLabelWidth = float.PositiveInfinity)
		{
			MyGuiControlIndeterminateCheckbox[] array = new MyGuiControlIndeterminateCheckbox[2];
			float x = m_currentPosition.X;
			if (text[0].HasValue)
			{
				MyGuiControlIndeterminateCheckbox myGuiControlIndeterminateCheckbox = new MyGuiControlIndeterminateCheckbox(m_currentPosition, null, tooltip[0].HasValue ? MyTexts.GetString(tooltip[0].Value) : string.Empty);
				myGuiControlIndeterminateCheckbox.PositionX = -0.0435f;
				myGuiControlIndeterminateCheckbox.State = values[0];
				array[0] = myGuiControlIndeterminateCheckbox;
				if (onClick[0] != null)
				{
					myGuiControlIndeterminateCheckbox.IsCheckedChanged = (Action<MyGuiControlIndeterminateCheckbox>)Delegate.Combine(myGuiControlIndeterminateCheckbox.IsCheckedChanged, onClick[0]);
				}
				m_currentPosition.X = myGuiControlIndeterminateCheckbox.PositionX + myGuiControlIndeterminateCheckbox.Size.X / 2f + m_padding / 3f;
				MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(myGuiControlIndeterminateCheckbox.Position - new Vector2(myGuiControlIndeterminateCheckbox.Size.X / 2f + m_padding * 10.45f, 0f), null, MyTexts.GetString(text[0].Value));
<<<<<<< HEAD
				if (maxLabelWidth != float.PositiveInfinity)
				{
					myGuiControlLabel.IsAutoScaleEnabled = true;
					myGuiControlLabel.IsAutoEllipsisEnabled = true;
					myGuiControlLabel.SetMaxWidth(maxLabelWidth - myGuiControlIndeterminateCheckbox.Size.X - m_padding);
				}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myGuiControlIndeterminateCheckbox.Enabled = enabled;
				myGuiControlLabel.Enabled = enabled;
				Controls.Add(myGuiControlIndeterminateCheckbox);
				Controls.Add(myGuiControlLabel);
			}
			if (text[1].HasValue)
			{
				MyGuiControlIndeterminateCheckbox myGuiControlIndeterminateCheckbox2 = new MyGuiControlIndeterminateCheckbox(m_currentPosition, null, tooltip[1].HasValue ? MyTexts.GetString(tooltip[1].Value) : string.Empty);
				myGuiControlIndeterminateCheckbox2.PositionX = 0.262f;
				myGuiControlIndeterminateCheckbox2.State = values[1];
				array[1] = myGuiControlIndeterminateCheckbox2;
				if (onClick[1] != null)
				{
					myGuiControlIndeterminateCheckbox2.IsCheckedChanged = (Action<MyGuiControlIndeterminateCheckbox>)Delegate.Combine(myGuiControlIndeterminateCheckbox2.IsCheckedChanged, onClick[1]);
				}
				m_currentPosition.X = myGuiControlIndeterminateCheckbox2.PositionX + myGuiControlIndeterminateCheckbox2.Size.X / 2f + m_padding / 2f;
				MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(myGuiControlIndeterminateCheckbox2.Position - new Vector2(myGuiControlIndeterminateCheckbox2.Size.X / 2f + m_padding * 10.45f, 0f), null, MyTexts.GetString(text[1].Value));
				if (maxLabelWidth != float.PositiveInfinity)
				{
					myGuiControlLabel2.IsAutoScaleEnabled = true;
<<<<<<< HEAD
					myGuiControlLabel2.IsAutoEllipsisEnabled = true;
					myGuiControlLabel2.SetMaxWidth(maxLabelWidth - myGuiControlIndeterminateCheckbox2.Size.X - m_padding);
=======
					myGuiControlLabel2.SetMaxWidth(maxLabelWidth);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				myGuiControlIndeterminateCheckbox2.Enabled = enabled;
				myGuiControlLabel2.Enabled = enabled;
				Controls.Add(myGuiControlIndeterminateCheckbox2);
				Controls.Add(myGuiControlLabel2);
			}
			m_currentPosition.X = x;
<<<<<<< HEAD
			m_currentPosition.Y += array.First((MyGuiControlIndeterminateCheckbox c) => c != null).Size.Y / 2f + m_padding + 0.005f;
=======
			m_currentPosition.Y += Enumerable.First<MyGuiControlIndeterminateCheckbox>((IEnumerable<MyGuiControlIndeterminateCheckbox>)array, (Func<MyGuiControlIndeterminateCheckbox, bool>)((MyGuiControlIndeterminateCheckbox c) => c != null)).Size.Y / 2f + m_padding + 0.005f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return array;
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (hasFocus)
			{
				m_btnDefault.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_searchButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			return result;
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				SearchClick(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
			{
				DefaultSettingsClick(null);
				DefaultModsClick(null);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT))
			{
				SwitchSelectedTab(right: false);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT))
			{
				SwitchSelectedTab(right: true);
			}
		}

		private void SwitchSelectedTab(bool right)
		{
			bool flag = MyPlatformGameSettings.IsModdingAllowed && MyFakes.ENABLE_WORKSHOP_MODS;
			switch (CurrentPage)
			{
			case SearchPageEnum.Settings:
				if (right)
				{
					CurrentPage = SearchPageEnum.Advanced;
				}
				else if (flag)
				{
					CurrentPage = SearchPageEnum.Mods;
				}
				else
				{
					CurrentPage = SearchPageEnum.Advanced;
				}
				break;
			case SearchPageEnum.Advanced:
				if (right)
				{
					if (flag)
					{
						CurrentPage = SearchPageEnum.Mods;
					}
					else
					{
						CurrentPage = SearchPageEnum.Settings;
					}
				}
				else
				{
					CurrentPage = SearchPageEnum.Settings;
				}
				break;
			case SearchPageEnum.Mods:
				if (right)
				{
					CurrentPage = SearchPageEnum.Settings;
				}
				else
				{
					CurrentPage = SearchPageEnum.Advanced;
				}
				break;
			}
		}
	}
}
