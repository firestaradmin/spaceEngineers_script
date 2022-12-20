using System;

namespace VRage.Mod.Io.Data
{
	[Serializable]
	internal class Ratings
	{
		public int game_id;

		public int mod_id;

		public int rating;

		public int date_added;
	}
}
