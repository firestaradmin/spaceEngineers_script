using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VRage.Scripting.Rewriters
{
	internal class ProtoTagRewriter : CSharpSyntaxRewriter
	{
		private readonly SemanticModel m_semanticModel;

		private readonly INamedTypeSymbol m_protoMemberAttributeType;

		private ProtoTagRewriter(CSharpCompilation compilation, SyntaxTree syntaxTree)
		{
			m_semanticModel = compilation.GetSemanticModel(syntaxTree);
			m_protoMemberAttributeType = compilation.GetTypeByMetadataName("ProtoBuf.ProtoMemberAttribute");
		}

		public override SyntaxNode VisitAttribute(AttributeSyntax node)
		{
			SymbolInfo symbolInfo = m_semanticModel.GetSymbolInfo(node);
			INamedTypeSymbol namedTypeSymbol = symbolInfo.Symbol as INamedTypeSymbol;
			if (namedTypeSymbol == null)
			{
				ImmutableArray<ISymbol>.Enumerator enumerator = symbolInfo.CandidateSymbols.GetEnumerator();
				while (enumerator.MoveNext())
				{
					IMethodSymbol methodSymbol = enumerator.Current as IMethodSymbol;
					if (methodSymbol != null && methodSymbol.MethodKind == MethodKind.Constructor && methodSymbol.ContainingType == m_protoMemberAttributeType)
					{
						namedTypeSymbol = methodSymbol.ContainingType;
						break;
					}
				}
			}
			if (namedTypeSymbol == m_protoMemberAttributeType)
			{
				SeparatedSyntaxList<AttributeArgumentSyntax>? separatedSyntaxList = node.ArgumentList?.Arguments;
				if (!separatedSyntaxList.HasValue || separatedSyntaxList.Value.Count == 0 || !HasTagArgument(separatedSyntaxList.Value))
				{
					AttributeSyntax attributeSyntax = node;
<<<<<<< HEAD
					AttributeArgumentSyntax[] first = new AttributeArgumentSyntax[1] { SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(node.GetLocation().GetLineSpan().StartLinePosition.Line))) };
=======
					AttributeArgumentSyntax[] obj = new AttributeArgumentSyntax[1] { SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(node.GetLocation().GetLineSpan().StartLinePosition.Line))) };
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					SeparatedSyntaxList<AttributeArgumentSyntax>? separatedSyntaxList2 = separatedSyntaxList;
					IEnumerable<AttributeArgumentSyntax> enumerable;
					if (!separatedSyntaxList2.HasValue)
					{
						enumerable = Enumerable.Empty<AttributeArgumentSyntax>();
					}
					else
					{
						IEnumerable<AttributeArgumentSyntax> enumerable2 = separatedSyntaxList2.GetValueOrDefault();
						enumerable = enumerable2;
					}
					node = attributeSyntax.WithArgumentList(SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList(Enumerable.Concat<AttributeArgumentSyntax>((IEnumerable<AttributeArgumentSyntax>)obj, enumerable))).NormalizeWhitespace());
				}
			}
			return node;
			static bool HasTagArgument(SeparatedSyntaxList<AttributeArgumentSyntax> args)
			{
				for (int i = 0; i < args.Count; i++)
				{
					AttributeArgumentSyntax attributeArgumentSyntax = args[i];
					if (attributeArgumentSyntax.NameEquals == null || attributeArgumentSyntax.NameEquals.IsMissing)
					{
						if (attributeArgumentSyntax.NameColon == null || attributeArgumentSyntax.NameColon.IsMissing)
						{
							return true;
						}
						if (attributeArgumentSyntax.NameColon.Name.Identifier.Text.Equals("tag", StringComparison.InvariantCultureIgnoreCase))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public static SyntaxTree Rewrite(CSharpCompilation compilation, SyntaxTree syntaxTree)
		{
			SyntaxNode root = new ProtoTagRewriter(compilation, syntaxTree).Visit(syntaxTree.GetRoot());
			return syntaxTree.WithRootAndOptions(root, syntaxTree.Options);
		}
	}
}
