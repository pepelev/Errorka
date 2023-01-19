using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Errorka;

[Generator]
public sealed class Generator : IIncrementalGenerator
{
    private const string Attributes = @"
namespace Errorka
{
    [global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class ResultAttribute : global::System.Attribute
    {
    }

    [global::System.AttributeUsage(global::System.AttributeTargets.Method, AllowMultiple = true)]
    public sealed class AreaAttribute : global::System.Attribute
    {
        public AreaAttribute(global::System.String name)
        {
            Name = name;
        }

        public global::System.String Name { get; }
    }
}";

    public void Initialize(IncrementalGeneratorInitializationContext generatorInitializationContext)
    {
        generatorInitializationContext.RegisterPostInitializationOutput(
            context => context.AddSource(
                "ErrorkaAttributes.g.cs",
                SourceText.From(Attributes, Encoding.UTF8)
            )
        );

        var resultClasses = generatorInitializationContext.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (node, _) => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 },
                transform: (syntaxContext, token) =>
                {
                    var declaration = (ClassDeclarationSyntax)syntaxContext.Node;
                    var semantic = syntaxContext.SemanticModel;
                    return HasAttribute(declaration, "Errorka.ResultAttribute", semantic, token)
                        ? declaration
                        : null!;
                }
            ).Where(x => x != null)
            .Collect();

        var data = resultClasses.Combine(generatorInitializationContext.CompilationProvider);


        generatorInitializationContext.RegisterSourceOutput(
            data,
            (context, entry) =>
            {
                var (classes, compilation) = entry;
                if (classes.IsDefaultOrEmpty)
                {
                    return;
                }

                var classSymbols = classes.Distinct().Select(
                    classDeclaration =>
                    {
                        var model = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                        // todo may return null
                        return model.GetDeclaredSymbol(classDeclaration);
                    }
                ).Distinct(SymbolEqualityComparer.Default);

                foreach (var symbol in classSymbols)
                {
                    var builder = new StringBuilder();
                    builder.AppendLine($"namespace {symbol.ContainingNamespace} {{");

                    builder.AppendLine($"partial class {symbol.Name} {{");

                    builder.AppendLine("public sealed class Result {");

                    builder.AppendLine("private readonly uint code;");

                    builder.AppendLine("}");

                    builder.AppendLine("}");

                    builder.AppendLine("}");

                    var s = builder.ToString();
                    context.AddSource($"{symbol}.g.cs", s);
                }
            }
        );
    }

    private static bool HasAttribute(
        MemberDeclarationSyntax declaration,
        string attributeDisplayString,
        SemanticModel semantic,
        CancellationToken token) => declaration.AttributeLists
        .SelectMany(list => list.Attributes)
        .Any(attributeSyntax =>
        {
            token.ThrowIfCancellationRequested();
            if (semantic.GetSymbolInfo(attributeSyntax).Symbol is IMethodSymbol attributeConstructor)
            {
                var attributeType = attributeConstructor.ContainingType;
                var attributeName = attributeType.ToDisplayString();
                if (attributeName == attributeDisplayString)
                {
                    return true;
                }
            }

            return false;
        });
}