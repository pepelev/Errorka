namespace Errorka;

internal readonly struct Area : IEquatable<Area>, IComparable<Area>
{
    public string Name { get; }

    public Area(string name)
    {
        Name = name;
    }

    public bool Equals(Area other) => Name == other.Name;
    public override bool Equals(object? obj) => obj is Area other && Equals(other);
    public override int GetHashCode() => Name.GetHashCode();
    public int CompareTo(Area other) => string.Compare(Name, other.Name, StringComparison.Ordinal);
    public override string ToString() => Name;
}