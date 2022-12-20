using System.Collections.Generic;
using Havok;
using VRage;
using VRage.Library.Utils;

namespace Sandbox.Engine.Physics
{
	internal interface IPhysicsStepOptimizer
	{
		void EnableOptimizations(List<MyTuple<HkWorld, MyTimeSpan>> timings);

		void DisableOptimizations();

		void Unload();
	}
}
