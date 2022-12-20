<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.Replication.StateGroups
{
	internal class MyEntityInventoryStateGroup : IMyStateGroup, IMyNetObject, IMyEventOwner
	{
		private struct InventoryDeltaInformation
		{
			public bool HasChanges;

			public uint MessageId;

			public byte SubId;

			public List<uint> RemovedItems;

			public Dictionary<uint, MyFixedPoint> ChangedItems;

			public SortedDictionary<int, MyPhysicalInventoryItem> NewItems;

			public Dictionary<uint, int> SwappedItems;
		}

		private struct ClientInvetoryData
		{
			public MyPhysicalInventoryItem Item;

			public MyFixedPoint Amount;
		}

		private class InventoryClientData
		{
			public uint CurrentMessageId;

			public InventoryDeltaInformation MainSendingInfo;

			public bool Dirty;

			public readonly Dictionary<byte, InventoryDeltaInformation> SendPackets = new Dictionary<byte, InventoryDeltaInformation>();

			public readonly List<InventoryDeltaInformation> FailedIncompletePackets = new List<InventoryDeltaInformation>();

			public readonly SortedDictionary<uint, ClientInvetoryData> ClientItemsSorted = new SortedDictionary<uint, ClientInvetoryData>();

			public readonly List<ClientInvetoryData> ClientItems = new List<ClientInvetoryData>();
		}

		private Dictionary<Endpoint, InventoryClientData> m_clientInventoryUpdate;

		private List<MyPhysicalInventoryItem> m_itemsToSend;

		private HashSet<uint> m_foundDeltaItems;

		private uint m_nextExpectedPacketId;

		private readonly SortedList<uint, InventoryDeltaInformation> m_buffer;

		/// <summary>
		/// Collection of all message parts that has arrived and were processed.
		/// </summary>
		private HashSet<byte> m_messagePartsProcessed = new HashSet<byte>();

		/// <summary>
		/// Collection of message parts that have been processed or must not be processed (either their parent or descendant arrived).
		/// </summary>
		private HashSet<byte> m_messagePartsBlocked = new HashSet<byte>();

		private Dictionary<int, MyPhysicalInventoryItem> m_tmpSwappingList;

		public bool IsHighPriority => false;

		private MyInventory Inventory { get; set; }

		public IMyReplicable Owner { get; private set; }

		public bool IsValid
		{
			get
			{
				if (Owner != null)
				{
					return Owner.IsValid;
				}
				return false;
			}
		}

		public bool IsStreaming => false;

		public bool NeedsUpdate => false;

		public MyEntityInventoryStateGroup(MyInventory entity, bool attach, IMyReplicable owner)
		{
			Inventory = entity;
			if (attach)
			{
				Inventory.ContentsChanged += InventoryChanged;
			}
			Owner = owner;
			if (!Sync.IsServer)
			{
				m_buffer = new SortedList<uint, InventoryDeltaInformation>();
			}
		}

		private void InventoryChanged(MyInventoryBase obj)
		{
			if (m_clientInventoryUpdate == null)
			{
				return;
<<<<<<< HEAD
			}
			foreach (KeyValuePair<Endpoint, InventoryClientData> item in m_clientInventoryUpdate)
			{
				m_clientInventoryUpdate[item.Key].Dirty = true;
			}
=======
			}
			foreach (KeyValuePair<Endpoint, InventoryClientData> item in m_clientInventoryUpdate)
			{
				m_clientInventoryUpdate[item.Key].Dirty = true;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyMultiplayer.GetReplicationServer().AddToDirtyGroups(this);
		}

		public void CreateClientData(MyClientStateBase forClient)
		{
			CreateClientData(forClient.EndpointId);
		}

		public void RefreshClientData(Endpoint clientEndpoint)
		{
			if (m_clientInventoryUpdate != null)
			{
				m_clientInventoryUpdate.Remove(clientEndpoint);
				CreateClientData(clientEndpoint);
			}
		}

		private void CreateClientData(Endpoint clientEndpoint)
		{
			if (m_clientInventoryUpdate == null)
			{
				m_clientInventoryUpdate = new Dictionary<Endpoint, InventoryClientData>();
			}
			if (!m_clientInventoryUpdate.TryGetValue(clientEndpoint, out var value))
			{
				m_clientInventoryUpdate[clientEndpoint] = new InventoryClientData();
				value = m_clientInventoryUpdate[clientEndpoint];
			}
			value.Dirty = false;
			foreach (MyPhysicalInventoryItem item in Inventory.GetItems())
			{
				MyFixedPoint amount = item.Amount;
				MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
				if (myObjectBuilder_GasContainerObject != null)
				{
					amount = (MyFixedPoint)myObjectBuilder_GasContainerObject.GasLevel;
				}
				ClientInvetoryData clientInvetoryData = default(ClientInvetoryData);
				clientInvetoryData.Item = item;
				clientInvetoryData.Amount = amount;
				ClientInvetoryData clientInvetoryData2 = clientInvetoryData;
				value.ClientItemsSorted.set_Item(item.ItemId, clientInvetoryData2);
				value.ClientItems.Add(clientInvetoryData2);
			}
		}

		public void DestroyClientData(MyClientStateBase forClient)
		{
			if (m_clientInventoryUpdate != null)
			{
				m_clientInventoryUpdate.Remove(forClient.EndpointId);
			}
		}

		public void ClientUpdate(MyTimeSpan clientTimestamp)
		{
		}

		public void Destroy()
		{
		}

		public float GetGroupPriority(int frameCountWithoutSync, MyClientInfo client)
		{
			if (m_clientInventoryUpdate == null || !m_clientInventoryUpdate.TryGetValue(client.EndpointId, out var value))
			{
				return -1f;
			}
			if (!value.Dirty && value.FailedIncompletePackets.Count == 0)
			{
				return -1f;
			}
			if (value.FailedIncompletePackets.Count > 0)
			{
				return 1f * (float)frameCountWithoutSync;
			}
			MyClientState myClientState = (MyClientState)client.State;
			if (Inventory.Owner is MyCharacter)
			{
				MyCharacter myCharacter = Inventory.Owner as MyCharacter;
				MyPlayer myPlayer = MyPlayer.GetPlayerFromCharacter(myCharacter);
				if (myPlayer == null && myCharacter.IsUsing != null)
				{
					MyShipController myShipController = myCharacter.IsUsing as MyShipController;
					if (myShipController != null && myShipController.ControllerInfo.Controller != null)
					{
						myPlayer = myShipController.ControllerInfo.Controller.Player;
					}
				}
				if (myPlayer != null && myPlayer.Id.SteamId == client.EndpointId.Id.Value)
				{
					return 1f * (float)frameCountWithoutSync;
				}
			}
			if (myClientState.ContextEntity is MyCharacter && myClientState.ContextEntity == Inventory.Owner)
			{
				return 1f * (float)frameCountWithoutSync;
			}
			if (myClientState.Context == MyClientState.MyContextKind.Inventory || myClientState.Context == MyClientState.MyContextKind.Building || (myClientState.Context == MyClientState.MyContextKind.Production && Inventory.Owner is MyAssembler))
			{
				return GetPriorityStateGroup(client) * (float)frameCountWithoutSync;
			}
			return 0f;
		}

		private float GetPriorityStateGroup(MyClientInfo client)
		{
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			MyClientState myClientState = (MyClientState)client.State;
			if (Inventory.ForcedPriority.HasValue)
			{
				return Inventory.ForcedPriority.Value;
			}
			if (myClientState.ContextEntity != null)
			{
				if (myClientState.ContextEntity == Inventory.Owner)
				{
					return 1f;
				}
				MyCubeGrid myCubeGrid = myClientState.ContextEntity.GetTopMostParent() as MyCubeGrid;
				if (myCubeGrid != null)
				{
					Enumerator<MyTerminalBlock> enumerator = myCubeGrid.GridSystems.TerminalSystem.Blocks.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							MyTerminalBlock current = enumerator.get_Current();
							if (current == Inventory.Container.Entity && (myClientState.Context != MyClientState.MyContextKind.Production || current is MyAssembler))
							{
								return 1f;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
				}
			}
			return 0f;
		}

		public void Serialize(BitStream stream, MyClientInfo forClient, MyTimeSpan serverTimestamp, MyTimeSpan lastClientTimestamp, byte packetId, int maxBitPosition, HashSet<string> cachedData)
		{
			if (stream.Writing)
			{
				if (m_clientInventoryUpdate == null || !m_clientInventoryUpdate.TryGetValue(forClient.EndpointId, out var value))
				{
					stream.WriteBool(value: false);
					stream.WriteUInt32(0u);
<<<<<<< HEAD
					stream.WriteByte(0);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					return;
				}
				bool needsSplit = false;
				if (value.FailedIncompletePackets.Count > 0)
				{
					InventoryDeltaInformation packetInfo = value.FailedIncompletePackets[0];
					value.FailedIncompletePackets.RemoveAtFast(0);
<<<<<<< HEAD
					InventoryDeltaInformation sentData = PrepareWrite(ref packetInfo, stream, maxBitPosition, out needsSplit);
=======
					InventoryDeltaInformation sentData = WriteInventory(ref packetInfo, stream, packetId, maxBitPosition, out needsSplit);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					sentData.MessageId = packetInfo.MessageId;
					sentData.SubId = packetInfo.SubId;
					if (needsSplit)
					{
<<<<<<< HEAD
						InventoryDeltaInformation item = CreateSplit(ref packetInfo, ref sentData);
						item.MessageId = sentData.MessageId;
						sentData.SubId = (byte)(2 * packetInfo.SubId + 1);
						item.SubId = (byte)(2 * packetInfo.SubId + 2);
						value.FailedIncompletePackets.Add(item);
					}
					WriteInventory(ref sentData, stream, maxBitPosition);
=======
						value.FailedIncompletePackets.Add(CreateSplit(ref packetInfo, ref sentData));
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					value.SendPackets[packetId] = sentData;
					return;
				}
				InventoryDeltaInformation packetInfo2 = CalculateInventoryDiff(ref value);
				packetInfo2.MessageId = value.CurrentMessageId;
<<<<<<< HEAD
				packetInfo2.SubId = 0;
				value.MainSendingInfo = PrepareWrite(ref packetInfo2, stream, maxBitPosition, out needsSplit);
				if (needsSplit)
				{
					InventoryDeltaInformation item2 = CreateSplit(ref packetInfo2, ref value.MainSendingInfo);
					item2.MessageId = packetInfo2.MessageId;
					value.MainSendingInfo.SubId = 1;
					item2.SubId = 2;
					value.FailedIncompletePackets.Add(item2);
				}
				WriteInventory(ref value.MainSendingInfo, stream, maxBitPosition);
				value.SendPackets[packetId] = value.MainSendingInfo;
				value.CurrentMessageId++;
=======
				value.MainSendingInfo = WriteInventory(ref packetInfo2, stream, packetId, maxBitPosition, out needsSplit);
				value.SendPackets[packetId] = value.MainSendingInfo;
				value.CurrentMessageId++;
				if (needsSplit)
				{
					InventoryDeltaInformation item = CreateSplit(ref packetInfo2, ref value.MainSendingInfo);
					item.MessageId = value.CurrentMessageId;
					value.FailedIncompletePackets.Add(item);
					value.CurrentMessageId++;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				value.Dirty = false;
			}
			else
			{
				ReadInventory(stream);
			}
		}

		private bool IsMessageFinished()
		{
			return IsMessageFinishedRecursive(0);
		}

		/// <summary>
		/// Search recursively for children. If both children arrived, set higher level to cache it.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="lastId"></param>
		/// <returns></returns>
		private bool IsMessageFinishedRecursive(byte id, byte lastId = byte.MaxValue)
		{
			if (id == lastId)
			{
				return false;
			}
			if (!m_messagePartsBlocked.Contains(id))
			{
				return false;
			}
			if (m_messagePartsProcessed.Contains(id))
			{
				return true;
			}
			bool num = IsMessageFinishedRecursive((byte)(2 * id + 1), id) && IsMessageFinishedRecursive((byte)(2 * id + 2), id);
			if (num)
			{
				m_messagePartsProcessed.Add(id);
			}
			return num;
		}

		/// <summary>
		/// Add subId to blocked and block all it's parents (if message was processed, parents mut not be, as they overlap)
		/// </summary>
		/// <param name="id"></param>
		private void AddMessagePartBlock(byte id)
		{
			if (m_messagePartsBlocked.Contains(id))
			{
				return;
			}
			m_messagePartsProcessed.Add(id);
			byte b = id;
			while (b >= 0 && !m_messagePartsBlocked.Contains(b))
			{
				m_messagePartsBlocked.Add(b);
				if (b == 0)
				{
					break;
				}
				b = (((int)b % 2 != 1) ? ((byte)((b - 2) / 2)) : ((byte)((b - 1) / 2)));
			}
		}

		/// <summary>
		/// Decide whether message with subid can be processed. 
		/// SubId must not be processed if it is blocked (that means that a child subId was already added) or any parent was added (would overlap with this)
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		private bool CanProcessMessagePart(byte id)
		{
			if (m_messagePartsBlocked.Contains(id))
			{
				return false;
			}
			for (byte b = id; b >= 0; b = (((int)b % 2 != 1) ? ((byte)((b - 2) / 2)) : ((byte)((b - 1) / 2))))
			{
				if (m_messagePartsProcessed.Contains(b))
				{
					return false;
				}
				if (b == 0)
				{
					break;
				}
			}
			return true;
		}

		private void ReadInventory(BitStream stream)
		{
			bool flag = stream.ReadBool();
			uint num = stream.ReadUInt32();
			byte b = stream.ReadByte();
			bool flag2 = true;
			bool flag3 = false;
			InventoryDeltaInformation inventoryDeltaInformation = default(InventoryDeltaInformation);
			if (num == m_nextExpectedPacketId)
			{
				if (CanProcessMessagePart(b))
				{
					AddMessagePartBlock(b);
					if (IsMessageFinished())
					{
						m_nextExpectedPacketId++;
						m_messagePartsProcessed.Clear();
						m_messagePartsBlocked.Clear();
						if (!flag)
						{
							FlushBuffer();
							return;
						}
					}
				}
				else
				{
					flag2 = false;
				}
			}
			else if (num > m_nextExpectedPacketId && !m_buffer.ContainsKey(num))
			{
				flag3 = true;
<<<<<<< HEAD
				value.MessageId = num;
				value.SubId = b;
=======
				inventoryDeltaInformation.MessageId = num;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				flag2 = false;
			}
			if (!flag)
			{
				if (flag3)
				{
					m_buffer.Add(num, inventoryDeltaInformation);
				}
				return;
			}
			if (stream.ReadBool())
			{
				int num2 = stream.ReadInt32();
				for (int i = 0; i < num2; i++)
				{
					uint num3 = stream.ReadUInt32();
					MyFixedPoint myFixedPoint = default(MyFixedPoint);
					myFixedPoint.RawValue = stream.ReadInt64();
					if (!flag2)
					{
						continue;
					}
					if (flag3)
					{
<<<<<<< HEAD
						if (value.ChangedItems == null)
						{
							value.ChangedItems = new Dictionary<uint, MyFixedPoint>();
						}
						value.ChangedItems.Add(num3, myFixedPoint);
=======
						if (inventoryDeltaInformation.ChangedItems == null)
						{
							inventoryDeltaInformation.ChangedItems = new Dictionary<uint, MyFixedPoint>();
						}
						inventoryDeltaInformation.ChangedItems.Add(num3, myFixedPoint);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else
					{
						Inventory.UpdateItemAmoutClient(num3, myFixedPoint);
					}
				}
			}
			if (stream.ReadBool())
			{
				int num4 = stream.ReadInt32();
				for (int j = 0; j < num4; j++)
				{
					uint num5 = stream.ReadUInt32();
					if (!flag2)
					{
						continue;
					}
					if (flag3)
					{
<<<<<<< HEAD
						if (value.RemovedItems == null)
						{
							value.RemovedItems = new List<uint>();
						}
						value.RemovedItems.Add(num5);
=======
						if (inventoryDeltaInformation.RemovedItems == null)
						{
							inventoryDeltaInformation.RemovedItems = new List<uint>();
						}
						inventoryDeltaInformation.RemovedItems.Add(num5);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else
					{
						Inventory.RemoveItemClient(num5);
					}
				}
			}
			if (stream.ReadBool())
			{
				int num6 = stream.ReadInt32();
				for (int k = 0; k < num6; k++)
				{
					int num7 = stream.ReadInt32();
<<<<<<< HEAD
					MySerializer.CreateAndRead<MyPhysicalInventoryItem>(stream, out var value2, MyObjectBuilderSerializer.Dynamic);
=======
					MySerializer.CreateAndRead<MyPhysicalInventoryItem>(stream, out var value, MyObjectBuilderSerializer.Dynamic);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (!flag2)
					{
						continue;
					}
					if (flag3)
					{
<<<<<<< HEAD
						if (value.NewItems == null)
						{
							value.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
						}
						value.NewItems.Add(num7, value2);
					}
					else
					{
						Inventory.AddItemClient(num7, value2);
=======
						if (inventoryDeltaInformation.NewItems == null)
						{
							inventoryDeltaInformation.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
						}
						inventoryDeltaInformation.NewItems.Add(num7, value);
					}
					else
					{
						Inventory.AddItemClient(num7, value);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			if (stream.ReadBool())
			{
				if (m_tmpSwappingList == null)
				{
					m_tmpSwappingList = new Dictionary<int, MyPhysicalInventoryItem>();
				}
				int num8 = stream.ReadInt32();
				for (int l = 0; l < num8; l++)
				{
					uint num9 = stream.ReadUInt32();
					int num10 = stream.ReadInt32();
					if (!flag2)
					{
						continue;
					}
					if (flag3)
					{
<<<<<<< HEAD
						if (value.SwappedItems == null)
						{
							value.SwappedItems = new Dictionary<uint, int>();
						}
						value.SwappedItems.Add(num9, num10);
=======
						if (inventoryDeltaInformation.SwappedItems == null)
						{
							inventoryDeltaInformation.SwappedItems = new Dictionary<uint, int>();
						}
						inventoryDeltaInformation.SwappedItems.Add(num9, num10);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else
					{
						MyPhysicalInventoryItem? itemByID = Inventory.GetItemByID(num9);
						if (itemByID.HasValue)
						{
							m_tmpSwappingList.Add(num10, itemByID.Value);
						}
					}
				}
				foreach (KeyValuePair<int, MyPhysicalInventoryItem> tmpSwapping in m_tmpSwappingList)
				{
					Inventory.ChangeItemClient(tmpSwapping.Value, tmpSwapping.Key);
				}
				m_tmpSwappingList.Clear();
<<<<<<< HEAD
			}
			if (flag3)
			{
				m_buffer.Add(num, value);
			}
=======
			}
			if (flag3)
			{
				m_buffer.Add(num, inventoryDeltaInformation);
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			else if (flag2)
			{
				FlushBuffer();
			}
			Inventory.Refresh();
		}

		private void FlushBuffer()
		{
			while (m_buffer.get_Count() > 0)
			{
				InventoryDeltaInformation changes = m_buffer.get_Values()[0];
				if (changes.MessageId == m_nextExpectedPacketId)
				{
					m_buffer.RemoveAt(0);
					if (CanProcessMessagePart(changes.SubId))
					{
						AddMessagePartBlock(changes.SubId);
						ApplyChangesOnClient(changes);
					}
					if (IsMessageFinished())
					{
						m_nextExpectedPacketId++;
					}
					continue;
				}
				break;
			}
		}

		private void ApplyChangesOnClient(InventoryDeltaInformation changes)
		{
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			if (changes.ChangedItems != null)
			{
				foreach (KeyValuePair<uint, MyFixedPoint> changedItem in changes.ChangedItems)
				{
					Inventory.UpdateItemAmoutClient(changedItem.Key, changedItem.Value);
				}
			}
			if (changes.RemovedItems != null)
			{
				foreach (uint removedItem in changes.RemovedItems)
				{
					Inventory.RemoveItemClient(removedItem);
				}
			}
			if (changes.NewItems != null)
			{
				Enumerator<int, MyPhysicalInventoryItem> enumerator3 = changes.NewItems.GetEnumerator();
				try
				{
<<<<<<< HEAD
					Inventory.AddItemClient(newItem.Key, newItem.Value);
				}
			}
			if (changes.SwappedItems == null)
			{
				return;
			}
			if (m_tmpSwappingList == null)
			{
				m_tmpSwappingList = new Dictionary<int, MyPhysicalInventoryItem>();
			}
			foreach (KeyValuePair<uint, int> swappedItem in changes.SwappedItems)
			{
				MyPhysicalInventoryItem? itemByID = Inventory.GetItemByID(swappedItem.Key);
				if (itemByID.HasValue)
				{
=======
					while (enumerator3.MoveNext())
					{
						KeyValuePair<int, MyPhysicalInventoryItem> current3 = enumerator3.get_Current();
						Inventory.AddItemClient(current3.Key, current3.Value);
					}
				}
				finally
				{
					((IDisposable)enumerator3).Dispose();
				}
			}
			if (changes.SwappedItems == null)
			{
				return;
			}
			if (m_tmpSwappingList == null)
			{
				m_tmpSwappingList = new Dictionary<int, MyPhysicalInventoryItem>();
			}
			foreach (KeyValuePair<uint, int> swappedItem in changes.SwappedItems)
			{
				MyPhysicalInventoryItem? itemByID = Inventory.GetItemByID(swappedItem.Key);
				if (itemByID.HasValue)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_tmpSwappingList.Add(swappedItem.Value, itemByID.Value);
				}
			}
			foreach (KeyValuePair<int, MyPhysicalInventoryItem> tmpSwapping in m_tmpSwappingList)
			{
				Inventory.ChangeItemClient(tmpSwapping.Value, tmpSwapping.Key);
			}
			m_tmpSwappingList.Clear();
		}

		private InventoryDeltaInformation CalculateInventoryDiff(ref InventoryClientData clientData)
		{
			if (m_itemsToSend == null)
			{
				m_itemsToSend = new List<MyPhysicalInventoryItem>();
			}
			if (m_foundDeltaItems == null)
			{
				m_foundDeltaItems = new HashSet<uint>();
			}
			m_foundDeltaItems.Clear();
			List<MyPhysicalInventoryItem> items = Inventory.GetItems();
			CalculateAddsAndRemovals(clientData, out var delta, items);
			if (delta.HasChanges)
			{
				ApplyChangesToClientItems(clientData, ref delta);
			}
			for (int i = 0; i < items.Count; i++)
			{
				if (i >= clientData.ClientItems.Count)
				{
					continue;
				}
				uint itemId = clientData.ClientItems[i].Item.ItemId;
				if (itemId == items[i].ItemId)
				{
					continue;
				}
				if (delta.SwappedItems == null)
				{
					delta.SwappedItems = new Dictionary<uint, int>();
				}
				for (int j = 0; j < items.Count; j++)
				{
					if (itemId == items[j].ItemId)
					{
						delta.SwappedItems[itemId] = j;
					}
				}
			}
			clientData.ClientItemsSorted.Clear();
			clientData.ClientItems.Clear();
			foreach (MyPhysicalInventoryItem item in items)
			{
				MyFixedPoint amount = item.Amount;
				MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
				if (myObjectBuilder_GasContainerObject != null)
				{
					amount = (MyFixedPoint)myObjectBuilder_GasContainerObject.GasLevel;
				}
				ClientInvetoryData clientInvetoryData = default(ClientInvetoryData);
				clientInvetoryData.Item = item;
				clientInvetoryData.Amount = amount;
				ClientInvetoryData clientInvetoryData2 = clientInvetoryData;
				clientData.ClientItemsSorted.set_Item(item.ItemId, clientInvetoryData2);
				clientData.ClientItems.Add(clientInvetoryData2);
			}
			return delta;
		}

		private static void ApplyChangesToClientItems(InventoryClientData clientData, ref InventoryDeltaInformation delta)
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			if (delta.RemovedItems != null)
			{
				foreach (uint removedItem in delta.RemovedItems)
				{
					int num = -1;
					for (int i = 0; i < clientData.ClientItems.Count; i++)
					{
						if (clientData.ClientItems[i].Item.ItemId == removedItem)
						{
							num = i;
							break;
						}
					}
					if (num != -1)
					{
						clientData.ClientItems.RemoveAt(num);
					}
				}
			}
			if (delta.NewItems == null)
<<<<<<< HEAD
			{
				return;
			}
			foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in delta.NewItems)
			{
				ClientInvetoryData clientInvetoryData = default(ClientInvetoryData);
				clientInvetoryData.Item = newItem.Value;
				clientInvetoryData.Amount = newItem.Value.Amount;
				ClientInvetoryData item = clientInvetoryData;
				if (newItem.Key >= clientData.ClientItems.Count)
				{
					clientData.ClientItems.Add(item);
				}
				else
				{
					clientData.ClientItems.Insert(newItem.Key, item);
=======
			{
				return;
			}
			Enumerator<int, MyPhysicalInventoryItem> enumerator2 = delta.NewItems.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, MyPhysicalInventoryItem> current2 = enumerator2.get_Current();
					ClientInvetoryData clientInvetoryData = default(ClientInvetoryData);
					clientInvetoryData.Item = current2.Value;
					clientInvetoryData.Amount = current2.Value.Amount;
					ClientInvetoryData item = clientInvetoryData;
					if (current2.Key >= clientData.ClientItems.Count)
					{
						clientData.ClientItems.Add(item);
					}
					else
					{
						clientData.ClientItems.Insert(current2.Key, item);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
		}

		private void CalculateAddsAndRemovals(InventoryClientData clientData, out InventoryDeltaInformation delta, List<MyPhysicalInventoryItem> items)
		{
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			delta = new InventoryDeltaInformation
			{
				HasChanges = false
			};
			int num = 0;
			ClientInvetoryData clientInvetoryData = default(ClientInvetoryData);
			foreach (MyPhysicalInventoryItem item in items)
			{
<<<<<<< HEAD
				if (clientData.ClientItemsSorted.TryGetValue(item.ItemId, out var value))
=======
				if (clientData.ClientItemsSorted.TryGetValue(item.ItemId, ref clientInvetoryData))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					if (clientInvetoryData.Item.Content.TypeId == item.Content.TypeId && clientInvetoryData.Item.Content.SubtypeId == item.Content.SubtypeId)
					{
						m_foundDeltaItems.Add(item.ItemId);
						MyFixedPoint myFixedPoint = item.Amount;
						MyObjectBuilder_GasContainerObject myObjectBuilder_GasContainerObject = item.Content as MyObjectBuilder_GasContainerObject;
						if (myObjectBuilder_GasContainerObject != null)
						{
							myFixedPoint = (MyFixedPoint)myObjectBuilder_GasContainerObject.GasLevel;
						}
						if (clientInvetoryData.Amount != myFixedPoint)
						{
							MyFixedPoint value = myFixedPoint - clientInvetoryData.Amount;
							if (delta.ChangedItems == null)
							{
								delta.ChangedItems = new Dictionary<uint, MyFixedPoint>();
							}
							delta.ChangedItems[item.ItemId] = value;
							delta.HasChanges = true;
						}
					}
				}
				else
				{
					if (delta.NewItems == null)
					{
						delta.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
					}
					delta.NewItems.set_Item(num, item);
					delta.HasChanges = true;
				}
				num++;
			}
			Enumerator<uint, ClientInvetoryData> enumerator2 = clientData.ClientItemsSorted.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<uint, ClientInvetoryData> current2 = enumerator2.get_Current();
					if (delta.RemovedItems == null)
					{
						delta.RemovedItems = new List<uint>();
					}
					if (!m_foundDeltaItems.Contains(current2.Key))
					{
						delta.RemovedItems.Add(current2.Key);
						delta.HasChanges = true;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
		}

		private InventoryDeltaInformation PrepareWrite(ref InventoryDeltaInformation packetInfo, BitStream stream, int maxBitPosition, out bool needsSplit)
		{
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			InventoryDeltaInformation result = PrepareSendData(ref packetInfo, stream, maxBitPosition, out needsSplit);
			result.MessageId = packetInfo.MessageId;
			result.SubId = packetInfo.SubId;
			return result;
		}

		private void WriteInventory(ref InventoryDeltaInformation sendPacketInfo, BitStream stream, int maxBitPosition)
		{
			stream.WriteBool(sendPacketInfo.HasChanges);
			stream.WriteUInt32(sendPacketInfo.MessageId);
			stream.WriteByte(sendPacketInfo.SubId);
			if (!sendPacketInfo.HasChanges)
			{
				return;
			}
			stream.WriteBool(sendPacketInfo.ChangedItems != null);
			if (sendPacketInfo.ChangedItems != null)
			{
				stream.WriteInt32(sendPacketInfo.ChangedItems.Count);
				foreach (KeyValuePair<uint, MyFixedPoint> changedItem in sendPacketInfo.ChangedItems)
				{
					stream.WriteUInt32(changedItem.Key);
					stream.WriteInt64(changedItem.Value.RawValue);
				}
			}
			stream.WriteBool(sendPacketInfo.RemovedItems != null);
			if (sendPacketInfo.RemovedItems != null)
			{
				stream.WriteInt32(sendPacketInfo.RemovedItems.Count);
				foreach (uint removedItem in sendPacketInfo.RemovedItems)
				{
					stream.WriteUInt32(removedItem);
				}
			}
			stream.WriteBool(sendPacketInfo.NewItems != null);
			if (sendPacketInfo.NewItems != null)
			{
<<<<<<< HEAD
				stream.WriteInt32(sendPacketInfo.NewItems.Count);
				foreach (KeyValuePair<int, MyPhysicalInventoryItem> newItem in sendPacketInfo.NewItems)
=======
				stream.WriteInt32(result.NewItems.get_Count());
				Enumerator<int, MyPhysicalInventoryItem> enumerator3 = result.NewItems.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						KeyValuePair<int, MyPhysicalInventoryItem> current3 = enumerator3.get_Current();
						stream.WriteInt32(current3.Key);
						MyPhysicalInventoryItem value = current3.Value;
						MySerializer.Write(stream, ref value, MyObjectBuilderSerializer.Dynamic);
					}
				}
				finally
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					((IDisposable)enumerator3).Dispose();
				}
			}
			stream.WriteBool(sendPacketInfo.SwappedItems != null);
			if (sendPacketInfo.SwappedItems == null)
			{
				return;
			}
			stream.WriteInt32(sendPacketInfo.SwappedItems.Count);
			foreach (KeyValuePair<uint, int> swappedItem in sendPacketInfo.SwappedItems)
			{
				stream.WriteUInt32(swappedItem.Key);
				stream.WriteInt32(swappedItem.Value);
			}
		}

		private InventoryDeltaInformation WriteInventoryComplete(ref InventoryDeltaInformation packetInfo, BitStream stream, byte packetId, int maxBitPosition, out bool needsSplit)
		{
			InventoryDeltaInformation sendPacketInfo = PrepareWrite(ref packetInfo, stream, maxBitPosition, out needsSplit);
			WriteInventory(ref sendPacketInfo, stream, maxBitPosition);
			return sendPacketInfo;
		}

		private InventoryDeltaInformation PrepareSendData(ref InventoryDeltaInformation packetInfo, BitStream stream, int maxBitPosition, out bool needsSplit)
		{
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			needsSplit = false;
			long bitPosition = stream.BitPosition;
			InventoryDeltaInformation inventoryDeltaInformation = default(InventoryDeltaInformation);
			inventoryDeltaInformation.HasChanges = false;
			InventoryDeltaInformation result = inventoryDeltaInformation;
			stream.WriteBool(value: false);
			stream.WriteUInt32(packetInfo.MessageId);
			stream.WriteBool(packetInfo.ChangedItems != null);
			if (packetInfo.ChangedItems != null)
			{
				stream.WriteInt32(packetInfo.ChangedItems.Count);
				if (stream.BitPosition > maxBitPosition)
				{
					needsSplit = true;
				}
				else
				{
					result.ChangedItems = new Dictionary<uint, MyFixedPoint>();
					foreach (KeyValuePair<uint, MyFixedPoint> changedItem in packetInfo.ChangedItems)
					{
						stream.WriteUInt32(changedItem.Key);
						stream.WriteInt64(changedItem.Value.RawValue);
						if (stream.BitPosition <= maxBitPosition)
						{
							result.ChangedItems[changedItem.Key] = changedItem.Value;
							result.HasChanges = true;
						}
						else
						{
							needsSplit = true;
						}
					}
				}
			}
			stream.WriteBool(packetInfo.RemovedItems != null);
			if (packetInfo.RemovedItems != null)
			{
				stream.WriteInt32(packetInfo.RemovedItems.Count);
				if (stream.BitPosition > maxBitPosition)
				{
					needsSplit = true;
				}
				else
				{
					result.RemovedItems = new List<uint>();
					foreach (uint removedItem in packetInfo.RemovedItems)
					{
						stream.WriteUInt32(removedItem);
						if (stream.BitPosition <= maxBitPosition)
						{
							result.RemovedItems.Add(removedItem);
							result.HasChanges = true;
						}
						else
						{
							needsSplit = true;
						}
					}
				}
			}
			stream.WriteBool(packetInfo.NewItems != null);
			if (packetInfo.NewItems != null)
			{
				stream.WriteInt32(packetInfo.NewItems.get_Count());
				if (stream.BitPosition > maxBitPosition)
				{
					needsSplit = true;
				}
				else
				{
					result.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
					Enumerator<int, MyPhysicalInventoryItem> enumerator3 = packetInfo.NewItems.GetEnumerator();
					try
					{
<<<<<<< HEAD
						MyPhysicalInventoryItem value = newItem.Value;
						stream.WriteInt32(newItem.Key);
						long bitPosition2 = stream.BitPosition;
						MySerializer.Write(stream, ref value, MyObjectBuilderSerializer.Dynamic);
						_ = stream.BitPosition;
						if (stream.BitPosition <= maxBitPosition)
						{
							result.NewItems[newItem.Key] = value;
							result.HasChanges = true;
						}
						else
=======
						while (enumerator3.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							KeyValuePair<int, MyPhysicalInventoryItem> current3 = enumerator3.get_Current();
							MyPhysicalInventoryItem value = current3.Value;
							stream.WriteInt32(current3.Key);
							long bitPosition2 = stream.BitPosition;
							MySerializer.Write(stream, ref value, MyObjectBuilderSerializer.Dynamic);
							_ = stream.BitPosition;
							if (stream.BitPosition <= maxBitPosition)
							{
								result.NewItems.set_Item(current3.Key, value);
								result.HasChanges = true;
							}
							else
							{
								needsSplit = true;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator3).Dispose();
					}
				}
			}
			stream.WriteBool(packetInfo.SwappedItems != null);
			if (packetInfo.SwappedItems != null)
			{
				stream.WriteInt32(packetInfo.SwappedItems.Count);
				if (stream.BitPosition > maxBitPosition)
				{
					needsSplit = true;
				}
				else
				{
					result.SwappedItems = new Dictionary<uint, int>();
					foreach (KeyValuePair<uint, int> swappedItem in packetInfo.SwappedItems)
					{
						stream.WriteUInt32(swappedItem.Key);
						stream.WriteInt32(swappedItem.Value);
						if (stream.BitPosition <= maxBitPosition)
						{
							result.SwappedItems[swappedItem.Key] = swappedItem.Value;
							result.HasChanges = true;
						}
						else
						{
							needsSplit = true;
						}
					}
				}
			}
			stream.SetBitPositionWrite(bitPosition);
			return result;
		}

		private InventoryDeltaInformation CreateSplit(ref InventoryDeltaInformation originalData, ref InventoryDeltaInformation sentData)
		{
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			InventoryDeltaInformation inventoryDeltaInformation = default(InventoryDeltaInformation);
			inventoryDeltaInformation.MessageId = sentData.MessageId;
			InventoryDeltaInformation result = inventoryDeltaInformation;
			if (originalData.ChangedItems != null)
			{
				if (sentData.ChangedItems == null)
				{
					result.ChangedItems = new Dictionary<uint, MyFixedPoint>();
					foreach (KeyValuePair<uint, MyFixedPoint> changedItem in originalData.ChangedItems)
					{
						result.ChangedItems[changedItem.Key] = changedItem.Value;
					}
				}
				else if (originalData.ChangedItems.Count != sentData.ChangedItems.Count)
				{
					result.ChangedItems = new Dictionary<uint, MyFixedPoint>();
					foreach (KeyValuePair<uint, MyFixedPoint> changedItem2 in originalData.ChangedItems)
					{
						if (!sentData.ChangedItems.ContainsKey(changedItem2.Key))
						{
							result.ChangedItems[changedItem2.Key] = changedItem2.Value;
						}
					}
				}
			}
			if (originalData.RemovedItems != null)
			{
				if (sentData.RemovedItems == null)
				{
					result.RemovedItems = new List<uint>();
					foreach (uint removedItem in originalData.RemovedItems)
					{
						result.RemovedItems.Add(removedItem);
					}
				}
				else if (originalData.RemovedItems.Count != sentData.RemovedItems.Count)
				{
					result.RemovedItems = new List<uint>();
					foreach (uint removedItem2 in originalData.RemovedItems)
					{
						if (!sentData.RemovedItems.Contains(removedItem2))
						{
							result.RemovedItems.Add(removedItem2);
						}
					}
				}
			}
			if (originalData.NewItems != null)
			{
				if (sentData.NewItems == null)
				{
					result.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
					Enumerator<int, MyPhysicalInventoryItem> enumerator3 = originalData.NewItems.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							KeyValuePair<int, MyPhysicalInventoryItem> current5 = enumerator3.get_Current();
							result.NewItems.set_Item(current5.Key, current5.Value);
						}
					}
					finally
					{
						((IDisposable)enumerator3).Dispose();
					}
				}
				else if (originalData.NewItems.get_Count() != sentData.NewItems.get_Count())
				{
					result.NewItems = new SortedDictionary<int, MyPhysicalInventoryItem>();
					Enumerator<int, MyPhysicalInventoryItem> enumerator3 = originalData.NewItems.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							KeyValuePair<int, MyPhysicalInventoryItem> current6 = enumerator3.get_Current();
							if (!sentData.NewItems.ContainsKey(current6.Key))
							{
								result.NewItems.set_Item(current6.Key, current6.Value);
							}
						}
					}
					finally
					{
						((IDisposable)enumerator3).Dispose();
					}
				}
			}
			if (originalData.SwappedItems != null)
			{
				if (sentData.SwappedItems == null)
				{
					result.SwappedItems = new Dictionary<uint, int>();
					{
						foreach (KeyValuePair<uint, int> swappedItem in originalData.SwappedItems)
						{
							result.SwappedItems[swappedItem.Key] = swappedItem.Value;
						}
						return result;
					}
				}
				if (originalData.SwappedItems.Count != sentData.SwappedItems.Count)
				{
					result.SwappedItems = new Dictionary<uint, int>();
					{
						foreach (KeyValuePair<uint, int> swappedItem2 in originalData.SwappedItems)
						{
							if (!sentData.SwappedItems.ContainsKey(swappedItem2.Key))
							{
								result.SwappedItems[swappedItem2.Key] = swappedItem2.Value;
							}
						}
						return result;
					}
				}
			}
			return result;
		}

		public void OnAck(MyClientStateBase forClient, byte packetId, bool delivered)
		{
			if (m_clientInventoryUpdate != null && m_clientInventoryUpdate.TryGetValue(forClient.EndpointId, out var value) && value.SendPackets.TryGetValue(packetId, out var value2))
			{
				if (!delivered)
				{
					value.FailedIncompletePackets.Add(value2);
					MyMultiplayer.GetReplicationServer().AddToDirtyGroups(this);
				}
				value.SendPackets.Remove(packetId);
			}
		}

		public void ForceSend(MyClientStateBase clientData)
		{
		}

		public void Reset(bool reinit, MyTimeSpan clientTimestamp)
		{
		}

		public bool IsStillDirty(Endpoint forClient)
		{
			if (m_clientInventoryUpdate != null && m_clientInventoryUpdate.TryGetValue(forClient, out var value))
			{
				if (!value.Dirty)
				{
					return value.FailedIncompletePackets.Count != 0;
				}
				return true;
			}
			return true;
		}

		public MyStreamProcessingState IsProcessingForClient(Endpoint forClient)
		{
			return MyStreamProcessingState.None;
		}
	}
}
