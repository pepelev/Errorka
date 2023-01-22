namespace Errorka.Contents;

internal static class ContentExtension
{
    public static string Print(this Content content)
    {
        var output = new Output();
        content.Write(output);
        return output.ToString();
    }
}