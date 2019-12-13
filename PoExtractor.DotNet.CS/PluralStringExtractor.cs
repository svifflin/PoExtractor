using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PoExtractor.Core;
using PoExtractor.Core.Contracts;

namespace PoExtractor.DotNet.CS {
    /// <summary>
    /// Extracts <see cref="LocalizableStringOccurence"/> with the singular text from the C# AST node
    /// </summary>
    /// <remarks>
    /// The localizable string is identified by the name convention - T.Plural(count, "1 book", "{0} books")
    /// </remarks>
    public class PluralStringExtractor : LocalizableStringExtractor<SyntaxNode> {
        public PluralStringExtractor(IMetadataProvider<SyntaxNode> metadataProvider) : base(metadataProvider) {
        }

        public override bool TryExtract(SyntaxNode node, out LocalizableStringOccurence result) {
            result = null;

            dynamic accessor;
            SimpleNameSyntax identifierName;
            var invocation = node as InvocationExpressionSyntax;
            switch (invocation?.Expression)
            {
                case InvocationExpressionSyntax syntax:
                    accessor = syntax;
                    identifierName = accessor.Expression as IdentifierNameSyntax ??
                                     (accessor.Expression as MemberAccessExpressionSyntax)?.Name as IdentifierNameSyntax;
                    break;
                case MemberAccessExpressionSyntax syntax:
                    accessor = syntax;
                    identifierName = accessor.Expression as IdentifierNameSyntax ??
                                     (accessor.Expression as MemberAccessExpressionSyntax)?.Name as IdentifierNameSyntax;
                    break;
                default:
                    return false;
            }
            if (identifierName != null &&
                LocalizerAccessors.LocalizerIdentifiers.Contains(identifierName.Identifier.Text) &&
                accessor.Name.Identifier.Text == "Plural")
            {
                var arguments = invocation.ArgumentList.Arguments;
                if (arguments.Count < 2)
                    return false;

                result = arguments[0].Expression is not LiteralExpressionSyntax ? FromOrchardCore(node, arguments) : FromOrchardCms(node, arguments);
            }

            return result != null;
        }
        
        private LocalizableStringOccurence FromOrchardCms(SyntaxNode node, SeparatedSyntaxList<ArgumentSyntax> arguments)
        {
            LocalizableStringOccurence result = null;

            if (arguments.Count >= 2 &&
                arguments[0].Expression is ArrayCreationExpressionSyntax array) {
                if (array.Type.ElementType is PredefinedTypeSyntax arrayType &&
                    arrayType.Keyword.Text == "string" &&
                    array.Initializer.Expressions.Count >= 2 &&
                    array.Initializer.Expressions[0] is LiteralExpressionSyntax singularLiteral && singularLiteral.IsKind(SyntaxKind.StringLiteralExpression) &&
                    array.Initializer.Expressions[1] is LiteralExpressionSyntax pluralLiteral && pluralLiteral.IsKind(SyntaxKind.StringLiteralExpression)) {

                    result = this.CreateLocalizedString(singularLiteral.Token.ValueText, pluralLiteral.Token.ValueText, node);
                }
            } else {
                if (arguments.Count >= 3 &&
                    arguments[^3].Expression is LiteralExpressionSyntax singularLiteral && singularLiteral.IsKind(SyntaxKind.StringLiteralExpression) &&
                    arguments[^2].Expression is LiteralExpressionSyntax pluralLiteral && pluralLiteral.IsKind(SyntaxKind.StringLiteralExpression)) {

                    result = this.CreateLocalizedString(singularLiteral.Token.ValueText, pluralLiteral.Token.ValueText, node);
                }
            }

            return result;
        }
        
        private LocalizableStringOccurence FromOrchardCore(SyntaxNode node, SeparatedSyntaxList<ArgumentSyntax> arguments)
        {
            LocalizableStringOccurence result = null;

            if (arguments.Count >= 2 &&
                arguments[1].Expression is ArrayCreationExpressionSyntax array) {
                if (array.Type.ElementType is PredefinedTypeSyntax arrayType &&
                    arrayType.Keyword.Text == "string" &&
                    array.Initializer.Expressions.Count >= 2 &&
                    array.Initializer.Expressions[0] is LiteralExpressionSyntax singularLiteral && singularLiteral.IsKind(SyntaxKind.StringLiteralExpression) &&
                    array.Initializer.Expressions[1] is LiteralExpressionSyntax pluralLiteral && pluralLiteral.IsKind(SyntaxKind.StringLiteralExpression)) {

                    result = this.CreateLocalizedString(singularLiteral.Token.ValueText, pluralLiteral.Token.ValueText, node);
                }
            } else {
                if (arguments.Count >= 3 &&
                    arguments[1].Expression is LiteralExpressionSyntax singularLiteral && singularLiteral.IsKind(SyntaxKind.StringLiteralExpression) &&
                    arguments[2].Expression is LiteralExpressionSyntax pluralLiteral && pluralLiteral.IsKind(SyntaxKind.StringLiteralExpression)) {

                    result = this.CreateLocalizedString(singularLiteral.Token.ValueText, pluralLiteral.Token.ValueText, node);
                }
            }

            return result;
        }
    }
}
