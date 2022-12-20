using System;

namespace VRage.Game.VisualScripting.ScriptBuilder.Nodes
{
	/// <summary>
	/// This node represents a class variable name getter.
	/// Genertes no syntax and only provides other node with
	/// variable name.
	/// </summary>
	[MyVisualScriptTag(typeof(MyObjectBuilder_GetterScriptNode))]
	public class MyVisualSyntaxGetterNode : MyVisualSyntaxNode
	{
		internal override bool SequenceDependent => false;

		public new MyObjectBuilder_GetterScriptNode ObjectBuilder => (MyObjectBuilder_GetterScriptNode)m_objectBuilder;

		public MyVisualSyntaxGetterNode(MyObjectBuilder_ScriptNode ob, Type scriptBaseType)
			: base(ob)
		{
		}

		protected internal override string VariableSyntaxName(string variableIdentifier = null, bool output = false)
		{
			return (output ? "out" : "") + ObjectBuilder.BoundVariableName;
		}

		protected internal override void Preprocess(int currentDepth)
		{
			if (!base.Preprocessed)
			{
				for (int i = 0; i < ObjectBuilder.OutputIDs.Ids.Count; i++)
				{
					foreach (MyVisualSyntaxNode item in base.Navigator.GetRoutedOutputNodesByID(ObjectBuilder.OutputIDs.Ids[i].NodeID))
					{
						if (item != null)
						{
							Outputs.Add(item);
						}
					}
				}
			}
			base.Preprocess(currentDepth);
		}
	}
}
