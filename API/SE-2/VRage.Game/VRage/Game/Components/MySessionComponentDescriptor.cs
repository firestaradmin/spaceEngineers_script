using System;
using VRage.ObjectBuilders;

namespace VRage.Game.Components
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class MySessionComponentDescriptor : Attribute
	{
		public MyUpdateOrder UpdateOrder;

		/// <summary>
		/// Lower Priority is loaded before higher Priority
		/// </summary>
		public int Priority;

		public MyObjectBuilderType ObjectBuilderType;

		public Type ComponentType;

		/// <summary>
		/// Is server only is used for client request of the world. if the component is server only, it's not sent to the client on world request.
		/// </summary>
		public bool IsServerOnly { get; private set; }

		public MySessionComponentDescriptor(MyUpdateOrder updateOrder)
			: this(updateOrder, 1000)
		{
		}

		public MySessionComponentDescriptor(MyUpdateOrder updateOrder, int priority)
			: this(updateOrder, priority, null)
		{
		}

		public MySessionComponentDescriptor(MyUpdateOrder updateOrder, int priority, Type obType, Type registrationType = null, bool serverOnly = false)
		{
			UpdateOrder = updateOrder;
			Priority = priority;
			ObjectBuilderType = obType;
			IsServerOnly = serverOnly;
			if (obType != null && !typeof(MyObjectBuilder_SessionComponent).IsAssignableFrom(obType))
			{
				ObjectBuilderType = MyObjectBuilderType.Invalid;
			}
			ComponentType = registrationType;
		}
	}
}
