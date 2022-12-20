using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// This node represents a Method signature / entrypoint to method.
	/// Contains only general information about the method signature.
	/// Generates no syntax.
	/// </summary>
	[MyVisualScriptTag(typeof(MyObjectBuilder_EventScriptNode))]
	public class MyVisualSyntaxEventNode : MyVisualSyntaxNode, IMyVisualSyntaxEntryPoint
	{
		private MyVisualSyntaxNode m_nextSequenceNode;

		protected FieldInfo m_fieldInfo;

		public new MyObjectBuilder_EventScriptNode ObjectBuilder => (MyObjectBuilder_EventScriptNode)m_objectBuilder;

		public virtual string EventName => m_fieldInfo.Name;

		public FieldInfo FieldInfo => m_fieldInfo;

		public MyVisualSyntaxEventNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
			if (!(ObjectBuilder is MyObjectBuilder_InputScriptNode))
			{
				m_fieldInfo = MyVisualScriptingProxy.GetField(ObjectBuilder.Name);
			}
		}

		public void AddSequenceInput(MyVisualSyntaxNode node)
		{
			SequenceInputs.Add(node);
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			foreach (string outputName in ObjectBuilder.OutputNames)
			{
				if (outputName == variableIdentifier)
				{
					return variableIdentifier;
				}
			}
			return null;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed && ObjectBuilder.SequenceOutputID != -1)
			{
				m_nextSequenceNode = base.Navigator.GetRoutedOutputNodeByID(ObjectBuilder.SequenceOutputID);
				SequenceOutputs.Add(m_nextSequenceNode);
			}
			base.Preprocess(currentDepth);
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			base.DebugCollected = true;
			base.CollectInputExpressions(expressions, statementsToAppend);
			expressions.Add(MySyntaxFactory.LogNodeSyntax(m_objectBuilder.ID, ObjectBuilder.OutputNames.ToArray()));
		}
	}
}
