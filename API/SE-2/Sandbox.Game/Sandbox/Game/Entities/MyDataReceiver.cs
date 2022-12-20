<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using VRage.Game.Components;
using VRage.Game.Gui;
using VRage.Groups;

namespace Sandbox.Game.Entities
{
	public abstract class MyDataReceiver : MyEntityComponentBase
	{
		public delegate void BroadcasterChangeInfo(MyDataBroadcaster broadcaster);

		protected List<MyDataBroadcaster> m_tmpBroadcasters = new List<MyDataBroadcaster>();

		protected HashSet<MyDataBroadcaster> m_broadcastersInRange = new HashSet<MyDataBroadcaster>();

		protected List<MyDataBroadcaster> m_lastBroadcastersInRange = new List<MyDataBroadcaster>();

		private HashSet<MyGridLogicalGroupData> m_broadcastersInRange_TopGrids = new HashSet<MyGridLogicalGroupData>();

		private HashSet<long> m_entitiesOnHud = new HashSet<long>();

		public bool Enabled { get; set; }

		public HashSet<MyDataBroadcaster> BroadcastersInRange => m_broadcastersInRange;

		public MyDataBroadcaster Broadcaster
		{
			get
			{
				MyDataBroadcaster component = null;
				if (base.Container != null)
				{
					base.Container.TryGet<MyDataBroadcaster>(out component);
				}
				return component;
			}
		}

		public override string ComponentTypeDebugString => "MyDataReciever";

		public event BroadcasterChangeInfo OnBroadcasterFound;

		public event BroadcasterChangeInfo OnBroadcasterLost;

		public void UpdateBroadcastersInRange()
		{
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			m_broadcastersInRange.Clear();
			if (!MyFakes.ENABLE_RADIO_HUD || !Enabled)
			{
				return;
			}
			if (base.Entity.Components.TryGet<MyDataBroadcaster>(out var component))
			{
				m_broadcastersInRange.Add(component);
			}
			GetBroadcastersInMyRange(ref m_broadcastersInRange);
			HashSet<long> hashSet = new HashSet<long>();
			HashSet<long> hashSet2 = new HashSet<long>();
			foreach (MyDataBroadcaster item in m_broadcastersInRange)
			{
				if (item.Entity != null)
				{
					hashSet.Add(item.AntennaEntityId);
				}
			}
			foreach (MyDataBroadcaster item2 in m_lastBroadcastersInRange)
			{
				if (item2.Entity != null)
				{
					hashSet2.Add(item2.AntennaEntityId);
				}
			}
			for (int num = m_lastBroadcastersInRange.Count - 1; num >= 0; num--)
			{
				MyDataBroadcaster myDataBroadcaster = m_lastBroadcastersInRange[num];
				if (myDataBroadcaster.Entity != null && !hashSet2.Contains(myDataBroadcaster.Entity.EntityId))
				{
					m_lastBroadcastersInRange.RemoveAtFast(num);
					this.OnBroadcasterLost?.Invoke(myDataBroadcaster);
				}
			}
<<<<<<< HEAD
			foreach (MyDataBroadcaster item3 in m_broadcastersInRange)
			{
				if (item3.Entity != null && !hashSet2.Contains(item3.AntennaEntityId))
				{
					m_lastBroadcastersInRange.Add(item3);
					this.OnBroadcasterFound?.Invoke(item3);
=======
			Enumerator<MyDataBroadcaster> enumerator = m_broadcastersInRange.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDataBroadcaster current = enumerator.get_Current();
					if (!m_lastBroadcastersInRange.Contains(current))
					{
						m_lastBroadcastersInRange.Add(current);
						this.OnBroadcasterFound?.Invoke(current);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public bool CanBeUsedByPlayer(long playerId)
		{
			return MyDataBroadcaster.CanBeUsedByPlayer(playerId, base.Entity);
		}

		protected abstract void GetBroadcastersInMyRange(ref HashSet<MyDataBroadcaster> broadcastersInRange);

		public void UpdateHud(bool showMyself = false)
		{
<<<<<<< HEAD
			if (Sandbox.Engine.Platform.Game.IsDedicated || MyHud.MinimalHud || MyHud.CutsceneHud)
			{
				return;
			}
			Clear();
			foreach (MyDataBroadcaster allRelayedBroadcaster in MyAntennaSystem.Static.GetAllRelayedBroadcasters(this, MySession.Static.LocalPlayerId, mutual: false))
			{
				bool allowBlink = allRelayedBroadcaster.CanBeUsedByPlayer(MySession.Static.LocalPlayerId);
				MyCubeGrid myCubeGrid = allRelayedBroadcaster.Entity.GetTopMostParent() as MyCubeGrid;
				if (myCubeGrid != null && !allRelayedBroadcaster.IsBeacon)
				{
					MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(myCubeGrid);
					if (group != null)
=======
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (Sandbox.Engine.Platform.Game.IsDedicated || MyHud.MinimalHud || MyHud.CutsceneHud)
			{
				return;
			}
			Clear();
			Enumerator<MyDataBroadcaster> enumerator = MyAntennaSystem.Static.GetAllRelayedBroadcasters(this, MySession.Static.LocalPlayerId, mutual: false).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDataBroadcaster current = enumerator.get_Current();
					bool allowBlink = current.CanBeUsedByPlayer(MySession.Static.LocalPlayerId);
					MyCubeGrid myCubeGrid = current.Entity.GetTopMostParent() as MyCubeGrid;
					if (myCubeGrid != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyGridLogicalGroupData groupData = group.GroupData;
						m_broadcastersInRange_TopGrids.Add(groupData);
					}
<<<<<<< HEAD
				}
				if (!allRelayedBroadcaster.ShowOnHud)
				{
					continue;
				}
				foreach (MyHudEntityParams hudParam in allRelayedBroadcaster.GetHudParams(allowBlink))
				{
					if (!m_entitiesOnHud.Contains(hudParam.EntityId))
					{
						m_entitiesOnHud.Add(hudParam.EntityId);
						if (hudParam.BlinkingTime > 0f)
						{
							MyHud.HackingMarkers.RegisterMarker(hudParam.EntityId, hudParam);
						}
						else if (!MyHud.HackingMarkers.MarkerEntities.ContainsKey(hudParam.EntityId))
						{
							MyHud.LocationMarkers.RegisterMarker(hudParam.EntityId, hudParam);
=======
					if (!current.ShowOnHud)
					{
						continue;
					}
					foreach (MyHudEntityParams hudParam in current.GetHudParams(allowBlink))
					{
						if (!m_entitiesOnHud.Contains(hudParam.EntityId))
						{
							m_entitiesOnHud.Add(hudParam.EntityId);
							if (hudParam.BlinkingTime > 0f)
							{
								MyHud.HackingMarkers.RegisterMarker(hudParam.EntityId, hudParam);
							}
							else if (!MyHud.HackingMarkers.MarkerEntities.ContainsKey(hudParam.EntityId))
							{
								MyHud.LocationMarkers.RegisterMarker(hudParam.EntityId, hudParam);
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
			}
<<<<<<< HEAD
=======
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.ShowPlayers))
			{
				return;
			}
			foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
			{
				MyCharacter character = onlinePlayer.Character;
<<<<<<< HEAD
				if (character?.ControllerInfo?.Controller?.Player == null || character.ControllerInfo.Controller.Player.IsWildlifeAgent)
				{
					continue;
				}
				foreach (MyHudEntityParams hudParam2 in character.GetHudParams(allowBlink: false))
				{
=======
				if (character == null)
				{
					continue;
				}
				foreach (MyHudEntityParams hudParam2 in character.GetHudParams(allowBlink: false))
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (!m_entitiesOnHud.Contains(hudParam2.EntityId))
					{
						m_entitiesOnHud.Add(hudParam2.EntityId);
						MyHud.LocationMarkers.RegisterMarker(hudParam2.EntityId, hudParam2);
					}
				}
			}
		}

		public bool HasAccessToLogicalGroup(MyGridLogicalGroupData group)
		{
			return m_broadcastersInRange_TopGrids.Contains(group);
		}

		public void Clear()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<long> enumerator = m_entitiesOnHud.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					long current = enumerator.get_Current();
					MyHud.LocationMarkers.UnregisterMarker(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_entitiesOnHud.Clear();
			m_broadcastersInRange_TopGrids.Clear();
		}
	}
}
