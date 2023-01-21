using System.Globalization;
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

                var declarations = classes.Distinct().Select(
                    declaration => ClassDeclaration.TryCreate(compilation, declaration)!
                ).Where(declaration => declaration != null);
                var parts = ClassDeclaration.Parts.Join(declarations).ToList();

                var output = new Output();
                foreach (var part in parts)
                {
                    var index = Index().ToList();

                    output.Clear();
                    PrintCodeEnum();
                    context.AddSource($"{part.Symbol}.Code.g.cs", output.ToString());

                    output.Clear();
                    PrintResultClass();
                    context.AddSource($"{part.Symbol}.Result.g.cs", output.ToString());

                    void PrintCodeEnum()
                    {
                        using (output.OpenNamespace(part.Symbol.ContainingNamespace))
                        {
                            using (output.OpenPartialClass(part.Symbol))
                            {
                                using (output.OpenEnum("Code"))
                                {
                                    foreach (var (_, name, code) in index)
                                    {
                                        output.EnumMember(name, code);
                                    }
                                }
                            }
                        }
                    }

                    void PrintResultClass()
                    {
                        using (output.OpenNamespace(part.Symbol.ContainingNamespace))
                        {
                            using (output.OpenPartialClass(part.Symbol))
                            {
                                using (output.OpenStruct("Result"))
                                {
                                    output.Constructor("Result", part.Symbol);
                                    output.GetAutoProperty($"global::{part.Symbol}.Code", "Code");
                                    output.GetAutoProperty("global::System.Object", "Value");

                                    foreach (var (method, name, code) in index)
                                    {
                                        output.Line($"public static global::{part.Symbol}.Result {name}() {{ return new global::{part.Symbol}.Result(global::{part.Symbol}.Code.{name}, global::{part.Symbol}.{name}()); }}");

                                        output.Write("public global::System.Boolean Is");
                                        output.Write(name);
                                        output.Write("([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ");
                                        output.Write(method.ReturnType);
                                        output.Write(" value)");
                                        output.End();

                                        using (output.OpenBlock())
                                        {
                                            output.Write("value = this.Value as ");
                                            output.Write(method.ReturnType);
                                            output.Write(";");
                                            output.End();
                                            output.Write("return this.Code == ");
                                            output.Write(part.Symbol);
                                            output.Write(".Code.");
                                            output.Write(name);
                                            output.Write(";");
                                            output.End();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    IEnumerable<(Method Method, string Name, int Code)> Index()
                    {
                        var names = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
                        var code = 1;
                        foreach (var method in part.Methods())
                        {
                            if (names.TryGetValue(method.Name, out var timesUsed))
                            {
                                var guessNumber = (timesUsed + 1).ToString(CultureInfo.InvariantCulture);
                                var guessName = $"{method.Name}_{guessNumber}";
                                while (names.ContainsKey(guessName))
                                {
                                    guessName += "x";
                                }

                                names[method.Name] = timesUsed + 1;
                                names[guessName] = 1;
                                yield return (method, guessName, code);
                            }
                            else
                            {
                                names[method.Name] = 1;
                                yield return (method, method.Name, code);
                            }

                            code++;
                        }
                    }
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