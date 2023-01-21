using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Errorka;

internal sealed class ClassDeclaration
{
    private readonly ClassDeclarationSyntax declaration;
    private readonly SemanticModel model;
    private readonly INamedTypeSymbol symbol;

    private ClassDeclaration(
        INamedTypeSymbol symbol,
        SemanticModel model,
        ClassDeclarationSyntax declaration)
    {
        this.symbol = symbol;
        this.model = model;
        this.declaration = declaration;
    }

    public static ClassDeclaration? TryCreate(Compilation compilation, ClassDeclarationSyntax declaration)
    {
        var model = compilation.GetSemanticModel(declaration.SyntaxTree);
        if (model.GetDeclaredSymbol(declaration) is { } symbol)
        {
            return new ClassDeclaration(symbol, model, declaration);
        }

        return null;
    }

    public sealed class Parts
    {
        private static readonly IEqualityComparer<INamedTypeSymbol> equality = SymbolEqualityComparer.Default;
        private readonly ImmutableArray<ClassDeclaration> declarations;

        public Parts(INamedTypeSymbol symbol, ImmutableArray<ClassDeclaration> declarations)
        {
            Symbol = symbol;
            this.declarations = declarations;
        }

        public INamedTypeSymbol Symbol { get; }

        public static IEnumerable<Parts> Join(IEnumerable<ClassDeclaration> declarations) => declarations
            .ToLookup(declaration => declaration.symbol, equality)
            .Select(group => new Parts(group.Key, group.ToImmutableArray()));

        public IEnumerable<Method> Methods()
        {
            foreach (var declaration in declarations)
            {
                foreach (var member in declaration.declaration.Members)
                {
                    var declaredSymbol = declaration.model.GetDeclaredSymbol(member);
                    if (declaredSymbol is IMethodSymbol method)
                    {
                        if (method.IsPartialDefinition)
                        {
                            continue;
                        }

                        if (method.IsStatic && method.MethodKind == MethodKind.Ordinary)
                        {
                            yield return new Method(method);
                        }
                    }
                }
            }
        }
    }
}