using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes wheel suspension (PB scripting interface)
	/// </summary>
	public interface IMyMotorSuspension : IMyMotorBase, IMyMechanicalConnectionBlock, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets or sets whether suspension can steer
		/// </summary>
		bool Steering { get; set; }

		/// <summary>
		/// Gets or sets whether suspension can propulse 
		/// </summary>
		bool Propulsion { get; set; }

		/// <summary>
		/// Gets or sets whether suspension steering is inverted
		/// </summary>
		bool InvertSteer { get; set; }

		/// <summary>
		/// Gets or sets whether suspension propulsion is inverted
		/// </summary>
		bool InvertPropulsion { get; set; }

		/// <summary>
		/// Gets or sets whether suspension reacts on parking break
		/// </summary>
		bool IsParkingEnabled { get; set; }
=======
		bool Steering { get; set; }

		bool Propulsion { get; set; }

		bool InvertSteer { get; set; }

		bool InvertPropulsion { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		[Obsolete]
		float Damping { get; }

<<<<<<< HEAD
		/// <summary>
		/// Gets or sets whether suspension strength [0..100]%
		/// </summary>
		float Strength { get; set; }

		/// <summary>
		/// Gets or sets whether suspension friction [0..100]%
		/// </summary>
		float Friction { get; set; }

		/// <summary>
		/// Gets or sets whether suspension power [0..100]%
		/// </summary>
		float Power { get; set; }

		/// <summary>
		/// Gets or sets whether suspension height in meters. Limited with block definition settings
		/// </summary>
=======
		float Strength { get; set; }

		float Friction { get; set; }

		float Power { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		float Height { get; set; }

		/// <summary>
		/// Gets suspension current steering angle
		/// </summary>
		float SteerAngle { get; }

		/// <summary>
		/// Gets or sets max steering angle in radians.
		/// </summary>
		float MaxSteerAngle { get; set; }

		/// <summary>
		/// Speed at which wheel steers.
		/// </summary>
		[Obsolete]
		float SteerSpeed { get; }

		/// <summary>
		/// Speed at which wheel returns from steering.
		/// </summary>
		[Obsolete]
		float SteerReturnSpeed { get; }

		/// <summary>
		/// Suspension travel, value from 0 to 1.
		/// </summary>
		[Obsolete]
		float SuspensionTravel { get; }
<<<<<<< HEAD

		/// <summary>
		/// Gets or sets if brakes are applied to the wheel. This is not a brake override.
		/// </summary>
		bool Brake { get; set; }

		/// <summary>
		/// Enables or disables AirShock function.
		/// </summary>
		bool AirShockEnabled { get; set; }
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Steering override proportion, value from -1 to 1.
		/// </summary>
<<<<<<< HEAD
		float SteeringOverride { get; set; }
=======
		bool Brake { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Propulsion override proportion, value from -1 to 1.
		/// </summary>
<<<<<<< HEAD
		float PropulsionOverride { get; set; }
=======
		bool AirShockEnabled { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
