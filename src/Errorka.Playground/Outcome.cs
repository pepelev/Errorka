namespace Errorka.Playground;

[Result]
public static partial class Outcome
{
    [Area("Users")]
    private static string UserNotExists() => "user-not-exists";

    [Area("Access")]
    [Area("Users")]
    private static object AccessDenied() => "access-denied";

    [Area("Resources")]
    private static (string Code, int UserId) ResourceLocked(int userId) =>
        ("resource-locked", userId);
}