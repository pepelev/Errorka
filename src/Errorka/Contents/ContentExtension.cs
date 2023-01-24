using System.Text;
using Errorka.Code;

namespace Errorka.Contents;

internal static class ContentExtension
{
    public static string Print(this Content content)
    {
        var output = new Output(new StringBuilder(128));
        content.Write(output);
        return output.ToString();
    }

    public static VerbatimPrefixed<T> VerbatimPrefixed<T>(this T content) where T : Content => new(content);
}