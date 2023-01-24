using Errorka.Code;

namespace Errorka.Contents;

internal readonly struct CommaSeparatedList<T1, T2> : Content
    where T1 : Content
    where T2 : Content
{
    private readonly T1 first;
    private readonly T2 second;

    public CommaSeparatedList(T1 first, T2 second)
    {
        this.first = first;
        this.second = second;
    }

    public void Write(Output output)
    {
        var arguments = output.CommaSeparated();
        arguments.Append(first);
        arguments.Append(second);
    }

    public override string ToString() => this.Print();
}