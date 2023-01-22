//HintName: Errorka.Playground.Service.Areas.Creation.g.cs
namespace @Errorka.@Playground
{
	partial class @Service
	{
		public readonly struct @Creation
		{
			[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
			internal @Creation(global::Errorka.Playground.Service.Code code, global::System.Object value)
			{
				this.Code = code;
				this.Value = value;
			}
			public global::Errorka.Playground.Service.Code Code { get; }
			public global::System.Object Value { get; }
			public static global::@Errorka.@Playground.@Service.@Creation Created()
			{
				return new global::@Errorka.@Playground.@Service.@Creation(global::@Errorka.@Playground.@Service.Code.Created, global::@Errorka.@Playground.@Service.Created());
			}
			public global::System.Boolean IsCreated([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@String value)
			{
				value = this.Value is global::@System.@String ? (global::@System.@String)this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Service.Code.Created;
			}
			public static global::@Errorka.@Playground.@Service.@Creation Unauthorized()
			{
				return new global::@Errorka.@Playground.@Service.@Creation(global::@Errorka.@Playground.@Service.Code.Unauthorized, global::@Errorka.@Playground.@Service.Unauthorized());
			}
			public global::System.Boolean IsUnauthorized([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@String value)
			{
				value = this.Value is global::@System.@String ? (global::@System.@String)this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Service.Code.Unauthorized;
			}
			public static global::@Errorka.@Playground.@Service.@Creation Forbidden()
			{
				return new global::@Errorka.@Playground.@Service.@Creation(global::@Errorka.@Playground.@Service.Code.Forbidden, global::@Errorka.@Playground.@Service.Forbidden());
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
		}
	}
}
