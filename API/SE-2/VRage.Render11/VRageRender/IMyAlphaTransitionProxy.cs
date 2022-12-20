using System;
using VRage.Render;

namespace VRageRender
{
	internal interface IMyAlphaTransitionProxy
	{
		/// <summary>
		/// Set the alpha value for the transition proxy.
		/// </summary>
		/// <param name="mode">Mode of the alpha value.</param>
		/// <param name="value">Value of the alpha blending.</param>
		void SetAlpha(MyAlphaMode mode, float value);

		/// <summary>
		/// Called upon completion of the transition operation.
		/// </summary>
		/// <param name="direction"></param>
<<<<<<< HEAD
		/// <param name="transitionFinishedCallback"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		void TransitionComplete(MyAlphaTransitionDirection direction, Action<uint> transitionFinishedCallback = null);

		/// <summary>
		/// Called upon completion of the transition operation.
		/// </summary>
<<<<<<< HEAD
		/// <param name="direction"></param>        
=======
		/// <param name="direction"></param>
		/// <param name="visible"></param>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		void TransitionStart(MyAlphaTransitionDirection direction);
	}
}
