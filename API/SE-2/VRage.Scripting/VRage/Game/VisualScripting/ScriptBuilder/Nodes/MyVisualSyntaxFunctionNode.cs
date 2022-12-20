using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Game.VisualScripting.Utils;
using VRage.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	[MyVisualScriptTag(typeof(MyObjectBuilder_FunctionScriptNode))]
	public class MyVisualSyntaxFunctionNode : MyVisualSyntaxNode
	{
		private readonly MethodInfo m_methodInfo;

		private MyVisualSyntaxNode m_sequenceOutputNode;

		private MyVisualSyntaxNode m_instance;

		private readonly Type m_scriptBaseType;

		private readonly Dictionary<ParameterInfo, MyTuple<MyVisualSyntaxNode, MyVariableIdentifier>> m_parametersToInputs = new Dictionary<ParameterInfo, MyTuple<MyVisualSyntaxNode, MyVariableIdentifier>>();

		internal override bool SequenceDependent => m_methodInfo.IsSequenceDependent();

		public UsingDirectiveSyntax Using { get; private set; }

		public new MyObjectBuilder_FunctionScriptNode ObjectBuilder => (MyObjectBuilder_FunctionScriptNode)m_objectBuilder;

		public MyVisualSyntaxFunctionNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
			m_objectBuilder = (MyObjectBuilder_FunctionScriptNode)ob;
			m_methodInfo = MyVisualScriptingProxy.GetMethodCaseInvariant(ObjectBuilder.Type);
			m_scriptBaseType = scriptBaseType;
			if (m_methodInfo == null && !string.IsNullOrEmpty(ObjectBuilder.DeclaringType))
			{
				Type type = MyVisualScriptingProxy.GetType(ObjectBuilder.DeclaringType);
				if (type != null)
				{
					m_methodInfo = MyVisualScriptingProxy.GetMethod(type, ObjectBuilder.Type);
				}
			}
			if (m_methodInfo == null)
			{
				int startIndex = ObjectBuilder.Type.IndexOf('(');
				string text = ObjectBuilder.Type.Remove(startIndex);
				string text2 = ObjectBuilder.Type.Remove(0, text.LastIndexOf('.') + 1);
				string value = ObjectBuilder.Type.Remove(ObjectBuilder.Type.LastIndexOf(')'));
				startIndex = text2.IndexOf('(');
				if (startIndex > 0)
				{
<<<<<<< HEAD
					(from x in text2.Substring(startIndex).Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries)
						select x.Trim(' ', '(', ')')).ToArray();
=======
					Enumerable.ToArray<string>(Enumerable.Select<string, string>((IEnumerable<string>)text2.Substring(startIndex).Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries), (Func<string, string>)((string x) => x.Trim(' ', '(', ')'))));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					text2 = text2.Remove(startIndex);
					if (scriptBaseType != null)
					{
						m_methodInfo = scriptBaseType.GetMethod(text2);
					}
					if (m_methodInfo == null)
					{
						foreach (MethodInfo method in MyVisualScriptingProxy.GetMethods())
						{
							if (method.Signature().StartsWith(value))
							{
								m_methodInfo = method;
								break;
							}
						}
					}
				}
			}
			if (m_methodInfo == null && !string.IsNullOrEmpty(ObjectBuilder.ExtOfType))
			{
				Type type2 = MyVisualScriptingProxy.GetType(ObjectBuilder.ExtOfType);
				m_methodInfo = MyVisualScriptingProxy.GetMethod(type2, ObjectBuilder.Type);
			}
			if (m_methodInfo != null)
			{
				InitUsing();
			}
		}

		private void InitUsing()
		{
			if (!(m_methodInfo.DeclaringType == null))
			{
				Using = MySyntaxFactory.UsingStatementSyntax(m_methodInfo.DeclaringType.Namespace);
			}
		}

		internal override void Reset()
		{
			base.Reset();
			m_parametersToInputs.Clear();
		}

		internal override void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			base.DebugCollected = true;
			base.CollectInputExpressions(expressions, statementsToAppend);
			List<string> list = new List<string>();
			try
			{
				List<SyntaxNodeOrToken> list2 = new List<SyntaxNodeOrToken>();
				ParameterInfo[] parameters = m_methodInfo.GetParameters();
				int i = 0;
				if (m_methodInfo.IsDefined(typeof(ExtensionAttribute), inherit: false))
				{
					i++;
				}
				for (; i < parameters.Length; i++)
				{
					ParameterInfo parameter = parameters[i];
					MyTuple<MyVisualSyntaxNode, MyVariableIdentifier> value2;
					if (parameter.IsOut)
					{
						string text = VariableSyntaxName(parameter.Name);
						expressions.Add(MySyntaxFactory.LocalVariable(parameter.ParameterType.GetElementType().Signature(), text));
						list2.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(text)).WithNameColon(SyntaxFactory.NameColon(parameter.Name)).WithRefOrOutKeyword(SyntaxFactory.Token(SyntaxKind.OutKeyword)));
						list.Add(text);
					}
					else if (m_parametersToInputs.TryGetValue(parameter, out value2))
					{
						string text2 = value2.Item1.VariableSyntaxName(value2.Item2.VariableName);
						if (parameter.ParameterType == typeof(string))
						{
							IdentifierNameSyntax expression = SyntaxFactory.IdentifierName("System.Convert.ToString( " + text2 + ")");
							list2.Add(SyntaxFactory.Argument(expression).WithNameColon(SyntaxFactory.NameColon(parameter.Name)));
						}
						else if (parameter.ParameterType.IsByRef)
						{
							list2.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(text2)).WithNameColon(SyntaxFactory.NameColon(parameter.Name)).WithRefOrOutKeyword(SyntaxFactory.Token(SyntaxKind.RefKeyword)));
						}
						else
						{
							list2.Add(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(text2)).WithNameColon(SyntaxFactory.NameColon(parameter.Name)));
						}
						list.Add(text2);
					}
					else
					{
						MyParameterValue myParameterValue = ObjectBuilder.InputParameterValues.Find((MyParameterValue value) => value.ParameterName.ToLower() == parameter.Name.ToLower());
						if (myParameterValue == null)
						{
							if (parameter.HasDefaultValue)
							{
								continue;
							}
							ArgumentSyntax argumentSyntax = MySyntaxFactory.ConstantDefaultArgument(parameter.ParameterType);
							list2.Add(argumentSyntax.WithNameColon(SyntaxFactory.NameColon(parameter.Name)));
							list.Add(argumentSyntax.ToString());
						}
						else
						{
							ArgumentSyntax argumentSyntax2 = MySyntaxFactory.ConstantArgument(parameter.ParameterType.Signature(), MyTexts.SubstituteTexts(myParameterValue.Value), parameter.Name);
							list2.Add(argumentSyntax2.WithNameColon(SyntaxFactory.NameColon(parameter.Name)));
							list.Add(argumentSyntax2.ToString());
						}
					}
					list2.Add(SyntaxFactory.Token(SyntaxKind.CommaToken));
				}
				if (list2.Count > 0)
				{
					list2.RemoveAt(list2.Count - 1);
				}
				InvocationExpressionSyntax invocationExpressionSyntax = null;
<<<<<<< HEAD
				if (m_methodInfo.IsStatic && !CustomAttributeExtensions.IsDefined(m_methodInfo, typeof(ExtensionAttribute)))
=======
				if (m_methodInfo.IsStatic && !m_methodInfo.IsDefined(typeof(ExtensionAttribute)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					invocationExpressionSyntax = MySyntaxFactory.MethodInvocationExpressionSyntax(SyntaxFactory.IdentifierName(m_methodInfo.DeclaringType.FullName + "." + m_methodInfo.Name), SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(list2)));
				}
				else if (m_methodInfo.DeclaringType == m_scriptBaseType)
				{
					invocationExpressionSyntax = MySyntaxFactory.MethodInvocationExpressionSyntax(SyntaxFactory.IdentifierName(m_methodInfo.Name), SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(list2)));
				}
				else
				{
					if (m_instance == null)
					{
						throw new Exception("FunctionNode: " + ObjectBuilder.ID + " Is missing mandatory instance input.");
					}
					string name = m_instance.VariableSyntaxName(ObjectBuilder.InstanceInputID.VariableName);
					invocationExpressionSyntax = MySyntaxFactory.MethodInvocationExpressionSyntax(SyntaxFactory.IdentifierName(m_methodInfo.Name), SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(list2)), SyntaxFactory.IdentifierName(name));
				}
				if (m_methodInfo.ReturnType == typeof(void))
				{
					expressions.Add(SyntaxFactory.ExpressionStatement(invocationExpressionSyntax));
				}
				else
				{
					expressions.Add(MySyntaxFactory.LocalVariable(string.Empty, VariableSyntaxName("Return"), invocationExpressionSyntax));
					list.Add(VariableSyntaxName("Return"));
				}
				expressions.Add(MySyntaxFactory.LogNodeSyntax(m_objectBuilder.ID, list.ToArray()));
			}
			catch (Exception ex)
			{
				MyLog.Default.Log(MyLogSeverity.Error, "Function node " + m_methodInfo.Name + " caused exception!");
				throw ex;
			}
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			return (output ? "out" : "") + "paramFunctionNode_" + ObjectBuilder.ID + "_" + variableIdentifier;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				if (SequenceDependent)
				{
					if (ObjectBuilder.SequenceOutputID != -1)
					{
						m_sequenceOutputNode = base.Navigator.GetRoutedOutputNodeByID(ObjectBuilder.SequenceOutputID);
						SequenceOutputs.Add(m_sequenceOutputNode);
					}
					foreach (int sequenceInput in ObjectBuilder.SequenceInputs)
					{
						if (sequenceInput != -1)
						{
							MyVisualSyntaxNode routedInputNodeByID = base.Navigator.GetRoutedInputNodeByID(sequenceInput);
							SequenceInputs.Add(routedInputNodeByID);
						}
					}
				}
				else
				{
					foreach (IdentifierList outputParametersID in ObjectBuilder.OutputParametersIDs)
					{
						foreach (MyVariableIdentifier id in outputParametersID.Ids)
						{
							foreach (MyVisualSyntaxNode item3 in base.Navigator.GetRoutedOutputNodesByID(id.NodeID))
							{
								Outputs.Add(item3);
							}
						}
					}
				}
				ParameterInfo[] parameters = m_methodInfo.GetParameters();
				Inputs.Capacity = ObjectBuilder.InputParameterIDs.Count;
				if (ObjectBuilder.Version == 0)
				{
					for (int i = 0; i < ObjectBuilder.InputParameterIDs.Count; i++)
					{
						MyVariableIdentifier item = ObjectBuilder.InputParameterIDs[i];
						MyVisualSyntaxNode routedInputNodeByID2 = base.Navigator.GetRoutedInputNodeByID(item.NodeID);
						if (routedInputNodeByID2 != null)
						{
							Inputs.Add(routedInputNodeByID2);
							m_parametersToInputs.Add(parameters[i], new MyTuple<MyVisualSyntaxNode, MyVariableIdentifier>(routedInputNodeByID2, item));
						}
					}
				}
				else
				{
					int j = 0;
					if (m_methodInfo.IsDefined(typeof(ExtensionAttribute), inherit: false))
					{
						j++;
					}
					for (; j < parameters.Length; j++)
					{
						ParameterInfo parameter = parameters[j];
						MyVariableIdentifier item2 = ObjectBuilder.InputParameterIDs.Find((MyVariableIdentifier ident) => ident.OriginName == parameter.Name);
						if (string.IsNullOrEmpty(item2.OriginName))
						{
							continue;
						}
						MyVisualSyntaxNode routedInputNodeByID3 = base.Navigator.GetRoutedInputNodeByID(item2.NodeID);
						if (routedInputNodeByID3 == null)
						{
							if (parameter.HasDefaultValue)
							{
							}
						}
						else
						{
							Inputs.Add(routedInputNodeByID3);
							m_parametersToInputs.Add(parameter, new MyTuple<MyVisualSyntaxNode, MyVariableIdentifier>(routedInputNodeByID3, item2));
						}
					}
					if (ObjectBuilder.InstanceInputID.NodeID != -1)
					{
						m_instance = base.Navigator.GetRoutedInputNodeByID(ObjectBuilder.InstanceInputID.NodeID);
						if (m_instance != null)
						{
							Inputs.Add(m_instance);
						}
					}
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
