using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.ObjectBuilders.VisualScripting;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Sequence dependent node that creates syntax for ForEach
	/// </summary>
	[MyVisualScriptTag(typeof(MyObjectBuilder_ForEachScriptNode))]
	public class MyVisualSyntaxForEachNode : MyVisualSyntaxNode
	{
<<<<<<< HEAD
=======
		private MyVisualSyntaxNode m_inputSequence;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MyVisualSyntaxNode m_bodySequence;

		private MyVisualSyntaxNode m_collectionInput;

		private readonly List<MyVisualSyntaxNode> m_itemOutputs = new List<MyVisualSyntaxNode>();

		public new MyObjectBuilder_ForEachScriptNode ObjectBuilder => (MyObjectBuilder_ForEachScriptNode)m_objectBuilder;

		public MyVisualSyntaxForEachNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			return (output ? "out" : "") + "forEach_" + ObjectBuilder.ID;
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
<<<<<<< HEAD
			m_itemOutputs.Clear();
			foreach (MyVisualSyntaxNode subTreeNode in SubTreeNodes)
			{
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
					m_itemOutputs.Add(subTreeNode);
				}
=======
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			m_itemOutputs.Clear();
			Enumerator<MyVisualSyntaxNode> enumerator = SubTreeNodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyVisualSyntaxNode current = enumerator.get_Current();
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
						m_itemOutputs.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (m_bodySequence == null)
			{
				return;
			}
			List<StatementSyntax> list = new List<StatementSyntax>();
			foreach (MyVisualSyntaxNode itemOutput in m_itemOutputs)
			{
				itemOutput.CollectInputExpressions(list, statementsToAppend);
			}
			m_bodySequence.CollectSequenceExpressions(list, statementsToAppend);
			ExpressionSyntax expression = null;
			if (m_collectionInput != null)
			{
				expression = SyntaxFactory.IdentifierName(m_collectionInput.VariableSyntaxName(ObjectBuilder.CollectionValueInput.VariableName));
			}
			ForEachStatementSyntax item = SyntaxFactory.ForEachStatement(SyntaxFactory.IdentifierName("var"), VariableSyntaxName(), expression, SyntaxFactory.Block(list));
			expressions.Add(item);
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
					m_bodySequence = base.Navigator.GetRoutedOutputNodeByID(ObjectBuilder.SequenceOutput);
					if (m_bodySequence != null)
					{
						SequenceOutputs.Add(m_bodySequence);
					}
				}
				m_collectionInput = TryRegisterInputNodes(ObjectBuilder.CollectionValueInput.NodeID, Inputs);
			}
			base.Preprocess(currentDepth);
		}
	}
}
