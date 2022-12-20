using System;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes API, that allows you get information about DLCs (mods interface)
	/// </summary>
	public interface IMyDLCs
	{
		/// <summary>
		/// Called when a new DLC is installed by a client. On the client, this only reports DLC installed on the local client.
		/// </summary>
		/// <remarks>
		/// The first action argument is the client SteamId. The second argument is the DLC AppId.
		/// This event will be called on the server each time a player connects.
		/// <para />
		/// Note:
		/// <para>Space Engineers 2013 is AppId 573900</para>
		/// <para>Deluxe Edition Artwork is AppId 573161</para>
		/// Neither of the above DLC are present in the list returned by <see cref="M:VRage.Game.ModAPI.IMyDLCs.GetDLCs" />. In addition, <see cref="M:VRage.Game.ModAPI.IMyDLCs.GetDLC(System.UInt32)" /> will throw <see cref="T:System.Collections.Generic.KeyNotFoundException" /> when passed either of those AppIds.
		/// </remarks>
		event Action<ulong, uint> DLCInstalled;

		/// <summary>
		/// Gets information on the requested DLC.
		/// </summary>
		/// <param name="appId">The <see cref="P:VRage.Game.ModAPI.IMyDLC.AppId" /> of the DLC</param>
		/// <param name="dlc">The requested DLC information.</param>
		/// <returns><see langword="true" /> if DLC exists and <paramref name="dlc" /> has valid contents.</returns>
		bool TryGetDLC(uint appId, out IMyDLC dlc);

		/// <summary>
		/// Gets information on the requested DLC.
		/// </summary>
		/// <param name="name">The <see cref="P:VRage.Game.ModAPI.IMyDLC.Name" /> of the DLC</param>
		/// <param name="dlc">The requested DLC information.</param>
		/// <returns><see langword="true" /> if DLC exists and <paramref name="dlc" /> has valid contents.</returns>
		bool TryGetDLC(string name, out IMyDLC dlc);

		/// <summary>
		/// Gets information on the requested DLC.
		/// </summary>
		/// <param name="name">The <see cref="P:VRage.Game.ModAPI.IMyDLC.Name" /> of the DLC</param>
		/// <returns>Requested DLC information.</returns>
		/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">Name supplied doesn't match a known DLC.</exception>
		IMyDLC GetDLC(string name);

		/// <summary>
		/// Gets information on the requested DLC.
		/// </summary>
		/// <param name="appId">The <see cref="P:VRage.Game.ModAPI.IMyDLC.AppId" /> of the DLC</param>
		/// <returns>Requested DLC information.</returns>
		/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">AppId supplied doesn't match a known DLC.</exception>
		IMyDLC GetDLC(uint appId);

		/// <summary>
		/// Check if DLC is supported on this platform.
		/// </summary>
		/// <param name="name">The <see cref="P:VRage.Game.ModAPI.IMyDLC.Name" /> of the DLC</param>
		/// <returns></returns>
		bool IsDLCSupported(string name);

		/// <summary>
		/// Returns the tooltip text that is shown to the user if they don't own the DLC.
		/// </summary>
		/// <param name="name">The <see cref="P:VRage.Game.ModAPI.IMyDLC.Name" /> of the DLC</param>
		/// <returns>Localized and formatted version of <see cref="F:MyCommonTexts.RequiresDlc" /></returns>
		string GetRequiredDLCTooltip(string name);

		/// <summary>
		/// Returns the tooltip text that is shown to the user if they don't own the DLC.
		/// </summary>
		/// <param name="appId">The <see cref="P:VRage.Game.ModAPI.IMyDLC.AppId" /> of the DLC</param>
		/// <returns>Localized and formatted version of <see cref="F:MyCommonTexts.RequiresDlc" /></returns>
		string GetRequiredDLCTooltip(uint appId);

		/// <summary>
		/// Get a list of all DLCs the game has. This will not change for the lifetime of the session and is safe to cache.
		/// </summary>
		ListReader<IMyDLC> GetDLCs();

		/// <summary>
		/// Returns a list of the DLCs installed on the local client.
		/// </summary>
		/// <returns>List of <see cref="P:VRage.Game.ModAPI.IMyDLC.AppId" />.</returns>
		ListReader<uint> GetAvailableClientDLCIds();

		/// <summary>
		/// Return whether a player owns a DLC.
		/// </summary>
		/// <param name="name">The <see cref="P:VRage.Game.ModAPI.IMyDLC.Name" /> of the DLC</param>
		/// <param name="steamId">The SteamID of the player to check.</param>
		/// <returns><see langword="true" /> if the player owns that DLC.</returns>
		bool HasDLC(string name, ulong steamId);

		/// <summary>
		/// Return whether a player owns a DLC.
		/// </summary>
		/// <param name="appId">The <see cref="P:VRage.Game.ModAPI.IMyDLC.AppId" /> of the DLC</param>
		/// <param name="steamId">The SteamID of the player to check.</param>
		/// <returns><see langword="true" /> if the player owns that DLC.</returns>
		bool HasDLC(uint appId, ulong steamId);

		/// <summary>
		/// Returns whether a player owns all DLCs required by this definition id.
		/// </summary>
		/// <param name="definitionId">The definition id to check.</param>
		/// <param name="steamId">The SteamID of the user to check against.</param>
		/// <returns><see langword="true" /> if the player owns all the DLC in the definition.</returns>
		bool HasDefinitionDLC(MyDefinitionId definitionId, ulong steamId);

		/// <summary>
		/// Returns whether a player owns all DLCs required by this definition.
		/// </summary>
		/// <param name="definition">The definition to check.</param>
		/// <param name="steamId">The SteamID of the user to check against.</param>
		/// <returns><see langword="true" /> if the player owns all the DLC in the definition.</returns>
		bool HasDefinitionDLC(MyDefinitionBase definition, ulong steamId);

		/// <summary>
		/// Returns whether the collection contains the DLCs in this definition.
		/// </summary>
		/// <param name="definition">The definition to check.</param>
		/// <param name="dlcs">The collection to compare against.</param>
		/// <returns><see langword="true" /> if the definition has no DLCs specified, or all the DLCs specified in <paramref name="definition" /> are present in <paramref name="dlcs" />.</returns>
		bool ContainsRequiredDLC(MyDefinitionBase definition, List<ulong> dlcs);

		/// <summary>
		/// Get the first DLC a player is missing that a definition requires. Null if they have all.
		/// </summary>
		/// <param name="definition">The definition to check.</param>
		/// <param name="steamId">The SteamID of the user to check against.</param>
		/// <returns>The first DLC not available by the specified <paramref name="steamId" />. <see langword="null" /> if definition has no DLC, or the user has all of them.</returns>
		IMyDLC GetFirstMissingDefinitionDLC(MyDefinitionBase definition, ulong steamId);
	}
}
