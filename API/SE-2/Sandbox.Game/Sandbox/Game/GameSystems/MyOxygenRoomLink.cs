namespace Sandbox.Game.GameSystems
{
	/// <summary>
	/// Used as a pointer so that we can change rooms fast without iterating through all of the blocks
	/// </summary>
	public class MyOxygenRoomLink
	{
		public MyOxygenRoom Room { get; set; }

		public MyOxygenRoomLink(MyOxygenRoom room)
		{
			SetRoom(room);
		}

		private void SetRoom(MyOxygenRoom room)
		{
			Room = room;
			Room.Link = this;
		}
	}
}
