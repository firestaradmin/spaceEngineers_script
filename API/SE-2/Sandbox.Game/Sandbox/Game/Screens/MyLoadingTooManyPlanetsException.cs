<<<<<<< HEAD
using System;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.Localization;

namespace Sandbox.Game.Screens
{
	public class MyLoadingTooManyPlanetsException : MyLoadingException
	{
		public MyLoadingTooManyPlanetsException()
			: base(MySpaceTexts.Notification_TooManyPlanets)
		{
		}
<<<<<<< HEAD

		public MyLoadingTooManyPlanetsException(Exception e)
			: base(MySpaceTexts.Notification_TooManyPlanets, e)
		{
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
