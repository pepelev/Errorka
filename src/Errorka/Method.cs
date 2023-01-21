using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Errorka;

internal sealed class Method
{
    private readonly IMethodSymbol symbol;

    public Method(IMethodSymbol symbol)
    {
        this.symbol = symbol;
    }

    public string Name => symbol.Name;
    public ITypeSymbol ReturnType => symbol.ReturnType;
    public ImmutableArray<IParameterSymbol> Parameters => symbol.Parameters;
}