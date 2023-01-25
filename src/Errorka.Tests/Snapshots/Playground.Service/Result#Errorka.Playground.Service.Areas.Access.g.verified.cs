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
