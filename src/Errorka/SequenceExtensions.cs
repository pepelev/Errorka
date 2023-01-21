namespace Errorka;

internal static class SequenceExtensions
{
    public static IEnumerable<(T Item, int Index)> Indexed<T>(this IEnumerable<T> sequence, int startIndex) =>
        sequence.Select((item, index) => (item, index + startIndex));

    public static bool AllEquals<T>(this IEnumerable<T> sequence, IEqualityComparer<T> equality)
    {
        using var enumerator = sequence.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            return true;
        }

        var item = enumerator.Current;
        while (enumerator.MoveNext())
        {
            if (!equality.Equals(item, enumerator.Current))
            {
                return false;
            }
        }

        return true;
    }
}