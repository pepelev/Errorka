using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Errorka.Contents;

internal static class ContentFactory
{
    public static Dotted3<T1, T2, T3> From<T1, T2, T3>(T1 first, T2 second, T3 third)
        where T1 : Content
        where T2 : Content
        where T3 : Content
        => new(first, second, third);
    
    public static Dotted2<T1, T2> From<T1, T2>(T1 first, T2 second)
        where T1 : Content
        where T2 : Content
        => new(first, second);

    public static Verbatim From(string value) => new(value);
    public static Type From(ITypeSymbol symbol) => new(symbol);
    public static Namespace Declaration(INamespaceSymbol symbol) => new(symbol, globalPrefix: false);

    public static ParametersArguments ParametersArguments(ImmutableArray<IParameterSymbol> parameters)
        => new(parameters);

    public static Call<T1, T2> Call<T1, T2>(T1 method, T2 arguments)
        where T1 : Content
        where T2 : Content
        => new(method, arguments);
}