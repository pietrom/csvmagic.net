using System.Text;
using CsvMagic;
using CsvMagic.Writing;

namespace CsvMagicTests.Writing;

public class CsvWritingEngineExtensionTest {
    private static readonly IEnumerable<Person> Input = new[] {
        new Person { Id = 19, FirstName = "Pietro", LastName = "Martinelli" },
        new Person { Id = 11, FirstName = "Cristina", LastName = "Russo" }
    };

    private const string Output = @"Id,FirstName,LastName
19,Pietro,Martinelli
11,Cristina,Russo
";

    record Person {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    private readonly CsvWritingEngine<Person> engine = new CsvWritingEngineFactory().Create<Person>();

    [Test]
    public async Task WriteToStreamWriter() {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await engine.Write(CsvOptions.Default(), Input, writer);
        stream.Seek(0, SeekOrigin.Begin);
        var result = Encoding.UTF8.GetString(stream.ToArray());
        Assert.That(result, Is.EqualTo(Output));
    }

    [Test]
    public async Task WriteToString() {
        var result = await engine.WriteToString(CsvOptions.Default(), Input);
        Assert.That(result, Is.EqualTo(Output));
    }

    [Test]
    public async Task WriteToByteArray() {
        var result = await engine.WriteToByteArray(CsvOptions.Default(), Input);
        Assert.That(result, Is.EqualTo(Encoding.UTF8.GetBytes(Output)));
    }

    [Test]
    public async Task WriteToFile() {
        var filePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        await engine.WriteToFile(CsvOptions.Default(), Input, filePath);
        var result = await File.ReadAllTextAsync(filePath);
        Assert.That(result, Is.EqualTo(Output));
    }
}
