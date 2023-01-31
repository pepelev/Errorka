//HintName: ErrorkaAttributes.g.cs

namespace Errorka
{
    [global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class ResultAttribute : global::System.Attribute
    {
    }

    [global::System.AttributeUsage(global::System.AttributeTargets.Method, AllowMultiple = true)]
    internal sealed class AreaAttribute : global::System.Attribute
    {
        public AreaAttribute(global::System.String name)
        {
            Name = name;
        }

        public global::System.String Name { get; }
    }
}