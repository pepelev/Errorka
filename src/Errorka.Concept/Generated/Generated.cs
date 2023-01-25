// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantAttributeUsageProperty
// ReSharper disable MemberHidesStaticFromOuterClass
// ReSharper disable UnusedMember.Global
// ReSharper disable RedundantNullableFlowAttribute
// ReSharper disable MemberCanBePrivate.Global
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

namespace @Errorka.@Concept
{
    partial class @Outcome
    {
        public readonly struct @Access
        {
            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            internal @Access(global::@Errorka.@Concept.@Outcome.Code code, global::System.Object value)
            {
                this.Code = code;
                this.Value = value;
            }
            public global::@Errorka.@Concept.@Outcome.Code Code { get; }
            public global::System.Object Value { get; }
            public static global::@Errorka.@Concept.@Outcome.@Access AccessDenied()
            {
                return new global::@Errorka.@Concept.@Outcome.@Access(global::@Errorka.@Concept.@Outcome.Code.AccessDenied, global::@Errorka.@Concept.@Outcome.AccessDenied());
            }
            public global::System.Boolean IsAccessDenied([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@Object value)
            {
                value = this.Value is global::@System.@Object ? (global::@System.@Object)this.Value : default;
                return this.Code == global::@Errorka.@Concept.@Outcome.Code.AccessDenied;
            }
            public global::@Errorka.@Concept.@Outcome.Result ToResult()
            {
                return new global::@Errorka.@Concept.@Outcome.Result(this.Code, this.Value);
            }
            public static implicit operator global::@Errorka.@Concept.@Outcome.Result(@Access area)
            {
                return area.ToResult();
            }
            public global::@Errorka.@Concept.@Outcome.@Users @ToUsers()
            {
                return new global::@Errorka.@Concept.@Outcome.@Users(this.Code, this.Value);
            }
            public static implicit operator global::@Errorka.@Concept.@Outcome.@Users(@Access area)
            {
                return area.ToUsers();
            }
        }
    }
}

namespace @Errorka.@Concept
{
    partial class @Outcome
    {
        public readonly struct @Resources
        {
            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            internal @Resources(global::@Errorka.@Concept.@Outcome.Code code, global::System.Object value)
            {
                this.Code = code;
                this.Value = value;
            }
            public global::@Errorka.@Concept.@Outcome.Code Code { get; }
            public global::System.Object Value { get; }
            public static global::@Errorka.@Concept.@Outcome.@Resources ResourceLocked(global::@System.@Int32 @userId)
            {
                return new global::@Errorka.@Concept.@Outcome.@Resources(global::@Errorka.@Concept.@Outcome.Code.ResourceLocked, global::@Errorka.@Concept.@Outcome.ResourceLocked(userId));
            }
            public global::System.Boolean IsResourceLocked([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@ValueTuple value)
            {
                value = this.Value is global::@System.@ValueTuple ? (global::@System.@ValueTuple)this.Value : default;
                return this.Code == global::@Errorka.@Concept.@Outcome.Code.ResourceLocked;
            }
            public global::@Errorka.@Concept.@Outcome.Result ToResult()
            {
                return new global::@Errorka.@Concept.@Outcome.Result(this.Code, this.Value);
            }
        }
    }
}

namespace @Errorka.@Concept
{
    partial class @Outcome
    {
        public readonly struct @Users
        {
            [global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
            internal @Users(global::@Errorka.@Concept.@Outcome.Code code, global::System.Object value)
            {
                this.Code = code;
                this.Value = value;
            }
            public global::@Errorka.@Concept.@Outcome.Code Code { get; }
            public global::System.Object Value { get; }
            public static global::@Errorka.@Concept.@Outcome.@Users UserNotExists()
            {
                return new global::@Errorka.@Concept.@Outcome.@Users(global::@Errorka.@Concept.@Outcome.Code.UserNotExists, global::@Errorka.@Concept.@Outcome.UserNotExists());
            }
            public global::System.Boolean IsUserNotExists([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@String value)
            {
                value = this.Value is global::@System.@String ? (global::@System.@String)this.Value : default;
                return this.Code == global::@Errorka.@Concept.@Outcome.Code.UserNotExists;
            }
            public static global::@Errorka.@Concept.@Outcome.@Users AccessDenied()
            {
                return new global::@Errorka.@Concept.@Outcome.@Users(global::@Errorka.@Concept.@Outcome.Code.AccessDenied, global::@Errorka.@Concept.@Outcome.AccessDenied());
            }
            public global::System.Boolean IsAccessDenied([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@Object value)
            {
                value = this.Value is global::@System.@Object ? (global::@System.@Object)this.Value : default;
                return this.Code == global::@Errorka.@Concept.@Outcome.Code.AccessDenied;
            }
            public global::@Errorka.@Concept.@Outcome.Result ToResult()
            {
                return new global::@Errorka.@Concept.@Outcome.Result(this.Code, this.Value);
            }
        }
    }
}

namespace @Errorka.@Concept
{
    partial class @Outcome
    {
        public enum Code
        {
            @UserNotExists = 1,
            @AccessDenied = 2,
            @ResourceLocked = 3,
        }
    }
}

namespace @Errorka.@Concept
{
	partial class @Outcome
	{
		public readonly struct Result
		{
			[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
			internal Result(global::@Errorka.@Concept.@Outcome.Code code, global::System.Object value)
			{
				this.Code = code;
				this.Value = value;
			}
			public global::@Errorka.@Concept.@Outcome.Code Code { get; }
			public global::System.Object Value { get; }
			public static global::@Errorka.@Concept.@Outcome.@Result UserNotExists()
			{
				return new global::@Errorka.@Concept.@Outcome.@Result(global::@Errorka.@Concept.@Outcome.Code.UserNotExists, global::@Errorka.@Concept.@Outcome.UserNotExists());
			}
			public global::System.Boolean IsUserNotExists([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@String value)
			{
				value = this.Value is global::@System.@String ? (global::@System.@String)this.Value : default;
				return this.Code == global::@Errorka.@Concept.@Outcome.Code.UserNotExists;
			}
			public static global::@Errorka.@Concept.@Outcome.@Result AccessDenied()
			{
				return new global::@Errorka.@Concept.@Outcome.@Result(global::@Errorka.@Concept.@Outcome.Code.AccessDenied, global::@Errorka.@Concept.@Outcome.AccessDenied());
			}
			public global::System.Boolean IsAccessDenied([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@Object value)
			{
				value = this.Value is global::@System.@Object ? (global::@System.@Object)this.Value : default;
				return this.Code == global::@Errorka.@Concept.@Outcome.Code.AccessDenied;
			}
			public static global::@Errorka.@Concept.@Outcome.@Result ResourceLocked(global::@System.@Int32 @userId)
			{
				return new global::@Errorka.@Concept.@Outcome.@Result(global::@Errorka.@Concept.@Outcome.Code.ResourceLocked, global::@Errorka.@Concept.@Outcome.ResourceLocked(userId));
			}
			public global::System.Boolean IsResourceLocked([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@ValueTuple value)
			{
				value = this.Value is global::@System.@ValueTuple ? (global::@System.@ValueTuple)this.Value : default;
				return this.Code == global::@Errorka.@Concept.@Outcome.Code.ResourceLocked;
			}
		}
	}
}
