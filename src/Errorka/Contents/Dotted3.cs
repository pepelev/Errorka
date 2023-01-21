namespace Errorka.Contents;

internal readonly struct Dotted3<T1, T2, T3> : Content
    where T1 : Content
    where T2 : Content
    where T3 : Content
{
    private readonly T1 first;
    private readonly T2 second;
    private readonly T3 third;

    public Dotted3(T1 first, T2 second, T3 third)
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
}