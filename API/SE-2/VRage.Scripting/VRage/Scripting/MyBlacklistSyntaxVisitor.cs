using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VRage.Scripting
{
	public class MyBlacklistSyntaxVisitor : CSharpSyntaxVisitor
	{
		public enum MyBlacklistedItemType
		{
			Undefined,
			Attribute
		}

		public struct MyBlacklistedItem
		{
			public MyBlacklistedItemType ItemType;

			public string Message;
		}

		private HashSet<string> m_blacklistedAttributes = new HashSet<string>();

		private SymbolDisplayFormat m_fullnameFormat;

		private SemanticModel m_model;

		private List<MyBlacklistedItem> m_results = new List<MyBlacklistedItem>();

		public IReadOnlyCollection<MyBlacklistedItem> Results => m_results.AsReadOnly();

		public MyBlacklistSyntaxVisitor()
		{
			m_blacklistedAttributes.Add(typeof(SuppressMessageAttribute).FullName);
			m_fullnameFormat = new SymbolDisplayFormat(SymbolDisplayGlobalNamespaceStyle.Omitted, SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
		}

		public void SetSemanticModel(SemanticModel model)
		{
			m_model = model;
		}

		public override void Visit(SyntaxNode node)
		{
			if (m_model == null)
			{
				return;
			}
			base.Visit(node);
			foreach (SyntaxNode item in node.ChildNodes())
			{
				Visit(item);
			}
		}

		public override void VisitAttribute(AttributeSyntax node)
		{
			node.ToFullString();
			string text = m_model.GetTypeInfo(node).Type.ToDisplayString(m_fullnameFormat);
			if (m_blacklistedAttributes.Contains(text))
			{
				string message = $"Error: Blacklisted attribute {text} was used. ";
				m_results.Add(new MyBlacklistedItem
				{
					ItemType = MyBlacklistedItemType.Attribute,
					Message = message
				});
			}
			base.VisitAttribute(node);
		}

		public void Clear()
		{
			m_results.Clear();
		}

		public bool HasAnyResult()
		{
			return m_results.Count > 0;
		}

		public void GetResultMessages(List<Message> results)
		{
			foreach (MyBlacklistedItem result in m_results)
			{
				Message item = new Message(isError: true, result.Message);
				results.Add(item);
			}
		}
	}
}
