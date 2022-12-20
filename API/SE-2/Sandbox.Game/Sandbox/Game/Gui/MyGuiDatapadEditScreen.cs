using System;
using System.Text;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
	public class MyGuiDatapadEditScreen : MyGuiScreenBase
	{
		private static BitStream m_stream;

		private static Vector2 m_defaultWindowSize;

		private static Vector2 m_windowSizeMulti;

		private MyObjectBuilder_Datapad m_datapad;

		private MyPhysicalInventoryItem m_item;

		private MyInventory m_inventory;

		private MyCharacter m_character;

		private MyGuiControlTextbox m_textboxName;

		private MyGuiControlMultilineEditableText m_textbox;

		private MyGuiControlButton m_okButton;

		private MyGuiControlLabel m_characterCountLabel;

		private MyGuiControlButton m_createGpsCoord;

		public static event Action OnDatapadOpened;

		public MyGuiDatapadEditScreen(MyObjectBuilder_Datapad pad, MyPhysicalInventoryItem item, MyInventory inventory, MyCharacter character)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, m_defaultWindowSize * m_windowSizeMulti)
		{
			m_datapad = pad;
			m_item = item;
			m_inventory = inventory;
			m_character = character;
			RecreateControls(constructor: true);
		}

		static MyGuiDatapadEditScreen()
		{
			m_stream = new BitStream();
			m_defaultWindowSize = new Vector2(0.6f, 0.7f);
			m_windowSizeMulti = new Vector2(1.333f, 1f);
			MyVRage.RegisterExitCallback(delegate
			{
				m_stream.Dispose();
			});
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MySpaceTexts.DatapadEditEcreen_Caption, null, new Vector2(0f, 0.003f));
			Controls.Add(new MyGuiControlLabel(new Vector2(-0.25f, -0.23f) * m_windowSizeMulti, null, MyTexts.GetString(MySpaceTexts.DatapadEditScreen_Name)));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.417f, m_size.Value.Y * 0.385f), m_size.Value.X * 0.834f);
			Controls.Add(myGuiControlSeparatorList);
			Controls.Add(new MyGuiControlLabel(new Vector2(-0.25f, -0.17f) * m_windowSizeMulti, null, MyTexts.GetString(MySpaceTexts.DatapadEditScreen_Content)));
			m_characterCountLabel = new MyGuiControlLabel(new Vector2(-0.25f, 0.28f) * m_windowSizeMulti, null, string.Empty);
			Controls.Add(m_characterCountLabel);
			m_textboxName = new MyGuiControlTextbox(new Vector2(0.05f, -0.23f) * m_windowSizeMulti, (m_datapad != null) ? m_datapad.Name : string.Empty, (m_datapad != null) ? MyObjectBuilder_Datapad.NAME_CHAR_LIMIT : 0);
			m_textboxName.Size += new Vector2(0.08f, 0f);
			m_textboxName.Size *= m_windowSizeMulti;
			Controls.Add(m_textboxName);
			MyGuiControlCompositePanel myGuiControlCompositePanel = new MyGuiControlCompositePanel();
			myGuiControlCompositePanel.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
			myGuiControlCompositePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			myGuiControlCompositePanel.Position = new Vector2(0f, 0.05f) * m_windowSizeMulti;
			myGuiControlCompositePanel.Size = new Vector2(0.5f, 0.4f) * m_windowSizeMulti;
			Controls.Add(myGuiControlCompositePanel);
			m_textbox = new MyGuiControlMultilineEditableText(new Vector2(0f, 0.05f) * m_windowSizeMulti, new Vector2(0.5f, 0.4f) * m_windowSizeMulti, Color.White.ToVector4(), "White", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_textbox.TextWrap = true;
			m_textbox.TextPadding = new MyGuiBorderThickness(0.01f);
			m_textbox.Text = new StringBuilder((m_datapad != null) ? m_datapad.Data : string.Empty);
			m_textbox.TextChanged += MultilineTextChanged;
			if (MyPlatformGameSettings.IsMultilineEditableByGamepad)
			{
				m_textbox.GamepadHelpTextId = MyCommonTexts.Gamepad_Help_MultiLineTextbox;
			}
			Controls.Add(m_textbox);
			m_okButton = new MyGuiControlButton(new Vector2(0.186f, 0.3f) * m_windowSizeMulti, MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: OkButtonClicked, toolTip: MyTexts.GetString(MySpaceTexts.ProgrammableBlock_CodeEditor_SaveExit_Tooltip));
			Controls.Add(m_okButton);
			m_createGpsCoord = new MyGuiControlButton(m_okButton.Position - new Vector2(MyGuiConstants.BACK_BUTTON_SIZE.X + 0.02f, 0f), MyGuiControlButtonStyleEnum.Default, MyGuiConstants.BACK_BUTTON_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MySpaceTexts.GUI_Datapad_CreateGPSCoord), onButtonClick: OnCreateGpsCoordClicked, toolTip: MyTexts.GetString(MySpaceTexts.GUI_Datapad_CreateGPSCoord_TTIP));
			Controls.Add(m_createGpsCoord);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton
			{
				Position = new Vector2(0.27f, -0.32f) * m_windowSizeMulti,
				Size = new Vector2(0.045f, 17f / 300f),
				Name = "Close",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				VisualStyle = MyGuiControlButtonStyleEnum.Close,
				ActivateOnMouseRelease = true
			};
			myGuiControlButton.ButtonClicked += CloseButtonClicked;
			Controls.Add(myGuiControlButton);
			MultilineTextChanged(m_textbox);
			_ = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(m_characterCountLabel.PositionX + 0.2f, m_characterCountLabel.PositionY));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.GamepadHelpTextId = MySpaceTexts.DatapadEdit_Help_Screen;
			base.FocusedControl = m_textboxName;
		}

		private void MultilineTextChanged(MyGuiControlMultilineEditableText obj)
		{
			if (m_textbox != null)
			{
				string text = m_textbox.Text.ToString();
				m_stream.ResetWrite();
				m_stream.WritePrefixLengthString(text, 0, Math.Min(text.Length, MyObjectBuilder_Datapad.DATA_CHAR_LIMIT - 1), Encoding.UTF8);
				int num = m_stream.BytePosition - 1;
				m_stream.ResetRead();
				string text2 = m_stream.ReadString();
				if (text2.Length != text.Length)
				{
					m_textbox.Text = m_textbox.Text.Clear().Append(text2);
				}
				m_characterCountLabel.Text = string.Format(MyTexts.GetString(MySpaceTexts.DatapadEditScreen_ContentUsage), num, (m_datapad != null) ? MyObjectBuilder_Datapad.DATA_CHAR_LIMIT : 0);
				if (m_textbox.Text.Length == 0)
				{
					m_createGpsCoord.Visible = false;
					return;
				}
				Vector3D coords = default(Vector3D);
				m_createGpsCoord.Visible = MyGpsCollection.ParseOneGPS(m_textbox.Text.ToString(), new StringBuilder(), ref coords);
			}
		}

		private void OkButtonClicked(MyGuiControlButton button)
		{
			if (m_datapad != null)
			{
				string text = m_textboxName.Text;
				string arg = m_textbox.Text.ToString();
				if (m_inventory == null || m_inventory.Owner == null)
				{
					CloseScreen();
					return;
				}
				int arg2 = -1;
				for (int i = 0; i < m_inventory.Owner.InventoryCount; i++)
				{
					if (m_inventory.Owner.GetInventory(i) == m_inventory)
					{
						arg2 = i;
						break;
					}
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyInventory.ModifyDatapad, m_inventory.Owner.EntityId, arg2, m_item.ItemId, text, arg);
			}
			CloseScreen();
		}

		private void CloseButtonClicked(MyGuiControlButton button)
		{
			CloseScreen();
		}

		private void OnCreateGpsCoordClicked(MyGuiControlButton button)
		{
			if (m_textbox.Text.Length != 0)
			{
				MySession.Static.Gpss.ScanText(m_textbox.Text.ToString(), MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewFromClipboard_Desc));
				m_createGpsCoord.Enabled = false;
			}
		}

		protected override void OnShow()
		{
			base.OnShow();
			MyGuiDatapadEditScreen.OnDatapadOpened?.Invoke();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiDatapadEditScreen";
		}

		public override bool Update(bool hasFocus)
		{
			m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			m_createGpsCoord.Visible = !MyInput.Static.IsJoystickLastUsed;
			return base.Update(hasFocus);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				OkButtonClicked(null);
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
			{
				OnCreateGpsCoordClicked(null);
			}
		}
	}
}
