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
    internal sealed class AreaAttribute : global::System.Attribute
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
                try
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
                            .GroupBy(method => method.Name, StringComparer.Ordinal)
                            .Indexed(startIndex: 1)
                            .Select(
                                group => new Variant(
                                    group.Item.Key,
                                    group.Index,
                                    group.Item.AsEnumerable(),
                                    group.Item.Select(method => method.ReturnType).AllEquals(SymbolEqualityComparer.Default)
                                        ? group.Item.First().ReturnType.SpecialType == SpecialType.System_Void
                                            ? compilation.GetSpecialType(SpecialType.System_Object)
                                            : group.Item.First().ReturnType
                                        : compilation.GetSpecialType(SpecialType.System_Object),
                                    new HashSet<string>(
                                        group.Item.SelectMany(method => method.Areas),
                                        StringComparer.Ordinal
                                    )
                                )
                            ).ToList();

                        var maxIndex = variants.Count > 0
                            ? variants.Max(variant => variant.Index)
                            : 1;
                        var decreasingAreas = variants
                            .SelectMany(variant => variant.Areas, (variant, area) => (variant, Area: area))
                            .GroupBy(
                                pair => pair.Area,
                                pair => pair.variant,
                                (area, areaVariants) =>
                                {
                                    var variantList = areaVariants.ToList();
                                    var indexList = variantList
                                        .Select(variant => variant.Index)
                                        .Distinct()
                                        .ToList();
                                    var set = new IntSet(maxIndex, indexList);
                                    return (Area: area, indexList.Count, Variants: variantList, Set: set);
                                }
                            ).OrderByDescending(triple => triple.Count).ThenBy(triple => triple.Area, StringComparer.Ordinal)
                            .ToList();

                        var areaGoes = Yield().ToLookup(x => x.FromArea, x => x.ToArea);

                        IEnumerable<(string FromArea, string ToArea)> Yield()
                        {
                            for (var i = 0; i < decreasingAreas.Count - 1; i++)
                            {
                                var aSet = decreasingAreas[i].Set;
                                var aArea = decreasingAreas[i].Area;
                                for (var j = i + 1; j < decreasingAreas.Count; j++)
                                {
                                    var bSet = decreasingAreas[j].Set;
                                    if (aSet.IsSuperset(bSet))
                                    {
                                        var bArea = decreasingAreas[j].Area;
                                        yield return (bArea, aArea);

                                        if (decreasingAreas[i].Count == decreasingAreas[j].Count)
                                        {
                                            yield return (aArea, bArea);
                                        }
                                    }
                                }
                            }
                        }

                        output.Clear();
                        PrintCodeEnum();
                        context.AddSource($"{part.Symbol}.Code.g.cs", output.ToString());

                        output.Clear();
                        PrintResultClass();
                        context.AddSource($"{part.Symbol}.Result.g.cs", output.ToString());

                        foreach (var (area, _, variantList, _) in decreasingAreas)
                        {
                            output.Clear();
                            PrintArea(area, variantList);
                            context.AddSource($"{part.Symbol}.Areas.{area}.g.cs", output.ToString());
                        }

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

                                        Creation(variants, output, part, "Result");
                                    }
                                }
                            }
                        }

                        void PrintArea(string area, List<Variant> variantList)
                        {
                            using (output.OpenNamespace(part.Symbol.ContainingNamespace))
                            {
                                using (output.OpenPartialClass(part.Symbol))
                                {
                                    using (output.OpenStruct(area))
                                    {
                                        output.Constructor(area, part.Symbol);
                                        output.GetAutoProperty($"global::{part.Symbol}.Code", "Code");
                                        output.GetAutoProperty("global::System.Object", "Value");

                                        Creation(variants, output, part, area);

                                        using (output.StartLine())
                                        {
                                            output.Write("public ");
                                            ContentFactory.From(part.Symbol).Write(output);
                                            output.Write(".Result ToResult()");
                                        }

                                        using (output.OpenBlock())
                                        {
                                            using (output.StartLine())
                                            {
                                                output.Write("return new ");
                                                ContentFactory.From(part.Symbol).Write(output);
                                                output.Write(".Result(this.Code, this.Value);");
                                            }
                                        }

                                        foreach (var next in areaGoes[area])
                                        {
                                            using (output.StartLine())
                                            {
                                                output.Write("public ");
                                                ContentFactory.From(part.Symbol).Write(output);
                                                output.Write(".@");
                                                output.Write(next);
                                                output.Write(" @To");
                                                output.Write(next);
                                                output.Write("()");
                                            }

                                            using (output.OpenBlock())
                                            {
                                                using (output.StartLine())
                                                {
                                                    output.Write("return new ");
                                                    ContentFactory.From(part.Symbol).Write(output);
                                                    output.Write(".@");
                                                    output.Write(next);
                                                    output.Write("(this.Code, this.Value);");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    var descriptor = new DiagnosticDescriptor(
                        "Errorka001",
                        "Internal source generator error",
                        "Exception occured during source generation {0} with stacktrace {1}",
                        "Errorka.InternalError",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true
                    );
                    context.ReportDiagnostic(
                        Diagnostic.Create(descriptor, location: null, e, e.StackTrace)
                    );
                }
            }
        );

        void Creation(List<Variant> variants, Output output, ClassDeclaration.Parts part, string returnType)
        {
            foreach (var variant in variants)
            {
                foreach (var method in variant.Methods)
                {
                    using (output.StartLine())
                    {
                        output.Write("public static ");
                        ContentFactory.From(part.Symbol).Write(output);
                        output.Write(".@");
                        output.Write(returnType);
                        output.Write(" ");
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
                            ContentFactory.From(part.Symbol).Write(output);
                            output.Write(".@");
                            output.Write(returnType);
                            output.Write("(");
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

                using (output.StartLine())
                {
                    output.Write("public global::System.Boolean Is");
                    output.Write(variant.Name);
                    output.Write("([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out ");
                    ContentFactory.From(variant.Type).Write(output);
                    output.Write(" value)");
                }

                using (output.OpenBlock())
                {
                    using (output.StartLine())
                    {
                        output.Write("value = this.Value is ");
                        ContentFactory.From(variant.Type).Write(output);
                        output.Write(" ? (");
                        ContentFactory.From(variant.Type).Write(output);
                        output.Write(")this.Value : default;");
                    }

                    using (output.StartLine())
                    {
                        output.Write("return this.Code == ");
                        ContentFactory.From(part.Symbol).Write(output);
                        output.Write(".Code.");
                        output.Write(variant.Name);
                        output.Write(";");
                    }
                }
            }
        }
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

internal sealed class Variant
{
    public Variant(string name, int index, IEnumerable<Method> methods, ITypeSymbol type, HashSet<string> areas)
    {
        Name = name;
        Index = index;
        Methods = methods;
        Type = type;
        Areas = areas;
    }

    public string Name { get; }
    public int Index { get; }
    public IEnumerable<Method> Methods { get; }
    public ITypeSymbol Type { get; }
    public HashSet<string> Areas { get; }
}