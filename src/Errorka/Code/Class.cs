using Errorka.Contents;

namespace Errorka.Code;

internal static class Class
{
    public static Type<T> Open<T>(Output output, string modifiers, T name) where T : Content
        => Type<T>.Open(output, modifiers, "class", name);
}