using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.VisualScripting.Utils;
using VRage.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Output node that fills in the output values of a method.
	/// </summary>
	[MyVisualScriptTag(typeof(MyObjectBuilder_OutputScriptNode))]
	public class MyVisualSyntaxOutputNode : MyVisualSyntaxNode
	{
		private readonly List<MyVisualSyntaxNode> m_inputNodes = new List<MyVisualSyntaxNode>();

		public new MyObjectBuilder_OutputScriptNode ObjectBuilder => (MyObjectBuilder_OutputScriptNode)m_objectBuilder;

		public MyVisualSyntaxOutputNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		internal override void Reset()
		{
			base.Reset();
			m_inputNodes.Clear();
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			base.CollectInputExpressions(expressions, statementsToAppend);
			List<StatementSyntax> list = new List<StatementSyntax>(ObjectBuilder.Inputs.Count);
			for (int i = 0; i < ObjectBuilder.Inputs.Count; i++)
			{
				if (i < m_inputNodes.Count)
				{
					string name = m_inputNodes[i].VariableSyntaxName(ObjectBuilder.Inputs[i].Input.VariableName);
					ExpressionStatementSyntax item = MySyntaxFactory.SimpleAssignment(ObjectBuilder.Inputs[i].Name, SyntaxFactory.IdentifierName(name));
					list.Add(item);
				}
			}
			expressions.AddRange(list);
			statementsToAppend.Add(SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression)));
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
				foreach (MyInputParameterSerializationData input in ObjectBuilder.Inputs)
				{
					if (input.Input.NodeID != -1)
					{
						MyVisualSyntaxNode routedInputNodeByID2 = base.Navigator.GetRoutedInputNodeByID(input.Input.NodeID);
						m_inputNodes.Add(routedInputNodeByID2);
						Inputs.Add(routedInputNodeByID2);
					}
					else
					{
						MyLog.Default.Log(MyLogSeverity.Warning, "Output node missing input for " + input.Name + ". NodeID: " + ObjectBuilder.ID);
					}
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
