using System;
using VRage.Game.ObjectBuilders.VisualScripting;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_SequenceScriptNode))]
	public class MyVisualSyntaxSequenceNode : MyVisualSyntaxNode
	{
		public new MyObjectBuilder_SequenceScriptNode ObjectBuilder => (MyObjectBuilder_SequenceScriptNode)m_objectBuilder;

		public MyVisualSyntaxSequenceNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				foreach (int sequenceInput in ObjectBuilder.SequenceInputs)
				{
					if (sequenceInput != -1)
					{
						MyVisualSyntaxNode routedInputNodeByID = base.Navigator.GetRoutedInputNodeByID(sequenceInput);
						SequenceInputs.Add(routedInputNodeByID);
					}
				}
				foreach (int sequenceOutput in ObjectBuilder.SequenceOutputs)
				{
					if (sequenceOutput != -1)
					{
						MyVisualSyntaxNode routedOutputNodeByID = base.Navigator.GetRoutedOutputNodeByID(sequenceOutput);
						SequenceOutputs.Add(routedOutputNodeByID);
					}
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
