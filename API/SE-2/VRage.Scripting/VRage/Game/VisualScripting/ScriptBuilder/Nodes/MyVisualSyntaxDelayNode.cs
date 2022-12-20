using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Sequence dependent node that creates syntax for timed delay
	/// </summary>
	[MyVisualScriptTag(typeof(MyObjectBuilder_DelayScriptNode))]
	public class MyVisualSyntaxDelayNode : MyVisualSyntaxNode, IMyVisualSyntaxEntryPoint
	{
		protected static readonly FieldInfo m_fieldInfo;

		public static List<string> FieldInfoOutputNames;

		public static List<string> FieldInfoOutputTypes;

		private MyVisualSyntaxNode m_durationInput;

		private bool m_eventHandlerProcessing = true;

		public static FieldInfo FieldInfo => m_fieldInfo;

		public new MyObjectBuilder_DelayScriptNode ObjectBuilder => (MyObjectBuilder_DelayScriptNode)m_objectBuilder;

		static MyVisualSyntaxDelayNode()
		{
			FieldInfoOutputNames = new List<string>();
			FieldInfoOutputTypes = new List<string>();
			m_fieldInfo = MyVisualScriptingProxy.GetField("VRage.Game.VisualScripting.MyVisualScriptLogicProvider.MissionUpdate");
			ParameterInfo[] parameters = m_fieldInfo.FieldType.GetMethod("Invoke").GetParameters();
			foreach (ParameterInfo parameterInfo in parameters)
			{
				Type parameterType = parameterInfo.ParameterType;
				FieldInfoOutputNames.Add(parameterInfo.Name);
				FieldInfoOutputTypes.Add(parameterType.Signature());
			}
		}

		public MyVisualSyntaxDelayNode(MyObjectBuilder_DelayScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			return (output ? "out" : "") + "delay_" + ObjectBuilder.ID + "_duration";
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			if (m_eventHandlerProcessing)
			{
				BinaryExpressionSyntax condition = SyntaxFactory.BinaryExpression(SyntaxKind.GreaterThanExpression, SyntaxFactory.IdentifierName(GetCounterName()), SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("MyTimeSpan"), SyntaxFactory.IdentifierName("Zero")));
				ExpressionStatementSyntax item = SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SubtractAssignmentExpression, SyntaxFactory.IdentifierName(GetCounterName()), SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName("MyTimeSpan.FromMilliseconds")).WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(new SyntaxNodeOrToken[1] { SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.ParseToken("16.6666f"))) })))));
				List<StatementSyntax> list = new List<StatementSyntax>();
				list.Add(GetCounterInitializer());
				base.CollectSequenceExpressions(list, statementsToAppend);
				IfStatementSyntax item2 = MySyntaxFactory.IfExpressionSyntax(SyntaxFactory.BinaryExpression(SyntaxKind.LessThanOrEqualExpression, SyntaxFactory.IdentifierName(GetCounterName()), SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("MyTimeSpan"), SyntaxFactory.IdentifierName("Zero"))), list);
				IfStatementSyntax item3 = MySyntaxFactory.IfExpressionSyntax(condition, new List<StatementSyntax> { item, item2 });
				expressions.Add(item3);
			}
			else
			{
				ExpressionSyntax expression = ((m_durationInput == null) ? ((ExpressionSyntax)MySyntaxFactory.Literal(typeof(float).Signature(), ObjectBuilder.Duration)) : ((ExpressionSyntax)SyntaxFactory.IdentifierName(m_durationInput.VariableSyntaxName(ObjectBuilder.DurationInput.VariableName))));
				ExpressionStatementSyntax item4 = SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName(GetCounterName()), SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName("MyTimeSpan.FromSeconds")).WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(new SyntaxNodeOrToken[1] { SyntaxFactory.Argument(expression) })))));
				expressions.Add(item4);
			}
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed && m_eventHandlerProcessing)
			{
				TryRegisterInputNodes(ObjectBuilder.SequenceInput, SequenceInputs);
				m_durationInput = TryRegisterInputNodes(ObjectBuilder.DurationInput.NodeID, Inputs);
				if (ObjectBuilder.CompletedOutput != -1)
				{
					MyVisualSyntaxNode routedOutputNodeByID = base.Navigator.GetRoutedOutputNodeByID(ObjectBuilder.CompletedOutput);
					SequenceOutputs.Add(routedOutputNodeByID);
				}
			}
			base.Preprocess(currentDepth);
		}

		public void AddSequenceInput(MyVisualSyntaxNode node)
		{
			SequenceInputs.Add(node);
			m_eventHandlerProcessing = true;
		}

		internal override void Reset()
		{
			base.Reset();
			m_eventHandlerProcessing = false;
		}

		internal string GetInstanceName()
		{
			return "DelayNode" + ObjectBuilder.ID;
		}

		internal string GetCounterName()
		{
			return GetInstanceName() + "_Counter";
		}

		internal StatementSyntax GetCounterInitializer()
		{
			return SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName(GetCounterName()), SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName("MyTimeSpan"), SyntaxFactory.IdentifierName("Zero"))));
		}
	}
}
