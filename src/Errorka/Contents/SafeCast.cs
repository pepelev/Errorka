using Errorka.Code;

namespace Errorka.Contents;

internal readonly struct SafeCast<TWhat, TTargetType> : Content
    where TWhat : Content
    where TTargetType : Content
{
    private readonly TWhat what;
    private readonly TTargetType targetType;

    public SafeCast(TWhat what, TTargetType targetType)
    {
        this.what = what;
        this.targetType = targetType;
    }

    public void Write(Output output)
    {
        what.Write(output);
        output.Write(" is ");
        targetType.Write(output);
        output.Write(" ? ");
        new ParenthesesEnclosed<TTargetType>(targetType).Write(output);
        what.Write(output);
        output.Write(" : default");
    }

    public override string ToString() => this.Print();
}