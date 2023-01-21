namespace Errorka.Concept;

/// <summary>
/// Класс описывает все возможные исходы работы модуля.
/// Исходы можно разделить по областям.
/// Область - это логически связанная функциональность,
/// которая может своим результатом иметь отмеченные варианты.
/// Один исход может присутствовать в разных областях одновременно.
///
/// Механизм трансформации значения из одной области в другую.
/// Например, из более узкой области в более общую.
///
/// Разные исходы могут иметь разную полезную нагрузку.
/// Достать полезную нагрузку можно с помощью визитора.
/// </summary>
[Results]
public static partial class Results
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