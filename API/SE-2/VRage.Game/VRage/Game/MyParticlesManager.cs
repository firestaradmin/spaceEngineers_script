using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Generics;
using VRage.Library.Utils;
using VRage.Render.Particles;
using VRageMath;

namespace VRage.Game
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyParticlesManager : MySessionComponentBase, IParticleManager
	{
<<<<<<< HEAD
=======
		private static bool m_sessionLoaded;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static bool Enabled;

		private static bool m_paused;

		private static readonly MyDynamicObjectPool<MyParticleEffect> EffectsPool;

		private static readonly ConcurrentDictionary<uint, MyParticleEffect> m_particleEffectsAll;

		private static readonly ConcurrentCachingHashSet<MyParticleEffect> m_particleEffectsUpdate;

		[Obsolete("Use MyGravityProviderSystem or IMyPhysics instead")]
		public static Func<Vector3D, Vector3> CalculateGravityInPoint;

<<<<<<< HEAD
		public static int InstanceCount => m_particleEffectsAll.Count;
=======
		public static int InstanceCount => m_particleEffectsAll.get_Count();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public static bool Paused
		{
			get
			{
				return m_paused;
			}
			set
			{
				m_paused = value;
			}
		}

		public static MyTimeSpan CurrentTime { get; private set; }

<<<<<<< HEAD
		public static IEnumerable<MyParticleEffect> Effects => m_particleEffectsAll.Values;
=======
		public static IEnumerable<MyParticleEffect> Effects => m_particleEffectsAll.get_Values();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		static MyParticlesManager()
		{
			m_paused = false;
			EffectsPool = new MyDynamicObjectPool<MyParticleEffect>(128, delegate(MyParticleEffect x)
			{
				x.Clear();
			});
			m_particleEffectsAll = new ConcurrentDictionary<uint, MyParticleEffect>();
			m_particleEffectsUpdate = new ConcurrentCachingHashSet<MyParticleEffect>();
			Enabled = true;
		}

		public MyParticlesManager()
		{
			MyParticleEffectsLibrary.Init(this);
			base.UpdateOnPause = true;
		}

		[Obsolete("Use TryCreateParticleEffect with parenting instead")]
		public static bool TryCreateParticleEffect(string effectName, out MyParticleEffect effect)
		{
			return TryCreateParticleEffect(effectName, ref MatrixD.Identity, ref Vector3D.Zero, uint.MaxValue, out effect);
		}

		[Obsolete("Use TryCreateParticleEffect with parenting instead")]
		public static bool TryCreateParticleEffect(string effectName, MatrixD worldMatrix, out MyParticleEffect effect)
		{
			Vector3D worldPosition = worldMatrix.Translation;
			return TryCreateParticleEffect(effectName, ref worldMatrix, ref worldPosition, uint.MaxValue, out effect);
		}

		public static bool TryCreateParticleEffect(string effectName, ref MatrixD effectMatrix, ref Vector3D worldPosition, uint parentID, out MyParticleEffect effect)
		{
			if (string.IsNullOrEmpty(effectName) || !Enabled || !MyParticleEffectsLibrary.Exists(effectName))
			{
				effect = null;
				return false;
			}
			effect = CreateParticleEffect(effectName, ref effectMatrix, ref worldPosition, parentID);
			return effect != null;
		}

		[Obsolete("Use TryCreateParticleEffect with parenting instead")]
		public static bool TryCreateParticleEffect(int id, out MyParticleEffect effect, bool userDraw = false)
		{
			return TryCreateParticleEffect(id, out effect, ref MatrixD.Identity, ref Vector3D.Zero, uint.MaxValue, userDraw);
		}

		public static bool TryCreateParticleEffect(int id, out MyParticleEffect effect, ref MatrixD effectMatrix, ref Vector3D worldPosition, uint parentID, bool userDraw = false)
		{
			effect = null;
			if (MyParticleEffectsLibrary.GetName(id, out var name))
			{
				effect = CreateParticleEffect(name, ref effectMatrix, ref worldPosition, parentID, userDraw);
			}
			return effect != null;
		}

		private static MyParticleEffect CreateParticleEffect(string name, ref MatrixD effectMatrix, ref Vector3D worldPosition, uint parentID, bool userDraw = false)
		{
			MyParticleEffectData myParticleEffectData = MyParticleEffectsLibrary.Get(name);
<<<<<<< HEAD
			if (myParticleEffectData != null && myParticleEffectData.Enabled)
=======
			if (myParticleEffectData != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyParticleEffect myParticleEffect = CreateInstance(myParticleEffectData, ref effectMatrix, ref worldPosition, parentID);
				if (myParticleEffect != null)
				{
					m_particleEffectsAll.TryAdd(myParticleEffect.Id, myParticleEffect);
					return myParticleEffect;
				}
			}
			return null;
		}

		private static MyParticleEffect CreateInstance(MyParticleEffectData effectData, ref MatrixD effectMatrix, ref Vector3D worldPosition, uint parentID)
		{
			MyParticleEffect myParticleEffect = null;
			if (effectData.DistanceMax > 0f)
			{
				if (!effectData.Loop)
				{
					Vector3D value = MyTransparentGeometry.Camera.Translation;
					Vector3D.DistanceSquared(ref worldPosition, ref value, out var result);
					if (result <= (double)(effectData.DistanceMax * effectData.DistanceMax))
					{
						myParticleEffect = EffectsPool.Allocate();
					}
				}
				else
				{
					myParticleEffect = EffectsPool.Allocate();
				}
			}
			else
<<<<<<< HEAD
			{
				myParticleEffect = EffectsPool.Allocate();
			}
			myParticleEffect?.Init(effectData, ref effectMatrix, parentID);
			return myParticleEffect;
=======
			{
				myParticleEffect = EffectsPool.Allocate();
			}
			myParticleEffect?.Init(effectData, ref effectMatrix, parentID);
			return myParticleEffect;
		}

		public static void RemoveParticleEffect(MyParticleEffect effect)
		{
			if (effect != null)
			{
				if (effect.Autodelete)
				{
					effect.Stop(instant: false);
				}
				else
				{
					effect.Close();
				}
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static void RemoveParticleEffect(MyParticleEffect effect)
		{
<<<<<<< HEAD
			if (effect != null)
			{
				if (effect.Autodelete)
				{
					effect.Stop(instant: false);
				}
				else
				{
					effect.Close();
				}
			}
		}

		public override void LoadData()
		{
		}

		protected override void UnloadData()
		{
			List<MyParticleEffect> list = new List<MyParticleEffect>();
			list.AddRange(m_particleEffectsAll.Values);
			m_particleEffectsAll.Clear();
			foreach (MyParticleEffect item in list)
			{
				item.AssertUnload();
				item.Stop();
				item.Update();
				EffectsPool.Deallocate(item);
			}
			m_particleEffectsUpdate.Clear();
		}

		public static void ScheduleUpdate(MyParticleEffect effect)
		{
			m_particleEffectsUpdate.Add(effect);
		}

		public override void UpdateAfterSimulation()
		{
			if (!Enabled)
			{
				return;
			}
			CurrentTime = new MyTimeSpan(Stopwatch.GetTimestamp());
			m_particleEffectsUpdate.ApplyChanges();
			foreach (MyParticleEffect item in m_particleEffectsUpdate)
			{
				item.Update();
			}
			m_particleEffectsUpdate.Clear(clearCache: false);
		}

		public static void OnRemoved(uint id)
		{
			if (m_particleEffectsAll.TryGetValue(id, out var value))
			{
				value.OnRemoved();
				m_particleEffectsAll.Remove(id);
				EffectsPool.Deallocate(value);
			}
		}

		public void RecreateParticleEffects(MyParticleEffectData data)
		{
		}

=======
			m_sessionLoaded = true;
		}

		protected override void UnloadData()
		{
			List<MyParticleEffect> list = new List<MyParticleEffect>();
			list.AddRange(m_particleEffectsAll.get_Values());
			m_particleEffectsAll.Clear();
			foreach (MyParticleEffect item in list)
			{
				item.AssertUnload();
				item.Stop();
				item.Update();
				EffectsPool.Deallocate(item);
			}
			m_particleEffectsUpdate.Clear();
			m_sessionLoaded = false;
		}

		public static void ScheduleUpdate(MyParticleEffect effect)
		{
			m_particleEffectsUpdate.Add(effect);
		}

		public override void UpdateAfterSimulation()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (!Enabled)
			{
				return;
			}
			CurrentTime = new MyTimeSpan(Stopwatch.GetTimestamp());
			m_particleEffectsUpdate.ApplyChanges();
			Enumerator<MyParticleEffect> enumerator = m_particleEffectsUpdate.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Update();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_particleEffectsUpdate.Clear(clearCache: false);
		}

		public static void OnRemoved(uint id)
		{
			MyParticleEffect myParticleEffect = default(MyParticleEffect);
			if (m_particleEffectsAll.TryGetValue(id, ref myParticleEffect))
			{
				myParticleEffect.OnRemoved();
				m_particleEffectsAll.Remove<uint, MyParticleEffect>(id);
				EffectsPool.Deallocate(myParticleEffect);
			}
		}

		public void RecreateParticleEffects(MyParticleEffectData data)
		{
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void RemoveParticleEffects(MyParticleEffectData data)
		{
			foreach (KeyValuePair<uint, MyParticleEffect> item in m_particleEffectsAll)
			{
				if (item.Value.Data == data)
				{
					item.Value.Stop();
					item.Value.Update();
					m_particleEffectsUpdate.Remove(item.Value);
<<<<<<< HEAD
					m_particleEffectsAll.Remove(item.Key);
=======
					m_particleEffectsAll.Remove<uint, MyParticleEffect>(item.Key);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}
	}
}
