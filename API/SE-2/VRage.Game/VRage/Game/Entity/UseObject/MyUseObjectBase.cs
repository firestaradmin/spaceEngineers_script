using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.Entity.UseObject
{
	public abstract class MyUseObjectBase : IMyUseObject
	{
<<<<<<< HEAD
		public IMyEntity Owner { get; }

		public IMyModelDummy Dummy { get; }

		public virtual float InteractiveDistance => MyConstants.DEFAULT_INTERACTIVE_DISTANCE;

		public virtual MatrixD ActivationMatrix => Dummy.Matrix * Owner.WorldMatrix;

		public virtual MatrixD WorldMatrix => Owner.WorldMatrix;

		public virtual uint RenderObjectID => Owner.Render.GetRenderObjectID();

		public virtual int InstanceID => -1;

		public virtual bool ShowOverlay => true;

		public virtual UseActionEnum SupportedActions => PrimaryAction | SecondaryAction;
=======
		public IMyEntity Owner { get; private set; }

		public MyModelDummy Dummy { get; private set; }

		public abstract float InteractiveDistance { get; }

		public abstract MatrixD ActivationMatrix { get; }

		public abstract MatrixD WorldMatrix { get; }

		public abstract uint RenderObjectID { get; }

		public virtual int InstanceID => -1;

		public abstract bool ShowOverlay { get; }

		public abstract UseActionEnum SupportedActions { get; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public abstract UseActionEnum PrimaryAction { get; }

		public abstract UseActionEnum SecondaryAction { get; }

<<<<<<< HEAD
		public virtual bool ContinuousUsage => false;

		public virtual bool PlayIndicatorSound => true;
=======
		public abstract bool ContinuousUsage { get; }

		public abstract bool PlayIndicatorSound { get; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		protected MyUseObjectBase(IMyEntity owner, IMyModelDummy dummy)
		{
			Owner = owner;
			Dummy = dummy;
		}

		public abstract void Use(UseActionEnum actionEnum, IMyEntity user);

		public abstract MyActionDescription GetActionInfo(UseActionEnum actionEnum);

		public virtual bool HandleInput()
		{
			return false;
		}

		public virtual void OnSelectionLost()
		{
		}

		public virtual void SetRenderID(uint id)
		{
		}

		public virtual void SetInstanceID(int id)
		{
		}
	}
}
