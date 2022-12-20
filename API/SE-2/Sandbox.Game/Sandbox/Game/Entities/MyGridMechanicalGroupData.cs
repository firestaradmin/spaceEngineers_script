using Sandbox.Game.Entities.Cube;
using VRage.Game.ModAPI;

namespace Sandbox.Game.Entities
{
	public class MyGridMechanicalGroupData : MyGridGroupData<MyGridMechanicalGroupData>
	{
		public MyGridMechanicalGroupData()
		{
			base.LinkType = GridLinkTypeEnum.Mechanical;
		}
	}
}
