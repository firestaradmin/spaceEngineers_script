using System;
using System.Collections.Generic;
using Sandbox.Engine.Networking;
using Sandbox.Game.World;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyNewCustomWorldInfoListResult : MyLoadListResult
	{
		public MyNewCustomWorldInfoListResult(List<string> customPaths = null)
			: base(customPaths)
		{
		}

		protected override List<Tuple<string, MyWorldInfo>> GetAvailableSaves()
		{
			return MyLocalCache.GetAvailableWorldInfos(CustomPaths);
		}
	}
}
