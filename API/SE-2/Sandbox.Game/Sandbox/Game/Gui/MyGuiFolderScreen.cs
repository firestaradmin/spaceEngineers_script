using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiFolderScreen : MyGuiScreenBase
	{
		private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 0.6f);

		/// <summary>
		/// Function that will be called with results of selection
		/// param1 - string - new path that was selected in folder screen, initialPath if path selection is canceled
		/// param2 - bool - true if path selection was confirmed, false on cancellation
		/// </summary>
		private Action<bool, string> m_onFinishedAction;

		private bool m_visibleFolderSelect;

<<<<<<< HEAD
		/// <summary>
		/// Function to check directory is in fact an item and should be displayed as such instead of directory (such as blueprint is directory that contains specific data)
		/// param1 - path to directory
		/// return - true if directory is item
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private Func<string, bool> m_isItem;

		private string m_rootPath = string.Empty;

		private string m_pathLocalInitial = string.Empty;

		private string m_pathLocalCurrent = string.Empty;

		private MyGuiControlLabel m_pathLabel;

		private MyGuiControlListbox m_fileList;

		private MyGuiControlButton m_buttonOk;

		private MyGuiControlButton m_buttonRefresh;

		private MyGuiControlImage m_refreshButtonIcon;

		public MyGuiFolderScreen(bool hideOthers, Action<bool, string> OnFinished, string rootPath, string localPath, Func<string, bool> isItem = null, bool visibleFolderSelect = false)
			: base(new Vector2(0.5f, 0.5f), size: SCREEN_SIZE, backgroundColor: MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity)
		{
			m_visibleFolderSelect = visibleFolderSelect;
			if (OnFinished == null)
			{
				CloseScreen();
			}
			base.CanHideOthers = hideOthers;
			m_onFinishedAction = OnFinished;
			m_rootPath = rootPath;
			m_pathLocalCurrent = (m_pathLocalInitial = localPath);
			if (isItem != null)
			{
				m_isItem = isItem;
			}
			else
			{
				m_isItem = IsItem_Default;
			}
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			Vector2 position = new Vector2(0f, 0.23f);
			Vector2 position2 = new Vector2(0.15f, 0.23f);
			Vector2 position3 = new Vector2(0f, 0.02f);
			Vector2 value = new Vector2(-0.143f, -0.2f);
			Vector2 size = new Vector2(0.143f, 0.035f);
			Vector2 size2 = new Vector2(0.026f, 0.035f);
			Vector2 size3 = new Vector2(0.32f, 0.38f);
			Vector2 value2 = new Vector2(0.5f, 0.5f);
			m_fileList = new MyGuiControlListbox(null, MyGuiControlListboxStyleEnum.Blueprints);
			m_fileList.Position = position3;
			m_fileList.Size = size3;
			m_fileList.ItemDoubleClicked += OnItemDoubleClick;
			m_fileList.ItemClicked += OnItemClick;
			m_fileList.VisibleRowsCount = 11;
			Controls.Add(m_fileList);
			AddCaption(MySpaceTexts.ScreenFolders_Caption);
			m_buttonOk = CreateButton(size, MyTexts.Get(MySpaceTexts.ScreenFolders_ButOpen), OnOk, enabled: true, MySpaceTexts.ScreenFolders_Tooltip_Open);
			m_buttonOk.Position = position;
			m_buttonOk.ShowTooltipWhenDisabled = true;
			m_buttonRefresh = CreateButton(size2, null, OnRefresh, enabled: true, MySpaceTexts.ScreenFolders_Tooltip_Refresh);
			m_buttonRefresh.Position = position2;
			m_buttonRefresh.ShowTooltipWhenDisabled = true;
			m_pathLabel = new MyGuiControlLabel(value, value2);
			Controls.Add(m_pathLabel);
			UpdatePathLabel();
			m_refreshButtonIcon = CreateButtonIcon(m_buttonRefresh, "Textures\\GUI\\Icons\\Blueprints\\Refresh.png");
			base.CloseButtonEnabled = true;
			RepopulateList();
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(m_fileList.PositionX, m_buttonOk.PositionY + minSizeGui.Y / 2f));
			myGuiControlLabel.OriginAlign = m_fileList.OriginAlign;
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.GamepadHelpTextId = MySpaceTexts.FolderScreen_Help_Screen;
		}

		public void UpdatePathLabel()
		{
			string text = "./" + BuildNewPath();
			if (text.Length > 40)
			{
				m_pathLabel.Text = text.Substring(text.Length - 41, 40);
			}
			else
			{
				m_pathLabel.Text = text;
			}
		}

		private static bool IsItem_Default(string path)
		{
			return false;
		}

		private void RepopulateList()
		{
			((Collection<MyGuiControlListbox.Item>)(object)m_fileList.Items).Clear();
			List<MyGuiControlListbox.Item> list = new List<MyGuiControlListbox.Item>();
			List<MyGuiControlListbox.Item> list2 = new List<MyGuiControlListbox.Item>();
			string text = Path.Combine(m_rootPath, m_pathLocalCurrent);
			if (!Directory.Exists(text))
			{
				return;
			}
			string[] directories = Directory.GetDirectories(text);
			List<string> list3 = new List<string>();
			string[] array = directories;
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[1] { '\\' });
				list3.Add(array2[array2.Length - 1]);
			}
			for (int j = 0; j < list3.Count; j++)
			{
				if (m_isItem(directories[j]))
				{
					MyFileItem myFileItem = new MyFileItem
					{
						Type = MyFileItemType.File,
						Name = list3[j],
						Path = directories[j]
					};
					string normal = MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_FOLDER.Normal;
<<<<<<< HEAD
					StringBuilder text = new StringBuilder(list3[j]);
=======
					StringBuilder text2 = new StringBuilder(list3[j]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					string toolTip = directories[j];
					object userData = myFileItem;
					MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(text2, toolTip, normal, userData);
					list2.Add(item);
				}
				else
				{
					MyFileItem myFileItem = new MyFileItem
					{
						Type = MyFileItemType.Directory,
						Name = list3[j],
						Path = directories[j]
					};
					string normal = MyGuiConstants.TEXTURE_ICON_MODS_LOCAL.Normal;
					StringBuilder text3 = new StringBuilder(list3[j]);
					string toolTip2 = directories[j];
					object userData = myFileItem;
					MyGuiControlListbox.Item item2 = new MyGuiControlListbox.Item(text3, toolTip2, normal, userData);
					list.Add(item2);
				}
			}
			if (!string.IsNullOrEmpty(m_pathLocalCurrent))
			{
				MyFileItem myFileItem2 = new MyFileItem
				{
					Type = MyFileItemType.Directory,
					Name = string.Empty,
					Path = string.Empty
				};
				StringBuilder text4 = new StringBuilder("[..]");
				string pathLocalCurrent = m_pathLocalCurrent;
				object userData = myFileItem2;
				MyGuiControlListbox.Item item3 = new MyGuiControlListbox.Item(text4, pathLocalCurrent, MyGuiConstants.TEXTURE_ICON_MODS_LOCAL.Normal, userData);
				m_fileList.Add(item3);
			}
			foreach (MyGuiControlListbox.Item item4 in list)
			{
				m_fileList.Add(item4);
			}
			foreach (MyGuiControlListbox.Item item5 in list2)
			{
				m_fileList.Add(item5);
			}
			UpdatePathLabel();
			m_fileList.SelectedItems.Clear();
		}

		protected MyGuiControlButton CreateButton(Vector2 size, StringBuilder text, Action<MyGuiControlButton> onClick, bool enabled = true, MyStringId? tooltip = null, float textScale = 1f)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, text, 0.8f * MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, onClick);
			myGuiControlButton.TextScale = textScale;
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			myGuiControlButton.Size = size;
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(tooltip.Value);
			}
			Controls.Add(myGuiControlButton);
			return myGuiControlButton;
		}

		private MyGuiControlImage CreateButtonIcon(MyGuiControlButton butt, string texture)
		{
			float num = 0.95f * Math.Min(butt.Size.X, butt.Size.Y);
			MyGuiControlImage icon = new MyGuiControlImage(size: new Vector2(num * 0.75f, num), position: butt.Position + new Vector2(-0.0016f, 0.015f), backgroundColor: null, backgroundTexture: null, textures: new string[1] { texture });
			Controls.Add(icon);
			butt.HighlightChanged += delegate(MyGuiControlBase x)
			{
				icon.ColorMask = (x.HasHighlight ? MyGuiConstants.HIGHLIGHT_TEXT_COLOR : Vector4.One);
			};
			return icon;
		}

		public string BuildNewPath(bool selectVisible = false)
		{
			string text = "";
			if (m_fileList.SelectedItems.Count != 1 || selectVisible)
			{
				return m_pathLocalCurrent;
			}
			MyFileItem myFileItem = (MyFileItem)m_fileList.SelectedItems[0].UserData;
			if (myFileItem.Type != MyFileItemType.Directory)
			{
				return m_pathLocalCurrent;
			}
			if (string.IsNullOrEmpty(myFileItem.Path))
			{
				string[] array = m_pathLocalCurrent.Split(new char[1] { Path.DirectorySeparatorChar });
				if (array.Length > 1)
				{
					array[array.Length - 1] = string.Empty;
					return Path.Combine(array);
				}
				return string.Empty;
			}
			return Path.Combine(m_pathLocalCurrent, myFileItem.Name);
		}

		private void OnItemClick(MyGuiControlListbox list)
		{
			if (list.SelectedItems != null && list.SelectedItems.Count != 0)
			{
				_ = list.SelectedItems[0];
				UpdatePathLabel();
			}
		}

		private void OnItemDoubleClick(MyGuiControlListbox list)
		{
			if (list.SelectedItems.Count > 0)
			{
				_ = list.SelectedItems[0];
				m_pathLocalCurrent = BuildNewPath();
				RepopulateList();
			}
		}

		private void OnRefresh(MyGuiControlButton button)
		{
			RecreateControls(constructor: false);
		}

		private void OnOk(MyGuiControlButton button)
		{
			m_onFinishedAction(arg1: true, BuildNewPath(m_visibleFolderSelect));
			CloseScreen();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiFolderScreen";
		}

		public override bool Update(bool hasFocus)
		{
			m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_buttonRefresh.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_refreshButtonIcon.Visible = !MyInput.Static.IsJoystickLastUsed;
			return base.Update(hasFocus);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				OnOk(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
			{
				OnRefresh(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT))
			{
				OnItemDoubleClick(m_fileList);
			}
		}
	}
}
