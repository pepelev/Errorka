using Errorka.Code;

namespace Errorka.Contents;

internal static class ContentExtension
{
    public static string Print(this Content content)
    {
        var output = new Output();
        content.Write(output);
        return output.ToString();
    }

    public static VerbatimPrefixed<T> VerbatimPrefixed<T>(this T content) where T : Content => new(content);
}