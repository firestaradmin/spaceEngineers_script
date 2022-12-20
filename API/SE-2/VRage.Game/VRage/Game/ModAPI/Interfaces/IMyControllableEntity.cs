using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI.Interfaces
{
	/// <summary>
	/// Describes that player can take under control (mods interface)
	/// </summary>
	public interface IMyControllableEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets information about who controls this Entity 
		/// </summary>
		IMyControllerInfo ControllerInfo { get; }

		/// <summary>
		/// Gets information which entity is being controlled
		/// </summary>
		IMyEntity Entity { get; }

		/// <summary>
		/// Gets or sets if camera should be first person
		/// </summary>
		bool ForceFirstPersonCamera { get; set; }

		/// <summary>
		/// Gets last motion indicator. User input W/A/S/D Space/C
		/// </summary>
		/// <remarks>Works only for cockpits and remote control blocks</remarks>
		Vector3 LastMotionIndicator { get; }

		/// <summary>
		/// Gets last rotation indicator. Z used for RollIndicator
		/// </summary>
		Vector3 LastRotationIndicator { get; }

		/// <summary>
		/// Gets if thrusts are enabled
		/// </summary>
		/// <remarks>Works only for character</remarks>
		bool EnabledThrusts { get; }

		/// <summary>
		/// Gets if Damping enabled
		/// </summary>
		/// <remarks>Works only for character and ship controller</remarks>
		bool EnabledDamping { get; }

		/// <summary>
		/// Gets if lights are enabled
		/// </summary>
		/// <remarks>Works only for character and ship controller</remarks>
		bool EnabledLights { get; }

		/// <summary>
		/// Gets if at least one leading gear is enabled
		/// </summary>
		/// <remarks>Works only for ship controller</remarks>
		bool EnabledLeadingGears { get; }

		/// <summary>
		/// Gets if grid is powered
		/// </summary>
		/// <remarks>Works only for ship controller</remarks>
		bool EnabledReactors { get; }

		/// <summary>
		/// Gets if helmet is opened
		/// </summary>
		/// <remarks>Works only for character</remarks>
		bool EnabledHelmet { get; }

		/// <summary>
		/// When false, blocks 3rd view look around
		/// </summary>
		bool PrimaryLookaround { get; }

		/// <summary>
		/// Gets head of character that controls this
		/// </summary>
		/// <param name="includeY">Should include Y axis rotation</param>
		/// <param name="includeX">Should include X axis rotation</param>
		/// <param name="forceHeadAnim">When true - use very accurate head position</param>
		/// <param name="forceHeadBone"></param>
		/// <returns></returns>
=======
		IMyControllerInfo ControllerInfo { get; }

		IMyEntity Entity { get; }

		bool ForceFirstPersonCamera { get; set; }

		Vector3 LastMotionIndicator { get; }

		Vector3 LastRotationIndicator { get; }

		bool EnabledThrusts { get; }

		bool EnabledDamping { get; }

		bool EnabledLights { get; }

		bool EnabledLeadingGears { get; }

		bool EnabledReactors { get; }

		bool EnabledHelmet { get; }

		bool PrimaryLookaround { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		MatrixD GetHeadMatrix(bool includeY, bool includeX = true, bool forceHeadAnim = false, bool forceHeadBone = false);

		/// <summary>
		/// Defines user input. Call <see cref="M:VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotateStopped" /> on user input finished
		/// </summary>
		/// <param name="moveIndicator"> User input W/A/S/D Space/C</param>
		/// <param name="rotationIndicator">User mouse input</param>
		/// <param name="rollIndicator">User input Q/E</param>
		void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator);

		/// <summary>
		/// Should be called when input is finished
		/// </summary>
		void MoveAndRotateStopped();

		void Use();

		void UseContinues();

		/// <summary>
		/// Forwarding this action to <see cref="M:VRage.Game.Entity.UseObject.IMyUseObject.Use(VRage.Game.Entity.UseObject.UseActionEnum,VRage.ModAPI.IMyEntity)" /> with argument <see cref="F:VRage.Game.Entity.UseObject.UseActionEnum.PickUp" />
		/// </summary>
		/// <remarks>Works only for <see cref="T:VRage.Game.ModAPI.IMyCharacter" /></remarks>
		void PickUp();

		/// <summary>
		/// Forwarding this action to <see cref="M:VRage.Game.Entity.UseObject.IMyUseObject.Use(VRage.Game.Entity.UseObject.UseActionEnum,VRage.ModAPI.IMyEntity)" /> with argument <see cref="F:VRage.Game.Entity.UseObject.UseActionEnum.PickUp" />
		/// </summary>
		/// <remarks>Works only for <see cref="T:VRage.Game.ModAPI.IMyCharacter" />. Method is called after first call of <see cref="M:VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUp" />, and only if target supports <see cref="P:VRage.Game.Entity.UseObject.IMyUseObject.ContinuousUsage" /></remarks>
		void PickUpContinues();

		/// <summary>
		/// Move direction : up. Only <see cref="T:VRage.Game.ModAPI.IMyCharacter" /> has implementation for this method.
		/// </summary>
		void Up();

		/// <summary>
		/// Move direction : down. Only <see cref="T:VRage.Game.ModAPI.IMyCharacter" /> has implementation for this method.
		/// </summary>
		void Down();

		/// <summary>
		/// Character jump. Only <see cref="T:VRage.Game.ModAPI.IMyCharacter" /> has implementation for this method.
		/// </summary>
		void Jump(Vector3 moveindicator = default(Vector3));

		/// <summary>
		/// Switch between walk / run mode. Only <see cref="T:VRage.Game.ModAPI.IMyCharacter" /> has implementation for this method.
		/// </summary>
		void SwitchWalk();

		/// <summary>
		/// Only <see cref="T:VRage.Game.ModAPI.IMyCharacter" /> has implementation for this method.
		/// </summary>
		void Crouch();

		/// <summary>
		/// Shows inventory gui of controlled Entity
		/// </summary>
		/// <remarks>Not all ControllableEntities implements this</remarks>
		void ShowInventory();

		/// <summary>
		/// Shows terminal gui
		/// </summary>
		/// <remarks>Not all ControllableEntities implements this</remarks>
		void ShowTerminal();

		/// <summary>
		/// Turns on jetpack on character
		/// </summary>
		/// <remarks>Not all ControllableEntities implements this</remarks>
		void SwitchThrusts();

		/// <summary>
		/// Switches damping state
		/// </summary>
		/// <remarks>Not all ControllableEntities implements this</remarks>
		void SwitchDamping();

		/// <summary>
		/// Switches lights state
		/// </summary>
		/// <remarks>Not all ControllableEntities implements this</remarks>
		void SwitchLights();

		/// <summary>
		/// Switches landing gears state
		/// </summary>
		/// <remarks>Not all ControllableEntities implements this</remarks>
		void SwitchLandingGears();

<<<<<<< HEAD
		/// <summary>
		/// Switches handbrake state
		/// </summary>
		/// <remarks>Not all ControllableEntities implements this</remarks>
		void SwitchHandbrake();

		/// <summary>
		/// Switches reactors state
		/// </summary>
		/// <remarks>Not all ControllableEntities implements this</remarks>
=======
		void SwitchHandbrake();

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		void SwitchReactors();

		/// <summary>
		/// Switches handbrake state
		/// </summary>
		/// <remarks>Same as <see cref="M:VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchReactors" /></remarks>
		void SwitchReactorsLocal();

		/// <summary>
		/// Switches helmet open/closed state
		/// </summary>
		/// <remarks>Not all ControllableEntities implements this</remarks>
		void SwitchHelmet();

		/// <summary>
		/// Updates hud logic, connected to entity
		/// </summary>
		/// <param name="camera">Current camera</param>
		/// <param name="playerId">IdentityId</param>
		void DrawHud(IMyCameraController camera, long playerId);

		/// <summary>
		/// Makes character dead. 
		/// </summary>
		/// <remarks>Works only for character</remarks>
		void Die();
	}
}
