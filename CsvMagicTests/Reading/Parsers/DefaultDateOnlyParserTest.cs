using CsvMagic;
using CsvMagic.Reading.Parsers;
using static CsvMagicTests.Reading.CsvReadingContextHelper;

namespace CsvMagicTests.Reading.Parsers;

public class DefaultDateOnlyParserTest {
    private readonly DefaultDateOnlyParser parser = new DefaultDateOnlyParser();

    [Test]
    public void ParseEmtpyStringToNull() {
        Assert.That(parser.ParseNext(ContextFrom(CsvOptions.Default()), "").Item1, Is.Null);
    }


    [Test]
    public void ParseIso8601StringProperly() {
        Assert.That(parser.ParseNext(ContextFrom(CsvOptions.Default()), "2023-11-05").Item1, Is.EqualTo(new DateOnly(2023, 11, 5)));
    }


    [Test]
    public void ParseIso8601StringProperlyWithQuotingNeeded() {
        var options = CsvOptions.Builder()
            .WithDelimiter('-')
            .WithoutHeaders()
            .Build();
        Assert.That(parser.ParseNext(ContextFrom(options), "\"2023-11-05\"").Item1, Is.EqualTo(new DateOnly(2023, 11, 5)));

        Assert.That(parser.ParseNext(ContextFrom(options), "\"2023-11-05\"").Item1, Is.EqualTo(new DateOnly(2023, 11, 5)));
    }
}
