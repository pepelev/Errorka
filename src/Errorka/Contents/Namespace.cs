using Microsoft.CodeAnalysis;

namespace Errorka.Contents;

internal readonly struct Namespace : Content
{
    private readonly INamespaceSymbol symbol;
    private readonly bool globalPrefix;

    public Namespace(INamespaceSymbol symbol, bool globalPrefix)
    {
        this.symbol = symbol;
        this.globalPrefix = globalPrefix;
    }

    public void Write(Output output)
    {
        var prefix = globalPrefix;
        Print(symbol);

        void Print(INamespaceSymbol @namespace)
        {
            var outer = @namespace.ContainingNamespace;
            if (outer == null || outer.IsGlobalNamespace)
            {
                if (prefix)
                {
                    output.Write("global::");
                }

                output.Write("@");
                output.Write(@namespace.Name);
            }
            else
            {
                Print(outer);
                output.Write(".@");
                output.Write(@namespace.Name);
            }
        }
    }

    public override string ToString() => this.Print();
}