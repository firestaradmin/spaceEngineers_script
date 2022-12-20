using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.ObjectBuilders.VisualScripting;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_RouteNode))]
	public class MyVisualSyntaxRouteNode : MyVisualSyntaxNode
	{
		public new MyObjectBuilder_RouteNode ObjectBuilder => (MyObjectBuilder_RouteNode)m_objectBuilder;

		internal override bool SequenceDependent => false;

		public MyVisualSyntaxRouteNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			return (output ? "out" : "") + "routeNode_" + ObjectBuilder.ID;
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			base.CollectInputExpressions(expressions, statementsToAppend);
		}

		internal override void Reset()
		{
			base.Reset();
			Inputs.Clear();
			Inputs.Add(base.Navigator.GetNodeByID(ObjectBuilder.Input.NodeID));
			Outputs.Clear();
			foreach (MyVariableIdentifier output in ObjectBuilder.Outputs)
			{
				Outputs.Add(base.Navigator.GetNodeByID(output.NodeID));
			}
		}

		protected internal override void Preprocess(int currentDepth)
		{
			base.Preprocess(currentDepth);
		}
	}
}
