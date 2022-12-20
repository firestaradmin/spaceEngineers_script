<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Sandbox.Game.World;
using VRage.Game.Components;

namespace Sandbox.Game.Entities.Cube
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	internal class MyOreDetectorSessionComponent : MySessionComponentBase
	{
		private HashSet<MyDepositQuery> m_queries = new HashSet<MyDepositQuery>();

		public static MyOreDetectorSessionComponent Static { get; private set; }

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override void LoadData()
		{
			base.LoadData();
			Static = this;
			MySession.OnUnloading += OnUnloading;
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected override void UnloadData()
		{
			base.UnloadData();
			MySession.OnUnloading -= OnUnloading;
			Static = null;
		}

		private void OnUnloading()
		{
<<<<<<< HEAD
			foreach (MyDepositQuery query in m_queries)
			{
				query.Cancel();
=======
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyDepositQuery> enumerator = m_queries.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Cancel();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void Track(MyDepositQuery query)
		{
			m_queries.Add(query);
		}

		public void Untrack(MyDepositQuery query)
		{
			m_queries.Remove(query);
		}
	}
}
