namespace Errorka;

internal static class SequenceExtensions
{
    public static IEnumerable<(T Item, int Index)> Indexed<T>(this IEnumerable<T> sequence, int startIndex) =>
        sequence.Select((item, index) => (item, index + startIndex));

    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> sequence)
        where T : class
    {
        return sequence.Where(item => item != null)!;
    }

    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> sequence) => new(sequence);
}