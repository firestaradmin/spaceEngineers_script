using VRage.Game.Components;
using VRage.ModAPI;

namespace Sandbox.Game.Entities.Character.Components
{
	public abstract class MyCharacterComponent : MyEntityComponentBase
	{
		private bool m_needsUpdateAfterSimulation;

		private bool m_needsUpdateAfterSimulationParallel;

		private bool m_needsUpdateSimulation;

		private bool m_needsUpdateAfterSimulation10;

		private bool m_needsUpdateBeforeSimulation100;

		private bool m_needsUpdateBeforeSimulation;

		private bool m_needsUpdateBeforeSimulationParallel;

<<<<<<< HEAD
		/// <summary>
		/// This set's flag for update. Set it after add to container!
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool NeedsUpdateAfterSimulation
		{
			get
			{
				return m_needsUpdateAfterSimulation;
			}
			set
			{
				m_needsUpdateAfterSimulation = value;
				base.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// This set's flag for update. Set it after add to container!
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool NeedsUpdateAfterSimulationParallel
		{
			get
			{
				return m_needsUpdateAfterSimulationParallel;
			}
			set
			{
				m_needsUpdateAfterSimulationParallel = value;
				base.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// This set's flag for update. Set it after add to container!
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool NeedsUpdateSimulation
		{
			get
			{
				return m_needsUpdateSimulation;
			}
			set
			{
				m_needsUpdateSimulation = value;
				base.Entity.NeedsUpdate |= MyEntityUpdateEnum.SIMULATE;
			}
		}

		/// <summary>
		/// This set's flag for update. Set it after add to container!
		/// </summary>
		public bool NeedsUpdateAfterSimulation10
		{
			get
			{
				return m_needsUpdateAfterSimulation10;
			}
			set
			{
				m_needsUpdateAfterSimulation10 = value;
				base.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
			}
		}

		/// <summary>
		/// This set's flag for update. Set it after add to container!
		/// </summary>
		public bool NeedsUpdateBeforeSimulation100
		{
			get
			{
				return m_needsUpdateBeforeSimulation100;
			}
			set
			{
				m_needsUpdateBeforeSimulation100 = value;
				base.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
		}

		/// <summary>
		/// This set's flag for update. Set it after add to container!
		/// </summary>
		public bool NeedsUpdateBeforeSimulation
		{
			get
			{
				return m_needsUpdateBeforeSimulation;
			}
			set
			{
				m_needsUpdateBeforeSimulation = value;
				base.Entity.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// This set's flag for update. Set it after add to container!
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool NeedsUpdateBeforeSimulationParallel
		{
			get
			{
				return m_needsUpdateBeforeSimulationParallel;
			}
			set
			{
				m_needsUpdateBeforeSimulationParallel = value;
				base.Entity.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public MyCharacter Character => (MyCharacter)base.Entity;

		public override string ComponentTypeDebugString => "Character Component";

		public virtual void UpdateAfterSimulation10()
		{
		}

		public virtual void UpdateBeforeSimulation()
		{
		}

		public virtual void UpdateBeforeSimulationParallel()
		{
		}

		public virtual void Simulate()
		{
		}

		public virtual void UpdateAfterSimulation()
		{
		}

		public virtual void UpdateAfterSimulationParallel()
		{
		}

		public virtual void UpdateBeforeSimulation100()
		{
		}

		public virtual void OnCharacterDead()
		{
		}
	}
}
