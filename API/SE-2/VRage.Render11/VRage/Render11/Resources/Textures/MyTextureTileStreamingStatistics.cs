namespace VRage.Render11.Resources.Textures
{
	internal struct MyTextureTileStreamingStatistics
	{
		public int LoadedTiles { get; internal set; }

		public int MissedTiles { get; internal set; }

		public int StreamedTiles { get; internal set; }

		public int SwapsPerformed { get; internal set; }

		public int TotalSwapsPerformed { get; internal set; }

		public int LoadedMaxPriority { get; internal set; }

		public int LoadedMedianPriority { get; internal set; }

		public int LoadedMinPriority { get; internal set; }

		public int MissedAvgPriority { get; internal set; }

		public void ResetFrame()
		{
			LoadedTiles = 0;
			MissedTiles = 0;
			StreamedTiles = 0;
			SwapsPerformed = 0;
		}
	}
}
