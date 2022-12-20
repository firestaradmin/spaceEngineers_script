using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyEntityType(typeof(MyObjectBuilder_Waypoint), true)]
	public class MyWaypoint : MyEntity, IMyEventProxy, IMyEventOwner
	{
		private class Sandbox_Game_Entities_MyWaypoint_003C_003EActor : IActivator, IActivator<MyWaypoint>
		{
			private sealed override object CreateInstance()
			{
				return new MyWaypoint();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWaypoint CreateInstance()
			{
				return new MyWaypoint();
			}

			MyWaypoint IActivator<MyWaypoint>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool Visible { get; set; }

		public bool Freeze { get; set; }

		public string Path { get; set; }

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_Waypoint myObjectBuilder_Waypoint = (MyObjectBuilder_Waypoint)objectBuilder;
			Visible = myObjectBuilder_Waypoint.Visible;
			Freeze = myObjectBuilder_Waypoint.Freeze;
			Path = myObjectBuilder_Waypoint.Path;
			float num = 0.3f;
			base.PositionComp.LocalAABB = new BoundingBox(new Vector3(0f - num), new Vector3(num));
			AddDebugRenderComponent(new MyDebugRenderComponentWaypoint(this));
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_Waypoint obj = (MyObjectBuilder_Waypoint)base.GetObjectBuilder(copy);
			obj.Path = Path;
			obj.Visible = Visible;
			obj.Freeze = Freeze;
			return obj;
		}
	}
}
