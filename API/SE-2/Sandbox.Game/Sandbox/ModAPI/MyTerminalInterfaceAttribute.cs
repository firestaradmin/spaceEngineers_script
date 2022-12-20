using System;

namespace Sandbox.ModAPI
{
	/// <inheritdoc />
	/// <summary>
	/// Used on block classes inheriting MyTerminalBlock.
	/// Attribute tells the terminal system which interface types
	/// should be linked to this class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class MyTerminalInterfaceAttribute : Attribute
	{
		public readonly Type[] LinkedTypes;

		public MyTerminalInterfaceAttribute(params Type[] linkedTypes)
		{
			LinkedTypes = linkedTypes;
		}
	}
}
