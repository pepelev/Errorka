using Microsoft.CodeAnalysis;

namespace Errorka;

internal sealed class Variant
{
    public Variant(string name, int index, IEnumerable<Method> methods, ITypeSymbol type, HashSet<Area> areas)
    {
        Name = name;
        Index = index;
        Methods = methods;
        Type = type;
        Areas = areas;
    }

    public string Name { get; }
    public int Index { get; }
    public IEnumerable<Method> Methods { get; }
    public ITypeSymbol Type { get; }
    public HashSet<Area> Areas { get; }
}