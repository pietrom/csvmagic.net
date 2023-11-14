using System.Text;
using CsvMagic;
using CsvMagic.Writing;

namespace CsvMagicTests.Writing;

[TestFixture]
public class CsvWritingEngineStringsTest {
    private static readonly CsvOptions Options = CsvOptions.Builder().WithoutHeaders().Build();

    [TestCase("AAA", "BBB", "AAA,BBB")]
    [TestCase("\"AAA", "BBB", "\"\"\"AAA\",BBB")]
    [TestCase("A,A", "BBB", "\"A,A\",BBB")]
    public async Task SerializeCsv(string input0, string input1, string output) {
        var engine = new CsvWritingEngineFactory().Create<CsvTextData>();

        var stream = new MemoryStream();
        await engine.Write(new[] { new CsvTextData(input0, input1) }, new StreamWriter(stream), Options);
        stream.Seek(0, SeekOrigin.Begin);
        var result = Encoding.UTF8.GetString(stream.ToArray());
        Assert.That(result.Trim(), Is.EqualTo(output));
    }
}

public class CsvTextData {
    public CsvTextData(string text0, string text1) {
        Text0 = text0;
        Text1 = text1;
    }

    public string Text0 { get; }
    public string Text1 { get; }
}
