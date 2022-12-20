using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.Entity;
using VRage.Game.VisualScripting.Utils;
using VRageMath;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_VariableScriptNode))]
	public class MyVisualSyntaxVariableNode : MyVisualSyntaxNode
	{
		private readonly Type m_variableType;

		public new MyObjectBuilder_VariableScriptNode ObjectBuilder => (MyObjectBuilder_VariableScriptNode)m_objectBuilder;

		internal override bool SequenceDependent => false;

		public UsingDirectiveSyntax Using { get; private set; }

		public MyVisualSyntaxVariableNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
			m_variableType = MyVisualScriptingProxy.GetType(ObjectBuilder.VariableType);
			Using = MySyntaxFactory.UsingStatementSyntax(m_variableType.Namespace);
		}

		public FieldDeclarationSyntax CreateFieldDeclaration()
		{
			return MySyntaxFactory.GenericFieldDeclaration(m_variableType, ObjectBuilder.VariableName);
		}

		public ExpressionStatementSyntax CreateInitializationSyntax()
		{
			if (m_variableType.IsGenericType)
			{
				ObjectCreationExpressionSyntax rightSide = MySyntaxFactory.GenericObjectCreation(m_variableType);
				return SyntaxFactory.ExpressionStatement(MySyntaxFactory.VariableAssignment(ObjectBuilder.VariableName, rightSide));
			}
			if (m_variableType == typeof(Vector3D))
			{
				return MySyntaxFactory.VectorAssignmentExpression(ObjectBuilder.VariableName, ObjectBuilder.VariableType, ObjectBuilder.Vector.X, ObjectBuilder.Vector.Y, ObjectBuilder.Vector.Z);
			}
			if (m_variableType == typeof(string))
			{
				return MySyntaxFactory.VariableAssignmentExpression(ObjectBuilder.VariableName, ObjectBuilder.VariableValue, SyntaxKind.StringLiteralExpression);
			}
			if (m_variableType == typeof(bool))
			{
				SyntaxKind expressionKind = ((MySyntaxFactory.NormalizeBool(ObjectBuilder.VariableValue) == "true") ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression);
				return MySyntaxFactory.VariableAssignmentExpression(ObjectBuilder.VariableName, ObjectBuilder.VariableValue, expressionKind);
			}
			if (m_variableType == typeof(MyEntity))
			{
				return MySyntaxFactory.VariableAssignmentExpression(ObjectBuilder.VariableName, ObjectBuilder.VariableValue, SyntaxKind.NullLiteralExpression);
			}
			return MySyntaxFactory.VariableAssignmentExpression(ObjectBuilder.VariableName, ObjectBuilder.VariableValue, SyntaxKind.NumericLiteralExpression);
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			string text = (output ? "out" : "") + ObjectBuilder.VariableName;
			if (!string.IsNullOrEmpty(variableIdentifier) && variableIdentifier != "Value")
			{
				text = ((!output) ? (text + "." + variableIdentifier) : (text + "_" + variableIdentifier));
			}
			return text;
		}
	}
}
