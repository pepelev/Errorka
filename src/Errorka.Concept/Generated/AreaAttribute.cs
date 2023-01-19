// ReSharper disable CheckNamespace
namespace Errorka
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class AreaAttribute : Attribute
    {
        public AreaAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ResultsAttribute : Attribute
    {
    }
}

namespace Errorka.Concept
{
    partial class Results
    {
        public readonly struct Users
        {
            private Users(uint code, string value)
            {
                this.code = code;
                Value = value;
            }

            private readonly uint code;
            public string Value { get; }

            public static Users UserNotExists() => new(0, Results.UserNotExists());
            public static Users AccessDenied() => new(1, Results.AccessDenied());
        }

        public readonly struct Access
        {
            private Access(uint code, string value)
            {
                this.code = code;
                Value = value;
            }

            private readonly uint code;
            public string Value { get; }

            public static Access AccessDenied() => new(1, Results.AccessDenied());
            public Users ToUsers() => Users.AccessDenied();
        
        }
    }
}