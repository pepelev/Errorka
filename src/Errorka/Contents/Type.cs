using Microsoft.CodeAnalysis;

namespace Errorka.Contents;

internal readonly struct Type : Content
{
    private readonly ITypeSymbol symbol;

    public Type(ITypeSymbol symbol)
    {
        this.symbol = symbol;
    }

    public void Write(Output output)
    {
        Print(symbol);

        void Print(ITypeSymbol type)
        {
            var outer = type.ContainingType;
            if (outer == null)
            {
                var @namespace = new Namespace(type.ContainingNamespace, globalPrefix: true);
                @namespace.Write(output);
                output.Write(".@");
                output.Write(type.Name);
                return;
            }

            Print(outer);
            output.Write(".@");
            output.Write(type.Name);
        }
    }

    public override string ToString() => this.Print();
}