namespace Errorka.Playground;

[Result]
public partial class Results
{
    private static object MemoryFailed()
    {
        return Code.MemoryFailed == Code.MemoryFailed;
    }

    private static string DiskFailed()
    {
        if (Result.DiskFailed().IsDiskFailed(out var str))
        {
            
        }
    }
}