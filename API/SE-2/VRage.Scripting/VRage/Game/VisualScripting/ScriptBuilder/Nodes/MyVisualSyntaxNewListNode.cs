using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_NewListScriptNode))]
	public class MyVisualSyntaxNewListNode : MyVisualSyntaxNode
	{
		public new MyObjectBuilder_NewListScriptNode ObjectBuilder => (MyObjectBuilder_NewListScriptNode)m_objectBuilder;

		internal override bool SequenceDependent => false;

		public MyVisualSyntaxNewListNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			return (output ? "out" : "") + "newListNode_" + ObjectBuilder.ID;
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			base.CollectInputExpressions(expressions, statementsToAppend);
			Type type = MyVisualScriptingProxy.GetType(ObjectBuilder.Type);
			Type type2 = typeof(List<>).MakeGenericType(type);
			List<SyntaxNodeOrToken> list = new List<SyntaxNodeOrToken>();
			for (int i = 0; i < ObjectBuilder.DefaultEntries.Count; i++)
			{
				string val = ObjectBuilder.DefaultEntries[i];
				LiteralExpressionSyntax literalExpressionSyntax = MySyntaxFactory.Literal(ObjectBuilder.Type, val);
				list.Add(literalExpressionSyntax);
				if (i < ObjectBuilder.DefaultEntries.Count - 1)
				{
					list.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
				}
			}
			ArrayCreationExpressionSyntax arrayCreationExpressionSyntax = null;
			if (list.Count > 0)
			{
				arrayCreationExpressionSyntax = SyntaxFactory.ArrayCreationExpression(SyntaxFactory.ArrayType(SyntaxFactory.IdentifierName(ObjectBuilder.Type), SyntaxFactory.SingletonList(SyntaxFactory.ArrayRankSpecifier(SyntaxFactory.SingletonSeparatedList((ExpressionSyntax)SyntaxFactory.OmittedArraySizeExpression())))), SyntaxFactory.InitializerExpression(SyntaxKind.ArrayInitializerExpression, SyntaxFactory.SeparatedList<ExpressionSyntax>(list)));
			}
			ObjectCreationExpressionSyntax initializerExpressionSyntax = MySyntaxFactory.GenericObjectCreation(type2, (arrayCreationExpressionSyntax == null) ? null : new ArrayCreationExpressionSyntax[1] { arrayCreationExpressionSyntax });
			LocalDeclarationStatementSyntax item = MySyntaxFactory.LocalVariable(type2, VariableSyntaxName(), initializerExpressionSyntax);
			expressions.Add(item);
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				foreach (MyVariableIdentifier connection in ObjectBuilder.Connections)
				{
					TryRegisterOutputNodes(connection.NodeID, Outputs);
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
