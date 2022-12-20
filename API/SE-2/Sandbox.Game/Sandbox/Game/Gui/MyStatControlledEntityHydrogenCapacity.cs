<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityHydrogenCapacity : MyStatBase
	{
		private float m_maxValue;

		private List<IMyGasTank> m_tankBlocks = new List<IMyGasTank>();

		private MyCubeGrid m_currentGrid;

		private MyCubeGrid CurrentGrid
		{
			get
			{
				return m_currentGrid;
			}
			set
			{
<<<<<<< HEAD
=======
				//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (value == m_currentGrid)
				{
					return;
				}
				if (m_currentGrid != null)
				{
					MyGridConveyorSystem conveyorSystem = m_currentGrid.GridSystems.ConveyorSystem;
					conveyorSystem.BlockRemoved -= ConveyorSystemOnBlockRemoved;
					conveyorSystem.BlockAdded -= ConveyorSystemOnBlockAdded;
					m_currentGrid.OnMarkForClose -= OnGridClosed;
<<<<<<< HEAD
				}
				m_tankBlocks.Clear();
				m_currentGrid = value;
				if (value == null)
				{
					return;
				}
				MyGridConveyorSystem conveyorSystem2 = value.GridSystems.ConveyorSystem;
				conveyorSystem2.BlockAdded += ConveyorSystemOnBlockAdded;
				conveyorSystem2.BlockRemoved += ConveyorSystemOnBlockRemoved;
				value.OnMarkForClose += OnGridClosed;
				foreach (IMyConveyorEndpointBlock conveyorEndpointBlock in conveyorSystem2.ConveyorEndpointBlocks)
				{
					if (IsHydrogenTank(conveyorEndpointBlock as MyCubeBlock, out var tank))
					{
						m_tankBlocks.Add(tank);
					}
				}
			}
		}

		public override float MaxValue => m_maxValue;

		public MyStatControlledEntityHydrogenCapacity()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_hydrogen_capacity");
		}

		public override void Update()
		{
			MyEntity myEntity = MySession.Static.ControlledEntity?.Entity;
			if (myEntity != null)
			{
				MyCubeGrid currentGrid = null;
				MyEntity myEntity2 = myEntity;
				if (myEntity2 != null && (myEntity2 is MyCockpit || myEntity2 is MyRemoteControl || myEntity2 is MyLargeTurretBase))
				{
					currentGrid = ((MyCubeBlock)myEntity).CubeGrid;
				}
=======
				}
				m_tankBlocks.Clear();
				m_currentGrid = value;
				if (value == null)
				{
					return;
				}
				MyGridConveyorSystem conveyorSystem2 = value.GridSystems.ConveyorSystem;
				conveyorSystem2.BlockAdded += ConveyorSystemOnBlockAdded;
				conveyorSystem2.BlockRemoved += ConveyorSystemOnBlockRemoved;
				value.OnMarkForClose += OnGridClosed;
				Enumerator<IMyConveyorEndpointBlock> enumerator = conveyorSystem2.ConveyorEndpointBlocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						if (IsHydrogenTank(enumerator.get_Current() as MyCubeBlock, out var tank))
						{
							m_tankBlocks.Add(tank);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
		}

		public override float MaxValue => m_maxValue;

		public MyStatControlledEntityHydrogenCapacity()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_hydrogen_capacity");
		}

		public override void Update()
		{
			MyEntity myEntity = MySession.Static.ControlledEntity?.Entity;
			if (myEntity != null)
			{
				MyCubeGrid currentGrid = null;
				MyEntity myEntity2 = myEntity;
				if (myEntity2 != null && (myEntity2 is MyCockpit || myEntity2 is MyRemoteControl || myEntity2 is MyLargeTurretBase))
				{
					currentGrid = ((MyCubeBlock)myEntity).CubeGrid;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				CurrentGrid = currentGrid;
				float num = 0f;
				float num2 = 0f;
				foreach (IMyGasTank tankBlock in m_tankBlocks)
				{
					double filledRatio = tankBlock.FilledRatio;
					float gasCapacity = tankBlock.GasCapacity;
					num += gasCapacity;
					num2 += (float)(filledRatio * (double)gasCapacity);
				}
				m_maxValue = num;
				base.CurrentValue = num2;
			}
			else
			{
				m_maxValue = 0f;
				base.CurrentValue = 0f;
				CurrentGrid = null;
			}
		}

		private static bool IsHydrogenTank(MyCubeBlock block, out IMyGasTank tank)
		{
			tank = block as IMyGasTank;
			return tank?.IsResourceStorage(MyResourceDistributorComponent.HydrogenId) ?? false;
		}

		private void ConveyorSystemOnBlockRemoved(MyCubeBlock myCubeBlock)
		{
			if (IsHydrogenTank(myCubeBlock, out var tank))
			{
				m_tankBlocks.Remove(tank);
			}
		}

		private void ConveyorSystemOnBlockAdded(MyCubeBlock myCubeBlock)
		{
			if (IsHydrogenTank(myCubeBlock, out var tank))
			{
				m_tankBlocks.Add(tank);
			}
		}

		private void OnGridClosed(MyEntity grid)
		{
			CurrentGrid = null;
		}

		public override string ToString()
		{
			float num = ((m_maxValue > 0f) ? (base.CurrentValue / m_maxValue) : 0f);
			return $"{num * 100f:0}";
		}
	}
}
