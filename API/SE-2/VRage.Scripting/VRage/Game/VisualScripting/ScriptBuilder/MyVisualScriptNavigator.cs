using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Factory;
using VRage.Game.VisualScripting.ScriptBuilder.Nodes;

namespace VRage.Game.VisualScripting.ScriptBuilder
{
	internal class MyVisualScriptNavigator
	{
		private static MyObjectFactory<MyVisualScriptTag, MyVisualSyntaxNode> m_objectFactory;

		private readonly Dictionary<int, MyVisualSyntaxNode> m_idToNode = new Dictionary<int, MyVisualSyntaxNode>();

		private readonly Dictionary<Type, List<MyVisualSyntaxNode>> m_nodesByType = new Dictionary<Type, List<MyVisualSyntaxNode>>();

		private readonly Dictionary<string, MyVisualSyntaxVariableNode> m_variablesByName = new Dictionary<string, MyVisualSyntaxVariableNode>();

		private readonly List<MyVisualSyntaxNode> m_freshNodes = new List<MyVisualSyntaxNode>();

		public List<MyVisualSyntaxNode> FreshNodes => m_freshNodes;

		static MyVisualScriptNavigator()
		{
			m_objectFactory = new MyObjectFactory<MyVisualScriptTag, MyVisualSyntaxNode>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyVisualScriptNavigator)));
		}

		public MyVisualScriptNavigator(MyObjectBuilder_VisualScript scriptOb)
		{
			Type type = (string.IsNullOrEmpty(scriptOb.Interface) ? null : MyVisualScriptingProxy.GetType(scriptOb.Interface));
			foreach (MyObjectBuilder_ScriptNode node in scriptOb.Nodes)
			{
				MyVisualSyntaxNode myVisualSyntaxNode = m_objectFactory.CreateInstance(node.TypeId, node, type);
				if (myVisualSyntaxNode != null)
				{
					myVisualSyntaxNode.Navigator = this;
					m_idToNode.Add(node.ID, myVisualSyntaxNode);
					Type type2 = myVisualSyntaxNode.GetType();
					if (!m_nodesByType.ContainsKey(type2))
					{
						m_nodesByType.Add(type2, new List<MyVisualSyntaxNode>());
					}
					m_nodesByType[type2].Add(myVisualSyntaxNode);
					if (type2 == typeof(MyVisualSyntaxVariableNode))
					{
						m_variablesByName.Add(((MyObjectBuilder_VariableScriptNode)node).VariableName, (MyVisualSyntaxVariableNode)myVisualSyntaxNode);
					}
				}
			}
		}

		public MyVisualSyntaxNode GetNodeByID(int id)
		{
			m_idToNode.TryGetValue(id, out var value);
			return value;
		}

		public MyVisualSyntaxNode GetRoutedInputNodeByID(int id)
		{
			m_idToNode.TryGetValue(id, out var value);
			while (value is MyVisualSyntaxRouteNode)
			{
				value = value.Inputs[0];
			}
			return value;
		}

		public MyVisualSyntaxNode GetRoutedOutputNodeByID(int id)
		{
			m_idToNode.TryGetValue(id, out var value);
			while (value is MyVisualSyntaxRouteNode)
			{
				value = value.Outputs[0];
			}
			return value;
		}

		public List<MyVisualSyntaxNode> GetRoutedOutputNodesByID(int id)
		{
			m_idToNode.TryGetValue(id, out var value);
			while (value is MyVisualSyntaxRouteNode)
			{
				if (!(value.Outputs[0] is MyVisualSyntaxRouteNode))
				{
					return value.Outputs;
				}
				value = value.Outputs[0];
			}
			return new List<MyVisualSyntaxNode> { value };
		}

		public List<T> OfType<T>() where T : MyVisualSyntaxNode
		{
			List<MyVisualSyntaxNode> list = new List<MyVisualSyntaxNode>();
			foreach (KeyValuePair<Type, List<MyVisualSyntaxNode>> item in m_nodesByType)
			{
				if (typeof(T) == item.Key)
				{
					list.AddRange(item.Value);
				}
			}
			return list.ConvertAll((MyVisualSyntaxNode node) => (T)node);
		}

		public void ResetNodes()
		{
			foreach (KeyValuePair<int, MyVisualSyntaxNode> item in m_idToNode)
			{
				item.Value.Reset();
			}
		}

		public MyVisualSyntaxVariableNode GetVariable(string name)
		{
			m_variablesByName.TryGetValue(name, out var value);
			return value;
		}
	}
}
