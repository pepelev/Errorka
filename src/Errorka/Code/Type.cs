using Errorka.Contents;
using Microsoft.CodeAnalysis;

namespace Errorka.Code;

internal readonly struct Type<T> : IDisposable
    where T : Content
{
    private readonly T name;
    private readonly Output output;
    private readonly Output.Block block;

    private Type(T name, Output output, Output.Block block)
    {
        this.name = name;
        this.block = block;
        this.output = output;
    }

    public static Type<T> Open(Output output, string modifiers, string type, T name)
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
        return new Type<T>(name, output, block);
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

    public void Dispose()
    {
        block.Dispose();
    }
}