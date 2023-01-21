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
        PrintType(symbol);

        void PrintNamespace(INamespaceSymbol @namespace)
        {
            var outer = @namespace.ContainingNamespace;
            if (outer == null || outer.IsGlobalNamespace)
            {
                output.Write("global::");
                output.Write(@namespace.Name);
            }
            else
            {
                PrintNamespace(outer);
                output.Write(".");
                output.Write(@namespace.Name);
            }
        }

        void PrintType(ITypeSymbol type)
        {
            var outer = type.ContainingType;
            if (outer == null)
            {
                PrintNamespace(type.ContainingNamespace);
                output.Write(".");
                output.Write(type.Name);
                return;
            }

            PrintType(outer);
            output.Write(".");
            output.Write(type.Name);
        }
    }
}