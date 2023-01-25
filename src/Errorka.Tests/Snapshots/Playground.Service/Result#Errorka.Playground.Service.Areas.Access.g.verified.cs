//HintName: Errorka.Playground.Service.Areas.Access.g.cs
namespace @Errorka.@Playground
{
	partial class @Service
	{
		public readonly struct @Access
		{
			[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
			internal @Access(global::@Errorka.@Playground.@Service.Code code, global::System.Object value)
			{
				this.Code = code;
				this.Value = value;
			}
			public global::@Errorka.@Playground.@Service.Code Code { get; }
			public global::System.Object Value { get; }
			public static global::@Errorka.@Playground.@Service.@Access Unauthorized()
			{
				return new global::@Errorka.@Playground.@Service.@Access(global::@Errorka.@Playground.@Service.Code.Unauthorized, global::@Errorka.@Playground.@Service.Unauthorized());
			}
			public global::System.Boolean IsUnauthorized([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@String value)
			{
				value = this.Value is global::@System.@String ? (global::@System.@String)this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Service.Code.Unauthorized;
			}
			public static global::@Errorka.@Playground.@Service.@Access Forbidden()
			{
				return new global::@Errorka.@Playground.@Service.@Access(global::@Errorka.@Playground.@Service.Code.Forbidden, global::@Errorka.@Playground.@Service.Forbidden());
			}
			public global::System.Boolean IsForbidden([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@String value)
			{
				value = this.Value is global::@System.@String ? (global::@System.@String)this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Service.Code.Forbidden;
			}
			public T Match<T>(global::System.Func<global::@System.@String, T> @Unauthorized, global::System.Func<global::@System.@String, T> @Forbidden)
			{
				switch (this.Code)
				{
					case global::@Errorka.@Playground.@Service.Code.@Unauthorized: return @Unauthorized(this.Value is global::@System.@String ? (global::@System.@String)this.Value : default);
					case global::@Errorka.@Playground.@Service.Code.@Forbidden: return @Forbidden(this.Value is global::@System.@String ? (global::@System.@String)this.Value : default);
					default: throw new Exception("Instance is broken. Code: " + this.Code);
				}
			}
			public T Match<T>(T @default, global::System.Func<global::@System.@String, T> @Unauthorized = null, global::System.Func<global::@System.@String, T> @Forbidden = null)
			{
				switch (this.Code)
				{
					case global::@Errorka.@Playground.@Service.Code.@Unauthorized: return @Unauthorized == null ? @default : @Unauthorized(this.Value is global::@System.@String ? (global::@System.@String)this.Value : default);
					case global::@Errorka.@Playground.@Service.Code.@Forbidden: return @Forbidden == null ? @default : @Forbidden(this.Value is global::@System.@String ? (global::@System.@String)this.Value : default);
					default: throw new Exception("Instance is broken. Code: " + this.Code);
				}
			}
			public global::@Errorka.@Playground.@Service.Result ToResult()
			{
				return new global::@Errorka.@Playground.@Service.Result(this.Code, this.Value);
			}
			public static implicit operator global::@Errorka.@Playground.@Service.Result(@Access area)
			{
				return area.ToResult();
			}
			public global::@Errorka.@Playground.@Service.@Lookup @ToLookup()
			{
				return new global::@Errorka.@Playground.@Service.@Lookup(this.Code, this.Value);
			}
			public static implicit operator global::@Errorka.@Playground.@Service.@Lookup(@Access area)
			{
				return area.@ToLookup();
			}
			public global::@Errorka.@Playground.@Service.@Creation @ToCreation()
			{
				return new global::@Errorka.@Playground.@Service.@Creation(this.Code, this.Value);
			}
			public static implicit operator global::@Errorka.@Playground.@Service.@Creation(@Access area)
			{
				return area.@ToCreation();
			}
		}
	}
}
