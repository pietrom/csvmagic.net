using System.Text;
using CsvMagic;
using CsvMagic.Reading;

namespace CsvMagicTests.Reading;

[TestFixture]
public class CsvReadingEngineStringsTest {
    private CsvReadingEngine<CsvTextData> engine;

    [SetUp]
    public void InitEngine() {
        engine = new CsvReadingEngineFactory().Create<CsvTextData>();
    }

    private async Task<CsvTextData> ReadSingleLineAsCsv(string input) {
        return await engine.Read(CsvOptions.Builder().WithoutHeaders().Build(), new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(input)))).SingleAsync();
    }

    // Simple text
    [TestCase(",", "", "")]
    [TestCase("AAA,BBB", "AAA", "BBB")]
    [TestCase(",BBB", "", "BBB")]
    [TestCase("AAA,", "AAA", "")]
    [TestCase("\"\"\"AAA\",\"\"\"\"", "\"AAA", "\"")]
    [TestCase("\"AAA,\",", "AAA,", "")]
    [TestCase("\"AAA,\",", "AAA,", "")]
    // Quoted text
    [TestCase("\"AAA\",\"BBB\"", "AAA", "BBB")]
    [TestCase("\"A\"\"A\"\"A\",\"BBB\"", "A\"A\"A", "BBB")]
    [TestCase("\"\"\"A\"\"\",\"BBB\"", "\"A\"", "BBB")]
    [TestCase("\"via Garibaldi, 28\",BBB", "via Garibaldi, 28", "BBB")]
    [TestCase("AA\"A,\"BBB\"", "AA\"A", "BBB")]
    public async Task Read(string input, string text1, string text2) {
        var row = await ReadSingleLineAsCsv(input);
        Assert.That(row, Is.EqualTo(new CsvTextData { Text1 = text1, Text2 = text2 }));
    }
}

public record CsvTextData {
    public string Text1 { get; set; }
    public string Text2 { get; set; }
}
