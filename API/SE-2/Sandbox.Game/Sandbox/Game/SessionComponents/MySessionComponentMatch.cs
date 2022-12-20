using System;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Utils;
using VRage.Network;

namespace Sandbox.Game.SessionComponents
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000, typeof(MyObjectBuilder_SessionComponentMatch), null, false)]
	public class MySessionComponentMatch : MySessionComponentBase
	{
		protected sealed class CreateScoreScreenSync_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateScoreScreenSync();
			}
		}

		protected sealed class CreateScoreScreenClient_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateScoreScreenClient();
			}
		}

		protected sealed class RemoveScoreScreenSync_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveScoreScreenSync();
			}
		}

		protected sealed class TurnOnGlobalDamage_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				TurnOnGlobalDamage();
			}
		}

		protected sealed class SyncRemainingTimeWithClients_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SyncRemainingTimeWithClients();
			}
		}

		protected sealed class RecieveTimeSync_003C_003ESystem_Single_0023System_Single : ICallSite<IMyEventOwner, float, float, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float syncTimeSeconds, in float timeLeftSeconds, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RecieveTimeSync(syncTimeSeconds, timeLeftSeconds);
			}
		}

		private bool m_isRunning;

		private MyTimeSpan m_stateRemainingTime = MyTimeSpan.FromTicks(long.MaxValue);

		private MyTimeSpan m_lastFrameTime;

		private MyMatchState m_state;

		public Action<MyMatchState> OnStateEnded;

		public Action<MyMatchState> OnStateStarted;

		public Action<MyMatchState, MyMatchState> OnStateChanged;

		public bool IsEnabled { get; private set; }

		public bool IsRunning
		{
			get
			{
				return m_isRunning;
			}
			private set
			{
				if (m_isRunning != value)
				{
					m_isRunning = value;
					if (m_isRunning)
					{
						m_lastFrameTime = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
					}
				}
			}
		}

		public MyMatchState State
		{
			get
			{
				return m_state;
			}
			private set
			{
				if (m_state == value)
				{
					return;
				}
				bool interruptStateChange = false;
				if (MyVisualScriptLogicProvider.MatchStateEnding != null)
				{
					MyVisualScriptLogicProvider.MatchStateEnding(m_state.ToString(), ref interruptStateChange);
				}
				if (!interruptStateChange)
				{
					MyMatchState state = m_state;
					if (MyVisualScriptLogicProvider.MatchStateEnded != null)
					{
						MyVisualScriptLogicProvider.MatchStateEnded(state.ToString());
					}
					OnStateEnded.InvokeIfNotNull(state);
					m_state = value;
					if (MyVisualScriptLogicProvider.MatchStateChanged != null)
					{
						MyVisualScriptLogicProvider.MatchStateChanged(state.ToString(), m_state.ToString());
					}
					OnStateChanged.InvokeIfNotNull(state, m_state);
					if (MyVisualScriptLogicProvider.MatchStateStarted != null)
					{
						MyVisualScriptLogicProvider.MatchStateStarted(m_state.ToString());
					}
					OnStateStarted.InvokeIfNotNull(m_state);
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// When changed, will sync time to clients. If you want to change remaining time without sync, use m_stateRemainingTime directly
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyTimeSpan RemainingTime
		{
			get
			{
				return m_stateRemainingTime;
			}
			set
			{
				m_stateRemainingTime = value;
				SyncRemainingTimeWithClients();
			}
		}

		public float RemainingMinutes
		{
			get
			{
				if (double.IsInfinity(m_stateRemainingTime.Minutes))
				{
					return 0f;
				}
				return (float)m_stateRemainingTime.Minutes;
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			IsEnabled = MySession.Static.Settings.EnableMatchComponent;
			SetInitState();
			MyObjectBuilder_SessionComponentMatch myObjectBuilder_SessionComponentMatch = sessionComponent as MyObjectBuilder_SessionComponentMatch;
			if (myObjectBuilder_SessionComponentMatch != null)
			{
				m_state = (MyMatchState)myObjectBuilder_SessionComponentMatch.State;
				RemainingTime = MyTimeSpan.FromMinutes(myObjectBuilder_SessionComponentMatch.RemainingTimeInMinutes);
				IsRunning = myObjectBuilder_SessionComponentMatch.IsRunning;
			}
			if (Sync.IsServer)
			{
				MyVisualScriptLogicProvider.PlayerSpawned = (SingleKeyPlayerEvent)Delegate.Combine(MyVisualScriptLogicProvider.PlayerSpawned, new SingleKeyPlayerEvent(OnPlayerSpawned));
				MyVisualScriptLogicProvider.PlayerConnected = (SingleKeyPlayerEvent)Delegate.Combine(MyVisualScriptLogicProvider.PlayerConnected, new SingleKeyPlayerEvent(OnPlayerConnected));
			}
		}

		private void OnPlayerConnected(long playerId)
		{
			if (State != MyMatchState.Match)
			{
				return;
			}
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(playerId);
			if (myIdentity != null && myIdentity.Character != null && !myIdentity.Character.IsDead)
			{
				ulong value = MySession.Static.Players.TryGetSteamId(playerId);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CreateScoreScreenClient, new EndpointId(value));
			}
		}

		private void OnPlayerSpawned(long playerId)
		{
			if (State == MyMatchState.Match)
			{
				ulong value = MySession.Static.Players.TryGetSteamId(playerId);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CreateScoreScreenClient, new EndpointId(value));
			}
		}

		protected override void UnloadData()
		{
			if (Sync.IsServer)
			{
				MyVisualScriptLogicProvider.PlayerSpawned = (SingleKeyPlayerEvent)Delegate.Remove(MyVisualScriptLogicProvider.PlayerSpawned, new SingleKeyPlayerEvent(OnPlayerSpawned));
				MyVisualScriptLogicProvider.PlayerConnected = (SingleKeyPlayerEvent)Delegate.Remove(MyVisualScriptLogicProvider.PlayerConnected, new SingleKeyPlayerEvent(OnPlayerConnected));
			}
		}

		private void SetInitState()
		{
			m_state = MyMatchState.PreMatch;
			RemainingTime = MyTimeSpan.FromMinutes(MySession.Static.Settings.PreMatchDuration);
		}

		public void ResetToFirstState()
		{
			State = MyMatchState.PreMatch;
			if (State == MyMatchState.PreMatch)
			{
				RemainingTime = MyTimeSpan.FromMinutes(MySession.Static.Settings.PreMatchDuration);
			}
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_SessionComponentMatch obj = base.GetObjectBuilder() as MyObjectBuilder_SessionComponentMatch;
			obj.State = (int)m_state;
			obj.RemainingTimeInMinutes = RemainingMinutes;
			obj.IsRunning = IsRunning;
			return obj;
		}

		public override void UpdateAfterSimulation()
		{
			MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(MySandboxGame.TotalGamePlayTimeInMilliseconds);
			MyTimeSpan myTimeSpan2 = myTimeSpan - m_lastFrameTime;
			m_stateRemainingTime -= myTimeSpan2;
			m_lastFrameTime = myTimeSpan;
			if (Sync.IsServer)
			{
				base.UpdateAfterSimulation();
				if (RemainingTime.Ticks < 0)
				{
					RemainingTime = MyTimeSpan.Zero;
					StateTimedOut();
				}
			}
		}

<<<<<<< HEAD
		[Event(null, 212)]
=======
		[Event(null, 216)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void CreateScoreScreenSync()
		{
			if (!Sync.IsDedicated && !MyScreenManager.ExistsScreenOfType(typeof(MyGuiScreenWarfareFactionScore)))
			{
				MyScreenManager.InsertScreen(new MyGuiScreenWarfareFactionScore(), 1);
			}
		}

<<<<<<< HEAD
		[Event(null, 228)]
=======
		[Event(null, 232)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void CreateScoreScreenClient()
		{
			if (!MyScreenManager.ExistsScreenOfType(typeof(MyGuiScreenWarfareFactionScore)))
			{
				MyScreenManager.InsertScreen(new MyGuiScreenWarfareFactionScore(), 1);
			}
		}

<<<<<<< HEAD
		[Event(null, 239)]
=======
		[Event(null, 243)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void RemoveScoreScreenSync()
		{
			if (!Sync.IsDedicated)
			{
				MyScreenManager.RemoveScreenByType(typeof(MyGuiScreenWarfareFactionScore));
			}
		}

		public void SetIsRunning(bool isRuning)
		{
			IsRunning = isRuning;
		}

		public void SetRemainingTime(float timeInMinutes)
		{
			if (timeInMinutes > 0f)
			{
				RemainingTime = MyTimeSpan.FromMinutes(timeInMinutes);
			}
			else
			{
				AdvanceToNextState();
			}
		}

		public void AddRemainingTime(float timeInMinutes)
		{
			RemainingTime += MyTimeSpan.FromMinutes(timeInMinutes);
			if (RemainingTime < MyTimeSpan.Zero)
			{
				AdvanceToNextState();
			}
		}

		public void AdvanceToNextState()
		{
			if (State != MyMatchState.Finished)
			{
				RemainingTime = MyTimeSpan.Zero;
				StateTimedOut();
			}
		}

		private void StateTimedOut()
		{
			switch (m_state)
			{
			case MyMatchState.PreMatch:
			{
				MyMatchState myMatchState4 = (State = MyMatchState.Match);
				if (State == myMatchState4)
				{
					RemainingTime = MyTimeSpan.FromMinutes(MySession.Static.Settings.MatchDuration);
				}
				if (State == MyMatchState.Match)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => TurnOnGlobalDamage);
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CreateScoreScreenSync);
				}
				break;
			}
			case MyMatchState.Match:
			{
				MyMatchState myMatchState2 = (State = MyMatchState.PostMatch);
				if (State == myMatchState2)
				{
					RemainingTime = MyTimeSpan.FromMinutes(MySession.Static.Settings.PostMatchDuration);
				}
				if (State == MyMatchState.PostMatch)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RemoveScoreScreenSync);
				}
				break;
			}
			case MyMatchState.PostMatch:
				State = MyMatchState.Finished;
				break;
			case MyMatchState.Finished:
				break;
			}
		}

		public static void SetTimeRemainingInternal(float syncTimeSeconds, float timeLeftSeconds)
		{
			MySession.Static.GetComponent<MySessionComponentMatch>().RemainingTime = MyTimeSpan.FromSeconds((double)timeLeftSeconds - MySession.Static.ElapsedGameTime.TotalSeconds + (double)syncTimeSeconds);
		}

<<<<<<< HEAD
		[Event(null, 331)]
=======
		[Event(null, 335)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void TurnOnGlobalDamage()
		{
			MySessionComponentSafeZones.AllowedActions |= MySafeZoneAction.Damage;
		}

<<<<<<< HEAD
		[Event(null, 337)]
=======
		[Event(null, 341)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void SyncRemainingTimeWithClients()
		{
			if (MySession.Static.IsServer)
			{
				MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RecieveTimeSync, (float)MySession.Static.ElapsedGameTime.TotalSeconds, (float)component.RemainingTime.Seconds);
			}
		}

<<<<<<< HEAD
		[Event(null, 347)]
=======
		[Event(null, 351)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void RecieveTimeSync(float syncTimeSeconds, float timeLeftSeconds)
		{
			SetTimeRemainingInternal(syncTimeSeconds, timeLeftSeconds);
		}
	}
}
