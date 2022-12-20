using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI;

namespace Sandbox.Game.GameSystems
{
	/// <summary>
	/// This session component has all damage routed through it.  This allows damage tracking, nullification, mitigation and amplification. 
	/// </summary>
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyDamageSystem : MySessionComponentBase, IMyDamageSystem
	{
		private List<Tuple<int, Action<object, MyDamageInformation>>> m_destroyHandlers = new List<Tuple<int, Action<object, MyDamageInformation>>>();

		private List<Tuple<int, BeforeDamageApplied>> m_beforeDamageHandlers = new List<Tuple<int, BeforeDamageApplied>>();

		private List<Tuple<int, Action<object, MyDamageInformation>>> m_afterDamageHandlers = new List<Tuple<int, Action<object, MyDamageInformation>>>();

		public static MyDamageSystem Static { get; private set; }

		public bool HasAnyBeforeHandler => m_beforeDamageHandlers.Count > 0;

		public override void LoadData()
		{
			Static = this;
			base.LoadData();
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_destroyHandlers.Clear();
			m_beforeDamageHandlers.Clear();
			m_afterDamageHandlers.Clear();
			Static = null;
		}

		/// <summary>
		/// Raised when an object is destroyed.
		/// </summary>
		/// <param name="target">The target object</param>
		/// <param name="info">Information about the damage</param>
		public void RaiseDestroyed(object target, MyDamageInformation info)
		{
			foreach (Tuple<int, Action<object, MyDamageInformation>> destroyHandler in m_destroyHandlers)
			{
				destroyHandler.Item2(target, info);
			}
		}

		/// <summary>
		/// Raised before damage is applied.  Can be modified.
		/// </summary>
		/// <param name="target">The target object</param>
		/// <param name="info">Information about the damage.  Can be modified</param>
		public void RaiseBeforeDamageApplied(object target, ref MyDamageInformation info)
		{
			if (m_beforeDamageHandlers.Count > 0)
			{
				RaiseBeforeDamageAppliedIntenal(target, ref info);
			}
		}

		private void RaiseBeforeDamageAppliedIntenal(object target, ref MyDamageInformation info)
		{
			foreach (Tuple<int, BeforeDamageApplied> beforeDamageHandler in m_beforeDamageHandlers)
			{
				beforeDamageHandler.Item2(target, ref info);
			}
		}

		/// <summary>
		/// Raised after damage is applied
		/// </summary>
		/// <param name="target">The target object</param>
		/// <param name="info">Information about the damage</param>
		public void RaiseAfterDamageApplied(object target, MyDamageInformation info)
		{
			foreach (Tuple<int, Action<object, MyDamageInformation>> afterDamageHandler in m_afterDamageHandlers)
			{
				afterDamageHandler.Item2(target, info);
			}
		}

		/// <summary>
		/// Registers a handler for when an object in game is destroyed.
		/// </summary>
		/// <param name="priority">Priority level.  Lower means higher priority.</param>
		/// <param name="handler">Actual handler delegate</param>
		public void RegisterDestroyHandler(int priority, Action<object, MyDamageInformation> handler)
		{
			Tuple<int, Action<object, MyDamageInformation>> item = new Tuple<int, Action<object, MyDamageInformation>>(priority, handler);
			m_destroyHandlers.Add(item);
			m_destroyHandlers.Sort((Tuple<int, Action<object, MyDamageInformation>> x, Tuple<int, Action<object, MyDamageInformation>> y) => x.Item1 - y.Item1);
		}

		/// <summary>
		/// Registers a handler that is called before an object in game is damaged.  The damage can be modified in this handler.
		/// </summary>
		/// <param name="priority">Priority level.  Lower means higher priority.</param>
		/// <param name="handler">Actual handler delegate</param>
		public void RegisterBeforeDamageHandler(int priority, BeforeDamageApplied handler)
		{
			Tuple<int, BeforeDamageApplied> item = new Tuple<int, BeforeDamageApplied>(priority, handler);
			m_beforeDamageHandlers.Add(item);
			m_beforeDamageHandlers.Sort((Tuple<int, BeforeDamageApplied> x, Tuple<int, BeforeDamageApplied> y) => x.Item1 - y.Item1);
		}

		/// <summary>
		/// Registers a handler that is called after an object in game is damaged.
		/// </summary>
		/// <param name="priority">Priority level.  Lower means higher priority.</param>
		/// <param name="handler">Actual handler delegate</param>
		public void RegisterAfterDamageHandler(int priority, Action<object, MyDamageInformation> handler)
		{
			Tuple<int, Action<object, MyDamageInformation>> item = new Tuple<int, Action<object, MyDamageInformation>>(priority, handler);
			m_afterDamageHandlers.Add(item);
			m_afterDamageHandlers.Sort((Tuple<int, Action<object, MyDamageInformation>> x, Tuple<int, Action<object, MyDamageInformation>> y) => x.Item1 - y.Item1);
		}
	}
}
