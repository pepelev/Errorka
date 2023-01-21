using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Errorka.Contents;

internal readonly struct ParametersArguments : Content
{
    private readonly ImmutableArray<IParameterSymbol> parameters;

    public ParametersArguments(ImmutableArray<IParameterSymbol> parameters)
    {
        this.parameters = parameters;
    }

    public void Write(Output output)
    {
        var arguments = output.Arguments();
        foreach (var parameter in parameters)
        {
            arguments.Append(ContentFactory.From(parameter.Name));
        }
    }
}