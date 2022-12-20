namespace Sandbox.Game.Replication
{
	public static class MyReplicationHelpers
	{
		/// <summary>
		/// Ramps the priority up or down based on how often it should send updates and when it was last sent.
		/// Returns zero when it was sent recently (not more than 'updateOncePer' before).
		/// </summary>
		/// <param name="priority">Original priority.</param>
		/// <param name="frameCountWithoutSync">Number of frames without sync</param>
		/// <param name="updateOncePer">How often it should be normally updated</param>
		/// <param name="rampAmount">How much to ramp (0.5f means increase priority by 50% for each 'updateOncePer' number of frame late)</param>
		/// <param name="alsoRampDown">Ramps priority also down (returns zero priority when sent less than 'updateOncePer' frames before)</param>
		public static float RampPriority(float priority, int frameCountWithoutSync, float updateOncePer, float rampAmount = 0.5f, bool alsoRampDown = true)
		{
			if ((float)frameCountWithoutSync >= updateOncePer)
			{
				float num = ((float)frameCountWithoutSync - updateOncePer) / updateOncePer;
				if (num > 1f)
				{
					float num2 = (num - 1f) * rampAmount;
					priority *= num2;
				}
				return priority;
			}
			if (!alsoRampDown)
			{
				return priority;
			}
			return 0f;
		}
	}
}
