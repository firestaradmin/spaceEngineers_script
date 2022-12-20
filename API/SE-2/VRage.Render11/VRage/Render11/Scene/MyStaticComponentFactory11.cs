using System;
using VRage.Render.Scene.Components;

namespace VRage.Render11.Scene
{
	internal class MyStaticComponentFactory11 : IMyComponentFactory
	{
		public MyActorComponent Create(Type type)
		{
			return MyComponentFactory.Create(type);
		}

		public void Deallocate(MyActorComponent item)
		{
			MyComponentFactory.Deallocate(item);
		}
	}
}
