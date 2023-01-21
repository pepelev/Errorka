using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Errorka;

internal class Output
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

    public void Line(string content)
    {
        buffer
            .Append('\t', indent)
            .AppendLine(content);
    }

    public void Constructor(string typeName, INamedTypeSymbol rootType)
    {
        Line("[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]");
        buffer
            .Append('\t', indent)
            .Append("internal @")
            .Append(typeName)
            .Append("(global::")
            .Append(rootType)
            .AppendLine(".Code code, global::System.Object value)");
        using (OpenBlock())
        {
            Line("this.Code = code;");
            Line("this.Value = value;");
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

    public void Write(ITypeSymbol symbol)
    {
        PrintType(symbol);

        void PrintNamespace(INamespaceSymbol @namespace)
        {
            var outer = @namespace.ContainingNamespace;
            if (outer == null || outer.IsGlobalNamespace)
            {
                Write("global::");
                Write(@namespace.Name);
            }
            else
            {
                PrintNamespace(outer);
                Write(".");
                Write(@namespace.Name);
            }
        }

        void PrintType(ITypeSymbol type)
        {
            var outer = type.ContainingType;
            if (outer == null)
            {
                PrintNamespace(type.ContainingNamespace);
                Write(".");
                Write(type.Name);
                return;
            }

            PrintType(outer);
            Write(".");
            Write(type.Name);
        }
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

    public void End()
    {
        buffer.AppendLine();
        started = false;
    }

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
}