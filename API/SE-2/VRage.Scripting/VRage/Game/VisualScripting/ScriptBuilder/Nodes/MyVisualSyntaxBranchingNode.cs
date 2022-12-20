using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_BranchingScriptNode))]
	public class MyVisualSyntaxBranchingNode : MyVisualSyntaxNode
	{
		private MyVisualSyntaxNode m_nextTrueSequenceNode;

		private MyVisualSyntaxNode m_nextFalseSequenceNode;

		private MyVisualSyntaxNode m_comparerNode;

		public new MyObjectBuilder_BranchingScriptNode ObjectBuilder => (MyObjectBuilder_BranchingScriptNode)m_objectBuilder;

		public MyVisualSyntaxBranchingNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			CollectInputExpressions(expressions, statementsToAppend);
			List<StatementSyntax> list = new List<StatementSyntax>();
			List<StatementSyntax> list2 = new List<StatementSyntax>();
			if (m_nextTrueSequenceNode != null)
			{
				m_nextTrueSequenceNode.CollectSequenceExpressions(list, list);
			}
			if (m_nextFalseSequenceNode != null)
			{
				m_nextFalseSequenceNode.CollectSequenceExpressions(list2, list2);
			}
			string conditionVariableName = m_comparerNode.VariableSyntaxName(ObjectBuilder.InputID.VariableName);
			expressions.Add(MySyntaxFactory.IfExpressionSyntax(conditionVariableName, list, list2));
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			return (output ? "out" : "") + "branchingNode_" + ObjectBuilder.ID;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.SequenceTrueOutputID != -1)
				{
					m_nextTrueSequenceNode = base.Navigator.GetRoutedOutputNodeByID(ObjectBuilder.SequenceTrueOutputID);
					SequenceOutputs.Add(m_nextTrueSequenceNode);
				}
				if (ObjectBuilder.SequenceFalseOutputID != -1)
				{
					m_nextFalseSequenceNode = base.Navigator.GetRoutedOutputNodeByID(ObjectBuilder.SequenceFalseOutputID);
					SequenceOutputs.Add(m_nextFalseSequenceNode);
				}
				if (ObjectBuilder.SequenceInputID != -1)
				{
					MyVisualSyntaxNode routedInputNodeByID = base.Navigator.GetRoutedInputNodeByID(ObjectBuilder.SequenceInputID);
					SequenceInputs.Add(routedInputNodeByID);
				}
				if (ObjectBuilder.InputID.NodeID == -1)
				{
					throw new Exception("Branching node has no comparer input. NodeID: " + ObjectBuilder.ID);
				}
				m_comparerNode = base.Navigator.GetRoutedInputNodeByID(ObjectBuilder.InputID.NodeID);
				Inputs.Add(m_comparerNode);
			}
			base.Preprocess(currentDepth);
		}
	}
}
