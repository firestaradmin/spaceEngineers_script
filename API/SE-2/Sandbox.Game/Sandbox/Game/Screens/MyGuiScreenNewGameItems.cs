using System.Collections.Generic;
using System.Linq;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenNewGameItems : MyGuiScreenBase
	{
		private List<MyGameInventoryItem> m_items;

		private MyGuiControlLabel m_itemName;

		private MyGuiControlImage m_itemBackground;

		private MyGuiControlImage m_itemImage;

		private MyGuiControlButton m_okButton;

		public MyGuiScreenNewGameItems(List<MyGameInventoryItem> newItems)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.41f, 0.4f), isTopMostScreen: true)
		{
			m_items = newItems;
			MyCueId cueId = MySoundPair.GetCueId("ArcNewItemImpact");
			MyAudio.Static.PlaySound(cueId);
			base.EnabledBackgroundFade = true;
			base.CloseButtonEnabled = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenCaptionClaimGameItem, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.74f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.74f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(0f, -0.096f), null, MyTexts.GetString(MyCommonTexts.ScreenCaptionNewItem), Vector4.One, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			myGuiControlLabel.Font = "White";
			Elements.Add(myGuiControlLabel);
			m_itemName = new MyGuiControlLabel(new Vector2(0f, 0.03f), null, "Item Name", Vector4.One, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			m_itemName.Font = "Blue";
			Controls.Add(m_itemName);
			m_itemBackground = new MyGuiControlImage(size: new Vector2(0.07f, 0.09f), position: new Vector2(0f, -0.025f), backgroundColor: null, backgroundTexture: null, textures: new string[1] { "Textures\\GUI\\blank.dds" });
			m_itemBackground.Margin = new Thickness(0.005f);
			Controls.Add(m_itemBackground);
			m_itemImage = new MyGuiControlImage(size: new Vector2(0.06f, 0.08f), position: new Vector2(0f, -0.025f));
			m_itemImage.Margin = new Thickness(0.005f);
			Controls.Add(m_itemImage);
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2(0f, 0.085f), null, MyTexts.GetString(MyCommonTexts.ScreenNewItemVisit), Vector4.One, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			myGuiControlLabel2.Font = "White";
			Elements.Add(myGuiControlLabel2);
			m_okButton = new MyGuiControlButton(new Vector2(0f, 0.168f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok));
			m_okButton.ButtonClicked += OnOkButtonClick;
			m_okButton.GamepadHelpTextId = MyStringId.NullOrEmpty;
			Controls.Add(m_okButton);
			LoadFirstItem();
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2(m_okButton.PositionX, m_okButton.Position.Y - minSizeGui.Y / 2f));
			myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
			myGuiControlLabel3.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel3);
			base.GamepadHelpTextId = MySpaceTexts.ClaimSkin_Help_Screen;
			UpdateGamepadHelp(null);
		}

		private void LoadFirstItem()
		{
<<<<<<< HEAD
			MyGameInventoryItem myGameInventoryItem = m_items.FirstOrDefault();
=======
			MyGameInventoryItem myGameInventoryItem = Enumerable.FirstOrDefault<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)m_items);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (myGameInventoryItem != null)
			{
				m_itemName.Text = myGameInventoryItem.ItemDefinition.Name;
				m_itemBackground.ColorMask = (string.IsNullOrEmpty(myGameInventoryItem.ItemDefinition.BackgroundColor) ? Vector4.One : ColorExtensions.HexToVector4(myGameInventoryItem.ItemDefinition.BackgroundColor));
				string[] array = new string[1] { "Textures\\GUI\\Blank.dds" };
				if (!string.IsNullOrEmpty(myGameInventoryItem.ItemDefinition.IconTexture))
				{
					array[0] = myGameInventoryItem.ItemDefinition.IconTexture;
				}
				m_itemImage.SetTextures(array);
			}
		}

		private void OnOkButtonClick(MyGuiControlButton obj)
		{
<<<<<<< HEAD
			if (m_items.Count() > 1)
=======
			if (Enumerable.Count<MyGameInventoryItem>((IEnumerable<MyGameInventoryItem>)m_items) > 1)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_items.RemoveAt(0);
				LoadFirstItem();
			}
			else
			{
				CloseScreen();
			}
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenNewGameItems";
		}

		public override bool Update(bool hasFocus)
		{
			m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			return base.Update(hasFocus);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				OnOkButtonClick(null);
			}
		}
	}
}
