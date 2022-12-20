using System;
using VRage.Game.Entity;

namespace Sandbox.Game.Entities
{
	public interface IMyUpdateOrchestrator
	{
		void AddEntity(MyEntity entity);

		void RemoveEntity(MyEntity entity, bool immediate);

		void EntityFlagsChanged(MyEntity entity);

<<<<<<< HEAD
		/// <summary>
		/// Schedule a callback to be invoked in the update thread at a later time.
		/// </summary>
		/// <remarks>This callback will be invoked as soon as possible (possibly even on the same frame).</remarks>
		/// <param name="action"></param>
		/// <param name="debugName"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		void InvokeLater(Action action, string debugName = null);

		void DispatchOnceBeforeFrame();

		void DispatchBeforeSimulation();

		void DispatchSimulate();

		void DispatchAfterSimulation();

		void DispatchUpdatingStopped();

		void Unload();

		void DebugDraw();
	}
}
