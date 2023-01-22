using Errorka.Contents;

namespace Errorka.Code;

internal readonly struct Class : IDisposable
{
    private readonly Output output;
    private readonly Output.Block block;

    private Class(Output output, Output.Block block)
    {
        this.block = block;
        this.output = output;
    }

    public static Class Open<T>(Output output, string modifiers, T name) where T : Content
    {
        using (output.StartLine())
        {
            output.Write(modifiers);
            output.Write(" class ");
            name.Write(output);
        }

        var block = output.OpenBlock();
        return new Class(output, block);
    }

    public void Dispose()
    {
        block.Dispose();
    }
}