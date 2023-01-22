namespace Errorka.Playground;

[Result]
public static partial class Service
{
    [Area("Lookup")]
    private static string Ok(string content) => content;

    [Area("Creation")]
    private static string Created() => "created";

    [Area("Lookup")]
    private static string NotFound() => "not-found";

    [Area("Lookup")]
    private static string Moved(string where) => $"moved to {where}";

    [Area("Access")]
    [Area("Lookup")]
    [Area("Creation")]
    private static string Unauthorized() => "unauthorized";

    [Area("Access")]
    [Area("Lookup")]
    [Area("Creation")]
    private static string Forbidden() => "forbidden";
}