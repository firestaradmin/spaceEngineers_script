using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.VisualScripting.Utils;
using VRageMath;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_ConstantScriptNode))]
	public class MyVisualSyntaxConstantNode : MyVisualSyntaxNode
	{
		internal override bool SequenceDependent => false;

		public new MyObjectBuilder_ConstantScriptNode ObjectBuilder => (MyObjectBuilder_ConstantScriptNode)m_objectBuilder;

		public MyVisualSyntaxConstantNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			string text = ObjectBuilder.Value ?? string.Empty;
			Type type = MyVisualScriptingProxy.GetType(ObjectBuilder.Type);
			base.CollectInputExpressions(expressions, statementsToAppend);
			if (type == typeof(Color) || type.IsEnum)
			{
				expressions.Add(MySyntaxFactory.LocalVariable(ObjectBuilder.Type, VariableSyntaxName(), SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(ObjectBuilder.Type), SyntaxFactory.IdentifierName(text))));
			}
			else if (type == typeof(Vector3D))
			{
				expressions.Add(MySyntaxFactory.LocalVariable(ObjectBuilder.Type, VariableSyntaxName(), MySyntaxFactory.NewVector3D(ObjectBuilder.Vector)));
			}
			else
			{
				expressions.Add(MySyntaxFactory.LocalVariable(ObjectBuilder.Type, VariableSyntaxName(), MySyntaxFactory.Literal(ObjectBuilder.Type, text)));
			}
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			string text = (output ? "out" : "") + "constantNode_" + ObjectBuilder.ID;
			if (!string.IsNullOrEmpty(variableIdentifier) && variableIdentifier != "Value")
			{
				text = ((!output) ? (text + "." + variableIdentifier) : (text + "_" + variableIdentifier));
			}
			return text;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				for (int i = 0; i < ObjectBuilder.OutputIds.Ids.Count; i++)
				{
					foreach (MyVisualSyntaxNode item in base.Navigator.GetRoutedOutputNodesByID(ObjectBuilder.OutputIds.Ids[i].NodeID))
					{
						if (item != null)
						{
							Outputs.Add(item);
						}
					}
				}
				for (int j = 0; j < ObjectBuilder.OutputIdsX.Ids.Count; j++)
				{
					foreach (MyVisualSyntaxNode item2 in base.Navigator.GetRoutedOutputNodesByID(ObjectBuilder.OutputIdsX.Ids[j].NodeID))
					{
						if (item2 != null)
						{
							Outputs.Add(item2);
						}
					}
				}
				for (int k = 0; k < ObjectBuilder.OutputIdsY.Ids.Count; k++)
				{
					foreach (MyVisualSyntaxNode item3 in base.Navigator.GetRoutedOutputNodesByID(ObjectBuilder.OutputIdsY.Ids[k].NodeID))
					{
						if (item3 != null)
						{
							Outputs.Add(item3);
						}
					}
				}
				for (int l = 0; l < ObjectBuilder.OutputIdsZ.Ids.Count; l++)
				{
					foreach (MyVisualSyntaxNode item4 in base.Navigator.GetRoutedOutputNodesByID(ObjectBuilder.OutputIdsZ.Ids[l].NodeID))
					{
						if (item4 != null)
						{
							Outputs.Add(item4);
						}
					}
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
