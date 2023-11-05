using CsvMagic;
using CsvMagic.Reading.Parsers;

namespace CsvMagicTests.Reading.Parsers;

public class DefaultDateTimeOffsetParserTest
{
    private readonly DefaultDateTimeOffsetParser parser = new DefaultDateTimeOffsetParser();

    [Test]
    public void ParseEmtpyStringToNull()
    {
        Assert.That(parser.ParseNext(new CsvRow().Options, "").Item1, Is.Null);
    }


    [Test]
    public void ParseIso8601StringProperly()
    {
        Assert.That(parser.ParseNext(new CsvRow().Options, "2023-11-05T13:13:45.1240000+02:00").Item1, Is.EqualTo(new DateTimeOffset(2023, 11, 5, 13, 13, 45, 124, TimeSpan.FromHours(2))));
    }


    [Test]
    public void ParseIso8601StringProperlyWithQuotingNeeded()
    {
        Assert.That(parser.ParseNext(new CsvOptions('-', '"', '.', false), "\"2023-11-05T13:13:45.1240000+02:00\"").Item1, Is.EqualTo(new DateTimeOffset(2023, 11, 5, 13, 13, 45, 124, TimeSpan.FromHours(2))));
    }
}