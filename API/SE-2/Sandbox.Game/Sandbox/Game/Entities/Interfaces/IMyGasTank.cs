using System;
using VRage.Game;

namespace Sandbox.Game.Entities.Interfaces
{
	public interface IMyGasTank
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets the gas capacity of this tank.
		/// </summary>
		float GasCapacity { get; }

		/// <summary>
		/// Gets the current fill level of this tank as a value between 0 (empty) and 1 (full).
		/// </summary>
		double FilledRatio { get; }

		/// <summary>
		/// When Filled Ratio is changed
		/// </summary>
=======
		float GasCapacity { get; }

		double FilledRatio { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		Action FilledRatioChanged { get; set; }

		/// <summary>
		/// Can the gas tank store a specified resource?
		/// </summary>
		bool IsResourceStorage(MyDefinitionId resourceDefinition);
	}
}
