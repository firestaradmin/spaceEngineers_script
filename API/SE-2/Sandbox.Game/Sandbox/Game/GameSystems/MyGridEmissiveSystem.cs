<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.GameSystems
{
	public class MyGridEmissiveSystem
	{
		private HashSet<MyEmissiveBlock> m_emissiveBlocks;

		private bool m_isPowered;

		private const float SYSTEM_CONSUMPTION = 1E-06f;

		private float m_emissivity
		{
			get
			{
				if (!m_isPowered)
				{
					return 0f;
				}
				return 1f;
			}
		}

		public MyResourceSinkComponent ResourceSink { get; private set; }

<<<<<<< HEAD
		/// <summary>
		/// This should not be used to modify the emissiveBlocks.
		/// Use Register/Unregister for that.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public HashSet<MyEmissiveBlock> EmissiveBlocks => m_emissiveBlocks;

		public MyGridEmissiveSystem(MyCubeGrid grid)
		{
			m_emissiveBlocks = new HashSet<MyEmissiveBlock>();
			ResourceSink = new MyResourceSinkComponent();
<<<<<<< HEAD
			ResourceSink.Init(MyStringHash.NullOrEmpty, 1E-06f, () => 1E-06f, null);
			ResourceSink.Grid = grid;
=======
			ResourceSink.Init(MyStringHash.NullOrEmpty, 1E-06f, () => 1E-06f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			ResourceSink.Update();
		}

		public void Register(MyEmissiveBlock emissiveBlock)
		{
			m_emissiveBlocks.Add(emissiveBlock);
			emissiveBlock.OnModelChanged += EmissiveBlock_OnModelChanged;
			emissiveBlock.SetEmissivity(m_emissivity);
		}

		public void Unregister(MyEmissiveBlock emissiveBlock)
		{
			emissiveBlock.OnModelChanged -= EmissiveBlock_OnModelChanged;
			m_emissiveBlocks.Remove(emissiveBlock);
		}

		public void UpdateEmissivity()
		{
<<<<<<< HEAD
			foreach (MyEmissiveBlock emissiveBlock in m_emissiveBlocks)
			{
				emissiveBlock.SetEmissivity(m_emissivity);
=======
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyEmissiveBlock> enumerator = m_emissiveBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().SetEmissivity(m_emissivity);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void Receiver_IsPoweredChanged()
		{
			m_isPowered = ResourceSink.IsPowerAvailable(MyResourceDistributorComponent.ElectricityId, 1E-06f);
			UpdateEmissivity();
		}

		private void EmissiveBlock_OnModelChanged(MyEmissiveBlock emissiveBlock)
		{
			emissiveBlock.SetEmissivity(m_emissivity);
		}

		public void UpdateBeforeSimulation100()
		{
			MySimpleProfiler.Begin("EmissiveBlocks", MySimpleProfiler.ProfilingBlockType.BLOCK, "UpdateBeforeSimulation100");
			ResourceSink.Update();
			MySimpleProfiler.End("UpdateBeforeSimulation100");
		}
	}
}
