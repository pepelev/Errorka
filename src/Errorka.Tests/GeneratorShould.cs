using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Errorka.Tests;

[TestFixture]
public class GeneratorShould
{
    [Test]
    [TestCase("EmptyCompilation", new string[0], TestName = "No files in compilation")]
    [TestCase("Playground.Service", new[] {"Service.cs"}, TestName = "Multiple areas with subsets")]
    [TestCase("Playground.NoVariants", new[] {"NoVariants.cs"}, TestName = "Result with no variants")]
    public Task Generate(string directory, string[] sources)
    {
        return Verify(directory, sources);
    }

    private static Task Verify(string directory, IEnumerable<string> sources)
    {
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        var syntaxTrees = sources.Select(
            source =>
            {
                var content = File.ReadAllText(source, Encoding.UTF8);
                return CSharpSyntaxTree.ParseText(content);
            }
        );
        var compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: syntaxTrees,
            references: references
        );

        var generator = new Generator();
        var driver = CSharpGeneratorDriver.Create(generator);
        var generatorRunDriver = driver.RunGenerators(compilation);
        return Verifier
            .Verify(generatorRunDriver)
            .UseFileName("Result")
            .UseDirectory(Path.Combine("Snapshots", directory));
    }
}