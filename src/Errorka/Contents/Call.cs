using Errorka.Code;

namespace Errorka.Contents;

internal readonly struct Call<TMethod, TArguments> : Content
    where TMethod : Content
    where TArguments : Content
{
    private readonly TMethod method;
    private readonly TArguments arguments;

    public Call(TMethod method, TArguments arguments)
    {
        this.method = method;
        this.arguments = arguments;
    }

    public void Write(Output output)
    {
        method.Write(output);
        new ParenthesesEnclosed<TArguments>(arguments).Write(output);
    }

    public override string ToString() => this.Print();
}