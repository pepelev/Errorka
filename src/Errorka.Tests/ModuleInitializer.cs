using System.Runtime.CompilerServices;

namespace Errorka.Tests;

internal static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifySourceGenerators.Enable();
    }
}