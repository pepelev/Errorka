using Errorka.Contents;
using Microsoft.CodeAnalysis;

namespace Errorka.Code;

internal readonly struct Type<TName> : IDisposable
    where TName : Content
{
    private readonly TName name;
    private readonly Output output;
    private readonly Output.Block block;

    private Type(TName name, Output output, Output.Block block)
    {
        this.name = name;
        this.block = block;
        this.output = output;
    }

    public static Type<TName> Open(Output output, string modifiers, string type, TName name)
    {
        using (output.StartLine())
        {
            output.Write(modifiers);
            output.Write(" ");
            output.Write(type);
            output.Write(" ");
            name.Write(output);
        }

        var block = output.OpenBlock();
        return new Type<TName>(name, output, block);
    }

    public void Constructor(INamedTypeSymbol rootType)
    {
        var attribute = ContentFactory.From("[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]");
        output.WriteLine(attribute);
        output.Write("internal ");
        name.Write(output);
        var parameters = ContentFactory.Parentheses(
            ContentFactory.CommaSeparated(
                ContentFactory.Parameter(
                    ContentFactory.Dotted(
                        ContentFactory.From(rootType),
                        ContentFactory.From("Code")
                    ),
                    ContentFactory.From("code")
                ),
                ContentFactory.From("global::System.Object value")
            )
        );
        output.WriteLine(parameters);
        using (output.OpenBlock())
        {
            output.WriteLine(ContentFactory.From("this.Code = code;"));
            output.WriteLine(ContentFactory.From("this.Value = value;"));
        }
    }

    public void GetAutoProperty<TType, TPropertyName>(TType type, TPropertyName propertyName)
        where TType : Content
        where TPropertyName : Content
    {
        using (output.StartLine())
        {
            output.Write("public ");
            type.Write(output);
            output.Write(" ");
            propertyName.Write(output);
            output.Write(" { get; }");
        }
    }

    public void Dispose()
    {
        block.Dispose();
    }
}