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
        public enum Code
        {
            UserNotExists = 1,
            AccessDenied = 2,
            ResourceLocked = 3
        }
    }

    partial class Results
    {
        public readonly struct Result
        {
            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            internal Result(global::Errorka.Concept.Results.Code code, global::System.Object value)
            {
                this.Code = code;
                this.Value = value;
            }

            public global::Errorka.Concept.Results.Code Code { get; }
            public global::System.Object Value { get; }

            public static global::Errorka.Concept.Results.Result UserNotExists() => new global::Errorka.Concept.Results.Result(global::Errorka.Concept.Results.Code.UserNotExists, global::Errorka.Concept.Results.UserNotExists());

            public global::System.Boolean IsUserNotExists([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::System.String? value)
            {
                value = Value as global::System.String;
                return Code == global::Errorka.Concept.Results.Code.UserNotExists;
            }

            public global::System.Boolean IsAccessDenied([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::System.Object? value)
            {
                value = Value as global::System.Object;
                return Code == global::Errorka.Concept.Results.Code.AccessDenied;
            }
        }

        public readonly struct Users
        {
            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            internal Users(global::Errorka.Concept.Results.Code code, global::System.Object value)
            {
                Code = code;
                Value = value;
            }

            public Code Code { get; }
            public object Value { get; }

            public static Users UserNotExists() => new(Code.UserNotExists, Results.UserNotExists());
            public static Users AccessDenied() => new(Code.AccessDenied, Results.AccessDenied());
            public Result ToResult() => new(Code, Value);

            public global::System.Boolean IsUserNotExists([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::System.String? value)
            {
                value = Value as global::System.String;
                return Code == global::Errorka.Concept.Results.Code.UserNotExists;
            }

            public global::System.Boolean IsAccessDenied([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::System.Object? value)
            {
                value = Value as global::System.Object;
                return Code == global::Errorka.Concept.Results.Code.AccessDenied;
            }
        }

        public readonly struct Access
        {
            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            internal Access(Code code, object value)
            {
                Code = code;
                Value = value;
            }

            public Code Code { get; }
            public object Value { get; }

            public static Access AccessDenied() => new(Code.AccessDenied, Results.AccessDenied());
            public Users ToUsers() => new(this.Code, this.Value);
        }
    }
}