using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_LocalizationScriptNode))]
	public class MyVisualSyntaxLocalizationNode : MyVisualSyntaxNode
	{
		private readonly List<MyVisualSyntaxNode> m_inputParameterNodes = new List<MyVisualSyntaxNode>();

		public new MyObjectBuilder_LocalizationScriptNode ObjectBuilder => (MyObjectBuilder_LocalizationScriptNode)m_objectBuilder;

		internal override bool SequenceDependent => false;

		public MyVisualSyntaxLocalizationNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		internal override void Reset()
		{
			base.Reset();
			m_inputParameterNodes.Clear();
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			return (output ? "out" : "") + "localizationNode_" + ObjectBuilder.ID;
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			base.CollectInputExpressions(expressions, statementsToAppend);
			ElementAccessExpressionSyntax expression = SyntaxFactory.ElementAccessExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("VRage.Game.Localization.MyLocalization"), SyntaxFactory.IdentifierName("Static"))).WithArgumentList(SyntaxFactory.BracketedArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(new SyntaxNodeOrToken[3]
			{
				SyntaxFactory.Argument(MySyntaxFactory.Literal(typeof(string).Signature(), ObjectBuilder.Context)),
				SyntaxFactory.Token(SyntaxKind.CommaToken),
				SyntaxFactory.Argument(MySyntaxFactory.Literal(typeof(string).Signature(), ObjectBuilder.MessageId))
			})));
			InvocationExpressionSyntax initializer = SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, SyntaxFactory.IdentifierName("ToString")));
			string text = "localizedText_" + ObjectBuilder.ID;
			if (m_inputParameterNodes.Count == 0)
			{
				text = VariableSyntaxName();
			}
			LocalDeclarationStatementSyntax item = MySyntaxFactory.LocalVariable(typeof(string).Signature(), text, initializer);
			if (m_inputParameterNodes.Count > 0)
			{
				List<string> list = new List<string>();
				list.Add(text);
				for (int i = 0; i < m_inputParameterNodes.Count; i++)
				{
					MyVisualSyntaxNode myVisualSyntaxNode = m_inputParameterNodes[i];
					list.Add(myVisualSyntaxNode.VariableSyntaxName(ObjectBuilder.ParameterInputs[i].VariableName));
				}
				InvocationExpressionSyntax initializer2 = MySyntaxFactory.MethodInvocation("Format", list, "string");
				LocalDeclarationStatementSyntax item2 = MySyntaxFactory.LocalVariable(typeof(string).Signature(), VariableSyntaxName(), initializer2);
				expressions.Add(item);
				expressions.Add(item2);
			}
			else
			{
				expressions.Add(item);
			}
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				foreach (MyVariableIdentifier valueOutput in ObjectBuilder.ValueOutputs)
				{
					TryRegisterOutputNodes(valueOutput.NodeID, Outputs);
				}
				foreach (MyVariableIdentifier parameterInput in ObjectBuilder.ParameterInputs)
				{
					MyVisualSyntaxNode myVisualSyntaxNode = TryRegisterInputNodes(parameterInput.NodeID, Inputs);
					if (myVisualSyntaxNode != null)
					{
						m_inputParameterNodes.Add(myVisualSyntaxNode);
					}
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
