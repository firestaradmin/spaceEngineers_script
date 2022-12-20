using System;
using VRage.ObjectBuilders;
using VRageMath;

namespace VRage.Game.ModAPI.Ingame
{
	/// <summary>
	/// Basic cube interface
	/// </summary>
	public interface IMyCubeBlock : IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets definition.Id assigned to this block
		/// </summary>
		SerializableDefinitionId BlockDefinition { get; }

		/// <summary>
		/// Whether the grid should call the ConnectionAllowed method for this block 
		///             (ConnectionAllowed checks mount points and other per-block requirements)
		/// </summary>
		bool CheckConnectionAllowed { get; set; }
=======
		SerializableDefinitionId BlockDefinition { get; }

		bool CheckConnectionAllowed { get; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Grid in which the block is placed
		/// </summary>
		IMyCubeGrid CubeGrid { get; }

		/// <summary>
		/// Definition name
		/// </summary>
		string DefinitionDisplayNameText { get; }

		/// <summary>
		/// Is set in definition
		/// Ratio at which is the block disassembled (grinding)
		/// Bigger values - longer grinding
		/// </summary>
		float DisassembleRatio { get; }

		/// <summary>
		/// Translated block name
		/// </summary>
		string DisplayNameText { get; }

		/// <summary>
		/// Hacking of the block is in progress
		/// </summary>
		bool IsBeingHacked { get; }

		/// <summary>
		/// Gets if integrity is above breaking threshold
		/// </summary>
		bool IsFunctional { get; }

		/// <summary>
		/// True if block is able to do its work depening on block type (is functional, powered, enabled, etc...)
		/// </summary>
		bool IsWorking { get; }

		/// <summary>
		/// Maximum coordinates of grid cells occupied by this block
		/// </summary>
		Vector3I Max { get; }

		/// <summary>
		/// Block mass
		/// </summary>
		float Mass { get; }

		/// <summary>
		/// Minimum coordinates of grid cells occupied by this block
		/// </summary>
		Vector3I Min { get; }

		/// <summary>
		/// Order in which were the blocks of same type added to grid
		/// Used in default display name
		/// </summary>
		int NumberInGrid { get; }

		/// <summary>
		/// Returns block orientation in base 6 directions
		/// </summary>
		MyBlockOrientation Orientation { get; }

		/// <summary>
		/// IdentityId of player owning block (not steam Id)
		/// </summary>
		long OwnerId { get; }

		/// <summary>
		/// Position in grid coordinates
		/// </summary>
		Vector3I Position { get; }

		/// <summary>
		/// Tag of faction owning block
		/// </summary>
		string GetOwnerFactionTag();

		/// <summary>
		/// Relation of local player to the block
		/// Should not be called on Dedicated Server.
		/// </summary>
		/// <returns>Relation</returns>
		[Obsolete("GetPlayerRelationToOwner() is useless ingame. Mods should use the one in ModAPI.IMyCubeBlock")]
		MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner();

		/// <summary>
		/// Gets relation to owner of block
		/// </summary>
		/// <param name="playerId">IdentityId of player to check relation with (not steam id!)</param>
		/// <param name="defaultNoUser"></param>
		/// <returns>Relation of defined player to the block</returns>
		MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership);

		/// <summary>
		/// Force refresh working state. Call if you change block state that could affect its working status.
		/// </summary>
		[Obsolete]
		void UpdateIsWorking();

		/// <summary>
		/// Updates block visuals (ie. block emissivity)
		/// </summary>
		[Obsolete]
		void UpdateVisual();
	}
}
