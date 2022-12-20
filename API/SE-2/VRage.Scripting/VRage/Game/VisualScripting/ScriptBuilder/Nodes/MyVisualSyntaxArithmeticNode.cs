using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_ArithmeticScriptNode))]
	public class MyVisualSyntaxArithmeticNode : MyVisualSyntaxNode
	{
		private MyVisualSyntaxNode m_inputANode;

		private MyVisualSyntaxNode m_inputBNode;

		public new MyObjectBuilder_ArithmeticScriptNode ObjectBuilder => (MyObjectBuilder_ArithmeticScriptNode)m_objectBuilder;

		internal override bool SequenceDependent => false;

		public MyVisualSyntaxArithmeticNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			base.DebugCollected = true;
			base.CollectInputExpressions(expressions, statementsToAppend);
			string text = null;
			string text2 = null;
			text = ((m_inputANode != null) ? m_inputANode.VariableSyntaxName(ObjectBuilder.InputAID.VariableName) : null);
			text2 = ((m_inputBNode != null) ? m_inputBNode.VariableSyntaxName(ObjectBuilder.InputBID.VariableName) : null);
			if (ObjectBuilder.Operation == "==" || ObjectBuilder.Operation == "!=")
			{
				if (m_inputANode == null)
				{
					text = "null";
				}
				if (m_inputBNode == null)
				{
					text2 = "null";
				}
			}
			else
			{
				if (m_inputANode == null)
				{
					text = "0";
				}
				if (m_inputBNode == null)
				{
					text2 = "0";
				}
			}
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
			{
				expressions.Add(MySyntaxFactory.ArithmeticStatement(VariableSyntaxName(), text, text2, ObjectBuilder.Operation));
				expressions.Add(MySyntaxFactory.LogNodeSyntax(m_objectBuilder.ID, text, text2, VariableSyntaxName()));
			}
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			return (output ? "out" : "") + "arithmeticResult_" + m_objectBuilder.ID;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.InputAID.NodeID != -1)
				{
					m_inputANode = base.Navigator.GetRoutedInputNodeByID(ObjectBuilder.InputAID.NodeID);
				}
				if (ObjectBuilder.InputBID.NodeID != -1)
				{
					m_inputBNode = base.Navigator.GetRoutedInputNodeByID(ObjectBuilder.InputBID.NodeID);
				}
				for (int i = 0; i < ObjectBuilder.OutputNodeIDs.Count; i++)
				{
					if (ObjectBuilder.OutputNodeIDs[i].NodeID == -1)
					{
						throw new Exception("-1 output in arithmetic node: " + ObjectBuilder.ID);
					}
					foreach (MyVisualSyntaxNode item in base.Navigator.GetRoutedOutputNodesByID(ObjectBuilder.OutputNodeIDs[i].NodeID))
					{
						Outputs.Add(item);
					}
				}
				if (m_inputANode != null)
				{
					Inputs.Add(m_inputANode);
				}
				if (m_inputBNode != null)
				{
					Inputs.Add(m_inputBNode);
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
