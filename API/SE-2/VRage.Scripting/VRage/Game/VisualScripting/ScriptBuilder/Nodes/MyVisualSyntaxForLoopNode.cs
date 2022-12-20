using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Sequence dependent node that creates syntax for For loops
	/// with support for custom initial index, increment and last index.
	/// </summary>
	[MyVisualScriptTag(typeof(MyObjectBuilder_ForLoopScriptNode))]
	public class MyVisualSyntaxForLoopNode : MyVisualSyntaxNode
	{
		private MyVisualSyntaxNode m_bodySequence;

		private MyVisualSyntaxNode m_finishSequence;

		private MyVisualSyntaxNode m_firstInput;

		private MyVisualSyntaxNode m_lastInput;

		private MyVisualSyntaxNode m_incrementInput;

		private MyVisualSyntaxNode m_conditionInput;

		private readonly List<MyVisualSyntaxNode> m_toCollectNodeCache = new List<MyVisualSyntaxNode>();

		public new MyObjectBuilder_ForLoopScriptNode ObjectBuilder => (MyObjectBuilder_ForLoopScriptNode)m_objectBuilder;

		public MyVisualSyntaxForLoopNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			return (output ? "out" : "") + "forLoop_" + ObjectBuilder.ID + "_counter";
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
<<<<<<< HEAD
			m_toCollectNodeCache.Clear();
			foreach (MyVisualSyntaxNode subTreeNode in SubTreeNodes)
			{
				if (subTreeNode == m_conditionInput)
				{
					break;
				}
				bool flag = false;
				foreach (MyVisualSyntaxNode output in subTreeNode.Outputs)
				{
					if (output == this && !output.Collected)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					subTreeNode.CollectInputExpressions(expressions, statementsToAppend);
				}
				else
				{
					m_toCollectNodeCache.Add(subTreeNode);
				}
=======
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			m_toCollectNodeCache.Clear();
			Enumerator<MyVisualSyntaxNode> enumerator = SubTreeNodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyVisualSyntaxNode current = enumerator.get_Current();
					if (current == m_conditionInput)
					{
						break;
					}
					bool flag = false;
					foreach (MyVisualSyntaxNode output in current.Outputs)
					{
						if (output == this && !output.Collected)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						current.CollectInputExpressions(expressions, statementsToAppend);
					}
					else
					{
						m_toCollectNodeCache.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (m_bodySequence != null)
			{
				List<StatementSyntax> list = new List<StatementSyntax>();
				if (m_conditionInput != null)
				{
					m_conditionInput.CollectInputExpressions(list, statementsToAppend);
					IfStatementSyntax item = SyntaxFactory.IfStatement(SyntaxFactory.IdentifierName(m_conditionInput.VariableSyntaxName(ObjectBuilder.ConditionValueInput.VariableName)), SyntaxFactory.BreakStatement());
					list.Add(item);
				}
				foreach (MyVisualSyntaxNode item3 in m_toCollectNodeCache)
				{
					item3.CollectInputExpressions(list, statementsToAppend);
				}
				m_bodySequence.CollectSequenceExpressions(list, statementsToAppend);
				ExpressionSyntax value = ((m_firstInput == null) ? ((ExpressionSyntax)MySyntaxFactory.Literal(typeof(int).Signature(), ObjectBuilder.FirstIndexValue)) : ((ExpressionSyntax)SyntaxFactory.IdentifierName(m_firstInput.VariableSyntaxName(ObjectBuilder.FirstIndexValueInput.VariableName))));
				ExpressionSyntax right = ((m_lastInput == null) ? ((ExpressionSyntax)MySyntaxFactory.Literal(typeof(int).Signature(), ObjectBuilder.LastIndexValue)) : ((ExpressionSyntax)SyntaxFactory.IdentifierName(m_lastInput.VariableSyntaxName(ObjectBuilder.LastIndexValueInput.VariableName))));
				ExpressionSyntax right2 = ((m_incrementInput == null) ? ((ExpressionSyntax)MySyntaxFactory.Literal(typeof(int).Signature(), ObjectBuilder.IncrementValue)) : ((ExpressionSyntax)SyntaxFactory.IdentifierName(m_incrementInput.VariableSyntaxName(ObjectBuilder.IncrementValueInput.VariableName))));
				ForStatementSyntax item2 = SyntaxFactory.ForStatement(SyntaxFactory.Block(list)).WithDeclaration(SyntaxFactory.VariableDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword))).WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(VariableSyntaxName())).WithInitializer(SyntaxFactory.EqualsValueClause(value))))).WithCondition(SyntaxFactory.BinaryExpression(SyntaxKind.LessThanOrEqualExpression, SyntaxFactory.IdentifierName(VariableSyntaxName()), right))
					.WithIncrementors(SyntaxFactory.SingletonSeparatedList((ExpressionSyntax)SyntaxFactory.AssignmentExpression(SyntaxKind.AddAssignmentExpression, SyntaxFactory.IdentifierName(VariableSyntaxName()), right2)));
				expressions.Add(item2);
			}
			if (m_finishSequence != null)
			{
				m_finishSequence.CollectSequenceExpressions(expressions, statementsToAppend);
			}
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				foreach (int sequenceInput in ObjectBuilder.SequenceInputs)
				{
					TryRegisterInputNodes(sequenceInput, SequenceInputs);
				}
				if (ObjectBuilder.SequenceOutput != -1)
				{
					m_finishSequence = base.Navigator.GetRoutedOutputNodeByID(ObjectBuilder.SequenceOutput);
					if (m_finishSequence != null)
					{
						SequenceOutputs.Add(m_finishSequence);
					}
				}
				if (ObjectBuilder.SequenceBody != -1)
				{
					m_bodySequence = base.Navigator.GetRoutedOutputNodeByID(ObjectBuilder.SequenceBody);
					if (m_bodySequence != null)
					{
						SequenceOutputs.Add(m_bodySequence);
					}
				}
				foreach (MyVariableIdentifier counterValueOutput in ObjectBuilder.CounterValueOutputs)
				{
					TryRegisterOutputNodes(counterValueOutput.NodeID, Outputs);
				}
				m_firstInput = TryRegisterInputNodes(ObjectBuilder.FirstIndexValueInput.NodeID, Inputs);
				m_lastInput = TryRegisterInputNodes(ObjectBuilder.LastIndexValueInput.NodeID, Inputs);
				m_incrementInput = TryRegisterInputNodes(ObjectBuilder.IncrementValueInput.NodeID, Inputs);
				m_conditionInput = TryRegisterInputNodes(ObjectBuilder.ConditionValueInput.NodeID, Inputs);
			}
			base.Preprocess(currentDepth);
		}
	}
}
