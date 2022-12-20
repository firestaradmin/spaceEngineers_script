using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Lights;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Generics;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MyExplosions : MySessionComponentBase
	{
		protected sealed class ProxyExplosionRequest_003C_003ESandbox_Game_MyExplosionInfoSimplified : ICallSite<IMyEventOwner, MyExplosionInfoSimplified, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyExplosionInfoSimplified explosionInfo, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ProxyExplosionRequest(explosionInfo);
			}
		}

		private static MyObjectsPool<MyExplosion> m_explosions;

		private static List<MyExplosionInfo> m_explosionBuffer1;

		private static List<MyExplosionInfo> m_explosionBuffer2;

		private static List<MyExplosionInfo> m_explosionsRead;

		private static List<MyExplosionInfo> m_explosionsWrite;

		private static List<MyExplosion> m_exploded;

		private static HashSet<long> m_activeEntityKickbacks;

		private static SortedDictionary<long, long> m_activeEntityKickbacksByTime;

		public override Type[] Dependencies => new Type[1] { typeof(MyLights) };
<<<<<<< HEAD

		public static event OnExplosionDel OnExplosion;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		static MyExplosions()
		{
			m_explosions = null;
			m_explosionBuffer1 = new List<MyExplosionInfo>();
			m_explosionBuffer2 = new List<MyExplosionInfo>();
			m_explosionsRead = m_explosionBuffer1;
			m_explosionsWrite = m_explosionBuffer2;
			m_exploded = new List<MyExplosion>();
			m_activeEntityKickbacks = new HashSet<long>();
			m_activeEntityKickbacksByTime = new SortedDictionary<long, long>();
		}

		public override void LoadData()
		{
			MySandboxGame.Log.WriteLine("MyExplosions.LoadData() - START");
			MySandboxGame.Log.IncreaseIndent();
			if (m_explosions == null)
			{
				m_explosions = new MyObjectsPool<MyExplosion>(1024, null, delegate(MyExplosion x)
				{
					x.Clear();
				});
			}
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("MyExplosions.LoadData() - END");
		}

		protected override void UnloadData()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (m_explosions != null && m_explosions.ActiveCount > 0)
			{
				Enumerator<MyExplosion> enumerator = m_explosions.Active.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						enumerator.get_Current()?.Close();
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				m_explosions.DeallocateAll();
				m_explosions.TrimInternalCollections();
			}
			m_explosionsRead.Clear();
			m_explosionsWrite.Clear();
			m_activeEntityKickbacks.Clear();
			m_activeEntityKickbacksByTime.Clear();
		}

		public static void AddExplosion(ref MyExplosionInfo explosionInfo, bool updateSync = true)
		{
			if (!MySessionComponentSafeZones.IsActionAllowed(BoundingBoxD.CreateFromSphere(explosionInfo.ExplosionSphere), MySafeZoneAction.Damage, 0L, 0uL))
			{
				return;
			}
			if (Sync.IsServer && updateSync)
			{
				MyExplosionInfoSimplified arg = default(MyExplosionInfoSimplified);
				arg.Damage = explosionInfo.Damage;
				arg.Center = explosionInfo.ExplosionSphere.Center;
				arg.Radius = (float)explosionInfo.ExplosionSphere.Radius;
				arg.Type = explosionInfo.ExplosionType;
				arg.Flags = explosionInfo.ExplosionFlags;
				arg.VoxelCenter = explosionInfo.VoxelExplosionCenter;
				arg.ParticleScale = explosionInfo.ParticleScale;
				arg.Velocity = explosionInfo.Velocity;
				arg.IgnoreFriendlyFireSetting = explosionInfo.IgnoreFriendlyFireSetting;
<<<<<<< HEAD
				arg.CustomEffect = explosionInfo.CustomEffect;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ProxyExplosionRequest, arg, default(EndpointId), explosionInfo.ExplosionSphere.Center);
			}
			m_explosionsWrite.Add(explosionInfo);
		}

		public override void UpdateBeforeSimulation()
		{
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			SwapBuffers();
			UpdateEntityKickbacks();
			for (int i = 0; i < m_explosionsRead.Count; i++)
			{
				MyExplosionInfo explosionInfo = m_explosionsRead[i];
				MyExplosion item = null;
				m_explosions.AllocateOrCreate(out item);
				if (item != null)
				{
					MyExplosions.OnExplosion?.Invoke(ref explosionInfo);
					item.Start(explosionInfo);
				}
			}
			m_explosionsRead.Clear();
<<<<<<< HEAD
			foreach (MyExplosion item2 in m_explosions.Active)
			{
				if (!item2.Update())
				{
					m_exploded.Add(item2);
					m_explosions.MarkForDeallocate(item2);
				}
				else
				{
					m_exploded.Add(item2);
				}
			}
=======
			Enumerator<MyExplosion> enumerator2 = m_explosions.Active.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					MyExplosion current2 = enumerator2.get_Current();
					if (!current2.Update())
					{
						m_exploded.Add(current2);
						m_explosions.MarkForDeallocate(current2);
					}
					else
					{
						m_exploded.Add(current2);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MyExplosion item3 in m_exploded)
			{
				item3.ApplyVolumetricDamageToGrid();
			}
			m_exploded.Clear();
			m_explosions.DeallocateAllMarked();
			MyDebris.Static.UpdateBeforeSimulation();
		}

<<<<<<< HEAD
		[Event(null, 178)]
=======
		[Event(null, 170)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[ServerInvoked]
		[BroadcastExcept]
		private static void ProxyExplosionRequest(MyExplosionInfoSimplified explosionInfo)
		{
			if (MySession.Static.Ready && !MyEventContext.Current.IsLocallyInvoked)
			{
				MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
				myExplosionInfo.PlayerDamage = 0f;
				myExplosionInfo.Damage = explosionInfo.Damage;
				myExplosionInfo.ExplosionType = explosionInfo.Type;
				myExplosionInfo.ExplosionSphere = new BoundingSphereD(explosionInfo.Center, explosionInfo.Radius);
				myExplosionInfo.LifespanMiliseconds = 700;
				myExplosionInfo.HitEntity = null;
				myExplosionInfo.ParticleScale = explosionInfo.ParticleScale;
				myExplosionInfo.OwnerEntity = null;
				myExplosionInfo.Direction = Vector3.Forward;
				myExplosionInfo.VoxelExplosionCenter = explosionInfo.VoxelCenter;
				myExplosionInfo.ExplosionFlags = explosionInfo.Flags;
				myExplosionInfo.VoxelCutoutScale = 1f;
				myExplosionInfo.PlaySound = true;
				myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
				myExplosionInfo.Velocity = explosionInfo.Velocity;
				myExplosionInfo.IgnoreFriendlyFireSetting = explosionInfo.IgnoreFriendlyFireSetting;
<<<<<<< HEAD
				myExplosionInfo.CustomEffect = explosionInfo.CustomEffect;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyExplosionInfo explosionInfo2 = myExplosionInfo;
				AddExplosion(ref explosionInfo2, updateSync: false);
			}
		}

		private void SwapBuffers()
		{
			if (m_explosionBuffer1 == m_explosionsRead)
			{
				m_explosionsWrite = m_explosionBuffer1;
				m_explosionsRead = m_explosionBuffer2;
			}
			else
			{
				m_explosionsWrite = m_explosionBuffer2;
				m_explosionsRead = m_explosionBuffer1;
			}
		}

		public override void Draw()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyExplosion> enumerator = m_explosions.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().DebugDraw();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public static bool ShouldUseMassScaleForEntity(MyEntity entity)
		{
			long entityId = entity.EntityId;
			if (m_activeEntityKickbacks.Contains(entityId))
			{
				return false;
			}
			long num;
			for (num = (MySession.Static.ElapsedGameTime + TimeSpan.FromSeconds(2.0)).Ticks + entityId % 100; m_activeEntityKickbacksByTime.ContainsKey(num); num++)
			{
			}
			m_activeEntityKickbacks.Add(entityId);
			m_activeEntityKickbacksByTime.Add(num, entityId);
			return true;
		}

		private void UpdateEntityKickbacks()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			long ticks = MySession.Static.ElapsedGameTime.Ticks;
			while (m_activeEntityKickbacksByTime.get_Count() != 0)
			{
				Enumerator<long, long> enumerator = m_activeEntityKickbacksByTime.GetEnumerator();
				try
				{
					if (enumerator.MoveNext())
					{
						KeyValuePair<long, long> current = enumerator.get_Current();
						if (current.Key > ticks)
						{
							return;
						}
						long value = current.Value;
						m_activeEntityKickbacks.Remove(value);
						m_activeEntityKickbacksByTime.Remove(current.Key);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
		}
	}
}
