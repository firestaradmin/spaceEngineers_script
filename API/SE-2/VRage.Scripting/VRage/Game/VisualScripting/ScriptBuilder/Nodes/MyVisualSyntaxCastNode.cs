using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_CastScriptNode))]
	public class MyVisualSyntaxCastNode : MyVisualSyntaxNode
	{
		private MyVisualSyntaxNode m_nextSequenceNode;

		private MyVisualSyntaxNode m_inputNode;

		public new MyObjectBuilder_CastScriptNode ObjectBuilder => (MyObjectBuilder_CastScriptNode)m_objectBuilder;

		public MyVisualSyntaxCastNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			base.CollectInputExpressions(expressions, statementsToAppend);
			expressions.Add(MySyntaxFactory.CastExpression(m_inputNode.VariableSyntaxName(ObjectBuilder.InputID.VariableName), ObjectBuilder.Type, VariableSyntaxName()));
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			return (output ? "out" : "") + "castResult_" + ObjectBuilder.ID;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.SequenceOuputID != -1)
				{
					m_nextSequenceNode = base.Navigator.GetRoutedOutputNodeByID(ObjectBuilder.SequenceOuputID);
					SequenceOutputs.Add(m_nextSequenceNode);
				}
				foreach (int sequenceInput in ObjectBuilder.SequenceInputs)
				{
					if (sequenceInput != -1)
					{
						MyVisualSyntaxNode routedInputNodeByID = base.Navigator.GetRoutedInputNodeByID(sequenceInput);
						SequenceInputs.Add(routedInputNodeByID);
					}
				}
				if (ObjectBuilder.InputID.NodeID == -1)
				{
					throw new Exception("Cast node has no input. NodeId: " + ObjectBuilder.ID);
				}
				m_inputNode = base.Navigator.GetRoutedInputNodeByID(ObjectBuilder.InputID.NodeID);
				Inputs.Add(m_inputNode);
			}
			base.Preprocess(currentDepth);
		}
	}
}
