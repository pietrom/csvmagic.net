using CsvMagic;
using CsvMagic.Reading.Parsers;
using static CsvMagicTests.Reading.CsvReadingContextHelper;

namespace CsvMagicTests.Reading.Parsers;

[TestFixture]
public class DefaultDecimalParserTest
{
    private readonly DefaultDecimalParser parser = new DefaultDecimalParser();

    [TestCase("1234", 1234)]
    [TestCase("1234.56", 1234.56)]
    public void ParseUsingDefaults(string text, decimal value)
    {
        var parsed = parser.ParseNext(ContextFrom(CsvOptions.Default()), text);
        Assert.That(parsed.Item1, Is.EqualTo(value));
    }

    [TestCase("1234", 1234)]
    [TestCase("1234,56", 1234.56)]
    public void ParseCustom(string text, decimal value)
    {
        var parsed = parser.ParseNext(ContextFrom(new CsvOptions(';', '"', ',', false)), text);
        Assert.That(parsed.Item1, Is.EqualTo(value));
    }

    [TestCase("\"1234,56\"", 1234.56)]
    public void ParseCustomWithQuotingNeeded(string text, decimal value)
    {
        var parsed = parser.ParseNext(ContextFrom(new CsvOptions(',', '"', ',', false)), text);
        Assert.That(parsed.Item1, Is.EqualTo(value));
    }
}