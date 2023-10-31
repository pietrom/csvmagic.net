using System.Text;
using CsvMagic;
using CsvMagic.Reading;

namespace CsvMagicTests.Reading;

[TestFixture]
public class CsvReadingEngineStringsTest
{
    private CsvReadingEngine<CsvTextData> engine;

    [SetUp]
    public void InitEngine()
    {
        engine = new CsvReadingEngineFactory().Create<CsvTextData>(() => new CsvTextData());
    }

    private async Task<CsvTextData> ReadSingleLIneAsCsv(string input)
    {
        return await engine.Read(new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(input))), false).SingleAsync();
    }

    // Simple text
    [TestCase(",", "", "")]
    [TestCase("AAA,BBB", "AAA", "BBB")]
    [TestCase(",BBB", "", "BBB")]
    [TestCase("AAA,", "AAA", "")]
    // Quoted text
    [TestCase("\"AAA\",\"BBB\"", "AAA", "BBB")]
    public async Task Read(string input, string text1, string text2)
    {
        var row = await ReadSingleLIneAsCsv(input);
        Assert.That(row, Is.EqualTo(new CsvTextData { Text1 = text1, Text2 = text2 }));
    }
}

[CsvRow]
public record CsvTextData
{
    public string Text1 { get; set; }
    public string Text2 { get; set; }
}