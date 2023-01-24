using Errorka.Code;

namespace Errorka.Contents;

internal readonly struct Parameter<TType, TName> : Content
    where TType : Content
    where TName : Content
{
    private readonly TType first;
    private readonly TName second;

    public Parameter(TType first, TName second)
    {
        this.first = first;
        this.second = second;
    }

    public void Write(Output output)
    {
        first.Write(output);
        output.Write(" ");
        second.Write(output);
    }

    public override string ToString() => this.Print();
}