using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.VisualScripting.Utils;
using VRageMath;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_VariableSetterScriptNode))]
	public class MyVisualSyntaxSetterNode : MyVisualSyntaxNode
	{
		private string m_inputVariableName;

		private MyVisualSyntaxNode m_inputNode;

		internal override bool SequenceDependent => true;

		public new MyObjectBuilder_VariableSetterScriptNode ObjectBuilder => (MyObjectBuilder_VariableSetterScriptNode)m_objectBuilder;

		public MyVisualSyntaxSetterNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		private StatementSyntax GetCorrectAssignmentsExpression()
		{
			Type type = MyVisualScriptingProxy.GetType(base.Navigator.GetVariable(ObjectBuilder.VariableName).ObjectBuilder.VariableType);
			if (type == typeof(string))
			{
				if (ObjectBuilder.ValueInputID.NodeID == -1)
				{
					return MySyntaxFactory.VariableAssignmentExpression(ObjectBuilder.VariableName, ObjectBuilder.VariableValue, SyntaxKind.StringLiteralExpression);
				}
				InvocationExpressionSyntax rightSide = SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(m_inputVariableName), SyntaxFactory.IdentifierName("ToString")));
				return MySyntaxFactory.SimpleAssignment(ObjectBuilder.VariableName, rightSide);
			}
			if (type == typeof(Vector3D))
			{
				if (ObjectBuilder.ValueInputID.NodeID == -1)
				{
					return MySyntaxFactory.SimpleAssignment(ObjectBuilder.VariableName, MySyntaxFactory.NewVector3D(ObjectBuilder.VariableValue));
				}
			}
			else if (type == typeof(bool))
			{
				if (ObjectBuilder.ValueInputID.NodeID == -1)
				{
					SyntaxKind expressionKind = ((MySyntaxFactory.NormalizeBool(ObjectBuilder.VariableValue) == "true") ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);
					return MySyntaxFactory.VariableAssignmentExpression(ObjectBuilder.VariableName, ObjectBuilder.VariableValue, expressionKind);
				}
			}
			else if (ObjectBuilder.ValueInputID.NodeID == -1)
			{
				return MySyntaxFactory.VariableAssignmentExpression(ObjectBuilder.VariableName, ObjectBuilder.VariableValue, SyntaxKind.NumericLiteralExpression);
			}
			return MySyntaxFactory.SimpleAssignment(ObjectBuilder.VariableName, SyntaxFactory.IdentifierName(m_inputVariableName));
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			base.CollectInputExpressions(expressions, statementsToAppend);
			if (ObjectBuilder.ValueInputID.NodeID != -1)
			{
				m_inputVariableName = m_inputNode.VariableSyntaxName(ObjectBuilder.ValueInputID.VariableName);
			}
			expressions.Add(GetCorrectAssignmentsExpression());
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.SequenceOutputID != -1)
				{
					MyVisualSyntaxNode routedOutputNodeByID = base.Navigator.GetRoutedOutputNodeByID(ObjectBuilder.SequenceOutputID);
					SequenceOutputs.Add(routedOutputNodeByID);
				}
				foreach (int sequenceInput in ObjectBuilder.SequenceInputs)
				{
					if (sequenceInput != -1)
					{
						MyVisualSyntaxNode routedInputNodeByID = base.Navigator.GetRoutedInputNodeByID(sequenceInput);
						SequenceInputs.Add(routedInputNodeByID);
					}
				}
				if (ObjectBuilder.ValueInputID.NodeID != -1)
				{
					m_inputNode = base.Navigator.GetRoutedInputNodeByID(ObjectBuilder.ValueInputID.NodeID);
					Inputs.Add(m_inputNode);
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
