using System.Text;
using Errorka.Contents;
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
                    var variants = part.Methods()
                        .GroupBy(method => method.Name, StringComparer.InvariantCulture)
                        .Indexed(startIndex: 1)
                        .Select(
                            group => new
                            {
                                Name = group.Item.Key,
                                group.Index,
                                Methods = group.Item.AsEnumerable(),
                                // todo refactor
                                Type = group.Item.Select(method => method.ReturnType).AllEquals(SymbolEqualityComparer.Default)
                                    ? group.Item.First().ReturnType.SpecialType == SpecialType.System_Void
                                        ? compilation.GetSpecialType(SpecialType.System_Object)
                                        : group.Item.First().ReturnType
                                    : compilation.GetSpecialType(SpecialType.System_Object)
                            }
                        ).ToList();

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
                                    foreach (var variant in variants)
                                    {
                                        output.EnumMember(variant.Name, variant.Index);
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

                                    foreach (var variant in variants)
                                    {
                                        foreach (var method in variant.Methods)
                                        {
                                            using (output.StartLine())
                                            {
                                                output.Write("public static ");
                                                output.Write(part.Symbol);
                                                output.Write(".Result ");
                                                output.Write(variant.Name);
                                                output.Write("(");
                                                var parameters = output.Parameters();
                                                foreach (var parameter in method.Parameters)
                                                {
                                                    parameters.Append(parameter);
                                                }
                                                output.Write(")");
                                            }
                                            using (output.OpenBlock())
                                            {
                                                using (output.StartLine())
                                                {
                                                    output.Write("return new ");
                                                    output.Write(part.Symbol);
                                                    output.Write(".Result(");
                                                    var arguments = output.Arguments();
                                                    arguments.Append(
                                                        ContentFactory.From(
                                                            ContentFactory.From(part.Symbol),
                                                            ContentFactory.From("Code"),
                                                            ContentFactory.From(variant.Name)
                                                        )
                                                    );
                                                    arguments.Append(
                                                        ContentFactory.Call(
                                                            ContentFactory.From(
                                                                ContentFactory.From(part.Symbol),
                                                                ContentFactory.From(variant.Name)
                                                            ),
                                                            ContentFactory.ParametersArguments(method.Parameters)
                                                        )
                                                    );
                                                    output.Write(");");
                                                }
                                            }
                                        }

                                        output.Write("public global::System.Boolean Is");
                                        output.Write(variant.Name);
                                        output.Write("([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ");
                                        output.Write(variant.Type);
                                        output.Write(" value)");
                                        output.EndLine();

                                        using (output.OpenBlock())
                                        {
                                            using (output.StartLine())
                                            {
                                                output.Write("value = this.Value is ");
                                                output.Write(variant.Type);
                                            
                                                output.Write(" ? (");
                                                output.Write(variant.Type);
                                                output.Write(")this.Value : default;");
                                            }
                                            using (output.StartLine())
                                            {
                                                output.Write("return this.Code == ");
                                                output.Write(part.Symbol);
                                                output.Write(".Code.");
                                                output.Write(variant.Name);
                                                output.Write(";");
                                            }
                                        }
                                    }
                                }
                            }
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