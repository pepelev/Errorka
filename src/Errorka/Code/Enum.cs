using System.Globalization;
using Errorka.Contents;

namespace Errorka.Code;

internal readonly struct Enum : IDisposable
{
    private readonly Output output;
    private readonly Output.Block block;

    private Enum(Output output, Output.Block block)
    {
        this.block = block;
        this.output = output;
    }

    public static Enum Open<T>(Output output, string modifiers, T name) where T : Content
    {
        using (output.StartLine())
        {
            output.Write(modifiers);
            output.Write(" enum ");
            name.Write(output);
        }

        var block = output.OpenBlock();
        return new Enum(output, block);
    }

    public void Member<T>(T name, int value) where T : Content
    {
        using (output.StartLine())
        {
            name.Write(output);
            output.Write(" = ");
            output.Write(value.ToString(CultureInfo.InvariantCulture));
            output.Write(",");
        }
    }

    public void Dispose()
    {
        block.Dispose();
    }
}