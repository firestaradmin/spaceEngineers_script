using System;

namespace Sandbox.Game.Entities.Debris
{
	/// <summary>
	/// Description of 
	/// </summary>
	public class MyDebrisBaseDescription
	{
		public string Model;

		public int LifespanMinInMiliseconds;

		public int LifespanMaxInMiliseconds;

		public Action<MyDebrisBase> OnCloseAction;
	}
}
