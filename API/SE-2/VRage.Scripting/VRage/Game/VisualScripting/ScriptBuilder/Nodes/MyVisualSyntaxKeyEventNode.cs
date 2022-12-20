using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Special case of Event node that also generates some syntax.
	/// Creates a simple if statement that filters the input to this node.
	/// </summary>
	[MyVisualScriptTag(typeof(MyObjectBuilder_KeyEventScriptNode))]
	public class MyVisualSyntaxKeyEventNode : MyVisualSyntaxEventNode
	{
		public new MyObjectBuilder_KeyEventScriptNode ObjectBuilder => (MyObjectBuilder_KeyEventScriptNode)m_objectBuilder;

		public MyVisualSyntaxKeyEventNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob, scriptBaseType)
		{
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			if (ObjectBuilder.SequenceOutputID == -1)
			{
				return;
			}
			MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(ObjectBuilder.SequenceOutputID);
			List<StatementSyntax> list = new List<StatementSyntax>();
			nodeByID.CollectSequenceExpressions(list, statementsToAppend);
			List<int> list2 = new List<int>();
			ParameterInfo[] parameters = m_fieldInfo.FieldType.GetMethod("Invoke").GetParameters();
<<<<<<< HEAD
			VisualScriptingEvent customAttribute = CustomAttributeExtensions.GetCustomAttribute<VisualScriptingEvent>(m_fieldInfo.FieldType);
=======
			VisualScriptingEvent customAttribute = m_fieldInfo.FieldType.GetCustomAttribute<VisualScriptingEvent>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			for (int i = 0; i < parameters.Length && i < customAttribute.IsKey.Length; i++)
			{
				if (customAttribute.IsKey[i] && i < ObjectBuilder.OuputTypes.Count)
				{
					list2.Add(i);
				}
			}
			IfStatementSyntax item = MySyntaxFactory.IfExpressionSyntax(CreateAndClauses(list2.Count - 1, list2), list);
			expressions.Add(item);
		}

		private ExpressionSyntax CreateAndClauses(int index, List<int> keyIndexes)
		{
			while (index > 0 && ObjectBuilder.OuputTypes[keyIndexes[index]] == "System.String" && string.IsNullOrEmpty(ObjectBuilder.Keys[keyIndexes[index]]))
			{
				index--;
			}
			LiteralExpressionSyntax right = MySyntaxFactory.Literal(ObjectBuilder.OuputTypes[keyIndexes[index]], ObjectBuilder.Keys[keyIndexes[index]]);
			if (index == 0)
			{
				return SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, SyntaxFactory.IdentifierName(ObjectBuilder.OutputNames[keyIndexes[index]]), right);
			}
			BinaryExpressionSyntax right2 = SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, SyntaxFactory.IdentifierName(ObjectBuilder.OutputNames[keyIndexes[index]]), right);
			return SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, CreateAndClauses(--index, keyIndexes), right2);
		}
	}
}
