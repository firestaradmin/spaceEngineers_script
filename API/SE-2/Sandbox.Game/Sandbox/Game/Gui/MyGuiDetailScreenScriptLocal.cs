using System;
using System.IO;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiDetailScreenScriptLocal : MyGuiBlueprintScreenBase
	{
		public bool WasPublished;

		protected MyGuiControlMultilineText m_textField;

		protected float m_textScale;

		protected Vector2 m_offset = new Vector2(-0.01f, 0f);

		protected int m_maxNameLenght = 40;

		protected MyGuiControlImage m_thumbnailImage;

		private MyGuiBlueprintTextDialog m_dialog;

		private MyBlueprintItemInfo m_selectedItem;

		private MyGuiIngameScriptsPage m_parent;

		protected MyGuiControlMultilineText m_descriptionField;

		private Action<MyBlueprintItemInfo> m_callBack;

		public MyGuiDetailScreenScriptLocal(Action<MyBlueprintItemInfo> callBack, MyBlueprintItemInfo selectedItem, MyGuiIngameScriptsPage parent, float textScale)
			: base(new Vector2(0.5f, 0.5f), new Vector2(0.778f, 0.594f), MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, isTopMostScreen: true)
		{
			WasPublished = false;
			m_callBack = callBack;
			m_parent = parent;
			m_selectedItem = selectedItem;
			m_textScale = textScale;
			m_localRoot = Path.Combine(MyFileSystem.UserDataPath, "IngameScripts", "local");
			base.CanBeHidden = true;
			base.CanHideOthers = true;
			RecreateControls(constructor: true);
			base.EnabledBackgroundFade = true;
		}

		protected void CreateButtons()
		{
			Vector2 vector = new Vector2(0.148f, -0.197f) + m_offset;
			Vector2 vector2 = new Vector2(0.132f, 0.045f);
			float num = 0.13f;
			if (m_selectedItem.Item == null)
			{
				MyBlueprintUtils.CreateButton(this, num, MyTexts.Get(MySpaceTexts.ProgrammableBlock_ButtonRename), OnRename, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.Scripts_RenameTooltip).Position = vector;
				MyGuiControlButton obj = MyBlueprintUtils.CreateButton(this, num, MyTexts.Get(MyCommonTexts.LoadScreenButtonPublish), OnPublish, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.Scripts_PublishTooltip);
				obj.Position = vector + new Vector2(1f, 0f) * vector2;
				obj.Enabled = MyFakes.ENABLE_WORKSHOP_PUBLISH;
				MyBlueprintUtils.CreateButton(this, num, MyTexts.Get(MyCommonTexts.LoadScreenButtonDelete), OnDelete, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.Scripts_DeleteTooltip).Position = vector + new Vector2(0f, 1f) * vector2;
				MyBlueprintUtils.CreateButton(this, num, MyTexts.Get(MyCommonTexts.ScreenLoadSubscribedWorldBrowseWorkshop), OnOpenWorkshop, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.Scripts_OpenWorkshopTooltip).Position = vector + new Vector2(1f, 1f) * vector2;
			}
			else
			{
				MyBlueprintUtils.CreateButton(this, num * 2f, MyTexts.Get(MyCommonTexts.ScreenLoadSubscribedWorldOpenInWorkshop), OnOpenInWorkshop, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.Scripts_OpenWorkshopTooltip).Position = new Vector2(0.215f, -0.197f) + m_offset;
			}
		}

		protected void CreateDescription()
		{
			Vector2 vector = new Vector2(-0.325f, -0.005f) + m_offset;
			Vector2 vector2 = new Vector2(0.67f, 0.155f);
			Vector2 vector3 = new Vector2(0.005f, -0.04f);
			AddCompositePanel(MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER, vector, vector2, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_descriptionField = CreateMultilineText(offset: vector + vector3, textScale: m_textScale, size: vector2 - (vector3 + new Vector2(0f, 0.045f)));
			RefreshDescriptionField();
		}

		protected void RefreshDescriptionField()
		{
			if (m_descriptionField != null)
			{
				m_descriptionField.Clear();
				if (m_selectedItem.Data.Description != null)
				{
					m_descriptionField.AppendText(m_selectedItem.Data.Description);
				}
			}
		}

		protected void CreateTextField()
		{
			Vector2 vector = new Vector2(-0.325f, -0.2f) + m_offset;
			Vector2 vector2 = new Vector2(0.175f, 0.175f);
			Vector2 vector3 = new Vector2(0.005f, -0.04f);
			AddCompositePanel(MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER, vector, vector2, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_textField = new MyGuiControlMultilineText();
			m_textField = CreateMultilineText(offset: vector + vector3, textScale: m_textScale, size: vector2 - vector3);
			RefreshTextField();
		}

		protected void RefreshTextField()
		{
			if (m_textField != null)
			{
				string text = m_selectedItem.Data.Name;
				if (text.Length > 25)
				{
					text = text.Substring(0, 25) + "...";
				}
				m_textField.Clear();
				m_textField.AppendText("Name: " + text);
				m_textField.AppendLine();
				m_textField.AppendText("Type: IngameScript");
			}
		}

		public override string GetFriendlyName()
		{
			return "MyDetailScreenScripts";
		}

		private void OnRename(MyGuiControlButton button)
		{
			HideScreen();
			m_dialog = new MyGuiBlueprintTextDialog(new Vector2(0.5f, 0.5f), delegate(string result)
			{
				if (result != null)
				{
					m_parent.ChangeName(result);
				}
			}, caption: MyTexts.GetString(MySpaceTexts.ProgrammableBlock_NewScriptName), defaultName: m_selectedItem.Data.Name, maxLenght: m_maxNameLenght, textBoxWidth: 0.3f);
			MyScreenManager.AddScreen(m_dialog);
		}

		private void OnDelete(MyGuiControlButton button)
		{
			m_parent.OnDelete(button);
			CloseScreen();
		}

		private void OnPublish(MyGuiControlButton button)
		{
			WorkshopId[] modInfo = MyWorkshop.GetWorkshopIdFromLocalBlueprint(m_selectedItem.Data.Name, new WorkshopId(m_selectedItem.Item?.Id ?? 0, m_selectedItem.Item?.ServiceName));
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.LoadScreenButtonPublish), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_PublishScriptDialogText), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum val)
			{
				if (val == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					WasPublished = true;
					string fullPath = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_LOCAL, m_selectedItem.Data.Name);
					MyWorkshop.PublishIngameScriptAsync(fullPath, m_selectedItem.Data.Name, m_selectedItem.Data.Description ?? "", modInfo, null, MyPublishedFileVisibility.Public, delegate(bool success, MyGameServiceCallResult result, string resultServiceName, MyWorkshopItem[] publishedFiles)
					{
						if (publishedFiles.Length != 0)
						{
							MyWorkshop.GenerateModInfo(fullPath, publishedFiles, Sync.MyId);
						}
						MyWorkshop.ReportPublish(publishedFiles, result, resultServiceName);
					});
				}
			}));
		}

		private void OnOpenWorkshop(MyGuiControlButton button)
		{
			MyWorkshop.OpenWorkshopBrowser(MySteamConstants.TAG_SCRIPTS);
		}

		private void OnOpenInWorkshop(MyGuiControlButton button)
		{
			MyGuiSandbox.OpenUrlWithFallback(m_selectedItem.Item.GetItemUrl(), m_selectedItem.Item.ServiceName + " Workshop");
		}

		protected void OnCloseButton(MyGuiControlButton button)
		{
			CloseScreen();
		}

		protected void CallResultCallback(MyBlueprintItemInfo val)
		{
			UnhideScreen();
			m_callBack(val);
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			if (m_dialog != null)
			{
				m_dialog.CloseScreen();
			}
			CallResultCallback(m_selectedItem);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenCaptionBlueprintDetails, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.86f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.86f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.86f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.86f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiControlButton obj = MyBlueprintUtils.CreateButton(this, 1f, MyTexts.Get(MySpaceTexts.DetailScreen_Button_Close), OnCloseButton, enabled: true, textScale: m_textScale, tooltip: MySpaceTexts.ToolTipNewsletter_Close);
			obj.Position = new Vector2(0f, m_size.Value.Y / 2f - 0.097f);
			obj.VisualStyle = MyGuiControlButtonStyleEnum.Default;
			m_thumbnailImage = new MyGuiControlImage
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK
			};
			m_thumbnailImage.SetPadding(new MyGuiBorderThickness(2f, 2f, 2f, 2f));
			m_thumbnailImage.BorderEnabled = true;
			m_thumbnailImage.BorderSize = 1;
			m_thumbnailImage.BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f);
			m_thumbnailImage.Position = new Vector2(-0.035f, -0.112f) + m_offset;
			m_thumbnailImage.Size = new Vector2(0.2f, 0.175f);
			Controls.Add(m_thumbnailImage);
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK
			};
			myGuiControlImage.SetPadding(new MyGuiBorderThickness(2f, 2f, 2f, 2f));
			myGuiControlImage.SetTexture((m_selectedItem.Item == null) ? MyGuiConstants.TEXTURE_ICON_BLUEPRINTS_LOCAL.Normal : MyGuiConstants.GetWorkshopIcon(m_selectedItem.Item).Normal);
			myGuiControlImage.Position = new Vector2(-0.035f, -0.112f) + m_offset;
			myGuiControlImage.Size = new Vector2(0.027f, 0.029f);
			Controls.Add(myGuiControlImage);
			CreateTextField();
			CreateDescription();
			CreateButtons();
		}

		protected MyGuiControlMultilineText CreateMultilineText(Vector2? size = null, Vector2? offset = null, float textScale = 1f, bool selectable = false)
		{
			Vector2 vector = size ?? base.Size ?? new Vector2(0.5f, 0.5f);
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(m_currentPosition + vector / 2f + (offset ?? Vector2.Zero), vector, null, "Debug", m_scale * textScale, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, selectable);
			Controls.Add(myGuiControlMultilineText);
			return myGuiControlMultilineText;
		}
	}
}
