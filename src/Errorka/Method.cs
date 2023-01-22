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

    public IEnumerable<string> Areas => symbol
        .GetAttributes()
        .Where(attribute => attribute.AttributeClass?.ToString() == "Errorka.AreaAttribute")
        .Select(
            attribute => attribute.ConstructorArguments is { IsDefaultOrEmpty: false } arguments
                ? arguments[0].Value as string
                : null
        ).NotNull();
}