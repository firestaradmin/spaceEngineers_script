using System;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// Simple method declaration node for implementing the interface methods.
	/// </summary>
	[MyVisualScriptTag(typeof(MyObjectBuilder_InterfaceMethodNode))]
	public class MyVisualSyntaxInterfaceMethodNode : MyVisualSyntaxNode, IMyVisualSyntaxEntryPoint
	{
		private readonly MethodInfo m_method;

		public new MyObjectBuilder_InterfaceMethodNode ObjectBuilder => (MyObjectBuilder_InterfaceMethodNode)m_objectBuilder;

		public MyVisualSyntaxInterfaceMethodNode(MyObjectBuilder_ScriptNode ob, Type baseClass)
			: base(ob)
		{
			m_method = baseClass.GetMethod(ObjectBuilder.MethodName);
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

		public void AddSequenceInput(MyVisualSyntaxNode parent)
		{
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				foreach (int sequenceOutputID in ObjectBuilder.SequenceOutputIDs)
				{
					MyVisualSyntaxNode routedOutputNodeByID = base.Navigator.GetRoutedOutputNodeByID(sequenceOutputID);
					SequenceOutputs.Add(routedOutputNodeByID);
				}
			}
			base.Preprocess(currentDepth);
		}

		public MethodDeclarationSyntax GetMethodDeclaration()
		{
			return MySyntaxFactory.PublicMethodDeclaration(m_method.Name, SyntaxKind.VoidKeyword, ObjectBuilder.OutputNames, ObjectBuilder.OuputTypes);
		}
	}
}
