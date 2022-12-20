using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	internal class MyGuiScreenWorkshopTags : MyGuiScreenBase
	{
		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

		private static readonly List<MyGuiControlCheckbox> m_cbTags = new List<MyGuiControlCheckbox>();

		private static readonly List<MyGuiControlCheckbox> m_cbServices = new List<MyGuiControlCheckbox>();

		private readonly Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]> m_callback;

		private readonly string m_typeTag;

		private static string m_ugcConsentServiceName;

		private static MyGuiScreenWorkshopTags Static;

		private const int TAGS_MAX_LENGTH = 128;

		private static Dictionary<string, MyStringId> m_activeTags;

		public MyGuiScreenWorkshopTags(string typeTag, MyWorkshop.Category[] categories, string[] tags, Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]> callback)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 0.7633588f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			Static = this;
			m_typeTag = typeTag ?? "";
			m_activeTags = new Dictionary<string, MyStringId>(categories.Length);
			for (int i = 0; i < categories.Length; i++)
			{
				MyWorkshop.Category category = categories[i];
				m_activeTags.Add(category.Id, category.LocalizableName);
			}
			m_callback = callback;
			base.EnabledBackgroundFade = true;
			base.CanHideOthers = true;
			RecreateControls(constructor: true);
			SetSelectedTags(tags);
			m_okButton.Enabled = false;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenCaptionWorkshopTags, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			Vector2 start = new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f);
			myGuiControlSeparatorList2.AddHorizontal(start, m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList2);
			Vector2 vector = new Vector2(-0.125f, -0.3f);
			Vector2 vector2 = new Vector2(0f, 0.05f);
			m_cbTags.Clear();
			m_cbServices.Clear();
			int num = 0;
			Vector2 vector3 = new Vector2(0.125f, vector.Y) + vector2;
			new Vector2(-0.05f, 0f);
			Vector2 vector4 = new Vector2(-0.03f, 0f);
			foreach (KeyValuePair<string, MyStringId> activeTag in m_activeTags)
			{
				num++;
				if (num == 11)
				{
					vector = vector3;
				}
				if (num == 1)
				{
					vector += vector2;
					AddLabel(vector + vector4, MyCommonTexts.WorkshopTagsHeader);
					num++;
				}
				AddLabeledCheckbox(vector += vector2, activeTag.Key, activeTag.Value);
				if (m_typeTag == "mod")
				{
					string text = activeTag.Key.Replace(" ", string.Empty);
					Path.Combine(MyFileSystem.ContentPath, "Textures", "GUI", "Icons", "buttons", text + ".dds");
				}
			}
			string[] uGCNamesList = MyGameService.GetUGCNamesList();
			if (uGCNamesList.Length != 0)
			{
				vector = ((num <= 0 || num >= 11) ? (vector3 + vector2 * (11 - uGCNamesList.Length - 3)) : vector3);
				vector += vector2;
				AddLabel(vector + vector4, MyCommonTexts.WorkshopTagsServicesHeader);
				string[] array = uGCNamesList;
				foreach (string text2 in array)
				{
					AddLabeledCheckbox(vector += vector2, text2, MyStringId.GetOrCompute(text2), isTag: false);
				}
			}
			vector += vector2;
			Controls.Add(m_okButton = MakeButton(vector += vector2, MyCommonTexts.Ok, MySpaceTexts.ToolTipNewsletter_Ok, OnOkClick, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER));
			Controls.Add(m_cancelButton = MakeButton(vector, MyCommonTexts.Cancel, MySpaceTexts.ToolTipOptionsSpace_Cancel, OnCancelClick, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER));
			Vector2 vector5 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 vector6 = (m_size.Value / 2f - vector5) * new Vector2(0f, 1f);
			float num2 = 25f;
			m_okButton.Position = vector6 + new Vector2(0f - num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_okButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_cancelButton.Position = vector6 + new Vector2(num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			m_cancelButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(start.X, m_okButton.Position.Y - minSizeGui.Y / 2f));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.CloseButtonEnabled = true;
			base.GamepadHelpTextId = MySpaceTexts.WorkshopTagsScreen_Help_Screen;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenWorkshopTags";
		}

		private void AddLabel(Vector2 position, MyStringId text)
		{
			MyGuiControlLabel control = MakeLabel(position, text);
			Controls.Add(control);
		}

		private MyGuiControlCheckbox AddLabeledCheckbox(Vector2 position, string tag, MyStringId text, bool isTag = true)
		{
			MyGuiControlLabel control = MakeLabel(position, text);
			MyGuiControlCheckbox checkbox = MakeCheckbox(position, text);
			m_ugcConsentServiceName = null;
			Controls.Add(control);
			Controls.Add(checkbox);
			checkbox.UserData = tag;
			if (isTag)
			{
				m_cbTags.Add(checkbox);
			}
			else
			{
				MyGuiControlCheckbox myGuiControlCheckbox = checkbox;
				myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, (Action<MyGuiControlCheckbox>)delegate
				{
					if (checkbox.IsChecked && !MyGameService.WorkshopService.GetAggregate(tag).IsConsentGiven)
					{
						checkbox.IsChecked = false;
						m_ugcConsentServiceName = tag;
						MyModIoConsentViewModel viewModel = new MyModIoConsentViewModel(UGCConsentGained);
						ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen(viewModel);
					}
				});
				if (checkbox.IsChecked)
				{
					checkbox.IsChecked = false;
				}
				m_cbServices.Add(checkbox);
			}
			return checkbox;
		}

		private void UGCConsentGained()
		{
			foreach (MyGuiControlCheckbox cbService in m_cbServices)
			{
				if (cbService.UserData.ToString() == m_ugcConsentServiceName)
				{
					cbService.IsChecked = true;
					break;
				}
			}
		}

		private MyGuiControlImage AddIcon(Vector2 position, string texture, Vector2 size)
		{
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage
			{
				Position = position,
				Size = size
			};
			myGuiControlImage.SetTexture(texture);
			Controls.Add(myGuiControlImage);
			return myGuiControlImage;
		}

		private MyGuiControlLabel MakeLabel(Vector2 position, MyStringId text)
		{
			return new MyGuiControlLabel(position, null, MyTexts.GetString(text));
		}

		private MyGuiControlCheckbox MakeCheckbox(Vector2 position, MyStringId tooltip)
		{
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(position, null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnCheckboxChanged));
			return myGuiControlCheckbox;
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, MyStringId toolTip, Action<MyGuiControlButton> onClick, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
		{
			Vector2? position2 = position;
			StringBuilder text2 = MyTexts.Get(text);
			string @string = MyTexts.GetString(toolTip);
			return new MyGuiControlButton(position2, MyGuiControlButtonStyleEnum.Default, null, null, originAlign, @string, text2, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, onClick);
		}

		private void OnCheckboxChanged(MyGuiControlCheckbox obj)
		{
			if (obj != null)
			{
				if (obj.IsChecked && Static.GetSelectedTagsLength() >= 128)
				{
					obj.IsChecked = false;
				}
<<<<<<< HEAD
				bool flag = m_cbServices.Any((MyGuiControlCheckbox sc) => sc.IsChecked);
=======
				bool flag = Enumerable.Any<MyGuiControlCheckbox>((IEnumerable<MyGuiControlCheckbox>)m_cbServices, (Func<MyGuiControlCheckbox, bool>)((MyGuiControlCheckbox sc) => sc.IsChecked));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (m_okButton != null)
				{
					m_okButton.Enabled = flag;
				}
				base.GamepadHelpTextId = (flag ? MySpaceTexts.WorkshopTagsScreen_Help_Screen : MySpaceTexts.WorkshopTagsScreen_HelpNoOk_Screen);
				UpdateGamepadHelp(base.FocusedControl);
			}
		}

		private void OnOkClick(MyGuiControlButton obj)
		{
			if (m_okButton.Enabled)
			{
				CloseScreen();
				m_callback(MyGuiScreenMessageBox.ResultEnum.YES, GetSelectedTags(), GetSelectedServiceNames());
			}
		}

		private void OnCancelClick(MyGuiControlButton obj)
		{
			CloseScreen();
			m_callback(MyGuiScreenMessageBox.ResultEnum.CANCEL, GetSelectedTags(), GetSelectedServiceNames());
		}

		protected override void Canceling()
		{
			base.Canceling();
			m_callback(MyGuiScreenMessageBox.ResultEnum.CANCEL, GetSelectedTags(), GetSelectedServiceNames());
		}

		private string[] GetSelectedServiceNames()
		{
			if (m_cbServices.Count == 0)
			{
				return new string[1] { MyGameService.GetDefaultUGC().ServiceName };
			}
			List<string> list = new List<string>();
			foreach (MyGuiControlCheckbox cbService in m_cbServices)
			{
				if (cbService.IsChecked)
				{
					list.Add((string)cbService.UserData);
				}
			}
			return list.ToArray();
		}

		public int GetSelectedTagsLength()
		{
			int num = m_typeTag.Length;
			foreach (MyGuiControlCheckbox cbTag in m_cbTags)
			{
				if (cbTag.IsChecked)
				{
					num += ((string)cbTag.UserData).Length;
				}
			}
			return num;
		}

		public string[] GetSelectedTags()
		{
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(m_typeTag))
			{
				list.Add(m_typeTag);
			}
			foreach (MyGuiControlCheckbox cbTag in m_cbTags)
			{
				if (cbTag.IsChecked)
				{
					list.Add((string)cbTag.UserData);
				}
			}
			return list.ToArray();
		}

		public void SetSelectedTags(string[] tags)
		{
			if (tags == null)
			{
				return;
			}
			foreach (string text in tags)
			{
				foreach (MyGuiControlCheckbox cbTag in m_cbTags)
				{
					if (text.Equals((string)cbTag.UserData, StringComparison.InvariantCultureIgnoreCase))
					{
						cbTag.IsChecked = true;
					}
				}
			}
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (!receivedFocusInThisUpdate)
			{
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
				{
					OnOkClick(null);
				}
				m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
		}
	}
}
