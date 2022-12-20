using System;
using VRageMath;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes GPS (mods interface) 
	/// </summary>
	public interface IMyGps
	{
		/// <summary>
		/// The GPS entry id hash which is generated using the GPS name and coordinates.
		/// </summary>
		int Hash { get; }

<<<<<<< HEAD
		/// <summary>
		/// Gets or sets GPS name
		/// </summary>
		/// <remarks>Set method doesn't synchronize data over network. Use <see cref="M:VRage.Game.ModAPI.IMyGpsCollection.ModifyGps(System.Int64,VRage.Game.ModAPI.IMyGps)" />, to update data</remarks>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets GPS description
		/// </summary>
		/// <remarks>Set method doesn't synchronize data over network. Use <see cref="M:VRage.Game.ModAPI.IMyGpsCollection.ModifyGps(System.Int64,VRage.Game.ModAPI.IMyGps)" />, to update data</remarks>
		string Description { get; set; }

		/// <summary>
		/// Gets or sets GPS coordinates
		/// </summary>
		/// <remarks>Set method doesn't synchronize data over network. Use <see cref="M:VRage.Game.ModAPI.IMyGpsCollection.ModifyGps(System.Int64,VRage.Game.ModAPI.IMyGps)" />, to update data</remarks>
		Vector3D Coords { get; set; }

		/// <summary>
		/// Gets or sets GPS color
		/// </summary>
		/// <remarks>Set method doesn't synchronize data over network. Use <see cref="M:VRage.Game.ModAPI.IMyGpsCollection.ModifyGps(System.Int64,VRage.Game.ModAPI.IMyGps)" />, to update data</remarks>
		Color GPSColor { get; set; }

		/// <summary>
		/// Gets or sets whether GPS should be visible on HUD
		/// </summary>
		/// <remarks>Set method doesn't synchronize data over network. Use <see cref="M:VRage.Game.ModAPI.IMyGpsCollection.ModifyGps(System.Int64,VRage.Game.ModAPI.IMyGps)" />, to update data</remarks>
=======
		string Name { get; set; }

		string Description { get; set; }

		Vector3D Coords { get; set; }

		Color GPSColor { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool ShowOnHud { get; set; }

		/// <summary>
		/// If it's null then the GPS is confirmed (does not expire automatically).
		/// Otherwise, time at which we should drop it from the list, relative to ElapsedPlayTime
		/// </summary>
<<<<<<< HEAD
		/// <remarks>Set method doesn't synchronize data over network. Use <see cref="M:VRage.Game.ModAPI.IMyGpsCollection.ModifyGps(System.Int64,VRage.Game.ModAPI.IMyGps)" />, to update data</remarks>
		TimeSpan? DiscardAt { get; set; }

		/// <summary>
		/// Gets or sets gps text that would be displayed on HUD
		/// </summary>
		/// <remarks>Set method doesn't synchronize data over network. Use <see cref="M:VRage.Game.ModAPI.IMyGpsCollection.ModifyGps(System.Int64,VRage.Game.ModAPI.IMyGps)" />, to update data</remarks>
=======
		TimeSpan? DiscardAt { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		string ContainerRemainingTime { get; set; }

		/// <summary>
		/// Updates the hash id if you've changed the name or the coordinates.
		/// NOTE: Do not use this if you plan on using this object to update existing GPS entries.
		/// </summary>
		void UpdateHash();

		/// <summary>
		/// Gets information about GPS
		/// </summary>
		/// <returns>String, same that user gets on `Copy to clipboard`</returns>
		/// <remarks>Set method doesn't synchronize data over network. Use <see cref="M:VRage.Game.ModAPI.IMyGpsCollection.ModifyGps(System.Int64,VRage.Game.ModAPI.IMyGps)" />, to update data</remarks>
		new string ToString();
	}
}
