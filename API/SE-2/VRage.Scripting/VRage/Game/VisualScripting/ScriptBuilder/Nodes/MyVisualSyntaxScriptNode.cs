using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Represents a method call from local instance of Script class.
	/// Contains some data that are used out of the graph generation process.
	/// </summary>
	[MyVisualScriptTag(typeof(MyObjectBuilder_ScriptScriptNode))]
	public class MyVisualSyntaxScriptNode : MyVisualSyntaxNode
	{
		private readonly string m_instanceName;

		public new MyObjectBuilder_ScriptScriptNode ObjectBuilder => (MyObjectBuilder_ScriptScriptNode)m_objectBuilder;

		public MyVisualSyntaxScriptNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
			m_instanceName = "m_scriptInstance_" + ObjectBuilder.ID;
		}

		/// <summary>
		/// MyClass m_instanceName = new MyClass();
		/// </summary>
		/// <returns></returns>
		public MemberDeclarationSyntax InstanceDeclaration()
		{
			return SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(ObjectBuilder.Name)).WithVariables(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(m_instanceName)).WithInitializer(SyntaxFactory.EqualsValueClause(SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(ObjectBuilder.Name)).WithArgumentList(SyntaxFactory.ArgumentList()))))));
		}

		public StatementSyntax DisposeCallDeclaration()
		{
			return SyntaxFactory.ExpressionStatement(MySyntaxFactory.MethodInvocation("Dispose", null, m_instanceName));
		}

		private StatementSyntax CreateScriptInvocationSyntax(List<StatementSyntax> dependentStatements)
		{
			List<string> list = new List<string>();
			int num = 0;
			foreach (MyInputParameterSerializationData input in ObjectBuilder.Inputs)
			{
				string text = Inputs[num++].VariableSyntaxName(input.Input.VariableName);
				if (input.Type == "System.String")
				{
					text += ".ToString()";
				}
				list.Add(text);
			}
<<<<<<< HEAD
			List<string> outputVarNames = ObjectBuilder.Outputs.Select((MyOutputParameterSerializationData t, int index) => Outputs[index].VariableSyntaxName(t.Name, output: true)).ToList();
=======
			List<string> outputVarNames = Enumerable.ToList<string>(Enumerable.Select<MyOutputParameterSerializationData, string>((IEnumerable<MyOutputParameterSerializationData>)ObjectBuilder.Outputs, (Func<MyOutputParameterSerializationData, int, string>)((MyOutputParameterSerializationData t, int index) => Outputs[index].VariableSyntaxName(t.Name, output: true))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			InvocationExpressionSyntax invocationExpressionSyntax = MySyntaxFactory.MethodInvocation("RunScript", list, outputVarNames, m_instanceName);
			if (dependentStatements == null)
			{
				return SyntaxFactory.ExpressionStatement(invocationExpressionSyntax);
			}
			return MySyntaxFactory.IfExpressionSyntax(invocationExpressionSyntax, dependentStatements);
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			base.DebugCollected = true;
			base.CollectInputExpressions(expressions, statementsToAppend);
<<<<<<< HEAD
			expressions.AddRange(ObjectBuilder.Outputs.Select((MyOutputParameterSerializationData t, int index) => MySyntaxFactory.LocalVariable(t.Type, Outputs[index].VariableSyntaxName(t.Name, output: true))));
=======
			expressions.AddRange(Enumerable.Select<MyOutputParameterSerializationData, LocalDeclarationStatementSyntax>((IEnumerable<MyOutputParameterSerializationData>)ObjectBuilder.Outputs, (Func<MyOutputParameterSerializationData, int, LocalDeclarationStatementSyntax>)((MyOutputParameterSerializationData t, int index) => MySyntaxFactory.LocalVariable(t.Type, Outputs[index].VariableSyntaxName(t.Name, output: true)))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal override void CollectSequenceExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			CollectInputExpressions(expressions, statementsToAppend);
			statementsToAppend = null;
			List<StatementSyntax> list = new List<StatementSyntax>();
			foreach (MyVisualSyntaxNode sequenceOutput in SequenceOutputs)
			{
				sequenceOutput.CollectSequenceExpressions(list, statementsToAppend);
			}
			StatementSyntax item = CreateScriptInvocationSyntax(list);
			expressions.Add(item);
<<<<<<< HEAD
			List<string> list2 = ObjectBuilder.Inputs.Select((MyInputParameterSerializationData t, int index) => Inputs[index].VariableSyntaxName(t.Input.VariableName)).ToList();
			List<string> list3 = ObjectBuilder.Outputs.Select((MyOutputParameterSerializationData t, int index) => Outputs[index].VariableSyntaxName(t.Name, output: true)).ToList();
			expressions.Add(MySyntaxFactory.LogNodeSyntax(m_objectBuilder.ID, list2.ToArray().Union(list3.ToArray()).ToArray()));
=======
			List<string> list2 = Enumerable.ToList<string>(Enumerable.Select<MyInputParameterSerializationData, string>((IEnumerable<MyInputParameterSerializationData>)ObjectBuilder.Inputs, (Func<MyInputParameterSerializationData, int, string>)((MyInputParameterSerializationData t, int index) => Inputs[index].VariableSyntaxName(t.Input.VariableName))));
			List<string> list3 = Enumerable.ToList<string>(Enumerable.Select<MyOutputParameterSerializationData, string>((IEnumerable<MyOutputParameterSerializationData>)ObjectBuilder.Outputs, (Func<MyOutputParameterSerializationData, int, string>)((MyOutputParameterSerializationData t, int index) => Outputs[index].VariableSyntaxName(t.Name, output: true))));
			expressions.Add(MySyntaxFactory.LogNodeSyntax(m_objectBuilder.ID, Enumerable.ToArray<string>(Enumerable.Union<string>((IEnumerable<string>)list2.ToArray(), (IEnumerable<string>)list3.ToArray()))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
<<<<<<< HEAD
			MyOutputParameterSerializationData myOutputParameterSerializationData = ObjectBuilder.Outputs.FirstOrDefault((MyOutputParameterSerializationData o) => o.Name == variableIdentifier);
=======
			MyOutputParameterSerializationData myOutputParameterSerializationData = Enumerable.FirstOrDefault<MyOutputParameterSerializationData>((IEnumerable<MyOutputParameterSerializationData>)ObjectBuilder.Outputs, (Func<MyOutputParameterSerializationData, bool>)((MyOutputParameterSerializationData o) => o.Name == variableIdentifier));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (myOutputParameterSerializationData != null)
			{
				int num = ObjectBuilder.Outputs.IndexOf(myOutputParameterSerializationData);
				if (num != -1)
				{
					variableIdentifier = Outputs[num].VariableSyntaxName(variableIdentifier, output: true);
				}
			}
			return variableIdentifier;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (ObjectBuilder.SequenceOutput != -1)
				{
					MyVisualSyntaxNode routedOutputNodeByID = base.Navigator.GetRoutedOutputNodeByID(ObjectBuilder.SequenceOutput);
					SequenceOutputs.Add(routedOutputNodeByID);
				}
				foreach (int sequenceInput in ObjectBuilder.SequenceInputs)
				{
					MyVisualSyntaxNode routedInputNodeByID = base.Navigator.GetRoutedInputNodeByID(sequenceInput);
					SequenceInputs.Add(routedInputNodeByID);
				}
				foreach (MyInputParameterSerializationData input in ObjectBuilder.Inputs)
				{
					if (input.Input.NodeID == -1)
					{
						throw new Exception("Script node " + ObjectBuilder.Name + " missing input data. NodeID: " + ObjectBuilder.ID);
					}
					MyVisualSyntaxNode routedInputNodeByID2 = base.Navigator.GetRoutedInputNodeByID(input.Input.NodeID);
					Inputs.Add(routedInputNodeByID2);
				}
				foreach (MyOutputParameterSerializationData output in ObjectBuilder.Outputs)
				{
					if (output.Outputs.Ids.Count != 0)
					{
						foreach (MyVariableIdentifier id in output.Outputs.Ids)
						{
							foreach (MyVisualSyntaxNode item2 in base.Navigator.GetRoutedOutputNodesByID(id.NodeID))
							{
								Outputs.Add(item2);
							}
						}
					}
					else
					{
						MyVisualSyntaxFakeOutputNode item = new MyVisualSyntaxFakeOutputNode(ObjectBuilder.ID);
						Outputs.Add(item);
					}
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
