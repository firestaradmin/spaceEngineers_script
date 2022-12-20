using VRage.Render.Scene;
using VRage.Render.Scene.Components;
using VRageRender;

namespace VRage.Render11.Scene
{
	internal class MyStaticActorFactory11 : IMyActorFactory
	{
		public MyLightComponent CreateLight(string debugName)
		{
			MyActor myActor = MyActorFactory.CreateLight(debugName);
			myActor.SetID(MyRenderProxy.AllocateObjectId(MyRenderProxy.ObjectType.Light));
			return myActor.GetLight();
		}

		public void Destroy(MyActor actor, bool fadeOut)
		{
			uint iD = actor.ID;
			MyRenderProxy.ObjectType objectType = MyRenderProxy.GetObjectType(iD);
			actor.Destroy(fadeOut);
			if (objectType != MyRenderProxy.ObjectType.Invalid)
			{
				MyRenderProxy.RemoveMessageId(iD, objectType);
			}
		}
	}
}
