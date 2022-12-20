using System;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Special case of Event node. The logic is the same for both,
	/// but on the gui side the logic is different. Thats why I kept
	/// it separated also here.
	/// </summary>
	[MyVisualScriptTag(typeof(MyObjectBuilder_InputScriptNode))]
	public class MyVisualSyntaxInputNode : MyVisualSyntaxEventNode
	{
		public MyVisualSyntaxInputNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob, scriptBaseType)
		{
		}
	}
}
