using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Network;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	/// <summary>
	/// System designed to propagate highlights over the network. 
	/// The replication happens only for server calls.
	/// Client side cannot ask for highlights on other clients.
	/// </summary>
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyHighlightSystem : MySessionComponentBase
	{
		public class ExclusiveHighlightIdentification : IEquatable<ExclusiveHighlightIdentification>
		{
			public long EntityId { get; private set; }

			public string SectionName { get; private set; }

			public ExclusiveHighlightIdentification(long entityId, string sectionName)
			{
				EntityId = entityId;
				SectionName = sectionName;
			}

			public ExclusiveHighlightIdentification(long entityId, string[] sectionNames)
				: this(entityId, (sectionNames == null) ? string.Empty : string.Join(";", sectionNames))
			{
			}

			public bool Equals(ExclusiveHighlightIdentification other)
			{
				if (other != null && EntityId == other.EntityId)
				{
					return SectionName.Equals(other.SectionName, StringComparison.InvariantCulture);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (-1285426570 * -1521134295 + EntityId.GetHashCode()) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SectionName);
			}
		}

		[Serializable]
		private struct HighlightMsg
		{
			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EHighlightMsg_003C_003EData_003C_003EAccessor : IMemberAccessor<HighlightMsg, MyHighlightData>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref HighlightMsg owner, in MyHighlightData value)
				{
					owner.Data = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref HighlightMsg owner, out MyHighlightData value)
				{
					value = owner.Data;
				}
			}

			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EHighlightMsg_003C_003EExclusiveKey_003C_003EAccessor : IMemberAccessor<HighlightMsg, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref HighlightMsg owner, in int value)
				{
					owner.ExclusiveKey = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref HighlightMsg owner, out int value)
				{
					value = owner.ExclusiveKey;
				}
			}

			protected class Sandbox_Game_SessionComponents_MyHighlightSystem_003C_003EHighlightMsg_003C_003EIsExclusive_003C_003EAccessor : IMemberAccessor<HighlightMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref HighlightMsg owner, in bool value)
				{
					owner.IsExclusive = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref HighlightMsg owner, out bool value)
				{
					value = owner.IsExclusive;
				}
			}

			public MyHighlightData Data;

			public int ExclusiveKey;

			public bool IsExclusive;
		}

		protected sealed class OnHighlightOnClient_003C_003ESandbox_Game_SessionComponents_MyHighlightSystem_003C_003EHighlightMsg : ICallSite<IMyEventOwner, HighlightMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in HighlightMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnHighlightOnClient(msg);
			}
		}

		protected sealed class OnRequestRejected_003C_003ESandbox_Game_SessionComponents_MyHighlightSystem_003C_003EHighlightMsg : ICallSite<IMyEventOwner, HighlightMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in HighlightMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnRequestRejected(msg);
			}
		}

		protected sealed class OnRequestAccepted_003C_003ESandbox_Game_SessionComponents_MyHighlightSystem_003C_003EHighlightMsg : ICallSite<IMyEventOwner, HighlightMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in HighlightMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnRequestAccepted(msg);
			}
		}

		private static MyHighlightSystem m_static;

		private static int m_exclusiveKeyCounter = 10;

		private readonly Dictionary<int, ExclusiveHighlightIdentification> m_exclusiveKeysToIds = new Dictionary<int, ExclusiveHighlightIdentification>();

		private readonly HashSet<long> m_highlightedIds = new HashSet<long>();

		private readonly MyHudSelectedObject m_highlightCalculationHelper = new MyHudSelectedObject();

		private readonly List<uint> m_subPartIndicies = new List<uint>();

		private readonly HashSet<uint> m_highlightOverlappingIds = new HashSet<uint>();

		private readonly Dictionary<int, MyHighlightData> m_exclusiveHighlightsData = new Dictionary<int, MyHighlightData>();

		private readonly Dictionary<long, MyHighlightData> m_clientExclusiveHighlightsDataCache = new Dictionary<long, MyHighlightData>();

		private readonly List<long> m_toRemove = new List<long>();

		private StringBuilder m_highlightAttributeBuilder = new StringBuilder();

		public HashSetReader<uint> HighlightOverlappingRenderIds => new HashSetReader<uint>(m_highlightOverlappingIds);

		public event Action<MyHighlightData> HighlightRejected;

		public event Action<MyHighlightData> HighlightAccepted;

		public event Action<MyHighlightData, int> ExclusiveHighlightRejected;

		public event Action<MyHighlightData, int> ExclusiveHighlightAccepted;

		public MyHighlightSystem()
		{
			m_static = this;
			ExclusiveHighlightAccepted += OnExclusiveHighlightAccepted;
			MyEntities.OnEntityAdd += MyEntities_OnEntityAdd;
		}

		/// <summary>
		/// Releases static reference
		/// </summary>
		protected override void UnloadData()
		{
			MyEntities.OnEntityAdd -= MyEntities_OnEntityAdd;
			m_clientExclusiveHighlightsDataCache.Clear();
			base.UnloadData();
			m_static = null;
		}

		/// <summary>
		/// Requests highlight from render proxy. The call is handled locally for
		/// default player id. Only server can use the player id to propagate the 
		/// Highlight calls to clients.
		/// </summary>
		/// <param name="data">Highlight data wrapper.</param>        
		public void RequestHighlightChange(MyHighlightData data)
		{
			ProcessRequest(data, -1, isExclusive: false);
		}

		/// <summary>
		/// Requests Exclusive highlight from render proxy. The call is handled locally for
		/// default player id. Only server can use the player id to propagate the 
		/// Highlight calls to clients.
		/// Uses Exclusive key as lock accessor. Can be obtained from ExclusiveHighlightAccepted event.
		/// </summary>
		/// <param name="data">Highlight data wrapper.</param>
		/// <param name="exclusiveKey">Exclusive key.</param>        
		public void RequestHighlightChangeExclusive(MyHighlightData data, int exclusiveKey = -1)
		{
			ProcessRequest(data, exclusiveKey, isExclusive: true);
		}

		/// <summary>
		/// Determines whenever is the entity highlighted by the system or not.
		/// </summary>
		/// <param name="entityId">Id of entity.</param>
		/// <returns>Highlighted.</returns>
		public bool IsHighlighted(long entityId)
		{
			return m_highlightedIds.Contains(entityId);
		}

<<<<<<< HEAD
		/// <summary>
		/// Is the entity locked for highlights by the system?
		/// </summary>
		/// <param name="highlightId">Id of the entity.</param>
		/// <returns>Reserved value.</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool IsReserved(ExclusiveHighlightIdentification highlightId)
		{
			return m_exclusiveKeysToIds.ContainsValue(highlightId);
		}

		/// <summary>
		/// Makes the model of provided id overlap highlights.
		/// </summary>
		/// <param name="modelRenderId">Actor render id.</param>
		public void AddHighlightOverlappingModel(uint modelRenderId)
		{
			if (modelRenderId != uint.MaxValue && !m_highlightOverlappingIds.Contains(modelRenderId))
			{
				m_highlightOverlappingIds.Add(modelRenderId);
				MyRenderProxy.UpdateHighlightOverlappingModel(modelRenderId);
			}
		}

		/// <summary>
		/// The model with of provided id will be overlapped by highlights again.
		/// </summary>
		/// <param name="modelRenderId">Actor render id.</param>
		public void RemoveHighlightOverlappingModel(uint modelRenderId)
		{
			if (modelRenderId != uint.MaxValue && m_highlightOverlappingIds.Contains(modelRenderId))
			{
				m_highlightOverlappingIds.Remove(modelRenderId);
				MyRenderProxy.UpdateHighlightOverlappingModel(modelRenderId, enable: false);
			}
		}

		public override void SaveData()
		{
			if (MyCampaignManager.Static != null && MyCampaignManager.Static.IsCampaignRunning)
			{
				MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
				if (component != null)
				{
					component.ExclusiveHighlightsData = GetExclusiveHighlightsObjectBuilder();
				}
			}
		}

		public MyObjectBuilder_ExclusiveHighlights GetExclusiveHighlightsObjectBuilder()
		{
			MyObjectBuilder_ExclusiveHighlights myObjectBuilder_ExclusiveHighlights = new MyObjectBuilder_ExclusiveHighlights();
			myObjectBuilder_ExclusiveHighlights.ExclusiveHighlightData.AddRange(m_exclusiveHighlightsData.Values);
			return myObjectBuilder_ExclusiveHighlights;
		}

		public override void BeforeStart()
		{
			MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
			if (component == null)
			{
				return;
			}
			MyObjectBuilder_ExclusiveHighlights exclusiveHighlightsData = component.ExclusiveHighlightsData;
			if (exclusiveHighlightsData == null)
			{
				return;
			}
			foreach (MyHighlightData exclusiveHighlightDatum in exclusiveHighlightsData.ExclusiveHighlightData)
			{
				MyVisualScriptLogicProvider.SetHighlight(exclusiveHighlightDatum, exclusiveHighlightDatum.PlayerId);
				if (!Sync.IsServer)
				{
					m_clientExclusiveHighlightsDataCache[exclusiveHighlightDatum.EntityId] = exclusiveHighlightDatum;
				}
			}
		}

		private void MyEntities_OnEntityAdd(MyEntity entity)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (Sync.IsServer || myCubeGrid == null || m_clientExclusiveHighlightsDataCache.Count <= 0)
			{
				return;
			}
			foreach (KeyValuePair<long, MyHighlightData> item in m_clientExclusiveHighlightsDataCache)
			{
				if (MyEntities.GetEntityById(item.Key) != null)
				{
					MyVisualScriptLogicProvider.SetHighlight(item.Value, item.Value.PlayerId);
					m_toRemove.Add(item.Key);
				}
			}
			foreach (long item2 in m_toRemove)
			{
				m_clientExclusiveHighlightsDataCache.Remove(item2);
			}
			m_toRemove.Clear();
		}

		private void ProcessRequest(MyHighlightData data, int exclusiveKey, bool isExclusive)
		{
			if (data.PlayerId == -1)
			{
				data.PlayerId = MySession.Static.LocalPlayerId;
			}
			if ((MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer) && data.PlayerId != MySession.Static.LocalPlayerId)
			{
				if (MySession.Static.Players.TryGetPlayerId(data.PlayerId, out var result))
				{
					HighlightMsg highlightMsg = default(HighlightMsg);
					highlightMsg.Data = data;
					highlightMsg.ExclusiveKey = exclusiveKey;
					highlightMsg.IsExclusive = isExclusive;
					HighlightMsg arg = highlightMsg;
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnHighlightOnClient, arg, new EndpointId(result.SteamId));
				}
				return;
			}
			bool flag = data.Thickness > -1;
			ExclusiveHighlightIdentification exclusiveHighlightIdentification = new ExclusiveHighlightIdentification(data.EntityId, data.SubPartNames);
			if (m_exclusiveKeysToIds.ContainsValue(exclusiveHighlightIdentification) && (!m_exclusiveKeysToIds.TryGetValue(exclusiveKey, out var value) || !value.Equals(exclusiveHighlightIdentification)))
			{
				if (this.HighlightRejected != null)
				{
					this.HighlightRejected(data);
				}
			}
			else if (isExclusive)
			{
				if (exclusiveKey == -1)
				{
					exclusiveKey = m_exclusiveKeyCounter++;
				}
				if (flag)
				{
					if (!m_exclusiveKeysToIds.ContainsKey(exclusiveKey))
					{
						m_exclusiveKeysToIds.Add(exclusiveKey, exclusiveHighlightIdentification);
					}
				}
				else
				{
					m_exclusiveKeysToIds.Remove(exclusiveKey);
				}
				MakeLocalHighlightChange(data);
				if (this.ExclusiveHighlightAccepted != null)
				{
					this.ExclusiveHighlightAccepted(data, exclusiveKey);
				}
			}
			else
			{
				MakeLocalHighlightChange(data);
				if (this.HighlightAccepted != null)
				{
					this.HighlightAccepted(data);
				}
			}
		}

		private void MakeLocalHighlightChange(MyHighlightData data)
		{
			if (data.Thickness > -1)
			{
				m_highlightedIds.Add(data.EntityId);
			}
			else
			{
				m_highlightedIds.Remove(data.EntityId);
			}
			if (!MyEntities.TryGetEntityById(data.EntityId, out var entity))
			{
				return;
			}
			if (!data.IgnoreUseObjectData)
			{
				IMyUseObject myUseObject = entity as IMyUseObject;
				MyUseObjectsComponentBase myUseObjectsComponentBase = entity.Components.Get<MyUseObjectsComponentBase>();
				if (myUseObject != null || myUseObjectsComponentBase != null)
				{
					if (myUseObjectsComponentBase == null)
					{
						HighlightUseObject(myUseObject, data);
						if (this.HighlightAccepted != null)
						{
							this.HighlightAccepted(data);
						}
						return;
					}
					List<IMyUseObject> list = new List<IMyUseObject>();
					myUseObjectsComponentBase.GetInteractiveObjects(list);
					for (int i = 0; i < list.Count; i++)
					{
						HighlightUseObject(list[i], data);
					}
					if (list.Count > 0)
					{
						if (this.HighlightAccepted != null)
						{
							this.HighlightAccepted(data);
						}
						return;
					}
				}
			}
			m_subPartIndicies.Clear();
			CollectSubPartIndicies(entity);
			uint[] renderObjectIDs = entity.Render.RenderObjectIDs;
			for (int j = 0; j < renderObjectIDs.Length; j++)
			{
				MyRenderProxy.UpdateModelHighlight(renderObjectIDs[j], null, m_subPartIndicies.ToArray(), data.OutlineColor, data.Thickness, data.PulseTimeInFrames);
			}
			if (this.HighlightAccepted != null)
			{
				this.HighlightAccepted(data);
			}
		}

		private void CollectSubPartIndicies(MyEntity currentEntity)
		{
			if (currentEntity.Subparts == null || currentEntity.Render == null)
			{
				return;
			}
			foreach (MyEntitySubpart value in currentEntity.Subparts.Values)
			{
				CollectSubPartIndicies(value);
				m_subPartIndicies.AddRange(value.Render.RenderObjectIDs);
			}
		}

		private void HighlightUseObject(IMyUseObject useObject, MyHighlightData data)
		{
			m_highlightCalculationHelper.HighlightAttribute = null;
			if (useObject.Dummy != null)
			{
				useObject.Dummy.CustomData.TryGetValue("highlight", out var value);
				string text = value as string;
				if (text == null)
				{
					return;
				}
				if (data.SubPartNames != null)
				{
					m_highlightAttributeBuilder.Clear();
					string[] array = data.SubPartNames.Split(new char[1] { ';' });
					foreach (string value2 in array)
					{
						if (text.Contains(value2))
						{
							m_highlightAttributeBuilder.Append(value2).Append(';');
						}
					}
					if (m_highlightAttributeBuilder.Length > 0)
					{
						m_highlightAttributeBuilder.TrimEnd(1);
					}
					m_highlightCalculationHelper.HighlightAttribute = m_highlightAttributeBuilder.ToString();
				}
				else
				{
					m_highlightCalculationHelper.HighlightAttribute = text;
				}
				if (string.IsNullOrEmpty(m_highlightCalculationHelper.HighlightAttribute))
				{
					return;
				}
			}
			m_highlightCalculationHelper.Highlight(useObject);
			MyRenderProxy.UpdateModelHighlight(m_highlightCalculationHelper.InteractiveObject.RenderObjectID, m_highlightCalculationHelper.SectionNames, m_highlightCalculationHelper.SubpartIndices, data.OutlineColor, data.Thickness, data.PulseTimeInFrames, m_highlightCalculationHelper.InteractiveObject.InstanceID);
		}

<<<<<<< HEAD
		[Event(null, 494)]
=======
		[Event(null, 496)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void OnHighlightOnClient(HighlightMsg msg)
		{
			ExclusiveHighlightIdentification exclusiveHighlightIdentification = new ExclusiveHighlightIdentification(msg.Data.EntityId, msg.Data.SubPartNames);
			if (m_static.m_exclusiveKeysToIds.ContainsValue(exclusiveHighlightIdentification) && (!m_static.m_exclusiveKeysToIds.TryGetValue(msg.ExclusiveKey, out var value) || !value.Equals(exclusiveHighlightIdentification)))
			{
				if (m_static.HighlightRejected != null)
				{
					m_static.HighlightRejected(msg.Data);
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRequestRejected, msg, MyEventContext.Current.Sender);
				return;
			}
			m_static.MakeLocalHighlightChange(msg.Data);
			if (msg.IsExclusive)
			{
				bool flag = msg.Data.Thickness > -1;
				if (msg.ExclusiveKey == -1)
				{
					msg.ExclusiveKey = m_exclusiveKeyCounter++;
					if (flag && !m_static.m_exclusiveKeysToIds.ContainsKey(msg.ExclusiveKey))
					{
						m_static.m_exclusiveKeysToIds.Add(msg.ExclusiveKey, exclusiveHighlightIdentification);
					}
				}
				if (!flag)
				{
					m_static.m_exclusiveKeysToIds.Remove(msg.ExclusiveKey);
				}
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRequestAccepted, msg, MyEventContext.Current.Sender);
		}

<<<<<<< HEAD
		[Event(null, 540)]
=======
		[Event(null, 542)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void OnRequestRejected(HighlightMsg msg)
		{
			if (msg.IsExclusive)
			{
				m_static.NotifyExclusiveHighlightRejected(msg.Data, msg.ExclusiveKey);
			}
			else if (m_static.HighlightRejected != null)
			{
				m_static.HighlightRejected(msg.Data);
			}
		}

<<<<<<< HEAD
		[Event(null, 555)]
=======
		[Event(null, 557)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void OnRequestAccepted(HighlightMsg msg)
		{
			if (msg.IsExclusive)
			{
				m_static.NotifyExclusiveHighlightAccepted(msg.Data, msg.ExclusiveKey);
			}
			else
			{
				m_static.NotifyHighlightAccepted(msg.Data);
			}
		}

		private void NotifyHighlightAccepted(MyHighlightData data)
		{
			if (this.HighlightAccepted != null)
			{
				this.HighlightAccepted(data);
				Delegate[] invocationList = this.HighlightAccepted.GetInvocationList();
				foreach (Delegate @delegate in invocationList)
				{
					HighlightAccepted -= (Action<MyHighlightData>)@delegate;
				}
			}
		}

		private void NotifyExclusiveHighlightAccepted(MyHighlightData data, int exclusiveKey)
		{
			if (this.ExclusiveHighlightAccepted != null)
			{
				this.ExclusiveHighlightAccepted(data, exclusiveKey);
				Delegate[] invocationList = this.ExclusiveHighlightAccepted.GetInvocationList();
				foreach (Delegate @delegate in invocationList)
				{
					ExclusiveHighlightAccepted -= (Action<MyHighlightData, int>)@delegate;
				}
			}
		}

		private void NotifyExclusiveHighlightRejected(MyHighlightData data, int exclusiveKey)
		{
			if (this.ExclusiveHighlightRejected != null)
			{
				this.ExclusiveHighlightRejected(data, exclusiveKey);
				Delegate[] invocationList = this.ExclusiveHighlightRejected.GetInvocationList();
				foreach (Delegate @delegate in invocationList)
				{
					ExclusiveHighlightRejected -= (Action<MyHighlightData, int>)@delegate;
				}
			}
		}

		private void OnExclusiveHighlightAccepted(MyHighlightData data, int key)
		{
			if (data.Thickness > 0)
			{
				m_exclusiveHighlightsData[key] = data;
			}
			else
			{
				m_exclusiveHighlightsData.Remove(key);
			}
		}
	}
}
