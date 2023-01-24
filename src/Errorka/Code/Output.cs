using System.Text;
using Errorka.Contents;
using Microsoft.CodeAnalysis;

namespace Errorka.Code;

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

    private void CloseBlock()
    {
        indent--;
        buffer
            .Append('\t', indent)
            .AppendLine("}");
        started = false;
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

    public Block OpenNamespace<T>(T content) where T : Content
    {
        buffer
            .Append('\t', indent)
            .Append("namespace ");
        content.Write(this);
        buffer.AppendLine();
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

    public void WriteLine<T>(T content) where T : Content
    {
        content.Write(this);
        EndLine();
    }

    private void EndLine()
    {
        buffer.AppendLine();
        started = false;
    }

    // todo kill in favour of CommaSeparatedList
    public ParameterList Parameters() => new(this);
    public CommaSeparatedList CommaSeparated() => new(this);

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
            output.CloseBlock();
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

        private void Append(ITypeSymbol type, string name)
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

    public struct CommaSeparatedList
    {
        private bool empty = true;
        private readonly Output output;

        public CommaSeparatedList(Output output)
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