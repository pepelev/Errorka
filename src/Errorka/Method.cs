using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Errorka;

internal readonly struct IntSet
{
    private readonly ulong[] bits;

    private IntSet(ulong[] bits)
    {
        this.bits = bits;
    }

    public IntSet(int maxIndex, IEnumerable<int> indexes)
    {
        var builder = new Builder(maxIndex);
        foreach (var index in indexes)
        {
            builder.Set(index);
        }

        this = builder.Build();
    }

    public bool IsSuperset(IntSet another)
    {
        for (var i = 0; i < another.bits.Length; i++)
        {
            if ((another.bits[i] & bits[i]) != another.bits[i])
            {
                return false;
            }
        }

        return true;
    }

    public readonly struct Builder
    {
        private const uint bitsInUlong = sizeof(ulong) * 8;
        private readonly ulong[] bits;

        public Builder(int maxIndex)
        {
            bits = new ulong[maxIndex / bitsInUlong + 1];
        }

        public void Set(int index)
        {
            var uIndex = (uint)index;
            var ulongIndex = uIndex / bitsInUlong;
            var bitIndex = (int)(uIndex % bitsInUlong);
            var bit = 1UL << bitIndex;
            bits[ulongIndex] |= bit;
        }

        public IntSet Build() => new(bits);
    }
}

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
        .Select(attribute => attribute.ConstructorArguments[0].Value as string)
        .NotNull();
}