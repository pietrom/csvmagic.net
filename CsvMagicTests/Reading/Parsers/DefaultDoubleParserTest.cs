using CsvMagic;
using CsvMagic.Reading.Parsers;

namespace CsvMagicTests.Reading.Parsers;

[TestFixture]
public class DefaultDoubleParserTest {
    private readonly DefaultDoubleParser parser = new DefaultDoubleParser();

    [TestCase("1234", 1234)]
    [TestCase("1234.56", 1234.56)]
    public void ParseUsingDefaults(string text, double value) {
        var parsed = parser.ParseNext(CsvReadingContextHelper.ContextFrom(CsvOptions.Default()), text);
        Assert.That(parsed.Item1, Is.EqualTo(value));
    }

    [TestCase("1234", 1234)]
    [TestCase("1234,56", 1234.56)]
    public void ParseCustom(string text, double value) {
        var parsed = parser.ParseNext(CsvReadingContextHelper.ContextFrom(new CsvOptions(';', '"', ',', false)), text);
        Assert.That(parsed.Item1, Is.EqualTo(value));
    }

    [TestCase("\"1234,56\"", 1234.56)]
    public void ParseCustomWithQuotingNeeded(string text, double value) {
        var parsed = parser.ParseNext(CsvReadingContextHelper.ContextFrom(new CsvOptions(',', '"', ',', false)), text);
        Assert.That(parsed.Item1, Is.EqualTo(value));
    }
}
