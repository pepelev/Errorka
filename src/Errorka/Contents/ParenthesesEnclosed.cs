using Errorka.Code;

namespace Errorka.Contents;

internal readonly struct ParenthesesEnclosed<T> : Content
    where T : Content
{
    private readonly T content;

    public ParenthesesEnclosed(T content)
    {
        this.content = content;
    }

    public void Write(Output output)
    {
        output.Write("(");
        content.Write(output);
        output.Write(")");
    }

    public override string ToString() => this.Print();
}