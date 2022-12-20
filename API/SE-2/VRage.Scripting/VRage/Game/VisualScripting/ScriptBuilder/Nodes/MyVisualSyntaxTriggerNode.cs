using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_TriggerScriptNode))]
	public class MyVisualSyntaxTriggerNode : MyVisualSyntaxNode
	{
		public new MyObjectBuilder_TriggerScriptNode ObjectBuilder => (MyObjectBuilder_TriggerScriptNode)m_objectBuilder;

		public MyVisualSyntaxTriggerNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < ObjectBuilder.InputNames.Count; i++)
			{
				list.Add(Inputs[i].VariableSyntaxName(ObjectBuilder.InputIDs[i].VariableName));
			}
			expressions.Add(SyntaxFactory.ExpressionStatement(MySyntaxFactory.MethodInvocation(ObjectBuilder.TriggerName, list)));
			base.CollectInputExpressions(expressions, statementsToAppend);
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			CollectInputExpressions(expressions, statementsToAppend);
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
				for (int i = 0; i < ObjectBuilder.InputNames.Count; i++)
				{
					if (ObjectBuilder.InputIDs[i].NodeID == -1)
					{
						throw new Exception("TriggerNode is missing an input of " + ObjectBuilder.InputNames[i] + " . NodeId: " + ObjectBuilder.ID);
					}
					MyVisualSyntaxNode routedInputNodeByID2 = base.Navigator.GetRoutedInputNodeByID(ObjectBuilder.InputIDs[i].NodeID);
					Inputs.Add(routedInputNodeByID2);
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
