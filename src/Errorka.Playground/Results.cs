namespace Errorka.Playground;

[Result]
public partial class Results
{
    [Area("Disk"), Area("Memory")]
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
        Result.DiskFailed().Match(DiskFailed: s => s.Length, MemoryFailed: s => s.GetHashCode());
        {
            Result.MemoryFailed(10).IsMemoryFailed(out var d);
            var s = d.ToString();
        }

        return "disk-failed";
    }
}