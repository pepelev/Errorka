namespace Errorka.Playground;

[Result]
public partial class Results
{
    [Area("Memory")]
    [Area("Disk")]
    private static bool MemoryFailed()
    {
        return Code.MemoryFailed == Code.MemoryFailed;
    }

    private static string MemoryFailed(int a)
    {
        return (Code.MemoryFailed == Code.MemoryFailed).ToString();
    }

    [Area("Disk")]
    private static string DiskFailed()
    {
        if (Result.DiskFailed().IsDiskFailed(out var str))
        {
            Result.MemoryFailed(10).IsMemoryFailed(out var d);
        }

        return "disk-failed";
    }
}