using Errorka.Code;

namespace Errorka.Contents;

internal readonly struct Generic<TMethod, TParameters> : Content
    where TMethod : Content
    where TParameters : Content
{
    private readonly TMethod method;
    private readonly TParameters parameters;

    public Generic(TMethod method, TParameters parameters)
    {
        this.method = method;
        this.parameters = parameters;
    }

    public void Write(Output output)
    {
        method.Write(output);
        output.Write("<");
        parameters.Write(output);
        output.Write(">");
    }

    public override string ToString() => this.Print();
}