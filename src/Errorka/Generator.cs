using System.Text;
using Errorka.Code;
using Errorka.Contents;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Errorka;

[Generator]
public sealed class Generator : IIncrementalGenerator
{
    private volatile Output? pooled;

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
                Output? output = null;
                try
                {
                    var (classes, compilation) = entry;
                    if (classes.IsDefaultOrEmpty)
                    {
                        return;
                    }

                    var declarations = classes
                        .Distinct()
                        .Select(declaration => ClassDeclaration.TryCreate(compilation, declaration))
                        .NotNull();
                    var parts = ClassDeclaration.Parts.Join(declarations).ToList();

                    output = Interlocked.Exchange(ref pooled, null) ?? new Output(new(80 * 1024 / sizeof(char)));
                    foreach (var part in parts)
                    {
                        ITypeSymbol CommonType(IEnumerable<ITypeSymbol> types)
                        {
                            var symbols = types
                                .Distinct<ITypeSymbol>(SymbolEqualityComparer.Default)
                                .Take(2)
                                .ToArray();
                            if (symbols.Length == 1)
                            {
                                return symbols[0].SpecialType == SpecialType.System_Void
                                    ? compilation.GetSpecialType(SpecialType.System_Object)
                                    : symbols[0];
                            }

                            return compilation.GetSpecialType(SpecialType.System_Object);
                        }

                        context.CancellationToken.ThrowIfCancellationRequested();
                        var variants = part.Methods()
                            .GroupBy(method => method.Name, StringComparer.Ordinal)
                            .Indexed(startIndex: 1)
                            .Select(
                                group => new Variant(
                                    group.Item.Key,
                                    group.Index,
                                    group.Item.AsEnumerable(),
                                    CommonType(group.Item.Select(method => method.ReturnType)),
                                    group.Item.SelectMany(method => method.Areas).ToHashSet()
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
                            ).OrderByDescending(triple => triple.Count).ThenBy(triple => triple.Area)
                            .ToList();

                        var areaGoes = Yield().ToLookup(x => x.From, x => x.To);

                        IEnumerable<(Area From, Area To)> Yield()
                        {
                            for (var i = 0; i < decreasingAreas.Count - 1; i++)
                            {
                                context.CancellationToken.ThrowIfCancellationRequested();
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
                            using (output.OpenNamespace(ContentFactory.Declaration(part.Symbol.ContainingNamespace)))
                            {
                                using (Class.Open(output, "partial", ContentFactory.From(part.Symbol.Name).VerbatimPrefixed()))
                                {
                                    using (var @enum = Code.Enum.Open(output, "public", ContentFactory.From("Code")))
                                    {
                                        foreach (var variant in variants)
                                        {
                                            var name = ContentFactory.From(variant.Name).VerbatimPrefixed();
                                            @enum.Member(name, variant.Index);
                                        }
                                    }
                                }
                            }
                        }

                        void PrintResultClass()
                        {
                            using (output.OpenNamespace(ContentFactory.Declaration(part.Symbol.ContainingNamespace)))
                            {
                                using (Class.Open(output, "partial", ContentFactory.From(part.Symbol.Name).VerbatimPrefixed()))
                                {
                                    using (var @struct = Struct.Open(output, "public readonly", ContentFactory.From("Result")))
                                    {
                                        Creation(variants, output, part, "Result", @struct, context.CancellationToken);
                                    }
                                }
                            }
                        }

                        void PrintArea(Area area, List<Variant> variantList)
                        {
                            using (output.OpenNamespace(ContentFactory.Declaration(part.Symbol.ContainingNamespace)))
                            {
                                using (Class.Open(output, "partial", ContentFactory.From(part.Symbol.Name).VerbatimPrefixed()))
                                {
                                    using (var @struct = Struct.Open(output, "public readonly", ContentFactory.From(area.Name).VerbatimPrefixed()))
                                    {
                                        Creation(variantList, output, part, area.Name, @struct, context.CancellationToken);

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

                                        using (output.StartLine())
                                        {
                                            output.Write("public static implicit operator ");
                                            ContentFactory.Dotted(
                                                ContentFactory.From(part.Symbol),
                                                ContentFactory.From("Result")
                                            ).Write(output);
                                            ContentFactory.Parentheses(
                                                ContentFactory.Parameter(
                                                    ContentFactory.From(area.Name).VerbatimPrefixed(),
                                                    ContentFactory.From("area")
                                                )
                                            ).Write(output);
                                        }

                                        using (output.OpenBlock())
                                        {
                                            output.WriteLine(ContentFactory.From("return area.ToResult();"));
                                        }

                                        foreach (var next in areaGoes[area])
                                        {
                                            using (output.StartLine())
                                            {
                                                output.Write("public ");
                                                ContentFactory.From(part.Symbol).Write(output);
                                                output.Write(".@");
                                                output.Write(next.Name);
                                                output.Write(" @To");
                                                output.Write(next.Name);
                                                output.Write("()");
                                            }

                                            using (output.OpenBlock())
                                            {
                                                using (output.StartLine())
                                                {
                                                    output.Write("return new ");
                                                    ContentFactory.From(part.Symbol).Write(output);
                                                    output.Write(".@");
                                                    output.Write(next.Name);
                                                    output.Write("(this.Code, this.Value);");
                                                }
                                            }

                                            using (output.StartLine())
                                            {
                                                output.Write("public static implicit operator ");
                                                ContentFactory.Dotted(
                                                    ContentFactory.From(part.Symbol),
                                                    ContentFactory.From(next.Name).VerbatimPrefixed()
                                                ).Write(output);
                                                ContentFactory.Parentheses(
                                                    ContentFactory.Parameter(
                                                        ContentFactory.From(area.Name).VerbatimPrefixed(),
                                                        ContentFactory.From("area")
                                                    )
                                                ).Write(output);
                                            }

                                            using (output.OpenBlock())
                                            {
                                                using (output.StartLine())
                                                {
                                                    output.Write("return area.@To");
                                                    output.Write(next.Name);
                                                    output.Write("();");
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
                        "Exception occured during source generation {0}",
                        "Errorka.InternalError",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true
                    );
                    context.ReportDiagnostic(
                        Diagnostic.Create(descriptor, location: null, e)
                    );
                }
                finally
                {
                    if (output != null)
                    {
                        Interlocked.CompareExchange(
                            ref pooled,
                            value: output,
                            comparand: null
                        );
                    }
                }
            }
        );

        void Creation<T>(
            List<Variant> variants,
            Output output,
            ClassDeclaration.Parts part,
            string returnType,
            Type<T> @struct,
            CancellationToken token)
            where T : Content
        {
            @struct.Constructor(part.Symbol);
            @struct.GetAutoProperty(
                ContentFactory.Dotted(
                    ContentFactory.From(part.Symbol),
                    ContentFactory.From("Code")
                ),
                ContentFactory.From("Code")
            );
            @struct.GetAutoProperty(
                ContentFactory.From("global::System.Object"),
                ContentFactory.From("Value")
            );

            foreach (var variant in variants)
            {
                token.ThrowIfCancellationRequested();
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
                        var parameters = output.CommaSeparated();
                        foreach (var parameter in method.Parameters)
                        {
                            parameters.Append(ContentFactory.Parameter(parameter));
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
                            var arguments = output.CommaSeparated();
                            arguments.Append(
                                ContentFactory.Dotted(
                                    ContentFactory.From(part.Symbol),
                                    ContentFactory.From("Code"),
                                    ContentFactory.From(variant.Name)
                                )
                            );
                            arguments.Append(
                                ContentFactory.Call(
                                    ContentFactory.Dotted(
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

            if (variants.Count <= 1)
                return;

            Match(output, variants, part);
            MatchWithDefault(output, variants, part);
        }
    }

    private static void Match(Output output, List<Variant> variants, ClassDeclaration.Parts part)
    {
        using (output.StartLine())
        {
            output.Write("public T Match<T>(");
            var parameter = output.CommaSeparated();
            foreach (var variant in variants)
            {
                parameter.Append(
                    ContentFactory.Parameter(
                        ContentFactory.Generic(
                            ContentFactory.From("global::System.Func"),
                            ContentFactory.CommaSeparated(
                                ContentFactory.From(variant.Type),
                                ContentFactory.From("T")
                            )
                        ),
                        ContentFactory.From(variant.Name).VerbatimPrefixed()
                    )
                );
            }

            output.Write(")");
        }

        using (output.OpenBlock())
        {
            output.WriteLine(ContentFactory.From("switch (this.Code)"));
            using (output.OpenBlock())
            {
                foreach (var variant in variants)
                {
                    using (output.StartLine())
                    {
                        output.Write("case ");
                        ContentFactory.Dotted(
                            ContentFactory.From(part.Symbol),
                            ContentFactory.From("Code"),
                            ContentFactory.From(variant.Name).VerbatimPrefixed()
                        ).Write(output);
                        output.Write(": return ");
                        ContentFactory.Call(
                            ContentFactory.From(variant.Name).VerbatimPrefixed(),
                            ContentFactory.SafeCast(
                                ContentFactory.From("this.Value"),
                                ContentFactory.From(variant.Type)
                            )
                        ).Write(output);
                        output.Write(";");
                    }
                }

                output.WriteLine(ContentFactory.From("default: throw new Exception(\"Instance is broken. Code: \" + this.Code);"));
            }
        }
    }

    private static void MatchWithDefault(Output output, List<Variant> variants, ClassDeclaration.Parts part)
    {
        using (output.StartLine())
        {
            output.Write("public T Match<T>(");
            var parameter = output.CommaSeparated();
            parameter.Append(ContentFactory.From("T @default"));
            foreach (var variant in variants)
            {
                parameter.Append(
                    ContentFactory.Sequence(
                        ContentFactory.Parameter(
                            ContentFactory.Generic(
                                ContentFactory.From("global::System.Func"),
                                ContentFactory.CommaSeparated(
                                    ContentFactory.From(variant.Type),
                                    ContentFactory.From("T")
                                )
                            ),
                            ContentFactory.From(variant.Name).VerbatimPrefixed()
                        ),
                        ContentFactory.From(" = null")
                    )
                );
            }

            output.Write(")");
        }

        using (output.OpenBlock())
        {
            output.WriteLine(ContentFactory.From("switch (this.Code)"));
            using (output.OpenBlock())
            {
                foreach (var variant in variants)
                {
                    using (output.StartLine())
                    {
                        output.Write("case ");
                        ContentFactory.Dotted(
                            ContentFactory.From(part.Symbol),
                            ContentFactory.From("Code"),
                            ContentFactory.From(variant.Name).VerbatimPrefixed()
                        ).Write(output);
                        output.Write(": return ");
                        ContentFactory.From(variant.Name).VerbatimPrefixed().Write(output);
                        output.Write(" == null ? @default : ");
                        ContentFactory.Call(
                            ContentFactory.From(variant.Name).VerbatimPrefixed(),
                            ContentFactory.SafeCast(
                                ContentFactory.From("this.Value"),
                                ContentFactory.From(variant.Type)
                            )
                        ).Write(output);
                        output.Write(";");
                    }
                }

                output.WriteLine(ContentFactory.From("default: throw new Exception(\"Instance is broken. Code: \" + this.Code);"));
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