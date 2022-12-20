using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Terminal
{
	internal class MyTerminalPropertiesController
	{
		private enum MyCubeGridConnectionStatus
		{
			PhysicallyConnected,
			Connected,
			OutOfBroadcastingRange,
			OutOfReceivingRange,
			Me,
			IsPreviewGrid
		}

		private enum MyRefuseReason
		{
			NoRemoteControl,
			NoMainRemoteControl,
			NoStableConnection,
			NoOwner,
			NoProblem,
			PlayerBroadcastOff,
			Forbidden
		}

		private struct UserData
		{
			public long GridEntityId;

			public long? RemoteEntityId;

			public bool IsSelectable;
		}

		private class CubeGridInfo
		{
			public long EntityId;

			public double Distance;

			public string Name;

			public StringBuilder AppendedDistance;

			public MyCubeGridConnectionStatus Status;

			public bool Owned;

			public MyRefuseReason RemoteStatus;

			public long? RemoteId;

			public override bool Equals(object obj)
			{
				if (!(obj is CubeGridInfo))
				{
					return false;
				}
				CubeGridInfo cubeGridInfo = obj as CubeGridInfo;
				string text = ((Name == null) ? "" : Name);
				string value = ((cubeGridInfo.Name == null) ? "" : cubeGridInfo.Name);
				if (EntityId.Equals(cubeGridInfo.EntityId) && text.Equals(value) && AppendedDistance.Equals(cubeGridInfo.AppendedDistance))
				{
					return Status == cubeGridInfo.Status;
				}
				return false;
			}

			public override int GetHashCode()
			{
				int hashCode = EntityId.GetHashCode();
				string text = ((Name == null) ? "" : Name);
				return (((((hashCode * 397) ^ text.GetHashCode()) * 397) ^ AppendedDistance.GetHashCode()) * 397) ^ (int)Status;
			}
		}

		private MyGuiControlCombobox m_shipsInRange;

		private MyGuiControlButton m_button;

		private MyGuiControlTable m_shipsData;

		private MyEntity m_interactedEntityRepresentative;

		private MyEntity m_openInventoryInteractedEntityRepresentative;

		private MyEntity m_interactedEntity;

		private bool m_isRemote;

		private int m_columnToSort;

		private HashSet<MyDataReceiver> m_tmpAntennas = new HashSet<MyDataReceiver>();

		private Dictionary<long, CubeGridInfo> m_tmpGridInfoOutput = new Dictionary<long, CubeGridInfo>();

		private HashSet<MyDataBroadcaster> m_tmpBroadcasters = new HashSet<MyDataBroadcaster>();

		private HashSet<MyAntennaSystem.BroadcasterInfo> m_previousMutualConnectionGrids;
<<<<<<< HEAD

		private HashSet<CubeGridInfo> m_previousShipInfo;

=======

		private HashSet<CubeGridInfo> m_previousShipInfo;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private int m_cnt;

		public event Action ButtonClicked;

		public void Init(MyGuiControlParent menuParent, MyGuiControlParent panelParent, MyEntity interactedEntity, MyEntity openInventoryInteractedEntity, bool isRemote)
		{
			m_interactedEntityRepresentative = GetInteractedEntityRepresentative(interactedEntity);
			m_openInventoryInteractedEntityRepresentative = GetInteractedEntityRepresentative(openInventoryInteractedEntity);
			m_interactedEntity = interactedEntity ?? MySession.Static.LocalCharacter;
			m_isRemote = isRemote;
			if (menuParent == null)
			{
				MySandboxGame.Log.WriteLine("menuParent is null");
			}
			if (panelParent == null)
			{
				MySandboxGame.Log.WriteLine("panelParent is null");
			}
			if (menuParent != null && panelParent != null)
			{
				m_shipsInRange = (MyGuiControlCombobox)menuParent.Controls.GetControlByName("ShipsInRange");
				m_button = (MyGuiControlButton)menuParent.Controls.GetControlByName("SelectShip");
				m_shipsData = (MyGuiControlTable)panelParent.Controls.GetControlByName("ShipsData");
				m_columnToSort = 1;
				m_button.ButtonClicked += Menu_ButtonClicked;
				m_shipsData.ColumnClicked += shipsData_ColumnClicked;
				m_shipsInRange.ItemSelected += shipsInRange_ItemSelected;
				Refresh();
			}
		}

		public void Refresh()
		{
			PopulateMutuallyConnectedCubeGrids(MyAntennaSystem.Static.GetConnectedGridsInfo(m_openInventoryInteractedEntityRepresentative, null, mutual: true, accessible: true));
			PopulateOwnedCubeGrids(GetAllCubeGridsInfo());
		}

		private void PopulateMutuallyConnectedCubeGrids(HashSet<MyAntennaSystem.BroadcasterInfo> playerMutualConnection)
		{
<<<<<<< HEAD
=======
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (playerMutualConnection == null || m_openInventoryInteractedEntityRepresentative == null || m_shipsInRange == null || m_interactedEntityRepresentative == null)
			{
				return;
			}
			m_shipsInRange.ClearItems();
			m_shipsInRange.AddItem(m_openInventoryInteractedEntityRepresentative.EntityId, new StringBuilder(m_openInventoryInteractedEntityRepresentative.DisplayName));
			Enumerator<MyAntennaSystem.BroadcasterInfo> enumerator = playerMutualConnection.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyAntennaSystem.BroadcasterInfo current = enumerator.get_Current();
					if (m_shipsInRange.TryGetItemByKey(current.EntityId) == null)
					{
						m_shipsInRange.AddItem(current.EntityId, new StringBuilder(current.Name));
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_shipsInRange.Visible = true;
			if (m_button != null)
			{
				m_button.Visible = true;
			}
			m_shipsInRange.SortItemsByValueText();
			if (m_shipsInRange.TryGetItemByKey(m_interactedEntityRepresentative.EntityId) == null && m_interactedEntityRepresentative is MyCubeGrid)
			{
				m_shipsInRange.AddItem(m_interactedEntityRepresentative.EntityId, new StringBuilder((m_interactedEntityRepresentative as MyCubeGrid).DisplayName));
			}
			m_shipsInRange.SelectItemByKey(m_interactedEntityRepresentative.EntityId);
		}

		private void PopulateOwnedCubeGrids(HashSet<CubeGridInfo> gridInfoList)
		{
<<<<<<< HEAD
=======
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (gridInfoList == null)
			{
				return;
			}
			long num = 0L;
			if (m_shipsData.SelectedRow?.UserData != null)
			{
				num = ((UserData)(m_shipsData.SelectedRow?.UserData)).GridEntityId;
			}
			float value = m_shipsData.ScrollBar.Value;
			m_shipsData.Clear();
			m_shipsData.Controls.Clear();
			MyGuiControlTable.Row row = null;
<<<<<<< HEAD
			UserData userData = default(UserData);
			foreach (CubeGridInfo gridInfo in gridInfoList)
			{
				userData.GridEntityId = gridInfo.EntityId;
				userData.RemoteEntityId = gridInfo.RemoteId;
				string collectiveTooltip = string.Empty;
				MyGuiControlTable.Cell cell;
				MyGuiControlTable.Cell cell2;
				MyGuiControlTable.Cell cell3;
				MyGuiControlTable.Cell cell4;
				MyGuiControlTable.Cell cell5;
				if (gridInfo.Status == MyCubeGridConnectionStatus.Connected || gridInfo.Status == MyCubeGridConnectionStatus.PhysicallyConnected || gridInfo.Status == MyCubeGridConnectionStatus.Me)
=======
			Enumerator<CubeGridInfo> enumerator = gridInfoList.GetEnumerator();
			try
			{
				UserData userData = default(UserData);
				while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					CubeGridInfo current = enumerator.get_Current();
					userData.GridEntityId = current.EntityId;
					userData.RemoteEntityId = current.RemoteId;
					string collectiveTooltip = string.Empty;
					MyGuiControlTable.Cell cell;
					MyGuiControlTable.Cell cell2;
					MyGuiControlTable.Cell cell3;
					MyGuiControlTable.Cell cell4;
					MyGuiControlTable.Cell cell5;
					if (current.Status == MyCubeGridConnectionStatus.Connected || current.Status == MyCubeGridConnectionStatus.PhysicallyConnected || current.Status == MyCubeGridConnectionStatus.Me)
					{
						StringBuilder stringBuilder = new StringBuilder();
						if (current.Status == MyCubeGridConnectionStatus.Connected)
						{
							stringBuilder = current.AppendedDistance;
						}
						userData.IsSelectable = true;
						cell = new MyGuiControlTable.Cell(new StringBuilder(current.Name), null, textColor: Color.White, toolTip: current.Name);
						cell2 = CreateControlCell(current, isActive: true);
						cell3 = new MyGuiControlTable.Cell(stringBuilder, toolTip: stringBuilder.ToString(), userData: current.Distance, textColor: Color.White);
						cell4 = CreateStatusIcons(current, isActive: true, out collectiveTooltip);
						cell5 = CreateTerminalCell(current, isActive: true);
					}
					else
					{
						userData.IsSelectable = false;
						cell = new MyGuiControlTable.Cell(new StringBuilder(current.Name), null, textColor: Color.Gray, toolTip: current.Name);
						cell2 = CreateControlCell(current, isActive: false);
						cell3 = new MyGuiControlTable.Cell(MyTexts.Get(MySpaceTexts.NotAvailable), double.MaxValue, MyTexts.GetString(MySpaceTexts.NotAvailable), Color.Gray);
						cell4 = CreateStatusIcons(current, isActive: true, out collectiveTooltip);
						cell5 = CreateTerminalCell(current, isActive: false);
					}
					MyGuiControlTable.Row row2 = new MyGuiControlTable.Row(userData, collectiveTooltip);
					row2.AddCell(cell);
					row2.AddCell(cell3);
					row2.AddCell(cell4);
					row2.AddCell(cell2);
					row2.AddCell(cell5);
					m_shipsData.Add(row2);
					if (num == userData.GridEntityId)
					{
						row = row2;
					}
<<<<<<< HEAD
					userData.IsSelectable = true;
					cell = new MyGuiControlTable.Cell(new StringBuilder(gridInfo.Name), null, textColor: Color.White, toolTip: gridInfo.Name);
					cell2 = CreateControlCell(gridInfo, isActive: true);
					cell3 = new MyGuiControlTable.Cell(stringBuilder, toolTip: stringBuilder.ToString(), userData: gridInfo.Distance, textColor: Color.White);
					cell4 = CreateStatusIcons(gridInfo, isActive: true, out collectiveTooltip);
					cell5 = CreateTerminalCell(gridInfo, isActive: true);
				}
				else
				{
					userData.IsSelectable = false;
					cell = new MyGuiControlTable.Cell(new StringBuilder(gridInfo.Name), null, textColor: Color.Gray, toolTip: gridInfo.Name);
					cell2 = CreateControlCell(gridInfo, isActive: false);
					cell3 = new MyGuiControlTable.Cell(MyTexts.Get(MySpaceTexts.NotAvailable), double.MaxValue, MyTexts.GetString(MySpaceTexts.NotAvailable), Color.Gray);
					cell4 = CreateStatusIcons(gridInfo, isActive: true, out collectiveTooltip);
					cell5 = CreateTerminalCell(gridInfo, isActive: false);
				}
				MyGuiControlTable.Row row2 = new MyGuiControlTable.Row(userData, collectiveTooltip);
				row2.AddCell(cell);
				row2.AddCell(cell3);
				row2.AddCell(cell4);
				row2.AddCell(cell2);
				row2.AddCell(cell5);
				m_shipsData.Add(row2);
				if (num == userData.GridEntityId)
				{
					row = row2;
				}
			}
=======
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_shipsData.SortByColumn(m_columnToSort, MyGuiControlTable.SortStateEnum.Ascending, switchSort: false);
			m_shipsData.ScrollBar.ChangeValue(value);
			m_shipsData.GamepadHelpTextId = MySpaceTexts.TerminalRemote_Help_ShipsTable;
			if (row != null)
			{
				m_shipsData.SelectedRow = row;
			}
		}

		private MyGuiControlTable.Cell CreateControlCell(CubeGridInfo gridInfo, bool isActive)
		{
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell();
			Vector2 value = new Vector2(0.1f, m_shipsData.RowHeight * 0.8f);
			MyRefuseReason remoteStatus = gridInfo.RemoteStatus;
			if ((uint)remoteStatus <= 1u || remoteStatus == MyRefuseReason.Forbidden)
			{
				isActive = false;
			}
			isActive &= CanTakeTerminalOuter(gridInfo);
			cell.Control = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, text: MyTexts.Get(MySpaceTexts.BroadcastScreen_TakeControlButton), size: value, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM, toolTip: null, textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: OnButtonClicked_TakeControl);
			cell.Control.ShowTooltipWhenDisabled = true;
			cell.Control.Enabled = isActive;
			if (cell.Control.Enabled)
			{
				cell.Control.SetToolTip(MySpaceTexts.BroadcastScreen_TakeControlButton_ToolTip);
			}
			else
			{
				cell.Control.SetToolTip(MySpaceTexts.BroadcastScreen_TakeControlButtonDisabled_ToolTip);
			}
			m_shipsData.Controls.Add(cell.Control);
			return cell;
		}

		private bool CanTakeTerminalOuter(CubeGridInfo gridInfo)
		{
			bool result = true;
			MyRefuseReason myRefuseReason = CanTakeTerminal(gridInfo);
			if ((uint)(myRefuseReason - 2) <= 1u || (uint)(myRefuseReason - 5) <= 1u)
			{
				result = false;
			}
			return result;
		}

		private MyGuiControlTable.Cell CreateTerminalCell(CubeGridInfo gridInfo, bool isActive)
		{
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell();
			Vector2 value = new Vector2(0.1f, m_shipsData.RowHeight * 0.8f);
			isActive &= CanTakeTerminalOuter(gridInfo);
			cell.Control = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, text: MyTexts.Get(MySpaceTexts.BroadcastScreen_TerminalButton), size: value, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, toolTip: null, textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: OnButtonClicked_OpenTerminal);
			cell.Control.ShowTooltipWhenDisabled = true;
			cell.Control.Enabled = isActive;
			if (cell.Control.Enabled)
			{
				cell.Control.SetToolTip(MySpaceTexts.BroadcastScreen_TerminalButton_ToolTip);
			}
			else
			{
				cell.Control.SetToolTip(MySpaceTexts.BroadcastScreen_TerminalButtonDisabled_ToolTip);
			}
			m_shipsData.Controls.Add(cell.Control);
			return cell;
		}

		private MyGuiControlTable.Cell CreateStatusIcons(CubeGridInfo gridInfo, bool isActive, out string collectiveTooltip)
		{
			collectiveTooltip = string.Empty;
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell();
			float num = m_shipsData.RowHeight * 0.7f;
			bool flag2;
			bool flag;
			bool flag3 = (flag2 = (flag = isActive));
			MyStringId myStringId;
			MyStringId myStringId2 = (myStringId = MyStringId.NullOrEmpty);
			StringBuilder stringBuilder = new StringBuilder();
			MyGuiControlParent myGuiControlParent = new MyGuiControlParent();
			myGuiControlParent.CanPlaySoundOnMouseOver = false;
			MyRefuseReason myRefuseReason = CanTakeTerminal(gridInfo);
			MyRefuseReason remoteStatus = gridInfo.RemoteStatus;
			switch (myRefuseReason)
			{
			case MyRefuseReason.PlayerBroadcastOff:
				flag3 = false;
				myStringId2 = MySpaceTexts.BroadcastScreen_TerminalButton_PlayerBroadcastOffToolTip;
				break;
			case MyRefuseReason.NoStableConnection:
				flag3 = false;
				myStringId2 = MySpaceTexts.BroadcastScreen_TerminalButton_NoStableConnectionToolTip;
				break;
			case MyRefuseReason.Forbidden:
				flag3 = false;
				myStringId2 = MySpaceTexts.BroadcastScreen_NoOwnership;
				break;
			case MyRefuseReason.NoProblem:
				myStringId2 = MySpaceTexts.BroadcastScreen_TerminalButton_StableConnectionToolTip;
				break;
			}
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(new Vector2(-1.25f * num, 0f), new Vector2(num * 0.78f, num), null, flag3 ? "Textures\\GUI\\Icons\\BroadcastStatus\\AntennaOn.png" : "Textures\\GUI\\Icons\\BroadcastStatus\\AntennaOff.png", null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			myGuiControlImage.SetToolTip(myStringId2);
			myGuiControlParent.Controls.Add(myGuiControlImage);
			switch (remoteStatus)
			{
			case MyRefuseReason.NoRemoteControl:
				myStringId = MySpaceTexts.BroadcastScreen_TakeControlButton_NoRemoteToolTip;
				flag = false;
				break;
			case MyRefuseReason.NoMainRemoteControl:
				myStringId = MySpaceTexts.BroadcastScreen_TakeControlButton_NoMainRemoteControl;
				flag = false;
				break;
			case MyRefuseReason.NoOwner:
			case MyRefuseReason.NoProblem:
				myStringId = MySpaceTexts.BroadcastScreen_TakeControlButton_RemoteToolTip;
				break;
			}
			MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(new Vector2(-0.25f * num, 0f), new Vector2(num * 0.78f, num), null, flag ? "Textures\\GUI\\Icons\\BroadcastStatus\\RemoteOn.png" : "Textures\\GUI\\Icons\\BroadcastStatus\\RemoteOff.png", null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			myGuiControlImage2.SetToolTip(myStringId);
			myGuiControlParent.Controls.Add(myGuiControlImage2);
			if ((myRefuseReason == MyRefuseReason.NoStableConnection || myRefuseReason == MyRefuseReason.PlayerBroadcastOff) && remoteStatus == MyRefuseReason.NoRemoteControl)
			{
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.BroadcastScreen_UnavailableControlButton));
				flag2 = false;
			}
			if (flag2 && (myRefuseReason == MyRefuseReason.NoOwner || remoteStatus == MyRefuseReason.Forbidden || myRefuseReason == MyRefuseReason.NoStableConnection || myRefuseReason == MyRefuseReason.PlayerBroadcastOff))
			{
				flag2 = false;
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.BroadcastScreen_NoOwnership));
			}
			if (myRefuseReason == MyRefuseReason.NoOwner)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.BroadcastScreen_Antenna));
			}
			if (remoteStatus == MyRefuseReason.Forbidden)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.BroadcastScreen_RemoteControl));
			}
			if (flag2)
			{
				stringBuilder.Append((object)MyTexts.Get(MySpaceTexts.BroadcastScreen_Ownership));
			}
			MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage(new Vector2(0.75f * num, 0f), new Vector2(num * 0.78f, num), null, flag2 ? "Textures\\GUI\\Icons\\BroadcastStatus\\KeyOn.png" : "Textures\\GUI\\Icons\\BroadcastStatus\\KeyOff.png", null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			myGuiControlImage3.SetToolTip(stringBuilder.ToString());
			myGuiControlParent.Controls.Add(myGuiControlImage3);
			cell.Control = myGuiControlParent;
			m_shipsData.Controls.Add(myGuiControlParent);
			collectiveTooltip = string.Format("{0}{3}{1}\n{2}", MyTexts.GetString(myStringId2), MyTexts.GetString(myStringId), stringBuilder.ToString(), string.IsNullOrEmpty(myStringId2.ToString()) ? "" : "\n");
			return cell;
		}

		private HashSet<CubeGridInfo> GetAllCubeGridsInfo()
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			HashSet<CubeGridInfo> val = new HashSet<CubeGridInfo>();
			m_tmpGridInfoOutput.Clear();
			m_tmpBroadcasters.Clear();
			if (MySession.Static.LocalCharacter == null)
			{
				return val;
			}
			Enumerator<MyDataBroadcaster> enumerator = MyAntennaSystem.Static.GetAllRelayedBroadcasters(m_interactedEntityRepresentative, MySession.Static.LocalPlayerId, mutual: false, m_tmpBroadcasters).GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (m_tmpGridInfoOutput.ContainsKey(allRelayedBroadcaster.Info.EntityId) || allRelayedBroadcaster == MySession.Static.LocalCharacter.RadioBroadcaster || !allRelayedBroadcaster.ShowInTerminal)
				{
					continue;
				}
				double playerBroadcasterDistance = GetPlayerBroadcasterDistance(allRelayedBroadcaster);
				MyCubeGridConnectionStatus broadcasterStatus = GetBroadcasterStatus(allRelayedBroadcaster);
				if (m_tmpGridInfoOutput.TryGetValue(allRelayedBroadcaster.Info.EntityId, out var value))
				{
					if (value.Status > broadcasterStatus)
					{
						value.Status = broadcasterStatus;
=======
				while (enumerator.MoveNext())
				{
					MyDataBroadcaster current = enumerator.get_Current();
					if (current == MySession.Static.LocalCharacter.RadioBroadcaster || !current.ShowInTerminal)
					{
						continue;
					}
					double playerBroadcasterDistance = GetPlayerBroadcasterDistance(current);
					MyCubeGridConnectionStatus broadcasterStatus = GetBroadcasterStatus(current);
					if (m_tmpGridInfoOutput.TryGetValue(current.Info.EntityId, out var value))
					{
						if (value.Status > broadcasterStatus)
						{
							value.Status = broadcasterStatus;
						}
						if (value.Distance > playerBroadcasterDistance)
						{
							value.Distance = playerBroadcasterDistance;
							value.AppendedDistance = new StringBuilder().AppendDecimal(playerBroadcasterDistance, 0).Append(" m");
						}
						if (!value.Owned && current.CanBeUsedByPlayer(MySession.Static.LocalPlayerId))
						{
							value.Owned = true;
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					if (value.Distance > playerBroadcasterDistance)
					{
						value.Distance = playerBroadcasterDistance;
						value.AppendedDistance = new StringBuilder().AppendDecimal(playerBroadcasterDistance, 0).Append(" m");
					}
					if (!value.Owned && allRelayedBroadcaster.CanBeUsedByPlayer(MySession.Static.LocalPlayerId))
					{
<<<<<<< HEAD
						value.Owned = true;
=======
						m_tmpGridInfoOutput.Add(current.Info.EntityId, new CubeGridInfo
						{
							EntityId = current.Info.EntityId,
							Distance = playerBroadcasterDistance,
							AppendedDistance = new StringBuilder().AppendDecimal(playerBroadcasterDistance, 0).Append(" m"),
							Name = current.Info.Name,
							Status = broadcasterStatus,
							Owned = current.CanBeUsedByPlayer(MySession.Static.LocalPlayerId),
							RemoteStatus = GetRemoteStatus(current),
							RemoteId = current.MainRemoteControlId
						});
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				else
				{
					m_tmpGridInfoOutput.Add(allRelayedBroadcaster.Info.EntityId, new CubeGridInfo
					{
						EntityId = allRelayedBroadcaster.Info.EntityId,
						Distance = playerBroadcasterDistance,
						AppendedDistance = new StringBuilder().AppendDecimal(playerBroadcasterDistance, 0).Append(" m"),
						Name = allRelayedBroadcaster.Info.Name,
						Status = broadcasterStatus,
						Owned = allRelayedBroadcaster.CanBeUsedByPlayer(MySession.Static.LocalPlayerId),
						RemoteStatus = GetRemoteStatus(allRelayedBroadcaster),
						RemoteId = allRelayedBroadcaster.MainRemoteControlId
					});
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			foreach (CubeGridInfo value2 in m_tmpGridInfoOutput.Values)
			{
				val.Add(value2);
			}
			return val;
		}

		private MyCubeGridConnectionStatus GetBroadcasterStatus(MyDataBroadcaster broadcaster)
		{
			if (!MyAntennaSystem.Static.CheckConnection(broadcaster.Receiver, m_openInventoryInteractedEntityRepresentative, MySession.Static.LocalPlayerId, mutual: false))
			{
				return MyCubeGridConnectionStatus.OutOfBroadcastingRange;
			}
			if (!MyAntennaSystem.Static.CheckConnection(m_openInventoryInteractedEntityRepresentative, broadcaster, MySession.Static.LocalPlayerId, mutual: false))
			{
				return MyCubeGridConnectionStatus.OutOfReceivingRange;
			}
			return MyCubeGridConnectionStatus.Connected;
		}

		private MyCubeGridConnectionStatus GetShipStatus(MyCubeGrid grid)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			HashSet<MyDataBroadcaster> output = new HashSet<MyDataBroadcaster>();
			MyAntennaSystem.Static.GetEntityBroadcasters(grid, ref output, MySession.Static.LocalPlayerId);
			MyCubeGridConnectionStatus result = MyCubeGridConnectionStatus.OutOfReceivingRange;
			Enumerator<MyDataBroadcaster> enumerator = output.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDataBroadcaster current = enumerator.get_Current();
					MyCubeGridConnectionStatus broadcasterStatus = GetBroadcasterStatus(current);
					switch (broadcasterStatus)
					{
					case MyCubeGridConnectionStatus.Connected:
						return broadcasterStatus;
					case MyCubeGridConnectionStatus.OutOfBroadcastingRange:
						result = broadcasterStatus;
						break;
					}
				}
				return result;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private MyRefuseReason GetRemoteStatus(MyDataBroadcaster broadcaster)
		{
			if (!broadcaster.HasRemoteControl)
			{
				return MyRefuseReason.NoRemoteControl;
			}
			long? mainRemoteControlOwner = broadcaster.MainRemoteControlOwner;
			if (!mainRemoteControlOwner.HasValue)
			{
				return MyRefuseReason.NoMainRemoteControl;
			}
			MyRelationsBetweenPlayers relationPlayerPlayer = MyIDModule.GetRelationPlayerPlayer(mainRemoteControlOwner.Value, MySession.Static.LocalHumanPlayer.Identity.IdentityId);
			if (relationPlayerPlayer == MyRelationsBetweenPlayers.Self)
			{
				return MyRefuseReason.NoProblem;
			}
			MyOwnershipShareModeEnum mainRemoteControlSharing = broadcaster.MainRemoteControlSharing;
			if (mainRemoteControlSharing == MyOwnershipShareModeEnum.All || (mainRemoteControlSharing == MyOwnershipShareModeEnum.Faction && relationPlayerPlayer == MyRelationsBetweenPlayers.Allies))
			{
				return MyRefuseReason.NoProblem;
			}
			if (mainRemoteControlOwner.Value == 0L)
			{
				return MyRefuseReason.NoOwner;
			}
			return MyRefuseReason.Forbidden;
		}

		private MyEntity GetInteractedEntityRepresentative(MyEntity controlledEntity)
		{
			if (controlledEntity is MyCubeBlock)
			{
				return MyAntennaSystem.Static.GetLogicalGroupRepresentative((controlledEntity as MyCubeBlock).CubeGrid);
			}
			return MySession.Static.LocalCharacter;
		}

		private double GetPlayerBroadcasterDistance(MyDataBroadcaster broadcaster)
		{
			if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity != null)
			{
				return Vector3D.Distance(MySession.Static.ControlledEntity.Entity.PositionComp.GetPosition(), broadcaster.BroadcastPosition);
			}
			return double.MaxValue;
		}

		private MyRefuseReason CanTakeTerminal(CubeGridInfo gridInfo)
		{
			if (!gridInfo.Owned)
			{
				return MyRefuseReason.NoOwner;
			}
			if (gridInfo.Status == MyCubeGridConnectionStatus.OutOfBroadcastingRange && MySession.Static.ControlledEntity.Entity is MyCharacter && !(MySession.Static.ControlledEntity.Entity as MyCharacter).RadioBroadcaster.Enabled)
			{
				return MyRefuseReason.PlayerBroadcastOff;
			}
			if (gridInfo.Status == MyCubeGridConnectionStatus.OutOfBroadcastingRange || gridInfo.Status == MyCubeGridConnectionStatus.OutOfReceivingRange)
			{
				return MyRefuseReason.NoStableConnection;
			}
			return MyRefuseReason.NoProblem;
		}

		private void OnButtonClicked_TakeControl(MyGuiControlButton obj)
		{
			if (m_shipsData.SelectedRow != null)
			{
				UserData userData = (UserData)m_shipsData.SelectedRow.UserData;
				if (userData.IsSelectable && userData.RemoteEntityId.HasValue)
				{
					FindRemoteControlAndTakeControl(userData.GridEntityId, userData.RemoteEntityId.Value);
				}
			}
		}

		private void Menu_ButtonClicked(MyGuiControlButton button)
		{
			if (this.ButtonClicked != null)
			{
				this.ButtonClicked();
			}
		}

		private void OnButtonClicked_OpenTerminal(MyGuiControlButton obj)
		{
			MyGuiControlTable.EventArgs args = default(MyGuiControlTable.EventArgs);
			args.MouseButton = MyMouseButtonsEnum.None;
			args.RowIndex = -1;
			shipsData_ItemDoubleClicked(null, args);
		}

		private void shipsData_ItemDoubleClicked(MyGuiControlTable sender, MyGuiControlTable.EventArgs args)
		{
			if (m_shipsData.SelectedRow != null)
			{
				UserData userData = (UserData)m_shipsData.SelectedRow.UserData;
				if (userData.IsSelectable)
				{
					OpenPropertiesByEntityId(userData.GridEntityId);
				}
			}
		}

		private void shipsData_ColumnClicked(MyGuiControlTable sender, int column)
		{
			m_columnToSort = column;
		}

		private void shipsInRange_ItemSelected()
		{
			if ((m_shipsInRange.IsMouseOver || m_shipsInRange.HasFocus) && m_shipsInRange.GetSelectedKey() != m_interactedEntityRepresentative.EntityId)
			{
				OpenPropertiesByEntityId(m_shipsInRange.GetSelectedKey());
			}
		}

		private void OpenPropertiesByEntityId(long entityId)
		{
<<<<<<< HEAD
=======
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyEntities.TryGetEntityById(entityId, out var entity);
			if (entity == null && !Sync.IsServer)
			{
				MyGuiScreenTerminal.RequestReplicable(entityId, entityId, OpenPropertiesByEntityId);
			}
			else if (entity is MyCharacter)
			{
				MyGuiScreenTerminal.ChangeInteractedEntity(null, isRemote: false);
			}
			else
			{
				if (entity == null || !(entity is MyCubeGrid))
				{
					return;
				}
				MyCubeGrid myCubeGrid = entity as MyCubeGrid;
				if (m_openInventoryInteractedEntityRepresentative == myCubeGrid && MySession.Static.LocalCharacter?.Parent != null)
				{
					MyGuiScreenTerminal.ChangeInteractedEntity(MySession.Static.LocalCharacter?.Parent, isRemote: false);
				}
				else
				{
					if (!MyAntennaSystem.Static.CheckConnection(myCubeGrid, m_openInventoryInteractedEntityRepresentative, MySession.Static.LocalHumanPlayer))
					{
						return;
					}
					m_tmpAntennas.Clear();
					MyAntennaSystem.Static.GetEntityReceivers(myCubeGrid, ref m_tmpAntennas, MySession.Static.LocalPlayerId);
					Enumerator<MyDataReceiver> enumerator = m_tmpAntennas.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							enumerator.get_Current().UpdateBroadcastersInRange();
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					if (m_tmpAntennas.get_Count() <= 0)
					{
						MyGuiScreenTerminal.ChangeInteractedEntity(MySession.Static.LocalCharacter?.Parent, isRemote: false);
					}
					else
					{
						MyGuiScreenTerminal.ChangeInteractedEntity(Enumerable.ElementAt<MyDataReceiver>((IEnumerable<MyDataReceiver>)m_tmpAntennas, 0).Entity as MyTerminalBlock, isRemote: true);
					}
				}
			}
		}

		private void FindRemoteControlAndTakeControl(long gridEntityId, long remoteEntityId)
		{
<<<<<<< HEAD
=======
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyEntities.TryGetEntityById(remoteEntityId, out MyRemoteControl entity, allowClosed: false);
			if (entity == null)
			{
				if (!Sync.IsServer)
				{
					MyGuiScreenTerminal.RequestReplicable(gridEntityId, remoteEntityId, delegate(long x)
					{
						FindRemoteControlAndTakeControl(gridEntityId, x);
					});
				}
				return;
			}
			m_tmpAntennas.Clear();
			MyAntennaSystem.Static.GetEntityReceivers(entity, ref m_tmpAntennas, MySession.Static.LocalPlayerId);
<<<<<<< HEAD
			foreach (MyDataReceiver tmpAntenna in m_tmpAntennas)
			{
				tmpAntenna.UpdateBroadcastersInRange();
			}
=======
			Enumerator<MyDataReceiver> enumerator = m_tmpAntennas.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().UpdateBroadcastersInRange();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			entity.RequestControl();
		}

		/// <summary>
		/// used to test if interacted entity is still connected with open inventory interacted entity
		/// </summary>
		/// <returns></returns>
		public bool TestConnection()
		{
			if (m_openInventoryInteractedEntityRepresentative == null || m_interactedEntityRepresentative == null || MySession.Static == null)
			{
				return false;
			}
			if (m_openInventoryInteractedEntityRepresentative.EntityId == m_interactedEntityRepresentative.EntityId && !m_isRemote)
			{
				MyCharacter localCharacter = MySession.Static.LocalCharacter;
				if (m_interactedEntity != null && localCharacter != null)
				{
					return m_interactedEntity.PositionComp.WorldAABB.DistanceSquared(localCharacter.PositionComp.GetPosition()) < (double)(MyConstants.DEFAULT_INTERACTIVE_DISTANCE * MyConstants.DEFAULT_INTERACTIVE_DISTANCE);
				}
			}
			if (m_interactedEntityRepresentative is MyCubeGrid)
			{
				return GetShipStatus(m_interactedEntityRepresentative as MyCubeGrid) == MyCubeGridConnectionStatus.Connected;
			}
			return true;
		}

		public void Close()
		{
			if (m_shipsInRange != null)
			{
				m_shipsInRange.ItemSelected -= shipsInRange_ItemSelected;
				m_shipsInRange.ClearItems();
				m_shipsInRange = null;
			}
			if (m_shipsData != null)
			{
				m_shipsData.ColumnClicked -= shipsData_ColumnClicked;
				m_shipsData.Clear();
				m_shipsData = null;
			}
			if (m_button != null)
			{
				m_button.ButtonClicked -= Menu_ButtonClicked;
				m_button = null;
			}
		}

		public void Update(bool isScreenActive)
		{
			m_cnt = ++m_cnt % 30;
			if (m_cnt == 0)
			{
				if (m_previousMutualConnectionGrids == null)
				{
					m_previousMutualConnectionGrids = MyAntennaSystem.Static.GetConnectedGridsInfo(m_openInventoryInteractedEntityRepresentative);
				}
				if (m_previousShipInfo == null)
				{
					m_previousShipInfo = GetAllCubeGridsInfo();
				}
				HashSet<MyAntennaSystem.BroadcasterInfo> connectedGridsInfo = MyAntennaSystem.Static.GetConnectedGridsInfo(m_openInventoryInteractedEntityRepresentative);
				HashSet<CubeGridInfo> allCubeGridsInfo = GetAllCubeGridsInfo();
<<<<<<< HEAD
				if (!m_previousMutualConnectionGrids.SetEquals(connectedGridsInfo))
=======
				if (!m_previousMutualConnectionGrids.SetEquals((IEnumerable<MyAntennaSystem.BroadcasterInfo>)connectedGridsInfo))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					PopulateMutuallyConnectedCubeGrids(connectedGridsInfo);
					m_previousMutualConnectionGrids = connectedGridsInfo;
				}
<<<<<<< HEAD
				if (isScreenActive && !m_previousShipInfo.SequenceEqual(allCubeGridsInfo))
=======
				if (isScreenActive && !Enumerable.SequenceEqual<CubeGridInfo>((IEnumerable<CubeGridInfo>)m_previousShipInfo, (IEnumerable<CubeGridInfo>)allCubeGridsInfo))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					PopulateOwnedCubeGrids(allCubeGridsInfo);
					m_previousShipInfo = allCubeGridsInfo;
				}
			}
		}

		public void HandleInput()
		{
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT))
			{
				(m_shipsData.GetInnerControlsFromCurrentCell(3) as MyGuiControlButton)?.PressButton();
			}
			if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				(m_shipsData.GetInnerControlsFromCurrentCell(4) as MyGuiControlButton)?.PressButton();
			}
		}
	}
}
