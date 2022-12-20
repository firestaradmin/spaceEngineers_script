using System.Collections.Generic;
using VRage.Utils;

namespace VRageRender.Animations
{
	/// <summary>
	/// Interface representing one node in animation tree.
	/// </summary>
	public abstract class MyAnimationTreeNode
	{
		public string Key;

		public abstract void Update(ref MyAnimationUpdateData data, List<string> eventCollection);

		public abstract float GetLocalTimeNormalized();

		public abstract void SetLocalTimeNormalized(float normalizedTime);

		public virtual void SetAction(MyStringId action)
		{
		}
	}
}
