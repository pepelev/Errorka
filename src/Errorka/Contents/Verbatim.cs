namespace Errorka.Contents;

internal readonly struct Verbatim : Content
{
    private readonly string value;

    public Verbatim(string value)
    {
        this.value = value;
    }

    public void Write(Output output)
    {
        output.Write(value);
    }

    public override string ToString() => this.Print();
}