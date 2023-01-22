using Errorka.Code;

namespace Errorka.Contents;

internal readonly struct VerbatimPrefixed<T> : Content
    where T : Content
{
    private readonly T content;

    public VerbatimPrefixed(T content)
    {
        this.content = content;
    }

    public void Write(Output output)
    {
        output.Write("@");
        content.Write(output);
    }

    public override string ToString() => this.Print();
}