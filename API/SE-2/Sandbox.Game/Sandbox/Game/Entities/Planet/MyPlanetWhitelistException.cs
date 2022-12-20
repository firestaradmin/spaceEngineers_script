using System;

namespace Sandbox.Game.Entities.Planet
{
	public class MyPlanetWhitelistException : Exception
	{
		public MyPlanetWhitelistException(string message)
			: base(message)
		{
		}
	}
}
