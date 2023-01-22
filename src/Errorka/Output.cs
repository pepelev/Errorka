using System.Text;
using Errorka.Contents;
using Microsoft.CodeAnalysis;

namespace Errorka;

internal sealed class Output
{
    private int indent = 0;
    private bool started = false;
    private readonly StringBuilder buffer = new(80 * 1024 / sizeof(char));

    public Block OpenBlock()
    {
        buffer
            .Append('\t', indent)
            .AppendLine("{");
        indent++;
        started = false;
        return new Block(this);
    }

    public Region OpenRegion(string name)
    {
        Write("#region ");
        Write(name);
        EndLine();
        return new Region(this);
    }

    private void CloseRegion()
    {
        Write("#endregion");
        EndLine();
    }

    public Block OpenNamespace(INamespaceSymbol @namespace)
    {
        buffer
            .Append('\t', indent)
            .Append("namespace ")
            .AppendLine(@namespace.ToString());
        return OpenBlock();
    }

    public Block OpenPartialClass(ISymbol @class)
    {
        buffer
            .Append('\t', indent)
            .Append("partial class @")
            .AppendLine(@class.Name);
        return OpenBlock();
    }

    public Block OpenEnum(string name)
    {
        buffer
            .Append('\t', indent)
            .Append("public enum @")
            .AppendLine(name);
        return OpenBlock();
    }

    public Block OpenStruct(string name)
    {
        buffer
            .Append('\t', indent)
            .Append("public readonly struct @")
            .AppendLine(name);
        return OpenBlock();
    }

    public void EnumMember(string name, int value)
    {
        buffer
            .Append('\t', indent)
            .Append('@')
            .Append(name)
            .Append(" = ")
            .Append(value)
            .AppendLine(",");
    }

    // todo kill
    [Obsolete]
    public void WriteLine(string content)
    {
        buffer
            .Append('\t', indent)
            .AppendLine(content);
    }


    public void Constructor(string typeName, INamedTypeSymbol rootType)
    {
        Write("[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]");
        EndLine();
        buffer
            .Append('\t', indent)
            .Append("internal @")
            .Append(typeName)
            .Append("(global::")
            .Append(rootType)
            .AppendLine(".Code code, global::System.Object value)");
        using (OpenBlock())
        {
            Write("this.Code = code;");
            EndLine();
            Write("this.Value = value;");
            EndLine();
        }
    }

    public void GetAutoProperty(string type, string name)
    {
        buffer
            .Append('\t', indent)
            .Append("public ")
            .Append(type)
            .Append(' ')
            .Append(name)
            .AppendLine(" { get; }");
    }

    public void Write(string content)
    {
        if (!started)
        {
            buffer.Append('\t', indent);
            started = true;
        }

        buffer.Append(content);
    }

    public Line StartLine() => new(this);

    public void EndLine()
    {
        buffer.AppendLine();
        started = false;
    }

    public ParameterList Parameters() => new(this);
    public ArgumentList Arguments() => new(this);

    public void Close()
    {
        indent--;
        buffer
            .Append('\t', indent)
            .AppendLine("}");
        started = false;
    }

    public void Clear()
    {
        buffer.Clear();
    }

    public override string ToString() => buffer.ToString();

    public readonly struct Region : IDisposable
    {
        private readonly Output output;

        public Region(Output output)
        {
            this.output = output;
        }

        public void Dispose()
        {
            output.CloseRegion();
        }
    }

    public readonly struct Block : IDisposable
    {
        private readonly Output output;

        public Block(Output output)
        {
            this.output = output;
        }

        public void Dispose()
        {
            output.Close();
        }
    }

    public readonly struct Line : IDisposable
    {
        private readonly Output output;

        public Line(Output output)
        {
            this.output = output;
        }

        public void Dispose()
        {
            output.EndLine();
        }
    }

    public struct ParameterList
    {
        private bool empty = true;
        private readonly Output output;

        public ParameterList(Output output)
        {
            this.output = output;
        }

        public void Append(IParameterSymbol parameter)
        {
            Append(parameter.Type, parameter.Name);
        }

        public void Append(ITypeSymbol type, string name)
        {
            if (!empty)
            {
                output.Write(", ");
            }

            ContentFactory.From(type).Write(output);
            output.Write(" ");
            output.Write(name);
            empty = false;
        }
    }

    public struct ArgumentList
    {
        private bool empty = true;
        private readonly Output output;

        public ArgumentList(Output output)
        {
            this.output = output;
        }

        public void Append<T>(T content) where T : Content
        {
            if (!empty)
            {
                output.Write(", ");
            }

            content.Write(output);
            empty = false;
        }
    }
}