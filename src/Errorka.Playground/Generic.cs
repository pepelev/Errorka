namespace Errorka.Playground;

[Result]
public partial class Generic
{
    private static int? Nullable() => null;
    private static int[] Array() => null;
    private static int[][] ArrayOfArray() => null;
    private static int[,] TwoDimensionalArray() => null;
    private static int[,,,,] FiveDimensionalArray() => null;
    private static int[][,] TwoDimensionalArrayOfArray() => (int[][,])(object)TwoDimensionalArrayOfArray()[12][12,3];
    private static List<int> List() => null;
    private static Dictionary<int, string> Dictionary() => null;
    private static Dictionary<int[], List<int?[,]>> Complex() => null;
}