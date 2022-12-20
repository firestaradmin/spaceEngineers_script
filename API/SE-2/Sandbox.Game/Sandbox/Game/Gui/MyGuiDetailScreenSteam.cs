using System;
using System.IO;
using System.Text;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.GameServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiDetailScreenSteam : MyGuiDetailScreenBase
	{
		private MyWorkshopItem m_workshopItem;

		private MyGuiControlCombobox m_sendToCombo;

		public MyGuiDetailScreenSteam(Action<MyGuiControlListbox.Item> callBack, MyGuiControlListbox.Item selectedItem, MyGuiBlueprintScreen parent, string thumbnailTexture, float textScale)
			: base(isTopMostScreen: false, parent, thumbnailTexture, selectedItem, textScale)
		{
			base.callBack = callBack;
			m_workshopItem = (selectedItem.UserData as MyBlueprintItemInfo).Item;
			string text = Path.Combine(MyBlueprintUtils.BLUEPRINT_FOLDER_WORKSHOP, m_workshopItem?.ServiceName, m_workshopItem?.Id.ToString() + MyBlueprintUtils.BLUEPRINT_WORKSHOP_EXTENSION);
			if (File.Exists(text))
			{
				m_loadedPrefab = MyBlueprintUtils.LoadWorkshopPrefab(text, m_workshopItem?.Id, m_workshopItem?.ServiceName, isOldBlueprintScreen: true);
				if (m_loadedPrefab == null)
				{
					m_killScreen = true;
					return;
				}
				string displayName = m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName;
				if (displayName.Length > 40)
				{
					string displayName2 = displayName.Substring(0, 40);
					m_loadedPrefab.ShipBlueprints[0].CubeGrids[0].DisplayName = displayName2;
				}
				RecreateControls(constructor: true);
			}
			else
			{
				m_killScreen = true;
			}
		}

		public override string GetFriendlyName()
		{
			return "MyDetailScreen";
		}

		protected override void CreateButtons()
		{
			Vector2 vector = new Vector2(0.215f, -0.197f) + m_offset;
			Vector2 vector2 = new Vector2(0.13f, 0.045f);
			float num = 0.26f;
			MyBlueprintUtils.CreateButton(this, num, MyTexts.Get(MySpaceTexts.DetailScreen_Button_OpenInWorkshop), OnOpenInWorkshop, enabled: true, textScale: m_textScale, tooltip: MyCommonTexts.ScreenLoadSubscribedWorldBrowseWorkshop).Position = vector;
			num = 0.14f;
			MyGuiControlLabel control = MakeLabel(MyTexts.GetString(MySpaceTexts.DetailScreen_Button_SendToPlayer), vector + new Vector2(-1f, 1.1f) * vector2, m_textScale);
			Controls.Add(control);
			m_sendToCombo = AddCombo(null, null, new Vector2(0.14f, 0.1f));
			m_sendToCombo.Position = vector + new Vector2(-0.082f, 1f) * vector2;
			m_sendToCombo.SetToolTip(MyCommonTexts.Blueprints_PlayersTooltip);
			foreach (MyNetworkClient client in Sync.Clients.GetClients())
			{
				if (client.SteamUserId != Sync.MyId)
				{
					m_sendToCombo.AddItem(Convert.ToInt64(client.SteamUserId), new StringBuilder(client.DisplayName));
				}
			}
			m_sendToCombo.ItemSelected += OnSendToPlayer;
		}

		private void OnSendToPlayer()
		{
			if (m_workshopItem != null)
			{
				ulong selectedKey = (ulong)m_sendToCombo.GetSelectedKey();
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyGuiBlueprintScreen.ShareBlueprintRequest, m_workshopItem.Id, m_blueprintName, selectedKey, MySession.Static.LocalHumanPlayer.DisplayName);
			}
		}

		private void OnOpenInWorkshop(MyGuiControlButton button)
		{
			if (m_workshopItem != null)
			{
				MyGuiSandbox.OpenUrlWithFallback(m_workshopItem.GetItemUrl(), m_workshopItem.ServiceName + " Workshop");
			}
			else
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("Invalid workshop id"), messageText: new StringBuilder("")));
			}
		}
	}
}
