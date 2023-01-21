namespace Errorka.Contents;

internal readonly struct Call<T1, T2> : Content
    where T1 : Content
    where T2 : Content
{
    private readonly T1 method;
    private readonly T2 arguments;

    public Call(T1 method, T2 arguments)
    {
        this.method = method;
        this.arguments = arguments;
    }

    public void Write(Output output)
    {
        method.Write(output);
        output.Write("(");
        arguments.Write(output);
        output.Write(")");
    }
}