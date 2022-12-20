using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VRage.Collections;
using VRage.Game.VisualScripting.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	public class MyVisualSyntaxNode
	{
		protected struct HeapNodeWrapper
		{
			public MyVisualSyntaxNode Node;
		}

		/// <summary>
		/// Depth of the node in the graph.
		/// </summary>
		internal int Depth = int.MaxValue;

		/// <summary>
		/// Nodes that got referenced more than once at one syntax level.
		/// </summary>
		internal HashSet<MyVisualSyntaxNode> SubTreeNodes = new HashSet<MyVisualSyntaxNode>();

		/// <summary>
		/// Data container;
		/// </summary>
		protected MyObjectBuilder_ScriptNode m_objectBuilder;

		private static readonly MyBinaryStructHeap<int, HeapNodeWrapper> m_activeHeap = new MyBinaryStructHeap<int, HeapNodeWrapper>();

		private static readonly HashSet<MyVisualSyntaxNode> m_commonParentSet = new HashSet<MyVisualSyntaxNode>();

		private static readonly HashSet<MyVisualSyntaxNode> m_sequenceHelper = new HashSet<MyVisualSyntaxNode>();

		/// <summary>
		/// Tells if the node was already preprocessed.
		/// (default value is false)
		/// </summary>
		protected bool Preprocessed { get; set; }

		/// <summary>
		/// Tells whenever the node has sequence or not.
		/// </summary>
		internal virtual bool SequenceDependent => true;

		/// <summary>
		/// Is getting set to true the first time the syntax from this node is collected.
		/// Prevents duplicities in syntax.
		/// </summary>
		internal bool Collected { get; private set; }

		/// <summary>
		/// Indication of debug data collected
		/// </summary>
		public bool DebugCollected { get; protected set; }

		/// <summary>
		/// List of sequence input nodes connected to this one.
		/// </summary>
		internal virtual List<MyVisualSyntaxNode> SequenceInputs { get; private set; }

		/// <summary>
		/// List of sequence output nodes connected to this one.
		/// </summary>
		internal virtual List<MyVisualSyntaxNode> SequenceOutputs { get; private set; }

		/// <summary>
		/// Output nodes.
		/// </summary>
		internal virtual List<MyVisualSyntaxNode> Outputs { get; private set; }

		/// <summary>
		/// Input Nodes.
		/// </summary>
		internal virtual List<MyVisualSyntaxNode> Inputs { get; private set; }

		/// <summary>
		/// Data container getter.
		/// </summary>
		public MyObjectBuilder_ScriptNode ObjectBuilder => m_objectBuilder;

		/// <summary>
		/// Member used for in-graph navigation.
		/// </summary>
		internal MyVisualScriptNavigator Navigator { get; set; }

		/// <summary>
		/// Resets nodes to state when they are ready for new run of the builder.
		/// </summary>
		internal virtual void Reset()
		{
			Depth = int.MaxValue;
			SubTreeNodes.Clear();
			Inputs.Clear();
			Outputs.Clear();
			SequenceOutputs.Clear();
			SequenceInputs.Clear();
			Collected = false;
			DebugCollected = false;
			Preprocessed = false;
		}

		/// <summary>
		/// Unique identifier within class syntax.
		/// </summary>
		protected internal virtual string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns ordered set of expressions from value suppliers.
		/// </summary>
		/// <returns></returns>
		internal virtual void CollectInputExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
<<<<<<< HEAD
=======
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Collected = true;
			if (m_objectBuilder != null && MySyntaxFactory.DEBUG_MODE && !DebugCollected)
			{
				expressions.Add(MySyntaxFactory.LogNodeSyntax(m_objectBuilder.ID));
			}
			DebugCollected = true;
<<<<<<< HEAD
			foreach (MyVisualSyntaxNode subTreeNode in SubTreeNodes)
			{
				if (!subTreeNode.Collected)
				{
					subTreeNode.CollectInputExpressions(expressions, statementsToAppend);
				}
			}
=======
			Enumerator<MyVisualSyntaxNode> enumerator = SubTreeNodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyVisualSyntaxNode current = enumerator.get_Current();
					if (!current.Collected)
					{
						current.CollectInputExpressions(expressions, statementsToAppend);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// Returns ordered set of all expressions.
		/// </summary>
		/// <param name="expressions"></param>
<<<<<<< HEAD
		/// <param name="statementsToAppend"></param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		internal virtual void CollectSequenceExpressions(List<StatementSyntax> expressions, List<StatementSyntax> statementsToAppend)
		{
			CollectInputExpressions(expressions, statementsToAppend);
			foreach (MyVisualSyntaxNode sequenceOutput in SequenceOutputs)
			{
				sequenceOutput.CollectSequenceExpressions(expressions, statementsToAppend);
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="ob">Should never be a base of _scriptNode.</param>
		internal MyVisualSyntaxNode(MyObjectBuilder_ScriptNode ob)
		{
			m_objectBuilder = ob;
			Inputs = new List<MyVisualSyntaxNode>();
			Outputs = new List<MyVisualSyntaxNode>();
			SequenceInputs = new List<MyVisualSyntaxNode>();
			SequenceOutputs = new List<MyVisualSyntaxNode>();
		}

		/// <summary>
		/// Pre-generation process that loads necessary data in derived nodes
		/// and adjusts the internal state of graph to generation ready state.
		/// </summary>
		/// <param name="currentDepth"></param>
		protected internal virtual void Preprocess(int currentDepth)
		{
			if (currentDepth < Depth)
			{
				Depth = currentDepth;
			}
			if (!Preprocessed)
			{
				foreach (MyVisualSyntaxNode sequenceOutput in SequenceOutputs)
				{
					sequenceOutput.Preprocess(Depth + 1);
				}
			}
			foreach (MyVisualSyntaxNode input in Inputs)
			{
				if (!input.SequenceDependent)
				{
					input.Preprocess(Depth);
				}
			}
			if (!SequenceDependent && !Preprocessed)
			{
				if (Outputs.Count == 1 && !Outputs[0].SequenceDependent)
				{
					Outputs[0].SubTreeNodes.Add(this);
				}
				else if (Outputs.Count > 0)
				{
					Navigator.FreshNodes.Add(this);
				}
			}
			Preprocessed = true;
		}

		/// <summary>
		/// Tries to put the node of id into collection.
		/// </summary>
		/// <param name="nodeID">Id of looked for node.</param>
		/// <param name="collection">Target collection.</param>
		protected MyVisualSyntaxNode TryRegisterInputNodes(int nodeID, List<MyVisualSyntaxNode> collection)
		{
			if (nodeID == -1)
			{
				return null;
			}
			MyVisualSyntaxNode routedInputNodeByID = Navigator.GetRoutedInputNodeByID(nodeID);
			if (routedInputNodeByID != null)
			{
				collection.Add(routedInputNodeByID);
			}
			return routedInputNodeByID;
		}

		/// <summary>
		/// Tries to put the node of id into collection.
		/// </summary>
		/// <param name="nodeID">Id of looked for node.</param>
		/// <param name="collection">Target collection.</param>
		protected MyVisualSyntaxNode TryRegisterOutputNodes(int nodeID, List<MyVisualSyntaxNode> collection)
		{
			if (nodeID == -1)
			{
				return null;
			}
			List<MyVisualSyntaxNode> routedOutputNodesByID = Navigator.GetRoutedOutputNodesByID(nodeID);
			foreach (MyVisualSyntaxNode item in routedOutputNodesByID)
			{
				collection.Add(item);
			}
			if (routedOutputNodesByID.Count <= 0)
			{
				return null;
			}
<<<<<<< HEAD
			return routedOutputNodesByID.Last();
=======
			return Enumerable.Last<MyVisualSyntaxNode>((IEnumerable<MyVisualSyntaxNode>)routedOutputNodesByID);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// Method that is needed for Hashing purposes.
		/// Id should be a unique identifier within a graph.
		/// </summary>
		/// <returns>Id of node.</returns>
		public override int GetHashCode()
		{
			if (ObjectBuilder == null)
			{
				return GetType().GetHashCode();
			}
			return ObjectBuilder.ID;
		}

		protected static MyVisualSyntaxNode CommonParent(IEnumerable<MyVisualSyntaxNode> nodes)
		{
			m_commonParentSet.Clear();
			m_activeHeap.Clear();
			foreach (MyVisualSyntaxNode node in nodes)
			{
				if (m_commonParentSet.Add(node))
				{
					m_activeHeap.Insert(new HeapNodeWrapper
					{
						Node = node
					}, -node.Depth);
				}
			}
			HeapNodeWrapper current;
			while (true)
			{
				current = m_activeHeap.RemoveMin();
				if (m_activeHeap.Count == 0)
				{
					break;
				}
				if (current.Node.SequenceInputs.Count == 0)
				{
					if (m_activeHeap.Count > 0)
					{
						return null;
					}
					continue;
				}
				current.Node.SequenceInputs.ForEach(delegate(MyVisualSyntaxNode node)
				{
					if (m_activeHeap.Count > 0 && m_commonParentSet.Add(node))
					{
						current.Node = node;
						m_activeHeap.Insert(current, -current.Node.Depth);
					}
				});
			}
			if (current.Node is MyVisualSyntaxForLoopNode)
			{
<<<<<<< HEAD
				return current.Node.SequenceInputs.FirstOrDefault();
=======
				return Enumerable.FirstOrDefault<MyVisualSyntaxNode>((IEnumerable<MyVisualSyntaxNode>)current.Node.SequenceInputs);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return current.Node;
		}

		public IEnumerable<MyVisualSyntaxNode> GetSequenceDependentOutputs()
		{
			m_sequenceHelper.Clear();
			SequenceDependentChildren(m_sequenceHelper);
<<<<<<< HEAD
			return m_sequenceHelper;
=======
			return (IEnumerable<MyVisualSyntaxNode>)m_sequenceHelper;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void SequenceDependentChildren(HashSet<MyVisualSyntaxNode> results)
		{
			if (Outputs.Count == 0 || Depth == int.MaxValue)
			{
				return;
			}
			foreach (MyVisualSyntaxNode output in Outputs)
			{
				if (output != null && output.Depth != int.MaxValue)
				{
					if (output.SequenceDependent)
					{
						results.Add(output);
					}
					else
					{
						output.SequenceDependentChildren(results);
					}
				}
			}
		}
	}
}
