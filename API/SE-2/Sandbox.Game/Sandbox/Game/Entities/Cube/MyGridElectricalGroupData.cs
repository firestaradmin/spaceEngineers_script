using VRage.Game.ModAPI;

namespace Sandbox.Game.Entities.Cube
{
	public class MyGridElectricalGroupData : MyGridGroupData<MyGridElectricalGroupData>
	{
		public MyGridElectricalGroupData()
		{
			base.LinkType = GridLinkTypeEnum.Electrical;
		}
	}
}
