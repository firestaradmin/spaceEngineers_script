using VRage.Render11.Common;

namespace VRage.Render11.GeometryStage2.Common
{
	internal class MyIDGeneratorManager : IManager, IManagerUnloadData
	{
		public readonly MyIDGenerator Lods = new MyIDGenerator();

		void IManagerUnloadData.OnUnloadData()
		{
		}
	}
}
