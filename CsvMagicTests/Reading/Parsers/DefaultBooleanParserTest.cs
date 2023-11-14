using CsvMagic;
using CsvMagic.Reading.Parsers;
using static CsvMagicTests.Reading.CsvReadingContextHelper;

namespace CsvMagicTests.Reading.Parsers;

[TestFixture]
public class DefaultBooleanParserTest {
    [TestCase("1", true)]
    [TestCase("0", false)]
    [TestCase("", null)]
    public void ParseUsingDefaults(string text, bool? value) {
        var parsed = new DefaultBooleanParser().ParseNext(ContextFrom(CsvOptions.Default()), text);
        Assert.That(parsed.Item1, Is.EqualTo(value));
    }

    [TestCase("T", true)]
    [TestCase("F", false)]
    [TestCase("", null)]
    public void ParseUsingCustomText(string text, bool? value) {
        var parsed = new DefaultBooleanParser("T", "F").ParseNext(ContextFrom(CsvOptions.Default()), text);
        Assert.That(parsed.Item1, Is.EqualTo(value));
    }
}
