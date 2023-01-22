//HintName: Errorka.Playground.Service.Areas.Lookup.g.cs
namespace Errorka.Playground
{
	partial class @Service
	{
		public readonly struct @Lookup
		{
			[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
			internal @Lookup(global::Errorka.Playground.Service.Code code, global::System.Object value)
			{
				this.Code = code;
				this.Value = value;
			}
			public global::Errorka.Playground.Service.Code Code { get; }
			public global::System.Object Value { get; }
			public static global::Errorka.Playground.Service.@Lookup Ok(global::System.String content)
			{
				return new global::Errorka.Playground.Service.@Lookup(global::Errorka.Playground.Service.Code.Ok, global::Errorka.Playground.Service.Ok(content));
			}
			public global::System.Boolean IsOk([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::System.String value)
			{
				value = this.Value is global::System.String ? (global::System.String)this.Value : default;
				return this.Code == global::Errorka.Playground.Service.Code.Ok;
			}
			public static global::Errorka.Playground.Service.@Lookup Created()
			{
				return new global::Errorka.Playground.Service.@Lookup(global::Errorka.Playground.Service.Code.Created, global::Errorka.Playground.Service.Created());
			}
			public global::System.Boolean IsCreated([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::System.String value)
			{
				value = this.Value is global::System.String ? (global::System.String)this.Value : default;
				return this.Code == global::Errorka.Playground.Service.Code.Created;
			}
			public static global::Errorka.Playground.Service.@Lookup NotFound()
			{
				return new global::Errorka.Playground.Service.@Lookup(global::Errorka.Playground.Service.Code.NotFound, global::Errorka.Playground.Service.NotFound());
			}
			public global::System.Boolean IsNotFound([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::System.String value)
			{
				value = this.Value is global::System.String ? (global::System.String)this.Value : default;
				return this.Code == global::Errorka.Playground.Service.Code.NotFound;
			}
			public static global::Errorka.Playground.Service.@Lookup Moved(global::System.String where)
			{
				return new global::Errorka.Playground.Service.@Lookup(global::Errorka.Playground.Service.Code.Moved, global::Errorka.Playground.Service.Moved(where));
			}
			public global::System.Boolean IsMoved([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::System.String value)
			{
				value = this.Value is global::System.String ? (global::System.String)this.Value : default;
				return this.Code == global::Errorka.Playground.Service.Code.Moved;
			}
			public static global::Errorka.Playground.Service.@Lookup Unauthorized()
			{
				return new global::Errorka.Playground.Service.@Lookup(global::Errorka.Playground.Service.Code.Unauthorized, global::Errorka.Playground.Service.Unauthorized());
			}
			public global::System.Boolean IsUnauthorized([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::System.String value)
			{
				value = this.Value is global::System.String ? (global::System.String)this.Value : default;
				return this.Code == global::Errorka.Playground.Service.Code.Unauthorized;
			}
			public static global::Errorka.Playground.Service.@Lookup Forbidden()
			{
				return new global::Errorka.Playground.Service.@Lookup(global::Errorka.Playground.Service.Code.Forbidden, global::Errorka.Playground.Service.Forbidden());
			}
			public global::System.Boolean IsForbidden([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::System.String value)
			{
				value = this.Value is global::System.String ? (global::System.String)this.Value : default;
				return this.Code == global::Errorka.Playground.Service.Code.Forbidden;
			}
			public global::Errorka.Playground.Service.Result ToResult()
			{
				return new global::Errorka.Playground.Service.Result(this.Code, this.Value);
			}
		}
	}
}
