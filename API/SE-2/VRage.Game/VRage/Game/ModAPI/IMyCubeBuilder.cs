using System;
using VRage.ModAPI;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes hand, than holding block (mods interface) 
	/// </summary>
	public interface IMyCubeBuilder
	{
		/// <summary>
		///  Returns state of building mode
		/// </summary>
		bool BlockCreationIsActivated { get; }

		/// <summary>
		/// Freezes the built object preview in current position
		/// </summary>
		bool FreezeGizmo { get; set; }

		/// <summary>
		/// Shows the delete area preview
		/// </summary>
		bool ShowRemoveGizmo { get; set; }

		/// <summary>
		/// Enables symmetry block placing
		/// </summary>
		bool UseSymmetry { get; set; }

		/// <summary>
		/// Gets or sets whether projected block should be 25% transparent
		/// </summary>
		bool UseTransparency { get; set; }

		/// <summary>
		/// Gets whether there block creation is activated
		/// </summary>
		bool IsActivated { get; }

		/// <summary>
		/// Activates the building mode
		/// </summary>
		void Activate(MyDefinitionId? blockDefinitionId = null);

		/// <summary>
		/// Deactivates all modes
		/// </summary>
		void Deactivate();

		/// <summary>
		/// Deactivates building mode
		/// </summary>
		void DeactivateBlockCreation();

		/// <summary>
		/// Calls <see cref="M:VRage.Game.ModAPI.IMyCubeBuilder.Activate(System.Nullable{VRage.Game.MyDefinitionId})" /> with LargeBlockArmorBlock definition and forces local player stop using weapon or tool 
		/// </summary>
		/// <param name="cubeSize">Ignored</param>
		/// <param name="isStatic">Ignored</param>
		void StartNewGridPlacement(MyCubeSize cubeSize, bool isStatic);

		/// <summary>
		/// Finds grid to build on
		/// </summary>
		/// <returns>Found grid</returns>
		IMyCubeGrid FindClosestGrid();

		[Obsolete("Not used. Always returns false")]
		bool AddConstruction(IMyEntity buildingEntity);
	}
}
