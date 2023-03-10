using Errorka.Code;

namespace Errorka.Contents;

internal readonly struct Dotted<T1, T2> : Content
    where T1 : Content
    where T2 : Content
{
    private readonly T1 first;
    private readonly T2 second;

    public Dotted(T1 first, T2 second)
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

internal readonly struct Dotted<T1, T2, T3> : Content
    where T1 : Content
    where T2 : Content
    where T3 : Content
{
    private readonly T1 first;
    private readonly T2 second;
    private readonly T3 third;

    public Dotted(T1 first, T2 second, T3 third)
    {
        this.first = first;
        this.second = second;
        this.third = third;
    }

    public void Write(Output output)
    {
        first.Write(output);
        output.Write(".");
        second.Write(output);
        output.Write(".");
        third.Write(output);
    }

    public override string ToString() => this.Print();
}