using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_SwitchScriptNode))]
	public class MyVisualSyntaxSwitchNode : MyVisualSyntaxNode
	{
		private readonly List<MyVisualSyntaxNode> m_sequenceOutputs = new List<MyVisualSyntaxNode>();

		private MyVisualSyntaxNode m_valueInput;

		public new MyObjectBuilder_SwitchScriptNode ObjectBuilder => (MyObjectBuilder_SwitchScriptNode)m_objectBuilder;

		public MyVisualSyntaxSwitchNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			CollectInputExpressions(expressions, statementsToAppend);
			if (m_valueInput == null)
			{
				return;
			}
			string name = m_valueInput.VariableSyntaxName(ObjectBuilder.ValueInput.VariableName);
			statementsToAppend = null;
			List<SwitchSectionSyntax> list = new List<SwitchSectionSyntax>();
			List<StatementSyntax> list2 = new List<StatementSyntax>();
			for (int i = 0; i < m_sequenceOutputs.Count; i++)
			{
				MyVisualSyntaxNode myVisualSyntaxNode = m_sequenceOutputs[i];
				if (myVisualSyntaxNode != null)
				{
					list2.Clear();
					myVisualSyntaxNode.CollectSequenceExpressions(list2, statementsToAppend);
					list2.Add(SyntaxFactory.BreakStatement());
					SwitchSectionSyntax item = SyntaxFactory.SwitchSection().WithLabels(SyntaxFactory.SingletonList((SwitchLabelSyntax)SyntaxFactory.CaseSwitchLabel(MySyntaxFactory.Literal(ObjectBuilder.NodeType, ObjectBuilder.Options[i].Option)))).WithStatements(SyntaxFactory.List(list2));
					list.Add(item);
				}
			}
			SwitchStatementSyntax item2 = SyntaxFactory.SwitchStatement(SyntaxFactory.IdentifierName(name)).WithSections(SyntaxFactory.List(list));
			expressions.Add(item2);
		}

		internal override void Reset()
		{
			base.Reset();
			m_sequenceOutputs.Clear();
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				TryRegisterInputNodes(ObjectBuilder.SequenceInput, SequenceInputs);
				m_valueInput = TryRegisterInputNodes(ObjectBuilder.ValueInput.NodeID, Inputs);
				foreach (MyObjectBuilder_SwitchScriptNode.OptionData option in ObjectBuilder.Options)
				{
					if (option.SequenceOutput != -1)
					{
						MyVisualSyntaxNode nodeByID = base.Navigator.GetNodeByID(option.SequenceOutput);
						if (nodeByID != null)
						{
							m_sequenceOutputs.Add(nodeByID);
						}
					}
				}
				m_sequenceOutputs.ForEach(delegate(MyVisualSyntaxNode node)
				{
					node.Preprocess(currentDepth + 1);
				});
			}
			base.Preprocess(currentDepth);
		}
	}
}
