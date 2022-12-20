using VRage.Game.ModAPI;

namespace Sandbox.Game.Entities.Cube
{
	public class MyGridNoDamageGroupData : MyGridGroupData<MyGridNoDamageGroupData>
	{
		public MyGridNoDamageGroupData()
		{
			base.LinkType = GridLinkTypeEnum.NoContactDamage;
		}
	}
}
