using System.Text;
<<<<<<< HEAD
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
=======
using Sandbox.Game.Gui;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityEnergyEstimatedTimeRemaining : MyStatBase
	{
		private readonly StringBuilder m_stringBuilder = new StringBuilder();

		public MyStatControlledEntityEnergyEstimatedTimeRemaining()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_estimated_time_remaining_energy");
		}

		public override void Update()
		{
			MyCubeGrid controlledGrid = MySession.Static.ControlledGrid;
			if (controlledGrid != null)
			{
				base.CurrentValue = controlledGrid.GridSystems.ResourceDistributor.RemainingFuelTimeByType(MyResourceDistributorComponent.ElectricityId, controlledGrid);
			}
			else
			{
				base.CurrentValue = 0f;
			}
		}

		public override string ToString()
		{
			m_stringBuilder.Clear();
			MyValueFormatter.AppendTimeInBestUnit(base.CurrentValue * 3600f, m_stringBuilder);
			return m_stringBuilder.ToString();
		}
	}
}
