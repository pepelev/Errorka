//HintName: Errorka.Playground.Generic.Result.g.cs
namespace @Errorka.@Playground
{
	partial class @Generic
	{
		public readonly struct Result
		{
			[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
			internal Result(global::@Errorka.@Playground.@Generic.Code code, global::System.Object value)
			{
				this.Code = code;
				this.Value = value;
			}
			public global::@Errorka.@Playground.@Generic.Code Code { get; }
			public global::System.Object Value { get; }
			public static global::@Errorka.@Playground.@Generic.@Result Nullable()
			{
				return new global::@Errorka.@Playground.@Generic.@Result(global::@Errorka.@Playground.@Generic.Code.Nullable, global::@Errorka.@Playground.@Generic.Nullable());
			}
			public global::System.Boolean IsNullable([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@Nullable<global::@System.@Int32> value)
			{
				value = this.Value is global::@System.@Nullable<global::@System.@Int32> ? (global::@System.@Nullable<global::@System.@Int32>)this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Generic.Code.Nullable;
			}
			public static global::@Errorka.@Playground.@Generic.@Result Array()
			{
				return new global::@Errorka.@Playground.@Generic.@Result(global::@Errorka.@Playground.@Generic.Code.Array, global::@Errorka.@Playground.@Generic.Array());
			}
			public global::System.Boolean IsArray([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@Int32[] value)
			{
				value = this.Value is global::@System.@Int32[] ? (global::@System.@Int32[])this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Generic.Code.Array;
			}
			public static global::@Errorka.@Playground.@Generic.@Result ArrayOfArray()
			{
				return new global::@Errorka.@Playground.@Generic.@Result(global::@Errorka.@Playground.@Generic.Code.ArrayOfArray, global::@Errorka.@Playground.@Generic.ArrayOfArray());
			}
			public global::System.Boolean IsArrayOfArray([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@Int32[][] value)
			{
				value = this.Value is global::@System.@Int32[][] ? (global::@System.@Int32[][])this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Generic.Code.ArrayOfArray;
			}
			public static global::@Errorka.@Playground.@Generic.@Result TwoDimensionalArray()
			{
				return new global::@Errorka.@Playground.@Generic.@Result(global::@Errorka.@Playground.@Generic.Code.TwoDimensionalArray, global::@Errorka.@Playground.@Generic.TwoDimensionalArray());
			}
			public global::System.Boolean IsTwoDimensionalArray([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@Int32[,] value)
			{
				value = this.Value is global::@System.@Int32[,] ? (global::@System.@Int32[,])this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Generic.Code.TwoDimensionalArray;
			}
			public static global::@Errorka.@Playground.@Generic.@Result FiveDimensionalArray()
			{
				return new global::@Errorka.@Playground.@Generic.@Result(global::@Errorka.@Playground.@Generic.Code.FiveDimensionalArray, global::@Errorka.@Playground.@Generic.FiveDimensionalArray());
			}
			public global::System.Boolean IsFiveDimensionalArray([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@Int32[,,,,] value)
			{
				value = this.Value is global::@System.@Int32[,,,,] ? (global::@System.@Int32[,,,,])this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Generic.Code.FiveDimensionalArray;
			}
			public static global::@Errorka.@Playground.@Generic.@Result TwoDimensionalArrayOfArray()
			{
				return new global::@Errorka.@Playground.@Generic.@Result(global::@Errorka.@Playground.@Generic.Code.TwoDimensionalArrayOfArray, global::@Errorka.@Playground.@Generic.TwoDimensionalArrayOfArray());
			}
			public global::System.Boolean IsTwoDimensionalArrayOfArray([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@Int32[][,] value)
			{
				value = this.Value is global::@System.@Int32[][,] ? (global::@System.@Int32[][,])this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Generic.Code.TwoDimensionalArrayOfArray;
			}
			public static global::@Errorka.@Playground.@Generic.@Result List()
			{
				return new global::@Errorka.@Playground.@Generic.@Result(global::@Errorka.@Playground.@Generic.Code.List, global::@Errorka.@Playground.@Generic.List());
			}
			public global::System.Boolean IsList([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@Collections.@Generic.@List<global::@System.@Int32> value)
			{
				value = this.Value is global::@System.@Collections.@Generic.@List<global::@System.@Int32> ? (global::@System.@Collections.@Generic.@List<global::@System.@Int32>)this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Generic.Code.List;
			}
			public static global::@Errorka.@Playground.@Generic.@Result Dictionary()
			{
				return new global::@Errorka.@Playground.@Generic.@Result(global::@Errorka.@Playground.@Generic.Code.Dictionary, global::@Errorka.@Playground.@Generic.Dictionary());
			}
			public global::System.Boolean IsDictionary([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32, global::@System.@String> value)
			{
				value = this.Value is global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32, global::@System.@String> ? (global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32, global::@System.@String>)this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Generic.Code.Dictionary;
			}
			public static global::@Errorka.@Playground.@Generic.@Result Complex()
			{
				return new global::@Errorka.@Playground.@Generic.@Result(global::@Errorka.@Playground.@Generic.Code.Complex, global::@Errorka.@Playground.@Generic.Complex());
			}
			public global::System.Boolean IsComplex([global::System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32[], global::@System.@Collections.@Generic.@List<global::@System.@Nullable<global::@System.@Int32>[,]>> value)
			{
				value = this.Value is global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32[], global::@System.@Collections.@Generic.@List<global::@System.@Nullable<global::@System.@Int32>[,]>> ? (global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32[], global::@System.@Collections.@Generic.@List<global::@System.@Nullable<global::@System.@Int32>[,]>>)this.Value : default;
				return this.Code == global::@Errorka.@Playground.@Generic.Code.Complex;
			}
			public T Match<T>(global::System.Func<global::@System.@Nullable<global::@System.@Int32>, T> @Nullable, global::System.Func<global::@System.@Int32[], T> @Array, global::System.Func<global::@System.@Int32[][], T> @ArrayOfArray, global::System.Func<global::@System.@Int32[,], T> @TwoDimensionalArray, global::System.Func<global::@System.@Int32[,,,,], T> @FiveDimensionalArray, global::System.Func<global::@System.@Int32[][,], T> @TwoDimensionalArrayOfArray, global::System.Func<global::@System.@Collections.@Generic.@List<global::@System.@Int32>, T> @List, global::System.Func<global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32, global::@System.@String>, T> @Dictionary, global::System.Func<global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32[], global::@System.@Collections.@Generic.@List<global::@System.@Nullable<global::@System.@Int32>[,]>>, T> @Complex)
			{
				switch (this.Code)
				{
					case global::@Errorka.@Playground.@Generic.Code.@Nullable: return @Nullable(this.Value is global::@System.@Nullable<global::@System.@Int32> ? (global::@System.@Nullable<global::@System.@Int32>)this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@Array: return @Array(this.Value is global::@System.@Int32[] ? (global::@System.@Int32[])this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@ArrayOfArray: return @ArrayOfArray(this.Value is global::@System.@Int32[][] ? (global::@System.@Int32[][])this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@TwoDimensionalArray: return @TwoDimensionalArray(this.Value is global::@System.@Int32[,] ? (global::@System.@Int32[,])this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@FiveDimensionalArray: return @FiveDimensionalArray(this.Value is global::@System.@Int32[,,,,] ? (global::@System.@Int32[,,,,])this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@TwoDimensionalArrayOfArray: return @TwoDimensionalArrayOfArray(this.Value is global::@System.@Int32[][,] ? (global::@System.@Int32[][,])this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@List: return @List(this.Value is global::@System.@Collections.@Generic.@List<global::@System.@Int32> ? (global::@System.@Collections.@Generic.@List<global::@System.@Int32>)this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@Dictionary: return @Dictionary(this.Value is global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32, global::@System.@String> ? (global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32, global::@System.@String>)this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@Complex: return @Complex(this.Value is global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32[], global::@System.@Collections.@Generic.@List<global::@System.@Nullable<global::@System.@Int32>[,]>> ? (global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32[], global::@System.@Collections.@Generic.@List<global::@System.@Nullable<global::@System.@Int32>[,]>>)this.Value : default);
					default: throw new Exception("Instance is broken. Code: " + this.Code);
				}
			}
			public T Match<T>(T @default, global::System.Func<global::@System.@Nullable<global::@System.@Int32>, T> @Nullable = null, global::System.Func<global::@System.@Int32[], T> @Array = null, global::System.Func<global::@System.@Int32[][], T> @ArrayOfArray = null, global::System.Func<global::@System.@Int32[,], T> @TwoDimensionalArray = null, global::System.Func<global::@System.@Int32[,,,,], T> @FiveDimensionalArray = null, global::System.Func<global::@System.@Int32[][,], T> @TwoDimensionalArrayOfArray = null, global::System.Func<global::@System.@Collections.@Generic.@List<global::@System.@Int32>, T> @List = null, global::System.Func<global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32, global::@System.@String>, T> @Dictionary = null, global::System.Func<global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32[], global::@System.@Collections.@Generic.@List<global::@System.@Nullable<global::@System.@Int32>[,]>>, T> @Complex = null)
			{
				switch (this.Code)
				{
					case global::@Errorka.@Playground.@Generic.Code.@Nullable: return @Nullable == null ? @default : @Nullable(this.Value is global::@System.@Nullable<global::@System.@Int32> ? (global::@System.@Nullable<global::@System.@Int32>)this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@Array: return @Array == null ? @default : @Array(this.Value is global::@System.@Int32[] ? (global::@System.@Int32[])this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@ArrayOfArray: return @ArrayOfArray == null ? @default : @ArrayOfArray(this.Value is global::@System.@Int32[][] ? (global::@System.@Int32[][])this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@TwoDimensionalArray: return @TwoDimensionalArray == null ? @default : @TwoDimensionalArray(this.Value is global::@System.@Int32[,] ? (global::@System.@Int32[,])this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@FiveDimensionalArray: return @FiveDimensionalArray == null ? @default : @FiveDimensionalArray(this.Value is global::@System.@Int32[,,,,] ? (global::@System.@Int32[,,,,])this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@TwoDimensionalArrayOfArray: return @TwoDimensionalArrayOfArray == null ? @default : @TwoDimensionalArrayOfArray(this.Value is global::@System.@Int32[][,] ? (global::@System.@Int32[][,])this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@List: return @List == null ? @default : @List(this.Value is global::@System.@Collections.@Generic.@List<global::@System.@Int32> ? (global::@System.@Collections.@Generic.@List<global::@System.@Int32>)this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@Dictionary: return @Dictionary == null ? @default : @Dictionary(this.Value is global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32, global::@System.@String> ? (global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32, global::@System.@String>)this.Value : default);
					case global::@Errorka.@Playground.@Generic.Code.@Complex: return @Complex == null ? @default : @Complex(this.Value is global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32[], global::@System.@Collections.@Generic.@List<global::@System.@Nullable<global::@System.@Int32>[,]>> ? (global::@System.@Collections.@Generic.@Dictionary<global::@System.@Int32[], global::@System.@Collections.@Generic.@List<global::@System.@Nullable<global::@System.@Int32>[,]>>)this.Value : default);
					default: throw new Exception("Instance is broken. Code: " + this.Code);
				}
			}
		}
	}
}
