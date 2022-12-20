using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.Game.VisualScripting.ScriptBuilder;
using VRage.Game.VisualScripting.ScriptBuilder.Nodes;
using VRage.Game.VisualScripting.Utils;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.VisualScripting
{
	/// <summary>
	/// Creates class syntax for provided file.
	///
	/// Notes:
	/// WorldScripts
	///     Consist of Event methods having only input purpose, so they have no output variables and void return value.
	///     One event type with same signature can appear on multiple places in the script. Such situaltion mean that 
	///     the method body will have multiple sections that will be later evaluated independently without 
	///     any order dependency.
	///
	/// NormalScripts
	///     Should have only one input as entry point of the method and multiple or none Output nodes. Output nodes have
	///     parameters defined. Method signature will contain input variables (from input node), output variables (from 
	///     outputs - all outputs must have same signature) and bool return value. 
	///     Return value tells the system if the output node was reached and whenever we should or should not continue
	///     executing the sequence chain.
	/// </summary>
	public class MyVisualScriptBuilder
	{
		private string m_scriptFilePath;

		private string m_scriptName;

		private MyObjectBuilder_VisualScript m_objectBuilder;

		private Type m_baseType;

		private CompilationUnitSyntax m_compilationUnit;

		private MyVisualScriptNavigator m_navigator;

		private ClassDeclarationSyntax m_scriptClassDeclaration;

		private ConstructorDeclarationSyntax m_constructor;

		private MethodDeclarationSyntax m_disposeMethod;

		private NamespaceDeclarationSyntax m_namespaceDeclaration;

		private readonly List<MemberDeclarationSyntax> m_fieldDeclarations = new List<MemberDeclarationSyntax>();

		private readonly List<MethodDeclarationSyntax> m_methodDeclarations = new List<MethodDeclarationSyntax>();

		private readonly List<StatementSyntax> m_helperStatementList = new List<StatementSyntax>();

		private readonly MyVisualSyntaxBuilderNode m_builderNode = new MyVisualSyntaxBuilderNode();

		public string Syntax => m_compilationUnit.ToFullString().Replace("\\\\n", "\\n");

		public string ScriptName => m_scriptName;

		public List<string> Dependencies => m_objectBuilder.DependencyFilePaths;

		public string ScriptFilePath
		{
			get
			{
				return m_scriptFilePath;
			}
			set
			{
				m_scriptFilePath = value;
			}
		}

		public string ErrorMessage { get; set; }

		private void Clear()
		{
			m_fieldDeclarations.Clear();
			m_methodDeclarations.Clear();
		}

		/// <summary>
		/// Loads the script file.
		/// </summary>
		/// <returns></returns>
		public bool Load()
		{
			if (string.IsNullOrEmpty(m_scriptFilePath))
			{
				return false;
			}
			MyObjectBuilder_VSFiles objectBuilder;
			using (Stream reader = MyFileSystem.OpenRead(m_scriptFilePath))
			{
				if (!MyObjectBuilderSerializer.DeserializeXML(reader, out objectBuilder))
				{
					ErrorMessage = "Deserialization failed : " + m_scriptFilePath;
					return false;
				}
			}
			try
			{
				ErrorMessage = string.Empty;
				if (objectBuilder.LevelScript != null)
				{
					m_objectBuilder = objectBuilder.LevelScript;
					if (m_objectBuilder.Nodes != null)
					{
						foreach (MyObjectBuilder_ScriptNode node in m_objectBuilder.Nodes)
						{
							node.AfterDeserialize();
						}
					}
				}
				else if (objectBuilder.VisualScript != null)
				{
					m_objectBuilder = objectBuilder.VisualScript;
					if (m_objectBuilder.Nodes != null)
					{
						foreach (MyObjectBuilder_ScriptNode node2 in m_objectBuilder.Nodes)
						{
							node2.AfterDeserialize();
						}
					}
				}
				m_navigator = new MyVisualScriptNavigator(m_objectBuilder);
				m_scriptName = m_objectBuilder.Name;
				if (m_objectBuilder.Interface != null)
				{
					m_baseType = MyVisualScriptingProxy.GetType(m_objectBuilder.Interface);
				}
			}
			catch (Exception ex)
			{
				string text = "Error occured during the graph reconstruction: " + ex;
				MyLog.Default.WriteLine(text);
				MyLog.Default.WriteLine(ex);
				ErrorMessage = text;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Creates syntax of a class generated out of interactionNodes.
		/// </summary>
		/// <returns></returns>
		public bool Build()
		{
			if (string.IsNullOrEmpty(m_scriptFilePath))
			{
				return false;
			}
			try
			{
				Clear();
				CreateClassSyntax();
				CreateDisposeMethod();
				CreateVariablesAndConstructorSyntax();
				CreateScriptInstances();
				CreateMethods();
				CreateNamespaceDeclaration();
				FinalizeSyntax();
			}
			catch (Exception ex)
			{
				string text = "Script: " + m_scriptName + " failed to build. Error message: " + ex.Message;
				MyLog.Default.Log(MyLogSeverity.Error, text);
				MyLog.Default.Log(MyLogSeverity.Error, ex.ToString());
				ErrorMessage = text;
				return false;
			}
			return true;
		}

		private void CreateDisposeMethod()
		{
			m_disposeMethod = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), SyntaxFactory.Identifier("Dispose")).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithBody(SyntaxFactory.Block());
		}

		private void CreateClassSyntax()
		{
			IdentifierNameSyntax identifierNameSyntax = SyntaxFactory.IdentifierName("IMyLevelScript");
			if (!(m_objectBuilder is MyObjectBuilder_VisualLevelScript))
			{
				identifierNameSyntax = (string.IsNullOrEmpty(m_objectBuilder.Interface) ? null : SyntaxFactory.IdentifierName(m_baseType.Name));
			}
			m_scriptClassDeclaration = MySyntaxFactory.PublicClass(m_scriptName);
			if (identifierNameSyntax != null)
			{
				m_scriptClassDeclaration = m_scriptClassDeclaration.WithBaseList(SyntaxFactory.BaseList(SyntaxFactory.SingletonSeparatedList((BaseTypeSyntax)SyntaxFactory.SimpleBaseType(identifierNameSyntax))));
			}
		}

		private void CreateVariablesAndConstructorSyntax()
		{
			m_constructor = MySyntaxFactory.Constructor(m_scriptClassDeclaration);
			foreach (MyVisualSyntaxVariableNode item in m_navigator.OfType<MyVisualSyntaxVariableNode>())
			{
				m_fieldDeclarations.Add(item.CreateFieldDeclaration());
				m_constructor = m_constructor.AddBodyStatements(item.CreateInitializationSyntax());
			}
		}

		private void LoadDataFromFieldInfo(MyVisualSyntaxEventNode eventNode, List<string> outputNames, List<string> outputTypes)
		{
			outputNames.Clear();
			outputTypes.Clear();
			ParameterInfo[] parameters = eventNode.FieldInfo.FieldType.GetMethod("Invoke").GetParameters();
			new List<string>();
			ParameterInfo[] array = parameters;
			foreach (ParameterInfo parameterInfo in array)
			{
				Type parameterType = parameterInfo.ParameterType;
				if (!outputNames.Contains(parameterInfo.Name))
				{
					outputNames.Add(parameterInfo.Name);
					outputTypes.Add(parameterType.Signature());
				}
			}
		}

		private void CreateMethods()
		{
			if (!string.IsNullOrEmpty(m_objectBuilder.Interface))
			{
				foreach (MyVisualSyntaxInterfaceMethodNode item in m_navigator.OfType<MyVisualSyntaxInterfaceMethodNode>())
				{
					MethodDeclarationSyntax methodDeclaration = item.GetMethodDeclaration();
					ProcessNodes(new MyVisualSyntaxInterfaceMethodNode[1] { item }, ref methodDeclaration);
					m_methodDeclarations.Add(methodDeclaration);
				}
			}
			List<MyVisualSyntaxEventNode> list = m_navigator.OfType<MyVisualSyntaxEventNode>();
			list.AddRange(m_navigator.OfType<MyVisualSyntaxKeyEventNode>());
			while (list.Count > 0)
			{
				MyVisualSyntaxEventNode firstEvent = list[0];
<<<<<<< HEAD
				IEnumerable<MyVisualSyntaxEventNode> eventsWithSameName = list.Where((MyVisualSyntaxEventNode @event) => @event.ObjectBuilder.Name == firstEvent.ObjectBuilder.Name);
=======
				IEnumerable<MyVisualSyntaxEventNode> eventsWithSameName = Enumerable.Where<MyVisualSyntaxEventNode>((IEnumerable<MyVisualSyntaxEventNode>)list, (Func<MyVisualSyntaxEventNode, bool>)((MyVisualSyntaxEventNode @event) => @event.ObjectBuilder.Name == firstEvent.ObjectBuilder.Name));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				List<string> list2 = new List<string>();
				List<string> list3 = new List<string>();
				LoadDataFromFieldInfo(firstEvent, list2, list3);
				MethodDeclarationSyntax methodDeclaration2 = MySyntaxFactory.PublicMethodDeclaration(firstEvent.EventName, SyntaxKind.VoidKeyword, list2, list3);
				ProcessNodes(eventsWithSameName, ref methodDeclaration2);
				m_constructor = m_constructor.AddBodyStatements(MySyntaxFactory.DelegateAssignment(firstEvent.ObjectBuilder.Name, methodDeclaration2.Identifier.ToString()));
				m_disposeMethod = m_disposeMethod.AddBodyStatements(MySyntaxFactory.DelegateRemoval(firstEvent.ObjectBuilder.Name, methodDeclaration2.Identifier.ToString()));
				m_methodDeclarations.Add(methodDeclaration2);
<<<<<<< HEAD
				list.RemoveAll((MyVisualSyntaxEventNode @event) => eventsWithSameName.Contains(@event));
=======
				list.RemoveAll((MyVisualSyntaxEventNode @event) => Enumerable.Contains<MyVisualSyntaxEventNode>(eventsWithSameName, @event));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			foreach (MyVisualSyntaxDelayNode item2 in m_navigator.OfType<MyVisualSyntaxDelayNode>())
			{
				string instanceName = item2.GetInstanceName();
				m_fieldDeclarations.Add(MySyntaxFactory.GenericFieldDeclaration(typeof(MyTimeSpan), item2.GetCounterName()));
				m_constructor = m_constructor.AddBodyStatements(item2.GetCounterInitializer());
				MethodDeclarationSyntax methodDeclaration3 = MySyntaxFactory.PublicMethodDeclaration(instanceName + "_Update", SyntaxKind.VoidKeyword, MyVisualSyntaxDelayNode.FieldInfoOutputNames, MyVisualSyntaxDelayNode.FieldInfoOutputTypes);
				ProcessNodes(new MyVisualSyntaxNode[1] { item2 }, ref methodDeclaration3);
				m_constructor = m_constructor.AddBodyStatements(MySyntaxFactory.DelegateAssignment(MyVisualSyntaxDelayNode.FieldInfo.Signature(), methodDeclaration3.Identifier.ToString()));
				m_disposeMethod = m_disposeMethod.AddBodyStatements(MySyntaxFactory.DelegateRemoval(MyVisualSyntaxDelayNode.FieldInfo.Signature(), methodDeclaration3.Identifier.ToString()));
				m_methodDeclarations.Add(methodDeclaration3);
			}
			List<MyVisualSyntaxInputNode> list4 = m_navigator.OfType<MyVisualSyntaxInputNode>();
			List<MyVisualSyntaxOutputNode> list5 = m_navigator.OfType<MyVisualSyntaxOutputNode>();
			if (list4.Count <= 0)
			{
				return;
			}
			MyVisualSyntaxInputNode myVisualSyntaxInputNode = list4[0];
			MethodDeclarationSyntax methodDeclarationSyntax = null;
			if (list5.Count > 0)
			{
				List<string> list6 = new List<string>(list5[0].ObjectBuilder.Inputs.Count);
				List<string> list7 = new List<string>(list5[0].ObjectBuilder.Inputs.Count);
				foreach (MyInputParameterSerializationData input in list5[0].ObjectBuilder.Inputs)
				{
					list6.Add(input.Name);
					list7.Add(input.Type);
				}
				methodDeclarationSyntax = MySyntaxFactory.PublicMethodDeclaration("RunScript", SyntaxKind.BoolKeyword, myVisualSyntaxInputNode.ObjectBuilder.OutputNames, myVisualSyntaxInputNode.ObjectBuilder.OuputTypes, list6, list7);
			}
			else
			{
				methodDeclarationSyntax = MySyntaxFactory.PublicMethodDeclaration("RunScript", SyntaxKind.BoolKeyword, myVisualSyntaxInputNode.ObjectBuilder.OutputNames, myVisualSyntaxInputNode.ObjectBuilder.OuputTypes);
			}
			ProcessNodes(new MyVisualSyntaxInputNode[1] { myVisualSyntaxInputNode }, ref methodDeclarationSyntax, new List<StatementSyntax> { SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression)) });
			m_methodDeclarations.Add(methodDeclarationSyntax);
		}

		private void ProcessNodes(IEnumerable<MyVisualSyntaxNode> nodes, ref MethodDeclarationSyntax methodDeclaration, List<StatementSyntax> statementsToAppend = null)
		{
			m_helperStatementList.Clear();
			m_navigator.ResetNodes();
			m_builderNode.Reset();
			m_builderNode.SequenceOutputs.AddRange(nodes);
			m_builderNode.Navigator = m_navigator;
			foreach (IMyVisualSyntaxEntryPoint node in nodes)
			{
				node.AddSequenceInput(m_builderNode);
			}
			List<StatementSyntax> list = new List<StatementSyntax>();
			m_builderNode.Preprocess();
			m_builderNode.CollectSequenceExpressions(m_helperStatementList, list);
			if (list.Count() > 0)
			{
				m_helperStatementList.AddRange(list);
			}
			else if (statementsToAppend != null)
			{
				m_helperStatementList.AddRange(statementsToAppend);
			}
			methodDeclaration = methodDeclaration.AddBodyStatements(m_helperStatementList.ToArray());
		}

		private void AddMissionLogicScriptMethods()
		{
			MethodDeclarationSyntax item = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.LongKeyword)), SyntaxFactory.Identifier("GetOwnerId")).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithBody(SyntaxFactory.Block(SyntaxFactory.SingletonList((StatementSyntax)SyntaxFactory.ReturnStatement(SyntaxFactory.IdentifierName("OwnerId")))));
			PropertyDeclarationSyntax item2 = SyntaxFactory.PropertyDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.LongKeyword)), SyntaxFactory.Identifier("OwnerId")).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List(new AccessorDeclarationSyntax[2]
			{
				SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
				SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
			})));
			PropertyDeclarationSyntax item3 = SyntaxFactory.PropertyDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)), SyntaxFactory.Identifier("TransitionTo")).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List(new AccessorDeclarationSyntax[2]
			{
				SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
				SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
			})));
			MethodDeclarationSyntax item4 = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), SyntaxFactory.Identifier("Complete")).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword))).WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Parameter(SyntaxFactory.Identifier("transitionName")).WithType(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword))).WithDefault(SyntaxFactory.EqualsValueClause(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("Completed")))))))
				.WithBody(SyntaxFactory.Block(SyntaxFactory.SingletonList((StatementSyntax)SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName("TransitionTo"), SyntaxFactory.IdentifierName("transitionName"))))));
			m_methodDeclarations.Add(item4);
			m_fieldDeclarations.Add(item3);
			m_fieldDeclarations.Add(item2);
			m_methodDeclarations.Add(item);
		}

		private void CreateScriptInstances()
		{
			List<MyVisualSyntaxScriptNode> list = m_navigator.OfType<MyVisualSyntaxScriptNode>();
			if (list == null)
			{
				return;
			}
			foreach (MyVisualSyntaxScriptNode item in list)
			{
				m_fieldDeclarations.Add(item.InstanceDeclaration());
				m_disposeMethod = m_disposeMethod.AddBodyStatements(item.DisposeCallDeclaration());
			}
		}

		private void CreateNamespaceDeclaration()
		{
			m_namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName("VisualScripting.CustomScripts"));
		}

		private void AddMissingInterfaceMethods()
		{
			if (m_baseType == null || !m_baseType.IsInterface)
			{
				return;
			}
			MethodInfo[] methods = m_baseType.GetMethods();
			foreach (MethodInfo methodInfo in methods)
			{
				bool flag = false;
				foreach (MethodDeclarationSyntax methodDeclaration in m_methodDeclarations)
				{
					if (methodDeclaration.Identifier.ToFullString() == methodInfo.Name)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
<<<<<<< HEAD
					VisualScriptingMember customAttribute = CustomAttributeExtensions.GetCustomAttribute<VisualScriptingMember>(methodInfo);
=======
					VisualScriptingMember customAttribute = methodInfo.GetCustomAttribute<VisualScriptingMember>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (customAttribute != null && !customAttribute.Reserved && !methodInfo.IsSpecialName)
					{
						m_methodDeclarations.Add(MySyntaxFactory.MethodDeclaration(methodInfo));
					}
				}
			}
		}

		private void FinalizeSyntax()
		{
			bool flag = false;
			for (int i = 0; i < m_methodDeclarations.Count; i++)
			{
				if (m_methodDeclarations[i].Identifier.ToString() == m_disposeMethod.Identifier.ToString())
				{
					if (m_disposeMethod.Body.Statements.Count > 0)
					{
						m_methodDeclarations[i] = m_methodDeclarations[i].AddBodyStatements(m_disposeMethod.Body);
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				m_methodDeclarations.Add(m_disposeMethod);
			}
			AddMissingInterfaceMethods();
			if (m_baseType == typeof(IMyStateMachineScript))
			{
				AddMissionLogicScriptMethods();
			}
			m_scriptClassDeclaration = m_scriptClassDeclaration.AddMembers(m_fieldDeclarations.ToArray());
			m_scriptClassDeclaration = m_scriptClassDeclaration.AddMembers(m_constructor);
			ClassDeclarationSyntax scriptClassDeclaration = m_scriptClassDeclaration;
			MemberDeclarationSyntax[] items = m_methodDeclarations.ToArray();
			m_scriptClassDeclaration = scriptClassDeclaration.AddMembers(items);
			m_namespaceDeclaration = m_namespaceDeclaration.AddMembers(m_scriptClassDeclaration);
			List<UsingDirectiveSyntax> list = new List<UsingDirectiveSyntax>();
<<<<<<< HEAD
			HashSet<string> hashSet = new HashSet<string>();
=======
			HashSet<string> val = new HashSet<string>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			UsingDirectiveSyntax usingDirectiveSyntax = MySyntaxFactory.UsingStatementSyntax("VRage.Game.VisualScripting");
			UsingDirectiveSyntax usingDirectiveSyntax2 = MySyntaxFactory.UsingStatementSyntax("System.Collections.Generic");
			UsingDirectiveSyntax usingDirectiveSyntax3 = MySyntaxFactory.UsingStatementSyntax("VRage.Library.Utils");
			list.Add(usingDirectiveSyntax);
			list.Add(usingDirectiveSyntax2);
			list.Add(usingDirectiveSyntax3);
<<<<<<< HEAD
			hashSet.Add(usingDirectiveSyntax.ToFullString());
			hashSet.Add(usingDirectiveSyntax2.ToFullString());
			hashSet.Add(usingDirectiveSyntax3.ToFullString());
			foreach (MyVisualSyntaxFunctionNode item in m_navigator.OfType<MyVisualSyntaxFunctionNode>())
			{
				if (hashSet.Add(item.Using.ToFullString()))
=======
			val.Add(usingDirectiveSyntax.ToFullString());
			val.Add(usingDirectiveSyntax2.ToFullString());
			val.Add(usingDirectiveSyntax3.ToFullString());
			foreach (MyVisualSyntaxFunctionNode item in m_navigator.OfType<MyVisualSyntaxFunctionNode>())
			{
				if (val.Add(item.Using.ToFullString()))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					list.Add(item.Using);
				}
			}
			foreach (MyVisualSyntaxVariableNode item2 in m_navigator.OfType<MyVisualSyntaxVariableNode>())
			{
<<<<<<< HEAD
				if (hashSet.Add(item2.Using.ToFullString()))
=======
				if (val.Add(item2.Using.ToFullString()))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					list.Add(item2.Using);
				}
			}
			m_compilationUnit = SyntaxFactory.CompilationUnit().WithUsings(SyntaxFactory.List(list)).AddMembers(m_namespaceDeclaration)
				.NormalizeWhitespace();
		}
	}
}
