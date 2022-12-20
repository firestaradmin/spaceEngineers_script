using VRage.Game.Components;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
	/// <summary>
	/// Reference counting component for entities.
	///
	/// Allows simplified management of short lived entities that may be shared amongst systems.
	///
	/// The count is initially 0 so the first referencee becomes the owner of the
	/// entity (this is sometimes called a floating reference).
	/// </summary>
	public class MyEntityReferenceComponent : MyEntityComponentBase
	{
		private class Sandbox_Game_EntityComponents_MyEntityReferenceComponent_003C_003EActor : IActivator, IActivator<MyEntityReferenceComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityReferenceComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityReferenceComponent CreateInstance()
			{
				return new MyEntityReferenceComponent();
			}

			MyEntityReferenceComponent IActivator<MyEntityReferenceComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private int m_references;

		public override string ComponentTypeDebugString => "ReferenceCount";

		/// <summary>
		/// Increase the reference count of this entity.
		/// </summary>
		public void Ref()
		{
			m_references++;
		}

		/// <summary>
		/// Decrease the entitie's reference count.
		/// </summary>
		/// <returns>Weather the count reached 0 and the entity was marked for close.</returns>
		public bool Unref()
		{
			m_references--;
			if (m_references <= 0)
			{
				base.Entity.Close();
				return true;
			}
			return false;
		}
	}
}
