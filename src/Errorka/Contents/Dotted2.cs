using Errorka.Code;

namespace Errorka.Contents;

internal readonly struct Dotted2<T1, T2> : Content
    where T1 : Content
    where T2 : Content
{
    private readonly T1 first;
    private readonly T2 second;

    public Dotted2(T1 first, T2 second)
    {
        this.first = first;
        this.second = second;
    }

    public void Write(Output output)
    {
        first.Write(output);
        output.Write(".");
        second.Write(output);
    }

    public override string ToString() => this.Print();
}