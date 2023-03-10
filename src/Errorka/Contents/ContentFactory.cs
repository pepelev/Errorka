using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Errorka.Contents;

internal static class ContentFactory
{
    public static Dotted<T1, T2, T3> Dotted<T1, T2, T3>(T1 first, T2 second, T3 third)
        where T1 : Content
        where T2 : Content
        where T3 : Content
        => new(first, second, third);
    
    public static Dotted<T1, T2> Dotted<T1, T2>(T1 first, T2 second)
        where T1 : Content
        where T2 : Content
        => new(first, second);

    public static Verbatim From(string value) => new(value);
    public static Type From(ITypeSymbol symbol) => new(symbol);

    public static Parameter<Type, VerbatimPrefixed<Verbatim>> Parameter(IParameterSymbol parameter) => Parameter(
        From(parameter.Type),
        From(parameter.Name).VerbatimPrefixed()
    );

    public static Parameter<TType, TName> Parameter<TType, TName>(TType type, TName name)
        where TType : Content
        where TName : Content
        => new(type, name);
    public static Namespace Declaration(INamespaceSymbol symbol) => new(symbol, globalPrefix: false);

    public static ParenthesesEnclosed<T> Parentheses<T>(T content) where T : Content => new(content);

    public static CommaSeparatedList<T1, T2> CommaSeparated<T1, T2>(T1 first, T2 second)
        where T1 : Content
        where T2 : Content
        => new(first, second);
    public static ParametersArguments ParametersArguments(ImmutableArray<IParameterSymbol> parameters)
        => new(parameters);

    public static Call<T1, T2> Call<T1, T2>(T1 method, T2 arguments)
        where T1 : Content
        where T2 : Content
        => new(method, arguments);

    public static Generic<TType, TParameters> Generic<TType, TParameters>(TType type, TParameters parameters)
        where TType : Content
        where TParameters : Content
        => new(type, parameters);

    public static SafeCast<TWhat, TTargetType> SafeCast<TWhat, TTargetType>(TWhat what, TTargetType targetType)
        where TWhat : Content
        where TTargetType : Content
        => new(what, targetType);

    public static Sequence<TContent, TSuffix> Sequence<TContent, TSuffix>(TContent content, TSuffix suffix)
        where TContent : Content
        where TSuffix : Content
        => new(content, suffix);
}