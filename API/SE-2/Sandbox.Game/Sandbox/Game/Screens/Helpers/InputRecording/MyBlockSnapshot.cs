using VRage.Game;

namespace Sandbox.Game.Screens.Helpers.InputRecording
{
	public class MyBlockSnapshot
	{
		public MyCubeSize Grid { get; set; }

		public string CurrentBlockName { get; set; }

		public int? Stage { get; set; }

		public int? LOD { get; set; }
	}
}
