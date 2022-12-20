using System;
using VRage.Game.ModAPI.Interfaces;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes entity controller (mods interface)
	/// Allows to take controls 
	/// </summary>
	public interface IMyEntityController
	{
<<<<<<< HEAD
		/// <summary>
		/// Get currently controlled entity
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		IMyControllableEntity ControlledEntity { get; }

		/// <summary>
		/// Event triggered when currently controlled grid is changed
		/// </summary>
		event Action<IMyControllableEntity, IMyControllableEntity> ControlledEntityChanged;

		/// <summary>
		/// Taking control on entity.
		/// </summary>
		/// <param name="entity">Entity to control</param>
		void TakeControl(IMyControllableEntity entity);
	}
}
