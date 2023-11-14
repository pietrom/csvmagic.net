using System.Text;
using CsvMagic;
using CsvMagic.Reading;

namespace CsvMagicTests.Reading;

[TestFixture]
public class CsvReadingEngineExtensionTest {
    private const string Input = @"Id,FirstName,LastName
19,Pietro,Martinelli
11,Cristina,Russo";

    private static readonly IEnumerable<Person> Output = new[] {
        new Person { Id = 19, FirstName = "Pietro", LastName = "Martinelli" },
        new Person { Id = 11, FirstName = "Cristina", LastName = "Russo" }
    };

    record Person {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    private readonly CsvReadingEngine<Person> engine = new CsvReadingEngineFactory().Create<Person>();

    [Test]
    public async Task ReadFromStreamReader() {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(Input));
        var reader = new StreamReader(stream);
        var result = await engine.Read(CsvOptions.Default(), reader).ToListAsync();
        Assert.That(result, Is.EquivalentTo(Output));
    }

    [Test]
    public async Task ReadFromString() {
        var result = await engine.ReadFromString(CsvOptions.Default(), Input).ToListAsync();
        Assert.That(result, Is.EquivalentTo(Output));
    }

    [Test]
    public async Task ReadFromFile() {
        var filePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        await File.WriteAllTextAsync(filePath, Input);
        var result = await engine.ReadFromFile(CsvOptions.Default(), filePath).ToListAsync();
        Assert.That(result, Is.EquivalentTo(Output));
    }
}
